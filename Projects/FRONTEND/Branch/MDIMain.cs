using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Accounts_ControlModules;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;
using SelectionTool_NmSp;
using Accounts;


namespace Accounts
{
    public interface Entry
    {
        void Entry_New();
        void Entry_Save();
        void Entry_Edit();
        void Entry_Delete();
        void Entry_Delete_Confirm();
        void Entry_View();
        void Entry_Print();
    }

    public partial class MDIMain : Form
    {
        public int UserCode, SysCode, CompCode, Proj_Login_Code;
        public String New_UserCode, New_SysCode, New_Today, Proj_Login_Name;
        public String Date_Only;
        public Double Idle = 0;
        public DateTime Min_Date;
        public Int32 Min_Days = 0;
        public Boolean _New, Edit, Delete, View, _Form, Save_Error, Progress_Flag, F3Mode = false, OnlyFor_Company = false;
        public Boolean Direct_voucher = false;
        public String Today, CompName, CompPrintName, UserName, CompAddress, Message = "Have a Nice Day ...!", CompPhone, CompEmail, CompFax, CompTin, CompCst;
        public DateTime SDate, EDate;
        public Boolean Duplicate = false;
        public Int32 User_Level_Code = 0;
        public Int64 Duplicate_Vcode = 0;
        public DateTime Duplicate_Vdate = DateTime.Now;
        public Boolean Carry_Overed = false;
        public DataTable Cri_DT = new DataTable();
        public Double Output = 0;
        public Int64 User_Entry_Id = 0;
        public Boolean Inventory = false;
        public String Tally_Server = String.Empty, Tally_Company = String.Empty;
        public String Head_Table = String.Empty;
        public Boolean Exe_Update = false;
        public DataGridView DGV;
        String Calculator_Table = "Calc" + Environment.MachineName.Replace("-", "");
        public String ERP_YearCode = String.Empty;
        public String ERP_DBName = String.Empty;
        public String TDS_Deduct_ON = "Payment".ToUpper();
        public Boolean AutoNo_Flag = false;
        public Boolean Previous_Balance_CarryOver = false;
        public Boolean Export_Order = false;
        public Boolean Export_Invoice = false;
        public String Company_Tin = String.Empty;
        public String Base_Dir = "C:\\Vaahrep";
        public Boolean OpBal_lock = true, Vouch_Edit_Lock = true, ERP_Link = false, Billing_Ledger_From_Accounts = false, SMS = false;
        public DataTable Report_DT;
        public int[] Multiple_Company_Code;
        public Boolean Update_Flag = false;
        public String[] Multiple_Company_Address;
        public String CompCode_String = String.Empty;
        public String Company_Address_String = String.Empty;
        public int Dup_Company_Code = 0;
        public Boolean Double_UOM = false;
        public Boolean Stock_Validation = false;
        public String Org_Server_Address = "[Vaahini28\\Vaahini_28].accounts_Empty.dbo.";
        public Int32 User_Datelock = 0;
        public Int32 Emplno = 0;
        public Int64 EmplNo_Org = 0;
        public Int64 EmplNo_TA = 0;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        public Boolean Accounts_Input = false;
        public Boolean Security_Flag = false;

        DataTable Menu_Dt = new DataTable();

        /// <summary>
        /// Voucher Single Entry Mode 
        /// </summary>
        
        public Boolean Voucher_Single_Entry_Mode = true;
        public Boolean Trial_Ledger_Opening = false;
        public String Cri_For = string.Empty;
        public String YearCode;
        public String[] ItemNameArr;
        Control_Modules MyBase = new Control_Modules();
        public String Sale_Return;
        String Str = String.Empty;
        DateTime Dtime;
        String OrderNo;
        Int64 Plan_ID;
        DateTime ODate;
        //DateTime SDate;
        Int64 LeadDays;
        Int64 LeadID;
        Int64 Division_ID;
        Int64 Action_ID, Comp_ID;
        String Division;

        #region Base Functions

        private enum Window
        {
            Minimized = 0,
            Normal=1,
            Maximized = 2,
        }

        private enum Entry_Mode
        {
            _New = 0,
            Edit = 1,
            Delete = 2,
            View = 3,
        }

        void BasicVariables()
        {
            try
            {
                Time_Update();
                Date_Only = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                //UserName = MyBase.GetData_InString("Socks_User_Master", "User_Code", Convert.ToString(UserCode), "User_Name");
                UserName = MyBase.GetData_InString("Projects.dbo.Projects_User_Master", "User_Code", Convert.ToString(UserCode), "User_Name");
                if (MyBase.Check_Directory(Base_Dir) == false)
                {
                    System.IO.Directory.CreateDirectory(Base_Dir);
                }
                loadSysName();
                this.Text = CompName;
                Company_Tin = string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Check_Validate_TDS(int Ledger_Code, ref DotnetVFGrid.MyDataGridView Grid, ref DataTable Dt1, double Amount)
        {
            DataTable Dt = new DataTable();
            try
            {
                if (Is_Tds_Applicable(Ledger_Code))
                {
                    MyBase.Load_Data("Select l1.TDS_Ledger_Code, l2.ledger_NAme, isnull(l1.TDSRATEPER, 0) tdsrateper from ledger_Master l1 left join Ledger_Master l2 on l1.TDS_ledger_Code = l2.ledger_Code and l1.company_Code = l2.company_Code and l1.year_Code = l2.year_Code  where l1.ledger_Code = " + Ledger_Code + " and l1.company_Code = " + CompCode + " and l1.year_Code = '" + YearCode + "'", ref Dt);
                    if (Dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                        {
                            if (Convert.ToInt32(Dt1.Rows[i]["Ledger_Code"]) == Convert.ToInt32(Dt.Rows[0]["tds_ledger_Code"]))
                            {
                                if (Convert.ToDouble(Grid["Credit", i].Value) != (Convert.ToDouble(Dt.Rows[0]["TDSRATEPer"]) * (Amount / 100)))
                                {
                                    Dt1.Rows[i]["Credit"] = String.Format("{0:n}", Math.Round(Convert.ToDouble(Dt.Rows[0]["TDSRATEPer"]) * (Amount / 100)));
                                    Grid.Refresh();
                                }
                                return true;
                            }
                        }
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public Boolean Is_This_Previous_Year ()
        {
            try
            {
                if (MyBase.Get_RecordCount("Socks_Companymas", "Compname like '%Gainup%'") > 0 || MyBase.Get_RecordCount("Socks_Companymas", "Compname like '%avaneetha%'") > 0)
                {
                    return false;
                }
                else
                {
                    if (MyBase.Get_RecordCount("Socks_Companymas", "Company_Code = " + CompCode + " and SDt > '" + String.Format("{0:dd-MMM-yyyy}", SDate) + "'") > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public Boolean Check_Validate_TDS_Credit(int Ledger_Code, ref DotnetVFGrid.MyDataGridView Grid, ref DataTable Dt1, double Amount)
        {
            DataTable Dt = new DataTable();
            try
            {
                if (Is_Tds_Applicable(Ledger_Code))
                {
                    MyBase.Load_Data("Select l1.TDS_Ledger_Code, l2.ledger_NAme, isnull(l1.TDSRATEPER, 0) tdsrateper from ledger_Master l1 left join Ledger_Master l2 on l1.TDS_ledger_Code = l2.ledger_Code and l1.company_Code = l2.company_Code and l1.year_Code = l2.year_Code  where l1.ledger_Code = " + Ledger_Code + " and l1.company_Code = " + CompCode + " and l1.year_Code = '" + YearCode + "'", ref Dt);
                    if (Dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                        {
                            if (Convert.ToInt32(Dt1.Rows[i]["Ledger_Code"]) == Convert.ToInt32(Dt.Rows[0]["tds_ledger_Code"]))
                            {
                                if (Convert.ToDouble(Grid["Credit", i].Value) != (Convert.ToDouble(Dt.Rows[0]["TDSRATEPer"]) * (Amount / 100)))
                                {
                                    Dt1.Rows[i]["Credit"] = String.Format("{0:n}", Math.Round(Convert.ToDouble(Dt.Rows[0]["TDSRATEPer"]) * (Amount / 100)));
                                    Grid.Refresh();
                                }
                                return true;
                            }
                        }
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }


        public void Fill_TDS_Details_Debit(int Ledger_Code, ref DotnetVFGrid.MyDataGridView Grid, ref DataTable Dt1, double Amount)
        {
            Double Rate = 0;
            DataTable Dt = new DataTable();
            Int32 Row = 0;
            try
            {
                if (Is_Tds_Applicable(Ledger_Code))
                {
                    Row = Grid.Rows.Count - 1;
                    MyBase.Load_Data("Select l1.TDS_Ledger_Code, l2.ledger_NAme, isnull(l1.TDSRATEPER, 0) tdsrateper from ledger_Master l1 left join Ledger_Master l2 on l1.TDS_ledger_Code = l2.ledger_Code and l1.company_Code = l2.company_Code and l1.year_Code = l2.year_Code  where l1.ledger_Code = " + Ledger_Code + " and l1.company_Code = " + CompCode + " and l1.year_Code = '" + YearCode + "'", ref Dt);
                    if (Dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                        {
                            if (Convert.ToInt32(Dt1.Rows[i]["Ledger_Code"]) == Convert.ToInt32(Dt.Rows[0]["tds_ledger_Code"]))
                            {
                                if (Convert.ToDouble(Grid["Credit", i].Value) != (Convert.ToDouble(Dt.Rows[0]["TDSRATEPer"]) * (Amount / 100)))
                                {
                                    Dt1.Rows[i]["Credit"] = String.Format("{0:n}", Math.Round(Convert.ToDouble(Dt.Rows[0]["TDSRATEPer"]) * (Amount / 100)));
                                    Grid.Refresh();
                                }
                                return;
                            }
                        }
                        DataRow Dr = Dt1.NewRow();
                        Dr["Ledger_Code"] = Dt.Rows[0]["tds_ledger_Code"].ToString();
                        Dr["Type"] = "TO";
                        if (MyBase.Check_Table("CurBal") == false)
                        {
                            MyBase.Current_Balance(0, SDate, CompCode, YearCode, true);
                        }
                        Dr["CurBal"] = MyBase.GetData_InString("CurBal", "Ledger_Code", Dr["ledger_Code"].ToString(), "Balance");
                        Dr["CurBal1"] = Dr["CurBal"];
                        Dr["Name"] = Dt.Rows[0]["Ledger_Name"].ToString();
                        Dr["Credit"] = String.Format("{0:n}", Math.Round(Convert.ToDouble(Dt.Rows[0]["TDSRATEPer"]) * (Amount / 100)));
                        Dr["Debit"] = String.Format("{0:n}", Convert.ToDouble("0.00"));
                        Dr["Narration"] = Grid["Narration", Row - 1].Value.ToString();
                        Dt1.Rows.InsertAt(Dr, Row);
                        Grid.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Fill_TDS_Details_Credit(int Ledger_Code, ref DotnetVFGrid.MyDataGridView Grid, ref DataTable Dt1, double Amount)
        {
            Double Rate = 0;
            DataTable Dt = new DataTable();
            Int32 Row = 0;
            Int32 Rows = 0;
            DataRow Dr;
            try
            {
                if (Is_Tds_Applicable(Ledger_Code))
                {
                    Row = Grid.Rows.Count - 1;
                    MyBase.Load_Data("Select l1.TDS_Ledger_Code, l2.ledger_NAme, isnull(l1.TDSRATEPER, 0) tdsrateper from ledger_Master l1 left join Ledger_Master l2 on l1.TDS_ledger_Code = l2.ledger_Code and l1.company_Code = l2.company_Code and l1.year_Code = l2.year_Code  where l1.ledger_Code = " + Ledger_Code + " and l1.company_Code = " + CompCode + " and l1.year_Code = '" + YearCode + "'", ref Dt);
                    if (Dt.Rows.Count > 0)
                    {
                        Rows = Dt1.Rows.Count;
                        for (int i = 1; i <= Rows - 1; i++)
                        {
                            Dt1.Rows.RemoveAt(1);
                            Row = 1;
                        }

                        // TDS Ledger
                        Dr = Dt1.NewRow();
                        Dr["Ledger_Code"] = Dt.Rows[0]["tds_ledger_Code"].ToString();
                        Dr["Type"] = "TO";
                        if (MyBase.Check_Table("CurBal") == false)
                        {
                            MyBase.Current_Balance(0, SDate, CompCode, YearCode, true);
                        }
                        Dr["CurBal"] = MyBase.GetData_InString("CurBal", "Ledger_Code", Dr["ledger_Code"].ToString(), "Balance");
                        Dr["CurBal1"] = Dr["CurBal"];
                        Dr["Name"] = Dt.Rows[0]["Ledger_Name"].ToString();
                        Dr["Credit"] = String.Format("{0:n}", Math.Round(Convert.ToDouble(Dt.Rows[0]["TDSRATEPer"]) * (Amount / 100)));
                        Dr["Debit"] = String.Format("{0:n}", Convert.ToDouble("0.00"));
                        Dr["Narration"] = Grid["Narration", Row - 1].Value.ToString();
                        Dt1.Rows.InsertAt(Dr, Row);


                        // Debit Head
                        Dr = Dt1.NewRow();
                        Dr["Ledger_Code"] = "0";
                        Dr["Type"] = "BY";
                        if (MyBase.Check_Table("CurBal") == false)
                        {
                            //MyBase.Current_Balance(0, SDate, CompCode, YearCode, true);
                        }
                        Dr["CurBal"] = "0.00 Dr";
                        Dr["CurBal1"] = Dr["CurBal"];
                        Dr["Name"] = String.Empty;
                        //Dr["Credit"] = String.Format("{0:n}", Convert.ToDouble(Dt.Rows[0]["TDSRATEPer"]) * (Amount / 100));
                        Dr["Debit"] = String.Format("{0:n}", Amount);
                        Dr["Credit"] = String.Format("{0:n}", Convert.ToDouble("0.00"));
                        Dr["Narration"] = Grid["Narration", Row - 1].Value.ToString();
                        Dt1.Rows.InsertAt(Dr, Row + 1);

                        // Debit Head - Party
                        Dr = Dt1.NewRow();
                        Dr["Ledger_Code"] = Ledger_Code;
                        Dr["Type"] = "BY";
                        if (MyBase.Check_Table("CurBal") == false)
                        {
                            MyBase.Current_Balance(0, SDate, CompCode, YearCode, true);
                        }
                        Dr["CurBal"] = MyBase.GetData_InString("CurBal", "Ledger_Code", Dr["ledger_Code"].ToString(), "Balance");
                        Dr["CurBal1"] = Dr["CurBal"];
                        Dr["Breakup"] = MyBase.GetData_InStringWC("Ledger_Master", "Ledger_Code", Ledger_Code.ToString(), "Breakup", CompCode, YearCode);
                        Dr["Name"] = MyBase.GetData_InStringWC("Ledger_Master", "Ledger_Code", Ledger_Code.ToString(), "Ledger_Name", CompCode, YearCode);
                        Dr["Debit"] = String.Format("{0:n}", Math.Round(Convert.ToDouble(Dt.Rows[0]["TDSRATEPer"]) * (Amount / 100)));
                        Dr["Credit"] = String.Format("{0:n}", Convert.ToDouble("0.00"));
                        Dr["Narration"] = Grid["Narration", Row - 1].Value.ToString();
                        Dt1.Rows.InsertAt(Dr, Row + 2);

                        Grid.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Time_Update()
        {
            try
            {
                //Today = "to_date('" + Date_Time() + "','dd-mon-yyyy hh:mi:ss PM')";
                Today = "'" + Date_Time() + "'";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Date_Time()
        {
            String Str;
            try
            {
                Str = String.Format("{0:dd-MMM-yyyy} {0:T}", MyBase.GetServerDateTime());
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Button and Menu Settings

        void ShowChild1(Form ChildFrm, Window State, Boolean ToolFrm, Boolean CanMaximize, Entry_Mode Mode)
        {
            try
            {
                ChildFrm.MdiParent = this;
                if (State == Window.Maximized)
                {
                    ChildFrm.WindowState = FormWindowState.Maximized;
                }
                else if (State == Window.Normal)
                {
                    ChildFrm.MaximizeBox = CanMaximize;
                    ChildFrm.WindowState = FormWindowState.Normal;
                    ChildFrm.StartPosition = FormStartPosition.Manual;
                }
                else
                {
                    ChildFrm.WindowState = FormWindowState.Minimized;
                }
                ChildFrm.Show();
                ChildFrm.Tag = String.Empty;
                if (ToolFrm == true)
                {
                    ButtonEnabled(false);
                }
                else
                {
                    if (ToolFrm == false)
                    {
                        if (Mode == Entry_Mode._New)
                        {
                            Load_NewEntry();
                        }
                        else if (Mode == Entry_Mode.Edit)
                        {
                            Load_EditEntry();
                        }
                        else if (Mode == Entry_Mode.Delete)
                        {
                            Load_DeleteEntry();
                        }
                        else if (Mode == Entry_Mode.View)
                        {
                            Load_ViewEntry();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        String Get_Menu_Control_Name(ToolStripMenuItem Tst)
        {
            try
            {
                return Tst.Name;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }


        void ShowChild(Form ChildFrm, Window State, Boolean ToolFrm, Boolean CanMaximize, Entry_Mode Mode, String Menu_Control_Name)
        {
            try
            {

                foreach (Form Frm in this.MdiChildren)
                {
                    if (Frm.Name.ToUpper() == ChildFrm.Name.ToUpper())
                    {
                        this.ActivateMdiChild(Frm);
                        return;
                    }
                }

                ChildFrm.MdiParent = this;
                ChildFrm.AccessibleName = Menu_Control_Name;
                if (State == Window.Maximized)
                {
                    ChildFrm.ControlBox = false;
                    ChildFrm.WindowState = FormWindowState.Maximized;
                }
                else if (State == Window.Normal)
                {
                    ChildFrm.ControlBox = false;
                    ChildFrm.Left = 0;
                    ChildFrm.Top = 0;
                    ChildFrm.WindowState = FormWindowState.Maximized;
                    ChildFrm.StartPosition = FormStartPosition.Manual;
                }
                else
                {
                    ChildFrm.WindowState = FormWindowState.Minimized;
                }
                ChildFrm.Show();
                ChildFrm.Tag = String.Empty;
                if (ToolFrm == true)
                {
                    ButtonEnabled(false);
                    ChildFrm.Tag = null;
                }
                else
                {
                    if (ToolFrm == false)
                    {
                        if (Mode == Entry_Mode._New)
                        {
                            Load_NewEntry();
                        }
                        else if (Mode == Entry_Mode.Edit)
                        {
                            Load_EditEntry();
                        }
                        else if (Mode == Entry_Mode.Delete)
                        {
                            Load_DeleteEntry();
                        }
                        else if (Mode == Entry_Mode.View)
                        {
                            Load_ViewEntry();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //void ShowChild(Form ChildFrm, Window State, Boolean ToolFrm, Boolean CanMaximize, Entry_Mode Mode)
        //{
        //    try
        //    {
        //        ChildFrm.MdiParent = this;
        //        if (State == Window.Maximized)
        //        {
        //            ChildFrm.ControlBox = false;
        //            ChildFrm.WindowState = FormWindowState.Maximized;
        //        }
        //        else if (State == Window.Normal)
        //        {
        //            //ChildFrm.MaximizeBox = CanMaximize;
        //            //ChildFrm.WindowState = FormWindowState.Normal;
        //            //ChildFrm.StartPosition = FormStartPosition.Manual;
        //            ChildFrm.ControlBox = false;
        //            ChildFrm.WindowState = FormWindowState.Maximized;
        //        }
        //        else
        //        {
        //            ChildFrm.WindowState = FormWindowState.Minimized;
        //        }
        //        ChildFrm.Show();
        //        ChildFrm.Tag = String.Empty;
        //        if (ToolFrm == true)
        //        {
        //            ButtonEnabled(false);
        //            ChildFrm.Tag = null;
        //        }
        //        else
        //        {
        //            if (ToolFrm == false)
        //            {
        //                if (Mode == Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender))
        //                {
        //                    Load_NewEntry();
        //                }
        //                else if (Mode == Entry_Mode.Edit)
        //                {
        //                    Load_EditEntry();
        //                }
        //                else if (Mode == Entry_Mode.Delete)
        //                {
        //                    Load_DeleteEntry();
        //                }
        //                else if (Mode == Entry_Mode.View)
        //                {
        //                    Load_ViewEntry();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        Boolean Per_Print()
        {
            try
            {
                if (MyBase.GetData_InNumber("Socks_Permission_Master", "User_code", UserCode.ToString(), "Print") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Boolean Per_Preview()
        {
            try
            {
                if (MyBase.GetData_InNumber("Socks_Permission_Master", "User_code", UserCode.ToString(), "Preview") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        Boolean Per_Delete()
        {
            try
            {
                if (MyBase.GetData_InNumber("Socks_Permission_Master", "User_code", UserCode.ToString(), "Delete") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        Boolean Per_View()
        {
            try
            {
                if (MyBase.GetData_InNumber("Socks_Permission_Master", "User_code", UserCode.ToString(), "View") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        Boolean Per_Edit()
        {
            try
            {
                if (MyBase.GetData_InNumber("Socks_Permission_Master", "User_code", UserCode.ToString(), "Edit") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        Boolean Per_New()
        {
            try
            {
                if (MyBase.GetData_InNumber("Socks_Permission_Master", "User_code", UserCode.ToString(), "New") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean MdiWindowMinimized()
        {
            try
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        String Truncate_From(String Str1, String Str2)
        {
            try
            {
                if (Str1.Contains(Str2))
                {
                    return Str1.Substring(0, Convert.ToInt32(Str1.IndexOf(Str2)));
                }
                else
                {
                    return Str1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Replace_Caption()
        {
            try
            {
                //this.ActiveMdiChild.Text = this.ActiveMdiChild.Text.Replace(" - New ", "");
                //this.ActiveMdiChild.Text = this.ActiveMdiChild.Text.Replace(" - Edit ", "");
                //this.ActiveMdiChild.Text = this.ActiveMdiChild.Text.Replace(" - Delete ", "");
                //this.ActiveMdiChild.Text = this.ActiveMdiChild.Text.Replace(" - View ", "");
                this.ActiveMdiChild.Text = Truncate_From(this.ActiveMdiChild.Text, " - New ");
                this.ActiveMdiChild.Text = Truncate_From(this.ActiveMdiChild.Text, " - Edit ");
                this.ActiveMdiChild.Text = Truncate_From(this.ActiveMdiChild.Text, " - Delete ");
                this.ActiveMdiChild.Text = Truncate_From(this.ActiveMdiChild.Text, " - View ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Help_Text(String Str)
        {
            try
            {
                return;
                if (this.ActiveMdiChild != null)
                {
                    if (this.ActiveMdiChild.Tag != null)
                    {
                        if (_New == true || Edit == true)
                        {
                            StripLabel1.Text = Str;
                        }
                        else if (Delete)
                        {
                            StripLabel1.Text = "Entry is in Delete Mode Now ...!";
                        }
                        else if (View)
                        {
                            StripLabel1.Text = "Entry is in View Mode Now ...!";
                        }
                    }
                    else
                    {
                        StripLabel1.Text = Str;
                    }
                }
                else
                {
                    StripLabel1.Text = Str;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void MenuButton_Status(String Sql)
        {
            try
            {
                ButtonEnabled(false);
                Flag_Set();
                switch (Sql)
                {
                    case "Form":
                        _Form = true;
                        this.ActiveMdiChild.Controls["GBMain"].Enabled = false;
                        if (UserName.ToUpper() == "ADMIN" || UserName.ToUpper() == "MD")
                        {
                            ButtonEnabled_New(true);
                            ButtonEnabled_Edit(true);
                            ButtonEnabled_Delete(true);
                            ButtonEnabled_View(true);
                            ButtonEnabled_Cancel(false);
                        }
                        else
                        {
                            if (Rights_On_ActiveChild("A"))
                            {
                                ButtonEnabled_New(true);
                            }
                            else
                            {
                                ButtonEnabled_New(false);
                            }
                            if (Rights_On_ActiveChild("E"))
                            {
                                ButtonEnabled_Edit(true);
                            }
                            else
                            {
                                ButtonEnabled_Edit(false);
                            }
                            if (Rights_On_ActiveChild("D"))
                            {
                                ButtonEnabled_Delete(true);
                            }
                            else
                            {
                                ButtonEnabled_Delete(false);
                            }
                            if (Rights_On_ActiveChild("V"))
                            {
                                ButtonEnabled_View(true);
                            }
                            else
                            {
                                ButtonEnabled_View(false);
                            }
                            ButtonEnabled_Cancel(false);
                        }
                        Common_Help_Text("F2-New, F3-Edit, F4-Delete, F6-View, F11-Close");
                        break;
                    case "New":
                        _New = true;
                        if (UserName.ToUpper() != "ADMIN" && UserName.ToUpper() != "MD")
                        {
                            if (Rights_On_ActiveChild("A") == false)
                            {
                                MenuButton_Status("Form");
                                return;
                            }
                        }
                        this.ActiveMdiChild.Controls["GBMain"].Enabled = true;
                        ButtonEnabled_Save(true);
                        ButtonEnabled_Cancel(true);
                        Common_Help_Text_New();
                        break;
                    case "Edit":
                        Edit = true;
                        this.ActiveMdiChild.Controls["GBMain"].Enabled = true;
                        ButtonEnabled_Save(true);
                        ButtonEnabled_Cancel(true);
                        Common_Help_Text_Edit();
                        break;
                    case "Delete":
                        Delete = true;
                        if (Rights_On_ActiveChild("D"))
                        {
                            ButtonEnabled_DeleteConfirm(true);
                        }
                        else
                        {
                            ButtonEnabled_DeleteConfirm(false);
                        }
                        Common_Help_Text_Delete();
                        ButtonEnabled_Cancel(true);
                        break;
                    case "View":
                        View = true;
                        this.ActiveMdiChild.Controls["GBMain"].Enabled = true;
                        ButtonEnabled_Cancel(true);
                        if (UserName.ToUpper() == "ADMIN" || UserName.ToUpper() == "MD")
                        {
                            ButtonEnabled_Print(true);
                        }
                        else
                        {
                            if (Rights_On_ActiveChild("P"))
                            {
                                ButtonEnabled_Print(true);
                            }
                            else
                            {
                                ButtonEnabled_Print(false);
                            }
                        }
                        Common_Help_Text_View();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ButtonEnabled(Boolean Flag)
        {
            try
            {
                ButtonEnabled_New(Flag);
                ButtonEnabled_Edit(Flag);
                ButtonEnabled_Save(Flag);
                ButtonEnabled_Delete(Flag);
                ButtonEnabled_DeleteConfirm(Flag);
                ButtonEnabled_View(Flag);
                ButtonEnabled_Cancel(Flag);
                ButtonEnabled_Print(Flag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void MenuButton_ChildDispose()
        {
            try
            {
                if (this.ActiveMdiChild == null)
                {
                    MenuButton_Status("Form");  
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ButtonEnabled_Print(Boolean Flag)
        {
            try
            {
                printToolStripButton.Enabled = Flag;
                printToolStripMenuItem.Enabled = Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ButtonEnabled_View(Boolean Flag)
        {
            try
            {
                ViewtoolStripButton.Enabled = Flag;
                ViewtoolStripMenuItem.Enabled = Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ButtonEnabled_DeleteConfirm(Boolean Flag)
        {
            try
            {
                DeleteConfirmtoolStripButton.Enabled = Flag;
                DeleteConfirmtoolStripMenuItem.Enabled = Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ButtonEnabled_Delete(Boolean Flag)
        {
            try
            {
                DeletetoolStripButton.Enabled = Flag;
                DeletetoolStripMenuItem.Enabled = Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ButtonEnabled_Edit(Boolean Flag)
        {
            try
            {
                openToolStripButton.Enabled = Flag;
                openToolStripMenuItem.Enabled = Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ButtonEnabled_New(Boolean Flag)
        {
            try
            {
                newToolStripButton.Enabled = Flag;
                newToolStripMenuItem.Enabled = Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void CReport_Normal(Object Rpt, String Caption)
        {
            try
            {
                FrmCRViewer Frm = new FrmCRViewer();
                Frm.WindowState = FormWindowState.Maximized;
                Frm.Text = Caption;
                Frm.MdiParent = this;
                Frm.View(ref Rpt);
                Frm.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public void CReport_Normal_PDF(ref CrystalDecisions.CrystalReports.Engine.ReportDocument Rpt, String Caption, String PDF_FileName, Boolean Message_Flag)
        //{
        //    try
        //    {
        //        FrmCRViewer Frm = new FrmCRViewer();
        //        Frm.View_PDF(ref Rpt, PDF_FileName, Message_Flag);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public void CReport(ref CrystalDecisions.CrystalReports.Engine.ReportDocument Rpt, String Caption)
        {
            try
            {
                FrmCRViewer Frm = new FrmCRViewer();
                Frm.WindowState = FormWindowState.Maximized;
                Frm.Text = Caption;
                Frm.MdiParent = this;
                Frm.LoadCR(ref Rpt);
                Frm.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FormulaFill(ref CrystalDecisions.CrystalReports.Engine.ReportDocument Rpt, String FormulaFieldName, String FormulaValue)
        {
            try
            {
                Rpt.DataDefinition.FormulaFields[FormulaFieldName].Text = "'" + FormulaValue + "'";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ButtonEnabled_Save(Boolean Flag)
        {
            try
            {
                saveToolStripButton.Enabled = Flag;
                saveToolStripMenuItem.Enabled = Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Entries

        public void Flag_Set()
        {
            try
            {
                _New = false;
                Edit = false;
                Delete = false;
                View = false;
                _Form = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //public void Load_NewEntry()
        //{
        //    try
        //    {
        //        if (this.ActiveMdiChild != null)
        //        {
        //            if (this.ActiveMdiChild.Name != "FrmCRViewer")
        //            {
        //                MenuButton_Status("New");
        //                Replace_Caption();
        //                this.ActiveMdiChild.Text = this.ActiveMdiChild.Text + " - New ";
        //                Entry Frm = (Entry)this.ActiveMdiChild;
        //                Frm.Entry_New();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        Boolean Rights_On_ActiveChild(String Key)
        {
            try
            {
                if (MyBase.Get_RecordCount("PRojects_Permission_Master", "User_ID = " + UserCode + " and Menu_Name = '" + this.ActiveMdiChild.AccessibleName + "' and Rights like '%" + Key + "%'") > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void Load_NewEntry()
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    if (this.ActiveMdiChild.Name != "FrmCRViewer")
                    {
                        if (UserName.ToUpper() == "ADMIN" || UserName.ToUpper() == "MD" || Rights_On_ActiveChild("A"))
                        {
                            //Record_Count.Text = "0 / 0";
                            MenuButton_Status("New");
                            Replace_Caption();
                            this.ActiveMdiChild.Text = this.ActiveMdiChild.Text + " - New ";
                            Entry Frm = (Entry)this.ActiveMdiChild;
                            Frm.Entry_New();
                        }
                        else
                        {
                            if (Rights_On_ActiveChild("E"))
                            {
                                //Record_Count.Text = "0 / 0";
                                MenuButton_Status("Edit");
                                Replace_Caption();
                                this.ActiveMdiChild.Text = this.ActiveMdiChild.Text + " - Edit ";
                                Entry Frm = (Entry)this.ActiveMdiChild;
                                Frm.Entry_Edit();
                            }
                            else if (Rights_On_ActiveChild("D"))
                            {
                                //Record_Count.Text = "0 / 0";
                                MenuButton_Status("Delete");
                                Replace_Caption();
                                this.ActiveMdiChild.Text = this.ActiveMdiChild.Text + " - Delete ";
                                Entry Frm = (Entry)this.ActiveMdiChild;
                                Frm.Entry_Delete();
                            }
                            else if (Rights_On_ActiveChild("V"))
                            {
                                //Record_Count.Text = "0 / 0";
                                MenuButton_Status("View");
                                Replace_Caption();
                                this.ActiveMdiChild.Text = this.ActiveMdiChild.Text + " - View ";
                                Entry Frm = (Entry)this.ActiveMdiChild;
                                Frm.Entry_View();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Rights ..!", "Vaahini");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Carry_Over_DBF()
        {
            try
            {
                //if (CompCode == 2)
                //{
                //    if (MyBase.Get_RecordCount("GroupMas", "Company_Code = " + CompCode) == 0)
                //    {
                //        MyBase.Execute("INSERT INTO GROUPMAS(GROUPCODE, GROUPNAME, GROUPUNDER, GROUPRESERVED, DEBIT, CREDIT, COMPANY_CODE, YEAR_cODE, BREAKUP) select RTRIM(GRPCODE), RTRIM(GNAME), RTRIM(GGRPCODE), RTRIM(GGRPCODE), RTRIM(DEBIT), RTRIM(CREDIT), " + CompCode + ", '" + YearCode + "', 'N' from openrowset('MSDASQL','DRIVER=Microsoft Visual Foxpro Driver;sourcedb=F:\\Vaahini\\ramac;sourcetype=DBF','select recno() Slno1,* from GRP077')");
                //    }
                //    if (MyBase.Get_RecordCount("ledger_Master", "Company_Code = " + CompCode) == 0)
                //    {
                //        MyBase.Execute("insert into ledger_Master (LEdger_Code, ledger_Name, ledger_title, ledger_inPrint, ledger_group_Code, ledger_odebit, ledger_Ocredit, Company_Code, year_Code, Breakup) select ldcode, prtclr, 'M/S.', Prtclr, Group1, odebit, ocredit, " + CompCode + ", '" + YearCode + "', 'N' from ctb077");
                //    }
                //    if (MyBase.Get_RecordCount("Voucher_Master", "Company_Code = " + CompCode) == 0)
                //    {
                //        MyBase.Update_Unique_Code_in_ENT();
                //        MyBase.Execute("insert into voucher_master(vcode, vmode, vno, vdate, remarks, user_date, company_Code, year_Code) Select distinct vcode, mode, vno, date, narr, date, " + CompCode + ", '" + YearCode + "' from ent077t");
                //    }
                //    if (MyBase.Get_RecordCount("Voucher_Details", "Company_Code = " + CompCode) == 0)
                //    {
                //        MyBase.Execute("insert into voucher_details Select vcode, date, 1, toby, ldcode, debit, credit, refdoc, " + CompCode + ", '" + YearCode + "', ldledger from ent077t");
                //        //MyBase.Execute("insert into voucher_details Select vcode, date, 1, toby, ldcode, Credit, Debit, refdoc, " + CompCode + ", '" + YearCode + "', ldledger from ent077t");
                //    }
                //}
                MyBase.Execute ("Update Voucher_Details set Narration  = '-' where narration is null");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Load_EditEntry()
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    if (this.ActiveMdiChild.Name == "FrmVoucherEntry" || this.ActiveMdiChild.Name == "FrmVoucherEntry_Single")
                    {
                        if (Vouch_Edit_Lock == false)
                        {
                            MenuButton_Status("Edit");
                            Replace_Caption();
                            this.ActiveMdiChild.Text = this.ActiveMdiChild.Text + " - Edit ";
                            Entry Frm = (Entry)this.ActiveMdiChild;
                            Frm.Entry_Edit();
                        }
                        else
                        {
                            MessageBox.Show("Entry Doesn't Have Edit Mode ...!", "Vaahini");
                            Load_ViewEntry();
                        }
                    }
                    else
                    {
                        MenuButton_Status("Edit");
                        Replace_Caption();
                        this.ActiveMdiChild.Text = this.ActiveMdiChild.Text + " - Edit ";
                        Entry Frm = (Entry)this.ActiveMdiChild;
                        Frm.Entry_Edit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Load_SaveEntry()
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    if (this.ActiveMdiChild.Text.Contains("- New") ==  true || this.ActiveMdiChild.Text.Contains("- Edit") == true)
                    {
                        if (MessageBox.Show("Sure To Save ...!", "Save ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            Time_Update();
                            if (_New == true || Edit == true)
                            {
                                Entry Frm = (Entry)this.ActiveMdiChild;
                                Frm.Entry_Save();
                                if (_New == true)
                                {
                                    if (Save_Error != true)
                                    {
                                        Load_NewEntry();
                                    }
                                }
                                else if (Edit == true)
                                {
                                    if (this.ActiveMdiChild.Name == "FrmCRViewer")
                                    {
                                        return;
                                    }
                                    if (Save_Error != true)
                                    {
                                        if (this.ActiveMdiChild.Name == "FrmVoucherEntry" || this.ActiveMdiChild.Name == "FrmVoucherEntry_Single")
                                        {
                                            if (Direct_voucher)
                                            {
                                                this.ActiveMdiChild.Close();
                                                return;
                                            }
                                            else
                                            {
                                                Load_EditEntry();
                                            }
                                        }
                                        else
                                        {
                                            Load_EditEntry();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Illegal Mode to Save ...!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Illegal Mode to Save ...!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Return_Datasource(ref DataTable Dt)
        {
            ContainerControl Ct;
            try
            {
                Ct = (ContainerControl)this.ActiveMdiChild;
                Set_Datasource(Ct, ref Dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Set_Datasource(ContainerControl Cr, ref DataTable Dt)
        {
            try
            {
                foreach (Control ct in Cr.Controls)
                {
                    if (ct is System.Windows.Forms.GroupBox || ct is Panel || ct is FlowLayoutPanel || ct is TabControl)
                    {
                        foreach (Control Co in ct.Controls)
                        {
                            if (Co is DataGridView)
                            {
                                DataGridView Obj;
                                Obj = (DataGridView)Co;
                                Obj.DataSource = MyBase.V_DataTable(ref Dt);
                                MyBase.V_DataGridView(ref Obj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Load_DeleteEntry()
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    if (this.ActiveMdiChild.Name == "FrmVoucherEntry" || this.ActiveMdiChild.Name == "FrmVoucherEntry_Single")
                    {
                        if (Vouch_Edit_Lock == false)
                        {
                            MenuButton_Status("Delete");
                            Replace_Caption();
                            this.ActiveMdiChild.Text = this.ActiveMdiChild.Text + " - Delete ";
                            Entry Frm = (Entry)this.ActiveMdiChild;
                            Frm.Entry_Delete();
                        }
                        else
                        {
                            MessageBox.Show("Entry Doesn't Have Delete Mode ...!", "Vaahini");
                            Load_ViewEntry();
                        }
                    }
                    else
                    {
                        MenuButton_Status("Delete");
                        Replace_Caption();
                        this.ActiveMdiChild.Text = this.ActiveMdiChild.Text + " - Delete ";
                        Entry Frm = (Entry)this.ActiveMdiChild;
                        Frm.Entry_Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public void Load_DeleteConfirmEntry()
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    if (MessageBox.Show("Sure To Delete ...!", "Delete ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (Delete)
                        {
                            Entry Frm = (Entry)this.ActiveMdiChild;
                            Frm.Entry_Delete_Confirm();
                        }
                        else
                        {
                            MessageBox.Show("Illegal Mode To Delete ...!");
                        }
                    }
                    else
                    {
                        Load_DeleteEntry();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Load_PrintEntry()
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    if (this.ActiveMdiChild.Name == "FrmSalesInvoiceNew" || this.ActiveMdiChild.Name == "FrmCopyQuotation" || this.ActiveMdiChild.Name == "FrmDC" || this.ActiveMdiChild.Name == "FrmInvoice" || this.ActiveMdiChild.Name == "FrmCashsales" || this.ActiveMdiChild.Name == "FrmVoucherEntry" || this.ActiveControl.Name == "FrmSalesEntry" || this.ActiveControl.Name == "FrmSocksYarnPOEntry" || this.ActiveControl.Name == "FrmSocksTrimsPOEntry" || this.ActiveControl.Name == "FrmVehicleCarBooking" || this.ActiveControl.Name == "FrmVehicleBookingEntry")
                    {
                        Entry Frm = (Entry)this.ActiveMdiChild;
                        Frm.Entry_Print();
                    }
                    else
                    {
                        if (CompName.ToUpper().Contains("DHANA"))
                        {
                            if (MessageBox.Show("Sure To Print ...!", "Print ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            {
                                Entry Frm = (Entry)this.ActiveMdiChild;
                                Frm.Entry_Print();
                            }
                            else
                            {
                                Load_ViewEntry();
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("Sure To Print ...!", "Print ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                Entry Frm = (Entry)this.ActiveMdiChild;
                                Frm.Entry_Print();
                            }
                            else
                            {
                                Load_ViewEntry();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Load_ViewEntry1(String TblName, String Condition)
        {
            try
            {
                MenuButton_Status("View");
                
                Replace_Caption();
                this.ActiveMdiChild.Text = this.ActiveMdiChild.Text + " - View ";
                Entry Frm = (Entry)this.ActiveMdiChild;
                Frm.Entry_View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Load_ViewEntry()
        {
            try
            {
                MenuButton_Status("View");
                Replace_Caption();
                this.ActiveMdiChild.Text = this.ActiveMdiChild.Text + " - View ";
                Entry Frm = (Entry)this.ActiveMdiChild;
                Frm.Entry_View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region ProgressBar

        public void Progress_visible(Boolean Flag)
        {
            try
            {
                toolStripProgressBar2.Visible = Flag;
                if (Flag == true)
                {
                    this.Cursor = Cursors.WaitCursor;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Progress_Max(int val)
        {
            try
            {
                toolStripProgressBar2.Maximum = val;
                toolStripProgressBar2.Value = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddProgress_Value(int Val)
        {
            try
            {
                if (toolStripProgressBar2.Value == 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    toolStripProgressBar2.Value = toolStripProgressBar2.Value + Val;
                }
                else
                {
                    toolStripProgressBar2.Value = toolStripProgressBar2.Value + Val;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public MDIMain()
        {
            InitializeComponent();
        }

        public void Child_Active(Form Frm)
        {
            try
            {
                if (Frm.Tag != null)
                {
                    if (Frm.Tag.ToString() == "New")
                    {
                        MenuButton_Status("New");
                    }
                    else if (Frm.Tag.ToString() == "Edit")
                    {
                        MenuButton_Status("Edit");
                    }
                    else if (Frm.Tag.ToString() == "Delete")
                    {
                        MenuButton_Status("Delete");
                    }
                    else if (Frm.Tag.ToString() == "View")
                    {
                        MenuButton_Status("View");
                    }
                    else if (Frm.Tag.ToString() == "_Form")
                    {
                        MenuButton_Status("Form");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Child_Deactive(Form Frm)
        {
            try
            {
                if (_New)
                {
                    Frm.Tag = "New";
                }
                else if (Edit)
                {
                    Frm.Tag = "Edit";
                }
                else if (Delete)
                {
                    Frm.Tag = "Delete";
                }
                else if (View)
                {
                    Frm.Tag = "View";
                }
                else if (_Form)
                {
                    Frm.Tag = "_Form";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Can_Delete(Int64 Vcode, int Compcode, String YearCode)
        {
            try
            {
                if (MyBase.Get_RecordCount("Ledger_breakup", "Ref = '" + Vcode + "' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and Amount_Cl > 0") > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void GRnDate()
        {
            try
            {
                if (MyBase.Check_TableField("GSN_Details", "GRN_Date") == false)
                {
                    MyBase.Add_NewField("GSN_Details", "GRN_No", "Number(10)");
                    MyBase.Add_NewField("GSN_Details", "GRN_Date", "Date");
                }
                if (MyBase.Check_TableField("GSN_Acceptance_Details", "GRN_Date") == false)
                {
                    MyBase.Add_NewField("GSN_Acceptance_Details", "GRN_No", "Number(10)");
                    MyBase.Add_NewField("GSN_Acceptance_Details", "GRN_Date", "Date");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lock_Table_Creation()
        {
            try
            {
                if (MyBase.Check_Table("Vlock") == false)
                {
                    MyBase.Execute("create table Vlock (Name varchar2(20), Lock_Status number(1))");
                    MyBase.Execute("insert into Vlock  values ('CASHBILL_MASTER', 0)");
                    MyBase.Execute("insert into Vlock  values ('Socks_User_Master', 0)");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Load_MenuItems()
        {
            ToolStripMenuItem M2;
            ToolStripMenuItem M4;
            try
            {
                foreach (object Obj in menuStrip.Items)
                {
                    if (Obj.GetType().Equals(typeof(ToolStripMenuItem)))
                    {
                        ToolStripMenuItem M = (ToolStripMenuItem)Obj;
                        if (M.Name == "windowsMenu" || M.Name == "fileMenu")
                        {
                        }
                        else if (M.Name == "CmbMenuList")
                        {
                        }
                        else
                        {
                            MyBase.Menu_save(M.Text, M.Name, M.Text);
                            foreach (ToolStripItem M1 in M.DropDownItems)
                            {
                                if (M1 is ToolStripMenuItem)
                                {
                                    M2 = (ToolStripMenuItem)M1;
                                    MyBase.Menu_save(M2.Text, M2.Name, M.Text);
                                    foreach (ToolStripItem M3 in M2.DropDownItems)
                                    {
                                        if (M3 is ToolStripMenuItem)
                                        {
                                            M4 = (ToolStripMenuItem)M3;
                                            MyBase.Menu_save(M3.Text, M3.Name, M2.Text);
                                            foreach (ToolStripItem M5 in M4.DropDownItems)
                                            {
                                                if (M5 is ToolStripMenuItem)
                                                {
                                                    MyBase.Menu_save(M5.Text, M5.Name, M4.Text);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //void Load_MenuItems()
        //{
        //    ToolStripMenuItem M2;
        //    try
        //    {
        //        foreach (ToolStripMenuItem M in menuStrip.Items)
        //        {
        //            if (M.Name == "windowsMenu" || M.Name == "fileMenu")
        //            {
        //            }
        //            else
        //            {
        //                MyBase.Menu_save(M.Text, M.Name, M.Text);
        //                foreach (ToolStripItem M1 in M.DropDownItems)
        //                {
        //                    if (M1 is ToolStripMenuItem)
        //                    {
        //                        M2 = (ToolStripMenuItem)M1;
        //                        MyBase.Menu_save(M2.Text, M2.Name, M.Text);
        //                        foreach (ToolStripItem M3 in M2.DropDownItems)
        //                        {
        //                            if (M3 is ToolStripMenuItem)
        //                            {
        //                                MyBase.Menu_save(M3.Text, M3.Name, M2.Text);
        //                            }
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        Boolean isPermitted(String MenuCname)
        {
            try
            {
                if (MyBase.GetData_InString("Socks_Permission_Master", "Menu_Code", MyBase.GetData_InString("Menu_Master", "Menu_Cname", MenuCname.ToUpper(), "Menu_Code").ToUpper(), "Status") == "True")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 Get_Vno(Int16 Vmode)
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("select Isnull(max(CAST(vno as int)), 0) + 1 Vno from voucher_Master where Company_Code = " + CompCode + " and Year_Code = '" + YearCode + "' and VMode = " + Vmode + " and PATINDEX('%[^0-9]%', vno) = 0 ", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(Dt.Rows[0][0]);
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        public void MenuItems_False()
        {
            DataTable Dt = new DataTable();
            DataTable Dt1 = new DataTable();
            DataTable Dt2 = new DataTable();
            ToolStripMenuItem T;
            ToolStripMenuItem T1;
            ToolStripMenuItem T2;
            try
            {
                MyBase.Load_Data("Select Menu_Code, Menu_Cname from menu_Master where under = 'Main' order by menu_Code", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    T = (ToolStripMenuItem)menuStrip.Items[Dt.Rows[i]["Menu_Cname"].ToString()];
                    T.Visible = false;
                    MyBase.Load_Data("Select Menu_Code, Menu_Cname from menu_Master where Under = '" + Dt.Rows[i]["Menu_Code"].ToString() + "' order by menu_Code", ref Dt1);
                    for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                    {
                        T1 = (ToolStripMenuItem)T.DropDownItems[Dt1.Rows[j]["Menu_Cname"].ToString()];
                        T1.Visible = false;
                        MyBase.Load_Data("Select Menu_Code, Menu_Cname from menu_Master where Under = '" + Dt1.Rows[j]["Menu_Code"].ToString() + "' order by menu_Code", ref Dt2);
                        for (int k = 0; k <= Dt2.Rows.Count - 1; k++)
                        {
                            T2 = (ToolStripMenuItem)T1.DropDownItems[Dt2.Rows[k]["Menu_Cname"].ToString()];
                            T2.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void MenuItems_True()
        {
            DataTable Dt = new DataTable();
            DataTable Dt1 = new DataTable();
            DataTable Dt2 = new DataTable();
            ToolStripMenuItem T;
            ToolStripMenuItem T1;
            ToolStripMenuItem T2;
            try
            {
                MyBase.Load_Data("Select Menu_Code, Menu_Cname from menu_Master where under = 'Main' and Menu_Code in (Select Menu_Code from Socks_Permission_Master where User_Code = " + UserCode + ") order by menu_Code", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    T = (ToolStripMenuItem)menuStrip.Items[Dt.Rows[i]["Menu_Cname"].ToString()];
                    T.Visible = true;
                    MyBase.Load_Data("Select Menu_Code, Menu_Cname from menu_Master where Under = '" + Dt.Rows[i]["Menu_Code"].ToString() + "' and Menu_Code in (Select Menu_Code from Socks_Permission_Master where User_Code = " + UserCode + ") order by menu_Code", ref Dt1);
                    for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                    {
                        T1 = (ToolStripMenuItem)T.DropDownItems[Dt1.Rows[j]["Menu_Cname"].ToString()];
                        T.Visible = true;
                        T1.Visible = true;
                        MyBase.Load_Data("Select Menu_Code, Menu_Cname from menu_Master where Under = '" + Dt1.Rows[j]["Menu_Code"].ToString() + "' and Menu_Code in (Select Menu_Code from Socks_Permission_Master where User_Code = " + UserCode + ") order by menu_Code", ref Dt2);
                        for (int k = 0; k <= Dt2.Rows.Count - 1; k++)
                        {
                            T2 = (ToolStripMenuItem)T1.DropDownItems[Dt2.Rows[k]["Menu_Cname"].ToString()];
                            T.Visible = true;
                            T1.Visible = true;
                            T2.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Menu_Settings()
        {
            DataTable Dt = new DataTable();
            try
            {
                //MyBase.Load_Data ("Select Menu_Code, 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void MenuMaster()
        {
            try
            {
                //if (MyBase.Check_Table("Menu_Master"))
                //{
                //   if (MyBase.Check_TableField("Menu_Master", "Under") == false)
                //   {
                //        MyBase.Execute("Drop table Menu_Master");
                //   }
                //}
                //if (MyBase.Check_Table("Menu_Master") == false)
                //{
                //    MyBase.Execute("Create Table Menu_Master (Menu_Code numerIC(4), Menu_Name varchar(50), Menu_CName varchar(100), Under varchar(10))");
                //}
                //else
                //{
                //    MyBase.Execute("Delete from Menu_Master");
                //    Load_MenuItems();
                //}

                if (MyBase.Check_Table("Projects_Menu_Master_New") == false)
                {
                    MyBase.Execute("Create Table Projects_Menu_Master_New (Menu_ID int Identity, Menu_Name varchar(100), Menu_CName varchar(500) unique Not null, Under_Menu_CName varchar(500))");
                }
                MyBase.Execute("Truncate Table Projects_Menu_Master_New");
                Load_MenuItems();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ItemIDUpdationsBranch()
        {
            DataTable Dt = new DataTable();
            String Str;
            try
            {
                if (CompCode != 6)
                {
                    MyBase.Execute("Create table GSN_Details_Backup as select * from GSN_Details");
                    MyBase.Execute("Create table GSN_Acceptance_Details_Backup as select * from GSN_Acceptance_Details");
                    MessageBox.Show("Backup");

                    MyBase.Execute("Create table GSN_Details_temp16 as select g1.*, 'No' as Status from GSN_Details g1");
                    MyBase.Execute("Create table GSN_Acceptance_Details_temp16 as select g1.*, 'No' as Status from GSN_Acceptance_Details g1");
                    MessageBox.Show("temp");

                    Str = "Select Item_Slno, Item_ID, Item_ID_old from item_master order by Item_ID_Old";
                    MyBase.Load_Data(Str, ref Dt);
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        MyBase.Execute("Update GSN_Acceptance_Details_temp16 Set Item_ID = '" + Dt.Rows[i]["Item_Slno"].ToString() + "', Status = 'Y' where item_ID = '" + Dt.Rows[i]["Item_ID_Old"].ToString() + "' and Status = 'No'");
                        MyBase.Execute("Update GSN_Details_temp16 Set Item_ID = '" + Dt.Rows[i]["Item_Slno"].ToString() + "', Status = 'Y' where item_ID = '" + Dt.Rows[i]["Item_ID_Old"].ToString() + "' and Status = 'No'");
                    }
                    MessageBox.Show ("Updatipns");

                    MyBase.Execute("alter table GSN_Details rename to GSN_Details_Backup1");
                    MyBase.Execute("alter table GSN_Acceptance_Details rename to GSN_Acceptance_Details_Backup1");

                    MyBase.Execute("Alter table GSN_Details_temp16 rename to GSN_Details");
                    MyBase.Execute("Alter table GSN_Acceptance_Details_temp16 rename to GSN_Acceptance_Details");
                    MessageBox.Show("Ok");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void DBF_TableUpdations()
        {
            try
            {
                return;
                DirectoryInfo DI = new DirectoryInfo(System.Windows.Forms.Application.StartupPath);
                FileInfo[] F1 = DI.GetFiles("*.DBF");
                if (F1.Length > 0)
                {
                    foreach (FileInfo F in F1)
                    {
                        if (F.Name.ToUpper() != "QUD091" && F.Name.ToUpper() != "DLD091")
                        {
                            MyBase.UpdateDBF(F.Name.ToUpper().Replace(".DBF", ""));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Tally()
        {
            DataTable Dt = new DataTable();
            try
            {
                //MyBase.Load_Data("select GroupName gname, '' alias, 'Direct Expenses' AS Parent, 290 from grp0896 where gname not in (select upper(groupName) from group_xml)", ref Dt);
                //MyBase.Write_Xml_Group(Company, ref Dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Carry_DBF_Sql()
        {
            try
            {
                MyBase.Execute_Qry("select c1.*, c2.LdCode LDcode1, c2.add1, c2.add2, c2.add3, c2.add4, c2.phone, c2.Phone1 fax, c2.phone2 email, c2.tngst, ' ' rct, c2.cst, c2.contact person, c2.agcode, c2.slno, 'M/S.' prfx, c2.tinno from ctb077 c1 left join cad077 c2 on c1.ldcode = c2.ldcode ", "Led");
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Carry_DBF_SQL1()
        {
            try
            {
                if (MyBase.DBFCn_Check_Table ("CTB091"))
                {
                    if (MyBase.DBFCn_Check_Table("Led_master") == false)
                    {
                        MyBase.DBFCN_Delete("select c1.*, c2.Ledcode Ledcode1, c2.LdCode LDcode1, c2.name, c2.add1, c2.add2, c2.add3, c2.add4, c2.phone, c2.fax, c2.email, c2.tngst, c2.rct, c2.cst, c2.person, c2.agcode, c2.slno, c2.prfx, c2.tinno into Led_Master from ctb091 c1 left join cad091 c2 on c1.ldcode = c2.ldcode and c1.ledcode = c2.ledcode");
                        MyBase.DBFCN_Delete("delete from led_master where group1 = ''");
                        if (MyBase.Check_Table("Ledger_Master") == false)
                        {
                            MyBase.Execute("create table Ledger_Master (Ledger_Code Numeric(10), Ledger_Name varchar(150), Ledger_title varchar(20), Ledger_InPrint varchar(150), Ledger_Group_Code numeric(5), Ledger_ODebit Numeric(10,2), Ledger_OCredit Numeric(10,2), Ledger_CLimit numeric(10,2),Ledger_Address varchar(4000), Ledger_Area_Code numeric(5), Ledger_Phone Varchar(100), Ledger_Fax varchar(100), Ledger_email varchar(100), Ledger_Website varchar(100), ledger_TIN varchar(50), Ledger_CST varchar(50))");
                            MyBase.Execute("create table Ledger_Contact (Ledger_Code Numeric(10), Slno int, Person varchar(100), Department_Code int, Designation varchar(100), Phone varchar(100))");
                        }
                        //MyBase.Execute("Insert into Ledger_Master select Ldcode, prtclr, prfx, prtclr, group1, Odebit, Ocredit, 0, Convert(varchar(300), convert(varchar(100), add1) + convert(varchar(100), add2) + convert(varchar(100), add3)+ convert(varchar(100), add4)) as address, areacode, Phone, fax, email, Null as website, tinno, cst from " + MyBase.DBF_SQL_DB + ".dbo.led_master ");
                        //MyBase.Execute("Insert into Ledger_Master select Ldcode, prtclr, prfx, prtclr, group1, Odebit, Ocredit, 0, Convert(varchar(300), isnull(add1,'') + isnull(add2,'') + isnull(add3,'') + isnull(add4,'')) as address, areacode, Phone, fax, email, Null as website, tinno, cst from " + MyBase.DBF_SQL_DB + ".dbo.led_master ");
                        MyBase.Execute("Insert into Ledger_Master select Ldcode, prtclr, prfx, prtclr, group1, Odebit, Ocredit, 0, Convert(varchar(300), isnull(add1,'') + CHAR(13) + isnull(add2,'') + CHAR(13) + isnull(add3,'') + CHAR(13) + isnull(add4,'')) as address, areacode, Phone, fax, email, Null as website, tinno, cst from " + MyBase.DBF_SQL_DB + ".dbo.led_master ");
                        MyBase.Execute("Insert into Ledger_Contact select LDCode, 1, Person, null, null, null from " + MyBase.DBF_SQL_DB + ".dbo.Led_master");
                        MyBase.Execute("UPDATE LEDGER_MASTER SET LEDGER_INPRINT = L2.NAME FROM LEDGER_MASTER L1, " + MyBase.DBF_SQL_DB + ".dbo.LED_MASTER L2 WHERE L1.LEDGER_CODE = L2.LDCODE AND L2.NAME IS NOT NULL ");
                    }
                }
                if (MyBase.DBFCn_Check_Table("Area"))
                {
                    if (MyBase.Check_Table("Area_Master") == false)
                    {
                        MyBase.Execute("Create table Area_Master (Area_Code Int, Area_Name varchar(100), Area_STD varchar(25))");
                        MyBase.Execute("insert into Area_Master select AreaCode, AreaName, Stdcode from " + MyBase.DBF_SQL_DB + ".dbo. Area");
                    }
                }
                if (MyBase.DBFCn_Check_Table("GRP091"))
                {
                    if (MyBase.Check_Table("Group_Master") == false)
                    {
                        MyBase.Execute("Create table Group_Master (Group_Code bigint, Group_Name varchar(200), Group_Under Bigint)");
                        MyBase.Execute("Insert into Group_master select Grpcode, Gname, GGrpcode from " + MyBase.DBF_SQL_DB + ".dbo.grp091");
                    }
                }
                if (MyBase.DBFCn_Check_Table("UOM"))
                {
                    if (MyBase.Check_Table("UOM_Master") == false)
                    {
                        MyBase.Execute("Create Table UOM_Master (UOM Varchar(25), Title Varchar(5))");
                        MyBase.Execute("Insert into UOM_Master Select UOM, Prfx from " + MyBase.DBF_SQL_DB + ".dbo.UOM");
                        MyBase.Add_NewField("UOM_Master", "UOM_Code", "Numeric(4)");
                        MyBase.Update_Code("UOM_Master", "UOM", "UOM_Code");
                    }
                }
                if (MyBase.DBFCn_Check_Table("Product"))
                {
                    if (MyBase.Check_Table("Category_Master") == false)
                    {
                        MyBase.Execute("Create table Category_master (Catg_Code Int, Catg_Name varchar(250))");
                        MyBase.Execute("Insert into Category_master Select Prcode, Prname from " + MyBase.DBF_SQL_DB + ".dbo.Product");
                    }
                }
                if (MyBase.DBFCn_Check_Table("ItHead"))
                {
                    if (MyBase.Check_Table("Product_SubGroup_master") == false)
                    {
                        MyBase.Execute("Create table Product_SubGroup_master (Product_Group_Code int, Product_Group_name varchar(250), Product_Group_desc varchar(250))");
                        MyBase.Execute("Insert into Product_SubGroup_master Select headcode, headname, desc1 from " + MyBase.DBF_SQL_DB + ".dbo.ITHead");
                    }
                }
                if (MyBase.DBFCn_Check_Table("Manuf"))
                {
                    if (MyBase.Check_Table("Manufacturer_Master") == false)
                    {
                        MyBase.Execute("Create table Manufacturer_Master (Manuf_Code varchar(5), Manufacturer_Name varchar(250), Discount numeric(10,2))");
                        MyBase.Execute("Insert into Manufacturer_master Select Manuf, ManuName, Dper from " + MyBase.DBF_SQL_DB + ".dbo.Manuf");
                        MyBase.Add_NewField("Manufacturer_Master", "Manufacturer_Code", "Numeric(4)");
                        MyBase.Update_Code("Manufacturer_Master", "Manuf_Code", "Manufacturer_Code");
                    }
                }
                if (MyBase.DBFCn_Check_Table("itm091"))
                {
                    if (MyBase.Check_Table("Product_Master") == false)
                    {
                        MyBase.Execute("create table Product_Master (item_Code Bigint, Item_Name varchar(1000), catg_Code int, Product_Group_Code Int, Manuf_Code varchar(5), UOM_name varchar(25), Opening_Qty numeric(10,2), Opening_Rate numeric(10,2), Opening_Value numeric(10,2), ReOrder_level numeric(10,2), MaxQty numeric(10,2), Selling_Rate numeric(10,2), Item_Details ntext)");
                        //MyBase.Execute("insert into Product_master select ItemCode, ItemName, Itcode, Prcode, manuf, uom, oQty, (case when oqty = 0 then 0 else ovalue/Oqty end) orate, Ovalue, rlevel, maxqty, srate, itemdetl from " + MyBase.DBF_SQL_DB + ".dbo.itm091");
                        if (MyBase.Check_Table("Prod1"))
                        {
                            MyBase.Execute("Drop table prod1");
                        }
                        MyBase.Execute("select ItemCode, ITEMNAME, DESC1, Itcode, Prcode, manuf, uom, oQty, (case when oqty = 0 then 0 else ovalue/Oqty end) orate, Ovalue, rlevel, maxqty, srate, itemdetl into prod1 from " + MyBase.DBF_SQL_DB + ".dbo.itm091");
                        MyBase.Execute("update prod1 set itemname = itemname + char(13) + desc1 where desc1 is not null");
                        //MyBase.Execute("insert into Product_master select ItemCode, ITEMNAME, Itcode, Prcode, manuf, uom, oQty, (case when oqty = 0 then 0 else ovalue/Oqty end) orate, Ovalue, rlevel, maxqty, srate, itemdetl from " + MyBase.DBF_SQL_DB + ".dbo.itm091");
                        MyBase.Execute("insert into Product_master select ItemCode, ITEMNAME, Itcode, Prcode, manuf, uom, oQty, (case when oqty = 0 then 0 else ovalue/Oqty end) orate, Ovalue, rlevel, maxqty, srate, itemdetl from Prod1");
                        MyBase.Execute("Drop table Prod1");
                    }
                }
                if (MyBase.DBFCn_Check_Table("Qut091"))
                {
                    if (MyBase.Check_Table("Quotation_Master") == false)
                    {
                        MyBase.Execute("Create table Quotation_master (Quotation_Code Bigint, Quotation_Date datetime, Ledger_Code int, Aboveto_add Varchar(300), Department_Code Int, Enquiry_no varchar(100), Enquiry_Date datetime, Discount Varchar(10), Terms ntext)");
                        MyBase.Execute("insert into Quotation_master Select Qtno, QtDate, PCode, isnull(adr1,'') + isnull(adr2,'') As Adr, null Deptcode, enqno, enqdate, discPrint, desc1 + desc2 + desc3 + desc4 + desc5 + remark1 + remark2 + remark3 + desc6 + desc7 + desc8 + desc9 + desc10 as Terms from " + MyBase.DBF_SQL_DB + ".dbo.qut091");
                        MyBase.Execute("Create table Quotation_Details (Quotation_Code Bigint, Quotation_Date datetime, Slno Int, I_Slno Varchar(5), Item_Code Bigint, Item_Printing varchar(1000), Desc_Print varchar(1), UOM varchar(20), Selling_Rate Numeric(10,2), Rate_Add Numeric(10,2), Rate Numeric(10,2), Disc Numeric(10,2),  Tax Numeric(10,2), NetRate Numeric(10,2), Item_Details ntext)");
                        MyBase.Execute("Insert into Quotation_Details select rtrim(qtno), null QtDate, Slno1, rtrim(sno) I_Slno, rtrim(itemcode), rtrim(itemdesc), rtrim(rdesc), rtrim(uom), rtrim(srate), rtrim(dper), rtrim(rate), rtrim(discp), rtrim(tax1p), rtrim(ratep), itemdetl from openrowset('MSDASQL','DRIVER=Microsoft Visual Foxpro Driver;sourcedb=" + System.Windows.Forms.Application.StartupPath + ";sourcetype=DBF','select recno() Slno1,* from qud091')");
                        MyBase.Execute("update quotation_Details set Quotation_Date = q1.Quotation_Date from Quotation_Master q1, Quotation_Details q2 where q1.Quotation_Code = q2.Quotation_Code");
                        //MyBase.Execute("insert into Quotation_Details select qtno, qtdate, null Slno, sno I_Slno, itemcode, itemdesc, rdesc, uom, srate, dper, rate, discp, tax1p, ratep, itemdetl from " + MyBase.DBF_SQL_DB + ".dbo.qud091 ");
                    }
                }
                if (MyBase.DBFCn_Check_Table("DLC091"))
                {
                    if (MyBase.Check_Table("DC_Master") == false)
                    {
                        MyBase.Execute("create table DC_master (DCNo numeric(10), DCdate Datetime, Ledger_Code int, Above_Add varchar(200), Due_Days int, SalesOrder varchar(100), DCType varchar(100), EnQNo varchar(100), EnqDate datetime, LRNO varchar(100), LRdate datetime, DespThro varchar(100), Freight varchar(10),  narration varchar(2000), netamount float)");
                        MyBase.DBFCN_Delete("update dlc091 set Splinst2 = isnull(Splinst2, '') + Char(13) + splinst3 where splinst3 is not null");
                        MyBase.DBFCN_Delete("update dlc091 set Splinst = isnull(Splinst,'') + Char(13) + splinst2 where splinst2 is not null");
                        MyBase.Execute("insert into DC_Master select dcno, dcdate, Pcode, gpadd1, dueper, sono, dctype, enqno, enqdate, lrno, lrdate, desp, freight, Splinst, netamt from " + MyBase.DBF_SQL_DB + ".dbo.DLC091");
                    }
                }
                if (MyBase.DBFCn_Check_Table("DLD091"))
                {
                    if (MyBase.Check_Table("DC_Details") == false)
                    {
                        MyBase.Execute("create table DC_Details (DCNo numeric(10), DCdate Datetime, I_SLno int, SLno varchar(10), Item_Code numeric(10), Item_Printing varchar(500), item_desc ntext, Desc1 varchar(1), Qty float, paran_Text varchar(500), Paran varchar(1), rate float, Dis float, Amount float, tax_per float, tax_Code int, invcan varchar(5))");
                        if (MyBase.Check_Table("DLD091"))
                        {
                            MyBase.Execute("Drop table DLD091");
                        }
                        MyBase.Execute("select * into DLD091 from openrowset('MSDASQL','DRIVER=Microsoft Visual Foxpro Driver;sourcedb=" + System.Windows.Forms.Application.StartupPath + ";sourcetype=DBF','select recno() Slno1,* from DLD091')");
                        if (MyBase.Check_Table("DC_Master"))
                        {
                            if (MyBase.Check_Table("DLD0911"))
                            {
                                MyBase.Execute("Drop table DLD0911");
                            }
                            MyBase.Execute_Tbl("Select D1.*, D2.DcDate from dld091 d1 left join dc_master d2 on d1.dcno = d2.dcno", "DLD0911");
                        }
                        MyBase.Execute("alter table dld0911 alter column tax1code int null");
                        MyBase.Execute("Update DLD0911 set tax1code = null where tax1code like ' %'");
                        MyBase.Execute("INSERT INTO DC_DETAILS select rtrim(D1.dcno), rtrim(d1.dcdate), rtrim(D1.sLNO1), null, rtrim(D1.itemcode), rtrim(p1.item_Name), d1.itemdetl,  rtrim(D1.rdesc), rtrim(D1.qty), rtrim(D1.soldon), rtrim(D1.mfrprn), rtrim(D1.rate), rtrim(D1.discp), convert(numeric, D1.Qty) * convert(numeric,D1.rate) as amount, rtrim(D1.tax1p), rtrim(D1.tax1Code), rtrim(D1.invcan) from dld0911 d1 left join product_master p1 on d1.itemcode = p1.item_code");
                    }
                }
                if (MyBase.DBFCn_Check_Table("Inv091"))
                {
                    if (MyBase.Check_Table("Invoice_Master") == false)
                    {
                        MyBase.Execute("create table Invoice_master (InvNo varchar(10), Invdate Datetime, Ledger_Code int, Above_Add varchar(200), Due_Days int, OrderNo varchar(100), orderDate datetime, DCNo varchar(100), DCDate datetime, FormnO varchar(100), formdate datetime, LRNO varchar(100), LRdate datetime, DespThro varchar(100), Freight varchar(10),  narration varchar(2000), Gamount float, tax_text varchar(100), other1 varchar(10), o1_amount float, other2 varchar(10), o2_amount float, other3 varchar(10), o3_per float, o3_amount float, R_Off_code varchar(10), R_off float, netamount float)");
                        MyBase.DBFCN_Delete("update Inv091 set Splinst2 = isnull(Splinst2, '') + Char(13) + splinst3 where splinst3 is not null");
                        MyBase.DBFCN_Delete("update Inv091 set Splinst = isnull(Splinst,'') + Char(13) + splinst2 where splinst2 is not null");
                        MyBase.Execute("insert into invoice_master select rtrim(Invno), rtrim(invdate), convert(numeric(4), pcode), rtrim(gpadd1), convert(numeric(10,2), dueper), rtrim(orderno), rtrim(ordate), rtrim(dcno), rtrim(dcdate), rtrim(formno), rtrim(formdate), rtrim(lrno), rtrim(lrdate), rtrim(desp), rtrim(freight), rtrim(Splinst), convert(numeric(10,2), gramt), rtrim(taxdet), rtrim(oth1code), convert(numeric(10,2), oth1amt), rtrim(oth2code), convert(numeric(10,2), oth2amt), rtrim(oth3code), convert(numeric(10,2), oth3p), convert(numeric(10,2), oth3amt), rtrim(rcode), convert(numeric(10,2), round), convert(numeric(10,2), netamt) from " + MyBase.DBF_SQL_DB + ".dbo.inv091");
                    }
                }
                if (MyBase.DBFCn_Check_Table("Ind091"))
                {
                    if (MyBase.Check_Table("Invoice_Details") == false)
                    {
                        MyBase.Execute("create table Invoice_Details (invNo varchar(10), invdate Datetime, I_SLno int, SLno varchar(10), Item_Code numeric(10), Item_Printing varchar(500), item_desc ntext, Desc1 varchar(1), Qty float, paran_Text varchar(500), Paran varchar(1), rate float, Dis float, Amount float, tax_per float, tax_Code int, invcan varchar(5))");
                        if (MyBase.Check_Table("IND091"))
                        {
                            MyBase.Execute("Drop table IND091");
                        }
                        MyBase.Execute("select * into IND091 from openrowset('MSDASQL','DRIVER=Microsoft Visual Foxpro Driver;sourcedb=" + System.Windows.Forms.Application.StartupPath + ";sourcetype=DBF','select recno() Slno1,* from IND091')");
                        if (MyBase.Check_Table("INVOICE_Master"))
                        {
                            if (MyBase.Check_Table("IND0911"))
                            {
                                MyBase.Execute("Drop table IND0911");
                            }
                            MyBase.Execute_Tbl("Select D1.*, D2.INVDate from INd091 d1 left join INVOICE_master d2 on d1.INVno = d2.INVno", "IND0911");
                        }
                        MyBase.Execute("alter table INd0911 alter column tax1code int null");
                        MyBase.Execute("Update IND0911 set tax1code = null where tax1code like ' %'");
                        MyBase.Execute("INSERT INTO INVOICE_DETAILS select rtrim(D1.INVno), rtrim(d1.INVdate), rtrim(D1.sLNO1), null, rtrim(D1.itemcode), rtrim(p1.item_Name), d1.itemdetl,  rtrim(D1.rdesc), rtrim(D1.qty), rtrim(D1.soldon), rtrim(D1.mfrprn), rtrim(D1.rate), rtrim(D1.discp), convert(numeric, D1.Qty) * convert(numeric,D1.rate) as amount, rtrim(D1.tax1p), rtrim(D1.tax1Code), rtrim(D1.invcan) from ind0911 d1 left join product_master p1 on d1.itemcode = p1.item_code");
                    }
                }
                if (MyBase.DBFCn_Check_Table("Sal091"))
                {
                    if (MyBase.Check_Table("Invoice_Cash_Master") == false)
                    {
                        MyBase.Execute("create table Invoice_Cash_master (InvNo varchar(10), Invdate Datetime, Ledger_Code int, Above_Add varchar(200), Due_Days int, OrderNo varchar(100), orderDate datetime, DCNo varchar(100), DCDate datetime, FormnO varchar(100), formdate datetime, LRNO varchar(100), LRdate datetime, DespThro varchar(100), Freight varchar(10),  narration varchar(2000), Gamount float, tax_text varchar(100), other1 varchar(10), o1_amount float, other2 varchar(10), o2_amount float, other3 varchar(10), o3_per float, o3_amount float, R_Off_code varchar(10), R_off float, netamount float)");
                        MyBase.DBFCN_Delete("update sal091 set Splinst2 = isnull(Splinst2, '') + Char(13) + splinst3 where splinst3 is not null");
                        MyBase.DBFCN_Delete("update sal091 set Splinst = isnull(Splinst,'') + Char(13) + splinst2 where splinst2 is not null");
                        MyBase.Execute("insert into Invoice_Cash_master select rtrim(Invno), rtrim(invdate), convert(numeric(4), pcode), rtrim(gpadd1), convert(numeric(10,2), dueper), rtrim(orderno), rtrim(ordate), rtrim(dcno), rtrim(dcdate), rtrim(formno), rtrim(formdate), rtrim(lrno), rtrim(lrdate), rtrim(desp), rtrim(freight), rtrim(Splinst), convert(numeric(10,2), gramt), rtrim(taxdet), rtrim(oth1code), convert(numeric(10,2), oth1amt), rtrim(oth2code), convert(numeric(10,2), oth2amt), rtrim(oth3code), convert(numeric(10,2), oth3p), convert(numeric(10,2), oth3amt), rtrim(rcode), convert(numeric(10,2), round), convert(numeric(10,2), netamt) from " + MyBase.DBF_SQL_DB + ".dbo.sal091");
                    }
                }
                if (MyBase.DBFCn_Check_Table("SAD091"))
                {
                    if (MyBase.Check_Table("Invoice_Cash_Details") == false)
                    {
                        MyBase.Execute("create table Invoice_Cash_Details (invNo varchar(10), invdate Datetime, I_SLno int, SLno varchar(10), Item_Code numeric(10), Item_Printing varchar(500), item_desc ntext, Desc1 varchar(1), Qty float, paran_Text varchar(500), Paran varchar(1), rate float, Dis float, Amount float, tax_per float, tax_Code int, invcan varchar(5))");
                        if (MyBase.Check_Table("sad091"))
                        {
                            MyBase.Execute("Drop table sad091");
                        }
                        MyBase.Execute("select * into sad091 from openrowset('MSDASQL','DRIVER=Microsoft Visual Foxpro Driver;sourcedb=" + System.Windows.Forms.Application.StartupPath + ";sourcetype=DBF','select recno() Slno1,* from saD091')");
                        if (MyBase.Check_Table("Invoice_Cash_Master"))
                        {
                            if (MyBase.Check_Table("saD0911"))
                            {
                                MyBase.Execute("Drop table saD0911");
                            }
                            MyBase.Execute_Tbl("Select D1.*, D2.INVDate from sad091 d1 left join Invoice_Cash_master d2 on d1.INVno = d2.INVno", "saD0911");
                        }
                        MyBase.Execute("alter table sad0911 alter column tax1code int null");
                        MyBase.Execute("Update saD0911 set tax1code = null where tax1code like ' %'");
                        MyBase.Execute("INSERT INTO Invoice_Cash_DETAILS select rtrim(D1.INVno), rtrim(d1.INVdate), rtrim(D1.sLNO1), null, rtrim(D1.itemcode), rtrim(p1.item_Name), d1.itemdetl,  rtrim(D1.rdesc), rtrim(D1.qty), rtrim(D1.soldon), rtrim(D1.mfrprn), rtrim(D1.rate), rtrim(D1.discp), convert(numeric, D1.Qty) * convert(numeric,D1.rate) as amount, rtrim(D1.tax1p), rtrim(D1.tax1Code), rtrim(D1.invcan) from sad0911 d1 left join product_master p1 on d1.itemcode = p1.item_code");
                    }
                }
                if (MyBase.DBFCn_Check_Table("PRO091"))
                {
                    if (MyBase.Check_Table("Invoice_Proforma_Master") == false)
                    {
                        MyBase.Execute("create table Invoice_Proforma_master (InvNo varchar(10), Invdate Datetime, Ledger_Code int, Above_Add varchar(200), Due_Days int, OrderNo varchar(100), orderDate datetime, DCNo varchar(100), DCDate datetime, FormnO varchar(100), formdate datetime, LRNO varchar(100), LRdate datetime, DespThro varchar(100), Freight varchar(10),  narration varchar(2000), Gamount float, tax_text varchar(100), other1 varchar(10), o1_amount float, other2 varchar(10), o2_amount float, other3 varchar(10), o3_per float, o3_amount float, R_Off_code varchar(10), R_off float, netamount float)");
                        MyBase.DBFCN_Delete("update Pro091 set Splinst2 = isnull(Splinst2, '') + Char(13) + splinst3 where splinst3 is not null");
                        MyBase.DBFCN_Delete("update Pro091 set Splinst = isnull(Splinst,'') + Char(13) + splinst2 where splinst2 is not null");
                        MyBase.Execute("insert into Invoice_Proforma_master select rtrim(Invno), rtrim(invdate), convert(numeric(4), pcode), rtrim(gpadd1), convert(numeric(10,2), dueper), rtrim(orderno), rtrim(ordate), rtrim(dcno), rtrim(dcdate), rtrim(formno), rtrim(formdate), rtrim(lrno), rtrim(lrdate), rtrim(desp), rtrim(freight), rtrim(Splinst), convert(numeric(10,2), gramt), rtrim(taxdet), rtrim(oth1code), convert(numeric(10,2), oth1amt), rtrim(oth2code), convert(numeric(10,2), oth2amt), rtrim(oth3code), convert(numeric(10,2), oth3p), convert(numeric(10,2), oth3amt), rtrim(rcode), convert(numeric(10,2), round), convert(numeric(10,2), netamt) from " + MyBase.DBF_SQL_DB + ".dbo.PRO091");
                    }
                }
                if (MyBase.DBFCn_Check_Table("prD091"))
                {
                    if (MyBase.Check_Table("Invoice_Proforma_Details") == false)
                    {
                        MyBase.Execute("create table Invoice_Proforma_Details (invNo varchar(10), invdate Datetime, I_SLno int, SLno varchar(10), Item_Code numeric(10), Item_Printing varchar(500), item_desc ntext, Desc1 varchar(1), Qty float, paran_Text varchar(500), Paran varchar(1), rate float, Dis float, Amount float, tax_per float, tax_Code int, invcan varchar(5))");
                        if (MyBase.Check_Table("PRd091"))
                        {
                            MyBase.Execute("Drop table Prd091");
                        }
                        MyBase.Execute("select * into Prd091 from openrowset('MSDASQL','DRIVER=Microsoft Visual Foxpro Driver;sourcedb=" + System.Windows.Forms.Application.StartupPath + ";sourcetype=DBF','select recno() Slno1,* from PrD091')");
                        if (MyBase.Check_Table("Invoice_Proforma_Master"))
                        {
                            if (MyBase.Check_Table("prD0911"))
                            {
                                MyBase.Execute("Drop table prD0911");
                            }
                            MyBase.Execute_Tbl("Select D1.*, D2.INVDate from prd091 d1 left join Invoice_Proforma_master d2 on d1.INVno = d2.INVno", "prD0911");
                        }
                        MyBase.Execute("alter table prd0911 alter column tax1code int null");
                        MyBase.Execute("Update prD0911 set tax1code = null where tax1code like ' %'");
                        MyBase.Execute("INSERT INTO Invoice_Proforma_DETAILS select rtrim(D1.INVno), rtrim(d1.INVdate), rtrim(D1.sLNO1), null, rtrim(D1.itemcode), rtrim(p1.item_Name), d1.itemdetl,  rtrim(D1.rdesc), rtrim(D1.qty), rtrim(D1.soldon), rtrim(D1.mfrprn), rtrim(D1.rate), rtrim(D1.discp), convert(numeric, D1.Qty) * convert(numeric,D1.rate) as amount, rtrim(D1.tax1p), rtrim(D1.tax1Code), rtrim(D1.invcan) from Prd0911 d1 left join product_master p1 on d1.itemcode = p1.item_code");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Tally_Conversion(String Company)
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("select g1.groupName gname, g1.groupname Alias, g2.groupName Parent from groupmas g1 left join groupmas G2 on g1.groupunder = g2.groupcode and g1.company_Code = g2.company_Code and g1.year_Code = g2.year_Code where g1.company_Code = " + CompCode + " and g1.year_Code = '" + YearCode + "' order by g1.groupcode", ref Dt);
                MyBase.Write_Xml_Group(Company, ref Dt);
                MessageBox.Show("Group Generated ...!");
                MyBase.Load_Data("select ledger_NAme PrtClr, Ledger_Name Alias, g1.groupName Parent, isnull(Ledger_OCredit, 0) OCredit, isnull(Ledger_ODebit, 0) Odebit, (CASE WHEN LEDGER_TIN IS NULL THEN '' ELSE (CASE WHEN Ledger_Tin = '-' THEN '' ELSE LEDGER_TIN END) END ) TinNo, (CASE WHEN LEDGER_CST IS NULL THEN '' ELSE (CASE WHEN Ledger_CST = '-' THEN '' ELSE LEDGER_CST END) END ) CSTno from ledger_Master l1 left join groupmas g1 on l1.ledger_group_code = g1.groupCode and l1.Company_Code = g1.company_Code and l1.year_Code = g1.year_Code where l1.company_Code = " + CompCode + " and l1.year_Code ='" + YearCode + "'", ref Dt);
                MyBase.Write_XML_Ledger(Company, ref Dt);
                MessageBox.Show("Ledger Generated ...!");
                MyBase.Write_XML_Payment(Company); MessageBox.Show("Payment Generated ...!");
                MyBase.Write_XML_Receipt(Company); MessageBox.Show("Receipt Generated ...!");
                MyBase.Write_XML_Contra(Company); MessageBox.Show("Contra Generated ...!");
                MyBase.Write_XML_Journal(Company); MessageBox.Show("Journal Generated ...!");
                MyBase.Write_XML_Sales(Company); MessageBox.Show("Sales Generated ...!");
                MyBase.Write_XML_Purchase(Company); MessageBox.Show("Purchase Generated ...!");
                MyBase.Write_XML_DebitNote(Company); MessageBox.Show("DebitNote Generated ...!");
                MyBase.Write_XML_CreditNote(Company); MessageBox.Show("CreditNote Generated ...!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        void Load_Basics()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Common_Help_Text(String Str)
        {
            try
            {
                return;
                StripLabel1.Text = Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Common_Help_Text_New()
        {
            try
            {
                Common_Help_Text("F12-Save, F11-Close");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Common_Help_Text_Edit()
        {
            try
            {
                Common_Help_Text("F12-Save, F11-Close");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Common_Help_Text_Delete()
        {
            try
            {
                Common_Help_Text("F5-Confirm, F11-Close");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Common_Help_Text_View()
        {
            try
            {
                Common_Help_Text("F8-Print, F11-Close");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Have_To_refresh()
        {
            DataTable Dt = new DataTable();
            DateTime Dtime1;
            try
            {
                MyBase.Load_Data("Select Max(Alter_Datetime) Dtime from voucher_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["Dtime"] != DBNull.Value)
                    {
                        Dtime1 = Convert.ToDateTime(Dt.Rows[0]["Dtime"]);
                    }
                    else
                    {
                        Dtime1 = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", DateTime.Today));
                    }
                }
                else
                {
                    Dtime1 = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", DateTime.Today));
                }
                if (Dtime != Dtime1)
                {
                    Dtime = Dtime1;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  SCRIPT FOR AUTOMATICE LEDGER UPDATION FROM LEDGERMAS_FAB

        //select cast('d' as varchar(10)) type, ledgerCode, ledgerName, groupcode, Odebit, Ocredit, TaxPer, BreakupFlag, compcode, yearCode, 0 as No
        //into LedFab_Accounts from ledgermas_Fab where 1= 2

        //Create trigger LedgerMas_Ins_Trig on ledgerMas_Fab for Insert 
        //as 
        //insert into LedFab_Accounts
        //Select 'Ins', ledgerCode, ledgerName, groupcode, Odebit, Ocredit, TaxPer, BreakupFlag, compcode, yearCode, 
        //0 as No from inserted

        //Create trigger LedgerMas_Del_Trig on ledgerMas_Fab for Delete
        //as 
        //insert into LedFab_Accounts
        //Select 'Del', ledgerCode, ledgerName, groupcode, Odebit, Ocredit, TaxPer, BreakupFlag, compcode, yearCode, 
        //0 as No from Deleted

        //Create trigger LedgerMas_Upd_Trig on ledgerMas_Fab for Update
        //as 
        //insert into LedFab_Accounts
        //Select distinct 'UPD', ledgerCode, ledgerName, groupcode, Odebit, Ocredit, TaxPer, BreakupFlag, compcode, yearCode, 
        //0 as No from Inserted


        /// </summary>


        public void Auto_Update_Ledger()
        {
            DataTable Ledger_Fab = new DataTable();
            DataTable Temp = new DataTable();
            Double LCode = 0;
            try
            {
                if (MyBase.Get_RecordCount("LEdger_Master", "Ledger_Code = 0 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Load_Data("Select * from ledger_Master where ledger_Code = 0 and Company_Code = " + CompCode + " and Year_Code = '" + YearCode + "' order by Ledger_Name", ref Ledger_Fab);
                    LCode = MyBase.Max("Ledger_MasteR", "Ledger_code", String.Empty, YearCode, CompCode);
                    for (int i = 0; i <= Ledger_Fab.Rows.Count - 1; i++)
                    {
                        MyBase.Execute("Update ledger_master set ledger_Code = " + LCode + " where link_compcode = " + Ledger_Fab.Rows[i]["link_compcode"].ToString() + " and link_yearCode = '" + Ledger_Fab.Rows[i]["link_yearCode"].ToString() + "' and link_ledgercode = " + Ledger_Fab.Rows[i]["link_ledgercode"].ToString() + " and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code = 0");
                        LCode += 1;
                    }
                }

                //MyBase.Load_Data("Select * from vaahini_erp_Aegan.dbo.LedFab_Accounts", ref Temp);
                //if (Temp.Rows.Count > 0)
                //{
                //    if (MyBase.Check_Table("LedgerMas_Fab"))
                //    {
                //        MyBase.Execute("Drop table LedgerMas_Fab");
                //    }
                //    MyBase.Execute("Select * into LedgerMas_Fab from vaahini_erp_Aegan.dbo.LedFab_Accounts");

                //    MyBase.Load_Data("Select * from ledger_code_up order by oldcode", ref Ledger_Fab);
                //    for (int i = 0; i <= Ledger_Fab.Rows.Count - 1; i++)
                //    {
                //        MyBase.Execute("UPdate ledgerMas_fab Set groupcode = " + Ledger_Fab.Rows[i]["newcode"].ToString() + ", no = 1 where groupcode = " + Ledger_Fab.Rows[i]["oldcode"].ToString() + " and no = 0");
                //    }

                //    MyBase.Load_Data("Select Distinct * from ledgerMas_fab order by compCode, ledgercode", ref Ledger_Fab);
                //    LCode = MyBase.Max("Ledger_MasteR", "Ledger_code", String.Empty, YearCode, CompCode);
                //    for (int i = 0; i <= Ledger_Fab.Rows.Count - 1; i++)
                //    {
                //        MyBase.Execute("insert into ledger_master (ledger_Code, Ledger_Name, ledger_INPrint, ledger_group_Code, ledger_Odebit, Ledger_OCredit, Link_compCode, Link_yearcode, tax_per, Breakup, company_code, year_code, link_ledgercode) values (" + LCode + ", '" + Ledger_Fab.Rows[i]["LedgerName"].ToString() + "', '" + Ledger_Fab.Rows[i]["LedgerName"].ToString() + "', " + Ledger_Fab.Rows[i]["GroupCode"].ToString() + ", " + Ledger_Fab.Rows[i]["Odebit"].ToString() + ", " + Ledger_Fab.Rows[i]["Ocredit"].ToString() + ", " + Ledger_Fab.Rows[i]["Compcode"].ToString() + ", '" + Ledger_Fab.Rows[i]["YearCode"].ToString() + "', " + Ledger_Fab.Rows[i]["TaxPer"].ToString() + ", '" + Ledger_Fab.Rows[i]["BreakupFlag"].ToString() + "', 1, '" + YearCode + "', " + Ledger_Fab.Rows[i]["Ledgercode"].ToString() + ")");
                //        MyBase.Execute("insert into ledger_master (ledger_Code, Ledger_Name, ledger_INPrint, ledger_group_Code, ledger_Odebit, Ledger_OCredit, Link_compCode, Link_yearcode, tax_per, Breakup, company_code, year_code, link_ledgercode) values (" + LCode + ", '" + Ledger_Fab.Rows[i]["LedgerName"].ToString() + "', '" + Ledger_Fab.Rows[i]["LedgerName"].ToString() + "', " + Ledger_Fab.Rows[i]["GroupCode"].ToString() + ", " + Ledger_Fab.Rows[i]["Odebit"].ToString() + ", " + Ledger_Fab.Rows[i]["Ocredit"].ToString() + ", " + Ledger_Fab.Rows[i]["Compcode"].ToString() + ", '" + Ledger_Fab.Rows[i]["YearCode"].ToString() + "', " + Ledger_Fab.Rows[i]["TaxPer"].ToString() + ", '" + Ledger_Fab.Rows[i]["BreakupFlag"].ToString() + "', 2, '" + YearCode + "', " + Ledger_Fab.Rows[i]["Ledgercode"].ToString() + ")");
                //        LCode += 1;
                //    }
                //    MyBase.Execute("Delete from vaahini_erp_Aegan.dbo.ledfab_Accounts");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Sales_Update()
        {
            String Str = String.Empty;
            double Vcode = 0;
            DataTable Dt = new DataTable();
            double Ledger_Code_RO = 0;
            try
            {
                if (MyBase.Check_Table("Sales_Voucher"))
                {
                    MyBase.Execute("dROP TABLE SALES_vOUCHER");
                }
                if (MyBase.Check_Table("Sales_Voucher") == false)
                {
                    Str =  " select cast(0 as Numeric(10)) as Vcode, i1.remark, i1.remark1, i1.invtype, i1.invoiceNo, i1.InvoiceDt, i1.salescode SalesAcCode, l1.ledgername SalesAc,i1.LedgerCode, l2.ledgername Party, ";
                    Str += " i1.bedcode, l3.ledgername BedLEdger, i1.aedcode, l4.ledgername AedLedger,i1.taxcode, l5.ledgername TaxLedger, ";
                    Str += " i1.other1code, l6.ledgername OtherLedger, i1.bedper, i1.aedper, i1.taxper, i1.other1per, i1.grossamount, i1.bed, i1.aed, ";
                    Str += " i1.subtotal, i1.tax, i1.other1, i1.roundedoff, i1.netamount, (case when (i1.compcode = 1 or i1.compcode = 2) then 1 else 2 end) COmpcode, i1.yearcode ";
                    Str += " from vaahini_erp_Aegan.dbo.invoicemas i1 left join vaahini_erp_Aegan.dbo.LedgerMas_Fab l1 on i1.salesCode = l1.ledgercode ";
                    Str += " left join vaahini_erp_Aegan.dbo.ledgermas_fab l2 on i1.ledgercode = l2.ledgercode left join vaahini_erp_Aegan.dbo.ledgermas_fab l3 on i1.bedcode = l3.ledgercode ";
                    Str += " left join vaahini_erp_Aegan.dbo.ledgermas_fab l4 on i1.Aedcode = l4.ledgercode left join vaahini_erp_Aegan.dbo.ledgermas_fab l5 on i1.taxcode = l5.ledgercode left join vaahini_erp_Aegan.dbo.ledgermas_fab l6 on i1.Other1code = l6.ledgercode ";
                    MyBase.Execute_Tbl(Str, "Sales_Voucher");
                }

                // Divide 4 Companys to 2
                MyBase.Execute("Update ledger_MAster set link_Compcode = 1 where LInk_Compcode = 1 or Link_Compcode = 2");
                MyBase.Execute("Update ledger_MAster set link_Compcode = 2 where LInk_Compcode = 3 or Link_Compcode = 4");

                //Ledgercode 
                MyBase.Execute ("update sales_voucher set ledgerCode = l1.ledger_code from sales_voucher s1, ledger_master l1 where s1.compcode = l1.company_code and s1.ledgercode = l1.link_ledgerCode AND UPPER(L1.LINK_FROM) = 'BILL'");

                //SalesAccode
                MyBase.Execute("update sales_voucher set SalesAcCode = l1.ledger_code from sales_voucher s1, ledger_master l1 where s1.compcode = l1.company_code and s1.SalesAcCode = l1.link_ledgerCode AND UPPER(L1.LINK_FROM) = 'BILL' ");

                //BedCode
                MyBase.Execute("update sales_voucher set Bedcode = l1.ledger_code from sales_voucher s1, ledger_master l1 where s1.compcode = l1.company_code and s1.Bedcode = l1.link_ledgerCode AND UPPER(L1.LINK_FROM) = 'BILL' ");

                //AedCode
                MyBase.Execute("update sales_voucher set Aedcode = l1.ledger_code from sales_voucher s1, ledger_master l1 where s1.compcode = l1.company_code and s1.Aedcode = l1.link_ledgerCode AND UPPER(L1.LINK_FROM) = 'BILL' ");

                //TaxCode
                MyBase.Execute("update sales_voucher set Taxcode = l1.ledger_code from sales_voucher s1, ledger_master l1 where s1.compcode = l1.company_code and s1.Taxcode = l1.link_ledgerCode AND UPPER(L1.LINK_FROM) = 'BILL' ");

                //Other1Code
                MyBase.Execute("update sales_voucher set Other1Code = l1.ledger_code from sales_voucher s1, ledger_master l1 where s1.compcode = l1.company_code and s1.Other1Code = l1.link_ledgerCode AND UPPER(L1.LINK_FROM) = 'BILL' ");

                // vcode updation for Company #1
                MyBase.Load_Data("Select invoiceNo, invoicedt, compcode, yearcode from sales_voucher where compcode = 1 order by invoicedt", ref Dt);
                Vcode = MyBase.Max("Voucher_Master", "Vcode", String.Empty, "2009-2010", 1);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Update sales_Voucher set Vcode = " + Vcode + " where invoiceno = '" + Dt.Rows[i]["invoiceno"].ToString() + "' and invoicedt = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["INvoicedt"])) + "' and compcode = " + Dt.Rows[i]["compcode"].ToString() + " and yearCode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    Vcode += 1;
                }

                //vcode updation for Company #2
                MyBase.Load_Data("Select invoiceNo, invoicedt, compcode, yearcode from sales_voucher where compcode = 2 order by invoicedt", ref Dt);
                Vcode = MyBase.Max("Voucher_Master", "Vcode", String.Empty, "2009-2010", 2);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Update sales_Voucher set Vcode = " + Vcode + " where invoiceno = '" + Dt.Rows[i]["invoiceno"].ToString() + "' and invoicedt = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["INvoicedt"])) + "' and compcode = " + Dt.Rows[i]["compcode"].ToString() + " and yearCode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    Vcode += 1;
                }

                MyBase.Execute("insert into voucher_master select vcode, 5, invoiceno, invtype, invoicedt, remark, invoicedt, null, null, null, null, null, null, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)) from sales_voucher");


                //voucher Details updation for Company #1
                MyBase.Load_Data("select vcode, compcode, yearcode, roundedoff, salesaccode, ledgercode, bedcode, aedcode, taxcode, other1code from sales_voucher where compcode = 1 order by vcode ", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Insert into Ledger_breakup select ledgercode, 'VOUCHER', 1, 'N', invoiceno, invoicedt, 0, netamount, 0, 5, 0, 0, vcode, Compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)) from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    MyBase.Execute("Insert into voucher_Breakup_Bills select vcode, invoicedt, LedgerCode, 1, 'N', invoiceno, invoicedt, netamount, 0, 0, vcode, 'CR', Compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)) from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    MyBase.Execute("insert into voucher_details select vcode, invoicedt, 1, 'BY', Ledgercode, netamount, 0, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 2, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    MyBase.Execute("insert into voucher_details select vcode, invoicedt, 2, 'TO', SalesAccode, 0, grossamount, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    if (Dt.Rows[i]["taxcode"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["taxcode"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 3, 'TO', taxcode, 0, tax, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["other1code"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["Other1code"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 4, 'TO', other1code, 0, other1, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["bedcode"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["bedcode"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 5, 'TO', bedcode, 0, bed, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["aedcode"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["aedcode"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 6, 'TO', aedcode, 0, aed, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Convert.ToDouble(Dt.Rows[i]["roundedoff"]) != 0)
                    {
                        Ledger_Code_RO = MyBase.GetData_InNumberWC("Ledger_Master", "Ledger_NAme", "ROUNDED OFF", "LEDGER_CODE", Dt.Rows[i]["Yearcode"].ToString() + '-' + Convert.ToString(Convert.ToInt32(Dt.Rows[i]["Yearcode"]) + 1), Convert.ToInt32(Dt.Rows[i]["compcode"]));
                        if (Ledger_Code_RO > 0)
                        {
                            MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', " + Ledger_Code_RO + ", 0, roundedOff, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                        }
                        else
                        {
                            MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', null, 0, roundedOff, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                        }
                    }
                    //else if (Convert.ToDouble(Dt.Rows[i]["roundedoff"]) < 0)
                    //{
                    //    Ledger_Code_RO = MyBase.GetData_InNumberWC("Ledger_Master", "Ledger_NAme", "ROUNDED OFF", "LEDGER_CODE", Dt.Rows[i]["Yearcode"].ToString() + '-' + Convert.ToString(Convert.ToInt32(Dt.Rows[i]["Yearcode"]) + 1), Convert.ToInt32(Dt.Rows[i]["compcode"]));
                    //    if (Ledger_Code_RO > 0)
                    //    {
                    //        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', " + Ledger_Code_RO + ", roundedOff * (-1), 0, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1 from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    //    }
                    //    else
                    //    {
                    //        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', null, roundedOff * (-1), 0, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1 from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    //    }
                    //}
                }

                // voucher Details updation for Company #2
                MyBase.Load_Data("select vcode, compcode, yearcode, roundedoff, salesaccode, ledgercode, bedcode, aedcode, taxcode, other1code from sales_voucher where compcode = 2 order by vcode ", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Insert into Ledger_breakup select ledgercode, 'VOUCHER', 1, 'N', invoiceno, invoicedt, 0, netamount, 0, 5, 0, 0, vcode, Compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)) from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    MyBase.Execute("Insert into voucher_Breakup_Bills select vcode, invoicedt, LedgerCode, 1, 'N', invoiceno, invoicedt, netamount, 0, 0, vcode, 'CR', Compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)) from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    MyBase.Execute("insert into voucher_details select vcode, invoicedt, 1, 'BY', Ledgercode, netamount, 0, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 2, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    MyBase.Execute("insert into voucher_details select vcode, invoicedt, 2, 'TO', SalesAccode, 0, grossamount, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    if (Dt.Rows[i]["taxcode"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["taxcode"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 3, 'TO', taxcode, 0, tax, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["other1code"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["Other1code"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 4, 'TO', other1code, 0, other1, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["bedcode"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["bedcode"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 5, 'TO', bedcode, 0, bed, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["aedcode"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["aedcode"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 6, 'TO', aedcode, 0, aed, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Convert.ToInt32(Dt.Rows[i]["roundedoff"]) > 0)
                    {
                        Ledger_Code_RO = MyBase.GetData_InNumberWC("Ledger_Master", "Ledger_NAme", "ROUNDED OFF", "LEDGER_CODE", Dt.Rows[i]["Yearcode"].ToString(), Convert.ToInt32(Dt.Rows[i]["compcode"]));
                        if (Ledger_Code_RO > 0)
                        {
                            MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', " + Ledger_Code_RO + ", 0, roundedOff, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                        }
                        else
                        {
                            MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', null, 0, roundedOff, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                        }
                    }
                    //else if (Convert.ToInt32(Dt.Rows[i]["roundedoff"]) < 0)
                    //{
                    //    Ledger_Code_RO = MyBase.GetData_InNumberWC("Ledger_Master", "Ledger_NAme", "ROUNDED OFF", "LEDGER_CODE", Dt.Rows[i]["Yearcode"].ToString(), Convert.ToInt32(Dt.Rows[i]["compcode"]));
                    //    if (Ledger_Code_RO > 0)
                    //    {
                    //        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', " + Ledger_Code_RO + ", roundedOff * (-1), 0, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    //    }
                    //    else
                    //    {
                    //        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', null, roundedOff * (-1), 0, Remark, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Waste_Sales_Update()
        {
            String Str = String.Empty;
            double Vcode = 0;
            DataTable Dt = new DataTable();
            double Ledger_Code_RO = 0;
            try
            {
                return;
                if (MyBase.Check_Table("Sales_Voucher"))
                {
                    MyBase.Execute("Drop table Sales_Voucher");
                }
                if (MyBase.Check_Table("Sales_Voucher") == false)
                {
                    Str = "select cast(0 as Numeric(10)) as Vcode, i1.remarks, i1.invtype, i1.invoiceNo, i1.InvoiceDt, i1.salescode SalesAcCode, l1.ledgername SalesAc, i1.LedgerCode, l2.ledgername Party,  i1.frecode, l3.ledgername freLEdger, i1.premitcode, l4.ledgername premitLedger, i1.tax3code, l5.ledgername Tax3Ledger,  ";
                    Str += " i1.other1code, l6.ledgername Other1Ledger, i1.other2code, l7.ledgername Other2Ledger, i1.freper, i1.premitper, i1.tax3per, i1.tax4code, l8.ledgername Tax4Ledger,  i1.tax4per, i1.other1per, i1.grossamount, i1.freamt, i1.premitamt,  i1.subtotal1, i1.tax3amt, i1.other1amt, i1.other2per, i1.other2amt, i1.tax4amt, ";
                    Str += " i1.roundoff, i1.netamount, (case when (i1.compcode = 1 or i1.compcode = 2) then 1 else 2 end) COmpcode, i1.yearcode from vaahini_erp_Aegan.dbo.it_wasmas i1 left join vaahini_erp_Aegan.dbo.LedgerMas_Fab l1 on i1.salesCode = l1.ledgercode  left join vaahini_erp_Aegan.dbo.ledgermas_fab l2 on i1.ledgercode = l2.ledgercode left join vaahini_erp_Aegan.dbo.ledgermas_fab l3  ";
                    Str += " on i1.frecode = l3.ledgercode  left join vaahini_erp_Aegan.dbo.ledgermas_fab l4 on i1.premitcode = l4.ledgercode left join vaahini_erp_Aegan.dbo.ledgermas_fab l5 on i1.tax3code = l5.ledgercode left join vaahini_erp_Aegan.dbo.ledgermas_fab l6 on i1.Other1code = l6.ledgercode left join vaahini_erp_Aegan.dbo.ledgermas_fab l7 on i1.Other2code = l7.ledgercode left join vaahini_erp_Aegan.dbo.ledgermas_fab l8 on i1.tax4code = l8.ledgercode ";
                    MyBase.Execute_Tbl(Str, "Sales_Voucher");
                }

                // Divide 4 Companys to 2
                MyBase.Execute("Update ledger_MAster set link_Compcode = 1 where LInk_Compcode = 1 or Link_Compcode = 2");
                MyBase.Execute("Update ledger_MAster set link_Compcode = 2 where LInk_Compcode = 3 or Link_Compcode = 4");

                //Ledgercode 
                MyBase.Execute("update sales_voucher set ledgerCode = l1.ledger_code from sales_voucher s1, ledger_master l1 where s1.compcode = l1.company_code and s1.ledgercode = l1.link_ledgerCode AND UPPER(L1.LINK_FROM) = 'BILL'");

                //SalesAccode
                MyBase.Execute("update sales_voucher set SalesAcCode = l1.ledger_code from sales_voucher s1, ledger_master l1 where s1.compcode = l1.company_code and s1.SalesAcCode = l1.link_ledgerCode AND UPPER(L1.LINK_FROM) = 'BILL' ");

                //BedCode
                MyBase.Execute("update sales_voucher set frecode = l1.ledger_code from sales_voucher s1, ledger_master l1 where s1.compcode = l1.company_code and s1.frecode = l1.link_ledgerCode AND UPPER(L1.LINK_FROM) = 'BILL' ");

                //AedCode
                MyBase.Execute("update sales_voucher set premitcode = l1.ledger_code from sales_voucher s1, ledger_master l1 where s1.compcode = l1.company_code and s1.premitcode = l1.link_ledgerCode AND UPPER(L1.LINK_FROM) = 'BILL' ");

                //Tax3Code
                MyBase.Execute("update sales_voucher set Tax3code = l1.ledger_code from sales_voucher s1, ledger_master l1 where s1.compcode = l1.company_code and s1.Tax3code = l1.link_ledgerCode AND UPPER(L1.LINK_FROM) = 'BILL' ");
                
                //Tax4Code
                MyBase.Execute("update sales_voucher set Tax4code = l1.ledger_code from sales_voucher s1, ledger_master l1 where s1.compcode = l1.company_code and s1.Tax4code = l1.link_ledgerCode AND UPPER(L1.LINK_FROM) = 'BILL' ");

                //Other1Code
                MyBase.Execute("update sales_voucher set Other1Code = l1.ledger_code from sales_voucher s1, ledger_master l1 where s1.compcode = l1.company_code and s1.Other1Code = l1.link_ledgerCode AND UPPER(L1.LINK_FROM) = 'BILL' ");

                //Other2Code
                MyBase.Execute("update sales_voucher set Other2Code = l1.ledger_code from sales_voucher s1, ledger_master l1 where s1.compcode = l1.company_code and s1.Other2Code = l1.link_ledgerCode AND UPPER(L1.LINK_FROM) = 'BILL' ");

                // vcode updation for Company #1
                MyBase.Load_Data("Select invoiceNo, invoicedt, compcode, yearcode from sales_voucher where compcode = 1 order by invoicedt", ref Dt);
                Vcode = MyBase.Max("Voucher_Master", "Vcode", String.Empty, "2009-2010", 1);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Update sales_Voucher set Vcode = " + Vcode + " where invoiceno = '" + Dt.Rows[i]["invoiceno"].ToString() + "' and invoicedt = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["INvoicedt"])) + "' and compcode = " + Dt.Rows[i]["compcode"].ToString() + " and yearCode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    Vcode += 1;
                }

                // vcode updation for Company #2
                MyBase.Load_Data("Select invoiceNo, invoicedt, compcode, yearcode from sales_voucher where compcode = 2 order by invoicedt", ref Dt);
                Vcode = MyBase.Max("Voucher_Master", "Vcode", String.Empty, "2009-2010", 2);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Update sales_Voucher set Vcode = " + Vcode + " where invoiceno = '" + Dt.Rows[i]["invoiceno"].ToString() + "' and invoicedt = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["INvoicedt"])) + "' and compcode = " + Dt.Rows[i]["compcode"].ToString() + " and yearCode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    Vcode += 1;
                }

                MyBase.Execute("insert into voucher_master select vcode, 5,  invoiceno, invtype, invoicedt, remarks, invoicedt, null, null, null, null, null, null, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)) from sales_voucher");

                // voucher Details updation for Company #1
                MyBase.Load_Data("select vcode, compcode, yearcode, roundoff roundedOff, salesaccode, ledgercode, frecode, premitcode, tax3code, tax4code, other1code, other2code from sales_voucher where compcode = 1 order by vcode ",ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Insert into Ledger_breakup select ledgercode, 'VOUCHER', 1, 'N', invoiceno, invoicedt, 0, netamount, 0, 5, 0, 0, vcode, Compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)) from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    MyBase.Execute("Insert into voucher_Breakup_Bills select vcode, invoicedt, LedgerCode, 1, 'N', invoiceno, invoicedt, netamount, 0, 0, vcode, 'CR', Compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)) from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    MyBase.Execute("insert into voucher_details select vcode, invoicedt, 1, 'BY', Ledgercode, netamount, 0, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 2, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    MyBase.Execute("insert into voucher_details select vcode, invoicedt, 2, 'TO', SalesAccode, 0, grossamount, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    if (Dt.Rows[i]["tax3code"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["tax3code"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 3, 'TO', tax3code, 0, tax3amt, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["tax4code"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["tax4code"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 3, 'TO', tax4code, 0, tax4amt, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["other1code"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["Other1code"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 4, 'TO', other1code, 0, other1amt, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["other2code"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["Other2code"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 4, 'TO', other2code, 0, other2amt, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["frecode"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["frecode"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 5, 'TO', frecode, 0, freamt, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["premitcode"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["premitcode"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 6, 'TO', premitcode, 0, premitamt, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Convert.ToDouble(Dt.Rows[i]["roundedoff"]) != 0)
                    {
                        Ledger_Code_RO = MyBase.GetData_InNumberWC("Ledger_Master", "Ledger_NAme", "ROUNDED OFF", "LEDGER_CODE", Dt.Rows[i]["Yearcode"].ToString() + '-' + Convert.ToString(Convert.ToInt32(Dt.Rows[i]["Yearcode"]) + 1), Convert.ToInt32(Dt.Rows[i]["compcode"]));
                        if (Ledger_Code_RO > 0)
                        {
                            MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', " + Ledger_Code_RO + ", 0, roundOff, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                        }
                        else
                        {
                            MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', null, 0, roundOff, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                        }
                    }
                    //else if (Convert.ToDouble(Dt.Rows[i]["roundedoff"]) < 0)
                    //{
                    //    Ledger_Code_RO = MyBase.GetData_InNumberWC("Ledger_Master", "Ledger_NAme", "ROUNDED OFF", "LEDGER_CODE", Dt.Rows[i]["Yearcode"].ToString() + '-' + Convert.ToString(Convert.ToInt32(Dt.Rows[i]["Yearcode"]) + 1), Convert.ToInt32(Dt.Rows[i]["compcode"]));
                    //    if (Ledger_Code_RO > 0)
                    //    {
                    //        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', " + Ledger_Code_RO + ", roundOff * (-1), 0, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    //    }
                    //    else
                    //    {
                    //        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', null, roundOff * (-1), 0, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    //    }
                    //}
                }

                // voucher Details updation for Company #2
                MyBase.Load_Data("select vcode, compcode, yearcode, roundoff roundedOff, salesaccode, ledgercode, frecode, premitcode, tax3code, tax4code, other1code, other2code from sales_voucher where compcode = 2 order by vcode ", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Insert into Ledger_breakup select ledgercode, 'VOUCHER', 1, 'N', invoiceno, invoicedt, 0, netamount, 0, 5, 0, 0, vcode, Compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)) from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    MyBase.Execute("Insert into voucher_Breakup_Bills select vcode, invoicedt, LedgerCode, 1, 'N', invoiceno, invoicedt, netamount, 0, 0, vcode, 'CR', Compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)) from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    MyBase.Execute("insert into voucher_details select vcode, invoicedt, 1, 'BY', Ledgercode, netamount, 0, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 2, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    MyBase.Execute("insert into voucher_details select vcode, invoicedt, 2, 'TO', SalesAccode, 0, grossamount, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    if (Dt.Rows[i]["tax3code"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["tax3code"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 3, 'TO', tax3code, 0, tax3amt, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["tax4code"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["tax4code"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 3, 'TO', tax4code, 0, tax4amt, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["other1code"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["Other1code"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 4, 'TO', other1code, 0, other1amt, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["other2code"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["Other2code"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 4, 'TO', other2code, 0, other2amt, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["frecode"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["frecode"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 5, 'TO', frecode, 0, freamt, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Dt.Rows[i]["premitcode"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["premitcode"]) > 0)
                    {
                        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 6, 'TO', premitcode, 0, premitamt, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    }
                    if (Convert.ToInt32(Dt.Rows[i]["roundedoff"]) != 0)
                    {
                        Ledger_Code_RO = MyBase.GetData_InNumberWC("Ledger_Master", "Ledger_NAme", "ROUNDED OFF", "LEDGER_CODE", Dt.Rows[i]["Yearcode"].ToString(), Convert.ToInt32(Dt.Rows[i]["compcode"]));
                        if (Ledger_Code_RO > 0)
                        {
                            MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', " + Ledger_Code_RO + ", 0, roundedOff, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                        }
                        else
                        {
                            MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', null, 0, roundedOff, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                        }
                    }
                    //else if (Convert.ToInt32(Dt.Rows[i]["roundedoff"]) < 0)
                    //{
                    //    Ledger_Code_RO = MyBase.GetData_InNumberWC("Ledger_Master", "Ledger_NAme", "ROUNDED OFF", "LEDGER_CODE", Dt.Rows[i]["Yearcode"].ToString(), Convert.ToInt32(Dt.Rows[i]["compcode"]));
                    //    if (Ledger_Code_RO > 0)
                    //    {
                    //        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', " + Ledger_Code_RO + ", roundedOff * (-1), 0, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    //    }
                    //    else
                    //    {
                    //        MyBase.Execute("insert into voucher_details select vcode, invoicedt, 7, 'TO', null, roundedOff * (-1), 0, Remarks, compcode, cast(yearCode as varchar(4)) + '-' + cast(cast(yearcode as int) + 1 as varchar(4)), 1, 'True', 'True', 'True' from sales_voucher where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and compcode = " + Dt.Rows[i]["Compcode"].ToString() + " and yearcode = '" + Dt.Rows[i]["Yearcode"].ToString() + "'");
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Remove_Temp_table()
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("Select NAme from sysobjects where name like '" + MyBase.GetSystemNameForTable() + "%'", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Drop table " + Dt.Rows[i]["Name"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Check_Profit_Loss()
        {
            try
            {
                if (MyBase.Check_Table("PandL_Heading_Master_V") == false)
                {
                    MyBase.Execute(" Create table PandL_Heading_Master_V (Code int, Head_Name varchar(50), Order_Slno int)");
                    
                    MyBase.Execute(" insert into PandL_Heading_Master_V values (1, 'Income', 1)");
                    MyBase.Execute(" insert into PandL_Heading_Master_V values (2, 'Expenditure', 2)");
                    MyBase.Execute(" insert into PandL_Heading_Master_V values (3, 'Expenditure After Net Profit', 3)");
                }

                if (MyBase.Check_Table("PandL_SubHeading_Master_V"))
                {
                    if (MyBase.Check_TableField("PandL_SubHeading_Master_V", "Type") == false)
                    {
                        MyBase.Execute("Drop table PandL_SubHeading_Master_V");
                    }
                }


                if (MyBase.Check_Table("PandL_SubHeading_Master_V") == false)
                {
                    MyBase.Execute(" create table PandL_SubHeading_Master_V (Code int, SubHead_Name varchar(50), Head_Code int, Order_Slno int, type varchar(2), Company_Code int)");

                    MyBase.Execute(" insert into PandL_SubHeading_Master_V values (1, 'Sales Less Return', 1, 1, 'Cr', " + CompCode + ")");
                    MyBase.Execute(" insert into PandL_SubHeading_Master_V values (2, 'Other Income', 1, 2, 'Cr', " + CompCode + ")");

                    MyBase.Execute(" insert into PandL_SubHeading_Master_V values (3, 'Cost Of Sales', 2, 1, 'Dr', " + CompCode + ")");
                    MyBase.Execute(" insert into PandL_SubHeading_Master_V values (4, 'Administrative Expenses', 2, 2, 'Dr', " + CompCode + ")");
                    MyBase.Execute(" insert into PandL_SubHeading_Master_V values (5, 'Staff Remuneration & Benefits', 2, 3, 'Dr', " + CompCode + ")");
                    MyBase.Execute(" insert into PandL_SubHeading_Master_V values (6, 'Selling & Distribution Charges', 2, 4, 'Dr', " + CompCode + ")");
                    MyBase.Execute(" insert into PandL_SubHeading_Master_V values (7, 'Financial Charges', 2, 5, 'Dr', " + CompCode + ")");
                    MyBase.Execute(" insert into PandL_SubHeading_Master_V values (8, 'Depriciation', 2, 6, 'Dr', " + CompCode + ")");

                }
                if (MyBase.Get_RecordCount("PandL_SubHeading_Master_V", "Subhead_Name = 'Expenditure After Net Profit'") > 0)
                {
                    MyBase.Execute("Delete from PandL_SubHeading_Master_V where code = 9 and Company_Code = " + CompCode);
                    MyBase.Execute(" insert into PandL_SubHeading_Master_V values (9, 'Less:Provision for Taxation', 3, 1, 'Dr', " + CompCode + ")");
                    MyBase.Execute(" insert into PandL_SubHeading_Master_V values (10, 'Less:Provision for FBT', 3, 2, 'Dr', " + CompCode + ")");
                    MyBase.Execute(" insert into PandL_SubHeading_Master_V values (11, 'Add:Deferred Tax Assets', 3, 3, 'Dr', " + CompCode + ")");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void PandL_Balance()
        {
            try
            {
                if (MyBase.Check_Table("Dont_Del") == false)
                {
                    MyBase.Execute("Create table Dont_Del (no NUmeric(4))");

                    if (MyBase.Check_Table("pandL_subheading_master_V"))
                    {
                        MyBase.Execute("Drop table pandL_subheading_master_V");
                    }

                    if (MyBase.Check_Table("balance_subheading_master_V"))
                    {
                        MyBase.Execute("Drop table Balance_subheading_master_V");
                    }

                    if (MyBase.Check_Table("groupmas_setting"))
                    {
                        MyBase.Execute("Drop table groupmas_setting");
                    }

                    MyBase.Execute(" CREATE TABLE [dbo].[GroupMas_Setting]([Groupcode] [int] NULL,	[Parent_Group] [int] NULL,	[type] [varchar](1) NULL,	[VOrH] [varchar](1) NULL,	[Subhead_Code] [int] NULL,	[order_Slno] [int] NULL,	[Operator_Symbol] [varchar](1) NULL,	[NEW_EMPCODE] [numeric](4, 0) NULL,	[NEW_SYSCODE] [numeric](4, 0) NULL,	[NEW_DATETIME] [datetime] NULL,	[ALTER_EMPCODE] [numeric](4, 0) NULL,	[ALTER_SYSCODE] [numeric](4, 0) NULL,	[ALTER_DATETIME] [datetime] NULL,	[COMPANY_CODE] [numeric](2, 0) NULL,	[YEAR_CODE] [varchar](10) NULL,	[MODE] [varchar](2) NULL)");
                    MyBase.Execute(" CREATE TABLE [dbo].[PandL_SubHeading_Master_V](	[Code] [int] NULL,	[SubHead_Name] [varchar](50) NULL,	[Head_Code] [int] NULL,	[Order_Slno] [int] NULL,	[type] [varchar](2) NULL,	[Company_Code] [int] NULL) ");
                    MyBase.Execute(" CREATE TABLE [dbo].[Balance_SubHeading_Master_V](	[Code] [int] NULL,	[SubHead_Name] [varchar](50) NULL,	[Head_Code] [int] NULL,	[Order_Slno] [int] NULL,	[type] [varchar](2) NULL,	[Company_Code] [int] NULL)");

                    //PandL
                    MyBase.Execute(" insert into pandL_subheading_master_V values (1,'Sales Less Return',1,1,'Cr',1)");
                    MyBase.Execute(" insert into pandL_subheading_master_V values (2,'Other Income',1,2,'Cr',1)");
                    MyBase.Execute(" insert into pandL_subheading_master_V values (3,'Raw Material Consumed',2,1,'Dr',1)");
                    MyBase.Execute(" insert into pandL_subheading_master_V values (4,'Administrative Expenses',2,4,'Dr',1)");
                    MyBase.Execute(" insert into pandL_subheading_master_V values (5,'Staff Remuneration & Benefits',2,3,'Dr',1)");
                    MyBase.Execute(" insert into pandL_subheading_master_V values (6,'Selling & Distribution Charges',2,4,'Dr',1)");
                    MyBase.Execute(" insert into pandL_subheading_master_V values (7,'Financial Charges',2,5,'Dr',1)");
                    MyBase.Execute(" insert into pandL_subheading_master_V values (8,'Depriciation',2,6,'Dr',1)");
                    MyBase.Execute(" insert into pandL_subheading_master_V values (9,'Stores & Spares Consumed',2,2,'Dr',1)");
                    MyBase.Execute(" insert into pandL_subheading_master_V values (10,'Repairs & Maintenance',2,3,'Dr',1)");


                    // Balance 
                    MyBase.Execute(" insert into Balance_subheading_master_V values (1,'Share Holders Fund',1,1,'Cr',1)");
                    MyBase.Execute(" insert into Balance_subheading_master_V values (2,'Loan Funds',1,2,'Cr',1)");
                    MyBase.Execute(" insert into Balance_subheading_master_V values (3,'Fixed Asstes',2,1,'Dr',1)");
                    MyBase.Execute(" insert into Balance_subheading_master_V values (4,'Investments',2,2,'Dr',1)");
                    MyBase.Execute(" insert into Balance_subheading_master_V values (5,'Current Assets, Loans & Advances',2,3,'Dr',1)");
                    MyBase.Execute(" insert into Balance_subheading_master_V values (6,'Current Liablities & Provisions',2,4,'Cr',1)");
                    MyBase.Execute(" insert into Balance_subheading_master_V values (7,'Miscelleneous Expenditure',2,5,'Dr',1)");
                    MyBase.Execute(" insert into Balance_subheading_master_V values (8,'Deferred Tax Liability',1,3,'Dr',1)");
                    MyBase.Execute(" insert into Balance_subheading_master_V values (9,'Deferred Tax Asset',2,3,'Dr',1)");
                    MyBase.Execute(" insert into Balance_subheading_master_V values (10,'Profit & Loss A/c',2,6,'Dr',1)");

                    // GroupMas_Setting

                    MyBase.Execute(" insert into groupmas_Setting values (1500,1500,'B','V',1,1,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");
                    MyBase.Execute(" insert into groupmas_Setting values (2300,2300,'B','V',2,1,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");
                    MyBase.Execute(" insert into groupmas_Setting values (1200,1200,'B','V',5,2,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (3200,3200,'B','V',3,1,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (1700,1700,'B','V',5,3,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (1900,1900,'B','V',1,2,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (3100,3100,'B','V',5,4,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (6600,6600,'B','V',6,1,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");
                    MyBase.Execute(" insert into groupmas_Setting values (1800,1800,'B','V',5,5,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (2400,2400,'B','V',3,2,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (2100,2100,'B','V',5,6,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (6200,6200,'B','V',5,7,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (3800,3800,'B','V',5,8,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (3600,3600,'B','V',6,2,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");
                    MyBase.Execute(" insert into groupmas_Setting values (5400,5400,'B','V',6,3,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");
                    MyBase.Execute(" insert into groupmas_Setting values (4600,4600,'B','V',2,2,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");
                    MyBase.Execute(" insert into groupmas_Setting values (4400,4400,'P','V',1,1,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");
                    MyBase.Execute(" insert into groupmas_Setting values (5500,5500,'P','V',4,1,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (2200,2200,'P','V',4,2,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (4300,4300,'P','V',2,1,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (6400,6400,'P','V',4,3,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (4900,4900,'P','V',4,4,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (6500,6500,'P','V',2,2,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");
                    MyBase.Execute(" insert into groupmas_Setting values (3700,3700,'P','V',2,3,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");

                    MyBase.Execute(" insert into groupmas_Setting values (3400,3400,'P','V',3,1,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (4200,4200,'P','V',3,2,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");
                    MyBase.Execute(" insert into groupmas_Setting values (4100,4100,'P','V',3,3,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (1800,1800,'P','V',3,4,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");

                    MyBase.Execute(" insert into groupmas_Setting values (3500,3500,'P','V',2,4,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");
                    MyBase.Execute(" insert into groupmas_Setting values (4500,4500,'P','V',1,2,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (5600,5600,'P','V',6,1,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (1300,1300,'B','V',6,5,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");
                    MyBase.Execute(" insert into groupmas_Setting values (2000,2000,'P','V',4,5,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (1400,1400,'P','V',1,3,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");
                    MyBase.Execute(" insert into groupmas_Setting values (6000,6000,'B','V',10,1,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (3300,3300,'B','V',10,2,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (1600,1600,'B','V',5,1,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (4700,4700,'B','V',6,4,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");
                    MyBase.Execute(" insert into groupmas_Setting values (4800,4800,'B','V',5,9,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Dr')");
                    MyBase.Execute(" insert into groupmas_Setting values (3000,3000,'B','V',2,3,'+'," + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "','Cr')");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Check_Balance_Sheet()
        {
            try
            {
                if (MyBase.Check_Table("Balance_Heading_Master_V") == false)
                {
                    MyBase.Execute(" Create table Balance_Heading_Master_V (Code int, Head_Name varchar(50), Order_Slno int)");

                    MyBase.Execute(" insert into Balance_Heading_Master_V values (1, 'Sources Of Funds', 1)");
                    MyBase.Execute(" insert into Balance_Heading_Master_V values (2, 'Application Of Funds', 2)");
                }


                if (MyBase.Check_Table("Balance_SubHeading_Master_V"))
                {
                    if (MyBase.Check_TableField("Balance_SubHeading_Master_V", "Type") == false)
                    {
                        MyBase.Execute("Drop table Balance_SubHeading_Master_V");
                    }
                }

                if (MyBase.Check_Table("Balance_SubHeading_Master_V") == false)
                {
                    MyBase.Execute(" create table Balance_SubHeading_Master_V (Code int, SubHead_Name varchar(50), Head_Code int, Order_Slno int, type varchar(2), Company_Code int)");

                    MyBase.Execute(" insert into Balance_SubHeading_Master_V values (1, 'Share Holders Fund', 1, 1, 'Cr', " + CompCode + ")");
                    MyBase.Execute(" insert into Balance_SubHeading_Master_V values (2, 'Loans Of Funds', 1, 2, 'Cr', " + CompCode + ")");

                    MyBase.Execute(" insert into Balance_SubHeading_Master_V values (3, 'Fixed Asstes', 2, 1, 'Dr', " + CompCode + ")");
                    MyBase.Execute(" insert into Balance_SubHeading_Master_V values (4, 'Capital Work in Progress', 2, 2, 'Dr', " + CompCode + ")");
                    MyBase.Execute(" insert into Balance_SubHeading_Master_V values (5, 'Current Assets, Loans & Advances', 2, 3, 'Dr', " + CompCode + ")");
                    MyBase.Execute(" insert into Balance_SubHeading_Master_V values (6, 'Less:Current Liablities & Provisions', 2, 4, 'Cr', " + CompCode + ")");
                }
                //if (MyBase.Get_RecordCount("Balance_SubHeading_Master_V", "Code = 7 and company_Code = " + CompCode) == 0)
                //{
                //    MyBase.Execute(" insert into Balance_SubHeading_Master_V values (7, 'Miscelleneous Expenditure', 2, 5, 'Dr', " + CompCode + ")");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Update_Group_Master()
        {
            try
            {
                if (MyBase.Check_Table("GroupMas_Temp"))
                {
                    MyBase.Add_NewField("GroupMas_temp", "Type", "Varchar(1)");
                    MyBase.Add_NewField("GroupMas_temp", "Head_Code", "int");
                }
                if (MyBase.Check_Table("GroupMas"))
                {
                    MyBase.Add_NewField("GroupMas", "Type", "Varchar(1)");
                    MyBase.Add_NewField("GroupMas", "Head_Code", "int");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Split_ledger_breakup()
        {
            try
            {
                if (CompName.Contains("AEGAN"))
                {
                    if (MyBase.Check_Table("ledger_breakup_Temp") == false)
                    {
                        MyBase.Execute("Select * into Ledger_breakup_temp from ledger_breakup where 1 = 2");
                        if (MyBase.Get_RecordCount("Ledger_breakup", "Year_Code = '2008-2009'") > 0)
                        {
                            MyBase.Execute("Insert into ledger_breakup_temp select * from ledger_breakup where year_Code = '2008-2009'");
                            MyBase.Execute("Delete from ledger_breakup where year_Code = '2008-2009'");
                            if (MyBase.Get_RecordCount("Ledger_breakup", "Year_Code = '2009-2010'") > 0)
                            {
                                MyBase.Execute("Insert into ledger_breakup_temp select * from ledger_breakup where year_Code = '2009-2010'");
                                MyBase.Execute("Delete from ledger_breakup where year_Code = '2009-2010'");
                            }
                            MyBase.Execute("Insert into ledger_breakup_temp select * from ledger_breakup where ledger_Code > 230");
                            MyBase.Execute("Delete from ledger_breakup where ledger_Code > 315");

                            MyBase.Execute("Insert into ledger_breakup_temp select * from ledger_breakup where RefDoc like 'J%'");
                            MyBase.Execute("Delete from ledger_breakup where RefDoc like 'J%'");

                            MyBase.Execute("Insert into ledger_breakup_temp select * from ledger_breakup where RefDoc like 'G%'");
                            MyBase.Execute("Delete from ledger_breakup where RefDoc like 'G%'");

                            MyBase.Execute("Insert into ledger_breakup_temp select * from ledger_breakup where Company_Code = 2");
                            MyBase.Execute("Delete from ledger_breakup where Company_Code = 2");
                        }
                    }

                    if (MyBase.Check_Table("Voucher_breakup_Bills_Temp") == false)
                    {
                        MyBase.Execute("Select * into Voucher_breakup_Bills_temp from Voucher_breakup_Bills where 1 = 2");
                        if (MyBase.Get_RecordCount("Voucher_breakup_Bills", "Year_Code = '2008-2009'") > 0)
                        {
                            MyBase.Execute("Insert into Voucher_breakup_Bills_temp select * from Voucher_breakup_Bills where year_Code = '2008-2009'");
                            MyBase.Execute("Delete from Voucher_breakup_Bills where year_Code = '2008-2009'");
                            if (MyBase.Get_RecordCount("Voucher_breakup_Bills", "Year_Code = '2009-2010'") > 0)
                            {
                                MyBase.Execute("Insert into Voucher_breakup_Bills_temp select * from Voucher_breakup_Bills where year_Code = '2009-2010'");
                                MyBase.Execute("Delete from Voucher_breakup_Bills where year_Code = '2009-2010'");
                            }
                            MyBase.Execute("Insert into Voucher_breakup_Bills_temp select * from Voucher_breakup_Bills where ledger_Code > 230");
                            MyBase.Execute("Delete from Voucher_breakup_Bills where ledger_Code > 315");

                            MyBase.Execute("Insert into Voucher_breakup_Bills_temp select * from Voucher_breakup_Bills where RefDOc like 'J%'");
                            MyBase.Execute("Delete from Voucher_breakup_Bills where RefDOc like 'J%'");

                            MyBase.Execute("Insert into Voucher_breakup_Bills_temp select * from Voucher_breakup_Bills where RefDOc like 'G%'");
                            MyBase.Execute("Delete from Voucher_breakup_Bills where RefDOc like 'G%'");

                            MyBase.Execute("Insert into Voucher_breakup_Bills_temp select * from Voucher_breakup_Bills where Company_Code = 2");
                            MyBase.Execute("Delete from Voucher_breakup_Bills where Company_Code = 2");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete_MisMatch_Values_Vcouher()
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("select vcode, sum(debit), sum(credit) from voucher_Details where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' group by vcode having (sum(debit) = sum(credit) and sum(debit) = 0)", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Delete from Voucher_Master where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    MyBase.Execute("Delete from Voucher_Details where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    MyBase.Execute("Delete from Voucher_Breakup_Bills where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    MyBase.Execute("Delete from Ledger_breakup where Ref = '" + Dt.Rows[i]["Vcode"].ToString() + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }
                MyBase.Load_Data("select vcode, sum(debit), sum(credit) from voucher_Details where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' group by vcode having (sum(cast(debit as numeric(15))) <> sum(cast(credit as numeric(15))))", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Delete from Voucher_Master where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    MyBase.Execute("Delete from Voucher_Details where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    MyBase.Execute("Delete from Voucher_Breakup_Bills where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    MyBase.Execute("Delete from Ledger_breakup where Ref = '" + Dt.Rows[i]["Vcode"].ToString() + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                MyBase.Load_Data("select vcode from voucher_details where ledger_code Not in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "') and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Delete from Voucher_Master where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    MyBase.Execute("Delete from Voucher_Details where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    MyBase.Execute("Delete from Voucher_Breakup_Bills where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    MyBase.Execute("Delete from Ledger_breakup where Ref = '" + Dt.Rows[i]["Vcode"].ToString() + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_Voucher_Breakup_Debit_Equal_Credit()
        {
            DataTable Dt = new DataTable();
            DataTable Dt1 = new DataTable();
            try
            {
                MyBase.Load_Data("Select * from voucher_breakup_bills where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and debit = credit", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Load_Data("select vcode, ledger_Code, debit, credit, (case when debit > 0 then 'CR' else 'DR' end) Bterm from voucher_Details where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and company_Code = " + CompCode + "  and year_Code = '" + YearCode + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString(), ref Dt1);
                    if (Dt1.Rows.Count > 0)
                    {
                        if (Dt1.Rows[0]["Bterm"].ToString() == "DR")
                        {
                            MyBase.Execute("Update voucher_Breakup_bills set Credit = " + Dt1.Rows[0]["Credit"].ToString() + ", BTerm = 'DR', Debit = 0 where vcode = " + Dt1.Rows[0]["Vcode"].ToString() + " and ledger_Code = " + Dt1.Rows[0]["Ledger_Code"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                        }
                        else
                        {
                            MyBase.Execute("Update voucher_Breakup_bills set Debit = " + Dt1.Rows[0]["debit"].ToString() + ", BTerm = 'CR', Credit = 0 where vcode = " + Dt1.Rows[0]["Vcode"].ToString() + " and ledger_Code = " + Dt1.Rows[0]["Ledger_Code"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_Voucher_Breakup_CR_DR()
        {
            try
            {
                if (MyBase.Get_RecordCount("Voucher_breakup_Bills", "Company_Code = " + CompCode + " and Year_Code = '" + YearCode + "' and Bterm = 'CR' and Credit > 0") > 0)
                {
                    MyBase.Execute("update voucher_breakup_bills set Credit = 0 where Company_Code = " + CompCode + " and Year_Code = '" + YearCode + "' and Bterm = 'CR' and Credit > 0");
                }

                if (MyBase.Get_RecordCount("Voucher_breakup_Bills", "Company_Code = " + CompCode + " and Year_Code = '" + YearCode + "' and Bterm = 'DR' and Debit > 0") > 0)
                {
                    MyBase.Execute("update voucher_breakup_bills set Debit = 0 where Company_Code = " + CompCode + " and Year_Code = '" + YearCode + "' and Bterm = 'DR' and Debit > 0");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_Closing_Stock_ledger()
        {
            try
            {
                if (MyBase.Check_Table("Ledger_Master"))
                {
                    MyBase.Add_NewField("Ledger_Master", "Cheque_Name", "Varchar(250)");
                    MyBase.Add_NewField("Ledger_Master", "SubLedger", "Varchar(1)");
                    MyBase.Add_NewField("Ledger_Master", "PANNo", "Varchar(50)");
                    if (MyBase.Get_RecordCount("Ledger_Master", "Subledger is null") > 0)
                    {
                        MyBase.Execute("Update Ledger_Master set Subledger = 'N' where subledger is null");
                    }

                    MyBase.Add_NewField("Ledger_Master", "TDSApplicable", "Varchar(1)");
                    MyBase.Add_NewField("Ledger_Master", "TDSType", "int");
                    MyBase.Add_NewField("Ledger_Master", "Section_No", "Varchar(5)");
                    MyBase.Add_NewField("Ledger_Master", "TdsRatePer", "float");

                    if (MyBase.Get_RecordCount("ledger_master", "Ledger_Code = -1 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                    {
                        //MyBase.Execute(" insert into ledger_master select -1, 'CLOSING STOCK', null, null, 1800, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, " + CompCode + ", '" + YearCode + "', null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null");
                        MyBase.Execute(" insert into ledger_master (ledger_Code, Ledger_Name, ledger_group_code, company_Code, year_Code) select -1, 'CLOSING STOCK', 1800, " + CompCode + ", '" + YearCode + "'");
                    }
                    if (MyBase.Get_RecordCount("ledger_master", "Ledger_Code = -1 and GROUP_ORDER_SLNO = 100 AND company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                    {
                        MyBase.Execute("Update Ledger_Master set group_order_slno = 100 where ledger_code = -1 and company_code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Opening_Stock_Updation()
        {
            String Str = String.Empty, Str1 = String.Empty;
            try
            {
                if (MyBase.Check_Table("Closing_Stock"))
                {
                    if (MyBase.Get_RecordCount("Closing_Stock", "Edate = '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") > 0)
                    {
                        Str1 = "Delete from Closing_Stock where edate = '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'";
                    }
                    Str = "Insert into Closing_Stock select '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', ledger_Code, ledger_Odebit, 0, " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + " , '" + YearCode + "' from ledger_master where ledger_Group_Code = 3400 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'";
                    MyBase.Run(Str1, Str);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Update_Fixed_Assets()
        {
            try
            {
                if (MyBase.Get_RecordCount("GroupMas", "GroupCode = 2400 and groupunder = 2400 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 2400, GroupReserved = 2400 where groupcode = 2400 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("GroupMas", "GroupCode = 3600 and groupunder = 3600 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 3600, GroupReserved = 3600 where groupcode = 3600 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas_Temp", "GroupCode = 2400 and groupunder = 2400 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas_Temp set groupUnder = 2400, GroupReserved = 2400 where groupcode = 2400 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("GroupMas_Temp", "GroupCode = 3600 and groupunder = 3600 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas_Temp set groupUnder = 3600, GroupReserved = 3600 where groupcode = 3600 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "GroupCode = 7077 and groupunder = 7077 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 7077, GroupReserved = 7077 where groupcode = 7077 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("GroupMas_Temp", "GroupCode = 7077 and groupunder = 7077 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas_Temp set groupUnder = 7077, GroupReserved = 7077 where groupcode = 7077 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "GroupCode = 7064 and groupunder = 7064 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 7064, GroupReserved = 7064 where groupcode = 7064 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("GroupMas_Temp", "GroupCode = 7064 and groupunder = 7064 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas_Temp set groupUnder = 7064, GroupReserved = 7064 where groupcode = 7064 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas_Temp", "GroupCode = 3100 and groupunder = 3100 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas_Temp set groupcode = 3100, groupUnder = 3100, GroupReserved = 3100 where groupcode = 7062 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("GroupMas", "GroupCode = 3100 and groupunder = 3100 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupcode = 3100, groupUnder = 3100, GroupReserved = 3100 where groupcode = 7062 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                    if (MyBase.Get_RecordCount("ledger_master", "ledger_Group_Code = 7062 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") > 0)
                    {
                        MyBase.Execute("update ledger_master set ledger_group_Code = 3100 where ledger_group_code = 7062 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    }
                    if (MyBase.Get_RecordCount("GrouPMas", "grouPreserved = 7062 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") > 0)
                    {
                        MyBase.Execute("Update groupmas set groupreserved = 3100 where grouPreserved = 7062 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    }
                    if (MyBase.Get_RecordCount("GrouPMas", "grouPUnder = 7062 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") > 0)
                    {
                        MyBase.Execute("Update groupmas set groupUnder = 3100 where groupunder = 7062 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    }
                }

                if (MyBase.Get_RecordCount("GroupMas_Temp", "GroupCode = 3000 and groupunder = 3000 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas_Temp set groupcode = 3000, groupUnder = 3000, GroupReserved = 3000 where groupcode = 7056 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("GroupMas", "GroupCode = 3000 and groupunder = 3000 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupcode = 3000, groupUnder = 3000, GroupReserved = 3000 where groupcode = 7056 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                    if (MyBase.Get_RecordCount("ledger_master", "ledger_Group_Code = 7056 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") > 0)
                    {
                        MyBase.Execute("update ledger_master set ledger_group_Code = 3000 where ledger_group_code = 7056 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    }
                }

                if (MyBase.Get_RecordCount("GroupMas_Temp", "GroupCode = 7007 and groupname like 'BRANCH DIVI%' and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("Update GroupMas_Temp set groupUnder = 4700, GroupReserved = 4700 where groupcode = 7007 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("GroupMas", "GroupCode = 7007 and groupname like 'BRANCH DIVI%' and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 4700, GroupReserved = 4700 where groupcode = 7007 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }


                if (MyBase.Get_RecordCount("GroupMas_Temp", "GroupCode = 7013 and groupname like 'CAPITAL RE%' and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("Update GroupMas_Temp set groupUnder = 1500, GroupReserved = 1500 where groupcode = 7013 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("GroupMas", "GroupCode = 7013 and groupname like 'CAPITAL RE%' and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 1500, GroupReserved = 1500 where groupcode = 7013 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }


                if (MyBase.Get_RecordCount("GroupMas_Temp", "GroupCode = 7064 and groupname like 'DEPOSITS%' and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("Update GroupMas_Temp set groupUnder = 1200, GroupReserved = 1200 where groupcode = 7064 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("GroupMas", "GroupCode = 7064 and groupname like 'DEPOSITS%' and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 1200, GroupReserved = 1200 where groupcode = 7064 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas_Temp", "GroupCode = 7081 and groupname like 'ACCRU%' and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("Update GroupMas_Temp set groupUnder = 1200, GroupReserved = 1200 where groupcode = 7081 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("GroupMas", "GroupCode = 7081 and groupname like 'ACCRU%' and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 1200, GroupReserved = 1200 where groupcode = 7081 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas_Temp", "GroupCode = 7077 and groupname like 'MANUFACTURING%' and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("Update GroupMas_Temp set groupUnder = 6400, GroupReserved = 6400 where groupcode = 7077 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("GroupMas", "GroupCode = 7077 and groupname like 'MANUFACTURING%' and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 6400, GroupReserved = 6400 where groupcode = 7077 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas_Temp", "GroupCode = 7016 and groupName like '%Progress%' and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("Update GroupMas_Temp set Groupcode = 3200, groupUnder = 3200, GroupReserved = 3200 where groupcode = 7016 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("GroupMas", "GroupCode = 7016 and groupName like '%Progress%' and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("Update GroupMas set Groupcode = 3200, groupUnder = 3200, GroupReserved = 3200 where groupcode = 7016 and company_Code = " + CompCode + " and Year_Code = '" + YearCode + "'");
                    if (MyBase.Get_RecordCount("ledger_master", "ledger_Group_Code = 7016 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") > 0)
                    {
                        MyBase.Execute("update ledger_master set ledger_group_Code = 3200 where ledger_group_code = 7016 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void GroupMaster_Initialize_Setting()
        {
            try
            {
                if (MyBase.Check_Table("GroupMas_Setting") == false)
                {
                    MyBase.Execute("create table GroupMas_Setting (Groupcode int, Parent_Group int, type varchar(1), VOrH Varchar(1), Subhead_Code int, order_Slno int, Operator_Symbol varchar(1))");
                }
                MyBase.UpdateSpecialFields("Groupmas_Setting");
                
                if (MyBase.Get_RecordCount("GroupMas_Setting", "Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    // P & L Vertical
                    MyBase.Execute("Insert into GroupMas_Setting Values (4400, 4400, 'P', 'V', 1, 1, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (4500, 4500, 'P', 'V', 1, 2, '-', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");

                    MyBase.Execute("Insert into GroupMas_Setting Values (3500, 3500, 'P', 'V', 2, 1, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (3700, 3700, 'P', 'V', 2, 2, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (6500, 6500, 'P', 'V', 2, 3, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");

                    MyBase.Execute("Insert into GroupMas_Setting Values (1800, 1800, 'P', 'V', 3, 4, '-', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (3400, 3400, 'P', 'V', 3, 1, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (4100, 4100, 'P', 'V', 3, 2, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (4200, 4200, 'P', 'V', 3, 3, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");

                    MyBase.Execute("Insert into GroupMas_Setting Values (4900, 4900, 'P', 'V', 4, 1, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (5500, 5500, 'P', 'V', 4, 1, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");

                    MyBase.Execute("Insert into GroupMas_Setting Values (6400, 6400, 'P', 'V', 5, 1, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");

                    MyBase.Execute("Insert into GroupMas_Setting Values (2000, 2000, 'P', 'V', 6, 1, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (5600, 5600, 'P', 'V', 6, 2, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");

                    MyBase.Execute("Insert into GroupMas_Setting Values (1300, 1300, 'P', 'V', 8, 1, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (3300, 3300, 'P', 'V', 8, 2, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (6000, 6000, 'P', 'V', 8, 3, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");


                    // Balance Sheet Vertical
                    MyBase.Execute("Insert into GroupMas_Setting Values (1500, 1500, 'B', 'V', 1, 1, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (1900, 1900, 'B', 'V', 1, 2, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");

                    MyBase.Execute("Insert into GroupMas_Setting Values (2300, 2300, 'B', 'V', 2, 3, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (4600, 4600, 'B', 'V', 2, 4, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");

                    MyBase.Execute("Insert into GroupMas_Setting Values (2200, 2200, 'B', 'V', 3, 5, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (2400, 2400, 'B', 'V', 3, 6, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");

                    MyBase.Execute("Insert into GroupMas_Setting Values (3200, 3200, 'B', 'V', 4, 7, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");

                    MyBase.Execute("Insert into GroupMas_Setting Values (1400, 1400, 'B', 'V', 5, 8, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (4800, 4800, 'B', 'V', 5, 9, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (1600, 1600, 'B', 'V', 5, 10, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (1700, 1700, 'B', 'V', 5, 11, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (1200, 1200, 'B', 'V', 5, 12, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (2100, 2100, 'B', 'V', 5, 13, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (6200, 6200, 'B', 'V', 5, 14, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");

                    MyBase.Execute("Insert into GroupMas_Setting Values (3600, 3600, 'B', 'V', 6, 15, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (4300, 4300, 'B', 'V', 6, 16, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (4700, 4700, 'B', 'V', 6, 17, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (5400, 5400, 'B', 'V', 6, 18, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                    MyBase.Execute("Insert into GroupMas_Setting Values (6600, 6600, 'B', 'V', 6, 19, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");

                    MyBase.Execute("Insert into GroupMas_Setting Values (3800, 3800, 'B', 'V', 7, 20, '+', " + UserCode + ", " + SysCode + ", " + Today + ", " + UserCode + ", " + SysCode + ", " + Today + ", " + CompCode + ", '" + YearCode + "')");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Remove_Unnecesary_Breakup()
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Execute_Qry("select Vcode, vdate, ledger_Code, Mode, refDoc, refDate, debit B_Debit, credit B_Credit, company_Code, year_Code from voucher_breakup_Bills where company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", "Brek1");

                MyBase.Execute_Qry("select v1.*, v2.byto, v2.ledger_Code ledcode, v2.debit, v2.credit from brek1 v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and  v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code and v1.ledger_Code = v2.ledger_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' ", "Brek2");

                MyBase.Load_Data("select * from Brek2 where byto is null and ledcode is null and debit is null and credit is null and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", ref Dt);

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyBase.Get_RecordCount("Voucher_Details", "Vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["Vdate"])) + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                    {
                        MyBase.Execute("Delete from voucher_Breakup_Bills where Vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["Vdate"])) + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                        MyBase.Execute("Delete from ledger_breakup where ref = '" + Dt.Rows[i]["Vcode"].ToString() + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and debit = " + Dt.Rows[i]["B_Debit"].ToString() + " and Credit = " + Dt.Rows[i]["B_Credit"].ToString() + " and Mode = '" + Dt.Rows[i]["Mode"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["refDate"])) + "' and refdoc = '" + Dt.Rows[i]["refdoc"].ToString() + "'");
                    }
                }

                MessageBox.Show("Ok ...!", "Vaahini");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_Breakup_Sundry()
        {
            try
            {
                if (MyBase.Get_RecordCount("Ledger_Master", "Breakup <> 'Y' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_group_code in (select groupcode from groupmas where groupname like 'SUNDRY%' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "')") > 0)
                {
                    MyBase.Execute("update ledger_master set breakup = 'Y' where Breakup <> 'Y' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_group_code in (select groupcode from groupmas where groupname like 'SUNDRY%' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "')");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Check_Linked_Server_Available(String ServerName)
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("Sp_LinkedServers", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["Srv_Name"].ToString().ToUpper() == ServerName.ToUpper())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void OutStanding_Creditors(DateTime AsONdate, Boolean FIFO)
        {
            String Str = String.Empty;
            DataTable Dt = new DataTable();
            DataTable Dt1 = new DataTable();
            Double Amount = 0;
            String RefDoc1 = "";
            Double Debit = 0;
            Double Credit = 0;
            try
            {
                //Ref_VCH_DB yearcode and company_Code condition Applied on 281010

                if (FIFO == false)
                {
                    MyBase.Execute_Qry("select v1.vcode, v1.vdate, v2.user_date, v1.ledger_Code, v1.debit, v1.credit, v1.company_Code, v1.year_Code from voucher_Details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_code = v2.company_code and v1.year_Code = v2.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' and v1.ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_group_code in (Select groupcode from groupmas where groupreserved = 4700 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'))", "Brk1");
                    MyBase.Execute_Qry(" select v1.*, v2.refdoc, v2.refdate from Brk1 v1 left join voucher_breakup_bills v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.ledger_code = v2.ledger_Code and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "'", "Brk2");

                    // New On 02082010                
                    //Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, (case when v1.RefDate is null then cast('" + String.Format("{0:dd-MMM-yyyy}", SDate) + "' as Datetime) else v1.refdate end) RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code, isnull(l1.duedays, 0) Duedays from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_breakup l1 on v1.ledger_Code = l1.ledger_code and v1.refDoc = l1.refdoc and v1.refdate = l1.refdate and v1.mode = l1.mode and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code where v1.mode = 'N' and v2.user_Date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' union ";
                    Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, (case when v1.RefDate is null then v2.user_Date else v1.refdate end) RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code, isnull(l1.duedays, 0) Duedays from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_breakup l1 on v1.ledger_Code = l1.ledger_code and v1.refDoc = l1.refdoc and v1.refdate = l1.refdate and v1.mode = l1.mode and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code where v1.mode = 'N' and v2.user_date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "'  and v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "'";
                    MyBase.Execute_Qry(Str, "Refdate_Vch");
                    // Upto 

                    Str = "Select * from refdate_Vch where refdate <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' union ";
                    Str += " select vcode, vdate, user_date, ledger_code, 'N', 'W.Br.New', user_date, debit, credit, company_Code, year_Code, 0 from Brk2 v2 where refdoc is null and refdate is null and user_date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' union ";
                    Str += " Select null vcode, null vdate, '31-Mar-2010', Ledger_Code, Mode, refDoc, (case when RefDate is null then cast('" + String.Format("{0:dd-MMM-yyyy}", SDate) + "' as Datetime) else refdate end) RefDate, Debit, credit, Company_Code, Year_Code, 0 from ledger_breakup where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ref = 'L1' and term = 'LEDGER' union ";
                    Str += "Select null vcode, null vdate, '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Code, 'N', 'Opn', '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Odebit, ledger_OCredit, Company_Code, Year_Code, 0 from ledger_master where ((Ledger_Odebit > 0) or (ledger_Ocredit >0)) and Breakup = 'Y' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code not in (Select ledger_Code from ledger_breakup where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ref = 'L1' and term = 'LEDGER')";
                    MyBase.Execute_Qry(Str, "New_out1");

                    //Commented On 030511
                    //Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, v1.RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.mode = 'A' and v2.user_Date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' and v2.company_Code = " + CompCode + " and v2.year_Code = '" + YearCode + "' ";

                    Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, v1.RefDate, (case when v1.bterm = 'DR' then 0 else v1.debit end) Debit, (case when v1.bterm = 'CR' then 0 else v1.Credit end) Credit, v1.company_Code, v1.year_Code from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.mode = 'A' and v2.user_Date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' and v2.company_Code = " + CompCode + " and v2.year_Code = '" + YearCode + "' ";
                    MyBase.Execute_Qry(Str, "Ref_out1");

                    //Str = "select vcode, vdate, user_date, ledger_Code, Mode, refDoc, refdate, (-1) * debit Debit, Credit, company_Code, year_Code, duedays from new_out1 where ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Group_Code in (Select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4700)) union ";
                    //Str += "Select *, 0 as DueDays from ref_out1 where credit > 0 ";

                    //MyBase.Execute_Qry(Str, "New_Out");

                    //// On 01-10-10
                    //MyBase.Execute_Qry("Select vcode, vdate, User_date, ledger_Code, mode, refdoc, refdate, (case when isnull(Debit, 0) <> 0 then isnull(Debit, 0) else isnull(credit, 0) end) Amount, (case when isnull(Debit, 0) <> 0 then 'Dr' else 'Cr' end) Type, company_Code, year_Code from ref_out1 where debit > 0 and ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Group_Code in (Select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4700))", "Ref_out");

                    //if (MyBase.Check_Table("Main_Outstanding"))
                    //{
                    //    if (MyBase.Check_TableField("Main_Outstanding", "Vcode") == false)
                    //    {
                    //        MyBase.Execute("Drop table Main_Outstanding");
                    //    }
                    //}

                    //if (MyBase.Check_Table("Main_Outstanding") == false)
                    //{
                    //    MyBase.Execute(" create table Main_Outstanding (Vcode bigint, vdate datetime, Ledger_Code int, Mode varchar(2), RefDoc varchar(100), refdate datetime, Due int, DueDate datetime, Amount numeric(20,2), Type varchar(2), Pending int, Balance numeric(20,2), Company_Code int, year_Code varchar(10))");
                    //}

                    //MyBase.Execute("Delete from Main_outstanding where company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    //MyBase.Execute("Insert into Main_outstanding Select vcode, vdate, Ledger_Code, Mode, refDoc, refdate, Duedays, '01-Jan-1899', (case when isnuLL(Debit, 0) <> 0 then isnull(debit, 0) else isnull(credit, 0) end), (case when isnuLL(Debit, 0) <> 0 then 'Dr' else 'Cr' end), 0, (case when isnuLL(Debit, 0) <> 0 then isnull(debit, 0) else isnull(credit, 0) end), company_Code, year_Code from new_out");


                    //MyBase.Execute("Update Main_outstanding set amount = (-1) * Amount where amount < 0");
                    //MyBase.Execute("Update Main_outstanding set Balance = (-1) * Balance where Balance < 0");

                    ////MyBase.Load_Data("Select vcode, vdate, Ledger_Code, refdoc, refDate, Sum(Amount) Amount, company_Code, year_Code, type from ref_out group by ledger_Code, refDoc, refdate, company_Code, year_Code, type, vcode, vdate Order by ledger_Code", ref Dt);
                    //MyBase.Load_Data("Select vcode, vdate, Ledger_Code, refdoc, refDate, Sum(Amount) Amount, company_Code, year_Code, type from ref_out where ledger_Code = 76 group by ledger_Code, refDoc, refdate, company_Code, year_Code, type, vcode, vdate Order by ledger_Code", ref Dt);

                    //for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    //{
                    //    Amount = Convert.ToDouble(Dt.Rows[i]["Amount"]);
                    //    RefDoc1 = Convert.ToString(Dt.Rows[i]["RefDoc"]);

                    //    // Without RefDate Condition
                    //    //MyBase.Load_Data("Select * from Main_outStanding where ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString() + "' and refDate = '" + String.Format ("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' order by refdate", ref Dt1);

                    //    MyBase.Load_Data("Select * from Main_outStanding where ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString() + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' order by refdate", ref Dt1);
                    //    for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                    //    {
                    //        if (Convert.ToDouble(Dt1.Rows[j]["Balance"]) > Amount)
                    //        {
                    //            MyBase.Execute("Update Main_outStanding set Balance = " + Convert.ToString(Convert.ToDouble(Dt1.Rows[j]["Balance"]) - Amount) + " where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and Mode = '" + Dt1.Rows[j]["Mode"].ToString() + "' and refDoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and Amount = " + Convert.ToDouble(Dt1.Rows[j]["Balance"]));
                    //            Amount = 0;
                    //            break;
                    //        }
                    //        else if (Convert.ToDouble(Dt1.Rows[j]["Balance"]) < Amount)
                    //        {
                    //            //MyBase.Execute("Update Main_outStanding set Balance = " + Convert.ToString(Amount - Convert.ToDouble(Dt1.Rows[j]["Balance"])) + " where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and Mode = '" + Dt1.Rows[j]["Mode"].ToString() + "' and refDoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    //            MyBase.Execute("Update Main_outStanding set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and Mode = '" + Dt1.Rows[j]["Mode"].ToString() + "' and refDoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and Amount = " + Convert.ToDouble(Dt1.Rows[j]["Balance"]));
                    //            Amount = Amount - Convert.ToDouble(Dt1.Rows[j]["Amount"]);
                    //        }
                    //        else if (Convert.ToDouble(Dt1.Rows[j]["Balance"]) == Amount)
                    //        {
                    //            MyBase.Execute("Update Main_outStanding set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and Mode = '" + Dt1.Rows[j]["Mode"].ToString() + "' and refDoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and Amount = " + Convert.ToDouble(Dt1.Rows[j]["Balance"]));
                    //            Amount = 0;
                    //            break;
                    //        }
                    //    }
                    //    if (Amount > 0)
                    //    {
                    //        MyBase.Execute("Insert into Main_outStanding Select " + Dt.Rows[i]["Vcode"].ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["Vdate"])) + "', " + Dt.Rows[i]["Ledger_Code"].ToString() + ", 'A', '" + Dt.Rows[i]["RefDoc"].ToString() + "', '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"])) + "', 0, '01-Jan-1899', " + Amount + ", '" + Dt.Rows[i]["Type"].ToString() + "', 0, " + Amount + ", " + CompCode + ", '" + YearCode + "'");
                    //    }
                    //}
                    //MyBase.Execute("Update Main_outstanding set amount = (-1) * Amount where type = 'Dr'");
                    //MyBase.Execute("Update Main_outstanding set Balance = (-1) * Balance where type = 'Dr'");

                    DataTable TempDt1 = new DataTable();

                    MyBase.Execute_Qry("select * from New_out1 union select *, 0 DueDays from Ref_out1 ", "Main_outstanding1");
                    MyBase.Execute_Qry("Select * from Main_outstanding1 where ledger_Code in (select ledger_Code from Ledger_master where ledger_group_Code in (select groupCode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4700) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') ", "Main_outstanding21");
                    MyBase.Execute_Tbl("Select * from Main_outstanding21", "Main_outstanding2");

                    MyBase.Load_Data ("Select distinct Ledger_Code from Main_outstanding2", ref Dt);
                    for (int i=0;i<=Dt.Rows.Count - 1;i++)
                    {
                        MyBase.Load_Data ("select Ledger_Code, RefDoc, RefDate, isnull(Debit, 0) Debit, isnull(Credit, 0) Credit from Main_outstanding2 where ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and Debit <> 0", ref Dt1);
                        for (int j=0;j<=Dt1.Rows.Count - 1;j++)
                        {
                            Debit = Convert.ToDouble(Dt1.Rows[j]["Debit"]);
                            MyBase.Load_Data ("Select * from Main_outstanding2 where ledger_Code = " + Dt.Rows[i]["ledger_Code"].ToString() + " and refDoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["RefDate"])) + "' and Credit <> 0", ref TempDt1);
                            for (int k=0;k<=TempDt1.Rows.Count - 1;k++)
                            {
                                Credit = Convert.ToDouble(TempDt1.Rows[k]["Credit"]);
                                if (Debit > Credit)
                                {
                                    MyBase.Execute("Update Main_OutStanding2 set Credit = 0 where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Credit > 0 and credit = " + Credit);
                                    MyBase.Execute("Update Main_OutStanding2 set Debit = Debit - " + Credit + " where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Debit > 0 and Debit = " + Debit);
                                    Debit = Debit - Credit;
                                }
                                else if (Debit < Credit)
                                {
                                    MyBase.Execute("Update Main_OutStanding2 set Credit = Credit - " + Debit + " where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Credit > 0 and credit = " + Credit);
                                    MyBase.Execute("Update Main_OutStanding2 set Debit = 0 where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Debit > 0  and Debit = " + Debit);
                                    Debit = 0;
                                }
                                else if (Debit == Credit)
                                {
                                    MyBase.Execute("Update Main_OutStanding2 set Credit = 0 where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Credit > 0 and credit = " + Credit);
                                    //MyBase.Execute("Update Main_OutStanding2 set Debit = Debit - " + Credit + " where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Debit > 0  and Debit = " + Debit);
                                    MyBase.Execute("Update Main_OutStanding2 set Debit = 0 where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Debit > 0  and Debit = " + Debit);
                                    Debit = Debit - Credit;
                                }
                            }
                        }
                    }
                    MyBase.Execute("Update Main_outstanding2 set Debit = (-1) * Debit");
                    MyBase.Execute("Delete from Main_outstanding2 where debit = 0 and credit = 0");

                    MyBase.Execute_Tbl("Select vcode, vdate, Ledger_Code, Mode, refDoc, refdate, Duedays Due, '01-Jan-1899' DueDate, (case when isnuLL(Debit, 0) <> 0 then isnull(debit, 0) else isnull(credit, 0) end) Amount, (case when isnuLL(Debit, 0) <> 0 then 'Dr' else 'Cr' end) Type, 0 Pending, (case when isnuLL(Debit, 0) <> 0 then isnull(debit, 0) else isnull(credit, 0) end) Balance, company_Code, year_Code from Main_outstanding2", "Main_outstanding");

                }
                else
                {
                    MyBase.Execute_Qry("select v1.vcode, v1.vdate, v2.user_date, v1.ledger_Code, v1.debit, v1.credit, v1.company_Code, v1.year_Code from voucher_Details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_code = v2.company_code and v1.year_Code = v2.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' and v1.ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_group_code in (Select groupcode from groupmas where groupreserved = 4700 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'))", "Brk1");
                    MyBase.Execute_Qry(" select v1.*, v2.refdoc, v2.refdate from Brk1 v1 left join voucher_breakup_bills v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.ledger_code = v2.ledger_Code and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "'", "Brk2");

                    Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, (case when v1.RefDate is null then v2.user_Date else v1.refdate end) RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code, isnull(l1.duedays, 0) Duedays from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_breakup l1 on v1.ledger_Code = l1.ledger_code and v1.refDoc = l1.refdoc and v1.refdate = l1.refdate and v1.mode = l1.mode and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code where v2.user_date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "'";
                    MyBase.Execute_Qry(Str, "Ref_Vch_Db");

                    Str = "Select * from ref_Vch_Db where refdate <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' union ";
                    Str += " select vcode, vdate, user_date, ledger_code, 'N', 'W.Br.New', user_date, debit, credit, company_Code, year_Code, 0 from Brk2 v2 where refdoc is null and refdate is null and user_date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' union ";
                    Str += " Select null vcode, null vdate, '31-Mar-2010', Ledger_Code, Mode, refDoc, (case when RefDate is null then cast('" + String.Format("{0:dd-MMM-yyyy}", SDate) + "' as Datetime) else refdate end) RefDate, Debit, credit, Company_Code, Year_Code, 0 from ledger_breakup where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ref = 'L1' and term = 'LEDGER' union ";
                    Str += "Select null vcode, null vdate, '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Code, 'N', 'Opn', '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Odebit, ledger_OCredit, Company_Code, Year_Code, 0 from ledger_master where ((Ledger_Odebit > 0) or (ledger_Ocredit >0)) and Breakup = 'Y' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code not in (Select ledger_Code from ledger_breakup where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ref = 'L1' and term = 'LEDGER')";
                    MyBase.Execute_Qry(Str, "F_New_out1");

                    MyBase.Execute_Qry("select vcode, vdate, user_date, ledger_Code, Mode, refDoc, refdate, Debit, Credit, company_Code, year_Code, duedays, credit Balance from F_new_out1 where ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Group_Code in (Select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4700))", "F_New_Out");

                    Str = "Select * from F_new_Out where Debit <> 0";
                    MyBase.Execute_Qry(Str, "F_Ref_out1");

                    Str = " select * from F_new_out where Credit <> 0 ";
                    MyBase.Execute_Qry(Str, "F_Outstanding_Credit1");

                    MyBase.Execute_Tbl("Select * from F_Outstanding_Credit1 order by Ledger_Code, RefDate, RefDoc", "F_Outstanding_Credit");

                    MyBase.Load_Data("Select Ledger_Code, Sum(Debit) Credit from F_Ref_Out1 where Debit <> 0 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "' group by Ledger_Code", ref Dt);
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        Credit = Convert.ToDouble(Dt.Rows[i]["Credit"]);
                        MyBase.Load_Data("Select * from F_Outstanding_Credit where Ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' order by refDate, refDoc ", ref Dt1);
                        for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                        {
                            if (Credit <= 0)
                            {
                                break;
                            }
                            else
                            {
                                Debit = Convert.ToDouble(Dt1.Rows[j]["Balance"]);
                                if (Credit > Debit)
                                {
                                    if (Dt1.Rows[j]["Vcode"] != DBNull.Value)
                                    {
                                        MyBase.Execute("Update F_Outstanding_Credit set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and Vcode = " + Dt1.Rows[j]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                    }
                                    else
                                    {
                                        MyBase.Execute("Update F_Outstanding_Credit set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                    }
                                    Credit = Credit - Debit;
                                }
                                else if (Credit < Debit)
                                {
                                    if (Dt1.Rows[j]["Vcode"] != DBNull.Value)
                                    {
                                        MyBase.Execute("Update F_Outstanding_Credit Set Balance = Balance - " + Credit + " where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and Vcode = " + Dt1.Rows[j]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                    }
                                    else
                                    {
                                        MyBase.Execute("Update F_Outstanding_Credit Set Balance = Balance - " + Credit + " where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                    }
                                    Credit = 0;
                                }
                                else if (Credit == Debit)
                                {
                                    if (Dt1.Rows[j]["Vcode"] != DBNull.Value)
                                    {
                                        MyBase.Execute("Update F_Outstanding_Credit Set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and Vcode = " + Dt1.Rows[j]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                    }
                                    else
                                    {
                                        MyBase.Execute("Update F_Outstanding_Credit Set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                    }
                                    Credit = 0;
                                }
                            }
                        }
                        if (Credit > 0)
                        {
                            MyBase.Execute("INsert into F_Outstanding_Credit values (0, '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Dt.Rows[i]["Ledger_Code"].ToString() + ", 'N', 'New', '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Convert.ToDouble((-1) * Credit) + ", 0, " + CompCode + ", '" + YearCode + "', 0, " + Convert.ToDouble((-1) * Credit) + ")");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void OutStanding_Debtors(DateTime AsONdate, Boolean FIFO)
        {
            String Str = String.Empty;
            DataTable Dt = new DataTable();
            DataTable Dt1 = new DataTable();
            Double Amount = 0;
            Double Amount1 = 0;
            Double Debit = 0;
            Double Credit = 0;
            try
            {


                //Ref_VCH_DB yearcode and company_Code condition Applied on 281010

                if (FIFO == false)
                {
                    MyBase.Execute_Qry("select v1.vcode, v1.vdate, v2.user_date, v1.ledger_Code, v1.debit, v1.credit, v1.company_Code, v1.year_Code from voucher_Details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_code = v2.company_code and v1.year_Code = v2.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' and v1.ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_group_code in (Select groupcode from groupmas where groupreserved = 4800 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'))", "Brk1");
                    MyBase.Execute_Qry(" select v1.*, v2.refdoc, v2.refdate from Brk1 v1 left join voucher_breakup_bills v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.ledger_code = v2.ledger_Code and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "'", "Brk2");

                    // New On 02082010
                    //Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, (case when v1.RefDate is null then cast('" + String.Format("{0:dd-MMM-yyyy}", SDate) + "' as Datetime) else v1.refdate end) RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code, isnull(l1.duedays, 0) Duedays from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_breakup l1 on v1.ledger_Code = l1.ledger_code and v1.refDoc = l1.refdoc and v1.refdate = l1.refdate and v1.mode = l1.mode and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code where v1.mode = 'N' and v2.user_Date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' union ";
                    Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, (case when v1.RefDate is null then v2.user_Date else v1.refdate end) RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code, isnull(l1.duedays, 0) Duedays from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_breakup l1 on v1.ledger_Code = l1.ledger_code and v1.refDoc = l1.refdoc and v1.refdate = l1.refdate and v1.mode = l1.mode and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code where v1.mode = 'N'  and v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "'";
                    MyBase.Execute_Qry(Str, "Ref_Vch_Db");

                    Str = "Select * from ref_Vch_Db where refdate <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' union ";
                    Str += " select vcode, vdate, user_date, ledger_code, 'N', 'W.Br.New', user_date, debit, credit, company_Code, year_Code, 0 from Brk2 v2 where refdoc is null and refdate is null and user_date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' union ";
                    Str += " Select null vcode, null vdate, '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Code, Mode, refDoc, (case when RefDate is null then cast('" + String.Format("{0:dd-MMM-yyyy}", SDate) + "' as Datetime) else refdate end) RefDate, Debit, credit, Company_Code, Year_Code, 0 from ledger_breakup where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ref = 'L1' and term = 'LEDGER' union ";
                    Str += "Select null vcode, null vdate, '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Code, 'N', 'Opn', '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Odebit, ledger_OCredit, Company_Code, Year_Code, 0 from ledger_master where ((Ledger_Odebit > 0) or (ledger_Ocredit >0)) and Breakup = 'Y' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code not in (Select ledger_Code from ledger_breakup where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ref = 'L1' and term = 'LEDGER')";
                    MyBase.Execute_Qry(Str, "New_out1");

                    MyBase.Execute_Qry("select vcode, vdate, user_date, ledger_Code, Mode, refDoc, refdate, (-1) * debit Debit, Credit, company_Code, year_Code, duedays from new_out1 where ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Group_Code in (Select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4800))", "New_Out");

                    Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, v1.RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code, 0 as DueDays from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.mode = 'A' and v2.user_Date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' and v2.company_Code = " + CompCode + " and v2.year_Code = '" + YearCode + "' ";
                    MyBase.Execute_Qry(Str, "Ref_out1");

                    DataTable TempDt1 = new DataTable();

                    MyBase.Execute_Qry("select * from New_out1 union select * from Ref_out1 ", "Main_outstanding1_DB");
                    MyBase.Execute_Qry("Select * from Main_outstanding1_DB where ledger_Code in (select ledger_Code from Ledger_master where ledger_group_Code in (select groupCode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4800) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') ", "Main_outstanding21_DB");
                    MyBase.Execute_Tbl("Select * from Main_outstanding21_DB", "Main_outstanding2_DB");

                    /* basic Condition On 01-Mar-2012

                    MyBase.Load_Data("Select distinct Ledger_Code from Main_outstanding2_DB", ref Dt);
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        MyBase.Load_Data("select Ledger_Code, RefDoc, RefDate, isnull(Debit, 0) Debit, isnull(Credit, 0) Credit from Main_outstanding2_DB where ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and Credit <> 0", ref Dt1);
                        for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                        {
                            Debit = Convert.ToDouble(Dt1.Rows[j]["Debit"]);
                            MyBase.Load_Data("Select * from Main_outstanding2_DB where ledger_Code = " + Dt.Rows[i]["ledger_Code"].ToString() + " and refDoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["RefDate"])) + "' and Debit <> 0", ref TempDt1);
                            for (int k = 0; k <= TempDt1.Rows.Count - 1; k++)
                            {
                                Credit = Convert.ToDouble(TempDt1.Rows[k]["Credit"]);
                                if (Debit > Credit)
                                {
                                    MyBase.Execute("Update Main_OutStanding2_DB set Credit = 0 where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Debit > 0 and Debit = " + Credit);
                                    MyBase.Execute("Update Main_OutStanding2_DB set Debit = Debit - " + Credit + " where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Credit > 0 and Credit = " + Debit);
                                    Debit = Debit - Credit;
                                }
                                else if (Debit < Credit)
                                {
                                    MyBase.Execute("Update Main_OutStanding2_DB set Credit = Credit - " + Debit + " where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and debit > 0 and Debit = " + Credit);
                                    MyBase.Execute("Update Main_OutStanding2_DB set Debit = 0 where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Credit > 0  and Credit = " + Debit);
                                    Debit = 0;
                                }
                                else if (Debit == Credit)
                                {
                                    MyBase.Execute("Update Main_OutStanding2_DB set Credit = 0 where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Debit > 0 and Debit = " + Credit);
                                    MyBase.Execute("Update Main_OutStanding2_Db set Debit = 0 where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Credit > 0  and Credit = " + Debit);
                                    Debit = Debit - Credit;
                                }
                            }
                        }
                    } */


                    MyBase.Load_Data("Select distinct Ledger_Code from Main_outstanding2_DB ", ref Dt);
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        MyBase.Load_Data("select Ledger_Code, RefDoc, RefDate, isnull(Debit, 0) Debit, isnull(Credit, 0) Credit from Main_outstanding2_DB where ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and Debit <> 0", ref Dt1);
                        for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                        {
                            Debit = Convert.ToDouble(Dt1.Rows[j]["Debit"]);
                            MyBase.Load_Data("Select * from Main_outstanding2_DB where ledger_Code = " + Dt.Rows[i]["ledger_Code"].ToString() + " and refDoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["RefDate"])) + "' and Credit <> 0", ref TempDt1);
                            for (int k = 0; k <= TempDt1.Rows.Count - 1; k++)
                            {
                                Credit = Convert.ToDouble(TempDt1.Rows[k]["Credit"]);
                                if (Debit > Credit)
                                {
                                    if (TempDt1.Rows[k]["vcode"].ToString() != String.Empty)
                                    {
                                        MyBase.Execute("Update Main_OutStanding2_DB set Credit = 0 where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and vcode = " + TempDt1.Rows[k]["vcode"].ToString() + " and Credit > 0 and Credit = " + Credit);
                                    }
                                    else
                                    {
                                        MyBase.Execute("Update Main_OutStanding2_DB set Credit = 0 where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Credit > 0 and Credit = " + Credit);
                                    }
                                    MyBase.Execute("Update Main_OutStanding2_DB set Debit = Debit - " + Credit + " where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Debit > 0 and Debit = " + Debit);
                                    Debit = Debit - Credit;
                                }
                                else if (Debit < Credit)
                                {
                                    if (TempDt1.Rows[k]["vcode"].ToString() != String.Empty)
                                    {
                                        MyBase.Execute("Update Main_OutStanding2_DB set Credit = Credit - " + Debit + " where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and vcode = " + TempDt1.Rows[k]["vcode"].ToString() + " and Credit > 0 and Credit = " + Credit);
                                    }
                                    else
                                    {
                                        MyBase.Execute("Update Main_OutStanding2_DB set Credit = Credit - " + Debit + " where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Credit > 0 and Credit = " + Credit);
                                    }
                                    MyBase.Execute("Update Main_OutStanding2_DB set Debit = 0 where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Debit > 0  and Debit = " + Debit);
                                    Debit = 0;
                                }
                                else if (Debit == Credit)
                                {
                                    if (TempDt1.Rows[k]["vcode"].ToString() != String.Empty)
                                    {
                                        MyBase.Execute("Update Main_OutStanding2_DB set Credit = 0 where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and vcode = " + TempDt1.Rows[k]["vcode"].ToString() + " and Debit > 0 and Debit = " + Credit);
                                    }
                                    else
                                    {
                                        MyBase.Execute("Update Main_OutStanding2_DB set Credit = 0 where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Debit > 0 and Debit = " + Credit);
                                    }
                                    MyBase.Execute("Update Main_OutStanding2_Db set Debit = 0 where ledger_Code = " + TempDt1.Rows[k]["ledger_Code"].ToString() + " and refdoc = '" + TempDt1.Rows[k]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TempDt1.Rows[k]["RefDate"].ToString())) + "' and Credit > 0  and Credit = " + Debit);
                                    Debit = Debit - Credit;
                                }
                            }
                        }
                    }

                    MyBase.Execute("Update Main_outstanding2_Db set Credit = (-1) * Credit");
                    MyBase.Execute("Delete from Main_outstanding2_DB where debit = 0 and credit = 0");

                    // If Any Problem with outstanding regular, remove this line.
                    //MyBase.Execute("Delete M1 from Main_outstanding2_DB M1 Inner join Main_outstanding2_DB M2 on M1.ledger_Code = M2.ledger_Code and M1.RefDoc = M2.RefDoc and M1.RefDate = M2.RefDate and m1.company_Code = m2.company_Code and m1.year_Code = m2.year_Code and ((M1.debit = (M2.Credit * -1)) or ((M1.Credit * -1) = M2.debit))");

                    DataTable tDT2 = new DataTable();
                    MyBase.Load_Data("select M1.vcode, M1.LEDGER_CODE, m2.vcode Vcode1, M1.vdate, M2.Vdate Vdate1, M1.Mode, M2.Mode, M1.RefDoc, M2.RefDoc, M1.RefDate, M2.RefDate, M1.debit, M2.debit, M1.Credit, M2.Credit from Main_outstanding2_DB M1 INNER JOIN Main_outstanding2_DB M2 ON M1.Mode = 'N' AND M2.Mode = 'A' AND M1.RefDate = M2.RefDate AND M1.RefDoc = M2.RefDoc AND M1.debit = (M2.Credit * -1) and M1.company_Code = M2.company_Code AND M1.year_Code = M2.year_Code WHERE M1.company_Code = " + CompCode + " AND M1.year_Code = '" + YearCode + "'", ref tDT2);
                    for (int i = 0; i <= tDT2.Rows.Count - 1; i++)
                    {
                        if (tDT2.Rows[i]["vcode"] == null || tDT2.Rows[i]["vcode"] == DBNull.Value)
                        {
                            MyBase.Execute("Delete from Main_outstanding2_DB where LEDGER_CODE = " + tDT2.Rows[i]["ledger_Code"].ToString() + " and Vcode is Null and vdate is null and refdoc = '" + tDT2.Rows[i]["refdoc"].ToString() + "' and mode = 'N' and company_code = " + CompCode + " and year_Code = '" + YearCode + "'");
                        }
                        else
                        {
                            MyBase.Execute("Delete from Main_outstanding2_DB where LEDGER_CODE = " + tDT2.Rows[i]["ledger_Code"].ToString() + " and Vcode = " + tDT2.Rows[i]["vcode"].ToString() + " and vdate = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(tDT2.Rows[i]["vdate"])) + "' and refdoc = '" + tDT2.Rows[i]["refdoc"].ToString() + "' and mode = 'N' and company_code = " + CompCode + " and year_Code = '" + YearCode + "'");
                        }
                        MyBase.Execute("Delete from Main_outstanding2_DB where LEDGER_CODE = " + tDT2.Rows[i]["ledger_Code"].ToString() + " and Vcode = " + tDT2.Rows[i]["vcode1"].ToString() + " and vdate = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(tDT2.Rows[i]["vdate1"])) + "' and refdoc = '" + tDT2.Rows[i]["refdoc"].ToString() + "' and mode = 'A' and company_code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    }

                    tDT2 = new DataTable();
                    MyBase.Load_Data("select ledger_Code, RefDoc, Refdate, Company_Code, year_Code from (Select m1.company_Code, M1.year_Code, m1.ledger_Code, M1.refdoc, m1.refdate, isnull(m1.debit, 0) Debit, isnull(m2.credit, 0) as Credit from (select ledger_Code, Refdoc, refdate, company_Code, year_Code, SUM(debit) debit from Main_outstanding2_DB where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and Mode = 'N' group by ledger_Code, refdoc, refdate, company_Code, year_Code) M1 inner join (select ledger_Code, Refdoc, refdate, company_Code, year_Code, SUM(Credit) Credit from Main_outstanding2_DB where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and Mode = 'A' group by ledger_Code, refdoc, refdate, company_Code, year_Code) M2 on M1.ledger_Code = M2.ledger_Code and M1.RefDoc = M2.RefDoc and M1.RefDate = M2.RefDate and M1.company_Code = M2.company_Code and M1.year_Code = M2.year_Code) S1 where S1.company_Code = " + CompCode + " and S1.year_Code = '" + YearCode + "' and S1.Debit = (S1.Credit * -1)", ref tDT2);
                    for (int i = 0; i <= tDT2.Rows.Count - 1; i++)
                    {
                        MyBase.Execute("Delete from Main_outstanding2_DB where ledger_Code = " + tDT2.Rows[i]["ledger_Code"].ToString() + " and refdoc = '" + tDT2.Rows[i]["refdoc"].ToString() + "' and company_code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    } 

                    // 2703 RajaRam
                    if (CompName.ToUpper().Contains("RAJARAM"))
                    {
                        DataTable Temp1 = new DataTable();
                        MyBase.Load_Data("select cast(ledger_Code as varchar(10)) + '_' + cast(refdoc as varchar(40)), sum(debit) - (Sum(credit) * -1) from Main_outstanding2_DB group by cast(ledger_Code as varchar(10)) + '_' + cast(refdoc as varchar(40)) having sum(debit) - (Sum(credit) * -1) = 0", ref Temp1);
                        for (int i = 0; i <= Temp1.Rows.Count - 1; i++)
                        {
                            MyBase.Execute("Delete from Main_outstanding2_DB where cast(ledger_Code as varchar(10)) + '_' + cast(refdoc as varchar(40)) = '" + Temp1.Rows[i][0].ToString() + "'");
                        }
                    }
                    MyBase.Execute_Tbl("Select vcode, vdate, Ledger_Code, Mode, refDoc, refdate, Duedays Due, '01-Jan-1899' DueDate, (case when isnuLL(Debit, 0) <> 0 then isnull(debit, 0) else isnull(credit, 0) end) Amount, (case when isnuLL(Debit, 0) <> 0 then 'Dr' else 'Cr' end) Type, 0 Pending, (case when isnuLL(Debit, 0) <> 0 then isnull(debit, 0) else isnull(credit, 0) end) Balance, company_Code, year_Code from Main_outstanding2_DB", "Main_outstanding_Deb");
                }
                else
                {
                    MyBase.Execute_Qry("select v1.vcode, v1.vdate, v2.user_date, v1.ledger_Code, v1.debit, v1.credit, v1.company_Code, v1.year_Code from voucher_Details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_code = v2.company_code and v1.year_Code = v2.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' and v1.ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_group_code in (Select groupcode from groupmas where groupreserved = 4800 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'))", "Brk1");
                    MyBase.Execute_Qry(" select v1.*, v2.refdoc, v2.refdate from Brk1 v1 left join voucher_breakup_bills v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.ledger_code = v2.ledger_Code and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "'", "Brk2");

                    Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, (case when v1.RefDate is null then v2.user_Date else v1.refdate end) RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code, isnull(l1.duedays, 0) Duedays from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_breakup l1 on v1.ledger_Code = l1.ledger_code and v1.refDoc = l1.refdoc and v1.refdate = l1.refdate and v1.mode = l1.mode and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code where v2.user_date <= '" + string.Format("{0:dd-MMM-yyyy}", AsONdate) + "'  and v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "'";
                    MyBase.Execute_Qry(Str, "Ref_Vch_Db");

                    Str = "Select * from ref_Vch_Db where refdate <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' union ";
                    Str += " select vcode, vdate, user_date, ledger_code, 'N', 'W.Br.New', user_date, debit, credit, company_Code, year_Code, 0 from Brk2 v2 where refdoc is null and refdate is null and user_date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' union ";
                    Str += " Select null vcode, null vdate, '31-Mar-2010', Ledger_Code, Mode, refDoc, (case when RefDate is null then cast('" + String.Format("{0:dd-MMM-yyyy}", SDate) + "' as Datetime) else refdate end) RefDate, Debit, credit, Company_Code, Year_Code, 0 from ledger_breakup where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ref = 'L1' and term = 'LEDGER' union ";
                    Str += "Select null vcode, null vdate, '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Code, 'N', 'Opn', '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Odebit, ledger_OCredit, Company_Code, Year_Code, 0 from ledger_master where ((Ledger_Odebit > 0) or (ledger_Ocredit >0)) and Breakup = 'Y' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code not in (Select ledger_Code from ledger_breakup where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ref = 'L1' and term = 'LEDGER')";
                    MyBase.Execute_Qry(Str, "F_New_out1");

                    MyBase.Execute_Qry("select vcode, vdate, user_date, ledger_Code, Mode, refDoc, refdate, Debit, Credit, company_Code, year_Code, duedays, Debit Balance from F_new_out1 where ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Group_Code in (Select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4800))", "F_New_Out");

                    Str = "Select * from F_new_Out where credit > 0";
                    MyBase.Execute_Qry(Str, "F_Ref_out1");

                    Str = " select * from F_new_out where debit > 0 ";
                    MyBase.Execute_Qry(Str, "F_Outstanding_Debit1");
                    MyBase.Execute_Tbl("Select * from F_Outstanding_Debit1 order by Ledger_Code, RefDate, RefDoc", "F_Outstanding_Debit");

                    MyBase.Load_Data("Select Ledger_Code, Sum(Credit) Credit from F_Ref_Out1 where credit > 0 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "' group by Ledger_Code", ref Dt);
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        Credit = Convert.ToDouble(Dt.Rows[i]["Credit"]);
                        MyBase.Load_Data("Select * from F_Outstanding_Debit where Ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' order by refDate, refDoc ", ref Dt1);
                        for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                        {
                            if (Credit <= 0)
                            {
                                break;
                            }
                            else
                            {
                                Debit = Convert.ToDouble(Dt1.Rows[j]["Balance"]);
                                if (Credit > Debit)
                                {
                                    if (Dt1.Rows[j]["Vcode"] != DBNull.Value)
                                    {
                                        MyBase.Execute("Update F_Outstanding_Debit set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and Vcode = " + Dt1.Rows[j]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                    }
                                    else
                                    {
                                        MyBase.Execute("Update F_Outstanding_Debit set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                    }
                                    Credit = Credit - Debit;
                                }
                                else if (Credit < Debit)
                                {
                                    if (Dt1.Rows[j]["Vcode"] != DBNull.Value)
                                    {
                                        MyBase.Execute("Update F_Outstanding_Debit Set Balance = Balance - " + Credit + " where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and Vcode = " + Dt1.Rows[j]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                    }
                                    else
                                    {
                                        MyBase.Execute("Update F_Outstanding_Debit Set Balance = Balance - " + Credit + " where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                    }
                                    Credit = 0;
                                }
                                else if (Credit == Debit)
                                {
                                    if (Dt1.Rows[j]["Vcode"] != DBNull.Value)
                                    {
                                        MyBase.Execute("Update F_Outstanding_Debit Set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and Vcode = " + Dt1.Rows[j]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                    }
                                    else
                                    {
                                        MyBase.Execute("Update F_Outstanding_Debit Set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                    }
                                    Credit = 0;
                                }
                            }
                        }
                        if (Credit > 0)
                        {
                            MyBase.Execute("INsert into F_OutStanding_Debit values (0, '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Dt.Rows[i]["Ledger_Code"].ToString() + ", 'N', 'New', '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Convert.ToDouble((-1) * Credit) + ", 0, " + CompCode + ", '" + YearCode + "', 0, " + Convert.ToDouble((-1) * Credit) + ")");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void OutStanding_Debtors_Agent_FIFO(DateTime AsONdate)
        {
            String Str = String.Empty;
            DataTable Dt = new DataTable();
            DataTable Dt1 = new DataTable();
            Double Amount = 0;
            Double Amount1 = 0;
            Double Debit = 0;
            Double Credit = 0;
            try
            {
                MyBase.Execute_Qry("select v1.vcode, v1.vdate, v2.user_date, v1.ledger_Code, v1.debit, v1.credit, v1.company_Code, v1.year_Code from voucher_Details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_code = v2.company_code and v1.year_Code = v2.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' and v1.ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_group_code in (Select groupcode from groupmas where groupreserved = 4800 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'))", "Brk1");
                MyBase.Execute_Qry(" select v1.*, v2.refdoc, v2.refdate, v2.Agent_Code, l1.Ledger_InPrint Agent from Brk1 v1 left join voucher_breakup_bills v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.ledger_code = v2.ledger_Code and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.agent_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "'", "Brk2");

                // New On 02082010
                //Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, (case when v1.RefDate is null then cast('" + String.Format("{0:dd-MMM-yyyy}", SDate) + "' as Datetime) else v1.refdate end) RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code, isnull(l1.duedays, 0) Duedays from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_breakup l1 on v1.ledger_Code = l1.ledger_code and v1.refDoc = l1.refdoc and v1.refdate = l1.refdate and v1.mode = l1.mode and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code where v1.mode = 'N' and v2.user_Date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' union ";
                Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, (case when v1.RefDate is null then v2.user_Date else v1.refdate end) RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code, isnull(l1.duedays, 0) Duedays, v1.Agent_Code, l2.Ledger_InPrint Agent from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_breakup l1 on v1.ledger_Code = l1.ledger_code and v1.refDoc = l1.refdoc and v1.refdate = l1.refdate and v1.mode = l1.mode and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code left join ledger_master l2 on v1.agent_Code = l2.ledger_Code and v1.company_Code = l2.company_Code and v1.year_Code = l2.year_Code where v1.mode = 'N' ";
                MyBase.Execute_Qry(Str, "Ref_Vch_Db");

                Str = "Select * from ref_Vch_Db where refdate <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' union ";
                Str += " select vcode, vdate, user_date, ledger_code, 'N', 'W.Br.New', user_date, debit, credit, company_Code, year_Code, 0, null, null from Brk2 v2 where refdoc is null and refdate is null and user_date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' union ";
                Str += " Select null vcode, null vdate, '31-Mar-2010', l1.Ledger_Code, Mode, refDoc, (case when RefDate is null then cast('01-Apr-2010' as Datetime) else refdate end) RefDate, Debit, credit, l1.Company_Code, l1.Year_Code, 0, l1.Agent_Code, l2.Ledger_InPrint Agent  from ledger_breakup l1 left join ledger_master l2 on l1.ledger_Code = l2.ledger_Code and l1.company_Code = l2.company_Code and l1.year_Code = l2.year_Code where l1.company_Code = " + CompCode + " and l1.year_Code = '" + YearCode + "' and ref = 'L1' and term = 'LEDGER' union ";
                Str += "Select null vcode, null vdate, '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Code, 'N', 'Opn', '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Odebit, ledger_OCredit, Company_Code, Year_Code, 0, null, null  from ledger_master where ((Ledger_Odebit > 0) or (ledger_Ocredit >0)) and Breakup = 'Y' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code not in (Select ledger_Code from ledger_breakup where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ref = 'L1' and term = 'LEDGER')";
                MyBase.Execute_Qry(Str, "F_New_out1");

                MyBase.Execute_Qry("Select Vcode, Vdate, User_date, Ledger_Code, Mode, refDoc, refdate, Debit, Credit, company_Code, year_Code, duedays, Debit Balance, Agent_Code, Agent from F_new_out1 where ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Group_Code in (Select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4800))", "F_New_Out");

                Str = "Select * from F_new_Out where credit > 0";
                MyBase.Execute_Qry(Str, "F_Ref_out1");

                Str = " select * from F_new_out where debit > 0 ";
                //Str += " select * from F_Ref_out1 where debit > 0 ";
                MyBase.Execute_Qry(Str, "F_Outstanding_Debit1");
                MyBase.Execute_Tbl("Select * from F_Outstanding_Debit1 order by Ledger_Code, RefDate, RefDoc", "F_Outstanding_Debit_Agent");

                MyBase.Load_Data("Select Ledger_Code, Sum(Credit) Credit from F_Ref_Out1 where credit > 0 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "' group by Ledger_Code", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Credit = Convert.ToDouble(Dt.Rows[i]["Credit"]);
                    MyBase.Load_Data("Select * from F_Outstanding_Debit_Agent where Ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' order by refDate, refDoc ", ref Dt1);
                    for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                    {
                        if (Credit <= 0)
                        {
                            break;
                        }
                        else
                        {
                            Debit = Convert.ToDouble(Dt1.Rows[j]["Balance"]);
                            if (Credit > Debit)
                            {
                                if (Dt1.Rows[j]["Vcode"] != DBNull.Value)
                                {
                                    MyBase.Execute("Update F_Outstanding_Debit_Agent set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and Vcode = " + Dt1.Rows[j]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                }
                                else
                                {
                                    MyBase.Execute("Update F_Outstanding_Debit_Agent set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                }
                                Credit = Credit - Debit;
                            }
                            else if (Credit < Debit)
                            {
                                if (Dt1.Rows[j]["Vcode"] != DBNull.Value)
                                {
                                    MyBase.Execute("Update F_Outstanding_Debit_Agent Set Balance = Balance - " + Credit + " where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and Vcode = " + Dt1.Rows[j]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                }
                                else
                                {
                                    MyBase.Execute("Update F_Outstanding_Debit_Agent Set Balance = Balance - " + Credit + " where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                }
                                Credit = 0;
                            }
                            else if (Credit == Debit)
                            {
                                if (Dt1.Rows[j]["Vcode"] != DBNull.Value)
                                {
                                    MyBase.Execute("Update F_Outstanding_Debit_Agent Set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and Vcode = " + Dt1.Rows[j]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                }
                                else
                                {
                                    MyBase.Execute("Update F_Outstanding_Debit_Agent Set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and refdoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[j]["RefDate"]) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                                }
                                Credit = 0;
                            }
                        }
                    }
                    if (Credit > 0)
                    {
                        MyBase.Execute("Insert into F_Outstanding_Debit_Agent values (0, '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Dt.Rows[i]["Ledger_Code"].ToString() + ", 'N', 'New', '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Convert.ToDouble((-1) * Credit) + ", 0, " + CompCode + ", '" + YearCode + "', 0, " + Convert.ToDouble((-1) * Credit) + ", Null, Null)");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void OutStanding_Debtors_Agent(DateTime AsONdate)
        {
            String Str = String.Empty;
            DataTable Dt = new DataTable();
            DataTable Dt1 = new DataTable();
            Double Amount = 0;
            Double Amount1 = 0;
            try
            {
                MyBase.Execute_Qry("select v1.vcode, v1.vdate, v2.user_date, v1.ledger_Code, v1.debit, v1.credit, v1.company_Code, v1.year_Code from voucher_Details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_code = v2.company_code and v1.year_Code = v2.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' and v1.ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_group_code in (Select groupcode from groupmas where groupreserved = 4800 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'))", "Brk1");
                MyBase.Execute_Qry(" select v1.*, v2.refdoc, v2.refdate, v2.Agent_Code, l1.Ledger_InPrint Agent from Brk1 v1 left join voucher_breakup_bills v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.ledger_code = v2.ledger_Code and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.agent_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "'", "Brk2");

                // New On 02082010
                //Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, (case when v1.RefDate is null then cast('" + String.Format("{0:dd-MMM-yyyy}", SDate) + "' as Datetime) else v1.refdate end) RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code, isnull(l1.duedays, 0) Duedays from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_breakup l1 on v1.ledger_Code = l1.ledger_code and v1.refDoc = l1.refdoc and v1.refdate = l1.refdate and v1.mode = l1.mode and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code where v1.mode = 'N' and v2.user_Date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' union ";
                Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, (case when v1.RefDate is null then v2.user_Date else v1.refdate end) RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code, isnull(l1.duedays, 0) Duedays, v1.Agent_Code, l2.Ledger_InPrint Agent from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_breakup l1 on v1.ledger_Code = l1.ledger_code and v1.refDoc = l1.refdoc and v1.refdate = l1.refdate and v1.mode = l1.mode and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code left join ledger_master l2 on v1.agent_Code = l2.ledger_Code and v1.company_Code = l2.company_Code and v1.year_Code = l2.year_Code where v1.mode = 'N' ";
                MyBase.Execute_Qry(Str, "Ref_Vch_Db");

                Str = "Select * from ref_Vch_Db where refdate <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' union ";
                Str += " select vcode, vdate, user_date, ledger_code, 'N', 'W.Br.New', user_date, debit, credit, company_Code, year_Code, 0, null, null from Brk2 v2 where refdoc is null and refdate is null and user_date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' union ";
                Str += " Select null vcode, null vdate, '31-Mar-2010', l1.Ledger_Code, Mode, refDoc, (case when RefDate is null then cast('01-Apr-2010' as Datetime) else refdate end) RefDate, Debit, credit, l1.Company_Code, l1.Year_Code, 0, l1.Agent_Code, l2.Ledger_InPrint Agent  from ledger_breakup l1 left join ledger_master l2 on l1.ledger_Code = l2.ledger_Code and l1.company_Code = l2.company_Code and l1.year_Code = l2.year_Code where l1.company_Code = " + CompCode + " and l1.year_Code = '" + YearCode + "' and ref = 'L1' and term = 'LEDGER' union ";
                Str += "Select null vcode, null vdate, '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Code, 'N', 'Opn', '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Odebit, ledger_OCredit, Company_Code, Year_Code, 0, null, null  from ledger_master where ((Ledger_Odebit > 0) or (ledger_Ocredit >0)) and Breakup = 'Y' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code not in (Select ledger_Code from ledger_breakup where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ref = 'L1' and term = 'LEDGER')";
                MyBase.Execute_Qry(Str, "New_out1");

                MyBase.Execute_Qry("select vcode, vdate, user_date, ledger_Code, Mode, refDoc, refdate, (-1) * debit Debit, Credit, company_Code, year_Code, duedays, Agent_Code, Agent from new_out1 where ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Group_Code in (Select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4800))", "New_Out");

                Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, v1.RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.mode = 'A' and v2.user_Date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' and v2.company_Code = " + CompCode + " and v2.year_Code = '" + YearCode + "' ";
                MyBase.Execute_Qry(Str, "Ref_out1");

                MyBase.Execute_Qry("Select vcode, vdate, User_date, ledger_Code, mode, refdoc, refdate, (case when isnull(Debit, 0) <> 0 then isnull(Debit, 0) else isnull(credit, 0) end) Amount, (case when isnull(Debit, 0) <> 0 then 'Dr' else 'Cr' end) Type, company_Code, year_Code from ref_out1 where ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Group_Code in (Select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4800))", "Ref_out");

                if (MyBase.Check_Table("Main_Outstanding_Deb"))
                {
                    if (MyBase.Check_TableField("Main_Outstanding_Deb", "Vcode") == false)
                    {
                        MyBase.Execute("Drop table Main_Outstanding_Deb");
                    }

                    if (MyBase.Check_TableField("Main_Outstanding_Deb", "Agent_Code") == false)
                    {
                        MyBase.Execute("Drop table Main_Outstanding_Deb");
                    }
                }

                if (MyBase.Check_Table("Main_Outstanding_Deb") == false)
                {
                    MyBase.Execute(" create table Main_Outstanding_Deb (Vcode bigint, vdate datetime, Ledger_Code int, Mode varchar(2), RefDoc varchar(100), refdate datetime, Due int, DueDate datetime, Amount numeric(20,2), Type varchar(2), Pending int, Balance numeric(20,2), Company_Code int, year_Code varchar(10), Agent_Code int, Agent_NAme Varchar(200))");
                }

                MyBase.Execute("Delete from Main_Outstanding_Deb where company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute("Insert into Main_Outstanding_Deb Select vcode, vdate, Ledger_Code, Mode, refDoc, refdate, Duedays, '01-Jan-1899', (case when isnuLL(Debit, 0) <> 0 then isnull(debit, 0) else isnull(credit, 0) end), (case when isnuLL(Debit, 0) <> 0 then 'Dr' else 'Cr' end), 0, (case when isnuLL(Debit, 0) <> 0 then isnull(debit, 0) else isnull(credit, 0) end), company_Code, year_Code, Agent_Code, Agent from new_out");


                MyBase.Execute("Update Main_Outstanding_Deb set amount = (-1) * Amount where amount < 0");
                MyBase.Execute("Update Main_Outstanding_Deb set Balance = (-1) * Balance where Balance < 0");

                MyBase.Load_Data("Select vcode, vdate, Ledger_Code, refdoc, refDate, Sum(Amount) Amount, company_Code, year_Code, type from ref_out group by ledger_Code, refDoc, refdate, company_Code, year_Code, type, vcode, vdate Order by ledger_Code", ref Dt);

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Amount = Convert.ToDouble(Dt.Rows[i]["Amount"]);
                    MyBase.Load_Data("Select * from Main_Outstanding_Deb where ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' order by refdate", ref Dt1);
                    for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                    {
                        if (Convert.ToDouble(Dt1.Rows[j]["Amount"]) > Amount)
                        {
                            MyBase.Execute("Update Main_Outstanding_Deb set Balance = " + Convert.ToString(Convert.ToDouble(Dt1.Rows[j]["Amount"]) - Amount) + " where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and Mode = '" + Dt1.Rows[j]["Mode"].ToString() + "' and refDoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["RefDate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                            Amount = 0;
                            break;
                        }
                        else if (Convert.ToDouble(Dt1.Rows[j]["Amount"]) < Amount)
                        {
                            MyBase.Execute("Update Main_Outstanding_Deb set Balance = " + Convert.ToString(Amount - Convert.ToDouble(Dt1.Rows[j]["Amount"])) + " where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and Mode = '" + Dt1.Rows[j]["Mode"].ToString() + "' and refDoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["RefDate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                            Amount = Amount - Convert.ToDouble(Dt1.Rows[j]["Amount"]);
                        }
                        else if (Convert.ToDouble(Dt1.Rows[j]["Amount"]) == Amount)
                        {
                            MyBase.Execute("Update Main_Outstanding_Deb set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and Mode = '" + Dt1.Rows[j]["Mode"].ToString() + "' and refDoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["RefDate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                            Amount = 0;
                            break;
                        }
                    }
                    if (Amount > 0)
                    {
                        MyBase.Execute("Insert into Main_Outstanding_Deb Select " + Dt.Rows[i]["Vcode"].ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["Vdate"])) + "', " + Dt.Rows[i]["Ledger_Code"].ToString() + ", 'A', '" + Dt.Rows[i]["RefDoc"].ToString() + "', '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"])) + "', 0, '01-Jan-1899', " + Amount + ", '" + Dt.Rows[i]["Type"].ToString() + "', 0, " + Amount + ", " + CompCode + ", '" + YearCode + "', Null, Null");
                    }
                }
                MyBase.Execute("Update Main_Outstanding_Deb set amount = (-1) * Amount where type = 'Cr'");
                MyBase.Execute("Update Main_Outstanding_Deb set Balance = (-1) * Balance where type = 'Cr'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void OutStanding_Debtors_old(DateTime AsONdate)
        {
            String Str = String.Empty;
            DataTable Dt = new DataTable();
            DataTable Dt1 = new DataTable();
            Double Amount = 0;
            Double Amount1 = 0;
            try
            {
                Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, (case when v1.RefDate is null then cast('" + String.Format("{0:dd-MMM-yyyy}", SDate) + "' as Datetime) else v1.refdate end) RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code, isnull(l1.duedays, 0) Duedays from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_breakup l1 on v1.ledger_Code = l1.ledger_code and v1.refDoc = l1.refdoc and v1.refdate = l1.refdate and v1.mode = l1.mode and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code where v1.mode = 'N' and v2.user_Date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' union ";
                Str += " Select null, null, '31-Mar-2010', Ledger_Code, Mode, refDoc, (case when RefDate is null then cast('" + String.Format("{0:dd-MMM-yyyy}", SDate) + "' as Datetime) else refdate end) RefDate, Debit, credit, Company_Code, Year_Code, 0 from ledger_breakup where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ref = 'L1' and term = 'LEDGER' union ";
                Str += "Select null, null, '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Code, 'N', 'Opn', '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', Ledger_Odebit, ledger_OCredit, Company_Code, Year_Code, 0 from ledger_master where ((Ledger_Odebit > 0) or (ledger_Ocredit >0)) and Breakup = 'Y' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code not in (Select ledger_Code from ledger_breakup where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ref = 'L1' and term = 'LEDGER')";
                MyBase.Execute_Qry(Str, "New_out1");

                MyBase.Execute_Qry("select vcode, vdate, user_date, ledger_Code, Mode, RefDoc, Refdate, Debit, (-1) * Credit Credit, company_Code, year_Code, duedays from new_out1 where ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Group_Code in (Select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4800))", "New_Out");

                Str = "select v1.vcode, v1.vdate, v2.user_Date, v1.ledger_Code, v1.Mode, v1.RefDoc, v1.RefDate, v1.debit, v1.Credit, v1.company_Code, v1.year_Code from voucher_breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.mode = 'A' and v2.user_Date <= '" + String.Format("{0:dd-MMM-yyyy}", AsONdate) + "' and v2.company_Code = " + CompCode + " and v2.year_Code = '" + YearCode + "' ";
                MyBase.Execute_Qry(Str, "Ref_out1");

                MyBase.Execute_Qry("Select vcode, vdate, User_date, ledger_Code, mode, refdoc, refdate, (case when isnull(Debit, 0) <> 0 then isnull(Debit, 0) else isnull(credit, 0) end) Amount, (case when isnull(Debit, 0) <> 0 then 'Dr' else 'Cr' end) Type, company_Code, year_Code from ref_out1 where ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Group_Code in (Select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4800))", "Ref_out");

                if (MyBase.Check_TableField("Main_outStanding_Deb", "vcode") == false)
                {
                    MyBase.Execute("Drop table Main_outStanding_Deb");
                }

                if (MyBase.Check_Table("Main_outStanding_Deb") == false)
                {
                    MyBase.Execute(" create table Main_outStanding_Deb (Vcode bigint, vdate datetime, Ledger_Code int, Mode varchar(2), RefDoc varchar(100), refdate datetime, Due int, DueDate datetime, Amount numeric(20,2), Type varchar(2), Pending int, Balance numeric(20,2), Company_Code int, year_Code varchar(10))");
                }

                MyBase.Execute("Delete from Main_outStanding_Deb where company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute("Insert into Main_outStanding_Deb Select vcode, vdate, Ledger_Code, Mode, refDoc, refdate, Duedays, '01-Jan-1899', (case when isnuLL(Debit, 0) <> 0 then isnull(debit, 0) else isnull(credit, 0) end), (case when isnuLL(Debit, 0) <> 0 then 'Dr' else 'Cr' end), 0, (case when isnuLL(Debit, 0) <> 0 then isnull(debit, 0) else isnull(credit, 0) end), company_Code, year_Code from new_out");

                MyBase.Load_Data("Select vcode, vdate, Ledger_Code, refdoc, refDate, Sum(Amount) Amount, company_Code, year_Code, type from ref_out group by ledger_Code, refDoc, refdate, company_Code, year_Code, type, vcode, vdate Order by ledger_Code", ref Dt);

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Amount = Convert.ToDouble(Dt.Rows[i]["Amount"]);
                    MyBase.Load_Data("Select * from Main_outStanding_Deb where ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString() + "' and refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' order by refdate", ref Dt1);
                    for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                    {
                        if (Convert.ToDouble(Dt1.Rows[j]["Amount"]) > Amount)
                        {
                            MyBase.Execute("Update Main_outStanding_Deb set Balance = " + Convert.ToString(Convert.ToDouble(Dt1.Rows[j]["Amount"]) - Amount) + " where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and Mode = '" + Dt1.Rows[j]["Mode"].ToString() + "' and refDoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["RefDate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                            Amount = 0;
                            break;
                        }
                        else if (Convert.ToDouble(Dt1.Rows[j]["Amount"]) < Amount)
                        {
                            MyBase.Execute("Update Main_outStanding_Deb set Balance = " + Convert.ToString(Amount - Convert.ToDouble(Dt1.Rows[j]["Amount"])) + " where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and Mode = '" + Dt1.Rows[j]["Mode"].ToString() + "' and refDoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["RefDate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                            Amount = Amount - Convert.ToDouble(Dt1.Rows[j]["Amount"]);
                        }
                        else if (Convert.ToDouble(Dt1.Rows[j]["Amount"]) == Amount)
                        {
                            MyBase.Execute("Update Main_outStanding_Deb set Balance = 0 where ledger_Code = " + Dt1.Rows[j]["Ledger_Code"].ToString() + " and Mode = '" + Dt1.Rows[j]["Mode"].ToString() + "' and refDoc = '" + Dt1.Rows[j]["RefDoc"].ToString() + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[j]["RefDate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                            Amount = 0;
                            break;
                        }
                    }
                    if (Amount > 0)
                    {
                        if (Dt.Rows[i]["Type"].ToString() == "Cr")
                        {
                            Amount = (-1) * Amount;
                        }
                        MyBase.Execute("Insert into Main_outStanding_Deb Select " + Dt.Rows[i]["Vcode"].ToString() + ", '" + String.Format ("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["Vdate"])) + "', " + Dt.Rows[i]["Ledger_Code"].ToString() + ", 'A', '" + Dt.Rows[i]["RefDoc"].ToString() + "', '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"])) + "', 0, '01-Jan-1899', " + Amount + ", '" + Dt.Rows[i]["Type"].ToString() + "', 0, " + Amount + ", " + CompCode + ", '" + YearCode + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Create_Ledger_String()
        {
            String Str = String.Empty;
            DataTable Dt = new DataTable();
            try
            {
                /*
                MyBase.Load_Data ("Select * from sysobjects where name like 'Ledger_String%'", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    MyBase.Execute("Drop function Dbo.ledger_String");
                }
                Str = " CREATE function Ledger_String(@Ledger varchar(300)) returns Varchar(300) as  begin        Declare @Str varchar(300)   set @Str = replace(@Ledger, ' ', '')  set @Str = replace(@Str, '!', '') set @Str = replace(@Str, '~', '')  set @Str = replace(@Str, '@', '') set @Str = replace(@Str, '#', '') set @Str = replace(@Str, '$', '') set @Str = replace(@Str, '^', '')  set @Str = replace(@Str, '&', '') set @Str = replace(@Str, '*', '')  set @Str = replace(@Str, '(', '')   set @Str = replace(@Str, ')', '') set @Str = replace(@Str, '-', '')      set @Str = replace(@Str, ',', '')  set @Str = replace(@Str, '~', '')  set @Str = replace(@Str, '`', '') set @Str = replace(@Str, '!', '') set @Str = replace(@Str, '@', '') set @Str = replace(@Str, '#', '')  set @Str = replace(@Str, '$', '') set @Str = replace(@Str, '^', '') set @Str = replace(@Str, '&', '') set @Str = replace(@Str, '*', '') set @Str = replace(@Str, '_', '')      set @Str = replace(@Str, '.', '')  set @Str = replace(@Str, '[', '') set @Str = replace(@Str, ']', '') set @Str = replace(@Str, '{', '') set @Str = replace(@Str, '}', '') set @Str = replace(@Str, '', '') set @Str = replace(@Str, '|', '') set @Str = replace(@Str, '+', '') set @Str = replace(@Str, '=', '') set @Str = replace(@Str, ':', '')      set @Str = replace(@Str, '/', '')      set @Str = replace(@Str, '', '')   set @Str = replace(@Str, '(', '')  set @Str = replace(@Str, ')', '')  set @Str = replace(@Str, '&', '')   return @Str  End ";
                MyBase.Execute(Str);
                
                MyBase.Load_Data("Select * from sysobjects where name like 'PADR%'", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    MyBase.Execute("Drop function Dbo.PADR");
                }
                Str = "Create function PadR(@Str as varchar(200), @Length as int)  returns varchar(500) as Begin	Declare @Result as varchar(500)	if (Len(@Str) < @Length)		Set @Result = @Str+space(@Length - len(@Str))	else		Set @Result = Substring(@Str, 1, @Length)	return @Result end";
                MyBase.Execute(Str);

                MyBase.Load_Data("Select * from sysobjects where name like 'PADL%'", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    MyBase.Execute("Drop function Dbo.PADL");
                }
                Str = " Create function PadL(@Str as varchar(200), @Length as int)  returns varchar(500) as Begin	Declare @Result as varchar(500)	if (Len(@Str) < @Length)		Set @Result = space(@Length - len(@Str)) + @Str	else		Set @Result = Substring(@Str, 1, @Length)	return @Result end";
                MyBase.Execute(Str);


                MyBase.Load_Data("Select * from sysobjects where name like 'Replace_All_Character%'", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    MyBase.Execute("Drop function Dbo.Replace_All_Character");
                }
                Str = " create function Replace_All_Character(@Ledger varchar(300)) 	returns Varchar(300) as  begin        	Declare @Str varchar(300)   	set @Str = replace(@Ledger, 'a', '')  	set @Str = replace(@Str, 'b', '')  	set @Str = replace(@Str, 'c', '')  	set @Str = replace(@Str, 'd', '')  	set @Str = replace(@Str, 'e', '')  	set @Str = replace(@Str, 'f', '')  	set @Str = replace(@Str, 'g', '')  	set @Str = replace(@Str, 'h', '')  	set @Str = replace(@Str, 'i', '')  	set @Str = replace(@Str, 'j', '')  	set @Str = replace(@Str, 'k', '')  	set @Str = replace(@Str, 'l', '')  	set @Str = replace(@Str, 'm', '')  	set @Str = replace(@Str, 'n', '')  	set @Str = replace(@Str, 'o', '')  	set @Str = replace(@Str, 'p', '')  	set @Str = replace(@Str, 'q', '')  	set @Str = replace(@Str, 'r', '')  	set @Str = replace(@Str, 's', '')  	set @Str = replace(@Str, 't', '')  	set @Str = replace(@Str, 'u', '')  	set @Str = replace(@Str, 'v', '')  	set @Str = replace(@Str, 'w', '')  	set @Str = replace(@Str, 'x', '')  	set @Str = replace(@Str, 'y', '')  	set @Str = replace(@Str, 'z', '')  	set @Str = replace(@Str, 'A', '')  	set @Str = replace(@Str, 'B', '')  	set @Str = replace(@Str, 'C', '')  	set @Str = replace(@Str, 'D', '')  	set @Str = replace(@Str, 'E', '')  	set @Str = replace(@Str, 'F', '')  	set @Str = replace(@Str, 'G', '')  	set @Str = replace(@Str, 'H', '')  	set @Str = replace(@Str, 'I', '')  	set @Str = replace(@Str, 'J', '')  	set @Str = replace(@Str, 'K', '')  	set @Str = replace(@Str, 'L', '')  	set @Str = replace(@Str, 'M', '')  	set @Str = replace(@Str, 'N', '')  	set @Str = replace(@Str, 'O', '')  	set @Str = replace(@Str, 'P', '')  	set @Str = replace(@Str, 'Q', '')  	set @Str = replace(@Str, 'R', '')  	set @Str = replace(@Str, 'S', '')  	set @Str = replace(@Str, 'T', '')  	set @Str = replace(@Str, 'U', '')  	set @Str = replace(@Str, 'V', '')  	set @Str = replace(@Str, 'W', '')  	set @Str = replace(@Str, 'X', '')  	set @Str = replace(@Str, 'Y', '')  	set @Str = replace(@Str, 'Z', '')      return @Str  End ";
                MyBase.Execute(Str);
                 */
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Regional_Negative(Boolean Minus)
        {
            String S = String.Empty;
            try
            {
                RegistryKey NgN = Registry.CurrentUser.OpenSubKey("Control Panel\\International\\", true);
                if (Minus)
                {
                    NgN.SetValue("iNegNumber", "1");
                }
                else
                {
                    NgN.SetValue("iNegNumber", "0");
                }
                NgN.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Group_Update()
        {
            try
            {
                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Advance for Rawmaterial' and groupunder = 6200 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 6200, groupreserved = 6200 where groupname = 'Advance for Rawmaterial' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Agents' and groupunder = 5400 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 5400, groupreserved = 4700 where groupname = 'Agents' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Bank Accounts' and groupunder = 1600 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 1600, groupreserved = 1600 where groupname = 'Bank Accounts' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Cotton Purchase' and groupunder = 4100 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 4100, groupreserved = 4100 where groupname = 'Cotton Purchase' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Deposits (Asset)' and groupunder = 1200 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 1200, groupreserved = 1200 where groupname = 'Deposits (Asset)' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Directors Advance' and groupunder = 1500 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 1500, groupreserved = 1500 where groupname = 'Directors Advance' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Electrical Repair Expenses' and groupunder = 6400 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 6400, groupreserved = 6400 where groupname = 'Electrical Repair Expenses' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Expenses' and groupunder = 4900 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 4900, groupreserved = 4900 where groupname = 'Expenses' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Genset Repair & Maintenance' and groupunder = 4900 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 4900, groupreserved = 4900 where groupname = 'Genset Repair & Maintenance' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'God`s Account' and groupunder = 3600 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 3600, groupreserved = 3600 where groupname = 'God`s Account' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Liabilities & Procision' and groupunder = 3600 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 3600, groupreserved = 3600 where groupname = 'Liabilities & Procision' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Machinery Repair - Spares' and groupunder = 4900 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 4900, groupreserved = 4900 where groupname = 'Machinery Repair - Spares' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Other Creditars' and groupunder = 4700 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 4700, groupreserved = 4700 where groupname = 'Other Creditars' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Other Debtors' and groupunder = 4800 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 4800, groupreserved = 4800 where groupname = 'Other Debtors' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Packing Material Purchase' and groupunder = 4100 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 4100, groupreserved = 4100 where groupname = 'Packing Material Purchase' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Sister Concern' and groupunder = 4700 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 4700, groupreserved = 4700 where groupname = 'Sister Concern' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Spare Parties' and groupunder = 4700 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 4700, groupreserved = 4700 where groupname = 'Spare Parties' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Spares & Stores' and groupunder = 4100 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 4100, groupreserved = 4100 where groupname = 'Spares & Stores' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Trading/Mfg Expenses' and groupunder = 6400 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 6400, groupreserved = 6400 where groupname = 'Trading/Mfg Expenses' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Yarn Party`s' and groupunder = 4800 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 4800, groupreserved = 4800 where groupname = 'Yarn Party`s' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                if (MyBase.Get_RecordCount("GroupMas", "groupname = 'Yarn Purchase' and groupunder = 4100 and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Update GroupMas set groupUnder = 4100, groupreserved = 4100 where groupname = 'Yarn Purchase' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }

                Group_Over(7120, 1500);
                Group_Over(7100, 1700);
                Group_Over(7094, 7001);
                Group_Over(7099, 6400);
                Group_Over(7105, 4300);
                Group_Over(7097, 4900);
                Group_Over(7098, 3700);
                Group_Over(7101, 6200);
                Group_Over(7107, 7052);
                Group_Over(7103, 4400);


                Group_Rename(1500, "Capital Account");
                Group_Rename(1700, "Cash-in-hand");
                Group_Rename(7001, "Cotton Parties");
                Group_Rename(6400, "Direct Expenses");
                Group_Rename(4300, "Duties & Taxes");
                Group_Rename(4900, "Indirect Expenses");
                Group_Rename(3700, "Indirect Incomes");
                Group_Rename(6200, "Loans & Advances (Asset)");
                Group_Rename(7052, "Reserves & Surplus");
                Group_Rename(4400, "Sales Accounts");

                MyBase.Execute("update groupmas set breakup = 'Y' where groupcode in (select GroupCode from groupmas where groupreserved in (4800, 4700) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute("update groupmas set breakup = 'N' where groupcode Not in (select GroupCode from groupmas where groupreserved in (4800, 4700) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");

                MyBase.Execute("update ledger_master set breakup = 'Y' where ledger_group_Code in (select GroupCode from groupmas where groupreserved in (4800, 4700) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute("update ledger_Master set breakup = 'N' where ledger_group_Code Not in (select GroupCode from groupmas where groupreserved in (4800, 4700) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'"); 

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Group_Rename(int Group_Code, String Group_name)
        {
            try
            {
                MyBase.Execute("Update GroupMas set groupName = '" + Group_name + "' where groupcode = " + Group_Code + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Group_Over(int Group_Code_Old, int Moveto_Group_Code)
        {
            try
            {
                if (MyBase.Get_RecordCount("GroupMas", "GroupCode = " + Group_Code_Old + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("Delete from GroupMas where GroupCode = " + Group_Code_Old + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    MyBase.Execute("Update ledger_master set ledger_group_code = " + Moveto_Group_Code + " where ledger_group_Code =  " + Group_Code_Old + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Avaneetha_Updation()
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("Select * from ERP_Ledger_Master Order By LedgerCode ", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("UPdate " + ERP_DBName + ".dbo.enqmaster set LedgerCode = " + Dt.Rows[i]["Acc_LEdger_Code"].ToString() + " where LedgerCOde = " + Dt.Rows[i]["LedgerCode"].ToString());
                    MyBase.Execute("UPdate " + ERP_DBName + ".dbo.enqdetail set LedgerCode = " + Dt.Rows[i]["Acc_LEdger_Code"].ToString() + " where LedgerCOde = " + Dt.Rows[i]["LedgerCode"].ToString());
                    MyBase.Execute("UPdate " + ERP_DBName + ".dbo.SUPRATEMASTER set LedgerCode = " + Dt.Rows[i]["Acc_LEdger_Code"].ToString() + " where LedgerCOde = " + Dt.Rows[i]["LedgerCode"].ToString());
                    MyBase.Execute("UPdate " + ERP_DBName + ".dbo.Pordmaster set LedgerCode = " + Dt.Rows[i]["Acc_LEdger_Code"].ToString() + " where LedgerCOde = " + Dt.Rows[i]["LedgerCode"].ToString());
                    MyBase.Execute("UPdate " + ERP_DBName + ".dbo.Grnmaster set LedgerCode = " + Dt.Rows[i]["Acc_LEdger_Code"].ToString() + " where LedgerCOde = " + Dt.Rows[i]["LedgerCode"].ToString());
                    MyBase.Execute("UPdate " + ERP_DBName + ".dbo.PURRETMASTER set LedgerCode = " + Dt.Rows[i]["Acc_LEdger_Code"].ToString() + " where LedgerCOde = " + Dt.Rows[i]["LedgerCode"].ToString());
                    MyBase.Execute("UPdate " + ERP_DBName + ".dbo.RGP_DCMASTER set LedgerCode = " + Dt.Rows[i]["Acc_LEdger_Code"].ToString() + " where LedgerCOde = " + Dt.Rows[i]["LedgerCode"].ToString());
                    MyBase.Execute("UPdate " + ERP_DBName + ".dbo.RGP_GRNMASTER set LedgerCode = " + Dt.Rows[i]["Acc_LEdger_Code"].ToString() + " where LedgerCOde = " + Dt.Rows[i]["LedgerCode"].ToString());
                    MyBase.Execute("UPdate " + ERP_DBName + ".dbo.NRGP_DCMASTER set LedgerCode = " + Dt.Rows[i]["Acc_LEdger_Code"].ToString() + " where LedgerCOde = " + Dt.Rows[i]["LedgerCode"].ToString());
                    MyBase.Execute("UPdate " + ERP_DBName + ".dbo.inhouse_GRNMASTER set LedgerCode = " + Dt.Rows[i]["Acc_LEdger_Code"].ToString() + " where LedgerCOde = " + Dt.Rows[i]["LedgerCode"].ToString());
                    MyBase.Execute("UPdate " + ERP_DBName + ".dbo.loan_DCMASTER set LedgerCode = " + Dt.Rows[i]["Acc_LEdger_Code"].ToString() + " where LedgerCOde = " + Dt.Rows[i]["LedgerCode"].ToString());
                    MyBase.Execute("UPdate " + ERP_DBName + ".dbo.loan_GRNMASTER set LedgerCode = " + Dt.Rows[i]["Acc_LEdger_Code"].ToString() + " where LedgerCOde = " + Dt.Rows[i]["LedgerCode"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Boolean Fill_Acc_Settings()
        {
            int Value = 0;
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("Select * from acc_Settings where company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", ref Dt);
                if (Dt.Rows.Count > 0)
                {

                    // Vcouher Single Entry Mode
                    if (Convert.ToInt16(Dt.Rows[0]["Voucher_Single"]) == 0)
                    {
                        Voucher_Single_Entry_Mode = false;
                    }
                    else
                    {
                        Voucher_Single_Entry_Mode = true;
                    }

                    // ERP Database Name
                    if (Dt.Rows[0]["Erp_Server_Name"].ToString() == String.Empty)
                    {
                        //MessageBox.Show("ERP Server Name Not Listed ...!", "Vaahini");
                        ERP_DBName = Dt.Rows[0]["Erp_DB_Name"].ToString();
                    }
                    else
                    {
                        if (Check_Linked_Server_Available(Dt.Rows[0]["ERP_Server_Name"].ToString()) == false)
                        {
                            MyBase.Execute("Sp_Addlinkedserver " + Dt.Rows[0]["ERP_Server_Name"]);
                        }
                        //ERP_DBName = Dt.Rows[0]["ERP_Server_Name"].ToString() + "." + Dt.Rows[0]["Erp_DB_Name"].ToString();
                        ERP_DBName = Dt.Rows[0]["Erp_DB_Name"].ToString();
                    }

                    if (Dt.Rows[0]["OpBal_lock"].ToString().ToUpper() == "TRUE")
                    {
                        OpBal_lock = true;
                    }
                    else
                    {
                        OpBal_lock = false;
                    }
                    if (Dt.Rows[0]["voucher_DelEdit_lock"].ToString().ToUpper() == "TRUE")
                    {
                        Vouch_Edit_Lock = true;
                    }
                    else
                    {
                        Vouch_Edit_Lock = false;
                    }

                    if (Dt.Rows[0]["Billing_Ledger_From_Accounts"].ToString().ToUpper() == "TRUE")
                    {
                        Billing_Ledger_From_Accounts = true;
                    }
                    else
                    {
                        Billing_Ledger_From_Accounts = false;
                    }
                    if (Dt.Rows[0]["Tally_Server_Address"].ToString() == "#")
                    {
                        Tally_Server = "#";
                    }
                    else
                    {
                        Tally_Server = "http://" + Dt.Rows[0]["Tally_Server_Address"].ToString() + ":9000";
                    }
                    Tally_Company = Dt.Rows[0]["Tally_Company_Name"].ToString();

                    if (Dt.Rows[0]["inventory"].ToString().ToUpper() == "TRUE")
                    {
                        Inventory = true;
                    }
                    else
                    {
                        Inventory = false;
                    }

                    if (Dt.Rows[0]["Previous_Balance"].ToString().ToUpper() == "TRUE")
                    {
                        Previous_Balance_CarryOver = true;
                    }
                    else
                    {
                        Previous_Balance_CarryOver = false;
                    }
                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Settings_Table()
        {
            try
            {
                if (MyBase.Check_Table ("Acc_Settings") == false)
                {
                    MyBase.Execute("Create table Acc_Settings (Company_Code int, Year_Code varchar(10), Voucher_Single int, ERP_DB_Name varchar(100))");
                }

                //if (MyBase.Get_RecordCount("Acc_Settings", "Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                if (MyBase.Get_RecordCount("Acc_Settings", String.Empty) > 0)
                {
                    if (MyBase.Get_RecordCount("Acc_Settings", "Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") == 0)
                    {
                        MyBase.Execute_Qry("Select * from Acc_Settings where Company_Code = (Select Min(Company_Code) from Acc_Settings)", "T11");
                        MyBase.Execute_Tbl("Select * from t11", "t1");
                        MyBase.Execute("Update T1 Set company_Code = " + CompCode + ", year_Code = '" + YearCode + "'");
                        MyBase.Execute("insert into Acc_Settings select * from t1");
                    }
                }
                
                if (MyBase.Get_RecordCount("Acc_Settings", String.Empty) == 0)
                {
                    if (CompName.ToUpper().Contains("GAINUP") || CompName.ToUpper().Contains("ALAMELU"))
                    {
                        MyBase.Execute("Insert into Acc_Settings values (" + CompCode + ", '" + YearCode + "', 0, 'Vaahini_erp_GAINUP')");
                    }
                    else
                    {
                        MyBase.Execute("Insert into Acc_Settings values (" + CompCode + ", '" + YearCode + "', 0, 'Vaahini_erp_Aegan')");
                    }
                }

                if (CompName.ToUpper().Contains("RAJARAM"))
                {
                    MyBase.Execute("update acc_Settings set voucher_single = 1");
                }

                MyBase.Add_NewField("Acc_Settings", "ERP_Server_Name", "varchar(250)");
                MyBase.Execute("Update Acc_Settings set ERP_Server_Name = '' where ERP_Server_Name is null");
                
                MyBase.Add_NewField("Acc_Settings", "OpBal_Lock", "varchar(5)");
                MyBase.Execute("Update Acc_Settings set OpBal_Lock = 'true' where opBal_Lock is null");

                if (MyBase.Check_TableField("Acc_Settings", "Voucher_DelEdit"))
                {
                    if (MyBase.Check_TableField("Acc_Settings", "Voucher_DelEdit_Lock"))
                    {
                        MyBase.Execute("Alter table Acc_Settings Drop Column Voucher_DelEdit");
                    }
                    else
                    {
                        MyBase.Execute("sp_rename 'acc_settings.Voucher_DelEdit', 'Voucher_DelEdit_Lock', 'Column'");
                    }
                }

                MyBase.Add_NewField("Acc_Settings", "Voucher_DelEdit_Lock", "varchar(5)");
                MyBase.Execute("Update Acc_Settings set Voucher_DelEdit_Lock = 'false' where Voucher_DelEdit_Lock is null");

                MyBase.Add_NewField("Acc_Settings", "ERP_Link", "varchar(5)");
                MyBase.Execute("Update Acc_Settings set ERP_Link = 'True' where ERP_Link is null");

                MyBase.Add_NewField("Acc_Settings", "Billing_Ledger_From_Accounts", "varchar(5)");
                MyBase.Execute("Update Acc_Settings set Billing_Ledger_From_Accounts = 'false' where Billing_Ledger_From_Accounts is null");

                MyBase.Add_NewField("Acc_Settings", "Stores_Ledger_From_Accounts", "varchar(5)");
                MyBase.Execute("Update Acc_Settings set Stores_Ledger_From_Accounts = 'false' where Stores_Ledger_From_Accounts is null");

                MyBase.Add_NewField("Acc_Settings", "Cotton_Ledger_From_Accounts", "varchar(5)");
                MyBase.Execute("Update Acc_Settings set Cotton_Ledger_From_Accounts = 'false' where Cotton_Ledger_From_Accounts is null");

                MyBase.Add_NewField("Acc_Settings", "Tally_Server_Address", "varchar(50)");
                MyBase.Execute("Update Acc_Settings set Tally_Server_Address = '#' where Tally_Server_Address is null");

                MyBase.Add_NewField("Acc_Settings", "Tally_Company_Name", "varchar(50)");
                MyBase.Execute("Update Acc_Settings set Tally_Company_Name = '#' where Tally_Company_Name is null");

                MyBase.Add_NewField("Acc_Settings", "Inventory", "varchar(5)");
                MyBase.Execute("Update Acc_Settings set Inventory = 'false' where inventory is null");

                MyBase.Add_NewField("Acc_Settings", "SMS", "varchar(5)");
                MyBase.Execute("Update Acc_Settings set SMS = 'false' where SMS is null");

                MyBase.Add_NewField("Acc_Settings", "User_Level_Fixed_Permission", "varchar(5)");
                MyBase.Execute("Update Acc_Settings set User_Level_Fixed_Permission = 'false' where User_Level_Fixed_Permission is null");

                MyBase.Add_NewField("Acc_Settings", "Previous_Balance", "varchar(5)");
                MyBase.Execute("Update Acc_Settings set Previous_Balance = 'true' where Previous_Balance is null");

                //// TDS Table
                
                if (MyBase.Check_Table("TDS_Table") == false)
                {
                    MyBase.Execute("Create table TDS_Table (voucher_Mode int)");
                    MyBase.Execute("Insert into tds_table Values (1)");
                    MyBase.Execute("Insert into tds_table Values (4)");
                    //MyBase.Execute("Insert into tds_table Values (6)");
                }

                MyBase.Add_NewField("TDS_Table", "Deduct_ON", "varchar(10)");

                //if (MyBase.Get_RecordCount("TDS_Table", "Deduct_On is null") > 0)
                //{
                //    MyBase.Execute("Update tds_table set Deduct_On = 'Journal' where deduct_On is null");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Update_Breakup_For_SI268()
        {
            try
            {
                MyBase.Execute("update voucher_breakup_bills set refDoc = 'SI -264' where Ref = '2678' and company_Code = 1 and year_Code = '209-2010'");
                MyBase.Execute("update voucher_breakup_bills set refDoc = 'SI -265' where Ref = '2560' and company_Code = 1 and year_Code = '209-2010'");
                MyBase.Execute("update voucher_breakup_bills set refDoc = 'SI -266' where Ref = '2563' and company_Code = 1 and year_Code = '209-2010'");
                MyBase.Execute("update voucher_breakup_bills set refDoc = 'SI -267' where Ref = '2564' and company_Code = 1 and year_Code = '209-2010'");

                MyBase.Execute("update ledger_breakup set refDoc = 'SI -264' where Ref = '2678' and company_Code = 1 and year_Code = '209-2010'");
                MyBase.Execute("update ledger_breakup set refDoc = 'SI -265' where Ref = '2560' and company_Code = 1 and year_Code = '209-2010'");
                MyBase.Execute("update ledger_breakup set refDoc = 'SI -266' where Ref = '2563' and company_Code = 1 and year_Code = '209-2010'");
                MyBase.Execute("update ledger_breakup set refDoc = 'SI -267' where Ref = '2564' and company_Code = 1 and year_Code = '209-2010'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Deleted_Groups()
        {
            DataTable TempDt = new DataTable();
            Int32 Min_GroupCode = 0;
            try
            {
                MyBase.Load_Data("Select Min(GroupCode) groupCode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", ref TempDt);
                if (TempDt.Rows.Count > 0)
                {
                    Min_GroupCode = Convert.ToInt32(TempDt.Rows[0]["Groupcode"]);
                }

                MyBase.Load_Data("select Ledger_Code, Ledger_Name from Ledger_Master where ledger_group_code not in (select Groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "') and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", ref TempDt);
                if (TempDt.Rows.Count > 0)
                {
                    MyBase.Execute("Update Ledger_Master Set Ledger_Group_Code = " + Min_GroupCode + " where ledger_Code in (select Ledger_Code from Ledger_Master where Ledger_Group_Code Not in (select Groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "') and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Update_Gainup_Ledger_Code(String TblName, String LedgerCode, int Increment)
        {
            try
            {
                if (MyBase.Check_TableField_OtherDB (ERP_DBName, TblName, LedgerCode))
                {
                    MyBase.Execute("Update " + ERP_DBName + ".dbo." + TblName + " set " + LedgerCode + " = " + LedgerCode + " + " + Increment);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Update_Gainup_Ledger_Code_Socks(String TblName, String LedgerCode, int Increment)
        {
            try
            {
                if (MyBase.Check_TableField_OtherDB("Vaahini_ERP_Gainup_SOCKS", TblName, LedgerCode))
                {
                    MyBase.Execute("Update vaahini_erp_gainup_socks.dbo." + TblName + " set " + LedgerCode + " = " + LedgerCode + " + " + Increment);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Gainup_Group_Code_Update()
        {
            try
            {
                if (MyBase.Check_Table_OtherDb(ERP_DBName, "GroupMas"))
                {
                    MyBase.Execute("Alter table " + ERP_DBName + ".dbo.GroupMas Alter Column GroupCode varchar(10)");
                    MyBase.Execute("Alter table " + ERP_DBName + ".dbo.GroupMas Alter Column GroupUnder varchar(10)");
                    MyBase.Execute("Alter table " + ERP_DBName + ".dbo.GroupMas Alter Column GroupReserved varchar(10)");
                    MyBase.Execute("Alter table " + ERP_DBName + ".dbo.ledgerMas Alter Column GroupCode varchar(10)");
                    MyBase.Execute("Update " + ERP_DBName + ".dbo.GroupMas set groupcode = cast(isnull(groupcode, 0) as bigint) + 10000");
                    MyBase.Execute("Update " + ERP_DBName + ".dbo.GroupMas set GroupUnder = cast(isnull(GroupUnder, 0) as bigint) + 10000");
                    MyBase.Execute("Update " + ERP_DBName + ".dbo.GroupMas set GroupReserved = cast(isnull(GroupReserved, 0) as bigint) + 10000");
                    MyBase.Execute("Update " + ERP_DBName + ".dbo.LedgerMas set groupcode = cast(isnull(groupcode, 0) as bigint) + 10000");
                }

                if (MyBase.Check_Table_OtherDb(ERP_DBName, "GroupMas_Cot"))
                {
                    MyBase.Execute("Alter table " + ERP_DBName + ".dbo.GroupMas_Cot Alter Column GroupCode varchar(10)");
                    MyBase.Execute("Alter table " + ERP_DBName + ".dbo.GroupMas_Cot Alter Column GroupUnder varchar(10)");
                    MyBase.Execute("Alter table " + ERP_DBName + ".dbo.GroupMas_Cot Alter Column GroupReserved varchar(10)");
                    MyBase.Execute("Alter table " + ERP_DBName + ".dbo.ledgerMas_Cot Alter Column GroupCode varchar(10)");

                    MyBase.Execute("Update " + ERP_DBName + ".dbo.GroupMas_Cot set groupcode = cast(isnull(groupcode, 0) as bigint) + 20000");
                    MyBase.Execute("Update " + ERP_DBName + ".dbo.GroupMas_Cot set GroupUnder = cast(isnull(GroupUnder, 0) as bigint) + 20000");
                    MyBase.Execute("Update " + ERP_DBName + ".dbo.GroupMas_Cot set GroupReserved = cast(isnull(GroupReserved, 0) as bigint) + 20000");
                    MyBase.Execute("Update " + ERP_DBName + ".dbo.LedgerMas_Cot set groupcode = cast(isnull(groupcode, 0) as bigint) + 20000");
                }

                if (MyBase.Check_Table_OtherDb(ERP_DBName, "GroupMas_Fab"))
                {
                    MyBase.Execute("Alter table " + ERP_DBName + ".dbo.GroupMas_Fab Alter Column GroupCode varchar(10)");
                    MyBase.Execute("Alter table " + ERP_DBName + ".dbo.GroupMas_Fab Alter Column GroupUnder varchar(10)");
                    MyBase.Execute("Alter table " + ERP_DBName + ".dbo.GroupMas_Fab Alter Column GroupReserved varchar(10)");
                    MyBase.Execute("Alter table " + ERP_DBName + ".dbo.ledgerMas_Fab Alter Column GroupCode varchar(10)");

                    MyBase.Execute("Update " + ERP_DBName + ".dbo.GroupMas_Fab set groupcode = cast(isnull(groupcode, 0) as bigint) + 30000");
                    MyBase.Execute("Update " + ERP_DBName + ".dbo.GroupMas_Fab set GroupUnder = cast(isnull(GroupUnder, 0) as bigint) + 30000");
                    MyBase.Execute("Update " + ERP_DBName + ".dbo.GroupMas_Fab set GroupReserved = cast(isnull(GroupReserved, 0) as bigint) + 30000");
                    MyBase.Execute("Update " + ERP_DBName + ".dbo.LedgerMas_Fab set groupcode = cast(isnull(groupcode, 0) as bigint) + 30000");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Record_Available(int LedgerCode, params String[] TblName)
        {
            DataTable Dt;
            try
            {
                foreach (String Str in TblName)
                {
                    Dt = new DataTable();
                    if (MyBase.Check_TableField_OtherDB (ERP_DBName, Str, "ledgerCode"))
                    {
                        MyBase.Load_Data("Select * from " + ERP_DBName + ".dbo." + Str + " where ledgerCode = " + LedgerCode, ref Dt);
                        if (Dt.Rows.Count > 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Record_Available_socks(int LedgerCode, params String[] TblName)
        {
            DataTable Dt;
            try
            {
                foreach (String Str in TblName)
                {
                    Dt = new DataTable();
                    if (MyBase.Check_TableField_OtherDB("vaahini_erp_gainup_socks", Str, "ledgerCode"))
                    {
                        MyBase.Load_Data("Select * from vaahini_erp_gainup_socks.dbo." + Str + " where ledgerCode = " + LedgerCode, ref Dt);
                        if (Dt.Rows.Count > 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Update_Ledger_InSocks(int LedgerCode, int New_ledgerCode, params String[] TblName)
        {
            DataTable Dt;
            try
            {
                foreach (String Str in TblName)
                {
                    Dt = new DataTable();
                    if (MyBase.Check_TableField_OtherDB("vaahini_erp_gainup_socks", Str, "ledgerCode"))
                    {
                        MyBase.Execute("Update vaahini_erp_gainup_socks.dbo." + Str + " set ledgerCode = " + New_ledgerCode + " where ledgercode = " + LedgerCode);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        void Truncate_2009()
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("Select Name from Sysobjects Where xtype = 'U' order by name", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["Name"].ToString().ToUpper() == "Socks_Companymas" || Dt.Rows[i]["Name"].ToString().ToUpper() == "Socks_User_Master")
                    {

                    }
                    else
                    {
                        if (MyBase.Check_TableField(Dt.Rows[i]["Name"].ToString(), "Year_Code"))
                        {
                            MyBase.Execute("Delete from " + Dt.Rows[i]["Name"].ToString() + " where year_Code = '2009-2010'");
                            MyBase.Execute("Delete from " + Dt.Rows[i]["Name"].ToString() + " where year_Code is null");
                            MyBase.Execute("Delete from " + Dt.Rows[i]["Name"].ToString() + " where year_Code = '2008-2009'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Gainup_Socks_ledger_Update()
        {
            DataTable Dt = new DataTable();
            try
            {
                if (MyBase.Check_Table("Gainup_Group_Update_Socks") == false)
                {
                    if (MyBase.Check_Table_OtherDb("VAAHINI_ERP_GAINUP_SOCKS", "GroupMas"))
                    {
                        MyBase.Execute("Alter table VAAHINI_ERP_GAINUP_SOCKS.dbo.GroupMas Alter Column GroupCode varchar(10)");
                        MyBase.Execute("Alter table VAAHINI_ERP_GAINUP_SOCKS.dbo.GroupMas Alter Column GroupUnder varchar(10)");
                        MyBase.Execute("Alter table VAAHINI_ERP_GAINUP_SOCKS.dbo.GroupMas Alter Column GroupReserved varchar(10)");
                        MyBase.Execute("Alter table VAAHINI_ERP_GAINUP_SOCKS.dbo.ledgerMas Alter Column GroupCode varchar(10)");
                        MyBase.Execute("Update VAAHINI_ERP_GAINUP_SOCKS.dbo.GroupMas set groupcode = cast(isnull(groupcode, 0) as bigint) + 40000");
                        MyBase.Execute("Update VAAHINI_ERP_GAINUP_SOCKS.dbo.GroupMas set GroupUnder = cast(isnull(GroupUnder, 0) as bigint) + 40000");
                        MyBase.Execute("Update VAAHINI_ERP_GAINUP_SOCKS.dbo.GroupMas set GroupReserved = cast(isnull(GroupReserved, 0) as bigint) + 40000");
                        MyBase.Execute("Update VAAHINI_ERP_GAINUP_SOCKS.dbo.LedgerMas set groupcode = cast(isnull(groupcode, 0) as bigint) + 40000");
                    }

                    Update_Gainup_Ledger_Code_Socks("ledgermas", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("ledaddress", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("DEPT", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("ithead", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("itsub", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("machmodel", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("supratemaster", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("supratedetail", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("pordmaster", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("porddetail", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("grnmaster", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("grndetail", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("billpmaster", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("billpdetail", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("purretmaster", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("purretdetail", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("enqmaster", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("enqdetail", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("issmaster", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("issdetail", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("mach", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("masempl", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("pindmaster", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("pinddetail", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("rgp_dcmaster", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("rgp_dcdetail", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("rgp_grndetail", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("rgp_grnmaster", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("nrgp_dcmaster", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("nrgp_dcdetail", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("loan_dcmaster", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("loan_dcdetail", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("loan_grnmaster", "ledgerCode", 5000);
                    Update_Gainup_Ledger_Code_Socks("loan_grndetail", "ledgerCode", 5000);

                    Str = " select groupcode, groupname, groupunder, groupreserved from VAAHINI_ERP_GAINUP_SOCKS.dbo.groupmas where groupcode in (Select groupcode from VAAHINI_ERP_GAINUP_SOCKS.dbo.ledgerMas) ";
                    MyBase.Execute_Qry(Str, "ERP_Group1");
                    MyBase.Execute_Tbl("Select * from ERP_Group1", "ERP_Group");

                    Str = " select l1.LedgerCode, l1.ledgerName, l1.groupcode,  l2.Laddress, l2.Lphone, L2.Lfax, l2.LEMail, L2.LContPer, L2.lContDept, l2.LLstNo, l2.LCSTNo from VAAHINI_ERP_GAINUP_SOCKS.dbo.ledgerMas l1 left join VAAHINI_ERP_GAINUP_SOCKS.dbo.ledAddress L2 on l1.ledgerCode = l2.ledgerCode ";
                    MyBase.Execute_Qry(Str, "ERP_ledger1");
                    MyBase.Execute_Tbl("Select * from ERP_ledger1", "ERP_Ledger");

                    MyBase.Load_Data("select distinct Groupcode, isnull(g1.Acc_Code, 0) Acc_Code from erp_group e1 left join VAAHINI_ERP_GAINUP_SOCKS.dbo.Group_verify G1 on substring(cast(groupcode as varchar(10)), 2, 4) = g1.erp_Code ", ref Dt);
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        MyBase.Execute("Update ErP_ledger set Groupcode = " + Dt.Rows[i]["Acc_Code"].ToString() + " where groupcode = " + Dt.Rows[i]["groupCode"].ToString());
                    }

                    MyBase.Load_Data("select e1.ledgerCode, l1.ledger_Code, l1.ledger_Name, l1.company_code, l1.year_Code from erp_ledger e1, ledger_master l1  where e1.ledgerName = l1.ledger_Name", ref Dt);
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        Meriging_ledger(Convert.ToInt32(Dt.Rows[i]["Ledger_Code"]), Convert.ToInt32(Dt.Rows[i]["Company_Code"]), Dt.Rows[i]["Year_Code"].ToString(), Convert.ToInt32(Dt.Rows[i]["LedgerCode"]), false);
                    }

                    MyBase.Execute_Tbl("select distinct e1.ledgerName from erp_ledger e1, ledger_master l1  where e1.ledgerName = l1.ledger_Name", "Tmp_table");

                    //Gainup
                    MyBase.Execute("insert into ledger_Master (ledger_Code, Ledger_Name, Ledger_Title, Ledger_InPrint, Ledger_Group_Code, Ledger_Odebit, Ledger_OCredit, ledger_Address, ledger_Phone, Ledger_Email, ledger_tin, ledger_Cst, company_Code, year_Code) select ledgerCode, LedgerName, 'M/S.', ledgerName, GroupCode, 0, 0, lAddress, lPhone, LEmail, lLstNo, lCstNo, 1, '2010-2011' from erp_ledger where ledgerName not in (select * from Tmp_table)");
                    MyBase.Execute("insert into ledger_Master (ledger_Code, Ledger_Name, Ledger_Title, Ledger_InPrint, Ledger_Group_Code, Ledger_Odebit, Ledger_OCredit, ledger_Address, ledger_Phone, Ledger_Email, ledger_tin, ledger_Cst, company_Code, year_Code) select ledgerCode, LedgerName, 'M/S.', ledgerName, GroupCode, 0, 0, lAddress, lPhone, LEmail, lLstNo, lCstNo, 1, '2009-2010' from erp_ledger where ledgerName not in (select * from Tmp_table)");
                    //Alamelu
                    MyBase.Execute("insert into ledger_Master (ledger_Code, Ledger_Name, Ledger_Title, Ledger_InPrint, Ledger_Group_Code, Ledger_Odebit, Ledger_OCredit, ledger_Address, ledger_Phone, Ledger_Email, ledger_tin, ledger_Cst, company_Code, year_Code) select ledgerCode, LedgerName, 'M/S.', ledgerName, GroupCode, 0, 0, lAddress, lPhone, LEmail, lLstNo, lCstNo, 2, '2010-2011' from erp_ledger where ledgerName not in (select * from Tmp_table)");
                    MyBase.Execute("insert into ledger_Master (ledger_Code, Ledger_Name, Ledger_Title, Ledger_InPrint, Ledger_Group_Code, Ledger_Odebit, Ledger_OCredit, ledger_Address, ledger_Phone, Ledger_Email, ledger_tin, ledger_Cst, company_Code, year_Code) select ledgerCode, LedgerName, 'M/S.', ledgerName, GroupCode, 0, 0, lAddress, lPhone, LEmail, lLstNo, lCstNo, 2, '2009-2010' from erp_ledger where ledgerName not in (select * from Tmp_table)");

                    MyBase.Execute("Create table Gainup_Group_Update_Socks (no int)");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Gainup_Ledger_Update()
        {
            DataTable Dt = new DataTable();
            try
            {
                // Drop All Triiger
                //drop trigger Trig_Ins_FabRec
                //drop trigger Trig_Ins_DoubRec
                //drop trigger Trig_Ins_DyedRec
                //drop trigger Trig_Clot_Del
                //drop trigger Trig_DyedRec_Del
                //drop trigger LedgerMas_Fab_Ins_Trig
                //drop trigger Trig_DoubRec_Del
                //drop trigger LedgerMas_Cot_Ins_Trig
                //drop trigger Trig_Ins_PRet
                //drop trigger Trig_Pret_Del






                //Update_Ledger_Code_Field("Invoicemas", "despcode", "sales");
                //Update_Ledger_Code_Field("Invoicemas", "agentcode", "sales");

                //Update_Ledger_Code_Field("SalesMaster", "salescode", "sales");
                //Update_Ledger_Code_Field("SalesMaster", "partycode", "sales");
                //Update_Ledger_Code_Field("SalesMaster", "brokercode", "sales");
                //Update_Ledger_Code_Field("SalesMaster", "taxcode", "sales");
                //Update_Ledger_Code_Field("SalesMaster", "othercode", "sales");

                //Update_Ledger_Code_Field("salor2Mas", "LCode", "sales");

                //Update_Ledger_Code_Field("yarntrn", "dpcode", "sales");
                //Update_Ledger_Code_Field("yarntrn", "OnacCode1", "sales");
                //Update_Ledger_Code_Field("yarntrn", "OnacCode2", "sales");

                //Update_Ledger_Code_Field("it_wasmas", "salescode", "sales");
                //Update_Ledger_Code_Field("it_wasmas", "frecode", "sales");
                //Update_Ledger_Code_Field("it_wasmas", "premitcode", "sales");
                //Update_Ledger_Code_Field("it_wasmas", "staxcode", "sales");
                //Update_Ledger_Code_Field("it_wasmas", "sccode", "sales");
                //Update_Ledger_Code_Field("it_wasmas", "other1code", "sales");
                //Update_Ledger_Code_Field("it_wasmas", "other2code", "sales");
                //Update_Ledger_Code_Field("it_wasmas", "delicode", "sales");
                //Update_Ledger_Code_Field("it_wasmas", "tax3code", "sales");
                //Update_Ledger_Code_Field("it_wasmas", "tax4code", "sales");

                //Update_Ledger_Code_Field("Invoicemas_socks", "salescode", "sales");
                //Update_Ledger_Code_Field("Invoicemas_socks", "frecode", "sales");
                //Update_Ledger_Code_Field("Invoicemas_socks", "premitcode", "sales");
                //Update_Ledger_Code_Field("Invoicemas_socks", "staxcode", "sales");
                //Update_Ledger_Code_Field("Invoicemas_socks", "sccode", "sales");
                //Update_Ledger_Code_Field("Invoicemas_socks", "other1code", "sales");
                //Update_Ledger_Code_Field("Invoicemas_socks", "other2code", "sales");
                //Update_Ledger_Code_Field("Invoicemas_socks", "delicode", "sales");
                //Update_Ledger_Code_Field("Invoicemas_socks", "tax3code", "sales");
                //Update_Ledger_Code_Field("Invoicemas_socks", "tax4code", "sales");

                //Update_Ledger_Code_Field("Invoicemas_Socksc", "salescode", "sales");
                //Update_Ledger_Code_Field("Invoicemas_Socksc", "frecode", "sales");
                //Update_Ledger_Code_Field("Invoicemas_Socksc", "premitcode", "sales");
                //Update_Ledger_Code_Field("Invoicemas_Socksc", "staxcode", "sales");
                //Update_Ledger_Code_Field("Invoicemas_Socksc", "sccode", "sales");
                //Update_Ledger_Code_Field("Invoicemas_Socksc", "other1code", "sales");
                //Update_Ledger_Code_Field("Invoicemas_Socksc", "other2code", "sales");
                //Update_Ledger_Code_Field("Invoicemas_Socksc", "delicode", "sales");
                //Update_Ledger_Code_Field("Invoicemas_Socksc", "tax3code", "sales");
                //Update_Ledger_Code_Field("Invoicemas_Socksc", "tax4code", "sales");

                //MyBase.Execute(" update fabreceipt set onaccode = onaccode + 3500 ");
                //MyBase.Execute(" update Doublingreceipt  set onaccode = onaccode + 3500 ");
                //MyBase.Execute(" update Dyeingreceipt  set onaccode = onaccode + 3500 ");

                //Truncate_2009();

                if (MyBase.Get_RecordCount("Socks_User_Master", "User_Name = 'ADMIN' and id = 1") == 0)
                {
                    MyBase.Execute("Insert into Socks_User_Master (Id, User_Code, User_Name, USer_Address, User_Status) values (1, 1, 'ADMIN', '083084065082', 'False')");
                }

                if (MyBase.Check_Table("Gainup_Group_Update"))
                {
                    return;
                }

                MyBase.Execute("Update Ledger_Master set Link_Status = null");

                if (MessageBox.Show("Is Trigger Removed ...!", "Vaahini", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                Gainup_Group_Code_Update();
               
                //Stores Ledger Updation
                Update_Gainup_Ledger_Code("ledgermas", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("ledaddress", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("DEPT", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("ithead", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("itsub", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("machmodel", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("supratemaster", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("supratedetail", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("pordmaster", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("porddetail", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("grnmaster", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("grndetail", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("billpmaster", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("billpdetail", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("purretmaster", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("purretdetail", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("enqmaster", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("enqdetail", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("issmaster", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("issdetail", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("mach", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("masempl", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("pindmaster", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("pinddetail", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("rgp_dcmaster", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("rgp_dcdetail", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("rgp_grndetail", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("rgp_grnmaster", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("nrgp_dcmaster", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("nrgp_dcdetail", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("loan_dcmaster", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("loan_dcdetail", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("loan_grnmaster", "ledgerCode", 2000);
                Update_Gainup_Ledger_Code("loan_grndetail", "ledgerCode", 2000);

                // Billing
                Update_Gainup_Ledger_Code("ledgermas_fab", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("ledaddress_fab", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("invoicemas", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("invoicedtl", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("it_wasmas", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("it_wasdtl", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("fabreceipt", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("dyeingreceipt", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("doublingreceipt", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("form_receipt", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("salesmaster", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("salesdetails", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("SalesOrdernew_Mst", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("SalesOrdernew_Det", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("SalesOrdernew_Det_Sch", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("salor2Mas", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("yarntrn", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("YARNRET", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("ratemaster", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("Invoicemas_socks", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("Invoicedtl_socks", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("Invoicemas_Socksc", "ledgerCode", 3500);
                Update_Gainup_Ledger_Code("Invoicedtl_Socksc", "ledgerCode", 3500);

                MyBase.Execute("update invoicemas set bedcode = bedcode + 3500");
                MyBase.Execute(" update invoicemas set AEDCode = AEDCode + 3500");
                MyBase.Execute(" update invoicemas set TaxCode = TaxCode + 3500");
                MyBase.Execute(" update invoicemas set OTHER1Code = OTHER1Code + 3500");
                MyBase.Execute(" update invoicemas set cesscode = cesscode + 3500");
                MyBase.Execute(" update invoicemas set DELICODE = DELICODE + 3500");
                MyBase.Execute(" update invoicemas set OnAcCode = OnAcCode + 3500");
                MyBase.Execute(" update invoicemas set salescode = salescode + 3500");

                //Cotton
                Update_Gainup_Ledger_Code("ledgermas_Cot", "ledgerCode", 4500);
                Update_Gainup_Ledger_Code("ledaddress_Cot", "ledgerCode", 4500);
                Update_Gainup_Ledger_Code("lotmaster", "ledgerCode", 4500);
                Update_Gainup_Ledger_Code("lotdetail", "ledgerCode", 4500);
                Update_Gainup_Ledger_Code("Pordmaster_cot", "ledgerCode", 4500);
                Update_Gainup_Ledger_Code("Porddetail_cot", "ledgerCode", 4500);
                Update_Gainup_Ledger_Code("retmaster_cot", "ledgerCode", 4500);
                Update_Gainup_Ledger_Code("retdetail_cot", "ledgerCode", 4500);


                Str = " select groupcode, groupname, groupunder, groupreserved from " + ERP_DBName + ".dbo.groupmas where groupcode in (Select groupcode from " + ERP_DBName + ".dbo.ledgerMas) union All ";
                Str += " select groupcode, groupname, groupunder, groupreserved  from " + ERP_DBName + ".dbo.groupmas_Fab where groupcode in (Select groupcode from " + ERP_DBName + ".dbo.ledgerMas_Fab) union All ";
                Str += " select groupcode, groupname, groupunder, groupreserved  from " + ERP_DBName + ".dbo.groupmas_cOT where groupcode in (Select groupcode from " + ERP_DBName + ".dbo.ledgerMas_cOT) ";
                MyBase.Execute_Qry(Str, "ERP_Group1");
                MyBase.Execute_Tbl("Select * from ERP_Group1", "ERP_Group");


                Str = " select l1.LedgerCode, l1.ledgerName, l1.groupcode,  l2.Laddress, l2.Lphone, L2.Lfax, l2.LEMail, L2.LContPer, L2.lContDept, l2.LLstNo, l2.LCSTNo from " + ERP_DBName + ".dbo.ledgerMas l1 left join " + ERP_DBName + ".dbo.ledAddress L2 on l1.ledgerCode = l2.ledgerCode union All  ";
                Str += " select l1.LedgerCode, l1.ledgerName, l1.groupcode,  l2.Laddress, l2.Lphone, L2.Lfax, l2.LEMail, L2.LContPer, L2.lContDept, l2.LLstNo, l2.LCSTNo from " + ERP_DBName + ".dbo.ledgerMas_Fab l1 left join " + ERP_DBName + ".dbo.ledAddress_Fab L2 on l1.ledgerCode = l2.ledgerCode  union All  ";
                Str += "    select l1.LedgerCode, l1.ledgerName, l1.groupcode,  l2.Laddress, l2.Lphone, L2.Lfax, l2.LEMail, L2.LContPer, L2.lContDept, l2.LLstNo, l2.LCSTNo from " + ERP_DBName + ".dbo.ledgerMas_Cot l1 left join " + ERP_DBName + ".dbo.ledAddress_Cot L2 on l1.ledgerCode = l2.ledgerCode ";
                MyBase.Execute_Qry(Str, "ERP_ledger1");
                MyBase.Execute_Tbl("Select * from ERP_ledger1", "ERP_Ledger");



                MyBase.Load_Data("select distinct Groupcode, g1.Acc_Code from erp_group e1 left join " + ERP_DBName + ".dbo.Group_verify G1 on substring(cast(groupcode as varchar(10)), 2, 4) = g1.erp_Code ", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Update ErP_ledger set Groupcode = " + Dt.Rows[i]["Acc_Code"].ToString() + " where groupcode = " + Dt.Rows[i]["groupCode"].ToString());
                }

                MyBase.Load_Data("select e1.ledgerCode, l1.ledger_Code, l1.ledger_Name, l1.company_code, l1.year_Code from erp_ledger e1, ledger_master l1  where e1.ledgerName = l1.ledger_Name", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Meriging_ledger(Convert.ToInt32(Dt.Rows[i]["Ledger_Code"]), Convert.ToInt32(Dt.Rows[i]["Company_Code"]), Dt.Rows[i]["Year_Code"].ToString(), Convert.ToInt32(Dt.Rows[i]["LedgerCode"]), false);
                }

                MyBase.Execute_Tbl("select distinct e1.ledgerName from erp_ledger e1, ledger_master l1  where e1.ledgerName = l1.ledger_Name", "Tmp_table");

                //Gainup
                MyBase.Execute("insert into ledger_Master (ledger_Code, Ledger_Name, Ledger_Title, Ledger_InPrint, Ledger_Group_Code, Ledger_Odebit, Ledger_OCredit, ledger_Address, ledger_Phone, Ledger_Email, ledger_tin, ledger_Cst, company_Code, year_Code) select ledgerCode, LedgerName, 'M/S.', ledgerName, GroupCode, 0, 0, lAddress, lPhone, LEmail, lLstNo, lCstNo, 1, '2010-2011' from erp_ledger where ledgerName not in (select * from Tmp_table)");
                MyBase.Execute("insert into ledger_Master (ledger_Code, Ledger_Name, Ledger_Title, Ledger_InPrint, Ledger_Group_Code, Ledger_Odebit, Ledger_OCredit, ledger_Address, ledger_Phone, Ledger_Email, ledger_tin, ledger_Cst, company_Code, year_Code) select ledgerCode, LedgerName, 'M/S.', ledgerName, GroupCode, 0, 0, lAddress, lPhone, LEmail, lLstNo, lCstNo, 1, '2009-2010' from erp_ledger where ledgerName not in (select * from Tmp_table)");
                //Alamelu
                MyBase.Execute("insert into ledger_Master (ledger_Code, Ledger_Name, Ledger_Title, Ledger_InPrint, Ledger_Group_Code, Ledger_Odebit, Ledger_OCredit, ledger_Address, ledger_Phone, Ledger_Email, ledger_tin, ledger_Cst, company_Code, year_Code) select ledgerCode, LedgerName, 'M/S.', ledgerName, GroupCode, 0, 0, lAddress, lPhone, LEmail, lLstNo, lCstNo, 2, '2010-2011' from erp_ledger where ledgerName not in (select * from Tmp_table)");
                MyBase.Execute("insert into ledger_Master (ledger_Code, Ledger_Name, Ledger_Title, Ledger_InPrint, Ledger_Group_Code, Ledger_Odebit, Ledger_OCredit, ledger_Address, ledger_Phone, Ledger_Email, ledger_tin, ledger_Cst, company_Code, year_Code) select ledgerCode, LedgerName, 'M/S.', ledgerName, GroupCode, 0, 0, lAddress, lPhone, LEmail, lLstNo, lCstNo, 2, '2009-2010' from erp_ledger where ledgerName not in (select * from Tmp_table)");                

                MyBase.Execute("Create table Gainup_Group_Update (no int)");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Check_ledgerCode(Int32 LedgerCode)
        {
            try
            {
                if (Check_ledgerCode_Isused(ERP_DBName, "DEPT", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "ithead", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "itsub", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "machmodel", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "supratemaster", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "supratedetail", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "pordmaster", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "porddetail", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "grnmaster", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "grndetail", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "billpmaster", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "billpdetail", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "purretmaster", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "purretdetail", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "enqdetail", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "issmaster", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "issdetail", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "mach", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "masempl", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "pindmaster", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "pinddetail", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "rgp_dcmaster", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "rgp_dcdetail", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "rgp_grndetail", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "rgp_grnmaster", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "nrgp_dcmaster", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "nrgp_dcdetail", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "loan_dcmaster", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "loan_dcdetail", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "loan_grnmaster", LedgerCode))
                {
                    return true;
                }
                if (Check_ledgerCode_Isused(ERP_DBName, "loan_grndetail", LedgerCode))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Check_ledgerCode_Isused(String DBName, String TblName, Int32 LedgerCode)
        {
            try
            {
                if (MyBase.Check_Table_OtherDb(DBName, TblName))
                {
                    if (MyBase.Get_RecordCount_OtherDB(DBName, TblName, "LedgerCode = " + LedgerCode) > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Meriging_ledger(int Ledger_Code_From, int CompCode, String Year_Code, int Ledger_Code_To, Boolean Delete_Condition)
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("Select * from ERP_ledger where ledgerCode = " + Ledger_Code_To, ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    if (MyBase.Get_RecordCount("Ledger_Master", "Ledger_Code = " + Ledger_Code_From + " and company_Code =  " + CompCode + " and year_Code = '" + Year_Code + "' and Link_Status is null") > 0)
                    {
                        MyBase.Execute("update Ledger_Master set Ledger_Code = " + Dt.Rows[0]["LedgerCode"].ToString() + ", Ledger_group_Code = " + Dt.Rows[0]["Groupcode"].ToString() + ", Ledger_Address = '" + Dt.Rows[0]["LAddress"].ToString() + "', ledger_Phone = '" + Dt.Rows[0]["LPhone"].ToString() + "', Ledger_Fax = '" + Dt.Rows[0]["LFax"].ToString() + "', Ledger_Email = '" + Dt.Rows[0]["LEmail"].ToString() + "', ledger_Tin = '" + Dt.Rows[0]["llstNo"].ToString() + "', Ledger_CST = '" + Dt.Rows[0]["LCSTNo"].ToString() + "', Link_Status = 'Y' where ledger_Code = " + Ledger_Code_From + " and ledger_NAme = '" + Dt.Rows[0]["LedgerName"].ToString() + "' and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and link_status is null");
                        MyBase.Execute("update Ledger_Contact set Ledger_Code = " + Dt.Rows[0]["LedgerCode"].ToString() + ", Person = '" + Dt.Rows[0]["LContPer"].ToString() + "', Department = '" + Dt.Rows[0]["LContDept"].ToString() + "', Phone = '" + Dt.Rows[0]["LPhone"].ToString() + "' where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");

                        MyBase.Execute("Update Cheque_Details set Ledger_Code = " + Dt.Rows[0]["LedgerCode"].ToString() + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                        MyBase.Execute("Update Cheque_Details set Bank_Ledger_Code = " + Dt.Rows[0]["LedgerCode"].ToString() + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");

                        MyBase.Execute("Update Ledger_breakup set Ledger_Code = " + Dt.Rows[0]["LedgerCode"].ToString() + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                        MyBase.Execute("Update Ledger_Recon set Ledger_Code = " + Dt.Rows[0]["LedgerCode"].ToString() + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");

                        MyBase.Execute("Update Voucher_Details set Ledger_Code = " + Dt.Rows[0]["LedgerCode"].ToString() + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                        MyBase.Execute("Update Voucher_Details set Rev_ledCode = " + Dt.Rows[0]["LedgerCode"].ToString() + " where Rev_ledCode = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                        MyBase.Execute("Update Voucher_breakup_bills set Ledger_Code = " + Dt.Rows[0]["LedgerCode"].ToString() + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");

                        if (Delete_Condition)
                        {
                            MyBase.Execute("Delete from ledger_master where company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Ledger_Code_To);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Meriging_ledger_WOut_LinkStatus(int Ledger_Code_From, int CompCode, String Year_Code, int Ledger_Code_To, Boolean Delete_Condition, String OPBal)
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("Select * from Ledger_Master where ledger_Code = " + Ledger_Code_To + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    if (MyBase.Get_RecordCount("Ledger_Master", "Ledger_Code = " + Ledger_Code_From + " and company_Code =  " + CompCode + " and year_Code = '" + Year_Code + "'") > 0)
                    {
                        if (Ledger_Code_From != Ledger_Code_To)
                        {
                            MyBase.Execute("Update Cheque_Details set Ledger_Code = " + Dt.Rows[0]["Ledger_Code"].ToString() + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                            MyBase.Execute("Update Cheque_Details set Bank_Ledger_Code = " + Dt.Rows[0]["Ledger_Code"].ToString() + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");

                            MyBase.Execute("Update Ledger_breakup set Ledger_Code = " + Dt.Rows[0]["Ledger_Code"].ToString() + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                            MyBase.Execute("Update Ledger_Recon set Ledger_Code = " + Dt.Rows[0]["Ledger_Code"].ToString() + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");

                            MyBase.Execute("Update Voucher_Details set Ledger_Code = " + Dt.Rows[0]["Ledger_Code"].ToString() + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                            MyBase.Execute("Update Voucher_Details set Rev_ledCode = " + Dt.Rows[0]["Ledger_Code"].ToString() + " where Rev_ledCode = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                            MyBase.Execute("Update Voucher_breakup_bills set Ledger_Code = " + Dt.Rows[0]["Ledger_Code"].ToString() + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");

                            MyBase.Execute("Delete from Ledger_Master where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                            MyBase.Execute("Delete from Ledger_Contact where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");

                            if (OPBal.ToUpper().Contains("CR"))
                            {
                                MyBase.Execute("Update Ledger_master set Ledger_OCredit = " + Convert.ToDouble(OPBal.ToUpper().Replace("CR", "")) + " where ledger_Code = " + Dt.Rows[0]["Ledger_Code"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                            }
                            else
                            {
                                MyBase.Execute("Update Ledger_master set Ledger_ODebit = " + Convert.ToDouble(OPBal.ToUpper().Replace("DR", "")) + " where ledger_Code = " + Dt.Rows[0]["Ledger_Code"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void Meriging_ledger_New(int Ledger_Code_From, int CompCode, String Year_Code, int Ledger_Code_To)
        {
            DataTable Dt = new DataTable();
            try
            {
                if (MyBase.Get_RecordCount("Ledger_Master", "Ledger_Code = " + Ledger_Code_From + " and company_Code =  " + CompCode + " and year_Code = '" + Year_Code + "'") > 0)
                {
                    MyBase.Execute("update Ledger_Master set Ledger_Code = " + Ledger_Code_To + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' ");
                    MyBase.Execute("update Ledger_Contact set Ledger_Code = " + Ledger_Code_To + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");

                    MyBase.Execute("Update Cheque_Details set Ledger_Code = " + Ledger_Code_To + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                    MyBase.Execute("Update Cheque_Details set Bank_Ledger_Code = " + Ledger_Code_To + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");

                    MyBase.Execute("Update Ledger_breakup set Ledger_Code = " + Ledger_Code_To + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                    MyBase.Execute("Update Ledger_Recon set Ledger_Code = " + Ledger_Code_To + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");

                    MyBase.Execute("Update Voucher_Details set Ledger_Code = " + Ledger_Code_To + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                    MyBase.Execute("Update Voucher_Details set Rev_ledCode = " + Ledger_Code_To + " where Rev_ledCode = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                    MyBase.Execute("Update Voucher_breakup_bills set Ledger_Code = " + Ledger_Code_To + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Rev_ledger_Updation()
        {

            DataTable Dt = new DataTable();

            try
            {
                MyBase.Execute_Qry(" select Vcode, Ledger_Code as F_Ledger, 0 as S_Ledger from Voucher_Details where Slno = 1 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' ", "Rev_V1");
                MyBase.Execute_Qry(" select Vcode, 0 as F_Ledger, Ledger_Code as S_Ledger from Voucher_Details where Slno = 2 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' ", "Rev_V2");

                MyBase.Execute_Qry(" select v1.Vcode, min(v1.F_Ledger) F_Ledger, min(v2.s_ledger) S_Ledger from Rev_V1 v1 left join Rev_V2 v2 on v1.Vcode = v2.vcode group by v1.vcode ", "Rev_Ledger");

                MyBase.Load_Data("Select * from Rev_Ledger order by vcode", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Update voucher_Details set Rev_LedCode = " + Dt.Rows[i]["F_Ledger"].ToString() + " where vcode = " + Dt.Rows[i]["vcode"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and slno > 1");
                    MyBase.Execute("Update voucher_Details set Rev_LedCode = " + Dt.Rows[i]["S_Ledger"].ToString() + " where vcode = " + Dt.Rows[i]["vcode"].ToString() + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and slno = 1");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Meriging_ledger_New_Delete(int Ledger_Code_From, int CompCode, String Year_Code, int Ledger_Code_To)
        {
            DataTable Dt = new DataTable();
            try
            {
                if (MyBase.Get_RecordCount("Ledger_Master", "Ledger_Code = " + Ledger_Code_From + " and company_Code =  " + CompCode + " and year_Code = '" + Year_Code + "'") > 0)
                {
                    if (Ledger_Code_From != Ledger_Code_To)
                    {
                        MyBase.Execute("Update Cheque_Details set Ledger_Code = " + Ledger_Code_To + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                        MyBase.Execute("Update Cheque_Details set Bank_Ledger_Code = " + Ledger_Code_To + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");

                        MyBase.Execute("Update Ledger_breakup set Ledger_Code = " + Ledger_Code_To + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                        MyBase.Execute("Update Ledger_Recon set Ledger_Code = " + Ledger_Code_To + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");

                        MyBase.Execute("Update Voucher_Details set Ledger_Code = " + Ledger_Code_To + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                        MyBase.Execute("Update Voucher_Details set Rev_ledCode = " + Ledger_Code_To + " where Rev_ledCode = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                        MyBase.Execute("Update Voucher_breakup_bills set Ledger_Code = " + Ledger_Code_To + " where ledger_Code = " + Ledger_Code_From + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");

                        MyBase.Execute("Delete from ledger_master where company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Ledger_Code_From);
                        MyBase.Execute("Delete from ledger_Contact where company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Ledger_Code_From);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Narration_Update()
        {
            DataTable Dt = new DataTable();
            DataTable Dt1 = new DataTable();
            try
            {
                if (CompName.ToUpper().Contains("RAJARAM") == true || CompName.ToUpper().Contains("AEGAN") == true)
                {
                    if (MyBase.Check_Table("narration_tab") == false)
                    {
                        MyBase.Load_Data("select vcode, vdate, ledger_Code, company_Code, year_Code from voucher_Details where ledger_Code in (select ledger_Code from ledger_master where company_Code = " + CompCode + "  and year_Code = '" + YearCode + "' and ledger_group_Code in (Select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved in (1700))) and ((narration = '') or (narration = '-') or (narration is null))", ref Dt);
                        for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                        {
                            MyBase.Load_Data("Select isnull(Narration, '') Narration from voucher_Details where vcode = " + Dt.Rows[i]["vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code <> " + Dt.Rows[i]["Ledger_Code"].ToString() + " and len(rtrim(ltrim(narration))) > 1 ", ref Dt1);
                            if (Dt1.Rows.Count > 0)
                            {
                                MyBase.Execute("Update voucher_Details set narration = '" + Dt1.Rows[0]["Narration"].ToString() + "' where vcode = " + Dt.Rows[i]["vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString());
                            }
                        }
                        MyBase.Execute("Create table Narration_tab (Code int)");
                    }

                    if (MyBase.Check_Table("narration_tab1") == false)
                    {
                        MyBase.Load_Data("select vcode, vdate, ledger_Code, company_Code, year_Code from voucher_Details where ledger_Code in (select ledger_Code from ledger_master where company_Code = " + CompCode + "  and year_Code = '" + YearCode + "' and ledger_group_Code in (Select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved in (1600))) and ((narration = '') or (narration = '-') or (narration is null))", ref Dt);
                        for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                        {
                            MyBase.Load_Data("Select isnull(Narration, '') Narration from voucher_Details where vcode = " + Dt.Rows[i]["vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code <> " + Dt.Rows[i]["Ledger_Code"].ToString() + " and len(rtrim(ltrim(narration))) > 1 ", ref Dt1);
                            if (Dt1.Rows.Count > 0)
                            {
                                MyBase.Execute("Update voucher_Details set narration = '" + Dt1.Rows[0]["Narration"].ToString() + "' where vcode = " + Dt.Rows[i]["vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString());
                            }
                        }
                        MyBase.Execute("Create table Narration_tab1 (Code int)");
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Int64 Next_voucher_Code_From_List(Int64 Vcode)
        {
            DataTable Dt = new DataTable();
            String TblName = String.Empty;
            try
            {
                TblName = "Current_voucher_List" + Environment.MachineName.Replace("-", "");
                MyBase.Load_Data("select isnull(vcode, 0) Vcode from " + TblName + " where slno = (Select Min(Slno) from " + TblName + " where Slno > (Select Max(Slno) from " + TblName + " where vcode = " + Vcode + "))", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    return Convert.ToInt64(Dt.Rows[0]["vcode"]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int64 Previous_voucher_Code_From_List(Int64 Vcode)
        {
            DataTable Dt = new DataTable();
            String TblName = String.Empty;
            try
            {
                TblName = "Current_voucher_List" + Environment.MachineName.Replace("-", "");
                MyBase.Load_Data("select isnull(vcode, 0) Vcode from " + TblName + " where slno = (Select Max(Slno) from " + TblName + " where Slno < (Select Min(Slno) from " + TblName + " where vcode = " + Vcode + "))", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    return Convert.ToInt64(Dt.Rows[0]["vcode"]);
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Set_voucher_list(ref DataTable Dt, string VcodeName)
        {
            String TblName = String.Empty;
            try
            {
                TblName = "Current_voucher_List" + Environment.MachineName.Replace("-", "");
                if (MyBase.Check_Table (TblName))
                {
                    MyBase.Execute ("Drop table " + TblName);
                }
                MyBase.Execute("Create table " + TblName + " (Slno int, Vcode Bigint)");
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute ("insert into " + TblName + " values (" + i + ", " + Dt.Rows[i][VcodeName].ToString() + ")");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //void Inventory_Menus(Boolean Val)
        //{
        //    try
        //    {
        //        for (int i=0;i<=MasterMenu.DropDownItems.Count - 1;i++)
        //        {
        //            if (MasterMenu.DropDownItems[i] is ToolStripMenuItem)
        //            {
        //                ToolStripMenuItem M = (ToolStripMenuItem)MasterMenu.DropDownItems[i];
        //                if (M.Name.ToUpper().Contains("INVENTORY"))
        //                {
        //                    M.Visible = Val;
        //                }
        //            }
        //        }

        //        for (int i = 0; i <= TransactionMenu.DropDownItems.Count - 1; i++)
        //        {
        //            if (TransactionMenu.DropDownItems[i] is ToolStripMenuItem)
        //            {
        //                ToolStripMenuItem M = (ToolStripMenuItem)TransactionMenu.DropDownItems[i];
        //                if (M.Name.ToUpper().Contains("INVENTORY"))
        //                {
        //                    M.Visible = Val;
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        void Inventory_Menus(Boolean Val)
        {
            try
            {
                //for (int i = 0; i <= ToolStrip_InventoryMenu.DropDownItems.Count - 1; i++)
                //{
                //    if (ToolStrip_InventoryMenu.DropDownItems[i] is ToolStripMenuItem)
                //    {
                //        ToolStripMenuItem M = (ToolStripMenuItem)ToolStrip_InventoryMenu.DropDownItems[i];
                //        M.Visible = Val;
                //    }
                //}
                //ToolStrip_InventoryMenu.Visible = Val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void DataBase_Options()
        {
            DataTable Dt = new DataTable();
            try
            {
                if (MyBase.Check_Table("Product_SubGroup_master"))
                {
                    MyBase.Load_Data("Select * from sysobjects where xtype = 'PK' and name = 'PK_Group_Code'", ref Dt);
                    if (Dt.Rows.Count == 0)
                    {
                        MyBase.Execute("alter table Product_Group_master alter column product_group_Code int not null");
                        MyBase.Execute("alter table Product_Group_master add constraint Pk_Group_Code primary key (product_group_Code)");
                    }

                    MyBase.Load_Data("Select * from sysobjects where xtype = 'PK' and name = 'PK_SubGroup_Code'", ref Dt);
                    if (Dt.Rows.Count == 0)
                    {
                        MyBase.Execute("alter table Product_SubGroup_master alter column product_Subgroup_Code int not null");
                        MyBase.Execute("alter table Product_SubGroup_master add constraint Pk_SubGroup_Code primary key (product_Subgroup_Code)");
                        MyBase.Execute("alter table Product_SubGroup_master add constraint Fk_Group_Code Foreign key(product_group_Code) references Product_Group_Master(Product_Group_Code)");
                    }

                    MyBase.Load_Data("Select * from sysobjects where xtype = 'PK' and name = 'PK_UOM_Code'", ref Dt);
                    if (Dt.Rows.Count == 0)
                    {
                        MyBase.Execute("alter table UOM_master alter column UOM_Code int not null");
                        MyBase.Execute("alter table UOM_master add constraint Pk_UOM_Code primary key (UOM_CODE)");
                    }

                    MyBase.Load_Data("Select * from sysobjects where xtype = 'PK' and name = 'PK_Location_Code'", ref Dt);
                    if (Dt.Rows.Count == 0)
                    {
                        MyBase.Execute("alter table Location_master alter column Location_Code int not null");
                        MyBase.Execute("alter table Location_master add constraint Pk_Location_Code primary key (Location_CODE)");
                    }

                    MyBase.Load_Data("Select * from sysobjects where xtype = 'PK' and name = 'PK_Item_Code'", ref Dt);
                    if (Dt.Rows.Count == 0)
                    {
                        MyBase.Execute("alter table Product_master alter column Item_Code int not null");
                        MyBase.Execute("alter table Product_master add constraint PK_Item_Code primary key (Item_Code)");
                        MyBase.Execute("alter table Product_master add constraint Fk_Product_SubGroup_Code Foreign key(product_Subgroup_Code) references Product_SubGroup_Master(Product_SubGroup_Code)");
                        MyBase.Execute("alter table Product_master add constraint Fk__UOM_Code Foreign key(UOM_Code) references UOM_Master(UOM_Code)");
                    }

                    MyBase.Load_Data("Select * from sysobjects where xtype = 'F' and name = 'PK_Item_Stock_Breakup_Code'", ref Dt);
                    if (Dt.Rows.Count == 0)
                    {
                        MyBase.Execute("alter table product_Master_Stock_Breakup add constraint PK_Item_Stock_Breakup_Code Foreign key(Item_Code) references Product_Master(Item_Code)");
                        MyBase.Execute("alter table product_Master_Stock_Breakup add constraint Fk_Location_Code Foreign key(Location_Code) references Location_Master(Location_Code)");
                    }
                    
                    MyBase.Load_Data("Select * from sysobjects where xtype = 'F' and name = 'FK_Item_Code_Price'", ref Dt);
                    if (Dt.Rows.Count == 0)
                    {
                        MyBase.Execute("alter table Item_PriceList add constraint FK_Item_Code_Price Foreign key(Item_Code) references Product_Master(Item_Code)");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //void Inventory_Portion()
        //{
        //    try
        //    {
        //        if (Inventory)
        //        {
        //            Inventory_Menus(true);
        //            if (MyBase.Check_Table("Product_Group_master") == false)
        //            {
        //                MyBase.Execute(" CREATE TABLE [dbo].[Product_Group_master](	[Product_Group_Code] [int] NULL,	[Product_Group_name] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,	[Product_Group_desc] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,	[NEW_EMPCODE] [numeric](4, 0) NULL,	[NEW_SYSCODE] [numeric](4, 0) NULL,	[NEW_DATETIME] [datetime] NULL,	[ALTER_EMPCODE] [numeric](4, 0) NULL,	[ALTER_SYSCODE] [numeric](4, 0) NULL,	[ALTER_DATETIME] [datetime] NULL,	[COMPANY_CODE] [numeric](2, 0) NULL,	[YEAR_CODE] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL)");
        //            }
        //            if (MyBase.Check_Table("Product_SubGroup_master") == false)
        //            {
        //                MyBase.Execute(" CREATE TABLE [dbo].[Product_SubGroup_master](	[Product_SubGroup_Code] [int] NULL,	[Product_SubGroup_name] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Product_group_Code int,	[Product_SubGroup_desc] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,	[NEW_EMPCODE] [numeric](4, 0) NULL,	[NEW_SYSCODE] [numeric](4, 0) NULL,	[NEW_DATETIME] [datetime] NULL,	[ALTER_EMPCODE] [numeric](4, 0) NULL,	[ALTER_SYSCODE] [numeric](4, 0) NULL,	[ALTER_DATETIME] [datetime] NULL,	[COMPANY_CODE] [numeric](2, 0) NULL,	[YEAR_CODE] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL)");
        //            }
        //            if (MyBase.Check_Table("UOM_Master") == false)
        //            {
        //                MyBase.Execute("CREATE TABLE [dbo].[UOM_Master]([UOM] [varchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,	[Title] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,	[UOM_CODE] [numeric](4, 0) NULL,	[NEW_EMPCODE] [numeric](4, 0) NULL,	[NEW_SYSCODE] [numeric](4, 0) NULL,	[NEW_DATETIME] [datetime] NULL,	[ALTER_EMPCODE] [numeric](4, 0) NULL,	[ALTER_SYSCODE] [numeric](4, 0) NULL,	[ALTER_DATETIME] [datetime] NULL,	[COMPANY_CODE] [numeric](2, 0) NULL,	[YEAR_CODE] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,	[CROSS1] [varchar](1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL)");
        //            }
        //            if (MyBase.Check_Table("Location_Master") == false)
        //            {
        //                MyBase.Execute("Create table Location_Master (Location_Code Int, Locaton_Name varchar(250))");
        //                MyBase.UpdateSpecialFields("Location_Master");
        //            }
        //            if (MyBase.Check_Table("Product_Master") == false)
        //            {
        //                MyBase.Execute("CREATE TABLE product_Master (Item_Code int, Name varchar(200), UOM_Code int, Product_SubGroup_Code int, Op_Stock numeric(10,3), Op_Rate Numeric(15, 2), Op_Value Numeric(15, 2), Rate Numeric(15,2), ReOrder_Qty Numeric(10, 3), Min_Qty Numeric(10,3), Stock_Location varchar(250), Introduced_On Datetime, Purchase_Rate Numeric(15,2))");
        //                MyBase.UpdateSpecialFields("Product_Master");
        //            }
        //            if (MyBase.Check_Table("product_Master_Stock_Breakup") == false)
        //            {
        //                MyBase.Execute("Create Table product_Master_Stock_Breakup (Item_Code int, Slno int, Location_Code int, Stock Numeric(12,3), Company_Code int, Year_Code varchar(10))");
        //            }
        //            if (MyBase.Check_Table("Item_PriceList") == false)
        //            {
        //                MyBase.Execute("create table Item_PriceList (E_No Numeric(10), E_Date datetime, Effect_Date dateTime, Item_Code int, Old_Rate numeric(15,2), Disc Numeric(10, 2), Disc_Per Numeric(10, 2), New_Rate Numeric(15,2))");
        //                MyBase.UpdateSpecialFields("Item_PriceList");
        //            }
        //            MyBase.Execute_Qry("Select I1.Item_Code, Effect_Date, Isnull(New_Rate, 0) New_Rate, i1.Company_Code, i1.year_Code from Product_Master i1 Left join item_PriceList i2 on i1.Item_Code = i2.Item_Code ", "Price_List");
        //            DataBase_Options();
        //        }
        //        else
        //        {
        //            Inventory_Menus(false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        void Stock_Settings()
        {
            DataTable TempDt = new DataTable();
            try
            {
                if (MyBase.Check_Table("Stock_Settings") == false)
                {
                    MyBase.Execute("create table Stock_settings (Double_UOM varchar(1), Multiple_Company varchar(1), Drawing_Code int)");
                    MyBase.Execute("Insert into Stock_Settings values ('Y', 'N', 50)");
                }
                
                MyBase.Add_NewField("Stock_Settings", "Stock_validation", "Char(1)");

                if (MyBase.Get_RecordCount("Stock_Settings", "Stock_validation is null") > 0)
                {
                    MyBase.Execute("Update stock_Settings set Stock_validation = 'N'");
                }

                MyBase.Load_Data("Select * from Stock_Settings", ref TempDt);
                if (TempDt.Rows.Count > 0)
                {
                    Dup_Company_Code = Convert.ToInt32(TempDt.Rows[0]["Drawing_Code"]);
                    if (TempDt.Rows[0]["Double_UOM"].ToString().ToUpper() == "Y")
                    {
                        Double_UOM = true;
                    }
                    else
                    {
                        Double_UOM = false;
                    }
                    if (TempDt.Rows[0]["Stock_Validation"].ToString().ToUpper() == "Y")
                    {
                        Stock_Validation = true;
                    }
                    else
                    {
                        Stock_Validation = false;
                    }
                }
                else
                {
                    Dup_Company_Code = 0;
                    Double_UOM = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Table_Creation_Stock()
        {
            try
            {
                if (MyBase.Check_Table("Stock") == false)
                {
                    MyBase.Execute("Create table Stock (Item_ID int, Stkwarehouse_ID int, Company_Code int, Year_Code Varchar(10), Stock1 int, Stock2 NUmeric(20,5), constraint FK_Stock_Item_ID foreign key (Item_ID, Company_Code, Year_Code) references Mas_Item (Item_ID, Company_Code, Year_Code), constraint FK_Stkwarehouse_ID foreign key (Stkwarehouse_ID, Company_Code, Year_Code) references Mas_StkWarehouse (Stkwarehouse_ID, Company_Code, Year_Code))");
                }

                if (MyBase.Check_Procedure("Stock_Updation") == false)
                {
                    MyBase.Execute("Create Proc Stock_Updation (@Mode Char(1), @Item_ID int, @Company_Code int, @year_Code varchar(10), @Stkwarehouse_id int, @Stock1 int, @Stock2 Numeric(20,5)) as Begin	Set Nocount on;	Begin try		Begin transaction;		if (@Mode = '+')		Begin			if Exists (Select * from stock where item_ID = @Item_ID and Company_Code = @Company_Code and Year_Code = @year_Code and Stkwarehouse_ID = @Stkwarehouse_id)				Begin					Update Stock Set Stock1 = Stock1 + @Stock1, Stock2 = Stock2 + @Stock2 where item_ID = @Item_ID and Company_Code = @Company_Code and Year_Code = @year_Code and Stkwarehouse_ID = @Stkwarehouse_id;				End 			else				Begin					insert into Stock (Item_ID, Stkwarehouse_ID, Company_Code, Year_Code, Stock1, Stock2) values (@Item_ID, @Stkwarehouse_ID, @Company_Code, @year_Code, @Stock1, @Stock2);				End 		End 		Else if (@Mode = '-')			Begin				Update Stock Set Stock1 = Stock1 - @Stock1, Stock2 = Stock2 - @Stock2 where item_ID = @Item_ID and Company_Code = @Company_Code and Year_Code = @year_Code and Stkwarehouse_ID = @Stkwarehouse_id;			End 		Commit transaction;	End try 	Begin catch 		rollback transaction;		Declare @M nvarchar(4000), @Sev int, @St int;		Select @M = Error_Message(), @Sev = Error_Severity(), @St = Error_State();		raiserror (@M, @Sev, @St);	End catch End ");
                }

                if (MyBase.Check_Function("Get_Stock_Stock1") == false)
                {
                    MyBase.Execute("Create Function Get_Stock_Stock1 (@Item_ID int, @Stkwarehouse_ID int, @Company_Code int, @Year_Code varchar(10)) returns int as Begin	Declare @Stk as int;	Select @Stk = Stock1 from Stock where item_ID = @Item_ID and Company_Code = @Company_Code and Year_Code = @Year_Code and Stkwarehouse_id = @Stkwarehouse_ID;	return @Stk End");
                }

                if (MyBase.Check_Function("Get_Stock_Stock2") == false)
                {
                    MyBase.Execute("Create Function Get_Stock_Stock2 (@Item_ID int, @Stkwarehouse_ID int, @Company_Code int, @Year_Code varchar(10)) returns Numeric(20,5) as Begin	Declare @Stk as Numeric(20,5);	Select @Stk = Stock2 from Stock where item_ID = @Item_ID and Company_Code = @Company_Code and Year_Code = @Year_Code and Stkwarehouse_id = @Stkwarehouse_ID;	return @Stk End");
                }

                if (Stock_Validation)
                {
                    if (MyBase.Get_RecordCount("SysObjects", "Name = 'CHK_Stock1'") == 0)
                    {
                        MyBase.Execute("Alter table stock add constraint CHK_Stock1 Check (Stock1 > 0)");
                    }

                    if (MyBase.Get_RecordCount("SysObjects", "Name = 'CHK_Stock2'") == 0)
                    {
                        MyBase.Execute("Alter table stock add constraint CHK_Stock2 Check (Stock2 > 0)");
                    }
                }
                else
                {
                    if (MyBase.Get_RecordCount("SysObjects", "Name = 'CHK_Stock1'") > 0)
                    {
                        MyBase.Execute("Alter table stock Drop constraint CHK_Stock1");
                    }

                    if (MyBase.Get_RecordCount("SysObjects", "Name = 'CHK_Stock2'") > 0)
                    {
                        MyBase.Execute("Alter table stock Drop constraint CHK_Stock2");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Table_Creation_Category()
        {
            try
            {
                if (MyBase.Check_Table("Mas_Category") == false)
                {
                    MyBase.Execute("create table Mas_Category (RowID int Identity, Category_id int, Category_Name Varchar(100), Company_Code int, Year_Code varchar(10), Constraint PK_Category_Id primary key (Category_ID, Company_Code, Year_Code), Constraint UK_category_Name Unique(Category_Name, Company_Code, Year_Code))");
                }
                MyBase.UpdateSpecialFields_Inventory("Mas_Category");


                if (MyBase.Check_Procedure("Ins_Category_Master") == false)
                {
                    MyBase.Execute("Create PROC INS_CATEGORY_MASTER (@Cate_ID int, @CATEGORY_NAME VARCHAR(100), @COMPANY_CODE INT, @YEAR_CODE VARCHAR(10), @Alter_EmpCode int, @Alter_SysCode int, @Alter_DateTime dateTime)  AS  BEGIN try  	Set nocount on; 	Begin Transaction; 	DECLARE @Category_Id as int;    	if @Cate_ID > 0		begin			set @Category_ID = @Cate_id;		end	else	begin		Select @Category_Id = isnull(MAX(category_id), 0) + 1 from Mas_Category where company_Code = @Company_Code and Year_code = @Year_Code;    	end	INSERT INTO Mas_Category(CATEGORY_ID, CATEGORY_NAME, COMPANY_CODE, YEAR_CODE, Alter_EmpCode, Alter_SysCode, Alter_Datetime) SELECT @CATEGORY_ID, @CATEGORY_NAME, @COMPANY_CODE, @YEAR_CODE, @Alter_EmpCode, @Alter_SysCode, @Alter_Datetime;    	Select @Category_Id 	Commit Transaction; END try Begin Catch  	Declare @EMessage as nvarchar(4000); Declare @ESeverity as int;  Declare @EState as int; 	Rollback Transaction  	Select @EMessage = ERROR_MESSAGE(), @Eseverity = Error_Severity(), @EState = ERROR_STATE();  	raiserror (@EMessage, @ESeverity, @EState); End catch ");
                }

                if (MyBase.Check_Procedure("Upd_category_Master") == false)
                {
                    MyBase.Execute("Create proc Upd_category_Master (@Category_Id int, @CATEGORY_NAME VARCHAR(100), @COMPANY_CODE INT, @YEAR_CODE VARCHAR(10), @Alter_EmpCode int, @Alter_SysCode int, @Alter_Datetime datetime) as Begin 	Set Nocount On;	Begin Try 		Update Mas_Category set Category_Name = @CATEGORY_NAME, Alter_EMpcode = @Alter_Empcode, Alter_SYSCode = @Alter_Syscode, Alter_Datetime = @Alter_Datetime where Category_id = @Category_Id and Company_Code = @COMPANY_CODE and year_Code = @Year_Code; 	End Try 	Begin Catch 		Declare @EMessage as nvarchar(4000); 		Declare @ESeverity as int; 		Declare @EState as int; 		Select @EMessage = ERROR_MESSAGE(), @ESeverity = ERROR_SEVERITY(), @EState = ERROR_STATE(); 		Raiserror (@EMessage, @ESeverity, @EState); 	End Catch end");
                }

                if (MyBase.Check_Procedure("Del_Category_Master") == false)
                {
                    MyBase.Execute("Create proc Del_category_Master (@Category_Id int, @Company_Code int, @Year_Code varchar(10)) as Begin 	Set Nocount on;	Begin Try 		Delete from Mas_Category where Category_id = @Category_Id and Company_Code = @COMPANY_CODE and year_Code = @Year_Code; 	End try 	Begin Catch 		Declare @EMessage nvarchar(4000); 		Declare @ESeverity int; 		Declare @EState int; 		Select @EMEssage = Error_Message(), @ESeverity = ERROR_SEVERITY(), @EState = ERROR_STATE(); 		raiserror (@Emessage, @ESeverity, @EState); 	End Catch end");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Script_Settings()
        {
            String Str = String.Empty;

            try
            {

                Table_Creation_Category();
                Table_Creation_Group();
                Table_Creation_SubGroup();
                Table_Creation_UOM();
                Table_Creation_StkWareHouse();
                Table_Creation_SalesType();
                Table_Creation_Machine();
                Table_Creation_Department();
                Table_Creation_Item();
                Table_Creation_Stock();

                if (Is_Duplicate_Company())
                {
                    if (MyBase.Check_Procedure("DATA_VERIFY_INSERT") == false)
                    {
                        MessageBox.Show("Execute Script Functions ....!", "Vaahini");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Inventory_Portion()
        {
            try
            {
                if (Inventory)
                {
                    Script_Settings();
                    Stock_Settings();
                    Inventory_Menus(true);
                }
                else
                {
                    Inventory_Menus(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Select_Company()
        {
            try
            {
                UserCode = 1;
                UserName = "ADMIN";
                CompCode = 1;
                CompName = "GAINUP INDUSTRIES INDIA PRIVATE LIMITED";
                CompPrintName = "GAINUP INDUSTRIES INDIA PRIVATE LIMITED";
                YearCode = "2010-2011";
                SDate = Convert.ToDateTime("01-APR-2014");
                EDate = Convert.ToDateTime("31-MAR-2020");
                CompPhone = "04545 267726";
                CompFax = "04545 267485";
                CompEmail = "mill@gainup.in";
                CompTin = "";
                CompCst = "";
                CompAddress = "OTTUPATTI";
                OnlyFor_Company = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        void Copy_Company()
        {
            try
            {
                if (MyBase.Get_RecordCount("Socks_Companymas", "CompCode = 2") == 0)
                {
                    MyBase.Execute_Tbl("select * from Socks_Companymas where compCode = 1", "t1");
                    MyBase.Execute("update t1 set compcode = 2, sdt = '01-Apr-2011', edt = '31-Mar-2012', company_Code = 2, year_code = '2011-2012'");
                    MyBase.Execute("insert into Socks_Companymas select * from t1");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Copy_Company_Gainup()
        {
            try
            {
                if (MyBase.Get_RecordCount("Socks_Companymas", "Sdt = '01-Apr-2011'") == 0)
                {
                    MyBase.Execute_Tbl("select * from Socks_Companymas where Sdt= '01-Apr-2010'", "t1");
                    MyBase.Execute("update t1 set sdt = '01-Apr-2011', edt = '31-Mar-2012', year_code = '2011-2012'");
                    MyBase.Execute("insert into Socks_Companymas select * from t1");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Update_User_Level_Fixed()
        {
            try
            {

                if (MyBase.Check_Table("Socks_Permission_Master_User_Level_Fixed") == false)
                {
                    MyBase.Execute("Create table Socks_Permission_Master_User_Level_Fixed (Menu_Code int, User_Level_Code int)");
                }

                if (MyBase.Check_Table("User_Level_Fixed") == false)
                {
                    MyBase.Execute("Create table User_Level_Fixed (User_Level_Code int, User_Level_Fixed Varchar(50))");
                }

                if (MyBase.Get_RecordCount("User_Level_Fixed", "User_Level_Fixed = 'MD'") == 0)
                {
                    MyBase.Execute("Insert into User_Level_Fixed Values (1, 'MD')");
                }

                if (MyBase.Get_RecordCount("User_Level_Fixed", "User_Level_Fixed = 'Administrator'") == 0)
                {
                    MyBase.Execute("Insert into User_Level_Fixed Values (2, 'Administrator')");
                }

                if (MyBase.Get_RecordCount("User_Level_Fixed", "User_Level_Fixed = 'Manager'") == 0)
                {
                    MyBase.Execute("Insert into User_Level_Fixed Values (3, 'Manager')");
                }

                if (MyBase.Get_RecordCount("User_Level_Fixed", "User_Level_Fixed = 'Accountant'") == 0)
                {
                    MyBase.Execute("Insert into User_Level_Fixed Values (4, 'Accountant')");
                }

                if (MyBase.Get_RecordCount("User_Level_Fixed", "User_Level_Fixed = 'Cashier'") == 0)
                {
                    MyBase.Execute("Insert into User_Level_Fixed Values (5, 'Cashier')");
                }

                if (MyBase.Get_RecordCount("User_Level_Fixed", "User_Level_Fixed = 'DataEntry'") == 0)
                {
                    MyBase.Execute("Insert into User_Level_Fixed Values (6, 'DataEntry')");
                }

                MyBase.Add_NewField("Socks_User_Master", "User_Level_Code", "int");

                MyBase.Execute("Update Socks_User_Master set User_Level_Code = 6 where User_Level_Code is null");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Create_Functions()
        {
            try
            {
                if (MyBase.Check_Function("Bank_Opening_Balance") == false)
                {
                    MyBase.Execute(" Create Function Bank_Opening_Balance (@Ledger_Code int, @From_Date Datetime, @To_Date Datetime, @Company_Code int, @Year_Code varchar(10)) returns Numeric(20,2) as begin 	declare @Balance as Numeric(20,2);	declare @Debit as Numeric(20,2);	declare @Credit as Numeric(20,2);	Set @Balance = (Select (Case when Ledger_Odebit > 0 then ledger_Odebit else ((-1) * ledger_Ocredit) end) from ledger_master where ledger_Code = @Ledger_Code and Company_Code = @Company_Code and year_Code = @Year_Code);	declare Bank_OpBal Cursor for 	select isnull(Debit, 0) as Debit, (-1) * isnull(credit, 0) as Credit from ledger_Recon where ledger_Code = @Ledger_Code and Company_Code = @Company_Code and year_Code = @Year_Code and Recon_Date not between @From_Date and @To_Date;	open Bank_OpBal;	fetch Next from Bank_OPBal into @Debit, @Credit;	while @@Fetch_status = 0	begin		if @Balance > 0			begin				if @debit > 0					begin						Set @Balance = @Balance + @Debit					end				else if @debit < 0					begin						Set @Balance = @Balance - @Debit					end				else if @Credit > 0					begin						Set @Balance = @Balance - @Credit					end				else if @Credit < 0					begin						Set @Balance = @Balance + @Credit					end			end		else			begin				if @debit > 0					begin						Set @Balance = @Balance - @Debit					end				else if @debit < 0					begin						Set @Balance = @Balance + @Debit					end				else if @Credit > 0				begin						Set @Balance = @Balance + @Credit					end				else if @Credit < 0					begin						Set @Balance = @Balance - @Credit					end			end		fetch Next from Bank_OPBal into @Debit, @Credit;	end	close Bank_OPBal;	deallocate Bank_OPBal;	return @Balance;end ");
                }

                if (MyBase.Check_Procedure("Breakup_Ledger") == false)
                {
                    MyBase.Execute(" Create Procedure Breakup_Ledger (@Ledger_Code Int, @Term Varchar(5), @COMPANY_CODE INT, @YEAR_cODE VARCHAR(10)) as begin	if UPPER(@Term) = 'DEBIT' 		BEGIN			Select Mode, RefDoc, RefDate, (DEBIT - AMOUNT_CL) amount FROM LEDGER_BREAKUP WHERE COMPANY_CODE = @COMPANY_cODE AND YEAR_cODE = @YEAR_cODE AND (DEBIT - AMOUNT_CL) > 0 and mode = 'N' and ledger_Code = @ledger_Code		END	ELSE 		BEGIN			Select Mode, RefDoc, RefDate, (CREDIT - amount_Cl) Amount FROM LEDGER_BREAKUP WHERE COMPANY_CODE = @COMPANY_cODE AND YEAR_cODE = @YEAR_cODE AND (credit - AMOUNT_CL) > 0 and mode = 'N' and ledger_Code = @ledger_Code		END End ");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        public void Company_Initialization()
        {
            DataTable Dt = new DataTable();
            DataTable Dt_1 = new DataTable();
            try
            {
                //Str = "\\\\172.16.10.200\\vaahini\\VSocks";

                //if (Environment.MachineName.ToUpper() != "SONY")
                //{
                //    if (MyBase.Check_Exe("VSocks", Str) == false)
                //    {
                //        MessageBox.Show("Updating VSocks ....!", "Auto Update");
                //        StreamWriter SW = new StreamWriter("C:\\Vaahrep\\EP.txt");
                //        SW.WriteLine(Str);
                //        SW.Close();
                //        Update_Flag = true;
                //        //this.Close();

                //        MyBase.Run_UpdateExe("C:\\ERP\\ERP_Batch\\VSocks.Bat");
                //        Application.Exit();
                //        return;
                //    }
                //}

                
                //Str = "\\\\172.16.10.200\\vaahini\\PRojects";

                //if (MyBase.Check_Exe("PRojects", Str) == false)
                //{
                //    MessageBox.Show("Updating PRojects ....!", "Auto Update");
                //    StreamWriter SW = new StreamWriter("C:\\Vaahrep\\EP.txt");
                //    SW.WriteLine(Str);
                //    SW.Close();
                //    Update_Flag = true;
                //    //this.Close();

                //    MyBase.Run_UpdateExe("C:\\ERP\\ERP_Batch\\PRojects.Bat");
                //    Application.Exit();
                //    return;
                //}

                //EmplNo_TA = Convert.ToInt64(MyBase.GetData_InString("Socks_User_Master", "User_Name", UserName, "EmplNo"));
                //EmplNo_TA = Convert.ToInt64(MyBase.GetData_InString("Socks_Login()", "User_Name", UserName, "Emplno"));
                EmplNo_TA = Convert.ToInt64(MyBase.GetData_InString("Projects.dbo.Projects_Login()", "User_Name", UserName, "Emplno"));
                Emplno = Convert.ToInt32(MyBase.GetData_InString("Projects.dbo.Projects_Login()", "User_Name", UserName, "Emplno"));
                UserSettings();
                if (UserCode != 1 && UserCode != 2 && UserName.ToString().Contains("GK") == true )
                {
                    Completion_Entry(0);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Completion_Entry(int i)
        {
            try
            {
                if (i == 0)
                {
                    //ShowChild(new FrmTimeActionPending(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name(completionEntryToolStripMenuItem));
                }
                else
                {
                    Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Pending", " Select Distinct ORDER_NO, ACTION_NAME,  LEAD_DAYS , MODE, PLAN_DATE , DIFF, Order_Date, Ship_Date, PLAN_ID, LeadTime_Id, Lead_Time, ACTION_ID, Division, Division_ID, EmplNo_Org, 0 Complete_ID, 1 'T' From  Vaahini_Erp_Gainup.Dbo.Time_Action_Fn(3) Where EmplNo = " + EmplNo_TA + " and Complete_Flag = 'N' and Current_Status = 'P' and ACTION_ID != 51 and Approval_Flag = 'T' ORder By  Plan_Date Asc ", string.Empty, 120, 300, 100, 60, 100, 100);
                    if (Dr != null)
                    {
                        Fill_Datas(Dr);
                        //ShowChild(new FrmTimeActionCompleteEntry(OrderNo, Plan_ID, ODate, SDate, LeadDays, LeadID, Division, Division_ID, Action_ID, Comp_ID), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name(completionEntryToolStripMenuItem));
                    }
                }
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
                OrderNo = Dr["ORDER_NO"].ToString();
                Plan_ID = Convert.ToInt32(Dr["Plan_ID"]);
                ODate = Convert.ToDateTime(Dr["Order_Date"].ToString());
                SDate = Convert.ToDateTime(Dr["Ship_Date"].ToString());
                LeadDays = Convert.ToInt64(Dr["Lead_Time"]);
                LeadID = Convert.ToInt64(Dr["LeadTime_ID"]);
                Division_ID = Convert.ToInt64(Dr["Division_ID"]);
                Action_ID = Convert.ToInt64(Dr["ACTION_ID"]);
                Division = Dr["Division"].ToString();
                Comp_ID = Convert.ToInt64(Dr["COMPLETE_ID"]);
                EmplNo_Org = Convert.ToInt64(Dr["EmplNo_Org"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Company_ChangeOver(int Company_Code, String Year_Code)
        {
            try
            {
                Progress_visible(true);
                Progress_Max(100);
                CompCode = Company_Code;
                CompName = MyBase.GetData_InString("Projects_Companymas", "CompCode", CompCode.ToString(), "CompName");
                CompPrintName = MyBase.GetData_InString("Projects_Companymas", "CompCode", CompCode.ToString(), "InPrinting");
                Progress_Max(20);
                YearCode = Year_Code;
                SDate = MyBase.GetData_InDate("Projects_Companymas", "CompCode", CompCode.ToString(), "Sdt");
                EDate = MyBase.GetData_InDate("Projects_Companymas", "CompCode", CompCode.ToString(), "Edt");
                CompPhone = MyBase.GetData_InString("Projects_Companymas", "CompCode", CompCode.ToString(), "CompPhone");
                CompFax = MyBase.GetData_InString("Projects_Companymas", "CompCode", CompCode.ToString(), "CompFax");
                Progress_Max(20);
                CompEmail = MyBase.GetData_InString("Projects_Companymas", "CompCode", CompCode.ToString(), "CompEmail");
                CompTin = MyBase.GetData_InString("Projects_Companymas", "CompCode", CompCode.ToString(), "CompTNGSTNo");
                CompCst = MyBase.GetData_InString("Projects_Companymas", "CompCode", CompCode.ToString(), "CompCSTNo");
                Progress_Max(20);
                CompAddress = MyBase.Company_Address(CompCode);
                Company_Initialization();
                Progress_Max(40);
                Progress_visible(false);
            }
            catch (Exception ex)
            {
                Progress_visible(false);
                throw ex;
            }
        }

        //void OutStanding_Updation_Tool()
        //{
        //    DataTable Dt = new DataTable();
        //    DataTable Dt1 = new DataTable();
        //    DataTable Dt2 = new DataTable();
        //    Double Credit = 0;
        //    try
        //    {
        //        MyBase.Load_Data("SELECT Ledger_Code FROM ledger_Master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_group_Code in (select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4800)", ref Dt);
        //        for (int i = 0; i <= Dt.Rows.Count - 1; i++)
        //        {
        //            MyBase.Load_Data("SELECT isnull(Sum(Credit), 0) as Credit  FROM VOUCHER_BREAKUP_BILLS WHERE LEDGEr_code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and credit > 0 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' order by RefDate", ref Dt1);
        //            Credit = Convert.ToDouble(Dt1.Rows[0][0]);
        //            if (Credit > 0)
        //            {
        //                MyBase.Load_Data("SELECT * FROM VOUCHER_BREAKUP_BILLS WHERE LEDGEr_code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and Debit > 0 and Mode = 'N' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' order by RefDate", ref Dt1);
        //                for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
        //                {

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void Year_Ending_Update_Ledger_heads(int From_Comp_Code, String Prv_Year_Code, int New_Comp_Code, String New_Year_Code)
        {
            DataTable Dt = new DataTable();
            Int32 Ledger_Code = 0;
            try
            {
                MyBase.Load_Data("select ledger_Code, ledger_Name, ledger_InPrint, Ledger_Group_Code, ledger_Odebit, Ledger_OCredit,ledger_Address, ledger_Area_Code, ledger_Phone, Ledger_Fax, Ledger_Email, ledger_Website, Ledger_TIN, Ledger_CST, tax_per, CRLimit, Breakup, cheque_name, panno, tdsapplicable, tdstype, section_No, tdsrateper, local_inter, category_type, tds_ledger_Code, tds_deduct_On from ledger_master where company_Code = " + From_Comp_Code + " and year_COde = '" + Prv_Year_Code + "' and Ledger_Name not in (Select Ledger_Name from Ledger_Master where company_Code = " + New_Comp_Code + " and year_Code = '" + New_Year_Code + "') order by ledger_Name", ref Dt);
                for (int i=0;i<=Dt.Rows.Count - 1;i++)
                {
                    if (MyBase.Get_RecordCount("Ledger_Master", "ledger_Name = '" + Dt.Rows[i]["ledger_name"].ToString() + "' and company_Code = " + New_Comp_Code + " and year_Code = '" + New_Year_Code + "'") == 0)
                    {
                        Ledger_Code = Convert.ToInt32(MyBase.Max("ledger_Master", "Ledger_Code", "Company_Code = " + New_Comp_Code + " and year_Code = '" + New_Year_Code + "'", true));
                        if (Ledger_Code <= Convert.ToInt32(Dt.Rows[i]["ledger_Code"]))
                        {
                            Ledger_Code = Convert.ToInt32(Dt.Rows[i]["ledger_Code"]);
                        }
                        MyBase.Execute("Insert into Ledger_Master (ledger_Code, ledger_name, ledger_inPrint, ledger_Group_Code, ledger_Odebit, Ledger_OCredit, Ledger_Address, ledger_Area_Code, Ledger_Phone, Ledger_Fax, Ledger_Email, Ledger_Website, Ledger_Tin, Ledger_Cst, Tax_Per, CrLimit, Breakup, Cheque_Name, PanNo, tdsapplicable, tdstype, section_No, tdsrateper, local_Inter, category_type, tds_ledger_Code, tds_deduct_on, company_Code, year_Code) select " + Ledger_Code + ", ledger_name, ledger_inPrint, ledger_Group_Code, ledger_Odebit, Ledger_OCredit, Ledger_Address, ledger_Area_Code, Ledger_Phone, Ledger_Fax, Ledger_Email, Ledger_Website, Ledger_Tin, Ledger_Cst, Tax_Per, CrLimit, Breakup, Cheque_Name, PanNo, tdsapplicable, tdstype, section_No, tdsrateper, local_Inter, category_type, tds_ledger_Code, tds_deduct_on, " + New_Comp_Code + ", '" + New_Year_Code + "' from ledger_Master where ledger_Code = " + Dt.Rows[i]["ledger_Code"].ToString() + " and company_Code = " + From_Comp_Code + " and year_Code = '" + Prv_Year_Code + "'");
                        MyBase.Execute("insert into Ledger_Contact select " + Ledger_Code + ", Slno, person, department, designation, Phone, " + New_Comp_Code + ", '" + New_Year_Code + "' from ledger_Contact where company_Code = " + From_Comp_Code + " and year_Code = '" + Prv_Year_Code + "' and ledger_Code = " + Dt.Rows[i]["ledger_Code"].ToString());
                    }
                }

                Dt = new DataTable();
                MyBase.Load_Data("Select * from Socks_Companymas where company_Code = " + From_Comp_Code, ref Dt);
                MyBase.Current_Balance(0, Convert.ToDateTime(Dt.Rows[0]["Sdt"]), From_Comp_Code, Prv_Year_Code, true);
                MyBase.Execute_Qry("select c1.ledger_Code, ledger_name, (case when Mode = 'Cr' then bal_amount else 0 end) Ocredit, (case when Mode = 'Dr' then bal_amount else 0 end) Odebit from curBal c1 left join ledger_master l1 on c1.ledger_Code = l1.ledger_Code where c1.ledger_Code > 0 and l1.company_Code = " + From_Comp_Code + " and l1.year_Code = '" + Prv_Year_Code + "' ", "CO_Ledger");
                MyBase.Execute("update ledger_Master set ledger_Odebit = c1.odebit, ledger_Ocredit = c1.Ocredit from co_ledger C1 left join ledger_master l1 on l1.ledger_Name = c1.ledger_name where l1.company_Code = " + New_Comp_Code + " and l1.year_Code = '" + New_Year_Code + "'");


                if (CompName.ToUpper().Contains("RAJARAM"))
                {
                    MyBase.Execute("update ledger_Master set ledger_Odebit = 0, ledger_Ocredit = 0 where company_code = " + New_Comp_Code + " and year_Code = '" + New_Year_Code + "' and ledger_Group_Code in (Select groupcode from groupmas where company_code = " + New_Comp_Code + " and year_Code = '" + New_Year_Code + "' and groupreserved in (4400, 4500, 4200, 4100, 3700, 4900, 5500, 1400, 1800, 3400, 6400, 3100, 3500))");
                }
                else if (CompName.ToUpper().Contains("AEGAN") || CompName.ToUpper().Contains("SHESHTHRA"))
                {
                    MyBase.Execute("update ledger_Master set ledger_Odebit = 0, ledger_Ocredit = 0 where company_code = " + New_Comp_Code + " and year_Code = '" + New_Year_Code + "' and ledger_Group_Code in (Select groupcode from groupmas where company_code = " + New_Comp_Code + " and year_Code = '" + New_Year_Code + "' and groupreserved in (4400, 4500, 4200, 4100, 3700, 4900, 5500, 1400, 1800, 3400, 6400, 3500, 6500, 5600))");
                }
                else if (CompName.ToUpper().Contains("GAINUP") || CompName.ToUpper().Contains("ALAMELU"))
                {
                    MyBase.Execute("update ledger_Master set ledger_Odebit = 0, ledger_Ocredit = 0 where company_code = " + New_Comp_Code + " and year_Code = '" + New_Year_Code + "' and ledger_Group_Code in (Select groupcode from groupmas where company_code = " + New_Comp_Code + " and year_Code = '" + New_Year_Code + "' and groupreserved in (4400, 4500, 4200, 4100, 3700, 4900, 5500, 1400, 1800, 3400, 6400, 3500, 6500, 5600))");
                }

                Opening_Stock_updation_New(From_Comp_Code, Prv_Year_Code, New_Comp_Code, New_Year_Code);


                //Simple BY Query
                //MyBase.Execute("update ledger_breakup set debit = l1.ledger_Odebit, credit = l1.ledger_Ocredit from ledger_Master l1 left join ledger_breakup l2 on l1.ledger_Code = l2.ledger_Code and l1.company_Code = l2.company_Code and l1.year_Code = l2.year_Code where l1.company_Code = " + New_Comp_Code + " and l1.year_COde = '" + New_Year_Code + "' and l2.refdoc = 'OPN' and ((l1.ledger_Odebit > 0) or (l1.ledger_Ocredit >0))");

                Dt = new DataTable();
                MyBase.Load_Data("Select Ledger_Code, ledger_odebit, ledger_Ocredit from ledger_Master where company_Code = " + New_Comp_Code + " and year_Code = '" + New_Year_Code + "' order by ledger_Code", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyBase.Get_RecordCount("ledger_breakup", "ledger_Code = " + Dt.Rows[i]["ledger_Code"].ToString() + " and refDoc = 'OPN' and term = 'LEDGER' AND REF = 'L1' AND COMPANY_cODE = " + New_Comp_Code + " AND YEAR_cODE = '" + New_Year_Code + "' AND MODE = 'N'") > 0)
                    {
                        MyBase.Execute("update ledger_breakup set debit = " + Convert.ToDouble(Dt.Rows[i]["ledger_Odebit"]) + ", credit = " + Convert.ToDouble(Dt.Rows[i]["ledger_Ocredit"]) + " where ledger_Code = " + Dt.Rows[i]["ledger_Code"].ToString() + " and refDoc = 'OPN' and term = 'LEDGER' AND REF = 'L1' AND COMPANY_cODE = " + New_Comp_Code + " AND YEAR_cODE = '" + New_Year_Code + "' AND MODE = 'N'");
                    }
                    else
                    {
                        MyBase.Execute("insert into ledger_breakup select " + Dt.Rows[i]["ledger_Code"].ToString() + ", 'LEDGER', 1, 'N', 'OPN', '" + String.Format("{0:dd-MMM-yyyy}", SDate) + "', " + Dt.Rows[i]["ledger_Ocredit"].ToString() + ", " + Dt.Rows[i]["ledger_OCredit"].ToString() + ", 0, 4, 0, 0, 'L1', " + New_Comp_Code + ", '" + New_Year_Code + "', NULL ");
                    }
                }

                MyBase.Execute("Delete from ledger_breakup where company_Code = " + New_Comp_Code + " and year_Code = '" + New_Year_Code + "' and Mode = 'N' and term = 'LEDGER' AND REF = 'L1' AND DEBIT = 0 AND CREDIT = 0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        void Opening_Stock_updation_New(int From_CompCode, String From_Year_Code, int To_CompCode, String To_Year_Code)
        {
            try
            {
                MyBase.Execute_Qry("select Ledger_Code, Debit from Closing_Stock where Edate = (Select Max(EDate) from Closing_Stock where COMPANY_CODE = " + From_CompCode + " and year_code = '" + From_Year_Code + "') and COMPANY_CODE = " + From_CompCode + " and YEAR_CODE = '" + From_Year_Code + "'", "Prv_Closing_Stock");
                MyBase.Execute("update Ledger_Master set Ledger_ODebit = isnull(P1.debit, 0) from Prv_Closing_Stock p1 left join Ledger_Master l1 on p1.ledger_Code = l1.Ledger_Code where l1.COMPANY_CODE = " + To_CompCode + " and l1.YEAR_CODE = '" + To_Year_Code + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Year_Carry_Over(int From_Comp_Code, String Prv_Year_Code, int New_Comp_Code, String New_Year_Code)
        {
            try
            {
                if (MyBase.Get_RecordCount("Socks_Companymas", "CompCode = " + New_Comp_Code) > 0)
                {
                    ////Ledger_Master
                    //MyBase.Execute("Delete from ledger_Master where Company_Code = " + New_Comp_Code + " and Year_Code = '" + New_Year_Code + "'");
                    //MyBase.Execute_Tbl("Select * from Ledger_master where company_Code = " + From_Comp_Code + " and year_Code = '" + Prv_Year_Code + "'", "T1");
                    //MyBase.Execute("Update t1 Set Ledger_Odebit = 0, Ledger_Ocredit = 0, Company_Code = " + New_Comp_Code + ", year_Code = '" + New_Year_Code + "'");
                    //MyBase.Execute("Insert into Ledger_Master select * from t1");
                    ////Ledger_Contact
                    //MyBase.Execute("Delete from ledger_Contact where Company_Code = " + New_Comp_Code + " and Year_Code = '" + New_Year_Code + "'");
                    //MyBase.Execute_Tbl("Select * from Ledger_Contact where company_Code = " + From_Comp_Code + " and year_Code = '" + Prv_Year_Code + "'", "T1");
                    //MyBase.Execute("Update t1 Set Company_Code = " + New_Comp_Code + ", year_Code = '" + New_Year_Code + "'");
                    //MyBase.Execute("Insert into Ledger_Contact select * from t1");

                    if (CompName.ToUpper().Contains("SHESHTHRA"))
                    {
                        ////GroupMaster
                        if (MyBase.Get_RecordCount("GroupMas", "Company_Code = " + New_Comp_Code + " AND YEAR_CODE = '" + New_Year_Code + "'") == 0)
                        {
                            MyBase.Execute("Delete from GroupMas where Company_Code = " + New_Comp_Code + " and Year_Code = '" + New_Year_Code + "'");
                            MyBase.Execute_Tbl("Select * from GroupMas where company_Code = " + From_Comp_Code + " and year_Code = '" + Prv_Year_Code + "'", "T1");
                            MyBase.Execute("Update t1 Set Company_Code = " + New_Comp_Code + ", year_Code = '" + New_Year_Code + "'");
                            MyBase.Execute("Insert into GroupMas select * from t1");
                        }
                        ////AreaMaster
                        if (MyBase.Get_RecordCount("Area_master", "Company_Code = " + New_Comp_Code + " AND YEAR_cODE = '" + New_Year_Code + "'") == 0)
                        {
                            MyBase.Execute("Delete from Area_master where Company_Code = " + New_Comp_Code + " and Year_Code = '" + New_Year_Code + "'");
                            MyBase.Execute_Tbl("Select * from Area_Master where company_Code = " + From_Comp_Code + " and year_Code = '" + Prv_Year_Code + "'", "T1");
                            MyBase.Execute("Update t1 Set Company_Code = " + New_Comp_Code + ", year_Code = '" + New_Year_Code + "'");
                            MyBase.Execute("Insert into Area_Master select * from t1");
                        }
                        ////AreaGroup_Master
                        if (MyBase.Get_RecordCount("AreaGroup_Master", "Company_Code = " + New_Comp_Code + " AND YEAR_cODE = '" + New_Year_Code + "'") == 0)
                        {
                            MyBase.Execute("Delete from AreaGroup_Master where Company_Code = " + New_Comp_Code + " and Year_Code = '" + New_Year_Code + "'");
                            MyBase.Execute_Tbl("Select * from AreaGroup_Master where company_Code = " + From_Comp_Code + " and year_Code = '" + Prv_Year_Code + "'", "T1");
                            MyBase.Execute("Update t1 Set Company_Code = " + New_Comp_Code + ", year_Code = '" + New_Year_Code + "'");
                            MyBase.Execute("Insert into AreaGroup_Master select * from t1");
                        }
                        ////Division_Master
                        if (MyBase.Get_RecordCount("Division_Master", "Company_Code = " + New_Comp_Code + " AND YEAR_cODE = '" + New_Year_Code + "'") == 0)
                        {
                            MyBase.Execute("Delete from Division_Master where Company_Code = " + New_Comp_Code + " and Year_Code = '" + New_Year_Code + "'");
                            MyBase.Execute_Tbl("Select * from Division_Master where company_Code = " + From_Comp_Code + " and year_Code = '" + Prv_Year_Code + "'", "T1");
                            MyBase.Execute("Update t1 Set Company_Code = " + New_Comp_Code + ", year_Code = '" + New_Year_Code + "'");
                            MyBase.Execute("Insert into Division_Master select * from t1");
                        }
                        ////ChequeBook_Master
                        //MyBase.Execute("Delete from ChequeBook_Master where Company_Code = " + New_Comp_Code + " and Year_Code = '" + New_Year_Code + "'");
                        //MyBase.Execute_Tbl("Select * from ChequeBook_Master where company_Code = " + From_Comp_Code + " and year_Code = '" + Prv_Year_Code + "'", "T1");
                        //MyBase.Execute("Update t1 Set Company_Code = " + New_Comp_Code + ", year_Code = '" + New_Year_Code + "'");
                        //MyBase.Execute("Insert into ChequeBook_Master select * from t1");
                        ////Location_Master
                        if (MyBase.Get_RecordCount("Location_Master", "Company_Code = " + New_Comp_Code + " AND YEAR_cODE = '" + New_Year_Code + "'") == 0)
                        {
                            MyBase.Execute("Delete from Location_Master where Company_Code = " + New_Comp_Code + " and Year_Code = '" + New_Year_Code + "'");
                            MyBase.Execute_Tbl("Select * from Location_Master where company_Code = " + From_Comp_Code + " and year_Code = '" + Prv_Year_Code + "'", "T1");
                            MyBase.Execute("Update t1 Set Company_Code = " + New_Comp_Code + ", year_Code = '" + New_Year_Code + "'");
                            MyBase.Execute("Insert into Location_Master select * from t1");
                        }
                    }

                    //Balance Update
                    //MyBase.Current_Balance(0, Convert.ToDateTime("01-Apr-2010"), From_Comp_Code, Prv_Year_Code, true);
                    //MyBase.Execute(" update ledger_Master set ledger_ODebit = c1.Bal_Amount from ledger_Master l1 left join curBal c1 on l1.ledger_Code = c1.ledger_Code where l1.company_Code = " + New_Comp_Code + " and l1.year_Code = '" + New_Year_Code + "' and c1.Mode = 'Dr' and c1.Bal_Amount > 0 ");
                    //MyBase.Execute(" update ledger_Master set ledger_OCredit = c1.Bal_Amount from ledger_Master l1 left join curBal c1 on l1.ledger_Code = c1.ledger_Code where l1.company_Code = " + New_Comp_Code + " and l1.year_Code = '" + New_Year_Code + "' and c1.Mode = 'Cr' and c1.Bal_Amount > 0");

                    //Debtors
                    //MyBase.Execute("Insert into Ledger_Breakup select ledger_Code, 'LEDGER' as Term, 1 as Slno, Mode, refdoc, refdate, (case when Bal_Amount < 0 then (-1) * Bal_Amount else 0 end), (case when Bal_Amount > 0 then Bal_Amount else 0 end), 0, 4, 0, 0, 'L1', 2, '2011-2012', 0 from F_Out_Debit_Qry_Last where vcode is not null and bal_Amount is not null");

                    //Creditors
                    //MyBase.Execute("Insert into Ledger_Breakup select ledger_Code, 'LEDGER' as Term, 1 as Slno, Mode, refdoc, refdate, (case when Bal_Amount > 0 then Bal_Amount else 0 end), (case when Bal_Amount < 0 then (-1) * Bal_Amount else 0 end), 0, 4, 0, 0, 'L1', 2, '2011-2012', 0 from F_Out_Credit_Qry_Last where vcode is not null and bal_Amount is not null");


                    Year_Ending_Update_Ledger_heads(From_Comp_Code, Prv_Year_Code, New_Comp_Code, New_Year_Code);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Year_Carry_Over_New(int From_Comp_Code, String Prv_Year_Code, int New_Comp_Code, String New_Year_Code)
        {
            DataTable Dt;
            try
            {
                MyBase.Current_Balance(0, Convert.ToDateTime("01-Apr-" + Prv_Year_Code.Substring(0, 4)), From_Comp_Code, Prv_Year_Code, true);
                MyBase.Execute_Qry("select c1.ledger_Code, ledger_name, (case when Mode = 'Cr' then bal_amount else 0 end) Ocredit, (case when Mode = 'Dr' then bal_amount else 0 end) Odebit from curBal c1 left join ledger_master l1 on c1.ledger_Code = l1.ledger_Code where c1.ledger_Code > 0 and l1.company_Code = " + From_Comp_Code + " and l1.year_Code = '" + Prv_Year_Code + "' ", "CO_Ledger");
                MyBase.Execute("Exec Year_Carry_Over " + From_Comp_Code + ", '" + Prv_Year_Code + "', " + New_Comp_Code + ", '" + New_Year_Code + "'");

                Opening_Stock_updation_New(From_Comp_Code, Prv_Year_Code, New_Comp_Code, New_Year_Code);

                Dt = new DataTable();
                MyBase.Load_Data("Select Ledger_Code, ledger_odebit, ledger_Ocredit from Ledger_Master where company_Code = " + New_Comp_Code + " and year_Code = '" + New_Year_Code + "' order by ledger_Code", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyBase.Get_RecordCount("ledger_breakup", "ledger_Code = " + Dt.Rows[i]["ledger_Code"].ToString() + " and refDoc = 'OPN' and term = 'LEDGER' AND REF = 'L1' AND COMPANY_cODE = " + New_Comp_Code + " AND YEAR_cODE = '" + New_Year_Code + "' AND MODE = 'N'") > 0)
                    {
                        MyBase.Execute("update ledger_breakup set debit = " + Convert.ToDouble(Dt.Rows[i]["ledger_Odebit"]) + ", credit = " + Convert.ToDouble(Dt.Rows[i]["ledger_Ocredit"]) + " where ledger_Code = " + Dt.Rows[i]["ledger_Code"].ToString() + " and refDoc = 'OPN' and term = 'LEDGER' AND REF = 'L1' AND COMPANY_cODE = " + New_Comp_Code + " AND YEAR_cODE = '" + New_Year_Code + "' AND MODE = 'N'");
                    }
                    else
                    {
                        MyBase.Execute("insert into ledger_breakup select " + Dt.Rows[i]["ledger_Code"].ToString() + ", 'LEDGER', 1, 'N', 'OPN', '" + String.Format("{0:dd-MMM-yyyy}", SDate) + "', " + Dt.Rows[i]["ledger_Ocredit"].ToString() + ", " + Dt.Rows[i]["ledger_OCredit"].ToString() + ", 0, 4, 0, 0, 'L1', " + New_Comp_Code + ", '" + New_Year_Code + "', NULL ");
                    }
                }

                MyBase.Execute("Delete from ledger_breakup where company_Code = " + New_Comp_Code + " and year_Code = '" + New_Year_Code + "' and Mode = 'N' and term = 'LEDGER' AND REF = 'L1' AND DEBIT = 0 AND CREDIT = 0");

                MessageBox.Show("Completed ...!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void File_Menu_Settings()
        {
            try
            {
                fileMenu.Visible = true;
                for (int i = 0; i <= fileMenu.DropDownItems.Count - 1; i++)
                {
                    fileMenu.DropDownItems[i].Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Menu_Settings_New()
        {
            try
            {
                for (int i = 0; i <= menuStrip.Items.Count - 1;i++)
                {
                    if (menuStrip.Items[i] is System.Windows.Forms.ToolStripMenuItem)
                    {
                        ToolStripMenuItem Ct = (ToolStripMenuItem)menuStrip.Items[i];
                        for (int j = 0; j <= Ct.DropDownItems.Count - 1; j++)
                        {
                            if (Ct.DropDownItems[j] is System.Windows.Forms.ToolStripMenuItem)
                            {
                                ToolStripMenuItem Ct1 = (ToolStripMenuItem)Ct.DropDownItems[j];
                                for (int k = 0; k <= Ct1.DropDownItems.Count - 1; k++)
                                {
                                    if (Ct1.DropDownItems[k] is System.Windows.Forms.ToolStripMenuItem)
                                    {
                                        ToolStripMenuItem Ct2 = (ToolStripMenuItem)Ct1.DropDownItems[k];
                                        for (int l = 0; l <= Ct2.DropDownItems.Count - 1; l++)
                                        {
                                            if (Ct2.DropDownItems[l] is System.Windows.Forms.ToolStripMenuItem)
                                            {
                                                ToolStripMenuItem Ct3 = (ToolStripMenuItem)Ct2.DropDownItems[l];
                                                for (int m = 0; m <= Ct3.DropDownItems.Count - 1; m++)
                                                {
                                                    if (Ct3.DropDownItems[l] is System.Windows.Forms.ToolStripMenuItem)
                                                    {
                                                        ToolStripMenuItem Ct4 = (ToolStripMenuItem)Ct3.DropDownItems[l];
                                                        Ct4.Visible = Is_Menu_Visible(Ct4.Name);
                                                    }
                                                }
                                                Ct3.Visible = Is_Menu_Visible(Ct3.Name);
                                            }
                                        }
                                        Ct2.Visible = Is_Menu_Visible(Ct2.Name);
                                    }
                                }
                                Ct1.Visible = Is_Menu_Visible(Ct1.Name);
                            }
                        }
                        Ct.Visible = Is_Menu_Visible(Ct.Name);
                    }
                }
                File_Menu_Settings();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Is_Menu_Visible(String Name)
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("select * from Socks_Permission_Master where Menu_Code in (select Menu_Code from Menu_Master where Menu_CName = '" + Name + "') and User_Code = " + UserCode, ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        void Bill_Passing_Deletion(int Vcode, DateTime VDate)
        {
            try
            {
                MyBase.Execute(" Delete from ERP_Accounts_Stores where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Stores_New where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Stores_Return where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Stores_RGP where Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Stores_RGP_Return where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Stores where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Stores_Return where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Stores_RGP_Socks where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Stores_RGP_Socks_Return where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Stores_Details where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Stores_InHouse where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Stores_InHouse_Return where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Stores_InHouse_Socks where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Stores_InHouse_Socks_Return where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Sales where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Cotton where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Doub_Rec where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Dyed_Rec where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Fab_Rec where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Purchase_Return where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Sales_Broker where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Sales_Socks where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Sales_Socks_Broker where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Sales_SocksC where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Sales_SocksC_Broker where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Waste_Sales where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from ERP_Accounts_Yarn_Return where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from Voucher_Breakup_Bills_Deleted where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from Voucher_Details_Deleted where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and  Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                MyBase.Execute(" Delete from Voucher_Master_Deleted where  vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void voucher_Entry_Deletion()
        {
            DataTable Dt1 = new DataTable();
            try
            {
                Help_Text("Voucher Entry Updation ");
                MyBase.Load_Data("Select Vcode, Vdate, Vno, user_Date from voucher_master where user_Date < '01-JUL-2011' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", ref Dt1);
                Progress_visible(true);
                Progress_Max(Dt1.Rows.Count + 1);
                try
                {
                    for (int i=0;i<=Dt1.Rows.Count - 1;i++)
                    {
                        MyBase.Execute("Delete from voucher_Breakup_Bills where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and vcode = " + Dt1.Rows[i]["vcode"].ToString() + " and vdate = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[i]["vdate"])) + "'");
                        MyBase.Execute("Delete from voucher_Details where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and vcode = " + Dt1.Rows[i]["vcode"].ToString() + " and vdate = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[i]["vdate"])) + "'");
                        MyBase.Execute("Delete from voucher_Master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and vcode = " + Dt1.Rows[i]["vcode"].ToString() + " and vdate = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[i]["vdate"])) + "'");
                        MyBase.Execute("Delete from Cheque_Details where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and vcode = " + Dt1.Rows[i]["vcode"].ToString() + " and vdate = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt1.Rows[i]["vdate"])) + "'");

                        MyBase.Execute("Update Ledger_Breakup set Term = 'Ledger', Ref = 'L1', refDoc = 'OPN : ' + refDoc where company_Code = " + CompCode + " and Year_Code = '" + YearCode + "' and ref = '" + Dt1.Rows[i]["vcode"].ToString() + "'");

                        Bill_Passing_Deletion (Convert.ToInt32(Dt1.Rows[i]["vcode"]), Convert.ToDateTime(Dt1.Rows[i]["vdate"]));
                        AddProgress_Value(1);
                    }

                    Progress_visible(false);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Remove_Old_Company(int Compcode, String Year_Code)
        {
            try
            {
                MyBase.Execute (" Delete from ledger_Master where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ledger_Breakup where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from Ledger_Contact where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from Voucher_Master where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from voucher_Details where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from Voucher_Breakup_bills where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                //MyBase.Execute (" Delete from ChequeBook_Master where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from Cheque_Details where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from Cheque_Details_Deleted where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from Cheque_Cancel_Details where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from Closing_Stock where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Stores where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Stores_New where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Stores_Return where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Stores_RGP where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Stores_RGP_Return where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Stores where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Stores_Return where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Stores_RGP_Socks where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Stores_RGP_Socks_Return where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Stores_Details where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Stores_InHouse where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Stores_InHouse_Return where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Stores_InHouse_Socks where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Stores_InHouse_Socks_Return where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Sales where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Cotton where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Doub_Rec where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Dyed_Rec where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Fab_Rec where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Purchase_Return where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Sales_Broker where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Sales_Socks where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Sales_Socks_Broker where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Sales_SocksC where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Sales_SocksC_Broker where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Waste_Sales where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from ERP_Accounts_Yarn_Return where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from Voucher_Breakup_Bills_Deleted where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from Voucher_Details_Deleted where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute (" Delete from Voucher_Master_Deleted where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute(" Delete from Exported_Vouchers where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "'");
                MyBase.Execute(" Delete from Socks_Companymas where CompCode = " + Compcode + " and sdt = '01-Apr-2010'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Gainup_Process()
        {
            DataTable Dt = new DataTable();
            try
            {
                if (CompName.ToUpper().Contains("GAINUP"))
                {
                    if (MyBase.Check_Table("Gainup_Process_G"))
                    {
                        return;
                    }
                }


                if (CompName.ToUpper().Contains("ALAMELU"))
                {
                    if (MyBase.Check_Table("Gainup_Process_A"))
                    {
                        return;
                    }
                }

                MyBase.CurBal_Period(SDate, SDate, Convert.ToDateTime("30-SEP-2011"), CompCode, YearCode);

                // Ledger Opening Balance Updation
                Help_Text("ledger Updation ");
                MyBase.Load_Data ("Select * from Curbal_Period order by ledger_Code", ref Dt);
                Progress_visible(true);
                Progress_Max(Dt.Rows.Count + 1);
                for (int i=0;i<Dt.Rows.Count - 1;i++)
                {
                    if (Dt.Rows[i]["mode"].ToString().ToUpper() == "CR")
                    {
                        MyBase.Execute("Update ledger_Master set Ledger_ODebit = 0, Ledger_OCredit = " + Dt.Rows[i]["Bal_Amount"].ToString() + " where Company_Code = " + CompCode + " and Year_Code = '" + YearCode + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString());
                    }
                    else
                    {
                        MyBase.Execute("Update ledger_Master set Ledger_ODebit = " + Dt.Rows[i]["Bal_Amount"].ToString() + ", ledger_Ocredit = 0 where Company_Code = " + CompCode + " and Year_Code = '" + YearCode + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString());
                    }
                    AddProgress_Value(1);
                }
                Progress_visible(false);


                // Voucher Details updation

                voucher_Entry_Deletion();

                if (CompName.ToUpper().Contains("GAINUP"))
                {
                    MyBase.Execute("Create table Gainup_Process_G (no Tinyint)");
                }

                if (CompName.ToUpper().Contains("ALAMELU"))
                {
                    MyBase.Execute("Create table Gainup_Process_A (no Tinyint)");
                }

                Remove_Old_Company(1, "2010-2011");
                Remove_Old_Company(2, "2010-2011");

                MyBase.Execute("Truncate Table Exported_Vouchers");

            }
            catch (Exception ex)
            {
                Progress_visible(false);
                throw ex;
            }
        }

        void Avaneetha_Update_Vouchers()
        {
            DataTable Dt1 = new DataTable();
            DataTable ERP = new DataTable();
            try
            {
                MyBase.Load_Data("Select VCode from voucher_Master where user_Date >= '01-Apr-2011'", ref Dt1);
                for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Update voucher_Master set Year_Code = '2011-2012' where vcode = " + Dt1.Rows[i][0].ToString());
                    MyBase.Execute("Update voucher_Details set Year_Code = '2011-2012' where vcode = " + Dt1.Rows[i][0].ToString());
                    MyBase.Execute("Update voucher_Breakup_Bills set Year_Code = '2011-2012' where vcode = " + Dt1.Rows[i][0].ToString());
                    MyBase.Execute("Update Exported_Vouchers set Year_Code = '2011-2012' where vcode = " + Dt1.Rows[i][0].ToString());
                    MyBase.Execute("Update Cheque_Details set Year_Code = '2011-2012' where vcode = " + Dt1.Rows[i][0].ToString());


                    MyBase.Load_Data("select Distinct TABLE_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME like '%ERP_Acc%'  and COLUMN_NAME = 'Company_Code'", ref ERP);
                    for (int j = 0; j <= ERP.Rows.Count - 1; j++)
                    {
                        MyBase.Execute("Update " + ERP.Rows[j]["Table_Name"].ToString() + " set Year_Code = '2011-2012' where vcode = " + Dt1.Rows[i][0].ToString());
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Avaneetha_Followup_Master(String TblName)
        {
            try
            {
                MyBase.Execute_Tbl("Select * from " + TblName + " where company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", "Prc_Tbl");
                MyBase.Execute("Update Prc_Tbl set Year_Code = '2011-2012'");
                MyBase.Execute("Insert into " + TblName + " select * from prc_Tbl");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Avaneetha_Process()
        {
            try
            {
                //Avaneetha_Followup_Master("GroupMas");
                //Avaneetha_Followup_Master("Ledger_Master");
                //Avaneetha_Followup_Master("Ledger_Breakup");
                //Avaneetha_Followup_Master("Acc_Settings");

                //Avaneetha_Update_Vouchers();


                //MyBase.Execute("update ledger_Master set Ledger_ODebit = 0, Ledger_OCredit = 0 where YEAR_CODE = '2011-2012'");
                //MyBase.CurBal_Period(Convert.ToDateTime("01-Apr-2010"), Convert.ToDateTime("01-Apr-2010"), Convert.ToDateTime("31-Mar-2011"), Convert.ToInt32("1"), "2010-2011");

                //DataTable Dt = new DataTable();
                //MyBase.Load_Data("select * from CURBAL_PERIOD where Bal_Amount > 0 order by Ledger_Code", ref Dt);
                //for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                //{
                //    if (Dt.Rows[i]["Mode"].ToString().ToUpper() == "DR")
                //    {
                //        MyBase.Execute("update ledger_Master set Ledger_Odebit = " + Dt.Rows[i]["Bal_Amount"].ToString() + ", Ledger_Ocredit = 0 where company_Code = 1 and year_Code = '2011-2012' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString());
                //    }
                //    else
                //    {
                //        MyBase.Execute("update ledger_Master set Ledger_Ocredit = " + Dt.Rows[i]["Bal_Amount"].ToString() + ", Ledger_Odebit = 0 where company_Code = 1 and year_Code = '2011-2012' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString());
                //    }
                //}




            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime Get_last_Day_in_Month(DateTime Dat)
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("select datediff(D, '" + String.Format("{0:dd-MMM-yyyy}", Dat) + "','" + String.Format("{0:dd-MMM-yyyy}", Dat.AddMonths(1)) + "')", ref Dt);
                return Convert.ToDateTime(Dt.Rows[0][0].ToString() + "/" + Dat.Month + "/" + Dat.Year);
            }
            catch (Exception ex)
            {
                return Dat;
            }

        }

        void Fill_Multiple_Company_Code()
        {
            DataTable TmpDt = new DataTable();
            try
            {
                Stock_Settings();
                if (MyBase.IS_Multiple_Company())
                {
                    MyBase.Load_Data("Select datasource from Linked_Source order by datasource", ref TmpDt);
                    if (TmpDt.Rows.Count == 0)
                    {
                        Multiple_Company_Code = new int[1];
                        Multiple_Company_Address = new String[1];
                        Multiple_Company_Code[0] = CompCode;
                        Multiple_Company_Address[0] = "dbo";
                        CompCode_String = CompCode.ToString();
                        Company_Address_String = "dbo.";
                    }
                    else
                    {
                        Multiple_Company_Code = new int[TmpDt.Rows.Count];
                        Multiple_Company_Address = new String[TmpDt.Rows.Count];

                        for (int i = 0; i <= TmpDt.Rows.Count - 1; i++)
                        {
                            Multiple_Company_Code[i] = MyBase.Get_Company_Code_From_datasource(TmpDt.Rows[i]["datasource"].ToString());
                            Multiple_Company_Address[i] = MyBase.Get_Source_From_datasource(TmpDt.Rows[i]["datasource"].ToString());
                        }

                        CompCode_String = "~`!@#$%^&#*" + CompCode.ToString() + "%$^**^#@%@~*";
                        Company_Address_String = "~`!@#$%^&#*dbo.%$^**^#@%@~*";
                    }
                }
                else
                {
                    Multiple_Company_Code = new int[1];
                    Multiple_Company_Address = new String[1];
                    Multiple_Company_Code[0] = CompCode;
                    Multiple_Company_Address[0] = "dbo";
                    CompCode_String = CompCode.ToString();
                    Company_Address_String = "dbo.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ButtonEnabled_Cancel(Boolean Flag)
        {
            try
            {
                toolStripButton1.Enabled = Flag;
                toolStripMenuItem32.Enabled = Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Is_Duplicate_Company()
        {
            try
            {
                if (CompCode == 50)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        void Reminder()
        {
            String Str = String.Empty;
            try
            {
                if (UserName.ToUpper() == "2ADMIN")
                {
                    Str = "Advance Payment Approval : " + MyBase.RowCount("Select Count(*) from Adv_Payment_Approval(" + CompCode + ", '" + YearCode + "')");
                }
                else if (UserName.ToUpper().Contains("ARU2NAGIRI"))
                {
                    Str = "";
                }
                else
                {
                    Str = "Welcome to " + CompName + " " + YearCode;
                }

                StripLabel1.Text = MyBase.PadR(Str, 150);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Remove_Company_Year(String TblName)
        {
            try
            {
                if (MyBase.Check_TableField(TblName, "Company_Code"))
                {
                    MyBase.Execute("Alter table " + TblName + " Drop column Company_Code");
                }

                if (MyBase.Check_TableField(TblName, "Year_Code"))
                {
                    MyBase.Execute("Alter table " + TblName + " Drop column Year_Code");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MDIMain_Load(object sender, EventArgs e)
        {
            try
            {

                if (File.Exists(Application.StartupPath + "\\$Vaahini$.jpg"))
                {
                    Image Im = Image.FromFile(Application.StartupPath + "\\$Vaahini$.jpg");
                    this.BackgroundImage = Im;
                }
                //Select_Company();

                if (UserName.ToUpper() == "ADMIN")
                {
                    User_Datelock = -100;
                }
                else
                {
                    User_Datelock = -3;
                }

                Company_Initialization();

                Complaint_Details();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void VAT_Updation()
        {
            String Str = String.Empty;
            try
            {
                if (CompName.ToUpper().Contains("RAJARAM"))
                {
                    MyBase.Execute("update voucher_master set vat_Account_date = invoice_date where invoice_No <> '' and vat_Account_date < invoice_date");
                    MyBase.Execute("update voucher_master set vat_type = 'Y', vat_Account_Date = invoice_Date where vcode in (select vcode from voucher_Details where ledger_Code = 3503 and company_Code = " + CompCode + ") and vat_type = '' and company_Code = " + CompCode); 
                    MyBase.Execute("update voucher_master set vat_type = 'Y', vat_Account_Date = invoice_Date where vcode in (select vcode from voucher_Details where ledger_Code = 3664 and company_Code = " + CompCode + ") and vat_type = '' and company_Code = " + CompCode);
                    Str = " select v1.vcode, v1.vdate, v1.company_Code, v1.year_Code, sum(v2.debit) Net_Amount, 0 as Tax_Amount from voucher_Master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.vmode = 6 and vat_type = 'Y' and v1.company_Code = " + CompCode + " and v1.year_Code = '"+ YearCode +"' group by v1.vcode, v1.vdate, v1.company_Code, v1.year_Code union ";
                    Str += " select v1.vcode, v1.vdate, v1.company_Code, v1.year_Code, 0 Net_Amount, sum(v2.debit) Tax_Amount from voucher_Master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.vmode = 6 and vat_type = 'Y' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' and v2.ledger_Code in (Select ledger_Code from ledger_master where ledger_group_code in (select groupcode from groupmas where groupreserved = 4300 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') group by v1.vcode, v1.vdate, v1.company_Code, v1.year_Code ";
                    MyBase.Execute_Qry(Str, "Qry_Vat_Purchase1");
                    MyBase.Execute_Qry("Select Vcode, vdate, company_Code, year_Code, sum(Net_Amount) - sum(Tax_Amount) Gross_Amount, sum(Tax_Amount) TAx_Amount, sum(Net_Amount) Net_Amount from Qry_Vat_Purchase1 group by Vcode, vdate, company_Code, year_Code", "Qry_Vat_Purchase");

                    Str = " select v1.vcode, v1.vdate, v1.company_Code, v1.year_Code, sum(v2.Credit) Net_Amount, 0 as Tax_Amount from voucher_Master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.vmode = 5 and v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' and v2.ledger_Code in (Select ledger_Code from ledger_Master where COMPANY_cODE = " + CompCode + " AND YEAR_cODE = '" + YearCode + "' and ledger_group_Code in (select groupcode from groupmas where groupreserved = 4400 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "')) group by v1.vcode, v1.vdate, v1.company_Code, v1.year_Code union ";
                    Str += " select v1.vcode, v1.vdate, v1.company_Code, v1.year_Code, 0 Net_Amount, sum(v2.Credit) Tax_Amount from voucher_Master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.vmode = 5 and v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' and v2.ledger_Code in (Select ledger_Code from ledger_master where ((ledger_Name like '%VAT%') or (Ledger_Name like '%CST%')) and ledger_group_code in (select groupcode from groupmas where groupreserved = 4300 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') group by v1.vcode, v1.vdate, v1.company_Code, v1.year_Code ";
                    MyBase.Execute_Qry(Str, "Qry_Vat_Sales1");
                    MyBase.Execute_Qry("Select Vcode, vdate, company_Code, year_Code, sum(Net_Amount) - sum(Tax_Amount) Gross_Amount, sum(Tax_Amount) TAx_Amount, sum(Net_Amount) Net_Amount from Qry_Vat_Sales1 group by Vcode, vdate, company_Code, year_Code", "Qry_Vat_Sales");
                }
                else
                {
                    MyBase.Execute("update voucher_master set vat_Account_date = invoice_date where invoice_No <> '' and vat_Account_date < invoice_date");
                    MyBase.Execute("update voucher_master set vat_type = 'Y', vat_Account_Date = invoice_Date where vcode in (select vcode from voucher_Details where ledger_Code = 3503 and company_Code = " + CompCode + ") and vat_type = '' and company_Code = " + CompCode);
                    MyBase.Execute("update voucher_master set vat_type = 'Y', vat_Account_Date = invoice_Date where vcode in (select vcode from voucher_Details where ledger_Code = 3664 and company_Code = " + CompCode + ") and vat_type = '' and company_Code = " + CompCode);

                    Str = " select v1.vcode, v1.vdate, v1.company_Code, v1.year_Code, ISNULL(v2.Assessible_Value, 0) Net_Amount, 0 as Tax_Amount from voucher_Master v1 left join voucher_Update_Purchase_Assesible v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.vmode = 6 and vat_type = 'Y' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' union ";
                    Str += " select v1.vcode, v1.vdate, v1.company_Code, v1.year_Code, 0 Net_Amount, ISNULL(v2.Tax_Value, 0) Tax_Amount from voucher_Master v1 left join voucher_Update_Purchase_Assesible v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.vmode = 6 and vat_type = 'Y' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' ";

                    MyBase.Execute_Qry(Str, "Qry_Vat_Purchase1");
                    MyBase.Execute_Qry("Select Vcode, Vdate, company_Code, year_Code, Net_Amount Gross_Amount, Tax_Amount TAx_Amount, Net_Amount + Tax_Amount Net_Amount from Qry_Vat_Purchase1 ", "Qry_Vat_Purchase");

                    Str = " select v1.vcode, V1.Vdate, v1.company_Code, v1.year_Code, sum(v2.Credit) Net_Amount, 0 as Tax_Amount from voucher_Master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.vmode = 5 and v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' and v2.ledger_Code in (Select ledger_Code from ledger_Master where COMPANY_cODE = " + CompCode + " AND YEAR_cODE = '" + YearCode + "' and ledger_group_Code in (select groupcode from groupmas where groupreserved = 4400 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "')) group by v1.vcode, v1.vdate, v1.company_Code, v1.year_Code union ";
                    Str += " select v1.vcode, V1.Vdate, v1.company_Code, v1.year_Code, 0 Net_Amount, sum(v2.Credit) Tax_Amount from voucher_Master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.vmode = 5 and v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' and v2.ledger_Code in (Select ledger_Code from ledger_master where ((ledger_Name like '%VAT%') or (Ledger_Name like '%CST%')) and ledger_group_code in (select groupcode from groupmas where groupreserved = 4300 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') group by v1.vcode, v1.vdate, v1.company_Code, v1.year_Code ";
                    MyBase.Execute_Qry(Str, "Qry_Vat_Sales1");

                    MyBase.Execute_Qry("Select Vcode, Vdate, Company_Code, Year_Code, Sum(Net_Amount) - Sum(Tax_Amount) Gross_Amount, Sum(Tax_Amount) Tax_Amount, Sum(Net_Amount) Net_Amount From Qry_Vat_Sales1 Group By Vcode, Vdate, Company_Code, Year_Code", "Qry_Vat_Sales");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Voucher_Type_Updation()
        {
            try
            {
                if (CompName.ToUpper().Contains("AVANEETHA"))
                {
                    MyBase.Execute("update Voucher_Master set VType = 'Sales' where VMode = 4 and VType <> 'Others' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("Voucher_masteR", "Company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and Vmode = 5 and vtype <> 'Sales' and Vtype <> 'Waste' and vtype <> 'Others'") > 0)
                {
                    MyBase.Execute("Update Voucher_Master set Vtype = 'Sales' where vmode = 5 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and vtype <> 'Sales' and Vtype <> 'Waste' and vtype <> 'Others'");
                }
                if (MyBase.Get_RecordCount("Voucher_masteR", "Company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and Vmode = 6 and vtype <> 'Stores' and Vtype <> 'Cotton' and vtype <> 'Others'") > 0)
                {
                    MyBase.Execute("Update Voucher_Master set Vtype = 'Stores' where vmode = 6 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and vtype <> 'Stores' and Vtype <> 'Cotton' and vtype <> 'Others'");
                }
                if (MyBase.Get_RecordCount("Voucher_masteR", "Company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and Vmode = 7 and vtype <> 'Stores' and Vtype <> 'Cotton' and vtype <> 'Others'") > 0)
                {
                    MyBase.Execute("Update Voucher_Master set Vtype = 'Stores' where vmode = 7 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and vtype <> 'Stores' and Vtype <> 'Cotton' and vtype <> 'Others'");
                }
                if (MyBase.Get_RecordCount("Voucher_Master", "Company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and upper(vtype) Not in ('STORES', 'SALES', 'BANK', 'CASH', 'COTTON', 'OTHERS')") > 0)
                {
                    MyBase.Execute("update voucher_master set Vtype = 'Others' where upper(vtype) Not in ('STORES', 'SALES', 'BANK', 'CASH', 'COTTON', 'OTHERS')");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Import_New_Ledgers()
        {
            DataTable TempDt = new DataTable();
            try
            {
                if (CompName.ToUpper().Contains("GAINUP") || CompName.ToUpper().Contains("ALAMELU"))
                {
                }
                else
                {
                    if (MyBase.Check_Table("CarryOver_Details"))
                    {
                        // New Company
                        MyBase.Load_Data("Select * from  CarryOver_Details where To_Company_Code = " + CompCode + " and To_Year_Code = '" + YearCode + "'", ref TempDt);
                        if (TempDt.Rows.Count > 0)
                        {
                            MyBase.Execute("Exec Carry_Over_Balance " + TempDt.Rows[0]["From_Company_Code"].ToString() + ", '" + TempDt.Rows[0]["From_Year_Code"].ToString() + "', " + TempDt.Rows[0]["To_Company_Code"].ToString() + ", '" + TempDt.Rows[0]["To_Year_Code"].ToString() + "'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Import_New_Ledger_Fresh()
        {
            DataTable TempDt = new DataTable();
            try
            {
                // Old Company
                MyBase.Load_Data("Select * from  CarryOver_Details where From_Company_Code = " + CompCode + " and From_Year_Code = '" + YearCode + "'", ref TempDt);
                if (TempDt.Rows.Count > 0)
                {
                    if (MyBase.GetServerDate() >= Convert.ToDateTime("01-Apr-" + TempDt.Rows[0]["To_Year_Code"].ToString().Substring(0, 4)))
                    {
                        Carry_Overed = true;
                        if (MyBase.Check_Table("Imp_ledger"))
                        {
                            MyBase.Execute("Drop table imp_ledger");
                        }
                        MyBase.Execute("Exec Import_New_Ledgers " + TempDt.Rows[0]["To_Company_Code"].ToString() + ", '" + TempDt.Rows[0]["To_Year_Code"].ToString() + "', " + TempDt.Rows[0]["From_Company_Code"].ToString() + ", '" + TempDt.Rows[0]["From_Year_Code"].ToString() + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Is_TDS_Available_For_Voucher_Mode(int vmode, int Ledger_Code)
        {
            String Vchmode = String.Empty;
            try
            {
                Vchmode = MyBase.GetData_InString("Voucher_Type", "VchTYpeNo", vmode.ToString(), "VchtypeName");
                if (MyBase.Get_RecordCount("Ledger_Master", "ledger_Code = " + Ledger_Code + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and tdsapplicable = 'Y'") > 0)
                {
                    TDS_Deduct_ON = MyBase.GetData_InStringWC ("Ledger_Master", "Ledger_Code", Ledger_Code.ToString(), "tds_deduct_On", CompCode, YearCode).ToUpper();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public Boolean Is_TDS_Available_For_Voucher_Mode_Old(int vmode)
        {
            try
            {
                if (MyBase.Get_RecordCount("TDS_Table", "voucher_Mode = " + vmode) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public void Basic_Refresh_Old(Boolean Must)
        {
            try
            {
                if (MyBase.Check_Table("Voucher_Details"))
                {
                    Progress_visible(true);
                    Progress_Max(100);
                    MyBase.Add_NewField("Voucher_Details", "Rev_LedCode", "int");
                    //MyBase.Execute_Qry("Select Vcode, Vdate, Min(Slno) Slno, Byto, Rev_ledcode, company_Code, year_Code from Voucher_details where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' group By vcode, vdate, Byto, Rev_ledcode, company_Code, year_Code ", "Voucher_Slno");
                    if (MyBase.Check_Table("Ledger_Master"))
                    {
                        MyBase.Execute_Qry("Select v1.Vcode, v1.Vdate, Min(v1.Slno) Slno, v1.Byto, v1.company_Code, v1.year_Code from Voucher_details v1 where v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' group By v1.vcode, v1.vdate, v1.Byto, v1.company_Code, v1.year_Code ", "Voucher_Slno1");
                        Str = "select v1.*, v2.Ledger_Code, v2.Rev_ledCode, l1.ledger_Name Ledger, l2.ledger_Name Rev_ledger from voucher_slno1 v1, voucher_Details v2, ledger_master l1, ledger_master l2  where v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code and v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.byto = v2.byto and v1.slno = v2.slno and l1.company_Code = v1.company_Code and l1.year_Code = v1.year_Code and l1.ledger_Code = v2.ledger_Code and l2.company_Code = v1.company_Code and l2.year_Code = v1.year_Code and l2.ledger_Code = v2.Rev_LedCode";
                        MyBase.Execute_Qry(Str, "Voucher_Slno");
                    }
                    AddProgress_Value(20);
                    if (Have_To_refresh() || Must == true)
                    {
                        MyBase.Current_Balance(0, SDate, CompCode, YearCode, true);
                        MyBase.Execute_Qry("select v1.vcode, v1.Ledger_Code, v1.vdate, v2.user_date, v1.company_Code, v1.year_Code from voucher_Details v1, voucher_master v2 where v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code and v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' ", "Noof_Voucher");
                        MyBase.Execute_Tbl("Select l1.Ledger_Name Ledger, (case when count(n1.vcode) = 0 then null else count(n1.vcode) end) Vouchers, (case when Mode = 'Dr' then (case when C1.Bal_Amount = 0 then null else C1.Bal_Amount end) else null end) Debit, (case when Mode = 'Cr' then (case when C1.Bal_Amount = 0 then null else C1.Bal_Amount end) else null end) Credit, l1.Ledger_Code Code from Ledger_Master l1 left join CurBal C1 on l1.Ledger_Code = c1.Ledger_Code left join noof_voucher n1 on n1.ledger_Code = l1.ledger_Code and n1.company_Code = l1.company_Code and n1.year_Code = l1.year_Code where ((l1.link_status = 'Y' or l1.link_Status is null)) and l1.Company_Code = " + CompCode + " and l1.Year_Code = '" + YearCode + "' group by l1.Ledger_Name, c1.mode, c1.bal_amount, l1.ledger_Code ", "Ledger_View");
                        AddProgress_Value(20);
                        MyBase.Execute_Qry("select c1.*, l1.ledger_group_Code from curbal c1 left join ledger_master l1 on c1.ledger_Code = l1.Ledger_Code where l1.company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", "Tbl_Gr_Bal");
                        MyBase.Execute_Qry("select ledger_group_Code, Sum(Bal_amount) as Debit, 0 as Credit from Tbl_Gr_Bal where Mode = 'Dr' group By ledger_group_Code union select ledger_group_Code, 0 as Debit, Sum(Bal_amount) as Credit from Tbl_Gr_Bal where Mode = 'Cr' group By ledger_group_Code", "Tbl_Gr_Bal1");
                        AddProgress_Value(20);
                        MyBase.Execute_Qry("Select ledger_group_Code, (case when debit = 0 then null else debit end) debit, (case when Credit = 0 then null else Credit end) Credit from tbl_Gr_Bal1", "tbl_Gr_Bal2");
                        MyBase.Execute_Tbl("Select GroupCode, GroupName from GroupMas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", "GroupMas_Comp");
                        AddProgress_Value(20);
                        MyBase.Execute_Qry("select top 1000000000 g1.GroupName Group_, g1.GroupCode Code, (case when isnull(sum(t1.Debit),0) > isnull(sum(t1.Credit), 0) then isnull(sum(t1.debit), 0) - isnull(sum(t1.credit), 0) else null end) Debit, (case when isnull(sum(t1.credit),0) > isnull(sum(t1.debit), 0) then isnull(sum(t1.Credit), 0) - isnull(sum(t1.debit), 0) else null end) Credit from GroupMas_Comp g1 left join Tbl_Gr_Bal2 t1 on g1.GroupCode = t1.ledger_group_Code group by g1.GroupName, g1.GroupCode order by g1.GroupName", "Group_View1");
                        MyBase.Execute_Tbl("Select * from group_View1", "Group_View");
                        AddProgress_Value(20);
                    }
                    Progress_visible(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        


        void LEdger_Matching_Update()
        {
            DataTable Dt = new DataTable();
            try
            {
                if (CompName.ToUpper().Contains("GAINUP") || CompName.ToUpper().Contains("ALAMELU"))
                {
                    MyBase.Load_Data("select Distinct Subledger_Code, Ledger_Code, COmpany_Code from LEdger_Matching", ref Dt);
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        MyBase.Execute("UPdate voucher_Details set Ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " where LEdger_Code = " + Dt.Rows[i]["SubLedger_Code"].ToString() + " and Company_Code = " + Dt.Rows[i]["Company_Code"].ToString());
                        MyBase.Execute("UPdate voucher_Details set Rev_LedCode = " + Dt.Rows[i]["Ledger_Code"].ToString() + " where Rev_LedCode = " + Dt.Rows[i]["SubLedger_Code"].ToString() + " and Company_Code = " + Dt.Rows[i]["Company_Code"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CReport_PrintOnly(ref CrystalDecisions.CrystalReports.Engine.ReportDocument Rpt, String Caption)
        {
            try
            {
                if (this.ActiveMdiChild.Name == "FrmGSNEntry" || this.ActiveMdiChild.Name == "FrmTransportCopy" || this.ActiveMdiChild.Name == "FrmSupplierReturn" || this.ActiveMdiChild.Name == "FrmPORTransportCopy")
                {
                    ButtonEnabled(false);
                    ButtonEnabled_View(true);
                }
                FrmCRViewer Frm = new FrmCRViewer();
                Frm.Text = Caption;
                Frm.MdiParent = this;
                Frm.LoadCR_Print(ref Rpt);
                Frm.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Sales_Update_new()
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("select * from vaahini_erp_Aegan.dbo.invoicemas where yearCode = 2010 order by invoiceDt", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Update vaahini_erp_Aegan.dbo.invoicemas set Remark1 = '.' where invoiceno = '" + Dt.Rows[i]["invoiceno"].ToString() + "' and yearCode = 2010 and CompCode = " + Dt.Rows[i]["compcode"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void WSales_Update_new()
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("select * from vaahini_erp_Aegan.dbo.It_wasMas order by invoiceDt", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Update vaahini_erp_Aegan.dbo.It_wasmas set Remarks = '.' where invoiceno = '" + Dt.Rows[i]["invoiceno"].ToString() + "'");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Clot_Update_new()
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("select * from vaahini_erp_Aegan.dbo.Lotmaster order by lotno1", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Update vaahini_erp_Aegan.dbo.LotMaster set Remarksin = '' where Lotno = '" + Dt.Rows[i]["Lotno"].ToString() + "'");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        int Return_LastSpace(String Str, int LastIndex)
        {
            try
            {
                if (Str.Contains(" "))
                {
                    for (int i = LastIndex; i >= 0; i--)
                    {
                        if (Str.Substring(i, 1) == " ")
                        {
                            return i;
                        }
                    }
                    return 0;
                }
                else
                {
                    return 50;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void Cheque(String Ledger, String Rupees, double Amount, Boolean ACPayee, String Bank, DateTime ChqDate, Boolean Seal)
        {
            String Led1 = String.Empty;
            String Led2 = String.Empty;
            String Rp1 = String.Empty;
            bool RptLoaded = false;
            String Rp2 = String.Empty;
            try
            {
                if (Ledger.Length > 65)
                {
                    if (Bank.ToUpper().Contains("AXIS"))
                    {
                        Led1 = Ledger.Substring(0, Return_LastSpace(Ledger, 75));
                        Led2 = Ledger.Replace(Ledger.Substring(0, Return_LastSpace(Ledger, 75)), String.Empty);
                    }
                    else if (Bank.ToUpper().Contains("BANK OF INDIA"))
                    {
                        Led1 = Ledger.Substring(0, Return_LastSpace(Ledger, 75));
                        Led2 = Ledger.Replace(Ledger.Substring(0, Return_LastSpace(Ledger, 75)), String.Empty);
                    }
                    else
                    {
                        Led1 = Ledger.Substring(0, Return_LastSpace(Ledger, 65));
                        Led2 = Ledger.Replace(Ledger.Substring(0, Return_LastSpace(Ledger, 65)), String.Empty);
                    }
                }
                else
                {
                    Led1 = Ledger;
                    Led2 = string.Empty;
                }
                if (Rupees.Length > 50)
                {
                    Rp1 = Rupees.Substring(0, Return_LastSpace(Rupees, 50));
                    Rp2 = Rupees.Replace(Rupees.Substring(0, Return_LastSpace(Rupees, 50)), String.Empty);
                }
                else
                {
                    Rp1 = Rupees;
                    Rp2 = String.Empty;
                }


                CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                if (Bank.ToUpper().Contains("SBI"))
                {
                    if (CompName.ToUpper().Contains("AVANEETHA"))
                    {
                        ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\SBI.rpt");
                        RptLoaded = true;
                    }
                    else
                    {
                        ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\SB1.rpt");
                        RptLoaded = true;
                    }
                }
                else if (Bank.ToUpper().Contains("AXIS"))
                {
                    ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\AXIS.rpt");
                    RptLoaded = true;
                }
                else if (Bank.ToUpper().Contains("CANARA"))
                {
                    ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\CANARA.rpt");
                    RptLoaded = true;
                }
                else if (Bank.ToUpper().Contains("BANK OF INDIA"))
                {
                    if (Bank.ToUpper().Contains("C/A"))
                    {
                        ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\BANKOFINDIACA.rpt");
                        RptLoaded = true;
                    }
                    else
                    {
                        ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\BANKOFINDIACC.rpt");
                        RptLoaded = true;
                    }
                }
                else if (Bank.ToUpper().Contains("BOM - CC"))
                {
                    if (CompName.ToUpper().Contains("AVANEETHA"))
                    {
                        if (MessageBox.Show("Is Seal Enabled ?", "Vaahini", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\MAHARASHTRA_seal.rpt");
                            FormulaFill(ref ObjRpt, "acno", MyBase.PadR("A/c. No: 20056806266", 50));
                            RptLoaded = true;
                        }
                        else
                        {
                            ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\MAHARASHTRA.rpt");
                            RptLoaded = true;
                        }
                    }
                }
                else if (Bank.ToUpper().Contains("TMB"))
                {
                    ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\TMB.rpt");
                    RptLoaded = true;
                }
                else
                {
                    ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\SB1.rpt");
                    RptLoaded = true;
                }

                if (RptLoaded)
                {
                    if (ACPayee)
                    {
                        FormulaFill(ref ObjRpt, "ACPAYEE", "A/C PAYEE");
                    }
                    else
                    {
                        FormulaFill(ref ObjRpt, "ACPAYEE", String.Empty);
                    }
                    FormulaFill(ref ObjRpt, "date", String.Format("{0:dd/MM/yyyy}", ChqDate));
                    FormulaFill(ref ObjRpt, "ledger1", MyBase.PadR(Led1, 65));
                    FormulaFill(ref ObjRpt, "ledger2", MyBase.PadR(Led2, 55));
                    FormulaFill(ref ObjRpt, "RUPEE1", MyBase.PadR(Rp1, 50));
                    FormulaFill(ref ObjRpt, "RUPEE2", MyBase.PadR(Rp2, 55));
                    FormulaFill(ref ObjRpt, "RUPEES", "**" + string.Format("{0:n}", Convert.ToDouble(Amount)));
                    if (Seal)
                    {
                        FormulaFill(ref ObjRpt, "seal", MyBase.PadL("For " + CompName, 42));
                        FormulaFill(ref ObjRpt, "partner", MyBase.PadL("Managing Director", 42));
                    }
                    else
                    {
                        FormulaFill(ref ObjRpt, "seal", String.Empty);
                        FormulaFill(ref ObjRpt, "partner", String.Empty);
                    }
                    if (CompName.ToUpper().Contains("AVANEETHA"))
                    {
                        CReport(ref ObjRpt, "Cheque Print");
                    }
                    else
                    {
                        CReport(ref ObjRpt, "Cheque Print");
                    }
                }
                else
                {
                    MessageBox.Show("Cheque Design Not Available ...!", "Vaahini");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Cash_Book(int Ledger_Code)
        {
            try
            {
                //Str = "select cast('" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "' as datetime) as Vdate, 0 as Vslno, null vno, null Mode, 'Op. Bal' as Ledger, NUll Slno, (case when Ledger_Odebit > Ledger_OCredit then 0 else Ledger_Ocredit end) Payment, (case when Ledger_Odebit > Ledger_OCredit then ledger_Odebit else 0 end) Receipt, '-' Narration from ledger_master where company_code = " + CompCode + " and ledger_Code = " + Ledger_Code + " union ";
                //Str = "select vdate, 2 as vslno, null vno, cast(null as varchar(2)) mode, cast(null as varchar(500)) ledger, null slno, sum(payment) Payment, sum(receipt) receipt, '-' narration from cash_ledger_view where ledger_Code = " + Ledger_Code + " group by vdate ";
                //MyBase.Execute_Qry(Str, "Cash1");

                //Str += "select vdate, 2 as vslno, null vno, cast(null as varchar(2)) mode, cast(null as varchar(500)) ledger, null slno, sum(payment) Payment, sum(receipt) receipt, '-' narration from cash_ledger_view where ledger_Code = " + Ledger_Code + " group by vdate ";

                //Str = "select vdate, 1 as vslno, cast(vno as numeric(10)) vno, cast(mode as varchar(3)) Mode, ledger, slno, payment, receipt, narration from cash_ledger_view where ledger_code = " + Ledger_Code;
                //MyBase.Execute_Tbl(Str, "Cash2");

                //MyBase.Execute_Qry("select vdate, 2 vslno, null vno, null mode, null slno, (case when sum(payment) - sum(receipt) > 0 then sum(payment) - sum(receipt) else sum(payment) - sum(receipt) end) Amount from cash1 group by vdate", "Cash4");

                //MyBase.Execute_Tbl("select v4.vdate, v4.vslno, v4.vno, v4.mode, v4.slno, v4.payment, v4.receipt, sum(isnull(v5.payment, 0)) - sum(isnull(v5.receipt, 0)) as Amount from Cash1 v4 left join Cash1 v5 on v5.vdate < v4.vdate group by v4.vdate, v4.vslno, v4.vno, v4.mode, v4.slno, v4.payment, v4.receipt", "vlast");

                //MyBase.Execute_Qry("select Vdate, 0 as vslno, vno, cast(mode as Varchar(2)) Mode, '" + MyBase.PadR("Cl. Bal", 35) + "' as Ledger, slno, (case when amount > 0 then amount else null end) Payment, (case when amount < 0 then amount * (-1) else null end) Receipt, '-' Narration from vlast union select * from Cash2 union select * from Cash1 ", "CFinal");

                //MyBase.Execute_Tbl("select c1.vdate, c1.vslno, c1.vno, c1.mode, c1.ledger, c1.slno, c1.payment, c1.receipt, c1.narration from cfinal c1 ", "Cfinal1");

                //MyBase.Execute_Tbl("select * from final1 where vslno =0", "Cfinal2");

                //MyBase.Execute("update Cfinal1 set receipt = isnull(f.receipt, 0) + isnull(f1.receipt, 0) from Cfinal1 f, Cfinal2 f1 where f.vdate = f1.vdate and f.vslno = 2 ");

                //MyBase.Execute("update Cfinal1 set payment = isnull(f.payment, 0) + isnull(f1.payment, 0) from Cfinal1 f, Cfinal2 f1 where f.vdate = f1.vdate and f.vslno = 2 ");

                //MyBase.Execute("delete from cfinal1 where vdate = '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Menu Events
        private void ShowNewForm(object sender, EventArgs e)
        {
            try
            {
                Load_NewEntry();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void DateValidations()
        {
            DataTable Dt;
            try
            {
                // Check is it Right
                // select count(*) from cashbill_details c1, cashbill_master c2 where c1.cashbill_Slno = c2.cashbill_Slno and c1.year_Code = c2.year_Code and c1.cashbill_Date <> c2.cashbill_date

                if (MyBase.Check_TableField("Cashbill_Details", "Cashbill_Date") == false)
                {
                    MyBase.Add_NewField("Cashbill_Details", "Cashbill_Date", "Date");
                    Dt = new DataTable();
                    MyBase.Load_Data("Select Distinct Cashbill_Slno, Cashbill_Date, Year_Code from cashbill_Master", ref Dt);
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        MyBase.Execute("Update Cashbill_details set cashbill_Date = '" + String.Format("{0:dd-MMM-yyyy}", Dt.Rows[i]["cashBill_Date"]) + "' where cashbill_Slno = " + Dt.Rows[i]["cashBill_Slno"].ToString() + " and year_Code = '" + Dt.Rows[i]["Year_Code"].ToString() + "'");
                    }
                }
                if (MyBase.Check_TableField("SalesReturn_Details", "Sr_Date") == false)
                {
                    MyBase.Add_NewField("SalesReturn_Details", "SR_Date", "Date");
                    Dt = new DataTable();
                    MyBase.Load_Data("Select Distinct SR_Slno, SR_Date, Year_Code from SalesReturn_Master", ref Dt);
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        MyBase.Execute("Update SalesReturn_details set SR_Date = '" + String.Format("{0:dd-MMM-yyyy}", Dt.Rows[i]["SR_Date"]) + "' where SR_Slno = " + Dt.Rows[i]["SR_Slno"].ToString() + " and year_Code = '" + Dt.Rows[i]["Year_Code"].ToString() + "'");
                    }
                }
                if (MyBase.Check_TableField("Dc_Details", "DC_Date") == false)
                {
                    MyBase.Add_NewField("DC_Details", "DC_Date", "Date");
                    Dt = new DataTable();
                    MyBase.Load_Data("Select Distinct DC_Slno, DC_Date, Year_Code from DC_Master", ref Dt);
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        MyBase.Execute("Update DC_details set DC_Date = '" + String.Format("{0:dd-MMM-yyyy}", Dt.Rows[i]["DC_Date"]) + "' where DC_Slno = " + Dt.Rows[i]["DC_Slno"].ToString() + " and year_Code = '" + Dt.Rows[i]["Year_Code"].ToString() + "'");
                    }
                }
                if (MyBase.Check_TableField("DcReturn_Details", "DCR_Date") == false)
                {
                    MyBase.Add_NewField("DCReturn_Details", "DCR_Date", "Date");
                    Dt = new DataTable();
                    MyBase.Load_Data("Select Distinct DCR_Slno, DCR_Date, Year_Code from DCReturn_Master", ref Dt);
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        MyBase.Execute("Update DCReturn_details set DCR_Date = '" + String.Format("{0:dd-MMM-yyyy}", Dt.Rows[i]["DCR_Date"]) + "' where DCR_Slno = " + Dt.Rows[i]["DCR_Slno"].ToString() + " and year_Code = '" + Dt.Rows[i]["Year_Code"].ToString() + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void StateMaster()
        {
            try
            {
                if (MyBase.Check_Table("State_Master") == false)
                {
                    MyBase.Execute("Create Table State_Master (State_Code number(3), State_Name varchar2(50))");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OpenFile(object sender, EventArgs e)
        {
            try
            {
                Load_EditEntry();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void UpdatePCI()
        {
            try
            {
                if (MyBase.Get_RecordCount("GSN_Details", "PCI =0") > 0)
                {
                    MyBase.Execute("update GSN_Details set PCI = 1 where PCI = 0");
                }
                if (MyBase.Get_RecordCount("GSN_Acceptance_Details", "PCI =0") > 0)
                {
                    MyBase.Execute("update GSN_Acceptance_Details set PCI = 1 where PCI = 0");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
                // TODO: Add code here to save the current contents of the form to a file.
            }
        }

        Boolean Sign_Out()
        {
            try
            {
                if (MessageBox.Show("Sure to Sign Out ?", "Sign Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        void Year()
        {
            try
            {
                if (MyBase.Check_Table("Year_Master") == false)
                {
                    MyBase.Execute("Create Table Year_Master (Year varchar(10))");
                }
                if (Convert.ToInt32(String.Format("{0:MM}", DateTime.Now)) < 4)
                {
                    YearCode = Convert.ToString(Convert.ToInt32(String.Format("{0:yyyy}", DateTime.Now)) - 1) + "-" + String.Format("{0:yyyy}", DateTime.Now);
                }
                else
                {
                    YearCode = String.Format("{0:yyyy}", DateTime.Now) + "-" + Convert.ToString(Convert.ToInt32(String.Format("{0:yyyy}", DateTime.Now)) + 1);
                }
                YearCode = "2009-2010";
                if (MyBase.Get_RecordCount("Year_master", "Year = '" + YearCode + "'") == 0)
                {
                    MyBase.Execute("Insert into Year_Master values ('" + YearCode + "')");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void CapsCheckup()
        {
            try
            {
                MyBase.Execute("Update Cashier_Bankdetails set Status = 'Card' where Status = 'CARD'");
                MyBase.Execute("Update Cashier_Bankdetails set Status = 'Cheque' where Status = 'CHEQUE'");
                MyBase.Execute("Update Cashbill_Details set Sale_Return = 'Cancel' where Sale_return = 'CANCEL'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TileVerticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }


        void CreateMenu_Master()
        {
            try
            {
                if (MyBase.Check_Table("Menu_Master") == false)
                {
                    MyBase.Run("Create Table Menu_Master (Menu_Code Number(4), Meun_Name varchar2(50))");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //void Update_Menus()
        //{
        //    int i = 1000;
        //    try
        //    {
        //        foreach (Control Ct in this.Controls)
        //        {
        //            if (Ct is MenuStrip)
        //            {
        //                foreach (MenuItem Co in this.menuStrip.menuitems)
        //                {
        //                    if (Co is MenuItem)
        //                    {
        //                        MyBase.Run("INsert into Menu_master values (" + i + ",'" + Ct.Name + "')");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Load_SaveEntry();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                Load_SaveEntry();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeletetoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                Load_DeleteEntry();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Load_DeleteEntry();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ConfirmtoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                Load_DeleteConfirmEntry();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                Load_DeleteConfirmEntry();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                Load_ViewEntry();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Load_PrintEntry();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void printSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                Load_PrintEntry();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Vew_Help(String tblName, String Condition)
        {
            try
            {

                if (UserName.ToUpper() == "ADMIN" || UserName.ToUpper() == "MD" || UserName.ToUpper() == "PSC")
                {
                    this.ActiveMdiChild.Text = this.ActiveMdiChild.Text + MyBase.View_Details(tblName, Condition);
                }
                else
                {
                    this.ActiveMdiChild.Text = this.ActiveMdiChild.Text; 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ViewtoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    Load_ViewEntry();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public String Get_Max_ChequeNo(String BookNo)
        {
            Double ChqNo1 = 0;
            Double ChqNo2 = 0;
            DataTable Dt1 = new DataTable();
            try
            {
                MyBase.Load_Data("Select isnull(Max(Chq_No), 0) Chq_No from Cheque_Details where BookNo = '" + BookNo + "'", ref Dt1);
                ChqNo1 = Convert.ToDouble(Dt1.Rows[0]["Chq_No"]);
                
                MyBase.Load_Data("Select isnull(Max(Cheque_No), 0) Chq_No from Cheque_cancel_Details where BookNo = '" + BookNo + "'", ref Dt1);
                ChqNo2 = Convert.ToDouble(Dt1.Rows[0]["Chq_No"]);

                if (ChqNo1 == 0 && ChqNo2 == 0)
                {
                    ChqNo1 = Convert.ToDouble(MyBase.GetData_InString("ChequeBook_master", "ENo", BookNo, "ChequeNo_From"));
                    return ChqNo1.ToString();
                }
                else
                {
                    if (ChqNo2 > ChqNo1)
                    {
                        return Convert.ToString(Convert.ToDouble(ChqNo2) + 1);
                    }
                    else
                    {
                        return Convert.ToString(Convert.ToDouble(ChqNo1) + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Form_Close()
        {
            MessageBox.Show("Closed");
        }

        void MoveTo_Oracle()
        {
            try
            {
                MyBase.BackupConnection_Initialize(false);
                DataTable TablDt = new DataTable(); 
             }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Boolean Menu_Items_Visible_Status(ToolStripItemCollection Main)
        {
            Boolean Flag = false;
            try
            {
                for (int i = 0; i <= Main.Count - 1; i++)
                {
                    if (Main[i] is ToolStripSeparator)
                    {
                    }
                    else if (Main[i] is ToolStripComboBox)
                    {
                    }
                    else
                    {
                        ToolStripMenuItem Tst;
                        Tst = (ToolStripMenuItem)Main[i];
                        if (Tst.Name != "fileMenu" && Tst.Name != "windowsMenu")
                        {
                            //if (MyBase.Get_RecordCount("Socks_Permission_Master", "Menu_Name = '" + Tst.Name + "' and user_Id = " + UserCode) > 0)
                            //{
                            //    Tst.Visible = true;
                            //    Flag = true;
                            //}
                            //else
                            //{
                            //    Tst.Visible = Menu_Items_Visible_Status(Tst.DropDownItems);
                            //}


                            if (Has_Menu_Rights(Tst.Name))
                            {
                                Tst.Visible = true;
                                Flag = true;
                            }
                            else
                            {
                                Tst.Visible = Menu_Items_Visible_Status(Tst.DropDownItems);
                            }

                            //N.G.Ravikumar - DropdownList 2 Not Display Correction. - 17-09-2011

                            if (Tst.DropDown.Items.Count > 0)
                            {
                                Tst.Visible = Menu_Items_Visible_Status(Tst.DropDownItems);
                            }

                        }
                    }
                }
                if (Flag != true)
                {
                    Flag = false;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                Flag = false;
                return Flag;
            }
        }

        Boolean Has_Menu_Rights(String Menu_Name)
        {
            try
            {
                for (int i = 0; i <= Menu_Dt.Rows.Count - 1; i++)
                {
                    if (Menu_Dt.Rows[i]["Menu_CName"].ToString().ToUpper() == Menu_Name.ToUpper())
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Menu_Items_Visible_False(ToolStripItemCollection Main)
        {
            try
            {
                for (int i = 0; i <= Main.Count - 1; i++)
                {
                    if (Main[i] is ToolStripSeparator)
                    {
                    }
                    else if (Main[i] is ToolStripComboBox)
                    {
                    }

                    else 
                    {
                        ToolStripMenuItem Tst;
                        Tst = (ToolStripMenuItem)Main[i];
                        if (Tst.Name != "fileMenu" && Tst.Name != "windowsMenu")
                        {
                            Menu_Items_Visible_False(Tst.DropDownItems);
                            Tst.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void UserSettings()
        {
            try
            {
                Load_Menu_Combo();

                if (UserName.ToUpper() != "ADMIN" && UserName.ToUpper() != "MD")
                {
                    Menu_Items_Visible_False((ToolStripItemCollection)menuStrip.Items);
                    Menu_Items_Visible_Status((ToolStripItemCollection)menuStrip.Items);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //void UserSettings()
        //{
        //    try
        //    {
        //        if (UserName.ToUpper() != "ADMIN")
        //        {
        //            //MenuItems_False();
        //            //MenuItems_True();
        //            Menu_Settings_New();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Sure to Backup ...!", "Backup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MyBase.Backup(String.Format("{0:dd_MM_yyyy}", DateTime.Now));
                    MessageBox.Show("Backup Saved ...!", "Vaahini");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

#endregion

        void Mail()
        {
            try
            {
                MyBase.Add_NewField("Mail", "Mode", "varchar(2)");
                if (MyBase.Get_RecordCount("Mail", "Mode is null") > 0)
                {
                    MyBase.Execute("update mail set mode = 'P' where mode is null");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        void MDICaption()
        {
            Double TWidth = this.Width;
            Double PerWidth = TWidth / 10;
            String Sql;
            try
            {
                Sql = "PROJECTS :  " + Proj_Login_Name + " - " + MyBase.PadR(CompName.ToUpper().Trim() + " ( " + YearCode.Trim() + " )", 77);
                this.Text = Sql + " " + UserName + " - " + MyBase.GetServerDateTime_Local();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void LocationField()
        {
            try
            {
                MyBase.Add_NewField("GSN_Details", "Location_Code", "Number(3)");
                MyBase.Add_NewField("GSN_Acceptance_Details", "Location_Code", "Number(3)");
                MyBase.Add_NewField("CashBill_Details", "Location_Code", "Number(3)");
                MyBase.Add_NewField("DC_Details", "Location_Code", "Number(3)");
                MyBase.Add_NewField("DCRETURN_Details", "Location_Code", "Number(3)");
                MyBase.Add_NewField("SalesReturn_Details", "Location_Code", "Number(3)");
                MyBase.Add_NewField("GSN_return_Details", "Location_Code", "Number(3)");
                MyBase.Add_NewField("Acc_Stock", "Location_Code", "Number(3)");
                BranchUpdations();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void BranchUpdations()
        {
            try
            {
                if (MyBase.Get_RecordCount("GSN_Details", "Location_Code is Null") > 0)
                {
                    MyBase.Execute("Update GSN_Details set Location_Code = 1");
                }
                if (MyBase.Get_RecordCount("GSN_Acceptance_Details", "Location_Code is Null") > 0)
                {
                    MyBase.Execute("Update GSN_Acceptance_Details set Location_Code = 1");
                }
                if (MyBase.Get_RecordCount("CashBIll_Details", "Location_Code is Null") > 0)
                {
                    MyBase.Execute("Update CashBill_Details set Location_Code = 1");
                }
                if (MyBase.Get_RecordCount("DC_Details", "Location_Code is Null") > 0)
                {
                    MyBase.Execute("Update DC_Details set Location_Code = 1");
                }
                if (MyBase.Get_RecordCount("DCReturn_Details", "Location_Code is Null") > 0)
                {
                    MyBase.Execute("Update DCReturn_Details set Location_Code = 1");
                }
                if (MyBase.Get_RecordCount("SalesReturn_Details", "Location_Code is Null") > 0)
                {
                    MyBase.Execute("Update SalesReturn_Details set Location_Code = 1");
                }
                if (MyBase.Get_RecordCount("GSN_Return_Details", "Location_Code is Null") > 0)
                {
                    MyBase.Execute("Update GSN_Return_Details set Location_Code = 1");
                }
                if (MyBase.Get_RecordCount("Acc_Stock", "Location_Code is Null") > 0)
                {
                    MyBase.Execute("Update Acc_Stock set Location_Code = 1");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void loadSysName()
        {
            try
            {
                if (MyBase.Check_Table("Sys_Master") == false)
                {
                    MyBase.Execute("Create table Sys_master (Sys_Code Numeric(3), Sys_Name Varchar(50))");
                }
                if (MyBase.Get_RecordCount("Sys_Master", "Sys_Name = '" + Environment.MachineName.Replace("-", String.Empty) + "'") == 0)
                {
                    MyBase.Execute("Insert into Sys_master values (" + MyBase.MaxWOCC("Sys_Master", "Sys_Code", "") + ", '" + Environment.MachineName.Replace("-", String.Empty) + "')");
                }
                SysCode = Convert.ToInt32(MyBase.GetData_InNumber("Sys_master", "Sys_Name", Environment.MachineName.Replace("-", String.Empty), "Sys_Code"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void ledgerMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void voucherEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {

        }

        private void MDIMain_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control == true && (e.KeyCode == Keys.C || e.KeyCode == Keys.V))
                {
                    Clipboard.Clear();
                }

                if (this.ActiveMdiChild != null)
                {
                    if (this.ActiveMdiChild.ActiveControl.Name == "TxtRemarks_Narration")
                    {
                        if (e.KeyCode != Keys.Escape)
                        {
                            e.Handled = true;
                        }
                    }
                }
                if (1 == 2)
                {
                    if (e.KeyCode == Keys.Escape)
                    {
                        if (this.ActiveMdiChild != null)
                        {
                            if (_Form == true)
                            {
                                if (MessageBox.Show("Sure to Close ...!", "Close ?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                {
                                    this.ActiveMdiChild.Close();
                                }
                            }
                        }
                    }
                }
                if (e.Control && e.KeyCode == Keys.F)
                {
                    Criteria();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Object reference not set to an instance of an object"))
                {
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public Int32 Max_Vno_Voucher(int Vmode)
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("Select isnull(Max(cast(Vno as Numeric(12))), 0) + 1 as VNo from voucher_Master where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and vmode = " + Vmode, ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(Dt.Rows[0]["Vno"]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0; 
            }
        }

        private void viewMenu_Click(object sender, EventArgs e)
        {

        }

        private void gSNReturnRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void supplierWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmRPTDDSupplierWise(), Window.Maximized, true, true, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void permissionMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmProjectsPermissionMaster(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void uOMMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void customerAreaMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void uOMMasterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fileMenu_Click(object sender, EventArgs e)
        {

        }

        private void uomMasterToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void supplierAreaMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void supplierAreaMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ledgerGroupMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void categoryMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void productGroupMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void manufacturerMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void productMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void godownMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void departmentMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void areaGroupMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void staffMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void Resize(Form Frm)
        {
            try
            {
                if (Frm.Name != "FrmCriteria")
                {
                    if (Frm.Text.Contains("~") == false)
                    {
                        if (Frm.Name == "Frm_ItemMaster")
                        {
                            MyBase.ReSize_Form(Frm, true, 929, 513);
                        }
                        else if (Frm.Name == "FrmDO_Entry")
                        {
                            MyBase.ReSize_Form(Frm, true, 995, 606);
                        }
                        else if (Frm.Name == "Frm_Sales_Buyer_Details")
                        {
                            MyBase.ReSize_Form(Frm, true, 1150, 492);
                        }
                        else if (Frm.Name == "FrmAssetMaster")
                        {
                            MyBase.ReSize_Form(Frm, true, 1000, 600);
                        }
                        else if (Frm.Name == "FrmSocksYarnPOEntry")
                        {
                            MyBase.ReSize_Form(Frm, true, 768, 541);
                        }
                        else if (Frm.Name == "FrmSocksTrimsPOEntry")
                        {
                            MyBase.ReSize_Form(Frm, true, 768, 541);
                        }
                        else if (Frm.Name == "FrmSocksYarnGRN")
                        {
                            MyBase.ReSize_Form(Frm, true, 719, 517);
                        }
                        else if (Frm.Name == "FrmGrnInvoicing")
                        {
                            MyBase.ReSize_Form(Frm, true, 765, 521);
                        }
                        else if (Frm.Name == "FrmSocksTrimsGRN")
                        {
                            MyBase.ReSize_Form(Frm, true, 719, 517);
                        }
                        else
                        {
                            MyBase.ReSize_Form(Frm, true, 959, 606);
                        }
                        Frm.Text = Frm.Text + "~";
                    }
                    else
                    {
                        if (Frm.WindowState == FormWindowState.Normal)
                        {
                            Frm.WindowState = FormWindowState.Maximized;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MDIMain_MdiChildActivate(object sender, EventArgs e)
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    if (this.ActiveMdiChild.Name == "FrmCriteria")
                    {
                        this.ActiveMdiChild.WindowState = FormWindowState.Normal;
                    }
                    else if (this.ActiveMdiChild.Name == "FrmCalculator")
                    {
                        return;
                    }
                    else
                    {
                        if (this.ActiveMdiChild.Name == "FrmCRViewer")
                        {
                            return;
                        }
                        Resize(this.ActiveMdiChild);
                        if (this.ActiveMdiChild.Tag != null)
                        {
                            if (this.ActiveMdiChild.Text.Contains(" - New"))
                            {
                                MenuButton_Status("New");
                            }
                            else if (this.ActiveMdiChild.Text.Contains(" - Edit"))
                            {
                                MenuButton_Status("Edit");
                            }
                            else if (this.ActiveMdiChild.Text.Contains(" - Delete"))
                            {
                                MenuButton_Status("Delete");
                            }
                            else if (this.ActiveMdiChild.Text.Contains(" - View"))
                            {
                                MenuButton_Status("View");
                            }
                            else
                            {
                                MenuButton_Status("Form");
                            }
                        }
                    }
                }
                else
                {
                    Common_Help_Text(CompName);
                }
                //Child_Active(this.ActiveMdiChild);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dCEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void creditSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void quotationRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cashSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void proformaInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void proformaRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void salesRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cashSalesRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dCRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MDIMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(39))
                {
                    e.Handled = true;
                    SendKeys.Send(Convert.ToChar(96).ToString());
                }
                if (this.ActiveMdiChild != null)
                {
                    if (this.ActiveMdiChild.ActiveControl.Name == "TxtRemarks_Narration")
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Fill_ItemName(String ItemName)
        {
            int k = 0;
            try
            {
                ItemNameArr = new String[3];
                StreamWriter SW = new StreamWriter("C:\\vaahrep\\in.txt");
                SW.WriteLine(ItemName);
                SW.Close();

                for (int i = 0; i <= 2; i++)
                {
                    ItemNameArr[i] = string.Empty;
                }
                k = 0;
                StreamReader SR = new StreamReader("C:\\vaahrep\\in.txt");
                while (SR.EndOfStream == false)
                {
                    if (View)
                    {
                        ItemNameArr[k] = SR.ReadLine().Replace("`","'");
                    }
                    else
                    {
                        ItemNameArr[k] = SR.ReadLine();
                    }
                    k += 1;
                }
                SR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Fill_ItemName(Double item_Code)
        {
            int k = 0;
            try
            {
                ItemNameArr = new String[3];
                StreamWriter SW = new StreamWriter("C:\\vaahrep\\in.txt");
                SW.WriteLine(MyBase.GetData_InString("Product_Master", "Item_Code", item_Code.ToString(), "Item_Name"));
                SW.Close();

                for (int i = 0; i <= 2; i++)
                {
                    ItemNameArr[i] = string.Empty;
                }
                k = 0;
                StreamReader SR = new StreamReader("C:\\vaahrep\\in.txt");
                while (SR.EndOfStream == false)
                {
                    if (View)
                    {
                        ItemNameArr[k] = SR.ReadLine().Replace("`", "'");
                    }
                    else
                    {
                        ItemNameArr[k] = SR.ReadLine();
                    }
                    k += 1;
                }
                SR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void estimationInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dCPendingRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addressPrintingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void customerAppointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void appointmentPendingRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void appointmentReminderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Load_Company_master()
        {
            try
            {
                ShowChild(new FrmCompanyMaster(), Window.Normal, false, false, Entry_Mode._New, "companyMasterToolStripMenuItem");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void companyMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (CompCode == 1)
                {
                    Load_Company_master();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public Boolean Voucher_Type_is_Others_Cash_Bank(Int64 Vcode, DateTime Vdate, int Compcode, string Year_Code)
        {
            try
            {
                if (CompName.ToUpper().Contains("AEGAN"))
                {
                    return true;
                }
                else
                {
                    //if (Compcode == 1)
                    //{
                    //    return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                    if (MyBase.Get_RecordCount("Voucher_Master", "Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "' and Vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and upper(Vtype) in ('OTHERS', 'CASH', 'BANK')") > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Boolean Is_Tally_Posted(Int64 Vcode)
        {
            try
            {
                if (MyBase.Get_RecordCount("Exported_Vouchers", "Vcode = " + Vcode + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        void Check_TDS()
        {
            try
            {
                if (MyBase.Check_Table("TDS_Type") == false)
                {
                    MyBase.Execute("create table TDS_Type (No int, NAme varchar(50))");
                    MyBase.Execute(" insert into tds_type values (1, 'Association of Persons')");
                    MyBase.Execute(" insert into tds_type values (2, 'Body of Individuals')");
                    MyBase.Execute(" insert into tds_type values (3, 'Company - Non Resident')");
                    MyBase.Execute(" insert into tds_type values (4, 'Company - Resident')");
                    MyBase.Execute(" insert into tds_type values (5, 'Co-Operative Society')");
                    MyBase.Execute(" insert into tds_type values (6, 'Individual/HUF - Non Resident')");
                    MyBase.Execute(" insert into tds_type values (7, 'Individual/HUF - Resident')");
                    MyBase.Execute(" insert into tds_type values (8, 'Local Authority')");
                    MyBase.Execute(" insert into tds_type values (9, 'Partnership Firm')");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void ledgerMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        String Get_Bank_Name_From_Voucher(Int64 Vcode, DateTime Vdate, int CompCode, string Year_Code)
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("Select v1.Ledger_Code, l1.Ledger_Name from voucher_details v1 left join ledger_master l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' and v1.vcode = " + Vcode + " and v1.vdate = '" + string.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.ledger_Code in (Select ledger_code from ledger_master where ledger_group_Code in (Select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and groupreserved = 1600) and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "') and v1.credit > 0", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    return Dt.Rows[0]["Ledger_Name"].ToString();
                }
                else
                {
                    return "-";
                }
            }
            catch (Exception ex)
            {
                return "-";
            }
        }

        String Get_ChqNo_From_Voucher(Int64 Vcode, DateTime Vdate, int CompCode, string Year_Code)
        {
            DataTable Dt = new DataTable();
            String Str = "-";
            try
            {
                MyBase.Load_Data("Select v1.Chq_No, v1.chq_Date from Cheque_Details v1 where v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' and v1.vcode = " + Vcode + " and v1.vdate = '" + string.Format("{0:dd-MMM-yyyy}", Vdate) + "' ", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        Str = Dt.Rows[i]["Chq_No"].ToString();
                    }
                    else
                    {
                        Str += ", " + Dt.Rows[i]["Chq_No"].ToString();
                    }
                }
                return Str;
            }
            catch (Exception ex)
            {
                return Str;
            }
        }

        String Get_Breakup_From_Voucher(Int64 Vcode, DateTime Vdate, int CompCode, string Year_Code)
        {
            DataTable Dt = new DataTable();
            String Str = "-";
            try
            {
                MyBase.Load_Data("select refdoc, refdate from voucher_breakup_bills where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and company_Code = "  +  CompCode + " and year_Code = '" + Year_Code + "'", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        Str = Dt.Rows[i]["RefDoc"].ToString() + "/" + String.Format("{0:dd.MM.yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"]));
                    }
                    else
                    {
                        Str += ", " + Dt.Rows[i]["RefDoc"].ToString() + "/" + String.Format("{0:dd.MM.yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"]));
                    }
                }
                return Str;
            }
            catch (Exception ex)
            {
                return Str;
            }
        }

        string Get_Bank_Name(Int64 Vcode, DateTime vdate)
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("Select isnull(Ledger_NAme, '') ledger_Name from Ledger_master where ledger_Code in (Select ledger_Code from voucher_Details where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", vdate) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_group_code in (Select groupcode from groupmas where groupreserved IN (1600, 1650) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "')", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    return Dt.Rows[0]["Ledger_NAme"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        string Get_Cheque_Bank_Name(Int64 Vcode, DateTime vdate)
        {
            DataTable Dt = new DataTable();
            try
            {
                if (CompName.ToUpper().Contains("DHANA"))
                {
                    MyBase.Load_Data("Select isnull(Ledger_NAme, '') ledger_Name from Ledger_master where ledger_Code in (Select ledger_Code from voucher_Details where vcode = " + Vcode + " and byto = 'TO' and vdate = '" + String.Format("{0:dd-MMM-yyyy}", vdate) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_group_code in (Select groupcode from groupmas where groupreserved IN (1600, 1650, 2300) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "')", ref Dt);
                }
                else
                {
                    MyBase.Load_Data("Select isnull(Ledger_NAme, '') ledger_Name from Ledger_master where ledger_Code in (Select ledger_Code from voucher_Details where vcode = " + Vcode + " and byto = 'TO' and vdate = '" + String.Format("{0:dd-MMM-yyyy}", vdate) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_group_code in (Select groupcode from groupmas where groupreserved IN (1600, 1650) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "')", ref Dt);
                }
                if (Dt.Rows.Count > 0)
                {
                    return Dt.Rows[0]["Ledger_NAme"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Voucher_Print_only_Cheque(int ModeNo, long VCode, DateTime Vdate, int CompCode, string Year_Code, String CompPrintName, String Term)
        {
            Double CrAmount = 0;
            String Str = String.Empty;
            System.Data.DataTable Dt1 = new DataTable();
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                MyBase.Load_Data("Select Sum(Credit) Credit from voucher_Details where vcode = " + VCode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["Credit"] != DBNull.Value)
                    {
                        CrAmount = Convert.ToDouble(Dt.Rows[0]["Credit"]);
                    }
                }
                if (ModeNo == 1 || ModeNo == 3)
                {
                    String Bank = Get_Cheque_Bank_Name(VCode, Vdate);
                    String Cheque_Name = String.Empty;
                    if (Term == "BANK")
                    {
                        if (MessageBox.Show("Do you Want Cheque Print ..!", "Print", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            MyBase.Load_Data("Select v1.vcode, l1.cheque_Name ledger_name, v1.chq_no, v1.chq_date Vdate, v1.Amount Debit from Cheque_Details v1 left join ledger_master l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' and v1.Status = 'TRUE' order by v1.slno", ref Dt1);
                            if (Dt1.Rows.Count > 0)
                            {
                                Cheque_Name = MyBase.GetData_InStringWC("Cheque_Details", "Vcode", Dt1.Rows[0]["Vcode"].ToString(), "Cheque_Name", CompCode, Year_Code);
                                if (Cheque_Name == String.Empty)
                                {
                                    Cheque_Name = Dt1.Rows[0]["Ledger_Name"].ToString();
                                }
                                for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                                {
                                    if (MessageBox.Show("Cheque No - " + Dt1.Rows[i]["Chq_No"].ToString() + " ..!", "Print", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        if (Dt1.Rows[i]["Ledger_Name"].ToString().ToUpper().Contains("SELF") == false)
                                        {
                                            if (MessageBox.Show("Is A/C PAYEE Enabled ? ", "A/C Payee", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                            {
                                                Cheque("**" + Cheque_Name + "**", MyBase.Rupee(Convert.ToDouble(Dt1.Rows[i]["Debit"])), Convert.ToDouble(Dt1.Rows[0]["Debit"]), true, Bank, Convert.ToDateTime(Dt1.Rows[i]["Vdate"]), true);
                                            }
                                            else
                                            {
                                                Cheque("**" + Cheque_Name + "**", MyBase.Rupee(Convert.ToDouble(Dt1.Rows[i]["Debit"])), Convert.ToDouble(Dt1.Rows[0]["Debit"]), false, Bank, Convert.ToDateTime(Dt1.Rows[i]["Vdate"]), true);
                                            }
                                        }
                                        else
                                        {
                                            Cheque(Cheque_Name + "**", MyBase.Rupee(Convert.ToDouble(Dt1.Rows[i]["Debit"])), Convert.ToDouble(Dt1.Rows[0]["Debit"]), false, Bank, Convert.ToDateTime(Dt1.Rows[i]["Vdate"]), true);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("No Data's to Print ...!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Voucher_Print(int ModeNo, long VCode, DateTime Vdate, int CompCode, string Year_Code, String CompPrintName, String Term)
        {
            Double CrAmount = 0;
            String Str = String.Empty;
            System.Data.DataTable Dt1 = new DataTable();
            System.Data.DataTable Dt = new System.Data.DataTable();
            Double Cash_Amount = 0;

            try
            {
                MyBase.Load_Data("Select Sum(Credit) Credit from voucher_Details where vcode = " + VCode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["Credit"] != DBNull.Value)
                    {
                        CrAmount = Convert.ToDouble(Dt.Rows[0]["Credit"]);
                    }
                }
                if (ModeNo == 1)
                {
                    MyBase.Load_Data("Select Sum(Credit) Credit from voucher_Details where ledger_Code in (Select ledger_Code from ledger_master where company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and ledger_Group_Code in (Select Groupcode from groupmas where company_code = " + CompCode + " and year_Code = '" + Year_Code + "' and groupreserved in (1700))) and vcode = " + VCode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'", ref Dt);
                    if (Dt.Rows.Count > 0)
                    {
                        if (Dt.Rows[0]["Credit"] != DBNull.Value)
                        {
                            Cash_Amount = Convert.ToDouble(Dt.Rows[0]["Credit"]);
                        }
                    }
                    if (MessageBox.Show("Sure to Print ..!", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        Str = "Select top 100000 v2.slno, '" + CompPrintName + "' Company, 'Payment Voucher - " + Term + "' as Voucher, '" + MyBase.Company_Address(CompCode) + "' CompAddress, l1.ledger_Name, " + Cash_Amount + " as Cash_Org, v1.vcode, v1.user_date vdate, v1.vno, v1.user_date, v1.remarks, v2.byto, v2.debit, v2.credit, v2.narration, '" + MyBase.Rupee(CrAmount) + "' as Rupee, " + Cash_Amount + " as Cash, v2.company_Code, v2.year_Code from voucher_master v1 left join voucher_details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' order by v2.slno";
                        MyBase.Execute_Qry(Str, "Rpt_Voucher");
                        CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Payment.rpt");
                        CReport(ref ObjRpt, "Payment Voucher ...!");
                    }
                    else
                    {
                        if (MessageBox.Show("Sure to Print Payment Advice ..!", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            Str = "Select top 100000 v2.slno, '" + CompPrintName + "' Company, 'Payment Voucher - " + Term + "' as Voucher, '" + MyBase.Company_Address(CompCode) + "' CompAddress, l1.ledger_Name, v1.vcode, v1.user_date vdate, v1.vno, v1.user_date, v1.remarks, v2.byto, v2.debit, v2.credit, v2.narration, '" + MyBase.Rupee(CrAmount) + "' as Rupee, v2.company_Code, v2.year_Code from voucher_master v1 left join voucher_details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v2.Byto = 'BY' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' order by v2.slno";
                            MyBase.Execute_Qry(Str, "Rpt_Voucher");

                            Str = "select top 100000 Slno, RefDoc, RefDate, Debit from voucher_Breakup_bills where vcode = " + VCode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and debit > 0 order by slno";
                            MyBase.Execute_Qry(Str, "Rpt_Bill_Details");

                            CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                            if (CompName.ToUpper().Contains("RAJARAM"))
                            {
                                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_PaymentAdvice_Ra.rpt");
                                if (Term == "BANK")
                                {
                                    FormulaFill(ref ObjRpt, "Amount", " Of - " + Get_Bank_Name_From_Voucher(VCode, Vdate, CompCode, Year_Code) + " Cheque No[s] : " + Get_ChqNo_From_Voucher(VCode, Vdate, CompCode, Year_Code) + " for the Amount - " + string.Format("{0:n}", CrAmount));
                                }
                                else
                                {
                                    FormulaFill(ref ObjRpt, "Amount", " For the Amount - " + string.Format("{0:n}", CrAmount));
                                }
                                FormulaFill(ref ObjRpt, "Naration", Get_Breakup_From_Voucher(VCode, Vdate, CompCode, Year_Code));
                            }
                            else
                            {
                                if (CompName.ToUpper().Contains("GAINUP") || CompName.ToUpper().Contains("ALAMELU"))
                                {
                                    ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_PaymentAdvice_GA.rpt");
                                    if (Term == "BANK")
                                    {
                                        FormulaFill(ref ObjRpt, "Amount", " Of - " + Get_Bank_Name_From_Voucher(VCode, Vdate, CompCode, Year_Code) + " Cheque No[s] : " + Get_ChqNo_From_Voucher(VCode, Vdate, CompCode, Year_Code) + " for the Amount - " + string.Format("{0:n}", CrAmount));
                                    }
                                    else
                                    {
                                        FormulaFill(ref ObjRpt, "Amount", " For the Amount - " + string.Format("{0:n}", CrAmount));
                                    }
                                }
                                else
                                {
                                    ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_PaymentAdvice.rpt");
                                }
                            }
                            CReport(ref ObjRpt, "Payment Advice ...!");
                        }
                        else
                        {
                            String Bank = Get_Cheque_Bank_Name(VCode, Vdate);
                            String Cheque_Name = String.Empty;
                            if (Term == "BANK")
                            {
                                if (MessageBox.Show("Do you Want Cheque Print ..!", "Print", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    if (MyBase.Get_RecordCount ("VouCher_Breakup_Bills", "Vcode = " + VCode + " and company_Code = " + CompCode + " and refDoc like '%ADVANCE%'") > 0)
                                    {
                                        if (MyBase.Get_RecordCount ("Advance_Payment_Approval", "Vcode = " + VCode + " and company_Code = " + CompCode) == 0)
                                        {
                                            MessageBox.Show ("Advance Payment Not Approval ... !", "Vaahini");
                                            return;
                                        }
                                    }
                                    MyBase.Load_Data("Select v1.vcode, l1.cheque_Name ledger_name, v1.chq_no, v1.chq_date Vdate, v1.Amount Debit from Cheque_Details v1 left join ledger_master l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' and v1.Status = 'TRUE' order by v1.slno", ref Dt1);
                                    if (Dt1.Rows.Count > 0)
                                    {
                                        Cheque_Name = MyBase.GetData_InStringWC("Cheque_Details", "Vcode", Dt1.Rows[0]["Vcode"].ToString(), "Cheque_Name", CompCode, Year_Code);
                                        if (Cheque_Name == String.Empty)
                                        {
                                            Cheque_Name = Dt1.Rows[0]["Ledger_Name"].ToString();
                                        }
                                        for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                                        {
                                            if (MessageBox.Show("Cheque No - " + Dt1.Rows[i]["Chq_No"].ToString() + " ..!", "Print", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                            {
                                                if (Dt1.Rows[i]["Ledger_Name"].ToString().ToUpper().Contains("SELF") == false)
                                                {
                                                    if (MessageBox.Show("Is A/C PAYEE Enabled ? ", "A/C Payee", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                                    {
                                                        Cheque("**" + Cheque_Name + "**", MyBase.Rupee(Convert.ToDouble(Dt1.Rows[i]["Debit"])), Convert.ToDouble(Dt1.Rows[0]["Debit"]), true, Bank, Convert.ToDateTime(Dt1.Rows[i]["Vdate"]), true);
                                                    }
                                                    else
                                                    {
                                                        Cheque("**" + Cheque_Name + "**", MyBase.Rupee(Convert.ToDouble(Dt1.Rows[i]["Debit"])), Convert.ToDouble(Dt1.Rows[0]["Debit"]), false, Bank, Convert.ToDateTime(Dt1.Rows[i]["Vdate"]), true);
                                                    }
                                                }
                                                else
                                                {
                                                    Cheque(Cheque_Name + "**", MyBase.Rupee(Convert.ToDouble(Dt1.Rows[i]["Debit"])), Convert.ToDouble(Dt1.Rows[0]["Debit"]), false, Bank, Convert.ToDateTime(Dt1.Rows[i]["Vdate"]), true);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No Data's to Print ...!");
                                    }
                                }
                            }
                        }
                    }
                }
                else if (ModeNo == 2)
                {
                    if (MessageBox.Show("Sure to Print ..!", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        //Str = "select '" + CompPrintName + "' Company, 'Receipt Voucher - " + Term + "' as Voucher, 'Received From :' as TOBy, '" + MyBase.Company_Address(CompCode) + "' CompAddress, l1.ledger_Name, v1.vcode, v1.vdate, v1.vno, v1.user_date, v1.remarks, v2.byto, v2.debit, v2.credit, v1.remarks narration, v2.rev_ledcode, (Case when v2.Debit > 0 then v2.debit else v2.credit end) Amount, '" + MyBase.Rupee(CrAmount) + "' as Rupee, v2.company_Code, v2.year_Code from voucher_master v1 left join voucher_details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "'";
                        //MyBase.Execute_Qry(Str, "Rpt_Voucher1");
                        //MyBase.Execute_Qry("select v2.*, v3.ledger Debit_Ledger, v3.Rev_ledger Credit_Ledger from voucher_Slno v3, Rpt_Voucher1 v2 where v3.vcode = v2.vcode and v3.vdate = v2.vdate and v3.company_Code = v2.company_Code and v3.year_Code = v2.year_Code and v2.byto = v3.byto and v3.byto = 'BY'", "Rpt_Voucher");
                        ////Str = "select '" + CompPrintName + "' Company, 'Receipt Voucher - " + Term + "' as Voucher, 'Received From :' as TOBy, '" + MyBase.Company_Address(CompCode) + "' CompAddress, l1.ledger_Name, v1.vcode, v1.vdate, v1.vno, v1.user_date, v1.remarks, v2.byto, v2.debit, v2.credit, v1.remarks narration, v2.rev_ledcode, (Case when v2.Debit > 0 then v2.debit else v2.credit end) Amount, '" + MyBase.Rupee(CrAmount) + "' as Rupee from voucher_master v1 left join voucher_details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v2.ledger_Code not in (select ledger_Code from ledger_master where ledger_group_Code in (1700, 1600, 1900, 3600) and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "') and v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "'";
                        ////MyBase.Execute_Qry(Str, "Rpt_Voucher");
                        //Str = "select top 1000 l1.ledger_Name, v1.vcode, v1.vdate, v1.vno, v1.user_date, v1.remarks, (case when v2.Mode = 'N' then 'New' else 'Against' end) Mode, v2.RefDoc, v2.RefDate, (case when v2.Bterm = 'CR' then v2.debit else v2.Credit end) Amount from voucher_master v1 left join voucher_Breakup_Bills v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v2.ledger_Code not in (select ledger_Code from ledger_master where ledger_group_Code in (1700, 1600, 1900, 3600) and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "') and v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' order by v2.RefDate, v2.refDoc";
                        //MyBase.Execute_Qry(Str, "Rpt_Breakup");
                        ////Load_Report("/Accounts_Reports/Rpt_Receipt", "Receipt Voucher");
                        //CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        //ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RPT_RECEIPT.rpt");
                        //CReport(ref ObjRpt, "Receipt Voucher ...!");

                        Str = "Select top 100000  v2.slno, '" + CompPrintName + "' Company, 'Receipt Voucher - " + Term + "' as Voucher, '" + MyBase.Company_Address(CompCode) + "' CompAddress, l1.ledger_Name, v1.vcode, v1.user_date vdate, v1.vno, v1.user_date, v1.remarks, v2.byto, v2.debit, v2.credit, v2.narration, '" + MyBase.Rupee(CrAmount) + "' as Rupee, v2.company_Code, v2.year_Code from voucher_master v1 left join voucher_details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' order by v2.slno";
                        MyBase.Execute_Qry(Str, "Rpt_Voucher");
                        CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        //ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RPT_RECEIPT.rpt");
                        ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RPT_RECEIPT.rpt");
                        CReport(ref ObjRpt, "Receipt Voucher ...!");
                    }
                }
                else if (ModeNo == 3)
                {
                    if (MessageBox.Show("Sure to Print ..!", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Str = "Select top 100000  v2.slno, '" + CompPrintName + "' Company, 'Contra Voucher ' as Voucher, '" + MyBase.Company_Address(CompCode) + "' CompAddress, l1.ledger_Name, v1.vcode, v1.user_date vdate, v1.vno, v1.user_date, v1.remarks, v2.byto, v2.debit, v2.credit, v2.narration, '" + MyBase.Rupee(CrAmount) + "' as Rupee, v2.company_Code, v2.year_Code from voucher_master v1 left join voucher_details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' order by v2.slno";
                        MyBase.Execute_Qry(Str, "Rpt_Voucher");
                        CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RPT_Journal.rpt");
                        CReport(ref ObjRpt, "Journal Voucher ...!");
                    }
                    else
                    {
                        String Bank = Get_Cheque_Bank_Name(VCode, Vdate);
                        String Cheque_Name = String.Empty;
                        if (MyBase.Get_RecordCount ("CHEQUE_DETAILS", "VCODE = " + VCode + " and vdate = '" + string.Format("{0:dd-MMM-yyyy}", Vdate) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") > 0)
                        {
                            if (MessageBox.Show("Do you Want Cheque Print ..!", "Print", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                MyBase.Load_Data("Select v1.vcode, l1.cheque_Name ledger_name, v1.chq_no, v1.chq_date Vdate, v1.Amount Debit from Cheque_Details v1 left join ledger_master l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' and v1.Status = 'TRUE' order by v1.slno", ref Dt1);
                                if (Dt1.Rows.Count > 0)
                                {
                                    Cheque_Name = MyBase.GetData_InStringWC("Cheque_Details", "Vcode", Dt1.Rows[0]["Vcode"].ToString(), "Cheque_Name", CompCode, Year_Code);
                                    if (Cheque_Name == String.Empty)
                                    {
                                        Cheque_Name = Dt1.Rows[0]["Ledger_Name"].ToString();
                                    }
                                    for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                                    {
                                        if (MessageBox.Show("Cheque No - " + Dt1.Rows[i]["Chq_No"].ToString() + " ..!", "Print", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            if (Dt1.Rows[i]["Ledger_Name"].ToString().ToUpper().Contains("SELF") == false)
                                            {
                                                if (MessageBox.Show("Is A/C PAYEE Enabled ? ", "A/C Payee", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                                {
                                                    Cheque("**" + Cheque_Name + "**", MyBase.Rupee(Convert.ToDouble(Dt1.Rows[i]["Debit"])), Convert.ToDouble(Dt1.Rows[0]["Debit"]), true, Bank, Convert.ToDateTime(Dt1.Rows[i]["Vdate"]), true);
                                                }
                                                else
                                                {
                                                    Cheque("**" + Cheque_Name + "**", MyBase.Rupee(Convert.ToDouble(Dt1.Rows[i]["Debit"])), Convert.ToDouble(Dt1.Rows[0]["Debit"]), false, Bank, Convert.ToDateTime(Dt1.Rows[i]["Vdate"]), true);
                                                }
                                            }
                                            else
                                            {
                                                Cheque(Cheque_Name + "**", MyBase.Rupee(Convert.ToDouble(Dt1.Rows[i]["Debit"])), Convert.ToDouble(Dt1.Rows[0]["Debit"]), false, Bank, Convert.ToDateTime(Dt1.Rows[i]["Vdate"]), true);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No Data's to Print ...!");
                                }
                            }
                        }
                    }
                }
                else if (ModeNo == 4)
                {
                    if (MessageBox.Show("Sure to Print ..!", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        //Str = "select '" + CompPrintName + "' Company, 'Journal' as Voucher, 'Received From :' as TOBy, '" + MyBase.Company_Address(CompCode) + "' CompAddress, l1.ledger_Name, v1.vcode, v1.vdate, v1.vno, v1.user_date, v1.remarks, v2.byto, v2.debit, v2.credit, v1.remarks narration, v2.rev_ledcode, (Case when v2.Debit > 0 then v2.debit else v2.credit end) Amount, '" + MyBase.Rupee(CrAmount) + "' as Rupee, v2.company_Code, v2.year_Code from voucher_master v1 left join voucher_details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "'";
                        //MyBase.Execute_Qry(Str, "Rpt_Voucher1");
                        //MyBase.Execute_Qry("select v2.*, v3.ledger Debit_Ledger, v3.Rev_ledger Credit_Ledger from voucher_Slno v3, Rpt_Voucher1 v2 where v3.vcode = v2.vcode and v3.vdate = v2.vdate and v3.company_Code = v2.company_Code and v3.year_Code = v2.year_Code and v2.byto = v3.byto and v3.byto = 'BY'", "Rpt_Voucher_Journal");
                        //Str = "select top 1000 l1.ledger_Name, v1.vcode, v1.vdate, v1.vno, v1.user_date, v1.remarks, (case when v2.Mode = 'N' then 'New' else 'Against' end) Mode, v2.RefDoc, v2.RefDate, (case when v2.Bterm = 'CR' then v2.debit else v2.Credit end) Amount from voucher_master v1 left join voucher_Breakup_Bills v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v2.ledger_Code not in (select ledger_Code from ledger_master where ledger_group_Code in (1700, 1600, 1900, 3600) and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "') and v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' order by v2.RefDate, v2.refDoc";
                        //MyBase.Execute_Qry(Str, "Rpt_Breakup");
                        ////Load_Report("/Accounts_Reports/RptJournal", "Journal Voucher");
                        //CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        //ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Journal.rpt");
                        //CReport(ref ObjRpt, "Journal Voucher ...!");
                        Str = "Select top 100000  v2.slno, '" + CompPrintName + "' Company, 'Journal Voucher ' as Voucher, '" + MyBase.Company_Address(CompCode) + "' CompAddress, l1.ledger_Name, v1.vcode, v1.user_date vdate, v1.vno, v1.user_date, v1.remarks, v2.byto, v2.debit, v2.credit, v2.narration, '" + MyBase.Rupee(CrAmount) + "' as Rupee, v2.company_Code, v2.year_Code from voucher_master v1 left join voucher_details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' order by v2.slno";
                        MyBase.Execute_Qry(Str, "Rpt_Voucher");
                        CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RPT_Journal.rpt");
                        CReport(ref ObjRpt, "Journal Voucher ...!");
                    }
                }
                else if (ModeNo == 5)
                {
                    if (MessageBox.Show("Sure to Print ..!", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Str = "Select top 100000  v2.slno, '" + CompPrintName + "' Company, 'Sales Voucher ' as Voucher, '" + MyBase.Company_Address(CompCode) + "' CompAddress, l1.ledger_Name, v1.vcode, v1.user_date vdate, v1.vno, v1.user_date, v1.remarks, v2.byto, v2.debit, v2.credit, v2.narration, '" + MyBase.Rupee(CrAmount) + "' as Rupee, v2.company_Code, v2.year_Code from voucher_master v1 left join voucher_details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' order by v2.slno";
                        MyBase.Execute_Qry(Str, "Rpt_Voucher");
                        CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RPT_Journal.rpt");
                        CReport(ref ObjRpt, "Sales Voucher ...!");
                    }
                }
                else if (ModeNo == 6)
                {
                    if (MessageBox.Show("Sure to Print ..!", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Str = "Select top 100000  v2.slno, '" + CompPrintName + "' Company, 'Purchase Voucher ' as Voucher, '" + MyBase.Company_Address(CompCode) + "' CompAddress, l1.ledger_Name, v1.vcode, v1.user_date vdate, v1.vno, v1.user_date, v1.remarks, v2.byto, v2.debit, v2.credit, v2.narration, '" + MyBase.Rupee(CrAmount) + "' as Rupee, v2.company_Code, v2.year_Code from voucher_master v1 left join voucher_details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' order by v2.slno";
                        MyBase.Execute_Qry(Str, "Rpt_Voucher");
                        CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RPT_Journal.rpt");
                        CReport(ref ObjRpt, "Purchase Voucher ...!");
                    }
                }
                else if (ModeNo == 7)
                {
                    if (MessageBox.Show("Sure to Print ..!", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        //Str = "select '" + CompPrintName + "' Company, 'Debit Note' as Voucher, 'Received From :' as TOBy, '" + MyBase.Company_Address(CompCode) + "' CompAddress, l1.ledger_Name, v1.vcode, v1.vdate, v1.vno, v1.user_date, v1.remarks, v2.byto, v2.debit, v2.credit, v1.remarks narration, v2.rev_ledcode, (Case when v2.Debit > 0 then v2.debit else v2.credit end) Amount, '" + MyBase.Rupee(CrAmount) + "' as Rupee, v2.company_Code, v2.year_Code from voucher_master v1 left join voucher_details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "'";
                        //MyBase.Execute_Qry(Str, "Rpt_Voucher1");
                        //MyBase.Execute_Qry("select v2.*, v3.ledger Debit_Ledger, v3.Rev_ledger Credit_Ledger from voucher_Slno v3, Rpt_Voucher1 v2 where v3.vcode = v2.vcode and v3.vdate = v2.vdate and v3.company_Code = v2.company_Code and v3.year_Code = v2.year_Code and v2.byto = v3.byto and v3.byto = 'BY'", "Rpt_Voucher_DebitNote");
                        //Str = "select top 1000 l1.ledger_Name, v1.vcode, v1.vdate, v1.vno, v1.user_date, v1.remarks, (case when v2.Mode = 'N' then 'New' else 'Against' end) Mode, v2.RefDoc, v2.RefDate, (case when v2.Bterm = 'CR' then v2.debit else v2.Credit end) Amount from voucher_master v1 left join voucher_Breakup_Bills v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v2.ledger_Code not in (select ledger_Code from ledger_master where ledger_group_Code in (1700, 1600, 1900, 3600) and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "') and v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' order by v2.RefDate, v2.refDoc";
                        //MyBase.Execute_Qry(Str, "Rpt_Breakup");
                        ////Load_Report("/Accounts_Reports/RptDebitNote", "DebitNote Voucher");
                        //CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        //ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\rpt_DebitNote.rpt");
                        //CReport(ref ObjRpt, "Credit Note Voucher ...!");
                        Str = "Select top 100000  v2.slno, '" + CompPrintName + "' Company, 'Debit Note' as Voucher, '" + MyBase.Company_Address(CompCode) + "' CompAddress, l1.ledger_Name, v1.vcode, v1.user_date vdate, v1.vno, v1.user_date, v1.remarks, v2.byto, v2.debit, v2.credit, v2.narration, '" + MyBase.Rupee(CrAmount) + "' as Rupee, v2.company_Code, v2.year_Code from voucher_master v1 left join voucher_details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' order by v2.slno";
                        MyBase.Execute_Qry(Str, "Rpt_Voucher");
                        CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Journal.rpt");
                        CReport(ref ObjRpt, "DebitNote Voucher ...!");
                    }
                }
                else if (ModeNo == 8)
                {
                    if (MessageBox.Show("Sure to Print ..!", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        //Str = "select '" + CompPrintName + "' Company, 'Credit Note' as Voucher, 'Received From :' as TOBy, '" + MyBase.Company_Address(CompCode) + "' CompAddress, l1.ledger_Name, v1.vcode, v1.vdate, v1.vno, v1.user_date, v1.remarks, v2.byto, v2.debit, v2.credit, v1.remarks narration, v2.rev_ledcode, (Case when v2.Debit > 0 then v2.debit else v2.credit end) Amount, '" + MyBase.Rupee(CrAmount) + "' as Rupee, v2.company_Code, v2.year_Code from voucher_master v1 left join voucher_details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "'";
                        //MyBase.Execute_Qry(Str, "Rpt_Voucher1");
                        //MyBase.Execute_Qry("select v2.*, v3.ledger Debit_Ledger, v3.Rev_ledger Credit_Ledger from voucher_Slno v3, Rpt_Voucher1 v2 where v3.vcode = v2.vcode and v3.vdate = v2.vdate and v3.company_Code = v2.company_Code and v3.year_Code = v2.year_Code and v2.byto = v3.byto and v3.byto = 'TO'", "Rpt_Voucher_CreditNote");
                        //Str = "select top 1000 l1.ledger_Name, v1.vcode, v1.vdate, v1.vno, v1.user_date, v1.remarks, (case when v2.Mode = 'N' then 'New' else 'Against' end) Mode, v2.RefDoc, v2.RefDate, (case when v2.Bterm = 'CR' then v2.debit else v2.Credit end) Amount from voucher_master v1 left join voucher_Breakup_Bills v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v2.ledger_Code not in (select ledger_Code from ledger_master where ledger_group_Code in (1700, 1600, 1900, 3600) and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "') and v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' order by v2.RefDate, v2.refDoc";
                        //MyBase.Execute_Qry(Str, "Rpt_Breakup");
                        ////Load_Report("/Accounts_Reports/RptCreditNote", "CreditNote Voucher");
                        //CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        //ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_CreditNote.rpt");
                        //CReport(ref ObjRpt, "Debit Note Voucher ...!");
                        Str = "Select top 100000  v2.slno, '" + CompPrintName + "' Company, 'Credit Note' as Voucher, '" + MyBase.Company_Address(CompCode) + "' CompAddress, l1.ledger_Name, v1.vcode, v1.user_date vdate, v1.vno, v1.user_date, v1.remarks, v2.byto, v2.debit, v2.credit, v2.narration, '" + MyBase.Rupee(CrAmount) + "' as Rupee, v2.company_Code, v2.year_Code from voucher_master v1 left join voucher_details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_code and l1.company_code = v2.company_code and l1.year_Code = v2.year_Code where v1.vcode = " + VCode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' order by v2.slno";
                        MyBase.Execute_Qry(Str, "Rpt_Voucher");
                        CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_journal.rpt");
                        CReport(ref ObjRpt, "CreditNote Voucher ...!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Load_Report(String ReportPath, String Caption)
        {
            String Sql = String.Empty;
            try
            {
                Progress_visible(true);
                Progress_Max(100);
                FrmReportViewer Frm = new FrmReportViewer();
                AddProgress_Value(20);
                Frm.Load_Report(ReportPath);
                AddProgress_Value(20);
                Frm.StartPosition = FormStartPosition.WindowsDefaultLocation;
                AddProgress_Value(20);
                //Frm.MdiParent = this;
                //Frm.ControlBox = false;
                Frm.MaximizeBox = false;
                Frm.MinimizeBox = false;
                AddProgress_Value(20);
                Frm.WindowState = FormWindowState.Maximized;
                //Frm.Show();
                Sql = MyBase.PadR(CompName.ToUpper().Trim() + " ( " + YearCode.Trim() + " )", 77);
                Frm.Text = Sql + " " + Caption;
                AddProgress_Value(20);
                Progress_visible(false);
                Frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Insert_IT_Vouchers(String Term, DateTime Dat, Int64 Vcode)
        {
            String Txt = String.Empty;
            try
            {
                StreamReader Read = new StreamReader("C:\\Vaahrep\\XmlStr.xml");
                Txt = Read.ReadToEnd().Replace("'", "%%%%%%%%%%").Replace("-e28a-422e-", "-e28a-423e-");
                Read.Close();
                Time_Update();

                MyBase.Execute("Insert into IT_Vouchers values (getdate(), '" + Txt + "', " + CompCode + ", '" + YearCode + "', '" + Term + "', " + Vcode + ", '" + String.Format("{0:dd-MMM-yyyy}", Dat) + "', 0, Null)");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ActiveChild_Close()
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    this.ActiveMdiChild.Close();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                ActiveChild_Close();
            }
            catch (Exception ex)
            {
                
            }
        }

        private void ledgerWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void voucherWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            try
            {
                ActiveChild_Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Update_Bank_Payment()
        {
            try
            {
                if (MyBase.Get_RecordCount("Voucher_master", "Vmode = 9 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("update Voucher_master set Vmode = 1, vtype = 'Bank' where vmode = 9 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    MyBase.Execute("update Voucher_master set Vmode = 1, vtype = 'Cash' where vmode = 12 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");

                    MyBase.Execute("update Voucher_master set Vmode = 2, vtype = 'Bank' where vmode = 11 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                    MyBase.Execute("update Voucher_master set Vmode = 2, vtype = 'Cash' where vmode = 13 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("Voucher_master", "Vmode = 14 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("update Voucher_master set Vmode = 7, vtype = 'Others' where vmode = 14 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }
                if (MyBase.Get_RecordCount("Voucher_master", "Vmode = 15 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'") > 0)
                {
                    MyBase.Execute("update Voucher_master set Vmode = 8, vtype = 'Others' where vmode = 15 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Regional_Settings()
        {
            String S = String.Empty;
            try
            {
                RegistryKey Dat = Registry.CurrentUser.OpenSubKey("Control Panel\\International\\", true);
                Dat.SetValue("sShortDate", "dd/MM/yyyy");
                Dat.Close();


                RegistryKey Ng = Registry.CurrentUser.OpenSubKey("Control Panel\\International\\", true);
                Ng.SetValue("sGrouping", "3;2;0");
                Ng.Close();

                RegistryKey Cg = Registry.CurrentUser.OpenSubKey("Control Panel\\International\\", true);
                Cg.SetValue("sMonGrouping", "3;2;0");
                Cg.Close();

                RegistryKey NgN = Registry.CurrentUser.OpenSubKey("Control Panel\\International\\", true);
                NgN.SetValue("iNegNumber", "0");
                NgN.Close();
                
                Regional_Negative(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dayBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        Boolean Is_Tds_Applicable(Int32 ledger_Code)
        {
            try
            {
                if (MyBase.Get_RecordCount("ledger_master", "ledger_Code = " + ledger_Code + " and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and TDSApplicable = 'Y'") > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private void cashBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void bankBookRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void purchaseBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void salesBookToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        Boolean Check_Grid(ContainerControl Ctl)
        {
            try
            {
                foreach (Control Ct in Ctl.Controls)
                {
                    if (Ct is System.Windows.Forms.DataGridView || Ct is DotnetVFGrid.MyDataGridView)
                    {
                        return true;
                    }
                    else if (Ct is System.Windows.Forms.GroupBox || Ct is System.Windows.Forms.TabControl || Ct is System.Windows.Forms.FlowLayoutPanel || Ct is System.Windows.Forms.Panel)
                    {
                        foreach (Control Co in Ct.Controls)
                        {
                            if (Co is System.Windows.Forms.DataGridView || Co is DotnetVFGrid.MyDataGridView)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        void Criteria()
        {
            ContainerControl Ct;
            DataTable Dt;
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    if (Check_Grid (this.ActiveMdiChild) == true)
                    {
                        Dt = Get_Datatable(this.ActiveMdiChild, out Dt);
                        Cri_For = this.ActiveMdiChild.Text;
                        Report_DT = Dt;
                        FrmCriteria Frm = new FrmCriteria(); 
                        //Frm.MdiParent = this;
                        Frm.Initial_Data(ref Report_DT, this.ActiveMdiChild.Name);
                        Frm.StartPosition = FormStartPosition.CenterScreen;
                        Frm.ShowDialog();
                        if (Frm.Criteria_DT != null)
                        {
                            Return_Datasource(ref Frm.Criteria_DT);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //void Criteria(ref DataTable OrgDt, String Cri_For)
        //{
        //    ContainerControl Ct;
        //    try
        //    {
        //        if (this.ActiveMdiChild != null)
        //        {
        //            if (this.ActiveMdiChild.Tag == null || this.ActiveMdiChild.Tag.ToString() == "REPORT")
        //            {
        //                FrmCriteria Frm = new FrmCriteria();
        //                Frm.Org_DT = OrgDt;
        //                Frm.Load_DGV(ref DGV);
        //                Frm.Text = "Criteria For " + Cri_For;
        //                Frm.ShowDialog();
        //                Cri_DT = Frm.Res_DT;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        public DataTable Get_Datatable(ContainerControl Cr, out DataTable Dt)
        {
            try
            {
                Dt = null;
                foreach (Control ct in Cr.Controls)
                {
                    if (ct is System.Windows.Forms.GroupBox || ct is Panel || ct is FlowLayoutPanel || ct is TabControl)
                    {
                        foreach (Control Co in ct.Controls)
                        {
                            if (Co is DataGridView)
                            {
                                DataGridView Obj;
                                Obj = (DataGridView)Co;
                                Dt = (DataTable)Obj.DataSource;
                            }
                        }
                    }
                }
                return Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Get_DataGird(ContainerControl Cr)
        {
            try
            {
                foreach (Control ct in Cr.Controls)
                {
                    if (ct is System.Windows.Forms.GroupBox || ct is Panel || ct is FlowLayoutPanel || ct is TabControl)
                    {
                        foreach (Control Co in ct.Controls)
                        {
                            if (Co is DataGridView)
                            {
                                DGV = (DataGridView)Co;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //private void criteriaToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    DataTable Dt;
        //    DataTable Org;
        //    try
        //    {
        //        if (this.ActiveMdiChild != null)
        //        {
        //            Org = Get_Datatable(this.ActiveMdiChild, out Org);
        //            Get_DataGird(this.ActiveMdiChild);
        //            Criteria(ref Org, this.ActiveMdiChild.Text);
        //            if (Cri_DT != null)
        //            {
        //                Set_Datasource(this.ActiveMdiChild, ref Cri_DT);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void userMasterToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text("Create / Alter / View - User Details ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void companyMasterToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text("Create / Alter / View - Company Details ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ledgerMasterToolStripMenuItem1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text("Create / Alter / View - Ledger Details ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void groupMasterToolStripMenuItem1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text("Create / Alter / View - Ledger Group Details ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void areaMasterToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text("Create / Alter / View - Area Details ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void areaGroupMasterToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text("Create / Alter / View - Area Group Details ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void voucherGroupMasterToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text("Create / Alter / View - Select Ledgers for Voucher Mode ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void voucherEntryToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text("Create / Alter / View - Voucher Details ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ledgerWiseToolStripMenuItem1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text(" Alter / View - Ledger View ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void dayBookToolStripMenuItem1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text(" Alter / View - Day Book View ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void voucherWiseToolStripMenuItem1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text(" Alter / View - Voucher View ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void groupSummaryToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text(" Alter / View - Ledger Group View ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void dayBookToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text(" Report for Day Book ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void cashBookToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text(" Report for Cash Book ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void bankBookRegisterToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text(" Report for Bank Book for One Month ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void purchaseBookToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text(" Report for Purchase Book for One Month ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void salesBookToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text(" Report for Sales Book for One Month ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void criteriaToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    Help_Text(" Fetch Datas from Multiple Criteria ...!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void newToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    Help_Text(" New Entry Mode / F2 ...!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void openToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    Help_Text(" Edit Existing Entry Mode / F3 ...!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void saveToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    Help_Text(" Save Entry / F12 ...!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void DeletetoolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    Help_Text(" Delete Entry Mode / F4 ...!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void DeleteConfirmtoolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    Help_Text(" Confirm to Delete / F5 ...!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void ViewtoolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    Help_Text(" View All Entries / F6 ...!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void printToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    Help_Text(" Print / F8 ...!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void toolStripMenuItem5_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    Help_Text(" Close Currently Opened Entry / F11 ...!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void exitToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Help_Text(" Exit ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void openingToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        public void Sales_Reg(DateTime From, DateTime To, String Ledger_Code, String Party)
        {
            DataTable Dt = new DataTable();
            try
            {

                if (Ledger_Code == String.Empty)
                {
                    if (Party == String.Empty)
                    {
                        Str = "select v1.* from voucher_details v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v2.vmode = 5 and v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", From) + "' and '" + String.Format("{0:dd-MMM-yyyy}", To) + "'";
                    }
                    else
                    {
                        Str = "select v1.* from voucher_details v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v2.vmode = 5 and v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", From) + "' and '" + String.Format("{0:dd-MMM-yyyy}", To) + "' and v1.vcode in (Select vcode from voucher_details where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code in (select ledger_Code from ledger_master where ledger_Name in (" + Party + ") and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'))";
                    }
                }
                else
                {
                    if (Party == String.Empty)
                    {
                        Str = "select v1.* from voucher_details v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v2.vmode = 5 and v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", From) + "' and '" + String.Format("{0:dd-MMM-yyyy}", To) + "' and v1.vcode in (Select vcode from voucher_details where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code in (select ledger_Code from ledger_master where ledger_Name in (" + Ledger_Code + ") and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'))";
                    }
                    else
                    {
                        Str = "select v1.* from voucher_details v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v2.vmode = 5 and v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", From) + "' and '" + String.Format("{0:dd-MMM-yyyy}", To) + "' and v1.vcode in (Select vcode from voucher_details where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code in (select ledger_Code from ledger_master where ledger_Name in (" + Ledger_Code + ") and ledger_Name in (" + Party + ") and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'))";
                    }
                }
                MyBase.Execute_Qry(Str, "Sales_reg_Qry1");

                Str = " select v1.vcode, v1.vdate, v2.vno, v2.user_date, v1.ledger_Code, l1.ledger_NAme, (case when v1.Credit > 0 then v1.credit else v1.debit end ) Amount, v1.company_Code, v1.year_Code from Sales_reg_Qry1 v1 left join ledger_master l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate  and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.ledger_Code in (Select Ledger_Code from ledger_Master where ledger_group_Code in (select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ((groupreserved = 4700) or (groupreserved = 4800))) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') ";
                MyBase.Execute_Qry(Str, "Sales_Party_Head");

                Str = " select v1.vcode, v1.vdate, v2.vno, v2.user_date, v1.ledger_Code, l1.ledger_NAme, (case when v1.Credit > 0 then v1.credit else v1.debit end ) Amount, v1.company_Code, v1.year_Code from Sales_reg_Qry1 v1 left join ledger_master l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.ledger_Code in (Select Ledger_Code from ledger_Master where ledger_group_Code in (select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4400) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "')";
                MyBase.Execute_Qry(Str, "Sales_Sales_Head");

                Str = " select v1.vcode, v1.vdate, v2.vno, v2.user_date, v1.ledger_Code, l1.ledger_NAme, (case when v1.Credit > 0 then v1.credit else v1.debit end ) Amount, v1.company_Code, v1.year_Code from Sales_reg_Qry1 v1 left join ledger_master l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.ledger_Code in (Select Ledger_Code from ledger_Master where ledger_group_Code in (select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ((groupreserved = 4300) or (groupreserved = 1300))) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "')";
                MyBase.Execute_Qry(Str, "Sales_Tax_Head");

                Str = " select v1.vcode, v1.vdate, v2.vno, v2.user_date, v1.ledger_Code, l1.ledger_NAme, (case when v1.Credit > 0 then v1.credit else v1.debit end ) Amount, v1.company_Code, v1.year_Code from Sales_reg_Qry1 v1 left join ledger_master l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.ledger_Code in (Select Ledger_Code from ledger_Master where ledger_group_Code Not in (select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and  groupreserved in (4300,1300,4700,4800,4400)) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') ";
                MyBase.Execute_Qry(Str, "Sales_Other_Head");

                if (MyBase.Check_Table("Sales_Reg"))
                {
                    MyBase.Execute("Drop table sales_reg");
                }
                MyBase.Execute("Create table Sales_Reg (Vcode int, vdate datetime, vno varchar(20), user_date datetime, Party varchar(250))");
                
                // Sales Head
                MyBase.Load_Data("Select Distinct ledger_Code, replace(Dbo.Ledger_String(Ledger_NAme), '%', '') Ledger from sales_Sales_Head", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Add_NewField("Sales_Reg", Dt.Rows[i]["Ledger"].ToString(), "Numeric(13, 2)");
                }

                // Tax Head
                MyBase.Load_Data("Select Distinct ledger_Code, replace(Dbo.Ledger_String(Ledger_NAme), '%', '') Ledger from sales_Tax_Head", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Add_NewField("Sales_Reg", Dt.Rows[i]["Ledger"].ToString(), "Numeric(13, 2)");
                }

                //Other Head
                MyBase.Load_Data("Select Distinct ledger_Code, replace(Dbo.Ledger_String(Ledger_NAme), '%', '') Ledger from sales_Other_Head", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Add_NewField("Sales_Reg", Dt.Rows[i]["Ledger"].ToString(), "Numeric(13, 2)");
                }

                MyBase.Add_NewField("Sales_Reg", "Amount", "Numeric(13, 2)");

                MyBase.Execute("insert into sales_reg(Vcode, vdate, vno, user_Date, Amount, Party) select distinct vcode, vdate, vno, user_date, Amount, Ledger_Name from sales_party_head");


                MyBase.Load_Data("Select *, replace(Dbo.Ledger_String(Ledger_NAme), '%', '') Ledger from Sales_Sales_head", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = "Update sales_reg set " + Dt.Rows[i]["Ledger"].ToString() + " =  isnull(" + Dt.Rows[i]["Ledger"].ToString() + ", 0) + " + Convert.ToDouble(Dt.Rows[i]["Amount"]) + " where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and vno = '" + Dt.Rows[i]["vno"].ToString() + "' and user_Date = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["User_date"])) + "' ";
                    MyBase.Execute(Str);
                }


                MyBase.Load_Data("Select *, replace(Dbo.Ledger_String(Ledger_NAme), '%', '') Ledger from Sales_Tax_head", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = "Update sales_reg set " + Dt.Rows[i]["Ledger"].ToString() + " = isnull(" + Dt.Rows[i]["Ledger"].ToString() + ", 0) + " + Convert.ToDouble(Dt.Rows[i]["Amount"]) + " where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and vno = '" + Dt.Rows[i]["vno"].ToString() + "' and user_Date = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["User_date"])) + "' ";
                    MyBase.Execute(Str);
                }

                MyBase.Load_Data("Select *, replace(Dbo.Ledger_String(Ledger_NAme), '%', '') Ledger from Sales_Other_head", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = "Update sales_reg set " + Dt.Rows[i]["Ledger"].ToString() + " = isnull(" + Dt.Rows[i]["Ledger"].ToString() + ", 0) + " + Convert.ToDouble(Dt.Rows[i]["Amount"]) + " where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and vno = '" + Dt.Rows[i]["vno"].ToString() + "' and user_Date = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["User_date"])) + "' ";
                    MyBase.Execute(Str);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void Purchase_Reg(DateTime From, DateTime To, String Ledger_Code, String Party, String Tax)
        {
            DataTable Dt = new DataTable();
            try
            {

                if (Ledger_Code == String.Empty)
                {
                    if (Party == String.Empty)
                    {
                        Str = "select v1.* from voucher_details v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v2.vmode = 6 and v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", From) + "' and '" + String.Format("{0:dd-MMM-yyyy}", To) + "'";
                    }
                    else
                    {
                        Str = "select v1.* from voucher_details v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v2.vmode = 6 and v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", From) + "' and '" + String.Format("{0:dd-MMM-yyyy}", To) + "' and v1.vcode in (Select vcode from voucher_details where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code in (select ledger_Code from ledger_master where ledger_Name in (" + Party + ") and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'))";
                    }
                }
                else
                {
                    if (Party == String.Empty)
                    {
                        Str = "select v1.* from voucher_details v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v2.vmode = 6 and v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", From) + "' and '" + String.Format("{0:dd-MMM-yyyy}", To) + "' and v1.vcode in (Select vcode from voucher_details where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code in (select ledger_Code from ledger_master where ledger_Name in (" + Ledger_Code + ") and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'))";
                    }
                    else
                    {
                        Str = "select v1.* from voucher_details v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v2.vmode = 6 and v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", From) + "' and '" + String.Format("{0:dd-MMM-yyyy}", To) + "' and v1.vcode in (Select vcode from voucher_details where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_Code in (select ledger_Code from ledger_master where ledger_Name in (" + Ledger_Code + ") and ledger_Name in (" + Party + ") and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'))";
                    }
                }
                MyBase.Execute_Qry(Str, "Sales_reg_Qry1_");

                if (Tax == String.Empty)
                {
                    Str = "Select * from Sales_reg_Qry1_";
                }
                else
                {
                    Str = "Select * from Sales_reg_Qry1_ where vcode in (Select vcode from Sales_reg_Qry1_ where ledger_Code in (select ledger_Code from ledger_master where ledger_name in (" + Tax + ") and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'))";
                }
                MyBase.Execute_Qry(Str, "Sales_reg_Qry1");

                Str = " select v1.vcode, v1.vdate, v2.vno, v2.user_date, v1.ledger_Code, l1.ledger_NAme, l1.ledger_Tin, (case when v1.Credit > 0 then v1.credit else v1.debit end ) Amount, v1.company_Code, v1.year_Code from Sales_reg_Qry1 v1 left join ledger_master l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate  and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.ledger_Code in (Select Ledger_Code from ledger_Master where ledger_group_Code in (select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ((groupreserved = 4700) or (groupreserved = 4800))) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') ";
                MyBase.Execute_Qry(Str, "Sales_Party_Head");

                Str = " select v1.vcode, v1.vdate, v2.vno, v2.user_date, v1.ledger_Code, l1.ledger_NAme, l1.ledger_Tin, (case when v1.Credit > 0 then v1.credit else v1.debit end ) Amount, v1.company_Code, v1.year_Code from Sales_reg_Qry1 v1 left join ledger_master l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.ledger_Code in (Select Ledger_Code from ledger_Master where ledger_group_Code in (select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and groupreserved = 4100) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "')";
                MyBase.Execute_Qry(Str, "Sales_Sales_Head");

                Str = " select v1.vcode, v1.vdate, v2.vno, v2.user_date, v1.ledger_Code, l1.ledger_NAme, l1.ledger_Tin, (case when v1.Credit > 0 then v1.credit else v1.debit end ) Amount, v1.company_Code, v1.year_Code from Sales_reg_Qry1 v1 left join ledger_master l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.ledger_Code in (Select Ledger_Code from ledger_Master where ledger_group_Code in (select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and ((groupreserved = 4300) or (groupreserved = 1300))) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "')";
                MyBase.Execute_Qry(Str, "Sales_Tax_Head");

                Str = " select v1.vcode, v1.vdate, v2.vno, v2.user_date, v1.ledger_Code, l1.ledger_NAme, l1.ledger_Tin, (case when v1.Credit > 0 then v1.credit else v1.debit end ) Amount, v1.company_Code, v1.year_Code from Sales_reg_Qry1 v1 left join ledger_master l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v1.ledger_Code in (Select Ledger_Code from ledger_Master where ledger_group_Code Not in (select groupcode from groupmas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and  groupreserved in (4300,1300,4700,4800,4400)) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "') ";
                MyBase.Execute_Qry(Str, "Sales_Other_Head");

                if (MyBase.Check_Table("Purchase_Reg"))
                {
                    MyBase.Execute("Drop table Purchase_reg");
                }
                MyBase.Execute("Create table Purchase_Reg (Vcode int, vdate datetime, vno varchar(20), user_date datetime, Party varchar(250), Tin Varchar(100))");

                // Sales Head
                MyBase.Load_Data("Select Distinct ledger_Code, replace(Dbo.Ledger_String(Ledger_NAme), '%', '') Ledger from sales_Sales_Head", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Add_NewField("Purchase_Reg", Dt.Rows[i]["Ledger"].ToString(), "Numeric(13, 2)");
                }

                // Tax Head
                MyBase.Load_Data("Select Distinct ledger_Code, replace(Dbo.Ledger_String(Ledger_NAme), '%', '') Ledger from sales_Tax_Head", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Add_NewField("Purchase_Reg", Dt.Rows[i]["Ledger"].ToString(), "Numeric(13, 2)");
                }

                //Other Head
                MyBase.Load_Data("Select Distinct ledger_Code, replace(Dbo.Ledger_String(Ledger_NAme), '%', '') Ledger from sales_Other_Head", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Add_NewField("Purchase_Reg", Dt.Rows[i]["Ledger"].ToString(), "Numeric(13, 2)");
                }

                MyBase.Add_NewField("Purchase_Reg", "Amount", "Numeric(13, 2)");

                MyBase.Execute("insert into Purchase_reg(Vcode, vdate, vno, user_Date, Amount, Party, TIn) select distinct vcode, vdate, vno, user_date, Amount, Ledger_Name, ledger_Tin from sales_party_head");


                MyBase.Load_Data("Select *, replace(Dbo.Ledger_String(Ledger_NAme), '%', '') Ledger from Sales_Sales_head", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = "Update Purchase_reg set " + Dt.Rows[i]["Ledger"].ToString() + " =  isnull(" + Dt.Rows[i]["Ledger"].ToString() + ", 0) + " + Convert.ToDouble(Dt.Rows[i]["Amount"]) + " where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and vno = '" + Dt.Rows[i]["vno"].ToString() + "' and user_Date = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["User_date"])) + "' ";
                    MyBase.Execute(Str);
                }


                MyBase.Load_Data("Select *, replace(Dbo.Ledger_String(Ledger_NAme), '%', '') Ledger from Sales_Tax_head", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = "Update Purchase_reg set " + Dt.Rows[i]["Ledger"].ToString() + " = isnull(" + Dt.Rows[i]["Ledger"].ToString() + ", 0) + " + Convert.ToDouble(Dt.Rows[i]["Amount"]) + " where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and vno = '" + Dt.Rows[i]["vno"].ToString() + "' and user_Date = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["User_date"])) + "' ";
                    MyBase.Execute(Str);
                }

                MyBase.Load_Data("Select *, replace(Dbo.Ledger_String(Ledger_NAme), '%', '') Ledger from Sales_Other_head", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = "Update Purchase_reg set " + Dt.Rows[i]["Ledger"].ToString() + " = isnull(" + Dt.Rows[i]["Ledger"].ToString() + ", 0) + " + Convert.ToDouble(Dt.Rows[i]["Amount"]) + " where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and vno = '" + Dt.Rows[i]["vno"].ToString() + "' and user_Date = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["User_date"])) + "' ";
                    MyBase.Execute(Str);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Create_OutStanding(Boolean Credit, DateTime DtpTo)
        {
            DataTable Dt1 = new DataTable();
            DataTable TempDt = new DataTable();
            DataTable SourceDt = new DataTable();
            DataTable LedgerDt = new DataTable();
            DataTable TDt = new DataTable();
            Double TempAmount = 0, RAmount = 0;
            Int32 j = 0;
            try
            {
                if (MyBase.Check_Table("WOB_Ledger"))
                {
                    MyBase.Execute("Drop table WOB_Ledger");
                }

                if (Credit == true)
                {
                    MyBase.Execute_Qry("select ledger_Code, ledger_name, company_Code, year_code from ledger_Master where ledger_group_code in (select groupcode from groupmas where groupreserved in (4700, 4800)) and (breakup = 'N' or Breakup is null) and company_code = " + CompCode + " and year_Code = '" + YearCode + "'", "WOB_Ledger1");
                    MyBase.Execute_Qry("select v3.ledger_Code Code, v3.ledger_name ledger, v2.vno, v2.user_Date, sum(v1.Credit) Amount, sum(v1.Credit) BalAmount from WOB_Ledger1 v3 left join voucher_details v1 on v3.ledger_Code = v1.ledger_Code and v3.company_Code = v1.company_Code and v3.year_Code = v1.year_Code left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v2.user_date <= '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "' and Credit > 0 group by v2.vno, v2.user_Date, v3.ledger_Code, v3.ledger_name  union select Ledger_Code, ledger_Name, 'Opening', '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', ledger_oCredit, ledger_oCredit from ledger_Master where ledger_group_code in (select groupcode from groupmas where groupreserved = 4700) and (breakup = 'N' or Breakup is null) and company_code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_oCredit > 0 ", "WOB_Ledger_Qry");
                    MyBase.Execute_Tbl("Select * from WOB_Ledger_Qry", "WOB_Ledger");
                    MyBase.Load_Data("Select Distinct Code from WOB_Ledger order by Code ", ref LedgerDt);
                    for (int k = 0; k <= LedgerDt.Rows.Count - 1; k++)
                    {
                        j = 0;

                        MyBase.Load_Data("select v3.ledger_Code Code, v3.ledger_name ledger, v2.vno, v2.user_Date, sum(v1.Credit) Amount from WOB_Ledger1 v3 left join voucher_details v1 on v3.ledger_Code = v1.ledger_Code and v3.company_Code = v1.company_Code and v3.year_Code = v1.year_Code left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where Credit > 0 and v3.ledger_Code = " + LedgerDt.Rows[k]["Code"].ToString() + " group by v2.vno, v2.user_Date, v3.ledger_Code, v3.ledger_name", ref SourceDt);
                        if (SourceDt.Rows.Count > 0)
                        {
                            MyBase.Load_Data("Select sum(ledger_ODebit) Amount from Ledger_Master where ledger_Code = " + LedgerDt.Rows[k]["Code"].ToString() + " and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", ref TDt);
                            if (TDt.Rows[0]["Amount"] != DBNull.Value)
                            {
                                RAmount = Convert.ToDouble(TDt.Rows[0]["Amount"]);
                            }
                            else
                            {
                                RAmount = 0;
                            }
                            MyBase.Load_Data("select (" + RAmount + " + isnull(sum(v1.Debit), 0)) Amount from WOB_Ledger1 v3 left join voucher_details v1 on v3.ledger_Code = v1.ledger_Code and v3.company_Code = v1.company_Code and v3.year_Code = v1.year_Code left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v2.user_date < '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "' and Debit > 0 and v3.ledger_Code = " + LedgerDt.Rows[k]["Code"].ToString(), ref TempDt);
                            if (TempDt.Rows[0]["Amount"] != DBNull.Value)
                            {
                                TempAmount = Convert.ToDouble(TempDt.Rows[0]["Amount"]);
                            }
                            else
                            {
                                TempAmount = 0;
                            }
                            while (TempAmount > 0 && SourceDt.Rows.Count > j)
                            {
                                if (Convert.ToDouble(SourceDt.Rows[j]["Amount"]) > TempAmount)
                                {
                                    MyBase.Execute("UPdate WoB_ledger set Amount = " + Convert.ToDouble(Convert.ToDouble(SourceDt.Rows[j]["Amount"]) - TempAmount) + " where Code = " + SourceDt.Rows[j]["Code"].ToString() + " and vno = '" + SourceDt.Rows[j]["vno"].ToString() + "' and user_Date = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(SourceDt.Rows[j]["user_Date"])) + "'");
                                    SourceDt.Rows[j]["Amount"] = Convert.ToDouble(SourceDt.Rows[j]["Amount"]) - TempAmount;
                                    TempAmount = 0;
                                }
                                else if (Convert.ToDouble(SourceDt.Rows[j]["Amount"]) < TempAmount)
                                {
                                    MyBase.Execute("UPdate WoB_ledger set Amount = 0 where Code = " + SourceDt.Rows[j]["Code"].ToString() + " and Vno = '" + SourceDt.Rows[j]["Vno"].ToString() + "' and user_Date = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(SourceDt.Rows[j]["user_Date"])) + "'");
                                    TempAmount = TempAmount - Convert.ToDouble(SourceDt.Rows[j]["Amount"]);
                                    SourceDt.Rows[j]["Amount"] = 0;
                                    j = j + 1;
                                }
                                else if (Convert.ToDouble(SourceDt.Rows[j]["Amount"]) == TempAmount)
                                {
                                    MyBase.Execute("UPdate WoB_ledger set Amount = 0 where Code = " + SourceDt.Rows[j]["Code"].ToString() + " and vno = '" + SourceDt.Rows[j]["vno"].ToString() + "' and user_Date = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(SourceDt.Rows[j]["user_Date"])) + "'");
                                    SourceDt.Rows[j]["Amount"] = 0;
                                    TempAmount = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    MyBase.Execute_Qry("select ledger_Code, ledger_name, company_Code, year_code from ledger_Master where ledger_group_code in (select groupcode from groupmas where groupreserved in (4700, 4800)) and (breakup = 'N' or Breakup is null) and company_code = " + CompCode + " and year_Code = '" + YearCode + "'", "WOB_Ledger1");
                    MyBase.Execute_Qry(" select v3.ledger_Code Code, v3.ledger_name ledger, v2.vno, v2.user_Date, sum(v1.Debit) Amount, sum(v1.Debit) BalAmount from WOB_Ledger1 v3 left join voucher_details v1 on v3.ledger_Code = v1.ledger_Code and v3.company_Code = v1.company_Code and v3.year_Code = v1.year_Code left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v2.user_date <= '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "' and Debit > 0 group by v2.vno, v2.user_Date, v3.ledger_Code, v3.ledger_name union select Ledger_Code, ledger_Name, 'Opening', '" + String.Format("{0:dd-MMM-yyyy}", SDate.AddDays(-1)) + "', ledger_odebit, Ledger_ODebit from ledger_Master where ledger_Group_Code = 4800 and (breakup = 'N' or Breakup is null) and company_code = " + CompCode + " and year_Code = '" + YearCode + "' and ledger_odebit > 0 ", "WOB_Ledger_Qry");
                    MyBase.Execute_Tbl("Select * from WOB_Ledger_Qry", "WOB_Ledger");
                    MyBase.Load_Data("Select Distinct Code from WOB_Ledger order by Code ", ref LedgerDt);
                    for (int k = 0; k <= LedgerDt.Rows.Count - 1; k++)
                    {
                        j = 0;
                        MyBase.Execute_Qry(" select Code, Ledger, vno, user_date, amount from WOB_Ledger where code = " + LedgerDt.Rows[k]["Code"].ToString() + " and UPPER(Vno) = 'OPENING' union select v3.ledger_Code Code, v3.ledger_name ledger, v2.vno, v2.user_Date, sum(v1.Debit) Amount from WOB_Ledger1 v3 left join voucher_details v1 on v3.ledger_Code = v1.ledger_Code and v3.company_Code = v1.company_Code and v3.year_Code = v1.year_Code left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where Debit > 0 and v3.ledger_Code = " + LedgerDt.Rows[k]["Code"].ToString() + " group by v2.vno, v2.user_Date, v3.ledger_Code, v3.ledger_name ", "T_Qry");
                        MyBase.Load_Data(" select * from T_Qry order by user_date, vno ", ref SourceDt);
                        if (SourceDt.Rows.Count > 0)
                        {
                            MyBase.Load_Data("Select sum(ledger_OCredit) Amount from Ledger_Master where ledger_Code = " + LedgerDt.Rows[k]["Code"].ToString() + " and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", ref TDt);
                            if (TDt.Rows[0]["Amount"] != DBNull.Value)
                            {
                                RAmount = Convert.ToDouble(TDt.Rows[0]["Amount"]);
                            }
                            else
                            {
                                RAmount = 0;
                            }
                            MyBase.Load_Data("select (" + RAmount + " + isnull(sum(v1.Credit), 0)) Amount from WOB_Ledger1 v3 left join voucher_details v1 on v3.ledger_Code = v1.ledger_Code and v3.company_Code = v1.company_Code and v3.year_Code = v1.year_Code left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v2.user_date < '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "' and Credit > 0 and v3.ledger_Code = " + LedgerDt.Rows[k]["Code"].ToString(), ref TempDt);
                            if (TempDt.Rows[0]["Amount"] != DBNull.Value)
                            {
                                TempAmount = Convert.ToDouble(TempDt.Rows[0]["Amount"]);
                            }
                            else
                            {
                                TempAmount = 0;
                            }
                            while (TempAmount > 0 && SourceDt.Rows.Count > j)
                            {
                                if (Convert.ToDouble(SourceDt.Rows[j]["Amount"]) > TempAmount)
                                {
                                    MyBase.Execute("UPdate WoB_ledger set Amount = " + Convert.ToDouble(Convert.ToDouble(SourceDt.Rows[j]["Amount"]) - TempAmount) + " where Code = " + SourceDt.Rows[j]["Code"].ToString() + " and vno = '" + SourceDt.Rows[j]["vno"].ToString() + "' and user_Date = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(SourceDt.Rows[j]["user_Date"])) + "'");
                                    SourceDt.Rows[j]["Amount"] = Convert.ToDouble(SourceDt.Rows[j]["Amount"]) - TempAmount;
                                    TempAmount = 0;
                                }
                                else if (Convert.ToDouble(SourceDt.Rows[j]["Amount"]) < TempAmount)
                                {
                                    MyBase.Execute("UPdate WoB_ledger set Amount = 0 where Code = " + SourceDt.Rows[j]["Code"].ToString() + " and Vno = '" + SourceDt.Rows[j]["Vno"].ToString() + "' and user_Date = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(SourceDt.Rows[j]["user_Date"])) + "'");
                                    TempAmount = TempAmount - Convert.ToDouble(SourceDt.Rows[j]["Amount"]);
                                    SourceDt.Rows[j]["Amount"] = 0;
                                    j = j + 1;
                                }
                                else if (Convert.ToDouble(SourceDt.Rows[j]["Amount"]) == TempAmount)
                                {
                                    MyBase.Execute("UPdate WoB_ledger set Amount = 0 where Code = " + SourceDt.Rows[j]["Code"].ToString() + " and vno = '" + SourceDt.Rows[j]["vno"].ToString() + "' and user_Date = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(SourceDt.Rows[j]["user_Date"])) + "'");
                                    SourceDt.Rows[j]["Amount"] = 0;
                                    TempAmount = 0;
                                }
                            }
                        }
                    }
                }

                MyBase.Execute("Delete from WOB_ledger where Amount = 0");

                if (Credit == true)
                {
                    MyBase.Execute_Qry("select v1.Vcode, v1.vdate, v2.vno, v2.user_date, l1.ledger_code, l1.term, l1.slno, l1.Mode, l1.RefDoc, l1.RefDate, l1.debit, l1.credit, l1.Amount_Cl, l1.OnEdit, l1.company_Code, l1.year_Code from voucher_breakup_bills v1 left join ledger_breakup l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code and v1.refDoc = l1.refDoc and v1.refdate = l1.refdate and v1.mode = l1.mode and v1.credit = l1.credit and v1.debit = l1.debit and v1.vcode = l1.ref left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where l1.ref <> 'L1' and v2.user_date <= '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "'  union select null Vcode, null vdate, null vno, null user_date, l1.ledger_code, l1.term, l1.slno, l1.Mode, l1.RefDoc, l1.RefDate, l1.debit, l1.credit, l1.Amount_Cl, l1.OnEdit, l1.company_Code, l1.year_Code from ledger_breakup l1 where l1.ref = 'L1' and l1.refdate <= '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "' and l1.company_Code = " + CompCode + " and l1.year_Code = '" + YearCode + "'", "Voucher_BBills");
                    Str = "select l1.Ledger_Code Code, l2.ledger_Name Ledger, l1.RefDoc, l1.RefDate, Datediff(D, l1.refdate, '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "') PenDays, l1.Credit Amount,  l1.Credit - (l1.Amount_Cl + l1.OnEdit) Balamount from Voucher_BBills l1 left join ledger_master l2 on l1.ledger_Code = l2.ledger_Code and l1.company_Code = l2.company_Code and l1.year_Code = l2.year_Code where l2.ledger_Group_Code in (select groupcode from groupmas where groupreserved in (4700, 4800)) and l2.breakup = 'Y' and l1.Mode = 'N' and l1.Credit > 0 and l2.company_Code = '" + CompCode + "' and l2.year_Code = '" + YearCode + "' union ";
                    Str += " Select Code, Ledger, vno, user_date, Datediff(D, user_date, '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "') PenDays, BalAmount Amount, Amount as BalAmount from WOB_Ledger ";
                    MyBase.Execute_Qry(Str, "OutSt1");
                }
                else
                {
                    // Correct
                    MyBase.Execute_Qry("select v1.Vcode, v1.vdate, v2.vno, v2.user_date, l1.ledger_code, l1.term, l1.slno, l1.Mode, l1.RefDoc, l1.RefDate, l1.debit, l1.credit, l1.Amount_Cl, l1.OnEdit, l1.company_Code, l1.year_Code from voucher_breakup_bills v1 left join ledger_breakup l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code and v1.refDoc = l1.refDoc and v1.refdate = l1.refdate and v1.mode = l1.mode and v1.credit = l1.credit and v1.debit = l1.debit and v1.vcode = l1.ref left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where l1.ref <> 'L1' and v2.user_date <= '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + YearCode + "' union select null Vcode, null vdate, null vno, null user_date, l1.ledger_code, l1.term, l1.slno, l1.Mode, l1.RefDoc, l1.RefDate, l1.debit, l1.credit, l1.Amount_Cl, l1.OnEdit, l1.company_Code, l1.year_Code from ledger_breakup l1 where l1.ref = 'L1' and l1.refdate <= '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "' and l1.company_Code = " + CompCode + " and l1.year_Code = '" + YearCode + "' ", "Voucher_BBills");
                    // Temp
                    //MyBase.Execute_Qry("select v1.Vcode, v1.vdate, v2.vno, v2.user_date, l1.ledger_code, l1.term, l1.slno, l1.Mode, l1.RefDoc, l1.RefDate, l1.debit, l1.credit, l1.Amount_Cl, l1.OnEdit, l1.company_Code, l1.year_Code from voucher_breakup_bills v1 left join ledger_breakup l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code and v1.refDoc = l1.refDoc and v1.refdate = l1.refdate and v1.mode = l1.mode and v1.credit = l1.credit and v1.debit = l1.debit and v1.ref = l1.ref left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code where v2.user_date < '" + String.Format("{0:dd-MMM-yyyy}", DtpTo.Value) + "' and v1.company_Code = " + MyParent.CompCode + " and v1.year_Code = '" + MyParent.YearCode + "'", "Voucher_BBills");
                    Str = "select l1.Ledger_Code Code, l2.ledger_Name Ledger, l1.RefDoc, l1.RefDate, Datediff(D, l1.refdate, '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "') PenDays, l1.Debit Amount,  l1.Debit - (l1.Amount_Cl + l1.OnEdit) Balamount from Voucher_BBills l1 left join ledger_master l2 on l1.ledger_Code = l2.ledger_Code and l1.company_Code = l2.company_Code and l1.year_Code = l2.year_Code where l2.ledger_Group_Code in (select groupcode from groupmas where groupreserved in (4800, 4700)) and l2.breakup = 'Y' and l1.Mode = 'N' and l1.Debit > 0 and l2.company_Code = '" + CompCode + "' and l2.year_Code = '" + YearCode + "' union ";
                    Str += " Select Code, Ledger, vno, user_date, Datediff(D, user_date, '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "') PenDays, BalAmount Amount, Amount BalAmount from WOB_Ledger ";
                    MyBase.Execute_Qry(Str, "OutSt1");
                }
                MyBase.Execute_Qry("Select top 100000000000 * from OutSt1 order by Ledger, RefDate, RefDoc", "OutSt2");

                Str = "Select top 100000000000 * from outSt2 where balamount > 0 order by Ledger, RefDate, RefDoc";
                MyBase.Execute_Qry(Str, "OutSt3");
                Str = "Select 1 as Slno, Code, Ledger, RefDoc, RefDate, Pendays, Amount, BalAmount, Null TAmount from OutSt3 union ";
                Str += "select 2 as Slno, Code, Ledger, Null, null, null, null, null, Sum(BalAmount) TAmount from outSt2 where balamount > 0 group by Code, Ledger";
                MyBase.Execute_Qry(Str, "OutSt4");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void criteriaReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Criteria();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Object reference not set to an instance of an object"))
                {
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        public void PANDL(DateTime DatFrom, DateTime DatTo)
        {
            String Str = String.Empty;
            try
            {
                Main_Table(DatFrom, DatTo);
                Str = "Select top 10000000000 * from Group_View order by Group_";
                MyBase.Execute_Qry(Str, "RPT_GRP_last");
                MyBase.Execute_Qry("select r1.*, g1.groupreserved from RPT_GRP_last r1 left join groupmas g1 on r1.code = g1.groupcode where g1.company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", "RPT_GRP_last_1");

                MyBase.Execute_Qry("select top 1000000000000 head_Code, subhead_code, r1.code groupcode, '        ' + r1.group_ Group_, isnull(r1.debit, 0) debit, isnull(r1.credit, 0) Credit, 0 as Balance, g1.order_slno, g1.Mode from groupmas_Setting g1 left join balance_Subheading_master_V b1 on g1.subhead_Code = b1.code left join RPT_GRP_last_1 r1 on g1.groupcode = r1.groupreserved where g1.type = 'P' and g1.vorh = 'V' order by subhead_Code, g1.order_Slno", "Grp_Bal_1");
                MyBase.Execute_Tbl("Select * from Grp_Bal_1", "Grp_Bal");

                MyBase.Execute_Qry("Select head_Code, subhead_Code, sum(debit) debit, sum(Credit) Credit, (case when sum(debit) > sum(credit) then sum(debit) - Sum(Credit) else sum(credit) - sum(Debit) end) Balance, (case when sum(debit) > sum(credit) then 'Dr' else 'Cr' end) Mode from Grp_Bal v1 group by head_Code, subhead_Code", "SubH_Bal_1");
                MyBase.Execute_Tbl("Select * from SubH_Bal_1", "SubH_Bal");

                MyBase.Execute_Qry("Select head_Code, 0 debit, 0 Credit, sum(Balance) Balance from SubH_Bal v1 group by head_Code", "Head_Bal");

                Str = " select * from Grp_Bal union ";
                Str += " select v2.head_Code, v2.subhead_Code, v2.subhead_Code Code, '   ' + v1.subhead_name subhead_name, 0, 0, v2.balance, 1, null from SubH_Bal v2 left join PandL_subheading_master_v v1 on v2.subhead_Code = v1.code union ";
                Str += " select v3.head_Code, 1000, v3.head_Code Code, null, 0, 0, v3.balance, 1000, null from Head_Bal v3 left join PandL_heading_master_v v1 on v3.head_Code = v1.code union ";
                Str += " select v3.head_Code, v3.head_Code, v3.head_Code Code, v1.head_name, 0, 0, 0, 0, null from Head_Bal v3 left join PandL_heading_master_v v1 on v3.head_Code = v1.code ";
                MyBase.Execute_Qry(Str, "Bal_Final_1");

                Str = "select head_Code, subhead_Code, groupcode, group_, (case when debit = 0 then null else cast(debit as numeric(20,2)) end) Debit, (case when credit = 0 then null else cast(credit as numeric(20,2)) end) Credit, (case when balance = 0 then null else cast(balance as numeric(20,2)) end) Balance, Order_Slno, Mode from Bal_Final_1 v4 ";
                MyBase.Execute_Qry(Str, "Bal_Final_Last");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Main_Table()
        {
            try
            {
                MyBase.Current_Balance(0, SDate, CompCode, YearCode, true);
                MyBase.Execute_Qry("select c1.*, l1.ledger_group_Code from curbal c1 left join ledger_master l1 on c1.ledger_Code = l1.Ledger_Code where l1.company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", "Tbl_Gr_Bal");
                MyBase.Execute_Qry("select ledger_group_Code, Sum(Bal_amount) as Debit, 0 as Credit from Tbl_Gr_Bal where Mode = 'Dr' group By ledger_group_Code union select ledger_group_Code, 0 as Debit, Sum(Bal_amount) as Credit from Tbl_Gr_Bal where Mode = 'Cr' group By ledger_group_Code", "Tbl_Gr_Bal1");
                MyBase.Execute_Qry("Select ledger_group_Code, sum(Debit) Debit, sum(credit) Credit from Tbl_Gr_Bal1 group by ledger_group_code", "tbl_gr_bal11");
                MyBase.Execute_Qry("Select ledger_group_Code, (case when debit = 0 then null else debit end) debit, (case when Credit = 0 then null else Credit end) Credit from tbl_Gr_Bal11", "tbl_Gr_Bal2");
                MyBase.Execute_Tbl("Select GroupCode, GroupName from GroupMas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", "GroupMas_Comp");
                MyBase.Execute_Tbl("select g1.GroupName Group_, g1.GroupCode Code, (case when isnull(sum(t1.Debit),0) > isnull(sum(t1.Credit), 0) then isnull(sum(t1.debit), 0) - isnull(sum(t1.credit), 0) else null end) Debit, (case when isnull(sum(t1.credit),0) > isnull(sum(t1.debit), 0) then isnull(sum(t1.Credit), 0) - isnull(sum(t1.debit), 0) else null end) Credit from GroupMas_Comp g1 left join Tbl_Gr_Bal2 t1 on g1.GroupCode = t1.ledger_group_Code group by g1.GroupName, g1.GroupCode order by g1.GroupName", "Group_View");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public void Update_Opening_Stock_PANDL(DateTime FromDate, DateTime ToDate, int Compcode, String Year_Code)
        {
            String Str = String.Empty;
            try
            {

                Str = "Select * from Tbl_Gr_Bal where ledger_group_code Not in (Select groupcode from groupmas where company_Code = " + Compcode + " and year_Code = '" + YearCode + "' and ((groupreserved = 3400) or (Groupreserved = 1800))) union ";
                // Opening
                Str += "select ledger_Code, (case when Debit > 0 then cast(Debit as varchar(15)) + ' Dr' else cast(Credit as varchar(15)) + ' Cr' end) Balance, (case when Debit > 0 then Debit else Credit end) Bal_Amount, (case when Debit > 0 then 'Dr' else 'Cr' end) Mode, 3400 from closing_Stock where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "' and edate = (Select Max(Edate) from closing_Stock where Edate < '" + String.Format("{0:dd-MMM-yyyy}", FromDate) + "' and company_Code = " + Compcode + " and year_Code = '" + Year_Code + "') union ";
                // Closing
                Str += "select ledger_Code, (case when Debit > 0 then cast(Debit as varchar(15)) + ' Cr' else cast(Credit as varchar(15)) + ' Cr' end) Balance, (case when Debit > 0 then Debit else Credit end) Bal_Amount, (case when Debit > 0 then 'Cr' else 'Cr' end) Mode, 1800 from closing_Stock where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "' and edate = (Select Max(Edate) from closing_Stock where Edate <= '" + String.Format("{0:dd-MMM-yyyy}", ToDate) + "' and Edate >= '" + String.Format("{0:dd-MMM-yyyy}", FromDate) + "' and company_Code = " + Compcode + " and year_Code = '" + Year_Code + "')";
                MyBase.Execute_Qry(Str, "Tbl_Gr_Bal_Last");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_Opening_Stock_Balance(DateTime FromDate, DateTime ToDate, int Compcode, String Year_Code)
        {
            String Str = String.Empty;
            try
            {

                Str = "Select * from Tbl_Gr_Bal where ledger_group_code Not in (Select groupcode from groupmas where company_Code = " + Compcode + " and year_Code = '" + YearCode + "' and ((groupreserved = 3400) or (Groupreserved = 1800))) union ";
                // Opening
                Str += "select ledger_Code, (case when Debit > 0 then cast(Debit as varchar(15)) + ' Dr' else cast(Credit as varchar(15)) + ' Cr' end) Balance, (case when Debit > 0 then Debit else Credit end) Bal_Amount, (case when Debit > 0 then 'Dr' else 'Cr' end) Mode, 3400 from closing_Stock where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "' and edate = (Select Max(Edate) from closing_Stock where Edate < '" + String.Format("{0:dd-MMM-yyyy}", FromDate) + "' and company_Code = " + Compcode + " and year_Code = '" + Year_Code + "') union ";
                // Closing
                Str += "select ledger_Code, (case when Debit > 0 then cast(Debit as varchar(15)) + ' Dr' else cast(Credit as varchar(15)) + ' Cr' end) Balance, (case when Debit > 0 then Debit else Credit end) Bal_Amount, (case when Debit > 0 then 'Dr' else 'Cr' end) Mode, 1800 from closing_Stock where Company_Code = " + Compcode + " and year_Code = '" + Year_Code + "' and edate = (Select Max(Edate) from closing_Stock where Edate < '" + String.Format("{0:dd-MMM-yyyy}", ToDate) + "' and Edate >= '" + String.Format("{0:dd-MMM-yyyy}", FromDate) + "' and company_Code = " + Compcode + " and year_Code = '" + Year_Code + "')";
                MyBase.Execute_Qry(Str, "Tbl_Gr_Bal_Last");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void Main_Table(DateTime DtFrom, DateTime DtpTo)
        {
            try
            {
                MyBase.Current_Balance_In_Period(0, SDate, DtFrom, DtpTo, CompCode, YearCode, true);
                MyBase.Execute_Qry("select c1.*, l1.ledger_group_Code from curbal c1 left join ledger_master l1 on c1.ledger_Code = l1.Ledger_Code where l1.company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", "Tbl_Gr_Bal");

                Update_Opening_Stock_PANDL(DtFrom, DtpTo, CompCode, YearCode);

                MyBase.Execute_Qry("select ledger_group_Code, Sum(Bal_amount) as Debit, 0 as Credit from Tbl_Gr_Bal_Last where Mode = 'Dr' group By ledger_group_Code union select ledger_group_Code, 0 as Debit, Sum(Bal_amount) as Credit from Tbl_Gr_Bal_Last where Mode = 'Cr' group By ledger_group_Code", "Tbl_Gr_Bal1");
                MyBase.Execute_Qry("Select ledger_group_Code, sum(Debit) Debit, sum(credit) Credit from Tbl_Gr_Bal1 group by ledger_group_code", "tbl_gr_bal11");
                MyBase.Execute_Qry("Select ledger_group_Code, (case when debit = 0 then null else debit end) debit, (case when Credit = 0 then null else Credit end) Credit from tbl_Gr_Bal11", "tbl_Gr_Bal2");
                MyBase.Execute_Tbl("Select GroupCode, GroupName from GroupMas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", "GroupMas_Comp");
                MyBase.Execute_Tbl("select g1.GroupName Group_, g1.GroupCode Code, (case when isnull(sum(t1.Debit),0) > isnull(sum(t1.Credit), 0) then isnull(sum(t1.debit), 0) - isnull(sum(t1.credit), 0) else null end) Debit, (case when isnull(sum(t1.credit),0) > isnull(sum(t1.debit), 0) then isnull(sum(t1.Credit), 0) - isnull(sum(t1.debit), 0) else null end) Credit from GroupMas_Comp g1 left join Tbl_Gr_Bal2 t1 on g1.GroupCode = t1.ledger_group_Code group by g1.GroupName, g1.GroupCode order by g1.GroupName", "Group_View");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Main_Table_Balance(DateTime DtFrom, DateTime DtpTo)
        {
            try
            {
                MyBase.Current_Balance_In_Period(0, SDate, DtFrom, DtpTo, CompCode, YearCode, true);
                MyBase.Execute_Qry("select c1.*, l1.ledger_group_Code from curbal c1 left join ledger_master l1 on c1.ledger_Code = l1.Ledger_Code where l1.company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", "Tbl_Gr_Bal");

                Update_Opening_Stock_PANDL(DtFrom, DtpTo, CompCode, YearCode);

                MyBase.Execute_Qry("select ledger_group_Code, Sum(Bal_amount) as Debit, 0 as Credit from Tbl_Gr_Bal_Last where Mode = 'Dr' group By ledger_group_Code union select ledger_group_Code, 0 as Debit, Sum(Bal_amount) as Credit from Tbl_Gr_Bal_Last where Mode = 'Cr' group By ledger_group_Code", "Tbl_Gr_Bal1");
                MyBase.Execute_Qry("Select ledger_group_Code, sum(Debit) Debit, sum(credit) Credit from Tbl_Gr_Bal1 group by ledger_group_code", "tbl_gr_bal11");
                MyBase.Execute_Qry("Select ledger_group_Code, (case when debit = 0 then null else debit end) debit, (case when Credit = 0 then null else Credit end) Credit from tbl_Gr_Bal11", "tbl_Gr_Bal2");
                MyBase.Execute_Tbl("Select GroupCode, GroupName from GroupMas where company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", "GroupMas_Comp");
                MyBase.Execute_Tbl("select g1.GroupName Group_, g1.GroupCode Code, (case when isnull(sum(t1.Debit),0) > isnull(sum(t1.Credit), 0) then isnull(sum(t1.debit), 0) - isnull(sum(t1.credit), 0) else null end) Debit, (case when isnull(sum(t1.credit),0) > isnull(sum(t1.debit), 0) then isnull(sum(t1.Credit), 0) - isnull(sum(t1.debit), 0) else null end) Credit from GroupMas_Comp g1 left join Tbl_Gr_Bal2 t1 on g1.GroupCode = t1.ledger_group_Code group by g1.GroupName, g1.GroupCode order by g1.GroupName", "Group_View");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            try
            {
                Criteria();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void Gainup_Old_Datas_Update()
        {
            try
            {

                if (MyBase.Check_Table("WO_Stores") == false)
                {
                    //Gainup Sales
                    MyBase.Execute("Insert into ERP_Accounts_Sales select v1.vno, v1.user_Date, i1.compcode, substring(v1.year_Code, 1,4), v1.vcode, v1.vdate, v1.company_Code, v1.year_Code from voucher_master V1 left join vaahini_erp_gainup.dbo.invoicemas i1 on v1.vno = i1.invoiceno and v1.user_date = i1.invoicedt and substring(v1.year_Code, 1,4) = i1.yearcode where vmode = 5 and vtype = 'sales' and i1.compcode is not null");
                    MyBase.Execute("update voucher_master set voucher_type = 'S' from voucher_master v1, erp_Accounts_sales e1 where v1.vno = e1.invoiceno and v1.vdate = e1.vdate and v1.company_Code = e1.company_Code and v1.year_Code = e1.year_Code and v1.vcode = e1.vcode and v1.vmode = 5 and vtype = 'sales'");

                    //Broker Commission
                    MyBase.Execute_Qry("select vcode, vdate, company_Code, year_Code from voucher_details where ledger_code in (Select distinct ledger_Code from ledger_master where ledger_group_code = 6300) and byto ='BY'", "Old_Br_Comm");
                    MyBase.Execute("Insert into ERP_ACCOUNTS_SALES_Broker select v1.vno, v1.user_Date, v1.division_code, substring(v1.year_Code, 1,4), v1.vcode, v1.vdate, v1.company_Code, v1.year_Code from voucher_master V1, Old_Br_Comm v2 where v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code and v1.vmode = 4 ");
                    MyBase.Execute("update voucher_master set voucher_type = 'S-BR' from voucher_master v1, erp_Accounts_sales_Broker e1 where v1.vno = e1.invoiceno and v1.vdate = e1.vdate and v1.company_Code = e1.company_Code and v1.year_Code = e1.year_Code and v1.vcode = e1.vcode and v1.vmode = 4");

                    //Waste Sales
                    MyBase.Execute("Insert into ERP_Accounts_Waste_Sales select v1.vno, v1.user_Date, i1.compcode, substring(v1.year_Code, 1,4), v1.vcode, v1.vdate, v1.company_Code, v1.year_Code from voucher_master V1 left join vaahini_erp_gainup.dbo.it_wasmas i1 on v1.vno = i1.invoiceno and v1.user_date = i1.invoicedt and substring(v1.year_Code, 1,4) = i1.yearcode where vmode = 5 and vtype = 'sales' and i1.compcode is not null");
                    MyBase.Execute("Update voucher_master set voucher_type = 'S-W' from voucher_master v1, ERP_Accounts_Waste_Sales e1 where v1.vdate = e1.vdate and v1.company_Code = e1.company_Code and v1.year_Code = e1.year_Code and v1.vcode = e1.vcode ");


                    //Fabric Recipt
                    MyBase.Execute("Update voucher_master set Vtype = 'Sales', voucher_type = 'F' where vmode = 8");
                    MyBase.Execute("Insert into ERP_Accounts_Waste_Sales select v1.vno, v1.user_Date, v1.division_Code, substring(v1.year_Code, 1,4), v1.vcode, v1.vdate, v1.company_Code, v1.year_Code from voucher_master V1 where vmode = 8 and vtype = 'sales'");

                    //Cotton
                    MyBase.Execute("Update voucher_master set vtype = 'Cotton' where vmode = 6 and vtype = 'Others' and remarks like 'For Lot%'");
                    MyBase.Execute("Update voucher_master set voucher_type = 'CO' where vmode = 6 and vtype = 'Cotton'");
                    MyBase.Execute("Insert into ERP_Accounts_Cotton select v1.vno, v1.user_Date, v1.division_Code, substring(v1.year_Code, 1,4), v1.vcode, v1.vdate, v1.company_Code, v1.year_Code from voucher_master V1 where vmode = 6 and vtype = 'Cotton'");

                    MyBase.Execute("Create table WO_Stores (No int)");
                }

                // Stores GRN

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Update_Ledger_Code_Field(String TblName, String FieldName, String Module)
        {
            int Val = 0;
            try
            {
                if (Module.ToUpper() == "SALES")
                {
                    Val = 3500;
                }
                else if (Module.ToUpper() == "STORES")
                {
                    Val = 2000;
                }
                else if (Module.ToUpper() == "COTTON")
                {
                    Val = 4500;
                }
                if (MyBase.Get_RecordCount_OtherDB(ERP_DBName, TblName, FieldName + " < " + Val) > 0)
                {
                    MyBase.Execute("update " + ERP_DBName + ".dbo." + TblName + " set " + FieldName + " = " + FieldName + " + " + Val + "  where " + FieldName + " < " + Val);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Double Get_OutPut()
        {
            //try
            //{
            //    FrmCalculator_Voucher Frm = new FrmCalculator_Voucher();
            //    Frm.StartPosition = FormStartPosition.CenterParent;
            //    Frm.ShowDialog();
            //    return Frm.Answer;
            //}
            //catch (Exception ex)
            //{
                return 0;
            //}
        }



        public void Get_Calculator(MDIMain Parent)
        {
            try
            {
                FrmCalculator Frm = new FrmCalculator();
                Frm.StartPosition = FormStartPosition.Manual;
                Frm.Height = 112;
                Frm.Top = (this.Height - (Frm.Height)) - 80;
                Frm.Left = this.Left + 10;
                Frm.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void permissionMasterToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            try
            {
                if (CompName.ToUpper().Contains("AEGAN"))
                {
                    //ShowChild(new FrmProjectsPermissionMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
                }
                else
                {
                    if (CompCode == 1 || CompCode == 2)
                    {
                        //ShowChild(new FrmPermissionMaster(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
                        //ShowChild(new Frm_Socks_Permission_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        private void toolStripMenuItem10_Click_1(object sender, EventArgs e)
        {
            try
            {
                Get_Calculator(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Get_Min_Date(String Voucher)
        {
            DataTable Dt = new DataTable();
            try
            {
                Min_Days = Convert.ToInt32(MyBase.GetData_InNumber("Min_Lock_Date", "Voucher", Voucher, "Days"));
                if (Min_Days == 0)
                {
                    Min_Date = MyBase.GetServerDate();
                }
                else
                {
                    MyBase.Load_Data("Select GetDate() - " + Min_Days + " ", ref Dt);
                    if (Dt.Rows[0][0] == null || Dt.Rows[0][0] == DBNull.Value)
                    {
                        Min_Date = MyBase.GetServerDate();
                    }
                    else
                    {
                        Min_Date = Convert.ToDateTime(String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[0][0])));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MDIMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (Security_Flag == false)
                {
                    if (Update_Flag == false)
                    {
                        if (Sign_Out())
                        {
                            this.Dispose();
                            this.Close();
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem3_Click_1(object sender, EventArgs e)
        {
            try
            {
                MyBase.Execute_Qry("select top 1000000000 Ledger_Code, Ledger_Name, g1.GroupName, Ledger_Address, Ledger_Area_Code, Ledger_phone, Ledger_Fax, Ledger_email, Ledger_Website, Ledger_Tin, Ledger_CST, Cheque_Name, PanNo from ledger_Master l1 left join groupmas g1 on l1.company_Code = g1.company_Code and l1.year_Code = g1.year_Code and l1.ledger_group_code = g1.groupCode where l1.company_Code = " + CompCode + " and l1.year_Code = '" + YearCode + "'  and g1.groupcode in (select groupcode from groupmas where groupreserved in (4800, 4700) and company_code = " + CompCode + " and year_Code = '" + YearCode + "') and l1.ledger_Name is not null order by l1.ledger_Name", "rpt_ledger_Address");
                CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RptLedgerAddress.rpt");
                FormulaFill(ref ObjRpt, "CompName", CompName);
                FormulaFill(ref ObjRpt, "title", "LEDGER ADDRESS PRINTING AS ON " + string.Format("{0:dd/MM/yyyy}", DateTime.Now));
                CReport(ref ObjRpt, "Ledger Address Printing ...!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Dhana_Ledger_Updates()
        {
            DataTable Dt = new DataTable();
            String Str = String.Empty;
            Int32 Ledger_Code = 0;
            try
            {
                Str = " select top 100000000 * from dhana_Accounts.dbo.ledger_Master where ledger_Name not in (Select Ledger_Name from ledger_Master) and ledger_Name not like '%Advance%' order by ledger_Name ";
                MyBase.Execute_Qry(Str, "DHA_Ledger_Update_Tally");

                Str = "Select Ledger_Code from DHA_Ledger_Update_Tally order by ledger_Code desc";
                MyBase.Load_Data(Str, ref Dt);

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Ledger_Code = Convert.ToInt32(MyBase.MaxOnlyComp("Ledger_Master", "Ledger_Code", String.Empty, YearCode, CompCode));
                    MyBase.Execute("Insert into Ledger_Master(ledger_Code, Ledger_Name, Ledger_Title, Ledger_InPrint, Ledger_Group_Code, ledger_Odebit, Ledger_Ocredit, ledger_Address, Ledger_Area_Code, Ledger_Phone, Ledger_Fax, Ledger_Tin, Ledger_CST, Company_Code, Year_Code, Tax_Per, Breakup, Cheque_Name, Panno, TDSApplicable) select " + Ledger_Code + ", Ledger_Name, Ledger_Title, Ledger_InPrint, Ledger_Group_Code, ledger_Odebit, Ledger_Ocredit, ledger_Address, Ledger_Area_Code, Ledger_Phone, Ledger_Fax, Ledger_Tin, Ledger_CST, Company_Code, Year_Code, Tax_Per, Breakup, Cheque_Name, Panno, TDSApplicable  from DHA_Ledger_Update_Tally where Ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_Voucher_Approval_Balance()
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Load_Data("Select * from Voucher_Details where Approval = 'False' and ((Debit > 0) or (Credit > 0)) and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["Debit"] != null && Dt.Rows[i]["Debit"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["Debit"]) > 0)
                    {
                        MyBase.Execute("Update voucher_Details set Debit = 0, other1 = '[* " + string.Format("{0:0.00}", Convert.ToDouble(Dt.Rows[i]["Debit"])) + " Dr *]', Narration = '[* " + string.Format("{0:0.00}", Convert.ToDouble(Dt.Rows[i]["Debit"])) + " Dr *] ' + Narration  where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["Vdate"])) + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and Slno = " + Dt.Rows[i]["Slno"].ToString() + " and Debit = " + Dt.Rows[i]["Debit"].ToString());
                    }
                    else if (Dt.Rows[i]["Credit"] != null && Dt.Rows[i]["Credit"] != DBNull.Value && Convert.ToDouble(Dt.Rows[i]["Credit"]) > 0)
                    {
                        MyBase.Execute("Update voucher_Details set Credit = 0, other1 = '[* " + string.Format("{0:0.00}", Convert.ToDouble(Dt.Rows[i]["Credit"])) + " Cr *]',  Narration = '[* " + string.Format("{0:0.00}", Convert.ToDouble(Dt.Rows[i]["Credit"])) + " Cr *] ' + Narration  where company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["Vdate"])) + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and Slno = " + Dt.Rows[i]["Slno"].ToString() + " and Credit = " + Dt.Rows[i]["Credit"].ToString());
                    }
                }


                MyBase.Load_Data("Select * from Voucher_Details where Approval = 'True' and Debit = 0 and Credit = 0 and company_Code = " + CompCode + " and year_Code = '" + YearCode + "'", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Run(" update voucher_Details set Approval = 'True', Narration = rtrim(ltrim(replace(Narration, other1, ''))), Debit = replace(replace(rtrim(ltrim(replace(replace(replace(Other1, '*', ''), '[', ''), ']', ''))), 'Dr', ''), 'Cr', '') where vcode = " + Dt.Rows[i]["vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and Other1 like '%Dr%'", " update voucher_Details set Approval = 'True', Narration = rtrim(ltrim(replace(Narration, other1, ''))), Credit = replace(replace(rtrim(ltrim(replace(replace(replace(Other1, '*', ''), '[', ''), ']', ''))), 'Dr', ''), 'Cr', '') where vcode = " + Dt.Rows[i]["vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and company_Code = " + CompCode + " and year_Code = '" + YearCode + "' and Other1 like '%Cr%'");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void toolStripMenuItem20_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmPermissionMaster_User_Level_Fixed(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            int Comp = 0;
            String Year1 = String.Empty;
            try
            {
                if (this.ActiveMdiChild == null)
                {
                    FrmCompany_ChangeOver Frm = new FrmCompany_ChangeOver();
                    Frm.StartPosition = FormStartPosition.CenterScreen;
                    Frm.CompName = CompName;
                    Frm.User_Code = Convert.ToInt16(UserCode);
                    Frm.ShowDialog();
                    if (Frm.Company_Code != 0)
                    {
                        Comp = Frm.Company_Code;
                        Year1 = Frm.Year_Code;
                        if ((CompCode != Comp) || (Year1.Trim() != YearCode))
                        {
                            Company_ChangeOver(Comp, Year1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void toolStripMenuItem22_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    if (this.ActiveMdiChild.Name == "FrmCRViewer")
                    {
                        FrmCRViewer Frm = (FrmCRViewer)this.ActiveMdiChild;
                        Frm.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void BIll_Passing_updation()
        {
            DataTable Dt = new DataTable();
            try
            {
                MyBase.Execute_Qry("select distinct grnno, grndate, billpno, billpdate from " + ERP_DBName + ".dbo.grndetail e1 where billpno is not null and billpno <> 0 ", "Billp_GRN");
                MyBase.Load_Data("select v1.billpno, v1.billpdate, v1.grnno, v1.grndate, e1.billpno from Billp_GRN v1 left join erp_accounts_stores e1 on v1.billpno = e1.billpno and v1.billpdate = e1.billpdate where e1.billpno is null order by v1.billpno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Execute("Update " + ERP_DBName + ".dbo.GRNDetail set Billpno = 0, billpdate = null where grnno = " + Dt.Rows[i]["Grnno"].ToString() + " and grndate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["grndate"])) + "'");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void billPassingUpdationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Sure to Update ...!", "Vaahini", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    BIll_Passing_updation();
                    MessageBox.Show("Ok ...!");
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }


        Boolean Data_Verify()
        {
            try
            {
                MyBase.Execute("Exec Data_Verify_Insert 1, '" + YearCode + "', " + CompCode);
                MyBase.Execute("Exec Data_Verify_Update 1, '" + YearCode + "', " + CompCode);
                MyBase.Execute("Exec Data_Verify_Delete 1, '" + YearCode + "', " + CompCode);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void toolStripMenuItem27_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select F1.Order_No, F1.OrderColorID, F1.SizeID, F1.KnitQty, F2.Production From Floor_Stock F1 Inner join (Select Order_No, OrderColorID, SizeID, Sum(Production) Production From Floor_Knitting_DEtails Group By Order_No, OrderColorID, SizeID) F2 on F1.Order_No = F2.Order_No and F1.OrderColorID = F2.OrderColorID and F1.SizeID = F2.SizeID  Where F1.KnitQty <> F2.Production ", ref Tdt);
                MessageBox.Show("Difference is : " + Tdt.Rows.Count.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Cancel()
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    if (this.ActiveMdiChild.Tag != null)
                    {
                        MyBase.Clear(this.ActiveMdiChild);
                        MenuButton_Status("Form");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                Entry_Cancel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Table_Creation_DO_Details()
        {
            try
            {
                if (MyBase.Check_Table("DO_Master") == false)
                {
                    MyBase.Execute("Create Table DO_Master (DO_ID int identity, DO_Code int, DO_Date Date, Party_Code int, Cash_Party_Name varchar(100), Narration varchar(2000), Due_days Int, Amount Numeric(20,2), Stkwarehouse_ID int, Emp_Code int, Sys_Code int,Entry_At datetime, Company_Code int,Year_Code varchar(10) Constraint PK_DO_Code Primary key (DO_Code, Company_Code, Year_Code), Constraint FK_Stkware_ID foreign key(Stkwarehouse_ID, company_Code, year_Code) references mas_stkwarehouse(Stkwarehouse_ID, Company_Code, year_Code))");
                }

                if (MyBase.Check_Table("DO_Details") == false)
                {
                    MyBase.Execute(" Create Table DO_Details (DO_Det_ID int identity, DO_Code int, DO_Date Date, I_Slno int, Item_Id int, Pcs_Bundle Char(1), Qty1 Numeric(20, 5), Qty2 Numeric(20, 5), Qty_Approx float, Rate Numeric(20,2), Amount Numeric(20,2), Company_Code int,Year_Code varchar(10), Constraint FK_DO_Code Foreign key(Do_Code, company_Code, Year_Code) references Do_Master(DO_Code, Company_Code, year_Code)) ");
                }

                if (MyBase.Check_Procedure("Ins_DO_Master") == false)
                {
                    MyBase.Execute("Create Proc Ins_DO_Master (@DO_Date date, @Party_Code int, @Cash_Party_Name varchar(100), @Narration varchar(2000), @Due_Days int, @Amount int, @Stkwarehouse_ID int, @EMP_Code int, @Sys_Code int, @Entry_At Datetime, @Company_Code int, @Year_Code varchar(10)) as Begin	set nocount on;	Begin try 			declare @DO_Code int; 		begin transaction;			select @DO_Code = (isnull(Max(Do_Code), 0) + 1) from DO_Master where Company_Code = @Company_Code; 			insert into do_master (DO_Code, Do_date, Party_Code, cash_party_Name, Narration, Due_days, Amount, Stkwarehouse_Id, EMp_Code, Sys_Code, Entry_at, Company_Code, year_Code) values (@DO_Code, @DO_Date, @Party_Code, @Cash_Party_Name, @Narration, @Due_Days, @Amount, @Stkwarehouse_Id, @Emp_Code, @Sys_Code, @Entry_At, @Company_Code, @Year_Code);			Commit transaction; 		Select @DO_Code;		end try Begin catch 			rollback transaction;			Declare @ESev int, @ESt as int, @EM as nvarchar(4000);	Select @ESev = Error_Severity(), @ESt = Error_State(), @EM = Error_Message();			raiserror (@EM, @Esev, @ESt);	End catch End");
                }

                if (MyBase.Check_Procedure("UPD_DO_Master") == false)
                {
                    MyBase.Execute("Create Proc UPD_DO_Master (@DO_Code int, @DO_Date Date, @Party_Code int, @Cash_Party_Name varchar(100), @Narration varchar(2000), @Due_days int, @Amount NUmeric(20,2), @Stkwarehouse_ID int, @EMP_Code int, @Sys_Code int, @Entry_At Datetime, @Company_Code int, @Year_Code varchar(10)) as Begin	set nocount on;	Begin try 		begin transaction;		Update DO_Master set DO_Date = @Do_Date, Narration = @narration, Due_days = @Due_Days, AMount = @Amount, Stkwarehouse_ID = @Stkwarehouse_ID, Party_Code = @Party_Code, cash_party_Name = @cash_party_Name, EMp_Code = @EMp_Code, Sys_Code = @Sys_Code, Entry_at = @Entry_at Where DO_Code = @DO_Code and Company_Code = @Company_Code and Year_Code = @Year_Code;		select @DO_Code;		Commit transaction;	end try	Begin catch		rollback transaction;		Declare @ESev int, @ESt as int, @EM as nvarchar(4000);		Select @ESev = Error_Severity(), @ESt = Error_State(), @EM = Error_Message();		raiserror (@EM, @Esev, @ESt);	End catch End");
                }

                if (MyBase.Check_Procedure("INS_DO_Details") == false)
                {
                    MyBase.Execute("Create proc INS_DO_Details (@DO_Code int, @DO_Date date, @I_SLno int, @Item_ID int, @PCS_Bundle char(1), @Qty1 Numeric(20, 6), @Qty2 Numeric(20, 6), @QTy_Approx float, @Rate NUmeric(20,2), @Amount Numeric(20,2), @Company_Code int, @Year_Code varchar(10)) as Begin 	begin try  		set nocount on;  		Begin transaction;   		insert into DO_Details (DO_Code, Do_date, I_Slno, Item_Id, PCS_bundle, Qty1, Qty2, Qty_Approx, Rate, Amount, Company_Code, Year_Code) values (@DO_Code, @Do_date, @I_Slno, @Item_Id, @PCS_bundle, @Qty1, @Qty2, @QTy_Approx, @Rate, @Amount, @Company_Code, @Year_Code);  		Commit Transaction; 	End try 	begin catch   		rollback transaction;  		declare @M Nvarchar(4000), @Sev int, @St int;  		Select @M = ERROR_MESSAGE(), @Sev = Error_Severity(), @St = Error_State();  		raiserror (@M, @Sev, @St); 	End catch end");
                }

                if (MyBase.Check_Procedure("Del_DO_Details") == false)
                {
                    MyBase.Execute("create proc Del_DO_Details (@DO_Code int, @Company_Code int, @Year_Code varchar(10)) as Begin	begin try		set nocount on;		Begin transaction;			delete from do_Details where do_Code = @Do_Code and company_Code = @Company_Code and Year_Code = @Year_Code;		Commit Transaction;	End try	begin catch 		rollback transaction;		declare @M Nvarchar(4000), @Sev int, @St int;		Select @M = ERROR_MESSAGE(), @Sev = Error_Severity(), @St = Error_State();		raiserror (@M, @Sev, @St);	End catch end");
                }

                if (MyBase.Check_Procedure("Del_DO") == false)
                {
                    MyBase.Execute("create proc Del_DO (@DO_Code int, @Company_Code int, @Year_Code varchar(10)) as Begin	begin try		set nocount on;		Begin transaction;			delete from DO_Details where do_Code = @Do_Code and company_Code = @Company_Code and Year_Code = @Year_Code;			delete from DO_Master where do_Code = @Do_Code and company_Code = @Company_Code and Year_Code = @Year_Code;		Commit Transaction;	End try	begin catch 		rollback transaction;		declare @M Nvarchar(4000), @Sev int, @St int;		Select @M = ERROR_MESSAGE(), @Sev = Error_Severity(), @St = Error_State();		raiserror (@M, @Sev, @St);	End catch end");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void toolStripMenuItem32_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripButton1_Click_1(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void categoryMasterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
        }

        public void Table_Creation_Group()
        {
            try
            {
                if (MyBase.Check_Table("Mas_ItemGroup") == false)
                {
                    MyBase.Execute("create table Mas_ItemGroup (RowID int Identity, ItemGroup_Id int, ItemGroup_Name Varchar(100), Category_id int, ItemGroup_StockYN Char(1), Company_Code int, Year_Code varchar(10), Constraint PK_ItemGroup_Id primary key (ItemGroup_Id, Company_Code, Year_Code), Constraint UK_ItemGroup_Name Unique (ItemGroup_Name, Company_Code, Year_Code), Constraint FK_Category_id Foreign Key (Category_ID, Company_Code, Year_Code) references Mas_Category(Category_ID, Company_Code, Year_Code))");
                }
                MyBase.UpdateSpecialFields_Inventory("Mas_itemGroup");

                if (MyBase.Check_Procedure("Ins_ItemGroup_Master") == false)
                {
                    MyBase.Execute("Create Proc Ins_ItemGroup_Master (@IGroup_Id int, @ItemgroupName as varchar(100), @Category_ID int, @Stock Char(1), @Company_Code int, @Year_Code varchar(10), @Alter_EmpCode int, @Alter_Syscode int, @Alter_Datetime Datetime) as Begin   	Set Nocount on; 	Begin try   		Declare @Itemgroup_ID int;   		if @IGroup_ID > 0		begin 			set @Itemgroup_ID = @IGroup_Id;		end		else		begin			Select @Itemgroup_ID = isnull(MAX(itemgroup_ID), 0) + 1 from Mas_ItemGroup where company_Code = @Company_Code and Year_Code = @Year_Code;   		end		Insert into Mas_ItemGroup (Itemgroup_ID, ItemGroup_Name, Category_Id, ItemGroup_StockYN, Company_Code, Year_Code, Alter_EMPCode, Alter_Syscode, Alter_Datetime) Select @Itemgroup_ID, @ItemgroupName, @category_ID, @Stock, @Company_Code, @Year_Code, @Alter_EMPCode, @Alter_Syscode, @Alter_Datetime;   		Select @Itemgroup_ID;    	end try  	Begin catch   		Declare @Emessage as nvarchar(4000);   Declare @ESeverity int;   Declare @EState int;   		Select @Emessage = ERROR_MESSAGE(), @ESeverity = ERROR_SEVERITY(), @EState = ERROR_STATE();   		raiserror (@EMessage, @ESeverity, @EState);  	End catch End ");
                }

                if (MyBase.Check_Procedure("UPD_ItemGroup_Master") == false)
                {
                    MyBase.Execute("Create Proc UPD_ItemGroup_Master (@ItemGroup_ID int, @ItemgroupName as varchar(100), @Category_ID int, @Stock Char(1), @Company_Code int, @Year_Code varchar(10), @Alter_EmpCode int, @Alter_SYScode int, @Alter_Datetime Datetime) As Begin 		Begin Try 		Update Mas_ItemGroup Set ItemGroup_Name = @ItemgroupName, Category_id = @Category_ID, Itemgroup_stockYN = @Stock, Alter_Empcode = @Alter_EMpcode, Alter_Syscode = @Alter_Syscode, Alter_Datetime = @Alter_Datetime where ItemGroup_ID = @ItemGroup_ID and Company_Code = @Company_Code and Year_Code = @Year_Code;  	end try 	Begin catch 		declare @EMessage nvarchar(4000);Declare @ESeverity int;Declare @EState int; 		Select @EMessage = ERROR_MESSAGE(), @ESeverity = Error_severity(), @EState = Error_State(); 		raiserror (@EMessage, @ESeverity, @EState); 	End catch End");
                }

                if (MyBase.Check_Procedure("Del_ItemGroup_Master") == false)
                {
                    MyBase.Execute("Create Proc Del_ItemGroup_Master (@ItemGroup_ID int, @Company_Code int, @Year_Code varchar(10)) As Begin 		begin Try		Delete from Mas_ItemGroup where ItemGroup_ID = @ItemGroup_ID and Company_Code = @Company_Code and Year_Code = @Year_Code; 	End Try 	begin catch 		declare @EMessage nvarchar(4000);Declare @ESeverity int;Declare @EState int; 				Select @EMessage = ERROR_MESSAGE(), @ESeverity = Error_severity(), @EState = Error_State(); 				raiserror (@EMessage, @ESeverity, @EState); 	End catch End");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void itemGroupMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        public void Table_Creation_SubGroup()
        {
            try
            {
                if (MyBase.Check_Table("Mas_ItemSubGroup") == false)
                {
                    MyBase.Execute("create table Mas_ItemSubGroup (RowID int Identity, SubGroup_id int, SubGroup_Name Varchar(100), Company_Code int, Year_Code varchar(10), Constraint PK_SubGroup_id primary key (SubGroup_id, Company_Code, Year_Code), Constraint UK_SubGroup_Name Unique(SubGroup_Name, Company_Code, Year_Code))");
                }
                MyBase.UpdateSpecialFields_Inventory("Mas_ItemSubGroup");

                if (MyBase.Check_Procedure("Ins_ItemSubGroup_Master") == false)
                {
                    MyBase.Execute("Create PROC INS_ItemSubGroup_MASTER (@ISGroup_ID int, @SubGroup_Name VARCHAR(100), @COMPANY_CODE INT, @YEAR_CODE VARCHAR(10), @Alter_EmpCode int, @Alter_Syscode int, @Alter_Datetime Datetime)  AS  begin	BEGIN try 			Set nocount on;			Begin Transaction;			DECLARE @SubGroup_id as int;   			if @ISGroup_ID > 0 			begin 				set @Subgroup_Id = @ISGroup_ID;			end		else			begin				Select @SubGroup_id = isnull(MAX(SubGroup_id), 0) + 1 from Mas_ItemSubGroup where company_Code = @Company_Code and Year_code = @Year_Code;   				end		INSERT INTO Mas_ItemSubGroup(SubGroup_id, SubGroup_Name, COMPANY_CODE, YEAR_CODE, Alter_EMpcode, Alter_Syscode, Alter_Datetime) SELECT @SubGroup_id, @SubGroup_Name, @COMPANY_CODE, @YEAR_CODE, @Alter_EMpcode, @Alter_Syscode, @Alter_Datetime;   			Select @SubGroup_id 			Commit Transaction; 	END try 	Begin Catch 			Declare @EMessage as nvarchar(4000);	Declare @ESeverity as int; 	Declare @EState as int;			Rollback Transaction 			Select @EMessage = ERROR_MESSAGE(), @Eseverity = Error_Severity(), @EState = ERROR_STATE(); 			raiserror (@EMessage, @ESeverity, @EState); 	End catch end ");
                }

                if (MyBase.Check_Procedure("Upd_ItemSubGroup_Master") == false)
                {
                    MyBase.Execute("Create proc Upd_ItemSubGroup_Master (@SubGroup_id int, @SubGroup_Name VARCHAR(100), @COMPANY_CODE INT, @YEAR_CODE VARCHAR(10), @Alter_Empcode int, @Alter_Syscode int, @Alter_Datetime datetime) as Begin 	Set Nocount On;	Begin Try 		Update Mas_ItemSubGroup set SubGroup_Name = @SubGroup_Name, Alter_EmpCode = @Alter_Empcode, Alter_Syscode = @Alter_Syscode, Alter_Datetime = @Alter_datetime where SubGroup_id = @SubGroup_id and Company_Code = @COMPANY_CODE and year_Code = @Year_Code; 	End Try 	Begin Catch 		Declare @EMessage as nvarchar(4000); 		Declare @ESeverity as int; 		Declare @EState as int; 		Select @EMessage = ERROR_MESSAGE(), @ESeverity = ERROR_SEVERITY(), @EState = ERROR_STATE(); 		Raiserror (@EMessage, @ESeverity, @EState); 	End Catch end");
                }

                if (MyBase.Check_Procedure("Del_ItemSubGroup_Master") == false)
                {
                    MyBase.Execute("Create proc Del_ItemSubGroup_Master (@SubGroup_id int, @Company_Code int, @Year_Code varchar(10)) as Begin 	Set Nocount on;	Begin Try 		Delete from Mas_ItemSubGroup where SubGroup_id = @SubGroup_id and Company_Code = @COMPANY_CODE and year_Code = @Year_Code; 	End try 	Begin Catch 		Declare @EMessage nvarchar(4000); 		Declare @ESeverity int; 		Declare @EState int; 		Select @EMEssage = Error_Message(), @ESeverity = ERROR_SEVERITY(), @EState = ERROR_STATE(); 		raiserror (@Emessage, @ESeverity, @EState); 	End Catch end");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void View_Browser_Time(String FormName, int Emplno, int UserCode)
        {
            try
            {
                FrmWeb_Browser Frm = new FrmWeb_Browser();
                Frm.MdiParent = this;
                Frm.Emplno = Emplno;
                Frm.FormName = FormName;
                Frm.UserCode = UserCode;
                Frm.WindowState = FormWindowState.Maximized;
                Frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void View_Browser(String FormName, Int64 UserCode)
        {
            try
            {
                FrmWeb_Browser Frm = new FrmWeb_Browser();
                Frm.FormName = FormName;
                Frm.UserCode = UserCode;
                Frm.MdiParent = this;
                Frm.WindowState = FormWindowState.Maximized;
                Frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void itemSubGroupMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        public void Table_Creation_Item()
        {
            try
            {
                // Table Creation

                if (MyBase.Check_Table("Mas_Item") == false)
                {
                    MyBase.Execute("Create table Mas_Item (Row_ID int identity,Item_ID int, ItemCode varchar(20),ItemName varchar(250),ItemDesc varchar(1000),ItemGroup_Id int,SubGroup_ID int,UOM_ID int,UOM_ID2 int,Item_OPenQty float,Item_OPenQty2 float,Item_OpenRate Money,Item_OpenRate2 Money,Item_OpenValue Money,Item_ClsQty float,Item_ClsQty2 float,Purcost money,Purcost2 money,SalesRate Money,SalesRate2 Money,ReorderLevel float,MaximumLevel float,ReorderLevel2 float,MaximumLevel2 float,QtyPer float,WtAllowance float,drawingNo varchar(25),CatelogNo varchar(25),Department_id int,Machine_Id int,Company_Code int, Year_Code varchar(10), Item_Photo Image, Constraint Pk_Item_ID Primary key (Item_ID, Company_Code, Year_Code),Constraint FK_Item_ItemGroup_Id Foreign Key (ItemGroup_ID, Company_Code, Year_Code) references Mas_itemGroup (ItemGroup_Id, Company_Code, Year_Code),Constraint FK_Item_ItemSubGroup_Id Foreign Key (SubGroup_ID, Company_Code, Year_Code) references Mas_itemSubGroup (SubGroup_Id, Company_Code, Year_Code),Constraint FK_Item_UOM_Id Foreign Key (UOM_ID, Company_Code, Year_Code) references Mas_UOM (UOM_Id, Company_Code, Year_Code),Constraint FK_Item_UOM_Id2 Foreign Key (UOM_ID2, Company_Code, Year_Code) references Mas_UOM (UOM_Id, Company_Code, Year_Code),Constraint FK_Item_Machine_Id Foreign Key (Machine_ID, Company_Code, Year_Code) references Mas_Machine (Machine_Id, Company_Code, Year_Code),Constraint FK_Item_Department_ID Foreign Key (Department_Id, Company_Code, Year_Code) references Mas_Department (Department_Id, Company_Code, Year_Code))");
                }
                MyBase.UpdateSpecialFields_Inventory("Mas_Item");

                if (MyBase.Check_Table("Mas_ItemSalesType") == false)
                {
                    MyBase.Execute("Create table Mas_ItemSalesType (Row_ID int Identity, Item_ID int, SalesType_ID int, Ledger_Code_Sales int, Ledger_Code_Tax int, Tax_Per Money, Company_Code int, year_Code varchar(10), Constraint FK_SType_Item_ID foreign Key (Item_ID, Company_Code, Year_Code) references Mas_Item (Item_ID, Company_Code, Year_Code),Constraint FK_SType_SalesType_ID foreign Key (SalesType_ID, Company_Code, Year_Code) references Mas_SalesType (SalesType_ID, Company_Code, Year_Code))");
                }
                MyBase.UpdateSpecialFields_Inventory("Mas_ItemSalesType");

                if (MyBase.Check_Table("Mas_ItemOpnStk") == false)
                {
                    MyBase.Execute("Create table Mas_ItemOpnStk(Row_ID int Identity, Item_ID int, stkwarehouse_id int, Stk_location varchar(20), item_OpenQty float, item_OpenQty2 float,  Company_Code int, year_Code varchar(10), Constraint FK_opnStk_Item_ID foreign Key (Item_ID, Company_Code, Year_Code) references Mas_Item (Item_ID, Company_Code, Year_Code))");
                }
                MyBase.UpdateSpecialFields_Inventory("Mas_ItemOpnStk");

                //// Insert Procedure Creation

                if (MyBase.Check_Procedure("Ins_Mas_Item") == false)
                {
                    MyBase.Execute("Create Proc Ins_Mas_Item (@ItemID int, @ItemCode varchar(20),@ItemName varchar(250),@ItemDesc varchar(1000),@ItemGroup_Id int,@SubGroup_ID int,@UOM_ID int,@UOM_ID2 int,@Item_OPenQty float,@Item_OPenQty2 float,@Item_OpenRate Money,@Item_OpenRate2 Money,@Item_OpenValue Money,@Item_ClsQty float,@Item_ClsQty2 float,@Purcost money,@Purcost2 money,@SalesRate Money,@SalesRate2 Money,@ReorderLevel float,@MaximumLevel float,@ReorderLevel2 float,@MaximumLevel2 float,@QtyPer float,@WtAllowance float,@drawingNo varchar(25),@CatelogNo varchar(25),@Department_id int,@Machine_Id int,@Company_Code int, @Year_Code varchar(10), @Image Image, @Alter_Empcode int, @Alter_Syscode int, @Alter_Datetime Datetime) as  Begin 	Declare @Item_ID int;	if @ItemID > 0 	begin		set @Item_ID = @Itemid;	end else begin	Select @Item_ID = isnull(Max(Item_Id), 0) + 1 from Mas_Item where Company_Code = @Company_Code and Year_Code = @Year_Code;	end insert into Mas_Item (Item_ID, ItemCode,ItemName,ItemDesc,ItemGroup_Id,SubGroup_ID,UOM_ID,UOM_ID2,Item_OPenQty,Item_OPenQty2,Item_OpenRate,Item_OpenRate2,Item_OpenValue,Item_ClsQty,Item_ClsQty2,Purcost,Purcost2,SalesRate,SalesRate2,ReorderLevel,MaximumLevel,ReorderLevel2,MaximumLevel2,QtyPer,WtAllowance,drawingNo,CatelogNo,Department_id,Machine_Id,Company_Code, Year_Code, Item_photo, Alter_Empcode, Alter_Syscode, Alter_Datetime) values (@item_ID, @ItemCode,@ItemName,@ItemDesc,@ItemGroup_Id,@SubGroup_ID,@UOM_ID,@UOM_ID2,@Item_OPenQty,@Item_OPenQty2,@Item_OpenRate,@Item_OpenRate2,@Item_OpenValue,@Item_ClsQty,@Item_ClsQty2,@Purcost,@Purcost2,@SalesRate,@SalesRate2,@ReorderLevel,@MaximumLevel,@ReorderLevel2,@MaximumLevel2,@QtyPer,@WtAllowance,@drawingNo,@CatelogNo,@Department_id,@Machine_Id,@Company_Code, @Year_Code, @IMage, @Alter_Empcode, @Alter_Syscode, @Alter_Datetime);	Select @Item_ID; End");
                }
                //// Update Procedure Creation

                if (MyBase.Check_Procedure("UPD_Mas_Item") == false)
                {
                    MyBase.Execute("Create Proc UPD_Mas_Item (@Item_ID int, @ItemCode varchar(20),@ItemName varchar(250),@ItemDesc varchar(1000),@ItemGroup_Id int,@SubGroup_ID int,@UOM_ID int,@UOM_ID2 int,@Item_OPenQty float,@Item_OPenQty2 float,@Item_OpenRate Money,@Item_OpenRate2 Money,@Item_OpenValue Money,@Item_ClsQty float,@Item_ClsQty2 float,@Purcost money,@Purcost2 money,@SalesRate Money,@SalesRate2 Money,@ReorderLevel float,@MaximumLevel float,@ReorderLevel2 float,@MaximumLevel2 float,@QtyPer float,@WtAllowance float,@drawingNo varchar(25),@CatelogNo varchar(25),@Department_id int,@Machine_Id int,@Company_Code int, @Year_Code varchar(10), @Image Image, @Alter_Empcode int, @Alter_SysCode int, @Alter_Datetime Datetime) as  Begin 		update Mas_Item set ItemCode = @ItemCode,ItemName = @ItemName,ItemDesc = @ItemDesc, Item_photo = @Image, ItemGroup_Id = @ItemGroup_Id,SubGroup_ID = @SubGroup_ID,UOM_ID=@UOM_ID,UOM_ID2=@UOM_ID2,Item_OPenQty=@Item_OPenQty,Item_OPenQty2=@Item_OPenQty2,Item_OpenRate=@Item_OpenRate,Item_OpenRate2=@Item_OpenRate2,Item_OpenValue=@Item_OpenValue,Item_ClsQty=@Item_ClsQty,Item_ClsQty2=@Item_ClsQty2,Purcost=@Purcost,Purcost2=@Purcost2,SalesRate=@SalesRate,	SalesRate2=@SalesRate2,ReorderLevel=@ReorderLevel,MaximumLevel=@MaximumLevel,ReorderLevel2=@ReorderLevel2,MaximumLevel2=@MaximumLevel2,QtyPer=@QtyPer,WtAllowance=@WtAllowance,drawingNo=@drawingNo,CatelogNo=@CatelogNo,Department_id=@Department_id,Machine_Id=@Machine_Id, Alter_EmpCode = @Alter_Empcode, Alter_SysCode = @Alter_SysCode, Alter_datetime = @Alter_datetime where item_ID = @Item_ID and Company_Code=@Company_Code and Year_Code = @Year_Code; Select @Item_ID; End");
                }
                if (MyBase.Check_Procedure("Ins_Mas_ItemSalesType") == false)
                {
                    MyBase.Execute("Create Proc Ins_Mas_ItemSalesType (@Item_ID int, @SalesType_ID int, @Ledger_Code_Sales int, @Ledger_Code_Tax int, @Tax_Per Money, @Company_Code int, @year_Code varchar(10), @Alter_EMPCode int, @Alter_SysCode int, @Alter_Datetime Datetime) as Begin 	insert into Mas_ItemSalesType (Item_ID, SalesType_ID, Ledger_Code_Sales, Ledger_Code_Tax, Tax_Per, Company_Code, year_Code, Alter_Empcode, Alter_SysCode, Alter_datetime) Values (@Item_ID, @SalesType_ID, @Ledger_Code_Sales, @Ledger_Code_Tax, @Tax_Per, @Company_Code, @year_Code, @Alter_Empcode, @Alter_SysCode, @Alter_datetime); End");
                }
                if (MyBase.Check_Procedure("Ins_Mas_ItemOpnStk") == false)
                {
                    MyBase.Execute("Create Proc Ins_Mas_ItemOpnStk (@Item_ID int, @stkwarehouse_id int, @Stk_location varchar(20), @Item_OPenqty float, @Item_OpenQty2 float, @Company_Code int, @year_Code varchar(10), @Alter_Empcode int, @Alter_Syscode int, @Alter_datetime Datetime) as Begin  insert into Mas_ItemOpnStk(Item_ID, stkwarehouse_id, Stk_location, Item_Openqty, Item_openQty2, Company_Code, year_Code, Alter_Empcode, Alter_Syscode, Alter_Datetime) values (@Item_ID, @stkwarehouse_id, @Stk_location, @Item_Openqty, @Item_openqty2, @Company_Code, @year_Code, @Alter_Empcode, @Alter_Syscode, @Alter_Datetime); End");
                }


                //// Delete Procedure Creation

                if (MyBase.Check_Procedure("Del_Mas_Item") == false)
                {
                    MyBase.Execute(" Create Proc Del_Mas_Item (@Item_ID int, @Company_Code int, @Year_Code varchar(10)) as  Begin 		delete from Mas_ItemSalesType where item_ID = @Item_ID and Company_Code=@Company_Code and Year_Code = @Year_Code; 	delete from Mas_ItemOpnStk where item_ID = @Item_ID and Company_Code=@Company_Code and Year_Code = @Year_Code; delete from Mas_Item where item_ID = @Item_ID and Company_Code=@Company_Code and Year_Code = @Year_Code; 	End ");
                }

                if (MyBase.Check_Table("DEL_MAS_ITEMDETAILS"))
                {
                    MyBase.Execute("Drop Proc DEL_MAS_ITEMDETAILS");
                }

                if (MyBase.Check_Procedure("Del_Mas_ItemSalesType") == false)
                {
                    MyBase.Execute(" Create Proc Del_Mas_ItemSalesType (@Item_ID int, @Company_Code int, @year_Code varchar(10)) as Begin  	Delete from Mas_ItemSalesType where item_ID = @Item_ID and Company_Code = @Company_Code and Year_Code = @Year_Code; End ");
                }

                if (MyBase.Check_Procedure("Del_Mas_ItemOpnStk") == false)
                {
                    MyBase.Execute(" Create Proc Del_Mas_ItemOpnStk (@Item_ID int, @Company_Code int, @year_Code varchar(10)) as Begin  	Delete from Mas_ItemOpnStk where item_ID = @Item_ID and Company_Code = @Company_Code and Year_Code = @Year_Code; End ");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void itemMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        public void Table_Creation_UOM()
        {
            try
            {
                if (MyBase.Check_Table("Mas_UOM") == false)
                {
                    MyBase.Execute("create table Mas_UOM (RowID int Identity, UOM_id int, UOM_Name Varchar(10), UOM_Description Varchar(25), UOM_Decimal Tinyint, Company_Code int, Year_Code varchar(10), Constraint PK_UOM_Id primary key (UOM_ID, Company_Code, Year_Code), Constraint UK_UOM_Name Unique(UOM_Name, Company_Code, Year_Code))");
                }
                MyBase.UpdateSpecialFields_Inventory("Mas_UOM");

                if (MyBase.Check_Procedure("Ins_UOM_Master") == false)
                {
                    MyBase.Execute("create Proc Ins_UOM_Master (@UOMID int, @UOM_Name varchar(10), @UOM_Description varchar(25), @UOM_decimal tinyint, @Company_Code int, @Year_Code varchar(10), @Alter_EmpCode int, @Alter_Syscode int, @Alter_datetime Datetime)  as  Begin 			Set NoCount on; 		begin try 				declare @UOM_Id int;					if @UOMID > 0 		begin			set @UOM_Id = @uomid;		end		else		begin			Select @UOM_ID = isnull(Max(UOM_Id), 0) + 1 from MAS_UOm where company_Code = @Company_Code and Year_Code = @Year_Code;					end		Insert into Mas_UOM Select @UOM_Id, @UOM_Name, @UOM_Description, @UOM_decimal, @Company_Code, @Year_Code, @Alter_Empcode, @Alter_Syscode, @Alter_datetime;					Select @UOM_Id; 		End try 		begin catch 				Declare @EMessage as nvarchar(4000); Declare @ESeverity int;Declare @EState int; 				Select @EMessage = ERROR_MESSAGE(), @Eseverity = Error_severity(), @EState = Error_State(); 				raiserror (@Emessage, @Eseverity, @EState); 		end catch End");
                }

                if (MyBase.Check_Procedure("UPD_UOM_Master") == false)
                {
                    MyBase.Execute("Create Proc UPD_UOM_Master (@UOM_ID int, @UOM_Name varchar(10), @UOM_Description varchar(25), @UOM_decimal tinyint, @Company_Code int, @Year_Code varchar(10), @Alter_EmpCode int, @Alter_sysCode int, @Alter_Datetime Datetime) as  Begin 		set nocount on;	begin try 		Update Mas_UOM Set uom_Name = @UOM_Name, UOM_description = @UOM_Description, UOM_decimal = @UOM_decimal, Alter_Empcode = @Alter_Empcode, Alter_syscode = @Alter_Syscode, Alter_datetime = @Alter_datetime where UOM_id = @UOM_ID and company_Code = @Company_Code and Year_Code = @Year_Code;  	end try 	begin catch 		declare @EMessage nvarchar(4000);Declare @ESeverity int;Declare @EState int; 		Select @EMessage = Error_message(), @ESeverity = error_severity(), @EState = ERROR_STATE(); 		raiserror (@EMessage, @ESeverity, @EState); 	end catch  End");
                }

                if (MyBase.Check_Procedure("Del_UOM_Master") == false)
                {
                    MyBase.Execute("create Proc Del_UOM_Master (@UOM_ID int, @Company_Code int, @Year_Code varchar(10)) as   Begin 	 	set nocount on; 	begin try 		Delete from Mas_UOM where UOM_id = @UOM_ID and company_Code = @Company_Code and Year_Code = @Year_Code; 	end try 	begin catch 		declare @EMessage nvarchar(4000);Declare @ESeverity int;Declare @EState int; 		Select @EMessage = Error_message(), @ESeverity = error_severity(), @EState = ERROR_STATE(); 		raiserror (@EMessage, @ESeverity, @EState); 	end catch	 End");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void uOMMasterToolStripMenuItem_Click_3(object sender, EventArgs e)
        {

        }

        public void Table_Creation_Department()
        {
            try
            {
                if (MyBase.Check_Table("Mas_Department") == false)
                {
                    MyBase.Execute("create table Mas_Department (RowID int Identity, Department_id int, Department_Name Varchar(100), Company_Code int, Year_Code varchar(10), Constraint PK_Department_id primary key (Department_id, Company_Code, Year_Code), Constraint UK_Depart_Name Unique(Department_Name, Company_Code, Year_Code))");
                }
                MyBase.UpdateSpecialFields_Inventory("Mas_Department");

                if (MyBase.Check_Procedure("Ins_Department_Master") == false)
                {
                    MyBase.Execute("Create PROC INS_Department_MASTER (@DeptID int, @Department_Name VARCHAR(100), @COMPANY_CODE INT, @YEAR_CODE VARCHAR(10), @Alter_Empcode int, @Alter_Syscode int, @Alter_Datetime Datetime)  AS  	BEGIN try 		Set nocount on;		Begin Transaction;			DECLARE @Department_id as int;   			if @DeptID > 0			begin				set @Department_id = @DeptID;			end		else			begin				Select @Department_id = isnull(MAX(Department_id), 0) + 1 from Mas_Department where company_Code = @Company_Code and Year_code = @Year_Code;   				end				INSERT INTO Mas_Department(Department_id, Department_Name, COMPANY_CODE, YEAR_CODE, Alter_Empcode,Alter_Syscode, Alter_datetime) SELECT @Department_id, @Department_Name, @COMPANY_CODE, @YEAR_CODE, @Alter_Empcode, @Alter_Syscode, @Alter_Datetime;   			Select @Department_id 			Commit Transaction; 	END try 	Begin Catch 			Declare @EMessage as nvarchar(4000);	Declare @ESeverity as int; 	Declare @EState as int;			Rollback Transaction 			Select @EMessage = ERROR_MESSAGE(), @Eseverity = Error_Severity(), @EState = ERROR_STATE(); 			raiserror (@EMessage, @ESeverity, @EState); 	End catch ");
                }

                if (MyBase.Check_Procedure("Upd_Department_Master") == false)
                {
                    MyBase.Execute("Create proc Upd_Department_Master (@Department_id int, @Department_Name VARCHAR(100), @COMPANY_CODE INT, @YEAR_CODE VARCHAR(10), @Alter_Empcode int, @Alter_syscode int, @Alter_datetime Datetime) as Begin 	Set Nocount On;	Begin Try 		Update Mas_Department set Department_Name = @Department_Name, Alter_Empcode = @Alter_Empcode, Alter_Syscode = @Alter_Syscode, Alter_datetime = @Alter_datetime where Department_id = @Department_id and Company_Code = @COMPANY_CODE and year_Code = @Year_Code; 	End Try 	Begin Catch 		Declare @EMessage as nvarchar(4000); 		Declare @ESeverity as int; 		Declare @EState as int; 		Select @EMessage = ERROR_MESSAGE(), @ESeverity = ERROR_SEVERITY(), @EState = ERROR_STATE(); 		Raiserror (@EMessage, @ESeverity, @EState); 	End Catch end");
                }

                if (MyBase.Check_Procedure("Del_Department_Master") == false)
                {
                    MyBase.Execute("Create proc Del_Department_Master (@Department_id int, @Company_Code int, @Year_Code varchar(10)) as Begin 	Set Nocount on;	Begin Try 		Delete from Mas_Department where Department_id = @Department_id and Company_Code = @COMPANY_CODE and year_Code = @Year_Code; 	End try 	Begin Catch 		Declare @EMessage nvarchar(4000); 		Declare @ESeverity int; 		Declare @EState int; 		Select @EMEssage = Error_Message(), @ESeverity = ERROR_SEVERITY(), @EState = ERROR_STATE(); 		raiserror (@Emessage, @ESeverity, @EState); 	End Catch end");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void departmentMasterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        public void Table_Creation_Machine()
        {
            try
            {
                if (MyBase.Check_Table("Mas_Machine") == false)
                {
                    MyBase.Execute("create table Mas_Machine (RowID int Identity, Machine_ID int, Machine_Name Varchar(100), Company_Code int, Year_Code varchar(10), Constraint PK_Machine_ID primary key (Machine_ID, Company_Code, Year_Code), Constraint UK_Machine_Name Unique(Machine_Name, Company_Code, Year_Code))");
                }
                MyBase.UpdateSpecialFields_Inventory("Mas_Machine");

                if (MyBase.Check_Procedure("Ins_Machine_Master") == false)
                {
                    MyBase.Execute("Create PROC INS_Machine_MASTER (@MachID int, @Machine_Name VARCHAR(100), @COMPANY_CODE INT, @YEAR_CODE VARCHAR(10), @Alter_Empcode int, @Alter_Syscode int, @Alter_datetime Datetime)  AS  	BEGIN try 			Set nocount on;			Begin Transaction;				DECLARE @Machine_ID as int;   				if @MachID > 0 				begin 					set @Machine_ID = @MachID; 				end			else 				begin 					Select @Machine_ID = isnull(MAX(Machine_ID), 0) + 1 from Mas_Machine where company_Code = @Company_Code and Year_code = @Year_Code;   					end 			INSERT INTO Mas_Machine(Machine_ID, Machine_Name, COMPANY_CODE, YEAR_CODE, Alter_Empcode, Alter_Syscode, Alter_Datetime) SELECT @Machine_ID, @Machine_Name, @COMPANY_CODE, @YEAR_CODE, @Alter_Empcode, @Alter_Syscode, @Alter_Datetime;   				Select @Machine_ID 			Commit Transaction; 	END try  	Begin Catch 			Declare @EMessage as nvarchar(4000);	Declare @ESeverity as int; 	Declare @EState as int;			Rollback Transaction 			Select @EMessage = ERROR_MESSAGE(), @Eseverity = Error_Severity(), @EState = ERROR_STATE(); 			raiserror (@EMessage, @ESeverity, @EState); 	End catch ");
                }

                if (MyBase.Check_Procedure("Upd_Machine_Master") == false)
                {
                    MyBase.Execute("Create proc Upd_Machine_Master (@Machine_ID int, @Machine_Name VARCHAR(100), @COMPANY_CODE INT, @YEAR_CODE VARCHAR(10), @Alter_Empcode int, @Alter_Syscode int, @Alter_datetime Datetime) as Begin 	Set Nocount On;	Begin Try 		Update Mas_Machine set Machine_Name = @Machine_Name, Alter_EmpCode = @Alter_Empcode, Alter_syscode = @Alter_Syscode, Alter_Datetime = @Alter_Datetime where Machine_ID = @Machine_ID and Company_Code = @COMPANY_CODE and year_Code = @Year_Code; 	End Try 	Begin Catch 		Declare @EMessage as nvarchar(4000); 		Declare @ESeverity as int; 		Declare @EState as int; 		Select @EMessage = ERROR_MESSAGE(), @ESeverity = ERROR_SEVERITY(), @EState = ERROR_STATE(); 		Raiserror (@EMessage, @ESeverity, @EState); 	End Catch end");
                }

                if (MyBase.Check_Procedure("Del_Machine_Master") == false)
                {
                    MyBase.Execute("Create proc Del_Machine_Master (@Machine_ID int, @Company_Code int, @Year_Code varchar(10)) as Begin 	Set Nocount on;	Begin Try 		Delete from Mas_Machine where Machine_ID = @Machine_ID and Company_Code = @COMPANY_CODE and year_Code = @Year_Code; 	End try 	Begin Catch 		Declare @EMessage nvarchar(4000); 		Declare @ESeverity int; 		Declare @EState int; 		Select @EMEssage = Error_Message(), @ESeverity = ERROR_SEVERITY(), @EState = ERROR_STATE(); 		raiserror (@Emessage, @ESeverity, @EState); 	End Catch end");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void machineMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public void Table_Creation_SalesType()
        {
            try
            {
                if (MyBase.Check_Table("Mas_SalesType") == false)
                {
                    MyBase.Execute("create table Mas_SalesType (RowID int Identity, SalesType_id int, SalesType_Name Varchar(100), SalesType_Category Char(1), Company_Code int, Year_Code varchar(10), Constraint PK_SalesType_id primary key (SalesType_id, Company_Code, Year_Code), Constraint UK_SalesType_Name Unique(SalesType_Name, Company_Code, Year_Code))");
                }
                MyBase.UpdateSpecialFields_Inventory("Mas_SalesType");

                if (MyBase.Check_Procedure("Ins_SalesType_Master") == false)
                {
                    MyBase.Execute("Create PROC INS_SalesType_MASTER (@STypeID int, @SalesType_Name VARCHAR(100), @SalesType_Category Char(1), @COMPANY_CODE INT, @YEAR_CODE VARCHAR(10), @Alter_Empcode int, @Alter_syscode int, @Alter_Datetime Datetime)  AS  BEGIN try 		Set nocount on;		Begin Transaction;		DECLARE @SalesType_id as int; 	if @STypeID > 0 		begin			set @Salestype_ID = @StypeID;		end  		else		begin			Select @SalesType_id = isnull(MAX(SalesType_id), 0) + 1 from Mas_SalesType where company_Code = @Company_Code and Year_code = @Year_Code;   			end	INSERT INTO Mas_SalesType(SalesType_id, SalesType_Name, SalesType_Category, COMPANY_CODE, YEAR_CODE, Alter_Empcode, Alter_Syscode, Alter_datetime) SELECT @SalesType_id, @SalesType_Name, @SalesType_Category, @COMPANY_CODE, @YEAR_CODE, @Alter_Empcode, @Alter_Syscode, @Alter_datetime;   		Select @SalesType_id 		Commit Transaction; END try 	Begin Catch 			Declare @EMessage as nvarchar(4000);	Declare @ESeverity as int; 	Declare @EState as int;			Rollback Transaction 			Select @EMessage = ERROR_MESSAGE(), @Eseverity = Error_Severity(), @EState = ERROR_STATE(); 			raiserror (@EMessage, @ESeverity, @EState); 	End catch ");
                }

                if (MyBase.Check_Procedure("Upd_SalesType_Master") == false)
                {
                    MyBase.Execute("Create proc Upd_SalesType_Master (@SalesType_id int, @SalesType_Name VARCHAR(100), @SalesType_Category Char(1), @COMPANY_CODE INT, @YEAR_CODE VARCHAR(10), @Alter_Empcode int, @Alter_syscode int, @Alter_Datetime Datetime) as Begin 	Set Nocount On;	Begin Try 		Update Mas_SalesType set SalesType_Name = @SalesType_Name, SalesType_Category = @SalesType_Category, Alter_Empcode = @Alter_Empcode, Alter_syscode = @Alter_Syscode, Alter_Datetime = @Alter_datetime where SalesType_id = @SalesType_id and Company_Code = @COMPANY_CODE and year_Code = @Year_Code; 	End Try 	Begin Catch 		Declare @EMessage as nvarchar(4000); 		Declare @ESeverity as int; 		Declare @EState as int; 		Select @EMessage = ERROR_MESSAGE(), @ESeverity = ERROR_SEVERITY(), @EState = ERROR_STATE(); 		Raiserror (@EMessage, @ESeverity, @EState); 	End Catch end");
                }

                if (MyBase.Check_Procedure("Del_SalesType_Master") == false)
                {
                    MyBase.Execute("Create proc Del_SalesType_Master (@SalesType_id int, @Company_Code int, @Year_Code varchar(10)) as Begin 	Set Nocount on;	Begin Try 		Delete from Mas_SalesType where SalesType_id = @SalesType_id and Company_Code = @COMPANY_CODE and year_Code = @Year_Code; 	End try 	Begin Catch 		Declare @EMessage nvarchar(4000); 		Declare @ESeverity int; 		Declare @EState int; 		Select @EMEssage = Error_Message(), @ESeverity = ERROR_SEVERITY(), @EState = ERROR_STATE(); 		raiserror (@Emessage, @ESeverity, @EState); 	End Catch end");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void salesTypeMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public void Table_Creation_StkWareHouse()
        {
            try
            {
                if (MyBase.Check_Table("Mas_StkWarehouse") == false)
                {
                    MyBase.Execute("create table Mas_StkWarehouse (RowID int Identity, StkWarehouse_Id int, StkWarehouse_Name Varchar(100), StkWarehouse_Address varchar(1000), StkWarehouse_Phone Varchar(100), Order_No tinyint, Company_Code int, Year_Code varchar(10), Constraint PK_StkWarehouse_Id primary key (StkWarehouse_Id, Company_Code, Year_Code), Constraint UK_StkWarehouse_Name Unique(StkWarehouse_Name, Company_Code, Year_Code))");
                }
                MyBase.UpdateSpecialFields_Inventory("Mas_Stkwarehouse");

                if (MyBase.Check_Procedure("Ins_StkWarehouse_Master") == false)
                {
                    MyBase.Execute("Create PROC INS_StkWarehouse_MASTER (@StkID int, @StkWarehouse_Name VARCHAR(100), @StkWarehouse_Address varchar(1000), @StkWarehouse_Phone varchar(100), @Order_No tinyint, @COMPANY_CODE INT, @YEAR_CODE VARCHAR(10), @Alter_Empcode int, @Alter_Syscode int, @Alter_Datetime Datetime)  AS  	BEGIN try 			Set nocount on;			Begin Transaction;			DECLARE @StkWarehouse_Id as int;   			if @StkID > 0			begin				set @StkWarehouse_Id = @StkID;			end 		else			begin				Select @StkWarehouse_Id = isnull(MAX(StkWarehouse_Id), 0) + 1 from Mas_StkWarehouse where company_Code = @Company_Code and Year_code = @Year_Code;   				end		INSERT INTO Mas_StkWarehouse(StkWarehouse_Id, StkWarehouse_Name, StkWarehouse_Address, StkWarehouse_Phone, Order_No, COMPANY_CODE, YEAR_CODE, Alter_Empcode, Alter_syscode, Alter_datetime) SELECT @StkWarehouse_Id, @StkWarehouse_Name, @StkWarehouse_Address, @StkWarehouse_Phone, @Order_No, @COMPANY_CODE, @YEAR_CODE, @Alter_Empcode, @Alter_syscode, @Alter_datetime;   			Select @StkWarehouse_Id 			Commit Transaction; 	END try 	Begin Catch 			Declare @EMessage as nvarchar(4000);	Declare @ESeverity as int; 	Declare @EState as int;			Rollback Transaction 			Select @EMessage = ERROR_MESSAGE(), @Eseverity = Error_Severity(), @EState = ERROR_STATE(); 			raiserror (@EMessage, @ESeverity, @EState); 	End catch ");
                }

                if (MyBase.Check_Procedure("Upd_StkWarehouse_Master") == false)
                {
                    MyBase.Execute("Create proc Upd_StkWarehouse_Master (@StkWarehouse_Id int, @StkWarehouse_Name VARCHAR(100), @StkWarehouse_Address varchar(1000), @StkWarehouse_Phone varchar(100), @Order_No tinyint, @COMPANY_CODE INT, @YEAR_CODE VARCHAR(10), @Alter_Empcode int, @Alter_SysCode int, @Alter_Datetime Datetime) as Begin 	Set Nocount On;	Begin Try 		Update Mas_StkWarehouse set StkWarehouse_Name = @StkWarehouse_Name, StkWarehouse_Address = @StkWarehouse_Address, Order_No = @Order_No, StkWarehouse_Phone = @StkWarehouse_Phone, Alter_Empcode = @Alter_Empcode, Alter_syscode = @Alter_Syscode, Alter_Datetime = @Alter_Datetime where StkWarehouse_Id = @StkWarehouse_Id and Company_Code = @COMPANY_CODE and year_Code = @Year_Code; 	End Try 	Begin Catch 		Declare @EMessage as nvarchar(4000); 		Declare @ESeverity as int; 		Declare @EState as int; 		Select @EMessage = ERROR_MESSAGE(), @ESeverity = ERROR_SEVERITY(), @EState = ERROR_STATE(); 		Raiserror (@EMessage, @ESeverity, @EState); 	End Catch end");
                }

                if (MyBase.Check_Procedure("Del_StkWarehouse_Master") == false)
                {
                    MyBase.Execute("Create proc Del_StkWarehouse_Master (@StkWarehouse_Id int, @Company_Code int, @Year_Code varchar(10)) as Begin 	Set Nocount on;	Begin Try 		Delete from Mas_StkWarehouse where StkWarehouse_Id = @StkWarehouse_Id and Company_Code = @COMPANY_CODE and year_Code = @Year_Code; 	End try 	Begin Catch 		Declare @EMessage nvarchar(4000); 		Declare @ESeverity int; 		Declare @EState int; 		Select @EMEssage = Error_Message(), @ESeverity = ERROR_SEVERITY(), @EState = ERROR_STATE(); 		raiserror (@Emessage, @ESeverity, @EState); 	End Catch end");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void stockWarehouseMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem13_Click_1(object sender, EventArgs e)
        {

        }

        private void removeUnnecessaryBreakupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (CompName.ToUpper().Contains("GAINUP") || CompName.ToUpper().Contains("ALAMELU"))
                {
                    if (UserName.ToUpper().Contains("ADMIN"))
                    {
                        if (MessageBox.Show("Sure to Check Breakup ? ", "Vaahini", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Remove_Unnecesary_Breakup();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void remainderLetterDebrosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void loadMenuItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                MDICaption();
                StripLabel1.Text = Rotate_String(StripLabel1.Text);

                //if (MyBase.Get_RecordCount_WO_CheckTable("Vaahini_ERP_Gainup.Dbo.Log_Security_Alert", "Flag = 1 And Module = '" + System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.ScopeName.ToUpper().Replace(".EXE", "") + "'") > 0)
                //{
                //    Security_Flag = true;
                //    Application.Exit();
                //}


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void AutoMail_Po_Socks()
        {
            DataTable Dtmm = new DataTable();
            DataTable Dtmm1 = new DataTable();
            String Str, Str1, Str2, Str3, Str4, Str5;
            Double Amt = 0;
            DataTable Dtmm2 = new DataTable();
            DataTable Dtmm3 = new DataTable();
            DataTable Dtmm4 = new DataTable();
            DataTable Dtmm5 = new DataTable();
            String Order = "";
            Int32 N = 0;

            //Str1 = " Select A.PONo, C.LEdgeR_NAme, C.Ledger_Email, A.RowID, 'Yarn' Mode  from Socks_Yarn_PO_Master A Left Join Socks_PO_Mail_Log_Details B On A.RowID = B.POMasID and B.Mode = 'Yarn' LEft JOin Supplier_All_Fn() C On A.Supplier_Code = C.LEdgeR_code where A.Approval_Flag = 'T' and A.PoDate >= '18-jul-2017' and B.POMasID is Null ";
            //Str1 = Str1 + " Union  Select A.PONo, C.LEdgeR_NAme, C.Ledger_Email, A.RowID, 'Dye' Mode  from Socks_Yarn_PO_Dyeing_Master A Left Join Socks_PO_Mail_Log_Details B On A.RowID = B.POMasID and B.Mode = 'Dye' LEft JOin Supplier_All_Fn() C On A.Supplier_Code = C.LEdgeR_code where A.Approval_Flag = 'T' and A.PoDate >= '18-jul-2017' and B.POMasID is Null ";

            Str1 = " Select A.PONo, C.LEdgeR_NAme, (Case when A.MailId is null then C.Ledger_email else a.MailId end) Ledger_email, A.RowID, 'Yarn' Mode  from Socks_Yarn_PO_Master A Left Join Socks_PO_Mail_Log_Details B On A.RowID = B.POMasID and B.Mode = 'Yarn' LEft JOin Supplier_All_Fn() C On A.Supplier_Code = C.LEdgeR_code where A.Approval_Flag = 'T' and A.PoDate >= '18-jul-2017' and B.POMasID is Null ";
            Str1 = Str1 + " Union  Select A.PONo, C.LEdgeR_NAme, C.Ledger_email, A.RowID, 'Dye' Mode  from Socks_Yarn_PO_Dyeing_Master A Left Join Socks_PO_Mail_Log_Details B On A.RowID = B.POMasID and B.Mode = 'Dye' LEft JOin Supplier_All_Fn() C On A.Supplier_Code = C.LEdgeR_code where A.Approval_Flag = 'T' and A.PoDate >= '18-jul-2017' and B.POMasID is Null ";


            MyBase.Load_Data(Str1, ref Dtmm);
            if (Dtmm.Rows.Count >= 1)
            {
                for (int m = 0; m <= Dtmm.Rows.Count - 1; m++)
                {
                    if (Dtmm.Rows[m]["Ledger_Email"].ToString() != String.Empty)
                    {
                        StringBuilder Body = new StringBuilder();
                        Body.Append("Dear Sir, ");
                        Body.Append(Environment.NewLine);
                        Body.Append(Environment.NewLine);
                        Body.Append("Pls Find Attachment");
                        //Frm.Result = "fit@gainup.in";

                        if (Dtmm.Rows[m]["Mode"].ToString() == "Yarn")
                        {
                            Str = " Select S1.PONo, L1.Ledger_Name Supplier, Cast(S1.PoDate As date)PoDate, S1.Required_Date,  S1.Commit_Date, (Case When S1.PO_Method = 0 Then 'OCN-WISE' When S1.PO_Method = 0 Then 'ITEM-WISE' End) PO_Method, L1.Ledger_Address Supplier_Address, L1.Ledger_Phone Supplier_Phone, L1.Ledger_email Supplier_Email From Socks_Yarn_PO_Master S1 left Join Supplier_All_Fn() L1 On L1.LEdgeR_Code = S1.Supplier_Code   Where S1.Approval_Flag = 'T' and S1.RowID = " + Dtmm.Rows[m]["RowID"];
                            MyBase.Load_Data(Str, ref Dtmm1);
                            //Str1 = " Select Top 100000000 ROW_NUMBER()Over(Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size)SlNo, I1.Item + ' - ' + C1.color + ' - ' + S4.Size Item_Color_Size, Sum(S2.Order_Qty) Order_Qty, Sum(S2.Cancel_Qty) Cancel_Qty,  S2.Rate, Sum(S2.Order_Qty) * S2.Rate Amount, S1.PODate, S1.Required_Date From Socks_Yarn_PO_Master S1 Inner join Socks_Yarn_PO_Details S2 ON S1.RowID = s2.Master_ID Inner join Socks_Order_Master S3 on S2.Order_ID = S3.RowID Inner join item I1 on S2.Item_id = I1.itemid Inner join color C1 on s2.Color_id = c1.colorid Inner join size S4 on s2.Size_ID = S4.sizeid Where S1.RowID = " + Dtmm.Rows[m]["RowID"] + " GRoup by I1.Item ,C1.color, S4.Size , S2.Rate, S1.PODate, S1.Required_Date  Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size ";
                            Str1 = " Select Top 100000000 ROW_NUMBER()Over(Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size)SlNo, I1.Item + ' - ' + C1.color + ' - ' + S4.Size + '  ( '  +  Max(S2.Remarks) + ' ) ' Item_Color_Size, Sum(S2.Order_Qty) Order_Qty, Sum(S2.Cancel_Qty) Cancel_Qty,  S2.Rate, Sum(S2.Order_Qty) * S2.Rate Amount, S1.PODate, S1.Required_Date From Socks_Yarn_PO_Master S1 Inner join Socks_Yarn_PO_Details S2 ON S1.RowID = s2.Master_ID Inner join Socks_Order_Master S3 on S2.Order_ID = S3.RowID Inner join item I1 on S2.Item_id = I1.itemid Inner join color C1 on s2.Color_id = c1.colorid Inner join size S4 on s2.Size_ID = S4.sizeid Where S1.RowID = " + Dtmm.Rows[m]["RowID"] + " GRoup by I1.Item ,C1.color, S4.Size , S2.Rate, S1.PODate, S1.Required_Date  Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size ";
                            MyBase.Execute_Qry(Str1, "Socks_Yarn_PO");

                            Str2 = " Select Top 2 S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From Socks_Yarn_Tax_Details S1 Left Join Socks_Tax_Accounts() L1 on S1.Tax_Code = L1.Ledger_Code  Where S1.Master_ID = " + Dtmm.Rows[m]["RowID"] + " Order by S1.Slno ";
                            MyBase.Load_Data(Str2, ref Dtmm2);

                            Str3 = " Select Distinct S3.Order_No From Socks_Yarn_PO_Master S1 Inner join Socks_Yarn_PO_Details S2 ON S1.RowID = s2.Master_ID Inner join Socks_Order_Master S3 on S2.Order_ID = S3.RowID Where S1.RowID = " + Dtmm.Rows[m]["RowID"];
                            MyBase.Load_Data(Str3, ref Dtmm3);

                            Str4 = " Select Getdate()PrintOutDate";
                            MyBase.Load_Data(Str4, ref Dtmm4);

                            Str5 = " Select RowID, (Sum(Amt) + Sum(Amt1)) Amt From (Select A.RowID, Sum(Amount) Amt, 0 Amt1 From Socks_Yarn_PO_Master A Inner Join Socks_Yarn_PO_Details B On A.RowID = B.Master_ID Group by A.RowID Union Select A.RowID, 0 Amt, Sum(Tax_Amount) Amt1 From Socks_Yarn_PO_Master A Inner Join Socks_Yarn_Tax_Details  B On A.RowID = B.Master_ID Group by A.RowID )A Where RowID = " + Dtmm.Rows[m]["RowID"] + " Group by RowID ";
                            MyBase.Load_Data(Str5, ref Dtmm5);

                            if (Dtmm3.Rows.Count > 0)
                            {
                                for (int i = 0; i <= Dtmm3.Rows.Count - 1; i++)
                                {
                                    if (Order.ToString() == String.Empty)
                                    {
                                        Order = Dtmm3.Rows[i]["Order_No"].ToString();
                                    }
                                    else
                                    {
                                        Order = Order + ", " + Dtmm3.Rows[i]["Order_No"].ToString();
                                    }
                                }
                            }
                        }
                        else if (Dtmm.Rows[m]["Mode"].ToString() == "Dye")
                        {
                            Str = " Select S1.PONo, L1.Ledger_Name Supplier, Cast(S1.PoDate As date)PoDate, S1.Required_Date,  S1.Commit_Date, (Case When S1.PO_Method = 0 Then 'OCN-WISE' When S1.PO_Method = 0 Then 'ITEM-WISE' End) PO_Method, L1.Ledger_Address Supplier_Address, L1.Ledger_Phone Supplier_Phone, L1.Ledger_email Supplier_Email From Socks_Yarn_PO_Dyeing_Master S1 left Join Supplier_All_Fn() L1 On L1.LEdgeR_Code = S1.Supplier_Code   Where S1.Approval_Flag = 'T' and S1.RowID = " + Dtmm.Rows[m]["RowID"];
                            MyBase.Load_Data(Str, ref Dtmm1);
                            Str1 = " Select Top 100000000 ROW_NUMBER()Over(Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size)SlNo, I1.Item + ' - ' + C1.color + ' - ' + S4.Size Item_Color_Size, Sum(S2.Order_Qty) Order_Qty, Sum(S2.Cancel_Qty) Cancel_Qty,  S2.Rate, Sum(S2.Order_Qty) * S2.Rate Amount, S1.PODate, S1.Required_Date From Socks_Yarn_PO_Dyeing_Master S1 Inner join Socks_Yarn_PO_Dyeing_Details S2 ON S1.RowID = s2.Master_ID Inner join Socks_Order_Master S3 on S2.Order_ID = S3.RowID Inner join item I1 on S2.Item_id = I1.itemid Inner join color C1 on s2.Color_id = c1.colorid Inner join size S4 on s2.Size_ID = S4.sizeid Where S1.RowID = " + Dtmm.Rows[m]["RowID"] + " GRoup by I1.Item ,C1.color, S4.Size , S2.Rate, S1.PODate, S1.Required_Date  Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size ";
                            MyBase.Execute_Qry(Str1, "Socks_Yarn_PO");

                            Str2 = " Select Top 2 S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From Socks_Yarn_PO_Dyeing_Tax_Details S1 Left Join Socks_Tax_Accounts() L1 on S1.Tax_Code = L1.Ledger_Code  Where S1.Master_ID = " + Dtmm.Rows[m]["RowID"] + " Order by S1.Slno ";
                            MyBase.Load_Data(Str2, ref Dtmm2);

                            Str3 = " Select Distinct S3.Order_No From Socks_Yarn_PO_Dyeing_Master S1 Inner join Socks_Yarn_PO_Dyeing_Details S2 ON S1.RowID = s2.Master_ID Inner join Socks_Order_Master S3 on S2.Order_ID = S3.RowID Where S1.RowID = " + Dtmm.Rows[m]["RowID"];
                            MyBase.Load_Data(Str3, ref Dtmm3);

                            Str4 = " Select Getdate()PrintOutDate";
                            MyBase.Load_Data(Str4, ref Dtmm4);

                            Str5 = " Select RowID, (Sum(Amt) + Sum(Amt1)) Amt From (Select A.RowID, Sum(Amount) Amt, 0 Amt1 From Socks_Yarn_PO_Dyeing_Master A Inner Join Socks_Yarn_PO_Dyeing_Details B On A.RowID = B.Master_ID Group by A.RowID Union Select A.RowID, 0 Amt, Sum(Tax_Amount) Amt1 From Socks_Yarn_PO_Dyeing_Master A Inner Join Socks_Yarn_PO_Dyeing_Tax_Details B On A.RowID = B.Master_ID Group by A.RowID )A Where RowID = " + Dtmm.Rows[m]["RowID"] + " Group by RowID ";
                            MyBase.Load_Data(Str5, ref Dtmm5);
                            if (Dtmm3.Rows.Count > 0)
                            {
                                for (int i = 0; i <= Dtmm3.Rows.Count - 1; i++)
                                {
                                    if (Order.ToString() == String.Empty)
                                    {
                                        Order = Dtmm3.Rows[i]["Order_No"].ToString();
                                    }
                                    else
                                    {
                                        Order = Order + ", " + Dtmm3.Rows[i]["Order_No"].ToString();
                                    }
                                }
                            }
                        }
                        else
                        {
                            return;
                        }

                        CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RptPurchasePO.rpt");

                        FormulaFill(ref ObjRpt, "Heading", "YARN PURCHASE ORDER");

                        FormulaFill(ref ObjRpt, "Supplier", Dtmm1.Rows[0]["Supplier"].ToString());
                        FormulaFill(ref ObjRpt, "Supplier_Address", Dtmm1.Rows[0]["Supplier_Address"].ToString().Replace("\r\n", "__"));
                        FormulaFill(ref ObjRpt, "Supplier_Phone", Dtmm1.Rows[0]["Supplier_Phone"].ToString());
                        FormulaFill(ref ObjRpt, "Supplier_Email", Dtmm1.Rows[0]["Supplier_Email"].ToString());

                        FormulaFill(ref ObjRpt, "PONo", Dtmm1.Rows[0]["PONo"].ToString());
                        FormulaFill(ref ObjRpt, "PoDate", String.Format("{0:dd-MMM-yyyy}", Dtmm1.Rows[0]["PoDate"].ToString()));
                        FormulaFill(ref ObjRpt, "ReqDate", String.Format("{0:dd-MMM-yyyy}", Dtmm1.Rows[0]["Required_Date"].ToString()));
                        FormulaFill(ref ObjRpt, "PO_Method", Dtmm1.Rows[0]["PO_Method"].ToString());
                        FormulaFill(ref ObjRpt, "PrintOutDate", Dtmm4.Rows[0]["PrintOutDate"].ToString());
                        FormulaFill(ref ObjRpt, "Net_Amount_Word", MyBase.Rupee(Convert.ToDouble(Dtmm5.Rows[0]["Amt"].ToString())));
                        if (Dtmm2.Rows.Count > 0)
                        {
                            for (int i = 0; i <= Dtmm2.Rows.Count - 1; i++)
                            {
                                if (i == 0)
                                {
                                    FormulaFill(ref ObjRpt, "Tax1", Dtmm2.Rows[0]["Tax"].ToString());
                                    FormulaFill(ref ObjRpt, "Tax1_Per", Dtmm2.Rows[0]["Tax_Per"].ToString());
                                    FormulaFill(ref ObjRpt, "Tax1_Amount", Dtmm2.Rows[0]["Tax_Amount"].ToString());
                                }
                                else if (i == 1)
                                {
                                    FormulaFill(ref ObjRpt, "Tax2", Dtmm2.Rows[1]["Tax"].ToString());
                                    FormulaFill(ref ObjRpt, "Tax2_Per", Dtmm2.Rows[1]["Tax_Per"].ToString());
                                    FormulaFill(ref ObjRpt, "Tax2_Amount", Dtmm2.Rows[1]["Tax_Amount"].ToString());
                                }
                            }
                        }
                        FormulaFill(ref ObjRpt, "Net_Amount", (Dtmm5.Rows[0]["Amt"].ToString()));
                        FormulaFill(ref ObjRpt, "Order", Order.ToString());

                        CReport_Normal_PDF(ref ObjRpt, "Yarn Purchase Order..!", "C:\\Vaahrep\\GainupPO.Pdf", false);
                        MyBase.sendEMailThroughOUTLOOK_Send(Dtmm.Rows[m]["Ledger_Email"].ToString(), "kumareshkanna@gainup.in", " Purchase Order..!", " ", "C:\\Vaahrep\\GainupPO.pdf");
                        if (Dtmm.Rows[m]["Mode"].ToString() == "Yarn")
                        {
                            MyBase.Run("Update Socks_Yarn_PO_Master Set Ack_Date = Getdate() Where RowID = " + Dtmm.Rows[m]["RowID"] + "", "Insert into Socks_PO_Mail_Log_Details (POMasID, MailID, Mode) Values (" + Dtmm.Rows[m]["RowID"] + ", '" + Dtmm.Rows[m]["Ledger_Email"].ToString() + "', 'Yarn')");
                        }
                        else if (Dtmm.Rows[m]["Mode"].ToString() == "Dye")
                        {
                            MyBase.Run("Update Socks_Yarn_PO_Dyeing_Master Set Ack_Date = Getdate() Where RowID = " + Dtmm.Rows[m]["RowID"] + "", "Insert into Socks_PO_Mail_Log_Details (POMasID, MailID, Mode) Values (" + Dtmm.Rows[m]["RowID"] + ", '" + Dtmm.Rows[m]["Ledger_Email"].ToString() + "', 'Dye')");
                        }
                        else
                        {
                            return;
                        }
                    }
                }

            }
        }

        public String Rotate_String(String Str)
        {
            try
            {
                return Str.Substring(1, Str.Length - 1) + Str.Substring(0, 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            try
            {
                Reminder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem23_Click(object sender, EventArgs e)
        {

        }

        private void menuMasterUpdationToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void userMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmUserMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void MDIMain_Deactivate(object sender, EventArgs e)
        {
            try
            {
                //Clipboard.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void groupMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void itemSubGroupMasterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        public void CReport_Normal_PDF(ref CrystalDecisions.CrystalReports.Engine.ReportDocument Rpt, String Caption, String PDF_FileName, Boolean Message_Flag)
        {
            try
            {
                FrmCRViewer Frm = new FrmCRViewer();
                Frm.View_PDF(ref Rpt, PDF_FileName, Message_Flag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmSingleItemBarcode(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void needleMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void userMasterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmUserMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public String EntryLog(String EntryName, String EntryType, String EntryID)
        {
            String Str;
            try
            {
                Str = "insert into USERENTRY_LOG( MODULE , USER_CODE , ENTRY_NAME , ENTRY_TYPE, ENTRY_ID, COMPANY_CODE, YEAR_CODE) values ('FLOOR', " + UserCode + " , '" + EntryName + "', '" + EntryType + "', " + EntryID + ", " + CompCode + ", '" + YearCode + "')";
                return Str;
            }
            catch (Exception ex)
            {
                return "sdf---";
            }
        }


        private void permissionMasterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (UserName.ToUpper() == "ADMIN")
                {
                    ShowChild(new Frm_Projects_Permission_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void menuMasterUpdationToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Sure to Update Menu Items ...!", "Socks", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MenuMaster();
                    MessageBox.Show("Ok ..!", "Socks");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void materialRequirementStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_YARNSTATUS_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void closeAllToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            String Str = String.Empty;
            System.Net.IPAddress[] IPList;
            try
            {
                foreach (System.Net.IPAddress IP in System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList)
                {
                    Str = IP.ToString();
                }
                MyBase.SqlCn_Open();
                MessageBox.Show(" *** " + MyBase.SqlCn.DataSource + " ***   ON : " + Str);
                MyBase.SqlCn_Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void stageImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Sure to Import ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    MyBase.Run("exec Check_T1", "exec Insert_Stage_item_Master", "exec Get_Itemcode_From_Stage", "exec Get_Sample_Master_From_Stage", "exec Get_Sample_Details_From_Stage", "exec Insert_Style_From_Sample_Master", "EXEC Insert_Stage_Color", "EXEC Insert_Stage_Item", "EXEC Insert_Stage_Count");
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Successfully Imported from Stage..!", "Gainup");
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void knittingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    ShowChild(new FrmFloorKnitting(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void linkingMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void feederMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void instructionMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void processMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void sampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSample(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            
        }

        private void linkingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmFloorLinking(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void salesInvoiceOffsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void washingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmFloorWashing(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void styleMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void sizeMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void productionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void productMasterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
               // ShowChild(new FrmFloorSetting(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void measurementMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem9_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmFloorKnitting(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmFloorPairing(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void packingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmFloorPacking(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
                //ShowChild(new FrmFloorPacking_FGS(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void machineGroupEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void employeeAllocationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void stoppageReasonMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void stoppageEntryForUnknownReasonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void stoppageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void qCProblemMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void knittinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocks_Knitting_QC_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void stockReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void monthlyStockReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_STOCK_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void orderEnquiryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void actionNameMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmTimeActionNameMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void leadDaysMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmTimeActionLeadTimeMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void leadDaysSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmTimeActionSettingMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void planninToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmTimeActionPlanEntry_Socks(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
                    
        }

        private void completionEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Completion_Entry(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }    
            
        }

        private void lotEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void completionEntryMultipleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Completion_Entry(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        private void orderClosingEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void fitBillsEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Accounts_Input = false;
            //    ShowChild(new FrmFit_Bill_Entry(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void fitBillsRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_STORES_FIT_BILL_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }

        private void tAToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void rateEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void packingEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void wagesApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void dyedYarnRequirementReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void yarnDyeingDeliveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void yarnDyeingReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void yarnDyeingInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void machinePlanningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void dailyInwardRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void embroideryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            
        }

        private void productionPlanningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void approvalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmRgpApprove(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cancelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmRgpCancel(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void approvalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void sampleCycleTimeChangeEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmTimeActionPlanEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void barcodePrintBundleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmBarcodePrint(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem14_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmFloorKnitting(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem9_Click_2(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksBarcodeScanner(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void greyStoreBarcodePrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmGreyStoreBarcodePrint(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void greyStoreBarcodeMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmGreyStoreMapping(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void linkingBarcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmLinkingBarcode(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void settingBarcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSettingBarcode(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void washingBarcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmWasingBarcode(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void workInProgressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_SUPPLIERWISE_OUTSTANDING_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void coveringRequirementEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void interofficeMemoEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmInterOfficeMemoEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void interofficeMemoApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmInterOfficeMemoApproval(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            
        }

        private void machineUtilizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void attendanceStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            
        }

        //private void toolStripMenuItem17_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ShowChild(new FrmProductionApproval(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void toolStripMenuItem17_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmActualProductionEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void capacityReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void pairingRejectionReasonMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void pairingProductionEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmPairingProductionWithRejection(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmComplaintEntry(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void toolStripMenuItem19_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (UserName.ToUpper() == "GKA0081" || UserName.ToUpper() == "ADMIN" || UserName.ToUpper() == "MD")
        //        {
        //            ShowChild(new FrmBudgetApproval(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void toolStripMenuItem20_Click_1(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem23_Click_1(object sender, EventArgs e)
        {
            
        }

        #region for Complaints

        void Complaint_Details()
        {
            try
            {
                if (Get_Complaints(Emplno) > 0)
                {
                    if (MessageBox.Show(Get_Complaints(Emplno).ToString() + " Complaint[s] are Pending Against for You. Do you want to See ? ", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        View_Browser("MIS_COMPLAINT_EMPLOYEE", Emplno);
                    }
                    else
                    {
                        if (Get_Complaints_Assigned(Emplno) > 0 && Emplno == 4111)
                        {
                            if (MessageBox.Show(Get_Complaints(Emplno).ToString() + " Complaint[s] are Pending Assigned for You. Do you want to See ? ", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                View_Browser("MIS_COMPLAINT_EMPLOYEE", Emplno);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        Int32 Get_Complaints(Int32 Emplno)
        {
            try
            {
                DataTable Dt = new DataTable();
                MyBase.Load_Data("Select COunt(*) Status From Vaahini_ERP_Gainup.DBo.MIS_Complaint_Details () Where To_Emplno = " + Emplno + " And Complete_Flag1 = 'Pending' and Moved_Flag = 'N'", ref Dt);
                return Convert.ToInt32(Dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        Int32 Get_Complaints_Assigned(Int32 Emplno)
        {
            try
            {
                DataTable Dt = new DataTable();
                MyBase.Load_Data("Select COunt(*) Status From Vaahini_ERP_Gainup.DBo.MIS_Complaint_Details () Where Complete_Flag1 = 'Pending' and Moved_Flag = 'Y'", ref Dt);
                return Convert.ToInt32(Dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion

        private void toolStripMenuItem24_Click(object sender, EventArgs e)
        {

        }

        private void memoRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem25_Click(object sender, EventArgs e)
        {

        }

        private void modelPriceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void modelPriceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void pOWiseOutstandingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_POWISE_OUTSTANDING", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem26_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_INTERVIEW_CANDIDATE", Emplno);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ageWiseStockReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_AGEWISE_STOCK_HOME", Emplno);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void yarnStoreLocationMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem28_Click(object sender, EventArgs e)
        {

        }

        private void knittingToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmProduction(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pairingPackingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Pack_Pair_Empl_Allocation_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem29_Click(object sender, EventArgs e)
        {

        }

        private void itemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmMasterSegItem(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmMasterSegColor(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void sizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmMasterSegSize(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void toolStripMenuItem30_Click(object sender, EventArgs e)
        {

        }

        private void oCNWithoutPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem31_Click(object sender, EventArgs e)
        {

        }

        private void lotEntryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void lotEntryNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void yarnStoreBarcodePrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void yarnIndentFromFloorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void yarnIssueFromStoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void yarnTransferEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void boardingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Board_Empl_Allocation(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem24_Click_1(object sender, EventArgs e)
        {

        }

        private void rGPRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_STORES_RGP_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem25_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem28_Click_1(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem29_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem30_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem34_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem35_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void barcodeDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void barcodeDetailsNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void toolStripMenuItem36_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem37_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem38_Click(object sender, EventArgs e)
        {

        }

        private void memoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem39_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmInterOfficeMemoApprovalAuthorize(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void yarnPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmSocksYarnPOEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void accessoriesPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksTrimsPOEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void yarnGRNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmSocksYarnGRN(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void accessoriesGRNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmSocksTrimsGRN(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void invoicingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmProjectGrnInvoicing(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void orderEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmProjectOrderMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void planningEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmProjectPlanningEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void budgetApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmPRojectBudgetApproval_New(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void yarnQualityEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksYarnQualityEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void palletwiseStockReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            try
            {
                View_Browser("MIS_SOCKS_PALLETWISE_STOCK_HOME", Emplno);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        
        }

        private void yarnPOApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksYarnPOApproval(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void accessoriesPOApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
               // ShowChild(new FrmSocksTrimsPOApproval(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem25_Click_2(object sender, EventArgs e)
        {
            
        }

        private void sampleRequirementEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSampleReqEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem31_Click_1(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem34_Click_1(object sender, EventArgs e)
        {
            
        }

        private void yarnPOStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void yarnReturnKnittingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Yarn_Return_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem35_Click_1(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem36_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem38_Click_1(object sender, EventArgs e)
        {

        }

        private void bookingCarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmVehicleCarBooking(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bookingGoodsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmVehicleBookingEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem36_Click_2(object sender, EventArgs e)
        {
            
        }

        private void yarnIndentCoveringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void yarnIssueCoveringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void vehicleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem38_Click_2(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Covering_RawMaterial_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void budgetApprovalViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmPRojectBudgetApproval_New(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void yarnIndentWashingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void yarnIssueWashingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void supplierReturnReProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmYarnRet(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem41_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksSampleYarnPOEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem42_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksSampleYarnPOApproval(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem43_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksSampleYarnGRN(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem44_Click(object sender, EventArgs e)
        {
            
        }

        private void accessoriesReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Trims_Retunr(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem45_Click(object sender, EventArgs e)
        {
            
        }

        private void rackWiseStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_YARN_STORE_RACKWISE_STOCK_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem46_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem47_Click(object sender, EventArgs e)
        {
            
        }

        private void dyeingGRNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksYarnGRNDyeing(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dyeingPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksYarnPOEntryDyeing(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbMenuList_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Load_Menu_Combo();
                CmbMenuList.Select(CmbMenuList.Text.Length, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Load_Menu_Combo()
        {
            DataTable Dt1 = new DataTable();
            try
            {
                CmbMenuList.Items.Clear();
                if (UserName.ToUpper() == "MD" || UserName.ToUpper() == "ADMIN")
                {
                    MyBase.Load_Data("Select Distinct Substring(upper(Replace(M1.Menu_Name, '&', '')), 1, 1) + '' + Substring(Lower(Replace(M1.Menu_Name, '&', '')), 2, Len(Replace(M1.Menu_Name, '&', '')) -1) Menu, M1.Menu_CName From PRojects.dbo.Projects_Menu_Master_New  M1 Where Replace(M1.Menu_Name, '&', '') like '%" + CmbMenuList.Text + "%' Order By Menu", ref Dt1);
                }
                else
                {
                    MyBase.Load_Data("Select Distinct Substring(upper(Replace(M1.Menu_Name, '&', '')), 1, 1) + '' + Substring(Lower(Replace(M1.Menu_Name, '&', '')), 2, Len(Replace(M1.Menu_Name, '&', '')) -1) Menu, M1.Menu_CName From PRojects.dbo.Projects_Menu_Master_New  M1 inner join Projects.dbo.projects_Permission_Master P1 on M1.Menu_CName = P1.Menu_Name Where P1.User_ID = " + UserCode + " and Replace(M1.Menu_Name, '&', '') like '%" + CmbMenuList.Text + "%' Order By Menu", ref Dt1);
                }

                for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                {
                    CmbMenuList.Items.Add(Dt1.Rows[i]["Menu"].ToString());
                }

                // This is Very Important;
                Menu_Dt = Dt1.Copy();
                // *** For Menu Settings Purpose
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbMenuList_SelectedIndexChanged(object sender, EventArgs e)
        {
            String Selected_Menu = String.Empty;
            try
            {
                DataTable Dt1 = new DataTable();
                if (CmbMenuList.Text.Trim() == String.Empty)
                {
                    return;
                }

                MyBase.Load_Data("Select upper(Menu_CName) Menu_CName From PRojects.dbo.Projects_Menu_Master_New  Where Replace(upper(Menu_Name), '&', '') = upper('" + CmbMenuList.Text + "')", ref Dt1);
                if (Dt1.Rows.Count == 0)
                {
                    return;
                }

                Selected_Menu = Dt1.Rows[0][0].ToString();
                for (int i = 0; i <= menuStrip.Items.Count - 1; i++)
                {
                    if (menuStrip.Items[i] is System.Windows.Forms.ToolStripMenuItem)
                    {
                        if (menuStrip.Items[i] is ToolStripComboBox)
                        {
                            return;
                        }

                        ToolStripMenuItem Ct = (ToolStripMenuItem)menuStrip.Items[i];
                        for (int j = 0; j <= Ct.DropDownItems.Count - 1; j++)
                        {
                            if (Ct.DropDownItems[j] is System.Windows.Forms.ToolStripMenuItem)
                            {
                                ToolStripMenuItem Ct1 = (ToolStripMenuItem)Ct.DropDownItems[j];
                                for (int k = 0; k <= Ct1.DropDownItems.Count - 1; k++)
                                {
                                    if (Ct1.DropDownItems[k] is System.Windows.Forms.ToolStripMenuItem)
                                    {
                                        ToolStripMenuItem Ct2 = (ToolStripMenuItem)Ct1.DropDownItems[k];
                                        for (int l = 0; l <= Ct2.DropDownItems.Count - 1; l++)
                                        {
                                            if (Ct2.DropDownItems[l] is System.Windows.Forms.ToolStripMenuItem)
                                            {
                                                ToolStripMenuItem Ct3 = (ToolStripMenuItem)Ct2.DropDownItems[l];
                                                for (int m = 0; m <= Ct3.DropDownItems.Count - 1; m++)
                                                {
                                                    if (Ct3.DropDownItems[l] is System.Windows.Forms.ToolStripMenuItem)
                                                    {
                                                        ToolStripMenuItem Ct4 = (ToolStripMenuItem)Ct3.DropDownItems[l];
                                                        if (Ct4.Name.ToUpper() == Selected_Menu)
                                                        {
                                                            Ct4.PerformClick();
                                                            return;
                                                        }
                                                    }
                                                }
                                                if (Ct3.Name.ToUpper() == Selected_Menu)
                                                {
                                                    Ct3.PerformClick();
                                                    return;
                                                }
                                            }
                                        }
                                        if (Ct2.Name.ToUpper() == Selected_Menu)
                                        {
                                            Ct2.PerformClick();
                                            return;
                                        }
                                    }
                                }
                                if (Ct1.Name.ToUpper() == Selected_Menu)
                                {
                                    Ct1.PerformClick();
                                    return;
                                }
                            }
                        }
                        if (Ct.Name.ToUpper() == Selected_Menu)
                        {
                            Ct.PerformClick();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem48_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem49_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem50_Click(object sender, EventArgs e)
        {
            
        }

        private void pairingProductionEntryBarcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmPairingProductionEntryBarcode(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem51_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem52_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmGridPo(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void purchaseRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmGridPo(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem53_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem54_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmFileServer(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void budgetApprovalDecathlonRepeatOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new Accounts.FrmBudgetApproval_Decathlon(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void offsetSaleInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmRptOffSetSalesInvoiceReport(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void toolStripMenuItem55_Click(object sender, EventArgs e)
        {
            
        }

        private void pORejectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Accounts.FrmPoReject(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem56_Click(object sender, EventArgs e)
        {
            
        }

        private void generalStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Accounts.FrmGridGeneralStock(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pOToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void embroideryPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Accounts.Frm_Socks_Embroidery_PO(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void embroideryGRNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Accounts.Frm_Socks_Embroidery_GRN(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem57_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Accounts.Frm_Socks_Embroidery_Invoicing(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem58_Click(object sender, EventArgs e)
        {
            
        }

        private void tAEmplTaskPendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser_Time("MIS_SOCKS_TA_EMPLOYEEWISE_HOME".ToUpper(), Emplno, UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tAToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser_Time("MIS_TIME_ACTION_HOME".ToUpper(), Emplno, UserCode);
                //View_Browser("MIS_TIME_ACTION_HOME".ToUpper(), UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tANewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser_Time("MIS_SOCKS_TIME_ACTION_HOME".ToUpper(), Emplno, UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem24_Click_2(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem60_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem61_Click(object sender, EventArgs e)
        {
            
        }

        private void linkingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Linking_Without_Barcode_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void outsorcingDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser_Time("MIS_Socks_Embroidery_Po_Home".ToUpper(), Emplno, UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void knittingRejDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser_Time("MIS_Socks_Knit_QC_Rej_Home".ToUpper(), Emplno, UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem64_Click(object sender, EventArgs e)
        {
            
        }

        private void trimsPORegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmGridTrimsPo(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem65_Click(object sender, EventArgs e)
        {
            
        }

        private void cuttingEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmCuttingEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem67_Click(object sender, EventArgs e)
        {
            
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                if (MyBase.Get_RecordCount("Socks_PO_Auto_Mail_Systems", " System_Name = '" + System.Environment.MachineName.ToString() + "'") > 0)
                {
                    AutoMail_Po_Socks();
                }     
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void settingBarcodeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Setting_Barcode_Generation(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tightsProductionEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmProductionEntry_Scan(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tightsOrderCostingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmOrderCostingEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem68_Click(object sender, EventArgs e)
        {

        }

        private void paymentEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
               // ShowChild(new FrmContractPayment(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cuttingDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_CONT_CUT_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void issueEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
           try
            {
                //ShowChild(new FrmJobOrderIssueEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void receiptEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Socks_Job_Work_Outsourcing_GRN(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void jobOrderPrintApproval1stLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //if (UserName == "MD" || UserName == "ADMIN" || UserName == "GKA0081" || UserName == "GKA0312")
                //{
                    //ShowChild(new FrmSocksProcutionApproval(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem48_Click_1(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem70_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Empl_All_Knit(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem71_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem72_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new Frm_Leave_App(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem73_Click(object sender, EventArgs e)
        {
            
        }

        private void fGSReceiptEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem74_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem75_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem76_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem77_Click(object sender, EventArgs e)
        {
            
        }

        private void packingContractorEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
               // ShowChild(new FrmPackingConatractorEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void packingContractorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_CONT_PCK_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem78_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem79_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem80_Click(object sender, EventArgs e)
        {
            
        }

        private void stockRefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{

            //    if (MessageBox.Show("Sure to Refresh ? ", "Gainup", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            //    {

            //        MyBase.Execute(" Exec Fitsocks.Dbo.Socks_CLosed_Approved_Stock_To_General");

            //        String Str2 = " UPDATE A Set A.Despatch_Closed = 'Y' From Fitsocks.Dbo.Socks_Order_Master A Inner Join (Select A.Order_No, SUM(ISnull(B.Cur_Stock, 0))Stock From Fitsocks.Dbo.Fit_Order_Status A ";
            //        Str2 = Str2 + " Left Join Fitsocks.Dbo.Socks_Store_Current_Stock()B On A.Order_No = B.Order_No ";
            //        Str2 = Str2 + " Where A.Status = 'N' And A.Need_Md_Approval = 'Y' ";
            //        Str2 = Str2 + " Group By A.Order_No Having SUM(ISnull(B.Cur_Stock, 0)) = 0)B On A.Order_No = B.Order_No ";

            //        MyBase.Execute(Str2);

            //        String Str1 = " UPDATE A Set A.Status = 'Y' From Fitsocks.Dbo.Fit_Order_Status A Inner Join (Select A.Order_No, SUM(ISnull(B.Cur_Stock, 0))Stock From Fitsocks.Dbo.Fit_Order_Status A ";
            //        Str1 = Str1 + " Left Join Fitsocks.Dbo.Socks_Store_Current_Stock()B On A.Order_No = B.Order_No ";
            //        Str1 = Str1 + " Where A.Status = 'N' And A.Need_Md_Approval = 'Y' ";
            //        Str1 = Str1 + " Group By A.Order_No Having SUM(ISnull(B.Cur_Stock, 0)) = 0)B On A.Order_No = B.Order_No ";

            //        MyBase.Execute(Str1);

            //        MessageBox.Show("Stock Moved...!", "Gainup");
            //        return;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    this.Cursor = Cursors.Default;
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void toolStripMenuItem81_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem82_Click(object sender, EventArgs e)
        {

        }

        private void needleIndentFromFloorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Needle_Indent_Request_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void needleIssueFromStoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Needle_Indent_Issue_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void allowanceActualWasteComparisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_YARN_WASTE_ESTIMATE_VS_ACTUAL", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem83_Click(object sender, EventArgs e)
        {
            
        }

        private void issueDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_SPARES_DETAILS_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void needleConsumptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Needle_Consumption_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem85_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem86_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmYarnPOAutomation(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tightsDeliveryEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Tights_Delivery_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tightsDeliveryAcknowledgementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Tights_Delivery_Acknowledgement(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void productionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_TIGHTS_PRODUCTION_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dCAcknowledgementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_TIGHTS_DELIVERY_ACKNOWLEDGEMENT_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem88_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem89_Click(object sender, EventArgs e)
        {
            
        }

        private void pLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_OFFSET_PANDL_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem91_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem92_Click(object sender, EventArgs e)
        {
            
        }
        
        private void toolStripMenuItem93_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem94_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem95_Click(object sender, EventArgs e)
        {
            
        }

        private void stoppageReasonMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmStoppageReasonMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void qCProblemReasonMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocks_QC_Problem_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pairingRejectionReasonMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmPairingRejectionMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void limitsPurchaseAutomationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSupplierLimits(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void supplierMailUpdationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Ledg_Email_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void orderClosingEntryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserName.ToUpper() == "ADMIN" || UserName.ToUpper() == "MD")
                {
                    //ShowChild(new FrmOrderCloseEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void orderClosingApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmOrderCloseApproval(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void linkingMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmLinkingMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void feederMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmFeederMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void instructionMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmInstructionMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void processMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmProcessMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void yarnStoreLocationMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Yarn_Store_Location_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void needleMasterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmNeedleMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void styleMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Style_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sizeMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Size_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void eANMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Accounts.Frm_EAN_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void productMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmProduct_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void measurementMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmMeasurementMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void modelMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmModelMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void modelPriceToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmModelPrice(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void orderEnquiryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmOrderEnquiry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void yarnDyeingDeliveryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void yarnDyeingReceiptToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void yarnDyeingInvoiceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void fGSTransferEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Fgs_Stock_Transfer(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fGSDespatchEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmFGSDespatchEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void needleSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmNeedleSetting(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void machinePlanningToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmMachinePlanning(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void productionPlanningToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmMachineProduction(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void specialRequisitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksYarnSplRequestation(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void oTApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_OT_Entry_New(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void oTEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_OT_Entry_Superviser(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void defaultAttendanceEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Default_Attendance_Supervisor(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void employeePermissionEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmPermissionEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void employeeLeaveEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Leave_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void leaveODPermissionApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Leave_Approval(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void nRGPApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmNRgpApprove(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem25_Click_3(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("");
                //ShowChild(new FrmNRgpCancel(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void stoppageEntryManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmStoppageEntryALL(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void stoppageEntryAutomatedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Socks_Machine_Stoppage(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void stoppageEntryMultipleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmMachineStoppageCumulative(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sampleCycleTimeChangeEntryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserName.ToString().ToUpper() == "ADMIN")
                {
                    //ShowChild(new FrmSampleCycleTimeChangeEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void machineGroupEntryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmDepartmentGroupEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void orderInHandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_ORDER_IN_HAND", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void orderHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_ORDER_HISTORY_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void eligibleForOrderClosingStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_ORDERS_TO_CLOSE", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sampleDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_SAMPLE_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fGSReceivedRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_JOB_ORDER_STATUS_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fGSStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_FGS_STOCK_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void socksProductionTABToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("FRMSOCKSLOGIN", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void socksProductionTimeSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserName.ToUpper() == "ADMIN")
                {
                    View_Browser("FRMSOCKSPRODUCTIONTIMESETTINGS", UserCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bPRReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_BPR_DEATIALS", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cKReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_CK_ABSTRACT", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void productionToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_FLOOR_SOCKS_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void productionOrderWiseLinkingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_ORDERWISE_LINKING_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void packingConveyorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_Socks_Conveyor_Production_Report_Home".ToUpper(), UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void productionAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_MACHINE_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void machineUtilizationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_MACHINE_UTILIZATION_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem48_Click_2(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_FLOOR_SOCKS_PRODUCTION_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }    
        }

        private void attendanceStatusToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_Attendance_Home", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem11_Click_1(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_ORDERWISE_PRODUCTION_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        private void employeeRecruitmentDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser_Time("MIS_EMPLOYEE_RECRUITMENT_HOME".ToUpper(), Emplno, UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buyerwiseKnittingEfficiencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser_Time("MIS_SOCKS_BUYERWISE_EFF_HOME".ToUpper(), Emplno, UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void socksDailyProductionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_Daily_Knit_Prod_Home", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void memoRegisterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_INTEROFFICEMEMO_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bOMVsToBeReceivedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser_Time("MIS_SOCKS_BOM_YARN_REQ_HOME".ToUpper(), Emplno, UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void coveringYarnStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_COVERING_PO_STATUS_MASTER", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pOGRNStatusMOQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_YARNMOQSTATUS_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem24_Click_3(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_YARNDYEINGREQ_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }

        private void toolStripMenuItem51_Click_1(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_YARN_GRN_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        private void toolStripMenuItem58_Click_1(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_CAPACITY_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void oCNWithoutPOExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_ORDER_EXPORT_DETAILS", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void yarnComparisonPlanningActualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_YARN_CONSUMPTION_REPORT_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void machineCountOperatorWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_OPERATOR_MACHINEWISE_INCREMENT_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem20_Click_2(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_PURCHASE_GRN_PENDING_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void stoppageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_STOPPAGE_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void yarnPOStatusToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_PO_GRN_PENDING_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fileRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_FILE_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fileServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // View_Browser("MIS_FILESERVER_HOME", Emplno);
                //ShowChild(new FrmFileServer(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fGSReceiptEntryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Floor_FGS_Receipt_Entry_Jono(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem36_Click_3(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmContractRate(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void packingEntryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmPackingDetails(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void wagesEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void wagesApprovalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmContractWorkerWages(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lotEntryToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocks_LotEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lotEntryNewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_VSocks_Grn_LotEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void yarnStoreBarcodePrintToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmBarcodeDetails(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void yarnStoreCurrentStockBarcodePrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Socks_Stock_Barcode_Print(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void indentFromFloorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Yarn_Indent_Request_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fromLinkingStitchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Yarn_Indent_Request_Link_Stiching(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fromStoreToLinkingStitchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Yarn_Indent_Issue_Link_Stitch(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fromStoreToKnittingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Yarn_Indent_Issue_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fromCoveringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Yarn_Indent_Request_Covering(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
                //ShowChild(new Frm_Yarn_Indent_Request_Covering_POwise(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fromStoreToCoveringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Yarn_Indent_Issue_Covering_Powise(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fromWashingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Yarn_Indent_Request_Washing(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fromStoreToWashingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Yarn_Indent_Issue_Washing(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fromSampleDeptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSampleYarnIndentEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fromStoreToSampleDeptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Accounts.FrmSampleYarnIssueEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void barcodeDetailsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmYarnBarcodeDetails(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void barcodeDetailsNewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmYarnBarcodeDetailsNew(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void itemwiseTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Yarn_Transfer_New(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void multipleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Yarn_Transfer_Multiple(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void productionEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Covering_Prod_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void coveringRequirementEntryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmCoveringrequirement(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deliveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Socks_Dyeing(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void receiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Socks_Dyeing_Receipt(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void invoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Socks_Dyeing_Invoice(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem53_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmGenInvoice(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void incentiveReportNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_DEPTWISE_INCENTIVE_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem55_Click_1(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmProjectGeneralPOEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem56_Click_1(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmProjectGeneralPOApproval(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem60_Click_1(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmProjectGRNEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void technicianAllocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Technician_Allocation(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void qualityDepartmentEmployeeAllocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Quality_Employee_Allocation(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void jobOrderPrintApproval2ndLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserName == "MD" || UserName == "ADMIN" || UserName == "GM")
                {
                    //ShowChild(new FrmSocksProductionApproval_2ndLevel(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fGSTransferApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
              
                    //ShowChild(new Frm_FGS_Stock_Transfer_Approval(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void orderImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmOrderImport (), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buyerItemSampleLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksSamplePlanningDetails(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void knittingQCEmployeePairingRejectionDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_KNIT_QC_PAIR_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void allEmployeeAllocationNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmLine_Incharge_New(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem61_Click_1(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmRgpEntry(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem64_Click_1(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmNrgpEntry(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void balanceSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_KNIT_BALANCE_SHEET_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pOPerformanceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_PO_PER_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void floorWiseStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_Socks_Floor_Stock_Home", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void embroideryProductionEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmEmbroidery(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void embroideryDeliveryChallanEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Garments_Embroidery_Delivery_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void embroideryDeliveryAcknowledgementEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Garments_Embroidery_Delivery_Acknowledgement(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem71_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksYarnPOEntry_Moq(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void indentFromFloorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
               // ShowChild(new Frm_Trims_Indent_Request_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void issueFromTrimsStoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
               // ShowChild(new Frm_Trims_Indent_Issue_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem74_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Yarn_Indent_Request_Multiple(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void colorMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Color_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem75_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksYarnPOApprovalMoq(), Window.Normal, true , false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void productionPlanningMultipleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Production_Planning_New(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void outPassRequestEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksOrderApproval(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void outPAssRequestApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksOrderApprovalTool(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void embroideryAllocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Embroidery_Employee_Allocation(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void productionEntryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmOffsetProd(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void keyPointMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Compliance_Audit_Points_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void evaluationEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Compliance_Auditing(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void budgetBreakupEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Embroidery_Budget_Breakup_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void outsourcingInvoiceEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Socks_Outsourcing_Receipt_Invoicing(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void itemMasterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmItemMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void productionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser_Time("MIS_SOCKS_EMBROIDERY_PROD_HOME".ToUpper(), Emplno, UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void employeeAllocationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser_Time("MIS_EMBROIDERY_EMPLOYEE_ALLOCATION_HOME".ToUpper(), Emplno, UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void budgetMarginReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserName.ToUpper() == "MD")
                {
                    //ShowChild(new FrmSocksGridMargin(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void productionDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_OFFSETPRINTING_PROD_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem76_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmProductionPlanningImportWholeUnit(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem77_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Checking_Contractor_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkingDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_CHECKING_CONTRACTOR_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void eANDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmEanConvertion(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void orderStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_ORDER_STATUS", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void attendanceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_ATTENDANCE_RPT_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void socksProduction1LevelApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_Socks_Order_Production_Approval1", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fGSStockDeliveryRequestFromFloorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Floor_FGS_Receipt_Delivery_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
                //ShowChild(new FrmFGSDeliveryEntryMultiple(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fGSStockAcknowledgementFromGodownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Floor_FGS_Receipt_Acknowledgement_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
                //ShowChild(new Frm_FGS_Acknowledgement_Entry_Multiple(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem78_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Linking_To_Washing_Delivery_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem80_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Linking_To_Washing_Acknowledge_Entry(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem81_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Washing_To_Boarding_Delivery_Entry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem83_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Washing_To_Boarding_Acknowledge_Entry(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem88_Click_1(object sender, EventArgs e)
        {
            try
            {
               // ShowChild(new FrmSocksOrderDateChange(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void supplyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Led_Tv_Display_Supply(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void indusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Led_Tv_Display_Indus(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void trendChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Led_Tv_Display_Trend_Chart(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fGSStockRackwiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_Socks_Fgs_Rackwise_Stock_Home", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chemicalIssueEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmIssueEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void jobOrderOutsourcingStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_SOCKS_JOB_ORDER_OUTSOURCING_HOME", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem89_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_DesigWise_Evaluation(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem92_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new Frm_Sewing_Barcode(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void employeeSkillMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_Designation_Eval_Home", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void footlessDetalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_Socks_Footless_Contractor_Home", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem93_Click_1(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_VedioPlayer", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem94_Click_1(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_VideoHome", UserCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mOQPoSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmSocksYarnMoqPoSettings(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void stoppageEntryLinkingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmStoppageEntryLinkingALL(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void vendorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmVendorForm(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void vendorFormApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmVendorApprovalForm(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gRNApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FRMGRNAPPROVAL(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void invoicingBillAttachmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FrmGrnInvoicing_imgAttach(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void invoicingBillApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ShowChild(new FRMINVOICINGAPPROVAL(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem79_Click_1(object sender, EventArgs e)
        {

        }

        private void lEDTVDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void floorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem34_Click_2(object sender, EventArgs e)
        {

        }

        private void dyeingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void projectMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new Frm_Projects_Name_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void activityNameMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            try
            {
                ShowChild(new Frm_Projects_Activity_Name_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem6_Click_1(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmProjectIndentPOEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void purchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmProjectPOEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mandaysEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmTestingPo(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void returnEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmProjectReturnEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void combinedDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmGridBigs(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem8_Click_1(object sender, EventArgs e)
        {
            try
            {
                View_Browser("MIS_IT_TICKET_FRM", Convert.ToInt32(Emplno));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pOAdvanceEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new Floor.FrmPoAdvanceEntry(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem9_Click_3(object sender, EventArgs e)
        {
          try    
            {
                View_Browser("MIS_Hardware_Ticket",Emplno);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void billEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmFit_Bill_Entry(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void itemMasterToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmItemMaster(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void colorMasterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new Frm_Color_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sizeMasterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new Frm_Size_Master(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void mandaysApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new Frm_ManDays_Cost_Approval(), Window.Normal, true, false, Entry_Mode.Edit, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem11_Click_2(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new Frm_Staff_KPI_Entry(), Window.Normal, false, false, Entry_Mode.Edit, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void kPIApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new Frm_Staff_KPI_Approval(), Window.Normal, true, false, Entry_Mode.Edit, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void billEntryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmFit_Bill_Entry(), Window.Normal, true, false, Entry_Mode.Edit, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void resignApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new Frm_Resign_Approval(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sLNOWiseBreakupMachineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmMachSlno(), Window.Normal, false, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem12_Click_1(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new Frm_Staff_KPI_Approval(), Window.Normal, true, false, Entry_Mode.Edit, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem13_Click_2(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmOrganogram(Emplno), Window.Normal, true, false, Entry_Mode.Edit, Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void supplierMasterSocksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmSupplierMaster(),Window.Normal,false,false,Entry_Mode._New,Get_Menu_Control_Name((ToolStripMenuItem)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void masterGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowChild(new FrmMasterGrid(), Window.Normal, true, false, Entry_Mode._New, Get_Menu_Control_Name((ToolStripMenuItem)sender));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

     


       
    }
}

