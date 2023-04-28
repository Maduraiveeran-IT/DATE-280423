namespace Accounts
{
    partial class FrmContractRate
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
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.DtpEDate = new System.Windows.Forms.DateTimePicker();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.TxtTotal = new V_Components.MyTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtProcess = new V_Components.MyTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.DtpEDate);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.TxtProcess);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Location = new System.Drawing.Point(5, 3);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(409, 364);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(18, 48);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(369, 273);
            this.Grid.TabIndex = 2;
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // DtpEDate
            // 
            this.DtpEDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpEDate.Location = new System.Drawing.Point(306, 19);
            this.DtpEDate.Name = "DtpEDate";
            this.DtpEDate.Size = new System.Drawing.Size(81, 20);
            this.DtpEDate.TabIndex = 1;
            this.DtpEDate.TabStop = false;
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Branch.Properties.Resources.Down;
            this.Arrow1.Location = new System.Drawing.Point(275, 19);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(25, 21);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 55;
            this.Arrow1.TabStop = false;
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(328, 327);
            this.TxtTotal.Multiline = true;
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(59, 20);
            this.TxtTotal.TabIndex = 3;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(272, 332);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "TOTAL";
            // 
            // TxtProcess
            // 
            this.TxtProcess.Location = new System.Drawing.Point(102, 20);
            this.TxtProcess.Name = "TxtProcess";
            this.TxtProcess.Size = new System.Drawing.Size(168, 20);
            this.TxtProcess.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(15, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "PROCESS";
            // 
            // FrmContractRate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 379);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmContractRate";
            this.Text = "CONTRACT RATE DETAILS";
            this.Load += new System.EventHandler(this.FrmContractRate_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmContractRate_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmContractRate_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private DotnetVFGrid.MyDataGridView Grid;
        private V_Components.MyTextBox TxtProcess;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker DtpEDate;
        private System.Windows.Forms.PictureBox Arrow1;
        private V_Components.MyTextBox TxtTotal;
        private System.Windows.Forms.Label label7;
    }
}