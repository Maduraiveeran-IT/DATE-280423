namespace Accounts
{
    partial class FrmTestingPo
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
            this.Arrow2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtSupplier = new V_Components.MyTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtTotAmnt = new V_Components.MyTextBox();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtENo = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.Arrow2);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.TxtSupplier);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtTotAmnt);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtENo);
            this.GBMain.Location = new System.Drawing.Point(8, 9);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(501, 289);
            this.GBMain.TabIndex = 4;
            this.GBMain.TabStop = false;
            // 
            // Arrow2
            // 
            this.Arrow2.Image = global::Accounts.Properties.Resources.Down;
            this.Arrow2.Location = new System.Drawing.Point(380, 41);
            this.Arrow2.Name = "Arrow2";
            this.Arrow2.Size = new System.Drawing.Size(25, 21);
            this.Arrow2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow2.TabIndex = 96;
            this.Arrow2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Party";
            // 
            // TxtSupplier
            // 
            this.TxtSupplier.Location = new System.Drawing.Point(75, 41);
            this.TxtSupplier.Name = "TxtSupplier";
            this.TxtSupplier.Size = new System.Drawing.Size(304, 20);
            this.TxtSupplier.TabIndex = 3;
            this.TxtSupplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(369, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "TOTAL";
            // 
            // TxtTotAmnt
            // 
            this.TxtTotAmnt.Location = new System.Drawing.Point(419, 197);
            this.TxtTotAmnt.Name = "TxtTotAmnt";
            this.TxtTotAmnt.Size = new System.Drawing.Size(69, 20);
            this.TxtTotAmnt.TabIndex = 10;
            this.TxtTotAmnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(4, 199);
            this.TxtRemarks.Multiline = true;
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(359, 41);
            this.TxtRemarks.TabIndex = 8;
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(6, 71);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(482, 120);
            this.Grid.TabIndex = 7;
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(246, 14);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(79, 20);
            this.DtpDate.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Entry No";
            // 
            // TxtENo
            // 
            this.TxtENo.Location = new System.Drawing.Point(75, 14);
            this.TxtENo.Name = "TxtENo";
            this.TxtENo.Size = new System.Drawing.Size(91, 20);
            this.TxtENo.TabIndex = 0;
            this.TxtENo.TabStop = false;
            this.TxtENo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 97;
            this.label2.Text = "Entry Date";
            // 
            // FrmTestingPo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 259);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmTestingPo";
            this.Text = "FrmTestingPo";
            this.Load += new System.EventHandler(this.FrmTestingPo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmTestingPo_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmTestingPo_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.PictureBox Arrow2;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox TxtSupplier;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtTotAmnt;
        private V_Components.MyTextBox TxtRemarks;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtENo;
        private System.Windows.Forms.Label label2;
    }
}