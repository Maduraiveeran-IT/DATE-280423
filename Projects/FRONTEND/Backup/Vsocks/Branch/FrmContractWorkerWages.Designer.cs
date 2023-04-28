namespace Accounts
{
    partial class FrmContractWorkerWages
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
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.DtpTDate = new System.Windows.Forms.DateTimePicker();
            this.DtpFDate = new System.Windows.Forms.DateTimePicker();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtDeduct = new V_Components.MyTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtKgs = new V_Components.MyTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtRo = new V_Components.MyTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.TxtNet = new V_Components.MyTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TxtGross = new V_Components.MyTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtProcess = new V_Components.MyTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtParty = new V_Components.MyTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtENo = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.DtpTDate);
            this.GBMain.Controls.Add(this.DtpFDate);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtDeduct);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.TxtKgs);
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.TxtRo);
            this.GBMain.Controls.Add(this.label11);
            this.GBMain.Controls.Add(this.TxtNet);
            this.GBMain.Controls.Add(this.label10);
            this.GBMain.Controls.Add(this.TxtGross);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.TxtProcess);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.TxtParty);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtENo);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Location = new System.Drawing.Point(5, 3);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(539, 371);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(11, 79);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(518, 165);
            this.Grid.TabIndex = 4;
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // DtpTDate
            // 
            this.DtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpTDate.Location = new System.Drawing.Point(158, 53);
            this.DtpTDate.Name = "DtpTDate";
            this.DtpTDate.Size = new System.Drawing.Size(76, 20);
            this.DtpTDate.TabIndex = 2;
            this.DtpTDate.Leave += new System.EventHandler(this.DtpTDate_Leave);
            // 
            // DtpFDate
            // 
            this.DtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpFDate.Location = new System.Drawing.Point(78, 53);
            this.DtpFDate.Name = "DtpFDate";
            this.DtpFDate.Size = new System.Drawing.Size(76, 20);
            this.DtpFDate.TabIndex = 1;
            this.DtpFDate.Leave += new System.EventHandler(this.DtpFDate_Leave);
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(450, 23);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(81, 20);
            this.DtpDate.TabIndex = 2;
            this.DtpDate.TabStop = false;
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Branch.Properties.Resources.Down;
            this.Arrow1.Location = new System.Drawing.Point(504, 52);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(25, 21);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 55;
            this.Arrow1.TabStop = false;
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down;
            this.Arrow3.Location = new System.Drawing.Point(235, 23);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(25, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 55;
            this.Arrow3.TabStop = false;
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(91, 337);
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(438, 20);
            this.TxtRemarks.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 340);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 15);
            this.label5.TabIndex = 15;
            this.label5.Text = "REMARKS";
            // 
            // TxtDeduct
            // 
            this.TxtDeduct.Location = new System.Drawing.Point(91, 281);
            this.TxtDeduct.Multiline = true;
            this.TxtDeduct.Name = "TxtDeduct";
            this.TxtDeduct.Size = new System.Drawing.Size(141, 20);
            this.TxtDeduct.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(8, 284);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 15);
            this.label8.TabIndex = 15;
            this.label8.Text = "DEDUCTION";
            // 
            // TxtKgs
            // 
            this.TxtKgs.Location = new System.Drawing.Point(91, 255);
            this.TxtKgs.Multiline = true;
            this.TxtKgs.Name = "TxtKgs";
            this.TxtKgs.Size = new System.Drawing.Size(141, 20);
            this.TxtKgs.TabIndex = 5;
            this.TxtKgs.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 258);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "TOTAL KGS";
            // 
            // TxtRo
            // 
            this.TxtRo.Location = new System.Drawing.Point(403, 281);
            this.TxtRo.Multiline = true;
            this.TxtRo.Name = "TxtRo";
            this.TxtRo.Size = new System.Drawing.Size(126, 20);
            this.TxtRo.TabIndex = 5;
            this.TxtRo.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(274, 284);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 15);
            this.label11.TabIndex = 15;
            this.label11.Text = "RO AMOUNT";
            // 
            // TxtNet
            // 
            this.TxtNet.Location = new System.Drawing.Point(403, 309);
            this.TxtNet.Multiline = true;
            this.TxtNet.Name = "TxtNet";
            this.TxtNet.Size = new System.Drawing.Size(126, 20);
            this.TxtNet.TabIndex = 3;
            this.TxtNet.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(274, 312);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 15);
            this.label10.TabIndex = 15;
            this.label10.Text = "NET AMOUNT";
            // 
            // TxtGross
            // 
            this.TxtGross.Location = new System.Drawing.Point(403, 255);
            this.TxtGross.Multiline = true;
            this.TxtGross.Name = "TxtGross";
            this.TxtGross.Size = new System.Drawing.Size(126, 20);
            this.TxtGross.TabIndex = 5;
            this.TxtGross.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(274, 258);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "GROSS AMOUNT";
            // 
            // TxtProcess
            // 
            this.TxtProcess.Location = new System.Drawing.Point(348, 52);
            this.TxtProcess.Name = "TxtProcess";
            this.TxtProcess.Size = new System.Drawing.Size(156, 20);
            this.TxtProcess.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(276, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "PROCESS";
            // 
            // TxtParty
            // 
            this.TxtParty.Location = new System.Drawing.Point(78, 23);
            this.TxtParty.Name = "TxtParty";
            this.TxtParty.Size = new System.Drawing.Size(156, 20);
            this.TxtParty.TabIndex = 0;
            this.TxtParty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "FROM \\ TO";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "PARTY";
            // 
            // TxtENo
            // 
            this.TxtENo.Location = new System.Drawing.Point(348, 23);
            this.TxtENo.Name = "TxtENo";
            this.TxtENo.Size = new System.Drawing.Size(96, 20);
            this.TxtENo.TabIndex = 0;
            this.TxtENo.TabStop = false;
            this.TxtENo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(274, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "ENTRY NO";
            // 
            // FrmContractWorkerWages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 383);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmContractWorkerWages";
            this.Text = "PACKING & SHIFTING CHARGES";
            this.Load += new System.EventHandler(this.FrmContractWorkerWages_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmContractWorkerWages_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmContractWorkerWages_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.PictureBox Arrow3;
        private V_Components.MyTextBox TxtParty;
        private System.Windows.Forms.Label label1;
        private DotnetVFGrid.MyDataGridView Grid;
        private V_Components.MyTextBox TxtRemarks;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtProcess;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private V_Components.MyTextBox TxtENo;
        private System.Windows.Forms.PictureBox Arrow1;
        private V_Components.MyTextBox TxtDeduct;
        private System.Windows.Forms.Label label8;
        private V_Components.MyTextBox TxtKgs;
        private System.Windows.Forms.Label label7;
        private V_Components.MyTextBox TxtGross;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtRo;
        private System.Windows.Forms.Label label11;
        private V_Components.MyTextBox TxtNet;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DtpFDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker DtpTDate;
    }
}