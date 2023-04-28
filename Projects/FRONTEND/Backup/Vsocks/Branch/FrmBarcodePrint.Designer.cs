namespace Accounts
{
    partial class FrmBarcodePrint
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
            this.BtnLoad = new System.Windows.Forms.Button();
            this.Arrow2 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.RBtBar2Honey = new System.Windows.Forms.RadioButton();
            this.RBtBar2TVS = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOK = new System.Windows.Forms.Button();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.LblSpecial = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.DtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.TxtOperator = new V_Components.MyTextBox();
            this.TxtToSlno = new V_Components.MyTextBox();
            this.TxtFrmSlno = new V_Components.MyTextBox();
            this.TxtUnit = new V_Components.MyTextBox();
            this.TxtTotal = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtShift = new V_Components.MyTextBox();
            this.TxtNo = new V_Components.MyTextBox();
            this.RBtBar1Honey = new System.Windows.Forms.RadioButton();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.BtnLoad);
            this.GBMain.Controls.Add(this.Arrow2);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.TxtOperator);
            this.GBMain.Controls.Add(this.groupBox1);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.TxtUnit);
            this.GBMain.Controls.Add(this.LblSpecial);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.DtpDate1);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtShift);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtNo);
            this.GBMain.Location = new System.Drawing.Point(3, 3);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(596, 514);
            this.GBMain.TabIndex = 1;
            this.GBMain.TabStop = false;
            // 
            // BtnLoad
            // 
            this.BtnLoad.Location = new System.Drawing.Point(495, 91);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(75, 23);
            this.BtnLoad.TabIndex = 5;
            this.BtnLoad.Text = "LOAD";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // Arrow2
            // 
            this.Arrow2.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow2.Location = new System.Drawing.Point(440, 92);
            this.Arrow2.Name = "Arrow2";
            this.Arrow2.Size = new System.Drawing.Size(26, 21);
            this.Arrow2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow2.TabIndex = 42;
            this.Arrow2.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(195, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "OPERATOR";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.GroupBox3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.TxtToSlno);
            this.groupBox1.Controls.Add(this.TxtFrmSlno);
            this.groupBox1.Controls.Add(this.BtnCancel);
            this.groupBox1.Controls.Add(this.BtnOK);
            this.groupBox1.Location = new System.Drawing.Point(99, 268);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 111);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.RBtBar1Honey);
            this.GroupBox3.Controls.Add(this.RBtBar2Honey);
            this.GroupBox3.Controls.Add(this.RBtBar2TVS);
            this.GroupBox3.Location = new System.Drawing.Point(0, 0);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(337, 43);
            this.GroupBox3.TabIndex = 43;
            this.GroupBox3.TabStop = false;
            // 
            // RBtBar2Honey
            // 
            this.RBtBar2Honey.AutoSize = true;
            this.RBtBar2Honey.Location = new System.Drawing.Point(115, 18);
            this.RBtBar2Honey.Name = "RBtBar2Honey";
            this.RBtBar2Honey.Size = new System.Drawing.Size(108, 17);
            this.RBtBar2Honey.TabIndex = 1;
            this.RBtBar2Honey.TabStop = true;
            this.RBtBar2Honey.Text = "Barcode 2 Honey";
            this.RBtBar2Honey.UseVisualStyleBackColor = true;
            // 
            // RBtBar2TVS
            // 
            this.RBtBar2TVS.AutoSize = true;
            this.RBtBar2TVS.Location = new System.Drawing.Point(11, 18);
            this.RBtBar2TVS.Name = "RBtBar2TVS";
            this.RBtBar2TVS.Size = new System.Drawing.Size(98, 17);
            this.RBtBar2TVS.TabIndex = 0;
            this.RBtBar2TVS.TabStop = true;
            this.RBtBar2TVS.Text = "Barcode 2 TVS";
            this.RBtBar2TVS.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(152, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 13);
            this.label7.TabIndex = 42;
            this.label7.Text = "TO";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 40;
            this.label4.Text = "NO FROM";
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(155, 82);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 4;
            this.BtnCancel.Text = "CANCEL";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click_1);
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(53, 82);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 3;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow1.Location = new System.Drawing.Point(139, 93);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(26, 21);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 39;
            this.Arrow1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 38;
            this.label6.Text = "UNIT";
            // 
            // LblSpecial
            // 
            this.LblSpecial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.LblSpecial.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSpecial.Location = new System.Drawing.Point(9, 16);
            this.LblSpecial.Name = "LblSpecial";
            this.LblSpecial.Size = new System.Drawing.Size(579, 26);
            this.LblSpecial.TabIndex = 2;
            this.LblSpecial.Text = "BARCODE PRINT FOR PRODUCTION";
            this.LblSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(437, 491);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "TOTAL";
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow3.Location = new System.Drawing.Point(517, 53);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(26, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 30;
            this.Arrow3.TabStop = false;
            // 
            // DtpDate1
            // 
            this.DtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate1.Location = new System.Drawing.Point(268, 53);
            this.DtpDate1.Name = "DtpDate1";
            this.DtpDate1.Size = new System.Drawing.Size(88, 20);
            this.DtpDate1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(195, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "DATE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(398, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SHIFT";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "#NO";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(244, 52);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(87, 17);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "ALL Barcode";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // TxtOperator
            // 
            this.TxtOperator.Location = new System.Drawing.Point(268, 93);
            this.TxtOperator.Name = "TxtOperator";
            this.TxtOperator.Size = new System.Drawing.Size(166, 20);
            this.TxtOperator.TabIndex = 4;
            this.TxtOperator.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtToSlno
            // 
            this.TxtToSlno.Location = new System.Drawing.Point(185, 49);
            this.TxtToSlno.Name = "TxtToSlno";
            this.TxtToSlno.Size = new System.Drawing.Size(45, 20);
            this.TxtToSlno.TabIndex = 2;
            this.TxtToSlno.TabStop = false;
            this.TxtToSlno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtToSlno.TextChanged += new System.EventHandler(this.myTextBox2_TextChanged);
            // 
            // TxtFrmSlno
            // 
            this.TxtFrmSlno.Location = new System.Drawing.Point(85, 49);
            this.TxtFrmSlno.Name = "TxtFrmSlno";
            this.TxtFrmSlno.Size = new System.Drawing.Size(45, 20);
            this.TxtFrmSlno.TabIndex = 1;
            this.TxtFrmSlno.TabStop = false;
            this.TxtFrmSlno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtUnit
            // 
            this.TxtUnit.Location = new System.Drawing.Point(45, 93);
            this.TxtUnit.Name = "TxtUnit";
            this.TxtUnit.Size = new System.Drawing.Size(88, 20);
            this.TxtUnit.TabIndex = 3;
            this.TxtUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(495, 488);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(90, 20);
            this.TxtTotal.TabIndex = 43;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(9, 123);
            this.Grid.Name = "Grid";
            this.Grid.ReadOnly = true;
            this.Grid.Size = new System.Drawing.Size(576, 359);
            this.Grid.TabIndex = 4;
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // TxtShift
            // 
            this.TxtShift.Location = new System.Drawing.Point(440, 54);
            this.TxtShift.Name = "TxtShift";
            this.TxtShift.Size = new System.Drawing.Size(71, 20);
            this.TxtShift.TabIndex = 2;
            this.TxtShift.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtNo
            // 
            this.TxtNo.Location = new System.Drawing.Point(45, 54);
            this.TxtNo.Name = "TxtNo";
            this.TxtNo.Size = new System.Drawing.Size(88, 20);
            this.TxtNo.TabIndex = 0;
            this.TxtNo.TabStop = false;
            this.TxtNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RBtBar1Honey
            // 
            this.RBtBar1Honey.AutoSize = true;
            this.RBtBar1Honey.Location = new System.Drawing.Point(227, 18);
            this.RBtBar1Honey.Name = "RBtBar1Honey";
            this.RBtBar1Honey.Size = new System.Drawing.Size(108, 17);
            this.RBtBar1Honey.TabIndex = 2;
            this.RBtBar1Honey.TabStop = true;
            this.RBtBar1Honey.Text = "Barcode 1 Honey";
            this.RBtBar1Honey.UseVisualStyleBackColor = true;
            // 
            // FrmBarcodePrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 520);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmBarcodePrint";
            this.Text = "FrmBarcodePrint";
            this.Load += new System.EventHandler(this.FrmBarcodePrint_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmBarcodePrint_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmBarcodePrint_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label LblSpecial;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtTotal;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.PictureBox Arrow3;
        private System.Windows.Forms.DateTimePicker DtpDate1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private V_Components.MyTextBox TxtShift;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtNo;
        private System.Windows.Forms.PictureBox Arrow1;
        private System.Windows.Forms.Label label6;
        private V_Components.MyTextBox TxtUnit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtToSlno;
        private V_Components.MyTextBox TxtFrmSlno;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox Arrow2;
        private System.Windows.Forms.Label label8;
        private V_Components.MyTextBox TxtOperator;
        private System.Windows.Forms.Button BtnLoad;
        private System.Windows.Forms.GroupBox GroupBox3;
        private System.Windows.Forms.RadioButton RBtBar2TVS;
        private System.Windows.Forms.RadioButton RBtBar2Honey;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton RBtBar1Honey;
    }
}