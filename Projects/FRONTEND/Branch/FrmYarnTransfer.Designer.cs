namespace Accounts
{
    partial class FrmYarnTransfer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label12 = new System.Windows.Forms.Label();
            this.TxtSize = new V_Components.MyTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtColor = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtItem = new V_Components.MyTextBox();
            this.TxtTotal = new V_Components.MyTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtRemarks = new System.Windows.Forms.TextBox();
            this.GBQty = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LblBal = new System.Windows.Forms.Label();
            this.LblTfr = new System.Windows.Forms.Label();
            this.LblReq = new System.Windows.Forms.Label();
            this.GridDetail = new DotnetVFGrid.MyDataGridView();
            this.TxtQty1 = new V_Components.MyTextBox();
            this.ButExit = new System.Windows.Forms.Button();
            this.ButOk = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtBalance = new V_Components.MyTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtEnteredWeight = new V_Components.MyTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TxtEntryNo = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.GBMain.SuspendLayout();
            this.GBQty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(131, 19);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(81, 20);
            this.DtpDate.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "ENTRY NO";
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.listBox1);
            this.GBMain.Controls.Add(this.label12);
            this.GBMain.Controls.Add(this.TxtSize);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.TxtColor);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtItem);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.GBQty);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtEntryNo);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Location = new System.Drawing.Point(6, 6);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(652, 428);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(599, 13);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(47, 17);
            this.listBox1.TabIndex = 79;
            this.listBox1.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(458, 54);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 13);
            this.label12.TabIndex = 78;
            this.label12.Text = "SIZE";
            // 
            // TxtSize
            // 
            this.TxtSize.Location = new System.Drawing.Point(513, 51);
            this.TxtSize.Name = "TxtSize";
            this.TxtSize.Size = new System.Drawing.Size(132, 20);
            this.TxtSize.TabIndex = 5;
            this.TxtSize.TabStop = false;
            this.TxtSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(235, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 76;
            this.label3.Text = "COLOR";
            // 
            // TxtColor
            // 
            this.TxtColor.Location = new System.Drawing.Point(294, 51);
            this.TxtColor.Name = "TxtColor";
            this.TxtColor.Size = new System.Drawing.Size(138, 20);
            this.TxtColor.TabIndex = 4;
            this.TxtColor.TabStop = false;
            this.TxtColor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 74;
            this.label2.Text = "ITEM";
            // 
            // TxtItem
            // 
            this.TxtItem.Location = new System.Drawing.Point(77, 51);
            this.TxtItem.Name = "TxtItem";
            this.TxtItem.Size = new System.Drawing.Size(135, 20);
            this.TxtItem.TabIndex = 3;
            this.TxtItem.TabStop = false;
            this.TxtItem.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(542, 395);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(103, 20);
            this.TxtTotal.TabIndex = 7;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(482, 395);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 16);
            this.label5.TabIndex = 72;
            this.label5.Text = "TOTAL";
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(7, 383);
            this.TxtRemarks.Multiline = true;
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(427, 36);
            this.TxtRemarks.TabIndex = 8;
            // 
            // GBQty
            // 
            this.GBQty.Controls.Add(this.label4);
            this.GBQty.Controls.Add(this.label11);
            this.GBQty.Controls.Add(this.label10);
            this.GBQty.Controls.Add(this.label6);
            this.GBQty.Controls.Add(this.LblBal);
            this.GBQty.Controls.Add(this.LblTfr);
            this.GBQty.Controls.Add(this.LblReq);
            this.GBQty.Controls.Add(this.GridDetail);
            this.GBQty.Controls.Add(this.TxtQty1);
            this.GBQty.Controls.Add(this.ButExit);
            this.GBQty.Controls.Add(this.ButOk);
            this.GBQty.Controls.Add(this.label8);
            this.GBQty.Controls.Add(this.TxtBalance);
            this.GBQty.Controls.Add(this.label7);
            this.GBQty.Controls.Add(this.TxtEnteredWeight);
            this.GBQty.Controls.Add(this.label9);
            this.GBQty.Location = new System.Drawing.Point(168, 97);
            this.GBQty.Name = "GBQty";
            this.GBQty.Size = new System.Drawing.Size(434, 269);
            this.GBQty.TabIndex = 9;
            this.GBQty.TabStop = false;
            this.GBQty.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(290, 168);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 13);
            this.label11.TabIndex = 84;
            this.label11.Text = "BALANCE";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(146, 168);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 83;
            this.label10.Text = "TFR QTY";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 82;
            this.label6.Text = "REQ";
            // 
            // LblBal
            // 
            this.LblBal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.LblBal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblBal.Location = new System.Drawing.Point(352, 163);
            this.LblBal.Name = "LblBal";
            this.LblBal.Size = new System.Drawing.Size(70, 23);
            this.LblBal.TabIndex = 81;
            this.LblBal.Text = "0";
            this.LblBal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblTfr
            // 
            this.LblTfr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.LblTfr.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTfr.Location = new System.Drawing.Point(211, 163);
            this.LblTfr.Name = "LblTfr";
            this.LblTfr.Size = new System.Drawing.Size(73, 23);
            this.LblTfr.TabIndex = 80;
            this.LblTfr.Text = "0";
            this.LblTfr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblReq
            // 
            this.LblReq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.LblReq.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblReq.Location = new System.Drawing.Point(67, 163);
            this.LblReq.Name = "LblReq";
            this.LblReq.Size = new System.Drawing.Size(73, 23);
            this.LblReq.TabIndex = 78;
            this.LblReq.Text = "0";
            this.LblReq.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GridDetail
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.GridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GridDetail.DefaultCellStyle = dataGridViewCellStyle2;
            this.GridDetail.Location = new System.Drawing.Point(22, 19);
            this.GridDetail.Name = "GridDetail";
            this.GridDetail.Size = new System.Drawing.Size(403, 123);
            this.GridDetail.TabIndex = 1;
            this.GridDetail.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.GridDetail_EditingControlShowing);
            this.GridDetail.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.GridDetail_RowsAdded);
            this.GridDetail.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.GridDetail_RowsRemoved);
            this.GridDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridDetail_KeyDown);
            // 
            // TxtQty1
            // 
            this.TxtQty1.Location = new System.Drawing.Point(67, 198);
            this.TxtQty1.Name = "TxtQty1";
            this.TxtQty1.Size = new System.Drawing.Size(73, 20);
            this.TxtQty1.TabIndex = 2;
            this.TxtQty1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ButExit
            // 
            this.ButExit.Location = new System.Drawing.Point(336, 224);
            this.ButExit.Name = "ButExit";
            this.ButExit.Size = new System.Drawing.Size(86, 33);
            this.ButExit.TabIndex = 6;
            this.ButExit.Text = "E&XIT";
            this.ButExit.UseVisualStyleBackColor = true;
            this.ButExit.Click += new System.EventHandler(this.ButExit_Click);
            // 
            // ButOk
            // 
            this.ButOk.Location = new System.Drawing.Point(244, 224);
            this.ButOk.Name = "ButOk";
            this.ButOk.Size = new System.Drawing.Size(86, 33);
            this.ButOk.TabIndex = 5;
            this.ButOk.Text = "&OK";
            this.ButOk.UseVisualStyleBackColor = true;
            this.ButOk.Click += new System.EventHandler(this.ButOk_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(290, 201);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 77;
            this.label8.Text = "BALANCE";
            // 
            // TxtBalance
            // 
            this.TxtBalance.Location = new System.Drawing.Point(352, 196);
            this.TxtBalance.Name = "TxtBalance";
            this.TxtBalance.Size = new System.Drawing.Size(70, 20);
            this.TxtBalance.TabIndex = 4;
            this.TxtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(146, 201);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 75;
            this.label7.Text = "ENTERED";
            // 
            // TxtEnteredWeight
            // 
            this.TxtEnteredWeight.Location = new System.Drawing.Point(211, 198);
            this.TxtEnteredWeight.Name = "TxtEnteredWeight";
            this.TxtEnteredWeight.Size = new System.Drawing.Size(73, 20);
            this.TxtEnteredWeight.TabIndex = 3;
            this.TxtEnteredWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 201);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 73;
            this.label9.Text = "TOTAL";
            // 
            // TxtEntryNo
            // 
            this.TxtEntryNo.Location = new System.Drawing.Point(77, 19);
            this.TxtEntryNo.Name = "TxtEntryNo";
            this.TxtEntryNo.Size = new System.Drawing.Size(52, 20);
            this.TxtEntryNo.TabIndex = 1;
            this.TxtEntryNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Grid
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Grid.DefaultCellStyle = dataGridViewCellStyle4;
            this.Grid.Location = new System.Drawing.Point(7, 83);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(639, 297);
            this.Grid.TabIndex = 6;
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(19, 229);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 23);
            this.label4.TabIndex = 85;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmYarnTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 437);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmYarnTransfer";
            this.Text = "FrmYarnTransfer";
            this.Load += new System.EventHandler(this.FrmYarnTransfer_Load);
            this.DoubleClick += new System.EventHandler(this.GridDetail_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmYarnTransfer_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmYarnTransfer_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.GBQty.ResumeLayout(false);
            this.GBQty.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtEntryNo;
        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.GroupBox GBQty;
        private DotnetVFGrid.MyDataGridView GridDetail;
        private V_Components.MyTextBox TxtQty1;
        private System.Windows.Forms.Button ButExit;
        private System.Windows.Forms.Button ButOk;
        private System.Windows.Forms.Label label8;
        private V_Components.MyTextBox TxtBalance;
        private System.Windows.Forms.Label label7;
        private V_Components.MyTextBox TxtEnteredWeight;
        private System.Windows.Forms.Label label9;
        private V_Components.MyTextBox TxtTotal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtRemarks;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox TxtColor;
        private System.Windows.Forms.Label label2;
        private V_Components.MyTextBox TxtItem;
        private System.Windows.Forms.Label LblBal;
        private System.Windows.Forms.Label LblTfr;
        private System.Windows.Forms.Label LblReq;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label12;
        private V_Components.MyTextBox TxtSize;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label4;
    }
}