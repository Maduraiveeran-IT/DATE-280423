namespace Accounts
{
    partial class FrmRgpCancel
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
            this.label7 = new System.Windows.Forms.Label();
            this.Txt_Employee = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.ButExit = new System.Windows.Forms.Button();
            this.ButClear = new System.Windows.Forms.Button();
            this.DtpRDate = new System.Windows.Forms.DateTimePicker();
            this.ButCancel = new System.Windows.Forms.Button();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.TxtRemarks = new V_Components.MyTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtDesp = new V_Components.MyTextBox();
            this.TxtRgpRemarks = new V_Components.MyTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtParty = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtRgpNo = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.Txt_Employee);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.ButExit);
            this.GBMain.Controls.Add(this.ButClear);
            this.GBMain.Controls.Add(this.DtpRDate);
            this.GBMain.Controls.Add(this.ButCancel);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtDesp);
            this.GBMain.Controls.Add(this.TxtRgpRemarks);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.TxtParty);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtRgpNo);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Location = new System.Drawing.Point(5, 3);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(647, 454);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 429);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 57;
            this.label7.Text = "ENTRY BY";
            // 
            // Txt_Employee
            // 
            this.Txt_Employee.Location = new System.Drawing.Point(82, 426);
            this.Txt_Employee.Name = "Txt_Employee";
            this.Txt_Employee.Size = new System.Drawing.Size(184, 20);
            this.Txt_Employee.TabIndex = 56;
            this.Txt_Employee.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(22, 83);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(604, 165);
            this.Grid.TabIndex = 4;
            this.Grid.TabStop = false;
            // 
            // ButExit
            // 
            this.ButExit.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButExit.Location = new System.Drawing.Point(522, 422);
            this.ButExit.Name = "ButExit";
            this.ButExit.Size = new System.Drawing.Size(104, 26);
            this.ButExit.TabIndex = 9;
            this.ButExit.Text = "EXIT";
            this.ButExit.UseVisualStyleBackColor = true;
            this.ButExit.Click += new System.EventHandler(this.ButExit_Click);
            // 
            // ButClear
            // 
            this.ButClear.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButClear.Location = new System.Drawing.Point(401, 422);
            this.ButClear.Name = "ButClear";
            this.ButClear.Size = new System.Drawing.Size(104, 26);
            this.ButClear.TabIndex = 8;
            this.ButClear.Text = "CLEAR";
            this.ButClear.UseVisualStyleBackColor = true;
            this.ButClear.Click += new System.EventHandler(this.ButClear_Click);
            // 
            // DtpRDate
            // 
            this.DtpRDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpRDate.Location = new System.Drawing.Point(423, 26);
            this.DtpRDate.Name = "DtpRDate";
            this.DtpRDate.Size = new System.Drawing.Size(81, 20);
            this.DtpRDate.TabIndex = 1;
            this.DtpRDate.TabStop = false;
            // 
            // ButCancel
            // 
            this.ButCancel.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButCancel.Location = new System.Drawing.Point(277, 422);
            this.ButCancel.Name = "ButCancel";
            this.ButCancel.Size = new System.Drawing.Size(104, 26);
            this.ButCancel.TabIndex = 7;
            this.ButCancel.Text = "CANCEL";
            this.ButCancel.UseVisualStyleBackColor = true;
            this.ButCancel.Click += new System.EventHandler(this.ButCancel_Click);
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Accounts.Properties.Resources.Down;
            this.Arrow3.Location = new System.Drawing.Point(219, 23);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(25, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 55;
            this.Arrow3.TabStop = false;
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(22, 345);
            this.TxtRemarks.Multiline = true;
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(604, 58);
            this.TxtRemarks.TabIndex = 6;
            this.TxtRemarks.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(21, 327);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 15);
            this.label5.TabIndex = 15;
            this.label5.Text = "REMARKS";
            // 
            // TxtDesp
            // 
            this.TxtDesp.Location = new System.Drawing.Point(421, 57);
            this.TxtDesp.Multiline = true;
            this.TxtDesp.Name = "TxtDesp";
            this.TxtDesp.Size = new System.Drawing.Size(205, 20);
            this.TxtDesp.TabIndex = 3;
            this.TxtDesp.TabStop = false;
            // 
            // TxtRgpRemarks
            // 
            this.TxtRgpRemarks.Location = new System.Drawing.Point(22, 269);
            this.TxtRgpRemarks.Multiline = true;
            this.TxtRgpRemarks.Name = "TxtRgpRemarks";
            this.TxtRgpRemarks.Size = new System.Drawing.Size(604, 46);
            this.TxtRgpRemarks.TabIndex = 5;
            this.TxtRgpRemarks.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(340, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "DESP THRH";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(21, 251);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "RGP REMARKS";
            // 
            // TxtParty
            // 
            this.TxtParty.Location = new System.Drawing.Point(82, 54);
            this.TxtParty.Name = "TxtParty";
            this.TxtParty.Size = new System.Drawing.Size(252, 20);
            this.TxtParty.TabIndex = 2;
            this.TxtParty.TabStop = false;
            this.TxtParty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "PARTY";
            // 
            // TxtRgpNo
            // 
            this.TxtRgpNo.Location = new System.Drawing.Point(82, 23);
            this.TxtRgpNo.Name = "TxtRgpNo";
            this.TxtRgpNo.Size = new System.Drawing.Size(131, 20);
            this.TxtRgpNo.TabIndex = 0;
            this.TxtRgpNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(340, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "RGP DATE";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(21, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "RGP NO";
            // 
            // FrmRgpCancel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 462);
            this.ControlBox = false;
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmRgpCancel";
            this.Text = "RGP Cancel";
            this.Load += new System.EventHandler(this.FrmRgpCancel_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmRgpCancel_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmRgpCancel_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox TxtRgpNo;
        private System.Windows.Forms.PictureBox Arrow3;
        private System.Windows.Forms.Button ButCancel;
        private System.Windows.Forms.Button ButExit;
        private System.Windows.Forms.Button ButClear;
        private V_Components.MyTextBox TxtParty;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DtpRDate;
        private DotnetVFGrid.MyDataGridView Grid;
        private V_Components.MyTextBox TxtRgpRemarks;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtRemarks;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox TxtDesp;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private V_Components.MyTextBox Txt_Employee;
    }
}