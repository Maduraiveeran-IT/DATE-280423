using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Accounts
{
    public partial class FrmProjectPlanningEntry : Form,Entry  
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataTable Dt2 = new DataTable();
        DataTable Dt3 = new DataTable();
        DataTable Dt4 = new DataTable();
        DataTable Dt5 = new DataTable();
        DataTable Dtm = new DataTable();
        DataTable[] DtTrims;
        DataTable[] DtQty;
        DataGrid dgv = new DataGrid();
        DataRow Dr;        
        Int64 Code;
        Int32 C=0;
        Boolean Status_Flag = false;
        TextBox Txt = null;        
        TextBox Txt_Qty = null;
        SqlTransaction Trans = null;
        String Body1;
        String MailID_S = "", MailID_SCC = "";
        Int16 T = 0;
        TextBox Txt_Trim = null;   
        TextBox Txt_Proc = null;
        TextBox Txt_Comm = null;  
        TextBox Txt_Spcl = null;
        TextBox Txt_Fgs = null;
        DataTable[] DtImg;
        String[] Queries;
        String Str;             
        String Str1;     
        String Str2;
        String Str3; 
        String Str4, Str5;
        Double TrimBomQty = 0;
        public FrmProjectPlanningEntry()
        {
            InitializeComponent();
        }
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);  
                DtQty = new DataTable[300];
                ChkCopy.Enabled = true;
             TxtTrimCost.Text = " "; TxtProcCost.Text = " "; TxtCommCost.Text = " ";  TxtProfit.Text = " "; TxtTotalCost.Text = " "; TxtPackCost.Text = " "; TxtExRate.Text = " ";  TxtIndRs.Text = " "; TxtExpRs.Text = " "; TxtSalePriceInd.Text = " "; TxtDBInd.Text = " "; TxtProfitInd.Text = " "; TxtValueInd.Text = " "; TxtSalePriceExp.Text = " "; TxtDBExp.Text = " "; TxtProfitExp.Text = " "; TxtValueExp.Text= " ";  
               // DtpDate.Value = MyBase.GetServerDate();
                DtTrims= new DataTable[300];
                ChkCopy.Checked = false;
                Grid_Data();                
                DataTable TDt = new DataTable();
                MyBase.Load_Data("Select IsNull(Max(Entry_No),0)+1 Entry_No From Project_Planning_Master Where Company_Code = " + MyParent.CompCode + " ", ref TDt);
                TxtENo.Text  = TDt.Rows[0][0].ToString();
                TxtProjNo.Focus();
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
                ChkCopy.Enabled = false;
                 DtQty = new DataTable[300];
                TxtTrimCost.Text = " "; TxtProcCost.Text = " "; TxtCommCost.Text = " "; TxtProfit.Text = " "; TxtTotalCost.Text = " "; TxtPackCost.Text = " "; TxtExRate.Text = " ";  TxtIndRs.Text = " "; TxtExpRs.Text = " "; TxtSalePriceInd.Text = " "; TxtDBInd.Text = " "; TxtProfitInd.Text = " "; TxtValueInd.Text = " "; TxtSalePriceExp.Text = " "; TxtDBExp.Text = " "; TxtProfitExp.Text = " "; TxtValueExp.Text= " ";
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Planning - Edit", "Select Distinct A.Order_No, A.Entry_No,  A.Effect_From, A.Proj_Type, A.Proj_ACtivity_NAme, A.Party, A.Employee, A.UOM_MAS, A.UNIT_MAS, A.BOM_Qty, A.Qty  , A.Total_Qty, A.OrdeR_ID, A.Proj_ACtivity_ID, A.UOM_ID_Mas, A.Proj_Type_ID, A.Order_Date , A.REmarks, A.Party_Code, A.Amount, A.RowID From Project_Planning_Material_Fn() A  Left Join Project_Order_Master D On A.Order_No = D.Order_No   Where D.Company_Code = " + MyParent.CompCode + " and D.Complete_Order = 'N' and D.Cancel_Order = 'N'   and D.PArty_Code = " + MyParent.Proj_Login_Code + " Order by A.Order_No desc ", String.Empty, 150, 80, 100, 100, 140, 100, 100, 100, 100, 100); 
                if (Dr != null)
                {
                    Fill_Datas(Dr);                                        
                    tabControl1.SelectTab(tabPage2);
                    GridTrim.CurrentCell = GridTrim["ITEM", 0];
                    GridTrim.Focus();
                    GridTrim.BeginEdit(true);
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
                Code = Convert.ToInt64(Dr["RowID"]);               
                TxtENo.Text = Dr["EntrY_No"].ToString();                
                TxtENo.Tag = Dr["RowID"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["Effect_From"]);
                TxtProjNo.Text = Dr["ORder_No"].ToString();  
                TxtProjNo.Tag= Dr["OrdeR_ID"].ToString();  
                TxtProjName.Text =  Dr["Proj_Type"].ToString();  
                TxtProjName.Tag = Dr["PRoj_Type_ID"].ToString();  
                DtpODate.Value = Convert.ToDateTime(Dr["Order_Date"]);
                TxtBuyer.Text = Dr["PArty"].ToString();
                TxtBuyer.Tag = Dr["PArty_Code"].ToString();
                TxtQty.Text = Dr["UNIT_MAS"].ToString();
                TxtUom.Text = Dr["UOM_MAS"].ToString();
                TxtUom.Tag = Dr["UOM_ID_Mas"].ToString(); 
                TxtActivity.Text = Dr["Proj_Activity_Name"].ToString();
                TxtActivity.Tag = Dr["Proj_Activity_ID"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();                             
                TxtTotTrimQty.Text = Dr["Total_Qty"].ToString();
                TxtExRate.Text  = "1";
                TxtCurrency.Text = "INR";
                Grid_Data();
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Calculate_Item()
        {
            try
            {
                for (int i = 0; i < GridTrim.Rows.Count - 1; i++)
                {
                    if (GridTrim["ITEM", i].Value == null)
                    {
                        return;
                    }

                    if (GridTrim["ITEM", i].Value.ToString() != String.Empty && GridTrim["ITEM_ID", i].Value.ToString() != String.Empty && GridTrim["UOM", i].Value.ToString() != GridTrim["PO_UOM", i].Value.ToString() && Convert.ToDouble(GridTrim["GRS_RATE", i].Value.ToString()) > 0)
                    {
                        if (GridTrim["CONV_VAL", i].Value == null || GridTrim["CONV_VAL", i].Value == DBNull.Value || GridTrim["CONV_VAL", i].Value.ToString() == String.Empty || Convert.ToDouble(GridTrim["CONV_VAL", i].Value.ToString()) == 0)
                        {
                            GridTrim["CONV_VAL", i].Value = "1.000";
                        }

                        if (GridTrim["CALC_TYPE", i].Value.ToString() == "*")
                        {
                            GridTrim["TOT_QTY_CONV", i].Value = String.Format("{0:0.000}", Math.Round(Convert.ToDouble(GridTrim["TOT_QTY", i].Value) * Convert.ToDouble(GridTrim["CONV_VAL", i].Value), 3));
                            GridTrim["GRS_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(GridTrim["GRS_RATE", i].Value) / Convert.ToDouble(GridTrim["CONV_VAL", i].Value), 4));
                            GridTrim["OTHER_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(GridTrim["OTHER_RATE", i].Value) / Convert.ToDouble(GridTrim["CONV_VAL", i].Value), 4));
                            GridTrim["PUR_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(GridTrim["PUR_RATE", i].Value) / Convert.ToDouble(GridTrim["CONV_VAL", i].Value), 4));
                        }
                        else
                        {
                            GridTrim["TOT_QTY_CONV", i].Value = String.Format("{0:0.000}", Math.Round(Convert.ToDouble(GridTrim["TOT_QTY", i].Value) / Convert.ToDouble(GridTrim["CONV_VAL", i].Value), 3));
                            GridTrim["GRS_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(GridTrim["GRS_RATE", i].Value) * Convert.ToDouble(GridTrim["CONV_VAL", i].Value), 4));
                            GridTrim["OTHER_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(GridTrim["OTHER_RATE", i].Value) * Convert.ToDouble(GridTrim["CONV_VAL", i].Value), 4));
                            GridTrim["PUR_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(GridTrim["PUR_RATE", i].Value) * Convert.ToDouble(GridTrim["CONV_VAL", i].Value), 4));
                        }
                    }
                    else
                    {
                        //GridTrim["TOT_QTY_CONV", i].Value = String.Format("{0:0.000}", 0);
                        //GridTrim["GRS_RATE_CONV", i].Value = String.Format("{0:0.0000}", 0);
                        //GridTrim["OTHER_RATE_CONV", i].Value = String.Format("{0:0.0000}",0);
                        //GridTrim["PUR_RATE_CONV", i].Value = String.Format("{0:0.0000}", 0);

                        GridTrim["TOT_QTY_CONV", i].Value = String.Format("{0:0.000}", Math.Round(Convert.ToDouble(GridTrim["TOT_QTY", i].Value) / Convert.ToDouble(GridTrim["CONV_VAL", i].Value), 3));
                        GridTrim["GRS_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(GridTrim["GRS_RATE", i].Value) * Convert.ToDouble(GridTrim["CONV_VAL", i].Value), 4));
                        GridTrim["OTHER_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(GridTrim["OTHER_RATE", i].Value) * Convert.ToDouble(GridTrim["CONV_VAL", i].Value), 4));
                        GridTrim["PUR_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(GridTrim["PUR_RATE", i].Value) * Convert.ToDouble(GridTrim["CONV_VAL", i].Value), 4));

                    }
                }
                Total_Count();
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
                Total_Count();
                Total_Cost_Calc();              
                if (TxtProjName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Name", "Gainup");
                    TxtProjName.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtProjNo.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Order No", "Gainup");
                    TxtProjNo.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtBuyer.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Buyer", "Gainup");
                    TxtBuyer.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtRemarks.Text.Trim() == string.Empty)
                {                    
                    TxtRemarks.Text  = "-";
                }
                
               
                
                for (int i = 0; i < GridTrim.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < GridTrim.Columns.Count - 7; j++)
                    {
                        if (GridTrim[j, i].Value == DBNull.Value || GridTrim[j, i].Value.ToString() == String.Empty || GridTrim[j, i].Value.ToString() == "0")
                        {
                            if(GridTrim["ACCESS_TYPE", i].Value.ToString() == "GENERAL")
                            {
                                if (GridTrim.Columns[j].Name.ToString() == "REMARKS" || GridTrim.Columns[j].Name.ToString() == "APP_PUR_RATE_CONV" || GridTrim.Columns[j].Name.ToString() == "TAX_PER" || GridTrim.Columns[j].Name.ToString() == "TAX_AMOUNT" || GridTrim.Columns[j].Name.ToString() == "OTHER_AMOUNT" || GridTrim.Columns[j].Name.ToString() == "OTHER_RATE" || GridTrim.Columns[j].Name.ToString() == "OTHER_RATE_CONV")
                                {

                                }
                                else
                                {
                                tabControl1.SelectTab(tabPage2);
                                MessageBox.Show("' " + GridTrim.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                GridTrim.CurrentCell = GridTrim[j, i];
                                GridTrim.Focus();
                                GridTrim.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                                }
                            }
                        }                      
                    }

                    if (GridTrim["TAX_PER", i].Value.ToString() == String.Empty)
                    {
                        GridTrim["TAX_PER", i].Value = 0.00;
                    }

                    if (Convert.ToDouble(GridTrim["UNIT", i].Value) == 0 || (GridTrim["UNIT", i].Value.ToString()) == String.Empty || Convert.ToDouble(GridTrim["CONS", i].Value) == 0 || (GridTrim["CONS", i].Value.ToString()) == String.Empty)
                    {
                        tabControl1.SelectTab(tabPage2);
                        MessageBox.Show("Invalid Unit & Cons..!", "Gainup");
                        GridTrim.CurrentCell = GridTrim[1, i];
                        GridTrim.Focus();
                        GridTrim.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }

                    else if (Convert.ToDouble(GridTrim["UNIT", i].Value) > 0)
                    {
                        GridTrim["TOT_QTY", i].Value = Convert.ToDouble(GridTrim["UNIT", i].Value) * Math.Round(Convert.ToDouble(GridTrim["CONS", i].Value), 5);
                    }

                    if (Convert.ToDouble(GridTrim["Rate", i].Value) == 0 || (GridTrim["Rate", i].Value.ToString()) == String.Empty || Convert.ToDouble(GridTrim["GRS_AMOUNT", i].Value) == 0 || (GridTrim["GRS_AMOUNT", i].Value.ToString()) == String.Empty)
                    {
                        tabControl1.SelectTab(tabPage2);
                        MessageBox.Show("Invalid Trims Rate & Amount..!", "Gainup");
                        GridTrim.CurrentCell = GridTrim[1, i];
                        GridTrim.Focus();
                        GridTrim.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                    else
                    {
                        GridTrim["GRS_RATE", i].Value = Convert.ToDouble(GridTrim["Ex_Rate", i].Value) * Math.Round(Convert.ToDouble(GridTrim["Rate", i].Value), 5);
                    }

                    if (Convert.ToDouble(GridTrim["GRS_RATE", i].Value) == 0 || (GridTrim["GRS_RATE", i].Value.ToString()) == String.Empty || Convert.ToDouble(GridTrim["GRS_AMOUNT", i].Value) == 0 || (GridTrim["GRS_AMOUNT", i].Value.ToString()) == String.Empty)
                    {
                            tabControl1.SelectTab(tabPage2);
                            MessageBox.Show("Invalid Trims Rate & Amount..!", "Gainup");
                            GridTrim.CurrentCell = GridTrim[1, i];
                            GridTrim.Focus();
                            GridTrim.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                    }
                    else
                    {
                         GridTrim["GRS_AMOUNT", i].Value = Convert.ToDouble(GridTrim["TOT_QTY", i].Value) * Math.Round(Convert.ToDouble(GridTrim["GRS_RATE", i].Value),5);
                    }

                    GridTrim["TAX_AMOUNT", i].Value = ((Convert.ToDouble(GridTrim["GRS_AMOUNT", i].Value)) * Convert.ToDouble(GridTrim["TAX_PER", i].Value)) / 100;
                    GridTrim["OTHER_AMOUNT", i].Value = (Convert.ToDouble(GridTrim["TOT_QTY", i].Value) * ((Convert.ToDouble(GridTrim["OTHER_RATE", i].Value))));
                    GridTrim["PUR_AMOUNT", i].Value = ((Convert.ToDouble(GridTrim["GRS_AMOUNT", i].Value) + Convert.ToDouble(GridTrim["TAX_AMOUNT", i].Value) + Convert.ToDouble(GridTrim["OTHER_AMOUNT", i].Value)));
                    GridTrim["PUR_RATE", i].Value = Convert.ToDouble(GridTrim["PUR_AMOUNT", i].Value) / (Convert.ToDouble(GridTrim["TOT_QTY", i].Value));                   
                }

                for (int i = 0; i < GridProc.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < GridProc.Columns.Count - 7; j++)
                    {
                        if (GridProc[j, i].Value == DBNull.Value || GridProc[j, i].Value.ToString() == String.Empty || GridProc[j, i].Value.ToString() == "0")
                        {
                            if (GridProc.Columns[j].Name.ToString() == "TAX_PER" || GridProc.Columns[j].Name.ToString() == "TAX_AMOUNT"  || GridProc.Columns[j].Name.ToString() == "OTHER_AMOUNT" ||   GridProc.Columns[j].Name.ToString() == "OTHER_RATE")
                            {

                            }
                            else
                            {
                                tabControl1.SelectTab(tabPage3);
                                MessageBox.Show("' " + GridProc.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                GridProc.CurrentCell = GridProc[j, i];
                                GridProc.Focus();
                                GridProc.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }                      
                    }

                    if (Convert.ToDouble(GridProc["GRS_RATE", i].Value) == 0 || (GridProc["GRS_RATE", i].Value.ToString()) == String.Empty || Convert.ToDouble(GridProc["GRS_AMOUNT", i].Value) == 0 || (GridProc["GRS_AMOUNT", i].Value.ToString()) == String.Empty)
                        {
                            tabControl1.SelectTab(tabPage3);
                            MessageBox.Show("Invalid Process Rate & Amount..!", "Gainup");
                            GridProc.CurrentCell = GridProc[1, i];
                            GridProc.Focus();
                            GridProc.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    else
                    {
                        GridProc["GRS_AMOUNT", i].Value = Convert.ToDouble(GridProc["TOT_QTY", i].Value) * Convert.ToDouble(GridProc["GRS_RATE", i].Value);
                    }
                    GridProc["TAX_AMOUNT", i].Value = ((Convert.ToDouble(GridProc["GRS_AMOUNT", i].Value)) * Convert.ToDouble(GridProc["TAX_PER", i].Value)) / 100;                    
                    GridProc["OTHER_AMOUNT", i].Value = (Convert.ToDouble(GridProc["TOT_QTY", i].Value) * ((Convert.ToDouble(GridProc["OTHER_RATE", i].Value))));
                    GridProc["PUR_AMOUNT", i].Value = ((Convert.ToDouble(GridProc["GRS_AMOUNT", i].Value) + Convert.ToDouble(GridProc["TAX_AMOUNT", i].Value) + Convert.ToDouble(GridProc["OTHER_AMOUNT", i].Value)));
                    GridProc["PUR_RATE", i].Value = Convert.ToDouble(GridProc["PUR_AMOUNT", i].Value) / (Convert.ToDouble(GridProc["TOT_QTY", i].Value));

                }

                for (int i = 0; i < GridComm.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < GridComm.Columns.Count - 7; j++)
                    {
                        if (GridComm[j, i].Value == DBNull.Value || GridComm[j, i].Value.ToString() == String.Empty || GridComm[j, i].Value.ToString() == "0")
                        {
                            if (GridComm.Columns[j].Name.ToString() == "TAX_PER" || GridComm.Columns[j].Name.ToString() == "TAX_AMOUNT" ||  GridComm.Columns[j].Name.ToString() == "OTHER_AMOUNT" ||  GridComm.Columns[j].Name.ToString() == "OTHER_RATE")
                            {

                            }
                            else
                            {
                                tabControl1.SelectTab(tabPage4);
                                MessageBox.Show("' " + GridComm.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                GridComm.CurrentCell = GridComm[j, i];
                                GridComm.Focus();
                                GridComm.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }
                    }

                    if (Convert.ToDouble(GridComm["GRS_RATE", i].Value) == 0 || (GridComm["GRS_RATE", i].Value.ToString()) == String.Empty || Convert.ToDouble(GridComm["GRS_AMOUNT", i].Value) == 0 || (GridComm["GRS_AMOUNT", i].Value.ToString()) == String.Empty)
                    {
                        tabControl1.SelectTab(tabPage4);
                        MessageBox.Show("Invalid Commercial Rate & Amount..!", "Gainup");
                        GridComm.CurrentCell = GridComm[1, i];
                        GridComm.Focus();
                        GridComm.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                    else
                    {
                        GridComm["GRS_AMOUNT", i].Value = Convert.ToDouble(GridComm["TOT_QTY", i].Value) * Convert.ToDouble(GridComm["GRS_RATE", i].Value);
                    }
                    GridComm["TAX_AMOUNT", i].Value = ((Convert.ToDouble(GridComm["GRS_AMOUNT", i].Value)) * Convert.ToDouble(GridComm["TAX_PER", i].Value)) / 100;                    
                    GridComm["OTHER_AMOUNT", i].Value = (Convert.ToDouble(GridComm["TOT_QTY", i].Value) * ((Convert.ToDouble(GridComm["OTHER_RATE", i].Value))));
                    GridComm["PUR_AMOUNT", i].Value = ((Convert.ToDouble(GridComm["GRS_AMOUNT", i].Value) + Convert.ToDouble(GridComm["TAX_AMOUNT", i].Value)  + Convert.ToDouble(GridComm["OTHER_AMOUNT", i].Value)));
                    GridComm["PUR_RATE", i].Value = Convert.ToDouble(GridComm["PUR_AMOUNT", i].Value) / (Convert.ToDouble(GridComm["TOT_QTY", i].Value));

                }
                Calculate_Item();
                Total_Count();
                Total_Cost_Calc();
                Queries = new String[(GridTrim.Rows.Count + GridProc.Rows.Count + GridComm.Rows.Count) * 6 + 400];
                if(MyParent._New)
                {                    
                     DataTable TDt = new DataTable();
                     MyBase.Load_Data("Select IsNull(Max(Entry_No),0) + 1 Entry_No From Project_Planning_Master Where Company_Code = " + MyParent.CompCode + " ", ref TDt);
                     TxtENo.Text  = TDt.Rows[0][0].ToString();
                    if (TxtENo.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Invalid Entry No", "Gainup");
                        TxtENo.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    Queries[Array_Index++] = "Insert into Project_Planning_Master (Entry_NO, Effect_FRom, Order_ID, Proj_Type_Id,  PRoj_Activity_ID, Total_Material_Amount, Total_PRocess_Amount, Total_Comm_Amount, Total_Qty, Remarks, Uom_Id, Unit) Values (" + TxtENo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}  {0:T}", DtpDate.Value) + "', '" + TxtProjNo.Tag + "', " + TxtProjName.Tag + ", " + TxtActivity.Tag + ",  " + Convert.ToDouble(TxtTotTrmPurAmt.Text.ToString()) + ",   " + Convert.ToDouble(TxtTotProAmt.Text.ToString()) + ",   " + Convert.ToDouble(TxtTotComAmt.Text.ToString()) + ", " + Convert.ToDouble(TxtQty.Text.ToString()) + ", '" + TxtRemarks.Text.ToString() + "', " + TxtUom.Tag + ", " + Convert.ToDouble(TxtQty.Text.ToString()) + ") ; Select Scope_Identity()";
                    Queries[Array_Index++] = "Insert into Project_Planning_Summary_Details (Master_ID, Yarn_Cost, Trim_Cost, Proc_Cost, Comm_Cost, Spcl_Req_Cost, Profit, Total_Cost, Per_Pack_Cost, Exc_Rate, Prod_Rate_Ind, Prod_Rate_Exp, Sale_Price_Ind, DB_Ind, Profit_Ind, Value_Ind, Sale_Price_Exp, DB_Exp, Profit_Exp, Value_Exp) Values (@@IDENTITY, " + Convert.ToDouble(0) + ", " + Convert.ToDouble(TxtTrimCost.Text.ToString()) + ", " + Convert.ToDouble(TxtProcCost.Text.ToString()) + ", " + Convert.ToDouble(TxtCommCost.Text.ToString()) + ", " + Convert.ToDouble(0) + ", " + Convert.ToDouble(TxtProfit.Text.ToString()) + ", " + Convert.ToDouble(TxtTotalCost.Text.ToString()) + ", " + Convert.ToDouble(TxtPackCost.Text.ToString()) + ", " + Convert.ToDouble(TxtExRate.Text.ToString()) + ", " + Convert.ToDouble(TxtIndRs.Text.ToString()) + ", " + Convert.ToDouble(TxtExpRs.Text.ToString()) + ", " + Convert.ToDouble(TxtSalePriceInd.Text.ToString()) + " , " + Convert.ToDouble(TxtDBInd.Text.ToString()) + " , " + Convert.ToDouble(TxtProfitInd.Text.ToString()) + ", " + Convert.ToDouble(TxtValueInd.Text.ToString()) + ", " + Convert.ToDouble(TxtSalePriceExp.Text.ToString()) + ", " + Convert.ToDouble(TxtDBExp.Text.ToString()) + ", " + Convert.ToDouble(TxtProfitExp.Text.ToString()) + ", " + Convert.ToDouble(TxtValueExp.Text.ToString()) + ")";
                    Queries[Array_Index++] = MyParent.EntryLog("PROJECT PLANNING ENTRY", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Project_Planning_Master Set  Order_ID = " + TxtProjNo.Tag + ", Proj_Type_Id = " + TxtProjName.Tag + ", PRoj_Activity_ID = " + TxtActivity.Tag + ", Remarks = '" + TxtRemarks.Text + "',  Total_Material_Amount = " + Convert.ToDouble(TxtTotTrmPurAmt.Text.ToString()) + ", Total_PRocess_Amount = " + Convert.ToDouble(TxtTotProAmt.Text.ToString()) + ",  Total_Comm_Amount = " + Convert.ToDouble(TxtTotComAmt.Text.ToString()) + ", Total_Qty = " + Convert.ToDouble(TxtQty.Text.ToString()) + ", Uom_Id = " + TxtUom.Tag + ", Unit = " + Convert.ToDouble(TxtQty.Text.ToString()) + " Where Rowid = " + Code;
                    Queries[Array_Index++] = "Update Project_Planning_Summary_Details  Set  Yarn_Cost = 0, Trim_Cost = " + Convert.ToDouble(TxtTrimCost.Text.ToString()) + ", Proc_Cost = " + Convert.ToDouble(TxtProcCost.Text.ToString()) + ", Comm_Cost = " + Convert.ToDouble(TxtCommCost.Text.ToString()) + ", Spcl_Req_Cost = 0, Profit = " + Convert.ToDouble(TxtProfit.Text.ToString()) + ", Total_Cost = " + Convert.ToDouble(TxtTotalCost.Text.ToString()) + ", Per_Pack_Cost  = " + Convert.ToDouble(TxtPackCost.Text.ToString()) + ", Exc_Rate  = " + Convert.ToDouble(TxtExRate.Text.ToString()) + ", Prod_Rate_Ind = " + Convert.ToDouble(TxtIndRs.Text.ToString()) + ", Prod_Rate_Exp = " + Convert.ToDouble(TxtExpRs.Text.ToString()) + ", Sale_Price_Ind = " + Convert.ToDouble(TxtSalePriceInd.Text.ToString()) + " , DB_Ind = " + Convert.ToDouble(TxtDBInd.Text.ToString()) + " , Profit_Ind = " + Convert.ToDouble(TxtProfitInd.Text.ToString()) + ", Value_Ind = " + Convert.ToDouble(TxtValueInd.Text.ToString()) + ", Sale_Price_Exp = " + Convert.ToDouble(TxtSalePriceExp.Text.ToString()) + ", DB_Exp = " + Convert.ToDouble(TxtDBExp.Text.ToString()) + ", Profit_Exp = " + Convert.ToDouble(TxtProfitExp.Text.ToString()) + ", Value_Exp = " + Convert.ToDouble(TxtValueExp.Text.ToString()) + " Where Master_ID = " + Code + "";
                    Queries[Array_Index++] = "Delete From Project_Planning_Material_Details Where Approval_Flag = 'F' and Master_id = " + Code;
                    Queries[Array_Index++] = "Delete From Project_Planning_Process_Details Where Approval_Flag = 'F' and Master_id = " + Code;
                    Queries[Array_Index++] = "Delete From Project_Planning_Comm_Details Where Approval_Flag = 'F' and Master_id = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("PROJECT PLANNING ENTRY", "EDIT", Code.ToString());
                }

              

                for (int i = 0; i < GridTrim.Rows.Count - 1; i++)
                {                    
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into Project_Planning_Material_Details (MAster_ID, SNo, Access_Type, Item_ID, Color_ID, Size_ID, Unit, Cons, Uom_ID, Cons_Uom_ID, Req_Qty, Loss_Perc, Loss_Qty, Tot_Qty, Grs_Rate, Tax_Per, Other_Rate, Pur_Rate, Grs_Amount, Tax_Amount, Other_Amount, Pur_Amount, Remarks, Approval_Flag, Approval_Time, Tot_Qty_Conv, Grs_Rate_Conv, Other_Rate_Conv, Pur_Rate_Conv, Conv_Val, CAlc_Type, UomId_Po, App_Pur_Rate_Conv, Rate, Curr_ID,Ex_Rate) Values (@@IDENTITY, " + (i + 1) + ", '" + GridTrim["Access_Type", i].Value + "', " + GridTrim["Item_ID", i].Value + ", " + GridTrim["COLOR_ID", i].Value + ", " + GridTrim["SIZE_ID", i].Value + ", " + GridTrim["UNIT", i].Value + ", " + GridTrim["CONS", i].Value + ", " + GridTrim["UOM_ID", i].Value + ", " + GridTrim["CONS_UOM_ID", i].Value + ",0, 0, 0, " + Convert.ToDouble(GridTrim["TOT_QTY", i].Value) + ", " + Math.Round(Convert.ToDouble(GridTrim["GRS_RATE", i].Value), 5) + ", " + Math.Round(Convert.ToDouble(GridTrim["TAX_PER", i].Value), 3) + ", " + Math.Round(Convert.ToDouble(GridTrim["OTHER_RATE", i].Value), 5) + ", " + Math.Round(Convert.ToDouble(GridTrim["PUR_RATE", i].Value), 5) + ", " + Convert.ToDouble(GridTrim["GRS_AMOUNT", i].Value) + ",  " + Convert.ToDouble(GridTrim["Tax_Amount", i].Value) + " , " + Convert.ToDouble(GridTrim["Other_Amount", i].Value) + " , " + Convert.ToDouble(GridTrim["Pur_Amount", i].Value) + ", '" + (GridTrim["Remarks", i].Value) + "', 'F', Null, " + Convert.ToDouble(GridTrim["Tot_Qty_Conv", i].Value) + ", " + Convert.ToDouble(GridTrim["Grs_Rate_Conv", i].Value) + ", " + Convert.ToDouble(GridTrim["Other_Rate_Conv", i].Value) + ", " + Convert.ToDouble(GridTrim["Pur_Rate_Conv", i].Value) + ", " + Convert.ToDouble(GridTrim["Conv_Val", i].Value) + ", '" + (GridTrim["CAlc_Type", i].Value.ToString()) + "', " + Convert.ToDouble(GridTrim["UomId_Po", i].Value) + ", " + Convert.ToDouble(GridTrim["App_Pur_Rate_Conv", i].Value) + "," + GridTrim["Rate", i].Value + "," + GridTrim["CID", i].Value + "," + GridTrim["Ex_Rate", i].Value + ")";
                        }
                        else
                        {
                            if (GridTrim["FLAG", i].Value.ToString() == "F")
                            {
                                Queries[Array_Index++] = "Insert into Project_Planning_Material_Details (MAster_ID, SNo, Access_Type, Item_ID, Color_ID, Size_ID, Unit, Cons, Uom_ID, Cons_Uom_ID, Req_Qty, Loss_Perc, Loss_Qty, Tot_Qty, Grs_Rate, Tax_Per, Other_Rate, Pur_Rate, Grs_Amount, Tax_Amount, Other_Amount, Pur_Amount, Remarks, Approval_Flag, Approval_Time, Tot_Qty_Conv, Grs_Rate_Conv, Other_Rate_Conv, Pur_Rate_Conv, Conv_Val, CAlc_Type, UomId_Po, App_Pur_Rate_Conv, Rate, Curr_ID,Ex_Rate) Values (" + Code + ", " + (i + 1) + ", '" + GridTrim["Access_Type", i].Value + "', " + GridTrim["Item_ID", i].Value + ", " + GridTrim["COLOR_ID", i].Value + ", " + GridTrim["SIZE_ID", i].Value + ", " + GridTrim["UNIT", i].Value + ", " + GridTrim["CONS", i].Value + ", " + GridTrim["UOM_ID", i].Value + ", " + GridTrim["CONS_UOM_ID", i].Value + ",0,0,0, " + Convert.ToDouble(GridTrim["TOT_QTY", i].Value) + ", " + Math.Round(Convert.ToDouble(GridTrim["GRS_RATE", i].Value), 5) + ", " + Math.Round(Convert.ToDouble(GridTrim["TAX_PER", i].Value), 3) + ", " + Math.Round(Convert.ToDouble(GridTrim["OTHER_RATE", i].Value), 5) + ", " + Math.Round(Convert.ToDouble(GridTrim["PUR_RATE", i].Value), 5) + ", " + Convert.ToDouble(GridTrim["GRS_AMOUNT", i].Value) + ",  " + Convert.ToDouble(GridTrim["Tax_Amount", i].Value) + " , " + Convert.ToDouble(GridTrim["Other_Amount", i].Value) + " , " + Convert.ToDouble(GridTrim["Pur_Amount", i].Value) + ", '" + (GridTrim["Remarks", i].Value) + "', 'F', Null, " + Convert.ToDouble(GridTrim["Tot_Qty_Conv", i].Value) + ", " + Convert.ToDouble(GridTrim["Grs_Rate_Conv", i].Value) + ", " + Convert.ToDouble(GridTrim["Other_Rate_Conv", i].Value) + ", " + Convert.ToDouble(GridTrim["Pur_Rate_Conv", i].Value) + ", " + Convert.ToDouble(GridTrim["Conv_Val", i].Value) + ", '" + (GridTrim["CAlc_Type", i].Value.ToString()) + "', " + Convert.ToDouble(GridTrim["UomId_Po", i].Value) + ", " + Convert.ToDouble(GridTrim["App_Pur_Rate_Conv", i].Value) + "," + GridTrim["Rate", i].Value + "," + GridTrim["CID", i].Value + "," + GridTrim["Ex_Rate", i].Value + ")";
                            }
                        }                 
                }

                for (int i = 0; i < GridProc.Rows.Count - 1; i++)
                {                    
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into Project_Planning_Process_Details (Master_ID, SNo, Proc_ID,  Tot_Qty, Grs_Rate, Tax_Per, Other_Rate, Pur_Rate, Grs_Amount, Tax_Amount, Other_Amount, Pur_Amount, Remarks, Approval_Flag, Approval_Time) Values (@@IDENTITY, " + (i + 1) + ", " + GridProc["Proc_ID", i].Value + ", " + Convert.ToDouble(GridProc["TOT_QTY", i].Value) + ", " + Math.Round(Convert.ToDouble(GridProc["GRS_RATE", i].Value), 5) + ", " + Math.Round(Convert.ToDouble(GridProc["TAX_PER", i].Value), 3) + ", " + Math.Round(Convert.ToDouble(GridProc["OTHER_RATE", i].Value), 5) + ", " + Math.Round(Convert.ToDouble(GridProc["PUR_RATE", i].Value), 5) + ", " + Convert.ToDouble(GridProc["GRS_AMOUNT", i].Value) + ",  " + Convert.ToDouble(GridProc["Tax_Amount", i].Value) + " , " + Convert.ToDouble(GridProc["Other_Amount", i].Value) + " , " + Convert.ToDouble(GridProc["Pur_Amount", i].Value) + ", '" + (GridProc["Remarks", i].Value) + "', 'F', Null)";
                        }
                        else
                        {
                            if (GridProc["FLAG", i].Value.ToString() == "F")
                            {
                                Queries[Array_Index++] = "Insert into Project_Planning_Process_Details (Master_ID, SNo, Proc_ID,  Tot_Qty, Grs_Rate, Tax_Per, Other_Rate, Pur_Rate, Grs_Amount, Tax_Amount, Other_Amount, Pur_Amount, Remarks, Approval_Flag, Approval_Time) Values (" + Code + ", " + (i + 1) + ", " + GridProc["Proc_ID", i].Value + ", " + Convert.ToDouble(GridProc["TOT_QTY", i].Value) + ", " + Math.Round(Convert.ToDouble(GridProc["GRS_RATE", i].Value), 5) + ", " + Math.Round(Convert.ToDouble(GridProc["TAX_PER", i].Value), 3) + ", " + Math.Round(Convert.ToDouble(GridProc["OTHER_RATE", i].Value), 5) + ", " + Math.Round(Convert.ToDouble(GridProc["PUR_RATE", i].Value), 5) + ", " + Convert.ToDouble(GridProc["GRS_AMOUNT", i].Value) + ",  " + Convert.ToDouble(GridProc["Tax_Amount", i].Value) + " , " + Convert.ToDouble(GridProc["Other_Amount", i].Value) + " , " + Convert.ToDouble(GridProc["Pur_Amount", i].Value) + ", '" + (GridProc["Remarks", i].Value) + "', 'F', Null)";
                            }
                        }                 
                }

                for (int i = 0; i < GridComm.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Project_Planning_Comm_Details (Master_ID, SNo, Comm_ID,  Tot_Qty, Grs_Rate, Tax_Per, Other_Rate, Pur_Rate, Grs_Amount, Tax_Amount, Other_Amount, Pur_Amount, Remarks, Approval_Flag, Approval_Time) Values (@@IDENTITY, " + (i + 1) + ", " + GridComm["Comm_ID", i].Value + ", " + Convert.ToDouble(GridComm["TOT_QTY", i].Value) + ", " + Math.Round(Convert.ToDouble(GridComm["GRS_RATE", i].Value), 5) + ", " + Math.Round(Convert.ToDouble(GridComm["TAX_PER", i].Value), 3) + ", " + Math.Round(Convert.ToDouble(GridComm["OTHER_RATE", i].Value), 5) + ", " + Math.Round(Convert.ToDouble(GridComm["PUR_RATE", i].Value), 5) + ", " + Convert.ToDouble(GridComm["GRS_AMOUNT", i].Value) + ",  " + Convert.ToDouble(GridComm["Tax_Amount", i].Value) + " , " + Convert.ToDouble(GridComm["Other_Amount", i].Value) + " , " + Convert.ToDouble(GridComm["Pur_Amount", i].Value) + ", '" + (GridComm["Remarks", i].Value) + "', 'F', Null)";
                    }
                    else
                    {
                        if (GridComm["FLAG", i].Value.ToString() == "F")
                        {
                            Queries[Array_Index++] = "Insert into Project_Planning_Comm_Details (Master_ID, SNo, Comm_ID,  Tot_Qty, Grs_Rate, Tax_Per, Other_Rate, Pur_Rate, Grs_Amount, Tax_Amount, Other_Amount, Pur_Amount, Remarks, Approval_Flag, Approval_Time) Values (" + Code + ", " + (i + 1) + ", " + GridComm["Comm_ID", i].Value + ", " + Convert.ToDouble(GridComm["TOT_QTY", i].Value) + ", " + Math.Round(Convert.ToDouble(GridComm["GRS_RATE", i].Value), 5) + ", " + Math.Round(Convert.ToDouble(GridComm["TAX_PER", i].Value), 3) + ", " + Math.Round(Convert.ToDouble(GridComm["OTHER_RATE", i].Value), 5) + ", " + Math.Round(Convert.ToDouble(GridComm["PUR_RATE", i].Value), 5) + ", " + Convert.ToDouble(GridComm["GRS_AMOUNT", i].Value) + ",  " + Convert.ToDouble(GridComm["Tax_Amount", i].Value) + " , " + Convert.ToDouble(GridComm["Other_Amount", i].Value) + " , " + Convert.ToDouble(GridComm["Pur_Amount", i].Value) + ", '" + (GridComm["Remarks", i].Value) + "', 'F', Null)";
                        }
                    }
                }

              
                
                //Queries[Array_Index++] = "Exec Socks_Trim_Planning_Import_Proc '" + TxtProjNo.Text.ToString() + "'";
                //Queries[Array_Index++] = "Exec Socks_Process_Planning_Import_Proc '" + TxtProjNo.Text.ToString() + "'";                             
                //Queries[Array_Index++] = "Exec FitSocks.Dbo.Socks_Yarn_Status_Budget '" + TxtProjNo.Text.ToString() + "' ";

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

        public void Entry_Print()
        {
            try
            {
                MyBase.Clear(this);      
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
              TxtTrimCost.Text = " "; TxtProcCost.Text = " "; TxtCommCost.Text = " ";  TxtProfit.Text = " "; TxtTotalCost.Text = " "; TxtPackCost.Text = " "; TxtExRate.Text = " ";  TxtIndRs.Text = " "; TxtExpRs.Text = " "; TxtSalePriceInd.Text = " "; TxtDBInd.Text = " "; TxtProfitInd.Text = " "; TxtValueInd.Text = " "; TxtSalePriceExp.Text = " "; TxtDBExp.Text = " "; TxtProfitExp.Text = " "; TxtValueExp.Text= " ";
              Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Planning - Delete", "Select Distinct A.Order_No, A.Entry_No,  A.Effect_From, A.Proj_Type, A.Proj_ACtivity_NAme, A.Party, A.Employee, A.UOM_MAS, A.UNIT_MAS, A.BOM_Qty, A.Qty  , A.Total_Qty, A.OrdeR_ID, A.Proj_ACtivity_ID, A.UOM_ID_Mas, A.Proj_Type_ID, A.Order_Date , A.REmarks, A.Party_Code, A.Amount, A.RowID From Project_Planning_Material_Fn() A  Left Join Project_Order_Master D On A.Order_No = D.Order_No   Where D.Complete_Order = 'N' and D.Cancel_Order = 'N' and D.Company_Code = " + MyParent.CompCode + " and D.PArty_Code = " + MyParent.Proj_Login_Code + " Order by A.Order_No desc ", String.Empty, 150, 80, 100, 100, 140, 100, 100, 100, 100, 100); 
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    tabControl1.SelectTab(tabPage2);
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
                    MyBase.Run("Delete from Project_Planning_Summary_Details Where MasteR_ID = " + Code + " ", "Delete from Project_Planning_Comm_Details Where Approval_Flag = 'F' and MasteR_ID = " + Code + " ", "Delete from Project_Planning_Process_Details Where Approval_Flag = 'F' and MasteR_ID = " + Code + " ",  "Delete from Project_Planning_Master Where RowID = " + Code + "", MyParent.EntryLog("PROJECT PLANNING ENTRY", "DELETE", Code.ToString()));
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
                ChkCopy.Enabled = false;
               TxtTrimCost.Text = " "; TxtProcCost.Text = " "; TxtCommCost.Text = " "; TxtProfit.Text = " "; TxtTotalCost.Text = " "; TxtPackCost.Text = " "; TxtExRate.Text = " ";  TxtIndRs.Text = " "; TxtExpRs.Text = " "; TxtSalePriceInd.Text = " "; TxtDBInd.Text = " "; TxtProfitInd.Text = " "; TxtValueInd.Text = " "; TxtSalePriceExp.Text = " "; TxtDBExp.Text = " "; TxtProfitExp.Text = " "; TxtValueExp.Text= " ";
               Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Planning - View", "Select Distinct A.Order_No, A.Entry_No,  A.Effect_From, A.Proj_Type, A.Proj_ACtivity_NAme, A.Party, A.Employee, A.UOM_MAS, A.UNIT_MAS, A.BOM_Qty, A.Qty  , A.Total_Qty, A.OrdeR_ID, A.Proj_ACtivity_ID, A.UOM_ID_Mas, A.Proj_Type_ID, A.Order_Date , A.REmarks, A.Party_Code, A.Amount, A.RowID From Project_Planning_Material_Fn() A  Left Join Project_Order_Master D On A.Order_No = D.Order_No   Where D.Complete_Order = 'N' and D.Cancel_Order = 'N' and D.PArty_Code = " + MyParent.Proj_Login_Code + " Order by A.Order_No desc ", String.Empty, 150, 80, 100, 100, 140, 100, 100, 100, 100, 100); 
                if (Dr != null)
                {
                    Fill_Datas(Dr); 
                    tabControl1.SelectTab(tabPage2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }    

        void Grid_Data()
        {            
            try            
            {
               GridTrim.DataSource = null; GridProc.DataSource = null; GridComm.DataSource = null; 
                if (MyParent._New == true)
                {
                    if (ChkCopy.Checked == true && TxtTotTrimQty.Tag.ToString() != String.Empty)
                    {
                        Str1 = "Select SNO,  ACCESS_TYPE, ITEM, COLOR, SIZE, UOM,  " + Convert.ToDouble(TxtQty.Text.ToString()) + " UNIT, CONS, CONS_UOM, PO_UOM, TOT_QTY, Currency, Ex_Rate, CID, GRS_RATE,  TAX_PER, OTHER_RATE, PUR_RATE, TOT_QTY_CONV, GRS_RATE_CONV, OTHER_RATE_CONV, PUR_RATE_CONV, CONV_VAL, CALC_TYPE, APP_PUR_RATE_CONV, GRS_AMOUNT, TAX_AMOUNT, OTHER_AMOUNT, PUR_AMOUNT, REMARKS, Item_ID, Color_ID, Size_ID, Uom_ID, Cons_Uom_ID, UOMID_PO, DESCR, 'A' CALC, 'F' FLAG  From Project_Planning_Material_Fn() Where Rowid = " + TxtTotTrimQty.Tag.ToString() + "";
                        Str2 = "Select SNO, PROCESS, TOT_QTY, GRS_RATE,  TAX_PER, OTHER_RATE, PUR_RATE, GRS_AMOUNT, TAX_AMOUNT, OTHER_AMOUNT, PUR_AMOUNT,  REMARKS, Proc_ID, Proc_ID DESCR, 'F'  FLAG  From Project_Planning_Process_fn() Where rowid = " + TxtTotTrimQty.Tag.ToString() + "";
                        Str3 = "Select 0 SNO, COMM, TOT_QTY, GRS_RATE,  TAX_PER, OTHER_RATE, PUR_RATE, GRS_AMOUNT, TAX_AMOUNT, OTHER_AMOUNT, PUR_AMOUNT, REMARKS, Comm_ID, Comm_ID DESCR, 'F' FLAG  From Project_Planning_Comm_fn() Where rowid = " + TxtTotTrimQty.Tag.ToString() + "";
                    }
                    else
                    {
                        Str1 = "Select 0 SNO, 'GENERAL' ACCESS_TYPE, '' ITEM, '' COLOR, '' SIZE, '' UOM, 0.000 UNIT, 0.00 CONS, '' CONS_UOM, '' PO_UOM, 0.000 TOT_QTY, '' Currency, 0.000 Ex_Rate, 0 CID, 0.000 Rate, 0.0000 GRS_RATE,  0.00 TAX_PER, 0.000 OTHER_RATE, 0.0000 PUR_RATE, 0.000 TOT_QTY_CONV, 0.0000 GRS_RATE_CONV, 0.0000 OTHER_RATE_CONV, 0.0000 PUR_RATE_CONV, 0.000 CONV_VAL, '*' CALC_TYPE, 0.0000 APP_PUR_RATE_CONV, 0.00 GRS_AMOUNT, 0.00 TAX_AMOUNT, 0.00 OTHER_AMOUNT, 0.00 PUR_AMOUNT, '' REMARKS, Item_ID, Color_ID, Size_ID, Uom_ID, Cons_Uom_ID, UOMID_PO, '' DESCR,  'A' CALC, 'F' FLAG  From Project_Planning_Material_Details Where 1 = 2";
                        Str2 = "Select 0 SNO, '' PROCESS, 0.000 TOT_QTY, 0.0000 GRS_RATE,  0.00 TAX_PER, 0.000 OTHER_RATE, 0.0000 PUR_RATE, 0.00 GRS_AMOUNT, 0.00 TAX_AMOUNT, 0.00 OTHER_AMOUNT, 0.00 PUR_AMOUNT, '' REMARKS, Proc_ID, '' DESCR,  'F' FLAG  From Project_Planning_Process_Details Where 1 = 2";
                        Str3 = "Select 0 SNO, '' COMM, 0.000 TOT_QTY, 0.0000 GRS_RATE,  0.00 TAX_PER, 0.000 OTHER_RATE, 0.0000 PUR_RATE, 0.00 GRS_AMOUNT, 0.00 TAX_AMOUNT, 0.00 OTHER_AMOUNT, 0.00 PUR_AMOUNT, '' REMARKS, Comm_ID, '' DESCR,  'F' FLAG  From Project_Planning_Comm_Details Where 1 = 2";
                    }
                }
                else
                {
                    Str1 = "Select SNO,  ACCESS_TYPE, ITEM, COLOR, SIZE, UOM, UNIT, CONS, CONS_UOM, PO_UOM, TOT_QTY, Currency, Ex_Rate, Rate, CID, GRS_RATE,  TAX_PER, OTHER_RATE, PUR_RATE, TOT_QTY_CONV, GRS_RATE_CONV, OTHER_RATE_CONV, PUR_RATE_CONV, CONV_VAL, CALC_TYPE, APP_PUR_RATE_CONV, GRS_AMOUNT, TAX_AMOUNT, OTHER_AMOUNT, PUR_AMOUNT, REMARKS, Item_ID, Color_ID, Size_ID, Uom_ID, Cons_Uom_ID, UOMID_PO, DESCR, CALC, FLAG  From Project_Planning_Material_Fn() Where Rowid = " + Code + "";
                    Str2 = "Select SNO, PROCESS, TOT_QTY, GRS_RATE,  TAX_PER, OTHER_RATE, PUR_RATE, GRS_AMOUNT, TAX_AMOUNT, OTHER_AMOUNT, PUR_AMOUNT,  REMARKS, Proc_ID, Proc_ID DESCR,  FLAG  From Project_Planning_Process_fn() Where rowid = " + Code + "";
                    Str3 = "Select 0 SNO, COMM, TOT_QTY, GRS_RATE,  TAX_PER, OTHER_RATE, PUR_RATE, GRS_AMOUNT, TAX_AMOUNT, OTHER_AMOUNT, PUR_AMOUNT, REMARKS, Comm_ID, Comm_ID DESCR,  FLAG  From Project_Planning_Comm_fn() Where rowid = " + Code + "";
                }
               
                GridTrim.DataSource = MyBase.Load_Data(Str1, ref Dt1);
                GridProc.DataSource = MyBase.Load_Data(Str2, ref Dt2);
                GridComm.DataSource = MyBase.Load_Data(Str3, ref Dt3);
                MyBase.ReadOnly_Grid_Without(ref GridTrim, "ITEM", "SIZE", "COLOR", "UOM", "CONS_UOM", "PO_UOM", "UNIT", "CONS", "TAX_PER", "GRS_RATE", "OTHER_RATE", "Currency", "Rate");
                MyBase.ReadOnly_Grid_Without(ref GridProc, "PROCESS", "TOT_QTY", "TAX_PER", "GRS_RATE", "OTHER_RATE");
                MyBase.ReadOnly_Grid_Without(ref GridComm, "COMM", "TOT_QTY", "GRS_RATE", "TAX_PER", "OTHER_RATE");
                MyBase.Grid_Designing(ref GridTrim, ref Dt1, "CID", "CALC", "ACCESS_TYPE", "OTHER_RATE_CONV", "PUR_RATE_CONV", "CONV_VAL", "CALC_TYPE", "UOMID_PO", "CONS_UOM_ID","Item_ID", "Size_ID", "Color_ID", "UOM_ID", "DESCR", "REMARKS");
                MyBase.Grid_Designing(ref GridProc, ref Dt2, "Proc_ID", "FLAG", "DESCR");
                MyBase.Grid_Designing(ref GridComm, ref Dt3, "Comm_ID", "FLAG", "DESCR");
                MyBase.Grid_Width(ref GridTrim, 50, 100, 120, 100, 100, 100, 100, 100, 80, 100, 80, 80, 100, 120, 120, 120, 100, 80, 80, 100, 100, 100, 100);
                MyBase.Grid_Width(ref GridProc, 50, 150, 100, 80, 80, 100, 100, 80, 80,100);
                MyBase.Grid_Width(ref GridComm, 50, 150, 100, 100, 80, 80, 100, 100, 100, 100, 100);               
                MyBase.Grid_Colouring(ref GridTrim, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Colouring(ref GridProc, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Colouring(ref GridComm, Control_Modules.Grid_Design_Mode.Column_Wise);                
                GridTrim.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                
                GridTrim.Columns["ACCESS_TYPE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;                                                
                GridTrim.Columns["FLAG"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;      
                GridTrim.Columns["ITEM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;                
                GridTrim.Columns["COLOR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridTrim.Columns["SIZE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridTrim.Columns["CONS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridTrim.Columns["UOM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridTrim.Columns["TOT_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridTrim.Columns["PUR_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridTrim.Columns["PUR_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridTrim.Columns["TAX_PER"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                GridTrim.Columns["OTHER_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;              
                GridTrim.Columns["OTHER_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridTrim.Columns["TAX_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridTrim.Columns["GRS_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridTrim.Columns["GRS_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridProc.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                
                GridProc.Columns["PROCESS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;                
                GridProc.Columns["TOT_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                                               
                GridProc.Columns["GRS_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridProc.Columns["GRS_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridProc.Columns["FLAG"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridProc.Columns["TAX_PER"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                GridProc.Columns["OTHER_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                GridProc.Columns["OTHER_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridProc.Columns["TAX_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridProc.Columns["PUR_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridProc.Columns["PUR_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridComm.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridComm.Columns["COMM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridComm.Columns["TOT_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                GridComm.Columns["GRS_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridComm.Columns["GRS_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridComm.Columns["FLAG"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridComm.Columns["TAX_PER"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                GridComm.Columns["OTHER_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                GridComm.Columns["OTHER_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridComm.Columns["TAX_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridComm.Columns["PUR_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridComm.Columns["PUR_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridTrim.Columns["Currency"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridProc.Columns["FLAG"].HeaderText = "APPROVAL"; GridComm.Columns["FLAG"].HeaderText = "APPROVAL";                                
                GridTrim.Columns["PUR_RATE"].DefaultCellStyle.Format = "0.0000";
                GridTrim.Columns["GRS_RATE"].DefaultCellStyle.Format = "0.0000";
                GridTrim.Columns["Ex_Rate"].DefaultCellStyle.Format = "0.0000";
                GridTrim.Columns["Rate"].DefaultCellStyle.Format = "0.0000"; 
                
                GridTrim.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridProc.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridComm.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;            
             
                if (MyParent._New == false)
                {                    
                    for (int q = 0; q < GridTrim.Rows.Count - 1; q++)
                    {
                        if (GridTrim["FLAG", q].Value.ToString() == "T")
                        {
                            GridTrim.Rows[q].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
                        }
                    }
                    for (int r = 0; r < GridProc.Rows.Count - 1; r++)
                    {
                        if (GridProc["FLAG", r].Value.ToString() == "T")
                        {
                            GridProc.Rows[r].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
                        }
                    }
                    for (int s = 0; s < GridComm.Rows.Count - 1; s++)
                    {
                        if (GridComm["FLAG", s].Value.ToString() == "T")
                        {
                            GridComm.Rows[s].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
                        }
                    }                
                      
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    
        private void GridTrim_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Trim  == null)
                {
                    Txt_Trim  = (TextBox)e.Control;
                    Txt_Trim.KeyDown += new KeyEventHandler(Txt_Trim_KeyDown);
                    Txt_Trim.KeyPress +=new KeyPressEventHandler(Txt_Trim_KeyPress);
                    Txt_Trim.Leave +=new EventHandler(Txt_Trim_Leave);
                    Txt_Trim.TextChanged +=new EventHandler(Txt_Trim_TextChanged);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridProc_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Proc  == null)
                {
                    Txt_Proc  = (TextBox)e.Control;
                    Txt_Proc.KeyDown += new KeyEventHandler(Txt_Proc_KeyDown);
                    Txt_Proc.KeyPress +=new KeyPressEventHandler(Txt_Proc_KeyPress);
                    Txt_Proc.Leave +=new EventHandler(Txt_Proc_Leave);                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

   

     

        private void GridComm_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Comm == null)
                {
                    Txt_Comm = (TextBox)e.Control;
                    Txt_Comm.KeyDown += new KeyEventHandler(Txt_Comm_KeyDown);
                    Txt_Comm.KeyPress += new KeyPressEventHandler(Txt_Comm_KeyPress);
                    Txt_Comm.Leave += new EventHandler(Txt_Comm_Leave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        void Txt_Trim_TextChanged(object sender, EventArgs e)
        {
            
        }

        void Txt_Proc_Leave(object sender, EventArgs e)
        {
            try
            {
                if (GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridProc["TAX_PER", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridProc["TAX_PER", GridProc.CurrentCell.RowIndex].Value = "0";
                    }
                    if (GridProc["TAX_AMOUNT", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridProc["TAX_AMOUNT", GridProc.CurrentCell.RowIndex].Value = "0";
                    }                   
                    if (GridProc["OTHER_RATE", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridProc["OTHER_RATE", GridProc.CurrentCell.RowIndex].Value = "0";
                    }
                    
                    if (GridProc["OTHER_AMOUNT", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridProc["OTHER_AMOUNT", GridProc.CurrentCell.RowIndex].Value = "0";
                    }
                    if (GridProc["PUR_AMOUNT", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridProc["PUR_AMOUNT", GridProc.CurrentCell.RowIndex].Value = "0";
                    }
                   
                    else if (GridProc.CurrentCell.ColumnIndex == GridProc.Columns["GRS_RATE"].Index && Txt_Proc.Text.ToString() != String.Empty)
                    {
                        GridProc["GRS_RATE", GridProc.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Proc.Text.ToString());
                        if (GridProc["GRS_RATE", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridProc["GRS_RATE", GridProc.CurrentCell.RowIndex].Value = 0.00;
                        }
                        else
                        {
                            GridProc["GRS_RATE", GridProc.CurrentCell.RowIndex].Value = GridProc["GRS_RATE", GridProc.CurrentCell.RowIndex].Value.ToString();
                            GridProc["GRS_AMOUNT", GridProc.CurrentCell.RowIndex].Value = Convert.ToDouble(GridProc["TOT_QTY", GridProc.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridProc["GRS_RATE", GridProc.CurrentCell.RowIndex].Value);                           
                        }                       
                    }
                    else if (GridProc.CurrentCell.ColumnIndex == GridProc.Columns["TAX_PER"].Index && Txt_Proc.Text.ToString() != String.Empty)
                    {
                        GridProc["TAX_PER", GridProc.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Proc.Text.ToString());
                        if (GridProc["TAX_PER", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridProc["TAX_PER", GridProc.CurrentCell.RowIndex].Value = 0;
                        }
                        else
                        {
                            GridProc["TAX_AMOUNT", GridProc.CurrentCell.RowIndex].Value = ((Convert.ToDouble(GridProc["GRS_AMOUNT", GridProc.CurrentCell.RowIndex].Value)) * Convert.ToDouble(GridProc["TAX_PER", GridProc.CurrentCell.RowIndex].Value)) / 100;                            
                            GridProc["OTHER_AMOUNT", GridProc.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridProc["TOT_QTY", GridProc.CurrentCell.RowIndex].Value) * ((Convert.ToDouble(GridProc["OTHER_RATE", GridProc.CurrentCell.RowIndex].Value))));
                            GridProc["PUR_AMOUNT", GridProc.CurrentCell.RowIndex].Value = ((Convert.ToDouble(GridProc["GRS_AMOUNT", GridProc.CurrentCell.RowIndex].Value) + Convert.ToDouble(GridProc["TAX_AMOUNT", GridProc.CurrentCell.RowIndex].Value)  + Convert.ToDouble(GridProc["OTHER_AMOUNT", GridProc.CurrentCell.RowIndex].Value)));
                            GridProc["PUR_RATE", GridProc.CurrentCell.RowIndex].Value = Convert.ToDouble(GridProc["GRS_AMOUNT", GridProc.CurrentCell.RowIndex].Value) / (Convert.ToDouble(GridProc["TOT_QTY", GridProc.CurrentCell.RowIndex].Value));
                        }
                    }                   
                    else if (GridProc.CurrentCell.ColumnIndex == GridProc.Columns["OTHER_RATE"].Index && Txt_Proc.Text.ToString() != String.Empty)
                    {
                        GridProc["OTHER_RATE", GridProc.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Proc.Text.ToString());
                        if (GridProc["OTHER_RATE", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridProc["OTHER_RATE", GridProc.CurrentCell.RowIndex].Value = 0;
                        }
                        else
                        {
                            GridProc["TAX_AMOUNT", GridProc.CurrentCell.RowIndex].Value = ((Convert.ToDouble(GridProc["GRS_AMOUNT", GridProc.CurrentCell.RowIndex].Value)) * Convert.ToDouble(GridProc["TAX_PER", GridProc.CurrentCell.RowIndex].Value)) / 100;
                            GridProc["OTHER_AMOUNT", GridProc.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridProc["TOT_QTY", GridProc.CurrentCell.RowIndex].Value) * ((Convert.ToDouble(GridProc["OTHER_RATE", GridProc.CurrentCell.RowIndex].Value))));
                            GridProc["PUR_AMOUNT", GridProc.CurrentCell.RowIndex].Value = ((Convert.ToDouble(GridProc["GRS_AMOUNT", GridProc.CurrentCell.RowIndex].Value) + Convert.ToDouble(GridProc["TAX_AMOUNT", GridProc.CurrentCell.RowIndex].Value) + Convert.ToDouble(GridProc["OTHER_AMOUNT", GridProc.CurrentCell.RowIndex].Value)));
                            GridProc["PUR_RATE", GridProc.CurrentCell.RowIndex].Value = Convert.ToDouble(GridProc["GRS_AMOUNT", GridProc.CurrentCell.RowIndex].Value) / (Convert.ToDouble(GridProc["TOT_QTY", GridProc.CurrentCell.RowIndex].Value));
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

        void Txt_Comm_Leave(object sender, EventArgs e)
        {
            try
            {
                if (GridComm["COMM", GridComm.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridComm["TAX_PER", GridComm.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridComm["TAX_PER", GridComm.CurrentCell.RowIndex].Value = "0";
                    }
                    if (GridComm["TAX_AMOUNT", GridComm.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridComm["TAX_AMOUNT", GridComm.CurrentCell.RowIndex].Value = "0";
                    }
                    if (GridComm["OTHER_RATE", GridComm.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridComm["OTHER_RATE", GridComm.CurrentCell.RowIndex].Value = "0";
                    }                   
                    if (GridComm["OTHER_AMOUNT", GridComm.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridComm["OTHER_AMOUNT", GridComm.CurrentCell.RowIndex].Value = "0";
                    }
                    if (GridComm["PUR_AMOUNT", GridComm.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridComm["PUR_AMOUNT", GridComm.CurrentCell.RowIndex].Value = "0";
                    }

                    if (GridComm.CurrentCell.ColumnIndex == GridComm.Columns["TOT_QTY"].Index)
                    {
                        if (GridComm["COMM", GridComm.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            MessageBox.Show("INVALID COMMERCIAL", "Gainup");
                            GridComm.CurrentCell = GridComm["COMM", GridComm.CurrentCell.RowIndex];
                            GridComm.Focus();
                            GridComm.BeginEdit(true);
                            return;
                        }
                    }
                    else if (GridComm.CurrentCell.ColumnIndex == GridComm.Columns["GRS_RATE"].Index && Txt_Comm.Text.ToString() != String.Empty)
                    {
                        GridComm["GRS_RATE", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Comm.Text.ToString());
                        if (GridComm["GRS_RATE", GridComm.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridComm["GRS_RATE", GridComm.CurrentCell.RowIndex].Value = 0.00;
                        }
                        else if (Convert.ToDouble(GridComm["GRS_RATE", GridComm.CurrentCell.RowIndex].Value) > 0)
                        {                           
                                GridComm["GRS_AMOUNT", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(GridComm["TOT_QTY", GridComm.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridComm["GRS_RATE", GridComm.CurrentCell.RowIndex].Value);                         
                        }
                        else
                        {
                            GridComm["GRS_RATE", GridComm.CurrentCell.RowIndex].Value = 0.00;
                        }
                    }
                    else if (GridComm.CurrentCell.ColumnIndex == GridComm.Columns["TAX_PER"].Index && Txt_Comm.Text.ToString() != String.Empty)
                    {
                        GridComm["TAX_PER", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Comm.Text.ToString());
                        if (GridComm["TAX_PER", GridComm.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridComm["TAX_PER", GridComm.CurrentCell.RowIndex].Value = 0;
                        }
                        else
                        {
                            GridComm["TAX_AMOUNT", GridComm.CurrentCell.RowIndex].Value = ((Convert.ToDouble(GridComm["GRS_AMOUNT", GridComm.CurrentCell.RowIndex].Value)) * Convert.ToDouble(GridComm["TAX_PER", GridComm.CurrentCell.RowIndex].Value)) / 100;                            
                            GridComm["OTHER_AMOUNT", GridComm.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridComm["TOT_QTY", GridComm.CurrentCell.RowIndex].Value) * ((Convert.ToDouble(GridComm["OTHER_RATE", GridComm.CurrentCell.RowIndex].Value))));
                            GridComm["PUR_AMOUNT", GridComm.CurrentCell.RowIndex].Value = ((Convert.ToDouble(GridComm["GRS_AMOUNT", GridComm.CurrentCell.RowIndex].Value) + Convert.ToDouble(GridComm["TAX_AMOUNT", GridComm.CurrentCell.RowIndex].Value)  + Convert.ToDouble(GridComm["OTHER_AMOUNT", GridComm.CurrentCell.RowIndex].Value)));
                            GridComm["PUR_RATE", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(GridComm["PUR_AMOUNT", GridComm.CurrentCell.RowIndex].Value) / (Convert.ToDouble(GridComm["TOT_QTY", GridComm.CurrentCell.RowIndex].Value));
                        }
                    }                    
                    else if (GridComm.CurrentCell.ColumnIndex == GridComm.Columns["OTHER_RATE"].Index && Txt_Comm.Text.ToString() != String.Empty)
                    {
                        GridComm["OTHER_RATE", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Comm.Text.ToString());
                        if (GridComm["OTHER_RATE", GridComm.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridComm["OTHER_RATE", GridComm.CurrentCell.RowIndex].Value = 0;
                        }
                        else
                        {
                            GridComm["TAX_AMOUNT", GridComm.CurrentCell.RowIndex].Value = ((Convert.ToDouble(GridComm["GRS_AMOUNT", GridComm.CurrentCell.RowIndex].Value)) * Convert.ToDouble(GridComm["TAX_PER", GridComm.CurrentCell.RowIndex].Value)) / 100;
                            GridComm["OTHER_AMOUNT", GridComm.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridComm["TOT_QTY", GridComm.CurrentCell.RowIndex].Value) * ((Convert.ToDouble(GridComm["OTHER_RATE", GridComm.CurrentCell.RowIndex].Value))));
                            GridComm["PUR_AMOUNT", GridComm.CurrentCell.RowIndex].Value = ((Convert.ToDouble(GridComm["GRS_AMOUNT", GridComm.CurrentCell.RowIndex].Value) + Convert.ToDouble(GridComm["TAX_AMOUNT", GridComm.CurrentCell.RowIndex].Value) + Convert.ToDouble(GridComm["OTHER_AMOUNT", GridComm.CurrentCell.RowIndex].Value)));
                            GridComm["PUR_RATE", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(GridComm["PUR_AMOUNT", GridComm.CurrentCell.RowIndex].Value) / (Convert.ToDouble(GridComm["TOT_QTY", GridComm.CurrentCell.RowIndex].Value));
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

        void Txt_Trim_Leave(object sender, EventArgs e)
        {
            try
            {
                if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex].Value = "0";
                    }
                    if (GridTrim["TAX_AMOUNT", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridTrim["TAX_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = "0";
                    }
                    if (GridTrim["OTHER_RATE", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridTrim["OTHER_RATE", GridTrim.CurrentCell.RowIndex].Value = "0";
                    }                    
                    if (GridTrim["OTHER_AMOUNT", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridTrim["OTHER_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = "0";
                    }
                    if (GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = "0";
                    }
                    if (GridTrim["Rate", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        GridTrim["Rate", GridTrim.CurrentCell.RowIndex].Value = "0";
                    }


                    if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["CONS"].Index && Txt_Trim.Text.ToString() != String.Empty)
                    {
                        if (GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridTrim["UNIT", GridTrim.CurrentCell.RowIndex].Value = 0.0000;
                            MessageBox.Show("INVALID UNIT ", "Gainup");
                            GridTrim.CurrentCell = GridTrim["UNIT", GridTrim.CurrentCell.RowIndex];
                            GridTrim.Focus();
                            GridTrim.BeginEdit(true);
                            return;
                        }
                        else if (Convert.ToDouble(GridTrim["UNIT", GridTrim.CurrentCell.RowIndex].Value) > 0)
                        {
                            GridTrim["TOT_QTY", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["UNIT", GridTrim.CurrentCell.RowIndex].Value) * Math.Round(Convert.ToDouble(GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value), 5);
                        }
                        else
                        {
                            GridTrim["TOT_QTY", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                        }

                        if (GridTrim["Rate", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridTrim["Rate", GridTrim.CurrentCell.RowIndex].Value = 0.0000;
                            MessageBox.Show("INVALID Rate ", "Gainup");
                            GridTrim.CurrentCell = GridTrim["Rate", GridTrim.CurrentCell.RowIndex];
                            GridTrim.Focus();
                            GridTrim.BeginEdit(true);
                            return;
                        }
                        else if (Convert.ToDouble(GridTrim["Rate", GridTrim.CurrentCell.RowIndex].Value) > 0)
                        {
                            GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["Ex_Rate", GridTrim.CurrentCell.RowIndex].Value) * Math.Round(Convert.ToDouble(GridTrim["Rate", GridTrim.CurrentCell.RowIndex].Value), 5);
                        }
                        else
                        {
                            GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                        }
                   
                        if (GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value = 0.0000;
                            MessageBox.Show("INVALID GRS_RATE ", "Gainup");
                            GridTrim.CurrentCell = GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex];
                            GridTrim.Focus();
                            GridTrim.BeginEdit(true);
                            return;
                        }
                        else if (Convert.ToDouble(GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value) > 0)
                        {
                            GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["TOT_QTY", GridTrim.CurrentCell.RowIndex].Value) * Math.Round(Convert.ToDouble(GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value), 5);
                        }
                        else
                        {
                            GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                        }                        
                    }                    
                    else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["GRS_RATE"].Index && Txt_Trim.Text.ToString() != String.Empty)
                    {
                        GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value = Math.Round(Convert.ToDouble(Txt_Trim.Text.ToString()), 5);
                        if (GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value = 0.0000;
                        }
                        else if (Convert.ToDouble(GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value) > 0)
                        {
                            GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["TOT_QTY", GridTrim.CurrentCell.RowIndex].Value) * Math.Round(Convert.ToDouble(GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value),5);
                        }
                        else
                        {
                            GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value = 0.0000;
                        }

                        if (GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex].Value = 0;
                        }
                        else
                        {
                            GridTrim["TAX_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = ((Convert.ToDouble(GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value)) * Convert.ToDouble(GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex].Value)) / 100;
                            GridTrim["OTHER_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridTrim["TOT_QTY", GridTrim.CurrentCell.RowIndex].Value) * ((Convert.ToDouble(GridTrim["OTHER_RATE", GridTrim.CurrentCell.RowIndex].Value))));
                            GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = ((Convert.ToDouble(GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value) + Convert.ToDouble(GridTrim["TAX_AMOUNT", GridTrim.CurrentCell.RowIndex].Value) + Convert.ToDouble(GridTrim["OTHER_AMOUNT", GridTrim.CurrentCell.RowIndex].Value)));
                            //GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value) / (Convert.ToDouble(GridTrim["TOT_QTY", GridTrim.CurrentCell.RowIndex].Value));
                        }
                        Calculate_Item();
                    }
                    else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["TAX_PER"].Index && Txt_Trim.Text.ToString() != String.Empty)
                    {
                        GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Trim.Text.ToString());
                        if (GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex].Value = 0;
                        }                        
                        else
                        {
                            GridTrim["TAX_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = ((Convert.ToDouble(GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value)) * Convert.ToDouble(GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex].Value)) / 100;                           
                            GridTrim["OTHER_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridTrim["TOT_QTY", GridTrim.CurrentCell.RowIndex].Value) * ((Convert.ToDouble(GridTrim["OTHER_RATE", GridTrim.CurrentCell.RowIndex].Value))));
                            GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = ((Convert.ToDouble(GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value) + Convert.ToDouble(GridTrim["TAX_AMOUNT", GridTrim.CurrentCell.RowIndex].Value)  + Convert.ToDouble(GridTrim["OTHER_AMOUNT", GridTrim.CurrentCell.RowIndex].Value)));
                            GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value) / (Convert.ToDouble(GridTrim["TOT_QTY", GridTrim.CurrentCell.RowIndex].Value));
                        }
                    }                                   
                    else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["OTHER_RATE"].Index && Txt_Trim.Text.ToString() != String.Empty)
                    {
                        GridTrim["OTHER_RATE", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Trim.Text.ToString());
                        if (GridTrim["OTHER_RATE", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridTrim["OTHER_RATE", GridTrim.CurrentCell.RowIndex].Value = 0;
                        }
                        else
                        {
                            GridTrim["TAX_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = ((Convert.ToDouble(GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value)) * Convert.ToDouble(GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex].Value)) / 100;
                            GridTrim["OTHER_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridTrim["TOT_QTY", GridTrim.CurrentCell.RowIndex].Value) * ((Convert.ToDouble(GridTrim["OTHER_RATE", GridTrim.CurrentCell.RowIndex].Value))));
                            GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = ((Convert.ToDouble(GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value) + Convert.ToDouble(GridTrim["TAX_AMOUNT", GridTrim.CurrentCell.RowIndex].Value) + Convert.ToDouble(GridTrim["OTHER_AMOUNT", GridTrim.CurrentCell.RowIndex].Value)));
                            GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value) / (Convert.ToDouble(GridTrim["TOT_QTY", GridTrim.CurrentCell.RowIndex].Value));
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

        void Txt_Proc_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridProc["FLAG", GridProc.CurrentCell.RowIndex].Value.ToString() == "F")
                    {
                        if (GridProc.CurrentCell.ColumnIndex == GridProc.Columns["TOT_QTY"].Index || GridProc.CurrentCell.ColumnIndex == GridProc.Columns["GRS_RATE"].Index || GridProc.CurrentCell.ColumnIndex == GridProc.Columns["TAX_PER"].Index || GridProc.CurrentCell.ColumnIndex == GridProc.Columns["OTHER_RATE"].Index)
                        {
                            MyBase.Valid_Decimal(Txt_Proc, e);
                        }
                        else
                        {
                            MyBase.Valid_Null(Txt_Proc, e);
                        }
                    }
                    else
                    {
                        MyBase.Valid_Null(Txt_Proc, e);
                    }
                }
                else
                {
                    MyBase.Valid_Null(Txt_Proc, e);
                }
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Comm_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GridComm["COMM", GridComm.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridComm["FLAG", GridComm.CurrentCell.RowIndex].Value.ToString() == "F")
                    {
                        if (GridComm.CurrentCell.ColumnIndex == GridComm.Columns["TOT_QTY"].Index || GridComm.CurrentCell.ColumnIndex == GridComm.Columns["GRS_RATE"].Index || GridComm.CurrentCell.ColumnIndex == GridComm.Columns["TAX_PER"].Index || GridComm.CurrentCell.ColumnIndex == GridComm.Columns["OTHER_RATE"].Index)
                        {
                            MyBase.Valid_Decimal(Txt_Comm, e);
                        }
                        else
                        {
                            MyBase.Valid_Null(Txt_Comm, e);
                        }
                    }
                    else
                    {
                        MyBase.Valid_Null(Txt_Comm, e);
                    }
                }
                else
                {
                    MyBase.Valid_Null(Txt_Comm, e);
                }
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void Txt_Trim_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridTrim["FLAG", GridTrim.CurrentCell.RowIndex].Value.ToString() == "F")
                    {
                        if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["CONS"].Index || GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["Rate"].Index || GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["TAX_PER"].Index || GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["OTHER_RATE"].Index)
                        {
                            MyBase.Valid_Decimal(Txt_Trim, e);
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["UNIT"].Index)
                        {
                            if ((GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString().Contains("STEEL") || GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString().Contains("(SC)")) || GridTrim["CALC", GridTrim.CurrentCell.RowIndex].Value.ToString() == "M" || MyParent.UserCode == 1)
                            {
                                MyBase.Valid_Decimal(Txt_Trim, e);
                            }
                            else
                            {
                                MyBase.Valid_Null(Txt_Trim, e);
                            }
                        }
                        else
                        {
                            MyBase.Valid_Null(Txt_Trim, e);
                        }
                    }
                    else
                    {
                        MyBase.Valid_Null(Txt_Trim, e);
                    }
                }
                else
                {
                    MyBase.Valid_Null(Txt_Trim, e);
                }
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void Txt_Trim_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                   if (e.KeyCode == Keys.Down)
                    {                                               
                        if (TxtBuyer.Text.Trim() == String.Empty)
                        {
                            MessageBox.Show ("Invalid Buyer Name", "Gainup");
                            TxtBuyer.Focus();
                            return;
                        }                        
                        if (TxtProjNo.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("Invalid OCN NO", "Gainup");
                            TxtProjNo.Focus();                           
                            return;
                        }
                       
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["ACCESS_TYPE"].Index)
                        {
                            if(GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            {
                                TxtTotTrimQty.Tag = "";
                                if(ChkCopy.Checked == false)
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Access", "Select 'GENERAL' Type  Union Select 'SPECIAL' Type", String.Empty, 250);
                                    if (Dr != null)
                                    {
                                        GridTrim["ACCESS_TYPE", GridTrim.CurrentCell.RowIndex].Value = Dr["Type"].ToString();
                                        Txt_Trim.Text = Dr["Type"].ToString();
                                    }
                                }
                                //else                                    
                                //{
                                //    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Order No", "Select Distinct A.Order_No, A.Entry_No,  A.Effect_From, A.Proj_Type, A.Proj_ACtivity_NAme, A.Party, A.Employee, A.UOM_MAS, A.UNIT_MAS, A.BOM_Qty, A.Qty  , A.Total_Qty, A.OrdeR_ID, A.Proj_ACtivity_ID, A.UOM_ID_Mas, A.Proj_Type_ID, A.Order_Date , A.REmarks, A.Party_Code, A.Amount, A.RowID From Project_Planning_Material_Fn() A  Left Join Project_Order_Master D On A.Order_No = D.Order_No   Where D.Complete_Order = 'N' and D.Cancel_Order = 'N' Order by A.Order_No desc ", String.Empty, 250);
                                //        if (Dr != null)
                                //        {
                                //            TxtTotTrimQty.Tag = Dr["Rowid"].ToString();
                                //            Grid_Data();
                                //        }
                                //}                                                          
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["ITEM"].Index)
                        {
                            TxtTotTrimQty.Tag = "";
                            //if(GridTrim["ACCESS_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() == "GENERAL" || GridTrim["ACCESS_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() == "SPECIAL")
                            //{         
                            if (ChkCopy.Checked == false )
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Item", "Select  Item, Calc, ItemID From Item Order by Item ", String.Empty, 250,100);
                                if (Dr != null)
                                {
                                    GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                                    GridTrim["ITEM_ID", GridTrim.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                                    GridTrim["CALC", GridTrim.CurrentCell.RowIndex].Value = Dr["CALC"].ToString();
                                    GridTrim["ACCESS_TYPE", GridTrim.CurrentCell.RowIndex].Value = "GENERAL";
                                    GridTrim["UNIT", GridTrim.CurrentCell.RowIndex].Value = TxtQty.Text.ToString();
                                    GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value = "0.0000";
                                    GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value = "0.0000";
                                    GridTrim["FLAG", GridTrim.CurrentCell.RowIndex].Value = "F";
                                    GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value = "-";
                                    GridTrim["COLOR_ID", GridTrim.CurrentCell.RowIndex].Value = 1;
                                    GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value = "";
                                    GridTrim["UOM", GridTrim.CurrentCell.RowIndex].Value = "";
                                    GridTrim["CONS_UOM", GridTrim.CurrentCell.RowIndex].Value = "";
                                    GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value = "0";
                                    GridTrim["APP_PUR_RATE_CONV", GridTrim.CurrentCell.RowIndex].Value = "0";
                                    GridTrim["DESCR", GridTrim.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString() + "-" + GridTrim["COLOR_ID", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                    GridTrim["Currency", GridTrim.CurrentCell.RowIndex].Value = "INR";
                                    GridTrim["Ex_Rate", GridTrim.CurrentCell.RowIndex].Value = "1.000";
                                    GridTrim["CID", GridTrim.CurrentCell.RowIndex].Value = "25";

                                    Txt_Trim.Text = Dr["ITEM"].ToString();
                                }
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Order No", "Select Distinct A.Order_No, A.Entry_No,  A.Effect_From, A.Proj_Type, A.Proj_ACtivity_NAme, A.Party, A.Employee, A.UOM_MAS, A.UNIT_MAS, A.BOM_Qty, A.Qty  , A.Total_Qty, A.OrdeR_ID, A.Proj_ACtivity_ID, A.UOM_ID_Mas, A.Proj_Type_ID, A.Order_Date , A.REmarks, A.Party_Code, A.Amount, A.RowID From Project_Planning_Material_Fn() A  Left Join Project_Order_Master D On A.Order_No = D.Order_No   Where D.Complete_Order = 'N' and D.Cancel_Order = 'N' Order by A.Order_No desc ", String.Empty, 100, 80, 150, 200, 200, 100, 100 ,100, 100, 100);
                                if (Dr != null)
                                {
                                    TxtTotTrimQty.Tag = Dr["Rowid"].ToString();
                                    Grid_Data();
                                }
                            }   
                            //}                          
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["SIZE"].Index)
                        {
                            if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                                {
                                    //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Size", "Select Size, SizeID  From Size Order by Size  ", String.Empty, 250);
                                    Dr = Tool.Selection_Tool_Except_New("DESCR", this, 30, 70, ref Dt1, SelectionTool_Class.ViewType.NormalView, "Size", "Select Distinct Size, SizeID , '" + GridTrim["DESCR", GridTrim.CurrentCell.RowIndex].Value.ToString() + "' + '-' + Cast(SizeID as Varchar(20)) DESCR From Size Where SizeID is Not Null and Size Not Like '%ZZZ%'  Order by Size", String.Empty, 250);
                                    if (Dr != null)
                                    {
                                        GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                                        GridTrim["SIZE_ID", GridTrim.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                                        GridTrim["DESCR", GridTrim.CurrentCell.RowIndex].Value = GridTrim["ITEM_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + "-" + GridTrim["COLOR_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + "-" + Dr["SizeID"].ToString();                                        
                                        Txt_Trim.Text = Dr["SIZE"].ToString();
                                    }
                                }                            
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["UOM"].Index)
                        {
                            if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Size", "Select Size, SizeID  From Size Order by Size  ", String.Empty, 250);
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Uom", "Select Uom, UomID From Uom_MAster", String.Empty, 250);
                                if (Dr != null)
                                {
                                    GridTrim["UOM", GridTrim.CurrentCell.RowIndex].Value = Dr["UOM"].ToString();
                                    GridTrim["UOM_ID", GridTrim.CurrentCell.RowIndex].Value = Dr["UOMID"].ToString();
                                    GridTrim["CONS_UOM", GridTrim.CurrentCell.RowIndex].Value = Dr["UOM"].ToString();
                                    GridTrim["CONS_UOM_ID", GridTrim.CurrentCell.RowIndex].Value = Dr["UOMID"].ToString();  
                                    Txt_Trim.Text = Dr["UOM"].ToString();
                                }
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["CONS_UOM"].Index)
                        {
                            if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Size", "Select Size, SizeID  From Size Order by Size  ", String.Empty, 250);
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Uom", "Select Uom, UomID From Uom_MAster", String.Empty, 250);
                                if (Dr != null)
                                {                                  
                                    GridTrim["CONS_UOM", GridTrim.CurrentCell.RowIndex].Value = Dr["UOM"].ToString();
                                    GridTrim["CONS_UOM_ID", GridTrim.CurrentCell.RowIndex].Value = Dr["UOMID"].ToString();
                                    Txt_Trim.Text = Dr["UOM"].ToString();
                                }
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["PO_UOM"].Index)
                        {                            
                            e.Handled = true;
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select UOM", "select UOM, Conv, Calc_Type,  UOMID From Project_Uom_Settings_Fn(" + GridTrim["UOM_ID", GridTrim.CurrentCell.RowIndex].Value + ") ", String.Empty, 100, 100, 100, 100);


                            if (Dr != null)
                            {
                                GridTrim["PO_UOM", GridTrim.CurrentCell.RowIndex].Value = Dr["UOM"].ToString();
                                GridTrim["UOMID_PO", GridTrim.CurrentCell.RowIndex].Value = Dr["UOMID"].ToString();
                                GridTrim["CONV_VAL", GridTrim.CurrentCell.RowIndex].Value = Dr["Conv"].ToString();
                                GridTrim["Calc_Type", GridTrim.CurrentCell.RowIndex].Value = Dr["Calc_Type"].ToString();
                                Txt_Trim.Text = Dr["UOM"].ToString();
                                Calculate_Item();
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["COLOR"].Index)
                        {                           
                                if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                                {
                                    
                                        Dr = Tool.Selection_Tool(this, 30, 70,  SelectionTool_Class.ViewType.NormalView, "Color", "Select Distinct Color, ColorID , '" + GridTrim["DESCR", GridTrim.CurrentCell.RowIndex].Value.ToString() + "' + '-' + Cast(ColorID as Varchar(20)) DESCR From color Where Color is Not Null and Color Not Like '%ZZZ%'  Order by Color", String.Empty, 250);
                                    
                                    
                                    if (Dr != null)
                                    {
                                        GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                                        GridTrim["COLOR_ID", GridTrim.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();
                                        GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value ="";
                                        GridTrim["DESCR", GridTrim.CurrentCell.RowIndex].Value = GridTrim["ITEM_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + "-" + Dr["ColorID"].ToString();                                        
                                        Txt_Trim.Text = Dr["COLOR"].ToString();
                                    }
                                }                          
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["Currency"].Index)
                        {
                            if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {

                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Currency", "select Abbreviation Currency, exchangeRate Ex_Rate, currencyid CID from fiterp1314.dbo.currency", String.Empty, 250, 100);


                                if (Dr != null)
                                {
                                    GridTrim["Currency", GridTrim.CurrentCell.RowIndex].Value = Dr["Currency"].ToString();
                                    GridTrim["Ex_Rate", GridTrim.CurrentCell.RowIndex].Value = Dr["Ex_Rate"].ToString();
                                    GridTrim["CID", GridTrim.CurrentCell.RowIndex].Value = Dr["CID"].ToString();
                                    Txt_Trim.Text = Dr["Currency"].ToString();
                                }
                            }
                        }                    
                    }                
               Total_Count();               
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

        void Txt_Comm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (TxtBuyer.Text.Trim() == String.Empty)
                    {
                        MessageBox.Show("Invalid Buyer Name", "Gainup");
                        TxtBuyer.Focus();
                        return;
                    }
                    if (TxtProjNo.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Invalid OCN NO", "Gainup");
                        TxtProjNo.Focus();
                        return;
                    }                    
                    else if (GridComm.CurrentCell.ColumnIndex == GridComm.Columns["COMM"].Index)
                    {
                        Dr = Tool.Selection_Tool_Except_New("COMM", this, 30, 70, ref Dt3, SelectionTool_Class.ViewType.NormalView, "Commercial", "Select A.Commercial COMM, A.commercialid CommID From CommercialMas A  Order by A.Commercial ", String.Empty, 250);
                        if (Dr != null)
                        {
                            GridComm["COMM", GridComm.CurrentCell.RowIndex].Value = Dr["COMM"].ToString();
                            GridComm["COMM_ID", GridComm.CurrentCell.RowIndex].Value = Dr["CommID"].ToString();
                            GridComm["TOT_QTY", GridComm.CurrentCell.RowIndex].Value = "0.00";
                            GridComm["GRS_RATE", GridComm.CurrentCell.RowIndex].Value = "0.000";
                            GridComm["GRS_AMOUNT", GridComm.CurrentCell.RowIndex].Value = "0.00";
                            GridComm["PUR_RATE", GridComm.CurrentCell.RowIndex].Value = "0.00";
                            GridComm["PUR_AMOUNT", GridComm.CurrentCell.RowIndex].Value = "0.00";
                            GridComm["FLAG", GridComm.CurrentCell.RowIndex].Value = "F";
                            Txt_Comm.Text = Dr["COMM"].ToString();
                        }
                    }                  
                }
                Total_Count();
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

     
        void Txt_Proc_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                   if (e.KeyCode == Keys.Down)
                    {
                        if (TxtBuyer.Text.Trim() == String.Empty)
                        {
                            MessageBox.Show ("Invalid Buyer Name", "Gainup");
                            TxtBuyer.Focus();
                            return;
                        }                        
                        if (TxtProjNo.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("Invalid OCN NO", "Gainup");
                            TxtProjNo.Focus();                           
                            return;
                        }
                                             
                        else if (GridProc.CurrentCell.ColumnIndex == GridProc.Columns["PROCESS"].Index)
                        {
                            Dr = Tool.Selection_Tool_Except_New("PROCESS", this, 30, 70, ref Dt3, SelectionTool_Class.ViewType.NormalView, "PROCESS", "Select A.PROCESS PROCESS, A.PROCESSID PROC_ID From PROCESS A  Order by A.PROCESS", String.Empty, 250);
                            if (Dr != null)
                            {
                                GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value = Dr["PROCESS"].ToString();
                                GridProc["PROC_ID", GridProc.CurrentCell.RowIndex].Value = Dr["PROC_ID"].ToString();
                                GridProc["TOT_QTY", GridProc.CurrentCell.RowIndex].Value = "0.00";
                                GridProc["GRS_RATE", GridProc.CurrentCell.RowIndex].Value = "0.0000";
                                GridProc["GRS_AMOUNT", GridProc.CurrentCell.RowIndex].Value = "0.00";
                                GridProc["PUR_RATE", GridProc.CurrentCell.RowIndex].Value = "0.00";
                                GridProc["PUR_AMOUNT", GridProc.CurrentCell.RowIndex].Value = "0.00";
                                GridProc["TAX_PER", GridProc.CurrentCell.RowIndex].Value = "0.00";
                                GridProc["FLAG", GridProc.CurrentCell.RowIndex].Value = "F";
                                Txt_Proc.Text = Dr["PROCESS"].ToString();
                            }                            
                        }                     
                    }                     
               Total_Count();               
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

        private void GridTrim_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if(GridTrim.Rows.Count >=2)                                                
                {
                  
                    MyBase.Row_Number(ref GridTrim);                                                
                  
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridComm_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (GridComm.Rows.Count >= 2)
                {
                    MyBase.Row_Number(ref GridComm);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridTrim_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref GridTrim);
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridComm_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref GridComm);
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridTrim_MouseDoubleClick(object sender, MouseEventArgs e)
        {            
            try
            {
                 if(GridTrim["FLAG", GridTrim.CurrentCell.RowIndex].Value.ToString() == "F" || GridTrim["FLAG", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)                 
                 {
                    GridTrim.Focus();
                     if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {                             
                                 Dt1.Rows.RemoveAt(GridTrim.CurrentCell.RowIndex);                             
                     }
                     
                 }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }


         private void GridTrim_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
             try
            {     
                 return;
                if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["ITEM"].Index)
                {
                   // Dt1.AcceptChanges();
                    GridTrim.CurrentCell = GridTrim["ITEM", GridTrim.CurrentCell.RowIndex];
                    GridTrim.Focus();
                    GridTrim.BeginEdit(true);   
                    return;
                }             
             }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridComm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (GridComm["FLAG", GridComm.CurrentCell.RowIndex].Value.ToString() == "F" || GridComm["FLAG", GridComm.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                    GridComm.Focus();
                        if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                        //listBox1.Items.Add(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString());
                            Dt3.Rows.RemoveAt(GridComm.CurrentCell.RowIndex);
                        }
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void GridProc_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if(GridProc.Rows.Count >=2)
                {
                    MyBase.Row_Number(ref GridProc);                                                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      

        private void GridProc_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                if(GridProc.Rows.Count >=2)
                {
                    MyBase.Row_Number(ref GridProc);
                    Total_Count();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridProc_MouseDoubleClick(object sender, MouseEventArgs e)
        {            
            try
            {
                if (GridProc["FLAG", GridTrim.CurrentCell.RowIndex].Value.ToString() == "F" || GridProc["FLAG", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                {
                    GridProc.Focus();
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Dt2.Rows.RemoveAt(GridProc.CurrentCell.RowIndex);
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
                TxtTotTrimQty.Text = MyBase.Sum(ref GridTrim, "TOT_QTY", "ITEM");
                TxtTotTrmPurAmt.Text = MyBase.Sum(ref GridTrim, "PUR_AMOUNT", "ITEM");
                TxtTotProAmt.Text = MyBase.Sum(ref GridProc, "PUR_AMOUNT", "PROCESS");
                TxtTotProQty.Text = MyBase.Sum(ref GridProc, "TOT_QTY", "PROCESS");
                TxtTotComAmt.Text = MyBase.Sum(ref GridComm, "PUR_AMOUNT", "COMM");
                TxtTotComQty.Text = MyBase.Sum(ref GridComm, "TOT_QTY", "COMM");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Total_Cost_Calc()
        {
            try
            {      
                double ProCost=0;
                if (GridTrim.Rows.Count >= 1 || GridProc.Rows.Count >= 1)
                {
                TxtTrimCost.Text = MyBase.Sum(ref GridTrim, "PUR_AMOUNT", "ITEM");
                TxtProcCost.Text = MyBase.Sum(ref GridProc, "PUR_AMOUNT", "PROCESS");                
                TxtCommCost.Text = MyBase.Sum(ref GridComm, "PUR_AMOUNT", "COMM");
                TxtTaxAmt.Text = MyBase.Sum(ref GridTrim, "TAX_AMOUNT", "ITEM");
                ProCost =  Convert.ToDouble(TxtProcCost.Text.ToString()) + Convert.ToDouble(0);
                TxtProcCost.Text =  ProCost.ToString();                
                if(TxtProfit.Text.ToString().Trim() == String.Empty)
                {
                    TxtProfit.Text = "0";
                }
                if (TxtProfitInd.Text.ToString().Trim() == String.Empty)
                {
                    TxtProfitInd.Text = "0";
                }
                if (TxtProfitExp.Text.ToString().Trim() == String.Empty)
                {
                    TxtProfitExp.Text = "0";
                } if (TxtDBInd.Text.ToString().Trim() == String.Empty)
                {
                    TxtDBInd.Text = "0";
                }
                if (TxtDBExp.Text.ToString().Trim() == String.Empty)
                {
                    TxtDBExp.Text = "0";
                }
                TxtTotalCost.Text = (((Convert.ToDouble(0) + Convert.ToDouble(TxtTrimCost.Text.ToString()) + Convert.ToDouble(TxtProcCost.Text.ToString()) + Convert.ToDouble(TxtCommCost.Text.ToString()) + Convert.ToDouble(0)) * (Convert.ToDouble(TxtProfit.Text.ToString()) / 100)) + ((Convert.ToDouble(0) + Convert.ToDouble(TxtTrimCost.Text.ToString()) + Convert.ToDouble(TxtProcCost.Text.ToString()) + Convert.ToDouble(TxtCommCost.Text.ToString()) + Convert.ToDouble(0)))).ToString();
                TxtTaxPer.Text = Math.Round((Convert.ToDouble(Convert.ToDouble(TxtTaxAmt.Text.ToString()) / Convert.ToDouble(TxtTotalCost.Text.ToString())) * 100),2).ToString();
                TxtPackCost.Text = Math.Round((Convert.ToDouble(TxtTotalCost.Text.ToString()) / Convert.ToDouble(TxtQty.Text.ToString())),2).ToString();
                TxtIndRs.Text = TxtPackCost.Text.ToString();
                TxtExpRs.Text = Math.Round((Convert.ToDouble(TxtPackCost.Text.ToString()) / Convert.ToDouble(TxtExRate.Text.ToString())),4).ToString();
                TxtSalePriceExp.Text = Math.Round(((Convert.ToDouble(TxtQty.Text.ToString()) / Convert.ToDouble(TxtQty.Text.ToString())) / Convert.ToDouble(TxtExRate.Text.ToString())), 4).ToString();          
                TxtSalePriceInd.Text = Math.Round((Convert.ToDouble(TxtSalePriceExp.Text.ToString()) * Convert.ToDouble(TxtExRate.Text.ToString())),4).ToString();
                TxtValueInd.Text = Math.Round((Convert.ToDouble(TxtSalePriceInd.Text.ToString()) - Convert.ToDouble(TxtIndRs.Text.ToString())), 4).ToString();
                TxtValueExp.Text = Math.Round((Convert.ToDouble(TxtSalePriceExp.Text.ToString()) - Convert.ToDouble(TxtExpRs.Text.ToString())), 4).ToString();
                if (Convert.ToDouble(TxtSalePriceInd.Text.ToString()) != 0)
                {
                    TxtProfitInd.Text = Math.Round((Convert.ToDouble(TxtValueInd.Text.ToString()) / Convert.ToDouble(TxtSalePriceInd.Text.ToString())) * 100, 4).ToString();
                    TxtProfitExp.Text = Math.Round((Convert.ToDouble(TxtValueExp.Text.ToString()) / Convert.ToDouble(TxtSalePriceExp.Text.ToString())) * 100, 4).ToString();
                }
                else
                {
                    TxtProfitInd.Text = "0";
                    TxtProfitExp.Text = "0";
                }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      

        private void GridComm_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Total_Count();
                    TxtTotComAmt.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      



        private void FrmProjectPlanningEntry_Load(object sender, EventArgs e)
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

        private void FrmProjectPlanningEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {
               
                if (this.ActiveControl.Name == "TxtProfit" || this.ActiveControl.Name == "TxtDBInd" || this.ActiveControl.Name == "TxtDBExp")
                {
                    MyBase.Valid_Decimal((TextBox)this.ActiveControl, e);
                }
                else if (this.ActiveControl.Name != String.Empty && this.ActiveControl.Name != "TxtRemarks")
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
            }
        }

        private void FrmProjectPlanningEntry_KeyDown(object sender, KeyEventArgs e)
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
                    else if (this.ActiveControl.Name == "TxtProjNo")
                    {
                        if(GridTrim.Rows.Count >0)
                        {
                            tabControl1.SelectTab(tabPage2);
                            GridTrim.CurrentCell = GridTrim["ITEM", 0];
                            GridTrim.Focus();
                            GridTrim.BeginEdit(true);
                            e.Handled = true;
                            return;         
                        }
                    } 
                   
                    else if (this.ActiveControl.Name == "TxtTotTrmPurAmt")
                    {
                        if(GridProc.Rows.Count >0)
                        {
                            tabControl1.SelectTab(tabPage3);
                            GridProc.CurrentCell = GridProc["PROCESS", 0];
                            GridProc.Focus();
                            GridProc.BeginEdit(true);
                            e.Handled = true;
                            return;         
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtTotProAmt")
                    {
                        if (GridComm.Rows.Count > 0)
                        {
                            tabControl1.SelectTab(tabPage4);
                            GridComm.CurrentCell = GridComm["COMM", 0];
                            GridComm.Focus();
                            GridComm.BeginEdit(true);
                            e.Handled = true;
                            return;
                        }
                    }
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (MyParent._New == true)
                    {
                        if (this.ActiveControl.Name == "TxtProjNo")
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select  A.Order_No,  A.Order_Date, A.Proj_Name, A.Proj_Activity_Name,  A.Uom, A.Party, A.Employee, A.Qty, A.Conv_Qty, A.OrdeR_ID, A.Proj_Activity_ID, A.Proj_Type_ID, A.Party_Code, A.Uom_ID, A.Amount  From Project_Bom_Item_Fn() A Left Join Project_Planning_Master B On A.ORder_ID = B.Order_ID and A.Proj_Activity_ID = B.Proj_Activity_ID  Where B.RowID IS Null and A.Complete_Order = 'N' and A.Cancel_ORder = 'N'  and A.Company_Code = " + MyParent.CompCode + " and A.PArty_Code = " + MyParent.Proj_Login_Code + " Order by A.Order_No desc, A.Proj_Activity_Name ", String.Empty, 100, 140, 100, 120, 140, 100);
                                if (Dr != null)
                                {                                   
                                    TxtProjNo.Text = Dr["Order_No"].ToString();
                                    TxtProjNo.Tag = Dr["Order_ID"].ToString();
                                    DtpODate.Value = Convert.ToDateTime(Dr["Order_Date"].ToString());
                                    TxtActivity.Text = Dr["Proj_Activity_Name"].ToString();
                                    TxtActivity.Tag = Dr["Proj_Activity_ID"].ToString();
                                    TxtBuyer.Text =  Dr["Party"].ToString();
                                    TxtBuyer.Tag =  Dr["Party_Code"].ToString();
                                    TxtUom.Text =  Dr["UOM"].ToString();
                                    TxtUom.Tag   = Dr["UOM_ID"].ToString();
                                    TxtQty.Text = Dr["Qty"].ToString();
                                    TxtProjName.Text = Dr["Proj_Name"].ToString();
                                    TxtProjName.Tag = Dr["Proj_Type_ID"].ToString();  
                                    TxtCurrency.Text = "INR";
                                    TxtExRate.Text = "1";
                                    Grid_Data();
                                    Total_Count();
                                    tabControl1.SelectTab(tabPage2);
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

          private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {               
               if (tabControl1.SelectedTab == tabControl1.TabPages[0])
                {
                   GridTrim.AllowUserToAddRows = true;
                   if (GridTrim.Rows.Count > 0)
                   {
                       GridTrim.CurrentCell = GridTrim["ITEM", 0];
                       GridTrim.Focus();
                       GridTrim.BeginEdit(true);
                   }
                   return;
                }
               else if (tabControl1.SelectedTab == tabControl1.TabPages[1])
               {                  
                   GridProc.AllowUserToAddRows = true;
                   if (GridProc.Rows.Count > 0)
                   {
                       GridProc.CurrentCell = GridProc["PROCESS", 0];
                       GridProc.Focus();
                       GridProc.BeginEdit(true);
                   }
                   return;
               }
               else if (tabControl1.SelectedTab == tabControl1.TabPages[2])
               {
                   GridComm.AllowUserToAddRows = true;
                   if (GridComm.Rows.Count > 0)
                   {
                       GridComm.CurrentCell = GridComm["COMM", 0];
                       GridComm.Focus();
                       GridComm.BeginEdit(true);
                   }
                   return;
               }  
              
               else if (tabControl1.SelectedTab == tabControl1.TabPages[3])
               {
                   Total_Cost_Calc();                   
                   return;                   
               }
              
             
                Total_Count();
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
                    DtpDate.Value = MyBase.GetServerDate();
                    DtpDate.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        }

        private void GBMain_Enter(object sender, EventArgs e)
        {

        }

     

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void TxtVerifiedBy_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtDept_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void GridProc_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Total_Count();
                    TxtTotProAmt.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        
        
        private void Arrow_Buyer_Click(object sender, EventArgs e)
        {
            try
            {
                 TxtProjNo.Focus();
                 SendKeys.Send("{Down}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void TxtBuyer_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TxtCurrency_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void GridTrim_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Total_Count();
                    TxtTotTrmPurAmt.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridTrim_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["ACCESS_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() != "SPECIAL")
                    {
                        if (GridTrim.Rows.Count > 2)
                        {
                            for (int k = 0; k < GridTrim.Rows.Count - 2; k++)
                            {
                                if ((k != GridTrim.CurrentCell.RowIndex) && (GridTrim["ITEM", k].Value.ToString()) == GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() && (GridTrim["COLOR", k].Value.ToString()) == (GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value.ToString()) && (GridTrim["SIZE", k].Value.ToString()) == GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value.ToString())
                                {
                                    if (GridTrim["ACCESS_TYPE", k].Value.ToString() != "SPECIAL")
                                    {
                                        MessageBox.Show("Already  ITEM , COLOR & SIZE are Available", "Gainup");
                                        GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value = "";
                                        GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value = "";
                                        GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value = "";                                        
                                        GridTrim["TOT_QTY", GridTrim.CurrentCell.RowIndex].Value = 0;
                                        k = GridTrim.Rows.Count;
                                        Total_Count();
                                        GridTrim.CurrentCell = GridTrim["ITEM", GridTrim.CurrentCell.RowIndex];
                                        GridTrim.Focus();
                                        GridTrim.BeginEdit(true);
                                        e.Handled = true;
                                        return;
                                    }
                                }
                            }

                        }
                    }

                    if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["Rate"].Index)
                    {
                        if (GridTrim["Rate", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {

                            GridTrim["Rate", GridTrim.CurrentCell.RowIndex].Value = 0.0000;
                            GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value = 0.0000;
                            GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                        }
                        else if (Convert.ToDouble(GridTrim["Rate", GridTrim.CurrentCell.RowIndex].Value) > 0)
                        {
                            GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["Ex_Rate", GridTrim.CurrentCell.RowIndex].Value) * Math.Round(Convert.ToDouble(GridTrim["Rate", GridTrim.CurrentCell.RowIndex].Value), 5);
                        }
                        else
                        {
                            MessageBox.Show("Invalid Rate");
                            GridTrim["Rate", GridTrim.CurrentCell.RowIndex].Value = 0.0000;
                            GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value = 0.0000;
                            GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                            GridTrim.CurrentCell = GridTrim["Rate", GridTrim.CurrentCell.RowIndex];
                            GridTrim.Focus();
                            GridTrim.BeginEdit(true);
                            e.Handled = true;
                            return;
                        }
                    } 

                    else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["GRS_RATE"].Index)
                    {
                        if (GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {

                            GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value = 0.0000;
                            GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                        }
                        else if (Convert.ToDouble(GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value) > 0)
                        {
                            GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["TOT_QTY", GridTrim.CurrentCell.RowIndex].Value) * Math.Round(Convert.ToDouble(GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value),5);
                        }
                        else
                        {
                            MessageBox.Show("Invalid GRS_RATE");
                            GridTrim["GRS_RATE", GridTrim.CurrentCell.RowIndex].Value = 0.0000;
                            GridTrim["GRS_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                            GridTrim.CurrentCell = GridTrim["Rate", GridTrim.CurrentCell.RowIndex];
                            GridTrim.Focus();
                            GridTrim.BeginEdit(true);
                            e.Handled = true;
                            return;
                        }
                    }                    
                    else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["TAX_PER"].Index)
                    {
                        if (GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                        }
                        else if (Convert.ToDouble(GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex].Value.ToString()) == 0)
                        {
                            GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex].Value = 0;
                           //// MessageBox.Show("Invalid Tax");
                           // GridTrim.CurrentCell = GridTrim["TAX_PER", GridTrim.CurrentCell.RowIndex];
                           // GridTrim.Focus();
                           // GridTrim.BeginEdit(true);
                           // e.Handled = true;
                           // return;
                        }
                    }
                }                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridProc_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        if (GridProc.Rows.Count > 2)
                        {
                            for (int k = 0; k < GridProc.Rows.Count - 2; k++)
                            {
                                if ((k != GridProc.CurrentCell.RowIndex) && (GridProc["PROCESS", k].Value.ToString()) == GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value.ToString())
                                {
                                    MessageBox.Show("Already Process Available", "Gainup");
                                    GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value = "";                                   
                                    k = GridProc.Rows.Count;
                                    Total_Count();
                                    GridProc.CurrentCell = GridProc["PROCESS", GridProc.CurrentCell.RowIndex];
                                    GridProc.Focus();
                                    GridProc.BeginEdit(true);
                                    e.Handled = true;
                                    return;
                                }
                            }
                        }
                    }

                    if (GridProc.CurrentCell.ColumnIndex == GridProc.Columns["GRS_RATE"].Index)
                    {
                        if (GridProc["GRS_RATE", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {

                            GridProc["GRS_RATE", GridProc.CurrentCell.RowIndex].Value = 0.00;
                            GridProc["GRS_AMOUNT", GridProc.CurrentCell.RowIndex].Value = 0.00;
                        }
                        else if (Convert.ToDouble(GridProc["GRS_RATE", GridProc.CurrentCell.RowIndex].Value) > 0)
                        {

                            GridProc["GRS_AMOUNT", GridProc.CurrentCell.RowIndex].Value = Convert.ToDouble(GridProc["TOT_QTY", GridProc.CurrentCell.RowIndex].Value) * Math.Round(Convert.ToDouble(GridProc["GRS_RATE", GridProc.CurrentCell.RowIndex].Value), 5);

                        }
                        else
                        {
                            MessageBox.Show("Invalid RATE");
                            GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value = 0.00;
                            GridProc["PRO_AMOUNT", GridProc.CurrentCell.RowIndex].Value = 0.00;
                            GridProc.CurrentCell = GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex];
                            GridProc.Focus();
                            GridProc.BeginEdit(true);
                            e.Handled = true;
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

        private void GridComm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (GridComm["COMM", GridComm.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridComm.CurrentCell.ColumnIndex == GridComm.Columns["GRS_RATE"].Index)
                    {
                        if (GridComm["GRS_RATE", GridComm.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {

                            GridComm["GRS_RATE", GridComm.CurrentCell.RowIndex].Value = 0.00;
                            GridComm["GRS_AMOUNT", GridComm.CurrentCell.RowIndex].Value = 0.00;
                        }                        
                         
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridProc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

      
        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void myTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void myTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void myTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void GBSum_Enter(object sender, EventArgs e)
        {

        }

        private void myTextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void myTextBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtProfit_Leave(object sender, EventArgs e)
        {
            try
            {
                Total_Cost_Calc();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtCurrency_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void TxtExRate_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtExpRs_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtProfit_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtProfitInd_Leave(object sender, EventArgs e)
        {
            try
            {
                Total_Cost_Calc();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtProfitExp_Leave(object sender, EventArgs e)
        {
            try
            {
                Total_Cost_Calc();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtDBInd_Leave(object sender, EventArgs e)
        {
            try
            {
                TxtDBExp.Text = TxtDBInd.Text.ToString();
                Total_Cost_Calc();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtDBExp_Leave(object sender, EventArgs e)
        {
            try
            {
                TxtDBInd.Text = TxtDBExp.Text.ToString();
                Total_Cost_Calc();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sendEMailThroughOUTLOOK_Send(String toid, String ccid, String subject, String Body, params String[] FilePath)
        {
            try
            {
                Int32 ArrayIndex = 0;
                String AttachmentName = String.Empty;

                Outlook.Application oApp = new Outlook.Application();
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                oMsg.HTMLBody = Body.ToString();
                String Text = "<html><body><basefont size=4 font face= courier new>" + Body.ToString() + "</font></body></html>";
                Text = Text + "<Br>";
                Text = Text + "<Br>";
                Text = Text + "<Br>";
                Text = Text + "System Generated Mail.";
                oMsg.HTMLBody = Text;
                oMsg.Subject = subject;
                oMsg.To = toid;
                //Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                //Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(toid);

                foreach (String Str in FilePath)
                {
                    ArrayIndex++;
                    AttachmentName = "Attachment" + ArrayIndex;
                    oMsg.Attachments.Add(Str, Outlook.OlAttachmentType.olByValue, ArrayIndex, (Object)AttachmentName);
                }

                if (ccid.Trim() != String.Empty)
                {
                    oMsg.CC = ccid;
                }
                //oRecip.Resolve();
                //oMsg.Display(false);
                oMsg.Send();
                //oRecip = null;
                //oRecips = null;
                oMsg = null;
                oApp = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }           
      
       
    }
}
