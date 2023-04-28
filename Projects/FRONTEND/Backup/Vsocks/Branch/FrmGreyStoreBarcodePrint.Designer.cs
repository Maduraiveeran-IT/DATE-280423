namespace Accounts
{
    partial class FrmGreyStoreBarcodePrint
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
            this.BtnClear = new System.Windows.Forms.Button();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.LblSpecial = new System.Windows.Forms.Label();
            this.TxtCount = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.BtnClear);
            this.GBMain.Controls.Add(this.BtnPrint);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.TxtCount);
            this.GBMain.Controls.Add(this.LblSpecial);
            this.GBMain.Location = new System.Drawing.Point(12, 3);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(278, 140);
            this.GBMain.TabIndex = 2;
            this.GBMain.TabStop = false;
            this.GBMain.Enter += new System.EventHandler(this.GBMain_Enter);
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(142, 104);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(123, 23);
            this.BtnClear.TabIndex = 43;
            this.BtnClear.Text = "CLEAR";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtnPrint
            // 
            this.BtnPrint.Location = new System.Drawing.Point(11, 104);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(123, 23);
            this.BtnPrint.TabIndex = 5;
            this.BtnPrint.Text = "PRINT";
            this.BtnPrint.UseVisualStyleBackColor = true;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "NO OF BARCODE\'S";
            // 
            // LblSpecial
            // 
            this.LblSpecial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.LblSpecial.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSpecial.Location = new System.Drawing.Point(6, 16);
            this.LblSpecial.Name = "LblSpecial";
            this.LblSpecial.Size = new System.Drawing.Size(259, 26);
            this.LblSpecial.TabIndex = 2;
            this.LblSpecial.Text = "BARCODE PRINT FOR GREY STORE";
            this.LblSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtCount
            // 
            this.TxtCount.Location = new System.Drawing.Point(118, 65);
            this.TxtCount.Name = "TxtCount";
            this.TxtCount.Size = new System.Drawing.Size(147, 20);
            this.TxtCount.TabIndex = 4;
            this.TxtCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmGreyStoreBarcodePrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 151);
            this.Controls.Add(this.GBMain);
            this.Name = "FrmGreyStoreBarcodePrint";
            this.Text = "FrmGreyStoreBarcodePrint";
            this.Load += new System.EventHandler(this.FrmGreyStoreBarcodePrint_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmGreyStoreBarcodePrint_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmGreyStoreBarcodePrint_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Label label8;
        private V_Components.MyTextBox TxtCount;
        private System.Windows.Forms.Label LblSpecial;
        private System.Windows.Forms.Button BtnClear;
    }
}