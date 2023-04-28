namespace Accounts
{
    partial class FrmSocksYarnSplRequestation
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
            this.GBQty = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.TxtTotalWeight = new V_Components.MyTextBox();
            this.GridDetail = new DotnetVFGrid.MyDataGridView();
            this.ButExit = new System.Windows.Forms.Button();
            this.ButOk = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtEnteredWeight = new V_Components.MyTextBox();
            this.Arrow4 = new System.Windows.Forms.PictureBox();
            this.LblSpecial = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.TxtJobOrderNo = new V_Components.MyTextBox();
            this.Arrow2 = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TxtUnit = new V_Components.MyTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TxtStyle = new V_Components.MyTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtRefNo = new V_Components.MyTextBox();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtReason = new V_Components.MyTextBox();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.TxtOrderNo = new V_Components.MyTextBox();
            this.TxtBuyer = new V_Components.MyTextBox();
            this.TxtTotal = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtEntryNo = new V_Components.MyTextBox();
            this.TxtEnter = new V_Components.MyTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.GBQty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).BeginInit();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBQty
            // 
            this.GBQty.Controls.Add(this.label12);
            this.GBQty.Controls.Add(this.TxtTotalWeight);
            this.GBQty.Controls.Add(this.GridDetail);
            this.GBQty.Controls.Add(this.ButExit);
            this.GBQty.Controls.Add(this.ButOk);
            this.GBQty.Controls.Add(this.label7);
            this.GBQty.Controls.Add(this.TxtEnteredWeight);
            this.GBQty.Location = new System.Drawing.Point(211, 194);
            this.GBQty.Name = "GBQty";
            this.GBQty.Size = new System.Drawing.Size(426, 203);
            this.GBQty.TabIndex = 4;
            this.GBQty.TabStop = false;
            this.GBQty.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(212, 150);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 13);
            this.label12.TabIndex = 77;
            this.label12.Text = "ENTERED";
            // 
            // TxtTotalWeight
            // 
            this.TxtTotalWeight.Location = new System.Drawing.Point(68, 147);
            this.TxtTotalWeight.Name = "TxtTotalWeight";
            this.TxtTotalWeight.Size = new System.Drawing.Size(138, 20);
            this.TxtTotalWeight.TabIndex = 76;
            this.TxtTotalWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // GridDetail
            // 
            this.GridDetail.AllowUserToAddRows = false;
            this.GridDetail.AllowUserToDeleteRows = false;
            this.GridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridDetail.Location = new System.Drawing.Point(12, 11);
            this.GridDetail.Name = "GridDetail";
            this.GridDetail.Size = new System.Drawing.Size(403, 123);
            this.GridDetail.TabIndex = 0;
            this.GridDetail.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.GridDetail_RowsAdded);
            this.GridDetail.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.GridDetail_EditingControlShowing);
            this.GridDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridDetail_KeyDown);
            this.GridDetail.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.GridDetail_RowsRemoved);
            // 
            // ButExit
            // 
            this.ButExit.Location = new System.Drawing.Point(329, 173);
            this.ButExit.Name = "ButExit";
            this.ButExit.Size = new System.Drawing.Size(86, 21);
            this.ButExit.TabIndex = 2;
            this.ButExit.Text = "E&XIT";
            this.ButExit.UseVisualStyleBackColor = true;
            this.ButExit.Click += new System.EventHandler(this.ButExit_Click);
            // 
            // ButOk
            // 
            this.ButOk.Location = new System.Drawing.Point(237, 173);
            this.ButOk.Name = "ButOk";
            this.ButOk.Size = new System.Drawing.Size(86, 21);
            this.ButOk.TabIndex = 1;
            this.ButOk.Text = "&OK";
            this.ButOk.UseVisualStyleBackColor = true;
            this.ButOk.Click += new System.EventHandler(this.ButOk_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 150);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 75;
            this.label7.Text = "TOTAL";
            // 
            // TxtEnteredWeight
            // 
            this.TxtEnteredWeight.Location = new System.Drawing.Point(277, 147);
            this.TxtEnteredWeight.Name = "TxtEnteredWeight";
            this.TxtEnteredWeight.Size = new System.Drawing.Size(138, 20);
            this.TxtEnteredWeight.TabIndex = 7;
            this.TxtEnteredWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Arrow4
            // 
            this.Arrow4.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow4.Location = new System.Drawing.Point(632, 43);
            this.Arrow4.Name = "Arrow4";
            this.Arrow4.Size = new System.Drawing.Size(26, 21);
            this.Arrow4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow4.TabIndex = 56;
            this.Arrow4.TabStop = false;
            // 
            // LblSpecial
            // 
            this.LblSpecial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.LblSpecial.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSpecial.Location = new System.Drawing.Point(11, 8);
            this.LblSpecial.Name = "LblSpecial";
            this.LblSpecial.Size = new System.Drawing.Size(647, 26);
            this.LblSpecial.TabIndex = 55;
            this.LblSpecial.Text = "YARN SPECIAL REQUESTATION";
            this.LblSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(347, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 16);
            this.label4.TabIndex = 53;
            this.label4.Text = "ORDER NO";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(11, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 51;
            this.label5.Text = "BUYER";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(171, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 16);
            this.label2.TabIndex = 49;
            this.label2.Text = "DATE";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(485, 449);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 16);
            this.label3.TabIndex = 48;
            this.label3.Text = "TOTAL";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(214, 44);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(98, 20);
            this.DtpDate.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "ENTRY NO";
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.TxtEnter);
            this.GBMain.Controls.Add(this.label13);
            this.GBMain.Controls.Add(this.label11);
            this.GBMain.Controls.Add(this.TxtJobOrderNo);
            this.GBMain.Controls.Add(this.Arrow2);
            this.GBMain.Controls.Add(this.label10);
            this.GBMain.Controls.Add(this.TxtUnit);
            this.GBMain.Controls.Add(this.label9);
            this.GBMain.Controls.Add(this.TxtStyle);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.TxtRefNo);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.TxtReason);
            this.GBMain.Controls.Add(this.GBQty);
            this.GBMain.Controls.Add(this.Arrow4);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.LblSpecial);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.TxtOrderNo);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtBuyer);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.TxtEntryNo);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(6, 6);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(669, 475);
            this.GBMain.TabIndex = 3;
            this.GBMain.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(347, 140);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(95, 16);
            this.label11.TabIndex = 70;
            this.label11.Text = "JOB ORD NO";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // TxtJobOrderNo
            // 
            this.TxtJobOrderNo.Location = new System.Drawing.Point(448, 139);
            this.TxtJobOrderNo.Name = "TxtJobOrderNo";
            this.TxtJobOrderNo.Size = new System.Drawing.Size(210, 20);
            this.TxtJobOrderNo.TabIndex = 69;
            this.TxtJobOrderNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtJobOrderNo.TextChanged += new System.EventHandler(this.myTextBox1_TextChanged);
            // 
            // Arrow2
            // 
            this.Arrow2.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow2.Location = new System.Drawing.Point(633, 110);
            this.Arrow2.Name = "Arrow2";
            this.Arrow2.Size = new System.Drawing.Size(26, 21);
            this.Arrow2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow2.TabIndex = 68;
            this.Arrow2.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(347, 111);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 16);
            this.label10.TabIndex = 67;
            this.label10.Text = "UNIT";
            // 
            // TxtUnit
            // 
            this.TxtUnit.Location = new System.Drawing.Point(450, 110);
            this.TxtUnit.Name = "TxtUnit";
            this.TxtUnit.Size = new System.Drawing.Size(176, 20);
            this.TxtUnit.TabIndex = 66;
            this.TxtUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(11, 110);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 16);
            this.label9.TabIndex = 64;
            this.label9.Text = "STYLE";
            // 
            // TxtStyle
            // 
            this.TxtStyle.Location = new System.Drawing.Point(92, 108);
            this.TxtStyle.Name = "TxtStyle";
            this.TxtStyle.Size = new System.Drawing.Size(220, 20);
            this.TxtStyle.TabIndex = 63;
            this.TxtStyle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(11, 139);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 16);
            this.label8.TabIndex = 61;
            this.label8.Text = "REF NO";
            // 
            // TxtRefNo
            // 
            this.TxtRefNo.Location = new System.Drawing.Point(91, 138);
            this.TxtRefNo.Name = "TxtRefNo";
            this.TxtRefNo.Size = new System.Drawing.Size(221, 20);
            this.TxtRefNo.TabIndex = 60;
            this.TxtRefNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow1.Location = new System.Drawing.Point(632, 76);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(26, 21);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 59;
            this.Arrow1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(347, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 16);
            this.label6.TabIndex = 58;
            this.label6.Text = "REASON";
            // 
            // TxtReason
            // 
            this.TxtReason.Location = new System.Drawing.Point(449, 76);
            this.TxtReason.Name = "TxtReason";
            this.TxtReason.Size = new System.Drawing.Size(177, 20);
            this.TxtReason.TabIndex = 57;
            this.TxtReason.TabStop = false;
            this.TxtReason.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(8, 448);
            this.TxtRemarks.Multiline = true;
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(258, 21);
            this.TxtRemarks.TabIndex = 4;
            this.TxtRemarks.TextChanged += new System.EventHandler(this.TxtRemarks_TextChanged);
            // 
            // TxtOrderNo
            // 
            this.TxtOrderNo.Location = new System.Drawing.Point(449, 43);
            this.TxtOrderNo.Name = "TxtOrderNo";
            this.TxtOrderNo.Size = new System.Drawing.Size(177, 20);
            this.TxtOrderNo.TabIndex = 1;
            this.TxtOrderNo.TabStop = false;
            this.TxtOrderNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtBuyer
            // 
            this.TxtBuyer.Location = new System.Drawing.Point(92, 76);
            this.TxtBuyer.Name = "TxtBuyer";
            this.TxtBuyer.Size = new System.Drawing.Size(220, 20);
            this.TxtBuyer.TabIndex = 2;
            this.TxtBuyer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(548, 449);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(110, 20);
            this.TxtTotal.TabIndex = 5;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtTotal.TextChanged += new System.EventHandler(this.TxtTotal_TextChanged);
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(11, 174);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(647, 268);
            this.Grid.TabIndex = 3;
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // TxtEntryNo
            // 
            this.TxtEntryNo.Location = new System.Drawing.Point(91, 44);
            this.TxtEntryNo.Name = "TxtEntryNo";
            this.TxtEntryNo.Size = new System.Drawing.Size(74, 20);
            this.TxtEntryNo.TabIndex = 0;
            this.TxtEntryNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtEnter
            // 
            this.TxtEnter.Location = new System.Drawing.Point(350, 448);
            this.TxtEnter.Name = "TxtEnter";
            this.TxtEnter.Size = new System.Drawing.Size(110, 20);
            this.TxtEnter.TabIndex = 71;
            this.TxtEnter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(272, 450);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(72, 16);
            this.label13.TabIndex = 72;
            this.label13.Text = "ENTERED";
            // 
            // FrmSocksYarnSplRequestation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 488);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmSocksYarnSplRequestation";
            this.Text = "FrmSocksYarnSplRequestation";
            this.Load += new System.EventHandler(this.FrmSocksYarnSplRequestation_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmSocksYarnSplRequestation_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSocksYarnSplRequestation_KeyDown);
            this.GBQty.ResumeLayout(false);
            this.GBQty.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).EndInit();
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBQty;
        private DotnetVFGrid.MyDataGridView GridDetail;
        private System.Windows.Forms.Button ButExit;
        private System.Windows.Forms.Button ButOk;
        private System.Windows.Forms.PictureBox Arrow4;
        private V_Components.MyTextBox TxtRemarks;
        private System.Windows.Forms.Label LblSpecial;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtOrderNo;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtBuyer;
        private System.Windows.Forms.Label label2;
        private V_Components.MyTextBox TxtTotal;
        private System.Windows.Forms.Label label3;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private V_Components.MyTextBox TxtEntryNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label7;
        private V_Components.MyTextBox TxtEnteredWeight;
        private System.Windows.Forms.PictureBox Arrow1;
        private System.Windows.Forms.Label label6;
        private V_Components.MyTextBox TxtReason;
        private System.Windows.Forms.Label label8;
        private V_Components.MyTextBox TxtRefNo;
        private System.Windows.Forms.Label label9;
        private V_Components.MyTextBox TxtStyle;
        private System.Windows.Forms.PictureBox Arrow2;
        private System.Windows.Forms.Label label10;
        private V_Components.MyTextBox TxtUnit;
        private System.Windows.Forms.Label label11;
        private V_Components.MyTextBox TxtJobOrderNo;
        private System.Windows.Forms.Label label12;
        private V_Components.MyTextBox TxtTotalWeight;
        private V_Components.MyTextBox TxtEnter;
        private System.Windows.Forms.Label label13;
    }
}