namespace SelectionTool
{
    partial class FrmSelectionTool
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
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnMinus = new System.Windows.Forms.Button();
            this.BtnPlus = new System.Windows.Forms.Button();
            this.cmblist = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.DtTo = new System.Windows.Forms.DateTimePicker();
            this.DtFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.CmbCondition = new System.Windows.Forms.ComboBox();
            this.CmbFilter = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.GBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripLabel3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.ascendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.descendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.TxtCriteria = new V_Components.MyTextBox();
            this.TxtFrom = new V_Components.MyTextBox();
            this.TxtTo = new V_Components.MyTextBox();
            this.TxtCST = new V_Components.MyTextBox();
            this.TxtTin = new V_Components.MyTextBox();
            this.TxtAddress = new V_Components.MyTextBox();
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
            this.GBox1.Controls.Add(this.button2);
            this.GBox1.Controls.Add(this.button1);
            this.GBox1.Controls.Add(this.BtnMinus);
            this.GBox1.Controls.Add(this.BtnPlus);
            this.GBox1.Controls.Add(this.cmblist);
            this.GBox1.Controls.Add(this.panel1);
            this.GBox1.Controls.Add(this.panel2);
            this.GBox1.Controls.Add(this.CmbCondition);
            this.GBox1.Controls.Add(this.CmbFilter);
            this.GBox1.Controls.Add(this.dataGridView1);
            this.GBox1.Location = new System.Drawing.Point(4, 122);
            this.GBox1.Name = "GBox1";
            this.GBox1.Size = new System.Drawing.Size(633, 344);
            this.GBox1.TabIndex = 0;
            this.GBox1.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(599, 49);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(30, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "RF";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(470, 51);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(62, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "=>";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnMinus
            // 
            this.BtnMinus.Location = new System.Drawing.Point(538, 51);
            this.BtnMinus.Name = "BtnMinus";
            this.BtnMinus.Size = new System.Drawing.Size(52, 23);
            this.BtnMinus.TabIndex = 8;
            this.BtnMinus.Text = "-";
            this.BtnMinus.UseVisualStyleBackColor = true;
            this.BtnMinus.Click += new System.EventHandler(this.BtnMinus_Click_1);
            // 
            // BtnPlus
            // 
            this.BtnPlus.Location = new System.Drawing.Point(599, 18);
            this.BtnPlus.Name = "BtnPlus";
            this.BtnPlus.Size = new System.Drawing.Size(30, 23);
            this.BtnPlus.TabIndex = 7;
            this.BtnPlus.Text = "+";
            this.BtnPlus.UseVisualStyleBackColor = true;
            this.BtnPlus.Click += new System.EventHandler(this.BtnPlus_Click);
            // 
            // cmblist
            // 
            this.cmblist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmblist.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmblist.FormattingEnabled = true;
            this.cmblist.Location = new System.Drawing.Point(6, 51);
            this.cmblist.Name = "cmblist";
            this.cmblist.Size = new System.Drawing.Size(458, 21);
            this.cmblist.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TxtCriteria);
            this.panel1.Location = new System.Drawing.Point(323, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(272, 32);
            this.panel1.TabIndex = 0;
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
            this.dataGridView1.Location = new System.Drawing.Point(6, 84);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(623, 253);
            this.dataGridView1.StandardTab = true;
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_Click);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.GotFocus += new System.EventHandler(this.dataGridView1_GotFocus);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            this.dataGridView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyUp);
            this.dataGridView1.LostFocus += new System.EventHandler(this.dataGridView1_LostFocus);
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
            this.label4.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "label4";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 19);
            this.label3.TabIndex = 4;
            this.label3.Text = "label3";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "label2";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripLabel1,
            this.ToolStripLabel3,
            this.toolStripLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 469);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(642, 23);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ToolStripLabel1
            // 
            this.ToolStripLabel1.AutoSize = false;
            this.ToolStripLabel1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToolStripLabel1.Name = "ToolStripLabel1";
            this.ToolStripLabel1.Size = new System.Drawing.Size(109, 18);
            this.ToolStripLabel1.Text = "toolStripStatusLabel1";
            // 
            // ToolStripLabel3
            // 
            this.ToolStripLabel3.AutoSize = false;
            this.ToolStripLabel3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ascendingToolStripMenuItem,
            this.descendingToolStripMenuItem});
            this.ToolStripLabel3.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToolStripLabel3.Name = "ToolStripLabel3";
            this.ToolStripLabel3.Size = new System.Drawing.Size(148, 21);
            this.ToolStripLabel3.Text = "toolStripDropDownButton1";
            this.ToolStripLabel3.ToolTipText = "List in Order";
            this.ToolStripLabel3.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ToolStripLabel3_DropDownItemClicked);
            // 
            // ascendingToolStripMenuItem
            // 
            this.ascendingToolStripMenuItem.Name = "ascendingToolStripMenuItem";
            this.ascendingToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.ascendingToolStripMenuItem.Text = "Ascending";
            // 
            // descendingToolStripMenuItem
            // 
            this.descendingToolStripMenuItem.Name = "descendingToolStripMenuItem";
            this.descendingToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.descendingToolStripMenuItem.Text = "Descending";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.AutoSize = false;
            this.toolStripLabel2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(109, 18);
            this.toolStripLabel2.Text = "toolStripStatusLabel2";
            // 
            // TxtCriteria
            // 
            this.TxtCriteria.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtCriteria.Location = new System.Drawing.Point(3, 6);
            this.TxtCriteria.Name = "TxtCriteria";
            this.TxtCriteria.Size = new System.Drawing.Size(264, 21);
            this.TxtCriteria.TabIndex = 0;
            this.TxtCriteria.TextChanged += new System.EventHandler(this.TxtCriteria_TextChanged);
            this.TxtCriteria.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtCriteria_KeyDown);
            this.TxtCriteria.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtCriteria_KeyPress);
            // 
            // TxtFrom
            // 
            this.TxtFrom.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtFrom.Location = new System.Drawing.Point(3, 6);
            this.TxtFrom.Name = "TxtFrom";
            this.TxtFrom.Size = new System.Drawing.Size(110, 21);
            this.TxtFrom.TabIndex = 4;
            this.TxtFrom.TextChanged += new System.EventHandler(this.TxtFrom_TextChanged);
            this.TxtFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtFrom_KeyPress);
            // 
            // TxtTo
            // 
            this.TxtTo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtTo.Location = new System.Drawing.Point(154, 6);
            this.TxtTo.Name = "TxtTo";
            this.TxtTo.Size = new System.Drawing.Size(110, 21);
            this.TxtTo.TabIndex = 5;
            this.TxtTo.TextChanged += new System.EventHandler(this.TxtTo_TextChanged);
            this.TxtTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtTo_KeyPress);
            // 
            // TxtCST
            // 
            this.TxtCST.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtCST.Location = new System.Drawing.Point(117, 94);
            this.TxtCST.Name = "TxtCST";
            this.TxtCST.Size = new System.Drawing.Size(305, 22);
            this.TxtCST.TabIndex = 3;
            this.TxtCST.TabStop = false;
            this.TxtCST.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtCST_KeyPress);
            // 
            // TxtTin
            // 
            this.TxtTin.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtTin.Location = new System.Drawing.Point(117, 71);
            this.TxtTin.Name = "TxtTin";
            this.TxtTin.Size = new System.Drawing.Size(305, 22);
            this.TxtTin.TabIndex = 2;
            this.TxtTin.TabStop = false;
            this.TxtTin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtTin_KeyPress);
            // 
            // TxtAddress
            // 
            this.TxtAddress.BackColor = System.Drawing.SystemColors.Window;
            this.TxtAddress.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtAddress.Location = new System.Drawing.Point(117, 14);
            this.TxtAddress.Multiline = true;
            this.TxtAddress.Name = "TxtAddress";
            this.TxtAddress.Size = new System.Drawing.Size(305, 53);
            this.TxtAddress.TabIndex = 0;
            this.TxtAddress.TabStop = false;
            this.TxtAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtAddress_KeyPress);
            // 
            // FrmSelectionTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Wheat;
            this.ClientSize = new System.Drawing.Size(642, 492);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.GBox1);
            this.Controls.Add(this.GBox2);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSelectionTool";
            this.Text = "Form1";
            this.TransparencyKey = System.Drawing.Color.DarkRed;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmSelectionTool_KeyPress);
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker DtTo;
        private System.Windows.Forms.DateTimePicker DtFrom;
        private System.Windows.Forms.GroupBox GBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripDropDownButton ToolStripLabel3;
        private System.Windows.Forms.ToolStripMenuItem ascendingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem descendingToolStripMenuItem;
        private System.Windows.Forms.ComboBox cmblist;
        private V_Components.MyTextBox TxtTo;
        private V_Components.MyTextBox TxtFrom;
        private V_Components.MyTextBox TxtCriteria;
        private V_Components.MyTextBox TxtAddress;
        private V_Components.MyTextBox TxtCST;
        private V_Components.MyTextBox TxtTin;
        private System.Windows.Forms.Button BtnMinus;
        private System.Windows.Forms.Button BtnPlus;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;


    }
}

