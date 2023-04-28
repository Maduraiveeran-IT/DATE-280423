namespace Accounts
{
    partial class Frm_Style_Master
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
            this.label1 = new System.Windows.Forms.Label();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.TxtName = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "STYLE";
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtName);
            this.GBMain.Location = new System.Drawing.Point(8, -1);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(283, 81);
            this.GBMain.TabIndex = 3;
            this.GBMain.TabStop = false;
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(111, 31);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(137, 20);
            this.TxtName.TabIndex = 2;
            this.TxtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Style_Master_KeyDown);
            this.TxtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtName_KeyPress);
            // 
            // Frm_Style_Master
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 90);
            this.Controls.Add(this.GBMain);
            this.Name = "Frm_Style_Master";
            this.Text = "Frm_Style_Master";
            this.Load += new System.EventHandler(this.Frm_Style_Master_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Style_Master_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private V_Components.MyTextBox TxtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox GBMain;

    }
}