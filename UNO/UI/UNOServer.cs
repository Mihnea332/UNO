using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UNO.Model;
using UNO.Logic;
namespace UNO
{
    public partial class UNOServer : Form
    {
        private Game game;
        private TcpListener server;
        private TcpClient connection;
        private NetworkStream stream;
        private Thread t;
        private bool isServerRunning = true;
        private int opponentCardCount = 5;
        private bool backToMenu = false;
        public UNOServer()
        {
            InitializeComponent();
            this.Text = "UNO Server - Player 1";
            game = new Game();
            //Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosed += (s, e) =>
            {
                if (stream != null) stream.Close();
                if (connection != null) connection.Close();
                if (server != null) server.Stop();
                isServerRunning = false;

                if (!backToMenu)
                {
                    if (server != null) server.Stop();
                    Environment.Exit(0);
                }
                
                
            };

        }
        private void HandleClientDisconnect()
        {
            if (this.Disposing || this.IsDisposed) return;
            this.Invoke((MethodInvoker)delegate
            {
                isServerRunning = false;
                MessageBox.Show("Client Disconnected, returning to Main Menu");
                backToMenu = true;
                MainMenu menu = new MainMenu();
                menu.Show();
                this.Close();
            });
        }
        private void SendMessage(string msg)
        {
            if (connection != null && connection.Connected)
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.AutoFlush = true;
                writer.WriteLine(msg);
            }
        }
        private void StartServer()
        {
            try
            {
                server = new TcpListener(System.Net.IPAddress.Any, 3000);
                server.Start();
                MessageBox.Show("Server started! Waiting for Player 2");
                connection = server.AcceptTcpClient();
                stream = connection.GetStream();
                this.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show("Player 2 connected!");
                    lblConnect.Text = "Connected!";
                    game.ShowOpponentHand(panelOpponentHand, opponentCardCount);

                });
                Player player2 = game.getPlayers()[1];
                string handString = "";
                foreach(Card c in player2.getHand())
                {
                    
                    string cardText = c.color.ToString() + "_" + c.value.ToString();
                    handString = handString + cardText+";";
                }
                SendMessage("HAND:" + handString);
                
                Card top = game.getTopCard();
                
