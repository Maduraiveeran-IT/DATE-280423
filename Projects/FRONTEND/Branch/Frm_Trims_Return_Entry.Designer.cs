namespace Accounts
{
    partial class Frm_Trims_Return_Entry
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtOrder = new V_Components.MyTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.TxtIssueNo = new V_Components.MyTextBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.BtnAccept = new System.Windows.Forms.Button();
            this.TxtTotal = new V_Components.MyTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtUnit = new V_Components.MyTextBox();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.GBStore = new System.Windows.Forms.GroupBox();
            this.BtnReject = new System.Windows.Forms.Button();
            this.LblSpecial = new System.Windows.Forms.Label();
            this.TxtRemarks = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.TxtEntryNo = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.Arrow2 = new System.Windows.Forms.PictureBox();
            this.Arrow4 = new System.Windows.Forms.PictureBox();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.GBStore.SuspendLayout();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(107, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 16);
            this.label4.TabIndex = 74;
            this.label4.Text = "ORDER NO";
            // 
            // TxtOrder
            // 
            this.TxtOrder.Location = new System.Drawing.Point(190, 69);
            this.TxtOrder.Name = "TxtOrder";
            this.TxtOrder.Size = new System.Drawing.Size(164, 20);
            this.TxtOrder.TabIndex = 2;
            this.TxtOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(390, 70);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(76, 16);
            this.label13.TabIndex = 71;
            this.label13.Text = "ISSUE NO";
            // 
            // TxtIssueNo
            // 
            this.TxtIssueNo.Location = new System.Drawing.Point(491, 69);
            this.TxtIssueNo.Name = "TxtIssueNo";
            this.TxtIssueNo.Size = new System.Drawing.Size(128, 20);
            this.TxtIssueNo.TabIndex = 3;
            this.TxtIssueNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Grid.DefaultCellStyle = dataGridViewCellStyle6;
            this.Grid.Location = new System.Drawing.Point(4, 95);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(643, 279);
            this.Grid.TabIndex = 4;
            this.Grid.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellLeave);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // BtnAccept
            // 
            this.BtnAccept.Location = new System.Drawing.Point(6, 11);
            this.BtnAccept.Name = "BtnAccept";
            this.BtnAccept.Size = new System.Drawing.Size(75, 23);
            this.BtnAccept.TabIndex = 79;
            this.BtnAccept.Text = "ACCEPT";
            this.BtnAccept.UseVisualStyleBackColor = true;
            this.BtnAccept.Click += new System.EventHandler(this.BtnAccept_Click);
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(491, 380);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(156, 20);
            this.TxtTotal.TabIndex = 6;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(432, 381);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 16);
            this.label3.TabIndex = 65;
            this.label3.Text = "TOTAL";
            // 
            // TxtUnit
            // 
            this.TxtUnit.Location = new System.Drawing.Point(189, 36);
            this.TxtUnit.Name = "TxtUnit";
            this.TxtUnit.Size = new System.Drawing.Size(165, 20);
            this.TxtUnit.TabIndex = 0;
            this.TxtUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(168, 11);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 81;
            this.BtnCancel.Text = "CANCEL";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // GBStore
            // 
            this.GBStore.Controls.Add(this.BtnAccept);
            this.GBStore.Controls.Add(this.BtnCancel);
            this.GBStore.Controls.Add(this.BtnReject);
            this.GBStore.Location = new System.Drawing.Point(393, 407);
            this.GBStore.Name = "GBStore";
            this.GBStore.Size = new System.Drawing.Size(253, 38);
            this.GBStore.TabIndex = 82;
            this.GBStore.TabStop = false;
            // 
            // BtnReject
            // 
            this.BtnReject.Location = new System.Drawing.Point(87, 11);
            this.BtnReject.Name = "BtnReject";
            this.BtnReject.Size = new System.Drawing.Size(75, 23);
            this.BtnReject.TabIndex = 80;
            this.BtnReject.Text = "REJECT";
            this.BtnReject.UseVisualStyleBackColor = true;
            this.BtnReject.Click += new System.EventHandler(this.BtnReject_Click);
            // 
            // LblSpecial
            // 
            this.LblSpecial.BackColor = System.Drawing.Color.Silver;
            this.LblSpecial.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSpecial.Location = new System.Drawing.Point(4, 9);
            this.LblSpecial.Name = "LblSpecial";
            this.LblSpecial.Size = new System.Drawing.Size(643, 21);
            this.LblSpecial.TabIndex = 69;
            this.LblSpecial.Text = "TRIMS INDENT RETURN ENTRY";
            this.LblSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Location = new System.Drawing.Point(4, 380);
            this.TxtRemarks.Multiline = true;
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(381, 20);
            this.TxtRemarks.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(390, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 16);
            this.label2.TabIndex = 61;
            this.label2.Text = "ENTRY DATE";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(107, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 16);
            this.label5.TabIndex = 59;
            this.label5.Text = "UNIT";
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(491, 37);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(128, 20);
            this.DtpDate.TabIndex = 1;
            // 
            // TxtEntryNo
            // 
            this.TxtEntryNo.Location = new System.Drawing.Point(39, 36);
            this.TxtEntryNo.Name = "TxtEntryNo";
            this.TxtEntryNo.Size = new System.Drawing.Size(62, 20);
            this.TxtEntryNo.TabIndex = 55;
            this.TxtEntryNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 16);
            this.label1.TabIndex = 56;
            this.label1.Text = "ENO";
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.GBStore);
            this.GBMain.Controls.Add(this.Arrow2);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.TxtOrder);
            this.GBMain.Controls.Add(this.Arrow4);
            this.GBMain.Controls.Add(this.label13);
            this.GBMain.Controls.Add(this.TxtIssueNo);
            this.GBMain.Controls.Add(this.LblSpecial);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.TxtRemarks);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtUnit);
            this.GBMain.Controls.Add(this.TxtEntryNo);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(4, 5);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(656, 451);
            this.GBMain.TabIndex = 6;
            this.GBMain.TabStop = false;
            // 
            // Arrow2
            // 
            this.Arrow2.Image = global::Accounts.Properties.Resources.Down1;
            this.Arrow2.Location = new System.Drawing.Point(363, 69);
            this.Arrow2.Name = "Arrow2";
            this.Arrow2.Size = new System.Drawing.Size(22, 20);
            this.Arrow2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow2.TabIndex = 75;
            this.Arrow2.TabStop = false;
            // 
            // Arrow4
            // 
            this.Arrow4.Image = global::Accounts.Properties.Resources.Down1;
            this.Arrow4.Location = new System.Drawing.Point(625, 70);
            this.Arrow4.Name = "Arrow4";
            this.Arrow4.Size = new System.Drawing.Size(22, 20);
            this.Arrow4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow4.TabIndex = 72;
            this.Arrow4.TabStop = false;
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Accounts.Properties.Resources.Down1;
            this.Arrow3.Location = new System.Drawing.Point(363, 37);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(22, 20);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 66;
            this.Arrow3.TabStop = false;
            // 
            // Frm_Trims_Return_Entry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 458);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "Frm_Trims_Return_Entry";
            this.Text = "Frm_Trims_Return_Entry";
            this.Load += new System.EventHandler(this.Frm_Trims_Return_Entry_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Trims_Return_Entry_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.GBStore.ResumeLayout(false);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Arrow2;
        private System.Windows.Forms.Label label4;
        private V_Components.MyTextBox TxtOrder;
        private System.Windows.Forms.PictureBox Arrow4;
        private System.Windows.Forms.Label label13;
        private V_Components.MyTextBox TxtIssueNo;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.Button BtnAccept;
        private V_Components.MyTextBox TxtTotal;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox TxtUnit;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.GroupBox GBStore;
        private System.Windows.Forms.Button BtnReject;
        private System.Windows.Forms.Label LblSpecial;
        private System.Windows.Forms.PictureBox Arrow3;
        private System.Windows.Forms.TextBox TxtRemarks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private V_Components.MyTextBox TxtEntryNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox GBMain;
    }
}