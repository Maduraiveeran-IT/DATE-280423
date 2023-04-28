namespace Accounts
{
    partial class FrmPrintPreview
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.GBExport = new System.Windows.Forms.GroupBox();
            this.ButCancel = new System.Windows.Forms.Button();
            this.ButExport1 = new System.Windows.Forms.Button();
            this.OptExcel = new System.Windows.Forms.RadioButton();
            this.OptMail = new System.Windows.Forms.RadioButton();
            this.OptPDf = new System.Windows.Forms.RadioButton();
            this.OptWord = new System.Windows.Forms.RadioButton();
            this.txtCopies = new V_Components.MyTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtTo = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtFrom = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ButExit = new System.Windows.Forms.Button();
            this.ButExport = new System.Windows.Forms.Button();
            this.ButPrint = new System.Windows.Forms.Button();
            this.GBMain.SuspendLayout();
            this.GBExport.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.GBMain.Controls.Add(this.richTextBox1);
            this.GBMain.Controls.Add(this.GBExport);
            this.GBMain.Controls.Add(this.txtCopies);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.TxtTo);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtFrom);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.ButExit);
            this.GBMain.Controls.Add(this.ButExport);
            this.GBMain.Controls.Add(this.ButPrint);
            this.GBMain.Location = new System.Drawing.Point(14, 14);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(837, 508);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(6, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(825, 455);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            this.richTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyDown);
            this.richTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.richTextBox1_KeyPress);
            // 
            // GBExport
            // 
            this.GBExport.Controls.Add(this.ButCancel);
            this.GBExport.Controls.Add(this.ButExport1);
            this.GBExport.Controls.Add(this.OptExcel);
            this.GBExport.Controls.Add(this.OptMail);
            this.GBExport.Controls.Add(this.OptPDf);
            this.GBExport.Controls.Add(this.OptWord);
            this.GBExport.Location = new System.Drawing.Point(285, 153);
            this.GBExport.Name = "GBExport";
            this.GBExport.Size = new System.Drawing.Size(312, 101);
            this.GBExport.TabIndex = 6;
            this.GBExport.TabStop = false;
            this.GBExport.Text = "Export Options ...!";
            this.GBExport.Visible = false;
            // 
            // ButCancel
            // 
            this.ButCancel.Location = new System.Drawing.Point(217, 57);
            this.ButCancel.Name = "ButCancel";
            this.ButCancel.Size = new System.Drawing.Size(74, 26);
            this.ButCancel.TabIndex = 5;
            this.ButCancel.Text = "Canc&el";
            this.ButCancel.UseVisualStyleBackColor = true;
            this.ButCancel.Click += new System.EventHandler(this.ButCancel_Click);
            // 
            // ButExport1
            // 
            this.ButExport1.Location = new System.Drawing.Point(217, 25);
            this.ButExport1.Name = "ButExport1";
            this.ButExport1.Size = new System.Drawing.Size(74, 26);
            this.ButExport1.TabIndex = 4;
            this.ButExport1.Text = "&Ok";
            this.ButExport1.UseVisualStyleBackColor = true;
            this.ButExport1.Click += new System.EventHandler(this.ButExport1_Click);
            // 
            // OptExcel
            // 
            this.OptExcel.AutoSize = true;
            this.OptExcel.Location = new System.Drawing.Point(24, 54);
            this.OptExcel.Name = "OptExcel";
            this.OptExcel.Size = new System.Drawing.Size(66, 20);
            this.OptExcel.TabIndex = 1;
            this.OptExcel.TabStop = true;
            this.OptExcel.Text = "Ex&cel";
            this.OptExcel.UseVisualStyleBackColor = true;
            // 
            // OptMail
            // 
            this.OptMail.AutoSize = true;
            this.OptMail.Location = new System.Drawing.Point(127, 54);
            this.OptMail.Name = "OptMail";
            this.OptMail.Size = new System.Drawing.Size(58, 20);
            this.OptMail.TabIndex = 3;
            this.OptMail.TabStop = true;
            this.OptMail.Text = "&Mail";
            this.OptMail.UseVisualStyleBackColor = true;
            this.OptMail.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // OptPDf
            // 
            this.OptPDf.AutoSize = true;
            this.OptPDf.Location = new System.Drawing.Point(127, 28);
            this.OptPDf.Name = "OptPDf";
            this.OptPDf.Size = new System.Drawing.Size(50, 20);
            this.OptPDf.TabIndex = 2;
            this.OptPDf.TabStop = true;
            this.OptPDf.Text = "&Pdf";
            this.OptPDf.UseVisualStyleBackColor = true;
            this.OptPDf.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // OptWord
            // 
            this.OptWord.AutoSize = true;
            this.OptWord.Location = new System.Drawing.Point(24, 28);
            this.OptWord.Name = "OptWord";
            this.OptWord.Size = new System.Drawing.Size(58, 20);
            this.OptWord.TabIndex = 0;
            this.OptWord.TabStop = true;
            this.OptWord.Text = "&Word";
            this.OptWord.UseVisualStyleBackColor = true;
            // 
            // txtCopies
            // 
            this.txtCopies.Location = new System.Drawing.Point(392, 476);
            this.txtCopies.Name = "txtCopies";
            this.txtCopies.Size = new System.Drawing.Size(84, 22);
            this.txtCopies.TabIndex = 3;
            this.txtCopies.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCopies.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(314, 479);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Copies :";
            // 
            // TxtTo
            // 
            this.TxtTo.Location = new System.Drawing.Point(217, 476);
            this.TxtTo.Name = "TxtTo";
            this.TxtTo.Size = new System.Drawing.Size(84, 22);
            this.TxtTo.TabIndex = 2;
            this.TxtTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(162, 478);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "To :";
            // 
            // TxtFrom
            // 
            this.TxtFrom.Location = new System.Drawing.Point(68, 476);
            this.TxtFrom.Name = "TxtFrom";
            this.TxtFrom.Size = new System.Drawing.Size(84, 22);
            this.TxtFrom.TabIndex = 1;
            this.TxtFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtFrom_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 478);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "From :";
            // 
            // ButExit
            // 
            this.ButExit.Location = new System.Drawing.Point(755, 476);
            this.ButExit.Name = "ButExit";
            this.ButExit.Size = new System.Drawing.Size(76, 26);
            this.ButExit.TabIndex = 5;
            this.ButExit.Text = "E&xit";
            this.ButExit.UseVisualStyleBackColor = true;
            this.ButExit.Click += new System.EventHandler(this.ButExit_Click);
            // 
            // ButExport
            // 
            this.ButExport.Location = new System.Drawing.Point(674, 416);
            this.ButExport.Name = "ButExport";
            this.ButExport.Size = new System.Drawing.Size(76, 26);
            this.ButExport.TabIndex = 4;
            this.ButExport.Text = "&Export";
            this.ButExport.UseVisualStyleBackColor = true;
            this.ButExport.Visible = false;
            this.ButExport.Click += new System.EventHandler(this.ButExport_Click);
            // 
            // ButPrint
            // 
            this.ButPrint.Location = new System.Drawing.Point(674, 476);
            this.ButPrint.Name = "ButPrint";
            this.ButPrint.Size = new System.Drawing.Size(76, 26);
            this.ButPrint.TabIndex = 4;
            this.ButPrint.Text = "&Print";
            this.ButPrint.UseVisualStyleBackColor = true;
            this.ButPrint.Click += new System.EventHandler(this.ButPrint_Click);
            // 
            // FrmPrintPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(863, 534);
            this.Controls.Add(this.GBMain);
            this.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPrintPreview";
            this.Text = "Print Preview ...!";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmPrintPreview_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmPrintPreview_KeyDown);
            this.Load += new System.EventHandler(this.FrmPrintPreview_Load);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.GBExport.ResumeLayout(false);
            this.GBExport.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Button ButExit;
        private System.Windows.Forms.Button ButPrint;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox TxtTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCopies;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ButExport;
        private System.Windows.Forms.GroupBox GBExport;
        private System.Windows.Forms.RadioButton OptExcel;
        private System.Windows.Forms.RadioButton OptWord;
        private System.Windows.Forms.RadioButton OptPDf;
        private System.Windows.Forms.Button ButCancel;
        private System.Windows.Forms.Button ButExport1;
        private System.Windows.Forms.RadioButton OptMail;
    }
}