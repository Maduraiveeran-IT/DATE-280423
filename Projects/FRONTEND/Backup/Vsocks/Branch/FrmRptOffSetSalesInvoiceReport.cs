using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounts_ControlModules;
using SelectionTool_NmSp;

namespace Accounts
{
    public partial class FrmRptOffSetSalesInvoiceReport : Form
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        DataTable Dt = new DataTable();
        DataRow Dr;
        Int16 PCompCode;
        public FrmRptOffSetSalesInvoiceReport()
        {
            InitializeComponent();
        }         

        private void ButExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                TxtOrderNo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButReport_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            try
            {
                str = "Select Distinct Sim.EntryNo,  Sim.EntryDate,  Sty.Style, K.Company_Unit, Upper(bm.Ref_no) Order_No,sum(Sit.Quantity) As InvQty,sum(Sit.Amount)Amount, IsNull(SIA.Percentage,0) As TaxPer ";
                str = str + " From OffSet_Printing.Dbo.Sales_Inv_Mas As Sim    Inner Join OffSet_Printing.Dbo.Company As Com On Sim.CompanyId=Com.CompanyId  Inner Join OffSet_Printing.Dbo.City as CTY on CTY.Cityid = Com.Cityid    Inner Join OffSet_Printing.Dbo.Country as Coun on Coun.Countryid = Com.Countryid    Inner Join OffSet_Printing.Dbo.Sales_Inv_Desp As Sid On Sim.SalInvId=Sid.SalInvId    Inner Join OffSet_Printing.Dbo.Sales_Inv_Item As Sit On Sid.SalDespId=Sit.SalDespId    Inner Join OffSet_Printing.Dbo.Style As Sty On Sid.StyleId=Sty.StyleId    Left Outer Join OffSet_Printing.Dbo.Item As It On Sit.ItemId=It.ItemId     Inner Join OffSet_Printing.Dbo.Color As Cr On Sit.ColorId=Cr.ColorId    Inner Join OffSet_Printing.Dbo.Size As Sz On Sit.SizeId=Sz.SizeId    Inner Join OffSet_Printing.Dbo.Desp_Mas As Dm On Sid.DespatchId=Dm.DespatchMasID    Left outer Join OffSet_Printing.Dbo.Sales_Inv_Addless as SIA on SIA.SalInvId = Sim.SalInvId    Left Outer Join OffSet_Printing.Dbo.Addless as Als on Als.addlessid = SIA.addlessid    Left Outer Join OffSet_Printing.Dbo.buy_ord_mas Bm on sid.Order_No = Bm.Order_No  Left Outer Join OffSet_Printing.Dbo.job_ord_mas J on Dm.JobOrderNo = J.Order_No and J.Styleid = Dm.Styleid Left Outer Join OffSet_Printing.Dbo.company_unit K on J.supplierid = K.company_unitid Where 1 = 1 ";
                if (TxtOrderNo.Text.Trim() != String.Empty)
                {
                    str = str + " And bm.Ref_no = '" + TxtOrderNo.Text.ToString() + "' ";
                }
                if (TxtInvoiceNO.Text.Trim() != String.Empty)
                {
                    str = str + " And Sim.EntryNo= '" + TxtInvoiceNO.Text.ToString() + "' ";
                }
                str = str + " group by Sim.EntryNo,  Sim.EntryDate,  Sty.Style, K.Company_Unit, bm.Ref_no,SIA.Percentage";
                MyBase.Load_Data(str, ref Dt);
                Grid.DataSource = MyBase.V_DataTable(ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt);                
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid,100,120,150,200,100,100,150,50);
                Grid.Columns["EntryNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["EntryDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["InvQty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Taxper"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButPrint_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            try
            {
                //str = "Select Distinct Sim.EntryNo,  Sim.EntryDate,  Sty.Style, K.Company_Unit, Upper(bm.Ref_no) Order_No,sum(Sit.Quantity) As InvQty,sum(Sit.Amount)Amount, IsNull(SIA.Percentage,0) As TaxPer   ";
                //str = str + " From OffSet_Printing.Dbo.Sales_Inv_Mas As Sim    Inner Join OffSet_Printing.Dbo.Company As Com On Sim.CompanyId=Com.CompanyId  Inner Join OffSet_Printing.Dbo.City as CTY on CTY.Cityid = Com.Cityid    Inner Join OffSet_Printing.Dbo.Country as Coun on Coun.Countryid = Com.Countryid    Inner Join OffSet_Printing.Dbo.Sales_Inv_Desp As Sid On Sim.SalInvId=Sid.SalInvId    Inner Join OffSet_Printing.Dbo.Sales_Inv_Item As Sit On Sid.SalDespId=Sit.SalDespId    Inner Join OffSet_Printing.Dbo.Style As Sty On Sid.StyleId=Sty.StyleId    Left Outer Join OffSet_Printing.Dbo.Item As It On Sit.ItemId=It.ItemId     Inner Join OffSet_Printing.Dbo.Color As Cr On Sit.ColorId=Cr.ColorId    Inner Join OffSet_Printing.Dbo.Size As Sz On Sit.SizeId=Sz.SizeId    Inner Join OffSet_Printing.Dbo.Desp_Mas As Dm On Sid.DespatchId=Dm.DespatchMasID    Left outer Join OffSet_Printing.Dbo.Sales_Inv_Addless as SIA on SIA.SalInvId = Sim.SalInvId    Left Outer Join OffSet_Printing.Dbo.Addless as Als on Als.addlessid = SIA.addlessid    Left Outer Join OffSet_Printing.Dbo.buy_ord_mas Bm on sid.Order_No = Bm.Order_No  Left Outer Join OffSet_Printing.Dbo.job_ord_mas J on Dm.JobOrderNo = J.Order_No and J.Styleid = Dm.Styleid Left Outer Join OffSet_Printing.Dbo.company_unit K on J.supplierid = K.company_unitid Where 1 = 1 ";
                //if (TxtOrderNo.Text.Trim() != String.Empty)
                //{
                //    str = str + " And bm.Ref_no = '" + TxtOrderNo.Text.ToString() + "' ";
                //}
                //if (TxtInvoiceNO.Text.Trim() != String.Empty)
                //{
                //    str = str + " And Sim.EntryNo= '" + TxtInvoiceNO.Text.ToString() + "' ";
                //}
                //str = str + " group by Sim.EntryNo,  Sim.EntryDate,  Sty.Style, K.Company_Unit, bm.Ref_no,SIA.Percentage";

                str = "Select Distinct Sim.EntryNo,  Sim.EntryDate,  Sim.SalInvId,  Sim.SalesAmt,  Sim.CompanyId,Com.Company, Com.Address1, Com.Address2, Com.Address3,Com.Zipcode, Com.Phone, Com.fax, Com.e_mail,   Com.Tngst_no, Com.cst_no, isNull(Com.cst_date,'') As cst_date,  Com.TinNo, Sty.Style,Dm.DespatchNo,Dm.DespatchDate, k.Company_Unit, bm.Ref_no Order_No,Sid.StyleId,Sid.Quantity AS DespQty,Sid.DespatchId,  IsNull(It.Item,'') as Item,Cr.Color,Sz.Size,  IsNull(Sit.ItemId,0) as ItemId,Sit.ColorId,Sit.SizeId,Sit.Rate,Sit.Quantity As InvQty,  Sit.Amount, IsNull(Sia.percentage,0) As TaxPer , Sit.SecQty, Sim.Remarks,   Sim.InvType    ";
                str = str + " From OffSet_Printing.Dbo.Sales_Inv_Mas As Sim    Inner Join OffSet_Printing.Dbo.Company As Com On Sim.CompanyId=Com.CompanyId  Inner Join OffSet_Printing.Dbo.City as CTY on CTY.Cityid = Com.Cityid    Inner Join OffSet_Printing.Dbo.Country as Coun on Coun.Countryid = Com.Countryid    Inner Join OffSet_Printing.Dbo.Sales_Inv_Desp As Sid On Sim.SalInvId=Sid.SalInvId    Inner Join OffSet_Printing.Dbo.Sales_Inv_Item As Sit On Sid.SalDespId=Sit.SalDespId    Inner Join OffSet_Printing.Dbo.Style As Sty On Sid.StyleId=Sty.StyleId    Left Outer Join OffSet_Printing.Dbo.Item As It On Sit.ItemId=It.ItemId     Inner Join OffSet_Printing.Dbo.Color As Cr On Sit.ColorId=Cr.ColorId    Inner Join OffSet_Printing.Dbo.Size As Sz On Sit.SizeId=Sz.SizeId    Inner Join OffSet_Printing.Dbo.Desp_Mas As Dm On Sid.DespatchId=Dm.DespatchMasID    Left outer Join OffSet_Printing.Dbo.Sales_Inv_Addless as SIA on SIA.SalInvId = Sim.SalInvId    Left Outer Join OffSet_Printing.Dbo.Addless as Als on Als.addlessid = SIA.addlessid    Left Outer Join OffSet_Printing.Dbo.buy_ord_mas Bm on sid.Order_No = Bm.Order_No  Left Outer Join OffSet_Printing.Dbo.job_ord_mas J on Dm.JobOrderNo = J.Order_No and J.Styleid = Dm.Styleid Left Outer Join OffSet_Printing.Dbo.company_unit K on J.supplierid = K.company_unitid Where 1 = 1 ";
                if (TxtOrderNo.Text.Trim() != String.Empty)
                {
                    str = str + " And bm.Ref_no = '" + TxtOrderNo.Text.ToString() + "' ";
                }
                if (TxtInvoiceNO.Text.Trim() != String.Empty)
                {
                    str = str + " And Sim.EntryNo= '" + TxtInvoiceNO.Text.ToString() + "' ";
                }

                MyBase.Execute_Qry(str, "SalesInvRpt_Ttx");               
                CrystalDecisions.CrystalReports.Engine.ReportDocument ORpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\RptSalesInvoice.rpt");                
                //MyParent.FormulaFill(ref ORpt, "CompName", MyParent.CompName);
                //MyParent.FormulaFill(ref ORpt, "Head1", " Stock Report As On  " + String.Format("{0:dd-MMM-yyyy}", DtpTo.Value) + " ");
                //MyParent.FormulaFill(ref ORpt, "PDate", string.Format("{0:dd-MMM-yyyy} {0:T}", MyBase.GetServerDateTime ()));
                
                MyParent.CReport(ref ORpt, "STOCK REPORT..!");

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
                if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    ButReport.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmRptOffSetSalesInvoiceReport_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmRptOffSetSalesInvoiceReport_KeyDown(object sender, KeyEventArgs e)
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
                   
                    if (this.ActiveControl.Name == "TxtOrderNo")
                    {
                        String Str1;
                        Str1 = "Select Distinct Bm.Ref_no OrderNo,  Sim.EntryNo InvoiceNo  ";
                        Str1  = Str1 + " From OffSet_Printing.Dbo.Sales_Inv_Mas As Sim    Inner Join OffSet_Printing.Dbo.Company As Com On Sim.CompanyId=Com.CompanyId  Inner Join OffSet_Printing.Dbo.City as CTY on CTY.Cityid = Com.Cityid    Inner Join OffSet_Printing.Dbo.Country as Coun on Coun.Countryid = Com.Countryid    Inner Join OffSet_Printing.Dbo.Sales_Inv_Desp As Sid On Sim.SalInvId=Sid.SalInvId    Inner Join OffSet_Printing.Dbo.Sales_Inv_Item As Sit On Sid.SalDespId=Sit.SalDespId    Inner Join OffSet_Printing.Dbo.Style As Sty On Sid.StyleId=Sty.StyleId    Left Outer Join OffSet_Printing.Dbo.Item As It On Sit.ItemId=It.ItemId     Inner Join OffSet_Printing.Dbo.Color As Cr On Sit.ColorId=Cr.ColorId    Inner Join OffSet_Printing.Dbo.Size As Sz On Sit.SizeId=Sz.SizeId    Inner Join OffSet_Printing.Dbo.Desp_Mas As Dm On Sid.DespatchId=Dm.DespatchMasID    Left outer Join OffSet_Printing.Dbo.Sales_Inv_Addless as SIA on SIA.SalInvId = Sim.SalInvId    Left Outer Join OffSet_Printing.Dbo.Addless as Als on Als.addlessid = SIA.addlessid    Left Outer Join OffSet_Printing.Dbo.buy_ord_mas Bm on sid.Order_No = Bm.Order_No  Left Outer Join OffSet_Printing.Dbo.job_ord_mas J on Dm.JobOrderNo = J.Order_No and J.Styleid = Dm.Styleid  ";
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "ORDER NO", Str1 ,  string.Empty, 200,200);
                        if (Dr != null)
                        {
                            TxtOrderNo.Text = Dr["OrderNo"].ToString();
                            TxtInvoiceNO.Text = Dr["InvoiceNo"].ToString(); 
                        }
                    }                             
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

        private void FrmRptOffSetSalesInvoiceReport_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                if (MyParent.CompCode == 1)
                {
                    PCompCode = 1;
                }
                else if (MyParent.CompCode == 2)
                {
                    PCompCode = 3;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
      
    }
}