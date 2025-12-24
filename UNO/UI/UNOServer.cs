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
    public partial class UNOServer : Form
    {
        private Game game; 
        public TcpListener server;
        private Socket clientSocket;
        public Thread listenThread;
        public bool running;
        public NetworkStream clientStream; 
        public StreamReader citire;
        public StreamWriter scriere;
        private bool gameOver=false;
        public UNOServer()
        {
            InitializeComponent();
            game = new Game();
            server = new TcpListener(System.Net.IPAddress.Any, 3000);
            server.Start();
            running = true;
            listenThread = new Thread(ListenLoop);
            listenThread.Start();
            this.FormClosed += UNOServer_FormClosed;



        }
        private void UNOServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            running = false;

            try { clientSocket?.Shutdown(SocketShutdown.Both); } catch { }
            try { clientSocket?.Close(); } catch { }
            try { server?.Stop(); } catch { }
            try { listenThread?.Join(200); } catch { }

            Environment.Exit(0);
        }
        private void ListenLoop()
        {
            if (gameOver) return;
            try
            {
                Socket socket = server.AcceptSocket();
                this.lblStatus.Text = "Connected";
                this.lblStatus.ForeColor = Color.Green;
                clientSocket = socket;
                clientStream = new NetworkStream(socket);
                citire = new StreamReader(clientStream);
                scriere = new StreamWriter(clientStream);
                scriere.AutoFlush = true;
                UNOMessage idMsg = new UNOMessage();
                idMsg.Type = "PLAYER_ID";
                idMsg.PlayerId = 0;
                string json = JsonConvert.SerializeObject(idMsg);
                scriere.WriteLine(json);
                SendGameStateToClient();
                while (running)
                {
                    if (gameOver) break;
                    string line = null;
                   try
                    {
                        line = citire.ReadLine();
                        

                    }
                    catch
                    {
                        break;
                    }
                    if (line==null)
                    {
                        break;
                        this.lblStatus.Text = "Waiting for client...";
                        this.lblStatus.ForeColor = Color.Gray;
                    }
                    UNOMessage msg = null;
                    try
                    {
                        msg = JsonConvert.DeserializeObject<UNOMessage>(line);

                    }
                    catch
                    {
                        continue;
                    }
                    if (msg == null || msg.Type == null)
                        continue;
                    HandleClientMessage(msg);
                }
            }
            catch (SocketException)
            { }
            catch (Exception) { }
            
        }

        private void HandleClientMessage(UNOMessage msg)
        {
            switch (msg.Type)
            {
                case "REQUESTE_STATE":
                    SendGameStateToClient();
                    return;
                case "PLAY": HandlePlay(msg);
                    break;
                case "DRAW": HandleDraw(msg);
                    break;
                default:
                    break;
            }
        }
        public void SendGameStateToClient()
        {
            if (clientSocket == null)
                return;
            GameStateMessage state = new GameStateMessage();
            Card top = game.getTopCard();
            state.TopColor = top.color.ToString();
            state.TopValue = top.value.ToString();
            Player me = game.getPlayers()[0];
            List<Card> hand = me.getHand();
            state.handColors = new List<string>();
            state.handValues = new List<string>();
            for(int i=0;i<hand.Count;i++)
            {
                state.handColors.Add(hand[i].color.ToString());
                state.handValues.Add(hand[i].value.ToString());
            }
            int opponentIndex = 1 - game.getCurrentPlayerIndex();
            Player opponent = game.getPlayers()[opponentIndex];
            state.OpponentCardCount = opponent.getHand().Count;
            state.CurrentPlayerIndex = game.getCurrentPlayerIndex();
            string json = JsonConvert.SerializeObject(state);
            scriere.WriteLine(json);
        }
        private void HandlePlay(UNOMessage msg)
        {
            if (gameOver) return;

            if (msg.PlayerId != game.getCurrentPlayerIndex())
            {
                SendGameStateToClient();
                return;
            }
            Player player = game.getPlayers()[msg.PlayerId];

            Card card = null;
            List<Card> hand = player.getHand();

            for (int i = 0; i < hand.Count; i++)
            {
                Card c = hand[i];
                if (c.color.ToString() == msg.Color &&
                    c.value.ToString() == msg.Value)
                {
                    card = c;
                    break;
                }
            }
            if (game.drawnThisTurn != null && card!=game.drawnThisTurn)
            {
                if (card != game.drawnThisTurn)
                {
                    SendGameStateToClient();
                    return;
                }
            }
            if (card == null)
            {
                SendGameStateToClient();
                return;
            }
                

            if (!player.IsCardValid(game.getTopCard(), card))
                return;
            game.drawnThisTurn = null;
            game.hasDrawnThisTurn = false;
            Colors chosenColor=card.color;
            if(card.value==Val.Wild || card.value==Val.WildDrawFour)
            {
                if (!Enum.TryParse(msg.ChosenColor, out chosenColor))
                {
                    SendGameStateToClient();
                }
                if(chosenColor!=Colors.Red && chosenColor!=Colors.Green && chosenColor!=Colors.Blue && chosenColor!=Colors.Yellow)
                {
                    SendGameStateToClient();
                    return;
                }
            }
            player.PlayCard(game.getTopCard(), card);
            game.AfterPlayerPlays(card, chosenColor);



            if (player.getHand().Count == 0)
            {
                SendWinnerMessage(msg.PlayerId);
                gameOver = true;
                Stop();
                return;
            }

            SendGameStateToClient();
            game.NextPlayer();
            

           
        }
        private void SendCanPlayDrawnMessage(Card drawn)
        {
            UNOMessage msg = new UNOMessage();
            msg.Type = "CAN_PLAY_DRAWN";
            msg.Color = drawn.color.ToString();
            msg.Value = drawn.value.ToString();
            string json = JsonConvert.SerializeObject(msg);
            scriere.WriteLine(json);
        }
        private void SendWinnerMessage (int playerId)
        {
            UNOMessage msg = new UNOMessage();
            msg.Type = "WINNER";
            msg.PlayerId = playerId;
            string json = JsonConvert.SerializeObject(msg);
            scriere.WriteLine(json);
        }
        private void HandleDraw(UNOMessage msg)
        {
            if (gameOver) return;
            if (msg.PlayerId!=game.getCurrentPlayerIndex())
            {
                SendGameStateToClient();
                return;
            }
            if(game.hasDrawnThisTurn)
            {
                SendGameStateToClient();
                return;
            }
            game.hasDrawnThisTurn = true;
            if (msg.PlayerId != game.getCurrentPlayerIndex())
            {
                SendGameStateToClient();
                return;
            }
            Player player = game.getPlayers()[msg.PlayerId];
            game.getdeck().DrawCard(player, game.getTopCard());
            List<Card> hand = player.getHand();
            Card drawnCard = hand[hand.Count - 1];
            game.drawnThisTurn = drawnCard;
            if (player.IsCardValid(game.getTopCard(), drawnCard)) 
            {
                SendCanPlayDrawnMessage(drawnCard);
                return;
            }
            game.hasDrawnThisTurn = false;
            game.drawnThisTurn = null;
            game.NextPlayer();
            SendGameStateToClient();  
        }
        private void Stop()
        {
            running = false;
            try
            {
                clientSocket?.Shutdown(SocketShutdown.Both);
            }
            catch { }
            try
            {
                clientSocket?.Close();
            }
            catch { }
            try
            {
                server?.Stop();
            }
            catch { }
            try
            {
                listenThread?.Join(200);

            }
            catch { }
            listenThread = null;
            clientSocket = null;
            server = null;
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedCard = sender as PictureBox;
            if (clickedCard == null) return;

            Card selectedCard = clickedCard.Tag as Card;
            if (selectedCard == null) return;

           
            if (game.getTopCard() == null) return;

            if (game.getcurrentPlayer().IsCardValid(game.getTopCard(), selectedCard))
            {
                
                game.getcurrentPlayer().RemoveCard(selectedCard);
                Colors color=game.getTopCard().color;
                if(selectedCard.value==Val.Wild||selectedCard.value==Val.WildDrawFour)
                {
                    panelHandControl.Enabled = false;
                    panel1.Controls.Clear();
                   
                    panel1.Visible = true;
                    int height = 40;
                    int width = 40;
                    int x = 0;
                    int y = 0;
                    int spacing = 50;
                    
                    Button Red = new Button()
                    {
                        Size = new Size(width, height),
                        Location = new Point(x, y),
                        BackColor = Color.Red,
                        Text="Red",
                        ForeColor = Color.Red
                    };
                    
                    Red.Click += (s, ev) =>
                    {
                        Enum.TryParse(Red.Text, out Colors parsedColor);
                        color = parsedColor;
                        game.ApplyEffect(selectedCard, color, game.getPlayers()[(game.getCurrentPlayerIndex() + 1) % 2]);
                        game.setTopCard(selectedCard);
                        panel1.Visible = false;
                        panelHandControl.Enabled = true;
                    };
                    x += spacing;
                    panel1.Controls.Add(Red);
                    Button Blue = new Button()
                    {
                        Size = new Size(width, height),
                        Location = new Point(x, y),
                        BackColor = Color.Blue,
                        Text = "Blue",
                        ForeColor = Color.Blue
                    };
                    Blue.Click += (s, ev) =>
                    {
                        Enum.TryParse(Blue.Text, out Colors parsedColor);
                        color = parsedColor;
                        
                        game.ApplyEffect(selectedCard, color, game.getPlayers()[(game.getCurrentPlayerIndex() + 1) % 2]);
                        game.setTopCard(selectedCard);
                        panel1.Visible = false;
                        panelHandControl.Enabled = true;
                    };
                    x += spacing;
                    panel1.Controls.Add(Blue);
                    Button Yellow = new Button()
                    {
                        Size = new Size(width, height),
                        Location = new Point(x, y),
                        BackColor = Color.Yellow,
                        Text = "Yellow",
                        ForeColor = Color.Yellow
                    };
                    Yellow.Click += (s, ev) =>
                    {
                        Enum.TryParse(Yellow.Text, out Colors parsedColor);
                        color = parsedColor;
                        
                        game.ApplyEffect(selectedCard, color, game.getPlayers()[(game.getCurrentPlayerIndex() + 1) % 2]);
                        game.setTopCard(selectedCard);
                        panel1.Visible = false;
                        panelHandControl.Enabled = true;
                    };
                    x += spacing;
                    panel1.Controls.Add(Yellow);
                    Button Green = new Button()
                    {
                        Size = new Size(width, height),
                        Location = new Point(x, y),
                        BackColor = Color.Green,
                        Text = "Green",
                        ForeColor = Color.Green
                    };
                    Green.Click +=  (s, ev) =>
                    {
                        Enum.TryParse(Green.Text, out Colors parsedColor);
                        color = parsedColor;
                        game.ApplyEffect(selectedCard, color, game.getPlayers()[(game.getCurrentPlayerIndex() + 1) % 2]);
                        game.setTopCard(selectedCard);
                        panel1.Visible = false;
                        panelHandControl.Enabled = true;
                    };
                    panel1.Controls.Add(Green);

                    
                    game.getcurrentPlayer().ShowHand(panelHandControl, PictureBox_Click);
                }
                
                if (selectedCard.value==Val.DrawTwo)
                    game.ApplyEffect(selectedCard, color, game.getPlayers()[(game.getCurrentPlayerIndex() + 1) % 2]);
                if (selectedCard.value == Val.Skip|| selectedCard.value==Val.DrawTwo)
                {
                    
                    game.getcurrentPlayer().RemoveCard(selectedCard);

                    game.setTopCard(selectedCard); 
                    game.getdeck().deck_played.Add(selectedCard);
                    game.ShowTopCard(panelTopCardControl);


                    game.setCurrentPlayerIndex((game.getCurrentPlayerIndex() + 2) % game.getPlayers().Count);
                    game.setcurrentPlayer(game.getPlayers()[game.getCurrentPlayerIndex()]);

                    
                    game.getcurrentPlayer().ShowHand(panelHandControl, PictureBox_Click);
                }

                game.setTopCard(selectedCard);
                game.getdeck().deck_played.Add(selectedCard);
                game.ShowTopCard(panelTopCardControl);
                if (game.getcurrentPlayer().getHand().Count == 0)
                {
                    MessageBox.Show("Player" + game.getCurrentPlayerIndex() + " a castigat");
                    Application.Exit(); 
                }
                if (selectedCard.value != Val.DrawTwo && selectedCard.value != Val.WildDrawFour && selectedCard.value != Val.Skip)
                {

                    game.setCurrentPlayerIndex((game.getCurrentPlayerIndex() + 2) % game.getPlayers().Count);
                    game.setCurrentPlayerIndex((game.getCurrentPlayerIndex() + 2) % game.getPlayers().Count);


                    game.getcurrentPlayer().ShowHand(panelHandControl, PictureBox_Click);

                }
            }
            

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Image original = Image.FromFile(@"..\..\Resources\Deck.png");
            Image resize = new Bitmap(original, new Size(90, 190)); 
            button1.Image = resize;


            game.getcurrentPlayer().getHand().Add(new WildCard(Colors.None, Val.Wild));
            game.getcurrentPlayer().getHand().Add(new WildCard(Colors.None, Val.WildDrawFour));
            game.getcurrentPlayer().getHand().Add(new SpecialCard(Colors.Red, Val.Skip));
            game.getcurrentPlayer().getHand().Add(new SpecialCard(Colors.Red, Val.Skip));
            game.getdeck().deck_played.Add(game.getTopCard());
            game.getcurrentPlayer().ShowHand(panelHandControl, PictureBox_Click);
            game.ShowTopCard(panelTopCardControl);

        }


        private void pictureBoxTest_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
          

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Card topCard = game.getdeck().deck_played[game.getdeck().deck_played.Count - 1];
            game.getdeck().DrawCard(game.getcurrentPlayer(), topCard);
            game.setCurrentPlayerIndex((game.getCurrentPlayerIndex() + 2) % game.getPlayers().Count);
            game.setcurrentPlayer(game.getPlayers()[game.getCurrentPlayerIndex()]);
            game.getcurrentPlayer().ShowHand(panelHandControl, PictureBox_Click);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
