namespace Accounts
{
    partial class Frm_ManDays_Cost_Approval
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
            this.LblSpecial = new System.Windows.Forms.Label();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtTotOrder = new V_Components.MyTextBox();
            this.DtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtEno = new V_Components.MyTextBox();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.Btn_Approve = new System.Windows.Forms.Button();
            this.Btn_Exit = new System.Windows.Forms.Button();
            this.TxtSupplier = new V_Components.MyTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Arrow4 = new System.Windows.Forms.PictureBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).BeginInit();
            this.SuspendLayout();
            // 
            // LblSpecial
            // 
            this.LblSpecial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.LblSpecial.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSpecial.Location = new System.Drawing.Point(11, 8);
            this.LblSpecial.Name = "LblSpecial";
            this.LblSpecial.Size = new System.Drawing.Size(429, 25);
            this.LblSpecial.TabIndex = 61;
            this.LblSpecial.Text = "PROJECT MANDAYS COST APPROVAL";
            this.LblSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.Arrow4);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.TxtSupplier);
            this.GBMain.Controls.Add(this.Btn_Cancel);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.Btn_Approve);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.Btn_Exit);
            this.GBMain.Controls.Add(this.label10);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.TxtEno);
            this.GBMain.Controls.Add(this.LblSpecial);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtTotOrder);
            this.GBMain.Controls.Add(this.DtpDate1);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Location = new System.Drawing.Point(6, -3);
            this.GBMain.Margin = new System.Windows.Forms.Padding(2);
            this.GBMain.Name = "GBMain";
            this.GBMain.Padding = new System.Windows.Forms.Padding(2);
            this.GBMain.Size = new System.Drawing.Size(448, 458);
            this.GBMain.TabIndex = 5;
            this.GBMain.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(292, 397);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 15);
            this.label1.TabIndex = 60;
            this.label1.Text = "TOTAL";
            // 
            // TxtTotOrder
            // 
            this.TxtTotOrder.Location = new System.Drawing.Point(347, 395);
            this.TxtTotOrder.Name = "TxtTotOrder";
            this.TxtTotOrder.Size = new System.Drawing.Size(93, 20);
            this.TxtTotOrder.TabIndex = 5;
            this.TxtTotOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DtpDate1
            // 
            this.DtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate1.Location = new System.Drawing.Point(355, 40);
            this.DtpDate1.Margin = new System.Windows.Forms.Padding(2);
            this.DtpDate1.Name = "DtpDate1";
            this.DtpDate1.Size = new System.Drawing.Size(85, 20);
            this.DtpDate1.TabIndex = 1;
            // 
            // Grid
            // 
            this.Grid.AllowUserToOrderColumns = true;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(14, 91);
            this.Grid.Margin = new System.Windows.Forms.Padding(2);
            this.Grid.Name = "Grid";
            this.Grid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Grid.Size = new System.Drawing.Size(426, 299);
            this.Grid.TabIndex = 3;
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // TxtEno
            // 
            this.TxtEno.Location = new System.Drawing.Point(86, 40);
            this.TxtEno.Name = "TxtEno";
            this.TxtEno.Size = new System.Drawing.Size(126, 20);
            this.TxtEno.TabIndex = 0;
            this.TxtEno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(86, 395);
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(200, 20);
            this.TxtRemarks.TabIndex = 4;
            this.TxtRemarks.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(11, 397);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 15);
            this.label10.TabIndex = 65;
            this.label10.Text = "REMARKS";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 15);
            this.label3.TabIndex = 66;
            this.label3.Text = "ENTRY NO";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(267, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 15);
            this.label2.TabIndex = 67;
            this.label2.Text = "ENTRY DATE";
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Location = new System.Drawing.Point(178, 422);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(74, 31);
            this.Btn_Cancel.TabIndex = 7;
            this.Btn_Cancel.Text = "&CANCEL";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // Btn_Approve
            // 
            this.Btn_Approve.Location = new System.Drawing.Point(98, 422);
            this.Btn_Approve.Name = "Btn_Approve";
            this.Btn_Approve.Size = new System.Drawing.Size(74, 31);
            this.Btn_Approve.TabIndex = 6;
            this.Btn_Approve.Text = "&APPROVE";
            this.Btn_Approve.UseVisualStyleBackColor = true;
            this.Btn_Approve.Click += new System.EventHandler(this.Btn_Approve_Click);
            // 
            // Btn_Exit
            // 
            this.Btn_Exit.Location = new System.Drawing.Point(257, 422);
            this.Btn_Exit.Name = "Btn_Exit";
            this.Btn_Exit.Size = new System.Drawing.Size(74, 31);
            this.Btn_Exit.TabIndex = 8;
            this.Btn_Exit.Text = "E&XIT";
            this.Btn_Exit.UseVisualStyleBackColor = true;
            this.Btn_Exit.Click += new System.EventHandler(this.Btn_Exit_Click);
            // 
            // TxtSupplier
            // 
            this.TxtSupplier.Location = new System.Drawing.Point(86, 66);
            this.TxtSupplier.Name = "TxtSupplier";
            this.TxtSupplier.Size = new System.Drawing.Size(354, 20);
            this.TxtSupplier.TabIndex = 2;
            this.TxtSupplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 69;
            this.label4.Text = "SUPPLIER";
            // 
            // Arrow4
            // 
            this.Arrow4.Image = global::Accounts.Properties.Resources.Down1;
            this.Arrow4.Location = new System.Drawing.Point(218, 39);
            this.Arrow4.Name = "Arrow4";
            this.Arrow4.Size = new System.Drawing.Size(26, 21);
            this.Arrow4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow4.TabIndex = 66;
            this.Arrow4.TabStop = false;
            // 
            // Frm_ManDays_Cost_Approval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 459);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "Frm_ManDays_Cost_Approval";
            this.Text = "Frm_ManDays_Cost_Approval";
            this.Load += new System.EventHandler(this.Frm_ManDays_Cost_Approval_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_ManDays_Cost_Approval_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_ManDays_Cost_Approval_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LblSpecial;
        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtTotOrder;
        private System.Windows.Forms.DateTimePicker DtpDate1;
        private DotnetVFGrid.MyDataGridView Grid;
        private V_Components.MyTextBox TxtEno;
        private V_Components.MyTextBox TxtRemarks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_Approve;
        private System.Windows.Forms.Button Btn_Exit;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtSupplier;
        private System.Windows.Forms.PictureBox Arrow4;
    }
}