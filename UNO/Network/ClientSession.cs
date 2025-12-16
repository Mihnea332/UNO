using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Network
{
    public class ClientSession
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private bool isRunning;

        public string SessionId { get; private set; }
        public bool IsActive => isRunning && client != null && client.Connected;

        public ClientSession(TcpClient tcpClient)
        {
            client = tcpClient;
            SessionId = Guid.NewGuid().ToString();

            NetworkStream stream = client.GetStream();
            reader = new StreamReader(stream, Encoding.UTF8);
            writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            isRunning = true;
        }

        public void StartListening(Action<string> onLineReceived)
        {
            Task.Run(async () =>
            {
                try
                {
                    while (isRunning && client.Connected)
                    {
                        string line = await reader.ReadLineAsync();
                        if (line == null)
                        {
                            // Client disconnected
                            break;
                        }

                        onLineReceived?.Invoke(line);
                    }
                }
                catch (Exception)
                {
                    // Connection error or closed
                }
                finally
                {
                    Close();
                }
            });
        }

        public async Task SendAsync(string line)
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("Session is not active");
            }

            try
            {
                await writer.WriteLineAsync(line);
            }
            catch (Exception)
            {
                Close();
                throw;
            }
        }

        public void Close()
        {
            if (!isRunning) return;

            isRunning = false;

            try
            {
                writer?.Close();
                reader?.Close();
                client?.Close();
            }
            catch
            {
                // Ignore close errors
            }

            writer = null;
            reader = null;
            client = null;
        }
    }
}
