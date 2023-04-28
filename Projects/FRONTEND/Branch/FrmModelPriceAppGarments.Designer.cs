namespace Accounts
{
    partial class FrmModelPriceAppGarments
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmModelPriceAppGarments));
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.ButClear = new System.Windows.Forms.Button();
            this.ButExit = new System.Windows.Forms.Button();
            this.ButApprove = new System.Windows.Forms.Button();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.checkBox1);
            this.GBMain.Controls.Add(this.ButClear);
            this.GBMain.Controls.Add(this.ButExit);
            this.GBMain.Controls.Add(this.ButApprove);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.TxtRemarks);
            resources.ApplyResources(this.GBMain, "GBMain");
            this.GBMain.Name = "GBMain";
            this.GBMain.TabStop = false;
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.TabStop = false;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // ButClear
            // 
            resources.ApplyResources(this.ButClear, "ButClear");
            this.ButClear.Name = "ButClear";
            this.ButClear.UseVisualStyleBackColor = true;
            this.ButClear.Click += new System.EventHandler(this.ButClear_Click);
            // 
            // ButExit
            // 
            resources.ApplyResources(this.ButExit, "ButExit");
            this.ButExit.Name = "ButExit";
            this.ButExit.UseVisualStyleBackColor = true;
            this.ButExit.Click += new System.EventHandler(this.ButExit_Click);
            // 
            // ButApprove
            // 
            resources.ApplyResources(this.ButApprove, "ButApprove");
            this.ButApprove.Name = "ButApprove";
            this.ButApprove.UseVisualStyleBackColor = true;
            this.ButApprove.Click += new System.EventHandler(this.ButApprove_Click);
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            resources.ApplyResources(this.Grid, "Grid");
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            // 
            // TxtRemarks
            // 
            resources.ApplyResources(this.TxtRemarks, "TxtRemarks");
            this.TxtRemarks.Name = "TxtRemarks";
            // 
            // FrmModelPriceAppGarments
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmModelPriceAppGarments";
            this.Load += new System.EventHandler(this.FrmModelPriceAppGarments_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmModelPriceAppGarments_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmModelPriceAppGarments_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button ButClear;
        private System.Windows.Forms.Button ButExit;
        private System.Windows.Forms.Button ButApprove;
        private DotnetVFGrid.MyDataGridView Grid;
        private V_Components.MyTextBox TxtRemarks;
    }
}