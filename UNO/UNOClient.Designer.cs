namespace UNO
{
    partial class UNOClient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowHand = new System.Windows.Forms.FlowLayoutPanel();
            this.labelOpponent = new System.Windows.Forms.Label();
            this.labelTurn = new System.Windows.Forms.Label();
            this.panelTopCard = new System.Windows.Forms.Panel();
            this.panelChooseColor = new System.Windows.Forms.Panel();
            this.btnPlayDrawnCard = new System.Windows.Forms.Button();
            this.btnDraw = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnPass = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // flowHand
            // 
            this.flowHand.Location = new System.Drawing.Point(196, 372);
            this.flowHand.Name = "flowHand";
            this.flowHand.Size = new System.Drawing.Size(580, 100);
            this.flowHand.TabIndex = 1;
            // 
            // labelOpponent
            // 
            this.labelOpponent.AutoSize = true;
            this.labelOpponent.Location = new System.Drawing.Point(333, 22);
            this.labelOpponent.Name = "labelOpponent";
            this.labelOpponent.Size = new System.Drawing.Size(44, 16);
            this.labelOpponent.TabIndex = 2;
            this.labelOpponent.Text = "label1";
            this.labelOpponent.Click += new System.EventHandler(this.label1_Click);
            // 
            // labelTurn
            // 
            this.labelTurn.AutoSize = true;
            this.labelTurn.Location = new System.Drawing.Point(704, 9);
            this.labelTurn.Name = "labelTurn";
            this.labelTurn.Size = new System.Drawing.Size(44, 16);
            this.labelTurn.TabIndex = 3;
            this.labelTurn.Text = "label1";
            // 
            // panelTopCard
            // 
            this.panelTopCard.Location = new System.Drawing.Point(336, 140);
            this.panelTopCard.Name = "panelTopCard";
            this.panelTopCard.Size = new System.Drawing.Size(200, 100);
            this.panelTopCard.TabIndex = 4;
            // 
            // panelChooseColor
            // 
            this.panelChooseColor.Location = new System.Drawing.Point(308, 311);
            this.panelChooseColor.Name = "panelChooseColor";
            this.panelChooseColor.Size = new System.Drawing.Size(250, 55);
            this.panelChooseColor.TabIndex = 0;
            // 
            // btnPlayDrawnCard
            // 
            this.btnPlayDrawnCard.Location = new System.Drawing.Point(184, 173);
            this.btnPlayDrawnCard.Name = "btnPlayDrawnCard";
            this.btnPlayDrawnCard.Size = new System.Drawing.Size(75, 67);
            this.btnPlayDrawnCard.TabIndex = 5;
            this.btnPlayDrawnCard.Text = "Play drawn card?";
            this.btnPlayDrawnCard.UseVisualStyleBackColor = true;
            this.btnPlayDrawnCard.Click += new System.EventHandler(this.btnPlayDrawnCard_Click);
            // 
            // btnDraw
            // 
            this.btnDraw.Location = new System.Drawing.Point(962, 32);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(75, 112);
            this.btnDraw.TabIndex = 6;
            this.btnDraw.Text = "button1";
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.button1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnPass
            // 
            this.btnPass.Location = new System.Drawing.Point(12, 65);
            this.btnPass.Name = "btnPass";
            this.btnPass.Size = new System.Drawing.Size(75, 23);
            this.btnPass.TabIndex = 8;
            this.btnPass.Text = "button2";
            this.btnPass.UseVisualStyleBackColor = true;
            this.btnPass.Click += new System.EventHandler(this.btnPass_Click_1);
            // 
            // UNOClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.btnPass);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnDraw);
            this.Controls.Add(this.btnPlayDrawnCard);
            this.Controls.Add(this.panelChooseColor);
            this.Controls.Add(this.panelTopCard);
            this.Controls.Add(this.labelTurn);
            this.Controls.Add(this.labelOpponent);
            this.Controls.Add(this.flowHand);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UNOClient";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.UNOClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowHand;
        private System.Windows.Forms.Label labelOpponent;
        private System.Windows.Forms.Label labelTurn;
        private System.Windows.Forms.Panel panelTopCard;
        private System.Windows.Forms.Panel panelChooseColor;
        private System.Windows.Forms.Button btnPlayDrawnCard;
        private System.Windows.Forms.Button btnDraw;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnPass;
    }
}