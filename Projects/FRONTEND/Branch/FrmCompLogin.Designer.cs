namespace Accounts
{
    partial class FrmCompLogin
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
            this.Grid = new System.Windows.Forms.DataGridView();
            this.ButOK = new System.Windows.Forms.Button();
            this.ButCancel = new System.Windows.Forms.Button();
            this.ButCompany = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(7, 12);
            this.Grid.Margin = new System.Windows.Forms.Padding(4);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid.Size = new System.Drawing.Size(639, 226);
            this.Grid.TabIndex = 0;
            // 
            // ButOK
            // 
            this.ButOK.Location = new System.Drawing.Point(436, 246);
            this.ButOK.Margin = new System.Windows.Forms.Padding(4);
            this.ButOK.Name = "ButOK";
            this.ButOK.Size = new System.Drawing.Size(101, 33);
            this.ButOK.TabIndex = 1;
            this.ButOK.Text = "&Ok";
            this.ButOK.UseVisualStyleBackColor = true;
            this.ButOK.Click += new System.EventHandler(this.ButOK_Click);
            // 
            // ButCancel
            // 
            this.ButCancel.Location = new System.Drawing.Point(545, 246);
            this.ButCancel.Margin = new System.Windows.Forms.Padding(4);
            this.ButCancel.Name = "ButCancel";
            this.ButCancel.Size = new System.Drawing.Size(101, 33);
            this.ButCancel.TabIndex = 1;
            this.ButCancel.Text = "Cance&l";
            this.ButCancel.UseVisualStyleBackColor = true;
            this.ButCancel.Click += new System.EventHandler(this.ButCancel_Click);
            // 
            // ButCompany
            // 
            this.ButCompany.Location = new System.Drawing.Point(7, 246);
            this.ButCompany.Margin = new System.Windows.Forms.Padding(4);
            this.ButCompany.Name = "ButCompany";
            this.ButCompany.Size = new System.Drawing.Size(101, 33);
            this.ButCompany.TabIndex = 1;
            this.ButCompany.Text = "&Company";
            this.ButCompany.UseVisualStyleBackColor = true;
            this.ButCompany.Visible = false;
            this.ButCompany.Click += new System.EventHandler(this.ButCompany_Click);
            // 
            // FrmCompLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(653, 292);
            this.ControlBox = false;
            this.Controls.Add(this.ButCancel);
            this.Controls.Add(this.ButCompany);
            this.Controls.Add(this.ButOK);
            this.Controls.Add(this.Grid);
            this.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmCompLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Company ...!";
            this.Load += new System.EventHandler(this.FrmCompLogin_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmCompLogin_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmCompLogin_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView Grid;
        private System.Windows.Forms.Button ButOK;
        private System.Windows.Forms.Button ButCancel;
        private System.Windows.Forms.Button ButCompany;
    }
}