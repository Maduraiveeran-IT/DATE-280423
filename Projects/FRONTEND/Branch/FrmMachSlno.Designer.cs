namespace Accounts
{
    partial class FrmMachSlno
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
            this.GBSlno = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.Arrow2 = new System.Windows.Forms.PictureBox();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.GridSlno = new DotnetVFGrid.MyDataGridView();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.txtDiv = new V_Components.MyTextBox();
            this.txtCmp = new V_Components.MyTextBox();
            this.TxtSupplier = new V_Components.MyTextBox();
            this.TxtGrnNo = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            this.GBSlno.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridSlno)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.GBSlno);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.Arrow2);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.label11);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.label10);
            this.GBMain.Controls.Add(this.txtDiv);
            this.GBMain.Controls.Add(this.txtCmp);
            this.GBMain.Controls.Add(this.TxtSupplier);
            this.GBMain.Controls.Add(this.TxtGrnNo);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Location = new System.Drawing.Point(12, 12);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(775, 461);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // GBSlno
            // 
            this.GBSlno.Controls.Add(this.btnCancel);
            this.GBSlno.Controls.Add(this.btnOK);
            this.GBSlno.Controls.Add(this.GridSlno);
            this.GBSlno.Location = new System.Drawing.Point(88, 113);
            this.GBSlno.Name = "GBSlno";
            this.GBSlno.Size = new System.Drawing.Size(578, 324);
            this.GBSlno.TabIndex = 4;
            this.GBSlno.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(281, 295);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(200, 295);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Accounts.Properties.Resources.Down;
            this.Arrow1.Location = new System.Drawing.Point(223, 17);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(25, 21);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 124;
            this.Arrow1.TabStop = false;
            // 
            // Arrow2
            // 
            this.Arrow2.Image = global::Accounts.Properties.Resources.Down;
            this.Arrow2.Location = new System.Drawing.Point(314, 47);
            this.Arrow2.Name = "Arrow2";
            this.Arrow2.Size = new System.Drawing.Size(25, 21);
            this.Arrow2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow2.TabIndex = 124;
            this.Arrow2.TabStop = false;
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Accounts.Properties.Resources.Down;
            this.Arrow3.Location = new System.Drawing.Point(721, 47);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(25, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 124;
            this.Arrow3.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(12, 51);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 15);
            this.label11.TabIndex = 6;
            this.label11.Text = "COMPANY";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(343, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "DIVISION";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(343, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 15);
            this.label10.TabIndex = 8;
            this.label10.Text = "SUPPLIER";
            // 
            // DtpDate
            // 
            this.DtpDate.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(254, 17);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(85, 22);
            this.DtpDate.TabIndex = 7;
            this.DtpDate.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(19, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "GRNNO";
            // 
            // GridSlno
            // 
            this.GridSlno.AllowUserToAddRows = false;
            this.GridSlno.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridSlno.Location = new System.Drawing.Point(6, 19);
            this.GridSlno.Name = "GridSlno";
            this.GridSlno.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.GridSlno.Size = new System.Drawing.Size(566, 270);
            this.GridSlno.TabIndex = 0;
            this.GridSlno.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.GridSlno_EditingControlShowing);
            this.GridSlno.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.GridSlno_RowsAdded);
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(6, 85);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid.Size = new System.Drawing.Size(740, 364);
            this.Grid.TabIndex = 3;
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            // 
            // txtDiv
            // 
            this.txtDiv.Location = new System.Drawing.Point(416, 47);
            this.txtDiv.Name = "txtDiv";
            this.txtDiv.Size = new System.Drawing.Size(299, 20);
            this.txtDiv.TabIndex = 2;
            this.txtDiv.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCmp
            // 
            this.txtCmp.Location = new System.Drawing.Point(88, 48);
            this.txtCmp.Name = "txtCmp";
            this.txtCmp.Size = new System.Drawing.Size(220, 20);
            this.txtCmp.TabIndex = 1;
            this.txtCmp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtSupplier
            // 
            this.TxtSupplier.Location = new System.Drawing.Point(416, 16);
            this.TxtSupplier.Name = "TxtSupplier";
            this.TxtSupplier.Size = new System.Drawing.Size(299, 20);
            this.TxtSupplier.TabIndex = 10;
            this.TxtSupplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtGrnNo
            // 
            this.TxtGrnNo.Location = new System.Drawing.Point(88, 17);
            this.TxtGrnNo.Name = "TxtGrnNo";
            this.TxtGrnNo.Size = new System.Drawing.Size(129, 20);
            this.TxtGrnNo.TabIndex = 0;
            this.TxtGrnNo.TabStop = false;
            this.TxtGrnNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmMachSlno
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 485);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmMachSlno";
            this.Text = "FrmMachSlno";
            this.Load += new System.EventHandler(this.FrmMachSlno_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMachSlno_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmMachSlno_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.GBSlno.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridSlno)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.PictureBox Arrow2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private V_Components.MyTextBox txtCmp;
        private V_Components.MyTextBox TxtSupplier;
        private V_Components.MyTextBox TxtGrnNo;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox Arrow1;
        private System.Windows.Forms.PictureBox Arrow3;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox txtDiv;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.GroupBox GBSlno;
        private DotnetVFGrid.MyDataGridView GridSlno;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}