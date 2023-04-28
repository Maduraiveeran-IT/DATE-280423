namespace Accounts
{
    partial class Frm_Image_Viewer
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
            this.PNL_Main = new System.Windows.Forms.Panel();
            this.PIC_Image = new System.Windows.Forms.PictureBox();
            this.Btn_Ok = new System.Windows.Forms.Button();
            this.PNL_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PIC_Image)).BeginInit();
            this.SuspendLayout();
            // 
            // PNL_Main
            // 
            this.PNL_Main.Controls.Add(this.PIC_Image);
            this.PNL_Main.Location = new System.Drawing.Point(10, 10);
            this.PNL_Main.Name = "PNL_Main";
            this.PNL_Main.Size = new System.Drawing.Size(453, 357);
            this.PNL_Main.TabIndex = 0;
            // 
            // PIC_Image
            // 
            this.PIC_Image.Location = new System.Drawing.Point(5, 8);
            this.PIC_Image.Name = "PIC_Image";
            this.PIC_Image.Size = new System.Drawing.Size(440, 339);
            this.PIC_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PIC_Image.TabIndex = 0;
            this.PIC_Image.TabStop = false;
            // 
            // Btn_Ok
            // 
            this.Btn_Ok.Location = new System.Drawing.Point(377, 373);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new System.Drawing.Size(85, 31);
            this.Btn_Ok.TabIndex = 1;
            this.Btn_Ok.Text = "&Ok";
            this.Btn_Ok.UseVisualStyleBackColor = true;
            this.Btn_Ok.Click += new System.EventHandler(this.Btn_Ok_Click);
            // 
            // Frm_Image_Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 409);
            this.Controls.Add(this.Btn_Ok);
            this.Controls.Add(this.PNL_Main);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_Image_Viewer";
            this.Text = "Image Viewer";
            this.Load += new System.EventHandler(this.Frm_Image_Viewer_Load);
            this.PNL_Main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PIC_Image)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PNL_Main;
        private System.Windows.Forms.Button Btn_Ok;
        private System.Windows.Forms.PictureBox PIC_Image;
    }
}