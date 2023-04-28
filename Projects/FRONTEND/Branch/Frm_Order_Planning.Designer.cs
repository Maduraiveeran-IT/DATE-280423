namespace Accounts
{
    partial class Frm_Order_Planning
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
            this.CmbColor = new System.Windows.Forms.ComboBox();
            this.CmbStyle = new System.Windows.Forms.ComboBox();
            this.CmbOCN = new System.Windows.Forms.ComboBox();
            this.ChkLineList = new System.Windows.Forms.CheckedListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.TxtActPlanQty = new V_Components.MyTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.TxtToBePlanning = new V_Components.MyTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.CmbItem = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Grid_Sample = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtBufferPlanned = new V_Components.MyTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.CmbBuyer = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.CmbBal = new System.Windows.Forms.ComboBox();
            this.CmbProd = new System.Windows.Forms.ComboBox();
            this.CmbBom = new System.Windows.Forms.ComboBox();
            this.TxtLinesCount = new V_Components.MyTextBox();
            this.TxtBuyer = new V_Components.MyTextBox();
            this.TxtPlanNo = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.TxtShift = new V_Components.MyTextBox();
            this.TxtUnit = new V_Components.MyTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.TxtTarget = new V_Components.MyTextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.CmbOperator = new System.Windows.Forms.ComboBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Sample)).BeginInit();
            this.SuspendLayout();
            // 
            // CmbColor
            // 
            this.CmbColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbColor.FormattingEnabled = true;
            this.CmbColor.Location = new System.Drawing.Point(327, 93);
            this.CmbColor.Name = "CmbColor";
            this.CmbColor.Size = new System.Drawing.Size(188, 21);
            this.CmbColor.TabIndex = 7;
            this.CmbColor.SelectedIndexChanged += new System.EventHandler(this.CmbColor_SelectedIndexChanged);
            // 
            // CmbStyle
            // 
            this.CmbStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbStyle.FormattingEnabled = true;
            this.CmbStyle.Location = new System.Drawing.Point(327, 66);
            this.CmbStyle.Name = "CmbStyle";
            this.CmbStyle.Size = new System.Drawing.Size(188, 21);
            this.CmbStyle.TabIndex = 6;
            this.CmbStyle.SelectedIndexChanged += new System.EventHandler(this.CmbStyle_SelectedIndexChanged);
            // 
            // CmbOCN
            // 
            this.CmbOCN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbOCN.FormattingEnabled = true;
            this.CmbOCN.Location = new System.Drawing.Point(87, 66);
            this.CmbOCN.Name = "CmbOCN";
            this.CmbOCN.Size = new System.Drawing.Size(172, 21);
            this.CmbOCN.TabIndex = 4;
            this.CmbOCN.SelectedIndexChanged += new System.EventHandler(this.CmbOCN_SelectedIndexChanged);
            // 
            // ChkLineList
            // 
            this.ChkLineList.FormattingEnabled = true;
            this.ChkLineList.Location = new System.Drawing.Point(532, 40);
            this.ChkLineList.Name = "ChkLineList";
            this.ChkLineList.Size = new System.Drawing.Size(140, 169);
            this.ChkLineList.TabIndex = 18;
            this.ChkLineList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChkLineList_MouseClick);
            this.ChkLineList.SelectedIndexChanged += new System.EventHandler(this.ChkLineList_SelectedIndexChanged);
            this.ChkLineList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ChkLineList_MouseDoubleClick);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 123);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 68;
            this.label8.Text = "BOM";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(271, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "SAMPLE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "BUYER";
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(172, 14);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(110, 20);
            this.DtpDate.TabIndex = 1;
            this.DtpDate.TabStop = false;
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.CmbOperator);
            this.GBMain.Controls.Add(this.label15);
            this.GBMain.Controls.Add(this.TxtTarget);
            this.GBMain.Controls.Add(this.label14);
            this.GBMain.Controls.Add(this.TxtUnit);
            this.GBMain.Controls.Add(this.label13);
            this.GBMain.Controls.Add(this.TxtShift);
            this.GBMain.Controls.Add(this.label12);
            this.GBMain.Controls.Add(this.TxtActPlanQty);
            this.GBMain.Controls.Add(this.label11);
            this.GBMain.Controls.Add(this.TxtToBePlanning);
            this.GBMain.Controls.Add(this.label10);
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.CmbItem);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.Grid_Sample);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.TxtBufferPlanned);
            this.GBMain.Controls.Add(this.label9);
            this.GBMain.Controls.Add(this.CmbBuyer);
            this.GBMain.Controls.Add(this.button2);
            this.GBMain.Controls.Add(this.button1);
            this.GBMain.Controls.Add(this.CmbBal);
            this.GBMain.Controls.Add(this.CmbProd);
            this.GBMain.Controls.Add(this.CmbBom);
            this.GBMain.Controls.Add(this.CmbColor);
            this.GBMain.Controls.Add(this.CmbStyle);
            this.GBMain.Controls.Add(this.CmbOCN);
            this.GBMain.Controls.Add(this.ChkLineList);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.TxtLinesCount);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.TxtBuyer);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.TxtPlanNo);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(5, 4);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(684, 425);
            this.GBMain.TabIndex = 1;
            this.GBMain.TabStop = false;
            // 
            // TxtActPlanQty
            // 
            this.TxtActPlanQty.Location = new System.Drawing.Point(455, 147);
            this.TxtActPlanQty.Name = "TxtActPlanQty";
            this.TxtActPlanQty.Size = new System.Drawing.Size(59, 20);
            this.TxtActPlanQty.TabIndex = 88;
            this.TxtActPlanQty.TabStop = false;
            this.TxtActPlanQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(195, 152);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(27, 13);
            this.label11.TabIndex = 87;
            this.label11.Text = "BAL";
            // 
            // TxtToBePlanning
            // 
            this.TxtToBePlanning.Location = new System.Drawing.Point(239, 149);
            this.TxtToBePlanning.Name = "TxtToBePlanning";
            this.TxtToBePlanning.Size = new System.Drawing.Size(95, 20);
            this.TxtToBePlanning.TabIndex = 86;
            this.TxtToBePlanning.TabStop = false;
            this.TxtToBePlanning.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(340, 123);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(27, 13);
            this.label10.TabIndex = 85;
            this.label10.Text = "BAL";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(195, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 84;
            this.label7.Text = "PROD";
            // 
            // CmbItem
            // 
            this.CmbItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbItem.FormattingEnabled = true;
            this.CmbItem.Location = new System.Drawing.Point(87, 93);
            this.CmbItem.Name = "CmbItem";
            this.CmbItem.Size = new System.Drawing.Size(172, 21);
            this.CmbItem.TabIndex = 83;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 82;
            this.label5.Text = "ITEM / SIZE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(271, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 81;
            this.label4.Text = "COLOR";
            // 
            // Grid_Sample
            // 
            this.Grid_Sample.AllowUserToAddRows = false;
            this.Grid_Sample.AllowUserToDeleteRows = false;
            this.Grid_Sample.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid_Sample.Location = new System.Drawing.Point(12, 213);
            this.Grid_Sample.Name = "Grid_Sample";
            this.Grid_Sample.Size = new System.Drawing.Size(660, 206);
            this.Grid_Sample.TabIndex = 80;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 79;
            this.label6.Text = "PLANNED";
            // 
            // TxtBufferPlanned
            // 
            this.TxtBufferPlanned.Location = new System.Drawing.Point(87, 149);
            this.TxtBufferPlanned.Name = "TxtBufferPlanned";
            this.TxtBufferPlanned.Size = new System.Drawing.Size(102, 20);
            this.TxtBufferPlanned.TabIndex = 78;
            this.TxtBufferPlanned.TabStop = false;
            this.TxtBufferPlanned.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 70);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 13);
            this.label9.TabIndex = 77;
            this.label9.Text = "OCN";
            // 
            // CmbBuyer
            // 
            this.CmbBuyer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBuyer.FormattingEnabled = true;
            this.CmbBuyer.Location = new System.Drawing.Point(87, 40);
            this.CmbBuyer.Name = "CmbBuyer";
            this.CmbBuyer.Size = new System.Drawing.Size(428, 21);
            this.CmbBuyer.TabIndex = 3;
            this.CmbBuyer.SelectedIndexChanged += new System.EventHandler(this.CmbBuyer_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(420, 175);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(96, 32);
            this.button2.TabIndex = 17;
            this.button2.Text = "CLEAR";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(317, 175);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 32);
            this.button1.TabIndex = 16;
            this.button1.Text = "FILL";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CmbBal
            // 
            this.CmbBal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBal.FormattingEnabled = true;
            this.CmbBal.Location = new System.Drawing.Point(397, 120);
            this.CmbBal.Name = "CmbBal";
            this.CmbBal.Size = new System.Drawing.Size(117, 21);
            this.CmbBal.TabIndex = 10;
            this.CmbBal.SelectedIndexChanged += new System.EventHandler(this.CmbToBePlanned_SelectedIndexChanged);
            // 
            // CmbProd
            // 
            this.CmbProd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbProd.FormattingEnabled = true;
            this.CmbProd.Location = new System.Drawing.Point(239, 120);
            this.CmbProd.Name = "CmbProd";
            this.CmbProd.Size = new System.Drawing.Size(95, 21);
            this.CmbProd.TabIndex = 9;
            this.CmbProd.SelectedIndexChanged += new System.EventHandler(this.CmbPlanned_SelectedIndexChanged);
            // 
            // CmbBom
            // 
            this.CmbBom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBom.FormattingEnabled = true;
            this.CmbBom.Location = new System.Drawing.Point(87, 120);
            this.CmbBom.Name = "CmbBom";
            this.CmbBom.Size = new System.Drawing.Size(102, 21);
            this.CmbBom.TabIndex = 8;
            this.CmbBom.SelectedIndexChanged += new System.EventHandler(this.CmbQuantity_SelectedIndexChanged);
            // 
            // TxtLinesCount
            // 
            this.TxtLinesCount.Location = new System.Drawing.Point(532, 14);
            this.TxtLinesCount.Name = "TxtLinesCount";
            this.TxtLinesCount.Size = new System.Drawing.Size(140, 20);
            this.TxtLinesCount.TabIndex = 13;
            this.TxtLinesCount.TabStop = false;
            this.TxtLinesCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtBuyer
            // 
            this.TxtBuyer.Location = new System.Drawing.Point(317, 35);
            this.TxtBuyer.Name = "TxtBuyer";
            this.TxtBuyer.Size = new System.Drawing.Size(10, 20);
            this.TxtBuyer.TabIndex = 5;
            this.TxtBuyer.TabStop = false;
            this.TxtBuyer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtBuyer.Visible = false;
            // 
            // TxtPlanNo
            // 
            this.TxtPlanNo.Location = new System.Drawing.Point(87, 14);
            this.TxtPlanNo.Name = "TxtPlanNo";
            this.TxtPlanNo.Size = new System.Drawing.Size(79, 20);
            this.TxtPlanNo.TabIndex = 0;
            this.TxtPlanNo.TabStop = false;
            this.TxtPlanNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "PLAN NO";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(288, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(38, 13);
            this.label12.TabIndex = 89;
            this.label12.Text = "SHIFT";
            // 
            // TxtShift
            // 
            this.TxtShift.Location = new System.Drawing.Point(327, 14);
            this.TxtShift.Name = "TxtShift";
            this.TxtShift.Size = new System.Drawing.Size(65, 20);
            this.TxtShift.TabIndex = 90;
            this.TxtShift.TabStop = false;
            this.TxtShift.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtUnit
            // 
            this.TxtUnit.Location = new System.Drawing.Point(442, 14);
            this.TxtUnit.Name = "TxtUnit";
            this.TxtUnit.Size = new System.Drawing.Size(73, 20);
            this.TxtUnit.TabIndex = 92;
            this.TxtUnit.TabStop = false;
            this.TxtUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(398, 17);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(33, 13);
            this.label13.TabIndex = 91;
            this.label13.Text = "UNIT";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(340, 152);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(51, 13);
            this.label14.TabIndex = 93;
            this.label14.Text = "TARGET";
            // 
            // TxtTarget
            // 
            this.TxtTarget.Location = new System.Drawing.Point(397, 147);
            this.TxtTarget.Name = "TxtTarget";
            this.TxtTarget.Size = new System.Drawing.Size(52, 20);
            this.TxtTarget.TabIndex = 94;
            this.TxtTarget.TabStop = false;
            this.TxtTarget.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 182);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 13);
            this.label15.TabIndex = 95;
            this.label15.Text = "OPERATOR";
            // 
            // CmbOperator
            // 
            this.CmbOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbOperator.FormattingEnabled = true;
            this.CmbOperator.Location = new System.Drawing.Point(87, 179);
            this.CmbOperator.Name = "CmbOperator";
            this.CmbOperator.Size = new System.Drawing.Size(223, 21);
            this.CmbOperator.TabIndex = 96;
            // 
            // Frm_Order_Planning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 441);
            this.Controls.Add(this.GBMain);
            this.Name = "Frm_Order_Planning";
            this.Text = "Frm_Order_Planning";
            this.Load += new System.EventHandler(this.Frm_Order_Planning_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Order_Planning_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Order_Planning_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Sample)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox CmbColor;
        private System.Windows.Forms.ComboBox CmbStyle;
        private System.Windows.Forms.ComboBox CmbOCN;
        private System.Windows.Forms.CheckedListBox ChkLineList;
        private System.Windows.Forms.Label label8;
        private V_Components.MyTextBox TxtLinesCount;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox TxtBuyer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label6;
        private V_Components.MyTextBox TxtBufferPlanned;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox CmbBuyer;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox CmbBal;
        private System.Windows.Forms.ComboBox CmbProd;
        private System.Windows.Forms.ComboBox CmbBom;
        private V_Components.MyTextBox TxtPlanNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView Grid_Sample;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox CmbItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label11;
        private V_Components.MyTextBox TxtToBePlanning;
        private V_Components.MyTextBox TxtActPlanQty;
        private V_Components.MyTextBox TxtShift;
        private System.Windows.Forms.Label label12;
        private V_Components.MyTextBox TxtUnit;
        private System.Windows.Forms.Label label13;
        private V_Components.MyTextBox TxtTarget;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox CmbOperator;
        private System.Windows.Forms.Label label15;
    }
}