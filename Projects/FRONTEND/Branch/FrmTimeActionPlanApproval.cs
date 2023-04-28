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
    public partial class FrmTimeActionPlanApproval : Form
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
        String Body;
        String Sub;
        String[] t;
        Int16 PCompCode;
        public FrmTimeActionPlanApproval()
        {
            InitializeComponent();
        }                 

        
        void Fill_Datas(DataRow Dr)
        {
            try
            {
                Code = Convert.ToInt64(Dr["RowId"]);
                TxtEntryNo.Text = Dr["Entry_No"].ToString();
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
       

        
        private void FrmTimeActionPlanApproval_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                MyBase.Clear(this);
                TxtEntryNo.Focus();
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
            }
        }

        void Grid_Data()
        {
            String Str = String.Empty;          
            try
            {
                if(TxtEntryNo.Text.ToString() == String.Empty)
                {
                    if (TxtDivision.Text == String.Empty || TxtOrderNo.Text == String.Empty || TxtLeadDays.Text == String.Empty)
                    {
                        Str = "Select 0 as SNO, '' ACTION_NAME, '' as EMPLOYEE, 0 as LEAD_DAYS, '' MODE, Plan_Date PLAN_DATE, 0 ACTION_ID, 0 as EMPLNO, 0 as Order_SlNo, 0 as LEAD_DAYS1, 'N' EDIT, 'S' FOLLOW_BY  From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details Where 1 = 2 ";
                    }
                    else
                    {
                        Str = " Select 0 as SNO, B.Name ACTION_NAME, '' as EMPLOYEE, A.LEAD_DAYS, B.Follow_By MODE, (Case When B.Follow_By = 'S' Then DATEADD(DD, - A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "') Else DATEADD(DD,  A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "') End) PLAN_DATE, A.Action_ID, B.Order_SlNo, 0 as EMPLNO, A.LEAD_DAYS LEAD_DAYS1, B.Edit_Flag EDIT, B.FOLLOW_BY  From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master B On A.Action_ID = B.Rowid Where A.Division_ID = " + TxtDivision.Tag + " and A.LEadTime_ID = " + TxtLeadDays.Tag + " Order by B.Order_SlNo ";                        
                    }
                }
                else
                {
                    //Str = "Select B.SNO, C.Name ACTION_NAME, D.Name EMPLOYEE, B.LEAD_DAYS, C.Follow_By MODE, B.PLAN_DATE, B.Action_ID, C.Order_SlNo, B.EmplNo EMPLNO, B.LEAD_DAYS LEAD_DAYS1, C.Edit_Flag EDIT, C.FOLLOW_BY from Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details B On A.Rowid = B.Master_ID Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master C On B.Action_ID = C.Rowid Inner Join Vaahini_Erp_Gainup.Dbo.EmployeeMas D On B.EmplNo = D.Emplno Where B.Master_ID = " + Code + " ORder By B.SNo";                        
                    Str = "Select  SNO, C.Name ACTION_NAME, C.Follow_By MODE, D.Name EMPLOYEE, B.LEAD_DAYS, B.PLAN_DATE, B.WORK_DAYS, B.END_DATE, B.Action_ID, C.Order_SlNo, B.EmplNo EMPLNO, B.LEAD_DAYS LEAD_DAYS1, C.Edit_Flag EDIT, C.FOLLOW_BY from Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details B On A.Rowid = B.Master_ID Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master C On B.Action_ID = C.Rowid Inner Join Vaahini_Erp_Gainup.Dbo.EmployeeMas D On B.EmplNo = D.Emplno Where B.Master_ID = " + Code + " ORder By B.SNo";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid(ref Grid, "PLAN_DATE", "MODE", "END_DATE");
                MyBase.Grid_Designing(ref Grid, ref Dt, "ACTION_ID", "EMPLNO", "EDIT", "FOLLOW_BY", "Order_SlNo", "LEAD_DAYS1");
                MyBase.Grid_Width(ref Grid, 40, 220, 35, 110, 70, 90, 70, 90);
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


        private void FrmTimeActionPlanApproval_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtEntryNo")
                    {
                        TxtTotPro.Focus();
                        //Grid_Data();
                        //Grid.CurrentCell = Grid["ACTION_NAME", 0];
                        //Grid.Focus();
                        //Grid.BeginEdit(true);
                        //return;
                    }                   
                    else if (this.ActiveControl.Name == "TxtTotPro")
                    {
                        //if (MyParent._New == true || MyParent.Edit == true)
                       // {
                         //   MyParent.Load_SaveEntry();
                        ButApprove.Focus();
                            return;
                       // }
                    }
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtEntryNo")
                    {
                        //if (MyParent.UserName.Contains ("GKA"))
                        //{
                        //    Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Division", "Select COMPANY, COMPCODE  From Division_Mas () Where CompCode in (3) ORder by COMPCODE ", String.Empty, 400);
                        //}
                        //else
                        //{
                        //    Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Division", "Select COMPANY, COMPCODE  From Division_Mas () Where CompCode in (4) ORder by COMPCODE ", String.Empty, 400);
                        //}
                        //if (Dr != null)
                        //{
                        //    TxtDivision.Text = Dr["COMPANY"].ToString();
                        //    TxtDivision.Tag = Dr["COMPCODE"].ToString();
                        //    TxtOrderNo.Text = "";
                        //    TxtOrderNo.Tag = "";
                        //    TxtLeadDays.Text = "";
                        //    TxtDivision.Focus();
                        //    return;
                        //}

                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Plan - Approve", " Select  A.Order_No, E.BuyerName,  A.Effect_From , A.Entry_No,  B.COMPANY Division, C.Lead_Time LeadTime, A.LeadTime_ID, A.Division_ID, A.RowID, A.Alter_Order_Date Order_Date, A.Ship_Date From Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() B On A.Division_ID = B.COMPCODE Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master C On A.LeadTime_ID = C.RowID  Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master D On D.Plan_ID = A.Rowid Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Orders() E On A.ORder_NO = E.ORder_NO  Where D.Rowid Is Null and A.Division_ID =  4  and A.Approval_Flag = 'F'  ORder by A.Entry_No Desc ", string.Empty, 120, 160, 100, 80, 120, 80);
                        if (Dr != null)
                        {
                            Fill_Datas(Dr);
                            TxtEntryNo.Focus();
                        }
                        
                    }

                    if (this.ActiveControl.Name == "TxtOrderNo")
                    {
                        //if (TxtDivision.Text != String.Empty)
                        //{
                        //    Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select OrderNo", "Select Order_No, Order_Date, Ship_Date,  Tot_Days  From Time_Action_Orders() Where Ship_Date is not null and Division = " + TxtDivision.Tag + " Order By Order_No ", String.Empty, 140, 150, 150, 100);
                        //    if (Dr != null)
                        //    {
                        //        TxtOrderNo.Text = Dr["Order_No"].ToString();
                        //        DtpODate.Value = Convert.ToDateTime(Dr["Order_Date"].ToString());
                        //        DtpSDate.Value = Convert.ToDateTime(Dr["Ship_Date"].ToString());
                        //        TxtLeadDays.Text = "";
                        //        TxtOrderNo.Focus();
                        //        return;
                        //    }
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Invalid Division", "Gainup");
                        //    return;
                        //}
                    }

                    if (this.ActiveControl.Name == "TxtLeadDays")
                    {
                        //if (TxtOrderNo.Text.ToString() != String.Empty)
                        //{
                        //    Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select LeadDays", "Select A.Lead_Time, B.RowID LeadTime_ID From (Select MAX(A.Lead_Time) Lead_Time from Time_Action_LeadTimeDays_Master A Inner Join Time_Action_LeadTime_Master B On A.RowID = B.LeadTime_ID  Where A.Lead_Time <= DateDiff(DD,'" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "') Union Select Min(A.Lead_Time) Lead_Time from Time_Action_LeadTimeDays_Master A Inner Join Time_Action_LeadTime_Master B On A.RowID = B.LeadTime_ID Where A.Lead_Time >= DateDiff(DD,'" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpSDate.Value) + "') ) A Inner Join Time_Action_LeadTimeDays_Master B On A.Lead_Time = B.Lead_Time ", String.Empty, 150);                            
                        //    if (Dr != null)
                        //    {
                        //        TxtLeadDays.Text = Dr["Lead_Time"].ToString();
                        //        TxtLeadDays.Tag  = Dr["LeadTime_ID"].ToString();                                
                        //        TxtLeadDays.Focus();
                        //        return;
                        //    }
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Invalid OrderNo", "Gainup");
                        //    return;
                        //}
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
                MyBase.Show(ex.Message, this);
            }
        }

        private void Grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt == null)
                {
                    Txt = (TextBox)e.Control;
                    //Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                    Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);                    
                    //Txt.Leave +=new EventHandler(Txt_Leave);
                }
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
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
                            MyBase.Load_Data("Select DateAdd(D, " + Convert.ToInt32(Grid["LEAD_DAYS", Grid.CurrentCell.RowIndex].Value) + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "' ) ", ref TmpDt);
                        }
                        //Grid["PLAN_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(DtpODate.Value).AddDays(Convert.ToInt32(Grid["LEAD_DAYS", Grid.CurrentCell.RowIndex].Value));
                        if (TmpDt.Rows.Count > 0)
                        {
                            Grid["PLAN_DATE", Grid.CurrentCell.RowIndex].Value = TmpDt.Rows[0][0];
                        }
                        

                    }
                }
                catch (Exception ex)
                {
                    MyBase.Show(ex.Message, this);
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
                            Dr = Tool.Selection_Tool_Except_New("ACTION_NAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "ACTION_NAME", " Select B.Name ACTION_NAME, A.LEAD_DAYS, B.Follow_By MODE, DATEADD(DD, A.Lead_Days,  '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "') PLAN_DATE, A.Action_ID, B.Order_SlNo, B.Edit_Flag   From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master B On A.Action_ID = B.Rowid Where A.Division_ID = " + TxtDivision.Tag + " and A.LEadTime_ID = " + TxtLeadDays.Tag + " Order by B.Order_SlNo ", String.Empty, 200);                           
                            if (Dr != null)
                            {
                                Grid["ACTION_NAME", Grid.CurrentCell.RowIndex].Value = Dr["ACTION_NAME"].ToString();
                                Grid["ACTION_ID", Grid.CurrentCell.RowIndex].Value = Dr["ACTION_ID"].ToString();
                                Grid["LEAD_DAYS", Grid.CurrentCell.RowIndex].Value = Dr["LEAD_DAYS"].ToString();
                                Grid["MODE", Grid.CurrentCell.RowIndex].Value = Dr["MODE"].ToString();
                                Grid["PLAN_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Dr["PLAN_DATE"].ToString());
                                Grid["EDIT", Grid.CurrentCell.RowIndex].Value = Dr["Edit_Flag"].ToString();
                                Grid["Order_SlNo", Grid.CurrentCell.RowIndex].Value = Dr["Order_SlNo"].ToString();
                                Txt.Text = Dr["ACTION_NAME"].ToString();
                                return;
                            }
                        }
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["EMPLOYEE"].Index)
                        {
                            //Dr = Tool.Selection_Tool_Except_New("EMPLNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "EMPLOYEE NAME", " select Distinct B.Name, B.Tno, A.EMPLNO  From Pay_Att A Left Join EmployeeMas B On A.emplno = B.Emplno where cast(etime as date)= '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "' Order By Name ", String.Empty, 200);
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "EMPLOYEE NAME", " Select B.Name , B.tno TNo, B.DeptName, B.DesignationName, A.EMPLNO From Vaahini_Erp_Gainup.Dbo.Floor_User_Master  A Inner Join Vaahini_Erp_Gainup.Dbo.MIS_Employee_Basic () B On A.Emplno = B.Emplno Where B.tno Not Like '%z' and B.Tno Not Like (Case When '" + MyParent.UserName.Contains("GKA") + "' = 'true' Then 'GGA%' Else 'GKA%' End)  Order By B.Name ", String.Empty, 200, 120, 150, 150);
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
                MyBase.Show(ex.Message, this);
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
                MyBase.Valid_Null(Txt, e);
                //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["LEAD_DAYS"].Index)
                //{
                //    if (Grid["EDIT", Grid.CurrentCell.RowIndex].Value.ToString() == "Y" && Grid["EMPLNO", Grid.CurrentCell.RowIndex].Value.ToString() != "0" )
                //    {
                //        MyBase.Valid_Number(Txt, e);
                //    }
                //    else
                //    {
                //        MyBase.Valid_Null(Txt, e);
                //    }
                //}
                //else
                //{
                //    MyBase.Valid_Null(Txt, e);
                //}
                Total_Count();
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
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
                MyBase.Show(ex.Message, this);
            }
        }

        private void FrmTimeActionPlanApproval_KeyPress(object sender, KeyPressEventArgs e)
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
              //  MyBase.Row_Number(ref Grid);
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
            }
        }


        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //try
            //{
            //    if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
            //    {
            //        if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            //        {                       
            //            Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MyBase.Show(ex.Message, this);
            //}
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
                MyBase.Show(ex.Message, this);
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
                MyBase.Show(ex.Message, this);
            }
        }

        private void ButApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyParent.Is_This_Previous_Year ())
                {
                    MessageBox.Show("You Can't Save Previous Year ...!", "Gainup");
                    return;
                }

                if (MessageBox.Show("Sure to Approve ..!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                if (TxtEntryNo.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Entry", "Gainup");
                    return;
                }
                String Str = "Update Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master Set  Approval_Flag = 'T' Where Rowid = " + Code; 
                MyBase.Run(Str);
                MessageBox.Show("Approved", "Gainup");
                MyBase.Clear(this);
                TxtEntryNo.Focus();
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
            }
        }

        private void ButClear_Click(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                TxtEntryNo.Focus();
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
            }

        }

        private void ButExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
            }

        }              
    }
}
