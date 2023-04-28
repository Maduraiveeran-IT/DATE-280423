namespace Accounts
{
    partial class FrmSampleReqEntry
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
            this.label1 = new System.Windows.Forms.Label();
            this.DtpRDate = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.DtpODate = new System.Windows.Forms.DateTimePicker();
            this.Arrow_Buyer = new System.Windows.Forms.PictureBox();
            this.Arrow_Merch = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.LblMerch = new System.Windows.Forms.Label();
            this.TxtOCNNo = new V_Components.MyTextBox();
            this.TxtBuyer = new V_Components.MyTextBox();
            this.TxtMerch = new V_Components.MyTextBox();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.TxtTotalBom = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow_Buyer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow_Merch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.DtpRDate);
            this.GBMain.Controls.Add(this.label13);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.DtpODate);
            this.GBMain.Controls.Add(this.Arrow_Buyer);
            this.GBMain.Controls.Add(this.Arrow_Merch);
            this.GBMain.Controls.Add(this.label10);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.LblMerch);
            this.GBMain.Controls.Add(this.TxtOCNNo);
            this.GBMain.Controls.Add(this.TxtBuyer);
            this.GBMain.Controls.Add(this.TxtMerch);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.TxtTotalBom);
            this.GBMain.Location = new System.Drawing.Point(6, 6);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(601, 385);
            this.GBMain.TabIndex = 1;
            this.GBMain.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(398, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 15);
            this.label1.TabIndex = 80;
            this.label1.Text = "REQ DATE";
            // 
            // DtpRDate
            // 
            this.DtpRDate.CustomFormat = "dd/MM/yyyy";
            this.DtpRDate.Enabled = false;
            this.DtpRDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpRDate.Location = new System.Drawing.Point(471, 16);
            this.DtpRDate.Name = "DtpRDate";
            this.DtpRDate.Size = new System.Drawing.Size(89, 20);
            this.DtpRDate.TabIndex = 79;
            this.DtpRDate.TabStop = false;
            this.DtpRDate.Value = new System.DateTime(2013, 10, 19, 0, 0, 0, 0);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(247, 19);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(39, 15);
            this.label13.TabIndex = 78;
            this.label13.Text = "DATE";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(21, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 15);
            this.label5.TabIndex = 78;
            this.label5.Text = "ENTRY NO";
            // 
            // DtpODate
            // 
            this.DtpODate.CustomFormat = "dd/MM/yyyy";
            this.DtpODate.Enabled = false;
            this.DtpODate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpODate.Location = new System.Drawing.Point(302, 16);
            this.DtpODate.Name = "DtpODate";
            this.DtpODate.Size = new System.Drawing.Size(89, 20);
            this.DtpODate.TabIndex = 0;
            this.DtpODate.TabStop = false;
            this.DtpODate.Value = new System.DateTime(2013, 10, 19, 0, 0, 0, 0);
            // 
            // Arrow_Buyer
            // 
            this.Arrow_Buyer.Image = global::Branch.Properties.Resources.Down;
            this.Arrow_Buyer.Location = new System.Drawing.Point(561, 46);
            this.Arrow_Buyer.Name = "Arrow_Buyer";
            this.Arrow_Buyer.Size = new System.Drawing.Size(25, 21);
            this.Arrow_Buyer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow_Buyer.TabIndex = 75;
            this.Arrow_Buyer.TabStop = false;
            // 
            // Arrow_Merch
            // 
            this.Arrow_Merch.Image = global::Branch.Properties.Resources.Down;
            this.Arrow_Merch.Location = new System.Drawing.Point(219, 46);
            this.Arrow_Merch.Name = "Arrow_Merch";
            this.Arrow_Merch.Size = new System.Drawing.Size(25, 21);
            this.Arrow_Merch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow_Merch.TabIndex = 75;
            this.Arrow_Merch.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(21, 355);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 15);
            this.label10.TabIndex = 56;
            this.label10.Text = "REMARKS";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(417, 355);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 15);
            this.label4.TabIndex = 56;
            this.label4.Text = "TOTAL";
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(24, 79);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid.Size = new System.Drawing.Size(536, 261);
            this.Grid.TabIndex = 7;
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(246, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "BUYER";
            // 
            // LblMerch
            // 
            this.LblMerch.AutoSize = true;
            this.LblMerch.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblMerch.Location = new System.Drawing.Point(21, 48);
            this.LblMerch.Name = "LblMerch";
            this.LblMerch.Size = new System.Drawing.Size(52, 15);
            this.LblMerch.TabIndex = 15;
            this.LblMerch.Text = "MERCH";
            // 
            // TxtOCNNo
            // 
            this.TxtOCNNo.Location = new System.Drawing.Point(97, 16);
            this.TxtOCNNo.Name = "TxtOCNNo";
            this.TxtOCNNo.Size = new System.Drawing.Size(121, 20);
            this.TxtOCNNo.TabIndex = 0;
            this.TxtOCNNo.TabStop = false;
            this.TxtOCNNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtBuyer
            // 
            this.TxtBuyer.Location = new System.Drawing.Point(302, 46);
            this.TxtBuyer.Name = "TxtBuyer";
            this.TxtBuyer.Size = new System.Drawing.Size(258, 20);
            this.TxtBuyer.TabIndex = 0;
            this.TxtBuyer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtMerch
            // 
            this.TxtMerch.Location = new System.Drawing.Point(97, 46);
            this.TxtMerch.Name = "TxtMerch";
            this.TxtMerch.Size = new System.Drawing.Size(121, 20);
            this.TxtMerch.TabIndex = 1;
            this.TxtMerch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(97, 352);
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(294, 20);
            this.TxtRemarks.TabIndex = 8;
            // 
            // TxtTotalBom
            // 
            this.TxtTotalBom.Location = new System.Drawing.Point(472, 352);
            this.TxtTotalBom.Name = "TxtTotalBom";
            this.TxtTotalBom.Size = new System.Drawing.Size(89, 20);
            this.TxtTotalBom.TabIndex = 9;
            this.TxtTotalBom.TabStop = false;
            this.TxtTotalBom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmSampleReqEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 397);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmSampleReqEntry";
            this.Text = "FrmSampleReqEntry";
            this.Load += new System.EventHandler(this.Frm_Socks_Dyeing_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Socks_Dyeing_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmSampleReqEntry_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow_Buyer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow_Merch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker DtpODate;
        private System.Windows.Forms.PictureBox Arrow_Buyer;
        private System.Windows.Forms.PictureBox Arrow_Merch;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label4;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LblMerch;
        private V_Components.MyTextBox TxtOCNNo;
        private V_Components.MyTextBox TxtBuyer;
        private V_Components.MyTextBox TxtMerch;
        private V_Components.MyTextBox TxtRemarks;
        private V_Components.MyTextBox TxtTotalBom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker DtpRDate;
    }
}