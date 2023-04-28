namespace Accounts
{
    partial class FrmGridBigs
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
            this.TxtOcn = new V_Components.MyTextBox();
            this.Arrow = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtRep = new System.Windows.Forms.TextBox();
            this.BtnCsv = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.Arrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.TxtOcn);
            this.GBMain.Controls.Add(this.Arrow);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.TxtRep);
            this.GBMain.Controls.Add(this.BtnCsv);
            this.GBMain.Controls.Add(this.BtnExport);
            this.GBMain.Controls.Add(this.btnExit);
            this.GBMain.Controls.Add(this.btnCancel);
            this.GBMain.Controls.Add(this.btnReport);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.DTTo);
            this.GBMain.Controls.Add(this.DTFrom);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(4, 5);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(766, 478);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(266, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 52;
            this.label4.Text = "Ocn / No";
            // 
            // TxtOcn
            // 
            this.TxtOcn.Location = new System.Drawing.Point(315, 41);
            this.TxtOcn.Name = "TxtOcn";
            this.TxtOcn.Size = new System.Drawing.Size(291, 20);
            this.TxtOcn.TabIndex = 51;
            // 
            // Arrow
            // 
            this.Arrow.Image = global::Accounts.Properties.Resources.Down;
            this.Arrow.Location = new System.Drawing.Point(607, 15);
            this.Arrow.Name = "Arrow";
            this.Arrow.Size = new System.Drawing.Size(25, 21);
            this.Arrow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow.TabIndex = 50;
            this.Arrow.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(266, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Report";
            // 
            // TxtRep
            // 
            this.TxtRep.Location = new System.Drawing.Point(315, 15);
            this.TxtRep.Name = "TxtRep";
            this.TxtRep.Size = new System.Drawing.Size(291, 20);
            this.TxtRep.TabIndex = 5;
            // 
            // BtnCsv
            // 
            this.BtnCsv.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCsv.Location = new System.Drawing.Point(430, 439);
            this.BtnCsv.Name = "BtnCsv";
            this.BtnCsv.Size = new System.Drawing.Size(78, 33);
            this.BtnCsv.TabIndex = 8;
            this.BtnCsv.Text = "Csv";
            this.BtnCsv.UseVisualStyleBackColor = true;
            this.BtnCsv.Visible = false;
            // 
            // BtnExport
            // 
            this.BtnExport.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnExport.Location = new System.Drawing.Point(514, 439);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(78, 33);
            this.BtnExport.TabIndex = 9;
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
            this.btnExit.TabIndex = 11;
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
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnReport
            // 
            this.btnReport.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReport.Location = new System.Drawing.Point(346, 439);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(78, 33);
            this.btnReport.TabIndex = 7;
            this.btnReport.Text = "Report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(7, 67);
            this.Grid.Name = "Grid";
            this.Grid.ReadOnly = true;
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid.Size = new System.Drawing.Size(753, 365);
            this.Grid.TabIndex = 6;
            this.Grid.TabStop = false;
            // 
            // DTTo
            // 
            this.DTTo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DTTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTTo.Location = new System.Drawing.Point(167, 14);
            this.DTTo.Name = "DTTo";
            this.DTTo.Size = new System.Drawing.Size(93, 21);
            this.DTTo.TabIndex = 3;
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
            this.label2.TabIndex = 2;
            this.label2.Text = "To";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "From";
            // 
            // FrmGridBigs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 485);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmGridBigs";
            this.Text = "FrmGridCombined";
            this.Load += new System.EventHandler(this.FrmGridBigs_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmGridBigs_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmGridBigs_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow)).EndInit();
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
        private System.Windows.Forms.Button BtnCsv;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtRep;
        private System.Windows.Forms.PictureBox Arrow;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtOcn;
    }
}