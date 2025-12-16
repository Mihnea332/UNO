using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UNO.Network
{
    public class NetworkClient
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private bool isRunning;

        // Events
        public event Action<ServerStatePayload> OnStateReceived;
        public event Action<ErrorPayload> OnErrorReceived;
        public event Action<string> OnLog;
        public event Action OnConnected;
        public event Action OnDisconnected;

        public bool IsConnected => client != null && client.Connected;

        public async Task ConnectAsync(string ip, int port)
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(ip, port);

                NetworkStream stream = client.GetStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                isRunning = true;
                OnConnected?.Invoke();
                OnLog?.Invoke($"Connected to {ip}:{port}");

                // Start read loop
                Task.Run(async () => await ReadLoopAsync());
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Connection error: {ex.Message}");
                throw;
            }
        }

        public async Task DisconnectAsync()
        {
            isRunning = false;
            
            if (writer != null)
            {
                writer.Close();
                writer = null;
            }
            
            if (reader != null)
            {
                reader.Close();
                reader = null;
            }
            
            if (client != null)
            {
                client.Close();
                client = null;
            }

            OnDisconnected?.Invoke();
            OnLog?.Invoke("Disconnected");
            
            await Task.CompletedTask;
        }

        private async Task ReadLoopAsync()
        {
            try
            {
                while (isRunning && client != null && client.Connected)
                {
                    string line = await reader.ReadLineAsync();
                    if (line == null)
                    {
                        OnLog?.Invoke("Server closed connection");
                        break;
                    }

                    OnLog?.Invoke($"Received: {line}");
                    ProcessMessage(line);
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Read error: {ex.Message}");
            }
            finally
            {
                await DisconnectAsync();
            }
        }

        private void ProcessMessage(string line)
        {
            try
            {
                var msgRaw = JsonConvert.DeserializeObject<MessageRaw>(line);
                if (msgRaw == null) return;

                switch (msgRaw.Type)
                {
                    case "State":
                        var state = JsonConvert.DeserializeObject<ServerStatePayload>(msgRaw.Payload);
                        OnStateReceived?.Invoke(state);
                        break;

                    case "Error":
                        var error = JsonConvert.DeserializeObject<ErrorPayload>(msgRaw.Payload);
                        OnErrorReceived?.Invoke(error);
                        break;

                    case "JoinedAck":
                        var ack = JsonConvert.DeserializeObject<JoinedAckPayload>(msgRaw.Payload);
                        OnLog?.Invoke($"Join acknowledged: {ack.Message}, PlayerId: {ack.PlayerId}");
                        break;

                    default:
                        OnLog?.Invoke($"Unknown message type: {msgRaw.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Error processing message: {ex.Message}");
            }
        }

        public async Task SendMessageAsync(string type, object payload)
        {
            if (!IsConnected)
            {
                OnLog?.Invoke("Cannot send: not connected");
                return;
            }

            try
            {
                var msg = new Message { Type = type, Payload = payload };
                string json = JsonConvert.SerializeObject(msg);
                await writer.WriteLineAsync(json);
                OnLog?.Invoke($"Sent: {json}");
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Send error: {ex.Message}");
                throw;
            }
        }

        public async Task SendJoinAsync(string playerName)
        {
            var payload = new JoinPayload { PlayerName = playerName };
            await SendMessageAsync("Join", payload);
        }

        public async Task SendPlayAsync(string playerId, CardDto card)
        {
            var payload = new PlayCardPayload { PlayerId = playerId, Card = card };
            await SendMessageAsync("PlayCard", payload);
        }

        public async Task SendDrawAsync(string playerId)
        {
            var payload = new DrawPayload { PlayerId = playerId };
            await SendMessageAsync("Draw", payload);
        }
    }
}
