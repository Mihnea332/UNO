using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UNO.Logic;
using UNO.Model;

namespace UNO.Network
{
    public class ServerManager
    {
        private TcpListener listener;
        private bool isRunning;
        private Dictionary<string, ClientSession> sessions = new Dictionary<string, ClientSession>();
        private Dictionary<string, Player> players = new Dictionary<string, Player>();
        private List<Card> deck = new List<Card>();
        private Card topCard;
        private string currentPlayerId;

        public event Action<string> OnLog;

        public void Start(int port)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                isRunning = true;

                OnLog?.Invoke($"Server started on port {port}");

                // Initialize game state (placeholder)
                InitializeGame();

                // Accept connections
                Task.Run(async () => await AcceptClientsAsync());
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Server start error: {ex.Message}");
                throw;
            }
        }

        public void Stop()
        {
            isRunning = false;

            // Close all sessions
            foreach (var session in sessions.Values.ToList())
            {
                session.Close();
            }
            sessions.Clear();
            players.Clear();

            listener?.Stop();
            OnLog?.Invoke("Server stopped");
        }

        private void InitializeGame()
        {
            // TODO: Use UNO.Logic.Deck for proper card initialization
            // For MVP, create a minimal deck
            deck.Clear();
            
            // Create a simple deck with some cards
            Colors[] colors = { Colors.Red, Colors.Blue, Colors.Green, Colors.Yellow };
            Val[] values = { Val.Zero, Val.One, Val.Two, Val.Three, Val.Four, Val.Five, Val.Six, Val.Seven, Val.Eight, Val.Nine };
            
            foreach (var color in colors)
            {
                foreach (var value in values)
                {
                    deck.Add(new NormalCard(color, value));
                }
            }

            // Add some special cards
            foreach (var color in colors)
            {
                deck.Add(new SpecialCard(color, Val.Skip));
                deck.Add(new SpecialCard(color, Val.Reverse));
                deck.Add(new SpecialCard(color, Val.DrawTwo));
            }

            // Add wild cards
            for (int i = 0; i < 4; i++)
            {
                deck.Add(new WildCard(Colors.None, Val.Wild));
                deck.Add(new WildCard(Colors.None, Val.WildDrawFour));
            }

            // TODO: Shuffle deck properly using UNO.Logic.Deck.Shuffle()
            ShuffleDeck();

            // Set initial top card
            if (deck.Count > 0)
            {
                topCard = deck[0];
                deck.RemoveAt(0);
            }
        }

        private void ShuffleDeck()
        {
            // Simple shuffle implementation
            Random rng = new Random();
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card temp = deck[k];
                deck[k] = deck[n];
                deck[n] = temp;
            }
        }

        private async Task AcceptClientsAsync()
        {
            while (isRunning)
            {
                try
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    ClientSession session = new ClientSession(client);
                    
                    OnLog?.Invoke($"Client connected: {session.SessionId}");

                    session.StartListening(line => OnClientMessage(session, line));
                }
                catch (Exception ex)
                {
                    if (isRunning)
                    {
                        OnLog?.Invoke($"Accept error: {ex.Message}");
                    }
                }
            }
        }

        private void OnClientMessage(ClientSession session, string line)
        {
            try
            {
                OnLog?.Invoke($"Received from {session.SessionId}: {line}");

                var msgRaw = JsonConvert.DeserializeObject<MessageRaw>(line);
                if (msgRaw == null) return;

                switch (msgRaw.Type)
                {
                    case "Join":
                        HandleJoin(session, msgRaw.Payload);
                        break;

                    case "PlayCard":
                        HandlePlayCard(msgRaw.Payload);
                        break;

                    case "Draw":
                        HandleDraw(msgRaw.Payload);
                        break;

                    default:
                        SendError(session, $"Unknown message type: {msgRaw.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Message handling error: {ex.Message}");
                SendError(session, $"Error: {ex.Message}");
            }
        }

        private void HandleJoin(ClientSession session, string payloadJson)
        {
            var payload = JsonConvert.DeserializeObject<JoinPayload>(payloadJson);
            if (payload == null || string.IsNullOrEmpty(payload.PlayerName))
            {
                SendError(session, "Invalid join payload");
                return;
            }

            // Create player (Player constructor auto-generates ID)
            Player player = new Player(payload.PlayerName);
            string playerId = player.ID;

            // Deal initial hand (placeholder - typically 7 cards)
            // TODO: Use proper game logic for dealing cards
            int initialHandSize = 7;
            for (int i = 0; i < initialHandSize && deck.Count > 0; i++)
            {
                Card card = deck[0];
                deck.RemoveAt(0);
                player.AddCard(card);
            }

            // Store player and session
            players[playerId] = player;
            sessions[playerId] = session;

            if (string.IsNullOrEmpty(currentPlayerId) && players.Count > 0)
            {
                currentPlayerId = playerId;
            }

            OnLog?.Invoke($"Player joined: {payload.PlayerName} (ID: {playerId})");

            // Send join acknowledgment
            var ack = new JoinedAckPayload
            {
                PlayerId = playerId,
                Message = $"Welcome, {payload.PlayerName}!"
            };
            SendToSession(session, "JoinedAck", ack);

            // Broadcast updated state to all players
            BroadcastState();
        }

        private void HandlePlayCard(string payloadJson)
        {
            var payload = JsonConvert.DeserializeObject<PlayCardPayload>(payloadJson);
            if (payload == null || string.IsNullOrEmpty(payload.PlayerId))
            {
                OnLog?.Invoke("Invalid play card payload");
                return;
            }

            if (!players.ContainsKey(payload.PlayerId))
            {
                OnLog?.Invoke($"Player not found: {payload.PlayerId}");
                return;
            }

            Player player = players[payload.PlayerId];
            Card cardToPlay = CardMapper.MapDtoToCard(payload.Card);

            if (cardToPlay == null)
            {
                SendError(sessions[payload.PlayerId], "Invalid card");
                return;
            }

            // TODO: Validate card can be played using player.IsCardValid(topCard, cardToPlay)
            // For MVP, just allow any card to be played
            
            // Find and remove the card from player's hand
            List<Card> hand = player.getHand();
            Card matchingCard = hand.FirstOrDefault(c => 
                c.color == cardToPlay.color && c.value == cardToPlay.value);

            if (matchingCard != null)
            {
                player.RemoveCard(matchingCard);
                topCard = matchingCard;

                OnLog?.Invoke($"Player {player.Name} played {cardToPlay}");

                // Move to next player
                // TODO: Implement proper turn rotation
                var playerIds = players.Keys.ToList();
                int currentIndex = playerIds.IndexOf(payload.PlayerId);
                currentPlayerId = playerIds[(currentIndex + 1) % playerIds.Count];

                BroadcastState();
            }
            else
            {
                SendError(sessions[payload.PlayerId], "Card not in hand");
            }
        }

        private void HandleDraw(string payloadJson)
        {
            var payload = JsonConvert.DeserializeObject<DrawPayload>(payloadJson);
            if (payload == null || string.IsNullOrEmpty(payload.PlayerId))
            {
                OnLog?.Invoke("Invalid draw payload");
                return;
            }

            if (!players.ContainsKey(payload.PlayerId))
            {
                OnLog?.Invoke($"Player not found: {payload.PlayerId}");
                return;
            }

            Player player = players[payload.PlayerId];

            if (deck.Count > 0)
            {
                Card card = deck[0];
                deck.RemoveAt(0);
                player.AddCard(card);

                OnLog?.Invoke($"Player {player.Name} drew a card");

                // Move to next player
                // TODO: Implement proper turn rotation
                var playerIds = players.Keys.ToList();
                int currentIndex = playerIds.IndexOf(payload.PlayerId);
                currentPlayerId = playerIds[(currentIndex + 1) % playerIds.Count];

                BroadcastState();
            }
            else
            {
                SendError(sessions[payload.PlayerId], "Deck is empty");
            }
        }

        public void BroadcastState()
        {
            foreach (var kvp in players)
            {
                string playerId = kvp.Key;
                if (sessions.ContainsKey(playerId) && sessions[playerId].IsActive)
                {
                    var snapshot = BuildSnapshotForPlayer(playerId);
                    SendToSession(sessions[playerId], "State", snapshot);
                }
            }
        }

        public ServerStatePayload BuildSnapshotForPlayer(string playerId)
        {
            var snapshot = new ServerStatePayload
            {
                Players = new List<PlayerPublic>(),
                TopCard = CardMapper.MapCardToDto(topCard),
                CurrentPlayerId = currentPlayerId,
                DeckCount = deck.Count
            };

            foreach (var kvp in players)
            {
                string pid = kvp.Key;
                Player player = kvp.Value;
                List<Card> hand = player.getHand();

                var playerPublic = new PlayerPublic
                {
                    Id = pid,
                    Name = player.Name,
                    CardCount = hand.Count,
                    Hand = null // Only show own hand
                };

                // Include full hand only for the requesting player
                if (pid == playerId)
                {
                    playerPublic.Hand = new List<CardDto>();
                    foreach (var card in hand)
                    {
                        playerPublic.Hand.Add(CardMapper.MapCardToDto(card));
                    }
                }

                snapshot.Players.Add(playerPublic);
            }

            return snapshot;
        }

        private void SendToSession(ClientSession session, string type, object payload)
        {
            try
            {
                var msg = new Message { Type = type, Payload = payload };
                string json = JsonConvert.SerializeObject(msg);
                Task.Run(async () => await session.SendAsync(json));
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Send error: {ex.Message}");
            }
        }

        private void SendError(ClientSession session, string errorMessage)
        {
            var error = new ErrorPayload { Message = errorMessage };
            SendToSession(session, "Error", error);
        }
    }
}
