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
    public partial class Frm_Board_Empl_Allocation : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        Int32 M = 0;
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataTable Dt2 = new DataTable();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        String S;
        String[] t;
        String[] Queries;
        TextBox Txt = null;
        TextBox Txt1 = null;
        Int64 Master_ID = 0;
        Int64 Detail_ID = 0;

        public Frm_Board_Empl_Allocation()
        {
            InitializeComponent();
        }

        private void Frm_Board_Empl_Allocation_Load(object sender, EventArgs e)
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
                if (MyParent.UserCode == 20 || MyParent.UserCode == 30)
                {
                    TxtDept.Text = "BOARDING";
                    TxtDept.Tag = 44;
                    if (MyParent.UserCode == 20)
                    {
                        TxtUnit.Text = "FLOOR - I";
                        TxtUnit.Tag = 1;
                    }
                    else if (MyParent.UserCode == 20)
                    {
                        TxtUnit.Text = "FLOOR - II";
                        TxtUnit.Tag = 2;
                    }
                }
                else if (MyParent.UserCode == 29 || MyParent.UserCode == 41)
                {
                    TxtDept.Text = "LINKING";
                    TxtDept.Tag = 43;
                    if (MyParent.UserCode == 29)
                    {
                        TxtUnit.Text = "FLOOR - I";
                        TxtUnit.Tag = 1;
                    }
                    else if (MyParent.UserCode == 41)
                    {
                        TxtUnit.Text = "FLOOR - II";
                        TxtUnit.Tag = 2;
                    }
                }
                else if (MyParent.UserCode == 18)
                {
                    TxtDept.Text = "WASHING";
                    TxtDept.Tag = 49;
                    TxtUnit.Text = "FLOOR - I";
                    TxtUnit.Tag = 1;
                    label7.Text = "OPERATOR";
                }
                else if (MyParent.UserCode == 43)
                {
                    TxtDept.Text = "STITCHING";
                    TxtDept.Tag = 73;
                    TxtUnit.Text = "FLOOR - I";
                    TxtUnit.Tag = 1;
                }
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
                if (TxtDept.Text.Trim() == String.Empty || TxtShift.Text.Trim() == String.Empty || TxtUnit.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtSupervisor.Text.Trim() == String.Empty)
                {
                    TxtSupervisor.Tag = 0;
                }

                if(TxtMesthri.Text.Trim() == String.Empty)
                {
                    TxtMesthri.Tag = 0;
                }

                if (TxtEmployees.Text.Trim() == String.Empty)
                {
                    TxtEmployees.Text = "0";
                }

                if (Convert.ToDouble(TxtEmployees.Text) == 0)
                {
                    MessageBox.Show("Invalid Grid Details ...!", "Gainup");
                    MyParent.Save_Error = true;
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Grid Details ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtShift.Focus();
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 2; i++)
                {
                    if (Grid["Machine", i].Value == null || Grid["Machine", i].Value == DBNull.Value || Grid["Machine", i].Value.ToString() == String.Empty || Grid["Operator", i].Value == null || Grid["Operator", i].Value == DBNull.Value || Grid["Operator", i].Value.ToString() == String.Empty || Grid["Operator_Emplno", i].Value == null || Grid["Operator_Emplno", i].Value == DBNull.Value || Grid["OPerator_Emplno", i].Value.ToString() == String.Empty)
                    {
                        MessageBox.Show("Invalid Data ...!", "Gainup");
                        MyParent.Save_Error = true;
                        Grid.CurrentCell = Grid["Machine", i];
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
                    if (TxtDept.Text.ToString() == "BOARDING")
                    {
                        MyBase.Load_Data("Select (Isnull(Max(EntryNo), 0) + 1) No From Socks_Boarding_Employee_Allocation_Master ", ref TDt);
                    }
                    else if (TxtDept.Text.ToString() == "LINKING")
                    {
                        MyBase.Load_Data("Select (Isnull(Max(EntryNo), 0) + 1) No From Socks_Linking_Employee_Allocation_Master ", ref TDt);
                    }
                    else if (TxtDept.Text.ToString() == "WASHING")
                    {
                        MyBase.Load_Data("Select (Isnull(Max(EntryNo), 0) + 1) No From Socks_Washing_Employee_Allocation_Master ", ref TDt);
                    }
                    else if (TxtDept.Text.ToString() == "STITCHING")
                    {
                        MyBase.Load_Data("Select (Isnull(Max(EntryNo), 0) + 1) No From Socks_Stitching_Employee_Allocation_Master ", ref TDt);
                    }
                    TxtEno.Text = TDt.Rows[0][0].ToString();
                    
                    Queries = new String[Dt.Rows.Count * 3];
                    
                    if (TxtDept.Text.ToString() == "BOARDING")
                    {
                        Queries[Array_index++] = "Insert into Socks_Boarding_Employee_Allocation_Master (EntryNo, Effect_From, ShiftCode, Unit_Code, DeptCode, EntryTime, EntrySystem, User_Code, Total_Employees, Remarks, Supervisor_Emplno, Mesthri_Emplno) Values (" + TxtEno.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', " + TxtShift.Tag.ToString() + ", " + TxtUnit.Tag.ToString() + ", " + TxtDept.Tag.ToString() + ", Getdate(), Host_Name(), " + MyParent.UserCode + ", " + TxtEmployees.Text + ", '" + TxtRemarks.Text + "', " + TxtSupervisor.Tag.ToString() + ", " + TxtMesthri.Tag.ToString() + "); Select Scope_Identity ()";
                    }
                    else if (TxtDept.Text.ToString() == "LINKING")
                    {
                        Queries[Array_index++] = "Insert into Socks_Linking_Employee_Allocation_Master (EntryNo, Effect_From, ShiftCode, Unit_Code, DeptCode, EntryTime, EntrySystem, User_Code, Total_Employees, Remarks, Supervisor_Emplno, Mesthri_Emplno) Values (" + TxtEno.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', " + TxtShift.Tag.ToString() + ", " + TxtUnit.Tag.ToString() + ", " + TxtDept.Tag.ToString() + ", Getdate(), Host_Name(), " + MyParent.UserCode + ", " + TxtEmployees.Text + ", '" + TxtRemarks.Text + "', " + TxtSupervisor.Tag.ToString() + ", " + TxtMesthri.Tag.ToString() + "); Select Scope_Identity ()";
                    }
                    else if (TxtDept.Text.ToString() == "WASHING")
                    {
                        Queries[Array_index++] = "Insert into Socks_Washing_Employee_Allocation_Master (EntryNo, Effect_From, ShiftCode, Unit_Code, DeptCode, EntryTime, EntrySystem, User_Code, Total_Employees, Remarks, Supervisor_Emplno, Operator_Emplno) Values (" + TxtEno.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', " + TxtShift.Tag.ToString() + ", " + TxtUnit.Tag.ToString() + ", " + TxtDept.Tag.ToString() + ", Getdate(), Host_Name(), " + MyParent.UserCode + ", " + TxtEmployees.Text + ", '" + TxtRemarks.Text + "', " + TxtSupervisor.Tag.ToString() + ", " + TxtMesthri.Tag.ToString() + "); Select Scope_Identity ()";
                    }
                    else if (TxtDept.Text.ToString() == "STITCHING")
                    {
                        Queries[Array_index++] = "Insert into Socks_Stitching_Employee_Allocation_Master (EntryNo, Effect_From, ShiftCode, Unit_Code, DeptCode, EntryTime, EntrySystem, User_Code, Total_Employees, Remarks, Supervisor_Emplno, Mesthri_Emplno) Values (" + TxtEno.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', " + TxtShift.Tag.ToString() + ", " + TxtUnit.Tag.ToString() + ", " + TxtDept.Tag.ToString() + ", Getdate(), Host_Name(), " + MyParent.UserCode + ", " + TxtEmployees.Text + ", '" + TxtRemarks.Text + "', " + TxtSupervisor.Tag.ToString() + ", " + TxtMesthri.Tag.ToString() + "); Select Scope_Identity ()";
                    }
                }
                else
                {
                    Queries = new String[Dt.Rows.Count * 3];
                    if (TxtDept.Text.ToString() == "BOARDING")
                    {
                        Queries[Array_index++] = "Update Socks_Boarding_Employee_Allocation_Master Set Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', Supervisor_Emplno = " + TxtSupervisor.Tag.ToString() + ", Mesthri_Emplno = " + TxtMesthri.Tag.ToString() + ", Total_Employees = " + TxtEmployees.Text + ", Remarks = '" + TxtRemarks.Text + "'  Where RowID = " + Master_ID;
                        Queries[Array_index++] = "Delete From Socks_Boarding_Employee_Allocation_Details Where MasterID = " + Master_ID;
                    }
                    else if (TxtDept.Text.ToString() == "LINKING")
                    {
                        Queries[Array_index++] = "Update Socks_Linking_Employee_Allocation_Master Set Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', Supervisor_Emplno = " + TxtSupervisor.Tag.ToString() + ", Mesthri_Emplno = " + TxtMesthri.Tag.ToString() + ", Total_Employees = " + TxtEmployees.Text + ", Remarks = '" + TxtRemarks.Text + "'  Where RowID = " + Master_ID;
                        Queries[Array_index++] = "Delete From Socks_Linking_Employee_Allocation_Details Where MasterID = " + Master_ID;
                    }
                    else if (TxtDept.Text.ToString() == "WASHING")
                    {
                        Queries[Array_index++] = "Update Socks_Washing_Employee_Allocation_Master Set Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', Supervisor_Emplno = " + TxtSupervisor.Tag.ToString() + ", Operator_Emplno = " + TxtMesthri.Tag.ToString() + ", Total_Employees = " + TxtEmployees.Text + ", Remarks = '" + TxtRemarks.Text + "'  Where RowID = " + Master_ID;
                        Queries[Array_index++] = "Delete From Socks_Washing_Employee_Allocation_Details Where MasterID = " + Master_ID;
                    }
                    else if (TxtDept.Text.ToString() == "STITCHING")
                    {
                        Queries[Array_index++] = "Update Socks_Stitching_Employee_Allocation_Master Set Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', Supervisor_Emplno = " + TxtSupervisor.Tag.ToString() + ", Mesthri_Emplno = " + TxtMesthri.Tag.ToString() + ", Total_Employees = " + TxtEmployees.Text + ", Remarks = '" + TxtRemarks.Text + "'  Where RowID = " + Master_ID;
                        Queries[Array_index++] = "Delete From Socks_Stitching_Employee_Allocation_Details Where MasterID = " + Master_ID;
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        if (TxtDept.Text.ToString() == "BOARDING")
                        {
                            Queries[Array_index++] = "Insert into Socks_Boarding_Employee_Allocation_Details (MasterID, MachineID, Operator_Emplno, Work_Nature, Remarks) Values(@@IDENTITY, '" + Grid["MachineID", i].Value.ToString() + "', " + Grid["Operator_Emplno", i].Value.ToString() + ", " + Grid["Work_Nature", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "')";
                        }
                        else if (TxtDept.Text.ToString() == "LINKING")
                        {
                            Queries[Array_index++] = "Insert into Socks_Linking_Employee_Allocation_Details (MasterID, MachineID, Operator_Emplno, Work_Nature, Remarks) Values(@@IDENTITY, '" + Grid["MachineID", i].Value.ToString() + "', " + Grid["Operator_Emplno", i].Value.ToString() + ", " + Grid["Work_Nature", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "')";
                        }
                        else if (TxtDept.Text.ToString() == "WASHING")
                        {
                            Queries[Array_index++] = "Insert into Socks_Washing_Employee_Allocation_Details (MasterID, MachineID, Operator_Emplno, Work_Nature, Remarks) Values(@@IDENTITY, '" + Grid["MachineID", i].Value.ToString() + "', " + Grid["Operator_Emplno", i].Value.ToString() + ", " + Grid["Work_Nature", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "')";
                        }
                        else if (TxtDept.Text.ToString() == "STITCHING")
                        {
                            Queries[Array_index++] = "Insert into Socks_Stitching_Employee_Allocation_Details (MasterID, MachineID, Operator_Emplno, Work_Nature, Remarks) Values(@@IDENTITY, '" + Grid["MachineID", i].Value.ToString() + "', " + Grid["Operator_Emplno", i].Value.ToString() + ", " + Grid["Work_Nature", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "')";
                        }
                    }
                    else
                    {
                        if (TxtDept.Text.ToString() == "BOARDING")
                        {
                            Queries[Array_index++] = "Insert into Socks_Boarding_Employee_Allocation_Details (MasterID, MachineID, Operator_Emplno, Work_Nature, Remarks) Values(" + Master_ID + ", '" + Grid["MachineID", i].Value.ToString() + "', " + Grid["Operator_Emplno", i].Value.ToString() + ", " + Grid["Work_Nature", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "')";
                        }
                        else if (TxtDept.Text.ToString() == "LINKING")
                        {
                            Queries[Array_index++] = "Insert into Socks_Linking_Employee_Allocation_Details (MasterID, MachineID, Operator_Emplno, Work_Nature, Remarks) Values(" + Master_ID + ", '" + Grid["MachineID", i].Value.ToString() + "', " + Grid["Operator_Emplno", i].Value.ToString() + ", " + Grid["Work_Nature", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "')";
                        }
                        else if (TxtDept.Text.ToString() == "WASHING")
                        {
                            Queries[Array_index++] = "Insert into Socks_Washing_Employee_Allocation_Details (MasterID, MachineID, Operator_Emplno, Work_Nature, Remarks) Values(" + Master_ID + ", '" + Grid["MachineID", i].Value.ToString() + "', " + Grid["Operator_Emplno", i].Value.ToString() + ", " + Grid["Work_Nature", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "')";
                        }
                        else if (TxtDept.Text.ToString() == "STITCHING")
                        {
                            Queries[Array_index++] = "Insert into Socks_Stitching_Employee_Allocation_Details (MasterID, MachineID, Operator_Emplno, Work_Nature, Remarks) Values(" + Master_ID + ", '" + Grid["MachineID", i].Value.ToString() + "', " + Grid["Operator_Emplno", i].Value.ToString() + ", " + Grid["Work_Nature", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "')";
                        }
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
                    TxtShift.Enabled = true;
                    TxtUnit.Enabled = true;
                    TxtDept.Enabled = true;
                }
                else
                {
                    DtpDate1.Enabled = false;
                    TxtShift.Enabled = false;
                    TxtUnit.Enabled = false;
                    TxtDept.Enabled = false;
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
                //Enable_Control();
                Master_ID = Convert.ToInt64(Dr["RowID"]);
                TxtEno.Text = Dr["EntryNo"].ToString();
                DtpDate1.Value = Convert.ToDateTime(Dr["Effect_From"]);
                TxtShift.Text = Dr["ShiftCode2"].ToString();
                TxtShift.Tag = Dr["ShiftCode"].ToString();
                TxtDept.Text = Dr["Department"].ToString();
                TxtDept.Tag = Dr["DeptCode"].ToString();
                TxtUnit.Text = Dr["Unit"].ToString();
                TxtUnit.Tag = Dr["Unit_Code"].ToString();
                TxtSupervisor.Text = Dr["Supervisor"].ToString();
                TxtSupervisor.Tag = Dr["Supervisor_Emplno"].ToString();
                TxtMesthri.Text = Dr["Mesthri"].ToString();
                TxtMesthri.Tag = Dr["Mesthri_Emplno"].ToString();
                Grid_Data();
                Total_Count();
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
                String Str;
                if (MyParent.UserCode == 20 || MyParent.UserCode == 30)
                {
                    //Boarding
                    Str = " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Boarding_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Boarding_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype Where Unit_Code = " + TxtUnit.Tag.ToString() + " D1 On S1.DeptCode = D1.DeptCode Order By S1.EntryNo Desc";
                }
                else if (MyParent.UserCode == 29 || MyParent.UserCode == 41)
                {
                    //Linking
                    Str = " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Linking_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Linking_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype Where Unit_Code = " + TxtUnit.Tag.ToString() + " D1 On S1.DeptCode = D1.DeptCode";
                    if (MyParent.UserCode == 29)
                    {
                        //Stitching
                        Str = Str + " Union All ";
                        Str = Str + " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                        Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                        Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Stitching_Employee_Allocation_Master S1 ";
                        Str = Str + " Left Join Socks_Stitching_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                        Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                        Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                        Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                        Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                    }
                }
                else if (MyParent.UserCode == 18)
                {
                    //Washing
                    Str = " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Operator_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Washing_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Washing_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Operator_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode Order By S1.EntryNo Desc";
                }
                else
                {
                    //Boarding
                    Str = " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Boarding_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Boarding_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                    //Linking
                    Str = Str + " Union All ";
                    Str = Str + " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Linking_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Linking_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                    //Washing
                    Str = Str + " Union All ";
                    Str = Str + " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Operator_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Washing_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Washing_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Operator_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                    //Stitching
                    Str = Str + " Union All ";
                    Str = Str + " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Stitching_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Stitching_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                }
                
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Production - Edit", Str, String.Empty, 80, 80, 50, 70, 100);

                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Machine", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    if (TxtDept.Text == "WASHING")
                    {
                        label7.Text = "OPERATOR";
                    }
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
                String Str;
                if (MyParent.UserCode == 20 || MyParent.UserCode == 30)
                {
                    //Boarding
                    Str = " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Boarding_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Boarding_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode Order By S1.EntryNo Desc";
                }
                else if (MyParent.UserCode == 29 || MyParent.UserCode == 41)
                {
                    //Linking
                    Str = " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Linking_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Linking_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                    if (MyParent.UserCode == 29)
                    {
                        //Stitching
                        Str = Str + " Union All ";
                        Str = Str + " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                        Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                        Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Stitching_Employee_Allocation_Master S1 ";
                        Str = Str + " Left Join Socks_Stitching_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                        Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                        Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                        Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                        Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                    }
                }
                else if (MyParent.UserCode == 18)
                {
                    //Washing
                    Str = " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Operator_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Washing_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Washing_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Operator_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode Order By S1.EntryNo Desc";
                }
                else
                {
                    //Boarding
                    Str = " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Boarding_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Boarding_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                    //Linking
                    Str = Str + " Union All ";
                    Str = Str + " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Linking_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Linking_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                    //Washing
                    Str = Str + " Union All ";
                    Str = Str + " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Operator_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Washing_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Washing_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Operator_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                    //Stitching
                    Str = Str + " Union All ";
                    Str = Str + " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Stitching_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Stitching_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                }
                
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Production - Delete", Str, String.Empty, 80, 80, 50, 70, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    MyParent.Load_DeleteConfirmEntry();
                    if (TxtDept.Text == "WASHING")
                    {
                        label7.Text = "OPERATOR";
                    }
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
                    if (TxtDept.Text.ToString() == "BOARDING")
                    {
                        MyBase.Run("Delete from Socks_Boarding_Employee_Allocation_Details where MasterID = " + Master_ID, "Delete from Socks_Boarding_Employee_Allocation_Master where RowID = " + Master_ID);
                    }
                    else if (TxtDept.Text.ToString() == "LINKING")
                    {
                        MyBase.Run("Delete from Socks_Linking_Employee_Allocation_Details where MasterID = " + Master_ID, "Delete from Socks_Linking_Employee_Allocation_Master where RowID = " + Master_ID);
                    }
                    else if (TxtDept.Text.ToString() == "WASHING")
                    {
                        MyBase.Run("Delete from Socks_Washing_Employee_Allocation_Details where MasterID = " + Master_ID, "Delete from Socks_Washing_Employee_Allocation_Master where RowID = " + Master_ID);
                    }
                    else if (TxtDept.Text.ToString() == "STITCHING")
                    {
                        MyBase.Run("Delete from Socks_Stitching_Employee_Allocation_Details where MasterID = " + Master_ID, "Delete from Socks_Stitching_Employee_Allocation_Master where RowID = " + Master_ID);
                    }
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

                String Str;
                if (MyParent.UserCode == 20 || MyParent.UserCode == 30)
                {
                    //Boarding
                    Str = " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Boarding_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Boarding_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode Order By S1.EntryNo Desc";
                }
                else if (MyParent.UserCode == 29 || MyParent.UserCode == 41)
                {
                    //Linking
                    Str = " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Linking_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Linking_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                    if (MyParent.UserCode == 29)
                    {
                        //Stitching
                        Str = Str + " Union All ";
                        Str = Str + " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                        Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                        Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Stitching_Employee_Allocation_Master S1 ";
                        Str = Str + " Left Join Socks_Stitching_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                        Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                        Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                        Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                        Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                    }
                }
                else if (MyParent.UserCode == 18)
                {
                    //Washing
                    Str = " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Operator_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Washing_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Washing_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Operator_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode Order By S1.EntryNo Desc";
                }
                else
                {
                    //Boarding
                    Str = " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Boarding_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Boarding_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                    //Linking
                    Str = Str + " Union All ";
                    Str = Str + " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Linking_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Linking_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                    //Washing
                    Str = Str + " Union All ";
                    Str = Str + " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Operator_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Washing_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Washing_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Operator_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                    //Stitching
                    Str = Str + " Union All ";
                    Str = Str + " Select Distinct S1.EntryNO, S1.Effect_From, S3.Shiftcode2, U1.Unit_Name Unit, D1.DeptName Department, S3.Shiftcode, S1.Unit_Code, ";
                    Str = Str + " Isnull(E1.Name, '')Supervisor, ISnull(S1.Supervisor_Emplno, 0)Supervisor_Emplno, Isnull(E2.Name, '')Mesthri, ISnull(S1.Mesthri_Emplno, 0)Mesthri_Emplno, ";
                    Str = Str + " S1.DeptCode, S1.Total_Employees, S1.RowID from Socks_Stitching_Employee_Allocation_Master S1 ";
                    Str = Str + " Left Join Socks_Stitching_Employee_Allocation_Details S2 On S1.Rowid = S2.MasterID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E1 On Isnull(S1.Supervisor_Emplno,0) = E1.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas E2 On Isnull(S1.Mesthri_Emplno,0) = E2.Emplno ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ShiftCode = S3.Shiftcode And S3.compcode = 2 And S3.Mode = 1 ";
                    Str = Str + " Left Join Unit_Master U1 On S1.Unit_Code = U1.RowID Left Join Vaahini_Erp_Gainup.Dbo.Depttype D1 On S1.DeptCode = D1.DeptCode";
                }
                
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Production - View", Str, String.Empty, 80, 80, 50, 70, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    if (TxtDept.Text == "WASHING")
                    {
                        label7.Text = "OPERATOR";
                    }
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

        private void Frm_Board_Empl_Allocation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtUnit")
                    {
                        if (TxtUnit.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Invalid Unit...!", "Gainup");
                            TxtUnit.Focus();
                            return;
                        }
                        else
                        {
                            TxtShift.Focus();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtShift")
                    {
                        if (TxtShift.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Invalid Shift...!", "Gainup");
                            TxtShift.Focus();
                            return;
                        }
                        else
                        {
                            TxtDept.Focus();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtDept")
                    {
                        if (TxtDept.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Invalid Department...!", "Gainup");
                            TxtDept.Focus();
                            return;
                        }
                        else
                        {
                            TxtSupervisor.Focus();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtSupervisor")
                    {
                            TxtMesthri.Focus();
                            return;
                    }
                    else if (this.ActiveControl.Name == "TxtMesthri")
                    {
                        Grid_Data();
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
                    if (this.ActiveControl.Name == "TxtShift")
                    {
                        if (TxtUnit.Text.Trim() == String.Empty)
                        {
                            MessageBox.Show("Invalid Division or Type...!", "Gainup");
                            TxtUnit.Focus();
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
                        Shift_Selection();
                    }
                    else if (this.ActiveControl.Name == "TxtDept")
                    {
                        if (TxtUnit.Text.Trim() == String.Empty || TxtShift.Text.Trim() == String.Empty)
                        {
                            MessageBox.Show("Invalid Division ...!", "Gainup");
                            TxtShift.Focus();
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
                        Dept_Selection();
                    }
                    else if (this.ActiveControl.Name == "TxtUnit")
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

                        Unit_Selection();
                    }
                    else if (this.ActiveControl.Name == "TxtSupervisor")
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

                        Supervisor_Selection();
                    }
                    else if (this.ActiveControl.Name == "TxtMesthri")
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

                        Mesthri_Selection();
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
                    if (TxtDept.Text.ToString() == "BOARDING")
                    {
                        Str = " Select 0 As Slno, B.Machine Machine, A.MachineID, C.Name Operator, A.Operator_Emplno, D.Work_Nature Work, A.Work_Nature, A.Remarks, '-' T From Socks_Boarding_Employee_Allocation_Details A ";
                        Str = Str + " Left Join Setting_Mc_NO_Unit() B On A.MachineID = B.Machine And B.Unit_Code = " + TxtUnit.Tag.ToString() + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas C On A.Operator_Emplno = C.Emplno ";
                        Str = Str + " Left Join Socks_Work_Nature D On A.Work_Nature = D.RowID Where 1 = 2 ";
                    }
                    else if (TxtDept.Text.ToString() == "LINKING")
                    {
                        Str = " Select 0 As Slno, B.Machine Machine, A.MachineID, C.Name Operator, A.Operator_Emplno, D.Work_Nature Work, A.Work_Nature, A.Remarks, '-' T From Socks_Linking_Employee_Allocation_Details A ";
                        Str = Str + " Left Join Linking_Mc_NO_Unit() B On A.MachineID = B.Machine Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas C On A.Operator_Emplno = C.Emplno ";
                        Str = Str + " Left Join Socks_Work_Nature D On A.Work_Nature = D.RowID Where 1 = 2 ";
                    }
                    else if (TxtDept.Text.ToString() == "WASHING")
                    {
                        Str = " Select 0 As Slno, B.Machine Machine, A.MachineID, C.Name Operator, A.Operator_Emplno, D.Work_Nature Work, A.Work_Nature, A.Remarks, '-' T From Socks_Washing_Employee_Allocation_Details A ";
                        Str = Str + " Left Join Washing_Mc_NO_Unit() B On A.MachineID = B.Machine Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas C On A.Operator_Emplno = C.Emplno ";
                        Str = Str + " Left Join Socks_Work_Nature D On A.Work_Nature = D.RowID Where 1 = 2 ";
                    }
                    else if (TxtDept.Text.ToString() == "STITCHING")
                    {
                        Str = " Select 0 As Slno, B.Machine_Name Machine, A.MachineID, C.Name Operator, A.Operator_Emplno, D.Work_Nature Work, A.Work_Nature, A.Remarks, '-' T From Socks_Stitching_Employee_Allocation_Details A ";
                        Str = Str + " Left Join Socks_Stiching_Machine_List B On A.MachineID = B.RowID Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas C On A.Operator_Emplno = C.Emplno ";
                        Str = Str + " Left Join Socks_Work_Nature D On A.Work_Nature = D.RowID Where 1 = 2 ";
                    }
                    else
                    {
                        Str = " Select 0 As Slno, B.Machine Machine, A.MachineID, C.Name Operator, A.Operator_Emplno, D.Work_Nature Work, A.Work_Nature, A.Remarks, '-' T From Socks_Boarding_Employee_Allocation_Details A ";
                        Str = Str + " Left Join Setting_Mc_NO_Unit() B On A.MachineID = B.Machine Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas C On A.Operator_Emplno = C.Emplno ";
                        Str = Str + " Left Join Socks_Work_Nature D On A.Work_Nature = D.RowID Where 1 = 2 ";
                    }
                }
                else
                {
                    if (TxtDept.Text.ToString() == "BOARDING")
                    {
                        Str = " Select 0 As Slno, B.Machine Machine, A.MachineID, C.Name Operator, A.Operator_Emplno, D.Work_Nature Work, A.Work_Nature, A.Remarks, '-' T From Socks_Boarding_Employee_Allocation_Details A ";
                        Str = Str + " Left Join Setting_Mc_NO_Unit()B On A.MachineID = B.Machine And B.Unit_Code = " + TxtUnit.Tag.ToString() + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas C On A.Operator_Emplno = C.Emplno ";
                        Str = Str + " Left Join Socks_Work_Nature D On A.Work_Nature = D.RowID Where A.MasterID = " + Master_ID;
                    }
                    else if (TxtDept.Text.ToString() == "LINKING")
                    {
                        Str = " Select 0 As Slno, B.Machine Machine, A.MachineID, C.Name Operator, A.Operator_Emplno, D.Work_Nature Work, A.Work_Nature, A.Remarks, '-' T From Socks_Linking_Employee_Allocation_Details A ";
                        Str = Str + " Left Join Linking_Mc_NO_Unit() B On A.MachineID = B.Machine And B.Unit_Code = " + TxtUnit.Tag.ToString() + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas C On A.Operator_Emplno = C.Emplno ";
                        Str = Str + " Left Join Socks_Work_Nature D On A.Work_Nature = D.RowID Where A.MasterID = " + Master_ID;
                    }
                    else if (TxtDept.Text.ToString() == "WASHING")
                    {
                        Str = " Select 0 As Slno, B.Machine Machine, A.MachineID, C.Name Operator, A.Operator_Emplno, D.Work_Nature Work, A.Work_Nature, A.Remarks, '-' T From Socks_Washing_Employee_Allocation_Details A ";
                        Str = Str + " Left Join Washing_Mc_NO_Unit() B On A.MachineID = B.Machine And B.Unit_Code = " + TxtUnit.Tag.ToString() + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas C On A.Operator_Emplno = C.Emplno ";
                        Str = Str + " Left Join Socks_Work_Nature D On A.Work_Nature = D.RowID Where A.MasterID = " + Master_ID;
                    }
                    else if (TxtDept.Text.ToString() == "STITCHING")
                    {
                        Str = " Select 0 As Slno, B.Machine_Name Machine, A.MachineID, C.Name Operator, A.Operator_Emplno, D.Work_Nature Work, A.Work_Nature, A.Remarks, '-' T From Socks_Stitching_Employee_Allocation_Details A ";
                        Str = Str + " Left Join Socks_Stiching_Machine_List B On A.MachineID = B.RowID And B.Unit_Code = " + TxtUnit.Tag.ToString() + " Left Join VAAHINI_ERP_GAINUP.Dbo.Employeemas C On A.Operator_Emplno = C.Emplno ";
                        Str = Str + " Left Join Socks_Work_Nature D On A.Work_Nature = D.RowID Where A.MasterID = " + Master_ID;
                    }
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "MachineID", "Operator_Emplno", "Work_Nature", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Machine", "Operator", "Work", "Remarks");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 50, 150, 150, 150, 150);
                
                Grid.RowHeadersWidth = 10;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Frm_Board_Empl_Allocation_KeyPress(object sender, KeyPressEventArgs e)
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
                for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                {
                    if (i != Row)
                    {
                        if (Grid["MachineID", Row].Value != null && Grid["MachineID", Row].Value != DBNull.Value && Grid["Operator_EmplNo", Row].Value != null && Grid["Operator_EmplNo", Row].Value != DBNull.Value)
                        {
                            if (Grid["MachineID", i].Value != null && Grid["MachineID", i].Value != DBNull.Value && Grid["Operator_EmplNo", i].Value != null && Grid["Operator_EmplNo", i].Value != DBNull.Value)
                            {
                                if (Grid["MachineID", i].Value.ToString() == Grid["MachineID", Row].Value.ToString() && Grid["Operator_EmplNo", i].Value.ToString() == Grid["Operator_EmplNo", Row].Value.ToString())
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["OPERATOR"].Index)
                    {
                        if (TxtDept.Text.ToString() != String.Empty)
                        {
                            if (TxtDept.Text.ToString() == "BOARDING" || TxtDept.Text.ToString() == "LINKING" )
                            {
                                Dr = Tool.Selection_Tool_Except_New_WOMDI("Operator_Emplno", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Employee", "Select Name Operator, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno Operator_Emplno From VAAHINI_ERP_GAINUP.DBO.Employee_List_Production('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', 2) A Where catcode in (1, 3, 5, 7) And A.deptcode = " + TxtDept.Tag.ToString() + " And A.Unit_Code = " + TxtUnit.Tag + " Group By Name, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno", String.Empty, 200, 80, 120, 120, 100);
                            }
                            else if (TxtDept.Text.ToString() == "WASHING")
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Employee", "Select Name Operator, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno Operator_Emplno From VAAHINI_ERP_GAINUP.DBO.Employee_List_Production('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', 2) A Where catcode in (1, 3, 5, 7) And A.deptcode = " + TxtDept.Tag.ToString() + " And A.Unit_Code = " + TxtUnit.Tag + " Group By Name, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno", String.Empty, 200, 80, 120, 120, 100);
                            }
                            else if (TxtDept.Text.ToString() == "STITCHING")
                            {
                                Dr = Tool.Selection_Tool_Except_New_WOMDI("Operator", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Employee", "Select Name Operator, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno Operator_Emplno From VAAHINI_ERP_GAINUP.DBO.Employee_List_Production('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', 2) A Where (Catcode in (1, 3, 5, 7) And A.deptcode = " + TxtDept.Tag.ToString() + " And A.Unit_Code = " + TxtUnit.Tag + ") Or (Catcode in (2)) Group By Name, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno", String.Empty, 200, 80, 120, 120, 100);
                            }

                            if (Dr != null)
                            {
                                Txt.Text = Dr["Operator"].ToString();
                                Grid["Operator", Grid.CurrentCell.RowIndex].Value = Dr["Operator"].ToString();
                                Grid["Operator_Emplno", Grid.CurrentCell.RowIndex].Value = Dr["Operator_Emplno"].ToString();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Choose Department...!Gainup");
                            TxtDept.Focus();
                            return;
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Work"].Index)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select WORK", "Select Work_Nature Work, RowID Work_Nature from Socks_Work_Nature Where DeptCode = " + TxtDept.Tag.ToString() + "", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Work"].ToString();
                            Grid["Work", Grid.CurrentCell.RowIndex].Value = Dr["Work"].ToString();
                            Grid["Work_Nature", Grid.CurrentCell.RowIndex].Value = Dr["Work_Nature"].ToString();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                    {
                        if (TxtDept.Text.ToString() == "BOARDING")
                        {
                            if (Txt.Text.ToString() == String.Empty)
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine Machine, Machine Machine_Code, '' Remarks From Setting_MC_NO()", String.Empty, 200, 250);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool_Except_New("Machine", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine Machine, Machine Machine_Code, '' Remarks From Setting_MC_NO()", String.Empty, 200, 250);
                            }
                        }
                        else if (TxtDept.Text.ToString() == "LINKING")
                        {
                            if (Txt.Text.ToString() == String.Empty)
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine Machine, Machine Machine_Code, '' Remarks From Linking_Mc_NO_Unit()Where Unit_Code = " + TxtUnit.Tag.ToString() + " ", String.Empty, 200, 250);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool_Except_New("Machine", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine Machine, Machine Machine_Code, '' Remarks From Linking_Mc_NO_Unit()Where Unit_Code = " + TxtUnit.Tag.ToString() + "", String.Empty, 200, 250);
                            }
                        }
                        else if (TxtDept.Text.ToString() == "WASHING")
                        {
                            if (Txt.Text.ToString() == String.Empty)
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine Machine, Machine Machine_Code, '' Remarks From Washing_Mc_NO_Unit()Where Unit_Code = " + TxtUnit.Tag.ToString() + " ", String.Empty, 200, 250);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool_Except_New("Machine", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine Machine, Machine Machine_Code, '' Remarks From Washing_Mc_NO_Unit()Where Unit_Code = " + TxtUnit.Tag.ToString() + "", String.Empty, 200, 250);
                            }
                        }
                        else if (TxtDept.Text.ToString() == "STITCHING")
                        {
                            if (Txt.Text.ToString() == String.Empty)
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine_Name Machine, RowID Machine_Code, '' Remarks From Socks_Stiching_Machine_List Where Unit_Code = " + TxtUnit.Tag.ToString() + " ", String.Empty, 200, 250);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool_Except_New("Machine", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine_Name Machine, RowID Machine_Code, '' Remarks From Socks_Stiching_Machine_List Where Unit_Code = " + TxtUnit.Tag.ToString() + "", String.Empty, 200, 250);
                            }
                        }


                        if (Dr != null)
                        {
                            if (Check_Designation_Machine(Grid.CurrentCell.RowIndex))
                            {
                                MessageBox.Show("Already Designation & Machine added ...!", "Gainup");
                                Txt.Text = String.Empty;
                                Grid["Machine", Grid.CurrentCell.RowIndex].Value = DBNull.Value;
                                Grid["MachineID", Grid.CurrentCell.RowIndex].Value = DBNull.Value;
                                return;
                            }
                            else
                            {
                                Txt.Text = Dr["Machine"].ToString();
                                Grid["Machine", Grid.CurrentCell.RowIndex].Value = Dr["Machine"].ToString();
                                Grid["MachineID", Grid.CurrentCell.RowIndex].Value = Dr["Machine_Code"].ToString();
                            }
                        }
                    }
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                    {
                        e.Handled = true;
                        Grid["MachineID", Grid.CurrentCell.RowIndex].Value = DBNull.Value;
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
                if (Grid.Rows.Count > 1)
                {
                    TxtEmployees.Text = MyBase.Count(ref Grid, "Operator_Emplno", "Machine");
                    //TxtTotal.Text = MyBase.Sum(ref Grid, "Emplno", "Machine");
                }
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
                    TxtRemarks.Focus();
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

        void Shift_Selection()
        {
            try
            {
                if (MyParent.UserCode == 43)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Shift", "Select Distinct Shiftcode2 Shift, Shiftdesc, ShiftCode Code, Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' as Datetime) + Cast(Starttime as Datetime) Starttime, Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' as Datetime) + (Case when cast(Datepart (hh, Cast(EndTime as datetime)) as int) < cast(Datepart (hh, Cast(StartTime as datetime)) as int) then  dateadd(d, 1, Cast(Endtime as Datetime)) else Cast(Endtime as Datetime) end) EndTime From VAAHINI_ERP_GAINUP.DBO.shiftmst A LEft Join Employee_Production_Master_Socks B On A.shiftcode = B.Shift_Code and B.Entry_Date = Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' as Date) and A.compcode = B.Division_Code where Compcode = 2 and Shiftcode2 Not Like '%Z' and shiftcode In (15, 16, 17, 18) ", String.Empty, 50, 300);
                }
                else
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Shift", "Select Distinct Shiftcode2 Shift, Shiftdesc, ShiftCode Code, Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' as Datetime) + Cast(Starttime as Datetime) Starttime, Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' as Datetime) + (Case when cast(Datepart (hh, Cast(EndTime as datetime)) as int) < cast(Datepart (hh, Cast(StartTime as datetime)) as int) then  dateadd(d, 1, Cast(Endtime as Datetime)) else Cast(Endtime as Datetime) end) EndTime From VAAHINI_ERP_GAINUP.DBO.shiftmst A LEft Join Employee_Production_Master_Socks B On A.shiftcode = B.Shift_Code and B.Entry_Date = Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' as Date) and A.compcode = B.Division_Code where Compcode = 2 and Shiftcode2 Not Like '%Z' and shiftcode In (15,16,17) ", String.Empty, 50, 300);
                }
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Unit_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Type", "Select Unit_Name, RowID Unit_Code From Fitsocks.Dbo.Unit_Master Where RowID in (1, 2)", String.Empty, 300);
                if (Dr != null)
                {
                    TxtUnit.Tag = Dr["Unit_Code"].ToString();
                    TxtUnit.Text = Dr["Unit_Name"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        void Dept_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Department", "select DISTINCT DeptName Name, DeptCode from Vaahini_Erp_Gainup.Dbo.Depttype where compcode = (Case When " + MyParent.CompCode + " = 1 Then 2 Else 1 End) ", String.Empty, 300);
                if (Dr != null)
                {
                    TxtDept.Tag = Dr["deptcode"].ToString();
                    TxtDept.Text = Dr["Name"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Supervisor_Selection()
        {
            try
            {
                if (TxtDept.Text.ToString() == "BOARDING")
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supervisor", "Select Name Supervisor, Tno, EmplNo Supervisor_Emplno From Vaahini_erp_Gainup.Dbo.Employeemas Where designationcode in (195) and tno not like '%Z' and CompCode = 2 And Unit_Code = " + TxtUnit.Tag + " ", String.Empty, 200);
                }
                else if (TxtDept.Text.ToString() == "LINKING")
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supervisor", "Select Name Supervisor, Tno, EmplNo Supervisor_Emplno From Vaahini_erp_Gainup.Dbo.Employeemas Where designationcode in (193) and tno not like '%Z' and CompCode = 2 And Unit_Code = " + TxtUnit.Tag + " ", String.Empty, 200);
                }
                else if (TxtDept.Text.ToString() == "WASHING")
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supervisor", "Select Name Supervisor, Tno, EmplNo Supervisor_Emplno From Vaahini_erp_Gainup.Dbo.Employeemas Where designationcode in (437, 206) and tno not like '%Z' and CompCode = 2 And Unit_Code = " + TxtUnit.Tag + " ", String.Empty, 200);
                }
                else if (TxtDept.Text.ToString() == "STITCHING")
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supervisor", "Select Name Supervisor, Tno, EmplNo Supervisor_Emplno From Vaahini_erp_Gainup.Dbo.Employeemas Where designationcode in (184) and tno not like '%Z' and CompCode = 2 And Unit_Code = " + TxtUnit.Tag + " ", String.Empty, 200);
                }
                else
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supervisor", "Select Name Supervisor, Tno, EmplNo Supervisor_Emplno From Vaahini_erp_Gainup.Dbo.Employeemas Where designationcode in (195, 193, 194, 437, 206, 184) and tno not like '%Z' and CompCode = 2 And Unit_Code = " + TxtUnit.Tag + " ", String.Empty, 200);
                }
                
                if (Dr != null)
                {
                    TxtSupervisor.Text = Dr["Supervisor"].ToString();
                    TxtSupervisor.Tag = Dr["Supervisor_Emplno"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Mesthri_Selection()
        {
            try
            {
                if (TxtDept.Text.ToString() == "BOARDING")
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Mesthri", "Select Name Mesthri, Tno, EmplNo Mesthri_Emplno From Vaahini_erp_Gainup.Dbo.Employeemas Where Catcode in (1, 3) and tno not like '%Z' and CompCode = 2 And Unit_Code = " + TxtUnit.Tag + " And DeptCode = " + TxtDept.Tag.ToString() + "", String.Empty, 200, 200);
                }
                else if (TxtDept.Text.ToString() == "LINKING")
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Mesthri", "Select Name Mesthri, Tno, EmplNo Mesthri_Emplno From Vaahini_erp_Gainup.Dbo.Employeemas Where Catcode in (1, 3) and tno not like '%Z' and CompCode = 2 And Unit_Code = " + TxtUnit.Tag + " And DeptCode = " + TxtDept.Tag.ToString() + "", String.Empty, 200, 200);
                }
                else if (TxtDept.Text.ToString() == "WASHING")
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Mesthri", "Select Name Mesthri, Tno, EmplNo Mesthri_Emplno From Vaahini_erp_Gainup.Dbo.Employeemas Where Catcode in (1, 3, 5) and tno not like '%Z' and CompCode = 2 And Unit_Code = " + TxtUnit.Tag + " And DeptCode = " + TxtDept.Tag.ToString() + "", String.Empty, 200, 200);
                }
                else if (TxtDept.Text.ToString() == "STITCHING")
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Mesthri", "Select Name Mesthri, Tno, EmplNo Mesthri_Emplno From Vaahini_erp_Gainup.Dbo.Employeemas Where Catcode in (1, 3, 5, 7) and tno not like '%Z' and CompCode = 2 And Unit_Code = " + TxtUnit.Tag + " And DeptCode = " + TxtDept.Tag.ToString() + " ", String.Empty, 200, 200);
                }
                
                if (Dr != null)
                {
                    TxtMesthri.Text = Dr["Mesthri"].ToString();
                    TxtMesthri.Tag = Dr["Mesthri_Emplno"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtShift_Enter(object sender, EventArgs e)
        {
            try
            {
                if (TxtShift.Text.ToString() == String.Empty)
                {
                    Shift_Selection();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtUnit_Enter(object sender, EventArgs e)
        {
            try
            {
                if (TxtUnit.Text.ToString() == String.Empty)
                {
                    Unit_Selection();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtDept_Enter(object sender, EventArgs e)
        {
            try
            {
                if (TxtDept.Text.ToString() == String.Empty)
                {
                    Dept_Selection();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (Grid.Rows.Count > 1)
                {
                    MyBase.Row_Number(ref Grid);
                }
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                {
                    if (Dt.Rows.Count > 0)
                    {
                        if (Grid["Machine", Grid.CurrentCell.RowIndex].Value == null || Grid["Machine", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            if (TxtDept.Text.ToString() == "BOARDING")
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine Machine, Machine Machine_Code, '' Remarks From Setting_Mc_NO_Unit() Where Unit_Code = " + TxtUnit.Tag.ToString() + "", String.Empty, 200, 250);
                            }
                            else if (TxtDept.Text.ToString() == "LINKING")
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine Machine, Machine Machine_Code, '' Remarks From Linking_Mc_NO_Unit() Where Unit_Code = " + TxtUnit.Tag.ToString() + " ", String.Empty, 200, 250);
                            }
                            else if (TxtDept.Text.ToString() == "WASHING")
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine Machine, Machine Machine_Code, '' Remarks From Washing_Mc_NO_Unit() Where Unit_Code = " + TxtUnit.Tag.ToString() + " ", String.Empty, 200, 250);
                            }
                            else if (TxtDept.Text.ToString() == "STITCHING")
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine_Name Machine, RowID Machine_Code, '' Remarks From Socks_Stiching_Machine_List Where Unit_Code = " + TxtUnit.Tag.ToString() + " ", String.Empty, 200, 250);
                            }
                            if (Dr != null)
                            {
                                Txt.Text = Dr["Machine"].ToString();
                                Grid["Machine", Grid.CurrentCell.RowIndex].Value = Dr["Machine"].ToString();
                                Grid["MachineID", Grid.CurrentCell.RowIndex].Value = Dr["Machine_Code"].ToString();
                            }
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index)
                {
                    if (TxtDept.Text.ToString() != String.Empty && Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && (Grid["Operator", Grid.CurrentCell.RowIndex].Value == null || Grid["Operator", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Operator", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty))
                    {
                        if (TxtDept.Text.ToString() == "BOARDING" || TxtDept.Text.ToString() == "LINKING")
                        {
                            Dr = Tool.Selection_Tool_Except_New_WOMDI("Operator_Emplno", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Employee", "Select Name Operator, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno Operator_Emplno From VAAHINI_ERP_GAINUP.DBO.Employee_List_Production('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', 2) A Where catcode in (1,3,5,7) And A.deptcode = " + TxtDept.Tag.ToString() + " And A.Unit_Code = " + TxtUnit.Tag + " Group By Name, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno", String.Empty, 200, 80, 120, 120, 100);
                        }
                        else if (TxtDept.Text.ToString() == "WASHING")
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Employee", "Select Name Operator, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno Operator_Emplno From VAAHINI_ERP_GAINUP.DBO.Employee_List_Production('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', 2) A Where catcode in (1,3,5,7) And A.deptcode = " + TxtDept.Tag.ToString() + " And A.Unit_Code = " + TxtUnit.Tag + " Group By Name, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno", String.Empty, 200, 80, 120, 120, 100);
                        }
                        else if (TxtDept.Text.ToString() == "STITCHING")
                        {
                            Dr = Tool.Selection_Tool_Except_New_WOMDI("Operator", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Employee", "Select Name Operator, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno Operator_Emplno From VAAHINI_ERP_GAINUP.DBO.Employee_List_Production('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', 2) A Where (catcode in (1,3,5,7) And A.deptcode = " + TxtDept.Tag.ToString() + " And A.Unit_Code = " + TxtUnit.Tag + ") Or (Catcode = 2) Group By Name, Tno, DeptName, DesignationName, CatName, DesignationCode, A.Emplno", String.Empty, 200, 80, 120, 120, 100);
                        }
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Operator"].ToString();
                            Grid["Operator", Grid.CurrentCell.RowIndex].Value = Dr["Operator"].ToString();
                            Grid["Operator_Emplno", Grid.CurrentCell.RowIndex].Value = Dr["Operator_Emplno"].ToString();
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Work"].Index)
                {
                    if (Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && (Grid["Work", Grid.CurrentCell.RowIndex].Value == null || Grid["Work", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Work", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty))
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select WORK", "Select Work_Nature Work, RowID Work_Nature from Socks_Work_Nature Where DeptCode = " + TxtDept.Tag.ToString() + "", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Work"].ToString();
                            Grid["Work", Grid.CurrentCell.RowIndex].Value = Dr["Work"].ToString();
                            Grid["Work_Nature", Grid.CurrentCell.RowIndex].Value = Dr["Work_Nature"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtSupervisor_Enter(object sender, EventArgs e)
        {
            try
            {
                Supervisor_Selection();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtMesthri_Enter(object sender, EventArgs e)
        {
            try
            {
                if (TxtMesthri.Text.ToString() == String.Empty)
                {
                    Mesthri_Selection();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        
    }
}
