using System.Collections.Generic;
using UNO.Model;

namespace UNO.Network
{
    // Generic message wrapper for typed payloads
    public class Message<T>
    {
        public string Type { get; set; }
        public T Payload { get; set; }
    }

    // Message wrapper for raw JSON parsing
    public class MessageRaw
    {
        public string Type { get; set; }
        public System.Text.Json.JsonElement Payload { get; set; }
    }

    // Client -> Server: Join game request
    public class JoinPayload
    {
        public string Name { get; set; }
        public string PlayerId { get; set; }
    }

    // Client -> Server: Play a card from hand
    public class PlayCardPayload
    {
        public int HandIndex { get; set; }
    }

    // Client -> Server: Draw a card
    public class DrawPayload
    {
        // Empty payload
    }

    // Server -> Client: Full game state update
    public class ServerStatePayload
    {
        public List<PlayerPublic> Players { get; set; }
        public List<Card> MyHand { get; set; }
        public Card TopDiscard { get; set; }
        public int CurrentTurnIndex { get; set; }
        public int DeckCount { get; set; }
        public string YourPlayerId { get; set; }
        public bool IsStarted { get; set; }
        public bool IsFinished { get; set; }
        public string WinnerPlayerId { get; set; }
    }

    // Public player information (no hand exposed)
    public class PlayerPublic
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int CardCount { get; set; }
    }

    // Server -> Client: Error message
    public class ErrorPayload
    {
        public string Reason { get; set; }
    }

    // Server -> Client: Join acknowledgment
    public class JoinedAckPayload
    {
        public string YourPlayerId { get; set; }
    }
}
