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
    public partial class FrmYarnTransfer : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable[] DtQty;
        DataRow Dr;
        Int64 Code;
        Int64 i;
        TextBox Txt = null;        
        TextBox Txt1 = null;
        String[] Queries;
        String Str;
        TextBox TxtOrder = null;
        String Eno;
        String A;
        String Buffer_Table = String.Empty;
        Boolean Buffer_Update = false;
        Int64 Mode = 0;
        Double Tfr = 0;

        Int16 Vis = 0;
        int Pos = 0;

        Int64 ItemID = 0;
        Int64 ColorID = 0;
        Int64 SizeID = 0;
        public FrmYarnTransfer()
        {
            InitializeComponent();
        }

        private void FrmYarnTransfer_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);             
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
                if (MyParent.UserCode == 11 || MyParent.UserCode == 39)
                {
                    TxtItem.Enabled = false;
                    TxtColor.Enabled = false;
                    TxtSize.Enabled = false;
                }


                if (MyParent.UserCode != 11 && MyParent.UserCode != 39)
                {
                    TxtItem.Focus();
                    Grid_Data();
                    DtQty = new DataTable[30];
                    return;
                }
                else
                {
                    Grid_Data();
                    DtQty = new DataTable[30];
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    return;
                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Stock Transfer Entry - Edit", "select G.Transno, A.Eno, A.EDate, D.Item, E.Color, F.Size, B.Order_No From_Order, C.Order_No To_Order, Isnull(C.Iss_Qty,0)Trans_Qty, A.Rowid, B.Itemid, B.Colorid, B.Sizeid from Vsocks_StockTranMasNew A Left Join Vsocks_StockTranDetails B on A.Rowid = B.Master_ID Left Join Vsocks_StockTranDetails_Orderwise C on A.Rowid = C.Master_Id And B.Master_ID = C.Master_Id And B.Slno1 = C.Slno1 Left Join Item D on B.Itemid = D.Itemid Left Join Color E on B.Colorid = E.Colorid Left Join Size F on B.SIzeid = F.Sizeid Inner Join Alloted_Stock() G on 'GUP-VST' + REPLICATE('0',5-LEN(RTRIM(A.Eno)))+Cast((A.Eno)as Varchar(25)) = G.Transno", String.Empty, 120, 80, 100, 150, 140, 120, 120, 120, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
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
        public void Entry_Print()
        {
            try
            {  

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
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Dyeing Entry - View", "select G.Transno, A.Eno, A.EDate, D.Item, E.Color, F.Size, B.Order_No From_Order, C.Order_No To_Order, Isnull(C.Iss_Qty,0)Trans_Qty, A.Rowid, B.Itemid, B.Colorid, B.Sizeid from Vsocks_StockTranMasNew A Left Join Vsocks_StockTranDetails B on A.Rowid = B.Master_ID Left Join Vsocks_StockTranDetails_Orderwise C on A.Rowid = C.Master_Id And B.Master_ID = C.Master_Id And B.Slno1 = C.Slno1 Left Join Item D on B.Itemid = D.Itemid Left Join Color E on B.Colorid = E.Colorid Left Join Size F on B.SIzeid = F.Sizeid Left Join (Select Distinct Transno From Itemstock) G on 'GUP-VST' + REPLICATE('0',5-LEN(RTRIM(A.Eno)))+Cast((A.Eno)as Varchar(25)) = G.Transno", String.Empty, 120, 80, 100, 150, 140, 120, 120, 120, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        Boolean Check_Qty_Breakup()
        {
            Double BRQty = 0;
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    BRQty = 0;
                    if (Convert.ToDouble(Dt.Rows[i]["Iss_Qty"].ToString()) > 0)
                    {
                        if (DtQty[Convert.ToInt32(Dt.Rows[i]["SlNo1"])] == null)
                        {
                            MessageBox.Show("Invalid Orderwise Breakup Details  ...!", "Gainup");
                            MyParent.Save_Error = true;
                            Grid.CurrentCell = Grid["Iss_Qty", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return false;
                        }
                        else
                        {
                            for (int j = 0; j <= DtQty[Convert.ToInt32(Dt.Rows[i]["Slno1"])].Rows.Count - 1; j++)
                            {
                                BRQty += Convert.ToDouble(DtQty[Convert.ToInt32(Dt.Rows[i]["Slno1"])].Rows[j]["Iss_Qty"]);
                            }

                            if (Convert.ToDouble(BRQty) != Convert.ToDouble(Grid["Iss_Qty", i].Value))
                            {
                                MessageBox.Show("Check Orderwise Iss Qty...!", "Gainup");                                
                                Grid.CurrentCell = Grid["Iss_Qty", i];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void Entry_Save()
        {
            try
            {
                Int32 Array_Index = 0;
                Total_Count();

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Details..!", "Gainup");
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtTotal.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotal.Text) == 0)
                {
                    MessageBox.Show("Invalid Details", "Gainup");
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {

                    if (Grid["Iss_Qty", i].Value == DBNull.Value || Grid["Iss_Qty", i].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Iss_Qty", i].Value) == 0)
                    {
                        MessageBox.Show(" ZERO is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Iss_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                    for (i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (DtQty[Convert.ToInt32(Dt.Rows[i]["SlNo1"])] == null)
                        {
                            MessageBox.Show("Invalid Orderwise Breakup Details ...!", "Gainup");
                            MyParent.Save_Error = true;
                            Grid.CurrentCell = Grid["Iss_Qty", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                    }

                }
                if (!Check_Qty_Breakup())
                {
                   // MessageBox.Show("Invalid Orderwise Breakup Details...!", "Gainup");
                    MyParent.Save_Error = true;
                    return;
                }

                
                if (MyParent._New == true)
                {
                    TxtEntryNo.Text = MyBase.MaxOnlyComp("Vsocks_StockTranMasNew", "ENo", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();                    
                }
                else
                {
                    DataTable Tdt = new DataTable();
                    MyBase.Load_Data("Select 'GUP-VST' + REPLICATE('0',5-LEN(RTRIM(Eno))) + RTRIM(Eno) ENo from Vsocks_StockTranMasNew Where Rowid = " + Code, ref Tdt);
                    Eno = Tdt.Rows[0][0].ToString();
                    
                }
                Queries = new string[Dt.Rows.Count * 100];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Vsocks_StockTranMasNew (ENo, EDate, Remarks, Company_Code, Year_Code, User_Code, Itemid, Colorid, Sizeid) values (" + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + TxtRemarks.Text + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "'," + MyParent.UserCode + ", " + TxtItem.Tag.ToString() + ", " + TxtColor.Tag.ToString() + ", " + TxtSize.Tag.ToString() + " ); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("Yarn Stock Transfer Entry", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Vsocks_StockTranMasNew Set EDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',  Remarks = '" + TxtRemarks.Text + "',Company_Code=" + MyParent.CompCode + " , Year_Code='" + MyParent.YearCode + "',User_Code=" + MyParent.UserCode + " Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Yarn Stock Transfer Entry", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from Vsocks_StockTranDetails_Orderwise where Master_ID = " + Code;
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        Queries[Array_Index++] = "Update ItemStock Set Alloted = (Alloted - " + Grid["Iss_Qty_Old", i].Value + ") + " + Grid["Iss_qty", i].Value + ", BalQty = (BalQty + " + Grid["Iss_Qty_Old", i].Value + ") - " + Grid["Iss_Qty", i].Value + " Where Stockid = " + Grid["StockID", i].Value + "";
                    }
                    if (listBox1.Items.Count > 0)
                    {
                        for (int l = 0; l < listBox1.Items.Count; l++)
                        {
                            Queries[Array_Index++] = " update I1 Set I1.Alloted = I1.Alloted - V1.Iss_Qty, I1.BalQty = I1.BalQty + V1.Iss_Qty From ItemStock I1 Left Join Vsocks_StockTranDetails V1 On I1.StockId = V1.StockID Left Join Vsocks_StockTranMasNew v2 On V1.Master_Id = V2.Rowid Where V1.Stockid = " + listBox1.Items[l] + " And V2.RowId = " + Code;
                        }
                    }
                    Queries[Array_Index++] = "Delete from Vsocks_StockTranDetails where Master_ID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Vsocks_StockTranDetails (Master_ID, Grn_No, Slno, Stockid, ItemID, SizeID, ColorID, Stock_Qty, Iss_Qty, Iss_Qty_Old, Slno1, Order_No) Values (@@IDENTITY, '" + Grid["Grn_No", i].Value + "', " + Grid["Slno", i].Value + ", " + Grid["StockID", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ",  " + Grid["Stock_qty", i].Value + ",  " + Grid["Iss_qty", i].Value + ", " + Grid["Iss_qty", i].Value + ",  " + Grid["Slno", i].Value + ", '" + Grid["Order_No", i].Value + "')";
                    }
                    else
                    {
                        //Queries[Array_Index++] = "Insert into Vsocks_StockTranDetails (Master_ID, Grn_No, Slno, Stockid, ItemID, SizeID, ColorID, Stock_Qty, Iss_Qty, Slno1, Order_No) Values (" + Code + ", '" + Grid["Grn_No", i].Value + "', " + Grid["Slno", i].Value + ", " + Grid["StockID", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ", " + Grid["Stock_qty", i].Value + ",  " + Grid["Iss_qty", i].Value + ", " + Grid["Slno", i].Value + ", '" + Grid["Order_No", i].Value + "')";
                        Queries[Array_Index++] = "Insert into Vsocks_StockTranDetails (Master_ID, Grn_No, Slno, Stockid, ItemID, SizeID, ColorID, Stock_Qty, Iss_Qty, Iss_Qty_Old, Slno1, Order_No) Values (" + Code + ", '" + Grid["Grn_No", i].Value + "', " + Grid["Slno", i].Value + ", " + Grid["StockID", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ", " + Grid["Stock_qty", i].Value + ",  " + Grid["Iss_qty", i].Value + ", " + Grid["Iss_qty_Old", i].Value + ", " + Grid["Slno", i].Value + ", '" + Grid["Order_No", i].Value + "')";
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (i = 0; i <= DtQty.Length - 1; i++)
                    {
                        if (DtQty[i] != null)
                        {
                            for (int j = 0; j <= DtQty[i].Rows.Count - 1; j++)
                            {
                                if (MyParent._New)
                                {
                                    Queries[Array_Index++] = "Insert Into Vsocks_StockTranDetails_Orderwise (slno, Master_ID, Order_No, Iss_Qty, Iss_Qty_Old, SlNo1) Values ( " + DtQty[i].Rows[j]["Sno"].ToString() + ",@@IDENTITY, '" + DtQty[i].Rows[j]["Order_No"].ToString() + "', " + DtQty[i].Rows[j]["Iss_Qty"].ToString() + ", " + DtQty[i].Rows[j]["Iss_Qty"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ")";
                                }
                                else
                                {
                                    Queries[Array_Index++] = "Insert Into Vsocks_StockTranDetails_Orderwise (slno, Master_ID, Order_No, Iss_Qty, Iss_Qty_Old, SlNo1) Values ( " + DtQty[i].Rows[j]["Sno"].ToString() + "," + Code + ", '" + DtQty[i].Rows[j]["Order_No"].ToString() + "', " + DtQty[i].Rows[j]["Iss_Qty"].ToString() + ", " + DtQty[i].Rows[j]["Iss_Qty_Old"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ")";
                                }
                            }
                        }
                    }
                }
                if (MyParent._New)
                {
                    //Str = " Insert Into Item_stock_outward Select B.Stockid, 71, A.EDate, 'GUP-VST' + REPLICATE('0',5-LEN(RTRIM(A.Eno))) + RTRIM(A.Eno), 'TFR', D.Iss_Qty, C.Markup_Rate, D.Order_No, NULL, '', 6, '',  NULL,  'N', NULL, A.Rowid  from Vsocks_StockTranMasNew A Left Join Vsocks_StockTranDetails B on A.Rowid = B.Master_ID Left Join Itemstock C on B.Stockid = C.StockId Left Join Vsocks_StockTranDetails_Orderwise D on B.Master_ID = D.Master_ID And B.Slno1 = D.Slno1 And A.Rowid = D.Master_ID Where A.Eno = " + TxtEntryNo.Text + " ";
                    //Queries[Array_Index++] = Str;

                    //Str = " Insert into Itemstock(UnitId, Itemid, Colorid, sizeid, qty, Rate, joborderNo, TransType, Transno, alloted, ItemCat, processId, sQty, lotNo, balQty, purorprod, transdate, companyid, supplierid, return_qty, uomid, MfrId, Styleid, unit_or_other, ReProg, StockType, remarks, Markup_Rate, StoreId, YarnCompId, GSM, ItemSpecId, BeamId, OprGrpId, StockStage, FabricTempMasId, FabricNumberId, Lvalue, StockCatagoryid, UnitPerWt)  Select 0, B.Itemid, B.Colorid, B.Sizeid, Sum(Isnull(C.Iss_Qty,0)), Avg(D.Rate), C.Order_No, 'TFR', 'GUP-VST' + REPLICATE('0',5-LEN(RTRIM(A.Eno))) + RTRIM(A.Eno), 0, 'B', 0, 0, '', Sum(Isnull(C.Iss_Qty,0)), 'PU', A.EDate, 93, 0, 0, 55, 0, NULL, '', NULL, 'S', '', Avg(D.Rate), D.StoreId, 0, 0, 0, 0, 0, '', NULL, NULL, 100, NULL, 0 from Vsocks_StockTranMasNew A Left Join Vsocks_StockTranDetails B on A.Rowid = B.Master_ID Left Join Vsocks_StockTranDetails_Orderwise C on B.Master_ID = C.Master_ID And B.Slno1 = C.Slno1 And A.Rowid = C.Master_ID Left Join Itemstock D on B.Stockid = D.StockId Where A.Eno = " + TxtEntryNo.Text + " Group By B.Itemid, B.Colorid, B.Sizeid, C.Order_No, A.Eno, A.EDate, D.StoreId, A.Rowid ";
                    //Queries[Array_Index++] = Str;
                }
                else
                {
                    Str = "Delete from ItemStock where Transno = '" + Eno + "' And (alloted is null or alloted=0) ";
                    Queries[Array_Index++] = Str;

                    Str = "Delete from Item_Stock_Outward where Vsocks_Tfr_Id = " + Code + " ";
                    Queries[Array_Index++] = Str;                   

                    //Str = " Insert Into Item_stock_outward Select B.Stockid, 71, A.EDate, 'GUP-VST' + REPLICATE('0',5-LEN(RTRIM(A.Eno))) + RTRIM(A.Eno), 'TFR', D.Iss_Qty, C.Markup_Rate, D.Order_No, NULL, '', 6, '',  NULL,  'N', NULL, A.Rowid  from Vsocks_StockTranMasNew A Left Join Vsocks_StockTranDetails B on A.Rowid = B.Master_ID Left Join Itemstock C on B.Stockid = C.StockId Left Join Vsocks_StockTranDetails_Orderwise D on B.Master_ID = D.Master_ID And B.Slno1 = D.Slno1 And A.Rowid = D.Master_ID Where A.Rowid = " + Code + " ";
                   // Queries[Array_Index++] = Str;

                    //Str = " Insert into Itemstock(UnitId, Itemid, Colorid, sizeid, qty, Rate, joborderNo, TransType, Transno, alloted, ItemCat, processId, sQty, lotNo, balQty, purorprod, transdate, companyid, supplierid, return_qty, uomid, MfrId, Styleid, unit_or_other, ReProg, StockType, remarks, Markup_Rate, StoreId, YarnCompId, GSM, ItemSpecId, BeamId, OprGrpId, StockStage, FabricTempMasId, FabricNumberId, Lvalue, StockCatagoryid, UnitPerWt)  Select 0, B.Itemid, B.Colorid, B.Sizeid, Sum(Isnull(C.Iss_Qty,0)), Avg(D.Rate), C.Order_No, 'TFR', 'GUP-VST' + REPLICATE('0',5-LEN(RTRIM(A.Eno))) + RTRIM(A.Eno), 0, 'B', 0, 0, '', Sum(Isnull(C.Iss_Qty,0)), 'PU', A.EDate, 93, 0, 0, 55, 0, E.Styleid, '', NULL, 'S', '', Avg(D.Rate), D.StoreId, 0, 0, 0, 0, 0, '', NULL, NULL, 100, NULL, 0 from Vsocks_StockTranMasNew A Left Join Vsocks_StockTranDetails B on A.Rowid = B.Master_ID Left Join Vsocks_StockTranDetails_Orderwise C on B.Master_ID = C.Master_ID And B.Slno1 = C.Slno1 And A.Rowid = C.Master_ID Left Join Itemstock D on B.Stockid = D.StockId Left Join buy_ord_style E on C.Order_No = E.order_no Where A.Rowid= " + Code + " Group By B.Itemid, B.Colorid, B.Sizeid, C.Order_No, A.Eno, A.EDate, D.StoreId, E.Styleid";
                    //Queries[Array_Index++] = Str;
                }
                if (MyParent._New)
                {
                    MyBase.Run_Identity(false, Queries);
                }
                else
                {
                    MyBase.Run_Identity(true, Queries);
                }
                if (MyParent._New == true)
                {
                    MyBase.Run("Exec Update_BomDet_StockIn_From_Vsocks " + TxtEntryNo.Text + ", New ", "Exec Update_BomDet_StockOut_From_Vsocks " + TxtEntryNo.Text + ", New  ", "Exec Update_ItemStock_New_From_VSocks '" + TxtEntryNo.Text + "' ", "Exec Insert_Item_Stock_Outward_From_VSocks '" + TxtEntryNo.Text + "' ", "Exec Insert_ItemStock_From_VSocks '" + TxtEntryNo.Text + "'");
                }
                else
                {
                    MyBase.Run("Exec Update_BomDet_StockIn_From_Vsocks " + TxtEntryNo.Text + ", Edit ", "Exec Update_BomDet_StockOut_From_Vsocks " + TxtEntryNo.Text + ", Edit  ", "Exec Insert_Itemstock_Outward_Edit " + Code + "", "Exec Insert_Itemstock_Edit " + Code + "");
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
        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Stock Transfer Entry - Delete", "select G.Transno, A.Eno, A.EDate, D.Item, E.Color, F.Size, B.Order_No From_Order, C.Order_No To_Order, Isnull(C.Iss_Qty,0)Trans_Qty, A.Rowid, B.Itemid, B.Colorid, B.Sizeid from Vsocks_StockTranMasNew A Left Join Vsocks_StockTranDetails B on A.Rowid = B.Master_ID Left Join Vsocks_StockTranDetails_Orderwise C on A.Rowid = C.Master_Id And B.Master_ID = C.Master_Id And B.Slno1 = C.Slno1 Left Join Item D on B.Itemid = D.Itemid Left Join Color E on B.Colorid = E.Colorid Left Join Size F on B.SIzeid = F.Sizeid Inner Join Alloted_Stock() G on 'GUP-VST' + REPLICATE('0',5-LEN(RTRIM(A.Eno)))+Cast((A.Eno)as Varchar(25)) = G.Transno", String.Empty, 120, 80, 100, 150, 140, 120, 120, 120, 100);
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
                    MyBase.Run("Exec Update_BomDet_StockIn_From_Vsocks " + TxtEntryNo.Text + ", 'Delete' ", "Exec Update_BomDet_StockOut_From_Vsocks " + TxtEntryNo.Text + ", 'Delete'  ", "Exec Update_ItemStock_Delete_From_VSocks '" + TxtEntryNo.Text + "'", "Delete from ItemStock where Transno = '" + A + "' And (alloted is null or alloted=0) ", "Delete from Item_Stock_Outward where Vsocks_Tfr_Id = " + Code, "Delete from Vsocks_StockTranDetails_Orderwise where Master_ID = " + Code, "Delete from Vsocks_StockTranDetails where Master_ID = " + Code, "Delete From Vsocks_StockTranMasNew Where RowID = " + Code, MyParent.EntryLog("Yarn Dyeing Entry", "DELETE", Code.ToString()));
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

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                //String A;
                Code = Convert.ToInt64(Dr["RowID"]);
                TxtEntryNo.Text = Dr["ENo"].ToString();
                A = Dr["Transno"].ToString();
                TxtItem.Text = Dr["Item"].ToString();
                TxtItem.Tag = Dr["Itemid"].ToString();
                TxtColor.Text = Dr["Color"].ToString();
                TxtColor.Tag = Dr["Colorid"].ToString();
                TxtSize.Text = Dr["Size"].ToString();
                TxtSize.Tag = Dr["Sizeid"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["EDate"]);                                
                Grid_Data();
                Total_Count();
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
                    Str = "select 0 as Slno, ''Order_No, '' Grn_No,  Item,  Color, Size, 0.000 Stock_Qty, 0.000 Iss_Qty, 0 Stockid, Itemid, Colorid, Sizeid,0 Slno1, 0 RNo, 0.000 Iss_Qty_Old, '-' T  from FITSOCKS.dbo.Yarn_Dyeing_Requirement_Details() where 1=2";
                }
                else
                {
                    Str = "Select A.Slno, A.Order_No, A.Grn_No,  C.Item, D.Color, E.Size, A.Stock_Qty, A.Iss_Qty, A.Stockid, A.Itemid, A.Colorid, A.Sizeid, A.Slno1, ROW_NUMBER() Over (Order by A.Itemid, A.Colorid, A.Sizeid) RNo, A.Iss_Qty Iss_Qty_Old, '-' T  from fitsocks.dbo.Vsocks_StockTranDetails A Left Join fitsocks.dbo.Vsocks_StockTranMasNew B on A.Master_ID = B.Rowid Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid Where B.Eno = '" + TxtEntryNo.Text + "' Order By A.Slno1 ";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Stockid", "ItemID", "SizeID", "ColorID", "Slno1", "RNo", "Iss_Qty_Old", "T");
                
                MyBase.ReadOnly_Grid_Without(ref Grid, "Order_No", "Iss_Qty");
                MyBase.Grid_Width(ref Grid, 50, 150, 150, 160, 160, 145, 100, 100);
                Grid.Columns["Stock_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Iss_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent.Edit)
                    {
                        //TxtQty.Text = Grid["Rejection", i].Value.ToString();
                        Vis = 1;
                        Pos = i;
                        GridDetail_Data(Convert.ToInt16(Grid["Slno1", i].Value));
                        Vis = 0;
                        Pos = 0;
                    }
                }
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
                // Due To Fill Bom Function For Transfer, Bal Qty Updation in Text box's
                //if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                //{
                //    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                //    {
                //        listBox1.Items.Add(Grid["StockID", Grid.CurrentCell.RowIndex].Value.ToString());
                //        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                //        //Dt.AcceptChanges(); 
                //    }
                //}
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Item"].Index)
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                    {
                        if (MyParent.UserCode != 11 && MyParent.UserCode != 39)
                        {
                            if (TxtSize.Text != String.Empty || TxtItem.Text != String.Empty || TxtColor.Text != String.Empty)
                            {
                                if (MyParent.UserCode == 19 || MyParent.UserCode == 92)
                                {
                                    Dr = Tool.Selection_Tool_Except_New("Stockid", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Order_No", "Select Order_No, Transno Grn_No,  Item, Color, Size, isnull(Stock_Qty,0) Stock_Qty, 0 Iss_Qty, Stockid, Itemid, Colorid, Sizeid, ROW_NUMBER() Over (Order by Itemid, Colorid, Sizeid, Stockid) RNo From Stock_Details_Transfer_Entry() Where Item = '" + TxtItem.Text + "' And Color = '" + TxtColor.Text + "' And Size = '" + TxtSize.Text + "' And Order_No not like '%MOQ%'", String.Empty, 102, 102, 128, 128, 75, 75, 50);
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool_Except_New("Stockid", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Order_No", "Select Order_No, Transno Grn_No, Item, Color, Size, isnull(Stock_Qty,0) Stock_Qty, 0 Iss_Qty, Stockid, Itemid, Colorid, Sizeid, ROW_NUMBER() Over (Order by Itemid, Colorid, Sizeid, Stockid) RNo From Stock_Details_Transfer_Entry() Where Item = '" + TxtItem.Text + "' And Color = '" + TxtColor.Text + "' And Size = '" + TxtSize.Text + "' ", String.Empty, 102, 102, 128, 128, 75, 75, 50);
                                }

                                if (Dr != null)
                                {
                                    Txt.Text = Dr["Order_No"].ToString();
                                    Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["ITEM"].ToString();
                                    Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                                    Grid["Grn_No", Grid.CurrentCell.RowIndex].Value = Dr["Grn_No"].ToString();
                                    Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["SIZE"].ToString();
                                    Grid["Color", Grid.CurrentCell.RowIndex].Value = Dr["COLOR"].ToString();
                                    Grid["Stock_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Stock_Qty"].ToString();
                                    Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Iss_Qty"].ToString();
                                    Grid["StockID", Grid.CurrentCell.RowIndex].Value = Dr["StockID"].ToString();
                                    Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                                    Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                                    Grid["ColorID", Grid.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();
                                    Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                                    Grid["RNo", Grid.CurrentCell.RowIndex].Value = Dr["RNo"].ToString();                                    
                                }
                            }
                            else
                            {
                                MessageBox.Show("Select Item , Color And Size ", "Gainup");
                                TxtItem.Focus();
                            }
                        }
                        else
                        {
                            if (Grid.Rows.Count == 1)
                            {
                                Dr = Tool.Selection_Tool_Except_New("Stockid", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Order_No", "Select Order_No, Transno Grn_No, Item, Color, Size, isnull(Stock_Qty,0) Stock_Qty, 0 Iss_Qty, Stockid, Itemid, Colorid, Sizeid, ROW_NUMBER() Over (Order by Itemid, Colorid, Sizeid, Stockid) RNo From Stock_Details_Transfer_Entry() Where Order_No like '%MOQ%'", String.Empty, 102, 102, 128, 128, 75, 75, 50);
                                if (Dr != null)
                                {
                                    Txt.Text = Dr["Order_No"].ToString();
                                    Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["ITEM"].ToString();
                                    Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                                    Grid["Grn_No", Grid.CurrentCell.RowIndex].Value = Dr["Grn_No"].ToString();
                                    Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["SIZE"].ToString();
                                    Grid["Color", Grid.CurrentCell.RowIndex].Value = Dr["COLOR"].ToString();
                                    Grid["Stock_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Stock_Qty"].ToString();
                                    Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Iss_Qty"].ToString();
                                    Grid["StockID", Grid.CurrentCell.RowIndex].Value = Dr["StockID"].ToString();
                                    Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                                    Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                                    Grid["ColorID", Grid.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();
                                    Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                                    Grid["RNo", Grid.CurrentCell.RowIndex].Value = Dr["RNo"].ToString();
                                    TxtItem.Text = Dr["Item"].ToString();
                                    TxtItem.Tag = Dr["Itemid"].ToString();
                                    TxtColor.Text = Dr["Color"].ToString();
                                    TxtColor.Tag = Dr["Colorid"].ToString();
                                    TxtSize.Text = Dr["Size"].ToString();
                                    TxtSize.Tag = Dr["Sizeid"].ToString();

                                }
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool_Except_New("Stockid", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Order_No", "Select Order_No, Transno Grn_No, Item, Color, Size, isnull(Stock_Qty,0) Stock_Qty, 0 Iss_Qty, Stockid, Itemid, Colorid, Sizeid, ROW_NUMBER() Over (Order by Itemid, Colorid, Sizeid, Stockid) RNo From Stock_Details_Transfer_Entry() Where Order_No like '%MOQ%' And Itemid = " + TxtItem.Tag + "  And Colorid = " + TxtColor.Tag + " And Sizeid = " + TxtSize.Tag + " ", String.Empty, 102, 102, 128, 128, 75, 75, 50);
                                if (Dr != null)
                                {
                                    Txt.Text = Dr["Order_No"].ToString();
                                    Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["ITEM"].ToString();
                                    Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                                    Grid["Grn_No", Grid.CurrentCell.RowIndex].Value = Dr["Grn_No"].ToString();
                                    Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["SIZE"].ToString();
                                    Grid["Color", Grid.CurrentCell.RowIndex].Value = Dr["COLOR"].ToString();
                                    Grid["Stock_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Stock_Qty"].ToString();
                                    Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Iss_Qty"].ToString();
                                    Grid["StockID", Grid.CurrentCell.RowIndex].Value = Dr["StockID"].ToString();
                                    Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                                    Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                                    Grid["ColorID", Grid.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();
                                    Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                                    Grid["RNo", Grid.CurrentCell.RowIndex].Value = Dr["RNo"].ToString();                                    

                                }

                            }
                        }

                    }
                }
                Total_Count();
                //if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                //{
                //    e.Handled = true;
                //}
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Iss_Qty"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Iss_Qty"].Index)
                {
                    if ((Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) != String.Empty)
                    {
                        if (Convert.ToDouble(Grid["Stock_Qty", Grid.CurrentCell.RowIndex].Value) < Convert.ToDouble(Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value))
                        {
                            MessageBox.Show("Invalid Iss_Qty..!", "Gainup");
                            Grid.CurrentCell = Grid["Iss_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
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


        void TxtIss_Leave(object sender, EventArgs e)
        {
            try
            {
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Order_No"].Index)
                {
                    LblBal.Text = "0";
                    LblReq.Text = "0";
                    LblTfr.Text = "0";
                    Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString()); 
                    if ((GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString()) != String.Empty)
                    {   
                        if (Convert.ToDouble(GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value) > Convert.ToDouble(LblBal.Text))
                        {
                            MessageBox.Show("Invalid Iss_Qty..!", "Gainup");
                            GridDetail.CurrentCell = GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            MyParent.Save_Error = true;
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
        void Total_Count()
        {
            try
            {
                TxtTotal.Text = MyBase.Sum_With_Three_Digits(ref Grid, "Iss_Qty");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //void GridDetail_Data(Int32 Row, Int32 Iss_Qty, Int64 Item, Int64 Color, Int64 Size)
        void GridDetail_Data(Int32 Row)
        {

            try
            {
                LblReq.Text = "0";
                LblTfr.Text = "0";
                LblBal.Text = "0";
                if (DtQty[Row] == null)
                {
                    DtQty[Row] = new DataTable();
                    if (MyParent._New)
                    {
                        MyBase.Load_Data("select 0 SNo, '0' Order_No, 0.000 Iss_Qty," + Row + " SlNo1, 0.000 Iss_Qty_old, '' T from Yarn_Dyeing_Requirement_Details() where 1=2 ", ref DtQty[Row]);
                    }
                    else
                    {
                        if (MyParent.Edit && Vis == 1)
                        {
                            MyBase.Load_Data("select A.slno Sno, A.Order_No,  A.Iss_Qty, B.Slno1, A.Iss_Qty  Iss_Qty_old,'' T from Vsocks_StockTranDetails_Orderwise A Left Join Vsocks_StockTranDetails B on A.Master_ID = B.Master_ID and A.SlNo1 = B.Slno1 Left Join Vsocks_StockTranMasNew C on A.Master_ID = C.RowID and B.Master_ID = C.RowID  Where  A.Master_ID =  " + Code + " and  B.Slno1 = " + Grid["Slno1", Pos].Value.ToString(), ref DtQty[Row]);
                        }
                        else
                        {
                            MyBase.Load_Data("select A.slno Sno, A.Order_No,  A.Iss_Qty, B.Slno1, A.Iss_Qty  Iss_Qty_old,'' T from Vsocks_StockTranDetails_Orderwise A Left Join Vsocks_StockTranDetails B on A.Master_ID = B.Master_ID and A.SlNo1 = B.Slno1 Left Join Vsocks_StockTranMasNew C on A.Master_ID = C.RowID and B.Master_ID = C.RowID  Where  A.Master_ID =  " + Code + " and  B.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString(), ref DtQty[Row]);
                        }
                    }
                }
                GridDetail.DataSource = DtQty[Row];
                MyBase.Grid_Designing(ref GridDetail, ref DtQty[Row],"Iss_Qty_Old", "SlNo1", "T");
                MyBase.ReadOnly_Grid_Without(ref GridDetail, "Iss_Qty", "Order_No");
                MyBase.Grid_Colouring(ref GridDetail, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridDetail, 50, 150, 100);
                GridDetail.Columns["Iss_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                GridDetail.RowHeadersWidth = 10;

                if (!MyParent._New)
                {
                    //Balance_Pieces();
                }

                if (MyParent.Edit && Vis == 1)
                {
                    GBQty.Visible = false;
                }
                else
                {
                    GBQty.Visible = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void GridDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.KeyCode == Keys.Enter)
                {
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Iss_Qty"].Index)
                    {
                        if (GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value) == 0 )
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Iss_Qty...!", "Gainup");
                            GridDetail.CurrentCell = GridDetail["Iss_Qty", Grid.CurrentCell.RowIndex];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            return;
                        }
                    }
                    
                }
                Iss_Balance();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Fill_BOM()
        {
            throw new NotImplementedException();
        }


        void Fill_BOM( String OrderNo)
        {
            try
            {
                LblTfr.Text = "0";
                LblReq.Text = "0";
                      LblBal.Text="0";
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select A.Order_No, A.Bal_Qty Req_Qty from fitsocks.dbo.Socks_Trans_Knit_Req_Orderwise()A Left Join Buy_Ord_Mas B on A.Order_No = B.Order_No where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + "  And A.Order_No = '" + OrderNo + "' ", ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    LblReq.Text = Tdt.Rows[0]["Req_Qty"].ToString();
                }

                for (int i = 1; i <= Dt.Rows.Count; i++)
                {
                    if (DtQty[i] != null)
                    {
                        for (int j = 0; j <= DtQty[i].Rows.Count - 1; j++)
                        {
                            if (DtQty[i].Rows[j]["Order_No"].ToString() == OrderNo)
                            {
                                if (Grid.CurrentCell.RowIndex != i - 1)
                                {
                                    LblTfr.Text = String.Format("{0:0.000}", Convert.ToDouble(LblTfr.Text) + Convert.ToDouble(DtQty[i].Rows[j]["Iss_Qty"]));
                                }
                            }
                        }
                    }
                }
                //LblTfr.Text = String.Format("{0:0.000}", Tfr);
                LblBal.Text = String.Format("{0:0.000}", Convert.ToDouble(LblReq.Text) - Convert.ToDouble(LblTfr.Text));
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void TxtIss_KeyDown(object sender, KeyEventArgs e)
        {
            try
            
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Order_No"].Index)
                    {
                        if (MyParent.UserCode == 19 || MyParent.UserCode == 92 || MyParent.UserCode == 92)
                        {
                            Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select A.Order_No, 0.000 Iss_Qty  from fitsocks.dbo.Socks_Trans_Knit_Req_Orderwise()A  where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And Order_No not like '%MOQ%' And Order_No!= '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' ", String.Empty, 150, 100);
                        }
                        else if (MyParent.UserCode == 11 || MyParent.UserCode == 39)
                        {
                            Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select A.Order_No, 0.000 Iss_Qty from fitsocks.dbo.Socks_Trans_Knit_Req_Orderwise()A  where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + "  And Order_No not like '%MOQ%' and Buyerid=100 And Order_No!= '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' ", String.Empty, 150, 100);
                        }
                        else
                        {
                            Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select A.Order_No,  0.000 Iss_Qty  from fitsocks.dbo.Socks_Trans_Knit_Req_Orderwise()A  where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And Order_No!= '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' ", String.Empty, 150, 100);
                        }

                        if (Dr != null)
                        {
                            Txt1.Text = Dr["Order_No"].ToString();
                            
                            //Fill_BOM(Dr["Order_No"].ToString());
                            
                            GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                            GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value = Dr["Iss_Qty"].ToString();
                            GridDetail["SlNo1", GridDetail.CurrentCell.RowIndex].Value = Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();
                            Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString()); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void TxtIss_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Iss_Qty"].Index)
                {
                    MyBase.Valid_Decimal(Txt1, e);
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
        void TxtIss_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Iss_Qty"].Index)
                {
                    if (GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value) == 0)
                    {

                        //GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value = Convert.ToDouble(TxtBalance.Text);
                    }
                }
                Iss_Balance();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Iss_Balance()
        {
            try
            {

                if (TxtQty1.Text.Trim() == String.Empty)
                {
                    TxtQty1.Text = "0.000";
                }

                TxtEnteredWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum_With_Three_Digits(ref GridDetail, "Iss_Qty")));

                if (TxtEnteredWeight.Text.Trim() == String.Empty)
                {
                    TxtEnteredWeight.Text = "0.000";
                }

                TxtBalance.Text = String.Format("{0:0.000}", Convert.ToDouble(TxtQty1.Text) - Convert.ToDouble(TxtEnteredWeight.Text));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GridDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt1 == null)
                {
                    Txt1 = (TextBox)e.Control;
                    Txt1.KeyPress += new KeyPressEventHandler(TxtIss_KeyPress);
                    Txt1.GotFocus += new EventHandler(TxtIss_GotFocus);
                    Txt1.KeyDown += new KeyEventHandler(TxtIss_KeyDown);
                    Txt1.Leave += new EventHandler(TxtIss_Leave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ButOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtBalance.Text.Trim() == String.Empty || TxtBalance.Text != "0.000")
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    GridDetail.CurrentCell = GridDetail["Iss_Qty", 0];
                    GridDetail.Focus();
                    GridDetail.BeginEdit(true);
                    return;
                }
                else
                {
                    GBQty.Visible = false;
                    Grid.CurrentCell = Grid["Order_No", (Grid.CurrentCell.RowIndex + 1)];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    return;
                }

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
                for (int i = 0; i <= GridDetail.Rows.Count - 1; i++)
                {
                    if (GridDetail["Iss_Qty", i].Value == DBNull.Value || Convert.ToDouble(GridDetail["Iss_Qty", i].Value) == 0.000)
                    {
                        MessageBox.Show("Invalid KGS ..!", "Gainup");
                        Grid.CurrentCell = Grid["Iss_Qty", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        GBQty.Visible = false;
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                DtQty = new DataTable[30];
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Iss_Qty", (Grid.CurrentCell.RowIndex)];
                Grid.Focus();
                Grid.BeginEdit(true);
                return;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmYarnTransfer_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                    if (e.KeyCode == Keys.Enter)
                    {
                        e.Handled = true;
                        if (MyParent.UserCode != 11 && MyParent.UserCode != 39)
                        {
                            if (this.ActiveControl.Name == "TxtItem")
                            {
                                TxtColor.Focus();
                                return;
                            }
                            else if (this.ActiveControl.Name == "TxtColor")
                            {
                                TxtSize.Focus();
                                return;
                            }
                            else if (this.ActiveControl.Name == "TxtSize")
                            {
                                Grid.CurrentCell = Grid["Order_No", 0];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                        }
                        if (this.ActiveControl.Name == "TxtTotal")
                        {
                            if (MyParent._New == true || MyParent.Edit == true)
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
                        if (MyParent.UserCode != 11 && MyParent.UserCode != 39)
                        {
                            if (this.ActiveControl.Name == "TxtItem")
                            {
                                if (TxtSize.Text == String.Empty && TxtColor.Text == String.Empty)
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", " Select Distinct Item, Itemid from Stock_Details_Transfer_Entry() ", String.Empty, 150);
                                }
                                else if (TxtSize.Text != String.Empty && TxtColor.Text == String.Empty)
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", " Select Distinct Item, Itemid from Stock_Details_Transfer_Entry() Where Size = '" + TxtSize.Text + "' ", String.Empty, 150);
                                }
                                else if (TxtSize.Text == String.Empty && TxtColor.Text != String.Empty)
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", " Select Distinct Item, Itemid from Stock_Details_Transfer_Entry() Where Color = '" + TxtColor.Text + "' ", String.Empty, 150);
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", " Select Distinct Item, Itemid from Stock_Details_Transfer_Entry() Where Size = '" + TxtSize.Text + "' And Color = '" + TxtColor.Text + "' ", String.Empty, 150);
                                }
                                if (Dr != null)
                                {
                                    TxtItem.Text = Dr["Item"].ToString();
                                    TxtItem.Tag = Dr["Itemid"].ToString();
                                }
                            }
                            if (this.ActiveControl.Name == "TxtColor")
                            {
                                if (TxtItem.Text == String.Empty && TxtSize.Text == String.Empty)
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", " Select Distinct Color, Colorid from Stock_Details_Transfer_Entry() ", String.Empty, 150);
                                }
                                else if (TxtItem.Text != String.Empty && TxtSize.Text == String.Empty)
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", " Select Distinct Color, Colorid from Stock_Details_Transfer_Entry() Where Item = '" + TxtItem.Text + "' ", String.Empty, 150);
                                }
                                else if (TxtItem.Text == String.Empty && TxtSize.Text != String.Empty)
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", " Select Distinct Color, Colorid from Stock_Details_Transfer_Entry() Where Size = '" + TxtSize.Text + "' ", String.Empty, 150);
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", " Select Distinct Color, Colorid from Stock_Details_Transfer_Entry() Where Item = '" + TxtItem.Text + "' And Size = '" + TxtSize.Text + "' ", String.Empty, 150);
                                }
                                if (Dr != null)
                                {
                                    TxtColor.Text = Dr["Color"].ToString();
                                    TxtColor.Tag = Dr["Colorid"].ToString();
                                }
                            }
                            if (this.ActiveControl.Name == "TxtSize")
                            {
                                if (TxtItem.Text == String.Empty && TxtColor.Text == String.Empty)
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", " Select Distinct Size, Sizeid from Stock_Details_Transfer_Entry() ", String.Empty, 150);
                                }
                                else if (TxtItem.Text != String.Empty && TxtColor.Text == String.Empty)
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", " Select Distinct Size, Sizeid from Stock_Details_Transfer_Entry() Where Item = '" + TxtItem.Text + "' ", String.Empty, 150);
                                }
                                else if (TxtItem.Text == String.Empty && TxtColor.Text != String.Empty)
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", " Select Distinct Size, Sizeid from Stock_Details_Transfer_Entry() Where Color = '" + TxtColor.Text + "' ", String.Empty, 150);
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", " Select Distinct Size, Sizeid from Stock_Details_Transfer_Entry() Where Item = '" + TxtItem.Text + "' And Color = '" + TxtColor.Text + "' ", String.Empty, 150);
                                }
                                if (Dr != null)
                                {
                                    TxtSize.Text = Dr["Size"].ToString();
                                    TxtSize.Tag = Dr["Sizeid"].ToString();
                                }
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
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {

                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Iss_Qty"].Index)
                    {
                        if ((Convert.ToDecimal(Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value) > Convert.ToDecimal(Grid["Stock_Qty", Grid.CurrentCell.RowIndex].Value)) || Convert.ToDecimal(Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value)==Convert.ToDecimal(0.000))                        
                        {
                            MessageBox.Show("Transfer Qty Should Be Less Than Or Equal To Stock_Qty");
                            Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value = 0.000;
                            Grid.CurrentCell = Grid["Iss_Qty", 0];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            e.Handled = true;
                            return;
                        }
                        else
                        {
                            
                            TxtQty1.Text = Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value.ToString();

                            ItemID = Convert.ToInt64(Grid["ItemId", Grid.CurrentCell.RowIndex].Value);
                            ColorID = Convert.ToInt64(Grid["ColorId", Grid.CurrentCell.RowIndex].Value);
                            SizeID = Convert.ToInt64(Grid["SizeId", Grid.CurrentCell.RowIndex].Value);

                            //GridDetail_Data(Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value), Convert.ToInt32(Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value), ItemID, ColorID, SizeID);
                            GridDetail_Data(Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value));
                            GridDetail.CurrentCell = GridDetail["Order_No", 0];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            e.Handled = true;
                            return;
                        }
                    }
                    //else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Item"].Index)
                    //{
                    //    Grid.CurrentCell = Grid["Iss_Qty", 0];
                    //    Grid.Focus();
                    //    Grid.BeginEdit(true);
                    //    e.Handled = true;
                    //    return;
                    //}
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
                //if (e.KeyChar == Convert.ToChar(Keys.Escape))
                //{
                //    Total_Count();
                //    TxtRemarks.Focus();
                //    return;
                //}
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    TxtRemarks.Focus();
                    TxtRemarks.SelectAll();
                    SendKeys.Send("{End}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridDetail_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
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

        private void GridDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //MyBase.Grid_Delete(ref GridDetail, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], GridDetail.CurrentCell.RowIndex);
                //DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].AcceptChanges();
                //MyBase.Row_Number(ref GridDetail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void FrmYarnTransfer_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtItem" || this.ActiveControl.Name == "TxtColor" || this.ActiveControl.Name == "TxtSize" || this.ActiveControl.Name == "TxtTotal")
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
        private void GridDetail_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (GridDetail.Rows.Count > 1)
                {
                    MyBase.Row_Number(ref GridDetail);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
