namespace Accounts
{
    partial class FrmGeneralPo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.Grid = new DotnetVFGrid.MyDataGridView();
            this.TxtAmount = new V_Components.MyTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtQTY = new V_Components.MyTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtTotal = new V_Components.MyTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.DtpReqDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.Grid_Tax = new DotnetVFGrid.MyDataGridView();
            this.TxtSupplier = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.TxtPONO = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ArrowOcnType = new System.Windows.Forms.PictureBox();
            this.GBMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Tax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArrowOcnType)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.ArrowOcnType);
            this.GBMain.Controls.Add(this.Grid);
            this.GBMain.Controls.Add(this.TxtAmount);
            this.GBMain.Controls.Add(this.label6);
            this.GBMain.Controls.Add(this.TxtQTY);
            this.GBMain.Controls.Add(this.label7);
            this.GBMain.Controls.Add(this.TxtTotal);
            this.GBMain.Controls.Add(this.label8);
            this.GBMain.Controls.Add(this.DtpReqDate);
            this.GBMain.Controls.Add(this.label5);
            this.GBMain.Controls.Add(this.Grid_Tax);
            this.GBMain.Controls.Add(this.TxtSupplier);
            this.GBMain.Controls.Add(this.label2);
            this.GBMain.Controls.Add(this.DtpDate);
            this.GBMain.Controls.Add(this.TxtPONO);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Location = new System.Drawing.Point(9, 9);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(697, 419);
            this.GBMain.TabIndex = 1;
            this.GBMain.TabStop = false;
            // 
            // Grid
            // 
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
            this.Grid.Location = new System.Drawing.Point(13, 59);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid.Size = new System.Drawing.Size(670, 235);
            this.Grid.TabIndex = 48;
            this.Grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_EditingControlShowing);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            this.Grid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Grid_RowsRemoved);
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDoubleClick);
            // 
            // TxtAmount
            // 
            this.TxtAmount.Location = new System.Drawing.Point(574, 305);
            this.TxtAmount.Name = "TxtAmount";
            this.TxtAmount.Size = new System.Drawing.Size(109, 20);
            this.TxtAmount.TabIndex = 43;
            this.TxtAmount.TabStop = false;
            this.TxtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(491, 308);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "GROSS";
            // 
            // TxtQTY
            // 
            this.TxtQTY.BackColor = System.Drawing.Color.White;
            this.TxtQTY.Location = new System.Drawing.Point(574, 361);
            this.TxtQTY.Name = "TxtQTY";
            this.TxtQTY.Size = new System.Drawing.Size(109, 20);
            this.TxtQTY.TabIndex = 45;
            this.TxtQTY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(491, 364);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 44;
            this.label7.Text = "QTY";
            // 
            // TxtTotal
            // 
            this.TxtTotal.Location = new System.Drawing.Point(574, 388);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(109, 20);
            this.TxtTotal.TabIndex = 47;
            this.TxtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(491, 391);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 46;
            this.label8.Text = "AMOUNT";
            // 
            // DtpReqDate
            // 
            this.DtpReqDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpReqDate.Location = new System.Drawing.Point(574, 334);
            this.DtpReqDate.Name = "DtpReqDate";
            this.DtpReqDate.Size = new System.Drawing.Size(109, 20);
            this.DtpReqDate.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(490, 338);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 41;
            this.label5.Text = "REQ. DATE";
            // 
            // Grid_Tax
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Grid_Tax.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.Grid_Tax.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Grid_Tax.DefaultCellStyle = dataGridViewCellStyle8;
            this.Grid_Tax.Location = new System.Drawing.Point(13, 300);
            this.Grid_Tax.Name = "Grid_Tax";
            this.Grid_Tax.Size = new System.Drawing.Size(367, 109);
            this.Grid_Tax.TabIndex = 7;
            this.Grid_Tax.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_Tax_EditingControlShowing);
            this.Grid_Tax.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_Tax_RowsAdded);
            this.Grid_Tax.DoubleClick += new System.EventHandler(this.Grid_Tax_DoubleClick);
            this.Grid_Tax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_Tax_KeyPress);
            // 
            // TxtSupplier
            // 
            this.TxtSupplier.Location = new System.Drawing.Point(377, 25);
            this.TxtSupplier.Name = "TxtSupplier";
            this.TxtSupplier.Size = new System.Drawing.Size(278, 20);
            this.TxtSupplier.TabIndex = 2;
            this.TxtSupplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(320, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SUPPLIER";
            // 
            // DtpDate
            // 
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDate.Location = new System.Drawing.Point(206, 25);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(109, 20);
            this.DtpDate.TabIndex = 1;
            this.DtpDate.TabStop = false;
            // 
            // TxtPONO
            // 
            this.TxtPONO.Location = new System.Drawing.Point(92, 25);
            this.TxtPONO.Name = "TxtPONO";
            this.TxtPONO.Size = new System.Drawing.Size(108, 20);
            this.TxtPONO.TabIndex = 0;
            this.TxtPONO.TabStop = false;
            this.TxtPONO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "PO NO";
            // 
            // ArrowOcnType
            // 
            this.ArrowOcnType.Image = global::Vsocks.Properties.Resources.Down;
            this.ArrowOcnType.Location = new System.Drawing.Point(658, 24);
            this.ArrowOcnType.Name = "ArrowOcnType";
            this.ArrowOcnType.Size = new System.Drawing.Size(25, 21);
            this.ArrowOcnType.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ArrowOcnType.TabIndex = 76;
            this.ArrowOcnType.TabStop = false;
            // 
            // FrmGeneralPo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 436);
            this.Controls.Add(this.GBMain);
            this.KeyPreview = true;
            this.Name = "FrmGeneralPo";
            this.Text = "FrmGeneralPo";
            this.Load += new System.EventHandler(this.FrmGeneralPo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmGeneralPo_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmGeneralPo_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Tax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArrowOcnType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private V_Components.MyTextBox TxtAmount;
        private System.Windows.Forms.Label label6;
        private V_Components.MyTextBox TxtQTY;
        private System.Windows.Forms.Label label7;
        private V_Components.MyTextBox TxtTotal;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker DtpReqDate;
        private System.Windows.Forms.Label label5;
        private DotnetVFGrid.MyDataGridView Grid_Tax;
        private V_Components.MyTextBox TxtSupplier;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private V_Components.MyTextBox TxtPONO;
        private System.Windows.Forms.Label label1;
        private DotnetVFGrid.MyDataGridView Grid;
        private System.Windows.Forms.PictureBox ArrowOcnType;
    }
}