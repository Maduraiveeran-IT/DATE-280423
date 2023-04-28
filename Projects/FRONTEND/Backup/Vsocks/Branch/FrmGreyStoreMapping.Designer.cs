namespace Accounts
{
    partial class FrmGreyStoreMapping
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
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LblSpecial = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.DtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.TxtBarcode = new V_Components.MyTextBox();
            this.TxtTotal = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtNo = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.TxtBarcode);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.LblSpecial);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.DtpDate1);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtNo);
            this.GBMain.Location = new System.Drawing.Point(-1, 3);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(373, 458);
            this.GBMain.TabIndex = 1;
            this.GBMain.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 435);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "REMARKS";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "GREY STORE BARCODE";
            // 
            // LblSpecial
            // 
            this.LblSpecial.BackColor = System.Drawing.Color.Transparent;
            this.LblSpecial.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSpecial.Location = new System.Drawing.Point(9, 16);
            this.LblSpecial.Name = "LblSpecial";
            this.LblSpecial.Size = new System.Drawing.Size(353, 26);
            this.LblSpecial.TabIndex = 35;
            this.LblSpecial.Text = "GREY STORE BARCODE MAPPING";
            this.LblSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(237, 435);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "TOTAL";
            // 
            // DtpDate1
            // 
            this.DtpDate1.Enabled = false;
            this.DtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate1.Location = new System.Drawing.Point(275, 51);
            this.DtpDate1.Name = "DtpDate1";
            this.DtpDate1.Size = new System.Drawing.Size(87, 20);
            this.DtpDate1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(237, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "DATE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "#ENTRY NO";
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(76, 432);
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(155, 20);
            this.TxtRemarks.TabIndex = 4;
            // 
            // TxtBarcode
            // 
            this.TxtBarcode.Location = new System.Drawing.Point(129, 85);
            this.TxtBarcode.Name = "TxtBarcode";
            this.TxtBarcode.Size = new System.Drawing.Size(233, 20);
            this.TxtBarcode.TabIndex = 2;
            this.TxtBarcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(285, 432);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(77, 20);
            this.TxtTotal.TabIndex = 5;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(12, 111);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(350, 313);
            this.Grid.TabIndex = 3;
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // TxtNo
            // 
            this.TxtNo.Location = new System.Drawing.Point(129, 51);
            this.TxtNo.Name = "TxtNo";
            this.TxtNo.Size = new System.Drawing.Size(101, 20);
            this.TxtNo.TabIndex = 0;
            this.TxtNo.TabStop = false;
            this.TxtNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmGreyStoreMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 467);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmGreyStoreMapping";
            this.Text = "FrmGreyStoreMapping";
            this.Load += new System.EventHandler(this.FrmGreyStoreMapping_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmGreyStoreMapping_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmGreyStoreMapping_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label LblSpecial;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtTotal;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.DateTimePicker DtpDate1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtNo;
        private V_Components.MyTextBox TxtBarcode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtRemarks;
    }
}