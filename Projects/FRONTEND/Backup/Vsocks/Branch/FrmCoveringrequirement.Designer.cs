namespace Accounts
{
    partial class FrmCoveringrequirement
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
            this.LblSpecial = new System.Windows.Forms.Label();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.TxtTotal = new V_Components.MyTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtRemarks = new System.Windows.Forms.TextBox();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtOrder = new V_Components.MyTextBox();
            this.TxtEntryNo = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GBMain.SuspendLayout();
            this.GBQty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.GBQty);
            this.GBMain.Controls.Add(this.LblSpecial);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtOrder);
            this.GBMain.Controls.Add(this.TxtEntryNo);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(5, 4);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(503, 323);
            this.GBMain.TabIndex = 1;
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
            this.GBQty.Location = new System.Drawing.Point(19, 88);
            this.GBQty.Name = "GBQty";
            this.GBQty.Size = new System.Drawing.Size(477, 221);
            this.GBQty.TabIndex = 70;
            this.GBQty.TabStop = false;
            this.GBQty.Visible = false;
            // 
            // GridDetail
            // 
            this.GridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridDetail.Location = new System.Drawing.Point(6, 13);
            this.GridDetail.Name = "GridDetail";
            this.GridDetail.Size = new System.Drawing.Size(465, 126);
            this.GridDetail.TabIndex = 0;
            this.GridDetail.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.GridDetail_EditingControlShowing);
            this.GridDetail.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.GridDetail_RowsAdded);
            this.GridDetail.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.GridDetail_RowsRemoved);
            this.GridDetail.DoubleClick += new System.EventHandler(this.GridDetail_DoubleClick);
            this.GridDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridDetail_KeyDown);
            this.GridDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridDetail_KeyPress);
            // 
            // TxtQty1
            // 
            this.TxtQty1.Location = new System.Drawing.Point(61, 154);
            this.TxtQty1.Name = "TxtQty1";
            this.TxtQty1.Size = new System.Drawing.Size(66, 20);
            this.TxtQty1.TabIndex = 6;
            this.TxtQty1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ButExit
            // 
            this.ButExit.Location = new System.Drawing.Point(386, 182);
            this.ButExit.Name = "ButExit";
            this.ButExit.Size = new System.Drawing.Size(86, 33);
            this.ButExit.TabIndex = 2;
            this.ButExit.Text = "E&XIT";
            this.ButExit.UseVisualStyleBackColor = true;
            this.ButExit.Click += new System.EventHandler(this.ButExit_Click);
            // 
            // ButOk
            // 
            this.ButOk.Location = new System.Drawing.Point(293, 182);
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
            this.label8.Location = new System.Drawing.Point(339, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 77;
            this.label8.Text = "BALANCE";
            // 
            // TxtBalance
            // 
            this.TxtBalance.Location = new System.Drawing.Point(405, 154);
            this.TxtBalance.Name = "TxtBalance";
            this.TxtBalance.Size = new System.Drawing.Size(66, 20);
            this.TxtBalance.TabIndex = 8;
            this.TxtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(157, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 75;
            this.label7.Text = "ENTERED";
            // 
            // TxtEnteredWeight
            // 
            this.TxtEnteredWeight.Location = new System.Drawing.Point(222, 154);
            this.TxtEnteredWeight.Name = "TxtEnteredWeight";
            this.TxtEnteredWeight.Size = new System.Drawing.Size(66, 20);
            this.TxtEnteredWeight.TabIndex = 7;
            this.TxtEnteredWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 157);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 73;
            this.label9.Text = "TOTAL";
            // 
            // LblSpecial
            // 
            this.LblSpecial.BackColor = System.Drawing.Color.Silver;
            this.LblSpecial.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSpecial.Location = new System.Drawing.Point(4, 16);
            this.LblSpecial.Name = "LblSpecial";
            this.LblSpecial.Size = new System.Drawing.Size(492, 27);
            this.LblSpecial.TabIndex = 69;
            this.LblSpecial.Text = "COVERING REQUIREMENT";
            this.LblSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(4, 90);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(492, 180);
            this.Grid.TabIndex = 1;
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow3.Location = new System.Drawing.Point(474, 47);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(22, 20);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 66;
            this.Arrow3.TabStop = false;
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(407, 293);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(89, 20);
            this.TxtTotal.TabIndex = 3;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(347, 293);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 16);
            this.label3.TabIndex = 65;
            this.label3.Text = "TOTAL";
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(5, 277);
            this.TxtRemarks.Multiline = true;
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(290, 36);
            this.TxtRemarks.TabIndex = 2;
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(182, 46);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(107, 20);
            this.DtpDate.TabIndex = 60;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(315, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 16);
            this.label5.TabIndex = 59;
            this.label5.Text = "PO NO";
            // 
            // TxtOrder
            // 
            this.TxtOrder.Location = new System.Drawing.Point(379, 46);
            this.TxtOrder.Name = "TxtOrder";
            this.TxtOrder.Size = new System.Drawing.Size(91, 20);
            this.TxtOrder.TabIndex = 0;
            this.TxtOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtEntryNo
            // 
            this.TxtEntryNo.Location = new System.Drawing.Point(88, 46);
            this.TxtEntryNo.Name = "TxtEntryNo";
            this.TxtEntryNo.Size = new System.Drawing.Size(91, 20);
            this.TxtEntryNo.TabIndex = 55;
            this.TxtEntryNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 16);
            this.label1.TabIndex = 56;
            this.label1.Text = "ENTRY NO";
            // 
            // FrmCoveringrequirement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 332);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmCoveringrequirement";
            this.Text = "FrmCoveringrequirement";
            this.Load += new System.EventHandler(this.FrmCoveringrequirement_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmCoveringrequirement_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmCoveringrequirement_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.GBQty.ResumeLayout(false);
            this.GBQty.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label LblSpecial;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.PictureBox Arrow3;
        private V_Components.MyTextBox TxtTotal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtRemarks;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtOrder;
        private V_Components.MyTextBox TxtEntryNo;
        private System.Windows.Forms.Label label1;
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
    }
}