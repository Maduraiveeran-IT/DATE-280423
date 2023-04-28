namespace Accounts
{
    partial class FrmRptOffSetSalesInvoiceReport
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.TxtInvoiceNO = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Arrow4 = new System.Windows.Forms.PictureBox();
            this.TxtOrderNo = new V_Components.MyTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ButExit = new System.Windows.Forms.Button();
            this.ButCancel = new System.Windows.Forms.Button();
            this.ButPrint = new System.Windows.Forms.Button();
            this.ButReport = new System.Windows.Forms.Button();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.pictureBox1);
            this.GBMain.Controls.Add(this.TxtInvoiceNO);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.Arrow4);
            this.GBMain.Controls.Add(this.TxtOrderNo);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.ButExit);
            this.GBMain.Controls.Add(this.ButCancel);
            this.GBMain.Controls.Add(this.ButPrint);
            this.GBMain.Controls.Add(this.ButReport);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Location = new System.Drawing.Point(5, 3);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(662, 441);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Branch.Properties.Resources.Down;
            this.pictureBox1.Location = new System.Drawing.Point(616, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(25, 21);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 70;
            this.pictureBox1.TabStop = false;
            // 
            // TxtInvoiceNO
            // 
            this.TxtInvoiceNO.Location = new System.Drawing.Point(434, 26);
            this.TxtInvoiceNO.Name = "TxtInvoiceNO";
            this.TxtInvoiceNO.Size = new System.Drawing.Size(176, 20);
            this.TxtInvoiceNO.TabIndex = 5;
            this.TxtInvoiceNO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(338, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 15);
            this.label2.TabIndex = 69;
            this.label2.Text = "INVOICE NO";
            // 
            // Arrow4
            // 
            this.Arrow4.Image = global::Branch.Properties.Resources.Down;
            this.Arrow4.Location = new System.Drawing.Point(277, 25);
            this.Arrow4.Name = "Arrow4";
            this.Arrow4.Size = new System.Drawing.Size(25, 21);
            this.Arrow4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow4.TabIndex = 67;
            this.Arrow4.TabStop = false;
            // 
            // TxtOrderNo
            // 
            this.TxtOrderNo.Location = new System.Drawing.Point(127, 26);
            this.TxtOrderNo.Name = "TxtOrderNo";
            this.TxtOrderNo.Size = new System.Drawing.Size(144, 20);
            this.TxtOrderNo.TabIndex = 0;
            this.TxtOrderNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(17, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 15);
            this.label6.TabIndex = 66;
            this.label6.Text = "ORDER NO";
            // 
            // ButExit
            // 
            this.ButExit.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButExit.Location = new System.Drawing.Point(566, 393);
            this.ButExit.Name = "ButExit";
            this.ButExit.Size = new System.Drawing.Size(75, 23);
            this.ButExit.TabIndex = 4;
            this.ButExit.Text = "EXIT";
            this.ButExit.UseVisualStyleBackColor = true;
            this.ButExit.Click += new System.EventHandler(this.ButExit_Click);
            // 
            // ButCancel
            // 
            this.ButCancel.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButCancel.Location = new System.Drawing.Point(485, 393);
            this.ButCancel.Name = "ButCancel";
            this.ButCancel.Size = new System.Drawing.Size(75, 23);
            this.ButCancel.TabIndex = 3;
            this.ButCancel.Text = "CANCEL";
            this.ButCancel.UseVisualStyleBackColor = true;
            this.ButCancel.Click += new System.EventHandler(this.ButCancel_Click);
            // 
            // ButPrint
            // 
            this.ButPrint.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButPrint.Location = new System.Drawing.Point(404, 393);
            this.ButPrint.Name = "ButPrint";
            this.ButPrint.Size = new System.Drawing.Size(75, 23);
            this.ButPrint.TabIndex = 2;
            this.ButPrint.Text = "PRINT";
            this.ButPrint.UseVisualStyleBackColor = true;
            this.ButPrint.Click += new System.EventHandler(this.ButPrint_Click);
            // 
            // ButReport
            // 
            this.ButReport.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButReport.Location = new System.Drawing.Point(323, 393);
            this.ButReport.Name = "ButReport";
            this.ButReport.Size = new System.Drawing.Size(75, 23);
            this.ButReport.TabIndex = 1;
            this.ButReport.Text = "REPORT";
            this.ButReport.UseVisualStyleBackColor = true;
            this.ButReport.Click += new System.EventHandler(this.ButReport_Click);
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(20, 61);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid.Size = new System.Drawing.Size(621, 317);
            this.Grid.TabIndex = 6;
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            // 
            // FrmRptOffSetSalesInvoiceReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 460);
            this.ControlBox = false;
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmRptOffSetSalesInvoiceReport";
            this.Text = "Stock  Report ...!";
            this.Load += new System.EventHandler(this.FrmRptOffSetSalesInvoiceReport_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmRptOffSetSalesInvoiceReport_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmRptOffSetSalesInvoiceReport_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Button ButReport;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.Button ButExit;
        private System.Windows.Forms.Button ButCancel;
        private System.Windows.Forms.Button ButPrint;
        private System.Windows.Forms.PictureBox Arrow4;
        private V_Components.MyTextBox TxtOrderNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private V_Components.MyTextBox TxtInvoiceNO;
        private System.Windows.Forms.Label label2;
    }
}