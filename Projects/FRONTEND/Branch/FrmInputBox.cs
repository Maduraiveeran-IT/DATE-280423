using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmInputBox : Form
    {
        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        public Int32 InputMode = 0;
        public String Result = null;

        public FrmInputBox()
        {
            InitializeComponent();
        }
        
        public FrmInputBox(String Title, String Caption)
        {
            InitializeComponent();
            this.Text = Title;
            LblCaption.Text = Caption + " : ";
        }

        private void FrmInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    BtnOk_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmInputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtInput")
                {
                    if (InputMode == 0)
                    {
                        MyBase.Valid_Number(TxtInput, e);
                    }
                    else if (InputMode == 1)
                    {
                        MyBase.Valid_Decimal(TxtInput, e);
                    }
                    else if (InputMode == 2)
                    {
                        MyBase.Valid_Yes_OR_No(TxtInput, e);
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Result = TxtInput.Text;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Result = null;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void FrmInputBox_Load(object sender, EventArgs e)
        {
            TxtInput.ContextMenu = new ContextMenu();
            if (InputMode == 4)
            {
                TxtInput.PasswordChar = '*';
            }
        }
    }
}