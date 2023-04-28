namespace Accounts
{
    partial class FrmMachinePlanning
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
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.CmbToWeek = new System.Windows.Forms.ComboBox();
            this.CmbToYear = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CmbFromWeek = new System.Windows.Forms.ComboBox();
            this.CmbFromYear = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnReport = new System.Windows.Forms.Button();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.BtnExit);
            this.GBMain.Controls.Add(this.BtnCancel);
            this.GBMain.Controls.Add(this.CmbToWeek);
            this.GBMain.Controls.Add(this.CmbToYear);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.CmbFromWeek);
            this.GBMain.Controls.Add(this.CmbFromYear);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.BtnReport);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Location = new System.Drawing.Point(7, -2);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(942, 542);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // BtnExit
            // 
            this.BtnExit.Location = new System.Drawing.Point(830, 14);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(90, 35);
            this.BtnExit.TabIndex = 7;
            this.BtnExit.Text = "E&XIT";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(734, 14);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(90, 35);
            this.BtnCancel.TabIndex = 6;
            this.BtnCancel.Text = "&CANCEL";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // CmbToWeek
            // 
            this.CmbToWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbToWeek.FormattingEnabled = true;
            this.CmbToWeek.Location = new System.Drawing.Point(387, 23);
            this.CmbToWeek.Name = "CmbToWeek";
            this.CmbToWeek.Size = new System.Drawing.Size(53, 21);
            this.CmbToWeek.TabIndex = 3;
            // 
            // CmbToYear
            // 
            this.CmbToYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbToYear.FormattingEnabled = true;
            this.CmbToYear.Location = new System.Drawing.Point(305, 23);
            this.CmbToYear.Name = "CmbToYear";
            this.CmbToYear.Size = new System.Drawing.Size(76, 21);
            this.CmbToYear.TabIndex = 2;
            this.CmbToYear.SelectedIndexChanged += new System.EventHandler(this.CmbToYear_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(260, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "TO";
            // 
            // CmbFromWeek
            // 
            this.CmbFromWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbFromWeek.FormattingEnabled = true;
            this.CmbFromWeek.Location = new System.Drawing.Point(143, 22);
            this.CmbFromWeek.Name = "CmbFromWeek";
            this.CmbFromWeek.Size = new System.Drawing.Size(53, 21);
            this.CmbFromWeek.TabIndex = 1;
            // 
            // CmbFromYear
            // 
            this.CmbFromYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbFromYear.FormattingEnabled = true;
            this.CmbFromYear.Location = new System.Drawing.Point(61, 22);
            this.CmbFromYear.Name = "CmbFromYear";
            this.CmbFromYear.Size = new System.Drawing.Size(76, 21);
            this.CmbFromYear.TabIndex = 0;
            this.CmbFromYear.SelectedIndexChanged += new System.EventHandler(this.CmbFromYear_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "FROM";
            // 
            // BtnReport
            // 
            this.BtnReport.BackColor = System.Drawing.SystemColors.Control;
            this.BtnReport.Location = new System.Drawing.Point(638, 15);
            this.BtnReport.Name = "BtnReport";
            this.BtnReport.Size = new System.Drawing.Size(90, 35);
            this.BtnReport.TabIndex = 4;
            this.BtnReport.Text = "&REPORT";
            this.BtnReport.UseVisualStyleBackColor = false;
            this.BtnReport.Click += new System.EventHandler(this.button1_Click);
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.AllowUserToResizeColumns = false;
            this.Grid.AllowUserToResizeRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(9, 55);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(925, 481);
            this.Grid.TabIndex = 5;
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            // 
            // FrmMachinePlanning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 546);
            this.Controls.Add(this.GBMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FrmMachinePlanning";
            this.Text = "MACHINE PLANNING";
            this.Load += new System.EventHandler(this.FrmMachinePlanning_Load);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.Button BtnReport;
        private System.Windows.Forms.ComboBox CmbToWeek;
        private System.Windows.Forms.ComboBox CmbToYear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CmbFromWeek;
        private System.Windows.Forms.ComboBox CmbFromYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnExit;
    }
}