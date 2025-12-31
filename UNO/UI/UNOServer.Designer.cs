namespace UNO
{
    partial class UNOServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UNOServer));
            this.panelHandControl = new System.Windows.Forms.Panel();
            this.panelTopCardControl = new System.Windows.Forms.Panel();
            this.btnDraw = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblConnect = new System.Windows.Forms.Label();
            this.lblTurn = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panelHandControl
            // 
            this.panelHandControl.BackColor = System.Drawing.Color.Transparent;
            this.panelHandControl.Location = new System.Drawing.Point(3, 356);
            this.panelHandControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelHandControl.Name = "panelHandControl";
            this.panelHandControl.Size = new System.Drawing.Size(1024, 169);
            this.panelHandControl.TabIndex = 0;
            // 
            // panelTopCardControl
            // 
            this.panelTopCardControl.BackColor = System.Drawing.Color.Transparent;
            this.panelTopCardControl.Location = new System.Drawing.Point(461, 130);
            this.panelTopCardControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelTopCardControl.Name = "panelTopCardControl";
            this.panelTopCardControl.Size = new System.Drawing.Size(147, 181);
            this.panelTopCardControl.TabIndex = 1;
            this.panelTopCardControl.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTopCardControl_Paint);
            // 
            // btnDraw
            // 
            this.btnDraw.Location = new System.Drawing.Point(919, 15);
            this.btnDraw.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(108, 146);
            this.btnDraw.TabIndex = 2;
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Location = new System.Drawing.Point(409, 299);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(251, 49);
            this.panel1.TabIndex = 3;
            this.panel1.Visible = false;
            // 
            // lblConnect
            // 
            this.lblConnect.AutoSize = true;
            this.lblConnect.Location = new System.Drawing.Point(12, 15);
            this.lblConnect.Name = "lblConnect";
            this.lblConnect.Size = new System.Drawing.Size(128, 16);
            this.lblConnect.TabIndex = 4;
            this.lblConnect.Text = "Waiting for Player 2..";
            // 
            // lblTurn
            // 
            this.lblTurn.AutoSize = true;
            this.lblTurn.Location = new System.Drawing.Point(493, 15);
            this.lblTurn.Name = "lblTurn";
            this.lblTurn.Size = new System.Drawing.Size(0, 16);
            this.lblTurn.TabIndex = 5;
            // 
            // UNOServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1043, 567);
            this.Controls.Add(this.lblTurn);
            this.Controls.Add(this.lblConnect);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnDraw);
            this.Controls.Add(this.panelHandControl);
            this.Controls.Add(this.panelTopCardControl);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UNOServer";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelHandControl;
        private System.Windows.Forms.Panel panelTopCardControl;
        private System.Windows.Forms.Button btnDraw;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblConnect;
        private System.Windows.Forms.Label lblTurn;
    }
}

