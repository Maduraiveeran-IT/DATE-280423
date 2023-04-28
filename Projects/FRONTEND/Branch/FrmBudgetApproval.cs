using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmBudgetApproval : Form
    {

        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        Control_Modules Mybase = new Control_Modules();
        DataTable Dt = new DataTable();
        DataRow Dr;
        CheckBox Chk = null;
        Boolean Status_Flag = false;
        DataTable[] Dt_OCN_List;
        String OCN_List = String.Empty;
        DataTable Dt_Sum = new DataTable();
        DataTable Dt_Budget = new DataTable();
        DataTable Dt_Budget_Qty = new DataTable();
        DataTable Dt_Final = new DataTable();

        public FrmBudgetApproval()
        {
            InitializeComponent();
        }

        private void FrmBudgetApproval_KeyDown(object sender, KeyEventArgs e)
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
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer", "Select Distinct Buyer, BuyerID From Vaahini_ERP_Gainup.dbo.Basic_Order_Details_Socks()", String.Empty, 350);
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
                Str = "Select Distinct B1.ORDER_NO, B1.STYLE, B1.SHIP_DATE, B1.ORD_QTY QTY, B1.CURRENCY, B1.Unit_Price PRICE From Vaahini_ERP_Gainup.DBo.Basic_Order_Details_Socks() B1 Left join FitSocks.DBo.Fit_Order_Status F1 on B1.Order_No = F1.Order_No Where B1.buyerid = " + TxtBuyer.Tag.ToString() + " and F1.Order_No is null order by B1.Order_No Desc";
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
                

                Mybase.Grid_Width(ref Grid, 80, 120, 200, 90, 100, 100, 90);

                Grid.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Currency"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid.Columns["Status"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;

                Grid.RowHeadersWidth = 10;

                TxtOrders.Text = Dt.Rows.Count.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmBudgetApproval_KeyPress(object sender, KeyPressEventArgs e)
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
                Mybase.Load_Data("Select Order_No, O_Slno, ProcessID, ItemID, ColorID, Sizeid From Vaahini_ERP_Gainup.Dbo.Budget_Approval_Socks () Where Order_No in (" + OCN_List + ") and App_Qty > 0 Order By O_Slno, ProcessID, ItemID", ref TempDt);
                for (int i = 0; i <= TempDt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Dt_Budget.Rows.Count - 1; j++)
                    {
                        if (Grid_Budget["O_Slno", j].Value.ToString() == TempDt.Rows[i]["O_Slno"].ToString() && Grid_Budget["ProcessID", j].Value.ToString() == TempDt.Rows[i]["ProcessID"].ToString() && Grid_Budget["ItemID", j].Value.ToString() == TempDt.Rows[i]["ItemID"].ToString() && Grid_Budget["ColorID", j].Value.ToString() == TempDt.Rows[i]["ColorID"].ToString() && Grid_Budget["SizeID", j].Value.ToString() == TempDt.Rows[i]["SizeID"].ToString())
                        {
                            for (int k = 10; k <= Grid_Budget.Columns.Count - 1; k++)
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

        private void FrmBudgetApproval_Load(object sender, EventArgs e)
        {
            try
            {
                MDIMain MyParent = (MDIMain)this.MdiParent;
                Mybase.Clear(this);
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
                Mybase.Load_Data("SELECT 0 AS SLNO, O_Slno, Access_Type Access, Cast(0 as Numeric (25, 2)) PREVIOUS, Cast(0 as Numeric (25, 2)) as NOW_, Cast(0 as Numeric (25, 2)) AMOUNT FROM Vaahini_ERP_Gainup.DBo.Budget_Approval_Summary_Socks () WHERE ORDER_NO in (" + OCN_List + ") Group By O_Slno, Access_Type Order By O_Slno", ref Dt_Sum);
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
                    for (int j = Dt_Budget.Columns.Count -1; j >= 11; j--)
                    {
                        if (Convert.ToDouble(Grid_Budget[j, i].Value) != Convert.ToDouble(Grid_Budget[j - 1, i].Value))
                        {
                            Grid_Budget[9, i].Value = Grid_Budget[9, i].Value.ToString() + " ** ";
                            break;
                        }
                    }
                }
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
                Mybase.Load_Data("SELECT 0 as SLNO, ORDER_NO, Ord_Qty QTY, Unit_Price_INR PRICE_INR, Unit_Price PRICE_CUR, Ex_Rate EX_RATE, CAST(0 AS NUMERIC (25, 2)) AP_AMOUNT, CAST(0 AS NUMERIC (25, 2)) AP_PRICE, CAST(0 AS NUMERIC (25, 2)) PROFIT_INR, CAST(0 AS NUMERIC (25, 4)) PROFIT_CUR, CAST(0 AS NUMERIC (25, 2)) PROFIT_PER FROM Vaahini_ERP_Gainup.DBo.Basic_Order_Details_Socks() WHERE ORDER_NO IN (" + OCN_List + ")", ref Dt_Final);
                Grid_Final.DataSource = Mybase.V_DataTable(ref Dt_Final);
                Mybase.Grid_Designing(ref Grid_Final, ref Dt_Final);
                Mybase.ReadOnly_Grid_Without(ref Grid_Final);
                Mybase.Grid_Colouring(ref Grid_Final, Control_Modules.Grid_Design_Mode.Column_Wise);
                Mybase.Grid_Width(ref Grid_Final, 60, 120, 80, 80, 80, 80, 130, 80, 80, 80, 80);
                Grid_Final.RowHeadersWidth = 10;

                Grid_Final.Columns["PRICE_CUR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Final.Columns["PRICE_INR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid_Final.Columns["PROFIT_INR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Final.Columns["PROFIT_CUR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid_Final.Columns["PROFIT_PER"].HeaderText = "%";
                Grid_Final.Columns["PROFIT_PER"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid_Final.Columns["PRICE_CUR"].DefaultCellStyle.Format = "0.0000";
                Grid_Final.Columns["PROFIT_CUR"].DefaultCellStyle.Format = "0.0000";

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
                            Grid_Final["Ap_Price", i].Value = Amount / Convert.ToDouble(Grid_Final["Qty", i].Value);


                            Grid_Final["Profit_INR", i].Value = Convert.ToDouble(Grid_Final["PRICE_INR", i].Value) - Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDouble(Grid_Final["AP_Price", i].Value)));
                            
                            Grid_Final["Profit_CUR", i].Value = Convert.ToDouble(Grid_Final["Profit_INR", i].Value) / Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDouble(Grid_Final["EX_RATE", i].Value)));

                            Grid_Final["Profit_Per", i].Value = (Convert.ToDouble(Grid_Final["PROFIT_INR", i].Value) / Convert.ToDouble(Grid_Final["PRICE_INR", i].Value)) * 100;

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


                this.Cursor = Cursors.WaitCursor;

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Status", i].Value != null && Grid["Status", i].Value != DBNull.Value && Grid["Status", i].Value.ToString().ToUpper() == "true".ToUpper())
                    {
                        if (OCN_List.Trim() == String.Empty)
                        {
                            OCN_List = "'" + Grid["Order_no", i].Value.ToString() + "'";
                        }
                        else
                        {
                            OCN_List += ", '" + Grid["Order_no", i].Value.ToString() + "'";
                        }
                    }
                }


                String Query = String.Empty;

                // Create Cursor for Ocn_List - QTY
                Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (4000) = ''; Declare @Ocn_No_List_Act Varchar (4000) = ''; Declare @Ocn_No_List_SQ Varchar (4000) = ''; Declare @Result as NVarchar (4000); Declare C1_OCn Cursor For Select Distinct Order_No From FITSocks.Dbo.Buy_ord_mas Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, quantity From Vaahini_ERP_Gainup.DBo.Budget_Approval_Socks () where order_no IN (' + @Ocn_No_List_SQ + ')) A1 PIVOT (sum(quantity) for order_no in (' + @Ocn_No_List_Act + ')) A '; Exec SP_ExecuteSql @Result;";
                Mybase.Load_Data(Query, ref Dt_Budget_Qty);


                // Create Cursor for Ocn_List - RATE
                Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (4000) = ''; Declare @Ocn_No_List_Act Varchar (4000) = ''; Declare @Ocn_No_List_SQ Varchar (4000) = ''; Declare @Result as NVarchar (4000); Declare C1_OCn Cursor For Select Distinct Order_No From FITSocks.Dbo.Buy_ord_mas Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, Rate From Vaahini_ERP_Gainup.DBo.Budget_Approval_Socks () where order_no IN (' + @Ocn_No_List_SQ + ')) A1 PIVOT (MIN(RATE) for order_no in (' + @Ocn_No_List_Act + ')) A '; Exec SP_ExecuteSql @Result;";
                Grid_Budget.DataSource = Mybase.Load_Data(Query, ref Dt_Budget);


                Mybase.Grid_Designing(ref Grid_Budget, ref Dt_Budget, "O_Slno", "ProcessID", "ItemID", "ColorID", "SizeID");
                //Mybase.Grid_Colouring(ref Grid_Budget, Control_Modules.Grid_Design_Mode.Column_Wise);
                Mybase.Grid_Width(ref Grid_Budget, 120, 120, 250, 80, 90, 100);
                Mybase.ReadOnly_Grid_Without(ref Grid_Budget);

                Grid_Budget.RowHeadersWidth = 10;

                tabControl1.SelectTab(TabDetails);



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
                            for (int k = 9; k <= Dt_Budget.Columns.Count - 1; k++)
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

        private void GBMain_Enter(object sender, EventArgs e)
        {

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
            Double Rate = 0;
            try
            {
                if (Grid_Budget.CurrentCell == null)
                {
                    return;
                }

                if (Dt_OCN_List[Grid_Budget.CurrentCell.RowIndex] == null)
                {
                    Dt_OCN_List[Grid_Budget.CurrentCell.RowIndex] = new DataTable();
                    Mybase.Load_Data("Select Order_No ORDER_NO, ITEM, COLOR, SIZE, Quantity QTY, Rate RATE From Vaahini_ERP_Gainup.DBo.Budget_Approval() Where Order_No in (" + OCN_List + ") And ProcessID = '" + Grid_Budget["ProcessID", Grid_Budget.CurrentCell.RowIndex].Value.ToString() + "' Order By ORDER_NO ", ref Dt_OCN_List[Grid_Budget.CurrentCell.RowIndex]);
                }

                Grid_OCN_list.DataSource = Mybase.V_DataTable (ref Dt_OCN_List[Grid_Budget.CurrentCell.RowIndex]);
                Mybase.Grid_Designing(ref Grid_OCN_list, ref Dt_OCN_List[Grid_Budget.CurrentCell.RowIndex]);
                Mybase.ReadOnly_Grid_Without(ref Grid_OCN_list);
                Mybase.Grid_Colouring(ref Grid_OCN_list, Control_Modules.Grid_Design_Mode.Column_Wise);
                Mybase.Grid_Width(ref Grid_OCN_list, 110, 200, 90, 90, 100, 100);
                Grid_OCN_list.RowHeadersWidth = 10;

                Mybase.V_DataGridView(ref Grid_OCN_list);


                for (int i = 0; i <= Dt_OCN_List[Grid_Budget.CurrentCell.RowIndex].Rows.Count - 1; i++)
                {
                    Rate += Convert.ToDouble(Dt_OCN_List[Grid_Budget.CurrentCell.RowIndex].Rows[i]["Rate"]);
                }

                Grid_OCN_list["Rate", Grid_OCN_list.Rows.Count - 1].Value = Convert.ToDouble(Rate / Dt_OCN_List[Grid_Budget.CurrentCell.RowIndex].Rows.Count);

                Grid_OCN_list.Rows[Grid_OCN_list.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                MessageBox.Show (ex.Message);
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
                    Grid_Budget[i, Grid_Budget.CurrentCell.RowIndex].Style.ForeColor = Color.Black ;

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


                if (MessageBox.Show ("Sure to Approve ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
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

                Int32 Rows = (Dt_Budget.Rows.Count * (Dt_Budget.Columns.Count - 10)) * 2;

                String[] Queries = new string[Rows];
                Int32 Array_Index = 0;
                

                for (int i = 0; i <= Dt_Budget.Rows.Count - 1; i++)
                {
                    for (int k = 10; k <= Dt_Budget.Columns.Count - 1; k++)
                    {
                        if (Convert.ToInt32(Dt_Budget.Rows[i]["O_Slno"]) == 7)
                        {
                            if (Grid_Budget[k, i].Style.BackColor == Color.Green || Grid_Budget[k, i].Style.BackColor == Color.Yellow)
                            {
                                Queries[Array_Index++] = "update C2 Set C2.AppCost = C2.Cost FROM FITSOCKS.DBO.COST_defn_mas C1 Inner Join FITSOCKS.DBO.Cost_Defn_COM C2 on c1.Cost_Defn_id = c2.Cost_Defn_id where C1.Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Particularid = " + Grid_Budget["ProcessID", i].Value.ToString();
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                            }
                            else
                            {
                                Queries[Array_Index++] = "update C2 Set C2.AppCost = 0 FROM FITSOCKS.DBO.COST_defn_mas C1 Inner Join FITSOCKS.DBO.Cost_Defn_COM C2 on c1.Cost_Defn_id = c2.Cost_Defn_id where C1.Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Particularid = " + Grid_Budget["ProcessID", i].Value.ToString();
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                            }
                        }
                        else
                        {
                            if (Grid_Budget[k, i].Style.BackColor == Color.Green || Grid_Budget[k, i].Style.BackColor == Color.Yellow)
                            {
                                Queries[Array_Index++] = "Update C2 Set C2.AppRate = C2.Rate, C2.AppQty = C2.Quantity FROM FITSOCKS.DBO.COST_defn_mas C1 Inner Join FITSOCKS.DBO.Cost_Defn_BOM C2 on c1.Cost_Defn_id = c2.Cost_Defn_id where c1.Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and itemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and ISNULL(C2.Processid, C2.ITEMID) = " + Grid_Budget["ProcessID", i].Value.ToString();
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                            }
                            else
                            {
                                Queries[Array_Index++] = "Update C2 Set C2.AppRate = 0, C2.AppQty = 0 FROM FITSOCKS.DBO.COST_defn_mas C1 Inner Join FITSOCKS.DBO.Cost_Defn_BOM C2 on c1.Cost_Defn_id = c2.Cost_Defn_id where c1.Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and itemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and ISNULL(C2.Processid, C2.ITEMID) = " + Grid_Budget["ProcessID", i].Value.ToString();
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                            }
                        }
                        
                    }
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

    }
}