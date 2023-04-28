namespace Accounts
{
    partial class FrmProcessMaster
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
            this.TxtRemarks = new V_Components.MyTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtName = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GBMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.TxtName);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GBMain.Location = new System.Drawing.Point(12, 5);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(439, 170);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.AcceptsReturn = true;
            this.TxtRemarks.Location = new System.Drawing.Point(137, 92);
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(264, 21);
            this.TxtRemarks.TabIndex = 1;
            this.TxtRemarks.Tag = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "REMARKS";
            // 
            // TxtName
            // 
            this.TxtName.AcceptsReturn = true;
            this.TxtName.Location = new System.Drawing.Point(137, 52);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(264, 21);
            this.TxtName.TabIndex = 0;
            this.TxtName.Tag = "";
            this.TxtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBankName_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "NAME";
            // 
            // FrmProcessMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 187);
            this.Controls.Add(this.GBMain);
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "FrmProcessMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "PROCESS MASTER";
            this.Deactivate += new System.EventHandler(this.FrmUserMaster_Deactivate);
            this.Load += new System.EventHandler(this.FrmBankMaster_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmBankMaster_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.ResumeLayout(false);

        }

      #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtName;
        private V_Components.MyTextBox TxtRemarks;
    }
}

