namespace Accounts
{
    partial class FrmTimeActionPlanEntry_Socks
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
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.DtpODate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.DtpSDate = new System.Windows.Forms.DateTimePicker();
            this.DtpOrdEnqDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtTotPro = new V_Components.MyTextBox();
            this.TxtTotalOrder = new V_Components.MyTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtEntryNo = new V_Components.MyTextBox();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.Arrow1 = new System.Windows.Forms.PictureBox();
            this.TxtOrderNo = new V_Components.MyTextBox();
            this.Arrow2 = new System.Windows.Forms.PictureBox();
            this.TxtDivision = new V_Components.MyTextBox();
            this.TxtParty = new V_Components.MyTextBox();
            this.TxtLeadDays = new V_Components.MyTextBox();
            this.TxtOrderList = new V_Components.MyTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.GBMain = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).BeginInit();
            this.GBMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // DtpDate
            // 
            this.DtpDate.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(155, 20);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(66, 22);
            this.DtpDate.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(17, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "LEAD DAYS";
            // 
            // DtpODate
            // 
            this.DtpODate.Enabled = false;
            this.DtpODate.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpODate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpODate.Location = new System.Drawing.Point(362, 83);
            this.DtpODate.Name = "DtpODate";
            this.DtpODate.Size = new System.Drawing.Size(61, 22);
            this.DtpODate.TabIndex = 8;
            this.DtpODate.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(228, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(157, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "ORDER / SHIP/ SRT DATE";
            // 
            // DtpSDate
            // 
            this.DtpSDate.Enabled = false;
            this.DtpSDate.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpSDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpSDate.Location = new System.Drawing.Point(424, 83);
            this.DtpSDate.Name = "DtpSDate";
            this.DtpSDate.Size = new System.Drawing.Size(60, 22);
            this.DtpSDate.TabIndex = 8;
            this.DtpSDate.TabStop = false;
            // 
            // DtpOrdEnqDate
            // 
            this.DtpOrdEnqDate.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpOrdEnqDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpOrdEnqDate.Location = new System.Drawing.Point(485, 83);
            this.DtpOrdEnqDate.Name = "DtpOrdEnqDate";
            this.DtpOrdEnqDate.Size = new System.Drawing.Size(58, 22);
            this.DtpOrdEnqDate.TabIndex = 8;
            this.DtpOrdEnqDate.TabStop = false;
            this.DtpOrdEnqDate.ValueChanged += new System.EventHandler(this.DtpOrdEnqDate_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(17, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "ORDER NO";
            // 
            // TxtTotPro
            // 
            this.TxtTotPro.Location = new System.Drawing.Point(467, 460);
            this.TxtTotPro.Name = "TxtTotPro";
            this.TxtTotPro.Size = new System.Drawing.Size(74, 20);
            this.TxtTotPro.TabIndex = 4;
            this.TxtTotPro.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtTotalOrder
            // 
            this.TxtTotalOrder.Location = new System.Drawing.Point(547, 461);
            this.TxtTotalOrder.Name = "TxtTotalOrder";
            this.TxtTotalOrder.Size = new System.Drawing.Size(103, 20);
            this.TxtTotalOrder.TabIndex = 6;
            this.TxtTotalOrder.TabStop = false;
            this.TxtTotalOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(228, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "DIVISION";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(228, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "PARTY";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "ENTRY NO";
            // 
            // TxtEntryNo
            // 
            this.TxtEntryNo.Enabled = false;
            this.TxtEntryNo.Location = new System.Drawing.Point(93, 21);
            this.TxtEntryNo.Name = "TxtEntryNo";
            this.TxtEntryNo.Size = new System.Drawing.Size(61, 20);
            this.TxtEntryNo.TabIndex = 0;
            this.TxtEntryNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Arrow3
            // 
            this.Arrow3.Image = global::Branch.Properties.Resources.Down;
            this.Arrow3.Location = new System.Drawing.Point(196, 52);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(25, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 55;
            this.Arrow3.TabStop = false;
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(20, 110);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid.Size = new System.Drawing.Size(521, 342);
            this.Grid.TabIndex = 3;
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
            // 
            // Arrow1
            // 
            this.Arrow1.Image = global::Branch.Properties.Resources.Down;
            this.Arrow1.Location = new System.Drawing.Point(516, 19);
            this.Arrow1.Name = "Arrow1";
            this.Arrow1.Size = new System.Drawing.Size(25, 21);
            this.Arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow1.TabIndex = 55;
            this.Arrow1.TabStop = false;
            // 
            // TxtOrderNo
            // 
            this.TxtOrderNo.Location = new System.Drawing.Point(93, 52);
            this.TxtOrderNo.Name = "TxtOrderNo";
            this.TxtOrderNo.Size = new System.Drawing.Size(103, 20);
            this.TxtOrderNo.TabIndex = 1;
            this.TxtOrderNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Arrow2
            // 
            this.Arrow2.Image = global::Branch.Properties.Resources.Down;
            this.Arrow2.Location = new System.Drawing.Point(196, 83);
            this.Arrow2.Name = "Arrow2";
            this.Arrow2.Size = new System.Drawing.Size(25, 21);
            this.Arrow2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow2.TabIndex = 55;
            this.Arrow2.TabStop = false;
            // 
            // TxtDivision
            // 
            this.TxtDivision.Location = new System.Drawing.Point(298, 20);
            this.TxtDivision.Name = "TxtDivision";
            this.TxtDivision.Size = new System.Drawing.Size(218, 20);
            this.TxtDivision.TabIndex = 0;
            this.TxtDivision.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtParty
            // 
            this.TxtParty.Location = new System.Drawing.Point(298, 52);
            this.TxtParty.Name = "TxtParty";
            this.TxtParty.Size = new System.Drawing.Size(243, 20);
            this.TxtParty.TabIndex = 0;
            this.TxtParty.TabStop = false;
            this.TxtParty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtLeadDays
            // 
            this.TxtLeadDays.Location = new System.Drawing.Point(93, 83);
            this.TxtLeadDays.Name = "TxtLeadDays";
            this.TxtLeadDays.Size = new System.Drawing.Size(103, 20);
            this.TxtLeadDays.TabIndex = 2;
            this.TxtLeadDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtOrderList
            // 
            this.TxtOrderList.Location = new System.Drawing.Point(547, 110);
            this.TxtOrderList.Multiline = true;
            this.TxtOrderList.Name = "TxtOrderList";
            this.TxtOrderList.Size = new System.Drawing.Size(103, 342);
            this.TxtOrderList.TabIndex = 5;
            this.TxtOrderList.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(388, 462);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 15);
            this.label5.TabIndex = 56;
            this.label5.Text = "MAX DAYS";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(20, 458);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 23);
            this.button1.TabIndex = 1;
            this.button1.TabStop = false;
            this.button1.Text = "TEMPLATE";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(113, 457);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 23);
            this.button2.TabIndex = 1;
            this.button2.TabStop = false;
            this.button2.Text = "APPROVE";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.button2);
            this.GBMain.Controls.Add(this.button1);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.TxtOrderList);
            this.GBMain.Controls.Add(this.TxtLeadDays);
            this.GBMain.Controls.Add(this.TxtParty);
            this.GBMain.Controls.Add(this.TxtDivision);
            this.GBMain.Controls.Add(this.Arrow2);
            this.GBMain.Controls.Add(this.TxtOrderNo);
            this.GBMain.Controls.Add(this.Arrow1);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.Arrow3);
            this.GBMain.Controls.Add(this.TxtEntryNo);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.TxtTotalOrder);
            this.GBMain.Controls.Add(this.TxtTotPro);
            this.GBMain.Controls.Add(this.label3);
            this.GBMain.Controls.Add(this.DtpOrdEnqDate);
            this.GBMain.Controls.Add(this.DtpSDate);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.DtpODate);
            this.GBMain.Controls.Add(this.label4);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Location = new System.Drawing.Point(12, 12);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(657, 486);
            this.GBMain.TabIndex = 1;
            this.GBMain.TabStop = false;
            // 
            // FrmTimeActionPlanEntry_Socks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 502);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmTimeActionPlanEntry_Socks";
            this.Text = "Time & Action Plan Entry";
            this.Load += new System.EventHandler(this.FrmTimeActionPlanEntry_Socks_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmTimeActionPlanEntry_Socks_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmTimeActionPlanEntry_Socks_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow2)).EndInit();
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker DtpODate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker DtpSDate;
        private System.Windows.Forms.DateTimePicker DtpOrdEnqDate;
        private System.Windows.Forms.Label label3;
        private V_Components.MyTextBox TxtTotPro;
        private V_Components.MyTextBox TxtTotalOrder;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private V_Components.MyTextBox TxtEntryNo;
        private System.Windows.Forms.PictureBox Arrow3;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.PictureBox Arrow1;
        private V_Components.MyTextBox TxtOrderNo;
        private System.Windows.Forms.PictureBox Arrow2;
        private V_Components.MyTextBox TxtDivision;
        private V_Components.MyTextBox TxtParty;
        private V_Components.MyTextBox TxtLeadDays;
        private V_Components.MyTextBox TxtOrderList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox GBMain;


    }
}