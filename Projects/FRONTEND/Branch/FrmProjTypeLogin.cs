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
    public partial class FrmProjTypeLogin : Form
    {
        Int32 Company_UserCode = 0;
        Int32 Company_Emplno = 0;
        Control_Modules MyBase = new Control_Modules();
        DataTable Dt = new DataTable();
        public Int32 USerCode = 0;
        Boolean Sec_Flag = false;
        Boolean Th_Flag = false;
        String CompList = "";

        public FrmProjTypeLogin()
        {
            InitializeComponent();
        }

        public FrmProjTypeLogin(Int32 User_Code, Int32 Emplno)
        {
            InitializeComponent();
            Company_UserCode = User_Code;
            Company_Emplno = Emplno;
        }


        private void FrmProjTypeLogin_Load(object sender, EventArgs e)
        {
            try
            {
                Grid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Blue;
                Grid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                Load_Company();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Select_Company()
        {
            try
            {
                if (Dt.Rows.Count > 0)
                {
                    this.Hide();
                    MDIMain MainBase = new MDIMain();
                    MainBase.UserCode = Company_UserCode;
                    MainBase.Emplno = Company_Emplno;
                    MainBase.UserName = MyBase.GetData_InString("Projects.dbo.Projects_Login()", "USer_Code", Company_UserCode.ToString(), "User_Name");
                    MainBase.Proj_Login_Code = Convert.ToInt32(Grid["Rowid", Grid.CurrentCell.RowIndex].Value);
                    MainBase.Proj_Login_Name = Grid["Name", Grid.CurrentCell.RowIndex].Value.ToString();

                    MainBase.CompCode = 0;
                    MainBase.CompName = "GAINUP";
                    MainBase.YearCode = "2022-2023";

                    FrmCompLogin CompLog = new FrmCompLogin(Company_UserCode, MainBase.Proj_Login_Code);                  
                    CompLog.ShowDialog();
                }
                else
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Grid_Color()
        {
            try
            {
                //for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                //{
                //    if (DateTime.Now >= Convert.ToDateTime(Dt.Rows[i]["Sdt"]) && DateTime.Now <= Convert.ToDateTime(Dt.Rows[i]["Edt"]))
                //    {
                //        //Grid.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Lime;
                //        Grid["Year", i].Value = Grid["Year", i].Value + " * ";
                //    }
                //}

                //if (Dt.Rows.Count > 0)
                //{
                //    Grid.Rows[0].DefaultCellStyle.BackColor = System.Drawing.Color.Lime;
                //    Grid["Year", 0].Value = Grid["Year", 0].Value + " * ";
                //}
                //if (Dt.Rows.Count > 1)
                //{
                //    Grid.Rows[1].DefaultCellStyle.BackColor = System.Drawing.Color.Lime;
                //    Grid["Year", 1].Value = Grid["Year", 1].Value + " * ";
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Load_Company()
        {
            try
            {

                DataTable TDt = new DataTable();
                MyBase.Load_Data("Select Project_List From Projects_User_Master Where User_Code = " + Company_UserCode + "", ref TDt);
                if (TDt.Rows.Count > 0)
                {
                    CompList = TDt.Rows[0][0].ToString();
                }
                else
                {
                    CompList = "-1";
                }
                Grid.DataSource = MyBase.Load_Data("select Name, Rowid, OSNo From Project_Login_Name Where Rowid in (" + CompList + ") Or 0 in (" + CompList + ") Order by OSNo", ref Dt);

               
                
                Grid.RowHeadersWidth = 10;
                MyBase.Grid_Designing(ref Grid, ref Dt, "OSNo", "Rowid");
                MyBase.ReadOnly_Grid_Without(ref Grid);
                MyBase.Grid_Width(ref Grid, 390);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Load_Company_Master()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void ButOK_Click(object sender, EventArgs e)
        {
            try
            {
                Select_Company();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmProjTypeLogin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Select_Company();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButCompany_Click(object sender, EventArgs e)
        {
            try
            {
                return;
                MDIMain MainBase = new MDIMain();
                MainBase.UserCode = USerCode;
                MainBase.UserName = MyBase.GetData_InString("PRojects_User_Master", "USer_Code", MyBase.UCode.ToString(), "User_Name");
                MainBase.CompCode = 99;
                MainBase.CompName = "Vaahini";
                MainBase.YearCode = "2009-2010";
                MainBase.OnlyFor_Company = true;
                this.Hide();
                //MainBase.SDate = Convert.ToDateTime(Grid["SDt", Grid.CurrentCell.RowIndex].Value);
                //MainBase.EDate = Convert.ToDateTime(Grid["EDt", Grid.CurrentCell.RowIndex].Value);
                MainBase.ShowDialog();
                Load_Company();
                Grid.Focus();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmProjTypeLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Control.ModifierKeys == Keys.Control && e.KeyChar == Convert.ToChar(Keys.Space))
                {
                    Sec_Flag = true;
                }
                else if (Control.ModifierKeys == Keys.Control && e.KeyChar == Convert.ToChar(20) && Sec_Flag == true)
                {
                    Sec_Flag = false;
                    Th_Flag = true;
                }
                else if (Control.ModifierKeys == Keys.Control && e.KeyChar == Convert.ToChar(16) && Th_Flag == true)
                {
                    Th_Flag = false;
                    DataTable TDt = new DataTable();
                    MyBase.Load_Data("Select * from acc_Settings where inventory = 'True'", ref TDt);
                    if (TDt.Rows.Count > 0)
                    {
                        if (System.Environment.GetEnvironmentVariable("Company_Code") == "1")
                        {
                            Grid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Yellow;
                            Grid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                            System.Environment.SetEnvironmentVariable("Company_Code", "50");
                            ButCompany.Visible = false;
                        }
                        else
                        {
                            Grid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Blue;
                            Grid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                            System.Environment.SetEnvironmentVariable("Company_Code", "1");
                            ButCompany.Visible = true;
                        }
                    }
                    Load_Company();
                }
                else
                {
                    Sec_Flag = false;
                    Th_Flag = false;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().Contains("SERVER DOES NOT EXIST"))
                {
                    Grid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Blue;
                    Grid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                    MessageBox.Show("Illeagal Server Settings ...!", "Vaahini", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    System.Environment.SetEnvironmentVariable("Company_Code", "1");
                    Load_Company();
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

    }
}
