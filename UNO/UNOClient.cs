using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UNO.Logic;
using UNO.Model;

namespace UNO
{
    public partial class UNOClient : Form
    {
      
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader citire;
        private StreamWriter scriere;
        private Thread listenThread;
        private bool running;
        public int myPlayerId;
        private string topColor;
        private string topValue;
        private List<string> handColors;
        private List<string> handValues;
        private int opponentCardCount;
        private int currentPlayerIndex;
        private bool canPlayDrawnCard;
        private string drawnColor;
        private string drawnValue;
        public event Action OnStateUpdated;
        public event Action OnCanPlayDrawn;
        public event Action<int> OnWinner;
        public UNOClient()
        {
            InitializeComponent();
            myPlayerId = 0;
            this.FormClosed += (s, e) => Application.Exit();
        }

        private void UNOClient_Load(object sender, EventArgs e)
        {

        }
        private void Connect(string ip,int port)
        {
            client = new TcpClient();
            client.Connect(ip, port);
            stream = client.GetStream();
            citire = new StreamReader(stream);
            scriere = new StreamWriter(stream);
            scriere.AutoFlush = true;
            running = true;
            listenThread = new Thread(ListenLoop);
            listenThread.Start();
        }
       private void ListenLoop()
        {
            while(running)
            {
                string line = citire.ReadLine();
                if (line == null)
                    break;
                UNOMessage msg = JsonConvert.DeserializeObject<UNOMessage>(line);
                if (msg == null)
                    continue;
                HandleServerMessage(line);
            }
        }
        private void HandleServerMessage(string json)
        {
            UNOMessage header = JsonConvert.DeserializeObject<UNOMessage>(json);
            if (header.Type == "STATE")
            {
                GameStateMessage state = JsonConvert.DeserializeObject<GameStateMessage>(json);
                HandleGameState(state);
                return;
            }

            if (header.Type == "CAN_PLAY_DRAWN")
            {
                HandleCanPlayDrawn(header);
                return;
            }

            if (header.Type == "WINNER")
            {
                HandleWinner(header.PlayerId);
                return;
            }
        }
        private void HandleWinner(int playerId)
        {
            if (OnWinner != null)
                OnWinner(playerId);
            if (playerId == myPlayerId)
                MessageBox.Show("Ai castigat!");
            else
                MessageBox.Show("Ai pierdut!");
            running = false;
            if(listenThread!=null)
            {
                listenThread.Abort();
                listenThread = null;
            }
            if(client==null)
            {
                client.Close();
                client = null;
            }
        }
        private void HandleGameState(GameStateMessage state)
        {
            topColor = state.TopColor;
            topValue = state.TopValue;
            handColors = new List<string>();
            handValues = new List<string>();
            opponentCardCount = state.OpponentCardCount;
            currentPlayerIndex = state.CurrentPlayerIndex;
            if (OnStateUpdated != null)
                OnStateUpdated();
        }
        private void HandleCanPlayDrawn(UNOMessage msg)
        {
            canPlayDrawnCard = true;
            drawnColor = msg.Color;
            drawnValue = msg.Value;
            if (OnCanPlayDrawn != null)
                OnCanPlayDrawn();
        }
    }
}
