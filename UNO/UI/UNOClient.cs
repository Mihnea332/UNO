using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using UNO.Model;
using UNO.Logic;
using System.IO;


namespace UNO
{
    public partial class UNOClient : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread t;
        private bool isConnected = false;
        private Game game;
        private Card currentTopCard;
        private int opponentCardCount=5;

        public UNOClient()
        {
            InitializeComponent();
            game = new Game();
            game.setcurrentPlayer(game.getPlayers()[1]);
            game.getPlayers().Clear();
            game.getdeck().deck.Clear();
            game.getdeck().deck_played.Clear();
            this.Text = "UNO Client - Player 2";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            game.getcurrentPlayer().getHand().Clear();
   
            this.FormClosed += (s, e) => {
                isConnected = false;
                if (t != null) t.Abort();
                if (client != null) client.Close();
                Environment.Exit(0);
            };
        }
        private Card StringToCard(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text)) return null;
                string[] parts = text.Split('_');
                Colors c = (Colors)Enum.Parse(typeof(Colors), parts[0]);
                Val v = (Val)Enum.Parse(typeof(Val), parts[1]);
                Card createdCard;
                if (v == Val.Wild || v == Val.WildDrawFour)
                    createdCard = new WildCard(c, v);
                else if (v == Val.Skip || v == Val.Reverse || v == Val.DrawTwo)
                    createdCard = new SpecialCard(c, v);
                else
                    createdCard = new NormalCard(c, v);
                createdCard.color = c;
                return createdCard;
            }
            catch
            {
                return null;
            }
        }
        private void SendMessage(string msg)
        {
            if(isConnected&& stream!=null)
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.AutoFlush = true;
                writer.WriteLine(msg);
                
            }
        }
        private void ListenToClient()
        {
            StreamReader reader = new StreamReader(stream);
            while(isConnected)
            {
                
                try
                {
                    string data = reader.ReadLine();
                    if(data!=null)
                        this.Invoke((MethodInvoker)delegate {
                            ProcessData(data);
                        });
                    else break;
                }
                catch
                {
                    break;
                }
            }
        }
        private void ProcessData(string data)
        {
            
            string[] parts = data.Split(':');
            string command = parts[0];
            if (command == "HAND")
            {
                string[] cardsArray = parts[1].Split(';');
                Player me = game.getcurrentPlayer();
                foreach (string cStr in cardsArray)
                {
                    Card c = StringToCard(cStr);
                    if (c != null) me.getHand().Add(c);
                }
                me.ShowHand(panelHandControl, PictureBox_Click);
                
            }
            else if (command == "TOP")
            {
                Card c = StringToCard(parts[1]);
                game.setTopCard(c);
                game.ShowTopCard(panelTopCardControl);
            }
            else if (command == "PLAY")
            {
                opponentCardCount--;
                game.ShowOpponentHand(panelOpponentHand, opponentCardCount);
                Card c = StringToCard(parts[1]);
                game.setTopCard(c);
               
                if (c.value == Val.Wild || c.value == Val.WildDrawFour)
                {
                    panelTopCardControl.Controls.Clear();
                    System.Drawing.Color visualColor = System.Drawing.Color.FromName(c.color.ToString());
                    panelTopCardControl.BackColor = visualColor;
                }
                else
                {
                    game.ShowTopCard(panelTopCardControl);
                    panelTopCardControl.BackColor = System.Drawing.Color.Transparent;
                }
                if(c.value==Val.DrawTwo)
                {
                   // MessageBox.Show("Draw Two!");
                    Player me = game.getcurrentPlayer();
                    for(int i=0;i<2;i++)
                    {
                        SendMessage("DRAW");
                    }
                    me.ShowHand(panelHandControl, PictureBox_Click);
                    SendMessage("SKIP");
                }
                else if(c.value==Val.Skip)
                {
                    //MessageBox.Show("Skip played!");
                    SendMessage("SKIP");
                }
                else if(c.value==Val.WildDrawFour)
                {
                   // MessageBox.Show("Draw Four!");
                    Player me = game.getcurrentPlayer();
                    for(int i=0;i<4;i++)
                    {
                        SendMessage("DRAW");
                    }
                    me.ShowHand(panelHandControl, PictureBox_Click);
                    SendMessage("SKIP");
                }
                else
                {
                   // MessageBox.Show("Your turn");
                    panelHandControl.Enabled = true;
                    btnDraw.Enabled = true;
                    lblTurn.Text = "Your turn";
                }
            }
            else if (command == "DRAW")
            {
                opponentCardCount++;
                game.ShowOpponentHand(panelOpponentHand, opponentCardCount);
                // MessageBox.Show("Opponent Drew.Your Turn");
                panelHandControl.Enabled = true;
                btnDraw.Enabled = true;
                lblTurn.Text = "Your turn";
            }
            else if(command=="DRAW_CARD")
            {
                string cardData = parts[1].Replace(";", "").Trim();
                Card drawnCard = StringToCard(cardData);
                if(drawnCard!=null)
                {
                    game.getcurrentPlayer().getHand().Add(drawnCard);
                    game.getcurrentPlayer().ShowHand(panelHandControl, PictureBox_Click);
                    panelHandControl.Enabled = false;
                    btnDraw.Enabled = false;
                    lblTurn.Text = "Opponent's turn";
                }
            }
            else if (command == "WIN")
            {
                MessageBox.Show("You lost");
                Application.Exit();
            }
            else if(command=="SKIP")
            {
               // MessageBox.Show("Opponent Skipped.");
                panelHandControl.Enabled = true;
                btnDraw.Enabled = true;
                lblTurn.Text = "Your turn";
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
            for (int i = 0; i < 4; i++)
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
        private void PlayCardAndSend(Card card, Colors finalColor)
        {
            game.getcurrentPlayer().RemoveCard(card);
            card.color = finalColor;
            game.setTopCard(card);
            game.getcurrentPlayer().ShowHand(panelHandControl, PictureBox_Click);
            if (card.value == Val.Wild|| card.value==Val.WildDrawFour)
            {
                panelTopCardControl.Controls.Clear();
                panelTopCardControl.BackColor = System.Drawing.Color.FromName(finalColor.ToString());
            }
            if (game.getcurrentPlayer().getHand().Count == 0)
            {
                SendMessage("WIN");
                MessageBox.Show("You Win!");
                Application.Exit();
            }
            else
            {
                string msg = "PLAY:" + finalColor.ToString() + "_" + card.value.ToString();
                SendMessage(msg);
            }
            panelHandControl.Enabled = false;
            btnDraw.Enabled = false;

            lblTurn.Text = "Opponent's turn";
        }
        private void PictureBox_Click(object sender,EventArgs e)
        {
            PictureBox clickedPB = sender as PictureBox;
            Card selectedCard = clickedPB.Tag as Card;
            bool isValid = false;
            Card top = game.getTopCard();
            if(selectedCard.value==Val.DrawTwo)
            {
                opponentCardCount += 2;
                game.ShowOpponentHand(panelOpponentHand, opponentCardCount);

            }
            if(selectedCard.value==Val.WildDrawFour)
            {
                opponentCardCount += 4;
                game.ShowOpponentHand(panelOpponentHand, opponentCardCount);

            }
            if (selectedCard.color == top.color) isValid = true;
            else if (selectedCard.value == top.value) isValid = true;
            else if (selectedCard.value == Val.Wild || selectedCard.value == Val.WildDrawFour) isValid = true;
            if (isValid)
            {
                if (selectedCard.value == Val.Wild || selectedCard.value == Val.WildDrawFour)
                {

                    SetupColorButtons(selectedCard);
                    return;
                }

                PlayCardAndSend(selectedCard, selectedCard.color);
            }
            else
                MessageBox.Show("Carte invalida!");
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string ipAddress = txtIP.Text;
                client = new TcpClient(ipAddress, 3000);
                stream = client.GetStream();
                isConnected = true;
                t = new Thread(ListenToClient);
                t.Start();
                MessageBox.Show("Connected");
                game.ShowOpponentHand(panelOpponentHand, opponentCardCount);
                btnConnect.Visible = false;
                txtIP.Visible = false;
                btnDraw.Visible = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void btnDraw_Click(object sender,EventArgs e)
        {
                SendMessage("DRAW");
                panelHandControl.Enabled = false;
                btnDraw.Enabled = false;
                lblTurn.Text = "Opponent's turn";
            
            
        }

        private void UNOClient_Load(object sender, EventArgs e)
        {
            
            Image original = Image.FromFile(@"..\..\Resources\Deck.png");
            Image resize = new Bitmap(original, new Size(90, 190));
            btnDraw.Image = resize;
            lblTurn.Text = "Your turn";
            

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelOpponentHand_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
