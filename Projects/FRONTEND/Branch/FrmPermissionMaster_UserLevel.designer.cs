namespace Accounts
{
    partial class FrmPermissionMaster_User_Level_Fixed
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
            this.ChkPreview = new System.Windows.Forms.CheckBox();
            this.ChkCopy = new System.Windows.Forms.CheckBox();
            this.ChkView = new System.Windows.Forms.CheckBox();
            this.ChkDelete = new System.Windows.Forms.CheckBox();
            this.ChkPrint = new System.Windows.Forms.CheckBox();
            this.ChkEdit = new System.Windows.Forms.CheckBox();
            this.ChkNew = new System.Windows.Forms.CheckBox();
            this.ButExit = new System.Windows.Forms.Button();
            this.ButClear = new System.Windows.Forms.Button();
            this.ButReset = new System.Windows.Forms.Button();
            this.ButSave = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtCopyFrom = new V_Components.MyTextBox();
            this.TxtBankName = new V_Components.MyTextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.GBMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBMain
            // 
            this.GBMain.Controls.Add(this.checkBox1);
            this.GBMain.Controls.Add(this.treeView1);
            this.GBMain.Controls.Add(this.ChkPreview);
            this.GBMain.Controls.Add(this.ChkCopy);
            this.GBMain.Controls.Add(this.ChkView);
            this.GBMain.Controls.Add(this.ChkDelete);
            this.GBMain.Controls.Add(this.ChkPrint);
            this.GBMain.Controls.Add(this.ChkEdit);
            this.GBMain.Controls.Add(this.ChkNew);
            this.GBMain.Controls.Add(this.ButExit);
            this.GBMain.Controls.Add(this.ButClear);
            this.GBMain.Controls.Add(this.ButReset);
            this.GBMain.Controls.Add(this.ButSave);
            this.GBMain.Controls.Add(this.label1);
            this.GBMain.Controls.Add(this.TxtCopyFrom);
            this.GBMain.Controls.Add(this.TxtBankName);
            this.GBMain.Location = new System.Drawing.Point(12, 12);
            this.GBMain.Name = "GBMain";
            this.GBMain.Size = new System.Drawing.Size(593, 499);
            this.GBMain.TabIndex = 0;
            this.GBMain.TabStop = false;
            this.GBMain.Text = "Permission Details";
            // 
            // ChkPreview
            // 
            this.ChkPreview.AutoSize = true;
            this.ChkPreview.Location = new System.Drawing.Point(313, 259);
            this.ChkPreview.Name = "ChkPreview";
            this.ChkPreview.Size = new System.Drawing.Size(71, 17);
            this.ChkPreview.TabIndex = 4;
            this.ChkPreview.Text = "Preview";
            this.ChkPreview.UseVisualStyleBackColor = true;
            this.ChkPreview.Visible = false;
            // 
            // ChkCopy
            // 
            this.ChkCopy.AutoSize = true;
            this.ChkCopy.Location = new System.Drawing.Point(149, 172);
            this.ChkCopy.Name = "ChkCopy";
            this.ChkCopy.Size = new System.Drawing.Size(98, 17);
            this.ChkCopy.TabIndex = 4;
            this.ChkCopy.Text = "&Copy From :";
            this.ChkCopy.UseVisualStyleBackColor = true;
            this.ChkCopy.Visible = false;
            this.ChkCopy.CheckedChanged += new System.EventHandler(this.ChkCopy_CheckedChanged);
            // 
            // ChkView
            // 
            this.ChkView.AutoSize = true;
            this.ChkView.Location = new System.Drawing.Point(254, 259);
            this.ChkView.Name = "ChkView";
            this.ChkView.Size = new System.Drawing.Size(53, 17);
            this.ChkView.TabIndex = 4;
            this.ChkView.Text = "View";
            this.ChkView.UseVisualStyleBackColor = true;
            this.ChkView.Visible = false;
            // 
            // ChkDelete
            // 
            this.ChkDelete.AutoSize = true;
            this.ChkDelete.Location = new System.Drawing.Point(406, 234);
            this.ChkDelete.Name = "ChkDelete";
            this.ChkDelete.Size = new System.Drawing.Size(63, 17);
            this.ChkDelete.TabIndex = 4;
            this.ChkDelete.Text = "Delete";
            this.ChkDelete.UseVisualStyleBackColor = true;
            this.ChkDelete.Visible = false;
            // 
            // ChkPrint
            // 
            this.ChkPrint.AutoSize = true;
            this.ChkPrint.Location = new System.Drawing.Point(406, 259);
            this.ChkPrint.Name = "ChkPrint";
            this.ChkPrint.Size = new System.Drawing.Size(52, 17);
            this.ChkPrint.TabIndex = 4;
            this.ChkPrint.Text = "Print";
            this.ChkPrint.UseVisualStyleBackColor = true;
            this.ChkPrint.Visible = false;
            // 
            // ChkEdit
            // 
            this.ChkEdit.AutoSize = true;
            this.ChkEdit.Location = new System.Drawing.Point(313, 234);
            this.ChkEdit.Name = "ChkEdit";
            this.ChkEdit.Size = new System.Drawing.Size(47, 17);
            this.ChkEdit.TabIndex = 4;
            this.ChkEdit.Text = "Edit";
            this.ChkEdit.UseVisualStyleBackColor = true;
            this.ChkEdit.Visible = false;
            // 
            // ChkNew
            // 
            this.ChkNew.AutoSize = true;
            this.ChkNew.Location = new System.Drawing.Point(254, 234);
            this.ChkNew.Name = "ChkNew";
            this.ChkNew.Size = new System.Drawing.Size(50, 17);
            this.ChkNew.TabIndex = 4;
            this.ChkNew.Text = "New";
            this.ChkNew.UseVisualStyleBackColor = true;
            this.ChkNew.Visible = false;
            // 
            // ButExit
            // 
            this.ButExit.Location = new System.Drawing.Point(505, 454);
            this.ButExit.Name = "ButExit";
            this.ButExit.Size = new System.Drawing.Size(75, 31);
            this.ButExit.TabIndex = 3;
            this.ButExit.Text = "E&xit";
            this.ButExit.UseVisualStyleBackColor = true;
            this.ButExit.Click += new System.EventHandler(this.ButExit_Click);
            // 
            // ButClear
            // 
            this.ButClear.Location = new System.Drawing.Point(424, 454);
            this.ButClear.Name = "ButClear";
            this.ButClear.Size = new System.Drawing.Size(75, 31);
            this.ButClear.TabIndex = 3;
            this.ButClear.Text = "C&lear";
            this.ButClear.UseVisualStyleBackColor = true;
            this.ButClear.Click += new System.EventHandler(this.ButClear_Click);
            // 
            // ButReset
            // 
            this.ButReset.Location = new System.Drawing.Point(18, 454);
            this.ButReset.Name = "ButReset";
            this.ButReset.Size = new System.Drawing.Size(75, 31);
            this.ButReset.TabIndex = 3;
            this.ButReset.Text = "&Reset";
            this.ButReset.UseVisualStyleBackColor = true;
            this.ButReset.Visible = false;
            this.ButReset.Click += new System.EventHandler(this.ButReset_Click);
            // 
            // ButSave
            // 
            this.ButSave.Location = new System.Drawing.Point(343, 454);
            this.ButSave.Name = "ButSave";
            this.ButSave.Size = new System.Drawing.Size(75, 31);
            this.ButSave.TabIndex = 3;
            this.ButSave.Text = "&Save";
            this.ButSave.UseVisualStyleBackColor = true;
            this.ButSave.Click += new System.EventHandler(this.ButSave_Click);
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Location = new System.Drawing.Point(18, 66);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(562, 377);
            this.treeView1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "User Level :";
            // 
            // TxtCopyFrom
            // 
            this.TxtCopyFrom.Enabled = false;
            this.TxtCopyFrom.Location = new System.Drawing.Point(268, 170);
            this.TxtCopyFrom.Name = "TxtCopyFrom";
            this.TxtCopyFrom.Size = new System.Drawing.Size(213, 21);
            this.TxtCopyFrom.TabIndex = 0;
            this.TxtCopyFrom.Visible = false;
            this.TxtCopyFrom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtCopyFrom_KeyDown);
            // 
            // TxtBankName
            // 
            this.TxtBankName.Location = new System.Drawing.Point(137, 31);
            this.TxtBankName.Name = "TxtBankName";
            this.TxtBankName.Size = new System.Drawing.Size(235, 21);
            this.TxtBankName.TabIndex = 0;
            this.TxtBankName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBankName_KeyPress_1);
            this.TxtBankName.TextChanged += new System.EventHandler(this.TxtBankName_TextChanged);
            this.TxtBankName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtBankName_KeyDown_1);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(492, 33);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(79, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Select All";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // FrmPermissionMaster_User_Level_Fixed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 521);
            this.Controls.Add(this.GBMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "FrmPermissionMaster_User_Level_Fixed";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Permission Master For User Level";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmBankMaster_KeyDown);
            this.Load += new System.EventHandler(this.FrmBankMaster_Load);
            this.GBMain.ResumeLayout(false);
            this.GBMain.PerformLayout();
            this.ResumeLayout(false);

        }

      #endregion

        private System.Windows.Forms.GroupBox GBMain;
        private System.Windows.Forms.TextBox TxtBankName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button ButExit;
        private System.Windows.Forms.Button ButClear;
        private System.Windows.Forms.Button ButSave;
        private System.Windows.Forms.CheckBox ChkNew;
        private System.Windows.Forms.CheckBox ChkEdit;
        private System.Windows.Forms.CheckBox ChkPreview;
        private System.Windows.Forms.CheckBox ChkDelete;
        private System.Windows.Forms.CheckBox ChkPrint;
        private System.Windows.Forms.Button ButReset;
        private System.Windows.Forms.TextBox TxtCopyFrom;
        private System.Windows.Forms.CheckBox ChkCopy;
        private System.Windows.Forms.CheckBox ChkView;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

