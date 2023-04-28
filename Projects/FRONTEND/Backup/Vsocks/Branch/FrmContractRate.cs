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
    public partial class FrmContractRate : Form, Entry 
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        DataTable Dt = new DataTable();
        DataRow Dr;
        Int16 PCompCode;
        Int64 Code;
        String[] Queries;
        String Str;
        TextBox Txt = null;
        public FrmContractRate()
        {
            InitializeComponent();
        }


        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                TxtProcess.Focus();
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
                Total();
                if (TxtProcess.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Process No", "Gainup");
                    TxtProcess.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
               

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 1; j++)
                    {                        
                        if (Grid[j, i].Value == DBNull.Value)
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

                if (TxtTotal.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotal.Text) == 0 || TxtTotal.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotal.Text) == 0)
                {
                    MessageBox.Show("Invalid Rate", "Gainup");
                    TxtTotal.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                Queries = new String[Dt.Rows.Count + 3];
                if (MyParent.Edit == true )
                {
                    Queries[Array_Index++] = "Delete From Vaahini_Erp_Gainup.Dbo.Contract_Process_Rate Where Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' and Process_ID =  " + TxtProcess.Tag + " and Company_Code = " + MyParent.CompCode + "";
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Grid["RATE", i].Value) > 0)
                    {
                        Queries[Array_Index++] = "Insert Into Vaahini_Erp_Gainup.Dbo.Contract_Process_Rate (Effect_From, Process_ID, Rate, Weight_Code, Company_Code) Values ('" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "', " + TxtProcess.Tag + ",  " + Grid["Rate", i].Value + ",  " + Grid["Weight_Code", i].Value + ", " + MyParent.CompCode + ")";
                    }
                }
                MyBase.Run(Queries);                
                MyParent.Save_Error = false;
                MessageBox.Show("Saved ..!", "Gainup");                
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Contract Rate Details - Edit", " Select Distinct B.Name Process_Name, A.Effect_From , A.Process_ID  From Vaahini_Erp_Gainup.Dbo.Contract_Process_Rate A  Inner join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name B On A.Process_Id = B.RowID Where A.Approval_flag = 'F' and  A.Company_Code= " + MyParent.CompCode + "  and A.Process_ID in (12) ORder by A.Effect_From DEsc, B.Name ", string.Empty, 140, 120);
                if (Dr != null)
                {
                    Fill_Datas(Dr);                    
                    TxtTotal.Focus();
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
                DtpEDate.Value = Convert.ToDateTime(Dr["Effect_From"]);
                TxtProcess.Text = Dr["Process_Name"].ToString();
                TxtProcess.Tag = Dr["Process_ID"].ToString();
                Grid_Data();                              
            }
            catch (Exception ex)
            {
                throw ex;
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
                    Txt.Leave += new EventHandler(Txt_Leave);
                }
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["RATE"].Index)
                    {
                        if (Grid["RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) == 0)
                        {
                            Grid["RATE", Grid.CurrentCell.RowIndex].Value = "0";
                        }                        
                    }
                Total();
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["NAME"].Index)
                    {
                        if (TxtProcess.Tag.ToString() == "1" || TxtProcess.Tag.ToString() == "9")
                        {
                            Dr = Tool.Selection_Tool_Except_New("NAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "NAME", "Select Weight NAME, Code WEIGHT_CODE From CountWeight (" + MyParent.CompCode + ") ", string.Empty, 200);
                        }
                        else if (TxtProcess.Tag.ToString() == "3" || TxtProcess.Tag.ToString() == "4" || TxtProcess.Tag.ToString() == "5" || TxtProcess.Tag.ToString() == "7" || TxtProcess.Tag.ToString() == "8" || TxtProcess.Tag.ToString() == "10" || TxtProcess.Tag.ToString() == "13")
                        {
                            Dr = Tool.Selection_Tool_Except_New("NAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "NAME", "Select Name , CAse When " + MyParent.CompCode + " = 3 Then 25 Else 21 End Weight_Code From Contract_Process_Name  Where RowID  = " + TxtProcess.Tag + " ", string.Empty, 250);
                        }
                        else if (TxtProcess.Tag.ToString() == "6")
                        {
                            Dr = Tool.Selection_Tool_Except_New("NAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "NAME", "Select StoppageNAme Name, StoppageCode Weight_Code From MasStoppage Where compcode = Case When " + MyParent.CompCode + " = 3 Then 5 Else " + MyParent.CompCode + " End and StoppageName Not Like '%ZZZ%' and StopTypeCode  = 3 Order by StoppageName  ", string.Empty, 200);
                        }
                        else if (TxtProcess.Tag.ToString() == "12")
                        {
                            Dr = Tool.Selection_Tool_Except_New("NAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "NAME", "Select  Name, RowID Weight_Code From Vaahini_Erp_Gainup.Dbo.Loading_Item_Name_Master Order by Name  ", string.Empty, 200);
                        }
                        else
                        {
                            Dr = Tool.Selection_Tool_Except_New("NAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select A.Name COUNTNAME, 0.000 PENDING, 0.000 PACKED, 0.000 KGS, 0.000 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight,  '' CountType, A.RowID  countcode, 0 TypeCode, 1.00 WEIGHT, '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' PRODDATE  From Vaahini_Erp_Gainup.Dbo.Waste_Item_Master A Inner Join Vaahini_Erp_Gainup.Dbo.CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 Left join Vaahini_Erp_Gainup.Dbo.Packing_Details B On A.RowID = B.CountCode and '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' = B.ProdDate LEft join Vaahini_Erp_Gainup.Dbo.Packing_Master C On B.Master_ID = C.RowID and C.Proc_Type in (3,4) Where C.RowID is Null", string.Empty, 200, 100);
                        }
                        if (Dr != null)
                        {
                            Grid["NAME", Grid.CurrentCell.RowIndex].Value = Dr["NAME"].ToString();
                            Grid["RATE", Grid.CurrentCell.RowIndex].Value = 0.00;
                            Grid["WEIGHT_CODE", Grid.CurrentCell.RowIndex].Value = Dr["WEIGHT_CODE"].ToString();
                            Txt.Text = Dr["NAME"].ToString();
                        }
                    }                    
                }
                Total();
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
                //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PACKED"].Index)
                //{
                //    if (Grid["PACKED", Grid.CurrentCell.RowIndex].Value == null || Grid["PACKED", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["PACKED", Grid.CurrentCell.RowIndex].Value) == 0)
                //    {
                //        Grid["PACKED", Grid.CurrentCell.RowIndex].Value = "0";
                //    }
                //    else
                //    {
                //        if (Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0)
                //        {
                //            Grid["PACKED", Grid.CurrentCell.RowIndex].Value = "0.00";
                //        }
                //        else
                //        {
                //            if (Grid["PACKED", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["PENDING", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                //            {
                //                Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["KGS", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value));
                //            }
                //        }
                //    }
                //}
                Total();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Total()
        {
            Double Amt = 0.0;
            Double TaxAmt = 0.0;
            try
            {               
                TxtTotal.Text = MyBase.Count (ref Grid, "RATE", "NAME");                
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["RATE"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
                }
                else
                {
                    MyBase.Valid_Null(Txt, e);
                }
                Total();
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

        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentRow.Index);
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
                    //if (TxtProcess.Tag.ToString() != "3" && TxtProcess.Tag.ToString() != "4")
                    //{
                    //    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PACKED"].Index)
                    //    {
                    //        if (Grid["PENDING", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                    //        {
                    //            if (Convert.ToDouble(Grid["PACKED", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["PENDING", Grid.CurrentCell.RowIndex].Value))
                    //            {
                    //                MessageBox.Show("Invalid Packed..!", "Gainup");
                    //                Grid["PACKED", Grid.CurrentCell.RowIndex].Value = 0.00;
                    //                Grid["KGS", Grid.CurrentCell.RowIndex].Value = 0.00;
                    //                Grid.CurrentCell = Grid["PACKED", Grid.CurrentCell.RowIndex];
                    //                Grid.Focus();
                    //                Grid.BeginEdit(true);
                    //                e.Handled = true;
                    //                return;
                    //            }
                    //        }
                    //    }
                    //}
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
                    Total();
                    TxtTotal.Focus();
                    return;
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
                if (Convert.ToDateTime(DtpEDate.Value) > MyBase.GetServerDateTime())
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DtpEDate.Value = MyBase.GetServerDateTime();
                    TxtProcess.Text = "";
                    DtpEDate.Focus();                    
                    return;
                }
                else if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(DtpEDate.Value), MyBase.GetServerDateTime()) > 0 && MyParent.UserCode != 1)
                {
                    MessageBox.Show("Date Locked", "Gainup");
                    DtpEDate.Value = MyBase.GetServerDateTime();
                    TxtProcess.Text = "";
                    DtpEDate.Focus();                    
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmContractRate_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);               
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
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Contract Rate Details - Delete", " Select Distinct B.Name Process_Name, A.Effect_From , A.Process_ID  From Vaahini_Erp_Gainup.Dbo.Contract_Process_Rate A  Inner join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name B On A.Process_Id = B.RowID Where A.Approval_flag = 'F' and A.Company_Code = " + MyParent.CompCode + "   and A.Process_ID in (12) ORder by Effect_From DEsc, B.Name ", string.Empty, 140, 120);
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
                    MyBase.Run("Delete From Vaahini_Erp_Gainup.Dbo.Contract_Process_Rate Where Effect_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' and Process_ID =  " + TxtProcess.Tag + " and Company_Code = " + MyParent.CompCode + "");
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
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Contract Rate Details - View", " Select Distinct B.Name Process_Name, A.Effect_From , A.Process_ID  From Vaahini_Erp_Gainup.Dbo.Contract_Process_Rate A  Inner join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name B On A.Process_Id = B.RowID Where A.Company_Code =" + MyParent.CompCode + "  and A.Process_ID in (12) ORder by Effect_From DEsc, B.Name ", string.Empty, 140, 120);
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
                    Str = "Select 0 SNO, '' NAME, 0.00 RATE,  0 WEIGHT_CODE  From Vaahini_Erp_Gainup.Dbo.TrnPrdSum A Where 1 = 2";                   
                }
                else
                {
                    //if (TxtProcess.Tag.ToString() == "1" || TxtProcess.Tag.ToString() == "9")
                    //{
                    //    Str = "Select 0 SNO, B.Weight NAME, A.RATE, A.WEIGHT_CODE From Contract_Process_Rate A Inner Join CountWeight(" + MyParent.CompCode + ") B On A.Weight_Code = B.Code Where A.Process_ID = " + TxtProcess.Tag + " and A.Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' and A.Company_Code = " + MyParent.CompCode + "";
                    //}
                    //else if (TxtProcess.Tag.ToString() == "3" || TxtProcess.Tag.ToString() == "4" || TxtProcess.Tag.ToString() == "5" || TxtProcess.Tag.ToString() == "7" || TxtProcess.Tag.ToString() == "8" || TxtProcess.Tag.ToString() == "13")
                    //{
                    //    Str = "Select 0 SNO, B.Name NAME, A.Rate RATE, A.WEIGHT_CODE  From Contract_Process_Rate A Inner Join Contract_Process_Name  B On A.Process_ID = B.RowID  Where A.Process_ID = " + TxtProcess.Tag + " and A.Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' and A.Company_Code = " + MyParent.CompCode + "";
                    //}
                    //else if (TxtProcess.Tag.ToString() == "6")
                    //{
                    //    Str = "Select  0 SNO, B.StoppageName NAME, A.Rate RATE, A.WEIGHT_CODE  From Contract_Process_Rate A Inner Join MasStoppage  B On A.Weight_Code = B.StoppageCode Where A.Process_ID = " + TxtProcess.Tag + " and A.Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' and A.Company_Code = " + MyParent.CompCode + "";
                    //}
                    //else
                    //{
                    //    Str = "Select 0 SNO, B.Name NAME, A.Rate RATE, A.WEIGHT_CODE  From Vaahini_Erp_Gainup.Dbo.Contract_Process_Rate A Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name  B On A.Process_ID = B.RowID  Where A.Process_ID = " + TxtProcess.Tag + " and A.Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' and A.Company_Code = " + MyParent.CompCode + "";
                    //}
                    if (TxtProcess.Tag.ToString() == "1" || TxtProcess.Tag.ToString() == "9")
                    {
                        Str = "Select 0 SNO, B.Weight NAME, A.RATE, A.WEIGHT_CODE From Vaahini_Erp_Gainup.Dbo.Contract_Process_Rate A Inner Join Vaahini_Erp_Gainup.Dbo.CountWeight(" + MyParent.CompCode + ") B On A.Weight_Code = B.Code Where A.Process_ID = " + TxtProcess.Tag + " and A.Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' and A.Company_Code = " + MyParent.CompCode + "";
                    }
                    else if (TxtProcess.Tag.ToString() == "3" || TxtProcess.Tag.ToString() == "4" || TxtProcess.Tag.ToString() == "5" || TxtProcess.Tag.ToString() == "7" || TxtProcess.Tag.ToString() == "8" || TxtProcess.Tag.ToString() == "10" || TxtProcess.Tag.ToString() == "13")
                    {
                        Str = "Select 0 SNO, B.Name NAME, A.Rate RATE, A.WEIGHT_CODE  From Vaahini_Erp_Gainup.Dbo.Contract_Process_Rate A Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name  B On A.Process_ID = B.RowID  Where A.Process_ID = " + TxtProcess.Tag + " and A.Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' and A.Company_Code = " + MyParent.CompCode + "";
                    }
                    else if (TxtProcess.Tag.ToString() == "6")
                    {
                        Str = "Select 0 SNO, B.StoppageName NAME, A.Rate RATE, A.WEIGHT_CODE  From Vaahini_Erp_Gainup.Dbo.Contract_Process_Rate A Inner Join Vaahini_Erp_Gainup.Dbo.MasStoppage  B On A.Weight_Code = B.StoppageCode Where A.Process_ID = " + TxtProcess.Tag + " and A.Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' and A.Company_Code = " + MyParent.CompCode + "";
                    }
                    else if (TxtProcess.Tag.ToString() == "12")
                    {
                        Str = "Select 0 SNO, B.NAME, A.Rate RATE, A.WEIGHT_CODE  From Vaahini_Erp_Gainup.Dbo.Contract_Process_Rate A Inner Join Vaahini_Erp_Gainup.Dbo.Loading_Item_Name_Master  B On A.Weight_Code = B.RowID Where A.Process_ID = " + TxtProcess.Tag + " and A.Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' and A.Company_Code = " + MyParent.CompCode + "";
                    }
                    else
                    {
                        Str = "Select 0 SNO, B.StoppageName NAME, A.Rate RATE, A.WEIGHT_CODE  From Vaahini_Erp_Gainup.Dbo.Contract_Process_Rate A Inner Join Vaahini_Erp_Gainup.Dbo.MasStoppage  B On A.Weight_Code = B.StoppageCode Where A.Process_ID = " + TxtProcess.Tag + " and A.Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "'  and A.Company_Code = " + MyParent.CompCode + "";
                    }
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid, "NAME", "RATE");
                MyBase.Grid_Designing(ref Grid, ref Dt, "WEIGHT_CODE");
                MyBase.Grid_Width(ref Grid, 50, 250, 120);
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                
                Grid.Columns["NAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;                
                Grid.Columns["RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmContractRate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {                   
                    if (this.ActiveControl.Name == "TxtProcess")
                    {
                        Grid.CurrentCell = Grid["NAME", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                    else if (this.ActiveControl.Name == "TxtTotal")
                    {
                        Total();
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                        return;
                    }                             
                        SendKeys.Send("{Tab}");                   
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtProcess")
                    {
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Process ", " Select Name , RowID  From Vaahini_Erp_Gainup.Dbo.Contract_Process_Name Where Rowid in (12) ", string.Empty, 250);
                        if (Dr != null)
                        {
                            TxtProcess.Text = Dr["Name"].ToString();
                            TxtProcess.Tag = Dr["RowID"].ToString();
                            Grid_Data();
                            TxtProcess.Focus();
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

        private void FrmContractRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtRemarks" || this.ActiveControl.Name == String.Empty )
                    {
                        MyBase.Return_Ucase(e);                        
                    }
                    else if (this.ActiveControl.Name == "TxtDeduct" )
                    {
                        MyBase.Valid_Decimal((TextBox)this.ActiveControl, e);
                    }
                    else
                    {
                        MyBase.Valid_Null((TextBox)this.ActiveControl, e);
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