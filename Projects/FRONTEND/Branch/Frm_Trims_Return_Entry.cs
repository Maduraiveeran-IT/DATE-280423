using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SelectionTool_NmSp;
using Accounts_ControlModules;
using Accounts;

namespace Accounts
{
    public partial class Frm_Trims_Return_Entry : Form, Entry
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

        public Frm_Trims_Return_Entry()
        {
            InitializeComponent();
        }

        private void Frm_Trims_Return_Entry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                TxtUnit.Focus();
                GBStore.Visible = false;
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
                Set_Min_Max_Date(true);
                TxtUnit.Focus();
                //Grid_Data();
                DtQty = new DataTable[30];
                if (MyParent.UserCode == 7 || MyParent.UserCode == 8)
                {
                    TxtUnit.Text = "FLOOR - I";
                    TxtUnit.Tag = 1;
                }
                else if (MyParent.UserCode == 23 || MyParent.UserCode == 40)
                {
                    TxtUnit.Text = "FLOOR - II";
                    TxtUnit.Tag = 2;
                }
                else
                {
                    TxtUnit.Text = "";
                    TxtUnit.Tag = 0;
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
                if (MyParent.UserCode == 19 || MyParent.UserCode == 92)
                {
                    return;
                }

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
                    if (Grid["Return_Qty", i].Value == DBNull.Value || Grid["Return_Qty", i].Value.ToString() == String.Empty)
                    {
                        MessageBox.Show(" ZERO Balance is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Balance", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }


                if (MyParent._New)
                {
                    TxtEntryNo.Text = MyBase.MaxOnlyWithoutComp("Socks_Yarn_Indent_Return_Master", "ReturnNO", String.Empty, String.Empty, 0).ToString();
                }
                Queries = new string[Dt.Rows.Count * 100];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_Yarn_Indent_Return_Master(ReturnNO, EntryDate, UnitCode, Remarks, DeptCode, UserCode, CompCode, EntrySystem, EntryTime, Issue_Master_RowID) values (" + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtUnit.Tag + ", '" + TxtRemarks.Text + "', 82, " + MyParent.UserCode + ", " + MyParent.CompCode + ", Host_Name(), GetDate(), " + TxtIssueNo.Tag + "); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_Yarn_Indent_Return_Master", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_Yarn_Indent_Return_Master Set EntryDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', Remarks = '" + TxtRemarks.Text + "', UserCode = " + MyParent.UserCode + ", DeptCode = 82, EntrySystem = Host_Name(), EntryTime = Getdate() Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_Yarn_Indent_Return_Master", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from Socks_Yarn_Indent_Return_Details where MasterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Grid["Return_Qty", i].Value.ToString()) > 0.000)
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into Socks_Yarn_Indent_Return_Details (MasterID, Slno, Order_NO, ItemID, ColorID, SizeID, LotNo, Weight, Remarks) Values (@@IDENTITY, " + Grid["Slno", i].Value + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + ", " + Grid["SizeID", i].Value + ", '" + Grid["LotNo", i].Value.ToString() + "', " + Grid["Return_Qty", i].Value + ", '" + Grid["Remarks", i].Value + "')";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert into Socks_Yarn_Indent_Return_Details (MasterID, Slno, Order_NO, ItemID, ColorID, SizeID, LotNo, Weight, Remarks) Values (" + Code + ", " + Grid["Slno", i].Value + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + ", " + Grid["SizeID", i].Value + ", '" + Grid["LotNo", i].Value.ToString() + "', " + Grid["Return_Qty", i].Value + ", '" + Grid["Remarks", i].Value + "')";
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
                Str = " Select S1.ReturnNo, S1.EntryDate, S2.Order_No, S3.IssueNo, U1.Unit_Name, S1.Remarks, S1.UnitCode, S1.RowID, S1.Issue_Master_RowID From Socks_Yarn_Indent_Return_Master S1 ";
                Str = Str + " Inner Join Socks_Yarn_Indent_Return_Details S2 On S1.RowID = S2.MasterID Left Join Socks_Yarn_Indent_Issue_Master S3 On S1.Issue_Master_RowID = S3.RowID ";
                Str = Str + " Left Join Unit_Master U1 On S1.UnitCode = U1.RowId Where S1.DeptCode in (82) And Isnull(S1.Store_Accept,'N') = 'N'";
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Indent Return Entry - View", Str, String.Empty, 80, 100, 100, 100, 100, 100);
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
                    MyBase.Run("Delete from Socks_Yarn_Indent_Return_Details where MasterID = " + Code, "Delete from Socks_Yarn_Indent_Return_Master where RowID = " + Code);
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
                TxtEntryNo.Text = Dr["ReturnNo"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["EntryDate"]);
                TxtUnit.Tag = Dr["UnitCode"].ToString();
                TxtUnit.Text = Dr["Unit_Name"].ToString();
                TxtIssueNo.Text = Dr["IssueNo"].ToString();
                TxtIssueNo.Tag = Dr["Issue_Master_RowID"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();
                Grid_Data();
                //Total_Count();
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

                if (MyParent.UserCode == 19 || MyParent.UserCode == 92 || MyParent.UserCode == 1)
                {
                    GBStore.Visible = true;
                }
                else
                {
                    GBStore.Visible = false;
                }
                Str = " Select S1.ReturnNo, S1.EntryDate, S2.Order_No, S3.IssueNo, U1.Unit_Name, S1.Remarks, S1.UnitCode, S1.RowID, S1.Issue_Master_RowID From Socks_Yarn_Indent_Return_Master S1 ";
                Str = Str + " Inner Join Socks_Yarn_Indent_Return_Details S2 On S1.RowID = S2.MasterID Left Join Socks_Yarn_Indent_Issue_Master S3 On S1.Issue_Master_RowID = S3.RowID ";
                Str = Str + " Left Join Unit_Master U1 On S1.UnitCode = U1.RowId Where S1.DeptCode in (82) And Isnull(S1.Store_Accept,'N') = 'N' ";
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Indent Return Entry - Edit", Str, String.Empty, 80, 100, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Return_Qty", 0];
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
                Str = " Select S1.ReturnNo, S1.EntryDate, S2.Order_No, S3.IssueNo, U1.Unit_Name, S1.Remarks, S1.UnitCode, S1.RowID, S1.Issue_Master_RowID From Socks_Yarn_Indent_Return_Master S1 ";
                Str = Str + " Inner Join Socks_Yarn_Indent_Return_Details S2 On S1.RowID = S2.MasterID Left Join Socks_Yarn_Indent_Issue_Master S3 On S1.Issue_Master_RowID = S3.RowID ";
                Str = Str + " Left Join Unit_Master U1 On S1.UnitCode = U1.RowId Where S1.DeptCode in (82) ";
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Indent Return Entry - View", Str, String.Empty, 80, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Return_Qty", 0];
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
                Str = " Select B.Slno, B.Order_No, C.Item, B.ItemID, D.Color, B.ColorID, E.Size, B.SizeID, B.LotNo, F.Weight, B.Weight Return_Qty, ";
                Str = Str + " B.Remarks, '-' T From Socks_Yarn_Indent_Return_Master A ";
                Str = Str + " Left Join Socks_Yarn_Indent_Return_Details B On A.RowID = B.MasterID Left Join Item C On B.ItemID = C.ItemID ";
                Str = Str + " Left Join Color D On B.ColorID = D.ColorID Left Join Size E On B.Sizeid = E.SizeID Left Join ";
                Str = Str + " (Select A.RowID, A.IssueNo, B.Order_No, B.ItemID, B.ColorID, B.SizeID, C.LotNo, SUM(C.Weight)Weight  From Socks_Yarn_Indent_Issue_Master A ";
                Str = Str + " Left Join Socks_Yarn_Indent_Issue_Details B On A.RowID = B.MasterID Left Join Socks_Yarn_Indent_SampleWise_Issue_Details C On A.RowID = C.MasterID And B.MasterID = C.MasterID And B.SlNo1 = C.Slno1 ";
                Str = Str + " Group By A.RowID, A.IssueNo, B.Order_No, B.ItemID, B.ColorID, B.SizeID, C.LotNo)F On A.Issue_Master_RowID = F.RowID And B.Order_No = F.Order_No And B.ItemID = F.ItemID And B.ColorID = F.ColorID And B.SizeID = F.SizeID And B.LotNo = F.LotNo ";
                Str = Str + " Where A.RowID = " + Code;

                MyBase.Execute_Qry(Str, "Yarn_Indent_Return_Floor");

                DataTable Dt1 = new DataTable();
                String Str1 = "Select Getdate()Date1";
                MyBase.Load_Data(Str1, ref Dt1);

                DataTable Dt2 = new DataTable();
                String Str2 = "Select Cast(EntryDate As date)EntryDate From Socks_Yarn_Indent_Return_Master Where RowID = " + Code + "";
                MyBase.Load_Data(Str2, ref Dt2);

                DataTable Dt3 = new DataTable();
                String Str3 = "Select B.Unit_Name From Socks_Yarn_Indent_Return_Master A Left Join Unit_Master B On A.UnitCode = B.RoWId  Where A.RowID = " + Code + "";
                MyBase.Load_Data(Str3, ref Dt3);

                CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Yarn_Indent_Return.rpt");
                MyParent.FormulaFill(ref ObjRpt, "From", Dt3.Rows[0][0].ToString());
                MyParent.FormulaFill(ref ObjRpt, "PrintOutDate", Dt1.Rows[0][0].ToString());
                MyParent.FormulaFill(ref ObjRpt, "ProductionDate", Dt2.Rows[0][0].ToString());
                MyParent.FormulaFill(ref ObjRpt, "IndentNo", TxtEntryNo.Text.ToString());
                MyParent.CReport(ref ObjRpt, "Yarn Indent Return..!");
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
                    Str = " Select 0 As Slno, B.Order_No, D.Item, B.ItemID, E.Color, B.ColorID, F.Size, B.SizeID, C.LotNo, SUM(C.Weight)Weight, Cast(0 As Numeric(20,3)) Return_Qty, '-' Remarks, '' T From Socks_Yarn_Indent_Issue_Master A ";
                    Str = Str + " Left Join Socks_Yarn_Indent_Issue_Details B On A.RowID = B.MasterID Left Join Socks_Yarn_Indent_SampleWise_Issue_Details C On A.RowID = C.MasterID And B.MasterID = C.MasterID And B.SlNo1 = C.Slno1 ";
                    Str = Str + " Left Join Item D On B.ItemID = D.ItemID Left Join Color E On B.ColorID = E.ColorID Left Join Size F On B.SizeID = F.SizeID Where A.RowID = " + TxtIssueNo.Tag + " Group By B.Order_No, D.Item, B.ItemID, E.Color, B.ColorID, F.Size, B.SizeID, C.LotNo Having SUM(C.Weight) > 0";
                }
                else
                {
                    Str = " Select B.Slno, B.Order_No, C.Item, B.ItemID, D.Color, B.ColorID, E.Size, B.SizeID, B.LotNo, F.Weight, B.Weight Return_Qty, B.Remarks, B.RowID Return_Details_ID, '-' T From Socks_Yarn_Indent_Return_Master A ";
                    Str = Str + " Left Join Socks_Yarn_Indent_Return_Details B On A.RowID = B.MasterID Left Join Item C On B.ItemID = C.ItemID ";
                    Str = Str + " Left Join Color D On B.ColorID = D.ColorID Left Join Size E On B.Sizeid = E.SizeID Left Join ";
                    Str = Str + " (Select A.RowID, A.IssueNo, B.Order_No, B.ItemID, B.ColorID, B.SizeID, C.LotNo, SUM(C.Weight)Weight  From Socks_Yarn_Indent_Issue_Master A ";
                    Str = Str + " Left Join Socks_Yarn_Indent_Issue_Details B On A.RowID = B.MasterID Left Join Socks_Yarn_Indent_SampleWise_Issue_Details C On A.RowID = C.MasterID And B.MasterID = C.MasterID And B.SlNo1 = C.Slno1 ";
                    Str = Str + " Group By A.RowID, A.IssueNo, B.Order_No, B.ItemID, B.ColorID, B.SizeID, C.LotNo)F On A.Issue_Master_RowID = F.RowID And B.Order_No = F.Order_No And B.ItemID = F.ItemID And B.ColorID = F.ColorID And B.SizeID = F.SizeID And B.LotNo = F.LotNo Where A.RowID = " + Code;
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                if (MyParent._New == true)
                {
                    MyBase.Grid_Designing(ref Grid, ref Dt, "ItemID", "ColorID", "SizeID", "T");
                }
                else
                {
                    MyBase.Grid_Designing(ref Grid, ref Dt, "ItemID", "ColorID", "SizeID", "Return_Details_ID", "T");
                }
                if (MyParent.UserCode == 19 || MyParent.UserCode == 92)
                {
                    MyBase.ReadOnly_Grid_Without(ref Grid);
                }
                else
                {
                    MyBase.ReadOnly_Grid_Without(ref Grid, "Return_Qty", "Remarks");
                }
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 45, 110, 120, 150, 100, 110, 110, 110, 150);
                Grid.Columns["Weight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Return_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Return_Qty"].Index)
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

                    }
                    else if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value == null || Grid["Order_No", Grid.CurrentCell.RowIndex].Value == DBNull.Value)
                    {

                    }
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
                //if (e.KeyCode == Keys.Down)
                //{
                //    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                //    {
                //        Str = " Select Distinct A.Order_No, Isnull(B.Buyer,'')Description, ISnull(B.Bom_Qty,0) Bom_Qty From Socks_Store_Current_Stock_OrderWise()A Inner Join ";
                //        Str = Str + " (Select Order_No, Buyer, SUM(Bom_Qty)Bom_Qty, Despatch_Closed From Socks_Bom() Group BY Order_No, Buyer, Despatch_Closed) B On A.Order_no = B.Order_No ";
                //        Str = Str + " Where ISnull(Despatch_Closed,'N') = 'N'";
                //        if (Convert.ToInt16(TxtUnit.Tag) == 1 || Convert.ToInt16(TxtUnit.Tag) == 2)
                //        {
                //            Str = Str + " And A.Order_No in (Select Distinct Order_No From Job_Ord_Mas Where Supplierid = (Case When " + TxtUnit.Tag + " = 1 Then 71 When " + TxtUnit.Tag + " = 2 Then 72 End) Union All Select Distinct Order_No From Socks_JobOrder_Master A Left Join Socks_JobOrder_Details B On A.RowID = B.Master_ID Left JOin Socks_Order_Master C On B.Order_ID = C.RowID Where Unit_Code = (Case When " + TxtUnit.Tag + " = 1 Then 71 When " + TxtUnit.Tag + " = 2 Then 72 End)) ";
                //        }
                //        Str = Str + " Order By A.Order_No ";
                //        Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Order_NO", Str, String.Empty, 120, 150, 100);
                //        if (Dr != null)
                //        {
                //            Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                //            Txt.Text = Dr["Order_No"].ToString();
                //            Grid["Descr", Grid.CurrentCell.RowIndex].Value = Dr["Description"].ToString();
                //            Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bom_Qty"].ToString();
                //            Grid["Record", Grid.CurrentCell.RowIndex].Value = "N";
                //            Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                //        }
                //    }
                //}
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
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Return_Qty"].Index)
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

        private void Frm_Trims_Return_Entry_KeyDown(object sender, KeyEventArgs e)
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
                            TxtOrder.Focus();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtOrder")
                    {
                        if (TxtOrder.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Select Order..!", "Gainup");
                            return;
                        }
                        else
                        {
                            TxtIssueNo.Focus();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtIssueNo")
                    {
                        if (TxtIssueNo.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Select Issue..!", "Gainup");
                            return;
                        }
                        else
                        {
                            DtpDate.Focus();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "DtpDate")
                    {
                        Grid.CurrentCell = Grid["Return_Qty", 0];
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
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtOrder")
                    {
                        if (TxtUnit.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Invalid Unit...!");
                            return;
                        }
                        else
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Order..!", "Select Distinct B.Order_No From Socks_Yarn_Indent_Issue_Master A Left Join Socks_Yarn_Indent_Issue_Details B On A.RowID = B.MasterID Where A.DeptCode = 82 And A.UnitCode = " + TxtUnit.Tag + " Order By B.Order_No Asc", String.Empty, 200);

                            if (Dr != null)
                            {
                                TxtOrder.Text = Dr["Order_No"].ToString();
                            }
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtIssueNo")
                    {
                        if (TxtUnit.Text.ToString() == String.Empty || TxtOrder.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Invalid Unit (or) Order Number...!");
                            return;
                        }
                        else
                        {
                            Str = " Select Distinct A.IssueNo, A.IssueDate, A.EntryNo, G.IndentNo, D.Item, E.Color, F.Size, Sum(Isnull(C.Weight,0))Issued, A.RowID From Socks_Yarn_Indent_Issue_Master A ";
                            Str = Str + " Left Join Socks_Yarn_Indent_Issue_Details B On A.RowID = B.MasterID ";
                            Str = Str + " Left Join Socks_Yarn_Indent_SampleWise_Issue_Details C On A.RowID = C.MasterID And B.MasterID = C.MasterID And B.SlNo1 = C.Slno1 ";
                            Str = Str + " Left Join Item D On B.ItemID = D.ItemID Left join Color E On B.ColorID = E.colorID Left Join Size F On B.SizeID = F.SizeID ";
                            Str = Str + " Left Join Socks_Yarn_Indent_Requset_Master G On A.Indent_Master_MasterID = G.RowID ";
                            Str = Str + " Where A.DeptCode = 82 And A.UnitCode = " + TxtUnit.Tag + " And B.Order_No = '" + TxtOrder.Text.ToString() + "'";
                            Str = Str + " Group BY A.IssueNo, A.IssueDate, A.EntryNo, G.IndentNo, D.Item, E.Color, F.Size, A.RowID ";
                            Str = Str + " Order By A.IssueNo, A.IssueDate, A.EntryNo, G.IndentNo, D.Item, E.Color, F.Size Desc";

                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Issue_No..!", Str, String.Empty, 150, 100, 50, 100, 100, 100, 100);

                            if (Dr != null)
                            {
                                TxtIssueNo.Text = Dr["IssueNo"].ToString();
                                TxtIssueNo.Tag = Dr["RowID"].ToString();
                                Grid_Data();
                                Grid.CurrentCell = Grid["Return_Qty", 0];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Return_Qty"].Index)
                    {
                        Order_No = Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString();
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

        private void Grid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Return_Qty"].Index)
                    {
                        if (Math.Round(Convert.ToDouble(Grid["Weight", Grid.CurrentCell.RowIndex].Value.ToString()), 3) < Math.Round(Convert.ToDouble(Grid["Return_Qty", Grid.CurrentCell.RowIndex].Value.ToString()), 3))
                        {
                            MessageBox.Show("Invalid Return Qty...!");
                            Grid.CurrentCell = Grid["Return_Qty", Grid.CurrentCell.RowIndex];
                            Grid["Return_Qty", Grid.CurrentCell.RowIndex].Value = "0";
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
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

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyParent.UserCode != 19 && MyParent.UserCode != 1 && MyParent.UserCode != 92)
                {
                    return;
                }

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

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Return_Qty", i].Value == DBNull.Value || Grid["Return_Qty", i].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Return_Qty", i].Value) == 0)
                    {
                        MessageBox.Show(" ZERO is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        MyParent.Save_Error = true;
                        return;
                    }
                }

                if (MessageBox.Show("Sure to Accept this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    TxtEntryNo.Text = MyBase.MaxOnlyWithoutComp("Socks_Store_Yarn_Return_Accept_Master", "EntryNO", String.Empty, String.Empty, 0).ToString();

                    Str = " Select Cast('GUP-SSR' As Varchar(20)) + RIGHT('00000' + Cast(Isnull(Max(Cast(Replace(ReturnNo,'GUP-SSR','') As Numeric(20))),0) + 1 As Varchar(20)), 5)ReturnNO From Socks_Store_Yarn_Return_Accept_Master ";
                    MyBase.Load_Data(Str, ref Is1);

                    Queries = new string[Dt.Rows.Count * 150];

                    Queries[Array_Index++] = "Insert into Socks_Store_Yarn_Return_Accept_Master (ReturnNo, ReturnDate, UserCode, Compcode, EntrySystem, EntryTime, EntryNo, Remarks, Return_Master_ID, UnitCode) values ('" + Is1.Rows[0]["ReturnNo"] + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + MyParent.UserCode + ", " + MyParent.CompCode + ", Host_Name(), GetDate(), " + TxtEntryNo.Text + ", '" + TxtRemarks.Text + "', " + Code + ", " + TxtUnit.Tag + "); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_Store_Yarn_Return_Accept_Master", "ADD", "@@IDENTITY");
                    Queries[Array_Index++] = "Update Socks_Yarn_Indent_Return_Master Set Store_Accept = 'Y' Where RowID = " + Code;

                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        Queries[Array_Index++] = "Insert into Socks_Store_Yarn_Return_Accept_Details (MasterID, Slno, Order_No, ItemID, ColorID, SizeID, Return_Qty, Return_Details_ID, Slno1) Values (@@IDENTITY, " + Grid["Slno", i].Value + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + ", " + Grid["SizeID", i].Value + ", " + Grid["Return_Qty", i].Value + ",  " + Grid["Return_Details_ID", i].Value + ", " + Grid["Slno", i].Value + ")";
                    }

                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Convert.ToDouble(Grid["Return_Qty", i].Value.ToString()) > 0)
                        {
                            DataTable Dt1 = new DataTable();

                            Str = " Select C.Grn_No, C.LotNo, C.BagNo, Weight Stock, C.VSocks_Lot_Bag_Details_RowID From Socks_Yarn_Indent_Issue_Master A ";
                            Str = Str + " Left Join Socks_Yarn_Indent_Issue_Details B On A.RowID = B.MasterID ";
                            Str = Str + " Left Join Socks_Yarn_Indent_SampleWise_Issue_Details C On A.RowID = C.MasterID And B.MasterID = C.MasterID And B.SlNo1 = C.Slno1 ";
                            Str = Str + " Where A.RowID = " + TxtIssueNo.Tag + " And B.Order_No = '" + Grid["Order_No", i].Value.ToString() + "' And B.ItemID = " + Grid["ItemID", i].Value.ToString() + " And B.ColorID = " + Grid["ColorID", i].Value.ToString() + " And B.SizeID = " + Grid["SizeID", i].Value.ToString() + " ";
                            Str = Str + " And C.LotNo = '" + Grid["LotNo", i].Value.ToString() + "' Order By C.RowID Desc ";

                            MyBase.Load_Data(Str, ref Dt1);
                            if (Dt1.Rows.Count > 0)
                            {
                                Double Tot_Iss_Qty = 0.000;
                                Double Iss_Qty = 0.000;
                                Double Bal_Qty = 0.000;
                                int l = 0;

                                Tot_Iss_Qty = Math.Round(Convert.ToDouble(Grid["Return_Qty", i].Value.ToString()), 3);
                                Bal_Qty = Math.Round(Convert.ToDouble(Grid["Return_Qty", i].Value.ToString()), 3);

                                while (Math.Round(Convert.ToDouble(Tot_Iss_Qty), 3) > Math.Round(Convert.ToDouble(Iss_Qty), 3) && Math.Round(Bal_Qty, 3) > 0)
                                {
                                    Iss_Qty = Iss_Qty + Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);

                                    if (Iss_Qty <= Tot_Iss_Qty)
                                    {
                                        Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_Return_Accept_Samplewise_Details (MasterID, SlNo, SlNo1, LotNo, Weight, BagNo, VSocks_Lot_Bag_Details_RowID, Grn_No) Values (@@IDENTITY, " + Grid["Slno", i].Value.ToString() + ", " + Grid["Slno", i].Value.ToString() + ", '" + Grid["LotNo", i].Value.ToString() + "', " + Dt1.Rows[l]["Stock"].ToString() + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ", '" + Dt1.Rows[l]["Grn_No"].ToString() + "')";
                                        Queries[Array_Index++] = "Update A Set Prod_Issue_Return = Prod_Issue_Return + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                        Bal_Qty = Bal_Qty - Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString());
                                    }
                                    else if ((Iss_Qty > Tot_Iss_Qty) && (Math.Round(Bal_Qty, 3) > 0))
                                    {
                                        if (Bal_Qty <= Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()))
                                        {
                                            Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_Return_Accept_Samplewise_Details (MasterID, SlNo, SlNo1, LotNo, Weight, BagNo, VSocks_Lot_Bag_Details_RowID, Grn_No) Values (@@IDENTITY, " + Grid["Slno", i].Value.ToString() + ", " + Grid["Slno", i].Value.ToString() + ", '" + Grid["LotNo", i].Value.ToString() + "', " + Bal_Qty + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ", '" + Dt1.Rows[l]["Grn_No"].ToString() + "')";
                                            Queries[Array_Index++] = "Update A Set Prod_Issue_Return = Prod_Issue_Return + " + Bal_Qty + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                            Bal_Qty = 0;
                                        }
                                        else if (Bal_Qty > Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()))
                                        {
                                            Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_Return_Accept_Samplewise_Details (MasterID, SlNo, SlNo1, LotNo, Weight, BagNo, VSocks_Lot_Bag_Details_RowID, Grn_No) Values (@@IDENTITY, " + Grid["Slno", i].Value.ToString() + ", " + Grid["Slno", i].Value.ToString() + ", '" + Grid["LotNo", i].Value.ToString() + "', " + Dt1.Rows[l]["Stock"].ToString() + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ", '" + Dt1.Rows[l]["Grn_No"].ToString() + "')";
                                            Queries[Array_Index++] = "Update A Set Prod_Issue_Return = Prod_Issue_Return + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                            Bal_Qty = Bal_Qty - Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString());
                                        }
                                    }

                                    l = l + 1;
                                }
                            }
                        }
                    }

                    MyBase.Run_Identity(false, Queries);
                    MyParent.Save_Error = false;
                    MessageBox.Show("Saved ..!", "Gainup");
                    MyBase.Clear(this);
                }
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnReject_Click(object sender, EventArgs e)
        {
            try
            {
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
