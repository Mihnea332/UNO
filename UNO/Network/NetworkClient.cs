using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace UNO.Network
{
    public class NetworkClient : IDisposable
    {
        // Events
        public Action<ServerStatePayload> OnStateReceived;
        public Action<ErrorPayload> OnErrorReceived;
        public Action OnConnected;
        public Action OnDisconnected;
        public Action<string> OnLog;

        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;
        private CancellationTokenSource _cts;
        private bool _isConnected;
        private bool _disposed;

        // JSON serializer options with camelCase
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public NetworkClient()
        {
            _isConnected = false;
            _disposed = false;
        }

        public async Task ConnectAsync(string ip, int port)
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(ip, port);
                
                var stream = _client.GetStream();
                _reader = new StreamReader(stream, Encoding.UTF8);
                _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                
                _isConnected = true;
                _cts = new CancellationTokenSource();
                
                OnLog?.Invoke($"Connected to {ip}:{port}");
                OnConnected?.Invoke();
                
                // Start read loop in background
                _ = Task.Run(() => ReadLoopAsync(_cts.Token));
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Connection error: {ex.Message}");
                OnErrorReceived?.Invoke(new ErrorPayload { Reason = $"Connection failed: {ex.Message}" });
                throw;
            }
        }

        public Task DisconnectAsync()
        {
            if (!_isConnected) return Task.CompletedTask;

            try
            {
                _isConnected = false;
                _cts?.Cancel();
                
                _writer?.Close();
                _reader?.Close();
                _client?.Close();
                
                OnLog?.Invoke("Disconnected");
                OnDisconnected?.Invoke();
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Disconnect error: {ex.Message}");
            }
            
            return Task.CompletedTask;
        }

        public async Task SendMessageAsync<T>(string type, T payload)
        {
            if (!_isConnected || _writer == null)
            {
                OnLog?.Invoke("Cannot send message: not connected");
                return;
            }

            try
            {
                var message = new Message<T> { Type = type, Payload = payload };
                var json = JsonSerializer.Serialize(message, JsonOptions);
                await _writer.WriteLineAsync(json);
                OnLog?.Invoke($"Sent: {type}");
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Send error: {ex.Message}");
                await DisconnectAsync();
            }
        }

        public async Task SendJoinAsync(string name, string playerId = null)
        {
            await SendMessageAsync("join", new JoinPayload { Name = name, PlayerId = playerId });
        }

        public async Task SendPlayAsync(int handIndex)
        {
            await SendMessageAsync("play", new PlayCardPayload { HandIndex = handIndex });
        }

        public async Task SendDrawAsync()
        {
            await SendMessageAsync("draw", new DrawPayload());
        }

        private async Task ReadLoopAsync(CancellationToken ct)
        {
            try
            {
                while (_isConnected && !ct.IsCancellationRequested)
                {
                    var line = await _reader.ReadLineAsync();
                    
                    if (line == null)
                    {
                        OnLog?.Invoke("Connection closed by server");
                        break;
                    }

                    try
                    {
                        // Parse as MessageRaw first
                        var rawMsg = JsonSerializer.Deserialize<MessageRaw>(line, JsonOptions);
                        
                        if (rawMsg == null || string.IsNullOrEmpty(rawMsg.Type))
                        {
                            OnLog?.Invoke("Received invalid message");
                            continue;
                        }

                        OnLog?.Invoke($"Received: {rawMsg.Type}");

                        // Dispatch based on type
                        switch (rawMsg.Type.ToLower())
                        {
                            case "state":
                                var state = JsonSerializer.Deserialize<ServerStatePayload>(
                                    rawMsg.Payload.GetRawText(), JsonOptions);
                                OnStateReceived?.Invoke(state);
                                break;

                            case "error":
                                var error = JsonSerializer.Deserialize<ErrorPayload>(
                                    rawMsg.Payload.GetRawText(), JsonOptions);
                                OnErrorReceived?.Invoke(error);
                                break;

                            case "joinedack":
                                var joinedAck = JsonSerializer.Deserialize<JoinedAckPayload>(
                                    rawMsg.Payload.GetRawText(), JsonOptions);
                                OnLog?.Invoke($"Joined with ID: {joinedAck?.YourPlayerId}");
                                break;

                            default:
                                OnLog?.Invoke($"Unknown message type: {rawMsg.Type}");
                                break;
                        }
                    }
                    catch (JsonException ex)
                    {
                        OnLog?.Invoke($"JSON parse error: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                if (_isConnected)
                {
                    OnLog?.Invoke($"Read loop error: {ex.Message}");
                }
            }
            finally
            {
                if (_isConnected)
                {
                    await DisconnectAsync();
                }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            
            _disposed = true;
            
            try
            {
                _isConnected = false;
                _cts?.Cancel();
                
                _writer?.Close();
                _reader?.Close();
                _client?.Close();
            }
            catch
            {
                // Ignore cleanup errors during disposal
            }
        }
    }
}
