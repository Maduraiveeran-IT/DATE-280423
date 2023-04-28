namespace Accounts
{
    partial class FrmTimeActionSettingMaster
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.TxtLeadTime = new V_Components.MyTextBox();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.TxtDivision = new V_Components.MyTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtTotPro = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.listBox1);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.TxtLeadTime);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.TxtDivision);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtTotPro);
            this.GBMain.Location = new System.Drawing.Point(12, 12);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(387, 355);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(19, 324);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(63, 4);
            this.listBox1.TabIndex = 59;
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Branch.Properties.Resources.Down;
            this.Arrow1.Location = new System.Drawing.Point(354, 19);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(25, 21);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 58;
            this.Arrow1.TabStop = false;
            // 
            // TxtLeadTime
            // 
            this.TxtLeadTime.Location = new System.Drawing.Point(298, 20);
            this.TxtLeadTime.Name = "TxtLeadTime";
            this.TxtLeadTime.Size = new System.Drawing.Size(53, 20);
            this.TxtLeadTime.TabIndex = 1;
            this.TxtLeadTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down;
            this.Arrow3.Location = new System.Drawing.Point(194, 19);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(25, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 58;
            this.Arrow3.TabStop = false;
            // 
            // TxtDivision
            // 
            this.TxtDivision.Location = new System.Drawing.Point(88, 19);
            this.TxtDivision.Name = "TxtDivision";
            this.TxtDivision.Size = new System.Drawing.Size(102, 20);
            this.TxtDivision.TabIndex = 0;
            this.TxtDivision.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(236, 326);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 15);
            this.label4.TabIndex = 56;
            this.label4.Text = "MAX DAY";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(225, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "LEAD TIME";
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(20, 46);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid.Size = new System.Drawing.Size(361, 272);
            this.Grid.TabIndex = 2;
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "DIVISION";
            // 
            // TxtTotPro
            // 
            this.TxtTotPro.Location = new System.Drawing.Point(307, 324);
            this.TxtTotPro.Name = "TxtTotPro";
            this.TxtTotPro.Size = new System.Drawing.Size(74, 20);
            this.TxtTotPro.TabIndex = 3;
            this.TxtTotPro.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmTimeActionSettingMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 372);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmTimeActionSettingMaster";
            this.Text = "Time & Action Master Setting";
            this.Load += new System.EventHandler(this.FrmTimeActionSettingMaster_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmTimeActionSettingMaster_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmTimeActionSettingMaster_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private DotnetVFGrid.MyDataGridView Grid;
        private V_Components.MyTextBox TxtTotPro;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtDivision;
        private System.Windows.Forms.PictureBox Arrow3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.PictureBox Arrow1;
        private V_Components.MyTextBox TxtLeadTime;
        private System.Windows.Forms.Label label1;
    }
}