namespace Accounts
{
    partial class FrmOrderEnquiry
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
            this.DtpRecStatus = new System.Windows.Forms.DateTimePicker();
            this.ChkStatus = new System.Windows.Forms.CheckBox();
            this.DtpDelDate = new System.Windows.Forms.DateTimePicker();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.Arrow2 = new System.Windows.Forms.PictureBox();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DtpOrderDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.Arrow4 = new System.Windows.Forms.PictureBox();
            this.TxtIOSDays = new V_Components.MyTextBox();
            this.TxtNeedle = new V_Components.MyTextBox();
            this.TxtQty = new V_Components.MyTextBox();
            this.TxtOCNNo = new V_Components.MyTextBox();
            this.TxtOrderNo = new V_Components.MyTextBox();
            this.TxtMerch = new V_Components.MyTextBox();
            this.TxtBuyer = new V_Components.MyTextBox();
            this.TxtEntryNo = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.DtpRecStatus);
            this.GBMain.Controls.Add(this.ChkStatus);
            this.GBMain.Controls.Add(this.DtpDelDate);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.Arrow4);
            this.GBMain.Controls.Add(this.Arrow2);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.TxtIOSDays);
            this.GBMain.Controls.Add(this.TxtNeedle);
            this.GBMain.Controls.Add(this.TxtQty);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.DtpOrderDate);
            this.GBMain.Controls.Add(this.TxtOCNNo);
            this.GBMain.Controls.Add(this.TxtOrderNo);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.TxtMerch);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.TxtBuyer);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.TxtEntryNo);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(10, 0);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(432, 229);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 201);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(161, 13);
            this.label7.TabIndex = 60;
            this.label7.Text = "IOS COMPLETE DAYS / DT.";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // DtpRecStatus
            // 
            this.DtpRecStatus.Enabled = false;
            this.DtpRecStatus.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpRecStatus.Location = new System.Drawing.Point(305, 193);
            this.DtpRecStatus.Name = "DtpRecStatus";
            this.DtpRecStatus.Size = new System.Drawing.Size(111, 21);
            this.DtpRecStatus.TabIndex = 11;
            this.DtpRecStatus.TabStop = false;
            this.DtpRecStatus.ValueChanged += new System.EventHandler(this.DtpRecStatus_ValueChanged);
            // 
            // ChkStatus
            // 
            this.ChkStatus.AutoSize = true;
            this.ChkStatus.Location = new System.Drawing.Point(139, 146);
            this.ChkStatus.Name = "ChkStatus";
            this.ChkStatus.Size = new System.Drawing.Size(43, 17);
            this.ChkStatus.TabIndex = 11;
            this.ChkStatus.Text = "NO";
            this.ChkStatus.UseVisualStyleBackColor = true;
            this.ChkStatus.Visible = false;
            this.ChkStatus.CheckedChanged += new System.EventHandler(this.ChkStatus_CheckedChanged);
            // 
            // DtpDelDate
            // 
            this.DtpDelDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDelDate.Location = new System.Drawing.Point(305, 112);
            this.DtpDelDate.Name = "DtpDelDate";
            this.DtpDelDate.Size = new System.Drawing.Size(111, 21);
            this.DtpDelDate.TabIndex = 6;
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Branch.Properties.Resources.Down;
            this.Arrow1.Location = new System.Drawing.Point(393, 166);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(25, 21);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 57;
            this.Arrow1.TabStop = false;
            // 
            // Arrow2
            // 
            this.Arrow2.Image = global::Branch.Properties.Resources.Down;
            this.Arrow2.Location = new System.Drawing.Point(393, 84);
            this.Arrow2.Name = "Arrow2";
            this.Arrow2.Size = new System.Drawing.Size(25, 21);
            this.Arrow2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow2.TabIndex = 57;
            this.Arrow2.TabStop = false;
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down;
            this.Arrow3.Location = new System.Drawing.Point(393, 58);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(25, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 56;
            this.Arrow3.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "QTY / NEEDLE";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // DtpOrderDate
            // 
            this.DtpOrderDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpOrderDate.Location = new System.Drawing.Point(188, 112);
            this.DtpOrderDate.Name = "DtpOrderDate";
            this.DtpOrderDate.Size = new System.Drawing.Size(111, 21);
            this.DtpOrderDate.TabIndex = 5;
            this.DtpOrderDate.ValueChanged += new System.EventHandler(this.DtpOrderDate_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 120);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(140, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "ORDER / DELIVERY DT.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "REF NO / OCN NO";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "MERCHANDISER";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "BUYER";
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(305, 31);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(111, 21);
            this.DtpDate.TabIndex = 1;
            this.DtpDate.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ENTRY NO / DATE";
            // 
            // Arrow4
            // 
            this.Arrow4.Image = global::Branch.Properties.Resources.Down;
            this.Arrow4.Location = new System.Drawing.Point(393, 139);
            this.Arrow4.Name = "Arrow4";
            this.Arrow4.Size = new System.Drawing.Size(25, 21);
            this.Arrow4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow4.TabIndex = 57;
            this.Arrow4.TabStop = false;
            // 
            // TxtIOSDays
            // 
            this.TxtIOSDays.Location = new System.Drawing.Point(188, 193);
            this.TxtIOSDays.Name = "TxtIOSDays";
            this.TxtIOSDays.Size = new System.Drawing.Size(111, 21);
            this.TxtIOSDays.TabIndex = 10;
            this.TxtIOSDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtIOSDays.TextChanged += new System.EventHandler(this.TxtIOSDays_TextChanged);
            this.TxtIOSDays.Leave += new System.EventHandler(this.TxtIOSDays_Leave);
            // 
            // TxtNeedle
            // 
            this.TxtNeedle.Location = new System.Drawing.Point(305, 139);
            this.TxtNeedle.Name = "TxtNeedle";
            this.TxtNeedle.Size = new System.Drawing.Size(89, 21);
            this.TxtNeedle.TabIndex = 8;
            this.TxtNeedle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtNeedle.TextChanged += new System.EventHandler(this.TxtQty_TextChanged);
            // 
            // TxtQty
            // 
            this.TxtQty.Location = new System.Drawing.Point(188, 139);
            this.TxtQty.Name = "TxtQty";
            this.TxtQty.Size = new System.Drawing.Size(111, 21);
            this.TxtQty.TabIndex = 7;
            this.TxtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtQty.TextChanged += new System.EventHandler(this.TxtQty_TextChanged);
            // 
            // TxtOCNNo
            // 
            this.TxtOCNNo.Location = new System.Drawing.Point(305, 84);
            this.TxtOCNNo.Name = "TxtOCNNo";
            this.TxtOCNNo.Size = new System.Drawing.Size(89, 21);
            this.TxtOCNNo.TabIndex = 4;
            this.TxtOCNNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtOrderNo
            // 
            this.TxtOrderNo.Location = new System.Drawing.Point(188, 85);
            this.TxtOrderNo.Name = "TxtOrderNo";
            this.TxtOrderNo.Size = new System.Drawing.Size(111, 21);
            this.TxtOrderNo.TabIndex = 3;
            this.TxtOrderNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtMerch
            // 
            this.TxtMerch.Location = new System.Drawing.Point(188, 166);
            this.TxtMerch.Name = "TxtMerch";
            this.TxtMerch.Size = new System.Drawing.Size(206, 21);
            this.TxtMerch.TabIndex = 9;
            this.TxtMerch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtMerch.TextChanged += new System.EventHandler(this.TxtMerch_TextChanged);
            // 
            // TxtBuyer
            // 
            this.TxtBuyer.Location = new System.Drawing.Point(188, 58);
            this.TxtBuyer.Name = "TxtBuyer";
            this.TxtBuyer.Size = new System.Drawing.Size(206, 21);
            this.TxtBuyer.TabIndex = 2;
            this.TxtBuyer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtEntryNo
            // 
            this.TxtEntryNo.Location = new System.Drawing.Point(188, 31);
            this.TxtEntryNo.Name = "TxtEntryNo";
            this.TxtEntryNo.Size = new System.Drawing.Size(111, 21);
            this.TxtEntryNo.TabIndex = 0;
            this.TxtEntryNo.TabStop = false;
            this.TxtEntryNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmOrderEnquiry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 240);
            this.Controls.Add(this.GBMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "FrmOrderEnquiry";
            this.Text = "ORDER ENQUIRY ...!";
            this.Load += new System.EventHandler(this.FrmOrderEnquiry_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmOrderEnquiry_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmOrderEnquiry_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private V_Components.MyTextBox TxtEntryNo;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtBuyer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private V_Components.MyTextBox TxtQty;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker DtpOrderDate;
        private V_Components.MyTextBox TxtOrderNo;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtMerch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox Arrow2;
        private System.Windows.Forms.PictureBox Arrow3;
        private System.Windows.Forms.DateTimePicker DtpDelDate;
        private System.Windows.Forms.DateTimePicker DtpRecStatus;
        private System.Windows.Forms.CheckBox ChkStatus;
        private System.Windows.Forms.Label label7;
        private V_Components.MyTextBox TxtIOSDays;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox Arrow1;
        private V_Components.MyTextBox TxtOCNNo;
        private System.Windows.Forms.PictureBox Arrow4;
        private V_Components.MyTextBox TxtNeedle;
    }
}