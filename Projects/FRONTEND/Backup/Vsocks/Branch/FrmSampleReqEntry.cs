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
    public partial class FrmSampleReqEntry : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataGrid dgv = new DataGrid();
        DataRow Dr;
        Int64 Code;
        Int32 C = 0;
        TextBox Txt = null;
        TextBox Txt_Img = null;
        DataTable[] DtImg;
        String[] Queries;
        String Str;
        public FrmSampleReqEntry()
        {
            InitializeComponent();
        }
        private void Frm_Socks_Dyeing_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                TxtBuyer.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Frm_Socks_Dyeing_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    
                    if (this.ActiveControl.Name == "DtpRDate")
                    {
                        TxtMerch.Focus();
                    }
                    else if (this.ActiveControl.Name == "TxtMerch")
                    {
                        if (TxtMerch.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Select Merch..!", "Gainup");
                            return;
                        }
                        else
                        {
                            TxtBuyer.Focus();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtBuyer")
                    {
                        if (TxtBuyer.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Select Buyer..!", "Gainup");
                            return;
                        }
                        else
                        {
                            Grid.CurrentCell = Grid["Product_No", 0];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                            
                        }
                    }                    
                    else if (this.ActiveControl.Name == "TxtRemarks")
                    {
                        TxtTotalBom.Focus();
                    }
                    else if (this.ActiveControl.Name == "TxtTotalBom")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }

                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtMerch")
                    {

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Merch..!", "select Distinct A.employee Merch, A.employeeid Merchid from employee A Left Join designation B on A.designationid = B.designationid Left Join department  C on B.DepartmentId = C.departmentid Where C.departmentid=27 Order By A.employee", String.Empty, 300);

                        if (Dr != null)
                        {
                            TxtMerch.Text = Dr["Merch"].ToString();
                            TxtMerch.Tag = Dr["Merchid"].ToString();

                        }
                    }
                    else if (this.ActiveControl.Name == "TxtBuyer")
                    {

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer..!", "select B.Ledger_Name Buyer, B.Ledger_Code Buyerid from Buyer A Inner Join Accounts.Dbo.Ledger_Master B on A.Acc_Ledger_Code = B.Ledger_Code And B.Company_Code=1 And Year_Code=dbo.Get_Accounts_YearCode(getdate()) Where A.Acc_Ledger_Code>0 Order By B.ledger_Name", String.Empty, 300);

                        if (Dr != null)
                        {
                            TxtBuyer.Text = Dr["Buyer"].ToString();
                            TxtBuyer.Tag = Dr["Buyerid"].ToString();

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
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                DtpODate.Value = MyBase.GetServerDate();
                MyBase.Date_Control(ref DtpRDate, 2);
                Grid_Data();
                DataTable TDt = new DataTable();
                MyBase.Load_Data("Select Substring(Max(OrdeR_No), 1, 7) A, Substring(Max(OrdeR_No), Len(Max(OrdeR_No))-4, 7) + 1 B , Max(OrdeR_No) Order_No From(Select Max(OrdeR_No) OrdeR_No From VSocks_Sample_Req_Master Union All Select Max(Order_No)  Order_No from sample_ord_mas)A ", ref TDt);
                TxtOCNNo.Text = TDt.Rows[0][0].ToString() + String.Format("{0:00000}", Convert.ToDouble(TDt.Rows[0][1]));
                DtpRDate.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Entry_Cancel()
        {
            MyBase.Clear(this);
        }
        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);                
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Order No - Edit", " select A.Order_No, A.EDate, A.Req_Date, F.Buyer, G.employee Merch, C.Item, D.Color, E.Size, B.Req_Qty, B.rate, A.Remarks, A.Rowid, A.Buyerid, A.Merchid from VSocks_Sample_Req_Master A Left Join VSocks_Sample_Req_Details B on A.Rowid = B.Master_ID Left Join Item C on B.Itemid = C.Itemid Left Join Color D on B.Colorid = D.Colorid Left Join Size E on B.Sizeid = E.Sizeid Left Join Buyer F on A.Buyerid = F.Acc_Ledger_Code Left Join Employee G on A.Merchid = G.employeeid ", String.Empty, 120, 100, 75, 175, 125, 120, 120, 75, 100, 100, 150);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Product_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
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
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Order No - View", " select A.Order_No, A.EDate, A.Req_Date, F.Buyer, G.employee Merch, C.Item, D.Color, E.Size, B.Req_Qty, B.rate, A.Remarks, A.Rowid, A.Buyerid, A.Merchid from VSocks_Sample_Req_Master A Left Join VSocks_Sample_Req_Details B on A.Rowid = B.Master_ID Left Join Item C on B.Itemid = C.Itemid Left Join Color D on B.Colorid = D.Colorid Left Join Size E on B.Sizeid = E.Sizeid Left Join Buyer F on A.Buyerid = F.Acc_Ledger_Code Left Join Employee G on A.Merchid = G.employeeid  ", String.Empty, 120, 100, 75, 175, 125, 120, 120, 75, 100, 100, 150);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Product_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
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
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Order No - Delete", " select A.Order_No, A.EDate, A.Req_Date, F.Buyer, G.employee Merch, C.Item, D.Color, E.Size, B.Req_Qty, B.rate, A.Remarks, A.Rowid, A.Buyerid, A.Merchid from VSocks_Sample_Req_Master A Left Join VSocks_Sample_Req_Details B on A.Rowid = B.Master_ID Left Join Item C on B.Itemid = C.Itemid Left Join Color D on B.Colorid = D.Colorid Left Join Size E on B.Sizeid = E.Sizeid Left Join Buyer F on A.Buyerid = F.Acc_Ledger_Code Left Join Employee G on A.Merchid = G.employeeid  ", String.Empty, 120, 100, 75, 175, 125, 120, 120, 75, 100, 100, 150);
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
                    MyBase.Run("Delete from VSocks_Sample_Req_Details where Master_ID = " + Code, "Delete from VSocks_Sample_Req_Master where Rowid = " + Code, MyParent.EntryLog("Sample Requirement Entry", "DELETE", Code.ToString()));
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
        public void Entry_Print()
        {
            try
            {   

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
                String From_Store = String.Empty;
                Total_Count();

                decimal Sum = 0;                

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Details..!", "Gainup");
                    Grid.CurrentCell = Grid["Yarn", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtTotalBom.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotalBom.Text.ToString()) == 0)
                {
                    MessageBox.Show("Invalid Details", "Gainup");
                    Grid.CurrentCell = Grid["Yarn", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Req_Qty", i].Value == DBNull.Value || Grid["Req_Qty", i].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Req_Qty", i].Value) == 0.000)
                    {
                        MessageBox.Show(" ZERO Qty is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Req_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                    if (Grid["Rate", i].Value == DBNull.Value || Grid["Rate", i].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Rate", i].Value) == 0.00)
                    {
                        MessageBox.Show(" Rate is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Rate", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }

               // TxtOCNNo.Text = MyBase.MaxOnlyComp("VSocks_Sample_Req_Master", "Order_No", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();
                Queries = new string[Dt.Rows.Count * 100];

                if (MyParent._New)
                {
                    DataTable TDt = new DataTable();
                    MyBase.Load_Data("Select Substring(Max(OrdeR_No), 1, 7) A, Substring(Max(OrdeR_No), Len(Max(OrdeR_No))-4, 7) + 1 B , Max(OrdeR_No) Order_No From(Select Max(OrdeR_No) OrdeR_No From VSocks_Sample_Req_Master Union All Select Max(Order_No)  Order_No from sample_ord_mas)A ", ref TDt);
                    TxtOCNNo.Text = TDt.Rows[0][0].ToString() + String.Format("{0:00000}", Convert.ToDouble(TDt.Rows[0][1]));                    

                    if (TxtOCNNo.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Invalid Order No", "Gainup");
                        TxtOCNNo.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    Queries[Array_Index++] = "Insert into VSocks_Sample_Req_Master (Order_No, EDate, Buyerid, Merchid, Req_Date, Remarks,  Company_Code, Year_Code, User_Code) values ('" + TxtOCNNo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "', " + TxtBuyer.Tag + ", " + TxtMerch.Tag + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpRDate.Value) + "', '" + TxtRemarks.Text + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "'," + MyParent.UserCode + "); Select Scope_Identity() ";
                    //Queries[Array_Index++] = MyParent.EntryLog("Yarn Dyeing Entry", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update VSocks_Sample_Req_Master Set EDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "', Req_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpRDate.Value) + "', Merchid = " + TxtMerch.Tag + " , Buyerid = " + TxtBuyer.Tag + ",  Remarks = '" + TxtRemarks.Text + "',Company_Code=" + MyParent.CompCode + " , Year_Code='" + MyParent.YearCode + "',User_Code=" + MyParent.UserCode + " Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Sample Req Entry", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from VSocks_Sample_Req_Details where Master_ID = " + Code;                    
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into VSocks_Sample_Req_Details (Master_ID, Slno, Product_Id, ItemID, SizeID, ColorID, Req_Qty, Rate, Slno1) Values (@@IDENTITY, " + Grid["Slno", i].Value + ", " + Grid["Product_id", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ",  " + Grid["Req_qty", i].Value + ",  " + Grid["Rate", i].Value + ", " + Grid["Slno", i].Value + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into VSocks_Sample_Req_Details (Master_ID, Slno, Product_Id, ItemID, SizeID, ColorID, Req_Qty, Rate, Slno1) Values (" + Code + ", " + Grid["Slno", i].Value + ", " + Grid["Product_id", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ", " + Grid["Req_qty", i].Value + ",  " + Grid["Rate", i].Value + ", " + Grid["Slno", i].Value + ")";
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
                MyParent.Save_Error = false;
                MessageBox.Show("Saved ..!", "Gainup");
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }   
        void Fill_Datas(DataRow Dr)
        {
            try
            {
                Code = Convert.ToInt64(Dr["RowID"]);
                TxtMerch.Text = Dr["Merch"].ToString();                
                TxtBuyer.Text = Dr["Buyer"].ToString();
                TxtOCNNo.Text = Dr["Order_No"].ToString();
                DtpRDate.Value = Convert.ToDateTime(Dr["Req_date"]);  
                TxtRemarks.Text = Dr["Remarks"].ToString();
                TxtBuyer.Tag = Dr["Buyerid"].ToString();
                TxtMerch.Tag = Dr["Merchid"].ToString();
                DtpODate.Value = Convert.ToDateTime(Dr["Edate"]);                
                Grid_Data();
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Grid_Data()
        {
            String Str = String.Empty;
            try
            {
                if (MyParent._New == true)
                {
                    Str = "select 0 as Slno, '' Product_No, Item Yarn,  Color, Size Count, Sum(Req_Qty)Req_Qty, 0.00 Rate, 0 Product_Id, Itemid, Colorid, Sizeid, 0 Slno1, 0 RNo, '-' T  from FITSOCKS.dbo.Yarn_Dyeing_Requirement_Details() where 1=2 Group By Itemid, Item, Colorid, Color, Sizeid, Size";
                }
                else
                {
                    Str = "Select A.Slno, F.Product_No, C.Item Yarn, D.Color, E.Size Count, A.Req_Qty, A.Rate, A.Itemid, A.Colorid, A.Sizeid, A.Slno1, A.Product_Id,  ROW_NUMBER() Over (Order by A.Itemid, A.Colorid, A.Sizeid) RNo,'-' T  from  fitsocks.dbo.VSocks_Sample_Req_Details A  Left Join fitsocks.dbo.VSocks_Sample_Req_Master B on A.Master_ID = B.Rowid Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid  Left Join VFit_Sample_Product_Master F on A.Product_Id = F.RowID Where B.Rowid = " + Code + " Order By A.Slno";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Designing(ref Grid, ref Dt, "ItemID", "SizeID", "ColorID", "Product_Id", "Slno1", "RNo", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Product_No", "Req_Qty", "Rate");
                MyBase.Grid_Width(ref Grid, 50, 100, 150, 175, 125, 110, 110);
                Grid.Columns["Req_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Req_Qty"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index)
                    {
                        if (Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value == null || Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                        }

                        if (Grid["Rate", Grid.CurrentCell.RowIndex].Value == null || Grid["Rate", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Rate", Grid.CurrentCell.RowIndex].Value = "0.00";
                        }

                        if (Convert.ToDouble(Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value) == 0.000)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Req_Qty ...!", "Gainup");
                            Grid.CurrentCell = Grid["Req_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index)
                        {

                            if (Convert.ToDouble(Grid["Rate", Grid.CurrentCell.RowIndex].Value) == 0.00)
                            {
                                e.Handled = true;
                                MessageBox.Show("Invalid Rate ...!", "Gainup");
                                Grid.CurrentCell = Grid["Rate", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                        }
                        //Grid["Bill_Amount", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["Bill_Qty", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["Bill_Rate", Grid.CurrentCell.RowIndex].Value);
                        

                    }
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
                if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                    }
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
                    //Txt.Enter += new EventHandler(Txt_Enter);
                    Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                    Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);
                    Txt.Leave += new EventHandler(Txt_Leave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void Txt_Enter(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Yarn"].Index)
                {
                    MyBase.Row_Number(ref Grid);
                    Total_Count();
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
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Product_No"].Index)
                    {
                        if (TxtBuyer.Text != String.Empty && TxtMerch.Text != String.Empty)
                        {
                            Dr = Tool.Selection_Tool_Except_New("RNo", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Product", "Select V1.Product_No, I1.Item, C1.color, S1.size, 0.000 Req_Qty, 0.00 Rate, ROW_NUMBER() Over (Order by V1.Itemid, V1.Colorid, V1.Sizeid) RNo,  V1.ItemID, V1.ColorID, V1.SizeID, V1.RowID Product_ID From VFit_Sample_Product_Master V1 Left Join Item I1 On V1.ItemID = I1.itemid Left join Color C1 On V1.ColorID = C1.colorid Left join Size S1 On V1.SizeID = S1.Sizeid Where I1.Item Not Like 'ZZZ%' And C1.Color Not Like 'ZZZ%' And S1.Size Not Like 'ZZZ%' ", String.Empty, 100, 150, 150, 100, 100, 100);

                            if (Dr != null)
                            {
                                Txt.Text = Dr["Product_No"].ToString();
                                Grid["Product_id", Grid.CurrentCell.RowIndex].Value = Dr["Product_id"].ToString();
                                Grid["Product_No", Grid.CurrentCell.RowIndex].Value = Dr["Product_NO"].ToString();
                                Grid["Yarn", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                                Grid["Count", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                                Grid["Color", Grid.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                                Grid["itemid", Grid.CurrentCell.RowIndex].Value = Dr["Itemid"].ToString();
                                Grid["Sizeid", Grid.CurrentCell.RowIndex].Value = Dr["Sizeid"].ToString();
                                Grid["Colorid", Grid.CurrentCell.RowIndex].Value = Dr["Colorid"].ToString();
                                Grid["RNo", Grid.CurrentCell.RowIndex].Value = Dr["RNo"].ToString();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Buyer / Merch", "Gainup");
                            if (TxtBuyer.Text == String.Empty)
                            {
                                TxtBuyer.Focus();
                            }
                            else
                            {
                                TxtMerch.Focus();
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
        Int16 Max_Slno_Grid()
        {
            Int16 No = 0;
            try
            {
                if (Dt.Rows.Count == 0)
                {
                    No = 1;
                    return No;
                }
                else
                {
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (No < Convert.ToInt16(Dt.Rows[i]["Slno1"]))
                        {
                            No = Convert.ToInt16(Dt.Rows[i]["Slno1"]);
                        }
                    }
                }
                No += 1;
                return No;
            }
            catch (Exception ex)
            {
                return No;
            }
        }

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Req_Qty"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
                {

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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Req_Qty"].Index)
                {
                    if ((Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) == String.Empty)
                    {
                        MessageBox.Show("Invalid Req_Qty..!", "Gainup");
                        Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";                            
                        Txt.Text = "0.000";
                        Grid.CurrentCell = Grid["Req_Qty", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                        
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index)
                {
                    if ((Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString()) == String.Empty)
                    {
                        MessageBox.Show("Invalid Rate..!", "Gainup");
                        Grid["Rate", Grid.CurrentCell.RowIndex].Value = "0.000";
                        Grid.CurrentCell = Grid["Rate", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;                        
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
               TxtTotalBom.Text = MyBase.Sum(ref Grid, "REQ_QTY");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmSampleReqEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtBuyer" || this.ActiveControl.Name == "TxtMerch")
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = false;
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
