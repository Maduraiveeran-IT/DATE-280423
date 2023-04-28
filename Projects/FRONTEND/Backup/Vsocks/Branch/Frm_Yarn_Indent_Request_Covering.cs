using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SelectionTool_NmSp;
using Accounts_ControlModules;

namespace Accounts
{
    public partial class Frm_Yarn_Indent_Request_Covering : Form, Entry 
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable DtLot = new DataTable();
        DataTable[] DtQty;
        DataRow Dr;
        Int64 Code;
        Int64 i;
        TextBox Txt = null;
        TextBox Txt1 = null;
        TextBox Txt_Qty = null;
        String[] Queries;
        String Str;
        String Buffer_Table = String.Empty;
        Boolean Buffer_Update = false;
        Int64 Mode = 0;

        String Order_No = String.Empty;
        String Sample = String.Empty;
        String Size = String.Empty;
        Int64 ItemID = 0;
        Int64 ColorID = 0;
        Int64 SizeID = 0;
        Int32 Delivery_No = 0;
        Int32 Row1 = 0;
        Int16 Vis = 0;
        int Pos = 0;


        public Frm_Yarn_Indent_Request_Covering()
        {
            InitializeComponent();
        }

        void Set_Min_Max_Date(Boolean Condition)
        {
            try
            {
                DataTable Tdt = new DataTable();
                if (Condition)
                {
                    MyBase.Load_Data("Select Cast(GetDate() as Date) MinDate, DateAdd(D, 3, Cast(GetDate() as Date)) MaxDate ", ref Tdt);
                    DcDate.MinDate = Convert.ToDateTime(Tdt.Rows[0][0]);
                    DcDate.MaxDate = Convert.ToDateTime(Tdt.Rows[0][1]);
                }
                else
                {
                    DcDate.MinDate = Convert.ToDateTime("01-Apr-2014");
                    DcDate.MaxDate = Convert.ToDateTime("31-Mar-2030");
                }
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
                Set_Min_Max_Date(true);
                TxtUnit.Focus();
                TxtUnit.Text = "COVERING";
                TxtUnit.Tag = 4;
                DcDate.Focus();
                GBLOT.Visible = false;
                Grid_Data();
                DtQty = new DataTable[30];
                return;
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

                //for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                //{
                //    if (Grid["Bom_Qty", i].Value == DBNull.Value || Grid["Bom_Qty", i].Value.ToString() == String.Empty || Convert.ToInt64(Grid["Bom_Qty", i].Value) == 0)
                //    {
                //        MessageBox.Show(" ZERO Balance is Invalid in Row " + (i + 1) + "  ", "Gainup");
                //        Grid.CurrentCell = Grid["Balance", i];
                //        Grid.Focus();
                //        Grid.BeginEdit(true);
                //        MyParent.Save_Error = true;
                //        return;
                //    }
                //}

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
                    TxtEntryNo.Text = MyBase.MaxOnlyWithoutComp("Socks_Covering_Yarn_Indent_Request_Master", "EntryNO", String.Empty, String.Empty, 0).ToString();
                }
                Queries = new string[Dt.Rows.Count * 100];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_Covering_Yarn_Indent_Request_Master(EntryNO, IndentNO, EntryDate, ProductionDate, UnitCode, SystemName, EntryTime, UserCode, Compcode, DeptCode, Remarks) values (" + TxtEntryNo.Text + ", " + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", DcDate.Value) + "', " + TxtUnit.Tag + ", Host_Name(), GetDate(), " + MyParent.UserCode + ", " + MyParent.CompCode + ", 51, '" + TxtRemarks.Text + "'); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_Covering_Yarn_Indent_Request_Master", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_Covering_Yarn_Indent_Request_Master Set EntryDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', ProductionDate = '" + String.Format("{0:dd-MMM-yyyy}", DcDate.Value) + "', Remarks = '" + TxtRemarks.Text + "', UserCode = " + MyParent.UserCode + ", DeptCode = 51 Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_Covering_Yarn_Indent_Request_Master", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from Socks_Covering_Yarn_Indent_SampleWise_Request_Details where MasterID = " + Code;
                    Queries[Array_Index++] = "Delete from Socks_Covering_Yarn_Indent_Request_Details where MasterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Grid["T", i].Value.ToString()) > 0.000)
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into Socks_Covering_Yarn_Indent_Request_Details (MasterID, Slno, Order_NO, Slno1, Remarks) Values (@@IDENTITY, " + Grid["Slno", i].Value + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["Slno", i].Value + ", '" + Grid["Remarks", i].Value + "')";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert into Socks_Covering_Yarn_Indent_Request_Details (MasterID, Slno, Order_NO, Slno1, Remarks) Values (" + Code + ", " + Grid["Slno", i].Value + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["Slno", i].Value + ", '" + Grid["Remarks", i].Value + "')";
                        }
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
                                    if (Convert.ToDouble(DtQty[i].Rows[j]["Indent_Qty"].ToString()) > 0)
                                    {
                                        Queries[Array_Index++] = "Insert Into Socks_Covering_Yarn_Indent_SampleWise_Request_Details (SlNo, MasterID, ItemID, ColorID, SizeID, Indent_Qty, SlNo1, Issue_Closed) Values (" + DtQty[i].Rows[j]["Slno"].ToString() + ", @@IDENTITY, " + DtQty[i].Rows[j]["ItemID"].ToString() + ", " + DtQty[i].Rows[j]["ColorID"].ToString() + ", " + DtQty[i].Rows[j]["SizeID"].ToString() + ", " + DtQty[i].Rows[j]["Indent_Qty"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ", 'P')";
                                    }
                                }
                                else
                                {
                                    if (Convert.ToDouble(DtQty[i].Rows[j]["Indent_Qty"].ToString()) > 0)
                                    {
                                        Queries[Array_Index++] = "Insert Into Socks_Covering_Yarn_Indent_SampleWise_Request_Details (SlNo, MasterID, ItemID, ColorID, SizeID, Indent_Qty, SlNo1, Issue_Closed) Values ( " + DtQty[i].Rows[j]["Slno"].ToString() + ", " + Code + ", " + DtQty[i].Rows[j]["ItemID"].ToString() + ", " + DtQty[i].Rows[j]["ColorID"].ToString() + ", " + DtQty[i].Rows[j]["SizeID"].ToString() + ", " + DtQty[i].Rows[j]["Indent_Qty"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ", 'P')";
                                    }
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
                GBLOT.Visible = false;
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Covering Yarn Indent Request Entry - Delete", "Select S1.EntryNo, S1.EntryDate, S2.Order_No, U1.Unit_Name, S1.ProductionDate, S1.Remarks, S1.UnitCode, S1.RowID From Socks_Covering_Yarn_Indent_Request_Master S1 Inner Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID Left Join Unit_Master U1 On S1.UnitCode = U1.RowId Where DeptCode in (51)", String.Empty, 80, 100, 100, 100, 100, 100);
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
                    MyBase.Run("Delete from Socks_Covering_Yarn_Indent_SampleWise_Request_Details where MasterID = " + Code, "Delete from Socks_Covering_Yarn_Indent_Request_Details where MasterID = " + Code, "Delete from Socks_Covering_Yarn_Indent_Request_Master where RowID = " + Code);
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
                TxtEntryNo.Text = Dr["EntryNo"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["EntryDate"]);
                DcDate.Value = Convert.ToDateTime(Dr["ProductionDate"]);
                TxtUnit.Tag = Dr["UnitCode"].ToString();
                TxtUnit.Text = Dr["Unit_Name"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();
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
                Set_Min_Max_Date(false);
                GBLOT.Visible = false;
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Covering Yarn Indent Request Entry - Edit", "Select S1.EntryNo, S1.EntryDate, S2.Order_No, U1.Unit_Name, S1.ProductionDate, S1.Remarks, S1.UnitCode, S1.RowID From Socks_Covering_Yarn_Indent_Request_Master S1 Inner Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID Left Join Unit_Master U1 On S1.UnitCode = U1.RowId Where DeptCode in (51)", String.Empty, 80, 100, 100, 100, 100, 100, 100);
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

        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                GBLOT.Visible = false;
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Covering Yarn Indent Request Entry - View", "Select S1.EntryNo, S1.EntryDate, S2.Order_No, U1.Unit_Name, S1.ProductionDate, S1.Remarks, S1.UnitCode, S1.RowID From Socks_Covering_Yarn_Indent_Request_Master S1 Inner Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID Left Join Unit_Master U1 On S1.UnitCode = U1.RowId Where DeptCode in (51)", String.Empty, 80, 100, 100, 100, 100, 100);
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

        public void Entry_Print()
        {
            try
            {
                GBLOT.Visible = false;

                Str = " Select Top 10000000000 U1.Unit_Name, S2.Order_No, I.Item, C.COlor, S.Size, Sum(ISnull(S3.Indent_Qty,0))Indent_Qty From Socks_Covering_Yarn_Indent_Request_Master S1 ";
                Str = Str + " Left Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID ";
                Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details S3 On S1.RowId = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                //Str = Str + " Left Join FITSOCKS.DBO.Base_Req_For_Yarn()S4 On S2.Order_No = S4.Order_No And S3.ItemID = S4.Itemid And S3.ColorID = S4.ColorId And S3.SizeID = S4.SizeId ";
                Str = Str + " Left Join Item I On S3.ItemID = I.ItemID Left Join Color C On S3.ColorID = C.ColorID Left Join Size S On S3.SizeID = S.SizeID ";
                Str = Str + " Left Join Unit_Master U1 On S1.UnitCode = U1.RowId ";
                Str = Str + " Where S1.RowID =  " + Code + " Group By U1.Unit_Name, S2.Order_No, I.Item, C.COlor, S.Size ";
                Str = Str + " Order By I.Item, C.COlor, S.Size ";

                MyBase.Execute_Qry(Str, "Yarn_Indent_Request_Receipt");

                Str = "Select Sno, Grn_No, Item, Color, Size, LotNo, Stock From Indent_Request_Orders_Lot_Details(" + Code + ")";
                MyBase.Execute_Qry(Str, "Yarn_Order_Lot_Details");

                DataTable Dt1 = new DataTable();
                String Str1 = "Select Getdate()Date1";
                MyBase.Load_Data(Str1, ref Dt1);

                DataTable Dt2 = new DataTable();
                String Str2 = "Select IndentNo, ProductionDate From Socks_Covering_Yarn_Indent_Request_Master Where RowID = " + Code + "";
                MyBase.Load_Data(Str2, ref Dt2);

                CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Cover_Yarn_Indent_Request_New.rpt");
                MyParent.FormulaFill(ref ObjRpt, "PrintOutDate", Dt1.Rows[0][0].ToString());
                MyParent.FormulaFill(ref ObjRpt, "ProductionDate", Dt2.Rows[0][1].ToString());
                MyParent.FormulaFill(ref ObjRpt, "IndentNo", Dt2.Rows[0][0].ToString());
                MyParent.CReport(ref ObjRpt, "Covering Yarn Indent Request..!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Yarn_Indent_Request_Covering_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                TxtUnit.Focus();
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
                    Str = " Select 0 As Slno, S2.Order_No, S3.Descr, S3.Bom_Qty, 0 Slno1, S2.Remarks, 'N' Record, '-' T From Socks_Covering_Yarn_Indent_Request_Master S1 ";
                    Str = Str + " Left Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID Left Join (Select Order_No, Max(Descr)Descr, SUM(Bom_Qty)Bom_Qty From Socks_Bom()Where despatch_Closed = 'N' Group BY Order_No) S3 On S2.Order_No = S3.Order_No Where 1 = 2";
                }
                else
                {
                    //Str = " Select S2.Slno, S2.Order_No, S3.Descr, S3.Bom_Qty, S2.Slno1, S2.Remarks, 'O' Record, 0 T From Socks_Covering_Yarn_Indent_Request_Master S1 ";
                    //Str = Str + "Left Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID Left Join (Select Order_No, Max(Descr)Descr, SUM(Bom_Qty)Bom_Qty From Socks_Bom() Group BY Order_No) S3 On S2.Order_No = S3.Order_No Where S1.RowId = " + Code;

                    Str = " Select S2.Slno, S2.Order_No, S3.Descr, S3.Bom_Qty, S2.Slno1, S2.Remarks, 'O' Record, 0 T From Socks_Covering_Yarn_Indent_Request_Master S1 ";
                    Str = Str + " Left Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID Left Join (Select Order_No, Max(Descr)Descr, 0 Bom_Qty From Order_Buyer_Details() Group BY Order_No) S3 On S2.Order_No = S3.Order_No Where S1.RowId = " + Code;
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Slno1", "T", "Record");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Order_No", "Bom_Qty", "Remarks");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 50, 120, 200, 100, 150);
                Grid.Columns["Bom_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid.Columns["Remarks"].DefaultCellStyle.Format = "-";

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (!MyParent._New)
                    {
                        TxtQty1.Text = Grid["Order_No", i].Value.ToString();
                        Vis = 1;
                        Pos = i;
                        GridDetail_Data(Convert.ToInt16(Grid["Slno1", i].Value), Grid["Slno1", i].Value.ToString());
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

        void GridDetail_Data(Int32 Row, String Order_NO)
        {
            try
            {
                Row1 = Row;
                if (DtQty[Row] == null)
                {
                    DtQty[Row] = new DataTable();
                    if (MyParent._New)
                    {
                        //Str = " Select 0 Slno, A.Cover_Req_Item Item, A.Cover_Req_Color Color, A.Cover_Req_Size Size, ISNULL(A.Cover_Req, 0)Tot_Req, 0 Issued, (ISNULL(A.Cover_Req, 0) - 0)Bal_Req, (SUM(ISNULL(B.Stock,0)) + SUM(ISNULL(C.Stock,0)))Stock, 0.000 Indent_Qty, A.Cover_Req_ItemID ItemID, ";
                        //Str = Str + " A.Cover_Req_ColorID ColorID, A.Cover_Req_SizeID SizeID, " + Row + " Slno1, '' T From FITSOCKS.Dbo.Covering_Req_Status_Orderwise()A Left Join FITSOCKS.Dbo.Socks_Store_Current_Stock_OrderWise()B On A.Order_No = B.Order_No And A.Cover_Req_ItemID = B.ItemID And ";
                        //Str = Str + " A.Cover_Req_ColorID = B.ColorID And A.Cover_Req_SizeID = B.SizeID Left Join FITSOCKS.Dbo.Socks_Store_Current_Stock_OrderWise()C On C.Order_No = 'GENERAL' And A.Cover_Req_ItemID = C.ItemID And A.Cover_Req_ColorID = C.ColorID And A.Cover_Req_SizeID = C.SizeID ";
                        //Str = Str + " Where A.Order_No Like '" + Order_No + "' Group BY A.Cover_Req_Item, A.Cover_Req_Color, A.Cover_Req_Size, ISNULL(A.Cover_Req, 0), A.Cover_Req_ItemID, A.Cover_Req_ColorID, A.Cover_Req_SizeID ";

                        //Commented On 05-Nov-2016
                        //Str = " Select 0 Slno, A.Cover_Req_Item Item, A.Cover_Req_Color Color, A.Cover_Req_Size Size, ISNULL(A.Cover_Req, 0)Tot_Req, Isnull(C.Issue_Qty,0) Issued, (ISNULL(A.Cover_Req, 0) - Isnull(C.Issue_Qty, 0))Bal_Req, SUM(ISNULL(B.Stock, 0))Stock, ";
                        //Str = Str + "  0.000 Indent_Qty, A.Cover_Req_ItemID ItemID, A.Cover_Req_ColorID ColorID, A.Cover_Req_SizeID SizeID, " + Row + " Slno1, '' T From FITSOCKS.Dbo.Covering_Req_Status_Orderwise()A ";
                        //Str = Str + " Left Join FITSOCKS.Dbo.Socks_Store_Current_Stock_OrderWise()B On B.Order_No = 'GENERAL' And A.Cover_Req_ItemID = B.ItemID And A.Cover_Req_ColorID = B.ColorID And A.Cover_Req_SizeID = B.SizeID ";
                        //Str = Str + " Left Join FITSOCKS.Dbo.Socks_Store_Covering_Indent_Issued_OrderWise() C On A.Order_No = C.Order_No And A.Cover_Req_ItemID = C.Itemid And A.Cover_Req_Colorid = C.Colorid and A.Cover_Req_Sizeid = C.Sizeid ";
                        //Str = Str + " Where A.Order_No Like '" + Order_No + "' Group BY A.Cover_Req_Item, A.Cover_Req_Color, A.Cover_Req_Size, ISNULL(A.Cover_Req, 0), A.Cover_Req_ItemID, A.Cover_Req_ColorID, A.Cover_Req_SizeID, Isnull(C.Issue_Qty,0) ";

                        Str = " Select 0 Slno, B.Item, C.COlor, D.Size, 1000000 Tot_Req, 0 Issued, 1000000 Bal_Req, A.Stock, 0.000 Indent_Qty, ";
                        Str = Str + " A.ItemID, A.ColorID, A.SizeID, " + Row + " Slno1, '' T From Socks_Store_Current_Stock_OrderWise()A ";
                        Str = Str + " Left Join Item B On A.ItemID = B.ItemID ";
                        Str = Str + " Left Join Color C On A.ColorID = C.COlorID ";
                        Str = Str + " Left Join Size D On A.SizeID = D.SizeID ";
                        Str = Str + " Where A.Order_No Like '%GENERAL%' ";
                        Str = Str + " And B.Item in ('POLYESTER', 'Rubber', 'Nylon', 'Lycra') ";
                        Str = Str + " Order By A.Order_No, B.Item, C.COlor, D.Size ";
                    }
                    else
                    {
                        if ((Grid["Record", Grid.CurrentCell.RowIndex].Value.ToString()) == "O" && Vis != 1)
                        {
                            //Str = " Select S3.Slno, I.Item, C.COlor, S.Size, Isnull(S5.Cover_Req,0) Tot_Req, Isnull(S6.Issue_Qty,0) Issued, Isnull(S5.Cover_Req,0) - Isnull(S6.Issue_Qty,0) Bal_Req, ";
                            //Str = Str + " (Isnull(S4.Stock,0) + Isnull(S41.Stock,0)) Stock,  S3.Indent_Qty, S3.ItemID, S3.ColorID, S3.SizeID, S2.Slno1, '' T ";
                            //Str = Str + " From Socks_Covering_Yarn_Indent_Request_Master S1 Left Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID ";
                            //Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details S3 On S1.RowId = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                            //Str = Str + " Left Join Socks_Store_Current_Stock_OrderWise() S4 On S2.Order_No = S4.Order_No And S3.ItemID = S4.Itemid And S3.ColorID = S4.ColorId And S3.SizeID = S4.SizeId ";
                            //Str = Str + " Left Join Socks_Store_Current_Stock_OrderWise() S41 On S41.Order_No = 'GENERAL' And S3.ItemID = S41.Itemid And S3.ColorID = S41.ColorId And S3.SizeID = S41.SizeId ";
                            //Str = Str + " Left Join FITSOCKS.Dbo.Covering_Req_Status_Orderwise() S5 On S2.Order_No = S5.Order_No And S3.ItemID = S5.Cover_Req_ItemID And S3.ColorID = S5.Cover_Req_ColorID And S3.SizeID = S5.Cover_Req_SizeID ";
                            //Str = Str + " Left Join FITSOCKS.Dbo.Socks_Store_Covering_Indent_Issued_OrderWise() S6 On S2.Order_No = S6.Order_No And S3.ItemID = S6.Itemid And S3.ColorID = S6.ColorId And S3.SizeID = S6.SizeId ";
                            //Str = Str + " Left Join Item I On S3.ItemID = I.ItemID Left Join Color C On S3.ColorID = C.ColorID Left Join Size S On S3.SizeID = S.SizeID ";
                            //Str = Str + " Where S1.RowID =  " + Code + " and S2.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() + " ";

                            //Commented On 05-Nov-2016

                            //Str = " Select S3.Slno, I.Item, C.COlor, S.Size, Isnull(S5.Cover_Req, 0) Tot_Req, Isnull(S6.Issue_Qty, 0) Issued, Isnull(S5.Cover_Req, 0) - Isnull(S6.Issue_Qty, 0) Bal_Req, ";
                            //Str = Str + " Isnull(S41.Stock, 0) Stock, S3.Indent_Qty, S3.ItemID, S3.ColorID, S3.SizeID, S2.Slno1, '' T ";
                            //Str = Str + " From Socks_Covering_Yarn_Indent_Request_Master S1 Left Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID ";
                            //Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details S3 On S1.RowId = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                            //Str = Str + " Left Join Socks_Store_Current_Stock_OrderWise() S41 On S41.Order_No = 'GENERAL' And S3.ItemID = S41.Itemid And S3.ColorID = S41.ColorId And S3.SizeID = S41.SizeId ";
                            //Str = Str + " Left Join FITSOCKS.Dbo.Covering_Req_Status_Orderwise() S5 On S2.Order_No = S5.Order_No And S3.ItemID = S5.Cover_Req_ItemID And S3.ColorID = S5.Cover_Req_ColorID And S3.SizeID = S5.Cover_Req_SizeID ";
                            //Str = Str + " Left Join FITSOCKS.Dbo.Socks_Store_Covering_Indent_Issued_OrderWise() S6 On S2.Order_No = S6.Order_No And S3.ItemID = S6.Itemid And S3.ColorID = S6.ColorId And S3.SizeID = S6.SizeId ";
                            //Str = Str + " Left Join Item I On S3.ItemID = I.ItemID Left Join Color C On S3.ColorID = C.ColorID Left Join Size S On S3.SizeID = S.SizeID ";
                            //Str = Str + " Where S1.RowID =  " + Code + " and S2.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() + " ";


                            Str = " Select S3.Slno, I.Item, C.COlor, S.Size, 100000 Tot_Req, Isnull(S6.Issue_Qty, 0) Issued, 100000 - Isnull(S6.Issue_Qty, 0) Bal_Req, ";
                            Str = Str + " Isnull(S41.Stock, 0) Stock, S3.Indent_Qty, S3.ItemID, S3.ColorID, S3.SizeID, S2.Slno1, '' T ";
                            Str = Str + " From Socks_Covering_Yarn_Indent_Request_Master S1 ";
                            Str = Str + " Left Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID ";
                            Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details S3 On S1.RowId = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                            Str = Str + " Left Join Socks_Store_Current_Stock_OrderWise() S41 On S41.Order_No = 'GENERAL' And S3.ItemID = S41.Itemid ";
                            Str = Str + " And S3.ColorID = S41.ColorId And S3.SizeID = S41.SizeId ";
                            Str = Str + " Left Join FITSOCKS.Dbo.Socks_Store_Covering_Indent_Issued_OrderWise() S6 On S2.Order_No = S6.Order_No And S3.ItemID = S6.Itemid ";
                            Str = Str + " And S3.ColorID = S6.ColorId And S3.SizeID = S6.SizeId ";
                            Str = Str + " Left Join Item I On S3.ItemID = I.ItemID Left Join Color C On S3.ColorID = C.ColorID Left Join Size S On S3.SizeID = S.SizeID ";
                            Str = Str + " Where S1.RowID =  " + Code + " and S2.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() + " ";
                        }
                        else if ((Grid["Record", Grid.CurrentCell.RowIndex].Value.ToString()) == "O" && Vis == 1)
                        {
                            //Str = " Select S3.Slno, I.Item, C.COlor, S.Size, Isnull(S5.Cover_Req,0) Tot_Req, Isnull(S6.Issue_Qty,0) Issued, Isnull(S5.Cover_Req,0) - Isnull(S6.Issue_Qty,0) Bal_Req, ";
                            //Str = Str + " (Isnull(S4.Stock,0) + Isnull(S41.Stock,0)) Stock,  S3.Indent_Qty, S3.ItemID, S3.ColorID, S3.SizeID, S2.Slno1, '' T ";
                            //Str = Str + " From Socks_Covering_Yarn_Indent_Request_Master S1 Left Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID ";
                            //Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details S3 On S1.RowId = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                            //Str = Str + " Left Join Socks_Store_Current_Stock_OrderWise() S4 On S2.Order_No = S4.Order_No And S3.ItemID = S4.Itemid And S3.ColorID = S4.ColorId And S3.SizeID = S4.SizeId ";
                            //Str = Str + " Left Join Socks_Store_Current_Stock_OrderWise() S41 On S41.Order_No = 'GENERAL' And S3.ItemID = S41.Itemid And S3.ColorID = S41.ColorId And S3.SizeID = S41.SizeId ";
                            //Str = Str + " Left Join FITSOCKS.Dbo.Covering_Req_Status_Orderwise() S5 On S2.Order_No = S5.Order_No And S3.ItemID = S5.Cover_Req_ItemID And S3.ColorID = S5.Cover_Req_ColorID And S3.SizeID = S5.Cover_Req_SizeID ";
                            //Str = Str + " Left Join FITSOCKS.Dbo.Socks_Store_Covering_Indent_Issued_OrderWise() S6 On S2.Order_No = S6.Order_No And S3.ItemID = S6.Itemid And S3.ColorID = S6.ColorId And S3.SizeID = S6.SizeId ";
                            //Str = Str + " Left Join Item I On S3.ItemID = I.ItemID Left Join Color C On S3.ColorID = C.ColorID Left Join Size S On S3.SizeID = S.SizeID ";
                            //Str = Str + " Where S1.RowID =  " + Code + " and S2.Slno1 = " + Grid["Slno1", Pos].Value.ToString() + " ";


                            //Commented On 05-Nov-2016
                            //Str = " Select S3.Slno, I.Item, C.COlor, S.Size, Isnull(S5.Cover_Req, 0) Tot_Req, Isnull(S6.Issue_Qty, 0) Issued, Isnull(S5.Cover_Req, 0) - Isnull(S6.Issue_Qty, 0) Bal_Req, ";
                            //Str = Str + " Isnull(S41.Stock, 0) Stock, S3.Indent_Qty, S3.ItemID, S3.ColorID, S3.SizeID, S2.Slno1, '' T ";
                            //Str = Str + " From Socks_Covering_Yarn_Indent_Request_Master S1 Left Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID ";
                            //Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details S3 On S1.RowId = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                            //Str = Str + " Left Join Socks_Store_Current_Stock_OrderWise() S41 On S41.Order_No = 'GENERAL' And S3.ItemID = S41.Itemid And S3.ColorID = S41.ColorId And S3.SizeID = S41.SizeId ";
                            //Str = Str + " Left Join FITSOCKS.Dbo.Covering_Req_Status_Orderwise() S5 On S2.Order_No = S5.Order_No And S3.ItemID = S5.Cover_Req_ItemID And S3.ColorID = S5.Cover_Req_ColorID And S3.SizeID = S5.Cover_Req_SizeID ";
                            //Str = Str + " Left Join FITSOCKS.Dbo.Socks_Store_Covering_Indent_Issued_OrderWise() S6 On S2.Order_No = S6.Order_No And S3.ItemID = S6.Itemid And S3.ColorID = S6.ColorId And S3.SizeID = S6.SizeId ";
                            //Str = Str + " Left Join Item I On S3.ItemID = I.ItemID Left Join Color C On S3.ColorID = C.ColorID Left Join Size S On S3.SizeID = S.SizeID ";
                            //Str = Str + " Where S1.RowID =  " + Code + " and S2.Slno1 = " + Grid["Slno1", Pos].Value.ToString() + " ";


                            Str = " Select S3.Slno, I.Item, C.COlor, S.Size, 100000 Tot_Req, Isnull(S6.Issue_Qty, 0) Issued, 100000 - Isnull(S6.Issue_Qty, 0) Bal_Req, ";
                            Str = Str + " Isnull(S41.Stock, 0) Stock, S3.Indent_Qty, S3.ItemID, S3.ColorID, S3.SizeID, S2.Slno1, '' T ";
                            Str = Str + " From Socks_Covering_Yarn_Indent_Request_Master S1 ";
                            Str = Str + " Left Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID ";
                            Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details S3 On S1.RowId = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                            Str = Str + " Left Join Socks_Store_Current_Stock_OrderWise() S41 On S41.Order_No = 'GENERAL' And S3.ItemID = S41.Itemid ";
                            Str = Str + " And S3.ColorID = S41.ColorId And S3.SizeID = S41.SizeId ";
                            Str = Str + " Left Join FITSOCKS.Dbo.Socks_Store_Covering_Indent_Issued_OrderWise() S6 On S2.Order_No = S6.Order_No And S3.ItemID = S6.Itemid ";
                            Str = Str + " And S3.ColorID = S6.ColorId And S3.SizeID = S6.SizeId ";
                            Str = Str + " Left Join Item I On S3.ItemID = I.ItemID Left Join Color C On S3.ColorID = C.ColorID Left Join Size S On S3.SizeID = S.SizeID ";
                            Str = Str + " Where S1.RowID =  " + Code + " and S2.Slno1 = " + Grid["Slno1", Pos].Value.ToString() + " ";
                        }
                        else
                        {
                            //Commented On 05-Nov-2016
                            //Str = " Select 0 Slno, A.Cover_Req_Item Item, A.Cover_Req_Color Color, A.Cover_Req_Size Size, ISNULL(A.Cover_Req, 0)Tot_Req, Isnull(C.Issue_Qty,0) Issued, (ISNULL(A.Cover_Req, 0) - Isnull(C.Issue_Qty, 0))Bal_Req, SUM(ISNULL(B.Stock, 0))Stock, ";
                            //Str = Str + "  0.000 Indent_Qty, A.Cover_Req_ItemID ItemID, A.Cover_Req_ColorID ColorID, A.Cover_Req_SizeID SizeID, " + Row + " Slno1, '' T From FITSOCKS.Dbo.Covering_Req_Status_Orderwise()A ";
                            //Str = Str + " Left Join FITSOCKS.Dbo.Socks_Store_Current_Stock_OrderWise()B On B.Order_No = 'GENERAL' And A.Cover_Req_ItemID = B.ItemID And A.Cover_Req_ColorID = B.ColorID And A.Cover_Req_SizeID = B.SizeID ";
                            //Str = Str + " Left Join FITSOCKS.Dbo.Socks_Store_Covering_Indent_Issued_OrderWise() C On A.Order_No = C.Order_No And A.Cover_Req_ItemID = C.Itemid And A.Cover_Req_Colorid = C.Colorid and A.Cover_Req_Sizeid = C.Sizeid ";
                            //Str = Str + " Where A.Order_No Like '" + Order_No + "' Group BY A.Cover_Req_Item, A.Cover_Req_Color, A.Cover_Req_Size, ISNULL(A.Cover_Req, 0), A.Cover_Req_ItemID, A.Cover_Req_ColorID, A.Cover_Req_SizeID ";

                            Str = " Select 0 Slno, B.Item, C.COlor, D.Size, 1000000 Tot_Req, 0 Issued, 1000000 Bal_Req, A.Stock, 0.000 Indent_Qty, ";
                            Str = Str + " A.ItemID, A.ColorID, A.SizeID, " + Row + " Slno1, '' T From Socks_Store_Current_Stock_OrderWise()A ";
                            Str = Str + " Left Join Item B On A.ItemID = B.ItemID ";
                            Str = Str + " Left Join Color C On A.ColorID = C.COlorID ";
                            Str = Str + " Left Join Size D On A.SizeID = D.SizeID ";
                            Str = Str + " Where A.Order_No Like '%GENERAL%' ";
                            Str = Str + " And B.Item in ('POLYESTER', 'Rubber', 'Nylon', 'Lycra') ";
                            Str = Str + " Order By A.Order_No, B.Item, C.COlor, D.Size ";
                        }
                    }
                    MyBase.Load_Data(Str, ref DtQty[Row]);
                }
                GridDetail.DataSource = DtQty[Row];
                MyBase.Grid_Designing(ref GridDetail, ref DtQty[Row], "SlNo1", "ItemID", "ColorID", "SizeID", "T");
                MyBase.ReadOnly_Grid_Without(ref GridDetail, "Indent_Qty");
                MyBase.Grid_Colouring(ref GridDetail, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridDetail, 40, 120, 80, 80, 90, 90, 80, 80, 80);

                GridDetail.Columns["Tot_Req"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridDetail.Columns["Issued"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridDetail.Columns["Bal_Req"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridDetail.Columns["Stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridDetail.Columns["Indent_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                GridDetail.RowHeadersWidth = 10;

                if (!MyParent._New && Vis == 1)
                {
                    Iss_Balance();
                    Grid["T", Pos].Value = TxtBalance.Text;
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

        public void Entry_Cancel()
        {
            MyBase.Clear(this);
            GBLOT.Visible = false;
        }

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
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

        private void Grid_DoubleClick(object sender, EventArgs e)
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
                    Txt.GotFocus += new EventHandler(Txt_GotFocus);
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Bom_Qty"].Index)
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

        void Txt_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                {
                    if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value != null && Grid["Order_No", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                    {
                        TxtOrder.Text = Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString();
                        GridLotDetail_Data(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString());
                    }
                    else if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value == null || Grid["Order_No", Grid.CurrentCell.RowIndex].Value == DBNull.Value)
                    {
                        TxtOrder.Text = "";
                        GBLOT.Visible = false;
                    }
                }
                Iss_Balance();
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
                        //Str = " Select Distinct A.Order_No, ISNULL(C.Buyer,'-')Description, 0 Bom_Qty From Socks_Store_Current_Stock_OrderWise()A ";
                        //Str = Str + " Left Join Order_Despatch_Details()B On A.Order_No = B.Order_No Left Join Order_Buyer_Details()C On A.Order_No = C.Order_No ";
                        //Str = Str + " Where Isnull(B.Despatch_Closed, 'N') = 'N' And A.Order_No Not in ('GUP-OCN00000', 'GENERAL') ";
                        //Str = Str + " And A.Order_No In (Select Distinct Order_No From Covering_Req_Status_Orderwise()) Order By A.Order_No ";


                        //Comented On 05-Nov-2016 For Close Stock 
                        //Str = " Select Distinct A.Order_No, ISNULL(C.Descr,'-')Description, ISNULL(C.Buyer,'-')Buyer, 0 Bom_Qty From Covering_Req_Status_Orderwise()A ";
                        //Str = Str + " Left Join Order_Despatch_Details()B On A.Order_No = B.Order_No Left Join (Select Order_No, Max(Descr)Descr, MAX(Buyer)Buyer From Order_Buyer_Details() Group By Order_No)C On A.Order_No = C.Order_No ";
                        //Str = Str + " Left Join Socks_Store_Current_Stock_OrderWise()D On D.Order_No = 'GENERAL' And A.Cover_Req_ItemID = D.ItemID And A.Cover_Req_ColorID = D.ColorID And A.Cover_Req_SizeID = SizeID ";
                        //Str = Str + " Where Isnull(B.Despatch_Closed, 'N') = 'N' And Isnull(D.Stock,0) > 0 ";

                        Str = " Select 'GENERAL' Order_No, 'GENERAL' Description, 'GENERAL' Buyer, 0 Bom_Qty ";    


                        Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Order_NO", Str, String.Empty, 120, 200, 200);
                        if (Dr != null)
                        {
                            Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                            Txt.Text = Dr["Order_No"].ToString();
                            Grid["Descr", Grid.CurrentCell.RowIndex].Value = Dr["Description"].ToString();
                            Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bom_Qty"].ToString();
                            Grid["Record", Grid.CurrentCell.RowIndex].Value = "N";
                            Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                            TxtOrder.Text = Dr["Order_No"].ToString();
                            GridLotDetail_Data(Dr["Order_No"].ToString());
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
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
                if (Dt.Rows.Count > 0)
                {
                    TxtTotal.Text = MyBase.Count(ref Grid, "Slno");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void GridLot()
        {
            String Str = String.Empty;
            try
            {
                if (GridDetail.Rows.Count > 0)
                {
                }
                else
                {
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Slno1", "T", "Record");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Order_No", "Bom_Qty", "Remarks");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 50, 120, 200, 100, 150);
                Grid.Columns["Bom_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid.Columns["Remarks"].DefaultCellStyle.Format = "-";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void GridLotDetail_Data(String Order_NO)
        {
            try
            {
                if (MyParent._New)
                {
                    Str = " Select ROW_NUMBER()Over (Order By Grn_No)Sno, Grn_No, B.Item, C.Color, D.Size, A.LotNo, SUM(Isnull(Cur_Stock,0))Stock ";
                    Str = Str + " From Socks_Store_Current_Stock()A Left Join Item B On A.ItemID = B.ItemID Left Join Color C On A.ColorID = C.ColorID ";
                    Str = Str + " Left Join Size D ON A.SizeID = D.SizeID Where Order_No = 'GENERAL' Group By Grn_No, B.Item, C.Color, D.Size, A.LotNo ";
                }
                else
                {
                    Str = " Select ROW_NUMBER()Over (Order By Grn_No)Sno, Grn_No, B.Item, C.Color, D.Size, A.LotNo, (SUM(Isnull(Cur_Stock,0)) - Sum(Isnull(G.Issued_Qty,0)))Stock ";
                    Str = Str + " From Socks_Store_Current_Stock()A Left Join Item B On A.ItemID = B.ItemID Left Join Color C On A.ColorID = C.ColorID ";
                    Str = Str + " Left Join Size D ON A.SizeID = D.SizeID Left Join VSocks_Order_Lot_Issue_Details_All()G On A.Order_No = G.Order_No And A.LotNo = G.LotNo And A.ItemID = G.ItemID ";
                    Str = Str + " And A.ColorID = G.ColorID And A.SizeID = G.SizeID Where A.Order_No = 'GENERAL' Group By Grn_No, B.Item, C.Color, D.Size, A.LotNo ";
                }
                MyBase.Load_Data(Str, ref DtLot);
                GridLotDetail.DataSource = DtLot;
                MyBase.Grid_Colouring(ref GridLotDetail, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridLotDetail, 30, 120, 120, 80, 80, 80, 80);

                GridLotDetail.RowHeadersWidth = 10;

                GBLOT.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Frm_Yarn_Indent_Request_Covering_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;

                    if (this.ActiveControl.Name == "TxtUnit")
                    {
                        if (TxtUnit.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Select Unit..!", "Gainup");
                            return;
                        }
                        else
                        {
                            DcDate.Focus();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "DcDate")
                    {
                        Grid.CurrentCell = Grid["Order_No", 0];
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
                    if (this.ActiveControl.Name == "TxtUnit")
                    {

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Unit..!", "Select Unit_Name, RowId Unit_Code From Unit_Master Where RowID = 4", String.Empty, 300, 50);

                        if (Dr != null)
                        {
                            TxtUnit.Text = Dr["Unit_Name"].ToString();
                            TxtUnit.Tag = Dr["Unit_Code"].ToString();
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Bom_Qty"].Index)
                    {

                        TxtQty1.Text = Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString();
                        Order_No = Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString();
                        GridDetail_Data(Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value), Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString());
                        if (GridDetail.Rows.Count > 0)
                        {
                            GridDetail.CurrentCell = GridDetail["Indent_Qty", 0];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            e.Handled = true;
                        }
                        return;
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
                    {
                        if (Grid["Remarks", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Remarks", Grid.CurrentCell.RowIndex].Value = "-";
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

        private void Frm_Yarn_Indent_Request_Covering_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtUnit" || this.ActiveControl.Name == "TxtTotal")
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
                MyBase.Row_Number(ref GridDetail);
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
                if (TxtBalance.Text.Trim() == String.Empty || TxtBalance.Text == "0.000")
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    GridDetail.CurrentCell = GridDetail["Indent_Qty", 0];
                    GridDetail.Focus();
                    GridDetail.BeginEdit(true);
                    return;
                }
                for (int i = 0; i <= DtQty[Row1].Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(GridDetail["Stock", i].Value.ToString()) <= 0)
                    {
                        GridDetail["Indent_Qty", i].Value = "0.000";
                    }
                    else if (Convert.ToDouble(GridDetail["Indent_Qty", i].Value.ToString()) > 0)
                    {
                        if (Convert.ToDouble(GridDetail["Indent_Qty", i].Value.ToString()) <= Convert.ToDouble(GridDetail["Bal_Req", i].Value.ToString()) && Convert.ToDouble(GridDetail["Indent_Qty", i].Value.ToString()) <= Convert.ToDouble(GridDetail["Stock", i].Value.ToString()))
                        {

                        }
                        else
                        {
                            MessageBox.Show("Invalid Indent_Qty...!", "Gainup");
                            if (Convert.ToDouble(GridDetail["Bal_Req", i].Value.ToString()) < Convert.ToDouble(GridDetail["Stock", i].Value.ToString()))
                            {
                                GridDetail["Indent_Qty", i].Value = GridDetail["Bal_Req", i].Value;
                            }
                            else if (Convert.ToDouble(GridDetail["Stock", i].Value.ToString()) < Convert.ToDouble(GridDetail["Bal_Req", i].Value.ToString()))
                            {
                                GridDetail["Indent_Qty", i].Value = GridDetail["Stock", i].Value;
                            }
                            else
                            {
                                GridDetail["Indent_Qty", i].Value = GridDetail["Bal_Req", i].Value;
                            }
                            GridDetail.CurrentCell = GridDetail["Indent_Qty", i];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            return;
                        }
                    }
                }
                GBLOT.Visible = false;
                TxtOrder.Text = "";
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Remarks", (Grid.CurrentCell.RowIndex)];
                Grid["T", (Grid.CurrentCell.RowIndex)].Value = TxtBalance.Text;
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
                if (TxtTotal.Text.ToString() != String.Empty && Convert.ToDouble(TxtTotal.Text.ToString()) < 0)
                {
                    MessageBox.Show("Invalid KGS ..!", "Gainup");
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    GBQty.Visible = false;
                    MyParent.Save_Error = true;
                    return;
                }
                for (int i = 0; i <= GridDetail.Rows.Count - 1; i++)
                {
                    if (GridDetail["Indent_Qty", i].Value == DBNull.Value)
                    {
                        MessageBox.Show("Invalid KGS ..!", "Gainup");
                        Grid.CurrentCell = Grid["Order_No", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        GBQty.Visible = false;
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                for (int i = 0; i <= DtQty[Row1].Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(GridDetail["Stock", i].Value.ToString()) <= 0)
                    {
                        GridDetail["Indent_Qty", i].Value = "0.000";
                    }
                    else if (Convert.ToDouble(GridDetail["Indent_Qty", i].Value.ToString()) <= Convert.ToDouble(GridDetail["Bal_Req", i].Value.ToString()) && Convert.ToDouble(GridDetail["Indent_Qty", i].Value.ToString()) <= Convert.ToDouble(GridDetail["Stock", i].Value.ToString()))
                    {

                    }
                    else
                    {
                        MessageBox.Show("Invalid Indent_Qty...!", "Gainup");
                        if (Convert.ToDouble(GridDetail["Bal_Req", i].Value.ToString()) < Convert.ToDouble(GridDetail["Stock", i].Value.ToString()))
                        {
                            GridDetail["Indent_Qty", i].Value = GridDetail["Bal_Req", i].Value;
                        }
                        else if (Convert.ToDouble(GridDetail["Stock", i].Value.ToString()) < Convert.ToDouble(GridDetail["Bal_Req", i].Value.ToString()))
                        {
                            GridDetail["Indent_Qty", i].Value = GridDetail["Stock", i].Value;
                        }
                        else
                        {
                            GridDetail["Indent_Qty", i].Value = GridDetail["Bal_Req", i].Value;
                        }
                        GridDetail.CurrentCell = GridDetail["Indent_Qty", i];
                        GridDetail.Focus();
                        GridDetail.BeginEdit(true);
                        return;
                    }
                }
                GBLOT.Visible = false;
                TxtOrder.Text = "";
                DtQty = new DataTable[30];
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Remarks", (Grid.CurrentCell.RowIndex)];
                Grid.Focus();
                Grid.BeginEdit(true);
                return;
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
                    Txt1.KeyPress += new KeyPressEventHandler(TxtIss_KeyPress);
                    Txt1.GotFocus += new EventHandler(TxtIss_GotFocus);
                    Txt1.KeyDown += new KeyEventHandler(TxtIss_KeyDown);
                }
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

                TxtEnteredWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum_With_Three_Digits(ref GridDetail, "Indent_Qty")));

                if (TxtEnteredWeight.Text.Trim() == String.Empty)
                {
                    TxtEnteredWeight.Text = "0.000";
                }

                TxtBalance.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum_With_Three_Digits(ref GridDetail, "Indent_Qty")));
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
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Indent_Qty"].Index)
                    {
                        if (GridDetail["Indent_Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Indent_Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Indent_Qty...!", "Gainup");
                            GridDetail.CurrentCell = GridDetail["Indent_Qty", Grid.CurrentCell.RowIndex];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            if (Convert.ToDouble(GridDetail["Stock", GridDetail.CurrentCell.RowIndex].Value.ToString()) <= 0)
                            {
                                GridDetail["Indent_Qty", GridDetail.CurrentCell.RowIndex].Value = "0.000";
                            }
                            else if (Convert.ToDouble(GridDetail["Indent_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString()) <= Convert.ToDouble(GridDetail["Bal_Req", GridDetail.CurrentCell.RowIndex].Value.ToString()) && Convert.ToDouble(GridDetail["Indent_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString()) <= Convert.ToDouble(GridDetail["Stock", GridDetail.CurrentCell.RowIndex].Value.ToString()))
                            {

                            }
                            else
                            {
                                MessageBox.Show("Invalid Indent_Qty...!", "Gainup");
                                if (Convert.ToDouble(GridDetail["Bal_Req", GridDetail.CurrentCell.RowIndex].Value.ToString()) < Convert.ToDouble(GridDetail["Stock", GridDetail.CurrentCell.RowIndex].Value.ToString()))
                                {
                                    GridDetail["Indent_Qty", GridDetail.CurrentCell.RowIndex].Value = GridDetail["Bal_Req", GridDetail.CurrentCell.RowIndex].Value;
                                }
                                else if (Convert.ToDouble(GridDetail["Stock", GridDetail.CurrentCell.RowIndex].Value.ToString()) < Convert.ToDouble(GridDetail["Bal_Req", GridDetail.CurrentCell.RowIndex].Value.ToString()))
                                {
                                    GridDetail["Indent_Qty", GridDetail.CurrentCell.RowIndex].Value = GridDetail["Stock", GridDetail.CurrentCell.RowIndex].Value;
                                }
                                else
                                {
                                    GridDetail["Indent_Qty", GridDetail.CurrentCell.RowIndex].Value = GridDetail["Bal_Req", GridDetail.CurrentCell.RowIndex].Value;
                                }
                                GridDetail.CurrentCell = GridDetail["Indent_Qty", GridDetail.CurrentCell.RowIndex];
                                GridDetail.Focus();
                                GridDetail.BeginEdit(true);
                                return;
                            }
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
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Indent_Qty"].Index)
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
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Indent_Qty"].Index)
                {
                    if (GridDetail["Indent_Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Indent_Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value)
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

    }
}
