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
    public partial class FrmSocksYarnQualityEntry : Form, Entry
    {
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        Control_Modules MyBase = new Control_Modules();
        DataTable Dt = new DataTable();
        DataTable Dt_Tax = new DataTable();
        DataTable[,,] Dt_OCN_New;
        String[] Queries;
        Int64 Code = 0;
        DataRow Dr;
        TextBox Txt = null;
        TextBox Txt_Lot = null;
        TextBox Txt_OCN = null;
        TextBox Txt_Tax = null;
        Int32 Excess_Limit = 60;
        String Str;
        public FrmSocksYarnQualityEntry()
        {
            InitializeComponent();
        }

        private void FrmSocksYarnQualityEntry_Load(object sender, EventArgs e)
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

       
        private void FrmSocksYarnQualityEntry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtGRNNo")
                    {
                        Grid.CurrentCell = Grid["Rej_Mode", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                    }                   
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtGRNNo")
                    {
                       // Entry_Edit();
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

        private void FrmSocksYarnQualityEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == String.Empty)
                    {

                    }
                    else
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rej_Qty"].Index)
                {
                    if (Grid["Rej_Mode", Grid.CurrentCell.RowIndex].Value.ToString() == "P")
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        MyBase.Valid_Decimal(Txt, e);
                    }                    
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rej_Mode"].Index)
                {
                   if (e.KeyChar == Convert.ToInt32('P') || e.KeyChar == Convert.ToInt32('p'))
                   {
                        e.Handled = true;
                        Txt.Text = "P";
                   }
                   else if (e.KeyChar == Convert.ToInt32('R') || e.KeyChar == Convert.ToInt32('r'))
                   {
                        e.Handled = true;
                        Txt.Text = "R";
                   }
                   else
                   {
                        e.Handled = true;
                   }
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

        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Reason"].Index)
                    {
                       if (Grid["Rej_Mode", Grid.CurrentCell.RowIndex].Value.ToString() == "R")  
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Reason", "Select Name Problem , RowID From Socks_Qc_Problem_Master Where Type = 'YARN' ", String.Empty, 150);
                            if (Dr != null)
                            {
                                Grid["Reason", Grid.CurrentCell.RowIndex].Value = Dr["Problem"].ToString();
                                Grid["Reason_ID", Grid.CurrentCell.RowIndex].Value = Dr["RowID"].ToString();
                                Txt.Text = Dr["Problem"].ToString();
                            }
                        }
                    }
                    Total_Count();
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
                Code = Convert.ToInt64(Dr["Code"]);
                TxtGRNNo.Text = Dr["GrnNO"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["GrnDate"]);
                TxtSupplier.Enabled = false;
                TxtSupplier.Text = Dr["Supplier"].ToString();
                TxtSupplier.Tag = Dr["Supplier_Code"].ToString();
                TxtGatePass.Text = Dr["GP_No"].ToString();
                DtpGPDate.Value = Convert.ToDateTime(Dr["GP_Date"]);
                if (Dr["Invoice_No"] == DBNull.Value)
                {
                    TxtDCNo.Text = Dr["DC_No"].ToString();
                    DtpDCDate.Value = Convert.ToDateTime(Dr["DC_Date"]);
                    TxtInvoiceNo.Text = "";
                    DtpInvoiceDate.Value = MyBase.GetServerDate();
                }
                else
                {
                    TxtDCNo.Text = "";
                    DtpDCDate.Value = MyBase.GetServerDate();
                    TxtInvoiceNo.Text = Dr["Invoice_No"].ToString();
                    DtpInvoiceDate.Value = Convert.ToDateTime(Dr["Invoice_Date"]);
                }
                TxtAmount.Text = Dr["Net_Amount"].ToString();

                Load_Item();  
                Total_Count();            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Load_Item()
        {
            try
            {
                if(MyParent._New)
                {
                    Str = "Select Row_Number() Over (Order By ORder_No, Item, Color, Size, Lot_No, Bag_No) SNo, ORder_No, Item, Color, Size, Lot_No, Bag_No, Qty, 'P' Rej_Mode, 0.000 Rej_Qty, '-' Reason, Qty Qty1, 0 Reason_ID, OcnDtlID, LotDtlID, DtlID, RowID FRom Socks_Yarn_GRn_Quality_Fn() Where RowID = " + Code + "  Order By ORder_No, Item, Color, Size, Lot_No, Bag_No";
                }
                else
                {
                    Str = "Select Row_Number() Over (Order By ORder_No, Item, Color, Size, Lot_No, Bag_No) SNo, ORder_No, Item, Color, Size, Lot_No, Bag_No, Qty, Rej_Mode, Rej_Qty, Reason, Qty Qty1, Reason_ID, OcnDtlID, LotDtlID, DtlID, RowID FRom Socks_Yarn_GRn_Quality_Fn() Where RowID = " + Code + "  Order By ORder_No, Item, Color, Size, Lot_No, Bag_No";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Qty1", "OcnDtlID", "LotDtlID", "DtlID", "RowID", "Reason_ID");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Rej_Qty", "Rej_Mode", "Reason");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);

                Grid.Columns["Item"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["Color"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["Size"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["ORder_No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["Lot_No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["Bag_No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Rej_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Rej_Mode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Reason"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                
                MyBase.Grid_Width(ref Grid, 40, 120, 120, 100, 90, 80, 60, 80, 60, 80, 100);

                Grid.RowHeadersWidth = 10;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Quality ", "Select GRNNo, GRNDate, Supplier, Order_No, Item, Color, Size, Lot_No, Qty, GRn_Qty, Rate, Net_Amount,  RowID Code, GP_NO, GP_Date, DC_No DC_No, DC_Date DC_Date, Invoice_No Invoice_No, Invoice_Date Invoice_Date, Supplier_Code From Fitsocks.Dbo.Socks_Yarn_GRn_Quality_Fn() Where Rej_Mode Is Null ORder by GRnNo desc, Order_No, Lot_No asc ", String.Empty, 120, 90, 250, 100, 120, 120, 120, 120, 100);
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

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Quality ", "Select GRNNo, GRNDate, Supplier, Order_No, Item, Color, Size, Lot_No, Qty, GRn_Qty, Rate, Net_Amount,  RowID Code, GP_NO, GP_Date, DC_No DC_No, DC_Date DC_Date, Invoice_No Invoice_No, Invoice_Date Invoice_Date, Supplier_Code From Fitsocks.Dbo.Socks_Yarn_GRn_Quality_Fn() Where Rej_Mode Is Not Null ORder by GRnNo Desc ", String.Empty, 120, 90, 250, 100, 120, 120, 120, 120, 100);
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
                    MyBase.Run("Update Socks_Yarn_GRN_OCN_Lot_Details Set  Rej_Mode = null , Rej_Qty = null, Rej_Reason_ID = null, Rej_Time = GetDate(), Rej_System = Host_Name() Where  Master_ID = " + Code + "  ");
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

        void Total_Count()
        {               
            try
            {                               
                TxtTotQty.Text = MyBase.Sum(ref Grid, "Rej_Qty", "ITEM");  
                Txt_Qty.Text = MyBase.Sum(ref Grid, "Qty", "ITEM");                
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
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Quality ", "Select GRNNo, GRNDate, Supplier, Order_No, Item, Color, Size, Lot_No, Qty, GRn_Qty, Rate, Net_Amount,  RowID Code, GP_NO, GP_Date, DC_No DC_No, DC_Date DC_Date, Invoice_No Invoice_No, Invoice_Date Invoice_Date, Supplier_Code From Fitsocks.Dbo.Socks_Yarn_GRn_Quality_Fn() Where Rej_Mode Is Not Null ORder by GRnNo Desc ", String.Empty, 120, 90, 250, 100, 120, 120, 120, 120, 100);
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
        
      

        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Yarn Quality ", "Select GRNNo, GRNDate, Supplier, Order_No, Item, Color, Size, Lot_No, Qty, GRn_Qty, Rate, Net_Amount,  RowID Code, GP_NO, GP_Date, DC_No DC_No, DC_Date DC_Date, Invoice_No Invoice_No, Invoice_Date Invoice_Date, Supplier_Code From Fitsocks.Dbo.Socks_Yarn_GRn_Quality_Fn() Where Rej_Mode Is Not Null ORder by GRnNo Desc ", String.Empty, 120, 90, 250, 100, 120, 120, 120, 120, 100);
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


        public void Entry_Save()
        {
            try
            {
                Int32 Array_Index = 0;
                Total_Count();               
               
                if (TxtGRNNo.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid GRN No", "Gainup");
                    TxtGRNNo.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtSupplier.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Supplier", "Gainup");
                    TxtSupplier.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                for (int i = 0; i < Grid.Rows.Count; i++)
                {
                    for (int j = 1; j < Grid.Columns.Count - 1; j++)
                    {
                        if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty || Grid[j, i].Value.ToString() == "0")
                        {
                            if (Grid["Rej_Mode", i].Value.ToString() == "P" && (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty || Grid[j, i].Value.ToString() == "0"))
                            {
                            
                            }
                            else
                            {
                                MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                Grid.CurrentCell = Grid["Rej_Qty", i];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }
                    }
                 

                     if (Grid["Rej_Mode", i].Value.ToString() == "R")
                     {
                         if(Convert.ToDouble(Grid["Rej_Qty", i].Value.ToString()) == 0 || (Convert.ToDouble(Grid["Rej_Qty", i].Value.ToString()) > Convert.ToDouble(Grid["Qty", i].Value.ToString())))
                         {
                                MessageBox.Show("Invalid Qty", "Gainup");
                                Grid.CurrentCell = Grid["Rej_Qty", i];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                         }
                     }
                }

                Queries = new String[Grid.Rows.Count + 40];
                
                for (int i = 0; i < Grid.Rows.Count; i++)
                {                    
                            Queries[Array_Index++] = "Update Socks_Yarn_GRN_OCN_Lot_Details Set Rej_Mode = '" + Grid["Rej_Mode", i].Value.ToString() + "', Rej_Qty = " + Convert.ToDouble(Grid["Rej_Qty", i].Value.ToString()) + ", Rej_Reason_ID = " + Grid["Reason_ID", i].Value.ToString() + ", Rej_Time = GetDate(), Rej_System = Host_Name() Where RowID = " + Grid["LotDtlID", i].Value.ToString() + " and Master_ID = " + Code + " ";
                }

                Queries[Array_Index++] = "Exec Vsocks_Lot_Quality " + Code + " ";

                MyBase.Run(Queries);
                MyParent.Save_Error = false;
                MessageBox.Show("Saved...!", "Gainup");



            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        private void ButFAll_Click(object sender, EventArgs e)
        {
            try
            {
                for(int f=Grid.CurrentCell.RowIndex+0; f<= Grid.Rows.Count-1; f++)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rej_Mode"].Index)
                    {                      
                        Grid["Rej_Mode", f].Value = Grid["Rej_Mode", Grid.CurrentCell.RowIndex].Value.ToString();                     
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Reason"].Index && Grid["Rej_Mode", f].Value.ToString() == "R")
                    {                      
                        Grid["Reason", f].Value = Grid["Reason", Grid.CurrentCell.RowIndex].Value.ToString();                     
                        Grid["Reason_ID", f].Value = Grid["Reason_ID", Grid.CurrentCell.RowIndex].Value.ToString();                    
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rej_Qty"].Index && Grid["Rej_Mode", f].Value.ToString() == "R")
                    {                      
                        Grid["Rej_Qty", f].Value = Grid["Qty", f].Value.ToString();
                    }
                }
                Total_Count();
            }             
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButFOrder_Click(object sender, EventArgs e)
        {
            try
            {
                for(int f=Grid.CurrentCell.RowIndex+0; f<= Grid.Rows.Count-1; f++)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rej_Mode"].Index)
                    {     
                        if(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Order_No", f].Value.ToString())
                        {
                            Grid["Rej_Mode", f].Value = Grid["Rej_Mode", Grid.CurrentCell.RowIndex].Value.ToString();                     
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Reason"].Index)
                    {   
                        if(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Order_No", f].Value.ToString() && Grid["Rej_Mode", f].Value.ToString() == "R")
                        {
                            Grid["Reason", f].Value = Grid["Reason", Grid.CurrentCell.RowIndex].Value.ToString();                     
                            Grid["Reason_ID", f].Value = Grid["Reason_ID", Grid.CurrentCell.RowIndex].Value.ToString();                    
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rej_Qty"].Index)
                    {    
                        if(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Order_No", f].Value.ToString() && Grid["Rej_Mode", f].Value.ToString() == "R")
                        {
                            Grid["Rej_Qty", f].Value = Grid["Qty", f].Value.ToString();
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

        private void ButFLot_Click(object sender, EventArgs e)
        {
            try
            {
                for(int f=Grid.CurrentCell.RowIndex+0; f<= Grid.Rows.Count-1; f++)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rej_Mode"].Index)
                    {     
                        if(Grid["Lot_No", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Lot_No", f].Value.ToString())
                        {
                            Grid["Rej_Mode", f].Value = Grid["Rej_Mode", Grid.CurrentCell.RowIndex].Value.ToString();                     
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Reason"].Index)
                    {   
                        if(Grid["Lot_No", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Lot_No", f].Value.ToString() && Grid["Rej_Mode", f].Value.ToString() == "R")
                        {
                            Grid["Reason", f].Value = Grid["Reason", Grid.CurrentCell.RowIndex].Value.ToString();                     
                            Grid["Reason_ID", f].Value = Grid["Reason_ID", Grid.CurrentCell.RowIndex].Value.ToString();                    
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rej_Qty"].Index)
                    {    
                        if(Grid["Lot_No", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Lot_No", f].Value.ToString() && Grid["Rej_Mode", f].Value.ToString() == "R")
                        {
                            Grid["Rej_Qty", f].Value = Grid["Qty", f].Value.ToString();
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

        private void ButFItem_Click(object sender, EventArgs e)
        {
            try
            {
                for(int f=Grid.CurrentCell.RowIndex+0; f<= Grid.Rows.Count-1; f++)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rej_Mode"].Index)
                    {     
                        if(Grid["Item", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Item", f].Value.ToString() && Grid["Color", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Color", f].Value.ToString() && Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Size", f].Value.ToString())
                        {
                            Grid["Rej_Mode", f].Value = Grid["Rej_Mode", Grid.CurrentCell.RowIndex].Value.ToString();                     
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Reason"].Index)
                    {   
                        if(Grid["Item", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Item", f].Value.ToString() && Grid["Color", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Color", f].Value.ToString() && Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Size", f].Value.ToString() && Grid["Rej_Mode", f].Value.ToString() == "R")
                        {
                            Grid["Reason", f].Value = Grid["Reason", Grid.CurrentCell.RowIndex].Value.ToString();                     
                            Grid["Reason_ID", f].Value = Grid["Reason_ID", Grid.CurrentCell.RowIndex].Value.ToString();                    
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rej_Qty"].Index)
                    {    
                        if(Grid["Item", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Item", f].Value.ToString() && Grid["Color", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Color", f].Value.ToString() && Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString() == Grid["Size", f].Value.ToString() && Grid["Rej_Mode", f].Value.ToString() == "R")
                        {
                            Grid["Rej_Qty", f].Value = Grid["Qty", f].Value.ToString();
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

        void Txt_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rej_Mode"].Index)
                {
                    if(Grid["Rej_Mode", Grid.CurrentCell.RowIndex].Value  != null && Txt.Text.ToString() != String.Empty)
                    {                       
                        Grid["Rej_Mode", Grid.CurrentCell.RowIndex].Value = Txt.Text.ToString();
                        if(Grid["Rej_Mode", Grid.CurrentCell.RowIndex].Value.ToString() == "P")
                        {
                            Grid["Rej_Qty", Grid.CurrentCell.RowIndex].Value= 0.000;
                            Grid["Reason", Grid.CurrentCell.RowIndex].Value= "-";
                            Grid["Reason_ID", Grid.CurrentCell.RowIndex].Value= 0;
                        }
                        else if(Grid["Rej_Mode", Grid.CurrentCell.RowIndex].Value.ToString() == "R" && Convert.ToDouble(Grid["Rej_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) == 0)
                        {
                            Grid["Rej_Qty", Grid.CurrentCell.RowIndex].Value= Grid["Qty", Grid.CurrentCell.RowIndex].Value;                            
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Reason"].Index)
                {
                    if(Grid["Reason", Grid.CurrentCell.RowIndex].Value  != null && Txt.Text.ToString() != String.Empty)
                    {                       
                        Grid["Reason", Grid.CurrentCell.RowIndex].Value = Txt.Text.ToString();
                        if(Grid["Rej_Mode", Grid.CurrentCell.RowIndex].Value.ToString() == "P")
                        {
                            Grid["Rej_Qty", Grid.CurrentCell.RowIndex].Value= 0.000;
                            Grid["Reason", Grid.CurrentCell.RowIndex].Value= "-";
                            Grid["Reason_ID", Grid.CurrentCell.RowIndex].Value= 0;
                        }
                        else if(Grid["Rej_Mode", Grid.CurrentCell.RowIndex].Value.ToString() == "R" && (Grid["Reason_ID", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Grid["Reason_ID", Grid.CurrentCell.RowIndex].Value.ToString() == "0"))
                        {
                            MessageBox.Show ("Invalid Reason","Gainup");
                            Grid["Reason", Grid.CurrentCell.RowIndex].Value="-";
                            Grid.CurrentCell = Grid["Reason", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);                
                            return; 
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rej_Qty"].Index)
                {
                    if(Grid["Rej_Qty", Grid.CurrentCell.RowIndex].Value  != null && Txt.Text.ToString() != String.Empty)
                    {
                        Grid["Rej_Qty", Grid.CurrentCell.RowIndex].Value = Txt.Text.ToString();
                        if(Grid["Rej_Mode", Grid.CurrentCell.RowIndex].Value.ToString() == "R")
                        {
                           if(Convert.ToDouble(Grid["Qty", Grid.CurrentCell.RowIndex].Value) < Convert.ToDouble(Grid["Rej_Qty", Grid.CurrentCell.RowIndex].Value))
                           {
                               MessageBox.Show("Invalid Qty","Gainup");
                               Grid["Rej_Qty", Grid.CurrentCell.RowIndex].Value= 0.000;
                              // Txt.Text = "0.000";
                               Grid.CurrentCell = Grid["Rej_Qty", Grid.CurrentCell.RowIndex];
                               Grid.Focus();
                               Grid.BeginEdit(true);                
                               return;                               
                           }
                           else if(Convert.ToDouble(Grid["Rej_Qty", Grid.CurrentCell.RowIndex].Value) == 0)
                           {
                               MessageBox.Show("Invalid Qty","Gainup");
                               Grid["Rej_Qty", Grid.CurrentCell.RowIndex].Value= 0.000;
                               Grid.CurrentCell = Grid["Rej_Qty", Grid.CurrentCell.RowIndex];
                               Grid.Focus();
                               Grid.BeginEdit(true);                
                               return;                               
                           }     
                        }
                        else
                        {
                            Grid["Rej_Qty", Grid.CurrentCell.RowIndex].Value= 0.000;
                        }
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