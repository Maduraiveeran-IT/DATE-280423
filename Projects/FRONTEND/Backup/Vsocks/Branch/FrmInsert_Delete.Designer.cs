namespace Accounts
{
    partial class FrmInsert_Delete
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
            this.ButInsert = new System.Windows.Forms.Button();
            this.ButDelete = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButInsert
            // 
            this.ButInsert.Location = new System.Drawing.Point(16, 14);
            this.ButInsert.Name = "ButInsert";
            this.ButInsert.Size = new System.Drawing.Size(89, 30);
            this.ButInsert.TabIndex = 0;
            this.ButInsert.Text = "&Insert";
            this.ButInsert.UseVisualStyleBackColor = true;
            this.ButInsert.Click += new System.EventHandler(this.ButInsert_Click);
            // 
            // ButDelete
            // 
            this.ButDelete.Location = new System.Drawing.Point(111, 14);
            this.ButDelete.Name = "ButDelete";
            this.ButDelete.Size = new System.Drawing.Size(89, 30);
            this.ButDelete.TabIndex = 0;
            this.ButDelete.Text = "&Delete";
            this.ButDelete.UseVisualStyleBackColor = true;
            this.ButDelete.Click += new System.EventHandler(this.ButDelete_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(206, 14);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(89, 30);
            this.button3.TabIndex = 0;
            this.button3.Text = "&Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FrmInsert_Delete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 57);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.ButDelete);
            this.Controls.Add(this.ButInsert);
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmInsert_Delete";
            this.Text = "Insert / Delete ?";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButInsert;
        private System.Windows.Forms.Button ButDelete;
        private System.Windows.Forms.Button button3;
    }
}