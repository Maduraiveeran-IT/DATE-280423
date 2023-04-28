namespace Accounts
{
    partial class FrmSampleCycleTimeChangeEntry
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
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.TxtYear = new V_Components.MyTextBox();
            this.TxtWeek = new V_Components.MyTextBox();
            this.TxtNo = new V_Components.MyTextBox();
            this.TxtTotal = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtShift = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtYear);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.TxtWeek);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtNo);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.TxtShift);
            this.GBMain.Controls.Add(this.DtpDate1);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(3, 3);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(699, 420);
            this.GBMain.TabIndex = 1;
            this.GBMain.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(193, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 67;
            this.label5.Text = "YEAR";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 65;
            this.label3.Text = "WEEK";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 63;
            this.label2.Text = "ENTRY NO";
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Branch.Properties.Resources.Down;
            this.Arrow1.Location = new System.Drawing.Point(666, 20);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(25, 18);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 61;
            this.Arrow1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(494, 390);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 59;
            this.label8.Text = "COUNT";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(424, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "EFFECT FROM SHIFT";
            // 
            // DtpDate1
            // 
            this.DtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate1.Location = new System.Drawing.Point(312, 19);
            this.DtpDate1.Name = "DtpDate1";
            this.DtpDate1.Size = new System.Drawing.Size(84, 20);
            this.DtpDate1.TabIndex = 0;
            this.DtpDate1.ValueChanged += new System.EventHandler(this.DtpDate1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(191, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "EFFECT FROM DATE";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 394);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 69;
            this.label7.Text = "REMARKS";
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(110, 387);
            this.TxtRemarks.Multiline = true;
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(312, 20);
            this.TxtRemarks.TabIndex = 5;
            // 
            // TxtYear
            // 
            this.TxtYear.Enabled = false;
            this.TxtYear.Location = new System.Drawing.Point(312, 50);
            this.TxtYear.Name = "TxtYear";
            this.TxtYear.Size = new System.Drawing.Size(84, 20);
            this.TxtYear.TabIndex = 66;
            this.TxtYear.TabStop = false;
            this.TxtYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtWeek
            // 
            this.TxtWeek.Enabled = false;
            this.TxtWeek.Location = new System.Drawing.Point(85, 50);
            this.TxtWeek.Name = "TxtWeek";
            this.TxtWeek.Size = new System.Drawing.Size(79, 20);
            this.TxtWeek.TabIndex = 64;
            this.TxtWeek.TabStop = false;
            this.TxtWeek.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtNo
            // 
            this.TxtNo.Enabled = false;
            this.TxtNo.Location = new System.Drawing.Point(85, 19);
            this.TxtNo.Name = "TxtNo";
            this.TxtNo.Size = new System.Drawing.Size(79, 20);
            this.TxtNo.TabIndex = 62;
            this.TxtNo.TabStop = false;
            this.TxtNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(545, 387);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(147, 20);
            this.TxtTotal.TabIndex = 6;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(14, 76);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(678, 305);
            this.Grid.TabIndex = 4;
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellEnter);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // TxtShift
            // 
            this.TxtShift.Location = new System.Drawing.Point(545, 19);
            this.TxtShift.Name = "TxtShift";
            this.TxtShift.Size = new System.Drawing.Size(115, 20);
            this.TxtShift.TabIndex = 3;
            this.TxtShift.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmSampleCycleTimeChangeEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 427);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmSampleCycleTimeChangeEntry";
            this.Text = "FrmSampleCycleTimeChangeEntry";
            this.Load += new System.EventHandler(this.FrmSampleCycleTimeChangeEntry_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmSampleCycleTimeChangeEntry_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSampleCycleTimeChangeEntry_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.PictureBox Arrow1;
        private System.Windows.Forms.Label label8;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker DtpDate1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private V_Components.MyTextBox TxtNo;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtYear;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox TxtWeek;
        private V_Components.MyTextBox TxtShift;
        private System.Windows.Forms.Label label7;
        private V_Components.MyTextBox TxtRemarks;
        private V_Components.MyTextBox TxtTotal;
    }
}