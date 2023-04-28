namespace Accounts
{
    partial class FrmFloorKnitting
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
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.LblBal = new System.Windows.Forms.Label();
            this.LblPre_Prod = new System.Windows.Forms.Label();
            this.LblProduction = new System.Windows.Forms.Label();
            this.LblBOM = new System.Windows.Forms.Label();
            this.LblDesc = new System.Windows.Forms.Label();
            this.LblSpecial = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.TxtTotal = new V_Components.MyTextBox();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtTiming = new V_Components.MyTextBox();
            this.TxtShift = new V_Components.MyTextBox();
            this.TxtNo = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.LblBal);
            this.GBMain.Controls.Add(this.LblPre_Prod);
            this.GBMain.Controls.Add(this.LblProduction);
            this.GBMain.Controls.Add(this.LblBOM);
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
            this.GBMain.Location = new System.Drawing.Point(8, 0);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(850, 458);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            this.GBMain.Enter += new System.EventHandler(this.GBMain_Enter);
            // 
            // LblBal
            // 
            this.LblBal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.LblBal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblBal.Location = new System.Drawing.Point(602, 83);
            this.LblBal.Name = "LblBal";
            this.LblBal.Size = new System.Drawing.Size(114, 23);
            this.LblBal.TabIndex = 40;
            this.LblBal.Text = "0";
            this.LblBal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblPre_Prod
            // 
            this.LblPre_Prod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.LblPre_Prod.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPre_Prod.Location = new System.Drawing.Point(722, 54);
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
            this.LblProduction.Location = new System.Drawing.Point(722, 82);
            this.LblProduction.Name = "LblProduction";
            this.LblProduction.Size = new System.Drawing.Size(114, 23);
            this.LblProduction.TabIndex = 38;
            this.LblProduction.Text = "0";
            this.LblProduction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblBOM
            // 
            this.LblBOM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.LblBOM.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblBOM.Location = new System.Drawing.Point(602, 53);
            this.LblBOM.Name = "LblBOM";
            this.LblBOM.Size = new System.Drawing.Size(114, 23);
            this.LblBOM.TabIndex = 37;
            this.LblBOM.Text = "0";
            this.LblBOM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblDesc
            // 
            this.LblDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.LblDesc.Location = new System.Drawing.Point(367, 418);
            this.LblDesc.Name = "LblDesc";
            this.LblDesc.Size = new System.Drawing.Size(474, 32);
            this.LblDesc.TabIndex = 36;
            this.LblDesc.Text = "-";
            // 
            // LblSpecial
            // 
            this.LblSpecial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.LblSpecial.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSpecial.Location = new System.Drawing.Point(9, 16);
            this.LblSpecial.Name = "LblSpecial";
            this.LblSpecial.Size = new System.Drawing.Size(832, 26);
            this.LblSpecial.TabIndex = 35;
            this.LblSpecial.Text = "KNITTING PRODUCTION";
            this.LblSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(693, 386);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "TOTAL";
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow3.Location = new System.Drawing.Point(161, 82);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(26, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 30;
            this.Arrow3.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(215, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "TIMING";
            // 
            // DtpDate1
            // 
            this.DtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate1.Location = new System.Drawing.Point(281, 54);
            this.DtpDate1.Name = "DtpDate1";
            this.DtpDate1.Size = new System.Drawing.Size(112, 21);
            this.DtpDate1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(215, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "DATE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SHIFT";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "#NO";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(751, 383);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(90, 21);
            this.TxtTotal.TabIndex = 6;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(12, 383);
            this.TxtRemarks.Multiline = true;
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(349, 67);
            this.TxtRemarks.TabIndex = 5;
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(12, 115);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(829, 257);
            this.Grid.TabIndex = 4;
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.CurrentCellChanged += new System.EventHandler(this.Grid_CurrentCellChanged);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellEnter);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // TxtTiming
            // 
            this.TxtTiming.Location = new System.Drawing.Point(281, 83);
            this.TxtTiming.Name = "TxtTiming";
            this.TxtTiming.Size = new System.Drawing.Size(112, 21);
            this.TxtTiming.TabIndex = 3;
            this.TxtTiming.TabStop = false;
            this.TxtTiming.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtShift
            // 
            this.TxtShift.Location = new System.Drawing.Point(67, 82);
            this.TxtShift.Name = "TxtShift";
            this.TxtShift.Size = new System.Drawing.Size(88, 21);
            this.TxtShift.TabIndex = 2;
            this.TxtShift.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtShift.Enter += new System.EventHandler(this.TxtShift_Enter);
            // 
            // TxtNo
            // 
            this.TxtNo.Location = new System.Drawing.Point(67, 54);
            this.TxtNo.Name = "TxtNo";
            this.TxtNo.Size = new System.Drawing.Size(88, 21);
            this.TxtNo.TabIndex = 0;
            this.TxtNo.TabStop = false;
            this.TxtNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmFloorKnitting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 466);
            this.Controls.Add(this.GBMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "FrmFloorKnitting";
            this.Text = "KNITTING FLOOR PRODUCTION ...!";
            this.Load += new System.EventHandler(this.FrmFloorKnitting_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmFloorKnitting_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmFloorKnitting_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtNo;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtTiming;
        private System.Windows.Forms.DateTimePicker DtpDate1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private V_Components.MyTextBox TxtShift;
        private System.Windows.Forms.PictureBox Arrow3;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtTotal;
        private V_Components.MyTextBox TxtRemarks;
        private System.Windows.Forms.Label LblSpecial;
        private System.Windows.Forms.Label LblDesc;
        private System.Windows.Forms.Label LblProduction;
        private System.Windows.Forms.Label LblBOM;
        private System.Windows.Forms.Label LblPre_Prod;
        private System.Windows.Forms.Label LblBal;
        private System.Windows.Forms.Timer timer1;
    }
}