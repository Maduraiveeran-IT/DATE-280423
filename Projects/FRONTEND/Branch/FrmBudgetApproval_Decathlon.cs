using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Windows.Forms;
using Accounts;

namespace Accounts
{
    public partial class FrmBudgetApproval_Decathlon : Form
    {
        SelectionTool_Class Tool = new SelectionTool_Class();
        Control_Modules Mybase = new Control_Modules();
        MDIMain MyParent;
        DataTable Dt = new DataTable();
        DataRow Dr;
        CheckBox Chk = null;
        Boolean Status_Flag = false;
        DataTable[] Dt_OCN_List;
        String OCN_List = String.Empty;
        String Item_List = String.Empty;
        DataTable Dt_Sum = new DataTable();
        DataTable Dt_Budget = new DataTable();
        DataTable Dt_Budget_Qty = new DataTable();
        DataTable Dt_Final = new DataTable();

        public FrmBudgetApproval_Decathlon()
        {
            InitializeComponent();
        }

        private void FrmBudgetApproval_Decathlon_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.F5)
                {
                    TxtBuyer.Focus();
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtBuyer")
                    {
                        if (MyParent.UserName.ToString() == "MD" || MyParent.UserName.ToString() == "ADMIN" || MyParent.UserName.ToString() == "GKA0081" || MyParent.UserName.ToString() == "GKA0312")
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer", "Select Distinct Party Buyer, BuyerID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Party Like 'Decathlon%' And BuyerID in (5275, 5465) Order by Party ", String.Empty, 350);
                        }
                        //else if (MyParent.UserName.ToString() == "GKA0081")
                        //{
                        //    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer", "Select Distinct A.Party Buyer , A.BuyerID, C.Acc_Empl_ID EmplNo  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() A Inner Join FitSocks.Dbo.Socks_Order_Master B On A.OrdeR_ID = B.RowID Inner Join FitSocks.Dbo.Employee C On B.Empl_ID = C.employeeid  Where A.BuyerID in (5275 ,5465) Order by A.PArty", String.Empty, 350);
                        //}
                        else
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer", "Select Distinct A.Party Buyer , A.BuyerID, C.Acc_Empl_ID EmplNo  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() A Inner Join FitSocks.Dbo.Socks_Order_Master B On A.OrdeR_ID = B.RowID Inner Join FitSocks.Dbo.Employee C On B.Empl_ID = C.employeeid  Where C.Acc_Empl_ID = " + MyParent.Emplno + " Order by A.PArty", String.Empty, 350);
                        }

