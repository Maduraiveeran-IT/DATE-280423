namespace Accounts
{
    partial class Frm_Ledger_Selection
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.OptP = new System.Windows.Forms.RadioButton();
            this.OptE = new System.Windows.Forms.RadioButton();
            this.OptS = new System.Windows.Forms.RadioButton();
            this.Grid = new System.Windows.Forms.DataGridView();
            this.textBox1 = new V_Components.MyTextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.OptP);
            this.groupBox1.Controls.Add(this.OptE);
            this.groupBox1.Controls.Add(this.OptS);
            this.groupBox1.Controls.Add(this.Grid);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(4, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(505, 376);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // OptP
            // 
            this.OptP.AutoSize = true;
            this.OptP.Location = new System.Drawing.Point(141, 17);
            this.OptP.Name = "OptP";
            this.OptP.Size = new System.Drawing.Size(52, 17);
            this.OptP.TabIndex = 2;
            this.OptP.Text = "&Part";
            this.OptP.UseVisualStyleBackColor = true;
            this.OptP.CheckedChanged += new System.EventHandler(this.OptP_CheckedChanged);
            // 
            // OptE
            // 
            this.OptE.AutoSize = true;
            this.OptE.Location = new System.Drawing.Point(236, 17);
            this.OptE.Name = "OptE";
            this.OptE.Size = new System.Drawing.Size(85, 17);
            this.OptE.TabIndex = 2;
            this.OptE.Text = "&EndsWith";
            this.OptE.UseVisualStyleBackColor = true;
            this.OptE.CheckedChanged += new System.EventHandler(this.OptE_CheckedChanged);
            // 
            // OptS
            // 
            this.OptS.AutoSize = true;
            this.OptS.Checked = true;
            this.OptS.Location = new System.Drawing.Point(9, 17);
            this.OptS.Name = "OptS";
            this.OptS.Size = new System.Drawing.Size(93, 17);
            this.OptS.TabIndex = 2;
            this.OptS.TabStop = true;
            this.OptS.Text = "&StartsWith";
            this.OptS.UseVisualStyleBackColor = true;
            this.OptS.CheckedChanged += new System.EventHandler(this.OptS_CheckedChanged);
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(6, 69);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(492, 300);
            this.Grid.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox1.Location = new System.Drawing.Point(6, 42);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(492, 21);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // Frm_Ledger_Selection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(514, 380);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Frm_Ledger_Selection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Ledger_Selection_KeyDown);
            this.Load += new System.EventHandler(this.Frm_Ledger_Selection_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView Grid;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton OptS;
        private System.Windows.Forms.RadioButton OptP;
        private System.Windows.Forms.RadioButton OptE;
    }
}