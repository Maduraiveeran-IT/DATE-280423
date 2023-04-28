namespace Accounts
{
    partial class FrmMachineProduction_Import
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DtpTDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtFNeedle = new V_Components.MyTextBox();
            this.TxtFShift = new V_Components.MyTextBox();
            this.TxtFYear = new V_Components.MyTextBox();
            this.TxtFWeek = new V_Components.MyTextBox();
            this.DtpFDate = new System.Windows.Forms.DateTimePicker();
            this.TxtTNeedle = new V_Components.MyTextBox();
            this.TxtTShift = new V_Components.MyTextBox();
            this.TxtTYear = new V_Components.MyTextBox();
            this.TxtTWeek = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(732, 467);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 34);
            this.button1.TabIndex = 2;
            this.button1.Text = "&OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(894, 467);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(78, 34);
            this.button3.TabIndex = 4;
            this.button3.Text = "E&XIT";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(813, 467);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 34);
            this.button2.TabIndex = 3;
            this.button2.Text = "&CANCEL";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(820, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "NEEDLE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(390, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "WEEK";
            // 
            // DtpTDate
            // 
            this.DtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpTDate.Location = new System.Drawing.Point(64, 10);
            this.DtpTDate.Name = "DtpTDate";
            this.DtpTDate.Size = new System.Drawing.Size(101, 20);
            this.DtpTDate.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "YEAR";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(623, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SHIFT";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "DATE";
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.TxtFNeedle);
            this.GBMain.Controls.Add(this.TxtFShift);
            this.GBMain.Controls.Add(this.TxtFYear);
            this.GBMain.Controls.Add(this.TxtFWeek);
            this.GBMain.Controls.Add(this.DtpFDate);
            this.GBMain.Controls.Add(this.TxtTNeedle);
            this.GBMain.Controls.Add(this.TxtTShift);
            this.GBMain.Controls.Add(this.TxtTYear);
            this.GBMain.Controls.Add(this.TxtTWeek);
            this.GBMain.Controls.Add(this.DtpTDate);
            this.GBMain.Controls.Add(this.button3);
            this.GBMain.Controls.Add(this.button2);
            this.GBMain.Controls.Add(this.button1);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(2, -1);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(978, 507);
            this.GBMain.TabIndex = 1;
            this.GBMain.TabStop = false;
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Grid.DefaultCellStyle = dataGridViewCellStyle2;
            this.Grid.Location = new System.Drawing.Point(12, 63);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(960, 398);
            this.Grid.TabIndex = 1;
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            // 
            // TxtFNeedle
            // 
            this.TxtFNeedle.Location = new System.Drawing.Point(876, 36);
            this.TxtFNeedle.Name = "TxtFNeedle";
            this.TxtFNeedle.Size = new System.Drawing.Size(96, 20);
            this.TxtFNeedle.TabIndex = 16;
            this.TxtFNeedle.TabStop = false;
            this.TxtFNeedle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtFNeedle.Visible = false;
            // 
            // TxtFShift
            // 
            this.TxtFShift.Location = new System.Drawing.Point(667, 36);
            this.TxtFShift.Name = "TxtFShift";
            this.TxtFShift.Size = new System.Drawing.Size(109, 20);
            this.TxtFShift.TabIndex = 15;
            this.TxtFShift.TabStop = false;
            this.TxtFShift.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtFShift.Visible = false;
            // 
            // TxtFYear
            // 
            this.TxtFYear.Location = new System.Drawing.Point(246, 37);
            this.TxtFYear.Name = "TxtFYear";
            this.TxtFYear.Size = new System.Drawing.Size(108, 20);
            this.TxtFYear.TabIndex = 13;
            this.TxtFYear.TabStop = false;
            this.TxtFYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtFYear.Visible = false;
            // 
            // TxtFWeek
            // 
            this.TxtFWeek.Location = new System.Drawing.Point(449, 36);
            this.TxtFWeek.Name = "TxtFWeek";
            this.TxtFWeek.Size = new System.Drawing.Size(106, 20);
            this.TxtFWeek.TabIndex = 14;
            this.TxtFWeek.TabStop = false;
            this.TxtFWeek.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtFWeek.Visible = false;
            // 
            // DtpFDate
            // 
            this.DtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpFDate.Location = new System.Drawing.Point(64, 36);
            this.DtpFDate.Name = "DtpFDate";
            this.DtpFDate.Size = new System.Drawing.Size(101, 20);
            this.DtpFDate.TabIndex = 12;
            this.DtpFDate.Visible = false;
            // 
            // TxtTNeedle
            // 
            this.TxtTNeedle.Location = new System.Drawing.Point(876, 10);
            this.TxtTNeedle.Name = "TxtTNeedle";
            this.TxtTNeedle.Size = new System.Drawing.Size(96, 20);
            this.TxtTNeedle.TabIndex = 11;
            this.TxtTNeedle.TabStop = false;
            this.TxtTNeedle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtTShift
            // 
            this.TxtTShift.Location = new System.Drawing.Point(667, 10);
            this.TxtTShift.Name = "TxtTShift";
            this.TxtTShift.Size = new System.Drawing.Size(109, 20);
            this.TxtTShift.TabIndex = 10;
            this.TxtTShift.TabStop = false;
            this.TxtTShift.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtTYear
            // 
            this.TxtTYear.Location = new System.Drawing.Point(246, 11);
            this.TxtTYear.Name = "TxtTYear";
            this.TxtTYear.Size = new System.Drawing.Size(108, 20);
            this.TxtTYear.TabIndex = 1;
            this.TxtTYear.TabStop = false;
            this.TxtTYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtTWeek
            // 
            this.TxtTWeek.Location = new System.Drawing.Point(449, 10);
            this.TxtTWeek.Name = "TxtTWeek";
            this.TxtTWeek.Size = new System.Drawing.Size(106, 20);
            this.TxtTWeek.TabIndex = 2;
            this.TxtTWeek.TabStop = false;
            this.TxtTWeek.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmMachineProduction_Import
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 510);
            this.ControlBox = false;
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmMachineProduction_Import";
            this.Text = "FrmMachineProduction_Import";
            this.Load += new System.EventHandler(this.FrmMachineProduction_Import_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMachineProduction_Import_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private V_Components.MyTextBox TxtTYear;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker DtpTDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox GBMain;
        private V_Components.MyTextBox TxtTNeedle;
        private V_Components.MyTextBox TxtTShift;
        private V_Components.MyTextBox TxtTWeek;
        private V_Components.MyTextBox TxtFNeedle;
        private V_Components.MyTextBox TxtFShift;
        private V_Components.MyTextBox TxtFYear;
        private V_Components.MyTextBox TxtFWeek;
        private System.Windows.Forms.DateTimePicker DtpFDate;
        private DotnetVFGrid.MyDataGridView Grid;
    }
}