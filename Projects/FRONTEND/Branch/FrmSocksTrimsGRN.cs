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
    public partial class FrmSocksTrimsGRN : Form, Entry
    {
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        Control_Modules MyBase = new Control_Modules();
        DataTable Dt = new DataTable();
        DataTable Dt_Tax = new DataTable();
        DataTable[,,] Dt_OCN_New;
        Int64 Code = 0;
        DataRow Dr;
        TextBox Txt = null;
        TextBox Txt_Lot = null;
        TextBox Txt_OCN = null;
        TextBox Txt_Tax = null;
        Int32 Max_Val= 120;
        Int32 Excess_Limit = 16;

        public FrmSocksTrimsGRN()
        {
            InitializeComponent();
        }

        private void FrmSocksYarnGRN_Load(object sender, EventArgs e)
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

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                TxtSupplier.Enabled = true;
                GRN_Generate();
                Load_Item();
                Load_Tax();
                Calculate_Item_Amount();
                Dt_OCN_New = new DataTable[30, Max_Val, 2];
                TxtSupplier.Focus();
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
                Dt_OCN_New = new DataTable[30, Max_Val, 2];

                Code = Convert.ToInt64(Dr["Code"]);
                TxtGRNNo.Text = Dr["GrnNO"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["GrnDate"]);
                TxtSupplier.Enabled = false;
                TxtSupplier.Text = Dr["Supplier"].ToString();
                TxtSupplier.Tag = Dr["Supplier_Code"].ToString();
                TxtGatePass.Text = Dr["GP_No"].ToString();
                DtpGPDate.Value = Convert.ToDateTime(Dr["GP_Date"]);
                if (Dr["Invoice_No"] == DBNull.Value)
                {
                    TxtDCNo.Text = Dr["DC_No"].ToString();
                    DtpDCDate.Value = Convert.ToDateTime(Dr["DC_Date"]);
                    TxtInvoiceNo.Text = "";
                    DtpInvoiceDate.Value = MyBase.GetServerDate();
                }
                else
                {
                    TxtDCNo.Text = "";
                    DtpDCDate.Value = MyBase.GetServerDate();
                    TxtInvoiceNo.Text = Dr["Invoice_No"].ToString();
                    DtpInvoiceDate.Value = Convert.ToDateTime(Dr["Invoice_Date"]);
                }

                Load_Item();
                Load_Tax();
                Load_OCN_Lot_EDIT();
                Calculate_Item_Amount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Load_OCN_Lot_EDIT()
        {
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Dt_OCN_New[i, 0, 0] = new DataTable();
                    // Commented On 01/09/2016
                    //MyBase.Load_Data("Select S1.Slno SL, (S2.Order_No + '-' + S8.PONo) Description, S1.RowID OCN_RowID, S1.Order_ID, S1.PO_Detail_ID, S2.Order_No, S8.PONo, Qty PO_QTY, S1.Qty GRN_QTY, '' T From Socks_Trims_GRN_OCN_DEtails S1 left Join Socks_Order_Master S2 on S1.Order_ID = S2.RowID Inner Join Socks_Yarn_PO_Details S7 on S1.PO_Detail_ID = S7.RowID Inner Join Socks_Yarn_PO_Master S8 on S7.Master_ID = S8.RowID Where S1.Detail_ID = " + Dt.Rows[i]["Detail_ID"].ToString(), ref Dt_OCN_New[i, 0, 0]);
                    MyBase.Load_Data("Select S1.Slno SL, (S2.Order_No + '-' + S8.PONo) Description, S1.RowID OCN_RowID, S1.Order_ID, S1.PO_Detail_ID, S2.Order_No, S8.PONo, (S9.Bal_Qty + S1.Qty) PO_QTY, S1.Qty GRN_QTY, '' T From Socks_Trims_GRN_OCN_DEtails S1 left Join Socks_Order_Master S2 on S1.Order_ID = S2.RowID Inner Join Socks_Trims_PO_Details S7 on S1.PO_Detail_ID = S7.RowID Inner Join Socks_Trims_PO_Master S8 on S7.Master_ID = S8.RowID Left join Socks_Trims_GRN_Pending_OCN() S9 on S7.Item_id = S9.Item_id and S7.Color_id = S9.Color_id and S7.Size_ID = S9.Size_ID and S1.Order_ID = S9.Order_ID and S9.PO_Detail_ID = S7.RowID Where S1.Detail_ID = " + Dt.Rows[i]["Detail_ID"].ToString(), ref Dt_OCN_New[i, 0, 0]);

                    for (int j = 0; j <= Dt_OCN_New[i, 0, 0].Rows.Count - 1; j++)
                    {
                        Dt_OCN_New[i, j, 1] = new DataTable();
                        MyBase.Load_Data("Select S1.Slno SL, S1.Lot_No, S1.Bag_No, S1.Qty, S1.Location_ID, S3.Location, '' T From Socks_Trims_GRN_OCN_Lot_Details S1 Left Join Socks_Trim_Stores_Location_Master S3 on S1.Location_ID = S3.rowID Where S1.OCN_RowID = " + Dt_OCN_New [i, 0, 0].Rows[j]["OCN_RowID"].ToString() , ref Dt_OCN_New[i, j, 1]);
                    }
                }
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select GRN - Edit", "Select S1.GRNNo, S1.GRNDate, L1.Ledger_Name Supplier, S1.Net_Amount, I1.Item, C1.Color, S3.size, S2.GRN_Qty, S2.Rate, S1.RowID Code, S1.GP_NO, S1.GP_Date, S1.DC_No, S1.DC_Date, S1.Invoice_No, S1.Invoice_Date, S1.Supplier_Code From Socks_Trims_Grn_Master S1 Inner join Socks_Trims_GRN_Details S2 on S1.RowID = S2.Master_ID Inner join item I1 on S2.Item_ID = I1.itemid inner join size S3 on S2.Size_ID = S3.sizeid inner join color C1 on S2.Color_ID = C1.colorid Left join Dbo.Supplier_All_Fn() L1 on S1.Supplier_Code = L1.Ledger_Code LEft Join ItemStock A1 On S1.GRNNo = A1.TransNo and A1.alloted > 0 Where A1.TRansNo Is Null and  S1.RowID Not in (Select GRN_MasterID From Socks_GRN_Invoicing_Details_OCN Where Mode = 'TRIMS')", String.Empty, 120, 90, 250, 100, 120, 120, 120, 100, 100);
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

        Boolean Validate_Qty()
        {
            Double Qty = 0;
            Double OCN_Qty = 0;
            Double OCN_Qty_For_Lot = 0;
            Double LOt_Qty = 0;
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Qty = Convert.ToDouble(Grid["GRN_Qty", i].Value);

                    for (int j = 0; j <= Dt_OCN_New[i, 0, 0].Rows.Count - 1; j++)
                    {
                        OCN_Qty_For_Lot += Convert.ToDouble(Dt_OCN_New[i, 0, 0].Rows[j]["GRN_Qty"]);

                        for (int k = 0; k <= Dt_OCN_New[i, j, 1].Rows.Count - 1; k++)
                        {
                            LOt_Qty += Convert.ToDouble(Dt_OCN_New[i, j, 1].Rows[k]["Qty"]);
                        }

                        if (OCN_Qty_For_Lot != Math.Round(LOt_Qty, 3))
                        {
                            MessageBox.Show("Invalid Lot Qty ..!", "Gainup");
                            Grid.CurrentCell = Grid["GRN_Qty", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return false;
                        }
                        else
                        {
                            OCN_Qty += OCN_Qty_For_Lot;
                            LOt_Qty = 0;
                            OCN_Qty_For_Lot = 0;
                        }

                    }

                    if (Qty == Math.Round(OCN_Qty,3))
                    {
                        OCN_Qty = 0; 
                    }
                    else
                    {
                        MessageBox.Show("Invalid OCN Qty ..!", "Gainup");
                        Grid.CurrentCell = Grid["GRN_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
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

        void GRN_Generate()
        {
            try
            {
                if(MyParent._New)
                {
                    DataTable Tdt = new DataTable();
                    MyBase.Load_Data("Select DBo.Get_Max_Socks_Trims_GRN ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "')", ref Tdt);
                    TxtGRNNo.Text = Tdt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                TxtGRNNo.Text = String.Empty;
                throw ex;
            }
        }

        public void Entry_Save()
        {
            String[] Queries;
            Int32 Array_Index = 0;
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
                GRN_Generate();

                DataTable Tdts = new DataTable();
                MyBase.Load_Data("Select No, Getdate() Date FRom Socks_Grn_Gatepass_UnLock Where No = '" + TxtSupplier.Tag.ToString() + "' and Mode = 'Internal' ", ref Tdts);
                if(Tdts.Rows.Count > 0)
                {
                    TxtGatePass.Text = TxtGRNNo.Text.ToString().Substring(7,5);
                    DtpGPDate.Value = Convert.ToDateTime(Tdts.Rows[0]["Date"].ToString());
                    TxtInvoiceNo.Text = TxtGRNNo.Text;
                    TxtDCNo.Text = TxtGRNNo.Text;       
                }                
                    if (!MyBase.Validate_Date_For_Entry(DtpGPDate.Value, 1, DtpDate.Value) && MyParent._New)
                    {
                        DataTable Tdtgp = new DataTable();
                        MyBase.Load_Data("Select No, Date FRom Socks_Grn_Gatepass_UnLock Where No = '" + TxtGatePass.Text.ToString() + "' and Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpGPDate.Value) + "' and Mode = 'DateLock'", ref Tdtgp);
                        if(Tdtgp.Rows.Count == 0)
                        {
                            MessageBox.Show("Min Date Locked for this Gate Pass ...!", "Gainup");
                            MyParent.Save_Error = true;
                            TxtGatePass.Focus();
                            return;
                        }
                    }


                //if (!MyBase.Validate_Date_For_Entry(DtpGPDate.Value, 1, DtpDate.Value))
                //{
                //    MessageBox.Show("Min Date Locked for this Gate Pass ...!", "Gainup");
                //    MyParent.Save_Error = true;
                //    TxtGatePass.Focus();
                //    return;
                //}

                if (TxtInvoiceNo.Text.Trim() == String.Empty && TxtDCNo.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Invoice/DC No ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtInvoiceNo.Focus();
                    return;
                }

                Calculate_Item_Amount();

                if (Txt_Gross.Text.Trim() == String.Empty || Convert.ToDouble(Txt_Gross.Text) == 0)
                {
                    MessageBox.Show("Invalid Items to Save ...!", "Gainup");
                    MyParent.Save_Error = true;
                    Grid.CurrentCell = Grid["Item", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    return;
                }

                if (Txt_Qty.Text.Trim() == String.Empty || Convert.ToDouble(Txt_Qty.Text) == 0)
                {
                    MessageBox.Show("Invalid Items to Save ...!", "Gainup");
                    MyParent.Save_Error = true;
                    Grid.CurrentCell = Grid["Item", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true); 
                    return;
                }

                if (!Validate_Qty())
                {
                     MessageBox.Show("Invalid Qty Breakups...!", "Gainup");                    
                    MyParent.Save_Error = true;
                    return;
                }

                for (int i = 0; i < Grid.Rows.Count - 1; i++)
                {
                    if ((Convert.ToDouble(Grid["Grn_Qty", i].Value)) > (Convert.ToDouble(Grid["Limit", i].Value)))
                    {
                        MessageBox.Show("GRN Qty Crossed Excess Limit [" + (Convert.ToDouble(Grid["Limit", i].Value)) + "] ...!", "Gainup");
                        Grid["GRN_Qty", i].Value = Grid["Bal_Qty", i].Value;
                        Grid.CurrentCell = Grid["GRN_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }

                Queries = new string[400];
                Array_Index = 0;

                int Slno = 1;
                Int64 Master_ID = 0;
                Int64 Detail_ID = 0;
                Int64 OCN_ID = 0;
                Int64 PoDtl_ID = 0;

                GRN_Generate();

                MyBase.SqlCn_Open();
                MyBase.SQLTrans = MyBase.SqlCn.BeginTransaction();
                MyBase.SQLCmd = new System.Data.SqlClient.SqlCommand();
                MyBase.SQLCmd.Transaction = MyBase.SQLTrans;
                MyBase.SQLCmd.Connection = MyBase.SqlCn;

                if (MyParent._New)
                {
                    if (TxtInvoiceNo.Text.Trim() == String.Empty)
                    {
                        MyBase.SQLCmd.CommandText = "Insert Into Socks_Trims_Grn_Master (GRNNo, GRNDate, Supplier_Code, Gross_Amount, Tax_Amount, Qty, Net_Amount, DC_No, DC_Date, GP_No, GP_Date) Values ('" + TxtGRNNo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtSupplier.Tag.ToString() + ", " + Convert.ToDouble(Txt_Gross.Text) + ", " + Convert.ToDouble(Txt_Tax_Amount.Text) + ", " + Convert.ToDouble(Txt_Qty.Text) + ", " + Convert.ToDouble(Txt_NetAmount.Text) + ", '" + TxtDCNo.Text.Trim() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDCDate.Value) + "', '" + TxtGatePass.Text.Trim() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpGPDate.Value) + "'); Select Scope_Identity()";
                    }
                    else
                    {
                        MyBase.SQLCmd.CommandText = "Insert Into Socks_Trims_Grn_Master (GRNNo, GRNDate, Supplier_Code, Gross_Amount, Tax_Amount, Qty, Net_Amount, Invoice_No, Invoice_Date, GP_No, GP_Date) Values ('" + TxtGRNNo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtSupplier.Tag.ToString() + ", " + Convert.ToDouble(Txt_Gross.Text) + ", " + Convert.ToDouble(Txt_Tax_Amount.Text) + ", " + Convert.ToDouble(Txt_Qty.Text) + ", " + Convert.ToDouble(Txt_NetAmount.Text) + ", '" + TxtInvoiceNo.Text.Trim() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpInvoiceDate.Value) + "', '" + TxtGatePass.Text.Trim() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpGPDate.Value) + "'); Select Scope_Identity()";
                    }
                    Code = Convert.ToInt64(MyBase.SQLCmd.ExecuteScalar());
                    Master_ID = Code;

                    MyBase.SQLCmd.CommandText = MyParent.EntryLog("SOCKS TRIMS GRN", "ADD", Code.ToString());
                    MyBase.SQLCmd.ExecuteNonQuery();
                }
                else
                {
                    Master_ID = Code;
                    if (TxtInvoiceNo.Text.Trim() == String.Empty)
                    {
                        MyBase.SQLCmd.CommandText = "Update Socks_Trims_Grn_Master Set Invoice_No = null, Invoice_Date = null, GP_No = '" + TxtGatePass.Text.Trim() + "', GP_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpGPDate.Value) + "', DC_No = '" + TxtDCNo.Text.Trim() + "', DC_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDCDate.Value) + "', Gross_Amount = " + Convert.ToDouble(Txt_Gross.Text) + ", Tax_Amount = " + Convert.ToDouble(Txt_Tax_Amount.Text) + ", Qty = " + Convert.ToDouble(Txt_Qty.Text) + ", Net_Amount = " + Convert.ToDouble(Txt_NetAmount.Text) + " Where RowID = " + Code;
                    }
                    else
                    {
                        MyBase.SQLCmd.CommandText = "Update Socks_Trims_Grn_Master Set DC_No = null, DC_Date = null, GP_No = '" + TxtGatePass.Text.Trim() + "', GP_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpGPDate.Value) + "', Invoice_No = '" + TxtInvoiceNo.Text.Trim() + "', Invoice_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpInvoiceDate.Value) + "', Gross_Amount = " + Convert.ToDouble(Txt_Gross.Text) + ", Tax_Amount = " + Convert.ToDouble(Txt_Tax_Amount.Text) + ", Qty = " + Convert.ToDouble(Txt_Qty.Text) + ", Net_Amount = " + Convert.ToDouble(Txt_NetAmount.Text) + " Where RowID = " + Code;
                    }
                    MyBase.SQLCmd.ExecuteNonQuery();
                    MyBase.SQLCmd.CommandText = "Delete From Socks_Trims_GRN_Tax_Details Where Master_ID = " + Code;
                    MyBase.SQLCmd.ExecuteNonQuery();
                    //MyBase.SQLCmd.CommandText = "update S3 Set S3.GRN_Qty = S3.GRN_Qty - S1.Qty From Socks_Trims_GRN_OCN_DEtails S1 Inner Join Socks_Trims_GRN_Details S2 on S1.Detail_ID = S2.RowID Inner Join Socks_Trims_BOM_Status S3 on S1.Order_ID = S3.Order_ID And S2.Item_ID = S3.Item_ID And S2.Color_ID = S3.Color_ID And S2.Size_ID = S3.Size_ID Where S2.Master_ID = " + Code;
                    //MyBase.SQLCmd.ExecuteNonQuery();
                    MyBase.SQLCmd.CommandText = "Delete From Socks_Trims_GRN_OCN_Lot_Details Where Master_ID = " + Code;
                    MyBase.SQLCmd.ExecuteNonQuery();
                    MyBase.SQLCmd.CommandText = "Delete From Socks_Trims_GRN_OCN_DEtails Where Master_ID = " + Code;
                    MyBase.SQLCmd.ExecuteNonQuery();
                    MyBase.SQLCmd.CommandText = "Delete From Socks_Trims_GRN_Details Where Master_ID = " + Code;
                    MyBase.SQLCmd.ExecuteNonQuery();
                    MyBase.SQLCmd.CommandText = MyParent.EntryLog("SOCKS TRIMS GRN", "EDIT", Code.ToString());
                    MyBase.SQLCmd.ExecuteNonQuery();
                }


                for (int i=0;i<=Dt.Rows.Count - 1;i++)
                {
                    MyBase.SQLCmd.CommandText = "Insert Into Socks_Trims_GRN_Details (Master_ID, SlNo, Item_ID, Color_ID, Size_ID, Grn_Qty, Rate, Amount) Values (" + Code + ", " + (i + 1) + ", " + Grid["Item_ID", i].Value.ToString() + ", " + Grid["Color_ID", i].Value.ToString() + ", " + Grid["Size_ID", i].Value.ToString() + ", " + Grid["GRN_Qty", i].Value.ToString() + ", " + Math.Round(Convert.ToDouble(Grid["Rate", i].Value.ToString()),4) + ", " + Grid["Amount", i].Value.ToString() + "); Select Scope_Identity()";
                    Detail_ID = Convert.ToInt64(MyBase.SQLCmd.ExecuteScalar());

                    Slno = 0;
                    for (int j=0;j<= Dt_OCN_New[i, 0, 0].Rows.Count - 1;j++)
                    {
                        if(Dt_OCN_New[i, 0, 0].Rows[j]["Order_ID"].ToString() == "148")
                        {
                            MyBase.SQLCmd.CommandText = " Exec  Socks_Trims_FreeOcn_Item_Proc " + Grid["Item_ID", i].Value.ToString() + ", " + Grid["Color_ID", i].Value.ToString() + ", " + Grid["Size_ID", i].Value.ToString() + ", " + Dt_OCN_New[i, 0, 0].Rows[j]["GRN_Qty"].ToString() + " ";
                            MyBase.SQLCmd.ExecuteNonQuery();
                            
                            DataTable TDtPoID = new DataTable();
                            //MyBase.Load_Data("Select RowID From Socks_Yarn_PO_Details Where Master_ID = 135 and Order_ID = 148 and Item_id =  " + Grid["Item_ID", i].Value.ToString() + " and Size_ID = " + Grid["Size_ID", i].Value.ToString() + " and Color_Id = " + Grid["Color_ID", i].Value.ToString() + "", ref TDtPoID);
                            MyBase.Load_Data("Select Ident_Current('Socks_Trims_PO_Details')", ref TDtPoID);
                            PoDtl_ID  = Convert.ToInt64(TDtPoID.Rows[0][0].ToString());

                            MyBase.SQLCmd.CommandText = "Insert Into Socks_Trims_GRN_OCN_DEtails (Master_ID, Slno, PO_Detail_ID, Order_ID, Qty, Detail_ID) Values (" + Master_ID + ", " + Slno + ", " + PoDtl_ID + ", " + Dt_OCN_New[i, 0, 0].Rows[j]["Order_ID"].ToString() + ", " + Dt_OCN_New[i, 0, 0].Rows[j]["GRN_Qty"].ToString() + ", " + Detail_ID + "); Select Scope_Identity() ";
                            OCN_ID = Convert.ToInt64(MyBase.SQLCmd.ExecuteScalar());
                        }
                        else
                        {
                            MyBase.SQLCmd.CommandText = "Insert Into Socks_Trims_GRN_OCN_DEtails (Master_ID, Slno, PO_Detail_ID, Order_ID, Qty, Detail_ID) Values (" + Master_ID + ", " + Slno + ", " + Dt_OCN_New[i, 0, 0].Rows[j]["PO_Detail_ID"].ToString() + ", " + Dt_OCN_New[i, 0, 0].Rows[j]["Order_ID"].ToString() + ", " + Dt_OCN_New[i, 0, 0].Rows[j]["GRN_Qty"].ToString() + ", " + Detail_ID + "); Select Scope_Identity() ";
                            OCN_ID = Convert.ToInt64(MyBase.SQLCmd.ExecuteScalar());
                        }
                      //  MyBase.SQLCmd.CommandText = "update Socks_Trims_bom_Status Set Grn_Qty = Grn_Qty + " + Convert.ToDouble(Dt_OCN_New[i, 0, 0].Rows[j]["GRN_Qty"]) + " Where Order_ID = " + Dt_OCN_New[i, 0, 0].Rows[j]["Order_ID"].ToString() + " and Item_ID = " + Grid["Item_ID", i].Value.ToString() + " and Color_ID = " + Grid["Color_ID", i].Value.ToString() + " and Size_ID = " + Grid["Size_ID", i].Value.ToString();
                      //  MyBase.SQLCmd.ExecuteNonQuery();

                        MyBase.SQLCmd.CommandText = " Exec  Vaahini_Erp_Gainup.Dbo.Time_Action_Auto_Save_Trim_Inward_Socks_Proc  '" + Dt_OCN_New[i, 0, 0].Rows[j]["Order_No"].ToString() + "' ";
                        MyBase.SQLCmd.ExecuteNonQuery();

                        for (int k = 0; k <= Dt_OCN_New[i, j, 1].Rows.Count - 1; k++)
                        {
                            MyBase.SQLCmd.CommandText = "Insert Into Socks_Trims_GRN_OCN_Lot_Details (Master_ID, OCN_RowID, Slno, Lot_No, Bag_No, Location_ID, Qty, Supplier_Code) Values (" + Master_ID + ", " + OCN_ID + ", " + (k + 1) + ", '" + Dt_OCN_New[i, j, 1].Rows[k]["Lot_No"].ToString() + "', " + Dt_OCN_New[i, j, 1].Rows[k]["Bag_No"].ToString() + ", " + Dt_OCN_New[i, j, 1].Rows[k]["Location_ID"].ToString() + ", " + Dt_OCN_New[i, j, 1].Rows[k]["Qty"].ToString() + ", " + TxtSupplier.Tag.ToString() + ")";
                            MyBase.SQLCmd.ExecuteNonQuery();
                        }                          
                    }
                }

                for (int i = 0; i <= Dt_Tax.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        MyBase.SQLCmd.CommandText = "Insert Into Socks_Trims_GRN_Tax_Details (Master_ID, Slno, Tax_Code, Mode, Tax_Per, Tax_Amount) Values (" + Code + ", " + (i + 1) + ", " + Dt_Tax.Rows[i]["Tax_Code"].ToString() + ", '" + Dt_Tax.Rows[i]["Tax_Mode"].ToString() + "', " + Dt_Tax.Rows[i]["Tax_Per"].ToString() + ", " + Dt_Tax.Rows[i]["Tax_Amount"].ToString() + ")";
                        MyBase.SQLCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        MyBase.SQLCmd.CommandText = "Insert Into Socks_Trims_GRN_Tax_Details (Master_ID, Slno, Tax_Code, Mode, Tax_Per, Tax_Amount) Values (" + Code + ", " + (i + 1) + ", " + Dt_Tax.Rows[i]["Tax_Code"].ToString() + ", '" + Dt_Tax.Rows[i]["Tax_Mode"].ToString() + "', " + Dt_Tax.Rows[i]["Tax_Per"].ToString() + ", " + Dt_Tax.Rows[i]["Tax_Amount"].ToString() + ")";
                        MyBase.SQLCmd.ExecuteNonQuery();
                    }
                }
                
                if(MyParent.Edit)
                {
                    MyBase.SQLCmd.CommandText = " Exec Vsocks_TrimsGrn_Stock_Delete  " + Code + " ";
                    MyBase.SQLCmd.ExecuteNonQuery();
                }
                MyBase.SQLCmd.CommandText = " Exec Vsocks_TrimsGrn_Stock_Insert " + Master_ID + " ";
                MyBase.SQLCmd.ExecuteNonQuery();
                

                MyBase.SQLTrans.Commit();
                MyBase.SqlCn_Close();
                MessageBox.Show("Saved ...!", "Gainup");
                MyParent.Save_Error = false;
                MyBase.Clear(this);

            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MyBase.SQLTrans.Rollback();
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, false);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select GRN - Delete", "Select S1.GRNNo, S1.GRNDate, L1.Ledger_Name Supplier, S1.Net_Amount, I1.Item, C1.Color, S3.size, S2.GRN_Qty, S2.Rate, S1.RowID Code, S1.GP_NO, S1.GP_Date, S1.DC_No, S1.DC_Date, S1.Invoice_No, S1.Invoice_Date, S1.Supplier_Code From Socks_Trims_Grn_Master S1 Inner join Socks_Trims_GRN_Details S2 on S1.RowID = S2.Master_ID Inner join item I1 on S2.Item_ID = I1.itemid inner join size S3 on S2.Size_ID = S3.sizeid inner join color C1 on S2.Color_ID = C1.colorid Left join Dbo.Supplier_All_Fn() L1 on S1.Supplier_Code = L1.Ledger_Code LEft Join ItemStock A1 On S1.GRNNo = A1.TransNo and A1.alloted > 0 Where A1.TRansNo Is Null and  S1.RowID Not in (Select GRN_MasterID From Socks_GRN_Invoicing_Details_OCN Where Mode = 'TRIMS') ", String.Empty, 120, 90, 250, 100, 120, 120, 120, 100, 100);
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
                    MyBase.SqlCn_Open();
                    MyBase.SQLTrans = MyBase.SqlCn.BeginTransaction();
                    MyBase.SQLCmd = new System.Data.SqlClient.SqlCommand();
                    MyBase.SQLCmd.Transaction = MyBase.SQLTrans;
                    MyBase.SQLCmd.Connection = MyBase.SqlCn;

                    MyBase.SQLCmd.CommandText = "Delete From Socks_Trims_GRN_Tax_Details Where Master_ID = " + Code;
                    MyBase.SQLCmd.ExecuteNonQuery();
                  //  MyBase.SQLCmd.CommandText = "update S3 Set S3.GRN_Qty = S3.GRN_Qty - S1.Qty From Socks_Trims_GRN_OCN_DEtails S1 Inner Join Socks_Trims_GRN_Details S2 on S1.Detail_ID = S2.RowID Inner Join Socks_Trims_BOM_Status S3 on S1.Order_ID = S3.Order_ID And S2.Item_ID = S3.Item_ID And S2.Color_ID = S3.Color_ID And S2.Size_ID = S3.Size_ID Where S2.Master_ID = " + Code;
                  //  MyBase.SQLCmd.ExecuteNonQuery();
                    MyBase.SQLCmd.CommandText = "Delete From Socks_Trims_GRN_OCN_Lot_Details Where Master_ID = " + Code;
                    MyBase.SQLCmd.ExecuteNonQuery();
                    MyBase.SQLCmd.CommandText = "Delete From Socks_Trims_GRN_OCN_DEtails Where Master_ID = " + Code;
                    MyBase.SQLCmd.ExecuteNonQuery();
                    MyBase.SQLCmd.CommandText = "Delete From Socks_Trims_GRN_Details Where Master_ID = " + Code;
                    MyBase.SQLCmd.ExecuteNonQuery();
                    MyBase.SQLCmd.CommandText = "Delete From Socks_Trims_Grn_Master Where RowID = " + Code;
                    MyBase.SQLCmd.ExecuteNonQuery();
                    MyBase.SQLCmd.CommandText = " Exec Vsocks_TrimsGrn_Stock_Delete " + Code + " ";
                    MyBase.SQLCmd.ExecuteNonQuery();
                    MyBase.SQLCmd.CommandText = MyParent.EntryLog("SOCKS TRIMS GRN", "DELETE", Code.ToString());
                    MyBase.SQLCmd.ExecuteNonQuery();

                    MyBase.SQLTrans.Commit();
                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyBase.Clear(this);
                    MyParent.Load_DeleteEntry();
                }
            }
            catch (Exception ex)
            {
                 MyBase.SQLTrans.Rollback();
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                 Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select GRN - View", "Select S1.GRNNo, S1.GRNDate, L1.Ledger_Name Supplier, S1.Net_Amount, I1.Item, C1.Color, S3.size, S2.GRN_Qty, S2.Rate,  S1.GP_NO, S1.RowID Code, S1.GP_Date, S1.DC_No, S1.DC_Date, S1.Invoice_No, S1.Invoice_Date, S1.Supplier_Code From Socks_Trims_Grn_Master S1 Inner join Socks_Trims_GRN_Details S2 on S1.RowID = S2.Master_ID Inner join item I1 on S2.Item_ID = I1.itemid inner join size S3 on S2.Size_ID = S3.sizeid inner join color C1 on S2.Color_ID = C1.colorid Left join Dbo.Supplier_All_Fn() L1 on S1.Supplier_Code = L1.Ledger_Code LEft Join ItemStock A1 On S1.GRNNo = A1.TransNo and A1.alloted > 0 ", String.Empty, 120, 90, 250, 100, 120, 120, 120, 100, 100, 120);
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
                String Str, Str1, Str2, Str3, Str4;
                String Order = "";
                DataTable Dt1 = new DataTable();
                DataTable Dt2 = new DataTable();
                DataTable Dt3 = new DataTable();
                DataTable Dt4 = new DataTable();

                //Str = " Select S1.PONo, L1.Ledger_Name Supplier, S1.PoDate, S1.Required_Date, (Case When S1.PO_Method = 0 Then 'OCN-WISE' When S1.PO_Method = 0 Then 'ITEM-WISE' End) PO_Method, L1.Ledger_Address Supplier_Address, L1.Ledger_Phone Supplier_Phone, L1.Ledger_email Supplier_Email From Socks_Yarn_PO_Master S1 left Join Supplier_All_Fn() L1 On L1.LEdgeR_Code = S1.Supplier_Code   Where S1.Approval_Flag = 'T' and S1.RowID = " + Code;
                //MyBase.Load_Data(Str, ref Dt1);


                //if (Dt1.Rows.Count <= 0)
                //{
                //    MessageBox.Show("PO Not Approved...!", "Gainup");
                //    return;
                //}

                Str1 = " Select Top 10000000 B.Item+' - '+C.Color+' - '+D.Size Particulars, A.Grn_Qty, A.Rate, A.Amount From Socks_Trims_GRN_Details A Left Join Item B On A.Item_ID = B.ItemID Left Join Color C On A.Color_ID = C.ColorID Left Join Size D On A.Size_ID = D.SizeID Where A.Master_ID = " + Code + " Order By B.Item+' - '+C.Color+' - '+D.Size ";
                MyBase.Execute_Qry(Str1, "Trims_Goods_Receipt");

                Str2 = " Select Dbo.Trims_PoWise_Receipt_Qty(" + Code + ")PoDetails";
                MyBase.Load_Data(Str2, ref Dt2);

                //Str3 = " Select Distinct S3.Order_No From Socks_Yarn_PO_Master S1 Inner join Socks_Yarn_PO_Details S2 ON S1.RowID = s2.Master_ID Inner join Socks_Order_Master S3 on S2.Order_ID = S3.RowID Where S1.RowID = " + Code;
                //MyBase.Load_Data(Str3, ref Dt3);

                Str4 = " Select Getdate()PrintOutDate";
                MyBase.Load_Data(Str4, ref Dt4);

                CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RptTrimsReceivedDetails.rpt");
                MyParent.FormulaFill(ref ObjRpt, "Supplier", TxtSupplier.Text.ToString());
                MyParent.FormulaFill(ref ObjRpt, "GRNNo", TxtGRNNo.Text.ToString());
                MyParent.FormulaFill(ref ObjRpt, "Date", DtpDate.Value.ToString());
                if(TxtDCNo.Text.ToString() != String.Empty)
                {
                    MyParent.FormulaFill(ref ObjRpt, "DCNO", TxtDCNo.Text.ToString());
                    MyParent.FormulaFill(ref ObjRpt, "DCDate", DtpDCDate.Value.ToString());
                }
                else
                {
                    MyParent.FormulaFill(ref ObjRpt, "DCNO", TxtInvoiceNo.Text.ToString());
                    MyParent.FormulaFill(ref ObjRpt, "DCDate", DtpInvoiceDate.Value.ToString());
                }
                //MyParent.FormulaFill(ref ObjRpt, "LOTNO", Txt_Lot.Text.ToString());
                MyParent.FormulaFill(ref ObjRpt, "InwardNo", TxtGatePass.Text.ToString());
                MyParent.FormulaFill(ref ObjRpt, "PoDetails", Dt2.Rows[0]["PoDetails"].ToString());
                MyParent.FormulaFill(ref ObjRpt, "PrintOutDate", Dt4.Rows[0]["PrintOutDate"].ToString());

                MyParent.CReport(ref ObjRpt, "Trims Purchase Goods Receipt..!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Load_Item()
        {
            try
            {

                if (MyParent._New)
                {
                    Grid.DataSource = MyBase.Load_Data("Select 0 as SL, Cast(0 as Bigint) Detail_ID, Item + ' ' + Color + ' ' + Size + ' @ ' + Cast(Rate as Varchar (15)) Description, Item_ID, Item ITEM, Color_ID, Color COLOR, Size_ID, Size SIZE, PO_Qty, Inward_Qty, Bal_Qty, Bal_Qty GRN_Qty, Rate, Cast(0 as Numeric (25, 2)) Amount, 0.000 Limit From Socks_Trims_GRN_Pending () Where 1=2", ref Dt);
                }
                else
                {
                    Grid.DataSource = MyBase.Load_Data("Select S2.Slno SL, S2.RowID Detail_ID, S1.Item + ' ' + S1.Color + ' ' + S1.Size + ' @ ' + Cast(S1.Rate as Varchar (15)) Description, S1.Item_ID, S1.Item ITEM, S1.Color_ID, S1.Color COLOR, S1.Size_ID, S1.Size SIZE, S1.PO_Qty, (S1.Inward_Qty - S2.GRN_Qty) Inward_Qty, (S1.Bal_Qty + S2.GRN_Qty) Bal_Qty, S2.GRN_Qty, S2.Rate, (S2.Rate * S2.GRN_Qty) Amount, (S1.Bal_Qty + S2.GRN_Qty) + (Case When 15 < Cast(Nullif(((S1.Bal_Qty + S2.GRN_Qty) * 5),0) /100 as Numeric(20,3)) Then 15 Else Cast(Nullif(((S1.Bal_Qty + S2.GRN_Qty) * 5),0) /100 as Numeric(20,3)) End) Limit  From Socks_Trims_GRN_Pending () S1 Inner join Socks_Trims_GRN_Details S2 on S1.Item_id = S2.Item_ID and S1.Color_id = S2.Color_ID and S1.Size_ID = S2.Size_ID And S1.Rate = S2.Rate Inner Join Socks_Trims_GRN_Master S3 On S2.Master_ID = S3.RowID and S1.Supplier_Code = S3.Supplier_Code  Where S2.Master_ID = " + Code + " Order By S2.Slno", ref Dt);
                }
                MyBase.Grid_Designing(ref Grid, ref Dt, "Item_ID", "Color_ID", "Detail_ID", "Size_ID", "Description");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Item", "GRN_Qty");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);

                Grid.Columns["po_Qty"].HeaderText = "PO";
                Grid.Columns["inward_Qty"].HeaderText = "INWARD";
                Grid.Columns["BAL_Qty"].HeaderText = "BAL";
                Grid.Columns["GRN_Qty"].HeaderText = "GRN";                

                Grid.Columns["po_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["bal_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["grn_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["inward_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


                Grid.Columns["po_qty"].DefaultCellStyle.Format = "0.000";
                Grid.Columns["bal_qty"].DefaultCellStyle.Format = "0.000";
                Grid.Columns["grn_qty"].DefaultCellStyle.Format = "0.000";
                Grid.Columns["inward_qty"].DefaultCellStyle.Format = "0.000";
                Grid.Columns["Limit"].DefaultCellStyle.Format = "0.000";
                Grid.Columns["Rate"].DefaultCellStyle.Format = "0.0000";

                MyBase.Grid_Width(ref Grid, 40, 140, 100, 100, 90, 90, 90, 90, 90, 100,80);

                Grid.RowHeadersWidth = 10;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmSocksYarnGRN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtGatePass")
                    {
                        Grid.CurrentCell = Grid["Item", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                    }
                    else if (this.ActiveControl.Name == "Txt_NetAmount")
                    {
                        if (MyParent._New || MyParent.Edit)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
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
                        if (Grid.Rows.Count <=1 || MyParent.UserCode ==1)
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "Select Distinct L1.Ledger_Name Supplier, S1.SUpplier_Code From Socks_Trims_GRN_Pending () S1   Left Join Supplier_All_Fn() L1 On S1.Supplier_Code = L1.LEdgeR_code   Where S1.Supplier_Code != 793", String.Empty, 350);
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
                                TxtSupplier.Tag = Dr["Supplier_Code"].ToString();
                                TxtSupplier.Text = Dr["Supplier"].ToString();
                            }
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtGatePass")
                    {
                        if (!MyBase.Validate_Date_For_Entry(DtpGPDate.Value, 1, DtpDate.Value) && MyParent.Edit == true)
                            {
                                MessageBox.Show("Min Date Locked for this Gate Pass ...!", "Gainup");
                                TxtGatePass.Focus();
                                return;
                            }
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select GATE PASS", "Select GPNo, GPDate, Party, isnull(InvNo, '') InvNo, InvDate, Isnull(DCno, '') DCno, DCDate, Qty From Socks_Gate_Pass_Details_Pending ()", String.Empty, 100, 100, 150, 100, 100, 100, 100);
                        if (Dr != null)
                        {
                            TxtGatePass.Text = Dr["GPNo"].ToString();
                            MyBase.Lock_DatetimePicker (ref DtpGPDate, Convert.ToDateTime(Dr["GPDate"]));
                            
                            if (Dr["InvNo"].ToString() != String.Empty)
                            {
                                TxtInvoiceNo.Text = Dr["InvNo"].ToString();
                                TxtDCNo.Text = "";
                                TxtQty.Text = Dr["Qty"].ToString();
                                MyBase.Lock_DatetimePicker (ref DtpInvoiceDate, Convert.ToDateTime(Dr["InvDate"]));
                                MyBase.Lock_DatetimePicker (ref DtpDCDate, MyBase.GetServerDate());
                            }
                            else
                            {
                                TxtDCNo.Text = Dr["DCNo"].ToString();
                                TxtInvoiceNo.Text = "";
                                MyBase.Lock_DatetimePicker(ref DtpInvoiceDate, MyBase.GetServerDate());
                                MyBase.Lock_DatetimePicker(ref DtpDCDate, Convert.ToDateTime(Dr["DCDate"]));
                            }
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

        private void FrmSocksYarnGRN_KeyPress(object sender, KeyPressEventArgs e)
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Item"].Index)
                {
                    e.Handled = true;
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Grn_Qty"].Index)
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
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Item"].Index)
                    {
                        Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Item", "Select Item + ' ' + Color + ' ' + Size + ' @ ' + Cast(Rate as Varchar (15)) Description, PO_Qty PO, Inward_Qty Inward, Bal_Qty Bal, Bal_Qty GRN, Rate, Cast(0 as Numeric (25, 2)) Amount, Item_ID, Item ITEM, Color_ID, Color COLOR, Size_ID, Size SIZE, Bal_Qty + (Case When 15 < Cast(Nullif((Bal_Qty * 5),0) /100 as Numeric(20,3)) Then 15 Else Cast(Nullif((Bal_Qty * 5),0) /100 as Numeric(20,3)) End) Limit From Socks_Trims_GRN_Pending () Where Bal_Qty > 0 And Supplier_Code = " + TxtSupplier.Tag.ToString(), String.Empty, 250, 80, 80, 80, 80, 80);
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Item"].ToString();

                            Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                            Grid["Description", Grid.CurrentCell.RowIndex].Value = Dr["Description"].ToString();
                            Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                            Grid["Color", Grid.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                            Grid["Item_ID", Grid.CurrentCell.RowIndex].Value = Dr["Item_ID"].ToString();
                            Grid["Size_ID", Grid.CurrentCell.RowIndex].Value = Dr["Size_ID"].ToString();
                            Grid["Color_ID", Grid.CurrentCell.RowIndex].Value = Dr["Color_ID"].ToString();

                            Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["PO"]);
                            Grid["Inward_Qty", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["Inward"]);
                            Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["Bal"]);
                            Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["GRN"]);
                            Grid["Rate", Grid.CurrentCell.RowIndex].Value = String.Format ("{0:0.0000}", Convert.ToDouble(Dr["Rate"]));
                            Grid["Limit", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["Limit"]);
                            Load_OCN(Grid.CurrentCell.RowIndex);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        double Bal_Qty_Lot()
        {
            double Qty = 0;
            try
            {
                Qty = Convert.ToDouble(Grid_OCN["grn_qty", Grid_OCN.CurrentCell.RowIndex].Value);

                for (int i = 0; i <= Dt_OCN_New[Grid.CurrentCell.RowIndex, Grid_OCN.CurrentCell.RowIndex, 1].Rows.Count - 1; i++)
                {
                    Qty -= Convert.ToDouble(Dt_OCN_New[Grid.CurrentCell.RowIndex, Grid_OCN.CurrentCell.RowIndex, 1].Rows[i]["Qty"]);
                }

                return Math.Round(Qty, 3);
            }
            catch (Exception ex)
            {
                return Qty;
            }
        }

         double Bal_Qty_Lot1()
        {
            double Qty = 0;
            try
            {
                Qty = Convert.ToDouble(Grid_OCN["grn_qty", Grid_OCN.CurrentCell.RowIndex].Value);

                for (int i = 0; i <= Dt_OCN_New[Grid.CurrentCell.RowIndex, Grid_OCN.CurrentCell.RowIndex, 1].Rows.Count - 1; i++)
                {
                    Qty += Convert.ToDouble(Dt_OCN_New[Grid.CurrentCell.RowIndex, Grid_OCN.CurrentCell.RowIndex, 1].Rows[i]["Qty"]);
                }

                return Math.Round(Qty, 3);
            }
            catch (Exception ex)
            {
                return Qty;
            }
        }



        double Bal_Qty_OCN()
        {
            double Qty = 0;
            try
            {
                Qty = Convert.ToDouble(Grid["grn_qty", Grid.CurrentCell.RowIndex].Value);

                for (int i = 0; i <= Dt_OCN_New[Grid.CurrentCell.RowIndex, 0, 0].Rows.Count - 1; i++)
                {
                    Qty -= Convert.ToDouble(Dt_OCN_New[Grid.CurrentCell.RowIndex, 0, 0].Rows[i]["GRN_Qty"]);
                }

                return Math.Round (Qty, 3);
            }
            catch (Exception ex)
            {
                return Qty;
            }
        }

        void Load_Lot(Int32 Row)
        {
            try
            {
                if (Dt_OCN_New[Grid.CurrentCell.RowIndex, Row, 1] == null)
                {
                    Dt_OCN_New[Grid.CurrentCell.RowIndex, Row, 1] = new DataTable();
                    MyBase.Load_Data("Select S1.Slno SL, S1.Lot_No, S1.Bag_No, S1.Qty, S1.Location_ID, S3.Location, '' T From Socks_Trims_GRN_OCN_Lot_Details S1 Left Join Socks_Trim_Stores_Location_Master S3 on S1.Location_ID = S3.rowID Where 1 = 2", ref Dt_OCN_New[Grid.CurrentCell.RowIndex, Row, 1]);
                }

                Grid_LotNo.DataSource = Dt_OCN_New[Grid.CurrentCell.RowIndex, Row, 1];
                MyBase.Grid_Designing(ref Grid_LotNo, ref Dt_OCN_New[Grid.CurrentCell.RowIndex, Row, 1], "Location_ID", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid_LotNo, "Lot_No", "Bag_NO", "Qty", "Location");
                MyBase.Grid_Colouring(ref Grid_LotNo, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid_LotNo, 40, 100, 80, 90, 120);

                Grid_LotNo.Columns["lot_no"].HeaderText = "LOTNO";
                Grid_LotNo.Columns["BAG_no"].HeaderText = "BAGNO";
                Grid_LotNo.Columns["QTY"].HeaderText = "QTY";
                Grid_LotNo.Columns["Location"].HeaderText = "LOCATION";

                Grid_LotNo.Columns["QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid_LotNo.RowHeadersWidth = 10;

                Grid_LotNo.CurrentCell = Grid_LotNo["SL", 0];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        void Load_OCN(Int32 Row)
        {
            try
            {
                if (Dt_OCN_New[Row, 0, 0] == null)
                {
                    Dt_OCN_New[Row, 0, 0] = new DataTable();
                    MyBase.Load_Data("Select S1.Slno SL, Cast('' as Varchar (30)) Description, Cast(0 as Bigint) OCN_RowID, S1.Order_ID, S1.PO_Detail_ID, S2.Order_No, S8.PONo, Qty PO_QTY, Cast(0 as Numeric (16, 3)) GRN_QTY, '' T From Socks_Trims_GRN_OCN_DEtails S1 left Join Socks_Order_Master S2 on S1.Order_ID = S2.RowID Inner Join Socks_Yarn_PO_Details S7 on S1.PO_Detail_ID = S7.RowID Inner Join Socks_Yarn_PO_Master S8 on S7.Master_ID = S8.RowID Where 1 = 2", ref Dt_OCN_New[Row, 0, 0]);
                }

                Grid_OCN.DataSource = Dt_OCN_New[Row, 0, 0];
                MyBase.Grid_Designing(ref Grid_OCN, ref Dt_OCN_New[Row, 0, 0], "Order_ID", "OCN_RowID", "T", "PO_Detail_ID", "Description");
                MyBase.ReadOnly_Grid_Without(ref Grid_OCN, "Order_No", "GRN_QTY");
                MyBase.Grid_Colouring(ref Grid_OCN, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid_OCN, 30, 110, 110, 100, 100);

                Grid_OCN.Columns["GRN_QTY"].HeaderText = "GRN";
                Grid_OCN.Columns["po_qty"].HeaderText = "PO_BALQTY";
                Grid_OCN.Columns["PONO"].HeaderText = "PO";

                Grid_OCN.Columns["GRN_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_OCN.Columns["po_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid_OCN.RowHeadersWidth = 10;

                Grid_OCN.CurrentCell = Grid_OCN["SL", 0];

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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["GRN_Qty"].Index)
                    {
                        MyBase.Row_Number(ref Grid);
                        if (Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value == null || Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                        }
                        if (Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid GRN Qty ...!", "Gainup");
                            Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value = Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["GRN_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            if ((Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value)) >= Excess_Limit)
                            {
                                e.Handled = true;
                                MessageBox.Show("GRN Qty Crossed Excess Limit [" + Excess_Limit + "] ...!", "Gainup");
                                Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value = Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value;
                                Grid.CurrentCell = Grid["GRN_Qty", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                            else
                            {
                                Grid["Amount", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["Rate", Grid.CurrentCell.RowIndex].Value);
                            }

                            if (!Grid_Amount())
                            {
                                e.Handled = true;
                            }
                            else
                            {
                                e.Handled = true;
                                Calculate_Item_Amount();
                                Grid_OCN.CurrentCell = Grid_OCN["ORDER_NO", 0];
                                Grid_OCN.Focus();
                                Grid_OCN.BeginEdit(true);
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

        Int32 Get_Max_Bag_No(Int32 Supplier_Code, String Lot_No)
        {
            Int32 Bag_No = 0;
            try
            {
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select Isnull(Max(Bag_No), 0) From Socks_Trims_GRN_OCN_Lot_Details Where SUpplier_Code = " + Supplier_Code.ToString() + " and Lot_No = '" + Lot_No + "'", ref Tdt);
                Bag_No = Convert.ToInt32(Tdt.Rows[0][0]);

                for (int i = 0; i <= 30 - 1; i++)
                {
                    for (int j = 0; j <= Max_Val - 1; j++)
                    {
                        for (int k = 1; k <= 1; k++)
                        {
                            if (Dt_OCN_New[i, j, k] != null)
                            {
                                for (int l = 0; l <= Dt_OCN_New[i, j, k].Rows.Count - 1; l++)
                                {
                                    if (Dt_OCN_New[i, j, k].Rows[l]["Lot_No"].ToString() == Lot_No && Convert.ToInt32(Dt_OCN_New[i, j, k].Rows[l]["Bag_No"]) >= Bag_No)
                                    {
                                        Bag_No = Convert.ToInt32(Dt_OCN_New[i, j, k].Rows[l]["Bag_No"]);
                                    }
                                }
                            }
                        }
                    }
                }
                return Bag_No + 1;
            }
            catch (Exception ex)
            {
                return Bag_No;
            }
        }


        Boolean Grid_Amount()
        {
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["GRN_Qty", i].Value == null || Grid["GRN_Qty", i].Value == DBNull.Value || Grid["GRN_Qty", i].Value.ToString() == String.Empty)
                    {
                        Grid["GRN_Qty", i].Value = "0.000";
                    }

                    if (Convert.ToDouble(Grid["GRN_Qty", i].Value) == 0)
                    {
                        MessageBox.Show("Invalid GRN Qty ...!", "Gainup");
                        Grid["GRN_Qty", i].Value = Grid["Bal_Qty", i].Value;
                        Grid.CurrentCell = Grid["GRN_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return false;
                    }
                    else
                    {
                        if ((Convert.ToDouble(Grid["GRN_Qty", i].Value) - Convert.ToDouble(Grid["Bal_Qty", i].Value)) >= Excess_Limit)
                        {
                            MessageBox.Show("GRN Qty Crossed Excess Limit [" + Excess_Limit + "] ...!", "Gainup");
                            Grid["GRN_Qty", i].Value = Grid["Bal_Qty", i].Value;
                            Grid.CurrentCell = Grid["GRN_Qty", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return false;
                        }
                        else
                        {
                            Grid["Amount", i].Value = Convert.ToDouble(Grid["GRN_Qty", i].Value) * Math.Round(Convert.ToDouble(Grid["Rate", i].Value),4);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void Grid_LotNo_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Lot == null)
                {
                    Txt_Lot = (TextBox)e.Control;
                    Txt_Lot.KeyDown += new KeyEventHandler(Txt_Lot_KeyDown);
                    Txt_Lot.KeyPress += new KeyPressEventHandler(Txt_Lot_KeyPress);
                    Txt_Lot.GotFocus += new EventHandler(Txt_Lot_GotFocus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Lot_GotFocus(object sender, EventArgs e)
        {
            try
            {

                MyBase.Row_Number(ref Grid_LotNo);
                if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Lot_No"].Index)
                {
                    if (Bal_Qty_Lot() > 0)
                    {
                        if (Grid_LotNo.CurrentCell.RowIndex > 0)
                        {
                            if (Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            {
                                Txt_Lot.Text = Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex - 1].Value.ToString();
                                Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value = Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex - 1].Value;
                            }
                        }
                    }
                }
                else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["qty"].Index)
                {
                    if (Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value) == 0)
                    {
                        Txt_Lot.Text = String.Format("{0:0.000}", Bal_Qty_Lot());
                        Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value = Txt_Lot.Text;
                    }
                }
                else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Bag_No"].Index)
                {
                    if (Bal_Qty_Lot() > 0)
                    {
                        if (Grid_LotNo.CurrentCell.RowIndex > 0)
                        {
                            if (Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            {
                                if (Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex - 1].Value.ToString())
                                {
                                    //Txt_Lot.Text = Convert.ToString(Convert.ToInt32(Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex - 1].Value) + 1);
                                    //Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value = Convert.ToInt32(Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex - 1].Value) + 1;
                                    Txt_Lot.Text = Get_Max_Bag_No(Convert.ToInt32(TxtSupplier.Tag), Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value.ToString()).ToString();
                                    Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value = Txt_Lot.Text;
                                }
                                else
                                {
                                    //Txt_Lot.Text = "1";
                                    //Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value = 1;
                                    Txt_Lot.Text = Get_Max_Bag_No(Convert.ToInt32(TxtSupplier.Tag), Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value.ToString()).ToString();
                                    Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value = Txt_Lot.Text;
                                }
                            }
                        }
                        else
                        {
                            //Txt_Lot.Text = "1";
                            //Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value = 1;
                            Txt_Lot.Text = Get_Max_Bag_No(Convert.ToInt32(TxtSupplier.Tag), Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value.ToString()).ToString();
                            Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value = Txt_Lot.Text;
                        }
                    }
                }
                else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Location"].Index)
                {
                    if (Grid_LotNo.CurrentCell.RowIndex > 0)
                    {
                        if (Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Txt_Lot.Text = Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex - 1].Value.ToString();
                            Grid_LotNo["Location_ID", Grid_LotNo.CurrentCell.RowIndex].Value = Grid_LotNo["Location_ID", Grid_LotNo.CurrentCell.RowIndex - 1].Value;
                            Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex].Value = Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex - 1].Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Lot_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["bag_no"].Index)
                {
                    e.Handled = true;
                }
                else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Qty"].Index)
                {
                    MyBase.Valid_Decimal(Txt_Lot, e);
                }
                else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Lot_No"].Index)
                {

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

        void Txt_Lot_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["location"].Index)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Location", "Select Location, RowID From Socks_Trim_Stores_Location_Master", String.Empty, 120);
                        if (Dr != null)
                        {
                            Grid_LotNo["Location_ID", Grid_LotNo.CurrentCell.RowIndex].Value = Dr["RowID"].ToString();
                            Grid_LotNo["LOcation", Grid_LotNo.CurrentCell.RowIndex].Value = Dr["Location"].ToString();
                            Txt_Lot.Text = Dr["Location"].ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_LotNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Qty"].Index)
                    {
                        if (Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value == null || Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value == DBNull.Value || Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value = "0.000";
                        }

                        if (Convert.ToDouble(Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Bag Weight ...!", "Gainup");
                            Grid_LotNo.CurrentCell = Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex];
                            Grid_LotNo.Focus();
                            Grid_LotNo.BeginEdit(true);
                            return;
                        }

                        if (Convert.ToDouble(Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value) > Bal_Qty_Lot1())
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Bag Weight greater than PO ...!", "Gainup");
                            Grid_LotNo.CurrentCell = Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex];
                            Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value = Bal_Qty_Lot();
                            Grid_LotNo.Focus();
                            Grid_LotNo.BeginEdit(true);
                            return;
                        }

                    }
                    else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Lot_No"].Index)
                    {
                        if (Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value == null || Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value == DBNull.Value || Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Lot No ...!", "Gainup");
                            Grid_LotNo.CurrentCell = Grid_LotNo["LOt_No", Grid_LotNo.CurrentCell.RowIndex];
                            Grid_LotNo.Focus();
                            Grid_LotNo.BeginEdit(true);
                            return;
                        }
                    }
                    else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Bag_No"].Index)
                    {
                        if (Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value == null || Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value == DBNull.Value || Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Bag No ...!", "Gainup");
                            Grid_LotNo.CurrentCell = Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex];
                            Grid_LotNo.Focus();
                            Grid_LotNo.BeginEdit(true);
                            return;
                        }
                    }
                    else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Location"].Index)
                    {
                        if (Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex].Value == null || Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex].Value == DBNull.Value || Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Location ...!", "Gainup");
                            Grid_LotNo.CurrentCell = Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex];
                            Grid_LotNo.Focus();
                            Grid_LotNo.BeginEdit(true);
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

        private void Grid_LotNo_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref Grid_LotNo, ref Dt_OCN_New[Grid.CurrentCell.RowIndex, Grid_OCN.CurrentCell.RowIndex, 1], Grid_LotNo.CurrentCell.RowIndex);
                Grid_LotNo.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_LotNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Grid_OCN.CurrentCell = Grid_OCN["Order_NO", Grid_OCN.CurrentCell.RowIndex + 1];
                    Grid_OCN.Focus();
                    Grid_OCN.BeginEdit(true);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            Int32 Row = 0;
            try
            {
                if (Grid_OCN.CurrentCell.RowIndex <= Dt_OCN_New[Grid.CurrentCell.RowIndex, Grid_OCN.CurrentCell.RowIndex, 0].Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Vaahini", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Dt_OCN_New[Grid.CurrentCell.RowIndex, 0, 0] = null;
                        Dt_OCN_New[Grid.CurrentCell.RowIndex, 0, 1] = null;
                        ReArrange_Datatable_Array_Item();
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                        Dt.AcceptChanges();
                        Grid_CurrentCellChanged(sender, e);
                    }
                }

                MyBase.Row_Number(ref Grid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void Grid_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell != null)
                {
                    Load_OCN(Grid.CurrentCell.RowIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_OCN_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Grid_OCN_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_OCN == null)
                {
                    Txt_OCN = (TextBox)e.Control;
                    Txt_OCN.KeyDown += new KeyEventHandler(Txt_OCN_KeyDown);
                    Txt_OCN.KeyPress += new KeyPressEventHandler(Txt_OCN_KeyPress);
                    Txt_OCN.GotFocus += new EventHandler(Txt_OCN_GotFocus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_OCN_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (Grid_OCN.CurrentCell.ColumnIndex == Grid_OCN.Columns["GRN_Qty"].Index)
                {
                    if (Bal_Qty_OCN() > 0)
                    {
                        if (Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value == null || Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value == DBNull.Value || Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Txt_OCN.Text = String.Format("{0:0.000}", Bal_Qty_OCN());
                            Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value = Bal_Qty_OCN();
                        }
                    }
                }
                
                /*else if (Grid_OCN.CurrentCell.ColumnIndex == Grid_OCN.Columns["ORDER_NO"].Index)
                {
                    if (Bal_Qty_OCN() > 0)
                    {
                        MyBase.Row_Number(ref Grid_OCN);
                        if (Grid_OCN.CurrentCell.RowIndex > 0)
                        {
                            if (Grid_OCN["Order_NO", Grid_OCN.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            {
                                Txt_OCN.Text = Grid_OCN["Order_NO", Grid_OCN.CurrentCell.RowIndex - 1].Value.ToString();
                                Grid_OCN["Order_NO", Grid_OCN.CurrentCell.RowIndex].Value = Grid_OCN["Order_NO", Grid_OCN.CurrentCell.RowIndex - 1].Value;
                                Grid_OCN["PONo", Grid_OCN.CurrentCell.RowIndex].Value = Grid_OCN["PONo", Grid_OCN.CurrentCell.RowIndex - 1].Value;
                                Grid_OCN["PO_Detail_ID", Grid_OCN.CurrentCell.RowIndex].Value = Grid_OCN["PO_Detail_ID", Grid_OCN.CurrentCell.RowIndex - 1].Value;
                                Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex].Value = Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex - 1].Value;
                            }
                        }
                    }
                }*/
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
                if (MyParent._New)
                {
                    Grid_Tax.DataSource = MyBase.Load_Data("Select S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From Socks_Trims_GRN_Tax_Details S1 Left Join Socks_Tax_Accounts() L1 on S1.Tax_Code = L1.Ledger_Code  Where 1 = 2 Order by S1.Slno ", ref Dt_Tax);
                }
                else
                {
                    Grid_Tax.DataSource = MyBase.Load_Data("Select S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From Socks_Trims_GRN_Tax_Details S1 Left Join Socks_Tax_Accounts() L1 on S1.Tax_Code = L1.Ledger_Code  Where S1.Master_ID = " + Code + " Order by S1.Slno ", ref Dt_Tax);
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


        void Txt_OCN_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid_OCN.CurrentCell.ColumnIndex == Grid_OCN.Columns["grn_Qty"].Index)
                {
                    MyBase.Valid_Decimal(Txt_OCN, e);
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

        void Txt_OCN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid_OCN.CurrentCell.ColumnIndex == Grid_OCN.Columns["ORDER_NO"].Index)
                    {
                        
                        if ((Convert.ToDouble(Grid["Grn_Qty", Grid.CurrentCell.RowIndex].Value)) > (Convert.ToDouble(Grid["Limit", Grid.CurrentCell.RowIndex].Value)))
                        {
                            MessageBox.Show("GRN Qty Crossed Excess Limit [" + (Convert.ToDouble(Grid["Limit", Grid.CurrentCell.RowIndex].Value)) + "] ...!", "Gainup");
                            Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value = Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["GRN_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            return;
                        }
                        MyBase.Row_Number(ref Grid_OCN);
                        //if (MyParent.UserCode == 1)
                        //{
                        //    Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt_OCN_New[Grid.CurrentCell.RowIndex, 0, 0], SelectionTool_Class.ViewType.NormalView, "Select OCN", "Select OrdeR_No, PoNo, PO_Qty, Order_ID, PO_Detail_ID, Description  From (Select Distinct S2.Order_No, S1.PONo, S1.Bal_Qty PO_Qty, S1.Order_ID, S1.PO_Detail_ID, (S2.Order_No + '-' + S1.PONo) Description From Socks_Trims_GRN_Pending_OCN () S1 Inner join Socks_Order_Master S2 on S1.Order_ID = S2.RowID and S2.Despatch_Closed = 'N' and S2.cancel_ORder = 'N' Where Supplier_Code = " + TxtSupplier.Tag + " and  S1.Bal_Qty > 0 and Item_ID = " + Grid["Item_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Color_ID = " + Grid["Color_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Size_ID = " + Grid["Size_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Rate = " + Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString() + "  and OrdeR_ID != 148 and S1.PODate <= '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "' Union all  Select Order_No, 'GUP-POA00000' PoNo, " + (Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value)) + " Po_Qty, RowID OrdeR_ID, 0 Po_Detail_ID, Order_No + '-' + 'GUP-POA00000' Description From Socks_Order_Master Where RowID = 148 and " + (Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value)) + " > 0  Union All Select Distinct 'GUP-OCN00000' Order_No, S1.PONo, S1.Bal_Qty PO_Qty, S1.Order_ID, S1.PO_Detail_ID, ('GUP-OCN00000' + '-' + S1.PONo) Description From Socks_Trims_GRN_Pending_OCN () S1 Inner join Socks_Order_Master S2 on S1.Order_ID = S2.RowID and S2.Despatch_Closed = 'Y' Where Supplier_Code = " + TxtSupplier.Tag + " and  S1.Bal_Qty > 0 and Item_ID = " + Grid["Item_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Color_ID = " + Grid["Color_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Size_ID = " + Grid["Size_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Rate = " + Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString() + "  and OrdeR_ID != 148 and S1.PODate <= '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "'  ) A ORder by PONO ", String.Empty, 120, 120, 100);
                        //}
                        //else
                        //{
                        //    Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt_OCN_New[Grid.CurrentCell.RowIndex, 0, 0], SelectionTool_Class.ViewType.NormalView, "Select OCN", "Select OrdeR_No, PoNo, PO_Qty, Order_ID, PO_Detail_ID, Description  From (Select Distinct S2.Order_No, S1.PONo, S1.Bal_Qty PO_Qty, S1.Order_ID, S1.PO_Detail_ID, (S2.Order_No + '-' + S1.PONo) Description From Socks_Trims_GRN_Pending_OCN () S1 Inner join Socks_Order_Master S2 on S1.Order_ID = S2.RowID and S2.Despatch_Closed = 'N' and S2.cancel_ORder = 'N' Where Supplier_Code = " + TxtSupplier.Tag + " and  S1.Bal_Qty > 0 and Item_ID = " + Grid["Item_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Color_ID = " + Grid["Color_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Size_ID = " + Grid["Size_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Rate = " + Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString() + "  and OrdeR_ID != 148 and S1.PODate <= '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "' Union all  Select Order_No, 'GUP-POA00000' PoNo, " + (Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value)) + " Po_Qty, RowID OrdeR_ID, 0 Po_Detail_ID, Order_No + '-' + 'GUP-POA00000' Description From Socks_Order_Master Where RowID = 148 and " + (Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value)) + " > 0) A ORder by PONO ", String.Empty, 120, 120, 100);
                        //}
                        Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt_OCN_New[Grid.CurrentCell.RowIndex, 0, 0], SelectionTool_Class.ViewType.NormalView, "Select OCN", "Select OrdeR_No, PoNo, PO_Qty, Order_ID, PO_Detail_ID, Description  From (Select Distinct S2.Order_No, S1.PONo, S1.Bal_Qty PO_Qty, S1.Order_ID, S1.PO_Detail_ID, (S2.Order_No + '-' + S1.PONo) Description From Socks_Trims_GRN_Pending_OCN () S1 left join Socks_Order_Master S2 on S1.Order_ID = S2.RowID Where Supplier_Code = " + TxtSupplier.Tag + " and  S1.Bal_Qty > 0 and Item_ID = " + Grid["Item_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Color_ID = " + Grid["Color_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Size_ID = " + Grid["Size_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Rate = " + Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString() + "  and OrdeR_ID != 148 and S1.PODate <= '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "' Union all  Select Order_No, 'GUP-POA00000' PoNo, " + (Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value)) + " Po_Qty, RowID OrdeR_ID, 0 Po_Detail_ID, Order_No + '-' + 'GUP-POA00000' Description From Socks_Order_Master Where RowID = 148 and " + (Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value)) + " > 0) A ORder by PONO ", String.Empty, 120, 120, 100);
                        
                        if (Dr != null)
                        {
                            if (Dr["order_NO"].ToString() == "GUP-OCN00000" && Dr["PONO"].ToString() == "GUP-POA00000")
                            {                                
                                if ((Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value))>0)
                                {
                                    Grid_OCN["Description", Grid_OCN.CurrentCell.RowIndex].Value = Dr["Description"].ToString();
                                    Grid_OCN["Order_id", Grid_OCN.CurrentCell.RowIndex].Value = Dr["order_id"].ToString();
                                    Txt_OCN.Text = Dr["order_NO"].ToString();
                                    Grid_OCN["Order_NO", Grid_OCN.CurrentCell.RowIndex].Value = Dr["order_NO"].ToString();
                                    Grid_OCN["PONO", Grid_OCN.CurrentCell.RowIndex].Value = Dr["PONO"].ToString();
                                    Grid_OCN["PO_Detail_ID", Grid_OCN.CurrentCell.RowIndex].Value = Dr["PO_Detail_ID"].ToString();
                                    Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value));
                                    Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value = Math.Round((Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value)),3);
                                    return;
                                }
                                else
                                {
                                    MessageBox.Show("Invalid OCN", "Gainup");
                                    return;
                                }
                            }
                            else if (Dr["order_NO"].ToString() == "GUP-OCN00000" && Dr["PONO"].ToString() != "GUP-POA00000")                            {
                                if (MyParent.UserCode == 1)
                                {
                                    Grid_OCN["Description", Grid_OCN.CurrentCell.RowIndex].Value = Dr["Description"].ToString();
                                    Grid_OCN["Order_id", Grid_OCN.CurrentCell.RowIndex].Value = Dr["order_id"].ToString();
                                    Txt_OCN.Text = Dr["order_NO"].ToString();
                                    Grid_OCN["Order_NO", Grid_OCN.CurrentCell.RowIndex].Value = Dr["order_NO"].ToString();
                                    Grid_OCN["PONO", Grid_OCN.CurrentCell.RowIndex].Value = Dr["PONO"].ToString();
                                    Grid_OCN["PO_Detail_ID", Grid_OCN.CurrentCell.RowIndex].Value = Dr["PO_Detail_ID"].ToString();
                                    Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex].Value = Dr["PO_Qty"].ToString();
                                    if (Bal_Qty_OCN() > Convert.ToDouble(Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex].Value))
                                    {
                                        Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value = Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex].Value;
                                    }
                                    else
                                    {
                                        Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value = Bal_Qty_OCN();
                                    }
                                    return;
                                }
                                else
                                {
                                    MessageBox.Show("Invalid OCN", "Gainup");
                                    return;
                                }
                            }

                            Grid_OCN["Description", Grid_OCN.CurrentCell.RowIndex].Value = Dr["Description"].ToString();
                            Grid_OCN["Order_id", Grid_OCN.CurrentCell.RowIndex].Value = Dr["order_id"].ToString();
                            Txt_OCN.Text = Dr["order_NO"].ToString();
                            Grid_OCN["Order_NO", Grid_OCN.CurrentCell.RowIndex].Value = Dr["order_NO"].ToString();
                            Grid_OCN["PONO", Grid_OCN.CurrentCell.RowIndex].Value = Dr["PONO"].ToString();
                            Grid_OCN["PO_Detail_ID", Grid_OCN.CurrentCell.RowIndex].Value = Dr["PO_Detail_ID"].ToString();
                            Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex].Value = Dr["PO_Qty"].ToString();
                            if (Bal_Qty_OCN() > Convert.ToDouble(Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex].Value))
                            {
                                Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value = Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex].Value;
                            }
                            else
                            {
                                Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value = Bal_Qty_OCN();
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

        private void Grid_OCN_DoubleClick(object sender, EventArgs e)
        {
            Int32 Row = 0;
            try
            {
                if (Grid_OCN.CurrentCell.RowIndex <= Dt_OCN_New[Grid.CurrentCell.RowIndex, 0, 0].Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Vaahini", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Dt_OCN_New[Grid.CurrentCell.RowIndex, Grid_OCN.CurrentCell.RowIndex, 1] = null;
                        ReArrange_Datatable_Array_OCN();
                        Dt_OCN_New[Grid.CurrentCell.RowIndex, 0, 0].Rows.RemoveAt(Grid_OCN.CurrentCell.RowIndex);
                        Grid_OCN_CurrentCellChanged(sender, e);
                    }
                }
                
                MyBase.Row_Number(ref Grid_OCN);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        Boolean IsAllNullInDatatableArray_OCN()
        {
            try
            {
                for (int i = 0; i <= Max_Val - 1; i++)
                {
                    if (Dt_OCN_New[Grid.CurrentCell.RowIndex, i, 1] != null)
                    {
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

        Boolean ReArrange_Datatable_Array_OCN()
        {
            Boolean IsAllNull = true;
            try
            {
                if (IsAllNullInDatatableArray_OCN())
                {
                    return true;
                }
                else
                {
                    for (int i = 0; i <= Max_Val - 2; i++)
                    {
                        if (Dt_OCN_New[Grid.CurrentCell.RowIndex, i, 1] == null && Dt_OCN_New[Grid.CurrentCell.RowIndex, i +1, 1] != null)
                        {
                            Dt_OCN_New[Grid.CurrentCell.RowIndex, i, 1] = Dt_OCN_New[Grid.CurrentCell.RowIndex, i + 1, 1].Copy();
                            Dt_OCN_New[Grid.CurrentCell.RowIndex, i + 1, 1] = null;
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        Boolean IsAllNullInDatatableArray_Item()
        {
            try
            {
                for (int i = 0; i <= Max_Val - 1; i++)
                {
                    for (int j = 0; j <= Max_Val - 1; j++)
                    {
                        for (int k = 0; k <= 1; k++)
                        {
                            if (Dt_OCN_New[i, j, k] != null)
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        Boolean ReArrange_Datatable_Array_Item()
        {
            Boolean IsAllNull = true;
            try
            {
                if (IsAllNullInDatatableArray_Item())
                {
                    return true;
                }
                else
                {

                    for (int i = 0; i <= Max_Val - 2; i++)
                    {
                        for (int j = 0; j <= Max_Val - 1; j++)
                        {
                            for (int k = 0; k <= 1; k++)
                            {
                                if (Dt_OCN_New[i, j, k] == null && Dt_OCN_New[i + 1, j, k] != null)
                                {
                                    Dt_OCN_New[i, j, k] = Dt_OCN_New[i + 1, j, k].Copy();
                                    Dt_OCN_New[i + 1, j, k] = null;
                                }
                            }
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        private void Grid_OCN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid_OCN.CurrentCell.ColumnIndex == Grid_OCN.Columns["GRN_Qty"].Index)
                    {
                        if (Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value == null || Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value == DBNull.Value || Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value = "0.000";
                        }


                        if (Convert.ToDouble(Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Qty ...!", "Gainup");
                            Grid_OCN.CurrentCell = Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex];
                            Grid_OCN.Focus();
                            Grid_OCN.BeginEdit(true);
                            return;
                        }

                        if (Convert.ToDouble(Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Qty greater than PO ...!", "Gainup");
                            Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value = Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex].Value;
                            Grid_OCN.CurrentCell = Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex];
                            Grid_OCN.Focus();
                            Grid_OCN.BeginEdit(true);
                            return;
                        }

                        e.Handled = true;
                        Load_Lot(Grid_OCN.CurrentCell.RowIndex);
                        Grid_LotNo.CurrentCell = Grid_LotNo["LOt_No", 0];
                        Grid_LotNo.Focus();
                        Grid_LotNo.BeginEdit(true);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_OCN_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Grid.CurrentCell = Grid["Item", Grid.CurrentCell.RowIndex + 1];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_OCN_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid_OCN.CurrentCell != null)
                {
                    Load_Lot(Grid_OCN.CurrentCell.RowIndex);
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
                        Grid_Tax["Tax_Amount", i].Value = String.Format("{0:0}", Convert.ToDouble(Grid_Tax["Tax_Per", i].Value) * (Convert.ToDouble(Txt_Gross.Text) / 100));
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
                    Dr = Tool.Selection_Tool_Except_New("Tax_Code", this, 30, 70, ref Dt_Tax, SelectionTool_Class.ViewType.NormalView, "Select Tax", "Select Ledger_Name Tax, Ledger_Code Tax_Code From Socks_Tax_Accounts()", String.Empty, 250);
                    if (Dr != null)
                    {
                        MyBase.Row_Number(ref Grid_Tax);
                        Grid_Tax["Tax", Grid_Tax.CurrentCell.RowIndex].Value = Dr["Tax"].ToString();
                        Grid_Tax["Tax_Code", Grid_Tax.CurrentCell.RowIndex].Value = Dr["Tax_Code"].ToString();
                        Txt_Tax.Text = Dr["Tax"].ToString();

                        DataTable Tdt = new DataTable();
                        MyBase.Load_Data("Select Dbo.Socks_Get_Tax_Per (" + Dr["Tax_Code"].ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", MyBase.GetServerDate()) + "')", ref Tdt);
                        if (Convert.ToDouble(Tdt.Rows[0][0]) > 0)
                        {
                            Grid_Tax["Tax_Mode", Grid_Tax.CurrentCell.RowIndex].Value = "Y";
                            Grid_Tax["Tax_Per", Grid_Tax.CurrentCell.RowIndex].Value = Convert.ToDouble(Tdt.Rows[0][0]);
                            Grid_Tax["Tax_Amount", Grid_Tax.CurrentCell.RowIndex].Value = Convert.ToDouble(String.Format("{0:0}", (Convert.ToDouble(Txt_Gross.Text) / 100) * Convert.ToDouble(Tdt.Rows[0][0])));
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

        private void Grid_Tax_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar (Keys.Escape))
                {
                    Calculate_Item_Amount();
                    Txt_Gross.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        Boolean Calculate_Item_Amount()
        {
            try
            {

                if (Txt_Gross.Text == String.Empty)
                {
                    Txt_Gross.Text = "0";
                }
                Txt_Gross.Text = String.Format("{0:n}", Convert.ToDouble(Txt_Gross.Text));

                if (Txt_Qty.Text == String.Empty)
                {
                    Txt_Qty.Text = "0";
                }
                Txt_Qty.Text = String.Format("{0:0.000}", Convert.ToDouble(Txt_Qty.Text));


                if (Txt_NetAmount.Text == String.Empty)
                {
                    Txt_NetAmount.Text = "0";
                }
                Txt_NetAmount.Text = String.Format("{0:n}", Convert.ToDouble(Txt_NetAmount.Text));

                if (Txt_Tax_Amount.Text == String.Empty)
                {
                    Txt_Tax_Amount.Text = "0";
                }
                Txt_Tax_Amount.Text = String.Format("{0:n}", Convert.ToDouble(Txt_Tax_Amount.Text));



                Txt_Gross.Text = String.Format ("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "Amount", "Item_ID", "Color_ID", "Size_ID")));
                Txt_Qty.Text = String.Format ("{0:0.000}", Convert.ToDouble(MyBase.Sum(ref Grid, "GRN_Qty", "Item_ID", "Color_ID", "Size_ID")));

                Refresh_Tax();

                Txt_Tax_Amount.Text = String.Format ("{0:n}", Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid_Tax, "Tax_Amount", "Tax_Code", "Tax")))));
                Txt_NetAmount.Text = String.Format("{0:0}", Convert.ToDouble(Txt_Gross.Text) + Convert.ToDouble(Txt_Tax_Amount.Text));
                Txt_NetAmount.Text = String.Format("{0:n}", Convert.ToDouble(Txt_NetAmount.Text));

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Calculate_Item_Amount();
                    Grid_Tax.CurrentCell = Grid_Tax["Tax", 0];
                    Grid_Tax.Focus();
                    Grid_Tax.BeginEdit(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

    }
}