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
        String Mode_List = String.Empty;
        String Grn_List = String.Empty;
        String Old_Inv_No = String.Empty;
        DateTime Old_Inv_Date = DateTime.Now;
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
                ChkTax.Checked = false;
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
                Old_Inv_No = TxtInvoiceNo.Text;
                TxtInvoiceNo.Text = Dr["Invoice_No"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();
                TxtRO.Text = Dr["RO_Amt"].ToString();
                DtpInvoiceDate.Value = Convert.ToDateTime(Dr["Invoice_Date"]);
                Old_Inv_Date = DtpInvoiceDate.Value;
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
                MyBase.Load_Data("Select Distinct (Case When Mode Like '%TRIM%' Then 1 Else 0 End) from Socks_GRN_Invoicing_Details_OCN Where Master_ID = " + Code, ref Tdt);
                if (Tdt.Rows.Count >0)
                {
                    CmbMode.SelectedIndex = Convert.ToInt32(Tdt.Rows[0][0].ToString());
                }
                Load_Grn();


                for (int i = 0; i <= Dt_Grn.Rows.Count - 1; i++)
                {
                    Grid_Grn["Status", i].Value = true;
                }
                if (Convert.ToInt16(Dr["Tax_Calc_Mode"].ToString()) == 1)
                {
                    ChkTax.Checked = true;
                }
                else
                {
                    ChkTax.Checked = false;
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Entry - Edit", "Select S1.Entry_No, S1.Entry_Date, L1.Ledger_Name SUpplier, S1.Invoice_No, S1.Invoice_Date, S1.Supplier_Code, S1.RowID Code, S1.Remarks, S1.Tax_Calc_Mode, S1.RO_Amt From Socks_GRN_Invoicing_Master S1  Left join FitSocks.Dbo.Supplier_All_Fn() L1 on S1.Supplier_Code = L1.Ledger_Code  Left Join ACCOUNTS.dbo.GST_Grn_Approval A1 On A1.Invoice_No = S1.Invoice_No and A1.Invoice_Date = S1.Invoice_Date and A1.Supplier_Id = S1.Supplier_Code  Left Join ACCOUNTS.dbo.ERp_Accounts_Socks_Combo A2 On A2.InvNo = S1.Invoice_No and A2.Invdate = S1.Invoice_Date Where A1.RowID Is Null and A2.VCode IS Null", String.Empty, 120, 100, 250, 120, 100);
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

                DataTable Dts = new DataTable();
                String St1 = "Select * From ACCOUNTS.dbo.Blocked_Ledgers_List(1) Where Ledger_Code= " + TxtSupplier.Tag.ToString() + "";
                MyBase.Load_Data(St1, ref Dts);
                if (Dts.Rows.Count > 0)
                {
                    MessageBox.Show("This Supplier Has Been Blocked By Accounts...!");
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

                if (TxtRO.Text.ToString() != String.Empty) 
                {
                    if (Convert.ToDouble(TxtRO.Text.ToString()) != -1 && Convert.ToDouble(TxtRO.Text.ToString()) != 1 && Convert.ToDouble(TxtRO.Text.ToString()) != 0)
                    {
                        MessageBox.Show("Invalid RO Amount (-1 & 1 Only Allowed) ...!", "Gainup");
                        MyParent.Save_Error = true;
                        TxtRO.Focus();
                        return;
                    }
                }

                if (TxtBillNet.Text.Trim() == String.Empty || Convert.ToDouble(TxtBillNet.Text) == 0)
                {
                    MessageBox.Show("Invalid Data's ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtInvoiceNo.Focus();
                    return;
                }

                //if (TxtPONet.Text.Trim() == String.Empty || Convert.ToDouble(TxtPONet.Text) == 0)
                //{
                //    MessageBox.Show("Invalid Data's ...!", "Gainup");
                //    MyParent.Save_Error = true;
                //    TxtInvoiceNo.Focus();
                //    return;
                //}

                Bill_Rate_Calc(-1);
                Bill_Amount();

                if (MyParent._New)
                {
                    GRN_Generate();
                }

                Int16 CTax = 0;

                if (ChkTax.Checked == true)
                {
                    CTax = 1;
                }

                String[] Queries = new String[(Grid.Rows.Count + Grid_Grn.Rows.Count)  * 10];
                Int32 Array_Index = 0;

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert Into Socks_GRN_Invoicing_Master (Entry_No, Entry_Date, Supplier_Code, Invoice_No, Invoice_Date, PO_Gross, PO_Tax, PO_Net, Bill_Gross, Bill_Tax, Bill_Net, To_Be_Paid, Rate_Debit, Qty_Debit, Approved_Debit, Remarks, Approval, Tax_Calc_Mode, RO_Amt) Values ('" + TxtEntryNo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtSupplier.Tag.ToString() + ", '" + TxtInvoiceNo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpInvoiceDate.Value) + "', " + Convert.ToDouble(TxtPOGross.Text) + ", " + Convert.ToDouble(TxtPOTax.Text) + ", " + Convert.ToDouble(TxtPONet.Text) + ", " + Convert.ToDouble(TxtBillGross.Text) + ", " + Convert.ToDouble(TxtBillTax.Text) + ", " + Convert.ToDouble(TxtBillNet.Text) + ", " + Convert.ToDouble(TxtToBePaid.Text) + ", " + Convert.ToDouble(TxtRateDifference.Text) + ", " + Convert.ToDouble(TxtQtyDifference.Text) + ", " + Convert.ToDouble(Convert.ToDouble(TxtRateDifference.Text) + Convert.ToDouble(TxtQtyDifference.Text)) + ", '" + TxtRemarks.Text + "', 'N', " + CTax + ", " + Convert.ToDouble(TxtRO.Text) + " ); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("SOCKS INVOICE", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_GRN_Invoicing_Master Set Approval = 'N', Invoice_No = '" + TxtInvoiceNo.Text + "', Invoice_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpInvoiceDate.Value) + "',  PO_Gross = " + Convert.ToDouble(TxtPOGross.Text) + ", PO_Tax = " + Convert.ToDouble(TxtPOTax.Text) + ", PO_Net = " + Convert.ToDouble(TxtPONet.Text) + ", Bill_Gross = " + Convert.ToDouble(TxtBillGross.Text) + ", Bill_Tax = " + Convert.ToDouble(TxtBillTax.Text) + ", Bill_Net = " + Convert.ToDouble(TxtBillNet.Text) + ", To_Be_Paid = " + Convert.ToDouble(TxtToBePaid.Text) + ",  Rate_Debit = " + Convert.ToDouble(TxtRateDifference.Text) + ", Qty_Debit = " + Convert.ToDouble(TxtQtyDifference.Text) + ", Approved_Debit = " + Convert.ToDouble(Convert.ToDouble(TxtRateDifference.Text) + Convert.ToDouble(TxtQtyDifference.Text)) + ", Remarks = '" + TxtRemarks.Text + "', Tax_CAlc_Mode = " + CTax + ", RO_Amt = " + Convert.ToDouble(TxtRO.Text) + " Where RowID = " + Code;
                    Queries[Array_Index++] = "Update Accounts.Dbo.Fit_Bill_Master Set InvNo = '" + TxtInvoiceNo.Text + "', InvDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpInvoiceDate.Value) + "' Where  Invno = '" + Old_Inv_No + "' And cAST(Invdate AS dATE)= '" + String.Format("{0:dd-MMM-yyyy}", Old_Inv_Date) + "' ";
                    Queries[Array_Index++] = "Delete From Socks_GRN_Invoicing_Details_OCN Where Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_GRN_Invoicing_Details_Tax Where Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_GRN_Invoicing_Details Where Master_ID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("SOCKS INVOICE", "EDIT", Code.ToString());
                }

                for (int i = 0; i <= Grid_Grn.Rows.Count - 1; i++)
                {
                    if (Grid_Grn["Status", i].Value != null && Grid_Grn["Status", i].Value != DBNull.Value && Grid_Grn["Status", i].Value.ToString().ToUpper() == "true".ToUpper())
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert Into Socks_GRN_Invoicing_Details_OCN (Master_ID, Slno, GRN_MasterID, Mode) Values (@@IDENTITY, " + (i + 1) + ", " + Grid_Grn["GRN_MasterID", i].Value.ToString() + ", '" + Grid_Grn["Mode", i].Value.ToString() + "')";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert Into Socks_GRN_Invoicing_Details_OCN (Master_ID, Slno, GRN_MasterID, Mode) Values (" + Code + ", " + (i + 1) + ", " + Grid_Grn["GRN_MasterID", i].Value.ToString() + ", '" + Grid_Grn["Mode", i].Value.ToString() + "')";
                        }
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Socks_GRN_Invoicing_Details (Master_ID, Slno, Item_ID, Color_ID, Size_ID, GRN_Qty, Rate, PO_Amount, Bill_Qty, Bill_Rate, Bill_Amount, Loss_Perc, Qty_Deb, Rate_Deb, Acc_Qty, Total_Rate, Total_Bud_Cost, Budget_NRate, Freight_BTax, OCharges_BTax, Tax_Per, Bill_Gross, Tax_Amount, Freight_ATax, OCharges_ATax, Bill_NRate, Budget_Tax, Budget_Freight, Budget_Others) Values (@@IDENTITY, " + (i + 1) + ", " + Dt.Rows[i]["Item_ID"].ToString() + ", " + Dt.Rows[i]["Color_ID"].ToString() + ", " + Dt.Rows[i]["Size_ID"].ToString() + ", " + Dt.Rows[i]["GRN_Qty"].ToString() + ", " + Dt.Rows[i]["Rate"].ToString() + ", " + Dt.Rows[i]["PO_AMount"].ToString() + ", " + Dt.Rows[i]["Bill_Qty"].ToString() + ", " + Dt.Rows[i]["Bill_Rate"].ToString() + ", " + Dt.Rows[i]["Bill_Amount"].ToString() + ", " + Dt.Rows[i]["Loss_Perc"].ToString() + ", " + Dt.Rows[i]["Qty_Deb"].ToString() + ", " + Dt.Rows[i]["Rate_Deb"].ToString() + ", " + Dt.Rows[i]["Acc_Qty"].ToString() + ", " + Dt.Rows[i]["Total_Rate"].ToString() + ", " + Dt.Rows[i]["Total_Rate1"].ToString() + ", " + Dt.Rows[i]["Budget_Rate"].ToString() + ", " + Dt.Rows[i]["Freight"].ToString() + ", " + Dt.Rows[i]["Others"].ToString() + ", " + Dt.Rows[i]["Tax_Per"].ToString() + ", " + Dt.Rows[i]["Bill_Gross"].ToString() + ", " + Dt.Rows[i]["Tax_Amount"].ToString() + ", " + Dt.Rows[i]["Freight1"].ToString() + ", " + Dt.Rows[i]["Others1"].ToString() + ", " + Dt.Rows[i]["Bill_NRate"].ToString() + ", " + Dt.Rows[i]["Budget_Tax"].ToString() + ", " + Dt.Rows[i]["Budget_Freight"].ToString() + ", " + Dt.Rows[i]["Budget_Others"].ToString() + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Socks_GRN_Invoicing_Details (Master_ID, Slno, Item_ID, Color_ID, Size_ID, GRN_Qty, Rate, PO_Amount, Bill_Qty, Bill_Rate, Bill_Amount, Loss_Perc, Qty_Deb, Rate_Deb, Acc_Qty, Total_Rate, Total_Bud_Cost, Budget_NRate, Freight_BTax, OCharges_BTax, Tax_Per, Bill_Gross, Tax_Amount, Freight_ATax, OCharges_ATax, Bill_NRate, Budget_Tax, Budget_Freight, Budget_Others) Values (" + Code + ", " + (i + 1) + ", " + Dt.Rows[i]["Item_ID"].ToString() + ", " + Dt.Rows[i]["Color_ID"].ToString() + ", " + Dt.Rows[i]["Size_ID"].ToString() + ", " + Dt.Rows[i]["GRN_Qty"].ToString() + ", " + Dt.Rows[i]["Rate"].ToString() + ", " + Dt.Rows[i]["PO_AMount"].ToString() + ", " + Dt.Rows[i]["Bill_Qty"].ToString() + ", " + Dt.Rows[i]["Bill_Rate"].ToString() + ", " + Dt.Rows[i]["Bill_Amount"].ToString() + ", " + Dt.Rows[i]["Loss_Perc"].ToString() + ", " + Dt.Rows[i]["Qty_Deb"].ToString() + ", " + Dt.Rows[i]["Rate_Deb"].ToString() + ", " + Dt.Rows[i]["Acc_Qty"].ToString() + ", " + Dt.Rows[i]["Total_Rate"].ToString() + ", " + Dt.Rows[i]["Total_Rate1"].ToString() + ", " + Dt.Rows[i]["Budget_Rate"].ToString() + ", " + Dt.Rows[i]["Freight"].ToString() + ", " + Dt.Rows[i]["Others"].ToString() + ", " + Dt.Rows[i]["Tax_Per"].ToString() + ", " + Dt.Rows[i]["Bill_Gross"].ToString() + ", " + Dt.Rows[i]["Tax_Amount"].ToString() + ", " + Dt.Rows[i]["Freight1"].ToString() + ", " + Dt.Rows[i]["Others1"].ToString() + ", " + Dt.Rows[i]["Bill_NRate"].ToString() + ", " + Dt.Rows[i]["Budget_Tax"].ToString() + ", " + Dt.Rows[i]["Budget_Freight"].ToString() + ", " + Dt.Rows[i]["Budget_Others"].ToString() + ")";
                    }
                }

                for (int i = 0; i <= Grid_Tax.Rows.Count - 2; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert Into Socks_GRN_Invoicing_Details_Tax (Master_ID, Slno, Tax_Code, Mode, Tax_Per, Tax_Amount) Values (@@IDENTITY, " + (i + 1) + ", " + Grid_Tax["Tax_Code", i].Value.ToString() + ", '" +  Grid_Tax["Tax_Mode", i].Value.ToString() + "', " + Grid_Tax["Tax_Per", i].Value.ToString() + ", " +  Grid_Tax["Tax_Amount", i].Value.ToString() + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert Into Socks_GRN_Invoicing_Details_Tax (Master_ID, Slno, Tax_Code, Mode, Tax_Per, Tax_Amount) Values (" + Code + ", " + (i + 1) + ", " + Grid_Tax["Tax_Code", i].Value.ToString() + ", '" + Grid_Tax["Tax_Mode", i].Value.ToString() + "', " + Grid_Tax["Tax_Per", i].Value.ToString() + ", " + Grid_Tax["Tax_Amount", i].Value.ToString() + ")";                        
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

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, false);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Entry - Delete", "Select S1.Entry_No, S1.Entry_Date, L1.Ledger_Name SUpplier, S1.Invoice_No, S1.Invoice_Date, S1.Supplier_Code, S1.RowID Code, S1.Remarks, S1.Tax_Calc_Mode, S1.Ro_Amt From Socks_GRN_Invoicing_Master S1 Left join Accounts.Dbo.Ledger_MAster L1 on S1.Supplier_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " And Approval = 'N' And L1.YEAR_CODE = '" + MyParent.YearCode + "' Left Join ACCOUNTS.dbo.GST_Grn_Approval A1 On A1.Invoice_No = S1.Invoice_No and A1.Invoice_Date = S1.Invoice_Date and A1.Supplier_Id = S1.Supplier_Code  Left Join ACCOUNTS.dbo.ERp_Accounts_Socks_Combo A2 On A2.InvNo = S1.Invoice_No and A2.Invdate = S1.Invoice_Date Where A1.RowID Is Null and A2.VCode IS Null", String.Empty, 120, 100, 250, 120, 100);
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
                    MyBase.Run("Delete from Socks_GRN_Invoicing_Details_OCN Where MAster_ID = " + Code, "Delete from Socks_GRN_Invoicing_Details_Tax Where MAster_ID = " + Code, "Delete from Socks_GRN_Invoicing_Details Where MAster_ID = " + Code, "Delete From Socks_GRN_Invoicing_Master Where RowID = " + Code, MyParent.EntryLog("SOCKS INVOICE", "DELETE", Code.ToString()));
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
                Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Entry - View", "Select Distinct S1.Entry_No, S1.Entry_Date, Case When S4.GRNNo is null And S5.GRNNo is null And S6.GRNNo is null Then S7.GRNNo When S4.GRNNo is null And S5.GRNNo is null And S7.GRNNo is null Then S6.GRNNo When S4.GRNNo is null And S6.GRNNo is null And S7.GRNNo is null Then S5.GRNNo Else S4.GRNNo End Grn_No, Case When S4.GRNDate is null And S5.GRNDate is null And S6.GRNDate is null Then S7.GRNDate When S4.GRNDate is null And S5.GRNDate is null And S7.GRNDate is null Then S6.GRNDate When S4.GRNDate is null And S6.GRNDate is null And S7.GRNDate is null Then S5.GRNDate Else S4.GRNDate End GRNDate, L1.Ledger_Name SUpplier, S1.Invoice_No, S1.Invoice_Date, S1.Bill_Gross, S1.Bill_Tax, S1.Bill_Net, S1.To_Be_paid, S1.Rate_Debit, S1.Qty_Debit, S1.Approved_Debit,  S1.Supplier_Code, S1.RowID Code, S1.Remarks, S1.Tax_Calc_Mode, S1.RO_Amt From Socks_GRN_Invoicing_Master S1 Left join FitSocks.Dbo.Supplier_All_Fn() L1 on S1.Supplier_Code = L1.Ledger_Code Left Join Socks_GRN_Invoicing_Details S2 on S1.RowID = S2.Master_ID Left Join Socks_GRN_Invoicing_Details_OCN S3 on S1.RowID = S3.Master_ID Left Join Socks_Yarn_GRN_Master S4 on S3.GRN_MasterID = S4.Rowid And S3.Mode='YARN' Left Join Socks_Yarn_GRN_Dyeing_Master S5 on S3.GRN_MasterID = S5.Rowid And S3.Mode='YARN DYEING' Left Join Socks_Trims_GRN_Master S6 on S3.GRN_MasterID = S6.RowID And S3.Mode='TRIMS' Left Join Socks_Sample_Yarn_GRN_Master S7 on S3.GRN_MasterID = S7.RowID And S3.Mode='SAM COV YARN'", String.Empty, 120, 100, 120, 100, 250, 120, 100, 100, 100, 100, 100,100,100,100);
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

                Str = " Select S1.Entry_No PONo, L1.Ledger_InPrint Supplier, S1.Invoice_No Bill_No, Cast(S1.Entry_Date As date)PoDate, Cast(S1.Invoice_Date as Date) Required_Date, (Case When S2.Item_Type='Yarn' Then 'Yarn' Else 'Trims' End) PO_Method,'' Supplier_Address, L1.Ledger_Phone Supplier_Phone, L1.Ledger_email Supplier_Email, S1.RO_Amt From Socks_GRN_Invoicing_Master S1 Left Join Dbo.Supplier_All_Fn() L1 on S1.Supplier_Code = L1.Ledger_Code Left Join (Select Distinct A.Master_Id, B.Item_Type from Socks_GRN_Invoicing_Details A Left Join Item B on A.Item_ID = B.itemid) S2 on S1.RowID = S2.Master_ID Where S1.RowID = " + Code;
                MyBase.Load_Data(Str, ref Dt1);

                Str1 = "Select Top 100000000 ROW_NUMBER()Over(Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size)SlNo, I1.Item + ' - ' + C1.color + ' - ' + S4.Size Item_Color_Size, Sum(S2.Bill_Qty) Order_Qty,  S2.Bill_Rate Rate,S2.Tax_Per,  (Sum(S2.Bill_Gross)- Sum(TaX_Amount)) Bill_GRoss, Sum(TaX_Amount) TaX_Amount , Sum(S2.Bill_Gross) Amount, S1.Rate_Debit, S1.Qty_Debit, S1.To_Be_paid  From Socks_GRN_Invoicing_Master S1 Inner join Socks_GRN_Invoicing_Details S2 ON S1.RowID = S2.Master_ID  Inner join Item I1 on S2.Item_id = I1.itemid Inner join color C1 on s2.Color_id = c1.colorid  Inner join size S4 on s2.Size_ID = S4.sizeid Where S1.RowID = " + Code + "  Group by I1.Item, C1.color, S4.Size, S2.Bill_Rate  , S2.Tax_PEr, S1.Rate_Debit, S1.Qty_Debit, S1.To_Be_paid  Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size ";
                //Str1 = " Select Top 100000000 ROW_NUMBER()Over(Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size)SlNo, I1.Item + ' - ' + C1.color + ' - ' + S4.Size Item_Color_Size, S2.Bill_Qty Order_Qty, S2.Bill_Rate Rate, S2.Bill_Amount Amount From Socks_GRN_Invoicing_Master S1 Inner join Socks_GRN_Invoicing_Details S2 ON S1.RowID = S2.Master_ID Inner join Item I1 on S2.Item_id = I1.itemid Inner join color C1 on s2.Color_id = c1.colorid Inner join size S4 on s2.Size_ID = S4.sizeid Where S1.RowID = " + Code + " Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size ";
                MyBase.Execute_Qry(Str1, "Socks_Purchase_Inv");

                Str2 = " Select Top 4 S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From Socks_GRN_Invoicing_Details_Tax S1 Left Join Socks_Tax_Accounts() L1 on S1.Tax_Code = L1.Ledger_Code  Where S1.Master_ID = " + Code + " Order by S1.Slno ";
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
                MyParent.FormulaFill(ref ObjRpt, "Net_Amount_Word", MyBase.Rupee(Convert.ToDouble(TxtBillNet.Text.ToString())));
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
                        Grid_Grn.DataSource = MyBase.Load_Data("Select Distinct GrnNo GRNNO, GrnDate GRNDATE, GRN_MasterID, Mode From Socks_Yrn_Grn_Invoicing_Pending (" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Where Supplier_Code = " + TxtSupplier.Tag.ToString() + " Order by GrnNo", ref Dt_Grn);
                    }
                    else
                    {
                        Grid_Grn.DataSource = MyBase.Load_Data("Select Distinct GrnNo GRNNO, GrnDate GRNDATE, GRN_MasterID, Mode From Socks_Trims_Grn_Invoicing_Pending (" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Where Supplier_Code = " + TxtSupplier.Tag.ToString() + " Order by GrnNo", ref Dt_Grn);
                    }
                }
                else
                {
                    if (CmbMode.Text.Trim() == "YARN")
                    {
                        Grid_Grn.DataSource = MyBase.Load_Data("Select Distinct S2.GrnNo GRNNO, S2.GrnDate GRNDATE, S1.GRN_MasterID, S1.Mode From Socks_GRN_Invoicing_Details_OCN S1 Left Join Socks_Yarn_Grn_Master S2 on S1.GRN_MasterID = S2.RowID and S1.Mode = 'Yarn' Where S1.Master_ID = " + Code + " and S2.RowID is not null Union All Select Distinct S2.GrnNo GRNNO, S2.GrnDate GRNDATE, S1.GRN_MasterID, S1.Mode From Socks_GRN_Invoicing_Details_OCN S1 Left Join Socks_Yarn_Grn_Dyeing_Master S2 on S1.GRN_MasterID = S2.RowID and S1.Mode like  'YARN DYEING' Where S1.Master_ID = " + Code + " and S2.RowID is not null     Union All Select Distinct S2.GrnNo GRNNO, S2.GrnDate GRNDATE, S1.GRN_MasterID, S1.Mode From Socks_GRN_Invoicing_Details_OCN S1 Left Join Socks_Sample_Yarn_Grn_Master S2 on S1.GRN_MasterID = S2.RowID and S1.Mode = 'Sam Cov Yarn' Where S1.Master_ID = " + Code + " and S2.RowID is not null  Union All Select Distinct S2.GrnNo GRNNO, S2.GrnDate GRNDATE, S1.GRN_MasterID, S1.Mode From Socks_GRN_Invoicing_Details_OCN S1 Left Join Socks_General_Grn_Master S2 on S1.GRN_MasterID = S2.RowID and S1.Mode = 'GENERAL YARN' Where S1.Master_ID = " + Code + " and S2.RowID is not null  Order By GrnNo", ref Dt_Grn);
                    }
                    else
                    {
                        Grid_Grn.DataSource = MyBase.Load_Data("Select Distinct S2.GrnNo GRNNO, S2.GrnDate GRNDATE, S1.GRN_MasterID, S1.Mode From Socks_GRN_Invoicing_Details_OCN S1 Inner Join Socks_Trims_Grn_Master S2 on S1.GRN_MasterID = S2.RowID Where S1.Master_ID = " + Code + " and S1.Mode = 'TRIMS' Union All Select Distinct S2.GrnNo GRNNO, S2.GrnDate GRNDATE, S1.GRN_MasterID, S1.Mode From Socks_GRN_Invoicing_Details_OCN S1 Inner Join Socks_General_Grn_Master S2 on S1.GRN_MasterID = S2.RowID Where S1.Master_ID = " + Code + " and S1.Mode = 'GENERAL TRIMS' Order By S2.GrnNo", ref Dt_Grn);
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
                            DataTable Dts = new DataTable();
                            String St1 = "Select * From ACCOUNTS.dbo.Blocked_Ledgers_List(1) Where Ledger_Code= " + Dr["Supplier_Code"].ToString() + "";
                            MyBase.Load_Data(St1, ref Dts);
                            if (Dts.Rows.Count > 0)
                            {
                                MessageBox.Show("This Supplier Has Been Blocked By Accounts...!");
                                TxtSupplier.Focus();
                                return;
                            }
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
                    else if (this.ActiveControl.Name == "TxtRO")
                    {
                        MyBase.Valid_DecimalPlusMinus((TextBox)this.ActiveControl, e);
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
                    Grid_Tax.DataSource = MyBase.Load_Data("Select S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From Socks_GRN_Invoicing_Details_Tax S1 Left Join Socks_Tax_Accounts() L1 on S1.Tax_Code = L1.Ledger_Code  Where 1 = 2 Order by S1.Slno ", ref Dt_Tax);
                }
                else
                {
                    Grid_Tax.DataSource = MyBase.Load_Data("Select S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From Socks_GRN_Invoicing_Details_Tax S1 Left Join Socks_Tax_Accounts() L1 on S1.Tax_Code = L1.Ledger_Code  Where S1.Master_ID = " + Code + " Order by S1.Slno ", ref Dt_Tax);
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
                        //Grid.DataSource = MyBase.Load_Data("Select 0 as Sl, Item_ID, Item, Color_ID, Color, Size_ID, Size, Sum(GRN_Qty) GRN_Qty, Rate, Sum((GRN_Qty * Rate)) PO_Amount, Sum(GRN_Qty) BIll_Qty, Rate Bill_Rate, Sum((GRN_Qty * Rate)) Bill_Amount, Loss_Perc, 0.000 Qty_Deb, 0.00 Rate_Deb, Sum(Acc_Qty) Acc_Qty, Cast(Avg(Total_Rate) as Numeric(22,2)) Total_Rate, Cast(Avg(Total_Rate) as Numeric(22,2)) Total_Rate1 From Socks_Yrn_Grn_Invoicing_Pending (" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Where Grn_MasterID in (" + OCN_List + ") and GrnNo in (" + Grn_List + ") and Mode in (" + Mode_List + ") Group by Item_ID, Item, Color_ID, Color, Size_ID, Size, Rate, Loss_Perc", ref Dt);
                        Grid.DataSource = MyBase.Load_Data("Select 0 as Sl, Item_ID, Item, Color_ID, Color, Size_ID, Size, Sum(GRN_Qty) GRN_Qty, Rate, Sum((GRN_Qty * Rate)) PO_Amount, Min(Plan_Rate) Budget_Rate, Min(Tax_Rate) Budget_Tax, MIn(Freight_Rate) Budget_Freight, Min(Other_Rate) Budget_Others, Sum(GRN_Qty) BIll_Qty, Rate Bill_Rate, Sum((GRN_Qty * Rate)) Bill_Amount, 0.0000 Freight, 0.0000 Others, 0.0000 Tax_Per, 0.00 Tax_Amount, 0.0000 Freight1, 0.0000 Others1, 0.00 Bill_Gross, 0.0000 Bill_NRate, Loss_Perc, 0.000 Qty_Deb, 0.00 Rate_Deb, Sum(Acc_Qty) Acc_Qty, Cast(Avg(Total_Rate) as Numeric(22,4)) Total_Rate, Cast(Avg(Total_Rate) as Numeric(22,4)) Total_Rate1 From Socks_Yrn_Grn_Invoicing_Pending (" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Where Grn_MasterID in (" + OCN_List + ") and GrnNo in (" + Grn_List + ") and Mode in (" + Mode_List + ") Group by Item_ID, Item, Color_ID, Color, Size_ID, Size, Rate, Loss_Perc", ref Dt);
                    }
                    else
                    {
                        if (ChkTax.Checked == false)
                        {
                            Grid.DataSource = MyBase.Load_Data("Select 0 as Sl, Item_ID, Item, Color_ID, Color, Size_ID, Size, Sum(GRN_Qty) GRN_Qty, Rate, Sum((GRN_Qty * Rate)) PO_Amount, Min(Plan_Rate) Budget_Rate, Min(Tax_Rate) Budget_Tax, Min(Freight_Rate) Budget_Freight, Min(Other_Rate) Budget_Others, Sum(GRN_Qty) BIll_Qty, Rate Bill_Rate, Sum((GRN_Qty * Rate)) Bill_Amount, 0.0000 Freight, 0.0000 Others, 0.0000 Tax_Per, 0.00 Tax_Amount, 0.0000 Freight1, 0.0000 Others1, 0.00 Bill_Gross, 0.0000 Bill_NRate, Loss_Perc,  0.000 Qty_Deb, 0.00 Rate_Deb, Sum(Acc_Qty) Acc_Qty, Cast(Avg(Total_Rate) as Numeric(22,4)) Total_Rate, Cast(Avg(Total_Rate) as Numeric(22,4)) Total_Rate1  From Socks_Trims_Grn_Invoicing_Pending (" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Where Grn_MasterID in (" + OCN_List + ")  and GrnNo in (" + Grn_List + ") and Mode in (" + Mode_List + ") Group by Item_ID, Item, Color_ID, Color, Size_ID, Size, Rate, Loss_Perc", ref Dt);
                        }
                        else
                        {
                            Grid.DataSource = MyBase.Load_Data("Select 0 as Sl, Item_ID, Item, Color_ID, Color, Size_ID, Size, Sum(GRN_Qty) GRN_Qty, Rate, Sum((GRN_Qty * Rate)) PO_Amount, Min(Plan_Rate) Budget_Rate, Min(Tax_Rate) Budget_Tax, Min(Freight_Rate) Budget_Freight, Min(Other_Rate) Budget_Others, Sum(GRN_Qty) BIll_Qty, Rate Bill_Rate, Sum((GRN_Qty * Rate)) Bill_Amount, 0.0000 Freight, 0.0000 Others, 0.0000 Tax_Per, 0.00 Tax_Amount, 0.0000 Freight1, 0.0000 Others1, 0.00 Bill_Gross, 0.0000 Bill_NRate, Loss_Perc,  0.000 Qty_Deb, 0.00 Rate_Deb, Sum(Acc_Qty) Acc_Qty, Cast(Avg(Total_Rate) as Numeric(22,4)) Total_Rate, Cast(Avg(Total_Rate) as Numeric(22,4)) Total_Rate1  From Socks_Trims_Grn_Invoicing_Pending_Freight (" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Where Grn_MasterID in (" + OCN_List + ")  and GrnNo in (" + Grn_List + ") and Mode in (" + Mode_List + ") Group by Item_ID, Item, Color_ID, Color, Size_ID, Size, Rate, Loss_Perc", ref Dt);
                        }
                    }
                }
                else
                {
                    Grid.DataSource = MyBase.Load_Data("Select S1.Slno Sl, S1.Item_ID, I1.Item, S1.Color_ID, C1.Color, S1.Size_ID, S2.Size, S1.GRN_Qty, S1.Rate, S1.PO_Amount, S1.Budget_NRate Budget_Rate, S1.Budget_Tax, S1.Budget_Freight, S1.Budget_Others, S1.Bill_Qty, S1.Bill_Rate, S1.Bill_Amount, S1.Freight_BTax Freight, S1.OCharges_BTax Others, S1.Tax_Per, S1.Tax_Amount, S1.Freight_ATax Freight1, S1.OCharges_ATax Others1, S1.Bill_Gross, S1.Bill_NRate, S1.Loss_Perc, S1.Qty_Deb, S1.Rate_Deb, S1.Acc_Qty, S1.Total_Rate, S1.Total_Bud_Cost Total_Rate1  From Socks_GRN_Invoicing_Details S1 Left Join Item I1 On S1.Item_ID = I1.itemid Left Join Color C1 On S1.Color_ID = C1.colorid Left Join Size S2 On S1.Size_ID = S2.Sizeid Where S1.Master_ID = " + Code + "  Order by S1.SLno", ref Dt);
                }

                MyBase.Grid_Designing(ref Grid, ref Dt, "Item_ID", "Color_ID", "Size_ID", "Total_Rate1", "Bill_Amount");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Bill_Qty", "Bill_Rate", "Freight", "Others", "Freight1", "Others1", "Tax_Per");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 40, 140, 110, 90, 90, 90, 100, 90, 90, 100, 80, 100, 80, 80, 80, 80, 80, 80, 80, 80, 80, 100, 100);

                if (CmbMode.Text == "YARN")
                {
                    Grid.Columns["Rate"].DefaultCellStyle.Format = "0.0000";
                    Grid.Columns["Bill_Rate"].DefaultCellStyle.Format = "0.0000";
                }
                else
                {
                    Grid.Columns["Rate"].DefaultCellStyle.Format = "0.0000";
                    Grid.Columns["Bill_Rate"].DefaultCellStyle.Format = "0.0000";
                }
                Grid.Columns["Total_Rate"].DefaultCellStyle.Format = "0.0000";
                Grid.Columns["Budget_Rate"].DefaultCellStyle.Format = "0.0000";
                Grid.Columns["Budget_Freight"].DefaultCellStyle.Format = "0.0000";
                Grid.Columns["Budget_Tax"].DefaultCellStyle.Format = "0.0000";
                Grid.Columns["Budget_Others"].DefaultCellStyle.Format = "0.0000"; 

                Grid.Columns["Freight"].DefaultCellStyle.Format = "0.0000"; Grid.Columns["Others"].DefaultCellStyle.Format = "0.0000"; Grid.Columns["Tax_Per"].DefaultCellStyle.Format = "0.0000";
                Grid.Columns["Freight1"].DefaultCellStyle.Format = "0.0000"; Grid.Columns["Others1"].DefaultCellStyle.Format = "0.0000"; Grid.Columns["Bill_Gross"].DefaultCellStyle.Format = "0.00"; Grid.Columns["Bill_NRate"].DefaultCellStyle.Format = "0.0000";


                Grid.Columns["Budget_Rate"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                Grid.Columns["Budget_Rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid.Columns["Budget_Freight"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                Grid.Columns["Budget_Freight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid.Columns["Budget_Tax"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                Grid.Columns["Budget_Tax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid.Columns["Budget_Others"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                Grid.Columns["Budget_Others"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid.Columns["Budget_Rate"].HeaderText = "Rate";
                Grid.Columns["Budget_Freight"].HeaderText = "Freight";
                Grid.Columns["Budget_Tax"].HeaderText = "Tax";
                Grid.Columns["Budget_Others"].HeaderText = "Others";
                
                Grid.Columns["Freight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; Grid.Columns["Others"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Tax_Per"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; Grid.Columns["Freight1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Others1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; Grid.Columns["Bill_Gross"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; Grid.Columns["Bill_NRate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


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

        void Bill_Rate_Calc(int CurRow)
        {
            Double Gross = 0;
            Double NetDebit = 0;
            Double BreakupDebit = 0;

            Double Freight = 0;
            Double Others = 0;
            
            Double Tax_Amount_Per_Qty = 0;
            Double Freight_Per_Qty = 0;
            Double Others_Per_Qty = 0;

            try
            {
                int Start_Row = 0;
                int End_Row = Grid.Rows.Count - 1;

                if (CurRow >= 0)
                {
                    Start_Row = CurRow; End_Row = CurRow + 1;
                }
                else
                {
                    Start_Row = 0; End_Row = Grid.Rows.Count;
                }


                for (int i =Start_Row;i<=End_Row - 1;i++)
                {

                    if (Grid["Freight", i].Value == null || Grid["Freight", i].Value.ToString() == String.Empty) Grid["Freight", i].Value = 0;
                    if (Grid["Others", i].Value == null || Grid["Others", i].Value.ToString() == String.Empty) Grid["Others", i].Value = 0;
                    if (Grid["Tax_Per", i].Value == null || Grid["Tax_Per", i].Value.ToString() == String.Empty) Grid["Tax_Per", i].Value = 0;
                    if (Grid["Tax_Amount", i].Value == null || Grid["Tax_Amount", i].Value.ToString() == String.Empty) Grid["Tax_Amount", i].Value = 0;
                    if (Grid["Freight1", i].Value == null || Grid["Freight1", i].Value.ToString() == String.Empty) Grid["Freight1", i].Value = 0;
                    if (Grid["Others1", i].Value == null || Grid["Others1", i].Value.ToString() == String.Empty) Grid["Others1", i].Value = 0;


                    Gross = Convert.ToDouble(Grid["BIll_Qty", i].Value) * (Convert.ToDouble(Grid["Bill_Rate", i].Value) + Convert.ToDouble(Grid["Freight", i].Value) + Convert.ToDouble(Grid["Others", i].Value));
                    Grid["Tax_Amount", i].Value = String.Format ("{0:0.0000}", ((Gross * Convert.ToDouble(Grid["Tax_Per", i].Value) / 100)));

                    if (Convert.ToDouble(Grid["Freight", i].Value) > 0)
                    {
                        Grid["Freight1", i].Value = "0.0000";
                        Grid["Others1", i].Value = "0.0000";
                    }

                    //Grid["Bill_Gross", i].Value = String.Format ("{0:0.00}", (Gross + Convert.ToDouble(Grid["Tax_Amount", i].Value) + Convert.ToDouble(Grid["Freight1", i].Value) + Convert.ToDouble(Grid["Others1", i].Value)));
                    Gross += Convert.ToDouble(Grid["Tax_Amount", i].Value);
                    Gross += (Convert.ToDouble(Grid["Freight1", i].Value) + Convert.ToDouble(Grid["Others1", i].Value)) * Convert.ToDouble(Grid["BIll_Qty", i].Value);
                    Grid["Bill_Gross", i].Value = String.Format("{0:0.00}", Gross);
                    Grid["Bill_NRate", i].Value = Convert.ToDouble(Grid["Bill_Gross", i].Value) / Convert.ToDouble(Grid["Bill_Qty", i].Value);

                    //if (Convert.ToDouble(Grid["Bill_Rate", i].Value) > Convert.ToDouble(Grid["Budget_Rate", i].Value) || Convert.ToDouble(Grid["Bill_Rate", i].Value) > Convert.ToDouble(Grid["Budget_Rate", i].Value))
                    //{
                        //Grid["Rate_Deb", i].Value = Convert.ToDouble(Grid["Grn_Qty", i].Value) * (Convert.ToDouble(Grid["Bill_NRate", i].Value) - Convert.ToDouble(Grid["Budget_Rate", i].Value));
                        NetDebit = Convert.ToDouble(Grid["Grn_Qty", i].Value) * (Convert.ToDouble(String.Format("{0:0.0000}", Convert.ToDouble(Grid["Bill_NRate", i].Value))) - (Convert.ToDouble(Grid["Budget_Rate", i].Value) + Convert.ToDouble(Grid["Budget_Freight", i].Value) + Convert.ToDouble(Grid["Budget_Tax", i].Value) + Convert.ToDouble(Grid["Budget_Others", i].Value)));


                        // Breakup Debit
                        BreakupDebit = 0;
                        
                        if (Convert.ToDouble(Grid["Bill_Rate", i].Value) > Convert.ToDouble(Grid["Budget_Rate", i].Value))
                        {
                            //change to bill qty on 04-10-18 livi
                            BreakupDebit = (Convert.ToDouble(Grid["Bill_Rate", i].Value) - Convert.ToDouble(Grid["Budget_Rate", i].Value)) * Convert.ToDouble(Grid["Bill_Qty", i].Value);
                        }

                        #region Tax_Amount_Per_Qty
                        //change to bill qty on 04-10-18  livi
                        Tax_Amount_Per_Qty = Convert.ToDouble(String.Format ("{0:0.0000}", Convert.ToDouble(Grid["Tax_Amount", i].Value) / Convert.ToDouble(Grid["Bill_Qty", i].Value)));

                        #endregion

                        if (Tax_Amount_Per_Qty > Convert.ToDouble(Grid["Budget_Tax", i].Value))
                        {
                            BreakupDebit += (Tax_Amount_Per_Qty - Convert.ToDouble(Grid["Budget_Tax", i].Value)) * Convert.ToDouble(Grid["Bill_Qty", i].Value);
                        }

                        #region Freight_Per_Qty

                        if (Convert.ToDouble(Grid["Freight", i].Value) > 0)
                        {
                            Freight = Convert.ToDouble(Grid["Freight", i].Value);
                        }
                        else
                        {
                            Freight = Convert.ToDouble(Grid["Freight1", i].Value);
                        }

                        //Freight_Per_Qty = Convert.ToDouble(Freight) / Convert.ToDouble(Grid["Bill_Qty", i].Value);
                        Freight_Per_Qty = Convert.ToDouble(String.Format ("{0:0.0000}", Convert.ToDouble(Freight)));

#endregion

                        if (Freight_Per_Qty > Convert.ToDouble(Grid["Budget_Freight", i].Value))
                        {
                            //change to bill qty on 04-10-18 livi
                            BreakupDebit += (Freight_Per_Qty - Convert.ToDouble(Grid["Budget_Freight", i].Value)) * Convert.ToDouble(Grid["Bill_Qty", i].Value);
                        }

                        #region Others_per_Qty
                        if (Convert.ToDouble(Grid["Others", i].Value) > 0)
                        {
                            Others = Convert.ToDouble(Grid["Others", i].Value);
                        }
                        else
                        {
                            Others = Convert.ToDouble(Grid["Others1", i].Value);
                        }

                        //Others_Per_Qty = Convert.ToDouble(Others) / Convert.ToDouble(Grid["Bill_Qty", i].Value);
                        Others_Per_Qty = Convert.ToDouble(String.Format ("{0:0.0000}", Convert.ToDouble(Others)));

                        #endregion

                        if (Others_Per_Qty > Convert.ToDouble(Grid["Budget_Others", i].Value))
                        {
                            //change to bill qty on 04-10-18
                            BreakupDebit += (Convert.ToDouble(Others_Per_Qty) - Convert.ToDouble(Grid["Budget_Others", i].Value)) * Convert.ToDouble(Grid["Bill_Qty", i].Value);
                        }


                        if (NetDebit > BreakupDebit)
                        {
                            Grid["Rate_Deb", i].Value = NetDebit;
                        }
                        else
                        {
                            Grid["Rate_Deb", i].Value = BreakupDebit;
                        }
                        if (Convert.ToDouble(Grid["GRN_QTY", i].Value) == 0)
                        {
                            Grid["RATE_DEB", i].Value = 0;
                        }

                    //}
                    //else
                    //{
                        //Grid["Rate_Deb", i].Value = "0.00";
                    //}
                }
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
                OCN_List = "";

                for (int i = 0; i <= Dt_Grn.Rows.Count - 1; i++)
                {
                    if (Grid_Grn["Status", i].Value != null && Grid_Grn["Status", i].Value != DBNull.Value && Grid_Grn["Status", i].Value.ToString().ToUpper() == "true".ToUpper())
                    {
                        if (OCN_List == String.Empty)
                        {
                            OCN_List = Grid_Grn["GRN_MasterID", i].Value.ToString();
                            Mode_List = " '" + Grid_Grn["Mode", i].Value.ToString() + "' ";
                            Grn_List = " '" + Grid_Grn["GRNNO", i].Value.ToString() + "' ";
                        }
                        else
                        {
                            OCN_List += ", '" + Grid_Grn["GRN_MasterID", i].Value.ToString() + "' ";
                            Mode_List += ", '" + Grid_Grn["Mode", i].Value.ToString() + "' ";
                            Grn_List += ", '" + Grid_Grn["GRNNO", i].Value.ToString() + "' ";
                        }
                    }
                }

                if (OCN_List == String.Empty || Mode_List == String.Empty || Grn_List == String.Empty)
                {
                    MessageBox.Show("Invalid GRN & Mode List ...!", "Gainup");
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
                Bill_Amount();
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
                    if(MyParent.UserCode == 1)
                    {
                        MyBase.Valid_Decimal(Txt_Tax, e);
                    }
                    else if (Grid_Tax["Tax_Mode", Grid_Tax.CurrentCell.RowIndex].Value.ToString() == "Y")
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

                if (Tax_Code == 0)
                {
                    Value = Convert.ToDouble(TxtBillGross.Text);
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
                for (int i = 0; i <= Grid_Tax.Rows.Count-2; i++)
                {
                    if (Grid_Tax["Tax_Mode", i].Value.ToString() == "Y")
                    {
                        DataTable Tdt1 = new DataTable();
                        MyBase.Load_Data("Select Tax1 From Accounts.Dbo.Cess_Details_FN (" + MyParent.CompCode + ") Where Tax1 >0 and Tax2 = " + Grid_Tax["Tax_Code", i].Value.ToString(), ref Tdt1);
                        if (Tdt1.Rows.Count > 0)
                        {
                            if (ChkTax.Checked == false)
                            {
                                //Grid_Tax["Tax_Amount", i].Value = String.Format("{0:0}", Convert.ToDouble(Grid_Tax["Tax_Per", i].Value) * ((Get_Conditional_Tax(Convert.ToInt32(Tdt1.Rows[0]["Tax1"])))) / 100);
                                Grid_Tax["Tax_Amount", i].Value = Convert.ToDouble(String.Format("{0:0}", ((Convert.ToDouble(TxtBillGross.Text) + Get_Conditional_Tax(Convert.ToInt32(Tdt1.Rows[0]["Tax1"]))) / 100) * Convert.ToDouble(Grid_Tax["Tax_Per", i].Value)));
                            }
                            else
                            {
                                Grid_Tax["Tax_Amount", i].Value = Convert.ToDouble(String.Format("{0:0}", ((Convert.ToDouble(TxtBillGross.Text) + 0) / 100) * Convert.ToDouble(Grid_Tax["Tax_Per", i].Value)));
                            }
                        }
                        else
                        {
                            Grid_Tax["Tax_Amount", i].Value = Convert.ToDouble(String.Format("{0:0}", ((Convert.ToDouble(TxtBillGross.Text) + 0) / 100) * Convert.ToDouble(Grid_Tax["Tax_Per", i].Value)));
                            //Grid_Tax["Tax_Amount", i].Value = String.Format("{0:0}", Convert.ToDouble(Grid_Tax["Tax_Per", i].Value) * ((Convert.ToDouble(TxtBillGross.Text) + 0) / 100));
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
                    return;

                    /* Dr = Tool.Selection_Tool_Except_New("Tax_Code", this, 30, 70, ref Dt_Tax, SelectionTool_Class.ViewType.NormalView, "Select Tax", "Select Ledger_Name Tax, Ledger_Code Tax_Code From Socks_Tax_Accounts()", String.Empty, 250);
                    if (Dr != null)
                    {
                        MyBase.Row_Number(ref Grid_Tax);
                        Grid_Tax["Tax", Grid_Tax.CurrentCell.RowIndex].Value = Dr["Tax"].ToString();
                        Grid_Tax["Tax_Code", Grid_Tax.CurrentCell.RowIndex].Value = Dr["Tax_Code"].ToString();
                        Txt_Tax.Text = Dr["Tax"].ToString();

                        DataTable Tdt = new DataTable();
                        MyBase.Load_Data("Select Dbo.Socks_Get_Tax_Per (" + Dr["Tax_Code"].ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpInvoiceDate.Value) + "')", ref Tdt);
                        if (Convert.ToDouble(Tdt.Rows[0][0]) > 0)
                        {
                            Grid_Tax["Tax_Mode", Grid_Tax.CurrentCell.RowIndex].Value = "Y";
                            Grid_Tax["Tax_Per", Grid_Tax.CurrentCell.RowIndex].Value = Convert.ToDouble(Tdt.Rows[0][0]);
                            //Grid_Tax["Tax_Amount", Grid_Tax.CurrentCell.RowIndex].Value = Convert.ToDouble(String.Format("{0:0}", (Convert.ToDouble(TxtPOGross.Text) / 100) * Convert.ToDouble(Tdt.Rows[0][0])));

                            DataTable Tdt1 = new DataTable();
                            MyBase.Load_Data("Select Tax1 From Accounts.Dbo.Cess_Details_FN (" + MyParent.CompCode + ") Where Tax2 = " + Dr["Tax_Code"].ToString(), ref Tdt1);
                            if (Tdt1.Rows.Count > 0)
                            {
                                Grid_Tax["Tax_Amount", Grid_Tax.CurrentCell.RowIndex].Value = Convert.ToDouble(String.Format("{0:0}", (( Convert.ToDouble(TxtBillGross.Text)  + Get_Conditional_Tax(Convert.ToInt32(Tdt1.Rows[0]["Tax1"]))) / 100) * Convert.ToDouble(Tdt.Rows[0][0])));
                            }
                            else
                            {
                                Grid_Tax["Tax_Amount", Grid_Tax.CurrentCell.RowIndex].Value = Convert.ToDouble(String.Format("{0:0}", ((Convert.ToDouble(TxtBillGross.Text) + 0) / 100) * Convert.ToDouble(Tdt.Rows[0][0])));
                                //Grid_Tax["Tax_Amount", Grid_Tax.CurrentCell.RowIndex].Value = Convert.ToDouble(String.Format("{0:0}", ((Convert.ToDouble(TxtBillGross.Text) + Previous_Tax_Values(Grid_Tax.CurrentCell.RowIndex)) / 100) * Convert.ToDouble(Tdt.Rows[0][0])));
                            }

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
                     */
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
                    Txt.TextChanged += new EventHandler(Txt_TextChanged);
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
               // return;
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_QTY"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_RATE"].Index)
                {
                    if (Mode_List.Contains("YARN DYEING") == true)
                    {
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_QTY"].Index && (Grid["LOSS_PERC", Grid.CurrentCell.RowIndex].Value == null || Grid["LOSS_PERC", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["LOSS_PERC", Grid.CurrentCell.RowIndex].Value) == 0))
                        {
                            Grid["QTY_DEB", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_QTY"].Index && (Grid["RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) == 0))
                        {
                            Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                        else
                        {

                            if ((Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0) && Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_QTY"].Index)
                            {
                                Grid["BILL_QTY", Grid.CurrentCell.RowIndex].Value = "0";
                            }
                            if ((Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0) && Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_RATE"].Index)
                            {
                                Grid["BILL_RATE", Grid.CurrentCell.RowIndex].Value = "0";
                            }

                            if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_RATE"].Index)
                            {
                                Grid["TOTAL_RATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["TOTAL_RATE1", Grid.CurrentCell.RowIndex].Value) - (Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Txt.Text));
                            }
                            else
                            {
                                Grid["TOTAL_RATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["TOTAL_RATE1", Grid.CurrentCell.RowIndex].Value) - (Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["BILL_RATE", Grid.CurrentCell.RowIndex].Value));
                            }

                            if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_QTY"].Index)
                            {
                                if (Grid["LOSS_PERC", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                                {
                                    Grid["QTY_DEB", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Txt.Text) - Convert.ToDouble(Grid["ACC_QTY", Grid.CurrentCell.RowIndex].Value)));
                                    if (Convert.ToDouble(Grid["QTY_DEB", Grid.CurrentCell.RowIndex].Value) < 0)
                                    {
                                        Grid["QTY_DEB", Grid.CurrentCell.RowIndex].Value = 0.00;
                                    }
                                    else
                                    {
                                        Grid["QTY_DEB", Grid.CurrentCell.RowIndex].Value = Math.Round(Convert.ToDouble(Grid["QTY_DEB", Grid.CurrentCell.RowIndex].Value), 3) * Convert.ToDouble(Grid["TOTAL_RATE", Grid.CurrentCell.RowIndex].Value);
                                    }
                                }
                            }

                            if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_RATE"].Index)
                            {
                                if (Grid["RATE", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                                {
                                    Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Txt.Text) - Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value)));
                                    if (Convert.ToDouble(Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value) < 0)
                                    {
                                        Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = 0.00;
                                    }
                                    else
                                    {
                                        Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = Math.Round(Convert.ToDouble(Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value), 4) * Convert.ToDouble(Grid["BILL_QTY", Grid.CurrentCell.RowIndex].Value);
                                    }
                                }
                            }


                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Freight"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Bill_Rate"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Bill_Qty"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Others"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Tax_Per"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Freight1"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Others1"].Index)
                {
                    Bill_Rate_Calc(Grid.CurrentCell.RowIndex);
                }
                //else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_RATE"].Index)
                //{
                //    if (Mode_List.Contains("YARN DYEING") == true)
                //    {
                //        if (Grid["RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) == 0)
                //        {
                //            Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = "0";
                //        }
                //        else
                //        {
                //            if (Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0)
                //            {
                //                Grid["BILL_RATE", Grid.CurrentCell.RowIndex].Value = "0";
                //            }
                //            else
                //            {
                //                if (Grid["RATE", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                //                {
                //                    Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Txt.Text) - Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value)));
                //                    if (Convert.ToDouble(Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value) < 0)
                //                    {
                //                        Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = 0.00;
                //                    }
                //                    else
                //                    {
                //                        Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = Math.Round(Convert.ToDouble(Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value), 3) * Convert.ToDouble(Grid["BILL_QTY", Grid.CurrentCell.RowIndex].Value);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                return;
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_QTY"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_RATE"].Index)
                {
                    if (Mode_List.Contains("YARN DYEING") == true)
                    {
                        if ( Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_QTY"].Index && (Grid["LOSS_PERC", Grid.CurrentCell.RowIndex].Value == null || Grid["LOSS_PERC", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["LOSS_PERC", Grid.CurrentCell.RowIndex].Value) == 0))
                        {
                            Grid["QTY_DEB", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_QTY"].Index && (Grid["RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) == 0))
                        {
                            Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                        else
                        {

                            if ((Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0) && Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_QTY"].Index)
                            {
                                Grid["BILL_QTY", Grid.CurrentCell.RowIndex].Value = "0";
                            }
                            if ((Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0) && Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_RATE"].Index)
                            {
                                Grid["BILL_RATE", Grid.CurrentCell.RowIndex].Value = "0";
                            }

                            if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_RATE"].Index)
                            {
                                Grid["TOTAL_RATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["TOTAL_RATE1", Grid.CurrentCell.RowIndex].Value) - (Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Txt.Text));
                            }
                            else
                            {
                                Grid["TOTAL_RATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["TOTAL_RATE1", Grid.CurrentCell.RowIndex].Value) - (Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["BILL_RATE", Grid.CurrentCell.RowIndex].Value));
                            }

                            if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_QTY"].Index)
                            {
                                if (Grid["LOSS_PERC", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                                {
                                    Grid["QTY_DEB", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Txt.Text) - Convert.ToDouble(Grid["ACC_QTY", Grid.CurrentCell.RowIndex].Value)));
                                    if (Convert.ToDouble(Grid["QTY_DEB", Grid.CurrentCell.RowIndex].Value) < 0)
                                    {
                                        Grid["QTY_DEB", Grid.CurrentCell.RowIndex].Value = 0.00;
                                    }
                                    else
                                    {
                                        Grid["QTY_DEB", Grid.CurrentCell.RowIndex].Value = Math.Round(Convert.ToDouble(Grid["QTY_DEB", Grid.CurrentCell.RowIndex].Value), 3) * Convert.ToDouble(Grid["TOTAL_RATE", Grid.CurrentCell.RowIndex].Value);
                                    }
                                }
                            }

                            if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_RATE"].Index)
                            {
                                if (Grid["RATE", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                                {
                                    Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Txt.Text) - Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value)));
                                    if (Convert.ToDouble(Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value) < 0)
                                    {
                                        Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = 0.00;
                                    }
                                    else
                                    {
                                        Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = Math.Round(Convert.ToDouble(Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value), 4) * Convert.ToDouble(Grid["BILL_QTY", Grid.CurrentCell.RowIndex].Value);
                                    }
                                }
                            }


                        }
                    }
                }
                //else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BILL_RATE"].Index)
                //{
                //    if (Mode_List.Contains("YARN DYEING") == true)
                //    {
                //        if (Grid["RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) == 0)
                //        {
                //            Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = "0";
                //        }
                //        else
                //        {
                //            if (Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0)
                //            {
                //                Grid["BILL_RATE", Grid.CurrentCell.RowIndex].Value = "0";
                //            }
                //            else
                //            {
                //                if (Grid["RATE", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                //                {
                //                    Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Txt.Text) - Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value)));
                //                    if (Convert.ToDouble(Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value) < 0)
                //                    {
                //                        Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = 0.00;
                //                    }
                //                    else
                //                    {
                //                        Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value = Math.Round(Convert.ToDouble(Grid["RATE_DEB", Grid.CurrentCell.RowIndex].Value), 3) * Convert.ToDouble(Grid["BILL_QTY", Grid.CurrentCell.RowIndex].Value);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                               
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Bill_Qty"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Bill_Rate"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Freight"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Others"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Tax_Per"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Freight1"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Others1"].Index)
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
                            Grid["Bill_Rate", Grid.CurrentCell.RowIndex].Value = "0.0000";
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

                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Others1"].Index)
                    {
                        if (Grid.CurrentCell.RowIndex == Grid.Rows.Count - 1)
                        {
                            Bill_Rate_Calc(-1);
                            Bill_Amount();
                            //Grid_Tax.CurrentCell = Grid_Tax["Tax", 0];
                            //Grid_Tax.Focus();
                            //Grid_Tax.BeginEdit(true);
                            TxtRemarks.Focus();
                            return;
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
                //for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                //{
                //    // Rate Debit Generation
                //    //if (Convert.ToDouble(Grid["Bill_Qty", i].Value) >= Convert.ToDouble(Grid["Qty", i].Value))
                //    //{
                //        Min_Qty = Convert.ToDouble(Grid["GRN_Qty", i].Value);
                //    //}
                //    //else
                //    //{
                //        //Min_Qty = Convert.ToDouble(Grid["Bill_Qty", i].Value);
                //    //}
                //    if (Convert.ToDouble(Grid["Bill_Rate", i].Value) > Convert.ToDouble(Grid["Rate", i].Value))
                //    {
                //        Rate_Debit += (Min_Qty * Convert.ToDouble(Grid["Bill_Rate", i].Value)) - (Min_Qty * Convert.ToDouble(Grid["Rate", i].Value));
                //    }

                //    // Qty Debit Generation
                //    //if (Convert.ToDouble(Grid["Bill_Rate", i].Value) >= Convert.ToDouble(Grid["Rate", i].Value))
                //    //{
                //        Min_Rate = Convert.ToDouble(Grid["Rate", i].Value);
                //    //}
                //    //else
                //    //{
                //        //Min_Rate = Convert.ToDouble(Grid["Bill_Rate", i].Value);
                //    //}
                //    if (Convert.ToDouble(Grid["Bill_Qty", i].Value) > Convert.ToDouble(Grid["GRN_Qty", i].Value))
                //    {
                //        Qty_Debit += (Convert.ToDouble(Grid["Bill_Qty", i].Value) * Min_Rate) - (Convert.ToDouble(Grid["GRN_Qty", i].Value) * Min_Rate);
                //    }
                //}


                //TxtRateDifference.Text = String.Format ("{0:n}", Convert.ToDouble(String.Format("{0:0}", Rate_Debit)));
                //TxtQtyDifference.Text = String.Format ("{0:n}", Convert.ToDouble(String.Format("{0:0}", Qty_Debit)));

                Bill_Rate_Calc( - 1);

                TxtRateDifference.Text = String.Format("{0:n}",  MyBase.Sum(ref Grid, "RATE_DEB", "ITEM"));
                TxtQtyDifference.Text = String.Format("{0:n}", MyBase.Sum(ref Grid, "QTY_DEB", "ITEM"));
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

                        if (TxtRO.Text.ToString() == String.Empty)
                        {
                            TxtRO.Text = "0";
                        }
                        if (Convert.ToDouble(TxtRO.Text.ToString()) != -1 && Convert.ToDouble(TxtRO.Text.ToString()) != 1 && Convert.ToDouble(TxtRO.Text.ToString()) != 0)
                        {
                            MessageBox.Show("Invalid RO Amount (-1 & 1 Only Allowed) ...!", "Gainup");                            
                            TxtRO.Focus();
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

                        Grid["Bill_Amount", i].Value = String.Format("{0:0.00}", Convert.ToDouble(Grid["Bill_Qty", i].Value) * Convert.ToDouble(Grid["Bill_Rate", i].Value));
                        Grid["TOTAL_RATE", i].Value = Convert.ToDouble(Grid["TOTAL_RATE1", i].Value) - (Convert.ToDouble(Grid["RATE", i].Value) - Convert.ToDouble(Grid["BILL_RATE", i].Value));
                        Grid["QTY_DEB", i].Value = ((Convert.ToDouble(Grid["BILL_QTY", i].Value) - Convert.ToDouble(Grid["ACC_QTY", i].Value)));
                        if (Convert.ToDouble(Grid["QTY_DEB", i].Value) < 0)
                        {
                            Grid["QTY_DEB", i].Value = 0.00;
                        }
                        else
                        {
                            //Grid["QTY_DEB", i].Value = Math.Round(Convert.ToDouble(Grid["QTY_DEB", i].Value), 3) * Convert.ToDouble(Grid["TOTAL_RATE", i].Value);
                            //bill rate change on 04-10-18 livi
                            Grid["QTY_DEB", i].Value = String.Format ("{0:0.00}", (Convert.ToDouble(Grid["QTY_DEB", i].Value) * Convert.ToDouble(Grid["BILL_RATe", i].Value)));
                        }
                        if (Convert.ToDouble(Grid["TAX_PER", i].Value) > 0 && Convert.ToDouble(Grid["QTY_DEB", i].Value) >0)
                        {
                            Grid["QTY_DEB", i].Value = String.Format("{0:0.00}", (Convert.ToDouble(Grid["QTY_DEB", i].Value) + (((((Convert.ToDouble(Grid["QTY_DEB", i].Value)))) * Convert.ToDouble(Grid["TAX_PER", i].Value)) / 100)));
                        }

                        Grid["RATE_DEB", i].Value = ((Convert.ToDouble(Grid["BILL_RATE", i].Value) - Convert.ToDouble(Grid["RATE", i].Value)));
                        if (Convert.ToDouble(Grid["RATE_DEB", i].Value) < 0)
                        {
                            Grid["RATE_DEB", i].Value = 0.00;
                        }
                        else
                        {
                            if (Convert.ToDouble(Grid["GRN_QTY", i].Value) == 0)
                            {
                                Grid["RATE_DEB", i].Value = 0;
                            }
                            else
                            {
                                Grid["RATE_DEB", i].Value = Math.Round(Convert.ToDouble(Grid["RATE_DEB", i].Value), 4) * Convert.ToDouble(Grid["BILL_QTY", i].Value);
                            }
                        }

                    }

                    MyBase.Row_Number(ref Grid);
                    
                TxtPOGross.Text = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "PO_Amount", "GRN_Qty", "Rate", "Item")));
                    if (Grid_Tax.Rows.Count > 1)
                    {
                        TxtBillGross.Text = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "Bill_Amount", "GRN_Qty", "Rate", "Item")));
                    }
                    else 
                    {
                        TxtBillGross.Text = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "Bill_Gross", "GRN_Qty", "Rate", "Item")));
                    }
                   // if (MyParent.View != true)
                  //  {
                        Refresh_Tax();
                  //  }
                    TxtBillTax.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(String.Format("{0:0}", MyBase.Sum(ref Grid_Tax, "Tax_Amount", "Tax_Code", "Tax"))))));
                    TxtPOTax.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0}", (Convert.ToDouble(TxtBillTax.Text) / Convert.ToDouble(TxtBillGross.Text)) * Convert.ToDouble(TxtPOGross.Text))));

                    TxtPONet.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(TxtPOGross.Text) + Convert.ToDouble(TxtPOTax.Text))));
                    TxtBillNet.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(TxtBillGross.Text) + Convert.ToDouble(TxtBillTax.Text) + Convert.ToDouble(TxtRO.Text))));

                    Debit_Amount();
                    TxtToBePaid.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(TxtBillNet.Text) - (Convert.ToDouble(TxtRateDifference.Text) + Convert.ToDouble(TxtQtyDifference.Text)))));
                
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


        private void Grid_Tax_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref Grid_Tax, ref Dt_Tax, Grid_Tax.CurrentCell.RowIndex);
                Bill_Amount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}