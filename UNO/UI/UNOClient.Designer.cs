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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UNOClient));
            this.panelHandControl = new System.Windows.Forms.Panel();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.panelTopCardControl = new System.Windows.Forms.Panel();
            this.btnDraw = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTurn = new System.Windows.Forms.Label();
            this.panelOpponentHand = new System.Windows.Forms.Panel();
            this.panelHandControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHandControl
            // 
            this.panelHandControl.BackColor = System.Drawing.Color.Transparent;
            this.panelHandControl.Controls.Add(this.txtIP);
            this.panelHandControl.Controls.Add(this.btnConnect);
            this.panelHandControl.Location = new System.Drawing.Point(3, 356);
            this.panelHandControl.Margin = new System.Windows.Forms.Padding(4);
            this.panelHandControl.Name = "panelHandControl";
            this.panelHandControl.Size = new System.Drawing.Size(1024, 169);
            this.panelHandControl.TabIndex = 0;
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(458, 90);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 22);
            this.txtIP.TabIndex = 2;
            this.txtIP.Text = "127.0.0.1";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(574, 80);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(83, 43);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // panelTopCardControl
            // 
            this.panelTopCardControl.BackColor = System.Drawing.Color.Transparent;
            this.panelTopCardControl.Location = new System.Drawing.Point(461, 130);
            this.panelTopCardControl.Margin = new System.Windows.Forms.Padding(4);
            this.panelTopCardControl.Name = "panelTopCardControl";
            this.panelTopCardControl.Size = new System.Drawing.Size(147, 181);
            this.panelTopCardControl.TabIndex = 1;
            // 
            // btnDraw
            // 
            this.btnDraw.Location = new System.Drawing.Point(919, 15);
            this.btnDraw.Margin = new System.Windows.Forms.Padding(4);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(108, 146);
            this.btnDraw.TabIndex = 0;
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Location = new System.Drawing.Point(409, 299);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(251, 49);
            this.panel1.TabIndex = 4;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // lblTurn
            // 
            this.lblTurn.AutoSize = true;
            this.lblTurn.Location = new System.Drawing.Point(875, 253);
            this.lblTurn.Name = "lblTurn";
            this.lblTurn.Size = new System.Drawing.Size(0, 16);
            this.lblTurn.TabIndex = 5;
            // 
            // panelOpponentHand
            // 
            this.panelOpponentHand.BackColor = System.Drawing.Color.Transparent;
            this.panelOpponentHand.Location = new System.Drawing.Point(19, 11);
            this.panelOpponentHand.Margin = new System.Windows.Forms.Padding(4);
            this.panelOpponentHand.Name = "panelOpponentHand";
            this.panelOpponentHand.Size = new System.Drawing.Size(1024, 169);
            this.panelOpponentHand.TabIndex = 8;
            // 
            // UNOClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1043, 567);
            this.Controls.Add(this.lblTurn);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnDraw);
            this.Controls.Add(this.panelTopCardControl);
            this.Controls.Add(this.panelHandControl);
            this.Controls.Add(this.panelOpponentHand);
            this.DoubleBuffered = true;
            this.Name = "UNOClient";
            this.Text = "UNOClient";
            this.Load += new System.EventHandler(this.UNOClient_Load);
            this.panelHandControl.ResumeLayout(false);
            this.panelHandControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelHandControl;
        private System.Windows.Forms.Panel panelTopCardControl;
        private System.Windows.Forms.Button btnDraw;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTurn;
        private System.Windows.Forms.Panel panelOpponentHand;
    }
}