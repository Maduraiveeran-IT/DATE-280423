namespace Accounts
{
    partial class FrmGridPo
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
            this.OptPerf = new System.Windows.Forms.RadioButton();
            this.OptGrnPenOcn = new System.Windows.Forms.RadioButton();
            this.OptLot = new System.Windows.Forms.RadioButton();
            this.OptAll = new System.Windows.Forms.RadioButton();
            this.OptGrnPen = new System.Windows.Forms.RadioButton();
            this.OptPoOcn = new System.Windows.Forms.RadioButton();
            this.OptPoPen = new System.Windows.Forms.RadioButton();
            this.BtnExport = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.Grid = new System.Windows.Forms.DataGridView();
            this.DTTo = new System.Windows.Forms.DateTimePicker();
            this.DTFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.OptPerf);
            this.GBMain.Controls.Add(this.OptGrnPenOcn);
            this.GBMain.Controls.Add(this.OptLot);
            this.GBMain.Controls.Add(this.OptAll);
            this.GBMain.Controls.Add(this.OptGrnPen);
            this.GBMain.Controls.Add(this.OptPoOcn);
            this.GBMain.Controls.Add(this.OptPoPen);
            this.GBMain.Controls.Add(this.BtnExport);
            this.GBMain.Controls.Add(this.btnExit);
            this.GBMain.Controls.Add(this.btnCancel);
            this.GBMain.Controls.Add(this.btnReport);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.DTTo);
            this.GBMain.Controls.Add(this.DTFrom);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(12, 12);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(766, 478);
            this.GBMain.TabIndex = 2;
            this.GBMain.TabStop = false;
            // 
            // OptPerf
            // 
            this.OptPerf.AutoSize = true;
            this.OptPerf.Location = new System.Drawing.Point(531, 46);
            this.OptPerf.Name = "OptPerf";
            this.OptPerf.Size = new System.Drawing.Size(101, 17);
            this.OptPerf.TabIndex = 19;
            this.OptPerf.TabStop = true;
            this.OptPerf.Text = "Po Performance";
            this.OptPerf.UseVisualStyleBackColor = true;
            // 
            // OptGrnPenOcn
            // 
            this.OptGrnPenOcn.AutoSize = true;
            this.OptGrnPenOcn.Location = new System.Drawing.Point(327, 46);
            this.OptGrnPenOcn.Name = "OptGrnPenOcn";
            this.OptGrnPenOcn.Size = new System.Drawing.Size(121, 17);
            this.OptGrnPenOcn.TabIndex = 18;
            this.OptGrnPenOcn.TabStop = true;
            this.OptGrnPenOcn.Text = "Grn Pending Activity";
            this.OptGrnPenOcn.UseVisualStyleBackColor = true;
            // 
            // OptLot
            // 
            this.OptLot.AutoSize = true;
            this.OptLot.Location = new System.Drawing.Point(644, 46);
            this.OptLot.Name = "OptLot";
            this.OptLot.Size = new System.Drawing.Size(77, 17);
            this.OptLot.TabIndex = 17;
            this.OptLot.TabStop = true;
            this.OptLot.Text = "Grn Details";
            this.OptLot.UseVisualStyleBackColor = true;
            // 
            // OptAll
            // 
            this.OptAll.AutoSize = true;
            this.OptAll.Location = new System.Drawing.Point(447, 46);
            this.OptAll.Name = "OptAll";
            this.OptAll.Size = new System.Drawing.Size(71, 17);
            this.OptAll.TabIndex = 16;
            this.OptAll.TabStop = true;
            this.OptAll.Text = "Po Status";
            this.OptAll.UseVisualStyleBackColor = true;
            // 
            // OptGrnPen
            // 
            this.OptGrnPen.AutoSize = true;
            this.OptGrnPen.Location = new System.Drawing.Point(226, 46);
            this.OptGrnPen.Name = "OptGrnPen";
            this.OptGrnPen.Size = new System.Drawing.Size(84, 17);
            this.OptGrnPen.TabIndex = 15;
            this.OptGrnPen.TabStop = true;
            this.OptGrnPen.Text = "Grn Pending";
            this.OptGrnPen.UseVisualStyleBackColor = true;
            // 
            // OptPoOcn
            // 
            this.OptPoOcn.AutoSize = true;
            this.OptPoOcn.Location = new System.Drawing.Point(104, 46);
            this.OptPoOcn.Name = "OptPoOcn";
            this.OptPoOcn.Size = new System.Drawing.Size(117, 17);
            this.OptPoOcn.TabIndex = 14;
            this.OptPoOcn.TabStop = true;
            this.OptPoOcn.Text = "Po Pending Activity";
            this.OptPoOcn.UseVisualStyleBackColor = true;
            // 
            // OptPoPen
            // 
            this.OptPoPen.AutoSize = true;
            this.OptPoPen.Location = new System.Drawing.Point(9, 46);
            this.OptPoPen.Name = "OptPoPen";
            this.OptPoPen.Size = new System.Drawing.Size(80, 17);
            this.OptPoPen.TabIndex = 13;
            this.OptPoPen.TabStop = true;
            this.OptPoPen.Text = "Po Pending";
            this.OptPoPen.UseVisualStyleBackColor = true;
            // 
            // BtnExport
            // 
            this.BtnExport.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnExport.Location = new System.Drawing.Point(514, 439);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(78, 33);
            this.BtnExport.TabIndex = 4;
            this.BtnExport.Text = "Export";
            this.BtnExport.UseVisualStyleBackColor = true;
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(682, 439);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(78, 33);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(598, 439);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 33);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnReport
            // 
            this.btnReport.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReport.Location = new System.Drawing.Point(430, 439);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(78, 33);
            this.btnReport.TabIndex = 3;
            this.btnReport.Text = "Report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(7, 69);
            this.Grid.Name = "Grid";
            this.Grid.ReadOnly = true;
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid.Size = new System.Drawing.Size(753, 364);
            this.Grid.TabIndex = 2;
            this.Grid.TabStop = false;
            // 
            // DTTo
            // 
            this.DTTo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DTTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTTo.Location = new System.Drawing.Point(167, 14);
            this.DTTo.Name = "DTTo";
            this.DTTo.Size = new System.Drawing.Size(93, 21);
            this.DTTo.TabIndex = 2;
            // 
            // DTFrom
            // 
            this.DTFrom.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DTFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTFrom.Location = new System.Drawing.Point(44, 14);
            this.DTFrom.Name = "DTFrom";
            this.DTFrom.Size = new System.Drawing.Size(94, 21);
            this.DTFrom.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(143, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "To";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "From";
            // 
            // FrmGridPo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 502);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmGridPo";
            this.Text = "FrmGridPoFabric";
            this.Load += new System.EventHandler(this.FrmGridPo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmGridPo_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmGridPo_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Button BtnExport;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.DataGridView Grid;
        private System.Windows.Forms.DateTimePicker DTTo;
        private System.Windows.Forms.DateTimePicker DTFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton OptLot;
        private System.Windows.Forms.RadioButton OptAll;
        private System.Windows.Forms.RadioButton OptGrnPen;
        private System.Windows.Forms.RadioButton OptPoOcn;
        private System.Windows.Forms.RadioButton OptPoPen;
        private System.Windows.Forms.RadioButton OptGrnPenOcn;
        private System.Windows.Forms.RadioButton OptPerf;
    }
}