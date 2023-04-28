namespace Accounts
{
    partial class FrmModelMaster
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Arrow5 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TxtPair = new V_Components.MyTextBox();
            this.TxtModel = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow5)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.panel1);
            this.GBMain.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GBMain.ForeColor = System.Drawing.Color.Black;
            this.GBMain.Location = new System.Drawing.Point(5, 6);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(337, 129);
            this.GBMain.TabIndex = 1;
            this.GBMain.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "PAIRS\\PACK";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 16);
            this.label2.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "MODEL";
            // 
            // Arrow5
            // 
            this.Arrow5.Image = global::Branch.Properties.Resources.Down;
            this.Arrow5.Location = new System.Drawing.Point(281, 63);
            this.Arrow5.Name = "Arrow5";
            this.Arrow5.Size = new System.Drawing.Size(25, 21);
            this.Arrow5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow5.TabIndex = 36;
            this.Arrow5.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.Arrow5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.TxtPair);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.TxtModel);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.ForeColor = System.Drawing.Color.Red;
            this.panel1.Location = new System.Drawing.Point(6, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(325, 110);
            this.panel1.TabIndex = 37;
            // 
            // TxtPair
            // 
            this.TxtPair.AcceptsReturn = true;
            this.TxtPair.Location = new System.Drawing.Point(80, 63);
            this.TxtPair.Name = "TxtPair";
            this.TxtPair.Size = new System.Drawing.Size(197, 23);
            this.TxtPair.TabIndex = 3;
            this.TxtPair.Tag = "";
            this.TxtPair.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtPair_KeyPress);
            // 
            // TxtModel
            // 
            this.TxtModel.AcceptsReturn = true;
            this.TxtModel.Location = new System.Drawing.Point(80, 19);
            this.TxtModel.Name = "TxtModel";
            this.TxtModel.Size = new System.Drawing.Size(197, 23);
            this.TxtModel.TabIndex = 0;
            this.TxtModel.Tag = "";
            // 
            // FrmModelMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 140);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmModelMaster";
            this.Text = "FrmModelMaster";
            this.Load += new System.EventHandler(this.FrmModelMaster_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmModelMaster_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmModelMaster_KeyPress);
            this.GBMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Arrow5)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private V_Components.MyTextBox TxtModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox TxtPair;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox Arrow5;
        private System.Windows.Forms.Panel panel1;

    }
}