                string topCardText = top.color.ToString() + "_" + top.value.ToString();
                SendMessage("TOP:" + topCardText);
                Thread listenthread = new Thread(ListenToClient);
                listenthread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server error: " + ex.Message);
            }

        }
        private Card StringToCard(string text)
        {
            string[] parts = text.Split('_');
            Colors c = (Colors)Enum.Parse(typeof(Colors), parts[0]);
            Val v = (Val)Enum.Parse(typeof(Val), parts[1]);
            Card createdCard;

            if (v == Val.Wild || v == Val.WildDrawFour) createdCard= new WildCard(c, v);
            if (v == Val.Skip || v == Val.Reverse || v == Val.DrawTwo) createdCard = new SpecialCard(c, v);
            else createdCard = new NormalCard(c, v);
            return createdCard;
        }
        private void ProcessData(string data)
        {
            try
            {
                string[] parts = data.Split(':');
                string command = parts[0];

                if (command == "PLAY")
                {
                    opponentCardCount--;
                    game.ShowOpponentHand(panelOpponentHand, opponentCardCount);
                    Card c = StringToCard(parts[1]);
                    game.ShowOpponentHand(panelOpponentHand, opponentCardCount);
                    game.setTopCard(c);
                    game.ShowTopCard(panelTopCardControl);

                    if (c.value == Val.Wild || c.value == Val.WildDrawFour)
                    {
                        System.Drawing.Color visualColor = System.Drawing.Color.FromName(c.color.ToString());
                        panelTopCardControl.BackColor = visualColor;
                    }
                    else
                    {
                        panelTopCardControl.BackColor = System.Drawing.Color.Transparent;
                    }

                    if (c.value == Val.DrawTwo)
                    {
                       // MessageBox.Show("Ai primit +2! Tragi 2 cărți și stai o tură.");
                        Player me = game.getcurrentPlayer();
                        for (int i = 0; i < 2; i++)
                        {
                            Card drawn = game.getdeck().Draw();
                            if (drawn != null) me.getHand().Add(drawn);
                        }
                        me.ShowHand(panelHandControl, PictureBox_Click);
                        SendMessage("SKIP");
                    }
                    else if (c.value == Val.Skip)
                    {
                       // MessageBox.Show("Ai primit Skip! Stai o tură.");
                        SendMessage("SKIP");
                    }
                    else if (c.value == Val.WildDrawFour)
                    {
                       // MessageBox.Show("Ai primit +4! Tragi 4 cărți și stai o tură.");
                        Player me = game.getcurrentPlayer();
                        for (int i = 0; i < 4; i++)
                        {
                            Card drawn = game.getdeck().Draw();
                            if (drawn != null) me.getHand().Add(drawn);
                        }
                        me.ShowHand(panelHandControl, PictureBox_Click);
                        SendMessage("SKIP");
                    }
                    else
                    {
                       // MessageBox.Show("E rândul tău!");
                        panelHandControl.Enabled = true;
                        btnDraw.Enabled = true;
                        lblTurn.Text = "Your turn";
                    }
                }
                else if (command == "DRAW")
                {
                    opponentCardCount++;
                    game.ShowOpponentHand(panelOpponentHand, opponentCardCount);
                    Card c = game.getdeck().Draw();
                    string cardText = c.color.ToString() + "_" + c.value.ToString();
                    string handString = "";
                    handString = handString + cardText + ";";
                    SendMessage("DRAW_CARD:" + handString);
                    // MessageBox.Show("Adversarul a tras o carte. E rândul tău!");
                    panelHandControl.Enabled = true;
                    btnDraw.Enabled = true;
                    lblTurn.Text = "Your turn";
                }
                else if (command == "WIN")
                {
                    MessageBox.Show("You lost");
                    Application.Exit();
                }
                else if (command == "SKIP")
                {
                   // MessageBox.Show("Adversarul a stat o tură (din cauza +2/+4/Skip). Joci din nou!");
                    
                    panelHandControl.Enabled = true;
                    btnDraw.Enabled = true;
                    lblTurn.Text = "Your turn";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }
        private void ListenToClient()
        {
            StreamReader reader = new StreamReader(stream);
            while(isServerRunning)
            {
                try
                {
                    string data = reader.ReadLine();
                    if (data!=null)
                    {
                        this.Invoke((MethodInvoker)delegate {
                            ProcessData(data);
                        });
                    }   
                    else
                    {
                        HandleClientDisconnect();
                        break;
                    }
                }
                catch
                {
                    HandleClientDisconnect();
                    break;
                }

            }
            
        }
        private void PlayCardAndSend(Card card,Colors finalColor)
        {
            game.getcurrentPlayer().RemoveCard(card);
            card.color = finalColor;
            game.setTopCard(card);
            game.ShowTopCard(panelTopCardControl);
            game.getcurrentPlayer().ShowHand(panelHandControl, PictureBox_Click);
            if(game.getcurrentPlayer().getHand().Count==0)
            {
                SendMessage("WIN");
                MessageBox.Show("You Win!");
                Application.Exit();
            }
            else
            {
                string msg="PLAY:"+finalColor.ToString()+"_"+card.value.ToString();
                SendMessage(msg);
            }
            panelHandControl.Enabled = false;
            btnDraw.Enabled = false;
            lblTurn.Text = "Opponent's turn";
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedCard = sender as PictureBox;
            if (clickedCard == null) return;
            Card selectedCard = clickedCard.Tag as Card;
            if (selectedCard == null) return;
            if(selectedCard.value==Val.DrawTwo)
            {
                
                game.ShowOpponentHand(panelOpponentHand, opponentCardCount);
            }
            if (selectedCard.value==Val.WildDrawFour)
            {
               
                game.ShowOpponentHand(panelOpponentHand, opponentCardCount);
            }
            if (game.getcurrentPlayer().IsCardValid(game.getTopCard(), selectedCard)) 
            {
                if(selectedCard.value==Val.Wild|| selectedCard.value==Val.WildDrawFour)
                {
                    panelHandControl.Enabled = false;
                    btnDraw.Enabled = false;
                    lblTurn.Text = "Opponent's turn";
                    panel1.Controls.Clear();
                    panel1.Visible = true;
                    SetupColorButtons(selectedCard);
                }
                else
                {
                    PlayCardAndSend(selectedCard, selectedCard.color);
                }
                
            }
            
        }
        private void SetupColorButtons(Card selectedCard)
        {
            int x = 0, y = 0, spacing = 50;
            System.Drawing.Color[] colors = {
                System.Drawing.Color.Red,
                System.Drawing.Color.Blue,
                System.Drawing.Color.Yellow,
                System.Drawing.Color.Green
            };
            string[] colorNames = { "Red", "Blue", "Yellow", "Green" };
            for(int i=0;i<4;i++)
            {
                Button btn = new Button();
                btn.Size = new Size(40, 40);
                btn.Location = new Point(x, y);
                btn.BackColor = colors[i];
                btn.Text = colorNames[i];
                btn.Click += (s, ev) =>
                {
                    Enum.TryParse(btn.Text, out Colors parsedColor);
                    panel1.Visible = false;
                    PlayCardAndSend(selectedCard, parsedColor);
                };
                panel1.Controls.Add(btn);
                x += spacing;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Image original = Image.FromFile(@"..\..\Resources\Deck.png");
            Image resize = new Bitmap(original, new Size(90, 190)); 
            btnDraw.Image = resize;
            game.getdeck().deck_played.Add(game.getTopCard());
            game.getcurrentPlayer().ShowHand(panelHandControl, PictureBox_Click);
            game.ShowTopCard(panelTopCardControl);
            t = new Thread(StartServer);
            t.Start();
            panelHandControl.Enabled = false;
            btnDraw.Enabled = false;
            lblTurn.Text = "Opponent's turn";
            
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

            Card c = game.getdeck().Draw();
            if(c!=null)
            {
                game.getcurrentPlayer().getHand().Add(c);
                game.getcurrentPlayer().ShowHand(panelHandControl, PictureBox_Click);
                SendMessage("DRAW");
                panelHandControl.Enabled = false;
                lblTurn.Text = "Opponent's turn";
                btnDraw.Enabled = false;

            }    
        }

        private void panelTopCardControl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelOpponentHand_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