                        if (Dr != null)
                        {
                            TxtBuyer.Text = Dr["Buyer"].ToString();
                            TxtBuyer.Tag = Dr["BuyerID"].ToString();

                            Grid_Data();

                            if (Dt.Rows.Count > 0)
                            {
                                Grid.CurrentCell = Grid["Status", 0];
                                Grid.Focus();
                            }
                        }
                    }
                }
                else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
                {
                    e.Handled = true;
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
                //Str = "Select A.Order_No, A.Item Style, A.Buy_Qty  Qty, Max(B.Ship_Date) Ship_Date, B.Currency , C1.Sale_Price_Exp Price, (Case When D.RowID Is Null Then 'False' Else 'True' End) TimeAction   From FitSocks.Dbo.Socks_Bom_Item_Fn() A Inner Join FitSocks.Dbo.Socks_Order_Fn() B On A.Order_ID = B.RowID and A.ItemID = B.ItemID Inner Join FitSocks.Dbo.Socks_Planning_Master C On A.Order_ID = C.Order_ID and A.ItemID = C.Item_ID Inner Join FitSocks.Dbo.Socks_Planning_Summary_Details C1 On C.RowID = C1.Master_ID  Left Join Vaahini_ERp_Gainup.Dbo.Time_Action_Plan_Master D On D.Order_No = B.Order_No Left Join FitSocks.Dbo.Job_Order_Taken_Order_Nos()E On B.Order_No = E.Order_No Where E.Order_No Is Null And A.Party_Code = " + TxtBuyer.Tag.ToString() + " Group by A.Order_No, A.Item, A.Buy_Qty , B.Currency, C1.Sale_Price_Exp, D.RowId Order by A.Order_No Desc";
                Str = "Select A.Order_No, A.Item Style, A.Buy_Qty  Qty, Max(B.Ship_Date) Ship_Date, B.Currency , C1.Sale_Price_Exp Price, (Case When D.RowID Is Null Then 'False' Else 'True' End) TimeAction From FitSocks.Dbo.Socks_Bom_Item_Fn() A Inner Join FitSocks.Dbo.Socks_Order_Fn() B On A.Order_ID = B.RowID and A.ItemID = B.ItemID Inner Join FitSocks.Dbo.Socks_Planning_Master C On A.Order_ID = C.Order_ID and A.ItemID = C.Item_ID Inner Join FitSocks.Dbo.Socks_Planning_Summary_Details C1 On C.RowID = C1.Master_ID Left Join Vaahini_ERp_Gainup.Dbo.Time_Action_Plan_Master D On D.Order_No = B.Order_No Left Join FitSocks.Dbo.Job_Order_Taken_Order_Nos()E On B.Order_No = E.Order_No Inner Join FitSocks.Dbo.Repeat_Order_Ratewise()F On B.Order_No = F.Order_No Where E.Order_No Is Null And A.Party_Code = " + TxtBuyer.Tag.ToString() + " Group by A.Order_No, A.Item, A.Buy_Qty , B.Currency, C1.Sale_Price_Exp, D.RowId Order by A.Order_No Desc ";
                Grid.DataSource = Mybase.Load_Data(Str, ref Dt);

                Mybase.ReadOnly_Grid_Without(ref Grid);

                if (Status_Flag)
                {
                    Grid.Columns.Remove("Status");
                    DataGridViewCheckBoxColumn Check = new DataGridViewCheckBoxColumn();
                    Check.HeaderText = "STATUS";
                    Check.Name = "STATUS";
                    Check.ValueType = typeof(String);
                    Check.Visible = true;
                    Check.ReadOnly = false;
                    Grid.Columns.Insert(0, Check);
                    Status_Flag = true;
                }
                else
                {
                    DataGridViewCheckBoxColumn Check = new DataGridViewCheckBoxColumn();
                    Check.HeaderText = "STATUS";
                    Check.Name = "STATUS";
                    Check.ValueType = typeof(String);
                    Check.Visible = true;
                    Check.ReadOnly = false;
                    Grid.Columns.Insert(0, Check);
                    Status_Flag = true;
                }

                Mybase.Grid_Designing(ref Grid, ref Dt);
                Mybase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);

                Grid.Columns["TimeAction"].HeaderText = "T&A";
                Mybase.Grid_Width(ref Grid, 80, 120, 200, 90, 100, 100, 90, 100);

                Grid.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Currency"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                //  Mybase.Grid_Designing(ref Grid, ref Dt, "TA");
                Grid.Columns["Status"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;

                Grid.RowHeadersWidth = 10;

                TxtOrders.Text = Dt.Rows.Count.ToString();

                for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                {
                    if (Grid["TimeAction", i].Value.ToString() == "False")
                    {
                        Grid.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmBudgetApproval_Decathlon_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Fill_Existing_Approval()
        {
            DataTable TempDt = new DataTable();
            try
            {
                Mybase.Load_Data("Select Order_No, O_Slno, ProcessID, ItemID, ColorID, Sizeid, PlanDtlID, PlanMasID From Vaahini_ERP_Gainup.Dbo.Budget_Approval_Socks_New ()  Where Order_No in (" + OCN_List + ") and App_Qty > 0 Order By O_Slno, ProcessID, ItemID", ref TempDt);
                //Mybase.Load_Data("Select Order_No, O_Slno, ProcessID, ItemID, ColorID, Sizeid From Vaahini_ERP_Gainup.Dbo.Budget_Approval_Socks () Where Order_No in (" + OCN_List + ") and App_Qty > 0 Order By O_Slno, ProcessID, ItemID", ref TempDt);
                for (int i = 0; i <= TempDt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Dt_Budget.Rows.Count - 1; j++)
                    {
                        //if (Grid_Budget["O_Slno", j].Value.ToString() == TempDt.Rows[i]["O_Slno"].ToString() && Grid_Budget["ProcessID", j].Value.ToString() == TempDt.Rows[i]["ProcessID"].ToString() && Grid_Budget["ItemID", j].Value.ToString() == TempDt.Rows[i]["ItemID"].ToString() && Grid_Budget["ColorID", j].Value.ToString() == TempDt.Rows[i]["ColorID"].ToString() && Grid_Budget["SizeID", j].Value.ToString() == TempDt.Rows[i]["SizeID"].ToString() && Grid_Budget["PlanDtlID", j].Value.ToString() == TempDt.Rows[i]["PlanDtlID"].ToString() && Grid_Budget["PlanMasID", j].Value.ToString() == TempDt.Rows[i]["PlanMasID"].ToString())
                        if (Grid_Budget["O_Slno", j].Value.ToString() == TempDt.Rows[i]["O_Slno"].ToString() && Grid_Budget["ProcessID", j].Value.ToString() == TempDt.Rows[i]["ProcessID"].ToString() && Grid_Budget["ItemID", j].Value.ToString() == TempDt.Rows[i]["ItemID"].ToString() && Grid_Budget["ColorID", j].Value.ToString() == TempDt.Rows[i]["ColorID"].ToString() && Grid_Budget["SizeID", j].Value.ToString() == TempDt.Rows[i]["SizeID"].ToString())
                        {
                            for (int k = 8; k <= Grid_Budget.Columns.Count - 1; k++)
                            {
                                if (Grid_Budget.Columns[k].Name == TempDt.Rows[i]["Order_No"].ToString())
                                {
                                    Grid_Budget[k, j].Style.BackColor = Color.Yellow;
                                    Grid_Budget[k, j].Style.ForeColor = Color.Black;
                                    Grid_Budget[k, j].Style.SelectionBackColor = Color.Yellow;
                                    Grid_Budget[k, j].Style.SelectionForeColor = Color.Black;
                                }
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

        private void FrmBudgetApproval_Decathlon_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                Mybase.Clear(this);
                if (MyParent.UserName == "MD" || MyParent.UserName == "ADMIN" || MyParent.UserName == "GKA0081" || MyParent.UserName == "GKA0312")
                {
                    button8.Visible = true;
                }
                else
                {
                    button8.Visible = false;
                }
                TxtBuyer.Focus();
                SendKeys.Send("{F5}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Summary()
        {
            try
            {
                Mybase.Load_Data("SELECT 0 AS SLNO, (Case When O_Slno = 1 Then 0 Else O_Slno End) O_Slno, (Case When Replace(Replace(Replace(Replace(Access_Type,'*',''), '- Add', ''), '- Rep', ''), '- Others', '') = 'Yarn Dyeing' Then 'Process' Else Replace(Replace(Replace(Replace(Access_Type,'*',''), '- Add', ''), '- Rep', ''), '- Others', '') End) Access, Cast(0 as Numeric (25, 2)) PREVIOUS, Cast(0 as Numeric (25, 2)) as NOW_, Cast(0 as Numeric (25, 2)) AMOUNT FROM Vaahini_ERP_Gainup.DBo.Budget_Approval_Summary_Socks_New() WHERE ORDER_NO in (" + OCN_List + ") Group By (Case When O_Slno = 1 Then 0 Else O_Slno End),    (Case When Replace(Replace(Replace(Replace(Access_Type,'*',''), '- Add', ''), '- Rep', ''), '- Others', '') = 'Yarn Dyeing' Then 'Process' Else Replace(Replace(Replace(Replace(Access_Type,'*',''), '- Add', ''), '- Rep', ''), '- Others', '') End)    Order By (Case When O_Slno = 1 Then 0 Else O_Slno End)", ref Dt_Sum);
                //SELECT 0 AS SLNO, (Case When O_Slno = 1 Then 0 Else O_Slno End) O_Slno, (Case When Replace(Replace(Replace(Replace(Access_Type,'*',''), '- Add', ''), '- Rep', ''), '- Others', '') = 'Yarn Dyeing' Then 'Process' Else Replace(Replace(Replace(Replace(Access_Type,'*',''), '- Add', ''), '- Rep', ''), '- Others', '') End) Access, Cast(0 as Numeric (25, 2)) PREVIOUS, Cast(0 as Numeric (25, 2)) as NOW_, Cast(0 as Numeric (25, 2)) AMOUNT FROM Vaahini_ERP_Gainup.DBo.Budget_Approval_Summary_Socks_New() WHERE ORDER_NO in ('GUP-OCN01910') Group By (Case When O_Slno = 1 Then 0 Else O_Slno End),    (Case When Replace(Replace(Replace(Replace(Access_Type,'*',''), '- Add', ''), '- Rep', ''), '- Others', '') = 'Yarn Dyeing' Then 'Process' Else Replace(Replace(Replace(Replace(Access_Type,'*',''), '- Add', ''), '- Rep', ''), '- Others', '') End)    Order By (Case When O_Slno = 1 Then 0 Else O_Slno End)
                //Mybase.Load_Data("SELECT 0 AS SLNO, (Case When O_Slno = 1 Then 0 Else O_Slno End) O_Slno, (Case When Replace(Access_Type,'*','') = 'Yarn Dyeing' Then 'Process' Else Replace(Access_Type,'*','') End) Access, Cast(0 as Numeric (25, 2)) PREVIOUS, Cast(0 as Numeric (25, 2)) as NOW_, Cast(0 as Numeric (25, 2)) AMOUNT FROM Vaahini_ERP_Gainup.DBo.Budget_Approval_Summary_Socks_New() WHERE ORDER_NO in (" + OCN_List + ") Group By (Case When O_Slno = 1 Then 0 Else O_Slno End), (Case When Replace(Access_Type,'*','') = 'Yarn Dyeing' Then 'Process' Else Replace(Access_Type,'*','') End) Order By (Case When O_Slno = 1 Then 0 Else O_Slno End)", ref Dt_Sum);                
                Grid_Sum.DataSource = Mybase.V_DataTable(ref Dt_Sum);
                Mybase.Grid_Designing(ref Grid_Sum, ref Dt_Sum, "O_Slno");
                Mybase.ReadOnly_Grid_Without(ref Grid_Sum);
                Mybase.Grid_Colouring(ref Grid_Sum, Control_Modules.Grid_Design_Mode.Column_Wise);
                Mybase.Grid_Width(ref Grid_Sum, 60, 200, 130, 130, 130);

                Mybase.Row_Number(ref Grid_Sum);

                Grid_Sum.Columns["NOW_"].HeaderText = "NOW";
                Grid_Sum.Columns["PREVIOUS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Sum.Columns["PREVIOUS"].DefaultCellStyle.Format = "n";

                Grid_Sum.Columns["NOW_"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Sum.Columns["NOW_"].DefaultCellStyle.Format = "n";

                Grid_Sum.Columns["amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Sum.Columns["amount"].DefaultCellStyle.Format = "n";

                Grid_Sum.RowHeadersWidth = 10;

                Mybase.V_DataGridView(ref Grid_Sum);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Mark_Deviation()
        {
            try
            {
                for (int i = 0; i <= Dt_Budget.Rows.Count - 1; i++)
                {
                    for (int j = 10; j <= Dt_Budget.Columns.Count - 1; j++)
                    {
                        for (int k = j; k <= Dt_Budget.Columns.Count - 1; k++)
                        {
                            if (Convert.ToDouble(Grid_Budget[j, i].Value) != 0 && Convert.ToDouble(Grid_Budget[k, i].Value) != 0)
                            {
                                if (Convert.ToDouble(Grid_Budget[j, i].Value) != Convert.ToDouble(Grid_Budget[k, i].Value))
                                {
                                    if (Grid_Budget[9, i].Value.ToString().Contains("**") == true)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Grid_Budget[9, i].Value = Grid_Budget[9, i].Value.ToString() + " ** ";
                                        break;
                                    }
                                }
                            }
                        }

                    }
                }



                //for (int i = 0; i <= Dt_Budget.Rows.Count - 1; i++)
                //{
                //    for (int j = Dt_Budget.Columns.Count -1; j >= 11; j--)
                //    {                        
                //                if (Convert.ToDouble(Grid_Budget[j, i].Value) != Convert.ToDouble(Grid_Budget[j - 1, i].Value))
                //                {
                //                    Grid_Budget[9, i].Value = Grid_Budget[9, i].Value.ToString() + " ** ";
                //                    break;
                //                }                     
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Grid_Final_Fn()
        {
            try
            {
                Mybase.Load_Data("SELECT 0 as SLNO, ORDER_NO, Ord_Qty QTY, Unit_Price PRICE_INR, Unit_Price_INR  PRICE_CUR, Ex_Rate EX_RATE, CAST(0 AS NUMERIC (25, 2)) AP_AMOUNT, CAST(0 AS NUMERIC (25, 2)) AP_PRICE, CAST(0 AS NUMERIC (25, 2)) AP_CUR, CAST(0 AS NUMERIC (25, 2)) PROFIT_INR, CAST(0 AS NUMERIC (25, 4)) PROFIT_CUR, CAST(0 AS NUMERIC (25, 2)) PROFIT_PER FROM Vaahini_ERP_Gainup.DBo.Basic_Order_Details_Socks_New() WHERE ORDER_NO IN (" + OCN_List + ")", ref Dt_Final);
                //Mybase.Load_Data("SELECT 0 as SLNO, ORDER_NO, Ord_Qty QTY, Unit_Price_INR PRICE_INR, Unit_Price PRICE_CUR, Ex_Rate EX_RATE, CAST(0 AS NUMERIC (25, 2)) AP_AMOUNT, CAST(0 AS NUMERIC (25, 2)) AP_PRICE, CAST(0 AS NUMERIC (25, 2)) PROFIT_INR, CAST(0 AS NUMERIC (25, 4)) PROFIT_CUR, CAST(0 AS NUMERIC (25, 2)) PROFIT_PER FROM Vaahini_ERP_Gainup.DBo.Basic_Order_Details_Socks() WHERE ORDER_NO IN (" + OCN_List + ")", ref Dt_Final);
                Grid_Final.DataSource = Mybase.V_DataTable(ref Dt_Final);
                Mybase.Grid_Designing(ref Grid_Final, ref Dt_Final);
                Mybase.ReadOnly_Grid_Without(ref Grid_Final);
                Mybase.Grid_Colouring(ref Grid_Final, Control_Modules.Grid_Design_Mode.Column_Wise);
                Mybase.Grid_Width(ref Grid_Final, 60, 120, 80, 80, 80, 80, 130, 80, 80, 80, 80, 80);
                Grid_Final.RowHeadersWidth = 10;

                Grid_Final.Columns["PRICE_CUR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Final.Columns["PRICE_INR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid_Final.Columns["PROFIT_INR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Final.Columns["PROFIT_CUR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid_Final.Columns["PROFIT_PER"].HeaderText = "%";
                Grid_Final.Columns["PROFIT_PER"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid_Final.Columns["PRICE_CUR"].DefaultCellStyle.Format = "0.0000";
                Grid_Final.Columns["PROFIT_CUR"].DefaultCellStyle.Format = "0.0000";
                Grid_Final.Columns["AP_PRICE"].DefaultCellStyle.Format = "0.0000";
                Grid_Final.Columns["AP_CUR"].DefaultCellStyle.Format = "0.0000";

                Grid_Final.Columns["PROFIT_CUR"].HeaderText = "PROF_CUR";
                Grid_Final.Columns["PROFIT_INR"].HeaderText = "PROF_INR";

                Mybase.Row_Number(ref Grid_Final);

                Mybase.V_DataGridView(ref Grid_Final);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Fill_Ocn_Amount()
        {
            Double Amount = 0;
            try
            {
                for (int i = 0; i <= Grid_Final.Rows.Count - 3; i++)
                {
                    Amount = 0;
                    for (int j = 0; j <= Grid_Budget.Columns.Count - 1; j++)
                    {
                        if (Grid_Final["Order_No", i].Value.ToString() == Grid_Budget.Columns[j].Name.ToString())
                        {
                            for (int k = 0; k <= Grid_Budget.Rows.Count - 1; k++)
                            {
                                if (Grid_Budget[j, k].Style.BackColor == System.Drawing.Color.Green || Grid_Budget[j, k].Style.BackColor == System.Drawing.Color.Yellow)
                                {
                                    Amount += Convert.ToDouble(Dt_Budget.Rows[k][j]) * Convert.ToDouble(Dt_Budget_Qty.Rows[k][j]);
                                }
                            }

                            Grid_Final["Ap_Amount", i].Value = Amount;
                            Grid_Final["Ap_Price", i].Value = (Convert.ToDouble(Amount / Convert.ToDouble(Grid_Final["Qty", i].Value)));


                            Grid_Final["Profit_INR", i].Value = Math.Round(Convert.ToDouble(Grid_Final["PRICE_INR", i].Value) - Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDouble(Grid_Final["AP_Price", i].Value))), 2);

                            Grid_Final["Profit_CUR", i].Value = Convert.ToDouble(Grid_Final["Profit_INR", i].Value) / Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDouble(Grid_Final["EX_RATE", i].Value)));

                            Grid_Final["Profit_Per", i].Value = (Convert.ToDouble(Grid_Final["PROFIT_INR", i].Value) / Convert.ToDouble(Grid_Final["PRICE_INR", i].Value)) * 100;

                            Grid_Final["Ap_Cur", i].Value = (Convert.ToDouble(Grid_Final["Ap_Price", i].Value) / Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDouble(Grid_Final["EX_RATE", i].Value))));


                        }
                    }
                }

                Grid_Final["ap_amount", Grid_Final.Rows.Count - 1].Value = Mybase.Sum(ref Grid_Final, "Ap_Amount", "Order_no", "Qty");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Int32 OCN_Count = 0;
            String ToolTip1 = String.Empty;
            try
            {

                OCN_List = String.Empty;
                Item_List = String.Empty;
                OCN_Count = Grid_CurrentCellChanged();

                if (TxtBuyer.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Buyer ...!", "Gainup");
                    TxtBuyer.Focus();
                    return;
                }

                if (OCN_Count == 0)
                {
                    MessageBox.Show("Select OCN No's ....!", "Gainup");
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    return;
                }


                if (MessageBox.Show(Grid_CurrentCellChanged().ToString() + " Order's Selected. Sure to Continue ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }


                Grid_Budget.DataSource = null;
                Dt_Budget = new DataTable();
                Dt_Budget_Qty = new DataTable();




                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Status", i].Value != null && Grid["Status", i].Value != DBNull.Value && Grid["Status", i].Value.ToString().ToUpper() == "true".ToUpper() && Grid["TimeAction", i].Value.ToString().ToUpper() == "True".ToUpper())
                    {
                        if (OCN_List.Trim() == String.Empty)
                        {
                            OCN_List = "'" + Grid["Order_no", i].Value.ToString() + "'";
                        }
                        else
                        {
                            OCN_List += ", '" + Grid["Order_no", i].Value.ToString() + "'";
                        }
                        if (Item_List.Trim() == String.Empty)
                        {
                            Item_List = "'" + Grid["Style", i].Value.ToString() + "'";
                        }
                        else
                        {
                            Item_List += ", '" + Grid["Style", i].Value.ToString() + "'";
                        }

                    }
                }

                if (OCN_List == String.Empty)
                {
                    MessageBox.Show("Time & Action Entry Not Available For This Order....!", "Gainup");
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                String Query = String.Empty;

                // Create Cursor for Ocn_List - QTY
                Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (MAX) = ''; Declare @Ocn_No_List_Act Varchar (MAX) = ''; Declare @Ocn_No_List_SQ Varchar (MAX) = ''; Declare @Result as NVarchar (MAX); Declare C1_OCn Cursor For Select Distinct Order_No From FITSocks.Dbo.Socks_Order_Fn() Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, Sum(quantity) quantity  From Vaahini_ERP_Gainup.DBo.Budget_Approval_Socks_New() where order_no IN (' + @Ocn_No_List_SQ + ') Group by Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size) A1 PIVOT (sum(quantity) for order_no in (' + @Ocn_No_List_Act + ')) A '; Exec SP_ExecuteSql @Result;";
                Mybase.Load_Data(Query, ref Dt_Budget_Qty);


                // Create Cursor for Ocn_List - RATE
                Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (4000) = ''; Declare @Ocn_No_List_Act Varchar (MAX) = ''; Declare @Ocn_No_List_SQ Varchar (MAX) = ''; Declare @Result as NVarchar (MAX); Declare C1_OCn Cursor For Select Distinct Order_No From FITSocks.Dbo.Socks_Order_Fn() Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, Rate From Vaahini_ERP_Gainup.DBo.Budget_Approval_Socks_New() where order_no IN (' + @Ocn_No_List_SQ + ') Group by Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, Rate) A1 PIVOT (Sum(RATE) for order_no in (' + @Ocn_No_List_Act + ')) A '; Exec SP_ExecuteSql @Result;";

                //// Create Cursor for Ocn_List - QTY
                //Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (4000) = ''; Declare @Ocn_No_List_Act Varchar (4000) = ''; Declare @Ocn_No_List_SQ Varchar (4000) = ''; Declare @Result as NVarchar (4000); Declare C1_OCn Cursor For Select Distinct Order_No From FITSocks.Dbo.Buy_ord_mas Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                //Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, quantity From Vaahini_ERP_Gainup.DBo.Budget_Approval_Socks () where order_no IN (' + @Ocn_No_List_SQ + ')) A1 PIVOT (sum(quantity) for order_no in (' + @Ocn_No_List_Act + ')) A '; Exec SP_ExecuteSql @Result;";
                //Mybase.Load_Data(Query, ref Dt_Budget_Qty);


                //// Create Cursor for Ocn_List - RATE
                //Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (4000) = ''; Declare @Ocn_No_List_Act Varchar (4000) = ''; Declare @Ocn_No_List_SQ Varchar (4000) = ''; Declare @Result as NVarchar (4000); Declare C1_OCn Cursor For Select Distinct Order_No From FITSocks.Dbo.Buy_ord_mas Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                //Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, Rate From Vaahini_ERP_Gainup.DBo.Budget_Approval_Socks () where order_no IN (' + @Ocn_No_List_SQ + ')) A1 PIVOT (MIN(RATE) for order_no in (' + @Ocn_No_List_Act + ')) A '; Exec SP_ExecuteSql @Result;";
                Grid_Budget.DataSource = Mybase.Load_Data(Query, ref Dt_Budget);


                Mybase.Grid_Designing(ref Grid_Budget, ref Dt_Budget, "O_Slno", "ProcessID", "ItemID", "ColorID", "SizeID");
                //Mybase.Grid_Colouring(ref Grid_Budget, Control_Modules.Grid_Design_Mode.Column_Wise);
                Mybase.Grid_Width(ref Grid_Budget, 120, 120, 250, 80, 90, 100);
                Mybase.ReadOnly_Grid_Without(ref Grid_Budget);

                Grid_Budget.RowHeadersWidth = 10;

                tabControl1.SelectTab(TabDetails);

                Grid_Budget.Refresh();

                for (int i = Dt_Budget.Rows.Count - 1; i >= 1; i--)
                {
                    if (Grid_Budget["Access", i].Value.ToString() == Grid_Budget["Access", i - 1].Value.ToString())
                    {
                        Grid_Budget["Access", i].Value = String.Empty;
                    }
                }

                for (int i = Dt_Budget.Rows.Count - 1; i >= 1; i--)
                {
                    if (Grid_Budget["Process", i].Value.ToString() == Grid_Budget["Process", i - 1].Value.ToString())
                    {
                        Grid_Budget["Process", i].Value = String.Empty;
                    }
                }


                for (int i = 0; i <= Grid_Budget.Columns.Count - 1; i++)
                {
                    if (i < 10)
                    {
                        Grid_Budget.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                    }
                    else
                    {
                        Grid_Budget.Columns[i].HeaderText = Convert.ToInt32(Grid_Budget.Columns[i].Name.Substring(7, 5)).ToString();
                        Grid_Budget.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }


                Grid_Budget.CurrentCell = Grid_Budget["ACCESS", 0];
                Grid_Budget.Focus();

                Mybase.Grid_Freeze(ref Grid_Budget, Control_Modules.FreezeBY.Column_Wise, 9);

                Fill_Existing_Approval();  // Fill Yellow
                Mark_Deviation();          // ** Deviation 
                Summary();                 // Summary Tab
                Grid_Final_Fn();           // Grid Final Profit

                this.Cursor = Cursors.Default;


            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        void Fill_Approved_Amount()
        {
            try
            {
                for (int i = 0; i <= Dt_Sum.Rows.Count - 1; i++)
                {

                    Grid_Sum["Now_", i].Value = "0.00";
                    Grid_Sum["Previous", i].Value = "0.00";

                    for (int j = 0; j <= Dt_Budget.Rows.Count - 1; j++)
                    {
                        if (Dt_Sum.Rows[i]["O_SLno"].ToString() == Dt_Budget.Rows[j]["O_SLno"].ToString())
                        {
                            for (int k = 10; k <= Dt_Budget.Columns.Count - 1; k++)
                            {
                                if (Grid_Budget[k, j].Style.BackColor == System.Drawing.Color.Green)
                                {
                                    Grid_Sum["Now_", i].Value = Convert.ToDouble(Grid_Sum["Now_", i].Value) + (Convert.ToDouble(Dt_Budget.Rows[j][k]) * Convert.ToDouble(Dt_Budget_Qty.Rows[j][k]));
                                }
                                else if (Grid_Budget[k, j].Style.BackColor == System.Drawing.Color.Yellow)
                                {
                                    Grid_Sum["Previous", i].Value = Convert.ToDouble(Grid_Sum["Previous", i].Value) + (Convert.ToDouble(Dt_Budget.Rows[j][k]) * Convert.ToDouble(Dt_Budget_Qty.Rows[j][k]));
                                }
                            }
                        }
                    }
                    Grid_Sum["Amount", i].Value = Convert.ToDouble(Grid_Sum["Previous", i].Value) + Convert.ToDouble(Grid_Sum["Now_", i].Value);
                }

                Grid_Sum["Previous", Grid_Sum.Rows.Count - 1].Value = Mybase.Sum(ref Grid_Sum, "Previous", "Access", "Previous");
                Grid_Sum["Now_", Grid_Sum.Rows.Count - 1].Value = Mybase.Sum(ref Grid_Sum, "Now_", "Access", "Previous");
                Grid_Sum["Amount", Grid_Sum.Rows.Count - 1].Value = Mybase.Sum(ref Grid_Sum, "Amount", "Access", "Previous");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void Grid_Ocn()
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
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Grid["STATUS", i].Value = checkBox1.Checked;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Grid["TimeAction", Grid.CurrentCell.RowIndex].Value.ToString() == "False")
                {
                    Grid["STATUS", Grid.CurrentCell.RowIndex].Value = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        Boolean Grid_Satus()
        {
            try
            {
                if (Grid.Columns["Status"].HeaderText.Length > 0)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        Int32 Grid_CurrentCellChanged()
        {
            int Times = 0;
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Status", i].Value != null && Grid["Status", i].Value != DBNull.Value && Grid["Status", i].Value.ToString().ToUpper() == "true".ToUpper())
                    {
                        Times++;
                    }
                }

                return Times;
            }
            catch (Exception ex)
            {
                return Times;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab == TabDetails || tabControl1.SelectedTab == TabAverage)
                {
                    if (Grid_Budget.DataSource == null)
                    {
                        tabControl1.SelectTab(TabOcnList);
                    }
                    else
                    {
                        if (tabControl1.SelectedTab == TabAverage)
                        {
                            Fill_Approved_Amount();
                            Fill_Ocn_Amount();

                            Grid_Sum["Slno", Grid_Sum.Rows.Count - 1].Value = DBNull.Value;
                            Grid_Sum["Slno", Grid_Sum.Rows.Count - 2].Value = DBNull.Value;

                            Grid_Final["Slno", Grid_Final.Rows.Count - 1].Value = DBNull.Value;
                            Grid_Final["Slno", Grid_Final.Rows.Count - 2].Value = DBNull.Value;

                            Grid_Sum.Rows[Grid_Sum.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                            Grid_Final.Rows[Grid_Final.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;


                        }
                    }
                }
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
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Budget_Click(object sender, EventArgs e)
        {

        }

        private void Grid_Budget_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataTable Tdt = new DataTable();
            String ToolTip1 = String.Empty;
            try
            {
                if (Grid_Budget.CurrentCell.ColumnIndex > 9)
                {
                    label4.Text = String.Format("{0:0.000}", Convert.ToDouble(Dt_Budget_Qty.Rows[Grid_Budget.CurrentCell.RowIndex][Grid_Budget.CurrentCell.ColumnIndex]));
                }
                else
                {
                    label4.Text = String.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Budget_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {   
                int Col = Grid_Budget.HitTest(e.X, e.Y).ColumnIndex;
                int Row = Grid_Budget.HitTest(e.X, e.Y).RowIndex;
                if (Col > 9 && Row >= 0)
                {
                    Grid_Budget.CurrentCell = Grid_Budget[Col, Row];
                    Grid_Budget.Focus();

                    if (e.Button == MouseButtons.Right)
                    {
                        ContextMenuStrip Cm = new ContextMenuStrip();

                        if (Grid_Budget.CurrentCell.Style.BackColor == Color.Green || Grid_Budget.CurrentCell.Style.BackColor == Color.Yellow)
                        {
                            ToolStripMenuItem NotOk = new ToolStripMenuItem("Not Ok");
                            NotOk.Name = "NotOk";
                            NotOk.Click += new EventHandler(NotOk_Click);

                            ToolStripMenuItem DProcess = new ToolStripMenuItem("DeSelect Process");
                            DProcess.Name = "DProcess";
                            DProcess.Click += new EventHandler(DProcess_Click);


                            ToolStripMenuItem DRowAll = new ToolStripMenuItem("DeSelect All - Row");
                            DRowAll.Name = "DRowAll";
                            DRowAll.Click += new EventHandler(DRowAll_Click);

                            ToolStripMenuItem DColAll = new ToolStripMenuItem("DeSelect All - Col");
                            DColAll.Name = "DColAll";
                            DColAll.Click += new EventHandler(DColAll_Click);

                            Cm.Items.Add(NotOk);
                            Cm.Items.Add(DProcess);
                            Cm.Items.Add(DRowAll);
                            Cm.Items.Add(DColAll);
                        }
                        else
                        {
                            ToolStripMenuItem Ok = new ToolStripMenuItem("Ok");
                            Ok.Name = "Ok";
                            Ok.Click += new EventHandler(Ok_Click);

                            ToolStripMenuItem Process = new ToolStripMenuItem("Select Process");
                            Process.Name = "Process";
                            Process.Click += new EventHandler(Process_Click);

                            ToolStripMenuItem RowAll = new ToolStripMenuItem("Select All - Row");
                            RowAll.Name = "RowAll";
                            RowAll.Click += new EventHandler(RowAll_Click);

                            ToolStripMenuItem ColAll = new ToolStripMenuItem("Select All - Col");
                            ColAll.Name = "ColAll";
                            ColAll.Click += new EventHandler(ColAll_Click);

                            Cm.Items.Add(Ok);
                            Cm.Items.Add(Process);
                            Cm.Items.Add(RowAll);
                            Cm.Items.Add(ColAll);
                        }

                        Cm.Show(Grid_Budget, new Point(e.X, e.Y));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void DProcess_Click(object sender, EventArgs e)
        {
            int Pos = 0;
            try
            {
                for (int i = Grid_Budget.CurrentCell.RowIndex; i >= 0; i--)
                {
                    if (Grid_Budget["Process", i].Value.ToString() != String.Empty)
                    {
                        Pos = i;
                        break;
                    }
                }

                for (int i = Pos; i <= Grid_Budget.Rows.Count - 1; i++)
                {
                    if (Grid_Budget["Process", i].Value.ToString() == String.Empty || i == Pos)
                    {
                        Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.BackColor = Color.White;
                        Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.ForeColor = Color.Black;

                        Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.SelectionBackColor = Grid_Budget[0, 0].Style.SelectionBackColor;
                        Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.SelectionForeColor = Grid_Budget[0, 0].Style.SelectionForeColor;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Process_Click(object sender, EventArgs e)
        {
            int Pos = 0;
            try
            {
                for (int i = Grid_Budget.CurrentCell.RowIndex; i >= 0; i--)
                {
                    if (Grid_Budget["Process", i].Value.ToString() != String.Empty)
                    {
                        Pos = i;
                        break;
                    }
                }

                for (int i = Pos; i <= Grid_Budget.Rows.Count - 1; i++)
                {
                    if (Grid_Budget["Process", i].Value.ToString() == String.Empty || i == Pos)
                    {
                        if (Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.BackColor != Color.Yellow)
                        {
                            Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.BackColor = Color.Green;
                            Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.ForeColor = Color.White;

                            Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.SelectionBackColor = Color.Green;
                            Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.SelectionForeColor = Color.White;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void DColAll_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= Grid_Budget.Rows.Count - 1; i++)
                {
                    Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.BackColor = Color.White;
                    Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.ForeColor = Color.Black;

                    Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.SelectionBackColor = Grid_Budget[0, 0].Style.SelectionBackColor;
                    Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.SelectionForeColor = Grid_Budget[0, 0].Style.SelectionForeColor;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void DRowAll_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 10; i <= Grid_Budget.Columns.Count - 1; i++)
                {
                    Grid_Budget[i, Grid_Budget.CurrentCell.RowIndex].Style.BackColor = Color.White;
                    Grid_Budget[i, Grid_Budget.CurrentCell.RowIndex].Style.ForeColor = Color.Black;

                    Grid_Budget[i, Grid_Budget.CurrentCell.RowIndex].Style.SelectionBackColor = Grid_Budget[0, 0].Style.SelectionBackColor;
                    Grid_Budget[i, Grid_Budget.CurrentCell.RowIndex].Style.SelectionForeColor = Grid_Budget[0, 0].Style.SelectionForeColor;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void ColAll_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= Grid_Budget.Rows.Count - 1; i++)
                {
                    if (Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.BackColor != Color.Yellow)
                    {
                        Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.BackColor = Color.Green;
                        Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.ForeColor = Color.White;

                        Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.SelectionBackColor = Color.Green;
                        Grid_Budget[Grid_Budget.CurrentCell.ColumnIndex, i].Style.SelectionForeColor = Color.White;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void RowAll_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 10; i <= Grid_Budget.Columns.Count - 1; i++)
                {
                    if (Grid_Budget[i, Grid_Budget.CurrentCell.RowIndex].Style.BackColor != Color.Yellow)
                    {
                        Grid_Budget[i, Grid_Budget.CurrentCell.RowIndex].Style.BackColor = Color.Green;
                        Grid_Budget[i, Grid_Budget.CurrentCell.RowIndex].Style.ForeColor = Color.White;

                        Grid_Budget[i, Grid_Budget.CurrentCell.RowIndex].Style.SelectionBackColor = Color.Green;
                        Grid_Budget[i, Grid_Budget.CurrentCell.RowIndex].Style.SelectionForeColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void NotOk_Click(object sender, EventArgs e)
        {
            try
            {
                Grid_Budget.CurrentCell.Style.BackColor = Color.White;
                Grid_Budget.CurrentCell.Style.ForeColor = Color.Black;

                Grid_Budget.CurrentCell.Style.SelectionBackColor = Grid_Budget[0, 0].Style.SelectionBackColor;
                Grid_Budget.CurrentCell.Style.SelectionForeColor = Grid_Budget[0, 0].Style.SelectionForeColor;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Ok_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Budget.CurrentCell.Style.BackColor != Color.Yellow)
                {
                    Grid_Budget.CurrentCell.Style.BackColor = Color.Green;
                    Grid_Budget.CurrentCell.Style.ForeColor = Color.White;

                    Grid_Budget.CurrentCell.Style.SelectionBackColor = Color.Green;
                    Grid_Budget.CurrentCell.Style.SelectionForeColor = Color.White;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectTab(TabOcnList);
                TxtBuyer.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Grid_Budget.DataSource = null;
                Grid_Sum.DataSource = null;
                Grid.DataSource = null;
                Grid_Final.DataSource = null;
                TxtBuyer.Text = String.Empty;
                TxtBuyer.Tag = String.Empty;
                TxtOrders.Text = String.Empty;
                TxtBuyer.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectTab(TabDetails);
                Grid_Budget.CurrentCell = Grid_Budget["Access", 0];
                Grid_Budget.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyParent.UserName == "MD" || MyParent.UserName == "ADMIN" || MyParent.UserName == "GKA0081" || MyParent.UserName == "GKA0312")
                {

                }
                else
                {
                    MessageBox.Show("Invalid Option", "Gainup");
                    return;
                }
                if (TxtBuyer.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Buyer ...!", "Gainup");
                    tabControl1.SelectTab(TabOcnList);
                    TxtBuyer.Focus();
                    return;
                }

                if (OCN_List.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid OCN List ...!", "Gainup");
                    tabControl1.SelectTab(TabOcnList);
                    Grid.CurrentCell = Grid["Style", 0];
                    Grid.Focus();
                    return;
                }

                if (Dt_Budget.Rows.Count == 0 || Dt_Budget_Qty.Rows.Count == 0 || Dt_Final.Rows.Count == 0 || Dt_Sum.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Grid Details ...!", "Gainup");
                    tabControl1.SelectTab(TabDetails);
                    Grid_Budget.CurrentCell = Grid_Budget["Access", 0];
                    Grid_Budget.Focus();
                    return;
                }


                if (MessageBox.Show("Sure to Approve ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }

                Boolean For_Break = false;

                for (int i = 0; i <= Dt_Budget.Rows.Count - 1; i++)
                {
                    if (Grid_Budget["Size", i].Value.ToString().Contains("**"))
                    {
                        if (!For_Break)
                        {
                            if (MessageBox.Show("** Cases available. Sure to Continue ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                            {
                                Grid_Budget.CurrentCell = Grid_Budget["Size", i];
                                Grid_Budget.Focus();
                                For_Break = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                if (For_Break)
                {
                    tabControl1.SelectTab(TabDetails);
                    return;
                }


                this.Cursor = Cursors.WaitCursor;

                Int32 Rows = (Dt_Budget.Rows.Count * (Dt_Budget.Columns.Count - 8)) * 3;

                String[] Queries = new string[Rows];
                Int32 Array_Index = 0;


                for (int i = 0; i <= Dt_Budget.Rows.Count - 1; i++)
                {
                    for (int k = 10; k <= Dt_Budget.Columns.Count - 1; k++)
                    {
                        if (Convert.ToInt32(Dt_Budget.Rows[i]["O_Slno"]) == 0)
                        {
                            if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                            {
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Spl_Req_Mode = 'F' and Dye_Mode = 'N' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                            }
                            else if (Grid_Budget[k, i].Style.BackColor != Color.Yellow)
                            {
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Spl_Req_mode = 'F' and Dye_Mode = 'N' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                            }
                        }
                        else if (Convert.ToInt32(Dt_Budget.Rows[i]["O_Slno"]) == 3)
                        {
                            if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                            {
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag_Sample = 'T' , Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name(), Approval_Flag_Dye = (CAse When Dyeing_Mode = 'Y' Then 'T' Else 'F' End) Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Access_Type = '" + Dt_Budget.Rows[i]["Access"].ToString() + "' and  Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and (Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " Or ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + ") and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                //if(Dt_Budget.Rows[i]["Access"].ToString().Contains("Others"))
                                //{
                                //  Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name(), Approval_Flag_Dye = (CAse When Dyeing_Mode = 'Y' Then 'T' Else 'F' End) Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Access_Type = 'OTHERS' and  Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and (Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " Or ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + ") and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                //}
                                //else if(Dt_Budget.Rows[i]["Access"].ToString().Contains("Rep"))
                                //{
                                //  Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name(), Approval_Flag_Dye = (CAse When Dyeing_Mode = 'Y' Then 'T' Else 'F' End) Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Access_Type = 'REPLACE' and Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and (Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " Or ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + ") and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                //}
                                //else if(Dt_Budget.Rows[i]["Access"].ToString().Contains("Add"))
                                //{
                                //  Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name(), Approval_Flag_Dye = (CAse When Dyeing_Mode = 'Y' Then 'T' Else 'F' End) Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Access_Type = 'EXCESS' and Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and (Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " Or ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + ") and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                //}
                                //else
                                //{
                                //  Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name(), Approval_Flag_Dye = (CAse When Dyeing_Mode = 'Y' Then 'T' Else 'F' End) Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and (Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " Or ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + ") and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                //}
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                            }
                            else if (Grid_Budget[k, i].Style.BackColor != Color.Yellow)
                            {
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag_Sample = 'F' , Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null, Approval_Flag_Dye = 'F'  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Access_Type = '" + Dt_Budget.Rows[i]["Access"].ToString() + "' and Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and (Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " Or ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + ") and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                //if(Dt_Budget.Rows[i]["Access"].ToString().Contains("Others"))
                                //{
                                //    Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null, Approval_Flag_Dye = 'F'  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Access_Type = 'OTHERS' and Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and (Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " Or ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + ") and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";                              
                                //}
                                //else if(Dt_Budget.Rows[i]["Access"].ToString().Contains("Rep"))
                                //{
                                //    Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null, Approval_Flag_Dye = 'F'  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Access_Type = 'REPLACE' and Spl_REq_Mode = 'T'  and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and (Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " Or ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + ") and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";                              
                                //}
                                //else if(Dt_Budget.Rows[i]["Access"].ToString().Contains("Add"))
                                //{
                                //    Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null, Approval_Flag_Dye = 'F'  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Access_Type = 'EXCESS' and Spl_REq_Mode = 'T'  and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and (Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " Or ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + ") and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";                              
                                //}
                                //else 
                                //{
                                //    Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null, Approval_Flag_Dye = 'F'  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and (Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " Or ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + ") and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";                              
                                //}

                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                            }
                        }
                        else if (Convert.ToInt32(Dt_Budget.Rows[i]["O_Slno"]) == 1)
                        {
                            if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                            {
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                            }
                            else if (Grid_Budget[k, i].Style.BackColor != Color.Yellow)
                            {
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                            }
                        }
                        else if (Convert.ToDouble(Dt_Budget.Rows[i]["O_Slno"]) == 2)
                        {
                            if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                            {
                                //Queries[Array_Index++] = "Insert Into FitSocks.Dbo.Socks_Trims_BOM_Status (Planning_Master_ID, Planning_Detail_ID, Order_ID, Item_ID, Color_ID, Size_ID, BOM, Dyeing_Status)  Select A.RowID, A.TrimDtlID, A.OrdeR_ID, A.Item_ID, A.COLOR_ID, A.SIZE_ID, A.Tot_Qty , 'N' DYE_MODE From FitSocks.Dbo.Socks_Trim_Planning_Fn() A Left Join FitSocks.Dbo.Socks_Trims_BOM_Status B On A.TrimDtlID = B.Planning_Detail_ID and A.OrdeR_ID = B.Order_ID and A.Item_ID = B.Item_ID and A.SIZE_ID = B.Size_ID and A.COLOR_ID = B.Color_ID Where  A.Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and A.Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and A.Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and A.Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + " and B.Planning_Master_ID IS null";
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Trim_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where RoWID In (Select Distinct TrimDtlID  From FitSocks.Dbo.Socks_Trim_Planning_Fn() Where   Access_Type != 'Special' and Plan_Type != 'M' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                            }
                            else if (Grid_Budget[k, i].Style.BackColor != Color.Yellow)
                            {
                                // Queries[Array_Index++] = "Delete From FitSocks.Dbo.Socks_Trims_BOM_Status  Where Planning_Detail_ID  In (Select Distinct TrimDtlID  From FitSocks.Dbo.Socks_Trim_Planning_Fn() Where Access_Type != 'Special' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Trim_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null   Where RoWID In (Select Distinct TrimDtlID  From FitSocks.Dbo.Socks_Trim_Planning_Fn() Where Access_Type != 'Special' and Plan_Type != 'M' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                            }
                        }
                        else if (Convert.ToDouble(Dt_Budget.Rows[i]["O_Slno"]) == 2.2)
                        {
                            if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                            {
                                // Queries[Array_Index++] = "Insert Into FitSocks.Dbo.Socks_Trims_BOM_Status (Planning_Master_ID, Planning_Detail_ID, Order_ID, Item_ID, Color_ID, Size_ID, BOM, Dyeing_Status)  Select A.RowID, A.TrimDtlID, A.OrdeR_ID, A.Item_ID, A.COLOR_ID, A.SIZE_ID, A.Tot_Qty , 'N' DYE_MODE From FitSocks.Dbo.Socks_Trim_Planning_Fn() A Left Join FitSocks.Dbo.Socks_Trims_BOM_Status B On A.TrimDtlID = B.Planning_Detail_ID and A.OrdeR_ID = B.Order_ID and A.Item_ID = B.Item_ID and A.SIZE_ID = B.Size_ID and A.COLOR_ID = B.Color_ID Where  A.Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and A.Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and A.Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and A.Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + " and B.Planning_Master_ID IS null";
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Trim_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where RoWID In (Select Distinct TrimDtlID  From FitSocks.Dbo.Socks_Trim_Planning_Fn() Where Access_Type != 'Special' and Plan_Type = 'M' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                            }
                            else if (Grid_Budget[k, i].Style.BackColor != Color.Yellow)
                            {
                                //  Queries[Array_Index++] = "Delete From FitSocks.Dbo.Socks_Trims_BOM_Status  Where Planning_Detail_ID  In (Select Distinct TrimDtlID  From FitSocks.Dbo.Socks_Trim_Planning_Fn() Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Trim_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null   Where RoWID In (Select Distinct TrimDtlID  From FitSocks.Dbo.Socks_Trim_Planning_Fn() Where Access_Type != 'Special' and Plan_Type = 'M' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                            }
                        }
                        else if (Convert.ToDouble(Dt_Budget.Rows[i]["O_Slno"]) == 2.1)
                        {
                            if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                            {
                                // Queries[Array_Index++] = "Insert Into FitSocks.Dbo.Socks_Trims_BOM_Status (Planning_Master_ID, Planning_Detail_ID, Order_ID, Item_ID, Color_ID, Size_ID, BOM, Dyeing_Status)  Select A.RowID, A.TrimDtlID, A.OrdeR_ID, A.Item_ID, A.COLOR_ID, A.SIZE_ID, A.Tot_Qty , 'N' DYE_MODE From FitSocks.Dbo.Socks_Trim_Planning_Fn() A Left Join FitSocks.Dbo.Socks_Trims_BOM_Status B On A.TrimDtlID = B.Planning_Detail_ID and A.OrdeR_ID = B.Order_ID and A.Item_ID = B.Item_ID and A.SIZE_ID = B.Size_ID and A.COLOR_ID = B.Color_ID Where  A.Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and A.Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and A.Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and A.Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + " and B.Planning_Master_ID IS null";
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Trim_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where RoWID In (Select Distinct TrimDtlID  From FitSocks.Dbo.Socks_Trim_Planning_Fn() Where Access_Type = 'Special' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                            }
                            else if (Grid_Budget[k, i].Style.BackColor != Color.Yellow)
                            {
                                //  Queries[Array_Index++] = "Delete From FitSocks.Dbo.Socks_Trims_BOM_Status  Where Planning_Detail_ID  In (Select Distinct TrimDtlID  From FitSocks.Dbo.Socks_Trim_Planning_Fn() Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Trim_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null   Where RoWID In (Select Distinct TrimDtlID  From FitSocks.Dbo.Socks_Trim_Planning_Fn() Where Access_Type = 'Special' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                            }
                        }
                        else if (Convert.ToInt32(Dt_Budget.Rows[i]["O_Slno"]) == 4)
                        {
                            if (Dt_Budget.Rows[i]["ProcessID"].ToString() == "158")
                            {
                                if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                                {
                                    Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag_Dye = 'T' , Approval_Time_Dye = Getdate(), Approval_System_Dye = Host_Name() Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Dye_Mode = 'Y' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                    Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                                }
                                else if (Grid_Budget[k, i].Style.BackColor != Color.Yellow)
                                {
                                    Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag_Dye = 'F' , Approval_Time_Dye = Null, Approval_System_Dye = Null  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Dye_Mode = 'Y' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                    Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                                }
                            }
                            else if (Dt_Budget.Rows[i]["ProcessID"].ToString() != "158")
                            {
                                if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                                {
                                    Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Proc_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where  RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Process_Planning_Fn() Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Proc_ID = " + Grid_Budget["ProcessID", i].Value.ToString() + " and Sample_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                    Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                                }
                                else if (Grid_Budget[k, i].Style.BackColor != Color.Yellow)
                                {
                                    Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Proc_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null  Where  RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Process_Planning_Fn() Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Proc_ID = " + Grid_Budget["ProcessID", i].Value.ToString() + " and Sample_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                    Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                                }
                            }

                        }
                        else if (Convert.ToInt32(Dt_Budget.Rows[i]["O_Slno"]) == 7)
                        {
                            if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                            {
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Commercial_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where  RoWID In (Select Distinct CommDtlID  From FitSocks.Dbo.Socks_Commercial_Planning_Fn() Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Comm_ID = " + Grid_Budget["ProcessID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                            }
                            else if (Grid_Budget[k, i].Style.BackColor != Color.Yellow)
                            {
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Commercial_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null  Where  RoWID In (Select Distinct CommDtlID  From FitSocks.Dbo.Socks_Commercial_Planning_Fn() Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Comm_ID = " + Grid_Budget["ProcessID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                            }
                        }
                        else
                        {
                            if (Grid_Budget[k, i].Style.BackColor == Color.Green || Grid_Budget[k, i].Style.BackColor == Color.Yellow)
                            {
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                            }
                            else
                            {
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                            }
                        }
                    }
                }     

                for (int p = 10; p <= Dt_Budget.Columns.Count - 1; p++)
                {
                    Queries[Array_Index++] = "Exec FitSocks.Dbo.Socks_Yarn_Planning_Import_Proc '" + Grid_Budget.Columns[p].Name.ToString() + "'";
                    Queries[Array_Index++] = "Exec FitSocks.Dbo.Socks_Trim_Planning_Import_Proc '" + Grid_Budget.Columns[p].Name.ToString() + "'";
                    Queries[Array_Index++] = "Exec FitSocks.Dbo.Socks_Process_Planning_Import_Proc '" + Grid_Budget.Columns[p].Name.ToString() + "'";
                    Queries[Array_Index++] = "Exec FitSocks.Dbo.Socks_Yarn_Status_Budget '" + Grid_Budget.Columns[p].Name.ToString() + "'";
                    Queries[Array_Index++] = "Exec Vaahini_Erp_Gainup.Dbo.Time_Action_Auto_Save_Budget_Socks_Proc '" + Grid_Budget.Columns[p].Name.ToString() + "'";
                }

                Mybase.Run(Queries);
                this.Cursor = Cursors.Default;
                MessageBox.Show("Saved ...!", "Gainup");
                button6_Click(sender, e);
                tabControl1.SelectTab(TabOcnList);
                TxtBuyer.Focus();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void TabAverage_Click(object sender, EventArgs e)
        {

        }


    }
}
