namespace UNO
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panelHandControl = new System.Windows.Forms.Panel();
            this.panelTopCardControl = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelHandControl
            // 
            this.panelHandControl.BackColor = System.Drawing.Color.Transparent;
            this.panelHandControl.Location = new System.Drawing.Point(2, 289);
            this.panelHandControl.Name = "panelHandControl";
            this.panelHandControl.Size = new System.Drawing.Size(768, 137);
            this.panelHandControl.TabIndex = 0;
            // 
            // panelTopCardControl
            // 
            this.panelTopCardControl.BackColor = System.Drawing.Color.Transparent;
            this.panelTopCardControl.Location = new System.Drawing.Point(346, 106);
            this.panelTopCardControl.Name = "panelTopCardControl";
            this.panelTopCardControl.Size = new System.Drawing.Size(110, 147);
            this.panelTopCardControl.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(689, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 119);
            this.button1.TabIndex = 2;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Location = new System.Drawing.Point(307, 243);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(188, 40);
            this.panel1.TabIndex = 3;
            this.panel1.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(782, 461);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panelHandControl);
            this.Controls.Add(this.panelTopCardControl);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHandControl;
        private System.Windows.Forms.Panel panelTopCardControl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
    }
}

