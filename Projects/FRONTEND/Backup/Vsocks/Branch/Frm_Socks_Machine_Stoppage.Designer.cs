namespace Accounts
{
    partial class Frm_Socks_Machine_Stoppage
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.LblSpecial = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.TxtTotal = new V_Components.MyTextBox();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.DtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtShift = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtNo = new V_Components.MyTextBox();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10000;
            // 
            // LblSpecial
            // 
            this.LblSpecial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.LblSpecial.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSpecial.Location = new System.Drawing.Point(9, 16);
            this.LblSpecial.Name = "LblSpecial";
            this.LblSpecial.Size = new System.Drawing.Size(679, 26);
            this.LblSpecial.TabIndex = 35;
            this.LblSpecial.Text = "MACHINE STOPPAGE";
            this.LblSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(522, 386);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "TOTAL";
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.LblSpecial);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.DtpDate1);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtShift);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtNo);
            this.GBMain.Location = new System.Drawing.Point(5, 3);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(695, 458);
            this.GBMain.TabIndex = 2;
            this.GBMain.TabStop = false;
            this.GBMain.Enter += new System.EventHandler(this.GBMain_Enter);
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(580, 383);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(108, 20);
            this.TxtTotal.TabIndex = 6;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtTotal.TextChanged += new System.EventHandler(this.TxtTotal_TextChanged);
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(12, 383);
            this.TxtRemarks.Multiline = true;
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(492, 67);
            this.TxtRemarks.TabIndex = 5;
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(12, 81);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(676, 291);
            this.Grid.TabIndex = 4;
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.CurrentCellChanged += new System.EventHandler(this.Grid_CurrentCellChanged);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellEnter);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            this.Grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellContentClick);
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow3.Location = new System.Drawing.Point(662, 53);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(26, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 30;
            this.Arrow3.TabStop = false;
            // 
            // DtpDate1
            // 
            this.DtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate1.Location = new System.Drawing.Point(296, 55);
            this.DtpDate1.Name = "DtpDate1";
            this.DtpDate1.Size = new System.Drawing.Size(112, 20);
            this.DtpDate1.TabIndex = 1;
            this.DtpDate1.ValueChanged += new System.EventHandler(this.DtpDate1_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(242, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "DATE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(466, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SHIFT";
            // 
            // TxtShift
            // 
            this.TxtShift.Location = new System.Drawing.Point(525, 55);
            this.TxtShift.Name = "TxtShift";
            this.TxtShift.Size = new System.Drawing.Size(131, 20);
            this.TxtShift.TabIndex = 2;
            this.TxtShift.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtShift.TextChanged += new System.EventHandler(this.TxtShift_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "#NO";
            // 
            // TxtNo
            // 
            this.TxtNo.Location = new System.Drawing.Point(51, 54);
            this.TxtNo.Name = "TxtNo";
            this.TxtNo.Size = new System.Drawing.Size(102, 20);
            this.TxtNo.TabIndex = 0;
            this.TxtNo.TabStop = false;
            this.TxtNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Frm_Socks_Machine_Stoppage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 465);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "Frm_Socks_Machine_Stoppage";
            this.Text = "Frm_Socks_Machine_Stoppage";
            this.Load += new System.EventHandler(this.Frm_Socks_Machine_Stoppage_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Socks_Machine_Stoppage_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Socks_Machine_Stoppage_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label LblSpecial;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtTotal;
        private V_Components.MyTextBox TxtRemarks;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.PictureBox Arrow3;
        private System.Windows.Forms.DateTimePicker DtpDate1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private V_Components.MyTextBox TxtShift;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtNo;
        private System.Drawing.Printing.PrintDocument printDocument1;

    }
}