namespace Accounts
{
    partial class FrmSocksYarnPOApproval
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
            this.ButApp = new System.Windows.Forms.Button();
            this.ButClr = new System.Windows.Forms.Button();
            this.ButExit = new System.Windows.Forms.Button();
            this.TxtAmount = new V_Components.MyTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtQTY = new V_Components.MyTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtTotal = new V_Components.MyTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.DtpReqDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.Grid_Tax = new DotnetVFGrid.MyDataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.Grid_OCN = new DotnetVFGrid.MyDataGridView();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.Grid_Item_OCN = new DotnetVFGrid.MyDataGridView();
            this.Grid_Item = new DotnetVFGrid.MyDataGridView();
            this.TxtBuyer = new V_Components.MyTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CmbBasedOn = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.TxtSupplier = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.TxtPONO = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CmbType = new System.Windows.Forms.ComboBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Tax)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_OCN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Item_OCN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Item)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.ButApp);
            this.GBMain.Controls.Add(this.ButClr);
            this.GBMain.Controls.Add(this.ButExit);
            this.GBMain.Controls.Add(this.TxtAmount);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.TxtQTY);
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.DtpReqDate);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.Grid_Tax);
            this.GBMain.Controls.Add(this.tabControl1);
            this.GBMain.Controls.Add(this.TxtBuyer);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.CmbType);
            this.GBMain.Controls.Add(this.CmbBasedOn);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.TxtSupplier);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.TxtPONO);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(6, 0);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(741, 490);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // ButApp
            // 
            this.ButApp.Location = new System.Drawing.Point(597, 370);
            this.ButApp.Name = "ButApp";
            this.ButApp.Size = new System.Drawing.Size(113, 31);
            this.ButApp.TabIndex = 1;
            this.ButApp.Text = "&APPROVE";
            this.ButApp.UseVisualStyleBackColor = true;
            this.ButApp.Click += new System.EventHandler(this.ButApp_Click);
            // 
            // ButClr
            // 
            this.ButClr.Location = new System.Drawing.Point(597, 410);
            this.ButClr.Name = "ButClr";
            this.ButClr.Size = new System.Drawing.Size(113, 31);
            this.ButClr.TabIndex = 2;
            this.ButClr.Text = "&CLEAR";
            this.ButClr.UseVisualStyleBackColor = true;
            this.ButClr.Click += new System.EventHandler(this.ButClr_Click);
            // 
            // ButExit
            // 
            this.ButExit.Location = new System.Drawing.Point(597, 450);
            this.ButExit.Name = "ButExit";
            this.ButExit.Size = new System.Drawing.Size(113, 31);
            this.ButExit.TabIndex = 3;
            this.ButExit.Text = "E&XIT";
            this.ButExit.UseVisualStyleBackColor = true;
            this.ButExit.Click += new System.EventHandler(this.ButExit_Click);
            // 
            // TxtAmount
            // 
            this.TxtAmount.Location = new System.Drawing.Point(236, 461);
            this.TxtAmount.Name = "TxtAmount";
            this.TxtAmount.Size = new System.Drawing.Size(83, 21);
            this.TxtAmount.TabIndex = 43;
            this.TxtAmount.TabStop = false;
            this.TxtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(181, 465);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "GROSS";
            // 
            // TxtQTY
            // 
            this.TxtQTY.BackColor = System.Drawing.Color.White;
            this.TxtQTY.Location = new System.Drawing.Point(361, 461);
            this.TxtQTY.Name = "TxtQTY";
            this.TxtQTY.Size = new System.Drawing.Size(67, 21);
            this.TxtQTY.TabIndex = 45;
            this.TxtQTY.TabStop = false;
            this.TxtQTY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtQTY.TextChanged += new System.EventHandler(this.myTextBox2_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(325, 465);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 44;
            this.label7.Text = "QTY";
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(496, 461);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(85, 21);
            this.TxtTotal.TabIndex = 47;
            this.TxtTotal.TabStop = false;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(434, 465);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 46;
            this.label8.Text = "AMOUNT";
            // 
            // DtpReqDate
            // 
            this.DtpReqDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpReqDate.Location = new System.Drawing.Point(89, 461);
            this.DtpReqDate.Name = "DtpReqDate";
            this.DtpReqDate.Size = new System.Drawing.Size(86, 21);
            this.DtpReqDate.TabIndex = 8;
            this.DtpReqDate.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 465);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 41;
            this.label5.Text = "REQ. DATE";
            // 
            // Grid_Tax
            // 
            this.Grid_Tax.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid_Tax.Location = new System.Drawing.Point(13, 367);
            this.Grid_Tax.Name = "Grid_Tax";
            this.Grid_Tax.Size = new System.Drawing.Size(568, 88);
            this.Grid_Tax.TabIndex = 7;
            this.Grid_Tax.TabStop = false;
            this.Grid_Tax.DoubleClick += new System.EventHandler(this.Grid_Tax_DoubleClick);
            this.Grid_Tax.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_Tax_RowsAdded);
            this.Grid_Tax.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_Tax_EditingControlShowing);
            this.Grid_Tax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_Tax_KeyPress);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 84);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(722, 280);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.TabStop = false;
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.Grid_OCN);
            this.tabPage1.Controls.Add(this.Grid);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(714, 254);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "OCN WISE";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 226);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(94, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.TabStop = false;
            this.checkBox1.Text = "&SELECT ALL";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(117, 222);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.TabStop = false;
            this.button1.Text = "&OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Grid_OCN
            // 
            this.Grid_OCN.AllowUserToAddRows = false;
            this.Grid_OCN.AllowUserToDeleteRows = false;
            this.Grid_OCN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid_OCN.Location = new System.Drawing.Point(6, 8);
            this.Grid_OCN.Name = "Grid_OCN";
            this.Grid_OCN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid_OCN.Size = new System.Drawing.Size(186, 200);
            this.Grid_OCN.TabIndex = 0;
            this.Grid_OCN.TabStop = false;
            this.Grid_OCN.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_OCN_CellContentClick);
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(198, 8);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid.Size = new System.Drawing.Size(510, 235);
            this.Grid.TabIndex = 1;
            this.Grid.TabStop = false;
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.Grid_Item_OCN);
            this.tabPage2.Controls.Add(this.Grid_Item);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(714, 254);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ITEM WISE";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(598, 147);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 28);
            this.button2.TabIndex = 3;
            this.button2.Text = "&FILL";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Grid_Item_OCN
            // 
            this.Grid_Item_OCN.AllowUserToAddRows = false;
            this.Grid_Item_OCN.AllowUserToDeleteRows = false;
            this.Grid_Item_OCN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid_Item_OCN.Location = new System.Drawing.Point(6, 147);
            this.Grid_Item_OCN.Name = "Grid_Item_OCN";
            this.Grid_Item_OCN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid_Item_OCN.Size = new System.Drawing.Size(577, 98);
            this.Grid_Item_OCN.TabIndex = 1;
            this.Grid_Item_OCN.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_Item_OCN_EditingControlShowing);
            this.Grid_Item_OCN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_Item_OCN_KeyDown);
            this.Grid_Item_OCN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_Item_OCN_KeyPress);
            // 
            // Grid_Item
            // 
            this.Grid_Item.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid_Item.Location = new System.Drawing.Point(6, 8);
            this.Grid_Item.Name = "Grid_Item";
            this.Grid_Item.Size = new System.Drawing.Size(701, 133);
            this.Grid_Item.TabIndex = 0;
            this.Grid_Item.DoubleClick += new System.EventHandler(this.Grid_Item_DoubleClick);
            this.Grid_Item.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_Item_EditingControlShowing);
            this.Grid_Item.CurrentCellChanged += new System.EventHandler(this.Grid_Item_CurrentCellChanged);
            this.Grid_Item.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_Item_KeyDown);
            this.Grid_Item.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_Item_KeyPress);
            // 
            // TxtBuyer
            // 
            this.TxtBuyer.Location = new System.Drawing.Point(415, 55);
            this.TxtBuyer.Name = "TxtBuyer";
            this.TxtBuyer.Size = new System.Drawing.Size(309, 21);
            this.TxtBuyer.TabIndex = 5;
            this.TxtBuyer.TabStop = false;
            this.TxtBuyer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtBuyer.TextChanged += new System.EventHandler(this.myTextBox3_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(344, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "BUYER";
            // 
            // CmbBasedOn
            // 
            this.CmbBasedOn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBasedOn.FormattingEnabled = true;
            this.CmbBasedOn.Location = new System.Drawing.Point(242, 55);
            this.CmbBasedOn.Name = "CmbBasedOn";
            this.CmbBasedOn.Size = new System.Drawing.Size(90, 21);
            this.CmbBasedOn.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "TYPE";
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow3.Location = new System.Drawing.Point(208, 25);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(26, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 31;
            this.Arrow3.TabStop = false;
            // 
            // TxtSupplier
            // 
            this.TxtSupplier.Location = new System.Drawing.Point(415, 25);
            this.TxtSupplier.Name = "TxtSupplier";
            this.TxtSupplier.Size = new System.Drawing.Size(310, 21);
            this.TxtSupplier.TabIndex = 2;
            this.TxtSupplier.TabStop = false;
            this.TxtSupplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(344, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SUPPLIER";
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(242, 25);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(90, 21);
            this.DtpDate.TabIndex = 1;
            this.DtpDate.TabStop = false;
            this.DtpDate.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // TxtPONO
            // 
            this.TxtPONO.Location = new System.Drawing.Point(92, 25);
            this.TxtPONO.Name = "TxtPONO";
            this.TxtPONO.Size = new System.Drawing.Size(108, 21);
            this.TxtPONO.TabIndex = 0;
            this.TxtPONO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "PO NO";
            // 
            // CmbType
            // 
            this.CmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbType.FormattingEnabled = true;
            this.CmbType.Items.AddRange(new object[] {
            "APPROVE",
            "REJECT"});
            this.CmbType.Location = new System.Drawing.Point(92, 55);
            this.CmbType.Name = "CmbType";
            this.CmbType.Size = new System.Drawing.Size(142, 21);
            this.CmbType.TabIndex = 3;
            // 
            // FrmSocksYarnPOApproval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 502);
            this.Controls.Add(this.GBMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "FrmSocksYarnPOApproval";
            this.Text = "SOCKS YARN PO APPROVAL ...!";
            this.Load += new System.EventHandler(this.FrmSocksYarnPOApproval_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmSocksYarnPOApproval_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSocksYarnPOApproval_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Tax)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_OCN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Item_OCN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Item)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private V_Components.MyTextBox TxtPONO;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtSupplier;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.PictureBox Arrow3;
        private System.Windows.Forms.ComboBox CmbBasedOn;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox TxtBuyer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private DotnetVFGrid.MyDataGridView Grid_OCN;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.DateTimePicker DtpReqDate;
        private System.Windows.Forms.Label label5;
        private DotnetVFGrid.MyDataGridView Grid_Tax;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private V_Components.MyTextBox TxtQTY;
        private System.Windows.Forms.Label label7;
        private V_Components.MyTextBox TxtAmount;
        private System.Windows.Forms.Label label6;
        private V_Components.MyTextBox TxtTotal;
        private System.Windows.Forms.Label label8;
        private DotnetVFGrid.MyDataGridView Grid_Item;
        private DotnetVFGrid.MyDataGridView Grid_Item_OCN;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button ButApp;
        private System.Windows.Forms.Button ButClr;
        private System.Windows.Forms.Button ButExit;
        private System.Windows.Forms.ComboBox CmbType;
    }
}