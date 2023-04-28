using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using Accounts;
using System.IO;
using System.Windows.Forms;

namespace Accounts
{
    public partial class Frm_Socks_Dyeing_Receipt : Form, Entry
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
        String Buffer_Table = String.Empty;
        Boolean Buffer_Update = false;
        Int64 Mode = 0;

        Int64 ItemID = 0;
        Int64 ColorID = 0;
        Int64 SizeID = 0;
        Int32 Delivery_No = 0;
        public Frm_Socks_Dyeing_Receipt()
        {
            InitializeComponent();
        }

        private void Frm_Socks_Dyeing_Receipt_Load(object sender, EventArgs e)
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
         public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                TxtSupplier.Focus();
                Grid_Data();
                DtQty = new DataTable[30];
                return;
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Dyeing Entry - Edit", "Select B.Eno, Cast(B.Edate as Date)Date, F.Supplier, B.Dc_No, Cast(B.DcDate as Date)Dc_Date,  C.Item Yarn, D.Color, E.Size Count, A.Rec_Qty, B.Remarks, B.Supplierid, A.Itemid, A.Colorid, A.Sizeid, B.Rowid   from fitsocks.dbo.Socks_Dyeing_Receipt_Details A Left Join fitsocks.dbo.Socks_Dyeing_Receipt_Master B on A.Master_ID = B.Rowid Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid Left Join fitsocks.dbo.supplier F on B.Supplierid = F.Supplierid Where B.Eno not in(Select Distinct Cast(substring(A.Transno,8,5)as Int)Eno from Itemstock A Inner Join Item_stock_outward B on A.StockId = B.Itemstockid Where A.Transno like '%vsr%')", String.Empty, 80, 100, 150, 100, 100, 150, 200, 100, 100, 150);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Delivery_No", 0];
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
                //MyParent.View_Browser("MIS_SOCKS_YARNDYEING_GRN", Code);
                Str = "Select C1.companyid, C1.company, C1.address1 Comp_Address1, C1.Address2 Comp_Address2, C1.City Comp_City, C1.TinNo Comp_Tin, C1.cst_no Comp_Cst_No, C1.Cst_Date Comp_Cst_Date,";
                Str = Str + " S1.Type, S1.RowID Supplier_ROdid, S1.ENo, S1.Date, S1.Supplierid, S1.Supplier, S1.Dc_No, S1.Dc_Date, S1.address1 Supplier_Address1, S1.Address2 Supplier_Address2, S1.address3 Supplier_Address3, S1.City Supplier_City,";
                Str = Str + " D1.Rowid, D1.itemid, D1.item, D1.Colorid, D1.Color, D1.Sizeid, D1.SIze, D1.Rec_Qty,  D1.remarks";
                Str = Str + " from [FITSOCKS].dbo.Supplier_Details_Yarn_Dyeing() S1 Left Join [FITSOCKS].Dbo.Dyeing_Receipt_For_Dc() D1 On S1.Rowid = D1.Rowid ";
                Str = Str + " Left Join [FITSOCKS].dbo.Company_Details() C1 On 1 =1 Where S1.Rowid = " + Code + " And S1.Type = 'Receipt' ";

                MyBase.Execute_Qry(Str, "Yarn_Dyeing_Receipt");
                CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Yarn_Dyeing_Receipt.rpt");
                MyParent.CReport(ref ObjRpt, "Yarn Dyeing Receipt..!");
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Dyeing Entry - View", "Select B.Eno, Cast(B.Edate as Date)Date, F.Supplier, B.Dc_No, Cast(B.DcDate as Date)Dc_Date,  C.Item Yarn, D.Color, E.Size Count,  A.Rec_Qty, B.Remarks, B.Supplierid, A.Itemid, A.Colorid, A.Sizeid, B.Rowid   from fitsocks.dbo.Socks_Dyeing_Receipt_Details A Left Join fitsocks.dbo.Socks_Dyeing_Receipt_Master B on A.Master_ID = B.Rowid Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid Left Join fitsocks.dbo.supplier F on B.Supplierid = F.Supplierid", String.Empty, 80, 100, 150, 100, 100, 150, 200, 100, 100, 150);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Yarn", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
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
                Total_Count();

                decimal Sum = 0;

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (i = 0; i <= DtQty.Length - 1; i++)
                    {
                        if (DtQty[i] != null)
                        {
                            Sum = 0;
                            for (int j = 0; j <= DtQty[i].Rows.Count - 1; j++)
                            {
                                Sum = Sum + Convert.ToDecimal(DtQty[i].Rows[j]["Rec_Qty"]);
                            }
                            if (Convert.ToDecimal(Grid["Rec_Qty", i - 1].Value) != Sum)
                            {
                                MessageBox.Show("Invalid Details..!", "Gainup");
                                Grid.CurrentCell = Grid["Rec_Qty", i - 1];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }
                    }
                }
                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Details..!", "Gainup");
                    Grid.CurrentCell = Grid["Yarn", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtTotal.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotal.Text) == 0)
                {
                    MessageBox.Show("Invalid Details", "Gainup");
                    Grid.CurrentCell = Grid["Yarn", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {

                    if (Grid["Rec_Qty", i].Value == DBNull.Value || Grid["Rec_Qty", i].Value.ToString() == String.Empty || Convert.ToInt64(Grid["Rec_Qty", i].Value) == 0)
                    {
                        MessageBox.Show(" ZERO is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Rec_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (DtQty[Convert.ToInt32(Dt.Rows[i]["SlNo1"])] == null)
                    {
                        MessageBox.Show("Invalid Orderwise Breakup Details ...!", "Gainup");
                        MyParent.Save_Error = true;
                        Grid.CurrentCell = Grid["Remarks", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                }


                if (MyParent._New)
                {
                    TxtEntryNo.Text = MyBase.MaxOnlyComp("Socks_Dyeing_Receipt_Master", "ENo", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();
                }
                Queries = new string[Dt.Rows.Count * 100];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_Dyeing_Receipt_Master (ENo, EDate, Remarks, Dc_No, DcDate, Company_Code, Year_Code,User_Code,Supplierid) values (" + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + TxtRemarks.Text + "','" + TxtSupplierDc.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DcDate.Value) + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "'," + MyParent.UserCode + ", " + TxtSupplier.Tag.ToString() + "); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("Yarn Dyeing Receipt Entry", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_Dyeing_Receipt_Master Set EDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', Dc_No='" + TxtSupplierDc.Text + "',DcDate = '" + String.Format("{0:dd-MMM-yyyy}", DcDate.Value) + "', Remarks = '" + TxtRemarks.Text + "',Company_Code=" + MyParent.CompCode + " , Year_Code='" + MyParent.YearCode + "',User_Code=" + MyParent.UserCode + " Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Yarn Dyeing Receipt Entry", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from Socks_Dyeing_Receipt_Details where Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete from Socks_Dyeing_OrderwiseReceipt_details where Master_ID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Socks_Dyeing_Receipt_Details (Master_ID, Slno, Delivery_No, ItemID, SizeID, ColorID, Rec_Qty,  Remarks, Slno1) Values (@@IDENTITY, " + Grid["Slno", i].Value + ", " + Grid["Delivery_No", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ",  " + Grid["Rec_qty", i].Value + ",  '" + Grid["Remarks", i].Value + "', " + Grid["Slno", i].Value + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Socks_Dyeing_Receipt_Details (Master_ID, Slno, Delivery_No, ItemID, SizeID, ColorID, Rec_Qty,  Remarks, Slno1) Values (" + Code + ", " + Grid["Slno", i].Value + ", " + Grid["Delivery_No", i].Value + "," + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ", " + Grid["Rec_qty", i].Value + ", '" + Grid["Remarks", i].Value + "','" + Grid["Slno", i].Value + "')";
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count-1 ; i++)
                {
                    for (i = 0; i <= DtQty.Length - 1; i++)
                    {
                        if (DtQty[i] != null)
                        {
                            for (int j = 0; j <= DtQty[i].Rows.Count - 1; j++)
                            {
                                if (MyParent._New)
                                {
                                    Queries[Array_Index++] = "Insert Into Socks_Dyeing_OrderwiseReceipt_details (slno, Master_ID, Order_No, Rec_Qty,  SlNo1) Values ( " + DtQty[i].Rows[j]["Sno"].ToString() + ",@@IDENTITY, '" + DtQty[i].Rows[j]["Order_No"].ToString() + "'," + DtQty[i].Rows[j]["Rec_Qty"].ToString() + "," + DtQty[i].Rows[j]["Slno1"].ToString() + ")";
                                }
                                else
                                {
                                    Queries[Array_Index++] = "Insert Into Socks_Dyeing_OrderwiseReceipt_details (slno, Master_ID, Order_No,  Rec_Qty,  SlNo1) Values ( " + DtQty[i].Rows[j]["Sno"].ToString() + "," + Code + ", '" + DtQty[i].Rows[j]["Order_No"].ToString() + "', " + DtQty[i].Rows[j]["Rec_Qty"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ")";
                                }
                            }
                        }
                    }
                }

                if (MyParent._New)
                {
                    Str = "Insert Into ItemStock (UnitId, Itemid, Colorid, sizeid, qty, Rate, joborderNo, TransType, Transno, alloted, ItemCat, processId, sQty, lotNo, balQty, ";
                    Str = Str + " purorprod, transdate, companyid, supplierid, return_qty, uomid, MfrId, Styleid, unit_or_other, ReProg, StockType, remarks, Markup_Rate, StoreId,";
                    Str = Str + " YarnCompId, GSM, ItemSpecId, BeamId, OprGrpId, StockStage, FabricTempMasId, FabricNumberId, Lvalue, StockCatagoryid, UnitPerWt)";
                    Str = Str + " Select 71 UnitId, S1.Itemid, S1.Colorid, S1.sizeid, Sum(S2.Rec_Qty) qty, 0 as Rate, S2.order_no joborderNo, 'PQC' TransType,  'GUP-VSR' + REPLICATE('0',5-LEN(RTRIM(S3.Eno))) + RTRIM(S3.Eno) Transno, ";
                    Str = Str + " 0 alloted, 'B' ItemCat, 158 processId, 0.000 sQty, '' lotNo, Sum(S2.Rec_Qty)balQty, 'RR' purorprod, Cast(S3.EDate As Date) transdate, 93 companyid,";
                    Str = Str + " S3.Supplierid supplierid, 0 as return_qty, 55 as uomid, 0 as MfrId,B1.Styleid Styleid, '' unit_or_other, Null ReProg, 'S' StockType, '' remarks, ";
                    Str = Str + " 0 as Markup_Rate, 6 StoreId, 0 YarnCompId, 0.000 GSM, 0 ItemSpecId, 0 BeamId, 0 OprGrpId,  '' StockStage, NUll FabricTempMasId, Null FabricNumberId, ";
                    Str = Str + " 100 Lvalue, Null StockCatagoryid, 0 UnitPerWt From Socks_Dyeing_Receipt_Details S1 Left Join Socks_Dyeing_OrderwiseReceipt_details S2 On S1.Master_ID = s2.Master_ID And S1.Slno1 = S2.Slno1";
                    Str = Str + " Left Join Socks_Dyeing_Receipt_Master S3 On S1.Master_ID = S3.RowID And S2.Master_ID = S3.RowID Left Join buy_ord_style B1 On S2.order_no = B1.order_no";
                    Str = Str + " Where S3.ENo = " + TxtEntryNo.Text + " Group By Itemid, Colorid, sizeid, S2.order_no, 'GUP-VSR' + REPLICATE('0',5-LEN(RTRIM(S3.Eno))) + RTRIM(S3.Eno) , Cast(S3.EDate As Date), S3.supplierid, B1.Styleid";

                    Queries[Array_Index++] = Str;
                }
                else
                {
                    Str = "Delete from ItemStock where Transno like 'GUP-VSD%' and Cast(substring(Transno,8,5)as Int) = " + TxtEntryNo.Text + " ";

                    Queries[Array_Index++] = Str;

                    Str = "Insert Into ItemStock (UnitId, Itemid, Colorid, sizeid, qty, Rate, joborderNo, TransType, Transno, alloted, ItemCat, processId, sQty, lotNo, balQty, ";
                    Str = Str + " purorprod, transdate, companyid, supplierid, return_qty, uomid, MfrId, Styleid, unit_or_other, ReProg, StockType, remarks, Markup_Rate, StoreId,";
                    Str = Str + " YarnCompId, GSM, ItemSpecId, BeamId, OprGrpId, StockStage, FabricTempMasId, FabricNumberId, Lvalue, StockCatagoryid, UnitPerWt)";
                    Str = Str + " Select 71 UnitId, S1.Itemid, S1.Colorid, S1.sizeid, Sum(S2.Rec_Qty) qty, 0 as Rate, S2.order_no joborderNo, 'PQC' TransType,  'GUP-VSR' + REPLICATE('0',5-LEN(RTRIM(S3.Eno))) + RTRIM(S3.Eno) Transno, ";
                    Str = Str + " 0 alloted, 'B' ItemCat, 158 processId, 0.000 sQty, '' lotNo, Sum(S2.Rec_Qty)balQty, 'RR' purorprod, Cast(S3.EDate As Date) transdate, 93 companyid,";
                    Str = Str + " S3.Supplierid supplierid, 0 as return_qty, 55 as uomid, 0 as MfrId,B1.Styleid Styleid, '' unit_or_other, Null ReProg, 'S' StockType, '' remarks, ";
                    Str = Str + " 0 as Markup_Rate, 6 StoreId, 0 YarnCompId, 0.000 GSM, 0 ItemSpecId, 0 BeamId, 0 OprGrpId,  '' StockStage, NUll FabricTempMasId, Null FabricNumberId, ";
                    Str = Str + " 100 Lvalue, Null StockCatagoryid, 0 UnitPerWt From Socks_Dyeing_Receipt_Details S1 Left Join Socks_Dyeing_OrderwiseReceipt_details S2 On S1.Master_ID = s2.Master_ID And S1.Slno1 = S2.Slno1";
                    Str = Str + " Left Join Socks_Dyeing_Receipt_Master S3 On S1.Master_ID = S3.RowID And S2.Master_ID = S3.RowID Left Join buy_ord_style B1 On S2.order_no = B1.order_no";
                    Str = Str + " Where S3.ENo = " + TxtEntryNo.Text.ToString()  + " Group By Itemid, Colorid, sizeid, S2.order_no, 'GUP-VSR' + REPLICATE('0',5-LEN(RTRIM(S3.Eno))) + RTRIM(S3.Eno) , Cast(S3.EDate As Date), S3.supplierid, B1.Styleid";

                    Queries[Array_Index++] = Str;
                }



                if (MyParent._New)
                {
                    MyBase.Run_Identity(false, Queries);
                    //MyBase.Run(Str);
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

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Dyeing Entry - Delete", "Select B.Eno, Cast(B.Edate as Date)Date, F.Supplier, B.Dc_No, Cast(B.DcDate as Date)Dc_Date,  C.Item Yarn, D.Color, E.Size Count, A.Rec_Qty, B.Remarks, B.Supplierid, A.Itemid, A.Colorid, A.Sizeid, B.Rowid   from fitsocks.dbo.Socks_Dyeing_Receipt_Details A Left Join fitsocks.dbo.Socks_Dyeing_Receipt_Master B on A.Master_ID = B.Rowid Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid Left Join fitsocks.dbo.supplier F on B.Supplierid = F.Supplierid Where B.Eno not in(Select Distinct Cast(substring(A.Transno,8,5)as Int)Eno from Itemstock A Inner Join Item_stock_outward B on A.StockId = B.Itemstockid Where A.Transno like '%vsr%')", String.Empty, 80, 100, 150, 100, 100, 150, 200, 100, 100, 150);
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
                    MyBase.Run("Delete from ItemStock where Transno like 'GUP-VSR%' and Cast(substring(Transno,8,5)as Int) = " + TxtEntryNo.Text ,"Delete from Socks_Dyeing_OrderwiseReceipt_details where Master_ID = " + Code, "Delete from Socks_Dyeing_Receipt_Details where Master_ID = " + Code, "Delete From Socks_Dyeing_Receipt_Master Where RowID = " + Code, MyParent.EntryLog("Yarn Dyeing Receipt Entry", "DELETE", Code.ToString()));
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

                Code = Convert.ToInt64(Dr["RowID"]);
                TxtEntryNo.Text = Dr["ENo"].ToString();
                TxtSupplierDc.Text = Dr["Dc_No"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["Date"]);
                DcDate.Value = Convert.ToDateTime(Dr["Dc_Date"]);                
                TxtSupplier.Tag = Dr["Supplierid"].ToString();
                TxtSupplier.Text = Dr["Supplier"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();
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
                    Str = "select 0 as Slno, '' Delivery_No, Item Yarn,  Color, Size Count, 0.000 Iss_Qty, 0.000 Rec_Qty, '' Remarks, Itemid, Colorid, Sizeid,0 Slno1, 0 RNo,'-' T  from FITSOCKS.dbo.Yarn_Dyeing_Requirement_Details() where 1=2 Group By Itemid, Item, Colorid, Color, Sizeid, Size";
                }
                else
                {
                    Str = "Select A.Slno, A.Delivery_No, C.Item Yarn, D.Color, E.Size Count, A.Rec_Qty Iss_Qty, A.Rec_Qty,  A.Remarks, A.Itemid, A.Colorid, A.Sizeid, A.Slno1, ROW_NUMBER() Over (Order by A.Itemid, A.Colorid, A.Sizeid) RNo,'-' T  from fitsocks.dbo.Socks_Dyeing_Receipt_Details A Left Join fitsocks.dbo.Socks_Dyeing_Receipt_Master B on A.Master_ID = B.Rowid Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid Where B.Eno = '" + TxtEntryNo.Text + "' Order By A.Slno ";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Designing(ref Grid, ref Dt, "ItemID", "SizeID", "ColorID", "Slno1", "RNo", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Delivery_No", "Rec_Qty", "Remarks");
                MyBase.Grid_Width(ref Grid, 50, 100, 150, 200, 100, 100, 100, 200);
                Grid.Columns["Rec_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                Grid.Columns["Iss_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Yarn"].Index)
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Delivery_No"].Index)
                    {
                        if (TxtSupplier.Text != String.Empty)
                        {
                            Dr = Tool.Selection_Tool_Except_New("RNo", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Item", "Select A.Delivery_No, A.Item, A.Color, A.Size, (isnull(A.Issued,0)-isnull(B.Rec_Qty,0))Iss_Qty, (isnull(A.Issued,0)-isnull(B.Rec_Qty,0))Rec_Qty, A.Remarks, A.Itemid, A.Colorid, A.Sizeid, ROW_NUMBER() Over (Order by A.Delivery_No, A.Itemid, A.Colorid, A.Sizeid) RNo   from Delivery_No_Wise_Issued()A Left Join Delivery_No_Wise_Received()B on A.Delivery_No = B.Delivery_No and A.Itemid = B.Itemid and A.Colorid = B.Colorid and A.Sizeid = B.Sizeid Where A.Supplierid= "+TxtSupplier.Tag.ToString()+" and (isnull(A.Issued,0)-isnull(B.Rec_Qty,0))>0", String.Empty, 100, 200, 150, 100, 100, 100, 200);

                            if (Dr != null)
                            {
                                Txt.Text = Dr["Delivery_No"].ToString();
                                Grid["Delivery_No", Grid.CurrentCell.RowIndex].Value = Dr["Delivery_No"].ToString();
                                Grid["Yarn", Grid.CurrentCell.RowIndex].Value = Dr["ITEM"].ToString();
                                Grid["Count", Grid.CurrentCell.RowIndex].Value = Dr["SIZE"].ToString();
                                Grid["COLOR", Grid.CurrentCell.RowIndex].Value = Dr["COLOR"].ToString();                                
                                Grid["Rec_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Rec_Qty"].ToString();
                                Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Iss_Qty"].ToString();
                                Grid["Remarks", Grid.CurrentCell.RowIndex].Value = Dr["Remarks"].ToString();
                                Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                                Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                                Grid["ColorID", Grid.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();
                                Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rec_Qty"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rec_Qty"].Index)
                {
                    if ((Grid["Rec_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) != String.Empty)
                    {
                        if (Convert.ToDouble(Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value) < Convert.ToDouble(Txt.Text))
                        {
                            MessageBox.Show("Invalid Rec_Qty..!", "Gainup");                            
                            Grid["Rec_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                            Txt.Text = "0.000";
                            Grid.CurrentCell = Grid["Rec_Qty", Grid.CurrentCell.RowIndex];
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

                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Rec_Qty"].Index)
                {
                    if ((GridDetail["Rec_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString()) != String.Empty)
                    {
                        if (Convert.ToDouble(GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value) < Convert.ToDouble(GridDetail["Rec_Qty", GridDetail.CurrentCell.RowIndex].Value))
                        {
                            MessageBox.Show("Invalid Rec_Qty..!", "Gainup");
                            GridDetail["Rec_Qty", GridDetail.CurrentCell.RowIndex].Value = "0.000";
                            GridDetail.CurrentCell = GridDetail["Rec_Qty", GridDetail.CurrentCell.RowIndex];
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
                TxtTotal.Text = MyBase.Sum(ref Grid, "Rec_Qty");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void GridDetail_Data(Int32 Row, Int64 Delivery_No, Int32 Rec_Qty, Int64 Item, Int64 Color, Int64 Size)
        {

            try
            {
                if (DtQty[Row] == null)
                {
                    DtQty[Row] = new DataTable();
                    if (MyParent._New)
                    {
                        MyBase.Load_Data("select 0 SNo, '0' Order_No, 0.000 Iss_Qty, 0.000 Rec_Qty, " + Row + " SlNo1, '' T from Yarn_Dyeing_Requirement_Details() where 1=2 ", ref DtQty[Row]);
                    }
                    else
                    {
                        MyBase.Load_Data("select A.slno Sno, A.Order_No, A.Rec_Qty Iss_Qty, A.Rec_Qty, B.Slno1,'' T from Socks_Dyeing_OrderwiseReceipt_details A Left Join Socks_Dyeing_Receipt_Details B on A.Master_ID = B.Master_ID and A.SlNo1 = B.Slno1 Left Join Socks_Dyeing_Receipt_Master C on A.Master_ID = C.RowID and B.Master_ID = C.RowID  Where  A.Master_ID =  " + Code + " and  B.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString(), ref DtQty[Row]);
                    }
                }
                GridDetail.DataSource = DtQty[Row];
                MyBase.Grid_Designing(ref GridDetail, ref DtQty[Row], "SlNo1", "T");
                MyBase.ReadOnly_Grid_Without(ref GridDetail, "Rec_Qty", "Order_No");
                MyBase.Grid_Colouring(ref GridDetail, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridDetail, 50, 150, 100, 100);
                GridDetail.Columns["Rec_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridDetail.Columns["Iss_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                GridDetail.RowHeadersWidth = 10;

                if (!MyParent._New)
                {
                    
                }

                GBQty.Visible = true;

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
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Rec_Qty"].Index)
                    {
                        if (GridDetail["Rec_Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Rec_Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Rec_Qty", GridDetail.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Rec_Qty...!", "Gainup");
                            GridDetail.CurrentCell = GridDetail["Rec_Qty", Grid.CurrentCell.RowIndex];
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
        void TxtIss_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Order_No"].Index)
                    {
                        Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select A.Order_No, (Isnull(A.Iss_Qty,0)-Isnull(B.Rec_Qty,0))Iss_Qty, (Isnull(A.Iss_Qty,0)-Isnull(B.Rec_Qty,0))Rec_Qty ,A.Itemid, A.Colorid, A.Sizeid  from Orderwise_Dyeing_Issued()A Left Join Orderwise_Dyeing_Received()B on A.Delivery_No = B.Delivery_No and A.Order_no = B.Order_no and A.Itemid = B.Itemid and A.Colorid = B.Colorid and A.Sizeid = B.Sizeid Where A.Delivery_No = " + Delivery_No + " and A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " and (Isnull(A.Iss_Qty,0)-Isnull(B.Rec_Qty,0))>0 Order By A.Order_No", String.Empty, 150, 100, 100);

                        if (Dr != null)
                        {
                            Txt1.Text = Dr["Order_No"].ToString();
                            GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                            GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value = Dr["Iss_Qty"].ToString();
                            GridDetail["Rec_Qty", GridDetail.CurrentCell.RowIndex].Value = Dr["Rec_Qty"].ToString();
                            GridDetail["SlNo1", GridDetail.CurrentCell.RowIndex].Value = Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();

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
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Rec_Qty"].Index)
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
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Rec_Qty"].Index)
                {
                    if (GridDetail["Rec_Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Rec_Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Rec_Qty", GridDetail.CurrentCell.RowIndex].Value) == 0)
                    {
                        
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

                TxtEnteredWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum_With_Three_Digits(ref GridDetail, "Rec_Qty")));

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
                    GridDetail.CurrentCell = GridDetail["Rec_Qty", 0];
                    GridDetail.Focus();
                    GridDetail.BeginEdit(true);
                    return;
                }                
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Delivery_No", (Grid.CurrentCell.RowIndex + 1)];
                Grid.Focus();
                Grid.BeginEdit(true);
                return;

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
                    if (GridDetail["Rec_Qty", i].Value == DBNull.Value || Convert.ToDouble(GridDetail["Rec_Qty", i].Value) != 0)
                    {
                        MessageBox.Show("Invalid KGS ..!", "Gainup");
                        Grid.CurrentCell = Grid["Rec_Qty", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        GBQty.Visible = false;
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                DtQty = new DataTable[30];
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Rec_Qty", (Grid.CurrentCell.RowIndex)];
                Grid.Focus();
                Grid.BeginEdit(true);
                return;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Socks_Dyeing_Receipt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    
                    if (this.ActiveControl.Name == "TxtSupplier")
                    {
                        if (TxtSupplier.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Select Supplier..!", "Gainup");
                            return;
                        }
                        else
                        {
                            TxtSupplierDc.Focus();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtSupplierDc")
                    {
                        DcDate.Focus();                        
                        return;
                    }
                    else if (this.ActiveControl.Name == "DcDate")
                    {
                        Grid.CurrentCell = Grid["Delivery_No", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;

                    }
                    else if (this.ActiveControl.Name == "TxtTotal")
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
                    if (this.ActiveControl.Name == "TxtSupplier")
                    {

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier..!", "Select Distinct upper(A.Supplier)Supplier, A.Supplierid From fitsocks.dbo.Supplier A Inner Join Socks_Dyeing_Master B on A.supplierid = B.SupplierId", String.Empty, 300);

                        if (Dr != null)
                        {
                            TxtSupplier.Text = Dr["Supplier"].ToString();
                            TxtSupplier.Tag = Dr["Supplierid"].ToString();

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

                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
                    {

                        TxtQty1.Text = Grid["Rec_Qty", Grid.CurrentCell.RowIndex].Value.ToString();
                                                
                        ItemID = Convert.ToInt64(Grid["ItemId", Grid.CurrentCell.RowIndex].Value);
                        ColorID = Convert.ToInt64(Grid["ColorId", Grid.CurrentCell.RowIndex].Value);
                        SizeID = Convert.ToInt64(Grid["SizeId", Grid.CurrentCell.RowIndex].Value);
                        Delivery_No = Convert.ToInt32(Grid["Delivery_No", Grid.CurrentCell.RowIndex].Value);

                        GridDetail_Data(Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value), Convert.ToInt32(Grid["Rec_Qty", Grid.CurrentCell.RowIndex].Value), Delivery_No, ItemID, ColorID, SizeID);
                        GridDetail.CurrentCell = GridDetail["Order_No", 0];
                        GridDetail.Focus();
                        GridDetail.BeginEdit(true);
                        e.Handled = true;
                        return;

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
                MyBase.Grid_Delete(ref GridDetail, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], GridDetail.CurrentCell.RowIndex);
                DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].AcceptChanges();
                MyBase.Row_Number(ref GridDetail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Frm_Socks_Dyeing_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtSupplier" || this.ActiveControl.Name == "TxtTotal")
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
