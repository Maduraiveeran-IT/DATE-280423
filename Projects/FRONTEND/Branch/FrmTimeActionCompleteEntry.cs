using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using Accounts;

namespace Accounts
{
    public partial class FrmTimeActionCompleteEntry : Form,Entry  
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataGrid dgv = new DataGrid();
        DataRow Dr;        
        Int64 Code;
        TextBox Txt = null;        
        String[] Queries;
        String[] t;
        String Body;
        String Sub;
        Int16 PCompCode;
        String CORderNo, CDivision;
        Int64 CPlan_ID, CLeadID, CLeadDays, CDivision_ID, CAction_ID, CComp_ID;
        DateTime CODate, CSDate;
        public FrmTimeActionCompleteEntry(String ORderNo,Int64 Plan_ID, DateTime ODate, DateTime SDate, Int64 LeadDays, Int64 LeadID, String Division, Int64 Division_ID, Int64 Action_ID, Int64 Comp_ID)
        {
            InitializeComponent();
            CORderNo = ORderNo;
            CPlan_ID = Plan_ID;
            CODate = ODate;
            CSDate = SDate;
            CLeadDays = LeadDays;
            CLeadID = LeadID;
            CDivision = Division;
            CDivision_ID = Division_ID;
            CAction_ID = Action_ID;
            CComp_ID = Comp_ID;
        }
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                Code = 0;
                TxtOrderNo.Text = CORderNo.ToString();
                TxtEntryNo.Tag = Convert.ToInt64(CPlan_ID);
                TxtLeadDays.Text = Convert.ToInt64(CLeadDays).ToString();
                TxtLeadDays.Tag = Convert.ToInt64(CLeadID);
                DtpODate.Value = Convert.ToDateTime(CODate);
                DtpSDate.Value = Convert.ToDateTime(CSDate);
                TxtDivision.Text = CDivision.ToString();
                TxtDivision.Tag = Convert.ToInt64(CDivision_ID);
                Grid_Data();
                Grid.CurrentCell = Grid["COMP_FLAG", 0];
                Grid.Focus();
                Grid.BeginEdit(true); 
               
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
                return;
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Complete -Edit", " Select D.Entry_No, D.Effect_From , A.Order_No, B.COMPANY Division, C.Lead_Time LeadTime, A.LeadTime_ID, A.Division_ID, A.Order_Date, A.Ship_Date, D.Rowid , D1.Plan_Id_Dtl Plan_ID  From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() B On A.Division_ID = B.COMPCODE Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master C On A.LeadTime_ID = C.RowID Inner join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master D On  D.EmplNo = " + MyParent.EmplNo_TA  + " Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Details D1 On D.Rowid = D1.Master_ID and A.Rowid = D1.Plan_Id_Dtl Inner Join Socks_User_Master E On E.Emplno = " + MyParent.EmplNo_TA  + "   ORder by D.Entry_No Desc ", string.Empty, 80, 100, 120, 120, 80);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    TxtOrderNo.Focus();
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
                TxtEntryNo.Tag  = Dr["Plan_ID"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["Effect_From"]);
                DtpODate.Value = Convert.ToDateTime(Dr["Order_Date"]);
                DtpSDate.Value = Convert.ToDateTime(Dr["Ship_Date"]);
                TxtOrderNo.Text = Dr["Order_No"].ToString();
                TxtDivision.Text = Dr["Division"].ToString();
                TxtDivision.Tag  = Dr["Division_ID"].ToString();
                TxtLeadDays.Text = Dr["LeadTime"].ToString();
                TxtLeadDays.Tag  = Dr["LeadTime_ID"].ToString();                      
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
               

