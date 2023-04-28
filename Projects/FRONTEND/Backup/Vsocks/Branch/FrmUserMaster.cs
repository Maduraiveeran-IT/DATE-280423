using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp; 
using Accounts; 
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmUserMaster : Form, Entry 
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        String Code;
        String Str;

        public FrmUserMaster()
        {
            InitializeComponent();
        }

        void TxtBankShortName_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (Convert.ToInt16(e.KeyChar) == Convert.ToInt16(Keys.Enter))
                {
                    e.Handled = true;
                }
                else
                {
                    MyBase.Return_Ucase(e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        void TxtBankName_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                MyBase.Return_Ucase(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        void CreateUser_Master()
        {
            try
            {
                if (MyBase.Check_Table("Socks_User_Master") == false)
                {
                    MyBase.Run("Create table Socks_User_Master (ID number(8), User_Code Number(8), User_Name Varchar2(200), User_Address1 varchar2(150), User_Status varchar2(5), new_EmpCode Number(4), New_Syscode Number(4), New_DateTime Date,alter_EmpCode Number(4), alter_Syscode Number(4), alter_DateTime Date, Company_Code number(2), Year_Code Varchar2(10))");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Load_User_Level_Socks()
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("Select * from User_Level_Socks Order by User_Level_Code", ref Dt);
                CmbUserLevel.DataSource = Dt;
                CmbUserLevel.DisplayMember = "User_level";
                CmbUserLevel.ValueMember = "User_Level_Code";
                CmbUserLevel.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmBankMaster_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                CreateUser_Master();
                MyBase.Clear(this);
                Load_User_Level_Socks();
                OptN.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        public void Entry_Print()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void TxtCustomerAddress_GotFocus(object sender, System.EventArgs e)
        {
            try
            {
                this.KeyPreview = false; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        void TxtCustomerAddress_LostFocus(object sender, System.EventArgs e)
        {
            try
            {
                this.KeyPreview = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void FrmBankMaster_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "TxtMailID")
                    {
                        if (MyParent._New || MyParent.Edit)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtCustomerName")
                    {
                        //MyBase.ActiveForm_Close(this, MyParent); 
                        Str = "select A.TNo, A.Name, A.DesignationName, A.DeptName , A.Emplno, B.Emplno From Vaahini_Erp_Gainup.Dbo.MIS_Employee_Basic() A Left Join Socks_User_Master B on B.Emplno=A.Emplno Where  CatCode in(5,6) and Tno not like '%Z'";
                        Dr = Tool.Selection_Tool(this, 200, 200, SelectionTool_Class.ViewType.NormalView, "Select Employee", Str, String.Empty, 100, 150,150,150);
                        if (Dr != null)
                        {
                            TxtCustomerName.Text = Dr["tno"].ToString();
                            TxtUserPass.Text = Dr["tno"].ToString();
                            TxtRetype.Text = Dr["tno"].ToString();
                            TxtName.Text = Dr["Name"].ToString();
                            TxtName.Tag = Dr["Emplno"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtLocation")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Location", "Select Locaton_Name Location, Location_Code Code from User_Location_master order by Locaton_Name", string.Empty, 200, 90);
                        if (Dr != null)
                        {
                            TxtLocation.Text = Dr["Location"].ToString();
                            TxtLocation.Tag = Dr["Code"].ToString();
                        }
                    }
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }


        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);

                CmbUserLevel.Enabled = true;
                TxtLocation.Enabled = true;
                OptN.Enabled = true;
                OptY.Enabled = true;

                if (MyParent.UserCode == 1)
                {
                    Code = Convert.ToString(MyBase.MaxWOCC("Socks_User_Master", "User_Code", "")); 
                    TxtCustomerName.Focus();  
                }
                else
                {
                    MyParent.Load_EditEntry();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Save()
        {
            String Sql, Sql1;
            String Block = String.Empty;
            try
            {
                if (TxtCustomerName.Text.Trim() != String.Empty && TxtUserPass.Text.Trim() != String.Empty)
                {
                    if (OptY.Checked == true)
                    {
                        Block = "True";
                    }
                    else
                    {
                        Block = "False";
                    }

                    if (TxtUserPass.Text.Length < 4)
                    {
                        MessageBox.Show("Minimum 4 Characters ...!");
                        MyParent.Save_Error = true;
                        TxtUserPass.Focus();
                        return;
                    }

                    if (TxtUserPass.Text.Trim().ToUpper() != TxtRetype.Text.Trim().ToUpper())
                    {
                        MessageBox.Show("Both are Not Same ...!");
                        MyParent.Save_Error = true;
                        TxtUserPass.Focus();
                        return;
                    }

                    if (CmbUserLevel.Text == String.Empty)
                    {
                        MessageBox.Show("Invalid User Level ...!", "Vaahini");
                        MyParent.Save_Error = true;
                        CmbUserLevel.Focus();
                        return;
                    }

                    if (TxtLocation.Text.Trim() == String.Empty)
                    {
                        MessageBox.Show("Invalid Location ...!", "Vaahini");
                        MyParent.Save_Error = true;
                        TxtLocation.Focus();
                        return;
                    }

                    MyBase.Fill_Null(this);
                    if (MyBase.Get_RecordCount("Socks_User_Master", "USer_Name = '" + TxtCustomerName.Text + "' and USer_Code <> " + Code) > 0)
                    {
                        MessageBox.Show("User Already Exists ...!");
                        TxtCustomerName.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    if (MyParent._New == true)
                    {
                        Code = MyBase.MaxWOCC("Socks_User_Master", "User_Code", "").ToString();
                    }
                    else
                    {
                        MyParent.New_UserCode = MyBase.GetEntryDetails_User("Socks_User_Master", "user_Code = " + Code);
                        MyParent.New_Today = MyBase.GetEntryDetails_Date("Socks_User_Master", "user_Code = " + Code);
                        MyParent.New_SysCode = MyBase.GetEntryDetails_Sys("Socks_User_Master", "user_Code = " + Code);
                    }
                    Sql = "Delete From Socks_User_Master where User_Code = "+ Code;
                    if (MyParent._New == true)
                    {
                        Sql1 = "Insert into Socks_User_Master values (" + Code + "," + Code + ",'" + TxtCustomerName.Text + "','" + MyBase.Ascii(TxtUserPass.Text) + "','" + Block + "'," + MyParent.UserCode + "," + MyParent.SysCode + "," + MyParent.Today + ", " + MyParent.UserCode + "," + MyParent.SysCode + "," + MyParent.Today + "," + MyParent.CompCode + ",'" + MyParent.YearCode + "', " + TxtLocation.Tag.ToString() + ", " + CmbUserLevel.SelectedValue.ToString() + ", " + TxtName.Tag.ToString() + ",'" + TxtMailID.Text + "')";
                    }
                    else
                    {
                        Sql1 = "Insert into Socks_User_Master values (" + Code + "," + Code + ",'" + TxtCustomerName.Text + "','" + MyBase.Ascii(TxtUserPass.Text) + "','" + Block + "'," + MyParent.New_UserCode + "," + MyParent.New_SysCode + "," + MyParent.New_Today + ", " + MyParent.UserCode + "," + MyParent.SysCode + "," + MyParent.Today + "," + MyParent.CompCode + ",'" + MyParent.YearCode + "', " + TxtLocation.Tag.ToString() + ", " + CmbUserLevel.SelectedValue.ToString() + ", " + TxtName.Tag.ToString() + ",'" + TxtMailID.Text + "')";
                    }
                    MyBase.Run(Sql, Sql1);
                    MessageBox.Show("Saved Successfully...!");
                    MyParent.Save_Error = false;
                    MyBase.Clear(this); 
                }
                else
                {
                    MessageBox.Show("Please Give User Details ...!");
                    MyParent.Save_Error = true;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ERROR [23000] [Oracle][ODBC][Ora]ORA-00001: unique constraint"))
                {
                    Entry_Save();
                }
                else
                {
                    throw ex;
                }
            }
        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Enable_Controls(this, true);
                if (MyParent.UserCode == 1)
                {
                    Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "User Master - Edit", "Select C1.USer_Code Code, C1.USer_name Name,E1.Name User_Name,C1.Mail_ID,C1.User_Status Blocked, C1." + MyBase.User_Address() + " User_Address, c1.location_Code, c1.User_Level_Code, C1.EmplNo from Socks_User_Master C1 left join Vaahini_Erp_Gainup.Dbo.EmployeeMas E1 on C1.Emplno=E1.Emplno order by C1.User_Code", "", 100, 200, 200, 200, 100);
                }
                else
                {
                    Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "User Master - Edit", "Select C1.USer_Code Code, C1.USer_name Name,E1.Name User_Name,C1.Mail_ID,C1.User_Status Blocked, C1.User_Status Blocked, C1." + MyBase.User_Address() + " USer_Address, c1.location_Code, c1.User_Level_Code, C1.EmplNo from Socks_User_Master C1 left join Vaahini_Erp_Gainup.Dbo.EmployeeMas E1 on C1.Emplno=E1.Emplno where c1.user_Code = " + MyParent.UserCode + " order by C1.User_Code", "", 100, 150, 150);
                }
                Fill_Datas(Dr);
                TxtCustomerName.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Delete()
        {
            try
            {
                if (MyParent.UserCode == 1)
                {
                    Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "User Master - Delete", "Select C1.USer_Code Code, C1.USer_name Name,E1.Name User_Name,C1.Mail_ID, C1.User_Status Blocked, C1." + MyBase.User_Address() + " USer_Address, c1.location_Code, c1.User_Level_Code, C1.EmplNo from Socks_User_Master C1 left join Vaahini_Erp_Gainup.Dbo.EmployeeMas E1 on C1.Emplno=E1.Emplno order by C1.User_Code", "", 100, 200, 200, 200, 100);
                }
                else
                {
                    MessageBox.Show("Can't Delete ...!");
                    MyParent.Load_ViewEntry();
                    return;
                    Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "User Master - Delete", "Select C1.USer_Code Code, C1.USer_name Name,E1.Name User_Name,C1.Mail_ID,C1.User_Status Blocked, C1.User_Status Blocked,E1.Name User_Name,C1.Mail_ID,C1.User_Status Blocked, C1." + MyBase.User_Address() + " USer_Address, c1.location_Code, c1.User_Level_Code, C1.EmplNo from Socks_User_Master C1 left join Vaahini_Erp_Gainup.Dbo.EmployeeMas E1 on C1.Emplno=E1.Emplno where c1.user_Code = " + MyParent.UserCode + " order by C1.User_Code", "", 100, 150, 150);
                }
                MyBase.Enable_Controls(this, false);
                Fill_Datas(Dr);
                if (Dr != null)
                {
                    MyParent.Load_DeleteConfirmEntry(); 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Delete_Confirm()
        {
            try
            {
                String Sql;
                if (Code.Trim() != String.Empty)
                {
                    Sql = "Delete from Socks_User_Master where User_Code = " + Code;
                    MyBase.Execute(Sql);
                    MessageBox.Show("Deleted Successfully ...!");
                    MyBase.Clear(this);
                    MyParent.Load_DeleteEntry(); 
                }
                else
                {
                    MessageBox.Show("Please Select any User details ...!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_View()
        {
            try
            {
                MyBase.Enable_Controls(this, false);
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "User Master - View", "Select C1.USer_Code Code, C1.USer_name Name,E1.Name User_Name,C1.Mail_ID, C1.User_Status Blocked, C1." + MyBase.User_Address() + "  USer_Address, c1.Location_Code, c1.User_Level_Code, C1.EmplNo from Socks_User_Master C1 left join Vaahini_Erp_Gainup.Dbo.EmployeeMas E1 on C1.Emplno=E1.Emplno where c1.user_Code = " + MyParent.UserCode + " order by C1.User_Code", "", 100, 200, 200, 200, 100);
                Fill_Datas(Dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                if (Dr != null)
                {
                    Code = Convert.ToString(Dr["Code"]);
                    TxtName.Tag = Convert.ToString(Dr["Emplno"]); 
                    TxtCustomerName.Text = Convert.ToString(Dr["Name"]);
                    TxtUserPass.Text = MyBase.Ascii_Reverse(Convert.ToString(Dr["User_Address"]));
                    TxtRetype.Text = MyBase.Ascii_Reverse(Convert.ToString(Dr["User_Address"]));
                    TxtName.Text = Convert.ToString(Dr["User_Name"]);
                    TxtMailID.Text = Convert.ToString(Dr["Mail_ID"]);
                    Load_User_Level_Socks();
                    if (Convert.ToString(Dr["Blocked"]) == "True")
                    {
                        OptY.Checked = true;
                    }
                    else
                    {
                        OptN.Checked = true;
                    }
                    if (Dr["Location_Code"] != DBNull.Value)
                    {
                        TxtLocation.Tag = Dr["Location_Code"].ToString();
                        TxtLocation.Text = MyBase.GetData_InString("User_Location_Master", "Location_Code", TxtLocation.Tag.ToString(), "Locaton_Name");
                    }

                    if (Dr["User_Level_Code"] != null && Dr["User_Level_Code"] != DBNull.Value)
                    {
                        CmbUserLevel.SelectedValue = Dr["User_Level_Code"].ToString();
                    }
                    else
                    {
                        CmbUserLevel.SelectedIndex = -1;
                    }
                    
                    if (TxtCustomerName.Enabled == true)
                    {
                        TxtCustomerName.Enabled = true;
                    }

                    
                    if (MyParent.UserCode == 1 || MyParent.UserCode==4)
                    {
                        CmbUserLevel.Enabled = true;
                        TxtLocation.Enabled = true;
                        TxtName.Enabled = true;
                        TxtMailID.Enabled = true;
                        OptN.Enabled = true;
                        OptY.Enabled = true;
                    }
                    else
                    {
                        TxtCustomerName.Enabled = false;
                        CmbUserLevel.Enabled = false;
                        TxtLocation.Enabled = false;
                        TxtName.Enabled = false;
                        TxtMailID.Enabled = false;
                        OptN.Enabled = false;
                        OptY.Enabled = false;
                    }

                    MyParent.Vew_Help("Socks_User_Master", "USer_Code = " + Code);
                }
                else
                {
                    Code = String.Empty; 
                    MyBase.Clear(this);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtLocation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyCode == Keys.Down)
                //{
                //    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Location", "Select Locaton_Name Location, Location_Code Code from User_Location_master order by Locaton_Name", string.Empty, 200, 90);
                //    if (Dr != null)
                //    {
                //        TxtLocation.Text = Dr["Location"].ToString();
                //        TxtLocation.Tag = Dr["Code"].ToString();
                //    }
                //}
                //else if (e.KeyCode == Keys.Enter)
                //{
                //    if (TxtUserPass.Text.Length < 4)
                //    {
                //        MessageBox.Show("Minimum 4 Characters ...!");
                //        e.Handled = true;
                //        TxtUserPass.Focus();
                //    }
                //    else
                //    {
                //        e.Handled = true;
                //        if (TxtUserPass.Text.Trim().ToUpper() != TxtRetype.Text.Trim().ToUpper())
                //        {
                //            MessageBox.Show("Both are Not Same ...!");
                //            TxtUserPass.Focus();
                //            return;
                //        }
                //        if (TxtLocation.Text.Trim() == String.Empty)
                //        {
                //            MessageBox.Show("Invalid Location ...!");
                //            TxtLocation.Focus();
                //            return;
                //        }
                //        MyParent.Load_SaveEntry();
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Null(TxtLocation, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GBMain_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FrmUserMaster_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtName" )
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}