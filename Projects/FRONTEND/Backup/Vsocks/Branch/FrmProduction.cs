using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Accounts
{
    public partial class FrmProduction : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        Int32 M = 0;
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        String S;
        String[] t;
        String[] Queries;
        TextBox Txt = null;
        TextBox Txt1 = null;
        Int64 Master_ID = 0;
        Int64 Detail_ID = 0;

        public FrmProduction()
        {
            InitializeComponent();
        }

        private void myTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmProduction_Load(object sender, EventArgs e)
         {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                MyBase.Clear(this);
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
                Grid_Data();
                Master_ID = 0; Detail_ID = 0;
                Enable_Control();
                DtpDate1.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Save()
        {
            
            Int32 Array_index = 0;
            try
            {
                Total_Count();
                if (TxtDivision.Text.Trim() == String.Empty || TxtType.Text.Trim() == String.Empty || TxtShift.Text.Trim() == String.Empty || TxtDivision.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtDivision.Focus();
                    return;
                }

                if (TxtEmployees.Text.Trim() == String.Empty)
                {
                    TxtEmployees.Text = "0";
                }

                if (Convert.ToDouble(TxtEmployees.Text) == 0)
                {
                    MessageBox.Show("Invalid Grid Details ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtDivision.Focus();
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Grid Details ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtShift.Focus();
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {

                    if (Grid["Technician", i].Value == null || Grid["Technician", i].Value == DBNull.Value || Grid["Technician", i].Value.ToString() == String.Empty || Grid["Supervisor", i].Value == null || Grid["Supervisor", i].Value == DBNull.Value || Grid["Supervisor", i].Value.ToString() == String.Empty || Grid["Machine", i].Value == null || Grid["Machine", i].Value == DBNull.Value || Grid["Machine", i].Value.ToString() == String.Empty || Grid["Machine", i].Value == null || Grid["Machine", i].Value == DBNull.Value || Grid["Employee", i].Value.ToString() == String.Empty )
                    {
                        MessageBox.Show("Invalid Data ...!", "Gainup");
                        MyParent.Save_Error = true;
                        Grid.CurrentCell = Grid["Department", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }

                    if (Check_Designation_Machine(i))
                    {
                        MessageBox.Show("Duplicate Designation , Machine & Employee ...!", "Gainup");
                        MyParent.Save_Error = true;
                        Grid.CurrentCell = Grid["Designation", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                }
               
                if (MyParent._New)
                {
                    DataTable TDt = new DataTable();
                    MyBase.Load_Data("Select (Isnull(Max(Entry_No), 0) + 1) No From Employee_Production_Master_Socks Where Division_Code = " + TxtDivision.Tag + " ", ref TDt);
                    TxtEno.Text = TDt.Rows[0][0].ToString();
                    Queries = new String[Dt.Rows.Count * 3];
                    Queries[Array_index++] = "Insert into Employee_Production_Master_Socks (Entry_Date, Division_Code, Shift_Code, Time_From, Time_To, Department_Code, Total_Employees, Entry_No) Values ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', " + TxtDivision.Tag.ToString() + ", " + TxtShift.Tag.ToString() + ", '" + String.Format("{0:dd-MMM-yyyy} {0:T}", DtpFromTime.Value) + "', '" + String.Format("{0:dd-MMM-yyyy} {0:T}", DtpToTime.Value) + "', " + TxtType.Tag.ToString() + ", " + TxtEmployees.Text + ", " + TxtEno.Text + "); Select Scope_Identity ()";
                }
                else
                {
                    Queries = new String[Dt.Rows.Count * 3];
                    Queries[Array_index++] = "Update Employee_Production_Master_Socks Set Time_From = '" + String.Format("{0:dd-MMM-yyyy} {0:T}", DtpFromTime.Value) + "', Time_To = '" + String.Format("{0:dd-MMM-yyyy} {0:T}", DtpToTime.Value) + "', Total_Employees = " + TxtEmployees.Text + " Where RowID = " + Master_ID;
                    Queries[Array_index++] = "Delete From Employee_Production_Details_Socks Where Master_ID = " + Master_ID;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_index++] = "Insert into Employee_Production_Details_Socks (Slno, Master_ID, Machine_Code, Machine_Name, Emplno, Technician_Emplno, Supervisor_Emplno, Remarks, Machine_FULL_Name) Select " + Grid["Slno", i].Value.ToString() + ", @@IDENTITY, Machine_Code, Machine_Name, " + Grid["EmplNo", i].Value.ToString() + ", " + Grid["Technician_Emplno", i].Value.ToString() + ", " + Grid["Supervisor_Emplno", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "', '" + Grid["Machine", i].Value.ToString() + "' From Department_Group_Master D1 Left Join Department_Group_Details D2 On D1.RowID = D2.Master_ID Where Group_Name = '" + Grid["Machine", i].Value.ToString() + "' And D1.Effect_From = (Select MAX(Effect_From) from Department_Group_Master Where Group_Name = '" + Grid["Machine", i].Value.ToString() + "') ";
                    }
                    else
                    {
                        Queries[Array_index++] = "Insert into Employee_Production_Details_Socks (Slno, Master_ID, Machine_Code, Machine_Name, Emplno, Technician_Emplno, Supervisor_Emplno, Remarks, Machine_FULL_Name) Select " + Grid["Slno", i].Value.ToString() + ", " + Master_ID + ", Machine_Code, Machine_Name, " + Grid["EmplNo", i].Value.ToString() + ", " + Grid["Technician_Emplno", i].Value.ToString() + ", " + Grid["Supervisor_Emplno", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "', '" + Grid["Machine", i].Value.ToString() + "' From Department_Group_Master D1 Left Join Department_Group_Details D2 On D1.RowID = D2.Master_ID Where Group_Name = '" + Grid["Machine", i].Value.ToString() + "' And Effect_From = (Select MAX(Effect_From) from Department_Group_Master Where Group_Name = '" + Grid["Machine", i].Value.ToString() + "') ";
                    }
                }
            
            
            if (MyParent._New)
            {
                MyBase.Run_Identity(false, Queries);
            }
            else
            {
                MyBase.Run_Identity(true, Queries);
            }
            MyParent.Save_Error = false;
            MessageBox.Show("Saved ..!", "Gainup");
        }
        catch (Exception ex)
    {
        MyParent.Save_Error = true;
        MessageBox.Show(ex.Message);
    }
}

        void Enable_Control()
        {
            try
            {
                if (MyParent._New)
                {
                    DtpDate1.Enabled = true;
                    TxtDivision.Enabled = true;                    
                    TxtShift.Enabled = true;
                    TxtType.Enabled = true;
                }
                else
                {
                    DtpDate1.Enabled = false;
                    TxtDivision.Enabled = false;                    
                    TxtShift.Enabled = false;
                    TxtType.Enabled = false;
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
                Enable_Control();
                Master_ID = Convert.ToInt64 (Dr["RowID"]);
                TxtEno.Text = Dr["ENo"].ToString();
                DtpDate1.Value = Convert.ToDateTime(Dr["PDate"]);
                TxtDivision.Tag = Dr["Division_Code"].ToString();
                TxtDivision.Text = Dr["Division"].ToString();               
                TxtShift.Text = Dr["Shift"].ToString();
                TxtShift.Tag = Dr["Shift_Code"].ToString();
                DtpFromTime.Value = Convert.ToDateTime(Dr["Time_From"]);
                DtpToTime.Value = Convert.ToDateTime(Dr["Time_To"]);
                TxtEmployees.Text = Dr["Total_Employees"].ToString();
                TxtType.Text = Dr["Type"].ToString();
                TxtType.Tag = Dr["Department_Code"].ToString();
                Total_Count();
                Grid_Data();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                //if (MyParent.UserName.Contains("ADMIN"))
                //{
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Production - Edit", "Select D1.DeptName Type, E1.Department_Code, E1.Entry_No ENo ,E1.Entry_Date PDate, S1.shiftcode2 Shift, E1.TIme_From, E1.Time_To, S1.shiftdesc, E1.Total_Employees, E1.Division_Code, E1.Shift_Code, E1.RowID, E1.Total Total_Qty, P1.CompName Division From Employee_Production_Master_Socks E1 Left Join VAAHINI_ERP_GAINUP.DBO.CompanyMas_pay P1 On E1.Division_Code = P1.Compcode Left Join VAAHINI_ERP_GAINUP.DBO.Shiftmst S1 On E1.Shift_Code = S1.shiftcode and E1.Division_Code = S1.Compcode LEft Join VAAHINI_ERP_GAINUP.DBO.DeptType D1 On E1.Department_Code = D1.DeptCode Where E1.Division_Code  = (Case When " + MyParent.CompCode + " = 2 Then 1 Else 2 End)  Order By  E1.Entry_No Desc ", String.Empty, 100, 80, 150, 80, 100, 100, 100);
                //}
                //else
                //{
                //    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Production - Edit", "Select D1.DeptName Type, E1.Entry_No ENo, E1.Entry_Date PDate, S1.shiftcode2 Shift, E1.TIme_From, E1.Time_To, S1.shiftdesc, E1.Total_Employees, E1.Division_Code, E1.Shift_Code, E1.RowID, E1.Total Total_Qty, P1.CompName Division From Employee_Production_Master_Socks E1 Left Join VAAHINI_ERP_GAINUP.DBO.Companymas_Pay P1 On E1.Division_Code = P1.Compcode Left Join VAAHINI_ERP_GAINUP.DBO.Shiftmst S1 On E1.Shift_Code = S1.shiftcode and E1.Division_Code = S1.Compcode LEft Join VAAHINI_ERP_GAINUP.DBO.DeptType D1 On E1.Department_Code = D1.DeptCode Where E1.Division_Code  = (Case When " + MyParent.CompCode + " = 2 Then 1 Else 2 End) and E1.Entry_Date =  Cast(GETDATE() as Date) Order By  E1.Rowid Desc ", String.Empty, 100, 80, 150, 80, 100, 100, 100);
                //}
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Machine", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Production - Delete", "Select D1.DeptName Type, E1.Entry_No ENo ,E1.Entry_Date PDate, S1.shiftcode2 Shift, E1.TIme_From, E1.Time_To, S1.shiftdesc, E1.Total_Employees, E1.Division_Code, E1.Shift_Code, E1.RowID, E1.Total Total_Qty, P1.CompName Division From Employee_Production_Master_Socks E1 Left Join VAAHINI_ERP_GAINUP.DBO.CompanyMas_pay P1 On E1.Division_Code = P1.Compcode Left Join VAAHINI_ERP_GAINUP.DBO.Shiftmst S1 On E1.Shift_Code = S1.shiftcode and E1.Division_Code = S1.Compcode LEft Join VAAHINI_ERP_GAINUP.DBO.DeptType D1 On E1.Department_Code = D1.DeptCode Where E1.Division_Code  = (Case When " + MyParent.CompCode + " = 2 Then 1 Else 2 End)  Order By  E1.Entry_No Desc ", String.Empty, 100, 80, 150, 80, 100, 100, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    MyParent.Load_DeleteConfirmEntry();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Delete_Confirm()
        {
            try
            {
                if (Master_ID > 0)
                {
                    MyBase.Run("Delete from Employee_Production_Details_Socks where Master_ID = " + Master_ID, "Delete from Employee_Production_Master_Socks where RowID = " + Master_ID);
                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyBase.Clear(this);
                    MyParent.Load_DeleteEntry();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Production - View", "Select D1.DeptName Type, E1.Entry_No ENo ,E1.Entry_Date PDate, S1.shiftcode2 Shift, E1.TIme_From, E1.Time_To, S1.shiftdesc, E1.Total_Employees, E1.Division_Code, E1.Shift_Code, E1.RowID, E1.Total Total_Qty, P1.CompName Division, E1.Department_Code From Employee_Production_Master_Socks E1 Left Join VAAHINI_ERP_GAINUP.DBO.CompanyMas_pay P1 On E1.Division_Code = P1.Compcode Left Join VAAHINI_ERP_GAINUP.DBO.Shiftmst S1 On E1.Shift_Code = S1.shiftcode and E1.Division_Code = S1.Compcode LEft Join VAAHINI_ERP_GAINUP.DBO.DeptType D1 On E1.Department_Code = D1.DeptCode Where E1.Division_Code  = (Case When " + MyParent.CompCode + " = 2 Then 1 Else 2 End)  Order By  E1.Entry_No Desc ", String.Empty, 100, 80, 150, 80, 100, 100, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                }
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
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmProduction_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtShift")
                    {
                        Grid.CurrentCell = Grid["Machine", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                    else if (this.ActiveControl.Name == "TxtEmployees")
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
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtDivision")
                    {

                        if (Dt.Rows.Count > 0)
                        {
                            if (MessageBox.Show("Sure to Clear Grid Detail ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                MyBase.Clear(this);
                                Grid_Data();
                                DtpDate1.Focus();
                            }
                            else
                            {
                                return;
                            }
                        }

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Division", "Select CompName Division, COmpCode Code From VAAHINI_ERP_GAINUP.DBO.Companymas_Pay Where Compcode  = (Case When " + MyParent.CompCode + " = 2 Then 3 Else 2 End)  ", String.Empty, 250);
                        if (Dr != null)
                        {
                            TxtDivision.Text = Dr["Division"].ToString();
                            TxtDivision.Tag = Dr["Code"].ToString();
                            TxtType.Text = "";
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtSupervisor")
                    {
                        if (TxtDivision.Text.ToString() != String.Empty)
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Name", "Select Name Supervisor, Tno, EmplNo Supervisor_Emplno From Employeemas Where designationcode in (8,184,214,219) and tno not like '%Z' and CompCode = " + TxtDivision.Tag + " ", String.Empty, 200);
                            if (Dr != null)
                            {
                                Txt1.Text = Dr["Supervisor"].ToString();
                                Grid["Supervisor", Grid.CurrentCell.RowIndex].Value = Dr["Supervisor"].ToString();
                                Grid["Supervisor_EmplNo", Grid.CurrentCell.RowIndex].Value = Dr["EmplNo"].ToString();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Division", "Gainup");
                            TxtDivision.Focus();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtShift")
                    {
                        if (TxtDivision.Text.Trim() == String.Empty || TxtType.Text.Trim() == String.Empty)
                        {
                            MessageBox.Show("Invalid Division or Type...!", "Gainup");
                            TxtDivision.Focus();
                            return;
                        }

                        if (Dt.Rows.Count > 0)
                        {
                            if (MessageBox.Show("Sure to Clear Grid Detail ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                MyBase.Clear(this);
                                Grid_Data();
                                DtpDate1.Focus();
                            }
                            else
                            {
                                return;
                            }
                        }
                        
                        //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Shift", "Select Shiftcode2 Shift, Shiftdesc, ShiftCode Code, Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' as Datetime) + Cast(Starttime as Datetime) Starttime,  Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' as Datetime) + (Case when cast(Datepart (hh, Cast(EndTime as datetime)) as int) < cast(Datepart (hh, Cast(StartTime as datetime)) as int) then  dateadd(d, 1, Cast(Endtime as Datetime)) else Cast(Endtime as Datetime) end) EndTime From shiftmst where Compcode = " + TxtDivision.Tag.ToString() + " and Shiftcode2 Not Like '%Z'", String.Empty, 50, 300);
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Shift", "Select Distinct Shiftcode2 Shift, Shiftdesc, ShiftCode Code, Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' as Datetime) + Cast(Starttime as Datetime) Starttime, Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' as Datetime) + (Case when cast(Datepart (hh, Cast(EndTime as datetime)) as int) < cast(Datepart (hh, Cast(StartTime as datetime)) as int) then  dateadd(d, 1, Cast(Endtime as Datetime)) else Cast(Endtime as Datetime) end) EndTime From VAAHINI_ERP_GAINUP.DBO.shiftmst A LEft Join Employee_Production_Master_Socks B On A.shiftcode = B.Shift_Code and B.Entry_Date = Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' as Date) and A.compcode = B.Division_Code where Compcode = " + TxtDivision.Tag.ToString() + " and Shiftcode2 Not Like '%Z' and shiftcode In (15,16,17) ", String.Empty, 50, 300);
                        if (Dr != null)
                        {
                            TxtShift.Tag = Dr["Code"].ToString();
                            TxtShift.Text = Dr["Shift"].ToString();
                            DtpFromTime.Value = Convert.ToDateTime(Dr["StartTime"]);
                            DtpToTime.Value = Convert.ToDateTime(Dr["EndTime"]);

                            DtpFromTime.MinDate = DtpDate1.Value;
                            DtpToTime.MinDate = DtpDate1.Value;
                        }

                    }
                    else if (this.ActiveControl.Name == "TxtType")
                    {
                        if (TxtDivision.Text.Trim() == String.Empty)
                        {
                            MessageBox.Show("Invalid Division ...!", "Gainup");
                            TxtDivision.Focus();
                            return;
                        }

                        if (Dt.Rows.Count > 0)
                        {
                            if (MessageBox.Show("Sure to Clear Grid Detail ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                MyBase.Clear(this);
                                Grid_Data();
                                DtpDate1.Focus();
                            }
                            else
                            {
                                return;
                            }
                        }

                        //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Type", "Select Name, RowID From Employee_Allocation_Type ", String.Empty, 300);
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Type", "select DISTINCT DeptName Name, DeptCode from VAAHINI_ERP_GAINUP.DBO.DeptType where compcode = (Case When " + MyParent.CompCode + " = 1 Then 2 Else 1 End) ", String.Empty, 300);
                        if (Dr != null)
                        {
                            TxtType.Tag = Dr["deptcode"].ToString();
                            TxtType.Text = Dr["Name"].ToString();                            
                        }

                    } 
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    if (this.ActiveControl.Name != String.Empty)
                    {
                        e.Handled = true;
                    }
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    MyBase.ActiveForm_Close(this, MyParent);
                }
                else
                {
                    if (this.ActiveControl is TextBox && this.ActiveControl.Name != String.Empty)
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

        void Grid_Data()
        {
            String Str = String.Empty;
            String Str1 = String.Empty;
            String Str2 = String.Empty;
            try
            {
                if (MyParent._New)
                {
                    Str = "Select P1.Slno, '' Machine_Code, '' Machine, E1.Name Employee, P1.Emplno, P1.Technician_Emplno, E2.Name Technician, P1.Supervisor_Emplno, E3.Name Supervisor, P1.Remarks, 1 Multiple From Employee_Production_Details_Socks P1 Left join VAAHINI_ERP_GAINUP.DBO.Employeemas E1 On P1.Emplno = E1.Emplno Left Join VAAHINI_ERP_GAINUP.DBO.Employeemas E2 On P1.Technician_Emplno = E2.Emplno Left Join VAAHINI_ERP_GAINUP.DBO.Employeemas E3 On P1.Supervisor_Emplno = E3.Emplno Where 1= 2";
                }
                else
                {
                    //Str = "Select P1.Slno, D2.DeptName Department, P1.Department_Code, P1.Supervisor_Emplno, E2.Name Supervisor, D1.DesignationName Designation, E1.Name Employee,  P1.Designation_Code, P1.Emplno, P1.Order_No, P1.Ref_No, P1.Machine_Code, P1.Machine_Name Machine, P1.Uom_Code, U1.Name Uom, P1.Production, P1.Remarks, 1 Multiple From Employee_Production_Details P1 Left join DesignationType D1 On P1.Designation_Code = D1.DesignationCode Left join Employeemas E1 On P1.Emplno = E1.Emplno Left Join Employeemas E2 On P1.Supervisor_Emplno = E2.Emplno Left join Uom_master U1 On P1.UOm_Code = U1.RowID LEft Join DeptType D2 On D2.DeptCode = P1.Department_Code Where P1.Master_ID = " + Master_ID + " Order By P1.Slno";
                    Str = "Select distinct P1.SlNo, '' Machine_Code, P1.Machine_FULL_Name Machine, E1.Name Employee, P1.EmplNo, E2.Name Technician, P1.Technician_Emplno, E3.Name Supervisor, P1.Supervisor_Emplno, P1.Remarks, 1 Multiple From Employee_Production_Details_Socks P1 Left Join Employee_Production_Master_Socks P2 On P1.Master_ID = P2.RowID Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 On P1.EmplNo = E1.Emplno Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E2 On P1.Technician_Emplno = E2.Emplno Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E3 On P1.Supervisor_Emplno = E3.Emplno Where P1.Master_ID = " + Master_ID + " Order By P1.Slno";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Emplno", "Machine_Code", "Multiple", "Technician_Emplno", "Supervisor_Emplno");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid, "Employee", "Supervisor", "Technician", "Machine", "Remarks");
                MyBase.Grid_Width(ref Grid, 50, 120, 150, 150, 150, 150, 150);
                Grid.RowHeadersWidth = 10;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmProduction_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name != String.Empty)
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

        private void Grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt == null)
                {
                    Txt = (TextBox)e.Control;
                    Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                    Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);
                    Txt.GotFocus += new EventHandler(Txt_GotFocus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void Txt_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Supervisor"].Index)
                {
                    MyBase.Row_Number(ref Grid);
                    if (Grid.CurrentCell.RowIndex > 0 && Grid["Supervisor", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["Supervisor_Emplno", Grid.CurrentCell.RowIndex].Value = Grid["Supervisor_Emplno", Grid.CurrentCell.RowIndex - 1].Value;
                        Grid["Supervisor", Grid.CurrentCell.RowIndex].Value = Grid["Supervisor", Grid.CurrentCell.RowIndex - 1].Value;
                        Txt.Text = Grid["Supervisor", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Technician"].Index)
                {
                    MyBase.Row_Number(ref Grid);
                    if (Grid.CurrentCell.RowIndex > 0 && Grid["Technician", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["Technician_Emplno", Grid.CurrentCell.RowIndex].Value = Grid["Technician_Emplno", Grid.CurrentCell.RowIndex - 1].Value;
                        Grid["Technician", Grid.CurrentCell.RowIndex].Value = Grid["Technician", Grid.CurrentCell.RowIndex - 1].Value;
                        Txt.Text = Grid["Technician", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                    }
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DtpDate1_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(DtpDate1.Value) > MyBase.GetServerDateTime())
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DtpDate1.Value = MyBase.GetServerDateTime();
                    DtpDate1.Focus();
                    return;
                }
                else if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(DtpDate1.Value), MyBase.GetServerDateTime()) > 1)
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DtpDate1.Value = MyBase.GetServerDateTime();
                    DtpDate1.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
                {
                }
                else
                {
                    e.Handled = true;
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        Boolean Check_Designation_Machine(Int32 Row)
        {
            try
            {
                //for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                //{
                //    if (i != Row)
                //    {
                //        if (Grid["Designation_Code", Row].Value != null && Grid["Designation_Code", Row].Value != DBNull.Value && Grid["Machine_Code", Row].Value != null && Grid["Machine_Code", Row].Value != DBNull.Value && Grid["EmplNo", Row].Value != null && Grid["EmplNo", Row].Value != DBNull.Value)
                //        {
                //            if (Grid["Designation_Code", i].Value != null && Grid["Designation_Code", i].Value != DBNull.Value && Grid["Machine_Code", i].Value != null && Grid["Machine_Code", i].Value != DBNull.Value && Grid["EmplNo", i].Value != null && Grid["EmplNo", i].Value != DBNull.Value)
                //            {
                //                if (Grid["Designation_Code", i].Value.ToString() == Grid["Designation_Code", Row].Value.ToString() && Grid["Machine_Code", i].Value.ToString() == Grid["Machine_Code", Row].Value.ToString() && Grid["EmplNo", i].Value.ToString() == Grid["EmplNo", Row].Value.ToString())
                //                {
                //                    return true;
                //                }
                //            }
                //        }
                //    }

                for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                {
                    if (i != Row)
                    {
                        if (Grid["Machine_Code", Row].Value != null && Grid["Machine_Code", Row].Value != DBNull.Value && Grid["EmplNo", Row].Value != null && Grid["EmplNo", Row].Value != DBNull.Value)
                        {
                            if (Grid["Machine_Code", i].Value != null && Grid["Machine_Code", i].Value != DBNull.Value && Grid["EmplNo", i].Value != null && Grid["EmplNo", i].Value != DBNull.Value)
                            {
                                if (Grid["Machine_Code", i].Value.ToString() == Grid["Machine_Code", Row].Value.ToString() && Grid["EmplNo", i].Value.ToString() == Grid["EmplNo", Row].Value.ToString())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {                     
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Supervisor"].Index)
                    {
                        //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supervisor", "Select Name Supervisor, Tno, Emplno Code From Attendance2('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', " + TxtDivision.Tag.ToString() + ") Where Shiftcode2 = '" + TxtShift.Text + "' and designationname = 'SUPERVISOR' AND InTime IS NOT NULL", String.Empty, 200, 80);
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Employee", "Select Name Supervisor, Tno, Emplno Code From VAAHINI_ERP_GAINUP.DBO.Employeemas Where Dateofreleave >= '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' and Compcode = " + TxtDivision.Tag.ToString() + " And Catcode = 6 and designationcode in (78,179,180,184,193,194,195,196,206,208)", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Supervisor"].ToString();
                            Grid["Supervisor", Grid.CurrentCell.RowIndex].Value = Dr["Supervisor"].ToString();
                            Grid["Supervisor_Emplno", Grid.CurrentCell.RowIndex].Value = Dr["Code"].ToString();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Employee"].Index)
                    {
                        Dr = Tool.Selection_Tool_Except_New("Emplno", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Employee", "Select Name Employee, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno From VAAHINI_ERP_GAINUP.DBO.Employee_List_Production('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', " + TxtDivision.Tag.ToString() + ") A Left Join Employee_Production_Details_Socks C On A.Emplno = C.EmplNo LEft Join Employee_Production_Master_Socks D On C.Master_ID = D.RowID and D.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' and D.Shift_Code = " + TxtShift.Tag + " Where catcode in (1,3,5,7) and D.Entry_Date is null And A.deptcode = " + TxtType.Tag.ToString() + " Group By Name, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno, D.Entry_Date , D.Shift_Code ", String.Empty, 200, 80, 120, 120, 100);
                        
                        
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Employee"].ToString();
                            Grid["Employee", Grid.CurrentCell.RowIndex].Value = Dr["Employee"].ToString();
                            Grid["Emplno", Grid.CurrentCell.RowIndex].Value = Dr["Emplno"].ToString();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Technician"].Index)
                    {
                        //Dr = Tool.Selection_Tool_Except_New("Technician_Emplno", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Employee", "Select Name Technician, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno Technician_Emplno From VAAHINI_ERP_GAINUP.DBO.Employee_List_Production('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', " + TxtDivision.Tag.ToString() + ") A Left Join Employee_Production_Details_Socks C On A.Emplno = C.EmplNo LEft Join Employee_Production_Master_Socks D On C.Master_ID = D.RowID and D.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' and D.Shift_Code = " + TxtShift.Tag + " Where catcode in (5, 6, 7) and D.Entry_Date is null Group By Name, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno, D.Entry_Date , D.Shift_Code ", String.Empty, 200, 80, 120, 120, 100);
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Employee", "Select Name Technician, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno Technician_Emplno From VAAHINI_ERP_GAINUP.DBO.Employee_List_Production('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', " + TxtDivision.Tag.ToString() + ") A Left Join Employee_Production_Details_Socks C On A.Emplno = C.EmplNo LEft Join Employee_Production_Master_Socks D On C.Master_ID = D.RowID and D.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' and D.Shift_Code = " + TxtShift.Tag + " Where catcode in (5, 6, 7) and D.Entry_Date is null Group By Name, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno, D.Entry_Date , D.Shift_Code ", String.Empty, 200, 80, 120, 120, 100);
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Technician"].ToString();
                            Grid["Technician", Grid.CurrentCell.RowIndex].Value = Dr["Technician"].ToString();
                            Grid["Technician_Emplno", Grid.CurrentCell.RowIndex].Value = Dr["Technician_Emplno"].ToString();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                    {
                        if (Txt.Text.ToString() == String.Empty)
                        {
                            //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Machine - Group", "Select Distinct Group_Name Machine, Remarks,  Master_ID Machine_Code, SNo From Get_Machine_List (" + TxtDivision.Tag.ToString() + "," + Grid["Department_Code", Grid.CurrentCell.RowIndex].Value.ToString() + ", " + TxtShift.Tag.ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "') Order by SNo", String.Empty, 200, 250);
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Machine - Group", "Select Distinct Group_Name Machine, Remarks,  Master_ID Machine_Code From Get_Machine_List (" + TxtDivision.Tag.ToString() + ", " + TxtType.Tag.ToString() + ", " + TxtShift.Tag.ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "') ", String.Empty, 200, 250);
                        }
                        else
                        {
                            // and A.Order_No Not In (" + TxtOrderNo.Text.Replace("`", "'") + ")
                            Dr = Tool.Selection_Tool_Except_New("Machine", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Machine - Group", "Select Distinct Group_Name Machine, Remarks,  Master_ID Machine_Code From Get_Machine_List (" + TxtDivision.Tag.ToString() + ", " + TxtType.Tag.ToString() + ", " + TxtShift.Tag.ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "') ", String.Empty, 200, 250);
                        }

                        
                        if (Dr != null)
                        {
                            if (Check_Designation_Machine(Grid.CurrentCell.RowIndex))
                            {
                                MessageBox.Show("Already Designation & Machine added ...!", "Gainup");
                                Txt.Text = String.Empty;
                                Grid["Machine", Grid.CurrentCell.RowIndex].Value = DBNull.Value;
                                Grid["Machine_Code", Grid.CurrentCell.RowIndex].Value = DBNull.Value;
                                return;
                            }
                            else
                            {
                                Txt.Text = Dr["Machine"].ToString();
                                Grid["Machine", Grid.CurrentCell.RowIndex].Value = Dr["Machine"].ToString();
                                Grid["Machine_Code", Grid.CurrentCell.RowIndex].Value = Dr["Machine_Code"].ToString();
                                Grid["Multiple", Grid.CurrentCell.RowIndex].Value = 1;
                            }
                            //if (Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            //{
                            //    M = 0;
                            //    Txt.Text = '`' + Dr["Machine"].ToString() + '`';
                            //    Grid["Machine", Grid.CurrentCell.RowIndex].Value = Dr["Machine"].ToString();
                            //    Grid["Machine_Code", Grid.CurrentCell.RowIndex].Value = Dr["Machine_Code"].ToString();
                            //    Grid["Multiple", Grid.CurrentCell.RowIndex].Value = 1;
                            //}
                            //else
                            //{
                            //    M = M + 1;
                            //    Txt.Text = Txt.Text + ",`" + Dr["Machine"].ToString() + '`';
                            //    Grid["Machine", Grid.CurrentCell.RowIndex].Value = Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() + "," + Dr["Machine"].ToString();
                            //    Grid["Machine_Code", Grid.CurrentCell.RowIndex].Value = Grid["Machine_Code", Grid.CurrentCell.RowIndex].Value.ToString() + "," + Dr["Machine_Code"].ToString();
                            //    Grid["Multiple", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["Multiple", Grid.CurrentCell.RowIndex].Value.ToString()) + 1;
                            //} 
                          
                       }
                    }
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                    {
                        e.Handled = true;
                        Grid["Machine_Code", Grid.CurrentCell.RowIndex].Value = DBNull.Value;
                        Grid["Machine", Grid.CurrentCell.RowIndex].Value = DBNull.Value;
                        Txt.Text = String.Empty;
                    }
                }
                else
                {
                    e.Handled = true;
                }
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentCell.RowIndex);
                Dt.AcceptChanges();
                Grid.RefreshEdit();
                MyBase.Row_Number(ref Grid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Total_Count()
        {
            try
            {
                TxtEmployees.Text = MyBase.Count(ref Grid, "Employee", "Machine");
                //TxtTotal.Text = MyBase.Sum(ref Grid, "Emplno", "Machine");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
                    {
                        if (Grid["Remarks", Grid.CurrentCell.RowIndex].Value.ToString().Trim() == String.Empty)
                        {
                            Grid["Remarks", Grid.CurrentCell.RowIndex].Value = "-";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    TxtEmployees.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtEmployees_Enter(object sender, EventArgs e)
        {
            try
            {
                TxtEmployees.Text = Dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Arrow1_Click(object sender, EventArgs e)
        {

        } 
    }
}
