namespace Accounts
{
    partial class FrmMail
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
            this.GBMail = new System.Windows.Forms.GroupBox();
            this.ButClose = new System.Windows.Forms.Button();
            this.ButSend = new System.Windows.Forms.Button();
            this.TxtBody = new V_Components.MyTextBox();
            this.TxtAttachments = new V_Components.MyTextBox();
            this.TxtSubject = new V_Components.MyTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtBccId = new V_Components.MyTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtCCId = new V_Components.MyTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtFrom = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtToId = new V_Components.MyTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.GBMail.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBMail
            // 
            this.GBMail.BackColor = System.Drawing.Color.Silver;
            this.GBMail.Controls.Add(this.ButClose);
            this.GBMail.Controls.Add(this.ButSend);
            this.GBMail.Controls.Add(this.TxtBody);
            this.GBMail.Controls.Add(this.TxtAttachments);
            this.GBMail.Controls.Add(this.TxtSubject);
            this.GBMail.Controls.Add(this.label8);
            this.GBMail.Controls.Add(this.label7);
            this.GBMail.Controls.Add(this.TxtBccId);
            this.GBMail.Controls.Add(this.label6);
            this.GBMail.Controls.Add(this.TxtCCId);
            this.GBMail.Controls.Add(this.label5);
            this.GBMail.Controls.Add(this.TxtFrom);
            this.GBMail.Controls.Add(this.label1);
            this.GBMail.Controls.Add(this.TxtToId);
            this.GBMail.Controls.Add(this.label4);
            this.GBMail.ForeColor = System.Drawing.Color.Black;
            this.GBMail.Location = new System.Drawing.Point(4, 2);
            this.GBMail.Name = "GBMail";
            this.GBMail.Size = new System.Drawing.Size(471, 433);
            this.GBMail.TabIndex = 0;
            this.GBMail.TabStop = false;
            this.GBMail.Text = "Mailing Options ...!";
            this.GBMail.Enter += new System.EventHandler(this.GBMail_Enter);
            // 
            // ButClose
            // 
            this.ButClose.ForeColor = System.Drawing.Color.Black;
            this.ButClose.Location = new System.Drawing.Point(382, 403);
            this.ButClose.Name = "ButClose";
            this.ButClose.Size = new System.Drawing.Size(83, 24);
            this.ButClose.TabIndex = 8;
            this.ButClose.Text = "C&lose";
            this.ButClose.UseVisualStyleBackColor = true;
            this.ButClose.Click += new System.EventHandler(this.ButClose_Click);
            // 
            // ButSend
            // 
            this.ButSend.ForeColor = System.Drawing.Color.Black;
            this.ButSend.Location = new System.Drawing.Point(294, 403);
            this.ButSend.Name = "ButSend";
            this.ButSend.Size = new System.Drawing.Size(83, 24);
            this.ButSend.TabIndex = 7;
            this.ButSend.Text = "&Send";
            this.ButSend.UseVisualStyleBackColor = true;
            this.ButSend.Click += new System.EventHandler(this.ButSend_Click);
            // 
            // TxtBody
            // 
            this.TxtBody.Location = new System.Drawing.Point(9, 193);
            this.TxtBody.Multiline = true;
            this.TxtBody.Name = "TxtBody";
            this.TxtBody.Size = new System.Drawing.Size(456, 204);
            this.TxtBody.TabIndex = 6;
            // 
            // TxtAttachments
            // 
            this.TxtAttachments.Location = new System.Drawing.Point(124, 165);
            this.TxtAttachments.Name = "TxtAttachments";
            this.TxtAttachments.Size = new System.Drawing.Size(341, 22);
            this.TxtAttachments.TabIndex = 5;
            this.TxtAttachments.DoubleClick += new System.EventHandler(this.TxtAttachments_DoubleClick);
            this.TxtAttachments.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtAttachments_KeyPress);
            this.TxtAttachments.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtAttachments_KeyDown);
            // 
            // TxtSubject
            // 
            this.TxtSubject.Location = new System.Drawing.Point(124, 137);
            this.TxtSubject.Name = "TxtSubject";
            this.TxtSubject.Size = new System.Drawing.Size(341, 22);
            this.TxtSubject.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(6, 168);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 16);
            this.label8.TabIndex = 0;
            this.label8.Text = "Attachments :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(6, 137);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 16);
            this.label7.TabIndex = 0;
            this.label7.Text = "Subject :";
            // 
            // TxtBccId
            // 
            this.TxtBccId.Location = new System.Drawing.Point(124, 109);
            this.TxtBccId.Name = "TxtBccId";
            this.TxtBccId.Size = new System.Drawing.Size(341, 22);
            this.TxtBccId.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(6, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 16);
            this.label6.TabIndex = 0;
            this.label6.Text = "BCc :";
            // 
            // TxtCCId
            // 
            this.TxtCCId.Location = new System.Drawing.Point(124, 81);
            this.TxtCCId.Name = "TxtCCId";
            this.TxtCCId.Size = new System.Drawing.Size(341, 22);
            this.TxtCCId.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(6, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "Cc :";
            // 
            // TxtFrom
            // 
            this.TxtFrom.Location = new System.Drawing.Point(124, 25);
            this.TxtFrom.Name = "TxtFrom";
            this.TxtFrom.Size = new System.Drawing.Size(341, 22);
            this.TxtFrom.TabIndex = 0;
            this.TxtFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtFrom_KeyPress);
            this.TxtFrom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtFrom_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "From :";
            // 
            // TxtToId
            // 
            this.TxtToId.Location = new System.Drawing.Point(124, 53);
            this.TxtToId.Name = "TxtToId";
            this.TxtToId.Size = new System.Drawing.Size(341, 22);
            this.TxtToId.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(6, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "To :";
            // 
            // FrmMail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 436);
            this.Controls.Add(this.GBMail);
            this.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMail";
            this.Text = "New Mail";
            this.GBMail.ResumeLayout(false);
            this.GBMail.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMail;
        private System.Windows.Forms.Button ButClose;
        private System.Windows.Forms.Button ButSend;
        private System.Windows.Forms.TextBox TxtBody;
        private System.Windows.Forms.TextBox TxtAttachments;
        private System.Windows.Forms.TextBox TxtSubject;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TxtBccId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TxtCCId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtToId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtFrom;
        private System.Windows.Forms.Label label1;
    }
}