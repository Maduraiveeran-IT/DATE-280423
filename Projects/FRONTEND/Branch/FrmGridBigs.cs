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
using System.IO;

namespace Accounts
{
    public partial class FrmGridBigs : Form
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();       
        DataRow Dr;

        public FrmGridBigs()
        {
            InitializeComponent();
        }

        private void FrmGridBigs_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                TxtRep.Focus();
                TxtOcn.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmGridBigs_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtRep")
                    { 
                       btnReport.Focus();
                    }
                    SendKeys.Send("{Tab}");
                }
                if (e.KeyCode == Keys.Down)
                {
                    BtnExport.Enabled = false;                   
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Report", "Select Name, Rowid from Grid_Cost_Report  Order By Name", String.Empty, 300);
                    
                    if (Dr != null)
                    {
                        //if (Dr["Rowid"].ToString() == "54")
                        //{
                            TxtOcn.Enabled = true;
                          //  TxtOcn.Focus();
                        //}
                        TxtRep.Text = Dr["Name"].ToString();
                        TxtRep.Tag = Dr["Rowid"].ToString();
                        this.btnReport_Click(sender, e);
                        
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
        private void btnReport_Click(object sender, EventArgs e)
        {
            if (TxtRep.Text == String.Empty)
            {
                MessageBox.Show("Select Report ...!", "Gainup");
                TxtRep.Focus();
                return;
            }
            String Str = String.Empty;
            String Str1 = String.Empty;
            String Str2 = String.Empty;
            String Str3 = String.Empty;
            String Str4 = String.Empty;
            String Str5 = String.Empty;
            String Str6 = String.Empty;
            String Str7 = String.Empty;
            String Str8 = String.Empty;
            String Str9 = String.Empty;
            String Str10 = String.Empty;
            String Str11 = String.Empty;
            String Str12 = String.Empty;
            String Str13 = String.Empty;
            String Str14 = String.Empty;
            String Str15 = String.Empty;
            String Str16 = String.Empty;
            String Str17 = String.Empty;
            String Str18 = String.Empty;
            String Str19 = String.Empty;
            String Str20 = String.Empty;
            String Str21 = String.Empty;
            String Str22 = String.Empty;
            String Str23 = String.Empty;
            String Str24 = String.Empty;
            String Str25 = String.Empty;
            String Str26 = String.Empty;
            String Str27 = String.Empty;
            String Str28 = String.Empty;
            String Str29 = String.Empty;
            String Str30 = String.Empty;
            String Str31 = String.Empty;
            String Str32 = String.Empty;
            String Str33 = String.Empty;
            try
            {
                DataTable Dt = new DataTable();
                DataTable Dt1 = new DataTable();
                DataTable Dt2 = new DataTable();
                DataTable Dt3 = new DataTable();
                DataTable Dt4 = new DataTable();
                DataTable Dt5 = new DataTable();
                DataTable Dt6 = new DataTable();
                DataTable Dt7 = new DataTable();
                DataTable Dt8 = new DataTable();
                DataTable Dt9 = new DataTable();
                DataTable Dt10 = new DataTable();
                DataTable Dt11 = new DataTable();
                DataTable Dt12 = new DataTable();
                DataTable Dt13 = new DataTable();
                DataTable Dt14 = new DataTable();
                DataTable Dt15 = new DataTable();
                DataTable Dt16 = new DataTable();
                DataTable Dt17 = new DataTable();
                DataTable Dt18 = new DataTable();
                DataTable Dt19 = new DataTable();
                DataTable Dt20 = new DataTable();
                DataTable Dt21 = new DataTable();
                DataTable Dt22 = new DataTable();
                DataTable Dt23 = new DataTable();
                DataTable Dt24 = new DataTable();
                DataTable Dt25 = new DataTable();
                DataTable Dt26 = new DataTable();
                DataTable Dt27 = new DataTable();
                DataTable Dt28 = new DataTable();
                DataTable Dt29 = new DataTable();
                DataTable Dt30 = new DataTable();
                DataTable Dt31 = new DataTable();
                DataTable Dt32 = new DataTable();
                DataTable Dt33 = new DataTable();
                Grid.DataSource = null;

                if (TxtRep.Text.ToString().ToUpper() == "PROJECT ACTIVITY DETAILS")
                {
                    Str = "Select Order_No, Order_Date, Proj_Name, PArty, Employee, Proj_ACtivity_Name, RefNo, Estimate_Date, Complete_Date, Qty, Allow_Per, Conv_Qty, Rate, Amount, Remarks, Total_Qty, Total_Conv_Qty, Total_Amount, Approval_Flag, Cancel_Order, Company_Code, Complete_Order, Uom, Order_By_Slno From  Project_Order_Fn() Where company_code = "+ MyParent.CompCode +" and 1 =1 ";
                    if (TxtOcn.Text.ToString() != String.Empty)
                    {
                        Str = Str + " and Order_NO like '%" + TxtOcn.Text.ToString() + "%'";
                    }
                    Str = Str + " ORder by OrdeR_NO, Order_By_Slno ";
                    MyBase.Load_Data(Str, ref Dt);
                    Grid.DataSource = MyBase.V_DataTable(ref Dt);                    
                                    
                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 100, 80, 140, 140, 100, 140, 80, 80, 80, 100, 80, 100, 80, 100, 120, 100, 100, 100, 100, 100, 80, 80, 80, 80);
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    MyBase.V_DataGridView(ref Grid);
                    Grid.Focus();
                }
                else if (TxtRep.Text.ToString().ToUpper() == "PROJECT MATERIAL PLANNING DETAILS")
                {
                    Str = "Select Order_No, Entry_No, Effect_From, Proj_Type, Proj_ACtivity_NAme, Party, Employee, Total_Qty, Order_Date, REmarks, SNo, Item, Color, Size, Tot_Qty, Grs_Rate, Tax_Per, Other_Rate, Pur_Rate, Grs_Amount, Tax_Amount, Other_Amount, Pur_Amount, Flag, Approval_Time, Access_Type, UOM, CONS_UOM, Unit, Cons, Uom_mas, Unit_Mas, Order_By_Slno, PO_UOM, App_Pur_Rate_Conv, App_Grs_Rate_Conv from Project_Planning_Material_Fn()Where company_code = " + MyParent.CompCode + " and 1 =1 ";
                    if (TxtOcn.Text.ToString() != String.Empty)
                    {
                        Str = Str + " and Order_NO like '%" + TxtOcn.Text.ToString() + "%'";
                    }
                    Str = Str + " Order by ORdeR_NO, Order_By_Slno, Sno";
                    MyBase.Load_Data(Str, ref Dt);
                    Grid.DataSource = MyBase.V_DataTable(ref Dt);

                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 100, 80, 80, 120, 140, 120, 100, 120, 100, 120, 80, 140, 100, 100, 120, 120, 100, 100, 120, 120, 100, 80, 80, 80, 80, 100, 100, 80, 100, 80, 80, 120, 120);
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    MyBase.V_DataGridView(ref Grid);
                    Grid.Focus();
                }
                else if (TxtRep.Text.ToString().ToUpper() == "PROJECT MATERIAL PO DETAILS")
                {
                    Str = "SElect PoNo, PoDate, Indent_NO, Supplier, Order_No, PArty, Proj_ACtivity_NAme, Proj_Type,  Item, Color, Size, Order_Qty, Cancel_Qty, Grs_Rate, Tax_Per, Freight_Rate, Pur_Rate, Pur_Amount, Grs_Amount_Dtl, Tax_Amount_Dtl, Freight_Amount_Dtl, Remarks, Uom, Conv_Val, Order_Qty_Conv, Rate_Conv, Cancel_Qty_Conv, Grs_Rate_Conv, Freight_Rate_Conv, FREI_TAX_MODE, PO_QTY_CONV, Tax_Rate, Tax_Rate_Conv,  Tot_Grs_Amount, Tot_Tax_Amount, Tot_Freight_Amount, Tot_Net_Amount, RO_Amt, Uom1, Indent_Date, Remarks_Mas, Address, LedgeR_Phone  from Project_PO_Details_Fn() Where company_code = " + MyParent.CompCode + " and 1 = 1 ";
                    if (TxtOcn.Text.ToString() != String.Empty)
                    {
                        Str = Str + " and Order_NO like '%" + TxtOcn.Text.ToString() + "%'";
                    }
                    Str = Str + " ORder by PoNO, Order_NO, Proj_Activity_Name, ITem, Color, Size";
                    MyBase.Load_Data(Str, ref Dt);
                    Grid.DataSource = MyBase.V_DataTable(ref Dt);

                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 100, 80, 80, 120, 100, 120, 130, 120, 120, 120, 100, 100, 100, 100, 100, 100, 100, 120, 120, 100, 100, 100, 100, 80, 100, 100, 100, 100, 100, 100, 100);
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    MyBase.V_DataGridView(ref Grid);
                    Grid.Focus();
                }
                else if (TxtRep.Text.ToString().ToUpper() == "PROJECT MATERIAL GRN DETAILS")
                {
                    Str = "SElect GRNNo, GRNDate, Supplier, INvoice_No, GP_NO, GP_Date, DC_No, DC_Date, Invoice_Date, Order_No, Proj_ACtivity_Name, Proj_Name, Item, Color, Size, Grn_Qty, Pur_Rate, Grs_Rate, Tax_Per, Net_Amount, Gross_Amount, Tax_Amount, Country_Code, Remarks, Grs_Amount_Dtl, Tax_Amount_Dtl, Pur_Amount_Dtl  from Project_Grn_DEtails_Fn_all() Where company_code = "+MyParent.CompCode+" and 1 = 1 ";
                    if (TxtOcn.Text.ToString() != String.Empty)
                    {
                        Str = Str + " and Order_NO like '%" + TxtOcn.Text.ToString() + "%'";
                    }
                    Str = Str + " Order by GrnNo, OrdeR_NO, Proj_ACtivity_Name, ITem, Color, SIze ";
                    MyBase.Load_Data(Str, ref Dt);
                    Grid.DataSource = MyBase.V_DataTable(ref Dt);

                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 100, 80, 120, 100, 100, 100, 100, 100, 100, 120, 100, 120, 100, 100, 100, 100, 100, 100, 120, 100, 100);
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    MyBase.V_DataGridView(ref Grid);
                    Grid.Focus();
                }

                else if (TxtRep.Text.ToString().ToUpper() == "PROJECT MATERIAL INVOICING DETAILS")
                {
                    Str = "sElect Entry_No, Entry_Date, SUpplier, Invoice_No, Invoice_Date, Item, Color, Size, GRN_Qty, Rate Po_Rate, PO_Amount, Budget_Rate, Budget_Tax, Budget_Freight, Budget_Others, Bill_Qty, Bill_Rate, Bill_Amount, Freight, Others, Tax_Per, Tax_Amount, Freight1, Others1, Bill_Gross, Bill_NRate, Qty_Deb, Rate_Deb from Project_Grn_Invoicing_dEtails_Fn() Where company_code = " + MyParent.CompCode + " and 1 = 1 ";
                    if (TxtOcn.Text.ToString() != String.Empty)
                    {
                        Str = Str + " and Entry_No like '%" + TxtOcn.Text.ToString() + "%'";
                    }
                    Str = Str + " Order by Entry_No, Item, Color, Size ";
                    MyBase.Load_Data(Str, ref Dt);
                    Grid.DataSource = MyBase.V_DataTable(ref Dt);

                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 100, 80, 120, 120, 100, 120, 120, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100);
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    MyBase.V_DataGridView(ref Grid);
                    Grid.Focus();
                }
                else if (TxtRep.Text.ToString().ToUpper() == "PROJECT MATERIAL RETURN DETAILS")
                {
                    Str = "sElect REtNo, REtDate, Supplier, Item, Color, Size, REt_Qty, Grs_Rate, Tax_Per, Pur_Rate, Grs_Amount_Dtl, Tax_Amount_Dtl, Pur_Amount_Dtl, Gross_Amount, Tax_Amount, Net_Amount   from Project_Return_Item_DEtails_Fn() Where 1 = 1 ";
                    if (TxtOcn.Text.ToString() != String.Empty)
                    {
                        Str = Str + " and REtNo like '%" + TxtOcn.Text.ToString() + "%'";
                    }
                    Str = Str + " ORder by REtNo, REtDate, Item, Color, Size ";
                    MyBase.Load_Data(Str, ref Dt);
                    Grid.DataSource = MyBase.V_DataTable(ref Dt);

                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 100, 80, 120, 120, 100, 120, 120, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100);
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    MyBase.V_DataGridView(ref Grid);
                    Grid.Focus();
                }
                else if (TxtRep.Text.ToString().ToUpper() == "PROJECT MANDAYS COST DETAILS")
                {
                    Str = "sElect ENo, EDate, Project, ORdeR_NO, Description, Value, Plan_Value, Approved, Remarks  from Project_Mandays_DEtails_Fn() Where company_code = "+ MyParent.CompCode +" and 1 = 1 ";
                    if (TxtOcn.Text.ToString() != String.Empty)
                    {
                        Str = Str + " and ENo like '%" + TxtOcn.Text.ToString() + "%'";
                    }
                    Str = Str + " ORder by ENo, EDate, Project, ORdeR_NO, Description ";
                    MyBase.Load_Data(Str, ref Dt);
                    Grid.DataSource = MyBase.V_DataTable(ref Dt);

                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 100, 80, 120, 100, 100, 100, 100, 100, 120);
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    MyBase.V_DataGridView(ref Grid);
                    Grid.Focus();
                }

                BtnExport.Enabled = true;

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
                TxtRep.Text = String.Empty;
                TxtOcn.Text = String.Empty;
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
        private void SaveToCSV(DataGridView DGV, String SFileName)
        {
            
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
                DataTable Dt7 = new DataTable();
                DataTable Dt8 = new DataTable();
                DataTable Dt9 = new DataTable();
                DataTable Dt10 = new DataTable();
                DataTable Dt11 = new DataTable();
                DataTable Dt12 = new DataTable();
                DataTable Dt13 = new DataTable();
                DataTable Dt14 = new DataTable();
                DataTable Dt15 = new DataTable();
                DataTable Dt16 = new DataTable();
                DataTable Dt17 = new DataTable();
                DataTable Dt18 = new DataTable();
                DataTable Dt19 = new DataTable();
                DataTable Dt20 = new DataTable();
                DataTable Dt21 = new DataTable();
                DataTable Dt22 = new DataTable();
                DataTable Dt24 = new DataTable();
                DataTable Dt25 = new DataTable();
                DataTable Dt26 = new DataTable();
                DataTable Dt27 = new DataTable();
                DataTable Dt28 = new DataTable();
                DataTable Dt29 = new DataTable();
                DataTable Dt30 = new DataTable();
                DataTable Dt31 = new DataTable();
                DataTable Dt32 = new DataTable();
                DataTable Dt33 = new DataTable();

                if (TxtRep.Text.ToString().ToUpper() == "PROJECT ACTIVITY DETAILS")
                {
                    Dt = Grid.DataSource as DataTable;
                    String FileName = MyBase.ShowSave("Activity Details ...!", "Activity Details On ", "XLS");
                    this.Cursor = Cursors.WaitCursor;
                    if (FileName.Trim() != String.Empty)
                    {
                        MyBase.ExportToExcel(ref Dt, FileName);
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Saved ...!", "Gainup");
                        btnCancel.PerformClick();
                    }
                }
                else if (TxtRep.Text.ToString().ToUpper() == "PROJECT MATERIAL PLANNING DETAILS")
                {
                    Dt = Grid.DataSource as DataTable;
                    String FileName = MyBase.ShowSave("Planning Details ...!", "Planning Details On ", "XLS");
                    this.Cursor = Cursors.WaitCursor;
                    if (FileName.Trim() != String.Empty)
                    {
                        MyBase.ExportToExcel(ref Dt, FileName);
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Saved ...!", "Gainup");
                        btnCancel.PerformClick();
                    }
                }
                else if (TxtRep.Text.ToString().ToUpper() == "PROJECT MATERIAL PO DETAILS")
                {
                    Dt = Grid.DataSource as DataTable;
                    String FileName = MyBase.ShowSave("PO Details ...!", "PO Details On ", "XLS");
                    this.Cursor = Cursors.WaitCursor;
                    if (FileName.Trim() != String.Empty)
                    {
                        MyBase.ExportToExcel(ref Dt, FileName);
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Saved ...!", "Gainup");
                        btnCancel.PerformClick();
                    }
                }
                else if (TxtRep.Text.ToString().ToUpper() == "PROJECT MATERIAL GRN DETAILS")
                {
                    Dt = Grid.DataSource as DataTable;
                    String FileName = MyBase.ShowSave("GRN Details ...!", "GRN Details On ", "XLS");
                    this.Cursor = Cursors.WaitCursor;
                    if (FileName.Trim() != String.Empty)
                    {
                        MyBase.ExportToExcel(ref Dt, FileName);
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Saved ...!", "Gainup");
                        btnCancel.PerformClick();
                    }
                }
                else if (TxtRep.Text.ToString().ToUpper() == "PROJECT MATERIAL INVOICING DETAILS")
                {
                    Dt = Grid.DataSource as DataTable;
                    String FileName = MyBase.ShowSave("INVOICING Details ...!", "INVOICING Details On ", "XLS");
                    this.Cursor = Cursors.WaitCursor;
                    if (FileName.Trim() != String.Empty)
                    {
                        MyBase.ExportToExcel(ref Dt, FileName);
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Saved ...!", "Gainup");
                        btnCancel.PerformClick();
                    }
                }
                else if (TxtRep.Text.ToString().ToUpper() == "PROJECT MATERIAL RETURN DETAILS")
                {
                    Dt = Grid.DataSource as DataTable;
                    String FileName = MyBase.ShowSave("RETURN Details ...!", "RETURN Details On ", "XLS");
                    this.Cursor = Cursors.WaitCursor;
                    if (FileName.Trim() != String.Empty)
                    {
                        MyBase.ExportToExcel(ref Dt, FileName);
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Saved ...!", "Gainup");
                        btnCancel.PerformClick();
                    }
                }
                else if (TxtRep.Text.ToString().ToUpper() == "PROJECT MANDAYS COST DETAILS")
                {
                    Dt = Grid.DataSource as DataTable;
                    String FileName = MyBase.ShowSave("MANDAYS COST Details ...!", "MANDAYS COST Details On ", "XLS");
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

        private void FrmGridBigs_KeyPress(object sender, KeyPressEventArgs e)
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

        //private void BtnCsv_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataTable Dt = new DataTable();
        //        DataTable Dt1 = new DataTable();
        //        DataTable Dt2 = new DataTable();
        //        DataTable Dt3 = new DataTable();
        //        DataTable Dt4 = new DataTable();
        //        DataTable Dt5 = new DataTable();
        //        DataTable Dt6 = new DataTable();

        //        if (OptShip.Checked == true)
        //        {
        //            Dt = Grid.DataSource as DataTable;
        //            String FileName = "Shipment Items";
        //            SaveToCSV(Grid, FileName);
        //            MessageBox.Show("CSV Saved ...!", "Gainup");
        //            return;
        //        }
        //        else if (OptBom.Checked == true)
        //        {
        //            Dt1 = Grid.DataSource as DataTable;
        //            String FileName = "Shipment Bom";
        //            SaveToCSV(Grid, FileName);
        //            MessageBox.Show("CSV Saved ...!", "Gainup");
        //            return;
        //        }
        //        else if (OptPur.Checked == true)
        //        {
        //            Dt2 = Grid.DataSource as DataTable;
        //            String FileName = "Purchase Details";
        //            SaveToCSV(Grid, FileName);
        //            MessageBox.Show("CSV Saved ...!", "Gainup");
        //            return;
        //        }
        //        else if (OptSpl.Checked == true)
        //        {
        //            Dt3 = Grid.DataSource as DataTable;
        //            String FileName = "Spl Req Details";
        //            SaveToCSV(Grid, FileName);
        //            MessageBox.Show("CSV Saved ...!", "Gainup");
        //            return;
        //        }
        //        else if (OptDate.Checked == true)
        //        {
        //            Dt4 = Grid.DataSource as DataTable;
        //            String FileName = "Fabric History";
        //            SaveToCSV(Grid, FileName);
        //            MessageBox.Show("CSV Saved ...!", "Gainup");
        //            return;
        //        }
        //        else if (OptEff.Checked == true)
        //        {
        //            Dt5 = Grid.DataSource as DataTable;
        //            String FileName = "Efficiency Details";
        //            SaveToCSV(Grid, FileName);
        //            MessageBox.Show("CSV Saved ...!", "Gainup");
        //            return;
        //        }
        //        else
        //        {
        //            Dt6 = Grid.DataSource as DataTable;
        //            String FileName = "Issued Details";
        //            SaveToCSV(Grid, FileName);
        //            MessageBox.Show("CSV Saved ...!", "Gainup");
        //            return;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
    }
}
