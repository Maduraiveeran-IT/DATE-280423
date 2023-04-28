/*
FileName 	: FrmPRojectBudgetApproval_New 
Module	        : Project

Developer details
-----------------------
Name of the Author	    : Livinstone K
Date Created		    : 
Tables Used		    : Projects.Dbo.Project_Planning_Master ,Projects.Dbo.Project_Planning_Summary_Details,Projects.Dbo.Project_Order_Master ,  Projects.Dbo.Project_Planning_Material_Details ,
			      Projects.Dbo.Budget_Approval_Status,Projects.Dbo.Project_Planning_Process_Details ,Projects.Dbo.Project_Planning_Process_Details ,Projects.Dbo.Project_Planning_Comm_Details  
 			     
				
Functions Used	            :  Projects.Dbo.Project_ORder_Fn,Projects.Dbo.Project_Bom_Item_Fn,Projects.Dbo.Budget_Approval_Project ,Projects.DBo.Basic_Order_Details_Project_New 
			       Projects.DBo.Budget_Approval_Project_ID, 
	      
View Used	            : 
Crystal report File Name    :  
Based On Ticket No	    :  
Reviewed By		    : Livingstone K

Modification details
-----------------------
Done By			        : Livingstone K
Modified On				: 18-oct-2021
Event/Procedure/Sub/Function Name	:Budget_Approval_Project()
Remarks    : Mandays Rate mismatch for duplicate, Change rate min to Actual rate,
Based On Ticket No			:
Reviewed By				:
*/


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
    public partial class FrmPRojectBudgetApproval_New : Form
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
        DataTable Dt_Budget_Qty1 = new DataTable();
        DataTable Dt_Budget_ID = new DataTable();
        DataTable Dt_Budget_IDC = new DataTable();   
        DataTable Dt_Final = new DataTable();
        TextBox Txt = null;

        public FrmPRojectBudgetApproval_New()
        {
            InitializeComponent();
        }

        private void FrmPRojectBudgetApproval_New_KeyDown(object sender, KeyEventArgs e)
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
                       if(MyParent.UserName.ToString() == "MD" || MyParent.UserName.ToString() == "ADMIN")
                       {
                           Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer", "Select Distinct Party Proj_Name, PArty_Code BuyerID From Projects.Dbo.Project_ORder_Fn() Where Company_Code = " + MyParent.CompCode + "  ORder by 1", String.Empty, 350);                       
                       }                       
                       else
                       {
                           Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer", "Select Distinct Party Proj_Name, PArty_Code BuyerID From Projects.Dbo.Project_ORder_Fn() Where Company_Code = " + MyParent.CompCode + " ORder by 1", String.Empty, 350);                       
                       }
                                               
                        if (Dr != null)
                        {
                            TxtBuyer.Text = Dr["Proj_Name"].ToString();
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
                Str = "Select A.Order_No, A.Proj_Name, A.Proj_Activity_Name Activity, A.Qty  Qty, Max(B.Complete_Date) Complete_Date, 'INR' Currency ,'True' TimeAction, C.Total_Material_Amount Amount   From Projects.Dbo.Project_Bom_Item_Fn() A Inner Join Projects.Dbo.Project_Order_Fn() B On A.Order_ID = B.RowID and A.Proj_Type_ID = B.Proj_Type_ID and A.Proj_Activity_ID = B.Proj_Activity_ID and B.Cancel_ORder in ('N') Inner Join Projects.Dbo.Project_Planning_Master C On A.Order_ID = C.Order_ID and A.Proj_Type_ID = C.Proj_Type_ID and A.Proj_Activity_ID = C.Proj_Activity_ID LEft Join Projects.Dbo.Project_Planning_Summary_Details C1 On C.RowID = C1.Master_ID  Where  A.Party_Code = " + TxtBuyer.Tag + " and B.Company_Code = " + MyParent.CompCode + " Group by A.Order_No, A.Proj_Name, A.Proj_Activity_Name, A.Qty, C.Total_Material_Amount Order by A.Order_No Desc";
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
              
                Mybase.Grid_Width(ref Grid, 60, 100, 200, 300, 100, 100, 180);

                Grid.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Mybase.Grid_Designing(ref Grid, ref Dt, "TimeAction", "Currency");
                Grid.Columns["Status"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                Grid.RowHeadersWidth = 10;
                TxtOrders.Text = Dt.Rows.Count.ToString();               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmPRojectBudgetApproval_New_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == String.Empty)
                    {
                    }
                    else
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

        void Fill_Existing_Approval()
        {
            DataTable TempDt = new DataTable();
            try
            {
                Mybase.Load_Data("Select Distinct Order_No, O_Slno, ProcessID, ItemID, ColorID, Sizeid, PlanDtlID, PlanMasID  From Projects.Dbo.Budget_Approval_Project ()  Where Order_No in (" + OCN_List + ") Group by Order_No, O_Slno, ProcessID, ItemID, ColorID, Sizeid, PlanDtlID, PlanMasID  Having Sum(Case When App_Qty =0 then 1 Else 0 End) =0 Order By O_Slno, ProcessID, ItemID", ref TempDt);
                //Mybase.Load_Data("Select Order_No, O_Slno, ProcessID, ItemID, ColorID, Sizeid, PlanDtlID, PlanMasID From Projects.Dbo.Budget_Approval_Project ()  Where Order_No in (" + OCN_List + ") and   App_Qty = Quantity  Order By O_Slno, ProcessID, ItemID", ref TempDt);
                
                for (int i = 0; i <= TempDt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Dt_Budget.Rows.Count - 1; j++)
                    {
                        //if (Grid_Budget["O_Slno", j].Value.ToString() == TempDt.Rows[i]["O_Slno"].ToString() && Grid_Budget["ProcessID", j].Value.ToString() == TempDt.Rows[i]["ProcessID"].ToString() && Grid_Budget["ItemID", j].Value.ToString() == TempDt.Rows[i]["ItemID"].ToString() && Grid_Budget["ColorID", j].Value.ToString() == TempDt.Rows[i]["ColorID"].ToString() && Grid_Budget["SizeID", j].Value.ToString() == TempDt.Rows[i]["SizeID"].ToString() && Grid_Budget["PlanDtlID", j].Value.ToString() == TempDt.Rows[i]["PlanDtlID"].ToString() && Grid_Budget["PlanMasID", j].Value.ToString() == TempDt.Rows[i]["PlanMasID"].ToString())
                        if (Grid_Budget_PR["O_Slno", j].Value.ToString() == TempDt.Rows[i]["O_Slno"].ToString() && Grid_Budget_PR["ProcessID", j].Value.ToString() == TempDt.Rows[i]["ProcessID"].ToString() && Grid_Budget_PR["ItemID", j].Value.ToString() == TempDt.Rows[i]["ItemID"].ToString() && Grid_Budget_PR["ColorID", j].Value.ToString() == TempDt.Rows[i]["ColorID"].ToString() && Grid_Budget_PR["SizeID", j].Value.ToString() == TempDt.Rows[i]["SizeID"].ToString())
                        {
                            for (int k = 8; k <= Grid_Budget_PR.Columns.Count - 1; k++)
                            {
                                if (Grid_Budget_PR.Columns[k].Name == TempDt.Rows[i]["Order_No"].ToString())
                                {
                                    Grid_Budget_PR[k, j].Style.BackColor = Color.Yellow;
                                    Grid_Budget_PR[k, j].Style.ForeColor = Color.Black;
                                    Grid_Budget_PR[k, j].Style.SelectionBackColor = Color.Yellow;
                                    Grid_Budget_PR[k, j].Style.SelectionForeColor = Color.Black;
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

        private void FrmPRojectBudgetApproval_New_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                Mybase.Clear(this);
                if(MyParent.UserName == "MD" || MyParent.UserName == "ADMIN")
                {
                    button8.Visible =true;
                }
                else
                {
                    button8.Visible =false;
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
                Mybase.Load_Data("SELECT 0 AS SLNO,  O_Slno,  Access_Type ACCESS, Sum(Quantity) UNIT,  Cast(0 as Numeric (25, 2)) PREVIOUS, Cast(0 as Numeric (25, 2)) as NOW_, Cast(0 as Numeric (25, 2)) AMOUNT FROM PROJECTS.DBo.Budget_Approval_Summary_Project() WHERE ORDER_NO in (" + OCN_List + ")  Group By O_Slno , Access_Type   Order By Access_Type, O_Slno ", ref Dt_Sum);
                Grid_Sum.DataSource = Mybase.V_DataTable(ref Dt_Sum);
                Mybase.Grid_Designing(ref Grid_Sum, ref Dt_Sum, "O_Slno");
                Mybase.ReadOnly_Grid_Without(ref Grid_Sum);
                Mybase.Grid_Colouring(ref Grid_Sum, Control_Modules.Grid_Design_Mode.Column_Wise);
                Mybase.Grid_Width(ref Grid_Sum, 60, 200, 120, 130, 130, 130);

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
                    for (int j = 10; j <= Dt_Budget.Columns.Count-1; j++)
                    {
                        for (int k = j; k <= Dt_Budget.Columns.Count-1; k++)
                        {
                            if(Convert.ToDouble(Grid_Budget_PR[j, i].Value) != 0 && Convert.ToDouble(Grid_Budget_PR[k, i].Value) != 0)
                            {
                                if (Convert.ToDouble(Grid_Budget_PR[j, i].Value) != Convert.ToDouble(Grid_Budget_PR[k, i].Value))
                                {
                                    if (Grid_Budget_PR[9, i].Value.ToString().Contains("**") == true)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Grid_Budget_PR[9, i].Value = Grid_Budget_PR[9, i].Value.ToString() + " ** ";
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
                Mybase.Load_Data("SELECT 0 as SLNO, ORDER_NO, Ord_Qty QTY, Unit_Price PRICE_INR, Unit_Price_INR  PRICE_CUR, Ex_Rate EX_RATE, CAST(0 AS NUMERIC (25, 2)) AP_AMOUNT, CAST(0 AS NUMERIC (25, 2)) AP_PRICE, CAST(0 AS NUMERIC (25, 2)) AP_CUR, CAST(0 AS NUMERIC (25, 2)) PROFIT_INR, CAST(0 AS NUMERIC (25, 4)) PROFIT_CUR, CAST(0 AS NUMERIC (25, 2)) PROFIT_PER FROM Projects.DBo.Basic_Order_Details_Project_New() WHERE ORDER_NO IN (" + OCN_List + ")", ref Dt_Final);
                //Mybase.Load_Data("SELECT 0 as SLNO, ORDER_NO, Ord_Qty QTY, Unit_Price_INR PRICE_INR, Unit_Price PRICE_CUR, Ex_Rate EX_RATE, CAST(0 AS NUMERIC (25, 2)) AP_AMOUNT, CAST(0 AS NUMERIC (25, 2)) AP_PRICE, CAST(0 AS NUMERIC (25, 2)) PROFIT_INR, CAST(0 AS NUMERIC (25, 4)) PROFIT_CUR, CAST(0 AS NUMERIC (25, 2)) PROFIT_PER FROM Vaahini_ERP_Gainup.DBo.Basic_Order_Details_Socks() WHERE ORDER_NO IN (" + OCN_List + ")", ref Dt_Final);
                Grid_Final.DataSource = Mybase.V_DataTable(ref Dt_Final);
                Mybase.Grid_Designing(ref Grid_Final, ref Dt_Final);
                Mybase.ReadOnly_Grid_Without(ref Grid_Final);
                Mybase.Grid_Designing(ref Grid_Final, ref Dt_Final, "PRICE_INR", "PRICE_CUR", "EX_RATE", "AP_CUR", "PROFIT_INR", "PROFIT_CUR", "PROFIT_PER");
                Mybase.Grid_Colouring(ref Grid_Final, Control_Modules.Grid_Design_Mode.Column_Wise);
                Mybase.Grid_Width(ref Grid_Final, 100, 160, 160, 140, 140, 140);
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
                    for (int j = 0; j <= Grid_Budget_PR.Columns.Count - 1; j++)
                    {
                        if (Grid_Final["Order_No", i].Value.ToString() == Grid_Budget_PR.Columns[j].Name.ToString())
                        {
                            for (int k = 0; k <= Grid_Budget_PR.Rows.Count - 1; k++)
                            {
                                if (Grid_Budget_PR[j, k].Style.BackColor == System.Drawing.Color.Green || Grid_Budget_PR[j, k].Style.BackColor == System.Drawing.Color.Yellow)
                                {
                                    Amount += Convert.ToDouble(Dt_Budget.Rows[k][j]) * Convert.ToDouble(Dt_Budget_Qty.Rows[k][j]);
                                }
                            }

                            Grid_Final["Ap_Amount", i].Value = Amount;
                            Grid_Final["Ap_Price", i].Value = (Convert.ToDouble(Amount / Convert.ToDouble(Grid_Final["Qty", i].Value)));


                            Grid_Final["Profit_INR", i].Value = Math.Round(Convert.ToDouble(Grid_Final["PRICE_INR", i].Value) - Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDouble(Grid_Final["AP_Price", i].Value))),2);
                            
                            Grid_Final["Profit_CUR", i].Value = Convert.ToDouble(Grid_Final["Profit_INR", i].Value) / Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDouble(Grid_Final["EX_RATE", i].Value)));

                            if (Convert.ToDouble(Grid_Final["PRICE_INR", i].Value) != 0)
                            {
                                Grid_Final["Profit_Per", i].Value = (Convert.ToDouble(Grid_Final["PROFIT_INR", i].Value) / Convert.ToDouble(Grid_Final["PRICE_INR", i].Value)) * 100;
                            }
                            else
                            {
                                Grid_Final["PRICE_INR", i].Value = 0;
                            }

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

                
                Grid_Budget_PR.DataSource = null;
                Dt_Budget = new DataTable();
                Dt_Budget_Qty = new DataTable();
                Dt_Budget_Qty1 = new DataTable();
                Dt_Budget_ID = new DataTable();
                Dt_Budget_IDC = new DataTable();


                

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Status", i].Value != null && Grid["Status", i].Value != DBNull.Value && Grid["Status", i].Value.ToString().ToUpper() == "true".ToUpper() &&  Grid["TimeAction", i].Value.ToString().ToUpper() == "True".ToUpper())
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
                            Item_List = "'" + Grid["Proj_Name", i].Value.ToString() + "'";
                        }
                        else
                        {
                            Item_List += ", '" + Grid["Proj_Name", i].Value.ToString() + "'";
                        }

                    }
                }

                if (OCN_List == String.Empty)
                {
                    MessageBox.Show("Invalid OCN....!", "Gainup");
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                String Query = String.Empty;
                // Create Cursor for Ocn_List - QTY
                Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (MAX) = ''; Declare @Ocn_No_List_Act Varchar (MAX) = ''; Declare @Ocn_No_List_SQ Varchar (MAX) = ''; Declare @Result as NVarchar (MAX); Declare C1_OCn Cursor For Select Distinct Order_No From Projects.Dbo.Project_Order_Master Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, Sum(quantity) quantity  From Projects.DBo.Budget_Approval_Project() where order_no IN (' + @Ocn_No_List_SQ + ') Group by   Rate, Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size) A1 PIVOT (sum(quantity) for order_no in (' + @Ocn_No_List_Act + ')) A Order by O_Slno, Access_Type, Item, Color, Size'; Exec SP_ExecuteSql @Result;";
                Mybase.Load_Data(Query, ref Dt_Budget_Qty);


                // Create Cursor for Ocn_List - RATE
                Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (MAX) = ''; Declare @Ocn_No_List_Act Varchar (MAX) = ''; Declare @Ocn_No_List_SQ Varchar (MAX) = ''; Declare @Result as NVarchar (MAX); Declare C1_OCn Cursor For Select Distinct Order_No From Projects.Dbo.Project_Order_Master  Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
               // Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, (Case When   (App_Rate) = 0 Then  (Rate) Else (Select Min(App_Rate) From  Projects.DBo.Budget_Approval_Project_ID() B Where  B.App_Rate > 0 And B.Item = A.Item and B.Color = A.Color and A.Size = b.Size And B.Order_No = A.Order_No) End)     Rate, PlanDtlID  From  Projects.DBo.Budget_Approval_Project() A where order_no IN (' + @Ocn_No_List_SQ + ') Group by PlanDtlID,Rate, App_Rate ,Order_No, O_Slno,  Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size) A1 PIVOT (Max(RATE) for order_no in (' + @Ocn_No_List_Act + ')) A Order by O_Slno, Access_Type, Item, Color, Size'; Exec SP_ExecuteSql @Result;";
                Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid,  ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, (Case When Max(App_Rate) = 0 Then Min(Rate) Else (Select Min(App_Rate) From  Projects.DBo.Budget_Approval_Project() B Where  B.App_Rate > 0 And B.Item = A.Item and B.Color = A.Color and A.Size = b.Size And B.Order_No = A.Order_No and A.Processid = B.Processid) End)     Rate From  Projects.DBo.Budget_Approval_Project() A where order_no IN (' + @Ocn_No_List_SQ + ') Group by Rate,  Order_No, O_Slno,  Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size) A1 PIVOT (Max(RATE) for order_no in (' + @Ocn_No_List_Act + ')) A Order by O_Slno, Access_Type, Item, Color, Size'; Exec SP_ExecuteSql @Result;";

                Grid_Budget_PR.DataSource = Mybase.Load_Data(Query, ref Dt_Budget);

                Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (MAX) = ''; Declare @Ocn_No_List_Act Varchar (MAX) = ''; Declare @Ocn_No_List_SQ Varchar (MAX) = ''; Declare @Result as NVarchar (MAX); Declare C1_OCn Cursor For Select Distinct Order_No From Projects.Dbo.Project_Order_Master  Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, Max(PlanDtlID) PlanDtlID  From  Projects.DBo.Budget_Approval_Project_ID() where order_no IN (' + @Ocn_No_List_SQ + ') Group by Rate,   Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, PlanDtlID) A1 PIVOT (Max(PlanDtlID) for order_no in (' + @Ocn_No_List_Act + ')) A Order by O_Slno, Access_Type, Item, Color, Size '; Exec SP_ExecuteSql @Result;";
                Mybase.Load_Data(Query, ref Dt_Budget_ID);

                Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (MAX) = ''; Declare @Ocn_No_List_Act Varchar (MAX) = ''; Declare @Ocn_No_List_SQ Varchar (MAX) = ''; Declare @Result as NVarchar (MAX); Declare C1_OCn Cursor For Select Distinct Order_No From Projects.Dbo.Project_Order_Master  Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, (PlanDtlID) PlanDtlID  From  Projects.DBo.Budget_Approval_Project_ID() where order_no IN (' + @Ocn_No_List_SQ + ') Group by Rate,  Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, PlanDtlID) A1 PIVOT (Count(PlanDtlID) for order_no in (' + @Ocn_No_List_Act + ')) A Order by O_Slno, Access_Type, Item, Color, Size'; Exec SP_ExecuteSql @Result;";
                Mybase.Load_Data(Query, ref Dt_Budget_IDC);
                //// Create Cursor for Ocn_List - QTY
                //Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (4000) = ''; Declare @Ocn_No_List_Act Varchar (4000) = ''; Declare @Ocn_No_List_SQ Varchar (4000) = ''; Declare @Result as NVarchar (4000); Declare C1_OCn Cursor For Select Distinct Order_No From FITSocks.Dbo.Buy_ord_mas Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                //Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, quantity From Vaahini_ERP_Gainup.DBo.Budget_Approval_Socks () where order_no IN (' + @Ocn_No_List_SQ + ')) A1 PIVOT (sum(quantity) for order_no in (' + @Ocn_No_List_Act + ')) A '; Exec SP_ExecuteSql @Result;";
                //Mybase.Load_Data(Query, ref Dt_Budget_Qty);

                Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (MAX) = ''; Declare @Ocn_No_List_Act Varchar (MAX) = ''; Declare @Ocn_No_List_SQ Varchar (MAX) = ''; Declare @Result as NVarchar (MAX); Declare C1_OCn Cursor For Select Distinct Order_No From Projects.Dbo.Project_Order_Master Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, Sum(App_Qty) quantity  From Projects.DBo.Budget_Approval_Project() where order_no IN (' + @Ocn_No_List_SQ + ') Group by  Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size) A1 PIVOT (sum(quantity) for order_no in (' + @Ocn_No_List_Act + ')) A Order by O_Slno, Access_Type, Item, Color, Size'; Exec SP_ExecuteSql @Result;";
                Mybase.Load_Data(Query, ref Dt_Budget_Qty1);

                //// Create Cursor for Ocn_List - QTY
                //Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (4000) = ''; Declare @Ocn_No_List_Act Varchar (4000) = ''; Declare @Ocn_No_List_SQ Varchar (4000) = ''; Declare @Result as NVarchar (4000); Declare C1_OCn Cursor For Select Distinct Order_No From FITSocks.Dbo.Buy_ord_mas Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                //Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, quantity From Vaahini_ERP_Gainup.DBo.Budget_Approval_Socks () where order_no IN (' + @Ocn_No_List_SQ + ')) A1 PIVOT (sum(quantity) for order_no in (' + @Ocn_No_List_Act + ')) A '; Exec SP_ExecuteSql @Result;";
                //Mybase.Load_Data(Query, ref Dt_Budget_Qty);

                //Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (MAX) = ''; Declare @Ocn_No_List_Act Varchar (MAX) = ''; Declare @Ocn_No_List_SQ Varchar (MAX) = ''; Declare @Result as NVarchar (MAX); Declare C1_OCn Cursor For Select Distinct Order_No From Projects.Dbo.Project_Order_Master Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                //Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, Sum(App_Qty) quantity  From Projects.DBo.Budget_Approval_Project() where order_no IN (' + @Ocn_No_List_SQ + ') Group by Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size) A1 PIVOT (sum(quantity) for order_no in (' + @Ocn_No_List_Act + ')) A Order by O_Slno, Access_Type, Item, Color, Size'; Exec SP_ExecuteSql @Result;";
                //Mybase.Load_Data(Query, ref Dt_Budget_Qty1);


                //// Create Cursor for Ocn_List - RATE
                //Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (4000) = ''; Declare @Ocn_No_List_Act Varchar (4000) = ''; Declare @Ocn_No_List_SQ Varchar (4000) = ''; Declare @Result as NVarchar (4000); Declare C1_OCn Cursor For Select Distinct Order_No From FITSocks.Dbo.Buy_ord_mas Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                //Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, Rate From Vaahini_ERP_Gainup.DBo.Budget_Approval_Socks () where order_no IN (' + @Ocn_No_List_SQ + ')) A1 PIVOT (MIN(RATE) for order_no in (' + @Ocn_No_List_Act + ')) A '; Exec SP_ExecuteSql @Result;";
                //Grid_Budget.DataSource = Mybase.Load_Data(Query, ref Dt_Budget);

                // Create Cursor for Ocn_List - QTY
                //Query = "Declare @Ocn_No Varchar (20); Declare @Ocn_No_List_Isnull Varchar (MAX) = ''; Declare @Ocn_No_List_Act Varchar (MAX) = ''; Declare @Ocn_No_List_SQ Varchar (MAX) = ''; Declare @Result as NVarchar (MAX); Declare C1_OCn Cursor For Select Distinct Order_No From FITSocks.Dbo.Socks_Order_Master Where order_No in (" + OCN_List + "); Open C1_Ocn; Fetch Next From C1_Ocn into @Ocn_No; While @@FETCH_STATUS = 0 Begin if (Len(@Ocn_No_List_Isnull) = 0) Begin Set @Ocn_No_List_Isnull = 'Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = '[' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  Char(39) + @Ocn_No + Char(39); end else Begin Set @Ocn_No_List_Isnull = @Ocn_No_List_Isnull + ', Isnull([' + @Ocn_No + '], 0) [' + @Ocn_No + ']'; Set @Ocn_No_List_Act = @Ocn_No_List_Act + ', [' + @Ocn_No + ']'; Set @Ocn_No_List_SQ =  @Ocn_No_List_SQ + ', ' + Char(39) + @Ocn_No + Char(39); end Fetch Next From C1_Ocn into @Ocn_No; End Close C1_Ocn; Deallocate C1_Ocn; ";
                //Query += " Set @Result = ' Select O_Slno, Access_Type ACCESS, Processid, ItemID, ColorID, SizeID, PROCESS, Item ITEM, Color COLOR, Size SIZE, ' + @Ocn_No_List_Isnull + ' FROM (Select Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size, Sum(quantity) quantity  From Vaahini_ERP_Gainup.DBo.Budget_Approval_Socks_New() where order_no IN (' + @Ocn_No_List_SQ + ') Group by Order_No, O_Slno, Access_Type, Processid, ItemID, ColorID, SizeID, PROCESS, Item, Color, Size) A1 PIVOT (sum(quantity) for order_no in (' + @Ocn_No_List_Act + ')) A '; Exec SP_ExecuteSql @Result;";
                //Mybase.Load_Data(Query, ref Dt_Budget_Qty);


                Mybase.Grid_Designing(ref Grid_Budget_PR, ref Dt_Budget, "O_Slno", "ProcessID", "ItemID", "ColorID", "SizeID");
                //Mybase.Grid_Colouring(ref Grid_Budget, Control_Modules.Grid_Design_Mode.Column_Wise);
                Mybase.Grid_Width(ref Grid_Budget_PR, 120, 100, 200, 200, 100, 120);
                Mybase.ReadOnly_Grid_Without(ref Grid_Budget_PR);

                for (int i = 10; i <= Grid_Budget_PR.Columns.Count - 1; i++)
                {
                    if (Grid_Budget_PR.Columns[i].Visible)
                    {
                        Grid_Budget_PR.Columns[i].ReadOnly = false;
                    }
                }

                Grid_Budget_PR.RowHeadersWidth = 10;

                tabControl1.SelectTab(TabDetails);

                Grid_Budget_PR.Refresh();

                for (int i = Dt_Budget.Rows.Count - 1; i >= 1; i--)
                {
                    if (Grid_Budget_PR["Access", i].Value.ToString() == Grid_Budget_PR["Access", i - 1].Value.ToString())
                    {
                        if(Convert.ToDouble(Grid_Budget_PR["O_Slno", i].Value.ToString()) != 3)
                        {
                            Grid_Budget_PR["Access", i].Value = String.Empty;
                        }
                    }
                }

                for (int i = Dt_Budget.Rows.Count - 1; i >= 1; i--)
                {
                    if (Grid_Budget_PR["Process", i].Value.ToString() == Grid_Budget_PR["Process", i - 1].Value.ToString())
                    {
                        Grid_Budget_PR["Process", i].Value = String.Empty;
                    }
                }


                for (int i = 0; i <= Grid_Budget_PR.Columns.Count - 1; i++)
                {
                    if (i < 10)
                    {
                        Grid_Budget_PR.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                    }
                    else
                    {
                        Grid_Budget_PR.Columns[i].HeaderText = Convert.ToInt32(Grid_Budget_PR.Columns[i].Name.Substring(7, 4)).ToString();
                        Grid_Budget_PR.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }

                Grid_Budget_PR.Columns["COLOR"].HeaderText = "DESCRIPTION";
                Grid_Budget_PR.CurrentCell = Grid_Budget_PR["ACCESS", 0];
                Grid_Budget_PR.Focus();

                Mybase.Grid_Freeze(ref Grid_Budget_PR, Control_Modules.FreezeBY.Column_Wise, 9);

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
                        if (Dt_Sum.Rows[i]["O_Slno"].ToString() == Dt_Budget.Rows[j]["ProcessID"].ToString() && Dt_Sum.Rows[i]["Access"].ToString() == Dt_Budget.Rows[j]["Item"].ToString())
                        {
                            for (int k = 10; k <= Dt_Budget.Columns.Count - 1; k++)
                            {
                                if (Grid_Budget_PR[k, j].Style.BackColor == System.Drawing.Color.Green)
                                {
                                    Grid_Sum["Now_", i].Value = Convert.ToDouble(Grid_Sum["Now_", i].Value) + (Convert.ToDouble(Dt_Budget.Rows[j][k]) * Convert.ToDouble(Dt_Budget_Qty.Rows[j][k]));
                                    //Grid_Sum["Now_", i].Value = Convert.ToDouble(Grid_Sum["Now_", i].Value) + (Convert.ToDouble(Dt_Budget.Rows[j][k]) * Convert.ToDouble(Dt_Budget_Qty1.Rows[j][k]));
                                }
                                else if (Grid_Budget_PR[k, j].Style.BackColor == System.Drawing.Color.Yellow)
                                {                                    
                                    Grid_Sum["Previous", i].Value = Convert.ToDouble(Grid_Sum["Previous", i].Value) + (Convert.ToDouble(Dt_Budget.Rows[j][k]) * Convert.ToDouble(Dt_Budget_Qty.Rows[j][k]));
                                    //Grid_Sum["Previous", i].Value = Convert.ToDouble(Grid_Sum["Previous", i].Value) + (Convert.ToDouble(Dt_Budget.Rows[j][k]) * Convert.ToDouble(Dt_Budget_Qty1.Rows[j][k]));
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
                    if (Grid_Budget_PR.DataSource == null)
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
                if (Grid_Budget_PR.CurrentCell == null)
                {
                    return;
                }

                if (Dt_OCN_List[Grid_Budget_PR.CurrentCell.RowIndex] == null)
                {
                    Dt_OCN_List[Grid_Budget_PR.CurrentCell.RowIndex] = new DataTable();
                    Mybase.Load_Data("Select Order_No ORDER_NO, ITEM, COLOR, SIZE, Quantity QTY, Rate RATE From Vaahini_ERP_Gainup.DBo.Budget_Approval() Where Order_No in (" + OCN_List + ") And ProcessID = '" + Grid_Budget_PR["ProcessID", Grid_Budget_PR.CurrentCell.RowIndex].Value.ToString() + "' Order By ORDER_NO ", ref Dt_OCN_List[Grid_Budget_PR.CurrentCell.RowIndex]);
                }

                Grid_OCN_list.DataSource = Mybase.V_DataTable (ref Dt_OCN_List[Grid_Budget_PR.CurrentCell.RowIndex]);
                Mybase.Grid_Designing(ref Grid_OCN_list, ref Dt_OCN_List[Grid_Budget_PR.CurrentCell.RowIndex]);
                Mybase.ReadOnly_Grid_Without(ref Grid_OCN_list);
                Mybase.Grid_Colouring(ref Grid_OCN_list, Control_Modules.Grid_Design_Mode.Column_Wise);
                Mybase.Grid_Width(ref Grid_OCN_list, 110, 200, 90, 90, 100, 100);
                Grid_OCN_list.RowHeadersWidth = 10;

                Mybase.V_DataGridView(ref Grid_OCN_list);


                for (int i = 0; i <= Dt_OCN_List[Grid_Budget_PR.CurrentCell.RowIndex].Rows.Count - 1; i++)
                {
                    Rate += Convert.ToDouble(Dt_OCN_List[Grid_Budget_PR.CurrentCell.RowIndex].Rows[i]["Rate"]);
                }

                Grid_OCN_list["Rate", Grid_OCN_list.Rows.Count - 1].Value = Convert.ToDouble(Rate / Dt_OCN_List[Grid_Budget_PR.CurrentCell.RowIndex].Rows.Count);

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
                if (Grid_Budget_PR.CurrentCell.ColumnIndex > 9)
                {
                    label4.Text = String.Format("{0:0.000}", Convert.ToDouble(Dt_Budget_Qty.Rows[Grid_Budget_PR.CurrentCell.RowIndex][Grid_Budget_PR.CurrentCell.ColumnIndex]));
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
                int Col = Grid_Budget_PR.HitTest(e.X, e.Y).ColumnIndex;
                int Row = Grid_Budget_PR.HitTest(e.X, e.Y).RowIndex;
                if (Col > 9 && Row >= 0)
                {

                    Grid_Budget_PR.CurrentCell = Grid_Budget_PR[Col, Row];
                    Grid_Budget_PR.Focus();

                    if (e.Button == MouseButtons.Right)
                    {
                        ContextMenuStrip Cm = new ContextMenuStrip();

                        if (Grid_Budget_PR.CurrentCell.Style.BackColor == Color.Green || Grid_Budget_PR.CurrentCell.Style.BackColor == Color.Yellow)
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

                        Cm.Show(Grid_Budget_PR, new Point(e.X, e.Y));
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
                for (int i = Grid_Budget_PR.CurrentCell.RowIndex; i >= 0; i--)
                {
                    if (Grid_Budget_PR["Process", i].Value.ToString() != String.Empty)
                    {
                        Pos = i;
                        break;
                    }
                }

                for (int i = Pos; i <= Grid_Budget_PR.Rows.Count - 1; i++)
                {
                    if (Grid_Budget_PR["Process", i].Value.ToString() == String.Empty || i == Pos)
                    {
                        Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.BackColor = Color.White;
                        Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.ForeColor = Color.Black;

                        Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.SelectionBackColor = Grid_Budget_PR[0, 0].Style.SelectionBackColor;
                        Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.SelectionForeColor = Grid_Budget_PR[0, 0].Style.SelectionForeColor;
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
                for (int i = Grid_Budget_PR.CurrentCell.RowIndex; i >= 0; i--)
                {
                    if (Grid_Budget_PR["Process", i].Value.ToString() != String.Empty)
                    {
                        Pos = i;
                        break;
                    }
                }

                for (int i = Pos; i <= Grid_Budget_PR.Rows.Count - 1; i++)
                {
                    if (Grid_Budget_PR["Process", i].Value.ToString() == String.Empty || i == Pos)
                    {
                        if (Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.BackColor != Color.Yellow)
                        {
                            Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.BackColor = Color.Green;
                            Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.ForeColor = Color.White;

                            Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.SelectionBackColor = Color.Green;
                            Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.SelectionForeColor = Color.White;
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
                for (int i = 0; i <= Grid_Budget_PR.Rows.Count - 1; i++)
                {
                    Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.BackColor = Color.White;
                    Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.ForeColor = Color.Black;

                    Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.SelectionBackColor = Grid_Budget_PR[0, 0].Style.SelectionBackColor;
                    Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.SelectionForeColor = Grid_Budget_PR[0, 0].Style.SelectionForeColor;

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
                for (int i = 10; i <= Grid_Budget_PR.Columns.Count - 1; i++)
                {
                    Grid_Budget_PR[i, Grid_Budget_PR.CurrentCell.RowIndex].Style.BackColor = Color.White;
                    Grid_Budget_PR[i, Grid_Budget_PR.CurrentCell.RowIndex].Style.ForeColor = Color.Black ;

                    Grid_Budget_PR[i, Grid_Budget_PR.CurrentCell.RowIndex].Style.SelectionBackColor = Grid_Budget_PR[0, 0].Style.SelectionBackColor;
                    Grid_Budget_PR[i, Grid_Budget_PR.CurrentCell.RowIndex].Style.SelectionForeColor = Grid_Budget_PR[0, 0].Style.SelectionForeColor;
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
                for (int i = 0; i <= Grid_Budget_PR.Rows.Count - 1; i++)
                {
                    if (Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.BackColor != Color.Yellow)
                    {
                        Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.BackColor = Color.Green;
                        Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.ForeColor = Color.White;

                        Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.SelectionBackColor = Color.Green;
                        Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, i].Style.SelectionForeColor = Color.White;
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
                for (int i = 10; i <= Grid_Budget_PR.Columns.Count - 1; i++)
                {
                    if (Grid_Budget_PR[i, Grid_Budget_PR.CurrentCell.RowIndex].Style.BackColor != Color.Yellow)
                    {
                        Grid_Budget_PR[i, Grid_Budget_PR.CurrentCell.RowIndex].Style.BackColor = Color.Green;
                        Grid_Budget_PR[i, Grid_Budget_PR.CurrentCell.RowIndex].Style.ForeColor = Color.White;

                        Grid_Budget_PR[i, Grid_Budget_PR.CurrentCell.RowIndex].Style.SelectionBackColor = Color.Green;
                        Grid_Budget_PR[i, Grid_Budget_PR.CurrentCell.RowIndex].Style.SelectionForeColor = Color.White;
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
                Grid_Budget_PR.CurrentCell.Style.BackColor = Color.White;
                Grid_Budget_PR.CurrentCell.Style.ForeColor = Color.Black;

                Grid_Budget_PR.CurrentCell.Style.SelectionBackColor = Grid_Budget_PR[0, 0].Style.SelectionBackColor;
                Grid_Budget_PR.CurrentCell.Style.SelectionForeColor = Grid_Budget_PR[0, 0].Style.SelectionForeColor;

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
                if (Grid_Budget_PR.CurrentCell.Style.BackColor != Color.Yellow)
                {
                    Grid_Budget_PR.CurrentCell.Style.BackColor = Color.Green;
                    Grid_Budget_PR.CurrentCell.Style.ForeColor = Color.White;

                    Grid_Budget_PR.CurrentCell.Style.SelectionBackColor = Color.Green;
                    Grid_Budget_PR.CurrentCell.Style.SelectionForeColor = Color.White;
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
                Grid_Budget_PR.DataSource = null;
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
                Grid_Budget_PR.CurrentCell = Grid_Budget_PR["Access", 0];
                Grid_Budget_PR.Focus();
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
                if(MyParent.UserName == "MD" || MyParent.UserName == "ADMIN")
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
                    Grid_Budget_PR.CurrentCell = Grid_Budget_PR["Access", 0];
                    Grid_Budget_PR.Focus();
                    return;
                }


                if (MessageBox.Show ("Sure to Approve ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }

                Boolean For_Break = false;

                for (int i = 0; i <= Dt_Budget.Rows.Count - 1; i++)
                {
                    if (Grid_Budget_PR["Size", i].Value.ToString().Contains("**"))
                    {
                        if (!For_Break)
                        {
                            if (MessageBox.Show("** Cases available. Sure to Continue ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                            {
                                Grid_Budget_PR.CurrentCell = Grid_Budget_PR["Size", i];
                                Grid_Budget_PR.Focus();
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
                    for (int k = 10; k <= Dt_Budget.Columns.Count-1; k++)
                    {
                        if (Convert.ToInt32(Dt_Budget.Rows[i]["O_Slno"]) == 0)
                        {                            
                                if (Grid_Budget_PR[k, i].Style.BackColor == Color.Green)
                                {
                                    if (Convert.ToDouble(Grid_Budget_PR[k, i].Value.ToString()) != 0)
                                    {
                                        if (Convert.ToInt64(Dt_Budget.Rows[i]["ProcessID"]) < 0)
                                        {
                                            Queries[Array_Index++] = "Update Projects.Dbo.Project_Planning_Material_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name(), App_Pur_Rate_Conv = " + Convert.ToDouble(Grid_Budget_PR[k, i].Value.ToString()) + " Where RoWID In (Select Distinct PlanDtlID  From Projects.Dbo.Project_Planning_MAterial_Fn() Where Order_No = '" + Grid_Budget_PR.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget_PR["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget_PR["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget_PR["SizeID", i].Value.ToString() + "   and  Proj_ACtivity_ID = (" + Convert.ToInt64(Dt_Budget.Rows[i]["ProcessID"]) + " * -1)) ";
                                        }
                                        else
                                        {
                                            Queries[Array_Index++] = "Update Projects.Dbo.Project_Planning_Material_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name(), App_Pur_Rate_Conv = " + Convert.ToDouble(Grid_Budget_PR[k, i].Value.ToString()) + " Where RoWID In (Select Distinct PlanDtlID  From Projects.Dbo.Project_Planning_MAterial_Fn() Where Order_No = '" + Grid_Budget_PR.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget_PR["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget_PR["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget_PR["SizeID", i].Value.ToString() + ") ";
                                        }
                                        Queries[Array_Index++] = "Insert into Projects.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status, Rate) Values ('" + Grid_Budget_PR.Columns[k].Name.ToString() + "', " + Grid_Budget_PR["ProcessID", i].Value.ToString() + ", " + Grid_Budget_PR["ItemID", i].Value.ToString() + ", " + Grid_Budget_PR["ColorID", i].Value.ToString() + ", " + Grid_Budget_PR["SizeID", i].Value.ToString() + ", 'T', " + Convert.ToDouble(Grid_Budget_PR[k, i].Value.ToString()) + ")";
                                    }
                                }
                                else if(Grid_Budget_PR[k, i].Style.BackColor != Color.Yellow)
                                {
                                    if (Convert.ToDouble(Grid_Budget_PR[k, i].Value.ToString()) != 0)
                                    {
                                        //if (Convert.ToDouble(Dt_Budget_IDC.Rows[i][k].ToString()) > 1)
                                        //{
                                            Queries[Array_Index++] = "Update Projects.Dbo.Project_Planning_Material_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null, App_Pur_Rate_Conv = 0  Where RowID In (Select Distinct PlanDtlID  From Projects.Dbo.Project_Planning_MAterial_Fn() Where Order_No = '" + Grid_Budget_PR.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget_PR["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget_PR["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget_PR["SizeID", i].Value.ToString() + ") ";
                                        //}
                                        //else
                                        //{
                                        //    Queries[Array_Index++] = "Update Projects.Dbo.Project_Planning_Material_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null, App_Pur_Rate_Conv = 0  Where RowID In (" + Convert.ToDouble(Dt_Budget_ID.Rows[i][k].ToString()) + ") ";
                                        //}
                                        Queries[Array_Index++] = "Insert into Projects.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status, Rate) Values ('" + Grid_Budget_PR.Columns[k].Name.ToString() + "', " + Grid_Budget_PR["ProcessID", i].Value.ToString() + ", " + Grid_Budget_PR["ItemID", i].Value.ToString() + ", " + Grid_Budget_PR["ColorID", i].Value.ToString() + ", " + Grid_Budget_PR["SizeID", i].Value.ToString() + ", 'N', " + Convert.ToDouble(Grid_Budget_PR[k, i].Value.ToString()) + ")";
                                    }
                                }                           
                        }                        
                        else if (Convert.ToInt32(Dt_Budget.Rows[i]["O_Slno"]) == 4)
                        {                                                        
                                if (Grid_Budget_PR[k, i].Style.BackColor == Color.Green)
                                {
                                    if (Convert.ToDouble(Grid_Budget_PR[k, i].Value.ToString()) != 0)
                                    {
                                        //if (Convert.ToDouble(Dt_Budget_IDC.Rows[i][k].ToString()) > 1)
                                        //{
                                            Queries[Array_Index++] = "Update Projects.Dbo.Project_Planning_Process_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where  RoWID In (Select Distinct PlanDtlID  From Projects.Dbo.Project_Planning_Process_Fn() Where Order_No = '" + Grid_Budget_PR.Columns[k].Name.ToString() + "' and Proc_ID = " + Grid_Budget_PR["ProcessID", i].Value.ToString() + ")";
                                        //}
                                        //else
                                        //{
                                        //    Queries[Array_Index++] = "Update Projects.Dbo.Project_Planning_Process_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where  RoWID In (" + Convert.ToDouble(Dt_Budget_ID.Rows[i][k].ToString()) + ") ";
                                        //}
                                        Queries[Array_Index++] = "Insert into Projects.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget_PR.Columns[k].Name.ToString() + "', " + Grid_Budget_PR["ProcessID", i].Value.ToString() + ", " + Grid_Budget_PR["ItemID", i].Value.ToString() + ", " + Grid_Budget_PR["ColorID", i].Value.ToString() + ", " + Grid_Budget_PR["SizeID", i].Value.ToString() + ", 'T')";
                                    }
                                }
                                else if(Grid_Budget_PR[k, i].Style.BackColor != Color.Yellow)                            
                                {
                                    if (Convert.ToDouble(Grid_Budget_PR[k, i].Value.ToString()) != 0)
                                    {
                                        //if (Convert.ToDouble(Dt_Budget_IDC.Rows[i][k].ToString()) > 1)
                                        //{
                                            Queries[Array_Index++] = "Update Projects.Dbo.Project_Planning_Process_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null  Where  RoWID In (Select Distinct PlanDtlID  From Projects.Dbo.Project_Planning_Process_Fn() Where Order_No = '" + Grid_Budget_PR.Columns[k].Name.ToString() + "' and Proc_ID = " + Grid_Budget_PR["ProcessID", i].Value.ToString() + ") ";
                                        //}
                                        //else
                                        //{
                                        //    Queries[Array_Index++] = "Update Projects.Dbo.Project_Planning_Process_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null  Where  RoWID In (" + Convert.ToDouble(Dt_Budget_ID.Rows[i][k].ToString()) + ") ";
                                        //}
                                        Queries[Array_Index++] = "Insert into Projects.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget_PR.Columns[k].Name.ToString() + "', " + Grid_Budget_PR["ProcessID", i].Value.ToString() + ", " + Grid_Budget_PR["ItemID", i].Value.ToString() + ", " + Grid_Budget_PR["ColorID", i].Value.ToString() + ", " + Grid_Budget_PR["SizeID", i].Value.ToString() + ", 'N')";
                                    }
                                }                              
                            

                        }
                        else if (Convert.ToInt32(Dt_Budget.Rows[i]["O_Slno"]) == 7)
                        {
                            if (Grid_Budget_PR[k, i].Style.BackColor == Color.Green)
                            {
                                if (Convert.ToDouble(Grid_Budget_PR[k, i].Value.ToString()) != 0)
                                {
                                    //if (Convert.ToDouble(Dt_Budget_IDC.Rows[i][k].ToString()) > 1)
                                    //{
                                        Queries[Array_Index++] = "Update Projects.Dbo.Project_Planning_Comm_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where  RoWID In (Select Distinct PlanDtlID  From Projects.Dbo.Project_Planning_Comm_Fn() Where Order_No = '" + Grid_Budget_PR.Columns[k].Name.ToString() + "' and Comm_ID = " + Grid_Budget_PR["ProcessID", i].Value.ToString() + ") ";
                                    //}
                                    //else
                                    //{
                                    //    Queries[Array_Index++] = "Update Projects.Dbo.Project_Planning_Comm_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where  RoWID In (" + Convert.ToDouble(Dt_Budget_ID.Rows[i][k].ToString()) + ") ";
                                    //}
                                    Queries[Array_Index++] = "Insert into Projects.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget_PR.Columns[k].Name.ToString() + "', " + Grid_Budget_PR["ProcessID", i].Value.ToString() + ", " + Grid_Budget_PR["ItemID", i].Value.ToString() + ", " + Grid_Budget_PR["ColorID", i].Value.ToString() + ", " + Grid_Budget_PR["SizeID", i].Value.ToString() + ", 'T')";
                                }
                            }
                            else if(Grid_Budget_PR[k, i].Style.BackColor != Color.Yellow)                            
                            {
                                if (Convert.ToDouble(Grid_Budget_PR[k, i].Value.ToString()) != 0)
                                {
                                    //if (Convert.ToDouble(Dt_Budget_IDC.Rows[i][k].ToString()) > 1)
                                    //{
                                        Queries[Array_Index++] = "Update Projects.Dbo.Project_Planning_Comm_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null  Where  RoWID In (Select Distinct PlanDtlID  From Projects.Dbo.Project_Planning_Comm_Fn() Where Order_No = '" + Grid_Budget_PR.Columns[k].Name.ToString() + "' and Comm_ID = " + Grid_Budget_PR["ProcessID", i].Value.ToString() + ") ";
                                    //}
                                    //else
                                    //{
                                    //    Queries[Array_Index++] = "Update Projects.Dbo.Project_Planning_Comm_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null  Where  RoWID In (" + Convert.ToDouble(Dt_Budget_ID.Rows[i][k].ToString()) + ") ";
                                    //}
                                    Queries[Array_Index++] = "Insert into Projects.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget_PR.Columns[k].Name.ToString() + "', " + Grid_Budget_PR["ProcessID", i].Value.ToString() + ", " + Grid_Budget_PR["ItemID", i].Value.ToString() + ", " + Grid_Budget_PR["ColorID", i].Value.ToString() + ", " + Grid_Budget_PR["SizeID", i].Value.ToString() + ", 'N')";
                                }
                            }
                        }
                        else
                        {
                            if (Grid_Budget_PR[k, i].Style.BackColor == Color.Green || Grid_Budget_PR[k, i].Style.BackColor == Color.Yellow)
                            {
                                Queries[Array_Index++] = "Insert into Projects.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status, Rate) Values ('" + Grid_Budget_PR.Columns[k].Name.ToString() + "', " + Grid_Budget_PR["ProcessID", i].Value.ToString() + ", " + Grid_Budget_PR["ItemID", i].Value.ToString() + ", " + Grid_Budget_PR["ColorID", i].Value.ToString() + ", " + Grid_Budget_PR["SizeID", i].Value.ToString() + ", 'T', " + Convert.ToDouble(Grid_Budget_PR[k, i].Value.ToString()) + ")";
                            }
                            else
                            {
                                Queries[Array_Index++] = "Insert into Projects.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status, Rate) Values ('" + Grid_Budget_PR.Columns[k].Name.ToString() + "', " + Grid_Budget_PR["ProcessID", i].Value.ToString() + ", " + Grid_Budget_PR["ItemID", i].Value.ToString() + ", " + Grid_Budget_PR["ColorID", i].Value.ToString() + ", " + Grid_Budget_PR["SizeID", i].Value.ToString() + ", 'N', " + Convert.ToDouble(Grid_Budget_PR[k, i].Value.ToString()) + ")";
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

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Grid_Budget_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt == null)
                {
                    Txt = (TextBox)e.Control;
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
                if (Grid_Budget_PR.CurrentCell.ColumnIndex >= 10)
                {
                    if (Txt.Text == String.Empty)
                    {
                        Txt.Text = "0";
                    }
                    Grid_Budget_PR[Grid_Budget_PR.CurrentCell.ColumnIndex, Grid_Budget_PR.CurrentCell.RowIndex].Value = String.Format("{0:0.000}", Convert.ToDouble(Txt.Text));
                    Txt.Text = String.Format("{0:0.000}", Convert.ToDouble(Txt.Text));
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
                if (Grid_Budget_PR.CurrentCell.ColumnIndex >= 10)
                {
                    Mybase.Valid_Decimal(Txt, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /*private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if(MyParent.UserName == "MD" || MyParent.UserName == "ADMIN")
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

                Int32 Rows = (Dt_Budget.Rows.Count * (Dt_Budget.Columns.Count - 8)) * 3;

                String[] Queries = new string[Rows];
                Int32 Array_Index = 0;
                

                for (int i = 0; i <= Dt_Budget.Rows.Count - 1; i++)
                {
                    for (int k = 10; k <= Dt_Budget.Columns.Count-1; k++)
                    {
                        if (Convert.ToInt32(Dt_Budget.Rows[i]["O_Slno"]) == 0)
                        {                            
                                if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                                {
                                      Queries[Array_Index++] = "Insert Into FitSocks.Dbo.Socks_Yarn_BOM_Status (Planning_Master_ID, Planning_Detail_ID, Order_ID, Item_ID, Color_ID, Size_ID, BOM, Dyeing_Status)  Select Top 1000000000 RowID, PlanDtlID, OrdeR_ID, Item_ID, Color_ID, Size_ID, BOM_CONS, DYE_MODE  FRom (Select B.Planning_Detail_ID Bom_PlnDtlID , A.RowID, A.PlanDtlID, A.OrdeR_NO, A.OrdeR_ID, A.ItemID Item_ID, A.COLORID Color_ID, A.SIZEID Size_ID, CAst((A.BOM_CONS + A.LOSS_WEIGHT) as Numeric(30,3)) BOM_CONS, A.DYE_MODE, A.Spl_Req_Mode From FitSocks.Dbo.Socks_Yarn_Planning_Fn() A Left Join FitSocks.Dbo.Socks_Yarn_BOM_Status B On A.PlanDtlID = B.Planning_Detail_ID and A.OrdeR_ID = B.Order_ID and A.ItemID = B.Item_ID and A.SIZEID = B.Size_ID and A.COLORID = B.Color_ID Where B.Planning_Detail_ID Is null)A   Where Spl_Req_Mode = 'F' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + " ";
                                      Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Yarn_BOM_Status Set BOM = " + Dt_Budget_Qty.Rows[i][k] + " Where Planning_Detail_ID in (Select PlanDtlID  FRom (Select B.Planning_Detail_ID Bom_PlnDtlID , A.RowID, A.PlanDtlID, A.OrdeR_NO, A.OrdeR_ID, A.ItemID Item_ID, A.COLORID Color_ID, A.SIZEID Size_ID, CAst((A.BOM_CONS + A.LOSS_WEIGHT) as Numeric(30,3)) BOM_CONS, A.DYE_MODE, A.Spl_Req_Mode From FitSocks.Dbo.Socks_Yarn_Planning_Fn() A Left Join FitSocks.Dbo.Socks_Yarn_BOM_Status B On A.PlanDtlID = B.Planning_Detail_ID and A.OrdeR_ID = B.Order_ID and A.ItemID = B.Item_ID and A.SIZEID = B.Size_ID and A.COLORID = B.Color_ID )A   Where A.Spl_Req_Mode = 'F' and A.Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and A.Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and A.Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and A.Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + " and A.Bom_Cons != " + Dt_Budget_Qty.Rows[i][k] + ")";
                                      Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Spl_Req_Mode = 'F' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Dye_ITemID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                      Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                                }
                                else if(Grid_Budget[k, i].Style.BackColor != Color.Yellow)
                                {
                                      Queries[Array_Index++] = "Delete From FitSocks.Dbo.Socks_Yarn_BOM_Status  Where Spl_req_Mode = 'F' and Planning_Detail_ID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where  Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") and (Spec_Req + PO_Qty + GRN_Qty + Transfer_In + Transfer_Out + Supplier_Return + Prod_Issue) = 0";
                                      Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Spl_Req_mode = 'F' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";                              
                                      Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                                }                           
                        }
                        else if (Convert.ToInt32(Dt_Budget.Rows[i]["O_Slno"]) == 3)
                        {                                                        
                                if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                                {
                                      Queries[Array_Index++] = "Insert Into FitSocks.Dbo.Socks_Yarn_BOM_Status (Planning_Master_ID, Planning_Detail_ID, Order_ID, Item_ID, Color_ID, Size_ID, BOM, Dyeing_Status, Spl_REq_Mode)  Select Top 1000000000 RowID, PlanDtlID, OrdeR_ID, Item_ID, Color_ID, Size_ID, BOM_CONS, DYE_MODE, Spl_REq_Mode  FRom (Select B.Planning_Detail_ID Bom_PlnDtlID , A.RowID, A.PlanDtlID, A.OrdeR_NO, A.OrdeR_ID, A.ItemID Item_ID, A.COLORID Color_ID, A.SIZEID Size_ID, CAst((A.BOM_CONS) as Numeric(30,3)) BOM_CONS, A.DYE_MODE, 'T' Spl_REq_Mode From FitSocks.Dbo.Socks_Yarn_Planning_Fn() A Left Join FitSocks.Dbo.Socks_Yarn_BOM_Status B On A.PlanDtlID = B.Planning_Detail_ID and A.OrdeR_ID = B.Order_ID and A.ItemID = B.Item_ID and A.SIZEID = B.Size_ID and A.COLORID = B.Color_ID Where A.Spl_REq_Mode = 'T' and B.Planning_Detail_ID Is null)A  Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + " ";
                                      Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Yarn_BOM_Status Set BOM = " + Dt_Budget_Qty.Rows[i][k] + " Where Planning_Detail_ID in (Select PlanDtlID  FRom (Select B.Planning_Detail_ID Bom_PlnDtlID , A.RowID, A.PlanDtlID, A.OrdeR_NO, A.OrdeR_ID, A.ItemID Item_ID, A.COLORID Color_ID, A.SIZEID Size_ID, CAst((A.BOM_CONS) as Numeric(30,3)) BOM_CONS, A.DYE_MODE, A.Spl_Req_Mode From FitSocks.Dbo.Socks_Yarn_Planning_Fn() A Left Join FitSocks.Dbo.Socks_Yarn_BOM_Status B On A.PlanDtlID = B.Planning_Detail_ID and A.OrdeR_ID = B.Order_ID and A.ItemID = B.Item_ID and A.SIZEID = B.Size_ID and A.COLORID = B.Color_ID )A   Where A.Spl_Req_Mode = 'T' and A.Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and A.Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and A.Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and A.Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + " and A.Bom_Cons != " + Dt_Budget_Qty.Rows[i][k] + ")";
                                        if(Dt_Budget.Rows[i]["Access"].ToString().Contains("Others"))
                                        {
                                          Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Access_Type = 'OTHERS' and  Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                        }
                                        else if(Dt_Budget.Rows[i]["Access"].ToString().Contains("Rep"))
                                        {
                                          Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Access_Type = 'REPLACE' and Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                        }
                                        else if(Dt_Budget.Rows[i]["Access"].ToString().Contains("Add"))
                                        {
                                          Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Access_Type = 'EXCESS' and Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                        }
                                        else
                                        {
                                          Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                        }
                                      Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                                }
                                else if(Grid_Budget[k, i].Style.BackColor != Color.Yellow)
                                {
                                          Queries[Array_Index++] = "Delete From FitSocks.Dbo.Socks_Yarn_BOM_Status  Where Planning_Detail_ID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") and (Spec_Req + PO_Qty + GRN_Qty + Transfer_In + Transfer_Out + Supplier_Return + Prod_Issue) = 0";
                                            if(Dt_Budget.Rows[i]["Access"].ToString().Contains("Others"))
                                            {
                                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Access_Type = 'OTHERS' and Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";                              
                                            }
                                            else if(Dt_Budget.Rows[i]["Access"].ToString().Contains("Rep"))
                                            {
                                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Access_Type = 'REPLACE' and Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";                              
                                            }
                                            else if(Dt_Budget.Rows[i]["Access"].ToString().Contains("Add"))
                                            {
                                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Access_Type = 'EXCESS' and Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";                              
                                            }
                                            else 
                                            {
                                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Spl_REq_Mode = 'T' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";                              
                                            }

                                          Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                                }                             
                        }                                                                      
                        else if (Convert.ToInt32(Dt_Budget.Rows[i]["O_Slno"]) == 1)
                            {
                                if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                                {
                                    Queries[Array_Index++] = "Insert Into FitSocks.Dbo.Socks_Yarn_BOM_Status (Planning_Master_ID, Planning_Detail_ID, Order_ID, Item_ID, Color_ID, Size_ID, BOM, Dyeing_Status)  Select Top 1000000000 RowID, PlanDtlID, A.OrdeR_ID, A.Item_ID, A.Color_ID, A.Size_ID, BOM_CONS, DYE_MODE  FRom (Select RowID, PlanDtlID, Order_No, OrdeR_ID, ItemID ITEM_ID, DYE_ITEMID COLOR_ID , SIZEID SIZE_ID, CAst((BOM_CONS + LOSS_WEIGHT) as Numeric(30,3)) BOM_CONS, 'N' DYE_MODE From FitSocks.Dbo.Socks_Yarn_Planning_Fn () Where DYE_MODE = 'Y' and Spl_Req_Mode = 'F' )A Left Join FitSocks.Dbo.Socks_Yarn_BOM_Status B On A.OrdeR_ID = B.Order_ID and A.RowID = B.Planning_Master_ID and A.PlanDtlID = B.Planning_Detail_ID and B.Dyeing_Status = 'N'  Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and A.Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and A.Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and A.Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + " and B.Bom Is Null ";
                                    Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Yarn_BOM_Status Set BOM = " + Dt_Budget_Qty.Rows[i][k] + " Where Planning_Detail_ID in (Select PlanDtlID  FRom (Select B.Planning_Detail_ID Bom_PlnDtlID , A.RowID, A.PlanDtlID, A.OrdeR_NO, A.OrdeR_ID, A.ItemID Item_ID, A.DYE_ITEMID Color_ID, A.SIZEID Size_ID, CAst((A.BOM_CONS + A.LOSS_WEIGHT) as Numeric(30,3)) BOM_CONS, 'N' DYE_MODE  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() A Left Join FitSocks.Dbo.Socks_Yarn_BOM_Status B On A.PlanDtlID = B.Planning_Detail_ID and A.OrdeR_ID = B.Order_ID and A.ItemID = B.Item_ID and A.SIZEID = B.Size_ID and A.COLORID = B.Color_ID )A   Where A.Dye_Mode = 'N' and A.Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and A.Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and A.Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and A.Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + " and A.Bom_Cons != " + Dt_Budget_Qty.Rows[i][k] + ")";
                                    Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Spl_Req_Mode= 'F' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                    Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                                }
                                else if(Grid_Budget[k, i].Style.BackColor != Color.Yellow)
                                {
                                    Queries[Array_Index++] = "Delete From FitSocks.Dbo.Socks_Yarn_BOM_Status  Where Dyeing_Status = 'N' and Planning_Detail_ID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Spl_Req_Mode = 'F' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") and (Spec_Req + PO_Qty + GRN_Qty + Transfer_In + Transfer_Out + Supplier_Return + Prod_Issue) = 0";
                                    Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Spl_Req_Mode = 'F' and Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Dye_ItemID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";                              
                                    Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                                }                                                     
                            }
                        else if (Convert.ToInt32(Dt_Budget.Rows[i]["O_Slno"]) == 2)
                        {
                            if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                            {
                                Queries[Array_Index++] = "Insert Into FitSocks.Dbo.Socks_Trims_BOM_Status (Planning_Master_ID, Planning_Detail_ID, Order_ID, Item_ID, Color_ID, Size_ID, BOM, Dyeing_Status)  Select A.RowID, A.TrimDtlID, A.OrdeR_ID, A.Item_ID, A.COLOR_ID, A.SIZE_ID, A.Tot_Qty , 'N' DYE_MODE From FitSocks.Dbo.Socks_Trim_Planning_Fn() A Left Join FitSocks.Dbo.Socks_Trims_BOM_Status B On A.TrimDtlID = B.Planning_Detail_ID and A.OrdeR_ID = B.Order_ID and A.Item_ID = B.Item_ID and A.SIZE_ID = B.Size_ID and A.COLOR_ID = B.Color_ID Where  A.Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and A.Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and A.Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and A.Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + " and B.Planning_Master_ID IS null";
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Trim_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where RoWID In (Select Distinct TrimDtlID  From FitSocks.Dbo.Socks_Trim_Planning_Fn() Where  Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                            }
                            else if(Grid_Budget[k, i].Style.BackColor != Color.Yellow)                            
                            {
                                Queries[Array_Index++] = "Delete From FitSocks.Dbo.Socks_Trims_BOM_Status  Where Planning_Detail_ID  In (Select Distinct TrimDtlID  From FitSocks.Dbo.Socks_Trim_Planning_Fn() Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Trim_Details Set Approval_Flag = 'F' , Approval_Time = Null, Approval_System = Null   Where RoWID In (Select Distinct TrimDtlID  From FitSocks.Dbo.Socks_Trim_Planning_Fn() Where  Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                            }                         
                        }
                        else if (Convert.ToInt32(Dt_Budget.Rows[i]["O_Slno"]) == 4)
                        {
                            if(Dt_Budget.Rows[i]["ProcessID"].ToString() == "158")
                            {
                                if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                                {
                                    Queries[Array_Index++] = "Insert Into FitSocks.Dbo.Socks_Yarn_BOM_Status (Planning_Master_ID, Planning_Detail_ID, Order_ID, Item_ID, Color_ID, Size_ID, BOM, Dyeing_Status)  Select Top 1000000000 RowID, PlanDtlID, A.OrdeR_ID, A.Item_ID, A.Color_ID, A.Size_ID, BOM_CONS, DYE_MODE  FRom (Select RowID, PlanDtlID, Order_No, OrdeR_ID, ItemID ITEM_ID, ColorID COLOR_ID , SIZEID SIZE_ID, CAst((BOM_CONS + LOSS_WEIGHT) as Numeric(30,3)) BOM_CONS, 'Y' DYE_MODE From FitSocks.Dbo.Socks_Yarn_Planning_Fn () Where DYE_MODE = 'Y' )A Left Join FitSocks.Dbo.Socks_Yarn_BOM_Status B On A.OrdeR_ID = B.Order_ID and A.RowID = B.Planning_Master_ID and A.PlanDtlID = B.Planning_Detail_ID and B.Dyeing_Status = 'Y' Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and A.Item_ID = " + Grid_Budget["ItemID", i].Value.ToString() + " and A.Color_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and A.Size_ID = " + Grid_Budget["SizeID", i].Value.ToString() + " and B.Bom Is Null ";
                                    Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag_Dye = 'T' , Approval_Time_Dye = Getdate(), Approval_System_Dye = Host_Name() Where RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                    Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                                }
                                else if(Grid_Budget[k, i].Style.BackColor != Color.Yellow)                            
                                {
                                    Queries[Array_Index++] = "Delete From FitSocks.Dbo.Socks_Yarn_BOM_Status  Where Dyeing_Status = 'Y' and Planning_Detail_ID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ")";
                                    Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Yarn_Details Set Approval_Flag_Dye = 'F' , Approval_Time_Dye = Null, Approval_System_Dye = Null  Where RowID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Yarn_Planning_Fn() Where  Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and ItemID = " + Grid_Budget["ItemID", i].Value.ToString() + " and ColorID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";                              
                                    Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'N')";
                                }                             
                            }
                            else if(Dt_Budget.Rows[i]["ProcessID"].ToString() != "158")
                            {
                                if (Grid_Budget[k, i].Style.BackColor == Color.Green)
                                {
                                    Queries[Array_Index++] = "Update FitSocks.Dbo.Socks_Planning_Proc_Details Set Approval_Flag = 'T' , Approval_Time = Getdate(), Approval_System = Host_Name() Where  RoWID In (Select Distinct PlanDtlID  From FitSocks.Dbo.Socks_Process_Planning_Fn() Where Order_No = '" + Grid_Budget.Columns[k].Name.ToString() + "' and Proc_ID = " + Grid_Budget["ProcessID", i].Value.ToString() + " and Sample_ID = " + Grid_Budget["ColorID", i].Value.ToString() + " and SizeID = " + Grid_Budget["SizeID", i].Value.ToString() + ") ";
                                    Queries[Array_Index++] = "Insert into Vaahini_ERP_Gainup.Dbo.Budget_Approval_Status (Order_No, ProcessID, ItemID, ColorID, SizeID, Status) Values ('" + Grid_Budget.Columns[k].Name.ToString() + "', " + Grid_Budget["ProcessID", i].Value.ToString() + ", " + Grid_Budget["ItemID", i].Value.ToString() + ", " + Grid_Budget["ColorID", i].Value.ToString() + ", " + Grid_Budget["SizeID", i].Value.ToString() + ", 'T')";
                                }
                                else if(Grid_Budget[k, i].Style.BackColor != Color.Yellow)                            
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
                            else if(Grid_Budget[k, i].Style.BackColor != Color.Yellow)                            
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
                Queries[Array_Index++] = "Exec FitSocks.Dbo.Socks_Update_Yarn_Bom ";

                for (int p = 10; p <= Dt_Budget.Columns.Count-1; p++)
                {
                    Queries[Array_Index++] = "Exec FitSocks.Dbo.Socks_Yarn_Planning_Import_Proc '" + Grid_Budget.Columns[p].Name.ToString() + "'";
                    Queries[Array_Index++] = "Exec FitSocks.Dbo.Socks_Trim_Planning_Import_Proc '" + Grid_Budget.Columns[p].Name.ToString() + "'";
                    Queries[Array_Index++] = "Exec FitSocks.Dbo.Socks_Process_Planning_Import_Proc '" + Grid_Budget.Columns[p].Name.ToString() + "'";
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
        }  */
         
          
       

    }
}