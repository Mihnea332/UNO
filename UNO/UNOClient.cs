using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private TcpClient tcp;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        private Thread listenThread;
        private bool running;
        private string topColor;
        private string topValue;
        private List<string> handColors = new List<string>();
        private List<string> handValues = new List<string>();
        private int currentPlayerIndex;
        private int myPlayerId;
        private int opponentCardCount;
        private string drawnColor;
        private string drawnValue;
        private bool canPlayDrawnCard=false;
        public UNOClient()
        {
            InitializeComponent();

        }

        private void UNOClient_Load(object sender, EventArgs e)
        {
            Connect();
        }
        private void ShowTopCard(Control parent)
        {
            parent.Controls.Clear();
            if (string.IsNullOrEmpty(topColor) || string.IsNullOrEmpty(topValue))
                return;
            PictureBox pb = new PictureBox
            {
                Size = new Size(80, 120),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            Colors c = (Colors)Enum.Parse(typeof(Colors), topColor);
            Val v = (Val)Enum.Parse(typeof(Val), topValue);

            Card topCard;
            if (v == Val.Wild)
                topCard = new WildCard(c, v);
            else if (v == Val.DrawTwo || v == Val.Reverse || v == Val.Skip || v == Val.WildDrawFour)
                topCard = new SpecialCard(c, v);
            else
                topCard = new NormalCard(c, v);


            string path = topCard.GetCardName();
            if (File.Exists(path))
                pb.Image = Image.FromFile(path);
            else
                pb.BackColor = Color.Gray;

            parent.Controls.Add(pb);

        }
        private void ShowHand(Control parent)
        {
            parent.Controls.Clear();
            for (int i = 0; i < handColors.Count; i++)
            {
                Colors c = (Colors)Enum.Parse(typeof(Colors), handColors[i]);
                Val v = (Val)Enum.Parse(typeof(Val), handValues[i]);
                Card card;
                if (v == Val.Wild || v == Val.WildDrawFour)
                    card = new WildCard(c, v);
                else if (v == Val.DrawTwo || v == Val.Skip || v == Val.Reverse)
                    card = new SpecialCard(c, v);
                else
                    card = new NormalCard(c, v);

                PictureBox pb = new PictureBox
                {
                    Size = new Size(60, 90),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent,
                    Tag = i
                };
                string path = card.GetCardName();
                if (File.Exists(path))
                    pb.Image = Image.FromFile(path);
                else
                    pb.BackColor = Color.Gray;
                pb.Click += Card_Click;
                parent.Controls.Add(pb);

            }
        }
        private void Card_Click(object sender, EventArgs e)
        {
            if (currentPlayerIndex != myPlayerId)
                return;

            PictureBox pb = sender as PictureBox;
            int index = (int)pb.Tag;
            string color = handColors[index];
            string value = handValues[index];
            if (value == "Wild" || value == "WildDrawFour")
            {
                flowHand.Enabled = false;
                panelChooseColor.Controls.Clear();
                panelChooseColor.Visible = true;
                int height = 40;
                int width = 40;
                int x = 0;
                int y = 0;
                int spacing = 50;
                void AddColorButton(string colorName, Color uiColor)
                {
                    Button btn = new Button()
                    {
                        Size = new Size(width, height),
                        Location = new Point(x, y),
                        BackColor = uiColor,
                        Text = colorName,

                        ForeColor = uiColor
                    };
                    btn.Click += (s, ev) =>
                    {
                        SendPlay(color, value, colorName);
                        panelChooseColor.Visible = false;
                        flowHand.Enabled = true;
                    };
                    panelChooseColor.Controls.Add(btn);
                    x += spacing;
                }
                AddColorButton("Red", Color.Red);
                AddColorButton("Blue", Color.Blue);
                AddColorButton("Yellow", Color.Yellow);
                AddColorButton("Green", Color.Green);

            }
            else
                SendPlay(color, value);
        }
    
        private void SendPlay(string color, string value)
        {
            SendPlay(color, value, null);
        }
        private void SendPlay(string color,  string value, string chosenColor)
        {
            UNOMessage msg = new UNOMessage();
            msg.Type = "PLAY";
            msg.PlayerId = myPlayerId;
            msg.Color = color;
            msg.Value = value;
            msg.ChosenColor = chosenColor;
            string json = JsonConvert.SerializeObject(msg);
            writer.WriteLine(json);
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void ShowOpponent()
        {
            labelOpponent.Text = "Adversar: " + opponentCardCount + " cărți";
        }
        private void ShowTurn()
        {
            if (currentPlayerIndex == myPlayerId)
                labelTurn.Text = "Your turn";
            else labelTurn.Text = "Opponent's turn";
        }
        private void UpdateUI()
        {
            ShowTopCard(panelTopCard);
            ShowHand(flowHand);
            ShowOpponent();
            ShowTurn();
            if(currentPlayerIndex==myPlayerId)
            {
                flowHand.Enabled = true;
                btnDraw.Enabled = true;

            }
            else
            {
                flowHand.Enabled = false;
                btnPlayDrawnCard.Enabled = false;
            }
        }
        private void HandleGameState(GameStateMessage state)
        {
            topColor = state.TopColor;
            topValue = state.TopValue;
            handColors = state.handColors;
            handValues = state.handValues;
            opponentCardCount = state.OpponentCardCount;
            currentPlayerIndex = state.CurrentPlayerIndex;
            UpdateUI();
        }
        private void HandleCanPlayDrawn(UNOMessage msg)
        {
            canPlayDrawnCard = true;
            drawnColor = msg.Color;
            drawnValue = msg.Value;

        }
        private void HandleWinner(int playerId)
        {
            if (playerId == myPlayerId)
                MessageBox.Show("You won!");
            else
                MessageBox.Show("You lost!");
            running = false;
        }
        private void HandleServerMessage(string json)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleServerMessage(json)));
                return;
            }

            UNOMessage msg = JsonConvert.DeserializeObject<UNOMessage>(json);
            if (msg.Type == "STATE")
            {
                GameStateMessage state = JsonConvert.DeserializeObject<GameStateMessage>(json);
                UpdateGameState(state);
                return;

            }
            else if (msg.Type == "CAN_PLAY_DRAWN")
            {
                drawnColor = msg.Color;
                drawnValue = msg.Value;
                canPlayDrawnCard = true;
                btnPlayDrawnCard.Visible = true;
                btnPlayDrawnCard.Enabled = true;
            }
            else if (msg.Type == "WINNER")
            {
                MessageBox.Show("Player " + msg.PlayerId + " wins!");
            }
            else if (msg.Type == "PLAYER_ID")
                myPlayerId = msg.PlayerId;
        }
        private void UpdateGameState(GameStateMessage state)
        {
            topColor = state.TopColor;
            topValue = state.TopValue;
            handColors = state.handColors;
            handValues = state.handValues;
            opponentCardCount = state.OpponentCardCount;
            currentPlayerIndex = state.CurrentPlayerIndex;
        }
        private void ListenLoop()
        {
            while (running)
            {
                string line = reader.ReadLine();
                if (line == null) break;
                UNOMessage msg = JsonConvert.DeserializeObject<UNOMessage>(line);
                if(msg.Type=="STATE")
                {
                    GameStateMessage state = JsonConvert.DeserializeObject<GameStateMessage>(line);
                    HandleGameState(state);
                }
                else if(msg.Type=="CAN_PLAY_DRAWN")
                {
                    HandleCanPlayDrawn(msg);
                }
                else if(msg.Type=="WINNER")
                {
                    HandleWinner(msg.PlayerId);
                }
            }
        }
        private void Connect()
        {
            tcp = new TcpClient();
            tcp.Connect("127.0.0.1", 3000);
            stream = tcp.GetStream();
            reader = new StreamReader(stream);
            writer= new StreamWriter(stream);
            writer.AutoFlush = true;
            running = true;
            listenThread = new Thread(ListenLoop);
            listenThread.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currentPlayerIndex != myPlayerId)
                return;

            SendDraw();

        }
        private void SendDraw()
        {
            UNOMessage msg = new UNOMessage();
            msg.Type = "DRAW";
            msg.PlayerId = myPlayerId;
            string json = JsonConvert.SerializeObject(msg);
            writer.WriteLine(json);
        }

        private void btnPlayDrawnCard_Click(object sender, EventArgs e)
        {
            if (!canPlayDrawnCard)
                return;
            if(drawnValue=="Wild"||drawnValue=="WildDrawFour")
            {

                flowHand.Enabled = false;
                panelChooseColor.Controls.Clear();
                panelChooseColor.Visible = true;
                int height = 40;
                int width = 40;
                int x = 0;
                int y = 0;
                int spacing = 50;
                void AddColorButton(string colorName, Color uiColor)
                {
                    Button btn = new Button()
                    {
                        Size = new Size(width, height),
                        Location = new Point(x, y),
                        BackColor = uiColor,
                        Text = colorName,

                        ForeColor = uiColor
                    };
                    btn.Click += (s, ev) =>
                    {
                        SendPlay(drawnValue, drawnValue, colorName);
                        panelChooseColor.Visible = false;
                        flowHand.Enabled = true;
                    };
                    panelChooseColor.Controls.Add(btn);
                    x += spacing;
                }
                AddColorButton("Red", Color.Red);
                AddColorButton("Blue", Color.Blue);
                AddColorButton("Yellow", Color.Yellow);
                AddColorButton("Green", Color.Green);
            }
            else
            {
                SendPlay(drawnColor, drawnValue, null);
            }
            btnPlayDrawnCard.Visible = false;
            btnPlayDrawnCard.Enabled = false;
            canPlayDrawnCard = false;
        }
    }
}