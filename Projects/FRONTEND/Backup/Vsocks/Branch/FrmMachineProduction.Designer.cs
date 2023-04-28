namespace Accounts
{
    partial class FrmMachineProduction
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ChkImport = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CmbTNeedle = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.CmbFNeedle = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.BtnImportExit = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.BtnImportCancel = new System.Windows.Forms.Button();
            this.BtnImportOk = new System.Windows.Forms.Button();
            this.CmbTShift = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.DtpTDate = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.CmbFShift = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.DtpFDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.DtpEDate = new System.Windows.Forms.DateTimePicker();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Grid = new System.Windows.Forms.DataGridView();
            this.CmbNeedle = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CmbShift = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtYear = new V_Components.MyTextBox();
            this.TxtWeek = new V_Components.MyTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ChkImport);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.TxtYear);
            this.groupBox1.Controls.Add(this.TxtWeek);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.DtpEDate);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.Grid);
            this.groupBox1.Controls.Add(this.CmbNeedle);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.CmbShift);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(8, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(865, 513);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // ChkImport
            // 
            this.ChkImport.AutoSize = true;
            this.ChkImport.Location = new System.Drawing.Point(141, 478);
            this.ChkImport.Name = "ChkImport";
            this.ChkImport.Size = new System.Drawing.Size(71, 17);
            this.ChkImport.TabIndex = 12;
            this.ChkImport.Text = "IMPORT";
            this.ChkImport.UseVisualStyleBackColor = true;
            this.ChkImport.CheckedChanged += new System.EventHandler(this.ChkImport_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.groupBox2.Controls.Add(this.CmbTNeedle);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.CmbFNeedle);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.BtnImportExit);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.BtnImportCancel);
            this.groupBox2.Controls.Add(this.BtnImportOk);
            this.groupBox2.Controls.Add(this.CmbTShift);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.DtpTDate);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.CmbFShift);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.DtpFDate);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(247, 193);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(458, 154);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            // 
            // CmbTNeedle
            // 
            this.CmbTNeedle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbTNeedle.FormattingEnabled = true;
            this.CmbTNeedle.Location = new System.Drawing.Point(352, 85);
            this.CmbTNeedle.Name = "CmbTNeedle";
            this.CmbTNeedle.Size = new System.Drawing.Size(96, 21);
            this.CmbTNeedle.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(290, 89);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(51, 13);
            this.label13.TabIndex = 21;
            this.label13.Text = "NEEDLE";
            // 
            // CmbFNeedle
            // 
            this.CmbFNeedle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbFNeedle.FormattingEnabled = true;
            this.CmbFNeedle.Location = new System.Drawing.Point(352, 37);
            this.CmbFNeedle.Name = "CmbFNeedle";
            this.CmbFNeedle.Size = new System.Drawing.Size(96, 21);
            this.CmbFNeedle.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(290, 41);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 13);
            this.label12.TabIndex = 14;
            this.label12.Text = "NEEDLE";
            // 
            // BtnImportExit
            // 
            this.BtnImportExit.Location = new System.Drawing.Point(289, 122);
            this.BtnImportExit.Name = "BtnImportExit";
            this.BtnImportExit.Size = new System.Drawing.Size(75, 26);
            this.BtnImportExit.TabIndex = 8;
            this.BtnImportExit.Text = "EX&IT";
            this.BtnImportExit.UseVisualStyleBackColor = true;
            this.BtnImportExit.Click += new System.EventHandler(this.BtnImportExit_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Verdana", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(7, 65);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "TO DATE && SHIFT";
            // 
            // BtnImportCancel
            // 
            this.BtnImportCancel.Location = new System.Drawing.Point(208, 122);
            this.BtnImportCancel.Name = "BtnImportCancel";
            this.BtnImportCancel.Size = new System.Drawing.Size(75, 26);
            this.BtnImportCancel.TabIndex = 7;
            this.BtnImportCancel.Text = "CL&EAR";
            this.BtnImportCancel.UseVisualStyleBackColor = true;
            this.BtnImportCancel.Click += new System.EventHandler(this.BtnImportCancel_Click);
            // 
            // BtnImportOk
            // 
            this.BtnImportOk.Location = new System.Drawing.Point(127, 122);
            this.BtnImportOk.Name = "BtnImportOk";
            this.BtnImportOk.Size = new System.Drawing.Size(75, 26);
            this.BtnImportOk.TabIndex = 6;
            this.BtnImportOk.Text = "O&K";
            this.BtnImportOk.UseVisualStyleBackColor = true;
            this.BtnImportOk.Click += new System.EventHandler(this.BtnImportOk_Click);
            // 
            // CmbTShift
            // 
            this.CmbTShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbTShift.FormattingEnabled = true;
            this.CmbTShift.Location = new System.Drawing.Point(211, 84);
            this.CmbTShift.Name = "CmbTShift";
            this.CmbTShift.Size = new System.Drawing.Size(58, 21);
            this.CmbTShift.TabIndex = 4;
            this.CmbTShift.Leave += new System.EventHandler(this.CmbTShift_Leave);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(159, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "SHIFT";
            // 
            // DtpTDate
            // 
            this.DtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpTDate.Location = new System.Drawing.Point(62, 84);
            this.DtpTDate.Name = "DtpTDate";
            this.DtpTDate.Size = new System.Drawing.Size(85, 21);
            this.DtpTDate.TabIndex = 3;
            this.DtpTDate.Leave += new System.EventHandler(this.DtpTDate_Leave);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 88);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(37, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "DATE";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Verdana", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(133, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "FROM DATE && SHIFT";
            // 
            // CmbFShift
            // 
            this.CmbFShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbFShift.FormattingEnabled = true;
            this.CmbFShift.Location = new System.Drawing.Point(211, 36);
            this.CmbFShift.Name = "CmbFShift";
            this.CmbFShift.Size = new System.Drawing.Size(58, 21);
            this.CmbFShift.TabIndex = 1;
            this.CmbFShift.SelectedIndexChanged += new System.EventHandler(this.CmbFShift_SelectedIndexChanged);
            this.CmbFShift.Leave += new System.EventHandler(this.CmbFShift_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(159, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "SHIFT";
            // 
            // DtpFDate
            // 
            this.DtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpFDate.Location = new System.Drawing.Point(62, 36);
            this.DtpFDate.Name = "DtpFDate";
            this.DtpFDate.Size = new System.Drawing.Size(85, 21);
            this.DtpFDate.TabIndex = 0;
            this.DtpFDate.Leave += new System.EventHandler(this.DtpFDate_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "DATE";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(6, 469);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 34);
            this.button4.TabIndex = 5;
            this.button4.Text = "&LOAD";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // DtpEDate
            // 
            this.DtpEDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpEDate.Location = new System.Drawing.Point(64, 24);
            this.DtpEDate.Name = "DtpEDate";
            this.DtpEDate.Size = new System.Drawing.Size(85, 21);
            this.DtpEDate.TabIndex = 0;
            this.DtpEDate.Leave += new System.EventHandler(this.DtpEDate_Leave);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(782, 469);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 34);
            this.button3.TabIndex = 9;
            this.button3.Text = "E&XIT";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(701, 469);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 34);
            this.button2.TabIndex = 8;
            this.button2.Text = "&CANCEL";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(620, 469);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 34);
            this.button1.TabIndex = 7;
            this.button1.Text = "&OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(7, 58);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid.Size = new System.Drawing.Size(849, 402);
            this.Grid.TabIndex = 6;
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // CmbNeedle
            // 
            this.CmbNeedle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbNeedle.FormattingEnabled = true;
            this.CmbNeedle.Location = new System.Drawing.Point(760, 25);
            this.CmbNeedle.Name = "CmbNeedle";
            this.CmbNeedle.Size = new System.Drawing.Size(96, 21);
            this.CmbNeedle.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(688, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "NEEDLE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(360, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "WEEK";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(202, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "YEAR";
            // 
            // CmbShift
            // 
            this.CmbShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbShift.FormattingEnabled = true;
            this.CmbShift.Location = new System.Drawing.Point(598, 25);
            this.CmbShift.Name = "CmbShift";
            this.CmbShift.Size = new System.Drawing.Size(58, 21);
            this.CmbShift.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(549, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SHIFT";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "DATE";
            // 
            // TxtYear
            // 
            this.TxtYear.Location = new System.Drawing.Point(247, 25);
            this.TxtYear.Name = "TxtYear";
            this.TxtYear.Size = new System.Drawing.Size(79, 21);
            this.TxtYear.TabIndex = 1;
            this.TxtYear.TabStop = false;
            this.TxtYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtWeek
            // 
            this.TxtWeek.Location = new System.Drawing.Point(419, 25);
            this.TxtWeek.Name = "TxtWeek";
            this.TxtWeek.Size = new System.Drawing.Size(55, 21);
            this.TxtWeek.TabIndex = 2;
            this.TxtWeek.TabStop = false;
            this.TxtWeek.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmMachineProduction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 519);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "FrmMachineProduction";
            this.Text = "MACHINE PRODUCTION ...!";
            this.Load += new System.EventHandler(this.FrmMachineProduction_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmMachineProduction_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMachineProduction_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CmbShift;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView Grid;
        private System.Windows.Forms.ComboBox CmbNeedle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DateTimePicker DtpEDate;
        private System.Windows.Forms.Button button4;
        private V_Components.MyTextBox TxtYear;
        private V_Components.MyTextBox TxtWeek;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button BtnImportExit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button BtnImportCancel;
        private System.Windows.Forms.Button BtnImportOk;
        private System.Windows.Forms.ComboBox CmbTShift;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker DtpTDate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox CmbFShift;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker DtpFDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox ChkImport;
        private System.Windows.Forms.ComboBox CmbFNeedle;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox CmbTNeedle;
        private System.Windows.Forms.Label label13;
    }
}