namespace Accounts
{
    partial class FrmUserMaster
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
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Arrow2 = new System.Windows.Forms.PictureBox();
            this.CmbUserLevel = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.OptN = new System.Windows.Forms.RadioButton();
            this.OptY = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtMailID = new V_Components.MyTextBox();
            this.TxtName = new V_Components.MyTextBox();
            this.TxtLocation = new V_Components.MyTextBox();
            this.TxtRetype = new V_Components.MyTextBox();
            this.TxtUserPass = new V_Components.MyTextBox();
            this.TxtCustomerName = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.TxtMailID);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.TxtName);
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.Arrow2);
            this.GBMain.Controls.Add(this.CmbUserLevel);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.TxtLocation);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.OptN);
            this.GBMain.Controls.Add(this.OptY);
            this.GBMain.Controls.Add(this.TxtRetype);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.TxtUserPass);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtCustomerName);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GBMain.Location = new System.Drawing.Point(12, 13);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(444, 374);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            this.GBMain.Text = "User Details";
            this.GBMain.Enter += new System.EventHandler(this.GBMain_Enter);
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Accounts.Properties.Resources.Down;
            this.Arrow1.Location = new System.Drawing.Point(300, 44);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(25, 21);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 32;
            this.Arrow1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(39, 289);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 17);
            this.label8.TabIndex = 31;
            this.label8.Text = "Mail ID";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 249);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 17);
            this.label7.TabIndex = 29;
            this.label7.Text = "Name";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Arrow2
            // 
            this.Arrow2.Image = global::Accounts.Properties.Resources.Down;
            this.Arrow2.Location = new System.Drawing.Point(407, 208);
            this.Arrow2.Name = "Arrow2";
            this.Arrow2.Size = new System.Drawing.Size(25, 21);
            this.Arrow2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow2.TabIndex = 27;
            this.Arrow2.TabStop = false;
            // 
            // CmbUserLevel
            // 
            this.CmbUserLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbUserLevel.FormattingEnabled = true;
            this.CmbUserLevel.Location = new System.Drawing.Point(137, 166);
            this.CmbUserLevel.Name = "CmbUserLevel";
            this.CmbUserLevel.Size = new System.Drawing.Size(264, 25);
            this.CmbUserLevel.TabIndex = 3;
            this.CmbUserLevel.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 169);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 17);
            this.label6.TabIndex = 16;
            this.label6.Text = "User Level";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "Location";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 335);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "Blocked";
            // 
            // OptN
            // 
            this.OptN.AutoSize = true;
            this.OptN.Location = new System.Drawing.Point(200, 335);
            this.OptN.Name = "OptN";
            this.OptN.Size = new System.Drawing.Size(39, 21);
            this.OptN.TabIndex = 6;
            this.OptN.TabStop = true;
            this.OptN.Text = "N";
            this.OptN.UseVisualStyleBackColor = true;
            // 
            // OptY
            // 
            this.OptY.AutoSize = true;
            this.OptY.Location = new System.Drawing.Point(137, 335);
            this.OptY.Name = "OptY";
            this.OptY.Size = new System.Drawing.Size(38, 21);
            this.OptY.TabIndex = 5;
            this.OptY.TabStop = true;
            this.OptY.Text = "Y";
            this.OptY.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Re-Type";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Password";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "User Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtMailID
            // 
            this.TxtMailID.AcceptsReturn = true;
            this.TxtMailID.Location = new System.Drawing.Point(137, 289);
            this.TxtMailID.Name = "TxtMailID";
            this.TxtMailID.Size = new System.Drawing.Size(264, 24);
            this.TxtMailID.TabIndex = 6;
            this.TxtMailID.Tag = "";
            // 
            // TxtName
            // 
            this.TxtName.AcceptsReturn = true;
            this.TxtName.Location = new System.Drawing.Point(137, 249);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(264, 24);
            this.TxtName.TabIndex = 5;
            this.TxtName.Tag = "";
            // 
            // TxtLocation
            // 
            this.TxtLocation.AcceptsReturn = true;
            this.TxtLocation.Location = new System.Drawing.Point(137, 208);
            this.TxtLocation.Name = "TxtLocation";
            this.TxtLocation.Size = new System.Drawing.Size(264, 24);
            this.TxtLocation.TabIndex = 4;
            this.TxtLocation.Tag = "";
            this.TxtLocation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtLocation_KeyDown);
            this.TxtLocation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtLocation_KeyPress);
            // 
            // TxtRetype
            // 
            this.TxtRetype.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TxtRetype.Location = new System.Drawing.Point(137, 124);
            this.TxtRetype.Name = "TxtRetype";
            this.TxtRetype.PasswordChar = '*';
            this.TxtRetype.Size = new System.Drawing.Size(157, 24);
            this.TxtRetype.TabIndex = 2;
            this.TxtRetype.Tag = "";
            this.TxtRetype.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // TxtUserPass
            // 
            this.TxtUserPass.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TxtUserPass.Location = new System.Drawing.Point(137, 84);
            this.TxtUserPass.Name = "TxtUserPass";
            this.TxtUserPass.PasswordChar = '*';
            this.TxtUserPass.Size = new System.Drawing.Size(157, 24);
            this.TxtUserPass.TabIndex = 1;
            this.TxtUserPass.Tag = "";
            // 
            // TxtCustomerName
            // 
            this.TxtCustomerName.AcceptsReturn = true;
            this.TxtCustomerName.Location = new System.Drawing.Point(137, 44);
            this.TxtCustomerName.Name = "TxtCustomerName";
            this.TxtCustomerName.Size = new System.Drawing.Size(157, 24);
            this.TxtCustomerName.TabIndex = 0;
            this.TxtCustomerName.Tag = "";
            this.TxtCustomerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            
            this.TxtCustomerName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBankName_KeyPress);
            // 
            // FrmUserMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 399);
            this.Controls.Add(this.GBMain);
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "FrmUserMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "User Master";
            this.Load += new System.EventHandler(this.FrmBankMaster_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmBankMaster_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmUserMaster_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).EndInit();
            this.ResumeLayout(false);

        }

      #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton OptY;
        private System.Windows.Forms.RadioButton OptN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CmbUserLevel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox Arrow2;
        private V_Components.MyTextBox TxtCustomerName;
        private V_Components.MyTextBox TxtUserPass;
        private V_Components.MyTextBox TxtRetype;
        private V_Components.MyTextBox TxtLocation;
        private System.Windows.Forms.PictureBox Arrow1;
        private V_Components.MyTextBox TxtMailID;
        private System.Windows.Forms.Label label8;
        private V_Components.MyTextBox TxtName;
        private System.Windows.Forms.Label label7;
    }
}

