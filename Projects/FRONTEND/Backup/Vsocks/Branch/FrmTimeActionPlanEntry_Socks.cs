using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounts_ControlModules;
using SelectionTool_NmSp;

namespace Accounts
{
    public partial class FrmTimeActionPlanEntry_Socks : Form,Entry  
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataGrid dgv = new DataGrid();
        DataRow Dr;
        DataRow Dr1; 
        Int64 Code;
        TextBox Txt = null;        
        String[] Queries;
        String Body;
        String Sub, OrderNo = "", OrderNo_Tmp="";
        String[] t;
        Int16 PCompCode;
        Int32 C = 0;
        public FrmTimeActionPlanEntry_Socks()
        {
            InitializeComponent();
        }
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                button2.Visible = false;
                Grid_Data();              
                TxtDivision.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Plan -Edit", " Select A.Entry_No, A.Effect_From , A.Order_No, A1.BuyerName Party,  C.Lead_Time LeadTime, A.LeadTime_ID, A.Division_ID, B.COMPANY Division,A.RowID, A.Order_Date, A.Ship_Date, A.Alter_Order_Date ,A.Order_No_Temp, A.Link_RowID From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() B On A.Division_ID = B.COMPCODE Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master C On A.LeadTime_ID = C.RowID  Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master D On D.Plan_ID = A.Rowid  Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Orders() A1 On A.Order_No = A1.Order_No and A1.Order_Date = A.Order_Date and A1.Ship_Date = A.Ship_Date  Where D.Rowid Is Null and A.Division_ID =  3   ORder by A.Entry_No Desc ", string.Empty, 80, 100, 120, 120, 80);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    button2.Visible = false;                                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                Code = Convert.ToInt64(Dr["RowId"]);
                TxtEntryNo.Text = Dr["Entry_No"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["Effect_From"]);
                DtpOrdEnqDate.Value = Convert.ToDateTime(Dr["Alter_Order_Date"]);
                DtpODate.Value = Convert.ToDateTime(Dr["Order_Date"]);
                DtpSDate.Value = Convert.ToDateTime(Dr["Ship_Date"]);                
                TxtOrderNo.Text = Dr["Order_No_Temp"].ToString().Replace("$"," ");
                TxtDivision.Text = Dr["Division"].ToString();
                TxtDivision.Tag  = Dr["Division_ID"].ToString();
                TxtLeadDays.Text = Dr["LeadTime"].ToString();
                TxtLeadDays.Tag  = Dr["LeadTime_ID"].ToString();
                TxtTotalOrder.Text = Dr["Link_RowID"].ToString();
                TxtOrderList.Text = Dr["Order_No_Temp"].ToString().Replace("$", "'");
                TxtParty.Text = Dr["Party"].ToString();
                Grid_Data();
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Save()
        {
            try
            {
                button2.Visible = false ;
                Int32 Array_Index = 0;
                Total_Count();
                Body = "";
                Sub = "";
                
                if (TxtOrderNo.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Order", "Gainup");
                    TxtOrderNo.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtDivision.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Division", "Gainup");
                    TxtDivision.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtLeadDays.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid LeadDays", "Gainup");
                    TxtLeadDays.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtTotPro.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotPro.Text) == 0 || TxtTotalOrder.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotalOrder.Text) == 0)
                {
                    MessageBox.Show("Invalid Total Lead Days & Orders ", "Gainup");
                    TxtTotPro.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (Convert.ToDouble(TxtTotPro.Text) > Convert.ToDouble(TxtLeadDays.Text))
                {
                    MessageBox.Show("Lead Days Not Match", "Gainup");
                    TxtTotPro.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (MyParent._New)
                {
                    
                    DataTable TDt = new DataTable();
                    MyBase.Load_Data("Select (Isnull(Max(Entry_No), 0) + 1) No From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master Where Division_ID = " + TxtDivision.Tag + " ", ref TDt);
                    TxtEntryNo.Text = TDt.Rows[0][0].ToString();
                }
                OrderNo = TxtOrderList.Text.Replace("'", "$");
                OrderNo_Tmp = OrderNo;                
                
                for (int j = 0; j < Convert.ToInt32(TxtTotalOrder.Text); j++)
                {                    
                    if (MyParent.Edit == true && j == 0)
                    {
                        Array_Index = 0;
                        Queries = new String[4];
                        Queries[Array_Index++] = "Delete Vaahini_Erp_Gainup.Dbo.Auto_Mail_Send_TimeAction Where Name = 'PLANNING' And RowID in (Select RowID From  Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master Where Division_ID = " + TxtDivision.Tag + " and Order_Date =  '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "' and Ship_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "' and LeadTime_ID = " + TxtLeadDays.Tag + ")";
                        Queries[Array_Index++] = "Delete From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details Where Master_id in (Select RowID From  Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master Where Division_ID = " + TxtDivision.Tag + " and Order_Date =  '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "' and Ship_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "' and LeadTime_ID = " + TxtLeadDays.Tag + " and Alter_Order_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpOrdEnqDate.Value) + "')";
                        Queries[Array_Index++] = "Delete From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master Where RowID in (Select RowID From  Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master Where Division_ID = " + TxtDivision.Tag + " and Order_Date =  '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "' and Ship_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "' and LeadTime_ID = " + TxtLeadDays.Tag + " and Alter_Order_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpOrdEnqDate.Value) + "')";
                        MyBase.Run_Identity(false, Queries);                    
                    }
                    Array_Index = 0;
                    Queries = new String[Dt.Rows.Count + 30];
                    DataTable TDt1 = new DataTable();
                    MyBase.Load_Data("Select Vaahini_Erp_Gainup.Dbo.GetStringBetween2Chars('" + OrderNo_Tmp + "','$')", ref TDt1);
                    OrderNo = TDt1.Rows[0][0].ToString();
                    OrderNo_Tmp = OrderNo_Tmp.Replace('$'+OrderNo+'$', " ");
                    Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master (Entry_No,Effect_From, Division_ID, Order_No, Order_Date, Ship_Date, LeadTime_ID, Link_RowID, Order_No_Temp, Alter_Order_Date, Approval_Flag) Values (" + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtDivision.Tag + ", '" + OrderNo + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "', " + TxtLeadDays.Tag + ", " + TxtTotalOrder.Text + ", '" + TxtOrderList.Text.Replace("'", "$") + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpOrdEnqDate.Value) + "', 'T') ; Select Scope_Identity()";
                    Queries[Array_Index++] = MyParent.EntryLog("TIME & ACTION PLAN", "ADD", "@@IDENTITY");
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Grid["ACTION_ID", i].Value.ToString() != String.Empty && Grid["ACTION_ID", i].Value != DBNull.Value)
                        {                            
                            Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details (Master_ID, SNo, Action_ID, EmplNo, Lead_Days, Plan_Date, Work_Days, End_Date)  Values (@@IDENTITY," + Grid["SNO", i].Value + ", " + Grid["ACTION_ID", i].Value + ", " + Grid["EMPLNO", i].Value + ", " + Grid["LEAD_DAYS", i].Value + ", '" + String.Format("{0:dd-MMM-yyyy}", Grid["PLAN_DATE", i].Value) + "', " + Grid["WORK_DAYS", i].Value + ", '" + String.Format("{0:dd-MMM-yyyy}", Grid["END_DATE", i].Value) + "')";
                        }
                    }
                    Sub = " Time & Action Planning :     Entry No :  " + TxtEntryNo.Text.ToString() + "     Plan Date  : '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "'  ";
                    Body = " Order No :  '" + OrderNo + "'     Lead Days " + TxtLeadDays.Text.ToString() + " " + Environment.NewLine;


                    Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Auto_Mail_Send_TimeAction (Name, RowID, Body, Subject) Values ('PLANNING', @@IDENTITY, '" + Body.Replace("'", "`") + "', '" + Sub.Replace("'", "`") + "')";
                        MyBase.Run_Identity(false, Queries);                    
                   
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

        public void Entry_Print()
        {
            try
            {
                button2.Visible = false ;
                String Str;
                CrystalDecisions.CrystalReports.Engine.ReportDocument ORpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                Str = "    From Vaahini_Erp_Gainup.Dbo.Time_Action_Fn(" + TxtDivision.Tag + ") Where Plan_ID = " + Code + " ";
                MyBase.Run("Drop Table Vaahini_Erp_Gainup.Dbo.Rpt_Time_Action_Plan_Tab_Print");
                MyBase.Run("Select * Into Vaahini_Erp_Gainup.Dbo.Rpt_Time_Action_Plan_Tab_Print " + Str + "");
                Str = "Select Top 1000000000 * From Vaahini_Erp_Gainup.Dbo.Rpt_Time_Action_Plan_Tab_Print Order by Sno ";
                MyBase.Execute_Qry(Str, "Rpt_Time_Action_Plan");
                ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Time_Action_Plan.rpt");
                MyParent.FormulaFill(ref ORpt, "CompName", MyParent.CompName);
                MyParent.FormulaFill(ref ORpt, "Heading", " TIME & ACTION PLAN FOR    " + TxtOrderNo.Text + " ");
                MyParent.FormulaFill(ref ORpt, "PDate", string.Format("{0:dd-MMM-yyyy} {0:T}", MyBase.GetServerDateTime()));
                MyParent.CReport(ref ORpt, "TIME ACTION PLAN..!");
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
                button2.Visible = false;
                MyBase.Clear(this);
                //Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Plan - Delete ", " Select A.Entry_No, A.Effect_From , A.Order_No, A1.BuyerName Party,  C.Lead_Time LeadTime, A.LeadTime_ID, A.Division_ID, B.COMPANY Division,A.RowID, A.Order_Date, A.Ship_Date, A.Order_No_Temp, A.Link_RowID , A.AlteR_Order_Date From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() B On A.Division_ID = B.COMPCODE Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master C On A.LeadTime_ID = C.RowID  Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master D On D.Plan_ID = A.Rowid  Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Orders() A1 On A.Order_No = A1.Order_No and A1.Order_Date = A.Order_Date and A1.Ship_Date = A.Ship_Date  Where D.Rowid Is Null and (A.Division_ID = (Case When '" + MyParent.UserName.Contains("GKA") + "' = 'true' Then 3 When '" + MyParent.UserName.Contains("GGA") + "' = 'true' Then 4 Else 3 End) Or A.Division_ID = (Case When '" + MyParent.UserName.Contains("GKA") + "' = 'true' Then 3 When '" + MyParent.UserName.Contains("GGA") + "' = 'true' Then 4 Else 4 End)) and A.Approval_Flag = 'F' ORder by A.Entry_No Desc ", string.Empty, 80, 100, 120, 120, 80);
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Plan - Delete ", " Select A.Entry_No, A.Effect_From , A.Order_No, A1.BuyerName Party,  C.Lead_Time LeadTime, A.LeadTime_ID, A.Division_ID, B.COMPANY Division,A.RowID, A.Order_Date, A.Ship_Date, A.Order_No_Temp, A.Link_RowID , A.AlteR_Order_Date From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() B On A.Division_ID = B.COMPCODE Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master C On A.LeadTime_ID = C.RowID  Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master D On D.Plan_ID = A.Rowid  Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Orders() A1 On A.Order_No = A1.Order_No and A1.Order_Date = A.Order_Date and A1.Ship_Date = A.Ship_Date  Where D.Rowid Is Null and A.Division_ID = 3  ORder by A.Entry_No Desc ", string.Empty, 80, 100, 120, 120, 80);
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
                if (Code > 0)
                {
                    MyBase.Run("Delete From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details Where Master_ID in (Select RowID From  Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master Where Division_ID = " + TxtDivision.Tag + " and Order_Date =  '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "' and Ship_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "' and AlteR_Order_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpOrdEnqDate.Value) + "' and LeadTime_ID = " + TxtLeadDays.Tag + ")", "Delete From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master Where  RowID in (Select RowID From  Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master Where Division_ID = " + TxtDivision.Tag + " and Order_Date =  '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "' and Ship_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "' and AlteR_Order_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpOrdEnqDate.Value) + "' and LeadTime_ID = " + TxtLeadDays.Tag + ")", MyParent.EntryLog("TIME & ACTION PLAN", "DELETE", Code.ToString())); 
                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyBase.Clear(this);
                }
                MyParent.Load_DeleteEntry();
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
                button2.Visible = false;
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Plan -Edit", " Select A.Entry_No, A.Effect_From , A.Order_No, A1.BuyerName Party,  C.Lead_Time LeadTime, A.LeadTime_ID, A.Division_ID, B.COMPANY Division,A.RowID, A.Order_Date, A.Ship_Date, A.Order_No_Temp, A.Link_RowID, A.AlteR_Order_Date From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() B On A.Division_ID = B.COMPCODE Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master C On A.LeadTime_ID = C.RowID  Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master D On D.Plan_ID = A.Rowid  Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Orders() A1 On A.Order_No = A1.Order_No and A1.Order_Date = A.Order_Date and A1.Ship_Date = A.Ship_Date  Where D.Rowid Is Null and A.Division_ID = 3  ORder by A.Entry_No Desc ", string.Empty, 80, 100, 120, 120, 80);
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

        private void FrmTimeActionPlanEntry_Socks_Load(object sender, EventArgs e)
        {
            try
            {
                button2.Visible = false;
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Grid_Data()
        {
            String Str = String.Empty;          
            try
            {
                if (MyParent._New == true)
                {
                    if (TxtDivision.Text == String.Empty || TxtOrderNo.Text == String.Empty || TxtLeadDays.Text == String.Empty)
                    {
                        //Str = "Select 0 as SNO, '' ACTION_NAME, '' as EMPLOYEE, 0 as LEAD_DAYS, '' MODE, Plan_Date PLAN_DATE, 0 ACTION_ID, 0 as EMPLNO, 0 as Order_SlNo, 0 as LEAD_DAYS1, 'N' EDIT, 'S' FOLLOW_BY  From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details Where 1 = 2 ";
                        Str = "Select 0 as SNO, '' ACTION_NAME, '' as EMPLOYEE, '' MODE, 0 as LEAD_DAYS, Plan_Date PLAN_DATE, 0 as WORK_DAYS, END_DATE, 0 ACTION_ID, 0 as EMPLNO, 0 as Order_SlNo, 0 as LEAD_DAYS1, 'N' EDIT, 'S' FOLLOW_BY  From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details Where 1 = 2 ";
                    }
                    else
                    {
                        Str = "  Select 0 as SNO, B.Name ACTION_NAME, '' as EMPLOYEE,  B.FOLLOW_BY MODE,  A.LEAD_DAYS,(Case When B.Follow_By = 'S' Then DATEADD(DD, - A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "') Else DATEADD(DD,  A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpOrdEnqDate.Value) + "') End) PLAN_DATE, A.WORK_DAYS, DateAdd(DD, A.Work_Days, (Case When B.Follow_By = 'S' Then DATEADD(DD, - A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "') Else DATEADD(DD,  A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpOrdEnqDate.Value) + "') End)) END_DATE,  A.Action_ID, B.Order_SlNo, 0 EmplNo, B.Edit_Flag EDIT, B.FOLLOW_BY   From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master B On A.Action_ID = B.Rowid Where A.Division_ID = " + TxtDivision.Tag + " and A.LEadTime_ID = " + TxtLeadDays.Tag + " and B.Name Not Like 'ZZZ%'  and Len(B.Short_Name) = 3 Order by B.Order_SlNo ";
                      //  Str = " Select Distinct 0 as SNO, B.Name ACTION_NAME, IsNull(E1.Name,'') as EMPLOYEE, (Case When A.Division_ID = 3  and A.Action_ID = 51 Then A1.IOSDays  Else A.LEAD_DAYS End) LEAD_DAYS , B.Follow_By MODE, (Case When  A.Division_ID = 3  and A.Action_ID = 51 Then (Case When B.Follow_By = 'S' Then DATEADD(DD, -  A1.IOSDays,  A1.Order_Date) Else DATEADD(DD,  A1.IOSDays ,  A1.Order_Date) End) Else (Case When B.Follow_By = 'S' Then DATEADD(DD, -  A.LEAD_DAYS,  '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "') Else DATEADD(DD,  A.LEAD_DAYS ,  '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "') End) End) PLAN_DATE, A.Action_ID, B.Order_SlNo, IsNull(A1.Merchandiser_Code,0) as EMPLNO, (Case When A.Division_ID = 3 and A.Action_ID = 51 Then A1.IOSDays Else A.LEAD_DAYS  End) LEAD_DAYS1, B.Edit_Flag EDIT, B.FOLLOW_BY  From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master B On A.Action_ID = B.Rowid Left Join Order_Enquiry A1 On A1.OCNNo in (" + TxtOrderList.Text + ") and B.Rowid = 51 LEft Join VAAHINI_ERP_GAINUP.dbo.EmployeeMas E1 On E1.Emplno = A1.Merchandiser_Code  Where A.Division_ID = " + TxtDivision.Tag + " and A.LEadTime_ID = " + TxtLeadDays.Tag + "  Order by B.Order_SlNo ";
                    }
                }
                else
                {
                    Str = "Select  SNO, C.Name ACTION_NAME, D.Name EMPLOYEE, C.Follow_By MODE, B.LEAD_DAYS,  B.PLAN_DATE, B.WORK_DAYS, B.END_DATE, B.Action_ID, C.Order_SlNo, B.EmplNo EMPLNO, B.LEAD_DAYS LEAD_DAYS1, C.Edit_Flag EDIT, C.FOLLOW_BY from Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details B On A.Rowid = B.Master_ID Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master C On B.Action_ID = C.Rowid Inner Join Vaahini_Erp_Gainup.Dbo.EmployeeMas D On B.EmplNo = D.Emplno Where B.Master_ID = " + Code + " ORder By B.SNo"; 
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);                               
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);               
                MyBase.ReadOnly_Grid(ref Grid,  "PLAN_DATE", "MODE", "END_DATE");
                MyBase.Grid_Designing(ref Grid, ref Dt, "ACTION_ID", "EMPLNO", "EDIT", "FOLLOW_BY", "Order_SlNo", "LEAD_DAYS1");
                MyBase.Grid_Width(ref Grid, 50, 250, 180, 50, 70, 110, 70, 110);
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["ACTION_NAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["EMPLOYEE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["MODE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["LEAD_DAYS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["PLAN_DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["WORK_DAYS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["END_DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["PLAN_DATE"].HeaderText = "START_DATE";
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void FrmTimeActionPlanEntry_Socks_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtLeadDays")
                    {
                        Grid_Data();
                        Grid.CurrentCell = Grid["ACTION_NAME", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }                   
                    else if (this.ActiveControl.Name == "TxtTotPro")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (MyParent._New)
                    {
                        if (this.ActiveControl.Name == "TxtDivision")
                        {
                            //if (MyParent.UserName.Contains("GKA"))
                            //{
                                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Division", "Select COMPANY, COMPCODE  From Vaahini_Erp_Gainup.Dbo.Division_Mas () Where CompCode in (3) ORder by COMPCODE ", String.Empty, 400);
                            //}
                            //else if (MyParent.UserName.Contains("GGA"))
                            //{
                            //    Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Division", "Select COMPANY, COMPCODE  From Vaahini_Erp_Gainup.Dbo.Division_Mas () Where CompCode in (4) ORder by COMPCODE ", String.Empty, 400);
                            //}
                            //else
                            //{
                            //    Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Division", "Select COMPANY, COMPCODE  From Vaahini_Erp_Gainup.Dbo.Division_Mas () Where CompCode in (3,4) ORder by COMPCODE ", String.Empty, 400);
                            //}
                            if (Dr != null)
                            {
                                TxtDivision.Text = Dr["COMPANY"].ToString();
                                TxtDivision.Tag = Dr["COMPCODE"].ToString();
                                TxtOrderNo.Text = "";
                                TxtOrderNo.Tag = "";
                                TxtOrderList.Text = "";
                                TxtLeadDays.Text = "";
                                TxtDivision.Focus();
                                return;
                            }
                        }
                        if (this.ActiveControl.Name == "TxtOrderNo")
                        {
                            if (TxtDivision.Text != String.Empty)
                            {
                                if (TxtOrderNo.Text.ToString() == String.Empty)
                                {
                                    Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select OrderNo", "Select A.Order_No, A.BuyerName Party, A.Order_Date, A.Ship_Date,  A.Tot_Days, A1.Order_No RefNo, A1.Order_Date OrdEnq_Date, A1.IOSDays  From Vaahini_Erp_Gainup.Dbo.Time_Action_Orders() A Left Join Order_Enquiry A1 On A1.OCNNo = A.Order_No Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master B On A.Order_No = B.Order_No Where A.Ship_Date is not null and A.Division = " + TxtDivision.Tag + " and B.Order_No Is Null  Order By A.Order_No ", String.Empty, 140, 200, 150, 150, 100, 100, 80);
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select OrderNo", "Select A.Order_No, A.BuyerName Party, A.Order_Date, A.Ship_Date,  A.Tot_Days, A1.Order_No RefNo, A1.Order_Date OrdEnq_Date, A1.IOSDays  From Vaahini_Erp_Gainup.Dbo.Time_Action_Orders() A Left Join Order_Enquiry A1 On A1.OCNNo = A.Order_No Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master B On A.Order_No = B.Order_No Where A.Ship_Date is not null and A.Division = " + TxtDivision.Tag + " and A.Order_No Not In(" + TxtOrderList.Text.ToString() + ")  and A.Order_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "' and A.BuyerName = '" + TxtParty.Text + "'   and B.Order_No Is Null  Order By A.Order_No ", String.Empty, 140, 200, 150, 150, 100, 100, 80);
                                }
                                if (Dr != null)
                                {
                                    if (TxtOrderNo.Text.Trim().ToString() == String.Empty)
                                    {
                                        TxtOrderNo.Text = Dr["Order_No"].ToString();
                                      //  TxtOrderNo.Tag = Dr["IOSDays"].ToString();
                                        TxtOrderList.Text = "'" + Dr["Order_No"].ToString() + "'";
                                        DtpODate.Value = Convert.ToDateTime(Dr["Order_Date"].ToString());
                                        DtpSDate.Value = Convert.ToDateTime(Dr["Ship_Date"].ToString());
                                        DtpOrdEnqDate.Value = Convert.ToDateTime(Dr["Order_Date"].ToString());
                                        TxtParty.Text = Dr["Party"].ToString();
                                        C = 1;
                                    }
                                    else
                                    {
                                        TxtOrderNo.Text = TxtOrderNo.Text.ToString() + " , " + Dr["Order_No"].ToString();
                                        TxtOrderList.Text = TxtOrderList.Text.ToString() + " , '" + Dr["Order_No"].ToString() + "'";
                                        C = C + 1;
                                    }
                                    TxtTotalOrder.Text = C.ToString();
                                    if (MyParent._New)
                                    {
                                        TxtLeadDays.Text = "";
                                        Grid_Data();
                                    }
                                    TxtOrderNo.Focus();
                                    TxtOrderNo.DeselectAll();
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid Division", "Gainup");
                                return;
                            }

                        }


                        if (this.ActiveControl.Name == "TxtLeadDays")
                        {
                            if (TxtOrderNo.Text.ToString() != String.Empty)
                            {
                                //if (TxtDivision.Tag.ToString() == "3")
                                //{
                                    Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select LeadDays", "Select Distinct A.Lead_Time, A.RowID LeadTime_ID From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master B On A.RowID = B.LeadTime_ID  ORder by A.Lead_Time", String.Empty, 150);
                                //}
                                //else
                                //{
                                //    Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select LeadDays", "Select A.Lead_Time, B.RowID LeadTime_ID From (Select MAX(A.Lead_Time) Lead_Time from Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master B On A.RowID = B.LeadTime_ID  Where A.Lead_Time <= DateDiff(DD,'" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "') Union Select Min(A.Lead_Time) Lead_Time from Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master B On A.RowID = B.LeadTime_ID Where A.Lead_Time >= DateDiff(DD,'" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "') ) A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master B On A.Lead_Time = B.Lead_Time ", String.Empty, 150);
                                //}
                                if (Dr != null)
                                {
                                    TxtLeadDays.Text = Dr["Lead_Time"].ToString();
                                    TxtLeadDays.Tag = Dr["LeadTime_ID"].ToString();
                                    TxtLeadDays.Focus();
                                    Grid_Data();
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid OrderNo", "Gainup");
                                return;
                            }
                        }
                    }

                }
                else if (this.ActiveControl.Name == "TxtOrderNo" && (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete))
                {
                    TxtOrderNo.Text = "";
                    TxtOrderList.Text = "";
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    MyBase.ActiveForm_Close(this, MyParent);
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
                    Txt.Leave +=new EventHandler(Txt_Leave);                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

         void Txt_Leave(object sender, EventArgs e)
        {
                try
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["LEAD_DAYS"].Index)
                    {
                        Total_Count();
                        if (Convert.ToInt32 (Grid["LEAD_DAYS", Grid.CurrentCell.RowIndex].Value) > Convert.ToInt32(TxtLeadDays.Text))
                        {
                            MessageBox.Show("LeadDays are Not Greater Than '" + TxtLeadDays.Text + "' ", "Gainup");
                            Grid["LEAD_DAYS", Grid.CurrentCell.RowIndex].Value = Grid["LEAD_DAYS1", Grid.CurrentCell.RowIndex].Value;                            
                            return;                            
                        }

                        DataTable  TmpDt = new DataTable();
                        if (Grid["FOLLOW_BY", Grid.CurrentCell.RowIndex].Value.ToString() == "S")
                        {
                            MyBase.Load_Data("Select DateAdd(D, -" + Convert.ToInt32(Grid["LEAD_DAYS", Grid.CurrentCell.RowIndex].Value) + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "' ) ", ref TmpDt);
                        }
                        else
                        {
                            MyBase.Load_Data("Select DateAdd(D, " + Convert.ToInt32(Grid["LEAD_DAYS", Grid.CurrentCell.RowIndex].Value) + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpOrdEnqDate.Value) + "' ) ", ref TmpDt);
                        }
                        //Grid["PLAN_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(DtpODate.Value).AddDays(Convert.ToInt32(Grid["LEAD_DAYS", Grid.CurrentCell.RowIndex].Value));
                        if (TmpDt.Rows.Count > 0)
                        {
                            Grid["PLAN_DATE", Grid.CurrentCell.RowIndex].Value = TmpDt.Rows[0][0];
                        }
                    }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["WORK_DAYS"].Index)
                        {
                            Total_Count();
                            if (Convert.ToInt32(Grid["WORK_DAYS", Grid.CurrentCell.RowIndex].Value) > Convert.ToInt32(TxtLeadDays.Text))
                            {
                                MessageBox.Show("WorkDays are Not Greater Than '" + TxtLeadDays.Text + "' ", "Gainup");
                                Grid["WORK_DAYS", Grid.CurrentCell.RowIndex].Value = 0;
                                return;
                            }
                            if (Grid["WORK_DAYS", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                DataTable TmpDt1 = new DataTable();
                                MyBase.Load_Data("Select DateAdd(D, " + Convert.ToInt32(Grid["WORK_DAYS", Grid.CurrentCell.RowIndex].Value) + ", '" + String.Format("{0:dd-MMM-yyyy}", Grid["PLAN_DATE", Grid.CurrentCell.RowIndex].Value) + "' ) ", ref TmpDt1);
                                if (TmpDt1.Rows.Count > 0)
                                {
                                    Grid["END_DATE", Grid.CurrentCell.RowIndex].Value = TmpDt1.Rows[0][0];
                                }
                            }
                        }                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (TxtOrderNo.Text.Trim() != String.Empty)
                    {
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ACTION_NAME"].Index)
                        {                            
                            Dr = Tool.Selection_Tool_Except_New("ACTION_NAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "ACTION_NAME", " Select B.Name ACTION_NAME, A.LEAD_DAYS, B.Follow_By MODE, (Case When B.Follow_By = 'S' Then DATEADD(DD, - A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "') Else DATEADD(DD,  A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpOrdEnqDate.Value) + "') End) PLAN_DATE, A.WORK_DAYS, DateAdd(DD, A.Work_Days, (Case When B.Follow_By = 'S' Then DATEADD(DD, - A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "') Else DATEADD(DD,  A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpOrdEnqDate.Value) + "') End)) END_DATE,  A.Action_ID, B.Order_SlNo, B.Edit_Flag   From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master B On A.Action_ID = B.Rowid Where A.Division_ID = " + TxtDivision.Tag + " and A.LEadTime_ID = " + TxtLeadDays.Tag + " and B.Name Not Like 'ZZZ%'  and Len(B.Short_Name) = 3 Order by B.Order_SlNo ", String.Empty, 200, 80, 80, 100, 80, 100);                           
                            if (Dr != null)
                            {
                                Grid["ACTION_NAME", Grid.CurrentCell.RowIndex].Value = Dr["ACTION_NAME"].ToString();
                                Grid["ACTION_ID", Grid.CurrentCell.RowIndex].Value = Dr["ACTION_ID"].ToString();
                                Grid["LEAD_DAYS", Grid.CurrentCell.RowIndex].Value = Dr["LEAD_DAYS"].ToString();
                                Grid["MODE", Grid.CurrentCell.RowIndex].Value = Dr["MODE"].ToString();
                                Grid["PLAN_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Dr["PLAN_DATE"].ToString());
                                Grid["EDIT", Grid.CurrentCell.RowIndex].Value = Dr["Edit_Flag"].ToString();
                                Grid["Order_SlNo", Grid.CurrentCell.RowIndex].Value = Dr["Order_SlNo"].ToString();
                                Grid["FOLLOW_BY", Grid.CurrentCell.RowIndex].Value = Dr["Mode"].ToString();
                                Grid["LEAD_DAYS1", Grid.CurrentCell.RowIndex].Value = Dr["LEAD_DAYS"].ToString();
                                Grid["WORK_DAYS", Grid.CurrentCell.RowIndex].Value = Dr["WORK_DAYS"].ToString();
                                Grid["END_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Dr["END_DATE"].ToString());
                                Txt.Text = Dr["ACTION_NAME"].ToString();
                                return;
                            }
                        }
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["EMPLOYEE"].Index)
                        {
                            //Dr = Tool.Selection_Tool_Except_New("EMPLNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "EMPLOYEE NAME", " select Distinct B.Name, B.Tno, A.EMPLNO  From Pay_Att A Left Join EmployeeMas B On A.emplno = B.Emplno where cast(etime as date)= '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "' Order By Name ", String.Empty, 200);
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "EMPLOYEE NAME", " Select B.Name , B.tno TNo, B.DeptName, B.DesignationName, A.EMPLNO From Socks_User_MAster  A Inner Join Vaahini_Erp_Gainup.Dbo.MIS_Employee_Basic () B On A.Emplno = B.Emplno Where B.tno Not Like '%z' and B.Tno  Like 'GKA%' Order By B.Name ", String.Empty, 200, 120, 150, 150);
                            if (Dr != null)
                            {
                                Grid["EMPLOYEE", Grid.CurrentCell.RowIndex].Value = Dr["Name"].ToString();
                                Grid["EMPLNO", Grid.CurrentCell.RowIndex].Value = Dr["EMPLNO"].ToString();
                                Txt.Text = Dr["Name"].ToString();
                                return;
                            }
                        }
                        Total_Count();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Order No..!", "Gainup");
                        return;
                    }
                }                
               Total_Count();               
               if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
        void Total_Count()
        {
            Double Kgs= 0;
            try
            {                
                TxtTotPro.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Grid_Max(ref Grid, "LEAD_DAYS", "EMPLOYEE")));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["LEAD_DAYS"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["WORK_DAYS"].Index)
                {
                    if (Grid["EDIT", Grid.CurrentCell.RowIndex].Value.ToString() == "Y" && Grid["EMPLNO", Grid.CurrentCell.RowIndex].Value.ToString() != "0")
                    {
                        MyBase.Valid_Number(Txt, e);
                    }
                    else
                    {
                        MyBase.Valid_Null(Txt, e);
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["SNO"].Index)
                {
                    MyBase.Valid_Number(Txt, e);
                }
                else
                {
                    MyBase.Valid_Null(Txt, e);
                }
                Total_Count();
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
                    Total_Count();
                    TxtTotPro.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmTimeActionPlanEntry_Socks_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {
                if (this.ActiveControl.Name == String.Empty)
                {
                    MyBase.Valid_Number((TextBox)this.ActiveControl, e);
                }                
                else
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
            }
        }

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {                       
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }      

        private void Grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid);
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {                
                    Total_Count();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyParent._New)
                {
                    if (TxtLeadDays.Text.ToString() != String.Empty)
                    {
                        Dr1 = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Plan -View", " Select A.Entry_No, A.Effect_From , A.Order_No, B.COMPANY Division, C.Lead_Time LeadTime, A.LeadTime_ID, A.Division_ID, A.RowID, A.Order_Date, A.Ship_Date From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() B On A.Division_ID = B.COMPCODE Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master C On A.LeadTime_ID = C.RowID and A.Division_ID = (Case When '" + MyParent.UserName.Contains("GKA") + "' = 'true' Then 3 Else 4 End) and A.LeadTime_Id = " + TxtLeadDays.Tag + " ORder by A.Entry_No Desc ", string.Empty, 80, 100, 120, 120, 80);
                        if (Dr1 != null)
                        {
                            String Str = " Select 0 as SNO, B.Name ACTION_NAME, '' as EMPLOYEE,  B.FOLLOW_BY MODE,  A.LEAD_DAYS,(Case When B.Follow_By = 'S' Then DATEADD(DD, - A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "') Else DATEADD(DD,  A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpOrdEnqDate.Value) + "') End) PLAN_DATE, A.WORK_DAYS, DateAdd(DD, A.Work_Days, (Case When B.Follow_By = 'S' Then DATEADD(DD, - A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "') Else DATEADD(DD,  A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpOrdEnqDate.Value) + "') End)) END_DATE,  A.Action_ID, B.Order_SlNo, 0 EmplNo, B.Edit_Flag EDIT, B.FOLLOW_BY     From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master B On A.Action_ID = B.Rowid  Where A.Division_ID = " + TxtDivision.Tag + " and A.LEadTime_ID = " + TxtLeadDays.Tag + "  Order by B.Order_SlNo ";
                            Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                            MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                            MyBase.ReadOnly_Grid(ref Grid, "PLAN_DATE", "MODE", "END_DATE");
                            MyBase.Grid_Designing(ref Grid, ref Dt, "ACTION_ID", "EMPLNO", "EDIT", "FOLLOW_BY", "Order_SlNo", "LEAD_DAYS1");
                            MyBase.Grid_Width(ref Grid, 50, 250, 180, 70, 60, 110);               
                            Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            Grid.Columns["ACTION_NAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            Grid.Columns["EMPLOYEE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            Grid.Columns["MODE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            Grid.Columns["LEAD_DAYS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            Grid.Columns["PLAN_DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            Grid.Columns["WORK_DAYS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            Grid.Columns["END_DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Lead Days ", "Gainup");
                        TxtLeadDays.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DtpOrdEnqDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Grid_Data();
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
                return;
                if (MessageBox.Show("Sure to Approve ..!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
                if (TxtEntryNo.Text.ToString() != String.Empty)
                {
                    MyBase.Run("Update Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master Set Approval_Flag = 'T' Where RowID in (Select RowID From  Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master Where Division_ID = " + TxtDivision.Tag + " and Order_Date =  '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "' and Ship_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "' and LeadTime_ID = " + TxtLeadDays.Tag + " and Alter_Order_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpOrdEnqDate.Value) + "' and Approval_Flag = 'F')");
                    MessageBox.Show("Approved", "Gainup");
                    MyBase.Clear(this);

                    String Str = "Select 0 as SNO, '' ACTION_NAME, '' as EMPLOYEE, '' MODE, 0 as LEAD_DAYS, Plan_Date PLAN_DATE, 0 as WORK_DAYS, END_DATE, 0 ACTION_ID, 0 as EMPLNO, 0 as Order_SlNo, 0 as LEAD_DAYS1, 'N' EDIT, 'S' FOLLOW_BY  From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details Where 1 = 2 ";
                    Grid.DataSource = MyBase.Load_Data(Str, ref Dt);   
                    button2.Visible = false;                   
                }
                else
                {
                    MessageBox.Show("Invalid OrderNo", "Gainup");
                    TxtOrderNo.Focus();
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
