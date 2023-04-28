namespace Accounts
{
    partial class FrmProductionEdit
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
            this.DtpFromTime = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtEno = new V_Components.MyTextBox();
            this.TxtType = new V_Components.MyTextBox();
            this.TxtShift = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtEmployees = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.DtpToTime = new System.Windows.Forms.DateTimePicker();
            this.TxtDivision = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            this.SuspendLayout();
            // 
            // DtpFromTime
            // 
            this.DtpFromTime.CustomFormat = "dd/MM/yyyy hh:mm tt";
            this.DtpFromTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpFromTime.Location = new System.Drawing.Point(540, 48);
            this.DtpFromTime.Name = "DtpFromTime";
            this.DtpFromTime.Size = new System.Drawing.Size(138, 20);
            this.DtpFromTime.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(357, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "SHIFT";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "TYPE";
            // 
            // TxtEno
            // 
            this.TxtEno.Enabled = false;
            this.TxtEno.Location = new System.Drawing.Point(150, 22);
            this.TxtEno.Name = "TxtEno";
            this.TxtEno.Size = new System.Drawing.Size(79, 20);
            this.TxtEno.TabIndex = 2;
            this.TxtEno.TabStop = false;
            this.TxtEno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtType
            // 
            this.TxtType.Location = new System.Drawing.Point(60, 47);
            this.TxtType.Name = "TxtType";
            this.TxtType.Size = new System.Drawing.Size(138, 20);
            this.TxtType.TabIndex = 2;
            this.TxtType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtShift
            // 
            this.TxtShift.Location = new System.Drawing.Point(427, 47);
            this.TxtShift.Name = "TxtShift";
            this.TxtShift.Size = new System.Drawing.Size(76, 20);
            this.TxtShift.TabIndex = 3;
            this.TxtShift.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(357, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "DIVISION";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // DtpDate1
            // 
            this.DtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate1.Location = new System.Drawing.Point(60, 22);
            this.DtpDate1.Name = "DtpDate1";
            this.DtpDate1.Size = new System.Drawing.Size(84, 20);
            this.DtpDate1.TabIndex = 0;
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.pictureBox1);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.TxtEmployees);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.DtpToTime);
            this.GBMain.Controls.Add(this.DtpFromTime);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.TxtEno);
            this.GBMain.Controls.Add(this.TxtType);
            this.GBMain.Controls.Add(this.TxtShift);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.DtpDate1);
            this.GBMain.Controls.Add(this.TxtDivision);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(3, 3);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(888, 429);
            this.GBMain.TabIndex = 1;
            this.GBMain.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(598, 401);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 13);
            this.label8.TabIndex = 59;
            this.label8.Text = "EMPLOYEES / QTY";
            // 
            // TxtEmployees
            // 
            this.TxtEmployees.Location = new System.Drawing.Point(736, 398);
            this.TxtEmployees.Name = "TxtEmployees";
            this.TxtEmployees.Size = new System.Drawing.Size(143, 20);
            this.TxtEmployees.TabIndex = 8;
            this.TxtEmployees.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(14, 76);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(865, 305);
            this.Grid.TabIndex = 6;
            // 
            // DtpToTime
            // 
            this.DtpToTime.CustomFormat = "dd/MM/yyyy hh:mm tt";
            this.DtpToTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpToTime.Location = new System.Drawing.Point(684, 48);
            this.DtpToTime.Name = "DtpToTime";
            this.DtpToTime.Size = new System.Drawing.Size(130, 20);
            this.DtpToTime.TabIndex = 5;
            // 
            // TxtDivision
            // 
            this.TxtDivision.Location = new System.Drawing.Point(427, 18);
            this.TxtDivision.Name = "TxtDivision";
            this.TxtDivision.Size = new System.Drawing.Size(356, 20);
            this.TxtDivision.TabIndex = 1;
            this.TxtDivision.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "DATE";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Branch.Properties.Resources.Down;
            this.pictureBox1.Location = new System.Drawing.Point(204, 50);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(25, 18);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 61;
            this.pictureBox1.TabStop = false;
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Branch.Properties.Resources.Down;
            this.Arrow1.Location = new System.Drawing.Point(509, 48);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(25, 18);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 61;
            this.Arrow1.TabStop = false;
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down;
            this.Arrow3.Location = new System.Drawing.Point(789, 18);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(25, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 55;
            this.Arrow3.TabStop = false;
            // 
            // FrmProductionEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 438);
            this.Controls.Add(this.GBMain);
            this.Name = "FrmProductionEdit";
            this.Text = "FrmProductionEdit";
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker DtpFromTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox TxtEno;
        private V_Components.MyTextBox TxtType;
        private V_Components.MyTextBox TxtShift;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DtpDate1;
        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox Arrow1;
        private System.Windows.Forms.Label label8;
        private V_Components.MyTextBox TxtEmployees;
        private System.Windows.Forms.PictureBox Arrow3;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.DateTimePicker DtpToTime;
        private V_Components.MyTextBox TxtDivision;
        private System.Windows.Forms.Label label1;
    }
}