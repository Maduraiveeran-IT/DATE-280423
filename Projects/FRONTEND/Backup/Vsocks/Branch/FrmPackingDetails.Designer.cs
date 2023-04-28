namespace Accounts
{
    partial class FrmPackingDetails
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
            this.DtpFDate = new System.Windows.Forms.DateTimePicker();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.DtpTDate = new System.Windows.Forms.DateTimePicker();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Grid1 = new DotnetVFGrid.MyDataGridView();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.TxtKgs = new V_Components.MyTextBox();
            this.TxtTotal = new V_Components.MyTextBox();
            this.TxtProcess = new V_Components.MyTextBox();
            this.TxtENo = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.Grid1);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.DtpFDate);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.DtpTDate);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtKgs);
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.TxtProcess);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.TxtENo);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Location = new System.Drawing.Point(5, 3);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(461, 480);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // DtpFDate
            // 
            this.DtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpFDate.Location = new System.Drawing.Point(100, 23);
            this.DtpFDate.Name = "DtpFDate";
            this.DtpFDate.Size = new System.Drawing.Size(81, 20);
            this.DtpFDate.TabIndex = 0;
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(367, 53);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(81, 20);
            this.DtpDate.TabIndex = 1;
            this.DtpDate.TabStop = false;
            // 
            // DtpTDate
            // 
            this.DtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpTDate.Location = new System.Drawing.Point(187, 23);
            this.DtpTDate.Name = "DtpTDate";
            this.DtpTDate.Size = new System.Drawing.Size(81, 20);
            this.DtpTDate.TabIndex = 1;
            this.DtpTDate.Visible = false;
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Branch.Properties.Resources.Down;
            this.Arrow1.Location = new System.Drawing.Point(273, 50);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(25, 21);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 55;
            this.Arrow1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 367);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 15);
            this.label5.TabIndex = 15;
            this.label5.Text = "REMARKS";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(275, 338);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "TOTAL KGS";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 339);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "PACKED";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(13, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "PROCESS";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(322, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "DATE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(322, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "ENO";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "EFFECT DATE";
            // 
            // Grid1
            // 
            this.Grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid1.Location = new System.Drawing.Point(16, 390);
            this.Grid1.MultiSelect = false;
            this.Grid1.Name = "Grid1";
            this.Grid1.Size = new System.Drawing.Size(432, 84);
            this.Grid1.TabIndex = 3;
            this.Grid1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            this.Grid1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid1.CurrentCellChanged += new System.EventHandler(this.Grid_CurrentCellChanged);
            this.Grid1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid1.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(11, 79);
            this.Grid.MultiSelect = false;
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(437, 249);
            this.Grid.TabIndex = 3;
            this.Grid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Grid_CellMouseClick);
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.CurrentCellChanged += new System.EventHandler(this.Grid_CurrentCellChanged);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(91, 364);
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(357, 20);
            this.TxtRemarks.TabIndex = 6;
            // 
            // TxtKgs
            // 
            this.TxtKgs.Location = new System.Drawing.Point(358, 334);
            this.TxtKgs.Multiline = true;
            this.TxtKgs.Name = "TxtKgs";
            this.TxtKgs.Size = new System.Drawing.Size(90, 20);
            this.TxtKgs.TabIndex = 5;
            this.TxtKgs.TabStop = false;
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(91, 336);
            this.TxtTotal.Multiline = true;
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(90, 20);
            this.TxtTotal.TabIndex = 4;
            this.TxtTotal.TabStop = false;
            // 
            // TxtProcess
            // 
            this.TxtProcess.Location = new System.Drawing.Point(100, 51);
            this.TxtProcess.Name = "TxtProcess";
            this.TxtProcess.Size = new System.Drawing.Size(168, 20);
            this.TxtProcess.TabIndex = 2;
            // 
            // TxtENo
            // 
            this.TxtENo.Location = new System.Drawing.Point(367, 25);
            this.TxtENo.Name = "TxtENo";
            this.TxtENo.Size = new System.Drawing.Size(81, 20);
            this.TxtENo.TabIndex = 0;
            this.TxtENo.TabStop = false;
            this.TxtENo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmPackingDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 495);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmPackingDetails";
            this.Text = "PACKING & SHIFTING DETAILS";
            this.Load += new System.EventHandler(this.FrmPackingDetails_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmPackingDetails_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmPackingDetails_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DtpTDate;
        private DotnetVFGrid.MyDataGridView Grid;
        private V_Components.MyTextBox TxtRemarks;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtProcess;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private V_Components.MyTextBox TxtENo;
        private System.Windows.Forms.PictureBox Arrow1;
        private V_Components.MyTextBox TxtTotal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker DtpFDate;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtKgs;
        private System.Windows.Forms.Label label7;
        private DotnetVFGrid.MyDataGridView Grid1;
    }
}