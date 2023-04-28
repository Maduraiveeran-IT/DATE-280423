namespace SelectionTool
{
    partial class FrmSelectionToolItem
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
            this.GBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TxtCriteria = new V_Components.MyTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.DtTo = new System.Windows.Forms.DateTimePicker();
            this.DtFrom = new System.Windows.Forms.DateTimePicker();
            this.TxtFrom = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtTo = new V_Components.MyTextBox();
            this.CmbCondition = new System.Windows.Forms.ComboBox();
            this.CmbFilter = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.GBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtCST = new V_Components.MyTextBox();
            this.TxtTin = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtAddress = new V_Components.MyTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.GBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.GBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBox1
            // 
            this.GBox1.Controls.Add(this.panel1);
            this.GBox1.Controls.Add(this.panel2);
            this.GBox1.Controls.Add(this.CmbCondition);
            this.GBox1.Controls.Add(this.CmbFilter);
            this.GBox1.Controls.Add(this.dataGridView1);
            this.GBox1.Location = new System.Drawing.Point(4, 122);
            this.GBox1.Name = "GBox1";
            this.GBox1.Size = new System.Drawing.Size(599, 344);
            this.GBox1.TabIndex = 0;
            this.GBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TxtCriteria);
            this.panel1.Location = new System.Drawing.Point(323, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(272, 32);
            this.panel1.TabIndex = 0;
            // 
            // TxtCriteria
            // 
            this.TxtCriteria.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtCriteria.Location = new System.Drawing.Point(3, 6);
            this.TxtCriteria.Name = "TxtCriteria";
            this.TxtCriteria.Size = new System.Drawing.Size(264, 21);
            this.TxtCriteria.TabIndex = 0;
            this.TxtCriteria.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtCriteria_KeyPress);
            this.TxtCriteria.TextChanged += new System.EventHandler(this.TxtCriteria_TextChanged);
            this.TxtCriteria.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtCriteria_KeyDown);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.DtTo);
            this.panel2.Controls.Add(this.DtFrom);
            this.panel2.Controls.Add(this.TxtFrom);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.TxtTo);
            this.panel2.Location = new System.Drawing.Point(323, 14);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(269, 32);
            this.panel2.TabIndex = 1;
            this.panel2.Visible = false;
            // 
            // DtTo
            // 
            this.DtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtTo.Location = new System.Drawing.Point(154, 6);
            this.DtTo.Name = "DtTo";
            this.DtTo.Size = new System.Drawing.Size(110, 21);
            this.DtTo.TabIndex = 1;
            this.DtTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DtTo_KeyDown);
            // 
            // DtFrom
            // 
            this.DtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtFrom.Location = new System.Drawing.Point(3, 6);
            this.DtFrom.Name = "DtFrom";
            this.DtFrom.Size = new System.Drawing.Size(110, 21);
            this.DtFrom.TabIndex = 0;
            this.DtFrom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DtFrom_KeyDown);
            // 
            // TxtFrom
            // 
            this.TxtFrom.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtFrom.Location = new System.Drawing.Point(3, 6);
            this.TxtFrom.Name = "TxtFrom";
            this.TxtFrom.Size = new System.Drawing.Size(110, 21);
            this.TxtFrom.TabIndex = 4;
            this.TxtFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtFrom_KeyPress);
            this.TxtFrom.TextChanged += new System.EventHandler(this.TxtFrom_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(128, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "-";
            // 
            // TxtTo
            // 
            this.TxtTo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtTo.Location = new System.Drawing.Point(154, 6);
            this.TxtTo.Name = "TxtTo";
            this.TxtTo.Size = new System.Drawing.Size(110, 21);
            this.TxtTo.TabIndex = 5;
            this.TxtTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtTo_KeyPress);
            this.TxtTo.TextChanged += new System.EventHandler(this.TxtTo_TextChanged);
            // 
            // CmbCondition
            // 
            this.CmbCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbCondition.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbCondition.FormattingEnabled = true;
            this.CmbCondition.Location = new System.Drawing.Point(179, 19);
            this.CmbCondition.Name = "CmbCondition";
            this.CmbCondition.Size = new System.Drawing.Size(135, 21);
            this.CmbCondition.TabIndex = 3;
            this.CmbCondition.SelectedIndexChanged += new System.EventHandler(this.CmbCondition_SelectedIndexChanged);
            // 
            // CmbFilter
            // 
            this.CmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbFilter.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbFilter.FormattingEnabled = true;
            this.CmbFilter.Location = new System.Drawing.Point(6, 19);
            this.CmbFilter.Name = "CmbFilter";
            this.CmbFilter.Size = new System.Drawing.Size(167, 21);
            this.CmbFilter.TabIndex = 2;
            this.CmbFilter.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGridView1.Location = new System.Drawing.Point(6, 52);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView1.Size = new System.Drawing.Size(587, 285);
            this.dataGridView1.StandardTab = true;
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            this.dataGridView1.GotFocus += new System.EventHandler(this.dataGridView1_GotFocus);
            this.dataGridView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyUp);
            this.dataGridView1.LostFocus += new System.EventHandler(this.dataGridView1_LostFocus);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_Click);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // GBox2
            // 
            this.GBox2.Controls.Add(this.label4);
            this.GBox2.Controls.Add(this.label3);
            this.GBox2.Controls.Add(this.TxtCST);
            this.GBox2.Controls.Add(this.TxtTin);
            this.GBox2.Controls.Add(this.label2);
            this.GBox2.Controls.Add(this.TxtAddress);
            this.GBox2.Location = new System.Drawing.Point(4, -2);
            this.GBox2.Name = "GBox2";
            this.GBox2.Size = new System.Drawing.Size(428, 123);
            this.GBox2.TabIndex = 3;
            this.GBox2.TabStop = false;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "label4";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 19);
            this.label3.TabIndex = 4;
            this.label3.Text = "label3";
            // 
            // TxtCST
            // 
            this.TxtCST.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtCST.Location = new System.Drawing.Point(117, 94);
            this.TxtCST.Name = "TxtCST";
            this.TxtCST.Size = new System.Drawing.Size(305, 21);
            this.TxtCST.TabIndex = 3;
            this.TxtCST.TabStop = false;
            this.TxtCST.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtCST_KeyPress);
            // 
            // TxtTin
            // 
            this.TxtTin.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtTin.Location = new System.Drawing.Point(117, 71);
            this.TxtTin.Name = "TxtTin";
            this.TxtTin.Size = new System.Drawing.Size(305, 21);
            this.TxtTin.TabIndex = 2;
            this.TxtTin.TabStop = false;
            this.TxtTin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtTin_KeyPress);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "label2";
            // 
            // TxtAddress
            // 
            this.TxtAddress.BackColor = System.Drawing.SystemColors.Window;
            this.TxtAddress.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtAddress.Location = new System.Drawing.Point(117, 14);
            this.TxtAddress.Multiline = true;
            this.TxtAddress.Name = "TxtAddress";
            this.TxtAddress.Size = new System.Drawing.Size(305, 53);
            this.TxtAddress.TabIndex = 0;
            this.TxtAddress.TabStop = false;
            this.TxtAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtAddress_KeyPress);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripLabel1,
            this.toolStripLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 469);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(609, 23);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ToolStripLabel1
            // 
            this.ToolStripLabel1.AutoSize = false;
            this.ToolStripLabel1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToolStripLabel1.Name = "ToolStripLabel1";
            this.ToolStripLabel1.Size = new System.Drawing.Size(590, 18);
            this.ToolStripLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.AutoSize = false;
            this.toolStripLabel2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(109, 18);
            this.toolStripLabel2.Text = "toolStripStatusLabel2";
            // 
            // FrmSelectionToolItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Wheat;
            this.ClientSize = new System.Drawing.Size(609, 492);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.GBox1);
            this.Controls.Add(this.GBox2);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSelectionToolItem";
            this.Text = "Form1";
            this.TransparencyKey = System.Drawing.Color.DarkRed;
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmSelectionToolItem_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.GBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.GBox2.ResumeLayout(false);
            this.GBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.GroupBox GBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox CmbFilter;
        private System.Windows.Forms.ComboBox CmbCondition;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtTo;
        private System.Windows.Forms.TextBox TxtFrom;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox TxtCriteria;
        private System.Windows.Forms.DateTimePicker DtTo;
        private System.Windows.Forms.DateTimePicker DtFrom;
        private System.Windows.Forms.GroupBox GBox2;
        private System.Windows.Forms.TextBox TxtAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtCST;
        private System.Windows.Forms.TextBox TxtTin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripLabel2;


    }
}

