namespace Accounts
{
    partial class FrmMasterSegSize
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
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtSize = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtTotOrder = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.TxtSize);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.TxtTotOrder);
            this.GBMain.Location = new System.Drawing.Point(10, 12);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(329, 452);
            this.GBMain.TabIndex = 2;
            this.GBMain.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 57;
            this.label1.Text = "SIZE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(211, 420);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 15);
            this.label4.TabIndex = 56;
            this.label4.Text = "TOTAL";
            // 
            // TxtSize
            // 
            this.TxtSize.AcceptsReturn = true;
            this.TxtSize.Location = new System.Drawing.Point(67, 17);
            this.TxtSize.Name = "TxtSize";
            this.TxtSize.Size = new System.Drawing.Size(223, 20);
            this.TxtSize.TabIndex = 58;
            this.TxtSize.Tag = "";
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(12, 49);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid.Size = new System.Drawing.Size(309, 356);
            this.Grid.TabIndex = 1;
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            //this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Txt_KeyDown);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            // 
            // TxtTotOrder
            // 
            this.TxtTotOrder.Location = new System.Drawing.Point(266, 418);
            this.TxtTotOrder.Name = "TxtTotOrder";
            this.TxtTotOrder.Size = new System.Drawing.Size(56, 20);
            this.TxtTotOrder.TabIndex = 2;
            this.TxtTotOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmMasterSegSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 473);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmMasterSegSize";
            this.Text = "FrmMasterSegSize";
            this.Load += new System.EventHandler(this.FrmMasterSegSize_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMasterSegSize_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmMasterSegSize_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private V_Components.MyTextBox TxtSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private DotnetVFGrid.MyDataGridView Grid;
        private V_Components.MyTextBox TxtTotOrder;
    }
}