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
    public partial class FrmSocksTrimsPOApproval : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        Int64 Code = 0;

        // Entry_New also Declared
        DataTable Dt = new DataTable();
        DataTable Dt_Virtual = new DataTable();

        DataTable Dt_OCN = new DataTable();
        DataTable Dt_Item = new DataTable();
        DataTable[] Dt_Item_OCN;
        Boolean Status_Flag = false;
        DataTable Dt_Tax = new DataTable();
        TextBox Txt_Tax = null;
        TextBox Txt = null;
        TextBox Txt_Item = null;
        TextBox Txt_OCN = null;
        public FrmSocksTrimsPOApproval()
        {
            InitializeComponent();
        }


        void PONO_Generate()
        {
            try
            {
                return;
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select DBo.Get_Max_Socks_Trims_PO ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "')", ref Tdt);
                TxtPONO.Text = Tdt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                TxtPONO.Text = String.Empty;
                throw ex;
            }
        }

        public void Entry_New()
        {
            try
            {
                MyBase.Enable_Controls(this, true);
                MyBase.Clear(this);
                Load_Type();
                Code = 0;
                Dt = new DataTable();
                Dt_OCN = new DataTable();
                Dt_Item = new DataTable();
                Dt_Tax = new DataTable();
                Dt_Item_OCN = new DataTable[15];

                CmbBasedOn.Enabled = true;
                TxtBuyer.Enabled = true;
                Grid_OCN.Enabled = true;
                checkBox1.Enabled = true;
                
                DtpReqDate.Value = DtpDate.Value.AddDays(15);
                PONO_Generate();
                TxtSupplier.Focus();
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
                Code = Convert.ToInt64(Dr["RowiD"]);
                TxtPONO.Text = Dr["PONo"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["PODate"]);
                TxtSupplier.Tag = Dr["Supplier_Code"].ToString();
                TxtSupplier.Text = Dr["Supplier"].ToString();
                TxtBuyer.Tag = Dr["Buyer_Code"].ToString();
                TxtBuyer.Text = Dr["Buyer"].ToString();
                TxtBuyer.Enabled = false;
                CmbBasedOn.SelectedIndex = Convert.ToInt32(Dr["PO_Method"]);
                CmbBasedOn.Enabled = false;

                if (CmbBasedOn.Text == "OCN WISE")
                {
                    tabControl1.SelectTab(tabPage1);
                    Load_OCN();
                    Load_Pivot_OCN (String.Empty);
                    Load_Tax();

                    if (!MyParent._New)  // Default Checked
                    {
                        for (int i = 0; i <= Dt_OCN.Rows.Count - 1; i++)
                        {
                            Grid_OCN["Status", i].Value = true;
                        }
                    }
                }
                else
                {
                    Dt_Item = new DataTable();
                    Dt_Item_OCN = new DataTable[15];
                    tabControl1.SelectTab(tabPage2);
                    Load_Dt_Item_OCN();
                    Load_Item();
                    Load_Tax();

                    Grid_Item.CurrentCell = Grid_Item["Item", 0];
                    Grid_Item.Focus();
                    Grid_Item.BeginEdit (true);
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
                MyBase.Enable_Controls(this, true);
                MyBase.Clear(this);
                Load_Type();
                ButApp.Text = CmbType.Text;
                if (CmbType.SelectedIndex == 1)
                {
                    Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select PO Details - Reject", "Select S1.PONo, L1.Ledger_Name Supplier, S1.PoDate, S3.Order_No, i1.item, c1.color, s4.size, s2.Order_Qty, S1.RowID, S1.PO_Method, S1.Supplier_Code, S1.Buyer_Code, L2.Ledger_Name Buyer From Socks_Trims_PO_Master S1 Inner join Socks_Trims_PO_Details S2 ON S1.RowID = s2.Master_ID Inner join Socks_Order_Master S3 on S2.Order_ID = S3.RowID inner join item I1 on S2.Item_id = I1.itemid inner join color C1 on s2.Color_id = c1.colorid inner join size S4 on s2.Size_ID = S4.sizeid left Join Accounts.Dbo.Ledger_Master L1 on S1.Supplier_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' Left Join Accounts.Dbo.Ledger_Master L2 on S1.buyer_Code = L2.Ledger_Code and L2.COMPANY_CODE = " + MyParent.CompCode + " and L2.YEAR_CODE = '" + MyParent.YearCode + "' LEft Join Socks_Trims_GRN_OCN_DEtails P1 On S2.RowID = P1.PO_Detail_ID Where  S1.Approval_Flag = 'T' and S1.RowID != 168 and P1.Detail_ID IS Null  ", String.Empty, 120, 250, 100, 120, 120, 120, 100, 100);
                }
                else
                {
                    Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select PO Details - Approve", "Select S1.PONo, L1.Ledger_Name Supplier, S1.PoDate, S3.Order_No, i1.item, c1.color, s4.size, s2.Order_Qty, S1.RowID, S1.PO_Method, S1.Supplier_Code, S1.Buyer_Code, L2.Ledger_Name Buyer From Socks_Trims_PO_Master S1 Inner join Socks_Trims_PO_Details S2 ON S1.RowID = s2.Master_ID Inner join Socks_Order_Master S3 on S2.Order_ID = S3.RowID inner join item I1 on S2.Item_id = I1.itemid inner join color C1 on s2.Color_id = c1.colorid inner join size S4 on s2.Size_ID = S4.sizeid left Join Accounts.Dbo.Ledger_Master L1 on S1.Supplier_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' Left Join Accounts.Dbo.Ledger_Master L2 on S1.buyer_Code = L2.Ledger_Code and L2.COMPANY_CODE = " + MyParent.CompCode + " and L2.YEAR_CODE = '" + MyParent.YearCode + "' Where S1.Approval_Flag = 'F' ", String.Empty, 120, 250, 100, 120, 120, 120, 100, 100);
                }
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


        public void Entry_Save()
        {
            int Slno = 1;
            try
            {
                return;
                if (TxtSupplier.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Supplier ...!", "Gainup");
                    TxtSupplier.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (CmbBasedOn.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Mode ...!", "Gainup");
                    CmbBasedOn.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (CmbBasedOn.Text == "OCN WISE")
                {
                    
                    Calculate_Item_Amount();

                    if (TxtBuyer.Text.Trim() == String.Empty)
                    {
                        MessageBox.Show("Invalid Buyer ...!", "Gainup");
                        TxtBuyer.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }

                    if (Dt.Rows.Count == 0 || Dt_OCN.Rows.Count == 0)
                    {
                        MessageBox.Show("Invalid Buyer ...!", "Gainup");
                        TxtBuyer.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }

                    if (!Validate_Dt_and_DtVirtual())
                    {
                        MessageBox.Show("Invalid OCN List [Virtual Comparision]. Contact IT ...!", "Gainup");
                        TxtBuyer.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                else
                {
                    Calculate_Item_Amount_1();

                    if (Dt_Item.Rows.Count == 0)
                    {
                        MessageBox.Show("Invalid Details ...!", "Gainup");
                        TxtSupplier.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }


                    for (int i = 0; i <= Dt_Item.Rows.Count - 1; i++)
                    {
                        if (!Verify_OCN_Qty(i))
                        {
                            MessageBox.Show("Invalid OCN Wise Qty Details ...!", "Gainup");
                            MyParent.Save_Error = true;
                            Grid_Item.CurrentCell = Grid_Item["PO_Qty", i];
                            Grid_Item.Focus();
                            Grid_Item.BeginEdit(true);
                            return;
                        }
                    }

                    if (TxtBuyer.Text.Trim() == String.Empty)
                    {
                        TxtBuyer.Tag = "0";
                    }
                }

                if (!Valid_Tax())
                {
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtQTY.Text.Trim() == String.Empty || Convert.ToDouble(TxtQTY.Text) == 0)
                {
                    MessageBox.Show("Invalid Qty ...!", "Gainup");
                    TxtSupplier.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtAmount.Text.Trim() == String.Empty || Convert.ToDouble(TxtAmount.Text) == 0)
                {
                    MessageBox.Show("Invalid Amount ...!", "Gainup");
                    TxtSupplier.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                PONO_Generate();

                String[] Queries = new String[500];
                Int32 Array_Index = 0;

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert Into Socks_Trims_PO_Master (PoNo, PODate, Supplier_Code, Required_Date, PO_Method, Buyer_Code) Values ('" + TxtPONO.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtSupplier.Tag.ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpReqDate.Value) + "', " + CmbBasedOn.SelectedIndex + ", " + TxtBuyer.Tag.ToString() + "); Select Scope_Identity ()";
                }
                else
                {
                    Queries[Array_Index++] = "update Socks_Trims_PO_Master Set Supplier_Code = " + TxtSupplier.Tag.ToString() + ", Required_Date = '" + String.Format ("{0:dd-MMM-yyyy}", DtpReqDate.Value) + "' Where RowID = " + Code;
                    Queries[Array_Index++] = "Delete from Socks_Trims_Tax_Details Where Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete from Socks_Trims_PO_OCN_Details Where Master_ID = " + Code;
                    Queries[Array_Index++] = "update S2 Set S2.PO_Qty = S2.Po_Qty - (Case when S1.Color_ID = 867 then (Case when S1.Order_Qty > S2.PO_Qty then S2.PO_Qty else S1.Order_Qty end) Else (S1.Order_Qty) End) From Socks_Trims_PO_Details S1 Left Join Socks_Trims_BOM_Status S2 on S1.Order_ID = S2.Order_ID and S1.Item_ID = S2.Item_ID and S1.Color_id = S2.Color_ID and S1.Size_ID = S2.Size_ID Where S1.Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_Trims_PO_Details Where Master_ID = " + Code;
                }

                if (CmbBasedOn.Text == "OCN WISE")
                {

                    // OCN List With Weight
                    Slno = 1;
                    for (int i = 0; i <= Dt_Virtual.Rows.Count - 1; i++)
                    {
                        for (int j = 7; j <= Dt_Virtual.Columns.Count - 1; j++)
                        {
                            if (Convert.ToDouble(Dt_Virtual.Rows[i][j]) > 0)
                            {
                                if (MyParent._New)
                                {
                                    Queries[Array_Index++] = "Insert Into Socks_Trims_PO_Details (Master_ID, Slno, Order_ID, Item_ID, Color_ID, Size_ID, Order_Qty, Rate, Amount) values (@@IDENTITY, " + Slno + ", " + Dt_Virtual.Columns[j].ColumnName + ", " + Dt_Virtual.Rows[i]["Item_ID"].ToString() + ", " + Dt_Virtual.Rows[i]["Color_ID"].ToString() + ", " + Dt_Virtual.Rows[i]["Size_ID"].ToString() + ", " + Dt_Virtual.Rows[i][j].ToString() + ", " + Dt.Rows[i]["Rate"].ToString() + ", " + Convert.ToDouble(Dt_Virtual.Rows[i][j]) * Convert.ToDouble(Dt.Rows[i]["Rate"]) + ")";
                                }
                                else
                                {
                                    Queries[Array_Index++] = "Insert Into Socks_Trims_PO_Details (Master_ID, Slno, Order_ID, Item_ID, Color_ID, Size_ID, Order_Qty, Rate, Amount) values (" + Code + ", " + Slno + ", " + Dt_Virtual.Columns[j].ColumnName + ", " + Dt_Virtual.Rows[i]["Item_ID"].ToString() + ", " + Dt_Virtual.Rows[i]["Color_ID"].ToString() + ", " + Dt_Virtual.Rows[i]["Size_ID"].ToString() + ", " + Dt_Virtual.Rows[i][j].ToString() + ", " + Dt.Rows[i]["Rate"].ToString() + ", " + Convert.ToDouble(Dt_Virtual.Rows[i][j]) * Convert.ToDouble(Dt.Rows[i]["Rate"]) + ")";
                                }
                                Queries[Array_Index++] = "UPdate Socks_Trims_BOM_Status Set PO_Qty = PO_Qty + " + Dt_Virtual.Rows[i][j].ToString() + " Where Order_ID = " + Dt_Virtual.Columns[j].ColumnName + " And Item_ID = " + Dt_Virtual.Rows[i]["Item_ID"].ToString() + " And Color_ID = " + Dt_Virtual.Rows[i]["Color_ID"].ToString() + " And Size_ID = " + Dt_Virtual.Rows[i]["Size_ID"].ToString();
                                Slno++;
                            }
                        }
                    }

                    // OCN List
                    Slno = 1;
                    for (int i = 0; i <= Grid_OCN.Rows.Count - 1; i++)
                    {
                        if (Grid_OCN["Status", i].Value != null && Grid_OCN["Status", i].Value != DBNull.Value && Grid_OCN["Status", i].Value.ToString().ToUpper() == "true".ToUpper())
                        {
                            if (MyParent._New)
                            {
                                Queries[Array_Index++] = "Insert into Socks_Trims_PO_OCN_Details (Master_ID, Slno, Order_ID) values (@@IDENTITY, " + Slno + ", " + Grid_OCN["RowID", i].Value.ToString() + ")";
                            }
                            else
                            {
                                Queries[Array_Index++] = "Insert into Socks_Trims_PO_OCN_Details (Master_ID, Slno, Order_ID) values (" + Code + ", " + Slno + ", " + Grid_OCN["RowID", i].Value.ToString() + ")";
                            }
                            Slno++;
                        }
                    }
                }
                else
                {
                    Slno = 1;
                    for (int i = 0; i <= Dt_Item.Rows.Count - 1; i++)
                    {
                        for (int j = 0; j <= Dt_Item_OCN[i].Rows.Count - 1; j++)
                        {
                            if (MyParent._New)
                            {
                                Queries[Array_Index++] = "Insert into Socks_Trims_PO_Details (Master_ID, Slno, Order_ID, Item_ID, Color_ID, Size_ID, Order_Qty, Rate, Amount) Values (@@IDENTITY, " + Slno + ", " + Dt_Item_OCN[i].Rows[j]["Order_ID"].ToString() + ", " + Dt_Item.Rows[i]["Item_ID"].ToString() + ", " + Dt_Item.Rows[i]["Color_ID"].ToString() + ", " + Dt_Item.Rows[i]["Size_ID"].ToString() + ", " + Dt_Item_OCN[i].Rows[j]["PO_Qty"].ToString() + ", " + Dt_Item.Rows[i]["Rate"].ToString() + ", " + (Convert.ToDouble(Dt_Item_OCN[i].Rows[j]["PO_Qty"]) * Convert.ToDouble(Dt_Item.Rows[i]["Rate"])) + ")";
                            }
                            else
                            {
                                Queries[Array_Index++] = "Insert into Socks_Trims_PO_Details (Master_ID, Slno, Order_ID, Item_ID, Color_ID, Size_ID, Order_Qty, Rate, Amount) Values (" + Code + ", " + Slno + ", " + Dt_Item_OCN[i].Rows[j]["Order_ID"].ToString() + ", " + Dt_Item.Rows[i]["Item_ID"].ToString() + ", " + Dt_Item.Rows[i]["Color_ID"].ToString() + ", " + Dt_Item.Rows[i]["Size_ID"].ToString() + ", " + Dt_Item_OCN[i].Rows[j]["PO_Qty"].ToString() + ", " + Dt_Item.Rows[i]["Rate"].ToString() + ", " + (Convert.ToDouble(Dt_Item_OCN[i].Rows[j]["PO_Qty"]) * Convert.ToDouble(Dt_Item.Rows[i]["Rate"])) + ")";
                            }

                            Queries[Array_Index++] = "UPdate Socks_Trims_BOM_Status Set PO_Qty = PO_Qty + " + Dt_Item_OCN[i].Rows[j]["PO_Qty"].ToString() + "  Where Order_ID = " + Dt_Item_OCN[i].Rows[j]["Order_ID"].ToString() + " and Item_ID = " + Dt_Item.Rows[i]["Item_ID"].ToString() + " and Color_ID = " + Dt_Item.Rows[i]["Color_ID"].ToString() + " and Size_ID = " + Dt_Item.Rows[i]["Size_ID"].ToString();
                            Slno++;
                        }
                    }
                }


                for (int i = 0; i <= Dt_Tax.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Socks_Trims_Tax_Details (Master_ID, Slno, Tax_Code, Mode, Tax_Per, Tax_Amount) Values (@@IDENTITY, " + (i + 1) + ", " + Dt_Tax.Rows[i]["Tax_Code"].ToString() + ", '" + Dt_Tax.Rows[i]["Tax_Mode"].ToString() + "', " + Dt_Tax.Rows[i]["Tax_Per"].ToString() + ", " + Dt_Tax.Rows[i]["Tax_Amount"].ToString() + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Socks_Trims_Tax_Details (Master_ID, Slno, Tax_Code, Mode, Tax_Per, Tax_Amount) Values (" + Code + ", " + (i + 1) + ", " + Dt_Tax.Rows[i]["Tax_Code"].ToString() + ", '" + Dt_Tax.Rows[i]["Tax_Mode"].ToString() + "', " + Dt_Tax.Rows[i]["Tax_Per"].ToString() + ", " + Dt_Tax.Rows[i]["Tax_Amount"].ToString() + ")";
                    }
                }

                MyBase.Run_Identity(MyParent.Edit, Queries);

                MessageBox.Show("Saved ...!", "Gainup");
                MyParent.Save_Error = false;
                MyBase.Clear(this);
                TxtSupplier.Focus();

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
                MyBase.Enable_Controls(this, false);
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select PO Details - Delete", "Select S1.PONo, L1.Ledger_Name Supplier, S1.PoDate, S3.Order_No, i1.item, c1.color, s4.size, s2.Order_Qty, S1.RowID, S1.PO_Method, S1.Supplier_Code, S1.Buyer_Code, L2.Ledger_Name Buyer From Socks_Trims_PO_Master S1 Inner join Socks_Trims_PO_Details S2 ON S1.RowID = s2.Master_ID Inner join Socks_Order_Master S3 on S2.Order_ID = S3.RowID inner join item I1 on S2.Item_id = I1.itemid inner join color C1 on s2.Color_id = c1.colorid inner join size S4 on s2.Size_ID = S4.sizeid left Join Accounts.Dbo.Ledger_Master L1 on S1.Supplier_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' left Join Accounts.Dbo.Ledger_Master L2 on S1.buyer_Code = L2.Ledger_Code and L2.COMPANY_CODE = " + MyParent.CompCode + " and L2.YEAR_CODE = '" + MyParent.YearCode + "' ", String.Empty, 120, 250, 100, 120, 120, 120, 100, 100);
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
            String[] Queries = new String[10];
            Int32 Array_Index = 0;
            try
            {
                if (Code > 0)
                {
                    Queries[Array_Index++] = "Delete from Socks_Trims_Tax_Details Where Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete from Socks_Trims_PO_OCN_Details Where Master_ID = " + Code;
                    if (CmbBasedOn.Text == "OCN WISE")
                    {
                        Queries[Array_Index++] = "update S2 Set S2.PO_Qty = S2.Po_Qty - (Case when S1.Color_ID = 867 then (Case when S1.Order_Qty > S2.PO_Qty then S2.PO_Qty else S1.Order_Qty end) Else (S1.Order_Qty) End) From Socks_Trims_PO_Details S1 Left Join Socks_Trims_BOM_Status S2 on S1.Order_ID = S2.Order_ID and S1.Item_ID = S2.Item_ID and S1.Color_id = S2.Color_ID and S1.Size_ID = S2.Size_ID Where S1.Master_ID = " + Code;
                    }
                    else
                    {
                        Queries[Array_Index++] = "update S2 Set S2.PO_Qty = S2.Po_Qty - S1.Order_Qty From Socks_Trims_PO_Details S1 Left Join Socks_Trims_BOM_Status S2 on S1.Order_ID = S2.Order_ID and S1.Item_ID = S2.Item_ID and S1.Color_id = S2.Color_ID and S1.Size_ID = S2.Size_ID Where S1.Master_ID = " + Code;
                    }

                    Queries[Array_Index++] = "Delete From Socks_Trims_PO_Details Where Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_Trims_PO_Master Where RowID = " + Code;

                    MyBase.Run(Queries);
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
                MyBase.Enable_Controls(this, false);
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select PO Details - View", "Select S1.PONo, L1.Ledger_Name Supplier, S1.PoDate, S3.Order_No, i1.item, c1.color, s4.size, s2.Order_Qty, S1.RowID, S1.PO_Method, S1.Supplier_Code, S1.Buyer_Code, L2.Ledger_Name Buyer From Socks_Trims_PO_Master S1 Inner join Socks_Trims_PO_Details S2 ON S1.RowID = s2.Master_ID Inner join Socks_Order_Master S3 on S2.Order_ID = S3.RowID inner join item I1 on S2.Item_id = I1.itemid inner join color C1 on s2.Color_id = c1.colorid inner join size S4 on s2.Size_ID = S4.sizeid left Join Accounts.Dbo.Ledger_Master L1 on S1.Supplier_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' left Join Accounts.Dbo.Ledger_Master L2 on S1.buyer_Code = L2.Ledger_Code and L2.COMPANY_CODE = " + MyParent.CompCode + " and L2.YEAR_CODE = '" + MyParent.YearCode + "' ", String.Empty, 120, 250, 100, 120, 120, 120, 100, 100);
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

                Str = " Select S1.PONo, L1.Ledger_Name Supplier, S1.PoDate, S1.Required_Date, (Case When S1.PO_Method = 0 Then 'OCN-WISE' When S1.PO_Method = 0 Then 'ITEM-WISE' End) PO_Method, L1.Ledger_Address Supplier_Address, L1.Ledger_Phone Supplier_Phone, L1.Ledger_email Supplier_Email From Socks_Trims_PO_Master S1 Left Join Accounts.Dbo.Ledger_Master L1 on S1.Supplier_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' Where S1.RowID = " + Code;
                MyBase.Load_Data(Str, ref Dt1);

                Str1 = " Select Top 100000000 ROW_NUMBER()Over(Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size)SlNo, I1.Item + ' - ' + C1.color + ' - ' + S4.Size Item_Color_Size, S2.Order_Qty, S2.Rate, S2.Amount From Socks_Trims_PO_Master S1 Inner join Socks_Trims_PO_Details S2 ON S1.RowID = S2.Master_ID Inner join Socks_Order_Master S3 on S2.Order_ID = S3.RowID Inner join Item I1 on S2.Item_id = I1.itemid Inner join color C1 on s2.Color_id = c1.colorid Inner join size S4 on s2.Size_ID = S4.sizeid Where S1.RowID = " + Code + " Order By I1.Item + ' - ' + C1.color + ' - ' + S4.Size ";
                MyBase.Execute_Qry(Str1, "Socks_Trims_PO");

                Str2 = " Select Top 4 S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From Socks_Trims_Tax_Details S1 Left Join Accounts.dbo.Ledger_Master L1 on S1.Tax_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' Where S1.Master_ID = " + Code + " Order by S1.Slno ";
                MyBase.Load_Data(Str2, ref Dt2);

                Str3 = " Select Distinct S3.Order_No From Socks_Trims_PO_Master S1 Inner join Socks_Trims_PO_Details S2 ON S1.RowID = S2.Master_ID Inner join Socks_Order_Master S3 on S2.Order_ID = S3.RowID Where S1.RowID = " + Code;
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
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RptTrimsPurchasePO.rpt");
                if (Dt1.Rows[0][0].ToString().Contains("GUP-POY"))
                {
                    MyParent.FormulaFill(ref ObjRpt, "Heading", "YARN PURCHASE ORDER");
                }
                else
                {
                    MyParent.FormulaFill(ref ObjRpt, "Heading", "ACCESSORY PURCHASE ORDER");
                }
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
                MyParent.FormulaFill(ref ObjRpt, "Net_Amount", TxtTotal.Text.ToString());
                MyParent.FormulaFill(ref ObjRpt, "Order", Order.ToString());
                MyParent.CReport(ref ObjRpt, "Accessory Purchase Order..!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Load_Type()
        {
            try
            {
                CmbBasedOn.Items.Clear();
                CmbBasedOn.Items.Add("OCN WISE");
                CmbBasedOn.Items.Add("ITEM WISE");
                CmbBasedOn.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Load_OCN()
        {
            try
            {
            //    if (MyParent._New)
            //    {
            //        Grid_OCN.DataSource = MyBase.Load_Data("Select S1.RowID, S1.Order_No From SocksPO_Trims_Pending_OCN (" + TxtBuyer.Tag.ToString() + ") S1 Order by S1.Order_NO", ref Dt_OCN);
            //        //Grid_OCN.DataSource = MyBase.Load_Data("Select S1.RowID, S1.Order_No From SocksPO_Trims_Pending_OCN (" + TxtBuyer.Tag.ToString() + ") S1 Inner join Socks_Time_Action_PO_Complete_Orders () S2 on S1.Order_No = S2.Order_No Where S2.Complete_Date_End <= Cast(GetDate() as Date) Order by S1.Order_NO", ref Dt_OCN);
            //    }
            //    else
            //    {
                    Grid_OCN.DataSource = MyBase.Load_Data("Select S2.RowID, S2.Order_No From Socks_Trims_PO_OCN_Details S1 Inner join Socks_Order_Master S2 on S1.Order_ID = S2.RowID Where S1.Master_ID = " + Code + " order by S2.Order_NO", ref Dt_OCN);
                //}
                MyBase.Grid_Designing(ref Grid_OCN, ref Dt_OCN, "RowID");
                MyBase.Grid_Colouring(ref Grid_OCN, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid_OCN);

                if (Status_Flag)
                {
                    Grid_OCN.Columns.Remove("Status");
                    DataGridViewCheckBoxColumn Check = new DataGridViewCheckBoxColumn();
                    Check.HeaderText = "Status";
                    Check.Name = "Status";
                    Check.ValueType = typeof(String);
                    Check.Visible = true;
                    Check.ReadOnly = false;
                    Grid_OCN.Columns.Insert(0, Check);
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
                    Grid_OCN.Columns.Insert(0, Check);
                    Status_Flag = true;
                }
                MyBase.Grid_Width(ref Grid_OCN, 70, 140);
                Grid_OCN.Columns["Status"].HeaderText = "STATUS";
                Grid_OCN.Columns["order_no"].HeaderText = "ORDER_NO";
                Grid_OCN.Columns["Status"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                Grid_OCN.RowHeadersWidth = 10;
                Grid_OCN.Focus();


                if (!MyParent._New)  // Default Checked
                {
                    for (int i = 0; i <= Dt_OCN.Rows.Count - 1; i++)
                    {
                        Grid_OCN["Status", i].Value = true;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        private void FrmSocksTrimsPOApproval_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                MyBase.Clear(this);
                CmbType.SelectedIndex = 0;
                ButApp.Text = CmbType.Text;
                TxtPONO.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmSocksTrimsPOApproval_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    SendKeys.Send("{Tab}");                    
                    return;
                    if (this.ActiveControl.Name == "TxtBuyer")
                    {
                        CmbBasedOn.Enabled = false;
                        if (CmbBasedOn.Text == "OCN WISE")
                        {
                            if (TxtBuyer.Text.Trim() == String.Empty)
                            {
                                MessageBox.Show("Invalid Buyer ...!", "Gainup");
                                return;
                            }
                            TxtBuyer.Enabled = false;
                            tabControl1.SelectTab(tabPage1);
                            if (Dt_OCN.Rows.Count == 0)
                            {
                                Load_OCN();
                            }
                        }
                        else
                        {
                            tabControl1.SelectTab(tabPage2);
                            if (Dt_Item.Rows.Count == 0)
                            {
                                Load_Item();
                            }
                            Grid_Item.CurrentCell = Grid_Item["Item", 0];
                            Grid_Item.Focus();
                            Grid_Item.BeginEdit(true);
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtTotal")
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
                    if (this.ActiveControl.Name == "TxtPONO")
                    {
                            Entry_Edit();
                            TxtPONO.Focus();
                    }
                    return;
                    if (this.ActiveControl.Name == "TxtBuyer")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer", "Select Party Buyer, Code From Accounts.Dbo.Debtors (" + MyParent.CompCode + ", '" + MyParent.YearCode + "')", String.Empty, 250);
                        if (Dr != null)
                        {
                            TxtBuyer.Text = Dr["Buyer"].ToString();
                            TxtBuyer.Tag = Dr["Code"].ToString();
                            /*
                            TxtBuyer.Enabled = false;
                            this.Cursor = Cursors.WaitCursor;
                            CmbBasedOn.Enabled = false;
                            if (CmbBasedOn.Text == "OCN WISE")
                            {
                                tabControl1.SelectTab(tabPage1);
                                Load_OCN();
                            }
                            else
                            {
                                tabControl1.SelectTab(tabPage2);
                                Load_Item();
                            }
                            this.Cursor = Cursors.Default;
                             */
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtSupplier")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "Select Party Supplier, Code From Accounts.Dbo.Creditors (" + MyParent.CompCode + ", '" + MyParent.YearCode + "')", String.Empty, 250);
                        if (Dr != null)
                        {
                            TxtSupplier.Text = Dr["Supplier"].ToString();
                            TxtSupplier.Tag = Dr["Code"].ToString();
                        }
                    }
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
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        void Load_Item()
        {
            try
            {
                //if (MyParent._New)
                //{
                //    Grid_Item.DataSource = MyBase.Load_Data("Select 0 as Sl, Size + ' ' + Item + ' ' + Color + ' @ ' + Cast(Rate as Varchar (15)) Description, Item_ID, Item ITEM, Color_ID, Color COLOR, Size_ID, size SIZE, Sum(BOM_Qty) BOM_Qty, Sum(Ordered_Qty) Ordered_Qty, Sum(PO_Qty) Bal_Qty, Sum(PO_Qty) PO_Qty, RATE ARATE, RATE, SUM(Amount) AMOUNT From SocksPO_Trims_Pending_Items (5275) where 1 = 2 Group by Item_ID, Item, Color_ID, Color, Size_ID, size, Rate", ref Dt_Item);
                //}
                //else
                //{
                    // Also Change in Load_Dt_Item_OCN for Edit
                    Grid_Item.DataSource = MyBase.Load_Data("Select 0 as Sl, S2.Size + ' ' + I1.Item + ' ' + C1.Color + ' @ ' + Cast(S1.Rate as Varchar (15)) Description, S1.Item_ID, I1.Item ITEM, S1.Color_ID, C1.Color COLOR, S1.Size_ID, S2.size SIZE, Sum(S1.BOM_Qty) BOM_Qty, (Sum(S1.Ordered_Qty) - Sum(S3.Order_Qty)) Ordered_Qty, (Sum(S1.BOM_Qty) - (Sum(S1.Ordered_Qty) - Sum(S3.Order_Qty))) Bal_Qty, Sum(S3.Order_Qty) PO_Qty, S1.RATE ARATE, S3.RATE, SUM(S3.Amount) AMOUNT From SocksPO_Trims_Pending_Items_All() S1 Inner Join Socks_Trims_PO_Details S3 on S1.Order_ID = S3.Order_ID and S1.Item_ID = S3.Item_id and S1.Color_ID = S3.Color_id and S1.Size_ID = S3.Size_ID  Inner Join item I1 on S1.Item_id = I1.itemid Inner Join color C1 on s1.Color_id = c1.colorid Inner Join size S2 on s1.Size_ID = S2.sizeid Where Master_ID = " + Code + " Group By S1.Item_ID, I1.Item, S1.Color_ID, C1.Color, S1.Size_ID, S2.size, S1.Rate, S3.Rate", ref Dt_Item);
                //}

                MyBase.Grid_Designing(ref Grid_Item, ref Dt_Item, "Item_ID", "Color_ID", "Size_ID", "Description", "ARATE");
                MyBase.ReadOnly_Grid_Without(ref Grid_Item, "Item", "PO_Qty", "RATE");
                MyBase.Grid_Colouring(ref Grid_Item, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid_Item, 30, 130, 90, 80, 110, 110, 80, 80, 90, 110);

                Grid_Item.Columns["BOM_QTY"].HeaderText = "BOM";
                Grid_Item.Columns["ORDERED_QTY"].HeaderText = "ORDERED";
                Grid_Item.Columns["BAL_QTY"].HeaderText = "BAL";
                Grid_Item.Columns["PO_QTY"].HeaderText = "PO";

                Grid_Item.Columns["BOM_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Item.Columns["ORDERED_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Item.Columns["BAL_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Item.Columns["PO_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Item.RowHeadersWidth = 10;

                MyBase.Row_Number(ref Grid_Item);

                Load_Tax();
                Calculate_Item_Amount_1();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmSocksTrimsPOApproval_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name != String.Empty)
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void myTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= Dt_OCN.Rows.Count - 1; i++)
                {
                    Grid_OCN["Status", i].Value = checkBox1.Checked;
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
                //if (MyParent._New)
                //{
                //    Grid_Tax.DataSource = MyBase.Load_Data("Select S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From Socks_Trims_Tax_Details S1 Left Join Accounts.dbo.Ledger_Master L1 on S1.Tax_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' Where 1 = 2 Order by S1.Slno ", ref Dt_Tax);
                //}
                //else
                //{
                    Grid_Tax.DataSource = MyBase.Load_Data("Select S1.Slno Sl, S1.Tax_Code, L1.Ledger_Name Tax, S1.Mode Tax_Mode, S1.Tax_Per, S1.Tax_Amount, '' T From Socks_Trims_Tax_Details S1 Left Join Accounts.dbo.Ledger_Master L1 on S1.Tax_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' Where S1.Master_ID = " + Code + " Order by S1.Slno ", ref Dt_Tax);
                //}
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

        Boolean Validate_Dt_and_DtVirtual()
        {
            try
            {

                if (Dt.Rows.Count == Dt_Virtual.Rows.Count && Dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Dt.Rows[i]["Item_ID"].ToString() == Dt_Virtual.Rows[i]["Item_ID"].ToString() && Dt.Rows[i]["Color_ID"].ToString() == Dt_Virtual.Rows[i]["Color_ID"].ToString() && Dt.Rows[i]["Size_ID"].ToString() == Dt_Virtual.Rows[i]["Size_ID"].ToString() && (Convert.ToDouble(Dt.Rows[i]["ARate"]) >= Convert.ToDouble(Dt_Virtual.Rows[i]["Rate"]) || Convert.ToDouble(Dt.Rows[i]["Rate"]) >= Convert.ToDouble(Dt_Virtual.Rows[i]["Rate"])))
                        {
                        }
                        else
                        {
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        void Load_Pivot_OCN(String OCN_List)
        {
            try
            {
                
                this.Cursor = Cursors.WaitCursor;
                Grid_OCN.Enabled = false;
                checkBox1.Enabled = false;
                Dt = new DataTable();
                Dt_Virtual = new DataTable();
                //if (MyParent._New)
                //{
                //    // For Virutal - OCN No Reference 
                //    MyBase.Load_Data("Socks_Trims_PO_Virutal_For_OCN_Splitup " + TxtBuyer.Tag.ToString() + ", '" + OCN_List + "'", ref Dt_Virtual);
                //    MyBase.Load_Data("Select 0 as Sl, Item_ID, Item ITEM, Color_ID, Color COLOR, Size_ID, Size SIZE, Sum(BOM_Qty) BOM_Qty, Sum(Ordered_Qty) Ordered_Qty, Sum(PO_Qty) Bal_Qty, Sum(PO_Qty) PO_Qty, Rate ARATE, Rate RATE, Sum(Amount) AMOUNT From SocksPO_Trims_Pending_Items (" + TxtBuyer.Tag.ToString() + ") Where Order_ID in (" + OCN_List + ") Group By Item_ID, Item, Color_ID, Color, Size_ID, Size, Rate Having Sum(PO_Qty) > 0 Order By Item, Color, Size, Rate", ref Dt);
                //}
                //else
                //{
                    // For Virutal - OCN No Reference 
                    MyBase.Load_Data("Socks_Trims_PO_Virutal_For_OCN_Splitup_Edit " + TxtBuyer.Tag.ToString() + ", " + Code, ref Dt_Virtual);
                    MyBase.Load_Data("Select 0 as Sl, S1.Item_ID, S1.Item ITEM, S1.Color_ID, S1.Color COLOR, S1.Size_ID, S1.Size SIZE, Sum(S1.BOM_Qty) BOM_Qty, (Sum(S1.Ordered_Qty) - (Case when S1.Color_ID = 867 then (Case When Sum(S2.order_Qty) > Sum(S1.Ordered_Qty) then Sum(S1.Ordered_Qty) else Sum(S2.order_Qty) End) else Sum(S2.order_Qty) end)) Ordered_Qty, (Sum(S1.PO_Qty) + (Case when S1.Color_ID = 867 then (Case When Sum(S2.order_Qty) > Sum(S1.Ordered_Qty) then Sum(S1.Ordered_Qty) else Sum(S2.order_Qty) End) else Sum(S2.order_Qty) end)) Bal_Qty, (Sum(S1.PO_Qty) + (Case when S1.Color_ID = 867 then (Case When Sum(S2.order_Qty) > Sum(S1.Ordered_Qty) then Sum(S1.Ordered_Qty) else Sum(S2.order_Qty) End) else Sum(S2.order_Qty) end)) PO_Qty, S1.Rate ARATE, S2.Rate RATE, (S2.Rate * (Sum(S1.PO_Qty) + (Case when S1.Color_ID = 867 then (Case When Sum(S2.order_Qty) > Sum(S1.Ordered_Qty) then Sum(S1.Ordered_Qty) else Sum(S2.order_Qty) End) else Sum(S2.order_Qty) end))) AMOUNT From SocksPO_Trims_Pending_Items (" + TxtBuyer.Tag.ToString() + ") S1 Inner Join (Select Master_ID, Order_ID, Item_ID, Size_ID, Color_ID, Rate, Sum(Order_Qty) Order_Qty From Socks_Trims_PO_Details Group By Master_ID, Order_ID, Item_ID, Color_ID, Size_ID, Rate) S2 on S1.Order_ID = S2.Order_ID and S1.Item_ID = S2.Item_id and S1.Color_ID = S2.Color_id and S1.Size_ID = S2.Size_ID Where S2.Master_ID = " + Code + " Group By S1.Item_ID, S1.Item, S1.Color_ID, S1.Color, S1.Size_ID, S1.Size, S1.Rate, S2.Rate Order by S1.Item, S1.Color, S1.Size, S2.Rate", ref Dt);
                //}
                if (!Validate_Dt_and_DtVirtual ())
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Contact IT. Illegal List ...!", "Gainup");
                    return;
                }
                Grid.DataSource = Dt;
                MyBase.Grid_Designing(ref Grid, ref Dt, "Item_ID", "Color_ID", "Size_ID", "ARATE");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Rate");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 40, 120, 130, 70, 90, 90, 90, 90, 90, 110);
                Grid.Columns["BOM_Qty"].HeaderText = "BOM";
                Grid.Columns["Ordered_Qty"].HeaderText = "ORDERED";
                Grid.Columns["Bal_Qty"].HeaderText = "BAL";
                Grid.Columns["PO_Qty"].HeaderText = "PO";
                Grid.Columns["BOM_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Ordered_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Bal_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["PO_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                MyBase.Row_Number(ref Grid);
                Grid.RowHeadersWidth = 10;
                if (Dt_Tax.Rows.Count == 0)
                {
                    Load_Tax();
                }
                Calculate_Item_Amount();
                Grid.CurrentCell = Grid["Rate", 0];
                Grid.Focus();
                Grid.BeginEdit(true);
                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String OCN_List = String.Empty;
            try
            {
                return;
                if (Dt_OCN == null)
                {
                    MessageBox.Show("Invalid Data's ...!", "Gainup");
                    return;
                }

                for (int i = 0; i <= Dt_OCN.Rows.Count - 1; i++)
                {
                    if (Grid_OCN["Status", i].Value != null && Grid_OCN["Status", i].Value != DBNull.Value && Grid_OCN["Status", i].Value.ToString().ToUpper() == "true".ToUpper())
                    {
                        if (OCN_List == String.Empty)
                        {
                            OCN_List = Grid_OCN["RowID", i].Value.ToString();
                        }
                        else
                        {
                            OCN_List += "," + Grid_OCN["RowID", i].Value.ToString();
                        }
                    }
                }

                if (OCN_List == String.Empty)
                {
                    MessageBox.Show("Invalid OCN List ...!", "Gainup");
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    if (MessageBox.Show("Sure to Clear existing Details ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                Load_Pivot_OCN (OCN_List);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_OCN_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void myTextBox2_TextChanged(object sender, EventArgs e)
        {

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
                        Grid_Tax["Tax_Amount", Grid_Tax.CurrentCell.RowIndex].Value = ((Convert.ToDouble(TxtAmount.Text)/100) * Convert.ToDouble(Grid_Tax["Tax_Per", Grid_Tax.CurrentCell.RowIndex].Value));
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
                 e.Handled = true;
                return;
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
                return;
                if (e.KeyCode == Keys.Down && Grid_Tax.CurrentCell.ColumnIndex == Grid_Tax.Columns["Tax"].Index)
                {
                    e.Handled = true;
                    Dr = Tool.Selection_Tool_Except_New("Tax_Code", this, 30, 70, ref Dt_Tax, SelectionTool_Class.ViewType.NormalView, "Select Tax", "Select Ledger_Name Tax, Ledger_Code Tax_Code From Accounts.Dbo.Tax_Accounts(" + MyParent.CompCode + ", '" + MyParent.YearCode + "')", String.Empty, 250);
                    if (Dr != null)
                    {
                        MyBase.Row_Number (ref Grid_Tax);
                        Grid_Tax["Tax", Grid_Tax.CurrentCell.RowIndex].Value = Dr["Tax"].ToString();
                        Grid_Tax["Tax_Code", Grid_Tax.CurrentCell.RowIndex].Value = Dr["Tax_Code"].ToString();
                        Txt_Tax.Text = Dr["Tax"].ToString();

                        DataTable Tdt = new DataTable();
                        MyBase.Load_Data("Select Accounts.Dbo.Get_Tax_Per (" + Dr["Tax_Code"].ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", MyBase.GetServerDate()) + "')", ref Tdt);
                        if (Convert.ToDouble(Tdt.Rows[0][0]) > 0)
                        {
                            Grid_Tax["Tax_Mode", Grid_Tax.CurrentCell.RowIndex].Value = "Y";
                            Grid_Tax["Tax_Per", Grid_Tax.CurrentCell.RowIndex].Value = Convert.ToDouble(Tdt.Rows[0][0]);
                            Grid_Tax["Tax_Amount", Grid_Tax.CurrentCell.RowIndex].Value = Convert.ToDouble(String.Format ("{0:0}", (Convert.ToDouble(TxtAmount.Text) / 100) * Convert.ToDouble(Tdt.Rows[0][0])));
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_Qty"].Index)
                {
                    if (Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value == null || Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                    }

                    Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value = String.Format("{0:0.000}", Convert.ToDouble(Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value));
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
                        Grid_Tax["Tax_Amount", i].Value = String.Format ("{0:0}", Convert.ToDouble(Grid_Tax["Tax_Per", i].Value) * (Convert.ToDouble(TxtAmount.Text) / 100));
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

                TxtQTY.Text = String.Format ("{0:0.000}", Convert.ToDouble(MyBase.Sum(ref Grid, "PO_Qty", "Item_ID", "Size_ID", "Color_ID")));
                TxtAmount.Text = String.Format ("{0:n}", Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "Amount", "Item_ID", "Size_ID", "Color_ID")))));

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
                for (int i = 0; i <= Dt_Item.Rows.Count - 1; i++)
                {
                    if (Grid_Item["PO_Qty", i].Value == null || Grid_Item["PO_Qty", i].Value == DBNull.Value || Grid_Item["PO_Qty", i].Value.ToString() == String.Empty)
                    {
                        Grid_Item["PO_Qty", i].Value = "0.000";
                    }

                    if (Convert.ToDouble(Grid_Item["PO_Qty", i].Value) > Convert.ToDouble(Grid_Item["Bal_Qty", i].Value))
                    {
                        MessageBox.Show("PO Qty is greater than Balance ...!", "Gainup");
                        Grid_Item["PO_Qty", i].Value = Grid_Item["Bal_Qty", i].Value;
                        Grid_Item.CurrentCell = Grid_Item["PO_Qty", i];
                        Grid_Item.Focus();
                        Grid_Item.BeginEdit(true);
                        return false;
                    }

                    if (Grid_Item["RATE", i].Value == null || Grid_Item["RATE", i].Value == DBNull.Value || Grid_Item["RATE", i].Value.ToString() == String.Empty)
                    {
                        Grid_Item["RATE", i].Value = "0.000";
                    }

                    if (Convert.ToDouble(Grid_Item["RATE", i].Value) > Convert.ToDouble(Grid_Item["ARATE", i].Value))
                    {
                        MessageBox.Show("RATE is greater than APPROVED [" + Grid_Item["ARate", i].Value.ToString() + "] ...!", "Gainup");
                        Grid_Item["RATE", i].Value = Grid_Item["ARATE", i].Value;
                        Grid_Item.CurrentCell = Grid_Item["RATE", i];
                        Grid_Item.Focus();
                        Grid_Item.BeginEdit(true);
                        return false;
                    }


                    Grid_Item["PO_Qty", i].Value = String.Format("{0:0.000}", Convert.ToDouble(Grid_Item["PO_Qty", i].Value));

                    Grid_Item["Amount", i].Value = Convert.ToDouble(Grid_Item["PO_Qty", i].Value) * Convert.ToDouble(Grid_Item["Rate", i].Value);
                }

                TxtQTY.Text = String.Format ("{0:0.000}", Convert.ToDouble(MyBase.Sum(ref Grid_Item, "PO_Qty", "Item_ID", "Size_ID", "Color_ID")));
                TxtAmount.Text = String.Format ("{0:0.00}", Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid_Item, "Amount", "Item_ID", "Size_ID", "Color_ID")))));

                Refresh_Tax();

                TxtTotal.Text = String.Format("{0:n}", Convert.ToDouble(TxtAmount.Text) + Convert.ToDouble(MyBase.Sum(ref Grid_Tax, "Tax_Amount", "Tax_Code", "Tax")));

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                 e.Handled = true;
                return;
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_Qty"].Index)
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
                return;
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_Qty"].Index)
                    {
                        if (Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value == null || Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid PO Qty ...!", "Gainup");
                            Grid.CurrentCell = Grid["PO_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                        }
                        else
                        {
                            if (Convert.ToDouble(Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value))
                            {
                                e.Handled = true;
                                MessageBox.Show("PO Qty is greater than Balance ...!", "Gainup");
                                Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value = Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value;
                                Grid.CurrentCell = Grid["PO_Qty", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                            }
                            else
                            {
                                if (!Calculate_Item_Amount())
                                {
                                    e.Handled = true;
                                }
                            }
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index)
                    {
                        if (Grid["Rate", Grid.CurrentCell.RowIndex].Value == null || Grid["Rate", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Rate", Grid.CurrentCell.RowIndex].Value = "0.00";
                        }

                        if (Convert.ToDouble(Grid["Rate", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["ARate", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Rate is greater than Approved [" + Grid["ARate", Grid.CurrentCell.RowIndex].Value.ToString() + "] ...!", "Gainup");
                            Grid["Rate", Grid.CurrentCell.RowIndex].Value = Grid["ARate", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["Rate", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                        }

                        Calculate_Item_Amount();
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    e.Handled = true;
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

        private void Grid_Tax_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                return;
                if (e.KeyChar == Convert.ToChar (Keys.Escape))
                {
                    if (CmbBasedOn.Text == "OCN WISE")
                    {
                        Calculate_Item_Amount();
                    }
                    else
                    {
                        Calculate_Item_Amount_1();
                    }
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

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                return;
                if (Grid.CurrentCell != null)
                {
                    Int32 Position = Grid.CurrentCell.RowIndex;
                    if (Position <= Dt.Rows.Count)
                    {
                        if (MessageBox.Show("Sure to Delete this ?", "Vaahini", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            Dt.Rows.RemoveAt(Position);
                            Dt_Virtual.Rows[Position].Delete();
                            Dt_Virtual.AcceptChanges();
                            MyBase.Row_Number(ref Grid);
                            Calculate_Item_Amount();
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
                return;
                MyBase.Grid_Delete(ref Grid_Tax, ref Dt_Tax, Grid_Tax.CurrentCell.RowIndex);
                Calculate_Item_Amount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Grid_Item_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Item == null)
                {
                    Txt_Item = (TextBox)e.Control;
                    Txt_Item.KeyDown += new KeyEventHandler(Txt_Item_KeyDown);
                    Txt_Item.KeyPress += new KeyPressEventHandler(Txt_Item_KeyPress);
                    //Txt_Item.Leave += new EventHandler(Txt_Item_Leave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Item_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["PO_Qty"].Index)
                {
                    if (Txt_Item.Text.Trim() == String.Empty)
                    {
                        Txt_Item.Text = "0.000";
                    }

                    Txt_Item.Text = String.Format("{0:0.000}", Convert.ToDouble(Txt_Item.Text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Item_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                 e.Handled = true;
                return;
                if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["PO_Qty"].Index || Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["RATE"].Index)
                {
                    MyBase.Valid_Decimal(Txt_Item, e);
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

        void Txt_Item_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                return;
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["Item"].Index)
                    {
                        e.Handled = true;
                        if (TxtBuyer.Text.Trim() == String.Empty)
                        {
                            Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt_Item, SelectionTool_Class.ViewType.NormalView, "Select Item", "Select S1.Size + ' ' + S1.Item + ' ' + S1.Color + ' @ ' + Cast(S1.Rate as Varchar (15)) Description, Sum(S1.BOM_Qty) Bom_Qty, Sum(S1.Ordered_Qty) Ordered_Qty, Sum(S1.PO_Qty) PO_Qty, S1.Rate, SUM(S1.Amount) Amount, S1.Item_ID, S1.Item, S1.Color_ID, S1.Color, S1.Size_ID, S1.size From SocksPO_Trims_Pending_Items_All () S1 LEft join Socks_Time_Action_PO_Complete_Orders () S2 on S1.Order_No = S2.Order_No and S2.Complete_Date_End <= Cast(GetDate() as Date) Where  S1.Color_ID <> 867 Group by S1.Item_ID, S1.Item, S1.Color_ID, S1.Color, S1.Size_ID, S1.size, S1.Rate Having Sum(S1.PO_Qty) > 0 ", String.Empty, 250, 80, 80, 80);
                        }
                        else
                        {
                            Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt_Item, SelectionTool_Class.ViewType.NormalView, "Select Item", "Select S1.Size + ' ' + S1.Item + ' ' + S1.Color + ' @ ' + Cast(S1.Rate as Varchar (15)) Description, Sum(S1.BOM_Qty) Bom_Qty, Sum(S1.Ordered_Qty) Ordered_Qty, Sum(S1.PO_Qty) PO_Qty, S1.Rate, SUM(S1.Amount) Amount, S1.Item_ID, S1.Item, S1.Color_ID, S1.Color, S1.Size_ID, S1.size From SocksPO_Trims_Pending_Items (" + TxtBuyer.Tag.ToString() + ") S1 Left join Socks_Time_Action_PO_Complete_Orders () S2 on S1.Order_No = S2.Order_No and S2.Complete_Date_End <= Cast(GetDate() as Date) Where  S1.Color_ID <> 867 Group by S1.Item_ID, S1.Item, S1.Color_ID, S1.Color, S1.Size_ID, S1.size, S1.Rate Having Sum(S1.PO_Qty) > 0 ", String.Empty, 250, 80, 80, 80);
                        }
                        if (Dr != null)
                        {
                            MyBase.Row_Number(ref Grid_Item);

                            Grid_Item["Description", Grid_Item.CurrentCell.RowIndex].Value = Dr["Description"].ToString();
                            Grid_Item["Item_ID", Grid_Item.CurrentCell.RowIndex].Value = Dr["Item_ID"].ToString();
                            Grid_Item["Size_ID", Grid_Item.CurrentCell.RowIndex].Value = Dr["Size_ID"].ToString();
                            Grid_Item["Color_ID", Grid_Item.CurrentCell.RowIndex].Value = Dr["Color_ID"].ToString();

                            Txt_Item.Text = Dr["Item"].ToString();
                            Grid_Item["Item", Grid_Item.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                            Grid_Item["Size", Grid_Item.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                            Grid_Item["Color", Grid_Item.CurrentCell.RowIndex].Value = Dr["Color"].ToString();

                            Grid_Item["BOM_Qty", Grid_Item.CurrentCell.RowIndex].Value = String.Format("{0:0.000}", Convert.ToDouble(Dr["BOM_Qty"]));
                            Grid_Item["ORDERED_Qty", Grid_Item.CurrentCell.RowIndex].Value = String.Format("{0:0.000}", Convert.ToDouble(Dr["ORDERED_Qty"]));
                            Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value = String.Format("{0:0.000}", Convert.ToDouble(Dr["PO_Qty"]));
                            Grid_Item["Bal_Qty", Grid_Item.CurrentCell.RowIndex].Value = String.Format("{0:0.000}", Convert.ToDouble(Dr["PO_Qty"]));
                            Grid_Item["ARATE", Grid_Item.CurrentCell.RowIndex].Value = String.Format("{0:0.00}", Convert.ToDouble(Dr["Rate"]));
                            Grid_Item["Rate", Grid_Item.CurrentCell.RowIndex].Value = String.Format("{0:0.00}", Convert.ToDouble(Dr["Rate"]));

                            Grid_Item["Amount", Grid_Item.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["PO_Qty"]) * Convert.ToDouble(Dr["Rate"]);

                            Calculate_Item_Amount_1();

                            load_grid_item_ocn(Grid_Item.CurrentCell.RowIndex);
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



        private void Grid_Item_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                return;
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["PO_Qty"].Index)
                    {
                        if (Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value == null || Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value = "0.000";
                        }
                        else
                        {
                            if (Convert.ToDouble(Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid_Item["Bal_Qty", Grid_Item.CurrentCell.RowIndex].Value))
                            {
                                e.Handled = true;
                                MessageBox.Show("PO Qty is greater than Balance ...!", "Gainup");
                                Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value = Grid_Item["Bal_Qty", Grid_Item.CurrentCell.RowIndex].Value;
                                Grid_Item.CurrentCell = Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex];
                                Grid_Item.Focus();
                                Grid_Item.BeginEdit(true);
                                return;
                            }

                            Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value = String.Format("{0:0.000}", Convert.ToDouble(Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value));
                            Grid_Item["Amount", Grid_Item.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid_Item["Rate", Grid_Item.CurrentCell.RowIndex].Value);

                            if (!Calculate_Item_Amount_1())
                            {
                                e.Handled = true;
                            }
                            else
                            {
                                Fill();
                            }
                        }
                    }
                    else if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["RATE"].Index)
                    {
                        if (Grid_Item["RATE", Grid_Item.CurrentCell.RowIndex].Value == null || Grid_Item["RATE", Grid_Item.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Item["RATE", Grid_Item.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid_Item["RATE", Grid_Item.CurrentCell.RowIndex].Value = "0.000";
                        }
                        else
                        {
                            if (Convert.ToDouble(Grid_Item["RATE", Grid_Item.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid_Item["ARATE", Grid_Item.CurrentCell.RowIndex].Value))
                            {
                                e.Handled = true;
                                MessageBox.Show("RATE is greater than APPROVED [" + Grid_Item["ARATE", Grid_Item.CurrentCell.RowIndex].Value.ToString() + "] ...!", "Gainup");
                                Grid_Item["RATE", Grid_Item.CurrentCell.RowIndex].Value = Grid_Item["ARATE", Grid_Item.CurrentCell.RowIndex].Value;
                                Grid_Item.CurrentCell = Grid_Item["RATE", Grid_Item.CurrentCell.RowIndex];
                                Grid_Item.Focus();
                                Grid_Item.BeginEdit(true);
                                return;
                            }

                            Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value = String.Format("{0:0.000}", Convert.ToDouble(Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value));
                            Grid_Item["Amount", Grid_Item.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid_Item["Rate", Grid_Item.CurrentCell.RowIndex].Value);

                            if (!Calculate_Item_Amount_1())
                            {
                                e.Handled = true;
                            }
                            else
                            {
                                Fill();
                                if (MyParent.UserName.ToUpper() == "MD")
                                {
                                    e.Handled = true;
                                    Grid_Item_OCN.CurrentCell = Grid_Item_OCN["PO_Qty", 0];
                                    Grid_Item_OCN.Focus();
                                    Grid_Item_OCN.BeginEdit(true);
                                    return;
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

        private void Grid_Item_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                return;
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    e.Handled = true;
                    Calculate_Item_Amount_1();
                    Grid_Tax.CurrentCell = Grid_Tax["Tax", 0];
                    Grid_Tax.Focus();
                    Grid_Tax.BeginEdit(true);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Item_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                return;
                Dt_Item_OCN[Grid_Item.CurrentCell.RowIndex] = null;
                MyBase.Grid_Delete(ref Grid_Item, ref Dt_Item, Grid_Item.CurrentCell.RowIndex);
                Calculate_Item_Amount_1();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Fill()
        {
            double Qty = 0;
            try
            {
                if (Grid_Item.CurrentCell == null)
                {
                    MessageBox.Show("Invalid Qty for ITEM ...!", "Gainup");
                    return;
                }

                Qty = Convert.ToDouble(Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value);

                if (Qty == Convert.ToDouble(MyBase.Sum(ref Grid_Item_OCN, "PO_Qty", "Order_ID", "BOM_Qty")))
                {
                    return;
                }

                for (int i = 0; i <= Grid_Item_OCN.Rows.Count - 1; i++)
                {
                    if (Qty > 0)
                    {
                        if (Convert.ToDouble(Grid_Item_OCN["Bal_Qty", i].Value) >= Qty)
                        {
                            Grid_Item_OCN["PO_Qty", i].Value = String.Format("{0:0.000}", Convert.ToDouble(Qty));
                            Qty = 0;
                        }
                        else
                        {
                            Grid_Item_OCN["PO_Qty", i].Value = Grid_Item_OCN["Bal_Qty", i].Value;
                            Qty -= Convert.ToDouble(Grid_Item_OCN["Bal_Qty", i].Value);
                        }
                    }
                    else
                    {
                        Grid_Item_OCN["PO_Qty", i].Value = "0.000";
                    }
                }
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
                Fill();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Load_Dt_Item_OCN()
        {
            DataTable TempDt = new DataTable();
            try
            {
                MyBase.Load_Data("Select 0 as Sl, S2.Size + ' ' + I1.Item + ' ' + C1.Color + ' @ ' + Cast(S1.Rate as Varchar (15)) Description, S1.Item_ID, I1.Item ITEM, S1.Color_ID, C1.Color COLOR, S1.Size_ID, S2.size SIZE, Sum(S1.BOM_Qty) BOM_Qty, (Sum(S1.Ordered_Qty) - Sum(S3.Order_Qty)) Ordered_Qty, (Sum(S1.BOM_Qty) - (Sum(S1.Ordered_Qty) - Sum(S3.Order_Qty))) Bal_Qty, Sum(S3.Order_Qty) PO_Qty, S1.RATE ARATE, S3.RATE, SUM(S3.Amount) AMOUNT From SocksPO_Trims_Pending_Items_All() S1 Inner Join Socks_Trims_PO_Details S3 on S1.Order_ID = S3.Order_ID and S1.Item_ID = S3.Item_id and S1.Color_ID = S3.Color_id and S1.Size_ID = S3.Size_ID  Inner Join item I1 on S1.Item_id = I1.itemid Inner Join color C1 on s1.Color_id = c1.colorid Inner Join size S2 on s1.Size_ID = S2.sizeid Where Master_ID = " + Code + " Group By S1.Item_ID, I1.Item, S1.Color_ID, C1.Color, S1.Size_ID, S2.size, S1.Rate, S3.Rate", ref TempDt);
                for (int i = 0; i <= TempDt.Rows.Count - 1; i++)
                {
                    Dt_Item_OCN[i] = new DataTable();
                    MyBase.Load_Data("Select 0 as Sl, S1.Order_ID, S2.Order_No ORDER_NO, L1.Ledger_Name BUYER, S1.BOM_Qty, (S1.Ordered_Qty - S3.Order_Qty) Ordered_Qty, (S1.BOM_Qty - (S1.Ordered_Qty - S3.Order_Qty)) Bal_Qty, S3.Order_Qty PO_Qty, '' T From SocksPO_Trims_Pending_Items_All() S1 Inner Join Socks_Order_Master S2 on S1.Order_ID = S2.RowID Inner Join Socks_Trims_PO_Details S3 On S1.Order_ID = S3.Order_ID and  S1.Item_ID = S3.Item_id and S1.Color_ID = S3.Color_id and S1.Size_ID = S3.Size_ID Left Join Accounts.Dbo.Ledger_Master L1 on S2.Party_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' Where S3.Master_ID = " + Code + " and S1.Item_ID = " + TempDt.Rows[i]["Item_ID"].ToString() + " And S1.Color_ID = " + TempDt.Rows[i]["Color_ID"].ToString() + " and S1.size_ID = " + TempDt.Rows[i]["Size_ID"].ToString() + " and S3.Rate = " + TempDt.Rows[i]["Rate"].ToString() + " Order by S2.Order_No", ref Dt_Item_OCN[i]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void load_grid_item_ocn(Int32 Parent_Row)
        {
            Boolean Fill_Flag = false;
            try
            {

                if (Dt_Item.Rows.Count == 0 && Grid_Item["Item_ID", 0].Value == DBNull.Value)
                {
                    return;
                }

                if (Dt_Item_OCN[Parent_Row] == null)
                {
                    Fill_Flag = true;
                    Dt_Item_OCN[Parent_Row] = new DataTable();
                    if (TxtBuyer.Tag.ToString() == String.Empty || Convert.ToInt32(TxtBuyer.Tag) == 0)
                    {
                        MyBase.Load_Data("Select 0 as Sl, S1.Order_ID, S2.Order_No ORDER_NO, L1.Ledger_Name BUYER, S1.Bom_Qty, S1.Ordered_Qty, S1.PO_Qty Bal_Qty, Cast(0 as Numeric (25, 3)) As Po_Qty, '' T From SocksPO_Trims_Pending_Items_All() S1 Inner join Socks_Order_Master S2 on S1.Order_ID = s2.RowID Left join Accounts.Dbo.ledger_Master L1 on S2.Party_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "' Left Join Socks_Time_Action_PO_Complete_Orders () S5 on S2.Order_No = S5.Order_No and S5.Complete_Date_End <= Cast(GetDate() as Date) Where  S1.Item_ID = " + Grid_Item["Item_ID", Parent_Row].Value.ToString() + " and S1.Color_ID = " + Grid_Item["Color_ID", Parent_Row].Value.ToString() + " and S1.size_ID = " + Grid_Item["Size_ID", Parent_Row].Value.ToString() + " and Rate = " + Grid_Item["ARATE", Parent_Row].Value.ToString() + " order by S2.Order_No", ref Dt_Item_OCN[Parent_Row]);
                    }
                    else
                    {
                        MyBase.Load_Data("Select 0 as Sl, S1.Order_ID, S2.Order_No ORDER_NO, L1.Ledger_Name BUYER, S1.Bom_Qty, S1.Ordered_Qty, S1.PO_Qty Bal_Qty, Cast(0 as Numeric (25, 3)) As Po_Qty, '' T From SocksPO_Trims_Pending_Items(" + TxtBuyer.Tag.ToString() + ") S1 Inner join Socks_Order_Master S2 on S1.Order_ID = s2.RowID Left join Accounts.Dbo.ledger_Master L1 on S2.Party_Code = L1.Ledger_Code and L1.COMPANY_CODE = " + MyParent.CompCode + " and L1.YEAR_CODE = '" + MyParent.YearCode + "'  Left Join Socks_Time_Action_PO_Complete_Orders () S5 on S2.Order_No = S5.Order_No and S5.Complete_Date_End <= Cast(GetDate() as Date) Where  S1.Item_ID = " + Grid_Item["Item_ID", Parent_Row].Value.ToString() + " and S1.Color_ID = " + Grid_Item["Color_ID", Parent_Row].Value.ToString() + " and S1.size_ID = " + Grid_Item["Size_ID", Parent_Row].Value.ToString() + " and Rate = " + Grid_Item["ARATE", Parent_Row].Value.ToString() + " order by S2.Order_No", ref Dt_Item_OCN[Parent_Row]);
                        TxtBuyer.Enabled = false;
                    }
                }

                Grid_Item_OCN.DataSource = Dt_Item_OCN[Grid_Item.CurrentCell.RowIndex];
                MyBase.Grid_Designing(ref Grid_Item_OCN, ref Dt_Item_OCN[Parent_Row], "Order_ID", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid_Item_OCN, "PO_Qty");

                Grid_Item_OCN.Columns["Bom_Qty"].HeaderText = "BOM";
                Grid_Item_OCN.Columns["ORDERED_Qty"].HeaderText = "ORDERED";
                Grid_Item_OCN.Columns["bal_Qty"].HeaderText = "BAL";
                Grid_Item_OCN.Columns["po_Qty"].HeaderText = "PO";

                Grid_Item_OCN.Columns["Bom_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Item_OCN.Columns["ORDERED_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Item_OCN.Columns["Bal_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Item_OCN.Columns["Po_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                MyBase.Grid_Colouring(ref Grid_Item_OCN, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid_Item_OCN, 40, 110, 200, 90, 90, 90, 90);
                Grid_Item_OCN.RowHeadersWidth = 10;
                MyBase.Row_Number(ref Grid_Item_OCN);

                if (Fill_Flag)
                {
                    Fill();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Item_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Item.CurrentCell != null && Grid_Item.CurrentCell.Value != DBNull.Value)
                {
                    load_grid_item_ocn(Grid_Item.CurrentCell.RowIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                if (CmbBasedOn.Text == "OCN WISE")
                {
                    if (e.TabPage == tabPage2)
                    {
                        e.Cancel = true;
                    }
                }
                else if (CmbBasedOn.Text == "ITEM WISE")
                {
                    if (e.TabPage == tabPage1)
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Item_OCN_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_OCN == null)
                {
                    Txt_OCN = (TextBox)e.Control;
                    Txt_OCN.KeyDown += new KeyEventHandler(Txt_OCN_KeyDown);
                    Txt_OCN.KeyPress += new KeyPressEventHandler(Txt_OCN_KeyPress);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_OCN_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                 e.Handled = true;
                return;
                if (Grid_Item_OCN.CurrentCell.ColumnIndex == Grid_Item_OCN.Columns["PO_Qty"].Index)
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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void Grid_Item_OCN_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                return;
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        }

        Boolean Verify_OCN_Qty(Int32 Grid_Item_Current_Row)
        {
            try
            {
                Double Qty = Convert.ToDouble(Grid_Item ["PO_Qty", Grid_Item_Current_Row].Value);
                if (Convert.ToDouble(MyBase.Sum_Trible(ref Dt_Item_OCN[Grid_Item_Current_Row], "PO_Qty", true)) == Qty)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void Grid_Item_OCN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                return;
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid_Item_OCN.CurrentCell.ColumnIndex == Grid_Item_OCN.Columns["PO_Qty"].Index)
                    {
                        if (Grid_Item_OCN["PO_Qty", Grid_Item_OCN.CurrentCell.RowIndex].Value == null || Grid_Item_OCN["PO_Qty", Grid_Item_OCN.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Item_OCN["PO_Qty", Grid_Item_OCN.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid_Item_OCN["PO_Qty", Grid_Item_OCN.CurrentCell.RowIndex].Value = "0.000";
                        }

                        if (Convert.ToDouble(Grid_Item_OCN["PO_Qty", Grid_Item_OCN.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid_Item_OCN["Bal_Qty", Grid_Item_OCN.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show ("Invalid Qty ...!", "Gainup");
                            Grid_Item_OCN["PO_Qty", Grid_Item_OCN.CurrentCell.RowIndex].Value = Grid_Item_OCN["Bal_Qty", Grid_Item_OCN.CurrentCell.RowIndex].Value;
                            Grid_Item_OCN.CurrentCell = Grid_Item_OCN["PO_Qty", Grid_Item_OCN.CurrentCell.RowIndex];
                            Grid_Item_OCN.Focus();
                            Grid_Item_OCN.BeginEdit (true);
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

        private void ButApp_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtSupplier.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Supplier ...!", "Gainup");
                    TxtPONO.Focus();                    
                    return;
                }

                if (CmbBasedOn.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Mode ...!", "Gainup");
                    TxtPONO.Focus();                    
                    return;
                }

                if (CmbBasedOn.Text == "OCN WISE")
                {                    
                    Calculate_Item_Amount();

                    if (TxtBuyer.Text.Trim() == String.Empty)
                    {
                        MessageBox.Show("Invalid Buyer ...!", "Gainup");
                        TxtPONO.Focus();                                            
                        return;
                    }

                    if (Dt.Rows.Count == 0 || Dt_OCN.Rows.Count == 0)
                    {
                        MessageBox.Show("Invalid Buyer ...!", "Gainup");
                        TxtPONO.Focus();                                            
                        return;
                    }

                    if (!Validate_Dt_and_DtVirtual())
                    {
                        MessageBox.Show("Invalid OCN List [Virtual Comparision]. Contact IT ...!", "Gainup");                        
                        TxtPONO.Focus();                    
                        return;
                    }
                }
                else
                {
                    Calculate_Item_Amount_1();

                    if (Dt_Item.Rows.Count == 0)
                    {
                        MessageBox.Show("Invalid Details ...!", "Gainup");
                        TxtPONO.Focus();                                            
                        return;
                    }


                    for (int i = 0; i <= Dt_Item.Rows.Count - 1; i++)
                    {
                        if (!Verify_OCN_Qty(i))
                        {
                            MessageBox.Show("Invalid OCN Wise Qty Details ...!", "Gainup");
                            TxtPONO.Focus();                    
                            return;
                        }
                    }

                    if (TxtBuyer.Text.Trim() == String.Empty)
                    {
                        TxtBuyer.Tag = "0";
                    }
                }

                if (!Valid_Tax())
                {                    
                    return;
                }

                if (TxtQTY.Text.Trim() == String.Empty || Convert.ToDouble(TxtQTY.Text) == 0)
                {
                    MessageBox.Show("Invalid Qty ...!", "Gainup");
                    TxtPONO.Focus();                    
                    return;
                }

                if (TxtAmount.Text.Trim() == String.Empty || Convert.ToDouble(TxtAmount.Text) == 0)
                {
                    MessageBox.Show("Invalid Amount ...!", "Gainup");
                    TxtPONO.Focus();                                        
                    return;
                }

                
                if (MessageBox.Show ("Sure to " + CmbType.Text + " ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }

                if(CmbType.SelectedIndex == 1)
                {
                    MyBase.Run("Update Socks_Trims_PO_Master Set Approval_Flag = 'F' , Approval_System = Host_Name(), Approval_Time = Getdate() Where RowID = " + Code + " ");
                }
                else
                {
                    MyBase.Run("Update Socks_Trims_PO_Master Set Approval_Flag = 'T' , Approval_System = Host_Name(), Approval_Time = Getdate() Where RowID = " + Code + " ");
                }
                MessageBox.Show("" + CmbType.Text + " ...!", "Gainup");                
                MyBase.Clear(this);
                CmbType.SelectedIndex = 0;
                ButApp.Text = CmbType.Text;
                TxtPONO.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButClr_Click(object sender, EventArgs e)
        {
             try
            {
                MyBase.Clear(this);
                TxtPONO.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButExit_Click(object sender, EventArgs e)
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

    }
}