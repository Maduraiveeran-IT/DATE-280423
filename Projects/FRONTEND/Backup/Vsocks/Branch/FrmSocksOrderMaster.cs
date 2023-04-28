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
    public partial class FrmSocksOrderMaster : Form,Entry  
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataGrid dgv = new DataGrid();
        DataRow Dr;        
        Int64 Code;
        Int32 C=0; 
        TextBox Txt = null;        
        TextBox Txt_Img = null;   
        DataTable[] DtImg;
        String[] Queries;
        String Str;             
        public FrmSocksOrderMaster()
        {
            InitializeComponent();
        }
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                ChkCopy.Checked = false;
                DtImg= new DataTable[100];
                DtpODate.Value = MyBase.GetServerDate();
                Grid_Data();
                TxtOcnType.Text = "OCN";
                DataTable TDtb = new DataTable();
                MyBase.Load_Data(" Select OrdStyleType , OrdStyleTypeId  From OrdStyle_type Where OrdStyleTypeId = 6 Order by OrdStyleType ", ref TDtb);
                if(TDtb.Rows.Count >0)
                {
                    TxtType.Text = TDtb.Rows[0]["OrdStyleType"].ToString();                                
                    TxtType.Tag = TDtb.Rows[0]["OrdStyleTypeId"].ToString();
                }     
                if(TxtOcnType.Text.ToString() == "MOQ")
                {
                    DataTable TDt = new DataTable();
                    MyBase.Load_Data("Select Substring(Max(OrdeR_No), 1, 7) A, Substring(Max(OrdeR_No), Len(Max(OrdeR_No))-4, 7) + 1 B , Max(OrdeR_No) Order_No From(Select Max(OrdeR_No) OrdeR_No From Socks_Order_Master Where Prefix = 'MOQ' Union All Select Max(Order_No)  Order_No from buy_ord_mas Where OrdeRType = 'B')A  ", ref TDt);
                    TxtOCNNo.Text  = TDt.Rows[0][0].ToString() + String.Format("{0:00000}", Convert.ToDouble(TDt.Rows[0][1]));
                }
                else
                {
                    DataTable TDt = new DataTable();
                    MyBase.Load_Data("Select Substring(Max(OrdeR_No), 1, 7) A, Substring(Max(OrdeR_No), Len(Max(OrdeR_No))-4, 7) + 1 B , Max(OrdeR_No) Order_No From(Select Max(OrdeR_No) OrdeR_No From Socks_Order_Master Where Prefix = 'OCN' Union All Select Max(Order_No)  Order_No from buy_ord_mas Where OrdeRType = 'S')A ", ref TDt);
                    TxtOCNNo.Text  = TDt.Rows[0][0].ToString() + String.Format("{0:00000}", Convert.ToDouble(TDt.Rows[0][1]));
                }
                DataTable TDt1 = new DataTable();
                MyBase.Load_Data("Select Substring(Max(Work_OrdeR_No), 1, 7) A, Substring(Max(Work_OrdeR_No), Len(Max(Work_OrdeR_No))-4, 7) + 1 B , Max(Work_OrdeR_No) Work_OrdeR_No From(Select Max(Work_OrdeR_No) Work_OrdeR_No From Socks_Order_Master Union All Select Max(Job_Ord_No)  Work_OrdeR_No From job_ord_mas Where Job_Ord_No like '%GUP-WRK%')A ", ref TDt1);
                TxtWorkOrdNo.Text = TDt1.Rows[0][0].ToString() + String.Format("{0:00000}", Convert.ToDouble(TDt1.Rows[0][1]));
                TxtBuyer.Focus();                
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
                ChkCopy.Checked = false;
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Order No - Edit", "Select Distinct A.Order_No, A.OrdeR_Date, A.Work_ORder_No, A.PArty, A.Employee, A.OrderCategory, A.OrdStyleType, A.Currency, A.Ex_Rate, A.PAy_Term, A.Mode_Of_Shipment, A.Total_Qty, A.Total_Buyer_Qty, A.Net_Amount, A.Remarks, A.Address, A.PArty_Code, A.ORdeR_Category_ID, A.Order_Type_ID, A.Currency_ID, A.PAy_Term_ID, A.Ship_Mode_ID, A.Empl_ID, A.RowID, A.Country_Code, A.Prefix  From Socks_Order_Fn() A Left Join Socks_Planning_Master A1 On A.RowID = A1.OrdeR_ID and A.ItemID = A1.Item_ID LEft Join Vaahini_Erp_Gainup.Dbo.Time_ACtion_Plan_Master B On A.ORdeR_NO = B.Order_No Where  A1.RowID IS Null Order by A.Order_No Desc  ", String.Empty, 120, 100, 100, 160, 140, 120, 120, 120, 100, 100, 100, 100, 120);                
                if (Dr != null)
                {
                    Fill_Datas(Dr);                                        
                    Grid.CurrentCell = Grid["PO_NO", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
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
                TxtOCNNo.Text = Dr["Order_No"].ToString();                
                TxtOCNNo.Tag = Dr["RowID"].ToString();
                DtpODate.Value = Convert.ToDateTime(Dr["Order_Date"]);
                TxtWorkOrdNo.Text = Dr["Work_Order_No"].ToString();  
                TxtBuyer.Text = Dr["Party"].ToString();
                TxtAddress.Text = Dr["Address"].ToString();
                TxtAddress.Tag = Dr["Country_Code"].ToString();
                TxtMerch.Text = Dr["Employee"].ToString();                                
                TxtCategory.Text = Dr["OrderCategory"].ToString();
                TxtType.Text = Dr["OrdStyleType"].ToString();
                TxtCurrency.Text = Dr["Currency"].ToString();
                TxtExRate.Text = Dr["Ex_Rate"].ToString();
                TxtPayTerms.Text = Dr["Pay_Term"].ToString();
                TxtShipMode.Text = Dr["Mode_Of_Shipment"].ToString();
                TxtTotalBom.Text = Dr["Total_Qty"].ToString();                
                TxtNetAmount.Text = Dr["Net_Amount"].ToString(); 
                TxtTotOrderQty.Text = Dr["Total_Buyer_Qty"].ToString();   
                TxtRemarks.Text = Dr["Remarks"].ToString();
                TxtBuyer.Tag = Dr["PArty_Code"].ToString();
                TxtCategory.Tag = Dr["ORdeR_Category_ID"].ToString();
                TxtType.Tag = Dr["Order_Type_ID"].ToString();
                TxtCurrency.Tag = Dr["Currency_ID"].ToString();
                TxtPayTerms.Tag = Dr["PAy_Term_ID"].ToString();
                TxtShipMode.Tag = Dr["Ship_Mode_ID"].ToString();
                TxtMerch.Tag = Dr["Empl_ID"].ToString();
                TxtOcnType.Text = Dr["Prefix"].ToString();
                Grid_Data();
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Fill_Datas_Fill(DataRow Dr)
        {
            try
            {
                TxtOCNNo.Tag = Dr["RowID"].ToString();                       
                TxtBuyer.Text = Dr["Party"].ToString();
                TxtAddress.Text = Dr["Address"].ToString();
                TxtAddress.Tag = Dr["Country_Code"].ToString();
                TxtMerch.Text = Dr["Employee"].ToString();                                
                TxtCategory.Text = Dr["OrderCategory"].ToString();
                TxtType.Text = Dr["OrdStyleType"].ToString();
                TxtCurrency.Text = Dr["Currency"].ToString();
                TxtExRate.Text = Dr["Ex_Rate"].ToString();
                TxtPayTerms.Text = Dr["Pay_Term"].ToString();
                TxtShipMode.Text = Dr["Mode_Of_Shipment"].ToString();
                TxtTotalBom.Text = Dr["Total_Qty"].ToString();                
                TxtNetAmount.Text = Dr["Net_Amount"].ToString(); 
                TxtTotOrderQty.Text = Dr["Total_Buyer_Qty"].ToString();   
                TxtRemarks.Text = Dr["Remarks"].ToString();
                TxtBuyer.Tag = Dr["PArty_Code"].ToString();
                TxtCategory.Tag = Dr["ORdeR_Category_ID"].ToString();
                TxtType.Tag = Dr["Order_Type_ID"].ToString();
                TxtCurrency.Tag = Dr["Currency_ID"].ToString();
                TxtPayTerms.Tag = Dr["PAy_Term_ID"].ToString();
                TxtShipMode.Tag = Dr["Ship_Mode_ID"].ToString();
                TxtMerch.Tag = Dr["Empl_ID"].ToString();

                DataTable TDtS1 = new DataTable();
                MyBase.Load_Data(" Select Sample_ID From Socks_OrdeR_Details Where Sample_ID in ( Select Master_ID From  (Select Distinct Master_ID From VFit_Sample_Details A Left Join VFit_Sample_Product_Master A1 On A.Product_ID = A1.RowID  LEft Join Item B On A1.ItemId = B.itemid LEft Join color C On A1.ColorID = C.colorid LEft Join size D On A1.SizeID = D.sizeid Where (B.item like '%ZZZ%' or C.color like '%zzz%' or D.size like '%zzz%')) A)  and MasteR_ID = " + TxtOCNNo.Tag.ToString() + " ", ref TDtS1);
                                    if(TDtS1.Rows.Count >0)
                                    {
                                        MessageBox.Show("Duplicate Colors are Available in this Sample, Kindly Modified or Create New Sample ", "Gainup");
                                        return;
                                    }
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
                Int32 Array_Index = 0;
                Total_Count();               
               
                if (TxtOcnType.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Prefix", "Gainup");
                    TxtOcnType.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if(TxtOcnType.Text == "MOQ")
                {
                    if(TxtBuyer.Tag.ToString() == "5275" || TxtBuyer.Tag.ToString() == "5465")
                    {
                    
                    }
                    else
                    {
                        MessageBox.Show("Invalid Prefix, For This Buyer", "Gainup");
                        TxtOcnType.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                }

                if (TxtShipMode.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Shipment Mode", "Gainup");
                    TxtShipMode.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtMerch.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Merch Name", "Gainup");
                    TxtMerch.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtType.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Order Type", "Gainup");
                    TxtType.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtPayTerms.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Payment Terms", "Gainup");
                    TxtPayTerms.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtRemarks.Text.Trim() == string.Empty)
                {                    
                    TxtRemarks.Text  = "-";
                }

                if (TxtBuyer.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Buyer", "Gainup");
                    TxtBuyer.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtTotalBom.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotalBom.Text) == 0 || TxtNetAmount.Text.Trim() == string.Empty || Convert.ToDouble(TxtNetAmount.Text) == 0)
                {
                    MessageBox.Show("Invalid Order Details ", "Gainup");
                    Grid.CurrentCell = Grid["PARAMETER", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtCurrency.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Currency Details", "Gainup");
                    TxtCurrency.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtExRate.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Exchange Rate", "Gainup");
                    TxtExRate.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                for (int i = 0; i < Grid.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Grid.Columns.Count - 1; j++)
                    {
                        if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty || Grid[j, i].Value.ToString() == "0")
                        {                           
                            MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                            Grid.CurrentCell = Grid[j, i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }                      
                    }
                        Grid["PO_DATE", i].Value= MyBase.Get_Date_Format(Grid["PO_DATE", i].Value.ToString());
                        Grid["SHIP_DATE", i].Value= MyBase.Get_Date_Format(Grid["SHIP_DATE", i].Value.ToString());
                        Grid["DELIVERY_DATE", i].Value= MyBase.Get_Date_Format(Grid["DELIVERY_DATE", i].Value.ToString());

                        if (Convert.ToDouble(Grid["ALLOW_PER", i].Value) <= 0 || Convert.ToDouble(Grid["ALLOW_PER", i].Value) > 8)
                        {
                            MessageBox.Show("Invalid Allowance Qty, Allowance % Must Between 1 To 8 ..!", "Gainup");
                            Grid.CurrentCell = Grid[1, i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }

                        if (Convert.ToDouble(Grid["BOM_QTY", i].Value) <= 0 || Convert.ToDouble(Grid["BUYER_QTY", i].Value) <= 0 || Convert.ToDouble(Grid["CONV_BOM_QTY", i].Value) <= 0 || Convert.ToDouble(Grid["CONV_BUYER_QTY", i].Value) <= 0 || Convert.ToDouble(Grid["ALLOW_PER", i].Value) <= 0 || Convert.ToDouble(Grid["RATE", i].Value) <= 0 || Convert.ToDouble(Grid["AMOUNT", i].Value) <= 0 )
                        {
                            MessageBox.Show("Invalid Qty & Rate..!", "Gainup");
                            Grid.CurrentCell = Grid[1, i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }

                        if (Convert.ToDouble(Grid["BOM_QTY", i].Value) < Convert.ToDouble(Grid["BUYER_QTY", i].Value))
                        {
                            MessageBox.Show("Invalid BOM Qty..!", "Gainup");
                            Grid.CurrentCell = Grid[1, i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }

                        if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(Grid["PO_DATE", i].Value), Convert.ToDateTime(Grid["SHIP_DATE", i].Value)) > 366  && MyParent.UserCode != 1)
                        {
                            MessageBox.Show("Invalid Ship Date (Lead Days Must Less than 366 Days)", "Gainup");
                            Grid["SHIP_DATE", i].Value = Convert.ToDateTime(Grid["PO_DATE", i].Value);
                            Grid.CurrentCell = Grid["SHIP_DATE", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }

                        if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(Grid["PO_DATE", i].Value), Convert.ToDateTime(Grid["SHIP_DATE", i].Value)) < 0 || MyBase.Date_Difference_In_Days(Convert.ToDateTime(Grid["PO_DATE", i].Value), Convert.ToDateTime(Grid["DELIVERY_DATE", i].Value)) < 0 || MyBase.Date_Difference_In_Days(Convert.ToDateTime(Grid["SHIP_DATE", i].Value), Convert.ToDateTime(Grid["DELIVERY_DATE", i].Value)) > 0)
                        {
                            MessageBox.Show("Invalid Ship Date, Delivery Date (Lead Days Must Less than 366 Days)", "Gainup");
                            Grid["SHIP_DATE", i].Value = Convert.ToDateTime(Grid["PO_DATE", i].Value);
                            Grid.CurrentCell = Grid["SHIP_DATE", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }

                        if ((Grid["IMAGE_REQ", i].Value).ToString() == "Y" && (Grid["IMAGE1", i].Value).ToString() == String.Empty)
                        {
                            MessageBox.Show("Invalid Image ..!", "Gainup");
                            Grid.CurrentCell = Grid["IMAGE_REQ", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return; 
                        }
                    Grid["AMOUNT", i].Value = ((Convert.ToDouble(Grid["RATE", i].Value) * Convert.ToDouble(Grid["BUYER_QTY", i].Value)) * Convert.ToDouble(TxtExRate.Text.ToString())) ;                            
                    Grid["CONV_BUYER_QTY", i].Value = ((Convert.ToDouble(Grid["BUYER_QTY", i].Value) * Convert.ToDouble(Grid["CONV_VAL", i].Value)));                            
                    Grid["BOM_QTY", i].Value = ((Convert.ToDouble(Grid["BUYER_QTY", i].Value))) + (((Convert.ToDouble(Grid["BUYER_QTY", i].Value))) * (Convert.ToDouble(Grid["ALLOW_PER", i].Value) / 100));
                    Grid["CONV_BOM_QTY", i].Value = ((Convert.ToDouble(Grid["BOM_QTY", i].Value) * Convert.ToDouble(Grid["CONV_VAL", i].Value)));
                }
               
                Queries = new String[Grid.Rows.Count * 6 + 40];                    
                if(MyParent._New)
                {                   
                    if(TxtOcnType.Text.ToString() == "MOQ")
                    {
                        DataTable TDt = new DataTable();
                        MyBase.Load_Data("Select Substring(Max(OrdeR_No), 1, 7) A, Substring(Max(OrdeR_No), Len(Max(OrdeR_No))-4, 7) + 1 B , Max(OrdeR_No) Order_No From(Select Max(OrdeR_No) OrdeR_No From Socks_Order_Master Where Prefix = 'MOQ'  Union All Select Max(Order_No)  Order_No from buy_ord_mas Where OrdeRType = 'B')A ", ref TDt);
                        TxtOCNNo.Text  = TDt.Rows[0][0].ToString() + String.Format("{0:00000}", Convert.ToDouble(TDt.Rows[0][1]));
                    }
                    else
                    {
                        DataTable TDt = new DataTable();
                        MyBase.Load_Data("Select Substring(Max(OrdeR_No), 1, 7) A, Substring(Max(OrdeR_No), Len(Max(OrdeR_No))-4, 7) + 1 B , Max(OrdeR_No) Order_No From(Select Max(OrdeR_No) OrdeR_No From Socks_Order_Master Where Prefix = 'OCN' Union All Select Max(Order_No)  Order_No from buy_ord_mas Where OrdeRType = 'S')A ", ref TDt);
                        TxtOCNNo.Text  = TDt.Rows[0][0].ToString() + String.Format("{0:00000}", Convert.ToDouble(TDt.Rows[0][1]));
                    }
                    //DataTable TDt = new DataTable();
                    //MyBase.Load_Data("Select Substring(Max(OrdeR_No), 1, 7) A, Substring(Max(OrdeR_No), Len(Max(OrdeR_No))-4, 7) + 1 B , Max(OrdeR_No) Order_No From(Select Max(OrdeR_No) OrdeR_No From Socks_Order_Master Union All Select Max(Order_No)  Order_No from buy_ord_mas)A ", ref TDt);
                    //TxtOCNNo.Text  = TDt.Rows[0][0].ToString() + String.Format("{0:00000}", Convert.ToDouble(TDt.Rows[0][1]));
                    DataTable TDt1 = new DataTable();
                    MyBase.Load_Data("Select Substring(Max(Work_OrdeR_No), 1, 7) A, Substring(Max(Work_OrdeR_No), Len(Max(Work_OrdeR_No))-4, 7) + 1 B , Max(Work_OrdeR_No) Work_OrdeR_No From(Select Max(Work_OrdeR_No) Work_OrdeR_No From Socks_Order_Master Union All Select Max(Job_Ord_No)  Work_OrdeR_No From job_ord_mas Where Job_Ord_No like '%GUP-WRK%')A ", ref TDt1);
                    TxtWorkOrdNo.Text  = TDt1.Rows[0][0].ToString() + String.Format("{0:00000}", Convert.ToDouble(TDt1.Rows[0][1]));

                    if (TxtOCNNo.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Invalid Order No", "Gainup");
                        TxtOCNNo.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    Queries[Array_Index++] = "Insert into Socks_Order_Master (Order_No, Order_Date, Work_OrdeR_No, Party_Code, Order_Category_ID, Order_Type_ID, Currency_ID, Ex_Rate, Pay_Term_ID, Ship_Mode_ID, Total_Qty, Remarks, Empl_ID, Net_Amount, Total_Buyer_Qty, Prefix) Values ('" + TxtOCNNo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}  {0:T}", DtpODate.Value) + "', '" + TxtWorkOrdNo.Text + "', " + TxtBuyer.Tag + ", " + TxtCategory.Tag + ", " + TxtType.Tag + ", " + TxtCurrency.Tag + ", " + Convert.ToDouble(TxtExRate.Text.ToString()) + ", " + TxtPayTerms.Tag + ", " + TxtShipMode.Tag + ", " + Convert.ToDouble(TxtTotalBom.Text.ToString()) + ", '" + TxtRemarks.Text.ToString() + "', " + TxtMerch.Tag + ", " + Convert.ToDouble(TxtNetAmount.Text.ToString()) + ", " + Convert.ToDouble(TxtTotOrderQty.Text.ToString()) + ", '" + TxtOcnType.Text.ToString() + "') ; Select Scope_Identity()";
                }
                else
                {                      
                    Queries[Array_Index++] = "Update Socks_Order_Master Set  Party_Code = " + TxtBuyer.Tag + ", Order_Category_ID = " + TxtCategory.Tag + ",  Order_Type_ID = " + TxtType.Tag + ", Currency_ID =  " + TxtCurrency.Tag  + ", Ex_Rate = " + Convert.ToDouble(TxtExRate.Text.ToString()) + "  , Pay_Term_ID = " + TxtPayTerms.Tag + ", Remarks = '" + TxtRemarks.Text + "', Ship_Mode_ID = " + TxtShipMode.Tag + ", Total_Qty = " + Convert.ToDouble(TxtTotalBom.Text.ToString()) + ", Empl_ID = " + TxtMerch.Tag + ", Net_Amount = " + Convert.ToDouble(TxtNetAmount.Text.ToString()) + ", Total_Buyer_Qty = " + Convert.ToDouble(TxtTotOrderQty.Text.ToString()) + ", Prefix = '" + TxtOcnType.Text.ToString() + "' Where Rowid = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_Order_Details Where Master_id = " + Code;                                       
                    Queries[Array_Index++] = "Delete From  Vaahini_Gainup_Photo.Dbo.Socks_Order_Image Where Master_id = " + Code;                    
                }

                for (int i = 0; i < Grid.Rows.Count - 1; i++)
                {
                    if (Grid["BOM_QTY", i].Value.ToString() != String.Empty && Grid["BOM_QTY", i].Value != DBNull.Value && Grid["CONV_BOM_QTY", i].Value.ToString() != String.Empty && Grid["CONV_BOM_QTY", i].Value != DBNull.Value)
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into Socks_Order_Details (Master_ID, PO_No, Po_Date, Ship_Date, Delivery_Date, Sample_ID, Pack_Type_ID, Rate, Buyer_Qty, Allow_Per, Bom_Qty, Destination_ID, Port_Load_ID, SNo, Conv_Buyer_Qty, Conv_BOM_Qty, Amount, Image_Req, Wash_Req, Party_Code) Values (@@IDENTITY, '" + Grid["PO_NO", i].Value + "',  '" + String.Format("{0:dd-MMM-yyyy}", Grid["PO_DATE", i].Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", Grid["SHIP_DATE", i].Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", Grid["Delivery_Date", i].Value) + "', " + Grid["SAMPLE_ID", i].Value + ", " + Grid["Pack_Type_ID", i].Value + ", " + Convert.ToDouble(Grid["RATE", i].Value) + ", " + Convert.ToDouble(Grid["BUYER_QTY", i].Value) + ", " +  Grid["ALLOW_PER", i].Value + ", " + Convert.ToDouble(Grid["BOM_QTY", i].Value) + ", " + Grid["Destination_Id", i].Value + ", " + Grid["Port_Load_ID", i].Value + ", " + (i + 1) + ", " + Convert.ToDouble(Grid["CONV_BUYER_QTY", i].Value) + ", " + Convert.ToDouble(Grid["CONV_BOM_QTY", i].Value) + ", " + Convert.ToDouble(Grid["AMOUNT", i].Value) + ", '" + Grid["IMAGE_REQ", i].Value + "', '" + Grid["WASH_REQ", i].Value + "', " + TxtBuyer.Tag + ")";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert into Socks_Order_Details (Master_ID, PO_No, Po_Date, Ship_Date, Delivery_Date, Sample_ID, Pack_Type_ID, Rate, Buyer_Qty, Allow_Per, Bom_Qty, Destination_ID, Port_Load_ID, SNo, Conv_Buyer_Qty, Conv_BOM_Qty, Amount, Image_Req, Wash_Req, Party_Code) Values (" + Code + ", '" + Grid["PO_NO", i].Value + "',  '" + String.Format("{0:dd-MMM-yyyy}", Grid["PO_DATE", i].Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", Grid["SHIP_DATE", i].Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", Grid["Delivery_Date", i].Value) + "', " + Grid["SAMPLE_ID", i].Value + ", " + Grid["Pack_Type_ID", i].Value + ", " + Convert.ToDouble(Grid["RATE", i].Value) + ", " + Convert.ToDouble(Grid["BUYER_QTY", i].Value) + ", " + Convert.ToDouble(Grid["ALLOW_PER", i].Value) + ", " + Convert.ToDouble(Grid["BOM_QTY", i].Value) + ", " + Grid["Destination_Id", i].Value + ", " + Grid["Port_Load_ID", i].Value + ", " + (i + 1) + ", " + Convert.ToDouble(Grid["CONV_BUYER_QTY", i].Value) + ", " + Convert.ToDouble(Grid["CONV_BOM_QTY", i].Value) + ", " + Convert.ToDouble(Grid["AMOUNT", i].Value) + ", '" + Grid["IMAGE_REQ", i].Value + "', '" + Grid["WASH_REQ", i].Value + "', " + TxtBuyer.Tag + ")";
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
                
                for (int i = 0; i < Grid.Rows.Count - 1; i++)
                {
                    if (Grid["IMAGE_REQ", i].Value.ToString() == "Y" && Grid["IMAGE1", i].Value.ToString() != String.Empty)
                    {
                        byte[] data = (byte[]) Grid["IMAGE1", i].Value;
                        if (MyParent._New)
                        {
                            DataTable TDt = new DataTable();
                            String Str1 = " Select IDENT_CURRENT('Socks_Order_Master')  Identity_Mas";
                            MyBase.Load_Data(Str1, ref TDt);                            
                            Str = " Insert into VAAHINI_GAINUP_PHOTO.dbo.Socks_Order_Image (Master_ID, Sno, Image1) Values (" + TDt.Rows[0][0] + ", " + Grid["SNO", i].Value + ",  ?)";
                        }
                        else
                        {
                            Str = " Insert into VAAHINI_GAINUP_PHOTO.dbo.Socks_Order_Image (Master_ID, Sno, Image1) Values (" + Code + ", " + Grid["SNO", i].Value + ",  ?)";
                        }
                            MyBase.Cn_Open();
                            MyBase.ODBCCmd = new OdbcCommand();
                            MyBase.ODBCCmd.Connection = MyBase.Cn;
                            MyBase.ODBCCmd.Transaction = MyBase.ODBCTrans;
                            MyBase.ODBCCmd.CommandText = Str;
                            MyBase.ODBCCmd.Parameters.Add("@Photo", OdbcType.Image);
                            MyBase.ODBCCmd.Parameters["@Photo"].Value = data;
                            int Result = MyBase.ODBCCmd.ExecuteNonQuery();
                    }
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
                 ChkCopy.Checked = false;
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Order No - Delete", "Select Distinct A.Order_No, A.OrdeR_Date, A.Work_ORder_No, A.PArty, A.Employee, A.OrderCategory, A.OrdStyleType, A.Currency, A.Ex_Rate, A.PAy_Term, A.Mode_Of_Shipment, A.Total_Qty, A.Total_Buyer_Qty, A.Net_Amount, A.Remarks, A.Address, A.PArty_Code, A.ORdeR_Category_ID, A.Order_Type_ID, A.Currency_ID, A.PAy_Term_ID, A.Ship_Mode_ID, A.Empl_ID, A.RowID, A.Country_Code, A.Prefix  From Socks_Order_Fn() A Left Join Socks_Planning_Master A1 On A.RowID = A1.OrdeR_ID and A.ItemID = A1.Item_ID LEft Join Vaahini_Erp_Gainup.Dbo.Time_ACtion_Plan_Master B On A.ORdeR_NO = B.Order_No Where B.RowID Is Null and A1.RowID IS Null Order by A.Order_No Desc ", String.Empty, 120, 100, 100, 160, 140, 120, 120, 120, 100, 100, 100, 100, 120);                
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
                    MyBase.Run("Delete from Vaahini_Gainup_Photo.Dbo.Socks_Order_Image Where MasteR_ID = " + Code + " ", "Delete from Socks_ORder_Details Where MasteR_ID = " + Code + " " , "Delete from Socks_ORder_Master Where RowID = " + Code);
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
                 ChkCopy.Checked = false;
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Order No - View", "Select Distinct Order_No, OrdeR_Date, Work_ORder_No, PArty, Employee, OrderCategory, OrdStyleType, Currency, Ex_Rate, PAy_Term, Mode_Of_Shipment, Total_Qty, Total_Buyer_Qty, Net_Amount, Remarks, Address, PArty_Code, ORdeR_Category_ID, Order_Type_ID, Currency_ID, PAy_Term_ID, Ship_Mode_ID, Empl_ID, RowID,  Country_Code, Prefix  From Socks_Order_Fn() Order by Order_No Desc  ", String.Empty, 120, 100, 100, 160, 140, 120, 120, 120, 100, 100, 100, 100, 120);                
                if (Dr != null)
                {
                    Fill_Datas(Dr);                   
                }
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
                    if(ChkCopy.Checked == false)
                    {
                        Str = "Select 0 SNO, '' PO_NO, CAst(PO_DATE as Varchar(20)) PO_DATE, Cast(SHIP_DATE as Varchar(20)) SHIP_DATE, Cast(DELIVERY_DATE as Varchar(20)) DELIVERY_DATE , '' SAMPLE_NO, '' IMAN_NO, '' ITEM, '' SIZE, '' COLOR, '' MODEL, 0.0000 RATE, '' PACK_TYPE, 0 BUYER_QTY, 0 CONV_BUYER_QTY, 0 ALLOW_PER, 0 BOM_QTY, 0 CONV_BOM_QTY, 0.00 AMOUNT, 'N' IMAGE_REQ, '' DESTINATION, '' PORTOFLOADING, '' WASH_REQ, 0 CONV_VAL, 0 SAMPLE_ID, 0 PACK_TYPE_ID, 0 DESTINATION_ID, 0 PORT_LOAD_ID, '' SAMPLE_NO1, IMAGE1 From Socks_ORder_Details A LEft Join Vaahini_Gainup_Photo.Dbo.Socks_Order_Image B On A.RowID = B.MAster_ID Where 1 = 2";
                    }
                    else
                    {
                        if(TxtBuyer.Tag.ToString() == "5275")
                        {
                            //(Case When " + TxtAddress.Tag.ToString() + " =  1 Then A.Rate_INR Else A.RATE End)
                            Str = "Select A.SNO, A.PO_NO, Convert(Varchar(20), A.Po_Date, 104) PO_DATE,  Convert(Varchar(20), A.SHIP_DATE, 104) SHIP_DATE, Convert(Varchar(20), A.DELIVERY_DATE, 104) DELIVERY_DATE, A.SAMPLE_NO,  A.IMAN_NO,  A.ITEM,  A.SIZE,  A.COLOR,  A.MODEL, (Case When B.Sample_ID Is Null then A.Rate When " + TxtAddress.Tag.ToString() + " = 1 Then B.Rate_INR Else B.Rate End) RATE, A.PACK_TYPE, A.BUYER_QTY, A.CONV_BUYER_QTY, A.ALLOW_PER, A.BOM_QTY, A.CONV_BOM_QTY, A.AMOUNT, A.IMAGE_REQ, A.DESTINATION, A.PORTOFLOADING, A.WASH_REQ, A.CONV_VAL, A.SAMPLE_ID, A.PACK_TYPE_ID,  A.DESTINATION_ID,  A.PORT_LOAD_ID, (A.PO_NO + A.SAMPLE_NO) SAMPLE_NO1, A.IMAGE1 From Socks_ORder_Fn() A LEft Join Socks_IMANNO_Dtl_Fn() B On A.Sample_ID = B.Sample_ID and A.IMAN_NO = B.IMAN_NO Where A.RowID = " + TxtOCNNo.Tag.ToString() + " Order by A.SNo";
                        }
                        else
                        {
                            Str = "Select SNO, PO_NO, Convert(Varchar(20),Po_Date, 104) PO_DATE,  Convert(Varchar(20),SHIP_DATE, 104) SHIP_DATE, Convert(Varchar(20),DELIVERY_DATE, 104) DELIVERY_DATE, SAMPLE_NO,  IMAN_NO,  ITEM,  SIZE,  COLOR,  MODEL, RATE, PACK_TYPE,  BUYER_QTY, CONV_BUYER_QTY, ALLOW_PER, BOM_QTY, CONV_BOM_QTY, AMOUNT, IMAGE_REQ, DESTINATION, PORTOFLOADING, WASH_REQ, CONV_VAL, SAMPLE_ID, PACK_TYPE_ID,  DESTINATION_ID,  PORT_LOAD_ID, (PO_NO + SAMPLE_NO) SAMPLE_NO1, IMAGE1 From Socks_ORder_Fn() Where RowID = " + TxtOCNNo.Tag.ToString() + " Order by SNo ";
                        }
                    }
                }
                else
                {
                    Str = "Select A.SNO, A.PO_NO, Convert(Varchar(20),A.Po_Date, 104) PO_DATE,  Convert(Varchar(20),A.SHIP_DATE, 104) SHIP_DATE, Convert(Varchar(20), A.DELIVERY_DATE, 104) DELIVERY_DATE, A.SAMPLE_NO,  A.IMAN_NO,  A.ITEM,  A.SIZE,  A.COLOR,  A.MODEL, A.RATE, A.PACK_TYPE, A.BUYER_QTY, A.CONV_BUYER_QTY, A.ALLOW_PER, A.BOM_QTY, A.CONV_BOM_QTY, A.AMOUNT, A.IMAGE_REQ, A.DESTINATION, A.PORTOFLOADING, A.WASH_REQ, A.CONV_VAL, A.SAMPLE_ID, A.PACK_TYPE_ID,  A.DESTINATION_ID,  A.PORT_LOAD_ID, (A.PO_NO + A.SAMPLE_NO) SAMPLE_NO1, A.IMAGE1 From Socks_ORder_Fn() A LEft Join VAAHINI_ERP_GAINUP.dbo.Time_Action_Plan_Master B On A.Order_No = B.Order_No and B.Division_ID = 3  Where A.RowID = " + Code + " Order by A.SNo ";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);                                               
                if(TxtBuyer.Tag.ToString() == "5275")
                {
                    MyBase.ReadOnly_Grid_Without(ref Grid, "PO_NO", "PO_DATE", "DELIVERY_DATE", "SHIP_DATE", "SAMPLE_NO", "BUYER_QTY", "ALLOW_PER", "DESTINATION", "PORTOFLOADING", "PACK_TYPE", "IMAGE_REQ" ,"WASH_REQ");                      
                }               
                else
                {
                    MyBase.ReadOnly_Grid_Without(ref Grid, "PO_NO", "PO_DATE", "DELIVERY_DATE", "SHIP_DATE", "SAMPLE_NO", "RATE", "BUYER_QTY", "ALLOW_PER", "DESTINATION", "PORTOFLOADING", "PACK_TYPE", "IMAGE_REQ", "WASH_REQ");  
                }
                MyBase.Grid_Designing(ref Grid, ref Dt, "SAMPLE_ID", "PACK_TYPE_ID", "DESTINATION_ID", "PORT_LOAD_ID", "CONV_VAL", "IMAGE1", "SAMPLE_NO1");
                MyBase.Grid_Width(ref Grid, 50, 120, 100, 100, 100, 100, 100, 120, 100, 100, 120, 80, 100, 100, 100, 120, 100, 120, 140, 80, 80, 80);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["PO_NO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["PO_DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["SHIP_DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["DELIVERY_DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["IMAN_NO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["SAMPLE_NO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["ITEM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["SIZE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["COLOR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["MODEL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["PACK_TYPE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["BUYER_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["ALLOW_PER"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["BOM_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["CONV_BOM_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["CONV_BUYER_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["DESTINATION"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["PORTOFLOADING"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["IMAGE_REQ"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["WASH_REQ"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                             

                //if (MyParent.Edit == true)
                //{
                //    for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                //    {
                //       // Data_Img(i);
                //        Str = "Select SNo, Image1, Master_ID From Vaahini_Gainup_Photo.Dbo.Socks_Order_Image  WHERE MasteR_ID= " + Code + " Order by SNo";
                //        MyBase.Load_Data(Str, ref DtImg[i]);
                //    }
                //}              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         void Data_Img(Int32 Row)
         {
            try
            {     
                if (DtImg[Row] == null)
                {
                    DtImg[Row] = new DataTable();                    
                    if (MyParent._New)
                    {
                        //Str = "Select SNo, Image1, Master_ID From Vaahini_Gainup_Photo.Dbo.Socks_Order_Image  WHERE 1=2";
                       
                        //DtImg[Row] = GetByteArray(openFileDialog1.FileName);
                       
                        
                    }
                    else
                    {
                       

                        Str = "Select SNo, Image1, Master_ID From Vaahini_Gainup_Photo.Dbo.Socks_Order_Image  WHERE MasteR_ID= " + Code + " Order by SNo";
                        MyBase.Load_Data(Str, ref DtImg[Row]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        private byte[] GetByteArray(String strFileName)
        {            
            System.IO.FileStream fs = new System.IO.FileStream(strFileName, System.IO.FileMode.Open, FileAccess.Read);            
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);            
            byte[] imgbyte = new byte[fs.Length + 1];            
            imgbyte = br.ReadBytes(Convert.ToInt32((fs.Length)));
            br.Close();            
            fs.Close();
            return imgbyte;
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
                    Txt.Leave +=new EventHandler(Txt_Leave);
                    Txt.TextChanged +=new EventHandler(Txt_TextChanged);
                    Txt.GotFocus +=new EventHandler(Txt_GotFocus);
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
                
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_NO"].Index)
                {                    
                    if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value  != null)
                    {                        
                        if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty && Grid.Rows.Count >=2 && Grid.CurrentCell.RowIndex >=1)
                        {
                                        Grid["PO_NO", Grid.CurrentCell.RowIndex].Value= Grid["PO_NO", Grid.CurrentCell.RowIndex-1].Value.ToString();


                        }
                        Txt.Text = Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString();
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_DATE"].Index)
                {                    
                    if(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value  != null)
                    {                        
                        if(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty && Grid.Rows.Count >2 && Grid.CurrentCell.RowIndex >=1)
                        {
                                        Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value= Grid["PO_DATE", Grid.CurrentCell.RowIndex-1].Value.ToString();


                        }
                        Txt.Text = Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString();
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["SHIP_DATE"].Index)
                {                    
                    if(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value  != null)
                    {                        
                        if(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty && Grid.Rows.Count >2 && Grid.CurrentCell.RowIndex >=1)
                        {
                                        Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value= Grid["SHIP_DATE", Grid.CurrentCell.RowIndex-1].Value.ToString();


                        }
                        Txt.Text = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString();
                    }
                }
                 else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DELIVERY_DATE"].Index)
                {                    
                    if(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value  != null)
                    {                        
                        if(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty && Grid.Rows.Count >2 && Grid.CurrentCell.RowIndex >=1)
                        {
                                        Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value= Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex-1].Value.ToString();


                        }
                        Txt.Text = Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString();
                    }
                }
                return;
                Grid.Refresh();
                //else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["SAMPLE_NO"].Index)
                //{
                //    if(Grid["SAMPLE_NO", Grid.CurrentCell.RowIndex].Value  != null)
                //    {                        
                //        if(Grid["SAMPLE_NO", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                //        {
                //              SendKeys.Send("{Down}");                            
                //        }
                //    }
                //}
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BUYER_QTY"].Index)
                {
                    if (Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value == null || Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) == 0)
                    {
                        Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value = "0";
                    }
                    else if (Grid["RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) == 0)
                    {
                        Grid["RATE", Grid.CurrentCell.RowIndex].Value = "0.000";
                    }
                    else
                    {
                        if (Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0)
                        {
                            Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                        else
                        {
                            if (Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                            {
                                 Grid["CONV_BUYER_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Txt.Text) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                                 Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Txt.Text))) + (((Convert.ToDouble(Txt.Text))) * (Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) / 100));
                                 Grid["CONV_BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                                                           
                                 Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value)) * Convert.ToDouble(TxtExRate.Text.ToString())) ;                            
                            }
                        }
                    }                   
                }    
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ALLOW_PER"].Index && Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value == null || Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value == DBNull.Value)
                    {
                        Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value = "0";
                    }
                    else if (Grid["RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) == 0)
                    {
                        Grid["RATE", Grid.CurrentCell.RowIndex].Value = "0.000";
                    }
                    else
                    {
                        if (Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0)
                        {
                            Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                        else
                        {
                            if (Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                            {
                                 Grid["CONV_BUYER_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                                 Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value))) + (((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value))) * (Convert.ToDouble(Txt.Text) / 100));
                                 Grid["CONV_BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                                
                                 Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value)) * Convert.ToDouble(TxtExRate.Text.ToString())) ;                            
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

        void Txt_KeyDown(object sender, KeyEventArgs e)
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
                        if (TxtShipMode.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("Invalid Shipment Mode", "Gainup");
                            TxtShipMode.Focus();                           
                            return;
                        }
                        if (TxtMerch.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("Invalid Merch Name", "Gainup");
                            TxtMerch.Focus();                           
                            return;
                        }
                        if (TxtExRate.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("Invalid Exchange Rate", "Gainup");
                            TxtExRate.Focus();                           
                            return;
                        }
                        if (TxtType.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("Invalid Order Type", "Gainup");
                            TxtType.Focus();                           
                            return;
                        }
                        if (TxtPayTerms.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("Invalid Payment Terms", "Gainup");
                            TxtPayTerms.Focus();                           
                            return;
                        }
                        if (TxtCurrency.Text.Trim() == String.Empty)
                        {
                            MessageBox.Show("Invalid Currency Details", "Gainup");
                            TxtCurrency.Focus();                           
                            return;
                        }
                      
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["SAMPLE_NO"].Index)
                        {
                            if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            {
                                 MessageBox.Show("Invalid PONO & PODATE");
                                 Grid.CurrentCell = Grid["PO_NO", Grid.CurrentCell.RowIndex];
                                 Grid.Focus();
                                 Grid.BeginEdit(true);
                                 e.Handled = true;
                                 return;
                            }
                            else if(TxtBuyer.Tag.ToString() == "5275" || TxtBuyer.Tag.ToString() == "5465")
                            {
                                Dr = Tool.Selection_Tool_Except_New("SAMPLE_NO1", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "SAMPLE_NO", "Select A.SAMPLE_NO, A.IMAN_NO, A.ITEM, A.SIZE, A.COLOR, A.MODEL, (Case When " + TxtAddress.Tag.ToString() + " =  1 Then A.Rate_INR Else A.RATE End) Rate, A.SAMPLE_ID, '" + Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "'  +  '" + Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString() + "' + A.SAMPLE_NO SAMPLE_NO1 FRom Socks_IMANNO_Dtl_Fn() A Where A.Acc_Ledger_Code In (5465, 5275) ORder by A.Sample_No", String.Empty, 120, 120, 140, 120, 120, 140, 100);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool_Except_New("SAMPLE_NO1", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "SAMPLE_NO", "Select A.SAMPLE_NO, A.IMAN_NO, A.ITEM, A.SIZE, A.COLOR, A.MODEL, A.RATE, A.SAMPLE_ID, '" + Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "'  +  '" + Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString() + "' + A.SAMPLE_NO SAMPLE_NO1 FRom Socks_IMANNO_Dtl_Fn() A Where A.Acc_Ledger_Code Not in (5275, 5465) ORder by A.Sample_No", String.Empty, 120, 120, 140, 120, 120, 140, 100);
                            }
                            if (Dr != null)
                            {
                                    DataTable TDtS1 = new DataTable();
                                    MyBase.Load_Data(" Select Master_ID From  (Select Distinct Master_ID From VFit_Sample_Details A Left Join VFit_Sample_Product_Master A1 On A.Product_ID = A1.RowID  LEft Join Item B On A1.ItemId = B.itemid LEft Join color C On A1.ColorID = C.colorid LEft Join size D On A1.SizeID = D.sizeid Where (B.item like '%ZZZ%' or C.color like '%zzz%' or D.size like '%zzz%')) A Where Master_ID = " + Dr["SAMPLE_ID"].ToString() + " ", ref TDtS1);
                                    if(TDtS1.Rows.Count >0)
                                    {
                                        MessageBox.Show("Duplicate Colors are Available in this Sample, Kindly Modified or Create New Sample ", "Gainup");
                                        return;
                                    }                                
                                    if(Dr["ITEM"].ToString() == "-")
                                    {
                                        MessageBox.Show("Invalid Item Type", "Gainup");
                                        return;
                                    }
                                Grid["SAMPLE_NO", Grid.CurrentCell.RowIndex].Value = Dr["SAMPLE_NO"].ToString();
                                Grid["IMAN_NO", Grid.CurrentCell.RowIndex].Value = Dr["IMAN_NO"].ToString();
                                Grid["ITEM", Grid.CurrentCell.RowIndex].Value = Dr["ITEM"].ToString();
                                Grid["SIZE", Grid.CurrentCell.RowIndex].Value = Dr["SIZE"].ToString();
                                Grid["COLOR", Grid.CurrentCell.RowIndex].Value = Dr["COLOR"].ToString();
                                Grid["MODEL", Grid.CurrentCell.RowIndex].Value = Dr["MODEL"].ToString();
                                Grid["RATE", Grid.CurrentCell.RowIndex].Value = Dr["RATE"].ToString();
                                Grid["SAMPLE_ID", Grid.CurrentCell.RowIndex].Value = Dr["SAMPLE_ID"].ToString();
                                Grid["IMAGE_REQ", Grid.CurrentCell.RowIndex].Value = "N";
                                Grid["WASH_REQ", Grid.CurrentCell.RowIndex].Value = "N";
                                Grid["SAMPLE_NO1", Grid.CurrentCell.RowIndex].Value = Dr["SAMPLE_NO1"].ToString();
                                if(Grid.Rows.Count > 2)
                                {
                                    Grid["DESTINATION", Grid.CurrentCell.RowIndex].Value =  Grid["DESTINATION", Grid.CurrentCell.RowIndex -1].Value;
                                    Grid["DESTINATION_ID", Grid.CurrentCell.RowIndex].Value =  Grid["DESTINATION_ID", Grid.CurrentCell.RowIndex -1].Value;
                                    Grid["PORTOFLOADING", Grid.CurrentCell.RowIndex].Value =  Grid["PORTOFLOADING", Grid.CurrentCell.RowIndex -1].Value;
                                    Grid["PORT_LOAD_ID", Grid.CurrentCell.RowIndex].Value =  Grid["PORT_LOAD_ID", Grid.CurrentCell.RowIndex -1].Value;                                    
                                }
                                Txt.Text = Dr["SAMPLE_NO"].ToString();
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DESTINATION"].Index)
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Destination", "Select Country Destination, CountryID Destination_ID From Country Order by Country", String.Empty, 250);
                            if (Dr != null)
                            {
                                Grid["DESTINATION", Grid.CurrentCell.RowIndex].Value = Dr["DESTINATION"].ToString();
                                Grid["DESTINATION_ID", Grid.CurrentCell.RowIndex].Value = Dr["DESTINATION_ID"].ToString();
                                Txt.Text = Dr["DESTINATION"].ToString();
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_NO"].Index)
                        {
                            if(TxtBuyer.Text.ToString() !=  String.Empty)
                            {                   
                                if(TxtBuyer.Tag.ToString() == "5275" || TxtBuyer.Tag.ToString() == "5465")
                                {
                                    Dr = Tool.Selection_Tool_Except_New("PO_NO",this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "PONO", "Select PO_NO, Convert(Varchar(20),Po_Date, 104) PO_DATE, Convert(Varchar(20),SHIP_DATE, 104) SHIP_DATE, Convert(Varchar(20),DELIVERY_DATE, 104) DELIVERY_DATE, SAMPLE_NO,  IMAN_NO,  ITEM,  SIZE,  COLOR,  MODEL,  RATE RATE, PACK_TYPE,  BUYER_QTY, CONV_BUYER_QTY, ALLOW_PER, BOM_QTY, CONV_BOM_QTY, AMOUNT, IMAGE_REQ, DESTINATION, PORTOFLOADING, WASH_REQ,  CONV_VAL, SAMPLE_ID, PACK_TYPE_ID,  DESTINATION_ID,  PORT_LOAD_ID, (PO_NO + Convert(Varchar(20),SHIP_DATE, 104) + SAMPLE_NO) SAMPLE_NO1 From Socks_ORder_Fn() Where Party_Code = " + TxtBuyer.Tag.ToString() + " ORder by DtlID desc ", String.Empty, 120, 100, 100, 100, 140, 140, 100, 120, 140, 120, 120, 120, 120, 100, 140, 140, 120, 140, 140, 80, 120, 120);
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool_Except_New("PO_NO",this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "PONO", "Select PO_NO, Convert(Varchar(20),Po_Date, 104) PO_DATE, Convert(Varchar(20),SHIP_DATE, 104) SHIP_DATE, Convert(Varchar(20),DELIVERY_DATE, 104) DELIVERY_DATE, SAMPLE_NO,  IMAN_NO,  ITEM,  SIZE,  COLOR,  MODEL, RATE, PACK_TYPE,  BUYER_QTY, CONV_BUYER_QTY, ALLOW_PER, BOM_QTY, CONV_BOM_QTY, AMOUNT, IMAGE_REQ, DESTINATION, PORTOFLOADING, WASH_REQ,  CONV_VAL, SAMPLE_ID, PACK_TYPE_ID,  DESTINATION_ID,  PORT_LOAD_ID, (PO_NO + Convert(Varchar(20),SHIP_DATE, 104) + SAMPLE_NO) SAMPLE_NO1 From Socks_ORder_Fn() Where Party_Code Not in (5465, 5275) ORder by DtlID desc ", String.Empty, 120, 100, 100, 100, 140, 140, 100, 120, 140, 120, 120, 120, 120, 100, 140, 140, 120, 140, 140, 80, 120, 120);
                                }
                                if (Dr != null)
                                {
                                    DataTable TDtS1 = new DataTable();
                                    MyBase.Load_Data(" Select Master_ID From  (Select Distinct Master_ID From VFit_Sample_Details A Left Join VFit_Sample_Product_Master A1 On A.Product_ID = A1.RowID  LEft Join Item B On A1.ItemId = B.itemid LEft Join color C On A1.ColorID = C.colorid LEft Join size D On A1.SizeID = D.sizeid Where (B.item like '%ZZZ%' or C.color like '%zzz%' or D.size like '%zzz%')) A Where Master_ID = " + Dr["SAMPLE_ID"].ToString() + " ", ref TDtS1);
                                    if(TDtS1.Rows.Count >0)
                                    {
                                        MessageBox.Show("Duplicate Colors are Available in this Sample, Kindly Modified or Create New Sample ", "Gainup");
                                        return;
                                    }                                
                                    if(Dr["ITEM"].ToString() == "-")
                                    {
                                        MessageBox.Show("Invalid Item Type", "Gainup");
                                        return;
                                    }
                                    Grid["PO_NO", Grid.CurrentCell.RowIndex].Value = Dr["PO_NO"].ToString();
                                    Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value = Dr["PO_DATE"].ToString();
                                    Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value = Dr["SHIP_DATE"].ToString();
                                    Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Dr["DELIVERY_DATE"].ToString();
                                    Grid["SAMPLE_NO", Grid.CurrentCell.RowIndex].Value = Dr["SAMPLE_NO"].ToString();
                                    Grid["IMAN_NO", Grid.CurrentCell.RowIndex].Value = Dr["IMAN_NO"].ToString();
                                    Grid["ITEM", Grid.CurrentCell.RowIndex].Value = Dr["ITEM"].ToString();
                                    Grid["SIZE", Grid.CurrentCell.RowIndex].Value = Dr["SIZE"].ToString();
                                    Grid["COLOR", Grid.CurrentCell.RowIndex].Value = Dr["COLOR"].ToString();
                                    Grid["MODEL", Grid.CurrentCell.RowIndex].Value = Dr["MODEL"].ToString();
                                    Grid["RATE", Grid.CurrentCell.RowIndex].Value = Dr["RATE"].ToString();
                                    Grid["PACK_TYPE", Grid.CurrentCell.RowIndex].Value = Dr["PACK_TYPE"].ToString();
                                    Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value = Dr["BUYER_QTY"].ToString();
                                    Grid["CONV_BUYER_QTY", Grid.CurrentCell.RowIndex].Value = Dr["CONV_BUYER_QTY"].ToString();
                                    Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value = Dr["ALLOW_PER"].ToString();
                                    Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value = Dr["BOM_QTY"].ToString();
                                    Grid["CONV_BOM_QTY", Grid.CurrentCell.RowIndex].Value =  Dr["CONV_BOM_QTY"].ToString();
                                    Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value =  Dr["AMOUNT"].ToString();
                                    Grid["DESTINATION", Grid.CurrentCell.RowIndex].Value =  Dr["DESTINATION"].ToString();
                                    Grid["PORTOFLOADING", Grid.CurrentCell.RowIndex].Value =  Dr["PORTOFLOADING"].ToString();
                                    Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value =  Dr["CONV_VAL"].ToString();
                                    Grid["DESTINATION_ID", Grid.CurrentCell.RowIndex].Value =  Dr["DESTINATION_ID"].ToString();
                                    Grid["PORT_LOAD_ID", Grid.CurrentCell.RowIndex].Value =  Dr["PORT_LOAD_ID"].ToString(); 
                                    Grid["IMAGE_REQ", Grid.CurrentCell.RowIndex].Value =  "N"; 
                                    Grid["WASH_REQ", Grid.CurrentCell.RowIndex].Value =  Dr["WASH_REQ"].ToString();  
                                    Grid["SAMPLE_NO1", Grid.CurrentCell.RowIndex].Value = Dr["SAMPLE_NO1"].ToString();
                                    Grid["SAMPLE_ID", Grid.CurrentCell.RowIndex].Value = Dr["SAMPLE_ID"].ToString();
                                    Grid["PACK_TYPE_ID", Grid.CurrentCell.RowIndex].Value = Dr["PACK_TYPE_ID"].ToString();
                                    Txt.Text = Dr["PO_NO"].ToString();
                                }
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PACK_TYPE"].Index)
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "PACK_TYPE", "Select GUOM PACK_TYPE, Cast(To_BUOM as Int) Conv_Val, GUOMid UOMID from Garment_UOM Where GUOMid Not In (32,37)  ORder by GUOM ", String.Empty, 250, 100);
                            if (Dr != null)
                            {
                                Grid["PACK_TYPE", Grid.CurrentCell.RowIndex].Value = Dr["PACK_TYPE"].ToString();
                                Grid["PACK_TYPE_ID", Grid.CurrentCell.RowIndex].Value = Dr["UOMID"].ToString();
                                Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value = Dr["CONV_VAL"].ToString();
                                Grid["CONV_BOM_QTY", Grid.CurrentCell.RowIndex].Value = 0;
                                Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value = 0;
                                Grid["CONV_BUYER_QTY", Grid.CurrentCell.RowIndex].Value = 0;
                                Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value = 0;
                                Txt.Text = Dr["PACK_TYPE"].ToString();
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PORTOFLOADING"].Index)
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "PORTOFLOADING", "Select PortOfLoading, PortOfLoadingid  From  PortOfLoading Where PortType = 'L' ", String.Empty, 250);
                            if (Dr != null)
                            {
                                Grid["PortOfLoading", Grid.CurrentCell.RowIndex].Value = Dr["PortOfLoading"].ToString();
                                Grid["PORT_LOAD_ID", Grid.CurrentCell.RowIndex].Value = Dr["PortOfLoadingid"].ToString();
                                Txt.Text = Dr["PortOfLoading"].ToString();
                            }
                        }                        
                    }                              
                   else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                   {
                           if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DESTINATION"].Index  || Grid.CurrentCell.ColumnIndex == Grid.Columns["PORTOFLOADING"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["PACK_TYPE"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["SAMPLE_NO"].Index)
                           {
                               e.Handled = true;
                           }
                   }
                   //else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["SAMPLE_NO"].Index)
                   //{
                   //    if(Grid["SAMPLE_NO", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                   //    {
                   //        SendKeys.Send("{Down}");
                   //    }

                   //}
                Total_Count();           
                 
            }
            catch (Exception ex)
            {
                if(ex.Message.Contains("Deleted row information cannot be accessed") == true)
                {
                    Dt.AcceptChanges();
                    Grid.Refresh();
                    Grid.RefreshEdit();
                }
                MessageBox.Show(ex.Message);
            }
        }

       
        void Total_Count()
        {               
            try
            {                               
                TxtTotOrderQty.Text = MyBase.Sum(ref Grid, "BUYER_QTY", "SAMPLE_ID");
                TxtTotalBom.Text = MyBase.Sum(ref Grid, "BOM_QTY", "SAMPLE_ID");
                TxtNetAmount.Text = MyBase.Sum(ref Grid, "AMOUNT", "PO_NO");
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["SHIP_DATE"].Index)
                {
                    if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        MessageBox.Show("Invalid PONO & PODATE");
                        Grid.CurrentCell = Grid["PO_NO", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                         MyBase.Valid_Date(Txt, e);
                    }
                }                
               
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ALLOW_PER"].Index)
                {
                    if(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToInt32(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value.ToString()) == 0)
                    {
                        MessageBox.Show("Invalid BUYER QTY");
                        Grid.CurrentCell = Grid["BUYER_QTY", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                         MyBase.Valid_Number(Txt, e);
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BUYER_QTY"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["ALLOW_PER"].Index)
                {
                    if(Grid["RATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Grid["SAMPLE_NO", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Grid["PACK_TYPE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        MessageBox.Show("Invalid PACK TYPE, SAMPLE_NO NO & RATE");
                        Grid.CurrentCell = Grid["RATE", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                        MyBase.Valid_Number(Txt, e);
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["RATE"].Index)
                {
                    if(TxtExRate.Text.ToString() == String.Empty)
                    {
                        MessageBox.Show("Invalid Exchange Rate");
                        TxtExRate.Focus();
                        return;
                    }
                    else if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Grid["SAMPLE_NO", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        MessageBox.Show("Invalid SAMPLE_NO  & PONO");
                        Grid.CurrentCell = Grid["SAMPLE_NO", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        e.Handled = true;
                        return;
                    }
                    else if(TxtBuyer.Tag.ToString() == "5275")
                    {
                        MyBase.Valid_Null(Txt, e);
                    }
                    else
                    {
                        MyBase.Valid_Decimal(Txt, e);
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_DATE"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["SHIP_DATE"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["DELIVERY_DATE"].Index)
                {
                    MyBase.Valid_Date(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_NO"].Index)
                {
                    MyBase.Return_Ucase(e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["IMAGE_REQ"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["WASH_REQ"].Index)
                {
                    if(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToInt32(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value.ToString()) == 0)
                    {
                        MessageBox.Show("Invalid BUYER QTY");
                        Grid.CurrentCell = Grid["BUYER_QTY", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        e.Handled = true;
                        return;
                    }
                    else 
                    {
                        MyBase.Valid_Yes_OR_No(Txt, e);
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
                
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_NO"].Index && Txt.Text.ToString() != String.Empty)
                {                       
                       Grid["PO_NO", Grid.CurrentCell.RowIndex].Value = Txt.Text.ToString();
                       if(Grid.Rows.Count >1 && Grid.CurrentCell.RowIndex >0 && Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                       {
                            if(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value == null)
                            {
                                Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value =  Grid["PO_DATE", Grid.CurrentCell.RowIndex -1].Value;
                            }
                            if(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value == null)
                            {
                                Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value =  Grid["SHIP_DATE", Grid.CurrentCell.RowIndex -1].Value;
                            }
                            if(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value == null)
                            {
                                Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value =  Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex -1].Value;
                            }
                            if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value == null)
                            {
                                Grid["PO_NO", Grid.CurrentCell.RowIndex].Value =  Grid["PO_NO", Grid.CurrentCell.RowIndex -1].Value;  
                            }
                            if(Grid["DESTINATION", Grid.CurrentCell.RowIndex].Value == null)
                            {
                                Grid["DESTINATION", Grid.CurrentCell.RowIndex].Value =  Grid["DESTINATION", Grid.CurrentCell.RowIndex -1].Value;
                                Grid["DESTINATION_ID", Grid.CurrentCell.RowIndex].Value =  Grid["DESTINATION_ID", Grid.CurrentCell.RowIndex -1].Value;
                            }
                            if(Grid["PORTOFLOADING", Grid.CurrentCell.RowIndex].Value == null)
                            {
                                Grid["PORTOFLOADING", Grid.CurrentCell.RowIndex].Value =  Grid["PORTOFLOADING", Grid.CurrentCell.RowIndex -1].Value;
                                Grid["PORT_LOAD_ID", Grid.CurrentCell.RowIndex].Value =  Grid["PORT_LOAD_ID", Grid.CurrentCell.RowIndex -1].Value;
                            }   
                       }
                        Grid["PO_NO", Grid.CurrentCell.RowIndex].Value = Txt.Text;
                }               
                    return;
                //else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_DATE"].Index)
                //{
                //        if(Txt.Text.ToString() != String.Empty)
                //        {                     
                //            Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value= MyBase.Get_Date_Format(Txt.Text.ToString()); 
                //            Txt.Text = Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString();
                   
                //                if (Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value) > Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", DtpODate.Value)))
                //                {
                //                    //MessageBox.Show("Invalid PO_DATE", "Gainup");
                //                    //Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", DtpODate.Value));
                //                    //Grid.CurrentCell = Grid["PO_DATE", Grid.CurrentCell.RowIndex];
                //                    //Grid.Focus();
                //                    //Grid.BeginEdit(true);
                //                    //return;
                //                }
                //                else if(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == null)
                //                {
                //                    Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value =  Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value;
                //                }
                //        }                       
                //}
                //else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["SHIP_DATE"].Index)                
                //{
                //        if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                //        {
                //            if(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                //            {
                //                Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value= MyBase.Get_Date_Format(Txt.Text.ToString());
                //                Txt.Text = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString();

                //                if (Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value) > Convert.ToDateTime(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value))
                //                {
                //                    //MessageBox.Show("Invalid Date", "Gainup");
                //                    //Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value);
                //                    //Grid.CurrentCell = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex];
                //                    //Grid.Focus();
                //                    //Grid.BeginEdit(true);
                //                    //return;
                //                }
                //                else if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value), Convert.ToDateTime(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value)) > 366  && MyParent.UserCode != 1)
                //                {
                //                    //MessageBox.Show("Invalid Ship Date (Lead Days Must Less than 366 Days)", "Gainup");
                //                    //Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value);
                //                    //Grid.CurrentCell = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex];
                //                    //Grid.Focus();
                //                    //Grid.BeginEdit(true);
                //                    //return;
                //                }
                //                else if(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                //                {
                //                    if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value), Convert.ToDateTime(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value)) > 7)
                //                    {
                //                        Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value).AddDays(-7);
                //                        return;
                //                    }
                //                    else
                //                    {
                //                        Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value);
                //                        return;
                //                    }
                //                }
                //                else if(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                //                {
                //                    if (Convert.ToDateTime(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value) > Convert.ToDateTime(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value))
                //                    {                                        
                //                        Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value);
                //                        return;
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value = Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value;
                //                    //MessageBox.Show("Invalid ShipDate", "Gainup");
                //                    //Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value);
                //                    //Grid.CurrentCell = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex];
                //                    //Grid.Focus();
                //                    //Grid.BeginEdit(true);
                //                    //return;
                //            }
                //        }
                //        else
                //        {                        
                //            Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value;
                //        }
                //}
                //else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DELIVERY_DATE"].Index)
                //{
                //    if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                //        {
                //            if(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                //            {
                //                Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value= MyBase.Get_Date_Format(Txt.Text.ToString());
                //                Txt.Text = Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString();
                //                if (Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value) > Convert.ToDateTime(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value))
                //                {
                //                    //MessageBox.Show("Invalid Date", "Gainup");
                //                    //Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value);
                //                    //Grid.CurrentCell = Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex];
                //                    //Grid.Focus();
                //                    //Grid.BeginEdit(true);
                //                    //return;
                //                }
                //                else if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value), Convert.ToDateTime(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value)) > 366  && MyParent.UserCode != 1)
                //                {
                //                    //MessageBox.Show("Invalid Delivery Date (Lead Days Must Less than 366 Days)", "Gainup");
                //                    //Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value);
                //                    //Grid.CurrentCell = Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex];
                //                    //Grid.Focus();
                //                    //Grid.BeginEdit(true);
                //                    //return;
                //                }
                //            }
                //            else
                //            {
                //                Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value;                            
                //                    //MessageBox.Show("Invalid DeliveryDate", "Gainup");
                //                    //Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value);
                //                    //Grid.CurrentCell = Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex];
                //                    //Grid.Focus();
                //                    //Grid.BeginEdit(true);
                //                    //return;
                //            }
                //        }
                //        else
                //        {                        
                //            Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value;                            
                //        }
                //}
                //else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BUYER_QTY"].Index && Txt.Text.ToString() != String.Empty )
                //{
                //    Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt.Text.ToString());
                //    if(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                //     {                               
                //         //MessageBox.Show("Invalid BUYER QTY", "Gainup");
                //         //Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value = 0;
                //         //Grid["CONV_BUYER_QTY", Grid.CurrentCell.RowIndex].Value = 0;
                //         //Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value = 0;
                //         //Grid["CONV_BOM_QTY", Grid.CurrentCell.RowIndex].Value = 0;
                //         //Grid.CurrentCell = Grid["BUYER_QTY", Grid.CurrentCell.RowIndex];
                //         //Grid.Focus();
                //         //Grid.BeginEdit(true);
                //         //return;                             
                //     }
                //     else
                //     {
                //        if(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                //        {
                //            Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value = "0";
                //        }
                //        if(Grid["RATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                //        {
                //            Grid["RATE", Grid.CurrentCell.RowIndex].Value = "0.000";
                //        }
                //         Grid["CONV_BUYER_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                //         Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value))) + (((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value))) * (Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) / 100));
                //         Grid["CONV_BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                //         Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value)) * Convert.ToDouble(TxtExRate.Text.ToString())) ;                            
                //     }
                //}
                //else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ALLOW_PER"].Index  && Txt.Text.ToString() != String.Empty)
                //{
                //    Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt.Text.ToString());
                //    if(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                //     {                               
                //         //MessageBox.Show("Invalid ALLOWANCE %", "Gainup");
                //         //Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value = 0;                         
                //         //Grid.CurrentCell = Grid["ALLOW_PER", Grid.CurrentCell.RowIndex];
                //         //Grid.Focus();
                //         //Grid.BeginEdit(true);
                //         //return;                             
                //     }
                //        else if (Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) <= 0 || Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) > 8)
                //        {
                //            //MessageBox.Show("Invalid Allowance Qty, Allowance % Must Between 1 To 8 ..!", "Gainup");
                //            //Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value = 0;   
                //            //Grid.CurrentCell = Grid["ALLOW_PER", Grid.CurrentCell.RowIndex];
                //            //Grid.Focus();
                //            //Grid.BeginEdit(true);                            
                //            //return;
                //        }
                //    else if(Grid["RATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                //     {
                //            Grid["RATE", Grid.CurrentCell.RowIndex].Value = "0.000";
                //     }
                //     else
                //     {
                //         Grid["CONV_BUYER_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                //         Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value))) + (((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value))) * (Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) / 100));
                //         Grid["CONV_BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                //         Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value)) * Convert.ToDouble(TxtExRate.Text.ToString())) ;                            
                //     }
                //}
                //else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["RATE"].Index && Txt.Text.ToString() != String.Empty)
                //{
                //    Grid["RATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Txt.Text.ToString());
                //    if(Grid["RATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || TxtExRate.Text.ToString() == String.Empty)
                //     { 
                //         //MessageBox.Show("Invalid RATE", "Gainup");
                //         ////Grid["RATE", Grid.CurrentCell.RowIndex].Value = 0.0000; 
                //         //Grid.CurrentCell = Grid["RATE", Grid.CurrentCell.RowIndex];
                //         //Grid.Focus();
                //         //Grid.BeginEdit(true);
                //         //return;                             
                //     }                     
                //}                
                //if (Grid["PO_NO", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["SAMPLE_ID", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["SAMPLE_ID", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                //{
                //    if (Grid.Rows.Count > 2)
                //    {
                //        for (int k = 0; k < Grid.Rows.Count - 2; k++)
                //        {
                //            if (( Grid["PO_NO", k].Value.ToString()) == Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() && (Grid["SAMPLE_ID", k].Value.ToString()) == (Grid["SAMPLE_ID", Grid.CurrentCell.RowIndex].Value.ToString()))
                //            {
                //                MessageBox.Show("Already PONO & IMAN NO is Available", "Gainup");
                //                Grid["PO_NO", Grid.CurrentCell.RowIndex].Value = "";
                //                Grid["IMAN_NO", Grid.CurrentCell.RowIndex].Value = "";
                //                Grid["RATE", Grid.CurrentCell.RowIndex].Value = "0.0000";                                
                //                k=Grid.Rows.Count ;
                //                Total_Count();                                
                //                return;
                //            }
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

        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Total_Count();
                    TxtNetAmount.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void FrmSocksOrderMaster_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                //MyBase.Disable_Cut_Copy(GBMain);    
                Disable();
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
         void Disable()
        {
            try
            {
                foreach (Control Ct in GBMain.Controls)
                {
                    if (Ct is System.Windows.Forms.TextBox)
                    {
                        if (Ct.Name != "Txt")
                        {                            
                            Ct.ContextMenu = new ContextMenu();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmSocksOrderMaster_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {                 
                if (this.ActiveControl.Name == "TxtExRate")
                {
                    if(TxtCurrency.Tag.ToString() != "25" && (Grid.Rows.Count <=1 || MyParent.UserCode ==12))
                    {
                        MyBase.Valid_Decimal((TextBox)this.ActiveControl, e);
                    }                   
                    else
                    {
                        MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                    }
                }
                else if (this.ActiveControl.Name != String.Empty && this.ActiveControl.Name != "TxtRemarks")
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
            }
        }

        private void FrmSocksOrderMaster_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtNetAmount")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtShipMode")
                    {
                        Grid.CurrentCell = Grid["PO_NO", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        e.Handled = true;
                        return;         
                    }                  
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Down)
                {                    
                    if (MyParent._New == true || MyParent.Edit == true)
                    {
                        if (this.ActiveControl.Name == "TxtBuyer")
                        {                            
                            if (Grid.Rows.Count <=1 || MyParent.UserCode ==1)
                            {  
                               if(ChkCopy.Checked == true && MyParent._New == true)
                               {
                                    Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Order No ", "Select Distinct A.Order_No, A.OrdeR_Date, A.Work_ORder_No, A.PArty, A.Employee, A.OrderCategory, A.OrdStyleType, A.Currency, A.Ex_Rate, A.PAy_Term, A.Mode_Of_Shipment, A.Total_Qty, A.Total_Buyer_Qty, A.Net_Amount, A.Remarks, A.Address, A.PArty_Code, A.ORdeR_Category_ID, A.Order_Type_ID, A.Currency_ID, A.PAy_Term_ID, A.Ship_Mode_ID, A.Empl_ID, A.RowID, IsNull(A.Country_Code,1) Country_Code  From Socks_Order_Fn() A LEft Join Socks_Planning_Master B On A.RowID = B.Order_ID  Order by A.Order_No Desc  ", String.Empty, 120, 100, 100, 160, 140, 120, 120, 120, 100, 100, 100, 100, 120);                
                                    if (Dr != null)
                                    {
                                        Fill_Datas_Fill(Dr);                                        
                                        Grid.CurrentCell = Grid["PO_NO", 0];
                                        Grid.Focus();
                                        Grid.BeginEdit(true);
                                    }   
                               }
                               else
                               {
                                   if(TxtOcnType.Text.ToString() == "MOQ")
                                   {
                                           Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer", "Select B.Ledger_Name Name, B.Ledger_Address Address, B.LEdger_Code RowID, IsNull(B.Country_Code,1) Country_Code  From  FitSocks.Dbo.Buyer A Inner Join ACCOUNTS.dbo.Ledger_Master B On A.Acc_Ledger_Code = B.Ledger_Code and Company_Code = 1  and B.YEAR_CODE = VAAHINI_ERP_GAINUP.dbo.Get_Accounts_YearCode(Getdate()) Where B.Ledger_Code in (5275, 5465) Order by B.Ledger_Name ", String.Empty, 600);
                                   }
                                   else
                                   {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer", "Select B.Ledger_Name Name, B.Ledger_Address Address, B.LEdger_Code RowID, IsNull(B.Country_Code,1) Country_Code  From  FitSocks.Dbo.Buyer A Inner Join ACCOUNTS.dbo.Ledger_Master B On A.Acc_Ledger_Code = B.Ledger_Code and Company_Code = 1  and B.YEAR_CODE = VAAHINI_ERP_GAINUP.dbo.Get_Accounts_YearCode(Getdate()) Order by B.Ledger_Name ", String.Empty, 600);
                                   }
                                        if (Dr != null)
                                        {
                                            TxtBuyer.Text = Dr["Name"].ToString();
                                            TxtBuyer.Tag = Dr["RowID"].ToString();
                                            TxtAddress.Text = Dr["Address"].ToString();
                                            TxtAddress.Tag =  Dr["Country_Code"].ToString();
                                            TxtCurrency.Text = "";
                                            TxtCategory.Text = "";
                                            TxtExRate.Text = "0";
                                            DataTable TDtb = new DataTable();
                                            MyBase.Load_Data("Select Distinct Employee, OrderCategory, OrdStyleType, Currency, Ex_Rate, Pay_Term, Mode_of_Shipment, Order_Category_ID, Order_Type_ID, Currency_ID, Pay_Term_ID, Ship_Mode_ID, Empl_ID FRom Socks_Order_Fn() Where Party_Code = " + TxtBuyer.Tag + " and Order_Date = (Select Max(Order_Date) From Socks_Order_Master Where Party_Code = " + TxtBuyer.Tag + ") ", ref TDtb);
                                            if(TDtb.Rows.Count >0)
                                            {
                                                    TxtMerch.Text = TDtb.Rows[0]["Employee"].ToString();                                
                                                    TxtCategory.Text = TDtb.Rows[0]["OrderCategory"].ToString();
                                                    TxtType.Text = TDtb.Rows[0]["OrdStyleType"].ToString();
                                                    TxtCurrency.Text = TDtb.Rows[0]["Currency"].ToString();
                                                    TxtExRate.Text = TDtb.Rows[0]["Ex_Rate"].ToString();
                                                    TxtPayTerms.Text = TDtb.Rows[0]["PAy_Term"].ToString();
                                                    TxtShipMode.Text = TDtb.Rows[0]["Mode_Of_Shipment"].ToString();
                                                    TxtCategory.Tag = TDtb.Rows[0]["ORdeR_Category_ID"].ToString();
                                                    TxtType.Tag = TDtb.Rows[0]["Order_Type_ID"].ToString();
                                                    TxtCurrency.Tag = TDtb.Rows[0]["Currency_ID"].ToString();
                                                    TxtPayTerms.Tag = TDtb.Rows[0]["PAy_Term_ID"].ToString();
                                                    TxtShipMode.Tag = TDtb.Rows[0]["Ship_Mode_ID"].ToString();
                                                    TxtMerch.Tag = TDtb.Rows[0]["Empl_ID"].ToString();
                                            }
                                        }
                               }
                            }
                        }
                        else if (this.ActiveControl.Name == "TxtOcnType")
                        {                            
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Prefix", " Select A, B From (Select 'OCN' A, 1 B Union All Select 'MOQ' A, 2 B) C Order by B ", String.Empty, 120);
                            if (Dr != null)
                            {
                                TxtOcnType.Text = Dr["A"].ToString();     
                                if(TxtOcnType.Text.ToString() == "MOQ")
                                    {
                                        DataTable TDt = new DataTable();
                                        MyBase.Load_Data("Select Substring(Max(OrdeR_No), 1, 7) A, Substring(Max(OrdeR_No), Len(Max(OrdeR_No))-4, 7) + 1 B , Max(OrdeR_No) Order_No From(Select Max(OrdeR_No) OrdeR_No From Socks_Order_Master Where Prefix = 'MOQ'  Union All Select Max(Order_No)  Order_No from buy_ord_mas Where OrdeRType = 'B')A ", ref TDt);
                                        TxtOCNNo.Text  = TDt.Rows[0][0].ToString() + String.Format("{0:00000}", Convert.ToDouble(TDt.Rows[0][1]));
                                    }
                                    else
                                    {
                                        DataTable TDt = new DataTable();
                                        MyBase.Load_Data("Select Substring(Max(OrdeR_No), 1, 7) A, Substring(Max(OrdeR_No), Len(Max(OrdeR_No))-4, 7) + 1 B , Max(OrdeR_No) Order_No From(Select Max(OrdeR_No) OrdeR_No From Socks_Order_Master Where Prefix = 'OCN' Union All Select Max(Order_No)  Order_No from buy_ord_mas Where OrdeRType = 'S')A ", ref TDt);
                                        TxtOCNNo.Text  = TDt.Rows[0][0].ToString() + String.Format("{0:00000}", Convert.ToDouble(TDt.Rows[0][1]));
                                    }
                            }
                        }
                        else if (this.ActiveControl.Name == "TxtMerch")
                        {
                            if (TxtBuyer.Text.Trim() == String.Empty)
                            {
                                MessageBox.Show("Invalid Buyer", "Gainup");
                                TxtBuyer.Focus();
                                return;
                            }
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Merch", "Select Employee, EmployeeID From Employee ORder by Employee  ", String.Empty, 250);
                            if (Dr != null)
                            {
                                TxtMerch.Text = Dr["Employee"].ToString();
                                TxtMerch.Tag = Dr["EmployeeID"].ToString();                                
                            }
                        }
                        else if (this.ActiveControl.Name == "TxtCategory")
                        {
                            if (TxtMerch.Text.Trim() == String.Empty)
                            {
                                MessageBox.Show("Invalid Merch ", "Gainup");
                                TxtMerch.Focus();
                                return;
                            }
                            if(TxtAddress.Tag.ToString() == "1")
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Category", "Select OrderCategory , OrderCategoryId From OrderCategory Where OrderCategoryId = 2 Order by OrderCategory ", String.Empty, 250);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Category", "Select OrderCategory , OrderCategoryId From OrderCategory Where OrderCategoryId = 1 Order by OrderCategory ", String.Empty, 250);
                            }
                            if (Dr != null)
                            {
                                TxtCategory.Text = Dr["OrderCategory"].ToString();
                                TxtCategory.Tag = Dr["OrderCategoryId"].ToString();
                                if(TxtCategory.Tag.ToString() == "2")
                                {
                                     DataTable TDtb = new DataTable();
                                        MyBase.Load_Data(" Select Currency, ExchangeRate, CurrencyID, ExchangeRate From Currency Where CurrencyID = 25 ", ref TDtb);
                                        if(TDtb.Rows.Count >0)
                                        {
                                                TxtCurrency.Text = TDtb.Rows[0]["Currency"].ToString();                                
                                                TxtCurrency.Tag = TDtb.Rows[0]["CurrencyID"].ToString();
                                                TxtExRate.Text = TDtb.Rows[0]["ExchangeRate"].ToString();
                                        }                                        
                                }
                                else
                                {
                                    TxtCurrency.Text = "";                                
                                    TxtCurrency.Tag = "";
                                    TxtExRate.Text = "0.00";
                                }

                            }
                        }
                        else if (this.ActiveControl.Name == "TxtType")
                        {
                            if (TxtCategory.Text.Trim() == String.Empty)
                            {
                                MessageBox.Show("Invalid Category ", "Gainup");
                                TxtCategory.Focus();
                                return;
                            }
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Type", "Select OrdStyleType , OrdStyleTypeId  From OrdStyle_type Where OrdStyleTypeId = 6 Order by OrdStyleType ", String.Empty, 250);
                                    if (Dr != null)
                                    {
                                        TxtType.Text = Dr["OrdStyleType"].ToString();
                                        TxtType.Tag = Dr["OrdStyleTypeId"].ToString();
                                    }                            
                        }
                        else if (this.ActiveControl.Name == "TxtCurrency")
                        {
                            if (TxtType.Text.Trim() == String.Empty || TxtCategory.Text.ToString() == String.Empty)
                            {
                                MessageBox.Show("Invalid Type & Category ", "Gainup");
                                TxtType.Focus();
                                return;
                            }
                            if (Grid.Rows.Count <=1 || MyParent.UserCode ==1)
                            { 
                                if(TxtCategory.Tag.ToString() == "2")
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Currency", "Select Currency, IsNull(ExchangeRate,0) ExchangeRate, CurrencyID From Currency Where CurrencyID = 25 Order by Currency", String.Empty, 250, 100);
                                }
                                else
                                {
                                       Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Currency", "Select Currency, IsNull(ExchangeRate,0) ExchangeRate, CurrencyID From Currency  Where CurrencyID != 25 Order by Currency", String.Empty, 250, 100);
                                }
                                    if (Dr != null)
                                    {
                                        TxtCurrency.Text = Dr["Currency"].ToString();
                                        TxtCurrency.Tag = Dr["CurrencyID"].ToString(); 
                                        TxtExRate.Text = Dr["ExchangeRate"].ToString(); 
                                    }                             
                            }
                        }
                        else if (this.ActiveControl.Name == "TxtPayTerms")
                        {
                            if (TxtCurrency.Text.Trim() == String.Empty)
                            {
                                MessageBox.Show("Invalid Currency ", "Gainup");
                                TxtCurrency.Focus();
                                return;
                            }
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select PayTerms", "Select Pay_Term, Pay_TermID From Payment_Terms Order by Pay_Term", String.Empty, 250);
                            if (Dr != null)
                            {
                                TxtPayTerms.Text = Dr["Pay_Term"].ToString();
                                TxtPayTerms.Tag = Dr["Pay_TermID"].ToString();
                            }
                        }
                        else if (this.ActiveControl.Name == "TxtShipMode")
                        {
                            if (TxtPayTerms.Text.Trim() == String.Empty)
                            {
                                MessageBox.Show("Invalid PayTerms ", "Gainup");
                                TxtPayTerms.Focus();
                                return;
                            }
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select ShipMode", "Select Mode_Of_Shipment , Mode_of_Shipmentid  From Mode_Of_Shipment Where Mode_of_Shipmentid != 12 Order by Mode_of_Shipment ", String.Empty, 250);
                            if (Dr != null)
                            {
                                TxtShipMode.Text = Dr["Mode_Of_Shipment"].ToString();
                                TxtShipMode.Tag = Dr["Mode_of_Shipmentid"].ToString();
                            }
                        }
                    }
                }
                else if ((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back) && this.ActiveControl.Name != String.Empty ) 
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

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if(Grid.Rows.Count >=2)
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
                Grid.Focus();
                if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                {                    
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        //listBox1.Items.Add(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString());
                       //Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                        Grid.Rows.RemoveAt(Grid.CurrentCell.RowIndex);                        
                       
                        Grid.Refresh();
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
                if (Convert.ToDateTime(DtpODate.Value) > MyBase.GetServerDateTime())
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DtpODate.Value = MyBase.GetServerDate();
                    DtpODate.Focus();
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

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Txt.ShortcutsEnabled = true;
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_NO"].Index)
                    {
                        if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            MessageBox.Show("Invalid PO_NO", "Gainup");
                            Grid.CurrentCell = Grid["PO_NO", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            e.Handled = true;
                            return;
                        }
                        else
                        {
                              if(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                                {
                                    if(Grid.Rows.Count >2 && Grid["PO_DATE", Grid.CurrentCell.RowIndex-1].Value.ToString() != String.Empty)
                                    {
                                        Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value =  Grid["PO_DATE", Grid.CurrentCell.RowIndex-1].Value;
                                    }
                                    else
                                    {
                                        Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value =  Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", DtpODate.Value));
                                    }
                                }
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_DATE"].Index)
                    {
                        if(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value= MyBase.Get_Date_Format(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString());                    
                                if (Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value) > Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", DtpODate.Value)))
                                {
                                    MessageBox.Show("Invalid PO_DATE", "Gainup");
                                    Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", DtpODate.Value));
                                    Grid.CurrentCell = Grid["PO_DATE", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    e.Handled = true;
                                    return;
                                }                               
                                else if(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                                {
                                    if(Grid.Rows.Count >2 && Grid["SHIP_DATE", Grid.CurrentCell.RowIndex-1].Value.ToString() != String.Empty)
                                    {
                                        Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value =  Grid["SHIP_DATE", Grid.CurrentCell.RowIndex-1].Value;
                                    }
                                    else
                                    {
                                        Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value =  Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value;
                                    }
                                }
                        }
                    }

                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["SHIP_DATE"].Index)
                    {
                        if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            if(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value= MyBase.Get_Date_Format(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString());                    
                                if (Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value) > Convert.ToDateTime(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value))
                                {
                                    MessageBox.Show("Invalid Date", "Gainup");
                                    Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value);
                                    Grid.CurrentCell = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    e.Handled = true;
                                    return;
                                }
                                else if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value), Convert.ToDateTime(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value)) > 366  && MyParent.UserCode != 1)
                                {
                                    MessageBox.Show("Invalid Ship Date (Lead Days Must Less than 366 Days)", "Gainup");
                                    Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value);
                                    Grid.CurrentCell = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    e.Handled = true;
                                    return;
                                }
                                else if(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                                {
                                     if(Grid.Rows.Count >2 && Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex-1].Value.ToString() != String.Empty)
                                    {
                                        Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value =  Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex-1].Value;
                                    }
                                    else
                                    {
                                          if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value), Convert.ToDateTime(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value)) > 7)
                                            {
                                                Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value).AddDays(-7);
                                                return;
                                            }
                                            else
                                            {
                                                Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value);
                                                return;
                                            }                                        
                                    }                                   
                                }
                                else if(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                                {
                                    if (Convert.ToDateTime(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value) > Convert.ToDateTime(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value))
                                    {                                        
                                        Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                    MessageBox.Show("Invalid ShipDate", "Gainup");
                                    Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value);
                                    Grid.CurrentCell = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    e.Handled = true;
                                    return;
                            }
                        }
                        else
                        {                        
                            Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value;
                        }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DELIVERY_DATE"].Index)
                {
                    if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            if(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value= MyBase.Get_Date_Format(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString());
                                if (Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value) > Convert.ToDateTime(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value))
                                {
                                    MessageBox.Show("Invalid Date", "Gainup");
                                    Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value);
                                    Grid.CurrentCell = Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    e.Handled = true;
                                    return;
                                }
                                else if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value), Convert.ToDateTime(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value)) > 366  && MyParent.UserCode != 1)
                                {
                                    MessageBox.Show("Invalid Delivery Date (Lead Days Must Less than 366 Days)", "Gainup");
                                    Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value);
                                    Grid.CurrentCell = Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    e.Handled = true;
                                    return;
                                }
                            }
                            else
                            {
                                    MessageBox.Show("Invalid DeliveryDate", "Gainup");
                                    Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value);
                                    Grid.CurrentCell = Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    e.Handled = true;
                                    return;
                            }
                        }
                        else
                        {                        
                            Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value;                            
                        }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BUYER_QTY"].Index)
                {
                    if(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                     {                               
                         MessageBox.Show("Invalid BUYER QTY", "Gainup");
                         Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value = 0;
                         Grid["CONV_BUYER_QTY", Grid.CurrentCell.RowIndex].Value = 0;
                         Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value = 0;
                         Grid["CONV_BOM_QTY", Grid.CurrentCell.RowIndex].Value = 0;
                         Grid.CurrentCell = Grid["BUYER_QTY", Grid.CurrentCell.RowIndex];
                         Grid.Focus();
                         Grid.BeginEdit(true);
                         e.Handled = true;
                         return;                             
                     }
                    else if (Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                        {
                            if (Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value)) 
                            {
                                //MessageBox.Show("Invalid BOM Qty..!", "Gainup");
                                //Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value = "0";                                
                                //Grid.CurrentCell = Grid["BUYER_QTY", Grid.CurrentCell.RowIndex];
                                //Grid.Focus();
                                //Grid.BeginEdit(true);
                                //e.Handled = true;
                                return;
                            }
                        }
                     else
                     {
                        if(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                        if(Grid["RATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["RATE", Grid.CurrentCell.RowIndex].Value = "0.000";
                        }
                         Grid["CONV_BUYER_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                         Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value))) + (((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value))) * (Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) / 100));
                         Grid["CONV_BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                         Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value)) * Convert.ToDouble(TxtExRate.Text.ToString())) ;                            
                     }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ALLOW_PER"].Index)
                {
                    if(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                     {                               
                         MessageBox.Show("Invalid ALLOWANCE %", "Gainup");
                         Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value = 0;                         
                         Grid.CurrentCell = Grid["ALLOW_PER", Grid.CurrentCell.RowIndex];
                         Grid.Focus();
                         Grid.BeginEdit(true);
                         e.Handled = true;
                         return;                             
                     }
                        else if (Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) <= 0 || Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) > 8)
                        {
                            MessageBox.Show("Invalid Allowance Qty, Allowance % Must Between 1 To 8 ..!", "Gainup");
                            Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value = 0;   
                            Grid.CurrentCell = Grid["ALLOW_PER", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);   
                            e.Handled = true;
                            return;
                        }
                    else if(Grid["RATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                     {
                            Grid["RATE", Grid.CurrentCell.RowIndex].Value = "0.000";
                     }
                     else
                     {
                         Grid["CONV_BUYER_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                         Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value))) + (((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value))) * (Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) / 100));
                         Grid["CONV_BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                         Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value)) * Convert.ToDouble(TxtExRate.Text.ToString())) ;                            
                     }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["RATE"].Index)
                {
                    if(Grid["RATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || TxtExRate.Text.ToString() == String.Empty)
                     { 
                         MessageBox.Show("Invalid RATE", "Gainup");
                         Grid["RATE", Grid.CurrentCell.RowIndex].Value = 0.00; 
                         Grid.CurrentCell = Grid["RATE", Grid.CurrentCell.RowIndex];
                         Grid.Focus();
                         Grid.BeginEdit(true);
                         e.Handled = true;
                         return;                             
                     }                     
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["IMAGE_REQ"].Index)
                {
                    if(Grid["IMAGE_REQ", Grid.CurrentCell.RowIndex].Value.ToString() == "Y")
                     { 
                        if(GBImage.Visible == false)
                        {
                               GBImage.Visible = true;
                               if (Grid["IMAGE1", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                               {                                    
                                    GBImage.Visible = true;
                                    if (Img1.Image != null)
                                    {
                                        Img1.Image  = null;
                                    }
                                   e.Handled = true;
                                   ButOK.Focus();                            
                               }
                               else
                               {

                                   byte[] data = (byte[]) Dt.Rows[Grid.CurrentCell.RowIndex]["IMAGE1"];
                                   MemoryStream ms = new MemoryStream(data);
                                   Img1.Image = Image.FromStream(ms);
                                   GBImage.Visible = true;
                                   e.Handled = true;
                                   ButOK.Focus();   
                               }
                        }
                     }                     
                }


                    else  if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DESTINATION"].Index)
                    {
                        if (Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                        {
                            if (Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value)) 
                            {
                                MessageBox.Show("Invalid BOM Qty..!", "Gainup");
                                Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value = "0";                                
                                Grid.CurrentCell = Grid["BUYER_QTY", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                e.Handled = true;
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

        private void ButCancel_Click(object sender, EventArgs e)
        {
            try
            {                
                GBImage.Visible = false;
                if (Grid["IMAGE_REQ", Grid.CurrentCell.RowIndex].Value.ToString() == "Y" && Grid["IMAGE1", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty )
                {
                    Grid["IMAGE_REQ", Grid.CurrentCell.RowIndex].Value = "N";  
                }
                Grid.CurrentCell = Grid["DESTINATION", Grid.CurrentCell.RowIndex];
                Grid.Focus();
                Grid.BeginEdit(true);                
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButOK_Click(object sender, EventArgs e)
        {
            try
            {
                        OpenFileDialog openFileDialog1 = new OpenFileDialog();
                        openFileDialog1.Filter = "Pictures(*.Jpg,*.Gif,*.Bmp)|*.Jpg;,*.Gif;,*.Bmp;";
                        openFileDialog1.FileName = String.Empty;
                        openFileDialog1.ShowDialog();

                        if (openFileDialog1.FileName.Trim() != String.Empty)
                        {
                            Img1.Image = Image.FromFile(openFileDialog1.FileName);
                            Img1.SizeMode = PictureBoxSizeMode.StretchImage;
                        }                                                        
                        Dt.Rows[Grid.CurrentCell.RowIndex]["IMAGE1"] = GetByteArray(openFileDialog1.FileName);

               
                GBImage.Visible = false;
                Grid.CurrentCell = Grid["DESTINATION", Grid.CurrentCell.RowIndex];
                Grid.Focus();
                Grid.BeginEdit(true);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }  
    
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        private void Grid_Leave(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < Grid.Rows.Count-1; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (( Grid["PO_NO", i].Value.ToString()) == Grid["PO_NO", j].Value.ToString() && (Grid["SAMPLE_ID", i].Value.ToString()) == (Grid["SAMPLE_ID", j].Value.ToString()) && (Grid["SHIP_DATE", i].Value.ToString()) == (Grid["SHIP_DATE", j].Value.ToString()))
                        {
                                MessageBox.Show("Already PONO , SAMPLE_NO & SHIP_DATE are Available", "Gainup");
                                Grid["PO_NO", j].Value = "";
                                Grid["SAMPLE_NO", j].Value = "";
                                Grid["RATE", j].Value = "0.0000";
                                j=Grid.Rows.Count ;
                                Total_Count();                                
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

        private void TxtExRate_Leave(object sender, EventArgs e)
        {
            try
            {
                if(TxtExRate.Text.ToString() != String.Empty)
                {                    
                    for (int i = 0; i < Grid.Rows.Count-1; i++)
                    {
                        if ((Grid["RATE", i].Value.ToString()) != String.Empty && (Grid["BUYER_QTY", i].Value.ToString()) != String.Empty )
                        {
                            Grid["AMOUNT", i].Value = ((Convert.ToDouble(Grid["RATE", i].Value) * Convert.ToDouble(Grid["BUYER_QTY", i].Value)) * Convert.ToDouble(TxtExRate.Text.ToString())) ;                            
                        }
                    }
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
                 TxtBuyer.Focus();
                 SendKeys.Send("{Down}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Arrow_Merch_Click(object sender, EventArgs e)
        {
            try
            {
                 TxtMerch.Focus();
                 SendKeys.Send("{Down}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Arrow_Category_Click(object sender, EventArgs e)
        {
            try
            {
                 TxtCategory.Focus();
                 SendKeys.Send("{Down}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Arrow_Type_Click(object sender, EventArgs e)
        {
            try
            {
                 TxtType.Focus();
                 SendKeys.Send("{Down}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Arrow_currency_Click(object sender, EventArgs e)
        {
            try
            {
                 TxtCurrency.Focus();
                 SendKeys.Send("{Down}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Arrow_Payterms_Click(object sender, EventArgs e)
        {
            try
            {
                 TxtPayTerms.Focus();
                 SendKeys.Send("{Down}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Arrow_ShipMode_Click(object sender, EventArgs e)
        {
            try
            {
                 TxtShipMode.Focus();
                 SendKeys.Send("{Down}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
             try
            {
                 return;                
                 if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_NO"].Index)
                {
                    if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value  != null)
                    {
                        if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty && Grid.Rows.Count >2)
                        {
                                        Grid["PO_NO", Grid.CurrentCell.RowIndex].Value= Grid["PO_NO", Grid.CurrentCell.RowIndex-1].Value.ToString();
                                        SendKeys.Send("{Enter}");

                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_DATE"].Index)
                {
                    if(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value  != null)
                    {
                        if(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty && Grid.Rows.Count >2)
                        {
                                        Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value= Grid["PO_DATE", Grid.CurrentCell.RowIndex-1].Value.ToString();

                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["SHIP_DATE"].Index)
                {
                    if(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value  != null)
                    {
                        if(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty && Grid.Rows.Count >2)
                        {
                                        Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value= Grid["SHIP_DATE", Grid.CurrentCell.RowIndex-1].Value.ToString();

                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DELIVERY_DATE"].Index)
                {
                    if(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value  != null)
                    {
                        if(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty && Grid.Rows.Count >2)
                        {
                                        Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value= Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex-1].Value.ToString();

                        }
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_DATE"].Index)
                {
                    if(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value  != null)
                    {
                        if(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                                        Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value= MyBase.Get_Date_Format(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString());

                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["SHIP_DATE"].Index)
                {
                    if(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value  != null)
                    {
                        if(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                                        Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value= MyBase.Get_Date_Format(Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString());
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DELIVERY_DATE"].Index)
                {
                    if(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value  != null)
                    {
                        if(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                                        Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value= MyBase.Get_Date_Format(Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString());
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_NO"].Index)
                {
                    if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value  != null)
                    {
                        if(Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty && Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                                        Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value= Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", DtpODate.Value));

                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BUYER_QTY"].Index)
                {                    
                    if(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value != null)
                     {                                                                      
                        if(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                        if(Grid["RATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["RATE", Grid.CurrentCell.RowIndex].Value = "0.000";
                        }
                         Grid["CONV_BUYER_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                         Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value))) + (((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value))) * (Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) / 100));
                         Grid["CONV_BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                         Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value)) * Convert.ToDouble(TxtExRate.Text.ToString())) ;                            
                     }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ALLOW_PER"].Index)
                {                
                    if(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value != null)
                    {
                         if(Grid["RATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                         {
                                Grid["RATE", Grid.CurrentCell.RowIndex].Value = "0.000";
                         }
                         else
                         {
                             Grid["CONV_BUYER_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                             Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value))) + (((Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value))) * (Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) / 100));
                             Grid["CONV_BOM_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["BOM_QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                             Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value)) * Convert.ToDouble(TxtExRate.Text.ToString())) ;                            
                         }
                    }
                }
            }            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButFill_Click(object sender, EventArgs e)
        {
            try
            {
                if(Grid.Rows.Count >1)
                {
                    for(int f=Grid.CurrentCell.RowIndex+1; f<= Grid.Rows.Count-2; f++)
                    {
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_DATE"].Index)
                        {
                            if (Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["PO_NO", f].Value.ToString())
                                {
                                    Grid["PO_DATE", f].Value = Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString();                                   
                                }
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["SHIP_DATE"].Index)
                        {
                            if (Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["PO_NO", f].Value.ToString())
                                {
                                    Grid["SHIP_DATE", f].Value = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString();                                   
                                }
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DELIVERY_DATE"].Index)
                        {
                            if (Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["PO_NO", f].Value.ToString())
                                {
                                    Grid["DELIVERY_DATE", f].Value = Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString();                                   
                                }
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DESTINATION"].Index)
                        {
                            if (Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["DESTINATION", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["PO_NO", f].Value.ToString())
                                {
                                    Grid["DESTINATION", f].Value = Grid["DESTINATION", Grid.CurrentCell.RowIndex].Value.ToString();                                   
                                }
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PORTOFLOADING"].Index)
                        {
                            if (Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["PORTOFLOADING", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["PO_NO", f].Value.ToString())
                                {
                                    Grid["PORTOFLOADING", f].Value = Grid["PORTOFLOADING", Grid.CurrentCell.RowIndex].Value.ToString();                                   
                                }
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["WASH_REQ"].Index)
                        {
                            if (Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["WASH_REQ", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["PO_NO", f].Value.ToString())
                                {
                                    Grid["WASH_REQ", f].Value = Grid["WASH_REQ", Grid.CurrentCell.RowIndex].Value.ToString();                                   
                                }
                            }
                        }
                        //else if (GridTrim.CurrentCell.ColumnIndex == GridTrim.Columns["CONS"].Index)
                        //{
                        //    if (GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        //    {
                        //        if(GridTrim["SNO1", GridTrim.CurrentCell.RowIndex].Value.ToString() == GridTrim["SNO1", f].Value.ToString())
                        //        {
                        //            GridTrim["CONS", f].Value = GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value.ToString();  
                        //            if (GridTrim["CONS", f].Value.ToString() == String.Empty)
                        //            {
                        //                GridTrim["CONS", GridTrim.CurrentCell.RowIndex].Value = 0;
                        //            }
                        //            else if (GridTrim["PLAN_TYPE", f].Value.ToString() == String.Empty)
                        //            {
                        //                GridTrim["PLAN_TYPE", f].Value = "M";
                        //            }
                        //            if (GridTrim["PLAN_TYPE", f].Value.ToString() == "/" && Convert.ToDouble(GridTrim["CONS", f].Value) > 0)
                        //            {
                        //                GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["BOM_QTY", f].Value) / Convert.ToDouble(GridTrim["CONS", f].Value);
                        //            }
                        //            else if (GridTrim["PLAN_TYPE", f].Value.ToString() == "*")
                        //            {
                        //                GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["CONS", f].Value) * Convert.ToDouble(GridTrim["BOM_QTY", f].Value);
                        //            }
                        //            else if (GridTrim["PLAN_TYPE", f].Value.ToString() == "M")
                        //            {
                        //                GridTrim["REQ_QTY", f].Value = Convert.ToDouble(GridTrim["CONS", f].Value);
                        //            }
                        //            else
                        //            {
                        //                GridTrim["REQ_QTY", f].Value = 0;
                        //            }
                        //        }
                        //    }
                        //}                       
                    }
                }
            }             
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ButFillAll_Click(object sender, EventArgs e)
        {
            try
            {
                if(Grid.Rows.Count >1)
                {
                    for(int f=Grid.CurrentCell.RowIndex+1; f<= Grid.Rows.Count-2; f++)
                    {
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PO_DATE"].Index)
                        {
                            if (Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                //if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["PO_NO", f].Value.ToString())
                                //{
                                    Grid["PO_DATE", f].Value = Grid["PO_DATE", Grid.CurrentCell.RowIndex].Value.ToString();                                   
                                ////}
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["SHIP_DATE"].Index)
                        {
                            if (Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                //if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["PO_NO", f].Value.ToString())
                                //{
                                    Grid["SHIP_DATE", f].Value = Grid["SHIP_DATE", Grid.CurrentCell.RowIndex].Value.ToString();                                   
                                //}
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DELIVERY_DATE"].Index)
                        {
                            if (Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                //if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["PO_NO", f].Value.ToString())
                                //{
                                    Grid["DELIVERY_DATE", f].Value = Grid["DELIVERY_DATE", Grid.CurrentCell.RowIndex].Value.ToString();                                   
                                //}
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DESTINATION"].Index)
                        {
                            if (Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["DESTINATION", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                //if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["PO_NO", f].Value.ToString())
                                //{
                                    Grid["DESTINATION", f].Value = Grid["DESTINATION", Grid.CurrentCell.RowIndex].Value.ToString();                                   
                                //}
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PORTOFLOADING"].Index)
                        {
                            if (Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["PORTOFLOADING", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                //if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["PO_NO", f].Value.ToString())
                                //{
                                    Grid["PORTOFLOADING", f].Value = Grid["PORTOFLOADING", Grid.CurrentCell.RowIndex].Value.ToString();                                   
                                //}
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["WASH_REQ"].Index)
                        {
                            if (Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["WASH_REQ", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                //if(Grid["PO_NO", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["PO_NO", f].Value.ToString())
                                //{
                                    Grid["WASH_REQ", f].Value = Grid["WASH_REQ", Grid.CurrentCell.RowIndex].Value.ToString();                                   
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

        private void ArrowOcnType_Click(object sender, EventArgs e)
        {
            try
            {
                 TxtOcnType.Focus();
                 SendKeys.Send("{Down}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }               
    }
}
