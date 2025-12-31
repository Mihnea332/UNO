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

        public UNOServer()
        {
            InitializeComponent();
            game = new Game();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosed += (s, e) =>
            {
                isServerRunning = false;
                Environment.Exit(0);
                
            };

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
                MessageBox.Show("Player 2 connected!");
                string handString = "";
                for (int i = 0; i < 7; i++)
                {
                    Card c = game.getdeck().Draw();
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
                if(command=="PLAY")
                {
                    string cardData = parts[1];
                    Card playedCard = StringToCard(cardData);
                    game.setTopCard(playedCard);
                    game.ShowTopCard(panelTopCardControl);
                    if(playedCard.value==Val.Wild||playedCard.value==Val.WildDrawFour)
                    {
                        System.Drawing.Color visualColor = System.Drawing.Color.FromName(playedCard.color.ToString());
                        panelTopCardControl.BackColor = visualColor;
                    }
                    else
                    {
                        panelTopCardControl.BackColor = System.Drawing.Color.Transparent;
                    }
                    MessageBox.Show("Opponent played " + playedCard.ToString());
                    panelHandControl.Enabled = true;
                }
                else if(command=="DRAW")
                {
                    MessageBox.Show("Opponent drew a card");
                    panelHandControl.Enabled = true;
                }
                else if (command=="WIN")
                {
                    MessageBox.Show("Opponent won!");
                    Application.Exit();
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
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
                        break;
                    }
                }
                catch
                {
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
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedCard = sender as PictureBox;
            if (clickedCard == null) return;
            Card selectedCard = clickedCard.Tag as Card;
            if (selectedCard == null) return;
            if (game.getcurrentPlayer().IsCardValid(game.getTopCard(), selectedCard)) 
            {
                if(selectedCard.value==Val.Wild|| selectedCard.value==Val.WildDrawFour)
                {
                    panelHandControl.Enabled = false;
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
            button1.Image = resize;


            game.getcurrentPlayer().getHand().Add(new WildCard(Colors.None, Val.Wild));
            game.getcurrentPlayer().getHand().Add(new WildCard(Colors.None, Val.WildDrawFour));
            game.getcurrentPlayer().getHand().Add(new SpecialCard(Colors.Red, Val.Skip));
            game.getcurrentPlayer().getHand().Add(new SpecialCard(Colors.Red, Val.Skip));
            game.getdeck().deck_played.Add(game.getTopCard());
            game.getcurrentPlayer().ShowHand(panelHandControl, PictureBox_Click);
            game.ShowTopCard(panelTopCardControl);
            t = new Thread(StartServer);
            t.Start();
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
            }    
        }
    }
}
