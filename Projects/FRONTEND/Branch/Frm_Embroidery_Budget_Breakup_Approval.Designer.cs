namespace Accounts
{
    partial class Frm_Embroidery_Budget_Breakup_Approval
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DtpEDate = new System.Windows.Forms.DateTimePicker();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.TxtSelectedApproved = new V_Components.MyTextBox();
            this.TxtSelectedEntered = new V_Components.MyTextBox();
            this.TxtTotalApproved = new V_Components.MyTextBox();
            this.TxtTotalEntered = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtTotOrder = new V_Components.MyTextBox();
            this.TxtEntryNo = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(360, 407);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 65;
            this.label1.Text = "SELECTED";
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(605, 399);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(74, 31);
            this.button9.TabIndex = 48;
            this.button9.Text = "&APPROVE";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 405);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(89, 17);
            this.checkBox1.TabIndex = 64;
            this.checkBox1.Text = "&SELECT ALL";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(685, 399);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(74, 31);
            this.button3.TabIndex = 46;
            this.button3.Text = "E&XIT";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(340, 15);
            this.label6.TabIndex = 63;
            this.label6.Text = "EMBROIDERY BUDGET APPROVAL BREAK\'UP DETIALS :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(486, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 15);
            this.label3.TabIndex = 60;
            this.label3.Text = "APPROVED / ENTERED";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(351, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 15);
            this.label4.TabIndex = 56;
            this.label4.Text = "ORDER\'S";
            // 
            // DtpEDate
            // 
            this.DtpEDate.CustomFormat = "dd/MM/yyyy";
            this.DtpEDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpEDate.Location = new System.Drawing.Point(125, 118);
            this.DtpEDate.Name = "DtpEDate";
            this.DtpEDate.Size = new System.Drawing.Size(86, 20);
            this.DtpEDate.TabIndex = 0;
            this.DtpEDate.Value = new System.DateTime(2013, 10, 19, 0, 0, 0, 0);
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.TxtSelectedApproved);
            this.GBMain.Controls.Add(this.TxtSelectedEntered);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.button9);
            this.GBMain.Controls.Add(this.checkBox1);
            this.GBMain.Controls.Add(this.button3);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.TxtTotalApproved);
            this.GBMain.Controls.Add(this.TxtTotalEntered);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.TxtTotOrder);
            this.GBMain.Controls.Add(this.TxtEntryNo);
            this.GBMain.Controls.Add(this.DtpEDate);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Location = new System.Drawing.Point(12, 12);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(765, 440);
            this.GBMain.TabIndex = 2;
            this.GBMain.TabStop = false;
            // 
            // TxtSelectedApproved
            // 
            this.TxtSelectedApproved.Location = new System.Drawing.Point(436, 405);
            this.TxtSelectedApproved.Name = "TxtSelectedApproved";
            this.TxtSelectedApproved.Size = new System.Drawing.Size(80, 20);
            this.TxtSelectedApproved.TabIndex = 67;
            this.TxtSelectedApproved.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtSelectedEntered
            // 
            this.TxtSelectedEntered.Location = new System.Drawing.Point(519, 405);
            this.TxtSelectedEntered.Name = "TxtSelectedEntered";
            this.TxtSelectedEntered.Size = new System.Drawing.Size(80, 20);
            this.TxtSelectedEntered.TabIndex = 66;
            this.TxtSelectedEntered.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtTotalApproved
            // 
            this.TxtTotalApproved.Location = new System.Drawing.Point(634, 13);
            this.TxtTotalApproved.Name = "TxtTotalApproved";
            this.TxtTotalApproved.Size = new System.Drawing.Size(59, 20);
            this.TxtTotalApproved.TabIndex = 59;
            this.TxtTotalApproved.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtTotalEntered
            // 
            this.TxtTotalEntered.Location = new System.Drawing.Point(699, 13);
            this.TxtTotalEntered.Name = "TxtTotalEntered";
            this.TxtTotalEntered.Size = new System.Drawing.Size(59, 20);
            this.TxtTotalEntered.TabIndex = 8;
            this.TxtTotalEntered.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(9, 39);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid.Size = new System.Drawing.Size(750, 354);
            this.Grid.TabIndex = 1;
            this.Grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellContentClick);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // TxtTotOrder
            // 
            this.TxtTotOrder.Location = new System.Drawing.Point(420, 13);
            this.TxtTotOrder.Name = "TxtTotOrder";
            this.TxtTotOrder.Size = new System.Drawing.Size(56, 20);
            this.TxtTotOrder.TabIndex = 2;
            this.TxtTotOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtEntryNo
            // 
            this.TxtEntryNo.Location = new System.Drawing.Point(217, 118);
            this.TxtEntryNo.Name = "TxtEntryNo";
            this.TxtEntryNo.Size = new System.Drawing.Size(52, 20);
            this.TxtEntryNo.TabIndex = 68;
            this.TxtEntryNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(36, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "ENTRY DATE";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Frm_Embroidery_Budget_Breakup_Approval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 458);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "Frm_Embroidery_Budget_Breakup_Approval";
            this.Text = "Frm_Embroidery_Budget_Breakup_Approval";
            this.Load += new System.EventHandler(this.Frm_Embroidery_Budget_Breakup_Approval_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Embroidery_Budget_Breakup_Approval_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Embroidery_Budget_Breakup_Approval_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private V_Components.MyTextBox TxtSelectedApproved;
        private V_Components.MyTextBox TxtSelectedEntered;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox TxtTotalApproved;
        private V_Components.MyTextBox TxtTotalEntered;
        private System.Windows.Forms.Label label4;
        private DotnetVFGrid.MyDataGridView Grid;
        private V_Components.MyTextBox TxtTotOrder;
        private V_Components.MyTextBox TxtEntryNo;
        private System.Windows.Forms.DateTimePicker DtpEDate;
        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
    }
}