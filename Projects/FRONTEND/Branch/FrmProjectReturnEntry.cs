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
    public partial class FrmProjectReturnEntry : Form,Entry  
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataTable Dt2 = new DataTable();
        DataTable Dt3 = new DataTable();
        DataGrid dgv = new DataGrid();
        DataRow Dr;        
        Int64 Code;
        Int64 Qty;
        TextBox Txt = null;        
        String[] Queries;
        String[] t;
        DataTable[] DtQty;
        TextBox Txt_Qty = null;
        Int16 PCompCode;


        public FrmProjectReturnEntry()
        {
            InitializeComponent();
        }
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                Grid_Data();           
                DtpDate.Enabled = true;
                DtQty = new DataTable[500];               
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select Dbo.[Get_Max_Project_GRN] ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "')", ref Tdt);
                TxtGrnNo.Text = Tdt.Rows[0][0].ToString();
                TabCtrl1.SelectTab(TabPageWrk);
                TxtSupplier.Focus();

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
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "GRN -Edit", "Select Distinct  GRNNo, GRNDate, Supplier, GP_NO, GP_Date, ITem,Color, SIze, Grn_Qty, Pur_Rate, Gross_Amount, Tax_Amount, Net_Amount, Dc_NO, DC_Date, INvoice_No, Invoice_Date, Remarks, Supplier_Code, Rowid  From Project_Grn_DEtails_Fn() Where Rowid Not in (Select Distinct Grn_MasterID From Project_GRN_Invoicing_Details_OCN) ORder by 1 desc", string.Empty, 100, 100, 100, 100, 100, 100, 120, 100, 80, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Total_Qty();
                    DtpDate.Enabled = false;
                    TabCtrl1.SelectTab(TabPageWrk);
                     
                   
                    //Grid.CurrentCell = Grid["CUT_NO", 0];
                    //Grid.Focus();
                    //Grid.BeginEdit(true);
                }
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
                DtQty = new DataTable[500];
                Code = Convert.ToInt64(Dr["RowId"]);
                DtpDate.Value = Convert.ToDateTime(Dr["RetDate"]);
                TxtGrnNo.Text = Dr["RetNo"].ToString();
                TxtGrnNo.Tag = Dr["RowId"].ToString();
                
                
                
                
                
                
                TxtNetAmt.Text = Dr["Net_Amount"].ToString();                
                TxtRemarks.Text = Dr["Remarks"].ToString();
                TxtSupplier.Text = Dr["Supplier"].ToString(); 
                Grid_Data();
                Load_Dt_Roll();
                TabCtrl1.SelectTab(TabPageWrk);

                TxtGrnQty.Text = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "GRN_QTY", "ITEM")))).ToString();
                TxtGrsAmt.Text = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "GRS_AMT", "ITEM")))).ToString();
                TxtTaxAmt.Text = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "TAX_AMT", "ITEM")))).ToString();
                TxtTaxAmt.Tag = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "GRN_QTY", "ITEM")))).ToString();
                TxtGrsAmt.Tag = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref GridQty, "GRN_QTY", "ACTIVITY_NAME")))).ToString();
                TxtBalQty.Text = (Convert.ToDouble(TxtTaxAmt.Tag.ToString()) - Convert.ToDouble(TxtGrsAmt.Tag.ToString())).ToString();

                
                  
                Total_Qty();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Grid_Data_Qty(Int32 Row)
        {
            try
            {
                int f = 0;
                String Str;
                if (DtQty[Row] == null)
                {
                    DtQty[Row] = new DataTable();                    
                    if (MyParent._New)
                    {
                        Str = "Select 0 SNO, '' ACTIVITY_NAME , '' ORDER_NO,  cast(" + Convert.ToDouble(Grid["GRN_QTY", Row].Value.ToString()) + " as Numeric(22,3)) GRN_QTY, CAst( " + Convert.ToDouble(Grid["GRN_QTY", Row].Value.ToString()) + " as Numeric(22,3)) BAL_QTY, 0 OrdeR_ID, 0 PO_DETAIL_ID, 0 PROJ_TYPE_ID, 0 PROJ_ACTIVITY_ID, 0 SLNO1, '' Type From Project_GRN_Pending_Ocn() WHERE 1=2";
                        MyBase.Load_Data(Str, ref DtQty[Row]);
                    }
                    else
                    {                        
                        Str = "Select A.Slno SNo, B.Proj_ACtivity_NAme ACTIVITY_NAME, B.ORdER_No, A.Qty Grn_Qty, A.Qty + B.Bal_Qty Bal_Qty, A.OrdER_ID, A.PO_Detail_ID, A.PROJ_TYPE_ID, A.PROJ_ACTIVITY_ID, A.SlNo_Dtl SlNo1 , '' Type From Project_Grn_Ocn_Details A Inner Join Project_GRN_Pending_OCn() B On A.PO_Detail_ID = B.PO_Detail_ID and A.ORdER_ID = B.Order_ID and A.Proj_Type_ID = B.Proj_Type_ID and A.Proj_Activity_ID = B.Proj_Activity_ID  Where A.MasteR_ID = " + Code + " and A.SlNo_Dtl = " + Grid["SNo", Row].Value.ToString() + "    and B.ITem_ID = " + Grid["ITem_ID", Row].Value.ToString() + " and B.Color_ID =" + Grid["Color_ID", Row].Value.ToString() + " and B.Size_ID =  " + Grid["Size_ID", Row].Value.ToString() + " and B.Pur_Rate =  " + Grid["Pur_Rate", Row].Value.ToString() + " ORder by A.SlNo ";
                        MyBase.Load_Data(Str, ref DtQty[Row]);
                    }
                    f = 1;

                }
                //if (f == 0)
                //{
                //    f = 1;
                //    return;
                //}
                    GridQty.DataSource = DtQty[Row];
                    MyBase.Grid_Designing(ref GridQty, ref DtQty[Row], "OrdER_ID", "PO_Detail_ID", "SLNO1", "PROJ_TYPE_ID", "PROJ_ACTIVITY_ID", "Type");
                    MyBase.ReadOnly_Grid_Without(ref GridQty, "ACTIVITY_NAME", "Grn_Qty");
                    MyBase.Grid_Colouring(ref GridQty, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref GridQty, 60, 150, 150, 100, 100, 120);
                    GridQty.RowHeadersWidth = 30;
                    GridQty.Columns["ORDER_NO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    GridQty.Columns["ACTIVITY_NAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    GridQty.Columns["Grn_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    GridQty.Columns["Grn_Qty"].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                    GridQty.Columns["Grn_Qty"].DefaultCellStyle.Format = "0.000";
                    GridQty.Columns["BAL_QTY"].DefaultCellStyle.Format = "0.000";
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Save()
        {
            try
            {
                Int32 Array_Index = 0;
                Total_Qty();                
                if (TxtNetAmt.Text.Trim() == string.Empty || Convert.ToDouble(TxtNetAmt.Text) == 0)
                {
                    MessageBox.Show("Invalid Total Qty ", "Gainup");
                    TxtNetAmt.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
               
                if (Convert.ToDateTime(DtpDate.Value) > MyBase.GetServerDateTime())
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DtpDate.Value = MyBase.GetServerDateTime();
                    DtpDate.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

             

                for (int i = 0; i < Grid.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Grid.Columns.Count - 1; j++)
                    {
                        if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty || Grid[j, i].Value.ToString() == "0")
                        {
                            if (Grid.Columns[j].Name.ToString() != "TAX_PER" || Grid.Columns[j].Name.ToString() != "TAX_AMT")
                            {

                            }
                            else
                            {
                                MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                TabCtrl1.SelectTab(TabPageWrk);
                                Grid.CurrentCell = Grid[j, i];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }
                    }
                    if ((Convert.ToDouble(Grid["GRN_QTY", i].Value) > Convert.ToDouble(Grid["BAL_QTY", i].Value)))
                    {
                        MessageBox.Show("GRN QTY is greater than BAL QTY in Row " + (i + 1) + " ...!", "Gainup");
                        Grid["GRN_QTY", i].Value = Grid["BAL_QTY", i].Value;
                        TabCtrl1.SelectTab(TabPageWrk);
                        Grid.CurrentCell = Grid["GRN_QTY", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                    Grid["GRS_AMT", i].Value = Convert.ToDouble(Grid["GRN_Qty", i].Value) * Convert.ToDouble(Grid["GRS_Rate", i].Value);
                    Grid["TAX_AMT", i].Value = ((Convert.ToDouble(Grid["GRS_AMT", i].Value)) * Convert.ToDouble(Grid["TAX_PER", i].Value)) / 100;
                    Grid["PUR_AMT", i].Value = Convert.ToDouble(Grid["GRN_Qty", i].Value) * Convert.ToDouble(Grid["PUR_Rate", i].Value);

                }

                Double G1 = 0;
                Double B1 = 0;
                for (int i = 0; i < Grid.Rows.Count - 1; i++)
                {
                    G1 = 0;
                    B1 = 0;
                    if (Convert.ToDouble(Grid["GRN_QTY", i].Value.ToString()) > 0)
                    {
                        G1 = Convert.ToDouble(Grid["GRN_QTY", i].Value.ToString());
                        for (int j = 0; j <= DtQty[i].Rows.Count - 1; j++)
                        {
                            B1 = B1 + Convert.ToDouble(DtQty[i].Rows[j]["Grn_Qty"].ToString());
                        }
                    }
                    if (G1 != B1)
                    {
                        MessageBox.Show("GRN QTY MisMatch " + (i + 1) + " ...!", "Gainup");                        
                        TabCtrl1.SelectTab(TabPageWrk);
                        Grid.CurrentCell = Grid["GRN_QTY", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }

                Queries = new String[Grid.Rows.Count * 6 + 40];
                if (MyParent._New)
                {
                    DataTable Tdt = new DataTable();
                    MyBase.Load_Data("Select Dbo.[Get_Max_Project_GRN] ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "')", ref Tdt);
                    TxtGrnNo.Text = Tdt.Rows[0][0].ToString();
                  
                    if(TxtGrnNo.Text.ToString() != String.Empty)
                    {
                        Queries[Array_Index++] = "Insert into Project_Grn_MAster (GRNNo, GRNDate, Supplier_Code, Gross_Amount, Qty, Net_Amount, Tax_Amount, Invoice_No, Invoice_Date, DC_No, DC_Date, GP_No, GP_Date, Remarks) Values ('" + TxtGrnNo.Text.ToString() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtSupplier.Tag.ToString() + ", " + Convert.ToDouble(TxtGrsAmt.Text.ToString()) + ", " + Convert.ToDouble(TxtGrnQty.Text.ToString()) + " , " + Convert.ToDouble(TxtNetAmt.Text.ToString()) + ",  " + Convert.ToDouble(TxtTaxAmt.Text.ToString()) + ",  '" + TxtInvoiceNo.Text.ToString() + "' , '" + String.Format("{0:dd-MMM-yyyy}", DtpInvDate.Value) + "', '" + TxtDcNo.Text.ToString() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDcdate.Value) + "', '" + TxtGpNo.Text.ToString() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpGpDate.Value) + "', '" + TxtRemarks.Text.ToString() + "') ; Select Scope_Identity()";
                        Queries[Array_Index++] = MyParent.EntryLog("PROJECT GRN", "ADD", "@@IDENTITY");
                    }
                    else
                    {
                        MessageBox.Show("Invalid Grn No", "Gainup");
                        TxtNetAmt.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                else
                {
                    Queries = new String[Grid.Rows.Count * 6 + 40]; 
                    Queries[Array_Index++] = "Update Project_Grn_MAster Set Gross_Amount = " + Convert.ToDouble(TxtGrsAmt.Text.ToString()) + ",Remarks = '" + TxtRemarks.Text + "', Qty = " + Convert.ToDouble(TxtGrnQty.Text.ToString()) + " , Net_Amount =" + Convert.ToDouble(TxtNetAmt.Text.ToString()) + " , Tax_Amount = " + Convert.ToDouble(TxtTaxAmt.Text.ToString()) + ", Invoice_No = '" + TxtInvoiceNo.Text.ToString() + "' , Invoice_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpInvDate.Value) + "', DC_No = '" + TxtDcNo.Text.ToString() + "', DC_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDcdate.Value) + "', GP_No = '" + TxtGpNo.Text.ToString() + "', GP_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpGpDate.Value) + "'  Where Rowid = " + Code;
                    Queries[Array_Index++] = "UPDate B Set Grn_Qty = GRn_Qty - A.Qty FRom Project_Grn_OCn_Details A LEft Join Project_Material_Status B On A.Proj_ACtivity_ID = B.Proj_Activity_ID and A.Proj_type_ID = B.Proj_type_ID and A.ORdeR_ID = B.ORder_ID and A.Rowid = B.Grn_OCn_DEtail_ID and A.MAster_ID = B.Grn_ID  Where Master_ID = " + Code + "";
                    Queries[Array_Index++] = "Delete From Project_Grn_Details Where Master_id = " + Code;
                    Queries[Array_Index++] = "Delete From Project_GRn_OCn_Details Where Master_id = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("PROJECT PROJECT", "EDIT", Code.ToString());
                }
                for (int i = 0; i < Grid.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Grid["GRN_QTY", i].Value.ToString()) > 0)
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into Project_Grn_Details(Master_ID, Slno, Item_ID, Color_ID, Size_ID, GRN_Qty, Grs_Amount, Tax_Amount, Pur_Amount, Grs_Rate, Tax_Per ,Pur_Rate) Values (@@IDENTITY, " + Grid["SNo", i].Value + ", " + Grid["ITem_ID", i].Value + ",  " + Grid["Color_ID", i].Value + ", " + Grid["Size_Id", i].Value + ", " + Convert.ToDouble(Grid["Grn_Qty", i].Value.ToString()) + ", " + Convert.ToDouble(Grid["Grs_Amt", i].Value.ToString()) + ", " + Convert.ToDouble(Grid["Tax_Amt", i].Value.ToString()) + ", " + Convert.ToDouble(Grid["Pur_Amt", i].Value.ToString()) + ", " + Convert.ToDouble(Grid["GRs_Rate", i].Value.ToString()) + ", " + Convert.ToDouble(Grid["Tax_Per", i].Value.ToString()) + ", " + Convert.ToDouble(Grid["Pur_Rate", i].Value.ToString()) + ")";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert into Project_Grn_Details(Master_ID, Slno, Item_ID, Color_ID, Size_ID, GRN_Qty, Grs_Amount, Tax_Amount, Pur_Amount, Grs_Rate, Tax_Per ,Pur_Rate) Values (" + Code + ", " + Grid["SNo", i].Value + ", " + Grid["ITem_ID", i].Value + ",  " + Grid["Color_ID", i].Value + ", " + Grid["Size_Id", i].Value + ", " + Convert.ToDouble(Grid["Grn_Qty", i].Value.ToString()) + ", " + Convert.ToDouble(Grid["Grs_Amt", i].Value.ToString()) + ", " + Convert.ToDouble(Grid["Tax_Amt", i].Value.ToString()) + ", " + Convert.ToDouble(Grid["Pur_Amt", i].Value.ToString()) + ", " + Convert.ToDouble(Grid["GRs_Rate", i].Value.ToString()) + ", " + Convert.ToDouble(Grid["Tax_Per", i].Value.ToString()) + ", " + Convert.ToDouble(Grid["Pur_Rate", i].Value.ToString()) + ")";
                        }
                    }
                }

                for (int i = 0; i < Grid.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Grid["GRN_QTY", i].Value.ToString()) > 0)
                    {                        
                            for (int j = 0; j <= DtQty[i].Rows.Count - 1; j++)
                            {
                                if (MyParent._New)
                                {
                                    Queries[Array_Index++] = "Insert Into Project_GRn_OCn_Details (Master_ID, Slno, PO_Detail_ID, Order_ID, Qty, Trans_Mode, Return_Qty, SlNo_Dtl, Proj_Type_ID, Proj_Activity_ID) Values (@@IDENTITY, " + DtQty[i].Rows[j]["SNo"].ToString() + ", " + DtQty[i].Rows[j]["PO_Detail_ID"].ToString() + ", " + DtQty[i].Rows[j]["Order_ID"].ToString() + ", " + DtQty[i].Rows[j]["Grn_Qty"].ToString() + ", 'Y', 0 ,  " + DtQty[i].Rows[j]["SlNo1"].ToString() + ", " + DtQty[i].Rows[j]["Proj_Type_ID"].ToString() + ", " + DtQty[i].Rows[j]["Proj_Activity_ID"].ToString() + ")"; 
                                }
                                else
                                {
                                    Queries[Array_Index++] = "Insert Into Project_GRn_OCn_Details (Master_ID, Slno, PO_Detail_ID, Order_ID, Qty, Trans_Mode, Return_Qty, SlNo_Dtl, Proj_Type_ID, Proj_Activity_ID) Values (" + Code + ", " + DtQty[i].Rows[j]["SNo"].ToString() + ", " + DtQty[i].Rows[j]["PO_Detail_ID"].ToString() + ", " + DtQty[i].Rows[j]["Order_ID"].ToString() + ", " + DtQty[i].Rows[j]["Grn_Qty"].ToString() + ", 'Y', 0 ,  " + DtQty[i].Rows[j]["SlNo1"].ToString() + ", " + DtQty[i].Rows[j]["Proj_Type_ID"].ToString() + ", " + DtQty[i].Rows[j]["Proj_Activity_ID"].ToString() + ")"; 
                                }                              
                            }                        
                    }
                    else
                    {
                        MessageBox.Show("Invalid Grn Qty...!", "Gainup");
                        MyParent.Save_Error = true;
                        return;
                    }

                }

                if (MyParent.Edit == true)
                {
                    Queries[Array_Index++] = "Exec Project_Grn_Delete_Status " + Code + "";
                    Queries[Array_Index++] = "Exec Project_Grn_Insert_Status " + Code + "";
                }

                if (MyParent._New)
                {
                    MyBase.Run_Identity(false, Queries);
                }
                else
                {
                    MyBase.Run_Identity(true, Queries);
                }

                DataTable TDt11 = new DataTable();
                String Str11 = " Select IDENT_CURRENT('Project_Grn_Master')  Identity_Mas";
                MyBase.Load_Data(Str11, ref TDt11);
                if (MyParent.Edit == true)
                {
                    MyBase.Run(" Exec Project_Grn_Delete_Status " + TDt11.Rows[0][0].ToString() + " ");
                }
                MyBase.Run(" Exec Project_Grn_Insert_Status " + TDt11.Rows[0][0].ToString() + " ");
                

                MyParent.Save_Error = false;
                MessageBox.Show("Saved ..!", "Gainup");                
            }            
            catch (Exception ex)
            {               
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Print()
        {
            try
            {
                String Str, Str1, Str2, Str3, Str4;
                String Order = "";
                DataTable Dt1 = new DataTable();
                DataTable Dt2 = new DataTable();
                DataTable Dt3 = new DataTable();
                DataTable Dt4 = new DataTable();

                Str1 = "select ITem + ' ' + Color + ' ' + Size PArticulars, Ret_Qty GRn_Qty, Grs_Rate Rate, Tax_Per, Pur_Rate , Grs_Amount_Dtl Amount, Tax_Amount_Dtl, Net_Amount from Project_Return_Item_DEtails_Fn() Where RowID = " + Code + " ";
                MyBase.Execute_Qry(Str1, "Yarn_Goods_Receipt");              
              
            
                Str4 = " Select Getdate()PrintOutDate";
                MyBase.Load_Data(Str4, ref Dt4);

                CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RptYarnReceivedDetails1.rpt");
                MyParent.FormulaFill(ref ObjRpt, "Head1", "PROJECT");
                MyParent.FormulaFill(ref ObjRpt, "Supplier", TxtSupplier.Text.ToString());
                MyParent.FormulaFill(ref ObjRpt, "GRNNo", TxtGrnNo.Text.ToString());
                MyParent.FormulaFill(ref ObjRpt, "Date", DtpDate.Value.ToString());
                if (TxtDcNo.Text.ToString() != String.Empty)
                {
                    MyParent.FormulaFill(ref ObjRpt, "DCNO", TxtDcNo.Text.ToString());
                    MyParent.FormulaFill(ref ObjRpt, "DCDate", DtpDcdate.Value.ToString());
                }
                else
                {
                    MyParent.FormulaFill(ref ObjRpt, "DCNO", TxtInvoiceNo.Text.ToString());
                    MyParent.FormulaFill(ref ObjRpt, "DCDate", DtpInvDate.Value.ToString());
                }
                //MyParent.FormulaFill(ref ObjRpt, "LOTNO", Txt_Lot.Text.ToString());
                MyParent.FormulaFill(ref ObjRpt, "InwardNo", TxtGpNo.Text.ToString());
             //  MyParent.FormulaFill(ref ObjRpt, "PoDetails", Dt2.Rows[0]["PoDetails"].ToString());
                MyParent.FormulaFill(ref ObjRpt, "PrintOutDate", Dt4.Rows[0]["PrintOutDate"].ToString());

                //if (TxtNetAmt.Tag.ToString() != "1")
                //{
                //    MyParent.FormulaFill(ref ObjRpt, "Rupee", MyBase.Rupee(Convert.ToDouble(TxtNetAmt.Text.ToString()), "Cents"));
                //    MyParent.FormulaFill(ref ObjRpt, "Currency", (("(In USD)")));
                //}
                //else
                //{
                    MyParent.FormulaFill(ref ObjRpt, "Rupee", MyBase.Rupee(Convert.ToDouble(TxtNetAmt.Text.ToString()), "Paise"));
                    MyParent.FormulaFill(ref ObjRpt, "Currency", (("(In RUPEE)")));
                //}
                MyParent.CReport(ref ObjRpt, "Fabric Purchase Goods Receipt..!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "GRN -Delete", "Select Distinct  GRNNo, GRNDate, Supplier, GP_NO, GP_Date, ITem,Color, SIze, Grn_Qty, Pur_Rate, Gross_Amount, Tax_Amount, Net_Amount, Dc_NO, DC_Date, INvoice_No, Invoice_Date, Remarks, Supplier_Code, Rowid  From Project_Grn_DEtails_Fn() Where Rowid Not in (Select Distinct Grn_MasterID From Project_GRN_Invoicing_Details_OCN) ORder by 1 desc", string.Empty, 100, 100, 100, 100, 100, 100, 120, 100, 80, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Total_Qty();
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
                    MyBase.Run("UPDate B Set Grn_Qty = Grn_Qty - A.Qty  FRom Project_Grn_OCn_Details A LEft Join Project_MAterial_Status B On A.Proj_Type_ID = B.Proj_Type_ID and A.Proj_Activity_ID = B.Proj_Activity_ID and A.ORdeR_ID = B.ORder_ID  and A.Rowid  = B.Grn_Ocn_DEtail_ID and A.MAsteR_ID = B.Grn_ID  Where Master_ID = " + Code + "", "Exec Project_Grn_Delete_Status " + Code + "", "Delete From Project_Grn_Ocn_Details Where Master_ID = " + Code, "Delete From Project_Grn_Details Where Master_ID = " + Code, "Delete From Project_Grn_MAster Where RowID = " + Code, MyParent.EntryLog("PROJECT GRN", "DELETE", Code.ToString())); 
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
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "GRN -Edit", "Select Distinct  RetNo, RetDate, Supplier, ITem,Color, SIze, Ret_Qty, Pur_Rate, Gross_Amount, Tax_Amount, Net_Amount, Remarks, Supplier_Code, Rowid  From Project_Return_DEtails_Fn() ORder by 1 desc", string.Empty, 100, 100, 100, 100, 100, 100, 120, 100, 80, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Total_Qty();                  
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void FrmProjectReturnEntry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Grid_Data()
        {
            String Str = String.Empty;
            String Str1 = String.Empty;
            String Str2 = String.Empty;
            String Str3 = String.Empty;
            String Str4 = String.Empty;    
            try
            {
                if (MyParent._New == true)
                {
                    Str = "Select 0 SNO, ITEM, COLOR,  SIZE, PO_QTY, INWARD_QTY, BAL_QTY, 0.000 GRN_QTY, GRS_RATE, 0.00 GRS_AMT, 0.00 TAX_PER, PUR_RATE, 0.00 TAX_AMT, 0.00 PUR_AMT,  ITEM_ID, COLOR_ID, SIZE_ID, (ITEM + COLOR + SIZE) DESCR  from Project_GRN_Pending() WHERE 1 = 2";
                } 
                else
                {
                    Str = "Select B.Slno SNO, B.ITEM, B.COLOR,  B.SIZE, A.PO_QTY, A.INWARD_QTY, B.GRN_QTY + A.BAL_QTY BAL_QTY,  B.GRN_QTY, B.GRS_RATE,  B.Grs_Amount_Dtl GRS_AMT,  B.TAX_PER, B.PUR_RATE, B.Tax_Amount_Dtl TAX_AMT, B.Pur_Amount_Dtl PUR_AMT, B.ITEM_ID, B.COLOR_ID, B.SIZE_ID, (B.ITEM + B.COLOR + B.SIZE) DESCR  from Project_GRN_Pending() A Inner Join Project_Grn_Item_DEtails_Fn() B On A.Item_id = B.Item_ID and A.Color_id = b.Color_ID and A.Size_ID = B.Size_ID and A.Pur_RAte = B.Pur_Rate and A.Supplier_Code = B.Supplier_Code WHERE B.RowId = " + Code + " Order by B.Slno";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);                
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid, "ITEM", "GRN_QTY");
                MyBase.Grid_Designing(ref Grid, ref Dt, "ITEM_ID", "COLOR_ID", "SIZE_ID", "DESCR");
                MyBase.Grid_Width(ref Grid, 50, 120, 110, 110, 110, 100, 100, 100, 120, 100, 100, 100);
                Grid.Columns["ITEM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;                
                Grid.Columns["COLOR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["SIZE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["PO_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["INWARD_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["BAL_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["GRN_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                MyBase.Grid_Freeze(ref Grid, Control_Modules.FreezeBY.Column_Wise, 3);
                Grid.Columns["GRN_QTY"].DefaultCellStyle.Format = "0.000";               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Load_Dt_Roll()
        {
            DataTable TempDt = new DataTable();
            try
            {
                MyBase.Load_Data("Select B.Slno SNO, B.ITEM, B.COLOR,  B.SIZE, A.PO_QTY, A.INWARD_QTY, B.GRN_QTY + A.BAL_QTY BAL_QTY,  B.GRN_QTY, B.GRS_RATE,  B.Grs_Amount_Dtl GRS_AMT,  B.TAX_PER, B.PUR_RATE, B.Tax_Amount_Dtl TAX_AMT, B.Pur_Amount_Dtl PUR_AMT, B.ITEM_ID, B.COLOR_ID, B.SIZE_ID, (B.ITEM + B.COLOR + B.SIZE) DESCR  from Project_GRN_Pending() A Inner Join Project_Grn_Item_DEtails_Fn() B On A.Item_id = B.Item_ID and A.Color_id = b.Color_ID and A.Size_ID = B.Size_ID and A.Pur_RAte = B.Pur_Rate and A.Supplier_Code = B.Supplier_Code WHERE B.RowId = " + Code + " Order by B.Slno", ref TempDt);
                        for (int i = 0; i <= TempDt.Rows.Count - 1; i++)
                        {
                            DtQty[i] = new DataTable();
                            MyBase.Load_Data("Select A.Slno SNo, B.Proj_ACtivity_NAme ACTIVITY_NAME, B.ORdER_No, A.Qty Grn_Qty, A.Qty + B.Bal_Qty Bal_Qty, A.OrdER_ID, A.PO_Detail_ID, A.PROJ_TYPE_ID, A.PROJ_ACTIVITY_ID, A.SlNo_Dtl SlNo1 , '' Type From Project_Grn_Ocn_Details A Inner Join Project_GRN_Pending_OCn() B On A.PO_Detail_ID = B.PO_Detail_ID and A.ORdER_ID = B.Order_ID and A.Proj_Type_ID = B.Proj_Type_ID and A.Proj_Activity_ID = B.Proj_Activity_ID  Where A.MasteR_ID = " + Code + " and A.SlNo_Dtl = " + TempDt.Rows[i]["SNo"].ToString() + "  and B.ITem_ID =  " + TempDt.Rows[i]["Item_ID"].ToString() + " and B.Color_ID = " + TempDt.Rows[i]["Color_ID"].ToString() + " and B.Size_ID =   " + TempDt.Rows[i]["Size_ID"].ToString() + " and B.Pur_Rate =  " + TempDt.Rows[i]["Pur_Rate"].ToString() + " ORder by A.SlNo  ", ref DtQty[i]);                            
                        }                              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void FrmProjectReturnEntry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtRemarks")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtGpNo")
                    {
                        Grid.Focus();
                        Grid.CurrentCell = Grid["ITEM", Grid.CurrentCell.RowIndex];
                        Grid.BeginEdit(true);
                        return;
                    }
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Down)
                {

                    if (this.ActiveControl.Name == "TxtSupplier")
                    {
                        if (Grid.Rows.Count <= 1 || MyParent.UserCode == 1)
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "Select Distinct L1.Ledger_Name Supplier, S1.SUpplier_Code From Project_GRN_Pending () S1 Left Join Supplier_All_Fn() L1 on S1.Supplier_Code = L1.Ledger_Code where L1.LEdgeR_Code not in (9000001) ", String.Empty, 350);
                            if (Dr != null)
                            {
                                DataTable Dts = new DataTable();
                                String St1 = "Select * From ACCOUNTS.dbo.Blocked_Ledgers_List(1) Where Ledger_Code= " + Dr["Supplier_Code"].ToString() + "";
                                MyBase.Load_Data(St1, ref Dts);
                                if (Dts.Rows.Count > 0)
                                {
                                    MessageBox.Show("This Supplier Has Been Blocked By Accounts...!");
                                    TxtSupplier.Focus();
                                    return;
                                }
                                TxtSupplier.Tag = Dr["Supplier_Code"].ToString();
                                TxtSupplier.Text = Dr["Supplier"].ToString();
                            }
                        }
                    }
                    if (this.ActiveControl.Name == "TxtGpNo")
                    {
                        if (!MyBase.Validate_Date_For_Entry(DtpGpDate.Value, 100, DtpDate.Value) && MyParent.Edit == true)
                        {
                            MessageBox.Show("Min Date Locked for this Gate Pass ...!", "Gainup");
                            TxtGpNo.Focus();
                            return;
                        }
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select GATE PASS", "Select GPNo, GPDate, Party, isnull(InvNo, '') InvNo, InvDate, Isnull(DCno, '') DCno, DCDate, Qty From [Project_Floor_Gate_Pass_Details_Pending]()", String.Empty, 100, 100, 150, 100, 100, 100, 100);
                        if (Dr != null)
                        {
                            TxtGpNo.Text = Dr["GPNo"].ToString();
                            MyBase.Lock_DatetimePicker(ref DtpGpDate, Convert.ToDateTime(Dr["GPDate"]));

                            if (Dr["InvNo"].ToString() != String.Empty)
                            {
                                TxtInvoiceNo.Text = Dr["InvNo"].ToString();
                                TxtDcNo.Text = "";
                                TxtTotQty.Text = Dr["Qty"].ToString();
                                MyBase.Lock_DatetimePicker(ref DtpInvDate, Convert.ToDateTime(Dr["InvDate"]));
                                MyBase.Lock_DatetimePicker(ref DtpDcdate, MyBase.GetServerDate());
                            }
                            else
                            {
                                TxtDcNo.Text = Dr["DCNo"].ToString();
                                TxtInvoiceNo.Text = "";
                                MyBase.Lock_DatetimePicker(ref DtpInvDate, MyBase.GetServerDate());
                                MyBase.Lock_DatetimePicker(ref DtpDcdate, Convert.ToDateTime(Dr["DCDate"]));
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

        private void Grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt == null)
                {
                    Txt = (TextBox)e.Control;
                    Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                    Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);
                    Txt.TextChanged += new EventHandler(Txt_TextChanged);
                    //  Txt.Leave +=new EventHandler(Txt_Leave);
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
                if (TxtGrnNo.Text.Trim() == String.Empty || TxtGrnNo.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Entry No ..!", "Gainup");
                    return;
                }
                else
                {
                    if (e.KeyCode == Keys.Down)
                    {
                        if (TxtSupplier.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Invalid Type", "Gainup");
                            TxtSupplier.Focus();
                            return;
                        }
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ITEM"].Index)
                        {
                            Dr = Tool.Selection_Tool_Except_New("DESCR", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "ITEM", "Select ITEM, COLOR,  SIZE, PO_QTY, INWARD_QTY, BAL_QTY, GRS_RATE, TAX_PER, PUR_RATE, GRS_AMT, TAX_AMT, PUR_AMT, ITEM_ID, COLOR_ID, SIZE_ID, (ITEM + COLOR + SIZE) DESCR  from Project_GRN_Pending() Where Supplier_Code = " + TxtSupplier.Tag + " ", string.Empty, 100, 100, 150, 100, 100, 110, 100, 100, 100, 100, 100);
                            if (Dr != null)
                            {
                                Grid["ITEM", Grid.CurrentCell.RowIndex].Value = Dr["ITEM"].ToString();
                                Grid["COLOR", Grid.CurrentCell.RowIndex].Value = Dr["COLOR"].ToString();
                                Grid["SIZE", Grid.CurrentCell.RowIndex].Value = Dr["SIZE"].ToString();
                                Grid["PO_QTY", Grid.CurrentCell.RowIndex].Value = Dr["PO_QTY"].ToString();
                                Grid["INWARD_QTY", Grid.CurrentCell.RowIndex].Value = Dr["INWARD_QTY"].ToString();
                                Grid["BAL_QTY", Grid.CurrentCell.RowIndex].Value = Dr["BAL_QTY"].ToString();
                                Grid["GRN_QTY", Grid.CurrentCell.RowIndex].Value = Dr["BAL_QTY"].ToString();
                                Grid["GRS_RATE", Grid.CurrentCell.RowIndex].Value = Dr["GRS_RATE"].ToString();
                                Grid["TAX_PER", Grid.CurrentCell.RowIndex].Value = Dr["TAX_PER"].ToString();
                                Grid["PUR_RATE", Grid.CurrentCell.RowIndex].Value = Dr["PUR_RATE"].ToString();
                                Grid["ITEM_ID", Grid.CurrentCell.RowIndex].Value = Dr["ITEM_ID"].ToString();
                                Grid["COLOR_ID", Grid.CurrentCell.RowIndex].Value = Dr["COLOR_ID"].ToString();
                                Grid["SIZE_ID", Grid.CurrentCell.RowIndex].Value = Dr["SIZE_ID"].ToString();
                                Grid["GRS_AMT", Grid.CurrentCell.RowIndex].Value = Dr["GRS_AMT"].ToString();
                                Grid["TAX_AMT", Grid.CurrentCell.RowIndex].Value = Dr["TAX_AMT"].ToString();
                                Grid["PUR_AMT", Grid.CurrentCell.RowIndex].Value = Dr["PUR_AMT"].ToString();
                                Grid["DESCR", Grid.CurrentCell.RowIndex].Value = Dr["DESCR"].ToString();
                                Txt.Text = Dr["ITEM"].ToString();
                                Grid_Data_Qty(Grid.CurrentCell.RowIndex);
                            }
                        }
                    }
                    else if (e.KeyCode == Keys.Enter)
                    {
                        //return;
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["GRN_QTY"].Index)
                        {

                            if (Convert.ToDouble(Grid["GRN_QTY", Grid.CurrentCell.RowIndex].Value) > 0)
                                {                                    
                                    e.Handled = true;
                                    TxtGrnQty.Text = Grid["GRN_QTY", Grid.CurrentCell.RowIndex].Value.ToString();
                                    TxtTaxAmt.Tag = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "GRN_QTY", "ITEM")))).ToString();
                                    TxtGrsAmt.Tag = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref GridQty, "GRN_QTY", "ACTIVITY_NAME")))).ToString();
                                    TxtBalQty.Text = (Convert.ToDouble(TxtTaxAmt.Tag.ToString()) - Convert.ToDouble(TxtGrsAmt.Tag.ToString())).ToString();
                         
                                    Grid_Data_Qty(Grid.CurrentCell.RowIndex);
                                    GridQty.Focus();
                                    GridQty.CurrentCell = GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex];
                                    GridQty.BeginEdit(true);
                                    return;
                                }                            
                        }
                    }
                }
                Total_Qty();
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        void Txt_TextChanged(object sender, EventArgs e)
        {
            try
            {               
               // Grid["KGS", Grid.CurrentCell.RowIndex].Value = String.Format("{0:0.00}", Grid["KGS", Grid.CurrentCell.RowIndex].Value);                
                Total_Qty();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Total_Qty()
        {
            Double Kgs= 0;            
            try
            {
                if (Grid.Rows.Count > 1)
                {
                    TxtGrnQty.Text = MyBase.Sum(ref Grid, "GRN_QTY", "ITEM");
                    TxtBalQty.Text = MyBase.Sum(ref Grid, "BAL_QTY", "ITEM");
                    TxtTaxAmt.Text = MyBase.Sum(ref Grid, "TAX_AMT", "ITEM");
                    TxtGrsAmt.Text = MyBase.Sum(ref Grid, "GRS_AMT", "ITEM");
                    TxtNetAmt.Text = MyBase.Sum(ref Grid, "PUR_AMT", "ITEM");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Total_Qty1()
        {
            Double Kgs = 0;
            try
            {
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["GRN_QTY"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);                    
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

        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Total_Qty();
                    TxtNetAmt.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmProjectReturnEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {
                if (this.ActiveControl.Name == String.Empty)
                {
                    MyBase.Valid_Decimal((TextBox)this.ActiveControl, e);
                }               
                else if (this.ActiveControl.Name != "TxtRemarks")
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {                
                  if (e.KeyCode == Keys.Enter)
                    {
                        //return;
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["GRN_QTY"].Index)
                        {

                            if (Convert.ToDouble(Grid["GRN_QTY", Grid.CurrentCell.RowIndex].Value) > 0)
                                {                                    
                                    e.Handled = true;
                                    TxtGrnQty.Text = Grid["GRN_QTY", Grid.CurrentCell.RowIndex].Value.ToString();
                                    TxtTaxAmt.Tag = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "GRN_QTY", "ITEM")))).ToString();
                                    TxtGrsAmt.Tag = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref GridQty, "GRN_QTY", "ACTIVITY_NAME")))).ToString();
                                    TxtBalQty.Text = (Convert.ToDouble(TxtTaxAmt.Tag.ToString()) - Convert.ToDouble(TxtGrsAmt.Tag.ToString())).ToString();
                         
                                    Grid_Data_Qty(Grid.CurrentCell.RowIndex);
                                    GridQty.Focus();
                                    GridQty.CurrentCell = GridQty["ACTIVITY_NAME", GridQty.CurrentCell.RowIndex];
                                    GridQty.BeginEdit(true);
                                    return;
                                }                            
                        }
                    }
                    Total_Qty();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtTotBales_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //MyBase.Valid_Number(TxtTotBales, e);
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
                if (Grid.Rows.Count > 1)
                {
                    MyBase.Row_Number(ref Grid);
                }
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
                if (Grid.Rows.Count > 1)
                {
                    MyBase.Row_Number(ref Grid);
                }
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
                DtQty[Grid.CurrentCell.RowIndex] = null;
                ReArrange_Datatable_Array();
                Grid.Rows.RemoveAt(Grid.CurrentCell.RowIndex);                  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }          
        }

        Boolean ReArrange_Datatable_Array()
        {
            Boolean IsAllNull = true;
            try
            {
                if (IsAllNullInDatatableArray())
                {
                    return true;
                }
                else
                {
                    for (int i = Grid.CurrentCell.RowIndex; i <= 300 - 2; i++)
                    {
                        if (DtQty[i] == null && DtQty[i + 1] != null)
                        {
                            DtQty[i] = DtQty[i + 1].Copy();
                            DtQty[i + 1] = null;
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        Boolean IsAllNullInDatatableArray()
        {
            try
            {
                for (int i = Grid.CurrentCell.RowIndex + 1; i <= 300 - 1; i++)
                {
                    if (DtQty[Grid.CurrentCell.RowIndex + 1] != null)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private void GridQty_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Qty == null)
                {
                    Txt_Qty = (TextBox)e.Control;
                    Txt_Qty.KeyDown += new KeyEventHandler(Txt_Qty_KeyDown);
                    Txt_Qty.KeyPress += new KeyPressEventHandler(Txt_Qty_KeyPress);
                    Txt_Qty.TextChanged += new EventHandler(Txt_Qty_TextChanged);
                    Txt_Qty.Leave += new EventHandler(Txt_Qty_Leave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Qty_Leave(object sender, EventArgs e)
        {
            try
            {
                if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["GRN_QTY"].Index && Txt_Qty.Text.ToString() != String.Empty)
                {
                    GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex].Value = Txt_Qty.Text.ToString();
                    if (GridQty["ACTIVITY_NAME", GridQty.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        if (Convert.ToDouble(GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex].Value) > Convert.ToDouble(TxtGrnQty.Text.ToString()))
                        {
                            //MessageBox.Show("Invalid Iss Kgs, Target Level Exceed ", "Gainup");
                            //GridQty["ISS_KGS", GridQty.CurrentCell.RowIndex].Value = Convert.ToDouble(TxtTotTarg.Text.ToString());
                            //GridQty.CurrentCell = GridQty["ISS_KGS", GridQty.CurrentCell.RowIndex];
                            //GridQty.Focus();
                            //GridQty.BeginEdit(true);
                            return;
                        }
                        else if (Convert.ToDouble(GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex].Value) > Convert.ToDouble(GridQty["BAL_QTY", GridQty.CurrentCell.RowIndex].Value))
                        {
                            MessageBox.Show("Invalid GRN_QTY are Greater than BAL_QTY ", "Gainup");
                            GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex].Value = GridQty["BAL_QTY", GridQty.CurrentCell.RowIndex].Value;
                            GridQty.CurrentCell = GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex];
                            GridQty.Focus();
                            GridQty.BeginEdit(true);
                            return;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Invalid ACTIVITY ", "Gainup");
                        GridQty.CurrentCell = GridQty["ACTIVITY_NAME", GridQty.CurrentCell.RowIndex];
                        GridQty.Focus();
                        GridQty.BeginEdit(true);
                        return;
                    }
                }
                TxtGrsAmt.Text = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "GRS_AMT", "ITEM")))).ToString();
                TxtTaxAmt.Text = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "TAX_AMT", "ITEM")))).ToString();
                TxtTaxAmt.Tag = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "GRN_QTY", "ITEM")))).ToString();
                TxtGrsAmt.Tag = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref GridQty, "GRN_QTY", "ACTIVITY_NAME")))).ToString();
                TxtBalQty.Text = (Convert.ToDouble(TxtTaxAmt.Tag.ToString()) - Convert.ToDouble(TxtGrsAmt.Tag.ToString())).ToString();
          
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Qty_TextChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        void Txt_Qty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["GRN_QTY"].Index)
                {
                    MyBase.Valid_Decimal(Txt_Qty, e);
                }
                else
                {
                    MyBase.Valid_Null(Txt_Qty, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void Txt_Qty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                String Strq = "";
                if (e.KeyCode == Keys.Down)
                {
                    if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["ACTIVITY_NAME"].Index)
                    {
                        //Select 0 SNO, '' ACTIVITY_NAME , '' ORDER_NO,  cast(" + Convert.ToDouble(Grid["GRN_QTY", Row].Value.ToString()) + " as Numeric(22,3)) GRN_QTY, CAst( " + Convert.ToDouble(Grid["GRN_QTY", Row].Value.ToString()) + " as Numeric(22,3)) BAL_QTY, 0 OrdeR_ID, 0 PO_DETAIL_ID, 0 PROJ_TYPE_ID, 0 PROJ_ACTIVITY_ID, 0 SLNO1, '' Type From Project_GRN_Pending_Ocn() 
                        Strq = "Select PROJ_ACTIVITY_NAME ACTIVITY_NAME, ORDER_NO, BAL_QTY GRN_QTY, BAL_QTY, OrdeR_ID, PO_DETAIL_ID, PROJ_TYPE_ID, PROJ_ACTIVITY_ID, " + Grid["SNO", Grid.CurrentCell.RowIndex].Value.ToString() + " SLNO1   From Project_GRN_Pending_Ocn() Where  Item_ID = " + Grid["Item_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Color_Id = " + Grid["Color_Id", Grid.CurrentCell.RowIndex].Value.ToString() + " and SizE_Id = " + Grid["SizE_Id", Grid.CurrentCell.RowIndex].Value.ToString() + "  and Supplier_Code = " + TxtSupplier.Tag.ToString() + " and Pur_RATe =  " + Grid["Pur_RATe", Grid.CurrentCell.RowIndex].Value.ToString() + "   ";
                        Dr = Tool.Selection_Tool_Except_New_Resize("PO_DETAIL_ID", this, 100, 100, ref DtQty[Grid.CurrentCell.RowIndex], SelectionTool_Class.ViewType.NormalView, "Select OCN...!", Strq, String.Empty, 100, 150, 100, 80, 100, 100, 100);
                        if (Dr != null)
                        {
                            Txt_Qty.Text = Dr["ACTIVITY_NAME"].ToString();
                            GridQty["ACTIVITY_NAME", GridQty.CurrentCell.RowIndex].Value = Dr["ACTIVITY_NAME"].ToString();
                            GridQty["ORDER_NO", GridQty.CurrentCell.RowIndex].Value = Dr["ORDER_NO"].ToString();                            
                            GridQty["BAL_QTY", GridQty.CurrentCell.RowIndex].Value = Dr["BAL_QTY"].ToString();
                            GridQty["OrdeR_ID", GridQty.CurrentCell.RowIndex].Value = Dr["OrdeR_ID"].ToString();
                            GridQty["PO_DETAIL_ID", GridQty.CurrentCell.RowIndex].Value = Dr["PO_DETAIL_ID"].ToString();
                            GridQty["PROJ_TYPE_ID", GridQty.CurrentCell.RowIndex].Value = Dr["PROJ_TYPE_ID"].ToString();
                            GridQty["PROJ_ACTIVITY_ID", GridQty.CurrentCell.RowIndex].Value = Dr["PROJ_ACTIVITY_ID"].ToString();
                            GridQty["SLNO1", GridQty.CurrentCell.RowIndex].Value = Dr["SLNO1"].ToString();                                                        
                            TxtGrsAmt.Text = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "GRS_AMT", "ITEM")))).ToString();
                            TxtTaxAmt.Text = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "TAX_AMT", "ITEM")))).ToString();
                            TxtTaxAmt.Tag = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "GRN_QTY", "ITEM")))).ToString();
                            TxtGrsAmt.Tag = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref GridQty, "GRN_QTY", "ACTIVITY_NAME")))).ToString();
                            TxtBalQty.Text =  (Convert.ToDouble(TxtTaxAmt.Tag.ToString()) - Convert.ToDouble(TxtGrsAmt.Tag.ToString())).ToString();
                            if (Convert.ToDouble(TxtBalQty.Text.ToString()) <= Convert.ToDouble(Dr["GRN_QTY"].ToString()))
                            {
                                GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex].Value = TxtBalQty.Text.ToString();                             
                            }
                            else
                            {
                                GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex].Value = Dr["GRN_QTY"].ToString();                               
                            }
                            //GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex].Value = Dr["GRN_QTY"].ToString();        

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void DtpEDate_Leave(object sender, EventArgs e)
        {
             try
            {
                if (Convert.ToDateTime(DtpDate.Value) > MyBase.GetServerDateTime())
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DtpDate.Value = MyBase.GetServerDateTime();
                    DtpDate.Focus();
                    return;
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["GRN_QTY"].Index)
                {
                    if (Grid["GRN_QTY", Grid.CurrentCell.RowIndex].Value == null || Grid["GRN_QTY", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["GRN_QTY", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["GRN_QTY", Grid.CurrentCell.RowIndex].Value = "0.000";
                        MessageBox.Show("Invalid GRN_QTY ...!", "Gainup");
                        TabCtrl1.SelectTab(TabPageWrk);
                        Grid.CurrentCell = Grid["GRN_QTY", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                    }
                    else
                    {

                        Grid["GRS_AMT", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["GRS_Rate", Grid.CurrentCell.RowIndex].Value);
                        Grid["TAX_AMT", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["GRS_AMT", Grid.CurrentCell.RowIndex].Value)) * Convert.ToDouble(Grid["TAX_PER", Grid.CurrentCell.RowIndex].Value)) / 100;
                        Grid["PUR_AMT", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["PUR_Rate", Grid.CurrentCell.RowIndex].Value);

                            Total_Qty();
                        
                    }
                }
               
            }            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TabCtrl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (TabCtrl1.SelectedTab.Text.ToString() == "GRN")
                {
                    Grid.AllowUserToAddRows = true;
                    if (Grid.Rows.Count > 0)
                    {
                        Grid.CurrentCell = Grid["ITEM", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                    }
                    return;
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtTotQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void GridQty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["GRN_QTY"].Index)
                    {
                        if (GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            if ((Convert.ToDouble(GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex].Value) == 0) || (GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex].Value == DBNull.Value))
                            {
                                e.Handled = true;
                                MessageBox.Show("Invalid GRN_QTY...!", "Gainup");
                                GridQty.Focus();
                                GridQty.CurrentCell = GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex];
                                GridQty.BeginEdit(true);
                                return;
                            }

                            if (Convert.ToDouble(TxtGrnQty.Text.ToString()) <= 0)
                            {
                                e.Handled = true;
                                MessageBox.Show("Invalid GRN_QTY ...!", "Gainup");
                                GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex].Value = 0;
                                GridQty.Focus();
                                GridQty.CurrentCell = GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex];
                                GridQty.BeginEdit(true);
                                return;
                            }
                        }
                    }

                    
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    //GBProc.Visible = false;
                    GridQty.Enabled = true;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Grid.CurrentCell = Grid["ITEM", Grid.CurrentCell.RowIndex + 1];
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

        private void GridQty_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (GridQty.Rows.Count >1)                
                {
                    MyBase.Row_Number(ref GridQty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridQty_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref GridQty, ref DtQty[Grid.CurrentCell.RowIndex], GridQty.CurrentCell.RowIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridQty_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
             try
            {
                if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["GRN_QTY"].Index)
                {
                    if (GridQty["ACTIVITY_NAME", GridQty.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {

                        if (Convert.ToDouble(GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex].Value) > Convert.ToDouble(GridQty["BAL_QTY", GridQty.CurrentCell.RowIndex].Value))
                        {
                            MessageBox.Show("Grn Qty are Greater than Bal Qty", "Gainup");
                            GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex].Value = GridQty["BAL_QTY", GridQty.CurrentCell.RowIndex].Value;
                            GridQty.CurrentCell = GridQty["GRN_QTY", GridQty.CurrentCell.RowIndex];
                            GridQty.Focus();
                            GridQty.BeginEdit(true);
                            return;
                        }

                    }
                    else
                    {
                      //  MessageBox.Show("Invalid GRN_NO ", "Gainup");
                        //GridQty.CurrentCell = GridQty["ISS_KGS", GridQty.CurrentCell.RowIndex];
                        //GridQty.Focus();
                        //GridQty.BeginEdit(true);
                        return;
                    }
                }
                if (TxtGrnQty.Text.ToString() != String.Empty)
                {
                    TxtGrsAmt.Text = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "GRS_AMT", "ITEM")))).ToString();
                    TxtTaxAmt.Text = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "TAX_AMT", "ITEM")))).ToString();
                    TxtTaxAmt.Tag = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "GRN_QTY", "ITEM")))).ToString();
                    TxtGrsAmt.Tag = Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref GridQty, "GRN_QTY", "ACTIVITY_NAME")))).ToString();
                    TxtBalQty.Text = (Convert.ToDouble(TxtTaxAmt.Tag.ToString()) - Convert.ToDouble(TxtGrsAmt.Tag.ToString())).ToString();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid.Rows.Count >= 2 && Grid.CurrentCell != null && Grid.CurrentCell.Value != DBNull.Value)
                {
                    
                    if (Grid["ITEM", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        Grid_Data_Qty(Grid.CurrentCell.RowIndex);
                        // Load_ROLL(Grid.CurrentCell.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Grid["ITEM", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    TxtGrnQty.Text = Grid["GRN_QTY", Grid.CurrentCell.RowIndex].Value.ToString();
                    //TxtTotBal.Text = Convert.ToString((Convert.ToDouble(TxtTotTarg.Text.ToString()) - Convert.ToDouble(String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum_With_Digits(ref GridQty, "ISS_KGS", 3))))));
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //void Load_ROLL(Int32 Row)
        //{                         
        //    try
        //    {

        //        if (Dt.Rows.Count == 0 && Grid["CUT_NO", 0].Value == DBNull.Value)
        //        {
        //            return;
        //        }
        //        if (DtQty[Row] == null)
        //        {                    
        //            DtQty[Row] = new DataTable();
        //            MyBase.Load_Data("Select 0 SNO, '' GRNNO , '' ORDER_NO, '' LOT_NO, '' ROLL_NO, " + Grid["ISS_KGS", Row].Value.ToString() + " ISS_KGS, " + Grid["ISS_KGS", Row].Value.ToString() + " BAL_KGS, 0 CutOrdReq_ID, 0 Grn_Lot_Rowid, 0 SLNO1 From Gloves_FAbric_RollWise_Stock_Issue_Fn() WHERE 1=2", ref DtQty[Row]);
        //            GridQty.DataSource = DtQty[Grid.CurrentCell.RowIndex];
        //            GridQty.DataSource = DtQty[Row];
        //            MyBase.Grid_Designing(ref GridQty, ref DtQty[Row], "Grn_Lot_Rowid", "CutOrdReq_ID", "SLNO1");
        //            MyBase.ReadOnly_Grid_Without(ref GridQty, "GRNNO", "ISS_KGS");
        //            MyBase.Grid_Colouring(ref GridQty, Control_Modules.Grid_Design_Mode.Column_Wise);
        //            MyBase.Grid_Width(ref GridQty, 60, 150, 150, 100, 100, 120);
        //            GridQty.RowHeadersWidth = 30;
        //            GridQty.Columns["GRNNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        //            GridQty.Columns["ORDER_NO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        //            GridQty.Columns["LOT_NO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        //            GridQty.Columns["ISS_KGS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //            GridQty.Columns["ISS_KGS"].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
        //            //if (Fill_Flag)
        //            //{
        //            //    Fill();
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}



    }
}

