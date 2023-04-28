using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Accounts_ControlModules;
using System.Text;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmMail : Form
    {
        Control_Modules MyBase = new Control_Modules();
        public String Term;
        public Int64 Code;
        public DateTime Date;

        public FrmMail()
        {
            InitializeComponent();
        }

        public void Mail_Initialize(String FromID, String ToId, String Subject, params String[] Attach)
        {
            try
            {
                TxtFrom.Text = FromID;
                TxtToId.Text = String.Empty;
                TxtSubject.Text = Subject;
                foreach (String Str in Attach)
                {
                    if (TxtAttachments.Text.Trim() == string.Empty)
                    {
                        TxtAttachments.Text = Str;
                    }
                    else
                    {
                        TxtAttachments.Text = ", " + Str;
                    }
                }
                TxtFrom.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ButSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtToId.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Address ...!");
                    TxtToId.Focus();
                    return;
                }
                ButSend.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                if (MyBase.CheckInternetConnection())
                {
                    if (MyBase.SendMail("psccbe@sancharnet.in", "psccbe", "sanchar123", "PSC", "smra.sancharnet.in", TxtSubject.Text.Trim(), TxtBody.Text.Trim(), TxtCCId.Text.Trim(), TxtBccId.Text.Trim(), TxtToId.Text.Trim(), Term, Code, Date, TxtAttachments.Text.Trim()))
                    {
                        MessageBox.Show("Mail Sent ...!");
                        this.Close(); 
                    }
                    else
                    {
                        MessageBox.Show("Please Check Mail Settings ...!");
                        ButSend.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Check Internet Connection ...!");
                    ButSend.Enabled = true;
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ButSend.Enabled = true;
            }
        }

        private void ButClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GBMail_Enter(object sender, EventArgs e)
        {

        }

        private void TxtFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Null(TxtFrom, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtFrom_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                MyBase.Handle_Delete(TxtFrom, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtAttachments_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Null(TxtAttachments, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtAttachments_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                MyBase.Handle_Delete(TxtAttachments, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtAttachments_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (TxtAttachments.Text != String.Empty)
                {
                    if (TxtAttachments.Text.ToUpper().Contains(".DOC"))
                    {
                        //MyBase.Open_PDf(TxtAttachments.Text.Trim());
                        MyBase.Open_Word(TxtAttachments.Text.Trim());
                    }
                    else
                    {
                        MyBase.Open_NotePad(TxtAttachments.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}