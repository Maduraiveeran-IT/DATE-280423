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
    public partial class FrmItemWiseRateEntry : Form
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

        public FrmItemWiseRateEntry()
        {
            InitializeComponent();
        }        

        void Total_Count()
        {               
            try
            {                               
                TxtTotalCount.Text = MyBase.Count(ref Grid, "PUR_RATE", "ITEM");
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["GRS_RATE"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["FREIGHT_RATE"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["TAX_PER"].Index)
                    {
                        if (Grid["GRS_RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["GRS_RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["GRS_RATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["GRS_RATE", Grid.CurrentCell.RowIndex].Value = "0.000";
                        }
                        if (Grid["FREIGHT_RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["FREIGHT_RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["FREIGHT_RATE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["FREIGHT_RATE", Grid.CurrentCell.RowIndex].Value = "0.000";
                        }
                        if (Grid["TAX_PER", Grid.CurrentCell.RowIndex].Value == null || Grid["TAX_PER", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["TAX_PER", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["TAX_PER", Grid.CurrentCell.RowIndex].Value = "0.00";
                        }                       
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmItemWiseRateEntry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                Grid_Data();                
                TxtMode.Focus();
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
                if (ChkOld.Checked == false)
                {                    
                    Str = "Select 0 SNO, ITEM, COLOR, SIZE, 0.000 GRS_RATE, 0.00 TAX_PER, 0.00 FREIGHT_RATE, 0.000 PUR_RATE, Item_ID, Color_ID, Size_ID, '' ITEM1 From  View_Socks_Items_For_Rate Where 1 = 2 ";
                }
                else
                {
                    Str = "Select 0 SNO, ITEM, COLOR, SIZE, GRS_RATE, TAX_PER, FREIGHT_RATE, PUR_RATE, Item_ID, Color_ID, Size_ID, ITEM + ' ' + COLOR + ' ' + SIZE ITEM1 From  Socks_ITem_Wise_Rate_All_Fn() Where EntryNo = " + TxtENo.Text.ToString() + " Order by 2,3,4";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);     
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid, "ITEM", "GRS_RATE", "TAX_PER", "FREIGHT_RATE");  
                MyBase.Grid_Designing(ref Grid, ref Dt, "Item_ID", "Color_ID", "Size_ID", "ITEM1");
                MyBase.Grid_Width(ref Grid, 50, 120, 120 , 100, 100, 80, 80, 120);
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                
                Grid.Columns["ITEM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["COLOR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["SIZE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["GRS_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["TAX_PER"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["FREIGHT_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["PUR_RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmItemWiseRateEntry_KeyDown(object sender, KeyEventArgs e)
        {
            String Str1 = "";
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name != "GRID")
                    {
                        e.Handled = true;
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
                {
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Down)
                {                    
                    if (this.ActiveControl.Name == "TxtMode" && Grid.Rows.Count <=1)
                    {
                        if (ChkOld.Checked == true)
                        {
                            Str = "Select  EntryNo, Effect_From, Mode, ITEM, COLOR, SIZE, GRS_RATE, TAX_PER, FREIGHT_RATE, PUR_RATE, Item_ID, Color_ID, Size_ID, ITEM + ' ' + COLOR + ' ' + SIZE ITEM1 From  Socks_ITem_Wise_Rate_All_Fn()  Order by 1, 2, 3, 4, 5, 6";
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Mode", Str, String.Empty, 110, 110, 110, 100, 100, 100, 100);
                        }
                        else
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Mode", "Select 'YARN' Mode Union Select 'TRIMS' Mode ", String.Empty, 250);
                        }                        
                        if (Dr != null)
                        {
                            TxtMode.Text = Dr["Mode"].ToString();
                            if (ChkOld.Checked == true)
                            {
                                TxtENo.Text = Dr["EntryNo"].ToString();
                                DtpDate.Value = Convert.ToDateTime(Dr["Effect_From"].ToString());
                            }
                            Grid_Data();
                            Grid.CurrentCell = Grid["ITEM",0];
                            Grid.Focus();
                            Grid.BeginEdit(true);
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

        private void FrmItemWiseRateEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtRemarks")
                    {
                        MyBase.Return_Ucase(e);
                    }
                    else if (this.ActiveControl.Name == "")
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

        private void DtpRDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void myTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void TxtPoNo_TextChanged(object sender, EventArgs e)
        {

        }

       
        private void GBMain_Enter(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
                TxtMode.Focus();
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
                TxtTotalCount.Text = MyBase.Count(ref Grid, "PUR_RATE", "ITEM");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ButSave_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 Array_Index = 0;
                Total();
                if (TxtMode.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Mode", "Gainup");
                    TxtMode.Focus();
                    MyParent.Save_Error = true;
                    return;
                }


                for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Grid.Columns.Count - 1; j++)
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

                if (TxtTotalCount.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotalCount.Text) == 0 || TxtTotalCount.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotalCount.Text) == 0)
                {
                    MessageBox.Show("Invalid Rate Details", "Gainup");
                    TxtTotalCount.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select IsNull(Max(EntryNo),0)  + 1 No From Socks_Item_Wise_Rate_Details  ", ref Tdt);
                TxtENo.Text = Tdt.Rows[0][0].ToString();

                Queries = new String[Grid.Rows.Count + 7];
                for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Grid["GRS_RATE", i].Value) > 0)
                    {
                        Queries[Array_Index++] = "Insert Into Socks_Item_Wise_Rate_Details (EntryNo, Effect_From, Mode, Item_ID, Color_ID, Size_ID, GRS_RATE, TAX_PER, FREIGHT_RATE, PUR_RATE) Values ('" + TxtENo.Text.ToString() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + TxtMode.Text.ToString() + "',  " + Grid["Item_Id", i].Value + ",  " + Grid["Color_ID", i].Value + ", " + Grid["Size_ID", i].Value + ", " + Grid["Grs_Rate", i].Value + ", " + Grid["Tax_Per", i].Value + ", " + Grid["Freight_Rate", i].Value + ", " + Grid["Pur_Rate", i].Value + ") ; Select Scope_Identity()";
                    }
                }
                Queries[Array_Index++] = MyParent.EntryLog("SOCKS RATE ENTRY", "ADD", "@@IDENTITY");
                if (MessageBox.Show("Sure to Save ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
                MyBase.Run(Queries);
                MyParent.Save_Error = false;
                MessageBox.Show("Saved ..!", "Gainup");
                MyBase.Clear(this);
                TxtMode.Focus();
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
                DtpDate.Value = Convert.ToDateTime(Dr["Effect_From"]);
                TxtMode.Text = Dr["Supplier"].ToString();                
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["GRS_RATE"].Index)
                {
                    if (Grid["GRS_RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["GRS_RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["GRS_RATE", Grid.CurrentCell.RowIndex].Value) == 0)
                    {
                        Grid["GRS_RATE", Grid.CurrentCell.RowIndex].Value = "0";
                    }
                }
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["FREIGHT_RATE"].Index)
                {
                    if (Grid["FREIGHT_RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["FREIGHT_RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["FREIGHT_RATE", Grid.CurrentCell.RowIndex].Value) == 0)
                    {
                        Grid["FREIGHT_RATE", Grid.CurrentCell.RowIndex].Value = "0";
                    }
                }
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["TAX_PER"].Index)
                {
                    if (Grid["TAX_PER", Grid.CurrentCell.RowIndex].Value == null || Grid["TAX_PER", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["TAX_PER", Grid.CurrentCell.RowIndex].Value) == 0)
                    {
                        Grid["TAX_PER", Grid.CurrentCell.RowIndex].Value = "0";
                    }
                }
                if (Grid["TAX_PER", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (Convert.ToDouble(Grid["TAX_PER", Grid.CurrentCell.RowIndex].Value) > 0)
                    {
                        Grid["PUR_RATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["GRS_RATE", Grid.CurrentCell.RowIndex].Value) + Convert.ToDouble(Grid["FREIGHT_RATE", Grid.CurrentCell.RowIndex].Value) + ((Convert.ToDouble(Grid["GRS_RATE", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Convert.ToDouble(Grid["TAX_PER", Grid.CurrentCell.RowIndex].Value)) / 100.0));
                    }
                    else
                    {
                        Grid["PUR_RATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["GRS_RATE", Grid.CurrentCell.RowIndex].Value) + Convert.ToDouble(Grid["FREIGHT_RATE", Grid.CurrentCell.RowIndex].Value);
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ITEM"].Index)
                    {
                        if (TxtMode.Text.ToString() == "YARN")
                        {
                            Dr = Tool.Selection_Tool_Except_New("ITEM1",this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "ITEM", "Select ITEM +  ' ' +  COLOR + ' ' + SIZE ITEM1, ITEM, COLOR, SIZE, Item_ID, Color_ID, Size_ID  From  View_Socks_Items_For_Rate Where Type = 'Yarn' ", string.Empty, 350,100, 100, 100);
                        }
                        else if (TxtMode.Text.ToString() == "TRIM")
                        {
                            Dr = Tool.Selection_Tool_Except_New("ITEM1", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "ITEM", "Select ITEM +  ' ' +  COLOR + ' ' + SIZE ITEM1, ITEM, COLOR, SIZE, Item_ID, Color_ID, Size_ID  From  View_Socks_Items_For_Rate Where Type = 'Trim' ", string.Empty, 350, 100, 100, 100);
                        }
                        if (Dr != null)
                        {
                            Grid["ITEM", Grid.CurrentCell.RowIndex].Value = Dr["ITEM"].ToString();
                            Grid["COLOR", Grid.CurrentCell.RowIndex].Value = Dr["COLOR"].ToString();
                            Grid["SIZE", Grid.CurrentCell.RowIndex].Value = Dr["SIZE"].ToString(); 
                            Grid["ITEM_ID", Grid.CurrentCell.RowIndex].Value = Dr["ITEM_ID"].ToString();
                            Grid["COLOR_ID", Grid.CurrentCell.RowIndex].Value = Dr["COLOR_ID"].ToString();
                            Grid["SIZE_ID", Grid.CurrentCell.RowIndex].Value = Dr["SIZE_ID"].ToString();
                            Grid["ITEM1", Grid.CurrentCell.RowIndex].Value = Dr["ITEM1"].ToString();
                            Grid["GRS_RATE", Grid.CurrentCell.RowIndex].Value = "0.00";
                            Grid["TAX_PER", Grid.CurrentCell.RowIndex].Value = "0.00";
                            Grid["FREIGHT_RATE", Grid.CurrentCell.RowIndex].Value = "0.00";
                            Grid["PUR_RATE", Grid.CurrentCell.RowIndex].Value = "0.00";                            
                            Txt.Text = Dr["ITEM"].ToString();
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


        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["GRS_RATE"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["TAX_PER"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["FREIGHT_RATE"].Index)
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

       
    }
}