namespace Accounts
{
    partial class FrmSocksYarnMoqPoOrder
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
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtBuyer = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtTotOrder = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(148, 27);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(181, 22);
            this.DtpDate.TabIndex = 22;
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtTotOrder);
            this.GBMain.Controls.Add(this.pictureBox1);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.TxtBuyer);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Location = new System.Drawing.Point(12, 12);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(642, 536);
            this.GBMain.TabIndex = 2;
            this.GBMain.TabStop = false;
            // 
            // pictureBox1
            // 
            
            // 
            // Arrow1
            // 
        
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 17);
            this.label4.TabIndex = 38;
            this.label4.Text = "BUYER";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 17);
            this.label2.TabIndex = 19;
            this.label2.Text = "EFFECT FROM";
            // 
            // TxtBuyer
            // 
            this.TxtBuyer.Location = new System.Drawing.Point(148, 70);
            this.TxtBuyer.Name = "TxtBuyer";
            this.TxtBuyer.Size = new System.Drawing.Size(418, 22);
            this.TxtBuyer.TabIndex = 37;
            this.TxtBuyer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Grid
            // 
            this.Grid.AllowUserToOrderColumns = true;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(19, 124);
            this.Grid.Name = "Grid";
            this.Grid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Grid.Size = new System.Drawing.Size(600, 354);
            this.Grid.TabIndex = 17;
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(468, 498);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 19);
            this.label1.TabIndex = 58;
            this.label1.Text = "TOTAL";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // TxtTotOrder
            // 
            this.TxtTotOrder.Location = new System.Drawing.Point(539, 495);
            this.TxtTotOrder.Margin = new System.Windows.Forms.Padding(4);
            this.TxtTotOrder.Name = "TxtTotOrder";
            this.TxtTotOrder.Size = new System.Drawing.Size(80, 22);
            this.TxtTotOrder.TabIndex = 57;
            this.TxtTotOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmSocksYarnMoqPoOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 562);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmSocksYarnMoqPoOrder";
            this.Text = "FrmSocksYarnMoqPoOrder";
            this.Load += new System.EventHandler(this.FrmSocksYarnMoqPoOrder_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSocksYarnMoqPoOrder_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmSocksYarnMoqPoOrder_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label2;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox Arrow1;
        private V_Components.MyTextBox TxtBuyer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtTotOrder;

    }
}