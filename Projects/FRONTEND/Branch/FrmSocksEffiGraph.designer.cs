namespace Accounts
{
    partial class FrmSocksEffiGraph
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
            this.process1 = new System.Diagnostics.Process();
            this.GraphPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // process1
            // 
            this.process1.StartInfo.Domain = "";
            this.process1.StartInfo.LoadUserProfile = false;
            this.process1.StartInfo.Password = null;
            this.process1.StartInfo.StandardErrorEncoding = null;
            this.process1.StartInfo.StandardOutputEncoding = null;
            this.process1.StartInfo.UserName = "";
            this.process1.SynchronizingObject = this;
            // 
            // GraphPanel
            // 
            this.GraphPanel.BackColor = System.Drawing.SystemColors.HighlightText;
            this.GraphPanel.Location = new System.Drawing.Point(20, 12);
            this.GraphPanel.Name = "GraphPanel";
            this.GraphPanel.Size = new System.Drawing.Size(110, 54);
            this.GraphPanel.TabIndex = 1;
            this.GraphPanel.Visible = false;
            // 
            // FrmSocksEffiGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 281);
            this.Controls.Add(this.GraphPanel);
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "FrmSocksEffiGraph";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SOCKS EFFI GRAPH";
            this.Load += new System.EventHandler(this.FrmSocksEffiGraph_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSocksEffiGraph_KeyDown);
            this.ResumeLayout(false);

        }

      #endregion

        private System.Diagnostics.Process process1;
        private System.Windows.Forms.Panel GraphPanel;

    }
}

