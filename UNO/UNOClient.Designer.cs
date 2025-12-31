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
            this.panelHandControl = new System.Windows.Forms.Panel();
            this.panelTopCardControl = new System.Windows.Forms.Panel();
            this.btnDraw = new System.Windows.Forms.Button();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelHandControl
            // 
            this.panelHandControl.Location = new System.Drawing.Point(3, 307);
            this.panelHandControl.Name = "panelHandControl";
            this.panelHandControl.Size = new System.Drawing.Size(763, 131);
            this.panelHandControl.TabIndex = 0;
            // 
            // panelTopCardControl
            // 
            this.panelTopCardControl.Location = new System.Drawing.Point(349, 74);
            this.panelTopCardControl.Name = "panelTopCardControl";
            this.panelTopCardControl.Size = new System.Drawing.Size(149, 184);
            this.panelTopCardControl.TabIndex = 1;
            // 
            // btnDraw
            // 
            this.btnDraw.Location = new System.Drawing.Point(713, 12);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(75, 120);
            this.btnDraw.TabIndex = 0;
            this.btnDraw.Text = "button1";
            this.btnDraw.UseVisualStyleBackColor = true;
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(39, 12);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 22);
            this.txtIP.TabIndex = 2;
            this.txtIP.Text = "127.0.0.1";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(159, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(39, 83);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(304, 91);
            this.panel1.TabIndex = 4;
            // 
            // UNOClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 505);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.btnDraw);
            this.Controls.Add(this.panelTopCardControl);
            this.Controls.Add(this.panelHandControl);
            this.Name = "UNOClient";
            this.Text = "UNOClient";
            this.Load += new System.EventHandler(this.UNOClient_Load);
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
    }
}