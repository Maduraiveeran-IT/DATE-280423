using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmGrnInvoicing : Form, Entry
    {
        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataRow Dr;
        Int64 Code = 0;
        DataTable Dt_Grn = new DataTable();
        DataTable Dt_Tax = new DataTable();
        Boolean Status_Flag = false;
        TextBox Txt = null;
        String OCN_List = String.Empty;
        TextBox Txt_Tax = null;

        public FrmGrnInvoicing()
        {
            InitializeComponent();
        }

        void Load_COmbo()
        {
            try
            {
                CmbMode.Items.Clear();
                CmbMode.Items.Add("YARN");
                CmbMode.Items.Add("TRIMS");
                CmbMode.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmGrnInvoicing_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                MyBase.Clear(this);
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
                MyBase.Enable_Controls(this, true);
                Load_COmbo();
                Load_Tax();
                GRN_Generate();
                button1.Enabled = true;
                Grid_Grn.Enabled = true;
                CmbMode.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Fill_Data(DataRow Dr)
        {
            try
            {
                Load_COmbo();
                Code = Convert.ToInt64(Dr["Code"]);
                TxtEntryNo.Text = Dr["Entry_No"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["Entry_Date"]);
                TxtSupplier.Text = Dr["Supplier"].ToString();
                TxtSupplier.Tag = Dr["Supplier_Code"].ToString();
                TxtInvoiceNo.Text = Dr["Invoice_No"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();
                DtpInvoiceDate.Value = Convert.ToDateTime(Dr["Invoice_Date"]);
                if (MyParent.Edit)
                {
                    button1.Enabled = true;
                    Grid_Grn.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                    Grid_Grn.Enabled = false;
                }

                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select Distinct Mode from Socks_GRN_Invoicing_Details_OCN Where Master_ID = " + Code, ref Tdt);
                CmbMode.Text = Tdt.Rows[0][0].ToString();

                Load_Grn();


                for (int i = 0; i <= Dt_Grn.Rows.Count - 1; i++)
                {
                    Grid_Grn["Status", i].Value = true;
                }

                Load_Tax();
                Grid_Data();
                Bill_Amount();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Entry - Edit", "Select S1.Entry_No, S1.Entry_Date, L1.Ledger_Name SUpplier, S1.Invoice_No, S1.Invoice_Date, S1.Supplier_Code, S1.RowID Code, S1.Remarks From Socks_GRN_Invoicing_Master S1 Left join Accounts.Dbo.Ledger_MAster L1 on S1.Supplier_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " And Approval = 'N' And L1.YEAR_CODE = '" + MyParent.YearCode + "'", String.Empty, 120, 100, 250, 120, 100);
                if (Dr != null)
                {
                    Fill_Data(Dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void GRN_Generate()
        {
            try
            {
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select DBo.Get_Max_Socks_Yarn_GRN_Invoicing ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "')", ref Tdt);
                TxtEntryNo.Text = Tdt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                TxtEntryNo.Text = String.Empty;
                throw ex;
            }
        }

        public void Entry_Save()
        {
            try
            {
                if (TxtSupplier.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Supplier ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtSupplier.Focus();
                    return;
                }

                if (TxtInvoiceNo.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Invoice No ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtInvoiceNo.Focus();
                    return;
                }

                if (!Bill_Amount())
                {
                    MyParent.Save_Error = true;
                    return;
                };

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Data's ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtInvoiceNo.Focus();
                    return;
                }

                if (TxtBillNet.Text.Trim() == String.Empty || Convert.ToDouble(TxtBillNet.Text) == 0)
                {
                    MessageBox.Show("Invalid Data's ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtInvoiceNo.Focus();
                    return;
                }

                if (TxtPONet.Text.Trim() == String.Empty || Convert.ToDouble(TxtPONet.Text) == 0)
                {
                    MessageBox.Show("Invalid Data's ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtInvoiceNo.Focus();
                    return;
                }

                if (MyParent.Edit)
                {
                    GRN_Generate();
                }

                String[] Queries = new String[20];
                Int32 Array_Index = 0;

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert Into Socks_GRN_Invoicing_Master (Entry_No, Entry_Date, Supplier_Code, Invoice_No, Invoice_Date, PO_Gross, PO_Tax, PO_Net, Bill_Gross, Bill_Tax, Bill_Net, To_Be_Paid, Rate_Debit, Qty_Debit, Approved_Debit, Remarks, Approval) Values ('" + TxtEntryNo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtSupplier.Tag.ToString() + ", '" + TxtInvoiceNo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpInvoiceDate.Value) + "', " + Convert.ToDouble(TxtPOGross.Text) + ", " + Convert.ToDouble(TxtPOTax.Text) + ", " + Convert.ToDouble(TxtPONet.Text) + ", " + Convert.ToDouble(TxtBillGross.Text) + ", " + Convert.ToDouble(TxtBillTax.Text) + ", " + Convert.ToDouble(TxtBillNet.Text) + ", " + Convert.ToDouble(TxtToBePaid.Text) + ", " + Convert.ToDouble(TxtRateDifference.Text) + ", " + Convert.ToDouble(TxtQtyDifference.Text) + ", " + Convert.ToDouble(Convert.ToDouble(TxtRateDifference.Text) + Convert.ToDouble(TxtQtyDifference.Text)) + ", '" + TxtRemarks.Text + "', 'N'); Select Scope_Identity() ";
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_GRN_Invoicing_Master Set Approval = 'N', Invoice_No = '" + TxtInvoiceNo.Text + "', Invoice_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',  PO_Gross = " + Convert.ToDouble(TxtPOGross.Text) + ", PO_Tax = " + Convert.ToDouble(TxtPOTax.Text) + ", PO_Net = " + Convert.ToDouble(TxtPONet.Text) + ", Bill_Gross = " + Convert.ToDouble(TxtBillGross.Text) + ", Bill_Tax = " + Convert.ToDouble(TxtBillTax.Text) + ", Bill_Net = " + Convert.ToDouble(TxtBillNet.Text) + ", To_Be_Paid = " + Convert.ToDouble(TxtToBePaid.Text) + ",  Rate_Debit = " + Convert.ToDouble(TxtRateDifference.Text) + ", Qty_Debit = " + Convert.ToDouble(TxtQtyDifference.Text) + ", Approved_Debit = " + Convert.ToDouble(Convert.ToDouble(TxtRateDifference.Text) + Convert.ToDouble(TxtQtyDifference.Text)) + ", Remarks = '" + TxtRemarks.Text + "' Where RowID = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_GRN_Invoicing_Details_OCN Where Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_GRN_Invoicing_Details_Tax Where Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_GRN_Invoicing_Details Where Master_ID = " + Code;
                }

                for (int i = 0; i <= Dt_Grn.Rows.Count - 1; i++)
                {
                    if (Grid_Grn["Status", i].Value != null && Grid_Grn["Status", i].Value != DBNull.Value && Grid_Grn["Status", i].Value.ToString().ToUpper() == "true".ToUpper())
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert Into Socks_GRN_Invoicing_Details_OCN (Master_ID, Slno, GRN_MasterID, Mode) Values (@@IDENTITY, " + (i + 1) + ", " + Grid_Grn["GRN_MasterID", i].Value.ToString() + ", '" + CmbMode.Text.Trim() + "')";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert Into Socks_GRN_Invoicing_Details_OCN (Master_ID, Slno, GRN_MasterID, Mode) Values (" + Code + ", " + (i + 1) + ", " + Grid_Grn["GRN_MasterID", i].Value.ToString() + ", '" + CmbMode.Text.Trim() + "')";
                        }
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Socks_GRN_Invoicing_Details (Master_ID, Slno, Item_ID, Color_ID, Size_ID, GRN_Qty, Rate, PO_Amount, Bill_Qty, Bill_Rate, Bill_Amount) Values (@@IDENTITY, " + (i + 1) + ", " + Dt.Rows[i]["Item_ID"].ToString() + ", " + Dt.Rows[i]["Color_ID"].ToString() + ", " + Dt.Rows[i]["Size_ID"].ToString() + ", " + Dt.Rows[i]["GRN_Qty"].ToString() + ", " + Dt.Rows[i]["Rate"].ToString() + ", " + Dt.Rows[i]["PO_AMount"].ToString() + ", " + Dt.Rows[i]["Bill_Qty"].ToString() + ", " + Dt.Rows[i]["Bill_Rate"].ToString() + ", " + Dt.Rows[i]["Bill_Amount"].ToString() + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Socks_GRN_Invoicing_Details (Master_ID, Slno, Item_ID, Color_ID, Size_ID, GRN_Qty, Rate, PO_Amount, Bill_Qty, Bill_Rate, Bill_Amount) Values (" + Code + ", " + (i + 1) + ", " + Dt.Rows[i]["Item_ID"].ToString() + ", " + Dt.Rows[i]["Color_ID"].ToString() + ", " + Dt.Rows[i]["Size_ID"].ToString() + ", " + Dt.Rows[i]["GRN_Qty"].ToString() + ", " + Dt.Rows[i]["Rate"].ToString() + ", " + Dt.Rows[i]["PO_AMount"].ToString() + ", " + Dt.Rows[i]["Bill_Qty"].ToString() + ", " + Dt.Rows[i]["Bill_Rate"].ToString() + ", " + Dt.Rows[i]["Bill_Amount"].ToString() + ")";
                    }
                }

                for (int i = 0; i <= Dt_Tax.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert Into Socks_GRN_Invoicing_Details_Tax (Master_ID, Slno, Tax_Code, Mode, Tax_Per, Tax_Amount) Values (@@IDENTITY, " + (i + 1) + ", " + Dt_Tax.Rows[i]["Tax_Code"].ToString() + ", '" + Dt_Tax.Rows[i]["Tax_Mode"].ToString() + "', " + Dt_Tax.Rows[i]["Tax_Per"].ToString() + ", " + Dt_Tax.Rows[i]["Tax_Amount"].ToString() + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert Into Socks_GRN_Invoicing_Details_Tax (Master_ID, Slno, Tax_Code, Mode, Tax_Per, Tax_Amount) Values (" + Code + ", " + (i + 1) + ", " + Dt_Tax.Rows[i]["Tax_Code"].ToString() + ", '" + Dt_Tax.Rows[i]["Tax_Mode"].ToString() + "', " + Dt_Tax.Rows[i]["Tax_Per"].ToString() + ", " + Dt_Tax.Rows[i]["Tax_Amount"].ToString() + ")";
                    }
                }

                MyBase.Run_Identity (MyParent.Edit, Queries);
                MessageBox.Show("Saved ....!", "Gainup");
                MyParent.Save_Error = false;
                MyBase.Clear(this);

            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Delete ()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, false);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Entry - Delete", "Select S1.Entry_No, S1.Entry_Date, L1.Ledger_Name SUpplier, S1.Invoice_No, S1.Invoice_Date, S1.Supplier_Code, S1.RowID Code, S1.Remarks From Socks_GRN_Invoicing_Master S1 Left join Accounts.Dbo.Ledger_MAster L1 on S1.Supplier_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " And Approval = 'N' And L1.YEAR_CODE = '" + MyParent.YearCode + "'", String.Empty, 120, 100, 250, 120, 100);
                if (Dr != null)
                {
                    Fill_Data(Dr);
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
                    MyBase.Run("Delete from Socks_GRN_Invoicing_Details_OCN Where MAster_ID = " + Code, "Delete from Socks_GRN_Invoicing_Details_Tax Where MAster_ID = " + Code, "Delete from Socks_GRN_Invoicing_Details Where MAster_ID = " + Code, "Delete From Socks_GRN_Invoicing_Master Where RowID = " + Code);
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Entry - View", "Select S1.Entry_No, S1.Entry_Date, L1.Ledger_Name SUpplier, S1.Invoice_No, S1.Invoice_Date, S1.Supplier_Code, S1.RowID Code, S1.Remarks From Socks_GRN_Invoicing_Master S1 Left join Accounts.Dbo.Ledger_MAster L1 on S1.Supplier_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " And L1.YEAR_CODE = '" + MyParent.YearCode + "'", String.Empty, 120, 100, 250, 120, 100);
                if (Dr != null)
                {
                    Fill_Data(Dr);
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
                String Str, Str1, Str2, Str3, Str4;
                String Order = "";
                DataTable Dt1 = new DataTable();
                DataTable Dt2 = new DataTable();
                DataTable Dt3 = new DataTable();
                DataTable Dt4 = new DataTable();

                Str = " Select S1.Entry_No PONo, L1.Ledger_Name Supplier, S1.Invoice_No Bill_No, Cast(S1.Entry_Date As date)PoDate, Cast(S1.Invoice_Date as Date) Required_Date, (Case When S2.Item_Type='Yarn' Then 'Yarn' Else 'Trims' End) PO_Method, L1.Ledger_Address Supplier_Address, L1.Ledger_Phone Supplier_Phone, L1.Ledger_email Supplier_Email From Socks_GRN_Invoicing_Master S1 Left Join Dbo.Supplier_All_Fn() L1 on S1.Supplier_Code = L1.Ledger_Code Left Join (Select Distinct A.Master_Id, B.Item_Type from Socks_GRN_Invoicing_Details A Left Join Item B on A.Item_ID = B.itemid) S2 on S1.RowID = S2.Master_ID Where S1.RowID = " + Code;
                MyBase.Load_Data(Str, ref Dt1);

                Str1 = " Select Top 100000000 ROW_NUMBER()Over(Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size)SlNo, I1.Item + ' - ' + C1.color + ' - ' + S4.Size Item_Color_Size, S2.Bill_Qty Order_Qty, S2.Bill_Rate Rate, S2.Bill_Amount Amount From Socks_GRN_Invoicing_Master S1 Inner join Socks_GRN_Invoicing_Details S2 ON S1.RowID = S2.Master_ID Inner join Item I1 on S2.Item_id = I1.itemid Inner join color C1 on s2.Color_id = c1.colorid Inner join size S4 on s2.Size_ID = S4.sizeid Where S1.RowID = " + Code + " Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size ";
                MyBase.Execute_Qry(Str1, "Socks_Purchase_Inv");

                Str2 = " Select Top 4 S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From Socks_GRN_Invoicing_Details_Tax S1 Left Join Accounts.dbo.Ledger_Master L1 on S1.Tax_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' Where S1.Master_ID = " + Code + " Order by S1.Slno ";
                MyBase.Load_Data(Str2, ref Dt2);

                Str3 = " Select Distinct S4.Order_No From Socks_GRN_Invoicing_Master S1 Inner join Socks_GRN_Invoicing_Details S2 ON S1.RowID = S2.Master_ID Inner Join Socks_GRN_Invoicing_Details_OCN S3 on S1.RowID = S3.Master_ID And S2.Master_ID = S3.Master_ID Left Join (select A.Rowid, D.Order_No, Sum(C.Qty)Qty from Socks_Trims_GRN_Master A Left Join Socks_Trims_GRN_Details B on A.RowID = B.Master_ID Left Join Socks_Trims_GRN_OCN_DEtails  C on A.RowID = C.Master_ID And B.RowID = C.Detail_ID  And B.Master_ID = C.Master_ID Left Join Socks_Order_Master D on C.Order_ID = D.RowID Group By A.Rowid, D.Order_No)S4 on GRN_MasterID = S4.RowID Where S1.RowID = " + Code;
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
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RptPurchaseInv.rpt");
                MyParent.FormulaFill(ref ObjRpt, "Heading", "PURCHASE INVOICE");                
                
                MyParent.FormulaFill(ref ObjRpt, "Supplier", Dt1.Rows[0]["Supplier"].ToString());
                MyParent.FormulaFill(ref ObjRpt, "Supplier_Address", Dt1.Rows[0]["Supplier_Address"].ToString().Replace("\r\n", "__"));
                MyParent.FormulaFill(ref ObjRpt, "Supplier_Phone", Dt1.Rows[0]["Supplier_Phone"].ToString());
                MyParent.FormulaFill(ref ObjRpt, "Supplier_Email", Dt1.Rows[0]["Supplier_Email"].ToString());
                MyParent.FormulaFill(ref ObjRpt, "PONo", Dt1.Rows[0]["PONo"].ToString());
                MyParent.FormulaFill(ref ObjRpt, "BillNo", Dt1.Rows[0]["Bill_No"].ToString());                
                MyParent.FormulaFill(ref ObjRpt, "PoDate", String.Format("{0:dd-MM-yyyy}", Dt1.Rows[0]["PoDate"]));
                MyParent.FormulaFill(ref ObjRpt, "ReqDate", String.Format("{0:dd-MM-yyyy}", Dt1.Rows[0]["Required_Date"]));                
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
                        else if (i == 2)
                        {
                            MyParent.FormulaFill(ref ObjRpt, "Tax3", Dt2.Rows[2]["Tax"].ToString());
                            MyParent.FormulaFill(ref ObjRpt, "Tax3_Per", Dt2.Rows[2]["Tax_Per"].ToString());
                            MyParent.FormulaFill(ref ObjRpt, "Tax3_Amount", Dt2.Rows[2]["Tax_Amount"].ToString());
                        }
                        else if (i == 3)
                        {
                            MyParent.FormulaFill(ref ObjRpt, "Tax4", Dt2.Rows[3]["Tax"].ToString());
                            MyParent.FormulaFill(ref ObjRpt, "Tax4_Per", Dt2.Rows[3]["Tax_Per"].ToString());
                            MyParent.FormulaFill(ref ObjRpt, "Tax4_Amount", Dt2.Rows[3]["Tax_Amount"].ToString());
                        }
                    }
                }
                MyParent.FormulaFill(ref ObjRpt, "Net_Amount", TxtBillNet.Text.ToString());
                MyParent.FormulaFill(ref ObjRpt, "Order", Order.ToString());
                MyParent.CReport(ref ObjRpt, "Accessory Purchase Order..!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Load_Grn()
        {
            try
            {
                Dt_Grn = new DataTable();

                if (MyParent._New)
                {
                    if (CmbMode.Text.Trim() == "YARN")
                    {
                        Grid_Grn.DataSource = MyBase.Load_Data("Select Distinct GrnNo GRNNO, GrnDate GRNDATE, GRN_MasterID From Socks_Yrn_Grn_Invoicing_Pending (" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Where Supplier_Code = " + TxtSupplier.Tag.ToString() + " Order by GrnNo", ref Dt_Grn);
                    }
                    else
                    {
                        Grid_Grn.DataSource = MyBase.Load_Data("Select Distinct GrnNo GRNNO, GrnDate GRNDATE, GRN_MasterID From Socks_Trims_Grn_Invoicing_Pending (" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Where Supplier_Code = " + TxtSupplier.Tag.ToString() + " Order by GrnNo", ref Dt_Grn);
                    }
                }
                else
                {
                    if (CmbMode.Text.Trim() == "YARN")
                    {
                        Grid_Grn.DataSource = MyBase.Load_Data("Select Distinct S2.GrnNo GRNNO, S2.GrnDate GRNDATE, S1.GRN_MasterID From Socks_GRN_Invoicing_Details_OCN S1 Inner Join Socks_Yarn_Grn_Master S2 on S1.GRN_MasterID = S2.RowID Where S1.Master_ID = " + Code + " Order By S2.GrnNo", ref Dt_Grn);
                    }
                    else
                    {
                        Grid_Grn.DataSource = MyBase.Load_Data("Select Distinct S2.GrnNo GRNNO, S2.GrnDate GRNDATE, S1.GRN_MasterID From Socks_GRN_Invoicing_Details_OCN S1 Inner Join Socks_Trims_Grn_Master S2 on S1.GRN_MasterID = S2.RowID Where S1.Master_ID = " + Code + " Order By S2.GrnNo", ref Dt_Grn);
                    }
                }

                MyBase.Grid_Designing(ref Grid_Grn, ref Dt_Grn, "GRN_MasterID");
                MyBase.ReadOnly_Grid_Without(ref Grid_Grn);
                MyBase.Grid_Colouring(ref Grid_Grn, Control_Modules.Grid_Design_Mode.Column_Wise);

                if (Status_Flag)
                {
                    Grid_Grn.Columns.Remove("Status");
                    DataGridViewCheckBoxColumn Check = new DataGridViewCheckBoxColumn();
                    Check.HeaderText = "Status";
                    Check.Name = "Status";
                    Check.ValueType = typeof(String);
                    Check.Visible = true;
                    Check.ReadOnly = false;
                    Grid_Grn.Columns.Insert(0, Check);
                    Status_Flag = true;
                }
                else
                {
                    DataGridViewCheckBoxColumn Check = new DataGridViewCheckBoxColumn();
                    Check.HeaderText = "Status";
                    Check.Name = "Status";
                    Check.ValueType = typeof(String);
                    Check.Visible = true;
                    Check.ReadOnly = false;
                    Grid_Grn.Columns.Insert(0, Check);
                    Status_Flag = true;
                }
                MyBase.Grid_Width(ref Grid_Grn, 70, 120, 100);
                Grid_Grn.Columns["Status"].HeaderText = "STATUS";
                Grid_Grn.Columns["Status"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                Grid_Grn.RowHeadersWidth = 10;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmGrnInvoicing_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtRemarks")
                    {

                    }
                    else if (this.ActiveControl.Name == "TxtRateDifference")
                    {
                        TxtRemarks.Focus();
                        SendKeys.Send("{END}");
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
                        if (CmbMode.Text == "YARN")
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "Select Distinct Supplier, Supplier_Code From Socks_Yrn_Grn_Invoicing_Pending (" + MyParent.CompCode + ", '" + MyParent.YearCode + "')", String.Empty, 250);
                        }
                        else
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "Select Distinct Supplier, Supplier_Code From Socks_Trims_Grn_Invoicing_Pending (" + MyParent.CompCode + ", '" + MyParent.YearCode + "')", String.Empty, 250);
                        }
                        if (Dr != null)
                        {
                            TxtSupplier.Text = Dr["Supplier"].ToString();
                            TxtSupplier.Tag = Dr["Supplier_Code"].ToString();
                            Load_Grn();
                        }
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    if (this.ActiveControl.Name == "TxtRemarks")
                    {
                        if (MyParent._New || MyParent.Edit)
                        {
                            MyParent.Load_SaveEntry();
                        }
                    }
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmGrnInvoicing_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtInvoiceNo")
                    {
                    }
                    else if (this.ActiveControl.Name == String.Empty)
                    {

                    }
                    else if (this.ActiveControl.Name == "TxtRemarks")
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

        void Load_Tax()
        {
            try
            {
                Dt_Tax = new DataTable();

                if (MyParent._New)
                {
                    Grid_Tax.DataSource = MyBase.Load_Data("Select S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From Socks_GRN_Invoicing_Details_Tax S1 Left Join Accounts.dbo.Ledger_Master L1 on S1.Tax_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' Where 1 = 2 Order by S1.Slno ", ref Dt_Tax);
                }
                else
                {
                    Grid_Tax.DataSource = MyBase.Load_Data("Select S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From Socks_GRN_Invoicing_Details_Tax S1 Left Join Accounts.dbo.Ledger_Master L1 on S1.Tax_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' Where S1.Master_ID = " + Code + " Order by S1.Slno ", ref Dt_Tax);
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

        void Grid_Data()
        {
            try
            {
                Dt = new DataTable();
                if (MyParent._New)
                {
                    if (CmbMode.Text == "YARN")
                    {
                        Grid.DataSource = MyBase.Load_Data("Select 0 as Sl, Item_ID, Item, Color_ID, Color, Size_ID, Size, Sum(GRN_Qty) GRN_Qty, Rate, Sum((GRN_Qty * Rate)) PO_Amount, Sum(GRN_Qty) BIll_Qty, Rate Bill_Rate, Sum((GRN_Qty * Rate)) Bill_Amount From Socks_Yrn_Grn_Invoicing_Pending (" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Where Grn_MasterID in (" + OCN_List + ") Group by Item_ID, Item, Color_ID, Color, Size_ID, Size, Rate", ref Dt);
                    }
                    else
                    {
                        Grid.DataSource = MyBase.Load_Data("Select 0 as Sl, Item_ID, Item, Color_ID, Color, Size_ID, Size, Sum(GRN_Qty) GRN_Qty, Rate, Sum((GRN_Qty * Rate)) PO_Amount, Sum(GRN_Qty) BIll_Qty, Rate Bill_Rate, Sum((GRN_Qty * Rate)) Bill_Amount From Socks_Trims_Grn_Invoicing_Pending (" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Where Grn_MasterID in (" + OCN_List + ") Group by Item_ID, Item, Color_ID, Color, Size_ID, Size, Rate", ref Dt);
                    }
                }
                else
                {
                    Grid.DataSource = MyBase.Load_Data("Select S1.Slno Sl, S1.Item_ID, I1.Item, S1.Color_ID, C1.Color, S1.Size_ID, S2.Size, S1.GRN_Qty, S1.Rate, S1.PO_Amount, S1.Bill_Qty, S1.Bill_Rate, S1.Bill_Amount From Socks_GRN_Invoicing_Details S1 Left Join Item I1 On S1.Item_ID = I1.itemid Left Join Color C1 On S1.Color_ID = C1.colorid Left Join Size S2 On S1.Size_ID = S2.Sizeid Where S1.Master_ID = " + Code + " Order by S1.SLno", ref Dt);
                }

                MyBase.Grid_Designing(ref Grid, ref Dt, "Item_ID", "Color_ID", "Size_ID");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Bill_Qty", "Bill_Rate");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 40, 140, 110, 90, 90, 90, 100, 90, 90, 100);

                if (CmbMode.Text == "YARN")
                {
                    Grid.Columns["Rate"].DefaultCellStyle.Format = "0.00";
                    Grid.Columns["Bill_Rate"].DefaultCellStyle.Format = "0.00";
                }
                else
                {
                    Grid.Columns["Rate"].DefaultCellStyle.Format = "0.0000";
                    Grid.Columns["Bill_Rate"].DefaultCellStyle.Format = "0.0000";
                }



                MyBase.Row_Number(ref Grid);

                Grid.RowHeadersWidth = 10;

                Grid.CurrentCell = Grid["Bill_Qty", 0];
                Grid.Focus();
                Grid.BeginEdit(true);

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
                if (Dt_Grn == null)
                {
                    MessageBox.Show("Invalid Data's ...!", "Gainup");
                    return;
                }

                for (int i = 0; i <= Dt_Grn.Rows.Count - 1; i++)
                {
                    if (Grid_Grn["Status", i].Value != null && Grid_Grn["Status", i].Value != DBNull.Value && Grid_Grn["Status", i].Value.ToString().ToUpper() == "true".ToUpper())
                    {
                        if (OCN_List == String.Empty)
                        {
                            OCN_List = Grid_Grn["GRN_MasterID", i].Value.ToString();
                        }
                        else
                        {
                            OCN_List += "," + Grid_Grn["GRN_MasterID", i].Value.ToString();
                        }
                    }
                }

                if (OCN_List == String.Empty)
                {
                    MessageBox.Show("Invalid OCN List ...!", "Gainup");
                    return;
                }

                button1.Enabled = false;
                Grid_Grn.Enabled = false;

                if (Dt.Rows.Count > 0)
                {
                    if (MessageBox.Show("Sure to Clear existing Details ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                Grid_Data();
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

        void Refresh_Tax()
        {
            try
            {
                for (int i = 0; i <= Dt_Tax.Rows.Count - 1; i++)
                {
                    if (Grid_Tax["Tax_Mode", i].Value.ToString() == "Y")
                    {
                        Grid_Tax["Tax_Amount", i].Value = String.Format("{0:0}", Convert.ToDouble(Grid_Tax["Tax_Per", i].Value) * (Convert.ToDouble(TxtBillGross.Text) / 100));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                            Grid_Tax["Tax_Amount", Grid_Tax.CurrentCell.RowIndex].Value = Convert.ToDouble(String.Format("{0:0}", (Convert.ToDouble(TxtPOGross.Text) / 100) * Convert.ToDouble(Tdt.Rows[0][0])));
                        }
                        else
                        {
                            Grid_Tax["Tax_Mode", Grid_Tax.CurrentCell.RowIndex].Value = "N";
                            Grid_Tax["Tax_Per", Grid_Tax.CurrentCell.RowIndex].Value = "0.00";

                            Grid_Tax.CurrentCell = Grid_Tax["Tax_Amount", Grid_Tax.CurrentCell.RowIndex];
                            Grid_Tax.Focus();
                            Grid_Tax.BeginEdit(true);
                        }

                        Bill_Amount();
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Bill_Qty"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Bill_Rate"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Bill_Rate"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Bill_Qty"].Index)
                    {
                        if (Grid["Bill_Qty", Grid.CurrentCell.RowIndex].Value == null || Grid["Bill_Qty", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Bill_Qty", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Bill_Qty", Grid.CurrentCell.RowIndex].Value = "0.00";
                        }

                        if (Grid["Bill_Rate", Grid.CurrentCell.RowIndex].Value == null || Grid["Bill_Rate", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Bill_Rate", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Bill_Rate", Grid.CurrentCell.RowIndex].Value = "0.00";
                        }

                        if (Convert.ToDouble(Grid["Bill_Qty", Grid.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Bill Qty ...!", "Gainup");
                            Grid.CurrentCell = Grid["Bill_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (Convert.ToDouble(Grid["Bill_Rate", Grid.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Bill Rate ...!", "Gainup");
                            Grid.CurrentCell = Grid["Bill_Rate", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        //if (Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) < Convert.ToDouble(Grid["Bill_Qty", Grid.CurrentCell.RowIndex].Value))
                        //{
                        //    e.Handled = true;
                        //    MessageBox.Show("Bill Qty is greater than GRN Qty...!", "Gainup");
                        //    Grid["Bill_Qty", Grid.CurrentCell.RowIndex].Value = Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value;
                        //    Grid.CurrentCell = Grid["Bill_Qty", Grid.CurrentCell.RowIndex];
                        //    Grid.Focus();
                        //    Grid.BeginEdit(true);
                        //    return;
                        //}

                        Grid["Bill_Amount", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["Bill_Qty", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["Bill_Rate", Grid.CurrentCell.RowIndex].Value);

                        if (!Bill_Amount())
                        {
                            return;
                        };

                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Bill_Rate"].Index)
                        {
                            if (Grid.CurrentCell.RowIndex == Grid.Rows.Count - 1)
                            {
                                Grid_Tax.CurrentCell = Grid_Tax["Tax", 0];
                                Grid_Tax.Focus();
                                Grid_Tax.BeginEdit(true);
                                return;
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

        void Debit_Amount()
        {
            Double Rate_Debit = 0;
            Double Qty_Debit = 0;

            Double Min_Qty = 0;
            Double Min_Rate = 0;
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    // Rate Debit Generation
                    //if (Convert.ToDouble(Grid["Bill_Qty", i].Value) >= Convert.ToDouble(Grid["Qty", i].Value))
                    //{
                        Min_Qty = Convert.ToDouble(Grid["GRN_Qty", i].Value);
                    //}
                    //else
                    //{
                        //Min_Qty = Convert.ToDouble(Grid["Bill_Qty", i].Value);
                    //}
                    if (Convert.ToDouble(Grid["Bill_Rate", i].Value) > Convert.ToDouble(Grid["Rate", i].Value))
                    {
                        Rate_Debit += (Min_Qty * Convert.ToDouble(Grid["Bill_Rate", i].Value)) - (Min_Qty * Convert.ToDouble(Grid["Rate", i].Value));
                    }

                    // Qty Debit Generation
                    //if (Convert.ToDouble(Grid["Bill_Rate", i].Value) >= Convert.ToDouble(Grid["Rate", i].Value))
                    //{
                        Min_Rate = Convert.ToDouble(Grid["Rate", i].Value);
                    //}
                    //else
                    //{
                        //Min_Rate = Convert.ToDouble(Grid["Bill_Rate", i].Value);
                    //}
                    if (Convert.ToDouble(Grid["Bill_Qty", i].Value) > Convert.ToDouble(Grid["GRN_Qty", i].Value))
                    {
                        Qty_Debit += (Convert.ToDouble(Grid["Bill_Qty", i].Value) * Min_Rate) - (Convert.ToDouble(Grid["GRN_Qty", i].Value) * Min_Rate);
                    }
                }

                TxtRateDifference.Text = String.Format ("{0:n}", Convert.ToDouble(String.Format("{0:0}", Rate_Debit)));
                TxtQtyDifference.Text = String.Format ("{0:n}", Convert.ToDouble(String.Format("{0:0}", Qty_Debit)));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Boolean Bill_Amount()
        {
            try
            {

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Bill_Qty", i].Value == null || Grid["Bill_Qty", i].Value == DBNull.Value || Grid["Bill_Qty", i].Value.ToString() == String.Empty)
                    {
                        Grid["Bill_Qty", i].Value = "0.00";
                    }

                    if (Convert.ToDouble(Grid["Bill_Qty", i].Value) == 0)
                    {
                        MessageBox.Show("Invalid Bill Qty ...!", "Gainup");
                        Grid.CurrentCell = Grid["Bill_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return false;   
                    }

                    if (Convert.ToDouble(Grid["Bill_Rate", i].Value) == 0)
                    {
                        MessageBox.Show("Invalid Bill Rate ...!", "Gainup");
                        Grid.CurrentCell = Grid["Bill_Rate", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return false;
                    }

                    //if (Convert.ToDouble(Grid["GRN_Qty", i].Value) < Convert.ToDouble(Grid["Bill_Qty", i].Value))
                    //{
                    //    MessageBox.Show("Bill Qty is greater than GRN Qty...!", "Gainup");
                    //    Grid["Bill_Qty", i].Value = Grid["GRN_Qty", i].Value;
                    //    Grid.CurrentCell = Grid["Bill_Qty", i];
                    //    Grid.Focus();
                    //    Grid.BeginEdit(true);
                    //    return false;
                    //}

                    Grid["Bill_Amount", i].Value = String.Format ("{0:0.00}", Convert.ToDouble(Grid["Bill_Qty", i].Value) * Convert.ToDouble(Grid["Bill_Rate", i].Value));
                }

                MyBase.Row_Number(ref Grid);

                TxtPOGross.Text = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "PO_Amount", "GRN_Qty", "Rate", "Item")));
                TxtBillGross.Text = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "Bill_Amount", "GRN_Qty", "Rate", "Item")));
                Refresh_Tax();

                TxtBillTax.Text = String.Format ("{0:n}", Convert.ToDouble(String.Format ("{0:0}", Convert.ToDouble(String.Format("{0:0}", MyBase.Sum(ref Grid_Tax, "Tax_Amount", "Tax_Code", "Tax"))))));
                TxtPOTax.Text = String.Format ("{0:n}", Convert.ToDouble(String.Format ("{0:0}", (Convert.ToDouble(TxtBillTax.Text) / Convert.ToDouble(TxtBillGross.Text)) * Convert.ToDouble(TxtPOGross.Text))));

                TxtPONet.Text = String.Format ("{0:n}", Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(TxtPOGross.Text) + Convert.ToDouble(TxtPOTax.Text))));
                TxtBillNet.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(TxtBillGross.Text) + Convert.ToDouble(TxtBillTax.Text))));

                Debit_Amount();
                TxtToBePaid.Text = String.Format ("{0:n}", Convert.ToDouble(String.Format ("{0:0}", Convert.ToDouble(TxtBillNet.Text) - (Convert.ToDouble(TxtRateDifference.Text) + Convert.ToDouble(TxtQtyDifference.Text)))));

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void Grid_Tax_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid_Tax.CurrentCell.ColumnIndex == Grid_Tax.Columns["Tax_Amount"].Index)
                    {
                        Bill_Amount();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Tax_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    TxtToBePaid.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}