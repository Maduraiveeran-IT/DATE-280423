using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Accounts_ControlModules;
using System.Drawing;
using System.IO;
using System.Text;
//using System.Threading;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmODBCLogin : Form
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        public FrmODBCLogin()
        {
            InitializeComponent();
        }

        void LoadUser()
        {
            try
            {
                CmbUser.Items.Add("Oracle");
                CmbUser.Items.Add("Sql Server");
                CmbUser.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Login()
        {
            String Driver = String.Empty;
            String Str = String.Empty;
            try
            {
                if (MyBase.Check_Directory("C:\\Vaahrep") == false)
                {
                    System.IO.Directory.CreateDirectory("C:\\Vaahrep");
                }
                StreamWriter Edit = new StreamWriter("C:\\Vaahrep\\Projects.txt");
                StreamWriter Edit1 = new StreamWriter("C:\\Vaahrep\\ProjectsSql.txt");
                if (CmbUser.Text.Trim().Length > 0)
                {
                    if (CmbUser.Text.Contains("Oracle"))
                    {
                        Driver = "Oracle in OraHome90";
                    }
                    else
                    {
                        Driver = "{Sql Server}";
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Mode ... !");
                    CmbUser.Text = String.Empty;
                    CmbUser.Focus();
                }

                if (CmbUser.Text.Contains("Oracle"))
                {
                    Str = MyBase.Connection_Ascii("Driver=" + Driver + ";Server=" + TxtServer.Text + ";Uid=" + TxtUSer.Text + ";Pwd=" + TxtPassword.Text + ";DBQ=" + TxtDatabase.Text + ";");
                    Edit.WriteLine(Str);
                }
                else
                {
                    Str = MyBase.Connection_Ascii("Driver=" + Driver + ";Server=" + TxtServer.Text + ";Uid=" + TxtUSer.Text + ";Pwd=" + TxtPassword.Text + ";Database=" + TxtDatabase.Text + ";");
                    Edit.WriteLine(Str);
                    Str = MyBase.Connection_Ascii("Data Source=" + TxtServer.Text + ";Uid=" + TxtUSer.Text + ";Pwd=" + TxtPassword.Text + ";Initial Catalog=" + TxtDatabase.Text + ";Integrated security=false");
                    Edit1.WriteLine(Str);
                }

                Edit.Close();
                Edit1.Close();
                //Thread Th = new Thread(new ThreadStart(Run));
                //Th.Start();
                //this.Close();
                this.Hide();
                Run();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                this.StartPosition = FormStartPosition.Manual;
                this.Top = 250;
                this.Left = 300;
                LoadUser();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.Close();
            Application.Exit();
        }

        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{Tab}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Login();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Run()
        {
            try
            {
                FrmLogin Frm = new FrmLogin();
                //Application.Run(Frm);
                Frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Login();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}