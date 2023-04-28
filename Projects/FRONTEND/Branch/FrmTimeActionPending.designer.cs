namespace Accounts
{
    partial class FrmTimeActionPending
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
            this.LblEntryNo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtTotPro = new V_Components.MyTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.LblEntryNo);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtTotPro);
            this.GBMain.Controls.Add(this.button3);
            this.GBMain.Controls.Add(this.button2);
            this.GBMain.Controls.Add(this.button1);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Location = new System.Drawing.Point(-1, 6);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(792, 500);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // LblEntryNo
            // 
            this.LblEntryNo.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblEntryNo.Location = new System.Drawing.Point(666, 16);
            this.LblEntryNo.Name = "LblEntryNo";
            this.LblEntryNo.Size = new System.Drawing.Size(110, 15);
            this.LblEntryNo.TabIndex = 60;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 15);
            this.label1.TabIndex = 59;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(610, 472);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 15);
            this.label5.TabIndex = 58;
            this.label5.Text = "TOT COMP";
            // 
            // TxtTotPro
            // 
            this.TxtTotPro.Location = new System.Drawing.Point(690, 470);
            this.TxtTotPro.Name = "TxtTotPro";
            this.TxtTotPro.Size = new System.Drawing.Size(86, 20);
            this.TxtTotPro.TabIndex = 4;
            this.TxtTotPro.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(261, 467);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "SAVE";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(342, 467);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "CANCEL";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(423, 467);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "EXIT";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(10, 34);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid.Size = new System.Drawing.Size(767, 427);
            this.Grid.TabIndex = 0;
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            // 
            // FrmTimeActionPending
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 513);
            this.ControlBox = false;
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmTimeActionPending";
            this.Text = "Time & Action Pending";
            this.Load += new System.EventHandler(this.FrmTimeActionPending_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmTimeActionPending_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmTimeActionPending_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtTotPro;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LblEntryNo;
    }
}