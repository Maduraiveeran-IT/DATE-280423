using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounts; 
using Accounts_ControlModules;
using SelectionTool_NmSp;

namespace Accounts
{
    public partial class FrmTimeActionPending : Form 
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataRow Dr;
        TextBox Txt = null;       
        public Int32 code, EmplNo;
        String[] Queries;
        String EntryNo;
        String Body;
        String Sub;
        Int64 Code;
        public FrmTimeActionPending()
        {
            InitializeComponent();
        }

        private void FrmTimeActionPending_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                DataTable TDt = new DataTable();
                MyBase.Load_Data("Select (Isnull(Max(Entry_No), 0) + 1) No From Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master ", ref TDt);
                EntryNo = TDt.Rows[0][0].ToString();
                LblEntryNo.Text = "Entry No : " + EntryNo;
                SendKeys.Send("{F5}");
                button2_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmTimeActionPending_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {               
                if (e.KeyCode == Keys.Escape)
                {
                    MyBase.ActiveForm_Close(this, MyParent);
                }
                else if (e.KeyCode == Keys.F5)
                {
                    Grid_Data();
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.CurrentCell = Grid["COMP_FLAG", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                    }
                    else
                    {
                        this.Hide();
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
            try
            {
                MyBase.Clear(this);
                //Str = "Select  0 as SNO, C.Order_No ORDER_NO, F.Name ACTION_NAME, B.LEAD_DAYS, F.Follow_By MODE, B.PLAN_DATE, 'N' COMP_FLAG, Substring('" + MyBase.GetServerDate() + "',0,11) COMPLETE_DATE,  DateDiff(DD,Cast(GETDATE() as Date), B.PLAN_DATE) DIFF_DAYS, '-' REMARKS, 1 as T, B.ACTION_ID, C.RowID PLAN_ID From Socks_User_MAster A Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Details B On A.Emplno = B.EmplNo Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master C On B.Master_ID = C.Rowid Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master D On C.LeadTime_ID = D.RowID Inner Join Vaahini_Erp_Gainup.Dbo.Fit_Order_Master_Socks() E On C.Order_No = E.Order_No Inner Join Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master F On B.Action_ID = F.Rowid Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master G On G.Plan_ID = C.Rowid and G.EmplNo = B.EmplNo LEFT Join Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Details H On H.Action_ID = B.Action_ID and H.Complete_Flag = 'N' Where G.EmplNo Is Null and B.EmplNo = " + MyParent.EmplNo_TA  + " and C.Approval_Flag = 'F' Order by B.PLAN_DATE ";
                Str = "Select Distinct 0 as SNO, ORDER_NO, ACTION_NAME,  LEAD_DAYS , MODE, PLAN_DATE , 'N' COMP_FLAG, Substring('" + MyBase.GetServerDate() + "',0,11) COMPLETE_DATE, DIFF DIFF_DAYS, '-' REMARKS, 1 as T, ACTION_ID, PLAN_ID From  Vaahini_Erp_Gainup.Dbo.Time_Action_Fn(3) Where EmplNo = " + MyParent.EmplNo_TA + " and Complete_Flag = 'N' and Current_Status = 'P' and ACTION_ID != 51 and Approval_Flag = 'T' ORder By  Plan_Date Asc ";
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Designing(ref Grid, ref Dt, "ACTION_ID", "T", "PLAN_ID");
                MyBase.ReadOnly_Grid_Without(ref Grid, "COMP_FLAG", "COMPLETE_DATE", "REMARKS");
                MyBase.Grid_Width(ref Grid, 40, 110, 250, 80, 50, 100, 70, 100, 70, 350);
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["ORDER_NO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["ACTION_NAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["LEAD_DAYS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["PLAN_DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["MODE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["COMP_FLAG"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["COMPLETE_DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["DIFF_DAYS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //DataGridViewCheckBoxColumn CheckBoxColumn = new DataGridViewCheckBoxColumn();                               
                //CheckBoxColumn.Width = 80;
                //CheckBoxColumn.ValueType = typeof(String);
                //CheckBoxColumn.Visible = true;
                //CheckBoxColumn.ReadOnly = false;
                //CheckBoxColumn.Name = "STATUS";
                //CheckBoxColumn.HeaderText = "STATUS";
                //Grid.Columns.Insert(0, CheckBoxColumn);
                //Grid.Columns["STATUS"].DefaultCellStyle.BackColor = System.Drawing.Color.AliceBlue;
                //for (int i = 0; i < Grid.Rows.Count; i++)
                //{
                //    Grid["STATUS", i].Value = false;                    
                //} 
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
             
            }
            catch (Exception ex)
            {
                throw ex;
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
                Grid_Data();
                if (Grid.Rows.Count > 0)
                {
                    Grid.CurrentCell = Grid["COMP_FLAG", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmTimeActionPending_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 Array_Index = 0;
                Body = "";
                Sub = "";
                Total_Count();
                Queries = new String[Dt.Rows.Count + 10];
                DataTable TDt = new DataTable();
                MyBase.Load_Data("Select (Isnull(Max(Entry_No), 0) + 1) No From Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master ", ref TDt);
                EntryNo = TDt.Rows[0][0].ToString();
                                                        
                        Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Master (Entry_No,Effect_From, Plan_ID, EmplNo) Values (" + EntryNo + ", '" + String.Format("{0:dd-MMM-yyyy}", MyBase.GetServerDate()) + "', 0, " + MyParent.EmplNo_TA + " ) ; Select Scope_Identity()";
                        Queries[Array_Index++] = MyParent.EntryLog("TIME & ACTION COMPLETE", "ADD", "@@IDENTITY");
                 
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if ((Grid["COMP_FLAG", i].Value.ToString() == "N" && Grid["REMARKS", i].Value.ToString().Length > 2) || Grid["COMP_FLAG", i].Value.ToString() == "Y")
                        {
                            if (Grid["ACTION_ID", i].Value.ToString() != String.Empty && Grid["ACTION_ID", i].Value != DBNull.Value)
                            {
                                Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Time_Action_Complete_Details (Master_ID, SNo, Action_ID, Complete_Flag, Complete_Date, Remarks, Plan_ID_Dtl)  Values (@@IDENTITY," + Array_Index + " - 1, " + Grid["ACTION_ID", i].Value + ", '" + Grid["COMP_FLAG", i].Value + "', '" + String.Format("{0:dd-MMM-yyyy}", Grid["COMPLETE_DATE", i].Value) + "', '" + Grid["REMARKS", i].Value + "', " + Grid["PLAN_ID", i].Value + ")"; 
                            }
                        }
                    }
                    Sub = " Time & Action Complete :     Entry No :  " + EntryNo + "     Entry Date  : '" + String.Format("{0:dd-MMM-yyyy}", MyBase.GetServerDate()) + "'     ";
                    Body = " Entered By :  '" + MyParent.UserName + "'     Order No :    Lead Days :  " + Environment.NewLine;

                   
                        Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Auto_Mail_Send_TimeAction (Name, RowID, Body, Subject) Values ('COMPLETE', @@IDENTITY, '" + Body.Replace("'", "`") + "', '" + Sub.Replace("'", "`") + "')";

                        if (Array_Index > 3)
                        {
                            MyBase.Run_Identity(false, Queries);
                            MyParent.Save_Error = false;
                            MessageBox.Show("Saved ..!", "Gainup");
                            MyBase.Clear(this);
                            button2_Click(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("Invalid Details", "Gainup");
                            MyParent.Save_Error = true;
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
                    Txt.Leave += new EventHandler(Txt_Leave);
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
                        Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value = MyBase.GetServerDate();
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["REMARKS"].Index)
                {
                    if (Grid["REMARKS", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["REMARKS", Grid.CurrentCell.RowIndex].Value = "-";
                    }
                }
                Total_Count();

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
               
                Total_Count();             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void Total_Count()
        {
            Double Kgs = 0;
            try
            {
                TxtTotPro.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.CountWithCondtion(ref Grid, "COMP_FLAG", "COMP_FLAG", "Y", "COMPLETE_DATE")));               
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



    }
}