                if (MyParent._New)
                {
                    Queries = new String[Dt.Rows.Count + 10];                    
                    DataTable TDt = new DataTable();
                    MyBase.Load_Data("Select (Isnull(Max(Entry_No), 0) + 1) No From Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master ", ref TDt);                                                                                       
                    TxtEntryNo.Text = TDt.Rows[0][0].ToString();
                    Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master (Entry_No,Effect_From, Plan_ID, EmplNo) Values (" + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', 0, " + TxtEmployee.Tag + " ) ; Select Scope_Identity()";
                    Queries[Array_Index++] = MyParent.EntryLog("TIME & ACTION COMPLETE", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries = new String[Dt.Rows.Count + 10];
                    Queries[Array_Index++] = "Update Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master Set  Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "' , Plan_ID = '" + TxtEntryNo.Tag + "', EmplNo = " + TxtEmployee.Tag + "  Where Rowid = " + Code;
                    Queries[Array_Index++] = "Delete From Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Details Where Master_id = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("TIME & ACTION COMPLETE", "EDIT", Code.ToString());
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["ACTION_ID", i].Value.ToString() != String.Empty && Grid["ACTION_ID", i].Value != DBNull.Value)
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Details (Master_ID, SNo, Action_ID, Complete_Flag, Complete_Date, Remarks, Plan_ID_Dtl)  Values (@@IDENTITY," + Grid["SNO", i].Value + ", " + Grid["ACTION_ID", i].Value + ", '" + Grid["COMP_FLAG", i].Value + "', '" + String.Format("{0:dd-MMM-yyyy}", Grid["COMPLETE_DATE", i].Value) + "', '" + Grid["REMARKS", i].Value + "', " + TxtEntryNo.Tag + ")";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Details (Master_ID, SNo, Action_ID, Complete_Flag, Complete_Date, Remarks, Plan_ID_Dtl)  Values (" + Code + "," + Grid["SNO", i].Value + ", " + Grid["ACTION_ID", i].Value + ", '" + Grid["COMP_FLAG", i].Value + "', '" + String.Format("{0:dd-MMM-yyyy}", Grid["COMPLETE_DATE", i].Value) + "', '" + Grid["REMARKS", i].Value + "', " + TxtEntryNo.Tag + ")";
                        }
                    }
                }
                Sub = " Time & Action Complete :     Entry No :  " + TxtEntryNo.Text.ToString() + "     Entry Date  : '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "'     ";
                Body = " Entered By :  '" +  TxtEmployee.Text.ToString()  + "'     Order No : '" + TxtOrderNo.Text.ToString() + "   Lead Days : " + TxtLeadDays.Text.ToString() + " " + Environment.NewLine;

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Auto_Mail_Send_TimeAction (Name, RowID, Body, Subject) Values ('COMPLETE', @@IDENTITY, '" + Body.Replace("'", "`") + "', '" + Sub.Replace("'", "`") + "')";
                    MyBase.Run_Identity(false, Queries);
                }
                else
                {
                    Queries[Array_Index++] = "Delete Vaahini_Erp_Gainup.Dbo.Auto_Mail_Send_TimeAction Where Name = 'COMPLETE' And RowID = " + Code;
                    Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Auto_Mail_Send_TimeAction (Name, RowID, Mail_Flag, Update_Mail_Flag, Body, Subject) Values ('COMPLETE', " + Code + ",  0 , 1, '" + Body.Replace("'", "`") + "',  '" + Sub.Replace("'", "`") + "')";
                    MyBase.Run_Identity(true, Queries);
                }
                MyParent.Save_Error = false;
                MessageBox.Show("Saved ..!", "Gainup");
                MyBase.Clear(this);
                this.Close();
                MyParent.Completion_Entry(0);
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
                MyBase.Clear(this);
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
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Complete - Delete ", " Select D.Entry_No, D.Effect_From , A.Order_No, B.COMPANY Division, C.Lead_Time LeadTime, A.LeadTime_ID, A.Division_ID, A.Order_Date, A.Ship_Date, D.Rowid , D1.Plan_Id_Dtl Plan_ID  From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() B On A.Division_ID = B.COMPCODE Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master C On A.LeadTime_ID = C.RowID Inner join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master D On  D.EmplNo = " + MyParent.EmplNo_TA  + " Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Details D1 On D.Rowid = D1.Master_ID and A.Rowid = D1.Plan_Id_Dtl Inner Join Socks_User_Master E On E.Emplno = " + MyParent.EmplNo_TA  + "   ORder by D.Entry_No Desc ", string.Empty, 80, 100, 120, 120, 80);
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
                    MyBase.Run("Delete From Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Details Where Master_ID = " + Code, "Delete From Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master Where  RowID = " + Code, MyParent.EntryLog("TIME & ACTION COMPLETE", "DELETE", Code.ToString())); 
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
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Complete - View", " Select D.Entry_No, D.Effect_From , A.Order_No, B.COMPANY Division, C.Lead_Time LeadTime, A.LeadTime_ID, A.Division_ID, A.Order_Date, A.Ship_Date, D.Rowid , D1.Plan_Id_Dtl Plan_ID  From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() B On A.Division_ID = B.COMPCODE Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master C On A.LeadTime_ID = C.RowID Inner join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master D On  D.EmplNo = " + MyParent.EmplNo_TA + " Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Details D1 On D.Rowid = D1.Master_ID and A.Rowid = D1.Plan_Id_Dtl Inner Join Socks_User_Master E On E.Emplno = " + MyParent.EmplNo_TA + "   ORder by D.Entry_No Desc ", string.Empty, 80, 100, 120, 120, 80);
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

        private void FrmTimeActionCompleteEntry_Load(object sender, EventArgs e)
        {
            try
            {
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
                DataTable TDte = new DataTable();
                //MyBase.Load_Data("Select Name From EmployeeMas Where Emplno = " + MyParent.Emplno + " ", ref TDte);
                MyBase.Load_Data("Select Distinct Employee, EmplNO_Org EmplNo From Vaahini_Erp_Gainup.Dbo.Time_Action_Fn(" + CDivision_ID + ") Where Plan_ID = " + CPlan_ID + " and Action_ID = " + CAction_ID + " and Complete_Flag = 'N'", ref TDte);
                if (TDte.Rows.Count > 0)
                {
                    TxtEmployee.Text = TDte.Rows[0][0].ToString();
                    TxtEmployee.Tag = TDte.Rows[0][1].ToString();
                }
                else
                {
                    TxtEmployee.Text = MyParent.UserName.ToString();
                }
                if (MyParent._New == true)
                {
                    if (TxtDivision.Text == String.Empty || TxtOrderNo.Text == String.Empty || TxtLeadDays.Text == String.Empty)
                    {
                        Str = "Select 0 as SNO, F.Name ACTION_NAME, B.LEAD_DAYS, F.Follow_By MODE,  B.PLAN_DATE, 'N' COMP_FLAG,  Cast(Getdate() as DateTime) COMPLETE_DATE, DateDiff(DD,Cast(GETDATE() as Date), B.PLAN_DATE)  DIFF_DAYS, '' REMARKS, B.ACTION_ID, 1 as T From Socks_User_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details B On A.Emplno = B.EmplNo Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master C On B.Master_ID = C.Rowid Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master D On C.LeadTime_ID = D.RowID Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Orders() E On C.Order_No = E.Order_No Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master F On B.Action_ID = F.Rowid   Where 1 = 2 ";
                    }
                    else
                    {
                        Str = "Select 0 as SNO, F.Name ACTION_NAME, B.LEAD_DAYS, F.Follow_By MODE, B.PLAN_DATE, 'N' COMP_FLAG, Cast(Getdate() as DateTime) COMPLETE_DATE,  DateDiff(DD,Cast(GETDATE() as Date), B.PLAN_DATE) DIFF_DAYS, '' REMARKS, B.ACTION_ID, 1 as T From Socks_User_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details B On A.Emplno = B.EmplNo Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master C On B.Master_ID = C.Rowid Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master D On C.LeadTime_ID = D.RowID Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Orders() E On C.Order_No = E.Order_No and E.Division = " + TxtDivision.Tag + " Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master F On B.Action_ID = F.Rowid  Where E.ORder_No = '" + TxtOrderNo.Text + "' and C.LEadTime_ID = " + TxtLeadDays.Tag + "  and B.EmplNo = " + TxtEmployee.Tag + "  and B.Action_ID = " + CAction_ID + " ORder By B.SNo ";
                    }
                }
                else 
                {
                    Str = "Select F.SNO, G.Name ACTION_NAME, H.Lead_Days LEAD_DAYS, G.Follow_By MODE, H.Plan_Date PLAN_DATE, F.Complete_Flag COMP_FLAG, (Case When F.Complete_Flag = 'N' Then Cast(GetDate() as Date) Else F.COMPLETE_DATE End)  COMPLETE_DATE , DATEDIFF(DD,F.Complete_Date, H.Plan_Date) DIFF_DAYS, F.REMARKS , F.ACTION_ID, 1 as T From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() B On A.Division_ID = B.COMPCODE Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master C On A.LeadTime_ID = C.RowID  Inner Join Socks_User_Master E On E.Emplno = " + MyParent.EmplNo_TA + " Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Details F On  A.RowID = F.Plan_ID_Dtl Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master G On F.Action_ID = G.Rowid Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details H On A.Rowid = H.Master_ID and H.EmplNo = " + TxtEmployee.Tag + " and H.Action_ID = F.Action_ID Where F.Master_id  = " + Code + "";                        
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);                               
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid, "COMP_FLAG", "COMPLETE_DATE", "REMARKS");
                MyBase.Grid_Designing(ref Grid, ref Dt, "ACTION_ID", "T");
                MyBase.Grid_Width(ref Grid, 40, 180, 60, 60, 100, 80, 100, 80, 300);               
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["ACTION_NAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["COMP_FLAG"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["LEAD_DAYS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["PLAN_DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["MODE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["COMPLETE_DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["REMARKS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["DIFF_DAYS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (MyParent._New)
                {
                    Str = "Select Distinct Comp_Entry_Date DATE, Remarks HISTORY From Vaahini_Erp_Gainup.Dbo.Time_Action_Fn(3) Where Order_No =  '" + TxtOrderNo.Text + "' and LeadTime_ID = " + TxtLeadDays.Tag + " and Action_ID = " + Grid["ACTION_ID", Grid.CurrentCell.RowIndex].Value + " and Current_Status = 'P'  and Remarks Is Not Null and Complete_ID != " + Code + " Order By Comp_Entry_Date ";
                }
                else
                {
                    Str = "Select Distinct Comp_Entry_Date DATE, Remarks HISTORY From Vaahini_Erp_Gainup.Dbo.Time_Action_Fn(3) Where Order_No =  '" + TxtOrderNo.Text + "' and LeadTime_ID = " + TxtLeadDays.Tag + " and Action_ID = " + Grid["ACTION_ID", Grid.CurrentCell.RowIndex].Value + " and Remarks Is Not Null and Complete_ID != " + Code + " Order By Comp_Entry_Date ";
                }
                    Grid1.DataSource = MyBase.Load_Data(Str, ref Dt1);
                    MyBase.Grid_Colouring(ref Grid1, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid1, 110, 800);
                    Grid1.Columns["DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    Grid1.Columns["HISTORY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    Grid1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void FrmTimeActionCompleteEntry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtOrderNo")
                    {
                        Grid_Data();
                        Grid.CurrentCell = Grid["COMP_FLAG", 0];
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
                    return;
                    if (this.ActiveControl.Name == "TxtDivision")
                    {
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Division", "Select COMPANY, COMPCODE  From Vaahini_Erp_Gainup.Dbo.Division_Mas () Where CompCode in (3,4) ORder by COMPCODE ", String.Empty, 400);
                        if (Dr != null)
                        {
                            TxtDivision.Text = Dr["COMPANY"].ToString();
                            TxtDivision.Tag = Dr["COMPCODE"].ToString();
                            TxtOrderNo.Text = "";
                            TxtLeadDays.Text = "";
                            TxtOrderNo.Tag = "";
                            TxtDivision.Focus();
                            return;
                        }
                    }

                    if (this.ActiveControl.Name == "TxtOrderNo")
                    {
                        if (TxtDivision.Text != String.Empty)
                        {
                            Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select OrderNo", "Select  Distinct E.Order_No, E.Order_Date, E.Ship_Date, D.Lead_Time, C.LeadTime_ID, C.RowID   from Socks_User_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details B On A.Emplno = B.EmplNo Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master C On B.Master_ID = C.Rowid Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master D On C.LeadTime_ID = D.RowID Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Orders() E On C.Order_No = E.Order_No and E.Division = " + TxtDivision.Tag + " Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master F On C.Rowid = F.Plan_ID and F.EmplNo = " + MyParent.EmplNo_TA + " Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Details G  On F.Rowid = G.Master_ID and F.EmplNo =  " + MyParent.EmplNo_TA  + "  Where B.EmplNo = " + MyParent.EmplNo_TA  + "  and G.Master_ID is Null and C.Approval_Flag = 'T' Order By E.Order_No ", String.Empty, 250, 150, 150, 80);
                            if (Dr != null)
                            {
                                TxtOrderNo.Text = Dr["Order_No"].ToString();
                                TxtEntryNo.Tag = Dr["RowID"].ToString();
                                DtpODate.Value = Convert.ToDateTime(Dr["Order_Date"].ToString());
                                DtpSDate.Value = Convert.ToDateTime(Dr["Ship_Date"].ToString());
                                TxtLeadDays.Text = Dr["Lead_Time"].ToString();
                                TxtLeadDays.Tag  = Dr["LeadTime_ID"].ToString();
                                Grid_Data();
                                TxtOrderNo.Focus();
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Division", "Gainup");
                            return;
                        }
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["COMP_FLAG"].Index)
                    {
                        if (Grid["COMP_FLAG", Grid.CurrentCell.RowIndex].Value.ToString() == "N")
                        {
                            Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value  = MyBase.GetServerDate();
                        }                        
                    }
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["COMPLETE_DATE"].Index)
                    {
                        Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value = MyBase.Get_Date_Format(Txt.Text);

                        if (Convert.ToDateTime(Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value) > MyBase.GetServerDateTime())
                        {
                            MessageBox.Show("Invalid Date", "Gainup");
                            Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value = MyBase.GetServerDate();
                            Grid.CurrentCell = Grid["COMPLETE_DATE", 0];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                        else if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value), MyBase.GetServerDateTime()) > 2)
                        {
                            MessageBox.Show("Invalid Date", "Gainup");
                            Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value = MyBase.GetServerDate();
                            Grid.CurrentCell = Grid["COMPLETE_DATE", 0];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            MessageBox.Show("Invalid Date Format, Pls Enter Date (DD/MM/YYYY) in This Format ", "Gainup");
                            Grid.CurrentCell = Grid["COMPLETE_DATE", 0];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            DataTable TmpDt = new DataTable();
                            MyBase.Load_Data("Select DateDiff(DD, '" + String.Format("{0:dd-MMM-yyyy}", Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", Grid["PLAN_DATE", Grid.CurrentCell.RowIndex].Value) + "' ) ", ref TmpDt);
                            //Grid["PLAN_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(DtpODate.Value).AddDays(Convert.ToInt32(Grid["LEAD_DAYS", Grid.CurrentCell.RowIndex].Value));
                            if (TmpDt.Rows.Count > 0)
                            {
                                Grid["DIFF_DAYS", Grid.CurrentCell.RowIndex].Value = TmpDt.Rows[0][0];
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
                TxtTotPro.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Count (ref Grid1,"DATE","REMARKS")));
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["COMPLETE_DATE"].Index)
                {
                    if (Grid["COMP_FLAG", Grid.CurrentCell.RowIndex].Value.ToString() == "Y")
                    {
                        MyBase.Valid_Date(Txt, e);
                    }
                    else
                    {
                        MyBase.Valid_Null(Txt, e);
                    }
                }
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["COMP_FLAG"].Index)
                {
                    MyBase.Valid_Yes_OR_No(Txt, e);
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

        private void FrmTimeActionCompleteEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {
                if ( this.ActiveControl.Name == "TxtDivision" || this.ActiveControl.Name == "TxtEmployee"  || this.ActiveControl.Name == "TxtOrderNo" || this.ActiveControl.Name == "TxtLeadDays" || this.ActiveControl.Name == "TxtTotPro")
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
                //if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                //{
                //    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                //    {                       
                //        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                //    }
                //}
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

             
    }
}
