namespace Accounts
{
    partial class FrmItemWiseRateEntry
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.ChkOld = new System.Windows.Forms.CheckBox();
            this.Arrow_Buyer = new System.Windows.Forms.PictureBox();
            this.ButExit = new System.Windows.Forms.Button();
            this.ButCancel = new System.Windows.Forms.Button();
            this.ButSave = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtMode = new V_Components.MyTextBox();
            this.TxtENo = new V_Components.MyTextBox();
            this.TxtTotalCount = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow_Buyer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.ChkOld);
            this.GBMain.Controls.Add(this.Arrow_Buyer);
            this.GBMain.Controls.Add(this.ButExit);
            this.GBMain.Controls.Add(this.ButCancel);
            this.GBMain.Controls.Add(this.ButSave);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.TxtMode);
            this.GBMain.Controls.Add(this.TxtENo);
            this.GBMain.Controls.Add(this.TxtTotalCount);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Location = new System.Drawing.Point(6, 8);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(607, 452);
            this.GBMain.TabIndex = 1;
            this.GBMain.TabStop = false;
            this.GBMain.Enter += new System.EventHandler(this.GBMain_Enter);
            // 
            // ChkOld
            // 
            this.ChkOld.AutoSize = true;
            this.ChkOld.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkOld.Location = new System.Drawing.Point(184, 420);
            this.ChkOld.Name = "ChkOld";
            this.ChkOld.Size = new System.Drawing.Size(52, 19);
            this.ChkOld.TabIndex = 2;
            this.ChkOld.Text = "OLD";
            this.ChkOld.UseVisualStyleBackColor = true;
            // 
            // Arrow_Buyer
            // 
            this.Arrow_Buyer.Image = global::Vsocks.Properties.Resources.Down;
            this.Arrow_Buyer.Location = new System.Drawing.Point(178, 17);
            this.Arrow_Buyer.Name = "Arrow_Buyer";
            this.Arrow_Buyer.Size = new System.Drawing.Size(25, 21);
            this.Arrow_Buyer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow_Buyer.TabIndex = 76;
            this.Arrow_Buyer.TabStop = false;
            // 
            // ButExit
            // 
            this.ButExit.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButExit.Location = new System.Drawing.Point(490, 416);
            this.ButExit.Name = "ButExit";
            this.ButExit.Size = new System.Drawing.Size(104, 28);
            this.ButExit.TabIndex = 9;
            this.ButExit.Text = "EXIT";
            this.ButExit.UseVisualStyleBackColor = true;
            this.ButExit.Click += new System.EventHandler(this.ButExit_Click);
            // 
            // ButCancel
            // 
            this.ButCancel.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButCancel.Location = new System.Drawing.Point(380, 416);
            this.ButCancel.Name = "ButCancel";
            this.ButCancel.Size = new System.Drawing.Size(104, 28);
            this.ButCancel.TabIndex = 8;
            this.ButCancel.Text = "CLEAR";
            this.ButCancel.UseVisualStyleBackColor = true;
            this.ButCancel.Click += new System.EventHandler(this.ButCancel_Click);
            // 
            // ButSave
            // 
            this.ButSave.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButSave.Location = new System.Drawing.Point(270, 416);
            this.ButSave.Name = "ButSave";
            this.ButSave.Size = new System.Drawing.Size(104, 28);
            this.ButSave.TabIndex = 4;
            this.ButSave.Text = "SAVE";
            this.ButSave.UseVisualStyleBackColor = true;
            this.ButSave.Click += new System.EventHandler(this.ButSave_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(21, 422);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 15);
            this.label6.TabIndex = 57;
            this.label6.Text = "TOTAL COUNT";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // TxtMode
            // 
            this.TxtMode.Location = new System.Drawing.Point(65, 17);
            this.TxtMode.Name = "TxtMode";
            this.TxtMode.Size = new System.Drawing.Size(107, 20);
            this.TxtMode.TabIndex = 0;
            this.TxtMode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtENo
            // 
            this.TxtENo.Location = new System.Drawing.Point(405, 17);
            this.TxtENo.Name = "TxtENo";
            this.TxtENo.Size = new System.Drawing.Size(38, 20);
            this.TxtENo.TabIndex = 3;
            this.TxtENo.TabStop = false;
            this.TxtENo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtTotalCount
            // 
            this.TxtTotalCount.Location = new System.Drawing.Point(125, 419);
            this.TxtTotalCount.Name = "TxtTotalCount";
            this.TxtTotalCount.Size = new System.Drawing.Size(47, 20);
            this.TxtTotalCount.TabIndex = 2;
            this.TxtTotalCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Grid
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Grid.DefaultCellStyle = dataGridViewCellStyle2;
            this.Grid.Location = new System.Drawing.Point(18, 48);
            this.Grid.Name = "Grid";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.Grid.Size = new System.Drawing.Size(579, 363);
            this.Grid.TabIndex = 1;
            this.Grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellContentClick);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(270, 17);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(78, 20);
            this.DtpDate.TabIndex = 1;
            this.DtpDate.ValueChanged += new System.EventHandler(this.DtpRDate_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(373, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "NO";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(225, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "DATE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "MODE";
            // 
            // FrmItemWiseRateEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 472);
            this.ControlBox = false;
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmItemWiseRateEntry";
            this.Text = "ITEM WISE RATE ENTRY";
            this.Load += new System.EventHandler(this.FrmItemWiseRateEntry_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmItemWiseRateEntry_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmItemWiseRateEntry_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow_Buyer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private V_Components.MyTextBox TxtTotalCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ButExit;
        private System.Windows.Forms.Button ButCancel;
        private System.Windows.Forms.Button ButSave;
        private System.Windows.Forms.PictureBox Arrow_Buyer;
        private System.Windows.Forms.CheckBox ChkOld;
        private V_Components.MyTextBox TxtENo;
        private V_Components.MyTextBox TxtMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;

    }
}