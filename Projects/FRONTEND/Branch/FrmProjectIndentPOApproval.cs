using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Text;
using System.Windows.Forms;
namespace Accounts
{
    public partial class FrmProjectIndentPOApproval : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        Int64 Code = 0;
        String NOOFOCN = "";
        // Entry_New also Declared
        DataTable Dt = new DataTable();
       
        String F1 = "T";
       
        DataTable Dt_Item = new DataTable();
       
        Boolean Status_Flag = false;
       
        Int32 Max_Val=500;
        TextBox Txt = null;
        TextBox Txt_Item = null;
       
        int cc = 0;
        public FrmProjectIndentPOApproval()
        {
            InitializeComponent();
        }

        void PONO_Generate()
        {
            try
            {                
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select Projects.Dbo.[Get_Max_Project_Po_Indent] ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "')", ref Tdt);
                TxtPONO.Text = Tdt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                TxtPONO.Text = String.Empty;
                throw ex;
            }
        }

        public void Entry_New()
        {
            try
            {
                MyBase.Enable_Controls(this, true);
                MyBase.Clear(this);
                lblMail.Text = "";
                DataTable Dtm = new DataTable();
                DataTable Dt1 = new DataTable();
                String Str, Str1, Str2, Str3, Str4;  
                DataTable Dt2 = new DataTable();
                DataTable Dt3 = new DataTable();
                DataTable Dt4 = new DataTable();
                String Order = "";
                Int32 N = 0;
                ChkSize.Checked = false;                
               
                Code = 0;
                Dt = new DataTable();
             
                Dt_Item = new DataTable();
                   
                TxtBuyer.Enabled = true;                               
                DtpReqDate.Value = DtpDate.Value.AddDays(15);
                DtpComDate.Value = DtpDate.Value.AddDays(15);
                PONO_Generate();
                Load_Item();
                TxtIndent.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Fill_Data(DataRow Dr)
        {
            try
            {
                Code = Convert.ToInt64(Dr["RowiD"]);
                TxtPONO.Text = Dr["Indent_No"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["Indent_Date"]);
                DtpReqDate.Value = Convert.ToDateTime(Dr["Required_Date"]);
                DtpComDate.Value = Convert.ToDateTime(Dr["Commit_Date"]); 
                TxtSupplier.Tag = Dr["Supplier_Code"].ToString();
                TxtSupplier.Text = Dr["Supplier"].ToString();
                lblMail.Text = Dr["MailId"].ToString();
                lblMail.Tag = Dr["Country_Code"].ToString();
                TxtBuyer.Tag = Dr["Buyer_Code"].ToString();
                if (TxtBuyer.Tag.ToString() == "0")
                {
                    TxtBuyer.Text = "";
                }
                else
                {
                    TxtBuyer.Text = Dr["PArty"].ToString();
                }
                TxtIndent.Text = Dr["Ref_No"].ToString();
                DtpIndent.Value = Convert.ToDateTime(Dr["Ref_Date"]); 
                TxtRemarks.Text = Dr["Remarks_Mas"].ToString();
                ChkSize.Checked = false;
                TxtBuyer.Enabled = false;              
                tabControl1.SelectTab(tabPage3);                
                Dt_Item = new DataTable();                
                Load_Item();
                Grid_Item.CurrentCell = Grid_Item["ORDER_NO", 0];
                Grid_Item.Focus();
                Grid_Item.BeginEdit(true);
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
                MyBase.Enable_Controls(this, true);
                MyBase.Clear(this);
                lblMail.Text = "";
                Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select PO Details - Approve", "select PoNo Indent_NO, PoDate Indent_Date, Indent_NO Ref_No, Indent_Date Ref_Date, Proj_Type, Proj_ACtivity_NAme, Order_No, Item, Color, Size, Uom1, Order_Qty_Conv Order_Qty, Grs_Rate_Conv Grs_Rate, Tax_Per, Freight_Rate_Conv Freight_Rate, Rate_Conv Pur_Rate, Grs_Amount_Dtl, Tax_Amount_Dtl, Freight_Amount_Dtl, Pur_Amount, Approval_Flag_PO First_Approval, Approval_Flag_PO1 Second_Approval, REquired_Date, Ack_Date, Commit_Date, MailId, PArty, Supplier, Cancel_Qty_Conv Cancel_Qty, Tot_Grs_Amount, Tot_Tax_Amount, Tot_Freight_Amount, Tot_Net_Amount,  Remarks_Mas ,  Supplier_Code, Buyer_Code, Country_Code, Rowid, Detail_ID  From Projects.Dbo.Project_PO_Indent_Details_Fn() Where Approval_Flag_PO = 'F' ORder by PoNo desc, Item, Color, Size, Proj_ACtivity_NAme ", String.Empty, 100, 100, 80, 80, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Fill_Data(Dr);
                    ButApp.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
      

        public void Entry_Save()
        {
            int Slno = 1;
            try
            {
                TxtSupplier.Focus();
                if (TxtSupplier.Text.Trim() == String.Empty)
                {
                    TxtSupplier.Tag = "0";
                }
                //if (TxtSupplier.Text.Trim() == String.Empty)
                //{
                //    MessageBox.Show("Invalid Supplier ...!", "Gainup");
                //    TxtSupplier.Focus();
                //    MyParent.Save_Error = true;
                //    return;
                //}
                F1 = "T";
                DataTable Dts = new DataTable();
                String St1 = "Select * From ACCOUNTS.dbo.Blocked_Ledgers_List(1) Where Ledger_Code= " + TxtSupplier.Tag.ToString() + "";
                MyBase.Load_Data(St1, ref Dts);
                if (Dts.Rows.Count > 0)
                {
                    MessageBox.Show("This Supplier Has Been Blocked By Accounts...!");
                    MyParent.Save_Error = true;
                    TxtSupplier.Focus();
                    return;
                }

                                
                    if (Dt_Item.Rows.Count == 0)
                    {
                        MessageBox.Show("Invalid Details ...!", "Gainup");
                        TxtSupplier.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    Calculate_Item();                    
                    if (TxtBuyer.Text.Trim() == String.Empty)
                    {
                        TxtBuyer.Tag = "0";
                    }
               
 

                if (TxtQTY.Text.Trim() == String.Empty || Convert.ToDouble(TxtQTY.Text) == 0)
                {
                    MessageBox.Show("Invalid Qty ...!", "Gainup");
                    TxtSupplier.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtAmount.Text.Trim() == String.Empty || Convert.ToDouble(TxtAmount.Text) == 0)
                {
                    MessageBox.Show("Invalid Amount ...!", "Gainup");
                    TxtSupplier.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (Convert.ToDateTime(DtpDate.Value) > Convert.ToDateTime(DtpReqDate.Value))
                {                    
                        MessageBox.Show("Invalid Req Date", "Gainup");
                        DtpReqDate.Value = MyBase.GetServerDate();
                        DtpReqDate.Focus();
                        MyParent.Save_Error = true;
                        return;                    
                }

                if (Convert.ToDateTime(DtpDate.Value) > Convert.ToDateTime(DtpComDate.Value))
                {
                    MessageBox.Show("Invalid Comm Date", "Gainup");
                    DtpComDate.Value = MyBase.GetServerDate();
                    DtpComDate.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                
                                if (Grid_Item.Rows.Count > 1)
                                {
                                    for (int i = 0; i <= Grid_Item.Rows.Count - 2; i++)
                                    {
                                        for (int k = i+1; k < Grid_Item.Rows.Count - 1; k++)
                                        {
                                            if (((Grid_Item["ORDER_NO", k].Value.ToString()) == Grid_Item["ORDER_NO", i].Value.ToString() && (Grid_Item["PROJ_ACTIVITY_ID", k].Value.ToString()) == Grid_Item["PROJ_ACTIVITY_ID", i].Value.ToString() && (Grid_Item["PROJ_TYPE_ID", k].Value.ToString()) == (Grid_Item["PROJ_TYPE_ID", i].Value.ToString()) && (Grid_Item["ITEM_ID", k].Value.ToString()) == (Grid_Item["ITEM_ID", i].Value.ToString()) && (Grid_Item["COLOR_ID", k].Value.ToString()) == (Grid_Item["COLOR_ID", i].Value.ToString()) && (Grid_Item["SIZE_ID", k].Value.ToString()) == (Grid_Item["SIZE_ID", i].Value.ToString())))
                                            {                                            
                                                MessageBox.Show("Already ORDER_NO & Activity , Items are Available", "Gainup");
                                                k = Grid_Item.Rows.Count;                                       
                                                Grid_Item.CurrentCell = Grid_Item["Grs_Rate", i];
                                                Grid_Item.Focus();
                                                Grid_Item.BeginEdit(true);    
                                                MyParent.Save_Error = true;
                                                return;
                                            }                                            
                                        }

                                        if ((Convert.ToDouble(Grid_Item["PO_Qty", i].Value) > Convert.ToDouble(Grid_Item["Bal", i].Value)))
                                        {
                                            MessageBox.Show("PO Qty is greater than Balance ...!", "Gainup");
                                            Grid_Item["PO_Qty", i].Value = Grid_Item["Bal", i].Value;
                                            Grid_Item.CurrentCell = Grid_Item["PO_Qty", i];
                                            Grid_Item.Focus();
                                            Grid_Item.BeginEdit(true);
                                            MyParent.Save_Error = true;
                                            return;
                                        }

                                        if ((Convert.ToDouble(Grid_Item["Grs_Rate", i].Value) > Convert.ToDouble(Grid_Item["App_Grs", i].Value)))
                                        {
                                            MessageBox.Show("Grs Rate is greater than App Grs...!", "Gainup");
                                            Grid_Item["Grs_Rate", i].Value = Grid_Item["App_Grs", i].Value;
                                            Grid_Item.CurrentCell = Grid_Item["Grs_Rate", i];
                                            Grid_Item.Focus();
                                            Grid_Item.BeginEdit(true);
                                            MyParent.Save_Error = true;
                                            return;
                                        }

                                        if ((Convert.ToDouble(Grid_Item["Pur_Rate", i].Value) > Convert.ToDouble(Grid_Item["App_Pur_Rate", i].Value)))
                                        {
                                            MessageBox.Show("Pur Rate is greater than App Pur...!", "Gainup");
                                            Grid_Item["Grs_Rate", i].Value = Grid_Item["Grs_Rate", i].Value;
                                            Grid_Item.CurrentCell = Grid_Item["Grs_Rate", i];
                                            Grid_Item.Focus();
                                            Grid_Item.BeginEdit(true);
                                            MyParent.Save_Error = true;
                                            return;
                                        }

                                        if ((Convert.ToDouble(Grid_Item["Grs_Rate", i].Value) > Convert.ToDouble(Grid_Item["App_Grs", i].Value)) || (Convert.ToDouble(Grid_Item["Grs_Rate_Conv", i].Value) > Convert.ToDouble(Grid_Item["App_Grs_org", i].Value)))
                                        {
                                            if (MessageBox.Show("Grs Rate is greater than App Grs..! Sure to Save ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                                            {
                                                MyParent.Save_Error = true;
                                                return;
                                            }
                                            else
                                            {
                                                F1 = "F";
                                            }                                           
                                        }


                                        //if (Convert.ToDouble(Grid_Item["Tax_Per", i].Value) > Convert.ToDouble(Grid_Item["App_Tax", i].Value))
                                        //{                                           
                                        //    if (MessageBox.Show("Tax Per is greater than App Tax..! Sure to Save ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                                        //    {
                                        //        MyParent.Save_Error = true;
                                        //        return;
                                        //    }
                                        //    else
                                        //    {
                                        //        F1 = "F";
                                        //    }
                                        //}

                                        if ((Convert.ToDouble(Grid_Item["Freight_Rate", i].Value) > Convert.ToDouble(Grid_Item["App_Frei", i].Value)) || (Convert.ToDouble(Grid_Item["Frei_Rate_Conv", i].Value) > Convert.ToDouble(Grid_Item["App_Frei_Org_Conv", i].Value)))
                                        {
                                          //  MessageBox.Show(" Freight Rate is greater than App Frei...!", "Gainup");
                                          ////  Grid_Item["Freight_Rate", i].Value = Grid_Item["App_Frei", i].Value;
                                          //  Grid_Item.CurrentCell = Grid_Item["Freight_Rate", i];
                                          //  Grid_Item.Focus();
                                          //  Grid_Item.BeginEdit(true);
                                          //  //MyParent.Save_Error = true;
                                          //  //return;
                                        }


                                        if ((Convert.ToDouble(Grid_Item["RO_Amt", i].Value) >= Convert.ToDouble("5")) || (Convert.ToDouble(Grid_Item["RO_Amt", i].Value) <= Convert.ToDouble("-5")))
                                        {
                                            //MessageBox.Show(" RO_Amt is greater than 5 or less than -5...!", "Gainup");
                                            ////Grid_Item["Pur_Rate", Grid_Item.CurrentCell.RowIndex].Value = Grid_Item["App_Pur_Rate", Grid_Item.CurrentCell.RowIndex].Value;
                                            //Grid_Item.CurrentCell = Grid_Item["RO_Amt", i];
                                            //Grid_Item.Focus();
                                            //Grid_Item.BeginEdit(true);
                                            //MyParent.Save_Error = true;
                                            //return;
                                            if (MessageBox.Show("Ro Rate is Mismatch..! Sure to Save ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                                            {
                                                MyParent.Save_Error = true;
                                                return;
                                            }
                                            else
                                            {
                                                F1 = "F";
                                            }
                                        }                                     
                                    }
                                }
               

                PONO_Generate();

                String[] Queries = new String[500];
                Int32 Array_Index = 0;               
                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert Into Projects.Dbo.Project_PO_Indent_Master (PoNo, PODate, Supplier_Code, Required_Date,  Buyer_Code, Commit_Date,MailId, Indent_No, Indent_Date, Grs_Amount, Tax_amount, Freight_Amount, Net_Amount, Remarks, Approval_flag, Approval_flag1) Values ('" + TxtPONO.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtSupplier.Tag.ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpReqDate.Value) + "',  " + TxtBuyer.Tag.ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpComDate.Value) + "','" + lblMail.Text + "', '" + TxtIndent.Text.ToString() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpIndent.Value) + "', " + Convert.ToDouble(TxtAmount.Text.ToString()) + ", " + Convert.ToDouble(TxtTotTax.Text.ToString()) + ", " + Convert.ToDouble(TxtFreightAmt.Text.ToString()) + ", " + Convert.ToDouble(TxtTotal.Text.ToString()) + ", '" + TxtRemarks.Text.ToString() + "', 'F', '" + F1 + "'); Select Scope_Identity ()";
                    Queries[Array_Index++] = MyParent.EntryLog("PROJECT INDENT PO", "ADD", "@@IDENTITY");

                }
                else
                {
                    Queries[Array_Index++] = "update Projects.Dbo.Project_PO_INDENT_Master  Set Supplier_Code = " + TxtSupplier.Tag.ToString() + ", Required_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpReqDate.Value) + "', Commit_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpComDate.Value) + "', MailId = '" + lblMail.Text + "', Indent_No = '" + TxtIndent.Text.ToString() + "', Indent_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpIndent.Value) + "', Grs_Amount = " + Convert.ToDouble(TxtAmount.Text.ToString()) + ",Tax_amount = " + Convert.ToDouble(TxtTotTax.Text.ToString()) + ", Freight_Amount = " + Convert.ToDouble(TxtFreightAmt.Text.ToString()) + ", Net_Amount = " + Convert.ToDouble(TxtTotal.Text.ToString()) + ", Remarks = '" + TxtRemarks.Text.ToString() + "', Approval_Flag = 'F', Approval_Flag1 = '" + F1 + "' Where RowID = " + Code;
                    Queries[Array_Index++] = "Delete From Projects.Dbo.Project_PO_Indent_Details Where Master_ID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("PROJECT INDENT PO", "EDIT", Code.ToString());
                }
                
                    Slno = 1;
                    for (int i = 0; i <= Dt_Item.Rows.Count - 1; i++)
                    {                               
                                if (MyParent._New)
                                {
                                    Queries[Array_Index++] = "Insert Into Projects.Dbo.Project_PO_Indent_Details (Master_ID, Slno, Order_ID, Proj_Type_ID, Proj_Activity_ID, Item_id, Color_id, Size_ID, Order_Qty, Rate, Amount, Grs_Rate, Tax_Per, Tax_Rate, Freight_Rate, Grs_Amount, Tax_Amount, Freight_Amount, Remarks, UomID1, Conv_Val, Ro_Rate, Order_Qty_Conv, Rate_Conv, Grs_Rate_Conv, Freight_Rate_Conv, Tax_Rate_Conv, APP_GRS_ORG, APP_TAX_ORG, APP_FREI_ORG, APP_PUR_RATE_ORG, APP_GRS_ORG_CONV, APP_FREI_ORG_CONV, APP_PUR_RATE_ORG_CONV, APP_GRS, APP_TAX, APP_FREI, APP_PUR_RATE, FREI_TAX_MODE, Ro_Amt, Grs_Amt_Inr, Tax_Amt_Inr, Frei_Amt_Inr, Pur_Amt_Inr, App_Sup_Ex_Rate, CAlc_Type, Cancel_Qty, Cancel_Qty_Conv, PO_Qty_Conv) values (@@IDENTITY, " + Slno + ", " + Dt_Item.Rows[i]["Order_ID"].ToString() + ", " + Dt_Item.Rows[i]["Proj_Type_Id"].ToString() + ", " + Dt_Item.Rows[i]["Proj_Activity_ID"].ToString() + ", " + Dt_Item.Rows[i]["Item_ID"].ToString() + ", " + Dt_Item.Rows[i]["Color_ID"].ToString() + ", " + Dt_Item.Rows[i]["Size_ID"].ToString() + ", " + Dt_Item.Rows[i]["PO_Qty_Conv"].ToString() + " , " + Dt_Item.Rows[i]["Pur_Rate_Conv"].ToString() + ", " + Dt_Item.Rows[i]["Pur_Amt"].ToString() + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Grs_Rate_Conv"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Tax_Per"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Tax_Rate_Conv"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["Frei_Rate_Conv"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Grs_Amt"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Tax_Amt"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Frei_Amt"].ToString()) + ", '" + Dt_Item.Rows[i]["Remarks"].ToString() + "', " + (Dt_Item.Rows[i]["UomID1"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Conv_Val"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["RO_RATe"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["PO_Qty"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Pur_Rate"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Grs_Rate"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Freight_Rate"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Tax_Rate"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["APP_GRS_ORG"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["APP_TAX_ORG"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["APP_FREI_ORG"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_PUR_RATE_ORG"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_GRS_ORG_CONV"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_FREI_ORG_CONV"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_PUR_RATE_ORG_CONV"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_GRS"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_TAX"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_FREI"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_PUR_RATE"].ToString()) + " , '" + (Dt_Item.Rows[i]["FREI_TAX_MODE"].ToString()) + "', " + Convert.ToDouble(Dt_Item.Rows[i]["RO_AMT"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["GRS_AMT_INR"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["TAX_AMT_INR"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["FREI_AMT_INR"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["PUR_AMT_INR"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["App_Sup_Ex_Rate"].ToString()) + ", '" + Dt_Item.Rows[i]["CALC_TYPE"].ToString() + "',0,0,0)";
                                }
                                else
                                {
                                    Queries[Array_Index++] = "Insert Into Projects.Dbo.Project_PO_Indent_Details  (Master_ID, Slno, Order_ID, Proj_Type_ID, Proj_Activity_ID, Item_id, Color_id, Size_ID, Order_Qty, Rate, Amount, Grs_Rate, Tax_Per, Tax_Rate, Freight_Rate, Grs_Amount, Tax_Amount, Freight_Amount, Remarks, UomID1, Conv_Val, Ro_Rate, Order_Qty_Conv, Rate_Conv, Grs_Rate_Conv, Freight_Rate_Conv, Tax_Rate_Conv, APP_GRS_ORG, APP_TAX_ORG, APP_FREI_ORG, APP_PUR_RATE_ORG, APP_GRS_ORG_CONV, APP_FREI_ORG_CONV, APP_PUR_RATE_ORG_CONV, APP_GRS, APP_TAX, APP_FREI, APP_PUR_RATE, FREI_TAX_MODE, Ro_Amt, Grs_Amt_Inr, Tax_Amt_Inr, Frei_Amt_Inr, Pur_Amt_Inr, App_Sup_Ex_Rate, CAlc_Type, Cancel_Qty, Cancel_Qty_Conv, PO_Qty_Conv) values (" + Code + ",  " + Slno + ", " + Dt_Item.Rows[i]["Order_ID"].ToString() + ", " + Dt_Item.Rows[i]["Proj_Type_Id"].ToString() + ", " + Dt_Item.Rows[i]["Proj_Activity_ID"].ToString() + ", " + Dt_Item.Rows[i]["Item_ID"].ToString() + ", " + Dt_Item.Rows[i]["Color_ID"].ToString() + ", " + Dt_Item.Rows[i]["Size_ID"].ToString() + ", " + Dt_Item.Rows[i]["PO_Qty_Conv"].ToString() + " , " + Dt_Item.Rows[i]["Pur_Rate_Conv"].ToString() + ", " + Dt_Item.Rows[i]["Pur_Amt"].ToString() + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Grs_Rate_Conv"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Tax_Per"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Tax_Rate_Conv"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["Frei_Rate_Conv"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Grs_Amt"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Tax_Amt"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Frei_Amt"].ToString()) + ", '" + Dt_Item.Rows[i]["Remarks"].ToString() + "', " + (Dt_Item.Rows[i]["UomID1"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Conv_Val"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["RO_RATe"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["PO_Qty"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Pur_Rate"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Grs_Rate"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Freight_Rate"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["Tax_Rate"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["APP_GRS_ORG"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["APP_TAX_ORG"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["APP_FREI_ORG"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_PUR_RATE_ORG"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_GRS_ORG_CONV"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_FREI_ORG_CONV"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_PUR_RATE_ORG_CONV"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_GRS"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_TAX"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_FREI"].ToString()) + " , " + Convert.ToDouble(Dt_Item.Rows[i]["APP_PUR_RATE"].ToString()) + " , '" + (Dt_Item.Rows[i]["FREI_TAX_MODE"].ToString()) + "', " + Convert.ToDouble(Dt_Item.Rows[i]["RO_AMT"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["GRS_AMT_INR"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["TAX_AMT_INR"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["FREI_AMT_INR"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["PUR_AMT_INR"].ToString()) + ", " + Convert.ToDouble(Dt_Item.Rows[i]["App_Sup_Ex_Rate"].ToString()) + ", '" + Dt_Item.Rows[i]["CALC_TYPE"].ToString() + "',0,0,0)";
                                }
                                Slno++;
                    }
         
                MyBase.Run_Identity(MyParent.Edit, Queries);                
                MessageBox.Show("Saved ...!", "Gainup");
                MyParent.Save_Error = false;
                MyBase.Clear(this);
                TxtSupplier.Focus();
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
                MyBase.Enable_Controls(this, false);
                MyBase.Clear(this);
                lblMail.Text = "";
                Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select PO Details - Delete", "select PoNo, Supplier, PoDate, Indent_NO, Indent_Date, Proj_Type, Proj_ACtivity_NAme, Order_No, PArty, Item, Color, Size, Order_Qty_Conv Order_Qty, Cancel_Qty_Conv Cancel_Qty, Grs_Rate_Conv Grs_Rate, Tax_Per, Freight_Rate_Conv Freight_Rate, Rate_Conv Pur_Rate, Grs_Amount_Dtl, Tax_Amount_Dtl, Freight_Amount_Dtl, Pur_Amount, Approval_Flag_PO First_Approval, Approval_Flag_PO1 Second_Approval, REquired_Date, Ack_Date, Commit_Date, MailId, Tot_Grs_Amount, Tot_Tax_Amount, Tot_Freight_Amount, Tot_Net_Amount,  Remarks_Mas , Supplier_Code, Buyer_Code, Country_Code, Rowid, Detail_ID From Projects.Dbo.Project_PO_Indent_Details_Fn() Where Approval_Flag_PO = 'F' ORder by 1 desc ", String.Empty, 100, 100, 80, 80, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Fill_Data(Dr);
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
            String[] Queries = new String[30];
            Int32 Array_Index = 0;
            try
            {
                if (Code > 0)
                {
                    Queries[Array_Index++] = "Delete From Projects.Dbo.Project_PO_Indent_Details Where Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete From Projects.Dbo.Project_PO_Indent_Master Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("PROJECT INDENT PO", "DELETE", Code.ToString());
                    MyBase.Run(Queries);
                    MessageBox.Show("Deleted ...!", "Gainup");

                    MyBase.Clear(this);
                    MyParent.Load_DeleteEntry();

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
                MyBase.Enable_Controls(this, false);
                MyBase.Clear(this);
                lblMail.Text = "";
                Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select IndentPO Details - View", "select PoNo, Supplier, PoDate, Indent_NO, Indent_Date, Proj_Type, Proj_ACtivity_NAme, Order_No, PArty, Item, Color, Size, Uom1, Order_Qty_Conv Order_Qty, Cancel_Qty_Conv Cancel_Qty, Grs_Rate_Conv Grs_Rate, Tax_Per, Freight_Rate_Conv Freight_Rate, Rate_Conv Pur_Rate, Grs_Amount_Dtl, Tax_Amount_Dtl, Freight_Amount_Dtl, Pur_Amount, Approval_Flag_PO First_Approval, Approval_Flag_PO1 Second_Approval, REquired_Date, Ack_Date, Commit_Date, MailId, Tot_Grs_Amount, Tot_Tax_Amount, Tot_Freight_Amount, Tot_Net_Amount,  Remarks_Mas , Supplier_Code, Buyer_Code, Country_Code, Rowid, Detail_ID From Projects.Dbo.Project_PO_Indent_Details_Fn() ORder by 1 desc ", String.Empty, 100, 100, 80, 80, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Fill_Data(Dr);
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
                String Str, Str1, Str2, Str3, Str4;
                String SMail = "";
                String Order = "";
                DataTable Dt1 = new DataTable();
                DataTable Dt2 = new DataTable();
                DataTable Dt3 = new DataTable();
                DataTable Dt4 = new DataTable();

                Str = "Select Rowid, Detail_ID, PoNo, PoDate, Indent_NO, Indent_Date, Supplier, Supplier_COde, REquired_Date, Buyer_Code, Approval_Flag_PO, Ack_Date, Commit_Date, MailId, Tot_Grs_Amount, Tot_Tax_Amount, Tot_Freight_Amount, Tot_Net_Amount, Order_ID, Item_id, Color_id, Size_ID, Order_Qty_Conv Order_Qty, Rate_Conv Pur_Rate, Pur_Amount, Cancel_Qty_Conv Cancel_Qty, Remarks, Grs_Rate_Conv Grs_Rate, Tax_Per,  Proj_ACtivity_ID, Proj_Type_ID, Proj_ACtivity_Name, Proj_Type, Freight_Rate_Conv Freight_Rate, Grs_Amount_Dtl, Tax_Amount_Dtl, Freight_Amount_Dtl, Approval_Time_PO, Approval_System_PO, Order_No, PArty, Party_Code, Item, Color, Size, Rate, Tax_Per_Bud, Freight_Rate_Bud, Grs_Rate_Bud, Bom_Plan, Approval_Flag_Bud, Remarks_Mas from Projects.Dbo.Project_PO_Indent_Details_Fn() Where RowID = " + Code;
                MyBase.Load_Data(Str, ref Dt1);                
                
                if(Dt1.Rows.Count <=0)
                {
                    MessageBox.Show("Indent Not Approved...!", "Gainup");                    
                    return;
                }
                
                DialogResult Res = MessageBox.Show("[Y] - Print; [N] - Mail; Sure to Continue ..?", "Gainup", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);


                Str = "Select Rowid, PoNo, PoDate, Indent_NO, Indent_Date, Supplier, Supplier_COde, REquired_Date, Approval_Flag_PO, Ack_Date, Commit_Date, MailId, Tot_Grs_Amount, Tot_Tax_Amount, Tot_Freight_Amount, Tot_Net_Amount, Approval_Time_PO, Approval_System_PO, Remarks_Mas, PrintOutDate, Address, LedgeR_Phone, Order_Qty, Pur_Rate, Cancel_Qty, Grs_Rate, Tax_Per, Freight_Rate, Grs_Amount_Dtl, Tax_Amount_Dtl, Freight_Amount_Dtl, Pur_Amount, Item, Color, Size, Rate, Description, Conv_Val, Country_Code, Uom1 From Projects.Dbo.Project_Po_Indent_Print_Fn(" + Code + ")";
                MyBase.Execute_Qry(Str, "Project_Material_PO");

                Str2 = "Select Rowid, PoNo, PoDate, Indent_NO, Supplier, Supplier_COde, OrdeR_No + '-' + Proj_ACtivity_Name OrdeR_No, Party, Sum(Order_Qty_Conv) Order_Qty, Rate_Conv Pur_Rate, Sum(Cancel_Qty_Conv) Cancel_Qty, Grs_Rate_Conv Grs_Rate, Tax_Per, Freight_Rate_Conv Freight_Rate, Sum(Grs_Amount_Dtl) Grs_Amount_Dtl, Sum(Tax_Amount_Dtl) Tax_Amount_Dtl, Sum(Freight_Amount_Dtl) Freight_Amount_Dtl, Sum(Pur_Amount) Pur_Amount,  Item, Color, Size, Rate_Conv Rate  from Projects.Dbo.Project_PO_Indent_Details_Fn() Where RowID = " + Code + " Group by Rowid, PoNo, PoDate, Indent_NO, Supplier, Supplier_COde, Pur_Rate, Grs_Rate_Conv, Tax_Per, Freight_Rate_Conv, Item, Color, Size, Rate_Conv, ORder_No, PArty, Proj_ACtivity_NAme ";
                MyBase.Execute_Qry(Str2, "Project_Material_PO_Order_Details");

                Str1 = "Select Distinct ORder_NO from Projects.Dbo.Project_Material_PO_Order_Details ";
                MyBase.Load_Data(Str1, ref Dt3);             

                if (Dt3.Rows.Count > 0)
                {
                    for (int i = 0; i <= Dt3.Rows.Count - 1; i++)
                    {
                        if (Order.ToString() == String.Empty)
                        {
                            Order = Dt3.Rows[i]["Order_No"].ToString();
                        }
                        else
                        {
                            Order = Order + ", " + Dt3.Rows[i]["Order_No"].ToString();
                        }
                    }
                }

                CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RptPurchasePO.rpt");
                if (lblMail.Tag.ToString() == "1")
                {
                  //  MyParent.FormulaFill(ref ObjRpt, "Net_Amount_Word", MyBase.Rupee(Convert.ToDouble(TxtTotal.Text.ToString()), "Paise"));
                }
                else
                {
                 //   MyParent.FormulaFill(ref ObjRpt, "Net_Amount_Word", MyBase.Rupee(Convert.ToDouble(TxtTotal.Text.ToString()), "Cents"));
                }              
                MyParent.FormulaFill(ref ObjRpt, "Order", Order.ToString());
                MyParent.FormulaFill(ref ObjRpt, "Heading", "MATERIAL INDENT PO");
                
                if (Res == DialogResult.Yes)
                {
                    MyParent.CReport(ref ObjRpt, "Material PO Indent ..!");
                }
                else if (Res == DialogResult.No)
                {
                    if (Dt1.Rows[0]["Supplier_Email"].ToString() != String.Empty)
                    {
                        StringBuilder Body = new StringBuilder();
                        Body.Append("Dear Sir, ");
                        Body.Append(Environment.NewLine);
                        Body.Append(Environment.NewLine);
                        Body.Append("Pls Find Attachment");
                        MyParent.CReport_Normal_PDF(ref ObjRpt, "Material PO Indent..!", "C:\\Vaahrep\\GainupPO.Pdf", false);
                      //  MyBase.sendEMailThroughOUTLOOK_Send(Dt1.Rows[0]["Supplier_Email"].ToString(), "Fit@gainup.in", " Material PO Indent ..!", " ", "C:\\Vaahrep\\GainupPO.pdf");
                        MyBase.Run("Update Projects.Dbo.Project_PO_Indent_Master Set Ack_Date = Getdate() Where RowID = " + Code + "", "Insert into Project_PO_Mail_Log_Details (POMasID, MailID, Mode) Values (" + Code + ", '" + Dt1.Rows[0]["Supplier_Email"].ToString() + "', 'Material Indent')");
                        MessageBox.Show("Mail has been Sent...!", "Gainup");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Invalid Mail ID...!", "Gainup");
                        return;
                    }
                    //}
                }
                else
                {
                    MyParent.Load_ViewEntry();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void FrmProjectIndentPOApproval_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                MyBase.Clear(this);
                ChkSize.Checked = false;
                MyParent.Edit = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmProjectIndentPOApproval_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtPONO")
                    {
                        ButApp.Focus();                         
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtPONO")
                    {
                        Entry_Edit();
                       
                    }
                    else if (this.ActiveControl.Name == "TxtSupplier")
                    {
                        Int16 Cont =0;
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "Select LedgeR_Name Supplier, LedgeR_Email, LedgeR_Code Code, Country_COde, 0 Old_ID From Projects.Dbo.Supplier_All_Fn()  ", String.Empty, 250, 200);
                            if (Dr != null)
                            {

                                DataTable Dts = new DataTable();
                                String St1 = "Select * From ACCOUNTS.dbo.Blocked_Ledgers_List(1) Where Ledger_Code= " + Dr["Code"].ToString() + "";
                                MyBase.Load_Data(St1, ref Dts);
                                if (Dts.Rows.Count > 0)
                                {
                                    MessageBox.Show("This Supplier Has Been Blocked By Accounts...!");
                                    TxtSupplier.Focus();
                                    return;
                                }

                               
                                    TxtSupplier.Text = Dr["Supplier"].ToString();
                                    TxtSupplier.Tag = Dr["Code"].ToString();
                                    lblMail.Text = Dr["LedgeR_Email"].ToString();
                                    lblMail.Tag = Dr["Country_COde"].ToString();                                    
                            }
                        
                    }
                }
                else
                {
                    if (this.ActiveControl is TextBox)
                    {
                        e.Handled = true;
                    }
                }
                
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        void Load_Item()
        {
            try
            {
                if (TxtPONO.Text.ToString() == string.Empty)
                {
                    Grid_Item.DataSource = MyBase.Load_Data("SELECT 0 SL, ORDER_NO, PROJ_TYPE, PROJ_ACTIVITY_NAME,  ITEM, COLOR, SIZE, UOM, UOM UOM1, 1.000 CONV_VAL, 0.000 BOM_ORG, 0.000 ORDERED_ORG, 0.000 BAL_ORG, 0.000 PO_QTY_ORG, 0.0000 APP_GRS_ORG, 0.0 APP_TAX_ORG, 0.0000 APP_FREI_ORG, 0.0000 APP_PUR_RATE_ORG, 0.000 PO_QTY_ORG_CONV, 0.0000 APP_GRS_ORG_CONV, 0.0000 APP_FREI_ORG_CONV, 0.0000 APP_PUR_RATE_ORG_CONV, 0.000 BOM, 0.000 ORDERED, 0.000 BAL, 0.000 PO_QTY, 0.0000 APP_GRS, 0.0 APP_TAX, 0.0000 APP_FREI, 0.0000 APP_PUR_RATE, 0.0000 GRS_RATE, 0.0 TAX_PER, 0.0000 TAX_RATE, 0.0000 FREIGHT_RATE, 'B' FREI_TAX_MODE, 0.0000 RO_RATE, 0.0000 PUR_RATE, 0.00 GRS_AMT, 0.00 TAX_AMT, 0.00 FREI_AMT, 0.00 RO_AMT, 0.00 PUR_AMT, 0.000 PO_QTY_CONV, 0.0000 GRS_RATE_CONV, 0.0000 TAX_RATE_CONV, 0.0000 FREI_RATE_CONV, 0.0000 PUR_RATE_CONV, 0.0000 GRS_AMT_INR, 0.0000 TAX_AMT_INR, 0.0000 FREI_AMT_INR, 0.0000 PUR_AMT_INR, App_Sup_Ex_Rate, Order_ID, PRoj_Type_ID, Proj_Activity_ID, CAst(Proj_Type_ID as Varchar(20)) + '-' + Cast(Proj_Activity_ID as Varchar(20)) + '-' + Cast(ITEM_ID as Varchar(20)) + '-' + Cast(COLOR_ID as Varchar(20)) + '-' + Cast(SIZE_ID as Varchar(20)) DESCRIPTION ,  0 UOMID, ITEM_ID, COLOR_ID, SIZE_ID, 0 UOMID1, '/' CALC_TYPE,  0 RNO, '' DESC2, '-' REMARKS FROM Projects.Dbo.[Project_PO_Indent_Pending_Fn]()  WHERE 1 = 2", ref Dt_Item);
                }
                else
                {
                    Grid_Item.DataSource = MyBase.Load_Data("SELECT SL, ORDER_NO, PROJ_TYPE, PROJ_ACTIVITY_NAME,  ITEM, COLOR, SIZE, UOM, UOM1, CONV_VAL, BOM_ORG, ORDERED_ORG, BAL_ORG, PO_QTY_ORG, APP_GRS_ORG, APP_TAX_ORG, APP_FREI_ORG, APP_PUR_RATE_ORG, PO_QTY_ORG_CONV, APP_GRS_ORG_CONV, APP_FREI_ORG_CONV, APP_PUR_RATE_ORG_CONV, BOM, ORDERED, BAL, PO_QTY, APP_GRS, APP_TAX, APP_FREI, APP_PUR_RATE, GRS_RATE, TAX_PER, TAX_RATE, FREIGHT_RATE, FREI_TAX_MODE, RO_RATE, PUR_RATE, GRS_AMT, TAX_AMT, FREI_AMT, RO_AMT, PUR_AMT, PO_QTY_CONV, GRS_RATE_CONV, TAX_RATE_CONV, FREI_RATE_CONV, PUR_RATE_CONV, GRS_AMT_INR, TAX_AMT_INR, FREI_AMT_INR, PUR_AMT_INR, App_Sup_Ex_Rate,  Order_ID, PRoj_Type_ID, Proj_Activity_ID, DESCRIPTION, UOMID, ITEM_ID, COLOR_ID, SIZE_ID, UOMID1, CALC_TYPE,  0 RNO, '' DESC2, REMARKS  FROM Projects.Dbo.Project_PO_Indent_Qty_GRid_View_Fn()  WHERE Master_ID = " + Code + " Order by Item ,Color, Size, PROJ_ACTIVITY_NAME", ref Dt_Item);                                                                                                                                                        
                }

                if (MyParent.UserCode == 1)
                {
                    MyBase.Grid_Designing(ref Grid_Item, ref Dt_Item,"UOM1", "CONV_VAL","BAL", "Order_ID", "PRoj_Type_ID", "Proj_Activity_ID", "ITEM_ID", "COLOR_ID", "SIZE_ID", "UOMID1", "Bom_Org", "Ordered_Org", "Bal_Org", "PO_Qty_Org", "App_Grs_Org", "App_Tax_Org", "App_Frei_Org", "App_Pur_Rate_Org", "PO_Qty_Org_Conv", "App_Grs_Org_Conv", "App_Frei_Org_Conv", "App_Pur_Rate_Org_Conv", "PO_Qty_Conv", "Grs_Rate_Conv", "Tax_Rate_Conv", "Frei_Rate_Conv", "Pur_Rate_Conv", "App_Sup_Ex_Rate", "GRS_AMT_INR", "TAX_AMT_INR", "FREI_AMT_INR", "PUR_AMT_INR", "RNO", "DESC2", "UOMID", "UOMID1", "ORDER_ID", "DESCRIPTION", "REMARKS", "APP_GRS", "APP_TAX", "APP_FREI", "APP_PUR_RATE");
                }
                else
                {
                    MyBase.Grid_Designing(ref Grid_Item, ref Dt_Item, "UOM1", "CONV_VAL","BAL", "Order_ID", "PRoj_Type_ID", "Proj_Activity_ID", "ITEM_ID", "COLOR_ID", "SIZE_ID", "UOMID1", "Bom_Org", "Ordered_Org", "Bal_Org", "PO_Qty_Org", "App_Grs_Org", "App_Tax_Org", "App_Frei_Org", "App_Pur_Rate_Org", "PO_Qty_Org_Conv", "App_Grs_Org_Conv", "App_Frei_Org_Conv", "App_Pur_Rate_Org_Conv", "PO_Qty_Conv", "Grs_Rate_Conv", "Tax_Rate_Conv", "Frei_Rate_Conv", "Pur_Rate_Conv", "App_Sup_Ex_Rate", "GRS_AMT_INR", "TAX_AMT_INR", "FREI_AMT_INR", "PUR_AMT_INR", "RNO", "DESC2", "UOMID", "UOMID1", "ORDER_ID", "DESCRIPTION", "REMARKS", "APP_GRS", "APP_TAX", "APP_FREI", "APP_PUR_RATE");
                }
                 MyBase.ReadOnly_Grid_Without(ref Grid_Item, "ORDER_NO");
                MyBase.Grid_Colouring(ref Grid_Item, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid_Item, 40, 120, 120, 150, 120, 100, 100, 100, 100, 100, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80);

																	
                Grid_Item.Columns["BOM"].DefaultCellStyle.Format = "0.000"; Grid_Item.Columns["ORDERED"].DefaultCellStyle.Format = "0.000"; Grid_Item.Columns["PO_QTY"].DefaultCellStyle.Format = "0.000"; Grid_Item.Columns["GRS_RATE"].DefaultCellStyle.Format = "0.0000";
                Grid_Item.Columns["TAX_PER"].DefaultCellStyle.Format = "0.0"; Grid_Item.Columns["FREIGHT_RATE"].DefaultCellStyle.Format = "0.000"; Grid_Item.Columns["PUR_RATE"].DefaultCellStyle.Format = "0.0000"; Grid_Item.Columns["GRS_AMT"].DefaultCellStyle.Format = "0.00";
                Grid_Item.Columns["TAX_AMT"].DefaultCellStyle.Format = "0.00"; Grid_Item.Columns["FREI_AMT"].DefaultCellStyle.Format = "0.00"; Grid_Item.Columns["PUR_AMT"].DefaultCellStyle.Format = "0.00"; Grid_Item.Columns["CONV_VAL"].DefaultCellStyle.Format = "0.000";
                Grid_Item.Columns["BOM_ORG"].DefaultCellStyle.Format = "0.000"; Grid_Item.Columns["ORDERED_ORG"].DefaultCellStyle.Format = "0.000"; Grid_Item.Columns["BAL_ORG"].DefaultCellStyle.Format = "0.000"; Grid_Item.Columns["PO_QTY_ORG"].DefaultCellStyle.Format = "0.000";
                Grid_Item.Columns["APP_GRS_ORG"].DefaultCellStyle.Format = "0.0000"; Grid_Item.Columns["APP_TAX_ORG"].DefaultCellStyle.Format = "0.0"; Grid_Item.Columns["APP_FREI_ORG"].DefaultCellStyle.Format = "0.000"; Grid_Item.Columns["APP_PUR_RATE_ORG"].DefaultCellStyle.Format = "0.0000";
                Grid_Item.Columns["PO_QTY_ORG_CONV"].DefaultCellStyle.Format = "0.000"; Grid_Item.Columns["APP_GRS_ORG_CONV"].DefaultCellStyle.Format = "0.0000"; Grid_Item.Columns["APP_FREI_ORG_CONV"].DefaultCellStyle.Format = "0.000"; Grid_Item.Columns["APP_PUR_RATE_ORG_CONV"].DefaultCellStyle.Format = "0.0000";
                Grid_Item.Columns["BAL"].DefaultCellStyle.Format = "0.000"; Grid_Item.Columns["APP_GRS"].DefaultCellStyle.Format = "0.0000"; Grid_Item.Columns["APP_TAX"].DefaultCellStyle.Format = "0.0"; Grid_Item.Columns["APP_FREI"].DefaultCellStyle.Format = "0.0000";
                Grid_Item.Columns["APP_PUR_RATE"].DefaultCellStyle.Format = "0.0000"; Grid_Item.Columns["TAX_RATE"].DefaultCellStyle.Format = "0.0000"; Grid_Item.Columns["RO_AMT"].DefaultCellStyle.Format = "0.00"; Grid_Item.Columns["PO_QTY_CONV"].DefaultCellStyle.Format = "0.000";
                Grid_Item.Columns["GRS_RATE_CONV"].DefaultCellStyle.Format = "0.0000"; Grid_Item.Columns["TAX_RATE_CONV"].DefaultCellStyle.Format = "0.0000"; Grid_Item.Columns["FREI_RATE_CONV"].DefaultCellStyle.Format = "0.000"; Grid_Item.Columns["PUR_RATE_CONV"].DefaultCellStyle.Format = "0.0000";
                Grid_Item.Columns["RO_RATE"].DefaultCellStyle.Format = "0.0000";
                Grid_Item.Columns["BOM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Item.Columns["ORDERED"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Item.Columns["PO_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid_Item.Columns["REMARKS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid_Item.RowHeadersWidth = 10;
                Grid_Item.Columns["PO_QTY"].HeaderText = "INDENT";
                MyBase.Row_Number(ref Grid_Item);              
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmProjectIndentPOApproval_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    e.Handled = true;
                    return;
                    if (this.ActiveControl.Name != String.Empty && this.ActiveControl.Name != "TxtIndent" && this.ActiveControl.Name != "TxtRemarks")
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

                
      
        void Calculate_Item()
        {
            try
            {
                for (int i = 0; i < Grid_Item.Rows.Count - 1; i++)
                {
                    if (Grid_Item["PROJ_TYPE_ID", i].Value == null)
                    {
                        return;
                    }
                    if (Grid_Item["PROJ_TYPE_ID", i].Value.ToString() != String.Empty && Grid_Item["PROJ_ACTIVITY_ID", i].Value.ToString() != String.Empty)
                    {
                        if (Grid_Item["CONV_VAL", i].Value == null || Grid_Item["CONV_VAL", i].Value == DBNull.Value || Grid_Item["CONV_VAL", i].Value.ToString() == String.Empty || Convert.ToDouble(Grid_Item["CONV_VAL", i].Value.ToString()) == 0)
                        {
                            Grid_Item["CONV_VAL", i].Value = "1.000";
                        }
                        if (Grid_Item["FREI_TAX_MODE", i].Value.ToString() == String.Empty)
                        {
                            Grid_Item["FREI_TAX_MODE", i].Value = "A";
                        }

                        if (Grid_Item["CALC_TYPE", i].Value.ToString() == "*")
                        {
                            Grid_Item["PO_QTY_ORG_CONV", i].Value = String.Format("{0:0.000}", Math.Round(Convert.ToDouble(Grid_Item["PO_QTY_ORG", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value), 3));

                              Grid_Item["APP_GRS_ORG_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["APP_GRS_ORG", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value) / Convert.ToDouble(Grid_Item["App_Sup_Ex_Rate", i].Value), 4));
                                Grid_Item["APP_FREI_ORG_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["APP_FREI_ORG", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value) / Convert.ToDouble(Grid_Item["App_Sup_Ex_Rate", i].Value), 4));
                                Grid_Item["APP_PUR_RATE_ORG_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["APP_PUR_RATE_ORG", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value) / Convert.ToDouble(Grid_Item["App_Sup_Ex_Rate", i].Value), 4));
                            

                            Grid_Item["BOM", i].Value = String.Format("{0:0.000}", Math.Round(Convert.ToDouble(Grid_Item["BOM_ORG", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value), 3));
                            Grid_Item["ORDERED", i].Value = String.Format("{0:0.000}", Math.Round(Convert.ToDouble(Grid_Item["ORDERED_ORG", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value), 3));
                        
                            Grid_Item["APP_GRS", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["APP_GRS_ORG_CONV", i].Value) * 1, 4));
                            Grid_Item["APP_FREI", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["APP_FREI_ORG_CONV", i].Value) * 1, 4));
                            Grid_Item["APP_PUR_RATE", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["APP_PUR_RATE_ORG_CONV", i].Value) * 1, 4));
                            if (Grid_Item["FREI_TAX_MODE", i].Value.ToString() == "B")
                            {
                                Grid_Item["TAX_RATE", i].Value = String.Format("{0:0.0000}", Math.Round((((Convert.ToDouble(Grid_Item["GRS_RATE", i].Value) + Convert.ToDouble(Grid_Item["FREIGHT_RATE", i].Value)) * Convert.ToDouble(Grid_Item["TAX_PER", i].Value)) / 100), 4));
                            }
                            else
                            {
                                Grid_Item["TAX_RATE", i].Value = String.Format("{0:0.0000}", Math.Round((((Convert.ToDouble(Grid_Item["GRS_RATE", i].Value) + 0) * Convert.ToDouble(Grid_Item["TAX_PER", i].Value)) / 100), 4));
                            }
                            Grid_Item["PUR_RATE", i].Value = String.Format("{0:0.0000}", Math.Round((Convert.ToDouble(Grid_Item["GRS_RATE", i].Value) + Convert.ToDouble(Grid_Item["FREIGHT_RATE", i].Value)) + Convert.ToDouble(Grid_Item["TAX_RATE", i].Value) - Convert.ToDouble(Grid_Item["RO_RATE", i].Value), 4));
                            Grid_Item["GRS_AMT", i].Value = String.Format("{0:0.00}", Math.Round(Convert.ToDouble(Grid_Item["PO_QTY", i].Value) * Convert.ToDouble(Grid_Item["GRS_RATE", i].Value), 2));
                            Grid_Item["TAX_AMT", i].Value = String.Format("{0:0.00}", Math.Round(Convert.ToDouble(Grid_Item["PO_QTY", i].Value) * Convert.ToDouble(Grid_Item["TAX_RATE", i].Value), 2));
                            Grid_Item["FREI_AMT", i].Value = String.Format("{0:0.00}", Math.Round(Convert.ToDouble(Grid_Item["PO_QTY", i].Value) * Convert.ToDouble(Grid_Item["FREIGHT_RATE", i].Value), 2));
                            Grid_Item["PUR_AMT", i].Value = String.Format("{0:0.00}", Math.Round((Convert.ToDouble(Grid_Item["PO_QTY", i].Value) * Convert.ToDouble(Grid_Item["PUR_RATE", i].Value)) - Convert.ToDouble(Grid_Item["RO_AMT", i].Value), 2));

                            Grid_Item["PO_QTY_CONV", i].Value = String.Format("{0:0.000}", Math.Round(Convert.ToDouble(Grid_Item["PO_QTY", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value), 3));
                            Grid_Item["GRS_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["GRS_RATE", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value), 4) * Convert.ToDouble(Grid_Item["App_Sup_Ex_Rate", i].Value));
                            Grid_Item["TAX_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["TAX_RATE", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value), 4) * Convert.ToDouble(Grid_Item["App_Sup_Ex_Rate", i].Value));
                            Grid_Item["FREI_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round((Convert.ToDouble(Grid_Item["FREIGHT_RATE", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value)), 4) * Convert.ToDouble(Grid_Item["App_Sup_Ex_Rate", i].Value));
                            Grid_Item["PUR_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round((Convert.ToDouble(Grid_Item["PUR_RATE", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value)), 4) * Convert.ToDouble(Grid_Item["App_Sup_Ex_Rate", i].Value));

                  
                            Grid_Item["GRS_AMT_INR", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["GRS_RATE_CONV", i].Value) * Convert.ToDouble(Grid_Item["PO_QTY_CONV", i].Value), 2));
                            Grid_Item["TAX_AMT_INR", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["TAX_RATE_CONV", i].Value) * Convert.ToDouble(Grid_Item["PO_QTY_CONV", i].Value), 2));
                            Grid_Item["FREI_AMT_INR", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["FREI_RATE_CONV", i].Value) * Convert.ToDouble(Grid_Item["PO_QTY_CONV", i].Value), 2));
                            Grid_Item["PUR_AMT_INR", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["PUR_RATE_CONV", i].Value) * Convert.ToDouble(Grid_Item["PO_QTY_CONV", i].Value), 2));
                        }
                        else
                        {
                            Grid_Item["PO_QTY_ORG_CONV", i].Value = String.Format("{0:0.000}", Math.Round(Convert.ToDouble(Grid_Item["PO_QTY_ORG", i].Value) / Convert.ToDouble(Grid_Item["CONV_VAL", i].Value), 3));

                              
                                Grid_Item["APP_GRS_ORG_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["APP_GRS_ORG", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value) / Convert.ToDouble(Grid_Item["App_Sup_Ex_Rate", i].Value), 4));
                                Grid_Item["APP_FREI_ORG_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["APP_FREI_ORG", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value) / Convert.ToDouble(Grid_Item["App_Sup_Ex_Rate", i].Value), 4));
                                Grid_Item["APP_PUR_RATE_ORG_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["APP_PUR_RATE_ORG", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value) / Convert.ToDouble(Grid_Item["App_Sup_Ex_Rate", i].Value), 4));
                            

                            Grid_Item["BOM", i].Value = String.Format("{0:0.000}", Math.Round(Convert.ToDouble(Grid_Item["BOM_ORG", i].Value) / Convert.ToDouble(Grid_Item["CONV_VAL", i].Value), 3));
                            Grid_Item["ORDERED", i].Value = String.Format("{0:0.000}", Math.Round(Convert.ToDouble(Grid_Item["ORDERED_ORG", i].Value) / Convert.ToDouble(Grid_Item["CONV_VAL", i].Value), 3));
                            Grid_Item["BAL", i].Value = String.Format("{0:0.000}", Math.Round(Convert.ToDouble(Grid_Item["BAL_ORG", i].Value) / Convert.ToDouble(Grid_Item["CONV_VAL", i].Value), 3));
                       
                            Grid_Item["APP_GRS", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["APP_GRS_ORG_CONV", i].Value) * 1, 4));
                            Grid_Item["APP_FREI", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["APP_FREI_ORG_CONV", i].Value) * 1, 4));
                            Grid_Item["APP_PUR_RATE", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["APP_PUR_RATE_ORG_CONV", i].Value) * 1, 4));

                              if (Grid_Item["FREI_TAX_MODE", i].Value.ToString() == "B")
                            {
                                Grid_Item["TAX_RATE", i].Value = String.Format("{0:0.0000}", Math.Round((((Convert.ToDouble(Grid_Item["GRS_RATE", i].Value) + Convert.ToDouble(Grid_Item["FREIGHT_RATE", i].Value)) * Convert.ToDouble(Grid_Item["TAX_PER", i].Value)) / 100), 4));
                            }
                            else
                            {
                                Grid_Item["TAX_RATE", i].Value = String.Format("{0:0.0000}", Math.Round((((Convert.ToDouble(Grid_Item["GRS_RATE", i].Value) + 0) * Convert.ToDouble(Grid_Item["TAX_PER", i].Value)) / 100), 4));
                            }
                            Grid_Item["PUR_RATE", i].Value = String.Format("{0:0.0000}", Math.Round((Convert.ToDouble(Grid_Item["GRS_RATE", i].Value) + Convert.ToDouble(Grid_Item["FREIGHT_RATE", i].Value)) + Convert.ToDouble(Grid_Item["TAX_RATE", i].Value) - Convert.ToDouble(Grid_Item["RO_RATE", i].Value), 4));
                            Grid_Item["GRS_AMT", i].Value = String.Format("{0:0.00}", Math.Round(Convert.ToDouble(Grid_Item["PO_QTY", i].Value) * Convert.ToDouble(Grid_Item["GRS_RATE", i].Value), 2));
                            Grid_Item["TAX_AMT", i].Value = String.Format("{0:0.00}", Math.Round(Convert.ToDouble(Grid_Item["PO_QTY", i].Value) * Convert.ToDouble(Grid_Item["TAX_RATE", i].Value), 2));
                            Grid_Item["FREI_AMT", i].Value = String.Format("{0:0.00}", Math.Round(Convert.ToDouble(Grid_Item["PO_QTY", i].Value) * Convert.ToDouble(Grid_Item["FREIGHT_RATE", i].Value), 2));
                            Grid_Item["PUR_AMT", i].Value = String.Format("{0:0.00}", Math.Round((Convert.ToDouble(Grid_Item["PO_QTY", i].Value) * Convert.ToDouble(Grid_Item["PUR_RATE", i].Value)) - Convert.ToDouble(Grid_Item["RO_AMT", i].Value), 2));
                            Grid_Item["PO_QTY_CONV", i].Value = String.Format("{0:0.000}", Math.Round(Convert.ToDouble(Grid_Item["PO_QTY", i].Value) * Convert.ToDouble(Grid_Item["CONV_VAL", i].Value), 3));
                            Grid_Item["GRS_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["GRS_RATE", i].Value) / Convert.ToDouble(Grid_Item["CONV_VAL", i].Value), 4) * Convert.ToDouble(Grid_Item["App_Sup_Ex_Rate", i].Value));
                            Grid_Item["TAX_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["TAX_RATE", i].Value) / Convert.ToDouble(Grid_Item["CONV_VAL", i].Value), 4) * Convert.ToDouble(Grid_Item["App_Sup_Ex_Rate", i].Value));
                            Grid_Item["FREI_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round((Convert.ToDouble(Grid_Item["FREIGHT_RATE", i].Value) / Convert.ToDouble(Grid_Item["CONV_VAL", i].Value)), 4) * Convert.ToDouble(Grid_Item["App_Sup_Ex_Rate", i].Value));
                            Grid_Item["PUR_RATE_CONV", i].Value = String.Format("{0:0.0000}", Math.Round((Convert.ToDouble(Grid_Item["PUR_RATE", i].Value) / Convert.ToDouble(Grid_Item["CONV_VAL", i].Value)), 4) * Convert.ToDouble(Grid_Item["App_Sup_Ex_Rate", i].Value));

                      

                            Grid_Item["GRS_AMT_INR", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["GRS_RATE_CONV", i].Value) * Convert.ToDouble(Grid_Item["PO_QTY_CONV", i].Value), 2));
                            Grid_Item["TAX_AMT_INR", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["TAX_RATE_CONV", i].Value) * Convert.ToDouble(Grid_Item["PO_QTY_CONV", i].Value), 2));
                            Grid_Item["FREI_AMT_INR", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["FREI_RATE_CONV", i].Value) * Convert.ToDouble(Grid_Item["PO_QTY_CONV", i].Value), 2));
                            Grid_Item["PUR_AMT_INR", i].Value = String.Format("{0:0.0000}", Math.Round(Convert.ToDouble(Grid_Item["PUR_RATE_CONV", i].Value) * Convert.ToDouble(Grid_Item["PO_QTY_CONV", i].Value), 2));
                        }

                    }
                }
                TxtQTY.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum(ref Grid_Item, "PO_Qty", "Item_ID", "Size_ID", "Color_ID")));
                TxtAmount.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDouble(MyBase.Sum(ref Grid_Item, "Grs_Amt", "Item_ID", "Size_ID", "Color_ID")))));
                TxtTotTax.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDouble(MyBase.Sum(ref Grid_Item, "Tax_Amt", "Item_ID", "Size_ID", "Color_ID")))));
                TxtFreightAmt.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDouble(MyBase.Sum(ref Grid_Item, "Frei_Amt", "Item_ID", "Size_ID", "Color_ID")))));
                TxtTotal.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDouble(MyBase.Sum(ref Grid_Item, "Pur_Amt", "Item_ID", "Size_ID", "Color_ID")))));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        Boolean Calculate_Item_Amount_1()
        {
            try
            {

                for (int i = 0; i < Grid_Item.Rows.Count - 1; i++)
                {
                    if (Grid_Item["PO_Qty", i].Value == null || Grid_Item["PO_Qty", i].Value == DBNull.Value || Grid_Item["PO_Qty", i].Value.ToString() == String.Empty)
                    {
                        Grid_Item["PO_Qty", i].Value = "0.000";
                    }

                    if (Grid_Item["APP_GRS", i].Value == null || Grid_Item["APP_GRS", i].Value == DBNull.Value || Grid_Item["APP_GRS", i].Value.ToString() == String.Empty)
                    {
                        Grid_Item["APP_GRS", i].Value = "0.000";
                    }

              
                    Grid_Item["PO_Qty", i].Value = String.Format("{0:0.000}", Convert.ToDouble(Grid_Item["PO_Qty", i].Value));
                    Grid_Item["Grs_Amount", i].Value = Convert.ToDouble(Grid_Item["PO_Qty", i].Value) * Convert.ToDouble(Grid_Item["Grs_Rate", i].Value);
                    Grid_Item["TAX_AMOUNT", i].Value = ((Convert.ToDouble(Grid_Item["GRS_AMOUNT", i].Value)) * Convert.ToDouble(Grid_Item["TAX_PER", i].Value)) / 100;
                    Grid_Item["FREIGHT_AMOUNT", i].Value = (Convert.ToDouble(Grid_Item["PO_Qty", i].Value) * ((Convert.ToDouble(Grid_Item["FREIGHT_RATE", i].Value))));
                    Grid_Item["PUR_AMOUNT", i].Value = Math.Round(((Convert.ToDouble(Grid_Item["GRS_AMOUNT", i].Value) + Convert.ToDouble(Grid_Item["TAX_AMOUNT", i].Value) + Convert.ToDouble(Grid_Item["FREIGHT_AMOUNT", i].Value))), 0);
                    Grid_Item["PUR_RATE", i].Value = Math.Round(Convert.ToDouble(Grid_Item["Grs_Rate", i].Value) + Math.Round((Convert.ToDouble(Grid_Item["Grs_Rate", i].Value) * Convert.ToDouble(Grid_Item["TAX_PER", i].Value)) / 100.0, 4) + Convert.ToDouble(Grid_Item["FREIGHT_RATE", i].Value), 3);
                    Grid_Item["RO_RATE", i].Value = Math.Round(Convert.ToDouble(Grid_Item["Pur_Rate", i].Value) - Convert.ToDouble(Grid_Item["App_Pur_Rate", i].Value), 2);

                    if (Convert.ToDouble(Grid_Item["Pur_Rate", i].Value) > Convert.ToDouble(Grid_Item["App_Pur_Rate", i].Value))
                    {
                        if ((Convert.ToDouble(Grid_Item["Pur_Rate", i].Value) - Convert.ToDouble(Grid_Item["App_Pur_Rate", i].Value)) >= 0.005)
                        {
                            if (Convert.ToDouble(Grid_Item["Ro_Rate", i].Value) >= 1)
                            {
                                MessageBox.Show("Pur Rate is greater than Approved [" + Grid_Item["App_Pur_Rate", i].Value.ToString() + "] ...!", "Gainup");
                                return false;
                            }
                            else
                            {
                                Grid_Item["PUR_RATE", i].Value = Convert.ToDouble(Grid_Item["APP_PUR_RATE", i].Value);
                            }
                        }
                        else
                        {
                            Grid_Item["PUR_RATE", i].Value = Convert.ToDouble(Grid_Item["APP_PUR_RATE", i].Value);
                        }
                    }
                    Grid_Item["PUR_AMOUNT", i].Value = Convert.ToDouble(Grid_Item["PO_Qty", i].Value) * Convert.ToDouble(Grid_Item["PUR_RATE", i].Value);
                }

                TxtQTY.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum(ref Grid_Item, "PO_Qty", "Item_ID", "Size_ID", "Color_ID")));
                TxtAmount.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid_Item, "Grs_Amount", "Item_ID", "Size_ID", "Color_ID")))));
                TxtTotTax.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid_Item, "Tax_Amount", "Item_ID", "Size_ID", "Color_ID")))));
                TxtFreightAmt.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid_Item, "Freight_Amount", "Item_ID", "Size_ID", "Color_ID")))));
                TxtTotal.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid_Item, "Pur_Amount", "Item_ID", "Size_ID", "Color_ID")))));            
                return true;                
            }
            catch (Exception ex)
            {
                return false;
            }
        }


     
        private void Grid_Item_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Item == null)
                {
                    Txt_Item = (TextBox)e.Control;
                    Txt_Item.KeyDown += new KeyEventHandler(Txt_Item_KeyDown);
                    Txt_Item.KeyPress += new KeyPressEventHandler(Txt_Item_KeyPress);
                    Txt_Item.Leave += new EventHandler(Txt_Item_Leave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Item_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["PO_Qty"].Index)
                {
                    if (Txt_Item.Text.Trim() == String.Empty)
                    {
                        Txt_Item.Text = "0.000";
                    }

                    Txt_Item.Text = String.Format("{0:0.000}", Convert.ToDouble(Txt_Item.Text));                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Item_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                e.Handled = true;
                return;

                if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["PO_Qty"].Index || Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["GRS_RATE"].Index || Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["TAX_PER"].Index || Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["FREIGHT_RATE"].Index)
                {                                        
                        MyBase.Valid_Decimal(Txt_Item, e);                                       
                }
                else if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["RO_AMT"].Index || Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["RO_RATE"].Index)
                {
                    MyBase.Valid_DecimalPlusMinus(Txt_Item, e); 
                }
                else if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["Remarks"].Index)
                {

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

        void Txt_Item_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                DataRow Dr3;
                DataTable TDtT1 = new DataTable();
                if (e.KeyCode == Keys.Down)
                {
                    return;
                    if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["ORDER_NO"].Index)
                    {
                        //if (TxtSupplier.Text.ToString() == String.Empty)
                        //{
                        //    MessageBox.Show("Invalid Supplier", "Gainup");
                        //    TxtSupplier.Focus();
                        //    return;
                        //}
                        e.Handled = true;
                        lblMail.Tag = "1";
                        if (lblMail.Tag.ToString() == "1")
                        {
                            if (TxtBuyer.Text.Trim() == String.Empty)
                            {
                                Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt_Item, SelectionTool_Class.ViewType.NormalView, "Select Item", "sElect OrdeR_NO, Proj_Activity_Name, Proj_Type , ITEM, COLOR, SIZE, UOM, BOM, ORDERED, PO_QTY, GRS_RATE, TAX_PER, FREIGHT_RATE, PUR_RATE, 0.0000 RO_RATE, GRS_AMT, TAX_AMT, FREI_AMT, PUR_AMT, UOMID, ITEM_ID, COLOR_ID, SIZE_ID,  DESCRIPTION, UOM1, UOMID1, 1.000 CONV_VAL, 0.000 BOM_ORG, 0.000 ORDERED_ORG, 0.000 BAL_ORG, 0.000 PO_QTY_ORG, APP_GRS_ORG, 0.0 APP_TAX_ORG, APP_FREI_ORG, APP_PUR_RATE_ORG, 0.000 PO_QTY_ORG_CONV, 0.0000 APP_GRS_ORG_CONV, 0.0000 APP_FREI_ORG_CONV, 0.0000 APP_PUR_RATE_ORG_CONV, 0.000 BAL, 0.0000 APP_GRS, 0.0 APP_TAX, 0.0000 APP_FREI, 0.0000 APP_PUR_RATE, 0.0000 TAX_RATE, TAX_MODE, 0.00 RO_AMT, 0.000 PO_QTY_CONV, 0.0000 GRS_RATE_CONV, 0.0000 TAX_RATE_CONV, 0.0000 FREI_RATE_CONV, 0.0000 PUR_RATE_CONV, 0.00 GRS_AMT_INR, 0.00 TAX_AMT_INR, 0.00 FREI_AMT_INR, 0.00 PUR_AMT_INR, App_Sup_Ex_Rate, RNo, '/' CALC_TYPE, DESC2, Proj_Activity_ID, Proj_Type_ID, ORder_ID, '-' REMARKS  FRom Projects.Dbo.ProjectPO_Indent_Pending_Items_All_Grid_View    ORder by OrdeR_NO, Rno, Proj_Type , Proj_Activity_Name ", String.Empty, 120, 150, 100, 100, 100, 100, 80, 80, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 80);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt_Item, SelectionTool_Class.ViewType.NormalView, "Select Item", "sElect OrdeR_NO, Proj_Activity_Name, Proj_Type , ITEM, COLOR, SIZE, UOM, BOM, ORDERED, PO_QTY, GRS_RATE, TAX_PER, FREIGHT_RATE, PUR_RATE, 0.0000 RO_RATE, GRS_AMT, TAX_AMT, FREI_AMT, PUR_AMT, UOMID, ITEM_ID, COLOR_ID, SIZE_ID,  DESCRIPTION, UOM1, UOMID1, 1.000 CONV_VAL, 0.000 BOM_ORG, 0.000 ORDERED_ORG, 0.000 BAL_ORG, 0.000 PO_QTY_ORG, APP_GRS_ORG, 0.0 APP_TAX_ORG, APP_FREI_ORG, APP_PUR_RATE_ORG, 0.000 PO_QTY_ORG_CONV, 0.0000 APP_GRS_ORG_CONV, 0.0000 APP_FREI_ORG_CONV, 0.0000 APP_PUR_RATE_ORG_CONV, 0.000 BAL, 0.0000 APP_GRS, 0.0 APP_TAX, 0.0000 APP_FREI, 0.0000 APP_PUR_RATE, 0.0000 TAX_RATE, TAX_MODE, 0.00 RO_AMT, 0.000 PO_QTY_CONV, 0.0000 GRS_RATE_CONV, 0.0000 TAX_RATE_CONV, 0.0000 FREI_RATE_CONV, 0.0000 PUR_RATE_CONV, 0.00 GRS_AMT_INR, 0.00 TAX_AMT_INR, 0.00 FREI_AMT_INR, 0.00 PUR_AMT_INR, App_Sup_Ex_Rate, RNo,'/' CALC_TYPE, DESC2, Proj_Activity_ID, Proj_Type_ID, ORder_ID, '-' REMARKS  FRom Projects.Dbo.ProjectPO_Indent_Pending_Items_All_Grid_View  Where Party_Code = " + TxtBuyer.Tag + " ORder by OrdeR_NO, Rno, Proj_Type , Proj_Activity_Name ", String.Empty, 120, 150, 100, 100, 100, 100, 80, 80, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 80);
                            }
                        }
                        else
                        {
                            if (TxtBuyer.Text.Trim() == String.Empty)
                            {
                                Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt_Item, SelectionTool_Class.ViewType.NormalView, "Select Item", "sElect OrdeR_NO,  Proj_Activity_Name, Proj_Type , ITEM, COLOR, SIZE, UOM, BOM, ORDERED, PO_QTY, GRS_RATE, TAX_PER, FREIGHT_RATE, PUR_RATE, 0.0000 RO_RATE, GRS_AMT, TAX_AMT, FREI_AMT, PUR_AMT, UOMID, ITEM_ID, COLOR_ID, SIZE_ID,  DESCRIPTION, UOM1, UOMID1, 1.000 CONV_VAL, 0.000 BOM_ORG, 0.000 ORDERED_ORG, 0.000 BAL_ORG, 0.000 PO_QTY_ORG, APP_GRS_ORG, 0.0 APP_TAX_ORG, APP_FREI_ORG, APP_PUR_RATE_ORG, 0.000 PO_QTY_ORG_CONV, 0.0000 APP_GRS_ORG_CONV, 0.0000 APP_FREI_ORG_CONV, 0.0000 APP_PUR_RATE_ORG_CONV, 0.000 BAL, 0.0000 APP_GRS, 0.0 APP_TAX, 0.0000 APP_FREI, 0.0000 APP_PUR_RATE, 0.0000 TAX_RATE, TAX_MODE, 0.00 RO_AMT, 0.000 PO_QTY_CONV, 0.0000 GRS_RATE_CONV, 0.0000 TAX_RATE_CONV, 0.0000 FREI_RATE_CONV, 0.0000 PUR_RATE_CONV, 0.00 GRS_AMT_INR, 0.00 TAX_AMT_INR, 0.00 FREI_AMT_INR, 0.00 PUR_AMT_INR, App_Sup_Ex_Rate, RNo, '/' CALC_TYPE, DESC2, Proj_Activity_ID, Proj_Type_ID, ORder_ID, '-' REMARKS  FRom Projects.Dbo.ProjectPO_Indent_Pending_Items_All_Grid_View     ORder by OrdeR_NO, Rno, Proj_Type , Proj_Activity_Name ", String.Empty, 120, 150, 100, 100, 100, 100, 80, 80, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 80);
                                //Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt_Item, SelectionTool_Class.ViewType.NormalView, "Select Item", "sElect OrdeR_NO,  Proj_Activity_Name, Proj_Type , ITEM, COLOR, SIZE, RNo, UOM, BOM, ORDERED, PO_QTY, GRS_RATE, TAX_PER, FREIGHT_RATE, PUR_RATE, 0.0000 RO_RATE, GRS_AMT, TAX_AMT, FREI_AMT, PUR_AMT, UOMID, ITEM_ID, COLOR_ID, SIZE_ID,  DESCRIPTION, UOM1, UOMID1, 1.000 CONV_VAL, 0.000 BOM_ORG, 0.000 ORDERED_ORG, 0.000 BAL_ORG, 0.000 PO_QTY_ORG, APP_GRS_ORG, 0.0 APP_TAX_ORG, APP_FREI_ORG, APP_PUR_RATE_ORG, 0.000 PO_QTY_ORG_CONV, 0.0000 APP_GRS_ORG_CONV, 0.0000 APP_FREI_ORG_CONV, 0.0000 APP_PUR_RATE_ORG_CONV, 0.000 BAL, 0.0000 APP_GRS, 0.0 APP_TAX, 0.0000 APP_FREI, 0.0000 APP_PUR_RATE, 0.0000 TAX_RATE, TAX_MODE, 0.00 RO_AMT, 0.000 PO_QTY_CONV, 0.0000 GRS_RATE_CONV, 0.0000 TAX_RATE_CONV, 0.0000 FREI_RATE_CONV, 0.0000 PUR_RATE_CONV, 0.00 GRS_AMT_INR, 0.00 TAX_AMT_INR, 0.00 FREI_AMT_INR, 0.00 PUR_AMT_INR, App_Sup_Ex_Rate, '/' CALC_TYPE, DESC2, Proj_Activity_ID, Proj_Type_ID, ORder_ID, '-' REMARKS  FRom GArmentsPO_Trims_Pending_Items_All_Grid_Fc_View ORder by 1, 2 ", String.Empty, 120, 150, 100, 100, 100, 100, 80, 80, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 80);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt_Item, SelectionTool_Class.ViewType.NormalView, "Select Item", "sElect OrdeR_NO,  Proj_Activity_Name, Proj_Type , ITEM, COLOR, SIZE, RNo, UOM, BOM, ORDERED, PO_QTY, GRS_RATE, TAX_PER, FREIGHT_RATE, PUR_RATE, 0.0000 RO_RATE, GRS_AMT, TAX_AMT, FREI_AMT, PUR_AMT, UOMID, ITEM_ID, COLOR_ID, SIZE_ID,  DESCRIPTION, UOM1, UOMID1, 1.000 CONV_VAL, 0.000 BOM_ORG, 0.000 ORDERED_ORG, 0.000 BAL_ORG, 0.000 PO_QTY_ORG, APP_GRS_ORG, 0.0 APP_TAX_ORG, APP_FREI_ORG, APP_PUR_RATE_ORG, 0.000 PO_QTY_ORG_CONV, 0.0000 APP_GRS_ORG_CONV, 0.0000 APP_FREI_ORG_CONV, 0.0000 APP_PUR_RATE_ORG_CONV, 0.000 BAL, 0.0000 APP_GRS, 0.0 APP_TAX, 0.0000 APP_FREI, 0.0000 APP_PUR_RATE, 0.0000 TAX_RATE, TAX_MODE, 0.00 RO_AMT, 0.000 PO_QTY_CONV, 0.0000 GRS_RATE_CONV, 0.0000 TAX_RATE_CONV, 0.0000 FREI_RATE_CONV, 0.0000 PUR_RATE_CONV, 0.00 GRS_AMT_INR, 0.00 TAX_AMT_INR, 0.00 FREI_AMT_INR, 0.00 PUR_AMT_INR, App_Sup_Ex_Rate, RNo, '/' CALC_TYPE, DESC2, Proj_Activity_ID, Proj_Type_ID, ORder_ID, '-' REMARKS  FRom Projects.Dbo.ProjectPO_Indent_Pending_Items_All_Grid_View    Where Party_Code = " + TxtBuyer.Tag + " ORder by OrdeR_NO, Rno, Proj_Type , Proj_Activity_Name ", String.Empty, 120, 150, 100, 100, 100, 100, 80, 80, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 80);
                                //Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt_Item, SelectionTool_Class.ViewType.NormalView, "Select Item", "sElect OrdeR_NO,  Proj_Activity_Name, Proj_Type , ITEM, COLOR, SIZE, RNo, UOM, BOM, ORDERED, PO_QTY, GRS_RATE, TAX_PER, FREIGHT_RATE, PUR_RATE, 0.0000 RO_RATE, GRS_AMT, TAX_AMT, FREI_AMT, PUR_AMT, UOMID, ITEM_ID, COLOR_ID, SIZE_ID,  DESCRIPTION, UOM1, UOMID1, 1.000 CONV_VAL, 0.000 BOM_ORG, 0.000 ORDERED_ORG, 0.000 BAL_ORG, 0.000 PO_QTY_ORG, APP_GRS_ORG, 0.0 APP_TAX_ORG, APP_FREI_ORG, APP_PUR_RATE_ORG, 0.000 PO_QTY_ORG_CONV, 0.0000 APP_GRS_ORG_CONV, 0.0000 APP_FREI_ORG_CONV, 0.0000 APP_PUR_RATE_ORG_CONV, 0.000 BAL, 0.0000 APP_GRS, 0.0 APP_TAX, 0.0000 APP_FREI, 0.0000 APP_PUR_RATE, 0.0000 TAX_RATE, TAX_MODE, 0.00 RO_AMT, 0.000 PO_QTY_CONV, 0.0000 GRS_RATE_CONV, 0.0000 TAX_RATE_CONV, 0.0000 FREI_RATE_CONV, 0.0000 PUR_RATE_CONV, 0.00 GRS_AMT_INR, 0.00 TAX_AMT_INR, 0.00 FREI_AMT_INR, 0.00 PUR_AMT_INR, App_Sup_Ex_Rate, '/' CALC_TYPE, DESC2, Proj_Activity_ID, Proj_Type_ID, ORder_ID, '-' REMARKS  FRom GArmentsPO_Trims_Pending_Items_All_Grid_Fc_Buyer_View  Where Party_Code = " + TxtBuyer.Tag + " ORder by 1,2 ", String.Empty, 120, 150, 100, 100, 100, 100, 80, 80, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 80);
                            }
                        }

                        if (ChkSize.Checked == false)
                        {
                            if (Dr != null)
                            {
                                MyBase.Row_Number(ref Grid_Item);
                                //if (Convert.ToDouble(Dr["RNO"].ToString()) != Convert.ToDouble(MyBase.Grid_Max_WithCondition(ref Grid_Item, "RNO", "DESC2", Dr["DESC2"].ToString(),"MODEL_CODE")) + 1)
                                //{
                                //    MessageBox.Show("Already Pending PO Available, Choose RNo Less than (" + Dr["RNO"].ToString() + ") ", "Gainup");
                                //    return;
                                //}
                                Grid_Item["PROJ_ACTIVITY_NAME", Grid_Item.CurrentCell.RowIndex].Value = Dr["PROJ_ACTIVITY_NAME"].ToString();
                                Grid_Item["PROJ_TYPE", Grid_Item.CurrentCell.RowIndex].Value = Dr["PROJ_TYPE"].ToString();
                                Grid_Item["PROJ_ACTIVITY_ID", Grid_Item.CurrentCell.RowIndex].Value = Dr["PROJ_ACTIVITY_ID"].ToString();
                                Grid_Item["PROJ_TYPE_ID", Grid_Item.CurrentCell.RowIndex].Value = Dr["PROJ_TYPE_ID"].ToString();
                                Grid_Item["ORDER_ID", Grid_Item.CurrentCell.RowIndex].Value = Dr["ORDER_ID"].ToString();
                                Grid_Item["ORDER_NO", Grid_Item.CurrentCell.RowIndex].Value = Dr["ORDER_NO"].ToString();
                                Grid_Item["ITEM", Grid_Item.CurrentCell.RowIndex].Value = Dr["ITEM"].ToString();
                                Grid_Item["COLOR", Grid_Item.CurrentCell.RowIndex].Value = Dr["COLOR"].ToString();
                                Grid_Item["SIZE", Grid_Item.CurrentCell.RowIndex].Value = Dr["SIZE"].ToString();
                                Grid_Item["UOM", Grid_Item.CurrentCell.RowIndex].Value = Dr["UOM"].ToString();
                                Grid_Item["UOM1", Grid_Item.CurrentCell.RowIndex].Value = Dr["UOM"].ToString();
                                Grid_Item["CONV_VAL", Grid_Item.CurrentCell.RowIndex].Value = Dr["CONV_VAL"].ToString();
                                Grid_Item["CALC_TYPE", Grid_Item.CurrentCell.RowIndex].Value = Dr["CALC_TYPE"].ToString();
                                Grid_Item["BOM_ORG", Grid_Item.CurrentCell.RowIndex].Value = Dr["BOM"].ToString();
                                Grid_Item["ORDERED_ORG", Grid_Item.CurrentCell.RowIndex].Value = Dr["ORDERED"].ToString();
                                Grid_Item["BAL_ORG", Grid_Item.CurrentCell.RowIndex].Value = Dr["PO_QTY"].ToString();
                                Grid_Item["PO_QTY_ORG", Grid_Item.CurrentCell.RowIndex].Value = Dr["PO_QTY"].ToString();
                                Grid_Item["APP_GRS_ORG", Grid_Item.CurrentCell.RowIndex].Value = Dr["GRS_RATE"].ToString();
                                Grid_Item["APP_TAX_ORG", Grid_Item.CurrentCell.RowIndex].Value = Dr["TAX_PER"].ToString();
                                Grid_Item["APP_FREI_ORG", Grid_Item.CurrentCell.RowIndex].Value = Dr["FREIGHT_RATE"].ToString();
                                Grid_Item["APP_PUR_RATE_ORG", Grid_Item.CurrentCell.RowIndex].Value = Dr["PUR_RATE"].ToString();
                                Grid_Item["PO_QTY_ORG_CONV", Grid_Item.CurrentCell.RowIndex].Value = Dr["PO_QTY"].ToString();
                                Grid_Item["APP_GRS_ORG_CONV", Grid_Item.CurrentCell.RowIndex].Value = Dr["GRS_RATE"].ToString();
                                Grid_Item["APP_FREI_ORG_CONV", Grid_Item.CurrentCell.RowIndex].Value = Dr["FREIGHT_RATE"].ToString();
                                Grid_Item["APP_PUR_RATE_ORG_CONV", Grid_Item.CurrentCell.RowIndex].Value = Dr["PUR_RATE"].ToString();
                                Grid_Item["BOM", Grid_Item.CurrentCell.RowIndex].Value = Dr["BOM"].ToString();
                                Grid_Item["ORDERED", Grid_Item.CurrentCell.RowIndex].Value = Dr["ORDERED"].ToString();
                                Grid_Item["BAL", Grid_Item.CurrentCell.RowIndex].Value = Dr["PO_QTY"].ToString();
                                Grid_Item["PO_QTY", Grid_Item.CurrentCell.RowIndex].Value = Dr["PO_QTY"].ToString();
                                Grid_Item["FREIGHT_RATE", Grid_Item.CurrentCell.RowIndex].Value = Dr["FREIGHT_RATE"].ToString();
                                Grid_Item["APP_GRS", Grid_Item.CurrentCell.RowIndex].Value = Dr["GRS_RATE"].ToString();
                                Grid_Item["APP_TAX", Grid_Item.CurrentCell.RowIndex].Value = Dr["TAX_PER"].ToString();
                                Grid_Item["APP_FREI", Grid_Item.CurrentCell.RowIndex].Value = Dr["FREIGHT_RATE"].ToString();
                                Grid_Item["APP_PUR_RATE", Grid_Item.CurrentCell.RowIndex].Value = Dr["PUR_RATE"].ToString();
                                Grid_Item["App_Sup_Ex_Rate", Grid_Item.CurrentCell.RowIndex].Value = "1";                            
                                Grid_Item["GRS_RATE", Grid_Item.CurrentCell.RowIndex].Value = Dr["GRS_RATE"].ToString();
                                Grid_Item["TAX_PER", Grid_Item.CurrentCell.RowIndex].Value = Dr["TAX_PER"].ToString();
                                Grid_Item["TAX_RATE", Grid_Item.CurrentCell.RowIndex].Value = Dr["TAX_RATE"].ToString();
                                Grid_Item["PUR_RATE", Grid_Item.CurrentCell.RowIndex].Value = Dr["PUR_RATE"].ToString();
                                Grid_Item["RO_RATE", Grid_Item.CurrentCell.RowIndex].Value = Dr["RO_RATE"].ToString();
                                Grid_Item["GRS_AMT", Grid_Item.CurrentCell.RowIndex].Value = Dr["GRS_AMT"].ToString();
                                Grid_Item["TAX_AMT", Grid_Item.CurrentCell.RowIndex].Value = Dr["TAX_AMT"].ToString();
                                Grid_Item["FREI_AMT", Grid_Item.CurrentCell.RowIndex].Value = Dr["FREI_AMT"].ToString();
                                Grid_Item["RO_AMT", Grid_Item.CurrentCell.RowIndex].Value = Dr["RO_AMT"].ToString();
                                Grid_Item["PUR_AMT", Grid_Item.CurrentCell.RowIndex].Value = Dr["PUR_AMT"].ToString();
                                Grid_Item["PO_QTY_CONV", Grid_Item.CurrentCell.RowIndex].Value = Dr["PO_QTY"].ToString();
                                Grid_Item["GRS_RATE_CONV", Grid_Item.CurrentCell.RowIndex].Value = Dr["GRS_RATE"].ToString();
                                Grid_Item["TAX_RATE_CONV", Grid_Item.CurrentCell.RowIndex].Value = Dr["TAX_RATE_CONV"].ToString();
                                Grid_Item["FREI_RATE_CONV", Grid_Item.CurrentCell.RowIndex].Value = Dr["FREIGHT_RATE"].ToString();
                                Grid_Item["PUR_RATE_CONV", Grid_Item.CurrentCell.RowIndex].Value = Dr["PUR_RATE"].ToString();
                                Grid_Item["DESCRIPTION", Grid_Item.CurrentCell.RowIndex].Value = Dr["DESCRIPTION"].ToString();
                                Grid_Item["UOMID", Grid_Item.CurrentCell.RowIndex].Value = Dr["UOMID"].ToString();
                                Grid_Item["ITEM_ID", Grid_Item.CurrentCell.RowIndex].Value = Dr["ITEM_ID"].ToString();
                                Grid_Item["COLOR_ID", Grid_Item.CurrentCell.RowIndex].Value = Dr["COLOR_ID"].ToString();
                                Grid_Item["SIZE_ID", Grid_Item.CurrentCell.RowIndex].Value = Dr["SIZE_ID"].ToString();
                                Grid_Item["UOMID1", Grid_Item.CurrentCell.RowIndex].Value = Dr["UOMID"].ToString();
                                Grid_Item["REMARKS", Grid_Item.CurrentCell.RowIndex].Value = Dr["REMARKS"].ToString();
                                Grid_Item["GRS_AMT_INR", Grid_Item.CurrentCell.RowIndex].Value = Dr["GRS_AMT"].ToString();
                                Grid_Item["TAX_AMT_INR", Grid_Item.CurrentCell.RowIndex].Value = Dr["TAX_AMT"].ToString();
                                Grid_Item["FREI_AMT_INR", Grid_Item.CurrentCell.RowIndex].Value = Dr["FREI_AMT"].ToString();
                                Grid_Item["PUR_AMT_INR", Grid_Item.CurrentCell.RowIndex].Value = Dr["PUR_AMT"].ToString();
                                Grid_Item["FREI_TAX_MODE", Grid_Item.CurrentCell.RowIndex].Value = "A";
                                Grid_Item["ORDER_NO", Grid_Item.CurrentCell.RowIndex].Value = Dr["ORDER_NO"].ToString();
                                Grid_Item["DESC2", Grid_Item.CurrentCell.RowIndex].Value = Dr["DESC2"].ToString();
                                Grid_Item["RNO", Grid_Item.CurrentCell.RowIndex].Value = Dr["RNO"].ToString();
                                Txt_Item.Text = Dr["ORDER_NO"].ToString();
                                Calculate_Item();                                
                            }
                        }
                        else
                        {
                        String PO_QTY = Tool.Get_Input(this, SelectionTool_Class.Input_Type.Decimal, "Enter PO QTY" , "PO QTY");

                            if (PO_QTY == null)
                            {
                                return;
                            }


                            //if (lblMail.Tag.ToString() == "1")
                            //{
                                if (TxtBuyer.Text.Trim() == String.Empty)
                                {
                                    MyBase.Load_Data("SELECT 0 SL, OrdeR_NO, Proj_Type, Proj_Activity_Name, ITEM, COLOR, SIZE, UOM, UOM UOM1, CONV_VAL, BOM BOM_ORG, ORDERED ORDERED_ORG, PO_QTY BAL_ORG, PO_QTY PO_QTY_ORG, GRS_RATE APP_GRS_ORG, TAX_PER APP_TAX_ORG, FREIGHT_RATE APP_FREI_ORG,  PUR_RATE APP_PUR_RATE_ORG,  PO_QTY PO_QTY_ORG_CONV, GRS_RATE APP_GRS_ORG_CONV, FREIGHT_RATE APP_FREI_ORG_CONV, PUR_RATE APP_PUR_RATE_ORG_CONV,  BOM, ORDERED, PO_QTY BAL, PO_QTY PO_QTY, GRS_RATE APP_GRS, TAX_PER APP_TAX, FREIGHT_RATE APP_FREI, PUR_RATE APP_PUR_RATE,  GRS_RATE, TAX_PER,  TAX_RATE,  FREIGHT_RATE, 'B' FREI_TAX_MODE,  RO_RATE,  PUR_RATE,  GRS_AMT, TAX_AMT,   FREI_AMT, RO_AMT, PUR_AMT, PO_QTY PO_QTY_CONV, GRS_RATE GRS_RATE_CONV,  TAX_RATE_CONV TAX_RATE_CONV, FREIGHT_RATE FREI_RATE_CONV,PUR_RATE PUR_RATE_CONV,  GRS_AMT GRS_AMT_INR, TAX_AMT TAX_AMT_INR, FREI_AMT FREI_AMT_INR, PUR_AMT PUR_AMT_INR, App_Sup_Ex_Rate, Order_ID, PRoj_Type_ID, Proj_Activity_ID,  DESCRIPTION ,   UOMID, ITEM_ID, COLOR_ID, SIZE_ID, UOMID1, '/' CALC_TYPE, RNO, DESC2, REMARKS  FROM    Projects.Dbo.ProjectPO_Indent_Pending_Items_All_Grid_View  Where PROJ_TYPE_ID = " + Dr["PROJ_TYPE_ID"].ToString() + " and ITem_Id = " + Dr["ITEM_ID"].ToString() + " and color_ID = " + Dr["Color_ID"].ToString() + " and SizE_ID = " + Dr["SIZE_ID"].ToString() + " ORder by RNo ", ref TDtT1);                                    
                                }
                                else
                                {
                                    MyBase.Load_Data("SELECT 0 SL, OrdeR_NO, Proj_Type, Proj_Activity_Name, ITEM, COLOR, SIZE, UOM, UOM UOM1, CONV_VAL, BOM BOM_ORG, ORDERED ORDERED_ORG, PO_QTY BAL_ORG, PO_QTY PO_QTY_ORG, GRS_RATE APP_GRS_ORG, TAX_PER APP_TAX_ORG, FREIGHT_RATE APP_FREI_ORG,  PUR_RATE APP_PUR_RATE_ORG,  PO_QTY PO_QTY_ORG_CONV, GRS_RATE APP_GRS_ORG_CONV, FREIGHT_RATE APP_FREI_ORG_CONV, PUR_RATE APP_PUR_RATE_ORG_CONV,  BOM, ORDERED, PO_QTY BAL, PO_QTY PO_QTY, GRS_RATE APP_GRS, TAX_PER APP_TAX, FREIGHT_RATE APP_FREI, PUR_RATE APP_PUR_RATE,  GRS_RATE, TAX_PER,  TAX_RATE,  FREIGHT_RATE, 'B' FREI_TAX_MODE,  RO_RATE,  PUR_RATE,  GRS_AMT, TAX_AMT,   FREI_AMT, RO_AMT, PUR_AMT, PO_QTY PO_QTY_CONV, GRS_RATE GRS_RATE_CONV,  TAX_RATE_CONV TAX_RATE_CONV, FREIGHT_RATE FREI_RATE_CONV,PUR_RATE PUR_RATE_CONV,  GRS_AMT GRS_AMT_INR, TAX_AMT TAX_AMT_INR, FREI_AMT FREI_AMT_INR, PUR_AMT PUR_AMT_INR, App_Sup_Ex_Rate, Order_ID, PRoj_Type_ID, Proj_Activity_ID,  DESCRIPTION ,   UOMID, ITEM_ID, COLOR_ID, SIZE_ID, UOMID1, '/' CALC_TYPE, RNO, DESC2, REMARKS FROM    Projects.Dbo.ProjectPO_Indent_Pending_Items_All_Grid_View  Where Party_Code = " + TxtBuyer.Tag + " and PROJ_TYPE_ID = " + Dr["PROJ_TYPE_ID"].ToString() + "  and ITem_Id = " + Dr["ITEM_ID"].ToString() + " and color_ID = " + Dr["Color_ID"].ToString() + " and SizE_ID = " + Dr["SIZE_ID"].ToString() + " ORder by RNo ", ref TDtT1);                                    
                                }
                           // }

                                Double POF = 0;
                            for (int p = 0; p <= TDtT1.Rows.Count - 1; p++)
                            {
                                POF = POF + Convert.ToDouble(TDtT1.Rows[p]["PO_QTY"].ToString());
                                if (Convert.ToDouble(PO_QTY) > POF)
                                {
                                    Dr3 = Dt_Item.NewRow();
                                    Dr3 = TDtT1.Rows[p];
                                    Dt_Item.ImportRow(Dr3);
                                }
                                else
                                {
                                    TDtT1.Rows[p]["PO_QTY"] = (Convert.ToDouble(PO_QTY) - (POF - Convert.ToDouble(TDtT1.Rows[p]["PO_QTY"].ToString())));
                                    TDtT1.Rows[p]["PO_QTY_CONV"] = (Convert.ToDouble(PO_QTY) - (POF - Convert.ToDouble(TDtT1.Rows[p]["PO_QTY"].ToString())));
                                    Dr3 = Dt_Item.NewRow();
                                    Dr3 = TDtT1.Rows[p];
                                    Dt_Item.ImportRow(Dr3);
                                    p = TDtT1.Rows.Count;
                                }
                            }
                            Grid_Item.Rows.RemoveAt(Grid_Item.CurrentCell.RowIndex);
                            Grid_Item.RefreshEdit();
                            Grid_Item.Refresh();
                        }
                         

                    }
                    else if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["FREI_TAX_MODE"].Index)
                    {
                        //if (TxtSupplier.Text.ToString() == String.Empty )
                        //{
                        //    MessageBox.Show("Invalid Supplier", "Gainup");
                        //    TxtSupplier.Focus();
                        //    return;
                        //}
                        e.Handled = true;

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select TAX Mode", "Select 'AFTER TAX' A , 'A' A1 Union Select 'BEFORE TAX' A , 'B' A1", String.Empty, 100, 50);
                        if (Dr != null)
                        {
                            Grid_Item["FREI_TAX_MODE", Grid_Item.CurrentCell.RowIndex].Value = Dr["A1"].ToString();
                            Txt_Item.Text = Dr["A1"].ToString();
                        }
                    }
                    else if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["UOM1"].Index)
                    {
                        //if (TxtSupplier.Text.ToString() == String.Empty)
                        //{
                        //    MessageBox.Show("Invalid Supplier", "Gainup");
                        //    TxtSupplier.Focus();
                        //    return;
                        //}
                        e.Handled = true;

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select UOM", "select UOM, Conv, Calc_Type,  UOMID From Projects.Dbo.Project_Uom_Settings_Fn(" + Grid_Item["UOMID", Grid_Item.CurrentCell.RowIndex].Value + ") ", String.Empty, 100, 100, 100, 100);


                        if (Dr != null)
                        {
                            MyBase.Row_Number(ref Grid_Item);

                            Grid_Item["UOM1", Grid_Item.CurrentCell.RowIndex].Value = Dr["UOM"].ToString();
                            Grid_Item["UOMID1", Grid_Item.CurrentCell.RowIndex].Value = Dr["UOMID"].ToString();
                            Grid_Item["CONV_VAL", Grid_Item.CurrentCell.RowIndex].Value = Dr["Conv"].ToString();
                            Grid_Item["Calc_Type", Grid_Item.CurrentCell.RowIndex].Value = Dr["Calc_Type"].ToString();
                            Txt_Item.Text = Dr["UOM"].ToString();
                            Calculate_Item();                                                
                        }
                    }
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void Grid_Item_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                return;
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["PO_Qty"].Index)
                    {
                        if (Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value == null || Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {

                            //MessageBox.Show("Invalid PO Qty ...!", "Gainup");
                            Grid_Item.CurrentCell = Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex];
                            Grid_Item.Focus();
                            Grid_Item.BeginEdit(true);
                        }
                        else
                        {
                            if (Convert.ToDouble(Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid_Item["Bal_Qty", Grid_Item.CurrentCell.RowIndex].Value))
                            {

                                //MessageBox.Show("PO Qty is greater than Balance ...!", "Gainup");
                                //Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value = Grid_Item["Bal_Qty", Grid_Item.CurrentCell.RowIndex].Value;
                                //Grid_Item.CurrentCell = Grid_Item["PO_Qty", Grid.CurrentCell.RowIndex];
                                //Grid_Item.Focus();
                                //Grid_Item.BeginEdit(true);
                            }
                            else
                            {
                                if (!Calculate_Item_Amount_1())
                                {
                                    e.Handled = true;
                                }
                                else
                                {
                                    
                                    //Fill();
                                }
                            }
                        }
                    }
                    else if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["Grs_Rate"].Index)
                    {
                        if (Grid_Item["Grs_Rate", Grid_Item.CurrentCell.RowIndex].Value == null || Grid_Item["Grs_Rate", Grid_Item.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Item["Grs_Rate", Grid_Item.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid_Item["Grs_Rate", Grid_Item.CurrentCell.RowIndex].Value = "0.00";
                        }

                        

                        Calculate_Item_Amount_1();
                    }
                    else if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["Tax_Per"].Index)
                    {
                        if (Grid_Item["Tax_Per", Grid_Item.CurrentCell.RowIndex].Value == null || Grid_Item["Tax_Per", Grid_Item.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Item["Tax_Per", Grid_Item.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid_Item["Tax_Per", Grid_Item.CurrentCell.RowIndex].Value = "0.00";
                        }

                        
                        Calculate_Item_Amount_1();
                    }
                    else if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["Freight_Rate"].Index)
                    {
                        if (Grid_Item["Freight_Rate", Grid_Item.CurrentCell.RowIndex].Value == null || Grid_Item["Freight_Rate", Grid_Item.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Item["Freight_Rate", Grid_Item.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid_Item["Freight_Rate", Grid_Item.CurrentCell.RowIndex].Value = "0.00";
                        }                        
                        Calculate_Item_Amount_1();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Item_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    e.Handled = true;
                    Calculate_Item_Amount_1();
                    TxtRemarks.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        

        private void Grid_Item_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                return;
                MyBase.Grid_Delete(ref Grid_Item, ref Dt_Item, Grid_Item.CurrentCell.RowIndex);
                Calculate_Item_Amount_1();             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        
      

        private void Grid_Item_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Calculate_Item();
                if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["PO_Qty"].Index)
                {
                    if (Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value == null || Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value = "0.000";
                        MessageBox.Show("Invalid PO Qty ...!", "Gainup");
                        Grid_Item.CurrentCell = Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex];
                        Grid_Item.Focus();
                        Grid_Item.BeginEdit(true);
                    }
                    else
                    {
                        if ((Convert.ToDouble(Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid_Item["Bal", Grid_Item.CurrentCell.RowIndex].Value)))
                        {
                            MessageBox.Show("PO Qty is greater than Balance ...!", "Gainup");
                            Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex].Value = Grid_Item["Bal_Qty", Grid_Item.CurrentCell.RowIndex].Value;
                            Grid_Item.CurrentCell = Grid_Item["PO_Qty", Grid_Item.CurrentCell.RowIndex];
                            Grid_Item.Focus();
                            Grid_Item.BeginEdit(true);
                        }
                        else
                        {
                            Calculate_Item();
                           // Fill();
                           
                        }
                    }
                }
                else if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["Grs_Rate"].Index)
                {
                    if (Grid_Item["Grs_Rate", Grid_Item.CurrentCell.RowIndex].Value == null || Grid_Item["Grs_Rate", Grid_Item.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Item["Grs_Rate", Grid_Item.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid_Item["Grs_Rate", Grid_Item.CurrentCell.RowIndex].Value = "0.0000";
                        MessageBox.Show("Invalid Grs Rate ...!", "Gainup");
                        Grid_Item.CurrentCell = Grid_Item["Grs_Rate", Grid_Item.CurrentCell.RowIndex];
                        Grid_Item.Focus();
                        Grid_Item.BeginEdit(true);
                    }
                    else
                    {
                        if ((Convert.ToDouble(Grid_Item["Grs_Rate", Grid_Item.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid_Item["App_Grs", Grid_Item.CurrentCell.RowIndex].Value)) || (Convert.ToDouble(Grid_Item["Grs_Rate_Conv", Grid_Item.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid_Item["App_Grs_org", Grid_Item.CurrentCell.RowIndex].Value)))
                        {
                            MessageBox.Show("Grs Rate is greater than App Grs...!", "Gainup");
                            Grid_Item["Grs_Rate", Grid_Item.CurrentCell.RowIndex].Value = Grid_Item["App_Grs", Grid_Item.CurrentCell.RowIndex].Value;
                            Grid_Item.CurrentCell = Grid_Item["Grs_Rate", Grid_Item.CurrentCell.RowIndex];
                            Grid_Item.Focus();
                            Grid_Item.BeginEdit(true);
                        }                        
                        else
                        {
                            //Calculate_Item();
                        }
                        Calculate_Item();
                    }
                }
                else if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["Tax_Per"].Index)
                {
                    if (Grid_Item["Tax_Per", Grid_Item.CurrentCell.RowIndex].Value == null || Grid_Item["Tax_Per", Grid_Item.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Item["Tax_Per", Grid_Item.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid_Item["Tax_Per", Grid_Item.CurrentCell.RowIndex].Value = "0.0";                        
                    }
                    else
                    {
                        if (Convert.ToDouble(Grid_Item["Tax_Per", Grid_Item.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid_Item["App_Tax", Grid_Item.CurrentCell.RowIndex].Value))
                        {
                            MessageBox.Show(" Tax Per is greater than App Tax...!", "Gainup");
                            //Grid_Item["Tax_Per", Grid_Item.CurrentCell.RowIndex].Value = Grid_Item["App_Tax", Grid_Item.CurrentCell.RowIndex].Value;
                            //Grid_Item.CurrentCell = Grid_Item["Tax_Per", Grid.CurrentCell.RowIndex];
                            //Grid_Item.Focus();
                            //Grid_Item.BeginEdit(true);
                        }
                        else
                        {
                            //Calculate_Item();
                        }
                        Calculate_Item();
                    }
                }
                else if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["Freight_Rate"].Index)
                {
                    if (Grid_Item["Freight_Rate", Grid_Item.CurrentCell.RowIndex].Value == null || Grid_Item["Freight_Rate", Grid_Item.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Item["Freight_Rate", Grid_Item.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid_Item["Freight_Rate", Grid_Item.CurrentCell.RowIndex].Value = "0.0000";                        
                    }
                    else
                    {
                        if ((Convert.ToDouble(Grid_Item["Freight_Rate", Grid_Item.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid_Item["App_Frei", Grid_Item.CurrentCell.RowIndex].Value)) || (Convert.ToDouble(Grid_Item["Frei_Rate_Conv", Grid_Item.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid_Item["App_Frei_Org_Conv", Grid_Item.CurrentCell.RowIndex].Value)))
                        {
                            MessageBox.Show(" Freight Rate is greater than App Frei...!", "Gainup");
                        ////    Grid_Item["Freight_Rate", Grid_Item.CurrentCell.RowIndex].Value = Grid_Item["App_Frei", Grid_Item.CurrentCell.RowIndex].Value;
                        //    Grid_Item.CurrentCell = Grid_Item["Freight_Rate", Grid.CurrentCell.RowIndex];
                        //    Grid_Item.Focus();
                        //    Grid_Item.BeginEdit(true);
                        }                        
                        else
                        {
                            Calculate_Item();
                        }
                        Calculate_Item();
                    }
                }
                else if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["FREI_TAX_MODE"].Index)
                {
                    if (Grid_Item["FREI_TAX_MODE", Grid_Item.CurrentCell.RowIndex].Value == null || Grid_Item["FREI_TAX_MODE", Grid_Item.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Item["FREI_TAX_MODE", Grid_Item.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid_Item["FREI_TAX_MODE", Grid_Item.CurrentCell.RowIndex].Value = "A";
                        MessageBox.Show("Invalid Freight Tax Mode (Choose A - After Tax or B - Before Tax) ...!", "Gainup");
                        Grid_Item.CurrentCell = Grid_Item["FREI_TAX_MODE", Grid_Item.CurrentCell.RowIndex];
                        Grid_Item.Focus();
                        Grid_Item.BeginEdit(true);
                    }
                    else
                    {                        
                            Calculate_Item();                        
                    }
                }
                else if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["RO_Amt"].Index)
                {
                    if (Grid_Item["RO_Amt", Grid_Item.CurrentCell.RowIndex].Value == null || Grid_Item["RO_Amt", Grid_Item.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Item["RO_Amt", Grid_Item.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid_Item["RO_Amt", Grid_Item.CurrentCell.RowIndex].Value = "0.0";                       
                    }
                    else
                    {
                        if ((Convert.ToDouble(Grid_Item["RO_Amt", Grid_Item.CurrentCell.RowIndex].Value) >= Convert.ToDouble("5")) || (Convert.ToDouble(Grid_Item["RO_Amt", Grid_Item.CurrentCell.RowIndex].Value) <= Convert.ToDouble("-5")))
                        {
                            MessageBox.Show(" RO_Amt is greater than 5 or less than -5...!", "Gainup");
                            //Grid_Item["Pur_Rate", Grid_Item.CurrentCell.RowIndex].Value = Grid_Item["App_Pur_Rate", Grid_Item.CurrentCell.RowIndex].Value;
                            Grid_Item.CurrentCell = Grid_Item["RO_Amt", Grid_Item.CurrentCell.RowIndex];
                            Grid_Item.Focus();
                            Grid_Item.BeginEdit(true);
                        }
                        else
                        {
                            Calculate_Item();
                        }
                    }
                }
                else if (Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["Grs_Rate"].Index || Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["Tax_Per"].Index || Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["Freight_Rate"].Index || Grid_Item.CurrentCell.ColumnIndex == Grid_Item.Columns["Ro_Amt"].Index)
                {
                    if (Grid_Item["Pur_Rate", Grid_Item.CurrentCell.RowIndex].Value == null || Grid_Item["Pur_Rate", Grid_Item.CurrentCell.RowIndex].Value == DBNull.Value || Grid_Item["Pur_Rate", Grid_Item.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid_Item["Pur_Rate", Grid_Item.CurrentCell.RowIndex].Value = "0.0";
                        MessageBox.Show("Invalid Pur Rate ...!", "Gainup");
                        Grid_Item.CurrentCell = Grid_Item["Grs_Rate", Grid_Item.CurrentCell.RowIndex];
                        Grid_Item.Focus();
                        Grid_Item.BeginEdit(true);
                    }
                    else
                    {
                        if ((Convert.ToDouble(Grid_Item["Pur_Rate", Grid_Item.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid_Item["App_Pur_Rate", Grid_Item.CurrentCell.RowIndex].Value)) || (Convert.ToDouble(Grid_Item["Pur_Rate_Conv", Grid_Item.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid_Item["App_Pur_Rate_Org", Grid_Item.CurrentCell.RowIndex].Value)))
                        {
                            MessageBox.Show(" Pur Rate is greater than App Pur Rate...!", "Gainup");
                            //Grid_Item["Pur_Rate", Grid_Item.CurrentCell.RowIndex].Value = Grid_Item["App_Pur_Rate", Grid_Item.CurrentCell.RowIndex].Value;
                            Grid_Item.CurrentCell = Grid_Item["Grs_Rate", Grid_Item.CurrentCell.RowIndex];
                            Grid_Item.Focus();
                            Grid_Item.BeginEdit(true);
                        }
                        else
                        {
                            //Calculate_Item();
                        }
                        Calculate_Item();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Grid_Item_OCN_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Calculate_Item_Dtl();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Double Tot = 0;
                for (int i = 0; i < Grid_Item.SelectedCells.Count; i++)
                {
                    if (Grid_Item.SelectedCells[i].Selected == true)
                    {
                        if (Grid_Item.SelectedCells[i].Value is Double || Grid_Item.SelectedCells[i].Value is Int64 || Grid_Item.SelectedCells[i].Value is Decimal || Grid_Item.SelectedCells[i].Value is Int16 || Grid_Item.SelectedCells[i].Value is Int32)
                        {
                            Tot = Tot + Convert.ToDouble(Grid_Item.SelectedCells[i].Value.ToString());
                        }
                    }
                }
                button4.Text = Tot.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }

       

        private void Grid_Item_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Double Tot = 0;
                for (int i = 0; i < Grid_Item.SelectedCells.Count; i++)
                {
                    if (Grid_Item.SelectedCells[i].Selected == true)
                    {
                        if (Grid_Item.SelectedCells[i].Value is Double || Grid_Item.SelectedCells[i].Value is Int64 || Grid_Item.SelectedCells[i].Value is Decimal || Grid_Item.SelectedCells[i].Value is Int16 || Grid_Item.SelectedCells[i].Value is Int32)
                        {
                            Tot = Tot + Convert.ToDouble(Grid_Item.SelectedCells[i].Value.ToString());
                        }
                    }
                }
                button4.Text = Tot.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void Grid_Item_CurrentCellChanged(object sender, EventArgs e)
        {

        }

        private void myTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Grid_Item_OCN_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

        }

        private void Grid_Item_OCN_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Grid_Item_OCN_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {

        }

        private void ButApp_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Sure To Approve ...!", " Approve ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    MyBase.Execute("update Projects.Dbo.Project_PO_INDENT_Master  Set  Approval_Flag = 'T', Approval_Flag1 = 'T', Approval_Time = Getdate(), Approval_System = Host_Name() Where RowID = " + Code);
                    MyBase.Execute(MyParent.EntryLog("PROJECT INDENT APPROVE", "APPROVE", Code.ToString()));
                    MessageBox.Show("Approved", "Gainup");
                    MyBase.Clear(this);
                    TxtPONO.Focus();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        private void ButClr_Click(object sender, EventArgs e)
        {
            try
            {
                
                MyBase.Clear(this);
                TxtPONO.Focus();
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


    }
}