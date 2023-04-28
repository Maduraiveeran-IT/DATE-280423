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
    public partial class Frm_Socks_Dyeing_Invoice : Form, Entry
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
        //private EventHandler Txt_LostFocus;
        public Frm_Socks_Dyeing_Invoice()
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
                Set_Min_Max_Date(true);                
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
        void Set_Min_Max_Date(Boolean Condition)
        {
            try
            {
                DataTable Tdt = new DataTable();
                if (Condition)
                {
                    MyBase.Load_Data("Select DateAdd (d, " + MyParent.User_Datelock + ", Cast(GetDate() as Date)) MinDate, Cast(GetDate() as Date) MaxDate ", ref Tdt);
                    //DtpDate1.MinDate = Convert.ToDateTime(Tdt.Rows[0][0]);
                    BillDate.MaxDate = Convert.ToDateTime(Tdt.Rows[0][1]);
                }
                else
                {
                    //DtpDate1.MinDate = Convert.ToDateTime("01-Apr-2014");
                    BillDate.MaxDate = Convert.ToDateTime("31-Mar-2030");
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
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Dyeing Entry - Edit", "Select B.Eno, Cast(B.Edate as Date)Date,F.Supplier, B.Bill_No, Cast(B.BillDate as Date)Bill_Date,C.Item Yarn, D.Color, E.Size Count, A.Inv_Qty, B.Supplierid, A.Itemid, A.Colorid, A.Sizeid, B.Rowid   from fitsocks.dbo.Socks_Dyeing_Invoice_Details A Left Join fitsocks.dbo.Socks_Dyeing_Invoice_Master B on A.Master_ID = B.Rowid Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid Left Join fitsocks.dbo.supplier F on B.Supplierid = F.Supplierid", String.Empty, 80, 100, 150, 100, 100, 150, 200, 100, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Grn_No", 0];
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


        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Dyeing Entry - View", "Select B.Eno, Cast(B.Edate as Date)Date,F.Supplier, B.Bill_No, Cast(B.BillDate as Date)Bill_Date,C.Item Yarn, D.Color, E.Size Count, A.Inv_Qty, B.Supplierid, A.Itemid, A.Colorid, A.Sizeid, B.Rowid   from fitsocks.dbo.Socks_Dyeing_Invoice_Details A Left Join fitsocks.dbo.Socks_Dyeing_Invoice_Master B on A.Master_ID = B.Rowid Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid Left Join fitsocks.dbo.supplier F on B.Supplierid = F.Supplierid", String.Empty, 80, 100, 150, 100, 100, 150, 200, 100, 100);
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
                                Sum = Sum + Convert.ToDecimal(DtQty[i].Rows[j]["Inv_Qty"]);
                            }
                            if (Convert.ToDecimal(Grid["Inv_Qty", i - 1].Value) != Sum)
                            {
                                MessageBox.Show("Invalid Details..!", "Gainup");
                                Grid.CurrentCell = Grid["Inv_Qty", i - 1];
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

                    if (Grid["Inv_Qty", i].Value == DBNull.Value || Grid["Inv_Qty", i].Value.ToString() == String.Empty || Convert.ToInt64(Grid["Inv_Qty", i].Value) == 0)
                    {
                        MessageBox.Show(" ZERO is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Inv_Qty", i];
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
                        Grid.CurrentCell = Grid["Inv_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                }

                TxtEntryNo.Text = MyBase.MaxOnlyComp("Socks_Dyeing_Invoice_Master", "ENo", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();
                Queries = new string[Dt.Rows.Count * 100];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_Dyeing_Invoice_Master (ENo, EDate, Remarks, Bill_No, BillDate, Company_Code, Year_Code,User_Code,Supplierid) values (" + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + TxtRemarks.Text + "','" + TxtSupplierDc.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", BillDate.Value) + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "'," + MyParent.UserCode + ", " + TxtSupplier.Tag.ToString() + "); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("Yarn Dyeing Invoice Entry", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_Dyeing_Invoice_Master Set EDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', Bill_No='" + TxtSupplierDc.Text + "',BillDate = '" + String.Format("{0:dd-MMM-yyyy}", BillDate.Value) + "', Remarks = '" + TxtRemarks.Text + "',Company_Code=" + MyParent.CompCode + " , Year_Code='" + MyParent.YearCode + "',User_Code=" + MyParent.UserCode + " Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Yarn Dyeing Invoice Entry", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from Socks_Dyeing_Invoice_Details where Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete from Socks_Dyeing_OrderwiseInvoice_details where Master_ID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Socks_Dyeing_Invoice_Details (Master_ID, Slno, Grn_No, ItemID, SizeID, ColorID, Po_Qty, Inv_Qty, Inv_Rate, Rate, Others, Amount, Slno1) Values (@@IDENTITY, " + Grid["Slno", i].Value + ", " + Grid["Grn_No", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ",  " + Grid["Po_qty", i].Value + ", " + Grid["Inv_qty", i].Value + ", " + Grid["Inv_Rate", i].Value + ", " + Grid["Inv_Rate", i].Value + ", " + Grid["Others", i].Value + ", " + Grid["Amount", i].Value + ",  " + Grid["Slno", i].Value + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Socks_Dyeing_Invoice_Details (Master_ID, Slno, Grn_No, ItemID, SizeID, ColorID, Po_Qty, Inv_Qty, Inv_Rate, Rate, Others, Amount, Slno1) Values (" + Code + ", " + Grid["Slno", i].Value + ", " + Grid["Grn_No", i].Value + "," + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ", " + Grid["Po_qty", i].Value + ", " + Grid["Inv_qty", i].Value + ", " + Grid["Inv_Rate", i].Value + ", " + Grid["Inv_Rate", i].Value + ", " + Grid["Others", i].Value + ", " + Grid["Amount", i].Value + ", " + Grid["Slno", i].Value + ")";
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
                                    Queries[Array_Index++] = "Insert Into Socks_Dyeing_OrderwiseInvoice_details (slno, Master_ID, Order_No, Inv_Qty, App_Rate,  SlNo1) Values ( " + DtQty[i].Rows[j]["Sno"].ToString() + ",@@IDENTITY, '" + DtQty[i].Rows[j]["Order_No"].ToString() + "'," + DtQty[i].Rows[j]["Inv_Qty"].ToString() + "," + DtQty[i].Rows[j]["App_Rate"].ToString() + "," + DtQty[i].Rows[j]["Slno1"].ToString() + ")";
                                }
                                else
                                {
                                    Queries[Array_Index++] = "Insert Into Socks_Dyeing_OrderwiseInvoice_details (slno, Master_ID, Order_No,  Inv_Qty,  App_Rate, SlNo1) Values ( " + DtQty[i].Rows[j]["Sno"].ToString() + "," + Code + ", '" + DtQty[i].Rows[j]["Order_No"].ToString() + "', " + DtQty[i].Rows[j]["Inv_Qty"].ToString() + ", " + DtQty[i].Rows[j]["App_Rate"].ToString() + "," + DtQty[i].Rows[j]["Slno1"].ToString() + ")";
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Dyeing Entry - Delete", "Select B.Eno, Cast(B.Edate as Date)Date,F.Supplier, B.Bill_No, Cast(B.BillDate as Date)Bill_Date,C.Item Yarn, D.Color, E.Size Count, A.Inv_Qty, B.Supplierid, A.Itemid, A.Colorid, A.Sizeid, B.Rowid   from fitsocks.dbo.Socks_Dyeing_Invoice_Details A Left Join fitsocks.dbo.Socks_Dyeing_Invoice_Master B on A.Master_ID = B.Rowid Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid Left Join fitsocks.dbo.supplier F on B.Supplierid = F.Supplierid", String.Empty, 80, 100, 150, 100, 100, 150, 200, 100, 100);
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
                    MyBase.Run("Delete from Socks_Dyeing_OrderwiseInvoice_details where Master_ID = " + Code, "Delete from Socks_Dyeing_Invoice_Details where Master_ID = " + Code, "Delete From Socks_Dyeing_Invoice_Master Where RowID = " + Code, MyParent.EntryLog("Yarn Dyeing Invoice Entry", "DELETE", Code.ToString()));
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
                TxtSupplierDc.Text = Dr["Bill_No"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["Date"]);
                BillDate.Value = Convert.ToDateTime(Dr["Bill_Date"]);                
                TxtSupplier.Tag = Dr["Supplierid"].ToString();
                TxtSupplier.Text = Dr["Supplier"].ToString();                
                Grid_Data();
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Entry_Print()
        {
            try
            {
               // MyParent.View_Browser("MIS_SOCKS_YARNDYEING_INVOICE", Code);
                Str = "Select C1.companyid, C1.company, C1.address1 Comp_Address1, C1.Address2 Comp_Address2, C1.City Comp_City, C1.TinNo Comp_Tin, C1.cst_no Comp_Cst_No, C1.Cst_Date Comp_Cst_Date,";
                Str = Str + " S1.Type, S1.RowID Supplier_ROdid, S1.ENo, S1.Date, S1.Supplierid, S1.Supplier, S1.Dc_No, S1.Dc_Date, S1.address1 Supplier_Address1, S1.Address2 Supplier_Address2, S1.address3 Supplier_Address3, S1.City Supplier_City,";
                Str = Str + " D1.Rowid, D1.itemid, D1.item, D1.Colorid, D1.Color, D1.Sizeid, D1.SIze, D1.Rec_Qty, D1.Inv_Qty, D1.Rate, D1.Others, D1.Amount";
                Str = Str + " from [FITSOCKS].dbo.Supplier_Details_Yarn_Dyeing() S1 Left Join [FITSOCKS].Dbo.Dyeing_Invoice_For_Dc() D1 On S1.Rowid = D1.Rowid ";
                Str = Str + " Left Join [FITSOCKS].dbo.Company_Details() C1 On 1 =1 Where S1.Rowid = " + Code + " And S1.Type = 'Invoice' ";

                MyBase.Execute_Qry(Str, "Yarn_Dyeing_Invoice");
                CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Yarn_Dyeing_Invoice.rpt");
                MyParent.CReport(ref ObjRpt, "Yarn Dyeing Invoice..!");
                
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
                    Str = "select 0 as Slno, '' Grn_No, '' Dc_No, Item Yarn,  Color, Size Count, Cast(0.000 As Numeric(10,3)) Po_Qty, Cast(0.000 As Numeric(10,3)) Rec_Qty, Cast(0.000 As Numeric(10,3)) Inv_Qty, Cast(0.000 As Numeric(20,4)) Rate, Cast(0.000 As Numeric(20,4)) Inv_Rate, Cast(0.000 As Numeric(20,4)) Others, Cast(0.000 As Numeric(20,4)) Amount,  Itemid, Colorid, Sizeid,0 Slno1, 0 RNo,'-' T  from FITSOCKS.dbo.Yarn_Dyeing_Requirement_Details() where 1=2 Group By Itemid, Item, Colorid, Color, Sizeid, Size";
                }
                else
                {
                    Str = "Select A.Slno, A.Grn_No, F.Dc_No, C.Item Yarn, D.Color, E.Size Count, A.Po_Qty, A.Inv_Qty Rec_Qty, A.Inv_Qty, isnull(A.Rate,0.0000)Rate, isnull(A.Inv_Rate,0.0000)Inv_Rate, isnull(A.Others,0.0000)Others, isnull(A.Amount,0.0000)Amount,  A.Itemid, A.Colorid, A.Sizeid, A.Slno1, ROW_NUMBER() Over (Order by B.ENo, A.Itemid, A.Colorid, A.Sizeid) RNo,'-' T  from fitsocks.dbo.Socks_Dyeing_Invoice_Details A Left Join fitsocks.dbo.Socks_Dyeing_Invoice_Master B on A.Master_ID = B.Rowid Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid Left Join fitsocks.dbo.Socks_Dyeing_Receipt_Master F on A.Grn_No = F.ENo Where B.Eno = '" + TxtEntryNo.Text + "' Order By A.Slno ";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Designing(ref Grid, ref Dt, "ItemID", "SizeID", "ColorID", "Rate", "Slno1", "RNo", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Grn_No", "Po_Qty", "Inv_Rate", "Inv_Qty", "Others");
                MyBase.Grid_Width(ref Grid, 40, 90, 90, 150, 90, 90, 100, 100, 100, 100, 100, 100);
                Grid.Columns["Rec_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                Grid.Columns["Inv_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Inv_Rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Po_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Others"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
                    Txt.LostFocus += new EventHandler(Txt_LostFocus);
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Grn_No"].Index)
                    {
                        if (TxtSupplier.Text != String.Empty)
                        {
                            Dr = Tool.Selection_Tool_Except_New("RNo", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Item", "Select B.ENo Grn_No, B.Dc_No, C.Item, D.Color, E.Size, 0.000 Po_Qty,(Sum(isnull(A.Rec_Qty,0))-isnull(F.Inv_Qty,0)) Rec_Qty, (sum(isnull(A.Rec_Qty,0))-isnull(F.Inv_Qty,0)) Inv_Qty, 0.0000 Rate, 0.0000 Inv_Rate, 0.0000 Others, 0.0000 Amount, A.Itemid, A.Colorid, A.Sizeid, ROW_NUMBER() Over (Order by B.ENo, A.Itemid, A.Colorid, A.Sizeid) RNo  from Socks_Dyeing_Receipt_Details A Left Join Socks_Dyeing_Receipt_Master B on  A.Master_ID = B.RowID Left Join Item C on A.Itemid = C.Itemid Left Join Color D on A.Colorid = D.colorid Left Join Size E on A.Sizeid = E.Sizeid Left Join Invoice_Details_Grnwise() F on A.ItemID = F.Itemid and A.ColorID = F.Colorid and A.Sizeid = F.Sizeid and B.ENo = F.Grn_No  Where B.Supplierid= " + TxtSupplier.Tag.ToString() + " Group By B.ENo, B.Dc_No, C.Item, D.Color, E.Size, F.Inv_Qty, A.Itemid, A.Colorid, A.Sizeid Having (sum(isnull(A.Rec_Qty,0))-isnull(F.Inv_Qty,0)) >0 ", String.Empty, 100, 100, 200, 150, 100, 100, 100);

                            if (Dr != null)
                            {
                                Txt.Text = Dr["Grn_No"].ToString();
                                Grid["Grn_No", Grid.CurrentCell.RowIndex].Value = Dr["Grn_No"].ToString();
                                Grid["Dc_No", Grid.CurrentCell.RowIndex].Value = Dr["Dc_No"].ToString();
                                Grid["Yarn", Grid.CurrentCell.RowIndex].Value = Dr["ITEM"].ToString();
                                Grid["Count", Grid.CurrentCell.RowIndex].Value = Dr["SIZE"].ToString();
                                Grid["COLOR", Grid.CurrentCell.RowIndex].Value = Dr["COLOR"].ToString();
                                Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Po_Qty"].ToString();
                                Grid["Rec_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Rec_Qty"].ToString();
                                Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Rec_Qty"].ToString();
                                Grid["Rate", Grid.CurrentCell.RowIndex].Value = Dr["Rate"].ToString();
                                Grid["Inv_Rate", Grid.CurrentCell.RowIndex].Value = Dr["Inv_Rate"].ToString();
                                Grid["Others", Grid.CurrentCell.RowIndex].Value = Dr["Others"].ToString();
                                Grid["Amount", Grid.CurrentCell.RowIndex].Value = Dr["Amount"].ToString();                                
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Inv_Qty"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Inv_Rate"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Po_Qty"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Others"].Index)
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Inv_Qty"].Index)
                {
                    if ((Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) != String.Empty)
                    {
                        if (Convert.ToDouble(Grid["Rec_Qty", Grid.CurrentCell.RowIndex].Value) < Convert.ToDouble(Txt.Text))
                        {
                            MessageBox.Show("Invalid Invoice Qty..!", "Gainup");
                            Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                            Txt.Text = "0.000";
                            Grid.CurrentCell = Grid["Inv_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                }
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Inv_Rate"].Index)
                {
                    if ((Grid["Inv_Rate", Grid.CurrentCell.RowIndex].Value.ToString()) != String.Empty)
                    {
                        if (Convert.ToDouble(Grid["Rate", Grid.CurrentCell.RowIndex].Value) < Convert.ToDouble(Txt.Text))
                        {
                            MessageBox.Show("Invoice Rate should be less than or equal to Approval rate..!", "Gainup");
                            Grid["Inv_Rate", Grid.CurrentCell.RowIndex].Value = "0.000";
                            Txt.Text = "0.000";
                            Grid.CurrentCell = Grid["Inv_Rate", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                }
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Po_Qty"].Index)
                {
                    if (Convert.ToDouble(Txt.Text) != 0.000)
                    {
                        if (Convert.ToDouble(Txt.Text) > Convert.ToDouble((Convert.ToDouble(Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value) + (Convert.ToDouble(Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value) * 0.1))))
                        {
                            MessageBox.Show("Invalid Po_Qty..!", "Gainup");
                            Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                            Txt.Text = "0.000";
                            Grid.CurrentCell = Grid["Po_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                }
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Inv_Rate"].Index)
                {
                    if (Convert.ToDouble(Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value) > 0.000)
                    {
                        Grid["Amount", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(Txt.Text)) + Convert.ToDouble(Grid["Others", Grid.CurrentCell.RowIndex].Value.ToString());
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

                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Inv_Qty"].Index)
                {
                    if ((GridDetail["Inv_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString()) != String.Empty)
                    {
                        if (Convert.ToDouble(GridDetail["Rec_Qty", GridDetail.CurrentCell.RowIndex].Value) < Convert.ToDouble(GridDetail["Inv_Qty", GridDetail.CurrentCell.RowIndex].Value))
                        {
                            MessageBox.Show("Invalid Inv_Qty..!", "Gainup");
                            GridDetail["Inv_Qty", GridDetail.CurrentCell.RowIndex].Value = "0.000";
                            GridDetail.CurrentCell = GridDetail["Inv_Qty", GridDetail.CurrentCell.RowIndex];
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
        void Txt_LostFocus(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Inv_Rate"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Inv_Qty"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Others"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Po_Qty"].Index)
                {
                    if ((Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) != String.Empty && (Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString()) != String.Empty && Convert.ToDouble(Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString()) != 0.0000 && (Grid["Inv_Rate", Grid.CurrentCell.RowIndex].Value.ToString()) != String.Empty && Convert.ToDouble(Grid["Inv_Rate", Grid.CurrentCell.RowIndex].Value.ToString()) != 0.0000)
                    {

                        //if (Convert.ToDouble(Grid["Rec_Qty", Grid.CurrentCell.RowIndex].Value) < Convert.ToDouble(Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value))
                        //{
                        //    MessageBox.Show("Invalid Invoice Qty..!", "Gainup");
                        //    Grid.CurrentCell = Grid["Inv_Qty", Grid.CurrentCell.RowIndex];
                        //    Grid.Focus();
                        //    Grid.BeginEdit(true);
                        //    MyParent.Save_Error = true;
                        //    return;
                        //}
                        //Grid["Amount", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(Grid["Inv_Rate", Grid.CurrentCell.RowIndex].Value.ToString())) + Convert.ToDouble(Grid["Others", Grid.CurrentCell.RowIndex].Value.ToString());
                        Grid["Amount", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(Grid["Inv_Rate", Grid.CurrentCell.RowIndex].Value.ToString())) + Convert.ToDouble(Grid["Others", Grid.CurrentCell.RowIndex].Value.ToString());

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
                TxtTotal.Text = MyBase.Sum(ref Grid, "Inv_Qty");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void GridDetail_Data(Int32 Row, Int32 Delivery_No, Int32 Rec_Qty, Int64 Item, Int64 Color, Int64 Size)
        {

            try
            {
                if (DtQty[Row] == null)
                {
                    DtQty[Row] = new DataTable();
                    if (MyParent._New)
                    {
                        MyBase.Load_Data("select 0 SNo, '0' Order_No, 0.000 Rec_Qty, 0.000 Inv_Qty, 0.000 App_Rate, " + Row + " SlNo1, '' T from Yarn_Dyeing_Requirement_Details() where 1=2 ", ref DtQty[Row]);
                    }
                    else
                    {
                        MyBase.Load_Data("select A.slno Sno, A.Order_No, A.Inv_Qty Rec_Qty, A.Inv_Qty, A.App_Rate, B.Slno1,'' T from Socks_Dyeing_OrderwiseInvoice_details A Left Join Socks_Dyeing_Invoice_Details B on A.Master_ID = B.Master_ID and A.SlNo1 = B.Slno1 Left Join Socks_Dyeing_Invoice_Master C on A.Master_ID = C.RowID and B.Master_ID = C.RowID  Where  A.Master_ID =  " + Code + " and  B.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString(), ref DtQty[Row]);
                    }
                }
                GridDetail.DataSource = DtQty[Row];
                MyBase.Grid_Designing(ref GridDetail, ref DtQty[Row], "SlNo1", "T");
                MyBase.ReadOnly_Grid_Without(ref GridDetail, "Inv_Qty", "Order_No");
                MyBase.Grid_Colouring(ref GridDetail, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridDetail, 50, 150, 100, 100, 100);
                GridDetail.Columns["Rec_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridDetail.Columns["Inv_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridDetail.Columns["App_Rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Inv_Qty"].Index)
                    {
                        if (GridDetail["Inv_Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Inv_Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Inv_Qty", GridDetail.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Inv_Qty...!", "Gainup");
                            GridDetail.CurrentCell = GridDetail["Inv_Qty", Grid.CurrentCell.RowIndex];
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
                        Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select A.Order_No, (Isnull(A.Rec_Qty,0)-Isnull(B.Inv_Qty,0))Rec_Qty,  (Isnull(A.Rec_Qty,0)-Isnull(B.Inv_Qty,0))Inv_Qty , C.App_Rate, A.Itemid, A.Colorid, A.Sizeid   from Orderwise_Dyeing_Receipt()A  Left Join Orderwise_Dyeing_Invoiced()B on A.Grn_No = B.Grn_No and A.Order_no = B.Order_no and A.Itemid = B.Itemid and A.Colorid = B.Colorid and A.Sizeid = B.Sizeid Left Join Approved_Details()C on A.order_no = C.Order_No And A.Itemid = C.Itemid And A.COlorid = C.COlorid And A.Sizeid = C.Sizeid Where A.Grn_No = " + Delivery_No + " and A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + "  and (Isnull(A.Rec_Qty,0)-Isnull(B.Inv_Qty,0))>0 Order By A.Order_No", String.Empty, 150, 100, 100, 100);

                        if (Dr != null)
                        {
                            Txt1.Text = Dr["Order_No"].ToString();
                            GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                            GridDetail["Inv_Qty", GridDetail.CurrentCell.RowIndex].Value = Dr["Rec_Qty"].ToString();
                            GridDetail["Rec_Qty", GridDetail.CurrentCell.RowIndex].Value = Dr["Rec_Qty"].ToString();
                            GridDetail["App_Rate", GridDetail.CurrentCell.RowIndex].Value = Dr["App_Rate"].ToString();
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
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Inv_Qty"].Index)
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
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Inv_Qty"].Index)
                {
                    if (GridDetail["Inv_Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Inv_Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Inv_Qty", GridDetail.CurrentCell.RowIndex].Value) == 0)
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

                TxtEnteredWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum_With_Three_Digits(ref GridDetail, "Inv_Qty")));
                TxtAvgRate.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum_With_Three_Digits(ref GridDetail, "App_Rate"))/(GridDetail.Rows.Count-1));
                
                if (TxtAvgRate.Text.Trim() == String.Empty)
                {
                    TxtAvgRate.Text = "0.000";
                }

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
                    GridDetail.CurrentCell = GridDetail["Inv_Qty", 0];
                    GridDetail.Focus();
                    GridDetail.BeginEdit(true);
                    return;
                }
                Grid["Rate", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(TxtAvgRate.Text);
                Grid["Inv_Rate", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(TxtAvgRate.Text);
                Grid["Amount", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(Grid["Inv_Rate", Grid.CurrentCell.RowIndex].Value.ToString())) + Convert.ToDouble(Grid["Others", Grid.CurrentCell.RowIndex].Value.ToString());
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Yarn", (Grid.CurrentCell.RowIndex + 1)];
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
                    if (GridDetail["Inv_Qty", i].Value == DBNull.Value || Convert.ToDouble(GridDetail["Inv_Qty", i].Value) != 0)
                    {
                        MessageBox.Show("Invalid KGS ..!", "Gainup");
                        Grid.CurrentCell = Grid["Inv_Qty", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        GBQty.Visible = false;
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                Grid["Rate", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(TxtAvgRate.Text);
                Grid["Amount", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString())) + Convert.ToDouble(Grid["Others", Grid.CurrentCell.RowIndex].Value.ToString());
                DtQty = new DataTable[30];
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Inv_Qty", (Grid.CurrentCell.RowIndex)];
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
                        BillDate.Focus();                        
                        return;
                    }
                    else if (this.ActiveControl.Name == "BillDate")
                    {
                        Grid.CurrentCell = Grid["Grn_No", 0];
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

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier..!", "Select Distinct A.Supplier, A.Supplierid From fitsocks.dbo.Supplier A Inner Join Socks_Dyeing_Receipt_Master B on A.supplierid = B.SupplierId Order By A.Supplier", String.Empty, 300);

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

                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Inv_Qty"].Index)
                    {
                        if (Convert.ToDouble(Grid["Rec_Qty", Grid.CurrentCell.RowIndex].Value) < Convert.ToDouble(Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value))
                        {
                            MessageBox.Show("Invalid Inv_Qty..!", "Gainup");
                            Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                            Grid.CurrentCell = Grid["Inv_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                        else if (Convert.ToDouble(Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value)==0.000)
                        {
                            MessageBox.Show("Invalid Inv_Qty..!", "Gainup");
                            Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                            Grid.CurrentCell = Grid["Inv_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }

                        TxtQty1.Text = Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value.ToString();
                                                
                        ItemID = Convert.ToInt64(Grid["ItemId", Grid.CurrentCell.RowIndex].Value);
                        ColorID = Convert.ToInt64(Grid["ColorId", Grid.CurrentCell.RowIndex].Value);
                        SizeID = Convert.ToInt64(Grid["SizeId", Grid.CurrentCell.RowIndex].Value);
                        Delivery_No = Convert.ToInt32(Grid["Grn_No", Grid.CurrentCell.RowIndex].Value);

                        GridDetail_Data(Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value), Convert.ToInt32(Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value), Delivery_No, ItemID, ColorID, SizeID);
                        GridDetail.CurrentCell = GridDetail["Order_No", 0];
                        GridDetail.Focus();
                        GridDetail.BeginEdit(true);
                        e.Handled = true;
                        return;

                    }
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Po_Qty"].Index)
                    {
                        if (Convert.ToDouble(Txt.Text)== 0.000)
                        {
                            MessageBox.Show("Invalid Po_Qty..!", "Gainup");
                            Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                            Grid.CurrentCell = Grid["Po_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                        else if (Convert.ToDouble(Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value) != 0.000)
                        {
                            if (Convert.ToDouble(Txt.Text) > Convert.ToDouble((Convert.ToDouble(Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value) + (Convert.ToDouble(Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value) * 0.1)))) 
                            {
                                MessageBox.Show("Invalid Po_Qty..!", "Gainup");
                                Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                                Grid.CurrentCell = Grid["Po_Qty", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }
                    }
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Inv_Rate"].Index)
                    {
                        if (Convert.ToDouble(Grid["Inv_Qty", Grid.CurrentCell.RowIndex].Value) > 0.000)
                        {
                            Grid["Amount", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["Po_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(Txt.Text)) + Convert.ToDouble(Grid["Others", Grid.CurrentCell.RowIndex].Value.ToString());
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

    }
}