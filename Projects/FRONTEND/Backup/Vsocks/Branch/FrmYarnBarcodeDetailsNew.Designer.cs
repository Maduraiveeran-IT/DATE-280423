namespace Accounts
{
    partial class FrmYarnBarcodeDetailsNew
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
            this.label2 = new System.Windows.Forms.Label();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.Txt_Barcode = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "BARCODE";
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.Txt_Barcode);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Location = new System.Drawing.Point(6, 8);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(698, 389);
            this.GBMain.TabIndex = 2;
            this.GBMain.TabStop = false;
            // 
            // Txt_Barcode
            // 
            this.Txt_Barcode.Location = new System.Drawing.Point(74, 22);
            this.Txt_Barcode.Name = "Txt_Barcode";
            this.Txt_Barcode.Size = new System.Drawing.Size(172, 20);
            this.Txt_Barcode.TabIndex = 1;
            this.Txt_Barcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(12, 58);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(678, 325);
            this.Grid.TabIndex = 2;
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(550, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "DATE";
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(592, 22);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(98, 20);
            this.DtpDate.TabIndex = 0;
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Branch.Properties.Resources.Down;
            this.Arrow1.Location = new System.Drawing.Point(252, 22);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(22, 21);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 30;
            this.Arrow1.TabStop = false;
            // 
            // FrmYarnBarcodeDetailsNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 403);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmYarnBarcodeDetailsNew";
            this.Text = "FrmYarnBarcodeDetailsNew";
            this.Load += new System.EventHandler(this.FrmYarnBarcodeDetailsNew_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmYarnBarcodeDetailsNew_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmYarnBarcodeDetailsNew_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Arrow1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox GBMain;
        private V_Components.MyTextBox Txt_Barcode;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker DtpDate;
    }
}