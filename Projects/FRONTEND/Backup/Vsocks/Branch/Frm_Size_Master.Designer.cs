namespace Accounts
{
    partial class Frm_Size_Master
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
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtName = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtName);
            this.GBMain.Location = new System.Drawing.Point(11, 0);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(377, 149);
            this.GBMain.TabIndex = 2;
            this.GBMain.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "SIZE";
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(145, 50);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(137, 20);
            this.TxtName.TabIndex = 2;
            // 
            // Frm_Size_Master
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 154);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "Frm_Size_Master";
            this.Text = "Frm_Size_Master";
            this.Load += new System.EventHandler(this.Frm_Size_Master_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Size_Master_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtName;

    }
}