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
    public partial class FrmTimeActionSettingMaster : Form,Entry  
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataGrid dgv = new DataGrid();
        DataRow Dr;        
        Int64 Code;
        Double Kgs = 0;        
        TextBox Txt = null;        
        String[] Queries;
        String Str;
        Int32 B =0;
        Int16 PCompCode;
        public FrmTimeActionSettingMaster()
        {
            InitializeComponent();
        }
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                TxtDivision.Focus();
                Grid_Data();
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
                if (MyParent.UserName.Contains("ADMIN"))
                {
                    Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Master Setting-Edit", "Select  Distinct C.COMPANY Division, D.Lead_Time , A.Division_ID, A.LeadTime_ID  from Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master B On A.Action_ID = B.Rowid and A.Division_ID = B.Division_ID  Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() C On A.Division_ID = C.COMPCODE  Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master D On A.LeadTime_ID =  D.RowID   Order by A.Division_ID, D.Lead_Time ", string.Empty, 250, 150);
                }
                else
                {
                    Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Master Setting-Edit", "Select Distinct C.COMPANY Division, D.Lead_Time , A.Division_ID, A.LeadTime_ID  from Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master B On A.Action_ID = B.Rowid and A.Division_ID = B.Division_ID  Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() C On A.Division_ID = C.COMPCODE  Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master D On A.LeadTime_ID =  D.RowID  Where  A.Division_ID = (Case When '" + MyParent.UserName.Contains("GKA") + "' = 'true' Then 3 When '" + MyParent.UserName.Contains("GGA") + "' = 'true' then 4 Else 0 End) Order by A.Division_ID, D.Lead_Time ", string.Empty, 250, 150);
                }
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["ACTION_NAME", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
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
                Code = Convert.ToInt64(Dr["Division_Id"]);
                TxtDivision.Text = Dr["Division"].ToString();
                TxtDivision.Tag = Dr["Division_Id"].ToString();
                TxtLeadTime.Text = Dr["Lead_Time"].ToString();
                TxtLeadTime.Tag = Dr["LeadTime_ID"].ToString();
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
                if (TxtTotPro.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotPro.Text) == 0)
                {
                    MessageBox.Show("Invalid Lead Days", "Gainup");
                    TxtTotPro.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (Convert.ToDouble(TxtTotPro.Text) > Convert.ToDouble(TxtLeadTime.Text))
                {
                    MessageBox.Show("Lead Days Not Match", "Gainup");
                    TxtTotPro.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 3; j++)
                    {
                        if (Grid[j, i].Value == DBNull.Value)
                        {
                            MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                            Grid.CurrentCell = Grid[j, i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                }
                            
                Queries = new String[Dt.Rows.Count + 10];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master (Division_ID, Action_ID, Lead_Days, LeadTime_ID) Values (" + TxtDivision.Tag + " , " + Grid["ACTION_ID", i].Value + ", " + Grid["LEAD_DAYS", i].Value + ", " + TxtLeadTime.Tag + ");Select Scope_Identity()";                        
                    }
                    else
                    {
                        //t = new String[Grid.Rows.Count];
                        Int32 k = 0;
                        Int32 p = 0;
                        Int32 q = 0;
                        DataTable Tdtc = new DataTable();
                        MyBase.Load_Data("Select RowID  From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master Where Division_ID = " + TxtDivision.Tag + " ", ref Tdtc);
                        for (int j = 0; j < Grid.Rows.Count - 1; j++)
                        {
                            if (Grid["RowID", j].Value == DBNull.Value)
                            {
                                Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master (Division_ID, Action_ID, Lead_Days, LeadTime_ID) Values (" + TxtDivision.Tag + " , " + Grid["ACTION_ID", j].Value + ", " + Grid["LEAD_DAYS", j].Value + ", " + TxtLeadTime.Tag + ");Select Scope_Identity()";                                
                            }
                        }
                        p = Grid.Rows.Count;
                        q = Grid.Rows.Count;
                        for (int j = 0; j < p -1 ; j++)
                        {
                            if (j < q)
                            {
                                if ((Grid["RowID", j].Value) != DBNull.Value && (Grid["RowID", j].Value) != null)
                                {
                                    if (Convert.ToDouble(Grid["RowID", k].Value) == Convert.ToDouble(Tdtc.Rows[j][0]))
                                    {
                                        Queries[Array_Index++] = "Update Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master Set Division_ID = " + TxtDivision.Tag + " , Action_ID = " + Grid["ACTION_ID", k].Value + ", Lead_Days = " + Grid["LEAD_DAYS", k].Value + " , LeadTime_ID = " + TxtLeadTime.Tag + " Where Rowid = " + Grid["ROWID", k].Value + "";
                                        k = k + 1;
                                        p = p + 1;
                                    }                                    
                                }
                               
                                
                            }
                        }
                        if (listBox1.Items.Count > 0)
                        {
                            for (int l = 0; l < listBox1.Items.Count; l++)
                            {
                                Queries[Array_Index++] = " Delete From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master Where  Rowid = " + listBox1.Items[l] + "";
                            }
                        }                        
                        i = Dt.Rows.Count;
                    }
                    listBox1.Items.Clear();
                }               
                if (MyParent._New)
                {
                    Queries[Array_Index++] = MyParent.EntryLog("TIME & ACTION MASTER SETTING", "ADD", TxtDivision.Tag.ToString());
                    MyBase.Run_Identity(false, Queries);
                }
                else
                {
                    Queries[Array_Index++] = MyParent.EntryLog("TIME & ACTION MASTER SETTING", "EDIT", TxtDivision.Tag.ToString());
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
                if (MyParent.UserName.Contains("ADMIN"))
                {
                    Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Master Setting-Delete", "Select  Distinct C.COMPANY Division, D.Lead_Time , A.Division_ID, A.LeadTime_ID  from Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master B On A.Action_ID = B.Rowid and A.Division_ID = B.Division_ID  Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() C On A.Division_ID = C.COMPCODE  Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master D On A.LeadTime_ID =  D.RowID   Order by A.Division_ID, D.Lead_Time ", string.Empty, 250, 150);
                }
                else
                {
                    Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Master Setting-Delete", "Select Distinct C.COMPANY Division, D.Lead_Time , A.Division_ID, A.LeadTime_ID  from Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master B On A.Action_ID = B.Rowid and A.Division_ID = B.Division_ID  Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() C On A.Division_ID = C.COMPCODE  Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master D On A.LeadTime_ID =  D.RowID  Where  A.Division_ID = (Case When '" + MyParent.UserName.Contains("GKA") + "' = 'true' Then 3 When '" + MyParent.UserName.Contains("GGA") + "' = 'true' then 4 Else 0 End) Order by A.Division_ID, D.Lead_Time ", string.Empty, 250, 150);
                }
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
                    MyBase.Run("Delete From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master Where Division_ID = " + TxtDivision.Tag + "", MyParent.EntryLog("TIME & ACTION MASTER SETTING", "DELETE", TxtDivision.Tag.ToString()));                     
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
                if (MyParent.UserName.Contains("ADMIN"))
                {
                    Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Master Setting-View", "Select  Distinct C.COMPANY Division, D.Lead_Time , A.Division_ID, A.LeadTime_ID  from Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master B On A.Action_ID = B.Rowid and A.Division_ID = B.Division_ID  Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() C On A.Division_ID = C.COMPCODE  Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master D On A.LeadTime_ID =  D.RowID   Order by A.Division_ID, D.Lead_Time ", string.Empty, 250, 150);
                }
                else
                {
                    Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time & Action Master Setting-View", "Select Distinct C.COMPANY Division, D.Lead_Time , A.Division_ID, A.LeadTime_ID  from Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master B On A.Action_ID = B.Rowid and A.Division_ID = B.Division_ID  Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() C On A.Division_ID = C.COMPCODE  Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master D On A.LeadTime_ID =  D.RowID  Where  A.Division_ID = (Case When '" + MyParent.UserName.Contains("GKA") + "' = 'true' Then 3 When '" + MyParent.UserName.Contains("GGA") + "' = 'true' then 4 Else 0 End) Order by A.Division_ID, D.Lead_Time ", string.Empty, 250, 150);
                }
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

        void Grid_Data()
        {
            String Str = String.Empty;          
            try            
            {
                if (MyParent._New == true)
                {
                    Str = "Select Distinct 0 as SNO, '' ACTION_NAME , '' MODE, 0 LEAD_DAYS, 0 as DIVISION_ID, 0 as ACTION_ID, 0 Order_SlNo,  0 as RowID  From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master Where 1=2";
                }
                else
                {
                    Str = "Select 0 as SNO, B.Name ACTION_NAME, B.Follow_By MODE, A.Lead_Days LEAD_DAYS, A.Division_ID, A.Action_ID, B.Order_SlNo, A.RowID  from Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master B On A.Action_ID = B.Rowid and A.Division_ID = B.Division_ID  Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() C On A.Division_ID = C.COMPCODE Where A.Division_ID = " + TxtDivision.Tag + " and A.LEadTime_ID = " + TxtLeadTime.Tag + " ";                    
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);                               
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid, "ACTION_NAME", "LEAD_DAYS");  
                MyBase.Grid_Designing(ref Grid, ref Dt, "DIVISION_ID", "ACTION_ID", "RowID", "Order_SlNo");
                MyBase.Grid_Width(ref Grid, 50, 250, 80, 150);               
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["MODE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["ACTION_NAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["LEAD_DAYS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;               
            }
            catch (Exception ex)
            {
                throw ex;
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
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ACTION_NAME"].Index)
                        {
                            if (TxtDivision.Text.ToString() != String.Empty || TxtLeadTime.Text.ToString() != String.Empty )
                            {
                                Dr = Tool.Selection_Tool_Except_New("ACTION_NAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "ACTION_NAME", "Select  A.Order_SlNo, A.Name ACTION_NAME, A.Follow_By MODE, A.RowID From Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master A Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master B On A.Rowid = B.Action_ID and B.LeadTime_ID =  " + TxtLeadTime.Tag + " Where A.Division_ID = " + TxtDivision.Tag + "  and B.RowID is Null Order by A.ORder_Slno  ", string.Empty, 80, 200, 80);
                                if (Dr != null)
                                {
                                    Grid["ACTION_NAME", Grid.CurrentCell.RowIndex].Value = Dr["ACTION_NAME"].ToString();
                                    Grid["ACTION_ID", Grid.CurrentCell.RowIndex].Value = Dr["RowID"].ToString();
                                    Grid["MODE", Grid.CurrentCell.RowIndex].Value = Dr["MODE"].ToString();
                                    Grid["DIVISION_ID", Grid.CurrentCell.RowIndex].Value = TxtDivision.Tag.ToString();
                                    Grid["Order_SlNo", Grid.CurrentCell.RowIndex].Value = Dr["Order_SlNo"].ToString();
                                    Txt.Text = Dr["ACTION_NAME"].ToString();
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
            try
            {
                TxtTotPro.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Grid_Max(ref Grid, "LEAD_DAYS", "DIVISION_ID")));
                
                //TxtTotPro.Text = MyBase.Sum(ref Grid, "LEAD_DAYS", "DIVISION_ID");                                    
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["LEAD_DAYS"].Index)
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



        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["LEAD_DAYS"].Index)
                    {

                        if (Convert.ToDouble(Grid["LEAD_DAYS", Grid.CurrentCell.RowIndex].Value) < 0)
                            {
                                MessageBox.Show("Invalid LeadDays..!", "Gainup");
                                Grid["LEAD_DAYS", Grid.CurrentCell.RowIndex].Value = 0;                               
                                Grid.CurrentCell = Grid["LEAD_DAYS", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                e.Handled = true;
                                return;
                            }                       
                    }
                }
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmTimeActionSettingMaster_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                if (MyParent.CompCode == 1)
                {
                    PCompCode = 1;
                }
                else if (MyParent.CompCode == 2)
                {
                    PCompCode = 3;
                }
                listBox1.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmTimeActionSettingMaster_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {
                if (this.ActiveControl.Name != String.Empty)
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
            }
        }

        private void FrmTimeActionSettingMaster_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtDivision")
                    {
                        if (MyParent.UserName.Contains("GKA"))
                        {
                            Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Division", "Select COMPANY, COMPCODE  From Vaahini_Erp_Gainup.Dbo.Division_Mas () Where CompCode in (3) ORder by COMPCODE ", String.Empty, 400);
                        }
                        else if (MyParent.UserName.Contains("GGA"))
                        {
                            Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Division", "Select COMPANY, COMPCODE  From Vaahini_Erp_Gainup.Dbo.Division_Mas () Where CompCode in (3) ORder by COMPCODE ", String.Empty, 400);
                        }
                        else
                        {
                            Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Division", "Select COMPANY, COMPCODE  From Vaahini_Erp_Gainup.Dbo.Division_Mas ()  ORder by COMPCODE ", String.Empty, 400);
                        }
                       
                        if (Dr != null)
                        {
                            TxtDivision.Text = Dr["COMPANY"].ToString();
                            TxtDivision.Tag = Dr["COMPCODE"].ToString();
                            TxtDivision.Focus();
                            return;
                        }
                    }
                    if (this.ActiveControl.Name == "TxtLeadTime")
                    {
                        if (TxtDivision.Text.ToString() != String.Empty)
                        {
                            Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select LeadTime", "Select A.Lead_Time, A.RowID From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master A Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTime_Master B On A.RowID = B.LeadTime_ID and B.Division_ID = " + TxtDivision.Tag + " Where B.RowID Is Null ", String.Empty, 200);
                            if (Dr != null)
                            {
                                TxtLeadTime.Text = Dr["Lead_Time"].ToString();
                                TxtLeadTime.Tag = Dr["RowID"].ToString();
                                TxtLeadTime.Focus();
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
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtTotPro")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    if (this.ActiveControl.Name == "TxtLeadTime")
                    {
                        Grid.CurrentCell = Grid["ACTION_NAME", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                    SendKeys.Send("{Tab}");
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

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {     
                if (Grid.Rows.Count > 1 ) 
                {
                    MyBase.Row_Number(ref Grid);                                
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

        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {            
            try
            {
                if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        listBox1.Items.Add(Grid["RowID", Grid.CurrentCell.RowIndex].Value.ToString());
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }      
       
    }
}
