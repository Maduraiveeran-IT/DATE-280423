using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using Accounts_ControlModules;
using System.Text;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmPrintPreview : Form
    {
        String File;
        Control_Modules MyBase = new Control_Modules();
        Boolean AutoPageLen = true;
        Int32[] pagesLen;

        public FrmPrintPreview()
        {
            InitializeComponent();
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    //e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmPrintPreview_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                }
                else if (e.KeyCode == Keys.Escape)
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Number(TxtFrom, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Number(TxtFrom, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void PrintMode(String Title, String Filepath, Int32 Page, Boolean Auto, params Int32[] PagesLen)
        {
            try
            {
                this.Text = Title + " - Print Preview ...!";
                AutoPageLen = Auto;
                pagesLen = PagesLen;
                richTextBox1.LoadFile(Filepath, RichTextBoxStreamType.PlainText);
                richTextBox1.ReadOnly = true;
                if (Page > 0)
                {
                    TxtFrom.Text = "1";
                    TxtTo.Text = Page.ToString();
                    txtCopies.Text = "1";
                    File = Filepath;
                    ButPrint.Enabled = true;
                }
                else
                {
                    ButPrint.Enabled = false;
                }
                richTextBox1.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmPrintPreview_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtFrom.Text.Trim() == string.Empty || TxtTo.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Page Range ...!");
                    TxtFrom.Focus();
                    return;
                }
                if (Convert.ToDouble(TxtTo.Text) < Convert.ToDouble(TxtFrom.Text))
                {
                    MessageBox.Show("Invalid Page Range ...!");
                    TxtFrom.Focus();
                    return;
                }
                if (Convert.ToDouble(TxtFrom.Text) == 0)
                {
                    MessageBox.Show("Invalid Page Range ...!");
                    TxtFrom.Focus();
                    return;
                }
                if (txtCopies.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid No Of Copies ...!");
                    txtCopies.Focus();
                    return;
                }
                if (Convert.ToDouble(txtCopies.Text) == 0)
                {
                    MessageBox.Show("Invalid No Of Copies ...!");
                    txtCopies.Focus();
                    return;
                }
                for (int i = 0; i <= Convert.ToInt32(txtCopies.Text) - 1; i++)
                {
                    //MyBase.Print(File);
                    if (AutoPageLen)
                    {
                        MyBase.Print_PageByPage(File, 72, Convert.ToInt16(TxtFrom.Text), Convert.ToInt16(TxtTo.Text));
                    }
                    else
                    {
                        MyBase.Print_PageDetails(Convert.ToInt16(TxtFrom.Text), Convert.ToInt16(TxtTo.Text), File, pagesLen);
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ButExport_Click(object sender, EventArgs e)
        {
            try
            {
                GBExport.Visible = true;
                OptWord.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void DisposeExport()
        {
            try
            {
                GBExport.Visible = false;
                ButExport.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ButExport1_Click(object sender, EventArgs e)
        {
            String FileName = String.Empty;
            try
            {
                if (OptWord.Checked == true)
                {
                    FileName = MyBase.ShowSave("Quotation Export ..!", "Quotation_", "Micrsoft Word (*.doc)|*.doc");
                    if (FileName != String.Empty)
                    {
                        FileInfo Fl1 = new FileInfo(File);
                        if (MyBase.Check_File(FileName))
                        {
                            MyBase.Delete_File(FileName);
                        }
                        Fl1.CopyTo(FileName);
                        DisposeExport();
                    }
                }
                else if (OptExcel.Checked == true)
                {
                    FileName = MyBase.ShowSave("Quotation Export ..!", "Quotation_", "Micrsoft Excel (*.xls)|*.xls");
                    if (FileName != String.Empty)
                    {
                        FileInfo Fl1 = new FileInfo(File);
                        if (MyBase.Check_File(FileName))
                        {
                            MyBase.Delete_File(FileName);
                        }
                        Fl1.CopyTo(FileName);
                        DisposeExport();
                    }
                }
                else if (OptMail.Checked == true)
                {
                    FileName = File.Replace(".txt", ".doc");
                    if (FileName != String.Empty)
                    {
                        FileInfo Fl1 = new FileInfo(File);
                        if (MyBase.Check_File(FileName))
                        {
                            MyBase.Delete_File(FileName);
                        }
                        Fl1.CopyTo(FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DisposeExport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButClose_Click(object sender, EventArgs e)
        {
            DisposeExport();
        }

        private void FrmPrintPreview_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void TxtAttachments_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButSend_Click(object sender, EventArgs e)
        {

        }
    }
}