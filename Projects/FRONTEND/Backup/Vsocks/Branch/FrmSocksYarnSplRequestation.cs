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
    public partial class FrmSocksYarnSplRequestation : Form, Entry
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
        int st = 0;

        Int16 Vis = 0;
        int Pos = 0;

        public FrmSocksYarnSplRequestation()
        {
            InitializeComponent();
        }

        private void FrmSocksYarnSplRequestation_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                st = 0;
                TxtOrderNo.Focus();
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
                TxtOrderNo.Focus();
                Grid_Data();
                DtQty = new DataTable[300];
                return;
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
                Code = Convert.ToInt64(Dr["Spl_Reqid"]);
                TxtEntryNo.Text = Dr["Spl_Req_No"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["Spl_Req_Date"]);
                TxtOrderNo.Text = Dr["Order_No"].ToString();
                TxtBuyer.Text = Dr["Buyer"].ToString();
                TxtBuyer.Tag = Dr["BuyerID"].ToString();
                TxtReason.Text = Dr["Reasons"].ToString();
                TxtReason.Tag = Dr["ReasonId"].ToString();
                TxtStyle.Text = Dr["Style"].ToString();
                TxtStyle.Tag = Dr["StyleID"].ToString();
                TxtUnit.Text = Dr["Company_Unit"].ToString();
                TxtUnit.Tag = Dr["CompanyUnitid"].ToString();
                TxtRefNo.Text = Dr["Ref_No"].ToString();
                TxtJobOrderNo.Text = Dr["Job_Ord_No"].ToString();
                TxtRemarks.Text = Dr["Req_Remarks"].ToString();
                Grid_Data();
                Total_Count();
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
                DtQty = new DataTable[300];
                
                //Str = " Select A.Spl_Req_No, A.Spl_Req_Date, A.Ref_No Order_No, B.Buyer, A.Job_Ord_No, C.Reasons, D.Company_Unit, B.Style, A.Ref_No, ";
                //Str = Str + " A.ReasonId, A.CompanyUnitid, B.StyleID, A.Spl_Reqid, B.BuyerID, A.Req_Remarks from Special_Req_Mas A Left Join (Select Distinct Order_No, Buyer, BuyerID, Item Style, ItemID StyleID from Socks_Bom())B On A.Ref_No = B.Order_No ";
                //Str = Str + " Left Join Reasons C On A.ReasonId = C.Reasonid Left Join company_unit D On A.CompanyUnitid = D.Company_UnitID Where App_By Is Null And Cast(Replace(Spl_Req_No, 'GUP-SRQ', '') As Numeric(20)) > 109 ";
                
                Str = " Select Distinct A.Spl_Req_No, A.Spl_Req_Date, A.Ref_No Order_No, B.Party Buyer, A.Job_Ord_No, C.Reasons, D.Company_Unit, B.Ord_Item Style, A.Ref_No, ";
                Str = Str + " A.ReasonId, A.CompanyUnitid, B.Ord_ItemId StyleID, A.Spl_Reqid, B.BuyerID, A.Req_Remarks from Special_Req_Mas A  Left Join Socks_Yarn_Planning_Fn()B On A.Job_Ord_No = B.Order_No ";
                Str = Str + " Left Join Reasons C On A.ReasonId = C.Reasonid Left Join company_unit D On A.CompanyUnitid = D.Company_UnitID ";
                Str = Str + " Where App_By Is Null And Cast(Replace(Spl_Req_No, 'GUP-SRQ', '') As Numeric(20)) > 108 ";

                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Special Requestation - Edit", Str, String.Empty, 100, 100, 150, 350, 100, 100, 100);

                //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Special Requestation - Edit", "Select Spl_Req_No, Spl_Req_Date, Ref_No, Job_Ord_No, Spl_Reqid From Special_Req_Mas Where Cast(Replace(Spl_Req_No, 'GUP-SRQ', '') As Numeric(20)) > 105 ", String.Empty, 100, 100, 150, 350, );
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Rate", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
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
                if (MyParent._New == true)
                {
                    if (st == 0)
                    {
                        Str = "Select 0 Slno, Order_No, Item, Color, Size, Itemid, Colorid, Sizeid, Knit_Req, 0.00 Rate, 0.000 Req_Qty, 0.000 Tfr_Qty, 0 Slno1, 0 Slno_Temp, 'N' Replace, 'O' Record, '-' Remarks, 0.000 Entered_Qty, '' T From Base_For_Yarn_Spl_Req()Where 1 = 2 ";
                    }
                    else
                    {
                        //Str = "Select 0 Slno, Order_No, Item, Color, Size, Itemid, Colorid, Sizeid, Knit_Req, 0.000 Req_Qty, 0.000 Tfr_Qty, 0 Slno1, 0 Slno_Temp, 'N' Replace, 'O' Record, '' T From Base_For_Yarn_Spl_Req()Where Order_No = '" + TxtOrderNo.Text + "' Order By Order_No, Item, Color, Size";

                        //Str = " Select 0 Slno, Order_No, Item, Color, Size, Itemid, Colorid, Sizeid, Knit_Req, 0.00 Rate, 0.000 Req_Qty, 0.000 Tfr_Qty, 0 Slno1, 0 Slno_Temp, 'N' Replace, 'O' Record, '' T From ";
                        //Str = Str + " (Select Order_No, Item, Color, Size, Itemid, Colorid, Sizeid, Knit_Req From Base_For_Yarn_Spl_Req() Where Order_No = '" + TxtOrderNo.Text + "' Union All ";
                        //Str = Str + " Select A.Order_No, B.Item, C.Color, D.Size, A.ItemID, A.ColorID, A.SizeID, A.Knit_Req_Qty Knit_Req From VSocks_Samplewise_All()A ";
                        //Str = Str + " Left Join Item B On A.ItemID = B.ItemID Left Join Color C On A.ColorID = C.ColorID Left Join Size D On A.SizeID = D.SizeID Where Order_No = '" + TxtOrderNo.Text + "' And A.Unit_Code = " + TxtUnit.Tag + ")A Order By Item, Color, Size ";

                        //Str = " Select 0 Slno, Order_No, Item, Color, Size, ItemID, ColorID, SizeID, Req Knit_Req, 0.00 Rate, 0.000 Req_Qty, 0.000 Tfr_Qty, 0 Slno1, ";
                        //Str = Str + " 0 Slno_Temp, 'N' Replace, 'O' Record, '' T From Dyed_Yarn_Status() Where Order_No = '" + TxtOrderNo.Text + "' ";

                        Str = " Select 0 Slno, A.Order_No, A.Item, A.Color, A.Size, A.ItemID, A.ColorID, A.SizeID, A.Req Knit_Req, Cast(Isnull(B.Rate,0) As Numeric(20,2))Rate, 0.000 Req_Qty, 0.000 Tfr_Qty, 0 Slno1,  0 Slno_Temp, 'N' Replace, 'O' Record, '-' Remarks, 0.000 Entered_Qty, '' T From Dyed_Yarn_Status() A ";
                        Str = Str + " Left Join Socks_Budget_App_Rate_Max_Fn()B On A.Itemid = B.Itemid And A.Colorid = B.Colorid And A.SizeID = B.SizeID Where Order_No = '" + TxtOrderNo.Text + "'";
                    }
                }
                else
                {
                    ////Str = "select A.Slno,A.Order_No,B1.Item,C.Color,D.Size,A.No_Of_Bags,A.Grn_Qty,A.Itemid,A.Colorid,A.Sizeid,A.Slno1, A.Slno1 Slno_Temp from Socks_Lot_Details A Left Join Socks_Lot_Master B on B.RowID = A.Master_ID  Left Join Item B1 on A.itemid = B1.itemid Left Join Color C on A.Colorid = C.Colorid Left Join Size D on A.Sizeid = D.Sizeid where B.Grn_No='" + TxtGrnNO.Text + "'";
                    //Str = "Select 0 Slno, A.Order_No, B.Item, C.Color, D.Size, A.ItemID, A.ColorID, A.SizeID, E.Req Knit_Req, SUM(ISNULL(Rate,0))Rate, SUM(Isnull(Req_Qty,0))Req_Qty, SUM(Isnull(Tfr_Qty,0))Tfr_Qty, A.SLno1, A.Slno1 Slno_Temp, ";
                    //Str = Str + " (Case When Isnull(A.Slno1,0) > 0 Then 'Y' Else 'N' End)Replace, 'O' Record, '' T From ";
                    //Str = Str + " (Select A.Job_Ord_No Order_no, Itemid, ColorID, SizeID, B.Rate, (Purchasable_Qty + Transferable_Qty)Req_Qty, Transferable_Qty Tfr_Qty, Slno1 From Special_Req_Mas A ";
                    //Str = Str + " Left Join Special_Req_Det B On A.Spl_Reqid = B.Spl_Reqid ";
                    //Str = Str + " Where B.To_itemid Is NUll And A.Spl_Reqid = " + Code;
                    //Str = Str + " Union All ";
                    //Str = Str + " Select A.Job_Ord_No Order_no, To_itemid Itemid, To_ColorID ColorID, To_Sizeid Sizeid, 0.000 Rate, 0.000 Req_Qty, 0.000 Tfr_Qty, Slno1 From Special_Req_Mas A ";
                    //Str = Str + " Left Join Special_Req_Det B On A.Spl_Reqid = B.Spl_Reqid ";
                    //Str = Str + " Where B.To_itemid Is Not NUll And A.Spl_Reqid = " + Code + ")A ";
                    //Str = Str + " Left Join Item B On A.ItemID = B.ItemID ";
                    //Str = Str + " Left Join Color C On A.ColorID = C.ColorID ";
                    //Str = Str + " Left Join Size D On A.SizeID = D.SizeID ";
                    //Str = Str + " Left Join Dyed_Yarn_Status() E On A.Order_no = E.Order_No And A.Itemid = E.Itemid And A.Colorid = E.Colorid And A.Sizeid = E.Sizeid ";
                    //Str = Str + " Group By A.Order_No, B.Item, C.Color, D.Size, A.ItemID, A.ColorID, A.SizeID, E.Req , A.SLno1 ";
                    //Str = Str + " Order By A.SLno1, A.ItemID, A.ColorID, A.SizeID ";

                    Str = " Select 0 Slno, C.Order_No, E.Item, F.Color, G.Size, C.ItemId, C.ColorID, C.SizeID, H.Req Knit_Req,";
                    Str = Str + " D.Rate, C.Req_Qty, C.Tfr_Qty, (Case When C.SLno1 > 0 Then 'Y' Else 'N' End)Replace, C.Slno1, '-' Remarks, 0.000 Entered_Qty, '' T From ";
                    Str = Str + " (Select B.Spl_Req_Detid, A.Ref_No Order_No, B.ItemID, B.ColorID, B.SizeID, (B.Purchasable_Qty + B.Transferable_Qty)Req_Qty,";
                    Str = Str + " B.Transferable_Qty Tfr_Qty, B.Slno1 From Special_Req_Mas A ";
                    Str = Str + " Left Join Special_Req_Det B On A.Spl_Reqid = B.Spl_Reqid ";
                    Str = Str + " Where A.Spl_Reqid = " + Code + " And To_itemid Is Null ";
                    Str = Str + " Union All ";
                    Str = Str + " Select B.Spl_Req_Detid, A.Ref_No Order_No, B.To_Itemid ItemID, B.To_colorid ColorID, B.To_sizeid SizeID, 0.000 Req_Qty, ";
                    Str = Str + " 0.000 Tfr_Qty, B.Slno1 From Special_Req_Mas A ";
                    Str = Str + " Left Join Special_Req_Det B On A.Spl_Reqid = B.Spl_Reqid ";
                    Str = Str + " Where A.Spl_Reqid = " + Code + " And To_itemid Is Not Null)C ";
                    Str = Str + " Left join Socks_Budget_App_Rate_Max_Fn() D On C.Itemid = D.Itemid And C.Colorid = D.Colorid And C.Sizeid = D.Sizeid ";
                    Str = Str + " Left Join Item E On C.ItemID = E.ItemID Left Join Color F On C.ColorID = F.ColorID Left Join Size G On C.SizeID = G.SizeID ";
                    Str = Str + " Left Join Dyed_Yarn_Status() H On C.Order_no = H.Order_No And C.Itemid = H.Itemid And C.Colorid = H.Colorid And C.Sizeid = H.Sizeid  ";
                    Str = Str + " Order By C.Spl_Req_Detid, C.Slno1 ";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.ReadOnly_Grid_Without(ref Grid, "Rate", "Req_Qty", "Tfr_Qty", "Replace", "Remarks");
                MyBase.Grid_Designing(ref Grid, ref Dt, "Order_No", "ItemID", "SizeID", "ColorID", "Slno1", "Record", "Slno_Temp", "Entered_Qty", "T");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 50, 150, 200, 100, 100, 100, 100, 100);
                Grid.Columns["Rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Req_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Tfr_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Replace"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (!MyParent._New)
                    {
                        Vis = 1;
                        Pos = i;
                        if (Grid["Replace", i].Value.ToString() == "Y")
                        {
                            GridDetail_Data(Convert.ToInt16(Grid["Slno1", i].Value));
                        }
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

        public void Entry_Save()
        {
            try
            {
                Int32 Array_Index = 0;
                String Order_Type = String.Empty;
                Total_Count();

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Details..!", "Gainup");
                    TxtOrderNo.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if ((TxtTotal.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotal.Text) == 0) && (TxtEnter.Text.Trim() == String.Empty || Convert.ToDouble(TxtEnter.Text) == 0))
                {
                    MessageBox.Show("Invalid Details", "Gainup");
                    TxtOrderNo.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 5; j++)
                    {
                        if (Math.Round(Convert.ToDecimal(Grid["Tfr_Qty", i].Value), 3) > Math.Round(Convert.ToDecimal(Grid["Req_Qty", i].Value), 3))
                        {                        
                            MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                            Grid.CurrentCell = Grid[j, i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Replace", i].Value.ToString() == "Y" && DtQty[Convert.ToInt32(Dt.Rows[i]["SlNo1"])] == null)
                    {
                        MessageBox.Show("Invalid Bagwise Breakup Details ...!", "Gainup");
                        MyParent.Save_Error = true;
                        Grid.CurrentCell = Grid["Replace", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                }

                if (MyParent._New)
                {
                    DataTable Is1 = new DataTable();
                    Str = "Select Dbo.Get_Max_Socks_Spl_Req()";
                    MyBase.Load_Data(Str, ref Is1);
                    if (Is1.Rows.Count > 0)
                    {
                        TxtEntryNo.Text = Is1.Rows[0][0].ToString();
                    }
                }
                
                Queries = new string[Dt.Rows.Count + 100000];
                
                
                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Special_Req_Mas (Spl_Req_No, Spl_Req_Date, Ref_No, Ref_Date, Job_Ord_No, Companyid, CompanyUnitid, Req_Remarks, Req_Commit_Cancel, App_By, App_Date, App_COmmit_Cancel, App_Remarks, Auto_Manual, OrderType, Unit_Or_Other, Type, ReasonId, DefectBy, DefectEmp_SuppId, ReqVal, StyleID) values ('" + TxtEntryNo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + TxtRefNo.Text.ToString() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + TxtJobOrderNo.Text.ToString() + "', 93, " + TxtUnit.Tag + ", '" + TxtRemarks.Text + "', 'N', NULL, NULL, NULL, NULL, 'A', NULL, NULL, 'W', " + TxtReason.Tag + ", 'E', NULL, 0.000, " + TxtStyle.Tag + "); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("Special Requestation", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Special_Req_Mas Set Spl_Req_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', Ref_No = '" + TxtRefNo.Text.ToString() + "', Ref_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', Job_Ord_No = '" + TxtJobOrderNo.Text.ToString() + "', CompanyUnitid = " + TxtUnit.Tag + ", Req_Remarks = '" + TxtRemarks.Text + "', ReasonId = " + TxtReason.Tag + ", StyleID = " + TxtStyle.Tag + " Where Spl_ReqId = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Special Requestation", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from Special_Req_Det where Spl_ReqId = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        if (Grid["Replace", i].Value.ToString() == "N" && Math.Round(Convert.ToDecimal(Grid["Req_Qty", i].Value.ToString()), 3) > 0 && Math.Round(Convert.ToDecimal(Grid["Tfr_Qty", i].Value.ToString()), 3) >= 0)
                        {
                            Queries[Array_Index++] = "Insert into Special_Req_Det (Spl_ReqId, ItemID, ColorID, SizeID, UOMId, Quantity, App_Qty, Issue_Qty, ReqType, TransferIn, Order_Qty, Received_Qty, Cancel_Qty, Purchasable_Qty, Transferable_Qty, To_ItemID, To_ColorID, To_SizeID, Slno1, Rate) Values (@@IDENTITY, " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + " , " + Grid["SizeID", i].Value + ", 55, " + Grid["Req_Qty", i].Value + ", NULL, 0.000, 'A', NULL, 0.000, 0.000, 0.000, (Cast(" + Grid["Req_Qty", i].Value + " As Numeric(25, 3)) - Cast(" + Grid["Tfr_Qty", i].Value + " As Numeric(25, 3))), Cast(" + Grid["Tfr_Qty", i].Value + " As Numeric(25, 3)), NULL, NULL, NULL, " + Grid["Slno1", i].Value + ", " + Grid["Rate", i].Value + ")";
                        }
                        else if (Grid["Replace", i].Value.ToString() == "Y" && Math.Round(Convert.ToDecimal(Grid["Req_Qty", i].Value.ToString()), 3) == 0 && Math.Round(Convert.ToDecimal(Grid["Tfr_Qty", i].Value.ToString()), 3) == 0)
                        {
                            if (DtQty[i + 1] != null)
                            {
                                for (int j = 0; j <= DtQty[i + 1].Rows.Count - 1; j++)
                                {
                                    if (MyParent._New)
                                    {
                                        Queries[Array_Index++] = "Insert into Special_Req_Det (Spl_ReqId, ItemID, ColorID, SizeID, UOMId, Quantity, App_Qty, Issue_Qty, ReqType, TransferIn, Order_Qty, Received_Qty, Cancel_Qty, Purchasable_Qty, Transferable_Qty, To_ItemID, To_ColorID, To_SizeID, Slno1, Rate) Values (@@IDENTITY, " + DtQty[i + 1].Rows[j]["ItemID"].ToString() + ", " + DtQty[i + 1].Rows[j]["ColorID"].ToString() + ", " + DtQty[i + 1].Rows[j]["SizeID"].ToString() + ", 55, Cast(" + Grid["Req_Qty", i].Value + " As Numeric(25, 3)), NULL, 0.000, 'A', NULL, 0.000, 0.000, 0.000, (Cast(" + DtQty[i + 1].Rows[j]["Req_Qty"].ToString() + " As Numeric(25, 3)) - Cast(" + DtQty[i + 1].Rows[j]["Tfr_Qty"].ToString() + " As Numeric(25, 3))), Cast(" + DtQty[i + 1].Rows[j]["Tfr_Qty"].ToString() + " As Numeric(25, 3)), " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + " , " + Grid["SizeID", i].Value + ", " + Grid["Slno1", i].Value + ", Cast(" + DtQty[i + 1].Rows[j]["Rate"].ToString() + " As Numeric(25, 2)))";
                                    }
                                }
                            }
                        }
                        else if (Grid["Replace", i].Value.ToString() == "Y" && Math.Round(Convert.ToDecimal(Grid["Req_Qty", i].Value.ToString()), 3) > 0 && Math.Round(Convert.ToDecimal(Grid["Tfr_Qty", i].Value.ToString()), 3) >= 0)
                        {
                            if (DtQty[i + 1] != null)
                            {
                                for (int j = 0; j <= DtQty[i+1].Rows.Count - 1; j++)
                                {
                                    if (MyParent._New)
                                    {
                                        Queries[Array_Index++] = "Insert into Special_Req_Det (Spl_ReqId, ItemID, ColorID, SizeID, UOMId, Quantity, App_Qty, Issue_Qty, ReqType, TransferIn, Order_Qty, Received_Qty, Cancel_Qty, Purchasable_Qty, Transferable_Qty, To_ItemID, To_ColorID, To_SizeID, Slno1, Rate) Values (@@IDENTITY, " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + " , " + Grid["SizeID", i].Value + ", 55, " + Grid["Req_Qty", i].Value + ", NULL, 0.000, 'A', NULL, 0.000, 0.000, 0.000, (Cast(" + Grid["Req_Qty", i].Value + " As Numeric(25, 3)) - Cast(" + Grid["Tfr_Qty", i].Value + " As Numeric(25, 3))), Cast(" + Grid["Tfr_Qty", i].Value + " As Numeric(25, 3)), NULL, NULL, NULL, " + Grid["Slno1", i].Value + ", " + Grid["Rate", i].Value + ")";
                                        Queries[Array_Index++] = "Insert into Special_Req_Det (Spl_ReqId, ItemID, ColorID, SizeID, UOMId, Quantity, App_Qty, Issue_Qty, ReqType, TransferIn, Order_Qty, Received_Qty, Cancel_Qty, Purchasable_Qty, Transferable_Qty, To_ItemID, To_ColorID, To_SizeID, Slno1, Rate) Values (@@IDENTITY, " + DtQty[i + 1].Rows[j]["ItemID"].ToString() + ", " + DtQty[i + 1].Rows[j]["ColorID"].ToString() + ", " + DtQty[i + 1].Rows[j]["SizeID"].ToString() + ", 55, " + Grid["Req_Qty", i].Value + ", NULL, 0.000, 'A', NULL, 0.000, 0.000, 0.000, (Cast(" + DtQty[i + 1].Rows[j]["Req_Qty"].ToString() + " As Numeric(25, 3)) - Cast(" + DtQty[i + 1].Rows[j]["Tfr_Qty"].ToString() + " As Numeric(25, 3))), Cast(" + DtQty[i + 1].Rows[j]["Tfr_Qty"].ToString() + " As Numeric(25, 3)), " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + " , " + Grid["SizeID", i].Value + ", " + Grid["Slno1", i].Value + ", Cast(" + DtQty[i + 1].Rows[j]["Rate"].ToString() + " As Numeric(25, 2)))";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Grid["Replace", i].Value.ToString() == "N" && Math.Round(Convert.ToDecimal(Grid["Req_Qty", i].Value.ToString()), 3) > 0 && Math.Round(Convert.ToDecimal(Grid["Tfr_Qty", i].Value.ToString()), 3) >= 0)
                        {
                            Queries[Array_Index++] = "Insert into Special_Req_Det (Spl_ReqId, ItemID, ColorID, SizeID, UOMId, Quantity, App_Qty, Issue_Qty, ReqType, TransferIn, Order_Qty, Received_Qty, Cancel_Qty, Purchasable_Qty, Transferable_Qty, To_ItemID, To_ColorID, To_SizeID, Slno1, Rate) Values (" + Code + ", " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + " , " + Grid["SizeID", i].Value + ", 55, " + Grid["Req_Qty", i].Value + ", NULL, 0.000, 'A', NULL, 0.000, 0.000, 0.000, (Cast(" + Grid["Req_Qty", i].Value + " As Numeric(25, 3)) - Cast(" + Grid["Tfr_Qty", i].Value + " As Numeric(25, 3))), Cast(" + Grid["Tfr_Qty", i].Value + " As Numeric(25, 3)), NULL, NULL, NULL, " + Grid["Slno1", i].Value + ", " + Grid["Rate", i].Value + ")";
                        }
                        else if (Grid["Replace", i].Value.ToString() == "Y" && Math.Round(Convert.ToDecimal(Grid["Req_Qty", i].Value.ToString()), 3) == 0 && Math.Round(Convert.ToDecimal(Grid["Tfr_Qty", i].Value.ToString()), 3) == 0)
                        {
                            if (DtQty[Convert.ToInt16(Grid["Slno1", i].Value)] != null)
                            {
                                for (int j = 0; j <= DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows.Count - 1; j++)
                                {
                                    Queries[Array_Index++] = "Insert into Special_Req_Det (Spl_ReqId, ItemID, ColorID, SizeID, UOMId, Quantity, App_Qty, Issue_Qty, ReqType, TransferIn, Order_Qty, Received_Qty, Cancel_Qty, Purchasable_Qty, Transferable_Qty, To_ItemID, To_ColorID, To_SizeID, Slno1, Rate) Values (" + Code + ", " + DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows[j]["ItemID"].ToString() + ", " + DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows[j]["ColorID"].ToString() + ", " + DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows[j]["SizeID"].ToString() + ", 55, Cast(" + Grid["Req_Qty", i].Value + " As Numeric(25, 3)), NULL, 0.000, 'A', NULL, 0.000, 0.000, 0.000, (Cast(" + DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows[j]["Req_Qty"].ToString() + " As Numeric(25, 3)) - Cast(" + DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows[j]["Tfr_Qty"].ToString() + " As Numeric(25, 3))), Cast(" + DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows[j]["Tfr_Qty"].ToString() + " As Numeric(25, 3)), " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + " , " + Grid["SizeID", i].Value + ", " + Grid["Slno1", i].Value + ", Cast(" + DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows[j]["Rate"].ToString() + " As Numeric(25, 2)))";
                                }
                            }
                        }
                        else if (Grid["Replace", i].Value.ToString() == "Y" && Math.Round(Convert.ToDecimal(Grid["Req_Qty", i].Value.ToString()), 3) > 0 && Math.Round(Convert.ToDecimal(Grid["Tfr_Qty", i].Value.ToString()), 3) >= 0)
                        {
                            if (DtQty[Convert.ToInt16(Grid["Slno1", i].Value)] != null)
                            {
                                for (int j = 0; j <= DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows.Count - 1; j++)
                                {
                                    Queries[Array_Index++] = "Insert into Special_Req_Det (Spl_ReqId, ItemID, ColorID, SizeID, UOMId, Quantity, App_Qty, Issue_Qty, ReqType, TransferIn, Order_Qty, Received_Qty, Cancel_Qty, Purchasable_Qty, Transferable_Qty, To_ItemID, To_ColorID, To_SizeID, Slno1, Rate) Values (" + Code + ", " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + " , " + Grid["SizeID", i].Value + ", 55, " + Grid["Req_Qty", i].Value + ", NULL, 0.000, 'A', NULL, 0.000, 0.000, 0.000, (Cast(" + Grid["Req_Qty", i].Value + " As Numeric(25, 3)) - Cast(" + Grid["Tfr_Qty", i].Value + " As Numeric(25, 3))), Cast(" + Grid["Tfr_Qty", i].Value + " As Numeric(25, 3)), NULL, NULL, NULL, " + Grid["Slno1", i].Value + ", " + Grid["Rate", i].Value + ")";
                                    Queries[Array_Index++] = "Insert into Special_Req_Det (Spl_ReqId, ItemID, ColorID, SizeID, UOMId, Quantity, App_Qty, Issue_Qty, ReqType, TransferIn, Order_Qty, Received_Qty, Cancel_Qty, Purchasable_Qty, Transferable_Qty, To_ItemID, To_ColorID, To_SizeID, Slno1, Rate) Values (" + Code + ", " + DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows[j]["ItemID"].ToString() + ", " + DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows[j]["ColorID"].ToString() + ", " + DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows[j]["SizeID"].ToString() + ", 55, " + Grid["Req_Qty", i].Value + ", NULL, 0.000, 'A', NULL, 0.000, 0.000, 0.000, (Cast(" + DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows[j]["Req_Qty"].ToString() + " As Numeric(25, 3)) - Cast(" + DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows[j]["Tfr_Qty"].ToString() + " As Numeric(25, 3))), Cast(" + DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows[j]["Tfr_Qty"].ToString() + " As Numeric(25, 3)), " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + " , " + Grid["SizeID", i].Value + ", " + Grid["Slno1", i].Value + ", Cast(" + DtQty[Convert.ToInt16(Grid["Slno1", i].Value)].Rows[j]["Rate"].ToString() + " As Numeric(25, 2)))";
                                }
                            }
                        }                    
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
                st = 0;
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
                //Str = " Select A.Spl_Req_No, A.Spl_Req_Date, A.Ref_No Order_No, B.Buyer, A.Job_Ord_No, C.Reasons, D.Company_Unit, B.Style, A.Ref_No, ";
                //Str = Str + " A.ReasonId, A.CompanyUnitid, B.StyleID, A.Spl_Reqid, B.BuyerID, A.Req_Remarks from Special_Req_Mas A Left Join (Select Distinct Order_No, Buyer, BuyerID, Item Style, ItemID StyleID from Socks_Bom())B On A.Ref_No = B.Order_No ";
                //Str = Str + " Left Join Reasons C On A.ReasonId = C.Reasonid Left Join company_unit D On A.CompanyUnitid = D.Company_UnitID Where Cast(Replace(Spl_Req_No, 'GUP-SRQ', '') As Numeric(20)) > 103 ";

                Str = " Select Distinct A.Spl_Req_No, A.Spl_Req_Date, A.Ref_No Order_No, B.Party Buyer, A.Job_Ord_No, C.Reasons, D.Company_Unit, B.Ord_Item Style, A.Ref_No, ";
                Str = Str + " A.ReasonId, A.CompanyUnitid, B.Ord_ItemId StyleID, A.Spl_Reqid, B.BuyerID, A.Req_Remarks from Special_Req_Mas A  Left Join Socks_Yarn_Planning_Fn()B On A.Job_Ord_No = B.Order_No ";
                Str = Str + " Left Join Reasons C On A.ReasonId = C.Reasonid Left Join company_unit D On A.CompanyUnitid = D.Company_UnitID ";
                Str = Str + " Where App_By Is Null And Cast(Replace(Spl_Req_No, 'GUP-SRQ', '') As Numeric(20)) > 108 ";
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Special Requestation - Edit", Str, String.Empty, 100, 100, 150, 350, 100, 100, 100);

                //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Lot Entry - Delete", "select A.ENo, A.EDate, A.Grn_No, B.supplier, D.item, E.color, F.size, C.Grn_Qty, C.No_Of_Bags, A.Remarks, A.Supplierid, A.Rowid from Socks_Lot_Master A Left Join FITSOCKS.dbo.Supplier B on A.Supplierid = B.Supplierid Left Join Socks_Lot_Details C on A.RowID = C.Master_ID Left Join Item D on C.ItemID = D.itemid Left Join Color E on C.ColorID = E.Colorid Left Join Size F on C.SizeID = F.sizeid ", String.Empty, 100, 100, 150, 350, 200, 150, 100, 100, 100, 350);
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
                    MyBase.Run("Delete from Special_Req_Det where Spl_ReqId = " + Code, "Delete from Special_Req_Mas where Spl_ReqId = " + Code, MyParent.EntryLog("Lot Entry", "DELETE", Code.ToString()));
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


        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                DtQty = new DataTable[300];
                //Str = " Select A.Spl_Req_No, A.Spl_Req_Date, A.Ref_No Order_No, B.Buyer, A.Job_Ord_No, C.Reasons, D.Company_Unit, B.Style, A.Ref_No, ";
                //Str = Str + " A.ReasonId, A.CompanyUnitid, B.StyleID, A.Spl_Reqid, B.BuyerID, A.Req_Remarks from Special_Req_Mas A Left Join (Select Distinct Order_No, Buyer, BuyerID, Item Style, ItemID StyleID from Socks_Bom())B On A.Ref_No = B.Order_No ";
                //Str = Str + " Left Join Reasons C On A.ReasonId = C.Reasonid Left Join company_unit D On A.CompanyUnitid = D.Company_UnitID Where Cast(Replace(Spl_Req_No, 'GUP-SRQ', '') As Numeric(20)) > 103 ";

                //Str = " Select Distinct A.Spl_Req_No, A.Spl_Req_Date, A.Ref_No Order_No, B.Party Buyer, A.Job_Ord_No, C.Reasons, D.Company_Unit, B.Ord_Item Style, A.Ref_No, ";
                //Str = Str + " A.ReasonId, A.CompanyUnitid, B.Ord_ItemId StyleID, A.Spl_Reqid, B.BuyerID, A.Req_Remarks from Special_Req_Mas A  Left Join Socks_Yarn_Planning_Fn()B On A.Job_Ord_No = B.Order_No ";
                //Str = Str + " Left Join Reasons C On A.ReasonId = C.Reasonid Left Join company_unit D On A.CompanyUnitid = D.Company_UnitID ";
                //Str = Str + " Where Cast(Replace(Spl_Req_No, 'GUP-SRQ', '') As Numeric(20)) > 108 ";
                
                Str = " Select Distinct A.Spl_Req_No, A.Spl_Req_Date, A.Ref_No Order_No, B2.Buyer Buyer, A.Job_Ord_No, C.Reasons, D.Company_Unit, B3.Item Style, A.Ref_No, ";
                Str = Str + " A.ReasonId, A.CompanyUnitid, B1.ItemId StyleID, A.Spl_Reqid, B.BuyerID, A.Req_Remarks from Special_Req_Mas A  Left Join Buy_Ord_mas B On A.Ref_No = B.Order_No ";
                Str = Str + " Left Join Buy_Ord_Det B1 On A.Ref_No = B1.order_NO Left Join Buyer B2 On B.buyerid = B2.buyerid Left Join Item B3 On B1.Itemid = B3.Itemid ";
                Str = Str + " Left Join Reasons C On A.ReasonId = C.Reasonid Left Join company_unit D On A.CompanyUnitid = D.Company_UnitID Where Cast(Replace(Spl_Req_No, 'GUP-SRQ', '') As Numeric(20)) > 144";

                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Special Requestation - Edit", Str, String.Empty, 100, 100, 150, 350, 100, 100, 100);
                
                //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Lot Entry - View", "select A.ENo, A.EDate, A.Grn_No, B.supplier, D.item, E.color, F.size, C.Grn_Qty, C.No_Of_Bags, A.Remarks, A.Supplierid, A.Rowid from Socks_Lot_Master A Left Join FITSOCKS.dbo.Supplier B on A.Supplierid = B.Supplierid Left Join Socks_Lot_Details C on A.RowID = C.Master_ID Left Join Item D on C.ItemID = D.itemid Left Join Color E on C.ColorID = E.Colorid Left Join Size F on C.SizeID = F.sizeid ", String.Empty, 100, 100, 150, 350, 200, 150, 100, 100, 100, 350);
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
        public void Entry_Cancel()
        {
            try
            {
                MyBase.Clear(this);
                GridDetail_Data(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmSocksYarnSplRequestation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;

                    if (this.ActiveControl.Name == "TxtOrderNo")
                    {
                        if (TxtOrderNo.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Select Order No..!", "Gainup");
                            TxtOrderNo.Focus();
                            return;
                        }
                        else
                        {
                            TxtReason.Focus();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtReason")
                    {
                        if (TxtReason.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Select Reason..!", "Gainup");
                            TxtReason.Focus();
                            return;
                        }
                        else
                        {
                            TxtUnit.Focus();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtUnit")
                    {
                        if (TxtUnit.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Select Unit..!", "Gainup");
                            return;
                        }
                        else
                        {
                            st = 1;
                            Grid_Data();
                            Grid.CurrentCell = Grid["Rate", 0];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

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
                if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtOrderNo")
                    {
                        if (Dt.Rows.Count > 0)
                        {
                            if (MessageBox.Show("Sure to Clear Grid Detail ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                MyBase.Clear(this);
                                Grid_Data();
                                TxtOrderNo.Focus();
                            }
                            else
                            {
                                return;
                            }
                        }

                        //Str = "Select A.Order_No, C.Buyer, D.style, E.Job_Ord_No, A.Order_No Ref_No, A.Buyerid, B.Styleid From buy_ord_mas A ";
                        //Str = Str + " Inner Join buy_ord_style B On A.Order_No = B.order_no Left Join Buyer C On A.buyerid = C.buyerid Left Join Style D On B.Styleid = D.styleid ";
                        //Str = Str + " Left Join (Select Order_No, Job_Ord_No From Job_Ord_Mas Where Job_Ord_No Like '%WRK%')E On A.Order_No = E.Order_No Where ISnull(B.Despatch_Closed,'N') = 'N' And A.Order_no Not Like '%MOQ%' ";
                        //Str = Str + " Union All Select A.Order_No, B.Ledger_Name Buyer, C.Style, C.Job_Order_No, A.Order_No Ref_No, A.Party_Code BuyerID, C.StyleID From Socks_Order_Master A ";
                        //Str = Str + " Left Join ACCOUNTS.Dbo.Ledger_Master B On A.Party_Code = B.Ledger_Code And B.COMPANY_CODE = 1 And B.YEAR_CODE = Dbo.Get_Accounts_YearCode(getdate()) ";
                        //Str = Str + " Inner Join (Select Distinct A.JONo Job_Order_No, B.Order_ID, C.SampleItemID StyleID, D.Item Style from Socks_JobOrder_Master A Left Join Socks_JobOrder_Details B On A.RowID = B.Master_ID Left Join VFit_Sample_Master C On B.Sample_ID = C.RowID Left Join Item D On C.SampleItemID = D.itemid )C On A.RowID = C.Order_ID ";
                        //Str = Str + " Left Join Fit_Order_Status D On A.Order_No = D.Order_No Where ISnull(D.Status,'N') = 'N' And A.Order_no Not Like '%MOQ%' ";

                        Str = " Select A.Order_No, C.Buyer, D.style, E.Job_Ord_No, A.Order_No Ref_No, A.Buyerid, B.Styleid From buy_ord_mas A ";
                        Str = Str + " Inner Join buy_ord_style B On A.Order_No = B.order_no Left Join Buyer C On A.buyerid = C.buyerid Left Join Style D On B.Styleid = D.styleid ";
                        Str = Str + " Left Join (Select Order_No, Job_Ord_No From Job_Ord_Mas Where Job_Ord_No Like '%WRK%')E On A.Order_No = E.Order_No Where ISnull(B.Despatch_Closed,'N') = 'N' And A.Order_no Not Like '%MOQ%' ";
                        //New Orders are Not For This Special Requestation Screen
                        //Str = Str + " Union All Select Distinct A.Order_No, A.Party Buyer, A.Ord_Item, A.Order_no Job_Ord_No, A.Order_No Ref_No, A.BuyerId, A.Ord_ItemId StyleID ";
                        //Str = Str + " From Socks_Yarn_Planning_Fn()A Left Join Fit_Order_Status B On A.Order_No = B.Order_No Where ISnull(B.Status,'N') = 'N' And A.Order_no Not Like '%MOQ%' ";

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Order No..!", Str, String.Empty, 120, 200, 150, 150);

                        //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Order No..!", "Select A.Order_No, C.Buyer, D.style, E.Job_Ord_No, A.Ref_No, A.Buyerid, B.Styleid From buy_ord_mas A Inner Join buy_ord_style B On A.Order_No = B.order_no Left Join Buyer C On A.buyerid = C.buyerid Left Join Style D On B.Styleid = D.styleid Left Join (Select Order_No, Job_Ord_No From Job_Ord_Mas Where Job_Ord_No Like '%WRK%')E On A.Order_No = E.Order_No Where ISnull(B.Despatch_Closed,'N') = 'N' And A.Order_no Not Like '%MOQ%' ", String.Empty, 120, 200, 150, 150);

                        if (Dr != null)
                        {
                            TxtOrderNo.Text = Dr["Order_No"].ToString();
                            TxtBuyer.Text = Dr["Buyer"].ToString();
                            TxtBuyer.Tag = Dr["Buyerid"].ToString();
                            TxtStyle.Text = Dr["Style"].ToString();
                            TxtStyle.Tag = Dr["StyleID"].ToString();
                            TxtRefNo.Text = Dr["Ref_No"].ToString();
                            TxtJobOrderNo.Text = Dr["Job_Ord_No"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtReason")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Reason..!", " Select Reasons, Reasonid From Reasons ", String.Empty, 300, 150);

                        if (Dr != null)
                        {
                            TxtReason.Text = Dr["Reasons"].ToString();
                            TxtReason.Tag = Dr["ReasonID"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtUnit")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Unit..!", " Select Company_Unit, Company_UnitID  From company_unit Where Company_UnitID in (71, 72)", String.Empty, 200, 350);

                        if (Dr != null)
                        {
                            TxtUnit.Text = Dr["Company_Unit"].ToString();
                            TxtUnit.Tag = Dr["Company_UnitID"].ToString();
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

        private void FrmSocksYarnSplRequestation_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtOrderNo")
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
                else if (this.ActiveControl.Name == "TxtBuyer")
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Replace"].Index)
                    {
                        if (Grid["Replace", Grid.CurrentCell.RowIndex].Value.ToString() == "Y")
                        {
                            if (Convert.ToInt64(Grid["Slno1", Grid.CurrentCell.RowIndex].Value) == 0)
                            {
                                //Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                                Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Grid["Slno", Grid.CurrentCell.RowIndex].Value.ToString();
                            }
                           
                            GridDetail_Data(Convert.ToInt32(Grid["Slno", Grid.CurrentCell.RowIndex].Value));
                            GridDetail.CurrentCell = GridDetail["Item", 0];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            e.Handled = true;
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

        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Total_Count();
                    TxtRemarks.Focus();
                    return;
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Req_Qty"].Index)
                {
                    if (Grid["Replace", Grid.CurrentCell.RowIndex].Value.ToString() == "Y")
                    {
                        MyBase.Valid_Null(Txt, e);
                    }
                    else
                    {
                        MyBase.Valid_Decimal(Txt, e);
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Tfr_Qty"].Index)
                {
                    if (Grid["Replace", Grid.CurrentCell.RowIndex].Value.ToString() == "Y")
                    {
                        MyBase.Valid_Null(Txt, e);
                    }
                    else
                    {
                        MyBase.Valid_Decimal(Txt, e);
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Replace"].Index)
                {
                    if (Math.Round(Convert.ToDouble(Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value.ToString()), 3) > 0 || Math.Round(Convert.ToDouble(Grid["Tfr_Qty", Grid.CurrentCell.RowIndex].Value.ToString()), 3) > 0)
                    {
                        MyBase.Valid_Null(Txt, e);
                    }
                    else
                    {
                        MyBase.Valid_Yes_OR_No(Txt, e);
                    }
                }
                else
                {
                    MyBase.Valid_Null(Txt, e);
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Req_Qty"].Index)
                {
                    if (Math.Round(Convert.ToDecimal(Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value.ToString()), 3) > Math.Round(Convert.ToDecimal(Grid["Knit_Req", Grid.CurrentCell.RowIndex].Value.ToString()), 3))
                    {
                        MessageBox.Show("Required Qty Must Less Than Req Qty");
                        Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                        Grid.CurrentCell = Grid["Req_Qty", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                }
                else if(Grid.CurrentCell.ColumnIndex == Grid.Columns["Tfr_Qty"].Index)
                {
                    if (Math.Round(Convert.ToDecimal(Grid["Tfr_Qty", Grid.CurrentCell.RowIndex].Value.ToString()), 3) > Math.Round(Convert.ToDecimal(Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value.ToString()), 3))
                    {
                        MessageBox.Show("Transfer Qty Must Less Than Req Qty");
                        Grid["Tfr_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                        Grid.CurrentCell = Grid["Tfr_Qty", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Roll_Balance()
        {
            try
            {
                TxtEnteredWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum_With_Three_Digits(ref GridDetail, "Req_Qty", "Size")));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void GridDetail_Data(Int32 Row)
        {

            try
            {
                if (DtQty[Row] == null)
                {
                    DtQty[Row] = new DataTable();
                    if (MyParent._New)
                    {
                        MyBase.Load_Data("Select Top 1 0 SNo, '' Item, '' Color, '' Size, CAST(Null as Numeric (25, 2))Rate, CAST(Null as Numeric (25, 3)) Req_Qty, CAST(Null as Numeric (25, 3)) Tfr_Qty, 0 ItemID, 0 ColorID, 0 SizeID, " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() + " SlNo1 From Spl_Req_Qty()  ", ref DtQty[Row]);
                    }
                    else
                    {
                        if (MyParent.Edit && Vis == 1)
                        {
                            Str = " Select 0 SNo, C.Item, D.Color, E.Size, Cast(B.Rate As Numeric(25, 2))Rate, (Purchasable_Qty + Transferable_Qty)Req_Qty, ";
                            Str = Str + " Transferable_Qty Tfr_Qty, B.Itemid, B.ColorID, B.SizeID, Slno1 From Special_Req_Mas A Left Join Special_Req_Det B On A.Spl_Reqid = B.Spl_Reqid ";
                            Str = Str + " Left Join Item C On B.Itemid = C.Itemid Left Join Color D On B.Colorid = D.ColorID Left Join Size E On B.Sizeid = E.SizeId ";
                            Str = Str + " Where B.To_itemid Is Not NUll And A.Spl_Reqid = " + Code + " And B.Slno1 = " + Grid["Slno1", Pos].Value.ToString() + " ";
                            MyBase.Load_Data(Str, ref DtQty[Row]);
                        }
                        else
                        {
                            Str = " Select 0 SNo, C.Item, D.Color, E.Size, Cast(B.Rate As Numeric(25, 2))Rate, (Purchasable_Qty + Transferable_Qty)Req_Qty, ";
                            Str = Str + " Transferable_Qty Tfr_Qty, B.Itemid, B.ColorID, B.SizeID, Slno1 From Special_Req_Mas A Left Join Special_Req_Det B On A.Spl_Reqid = B.Spl_Reqid ";
                            Str = Str + " Left Join Item C On B.Itemid = C.Itemid Left Join Color D On B.Colorid = D.ColorID Left Join Size E On B.Sizeid = E.SizeId ";
                            Str = Str + " Where B.To_itemid Is Not NUll And A.Spl_Reqid = " + Code + " And B.Slno1 = " + Row + " ";
                            MyBase.Load_Data(Str, ref DtQty[Row]);
                        }
                    }
                }
                GridDetail.DataSource = DtQty[Row];
                MyBase.Grid_Designing(ref GridDetail, ref DtQty[Row], "SlNo1", "ItemID", "ColorID", "SizeID");
                MyBase.ReadOnly_Grid_Without(ref GridDetail, "Item", "Rate", "Req_Qty", "Tfr_Qty");
                MyBase.Grid_Colouring(ref GridDetail, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridDetail, 50, 120, 120, 120, 120);
                GridDetail.Columns["Rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridDetail.Columns["Req_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridDetail.Columns["Tfr_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                GridDetail.RowHeadersWidth = 10;

                if (!MyParent._New)
                {
                    //Balance_Pieces();
                }

                if (!MyParent._New && Vis == 1)
                {
                    GBQty.Visible = false;
                }
                else
                {
                    GBQty.Visible = true;
                }
                TxtTotalWeight.Text = Grid["Knit_Req", Grid.CurrentCell.RowIndex].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Balance_Pieces()
        {
            try
            {
                TxtEnteredWeight.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref GridDetail, "Req_Qty", "Item", "Color")));

                if (TxtEnteredWeight.Text.Trim() == String.Empty)
                {
                    TxtEnteredWeight.Text = String.Format("{0:0}");
                }
                else
                {
                    Roll_Balance();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Total_Count()
        {
            try
            {
                TxtTotal.Text = MyBase.Sum(ref Grid, "Req_Qty", "Item");
                TxtEnter.Text = MyBase.Sum(ref Grid, "Entered_Qty", "Item");
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
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Req_Qty"].Index)
                    {
                        //if (GridDetail["Req_Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Req_Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Req_Qty", GridDetail.CurrentCell.RowIndex].Value) == 0)
                        //{
                        //    e.Handled = true;
                        //    MessageBox.Show("Invalid Weight...!", "Gainup");
                        //    GridDetail.CurrentCell = GridDetail["Req_Qty", Grid.CurrentCell.RowIndex];
                        //    GridDetail.Focus();
                        //    GridDetail.BeginEdit(true);
                        //    return;
                        //}
                        
                    }
                }
                Roll_Balance();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void TxtRoll_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Req_Qty"].Index)
                {
                    MyBase.Valid_Decimal(Txt1, e);
                }
                else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Tfr_Qty"].Index)
                {
                    MyBase.Valid_Decimal(Txt1, e);
                }
                else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Rate"].Index)
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

        void TxtRoll_GotFocus(object sender, EventArgs e)
        {
            try
            {
                //if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Weight"].Index)
                //{
                //    Roll_Balance();
                //    if (GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value) == 0)
                //    {

                //        GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value = Convert.ToDouble(TxtBalance.Text);
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void TxtRoll_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Item"].Index)
                    {
                        //Str = "Select Distinct Item, Color, Size, ItemID, ColorID, SizeID From Socks_Store_Lot_For_Transfer_Admin() Order By Item, Color, Size ";

                        Str = " Select Distinct A.Item, A.Color, A.Size, A.ItemID, A.ColorID, A.SizeID, Cast(Isnull(B.Rate,0) As Numeric(20, 2))Rate From Socks_Store_Lot_For_Transfer_Admin() A Left Join Socks_Budget_App_Rate_Max_Fn()B On A.Itemid = B.Itemid And A.Colorid = B.Colorid And A.SizeID = B.SizeID Order By A.Item, A.Color, A.Size ";


                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", Str, String.Empty, 150, 150, 150);

                        if (Dr != null)
                        {

                            GridDetail["Item", GridDetail.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                            GridDetail["ItemID", GridDetail.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                            Txt1.Text = Dr["Item"].ToString();
                            GridDetail["Color", GridDetail.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                            GridDetail["ColorID", GridDetail.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();
                            GridDetail["Size", GridDetail.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                            GridDetail["SizeID", GridDetail.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                            GridDetail["Rate", GridDetail.CurrentCell.RowIndex].Value = Dr["Rate"].ToString();
                        }
                    }
                    //else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Color"].Index)
                    //{
                    //    Str = "Select Distinct Color, ColorID From Socks_Store_Lot_For_Transfer_Admin() Where ItemID = " + GridDetail["ItemID", GridDetail.CurrentCell.RowIndex].Value + " Order By Color";
                        
                    //    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", Str, String.Empty, 150, 100);

                    //    if (Dr != null)
                    //    {
                    //        GridDetail["Color", GridDetail.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                    //        Txt1.Text = Dr["Color"].ToString();
                    //        GridDetail["ColorID", GridDetail.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();
                    //    }
                    //}
                    //else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Size"].Index)
                    //{
                    //    Str = " Select Distinct Size, SizeID From Socks_Store_Lot_For_Transfer_Admin() Where ItemID = " + GridDetail["ItemID", GridDetail.CurrentCell.RowIndex].Value + " And ColorID = " + GridDetail["ColorID", GridDetail.CurrentCell.RowIndex].Value + " Order By Size ";

                    //    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", Str, String.Empty, 150, 100);

                    //    if (Dr != null)
                    //    {
                    //        GridDetail["Size", GridDetail.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                    //        Txt1.Text = Dr["Size"].ToString();
                    //        GridDetail["SizeID", GridDetail.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void TxtRoll_Leave(object sender, EventArgs e)
        {
            try
            {
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Req_Qty"].Index)
                {
                    if (GridDetail["Req_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        if (Math.Round(Convert.ToDouble(GridDetail["Req_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString())) > Math.Round(Convert.ToDouble(TxtTotalWeight.Text.ToString())))
                        {
                            MessageBox.Show("Required Qty Must Less Than Req Qty");
                            GridDetail["Req_Qty", GridDetail.CurrentCell.RowIndex].Value = "0.000";
                            GridDetail.CurrentCell = GridDetail["Req_Qty", GridDetail.CurrentCell.RowIndex];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            return;
                        }
                    }
                }
                else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Tfr_Qty"].Index)
                {
                    if (GridDetail["Tfr_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        if ((Math.Round(Convert.ToDecimal(GridDetail["Tfr_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString()), 3) > Math.Round(Convert.ToDecimal(GridDetail["Req_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString()), 3)) || (Math.Round(Convert.ToDouble(GridDetail["Tfr_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString())) > Math.Round(Convert.ToDouble(TxtTotalWeight.Text.ToString()))))
                        {
                            MessageBox.Show("Transfer Qty Must Less Than Req Qty");
                            GridDetail["Tfr_Qty", GridDetail.CurrentCell.RowIndex].Value = "0.000";
                            GridDetail.CurrentCell = GridDetail["Tfr_Qty", GridDetail.CurrentCell.RowIndex];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
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

        private void GridDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt1 == null)
                {
                    Txt1 = (TextBox)e.Control;
                    Txt1.KeyPress += new KeyPressEventHandler(TxtRoll_KeyPress);
                    Txt1.GotFocus += new EventHandler(TxtRoll_GotFocus);
                    Txt1.KeyDown += new KeyEventHandler(TxtRoll_KeyDown);
                    Txt1.Leave += new EventHandler(TxtRoll_Leave);
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

                if (TxtEnteredWeight.Text == "0.000")
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    GridDetail.CurrentCell = GridDetail["Req_Qty", 0];
                    GridDetail.Focus();
                    GridDetail.BeginEdit(true);
                    return;
                }
                for (int i = 0; i <= DtQty[Convert.ToInt64(Grid["Slno", Grid.CurrentCell.RowIndex].Value)].Rows.Count -1 ; i++)
                {
                    if (Math.Round(Convert.ToDecimal(GridDetail["Tfr_Qty", i].Value), 3) > Math.Round(Convert.ToDecimal(GridDetail["Req_Qty", i].Value), 3))
                    {
                        MessageBox.Show("Tfr Qty Is Invalid in Row " + i.ToString() + "");
                        GridDetail["Tfr_Qty", i].Value = "0.000";
                        GridDetail.CurrentCell = GridDetail["Req_Qty", i];
                        GridDetail.Focus();
                        GridDetail.BeginEdit(true);
                        return;
                    }
                }
                Grid["Slno_Temp", Grid.CurrentCell.RowIndex].Value = Grid["Slno", Grid.CurrentCell.RowIndex].Value;
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Remarks", Grid.CurrentCell.RowIndex];
                Grid["Entered_Qty", Grid.CurrentCell.RowIndex].Value = TxtEnteredWeight.Text.ToString();
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
                    if (GridDetail["Req_Qty", i].Value == DBNull.Value || Convert.ToDouble(GridDetail["Req_Qty", i].Value) <= 0)
                    {
                        MessageBox.Show("Invalid KGS ..!", "Gainup");
                        Grid.CurrentCell = Grid["Req_Qty", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        GBQty.Visible = false;
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                for (int i = 0; i <= DtQty[Convert.ToInt64(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].Rows.Count; i++)
                {
                    if (Math.Round(Convert.ToDecimal(GridDetail["Tfr_Qty", i].Value), 3) > Math.Round(Convert.ToDecimal(GridDetail["Req_Qty", i].Value), 3))
                    {
                        MessageBox.Show("Tfr Qty Is Invalid in Row " + i.ToString() + "");
                        GridDetail["Tfr_Qty", i].Value = "0.000";
                        GridDetail.CurrentCell = GridDetail["Req_Qty", i];
                        GridDetail.Focus();
                        GridDetail.BeginEdit(true);
                        return;
                    }
                }
                DtQty = new DataTable[300];
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Req_Qty", (Grid.CurrentCell.RowIndex)];
                Grid.Focus();
                Grid.BeginEdit(true);
                return;

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
                MyBase.Row_Number(ref Grid);
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

        private void Grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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

        private void TxtRemarks_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void TxtTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void myTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

    }
}