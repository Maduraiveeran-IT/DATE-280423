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
    public partial class Frm_Yarn_Indent_Request_Link_Stiching : Form, Entry
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

        Int64 Unit = 0;

        public Frm_Yarn_Indent_Request_Link_Stiching()
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
                Grid_Data();
                DtQty = new DataTable[30];
                //29 - Vanagamudi Link Unit -1 
                //43 - Thangapandi Stich Unit - 1
                //41 - Senthi Link Link Unit - 2

                //DeptCode3 - Linking  73 - Stitching
                if (MyParent.UserCode == 29 || MyParent.UserCode == 43)
                {
                    TxtUnit.Text = "FLOOR - I";
                    TxtUnit.Tag = 1;
                    Unit = 71;
                    if (MyParent.UserCode == 29)
                    {
                        TxtDept.Text = "LINKING";
                        TxtDept.Tag = 43;
                    }
                    else if (MyParent.UserCode == 43)
                    {
                        TxtDept.Text = "STITCHING";
                        TxtDept.Tag = 73;
                    }
                    DcDate.Focus();
                }
                else if (MyParent.UserCode == 41)
                {
                    TxtUnit.Text = "FLOOR - II";
                    TxtUnit.Tag = 2;
                    Unit = 72;
                    TxtDept.Text = "LINKING";
                    TxtDept.Tag = 43;
                    DcDate.Focus();
                }
                else
                {
                    TxtUnit.Text = "";
                    TxtUnit.Tag = 0;
                    Unit = 0;
                    TxtDept.Text = "";
                    TxtDept.Tag = 0;
                    TxtUnit.Focus();
                }
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

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Indent_Qty", i].Value == DBNull.Value || Grid["Indent_Qty", i].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Indent_Qty", i].Value) == 0)
                    {
                        MessageBox.Show(" ZERO is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Iss_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }

                if (MyParent._New)
                {
                    TxtEntryNo.Text = MyBase.MaxOnlyWithoutComp("Socks_Yarn_Indent_Requset_Master", "EntryNO", String.Empty, String.Empty, 0).ToString();
                }
                Queries = new string[Dt.Rows.Count * 100];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_Yarn_Indent_Requset_Master(EntryNO, IndentNO, EntryDate, ProductionDate, UnitCode, Remarks, SystemName, EntryTime, UserCode, Compcode, DeptCode) values (" + TxtEntryNo.Text + ", " + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", DcDate.Value) + "', " + TxtUnit.Tag + ", '" + TxtRemarks.Text + "', Host_Name(), GetDate(), " + MyParent.UserCode + ", " + MyParent.CompCode + ", " + TxtDept.Tag + "); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_Yarn_Indent_Requset_Master", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_Yarn_Indent_Requset_Master Set EntryDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', ProductionDate = '" + String.Format("{0:dd-MMM-yyyy}", DcDate.Value) + "', UserCode = " + MyParent.UserCode + ", DeptCode = " + TxtDept.Tag + ", Remarks = '" + TxtRemarks.Text.ToString() + "' Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_Yarn_Indent_Requset_Master", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from Socks_Yarn_Indent_Requset_Details where MasterID = " + Code;
                    Queries[Array_Index++] = "Delete from Socks_Yarn_Indent_SampleWise_Requset_Details where MasterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Grid["Indent_Qty", i].Value.ToString()) > 0.000)
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into Socks_Yarn_Indent_Requset_Details (MasterID, Slno, Order_NO, Slno1, Remarks) Values (@@IDENTITY, " + Grid["Slno", i].Value + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["Slno", i].Value + ", '" + Grid["Remarks", i].Value + "')";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert into Socks_Yarn_Indent_Requset_Details (MasterID, Slno, Order_NO, Slno1, Remarks) Values (" + Code + ", " + Grid["Slno", i].Value + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["Slno", i].Value + ", '" + Grid["Remarks", i].Value + "')";
                        }
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        if (Math.Round(Convert.ToDouble(Grid["Indent_Qty", Grid.CurrentCell.RowIndex].Value.ToString()),3) > 0)
                        {
                            Queries[Array_Index++] = "Insert Into Socks_Yarn_Indent_SampleWise_Requset_Details (SlNo, MasterID, ItemID, ColorID, SizeID, Indent_Qty, SlNo1, Issue_Closed) Values (" + Grid["Slno", i].Value + ", @@IDENTITY, " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + ", " + Grid["SizeID", i].Value + ", " + Grid["Indent_Qty", i].Value.ToString() + ", " + Grid["Slno", i].Value.ToString() + ", 'P')";
                        }
                    }
                    else
                    {
                        if (Math.Round(Convert.ToDouble(Grid["Indent_Qty", Grid.CurrentCell.RowIndex].Value.ToString()), 3) > 0)
                        {
                            Queries[Array_Index++] = "Insert Into Socks_Yarn_Indent_SampleWise_Requset_Details (SlNo, MasterID, ItemID, ColorID, SizeID, Indent_Qty, SlNo1, Issue_Closed) Values (" + Grid["Slno", i].Value + ", " + Code + ", " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + ", " + Grid["SizeID", i].Value + ", " + Grid["Indent_Qty", i].Value.ToString() + ", " + Grid["Slno", i].Value.ToString() + ", 'P')";
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Indent Request Entry - Delete", "Select S1.EntryNo, S1.EntryDate, D1.DeptName, S2.Order_No, U1.Unit_Name, S1.ProductionDate, S1.Remarks, S1.UnitCode, S1.RowID, S1.DeptCode From Socks_Yarn_Indent_Requset_Master S1 Inner Join Socks_Yarn_Indent_Requset_Details S2 On S1.RowID = S2.MasterID Left Join VAAHINI_ERP_GAINUP.Dbo.Unit_Master U1 On S1.UnitCode = U1.RowId Left Join VAAHINI_ERP_GAINUP.Dbo.DeptType D1 On S1.DeptCode = D1.DeptCode Where S1.DeptCode in (43, 73)", String.Empty, 80, 100, 100, 100, 100, 100);
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
                    MyBase.Run("Delete from Socks_Yarn_Indent_SampleWise_Requset_Details where MasterID = " + Code, "Delete from Socks_Yarn_Indent_Requset_Details where MasterID = " + Code, "Delete from Socks_Yarn_Indent_Requset_Master where RowID = " + Code);
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
                if (Convert.ToInt64(TxtUnit.Tag) == 1)
                {
                    Unit = 71;
                }
                else if (Convert.ToInt64(TxtUnit.Tag) == 2)
                {
                    Unit = 72;
                }
                TxtRemarks.Text = Dr["Remarks"].ToString();
                TxtDept.Text = Dr["DeptName"].ToString();
                TxtDept.Tag = Dr["Deptcode"].ToString();
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
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Indent Request Entry - Edit", "Select S1.EntryNo, S1.EntryDate, D1.DeptName, S2.Order_No, U1.Unit_Name, S1.ProductionDate, S1.Remarks, S1.UnitCode, S1.RowID, S1.DeptCode From Socks_Yarn_Indent_Requset_Master S1 Inner Join Socks_Yarn_Indent_Requset_Details S2 On S1.RowID = S2.MasterID Left Join VAAHINI_ERP_GAINUP.Dbo.Unit_Master U1 On S1.UnitCode = U1.RowId Left Join VAAHINI_ERP_GAINUP.Dbo.DeptType D1 On S1.DeptCode = D1.DeptCode Where S1.DeptCode in (43, 73)", String.Empty, 80, 100, 100, 100, 100, 100, 100);
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
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Indent Request Entry - View", "Select S1.EntryNo, S1.EntryDate, D1.DeptName, S2.Order_No, U1.Unit_Name, S1.ProductionDate, S1.Remarks, S1.UnitCode, S1.RowID, S1.DeptCode From Socks_Yarn_Indent_Requset_Master S1 Inner Join Socks_Yarn_Indent_Requset_Details S2 On S1.RowID = S2.MasterID Left Join VAAHINI_ERP_GAINUP.Dbo.Unit_Master U1 On S1.UnitCode = U1.RowId Left Join VAAHINI_ERP_GAINUP.Dbo.DeptType D1 On S1.DeptCode = D1.DeptCode Where S1.DeptCode in (43, 73)", String.Empty, 80, 100, 100, 100, 100, 100);
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
                //MyParent.View_Browser("MIS_SOCKS_YARNDYEING_GRN", Code);

                Str = " Select G.Unit_Name, B.Order_No, D.Item, E.Color, F.Size, Sum(Isnull(C.Indent_Qty,0))Indent_Qty From Socks_Yarn_Indent_Requset_Master A ";
                Str = Str + " Left Join Socks_Yarn_Indent_Requset_Details B On A.RowID = B.MasterID ";
                Str = Str + " Left Join Socks_Yarn_Indent_SampleWise_Requset_Details C On A.RowID = C.MasterID And B.MasterID = C.MasterID And B.Slno1 = C.Slno1 ";
                Str = Str + " Left Join Item D On C.ItemID = D.ItemID Left Join Color E On C.ColorID = E.ColorID Left Join Size F On C.SizeID = F.SizeID ";
                Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Unit_Master G On A.UnitCode = G.RowId ";
                Str = Str + " Where A.DeptCode in (43, 73) And A.RowID = " + Code;
                Str = Str + " Group By G.Unit_Name, B.Order_No, D.Item, E.Color, F.Size ";

                MyBase.Execute_Qry(Str, "Yarn_Indent_Request_Receipt");

                Str = "Select Sno, Grn_No, Item, Color, Size, LotNo, Stock From Indent_Request_Orders_Lot_Details(" + Code + ")";
                MyBase.Execute_Qry(Str, "Yarn_Order_Lot_Details");

                DataTable Dt1 = new DataTable();
                String Str1 = "Select Getdate()Date1";
                MyBase.Load_Data(Str1, ref Dt1);

                DataTable Dt2 = new DataTable();
                String Str2 = "Select IndentNo, ProductionDate from Socks_Yarn_Indent_Requset_Master Where RowID = " + Code + "";
                MyBase.Load_Data(Str2, ref Dt2);

                CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Yarn_Indent_Request_New.rpt");
                MyParent.FormulaFill(ref ObjRpt, "PrintOutDate", Dt1.Rows[0][0].ToString());
                MyParent.FormulaFill(ref ObjRpt, "ProductionDate", Dt2.Rows[0][1].ToString());
                MyParent.FormulaFill(ref ObjRpt, "IndentNo", Dt2.Rows[0][0].ToString());
                MyParent.CReport(ref ObjRpt, "Yarn Indent Request..!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Yarn_Indent_Request_Link_Stiching_Load(object sender, EventArgs e)
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
                    Str = "Select 0 Slno, S2.Order_No, S4.Item, S3.ItemID, S5.Color, S3.ColorID, S6.Size, S6.SizeID, 0.000 Stock, 0.000 Req_Qty, S3.Indent_Qty, S4.Item+S5.Color+S6.Size Particulars, '' Remarks, S2.Slno1, '-' T From Socks_Yarn_Indent_Requset_Master S1 ";
                    Str = Str + " Left Join Socks_Yarn_Indent_Requset_Details S2 On S1.RowID = S2.MasterID Left Join Socks_Yarn_Indent_SampleWise_Requset_Details S3 On S1.RowID = S3.MasterID And S2.MasterID = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                    Str = Str + " Left Join Item S4 On S3.ItemID = S4.ItemID Left Join Color S5 On S3.ColorID = S5.ColorID Left Join Size S6 On S3.SizeID = S6.SizeID Where 1 = 2 ";
                }
                else
                {
                    Str = " Select S2.Slno, S2.Order_No, S3.Descr, S3.Bom_Qty, S2.Slno1, S2.Remarks, 'O' Record, 0 T From Socks_Yarn_Indent_Requset_Master S1 ";
                    Str = Str + "Left Join Socks_Yarn_Indent_Requset_Details S2 On S1.RowID = S2.MasterID Left Join (Select Order_No, Descr, SUM(Bom_Qty)Bom_Qty From Socks_Bom() Group BY Order_No, Descr) S3 On S2.Order_No = S3.Order_No Where S1.RowId = " + Code;
                    Str = " Select 0 Slno, S2.Order_No, S4.Item, S3.ItemID, S5.Color, S3.ColorID, S6.Size, S6.SizeID, 0.000 Stock, 0.000 Req_Qty, ";
                    Str = Str + " S3.Indent_Qty, S4.Item+S5.Color+S6.Size Particulars, '' Remarks, S2.Slno1, '-' T From Socks_Yarn_Indent_Requset_Master S1 ";
                    Str = Str + " Left Join Socks_Yarn_Indent_Requset_Details S2 On S1.RowID = S2.MasterID ";
                    Str = Str + " Left Join Socks_Yarn_Indent_SampleWise_Requset_Details S3 On S1.RowID = S3.MasterID And S2.MasterID = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                    Str = Str + " Left Join Item S4 On S3.ItemID = S4.ItemID Left Join Color S5 On S3.ColorID = S5.ColorID ";
                    Str = Str + " Left Join Size S6 On S3.SizeID = S6.SizeID Where S1.RowID = " + Code;

                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "ItemID", "ColorID", "SizeID", "Particulars", "Slno1", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Order_No", "Item", "Indent_Qty", "Remarks");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 50, 120, 100, 100, 100, 100, 100, 100,180);
                Grid.Columns["Req_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Indent_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid.Columns["Remarks"].DefaultCellStyle.Format = "-";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Cancel()
        {
            MyBase.Clear(this);
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
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
                        Str = " Select Distinct Order_NO From Link_Stitch_Input(" + Unit + ", " + TxtDept.Tag + ") Order By Order_No ";

                        Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Order_NO", Str, String.Empty, 120);
                        if (Dr != null)
                        {
                            Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                            Txt.Text = Dr["Order_No"].ToString();
                            Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Item"].Index)
                    {
                        if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Str = " Select Item, Color, Size, Stock, ItemID, ColorID, SizeID, Item+Color+Size Particulars From Link_Stitch_Input(" + Unit + ", " + TxtDept.Tag + ") Where Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' Order By Order_No ";
                            Dr = Tool.Selection_Tool_Except_New("Particulars", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Particulars", Str, String.Empty, 120, 100, 100, 100);
                            if (Dr != null)
                            {
                                Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                                Txt.Text = Dr["Item"].ToString();
                                Grid["Color", Grid.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                                Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                                Grid["Stock", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["Stock"].ToString());
                                Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                                Grid["ColorID", Grid.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();
                                Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                                Grid["Particulars", Grid.CurrentCell.RowIndex].Value = Dr["Particulars"].ToString();
                                //Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Req_Qty"].ToString();
                                Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value = 0;
                            }
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
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Indent_Qty"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
                }
                else
                {
                    e.Handled = true;
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

        private void Frm_Yarn_Indent_Request_Link_Stiching_KeyDown(object sender, KeyEventArgs e)
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
                            TxtUnit.Focus();
                            return;
                        }
                        else
                        {
                            TxtDept.Focus();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtDept")
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

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Unit..!", "Select Unit_Name, RowId Unit_Code From Unit_Master", String.Empty, 300, 50);

                        if (Dr != null)
                        {
                            TxtUnit.Text = Dr["Unit_Name"].ToString();
                            TxtUnit.Tag = Dr["Unit_Code"].ToString();
                            if (Convert.ToInt64(TxtUnit.Tag) == 1)
                            {
                                Unit = 71;
                            }
                            else if (Convert.ToInt64(TxtUnit.Tag) == 2)
                            {
                                Unit = 72;
                            }
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtDept")
                    {

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Department..!", "Select DeptName, DeptCode From VAAHINI_ERP_GAINUP.Dbo.DeptType Where DeptCode in (73, 43)", String.Empty, 300, 50);

                        if (Dr != null)
                        {
                            TxtDept.Text = Dr["DeptName"].ToString();
                            TxtDept.Tag = Dr["DeptCode"].ToString();
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
                        if (Math.Round(Convert.ToDouble(Grid["Indent_Qty", Grid.CurrentCell.RowIndex].Value.ToString()), 3) <= 0)
                        {
                            MessageBox.Show("Invalid Row");
                            Grid.CurrentCell = Grid["Indent_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
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

        private void Frm_Yarn_Indent_Request_Link_Stiching_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Grid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Indent_Qty"].Index)
                {
                    if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        if (Math.Round(Convert.ToDouble(Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value.ToString()), 3) > 0)
                        {
                            if (Math.Round(Convert.ToDouble(Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value.ToString()), 3) < Math.Round(Convert.ToDouble(Grid["Indent_Qty", Grid.CurrentCell.RowIndex].Value.ToString()), 3))
                            {
                                MessageBox.Show("Invalid Indent Qty");
                                Grid.CurrentCell = Grid["Indent_Qty", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                        }
                        if (Math.Round(Convert.ToDouble(Grid["Stock", Grid.CurrentCell.RowIndex].Value.ToString()), 3) > 0)
                        {
                            if (Math.Round(Convert.ToDouble(Grid["Stock", Grid.CurrentCell.RowIndex].Value.ToString()), 3) < Math.Round(Convert.ToDouble(Grid["Indent_Qty", Grid.CurrentCell.RowIndex].Value.ToString()), 3))
                            {
                                MessageBox.Show("Invalid Indent Qty");
                                Grid["Indent_Qty", Grid.CurrentCell.RowIndex].Value = 0;
                                Grid.CurrentCell = Grid["Indent_Qty", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
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
    }
}
