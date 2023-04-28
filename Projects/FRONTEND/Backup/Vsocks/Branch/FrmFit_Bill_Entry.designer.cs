namespace Accounts
{
    partial class FrmFit_Bill_Entry
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
            this.DtpIssue = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.Arrow2 = new System.Windows.Forms.PictureBox();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Grid1 = new DotnetVFGrid.MyDataGridView();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtBillType = new V_Components.MyTextBox();
            this.TxtCompany = new V_Components.MyTextBox();
            this.TxtSupplier = new V_Components.MyTextBox();
            this.TxtEntryNo = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.Grid1);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.DtpIssue);
            this.GBMain.Controls.Add(this.label9);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.TxtBillType);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.BtnOK);
            this.GBMain.Controls.Add(this.BtnCancel);
            this.GBMain.Controls.Add(this.BtnExit);
            this.GBMain.Controls.Add(this.Arrow2);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.TxtCompany);
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.TxtSupplier);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtEntryNo);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(6, 0);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(460, 505);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // DtpIssue
            // 
            this.DtpIssue.Enabled = false;
            this.DtpIssue.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpIssue.Location = new System.Drawing.Point(355, 65);
            this.DtpIssue.Name = "DtpIssue";
            this.DtpIssue.Size = new System.Drawing.Size(94, 21);
            this.DtpIssue.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(269, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 31);
            this.label9.TabIndex = 72;
            this.label9.Text = "BILL ISSUE DATE";
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down;
            this.Arrow3.Location = new System.Drawing.Point(231, 64);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(29, 22);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 71;
            this.Arrow3.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 70;
            this.label8.Text = "BILL TYPE";
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(355, 24);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(94, 21);
            this.DtpDate.TabIndex = 1;
            // 
            // BtnOK
            // 
            this.BtnOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnOK.Location = new System.Drawing.Point(358, 375);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(91, 36);
            this.BtnOK.TabIndex = 8;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancel.Location = new System.Drawing.Point(358, 417);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(90, 36);
            this.BtnCancel.TabIndex = 9;
            this.BtnCancel.Text = "CANCEL";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnExit.Location = new System.Drawing.Point(358, 459);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(90, 36);
            this.BtnExit.TabIndex = 10;
            this.BtnExit.Text = "EXIT";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // Arrow2
            // 
            this.Arrow2.Image = global::Branch.Properties.Resources.Down;
            this.Arrow2.Location = new System.Drawing.Point(420, 144);
            this.Arrow2.Name = "Arrow2";
            this.Arrow2.Size = new System.Drawing.Size(29, 22);
            this.Arrow2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow2.TabIndex = 67;
            this.Arrow2.TabStop = false;
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Branch.Properties.Resources.Down;
            this.Arrow1.Location = new System.Drawing.Point(231, 104);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(29, 22);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 66;
            this.Arrow1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 109);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "COMPANY";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "SUPPLIER";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(269, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "DATE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ENTRY NO";
            // 
            // Grid1
            // 
            this.Grid1.AllowUserToAddRows = false;
            this.Grid1.AllowUserToDeleteRows = false;
            this.Grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid1.Location = new System.Drawing.Point(9, 375);
            this.Grid1.Name = "Grid1";
            this.Grid1.Size = new System.Drawing.Size(334, 120);
            this.Grid1.TabIndex = 73;
            this.Grid1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid1_RowsAdded);
            // 
            // Grid
            // 
            this.Grid.AllowUserToOrderColumns = true;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(9, 180);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(440, 187);
            this.Grid.TabIndex = 5;
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.CurrentCellChanged += new System.EventHandler(this.Grid_CurrentCellChanged);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // TxtBillType
            // 
            this.TxtBillType.Location = new System.Drawing.Point(100, 65);
            this.TxtBillType.Name = "TxtBillType";
            this.TxtBillType.Size = new System.Drawing.Size(123, 21);
            this.TxtBillType.TabIndex = 0;
            this.TxtBillType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtCompany
            // 
            this.TxtCompany.Location = new System.Drawing.Point(100, 105);
            this.TxtCompany.Name = "TxtCompany";
            this.TxtCompany.Size = new System.Drawing.Size(123, 21);
            this.TxtCompany.TabIndex = 1;
            this.TxtCompany.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtSupplier
            // 
            this.TxtSupplier.Location = new System.Drawing.Point(100, 145);
            this.TxtSupplier.Name = "TxtSupplier";
            this.TxtSupplier.Size = new System.Drawing.Size(314, 21);
            this.TxtSupplier.TabIndex = 2;
            this.TxtSupplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtEntryNo
            // 
            this.TxtEntryNo.Location = new System.Drawing.Point(100, 24);
            this.TxtEntryNo.Name = "TxtEntryNo";
            this.TxtEntryNo.Size = new System.Drawing.Size(87, 21);
            this.TxtEntryNo.TabIndex = 11;
            this.TxtEntryNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmFit_Bill_Entry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 512);
            this.Controls.Add(this.GBMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "FrmFit_Bill_Entry";
            this.Text = "Bill Entry...!";
            this.Load += new System.EventHandler(this.FrmFit_Bill_Entry_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmFit_Bill_Entry_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmFit_Bill_Entry_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private V_Components.MyTextBox TxtEntryNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private V_Components.MyTextBox TxtSupplier;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtCompany;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox Arrow2;
        private System.Windows.Forms.PictureBox Arrow1;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.PictureBox Arrow3;
        private V_Components.MyTextBox TxtBillType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker DtpIssue;
        private System.Windows.Forms.Label label9;
        private DotnetVFGrid.MyDataGridView Grid;
        private DotnetVFGrid.MyDataGridView Grid1;
    }
}