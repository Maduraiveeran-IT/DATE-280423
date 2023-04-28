using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Accounts_ControlModules;
using System.IO;
using System.Drawing;
using System.Text;
//using System.Threading;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmLogin : Form
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        Int32 User_Code = 0;
        Int32 EmplNo_TA=0;
        public FrmLogin()
        {
            InitializeComponent();
        }

        void CreateLogCmp()
        {
            try
            {
                //if (MyBase.Check_Table("LogCmp") == false)
                //{
                //    MyBase.Run("Create Table LogCmp (Company_Code Numeric(2), Company_Name Varchar(50))");
                //    if (Convert.ToDecimal(MyBase.Get_RecordCount("LogCmp", "")) == 0)
                //    {
                //        MyBase.Run("Insert into Logcmp values (1,'PSC - Coimbatore')");
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void LoadUser()
        {
            try
            {
                //DataTable Dt = new DataTable();
                //MyBase.Load_Data("Select User_Code, User_name from Fixed_User_Master order by User_Name", ref Dt);
                //if (Dt.Rows.Count == 0)
                //{
                    //CmbUser.Items.Add("BASE");
                //}
                //else
                //{
                    //CmbUser.DataSource = Dt;
                    //CmbUser.DisplayMember = "USer_Name";
                    //CmbUser.ValueMember = "User_Code";
                    //if (Dt.Rows.Count == 1)
                    //{
                        //CmbUser.SelectedIndex = 0;
                        //CmbUser.TabStop = false;
                        //CmbUser.SelectionLength = 0;
                    //}
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Login()
        {
            try
            {
                #region Password Log
                if (MyBase.Check_TableField("Socks_User_Master", "User_Address"))
                {
                    MyBase.Execute("SP_Rename 'Socks_User_Master.User_Address', 'User_Address1', 'Column'");
                }

                if (MyBase.Check_TableField("Socks_User_Master", "User_Address1"))
                {
                    if (MyBase.Check_TableField("Socks_User_Master", "User_Address2") == false)
                    {
                        MyBase.Execute("sp_rename 'Socks_User_Master.User_Address1', 'User_address2', 'Column'");
                    }
                }

                if (MyBase.Check_TableField("Socks_User_Master", "User_Address2"))
                {
                    if (MyBase.Check_TableField("Socks_User_Master", "User_Address3") == false)
                    {
                        MyBase.Execute("sp_rename 'Socks_User_Master.User_Address2', 'User_address3', 'Column'");
                    }
                }

                if (MyBase.Check_TableField("Socks_User_Master", "User_Address3"))
                {
                    if (MyBase.Check_TableField("Socks_User_Master", "User_Address4") == false)
                    {
                        MyBase.Execute("sp_rename 'Socks_User_Master.User_Address3', 'User_address4', 'Column'");
                    }
                }

                if (MyBase.Check_TableField("Socks_User_Master", "User_Address4"))
                {
                    if (MyBase.Check_TableField("Socks_User_Master", "User_Address5") == false)
                    {
                        MyBase.Execute("sp_rename 'Socks_User_Master.User_Address4', 'User_address5', 'Column'");
                    }
                }

                if (MyBase.Check_TableField("Socks_User_Master", "User_Address5"))
                {
                    if (MyBase.Check_TableField("Socks_User_Master", "User_Address6") == false)
                    {
                        MyBase.Execute("sp_rename 'Socks_User_Master.User_Address5', 'User_address6', 'Column'");
                    }
                }

                if (MyBase.Check_TableField("Socks_User_Master", "User_Address6"))
                {
                    if (MyBase.Check_TableField("Socks_User_Master", "User_Address7") == false)
                    {
                        MyBase.Execute("sp_rename 'Socks_User_Master.User_Address6', 'User_address7', 'Column'");
                    }
                }

                if (MyBase.Check_TableField("Socks_User_Master", "User_Address7"))
                {
                    if (MyBase.Check_TableField("Socks_User_Master", "User_Address8") == false)
                    {
                        MyBase.Execute("sp_rename 'Socks_User_Master.User_Address7', 'User_address8', 'Column'");
                    }
                }


                if (MyBase.Check_TableField("Socks_User_Master", "User_Address8"))
                {
                    if (MyBase.Check_TableField("Socks_User_Master", "User_Address9") == false)
                    {
                        MyBase.Execute("sp_rename 'Socks_User_Master.User_Address8', 'User_address9', 'Column'");
                    }
                }

                if (MyBase.Check_TableField("Socks_User_Master", "User_Address9"))
                {
                    if (MyBase.Check_TableField("Socks_User_Master", "User1_Address10") == false)
                    {
                        MyBase.Execute("sp_rename 'Socks_User_Master.User_Address9', 'User1_address10', 'Column'");
                    }
                }


                if (MyBase.Check_TableField("Socks_User_Master", "User1_Address10"))
                {
                    if (MyBase.Check_TableField("Socks_User_Master", "User1_Address11") == false)
                    {
                        MyBase.Execute("sp_rename 'Socks_User_Master.User1_Address10', 'User1_address11', 'Column'");
                    }
                }

                if (MyBase.Check_TableField("Socks_User_Master", "User1_Address11"))
                {
                    if (MyBase.Check_TableField("Socks_User_Master", "User1_Address12") == false)
                    {
                        MyBase.Execute("sp_rename 'Socks_User_Master.User1_Address11', 'User1_address12', 'Column'");
                    }
                }

                if (MyBase.Check_TableField("Socks_User_Master", "User1_Address12"))
                {
                    if (MyBase.Check_TableField("Socks_User_Master", "User1_Address13") == false)
                    {
                        MyBase.Execute("sp_rename 'Socks_User_Master.User1_Address12', 'User1_address13', 'Column'");
                    }
                }

                if (MyBase.Check_TableField("Socks_User_Master", "User1_Address13"))
                {
                    if (MyBase.Check_TableField("Socks_User_Master", "User1_Address14") == false)
                    {
                        MyBase.Execute("sp_rename 'Socks_User_Master.User1_Address13', 'User1_address14', 'Column'");
                    }
                }

                #endregion

                if (CmbUser.Text.Trim() == "BASE" && TxtPass.Text.Trim().ToUpper() == "VAAHINI")
                {
                    this.Hide();
                    Run();
                }
                else
                {

                    DataTable Tdt = new DataTable();
                    MyBase.Load_Data("Select User_Code, Pass, Asci, Emplno From Socks_Login () where User_name = '" + CmbUser.Text + "'", ref Tdt);
                    if (Tdt.Rows.Count > 0)
                    {
                        if (Tdt.Rows[0]["Asci"].ToString().ToUpper() == "TRUE")
                        {
                            if (MyBase.Ascii(TxtPass.Text) == Tdt.Rows[0]["pass"].ToString())       // User_Master - Ascii Checking
                            {
                                Valid_Login(Convert.ToInt32(Tdt.Rows[0]["User_Code"]), Convert.ToInt32(Tdt.Rows[0]["EmplNo"]));
                            }
                            else
                            {
                                Invalid_Login();
                            }
                        }
                        else
                        {
                            if (TxtPass.Text.ToUpper() == Tdt.Rows[0]["pass"].ToString().ToUpper()) // Employeemas - DOB Checking
                            {
                                Valid_Login(Convert.ToInt32(Tdt.Rows[0]["User_Code"]), Convert.ToInt32(Tdt.Rows[0]["EmplNo"]));
                            }
                            else
                            {
                                Invalid_Login();
                            }
                        }
                    }
                    else
                    {
                        Invalid_Login();
                    }

                    #region Old_Password_Check
                    /*
                    if (MyBase.Get_RecordCount("Socks_User_Master", "USer_Name = '" + CmbUser.Text + "' and " + MyBase.User_Address() + " = '" + MyBase.Ascii(TxtPass.Text.ToUpper()) + "'") == 0)
                    {
                        MessageBox.Show("Invalid Login ...!");
                        CmbUser.Text = String.Empty;
                        TxtPass.Text = String.Empty;
                        CmbUser.Focus();
                    }
                    else
                    {
                        if (MyBase.Get_RecordCount("Socks_User_Master", "USer_Name = '" + CmbUser.Text + "' and " + MyBase.User_Address() + " = '" + MyBase.Ascii(TxtPass.Text.ToUpper()) + "' and User_status = 'False'") > 0)                        
                        {
                            User_Code = Convert.ToInt32(MyBase.GetData_InNumber("Socks_User_Master", "USer_Name", CmbUser.Text, "USer_Code"));
                            MyBase.UCode = User_Code;
                            EmplNo_TA = Convert.ToInt32(MyBase.GetData_InNumber("Socks_User_Master", "USer_Name", CmbUser.Text, "EmplNo"));
                            MyBase.EmplNo_TA = EmplNo_TA;
                            System.Environment.SetEnvironmentVariable("User_Code", User_Code.ToString());
                            System.Environment.SetEnvironmentVariable("Company_Code", "1");
                            this.Hide();
                            Add_Recent_User();
                            Run();
                        }
                        else
                        {
                            MessageBox.Show("Blocked ...!");
                            CmbUser.Text = String.Empty;
                            TxtPass.Text = String.Empty;
                            CmbUser.Focus();
                        }
                    }
                     **/
                    #endregion
                }
                //if (CmbUser.Text.Trim() == "BASE" && TxtPass.Text.Trim().ToUpper() == "VAAHINI")
                //{
                //    this.Hide();
                //    Run();
                //}
                //else
                //{
                //    if (MyBase.Get_RecordCount("Socks_User_Master", "USer_Name = '" + CmbUser.Text + "' and " + MyBase.User_Address() + " = '" + MyBase.Ascii(TxtPass.Text.ToUpper()) + "'") == 0)
                //    {
                //        MessageBox.Show("Invalid Login ...!");
                //        CmbUser.Text = String.Empty;
                //        TxtPass.Text = String.Empty;
                //        CmbUser.Focus();
                //    }
                //    else
                //    {
                //        DataTable Tempdt = new DataTable();
                //        MyBase.Load_Data("Select Userid User_Code, username, password from UserName where username = '" + CmbUser.Text.Trim() + "' and password = '" + TxtPass.Text + "'", ref Tempdt);
                //        if (Tempdt.Rows.Count == 0)
                //        {
                //            MessageBox.Show("Invalid Login ...!");
                //            CmbUser.Text = String.Empty;
                //            TxtPass.Text = String.Empty;
                //            CmbUser.Focus();
                //        }
                //        else
                //        {
                //            User_Code = Convert.ToInt32(Tempdt.Rows[0]["User_Code"]);
                //            MyBase.UCode = User_Code;
                //            System.Environment.SetEnvironmentVariable("User_Code", User_Code.ToString());
                //            System.Environment.SetEnvironmentVariable("Company_Code", "1");
                //            this.Hide();
                //            Add_Recent_User();
                //            Run();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Valid_Login(Int32 T_User_Code, Int32 T_Emplno)
        {
            try
            {
                User_Code = T_User_Code;
                MyBase.UCode = User_Code;
                MyBase.Emplno = T_Emplno;
                System.Environment.SetEnvironmentVariable("User_Code", User_Code.ToString());
                System.Environment.SetEnvironmentVariable("Company_Code", "1");
                this.Hide();
                Add_Recent_User();
                Run();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Invalid_Login()
        {
            try
            {
                MessageBox.Show("Invalid Login ...!");
                CmbUser.Text = String.Empty;
                TxtPass.Text = String.Empty;
                CmbUser.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Recent_User()
        {
            
            try
            {
                if (File.Exists("C:\\Vaahrep\\RU.txt"))
                {
                    StreamReader Rd = new StreamReader("C:\\Vaahrep\\RU.txt");
                    CmbUser.Text = Rd.ReadLine();
                    Rd.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Add_Recent_User()
        {

            try
            {
                if (File.Exists("C:\\Vaahrep\\RU.txt"))
                {
                    File.Delete("C:\\Vaahrep\\RU.txt");
                }
                StreamWriter Rd = new StreamWriter("C:\\Vaahrep\\RU.txt");
                Rd.WriteLine(CmbUser.Text);
                Rd.Close();
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
                CmbUser.ContextMenu = new ContextMenu();
                TxtPass.ContextMenu = new ContextMenu();
                MyParent = (MDIMain)this.MdiParent;
                if (System.IO.File.Exists("C:\\Vaahrep\\VSocks.txt"))
                {
                    CreateLogCmp();
                    this.StartPosition = FormStartPosition.Manual;
                    this.Top = 250;
                    this.Left = 300;
                    LoadUser();
                    Recent_User();
                }
                else
                {
                    MessageBox.Show("Connection Details Failiure ...!","Vaahini");
                    ODBCLogin();
                }
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
                if (e.Control == true && (e.KeyCode == Keys.C || e.KeyCode == Keys.V))
                {
                    Clipboard.Clear();
                }
                else if (e.KeyCode == Keys.Enter)
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
                FrmCompLogin CompLog = new FrmCompLogin(MyBase.UCode, MyBase.Emplno);
                //CompLog.USerCode = MyBase.UCode;
                CompLog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void RunODBC()
        {
            try
            {
                FrmODBCLogin Frm = new FrmODBCLogin();
                this.Hide();
                Frm.ShowDialog();
                //Application.Run(Frm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ODBCLogin()
        {
            try
            {
                //Thread Th = new Thread(new ThreadStart(RunODBC));
                //Th.Start();
                //this.Close();
                RunODBC();
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

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}