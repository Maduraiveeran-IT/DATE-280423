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
    public partial class FrmGeneralPo : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataGrid dgv = new DataGrid();
        DataRow Dr;
        Int64 Code;
        Int32 C = 0;
        TextBox Txt = null;
        TextBox Txt_Img = null;
        DataTable Dt_Tax = new DataTable();
        TextBox Txt_Tax = null;
        Int32 Max_Val = 80;
        DataTable[] DtImg;
        String[] Queries;
        String Str;
        public FrmGeneralPo()
        {
            InitializeComponent();
        }

        private void FrmGeneralPo_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                TxtSupplier.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmGeneralPo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;

                    if (this.ActiveControl.Name == "DtpRDate")
                    {
                        TxtSupplier.Focus();
                    }
                    else if (this.ActiveControl.Name == "TxtSupplier")
                    {
                        if (TxtSupplier.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Select Supplier..!", "Gainup");
                            return;
                        }
                        else
                        {
                            Grid.CurrentCell = Grid["Description", 0];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;

                        }
                    }
                    else if (this.ActiveControl.Name == "TxtRemarks")
                    {
                        TxtTotal.Focus();
                    }
                    else if (this.ActiveControl.Name == "TxtTotal")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }

                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtSupplier")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "Select LedgeR_Name Supplier, LedgeR_Code SupplierId From Supplier_All_Fn() Where LEdgeR_code != 793 ", String.Empty, 250);

                        if (Dr != null)
                        {
                            TxtSupplier.Text = Dr["Supplier"].ToString();
                            TxtSupplier.Tag = Dr["SupplierID"].ToString();
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
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                DtpDate.Value = MyBase.GetServerDate();
                TxtSupplier.Focus();
                Grid_Data();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Cancel()
        {
            MyBase.Clear(this);
        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select General Po - Edit", " Select A.Entry_No, A.EDate, A.Req_Date, Isnull(G.Supplier,F.Supplier)Supplier, C.Item, D.Color, E.Size, B.Po_Qty, B.rate, A.Remarks, A.Rowid, A.Supplierid from VSocks_General_Po_Master A Left Join VSocks_General_Po_Details B on A.Rowid = B.MasterID Left Join Item C on B.Itemid = C.Itemid Left Join Color D on B.Colorid = D.Colorid Left Join Size E on B.Sizeid = E.Sizeid Left Join Supplier F on A.Supplierid = F.Acc_Ledger_Code Left Join Supplier G on A.SupplierID = G.supplierid", String.Empty, 75, 100, 100, 150, 110, 110, 75, 110, 110, 150);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Description", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
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
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select General Po - View", " Select A.Entry_No, A.EDate, A.Req_Date, Isnull(G.Supplier,F.Supplier)Supplier, C.Item, D.Color, E.Size, B.Po_Qty, B.rate, A.Remarks, A.Rowid, A.Supplierid from VSocks_General_Po_Master A Left Join VSocks_General_Po_Details B on A.Rowid = B.MasterID Left Join Item C on B.Itemid = C.Itemid Left Join Color D on B.Colorid = D.Colorid Left Join Size E on B.Sizeid = E.Sizeid Left Join Supplier F on A.Supplierid = F.Acc_Ledger_Code Left Join Supplier G on A.SupplierID = G.supplierid", String.Empty, 75, 100, 100, 150, 110, 110, 75, 110, 110, 150);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Description", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
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
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select General Po - Delete", " Select A.Entry_No, A.EDate, A.req_Date, Isnull(G.Supplier,F.Supplier)Supplier, C.Item, D.Color, E.Size, B.Po_Qty, B.rate, A.Remarks, A.Rowid, A.Supplierid from VSocks_General_Po_Master A Left Join VSocks_General_Po_Details B on A.Rowid = B.MasterID Left Join Item C on B.Itemid = C.Itemid Left Join Color D on B.Colorid = D.Colorid Left Join Size E on B.Sizeid = E.Sizeid Left Join Supplier F on A.Supplierid = F.Acc_Ledger_Code Left Join Supplier G on A.SupplierID = G.supplierid", String.Empty, 75, 100, 100, 150, 110, 110, 75, 110, 110, 150);
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

                    MyBase.Run("Delete from VSocks_General_Po_Tax_Details where MasterID = " + Code, "Delete from VSocks_General_Po_Details where MasterID = " + Code, "Delete from VSocks_General_Po_Master Where Rowid = " + Code, MyParent.EntryLog("General Po", "DELETE", Code.ToString()));                    
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
        public void Entry_Print()
        {
            try
            {
                String Str, Str1, Str2, Str3, Str4;
                String Order = "";
                DataTable Dt1 = new DataTable();
                DataTable Dt2 = new DataTable();
                DataTable Dt3 = new DataTable();
                DataTable Dt4 = new DataTable();

                Str = " Select S1.Entry_No PONo, L1.Ledger_Name Supplier, Cast(S1.EDate As date)PoDate, S1.Req_Date Required_Date, 'GENERAL' PO_Method, L1.Ledger_Address Supplier_Address, L1.Ledger_Phone Supplier_Phone, L1.Ledger_email Supplier_Email From VSocks_General_Po_Master S1 left Join Supplier_All_Fn() L1 On L1.LEdgeR_Code = S1.SupplierID  Where S1.Approval_Flag = 'T' and S1.RowID = " + Code;
                MyBase.Load_Data(Str, ref Dt1);


                if (Dt1.Rows.Count <= 0)
                {
                    MessageBox.Show("PO Not Approved...!", "Gainup");
                    return;
                }

                DialogResult Res = MessageBox.Show("[Y] - Print; [N] - Mail; Sure to Continue ..?", "Gainup", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                Str1 = " Select Top 100000000 ROW_NUMBER()Over(Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size)SlNo, I1.Item + ' - ' + C1.color + ' - ' + S4.Size Item_Color_Size, S2.Po_Qty Order_Qty, S2.Rate, (S2.Po_Qty*S2.Rate) Amount From VSocks_General_Po_Master S1 Inner join VSocks_General_Po_Details S2 ON S1.RowID = s2.MasterID Inner join item I1 on S2.Itemid = I1.itemid Inner join color C1 on s2.Colorid = c1.colorid Inner join size S4 on s2.SizeID = S4.sizeid Where S1.RowID = " + Code + " Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size ";
                MyBase.Execute_Qry(Str1, "Socks_General_PO");

                Str2 = " Select Top 2 S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From VSocks_General_Po_Tax_Details S1 Left Join Accounts.dbo.Ledger_Master L1 on S1.Tax_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' Where S1.Master_ID = " + Code + " Order by S1.Slno ";
                MyBase.Load_Data(Str2, ref Dt2);

                Str3 = " Select Distinct 'General' Order_No From VSocks_General_Po_Master S1  Where S1.RowID = " + Code;
                MyBase.Load_Data(Str3, ref Dt3);

                Str4 = " Select Getdate()PrintOutDate";
                MyBase.Load_Data(Str4, ref Dt4);

                if (Dt3.Rows.Count > 0)
                {
                    for (int i = 0; i <= Dt3.Rows.Count - 1; i++)
                    {
                        if (Order.ToString() == String.Empty)
                        {
                            Order = Dt3.Rows[i]["Order_No"].ToString();
                        }
                        else
                        {
                            Order = Order + ", " + Dt3.Rows[i]["Order_No"].ToString();
                        }
                    }
                }

                CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RptGeneralPurchasePO.rpt");
                MyParent.FormulaFill(ref ObjRpt, "Heading", "GENERAL PURCHASE ORDER");
                
                MyParent.FormulaFill(ref ObjRpt, "Supplier", Dt1.Rows[0]["Supplier"].ToString());
                MyParent.FormulaFill(ref ObjRpt, "Supplier_Address", Dt1.Rows[0]["Supplier_Address"].ToString().Replace("\r\n", "__"));
                MyParent.FormulaFill(ref ObjRpt, "Supplier_Phone", Dt1.Rows[0]["Supplier_Phone"].ToString());
                MyParent.FormulaFill(ref ObjRpt, "Supplier_Email", Dt1.Rows[0]["Supplier_Email"].ToString());
                MyParent.FormulaFill(ref ObjRpt, "PONo", Dt1.Rows[0]["PONo"].ToString());
                MyParent.FormulaFill(ref ObjRpt, "PoDate", Dt1.Rows[0]["PoDate"].ToString());
                MyParent.FormulaFill(ref ObjRpt, "ReqDate", Dt1.Rows[0]["Required_Date"].ToString());
                MyParent.FormulaFill(ref ObjRpt, "PO_Method", Dt1.Rows[0]["PO_Method"].ToString());
                MyParent.FormulaFill(ref ObjRpt, "PrintOutDate", Dt4.Rows[0]["PrintOutDate"].ToString());
                if (Dt2.Rows.Count > 0)
                {
                    for (int i = 0; i <= Dt2.Rows.Count - 1; i++)
                    {
                        if (i == 0)
                        {
                            MyParent.FormulaFill(ref ObjRpt, "Tax1", Dt2.Rows[0]["Tax"].ToString());
                            MyParent.FormulaFill(ref ObjRpt, "Tax1_Per", Dt2.Rows[0]["Tax_Per"].ToString());
                            MyParent.FormulaFill(ref ObjRpt, "Tax1_Amount", Dt2.Rows[0]["Tax_Amount"].ToString());
                        }
                        else if (i == 1)
                        {
                            MyParent.FormulaFill(ref ObjRpt, "Tax2", Dt2.Rows[1]["Tax"].ToString());
                            MyParent.FormulaFill(ref ObjRpt, "Tax2_Per", Dt2.Rows[1]["Tax_Per"].ToString());
                            MyParent.FormulaFill(ref ObjRpt, "Tax2_Amount", Dt2.Rows[1]["Tax_Amount"].ToString());
                        }
                    }
                }
                MyParent.FormulaFill(ref ObjRpt, "Net_Amount", TxtTotal.Text.ToString());
                MyParent.FormulaFill(ref ObjRpt, "Order", Order.ToString());

                if (Res == DialogResult.Yes)
                {
                    MyParent.CReport(ref ObjRpt, "General Purchase Order..!");
                }
                else if (Res == DialogResult.No)
                {
                    StringBuilder Body = new StringBuilder();
                    Body.Append("Dear Sir, ");
                    Body.Append(Environment.NewLine);
                    Body.Append(Environment.NewLine);
                    Body.Append("Pls Find Attachment");

                    MyParent.CReport_Normal_PDF(ref ObjRpt, "General Purchase Order..!", "C:\\Vaahrep\\GainupPO.Pdf", false);
                    MyBase.sendEMailThroughOUTLOOK(String.Empty, "kumareshkanna@gainup.in", "Purchase Order", Body.ToString(), "C:\\Vaahrep\\GainupPO.Pdf");
                    return;
                }
                else
                {
                    MyParent.Load_ViewEntry();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Entry_Save()
        {
            try
            {
                Int32 Array_Index = 0;
                String From_Store = String.Empty;
                //Total_Count();

                //if (Dt.Rows.Count > 0)
                //{
                //    if (Grid.CurrentCell == Grid["Rate", Grid.CurrentCell.RowIndex])
                //    {
                //        Grid.CurrentCell = Grid["Order_No", Grid.CurrentCell.RowIndex + 1];
                //    }
                //}

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Details..!", "Gainup");
                    Grid.CurrentCell = Grid["Description", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtTotal.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotal.Text) == 0)
                {
                    MessageBox.Show("Invalid Details", "Gainup");
                    Grid.CurrentCell = Grid["Description", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["PO_Qty", i].Value == DBNull.Value || Grid["PO_Qty", i].Value.ToString() == String.Empty || Convert.ToDouble(Grid["PO_Qty", i].Value.ToString()) == 0.0000 || Convert.ToDouble(Grid["Rate", i].Value.ToString()) == 0.0000)
                    {
                        MessageBox.Show(" Zero Qty is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["PO_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                    else if (Grid["Rate", i].Value == DBNull.Value || Grid["Rate", i].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Rate", i].Value.ToString()) == 0.0000)
                    {
                        MessageBox.Show(" Zero Qty is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Po_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }                

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Grid["Rate", i].Value.ToString()) > Convert.ToDouble(Grid["Rate1", i].Value.ToString()))
                    {
                        MessageBox.Show(" Rate Is Invalid in Row (Rate Must Be Greater or Equal to Approved Rate)" + (i + 1) + "  ", "Gainup");
                        Grid["Rate", i].Value = Grid["Rate1", i].Value.ToString();
                        Grid.CurrentCell = Grid["Rate", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                if (DtpDate.Value > DtpReqDate.Value)
                {
                    MessageBox.Show("Invalid Req Date..!", "Gainup");
                    DtpReqDate.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }


                if (MyParent._New)
                {
                    TxtPONO.Text = MyBase.MaxOnlyWithoutComp("VSocks_General_Po_Master", "Entry_No", String.Empty, String.Empty, 0).ToString();
                }
                Queries = new string[Dt.Rows.Count * 100];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into VSocks_General_Po_Master(EDate, Entry_No, SupplierID, ETime, SystemName, Remarks, Company_Code, User_Code, Req_Date) values ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtPONO.Text + ", " + TxtSupplier.Tag + ", GetDate(), HOST_NAME(), '-', " + MyParent.CompCode + ", " + MyParent.UserCode + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpReqDate.Value) + "'); Select Scope_Identity() ";
                }
                else
                {
                    Queries[Array_Index++] = "Update VSocks_General_Po_Master Set EDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', Req_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpReqDate.Value) + "', Supplierid = " + TxtSupplier.Tag + ", User_Code = " + MyParent.UserCode + ", SystemName = Host_Name(), ETime = Getdate() Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("VSocks_General_Po_Master", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from VSocks_General_Po_Tax_Details where Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete from VSocks_General_Po_Details where MasterID = " + Code;
                    
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Grid["Po_Qty", i].Value.ToString()) > 0.0000)
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into VSocks_General_Po_Details (MasterID, Slno, ItemID, ColorID, SizeID, Po_Qty, Rate, Item_Remarks) Values (@@IDENTITY, " + Grid["Slno", i].Value + ",  " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + ", " + Grid["SizeID", i].Value + ", " + Grid["PO_Qty", i].Value + ", '" + Grid["Rate", i].Value + "', '" + Grid["Item_Remarks", i].Value + "')";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert into VSocks_General_Po_Details (MasterID, Slno, ItemID, ColorID, SizeID, Po_Qty, Rate, Item_Remarks) Values (" + Code + ", " + Grid["Slno", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + ", " + Grid["SizeID", i].Value + ", " + Grid["PO_Qty", i].Value + ", " + Grid["Rate", i].Value + ", '" + Grid["Item_Remarks", i].Value + "')";
                        }
                    }
                }
                for (int i = 0; i <= Dt_Tax.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into VSocks_General_Po_Tax_Details (Master_ID, Slno, Tax_Code, Mode, Tax_Per, Tax_Amount) Values (@@IDENTITY, " + (i + 1) + ", " + Dt_Tax.Rows[i]["Tax_Code"].ToString() + ", '" + Dt_Tax.Rows[i]["Tax_Mode"].ToString() + "', " + Dt_Tax.Rows[i]["Tax_Per"].ToString() + ", " + Dt_Tax.Rows[i]["Tax_Amount"].ToString() + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into VSocks_General_Po_Tax_Details (Master_ID, Slno, Tax_Code, Mode, Tax_Per, Tax_Amount) Values (" + Code + ", " + (i + 1) + ", " + Dt_Tax.Rows[i]["Tax_Code"].ToString() + ", '" + Dt_Tax.Rows[i]["Tax_Mode"].ToString() + "', " + Dt_Tax.Rows[i]["Tax_Per"].ToString() + ", " + Dt_Tax.Rows[i]["Tax_Amount"].ToString() + ")";
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
        void Fill_Datas(DataRow Dr)
        {
            try
            {
                Code = Convert.ToInt64(Dr["RowID"]);
                TxtPONO.Text = Dr["Entry_No"].ToString();                
                TxtSupplier.Text = Dr["Supplier"].ToString();
                TxtSupplier.Tag = Dr["SupplierID"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["Edate"]);
                DtpReqDate.Value = Convert.ToDateTime(Dr["Req_date"]);
                Grid_Data();
                Load_Tax();
                
                //Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Grid_Data()
        {
            String Str = String.Empty;
            try
            {
                if (MyParent._New == true)
                {
                    Str = "Select 0 as Slno,  '' Description, 0.0000 Po_Qty, 0.0000 Rate, 0.0000 Amount, '' Item_Remarks, 0.0000 Rate1, 0 Itemid, 0 Colorid, 0 Sizeid, 0 Slno1, 0 RNo, '-' T From Itemstock Where 1=2 ";
                }
                else
                {
                    Str = "Select A.Slno, C.Item + ' ' + D.Color + ' ' + E.Size  Description, Isnull(Po_Qty,0)Po_Qty, Rate, Isnull(Po_Qty,0)*Rate Amount, Item_Remarks, A.Rate Rate1, A.Itemid, A.Colorid, A.Sizeid, A.Slno1, ROW_NUMBER() Over (Order by C.Item, D.Color, E.Size) RNo, '-' T from fitsocks.dbo.VSocks_General_Po_Details A Left Join fitsocks.dbo.VSocks_General_Po_Master B on A.MasterID = B.Rowid Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid Where B.Rowid = " + Code + " Order By A.Slno ";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Slno1", "Rate1", "ItemID", "SizeID", "RNo", "ColorID", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Description", "Item_Remarks", "Po_Qty", "Rate");
                MyBase.Grid_Width(ref Grid, 50, 425, 110, 110, 110, 225);
                Grid.Columns["Po_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Load_Tax();
                Calculate_Item_Amount_1();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        Boolean Valid_Tax()
        {
            DataTable Tdt = new DataTable();
            try
            {
                for (int i = 0; i <= Dt_Tax.Rows.Count - 1; i++)
                {

                    if (Grid_Tax["Tax_Code", i].Value == null || Grid_Tax["Tax_Code", i].Value == DBNull.Value || Grid_Tax["Tax_Code", i].Value.ToString() == String.Empty)
                    {
                        MessageBox.Show("Invalid Tax ...!", "Gainup");
                        Grid_Tax.CurrentCell = Grid_Tax["Tax", i];
                        Grid_Tax.Focus();
                        Grid_Tax.BeginEdit(true);
                        return false;
                    }

                    Tdt = new DataTable();
                    MyBase.Load_Data("Select Accounts.Dbo.Get_Tax_Per (" + Grid_Tax["Tax_Code", i].Value.ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", MyBase.GetServerDate()) + "')", ref Tdt);
                    if (Convert.ToDouble(Tdt.Rows[0][0]) > 0)
                    {
                        Grid_Tax["Tax_Mode", i].Value = "Y";
                        Grid_Tax["Tax_Per", i].Value = Convert.ToDouble(Tdt.Rows[0][0]);
                        Grid_Tax["Tax_Amount", i].Value = Convert.ToDouble(String.Format("{0:0}", (Convert.ToDouble(TxtAmount.Text) / 100) * Convert.ToDouble(Tdt.Rows[0][0])));
                    }
                    else
                    {
                        Grid_Tax["Tax_Mode", i].Value = "N";
                        Grid_Tax["Tax_Per", i].Value = "0.00";
                        if (Grid_Tax["Tax_Amount", i].Value == null || Grid_Tax["Tax_Amount", i].Value == DBNull.Value || Grid_Tax["Tax_Amount", i].Value.ToString() == String.Empty)
                        {
                            Grid_Tax["Tax_Amount", i].Value = "0.00";
                        }
                    }

                    if (Convert.ToDouble(Grid_Tax["Tax_Amount", i].Value) == 0)
                    {
                        MessageBox.Show("Invalid Tax Amount ...!", "Gainup");
                        Grid_Tax.CurrentCell = Grid_Tax["Tax", i];
                        Grid_Tax.Focus();
                        Grid_Tax.BeginEdit(true);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        void Load_Tax()
        {
            try
            {
                if (MyParent._New)
                {
                    Grid_Tax.DataSource = MyBase.Load_Data("Select S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From Socks_Yarn_Tax_Details S1 Left Join Accounts.dbo.Ledger_Master L1 on S1.Tax_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' Where 1 = 2 Order by S1.Slno ", ref Dt_Tax);
                }
                else
                {
                    Grid_Tax.DataSource = MyBase.Load_Data("Select S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From VSocks_General_Po_Tax_Details S1 Left Join Accounts.dbo.Ledger_Master L1 on S1.Tax_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' Where S1.Master_ID = " + Code + " Order by S1.Slno ", ref Dt_Tax);
                }
                MyBase.Grid_Designing(ref Grid_Tax, ref Dt_Tax, "Tax_Code", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid_Tax, "Tax", "Tax_Per", "Tax_Amount");
                MyBase.Grid_Colouring(ref Grid_Tax, Control_Modules.Grid_Design_Mode.Column_Wise);
                Grid_Tax.Columns["Tax_Mode"].HeaderText = "Mode";
                MyBase.Grid_Width(ref Grid_Tax, 50, 230, 50, 100, 120);
                Grid_Tax.RowHeadersWidth = 10;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Grid_Tax_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Calculate_Item_Amount_1();                    
                    DtpReqDate.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Tax_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (Grid_Tax.Rows.Count > 2)
                {
                    MyBase.Row_Number(ref Grid_Tax);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Grid_Tax_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Tax == null)
                {
                    Txt_Tax = (TextBox)e.Control;
                    Txt_Tax.KeyDown += new KeyEventHandler(Txt_Tax_KeyDown);
                    Txt_Tax.KeyPress += new KeyPressEventHandler(Txt_Tax_KeyPress);
                    //Txt_Tax.GotFocus += new EventHandler(Txt_Tax_GotFocus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Tax_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Tax["Tax_Mode", Grid_Tax.CurrentCell.RowIndex].Value.ToString() == "Y")
                {
                    if (Grid_Tax["Tax_Per", Grid_Tax.CurrentCell.RowIndex].Value == null || Grid_Tax["Tax_Per", Grid_Tax.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Tax["Tax_Per", Grid_Tax.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid_Tax["Tax_Per", Grid_Tax.CurrentCell.RowIndex].Value = "0.00";
                    }

                    if (Grid_Tax.CurrentCell.ColumnIndex == Grid_Tax.Columns["Tax_Amount"].Index)
                    {
                        Grid_Tax["Tax_Amount", Grid_Tax.CurrentCell.RowIndex].Value = ((Convert.ToDouble(TxtAmount.Text) / 100) * Convert.ToDouble(Grid_Tax["Tax_Per", Grid_Tax.CurrentCell.RowIndex].Value));
                    }
                }
                else
                {
                    Grid_Tax["Tax_Per", Grid_Tax.CurrentCell.RowIndex].Value = "0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Tax_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid_Tax.CurrentCell.ColumnIndex == Grid_Tax.Columns["Tax_Mode"].Index)
                {
                    MyBase.Valid_Yes_OR_No(Txt_Tax, e);
                }
                else if (Grid_Tax.CurrentCell.ColumnIndex == Grid_Tax.Columns["Tax_Per"].Index)
                {
                    e.Handled = true;
                }
                else if (Grid_Tax.CurrentCell.ColumnIndex == Grid_Tax.Columns["Tax_Amount"].Index)
                {
                    if (Grid_Tax["Tax_Mode", Grid_Tax.CurrentCell.RowIndex].Value.ToString() == "Y")
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        MyBase.Valid_Decimal(Txt_Tax, e);
                    }
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

        void Txt_Tax_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down && Grid_Tax.CurrentCell.ColumnIndex == Grid_Tax.Columns["Tax"].Index)
                {
                    e.Handled = true;
                    Dr = Tool.Selection_Tool_Except_New("Tax_Code", this, 30, 70, ref Dt_Tax, SelectionTool_Class.ViewType.NormalView, "Select Tax", "Select Ledger_Name Tax, Ledger_Code Tax_Code From Accounts.Dbo.Tax_Accounts(" + MyParent.CompCode + ", '" + MyParent.YearCode + "')", String.Empty, 250);
                    if (Dr != null)
                    {
                        MyBase.Row_Number(ref Grid_Tax);
                        Grid_Tax["Tax", Grid_Tax.CurrentCell.RowIndex].Value = Dr["Tax"].ToString();
                        Grid_Tax["Tax_Code", Grid_Tax.CurrentCell.RowIndex].Value = Dr["Tax_Code"].ToString();
                        Txt_Tax.Text = Dr["Tax"].ToString();

                        DataTable Tdt = new DataTable();
                        MyBase.Load_Data("Select Accounts.Dbo.Get_Tax_Per (" + Dr["Tax_Code"].ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", MyBase.GetServerDate()) + "')", ref Tdt);
                        if (Convert.ToDouble(Tdt.Rows[0][0]) > 0)
                        {
                            Grid_Tax["Tax_Mode", Grid_Tax.CurrentCell.RowIndex].Value = "Y";
                            Grid_Tax["Tax_Per", Grid_Tax.CurrentCell.RowIndex].Value = Convert.ToDouble(Tdt.Rows[0][0]);

                            DataTable Tdt1 = new DataTable();
                            MyBase.Load_Data("Select Tax1 From Accounts.Dbo.Cess_Details_FN (" + MyParent.CompCode + ") Where Tax2 = " + Dr["Tax_Code"].ToString(), ref Tdt1);
                            if (Tdt1.Rows.Count > 0)
                            {
                                Grid_Tax["Tax_Amount", Grid_Tax.CurrentCell.RowIndex].Value = Convert.ToDouble(String.Format("{0:0}", ((Get_Conditional_Tax(Convert.ToInt32(Tdt1.Rows[0]["Tax1"]))) / 100) * Convert.ToDouble(Tdt.Rows[0][0])));
                            }
                            else
                            {
                                Grid_Tax["Tax_Amount", Grid_Tax.CurrentCell.RowIndex].Value = Convert.ToDouble(String.Format("{0:0}", ((Convert.ToDouble(TxtAmount.Text) + Previous_Tax_Values(Grid_Tax.CurrentCell.RowIndex)) / 100) * Convert.ToDouble(Tdt.Rows[0][0])));
                            }

                            //Grid_Tax["Tax_Amount", Grid_Tax.CurrentCell.RowIndex].Value = Convert.ToDouble(String.Format ("{0:0}", (Convert.ToDouble(TxtAmount.Text) / 100) * Convert.ToDouble(Tdt.Rows[0][0])));
                        }
                        else
                        {
                            Grid_Tax["Tax_Mode", Grid_Tax.CurrentCell.RowIndex].Value = "N";
                            Grid_Tax["Tax_Per", Grid_Tax.CurrentCell.RowIndex].Value = "0.00";

                            Grid_Tax.CurrentCell = Grid_Tax["Tax_Amount", Grid_Tax.CurrentCell.RowIndex];
                            Grid_Tax.Focus();
                            Grid_Tax.BeginEdit(true);
                        }
                    }
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
        Double Previous_Tax_Values(Int32 Row)
        {
            Double Value = 0;
            try
            {
                for (int i = 0; i <= Row - 1; i++)
                {
                    Value += Convert.ToDouble(Grid_Tax["Tax_Amount", i].Value);
                }

                return Value;
            }
            catch (Exception ex)
            {
                return Value;
            }
        }


        Double Get_Conditional_Tax(int Tax_Code)
        {
            Double Value = 0;
            try
            {
                for (int i = 0; i <= Grid_Tax.Rows.Count - 1; i++)
                {
                    if (Tax_Code == Convert.ToInt32(Grid_Tax["Tax_Code", i].Value))
                    {
                        Value = Convert.ToDouble(Grid_Tax["Tax_Amount", i].Value);
                    }
                }

                return Value;
            }
            catch (Exception ex)
            {
                return Value;
            }
        }

        void Refresh_Tax()
        {
            try
            {
                for (int i = 0; i <= Dt_Tax.Rows.Count - 1; i++)
                {
                    if (Grid_Tax["Tax_Mode", i].Value.ToString() == "Y")
                    {
                        DataTable Tdt1 = new DataTable();
                        MyBase.Load_Data("Select Tax1 From Accounts.Dbo.Cess_Details_FN (" + MyParent.CompCode + ") Where Tax2 = " + Grid_Tax["Tax_Code", i].Value.ToString(), ref Tdt1);
                        if (Tdt1.Rows.Count > 0)
                        {
                            Grid_Tax["Tax_Amount", i].Value = String.Format("{0:0}", Convert.ToDouble(Grid_Tax["Tax_Per", i].Value) * ((Get_Conditional_Tax(Convert.ToInt32(Tdt1.Rows[0]["Tax1"])))) / 100);
                        }
                        else
                        {
                            Grid_Tax["Tax_Amount", i].Value = String.Format("{0:0}", Convert.ToDouble(Grid_Tax["Tax_Per", i].Value) * ((Convert.ToDouble(TxtAmount.Text) + Previous_Tax_Values(i)) / 100));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        void Refresh_Tax_Old()
        {
            try
            {
                for (int i = 0; i <= Dt_Tax.Rows.Count - 1; i++)
                {
                    if (Grid_Tax["Tax_Mode", i].Value.ToString() == "Y")
                    {
                        Grid_Tax["Tax_Amount", i].Value = String.Format("{0:0}", Convert.ToDouble(Grid_Tax["Tax_Per", i].Value) * (Convert.ToDouble(TxtAmount.Text) / 100));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Boolean Calculate_Item_Amount()
        {
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["PO_Qty", i].Value == null || Grid["PO_Qty", i].Value == DBNull.Value || Grid["PO_Qty", i].Value.ToString() == String.Empty)
                    {
                        Grid["PO_Qty", i].Value = "0.000";
                    }

                    if (Grid["Rate", i].Value == null || Grid["Rate", i].Value == DBNull.Value || Grid["Rate", i].Value.ToString() == String.Empty)
                    {
                        Grid["Rate", i].Value = "0.00";
                    }

                    if (Convert.ToDouble(Grid["PO_Qty", i].Value) > Convert.ToDouble(Grid["Bal_Qty", i].Value))
                    {
                        MessageBox.Show("PO Qty is greater than Balance ...!", "Gainup");
                        Grid["PO_Qty", i].Value = Grid["Bal_Qty", i].Value;
                        Grid.CurrentCell = Grid["PO_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return false;
                    }

                    if (Convert.ToDouble(Grid["Rate", i].Value) > Convert.ToDouble(Grid["ARate", i].Value))
                    {
                        MessageBox.Show("Rate is greater than Approved [" + Grid["ARate", i].Value.ToString() + "] ...!", "Gainup");
                        Grid["Rate", i].Value = Grid["ARate", i].Value;
                        Grid.CurrentCell = Grid["Rate", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return false;
                    }

                    Grid["PO_Qty", i].Value = String.Format("{0:0.000}", Convert.ToDouble(Grid["PO_Qty", i].Value));

                    Grid["Amount", i].Value = Convert.ToDouble(Grid["PO_Qty", i].Value) * Convert.ToDouble(Grid["Rate", i].Value);
                }

                TxtQTY.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum(ref Grid, "PO_Qty", "Item_ID", "Size_ID", "Color_ID")));
                TxtAmount.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "Amount", "Item_ID", "Size_ID", "Color_ID")))));

                Refresh_Tax();

                TxtTotal.Text = String.Format("{0:n}", Convert.ToDouble(TxtAmount.Text) + Convert.ToDouble(MyBase.Sum(ref Grid_Tax, "Tax_Amount", "Tax_Code", "Tax")));

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        Boolean Calculate_Item_Amount_1()
        {
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["PO_Qty", i].Value == null || Grid["PO_Qty", i].Value == DBNull.Value || Grid["PO_Qty", i].Value.ToString() == String.Empty)
                    {
                        Grid["PO_Qty", i].Value = "0.0000";
                    }
                    if (Grid["RATE", i].Value == null || Grid["RATE", i].Value == DBNull.Value || Grid["RATE", i].Value.ToString() == String.Empty)
                    {
                        Grid["RATE", i].Value = "0.0000";
                    }

                    if (Convert.ToDouble(Grid["RATE", i].Value) > Convert.ToDouble(Grid["RATE1", i].Value))
                    {
                        MessageBox.Show("RATE is greater than APPROVED [" + Grid["ARate", i].Value.ToString() + "] ...!", "Gainup");
                        Grid["RATE", i].Value = Grid["RATE1", i].Value;
                        Grid.CurrentCell = Grid["RATE", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return false;
                    }


                    Grid["PO_Qty", i].Value = String.Format("{0:0.0000}", Convert.ToDouble(Grid["PO_Qty", i].Value));

                    Grid["Amount", i].Value = Convert.ToDouble(Grid["PO_Qty", i].Value) * Convert.ToDouble(Grid["Rate", i].Value);
                }

                TxtQTY.Text = String.Format("{0:0.0000}", Convert.ToDouble(MyBase.Sum(ref Grid, "Po_Qty", "ItemID", "SizeID", "ColorID")));
                TxtAmount.Text = String.Format("{0:0.0000}", Convert.ToDouble(String.Format("{0:0.0000}", Convert.ToDouble(MyBase.Sum(ref Grid, "Amount", "ItemID", "SizeID", "ColorID")))));

                Refresh_Tax();

                TxtTotal.Text = String.Format("{0:n}", Convert.ToDouble(TxtAmount.Text) + Convert.ToDouble(MyBase.Sum(ref Grid_Tax, "Tax_Amount", "Tax_Code", "Tax")));

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_Qty"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index)
                    {
                        if (Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value == null || Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value = "0.0000";
                        }

                        if (Grid["Rate", Grid.CurrentCell.RowIndex].Value == null || Grid["Rate", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Rate", Grid.CurrentCell.RowIndex].Value = "0.0000";
                        }

                        if (Convert.ToDouble(Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value) == 0.0000)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Po_Qty ...!", "Gainup");
                            Grid.CurrentCell = Grid["Po_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index)
                        {

                            if (Convert.ToDouble(Grid["Rate", Grid.CurrentCell.RowIndex].Value) == 0.0000)
                            {
                                e.Handled = true;
                                MessageBox.Show("Invalid Rate ...!", "Gainup");
                                Grid.CurrentCell = Grid["Rate", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                        }
                        //Grid["Bill_Amount", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["Bill_Qty", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["Bill_Rate", Grid.CurrentCell.RowIndex].Value);
                    }
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
                MyBase.Row_Number(ref Grid);
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
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
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
                    Txt.Enter += new EventHandler(Txt_Enter);
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

        void Txt_Enter(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Description"].Index)
                {
                    MyBase.Row_Number(ref Grid);
                    Total_Count();
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Description"].Index)
                    {
                        if (TxtSupplier.Text != String.Empty)
                        {

                            Str = " Select B.Item + ' ' + D.Color + ' ' + E.Size Description, 0.0000 Po_Qty, 0.0000 Rate, 0.0000 Amount, '' Item_Remarks, ROW_NUMBER() Over (Order by A.ItemID, A.ColorID, A.SizeID) RNo, A.ItemID, A.ColorID, A.SizeID, 0.0000 Rate1 from (Select Distinct Itemid, Colorid, Sizeid From ItemStock) A  Left Join Item B on A.Itemid = B.itemid Left Join Color D on A.Colorid = D.colorid Left Join Size E on A.sizeid = E.sizeid  Where B.Item_Type not in('Garment') And Item not like '%zz%' And Color not like '%zz%' And Size not like '%zz%' And Item not like ' %' And Color not like ' %' And Size not like ' %' Order By B.Item";

                            Dr = Tool.Selection_Tool_Except_New("RNo", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Item ", Str, String.Empty, 425, 110, 110, 110, 175);

                            if (Dr != null)
                            {
                                Txt.Text = Dr["Description"].ToString();
                                Grid["Description", Grid.CurrentCell.RowIndex].Value = Dr["Description"].ToString();                                
                                Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Po_Qty"].ToString();                                
                                Grid["Rate", Grid.CurrentCell.RowIndex].Value = Dr["Rate"].ToString();
                                Grid["Amount", Grid.CurrentCell.RowIndex].Value = Dr["Amount"].ToString();
                                Grid["Item_Remarks", Grid.CurrentCell.RowIndex].Value = Dr["Item_Remarks"].ToString();
                                Grid["itemid", Grid.CurrentCell.RowIndex].Value = Dr["Itemid"].ToString();
                                Grid["Rate1", Grid.CurrentCell.RowIndex].Value = Dr["Rate1"].ToString();
                                Grid["Sizeid", Grid.CurrentCell.RowIndex].Value = Dr["Sizeid"].ToString();
                                Grid["Colorid", Grid.CurrentCell.RowIndex].Value = Dr["Colorid"].ToString();
                                Grid["RNo", Grid.CurrentCell.RowIndex].Value = Dr["RNo"].ToString();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Supplier", "Gainup");
                            TxtSupplier.Focus();
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
        Int16 Max_Slno_Grid()
        {
            Int16 No = 0;
            try
            {
                if (Dt.Rows.Count == 0)
                {
                    No = 1;
                    return No;
                }
                else
                {
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (No < Convert.ToInt16(Dt.Rows[i]["Slno1"]))
                        {
                            No = Convert.ToInt16(Dt.Rows[i]["Slno1"]);
                        }
                    }
                }
                No += 1;
                return No;
            }
            catch (Exception ex)
            {
                return No;
            }
        }

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Po_Qty"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Item_Remarks"].Index)
                {

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

        void Txt_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Po_Qty"].Index)
                {
                    if ((Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) == String.Empty || Convert.ToDouble(Txt.Text)==0.0000)
                    {
                        MessageBox.Show("Invalid Po_Qty..!", "Gainup");
                        Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value = "0.0000";
                        Txt.Text = "0.0000";
                        Grid.CurrentCell = Grid["Po_Qty", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index)
                {
                    if ((Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString()) == String.Empty)
                    {
                        MessageBox.Show("Invalid Rate..!", "Gainup");
                        Grid["Rate", Grid.CurrentCell.RowIndex].Value = "0.0000";
                        Grid.CurrentCell = Grid["Rate", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index)
                {
                    if (Convert.ToDouble(Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value) > 0.000)
                    {
                        Grid["Amount", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(Txt.Text));
                    }
                }
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Po_Qty"].Index)
                {
                    if (Convert.ToDouble(Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value) > 0.000)
                    {
                        Grid["Amount", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(Txt.Text));
                    }
                }
                Calculate_Item_Amount_1();
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
                TxtTotal.Text = MyBase.Sum(ref Grid, "Po_Qty");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmGeneralPo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtSupplier" || this.ActiveControl.Name == "TxtPONo")
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Grid_Leave(object sender, EventArgs e)
        {
            if (Grid.Rows.Count >= 1)
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Grid["Rate", i].Value.ToString()) > Convert.ToDouble(Grid["Rate1", i].Value.ToString()))
                    {
                        MessageBox.Show(" Rate Is Invalid in Row (Po Rate is greater than Approved Rate)" + (i + 1) + "  ", "Gainup");
                        Grid["Rate", i].Value = Grid["Rate1", i].Value.ToString();
                        Grid.CurrentCell = Grid["Rate", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }
            }
        }

        private void Grid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Grid["Description", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index)
                    {
                        if (Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            if (Convert.ToDouble(Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString()) > Convert.ToDouble(Grid["Rate1", Grid.CurrentCell.RowIndex].Value.ToString()))
                            {
                                //Txt.Text
                                MessageBox.Show("Po Rate is greater than Approved Rate...!", "Gainup");                                
                                Grid["Rate", Grid.CurrentCell.RowIndex].Value = Grid["Rate1", Grid.CurrentCell.RowIndex].Value.ToString();
                                Grid.CurrentCell = Grid["Rate", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                            else
                            {

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

        private void Grid_Tax_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref Grid_Tax, ref Dt_Tax, Grid_Tax.CurrentCell.RowIndex);
                Calculate_Item_Amount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
