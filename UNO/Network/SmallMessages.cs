using System;
using System.Collections.Generic;
using UNO.Model;

namespace UNO.Network
{
    // Base message structure for JSON protocol
    public class Message
    {
        public string Type { get; set; }
        public object Payload { get; set; }
    }

    // Raw message for parsing
    public class MessageRaw
    {
        public string Type { get; set; }
        public string Payload { get; set; }
    }

    // Join request from client
    public class JoinPayload
    {
        public string PlayerName { get; set; }
    }

    // Play card request from client
    public class PlayCardPayload
    {
        public string PlayerId { get; set; }
        public CardDto Card { get; set; }
    }

    // Draw card request from client
    public class DrawPayload
    {
        public string PlayerId { get; set; }
    }

    // Server state broadcast to clients
    public class ServerStatePayload
    {
        public List<PlayerPublic> Players { get; set; }
        public CardDto TopCard { get; set; }
        public string CurrentPlayerId { get; set; }
        public int DeckCount { get; set; }
    }

    // Public player info (visible to all clients)
    public class PlayerPublic
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int CardCount { get; set; }
        public List<CardDto> Hand { get; set; } // Only populated for the player's own view
    }

    // Card DTO for network transmission
    public class CardDto
    {
        public string Color { get; set; }
        public string Value { get; set; }
    }

    // Error message from server
    public class ErrorPayload
    {
        public string Message { get; set; }
    }

    // Join acknowledgment from server
    public class JoinedAckPayload
    {
        public string PlayerId { get; set; }
        public string Message { get; set; }
    }

    // Helper methods for Card conversion
    public static class CardMapper
    {
        public static CardDto MapCardToDto(Card card)
        {
            if (card == null) return null;
            return new CardDto
            {
                Color = card.color.ToString(),
                Value = card.value.ToString()
            };
        }

        public static Card MapDtoToCard(CardDto dto)
        {
            if (dto == null) return null;
            
            Colors color;
            Val value;
            
            if (!Enum.TryParse(dto.Color, out color))
                color = Colors.None;
            
            if (!Enum.TryParse(dto.Value, out value))
                value = Val.None;

            // Create appropriate card type based on value
            if (value == Val.Wild || value == Val.WildDrawFour)
            {
                return new WildCard(color, value);
            }
            else if (value == Val.Skip || value == Val.Reverse || value == Val.DrawTwo)
            {
                return new SpecialCard(color, value);
            }
            else
            {
                return new NormalCard(color, value);
            }
        }
    }
}
