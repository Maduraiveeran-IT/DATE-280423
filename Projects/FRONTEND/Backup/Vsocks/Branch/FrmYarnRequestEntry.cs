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
    public partial class FrmYarnRequestEntry : Form, Entry
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

        String Order_No = String.Empty;
        String Sample = String.Empty;
        String Size = String.Empty;
        Int64 ItemID = 0;
        Int64 ColorID = 0;
        Int64 SizeID = 0;
        Int32 Delivery_No = 0;
        Int32 Row1 = 0;
        
        public FrmYarnRequestEntry()
        {
            InitializeComponent();
        }

        private void FrmYarnRequestEntry_Load(object sender, EventArgs e)
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
                    MyBase.Load_Data("Select DateAdd (d, " + MyParent.User_Datelock + ", Cast(GetDate() as Date)) MinDate, Cast(GetDate() as Date) MaxDate ", ref Tdt);
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
                TxtUnit.Focus();
                Grid_Data();
                DtQty = new DataTable[30];
                if (MyParent.UserCode == 7 || MyParent.UserCode == 8)
                {
                    TxtUnit.Text = "FLOOR - I";
                    TxtUnit.Tag = 1;
                    DcDate.Focus(); 
                }
                else if (MyParent.UserCode == 23 || MyParent.UserCode == 40)
                {
                    TxtUnit.Text = "FLOOR - II";
                    TxtUnit.Tag = 2;
                    DcDate.Focus();
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

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Indent Request Entry - View", "Select S1.EntryNo, S1.EntryDate, U1.Unit_Name, S1.ProductionDate, S1.Remarks, S1.UnitCode, S1.RowID From Socks_Yarn_Indent_Requset_Master S1 Left Join VAAHINI_ERP_GAINUP.Dbo.Unit_Master U1 On S1.UnitCode = U1.RowId", String.Empty, 80, 100, 100, 100, 100, 100);
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
                //MyParent.View_Browser("MIS_SOCKS_YARNDYEING_GRN", Code);

                Str = "Select U1.Unit_Name, S2.Order_No, S5.Item, S5.COlor, S5.Size, Sum(ISnull(S3.Indent_Qty,0))Indent_Qty From Socks_Yarn_Indent_Requset_Master S1 ";
                Str = Str + " Left Join Socks_Yarn_Indent_Requset_Details S2 On S1.RowID = S2.MasterID ";
                Str = Str + " Left Join Socks_Yarn_Indent_SampleWise_Requset_Details S3 On S1.RowId = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                Str = Str + " Left Join Socks_Bom()S4 On S2.Order_No = S4.Order_No And S2.OrderColorID = S4.OrderColorId And S2.SizeID = S4.sizeid ";
                Str = Str + " Left Join Samplenowise_Cons_New() S5 On S4.color = S5.Sample_No And S3.ItemID = S5.Itemid And S3.ColorID = S5.Colorid And S3.SizeID = S5.Sizeid ";
                Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Unit_Master U1 On S1.UnitCode = U1.RowId ";
                Str = Str + " Where S1.RowID =  " + Code + " Group By U1.Unit_Name, S2.Order_No, S5.Item, S5.COlor, S5.Size";

                MyBase.Execute_Qry(Str, "Yarn_Indent_Request_Receipt");

                DataTable Dt1 = new DataTable(); 
                String Str1 = "Select Getdate()Date1";
                MyBase.Load_Data(Str1, ref Dt1);

                DataTable Dt2 = new DataTable();
                String Str2 = "Select IndentNo, ProductionDate from Socks_Yarn_Indent_Requset_Master Where RowID = " + Code + "";
                MyBase.Load_Data(Str2, ref Dt2);

                CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Yarn_Indent_Request.rpt");
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

        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Indent Request Entry - View", "Select S1.EntryNo, S1.EntryDate, U1.Unit_Name, S1.ProductionDate, S1.Remarks, S1.UnitCode, S1.RowID From Socks_Yarn_Indent_Requset_Master S1 Left Join VAAHINI_ERP_GAINUP.Dbo.Unit_Master U1 On S1.UnitCode = U1.RowId", String.Empty, 80, 100, 100, 100, 100, 100);
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

        public void Entry_Save()
        {
            try
            {
                Int32 Array_Index = 0;
                String From_Store = String.Empty;
                //Total_Count();

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

                    if (Grid["Balance", i].Value == DBNull.Value || Grid["Balance", i].Value.ToString() == String.Empty || Convert.ToInt64(Grid["Balance", i].Value) == 0)
                    {
                        MessageBox.Show(" ZERO Balance is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Balance", i];
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
                    TxtEntryNo.Text = MyBase.MaxOnlyWithoutComp("Socks_Yarn_Indent_Requset_Master", "EntryNO", String.Empty, String.Empty, 0).ToString();
                }
                Queries = new string[Dt.Rows.Count * 100];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_Yarn_Indent_Requset_Master(EntryNO, IndentNO, EntryDate, ProductionDate, UnitCode, Remarks, SystemName, EntryTime, UserCode, Compcode) values (" + TxtEntryNo.Text + ", " + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", DcDate.Value) + "', " + TxtUnit.Tag + ", '" + TxtRemarks.Text + "', Host_Name(), GetDate(), " + MyParent.UserCode + ", " + MyParent.CompCode + "); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_Yarn_Indent_Requset_Master", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_Yarn_Indent_Requset_Master Set EntryDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', ProductionDate = '" + String.Format("{0:dd-MMM-yyyy}", DcDate.Value) + "', Remarks = '" + TxtRemarks.Text + "', UserCode = " + MyParent.UserCode + " Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_Yarn_Indent_Requset_Master", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from Socks_Yarn_Indent_Requset_Details where MasterID = " + Code;
                    Queries[Array_Index++] = "Delete from Socks_Yarn_Indent_SampleWise_Requset_Details where MasterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Socks_Yarn_Indent_Requset_Details (MasterID, Slno, Order_NO, OrderColorID, SizeID, ItemID, BalanceQty,  Slno1, Remarks) Values (@@IDENTITY, " + Grid["Slno", i].Value + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ItemID", i].Value + ",  " + Grid["Balance", i].Value + ",  " + Grid["Slno", i].Value + ", '" + Grid["Remarks", i].Value + "')";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Socks_Yarn_Indent_Requset_Details (MasterID, Slno, Order_NO, OrderColorID, SizeID, ItemID, BalanceQty,  Slno1, Remarks) Values (" + Code + ", " + Grid["Slno", i].Value + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ItemID", i].Value + ",  " + Grid["Balance", i].Value + ",  " + Grid["Slno", i].Value + ", '" + Grid["Remarks", i].Value + "')";
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
                                        Queries[Array_Index++] = "Insert Into Socks_Yarn_Indent_SampleWise_Requset_Details (SlNo, MasterID, ItemID, ColorID, SizeID, Indent_Qty, SlNo1) Values ( " + DtQty[i].Rows[j]["Slno"].ToString() + ", @@IDENTITY, " + DtQty[i].Rows[j]["ItemID"].ToString() + ", " + DtQty[i].Rows[j]["ColorID"].ToString() + ", " + DtQty[i].Rows[j]["SizeID"].ToString() + ", " + DtQty[i].Rows[j]["Indent_Qty"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ")";
                                    }
                                }
                                else
                                {
                                    if (Convert.ToDouble(DtQty[i].Rows[j]["Indent_Qty"].ToString()) > 0)
                                    {
                                        Queries[Array_Index++] = "Insert Into Socks_Yarn_Indent_SampleWise_Requset_Details (SlNo, MasterID, ItemID, ColorID, SizeID, Indent_Qty, SlNo1) Values ( " + DtQty[i].Rows[j]["Slno"].ToString() + ", " + Code + ", " + DtQty[i].Rows[j]["ItemID"].ToString() + ", " + DtQty[i].Rows[j]["ColorID"].ToString() + ", " + DtQty[i].Rows[j]["SizeID"].ToString() + ", " + DtQty[i].Rows[j]["Indent_Qty"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ")";
                                    }
                                }
                            }
                        }
                    }
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Indent Request Entry - View", "Select S1.EntryNo, S1.EntryDate, U1.Unit_Name, S1.ProductionDate, S1.Remarks, S1.UnitCode, S1.RowID From Socks_Yarn_Indent_Requset_Master S1 Left Join VAAHINI_ERP_GAINUP.Dbo.Unit_Master U1 On S1.UnitCode = U1.RowId", String.Empty, 80, 100, 100, 100, 100, 100);
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
                TxtRemarks.Text = Dr["Remarks"].ToString();
                Grid_Data();
                //Total_Count();
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
                    Str = "Select 0 As Slno, S2.Order_No, S2.OrderColorID, S3.Color Sample, S2.SizeID, S3.Size, S3.Item, S2.ItemID, S3.Descr, S3.Bom_Qty, ";
                    Str = Str + "(S3.Bom_Qty - S4.Production) Balance, 0 Slno1, S2.Remarks, '-' T, 'N' Record From Socks_Yarn_Indent_Requset_Master S1 ";
                    Str = Str + "Left Join Socks_Yarn_Indent_Requset_Details S2 On S1.RowID = S2.MasterID ";
                    Str = Str + "Left Join Socks_Bom() S3 On S2.Order_No = S3.Order_No And S2.OrderColorID = S3.OrderColorId And S2.SizeID = S3.SizeId ";
                    Str = Str + "Left Join Socks_Knit_Prod_OrderWise()S4 On S3.Order_No = S4.Order_No And S3.Color = S4.Sample And S3.Size = S4.Size Where 1 = 2 ";
                }
                else
                {
                    Str = "Select 0 As Slno, S2.Order_No, S2.OrderColorID, S3.Color Sample, S2.SizeID, S3.Size, S3.Item, S2.ItemID, S3.Descr, S3.Bom_Qty, ";
                    Str = Str + "S2.BalanceQty Balance, Slno1, '-' T, 'O' Record From Socks_Yarn_Indent_Requset_Master S1 ";
                    Str = Str + "Left Join Socks_Yarn_Indent_Requset_Details S2 On S1.RowID = S2.MasterID ";
                    Str = Str + "Left Join Socks_Bom() S3 On S2.Order_No = S3.Order_No And S2.OrderColorID = S3.OrderColorId And S2.SizeID = S3.SizeId ";
                    Str = Str + "Left Join Socks_Knit_Prod_OrderWise()S4 On S3.Order_No = S4.Order_No And S3.Color = S4.Sample And S3.Size = S4.Size Where S1.RowId = " + Code;
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Designing(ref Grid, ref Dt, "OrderColorID", "SizeID", "ItemID", "Slno1", "T", "Record");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Order_No", "Sample", "Balance", "Remarks");
                MyBase.Grid_Width(ref Grid, 50, 110, 110, 110, 110, 130, 80, 80, 120);
                Grid.Columns["Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Balance"].Index)
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
                    //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                    //{
                    //    Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Order_NO", "Select Order_No, Sample, Size, Descr, Bom_Qty, Production, Balance, Item, Buyer, OrderColorID, SizeID From OrderWise_Production_Balance()", String.Empty, 110, 75, 65, 90, 65, 65, 65);
                    //    if (Dr != null)
                    //    {
                    //        Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                    //        Txt.Text = Dr["Order_No"].ToString();
                    //        Grid["Sample", Grid.CurrentCell.RowIndex].Value = Dr["Sample"].ToString();
                    //        Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Dr["OrderColorID"].ToString();
                    //        Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["SIZE"].ToString();
                    //        Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SIZEID"].ToString();
                    //        Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                    //        Grid["Descr", Grid.CurrentCell.RowIndex].Value = Dr["Descr"].ToString();
                    //        Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bom_Qty"].ToString();
                    //        Grid["Bal", Grid.CurrentCell.RowIndex].Value = Dr["Balance"].ToString();
                    //        Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                    //    }
                    //}
                    //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                    //{
                    //    Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Order_NO", "Select Order_No, No_OF_Machines From Get_Weekly_Planned_Orders(DatePart(Week, '" + String.Format("{0:dd-MMM-yyyy}", DcDate.Value) + "'), DatePart(Year, '" + String.Format("{0:dd-MMM-yyyy}", DcDate.Value) + "'), " + TxtUnit.Tag + ")", String.Empty, 150, 100);

                    //    if (Dr != null)
                    //    {
                    //        Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                    //        Txt.Text = Dr["Order_No"].ToString();
                    //    }
                    //}
                    //else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Sample"].Index)
                    //{
                    //    if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    //    {
                    //        Dr = Tool.Selection_Tool_Except_New("Sample", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Sample", "Select Sample, Size, Item, Descr, Bom_Qty, Bal, OrderColorId, Sizeid  From Get_Sample_Details_For_Order('" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "')", String.Empty, 100, 80, 80, 100, 80, 80, 80);
                     
                    //        if (Dr != null)
                    //        {
                    //            Grid["Sample", Grid.CurrentCell.RowIndex].Value = Dr["Sample"].ToString();
                    //            Txt.Text = Dr["Sample"].ToString();
                    //            Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Dr["OrderColorID"].ToString();
                    //            Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["SIZE"].ToString();
                    //            Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SIZEID"].ToString();
                    //            Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                    //            Grid["Descr", Grid.CurrentCell.RowIndex].Value = Dr["Descr"].ToString();
                    //            Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bom_Qty"].ToString();
                    //            Grid["Bal", Grid.CurrentCell.RowIndex].Value = Dr["Bal"].ToString();
                    //            Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                    //        }
                    //    }
 
                    //}

                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                    {
                        Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Order_NO", " Select Distinct S1.Order_No, Isnull(S2.Descr,'') Description From Stock_Details_Transfer_Entry() S1 Left Join Socks_Bom()S2 On S1.Order_No = S2.Order_No Where S1.Colorid <> 867 And S1.Order_No like '%OCN%' And S1.Order_No in (Select Distinct Order_No From Job_Ord_Mas Where Supplierid = (Case When " + TxtUnit.Tag + " = 1 Then 71 When " + TxtUnit.Tag + " = 2 Then 72 End)  And Job_Ord_No like '%JOB%') Order By S1.Order_No ", String.Empty, 120, 150);
                        if (Dr != null)
                        {
                            Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                            Txt.Text = Dr["Order_No"].ToString();
                            if (Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                Grid["Sample", Grid.CurrentCell.RowIndex].Value = "";
                                Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = "0";
                                Grid["Size", Grid.CurrentCell.RowIndex].Value = "";
                                Grid["SizeID", Grid.CurrentCell.RowIndex].Value = "0";
                                Grid["Item", Grid.CurrentCell.RowIndex].Value = "";
                                Grid["ItemID", Grid.CurrentCell.RowIndex].Value = "0";
                                Grid["Descr", Grid.CurrentCell.RowIndex].Value = "";
                                Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value = 0;
                                Grid["Balance", Grid.CurrentCell.RowIndex].Value = 0;
                                Grid["Slno1", Grid.CurrentCell.RowIndex].Value = 0;
                                Grid["Record", Grid.CurrentCell.RowIndex].Value = "N";
                            }

                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Sample"].Index)
                    {
                        if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Dr = Tool.Selection_Tool_Except_New("Sample", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Sample", "Select Sample, Size, Item, Descr, Bom_Qty, Bal Balance, OrderColorId, Sizeid, ItemID  From Get_Sample_Details_For_Order('" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "') Where Bal > 0", String.Empty, 100, 80, 80, 100, 80, 80, 80);
                            if (Dr != null)
                            {
                                Grid["Sample", Grid.CurrentCell.RowIndex].Value = Dr["Sample"].ToString();
                                Txt.Text = Dr["Sample"].ToString();
                                Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Dr["OrderColorID"].ToString();
                                Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["SIZE"].ToString();
                                Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SIZEID"].ToString();
                                Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                                Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                                Grid["Descr", Grid.CurrentCell.RowIndex].Value = Dr["Descr"].ToString();
                                Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bom_Qty"].ToString();
                                Grid["Balance", Grid.CurrentCell.RowIndex].Value = Dr["Balance"].ToString();
                                Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                                Grid["Record", Grid.CurrentCell.RowIndex].Value = "N";
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
                //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rec_Qty"].Index)
                //{
                //    if ((Grid["Rec_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) != String.Empty)
                //    {
                //        if (Convert.ToDouble(Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value) < Convert.ToDouble(Grid["Rec_Qty", Grid.CurrentCell.RowIndex].Value))
                //        {
                //            MessageBox.Show("Invalid Rec_Qty..!", "Gainup");
                //            Grid.CurrentCell = Grid["Rec_Qty", Grid.CurrentCell.RowIndex];
                //            Grid.Focus();
                //            Grid.BeginEdit(true);
                //            MyParent.Save_Error = true;
                //            return;
                //        }
                //    }
                //}
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
                TxtTotal.Text = MyBase.Sum(ref Grid, "Balance");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void GridDetail_Data(Int32 Row, String Order_NO, String Sample, String Size)
        {

            try
            {
                Row1 = Row;
                if (DtQty[Row] == null)
                {
                    DtQty[Row] = new DataTable();
                    if (MyParent._New)
                    {
                        MyBase.Load_Data("Select 0 SlNo, Item, Color, Size, Balance_Prod_Qty, Sing_Wt, Quantity Req_Qty, Stock_Qty, Quantity Indent_Qty, ItemID, ColorID, SizeID, " + Row + " SlNo1, '' T From Yarn_Indent_Require('" + Order_No.ToString() + "', '" + Sample.ToString() + "', '" + Size.ToString() + "', " + TxtUnit.Tag + ")", ref DtQty[Row]);
                    }
                    else
                    {
                        if ((Grid["Record", Grid.CurrentCell.RowIndex].Value.ToString()) == "O")
                        {
                            //MyBase.Load_Data("select A.slno Sno, A.Order_No, A.Rec_Qty Iss_Qty, A.Rec_Qty, B.Slno1,'' T from Socks_Dyeing_OrderwiseReceipt_details A Left Join Socks_Dyeing_Receipt_Details B on A.Master_ID = B.Master_ID and A.SlNo1 = B.Slno1 Left Join Socks_Dyeing_Receipt_Master C on A.Master_ID = C.RowID and B.Master_ID = C.RowID  Where  A.Master_ID =  " + Code + " and  B.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString(), ref DtQty[Row]);
                            Str = " Select S3.Slno, S5.Item, S5.COlor, S5.Size, S2.BalanceQty Balance_Prod_Qty, Cast((((S5.Single_unit_Cons * (-(P1.Loss) / (100+(P1.Loss)))) + S5.Single_unit_Cons) / 1000)as Numeric(25,4)) Sing_Wt, ";
                            Str = Str + " Cast((S2.BalanceQty * (((S5.Single_unit_Cons * (-(P1.Loss)/(100+(P1.Loss)))) + S5.Single_unit_Cons)/1000))as Numeric(25,4)) Req_Qty, Isnull(S6.Stock_Qty,0)Stock_Qty,  S3.Indent_Qty, S2.Slno1, S3.ItemID, S3.ColorID, S3.SizeID, '' T ";
                            Str = Str + " From Socks_Yarn_Indent_Requset_Master S1 Left Join Socks_Yarn_Indent_Requset_Details S2 On S1.RowID = S2.MasterID ";
                            Str = Str + " Left Join Socks_Yarn_Indent_SampleWise_Requset_Details S3 On S1.RowId = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                            Str = Str + " Left Join Socks_Bom()S4 On S2.Order_No = S4.Order_No And S2.OrderColorID = S4.OrderColorId And S2.SizeID = S4.sizeid ";
                            Str = Str + " Left Join Samplenowise_Cons() S5 On S4.color = S5.Sample_No And S3.ItemID = S5.Itemid And S3.ColorID = S5.Colorid And S3.SizeID = S5.Sizeid ";
                            Str = Str + " Left Join Process_Loss_Details()P1 on S2.Order_No = P1.Order_No Left Join Stock_Details_Transfer_Entry()S6 On S2.Order_No = S6.Order_No And S5.Item = S6.Item And S5.COlor = S6.Color";
                            Str = Str + " Where S1.RowID =  " + Code + " and S2.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() + "";
                        }
                        else
                        {
                            Str = "Select 0 SlNo, Item, Color, Size, Balance_Prod_Qty, Sing_Wt, Quantity Req_Qty, Stock_Qty, Quantity Indent_Qty, ItemID, ColorID, SizeID, " + Row + " SlNo1, '' T From Yarn_Indent_Require('" + Order_No.ToString() + "', '" + Sample.ToString() + "', '" + Size.ToString() + "', " + TxtUnit.Tag + ")";
                        }
                        MyBase.Load_Data(Str, ref DtQty[Row]);
                    }
                }
                GridDetail.DataSource = DtQty[Row];
                MyBase.Grid_Designing(ref GridDetail, ref DtQty[Row], "SlNo1", "ItemID", "ColorID", "SizeID", "T");
                MyBase.ReadOnly_Grid_Without(ref GridDetail, "Indent_Qty");
                MyBase.Grid_Colouring(ref GridDetail, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridDetail, 50, 150, 80, 80, 90, 90, 80, 80, 80, 80);
                
                GridDetail.Columns["Req_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                GridDetail.Columns["Stock_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                GridDetail.Columns["Indent_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                
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

        private void FrmYarnRequestEntry_KeyDown(object sender, KeyEventArgs e)
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

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Unit..!", "Select Unit_Name, RowId Unit_Code From VAAHINI_ERP_GAINUP.Dbo.Unit_Master Where Rowid in (1, 2)", String.Empty, 300, 50);

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

                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Balance"].Index)
                    {

                        TxtQty1.Text = Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString();
                        Order_No = Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString();
                        Sample = Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString();
                        Size = Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString();

                        GridDetail_Data(Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value), Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString());
                        GridDetail.CurrentCell = GridDetail["Indent_Qty", 0];
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
                //MyBase.Grid_Delete(ref GridDetail, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], GridDetail.CurrentCell.RowIndex);
                //DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].AcceptChanges();
                //MyBase.Row_Number(ref GridDetail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmYarnRequestEntry_KeyPress(object sender, KeyPressEventArgs e)
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
                if (TxtBalance.Text.Trim() == String.Empty || TxtBalance.Text != "0.000")
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    GridDetail.CurrentCell = GridDetail["Indent_Qty", 0];
                    GridDetail.Focus();
                    GridDetail.BeginEdit(true);
                    return;
                }
                for (int i = 0; i <= DtQty[Row1].Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(GridDetail["Indent_Qty", i].Value.ToString()) > Convert.ToDouble(GridDetail["REq_Qty", i].Value.ToString()))
                    {
                        MessageBox.Show("Invalid Indent_Qty...!", "Gainup");
                        GridDetail["Indent_Qty", i].Value = GridDetail["Req_Qty", i].Value;
                        GridDetail.CurrentCell = GridDetail["Indent_Qty", i];
                        GridDetail.Focus();
                        GridDetail.BeginEdit(true);
                        return;
                    }
                }
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

        private void ButExit_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= GridDetail.Rows.Count - 1; i++)
                {
                    if (GridDetail["Indent_Qty", i].Value == DBNull.Value)
                    {
                        MessageBox.Show("Invalid KGS ..!", "Gainup");
                        Grid.CurrentCell = Grid["Balance", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        GBQty.Visible = false;
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                for (int i = 0; i <= DtQty[Row1].Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(GridDetail["Indent_Qty", i].Value.ToString()) > Convert.ToDouble(GridDetail["REq_Qty", i].Value.ToString()))
                    {
                        MessageBox.Show("Invalid Indent_Qty...!", "Gainup");
                        GridDetail["Indent_Qty", i].Value = GridDetail["Req_Qty", i].Value;
                        GridDetail.CurrentCell = GridDetail["Indent_Qty", i];
                        GridDetail.Focus();
                        GridDetail.BeginEdit(true);
                        return;
                    }
                }
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

                TxtEnteredWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum_With_Three_Digits(ref GridDetail, "Req_Qty")));

                if (TxtEnteredWeight.Text.Trim() == String.Empty)
                {
                    TxtEnteredWeight.Text = "0.000";
                }

                //TxtBalance.Text = String.Format("{0:0.000}", Convert.ToDouble(TxtQty1.Text) - Convert.ToDouble(TxtEnteredWeight.Text));
                TxtBalance.Text = "0.000";

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
                        else if (Convert.ToDouble(GridDetail["Indent_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString()) > Convert.ToDouble(GridDetail["REq_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString()))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Indent_Qty...!", "Gainup");
                            GridDetail["Indent_Qty", GridDetail.CurrentCell.RowIndex].Value = GridDetail["Req_Qty", GridDetail.CurrentCell.RowIndex].Value;
                            GridDetail.CurrentCell = GridDetail["Indent_Qty", Grid.CurrentCell.RowIndex];
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

        private void TxtEnteredWeight_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
