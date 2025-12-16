using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace UNO.Network
{
    public class ClientSession : IDisposable
    {
        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;
        private CancellationTokenSource _cts;
        private bool _isActive;
        private bool _disposed;

        // JSON serializer options with camelCase
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public string PlayerId { get; set; }
        public bool IsActive => _isActive;

        public ClientSession(TcpClient client)
        {
            _client = client;
            _isActive = true;
            _disposed = false;
            
            var stream = client.GetStream();
            _reader = new StreamReader(stream, Encoding.UTF8);
            _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            _cts = new CancellationTokenSource();
        }

        public void StartListening(Action<ClientSession, MessageRaw> onMessageReceived)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    while (_isActive && !_cts.Token.IsCancellationRequested)
                    {
                        var line = await _reader.ReadLineAsync();
                        
                        if (line == null)
                        {
                            // Connection closed
                            break;
                        }

                        try
                        {
                            var rawMsg = JsonSerializer.Deserialize<MessageRaw>(line, JsonOptions);
                            if (rawMsg != null && !string.IsNullOrEmpty(rawMsg.Type))
                            {
                                onMessageReceived?.Invoke(this, rawMsg);
                            }
                        }
                        catch (JsonException)
                        {
                            // Ignore parse errors
                        }
                    }
                }
                catch (Exception)
                {
                    // Connection error
                }
                finally
                {
                    Close();
                }
            });
        }

        public async Task SendMessageAsync<T>(string type, T payload)
        {
            if (!_isActive || _writer == null) return;

            try
            {
                var message = new Message<T> { Type = type, Payload = payload };
                var json = JsonSerializer.Serialize(message, JsonOptions);
                await _writer.WriteLineAsync(json);
            }
            catch (Exception)
            {
                Close();
            }
        }

        public void Close()
        {
            if (!_isActive) return;

            _isActive = false;
            _cts?.Cancel();
            
            try
            {
                _writer?.Close();
                _reader?.Close();
                _client?.Close();
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            
            _disposed = true;
            Close();
        }
    }
}
