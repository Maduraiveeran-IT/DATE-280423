using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmGridPo : Form
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();        
        DataRow Dr;

        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataTable Dt2 = new DataTable();
        DataTable Dt3 = new DataTable();
        DataTable Dt4 = new DataTable();
        DataTable Dt5 = new DataTable();
        DataTable Dt6 = new DataTable();


        public FrmGridPo()
        {
            InitializeComponent();
        }

        private void FrmGridPo_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                OptPoPen.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmGridPo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Down)
                {
                    //if (this.ActiveControl.Name == "TxtGroup")
                    //{
                    //    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Group", "select Name,RowID from Asset_Group_Master ", String.Empty, 200);
                    //    if (Dr != null)
                    //    {
                    //        TxtGroup.Text = Dr["Name"].ToString();
                    //        TxtGroup.Tag = Dr["RowID"].ToString();
                    //    }
                    //}
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
        private void btnReport_Click(object sender, EventArgs e)
        {
            String Str = String.Empty;
            String Str1 = String.Empty;
            String Str2 = String.Empty;
            String Str3 = String.Empty;
            String Str4 = String.Empty;
            String Str5 = String.Empty;
            String Str6 = String.Empty;
            try
            {
                Dt = new DataTable();
                Dt1 = new DataTable();
                Dt2 = new DataTable();
                Dt3 = new DataTable();
                Dt4 = new DataTable();
                Dt5 = new DataTable();
                Dt6 = new DataTable();
                Grid.DataSource = null;
                if (OptLot.Checked == true)
                {
                    Str = "Select supplier, PONo, PoDate, Item, Color, Size, Po_Qty, GRNNo, GRNDate, Qty, Rej_Qty, Rej_Reas, Entry_No, Entry_Date, Invoice_No, Invoice_Date, Bill_Qty, Bill_Rate, Item_Id, Color_Id, Size_Id, Order_ID from Grid_Po_Grn_Inv_New('" + String.Format("{0:dd-MMM-yyyy}", DTFrom.Value) + "','" + String.Format("{0:dd-MMM-yyyy}", DTTo.Value) + "') ";
                    
                    MyBase.Load_Data(Str, ref Dt);
                    Grid.DataSource = MyBase.V_DataTable(ref Dt);
                    MyBase.Grid_Designing(ref Grid, ref Dt, "Item_id", "Color_Id", "Size_Id", "Order_Id");
                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100);
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    MyBase.V_DataGridView(ref Grid);
                    Grid.Focus();
                }
                else if (OptAll.Checked == true)
                {
                    Str1 = "Select Po_Type, Supplier, PoNo, PoDate, Req_Date Our_Req_Date, Commit_Date Sup_Commit,  Ack_Date Po_Ack_Date, Mail Mail_Status, Item, Color, Size, Po_Qty, GRNDate, Grn_Qty, Rej_Qty, Pen_Qty, Item_Id, Color_Id, Size_Id from Grid_Data_Summary('" + String.Format("{0:dd-MMM-yyyy}", DTFrom.Value) + "','" + String.Format("{0:dd-MMM-yyyy}", DTTo.Value) + "') ";

                    //MyBase.Execute_Qry(Str, "RPT_DEPRECIATION_REGISTER");
                    //Str = "Select * from RPT_DEPRECIATION_REGISTER";
                    MyBase.Load_Data(Str1, ref Dt1);
                    Grid.DataSource = MyBase.V_DataTable(ref Dt1);
                    MyBase.Grid_Designing(ref Grid, ref Dt1, "Item_id", "Color_Id", "Size_Id");
                    Grid.Columns["Mail_Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    Grid.Columns["Po_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns["Grn_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns["Rej_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns["Pen_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 110, 170, 105, 95, 95, 95, 95, 95, 115, 145, 95, 95, 95, 95, 95, 95);
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    MyBase.V_DataGridView(ref Grid);
                    Grid.Focus();
                }
                else if (OptPerf.Checked == true)
                {
                    Str6 = "Select Buyer, Merch, OCN, PoNo, Supplier, Item, Color, Size, Po_Qty, Grn_Qty, Rej_Qty, Pen_Qty, PoDate, GRNDate, Lead_Days, Po_Type, Order_Id, Item_Id, Color_Id, Size_Id from Grid_Data_Summary_Orderwise('" + String.Format("{0:dd-MMM-yyyy}", DTFrom.Value) + "','" + String.Format("{0:dd-MMM-yyyy}", DTTo.Value) + "') ";

                    //MyBase.Execute_Qry(Str, "RPT_DEPRECIATION_REGISTER");
                    //Str = "Select * from RPT_DEPRECIATION_REGISTER";
                    MyBase.Load_Data(Str6, ref Dt6);
                    Grid.DataSource = MyBase.V_DataTable(ref Dt6);
                    MyBase.Grid_Designing(ref Grid, ref Dt6, "Order_Id", "Item_id", "Color_Id", "Size_Id");                    
                    Grid.Columns["Po_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns["Grn_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns["Rej_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns["Pen_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns["Lead_Days"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 130, 110, 110, 110, 130, 110, 110, 75, 75, 75, 75, 75, 100, 100, 75, 100);
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    MyBase.V_DataGridView(ref Grid);
                    Grid.Focus();
                }
                else if(OptGrnPen.Checked == true)
                {
                    Str2 = "Select Po_Type, Supplier, PoNo, PoDate, Req_Date Our_Req_Date, Commit_Date Sup_Commit,  Ack_Date Po_Ack_Date, Mail Mail_Status, Item, Color, Size, Po_Qty, GRNDate,  Grn_Qty, Rej_Qty, Pen_Qty, Item_Id, Color_Id, Size_Id from Grid_Data_Summary('" + String.Format("{0:dd-MMM-yyyy}", DTFrom.Value) + "','" + String.Format("{0:dd-MMM-yyyy}", DTTo.Value) + "') Where Pen_Qty>0";

                    //MyBase.Execute_Qry(Str, "RPT_DEPRECIATION_REGISTER");
                    //Str = "Select * from RPT_DEPRECIATION_REGISTER";
                    MyBase.Load_Data(Str2, ref Dt2);
                    Grid.DataSource = MyBase.V_DataTable(ref Dt2);
                    MyBase.Grid_Designing(ref Grid, ref Dt2, "Item_id", "Color_Id", "Size_Id");
                    Grid.Columns["Mail_Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    Grid.Columns["Po_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns["Grn_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns["Rej_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns["Pen_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 120, 170, 105, 95, 95, 95, 95, 95, 115, 140, 95, 95, 95, 95, 95, 95);
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    MyBase.V_DataGridView(ref Grid);
                    Grid.Focus();
                }
                else if (OptGrnPenOcn.Checked == true)
                {
                    Str5 = "Select Po_Type, Merch, Buyer, Supplier, Order_No, Item, Color, Size, Pen_Qty, Stock_Req,  'OCN' Order_Type From Grid_Data_Grn_Pending_Ocn() where company_code = "+ MyParent.CompCode +" Order By Item, Color, Size";

                    //MyBase.Execute_Qry(Str, "RPT_DEPRECIATION_REGISTER");
                    //Str = "Select * from RPT_DEPRECIATION_REGISTER";
                    MyBase.Load_Data(Str5, ref Dt5);
                    Grid.DataSource = MyBase.V_DataTable(ref Dt5);
                    //MyBase.Grid_Designing(ref Grid, ref Dt2, "Item_id", "Color_Id", "Size_Id");
                    Grid.Columns["Pen_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns["Stock_Req"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                    
                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 110, 150, 170, 170, 115, 145, 145, 95, 95, 95, 95);
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    MyBase.V_DataGridView(ref Grid);
                    Grid.Focus();
                }
                else if (OptPoPen.Checked == true)
                {
                    Str3 = "Select Plan_Type, Item, Color, Size, Sum(Po_Pending)Po_Pending_Qty, Uom  from Grid_Po_Pending_Details_Material()  Group By Plan_Type, Item, Color, Size, Uom Order By PLan_Type Desc, Item, Color, Size, Uom";

                    //MyBase.Execute_Qry(Str, "RPT_DEPRECIATION_REGISTER");
                    //Str = "Select * from RPT_DEPRECIATION_REGISTER";
                    MyBase.Load_Data(Str3, ref Dt3);
                    Grid.DataSource = MyBase.V_DataTable(ref Dt3);
                   // MyBase.Grid_Designing(ref Grid, ref Dt, "Item_id", "Color_Id", "Size_Id");
                    Grid.Columns["Po_Pending_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 120, 200, 250, 200, 125, 125, 125);
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    MyBase.V_DataGridView(ref Grid);
                    Grid.Focus();
                }
                else
                {
                    Str4 = "Select Plan_Type, Buyer, Ocn, Item, Color, Size, Rate, Bom_Qty, Po_Pending, Uom, Item_ID, Color_ID, Size_Id from Grid_Po_Pending_Details_Material() where company_code = " + MyParent.CompCode + " Order By Plan_Type Desc, Buyer, Ocn, Item, Color, Size, Uom";

                    //MyBase.Execute_Qry(Str, "RPT_DEPRECIATION_REGISTER");
                    //Str = "Select * from RPT_DEPRECIATION_REGISTER";
                    MyBase.Load_Data(Str4, ref Dt4);
                    Grid.DataSource = MyBase.V_DataTable(ref Dt4);
                    MyBase.Grid_Designing(ref Grid, ref Dt4, "Item_id", "Color_Id", "Size_Id");
                    Grid.Columns["Rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns["Bom_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns["Po_Pending"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 100, 250, 110, 150, 175, 100, 110, 110, 110, 110, 110, 100);
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    MyBase.V_DataGridView(ref Grid);
                    Grid.Focus();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                OptPoPen.Checked = true;
                DTFrom.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable Dt = new DataTable();
                DataTable Dt1 = new DataTable();
                DataTable Dt2 = new DataTable();
                DataTable Dt3 = new DataTable();
                DataTable Dt4 = new DataTable();
                DataTable Dt5 = new DataTable();
                DataTable Dt6 = new DataTable();

                if (OptLot.Checked == true)
                {
                    Dt = Grid.DataSource as DataTable;
                    String FileName = MyBase.ShowSave("Lotwise Grn Register ...!", "Lotwise Grn Register On ", "XLS");
                    this.Cursor = Cursors.WaitCursor;
                    if (FileName.Trim() != String.Empty)
                    {
                        MyBase.ExportToExcel(ref Dt, FileName);
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Saved ...!", "Gainup");
                        btnCancel.PerformClick();
                    }
                }
                else if (OptAll.Checked == true)
                {
                    Dt = Grid.DataSource as DataTable;
                    String FileName = MyBase.ShowSave("Grn Pending and Completed Details ...!", "Grn Pending and Completed Details On ", "XLS");
                    this.Cursor = Cursors.WaitCursor;
                    if (FileName.Trim() != String.Empty)
                    {
                        MyBase.ExportToExcel(ref Dt, FileName);
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Saved ...!", "Gainup");
                        btnCancel.PerformClick();
                    }
                }
                else if (OptPerf.Checked == true)
                {
                    Dt = Grid.DataSource as DataTable;
                    String FileName = MyBase.ShowSave("PO wise Performance Details ...!", "PO wise Performance Details On ", "XLS");
                    this.Cursor = Cursors.WaitCursor;
                    if (FileName.Trim() != String.Empty)
                    {
                        MyBase.ExportToExcel(ref Dt, FileName);
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Saved ...!", "Gainup");
                        btnCancel.PerformClick();
                    }
                }
                else if (OptGrnPen.Checked == true)
                {
                    Dt = Grid.DataSource as DataTable;
                    String FileName = MyBase.ShowSave("Grn Pending Register ...!", "Grn Pending On ", "XLS");
                    this.Cursor = Cursors.WaitCursor;
                    if (FileName.Trim() != String.Empty)
                    {
                        MyBase.ExportToExcel(ref Dt, FileName);
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Saved ...!", "Gainup");
                        btnCancel.PerformClick();
                    }
                }
                else if (OptGrnPenOcn.Checked == true)
                {
                    Dt = Grid.DataSource as DataTable;
                    String FileName = MyBase.ShowSave("Grn Pending Register Ocnwise ...!", "Ocnwise Grn Pending On ", "XLS");
                    this.Cursor = Cursors.WaitCursor;
                    if (FileName.Trim() != String.Empty)
                    {
                        MyBase.ExportToExcel(ref Dt, FileName);
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Saved ...!", "Gainup");
                        btnCancel.PerformClick();
                    }
                }
                else if (OptPoPen.Checked == true)
                {
                    Dt = Grid.DataSource as DataTable;
                    String FileName = MyBase.ShowSave("Po Pending Register ...!", "Po Pending Details On ", "XLS");
                    this.Cursor = Cursors.WaitCursor;
                    if (FileName.Trim() != String.Empty)
                    {
                        MyBase.ExportToExcel(ref Dt, FileName);
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Saved ...!", "Gainup");
                        btnCancel.PerformClick();
                    }
                }
                else
                {
                    Dt = Grid.DataSource as DataTable;
                    String FileName = MyBase.ShowSave("Po Pending Details Ocnwise ...!", "Po Pending Details Ocnwise On ", "XLS");
                    this.Cursor = Cursors.WaitCursor;
                    if (FileName.Trim() != String.Empty)
                    {
                        MyBase.ExportToExcel(ref Dt, FileName);
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Saved ...!", "Gainup");
                        btnCancel.PerformClick();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmGridPo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //if (this.ActiveControl.Name == "TxtGroup")
                //{
                //    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}
