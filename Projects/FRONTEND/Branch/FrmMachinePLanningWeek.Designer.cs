namespace Accounts
{
    partial class FrmMachinePLanningWeek
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
            this.ToBePlanned = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.LblCySecds = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LblFreeMins = new System.Windows.Forms.Label();
            this.LblPlanMins = new System.Windows.Forms.Label();
            this.LblTotalMins = new System.Windows.Forms.Label();
            this.LblEfficiency = new V_Components.MyTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LblWeekDays = new System.Windows.Forms.Label();
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtNeedle = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtYear = new V_Components.MyTextBox();
            this.TxtWeek = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DtpEDate = new System.Windows.Forms.DateTimePicker();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.DtpEDate);
            this.GBMain.Controls.Add(this.ToBePlanned);
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.LblCySecds);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.LblFreeMins);
            this.GBMain.Controls.Add(this.LblPlanMins);
            this.GBMain.Controls.Add(this.LblTotalMins);
            this.GBMain.Controls.Add(this.LblEfficiency);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.LblWeekDays);
            this.GBMain.Controls.Add(this.BtnExit);
            this.GBMain.Controls.Add(this.BtnOk);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.TxtNeedle);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtYear);
            this.GBMain.Controls.Add(this.TxtWeek);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(7, 0);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(926, 427);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            this.GBMain.Enter += new System.EventHandler(this.GBMain_Enter);
            // 
            // ToBePlanned
            // 
            this.ToBePlanned.BackColor = System.Drawing.SystemColors.Control;
            this.ToBePlanned.Location = new System.Drawing.Point(661, 24);
            this.ToBePlanned.Name = "ToBePlanned";
            this.ToBePlanned.Size = new System.Drawing.Size(87, 21);
            this.ToBePlanned.TabIndex = 17;
            this.ToBePlanned.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(545, 399);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "CY SECS";
            // 
            // LblCySecds
            // 
            this.LblCySecds.BackColor = System.Drawing.SystemColors.Control;
            this.LblCySecds.Location = new System.Drawing.Point(634, 395);
            this.LblCySecds.Name = "LblCySecds";
            this.LblCySecds.Size = new System.Drawing.Size(87, 21);
            this.LblCySecds.TabIndex = 15;
            this.LblCySecds.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(362, 399);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "FREE SECS";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(189, 399);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "PLAN SECS";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 399);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "TOT SECS";
            // 
            // LblFreeMins
            // 
            this.LblFreeMins.BackColor = System.Drawing.SystemColors.Control;
            this.LblFreeMins.Location = new System.Drawing.Point(444, 395);
            this.LblFreeMins.Name = "LblFreeMins";
            this.LblFreeMins.Size = new System.Drawing.Size(87, 21);
            this.LblFreeMins.TabIndex = 11;
            this.LblFreeMins.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblPlanMins
            // 
            this.LblPlanMins.BackColor = System.Drawing.SystemColors.Control;
            this.LblPlanMins.Location = new System.Drawing.Point(268, 395);
            this.LblPlanMins.Name = "LblPlanMins";
            this.LblPlanMins.Size = new System.Drawing.Size(87, 21);
            this.LblPlanMins.TabIndex = 10;
            this.LblPlanMins.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblTotalMins
            // 
            this.LblTotalMins.BackColor = System.Drawing.SystemColors.Control;
            this.LblTotalMins.Location = new System.Drawing.Point(90, 395);
            this.LblTotalMins.Name = "LblTotalMins";
            this.LblTotalMins.Size = new System.Drawing.Size(87, 21);
            this.LblTotalMins.TabIndex = 9;
            this.LblTotalMins.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblEfficiency
            // 
            this.LblEfficiency.Location = new System.Drawing.Point(835, 25);
            this.LblEfficiency.Name = "LblEfficiency";
            this.LblEfficiency.Size = new System.Drawing.Size(84, 21);
            this.LblEfficiency.TabIndex = 4;
            this.LblEfficiency.TabStop = false;
            this.LblEfficiency.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Yellow;
            this.label3.Location = new System.Drawing.Point(762, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "PD. SECS";
            // 
            // LblWeekDays
            // 
            this.LblWeekDays.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.LblWeekDays.Location = new System.Drawing.Point(198, 25);
            this.LblWeekDays.Name = "LblWeekDays";
            this.LblWeekDays.Size = new System.Drawing.Size(233, 21);
            this.LblWeekDays.TabIndex = 7;
            this.LblWeekDays.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnExit
            // 
            this.BtnExit.Location = new System.Drawing.Point(833, 385);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(86, 34);
            this.BtnExit.TabIndex = 6;
            this.BtnExit.Text = "E&XIT";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(741, 385);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(86, 34);
            this.BtnOk.TabIndex = 5;
            this.BtnOk.Text = "&OK";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(7, 60);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(912, 316);
            this.Grid.TabIndex = 3;
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.CurrentCellChanged += new System.EventHandler(this.Grid_CurrentCellChanged);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.Click += new System.EventHandler(this.Grid_Click);
            // 
            // TxtNeedle
            // 
            this.TxtNeedle.Location = new System.Drawing.Point(530, 25);
            this.TxtNeedle.Name = "TxtNeedle";
            this.TxtNeedle.Size = new System.Drawing.Size(108, 21);
            this.TxtNeedle.TabIndex = 2;
            this.TxtNeedle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(459, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "NEEDLE";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // TxtYear
            // 
            this.TxtYear.Location = new System.Drawing.Point(70, 25);
            this.TxtYear.Name = "TxtYear";
            this.TxtYear.Size = new System.Drawing.Size(62, 21);
            this.TxtYear.TabIndex = 0;
            this.TxtYear.TabStop = false;
            this.TxtYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtWeek
            // 
            this.TxtWeek.Location = new System.Drawing.Point(138, 25);
            this.TxtWeek.Name = "TxtWeek";
            this.TxtWeek.Size = new System.Drawing.Size(44, 21);
            this.TxtWeek.TabIndex = 1;
            this.TxtWeek.TabStop = false;
            this.TxtWeek.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "WEEK";
            // 
            // DtpEDate
            // 
            this.DtpEDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpEDate.Location = new System.Drawing.Point(396, 25);
            this.DtpEDate.Name = "DtpEDate";
            this.DtpEDate.Size = new System.Drawing.Size(57, 21);
            this.DtpEDate.TabIndex = 77;
            this.DtpEDate.Visible = false;
            // 
            // FrmMachinePLanningWeek
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 430);
            this.ControlBox = false;
            this.Controls.Add(this.GBMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "FrmMachinePLanningWeek";
            this.Text = "WEEK & MACHINE DETAILS ...!";
            this.Load += new System.EventHandler(this.FrmMachinePLanningWeek_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmMachinePLanningWeek_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMachinePLanningWeek_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private V_Components.MyTextBox TxtWeek;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtNeedle;
        private System.Windows.Forms.Label label2;
        private V_Components.MyTextBox TxtYear;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Label LblWeekDays;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox LblEfficiency;
        private System.Windows.Forms.Label LblTotalMins;
        private System.Windows.Forms.Label LblFreeMins;
        private System.Windows.Forms.Label LblPlanMins;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label LblCySecds;
        private System.Windows.Forms.Label ToBePlanned;
        private System.Windows.Forms.DateTimePicker DtpEDate;
    }
}