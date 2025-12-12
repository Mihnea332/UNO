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


        public Form1()
        {
            InitializeComponent();
            game = new Game();
            this.FormClosed += (s, e) => Application.Exit();


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
    }
}
