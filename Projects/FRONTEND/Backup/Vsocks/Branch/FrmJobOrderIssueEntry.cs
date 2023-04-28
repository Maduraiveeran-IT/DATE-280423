using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Text;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmJobOrderIssueEntry : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        DataTable Dt = new DataTable();
        DataRow Dr;
        SelectionTool_Class Tool = new SelectionTool_Class();
        TextBox Txt = null;
        Int64 Code = 0;

        public FrmJobOrderIssueEntry()
        {
            InitializeComponent();
        }

        private void FrmJobOrderIssueEntry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Total_Qty()
        {
            try
            {
                TxtTotalQty.Text = String.Format ("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "JO_QTY", "Order_No", "PO_No", "SAMPLE_NO")));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Grid_Data()
        {
            try
            {
                if (MyParent._New)
                {
                    MyBase.Load_Data("Select 0 As SL, (Order_No + '-' + PO_No + '-' + Sample_No) Description, Order_ID, Order_No ORDER_NO, PO_No PO_NO, Sample_ID, Sample_No SAMPLE_NO, BOM_Qty, JO_Issued_Qty Issued_Qty, Bal_Qty, Bal_Qty JO_QTY, '' T From Socks_JobOrder_Pending () Where 1 = 2", ref Dt);
                }
                else
                {
                    MyBase.Load_Data("Select S2.SlNo SL, (S1.Order_No + '-' + S1.PO_No + '-' + S1.Sample_No) Description, S2.Order_ID, S1.Order_No ORDER_NO, S1.PO_No PO_NO, S2.Sample_ID, S1.Sample_No SAMPLE_NO, S1.BOM_Qty, (S1.JO_Issued_Qty - S2.JO_Qty) Issued_Qty, (S1.BOM_Qty - (S1.JO_Issued_Qty - S2.JO_Qty)) Bal_Qty, S2.JO_QTY, '' T From Socks_JobOrder_Pending () S1 Inner Join Socks_JobOrder_Details S2 on S1.Order_ID = S2.Order_ID and S1.Sample_ID = S2.Sample_ID and S1.Po_No = S2.Po_No Where S2.Master_ID = " + Code, ref Dt);
                }
                Grid.DataSource = Dt;
                MyBase.Grid_Designing(ref Grid, ref Dt, "Order_ID", "Sample_ID", "Description", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Order_No", "JO_QTY");
                MyBase.Grid_Colouring (ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width (ref Grid, 40, 120, 100, 100, 90, 90, 90, 90);

                Grid.Columns["BOM_Qty"].HeaderText = "BOM";
                Grid.Columns["Issued_Qty"].HeaderText = "ISSUED";
                Grid.Columns["BAL_QTY"].HeaderText = "BAL";
                Grid.Columns["JO_QTY"].HeaderText = "JOQTY";

                Grid.Columns["BOM_Qty"].DefaultCellStyle.Format = "0";
                Grid.Columns["Issued_Qty"].DefaultCellStyle.Format = "0";
                Grid.Columns["BAL_QTY"].DefaultCellStyle.Format = "0";
                Grid.Columns["JO_QTY"].DefaultCellStyle.Format = "0";

                Grid.RowHeadersWidth = 10;

                MyBase.Row_Number(ref Grid);
                Total_Qty();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Load_Combo()
        {
            try
            {
                CmbIssueType.Items.Clear();
                CmbIssueType.Items.Add("Internal");
                CmbIssueType.Items.Add("Supplier");
                CmbIssueType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                TxtBuyer.Enabled = true;
                JONO_Generate();
                Load_Combo();
                Grid_Data();
                Code = 0;
                TxtBuyer.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void JONO_Generate()
        {
            try
            {
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select DBo.Get_Max_JobOrder ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "')", ref Tdt);
                TxtJONo.Text = Tdt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                TxtJONo.Text = String.Empty;
                throw ex;
            }
        }

        public void Entry_Save()
        {
            try
            {
                if (TxtBuyer.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Buyer ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtBuyer.Focus();
                    return;
                }
                if (TxtUnit.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Unit ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtUnit.Focus();
                    return;
                }

                JONO_Generate();

                if (TxtJONo.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid JobOrder No ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtUnit.Focus();
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Data's ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtUnit.Focus();
                    return;
                }

                Total_Qty();

                if (TxtTotalQty.Text.Trim() == String.Empty || Convert.ToInt32(TxtTotalQty.Text) == 0)
                {
                    MessageBox.Show("Invalid Data's ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtUnit.Focus();
                    return;
                }

                RowsCount();

                String[] Queries = new String[Dt.Rows.Count + 2];
                Int32 Array_Index = 0;
                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_JobOrder_Master (JoNo, JODate, Buyer_ID, Issue_Type, Unit_Code) Values ('" + TxtJONo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtBuyer.Tag.ToString() + ", " + CmbIssueType.SelectedIndex + ", " + TxtUnit.Tag.ToString() + "); Select Scope_Identity ()";
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_JobOrder_Master Set Issue_Type = " + CmbIssueType.SelectedIndex + ", Unit_Code = " + TxtUnit.Tag.ToString() + " Where RowID = " + Code;
                    Queries[Array_Index++] = "Delete from Socks_JobOrder_Details Where Master_ID = " + Code;

                }

                Grid.Refresh();

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Socks_JobOrder_Details (Master_ID, Slno, Order_ID, Sample_ID, JO_Qty, Po_No) Values (@@IDENTITY, " + (i + 1) + ",  " + Dt.Rows[i]["Order_ID"].ToString() + ", " + Dt.Rows[i]["Sample_ID"].ToString() + ", " + Dt.Rows[i]["JO_Qty"].ToString() + ", '" + Dt.Rows[i]["PO_NO"].ToString() + "')";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Socks_JobOrder_Details (Master_ID, Slno, Order_ID, Sample_ID, JO_Qty, Po_No) Values (" + Code + ", " + (i + 1) + ",  " + Dt.Rows[i]["Order_ID"].ToString() + ", " + Dt.Rows[i]["Sample_ID"].ToString() + ", " + Dt.Rows[i]["JO_Qty"].ToString() + ", '" + Dt.Rows[i]["PO_NO"].ToString() + "')";
                    }
                }

                MyBase.Run_Identity (MyParent.Edit, Queries);
                MessageBox.Show("Saved ...!", "Gainup");
                MyBase.Clear(this);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MyParent.Save_Error = true;
            }
        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Job Order - Edit", "Select S1.JONo, S1.JoDate, L1.Ledger_Name Buyer, Isnull (C1.Party, C2.Company_Unit) Unit, S3.Order_NO, S3.Sample_No, S2.JO_Qty,  S1.Buyer_ID, S1.Issue_Type, S1.Unit_Code, S1.RowID Code From Socks_JobOrder_Master S1 Inner Join Socks_JobOrder_Details S2 on S1.RowID = S2.Master_ID Left Join Accounts.DBo.Ledger_Master L1 on S1.Buyer_ID = L1.Ledger_Code and L1.Company_Code = 1 and L1.Year_Code = '2015-2016' Left Join Accounts.Dbo.Creditors (1, '2015-2016') C1 on S1.Unit_Code = C1.Code Left join company_unit C2 On S1.Unit_Code = C2.company_unitid Left join Socks_Order_BOM () S3 on S2.Order_ID = S3.Order_ID and S2.Sample_ID = S3.Sample_ID and S2.Po_No = S3.Po_No Where Isnull(S1.Print_Out_taken,'N') = 'N' ", String.Empty, 120, 100, 250, 250, 120, 100, 90);
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

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                TxtBuyer.Enabled = false;
                Code = Convert.ToInt64(Dr["Code"]);
                TxtJONo.Text = Dr["JONo"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["JoDate"]);
                TxtBuyer.Tag = Dr["Buyer_ID"].ToString();
                TxtBuyer.Text = Dr["Buyer"].ToString();
                CmbIssueType.SelectedIndex = Convert.ToInt32(Dr["Issue_Type"]);
                TxtUnit.Text = Dr["Unit"].ToString();
                TxtUnit.Tag = Dr["Unit_Code"].ToString();
                Grid_Data();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, false);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Job Order - Delete", "Select S1.JONo, S1.JoDate, L1.Ledger_Name Buyer, Isnull (C1.Party, C2.Company_Unit) Unit, S3.Order_NO, S3.Sample_No, S2.JO_Qty,  S1.Buyer_ID, S1.Issue_Type, S1.Unit_Code, S1.RowID Code From Socks_JobOrder_Master S1 Inner Join Socks_JobOrder_Details S2 on S1.RowID = S2.Master_ID Left Join Accounts.DBo.Ledger_Master L1 on S1.Buyer_ID = L1.Ledger_Code and L1.Company_Code = 1 and L1.Year_Code = '2015-2016' Left Join Accounts.Dbo.Creditors (1, '2015-2016') C1 on S1.Unit_Code = C1.Code Left join company_unit C2 On S1.Unit_Code = C2.company_unitid Left join Socks_Order_BOM () S3 on S2.Order_ID = S3.Order_ID and S2.Sample_ID = S3.Sample_ID  and S2.Po_No = S3.Po_No Where Isnull(S1.Print_Out_taken,'N') = 'N' ", String.Empty, 120, 100, 250, 250, 120, 100, 90);
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
                    MyBase.Run("Delete from Socks_JobOrder_Details Where Master_ID = " + Code, "Delete from Socks_Joborder_Master Where RowID = " + Code);
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
                MyBase.Enable_Controls(this, false);
                Load_Combo();
                if (MyParent.UserCode == 19)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Job Order - View", "Select S1.JONo, S1.JoDate, L1.Ledger_Name Buyer, Isnull (C1.Party, C2.Company_Unit) Unit, S3.Order_NO, S3.Sample_No, S2.JO_Qty,  S1.Buyer_ID, S1.Issue_Type, S1.Unit_Code, S1.RowID Code From Socks_JobOrder_Master S1 Inner Join Socks_JobOrder_Details S2 on S1.RowID = S2.Master_ID Left Join Accounts.DBo.Ledger_Master L1 on S1.Buyer_ID = L1.Ledger_Code and L1.Company_Code = 1 and L1.Year_Code = '2015-2016' Left Join Accounts.Dbo.Creditors (1, '2015-2016') C1 on S1.Unit_Code = C1.Code Left join company_unit C2 On S1.Unit_Code = C2.company_unitid Left join Socks_Order_BOM () S3 on S2.Order_ID = S3.Order_ID and S2.Sample_ID = S3.Sample_ID and S2.Po_No = S3.Po_No Where S1.print_Out_Taken = 'Y'", String.Empty, 120, 100, 250, 250, 120, 100, 90);
                }
                else
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Job Order - View", "Select S1.JONo, S1.JoDate, L1.Ledger_Name Buyer, Isnull (C1.Party, C2.Company_Unit) Unit, S3.Order_NO, S3.Sample_No, S2.JO_Qty,  S1.Buyer_ID, S1.Issue_Type, S1.Unit_Code, S1.RowID Code From Socks_JobOrder_Master S1 Inner Join Socks_JobOrder_Details S2 on S1.RowID = S2.Master_ID Left Join Accounts.DBo.Ledger_Master L1 on S1.Buyer_ID = L1.Ledger_Code and L1.Company_Code = 1 and L1.Year_Code = '2015-2016' Left Join Accounts.Dbo.Creditors (1, '2015-2016') C1 on S1.Unit_Code = C1.Code Left join company_unit C2 On S1.Unit_Code = C2.company_unitid Left join Socks_Order_BOM () S3 on S2.Order_ID = S3.Order_ID and S2.Sample_ID = S3.Sample_ID and S2.Po_No = S3.Po_No", String.Empty, 120, 100, 250, 250, 120, 100, 90);
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

        public void Entry_Print()
        {
            try
            {
                try
                {
                    String Str;
                    DataTable Dt2 = new DataTable();
                    DataTable Dt3 = new DataTable();
                    DataTable Dt4 = new DataTable();

                    Str = "Select * From Socks_JobOrder_Details A Inner Join VFit_Sample_Master A1 On A.Sample_ID = A1.RowID Inner Join Socks_Planning_Master B On A.Order_ID = B.Order_ID and B.Item_ID = A1.SampleItemID ";
                    Str = Str + " Inner Join Socks_Planning_Proc_Details C On B.RowID = C.Master_ID And C.Approval_Flag = 'F' and C.Proc_ID = 152 Where A.Master_ID = " + Code;
                    MyBase.Load_Data(Str, ref Dt3);

                    if (Dt3.Rows.Count > 0)
                    {
                        MessageBox.Show("Pls Get Budget Approval");
                        return;
                    }

                    DataTable Dt5 = new DataTable();
                    Str = " Select * From Socks_JobOrder_Master Where print_Out_Taken = 'Y' And RowID = " + Code + "";
                    MyBase.Load_Data(Str, ref Dt5);
                    {
                        Str = "Select * From Check_job_Order_Stock_New(" + Code + ")";
                        MyBase.Load_Data(Str, ref Dt4);

                        if (Dt4.Rows.Count > 0)
                        {
                            MessageBox.Show("Stock Not Available");
                            return;
                        }
                    }
                    //Str = " Select Top 10000000000000 S1.JONo, S1.JoDate, L1.Ledger_Name Buyer, Isnull (C1.Party, C2.Company_Unit) Unit, S3.Order_NO, S3.Sample_No, S3.Po_No, S2.JO_Qty,  S1.Buyer_ID, S1.Issue_Type, S1.Unit_Code, S1.RowID Code, L1.Ledger_Address Supplier_Address, L1.Ledger_Phone Supplier_Phone, L1.Ledger_email Supplier_Email From Socks_JobOrder_Master S1 Inner Join Socks_JobOrder_Details S2 on S1.RowID = S2.Master_ID Left Join Accounts.DBo.Ledger_Master L1 on S1.Buyer_ID = L1.Ledger_Code and L1.Company_Code = 1 and L1.Year_Code = '"  + MyParent.YearCode  + "' Left Join Accounts.Dbo.Creditors (1, '" + MyParent.YearCode + "') C1 on S1.Unit_Code = C1.Code Left join company_unit C2 On S1.Unit_Code = C2.company_unitid Left join Socks_Order_BOM () S3 on S2.Order_ID = S3.Order_ID and S2.Sample_ID = S3.Sample_ID  Where S1.RowID = " + Code + " Order By S3.Order_NO, S3.Sample_No";

                    Str = " Select Top 10000000000000 S1.JONo, S1.JoDate, L1.Ledger_Name Buyer, Isnull (C1.Party, C2.Company_Unit) Unit, S3.Order_NO, S3.Sample_No, S3.Po_No, S2.JO_Qty  JO_Qty,  S1.Buyer_ID, S1.Issue_Type, S1.Unit_Code, S1.RowID Code, L1.Ledger_Address Supplier_Address, ";
                    Str = Str + " L1.Ledger_Phone Supplier_Phone, L1.Ledger_email Supplier_Email, I1.Item, S4.Sample_No + ' - ' + S4.Remarks + ' - ' + N1.Name + ' - ' + Cast(Cast(S4.Weight As Numeric(20))As Varchar(20)) + ' - ' + 'Grams' Color, S5.Size, S2.JO_Qty QTY, S6.Model_Name, S3.Image1 ";
                    Str = Str + " From Socks_JobOrder_Master S1 Inner Join Socks_JobOrder_Details S2 on S1.RowID = S2.Master_ID Left Join Accounts.DBo.Ledger_Master L1 on S1.Buyer_ID = L1.Ledger_Code and L1.Company_Code = 1 and L1.Year_Code = '2015-2016' ";
                    Str = Str + " Left Join Accounts.Dbo.Creditors (1, '2015-2016') C1 on S1.Unit_Code = C1.Code Left join company_unit C2 On S1.Unit_Code = C2.company_unitid Left join Socks_Order_BOM () S3 on S2.Order_ID = S3.Order_ID and S2.Sample_ID = S3.Sample_ID and S2.Po_No = S3.Po_No";
                    Str = Str + " Left Join VFit_Sample_Master S4 On S2.Sample_ID = S4.RowID Left Join VFit_Sample_Needle_Master N1 On S4.NeedleID = N1.RowID Left Join Garment_UOM G1 On S4.UOM3 = G1.GUOMID ";
                    Str = Str + " Left Join Item I1 On S4.SampleItemID = I1.ItemID Left Join Size S5 On S4.SizeID = S5.SizeID Left Join Socks_Model S6 On S4.ModelID = S6.Rowid Where S1.RowID = " + Code + " Order By S3.Order_NO, S3.Sample_No ";
                    MyBase.Execute_Qry(Str, "RptJobOrderIssue");

                    Str = " Select Top 1000000 ROW_NUMBER()Over(Order By Item_Group, Particulars, Cons, Uom)Slno, Item_Group, Particulars, Cons, Uom From VSocks_Samplewise(" + Code + ") Order By Item_Group, Particulars, Cons, Uom ";
                    MyBase.Execute_Qry(Str, "Vsocks_Sample_Cons");

                    String Str1 = " Select getdate()PDate";
                    MyBase.Load_Data(Str1, ref Dt3);

                    MyBase.Run("Update Socks_JobOrder_Master Set Print_Out_taken = 'Y' Where RowID = " + Code);

                    CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RptJobOrderIssue.rpt");
                    MyParent.FormulaFill(ref ObjRpt, "Heading", "JOB ORDER SHEET");
                    MyParent.FormulaFill(ref ObjRpt, "PDate", Dt3.Rows[0]["PDate"].ToString());
                    MyParent.FormulaFill(ref ObjRpt, "IssueType", CmbIssueType.Text.ToString());
                    MyParent.CReport(ref ObjRpt, "JOB ORDER ISSUE..!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmJobOrderIssueEntry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtUnit")
                    {
                        Grid.CurrentCell = Grid["Order_No", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                    }
                    else if (this.ActiveControl.Name == "TxtTotalQty")
                    {
                        if (MyParent._New || MyParent.Edit)
                        {
                            MyParent.Load_SaveEntry();
                        }
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtUnit")
                    {
                        if (CmbIssueType.Text.ToUpper() == "INTERNAL")
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Unit", "Select company_unit Unit, company_unitid Code From company_unit", String.Empty, 300);
                            if (Dr != null)
                            {
                                TxtUnit.Text = Dr["Unit"].ToString();
                                TxtUnit.Tag = Dr["Code"].ToString();
                            }
                        }
                        else
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Party", "Select Party, Code From Accounts.Dbo.Creditors (" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Where Party Not like 'ZZZ%'", String.Empty, 300);
                            if (Dr != null)
                            {
                                TxtUnit.Text = Dr["Party"].ToString();
                                TxtUnit.Tag = Dr["Code"].ToString();
                            }
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtBuyer")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer", "Select Party Buyer, Code From Accounts.Dbo.Debtors (" + MyParent.CompCode + ", '" + MyParent.YearCode + "')", String.Empty, 300);
                        if (Dr != null)
                        {
                            TxtBuyer.Text = Dr["Buyer"].ToString();
                            TxtBuyer.Tag = Dr["Code"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmJobOrderIssueEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == String.Empty)
                {
                }
                else
                {
                    if (this.ActiveControl is TextBox)
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

        private void CmbIssueType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TxtUnit.Text = String.Empty;
                TxtUnit.Tag = String.Empty;
                TxtUnit.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtUnit_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtJONo_TextChanged(object sender, EventArgs e)
        {

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

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["JO_QTY"].Index)
                {
                    MyBase.Valid_Number(Txt, e);
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

        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_NO"].Index)
                    {
                        Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Order", "Select Order_No, PO_No, Sample_NO, BOM_Qty, JO_Issued_Qty Issued, Bal_Qty, Order_ID, Sample_ID, (Order_No + '-' + PO_No + '-' + Sample_NO) Description From Socks_JobOrder_Pending () Where Buyer_ID = " + TxtBuyer.Tag.ToString() + " and Bal_Qty > 0", String.Empty, 100, 100, 80, 80, 80, 80);
                        if (Dr != null)
                        {
                            MyBase.Row_Number(ref Grid);
                            Txt.Text = Dr["Order_No"].ToString();
                            Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                            Grid["Order_ID", Grid.CurrentCell.RowIndex].Value = Dr["Order_ID"].ToString();
                            Grid["Sample_No", Grid.CurrentCell.RowIndex].Value = Dr["Sample_No"].ToString();
                            Grid["Sample_ID", Grid.CurrentCell.RowIndex].Value = Dr["Sample_ID"].ToString();
                            Grid["PO_NO", Grid.CurrentCell.RowIndex].Value = Dr["PO_NO"].ToString();
                            Grid["Description", Grid.CurrentCell.RowIndex].Value = Dr["Description"].ToString();
                            Grid["BOM_Qty", Grid.CurrentCell.RowIndex].Value = Dr["BOM_Qty"].ToString();
                            Grid["Issued_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Issued"].ToString();
                            Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bal_Qty"].ToString();
                            Grid["JO_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bal_Qty"].ToString();
                        }
                    }
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["JO_QTY"].Index)
                    {
                        if (Grid["JO_Qty", Grid.CurrentCell.RowIndex].Value == null || Grid["JO_Qty", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["JO_Qty", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["JO_Qty", Grid.CurrentCell.RowIndex].Value = "0";
                        }

                        if (Convert.ToDouble(Grid["JO_Qty", Grid.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid JO Qty ...!", "Gainup");
                            Grid["JO_Qty", Grid.CurrentCell.RowIndex].Value = Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["JO_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (Convert.ToDouble(Grid["JO_Qty", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show ("JO Qty is greater than Bal Qty ...!", "Gainup");
                            Grid["JO_Qty", Grid.CurrentCell.RowIndex].Value = Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["JO_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit (true);
                            return;
                        }

                        Total_Qty();

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
                    e.Handled = true;
                    Total_Qty();
                    TxtTotalQty.Focus();
                }
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
                Total_Qty();
                MyBase.Row_Number(ref Grid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Leave(object sender, EventArgs e)
        {

        }
        void RowsCount()
        {
            try
            {
                if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    Grid.CurrentCell = Grid["Order_No", Grid.CurrentCell.RowIndex + 1];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
                if (Grid["Jo_Qty", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    Grid.CurrentCell = Grid["Order_No", Grid.CurrentCell.RowIndex + 1];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}