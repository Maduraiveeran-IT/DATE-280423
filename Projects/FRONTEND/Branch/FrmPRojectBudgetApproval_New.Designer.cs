namespace Accounts
{
    partial class FrmPRojectBudgetApproval_New
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPRojectBudgetApproval_New));
            this.GBMain = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TabOcnList = new System.Windows.Forms.TabPage();
            this.button6 = new System.Windows.Forms.Button();
            this.Grid = new System.Windows.Forms.DataGridView();
            this.TxtSelectedOrders = new V_Components.MyTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtOrders = new V_Components.MyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Arrow3 = new System.Windows.Forms.PictureBox();
            this.TxtBuyer = new V_Components.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TabDetails = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.Grid_Budget_PR = new DotnetVFGrid.MyDataGridView();
            this.Grid_OCN_list = new DotnetVFGrid.MyDataGridView();
            this.TabAverage = new System.Windows.Forms.TabPage();
            this.button8 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.Grid_Final = new DotnetVFGrid.MyDataGridView();
            this.Grid_Sum = new DotnetVFGrid.MyDataGridView();
            this.GBMain.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.TabOcnList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).BeginInit();
            this.TabDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Budget_PR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_OCN_list)).BeginInit();
            this.TabAverage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Final)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Sum)).BeginInit();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.BackColor = System.Drawing.Color.LightSalmon;
            this.GBMain.Controls.Add(this.tabControl1);
            this.GBMain.Location = new System.Drawing.Point(7, 5);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(884, 521);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            this.GBMain.Enter += new System.EventHandler(this.GBMain_Enter);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TabOcnList);
            this.tabControl1.Controls.Add(this.TabDetails);
            this.tabControl1.Controls.Add(this.TabAverage);
            this.tabControl1.Location = new System.Drawing.Point(5, 14);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(873, 500);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // TabOcnList
            // 
            this.TabOcnList.BackColor = System.Drawing.Color.Transparent;
            this.TabOcnList.Controls.Add(this.button6);
            this.TabOcnList.Controls.Add(this.Grid);
            this.TabOcnList.Controls.Add(this.TxtSelectedOrders);
            this.TabOcnList.Controls.Add(this.label3);
            this.TabOcnList.Controls.Add(this.TxtOrders);
            this.TabOcnList.Controls.Add(this.label2);
            this.TabOcnList.Controls.Add(this.checkBox1);
            this.TabOcnList.Controls.Add(this.button2);
            this.TabOcnList.Controls.Add(this.button1);
            this.TabOcnList.Controls.Add(this.Arrow3);
            this.TabOcnList.Controls.Add(this.TxtBuyer);
            this.TabOcnList.Controls.Add(this.label1);
            this.TabOcnList.Location = new System.Drawing.Point(4, 22);
            this.TabOcnList.Name = "TabOcnList";
            this.TabOcnList.Padding = new System.Windows.Forms.Padding(3);
            this.TabOcnList.Size = new System.Drawing.Size(865, 474);
            this.TabOcnList.TabIndex = 0;
            this.TabOcnList.Text = "SELECT OCN";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(15, 431);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(122, 31);
            this.button6.TabIndex = 39;
            this.button6.Text = "&CLEAR";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(15, 56);
            this.Grid.Margin = new System.Windows.Forms.Padding(4);
            this.Grid.Name = "Grid";
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid.Size = new System.Drawing.Size(837, 361);
            this.Grid.TabIndex = 38;
            this.Grid.Tag = "BUDGET";
            this.Grid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellClick);
            this.Grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellContentClick);
            // 
            // TxtSelectedOrders
            // 
            this.TxtSelectedOrders.BackColor = System.Drawing.Color.White;
            this.TxtSelectedOrders.Location = new System.Drawing.Point(316, 366);
            this.TxtSelectedOrders.Name = "TxtSelectedOrders";
            this.TxtSelectedOrders.Size = new System.Drawing.Size(96, 21);
            this.TxtSelectedOrders.TabIndex = 37;
            this.TxtSelectedOrders.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtSelectedOrders.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(178, 366);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "SELECTED ORDERS";
            this.label3.Visible = false;
            // 
            // TxtOrders
            // 
            this.TxtOrders.BackColor = System.Drawing.Color.White;
            this.TxtOrders.Location = new System.Drawing.Point(756, 23);
            this.TxtOrders.Name = "TxtOrders";
            this.TxtOrders.Size = new System.Drawing.Size(96, 21);
            this.TxtOrders.TabIndex = 35;
            this.TxtOrders.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(684, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "ORDERS";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(513, 24);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(94, 17);
            this.checkBox1.TabIndex = 33;
            this.checkBox1.Text = "&SELECT ALL";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(730, 431);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(122, 31);
            this.button2.TabIndex = 3;
            this.button2.Text = "E&XIT";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(603, 431);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 31);
            this.button1.TabIndex = 2;
            this.button1.Text = "&VIEW BUDGET";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Arrow3
            // 
            this.Arrow3.Image = ((System.Drawing.Image)(resources.GetObject("Arrow3.Image")));
            this.Arrow3.Location = new System.Drawing.Point(471, 20);
            this.Arrow3.Name = "Arrow3";
            this.Arrow3.Size = new System.Drawing.Size(25, 21);
            this.Arrow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Arrow3.TabIndex = 32;
            this.Arrow3.TabStop = false;
            // 
            // TxtBuyer
            // 
            this.TxtBuyer.BackColor = System.Drawing.Color.White;
            this.TxtBuyer.Location = new System.Drawing.Point(83, 20);
            this.TxtBuyer.Name = "TxtBuyer";
            this.TxtBuyer.Size = new System.Drawing.Size(382, 21);
            this.TxtBuyer.TabIndex = 0;
            this.TxtBuyer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "BUYER";
            // 
            // TabDetails
            // 
            this.TabDetails.BackColor = System.Drawing.Color.Transparent;
            this.TabDetails.Controls.Add(this.label7);
            this.TabDetails.Controls.Add(this.label6);
            this.TabDetails.Controls.Add(this.label4);
            this.TabDetails.Controls.Add(this.button5);
            this.TabDetails.Controls.Add(this.button3);
            this.TabDetails.Controls.Add(this.Grid_Budget_PR);
            this.TabDetails.Controls.Add(this.Grid_OCN_list);
            this.TabDetails.Location = new System.Drawing.Point(4, 22);
            this.TabDetails.Name = "TabDetails";
            this.TabDetails.Padding = new System.Windows.Forms.Padding(3);
            this.TabDetails.Size = new System.Drawing.Size(865, 474);
            this.TabDetails.TabIndex = 1;
            this.TabDetails.Text = "BUDGET";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label7.Location = new System.Drawing.Point(6, 444);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(423, 17);
            this.label7.TabIndex = 41;
            this.label7.Text = "** in SIZE - Some difference on that row.";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(435, 435);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 31);
            this.label6.TabIndex = 40;
            this.label6.Text = "QTY :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(511, 435);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 31);
            this.label4.TabIndex = 39;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(621, 435);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(116, 31);
            this.button5.TabIndex = 38;
            this.button5.Text = "&BACK";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(743, 435);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(116, 31);
            this.button3.TabIndex = 35;
            this.button3.Text = "E&XIT";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Grid_Budget_PR
            // 
            this.Grid_Budget_PR.AllowUserToAddRows = false;
            this.Grid_Budget_PR.AllowUserToDeleteRows = false;
            this.Grid_Budget_PR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid_Budget_PR.Location = new System.Drawing.Point(9, 10);
            this.Grid_Budget_PR.Name = "Grid_Budget_PR";
            this.Grid_Budget_PR.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Grid_Budget_PR.Size = new System.Drawing.Size(850, 415);
            this.Grid_Budget_PR.TabIndex = 34;
            this.Grid_Budget_PR.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_Budget_CellClick);
            this.Grid_Budget_PR.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Grid_Budget_EditingControlShowing);
            this.Grid_Budget_PR.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Grid_Budget_MouseClick);
            // 
            // Grid_OCN_list
            // 
            this.Grid_OCN_list.AllowUserToAddRows = false;
            this.Grid_OCN_list.AllowUserToDeleteRows = false;
            this.Grid_OCN_list.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid_OCN_list.Location = new System.Drawing.Point(73, 151);
            this.Grid_OCN_list.Name = "Grid_OCN_list";
            this.Grid_OCN_list.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid_OCN_list.Size = new System.Drawing.Size(585, 156);
            this.Grid_OCN_list.TabIndex = 37;
            // 
            // TabAverage
            // 
            this.TabAverage.BackColor = System.Drawing.Color.Transparent;
            this.TabAverage.Controls.Add(this.button8);
            this.TabAverage.Controls.Add(this.button4);
            this.TabAverage.Controls.Add(this.button7);
            this.TabAverage.Controls.Add(this.Grid_Final);
            this.TabAverage.Controls.Add(this.Grid_Sum);
            this.TabAverage.Location = new System.Drawing.Point(4, 22);
            this.TabAverage.Name = "TabAverage";
            this.TabAverage.Size = new System.Drawing.Size(865, 474);
            this.TabAverage.TabIndex = 2;
            this.TabAverage.Text = "SUMMARY";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(596, 303);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(86, 31);
            this.button8.TabIndex = 44;
            this.button8.Text = "&SAVE";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Visible = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(688, 303);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(77, 31);
            this.button4.TabIndex = 43;
            this.button4.Text = "&BACK";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(771, 303);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(77, 31);
            this.button7.TabIndex = 38;
            this.button7.Text = "E&XIT";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // Grid_Final
            // 
            this.Grid_Final.AllowUserToAddRows = false;
            this.Grid_Final.AllowUserToDeleteRows = false;
            this.Grid_Final.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid_Final.Location = new System.Drawing.Point(15, 340);
            this.Grid_Final.Name = "Grid_Final";
            this.Grid_Final.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid_Final.Size = new System.Drawing.Size(833, 130);
            this.Grid_Final.TabIndex = 40;
            // 
            // Grid_Sum
            // 
            this.Grid_Sum.AllowUserToAddRows = false;
            this.Grid_Sum.AllowUserToDeleteRows = false;
            this.Grid_Sum.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid_Sum.Location = new System.Drawing.Point(15, 9);
            this.Grid_Sum.Name = "Grid_Sum";
            this.Grid_Sum.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid_Sum.Size = new System.Drawing.Size(575, 325);
            this.Grid_Sum.TabIndex = 0;
            // 
            // FrmPRojectBudgetApproval_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 530);
            this.Controls.Add(this.GBMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "FrmPRojectBudgetApproval_New";
            this.Text = "BUDGET APPROVAL";
            this.Load += new System.EventHandler(this.FrmPRojectBudgetApproval_New_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmPRojectBudgetApproval_New_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmPRojectBudgetApproval_New_KeyPress);
            this.GBMain.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.TabOcnList.ResumeLayout(false);
            this.TabOcnList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Arrow3)).EndInit();
            this.TabDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Budget_PR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_OCN_list)).EndInit();
            this.TabAverage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Final)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Sum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TabOcnList;
        private System.Windows.Forms.TabPage TabDetails;
        private System.Windows.Forms.TabPage TabAverage;
        private V_Components.MyTextBox TxtBuyer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox Arrow3;
        private System.Windows.Forms.Button button3;
        private DotnetVFGrid.MyDataGridView Grid_Budget_PR;
        private System.Windows.Forms.CheckBox checkBox1;
        private V_Components.MyTextBox TxtOrders;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView Grid;
        private V_Components.MyTextBox TxtSelectedOrders;
        private System.Windows.Forms.Label label3;
        private DotnetVFGrid.MyDataGridView Grid_OCN_list;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button6;
        private DotnetVFGrid.MyDataGridView Grid_Sum;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private DotnetVFGrid.MyDataGridView Grid_Final;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button8;
    }
}