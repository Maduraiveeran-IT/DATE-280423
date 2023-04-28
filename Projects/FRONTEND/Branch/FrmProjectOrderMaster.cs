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
    public partial class FrmProjectOrderMaster : Form,Entry  
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataTable Dt2 = new DataTable();
        DataGrid dgv = new DataGrid();
        DataRow Dr;        
        Int64 Code;
        Int32 C=0; 
        TextBox Txt = null;
        TextBox Txt_Qty = null;    
        TextBox Txt_Img = null;   
        DataTable[] DtImg;
        String[] Queries;
        String Str, SName="";      
       
        public FrmProjectOrderMaster()
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
                TxtPrjType.Focus();                
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
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Order No - Edit", "select Distinct Order_NO, ORder_Date, Proj_Name, PArty, Employee, Proj_ACtivity_Name, UOM, Estimate_Date, Complete_Date, Qty, Total_Qty, Total_Conv_Qty, Total_Amount, Company_Code, Year_Code, Approval_Flag, Complete_Order,  EmplNo, PArty_Code, Proj_activity_ID, Proj_Type_ID, Remarks, Rowid From Project_Order_Fn() Where Rowid Not in (Select Distinct ORder_ID From Project_Planning_MAster) and Company_Code = " + MyParent.CompCode + " and PArty_Code = " + MyParent.Proj_Login_Code + " ORder by Order_NO Desc ", String.Empty, 120, 100, 120, 140, 120, 140, 120, 80, 80, 80, 80, 80, 80, 80);
                if (Dr != null)
                {
                    Fill_Datas(Dr);                                        
                    Grid.CurrentCell = Grid["PROJ_ACTIVITY", 0];
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
                TxtPrjNo.Text = Dr["Order_No"].ToString();                
                TxtPrjNo.Tag = Dr["RowID"].ToString();
                DtpODate.Value = Convert.ToDateTime(Dr["Order_Date"]);                
                TxtBuyer.Text = Dr["Party"].ToString();
                TxtEmployee.Text = Dr["Employee"].ToString();                                
                TxtTotalBom.Text = Dr["Total_Conv_Qty"].ToString();
                TxtNetAmount.Text = Dr["Total_Amount"].ToString();                
                 
                TxtTotOrderQty.Text = Dr["Total_Qty"].ToString();                
                TxtRemarks.Text = Dr["Remarks"].ToString();
                TxtBuyer.Tag = Dr["PArty_Code"].ToString();                
                TxtEmployee.Tag = Dr["EmplNo"].ToString();
                TxtPrjType.Text = Dr["Proj_Name"].ToString();
                TxtPrjType.Tag = Dr["Proj_Type_ID"].ToString();               
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
               
              
                if (TxtPrjType.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Project Name", "Gainup");
                    TxtPrjType.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtBuyer.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Party", "Gainup");
                    TxtBuyer.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                DataTable Dts = new DataTable();
                String St1 = "Select * From ACCOUNTS.dbo.Blocked_Ledgers_List(2) Where Ledger_Code= " + TxtBuyer.Tag.ToString() + "";
                MyBase.Load_Data(St1, ref Dts);
                if (Dts.Rows.Count > 0)
                {
                    MessageBox.Show("This Buyer Has Been Blocked By Accounts...!");
                    TxtBuyer.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (MyParent.UserCode != 1 && MyParent._New == true && (TxtBuyer.Tag.ToString() == "5465" || TxtBuyer.Tag.ToString() == "5275"))
                {
                    MessageBox.Show("Buyer Locked", "Gainup");
                    TxtBuyer.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

               

              
                if (TxtEmployee.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Employee Name", "Gainup");
                    TxtEmployee.Focus();
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

                if (TxtTotalBom.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Order Qty ", "Gainup");
                    Grid.CurrentCell = Grid["PROJ_ACTIVITY", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

              
                for (int i = 0; i < Grid.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Grid.Columns.Count - 1; j++)
                    {
                        if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty || Grid[j, i].Value.ToString() == "0")
                        {
                            if (Grid.Columns[j].Name.ToString() == "ALLOW_PER" || Grid.Columns[j].Name.ToString() == "REFNO" || Grid.Columns[j].Name.ToString() == "RATE" || Grid.Columns[j].Name.ToString() == "AMOUNT")
                            {

                            }
                            else if (Grid["IMAGE_REQ", i].Value.ToString() == String.Empty || Grid["IMAGE1", i].Value.ToString() == String.Empty)
                            {
                                Grid["IMAGE_REQ", i].Value = "N";
                            }
                            else
                            {
                                MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                Grid.CurrentCell = Grid[j, i];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }                      
                    }
                        Grid["ESTIMATE_DATE", i].Value = MyBase.Get_Date_Format(Grid["ESTIMATE_DATE", i].Value.ToString());
                        Grid["COMPLETE_DATE", i].Value = MyBase.Get_Date_Format(Grid["COMPLETE_DATE", i].Value.ToString());

                        //if (Convert.ToDouble(Grid["ALLOW_PER", i].Value) < 0 || Convert.ToDouble(Grid["ALLOW_PER", i].Value) > 8)
                        //{
                        //    MessageBox.Show("Invalid Allowance Qty, Allowance % Must Between 0 To 8 ..!", "Gainup");
                        //    Grid.CurrentCell = Grid[1, i];
                        //    Grid.Focus();
                        //    Grid.BeginEdit(true);
                        //    MyParent.Save_Error = true;
                        //    return;
                        //}

                        if (Convert.ToDouble(Grid["QTY", i].Value) <= 0)
                        {
                            MessageBox.Show("Invalid Qty ..!", "Gainup");
                            Grid.CurrentCell = Grid[1, i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }

                        //if (Convert.ToDouble(Grid["QTY", i].Value) < Convert.ToDouble(Grid["CONV_QTY", i].Value))
                        //{
                        //    MessageBox.Show("Invalid Qty..!", "Gainup");
                        //    Grid.CurrentCell = Grid[1, i];
                        //    Grid.Focus();
                        //    Grid.BeginEdit(true);
                        //    MyParent.Save_Error = true;
                        //    return;
                        //}

                   

                        

                        if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(Grid["ESTIMATE_DATE", i].Value), Convert.ToDateTime(Grid["COMPLETE_DATE", i].Value)) < 0 )
                        {
                            MessageBox.Show("Invalid ESTIMATE_DATE &  COMPLETE_DATE ", "Gainup");
                            Grid["ESTIMATE_DATE", i].Value = Convert.ToDateTime(Grid["ESTIMATE_DATE", i].Value);
                            Grid.CurrentCell = Grid["ESTIMATE_DATE", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }

                    if(Grid["IMAGE_REQ", i].Value.ToString() == String.Empty)
                    {
                        Grid["IMAGE_REQ", i].Value = "N";
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
                    Grid["AMOUNT", i].Value = ((Convert.ToDouble(Grid["RATE", i].Value) * Convert.ToDouble(Grid["QTY", i].Value))) ;                            
                    Grid["CONV_QTY", i].Value = ((Convert.ToDouble(Grid["QTY", i].Value) * 1));

                }
               
                Queries = new String[Grid.Rows.Count * 6 + 40];                    
                if(MyParent._New)
                {

                    DataTable TDt1 = new DataTable();
                    MyBase.Load_Data("Select Substring(Max(OrdeR_No), 1, 7) A, Substring(Max(OrdeR_No), Len(Max(OrdeR_No))-3, 7) + 1 B , Max(OrdeR_No) OrdeR_No From(Select Max(OrdeR_No) OrdeR_No From Project_ORder_MASter Where Company_Code = " + MyParent.CompCode + " and PArty_Code = " + TxtBuyer.Tag + ")A having Substring(Max(OrdeR_No), 1, 7)  is not null", ref TDt1);
                    if (TDt1.Rows.Count > 0)
                    {
                        TxtPrjNo.Text = TDt1.Rows[0][0].ToString() + String.Format("{0:0000}", Convert.ToDouble(TDt1.Rows[0][1]));
                    }
                    else
                    {
                        if (MyParent.CompCode == 1)
                        {
                            TxtPrjNo.Text = "GUP-" + SName + "0001";
                        }
                        else if (MyParent.CompCode == 2)
                        {
                            TxtPrjNo.Text = "ALM-" + SName + "0001";
                        }
                        else if (MyParent.CompCode == 3)
                        {
                            TxtPrjNo.Text = "IRL-" + SName + "0001";
                        }
                        else if (MyParent.CompCode == 8)
                        {
                            TxtPrjNo.Text = "GUT-" + SName + "0001";
                        }
                        else
                        {
                            TxtPrjNo.Text = "GUP-" + SName + "0001";
                        }
                    }
                    if (TxtPrjNo.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Invalid Order No", "Gainup");
                        TxtPrjNo.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    Queries[Array_Index++] = "Insert into Project_ORdeR_MASter (Order_No, Order_Date, Proj_Type_ID,  PArty_Code, EmplNo, Approval_Flag,  Remarks, Total_Qty, Total_Conv_Qty, Total_Amount, Complete_Order, Cancel_Order, Company_Code, Year_Code) Values ('" + TxtPrjNo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}  {0:T}", DtpODate.Value) + "', " + TxtPrjType.Tag + ",  " + TxtBuyer.Tag + ", " + TxtEmployee.Tag + ",  'T', '" + TxtRemarks.Text.ToString() + "', " + TxtTotOrderQty.Text + ", " + Convert.ToDouble(TxtTotalBom.Text.ToString()) + ", " + Convert.ToDouble(TxtNetAmount.Text.ToString()) + ",  'N', 'N', " + MyParent.CompCode + ", '" + MyParent.YearCode.ToString() + "') ; Select Scope_Identity()";
                    Queries[Array_Index++] = MyParent.EntryLog("PROJECT ORDER MASTER", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Project_ORdeR_MASter Set  Proj_Type_ID = " + TxtPrjType.Tag + ",  PArty_Code = " + TxtBuyer.Tag + ", EmplNo =  " + TxtEmployee.Tag + ", Remarks = '" + TxtRemarks.Text + "', Total_Qty = " + Convert.ToDouble(TxtTotOrderQty.Text.ToString()) + ",  Total_Amount = " + Convert.ToDouble(TxtNetAmount.Text.ToString()) + ", Total_Conv_Qty = " + Convert.ToDouble(TxtTotalBom.Text.ToString()) + "  Where Rowid = " + Code;
                    Queries[Array_Index++] = "Delete From Project_ORdeR_Details Where Master_id = " + Code;
                    Queries[Array_Index++] = "Delete From  Vaahini_Gainup_Photo.Dbo.Project_Order_Image Where Master_id = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("PROJECT ORDER MASTER", "EDIT", Code.ToString());
                }

                for (int i = 0; i < Grid.Rows.Count - 1; i++)
                {
                    if (Grid["QTY", i].Value.ToString() != String.Empty && Grid["QTY", i].Value != DBNull.Value) 
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into Project_ORdeR_Details (MAsteR_ID, Sno, RefNo, Estimate_Date, Complete_Date, Uom_ID, Qty, Allow_Per, Conv_Qty, Rate, Amount, IMAGE_REQ, Proj_Activity_ID) Values (@@IDENTITY, " + Grid["SNO", i].Value + ", '" + Grid["REFNO", i].Value + "',  '" + String.Format("{0:dd-MMM-yyyy}", Grid["ESTIMATE_DATE", i].Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", Grid["COMPLETE_DATE", i].Value) + "',  " + (Grid["UOM_ID", i].Value) + ", " + Convert.ToDouble(Grid["QTY", i].Value) + ", " + Grid["ALLOW_PER", i].Value + ", " + Convert.ToDouble(Grid["CONV_QTY", i].Value) + ", " + Convert.ToDouble(Grid["RATE", i].Value) + ", " + Convert.ToDouble(Grid["AMOUNT", i].Value) + ", '" + Grid["IMAGE_REQ", i].Value + "', " + Grid["PROJ_ACTIVITY_ID", i].Value + ")";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert into Project_ORdeR_Details (MAsteR_ID, Sno, RefNo, Estimate_Date, Complete_Date, Uom_ID,  Qty, Allow_Per, Conv_Qty, Rate, Amount, IMAGE_REQ, Proj_Activity_ID) Values (" + Code + ", " + Grid["SNO", i].Value + ", '" + Grid["REFNO", i].Value + "',  '" + String.Format("{0:dd-MMM-yyyy}", Grid["ESTIMATE_DATE", i].Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", Grid["COMPLETE_DATE", i].Value) + "',  " + (Grid["UOM_ID", i].Value) + ", " + Convert.ToDouble(Grid["QTY", i].Value) + ", " + Grid["ALLOW_PER", i].Value + ", " + Convert.ToDouble(Grid["CONV_QTY", i].Value) + ", " + Convert.ToDouble(Grid["RATE", i].Value) + ", " + Convert.ToDouble(Grid["AMOUNT", i].Value) + ", '" + Grid["IMAGE_REQ", i].Value + "',  " + Grid["PROJ_ACTIVITY_ID", i].Value + ")";
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
                            String Str1 = " Select IDENT_CURRENT('Project_ORdeR_MASter')  Identity_Mas";
                            MyBase.Load_Data(Str1, ref TDt);                            
                            Str = " Insert into VAAHINI_GAINUP_PHOTO.dbo.Project_Order_Image (Master_ID, Sno, Image1) Values (" + TDt.Rows[0][0] + ", " + Grid["SNO", i].Value + ",  ?)";
                        }
                        else
                        {
                            Str = " Insert into VAAHINI_GAINUP_PHOTO.dbo.Project_Order_Image (Master_ID, Sno, Image1) Values (" + Code + ", " + Grid["SNO", i].Value + ",  ?)";
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
                 Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Order No - Delete", "select Distinct Order_NO, ORder_Date, Proj_Name, PArty, Employee, Proj_ACtivity_Name, UOM, Estimate_Date, Complete_Date, Qty, Total_Qty, Total_Conv_Qty, Total_Amount, Company_Code, Year_Code, Approval_Flag, Complete_Order,  EmplNo, PArty_Code, Proj_activity_ID, Proj_Type_ID, Remarks, Rowid From Project_Order_Fn() Where Rowid Not in (Select Distinct ORder_ID From Project_Planning_MAster) and company_Code = " + MyParent.CompCode + " and PArty_Code = " + MyParent.Proj_Login_Code + " ORder by Order_NO Desc ", String.Empty, 120, 100, 120, 140, 120, 140, 120, 80, 80, 80, 80, 80, 80, 80);
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
                    MyBase.Run("Delete from Vaahini_Gainup_Photo.Dbo.Project_Order_Image Where MasteR_ID = " + Code + " ", "Delete from Project_ORder_Details Where MasteR_ID = " + Code + " ", "Delete from Project_ORder_Master Where RowID = " + Code, MyParent.EntryLog("PROJECT ORDER MASTER", "DELETE", Code.ToString()));
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
                 Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Order No - View", "select Distinct Order_NO, ORder_Date, Proj_Name, PArty, Employee, Proj_ACtivity_Name, UOM, Estimate_Date, Complete_Date, Qty, Total_Qty, Total_Conv_Qty, Total_Amount, Company_Code, Year_Code, Approval_Flag, Complete_Order,  EmplNo, PArty_Code, Proj_activity_ID, Proj_Type_ID, Remarks, Rowid,'N' T From Project_Order_Fn() Where Company_Code = " + MyParent.CompCode + " and PArty_Code = " + MyParent.Proj_Login_Code + " ORder by Order_NO Desc ", String.Empty, 120, 100, 120, 140, 120, 140, 120, 80, 80, 80, 80, 80, 80, 80);
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
                    Str = "Select 0 SNO, '' PROJ_ACTIVITY, '' UOM, '' REFNO, CAst(ESTIMATE_DATE as Varchar(20)) ESTIMATE_DATE, Cast(COMPLETE_DATE as Varchar(20)) COMPLETE_DATE,  'N' IMAGE_REQ, 0.00 RATE, 0.000 QTY, 0 ALLOW_PER, 0 CONV_QTY, 0.00 AMOUNT, 0 PROJ_ACTIVITY_ID, 0 UOM_ID, IMAGE1, 'N' T From Project_ORder_Details A LEft Join Vaahini_Gainup_Photo.Dbo.Project_Order_Image B On A.RowID = B.MAster_ID Where 1 = 2";
                }
                else
                {
                    Str = "Select A.SNO, A.Proj_ACtivity_Name PROJ_ACTIVITY, A.UOM, A.REFNO, Convert(Varchar(20),A.ESTIMATE_DATE, 104) ESTIMATE_DATE,  Convert(Varchar(20),A.COMPLETE_DATE, 104) COMPLETE_DATE,  A.IMAGE_REQ, A.RATE, A.QTY, A.ALLOW_PER, A.CONV_QTY, A.AMOUNT, A.PROJ_ACTIVITY_ID PROJ_ACTIVITY_ID, A.UOM_ID UOM_ID, A.IMAGE1, 'N' T From Project_ORder_Fn() A Where A.RowID = " + Code + "  Order by A.Order_By_Slno ";                    
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.ReadOnly_Grid_Without(ref Grid, "PROJ_ACTIVITY", "ESTIMATE_DATE", "COMPLETE_DATE", "QTY");
                MyBase.Grid_Designing(ref Grid, ref Dt, "IMAGE1", "ALLOW_PER", "CONV_QTY", "PROJ_ACTIVITY_ID", "IMAGE_REQ", "REFNO", "RATE", "AMOUNT", "UOM_ID", "T");
                MyBase.Grid_Width(ref Grid, 50, 300, 120, 100, 100, 140);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["REFNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["PROJ_ACTIVITY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["ESTIMATE_DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["COMPLETE_DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                
                Grid.Columns["RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["CONV_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["ALLOW_PER"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                Grid.Columns["AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["IMAGE_REQ"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["ESTIMATE_DATE"].HeaderText = "START_DATE";           
                Grid.Columns["RATE"].DefaultCellStyle.Format = "0.00";                
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
                       

                        Str = "Select SNo, Image1, Master_ID From Vaahini_Gainup_Photo.Dbo.Project_Order_Image  WHERE MasteR_ID= " + Code + " Order by SNo";
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
                 //   Txt.Leave +=new EventHandler(Txt_Leave);
                    Txt.TextChanged +=new EventHandler(Txt_TextChanged);
                  //  Txt.GotFocus +=new EventHandler(Txt_GotFocus);
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
                
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["REFNO"].Index)
                {
                    if (Grid["REFNO", Grid.CurrentCell.RowIndex].Value != null)
                    {
                        if (Grid["REFNO", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty && Grid.Rows.Count >= 2 && Grid.CurrentCell.RowIndex >= 1)
                        {
                            Grid["REFNO", Grid.CurrentCell.RowIndex].Value = Grid["REFNO", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        }
                        Txt.Text = Grid["REFNO", Grid.CurrentCell.RowIndex].Value.ToString();
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ESTIMATE_DATE"].Index)
                {
                    if (Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value != null)
                    {
                        if (Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty && Grid.Rows.Count > 2 && Grid.CurrentCell.RowIndex >= 1)
                        {
                            Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value = Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex - 1].Value.ToString();


                        }
                        Txt.Text = Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value.ToString();
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["COMPLETE_DATE"].Index)
                {
                    if (Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value != null)
                    {
                        if (Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty && Grid.Rows.Count > 2 && Grid.CurrentCell.RowIndex >= 1)
                        {
                            Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value = Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex - 1].Value.ToString();


                        }
                        Txt.Text = Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value.ToString();
                    }
                }                
                return;                                
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["QTY"].Index)
                {
                    if (Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value == null || Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) == 0)
                    {
                        Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value = "0";
                    }
                    else if (Grid["RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) == 0)
                    {
                        Grid["RATE", Grid.CurrentCell.RowIndex].Value = "0.0000";
                    }
                    else
                    {
                        if (Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0)
                        {
                            Grid["QTY", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                        else
                        {
                            if (Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                            {
                                 Grid["CONV_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Txt.Text) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                                 Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = (( Math.Round(Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value),4) * Convert.ToDouble(Grid["QTY", Grid.CurrentCell.RowIndex].Value))) ;
                            }
                        }
                    }                   
                }    
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ALLOW_PER"].Index && Grid["QTY", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (Grid["QTY", Grid.CurrentCell.RowIndex].Value == null || Grid["QTY", Grid.CurrentCell.RowIndex].Value == DBNull.Value)
                    {
                        Grid["QTY", Grid.CurrentCell.RowIndex].Value = "0";
                    }
                    else if (Grid["RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) == 0)
                    {
                        Grid["RATE", Grid.CurrentCell.RowIndex].Value = "0.00";
                    }
                    else
                    {
                        if (Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0)
                        {
                            Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                        else
                        {
                            if (Grid["QTY", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                            {
                                 Grid["CONV_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["QTY", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["CONV_VAL", Grid.CurrentCell.RowIndex].Value)));                            
                                 Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = ((Math.Round(Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value),4) * Convert.ToDouble(Grid["BUYER_QTY", Grid.CurrentCell.RowIndex].Value)));
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
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PROJ_ACTIVITY"].Index)
                        {
                            Dr = Tool.Selection_Tool_Except_New("PROJ_ACTIVITY", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select PROJ_ACTIVITY", " Select A.Name PROJ_ACTIVITY, B.UOM, A.Order_By_Slno, A.Rowid, B.UOMID, 'Y' T From Project_Activity_NAme_Master A LEft Join Uom_Master B On A.Uom_ID = B.UOMID ORder by A.Order_By_Slno, A.Name ", String.Empty, 400, 200, 60);
                            if (Dr != null)
                            {
                                Grid["PROJ_ACTIVITY", Grid.CurrentCell.RowIndex].Value = Dr["PROJ_ACTIVITY"].ToString();
                                Grid["PROJ_ACTIVITY_ID", Grid.CurrentCell.RowIndex].Value = Dr["RowId"].ToString();
                                Grid["UOM", Grid.CurrentCell.RowIndex].Value = Dr["UOM"].ToString();
                                Grid["UOM_ID", Grid.CurrentCell.RowIndex].Value = Dr["UOMID"].ToString();
                                Grid["T", Grid.CurrentCell.RowIndex].Value = Dr["T"].ToString();
                                Txt.Text = Dr["PROJ_ACTIVITY"].ToString();
                            }
                        }
                                                                       
                    }                              
                   else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                   {
                         
                   }                   
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
                TxtTotOrderQty.Text = MyBase.Sum(ref Grid, "QTY", "PROJ_ACTIVITY");
                TxtTotalBom.Text = MyBase.Sum(ref Grid, "CONV_QTY", "PROJ_ACTIVITY");
                TxtNetAmount.Text = MyBase.Sum(ref Grid, "AMOUNT", "PROJ_ACTIVITY");
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ESTIMATE_DATE"].Index)
                {
                    if (Grid["PROJ_ACTIVITY", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        MessageBox.Show("Invalid PROJ_ACTIVITY & ESTIMATE_DATE");
                        Grid.CurrentCell = Grid["PROJ_ACTIVITY", Grid.CurrentCell.RowIndex];
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
                    if (Grid["QTY", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToInt32(Grid["QTY", Grid.CurrentCell.RowIndex].Value.ToString()) == 0)
                    {
                        MessageBox.Show("Invalid QTY");
                        Grid.CurrentCell = Grid["QTY", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                         MyBase.Valid_Decimal(Txt, e);
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["QTY"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["ALLOW_PER"].Index)
                {

                    MyBase.Valid_Decimal(Txt, e);
                     
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["RATE"].Index)
                {
                    
                        MyBase.Valid_Decimal(Txt, e);
                    
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ESTIMATE_DATE"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["COMPLETE_DATE"].Index)
                {
                    MyBase.Valid_Date(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["REFNO"].Index)
                {
                    MyBase.Return_Ucase(e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["IMAGE_REQ"].Index)
                {                  
                       MyBase.Valid_Yes_OR_No(Txt, e);                  
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
                
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["REFNO"].Index && Txt.Text.ToString() != String.Empty)
                {
                    Grid["REFNO", Grid.CurrentCell.RowIndex].Value = Txt.Text.ToString();
                    if (Grid.Rows.Count > 1 && Grid.CurrentCell.RowIndex > 0 && Grid["REFNO", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                       {
                            if(Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value == null)
                            {
                                Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value = Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex - 1].Value;
                            }
                            if(Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value == null)
                            {
                                Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value = Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex - 1].Value;
                            }
                            if (Grid["REFNO", Grid.CurrentCell.RowIndex].Value == null)
                            {
                                Grid["REFNO", Grid.CurrentCell.RowIndex].Value = Grid["REFNO", Grid.CurrentCell.RowIndex - 1].Value;  
                            }                           
                       }
                    Grid["REFNO", Grid.CurrentCell.RowIndex].Value = Txt.Text;
                    if (Grid["IMAGE_REQ", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["IMAGE_REQ", Grid.CurrentCell.RowIndex].Value = "N";
                    }
                    if (Grid["IMAGE_REQ", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["IMAGE_REQ", Grid.CurrentCell.RowIndex].Value = "N";
                    }

                }               
                    return;             
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
                    TxtRemarks.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void FrmProjectOrderMaster_Load(object sender, EventArgs e)
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

         private void FrmProjectOrderMaster_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {                               
               if (this.ActiveControl.Name != String.Empty && this.ActiveControl.Name != "TxtRemarks")
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
            }
        }

        private void FrmProjectOrderMaster_KeyDown(object sender, KeyEventArgs e)
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
                    else if (this.ActiveControl.Name == "TxtEmployee")
                    {
                        Grid.CurrentCell = Grid["PROJ_ACTIVITY", 0];
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
                            if (Grid.Rows.Count <= 1 || MyParent.UserCode == 1)
                            {
                                if (this.ActiveControl.Name == "TxtBuyer")
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer", "Select Ledger_Name Name, LEdger_Code RowID, Short  From  Buyer_all_Fn() Where LEdger_Code = " + MyParent.Proj_Login_Code + " ", String.Empty, 600);

                                    if (Dr != null)
                                    {
                                        TxtBuyer.Text = Dr["Name"].ToString();
                                        TxtBuyer.Tag = Dr["RowID"].ToString();
                                    }

                                    if (TxtBuyer.Tag.ToString() != String.Empty)
                                    {
                                        DataTable Dts = new DataTable();
                                        String St1 = "Select * From ACCOUNTS.dbo.Blocked_Ledgers_List(2) Where Ledger_Code= " + TxtBuyer.Tag.ToString() + "";
                                        MyBase.Load_Data(St1, ref Dts);
                                        if (Dts.Rows.Count > 0)
                                        {
                                            MessageBox.Show("This Ledger Has Been Blocked By Accounts...!");
                                            Entry_New();
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        else if (this.ActiveControl.Name == "TxtPrjType")
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Type", " Select Name, Short_NAme, Rowid From Project_Name_Master ", String.Empty, 400, 100);
                            if (Dr != null)
                            {
                                TxtPrjType.Text = Dr["Name"].ToString();
                                TxtPrjType.Tag = Dr["Rowid"].ToString();
                                

                                

                                DataTable TDtp = new DataTable();
                                MyBase.Load_Data(" Select  Short  From Buyer_All_Fn() Where  LEdgeR_code = " + MyParent.Proj_Login_Code  + " ", ref TDtp);
                                if (TDtp.Rows.Count > 0)
                                {
                                    SName = TDtp.Rows[0][0].ToString();
                                }
                                else
                                {
                                    SName = "PRJ";
                                }


                                DataTable TDt1 = new DataTable();
                                MyBase.Load_Data("Select Substring(Max(OrdeR_No), 1, 7) A, Substring(Max(OrdeR_No), Len(Max(OrdeR_No))-3, 7) + 1 B , Max(OrdeR_No) OrdeR_No From(Select Max(OrdeR_No) OrdeR_No From Project_ORder_MASter Where Company_Code = " + MyParent.CompCode + " and PArty_Code = " +  MyParent.Proj_Login_Code + "   )A having Substring(Max(OrdeR_No), 1, 7)  is not null", ref TDt1);
                                if (TDt1.Rows.Count > 0)
                                {
                                    TxtPrjNo.Text = TDt1.Rows[0][0].ToString() + String.Format("{0:0000}", Convert.ToDouble(TDt1.Rows[0][1]));
                                }
                                else
                                {
                                    if (MyParent.CompCode == 1)
                                    {
                                        TxtPrjNo.Text = "GUP-" + SName + "0001";
                                    }
                                    else if (MyParent.CompCode == 2)
                                    {
                                        TxtPrjNo.Text = "ALM-" + SName + "0001";
                                    }
                                    else if (MyParent.CompCode == 3)
                                    {
                                        TxtPrjNo.Text = "IRL-" + SName + "0001";
                                    }
                                    else if (MyParent.CompCode == 8)
                                    {
                                        TxtPrjNo.Text = "GUT-" + SName + "0001";
                                    }
                                    else
                                    {
                                        TxtPrjNo.Text = "GUP-" + SName + "0001";
                                    }
                                }

                            }
                        }

                        else if (this.ActiveControl.Name == "TxtEmployee")
                        {
                            if (TxtBuyer.Text.Trim() == String.Empty)
                            {
                                MessageBox.Show("Invalid Buyer", "Gainup");
                                TxtBuyer.Focus();
                                return;
                            }
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Merch", "Select NAme, Tno, EmplNO From Vaahini_erp_Gainup.Dbo.Employeemas Where Tno not Like '%Z%' and TNo like '%A%'  ORder by tNo, Name  ", String.Empty, 250, 150);
                            if (Dr != null)
                            {
                                TxtEmployee.Text = Dr["Name"].ToString();
                                TxtEmployee.Tag = Dr["Emplno"].ToString();
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PROJ_ACTIVITY"].Index)
                    {
                        if (Grid["PROJ_ACTIVITY", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            //MessageBox.Show("Invalid PROJ_ACTIVITY", "Gainup");
                            //Grid.CurrentCell = Grid["PROJ_ACTIVITY", Grid.CurrentCell.RowIndex];
                            //Grid.Focus();
                            //Grid.BeginEdit(true);
                            //e.Handled = true;
                            //return;
                        }
                        else
                        {
                              if(Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                                {
                                    if (Grid.Rows.Count > 2 && Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex - 1].Value.ToString() != String.Empty)
                                    {
                                        Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value = Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex - 1].Value;
                                    }
                                    else
                                    {
                                        Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", DtpODate.Value));
                                    }
                                }
                              //if (Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                              //{
                              //    if (Grid.Rows.Count > 2 && Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex - 1].Value.ToString() != String.Empty)
                              //    {
                              //        Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value = Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex - 1].Value;
                              //    }
                              //    else
                              //    {
                              //        Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", DtpODate.Value));
                              //    }
                              //}
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ESTIMATE_DATE"].Index)
                    {
                        if (Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value = MyBase.Get_Date_Format(Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value.ToString());
                            if (Convert.ToDateTime(Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value) > Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", DtpODate.Value)))
                                {
                                    MessageBox.Show("Invalid ESTIMATE_DATE", "Gainup");
                                    Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", DtpODate.Value));
                                    Grid.CurrentCell = Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    e.Handled = true;
                                    return;
                                }                               
                                else if(Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                                {
                                    if (Grid.Rows.Count > 2 && Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex - 1].Value.ToString() != String.Empty)
                                    {
                                        Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value = Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex - 1].Value;
                                    }
                                    else
                                    {
                                        Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value = Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value;
                                    }
                                }
                        }
                    }

                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["COMPLETE_DATE"].Index)
                    {
                        if (Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            if (Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value = MyBase.Get_Date_Format(Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value.ToString());
                                if (Convert.ToDateTime(Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value) > Convert.ToDateTime(Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value))
                                {
                                    MessageBox.Show("Invalid Date", "Gainup");
                                    Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value);
                                    Grid.CurrentCell = Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    e.Handled = true;
                                    return;
                                }                                                               
                            }
                            else
                            {
                                MessageBox.Show("Invalid COMPLETE_DATE", "Gainup");
                                Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Grid["ESTIMATE_DATE", Grid.CurrentCell.RowIndex].Value);
                                Grid.CurrentCell = Grid["COMPLETE_DATE", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    e.Handled = true;
                                    return;
                            }
                        }                       
                }                
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["QTY"].Index)
                {
                    if (Grid["QTY", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                     {
                         MessageBox.Show("Invalid QTY", "Gainup");
                         Grid["QTY", Grid.CurrentCell.RowIndex].Value = 0;
                         Grid["CONV_QTY", Grid.CurrentCell.RowIndex].Value = 0;
                         Grid.CurrentCell = Grid["QTY", Grid.CurrentCell.RowIndex];
                         Grid.Focus();
                         Grid.BeginEdit(true);
                         e.Handled = true;
                         return;                             
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
                         Grid["CONV_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["QTY", Grid.CurrentCell.RowIndex].Value) * 1));
                         Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["QTY", Grid.CurrentCell.RowIndex].Value)));
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
                        else if (Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) < 0 || Convert.ToDouble(Grid["ALLOW_PER", Grid.CurrentCell.RowIndex].Value) > 8)
                        {
                            MessageBox.Show("Invalid Allowance Qty, Allowance % Must Between 0 To 8 ..!", "Gainup");
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
                         Grid["CONV_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["QTY", Grid.CurrentCell.RowIndex].Value) * 1));
                         Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["QTY", Grid.CurrentCell.RowIndex].Value)));
                     }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["RATE"].Index)
                {
                    if(Grid["RATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                     { 
                         MessageBox.Show("Invalid RATE", "Gainup");
                         Grid["RATE", Grid.CurrentCell.RowIndex].Value = 0.00; 
                         Grid.CurrentCell = Grid["RATE", Grid.CurrentCell.RowIndex];
                         Grid.Focus();
                         Grid.BeginEdit(true);
                         e.Handled = true;
                         return;                             
                     }
                    else if (Grid["QTY", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["QTY", Grid.CurrentCell.RowIndex].Value = "0";
                    }
                    else
                    {
                        Grid["CONV_QTY", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["QTY", Grid.CurrentCell.RowIndex].Value) * 1));
                        Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = ((Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["QTY", Grid.CurrentCell.RowIndex].Value)));
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["IMAGE_REQ"].Index)
                {
                    if(Grid["IMAGE_REQ", Grid.CurrentCell.RowIndex].Value.ToString() == "Y")
                     {
                         if (MyParent.View == true || MyParent.Edit == true)
                         {
                              DataTable TDtp = new DataTable();
                              MyBase.Load_Data(" Select Image1 FRom  VAAHINI_GAINUP_PHOTO.dbo.Project_Order_Image Where MAsteR_ID = " + Code + " and SNo = " + Grid["SNO", Grid.CurrentCell.RowIndex].Value.ToString() + "  ", ref TDtp);
                              if (TDtp.Rows.Count > 0)
                              {
                                  Grid["IMAGE1", Grid.CurrentCell.RowIndex].Value = TDtp.Rows[0][0];
                              }
                         }
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
                Grid.CurrentCell = Grid["QTY", Grid.CurrentCell.RowIndex];
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
                Grid.CurrentCell = Grid["QTY", Grid.CurrentCell.RowIndex];
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
                        if ((Grid["PROJ_ACTIVITY", i].Value.ToString()) == Grid["PROJ_ACTIVITY", j].Value.ToString())
                        {
                            MessageBox.Show("Already PROJ_ACTIVITY Available", "Gainup");
                                Grid["REFNO", j].Value = "";
                                Grid["PROJ_ACTIVITY", j].Value = "";
                                Grid["PROJ_ACTIVITY_ID", j].Value = "0"; 
                                Grid["RATE", j].Value = "0.00";
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
                 TxtEmployee.Focus();
                 SendKeys.Send("{Down}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Grid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Arrow_Name_Click(object sender, EventArgs e)
        {
            try
            {
                TxtPrjType.Focus();
                SendKeys.Send("{Down}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void BtnUpd_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyParent.View == true)
                {

                    for (int i = 0; i < Grid.Rows.Count - 1; i++)
                    {
                        if (Grid["T", i].Value.ToString() == "Y")
                        {
                            MyBase.Run("Insert into Project_ORdeR_Details (MAsteR_ID, Sno, RefNo, Estimate_Date, Complete_Date, Uom_ID,  Qty, Allow_Per, Conv_Qty, Rate, Amount, IMAGE_REQ, Proj_Activity_ID) Values (" + Code + ", " + Grid["SNO", i].Value + ", '" + Grid["REFNO", i].Value + "',  '" + String.Format("{0:dd-MMM-yyyy}", Grid["ESTIMATE_DATE", i].Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", Grid["COMPLETE_DATE", i].Value) + "',  " + (Grid["UOM_ID", i].Value) + ", " + Convert.ToDouble(Grid["QTY", i].Value) + ", " + Grid["ALLOW_PER", i].Value + ", " + Convert.ToDouble(Grid["CONV_QTY", i].Value) + ", " + Convert.ToDouble(Grid["RATE", i].Value) + ", " + Convert.ToDouble(Grid["AMOUNT", i].Value) + ", 'N',  " + Grid["PROJ_ACTIVITY_ID", i].Value + ")");
                        }
                    }
                    MessageBox.Show("Updated", "Gainup");
                    MyBase.Clear(this);
                    Entry_View();

                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
        private void ButFDel_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (MyParent.View == true)
                {
                    if (MyParent.UserCode == 1)
                    {
                        MyBase.Run("Delete from Project_Order_Details  where MAsteR_ID = " + Code + " and Rowid in (select  B.Rowid from Project_Order_Master A Left Join Project_Order_details B On A.Rowid= B.Master_id Left Join Project_PLanning_Master C On B.Proj_Activity_ID = C.PRoj_Activity_ID and A.Rowid  =C.Order_ID where C.Rowid is null and B.MAsteR_ID = " + Code + ")");
                        MessageBox.Show("Deleted", "Gainup");
                        MyBase.Clear(this);
                        Entry_View();
                    }
                    else
                    {
                        MessageBox.Show("Only Admin can delete the Activity", "Gainup");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
