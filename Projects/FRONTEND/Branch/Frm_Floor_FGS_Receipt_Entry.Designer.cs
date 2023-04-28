namespace Accounts
{
    partial class Frm_Floor_FGS_Receipt_Entry
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
            this.ButOk = new System.Windows.Forms.Button();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.Group1 = new System.Windows.Forms.GroupBox();
            this.RbtMultiple = new System.Windows.Forms.RadioButton();
            this.RbtSingle = new System.Windows.Forms.RadioButton();
            this.DtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.LblSpecial = new System.Windows.Forms.Label();
            this.GBQty = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ButCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtEntryNo = new V_Components.MyTextBox();
            this.TxtBalance = new V_Components.MyTextBox();
            this.TxtEnteredPieces = new V_Components.MyTextBox();
            this.TxtQty = new V_Components.MyTextBox();
            this.GridQty = new DotnetVFGrid.MyDataGridView();
            this.TxtTotal = new V_Components.MyTextBox();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.GBMain.SuspendLayout();
            this.Group1.SuspendLayout();
            this.GBQty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // ButOk
            // 
            this.ButOk.Location = new System.Drawing.Point(329, 175);
            this.ButOk.Name = "ButOk";
            this.ButOk.Size = new System.Drawing.Size(78, 27);
            this.ButOk.TabIndex = 1;
            this.ButOk.Text = "&OK";
            this.ButOk.UseVisualStyleBackColor = true;
            this.ButOk.Click += new System.EventHandler(this.ButOk_Click);
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.Group1);
            this.GBMain.Controls.Add(this.DtpDate1);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.TxtEntryNo);
            this.GBMain.Controls.Add(this.LblSpecial);
            this.GBMain.Controls.Add(this.GBQty);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Location = new System.Drawing.Point(7, 5);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(641, 489);
            this.GBMain.TabIndex = 2;
            this.GBMain.TabStop = false;
            // 
            // Group1
            // 
            this.Group1.Controls.Add(this.RbtMultiple);
            this.Group1.Controls.Add(this.RbtSingle);
            this.Group1.Location = new System.Drawing.Point(375, 38);
            this.Group1.Name = "Group1";
            this.Group1.Size = new System.Drawing.Size(256, 32);
            this.Group1.TabIndex = 1;
            this.Group1.TabStop = false;
            // 
            // RbtMultiple
            // 
            this.RbtMultiple.AutoSize = true;
            this.RbtMultiple.Location = new System.Drawing.Point(128, 12);
            this.RbtMultiple.Name = "RbtMultiple";
            this.RbtMultiple.Size = new System.Drawing.Size(127, 17);
            this.RbtMultiple.TabIndex = 1;
            this.RbtMultiple.TabStop = true;
            this.RbtMultiple.Text = "Multiple Sample Pack";
            this.RbtMultiple.UseVisualStyleBackColor = true;
            this.RbtMultiple.CheckedChanged += new System.EventHandler(this.RbtMultiple_CheckedChanged);
            // 
            // RbtSingle
            // 
            this.RbtSingle.AutoSize = true;
            this.RbtSingle.Location = new System.Drawing.Point(6, 12);
            this.RbtSingle.Name = "RbtSingle";
            this.RbtSingle.Size = new System.Drawing.Size(120, 17);
            this.RbtSingle.TabIndex = 0;
            this.RbtSingle.TabStop = true;
            this.RbtSingle.Text = "Single Sample Pack";
            this.RbtSingle.UseVisualStyleBackColor = true;
            this.RbtSingle.CheckedChanged += new System.EventHandler(this.RbtSingle_CheckedChanged);
            // 
            // DtpDate1
            // 
            this.DtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate1.Location = new System.Drawing.Point(193, 50);
            this.DtpDate1.Name = "DtpDate1";
            this.DtpDate1.Size = new System.Drawing.Size(112, 20);
            this.DtpDate1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(151, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "DATE";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "#NO";
            // 
            // LblSpecial
            // 
            this.LblSpecial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.LblSpecial.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSpecial.Location = new System.Drawing.Point(10, 12);
            this.LblSpecial.Name = "LblSpecial";
            this.LblSpecial.Size = new System.Drawing.Size(621, 26);
            this.LblSpecial.TabIndex = 36;
            this.LblSpecial.Text = "FGS RECEIPT ENTRY";
            this.LblSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GBQty
            // 
            this.GBQty.Controls.Add(this.label7);
            this.GBQty.Controls.Add(this.TxtBalance);
            this.GBQty.Controls.Add(this.label6);
            this.GBQty.Controls.Add(this.TxtEnteredPieces);
            this.GBQty.Controls.Add(this.label5);
            this.GBQty.Controls.Add(this.TxtQty);
            this.GBQty.Controls.Add(this.ButCancel);
            this.GBQty.Controls.Add(this.ButOk);
            this.GBQty.Controls.Add(this.GridQty);
            this.GBQty.Location = new System.Drawing.Point(113, 143);
            this.GBQty.Name = "GBQty";
            this.GBQty.Size = new System.Drawing.Size(501, 208);
            this.GBQty.TabIndex = 3;
            this.GBQty.TabStop = false;
            this.GBQty.Text = "JOB ORDER DETAILS ...!";
            this.GBQty.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(352, 150);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "BALANCE";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(169, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "ENTERED";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "TOTAL";
            // 
            // ButCancel
            // 
            this.ButCancel.Location = new System.Drawing.Point(417, 175);
            this.ButCancel.Name = "ButCancel";
            this.ButCancel.Size = new System.Drawing.Size(78, 27);
            this.ButCancel.TabIndex = 2;
            this.ButCancel.Text = "&CANCEL";
            this.ButCancel.UseVisualStyleBackColor = true;
            this.ButCancel.Click += new System.EventHandler(this.ButCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(470, 412);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "TOTAL";
            // 
            // TxtEntryNo
            // 
            this.TxtEntryNo.Location = new System.Drawing.Point(46, 50);
            this.TxtEntryNo.Name = "TxtEntryNo";
            this.TxtEntryNo.Size = new System.Drawing.Size(88, 20);
            this.TxtEntryNo.TabIndex = 37;
            this.TxtEntryNo.TabStop = false;
            this.TxtEntryNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtBalance
            // 
            this.TxtBalance.Location = new System.Drawing.Point(425, 146);
            this.TxtBalance.Name = "TxtBalance";
            this.TxtBalance.Size = new System.Drawing.Size(70, 20);
            this.TxtBalance.TabIndex = 2;
            this.TxtBalance.TabStop = false;
            this.TxtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtEnteredPieces
            // 
            this.TxtEnteredPieces.Location = new System.Drawing.Point(234, 146);
            this.TxtEnteredPieces.Name = "TxtEnteredPieces";
            this.TxtEnteredPieces.Size = new System.Drawing.Size(78, 20);
            this.TxtEnteredPieces.TabIndex = 5;
            this.TxtEnteredPieces.TabStop = false;
            this.TxtEnteredPieces.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtQty
            // 
            this.TxtQty.Location = new System.Drawing.Point(57, 146);
            this.TxtQty.Name = "TxtQty";
            this.TxtQty.Size = new System.Drawing.Size(70, 20);
            this.TxtQty.TabIndex = 4;
            this.TxtQty.TabStop = false;
            this.TxtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // GridQty
            // 
            this.GridQty.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridQty.Location = new System.Drawing.Point(10, 19);
            this.GridQty.Name = "GridQty";
            this.GridQty.Size = new System.Drawing.Size(485, 117);
            this.GridQty.TabIndex = 0;
            this.GridQty.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.GridQty_EditingControlShowing);
            this.GridQty.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.GridQty_RowsAdded);
            this.GridQty.DoubleClick += new System.EventHandler(this.GridQty_DoubleClick);
            this.GridQty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridQty_KeyDown);
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(525, 409);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(106, 20);
            this.TxtTotal.TabIndex = 5;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(9, 409);
            this.TxtRemarks.Multiline = true;
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(441, 63);
            this.TxtRemarks.TabIndex = 4;
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(6, 76);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid.Size = new System.Drawing.Size(625, 314);
            this.Grid.TabIndex = 2;
            this.Grid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellEnter);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            this.Grid.Leave += new System.EventHandler(this.Grid_Leave);
            // 
            // Frm_Floor_FGS_Receipt_Entry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 500);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "Frm_Floor_FGS_Receipt_Entry";
            this.Text = "Frm_Floor_FGS_Receipt_Entry";
            this.Load += new System.EventHandler(this.Frm_Floor_FGS_Receipt_Entry_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Floor_FGS_Receipt_Entry_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Floor_FGS_Receipt_Entry_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.Group1.ResumeLayout(false);
            this.Group1.PerformLayout();
            this.GBQty.ResumeLayout(false);
            this.GBQty.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButOk;
        private DotnetVFGrid.MyDataGridView GridQty;
        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.DateTimePicker DtpDate1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private V_Components.MyTextBox TxtEntryNo;
        private System.Windows.Forms.Label LblSpecial;
        private System.Windows.Forms.GroupBox GBQty;
        private System.Windows.Forms.Label label7;
        private V_Components.MyTextBox TxtBalance;
        private System.Windows.Forms.Label label6;
        private V_Components.MyTextBox TxtEnteredPieces;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtQty;
        private System.Windows.Forms.Button ButCancel;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox TxtTotal;
        private V_Components.MyTextBox TxtRemarks;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.GroupBox Group1;
        private System.Windows.Forms.RadioButton RbtMultiple;
        private System.Windows.Forms.RadioButton RbtSingle;
    }
}