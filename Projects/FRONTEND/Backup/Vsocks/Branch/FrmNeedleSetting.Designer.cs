namespace Accounts
{
    partial class FrmNeedleSetting
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
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtWeek = new V_Components.MyTextBox();
            this.TxtYear = new V_Components.MyTextBox();
            this.TxtTot = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.listBox1);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.TxtWeek);
            this.GBMain.Controls.Add(this.TxtYear);
            this.GBMain.Controls.Add(this.TxtTot);
            this.GBMain.Location = new System.Drawing.Point(12, 12);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(317, 462);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Branch.Properties.Resources.Down;
            this.Arrow1.Location = new System.Drawing.Point(159, 17);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(25, 21);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 75;
            this.Arrow1.TabStop = false;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(21, 431);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(47, 17);
            this.listBox1.TabIndex = 74;
            this.listBox1.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(178, 433);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 15);
            this.label4.TabIndex = 56;
            this.label4.Text = "TOTAL ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "YEAR \\ WEEK";
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(233, 19);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(65, 20);
            this.DtpDate.TabIndex = 76;
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(20, 49);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid.Size = new System.Drawing.Size(278, 376);
            this.Grid.TabIndex = 2;
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // TxtWeek
            // 
            this.TxtWeek.Location = new System.Drawing.Point(187, 18);
            this.TxtWeek.Name = "TxtWeek";
            this.TxtWeek.Size = new System.Drawing.Size(42, 20);
            this.TxtWeek.TabIndex = 0;
            this.TxtWeek.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtYear
            // 
            this.TxtYear.Location = new System.Drawing.Point(107, 18);
            this.TxtYear.Name = "TxtYear";
            this.TxtYear.Size = new System.Drawing.Size(49, 20);
            this.TxtYear.TabIndex = 1;
            this.TxtYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtTot
            // 
            this.TxtTot.Location = new System.Drawing.Point(235, 431);
            this.TxtTot.Name = "TxtTot";
            this.TxtTot.Size = new System.Drawing.Size(63, 20);
            this.TxtTot.TabIndex = 3;
            this.TxtTot.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmNeedleSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 486);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmNeedleSetting";
            this.Text = "FrmNeedleSetting";
            this.Load += new System.EventHandler(this.FrmNeedleSetting_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmNeedleSetting_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmNeedleSetting_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private DotnetVFGrid.MyDataGridView Grid;
        private V_Components.MyTextBox TxtTot;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBox1;
        private V_Components.MyTextBox TxtYear;
        private System.Windows.Forms.PictureBox Arrow1;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtWeek;
        private System.Windows.Forms.DateTimePicker DtpDate;
    }
}