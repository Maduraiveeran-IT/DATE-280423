namespace Accounts
{
    partial class FrmSocks_LotEntry
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
            this.GBQty = new System.Windows.Forms.GroupBox();
            this.GridDetail = new DotnetVFGrid.MyDataGridView();
            this.TxtQty1 = new V_Components.MyTextBox();
            this.ButExit = new System.Windows.Forms.Button();
            this.ButOk = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtBalance = new V_Components.MyTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtEnteredWeight = new V_Components.MyTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.Arrow4 = new System.Windows.Forms.PictureBox();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.LblSpecial = new System.Windows.Forms.Label();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtGrnNO = new V_Components.MyTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtSupplier = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtTotal = new V_Components.MyTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.TxtEntryNo = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GBMain.SuspendLayout();
            this.GBQty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.GBQty);
            this.GBMain.Controls.Add(this.Arrow4);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.LblSpecial);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.TxtGrnNO);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtSupplier);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.TxtEntryNo);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(5, 6);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(666, 475);
            this.GBMain.TabIndex = 2;
            this.GBMain.TabStop = false;
            // 
            // GBQty
            // 
            this.GBQty.Controls.Add(this.GridDetail);
            this.GBQty.Controls.Add(this.TxtQty1);
            this.GBQty.Controls.Add(this.ButExit);
            this.GBQty.Controls.Add(this.ButOk);
            this.GBQty.Controls.Add(this.label8);
            this.GBQty.Controls.Add(this.TxtBalance);
            this.GBQty.Controls.Add(this.label7);
            this.GBQty.Controls.Add(this.TxtEnteredWeight);
            this.GBQty.Controls.Add(this.label9);
            this.GBQty.Location = new System.Drawing.Point(208, 143);
            this.GBQty.Name = "GBQty";
            this.GBQty.Size = new System.Drawing.Size(443, 229);
            this.GBQty.TabIndex = 4;
            this.GBQty.TabStop = false;
            this.GBQty.Visible = false;
            // 
            // GridDetail
            // 
            this.GridDetail.AllowUserToAddRows = false;
            this.GridDetail.AllowUserToDeleteRows = false;
            this.GridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridDetail.Location = new System.Drawing.Point(22, 19);
            this.GridDetail.Name = "GridDetail";
            this.GridDetail.Size = new System.Drawing.Size(403, 123);
            this.GridDetail.TabIndex = 0;
            this.GridDetail.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.GridDetail_EditingControlShowing);
            this.GridDetail.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.GridDetail_RowsAdded);
            this.GridDetail.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.GridDetail_RowsRemoved);
            this.GridDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridDetail_KeyDown);
            // 
            // TxtQty1
            // 
            this.TxtQty1.Location = new System.Drawing.Point(67, 158);
            this.TxtQty1.Name = "TxtQty1";
            this.TxtQty1.Size = new System.Drawing.Size(66, 20);
            this.TxtQty1.TabIndex = 6;
            this.TxtQty1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ButExit
            // 
            this.ButExit.Location = new System.Drawing.Point(339, 188);
            this.ButExit.Name = "ButExit";
            this.ButExit.Size = new System.Drawing.Size(86, 33);
            this.ButExit.TabIndex = 2;
            this.ButExit.Text = "E&XIT";
            this.ButExit.UseVisualStyleBackColor = true;
            this.ButExit.Click += new System.EventHandler(this.ButExit_Click);
            // 
            // ButOk
            // 
            this.ButOk.Location = new System.Drawing.Point(247, 188);
            this.ButOk.Name = "ButOk";
            this.ButOk.Size = new System.Drawing.Size(86, 33);
            this.ButOk.TabIndex = 1;
            this.ButOk.Text = "&OK";
            this.ButOk.UseVisualStyleBackColor = true;
            this.ButOk.Click += new System.EventHandler(this.ButOk_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(290, 161);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 77;
            this.label8.Text = "BALANCE";
            // 
            // TxtBalance
            // 
            this.TxtBalance.Location = new System.Drawing.Point(356, 156);
            this.TxtBalance.Name = "TxtBalance";
            this.TxtBalance.Size = new System.Drawing.Size(66, 20);
            this.TxtBalance.TabIndex = 8;
            this.TxtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(146, 161);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 75;
            this.label7.Text = "ENTERED";
            // 
            // TxtEnteredWeight
            // 
            this.TxtEnteredWeight.Location = new System.Drawing.Point(211, 158);
            this.TxtEnteredWeight.Name = "TxtEnteredWeight";
            this.TxtEnteredWeight.Size = new System.Drawing.Size(66, 20);
            this.TxtEnteredWeight.TabIndex = 7;
            this.TxtEnteredWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 161);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 73;
            this.label9.Text = "TOTAL";
            // 
            // Arrow4
            // 
            this.Arrow4.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow4.Location = new System.Drawing.Point(610, 43);
            this.Arrow4.Name = "Arrow4";
            this.Arrow4.Size = new System.Drawing.Size(26, 21);
            this.Arrow4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow4.TabIndex = 56;
            this.Arrow4.TabStop = false;
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(11, 402);
            this.TxtRemarks.Multiline = true;
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(304, 67);
            this.TxtRemarks.TabIndex = 4;
            // 
            // LblSpecial
            // 
            this.LblSpecial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.LblSpecial.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSpecial.Location = new System.Drawing.Point(11, 8);
            this.LblSpecial.Name = "LblSpecial";
            this.LblSpecial.Size = new System.Drawing.Size(647, 26);
            this.LblSpecial.TabIndex = 55;
            this.LblSpecial.Text = "LOT ENTRY";
            this.LblSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow3.Location = new System.Drawing.Point(364, 76);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(26, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 54;
            this.Arrow3.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(368, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 16);
            this.label4.TabIndex = 53;
            this.label4.Text = "GRN NO";
            // 
            // TxtGrnNO
            // 
            this.TxtGrnNO.Location = new System.Drawing.Point(432, 43);
            this.TxtGrnNO.Name = "TxtGrnNO";
            this.TxtGrnNO.Size = new System.Drawing.Size(177, 20);
            this.TxtGrnNO.TabIndex = 1;
            this.TxtGrnNO.TabStop = false;
            this.TxtGrnNO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 16);
            this.label5.TabIndex = 51;
            this.label5.Text = "SUPPLIER";
            // 
            // TxtSupplier
            // 
            this.TxtSupplier.Location = new System.Drawing.Point(92, 76);
            this.TxtSupplier.Name = "TxtSupplier";
            this.TxtSupplier.Size = new System.Drawing.Size(270, 20);
            this.TxtSupplier.TabIndex = 2;
            this.TxtSupplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(198, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 16);
            this.label2.TabIndex = 49;
            this.label2.Text = "DATE";
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(551, 403);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(103, 20);
            this.TxtTotal.TabIndex = 5;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(488, 403);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 16);
            this.label3.TabIndex = 48;
            this.label3.Text = "TOTAL";
            // 
            // Grid
            // 
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(8, 110);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(650, 281);
            this.Grid.TabIndex = 3;
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(250, 44);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(112, 20);
            this.DtpDate.TabIndex = 0;
            // 
            // TxtEntryNo
            // 
            this.TxtEntryNo.Location = new System.Drawing.Point(91, 44);
            this.TxtEntryNo.Name = "TxtEntryNo";
            this.TxtEntryNo.Size = new System.Drawing.Size(102, 20);
            this.TxtEntryNo.TabIndex = 0;
            this.TxtEntryNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "ENTRY NO";
            // 
            // FrmSocks_LotEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 483);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmSocks_LotEntry";
            this.Text = "FrmSocks_LotEntry";
            this.Load += new System.EventHandler(this.FrmSocks_LotEntry_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSocks_LotEntry_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmSocks_LotEntry_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.GBQty.ResumeLayout(false);
            this.GBQty.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private V_Components.MyTextBox TxtRemarks;
        private System.Windows.Forms.Label LblSpecial;
        private System.Windows.Forms.PictureBox Arrow3;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtGrnNO;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtSupplier;
        private System.Windows.Forms.Label label2;
        private V_Components.MyTextBox TxtTotal;
        private System.Windows.Forms.Label label3;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private V_Components.MyTextBox TxtEntryNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox Arrow4;
        private System.Windows.Forms.GroupBox GBQty;
        private V_Components.MyTextBox TxtQty1;
        private System.Windows.Forms.Button ButExit;
        private System.Windows.Forms.Button ButOk;
        private System.Windows.Forms.Label label8;
        private V_Components.MyTextBox TxtBalance;
        private System.Windows.Forms.Label label7;
        private V_Components.MyTextBox TxtEnteredWeight;
        private System.Windows.Forms.Label label9;
        private DotnetVFGrid.MyDataGridView GridDetail;
        //private DotnetVFGrid.MyDataGridView GridLot;
    }
}