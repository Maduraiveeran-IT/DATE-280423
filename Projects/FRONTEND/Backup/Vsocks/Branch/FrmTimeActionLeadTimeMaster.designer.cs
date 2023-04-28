namespace Accounts
{
    partial class FrmTimeActionLeadTimeMaster
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
            this.TxtLeadTime = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GBMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.TxtLeadTime);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(21, 21);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(247, 76);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // TxtLeadTime
            // 
            this.TxtLeadTime.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtLeadTime.Location = new System.Drawing.Point(131, 30);
            this.TxtLeadTime.Name = "TxtLeadTime";
            this.TxtLeadTime.Size = new System.Drawing.Size(93, 22);
            this.TxtLeadTime.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(48, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "LEAD DAYS";
            // 
            // FrmTimeActionLeadTimeMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 117);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmTimeActionLeadTimeMaster";
            this.Text = "LeadTime Master ...!";
            this.Load += new System.EventHandler(this.FrmTimeActionLeadTimeMaster_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmTimeActionLeadTimeMaster_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmTimeActionLeadTimeMaster_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtLeadTime;
    }
}