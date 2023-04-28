namespace Accounts
{
    partial class FrmFloorPacking_FGS
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
            this.LblBal = new System.Windows.Forms.Label();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.LblPre_Prod = new System.Windows.Forms.Label();
            this.LblProduction = new System.Windows.Forms.Label();
            this.LblPaired = new System.Windows.Forms.Label();
            this.LblDesc = new System.Windows.Forms.Label();
            this.LblSpecial = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.TxtTotal = new V_Components.MyTextBox();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtTiming = new V_Components.MyTextBox();
            this.TxtShift = new V_Components.MyTextBox();
            this.TxtNo = new V_Components.MyTextBox();
            this.TxtUnit = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // LblBal
            // 
            this.LblBal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.LblBal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblBal.Location = new System.Drawing.Point(488, 77);
            this.LblBal.Name = "LblBal";
            this.LblBal.Size = new System.Drawing.Size(114, 23);
            this.LblBal.TabIndex = 40;
            this.LblBal.Text = "0";
            this.LblBal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.TxtUnit);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.LblBal);
            this.GBMain.Controls.Add(this.LblPre_Prod);
            this.GBMain.Controls.Add(this.LblProduction);
            this.GBMain.Controls.Add(this.LblPaired);
            this.GBMain.Controls.Add(this.LblDesc);
            this.GBMain.Controls.Add(this.LblSpecial);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.TxtTiming);
            this.GBMain.Controls.Add(this.DtpDate1);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtShift);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtNo);
            this.GBMain.Location = new System.Drawing.Point(-7, -4);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(728, 458);
            this.GBMain.TabIndex = 2;
            this.GBMain.TabStop = false;
            // 
            // LblPre_Prod
            // 
            this.LblPre_Prod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.LblPre_Prod.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPre_Prod.Location = new System.Drawing.Point(608, 47);
            this.LblPre_Prod.Name = "LblPre_Prod";
            this.LblPre_Prod.Size = new System.Drawing.Size(114, 23);
            this.LblPre_Prod.TabIndex = 39;
            this.LblPre_Prod.Text = "0";
            this.LblPre_Prod.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblProduction
            // 
            this.LblProduction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.LblProduction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblProduction.Location = new System.Drawing.Point(608, 77);
            this.LblProduction.Name = "LblProduction";
            this.LblProduction.Size = new System.Drawing.Size(114, 23);
            this.LblProduction.TabIndex = 38;
            this.LblProduction.Text = "0";
            this.LblProduction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblPaired
            // 
            this.LblPaired.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.LblPaired.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPaired.Location = new System.Drawing.Point(488, 49);
            this.LblPaired.Name = "LblPaired";
            this.LblPaired.Size = new System.Drawing.Size(114, 23);
            this.LblPaired.TabIndex = 37;
            this.LblPaired.Text = "0";
            this.LblPaired.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblDesc
            // 
            this.LblDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.LblDesc.Location = new System.Drawing.Point(367, 413);
            this.LblDesc.Name = "LblDesc";
            this.LblDesc.Size = new System.Drawing.Size(355, 38);
            this.LblDesc.TabIndex = 36;
            this.LblDesc.Text = "-";
            // 
            // LblSpecial
            // 
            this.LblSpecial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.LblSpecial.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSpecial.Location = new System.Drawing.Point(9, 16);
            this.LblSpecial.Name = "LblSpecial";
            this.LblSpecial.Size = new System.Drawing.Size(713, 26);
            this.LblSpecial.TabIndex = 35;
            this.LblSpecial.Text = "PACKING PRODUCTION";
            this.LblSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(561, 386);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "TOTAL";
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow3.Location = new System.Drawing.Point(153, 79);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(26, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 30;
            this.Arrow3.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(191, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "TIMING";
            // 
            // DtpDate1
            // 
            this.DtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate1.Location = new System.Drawing.Point(249, 52);
            this.DtpDate1.Name = "DtpDate1";
            this.DtpDate1.Size = new System.Drawing.Size(112, 20);
            this.DtpDate1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(191, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "DATE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SHIFT";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "#NO";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(367, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "UNIT";
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(625, 383);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(97, 20);
            this.TxtTotal.TabIndex = 7;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(12, 383);
            this.TxtRemarks.Multiline = true;
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(349, 67);
            this.TxtRemarks.TabIndex = 6;
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(12, 115);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(710, 257);
            this.Grid.TabIndex = 5;
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.CurrentCellChanged += new System.EventHandler(this.Grid_CurrentCellChanged);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellEnter);
            // 
            // TxtTiming
            // 
            this.TxtTiming.Location = new System.Drawing.Point(249, 79);
            this.TxtTiming.Name = "TxtTiming";
            this.TxtTiming.Size = new System.Drawing.Size(112, 20);
            this.TxtTiming.TabIndex = 3;
            this.TxtTiming.TabStop = false;
            this.TxtTiming.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtShift
            // 
            this.TxtShift.Location = new System.Drawing.Point(59, 79);
            this.TxtShift.Name = "TxtShift";
            this.TxtShift.Size = new System.Drawing.Size(88, 20);
            this.TxtShift.TabIndex = 2;
            this.TxtShift.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtNo
            // 
            this.TxtNo.Location = new System.Drawing.Point(59, 52);
            this.TxtNo.Name = "TxtNo";
            this.TxtNo.Size = new System.Drawing.Size(88, 20);
            this.TxtNo.TabIndex = 0;
            this.TxtNo.TabStop = false;
            this.TxtNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtUnit
            // 
            this.TxtUnit.Location = new System.Drawing.Point(406, 79);
            this.TxtUnit.Name = "TxtUnit";
            this.TxtUnit.Size = new System.Drawing.Size(76, 20);
            this.TxtUnit.TabIndex = 4;
            this.TxtUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmFloorPacking_FGS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 459);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmFloorPacking_FGS";
            this.Text = "FrmFloorPacking";
            this.Load += new System.EventHandler(this.FrmFloorPacking_FGS_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmFloorPacking_FGS_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmFloorPacking_FGS_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LblBal;
        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label LblPre_Prod;
        private System.Windows.Forms.Label LblProduction;
        private System.Windows.Forms.Label LblPaired;
        private System.Windows.Forms.Label LblDesc;
        private System.Windows.Forms.Label LblSpecial;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtTotal;
        private V_Components.MyTextBox TxtRemarks;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.PictureBox Arrow3;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtTiming;
        private System.Windows.Forms.DateTimePicker DtpDate1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private V_Components.MyTextBox TxtShift;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtNo;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label6;
        private V_Components.MyTextBox TxtUnit;
    }
}