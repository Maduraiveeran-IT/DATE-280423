namespace Accounts
{
    partial class FrmSocks_QC_Problem_Master
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
            this.label2 = new System.Windows.Forms.Label();
            this.TxtType = new V_Components.MyTextBox();
            this.TxtCustomerName = new V_Components.MyTextBox();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.TxtType);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtCustomerName);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GBMain.Location = new System.Drawing.Point(12, 13);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(400, 110);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Type";
            // 
            // TxtType
            // 
            this.TxtType.AcceptsReturn = true;
            this.TxtType.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TxtType.Location = new System.Drawing.Point(87, 69);
            this.TxtType.Name = "TxtType";
            this.TxtType.Size = new System.Drawing.Size(264, 21);
            this.TxtType.TabIndex = 3;
            this.TxtType.Tag = "";
            // 
            // TxtCustomerName
            // 
            this.TxtCustomerName.AcceptsReturn = true;
            this.TxtCustomerName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TxtCustomerName.Location = new System.Drawing.Point(87, 31);
            this.TxtCustomerName.Name = "TxtCustomerName";
            this.TxtCustomerName.Size = new System.Drawing.Size(264, 21);
            this.TxtCustomerName.TabIndex = 0;
            this.TxtCustomerName.Tag = "";
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow3.Location = new System.Drawing.Point(363, 68);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(26, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 55;
            this.Arrow3.TabStop = false;
            // 
            // FrmSocks_QC_Problem_Master
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 135);
            this.Controls.Add(this.GBMain);
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "FrmSocks_QC_Problem_Master";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "QC Problem Master...!";
            this.Load += new System.EventHandler(this.FrmSocks_QC_Problem_Master_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmSocks_QC_Problem_Master_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSocks_QC_Problem_Master_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            this.ResumeLayout(false);

        }

      #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtCustomerName;
        private V_Components.MyTextBox TxtType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox Arrow3;
    }
}

