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
        public UNOClient()
        {
            InitializeComponent();
            game = new Game();
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            game.getcurrentPlayer().getHand().Clear();
            Control.CheckForIllegalCrossThreadCalls = false;
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
                if (v == Val.Wild || v == Val.WildDrawFour) return new WildCard(c, v);
                if (v == Val.Skip || v == Val.Reverse || v == Val.DrawTwo) return new SpecialCard(c, v);
                return new NormalCard(c, v);
              
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
                Card c = StringToCard(parts[1]);
                game.setTopCard(c);
                game.ShowTopCard(panelTopCardControl);
                MessageBox.Show("Your turn");
                panelHandControl.Enabled = true;
                btnDraw.Enabled = true;
            }
            else if (command == "DRAW")
            {
                MessageBox.Show("Opponent Drew.Your Turn");
                panelHandControl.Enabled = true;
                btnDraw.Enabled = true;
            }
            else if (command == "WIN")
            {
                MessageBox.Show("You lost");
                Application.Exit();
            }
        }
        private void ShowColorMenu(Card wildCard)
        {
            
            Panel pnlColors = new Panel();
            pnlColors.Size = new Size(200, 200);
            
            pnlColors.Location = new Point((this.Width - 200) / 2, (this.Height - 200) / 2);
            pnlColors.BackColor = Color.Gray;
            pnlColors.Name = "pnlColorSelect";

            string[] colorNames = { "Red", "Blue", "Yellow", "Green" };
            Color[] drawColors = { Color.Red, Color.Blue, Color.Yellow, Color.Green };

            int x = 10, y = 10;

            
            for (int i = 0; i < 4; i++)
            {
                Button btn = new Button();
                btn.Text = colorNames[i];
                btn.BackColor = drawColors[i];
                btn.Size = new Size(80, 80);
                btn.Location = new Point(x, y);

                
                btn.Click += (s, e) =>
                {
                    
                    Enum.TryParse(btn.Text, out Colors selectedColor);

                    
                    this.Controls.Remove(pnlColors);
                    panelHandControl.Enabled = true;

                    
                    PlayCardAndSend(wildCard, selectedColor);
                };

                pnlColors.Controls.Add(btn);

                
                x += 90;
                if (i == 1) { x = 10; y += 90; }
            }

            
            this.Controls.Add(pnlColors);
            pnlColors.BringToFront();

           
            panelHandControl.Enabled = false;
        }
        private void PlayCardAndSend(Card card, Colors finalColor)
        {
            game.getcurrentPlayer().RemoveCard(card);
            card.color = finalColor;
            game.setTopCard(card);
            game.ShowTopCard(panelTopCardControl);
            game.getcurrentPlayer().ShowHand(panelHandControl, PictureBox_Click);
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
        }
        private void PictureBox_Click(object sender,EventArgs e)
        {
            PictureBox clickedPB = sender as PictureBox;
            Card selectedCard = clickedPB.Tag as Card;
            bool isValid = false;
            Card top = game.getTopCard();
            if (selectedCard.color == top.color) isValid = true;
            else if (selectedCard.value == top.value) isValid = true;
            else if (selectedCard.value == Val.Wild || selectedCard.value == Val.WildDrawFour) isValid = true;
            if (isValid)
            {
                if (selectedCard.value == Val.Wild || selectedCard.value == Val.WildDrawFour)
                {

                    ShowColorMenu(selectedCard);
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
                client = new TcpClient("127.0.0.1", 3000);
                stream = client.GetStream();
                isConnected = true;
                t = new Thread(ListenToClient);
                t.Start();
                MessageBox.Show("Connected");
                btnConnect.Visible = false;
                txtIP.Visible = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void btnDraw_Click(object sender,EventArgs e)
        {
            Card c = game.getdeck().Draw();
            if (c != null)
            {
                game.getcurrentPlayer().getHand().Add(c);
                game.getcurrentPlayer().ShowHand(panelHandControl, PictureBox_Click);
                SendMessage("DRAW");
                panelHandControl.Enabled = false;
            }
            this.Enabled = false;
        }

        private void UNOClient_Load(object sender, EventArgs e)
        {
            game.getcurrentPlayer().getHand().Add(new WildCard(Colors.None, Val.Wild));
        }
    }
}
