namespace Accounts
{
    partial class Frm_Resign_Approval
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
            this.Remarks = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnRej = new System.Windows.Forms.Button();
            this.BtnApp = new System.Windows.Forms.Button();
            this.Letterphoto = new System.Windows.Forms.PictureBox();
            this.EmplPhoto = new System.Windows.Forms.PictureBox();
            this.label15 = new System.Windows.Forms.Label();
            this.DtpEDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.Arrow4 = new System.Windows.Forms.PictureBox();
            this.TxtHR_Remarks = new V_Components.MyTextBox();
            this.Txt_Floor_Hr_Rmk = new V_Components.MyTextBox();
            this.Txt_Tno = new V_Components.MyTextBox();
            this.Txt_AgName = new V_Components.MyTextBox();
            this.Txt_Reason = new V_Components.MyTextBox();
            this.Txt_AgDesignation = new V_Components.MyTextBox();
            this.Txt_Designation = new V_Components.MyTextBox();
            this.Txt_Dept = new V_Components.MyTextBox();
            this.Txt_Name = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Letterphoto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmplPhoto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.Arrow4);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.Remarks);
            this.GBMain.Controls.Add(this.TxtHR_Remarks);
            this.GBMain.Controls.Add(this.button1);
            this.GBMain.Controls.Add(this.BtnRej);
            this.GBMain.Controls.Add(this.BtnApp);
            this.GBMain.Controls.Add(this.Letterphoto);
            this.GBMain.Controls.Add(this.EmplPhoto);
            this.GBMain.Controls.Add(this.Txt_Floor_Hr_Rmk);
            this.GBMain.Controls.Add(this.label15);
            this.GBMain.Controls.Add(this.Txt_Tno);
            this.GBMain.Controls.Add(this.Txt_AgName);
            this.GBMain.Controls.Add(this.DtpEDate);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.Txt_Reason);
            this.GBMain.Controls.Add(this.Txt_AgDesignation);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.Txt_Designation);
            this.GBMain.Controls.Add(this.Txt_Dept);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.Txt_Name);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(12, 12);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(793, 384);
            this.GBMain.TabIndex = 3;
            this.GBMain.TabStop = false;
            // 
            // Remarks
            // 
            this.Remarks.AutoSize = true;
            this.Remarks.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Remarks.Location = new System.Drawing.Point(12, 241);
            this.Remarks.Name = "Remarks";
            this.Remarks.Size = new System.Drawing.Size(74, 15);
            this.Remarks.TabIndex = 188;
            this.Remarks.Text = "HR Remarks";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(454, 331);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 27);
            this.button1.TabIndex = 183;
            this.button1.Text = "EXIT";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnRej
            // 
            this.BtnRej.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnRej.Location = new System.Drawing.Point(357, 331);
            this.BtnRej.Name = "BtnRej";
            this.BtnRej.Size = new System.Drawing.Size(91, 27);
            this.BtnRej.TabIndex = 182;
            this.BtnRej.Text = "REJECT";
            this.BtnRej.UseVisualStyleBackColor = true;
            this.BtnRej.Click += new System.EventHandler(this.BtnRej_Click);
            // 
            // BtnApp
            // 
            this.BtnApp.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnApp.Location = new System.Drawing.Point(263, 331);
            this.BtnApp.Name = "BtnApp";
            this.BtnApp.Size = new System.Drawing.Size(88, 27);
            this.BtnApp.TabIndex = 181;
            this.BtnApp.Text = "APPROVE";
            this.BtnApp.UseVisualStyleBackColor = true;
            this.BtnApp.Click += new System.EventHandler(this.BtnApp_Click);
            // 
            // Letterphoto
            // 
            this.Letterphoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Letterphoto.Location = new System.Drawing.Point(583, 23);
            this.Letterphoto.Name = "Letterphoto";
            this.Letterphoto.Size = new System.Drawing.Size(190, 270);
            this.Letterphoto.TabIndex = 180;
            this.Letterphoto.TabStop = false;
            this.Letterphoto.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Letterphoto_MouseDoubleClick);
            // 
            // EmplPhoto
            // 
            this.EmplPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EmplPhoto.Location = new System.Drawing.Point(485, 43);
            this.EmplPhoto.Name = "EmplPhoto";
            this.EmplPhoto.Size = new System.Drawing.Size(86, 94);
            this.EmplPhoto.TabIndex = 170;
            this.EmplPhoto.TabStop = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(12, 160);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(116, 15);
            this.label15.TabIndex = 145;
            this.label15.Text = "Floor HRM Remarks";
            // 
            // DtpEDate
            // 
            this.DtpEDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpEDate.Location = new System.Drawing.Point(133, 17);
            this.DtpEDate.Name = "DtpEDate";
            this.DtpEDate.Size = new System.Drawing.Size(89, 20);
            this.DtpEDate.TabIndex = 1;
            this.DtpEDate.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 15);
            this.label3.TabIndex = 95;
            this.label3.Text = "Reason";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 15);
            this.label8.TabIndex = 84;
            this.label8.Text = "Follow By";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 15);
            this.label6.TabIndex = 76;
            this.label6.Text = "Department/Design";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Tno / Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ety Date";
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Accounts.Properties.Resources.Down1;
            this.Arrow3.Location = new System.Drawing.Point(454, 45);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(22, 20);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 189;
            this.Arrow3.TabStop = false;
            // 
            // Arrow4
            // 
            this.Arrow4.Image = global::Accounts.Properties.Resources.Down1;
            this.Arrow4.Location = new System.Drawing.Point(291, 126);
            this.Arrow4.Name = "Arrow4";
            this.Arrow4.Size = new System.Drawing.Size(22, 20);
            this.Arrow4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow4.TabIndex = 190;
            this.Arrow4.TabStop = false;
            // 
            // TxtHR_Remarks
            // 
            this.TxtHR_Remarks.Location = new System.Drawing.Point(133, 239);
            this.TxtHR_Remarks.Multiline = true;
            this.TxtHR_Remarks.Name = "TxtHR_Remarks";
            this.TxtHR_Remarks.Size = new System.Drawing.Size(438, 71);
            this.TxtHR_Remarks.TabIndex = 187;
            this.TxtHR_Remarks.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Resign_Approval_KeyPress);
            // 
            // Txt_Floor_Hr_Rmk
            // 
            this.Txt_Floor_Hr_Rmk.Location = new System.Drawing.Point(133, 155);
            this.Txt_Floor_Hr_Rmk.Multiline = true;
            this.Txt_Floor_Hr_Rmk.Name = "Txt_Floor_Hr_Rmk";
            this.Txt_Floor_Hr_Rmk.Size = new System.Drawing.Size(438, 65);
            this.Txt_Floor_Hr_Rmk.TabIndex = 167;
            this.Txt_Floor_Hr_Rmk.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Resign_Approval_KeyDown);
            this.Txt_Floor_Hr_Rmk.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Resign_Approval_KeyPress);
            // 
            // Txt_Tno
            // 
            this.Txt_Tno.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Txt_Tno.Location = new System.Drawing.Point(133, 43);
            this.Txt_Tno.Name = "Txt_Tno";
            this.Txt_Tno.Size = new System.Drawing.Size(151, 20);
            this.Txt_Tno.TabIndex = 3;
            this.Txt_Tno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Txt_Tno.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Resign_Approval_KeyDown);
            this.Txt_Tno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Resign_Approval_KeyPress);
            // 
            // Txt_AgName
            // 
            this.Txt_AgName.Location = new System.Drawing.Point(133, 96);
            this.Txt_AgName.Name = "Txt_AgName";
            this.Txt_AgName.Size = new System.Drawing.Size(151, 20);
            this.Txt_AgName.TabIndex = 102;
            this.Txt_AgName.TabStop = false;
            this.Txt_AgName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Txt_AgName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Resign_Approval_KeyDown);
            this.Txt_AgName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Resign_Approval_KeyPress);
            // 
            // Txt_Reason
            // 
            this.Txt_Reason.Location = new System.Drawing.Point(133, 125);
            this.Txt_Reason.Name = "Txt_Reason";
            this.Txt_Reason.Size = new System.Drawing.Size(151, 20);
            this.Txt_Reason.TabIndex = 3;
            this.Txt_Reason.TabStop = false;
            this.Txt_Reason.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Txt_Reason.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Resign_Approval_KeyDown);
            this.Txt_Reason.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Resign_Approval_KeyPress);
            // 
            // Txt_AgDesignation
            // 
            this.Txt_AgDesignation.Location = new System.Drawing.Point(291, 97);
            this.Txt_AgDesignation.Name = "Txt_AgDesignation";
            this.Txt_AgDesignation.Size = new System.Drawing.Size(185, 20);
            this.Txt_AgDesignation.TabIndex = 87;
            this.Txt_AgDesignation.TabStop = false;
            this.Txt_AgDesignation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Txt_AgDesignation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Resign_Approval_KeyDown);
            this.Txt_AgDesignation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Resign_Approval_KeyPress);
            // 
            // Txt_Designation
            // 
            this.Txt_Designation.Location = new System.Drawing.Point(291, 70);
            this.Txt_Designation.Name = "Txt_Designation";
            this.Txt_Designation.Size = new System.Drawing.Size(185, 20);
            this.Txt_Designation.TabIndex = 4;
            this.Txt_Designation.TabStop = false;
            this.Txt_Designation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Txt_Designation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Resign_Approval_KeyDown);
            this.Txt_Designation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Resign_Approval_KeyPress);
            // 
            // Txt_Dept
            // 
            this.Txt_Dept.Location = new System.Drawing.Point(133, 70);
            this.Txt_Dept.Name = "Txt_Dept";
            this.Txt_Dept.Size = new System.Drawing.Size(151, 20);
            this.Txt_Dept.TabIndex = 3;
            this.Txt_Dept.TabStop = false;
            this.Txt_Dept.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Txt_Dept.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Resign_Approval_KeyDown);
            this.Txt_Dept.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Resign_Approval_KeyPress);
            // 
            // Txt_Name
            // 
            this.Txt_Name.Location = new System.Drawing.Point(291, 43);
            this.Txt_Name.Name = "Txt_Name";
            this.Txt_Name.Size = new System.Drawing.Size(157, 20);
            this.Txt_Name.TabIndex = 2;
            this.Txt_Name.TabStop = false;
            this.Txt_Name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Txt_Name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Resign_Approval_KeyDown);
            this.Txt_Name.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Resign_Approval_KeyPress);
            // 
            // Frm_Resign_Approval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 406);
            this.Controls.Add(this.GBMain);
            this.Name = "Frm_Resign_Approval";
            this.Text = "Frm_Resign_Approval";
            this.Load += new System.EventHandler(this.Frm_Resign_Approval_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Resign_Approval_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Resign_Approval_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Letterphoto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmplPhoto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.PictureBox Letterphoto;
        private System.Windows.Forms.PictureBox EmplPhoto;
        private V_Components.MyTextBox Txt_Floor_Hr_Rmk;
        private System.Windows.Forms.Label label15;
        private V_Components.MyTextBox Txt_Tno;
        private V_Components.MyTextBox Txt_AgName;
        private System.Windows.Forms.DateTimePicker DtpEDate;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox Txt_Reason;
        private V_Components.MyTextBox Txt_AgDesignation;
        private System.Windows.Forms.Label label8;
        private V_Components.MyTextBox Txt_Designation;
        private V_Components.MyTextBox Txt_Dept;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private V_Components.MyTextBox Txt_Name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BtnRej;
        private System.Windows.Forms.Button BtnApp;
        private V_Components.MyTextBox TxtHR_Remarks;
        private System.Windows.Forms.Label Remarks;
        private System.Windows.Forms.PictureBox Arrow4;
        private System.Windows.Forms.PictureBox Arrow3;
    }
}