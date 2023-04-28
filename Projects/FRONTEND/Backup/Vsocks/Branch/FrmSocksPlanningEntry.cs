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
using System.IO;

namespace Accounts
{
    public partial class FrmSocksPlanningEntry : Form,Entry  
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataTable Dt2 = new DataTable();
        DataTable Dt3 = new DataTable();
        DataTable Dt4 = new DataTable();
        DataTable[] DtTrims;
        DataGrid dgv = new DataGrid();
        DataRow Dr;        
        Int64 Code;
        Int32 C=0; 
        TextBox Txt = null;        
        TextBox Txt_Trim = null;   
        TextBox Txt_Proc = null;
        TextBox Txt_Comm = null;  
        TextBox Txt_Spcl = null;
        DataTable[] DtImg;
        String[] Queries;
        String Str;             
        String Str1;     
        String Str2;
        String Str3; 
        String Str4; 
        public FrmSocksPlanningEntry()
        {
            InitializeComponent();
        }
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);  
                ChkCopy.Enabled = true;
                TxtYarnCost.Text = " "; TxtTrimCost.Text = " "; TxtProcCost.Text = " "; TxtCommCost.Text = " "; TxtSpclReqCost.Text = " "; TxtProfit.Text = " "; TxtTotalCost.Text = " "; TxtPackCost.Text = " "; TxtExRate.Text = " ";  TxtIndRs.Text = " "; TxtExpRs.Text = " "; TxtSalePriceInd.Text = " "; TxtDBInd.Text = " "; TxtProfitInd.Text = " "; TxtValueInd.Text = " "; TxtSalePriceExp.Text = " "; TxtDBExp.Text = " "; TxtProfitExp.Text = " "; TxtValueExp.Text= " ";  
               // DtpDate.Value = MyBase.GetServerDate();
                DtTrims= new DataTable[300];
                ChkCopy.Checked = false;
                Grid_Data();                
                DataTable TDt = new DataTable();
                MyBase.Load_Data("Select IsNull(Max(Entry_No),0)+1 Entry_No From Socks_Planning_Master ", ref TDt);
                TxtENo.Text  = TDt.Rows[0][0].ToString();
                TxtOcnNo.Focus();
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
                TxtYarnCost.Text = " "; TxtTrimCost.Text = " "; TxtProcCost.Text = " "; TxtCommCost.Text = " "; TxtSpclReqCost.Text = " "; TxtProfit.Text = " "; TxtTotalCost.Text = " "; TxtPackCost.Text = " "; TxtExRate.Text = " ";  TxtIndRs.Text = " "; TxtExpRs.Text = " "; TxtSalePriceInd.Text = " "; TxtDBInd.Text = " "; TxtProfitInd.Text = " "; TxtValueInd.Text = " "; TxtSalePriceExp.Text = " "; TxtDBExp.Text = " "; TxtProfitExp.Text = " "; TxtValueExp.Text= " ";  
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Planning - Edit", "Select Distinct A.Order_No, B.Entry_No,  B.Effect_From,  A.Item, A.Party, A.Employee, Bom_Qty_Plan BOM_Qty, Buyer_Qty_Plan Buyer_Qty, B.Tot_Yarn_Cons, B.Tot_Trim_Cons, B.OrdeR_ID, B.Item_ID, A.Order_Date , B.REmarks, A.Party_Code, A.Amount, A.Ex_Rate, A.Currency, C.Profit, C.Profit_Ind, C.Profit_Exp, C.DB_Ind, C.DB_Exp, B.RowID, A.Buy_Qty From Socks_Bom_Item_Fn() A Inner Join Socks_Planning_Master B On A.ORder_ID = B.Order_ID and A.ItemID = B.Item_ID Left Join Socks_Planning_Summary_Details C On B.RowID = C.Master_ID Order by A.Order_No desc ", String.Empty, 150, 80, 100, 100, 140, 100, 100, 100, 100, 100);                
                if (Dr != null)
                {
                    Fill_Datas(Dr);                                        
                    tabControl1.SelectTab(tabPage1);
                    GridYarn.CurrentCell = GridYarn["DYE_MODE", 0];
                    GridYarn.Focus();
                    GridYarn.BeginEdit(true);
                }
                if(MyParent.UserCode == 37 || MyParent.UserCode == 11 || MyParent.UserCode == 1)
                {
                    groupBox5.Visible = true;
                    tabControl1.SelectTab(tabPage6);
                }
                if(MyParent.UserCode == 1)
                {
                    groupBox6.Visible = true;
                    tabControl1.SelectTab(tabPage6);
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
                TxtOcnNo.Text = Dr["ORder_No"].ToString();  
                TxtOcnNo.Tag= Dr["OrdeR_ID"].ToString();  
                TxtItem.Text =  Dr["Item"].ToString();  
                TxtItem.Tag = Dr["Item_ID"].ToString();  
                DtpODate.Value = Convert.ToDateTime(Dr["Order_Date"]);
                TxtBuyer.Text = Dr["PArty"].ToString();
                TxtBuyer.Tag = Dr["PArty_Code"].ToString();
                TxtBomQty.Text = Dr["Bom_Qty"].ToString();
                TxtOrdQty.Text = Dr["Buyer_Qty"].ToString();                                
                TxtOrdQty.Tag = Dr["Buy_Qty"].ToString();
                TxtEmpl.Text = Dr["Employee"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();                
                TxtTotWeight.Text = Dr["Tot_Yarn_Cons"].ToString();
                TxtTotTrimQty.Text = Dr["Tot_Trim_Cons"].ToString(); 
                TxtExRate.Text = Dr["Ex_Rate"].ToString(); 
                TxtBomQty.Tag =  Dr["Amount"].ToString();
                TxtCurrency.Text = Dr["Currency"].ToString();
                Grid_Data();
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
                if(MyParent.UserCode == 37)
                {
                    return;
                }
                Int32 Array_Index = 0;
                Total_Count();
                Total_Cost_Calc();              
                if (TxtItem.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Item", "Gainup");
                    TxtItem.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtOcnNo.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Order No", "Gainup");
                    TxtOcnNo.Focus();
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
                
                 //if (MyBase.Validate_Date_For_Entry(Convert.ToDateTime(Dr["Order_Date"].ToString()) , 30, Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", DtpDate.Value))) == false && Convert.ToInt16(MyParent.UserCode) != 1)                                
                 //{
                 //   MessageBox.Show("Date Locked, Only 30 Days are Allowed From Order Creation");
                 //   TxtOcnNo.Focus();
                 //   MyParent.Save_Error = true;
                 //   return;
                 //}
                if (TxtTotCons.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotCons.Text) == 0 || TxtTotWeight.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotWeight.Text) == 0 || TxtTotYrnPurAmt.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotYrnPurAmt.Text) == 0)
                {
                    tabControl1.SelectTab(tabPage1);
                    MessageBox.Show("Invalid Yarn Details ", "Gainup");
                    GridYarn.CurrentCell = GridYarn["DYE_MODE", 0];
                    GridYarn.Focus();
                    GridYarn.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                if(TxtOcnNo.Text.ToString().Contains("OCN"))
                {
                    if (TxtTotTrimCons.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotTrimCons.Text) == 0 || TxtTotTrimQty.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotTrimQty.Text) == 0 || TxtTotTrmPurAmt.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotTrmPurAmt.Text) == 0)
                    {
                        tabControl1.SelectTab(tabPage2);
                        MessageBox.Show("Invalid Trim Details ", "Gainup");
                        GridTrim.CurrentCell = GridTrim["ITEM", 0];
                        GridTrim.Focus();
                        GridTrim.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                
                if (TxtTotProQty.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotProQty.Text) == 0 || TxtTotProAmt.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotProAmt.Text) == 0)
                {
                    tabControl1.SelectTab(tabPage3);
                    MessageBox.Show("Invalid Process Details ", "Gainup");
                    GridProc.CurrentCell = GridProc["PROCESS", 0];
                    GridProc.Focus();
                    GridProc.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                //if (TxtTotComAmt.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotComAmt.Text) == 0 || TxtTotComQty.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotComQty.Text) == 0)
                //{
                //    tabControl1.SelectTab(tabPage4);
                //    MessageBox.Show("Invalid Commercial Details ", "Gainup");
                //    GridComm.CurrentCell = GridComm["COMM_NAME", 0];
                //    GridComm.Focus();
                //    GridComm.BeginEdit(true);
                //    MyParent.Save_Error = true;
                //    return;
                //}

                for (int i = 0; i < GridYarn.Rows.Count; i++)
                {
                    for (int j = 1; j < GridYarn.Columns.Count - 1; j++)
                    {
                        if (GridYarn[j, i].Value == DBNull.Value || GridYarn[j, i].Value.ToString() == String.Empty || GridYarn[j, i].Value.ToString() == "0")
                        {
                            if (GridYarn["DYE_MODE", i].Value.ToString() == "N" && (GridYarn[j, i].Value == DBNull.Value || GridYarn[j, i].Value.ToString() == String.Empty || GridYarn[j, i].Value.ToString() == "0"))
                            {
                                
                            }
                            else if ((GridYarn["FLAG", i].Value.ToString() == "T" || GridYarn["FLAG", i].Value.ToString() == "S" || GridYarn["FLAG", i].Value.ToString() == "A") && (GridYarn[j, i].Value == DBNull.Value || GridYarn[j, i].Value.ToString() == String.Empty || GridYarn[j, i].Value.ToString() == "0"))
                            {
                                
                            }
                            else
                            {
                                tabControl1.SelectTab(tabPage1);
                                MessageBox.Show("' " + GridYarn.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                GridYarn.CurrentCell = GridYarn[j, i];
                                GridYarn.Focus();
                                GridYarn.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }                      
                    }
                        if (Convert.ToDouble(GridYarn["LOSS_PER", i].Value) < 0 || Convert.ToDouble(GridYarn["LOSS_PER", i].Value) > 10)
                        {
                            tabControl1.SelectTab(tabPage1);
                            MessageBox.Show("Invalid Loss  %, It Must Between 0 To 10 ..!", "Gainup");
                            GridYarn.CurrentCell = GridYarn[1, i];
                            GridYarn.Focus();
                            GridYarn.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }

                        if (Convert.ToDouble(GridYarn["BOM_QTY", i].Value) <= 0 || Convert.ToDouble(GridYarn["BOM_CONS", i].Value) <= 0)
                        {
                            if (Convert.ToDouble(GridYarn["BOM_CONS", i].Value) != Convert.ToDouble(GridYarn["BOM_CONS1", i].Value) && MyParent.Edit == true)
                            {

                            }
                            else
                            {
                                tabControl1.SelectTab(tabPage1);
                                MessageBox.Show("Invalid Bom Cons & Bom Qty..!", "Gainup");
                                GridYarn.CurrentCell = GridYarn[1, i];
                                GridYarn.Focus();
                                GridYarn.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }         
                    
                    if(Convert.ToDouble(GridYarn["PUR_RATE", i].Value) == 0 || (GridYarn["PUR_RATE", i].Value.ToString()) == String.Empty || Convert.ToDouble(GridYarn["PUR_AMOUNT", i].Value) <= 0 || (GridYarn["PUR_AMOUNT", i].Value.ToString()) == String.Empty)
                        {
                            if (Convert.ToDouble(GridYarn["BOM_CONS", i].Value) != Convert.ToDouble(GridYarn["BOM_CONS1", i].Value) && MyParent.Edit == true)
                            {

                            }
                            else
                            {
                                tabControl1.SelectTab(tabPage1);
                                MessageBox.Show("Invalid Yarn Purchase Rate & Amount ..!", "Gainup");
                                GridYarn.CurrentCell = GridYarn[1, i];
                                GridYarn.Focus();
                                GridYarn.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }
                    else
                    {
                         GridYarn["PUR_AMOUNT", i].Value = (Convert.ToDouble(GridYarn["BOM_CONS", i].Value) + Convert.ToDouble(GridYarn["LOSS_WEIGHT", i].Value)) * Convert.ToDouble(GridYarn["PUR_RATE", i].Value);
                    }
                   
                    if (GridYarn["DYE_MODE", i].Value.ToString() == "Y" && (GridYarn["COLOR", i].Value.ToString().ToUpper() != "GREIGE" || GridYarn["COLOR", i].Value.ToString().ToUpper() != "R.WHITE"))
                    {
                        if(Convert.ToDouble(GridYarn["DYE_RATE", i].Value) == 0 || (GridYarn["DYE_RATE", i].Value.ToString()) == String.Empty || Convert.ToDouble(GridYarn["DYE_AMOUNT", i].Value) == 0 || (GridYarn["DYE_AMOUNT", i].Value.ToString()) == String.Empty)
                        {
                            if (Convert.ToDouble(GridYarn["BOM_CONS", i].Value) != Convert.ToDouble(GridYarn["BOM_CONS1", i].Value) && MyParent.Edit == true)
                            {

                            }
                            else
                            {
                                tabControl1.SelectTab(tabPage1);
                                MessageBox.Show("Invalid Dye Rate, Yarn Rate & Amount..!", "Gainup");
                                GridYarn.CurrentCell = GridYarn[1, i];
                                GridYarn.Focus();
                                GridYarn.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }

                        else if (GridYarn["ITEM", i].Value.ToString().ToUpper() == "NYLON")
                        {
                            GridYarn["DYE_COLOR", i].Value =  "R.White";
                            GridYarn["DYE_ITEMID", i].Value =  3343;
                            GridYarn["LOSS_WEIGHT", i].Value = ((Convert.ToDouble(GridYarn["BOM_CONS", i].Value) * (Convert.ToDouble(GridYarn["LOSS_PER", i].Value))/100));                     
                            GridYarn["DYE_AMOUNT", i].Value = (((Convert.ToDouble(GridYarn["BOM_CONS", i].Value) + Convert.ToDouble(GridYarn["LOSS_WEIGHT", i].Value)) * (Convert.ToDouble(GridYarn["DYE_RATE", i].Value))));                     
                        }                        
                        else
                        {
                            GridYarn["DYE_COLOR", i].Value =  "GREIGE";
                            GridYarn["DYE_ITEMID", i].Value =  867;
                            GridYarn["LOSS_WEIGHT", i].Value = ((Convert.ToDouble(GridYarn["BOM_CONS", i].Value) * (Convert.ToDouble(GridYarn["LOSS_PER", i].Value))/100));                     
                            GridYarn["DYE_AMOUNT", i].Value = (((Convert.ToDouble(GridYarn["BOM_CONS", i].Value) + Convert.ToDouble(GridYarn["LOSS_WEIGHT", i].Value)) * (Convert.ToDouble(GridYarn["DYE_RATE", i].Value))));                     
                        }
                    }
                    else
                    {
                           GridYarn["LOSS_PER", i].Value = 0;
                           GridYarn["LOSS_WEIGHT", i].Value = 0.000;
                    }
                }
               
                
                for (int i = 0; i < GridTrim.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < GridTrim.Columns.Count - 1; j++)
                    {
                        if (GridTrim[j, i].Value == DBNull.Value || GridTrim[j, i].Value.ToString() == String.Empty || GridTrim[j, i].Value.ToString() == "0")
                        {
                            if(GridTrim["ACCESS_TYPE", i].Value.ToString() == "GENERAL" && Convert.ToDouble(GridTrim["SAMPLE_ID", i].Value) != 0)
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
  
                    if (GridTrim["PLAN_TYPE", i].Value.ToString() == "/")
                    {
                        GridTrim["REQ_QTY", i].Value = ((Convert.ToDouble(GridTrim["BOM_QTY", i].Value) / (Convert.ToDouble(GridTrim["CONS", i].Value))));                     
                    }
                    else if (GridTrim["PLAN_TYPE", i].Value.ToString() == "*")
                    {
                        GridTrim["REQ_QTY", i].Value = ((Convert.ToDouble(GridTrim["BOM_QTY", i].Value) * (Convert.ToDouble(GridTrim["CONS", i].Value))));                     
                    }
                    else if (GridTrim["PLAN_TYPE", i].Value.ToString() == "M")
                    {
                        GridTrim["REQ_QTY", i].Value = ((Convert.ToDouble(GridTrim["CONS", i].Value))); 
                    }
                    else
                    {
                            tabControl1.SelectTab(tabPage2);
                            MessageBox.Show("Invalid Plan Type ", "Gainup");
                            GridTrim.CurrentCell = GridTrim["PLAN_TYPE", i];
                            GridTrim.Focus();
                            GridTrim.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                    }
                    
                    if(Convert.ToDouble(GridTrim["PUR_RATE", i].Value) == 0 || (GridTrim["PUR_RATE", i].Value.ToString()) == String.Empty || Convert.ToDouble(GridTrim["PUR_AMOUNT", i].Value) == 0 || (GridTrim["PUR_AMOUNT", i].Value.ToString()) == String.Empty)
                    {
                            tabControl1.SelectTab(tabPage2);
                            MessageBox.Show("Invalid Trims Purchase Rate & Amount..!", "Gainup");
                            GridTrim.CurrentCell = GridTrim[1, i];
                            GridTrim.Focus();
                            GridTrim.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                    }
                    else
                    {
                         GridTrim["PUR_AMOUNT", i].Value = Convert.ToDouble(GridTrim["REQ_QTY", i].Value) * Convert.ToDouble(GridTrim["PUR_RATE", i].Value);
                    }                      
                   
                }

                for (int i = 0; i < GridProc.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < GridProc.Columns.Count - 1; j++)
                    {
                        if (GridProc[j, i].Value == DBNull.Value || GridProc[j, i].Value.ToString() == String.Empty || GridProc[j, i].Value.ToString() == "0")
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
                    
                    if(Convert.ToDouble(GridProc["PRO_RATE", i].Value) == 0 || (GridProc["PRO_RATE", i].Value.ToString()) == String.Empty || Convert.ToDouble(GridProc["PRO_AMOUNT", i].Value) == 0 || (GridProc["PRO_AMOUNT", i].Value.ToString()) == String.Empty)
                        {
                            tabControl1.SelectTab(tabPage3);
                            MessageBox.Show("Invalid Process Purchase Rate & Amount..!", "Gainup");
                            GridProc.CurrentCell = GridProc[1, i];
                            GridProc.Focus();
                            GridProc.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    else
                    {
                         GridProc["PRO_AMOUNT", i].Value = Convert.ToDouble(GridProc["REQ_QTY", i].Value) * Convert.ToDouble(GridProc["PRO_RATE", i].Value);
                    }                                         
                }

                for (int i = 0; i < GridComm.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < GridComm.Columns.Count - 1; j++)
                    {
                        if (GridComm[j, i].Value == DBNull.Value || GridComm[j, i].Value.ToString() == String.Empty || GridComm[j, i].Value.ToString() == "0")
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

                    if (Convert.ToDouble(GridComm["RATE", i].Value) == 0 || (GridComm["RATE", i].Value.ToString()) == String.Empty || Convert.ToDouble(GridComm["AMOUNT", i].Value) == 0 || (GridComm["AMOUNT", i].Value.ToString()) == String.Empty)
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
                         if (GridComm["CALC_MODE", i].Value.ToString() == "PER QTY")
                            {
                                GridComm["AMOUNT", i].Value = Convert.ToDouble(GridComm["QTY", i].Value) * Convert.ToDouble(GridComm["RATE", i].Value);
                            }
                            else if (GridComm["CALC_MODE", i].Value.ToString() == "MANUAL")
                            {
                                GridComm["AMOUNT", i].Value =  GridComm["RATE", i].Value;
                            }
                            else if (GridComm["CALC_MODE", i].Value.ToString() == "PERCENTAGE")
                            {
                                if((Convert.ToDouble(GridComm["RATE", i].Value) > 20))
                                {
                                    tabControl1.SelectTab(tabPage4);
                                    MessageBox.Show("Invalid Percentage");
                                    GridComm["RATE", i].Value = 0.00;
                                    GridComm["AMOUNT", i].Value = 0.00;
                                    GridComm.CurrentCell = GridComm["RATE", i];
                                    GridComm.Focus();
                                    GridComm.BeginEdit(true);
                                    MyParent.Save_Error = true;
                                    return;
                                }
                                else
                                {
                                    GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble( Convert.ToDouble(GridComm["QTY", GridComm.CurrentCell.RowIndex].Value) * (Convert.ToDouble(GridComm["RATE", GridComm.CurrentCell.RowIndex].Value) / 100));
                                }
                                GridComm["AMOUNT", i].Value = Convert.ToDouble( Convert.ToDouble(GridComm["QTY", i].Value) * (Convert.ToDouble(GridComm["RATE", i].Value) / 100));
                            }
                            else
                            {
                                GridComm["AMOUNT", i].Value = Convert.ToDouble(GridComm["QTY", i].Value) * Convert.ToDouble(GridComm["RATE", i].Value);
                            }
                    }
                }

                if(GridSpcl.Rows.Count >=2)
                {
                    for (int i = 0; i < GridSpcl.Rows.Count - 1; i++)
                    {
                        for (int j = 1; j < GridSpcl.Columns.Count - 1; j++)
                        {
                            if (GridSpcl[j, i].Value == DBNull.Value || GridSpcl[j, i].Value.ToString() == String.Empty || GridSpcl[j, i].Value.ToString() == "0")
                            {
                                if (GridSpcl["DYE_MODE", i].Value.ToString() == "N" && (GridSpcl[j, i].Value == DBNull.Value || GridSpcl[j, i].Value.ToString() == String.Empty || GridSpcl[j, i].Value.ToString() == "0"))
                                {
                                
                                }
                                else
                                {
                                    tabControl1.SelectTab(tabPage6);
                                    MessageBox.Show("' " + GridSpcl.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                    GridSpcl.CurrentCell = GridSpcl[j, i];
                                    GridSpcl.Focus();
                                    GridSpcl.BeginEdit(true);
                                    MyParent.Save_Error = true;
                                    return;
                                }
                            }
                        }

                        if (Convert.ToDouble(GridSpcl["BOM_CONS", i].Value) > Convert.ToDouble(GridSpcl["BOM_CONS1", i].Value))
                        {
                            MessageBox.Show("Invalid SPEC REQ BOM CONS..!", "Gainup");
                            GridSpcl.CurrentCell = GridSpcl["BOM_CONS", i];
                            GridSpcl.Focus();
                            GridSpcl.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }

                        if(Convert.ToDouble(GridSpcl["PUR_RATE", i].Value) == 0 || (GridSpcl["PUR_RATE", i].Value.ToString()) == String.Empty || Convert.ToDouble(GridSpcl["PUR_AMOUNT", i].Value) == 0 || (GridSpcl["PUR_AMOUNT", i].Value.ToString()) == String.Empty)
                        {
                            tabControl1.SelectTab(tabPage6);
                            MessageBox.Show("Invalid Spcl Req Purchase Rate & Amount ..!", "Gainup");
                            GridSpcl.CurrentCell = GridSpcl[1, i];
                            GridSpcl.Focus();
                            GridSpcl.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    else
                    {
                         GridSpcl["PUR_AMOUNT", i].Value = (Convert.ToDouble(GridSpcl["BOM_CONS", i].Value)) * Convert.ToDouble(GridSpcl["PUR_RATE", i].Value);
                    }
                   
                    if (GridSpcl["DYE_MODE", i].Value.ToString() == "Y" && (GridSpcl["COLOR", i].Value.ToString().ToUpper() != "GREIGE" || GridSpcl["COLOR", i].Value.ToString().ToUpper() != "R.WHITE"))
                    {
                        if(Convert.ToDouble(GridSpcl["DYE_RATE", i].Value) == 0 || (GridSpcl["DYE_RATE", i].Value.ToString()) == String.Empty || Convert.ToDouble(GridSpcl["DYE_AMOUNT", i].Value) == 0 || (GridSpcl["DYE_AMOUNT", i].Value.ToString()) == String.Empty)
                        {
                            tabControl1.SelectTab(tabPage6);
                            MessageBox.Show("Invalid Spcl Req Dye Rate, Yarn Rate & Amount..!", "Gainup");
                            GridSpcl.CurrentCell = GridSpcl[1, i];
                            GridSpcl.Focus();
                            GridSpcl.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }

                        else if (GridSpcl["ITEM", i].Value.ToString().ToUpper() == "NYLON")
                        {
                            GridSpcl["DYE_COLOR", i].Value =  "R.White";
                            GridSpcl["DYE_ITEMID", i].Value =  3343;                            
                            GridSpcl["DYE_AMOUNT", i].Value = (((Convert.ToDouble(GridSpcl["BOM_CONS", i].Value)) * (Convert.ToDouble(GridSpcl["DYE_RATE", i].Value)))); 
                        }                        
                        else
                        {
                            GridSpcl["DYE_COLOR", i].Value =  "GREIGE";
                            GridSpcl["DYE_ITEMID", i].Value =  867;                            
                            GridSpcl["DYE_AMOUNT", i].Value = (((Convert.ToDouble(GridSpcl["BOM_CONS", i].Value)) * (Convert.ToDouble(GridSpcl["DYE_RATE", i].Value))));
                        }
                    }

                        if (Convert.ToDouble(GridSpcl["PUR_RATE", i].Value) == 0 || (GridSpcl["PUR_RATE", i].Value.ToString()) == String.Empty || Convert.ToDouble(GridSpcl["PUR_AMOUNT", i].Value) == 0 || (GridSpcl["PUR_AMOUNT", i].Value.ToString()) == String.Empty)
                        {
                            tabControl1.SelectTab(tabPage6);
                            MessageBox.Show("Invalid  Rate & Amount..!", "Gainup");
                            GridSpcl.CurrentCell = GridSpcl[1, i];
                            GridSpcl.Focus();
                            GridSpcl.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                        else
                        {
                             GridSpcl["PUR_AMOUNT", i].Value = (Convert.ToDouble(GridSpcl["BOM_CONS", i].Value)) * Convert.ToDouble(GridSpcl["PUR_RATE", i].Value);
                        }

                        
                        if (GridSpcl["ACCESS_TYPE", i].Value.ToString() == "REPLACE")
                           {
                               for (int k = 0; k < GridYarn.Rows.Count; k++)
                               {
                                   Double TBom = 0;
                                   if (GridSpcl["ITEMID", i].Value.ToString() == GridSpcl["REQ_ITEMID", i].Value.ToString() && GridSpcl["SIZEID", i].Value.ToString() == GridSpcl["REQ_SIZEID", i].Value.ToString() && GridSpcl["COLORID", i].Value.ToString() == GridSpcl["REQ_COLORID", i].Value.ToString() && GridSpcl["ACCESS_TYPE", i].Value.ToString() == "REPLACE")
                                   {
                                       tabControl1.SelectTab(tabPage6);
                                       MessageBox.Show("Invalid Access Type, Planning Items are Same","Gainup");  
                                       GridSpcl.CurrentCell = GridSpcl[1, i];
                                       GridSpcl.Focus();
                                       GridSpcl.BeginEdit(true);
                                       MyParent.Save_Error = true;
                                       return;                                       
                                   }
                                   else
                                   {
                                       if (GridYarn["ITEMID", k].Value.ToString() == GridSpcl["REQ_ITEMID", i].Value.ToString() && GridYarn["SIZEID", k].Value.ToString() == GridSpcl["REQ_SIZEID", i].Value.ToString() && GridYarn["COLORID", k].Value.ToString() == GridSpcl["REQ_COLORID", i].Value.ToString() && GridYarn["SNO", k].Value.ToString() == GridSpcl["SNO1", i].Value.ToString())
                                       {
                                           for (int p = 0; p < GridSpcl.Rows.Count-1; p++)
                                           {
                                               if (GridYarn["ITEMID", k].Value.ToString() == GridSpcl["REQ_ITEMID", p].Value.ToString() && GridYarn["SIZEID", k].Value.ToString() == GridSpcl["REQ_SIZEID", p].Value.ToString() && GridYarn["COLORID", k].Value.ToString() == GridSpcl["REQ_COLORID", p].Value.ToString() && GridYarn["SNO", k].Value.ToString() == GridSpcl["SNO1", p].Value.ToString())
                                               {
                                                   TBom = TBom + Convert.ToDouble(GridSpcl["BOM_CONS", p].Value);
                                               }
                                           }
                                           GridYarn["LOSS_WEIGHT", k].Value = (((Convert.ToDouble(GridYarn["BOM_CONS1", k].Value) - Convert.ToDouble(TBom)) * (Convert.ToDouble(GridYarn["LOSS_PER", k].Value))/100));
                                           GridYarn["DYE_AMOUNT", k].Value = (((Convert.ToDouble(GridYarn["BOM_CONS1", k].Value) - Convert.ToDouble(TBom)) + Convert.ToDouble(GridYarn["LOSS_WEIGHT", k].Value)) * (Convert.ToDouble(GridYarn["DYE_RATE", k].Value)));                     

                                           GridYarn["BOM_CONS", k].Value = ((Convert.ToDouble(GridYarn["BOM_CONS1", k].Value) - Convert.ToDouble(TBom)));
                                           GridYarn["PUR_AMOUNT", k].Value = ((Convert.ToDouble(GridYarn["BOM_CONS", k].Value)) + Convert.ToDouble(GridYarn["LOSS_WEIGHT", k].Value)) * Convert.ToDouble(GridYarn["PUR_RATE", k].Value);
                                           if(GridYarn["FLAG", k].Value.ToString() == "T")
                                           {
                                                GridYarn["FLAG", k].Value = 'S';
                                           }
                                           else
                                           {
                                               GridYarn["FLAG", k].Value = GridYarn["FLAG", k].Value.ToString(); 
                                           }
                                           k= GridYarn.Rows.Count ;
                                       }
                                   }
                               }
                           }
                    }
                }
                Total_Count();
                Total_Cost_Calc();
                Queries = new String[(GridYarn.Rows.Count + GridTrim.Rows.Count + GridProc.Rows.Count + GridComm.Rows.Count) * 6 + 40];
                if(MyParent._New)
                {                    
                     DataTable TDt = new DataTable();
                     MyBase.Load_Data("Select IsNull(Max(Entry_No),0) + 1 Entry_No From Socks_Planning_Master ", ref TDt);
                     TxtENo.Text  = TDt.Rows[0][0].ToString();
                    if (TxtENo.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Invalid Entry No", "Gainup");
                        TxtENo.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    Queries[Array_Index++] = "Insert into Socks_Planning_Master (Entry_No, Effect_From, Order_ID, Item_ID, Tot_Yarn_Cons, Tot_Trim_Cons, Remarks, Tot_Yarn_Pur_Amt, Tot_Yarn_Dye_Amt, Tot_Trim_Pur_Amt, Tot_Proc_Pur_Amt, Tot_Proc_Cons, Tot_Comm_Qty, Tot_Comm_Amt) Values (" + TxtENo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}  {0:T}", DtpDate.Value) + "', '" + TxtOcnNo.Tag + "', " + TxtItem.Tag + ", " + Convert.ToDouble(TxtTotWeight.Text.ToString()) + ",  " + Convert.ToDouble(TxtTotTrimQty.Text.ToString()) + ", '" + TxtRemarks.Text.ToString() + "', " + Convert.ToDouble(TxtTotYrnPurAmt.Text.ToString()) + ", " + Convert.ToDouble(TxtTotYrnDyeAmt.Text.ToString()) + ", " + Convert.ToDouble(TxtTotTrmPurAmt.Text.ToString()) + " ,  " + Convert.ToDouble(TxtTotProAmt.Text.ToString()) + ",  " + Convert.ToDouble(TxtTotProQty.Text.ToString()) + ", " + Convert.ToDouble(TxtTotComQty.Text.ToString()) + ",  " + Convert.ToDouble(TxtTotComAmt.Text.ToString()) + ") ; Select Scope_Identity()";
                    Queries[Array_Index++] = "Insert into Socks_Planning_Summary_Details (Master_ID, Yarn_Cost, Trim_Cost, Proc_Cost, Comm_Cost, Spcl_Req_Cost, Profit, Total_Cost, Per_Pack_Cost, Exc_Rate, Prod_Rate_Ind, Prod_Rate_Exp, Sale_Price_Ind, DB_Ind, Profit_Ind, Value_Ind, Sale_Price_Exp, DB_Exp, Profit_Exp, Value_Exp) Values (@@IDENTITY, " + Convert.ToDouble(TxtYarnCost.Text.ToString()) + ", " + Convert.ToDouble(TxtTrimCost.Text.ToString()) + ", " + Convert.ToDouble(TxtProcCost.Text.ToString()) + ", " + Convert.ToDouble(TxtCommCost.Text.ToString()) + ", " + Convert.ToDouble(TxtSpclReqCost.Text.ToString()) + ", " + Convert.ToDouble(TxtProfit.Text.ToString()) + ", " + Convert.ToDouble(TxtTotalCost.Text.ToString()) + ", " + Convert.ToDouble(TxtPackCost.Text.ToString()) + ", " + Convert.ToDouble(TxtExRate.Text.ToString()) + ", " + Convert.ToDouble(TxtIndRs.Text.ToString()) + ", " + Convert.ToDouble(TxtExpRs.Text.ToString()) + ", " + Convert.ToDouble(TxtSalePriceInd.Text.ToString()) + " , " + Convert.ToDouble(TxtDBInd.Text.ToString()) + " , " + Convert.ToDouble(TxtProfitInd.Text.ToString()) + ", " + Convert.ToDouble(TxtValueInd.Text.ToString()) + ", " + Convert.ToDouble(TxtSalePriceExp.Text.ToString()) + ", " + Convert.ToDouble(TxtDBExp.Text.ToString()) + ", " + Convert.ToDouble(TxtProfitExp.Text.ToString()) + ", " + Convert.ToDouble(TxtValueExp.Text.ToString()) + ")";
                }
                else
                {                    
                    Queries[Array_Index++] = "Update Socks_Planning_Master Set  Order_ID = " + TxtOcnNo.Tag + ", Item_ID = " + TxtItem.Tag + ",  Tot_Yarn_Cons = " + Convert.ToDouble(TxtTotWeight.Text.ToString()) + "  , Tot_Trim_Cons = " + Convert.ToDouble(TxtTotTrimQty.Text.ToString()) + ", Remarks = '" + TxtRemarks.Text + "', Tot_Yarn_Pur_Amt = " + Convert.ToDouble(TxtTotYrnPurAmt.Text.ToString()) + "  , Tot_Yarn_Dye_Amt = " + Convert.ToDouble(TxtTotYrnDyeAmt.Text.ToString()) + ", Tot_Trim_Pur_Amt = " + Convert.ToDouble(TxtTotTrmPurAmt.Text.ToString()) + ", Tot_Proc_Pur_Amt = " + Convert.ToDouble(TxtTotProAmt.Text.ToString()) + ", Tot_Proc_Cons = " + Convert.ToDouble(TxtTotProQty.Text.ToString()) + ", Tot_Comm_Qty = " + Convert.ToDouble(TxtTotComQty.Text.ToString()) + ", Tot_Comm_Amt = " + Convert.ToDouble(TxtTotComAmt.Text.ToString()) + " Where Rowid = " + Code;
                    Queries[Array_Index++] = "Update Socks_Planning_Summary_Details  Set  Yarn_Cost = " + Convert.ToDouble(TxtYarnCost.Text.ToString()) + ", Trim_Cost = " + Convert.ToDouble(TxtTrimCost.Text.ToString()) + ", Proc_Cost = " + Convert.ToDouble(TxtProcCost.Text.ToString()) + ", Comm_Cost = " + Convert.ToDouble(TxtCommCost.Text.ToString()) + ", Spcl_Req_Cost = " + Convert.ToDouble(TxtSpclReqCost.Text.ToString()) + ", Profit = " + Convert.ToDouble(TxtProfit.Text.ToString()) + ", Total_Cost = " + Convert.ToDouble(TxtTotalCost.Text.ToString()) + ", Per_Pack_Cost  = " + Convert.ToDouble(TxtPackCost.Text.ToString()) + ", Exc_Rate  = " + Convert.ToDouble(TxtExRate.Text.ToString()) + ", Prod_Rate_Ind = " + Convert.ToDouble(TxtIndRs.Text.ToString()) + ", Prod_Rate_Exp = " + Convert.ToDouble(TxtExpRs.Text.ToString()) + ", Sale_Price_Ind = " + Convert.ToDouble(TxtSalePriceInd.Text.ToString()) + " , DB_Ind = " + Convert.ToDouble(TxtDBInd.Text.ToString()) + " , Profit_Ind = " + Convert.ToDouble(TxtProfitInd.Text.ToString()) + ", Value_Ind = " + Convert.ToDouble(TxtValueInd.Text.ToString()) + ", Sale_Price_Exp = " + Convert.ToDouble(TxtSalePriceExp.Text.ToString()) + ", DB_Exp = " + Convert.ToDouble(TxtDBExp.Text.ToString()) + ", Profit_Exp = " + Convert.ToDouble(TxtProfitExp.Text.ToString()) + ", Value_Exp = " + Convert.ToDouble(TxtValueExp.Text.ToString()) + " Where Master_ID = " + Code + "";
                    Queries[Array_Index++] = "Delete From Socks_Planning_Yarn_Details Where Approval_Flag = 'F' and Spl_Req_Mode = 'F' and  Master_id = " + Code + " and RowID Not In (Select Distinct Planning_Detail_ID from Socks_Yarn_BOM_Status Where Master_ID = " + Code + ")";
                    Queries[Array_Index++] = "Delete From Socks_Planning_Yarn_Details Where Approval_Flag = 'F' and Approval_Flag_Sample = 'F' and Spl_Req_Mode = 'T' and  Master_id = " + Code + " and RowID Not In (Select Distinct Planning_Detail_ID from Socks_Yarn_BOM_Status Where Master_ID = " + Code + ")";
                    Queries[Array_Index++] = "Delete From Socks_Planning_Trim_Details Where Approval_Flag = 'F' and Master_id = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_Planning_Proc_Details Where Approval_Flag = 'F' and Master_id = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_Planning_Commercial_Details Where Approval_Flag = 'F' and Master_id = " + Code;                    
                }

                for (int i = 0; i < GridYarn.Rows.Count; i++)
                {                   
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into Socks_Planning_Yarn_Details (Master_ID, SNo, Item_ID, Color_ID, Size_ID, Yarn_Weight, Dyeing_Mode, Loss_Per, Yarn_Weight_With_Loss, Dyeing_Item_ID, Pur_Rate, Dye_Rate, Pur_Amount, Dye_Amount, Approval_Flag, Yarn_Loss_Perc, Spl_Req_Mode, Access_Type, REQ_ITEMID, REQ_SIZEID, REQ_COLORID, SNo1) Values (@@IDENTITY, " + (i + 1) + ", " + GridYarn["ItemID", i].Value + ", " +  GridYarn["COLORID", i].Value + ", " + GridYarn["SIZEID", i].Value + ", " + Convert.ToDouble(GridYarn["BOM_CONS", i].Value) + ", '" + GridYarn["DYE_MODE", i].Value + "', " + Convert.ToDouble(GridYarn["LOSS_PER", i].Value) + ", " + Convert.ToDouble(GridYarn["LOSS_WEIGHT", i].Value) + ", " + GridYarn["DYE_ITEMID", i].Value + ", " + Convert.ToDouble(GridYarn["PUR_RATE", i].Value) + ",  " + Convert.ToDouble(GridYarn["DYE_RATE", i].Value) + ", " + Convert.ToDouble(GridYarn["PUR_AMOUNT", i].Value) + ", " + Convert.ToDouble(GridYarn["DYE_AMOUNT", i].Value) + ", 'F', " + Convert.ToDouble(GridYarn["YARN_LOSS_PERC", i].Value) + ", 'F', 'PLAN', 0, 0, 0, 0)";
                        }
                        else
                        {
                            if (GridYarn["FLAG", i].Value.ToString() == "F")
                            {
                                Queries[Array_Index++] = "Insert into Socks_Planning_Yarn_Details (Master_ID, SNo, Item_ID, Color_ID, Size_ID, Yarn_Weight, Dyeing_Mode, Loss_Per, Yarn_Weight_With_Loss, Dyeing_Item_ID, Pur_Rate, Dye_Rate, Pur_Amount, Dye_Amount, Approval_Flag, Yarn_Loss_Perc, Spl_Req_Mode, Access_Type, REQ_ITEMID, REQ_SIZEID, REQ_COLORID, SNo1) Values (" + Code + ", " + (i + 1) + ",  " + GridYarn["ItemID", i].Value + ", " +  GridYarn["COLORID", i].Value + ", " + GridYarn["SIZEID", i].Value + ", " + Convert.ToDouble(GridYarn["BOM_CONS", i].Value) + ", '" + GridYarn["DYE_MODE", i].Value + "', " + Convert.ToDouble(GridYarn["LOSS_PER", i].Value) + ", " + Convert.ToDouble(GridYarn["LOSS_WEIGHT", i].Value) + ", " + GridYarn["DYE_ITEMID", i].Value + ", " + Convert.ToDouble(GridYarn["PUR_RATE", i].Value) + ",  " + Convert.ToDouble(GridYarn["DYE_RATE", i].Value) + ", " + Convert.ToDouble(GridYarn["PUR_AMOUNT", i].Value) + ", " + Convert.ToDouble(GridYarn["DYE_AMOUNT", i].Value) + ", 'F', " + Convert.ToDouble(GridYarn["YARN_LOSS_PERC", i].Value) + ", 'F', 'PLAN', 0, 0, 0, 0)";
                            }
                            else if (GridYarn["FLAG", i].Value.ToString() == "S" || GridYarn["FLAG", i].Value.ToString() == "A")
                            {
                                Queries[Array_Index++] = "Update Socks_Planning_Yarn_Details Set Pur_Rate = " + Convert.ToDouble(GridYarn["Pur_Rate", i].Value) + ",  Yarn_Weight = " + Convert.ToDouble(GridYarn["BOM_CONS", i].Value) + ",Pur_Amount = " + Convert.ToDouble(GridYarn["PUR_AMOUNT", i].Value) + ", Dye_Amount = " + Convert.ToDouble(GridYarn["DYE_AMOUNT", i].Value) + ", Yarn_Weight_With_Loss = " + Convert.ToDouble(GridYarn["LOSS_WEIGHT", i].Value) + " Where Master_ID = " + Code + " and SNo = " + GridYarn["SNO", i].Value + " ";
                                Queries[Array_Index++] = "Update Socks_Yarn_BOM_Status Set BOM = " + Convert.ToDouble(GridYarn["BOM_CONS", i].Value) + " Where Planning_Detail_ID =  (Select RowID From  Socks_Planning_Yarn_Details Where Master_ID = " + Code + " and SNo = " + GridYarn["SNO", i].Value + ") and Spl_Req_Mode = 'F'";
                            }
                        }                 
                }

                for (int i = 0; i <GridSpcl.Rows.Count-1; i++)
                {                   
                        if (MyParent.Edit == true && GridSpcl["FLAG", i].Value.ToString() == "F")
                        {
                            Queries[Array_Index++] = "Insert into Socks_Planning_Yarn_Details (Master_ID, SNo, Item_ID, Color_ID, Size_ID, Yarn_Weight, Dyeing_Mode, Loss_Per, Yarn_Weight_With_Loss, Dyeing_Item_ID, Pur_Rate, Dye_Rate, Pur_Amount, Dye_Amount, Approval_Flag, Yarn_Loss_Perc, Spl_Req_Mode, Access_Type, REQ_ITEMID, REQ_SIZEID, REQ_COLORID, SNo1, Approval_Flag_Sample) Values (" + Code + ", " + (Dt.Rows.Count + i + 1) + ", " + GridSpcl["ItemID", i].Value + ", " +  GridSpcl["COLORID", i].Value + ", " + GridSpcl["SIZEID", i].Value + ", " + Convert.ToDouble(GridSpcl["BOM_CONS", i].Value) + ", '" + GridSpcl["DYE_MODE", i].Value + "', 0, 0, " +  GridSpcl["DYE_ITEMID", i].Value + ", " + Convert.ToDouble(GridSpcl["PUR_RATE", i].Value) + ", " + Convert.ToDouble(GridSpcl["DYE_RATE", i].Value) + ", " + Convert.ToDouble(GridSpcl["PUR_AMOUNT", i].Value) + ", " + Convert.ToDouble(GridSpcl["DYE_AMOUNT", i].Value) + ", 'F', 0, 'T', '" + GridSpcl["ACCESS_TYPE", i].Value.ToString() + "', " + GridSpcl["REQ_ItemID", i].Value + ", " +  GridSpcl["REQ_SIZEID", i].Value + ", " + GridSpcl["REQ_COLORID" , i].Value + ",  " + GridSpcl["SNO1" , i].Value + ", 'F')";
                        }
                       // Queries[Array_Index++] = "Update Socks_Yarn_BOM_Status Set Spec_Req = Spec_Req + " + Convert.ToDouble(GridSpcl["BOM_CONS", i].Value) + " Where Planning_Detail_ID =  (Select RowID From  Socks_Planning_Yarn_Details Where Master_ID = " + Code + " and SNo = " + GridSpcl["SNO", i].Value + ") and Spl_Req_Mode = 'F'";                     
                }
                for (int i = 0; i < GridTrim.Rows.Count - 1; i++)
                {                    
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into Socks_Planning_Trim_Details (Master_ID, SNo, Access_Type, Item_ID, Color_ID, Size_ID, Plan_Type, Trim_Cons, Tot_Qty, Pur_Rate, Pur_Amount, Approval_Flag, Sample_ID, SNo1) Values (@@IDENTITY, " + (i + 1) + ", '" + GridTrim["Access_Type", i].Value + "', " + GridTrim["Item_ID", i].Value + ", " +  GridTrim["COLOR_ID", i].Value + ", " + GridTrim["SIZE_ID", i].Value + ", '" + GridTrim["PLAN_TYPE", i].Value + "', " + Convert.ToDouble(GridTrim["CONS", i].Value) + ", " + Convert.ToDouble(GridTrim["REQ_QTY", i].Value) + ",  " + Convert.ToDouble(GridTrim["PUR_RATE", i].Value) + ", " + Convert.ToDouble(GridTrim["PUR_AMOUNT", i].Value) + ", 'F', " + GridTrim["Sample_ID", i].Value + ", " + GridTrim["SNo1", i].Value + ")";
                        }
                        else
                        {
                            if (GridTrim["FLAG", i].Value.ToString() == "F")
                            {
                                Queries[Array_Index++] = "Insert into Socks_Planning_Trim_Details (Master_ID, SNo, Access_Type, Item_ID, Color_ID, Size_ID, Plan_Type, Trim_Cons, Tot_Qty, Pur_Rate, Pur_Amount, Approval_Flag, Sample_ID, SNo1) Values (" + Code + ", " + (i + 1) + ", '" + GridTrim["Access_Type", i].Value + "' , " + GridTrim["Item_ID", i].Value + ", " +  GridTrim["COLOR_ID", i].Value + ", " + GridTrim["SIZE_ID", i].Value + ", '" + GridTrim["PLAN_TYPE", i].Value + "', " + Convert.ToDouble(GridTrim["CONS", i].Value) + ", " + Convert.ToDouble(GridTrim["REQ_QTY", i].Value) + ", " + Convert.ToDouble(GridTrim["PUR_RATE", i].Value) + ", " + Convert.ToDouble(GridTrim["PUR_AMOUNT", i].Value) + ", 'F', " + GridTrim["Sample_ID", i].Value + ", " + GridTrim["SNo1", i].Value + ")";
                            }
                        }                 
                }

                for (int i = 0; i < GridProc.Rows.Count - 1; i++)
                {                    
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into Socks_Planning_Proc_Details (Master_ID, SNo, Proc_ID, Sample_ID, Tot_Qty, Pro_Rate, Pro_Amount, SizeID, Approval_Flag) Values (@@IDENTITY, " + (i + 1) + ", " + GridProc["Proc_ID", i].Value + ", " +  GridProc["Sample_ID", i].Value + ", " + Convert.ToDouble(GridProc["REQ_QTY", i].Value) + ",  " + Convert.ToDouble(GridProc["PRO_RATE", i].Value) + ", " + Convert.ToDouble(GridProc["PRO_AMOUNT", i].Value) + ", " +  GridProc["SizeID", i].Value + ", 'F')";
                        }
                        else
                        {
                            if (GridProc["FLAG", i].Value.ToString() == "F")
                            {
                                Queries[Array_Index++] = "Insert into Socks_Planning_Proc_Details (Master_ID, SNo, Proc_ID, Sample_ID, Tot_Qty, Pro_Rate, Pro_Amount, SizeID, Approval_Flag) Values (" + Code + ", " + (i + 1) + ", " + GridProc["Proc_ID", i].Value + ", " +  GridProc["Sample_ID", i].Value + ", " + Convert.ToDouble(GridProc["REQ_QTY", i].Value) + ", " + Convert.ToDouble(GridProc["PRO_RATE", i].Value) + ", " + Convert.ToDouble(GridProc["PRO_AMOUNT", i].Value) + ", " +  GridProc["SizeID", i].Value + ", 'F')";
                            }
                        }                 
                }

                for (int i = 0; i < GridComm.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Socks_Planning_Commercial_Details (Master_ID, SNo, Comm_ID, Tot_Qty, Calc_Mode, Rate, Amount, Approval_Flag) Values (@@IDENTITY, " + (i + 1) + ", " + GridComm["Comm_ID", i].Value + ",  " + Convert.ToDouble(GridComm["QTY", i].Value) + ",  '" + GridComm["Calc_Mode", i].Value + "', " + Convert.ToDouble(GridComm["RATE", i].Value) + ", " + Convert.ToDouble(GridComm["AMOUNT", i].Value) + ", 'F')";
                    }
                    else
                    {
                        if (GridComm["FLAG", i].Value.ToString() == "F")
                        {
                            Queries[Array_Index++] = "Insert into Socks_Planning_Commercial_Details (Master_ID, SNo, Comm_ID, Tot_Qty, Calc_Mode, Rate, Amount, Approval_Flag) Values (" + Code + ", " + (i + 1) + ", " + GridComm["Comm_ID", i].Value + ",  " + Convert.ToDouble(GridComm["QTY", i].Value) + ",  '" + GridComm["Calc_Mode", i].Value + "', " + Convert.ToDouble(GridComm["RATE", i].Value) + ", " + Convert.ToDouble(GridComm["AMOUNT", i].Value) + ", 'F')";
                        }
                    }
                }
                Queries[Array_Index++] = "Exec Socks_Yarn_Planning_Import_Proc '" + TxtOcnNo.Text.ToString() + "'";
                Queries[Array_Index++] = "Exec Socks_Trim_Planning_Import_Proc '" + TxtOcnNo.Text.ToString() + "'";

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
                TxtYarnCost.Text = " "; TxtTrimCost.Text = " "; TxtProcCost.Text = " "; TxtCommCost.Text = " "; TxtSpclReqCost.Text = " "; TxtProfit.Text = " "; TxtTotalCost.Text = " "; TxtPackCost.Text = " "; TxtExRate.Text = " ";  TxtIndRs.Text = " "; TxtExpRs.Text = " "; TxtSalePriceInd.Text = " "; TxtDBInd.Text = " "; TxtProfitInd.Text = " "; TxtValueInd.Text = " "; TxtSalePriceExp.Text = " "; TxtDBExp.Text = " "; TxtProfitExp.Text = " "; TxtValueExp.Text= " ";  
                if(MyParent.UserCode == 1)
                {
                    Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Planning - Delete ", "Select Distinct B.Entry_No,  B.Effect_From, A.Order_No, A.Item, A.Party, A.Employee, Bom_Qty_Plan BOM_Qty, Buyer_Qty_Plan Buyer_Qty, B.Tot_Yarn_Cons, B.Tot_Trim_Cons, B.OrdeR_ID, B.Item_ID, A.Order_Date , B.REmarks,A.Party_Code, A.Amount, A.Ex_Rate, A.Currency,  C.Profit, C.Profit_Ind, C.Profit_Exp, C.DB_Ind, C.DB_Exp, B.RowID, A.Buy_Qty From Socks_Bom_Item_Fn() A Inner Join Socks_Planning_Master B On A.ORder_ID = B.Order_ID and A.ItemID = B.Item_ID  Left Join Socks_Planning_Summary_Details C On B.RowID = C.Master_ID Left Join FitSocks.Dbo.Employee D On A.Empl_ID = D.employeeid and D.Acc_Empl_ID = " + MyParent.Emplno + " Where B.RowID Not In (Select Distinct Master_ID From Socks_Planning_Yarn_Details Where Approval_Flag = 'T') and B.RowID Not In (Select Distinct Master_ID From Socks_Planning_Trim_Details Where Approval_Flag = 'T') and B.RowID Not In (Select Distinct Planning_Master_ID From Socks_Yarn_BOM_Status Where (PO_Qty + Transfer_In + Transfer_Out + GRN_Qty + Prod_Issue) > 0) Order by B.Entry_No desc ", String.Empty, 80, 80, 100, 100, 140, 100, 100, 100, 100, 100);                
                }
                else
                {
                       Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Planning - Delete ", "Select Distinct B.Entry_No,  B.Effect_From, A.Order_No, A.Item, A.Party, A.Employee, Bom_Qty_Plan BOM_Qty, Buyer_Qty_Plan Buyer_Qty, B.Tot_Yarn_Cons, B.Tot_Trim_Cons, B.OrdeR_ID, B.Item_ID, A.Order_Date , B.REmarks,A.Party_Code, A.Amount, A.Ex_Rate, A.Currency,  C.Profit, C.Profit_Ind, C.Profit_Exp, C.DB_Ind, C.DB_Exp, B.RowID, A.Buy_Qty From Socks_Bom_Item_Fn() A Inner Join Socks_Planning_Master B On A.ORder_ID = B.Order_ID and A.ItemID = B.Item_ID  Left Join Socks_Planning_Summary_Details C On B.RowID = C.Master_ID Inner Join FitSocks.Dbo.Employee D On A.Empl_ID = D.employeeid and D.Acc_Empl_ID = " + MyParent.Emplno + " Where B.RowID Not In (Select Distinct Master_ID From Socks_Planning_Yarn_Details Where Approval_Flag = 'T') and B.RowID Not In (Select Distinct Master_ID From Socks_Planning_Trim_Details Where Approval_Flag = 'T') and B.RowID Not In (Select Distinct Planning_Master_ID From Socks_Yarn_BOM_Status Where (PO_Qty + Transfer_In + Transfer_Out + GRN_Qty + Prod_Issue) > 0) Order by B.Entry_No desc ", String.Empty, 80, 80, 100, 100, 140, 100, 100, 100, 100, 100);                
                }
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    tabControl1.SelectTab(tabPage1);
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
                    MyBase.Run("Delete from Socks_Planning_Summary_Details Where MasteR_ID = " + Code + " ", "Delete from Socks_Planning_Commercial_Details Where Approval_Flag = 'F' and MasteR_ID = " + Code + " ", "Delete from Socks_Planning_Proc_Details Where Approval_Flag = 'F' and MasteR_ID = " + Code + " ", "Delete from Socks_Planning_Trim_Details Where  Approval_Flag = 'F' and MasteR_ID = " + Code + " ", "Delete from Socks_Planning_Yarn_Details Where  Approval_Flag = 'F' and MasteR_ID = " + Code + " ", "Delete from Socks_Planning_Master Where RowID = " + Code + "", "Exec Socks_Trim_Planning_Import_Proc '" + TxtOcnNo.Text.ToString() + "'", "Exec Socks_Yarn_Planning_Import_Proc '" + TxtOcnNo.Text.ToString() + "'");
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
                TxtYarnCost.Text = " "; TxtTrimCost.Text = " "; TxtProcCost.Text = " "; TxtCommCost.Text = " "; TxtSpclReqCost.Text = " "; TxtProfit.Text = " "; TxtTotalCost.Text = " "; TxtPackCost.Text = " "; TxtExRate.Text = " ";  TxtIndRs.Text = " "; TxtExpRs.Text = " "; TxtSalePriceInd.Text = " "; TxtDBInd.Text = " "; TxtProfitInd.Text = " "; TxtValueInd.Text = " "; TxtSalePriceExp.Text = " "; TxtDBExp.Text = " "; TxtProfitExp.Text = " "; TxtValueExp.Text= " ";  
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Planning - View", "Select Distinct A.Order_No, B.Entry_No,  B.Effect_From,  A.Item, A.Party, A.Employee, Bom_Qty_Plan BOM_Qty, Buyer_Qty_Plan Buyer_Qty, B.Tot_Yarn_Cons, B.Tot_Trim_Cons, B.OrdeR_ID, B.Item_ID, A.Order_Date , B.REmarks, A.Party_Code, A.Amount, A.Ex_Rate, A.Currency,  C.Profit, C.Profit_Ind, C.Profit_Exp, C.DB_Ind, C.DB_Exp, B.RowID, A.Buy_Qty From Socks_Bom_Item_Fn() A Inner Join Socks_Planning_Master B On A.ORder_ID = B.Order_ID and A.ItemID = B.Item_ID Left Join Socks_Planning_Summary_Details C On B.RowID = C.Master_ID  Order by A.Order_No desc ", String.Empty, 150, 80, 100, 100, 140, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr); 
                    tabControl1.SelectTab(tabPage1);
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
                if (MyParent._New == true)
                {
                    Str1 = "Select 0 SNO, 'GENERAL' ACCESS_TYPE, '' ITEM, '-' SAMPLE_NO, '' SAMP_SIZE, 0 BOM_QTY, '' SIZE, '' COLOR, 0 CONS, '' PLAN_TYPE, 0 REQ_QTY, 0.0000 PUR_RATE, 0.00 PUR_AMOUNT, Item_ID, Color_ID, Size_ID, 0 SAMPLE_ID, '' SAMPLE_NO1, 0 SNO1, 'F' FLAG  From Socks_Planning_Trim_Details Where 1 = 2";
                    Str3 = "Select 0 SNO, '' COMM_NAME, 0 QTY, '' CALC_MODE, 0.00 RATE, 0.00 AMOUNT, 0 COMM_ID, 'F' FLAG  From Socks_Planning_Commercial_Details Where 1=2";
                    if(TxtOcnNo.Text.ToString() == String.Empty || TxtItem.Text.ToString() == String.Empty)
                    {
                        Str = "Select 0 SNO, BOM_QTY,  ITEM, COLOR, SIZE, 0.0 YARN_LOSS_PERC, BOM_CONS, 'F' DYE_MODE, '' DYE_COLOR, 0 LOSS_PER, 0.000 LOSS_WEIGHT, 0.00 PUR_RATE, 0.00 PUR_AMOUNT, 0.00 DYE_RATE, 0.00 DYE_AMOUNT,  ItemID, COLORID, SIZEID,  0 DYE_ITEMID, 'F' FLAG, BOM_CONS BOM_CONS1 From Socks_Consumption_Ord_Bom_Fn() Where 1 = 2 ";
                        Str2 = "Select SNO, PROCESS, SAMPLE_NO, BOM_QTY, SIZE, BOM_QTY REQ_QTY, 0.00 PRO_RATE, 0.00 PRO_AMOUNT, PROC_ID, SAMPLE_ID, SIZEID, SAMPLE_NO1, 'F' FLAG  From Socks_Sample_Process_Grid_Fn(0, 0) Where 1 = 2 Order by Sno";                        
                    }
                    else
                    {
                        //Str = "Select ROW_NUMBER() Over (Order by Yarn_Item, Color, Size) SNO, Sum(Bom_Qty_Plan) BOM_QTY, Yarn_Item ITEM, COLOR, SIZE, Sum(BOM_Cons_Weight) BOM_CONS, Dbo.Socks_Budget_Last_App_Rate_Fn('Yarn', Yarn_ITEMID, ColorID, SizeID) PUR_RATE, Cast(Dbo.Socks_Budget_Last_App_Rate_Fn('Yarn', Yarn_ITEMID, ColorID, SizeID) * Sum(BOM_Cons_Weight) as Numeric(30,2)) PUR_AMOUNT, 'N' DYE_MODE, COLOR DYE_COLOR, 0 LOSS_PER, 0.000 LOSS_WEIGHT, 0.00 DYE_RATE, 0.00 DYE_AMOUNT, Yarn_ITEMID ItemID, COLORID, SIZEID, COLORID DYE_ITEMID, 'F' FLAG From Socks_Sample_Consumption_Ord_Bom_Fn() Where Order_ID = " + TxtOcnNo.Tag.ToString() + " and Ord_ItemID = " + TxtItem.Tag.ToString() + " Group by Yarn_Item, Color, Size, Yarn_ITEMID , COLORID, SIZEID Order by Yarn_Item, Color, Size";
                        Str = "Select ROW_NUMBER() Over (Order by A.Item, A.Color, A.Size) SNO, A.BOM_QTY, A.Item ITEM, A.COLOR, A.SIZE, A.Yarn_Loss_Perc YARN_LOSS_PERC, A.BOM_CONS, ISNull(B.Mode,'N') DYE_MODE,  (CAse When ISNull(B.Mode,'N') = 'Y' and A.ItemID = 2109 Then 'R.White' When  ISNull(B.Mode,'N') = 'Y' Then 'GREIGE' Else A.Color End) DYE_COLOR, Cast(Isnull(B.Loss,0) as Int) LOSS_PER, Isnull(Nullif(Cast((A.BOM_CONS * Cast(Isnull(B.Loss,0) as Int)) / 100 as Numeric(30,3)),0),0.000) LOSS_WEIGHT, Isnull(B.Rate, 0.00) PUR_RATE, Cast(Isnull(B.Rate, 0.00) * (A.BOM_Cons + Isnull(Nullif(Cast((A.BOM_CONS * Cast(Isnull(B.Loss,0) as Int)) / 100 as Numeric(30,3)),0),0.000)) as Numeric(30,2)) PUR_AMOUNT, IsNull(B.PRate,0.00) DYE_RATE, Cast((A.BOM_CONS + Isnull(Nullif(Cast((A.BOM_CONS * Cast(Isnull(B.Loss,0) as Int)) / 100 as Numeric(30,3)),0),0.000)) * IsNull(B.PRate,0.00) as Numeric(20,2)) DYE_AMOUNT, A.ITEMID ItemID, A.COLORID, A.SIZEID, (CAse When ISNull(B.Mode,'N') = 'Y' and A.ITEMID = 2109 Then 3343 When ISNull(B.Mode,'N') = 'Y' Then 867  Else A.COLORID  End) DYE_ITEMID, A.BOM_CONS BOM_CONS1, 'F' FLAG  From Socks_Consumption_Ord_Bom_Fn() A Left Join  Socks_Budget_App_Rate_Max_Fn() B On B.Type = 'Yarn' and A.ItemID = B.ItemID and A.COLORID = B.ColorID and A.SIZEID = B.SizeID Where A.Order_ID = " + TxtOcnNo.Tag.ToString() + " and A.Ord_ItemID = " + TxtItem.Tag.ToString() + "  Order by A.Item, A.Color, A.Size";
                        Str2 = "Select Distinct A.SNO, A.PROCESS, A.SAMPLE_NO, A.BOM_QTY, A.SIZE, A.BOM_QTY REQ_QTY, IsNull(B.Rate,0.00) PRO_RATE, CAst((A.Bom_Qty * IsNull(B.RAte,0.00)) as Numeric(25,2)) PRO_AMOUNT, A.PROC_ID, A.SAMPLE_ID, A.SIZEID, A.SAMPLE_NO1, 'F' FLAG  From Socks_Sample_Process_Grid_Fn(" + TxtOcnNo.Tag.ToString() + ", " + TxtItem.Tag.ToString() + ") A Left Join Socks_Budget_App_Rate_Max_Fn() B On A.ItemID = " + TxtItem.Tag.ToString() + " and A.PROC_ID = B.ItemID and A.Sample_Id = B.ColorID and B.Type = 'Process' Order by A.Sno";
                    }
                    Str4 = "Select 0 SNO, '' ACCESS_TYPE, '' PLAN_ITEM, ITEM, COLOR, SIZE, BOM_CONS, BOM_CONS BOM_CONS1, 'F' DYE_MODE, '' DYE_COLOR,  0.00 PUR_RATE, 0.00 PUR_AMOUNT, 0.00 DYE_RATE, 0.00 DYE_AMOUNT, ItemID, COLORID, SIZEID, 0 DYE_ITEMID, 0 REQ_ITEMID, 0 REQ_SIZEID, 0 REQ_COLORID, '' PLAN_DTL_ID, 0 SNO1, 'F' FLAG From Socks_Consumption_Ord_Bom_Fn() Where 1 = 2 ";
                }
                else
                {                    
                    Str = "Select A.SNO, A.BOM_QTY, A.ITEM, A.COLOR, A.SIZE, A.YARN_LOSS_PERC, A.BOM_CONS BOM_CONS, A.DYE_MODE, A.DYE_COLOR, A.LOSS_PER, A.LOSS_WEIGHT, A.PUR_RATE, A.PUR_AMOUNT, A.DYE_RATE, A.DYE_AMOUNT, A.ItemID, A.COLORID, A.SIZEID,  A.DYE_ITEMID, A.BOM_CONS + ISnull(B.Spl_Yarn_Weight,0) BOM_CONS1, (Case When A.Approval_Flag = 'F' and C.BOM Is Not Null Then 'A' Else A.Approval_Flag End) FLAG From Socks_Yarn_Planning_Fn() A Left Join Socks_Yarn_Spl_Req_Fn() B On A.ItemID = B.REQ_ITEMID and A.COLORID = B.REQ_COLORID and A.SIZEID = B.REQ_SIZEID and B.Access_Type = 'REPLACE' and A.RowID = B.Master_ID Left Join Socks_Yarn_BOM_Status C On A.PlanDtlID = C.Planning_Detail_ID and A.RowID = C.Planning_Master_ID and C.Dyeing_Status = 'N' Where A.RoWID = " + Code + " and A.Spl_Req_Mode = 'F'  Order by A.Sno ";
                    //Str = "Select A.SNO, A.BOM_QTY, A.ITEM, A.COLOR, A.SIZE, A.YARN_LOSS_PERC, A.BOM_CONS BOM_CONS, A.DYE_MODE, A.DYE_COLOR, A.LOSS_PER, A.LOSS_WEIGHT, A.PUR_RATE, A.PUR_AMOUNT, A.DYE_RATE, A.DYE_AMOUNT, A.ItemID, A.COLORID, A.SIZEID,  A.DYE_ITEMID, A.BOM_CONS + ISnull(B.Spl_Yarn_Weight,0) BOM_CONS1, A.Approval_Flag FLAG  From Socks_Yarn_Planning_Fn() A Left Join Socks_Yarn_Spl_Req_Fn() B On A.ItemID = B.REQ_ITEMID and A.COLORID = B.REQ_COLORID and A.SIZEID = B.REQ_SIZEID and B.Access_Type = 'REPLACE' and A.RowID = B.Master_ID Where A.RoWID = " + Code + " and A.Spl_Req_Mode = 'F'  Order by A.Sno ";
                    Str1 = "Select SNO, ACCESS_TYPE, ITEM, SAMPLE_NO, SAMP_SIZE, BOM_QTY,  SIZE,  COLOR,  PLAN_TYPE, Trim_Cons CONS, Tot_Qty REQ_QTY, PUR_RATE, PUR_AMOUNT, Item_ID, Color_ID, Size_ID, Sample_ID, CAst(SAMPLE_ID as Varchar(20)) + '-' + Cast(Item_ID as Varchar(20)) + '-' + Cast(Size_ID as Varchar(20)) + '-' + Cast(Color_ID as Varchar(20)) SAMPLE_NO1, SNO1, Approval_Flag FLAG From Socks_Trim_Planning_Fn() Where Rowid = " + Code + " Order by SNo";
                    Str2 = "Select SNO, PROCESS, SAMPLE_NO, BOM_QTY, SIZE,  Tot_Qty REQ_QTY, PRO_RATE, PRO_AMOUNT, PROC_ID, SAMPLE_ID, SIZEID, CAst(PROC_ID as VArchar(10)) + '-' + CAst(SAMPLE_ID as Varchar(20)) + '-' + CAst(SIZEID as Varchar(20)) + '-' + CAst(Ord_ItemId as Varchar(20)) SAMPLE_NO1, Approval_Flag FLAG From Socks_Process_Planning_Fn() Where Rowid = " + Code + " Order by SNo";
                    Str3 = "Select SNO, COMM_NAME, Tot_Qty QTY, CALC_MODE,  RATE, AMOUNT, COMM_ID, Approval_Flag FLAG  From Socks_Commercial_Planning_Fn() Where RowID = " + Code + "";
                    Str4 = "Select A.SNO, A.ACCESS_TYPE, A.REQ_ITEM + '-' + A.REQ_COLOR + '-' + A.REQ_SIZE PLAN_ITEM, A.ITEM, A.COLOR, A.SIZE, A.BOM_CONS, A.BOM_CONS BOM_CONS1,  A.DYE_MODE, A.DYE_COLOR, A.PUR_RATE, A.PUR_AMOUNT, A.DYE_RATE, A.DYE_AMOUNT, A.ItemID, A.COLORID, A.SIZEID, A.DYE_ITEMID, A.REQ_ITEMID, A.REQ_SIZEID, A.REQ_COLORID, CAst(A.REQ_ITEMID as VArchar(20)) + '-' + Cast(A.REQ_SIZEID as Varchar(20)) + '-' + CASt(A.REQ_COLORID as VArchar(20)) PLAN_DTL_ID, A.SNO1, IsNull(A.Approval_Flag_Sample, 'F') FLAG  From Socks_Yarn_Planning_Fn() A   Where A.RoWID = " + Code + " and A.Spl_Req_Mode = 'T' Order by A.Sno ";
                }
                GridYarn.DataSource = MyBase.Load_Data(Str, ref Dt);
                GridTrim.DataSource = MyBase.Load_Data(Str1, ref Dt1);
                GridProc.DataSource = MyBase.Load_Data(Str2, ref Dt2);
                GridComm.DataSource = MyBase.Load_Data(Str3, ref Dt3);                
                GridSpcl.DataSource = MyBase.Load_Data(Str4, ref Dt4);   
                MyBase.ReadOnly_Grid_Without(ref GridYarn, "DYE_MODE", "LOSS_PER", "PUR_RATE", "DYE_RATE");
                MyBase.ReadOnly_Grid_Without(ref GridTrim, "ACCESS_TYPE", "ITEM", "SIZE", "COLOR", "PLAN_TYPE", "CONS", "PUR_RATE");
                MyBase.ReadOnly_Grid_Without(ref GridProc, "PROCESS", "PRO_RATE");
                MyBase.ReadOnly_Grid_Without(ref GridComm, "COMM_NAME", "CALC_MODE", "RATE");
                MyBase.ReadOnly_Grid_Without(ref GridSpcl, "ACCESS_TYPE", "ITEM", "COLOR", "SIZE", "PLAN_ITEM", "BOM_CONS", "PUR_RATE", "DYE_MODE", "DYE_RATE");
                if(MyParent._New)
                {
                    MyBase.Grid_Designing(ref GridYarn, ref Dt, "BOM_QTY", "ItemID", "SizeID", "ColorID", "DYE_ITEMID", "FLAG", "BOM_CONS1");
                    MyBase.Grid_Designing(ref GridTrim, ref Dt1, "Item_ID", "Size_ID", "Color_ID", "SAMPLE_NO1", "FLAG", "Sample_ID", "Sno1");
                    MyBase.Grid_Designing(ref GridProc, ref Dt2, "Sample_ID",  "Proc_ID", "SAMPLE_NO1", "SizeID" , "FLAG");
                    MyBase.Grid_Designing(ref GridComm, ref Dt3, "Comm_ID", "FLAG");
                    MyBase.Grid_Designing(ref GridSpcl, ref Dt4, "BOM_CONS1", "ItemID", "SizeID", "ColorID", "FLAG", "REQ_ITEMID", "REQ_SIZEID", "REQ_COLORID", "PLAN_DTL_ID", "SNO1", "DYE_ITEMID");
                }
                else 
                {
                    MyBase.Grid_Designing(ref GridYarn, ref Dt, "BOM_QTY", "ItemID", "SizeID", "ColorID", "DYE_ITEMID", "BOM_CONS1");
                    MyBase.Grid_Designing(ref GridTrim, ref Dt1, "Item_ID", "Size_ID", "Color_ID", "SAMPLE_NO1", "Sample_ID", "SNo1");
                    MyBase.Grid_Designing(ref GridProc, ref Dt2, "Sample_ID",  "Proc_ID", "SAMPLE_NO1", "SizeID");
                    MyBase.Grid_Designing(ref GridComm, ref Dt3, "Comm_ID");
                    MyBase.Grid_Designing(ref GridSpcl, ref Dt4, "BOM_CONS1", "ItemID", "SizeID", "ColorID", "REQ_ITEMID", "REQ_SIZEID", "REQ_COLORID", "PLAN_DTL_ID", "SNO1", "DYE_ITEMID");
                }
                MyBase.Grid_Width(ref GridYarn, 50, 110, 100, 80, 80, 80, 80, 100, 70, 90, 80, 120, 90, 90);
                MyBase.Grid_Width(ref GridTrim, 50, 100, 120, 100, 100, 100, 100, 120, 100, 100, 120, 100, 140, 80);
                MyBase.Grid_Width(ref GridProc, 50, 100, 100, 100, 100, 100, 100, 140, 80);
                MyBase.Grid_Width(ref GridComm, 50, 120, 100, 100, 100, 140, 80);
                MyBase.Grid_Width(ref GridSpcl, 50, 100, 160, 100, 100, 100, 100, 80, 100, 100, 120, 100, 120);
                MyBase.Grid_Colouring(ref GridYarn, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Colouring(ref GridTrim, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Colouring(ref GridProc, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Colouring(ref GridComm, Control_Modules.Grid_Design_Mode.Column_Wise);                
                MyBase.Grid_Colouring(ref GridSpcl, Control_Modules.Grid_Design_Mode.Column_Wise);
                GridYarn.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; 
                GridYarn.Columns["BOM_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridYarn.Columns["ITEM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridYarn.Columns["COLOR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridYarn.Columns["SIZE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridYarn.Columns["BOM_CONS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridYarn.Columns["PUR_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridYarn.Columns["DYE_MODE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridYarn.Columns["DYE_COLOR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridYarn.Columns["LOSS_PER"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridYarn.Columns["LOSS_WEIGHT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridYarn.Columns["YARN_LOSS_PERC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridYarn.Columns["PUR_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                GridYarn.Columns["DYE_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridYarn.Columns["DYE_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridYarn.Columns["FLAG"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridTrim.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                
                GridTrim.Columns["ACCESS_TYPE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;                
                GridTrim.Columns["SAMPLE_NO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridTrim.Columns["SAMP_SIZE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridTrim.Columns["FLAG"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;      
                GridTrim.Columns["ITEM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridTrim.Columns["BOM_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridTrim.Columns["COLOR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridTrim.Columns["SIZE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridTrim.Columns["CONS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridTrim.Columns["PLAN_TYPE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridTrim.Columns["REQ_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridTrim.Columns["PUR_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridTrim.Columns["PUR_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridProc.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                
                GridProc.Columns["PROCESS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridProc.Columns["SAMPLE_NO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridProc.Columns["BOM_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;               
                GridProc.Columns["SIZE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;                                
                GridProc.Columns["REQ_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridProc.Columns["PRO_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridProc.Columns["PRO_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridProc.Columns["FLAG"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridComm.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridComm.Columns["COMM_NAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridComm.Columns["QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridComm.Columns["CALC_MODE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridComm.Columns["RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridComm.Columns["AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridComm.Columns["FLAG"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridYarn.Columns["FLAG"].HeaderText = "APPROVAL"; GridTrim.Columns["FLAG"].HeaderText = "APPROVAL"; 
                GridProc.Columns["FLAG"].HeaderText = "APPROVAL"; GridComm.Columns["FLAG"].HeaderText = "APPROVAL";
                GridSpcl.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; 
                GridSpcl.Columns["PLAN_ITEM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridSpcl.Columns["ITEM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridSpcl.Columns["COLOR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridSpcl.Columns["SIZE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridSpcl.Columns["BOM_CONS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridSpcl.Columns["PUR_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridSpcl.Columns["PUR_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridSpcl.Columns["FLAG"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridSpcl.Columns["DYE_MODE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridSpcl.Columns["DYE_COLOR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                GridSpcl.Columns["DYE_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridSpcl.Columns["DYE_AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridTrim.Columns["PLAN_TYPE"].HeaderText = "CALC_TYPE"; 
                GridYarn.Columns["YARN_LOSS_PERC"].HeaderText = "KNIT ALW%"; 
                GridTrim.Columns["SIZE"].HeaderText = "TRIM SIZE";GridTrim.Columns["COLOR"].HeaderText = "TRIM COLOR";
                GridYarn.Columns["LOSS_PER"].HeaderText = "DYE ALW%";  GridYarn.Columns["LOSS_WEIGHT"].HeaderText = "DYE LOSS WT";
                GridYarn.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridTrim.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridProc.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridComm.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridSpcl.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
               
                if (MyParent._New == false)
                {
                    for (int p = 0; p <= GridYarn.Rows.Count - 1; p++)
                    {
                        if (GridYarn["FLAG", p].Value.ToString() == "T")
                        {
                            GridYarn.Rows[p].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
                        }
                    }
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
                    for (int t = 0; t < GridSpcl.Rows.Count - 1; t++)
                    {
                        if (GridSpcl["FLAG", t].Value.ToString() == "T")
                        {
                            GridSpcl.Rows[t].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        private void GridYarn_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt == null)
                {
                    Txt = (TextBox)e.Control;
                   // Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                    Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);  
                    Txt.Leave +=new EventHandler(Txt_Leave);
                    Txt.TextChanged +=new EventHandler(Txt_TextChanged);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

                    if (GridProc.CurrentCell.ColumnIndex == GridProc.Columns["SAMPLE_NO"].Index)
                    {
                        if (GridProc["SAMPLE_NO", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            MessageBox.Show("INVALID SAMPLE_NO", "Gainup");
                            GridProc.CurrentCell = GridProc["SAMPLE_NO", GridProc.CurrentCell.RowIndex];
                            GridProc.Focus();
                            GridProc.BeginEdit(true);
                            return;
                        }
                    }
                    else if (GridProc.CurrentCell.ColumnIndex == GridProc.Columns["PRO_RATE"].Index && Txt_Proc.Text.ToString() != String.Empty)
                    {
                        GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Proc.Text.ToString());
                        if (GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value = 0.00;
                        }
                        else
                        {
                            GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value = GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value.ToString();
                            GridProc["PRO_AMOUNT", GridProc.CurrentCell.RowIndex].Value = Convert.ToDouble(GridProc["REQ_QTY", GridProc.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value);
                        }
                        //else if (Convert.ToDouble(GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value) > 0)
                        //{                            
                        //    if(Convert.ToDouble(GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value.ToString()) >0 && GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        //    {
                        //        for(int i=0; i<= GridProc.Rows.Count-2; i++)
                        //        {
                        //            if(GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value.ToString() == GridProc["PROCESS", i].Value.ToString())
                        //            {
                        //                GridProc["PRO_RATE", i].Value = GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value.ToString();
                        //                GridProc["PRO_AMOUNT", i].Value = Convert.ToDouble(GridProc["REQ_QTY", GridProc.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value);
                        //            }
                        //        }

                        //    }                        
                            
                        //}
                        //else
                        //{
                        //    GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value = 0.00;
                        //}
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
                if (GridComm["COMM_NAME", GridComm.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridComm.CurrentCell.ColumnIndex == GridComm.Columns["QTY"].Index)
                    {
                        if (GridComm["COMM_NAME", GridComm.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            MessageBox.Show("INVALID COMMERCIAL", "Gainup");
                            GridComm.CurrentCell = GridComm["COMM_NAME", GridComm.CurrentCell.RowIndex];
                            GridComm.Focus();
                            GridComm.BeginEdit(true);
                            return;
                        }
                    }
                    else if (GridComm.CurrentCell.ColumnIndex == GridComm.Columns["RATE"].Index && Txt_Comm.Text.ToString() != String.Empty)
                    {
                        GridComm["RATE", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Comm.Text.ToString());
                        if (GridComm["RATE", GridComm.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridComm["RATE", GridComm.CurrentCell.RowIndex].Value = 0.00;
                        }
                        else if (Convert.ToDouble(GridComm["RATE", GridComm.CurrentCell.RowIndex].Value) > 0)
                        {
                            if (GridComm["CALC_MODE", GridComm.CurrentCell.RowIndex].Value.ToString() == "PER QTY")
                            {
                                GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(GridComm["QTY", GridComm.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridComm["RATE", GridComm.CurrentCell.RowIndex].Value);
                            }
                            else if (GridComm["CALC_MODE", GridComm.CurrentCell.RowIndex].Value.ToString() == "MANUAL")
                            {
                                GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value =  GridComm["RATE", GridComm.CurrentCell.RowIndex].Value;
                            }
                            else if (GridComm["CALC_MODE", GridComm.CurrentCell.RowIndex].Value.ToString() == "PERCENTAGE")
                            {
                                GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble( Convert.ToDouble(GridComm["QTY", GridComm.CurrentCell.RowIndex].Value) * (Convert.ToDouble(GridComm["RATE", GridComm.CurrentCell.RowIndex].Value) / 100));
                            }
                            else
                            {
                                GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(GridComm["QTY", GridComm.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridComm["RATE", GridComm.CurrentCell.RowIndex].Value);
                            }
                        }
                        else
                        {
                            GridComm["RATE", GridComm.CurrentCell.RowIndex].Value = 0.00;
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
                    if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["CONS"].Index && Txt_Trim.Text.ToString() != String.Empty)
                    {
                        GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Trim.Text.ToString());
                        if (GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value = 0;
                        }
                        else if (GridTrim["PLAN_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() == "/" && Convert.ToDouble(GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value) > 0)
                        {
                            GridTrim["REQ_QTY", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["BOM_QTY", GridTrim.CurrentCell.RowIndex].Value) / Convert.ToDouble(GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value);
                        }
                        else if (GridTrim["PLAN_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() == "*")
                        {
                            GridTrim["REQ_QTY", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridTrim["BOM_QTY", GridTrim.CurrentCell.RowIndex].Value);
                        }
                        else if (GridTrim["PLAN_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() == "M")
                        {
                            GridTrim["REQ_QTY", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value);
                        }
                        else
                        {
                            GridTrim["REQ_QTY", GridTrim.CurrentCell.RowIndex].Value = 0;
                        }

                        if (GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                            MessageBox.Show("INVALID PUR RATE ", "Gainup");
                            GridTrim.CurrentCell = GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex];
                            GridTrim.Focus();
                            GridTrim.BeginEdit(true);
                            return;
                        }
                        else if (Convert.ToDouble(GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value) > 0)
                        {
                            GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["REQ_QTY", GridTrim.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value);
                        }
                        else
                        {
                            GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                        }
                    }
                    else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["PLAN_TYPE"].Index && Txt_Trim.Text.ToString() != String.Empty)
                    {
                        GridTrim["PLAN_TYPE", GridTrim.CurrentCell.RowIndex].Value = Txt_Trim.Text.ToString();
                        if (GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value = 0;
                        }
                        else if (GridTrim["PLAN_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() == "/" && Convert.ToDouble(GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value) > 0)
                        {
                            GridTrim["REQ_QTY", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["BOM_QTY", GridTrim.CurrentCell.RowIndex].Value) / Convert.ToDouble(GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value);
                        }
                        else if (GridTrim["PLAN_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() == "*")
                        {
                            GridTrim["REQ_QTY", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridTrim["BOM_QTY", GridTrim.CurrentCell.RowIndex].Value);
                        }
                        else if (GridTrim["PLAN_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() == "M")
                        {
                            GridTrim["REQ_QTY", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value);
                        }
                        else
                        {
                            GridTrim["REQ_QTY", GridTrim.CurrentCell.RowIndex].Value = 0;
                        }

                        if (GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                            MessageBox.Show("INVALID PUR RATE ", "Gainup");
                            GridTrim.CurrentCell = GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex];
                            GridTrim.Focus();
                            GridTrim.BeginEdit(true);
                            return;
                        }
                        else if (Convert.ToDouble(GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value) > 0)
                        {
                            GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["REQ_QTY", GridTrim.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value);
                        }
                        else
                        {
                            GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                        }
                    }
                    else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["PUR_RATE"].Index && Txt_Trim.Text.ToString() != String.Empty)
                    {
                         GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Trim.Text.ToString());
                        if (GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                        }
                        else if (Convert.ToDouble(GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value) > 0)
                        {
                            GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["REQ_QTY", GridTrim.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value);
                        }
                        else
                        {
                            GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value = 0.00;
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
                        if (GridProc.CurrentCell.ColumnIndex == GridProc.Columns["PRO_RATE"].Index)
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
                if (GridComm["COMM_NAME", GridComm.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridComm["FLAG", GridComm.CurrentCell.RowIndex].Value.ToString() == "F")
                    {
                        if (GridComm.CurrentCell.ColumnIndex == GridComm.Columns["RATE"].Index)
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
                        if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["PLAN_TYPE"].Index)
                        {
                            if (e.KeyChar == Convert.ToInt32('/') || e.KeyChar == Convert.ToInt32('/'))
                            {
                                e.Handled = true;
                                Txt_Trim.Text = "/";
                            }
                            else if (e.KeyChar == Convert.ToInt32('*') || e.KeyChar == Convert.ToInt32('*'))
                            {
                                e.Handled = true;
                                Txt_Trim.Text = "*";
                            }
                            else if (e.KeyChar == Convert.ToInt32('M') || e.KeyChar == Convert.ToInt32('m'))
                            {
                                e.Handled = true;
                                Txt_Trim.Text = "M";
                            }
                            else
                            {
                                e.Handled = true;
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["CONS"].Index)
                        {
                            MyBase.Valid_Number(Txt_Trim, e);
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["PUR_RATE"].Index)
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
                        if (TxtOcnNo.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("Invalid OCN NO", "Gainup");
                            TxtOcnNo.Focus();                           
                            return;
                        }
                        if (TxtTotYrnPurAmt.Text.Trim() == String.Empty  || TxtTotWeight.Text.Trim() == String.Empty  || Convert.ToDouble(TxtTotWeight.Text.ToString()) == 0 || GridYarn.Rows.Count ==0 || Convert.ToDouble(TxtTotYrnPurAmt.Text.ToString()) == 0)
                        {
                            MessageBox.Show("Invalid Yarn Planning Details", "Gainup");
                            TxtTotWeight.Focus();                           
                            return;
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["ACCESS_TYPE"].Index)
                        {
                            if(GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            {
                                if(ChkCopy.Checked == false)
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Access", "Select 'GENERAL' Type Union Select 'SAMPLE' Type", String.Empty, 250);
                                    if (Dr != null)
                                    {
                                        GridTrim["ACCESS_TYPE", GridTrim.CurrentCell.RowIndex].Value = Dr["Type"].ToString();
                                        Txt_Trim.Text = Dr["Type"].ToString();
                                    }
                                }
                                else
                                {
                                    if(GridTrim.Rows.Count ==1 && MyParent._New)
                                    {
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Access", "Select Distinct Order_No, Ord_Item , OrdeR_ID, Ord_ItemId  From Socks_Trim_Planning_Fn() Where Ord_ItemID = " + TxtItem.Tag + " and Party_Code = " + TxtBuyer.Tag.ToString() + " and Order_ID != " + TxtOcnNo.Tag.ToString() + " Order by Order_No desc ", String.Empty, 150, 100);
                                            if (Dr != null)
                                            {
                                                    DataRow Dr3;
                                                    DataTable TDtT1 = new DataTable();
                                                    MyBase.Load_Data("Select SNO, ACCESS_TYPE, Item, Sample_No, Samp_Size, BOM_QTY, Size, Color, Plan_Type, Cons, Req_Qty, Pur_Rate, Pur_Amount, Item_ID, Color_Id, Size_ID, Sample_ID, SAMPLE_NO1, IsNull(SNo1,-1) SNo1, FLAG  From ( Select  0 SNO, 'SAMPLE' ACCESS_TYPE, B.Item, A.Sample_No, A.SIZE Samp_Size, A.Bom_Qty_Plan BOM_QTY, B.Size, B.Color, B.Plan_Type, B.Trim_Cons Cons, (Case When B.Plan_Type = '/' Then  Cast((A.Bom_Qty_Plan / B.Trim_Cons) as Numeric(20,2)) When B.Plan_Type = '*' Then  Cast((A.Bom_Qty_Plan * B.Trim_Cons) as Numeric(20,2)) Else B.Trim_Cons End) Req_Qty, B.Pur_Rate, B.Pur_Amount, B.Item_ID, B.Color_Id, B.Size_ID, A.Sample_ID, Cast(A.Sample_ID as Varchar(20)) + '-' + Cast(B.Item_ID as Varchar(20)) + '-' + Cast(B.Size_ID as Varchar(20)) + '-' + Cast(B.Color_Id as Varchar(20)) SAMPLE_NO1, B.SNo1, 'F' FLAG From Socks_Bom_Sample_Item_Trim_Fn() A Left Join (Select Sample_NO, Item, Color, Size, Item_ID, Color_Id, Size_ID, Plan_Type, Pur_Rate, Pur_Amount, SNo1, Sample_ID, Trim_Cons, Tot_Qty  From Socks_Trim_Planning_Fn () Where OrdeR_ID = " + Dr["OrdeR_ID"].ToString() + " and Access_Type = 'Sample') B On A.Sample_ID = B.Sample_ID and A.Sample_No = B.Sample_NO Where A.Order_ID = " + TxtOcnNo.Tag.ToString() + "   Union Select  0 SNO, 'GENERAL' ACCESS_TYPE, B.Item, '-' Sample_No, '-' Samp_Size, " + Convert.ToDouble(TxtBomQty.Text.ToString()) + " BOM_QTY, B.Size, B.Color, B.Plan_Type, B.Trim_Cons Cons, (Case When B.Plan_Type = '/' Then  Cast((" + Convert.ToDouble(TxtBomQty.Text.ToString()) + " / B.Trim_Cons) as Numeric(20,2)) When B.Plan_Type = '*' Then  Cast((" + Convert.ToDouble(TxtBomQty.Text.ToString()) + " * B.Trim_Cons) as Numeric(20,2)) Else B.Trim_Cons End) Req_Qty, B.Pur_Rate, (Case When B.Plan_Type = '/' Then  Cast((" + Convert.ToDouble(TxtBomQty.Text.ToString()) + " / B.Trim_Cons) as Numeric(20,2)) When B.Plan_Type = '*' Then  Cast((" + Convert.ToDouble(TxtBomQty.Text.ToString()) + " * B.Trim_Cons) as Numeric(20,2)) Else B.Trim_Cons End) * B.Pur_Rate Pur_Amount, B.Item_ID, B.Color_Id, B.Size_ID,  0 Sample_ID, Cast(0 as Varchar(20)) + '-' + Cast(B.Item_ID as Varchar(20)) + '-' + Cast(B.Size_ID as Varchar(20)) + '-' + Cast(B.Color_Id as Varchar(20)) SAMPLE_NO1, B.SNo1, 'F' FLAG From Socks_Trim_Planning_Fn () B Where OrdeR_ID = " + Dr["OrdeR_ID"].ToString() + " and Access_Type = 'General' ) A1 Order by Access_Type desc, Sno1 , Sample_No  ", ref TDtT1);
                                                    for(int p=0; p<=TDtT1.Rows.Count -1; p++)
                                                    {                           
                                                        Dr3 = Dt1.NewRow();
                                                        Dr3 = TDtT1.Rows[p];
                                                        Dt1.ImportRow (Dr3);                                               
                                                        if(p==0)
                                                        {
                                                            GridTrim.Rows.RemoveAt (GridTrim.CurrentCell.RowIndex);
                                                        }
                                                    }   
                                                    
                                                    GridTrim.Rows.RemoveAt (GridTrim.CurrentCell.RowIndex);

                                                    GridTrim.RefreshEdit ();
                                                    GridTrim.Refresh ();
                                                    //ChkCopy.Checked = false; 
                                                    GridTrim.CurrentCell = GridTrim["SIZE", GridTrim.CurrentCell.RowIndex ];
                                                    GridTrim.Focus();
                                                    GridTrim.BeginEdit(true);   
                                                    return;

                                            }
                                    }
                                }
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["ITEM"].Index)
                        {
                            if(GridTrim["ACCESS_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() == "GENERAL")
                            {
                                    //if(ChkCopy.Checked == false)
                                    //{
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Item", "Select  Item, ItemID From Socks_Trims_Item_Fn() Order by Item ", String.Empty, 250);
                                        if (Dr != null)
                                        {
                                            GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                                            GridTrim["ITEM_ID", GridTrim.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                                            GridTrim["BOM_QTY", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(TxtBomQty.Text.ToString());
                                            GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value = "0.00";
                                            GridTrim["PLAN_TYPE", GridTrim.CurrentCell.RowIndex].Value = "*";
                                            GridTrim["FLAG", GridTrim.CurrentCell.RowIndex].Value = "F";
                                            GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value = "";
                                            GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value = "";
                                            GridTrim["Sample_No", GridTrim.CurrentCell.RowIndex].Value = "-";
                                            GridTrim["Samp_Size", GridTrim.CurrentCell.RowIndex].Value = "-";
                                            GridTrim["Sample_ID", GridTrim.CurrentCell.RowIndex].Value = 0;
                                            GridTrim["SNo1", GridTrim.CurrentCell.RowIndex].Value =  GridTrim["SNo", GridTrim.CurrentCell.RowIndex].Value;
                                            GridTrim["Sample_No1", GridTrim.CurrentCell.RowIndex].Value = GridTrim["Sample_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + "-" +  Dr["ItemID"].ToString();
                                            Txt_Trim.Text = Dr["ITEM"].ToString();
                                        }
                                    //}                                   
                            }
                            else
                            {
                                if(ChkCopy.Checked == false && GridTrim["SAMPLE_NO", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Item", "Select  Item, ItemID From Socks_Trims_Item_Fn() Order by Item ", String.Empty, 250);
                                        if (Dr != null)
                                        {
                                            DataRow Dr2;
                                            DataTable TDtT = new DataTable();
                                            MyBase.Load_Data("Select Distinct 0 SNO, 'SAMPLE' ACCESS_TYPE, '" + Dr["Item"].ToString() + "' ITEM, Sample_No, Size Samp_Size, Bom_Qty_Plan BOM_Qty, '' Size, '' Color, '*' Plan_Type, Bom_Qty_Plan Req_Qty, 0.00 PUR_RATE, 0.00 PUR_AMOUNT, " + Dr["ItemID"].ToString() + " Item_ID, 0 Color_ID, 0 Size_ID, Sample_ID,  Cast(Sample_ID as Varchar(20)) + '-' + Cast(" + Dr["ItemID"].ToString() + " as Varchar(20)) + '-' + Cast(0 as Varchar(20)) + '-' + Cast(0 as Varchar(20)) SAMPLE_NO1,  " + Convert.ToInt32(GridTrim.CurrentCell.RowIndex) + 1 + " SNo1, 'F' FLAG  From Socks_Bom_Sample_Item_Trim_Fn() Where Order_Id = " + TxtOcnNo.Tag + " and ItemID = " + TxtItem.Tag + " Order by Sample_No  ", ref TDtT);
                                            for(int p=0; p<=TDtT.Rows.Count -1; p++)
                                            {                           
                                                Dr2 = Dt1.NewRow();
                                                Dr2 = TDtT.Rows[p];
                                                Dt1.ImportRow (Dr2);                                               
                                                if(p==0)
                                                {
                                                    GridTrim.Rows.RemoveAt (GridTrim.CurrentCell.RowIndex);
                                                }
                                            }   
                                            
                                            GridTrim.Rows.RemoveAt (GridTrim.CurrentCell.RowIndex);

                                            GridTrim.RefreshEdit();
                                            GridTrim.Refresh ();
                                            
                                            GridTrim.CurrentCell = GridTrim["SIZE", GridTrim.CurrentCell.RowIndex - (TDtT.Rows.Count-1)];
                                            GridTrim.Focus();
                                            GridTrim.BeginEdit(true);   
                                            return;                                          
                                        }
                                    }
                                    else if(ChkCopy.Checked == true && GridTrim["SAMPLE_NO", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Item", "Select  Item, ItemID From Socks_Trims_Item_Fn() Order by Item ", String.Empty, 250);
                                        if (Dr != null)
                                        {
                                            GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                                            GridTrim["ITEM_ID", GridTrim.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                                           // GridTrim["BOM_QTY", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(TxtBomQty.Text.ToString());
                                            if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)                                        
                                            {
                                                DataTable TDttr = new DataTable();
                                                MyBase.Load_Data("Select Rate From Socks_Budget_App_Rate_Max_Fn() Where Type = 'Trims' and ItemID = " + GridTrim["ITEM_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + " and SizeID = " + GridTrim["Size_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + " and ColorID = " + GridTrim["Color_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + " ", ref TDttr);
                                                if(TDttr.Rows.Count >0)
                                                {
                                                    GridTrim["Pur_Rate", GridTrim.CurrentCell.RowIndex].Value  = TDttr.Rows[0][0].ToString();
                                                }
                                                else
                                                {
                                                    GridTrim["Pur_Rate", GridTrim.CurrentCell.RowIndex].Value  = "0.00";
                                                }
                                            }
                                            else
                                            {
                                                GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value = "0.00";
                                            }
                                            GridTrim["PLAN_TYPE", GridTrim.CurrentCell.RowIndex].Value = "/";
                                            GridTrim["FLAG", GridTrim.CurrentCell.RowIndex].Value = "F";
                                            GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value = "";
                                            GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value = "";                                                                                        
                                            GridTrim["Sample_No1", GridTrim.CurrentCell.RowIndex].Value = GridTrim["Sample_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + "-" +  Dr["ItemID"].ToString();
                                            Txt_Trim.Text = Dr["ITEM"].ToString();
                                        }
                                    }


                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["SIZE"].Index)
                        {
                            //if(GridTrim["ACCESS_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() == "GENERAL")
                            //{
                                if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Size", "Select Distinct Size, SizeID  From Socks_Trims_Size_Fn() Order by Size  ", String.Empty, 250);
                                    if (Dr != null)
                                    {
                                        GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                                        GridTrim["SIZE_ID", GridTrim.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                                        GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value = "";
                                        GridTrim["Sample_No1", GridTrim.CurrentCell.RowIndex].Value = GridTrim["Sample_No1", GridTrim.CurrentCell.RowIndex].Value.ToString() + "-" + Dr["SizeID"].ToString();
                                        if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                                        {
                                            DataTable TDttr = new DataTable();
                                            MyBase.Load_Data("Select Rate From Socks_Budget_App_Rate_Max_Fn() Where Type = 'Trims' and ItemID = " + GridTrim["ITEM_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + " and SizeID = " + GridTrim["Size_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + " and ColorID = " + GridTrim["Color_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + " ", ref TDttr);
                                            if(TDttr.Rows.Count >0)
                                            {
                                                GridTrim["Pur_Rate", GridTrim.CurrentCell.RowIndex].Value  = TDttr.Rows[0][0].ToString();
                                            }
                                            else
                                            {
                                                GridTrim["Pur_Rate", GridTrim.CurrentCell.RowIndex].Value  = "0.00";
                                            }
                                        }
                                        Txt_Trim.Text = Dr["SIZE"].ToString();
                                    }
                                }
                            //}
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["COLOR"].Index)
                        {
                           // if(GridTrim["ACCESS_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() == "GENERAL")
                           // {
                                if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)                                
                                {                                    
                                    Dr = Tool.Selection_Tool_Except_New ("SAMPLE_NO1", this, 30, 70, ref Dt1, SelectionTool_Class.ViewType.NormalView, "Color", "Select Distinct Color, ColorID , '" + GridTrim["Sample_No1", GridTrim.CurrentCell.RowIndex].Value.ToString() + "' + '-' + Cast(ColorID as Varchar(20)) Sample_No1 From color Where Color is Not Null and Color Not Like '%ZZZ%' and Len(Color) >2 Order by Color", String.Empty, 250);
                                    if (Dr != null)
                                    {
                                        GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                                        GridTrim["COLOR_ID", GridTrim.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();                                    
                                        GridTrim["Sample_No1", GridTrim.CurrentCell.RowIndex].Value = GridTrim["Sample_No1", GridTrim.CurrentCell.RowIndex].Value.ToString() + "-" + Dr["ColorID"].ToString();

                                        if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                                        {
                                            DataTable TDttr = new DataTable();
                                            MyBase.Load_Data("Select Rate From Socks_Budget_App_Rate_Max_Fn() Where  Type = 'Trims' and ItemID = " + GridTrim["ITEM_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + " and SizeID = " + GridTrim["Size_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + " and ColorID = " + GridTrim["Color_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + " ", ref TDttr);
                                            if(TDttr.Rows.Count >0)
                                            {
                                                GridTrim["Pur_Rate", GridTrim.CurrentCell.RowIndex].Value  = TDttr.Rows[0][0].ToString();
                                            }
                                            else
                                            {
                                                GridTrim["Pur_Rate", GridTrim.CurrentCell.RowIndex].Value  = "0.00";
                                            }
                                        }
                                        Txt_Trim.Text = Dr["COLOR"].ToString();
                                    }
                                }
                          //  }
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
                    if (TxtOcnNo.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Invalid OCN NO", "Gainup");
                        TxtOcnNo.Focus();
                        return;
                    }
                    if (TxtTotProAmt.Text.Trim() == String.Empty || TxtTotProQty.Text.Trim() == String.Empty || Convert.ToDouble(TxtTotProAmt.Text.ToString()) == 0 || GridProc.Rows.Count == 0 || Convert.ToDouble(TxtTotProQty.Text.ToString()) == 0)
                    {
                        MessageBox.Show("Invalid Process Planning Details", "Gainup");
                        TxtTotProAmt.Focus();
                        return;
                    }
                    else if (GridComm.CurrentCell.ColumnIndex == GridComm.Columns["COMM_NAME"].Index)
                    {
                        Dr = Tool.Selection_Tool_Except_New("COMM_NAME", this, 30, 70, ref Dt3, SelectionTool_Class.ViewType.NormalView, "Commercial", "Select A.Commercial COMM_NAME, A.commercialid CommID, IsNull(B.Rate,0.00) Rate, Cast((IsNull(B.RAte,0.00)  * " + Convert.ToDouble(TxtBomQty.Text.ToString()) + ") as Numeric(30,2)) Amount From CommercialMas A  Left join Socks_Budget_App_Rate_Max_Fn() B On A.commercialid = B.ItemID and B.Type = 'Commercial'  Order by A.Commercial ", String.Empty, 250);
                        if (Dr != null)
                        {
                            GridComm["COMM_NAME", GridComm.CurrentCell.RowIndex].Value = Dr["COMM_NAME"].ToString();
                            GridComm["COMM_ID", GridComm.CurrentCell.RowIndex].Value = Dr["CommID"].ToString();
                            GridComm["QTY", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(TxtBomQty.Text.ToString());
                            GridComm["RATE", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["Rate"].ToString());
                            GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["Amount"].ToString());
                            GridComm["CALC_MODE", GridComm.CurrentCell.RowIndex].Value = "PER QTY";
                            GridComm["FLAG", GridComm.CurrentCell.RowIndex].Value = "F";
                            Txt_Comm.Text = Dr["COMM_NAME"].ToString();
                        }
                    }                   
                    else if (GridComm.CurrentCell.ColumnIndex == GridComm.Columns["CALC_MODE"].Index)
                    {
                        if (GridComm["COMM_NAME", GridComm.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "CALC MODE", "Select  'PER QTY' Mode Union Select 'PERCENTAGE' Mode Union Select 'MANUAL' Mode ", String.Empty, 250);
                            if (Dr != null)
                            {
                                GridComm["CALC_MODE", GridComm.CurrentCell.RowIndex].Value = Dr["Mode"].ToString();

                                if (GridComm["CALC_MODE", GridComm.CurrentCell.RowIndex].Value.ToString() == "PER QTY")
                                {
                                    GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(GridComm["QTY", GridComm.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridComm["RATE", GridComm.CurrentCell.RowIndex].Value);
                                }
                                else if (GridComm["CALC_MODE", GridComm.CurrentCell.RowIndex].Value.ToString() == "MANUAL")
                                {
                                    GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value = GridComm["RATE", GridComm.CurrentCell.RowIndex].Value;
                                }
                                else if (GridComm["CALC_MODE", GridComm.CurrentCell.RowIndex].Value.ToString() == "PERCENTAGE")
                                {
                                    GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(Convert.ToDouble(GridComm["QTY", GridComm.CurrentCell.RowIndex].Value) * (Convert.ToDouble(GridComm["RATE", GridComm.CurrentCell.RowIndex].Value) / 100));
                                }
                                else
                                {
                                    GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(GridComm["RATE", GridComm.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(TxtBomQty.Text.ToString());
                                }
                                Txt_Comm.Text = Dr["Mode"].ToString();
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


         void Txt_Spcl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if(MyParent._New)
                    {
                        e.Handled = true;
                        return;
                    }
                    if (TxtBuyer.Text.Trim() == String.Empty)
                    {
                        MessageBox.Show("Invalid Buyer Name", "Gainup");
                        TxtBuyer.Focus();
                        return;
                    }
                    if (TxtOcnNo.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Invalid OCN NO", "Gainup");
                        TxtOcnNo.Focus();
                        return;
                    }
                    if (TxtTotProAmt.Text.Trim() == String.Empty || TxtTotProQty.Text.Trim() == String.Empty || Convert.ToDouble(TxtTotProAmt.Text.ToString()) == 0 || GridProc.Rows.Count == 0 || Convert.ToDouble(TxtTotProQty.Text.ToString()) == 0)
                    {
                        MessageBox.Show("Invalid Process Planning Details", "Gainup");
                        TxtTotProAmt.Focus();
                        return;
                    }
                    else if (GridSpcl.CurrentCell.ColumnIndex == GridSpcl.Columns["ACCESS_TYPE"].Index && GridSpcl["ACCESS_TYPE", GridSpcl.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Access Type", "Select 'EXCESS' TYPE Union Select 'REPLACE' TYPE ", String.Empty, 250);
                        if (Dr != null)
                        {
                            GridSpcl["ACCESS_TYPE", GridSpcl.CurrentCell.RowIndex].Value = Dr["TYPE"].ToString();                            
                            GridSpcl["FLAG", GridSpcl.CurrentCell.RowIndex].Value = "F";
                            Txt_Spcl.Text = Dr["TYPE"].ToString();
                        }
                    }                   
                    else if (GridSpcl.CurrentCell.ColumnIndex == GridSpcl.Columns["PLAN_ITEM"].Index)
                    {
                        if (GridSpcl["ACCESS_TYPE", GridSpcl.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {      
                            if (GridSpcl["ACCESS_TYPE", GridSpcl.CurrentCell.RowIndex].Value.ToString() == "REPLACE")
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "PLAN_ITEM", " Select  ITEM, COLOR, SIZE, BOM_CONS, PUR_RATE, PUR_AMOUNT, DYE_MODE, DYE_COLOR, DYE_RATE, DYE_AMOUNT, DYE_ITEMID, (ITEM + ' - ' + COLOR + ' - ' + SIZE) PLAN_ITEM,  ItemID, COLORID, SIZEID, CAst(ITEMID as VArchar(20)) + '-' + Cast(SIZEID as Varchar(20)) + '-' + CASt(COLORID as VArchar(20)) Plan_Dtl_ID , SNo From Socks_Yarn_Planning_Fn() Where RoWID = " + Code + " and  Spl_Req_Mode = 'F' Order by Sno  ", String.Empty, 120, 120, 120, 120, 120, 100, 140, 100, 120, 120);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool_Except_New("Plan_Dtl_ID", this, 30, 70, ref Dt4, SelectionTool_Class.ViewType.NormalView, "PLAN_ITEM", " Select  ITEM, COLOR, SIZE, BOM_CONS, PUR_RATE, PUR_AMOUNT, DYE_MODE, DYE_COLOR, DYE_RATE, DYE_AMOUNT, DYE_ITEMID, (ITEM + ' - ' + COLOR + ' - ' + SIZE) PLAN_ITEM,  ItemID, COLORID, SIZEID, CAst(ITEMID as VArchar(20)) + '-' + Cast(SIZEID as Varchar(20)) + '-' + CASt(COLORID as VArchar(20)) Plan_Dtl_ID , SNo From Socks_Yarn_Planning_Fn() Where RoWID = " + Code + " and  Spl_Req_Mode = 'F' Order by Sno  ", String.Empty, 120, 120, 120, 120, 120, 100, 140, 100, 120, 120);
                            }
                            if (Dr != null)
                            {
                                GridSpcl["ITEM", GridSpcl.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                                GridSpcl["ITEMID", GridSpcl.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                                GridSpcl["SIZE", GridSpcl.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                                GridSpcl["SIZEID", GridSpcl.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                                GridSpcl["COLOR", GridSpcl.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                                GridSpcl["COLORID", GridSpcl.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();
                                GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value = Dr["Bom_Cons"].ToString();
                                GridSpcl["BOM_CONS1", GridSpcl.CurrentCell.RowIndex].Value = Dr["Bom_Cons"].ToString();
                                GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value = Dr["Pur_Rate"].ToString();
                                GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = Dr["Pur_Amount"].ToString();
                                GridSpcl["FLAG", GridSpcl.CurrentCell.RowIndex].Value = "F";
                                GridSpcl["REQ_ITEMID", GridSpcl.CurrentCell.RowIndex].Value = Dr["ITEMID"].ToString();
                                GridSpcl["REQ_SIZEID", GridSpcl.CurrentCell.RowIndex].Value = Dr["SIZEID"].ToString();
                                GridSpcl["REQ_COLORID", GridSpcl.CurrentCell.RowIndex].Value = Dr["COLORID"].ToString();
                                GridSpcl["PLAN_DTL_ID", GridSpcl.CurrentCell.RowIndex].Value = Dr["PLAN_DTL_ID"].ToString();
                                GridSpcl["PLAN_ITEM", GridSpcl.CurrentCell.RowIndex].Value = Dr["Plan_Item"].ToString();
                                GridSpcl["DYE_MODE", GridSpcl.CurrentCell.RowIndex].Value = Dr["Dye_Mode"].ToString();
                                GridSpcl["DYE_RATE", GridSpcl.CurrentCell.RowIndex].Value = Dr["Dye_Rate"].ToString();
                                GridSpcl["DYE_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = Dr["Dye_Amount"].ToString();
                                GridSpcl["DYE_COLOR", GridSpcl.CurrentCell.RowIndex].Value = Dr["Dye_Color"].ToString();
                                GridSpcl["DYE_ITEMID", GridSpcl.CurrentCell.RowIndex].Value = Dr["Dye_ItemID"].ToString();
                                GridSpcl["SNO1", GridSpcl.CurrentCell.RowIndex].Value = Dr["SNo"].ToString();
                                Txt_Spcl.Text = Dr["PLAN_ITEM"].ToString();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Access Type", "Gainup");
                            GridSpcl.CurrentCell = GridSpcl["ACCESS_TYPE", GridSpcl.CurrentCell.RowIndex];
                            GridSpcl.Focus();
                            GridSpcl.BeginEdit(true);   
                            return;                            
                        }
                    }
                        if (GridSpcl["ACCESS_TYPE", GridSpcl.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridSpcl["PLAN_ITEM", GridSpcl.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        { 
                            if(GridSpcl["ACCESS_TYPE", GridSpcl.CurrentCell.RowIndex].Value.ToString() == "REPLACE")
                            {
                                if (GridSpcl.CurrentCell.ColumnIndex == GridSpcl.Columns["ITEM"].Index)
                                {
                                     Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Item", "Select  Item, ItemID From Item Where Item_Type = 'YARN' Order by Item ", String.Empty, 250);
                                     if (Dr != null)
                                     {
                                         GridSpcl["ITEM", GridSpcl.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                                         GridSpcl["ITEMID", GridSpcl.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                                         Txt_Spcl.Text = Dr["ITEM"].ToString();
                                     }
                                }
                                else if (GridSpcl.CurrentCell.ColumnIndex == GridSpcl.Columns["SIZE"].Index)
                                {
                                     Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Size", "Select  Size, SizeID From Size Where Item_Type = 'YARN' Order by Size ", String.Empty, 250);
                                     if (Dr != null)
                                     {
                                         GridSpcl["SIZE", GridSpcl.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                                         GridSpcl["SIZEID", GridSpcl.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                                         Txt_Spcl.Text = Dr["SIZE"].ToString();
                                     }
                                    
                                }
                                else if (GridSpcl.CurrentCell.ColumnIndex == GridSpcl.Columns["COLOR"].Index)
                                {
                                     Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Color", "Select  Color, ColorID From Color Where Color Not Like '%ZZZ%' Order by Color ", String.Empty, 250);
                                     if (Dr != null)
                                     {
                                         GridSpcl["COLOR", GridSpcl.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                                         GridSpcl["COLORID", GridSpcl.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();

                       if(GridSpcl["DYE_MODE", GridSpcl.CurrentCell.RowIndex].Value.ToString() == "Y" && GridSpcl["COLOR", GridSpcl.CurrentCell.RowIndex].Value.ToString().ToUpper() != "R.WHITE" && GridSpcl["ITEM", GridSpcl.CurrentCell.RowIndex].Value.ToString().ToUpper() == "NYLON")
                       {
                           GridSpcl["DYE_COLOR", GridSpcl.CurrentCell.RowIndex].Value =  "R.White";
                           GridSpcl["DYE_ITEMID", GridSpcl.CurrentCell.RowIndex].Value =  3343;
                           DataTable TDtd = new DataTable();
                           MyBase.Load_Data("Select IsNull(Rate,0.00) Rate FRom Socks_Budget_App_Rate_Max_Fn() Where Type = 'Yarn' and Colorid = 3343 and Itemid = " + GridSpcl["ITEMID", GridSpcl.CurrentCell.RowIndex].Value + " and Sizeid = " + GridSpcl["SIZEID", GridSpcl.CurrentCell.RowIndex].Value + " ", ref TDtd);

                           GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value = Convert.ToDouble(TDtd.Rows[0][0].ToString());
                           GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = Convert.ToDouble(TDtd.Rows[0][0].ToString()) * (Convert.ToDouble(GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value.ToString()));
                       }
                       else if(GridSpcl["DYE_MODE", GridSpcl.CurrentCell.RowIndex].Value.ToString() == "Y" && GridSpcl["COLOR", GridSpcl.CurrentCell.RowIndex].Value.ToString().ToUpper() != "GREIGE")
                       {
                           GridSpcl["DYE_COLOR", GridSpcl.CurrentCell.RowIndex].Value =  "GREIGE";
                           GridSpcl["DYE_ITEMID", GridSpcl.CurrentCell.RowIndex].Value =  867;
                           DataTable TDtd = new DataTable();
                           MyBase.Load_Data("Select IsNull(Rate,0.00) Rate FRom Socks_Budget_App_Rate_Max_Fn() Where Type = 'Yarn' and Colorid = 867 and Itemid = " + GridSpcl["ITEMID", GridSpcl.CurrentCell.RowIndex].Value + " and Sizeid = " + GridSpcl["SIZEID", GridSpcl.CurrentCell.RowIndex].Value + " ", ref TDtd);

                           GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value = Convert.ToDouble(TDtd.Rows[0][0].ToString());
                           GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = Convert.ToDouble(TDtd.Rows[0][0].ToString()) * (Convert.ToDouble(GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value.ToString()));
                       }
                       else
                       {
                           GridSpcl["DYE_COLOR", GridSpcl.CurrentCell.RowIndex].Value =  GridSpcl["COLOR", GridSpcl.CurrentCell.RowIndex].Value;
                           GridSpcl["DYE_ITEMID", GridSpcl.CurrentCell.RowIndex].Value =  GridSpcl["COLORID", GridSpcl.CurrentCell.RowIndex].Value;                           
                           GridSpcl["DYE_RATE", GridSpcl.CurrentCell.RowIndex].Value =  0.00;
                           GridSpcl["DYE_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value =  0.00;
                       }
                                         Txt_Spcl.Text = Dr["COLOR"].ToString();
                                     }
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


        void Txt_Spcl_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GridSpcl["ACCESS_TYPE", GridSpcl.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridSpcl["FLAG", GridSpcl.CurrentCell.RowIndex].Value.ToString() == "F")
                    {
                        if (GridSpcl.CurrentCell.ColumnIndex == GridSpcl.Columns["PUR_RATE"].Index || GridSpcl.CurrentCell.ColumnIndex == GridSpcl.Columns["BOM_CONS"].Index)
                        {
                            MyBase.Valid_Decimal(Txt_Spcl, e);
                        }                                                                                      
                        else if (GridSpcl.CurrentCell.ColumnIndex == GridSpcl.Columns["DYE_MODE"].Index)
                        {
                            if (GridSpcl["COLOR", GridSpcl.CurrentCell.RowIndex].Value.ToString().ToUpper() != "GREIGE" || GridSpcl["COLOR", GridSpcl.CurrentCell.RowIndex].Value.ToString().ToUpper() != "R.WHITE")
                            {
                                MyBase.Valid_Yes_OR_No(Txt_Spcl,e);
                            }                        
                            else
                            {
                                MyBase.Valid_Null(Txt_Spcl, e);
                            }
                        }
                        else if (GridSpcl.CurrentCell.ColumnIndex == GridSpcl.Columns["DYE_RATE"].Index)
                        {
                            if(GridSpcl["DYE_MODE", GridSpcl.CurrentCell.RowIndex].Value.ToString() == "Y")
                            {
                                MyBase.Valid_Decimal(Txt_Spcl, e);
                            }
                            else
                            {
                                MyBase.Valid_Null(Txt_Spcl, e);
                            }
                        }      
                        else
                        {
                            MyBase.Valid_Null(Txt_Spcl, e);
                        }
                    }
                }
                else
                {
                    MyBase.Valid_Null(Txt_Spcl, e);
                }
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Spcl_Leave(object sender, EventArgs e)
        {
            try
            {                
                if (GridSpcl.CurrentCell.ColumnIndex == GridSpcl.Columns["PUR_RATE"].Index && Txt_Spcl.Text.ToString() != String.Empty)
                {
                       GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Spcl.Text.ToString());
                       if(GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                       {                            
                            GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value =  0.00;
                            GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value =  0.00;                            
                       }
                       else if(Convert.ToDouble(GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value) >0)
                       {
                            GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value)) * Convert.ToDouble(GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value);
                       }                      
                       else
                       {
                            //MessageBox.Show("Invalid PUR_RATE");
                            GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value =  0.00;
                            GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value =  0.00;
                            GridSpcl.CurrentCell = GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex];                        
                            GridSpcl.Focus();
                            GridSpcl.BeginEdit(true);                            
                            return;                                        
                       }                         
                }
                else if (GridSpcl.CurrentCell.ColumnIndex == GridSpcl.Columns["BOM_CONS"].Index && Txt_Spcl.Text.ToString() != String.Empty)
                {
                       GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Spcl.Text.ToString());
                       if(GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                       {                            
                            GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value =  0.00;                            
                       }                          
                       else if(Convert.ToDouble(GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value) > Convert.ToDouble(GridSpcl["BOM_CONS1", GridSpcl.CurrentCell.RowIndex].Value))
                       {
                            MessageBox.Show("Invalid BOM_CONS");
                            GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value =  0.00;                           
                            GridSpcl.CurrentCell = GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex];                        
                            GridSpcl.Focus();
                            GridSpcl.BeginEdit(true);                            
                            return;    
                       }
                       else
                       {
                          
                       }

                       if(GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                       {                            
                            GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value =  0.00;
                            GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value =  0.00;                            
                       }
                       else if(Convert.ToDouble(GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value) >0)
                       {
                            GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value)) * Convert.ToDouble(GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value);
                       }                      
                       else
                       {
                            // MessageBox.Show("Invalid PUR_RATE");
                            GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value =  0.00;
                            GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value =  0.00;
                            GridSpcl.CurrentCell = GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex];                        
                            GridSpcl.Focus();
                            GridSpcl.BeginEdit(true);                            
                            return;                                        
                       }                         
                }
                else if (GridSpcl.CurrentCell.ColumnIndex == GridSpcl.Columns["DYE_MODE"].Index && Txt_Spcl.Text.ToString() != String.Empty)
                {
                       GridSpcl["DYE_MODE", GridSpcl.CurrentCell.RowIndex].Value = Txt_Spcl.Text.ToString();
                       if(GridSpcl["DYE_MODE", GridSpcl.CurrentCell.RowIndex].Value.ToString() == "Y" && GridSpcl["COLOR", GridSpcl.CurrentCell.RowIndex].Value.ToString().ToUpper() != "R.WHITE" && GridSpcl["ITEM", GridSpcl.CurrentCell.RowIndex].Value.ToString().ToUpper() == "NYLON")
                       {
                           GridSpcl["DYE_COLOR", GridSpcl.CurrentCell.RowIndex].Value =  "R.White";
                           GridSpcl["DYE_ITEMID", GridSpcl.CurrentCell.RowIndex].Value =  3343;
                           DataTable TDtd = new DataTable();
                           MyBase.Load_Data("Select IsNull(Rate,0.00) Rate FRom Socks_Budget_App_Rate_Max_Fn() Where Type = 'Yarn' and Colorid = 3343 and Itemid = " + GridSpcl["ITEMID", GridSpcl.CurrentCell.RowIndex].Value + " and Sizeid = " + GridSpcl["SIZEID", GridSpcl.CurrentCell.RowIndex].Value + " ", ref TDtd);

                           GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value = Convert.ToDouble(TDtd.Rows[0][0].ToString());
                           GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = Convert.ToDouble(TDtd.Rows[0][0].ToString()) * (Convert.ToDouble(GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value.ToString()));
                       }
                       else if(GridSpcl["DYE_MODE", GridSpcl.CurrentCell.RowIndex].Value.ToString() == "Y" && GridSpcl["COLOR", GridSpcl.CurrentCell.RowIndex].Value.ToString().ToUpper() != "GREIGE")
                       {
                           GridSpcl["DYE_COLOR", GridSpcl.CurrentCell.RowIndex].Value =  "GREIGE";
                           GridSpcl["DYE_ITEMID", GridSpcl.CurrentCell.RowIndex].Value =  867;
                           DataTable TDtd = new DataTable();
                           MyBase.Load_Data("Select IsNull(Rate,0.00) Rate FRom Socks_Budget_App_Rate_Max_Fn() Where Type = 'Yarn' and Colorid = 867 and Itemid = " + GridSpcl["ITEMID", GridSpcl.CurrentCell.RowIndex].Value + " and Sizeid = " + GridSpcl["SIZEID", GridSpcl.CurrentCell.RowIndex].Value + " ", ref TDtd);
                           if(TDtd.Rows.Count >0)
                           {
                            GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value = Convert.ToDouble(TDtd.Rows[0][0].ToString());
                            GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = Convert.ToDouble(TDtd.Rows[0][0].ToString()) * (Convert.ToDouble(GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value.ToString()));
                           }                           
                       }
                       else
                       {
                           GridSpcl["DYE_COLOR", GridSpcl.CurrentCell.RowIndex].Value =  GridSpcl["COLOR", GridSpcl.CurrentCell.RowIndex].Value;
                           GridSpcl["DYE_ITEMID", GridSpcl.CurrentCell.RowIndex].Value =  GridSpcl["COLORID", GridSpcl.CurrentCell.RowIndex].Value;                           
                           GridSpcl["DYE_RATE", GridSpcl.CurrentCell.RowIndex].Value =  0.00;
                           GridSpcl["DYE_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value =  0.00;
                       }
                }
                else if (GridSpcl.CurrentCell.ColumnIndex == GridSpcl.Columns["DYE_RATE"].Index && Txt_Spcl.Text.ToString() != String.Empty)
                {                    
                    if(GridSpcl["DYE_MODE", GridSpcl.CurrentCell.RowIndex].Value.ToString() == "Y")
                       {
                           GridSpcl["DYE_RATE", GridSpcl.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt_Spcl.Text.ToString());
                           if(GridSpcl["DYE_RATE", GridSpcl.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                           {
                               GridSpcl["DYE_RATE", GridSpcl.CurrentCell.RowIndex].Value =  0.00;
                               GridSpcl["DYE_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value =  0.00;                             
                           }
                           else if(Convert.ToDouble(GridSpcl["DYE_RATE", GridSpcl.CurrentCell.RowIndex].Value) >0)
                           {
                                GridSpcl["DYE_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value)) * Convert.ToDouble(GridSpcl["DYE_RATE", GridSpcl.CurrentCell.RowIndex].Value);
                           }                      
                           else
                           {
                                GridSpcl["DYE_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = 0.00;                              
                                GridSpcl["DYE_RATE", GridSpcl.CurrentCell.RowIndex].Value =  0.00;                                                                
                           }
                        }
                    else 
                    {
                                  GridSpcl["DYE_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = 0.00;
                    }
                }       
                Total_Count();
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
                        if (TxtOcnNo.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("Invalid OCN NO", "Gainup");
                            TxtOcnNo.Focus();                           
                            return;
                        }
                        if (TxtTotWeight.Text.Trim() == String.Empty  || Convert.ToDouble(TxtTotWeight.Text.ToString()) == 0 || Convert.ToDouble(TxtTotYrnPurAmt.Text.ToString()) == 0 || GridYarn.Rows.Count ==0 || TxtTotTrimQty.Text.Trim() == String.Empty  || Convert.ToDouble(TxtTotTrmPurAmt.Text.ToString()) == 0 || GridTrim.Rows.Count ==0)
                        {
                            MessageBox.Show("Invalid Yarn & Trim Planning Details", "Gainup");
                            TxtTotWeight.Focus();                           
                            return;
                        }                      
                        else if (GridProc.CurrentCell.ColumnIndex == GridProc.Columns["PROCESS"].Index)
                        {
                            if(GridProc["PROC_ID", GridProc.CurrentCell.RowIndex].Value.ToString() != "152" && GridProc["PROC_ID", GridProc.CurrentCell.RowIndex].Value.ToString() != "163" && GridProc["PROC_ID", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            {
                                Dr = Tool.Selection_Tool_Except_New("PROC_ID",this, 30, 70, ref Dt2, SelectionTool_Class.ViewType.NormalView, "Process", "Select Process, ProcessID PROC_ID From Process Where Socks_Plan_Avail = 'T' and Processid Not In (158, 152, 163) Order by process  ", String.Empty, 250);
                                if (Dr != null)
                                {
                                    DataRow Dr1;
                                    DataTable TDtP = new DataTable();
                                    MyBase.Load_Data("Select Distinct 0 SNO,  B.PROCESS,  A.SAMPLE_NO, A.Bom_Qty_Plan BOM_QTY, A.SIZE,  A.Bom_Qty_Plan REQ_QTY, IsNull(C.Rate,0.00) PRO_RATE, Cast(((IsNull(C.Rate,0.00)) * A.BOM_Qty_Plan) as Numeric(30,2)) PRO_AMOUNT, B.processid PROC_ID, A.SAMPLE_ID, A.SIZEID, CAst(B.processid as VArchar(10)) + '-' + CAst(A.SAMPLE_ID as Varchar(20)) + '-' + CAst(A.SIZEID as Varchar(20)) + '-' + CAst(A.ItemID as Varchar(20)) SAMPLE_NO1, 'F' FLAG  From Socks_Bom_Sample_Item_Proc_Fn() A Left Join Process B On B.processid in (" + Dr["PROC_ID"].ToString() + ") Left Join Socks_Budget_App_Rate_Max_Fn() C On A.ItemID = " + TxtItem.Tag + " and A.Sample_ID = C.ColorID and B.processid  = C.ItemID and C.Type = 'Process' Where A.Order_ID = " + TxtOcnNo.Tag + " and A.ItemID = " + TxtItem.Tag + " Order by A.Sample_No, A.Size ", ref TDtP);
                                    for(int p=0; p<=TDtP.Rows.Count -1; p++)
                                    {                           
                                        Dr1 = Dt2.NewRow();
                                        Dr1 = TDtP.Rows[p];
                                        Dt2.ImportRow (Dr1);
                                    }   

                                    GridProc.Rows.RemoveAt (GridProc.CurrentCell.RowIndex + 1);
                                    GridProc.Rows.RemoveAt (GridProc.CurrentCell.RowIndex);

                                    GridProc.RefreshEdit ();
                                    GridProc.Refresh ();

                                    GridProc.CurrentCell = GridProc["PROCESS", GridProc.Rows.Count -1];
                                    GridProc.Focus();
                                    GridProc.BeginEdit(true);   
                                    return;
                                    /*
                                    DataTable Dt2 = new DataTable();
                                    Dt2 = (DataTable)GridProc.DataSource;
                                    DataTable TDtP = new DataTable();
                                    MyBase.Load_Data("Select 0 SNO,  B.PROCESS,  SAMPLE_NO, Bom_Qty_Plan BOM_QTY, Yarn_Item ITEM, SIZE, COLOR, Bom_Qty_Plan REQ_QTY, 0.00 PRO_RATE, 0.00 PRO_AMOUNT, B.processid PROC_ID, SAMPLE_ID, CAst(B.processid as VArchar(10)) + '-' + CAst(SAMPLE_ID as Varchar(20)) SAMPLE_NO1  From Socks_Sample_Consumption_Ord_Bom_Fn() A Left Join Process B On B.processid in (" + Dr["ProcessID"].ToString() + ")  Where Order_ID = " + TxtOcnNo.Tag + " and Ord_ItemID = " + TxtItem.Tag + " Order by Sample_No ", ref TDtP);
                                    for(int p=0; p<=TDtP.Rows.Count -1; p++)
                                    {                                        
                                        DataRow TDr1 = TDtP.Rows[p]; 
                                        //Dt2.Rows[0]["SNO"]  = TDr1["SNO"].ToString();      
                                        //Dt2.Rows[0]["PROCESS"] = TDr1["PROCESS"].ToString();
                                        Dt2.ImportRow(TDr1);                                        
                                    }   
                                    GridProc.DataSource = Dt2;
                                    GridProc.Refresh();                                   
                                    GridProc.CurrentCell = GridProc["PROCESS", GridProc.Rows.Count -2];
                                    GridProc.Focus();
                                    GridProc.BeginEdit(true);                                                                                
                                    //Txt_Proc.Text = GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value.ToString();
                                     */
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
                             if(GridTrim["ACCESS_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() == "SAMPLE")
                             {
                                
                                    String Tid = GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                    for(int q=0; q<= GridTrim.Rows.Count-2; q++)
                                    {
                                         if(GridTrim["SNO1", q].Value.ToString() ==  Tid)
                                         {                                     
                                             Dt1.Rows.RemoveAt(q);                                      
                                             //Dt1.AcceptChanges();                             
                                             //GridTrim.Rows.RemoveAt (q);
                                             GridTrim.RefreshEdit();  
                                             GridTrim.Refresh();
                                             q=-1;
                                         }
                                    }                                        
                                                GridTrim.CurrentCell = GridTrim["ACCESS_TYPE", GridTrim.Rows.Count -1];
                                                GridTrim.Focus();
                                                GridTrim.BeginEdit(true);                                           
                                                return;
                                }
                             
                             else
                             {
                                 Dt1.Rows.RemoveAt(GridTrim.CurrentCell.RowIndex);
                             }
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
                if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["ACCESS_TYPE"].Index)
                {
                   // Dt1.AcceptChanges();
                    GridTrim.CurrentCell = GridTrim["ACCESS_TYPE", GridTrim.CurrentCell.RowIndex];
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
                if (GridProc["FLAG", GridProc.CurrentCell.RowIndex].Value.ToString() == "F" || GridProc["FLAG", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                  {
                    GridProc.Focus();
                    if(GridProc["PROC_ID", GridProc.CurrentCell.RowIndex].Value.ToString() != "152" && GridProc["PROC_ID", GridProc.CurrentCell.RowIndex].Value.ToString() != "163")
                    {
                        if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            String Pid = GridProc["PROC_ID", GridProc.CurrentCell.RowIndex].Value.ToString();
                            for(int q=0; q<= GridProc.Rows.Count-2; q++)
                            {
                                 if(GridProc["PROC_ID", q].Value.ToString() ==  Pid)
                                 {
                                     Dt2.Rows.RemoveAt(q); 
                                     Dt2.AcceptChanges(); 
                                     GridProc.RefreshEdit ();
                                     GridProc.Refresh ();  
                                     q=-1;                                                            
                                 }                            
                            }                                     
                                        GridProc.CurrentCell = GridProc["PROCESS", GridProc.Rows.Count -1];
                                        GridProc.Focus();
                                        GridProc.BeginEdit(true);   
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


        void Txt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["LOSS_PER"].Index)
                {
                    if (GridYarn["LOSS_WEIGHT", GridYarn.CurrentCell.RowIndex].Value == null || GridYarn["LOSS_WEIGHT", GridYarn.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridYarn["LOSS_WEIGHT", GridYarn.CurrentCell.RowIndex].Value) == 0)
                    {
                        GridYarn["LOSS_WEIGHT", GridYarn.CurrentCell.RowIndex].Value = "0.000";
                    }
                    else if (GridYarn["BOM_QTY", GridYarn.CurrentCell.RowIndex].Value == null || GridYarn["BOM_QTY", GridYarn.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridYarn["BOM_QTY", GridYarn.CurrentCell.RowIndex].Value) == 0)
                    {
                        GridYarn["BOM_QTY", GridYarn.CurrentCell.RowIndex].Value = "0";
                    }
                    else
                    {
                        if (Txt.Text.ToString() == String.Empty )
                        {
                            GridYarn["LOSS_PER", GridYarn.CurrentCell.RowIndex].Value = 0;
                        }
                        else
                        {                            
                              //   GridYarn["LOSS_WEIGHT", GridYarn.CurrentCell.RowIndex].Value = ((Convert.ToInt32(Txt.Text.ToString()) * Convert.ToDouble(GridYarn["BOM_CONS", GridYarn.CurrentCell.RowIndex].Value)));
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
       
        void Total_Count()
        {               
            try
            {                               
                TxtTotCons.Text = MyBase.Count(ref GridYarn, "BOM_CONS", "ITEM");
                TxtTotWeight.Text = MyBase.Sum(ref GridYarn, "BOM_CONS", "ITEM");
                TxtTotTrimCons.Text = MyBase.Sum(ref GridTrim, "CONS", "ITEM");
                TxtTotTrimQty.Text = MyBase.Sum(ref GridTrim, "REQ_QTY", "ITEM"); 
                TxtTotYrnPurAmt.Text = MyBase.Sum(ref GridYarn, "PUR_AMOUNT", "BOM_CONS");
                TxtTotYrnDyeAmt.Text = MyBase.Sum(ref GridYarn, "DYE_AMOUNT", "BOM_CONS");
                TxtTotTrmPurAmt.Text = MyBase.Sum(ref GridTrim, "PUR_AMOUNT", "ITEM"); 
                TxtTotProAmt.Text = MyBase.Sum(ref GridProc, "PRO_AMOUNT", "PROCESS");
                TxtTotProQty.Text = MyBase.Sum(ref GridProc, "REQ_QTY", "PROCESS");
                TxtTotComAmt.Text = MyBase.Sum(ref GridComm, "AMOUNT", "COMM_NAME");
                TxtTotComQty.Text = MyBase.Sum(ref GridComm, "QTY", "COMM_NAME"); 
                TxtSpclWgt.Text = MyBase.Sum(ref GridSpcl, "PUR_AMOUNT", "ACCESS_TYPE");
                TxtSpclCons.Text = MyBase.Sum(ref GridSpcl, "BOM_CONS", "ACCESS_TYPE"); 
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
                if(GridYarn.Rows.Count >1)
                {
                TxtYarnCost.Text = MyBase.Sum(ref GridYarn, "PUR_AMOUNT", "BOM_CONS");                
                TxtTrimCost.Text = MyBase.Sum(ref GridTrim, "PUR_AMOUNT", "ITEM");
                TxtProcCost.Text = MyBase.Sum(ref GridProc, "PRO_AMOUNT", "PROCESS");                
                TxtCommCost.Text = MyBase.Sum(ref GridComm, "AMOUNT", "COMM_NAME");    
                TxtTotYrnDyeAmt.Text = MyBase.Sum(ref GridYarn, "DYE_AMOUNT", "BOM_CONS");
                TxtSpclReqCost.Text = MyBase.Sum(ref GridSpcl, "PUR_AMOUNT", "ACCESS_TYPE");
                ProCost =  Convert.ToDouble(TxtProcCost.Text.ToString()) + Convert.ToDouble(TxtTotYrnDyeAmt.Text.ToString());
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
                if (TxtSpclReqCost.Text.ToString().Trim() == String.Empty)
                {
                    TxtSpclReqCost.Text = "0";
                }
                TxtTotalCost.Text = (((Convert.ToDouble(TxtYarnCost.Text.ToString()) + Convert.ToDouble(TxtTrimCost.Text.ToString()) + Convert.ToDouble(TxtProcCost.Text.ToString()) + Convert.ToDouble(TxtCommCost.Text.ToString()) + Convert.ToDouble(TxtSpclReqCost.Text.ToString())) * (Convert.ToDouble(TxtProfit.Text.ToString()) / 100)) + ((Convert.ToDouble(TxtYarnCost.Text.ToString()) + Convert.ToDouble(TxtTrimCost.Text.ToString()) + Convert.ToDouble(TxtProcCost.Text.ToString()) + Convert.ToDouble(TxtCommCost.Text.ToString()) + Convert.ToDouble(TxtSpclReqCost.Text.ToString())))).ToString();
                TxtPackCost.Text = Math.Round((Convert.ToDouble(TxtTotalCost.Text.ToString()) / Convert.ToDouble(TxtOrdQty.Tag.ToString())),2).ToString();
                TxtIndRs.Text = TxtPackCost.Text.ToString();
                TxtExpRs.Text = Math.Round((Convert.ToDouble(TxtPackCost.Text.ToString()) / Convert.ToDouble(TxtExRate.Text.ToString())),4).ToString();                
                TxtSalePriceExp.Text = Math.Round(((Convert.ToDouble(TxtBomQty.Tag.ToString()) / Convert.ToDouble(TxtOrdQty.Tag.ToString())) / Convert.ToDouble(TxtExRate.Text.ToString())), 2).ToString();          
                TxtSalePriceInd.Text = Math.Round((Convert.ToDouble(TxtSalePriceExp.Text.ToString()) * Convert.ToDouble(TxtExRate.Text.ToString())),2).ToString();
                TxtValueInd.Text = (Convert.ToDouble(TxtSalePriceInd.Text.ToString()) - Convert.ToDouble(TxtIndRs.Text.ToString())).ToString();
                TxtValueExp.Text = (Convert.ToDouble(TxtSalePriceExp.Text.ToString()) - Convert.ToDouble(TxtExpRs.Text.ToString())).ToString();                
                TxtProfitInd.Text = Math.Round((Convert.ToDouble(TxtValueInd.Text.ToString()) / Convert.ToDouble(TxtSalePriceInd.Text.ToString())) * 100,2).ToString();
                TxtProfitExp.Text = Math.Round((Convert.ToDouble(TxtValueExp.Text.ToString()) / Convert.ToDouble(TxtSalePriceExp.Text.ToString())) * 100,2).ToString();                
                }
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
                if(GridYarn["FLAG", GridYarn.CurrentCell.RowIndex].Value.ToString() == "F" || GridYarn["FLAG", GridYarn.CurrentCell.RowIndex].Value.ToString() == "A")
                {
                    if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["DYE_MODE"].Index)
                    {
                        if(GridYarn["BOM_QTY", GridYarn.CurrentCell.RowIndex].Value.ToString() == String.Empty || GridYarn["BOM_CONS", GridYarn.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            MessageBox.Show("Invalid BOM QTY & BOM CONS");
                            GridYarn.CurrentCell = GridYarn["DYE_MODE", GridYarn.CurrentCell.RowIndex];
                            GridYarn.Focus();
                            GridYarn.BeginEdit(true);
                            e.Handled = true;
                            return;
                        }
                        else if (GridYarn["COLOR", GridYarn.CurrentCell.RowIndex].Value.ToString().ToUpper() != "GREIGE" || GridYarn["COLOR", GridYarn.CurrentCell.RowIndex].Value.ToString().ToUpper() != "R.WHITE")
                        {
                            MyBase.Valid_Yes_OR_No(Txt,e);
                        }
                        else
                        {
                            MyBase.Valid_Null(Txt, e);
                        }
                    }                   
                    if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["YARN_LOSS_PERC"].Index)
                    {                       
                            MyBase.Valid_Number(Txt,e);
                    }
                    else if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["LOSS_PER"].Index)
                    {
                        if(GridYarn["DYE_MODE", GridYarn.CurrentCell.RowIndex].Value.ToString() == "Y")
                        {
                            MyBase.Valid_Number(Txt,e);
                        }
                        else
                        {
                            MyBase.Valid_Null(Txt, e);
                        }
                    }
                    else if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["DYE_RATE"].Index)
                    {
                        if(GridYarn["DYE_MODE", GridYarn.CurrentCell.RowIndex].Value.ToString() == "Y")
                        {
                            MyBase.Valid_Decimal(Txt,e);
                        }
                        else
                        {
                            MyBase.Valid_Null(Txt, e);
                        }
                    }                   
                    else if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["PUR_RATE"].Index)
                    {                    
                            MyBase.Valid_Decimal(Txt,e);                    
                    }
                    else
                    {
                        MyBase.Valid_Null(Txt, e);
                    }
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
                if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["LOSS_PER"].Index && Txt.Text.ToString() != String.Empty)
                {
                       GridYarn["LOSS_PER", GridYarn.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt.Text.ToString());
                       if(GridYarn["LOSS_PER", GridYarn.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                       {
                           GridYarn["LOSS_PER", GridYarn.CurrentCell.RowIndex].Value =  0;
                       }                     
                       else
                       {
                           GridYarn["LOSS_WEIGHT", GridYarn.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridYarn["BOM_CONS", GridYarn.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridYarn["LOSS_PER", GridYarn.CurrentCell.RowIndex].Value)) / 100;
                       }                                                 
                }
                else if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["YARN_LOSS_PERC"].Index && Txt.Text.ToString() != String.Empty)
                {
                       GridYarn["YARN_LOSS_PERC", GridYarn.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt.Text.ToString());
                       if(GridYarn["YARN_LOSS_PERC", GridYarn.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                       {
                           GridYarn["YARN_LOSS_PERC", GridYarn.CurrentCell.RowIndex].Value =  0;
                       }                     
                       else
                       {                           
                        //   GridYarn["BOM_CONS", GridYarn.CurrentCell.RowIndex].Value =  Math.Round(Convert.ToDouble((Convert.ToDouble(GridYarn["BOM_CONS", GridYarn.CurrentCell.RowIndex].Value) * (Convert.ToDouble(GridYarn["YARN_LOSS_PERC", GridYarn.CurrentCell.RowIndex].Value) / (100 - Convert.ToDouble(GridYarn["YARN_LOSS_PERC", GridYarn.CurrentCell.RowIndex].Value)))) +  Convert.ToDouble(GridYarn["BOM_CONS", GridYarn.CurrentCell.RowIndex].Value)),3);
                       }                                                 
                }
                else if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["DYE_MODE"].Index && Txt.Text.ToString() != String.Empty)
                {
                       GridYarn["DYE_MODE", GridYarn.CurrentCell.RowIndex].Value = Txt.Text.ToString();
                       if(GridYarn["DYE_MODE", GridYarn.CurrentCell.RowIndex].Value.ToString() == "Y" && GridYarn["COLOR", GridYarn.CurrentCell.RowIndex].Value.ToString().ToUpper() != "R.WHITE" && GridYarn["ITEM", GridYarn.CurrentCell.RowIndex].Value.ToString().ToUpper() == "NYLON")
                       {
                           GridYarn["DYE_COLOR", GridYarn.CurrentCell.RowIndex].Value =  "R.White";
                           GridYarn["DYE_ITEMID", GridYarn.CurrentCell.RowIndex].Value =  3343;
                           DataTable TDtd = new DataTable();
                           MyBase.Load_Data("Select IsNull(Rate,0.00) Rate FRom Socks_Budget_App_Rate_Max_Fn() Where Type = 'Yarn' and Colorid = 3343 and Itemid = " + GridYarn["ITEMID", GridYarn.CurrentCell.RowIndex].Value + " and Sizeid = " + GridYarn["SIZEID", GridYarn.CurrentCell.RowIndex].Value + " ", ref TDtd);

                           GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex].Value = Convert.ToDouble(TDtd.Rows[0][0].ToString());
                           GridYarn["PUR_AMOUNT", GridYarn.CurrentCell.RowIndex].Value = Convert.ToDouble(TDtd.Rows[0][0].ToString()) * (Convert.ToDouble(GridYarn["BOM_CONS", GridYarn.CurrentCell.RowIndex].Value.ToString()) + Convert.ToDouble(GridYarn["LOSS_WEIGHT", GridYarn.CurrentCell.RowIndex].Value.ToString()));
                       }
                       else if(GridYarn["DYE_MODE", GridYarn.CurrentCell.RowIndex].Value.ToString() == "Y" && GridYarn["COLOR", GridYarn.CurrentCell.RowIndex].Value.ToString().ToUpper() != "GREIGE")
                       {
                           GridYarn["DYE_COLOR", GridYarn.CurrentCell.RowIndex].Value =  "GREIGE";
                           GridYarn["DYE_ITEMID", GridYarn.CurrentCell.RowIndex].Value =  867;
                           DataTable TDtd = new DataTable();
                           MyBase.Load_Data("Select IsNull(Rate,0.00) Rate FRom Socks_Budget_App_Rate_Max_Fn() Where Type = 'Yarn' and Colorid = 867 and Itemid = " + GridYarn["ITEMID", GridYarn.CurrentCell.RowIndex].Value + " and Sizeid = " + GridYarn["SIZEID", GridYarn.CurrentCell.RowIndex].Value + " ", ref TDtd);

                           GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex].Value = Convert.ToDouble(TDtd.Rows[0][0].ToString());
                           GridYarn["PUR_AMOUNT", GridYarn.CurrentCell.RowIndex].Value = Convert.ToDouble(TDtd.Rows[0][0].ToString()) * (Convert.ToDouble(GridYarn["BOM_CONS", GridYarn.CurrentCell.RowIndex].Value.ToString()) + Convert.ToDouble(GridYarn["LOSS_WEIGHT", GridYarn.CurrentCell.RowIndex].Value.ToString()));
                       }
                       else
                       {
                           GridYarn["DYE_COLOR", GridYarn.CurrentCell.RowIndex].Value =  GridYarn["COLOR", GridYarn.CurrentCell.RowIndex].Value;
                           GridYarn["DYE_ITEMID", GridYarn.CurrentCell.RowIndex].Value =  GridYarn["COLORID", GridYarn.CurrentCell.RowIndex].Value;
                           GridYarn["LOSS_PER", GridYarn.CurrentCell.RowIndex].Value =  0;
                           GridYarn["LOSS_WEIGHT", GridYarn.CurrentCell.RowIndex].Value =  0.000;
                           GridYarn["DYE_RATE", GridYarn.CurrentCell.RowIndex].Value =  0.00;
                           GridYarn["DYE_AMOUNT", GridYarn.CurrentCell.RowIndex].Value =  0.00;
                       }
                }
                else if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["DYE_RATE"].Index && Txt.Text.ToString() != String.Empty)
                {                    
                    if(GridYarn["DYE_MODE", GridYarn.CurrentCell.RowIndex].Value.ToString() == "Y")
                       {
                           GridYarn["DYE_RATE", GridYarn.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt.Text.ToString());
                           if(GridYarn["DYE_RATE", GridYarn.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                           {
                               GridYarn["DYE_RATE", GridYarn.CurrentCell.RowIndex].Value =  0.00;
                               GridYarn["DYE_AMOUNT", GridYarn.CurrentCell.RowIndex].Value =  0.00;                             
                           }
                           else if(Convert.ToDouble(GridYarn["DYE_RATE", GridYarn.CurrentCell.RowIndex].Value) >0)
                           {
                                GridYarn["DYE_AMOUNT", GridYarn.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridYarn["BOM_CONS", GridYarn.CurrentCell.RowIndex].Value) + Convert.ToDouble(GridYarn["LOSS_WEIGHT", GridYarn.CurrentCell.RowIndex].Value)) * Convert.ToDouble(GridYarn["DYE_RATE", GridYarn.CurrentCell.RowIndex].Value);
                           }                      
                           else
                           {
                                GridYarn["DYE_AMOUNT", GridYarn.CurrentCell.RowIndex].Value = 0.00;                              
                                GridYarn["DYE_RATE", GridYarn.CurrentCell.RowIndex].Value =  0.00;                                                                
                           }
                        }
                    else 
                    {
                                  GridYarn["DYE_AMOUNT", GridYarn.CurrentCell.RowIndex].Value = 0.00;
                    }
                }                
                else if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["PUR_RATE"].Index && Txt.Text.ToString() != String.Empty)
                {
                       GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt.Text.ToString());
                       if(GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                       {                            
                            GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex].Value =  0.00;
                            GridYarn["PUR_AMOUNT", GridYarn.CurrentCell.RowIndex].Value =  0.00;                            
                       }
                       else if(Convert.ToDouble(GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex].Value) >0)
                       {
                            GridYarn["PUR_AMOUNT", GridYarn.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridYarn["BOM_CONS", GridYarn.CurrentCell.RowIndex].Value) + Convert.ToDouble(GridYarn["LOSS_WEIGHT", GridYarn.CurrentCell.RowIndex].Value)) * Convert.ToDouble(GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex].Value);
                       }                      
                       else
                       {
                          //  MessageBox.Show("Invalid PUR_RATE");
                            GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex].Value =  0.00;
                            GridYarn["PUR_AMOUNT", GridYarn.CurrentCell.RowIndex].Value =  0.00;
                            GridYarn.CurrentCell = GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex];                        
                            GridYarn.Focus();
                            GridYarn.BeginEdit(true);                            
                            return;                                        
                       }                         
                }   
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridYarn_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Total_Count();
                    TxtTotYrnDyeAmt.Focus();
                    return;
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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


        private void FrmSocksPlanningEntry_Load(object sender, EventArgs e)
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
        
        private void FrmSocksPlanningEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {
                if(MyParent.UserCode == 37)
                {
                    e.Handled =true;
                    return;
                }
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

        private void FrmSocksPlanningEntry_KeyDown(object sender, KeyEventArgs e)
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
                    else if (this.ActiveControl.Name == "TxtOcnNo")
                    {
                        if(GridYarn.Rows.Count >0)
                        {
                            tabControl1.SelectTab(tabPage1);
                            GridYarn.CurrentCell = GridYarn["DYE_MODE", 0];
                            GridYarn.Focus();
                            GridYarn.BeginEdit(true);
                            e.Handled = true;
                            return;         
                        }
                    } 
                    else if (this.ActiveControl.Name == "TxtTotYrnDyeAmt")
                    {
                        if(GridTrim.Rows.Count >0)
                        {
                            tabControl1.SelectTab(tabPage2);
                            GridTrim.CurrentCell = GridTrim["ACCESS_TYPE", 0];
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
                            GridComm.CurrentCell = GridComm["COMM_NAME", 0];
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
                        if (this.ActiveControl.Name == "TxtOcnNo")
                        {
                            //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select  A.Order_No,  A.Order_Date, A.Item, A.Party, A.Employee, Bom_Qty_Plan BOM_Qty, Buyer_Qty_Plan Buyer_Qty,  A.OrdeR_ID, A.ItemID, A.Party_Code, dbo.Socks_Yarn_Loss_Perc_Buyer('" + String.Format("{0:dd-MMM-yyyy}  {0:T}", DtpDate.Value) + "', Party_Code) Yarn_Loss_Perc, A.Currency, A.Ex_Rate, A.Amount, A.Buy_Qty  From Socks_Bom_Item_Fn() A Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A1 On A1.ORder_No = A.ORder_No and A1.Division_ID=3 Left Join Socks_Planning_Master B On A.ORder_ID = B.Order_ID and A.ItemID = B.Item_ID   Order by A.Order_No desc, A.Item ", String.Empty, 120, 100, 140, 100, 120, 100, 100);
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select  A.Order_No,  A.Order_Date, A.Item, A.Party, A.Employee, Bom_Qty_Plan BOM_Qty, Buyer_Qty_Plan Buyer_Qty,  A.OrdeR_ID, A.ItemID, A.Party_Code, dbo.Socks_Yarn_Loss_Perc_Buyer('" + String.Format("{0:dd-MMM-yyyy}  {0:T}", DtpDate.Value) + "', Party_Code) Yarn_Loss_Perc, A.Currency, A.Ex_Rate, A.Amount, A.Buy_Qty  From Socks_Bom_Item_Fn() A Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master A1 On A1.ORder_No = A.ORder_No and A1.Division_ID=3 Left Join Socks_Planning_Master B On A.ORder_ID = B.Order_ID and A.ItemID = B.Item_ID  Where B.RowID IS Null  Order by A.Order_No desc, A.Item ", String.Empty, 120, 100, 140, 100, 120, 100, 100);
                                if (Dr != null)
                                {
                                    // 30 days test
                                    if (MyBase.Validate_Date_For_Entry(Convert.ToDateTime(Dr["Order_Date"].ToString()) , 30, Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", DtpDate.Value))) == false && Convert.ToInt16(MyParent.UserCode) != 1)                                
                                    {
                                        MessageBox.Show("Date Locked, Only 30 Days are Allowed From Order Creation");
                                        TxtOcnNo.Focus();
                                        return;
                                    }
                                    TxtOcnNo.Text = Dr["Order_No"].ToString();
                                    TxtOcnNo.Tag = Dr["Order_ID"].ToString();
                                    DtpODate.Value = Convert.ToDateTime(Dr["Order_Date"].ToString());
                                    TxtEmpl.Text = Dr["Employee"].ToString();
                                    TxtBuyer.Text =  Dr["Party"].ToString();
                                    TxtBuyer.Tag =  Dr["Party_Code"].ToString();
                                    TxtOrdQty.Text =  Dr["Buyer_Qty"].ToString();                                    
                                    TxtOrdQty.Tag =  Dr["Buy_Qty"].ToString();  
                                    TxtBomQty.Text =  Dr["Bom_Qty"].ToString();
                                    TxtBomQty.Tag = Dr["Amount"].ToString();
                                    TxtItem.Text =   Dr["Item"].ToString();
                                    TxtItem.Tag =   Dr["ItemID"].ToString();
                                    TxtCurrency.Text = Dr["Currency"].ToString();
                                    TxtExRate.Text = Dr["Ex_Rate"].ToString();
                                    Grid_Data();
                                    Total_Count();
                                    tabControl1.SelectTab(tabPage1);
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
               if (tabControl1.SelectedTab == tabControl1.TabPages[1])
                {
                   GridTrim.AllowUserToAddRows = true;
                   GridTrim.CurrentCell = GridTrim["ACCESS_TYPE", 0];
                   GridTrim.Focus();
                   GridTrim.BeginEdit(true);                   
                   return;
                }
               else if (tabControl1.SelectedTab == tabControl1.TabPages[2])
               {                  
                   GridProc.AllowUserToAddRows = true;
                   GridProc.CurrentCell = GridProc["PROCESS", 0];
                   GridProc.Focus();
                   GridProc.BeginEdit(true);                   
                   return;
               }
               else if (tabControl1.SelectedTab == tabControl1.TabPages[3])
               {
                   GridComm.AllowUserToAddRows = true;
                   GridComm.CurrentCell = GridComm["COMM_NAME", 0];
                   GridComm.Focus();
                   GridComm.BeginEdit(true);                   
                   return;
               }  
               else if (tabControl1.SelectedTab == tabControl1.TabPages[0])
               {  
                   if(GridYarn.Rows.Count > 1)
                   {
                    GridYarn.CurrentCell = GridYarn["DYE_MODE", 0];
                    GridYarn.Focus();
                    GridYarn.BeginEdit(true);                   
                    return;
                   }
               }
               else if (tabControl1.SelectedTab == tabControl1.TabPages[5])
               {  
                   if(GridSpcl.Rows.Count > 1)
                   {
                    GridSpcl.CurrentCell = GridSpcl["ACCESS_TYPE", 0];
                    GridSpcl.Focus();
                    GridSpcl.BeginEdit(true);                   
                    return;
                   }
               }
               else if (tabControl1.SelectedTab == tabControl1.TabPages[4])
               {
                   
                   Total_Cost_Calc();
                 //  TxtProfit.Focus();
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

        private void GridYarn_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["LOSS_PER"].Index)
                    {
                        if (GridYarn["LOSS_PER", GridYarn.CurrentCell.RowIndex].Value != DBNull.Value)
                        {
                            if ((GridYarn["DYE_MODE", GridYarn.CurrentCell.RowIndex].Value.ToString() == "Y" &&  (Convert.ToInt32(GridYarn["LOSS_PER", GridYarn.CurrentCell.RowIndex].Value) < 1 || Convert.ToInt32(GridYarn["LOSS_PER", GridYarn.CurrentCell.RowIndex].Value) > 10))) 
                            {
                                MessageBox.Show("Invalid LOSS PERC..!", "Gainup");
                                GridYarn["LOSS_PER", GridYarn.CurrentCell.RowIndex].Value = 0;                                
                                GridYarn["LOSS_WEIGHT", GridYarn.CurrentCell.RowIndex].Value = 0.000; 
                                GridYarn.CurrentCell = GridYarn["LOSS_PER", GridYarn.CurrentCell.RowIndex];
                                GridYarn.Focus();
                                GridYarn.BeginEdit(true);
                                e.Handled = true;
                                return;
                            }
                        }
                    }
                    else if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["YARN_LOSS_PERC"].Index)
                    {
                        if (GridYarn["YARN_LOSS_PERC", GridYarn.CurrentCell.RowIndex].Value != DBNull.Value)
                        {
                            if (((Convert.ToInt32(GridYarn["YARN_LOSS_PERC", GridYarn.CurrentCell.RowIndex].Value) < 1 || Convert.ToInt32(GridYarn["YARN_LOSS_PERC", GridYarn.CurrentCell.RowIndex].Value) > 10))) 
                            {
                                MessageBox.Show("Invalid KNITTING ALLOW %..!", "Gainup");
                                GridYarn["YARN_LOSS_PERC", GridYarn.CurrentCell.RowIndex].Value = 0;                                                                
                                GridYarn.CurrentCell = GridYarn["YARN_LOSS_PERC", GridYarn.CurrentCell.RowIndex];
                                GridYarn.Focus();
                                GridYarn.BeginEdit(true);
                                e.Handled = true;
                                return;
                            }
                        }
                    }
                    else if (((GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["LOSS_PER"].Index && GridYarn.Rows.Count == Dt.Rows.Count && GridYarn["DYE_MODE", GridYarn.CurrentCell.RowIndex].Value.ToString() == "N")) || (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["LOSS_PER"].Index && GridYarn.Rows.Count == Dt.Rows.Count))
                    {
                        TxtTotYrnDyeAmt.Focus();
                        return;
                    }
                    else if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["PUR_RATE"].Index)
                    {
                       if(GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                       {
                            
                            GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex].Value =  0.00;
                            GridYarn["PUR_AMOUNT", GridYarn.CurrentCell.RowIndex].Value =  0.00;                            
                       }
                       else if(Convert.ToDouble(GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex].Value) >0)
                       {
                            GridYarn["PUR_AMOUNT", GridYarn.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridYarn["BOM_CONS", GridYarn.CurrentCell.RowIndex].Value) + Convert.ToDouble(GridYarn["LOSS_WEIGHT", GridYarn.CurrentCell.RowIndex].Value)) * Convert.ToDouble(GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex].Value);
                       }                      
                       else
                       {
                            MessageBox.Show("Invalid PUR_RATE");
                            GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex].Value =  0.00;
                            GridYarn["PUR_AMOUNT", GridYarn.CurrentCell.RowIndex].Value =  0.00;
                            GridYarn.CurrentCell = GridYarn["PUR_RATE", GridYarn.CurrentCell.RowIndex];                        
                            GridYarn.Focus();
                            GridYarn.BeginEdit(true);
                            e.Handled = true; 
                            return;                                        
                       }                         
                    }                        
                        else if (GridYarn.CurrentCell.ColumnIndex == GridYarn.Columns["DYE_RATE"].Index)
                        {
                           if(GridYarn["DYE_MODE", GridYarn.CurrentCell.RowIndex].Value.ToString() == "Y")
                           {
                               if(GridYarn["DYE_RATE", GridYarn.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                               {
                                   GridYarn["DYE_RATE", GridYarn.CurrentCell.RowIndex].Value =  0.00;
                                   GridYarn["DYE_AMOUNT", GridYarn.CurrentCell.RowIndex].Value =  0.00;                             
                               }
                               else if(Convert.ToDouble(GridYarn["DYE_RATE", GridYarn.CurrentCell.RowIndex].Value) >0)
                               {
                                    GridYarn["DYE_AMOUNT", GridYarn.CurrentCell.RowIndex].Value = (Convert.ToDouble(GridYarn["BOM_CONS", GridYarn.CurrentCell.RowIndex].Value)+ Convert.ToDouble(GridYarn["LOSS_WEIGHT", GridYarn.CurrentCell.RowIndex].Value)) * Convert.ToDouble(GridYarn["DYE_RATE", GridYarn.CurrentCell.RowIndex].Value);
                               }                      
                               else
                               {
                                    
                                    MessageBox.Show("Invalid DYE_RATE");  
                                    GridYarn["DYE_AMOUNT", GridYarn.CurrentCell.RowIndex].Value = 0.00;                              
                                    GridYarn["DYE_RATE", GridYarn.CurrentCell.RowIndex].Value =  0.00; 
                                    GridYarn.CurrentCell = GridYarn["DYE_RATE", GridYarn.CurrentCell.RowIndex];                        
                                    GridYarn.Focus();
                                    GridYarn.BeginEdit(true);
                                    e.Handled = true; 
                                    return;                                                                              
                               }
                            }
                        else 
                        {
                                      GridYarn["DYE_AMOUNT", GridYarn.CurrentCell.RowIndex].Value = 0.00;
                        }
                    }   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                 TxtOcnNo.Focus();
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
                    if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SAMPLE_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        if (GridTrim.Rows.Count > 2)
                        {
                            for (int k = 0; k < GridTrim.Rows.Count - 2; k++)
                            {
                                if ((k != GridTrim.CurrentCell.RowIndex) && (GridTrim["ITEM", k].Value.ToString()) == GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() && (GridTrim["COLOR", k].Value.ToString()) == (GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value.ToString()) && (GridTrim["SIZE", k].Value.ToString()) == GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value.ToString() && (GridTrim["SAMPLE_ID", k].Value.ToString()) == GridTrim["SAMPLE_ID", GridTrim.CurrentCell.RowIndex].Value.ToString())
                                {
                                    MessageBox.Show("Already  ITEM , COLOR & SIZE are Available", "Gainup");
                                    GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value = "";
                                    GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value = "";
                                    GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value = "";
                                    GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value = 0;
                                    GridTrim["REQ_QTY", GridTrim.CurrentCell.RowIndex].Value = 0;
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

                    if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["PUR_RATE"].Index)
                    {
                        if (GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {

                            GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                            GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                        }
                        else if (Convert.ToDouble(GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value) > 0)
                        {
                            GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = Convert.ToDouble(GridTrim["REQ_QTY", GridTrim.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value);
                        }
                        else
                        {
                            MessageBox.Show("Invalid PUR_RATE");
                            GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                            GridTrim["PUR_AMOUNT", GridTrim.CurrentCell.RowIndex].Value = 0.00;
                            GridTrim.CurrentCell = GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex];
                            GridTrim.Focus();
                            GridTrim.BeginEdit(true);
                            e.Handled = true;
                            return;
                        }
                    }
                    else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["CONS"].Index)
                    {
                        if (GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value) <= 0)
                        {
                            MessageBox.Show("Invalid CONS");
                            GridTrim.CurrentCell = GridTrim["CONS", GridTrim.CurrentCell.RowIndex];
                            GridTrim.Focus();
                            GridTrim.BeginEdit(true);
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

        private void GridProc_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridProc["SAMPLE_NO", GridProc.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        if (GridProc.Rows.Count > 2)
                        {
                            for (int k = 0; k < GridProc.Rows.Count - 2; k++)
                            {
                                if ((k != GridProc.CurrentCell.RowIndex) && (GridProc["PROCESS", k].Value.ToString()) == GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value.ToString() && (GridProc["SAMPLE_NO", k].Value.ToString()) == (GridProc["SAMPLE_NO", GridProc.CurrentCell.RowIndex].Value.ToString()) && (GridProc["SIZE", k].Value.ToString()) == (GridProc["SIZE", GridProc.CurrentCell.RowIndex].Value.ToString()))
                                {
                                    MessageBox.Show("Already Process, Sample No & Size are Available", "Gainup");
                                    GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value = "";
                                    GridProc["SAMPLE_NO", GridProc.CurrentCell.RowIndex].Value = "";
                                    GridProc["SIZE", GridProc.CurrentCell.RowIndex].Value = "";
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

                    if (GridProc.CurrentCell.ColumnIndex == GridProc.Columns["PRO_RATE"].Index)
                    {
                        if (GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {

                            GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value = 0.00;
                            GridProc["PRO_AMOUNT", GridProc.CurrentCell.RowIndex].Value = 0.00;
                        }
                        else if (Convert.ToDouble(GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value) > 0)
                        {                            
                                //for(int i=0; i<= GridProc.Rows.Count-2; i++)
                                //{
                                //    if(GridProc["PROCESS", GridProc.CurrentCell.RowIndex].Value.ToString() == GridProc["PROCESS", i].Value.ToString() && GridProc["SIZE", GridProc.CurrentCell.RowIndex].Value.ToString() == GridProc["SIZE", i].Value.ToString())
                                //    {
                                //        GridProc["PRO_RATE", i].Value = GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value.ToString();
                                //        GridProc["PRO_AMOUNT", i].Value = Convert.ToDouble(GridProc["REQ_QTY", GridProc.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value);
                                //    }
                                //}                         
                           
                        }
                        else
                        {
                            MessageBox.Show("Invalid PRO_RATE");
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
                if (GridComm["COMM_NAME", GridComm.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridComm["CALC_MODE", GridComm.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridComm.CurrentCell.ColumnIndex == GridComm.Columns["RATE"].Index)
                    {
                        if (GridComm["RATE", GridComm.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {

                            GridComm["RATE", GridComm.CurrentCell.RowIndex].Value = 0.00;
                            GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value = 0.00;
                        }
                        else if (Convert.ToDouble(GridComm["RATE", GridComm.CurrentCell.RowIndex].Value) > 0)
                        {
                            if (GridComm["CALC_MODE", GridComm.CurrentCell.RowIndex].Value.ToString() == "PER QTY")
                            {
                                GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble(GridComm["QTY", GridComm.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridComm["RATE", GridComm.CurrentCell.RowIndex].Value);
                            }
                            else if (GridComm["CALC_MODE", GridComm.CurrentCell.RowIndex].Value.ToString() == "MANUAL")
                            {
                                GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value =  GridComm["RATE", GridComm.CurrentCell.RowIndex].Value;
                            }
                            else if (GridComm["CALC_MODE", GridComm.CurrentCell.RowIndex].Value.ToString() == "PERCENTAGE")
                            {
                                if((Convert.ToDouble(GridComm["RATE", GridComm.CurrentCell.RowIndex].Value) > 10))
                                {
                                    MessageBox.Show("Invalid Percentage");
                                    GridComm["RATE", GridComm.CurrentCell.RowIndex].Value = 0.00;
                                    GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value = 0.00;
                                    GridComm.CurrentCell = GridComm["RATE", GridComm.CurrentCell.RowIndex];
                                    GridComm.Focus();
                                    GridComm.BeginEdit(true);
                                    e.Handled = true;
                                    return;
                                }
                                else
                                {
                                    GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value = Convert.ToDouble( Convert.ToDouble(GridComm["QTY", GridComm.CurrentCell.RowIndex].Value) * (Convert.ToDouble(GridComm["RATE", GridComm.CurrentCell.RowIndex].Value) / 100));
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid RATE");
                            GridComm["RATE", GridComm.CurrentCell.RowIndex].Value = 0.00;
                            GridComm["AMOUNT", GridComm.CurrentCell.RowIndex].Value = 0.00;
                            GridComm.CurrentCell = GridComm["RATE", GridComm.CurrentCell.RowIndex];
                            GridComm.Focus();
                            GridComm.BeginEdit(true);
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

        private void GridProc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ButFillAll_Click(object sender, EventArgs e)
        {
            try
            {
                if(GridTrim.Rows.Count >1)
                {
                    for(int f=GridTrim.CurrentCell.RowIndex+1; f<= GridTrim.Rows.Count-2; f++)
                    {
                        if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["SIZE"].Index)
                        {
                            if (GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                //if(GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() == GridTrim["SNO1", f].Value.ToString())
                                //{
                                    GridTrim["SIZE", f].Value = GridTrim["Size", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                    GridTrim["SIZE_ID", f].Value = GridTrim["Size_ID", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                    GridTrim["Sample_No1", f].Value = GridTrim["Sample_No1", GridTrim.CurrentCell.RowIndex].Value.ToString() + "-" + GridTrim["Size_ID", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                //}
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["COLOR"].Index)
                        {
                            if (GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                //if(GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() == GridTrim["SNO1", f].Value.ToString())
                                //{
                                    GridTrim["COLOR", f].Value = GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                    GridTrim["COLOR_ID", f].Value = GridTrim["COLOR_ID", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                    GridTrim["Sample_No1", f].Value = GridTrim["Sample_No1", GridTrim.CurrentCell.RowIndex].Value.ToString() + "-" + GridTrim["COLOR_ID", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                //}
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["CONS"].Index)
                        {
                            if (GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                //if(GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() == GridTrim["SNO1", f].Value.ToString())
                                //{
                                    GridTrim["CONS", f].Value = GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value.ToString();  
                                    if (GridTrim["CONS", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value = 0;
                                    }
                                    else if (GridTrim["PLAN_TYPE", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["PLAN_TYPE", f].Value = "M";
                                    }
                                    if (GridTrim["PLAN_TYPE", f].Value.ToString() == "/" && Convert.ToDouble(GridTrim["CONS", f].Value) > 0)
                                    {
                                        GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["BOM_QTY", f].Value) / Convert.ToDouble(GridTrim["CONS", f].Value);
                                    }
                                    else if (GridTrim["PLAN_TYPE", f].Value.ToString() == "*")
                                    {
                                        GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["CONS", f].Value) * Convert.ToDouble(GridTrim["BOM_QTY", f].Value);
                                    }
                                    else if (GridTrim["PLAN_TYPE", f].Value.ToString() == "M")
                                    {
                                        GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["CONS", f].Value);
                                    }
                                    else
                                    {
                                        GridTrim["REQ_QTY", f].Value = 0;
                                    }
                                //}
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["PLAN_TYPE"].Index)
                        {
                            if (GridTrim["PLAN_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                //if(GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() == GridTrim["SNO1", f].Value.ToString())
                                //{
                                    GridTrim["PLAN_TYPE", f].Value = GridTrim["PLAN_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString();  
                                    if (GridTrim["CONS", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value = 0;
                                    }
                                    else if (GridTrim["PLAN_TYPE", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["PLAN_TYPE", f].Value = "M";
                                    }
                                    if (GridTrim["PLAN_TYPE", f].Value.ToString() == "/" && Convert.ToDouble(GridTrim["CONS", f].Value) > 0)
                                    {
                                        GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["BOM_QTY", f].Value) / Convert.ToDouble(GridTrim["CONS", f].Value);
                                    }
                                    else if (GridTrim["PLAN_TYPE", f].Value.ToString() == "*")
                                    {
                                        GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["CONS", f].Value) * Convert.ToDouble(GridTrim["BOM_QTY", f].Value);
                                    }
                                    else if (GridTrim["PLAN_TYPE", f].Value.ToString() == "M")
                                    {
                                        GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["CONS", f].Value);
                                    }
                                    else
                                    {
                                        GridTrim["REQ_QTY", f].Value = 0;
                                    }
                                //}
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["PUR_RATE"].Index)
                        {
                            if (GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                //if(GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() == GridTrim["SNO1", f].Value.ToString())
                                //{
                                    GridTrim["PUR_RATE", f].Value = GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value.ToString();  
                                    if (GridTrim["CONS", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value = 0;
                                    }
                                    else if (GridTrim["PLAN_TYPE", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["PLAN_TYPE", f].Value = "M";
                                    }
                                    else if (GridTrim["PUR_RATE", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["PUR_RATE", f].Value = 0.00;
                                    }
                                    else if (GridTrim["REQ_QTY", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["REQ_QTY", f].Value = 0.00;
                                    }                                
                                    GridTrim["PUR_AMOUNT", f].Value = Convert.ToDouble(GridTrim["PUR_RATE", f].Value) * Convert.ToDouble(GridTrim["REQ_QTY", f].Value);                                
                                //}
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

        private void ButPFill_Click(object sender, EventArgs e)
        {
            try
            {
                for(int f=GridProc.CurrentCell.RowIndex+0; f<= GridProc.Rows.Count-2; f++)
                {
                    if (GridProc.CurrentCell.ColumnIndex == GridProc.Columns["PRO_RATE"].Index)
                    {
                        if (GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridProc["PROC_ID", GridProc.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            if(GridProc["PROC_ID", GridProc.CurrentCell.RowIndex].Value.ToString() == GridProc["PROC_ID", f].Value.ToString())
                            {
                                GridProc["PRO_RATE", f].Value = GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value.ToString();
                                if (GridProc["REQ_QTY", f].Value.ToString() == String.Empty)
                                {
                                    GridProc["REQ_QTY", f].Value = 0.00;
                                }                                
                                GridProc["PRO_AMOUNT", f].Value = Convert.ToDouble(GridProc["PRO_RATE", f].Value) * Convert.ToDouble(GridProc["REQ_QTY", f].Value);                                
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

        private void TxtOrdQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void ButFill_Click(object sender, EventArgs e)
        {
            try
            {
                if(GridTrim.Rows.Count >1)
                {
                    for(int f=GridTrim.CurrentCell.RowIndex+1; f<= GridTrim.Rows.Count-2; f++)
                    {
                        if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["ITEM"].Index)
                        {
                            if (GridTrim["ITEM", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                if(GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() == GridTrim["SNO1", f].Value.ToString())
                                {
                                    GridTrim["ITEM", f].Value = GridTrim["Item", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                    GridTrim["ITEM_ID", f].Value = GridTrim["ITEM_ID", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                    GridTrim["Sample_No1", f].Value = GridTrim["Sample_No1", GridTrim.CurrentCell.RowIndex].Value.ToString() + "-" + GridTrim["ITEM_ID", GridTrim.CurrentCell.RowIndex].Value.ToString() + "-" + GridTrim["Size_ID", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                }
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["SIZE"].Index)
                        {
                            if (GridTrim["SIZE", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                if(GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() == GridTrim["SNO1", f].Value.ToString())
                                {
                                    GridTrim["SIZE", f].Value = GridTrim["Size", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                    GridTrim["SIZE_ID", f].Value = GridTrim["Size_ID", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                    GridTrim["Sample_No1", f].Value = GridTrim["Sample_No1", GridTrim.CurrentCell.RowIndex].Value.ToString() + "-" + GridTrim["Size_ID", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                }
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["COLOR"].Index)
                        {
                            if (GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                if(GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() == GridTrim["SNO1", f].Value.ToString())
                                {
                                    GridTrim["COLOR", f].Value = GridTrim["COLOR", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                    GridTrim["COLOR_ID", f].Value = GridTrim["COLOR_ID", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                    GridTrim["Sample_No1", f].Value = GridTrim["Sample_No1", GridTrim.CurrentCell.RowIndex].Value.ToString() + "-" + GridTrim["COLOR_ID", GridTrim.CurrentCell.RowIndex].Value.ToString();
                                }
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["CONS"].Index)
                        {
                            if (GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                if(GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() == GridTrim["SNO1", f].Value.ToString())
                                {
                                    GridTrim["CONS", f].Value = GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value.ToString();  
                                    if (GridTrim["CONS", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value = 0;
                                    }
                                    else if (GridTrim["PLAN_TYPE", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["PLAN_TYPE", f].Value = "M";
                                    }
                                    if (GridTrim["PLAN_TYPE", f].Value.ToString() == "/" && Convert.ToDouble(GridTrim["CONS", f].Value) > 0)
                                    {
                                        GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["BOM_QTY", f].Value) / Convert.ToDouble(GridTrim["CONS", f].Value);
                                    }
                                    else if (GridTrim["PLAN_TYPE", f].Value.ToString() == "*")
                                    {
                                        GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["CONS", f].Value) * Convert.ToDouble(GridTrim["BOM_QTY", f].Value);
                                    }
                                    else if (GridTrim["PLAN_TYPE", f].Value.ToString() == "M")
                                    {
                                        GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["CONS", f].Value);
                                    }
                                    else
                                    {
                                        GridTrim["REQ_QTY", f].Value = 0;
                                    }
                                }
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["PLAN_TYPE"].Index)
                        {
                            if (GridTrim["PLAN_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                if(GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() == GridTrim["SNO1", f].Value.ToString())
                                {
                                    GridTrim["PLAN_TYPE", f].Value = GridTrim["PLAN_TYPE", GridTrim.CurrentCell.RowIndex].Value.ToString();  
                                    if (GridTrim["CONS", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value = 0;
                                    }
                                    else if (GridTrim["PLAN_TYPE", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["PLAN_TYPE", f].Value = "M";
                                    }
                                    if (GridTrim["PLAN_TYPE", f].Value.ToString() == "/" && Convert.ToDouble(GridTrim["CONS", f].Value) > 0)
                                    {
                                        GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["BOM_QTY", f].Value) / Convert.ToDouble(GridTrim["CONS", f].Value);
                                    }
                                    else if (GridTrim["PLAN_TYPE", f].Value.ToString() == "*")
                                    {
                                        GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["CONS", f].Value) * Convert.ToDouble(GridTrim["BOM_QTY", f].Value);
                                    }
                                    else if (GridTrim["PLAN_TYPE", f].Value.ToString() == "M")
                                    {
                                        GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["CONS", f].Value);
                                    }
                                    else
                                    {
                                        GridTrim["REQ_QTY", f].Value = 0;
                                    }
                                }
                            }
                        }
                        else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["PUR_RATE"].Index)
                        {
                            if (GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                if(GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() == GridTrim["SNO1", f].Value.ToString())
                                {
                                    GridTrim["PUR_RATE", f].Value = GridTrim["PUR_RATE", GridTrim.CurrentCell.RowIndex].Value.ToString();  
                                    if (GridTrim["CONS", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value = 0;
                                    }
                                    else if (GridTrim["PLAN_TYPE", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["PLAN_TYPE", f].Value = "M";
                                    }
                                    else if (GridTrim["PUR_RATE", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["PUR_RATE", f].Value = 0.00;
                                    }
                                    else if (GridTrim["REQ_QTY", f].Value.ToString() == String.Empty)
                                    {
                                        GridTrim["REQ_QTY", f].Value = 0.00;
                                    }                                
                                    GridTrim["PUR_AMOUNT", f].Value = Convert.ToDouble(GridTrim["PUR_RATE", f].Value) * Convert.ToDouble(GridTrim["REQ_QTY", f].Value);                                
                                }
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

        private void ButPAll_Click(object sender, EventArgs e)
        {
            try
            {
                for(int f=GridProc.CurrentCell.RowIndex+1; f<= GridProc.Rows.Count-2; f++)
                {
                    if (GridProc.CurrentCell.ColumnIndex == GridProc.Columns["PRO_RATE"].Index)
                    {
                        if (GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridProc["PROC_ID", GridProc.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            //if(GridProc["PROC_ID", GridProc.CurrentCell.RowIndex].Value.ToString() == GridProc["PROC_ID", f].Value.ToString())
                            //{
                                GridProc["PRO_RATE", f].Value = GridProc["PRO_RATE", GridProc.CurrentCell.RowIndex].Value.ToString();
                                if (GridProc["REQ_QTY", f].Value.ToString() == String.Empty)
                                {
                                    GridProc["REQ_QTY", f].Value = 0.00;
                                }                                
                                GridProc["PRO_AMOUNT", f].Value = Convert.ToDouble(GridProc["PRO_RATE", f].Value) * Convert.ToDouble(GridProc["REQ_QTY", f].Value);                                
                            //}
                        }
                    }                    
                }
            }             
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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

        private void GridSpcl_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Spcl == null)
                {
                    Txt_Spcl = (TextBox)e.Control;
                    Txt_Spcl.KeyDown +=  new KeyEventHandler(Txt_Spcl_KeyDown);
                    Txt_Spcl.KeyPress += new KeyPressEventHandler(Txt_Spcl_KeyPress);  
                    Txt_Spcl.Leave +=new EventHandler(Txt_Spcl_Leave);                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridSpcl_KeyPress(object sender, KeyPressEventArgs e)
        {
             try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Total_Count();
                    TxtSpclWgt.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridSpcl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (GridSpcl["ITEM", GridSpcl.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridSpcl["ITEM", GridSpcl.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridSpcl["COLOR", GridSpcl.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridSpcl["SIZE", GridSpcl.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridSpcl["ACCESS_TYPE", GridSpcl.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        if (GridSpcl.Rows.Count > 2)
                        {
                            for (int k = 0; k < GridSpcl.Rows.Count - 2; k++)
                            {
                                if ((k != GridSpcl.CurrentCell.RowIndex) && (GridSpcl["ACCESS_TYPE", k].Value.ToString()) == "EXCESS" && (GridSpcl["ITEM", k].Value.ToString()) == GridSpcl["ITEM", GridSpcl.CurrentCell.RowIndex].Value.ToString() && (GridSpcl["COLOR", k].Value.ToString()) == (GridSpcl["COLOR", GridSpcl.CurrentCell.RowIndex].Value.ToString()) && (GridSpcl["SIZE", k].Value.ToString()) == GridSpcl["SIZE", GridSpcl.CurrentCell.RowIndex].Value.ToString() && (GridSpcl["ACCESS_TYPE", k].Value.ToString()) == GridSpcl["ACCESS_TYPE", GridSpcl.CurrentCell.RowIndex].Value.ToString())
                                {
                                    MessageBox.Show("Already ACCESS_TYPE, ITEM , COLOR & SIZE are Available", "Gainup");
                                    GridSpcl["ITEM", GridSpcl.CurrentCell.RowIndex].Value = "";
                                    GridSpcl["COLOR", GridSpcl.CurrentCell.RowIndex].Value = "";
                                    GridSpcl["SIZE", GridSpcl.CurrentCell.RowIndex].Value = "";
                                    GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value = 0;                                    
                                    k = GridSpcl.Rows.Count;
                                    Total_Count();
                                    GridSpcl.CurrentCell = GridSpcl["ACCESS_TYPE", GridSpcl.CurrentCell.RowIndex];
                                    GridSpcl.Focus();
                                    GridSpcl.BeginEdit(true);
                                    e.Handled = true;
                                    return;
                                }
                            }

                        }
                    }

                    if (GridSpcl.CurrentCell.ColumnIndex == GridSpcl.Columns["PUR_RATE"].Index)
                    {
                        if (GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {

                            GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value = 0.00;
                            GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = 0.00;
                        }
                        else if (Convert.ToDouble(GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value) > 0)
                        {
                            if (Convert.ToDouble(GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value) <= Convert.ToDouble(GridSpcl["BOM_CONS1", GridSpcl.CurrentCell.RowIndex].Value))
                            {
                                GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = Convert.ToDouble(GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value);
                            }
                            else
                            {
                                MessageBox.Show("Invalid BOM_CONS");
                                GridSpcl["BOM_CONS", GridSpcl.CurrentCell.RowIndex].Value = 0.00;
                                GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value = 0.00;
                                GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = 0.00;
                                GridSpcl.CurrentCell = GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex];
                                GridSpcl.Focus();
                                GridSpcl.BeginEdit(true);
                                e.Handled = true;
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid PUR_RATE");
                            GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex].Value = 0.00;
                            GridSpcl["PUR_AMOUNT", GridSpcl.CurrentCell.RowIndex].Value = 0.00;
                            GridSpcl.CurrentCell = GridSpcl["PUR_RATE", GridSpcl.CurrentCell.RowIndex];
                            GridSpcl.Focus();
                            GridSpcl.BeginEdit(true);
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

        private void GridSpcl_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
             try
            {
                if (GridSpcl.Rows.Count >= 2)
                {
                    MyBase.Row_Number(ref GridSpcl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridSpcl_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
             try
            {
                MyBase.Row_Number(ref GridSpcl);
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridSpcl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (GridSpcl["FLAG", GridSpcl.CurrentCell.RowIndex].Value.ToString() == "F" || GridSpcl["FLAG", GridSpcl.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                       //if(GridSpcl["ACCESS_TYPE", GridSpcl.CurrentCell.RowIndex].Value.ToString() != "REPLACE")
                       //{
                            GridSpcl.Focus();
                            if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                 if (GridSpcl["ACCESS_TYPE", GridSpcl.CurrentCell.RowIndex].Value.ToString() == "REPLACE")
                                    {
                                        for (int k = 0; k < GridYarn.Rows.Count; k++)
                                        {
                                            if (GridYarn["ITEMID", k].Value.ToString() == GridSpcl["REQ_ITEMID", GridSpcl.CurrentCell.RowIndex].Value.ToString() && GridYarn["SIZEID", k].Value.ToString() == GridSpcl["REQ_SIZEID", GridSpcl.CurrentCell.RowIndex].Value.ToString() && GridYarn["COLORID", k].Value.ToString() == GridSpcl["REQ_COLORID", GridSpcl.CurrentCell.RowIndex].Value.ToString() && GridYarn["SNO", k].Value.ToString() == GridSpcl["SNO1", GridSpcl.CurrentCell.RowIndex].Value.ToString())
                                            {
                                                GridYarn["LOSS_WEIGHT", k].Value = (((Convert.ToDouble(GridYarn["BOM_CONS1", k].Value)) * (Convert.ToDouble(GridYarn["LOSS_PER", k].Value))/100));
                                                GridYarn["DYE_AMOUNT", k].Value = (((Convert.ToDouble(GridYarn["BOM_CONS1", k].Value)) + Convert.ToDouble(GridYarn["LOSS_WEIGHT", k].Value)) * (Convert.ToDouble(GridYarn["DYE_RATE", k].Value)));                     
                                                GridYarn["BOM_CONS", k].Value = ((Convert.ToDouble(GridYarn["BOM_CONS1", k].Value)) +  Convert.ToDouble(GridYarn["LOSS_WEIGHT", k].Value)) ;                                                
                                                GridYarn["PUR_AMOUNT", k].Value = (Convert.ToDouble(GridYarn["BOM_CONS1", k].Value) +  Convert.ToDouble(GridYarn["LOSS_WEIGHT", k].Value)) * Convert.ToDouble(GridYarn["PUR_RATE", k].Value);
                                                if(GridYarn["FLAG", k].Value.ToString() == "T")
                                                {
                                                    GridYarn["FLAG", k].Value = "S";
                                                }
                                                else
                                                {
                                                       GridYarn["FLAG", k].Value = "F";
                                                }
                                                k= GridYarn.Rows.Count;
                                            }
                                        }
                                    }                                    
                                   // Dt4.Rows.RemoveAt(GridSpcl.CurrentCell.RowIndex);
                                    GridSpcl.Rows.RemoveAt(GridSpcl.CurrentCell.RowIndex);  
                            }
                       //}
                    }
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
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButClear_Click(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                Entry_Edit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 

        }

        private void ButApp_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show ("Sure to Approve ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
                
                MyBase.Run("Update Socks_Planning_Yarn_Details Set Approval_Flag_Sample = 'T', Approval_Time_Sample = Getdate(), Approval_System_Sample = Host_NAme() Where Master_ID = " + Code + " and Spl_Req_Mode = 'T' and Approval_Flag = 'F'", "Exec Socks_Yarn_Planning_Import_Proc '" + TxtOcnNo.Text.ToString() + "'");
                MessageBox.Show("Approved", "Gainup");
                MyBase.Clear(this);
                Entry_Edit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButReject_Click(object sender, EventArgs e)
        {
             try
            {
                if (MessageBox.Show ("Sure to Reject ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
                MyBase.Run("Update Socks_Planning_Yarn_Details Set Approval_Flag_Sample = 'F', Approval_Time_Sample = Getdate(), Approval_System_Sample = Host_NAme() Where Master_ID = " + Code + " and Spl_Req_Mode = 'T' and Approval_Flag = 'F'", "Exec Socks_Yarn_Planning_Import_Proc '" + TxtOcnNo.Text.ToString() + "'");
                MessageBox.Show("Rejected", "Gainup");
                MyBase.Clear(this);
                Entry_Edit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
       
    }
}
