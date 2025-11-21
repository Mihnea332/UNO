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
using UNO.Model;
using UNO.Logic;
namespace UNO
{
    public partial class Form1 : Form
    {
        private Game game;
        private Panel panelHand;
        private Panel panelTopCard;

        public Form1()
        {
            InitializeComponent();
            game = new Game(2);
            game.deck = new Deck();
            game.currentPlayer = new Player("Mihnea");
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedCard = sender as PictureBox;
            if (clickedCard == null) return;

            Card selectedCard = clickedCard.Tag as Card;
            if (selectedCard == null) return;

           
            if (game.TopCard == null) return;

            if (game.currentPlayer.IsCardValid(game.TopCard, selectedCard))
            {
                
                game.currentPlayer.RemoveCard(selectedCard);
                Colors color=game.TopCard.color;
                if(selectedCard.value==Val.Wild||selectedCard.value==Val.WildDrawFour)
                {
                    panel1.Visible = true;
                    int height = 40;
                    int width = 40;
                    int x = 100;
                    int y = 100;
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
                        MessageBox.Show("Ai ales: " + parsedColor);
                        game.ApplyEffect(selectedCard, color);
                        panel1.Visible = false;
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
                        MessageBox.Show("Ai ales: " + parsedColor);
                        game.ApplyEffect(selectedCard, color);
                        panel1.Visible = false;
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
                        MessageBox.Show("Ai ales: " + parsedColor);
                        game.ApplyEffect(selectedCard, color);
                        panel1.Visible = false;
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
                        MessageBox.Show("Ai ales: " + parsedColor);
                        game.ApplyEffect(selectedCard, color);
                        panel1.Visible = false;
                    };
                    panel1.Controls.Add(Green);



                }
               
                game.deck.deck_played.Add(selectedCard);

                game.ShowTopCard(panelTopCard);
                game.currentPlayer.ShowHand(panelHand, PictureBox_Click);

            }
            else
            {
                MessageBox.Show("Cartea nu este validă!");
            }
           
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Image original = Image.FromFile(@"..\..\Resources\Deck.png");
            Image resize = new Bitmap(original, new Size(90, 190)); 
            button1.Image = resize;
            panelHand = panelHandControl;
            panelTopCard = panelTopCardControl;

            game.currentPlayer = new Player("Mihnea");
            game.deck = new Deck();

            game.deck.Generate();
            game.deck.Shuffle();
            game.deck.Deal(game.currentPlayer);
            game.currentPlayer.Hand.Add(new WildCard(Colors.None, Val.Wild));
            game.currentPlayer.Hand.Add(new WildCard(Colors.None, Val.WildDrawFour));
            game.TopCard = game.deck.GetTopCard();
            game.deck.deck_played.Add(game.TopCard);

            game.currentPlayer.ShowHand(panelHand, PictureBox_Click);
            game.ShowTopCard(panelTopCard);

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
            
            Card topCard = game.deck.deck_played[game.deck.deck_played.Count - 1];
            game.deck.DrawCard(game.currentPlayer, topCard);
            game.currentPlayer.ShowHand(panelHand, PictureBox_Click);
        }
    }
}
