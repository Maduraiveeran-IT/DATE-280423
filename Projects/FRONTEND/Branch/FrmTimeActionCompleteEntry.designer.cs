namespace Accounts
{
    partial class FrmTimeActionCompleteEntry
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
            this.label5 = new System.Windows.Forms.Label();
            this.TxtLeadDays = new V_Components.MyTextBox();
            this.TxtDivision = new V_Components.MyTextBox();
            this.TxtOrderNo = new V_Components.MyTextBox();
            this.Grid1 = new DotnetVFGrid.MyDataGridView();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtEntryNo = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtTotPro = new V_Components.MyTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DtpSDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.DtpODate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtEmployee = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtEmployee);
            this.GBMain.Controls.Add(this.TxtLeadDays);
            this.GBMain.Controls.Add(this.TxtDivision);
            this.GBMain.Controls.Add(this.TxtOrderNo);
            this.GBMain.Controls.Add(this.Grid1);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.TxtEntryNo);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.TxtTotPro);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.DtpSDate);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.DtpODate);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Location = new System.Drawing.Point(12, 12);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(640, 451);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(432, 426);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 15);
            this.label5.TabIndex = 56;
            this.label5.Text = "TOTAL HISTORY";
            // 
            // TxtLeadDays
            // 
            this.TxtLeadDays.Location = new System.Drawing.Point(94, 82);
            this.TxtLeadDays.Name = "TxtLeadDays";
            this.TxtLeadDays.Size = new System.Drawing.Size(97, 20);
            this.TxtLeadDays.TabIndex = 2;
            this.TxtLeadDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtDivision
            // 
            this.TxtDivision.Location = new System.Drawing.Point(93, 53);
            this.TxtDivision.Name = "TxtDivision";
            this.TxtDivision.Size = new System.Drawing.Size(226, 20);
            this.TxtDivision.TabIndex = 0;
            this.TxtDivision.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtOrderNo
            // 
            this.TxtOrderNo.Location = new System.Drawing.Point(424, 51);
            this.TxtOrderNo.Name = "TxtOrderNo";
            this.TxtOrderNo.Size = new System.Drawing.Size(208, 20);
            this.TxtOrderNo.TabIndex = 1;
            this.TxtOrderNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Grid1
            // 
            this.Grid1.AllowUserToAddRows = false;
            this.Grid1.AllowUserToDeleteRows = false;
            this.Grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid1.Location = new System.Drawing.Point(19, 323);
            this.Grid1.Name = "Grid1";
            this.Grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid1.Size = new System.Drawing.Size(614, 97);
            this.Grid1.TabIndex = 3;
            this.Grid1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            this.Grid1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid1.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(20, 110);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid.Size = new System.Drawing.Size(614, 210);
            this.Grid.TabIndex = 3;
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // TxtEntryNo
            // 
            this.TxtEntryNo.Enabled = false;
            this.TxtEntryNo.Location = new System.Drawing.Point(93, 21);
            this.TxtEntryNo.Name = "TxtEntryNo";
            this.TxtEntryNo.Size = new System.Drawing.Size(97, 20);
            this.TxtEntryNo.TabIndex = 0;
            this.TxtEntryNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "ENTRY NO";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(17, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "DIVISION";
            // 
            // TxtTotPro
            // 
            this.TxtTotPro.Location = new System.Drawing.Point(546, 424);
            this.TxtTotPro.Name = "TxtTotPro";
            this.TxtTotPro.Size = new System.Drawing.Size(86, 20);
            this.TxtTotPro.TabIndex = 4;
            this.TxtTotPro.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(325, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "ORDER NO";
            // 
            // DtpSDate
            // 
            this.DtpSDate.Enabled = false;
            this.DtpSDate.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpSDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpSDate.Location = new System.Drawing.Point(531, 82);
            this.DtpSDate.Name = "DtpSDate";
            this.DtpSDate.Size = new System.Drawing.Size(101, 22);
            this.DtpSDate.TabIndex = 8;
            this.DtpSDate.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(325, 86);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 15);
            this.label8.TabIndex = 1;
            this.label8.Text = "PO / SHIP Dt";
            // 
            // DtpODate
            // 
            this.DtpODate.Enabled = false;
            this.DtpODate.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpODate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpODate.Location = new System.Drawing.Point(424, 82);
            this.DtpODate.Name = "DtpODate";
            this.DtpODate.Size = new System.Drawing.Size(101, 22);
            this.DtpODate.TabIndex = 8;
            this.DtpODate.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(17, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "LEAD DAYS";
            // 
            // DtpDate
            // 
            this.DtpDate.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(424, 16);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(101, 22);
            this.DtpDate.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(325, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "ENTRY DATE";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(17, 430);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "ENTERED BY";
            // 
            // TxtEmployee
            // 
            this.TxtEmployee.Location = new System.Drawing.Point(106, 428);
            this.TxtEmployee.Name = "TxtEmployee";
            this.TxtEmployee.Size = new System.Drawing.Size(132, 20);
            this.TxtEmployee.TabIndex = 6;
            this.TxtEmployee.TabStop = false;
            this.TxtEmployee.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmTimeActionCompleteEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 469);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmTimeActionCompleteEntry";
            this.Text = "Time & Action Complete  Entry";
            this.Load += new System.EventHandler(this.FrmTimeActionCompleteEntry_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmTimeActionCompleteEntry_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmTimeActionCompleteEntry_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox TxtOrderNo;
        private V_Components.MyTextBox TxtEntryNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtTotPro;
        private System.Windows.Forms.DateTimePicker DtpODate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker DtpSDate;
        private V_Components.MyTextBox TxtDivision;
        private System.Windows.Forms.Label label7;
        private V_Components.MyTextBox TxtLeadDays;
        private System.Windows.Forms.Label label8;
        private DotnetVFGrid.MyDataGridView Grid1;
        private V_Components.MyTextBox TxtEmployee;
        private System.Windows.Forms.Label label6;
    }
}