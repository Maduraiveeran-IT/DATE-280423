namespace Accounts
{
    partial class FrmStoppageReasonMaster
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtShortName = new V_Components.MyTextBox();
            this.TxtName = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.TxtShortName);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtName);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GBMain.Location = new System.Drawing.Point(8, 2);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(439, 146);
            this.GBMain.TabIndex = 1;
            this.GBMain.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Reason Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Short Name";
            // 
            // TxtShortName
            // 
            this.TxtShortName.AcceptsReturn = true;
            this.TxtShortName.Location = new System.Drawing.Point(131, 102);
            this.TxtShortName.MaxLength = 4;
            this.TxtShortName.Name = "TxtShortName";
            this.TxtShortName.Size = new System.Drawing.Size(289, 21);
            this.TxtShortName.TabIndex = 4;
            this.TxtShortName.Tag = "";
            this.TxtShortName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtShortName_KeyPress);
            // 
            // TxtName
            // 
            this.TxtName.AcceptsReturn = true;
            this.TxtName.Location = new System.Drawing.Point(131, 49);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(289, 21);
            this.TxtName.TabIndex = 0;
            this.TxtName.Tag = "";
            this.TxtName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtName_KeyUp);
            this.TxtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtName_KeyPress);
            // 
            // FrmStoppageReasonMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 157);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmStoppageReasonMaster";
            this.Text = "FrmStoppageReasonMaster";
            this.Load += new System.EventHandler(this.FrmStoppageReasonMaster_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmStoppageReasonMaster_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private V_Components.MyTextBox TxtName;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtShortName;
        private System.Windows.Forms.Label label2;
    }
}