namespace Accounts
{
    partial class FrmMachineStoppageCumulative
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
            this.components = new System.ComponentModel.Container();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.LblSpecial = new System.Windows.Forms.Label();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.Arrow5 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.DtpStartTIme1 = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.DtpStopTime1 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.DtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Arrow4 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtUnit = new V_Components.MyTextBox();
            this.TxtReason = new V_Components.MyTextBox();
            this.TxtDuration = new V_Components.MyTextBox();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.TxtShift = new V_Components.MyTextBox();
            this.TxtNo = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).BeginInit();
            this.SuspendLayout();
            // 
            // LblSpecial
            // 
            this.LblSpecial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.LblSpecial.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSpecial.Location = new System.Drawing.Point(8, 13);
            this.LblSpecial.Name = "LblSpecial";
            this.LblSpecial.Size = new System.Drawing.Size(423, 26);
            this.LblSpecial.TabIndex = 18;
            this.LblSpecial.Text = "MACHINE STOPPAGE";
            this.LblSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.Arrow4);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtUnit);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.label9);
            this.GBMain.Controls.Add(this.Arrow5);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.TxtReason);
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.TxtDuration);
            this.GBMain.Controls.Add(this.DtpStartTIme1);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.DtpStopTime1);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.LblSpecial);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.DtpDate1);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtShift);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtNo);
            this.GBMain.Location = new System.Drawing.Point(9, -2);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(440, 236);
            this.GBMain.TabIndex = 4;
            this.GBMain.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "DATE";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 205);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "REMARKS";
            // 
            // Arrow5
            // 
            this.Arrow5.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow5.Location = new System.Drawing.Point(405, 164);
            this.Arrow5.Name = "Arrow5";
            this.Arrow5.Size = new System.Drawing.Size(26, 21);
            this.Arrow5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow5.TabIndex = 44;
            this.Arrow5.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(212, 167);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "REASON";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 167);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "DURATION";
            // 
            // DtpStartTIme1
            // 
            this.DtpStartTIme1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.DtpStartTIme1.Location = new System.Drawing.Point(287, 127);
            this.DtpStartTIme1.Name = "DtpStartTIme1";
            this.DtpStartTIme1.Size = new System.Drawing.Size(112, 20);
            this.DtpStartTIme1.TabIndex = 5;
            this.DtpStartTIme1.Leave += new System.EventHandler(this.DtpStartTIme1_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(212, 131);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "START TIME";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // DtpStopTime1
            // 
            this.DtpStopTime1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.DtpStopTime1.Location = new System.Drawing.Point(78, 127);
            this.DtpStopTime1.Name = "DtpStopTime1";
            this.DtpStopTime1.Size = new System.Drawing.Size(112, 20);
            this.DtpStopTime1.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "STOP TIME";
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow3.Location = new System.Drawing.Point(405, 94);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(26, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 30;
            this.Arrow3.TabStop = false;
            // 
            // DtpDate1
            // 
            this.DtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate1.Location = new System.Drawing.Point(78, 93);
            this.DtpDate1.Name = "DtpDate1";
            this.DtpDate1.Size = new System.Drawing.Size(112, 20);
            this.DtpDate1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(212, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "SHIFT";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "#NO";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10000;
            // 
            // Arrow4
            // 
            this.Arrow4.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow4.Location = new System.Drawing.Point(405, 58);
            this.Arrow4.Name = "Arrow4";
            this.Arrow4.Size = new System.Drawing.Size(26, 21);
            this.Arrow4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow4.TabIndex = 49;
            this.Arrow4.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(212, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "UNIT";
            // 
            // TxtUnit
            // 
            this.TxtUnit.Location = new System.Drawing.Point(287, 56);
            this.TxtUnit.Name = "TxtUnit";
            this.TxtUnit.Size = new System.Drawing.Size(112, 20);
            this.TxtUnit.TabIndex = 1;
            this.TxtUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtReason
            // 
            this.TxtReason.Location = new System.Drawing.Point(287, 164);
            this.TxtReason.Name = "TxtReason";
            this.TxtReason.Size = new System.Drawing.Size(112, 20);
            this.TxtReason.TabIndex = 7;
            this.TxtReason.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtDuration
            // 
            this.TxtDuration.Location = new System.Drawing.Point(78, 164);
            this.TxtDuration.Name = "TxtDuration";
            this.TxtDuration.Size = new System.Drawing.Size(112, 20);
            this.TxtDuration.TabIndex = 6;
            this.TxtDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(78, 202);
            this.TxtRemarks.Multiline = true;
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(353, 20);
            this.TxtRemarks.TabIndex = 8;
            // 
            // TxtShift
            // 
            this.TxtShift.Location = new System.Drawing.Point(287, 92);
            this.TxtShift.Name = "TxtShift";
            this.TxtShift.Size = new System.Drawing.Size(112, 20);
            this.TxtShift.TabIndex = 3;
            this.TxtShift.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtShift.Leave += new System.EventHandler(this.TxtShift_Leave);
            this.TxtShift.Enter += new System.EventHandler(this.TxtShift_Enter);
            // 
            // TxtNo
            // 
            this.TxtNo.Location = new System.Drawing.Point(78, 56);
            this.TxtNo.Name = "TxtNo";
            this.TxtNo.Size = new System.Drawing.Size(112, 20);
            this.TxtNo.TabIndex = 0;
            this.TxtNo.TabStop = false;
            this.TxtNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmMachineStoppageCumulative
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 239);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmMachineStoppageCumulative";
            this.Text = "MACHINE STOPPAGE CUMULATIVE ENTRY";
            this.Load += new System.EventHandler(this.FrmMachineStoppageCumilative_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmMachineStoppageCumulative_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMachineStoppageCumulative_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Label LblSpecial;
        private System.Windows.Forms.GroupBox GBMain;
        private V_Components.MyTextBox TxtRemarks;
        private System.Windows.Forms.PictureBox Arrow3;
        private System.Windows.Forms.DateTimePicker DtpDate1;
        private System.Windows.Forms.Label label2;
        private V_Components.MyTextBox TxtShift;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtNo;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DateTimePicker DtpStartTIme1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker DtpStopTime1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private V_Components.MyTextBox TxtDuration;
        private System.Windows.Forms.Label label8;
        private V_Components.MyTextBox TxtReason;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox Arrow5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox Arrow4;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtUnit;
    }
}