namespace Accounts
{
    partial class FrmJobOrderIssueEntry
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
            this.CmbIssueType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtTotalQty = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtBuyer = new V_Components.MyTextBox();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtUnit = new V_Components.MyTextBox();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtJONo = new V_Components.MyTextBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.CmbIssueType);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.TxtTotalQty);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.TxtBuyer);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.TxtUnit);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtJONo);
            this.GBMain.Location = new System.Drawing.Point(7, 0);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(762, 416);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            // 
            // CmbIssueType
            // 
            this.CmbIssueType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbIssueType.FormattingEnabled = true;
            this.CmbIssueType.Location = new System.Drawing.Point(79, 57);
            this.CmbIssueType.Name = "CmbIssueType";
            this.CmbIssueType.Size = new System.Drawing.Size(128, 21);
            this.CmbIssueType.TabIndex = 3;
            this.CmbIssueType.SelectedIndexChanged += new System.EventHandler(this.CmbIssueType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 63;
            this.label5.Text = "ISSUE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(607, 386);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 61;
            this.label4.Text = "QTY";
            // 
            // TxtTotalQty
            // 
            this.TxtTotalQty.Location = new System.Drawing.Point(658, 383);
            this.TxtTotalQty.Name = "TxtTotalQty";
            this.TxtTotalQty.Size = new System.Drawing.Size(92, 21);
            this.TxtTotalQty.TabIndex = 6;
            this.TxtTotalQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(9, 98);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(741, 273);
            this.Grid.TabIndex = 5;
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.Leave += new System.EventHandler(this.Grid_Leave);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow1.Location = new System.Drawing.Point(724, 30);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(26, 21);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 59;
            this.Arrow1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(339, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 57;
            this.label3.Text = "BUYER";
            // 
            // TxtBuyer
            // 
            this.TxtBuyer.Location = new System.Drawing.Point(396, 30);
            this.TxtBuyer.Name = "TxtBuyer";
            this.TxtBuyer.Size = new System.Drawing.Size(322, 21);
            this.TxtBuyer.TabIndex = 2;
            this.TxtBuyer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down1;
            this.Arrow3.Location = new System.Drawing.Point(725, 57);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(26, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 56;
            this.Arrow3.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(339, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "UNIT";
            // 
            // TxtUnit
            // 
            this.TxtUnit.Location = new System.Drawing.Point(396, 57);
            this.TxtUnit.Name = "TxtUnit";
            this.TxtUnit.Size = new System.Drawing.Size(322, 21);
            this.TxtUnit.TabIndex = 4;
            this.TxtUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(213, 30);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(93, 21);
            this.DtpDate.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "JO NO";
            // 
            // TxtJONo
            // 
            this.TxtJONo.Location = new System.Drawing.Point(79, 28);
            this.TxtJONo.Name = "TxtJONo";
            this.TxtJONo.Size = new System.Drawing.Size(128, 21);
            this.TxtJONo.TabIndex = 0;
            this.TxtJONo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmJobOrderIssueEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 424);
            this.Controls.Add(this.GBMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "FrmJobOrderIssueEntry";
            this.Text = "JOB ORDER ISSUE ENTRY";
            this.Load += new System.EventHandler(this.FrmJobOrderIssueEntry_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmJobOrderIssueEntry_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmJobOrderIssueEntry_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtJONo;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.Label label2;
        private V_Components.MyTextBox TxtUnit;
        private System.Windows.Forms.PictureBox Arrow3;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox TxtBuyer;
        private System.Windows.Forms.PictureBox Arrow1;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtTotalQty;
        private System.Windows.Forms.ComboBox CmbIssueType;
        private System.Windows.Forms.Label label5;
    }
}