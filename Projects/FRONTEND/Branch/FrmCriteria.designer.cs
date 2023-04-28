namespace Accounts
{
    partial class FrmCriteria
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
            this.GBCriteria = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PLText = new System.Windows.Forms.Panel();
            this.TxtText = new V_Components.MyTextBox();
            this.TxtConditions = new V_Components.MyTextBox();
            this.PLDate = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.DtpTo = new System.Windows.Forms.DateTimePicker();
            this.DtpFrom = new System.Windows.Forms.DateTimePicker();
            this.OptDesc = new System.Windows.Forms.RadioButton();
            this.OptAsc = new System.Windows.Forms.RadioButton();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.PLFromTo = new System.Windows.Forms.Panel();
            this.TxtTo = new V_Components.MyTextBox();
            this.TxtFrom = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.CmbText = new System.Windows.Forms.ComboBox();
            this.CmbOrder = new System.Windows.Forms.ComboBox();
            this.CmbField = new System.Windows.Forms.ComboBox();
            this.GBCriteria.SuspendLayout();
            this.PLText.SuspendLayout();
            this.PLDate.SuspendLayout();
            this.PLFromTo.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBCriteria
            // 
            this.GBCriteria.Controls.Add(this.label6);
            this.GBCriteria.Controls.Add(this.label5);
            this.GBCriteria.Controls.Add(this.label3);
            this.GBCriteria.Controls.Add(this.PLText);
            this.GBCriteria.Controls.Add(this.TxtConditions);
            this.GBCriteria.Controls.Add(this.PLDate);
            this.GBCriteria.Controls.Add(this.OptDesc);
            this.GBCriteria.Controls.Add(this.OptAsc);
            this.GBCriteria.Controls.Add(this.button4);
            this.GBCriteria.Controls.Add(this.button3);
            this.GBCriteria.Controls.Add(this.button2);
            this.GBCriteria.Controls.Add(this.PLFromTo);
            this.GBCriteria.Controls.Add(this.button1);
            this.GBCriteria.Controls.Add(this.CmbText);
            this.GBCriteria.Controls.Add(this.CmbOrder);
            this.GBCriteria.Controls.Add(this.CmbField);
            this.GBCriteria.Location = new System.Drawing.Point(8, 7);
            this.GBCriteria.Name = "GBCriteria";
            this.GBCriteria.Size = new System.Drawing.Size(643, 230);
            this.GBCriteria.TabIndex = 0;
            this.GBCriteria.TabStop = false;
            this.GBCriteria.Text = "Criteria";
            this.GBCriteria.Enter += new System.EventHandler(this.GBCriteria_Enter);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(464, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Value";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(240, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Condition";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Field";
            // 
            // PLText
            // 
            this.PLText.Controls.Add(this.TxtText);
            this.PLText.Location = new System.Drawing.Point(362, 57);
            this.PLText.Name = "PLText";
            this.PLText.Size = new System.Drawing.Size(277, 27);
            this.PLText.TabIndex = 2;
            // 
            // TxtText
            // 
            this.TxtText.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TxtText.Location = new System.Drawing.Point(5, 4);
            this.TxtText.Name = "TxtText";
            this.TxtText.Size = new System.Drawing.Size(263, 21);
            this.TxtText.TabIndex = 0;
            this.TxtText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtText_KeyPress);
            // 
            // TxtConditions
            // 
            this.TxtConditions.Location = new System.Drawing.Point(15, 101);
            this.TxtConditions.Multiline = true;
            this.TxtConditions.Name = "TxtConditions";
            this.TxtConditions.Size = new System.Drawing.Size(341, 112);
            this.TxtConditions.TabIndex = 5;
            // 
            // PLDate
            // 
            this.PLDate.Controls.Add(this.label1);
            this.PLDate.Controls.Add(this.DtpTo);
            this.PLDate.Controls.Add(this.DtpFrom);
            this.PLDate.Location = new System.Drawing.Point(362, 57);
            this.PLDate.Name = "PLDate";
            this.PLDate.Size = new System.Drawing.Size(277, 27);
            this.PLDate.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(124, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "   -";
            // 
            // DtpTo
            // 
            this.DtpTo.CustomFormat = "dd/MM/yyyy";
            this.DtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpTo.Location = new System.Drawing.Point(155, 3);
            this.DtpTo.Name = "DtpTo";
            this.DtpTo.Size = new System.Drawing.Size(113, 21);
            this.DtpTo.TabIndex = 1;
            // 
            // DtpFrom
            // 
            this.DtpFrom.CustomFormat = "dd/MM/yyyy";
            this.DtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpFrom.Location = new System.Drawing.Point(5, 3);
            this.DtpFrom.Name = "DtpFrom";
            this.DtpFrom.Size = new System.Drawing.Size(113, 21);
            this.DtpFrom.TabIndex = 0;
            // 
            // OptDesc
            // 
            this.OptDesc.AutoSize = true;
            this.OptDesc.Location = new System.Drawing.Point(427, 147);
            this.OptDesc.Name = "OptDesc";
            this.OptDesc.Size = new System.Drawing.Size(56, 17);
            this.OptDesc.TabIndex = 4;
            this.OptDesc.TabStop = true;
            this.OptDesc.Text = "&Desc";
            this.OptDesc.UseVisualStyleBackColor = true;
            this.OptDesc.Visible = false;
            // 
            // OptAsc
            // 
            this.OptAsc.AutoSize = true;
            this.OptAsc.Location = new System.Drawing.Point(373, 147);
            this.OptAsc.Name = "OptAsc";
            this.OptAsc.Size = new System.Drawing.Size(48, 17);
            this.OptAsc.TabIndex = 4;
            this.OptAsc.TabStop = true;
            this.OptAsc.Text = "&Asc";
            this.OptAsc.UseVisualStyleBackColor = true;
            this.OptAsc.Visible = false;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(467, 184);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(80, 29);
            this.button4.TabIndex = 7;
            this.button4.Text = "C&lear";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(553, 184);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 29);
            this.button3.TabIndex = 7;
            this.button3.Text = "&Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(370, 184);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 29);
            this.button2.TabIndex = 6;
            this.button2.Text = "&Load";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // PLFromTo
            // 
            this.PLFromTo.Controls.Add(this.TxtTo);
            this.PLFromTo.Controls.Add(this.TxtFrom);
            this.PLFromTo.Controls.Add(this.label2);
            this.PLFromTo.Location = new System.Drawing.Point(362, 57);
            this.PLFromTo.Name = "PLFromTo";
            this.PLFromTo.Size = new System.Drawing.Size(277, 27);
            this.PLFromTo.TabIndex = 3;
            // 
            // TxtTo
            // 
            this.TxtTo.Location = new System.Drawing.Point(155, 3);
            this.TxtTo.Name = "TxtTo";
            this.TxtTo.Size = new System.Drawing.Size(113, 21);
            this.TxtTo.TabIndex = 1;
            this.TxtTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtTo_KeyPress);
            // 
            // TxtFrom
            // 
            this.TxtFrom.Location = new System.Drawing.Point(5, 3);
            this.TxtFrom.Name = "TxtFrom";
            this.TxtFrom.Size = new System.Drawing.Size(113, 21);
            this.TxtFrom.TabIndex = 0;
            this.TxtFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtFrom_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(124, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "   -";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(591, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 25);
            this.button1.TabIndex = 5;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CmbText
            // 
            this.CmbText.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbText.FormattingEnabled = true;
            this.CmbText.Location = new System.Drawing.Point(195, 63);
            this.CmbText.Name = "CmbText";
            this.CmbText.Size = new System.Drawing.Size(161, 21);
            this.CmbText.TabIndex = 1;
            this.CmbText.SelectedIndexChanged += new System.EventHandler(this.CmbText_SelectedIndexChanged);
            // 
            // CmbOrder
            // 
            this.CmbOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbOrder.FormattingEnabled = true;
            this.CmbOrder.Location = new System.Drawing.Point(489, 147);
            this.CmbOrder.Name = "CmbOrder";
            this.CmbOrder.Size = new System.Drawing.Size(141, 21);
            this.CmbOrder.TabIndex = 40;
            this.CmbOrder.Visible = false;
            // 
            // CmbField
            // 
            this.CmbField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbField.FormattingEnabled = true;
            this.CmbField.Location = new System.Drawing.Point(15, 63);
            this.CmbField.Name = "CmbField";
            this.CmbField.Size = new System.Drawing.Size(174, 21);
            this.CmbField.TabIndex = 0;
            this.CmbField.SelectedIndexChanged += new System.EventHandler(this.CmbField_SelectedIndexChanged);
            // 
            // FrmCriteria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 245);
            this.Controls.Add(this.GBCriteria);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCriteria";
            this.Text = "FrmCriteria";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmCriteria_KeyDown);
            this.Load += new System.EventHandler(this.FrmCriteria_Load);
            this.GBCriteria.ResumeLayout(false);
            this.GBCriteria.PerformLayout();
            this.PLText.ResumeLayout(false);
            this.PLText.PerformLayout();
            this.PLDate.ResumeLayout(false);
            this.PLDate.PerformLayout();
            this.PLFromTo.ResumeLayout(false);
            this.PLFromTo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBCriteria;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox CmbField;
        private System.Windows.Forms.ComboBox CmbText;
        private System.Windows.Forms.Panel PLFromTo;
        private System.Windows.Forms.Panel PLDate;
        private System.Windows.Forms.DateTimePicker DtpFrom;
        private System.Windows.Forms.Panel PLText;
        private System.Windows.Forms.TextBox TxtText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker DtpTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtTo;
        private System.Windows.Forms.TextBox TxtFrom;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RadioButton OptDesc;
        private System.Windows.Forms.RadioButton OptAsc;
        private System.Windows.Forms.TextBox TxtConditions;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CmbOrder;
    }
}