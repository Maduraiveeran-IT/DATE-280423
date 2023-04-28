namespace Accounts
{
    partial class Frm_Projects_Permission_Master
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
            this.GBMain = new System.Windows.Forms.Panel();
            this.butCollapseAll = new System.Windows.Forms.Button();
            this.butExpandAll = new System.Windows.Forms.Button();
            this.Txt_Rights = new V_Components.MyTextBox();
            this.Chk_SelectAll = new System.Windows.Forms.CheckBox();
            this.PNL_Rights = new System.Windows.Forms.Panel();
            this.Chk_Report = new System.Windows.Forms.CheckBox();
            this.Chk_Print = new System.Windows.Forms.CheckBox();
            this.Chk_View = new System.Windows.Forms.CheckBox();
            this.Chk_Delete = new System.Windows.Forms.CheckBox();
            this.Chk_Edit = new System.Windows.Forms.CheckBox();
            this.Chk_Add = new System.Windows.Forms.CheckBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.Txt_UserName = new V_Components.MyTextBox();
            this.Txt_Description = new V_Components.MyTextBox();
            this.Chk_Cancel = new System.Windows.Forms.CheckBox();
            this.Lbl_Description = new System.Windows.Forms.Label();
            this.Lbl_Menu = new System.Windows.Forms.Label();
            this.Lbl_UserName = new System.Windows.Forms.Label();
            this.GBMain.SuspendLayout();
            this.PNL_Rights.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.butCollapseAll);
            this.GBMain.Controls.Add(this.butExpandAll);
            this.GBMain.Controls.Add(this.Txt_Rights);
            this.GBMain.Controls.Add(this.Chk_SelectAll);
            this.GBMain.Controls.Add(this.PNL_Rights);
            this.GBMain.Controls.Add(this.treeView1);
            this.GBMain.Controls.Add(this.Txt_UserName);
            this.GBMain.Controls.Add(this.Txt_Description);
            this.GBMain.Controls.Add(this.Chk_Cancel);
            this.GBMain.Controls.Add(this.Lbl_Description);
            this.GBMain.Controls.Add(this.Lbl_Menu);
            this.GBMain.Controls.Add(this.Lbl_UserName);
            this.GBMain.Location = new System.Drawing.Point(2, 2);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(540, 515);
            this.GBMain.TabIndex = 2;
            // 
            // butCollapseAll
            // 
            this.butCollapseAll.Location = new System.Drawing.Point(432, 352);
            this.butCollapseAll.Name = "butCollapseAll";
            this.butCollapseAll.Size = new System.Drawing.Size(98, 23);
            this.butCollapseAll.TabIndex = 61;
            this.butCollapseAll.Text = "Collapse All";
            this.butCollapseAll.UseVisualStyleBackColor = true;
            this.butCollapseAll.Click += new System.EventHandler(this.butCollapseAll_Click);
            // 
            // butExpandAll
            // 
            this.butExpandAll.Location = new System.Drawing.Point(432, 381);
            this.butExpandAll.Name = "butExpandAll";
            this.butExpandAll.Size = new System.Drawing.Size(98, 23);
            this.butExpandAll.TabIndex = 61;
            this.butExpandAll.Text = "Expand All";
            this.butExpandAll.UseVisualStyleBackColor = true;
            this.butExpandAll.Click += new System.EventHandler(this.butExpandAll_Click);
            // 
            // Txt_Rights
            // 
            this.Txt_Rights.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.Txt_Rights.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txt_Rights.ForeColor = System.Drawing.SystemColors.Window;
            this.Txt_Rights.Location = new System.Drawing.Point(432, 73);
            this.Txt_Rights.MaxLength = 50;
            this.Txt_Rights.Name = "Txt_Rights";
            this.Txt_Rights.ReadOnly = true;
            this.Txt_Rights.Size = new System.Drawing.Size(98, 22);
            this.Txt_Rights.TabIndex = 60;
            this.Txt_Rights.Text = "Rights";
            // 
            // Chk_SelectAll
            // 
            this.Chk_SelectAll.AutoSize = true;
            this.Chk_SelectAll.Location = new System.Drawing.Point(432, 50);
            this.Chk_SelectAll.Name = "Chk_SelectAll";
            this.Chk_SelectAll.Size = new System.Drawing.Size(79, 17);
            this.Chk_SelectAll.TabIndex = 1;
            this.Chk_SelectAll.Text = "Select All";
            this.Chk_SelectAll.UseVisualStyleBackColor = true;
            this.Chk_SelectAll.CheckedChanged += new System.EventHandler(this.Chk_SelectAll_CheckedChanged);
            // 
            // PNL_Rights
            // 
            this.PNL_Rights.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_Rights.Controls.Add(this.Chk_Report);
            this.PNL_Rights.Controls.Add(this.Chk_Print);
            this.PNL_Rights.Controls.Add(this.Chk_View);
            this.PNL_Rights.Controls.Add(this.Chk_Delete);
            this.PNL_Rights.Controls.Add(this.Chk_Edit);
            this.PNL_Rights.Controls.Add(this.Chk_Add);
            this.PNL_Rights.Location = new System.Drawing.Point(432, 100);
            this.PNL_Rights.Name = "PNL_Rights";
            this.PNL_Rights.Size = new System.Drawing.Size(98, 144);
            this.PNL_Rights.TabIndex = 59;
            // 
            // Chk_Report
            // 
            this.Chk_Report.AutoSize = true;
            this.Chk_Report.Location = new System.Drawing.Point(5, 118);
            this.Chk_Report.Name = "Chk_Report";
            this.Chk_Report.Size = new System.Drawing.Size(64, 17);
            this.Chk_Report.TabIndex = 0;
            this.Chk_Report.Text = "&Report";
            this.Chk_Report.UseVisualStyleBackColor = true;
            this.Chk_Report.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Chk_Report_MouseClick);
            this.Chk_Report.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Chk_Report_KeyDown);
            // 
            // Chk_Print
            // 
            this.Chk_Print.AutoSize = true;
            this.Chk_Print.Location = new System.Drawing.Point(5, 96);
            this.Chk_Print.Name = "Chk_Print";
            this.Chk_Print.Size = new System.Drawing.Size(52, 17);
            this.Chk_Print.TabIndex = 0;
            this.Chk_Print.Text = "&Print";
            this.Chk_Print.UseVisualStyleBackColor = true;
            this.Chk_Print.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Chk_Print_MouseClick);
            this.Chk_Print.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Chk_Print_KeyDown);
            // 
            // Chk_View
            // 
            this.Chk_View.AutoSize = true;
            this.Chk_View.Location = new System.Drawing.Point(5, 74);
            this.Chk_View.Name = "Chk_View";
            this.Chk_View.Size = new System.Drawing.Size(53, 17);
            this.Chk_View.TabIndex = 0;
            this.Chk_View.Text = "&View";
            this.Chk_View.UseVisualStyleBackColor = true;
            this.Chk_View.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Chk_View_MouseClick);
            this.Chk_View.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Chk_View_KeyDown);
            // 
            // Chk_Delete
            // 
            this.Chk_Delete.AutoSize = true;
            this.Chk_Delete.Location = new System.Drawing.Point(5, 52);
            this.Chk_Delete.Name = "Chk_Delete";
            this.Chk_Delete.Size = new System.Drawing.Size(63, 17);
            this.Chk_Delete.TabIndex = 0;
            this.Chk_Delete.Text = "&Delete";
            this.Chk_Delete.UseVisualStyleBackColor = true;
            this.Chk_Delete.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Chk_Delete_MouseClick);
            this.Chk_Delete.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Chk_Delete_KeyDown);
            // 
            // Chk_Edit
            // 
            this.Chk_Edit.AutoSize = true;
            this.Chk_Edit.Location = new System.Drawing.Point(5, 30);
            this.Chk_Edit.Name = "Chk_Edit";
            this.Chk_Edit.Size = new System.Drawing.Size(47, 17);
            this.Chk_Edit.TabIndex = 0;
            this.Chk_Edit.Text = "&Edit";
            this.Chk_Edit.UseVisualStyleBackColor = true;
            this.Chk_Edit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Chk_Edit_MouseClick);
            this.Chk_Edit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Chk_Edit_KeyDown);
            // 
            // Chk_Add
            // 
            this.Chk_Add.AutoSize = true;
            this.Chk_Add.Location = new System.Drawing.Point(5, 8);
            this.Chk_Add.Name = "Chk_Add";
            this.Chk_Add.Size = new System.Drawing.Size(48, 17);
            this.Chk_Add.TabIndex = 0;
            this.Chk_Add.Text = "&Add";
            this.Chk_Add.UseVisualStyleBackColor = true;
            this.Chk_Add.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Chk_Add_MouseClick);
            this.Chk_Add.CheckedChanged += new System.EventHandler(this.Chk_Add_CheckedChanged);
            this.Chk_Add.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Chk_Add_KeyDown);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(127, 50);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(299, 354);
            this.treeView1.TabIndex = 58;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.Leave += new System.EventHandler(this.treeView1_Leave);
            this.treeView1.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeCheck);
            this.treeView1.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeSelect);
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            this.treeView1.Click += new System.EventHandler(this.treeView1_Click);
            // 
            // Txt_UserName
            // 
            this.Txt_UserName.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Txt_UserName.Location = new System.Drawing.Point(127, 10);
            this.Txt_UserName.MaxLength = 50;
            this.Txt_UserName.Name = "Txt_UserName";
            this.Txt_UserName.ReadOnly = true;
            this.Txt_UserName.Size = new System.Drawing.Size(299, 21);
            this.Txt_UserName.TabIndex = 57;
            this.Txt_UserName.TextChanged += new System.EventHandler(this.Txt_UserName_TextChanged);
            // 
            // Txt_Description
            // 
            this.Txt_Description.Location = new System.Drawing.Point(127, 420);
            this.Txt_Description.MaxLength = 250;
            this.Txt_Description.Multiline = true;
            this.Txt_Description.Name = "Txt_Description";
            this.Txt_Description.Size = new System.Drawing.Size(410, 63);
            this.Txt_Description.TabIndex = 7;
            // 
            // Chk_Cancel
            // 
            this.Chk_Cancel.AutoSize = true;
            this.Chk_Cancel.Location = new System.Drawing.Point(472, 489);
            this.Chk_Cancel.Name = "Chk_Cancel";
            this.Chk_Cancel.Size = new System.Drawing.Size(65, 17);
            this.Chk_Cancel.TabIndex = 9;
            this.Chk_Cancel.Text = "Cancel";
            this.Chk_Cancel.UseVisualStyleBackColor = true;
            // 
            // Lbl_Description
            // 
            this.Lbl_Description.AutoSize = true;
            this.Lbl_Description.Location = new System.Drawing.Point(6, 420);
            this.Lbl_Description.Name = "Lbl_Description";
            this.Lbl_Description.Size = new System.Drawing.Size(71, 13);
            this.Lbl_Description.TabIndex = 56;
            this.Lbl_Description.Text = "Description";
            // 
            // Lbl_Menu
            // 
            this.Lbl_Menu.AutoSize = true;
            this.Lbl_Menu.Location = new System.Drawing.Point(7, 50);
            this.Lbl_Menu.Name = "Lbl_Menu";
            this.Lbl_Menu.Size = new System.Drawing.Size(37, 13);
            this.Lbl_Menu.TabIndex = 0;
            this.Lbl_Menu.Text = "Menu";
            // 
            // Lbl_UserName
            // 
            this.Lbl_UserName.AutoSize = true;
            this.Lbl_UserName.Location = new System.Drawing.Point(4, 9);
            this.Lbl_UserName.Name = "Lbl_UserName";
            this.Lbl_UserName.Size = new System.Drawing.Size(70, 13);
            this.Lbl_UserName.TabIndex = 0;
            this.Lbl_UserName.Text = "User Name";
            // 
            // Frm_Socks_Permission_Master
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 518);
            this.Controls.Add(this.GBMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_Projects_Permission_Master";
            this.Text = "Permission ";
            this.Load += new System.EventHandler(this.Frm_Projects_Permission_Master_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Frm_Projects_Permission_Master_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Projects_Permission_Master_KeyDown);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.PNL_Rights.ResumeLayout(false);
            this.PNL_Rights.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel GBMain;
        private V_Components.MyTextBox Txt_Description;
        private System.Windows.Forms.CheckBox Chk_Cancel;
        private System.Windows.Forms.Label Lbl_Description;
        private System.Windows.Forms.Label Lbl_UserName;
        private V_Components.MyTextBox Txt_UserName;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label Lbl_Menu;
        private System.Windows.Forms.Panel PNL_Rights;
        private System.Windows.Forms.CheckBox Chk_Report;
        private System.Windows.Forms.CheckBox Chk_Print;
        private System.Windows.Forms.CheckBox Chk_Delete;
        private System.Windows.Forms.CheckBox Chk_Edit;
        private System.Windows.Forms.CheckBox Chk_Add;
        private System.Windows.Forms.CheckBox Chk_SelectAll;
        private System.Windows.Forms.CheckBox Chk_View;
        private V_Components.MyTextBox Txt_Rights;
        private System.Windows.Forms.Button butCollapseAll;
        private System.Windows.Forms.Button butExpandAll;
    }
}