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
    public partial class Frm_Yarn_Indent_Issue_Covering : Form, Entry
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

        public Frm_Yarn_Indent_Issue_Covering()
        {
            InitializeComponent();
        }

        private void Frm_Yarn_Indent_Issue_Covering_Load(object sender, EventArgs e)
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

        void Set_Min_Max_Date(Boolean Condition)
        {
            try
            {
                DataTable Tdt = new DataTable();
                if (Condition)
                {
                    MyBase.Load_Data("Select Cast(GetDate() as Date) MinDate, DateAdd(D, 3, Cast(GetDate() as Date)) MaxDate ", ref Tdt);
                }
                else
                {

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
                DtQty = new DataTable[30];

                Str = " Select Distinct S1.EntryNo, S1.EntryDate, U1.Unit_Name, S2.Order_No, S1.ProductionDate, S1.Remarks, S1.UnitCode, S1.RowID, S1.RowID Indent_Master_MasterID, Cast(Getdate() as Date)IssueDate ";
                Str = Str + " From Socks_Covering_Yarn_Indent_Request_Master S1 Left Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID ";
                Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details S3 On S1.RowID = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                Str = Str + " Left Join Unit_Master U1 On S1.UnitCode = U1.RowId Left Join Fit_Order_Status S4 On S2.Order_No = S4.Order_No ";
                Str = Str + " Where (ISnull(S3.Issue_Closed, 'C') = 'P' Or ISnull(S3.Issue_Closed, 'C') = 'PC') And Isnull(S4.Status,'N') = 'N' And S1.Deptcode = 51 ";
                
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Indent Issue Entry - New", Str, String.Empty, 80, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    if (Dt.Rows.Count > 0)
                    {
                        Grid.CurrentCell = Grid["Issue_Qty", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                    }
                    else
                    {
                        MessageBox.Show("Total Order Req Closed!...Gainup");
                        return;
                    }
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Covering Yarn Indent Issue Entry - Edit", "Select S1.EntryNo, S1.IssueNo, S1.IssueDate, U1.Unit_Name, S2.Order_No, S1.Remarks, S1.UnitCode, S1.RowID, S1.Indent_Master_MasterID From Socks_Covering_Yarn_Indent_Issue_Master S1 Inner Join Socks_Covering_Yarn_Indent_Issue_Details S2 On S1.RowID = S2.MasterID Left Join Unit_Master U1 On S1.UnitCode = U1.RowId", String.Empty, 80, 100, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Issue_Qty", 0];
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

                Str = " Select Top 10000000000 F.Unit_Name, B.Order_No, It.Item, Co.Color, Si.Size, G.IndentNo, D.Indent_Qty, ";
                Str = Str + " Sum(Isnull(C.Weight,0))Issue_Qty From Socks_Covering_Yarn_Indent_Issue_Master A ";
                Str = Str + " Left Join Socks_Covering_Yarn_Indent_Issue_Details B On A.RowID = B.MasterID ";
                Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Issue_Details C On A.RowID = C.MasterID And B.SlNo1 = C.Slno1 ";
                Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details D On B.Indent_Request_Details_RowID = D.RowID And B.ItemID = D.ItemID And B.ColorID = D.ColorID And B.SizeID = D.SizeID ";
                Str = Str + " Left Join Socks_Covering_Yarn_Indent_Request_Master G On D.MasterID = G.RowID "; 
                Str = Str + " Left Join FITSOCKS.DBO.Covering_Req_Status_Orderwise()E On B.Order_No = E.Order_No And B.ItemID = E.Cover_Req_ItemID And B.ColorID = E.Cover_Req_ColorID And B.SizeID = E.Cover_Req_SizeID ";
                Str = Str + " Left Join Item It On D.ItemID = It.ItemID Left Join Color Co On D.ColorID = Co.ColorID Left Join Size Si On D.SizeID = Si.SizeID ";
                Str = Str + " Left Join Unit_Master F On A.UnitCode = F.RowId ";
                Str = Str + " Where A.RowID = " + Code + " Group By F.Unit_Name, B.Order_No, It.Item, Co.COlor, Si.Size, G.IndentNo, D.Indent_Qty Order By It.Item, Co.COlor, Si.Size ";

                MyBase.Execute_Qry(Str, "Yarn_Indent_Issue_Receipt");

                DataTable Dt1 = new DataTable();
                String Str1 = "Select Getdate()Date1";
                MyBase.Load_Data(Str1, ref Dt1);

                DataTable Dt2 = new DataTable();
                String Str2 = "Select IssueNo, IssueDate From Socks_Covering_Yarn_Indent_Issue_Master Where RowID = " + Code + "";
                MyBase.Load_Data(Str2, ref Dt2);

                CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Cover_Yarn_Indent_Issue.rpt");
                MyParent.FormulaFill(ref ObjRpt, "PrintOutDate", Dt1.Rows[0][0].ToString());
                MyParent.FormulaFill(ref ObjRpt, "IssueDate", Dt2.Rows[0][1].ToString());
                MyParent.FormulaFill(ref ObjRpt, "IssueNo", Dt2.Rows[0][0].ToString());
                MyParent.CReport(ref ObjRpt, "Covering Yarn Indent Issue..!");
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Covering Yarn Indent Issue Entry - View", "Select S1.EntryNo, S1.IssueNo, S1.IssueDate, U1.Unit_Name, S2.Order_No, S1.Remarks, S1.UnitCode, S1.RowID, S1.Indent_Master_MasterID From Socks_Covering_Yarn_Indent_Issue_Master S1 Inner Join Socks_Covering_Yarn_Indent_Issue_Details S2 On S1.RowID = S2.MasterID Left Join Unit_Master U1 On S1.UnitCode = U1.RowId Where S1.Deptcode = 51", String.Empty, 80, 100, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Issue_Qty", 0];
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
                DataTable Is1 = new DataTable();
                DataTable Is2 = new DataTable();

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
                    if (Grid["Issue_Qty", i].Value == DBNull.Value || Grid["Issue_Qty", i].Value.ToString() == String.Empty || Grid["Issue_Qty", i].Value.ToString() == "0")
                    {
                        Grid["Issue_Qty", i].Value = "0";
                    }
                    else if (Convert.ToDouble(Grid["Issue_Qty", i].Value.ToString()) + Convert.ToDouble(Grid["Balance_Qty", i].Value.ToString()) != Convert.ToDouble(Grid["Issue_Qty1", i].Value.ToString()))
                    {
                        Grid["Issue_Qty", i].Value = "0";
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDecimal(Grid["Issue_Qty", i].Value.ToString()) > 0)
                    {
                        if (Convert.ToDecimal(Grid["Issue_Qty1", i].Value.ToString()) != (Convert.ToDecimal(Grid["Issue_Qty", i].Value.ToString()) + Convert.ToDecimal(Grid["Balance_Qty", i].Value.ToString())))
                        {
                            MessageBox.Show("Invalid Details", "Gainup");
                            Grid.CurrentCell = Grid["Order_No", 0];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDecimal(Grid["Issue_Qty", i].Value.ToString()) > 0 && DtQty[Convert.ToInt32(Dt.Rows[i]["SlNo1"])] == null)
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
                    TxtEntryNo.Text = MyBase.MaxOnlyWithoutComp("Socks_Covering_Yarn_Indent_Issue_Master", "EntryNO", String.Empty, String.Empty, 0).ToString();
                }
                Queries = new string[Dt.Rows.Count * 100];

                if (MyParent._New)
                {
                    Str = " Select Cast('GUP-SCI' As Varchar(20)) + RIGHT('00000' + Cast(Isnull(Max(Cast(Replace(IssueNo,'GUP-SCI','') As Numeric(20))),0) + 1 As Varchar(20)), 5)IssueNO from Socks_Covering_Yarn_Indent_Issue_Master ";
                    MyBase.Load_Data(Str, ref Is1);
                }

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_Covering_Yarn_Indent_Issue_Master(EntryNO, IssueNO, IssueDate, UnitCode, SystemName, EntryTime, UserCode, Compcode, Indent_Master_MasterID, DeptCode, Remarks) values (" + TxtEntryNo.Text + ", '" + Is1.Rows[0][0].ToString() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtUnit.Tag + ", Host_Name(), GetDate(), " + MyParent.UserCode + ", " + MyParent.CompCode + ", " + TxtEntryNo.Tag + ", 51, '" + TxtRemarks.Text + "'); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_Covering_Yarn_Indent_Issue_Master", "ADD", "@@IDENTITY");
                }
                else
                {
                    //Queries[Array_Index++] = "Exec Update_Socks_Yarn_BOM_Status_Prod_Issue " + Code;
                    Queries[Array_Index++] = "Update Socks_Covering_Yarn_Indent_Issue_Master Set IssueDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', Remarks = '" + TxtRemarks.Text + "', UserCode = " + MyParent.UserCode + ", DeptCode = 51 Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_Covering_Yarn_Indent_Issue_Master", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from Socks_Covering_Yarn_Indent_Issue_Details where MasterID = " + Code;
                    Queries[Array_Index++] = "Delete from Socks_Covering_Yarn_Indent_SampleWise_Issue_Details where MasterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Socks_Covering_Yarn_Indent_Issue_Details (MasterID, Slno, Order_NO, ItemID, ColorID, SizeID, Issue_Qty, Indent_Request_Details_RowID, Slno1, Remarks) Values (@@IDENTITY, " + Grid["Slno", i].Value + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + ", " + Grid["SizeID", i].Value + ", '" + Grid["Issue_Qty", i].Value + "', " + Grid["Indent_Request_Details_RowID", i].Value + ", " + Grid["Slno1", i].Value + ", '" + Grid["Remarks", i].Value + "')";
                        if (Convert.ToDecimal(Grid["Issue_Qty", i].Value) > 0)
                        {
                            //Queries[Array_Index++] = "Update Socks_Yarn_Indent_SampleWise_Requset_Details Set Issue_Closed = (Case When Cast('" + Grid["Issue_Qty", i].Value + "' As Numeric(20,4)) = Cast('" + Grid["Indent_Qty", i].Value + "' As Numeric(20,4)) Then 'C' Else 'P' End) Where RowID =  " + Grid["Indent_Request_Details_RowID", i].Value + "";
                            Queries[Array_Index++] = " Update A Set A.Issue_Closed = (Case When Isnull(A.Indent_Qty,0) = (ISNULL(B.Issue_Qty, 0) + ISNULL(" + Grid["Issue_Qty", i].Value + ", 0)) Then 'C' Else 'PC' End) From Socks_Covering_Yarn_Indent_SampleWise_Request_Details A Left Join (Select Indent_Request_Details_RowID, Sum(Isnull(Issue_qty,0))Issue_qty From FITSOCKS.DBO.Socks_Store_Covering_Yarn_EntryWise_Indent_Issue_Details() Where EntryNo <> " + TxtEntryNo.Text + " Group By Indent_Request_Details_RowID) B On A.RowID = B.Indent_Request_Details_RowID Where A.RowID = " + Grid["Indent_Request_Details_RowID", i].Value + " ";
                        }
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Socks_Covering_Yarn_Indent_Issue_Details (MasterID, Slno, Order_NO, ItemID, ColorID, SizeID, Issue_Qty, Indent_Request_Details_RowID, Slno1, Remarks) Values (" + Code + ", " + Grid["Slno", i].Value + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + ", " + Grid["SizeID", i].Value + ", '" + Grid["Issue_Qty", i].Value + "', " + Grid["Indent_Request_Details_RowID", i].Value + ", " + Grid["Slno1", i].Value + ", '" + Grid["Remarks", i].Value + "')";
                        if (Convert.ToDecimal(Grid["Issue_Qty", i].Value) > 0)
                        {
                            Queries[Array_Index++] = " Update A Set A.Issue_Closed = (Case When Isnull(A.Indent_Qty,0) = (ISNULL(B.Issue_Qty, 0) + ISNULL(" + Grid["Issue_Qty", i].Value + ", 0)) Then 'C' Else 'PC' End) From Socks_Covering_Yarn_Indent_SampleWise_Request_Details A Left Join (Select Indent_Request_Details_RowID, Sum(Isnull(Issue_qty,0))Issue_qty From FITSOCKS.DBO.Socks_Store_Covering_Yarn_EntryWise_Indent_Issue_Details() Where EntryNo <> " + TxtEntryNo.Text + " Group By Indent_Request_Details_RowID) B On A.RowID = B.Indent_Request_Details_RowID Where A.RowID = " + Grid["Indent_Request_Details_RowID", i].Value + " ";
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
                                    if (Convert.ToDouble(DtQty[i].Rows[j]["Issue_Qty"].ToString()) > 0)
                                    {

                                        DataTable Dt1 = new DataTable();

                                        Str = " Select Grn_Date, Grn_No, LotNo, BagNo, Cur_Stock Stock, VSocks_Lot_Bag_Details_RowID From Socks_Store_Current_Stock() ";
                                        Str = Str + " Where Order_No = 'GENERAL' And ItemID = " + Grid["ItemID", i - 1].Value + " And ColorID = " + Grid["ColorID", i - 1].Value + " And SizeID = " + Grid["SizeID", i - 1].Value + " And LotNo = '" + DtQty[i].Rows[j]["LotNO"].ToString() + "' And Grn_No = '" + DtQty[i].Rows[j]["Grn_NO"].ToString() + "'";
                                        Str = Str + " Order By Grn_Date, Grn_No, LotNo, BagNo ";

                                        MyBase.Load_Data(Str, ref Dt1);
                                        if (Dt1.Rows.Count > 0)
                                        {
                                            Double Tot_Iss_Qty = 0.000;
                                            Double Iss_Qty = 0.000;
                                            Double Bal_Qty = 0.000;
                                            int l = 0;

                                            Tot_Iss_Qty = Convert.ToDouble(DtQty[i].Rows[j]["Issue_Qty"].ToString());
                                            Bal_Qty = Convert.ToDouble(DtQty[i].Rows[j]["Issue_Qty"].ToString());

                                            while (Convert.ToDouble(Tot_Iss_Qty) >= Convert.ToDouble(Iss_Qty) && Math.Round(Bal_Qty, 3) > 0)
                                            {
                                                Iss_Qty = Iss_Qty + Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString());

                                                if (Iss_Qty <= Tot_Iss_Qty)
                                                {
                                                    Queries[Array_Index++] = "Insert Into Socks_Covering_Yarn_Indent_SampleWise_Issue_Details (MasterID, SlNo, SlNo1, LotNo, Weight, BagNo, VSocks_Lot_Bag_Details_RowID, Grn_No) Values (@@IDENTITY, " + DtQty[i].Rows[j]["Slno"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i].Rows[j]["LotNo"].ToString() + "', " + Dt1.Rows[l]["Stock"].ToString() + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ", '" + Dt1.Rows[l]["Grn_No"].ToString() + "')";
                                                    Queries[Array_Index++] = "Update A Set Prod_Issue = Prod_Issue + Cast(" + Dt1.Rows[l]["Stock"].ToString() + " As Numeric(20,3)) From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and Size_ID = " + Grid["SizeID", i - 1].Value + "";
                                                    Bal_Qty = Bal_Qty - Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString());
                                                }
                                                else if ((Iss_Qty > Tot_Iss_Qty) && (Math.Round(Bal_Qty, 3) > 0))
                                                {
                                                    if (Bal_Qty <= Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()))
                                                    {
                                                        Queries[Array_Index++] = "Insert Into Socks_Covering_Yarn_Indent_SampleWise_Issue_Details (MasterID, SlNo, SlNo1, LotNo, Weight, BagNo, VSocks_Lot_Bag_Details_RowID, Grn_No) Values (@@IDENTITY, " + DtQty[i].Rows[j]["Slno"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i].Rows[j]["LotNo"].ToString() + "', " + Bal_Qty + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ", '" + Dt1.Rows[l]["Grn_No"].ToString() + "')";
                                                        Queries[Array_Index++] = "Update A Set Prod_Issue = Prod_Issue + Cast(" + Bal_Qty + " As Numeric(20,3)) From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and Size_ID = " + Grid["SizeID", i - 1].Value + "";
                                                        Bal_Qty = 0;
                                                    }
                                                    else if (Bal_Qty > Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()))
                                                    {
                                                        Queries[Array_Index++] = "Insert Into Socks_Covering_Yarn_Indent_SampleWise_Issue_Details (MasterID, SlNo, SlNo1, LotNo, Weight, BagNo, VSocks_Lot_Bag_Details_RowID, Grn_No) Values (@@IDENTITY, " + DtQty[i].Rows[j]["Slno"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i].Rows[j]["LotNo"].ToString() + "', " + Dt1.Rows[l]["Stock"].ToString() + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ", '" + Dt1.Rows[l]["Grn_No"].ToString() + "')";
                                                        Queries[Array_Index++] = "Update A Set Prod_Issue = Prod_Issue + Cast(" + Dt1.Rows[l]["Stock"].ToString() + " As Numeric(20,3)) From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and Size_ID = " + Grid["SizeID", i - 1].Value + "";
                                                        Bal_Qty = Bal_Qty - Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString());
                                                    }
                                                }

                                                l = l + 1;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (Convert.ToDouble(DtQty[i].Rows[j]["Issue_Qty"].ToString()) > 0)
                                    {
                                        DataTable Dt1 = new DataTable();

                                        Str = " Select Grn_Date, Grn_No, LotNo, BagNo, Stock, VSocks_Lot_Bag_Details_RowID From Socks_Store_Current_Stock_For_Issue_Edit('GENERAL', " + Grid["ItemID", i - 1].Value + ", " + Grid["ColorID", i - 1].Value + ", " + Grid["SizeID", i - 1].Value + ", '" + DtQty[i].Rows[j]["Grn_No"].ToString() + "', '" + DtQty[i].Rows[j]["LotNO"].ToString() + "') ";

                                        MyBase.Load_Data(Str, ref Dt1);
                                        if (Dt1.Rows.Count > 0)
                                        {
                                            Double Tot_Iss_Qty = 0.000;
                                            Double Iss_Qty = 0.000;
                                            Double Bal_Qty = 0.000;
                                            int l = 0;

                                            Tot_Iss_Qty = Convert.ToDouble(DtQty[i].Rows[j]["Issue_Qty"].ToString());
                                            Bal_Qty = Convert.ToDouble(DtQty[i].Rows[j]["Issue_Qty"].ToString());

                                            while (Convert.ToDouble(Tot_Iss_Qty) >= Convert.ToDouble(Iss_Qty) && Math.Round(Bal_Qty, 3) > 0)
                                            {
                                                Iss_Qty = Iss_Qty + Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString());

                                                if (Iss_Qty <= Tot_Iss_Qty)
                                                {
                                                    Queries[Array_Index++] = "Insert Into Socks_Covering_Yarn_Indent_SampleWise_Issue_Details (MasterID, SlNo, SlNo1, LotNo, Weight, BagNo, VSocks_Lot_Bag_Details_RowID, Grn_NO) Values (" + Code + ", " + DtQty[i].Rows[j]["Slno"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i].Rows[j]["LotNo"].ToString() + "', " + Dt1.Rows[l]["Stock"].ToString() + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ", '" + Dt1.Rows[l]["Grn_No"].ToString() + "')";
                                                    Queries[Array_Index++] = "Update A Set Prod_Issue = Prod_Issue + Cast(" + Dt1.Rows[l]["Stock"].ToString() + " As Numeric(20,3)) From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and Size_ID = " + Grid["SizeID", i - 1].Value + "";
                                                    Bal_Qty = Bal_Qty - Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString());
                                                }
                                                else if ((Iss_Qty > Tot_Iss_Qty) && (Math.Round(Bal_Qty, 3) > 0))
                                                {
                                                    if (Bal_Qty <= Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()))
                                                    {
                                                        Queries[Array_Index++] = "Insert Into Socks_Covering_Yarn_Indent_SampleWise_Issue_Details (MasterID, SlNo, SlNo1, LotNo, Weight, BagNo, VSocks_Lot_Bag_Details_RowID, Grn_NO) Values (" + Code + ", " + DtQty[i].Rows[j]["Slno"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i].Rows[j]["LotNo"].ToString() + "', " + Bal_Qty + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ", '" + Dt1.Rows[l]["Grn_No"].ToString() + "')";
                                                        Queries[Array_Index++] = "Update A Set Prod_Issue = Prod_Issue + Cast(" + Bal_Qty + " As Numeric(20,3)) From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and Size_ID = " + Grid["SizeID", i - 1].Value + "";
                                                        Bal_Qty = 0;
                                                    }
                                                    else if (Bal_Qty > Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()))
                                                    {
                                                        Queries[Array_Index++] = "Insert Into Socks_Covering_Yarn_Indent_SampleWise_Issue_Details (MasterID, SlNo, SlNo1, LotNo, Weight, BagNo, VSocks_Lot_Bag_Details_RowID, Grn_NO) Values (" + Code + ",, " + DtQty[i].Rows[j]["Slno"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i].Rows[j]["LotNo"].ToString() + "', " + Dt1.Rows[l]["Stock"].ToString() + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ", '" + Dt1.Rows[l]["Grn_No"].ToString() + "')";
                                                        Queries[Array_Index++] = "Update A Set Prod_Issue = Prod_Issue + Cast(" + Dt1.Rows[l]["Stock"].ToString() + " As Numeric(20,3)) From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and Size_ID = " + Grid["SizeID", i - 1].Value + "";
                                                        Bal_Qty = Bal_Qty - Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString());
                                                    }
                                                }

                                                l = l + 1;
                                            }
                                        }
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
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Covering Yarn Indent Issue Entry - Delete", "Select S1.EntryNo, S1.IssueNo, S1.IssueDate, S2.Order_No, U1.Unit_Name, S1.Remarks, S1.UnitCode, S1.RowID, S1.Indent_Master_MasterID From Socks_Covering_Yarn_Indent_Issue_Master S1 Inner Join Socks_Covering_Yarn_Indent_Issue_Details S2 On S1.RowID = S2.MasterID Left Join Unit_Master U1 On S1.UnitCode = U1.RowId Where S1.DeptCode = 51", String.Empty, 80, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    Entry_Delete_Confirm();
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
                    String RowID = String.Empty;
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (RowID.ToString() == String.Empty)
                        {
                            RowID = Grid["Indent_Request_Details_RowID", i].Value.ToString();
                        }
                        else
                        {
                            RowID = RowID + "," + Grid["Indent_Request_Details_RowID", i].Value.ToString();
                        }
                    }

                    //MyBase.Run("Exec Update_Socks_Yarn_BOM_Status_Prod_Issue " + Code, "Delete from Socks_Yarn_Indent_SampleWise_Issue_Details where MasterID = " + Code, "Delete from Socks_Yarn_Indent_Issue_Details where MasterID = " + Code, "Delete from Socks_Yarn_Indent_Issue_Master where RowID = " + Code, "Update Socks_Yarn_Indent_SampleWise_Requset_Details Set Issue_Closed = 'P' where RowID in (" + RowID + ")");
                    MyBase.Run("Delete from Socks_Covering_Yarn_Indent_SampleWise_Issue_Details where MasterID = " + Code, "Delete from Socks_Covering_Yarn_Indent_Issue_Details where MasterID = " + Code, "Delete from Socks_Covering_Yarn_Indent_Issue_Master where RowID = " + Code, "Update Socks_Covering_Yarn_Indent_SampleWise_Request_Details Set Issue_Closed = 'P' where RowID in (" + RowID + ")");
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
                TxtEntryNo.Tag = Dr["Indent_Master_MasterID"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["IssueDate"]);
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

        void Grid_Data()
        {
            String Str = String.Empty;
            try
            {
                if (MyParent._New == true)
                {
                    //Str = " Select S2.Slno, S2.Order_No, I.Item, C.Color, S.Size, Isnull(S4.Cover_Req, 0) Tot_Req, Isnull(S7.Issue_Qty, 0) Issued, (Isnull(S4.Cover_Req, 0) - Isnull(S7.Issue_Qty,0)) Bal_Req, (Isnull(S6.Stock, 0) + Isnull(S61.Stock, 0)) Stock, ";
                    //Str = Str + " (Case When (Isnull(S4.Cover_Req, 0) - Isnull(S7.Issue_Qty, 0)) < S3.Indent_Qty Then (Isnull(S4.Cover_Req, 0) - Isnull(S7.Issue_Qty,0)) Else S3.Indent_Qty End)Indent_Qty, ";
                    //Str = Str + " Isnull(S5.Issue_Qty,0)Indent_Issued, (Case When (Isnull(S4.Cover_Req, 0) - Isnull(S7.Issue_Qty,0)) < (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) Then (Isnull(S4.Cover_Req, 0) - Isnull(S7.Issue_Qty,0)) Else (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) End)Pending_Qty, "; 
                    //Str = Str + " Cast(0.000 As Numeric(20, 3))Issue_Qty, (Case When (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) < (Isnull(S6.Stock,0) + Isnull(S61.Stock,0)) Then Cast((Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) As Numeric(20,3)) Else Cast((Isnull(S6.Stock, 0) + Isnull(S61.Stock, 0)) As Numeric(20,3)) End) Issue_Qty1, ";
                    //Str = Str + " (Case When (Isnull(S3.Indent_Qty,0) - Isnull(S5.Issue_Qty,0)) < (Isnull(S6.Stock,0) + Isnull(S61.Stock,0)) Then Cast((Isnull(S3.Indent_Qty,0) - Isnull(S5.Issue_Qty,0)) As Numeric(20,3)) Else Cast((Isnull(S6.Stock,0) + Isnull(S61.Stock,0)) As Numeric(20,3)) End) Balance_Qty, ";
                    //Str = Str + " S3.ItemID, S3.ColorID, S3.SizeID, ROW_NUMBER() Over(Order By S2.Slno) Slno1, S3.RowID Indent_Request_Details_RowID, S2.Remarks, '-' T, 'O' Record "; 
                    //Str = Str + " From Socks_Covering_Yarn_Indent_Request_Master S1 Left Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID "; 
                    //Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details S3 On S1.RowID = S3.MasterID And S2.MasterID = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Covering_Req_Status_Orderwise()S4 On S2.Order_No = S4.Order_No And S3.ItemID = S4.Cover_Req_ItemID And S3.ColorID = S4.Cover_Req_ColorID And S3.SizeID = S4.Cover_Req_SizeID ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Item I On S4.Cover_Req_ItemID = I.ItemID Left Join FITSOCKS.DBO.Color C On S4.Cover_Req_ColorID = C.ColorID Left Join FITSOCKS.DBO.Size S On S4.Cover_Req_SizeID = S.SizeID ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Covering_Yarn_Lot_Indent_Issue_Details()S5 On S3.RowID = S5.Indent_Request_Details_RowID ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Current_Stock_OrderWise()S6 On S2.Order_No = S6.Order_No And S3.ItemID = S6.ItemID And S3.ColorID = S6.ColorID And S3.SizeID = S6.SizeID ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Current_Stock_OrderWise()S61 On S61.Order_No = 'GENERAL' And S3.ItemID = S61.ItemID And S3.ColorID = S61.ColorID And S3.SizeID = S61.SizeID ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Covering_Indent_Issued_OrderWise()S7 On S2.Order_No = S7.Order_No And S3.ItemID = S7.ItemID And S3.ColorID = S7.ColorID And S3.SizeID = S7.SizeID ";
                    //Str = Str + " Where (S3.Issue_Closed = 'P' Or S3.Issue_Closed = 'PC') And (Case When (Isnull(S4.Cover_Req, 0) - Isnull(S7.Issue_Qty,0)) < (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) Then (Isnull(S4.Cover_Req, 0) - Isnull(S7.Issue_Qty,0)) Else (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) End) != 0 And S1.RowId = " + Code ;

                    //Commented On 05-Nov-2016
                    //Str = Str + " Select S2.Slno, S2.Order_No, I.Item, C.Color, S.Size, Isnull(S4.Cover_Req, 0) Tot_Req, Isnull(S7.Issue_Qty, 0) Issued, (Isnull(S4.Cover_Req, 0) - Isnull(S7.Issue_Qty,0)) Bal_Req, Isnull(S6.Stock, 0) Stock, "; 
                    //Str = Str + " (Case When (Isnull(S4.Cover_Req, 0) - Isnull(S7.Issue_Qty, 0)) < S3.Indent_Qty Then (Isnull(S4.Cover_Req, 0) - Isnull(S7.Issue_Qty,0)) Else S3.Indent_Qty End)Indent_Qty, ";
                    //Str = Str + " Isnull(S5.Issue_Qty,0)Indent_Issued, (Case When (Isnull(S4.Cover_Req, 0) - Isnull(S7.Issue_Qty,0)) < (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) Then (Isnull(S4.Cover_Req, 0) - Isnull(S7.Issue_Qty,0)) Else (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) End)Pending_Qty, "; 
                    //Str = Str + " Cast(0.000 As Numeric(20, 3))Issue_Qty, (Case When (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) < Isnull(S6.Stock,0) Then Cast((Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) As Numeric(20,3)) Else Cast(Isnull(S6.Stock, 0) As Numeric(20,3)) End) Issue_Qty1, ";
                    //Str = Str + " (Case When (Isnull(S3.Indent_Qty,0) - Isnull(S5.Issue_Qty,0)) < Isnull(S6.Stock,0) Then Cast((Isnull(S3.Indent_Qty,0) - Isnull(S5.Issue_Qty,0)) As Numeric(20,3)) Else Cast(Isnull(S6.Stock,0) As Numeric(20,3)) End) Balance_Qty, ";
                    //Str = Str + " S3.ItemID, S3.ColorID, S3.SizeID, ROW_NUMBER() Over(Order By S2.Slno) Slno1, S3.RowID Indent_Request_Details_RowID, S2.Remarks, '-' T, 'O' Record ";
                    //Str = Str + " From Socks_Covering_Yarn_Indent_Request_Master S1 Left Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID ";
                    //Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details S3 On S1.RowID = S3.MasterID And S2.MasterID = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Covering_Req_Status_Orderwise()S4 On S2.Order_No = S4.Order_No And S3.ItemID = S4.Cover_Req_ItemID And S3.ColorID = S4.Cover_Req_ColorID And S3.SizeID = S4.Cover_Req_SizeID ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Item I On S4.Cover_Req_ItemID = I.ItemID Left Join FITSOCKS.DBO.Color C On S4.Cover_Req_ColorID = C.ColorID Left Join FITSOCKS.DBO.Size S On S4.Cover_Req_SizeID = S.SizeID ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Covering_Yarn_Lot_Indent_Issue_Details()S5 On S3.RowID = S5.Indent_Request_Details_RowID ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Current_Stock_OrderWise()S6 On S6.Order_No = 'GENERAL' And S3.ItemID = S6.ItemID And S3.ColorID = S6.ColorID And S3.SizeID = S6.SizeID ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Covering_Indent_Issued_OrderWise()S7 On S2.Order_No = S7.Order_No And S3.ItemID = S7.ItemID And S3.ColorID = S7.ColorID And S3.SizeID = S7.SizeID ";
                    //Str = Str + " Where (S3.Issue_Closed = 'P' Or S3.Issue_Closed = 'PC') And (Case When (Isnull(S4.Cover_Req, 0) - Isnull(S7.Issue_Qty,0)) < (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) Then (Isnull(S4.Cover_Req, 0) - Isnull(S7.Issue_Qty,0)) Else (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) End) != 0 And S1.RowId = " + Code;

                    Str = " Select S2.Slno, S2.Order_No, I.Item, C.Color, S.Size, 100000 Tot_Req, Isnull(S7.Issue_Qty, 0) Issued, (100000 - Isnull(S7.Issue_Qty,0)) Bal_Req, Isnull(S6.Stock, 0) Stock, ";
                    Str = Str + " (Case When (100000 - Isnull(S7.Issue_Qty, 0)) < S3.Indent_Qty Then (100000 - Isnull(S7.Issue_Qty,0)) Else S3.Indent_Qty End)Indent_Qty, Isnull(S5.Issue_Qty,0)Indent_Issued, ";
                    Str = Str + " (Case When (100000 - Isnull(S7.Issue_Qty,0)) < (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) Then (100000 - Isnull(S7.Issue_Qty,0)) Else (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) End)Pending_Qty, ";
                    Str = Str + " Cast(0.000 As Numeric(20, 3))Issue_Qty, (Case When (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) < Isnull(S6.Stock,0) Then Cast((Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) As Numeric(20,3)) Else Cast(Isnull(S6.Stock, 0) As Numeric(20,3)) End) Issue_Qty1, ";
                    Str = Str + " (Case When (Isnull(S3.Indent_Qty,0) - Isnull(S5.Issue_Qty,0)) < Isnull(S6.Stock,0) Then Cast((Isnull(S3.Indent_Qty,0) - Isnull(S5.Issue_Qty,0)) As Numeric(20,3)) Else Cast(Isnull(S6.Stock,0) As Numeric(20,3)) End) Balance_Qty, ";
                    Str = Str + " S3.ItemID, S3.ColorID, S3.SizeID, ROW_NUMBER() Over(Order By S2.Slno) Slno1, S3.RowID Indent_Request_Details_RowID, S2.Remarks, '-' T, 'O' Record ";
                    Str = Str + " From Socks_Covering_Yarn_Indent_Request_Master S1 "; 
                    Str = Str + " Left Join Socks_Covering_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID ";
                    Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details S3 On S1.RowID = S3.MasterID And S2.MasterID = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                    Str = Str + " Left Join FITSOCKS.DBO.Item I On S3.ItemID= I.ItemID ";
                    Str = Str + " Left Join FITSOCKS.DBO.Color C On S3.ColorID = C.ColorID "; 
                    Str = Str + " Left Join FITSOCKS.DBO.Size S On S3.SizeID = S.SizeID ";
                    Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Covering_Yarn_Lot_Indent_Issue_Details()S5 On S3.RowID = S5.Indent_Request_Details_RowID ";
                    Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Current_Stock_OrderWise()S6 On S6.Order_No = 'GENERAL' And S3.ItemID = S6.ItemID And S3.ColorID = S6.ColorID And S3.SizeID = S6.SizeID ";
                    Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Covering_Indent_Issued_OrderWise()S7 On S2.Order_No = S7.Order_No And S3.ItemID = S7.ItemID And S3.ColorID = S7.ColorID And S3.SizeID = S7.SizeID ";
                    Str = Str + " Where (S3.Issue_Closed = 'P' Or S3.Issue_Closed = 'PC') And (Case When (100000 - Isnull(S7.Issue_Qty,0)) < (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) ";
                    Str = Str + " Then (100000 - Isnull(S7.Issue_Qty,0)) Else (Isnull(S3.Indent_Qty, 0) - Isnull(S5.Issue_Qty, 0)) End) != 0 And S1.RowId = " + Code;
                }
                else
                {
                    //Str = " Select B.Slno, B.Order_No, It.Item, Co.Color, Si.Size, Isnull(D.Cover_Req, 0) Tot_Req, ISnull(E.Issue_Qty, 0) Issued, (Isnull(D.Cover_Req, 0) - ISnull(E.Issue_Qty, 0)) Bal_Req, ";
                    //Str = Str + " ((ISNULL(F.Stock, 0) + ISNULL(F1.Stock, 0)) + ISNULL(B.Issue_Qty,0)) Stock, (Case When (Isnull(D.Cover_Req, 0) - Isnull(E.Issue_Qty,0)) < G.Indent_Qty Then (Isnull(D.Cover_Req, 0) - Isnull(E.Issue_Qty,0)) Else G.Indent_Qty End)Indent_Qty, Isnull(H.Issue_Qty,0)Indent_Issued, ";
                    //Str = Str + " (Case When (ISNULL(G.Indent_Qty,0) - Isnull(H.Issue_Qty,0)) < ((Isnull(F.Stock,0) + Isnull(F1.Stock,0)) + Isnull(B.Issue_Qty,0)) Then (ISNULL(G.Indent_Qty,0) - Isnull(H.Issue_Qty,0)) Else ((Isnull(F.Stock,0) + Isnull(F1.Stock,0)) + Isnull(B.Issue_Qty,0)) End) Pending_Qty, ";
                    //Str = Str + " Isnull(B.Issue_Qty,0)Issue_Qty, (Case When Isnull(G.Indent_Qty,0) < ((Isnull(F.Stock,0) + Isnull(F1.Stock,0)) + Isnull(B.Issue_Qty,0)) Then Cast(Isnull(G.Indent_Qty,0) As Numeric(20,3)) Else Cast(((Isnull(F.Stock,0) + Isnull(F1.Stock,0)) + ISNULL(B.Issue_Qty,0)) As Numeric(20,3)) End) Issue_Qty1, ";
                    //Str = Str + " (Case When Isnull(G.Indent_Qty,0) < ((Isnull(F.Stock,0) + Isnull(F1.Stock,0)) + ISNULL(B.Issue_Qty,0)) Then Cast(Isnull(G.Indent_Qty,0) As Numeric(20,3)) Else Cast(((Isnull(F.Stock,0) + Isnull(F1.Stock,0)) + ISNULL(B.Issue_Qty,0)) As Numeric(20,3)) End) Balance_Qty, ";
                    //Str = Str + " B.ItemID, B.ColorID, B.SizeID, B.Slno1, B.Indent_Request_Details_RowID,  B.Remarks, '-' T, 'O' Record ";
                    //Str = Str + " From Socks_Covering_Yarn_Indent_Issue_Master A  Left Join Socks_Covering_Yarn_Indent_Issue_Details B On A.RowID = B.MasterID ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Covering_Req_Status_Orderwise()D On B.Order_No = D.Order_No And B.ItemID = D.Cover_Req_ItemID And B.ColorID = D.Cover_Req_Colorid And B.SizeID = D.Cover_Req_Sizeid ";
                    //Str = Str + " Left Join Item It On D.Cover_Req_ItemID = It.ItemID Left Join Color Co On D.Cover_Req_ColorID = Co.ColorID Left Join Size Si On D.Cover_Req_SizeID = Si.SizeID ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Covering_Yarn_Order_Issue_Details_Edit(" + Code + ")E On B.Order_No = E.Order_No And B.ItemID = E.Itemid And B.ColorID = E.Colorid And B.SizeID = E.Sizeid ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Lot_Stock()F On B.Order_No = F.Order_No And B.ItemID = F.ItemID And B.ColorID = F.ColorID And B.SizeID = F.SizeID ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Lot_Stock()F1 On F1.Order_No = 'GENERAL' And B.ItemID = F.ItemID And B.ColorID = F.ColorID And B.SizeID = F.SizeID ";
                    //Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details G On B.Indent_Request_Details_RowID = G.RowID And B.ItemID = G.ItemID And B.ColorID = G.ColorID And B.SizeID = G.SizeID ";
                    //Str = Str + " Left Join Socks_Covering_Indent_Request_Against_Without_Current_Issue(" + Code + ")H On B.Order_No = H.Order_No And B.ItemID = H.ItemID And B.ColorID = H.ColorID And B.SizeID = H.SizeID ";
                    //Str = Str + " Where A.RowID = " + Code;

                    
                    //Commented On 05-Nov-2016
                    //Str = " Select B.Slno, B.Order_No, It.Item, Co.Color, Si.Size, Isnull(D.Cover_Req, 0) Tot_Req, ISnull(E.Issue_Qty, 0) Issued, (Isnull(D.Cover_Req, 0) - ISnull(E.Issue_Qty, 0)) Bal_Req, ";
                    //Str = Str + " (ISNULL(F.Stock, 0) + ISNULL(B.Issue_Qty,0)) Stock, (Case When (Isnull(D.Cover_Req, 0) - Isnull(E.Issue_Qty,0)) < G.Indent_Qty Then (Isnull(D.Cover_Req, 0) - Isnull(E.Issue_Qty,0)) Else G.Indent_Qty End)Indent_Qty, Isnull(H.Issue_Qty,0)Indent_Issued, ";
                    //Str = Str + " (Case When (ISNULL(G.Indent_Qty,0) - Isnull(H.Issue_Qty,0)) < (Isnull(F.Stock,0) + Isnull(B.Issue_Qty,0)) Then (ISNULL(G.Indent_Qty,0) - Isnull(H.Issue_Qty,0)) Else (Isnull(F.Stock,0) + Isnull(B.Issue_Qty,0)) End) Pending_Qty, ";
                    //Str = Str + " Isnull(B.Issue_Qty,0)Issue_Qty, (Case When Isnull(G.Indent_Qty,0) < (Isnull(F.Stock,0) + Isnull(B.Issue_Qty,0)) Then Cast(Isnull(G.Indent_Qty,0) As Numeric(20,3)) Else Cast((Isnull(F.Stock,0) + ISNULL(B.Issue_Qty,0)) As Numeric(20,3)) End) Issue_Qty1, ";
                    //Str = Str + " (Case When Isnull(G.Indent_Qty,0) < (Isnull(F.Stock,0) + ISNULL(B.Issue_Qty,0)) Then Cast(Isnull(G.Indent_Qty,0) As Numeric(20,3)) Else Cast((Isnull(F.Stock,0) + ISNULL(B.Issue_Qty,0)) As Numeric(20,3)) End) Balance_Qty, "; 
                    //Str = Str + " B.ItemID, B.ColorID, B.SizeID, B.Slno1, B.Indent_Request_Details_RowID,  B.Remarks, '-' T, 'O' Record ";
                    //Str = Str + " From Socks_Covering_Yarn_Indent_Issue_Master A  Left Join Socks_Covering_Yarn_Indent_Issue_Details B On A.RowID = B.MasterID ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Covering_Req_Status_Orderwise()D On B.Order_No = D.Order_No And B.ItemID = D.Cover_Req_ItemID And B.ColorID = D.Cover_Req_Colorid And B.SizeID = D.Cover_Req_Sizeid ";
                    //Str = Str + " Left Join Item It On D.Cover_Req_ItemID = It.ItemID Left Join Color Co On D.Cover_Req_ColorID = Co.ColorID Left Join Size Si On D.Cover_Req_SizeID = Si.SizeID ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Covering_Yarn_Order_Issue_Details_Edit(" + Code + ")E On B.Order_No = E.Order_No And B.ItemID = E.Itemid And B.ColorID = E.Colorid And B.SizeID = E.Sizeid ";
                    //Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Lot_Stock()F On F.Order_No = 'GENERAL' And B.ItemID = F.ItemID And B.ColorID = F.ColorID And B.SizeID = F.SizeID ";
                    //Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details G On B.Indent_Request_Details_RowID = G.RowID And B.ItemID = G.ItemID And B.ColorID = G.ColorID And B.SizeID = G.SizeID ";
                    //Str = Str + " Left Join Socks_Covering_Indent_Request_Against_Without_Current_Issue(" + Code + ")H On B.Order_No = H.Order_No And B.ItemID = H.ItemID And B.ColorID = H.ColorID And B.SizeID = H.SizeID ";
                    //Str = Str + " Where A.RowID = " + Code;


                    Str = " Select B.Slno, B.Order_No, It.Item, Co.Color, Si.Size, 100000 Tot_Req, ISnull(E.Issue_Qty, 0) Issued, (100000 - ISnull(E.Issue_Qty, 0)) Bal_Req, ";
                    Str = Str + " (ISNULL(F.Stock, 0) + ISNULL(B.Issue_Qty,0)) Stock, (Case When (100000 - Isnull(E.Issue_Qty,0)) < G.Indent_Qty Then (100000 - Isnull(E.Issue_Qty,0)) Else G.Indent_Qty End)Indent_Qty, Isnull(H.Issue_Qty,0)Indent_Issued, ";
                    Str = Str + " (Case When (ISNULL(G.Indent_Qty,0) - Isnull(H.Issue_Qty,0)) < (Isnull(F.Stock,0) + Isnull(B.Issue_Qty,0)) Then (ISNULL(G.Indent_Qty,0) - Isnull(H.Issue_Qty,0)) Else (Isnull(F.Stock,0) + Isnull(B.Issue_Qty,0)) End) Pending_Qty, "; 
                    Str = Str + " Isnull(B.Issue_Qty,0)Issue_Qty, (Case When Isnull(G.Indent_Qty,0) < (Isnull(F.Stock,0) + Isnull(B.Issue_Qty,0)) Then Cast(Isnull(G.Indent_Qty,0) As Numeric(20,3)) Else Cast((Isnull(F.Stock,0) + ISNULL(B.Issue_Qty,0)) As Numeric(20,3)) End) Issue_Qty1, ";
                    Str = Str + " (Case When Isnull(G.Indent_Qty,0) < (Isnull(F.Stock,0) + ISNULL(B.Issue_Qty,0)) Then Cast(Isnull(G.Indent_Qty,0) As Numeric(20,3)) Else Cast((Isnull(F.Stock,0) + ISNULL(B.Issue_Qty,0)) As Numeric(20,3)) End) Balance_Qty, ";
                    Str = Str + " B.ItemID, B.ColorID, B.SizeID, B.Slno1, B.Indent_Request_Details_RowID,  B.Remarks, '-' T, 'O' Record ";
                    Str = Str + " From Socks_Covering_Yarn_Indent_Issue_Master A  Left Join Socks_Covering_Yarn_Indent_Issue_Details B On A.RowID = B.MasterID ";
                    Str = Str + " Left Join Item It On B.ItemID = It.ItemID Left Join Color Co On B.ColorID = Co.ColorID Left Join Size Si On B.SizeID = Si.SizeID ";
                    Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Covering_Yarn_Order_Issue_Details_Edit(" + Code + ")E On B.Order_No = E.Order_No And B.ItemID = E.Itemid And B.ColorID = E.Colorid And B.SizeID = E.Sizeid ";
                    Str = Str + " Left Join FITSOCKS.DBO.Socks_Store_Lot_Stock()F On F.Order_No = 'GENERAL' And B.ItemID = F.ItemID And B.ColorID = F.ColorID And B.SizeID = F.SizeID ";
                    Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Request_Details G On B.Indent_Request_Details_RowID = G.RowID And B.ItemID = G.ItemID And B.ColorID = G.ColorID And B.SizeID = G.SizeID ";
                    Str = Str + " Left Join Socks_Covering_Indent_Request_Against_Without_Current_Issue(" + Code + ")H On B.Order_No = H.Order_No And B.ItemID = H.ItemID And B.ColorID = H.ColorID And B.SizeID = H.SizeID ";
                    Str = Str + " Where A.RowID = " + Code;
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "ItemID", "ColorID", "SizeID", "Slno1", "Indent_Request_Details_RowID", "Issue_Qty1", "Balance_Qty", "T", "Record");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Issue_Qty", "Remarks");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 50, 110, 100, 130, 80, 90, 90, 100, 100, 100, 100, 100);
                Grid.Columns["Issue_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid.Columns["Remarks"].DefaultCellStyle.Format = "-";

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (!MyParent._New)
                    {
                        if (Convert.ToInt16(Grid["Issue_Qty", i].Value) > 0)
                        {
                            Vis = 1;
                            Pos = i;
                            GridDetail_Data(Convert.ToInt16(Grid["Slno1", i].Value), Grid["Order_No", i].Value.ToString(), Convert.ToInt64(Grid["ColorID", i].Value), Convert.ToInt64(Grid["ItemID", i].Value), Convert.ToInt64(Grid["SizeID", i].Value), Convert.ToInt64(Grid["Indent_Qty", i].Value));
                            Vis = 0;
                            Pos = 0;
                        }
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
                if (Grid.Rows.Count > 0)
                {
                    MyBase.Row_Number(ref Grid);
                }
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
                //if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                //{
                //    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                //    {
                //        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Issue_Qty"].Index)
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
                TxtTotal.Text = MyBase.Count(ref Grid, "Slno");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void GridDetail_Data(Int32 Row, String Order_NO, Int64 ColorID, Int64 ItemID, Int64 SizeID, Double Indent_Qty)
        {
            try
            {
                Row1 = Row;
                if (DtQty[Row] == null)
                {
                    DtQty[Row] = new DataTable();
                    if (MyParent._New)
                    {
                        Str = " Select 0 As Slno, LotNo, Cast(0.000 As Numeric(20,3)) Weight, Cast('' As Varchar(30)) Grn_No, Cast(0.000 As Numeric(20,3)) Issued, Cast(0.000 As Numeric(20,3)) Stock, Issue_Qty, 0 As Slno1, Order_No, '' T ";
                        Str = Str + " From Socks_Covering_Yarn_Indent_Issue_Master A Left Join Socks_Covering_Yarn_Indent_Issue_Details B On A.RowID = B.MasterID ";
                        Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Issue_Details C On A.RowID = C.MasterID And B.SlNo1 = C.Slno1 Where 1 = 2 ";
                    }
                    else
                    {
                        if ((Grid["Record", Grid.CurrentCell.RowIndex].Value.ToString()) == "O")
                        {
                            if (Vis == 1)
                            {
                                Str = "Select * From Socks_Covering_Yarn_Indent_Samplewise_Lot_Details_Edit(" + Code + ", " + Grid["Slno1", Pos].Value.ToString() + ")";
                            }
                            else
                            {
                                Str = "Select * From Socks_Covering_Yarn_Indent_Samplewise_Lot_Details_Edit(" + Code + ", " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() + ")";
                            }
                        }
                        else
                        {
                            Str = " Select 0 As Slno, LotNo, Cast(0.000 As Numeric(20,3)) Weight, Cast('' As Varchar(30)) Grn_No, Cast(0.000 As Numeric(20,3)) Issued, Cast(0.000 As Numeric(20,3)) Stock, Issue_Qty, 0 As Slno1, Order_No, '' T ";
                            Str = Str + " From Socks_Covering_Yarn_Indent_Issue_Master A Left Join Socks_Covering_Yarn_Indent_Issue_Details B On A.RowID = B.MasterID ";
                            Str = Str + " Left Join Socks_Covering_Yarn_Indent_SampleWise_Issue_Details C On A.RowID = C.MasterID And B.SlNo1 = C.Slno1 Where 1 = 2 ";
                        }
                    }
                    MyBase.Load_Data(Str, ref DtQty[Row]);
                }
                GridDetail.DataSource = DtQty[Row];
                MyBase.Grid_Designing(ref GridDetail, ref DtQty[Row], "SlNo1", "Order_No", "T");
                MyBase.ReadOnly_Grid_Without(ref GridDetail, "LotNo", "Issue_Qty");
                MyBase.Grid_Colouring(ref GridDetail, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridDetail, 50, 110, 100, 150, 100, 100, 100, 100);

                GridDetail.Columns["Weight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridDetail.Columns["Issued"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridDetail.Columns["Stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                GridDetail.RowHeadersWidth = 10;

                if (!MyParent._New && Vis == 1)
                {
                    TxtQty1.Text = Grid["Pending_Qty", Pos].Value.ToString();
                    Order_No = Grid["Order_No", Pos].Value.ToString();
                    TxtOrder.Text = Grid["Order_No", Pos].Value.ToString();
                    TxtItem.Text = Grid["Item", Pos].Value.ToString();
                    TxtColor.Text = Grid["Color", Pos].Value.ToString();
                    TxtSize.Text = Grid["Size", Pos].Value.ToString();
                    TxtIndent.Text = Grid["Pending_Qty", Pos].Value.ToString();
                    Iss_Balance();
                    Grid["Issue_Qty", Pos].Value = TxtEnteredWeight.Text;
                    Grid["Balance_Qty", Pos].Value = TxtBalance.Text;
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

       private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {

                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Issue_Qty"].Index)
                    {
                        TxtQty1.Text = Grid["Pending_Qty", Grid.CurrentCell.RowIndex].Value.ToString();
                        Order_No = Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString();
                        TxtOrder.Text = Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString();
                        TxtItem.Text = Grid["Item", Grid.CurrentCell.RowIndex].Value.ToString();
                        TxtColor.Text = Grid["Color", Grid.CurrentCell.RowIndex].Value.ToString();
                        TxtSize.Text = Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString();
                        TxtIndent.Text = Grid["Pending_Qty", Grid.CurrentCell.RowIndex].Value.ToString();

                        GridDetail_Data(Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value), Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Convert.ToInt64(Grid["ColorID", Grid.CurrentCell.RowIndex].Value), Convert.ToInt64(Grid["ItemID", Grid.CurrentCell.RowIndex].Value), Convert.ToInt32(Grid["SizeID", Grid.CurrentCell.RowIndex].Value), Convert.ToDouble(Grid["Indent_Qty", Grid.CurrentCell.RowIndex].Value));
                        GridDetail.CurrentCell = GridDetail["LotNo", 0];
                        GridDetail.Focus();
                        GridDetail.BeginEdit(true);
                        e.Handled = true;
                        Iss_Balance();
                        return;
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
                    {
                        //if ((Convert.ToDecimal(Grid["Issue_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) + Convert.ToDecimal(Grid["Balance_Qty", Grid.CurrentCell.RowIndex].Value.ToString())) != Convert.ToDecimal(Grid["Issue_Qty1", Grid.CurrentCell.RowIndex].Value.ToString()))
                        //if ((Math.Round(Convert.ToDecimal(Grid["Issue_Qty", Grid.CurrentCell.RowIndex].Value.ToString())) + Math.Round(Convert.ToDecimal(Grid["Balance_Qty", Grid.CurrentCell.RowIndex].Value.ToString()))) != Math.Round(Convert.ToDecimal(Grid["Indent_Qty", Grid.CurrentCell.RowIndex].Value.ToString())))
                        if ((Math.Round(Convert.ToDecimal(Grid["Issue_Qty", Grid.CurrentCell.RowIndex].Value.ToString())) + Math.Round(Convert.ToDecimal(Grid["Balance_Qty", Grid.CurrentCell.RowIndex].Value.ToString()))) > Math.Round(Convert.ToDecimal(Grid["Pending_Qty", Grid.CurrentCell.RowIndex].Value.ToString())))
                        {
                            MessageBox.Show("Invalid Breakup Qty In Row " + Convert.ToInt64(Grid.CurrentCell.RowIndex) + 1 + "");
                            Grid.CurrentCell = Grid["Issue_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            e.Handled = true;
                            return;
                        }
                        if ((Math.Round(Convert.ToDecimal(Grid["Issue_Qty", Grid.CurrentCell.RowIndex].Value.ToString())) + Math.Round(Convert.ToDecimal(Grid["Balance_Qty", Grid.CurrentCell.RowIndex].Value.ToString()))) > Math.Round(Convert.ToDecimal(Grid["Stock", Grid.CurrentCell.RowIndex].Value.ToString())))
                        {
                            MessageBox.Show("Invalid Breakup Qty In Row " + Convert.ToInt64(Grid.CurrentCell.RowIndex) + 1 + "");
                            Grid.CurrentCell = Grid["Issue_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            e.Handled = true;
                            return;
                        }
                        if (Math.Round(Convert.ToDecimal(Grid["Issue_Qty", Grid.CurrentCell.RowIndex].Value.ToString())) > Math.Round(Convert.ToDecimal(Grid["Issue_Qty1", Grid.CurrentCell.RowIndex].Value.ToString())))
                        {
                            MessageBox.Show("Invalid Issue Qty In Row " + Convert.ToInt64(Grid.CurrentCell.RowIndex) + 1 + "");
                            Grid.CurrentCell = Grid["Issue_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            e.Handled = true;
                            return;
                        }
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
                //MyBase.Grid_Delete(ref GridDetail, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], GridDetail.CurrentCell.RowIndex);
                //DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].AcceptChanges();
                //MyBase.Row_Number(ref GridDetail);
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

        private void ButOk_Click(object sender, EventArgs e)
        {
            try
            {
                Iss_Balance();

                if (TxtBalance.Text.Trim() == String.Empty || Convert.ToDouble(TxtBalance.Text.ToString()) < 0.000)
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    GridDetail.CurrentCell = GridDetail["Issue_Qty", 0];
                    GridDetail.Focus();
                    GridDetail.BeginEdit(true);
                    return;
                }
                Iss_Balance();
                Double Iss_Qty_Ok = 0, Iss_Bal_Ok = 0;
                Iss_Qty_Ok = Convert.ToDouble(TxtEnteredWeight.Text.ToString());
                Iss_Bal_Ok = Convert.ToDouble(TxtBalance.Text.ToString());
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Remarks", (Grid.CurrentCell.RowIndex)];
                Grid["Issue_Qty", (Grid.CurrentCell.RowIndex)].Value = Iss_Qty_Ok;
                Grid["Balance_Qty", (Grid.CurrentCell.RowIndex)].Value = Iss_Bal_Ok;
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
                if (TxtTotal.Text.ToString() != "")
                {
                    if (Convert.ToDouble(TxtTotal.Text.ToString()) < 0.000)
                    {
                        MessageBox.Show("Invalid KGS ..!", "Gainup");
                        Grid["T", (Grid.CurrentCell.RowIndex)].Value = "0.000";
                        Grid.CurrentCell = Grid["Order_No", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        GBQty.Visible = false;
                        MyParent.Save_Error = true;
                        return;
                    }
                    for (int i = 0; i <= GridDetail.Rows.Count - 1; i++)
                    {
                        if (GridDetail["LotNO", i].Value != null)
                        {
                            if (GridDetail["Issue_Qty", i].Value == DBNull.Value || Convert.ToDouble(GridDetail["Issue_Qty", i].Value.ToString()) > Convert.ToDouble(GridDetail["Stock", i].Value.ToString()) || GridDetail["Stock", i].Value == DBNull.Value)
                            {
                                MessageBox.Show("Invalid KGS ..!", "Gainup");
                                GridDetail.CurrentCell = GridDetail["Issue_Qty", i];
                                GridDetail.Focus();
                                GridDetail.BeginEdit(true);
                                return;
                            }
                        }
                    }
                    if (Convert.ToDouble(TxtBalance.Text.ToString()) < 0)
                    {
                        MessageBox.Show("Invalid Issue_Qty...!", "Gainup");
                        GridDetail.CurrentCell = GridDetail["Issue_Qty", 0];
                        GridDetail.Focus();
                        GridDetail.BeginEdit(true);
                        return;
                    }

                    for (int i = 0; i <= DtQty[Row1].Rows.Count - 1; i++)
                    {
                        if (Convert.ToDouble(GridDetail["Stock", i].Value.ToString()) <= 0)
                        {
                            GridDetail["Issue_Qty", i].Value = "0.000";
                            Iss_Balance();
                        }
                        else if (Convert.ToDouble(GridDetail["Issue_Qty", i].Value.ToString()) <= Convert.ToDouble(Grid["Indent_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) && Convert.ToDouble(GridDetail["Issue_Qty", i].Value.ToString()) <= Convert.ToDouble(GridDetail["Stock", i].Value.ToString()))
                        {

                        }
                        else
                        {
                            MessageBox.Show("Invalid Issue_Qty...!", "Gainup");
                            if (Convert.ToDouble(Grid["Indent_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) < Convert.ToDouble(GridDetail["Stock", i].Value.ToString()))
                            {
                                GridDetail["Issue_Qty", i].Value = Grid["Indent_Qty", i].Value;
                            }
                            else if (Convert.ToDouble(GridDetail["Stock", i].Value.ToString()) < Convert.ToDouble(GridDetail["Indent_Qty", i].Value.ToString()))
                            {
                                GridDetail["Issue_Qty", i].Value = GridDetail["Stock", i].Value;
                            }
                            else
                            {
                                GridDetail["Issue_Qty", i].Value = GridDetail["Indent_Qty", i].Value;
                            }
                            GridDetail.CurrentCell = GridDetail["Issue_Qty", i];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            return;
                        }
                    }
                    Iss_Balance();
                    Double Iss_Qty_Ok = 0, Iss_Bal_Ok = 0;
                    Iss_Qty_Ok = Convert.ToDouble(TxtEnteredWeight.Text.ToString());
                    Iss_Bal_Ok = Convert.ToDouble(TxtBalance.Text.ToString());
                    DtQty = new DataTable[30];
                    GBQty.Visible = false;
                    Grid.CurrentCell = Grid["Remarks", (Grid.CurrentCell.RowIndex)];
                    Grid["Issue_Qty", (Grid.CurrentCell.RowIndex)].Value = Iss_Qty_Ok;
                    Grid["Balance_Qty", (Grid.CurrentCell.RowIndex)].Value = Iss_Bal_Ok;
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

                TxtEnteredWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum_With_Three_Digits(ref GridDetail, "Issue_Qty")));

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

        private void GridDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Issue_Qty"].Index)
                    {
                        if (GridDetail["Issue_Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Issue_Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Issue_Qty...!", "Gainup");
                            GridDetail.CurrentCell = GridDetail["Issue_Qty", Grid.CurrentCell.RowIndex];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            if (Convert.ToDouble(GridDetail["Stock", GridDetail.CurrentCell.RowIndex].Value.ToString()) <= 0)
                            {
                                GridDetail["Issue_Qty", GridDetail.CurrentCell.RowIndex].Value = "0.000";
                            }
                            else if (Convert.ToDouble(GridDetail["Issue_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString()) <= Convert.ToDouble(TxtBalance.Text.ToString()) && Convert.ToDouble(GridDetail["Issue_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString()) <= Convert.ToDouble(GridDetail["Stock", GridDetail.CurrentCell.RowIndex].Value.ToString()))
                            {

                            }
                            else
                            {
                                MessageBox.Show("Invalid Issue_Qty...!", "Gainup");
                                if (Convert.ToDouble(TxtBalance.Text.ToString()) < Convert.ToDouble(GridDetail["Stock", GridDetail.CurrentCell.RowIndex].Value.ToString()))
                                {
                                    if (Convert.ToDouble(TxtBalance.Text.ToString()) <= 0)
                                    {
                                        GridDetail["Issue_Qty", GridDetail.CurrentCell.RowIndex].Value = "0";
                                    }
                                    else
                                    {
                                        GridDetail["Issue_Qty", GridDetail.CurrentCell.RowIndex].Value = "0";
                                    }
                                }
                                else if (Convert.ToDouble(GridDetail["Stock", GridDetail.CurrentCell.RowIndex].Value.ToString()) < Convert.ToDouble(TxtBalance.Text.ToString()))
                                {
                                    GridDetail["Issue_Qty", GridDetail.CurrentCell.RowIndex].Value = GridDetail["Stock", GridDetail.CurrentCell.RowIndex].Value;
                                }
                                else
                                {
                                    GridDetail["Issue_Qty", GridDetail.CurrentCell.RowIndex].Value = GridDetail["Stock", GridDetail.CurrentCell.RowIndex].Value;
                                }
                                GridDetail.CurrentCell = GridDetail["Issue_Qty", GridDetail.CurrentCell.RowIndex];
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
                    if (e.KeyCode == Keys.Down)
                    {
                        if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["LotNo"].Index)
                        {
                            Str = "Select LotNO, Grn_No, Stock, Order_No, Weight, Issued From Stock_for_Issue('GENERAL', " + Grid["ItemID", Grid.CurrentCell.RowIndex].Value.ToString() + ", " + Grid["ColorID", Grid.CurrentCell.RowIndex].Value.ToString() + ", " + Grid["SizeID", Grid.CurrentCell.RowIndex].Value.ToString() + ")";

                            Dr = Tool.Selection_Tool_Except_New("LotNo", this, 100, 100, ref DtQty[Convert.ToInt16(Grid["SLNO1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select LOT NO...!", Str, String.Empty, 150, 100, 100, 120);
                            if (Dr != null)
                            {
                                GridDetail["LotNo", GridDetail.CurrentCell.RowIndex].Value = Dr["LotNo"].ToString();
                                Txt1.Text = Dr["LotNo"].ToString();
                                GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value = Dr["Weight"].ToString();
                                GridDetail["Grn_No", GridDetail.CurrentCell.RowIndex].Value = Dr["Grn_No"].ToString();
                                GridDetail["Issued", GridDetail.CurrentCell.RowIndex].Value = Dr["Issued"].ToString();
                                GridDetail["Stock", GridDetail.CurrentCell.RowIndex].Value = Dr["Stock"].ToString();
                                GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                                GridDetail["Slno1", GridDetail.CurrentCell.RowIndex].Value = Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();
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

        void TxtIss_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Issue_Qty"].Index)
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
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Issue_Qty"].Index)
                {

                    Iss_Balance();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Yarn_Indent_Issue_Covering_KeyDown(object sender, KeyEventArgs e)
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
                            Grid.CurrentCell = Grid["Issue_Qty", 0];
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
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtUnit")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Unit..!", "Select Unit_Name, RowId Unit_Code From Unit_Master", String.Empty, 300, 50);

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

        private void Frm_Yarn_Indent_Issue_Covering_KeyPress(object sender, KeyPressEventArgs e)
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

    }
}
