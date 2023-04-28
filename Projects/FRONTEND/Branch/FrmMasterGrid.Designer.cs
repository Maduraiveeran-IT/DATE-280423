namespace Accounts
{
    partial class FrmMasterGrid
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
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtusername = new System.Windows.Forms.TextBox();
            this.textGridList = new System.Windows.Forms.TextBox();
            this.dataGrid = new DotnetVFGrid.MyDataGridView();
            this.textRowsCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.textRowsCount);
            this.GBMain.Controls.Add(this.textGridList);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.dataGrid);
            this.GBMain.Controls.Add(this.BtnExit);
            this.GBMain.Controls.Add(this.BtnClear);
            this.GBMain.Controls.Add(this.BtnSave);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.txtusername);
            this.GBMain.Location = new System.Drawing.Point(12, 12);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(500, 541);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            this.GBMain.Text = "MasterGrid";
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Accounts.Properties.Resources.Down;
            this.Arrow1.Location = new System.Drawing.Point(408, 118);
            this.Arrow1.Margin = new System.Windows.Forms.Padding(4);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(33, 22);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 125;
            this.Arrow1.TabStop = false;
            this.Arrow1.Click += new System.EventHandler(this.Arrow1_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Location = new System.Drawing.Point(366, 466);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(75, 23);
            this.BtnExit.TabIndex = 4;
            this.BtnExit.Text = "Exit";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(233, 466);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(75, 23);
            this.BtnClear.TabIndex = 3;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(96, 466);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 2;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(102, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "User Name";
            // 
            // txtusername
            // 
            this.txtusername.Location = new System.Drawing.Point(208, 113);
            this.txtusername.Name = "txtusername";
            this.txtusername.Size = new System.Drawing.Size(193, 22);
            this.txtusername.TabIndex = 0;
            // 
            // textGridList
            // 
            this.textGridList.Location = new System.Drawing.Point(96, 41);
            this.textGridList.Name = "textGridList";
            this.textGridList.Size = new System.Drawing.Size(345, 22);
            this.textGridList.TabIndex = 126;
            // 
            // dataGrid
            // 
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Location = new System.Drawing.Point(96, 162);
            this.dataGrid.Margin = new System.Windows.Forms.Padding(4);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGrid.Size = new System.Drawing.Size(346, 288);
            this.dataGrid.TabIndex = 5;
            this.dataGrid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGrid_EditingControlShowing);
            this.dataGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGrid_KeyDown);
            this.dataGrid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGrid_MouseDoubleClick);
            // 
            // textRowsCount
            // 
            this.textRowsCount.Location = new System.Drawing.Point(342, 513);
            this.textRowsCount.Name = "textRowsCount";
            this.textRowsCount.Size = new System.Drawing.Size(100, 22);
            this.textRowsCount.TabIndex = 127;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(246, 516);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 17);
            this.label2.TabIndex = 128;
            this.label2.Text = "Rows Count";
            // 
            // FrmMasterGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 579);
            this.ControlBox = false;
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmMasterGrid";
            this.Text = "FrmMasterGrid";
            this.Load += new System.EventHandler(this.FrmMasterGrid_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMasterGrid_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmMasterGrid_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtusername;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button BtnSave;
        private DotnetVFGrid.MyDataGridView dataGrid;
        private System.Windows.Forms.PictureBox Arrow1;
        private System.Windows.Forms.TextBox textGridList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textRowsCount;
    }
}