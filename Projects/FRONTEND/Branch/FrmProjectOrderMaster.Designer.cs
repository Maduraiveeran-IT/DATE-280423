namespace Accounts
{
    partial class FrmProjectOrderMaster
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
            this.GBImage = new System.Windows.Forms.GroupBox();
            this.ButCancel = new System.Windows.Forms.Button();
            this.ButOK = new System.Windows.Forms.Button();
            this.Img1 = new System.Windows.Forms.PictureBox();
            this.ChkCopy = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.DtpODate = new System.Windows.Forms.DateTimePicker();
            this.Arrow_Buyer = new System.Windows.Forms.PictureBox();
            this.Arrow_Name = new System.Windows.Forms.PictureBox();
            this.Arrow_Empl = new System.Windows.Forms.PictureBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtPrjType = new V_Components.MyTextBox();
            this.LblResponse = new System.Windows.Forms.Label();
            this.TxtPrjNo = new V_Components.MyTextBox();
            this.TxtBuyer = new V_Components.MyTextBox();
            this.TxtEmployee = new V_Components.MyTextBox();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.TxtTotOrderQty = new V_Components.MyTextBox();
            this.TxtNetAmount = new V_Components.MyTextBox();
            this.TxtTotalBom = new V_Components.MyTextBox();
            this.BtnUpd = new System.Windows.Forms.Button();
            this.ButFDel = new System.Windows.Forms.Button();
            this.GBMain.SuspendLayout();
            this.GBImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Img1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow_Buyer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow_Name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow_Empl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.ButFDel);
            this.GBMain.Controls.Add(this.BtnUpd);
            this.GBMain.Controls.Add(this.GBImage);
            this.GBMain.Controls.Add(this.ChkCopy);
            this.GBMain.Controls.Add(this.label11);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.DtpODate);
            this.GBMain.Controls.Add(this.Arrow_Buyer);
            this.GBMain.Controls.Add(this.Arrow_Name);
            this.GBMain.Controls.Add(this.Arrow_Empl);
            this.GBMain.Controls.Add(this.listBox1);
            this.GBMain.Controls.Add(this.label10);
            this.GBMain.Controls.Add(this.label14);
            this.GBMain.Controls.Add(this.label15);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtPrjType);
            this.GBMain.Controls.Add(this.LblResponse);
            this.GBMain.Controls.Add(this.TxtPrjNo);
            this.GBMain.Controls.Add(this.TxtBuyer);
            this.GBMain.Controls.Add(this.TxtEmployee);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.TxtTotOrderQty);
            this.GBMain.Controls.Add(this.TxtNetAmount);
            this.GBMain.Controls.Add(this.TxtTotalBom);
            this.GBMain.Location = new System.Drawing.Point(16, 9);
            this.GBMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GBMain.Name = "GBMain";
            this.GBMain.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GBMain.Size = new System.Drawing.Size(876, 504);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            this.GBMain.Enter += new System.EventHandler(this.GBMain_Enter);
            // 
            // GBImage
            // 
            this.GBImage.Controls.Add(this.ButCancel);
            this.GBImage.Controls.Add(this.ButOK);
            this.GBImage.Controls.Add(this.Img1);
            this.GBImage.Location = new System.Drawing.Point(525, 133);
            this.GBImage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GBImage.Name = "GBImage";
            this.GBImage.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GBImage.Size = new System.Drawing.Size(304, 282);
            this.GBImage.TabIndex = 79;
            this.GBImage.TabStop = false;
            this.GBImage.Visible = false;
            // 
            // ButCancel
            // 
            this.ButCancel.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButCancel.Location = new System.Drawing.Point(152, 241);
            this.ButCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ButCancel.Name = "ButCancel";
            this.ButCancel.Size = new System.Drawing.Size(100, 28);
            this.ButCancel.TabIndex = 2;
            this.ButCancel.Text = "CANCEL";
            this.ButCancel.UseVisualStyleBackColor = true;
            this.ButCancel.Click += new System.EventHandler(this.ButCancel_Click);
            // 
            // ButOK
            // 
            this.ButOK.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButOK.Location = new System.Drawing.Point(32, 241);
            this.ButOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ButOK.Name = "ButOK";
            this.ButOK.Size = new System.Drawing.Size(100, 28);
            this.ButOK.TabIndex = 1;
            this.ButOK.Text = "ADD";
            this.ButOK.UseVisualStyleBackColor = true;
            this.ButOK.Click += new System.EventHandler(this.ButOK_Click);
            // 
            // Img1
            // 
            this.Img1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Img1.Location = new System.Drawing.Point(8, 22);
            this.Img1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Img1.Name = "Img1";
            this.Img1.Size = new System.Drawing.Size(287, 211);
            this.Img1.TabIndex = 0;
            this.Img1.TabStop = false;
            // 
            // ChkCopy
            // 
            this.ChkCopy.AutoSize = true;
            this.ChkCopy.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkCopy.Location = new System.Drawing.Point(433, 22);
            this.ChkCopy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ChkCopy.Name = "ChkCopy";
            this.ChkCopy.Size = new System.Drawing.Size(69, 21);
            this.ChkCopy.TabIndex = 1;
            this.ChkCopy.TabStop = false;
            this.ChkCopy.Text = "COPY";
            this.ChkCopy.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(521, 22);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(95, 19);
            this.label11.TabIndex = 78;
            this.label11.Text = "PRJ NAME";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(27, 23);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 19);
            this.label5.TabIndex = 78;
            this.label5.Text = "PRJ NO";
            // 
            // DtpODate
            // 
            this.DtpODate.CustomFormat = "dd/MM/yyyy";
            this.DtpODate.Enabled = false;
            this.DtpODate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpODate.Location = new System.Drawing.Point(324, 20);
            this.DtpODate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DtpODate.Name = "DtpODate";
            this.DtpODate.Size = new System.Drawing.Size(100, 22);
            this.DtpODate.TabIndex = 0;
            this.DtpODate.TabStop = false;
            this.DtpODate.Value = new System.DateTime(2013, 10, 19, 0, 0, 0, 0);
            this.DtpODate.Leave += new System.EventHandler(this.DtpEDate_Leave);
            // 
            // Arrow_Buyer
            // 
            this.Arrow_Buyer.Image = global::Accounts.Properties.Resources.Down;
            this.Arrow_Buyer.Location = new System.Drawing.Point(433, 58);
            this.Arrow_Buyer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Arrow_Buyer.Name = "Arrow_Buyer";
            this.Arrow_Buyer.Size = new System.Drawing.Size(33, 26);
            this.Arrow_Buyer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow_Buyer.TabIndex = 75;
            this.Arrow_Buyer.TabStop = false;
            this.Arrow_Buyer.Click += new System.EventHandler(this.Arrow_Buyer_Click);
            // 
            // Arrow_Name
            // 
            this.Arrow_Name.Image = global::Accounts.Properties.Resources.Down;
            this.Arrow_Name.Location = new System.Drawing.Point(828, 16);
            this.Arrow_Name.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Arrow_Name.Name = "Arrow_Name";
            this.Arrow_Name.Size = new System.Drawing.Size(33, 26);
            this.Arrow_Name.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow_Name.TabIndex = 75;
            this.Arrow_Name.TabStop = false;
            this.Arrow_Name.Click += new System.EventHandler(this.Arrow_Name_Click);
            // 
            // Arrow_Empl
            // 
            this.Arrow_Empl.Image = global::Accounts.Properties.Resources.Down;
            this.Arrow_Empl.Location = new System.Drawing.Point(828, 62);
            this.Arrow_Empl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Arrow_Empl.Name = "Arrow_Empl";
            this.Arrow_Empl.Size = new System.Drawing.Size(33, 26);
            this.Arrow_Empl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow_Empl.TabIndex = 75;
            this.Arrow_Empl.TabStop = false;
            this.Arrow_Empl.Click += new System.EventHandler(this.Arrow_Merch_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(39, 464);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(41, 4);
            this.listBox1.TabIndex = 74;
            this.listBox1.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(32, 437);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 19);
            this.label10.TabIndex = 56;
            this.label10.Text = "REMARKS";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(545, 437);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(87, 19);
            this.label14.TabIndex = 56;
            this.label14.Text = "AMOUMT";
            this.label14.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(644, 437);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(43, 19);
            this.label15.TabIndex = 56;
            this.label15.Text = "QTY";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(545, 437);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 19);
            this.label4.TabIndex = 56;
            this.label4.Text = "BOM";
            this.label4.Visible = false;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(31, 108);
            this.Grid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid.Size = new System.Drawing.Size(831, 306);
            this.Grid.TabIndex = 5;
            this.Grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellContentClick);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            this.Grid.Leave += new System.EventHandler(this.Grid_Leave);
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(27, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 19);
            this.label2.TabIndex = 15;
            this.label2.Text = "BUYER";
            // 
            // TxtPrjType
            // 
            this.TxtPrjType.Location = new System.Drawing.Point(623, 20);
            this.TxtPrjType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtPrjType.Name = "TxtPrjType";
            this.TxtPrjType.Size = new System.Drawing.Size(196, 22);
            this.TxtPrjType.TabIndex = 0;
            this.TxtPrjType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LblResponse
            // 
            this.LblResponse.AutoSize = true;
            this.LblResponse.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblResponse.Location = new System.Drawing.Point(521, 60);
            this.LblResponse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblResponse.Name = "LblResponse";
            this.LblResponse.Size = new System.Drawing.Size(90, 19);
            this.LblResponse.TabIndex = 15;
            this.LblResponse.Text = "ENTRY BY";
            this.LblResponse.Click += new System.EventHandler(this.label6_Click);
            // 
            // TxtPrjNo
            // 
            this.TxtPrjNo.Location = new System.Drawing.Point(159, 20);
            this.TxtPrjNo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtPrjNo.Name = "TxtPrjNo";
            this.TxtPrjNo.Size = new System.Drawing.Size(156, 22);
            this.TxtPrjNo.TabIndex = 0;
            this.TxtPrjNo.TabStop = false;
            this.TxtPrjNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtBuyer
            // 
            this.TxtBuyer.Location = new System.Drawing.Point(159, 58);
            this.TxtBuyer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtBuyer.Name = "TxtBuyer";
            this.TxtBuyer.Size = new System.Drawing.Size(265, 22);
            this.TxtBuyer.TabIndex = 1;
            this.TxtBuyer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtEmployee
            // 
            this.TxtEmployee.Location = new System.Drawing.Point(623, 62);
            this.TxtEmployee.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtEmployee.Name = "TxtEmployee";
            this.TxtEmployee.Size = new System.Drawing.Size(196, 22);
            this.TxtEmployee.TabIndex = 3;
            this.TxtEmployee.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtEmployee.TextChanged += new System.EventHandler(this.TxtDept_TextChanged);
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(132, 433);
            this.TxtRemarks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(380, 22);
            this.TxtRemarks.TabIndex = 6;
            this.TxtRemarks.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtTotOrderQty
            // 
            this.TxtTotOrderQty.Location = new System.Drawing.Point(708, 433);
            this.TxtTotOrderQty.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtTotOrderQty.Name = "TxtTotOrderQty";
            this.TxtTotOrderQty.Size = new System.Drawing.Size(152, 22);
            this.TxtTotOrderQty.TabIndex = 9;
            this.TxtTotOrderQty.TabStop = false;
            this.TxtTotOrderQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtNetAmount
            // 
            this.TxtNetAmount.Location = new System.Drawing.Point(549, 433);
            this.TxtNetAmount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtNetAmount.Name = "TxtNetAmount";
            this.TxtNetAmount.Size = new System.Drawing.Size(80, 22);
            this.TxtNetAmount.TabIndex = 9;
            this.TxtNetAmount.TabStop = false;
            this.TxtNetAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtNetAmount.Visible = false;
            // 
            // TxtTotalBom
            // 
            this.TxtTotalBom.Location = new System.Drawing.Point(549, 433);
            this.TxtTotalBom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtTotalBom.Name = "TxtTotalBom";
            this.TxtTotalBom.Size = new System.Drawing.Size(77, 22);
            this.TxtTotalBom.TabIndex = 9;
            this.TxtTotalBom.TabStop = false;
            this.TxtTotalBom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtTotalBom.Visible = false;
            // 
            // BtnUpd
            // 
            this.BtnUpd.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnUpd.Location = new System.Drawing.Point(132, 464);
            this.BtnUpd.Margin = new System.Windows.Forms.Padding(4);
            this.BtnUpd.Name = "BtnUpd";
            this.BtnUpd.Size = new System.Drawing.Size(100, 28);
            this.BtnUpd.TabIndex = 3;
            this.BtnUpd.Text = "UPDATE";
            this.BtnUpd.UseVisualStyleBackColor = true;
            this.BtnUpd.Click += new System.EventHandler(this.BtnUpd_Click);
            // 
            // ButFDel
            // 
            this.ButFDel.Font = new System.Drawing.Font("Times New Roman", 9.7F, System.Drawing.FontStyle.Bold);
            this.ButFDel.Location = new System.Drawing.Point(255, 464);
            this.ButFDel.Margin = new System.Windows.Forms.Padding(4);
            this.ButFDel.Name = "ButFDel";
            this.ButFDel.Size = new System.Drawing.Size(100, 28);
            this.ButFDel.TabIndex = 80;
            this.ButFDel.Text = "DELETE";
            this.ButFDel.UseVisualStyleBackColor = true;
            this.ButFDel.Click += new System.EventHandler(this.ButFDel_Click_1);
            // 
            // FrmProjectOrderMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 516);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmProjectOrderMaster";
            this.Text = "FrmProjectOrderMaster";
            this.Load += new System.EventHandler(this.FrmProjectOrderMaster_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmProjectOrderMaster_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmProjectOrderMaster_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.GBImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Img1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow_Buyer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow_Name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow_Empl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private DotnetVFGrid.MyDataGridView Grid;
        private V_Components.MyTextBox TxtTotalBom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBox1;
        private V_Components.MyTextBox TxtPrjNo;
        private System.Windows.Forms.DateTimePicker DtpODate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox Arrow_Empl;
        private System.Windows.Forms.Label label2;
        private V_Components.MyTextBox TxtBuyer;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private V_Components.MyTextBox TxtRemarks;
        private System.Windows.Forms.PictureBox Arrow_Buyer;
        private System.Windows.Forms.Label LblResponse;
        private V_Components.MyTextBox TxtEmployee;
        private System.Windows.Forms.Label label14;
        private V_Components.MyTextBox TxtNetAmount;
        private System.Windows.Forms.GroupBox GBImage;
        private System.Windows.Forms.PictureBox Img1;
        private System.Windows.Forms.Button ButCancel;
        private System.Windows.Forms.Button ButOK;
        private System.Windows.Forms.Label label15;
        private V_Components.MyTextBox TxtTotOrderQty;
        private System.Windows.Forms.CheckBox ChkCopy;
        private V_Components.MyTextBox TxtPrjType;
        private System.Windows.Forms.PictureBox Arrow_Name;
        private System.Windows.Forms.Button BtnUpd;
        private System.Windows.Forms.Button ButFDel;
    }
}