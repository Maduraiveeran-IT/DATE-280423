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
    public partial class FrmSocksYarnGRN : Form, Entry
    {
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        Control_Modules MyBase = new Control_Modules();
        DataTable Dt = new DataTable();
        DataTable[] Dt_Lot;
        DataTable[] Dt_OCN;
        DataRow Dr;
        TextBox Txt = null;
        TextBox Txt_Lot = null;
        TextBox Txt_OCN = null;
        Int32 Excess_Limit = 60;

        public FrmSocksYarnGRN()
        {
            InitializeComponent();
        }

        private void FrmSocksYarnGRN_Load(object sender, EventArgs e)
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

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                Load_Item();
                Dt_Lot = new DataTable[20];
                Dt_OCN = new DataTable[20];
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

        void Load_Item()
        {
            try
            {
                
                Grid.DataSource = MyBase.Load_Data("Select 0 as SL, Item + ' ' + Color + ' ' + Size + ' @ ' + Cast(Rate as Varchar (15)) Description, Item_ID, Item ITEM, Color_ID, Color COLOR, Size_ID, Size SIZE, PO_Qty, Inward_Qty, Bal_Qty, Bal_Qty GRN_Qty, Rate, Cast(0 as Numeric (25, 2)) Amount From Socks_Yarn_GRN_Pending () Where 1=2", ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Item_ID", "Color_ID", "Size_ID", "Description");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Item", "GRN_Qty");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);

                Grid.Columns["po_Qty"].HeaderText = "PO";
                Grid.Columns["inward_Qty"].HeaderText = "INWARD";
                Grid.Columns["BAL_Qty"].HeaderText = "BAL";
                Grid.Columns["GRN_Qty"].HeaderText = "GRN";


                Grid.Columns["po_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["bal_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["grn_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["inward_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


                Grid.Columns["po_qty"].DefaultCellStyle.Format = "0.000";
                Grid.Columns["bal_qty"].DefaultCellStyle.Format = "0.000";
                Grid.Columns["grn_qty"].DefaultCellStyle.Format = "0.000";
                Grid.Columns["inward_qty"].DefaultCellStyle.Format = "0.000";

                MyBase.Grid_Width(ref Grid, 40, 140, 100, 100, 90, 90, 90, 90, 90, 100);

                Grid.RowHeadersWidth = 10;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmSocksYarnGRN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtGatePass")
                    {
                        Grid.CurrentCell = Grid["Item", 0];
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
                    if (this.ActiveControl.Name == "TxtSupplier")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "Select Distinct L1.Ledger_Name Supplier, S1.SUpplier_Code From Socks_Yarn_GRN_Pending () S1 Left Join Accounts.Dbo.Ledger_Master L1 on S1.Supplier_Code = L1.Ledger_Code and L1.Company_Code = " + MyParent.CompCode + " and L1.Year_Code = '" + MyParent.YearCode + "'", String.Empty, 350);
                        if (Dr != null)
                        {
                            TxtSupplier.Tag = Dr["Supplier_Code"].ToString();
                            TxtSupplier.Text = Dr["Supplier"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtGatePass")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select GATE PASS", "Select GPNo, GPDate, Party, isnull(InvNo, '') InvNo, InvDate, Isnull(DCno, '') DCno, DCDate, Qty From Socks_Gate_Pass_Details ()", String.Empty, 100, 100, 150, 100, 100, 100, 100);
                        if (Dr != null)
                        {
                            TxtGatePass.Text = Dr["GPNo"].ToString();
                            MyBase.Lock_DatetimePicker (ref DtpGPDate, Convert.ToDateTime(Dr["GPDate"]));
                            
                            if (Dr["InvNo"].ToString() != String.Empty)
                            {
                                TxtInvoiceNo.Text = Dr["InvNo"].ToString();
                                TxtDCNo.Text = "";
                                TxtQty.Text = Dr["Qty"].ToString();
                                MyBase.Lock_DatetimePicker (ref DtpInvoiceDate, Convert.ToDateTime(Dr["InvDate"]));
                                MyBase.Lock_DatetimePicker (ref DtpDCDate, MyBase.GetServerDate());
                            }
                            else
                            {
                                TxtDCNo.Text = Dr["DCNo"].ToString();
                                TxtInvoiceNo.Text = "";
                                MyBase.Lock_DatetimePicker(ref DtpInvoiceDate, MyBase.GetServerDate());
                                MyBase.Lock_DatetimePicker(ref DtpDCDate, Convert.ToDateTime(Dr["DCDate"]));
                            }
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

        private void FrmSocksYarnGRN_KeyPress(object sender, KeyPressEventArgs e)
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Item"].Index)
                {
                    e.Handled = true;
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Grn_Qty"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Item"].Index)
                    {
                        Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Item", "Select Item + ' ' + Color + ' ' + Size + ' @ ' + Cast(Rate as Varchar (15)) Description, PO_Qty PO, Inward_Qty Inward, Bal_Qty Bal, Bal_Qty GRN, Rate, Cast(0 as Numeric (25, 2)) Amount, Item_ID, Item ITEM, Color_ID, Color COLOR, Size_ID, Size SIZE From Socks_Yarn_GRN_Pending () Where Supplier_Code = " + TxtSupplier.Tag.ToString(), String.Empty, 250, 80, 80, 80, 80, 80);
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Item"].ToString();

                            Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                            Grid["Description", Grid.CurrentCell.RowIndex].Value = Dr["Description"].ToString();
                            Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                            Grid["Color", Grid.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                            Grid["Item_ID", Grid.CurrentCell.RowIndex].Value = Dr["Item_ID"].ToString();
                            Grid["Size_ID", Grid.CurrentCell.RowIndex].Value = Dr["Size_ID"].ToString();
                            Grid["Color_ID", Grid.CurrentCell.RowIndex].Value = Dr["Color_ID"].ToString();

                            Grid["PO_Qty", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["PO"]);
                            Grid["Inward_Qty", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["Inward"]);
                            Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["Bal"]);
                            Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["GRN"]);
                            Grid["Rate", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["Rate"]);

                            Load_OCN(Grid.CurrentCell.RowIndex);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        double Bal_Qty_Lot()
        {
            double Qty = 0;
            try
            {
                Qty = Convert.ToDouble(Grid_OCN["grn_qty", Grid_OCN.CurrentCell.RowIndex].Value);

                for (int i = 0; i <= Dt_Lot[Grid_OCN.CurrentCell.RowIndex].Rows.Count - 1; i++)
                {
                    Qty -= Convert.ToDouble(Dt_Lot[Grid_OCN.CurrentCell.RowIndex].Rows[i]["Qty"]);
                }

                return Math.Round(Qty, 3);
            }
            catch (Exception ex)
            {
                return Qty;
            }
        }


        double Bal_Qty_OCN()
        {
            double Qty = 0;
            try
            {
                Qty = Convert.ToDouble(Grid["grn_qty", Grid.CurrentCell.RowIndex].Value);

                for (int i = 0; i <= Dt_OCN[Grid.CurrentCell.RowIndex].Rows.Count - 1; i++)
                {
                    Qty -= Convert.ToDouble(Dt_OCN[Grid.CurrentCell.RowIndex].Rows[i]["GRN_Qty"]);
                }

                return Math.Round (Qty, 3);
            }
            catch (Exception ex)
            {
                return Qty;
            }
        }

        void Load_Lot(Int32 Row)
        {
            try
            {
                if (Dt_Lot[Row] == null)
                {
                    Dt_Lot[Row] = new DataTable();
                    MyBase.Load_Data("Select S1.Slno SL, S1.Lot_No, S1.Bag_No, S1.Qty, S1.Location_ID, S3.Location, '' T From Socks_Yarn_GRN_OCN_Lot_Details S1 Left Join Socks_Yarn_Stores_location_Master S3 on S1.Location_ID = S3.rowID Where 1 = 2", ref Dt_Lot[Row]);
                }

                Grid_LotNo.DataSource = Dt_Lot[Row];
                MyBase.Grid_Designing(ref Grid_LotNo, ref Dt_Lot[Row], "Location_ID", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid_LotNo, "Lot_No", "Bag_NO", "Qty", "Location");
                MyBase.Grid_Colouring(ref Grid_LotNo, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid_LotNo, 40, 100, 80, 90, 120);

                Grid_LotNo.Columns["lot_no"].HeaderText = "LOTNO";
                Grid_LotNo.Columns["BAG_no"].HeaderText = "BAGNO";
                Grid_LotNo.Columns["QTY"].HeaderText = "QTY";
                Grid_LotNo.Columns["Location"].HeaderText = "LOCATION";

                Grid_LotNo.RowHeadersWidth = 10;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        void Load_OCN(Int32 Row)
        {
            try
            {
                if (Dt_OCN[Row] == null)
                {
                    Dt_OCN[Row] = new DataTable();
                    MyBase.Load_Data("Select S1.Slno SL, Cast('' as Varchar (30)) Description, S1.Order_ID, S1.PO_Detail_ID, S2.Order_No, S8.PONo, Qty PO_QTY, Cast(0 as Numeric (16, 3)) GRN_QTY, '' T From Socks_Yarn_GRN_OCN_DEtails S1 left Join Socks_Order_Master S2 on S1.Order_ID = S2.RowID Inner Join Socks_Yarn_PO_Details S7 on S1.PO_Detail_ID = S7.RowID Inner Join Socks_Yarn_PO_Master S8 on S7.Master_ID = S8.RowID Where 1 = 2", ref Dt_OCN[Row]);
                }

                Grid_OCN.DataSource = Dt_OCN[Row];
                MyBase.Grid_Designing(ref Grid_OCN, ref Dt_OCN[Row], "Order_ID", "T", "PO_Detail_ID", "Description");
                MyBase.ReadOnly_Grid_Without(ref Grid_OCN, "Order_No", "GRN_QTY");
                MyBase.Grid_Colouring(ref Grid_OCN, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid_OCN, 30, 110, 110, 100, 100);

                Grid_OCN.Columns["GRN_QTY"].HeaderText = "GRN";
                Grid_OCN.Columns["po_qty"].HeaderText = "PO_BALQTY";
                Grid_OCN.Columns["PONO"].HeaderText = "PO";

                Grid_OCN.RowHeadersWidth = 10;

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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["GRN_Qty"].Index)
                    {
                        MyBase.Row_Number(ref Grid);
                        if (Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value == null || Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                        }
                        if (Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid GRN Qty ...!", "Gainup");
                            Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value = Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["GRN_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            if ((Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value)) >= Excess_Limit)
                            {
                                e.Handled = true;
                                MessageBox.Show("GRN Qty Crossed Excess Limit [" + Excess_Limit + "] ...!", "Gainup");
                                Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value = Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value;
                                Grid.CurrentCell = Grid["GRN_Qty", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                            else
                            {
                                Grid["Amount", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["GRN_Qty", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["Rate", Grid.CurrentCell.RowIndex].Value);
                            }

                            if (!Grid_Amount())
                            {
                                e.Handled = true;
                            }
                            else
                            {
                                e.Handled = true;
                                Grid_OCN.CurrentCell = Grid_OCN["ORDER_NO", 0];
                                Grid_OCN.Focus();
                                Grid_OCN.BeginEdit(true);
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


        Boolean Grid_Amount()
        {
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["GRN_Qty", i].Value == null || Grid["GRN_Qty", i].Value == DBNull.Value || Grid["GRN_Qty", i].Value.ToString() == String.Empty)
                    {
                        Grid["GRN_Qty", i].Value = "0.000";
                    }

                    if (Convert.ToDouble(Grid["GRN_Qty", i].Value) == 0)
                    {
                        MessageBox.Show("Invalid GRN Qty ...!", "Gainup");
                        Grid["GRN_Qty", i].Value = Grid["Bal_Qty", i].Value;
                        Grid.CurrentCell = Grid["GRN_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return false;
                    }
                    else
                    {
                        if ((Convert.ToDouble(Grid["GRN_Qty", i].Value) - Convert.ToDouble(Grid["Bal_Qty", i].Value)) >= Excess_Limit)
                        {
                            MessageBox.Show("GRN Qty Crossed Excess Limit [" + Excess_Limit + "] ...!", "Gainup");
                            Grid["GRN_Qty", i].Value = Grid["Bal_Qty", i].Value;
                            Grid.CurrentCell = Grid["GRN_Qty", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return false;
                        }
                        else
                        {
                            Grid["Amount", i].Value = Convert.ToDouble(Grid["GRN_Qty", i].Value) * Convert.ToDouble(Grid["Rate", i].Value);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void Grid_LotNo_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Lot == null)
                {
                    Txt_Lot = (TextBox)e.Control;
                    Txt_Lot.KeyDown += new KeyEventHandler(Txt_Lot_KeyDown);
                    Txt_Lot.KeyPress += new KeyPressEventHandler(Txt_Lot_KeyPress);
                    Txt_Lot.GotFocus += new EventHandler(Txt_Lot_GotFocus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Lot_GotFocus(object sender, EventArgs e)
        {
            try
            {

                MyBase.Row_Number(ref Grid_LotNo);
                if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Lot_No"].Index)
                {
                    if (Bal_Qty_Lot() > 0)
                    {
                        if (Grid_LotNo.CurrentCell.RowIndex > 0)
                        {
                            if (Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            {
                                Txt_Lot.Text = Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex - 1].Value.ToString();
                                Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value = Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex - 1].Value;
                            }
                        }
                    }
                }
                else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["qty"].Index)
                {
                    if (Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value) == 0)
                    {
                        Txt_Lot.Text = String.Format("{0:0.000}", Bal_Qty_Lot());
                        Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value = Txt_Lot.Text;
                    }
                }
                else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Bag_No"].Index)
                {
                    if (Bal_Qty_Lot() > 0)
                    {
                        if (Grid_LotNo.CurrentCell.RowIndex > 0)
                        {
                            if (Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            {
                                if (Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex - 1].Value.ToString())
                                {
                                    Txt_Lot.Text = Convert.ToString(Convert.ToInt32(Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex - 1].Value) + 1);
                                    Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value = Convert.ToInt32(Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex - 1].Value) + 1;
                                }
                                else
                                {
                                    Txt_Lot.Text = "1";
                                    Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value = 1;
                                }
                            }
                        }
                        else
                        {
                            Txt_Lot.Text = "1";
                            Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value = 1;
                        }
                    }
                }
                else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Location"].Index)
                {
                    if (Grid_LotNo.CurrentCell.RowIndex > 0)
                    {
                        if (Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Txt_Lot.Text = Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex - 1].Value.ToString();
                            Grid_LotNo["Location_ID", Grid_LotNo.CurrentCell.RowIndex].Value = Grid_LotNo["Location_ID", Grid_LotNo.CurrentCell.RowIndex - 1].Value;
                            Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex].Value = Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex - 1].Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Lot_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["bag_no"].Index)
                {
                    //e.Handled = true;
                }
                else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Qty"].Index)
                {
                    MyBase.Valid_Decimal(Txt_Lot, e);
                }
                else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Lot_No"].Index)
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

        void Txt_Lot_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["location"].Index)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Location", "Select Location, RowID From Socks_Yarn_stores_location_Master", String.Empty, 120);
                        if (Dr != null)
                        {
                            Grid_LotNo["Location_ID", Grid_LotNo.CurrentCell.RowIndex].Value = Dr["RowID"].ToString();
                            Grid_LotNo["LOcation", Grid_LotNo.CurrentCell.RowIndex].Value = Dr["Location"].ToString();
                            Txt_Lot.Text = Dr["Location"].ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_LotNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Qty"].Index)
                    {
                        if (Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value == null || Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value == DBNull.Value || Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value = "0.000";
                        }

                        if (Convert.ToDouble(Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Bag Weight ...!", "Gainup");
                            Grid_LotNo.CurrentCell = Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex];
                            Grid_LotNo.Focus();
                            Grid_LotNo.BeginEdit(true);
                            return;
                        }

                        if (Convert.ToDouble(Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value) > Bal_Qty_Lot ())
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Bag Weight greater than PO ...!", "Gainup");
                            Grid_LotNo.CurrentCell = Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex];
                            Grid_LotNo["Qty", Grid_LotNo.CurrentCell.RowIndex].Value = Bal_Qty_Lot();
                            Grid_LotNo.Focus();
                            Grid_LotNo.BeginEdit(true);
                            return;
                        }

                    }
                    else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Lot_No"].Index)
                    {
                        if (Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value == null || Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value == DBNull.Value || Grid_LotNo["Lot_No", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Lot No ...!", "Gainup");
                            Grid_LotNo.CurrentCell = Grid_LotNo["LOt_No", Grid_LotNo.CurrentCell.RowIndex];
                            Grid_LotNo.Focus();
                            Grid_LotNo.BeginEdit(true);
                            return;
                        }
                    }
                    else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Bag_No"].Index)
                    {
                        if (Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value == null || Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value == DBNull.Value || Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Bag No ...!", "Gainup");
                            Grid_LotNo.CurrentCell = Grid_LotNo["Bag_No", Grid_LotNo.CurrentCell.RowIndex];
                            Grid_LotNo.Focus();
                            Grid_LotNo.BeginEdit(true);
                            return;
                        }
                    }
                    else if (Grid_LotNo.CurrentCell.ColumnIndex == Grid_LotNo.Columns["Location"].Index)
                    {
                        if (Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex].Value == null || Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex].Value == DBNull.Value || Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Location ...!", "Gainup");
                            Grid_LotNo.CurrentCell = Grid_LotNo["Location", Grid_LotNo.CurrentCell.RowIndex];
                            Grid_LotNo.Focus();
                            Grid_LotNo.BeginEdit(true);
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

        private void Grid_LotNo_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref Grid_LotNo, ref Dt_Lot[Grid.CurrentCell.RowIndex], Grid_LotNo.CurrentCell.RowIndex);
                MyBase.Row_Number(ref Grid_LotNo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_LotNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Grid.CurrentCell = Grid["Item", Grid.CurrentCell.RowIndex + 1];
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

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            Int32 Row = 0;
            try
            {
                if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Vaahini", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                        Dt_Lot[Grid.CurrentCell.RowIndex] = null;
                        MyBase.ReArrange_Datatable_Array(Dt_Lot);
                        Grid_CurrentCellChanged(sender, e);
                    }
                }

                MyBase.Row_Number(ref Grid_LotNo);
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
                if (Grid.CurrentCell != null)
                {
                    Load_OCN(Grid.CurrentCell.RowIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_OCN_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Grid_OCN_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_OCN == null)
                {
                    Txt_OCN = (TextBox)e.Control;
                    Txt_OCN.KeyDown += new KeyEventHandler(Txt_OCN_KeyDown);
                    Txt_OCN.KeyPress += new KeyPressEventHandler(Txt_OCN_KeyPress);
                    Txt_OCN.GotFocus += new EventHandler(Txt_OCN_GotFocus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_OCN_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (Grid_OCN.CurrentCell.ColumnIndex == Grid_OCN.Columns["GRN_Qty"].Index)
                {
                    if (Bal_Qty_OCN() > 0)
                    {
                        if (Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value == null || Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value == DBNull.Value || Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Txt_OCN.Text = String.Format("{0:0.000}", Bal_Qty_OCN());
                            Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value = Bal_Qty_OCN();
                        }
                    }
                }
                
                /*else if (Grid_OCN.CurrentCell.ColumnIndex == Grid_OCN.Columns["ORDER_NO"].Index)
                {
                    if (Bal_Qty_OCN() > 0)
                    {
                        MyBase.Row_Number(ref Grid_OCN);
                        if (Grid_OCN.CurrentCell.RowIndex > 0)
                        {
                            if (Grid_OCN["Order_NO", Grid_OCN.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            {
                                Txt_OCN.Text = Grid_OCN["Order_NO", Grid_OCN.CurrentCell.RowIndex - 1].Value.ToString();
                                Grid_OCN["Order_NO", Grid_OCN.CurrentCell.RowIndex].Value = Grid_OCN["Order_NO", Grid_OCN.CurrentCell.RowIndex - 1].Value;
                                Grid_OCN["PONo", Grid_OCN.CurrentCell.RowIndex].Value = Grid_OCN["PONo", Grid_OCN.CurrentCell.RowIndex - 1].Value;
                                Grid_OCN["PO_Detail_ID", Grid_OCN.CurrentCell.RowIndex].Value = Grid_OCN["PO_Detail_ID", Grid_OCN.CurrentCell.RowIndex - 1].Value;
                                Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex].Value = Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex - 1].Value;
                            }
                        }
                    }
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_OCN_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid_OCN.CurrentCell.ColumnIndex == Grid_OCN.Columns["grn_Qty"].Index)
                {
                    MyBase.Valid_Decimal(Txt_OCN, e);
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

        void Txt_OCN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid_OCN.CurrentCell.ColumnIndex == Grid_OCN.Columns["ORDER_NO"].Index)
                    {
                        MyBase.Row_Number(ref Grid_OCN);
                        Dr = Tool.Selection_Tool_Except_New ("Description", this, 30, 70, ref Dt_OCN[Grid.CurrentCell.RowIndex], SelectionTool_Class.ViewType.NormalView, "Select OCN", "Select Distinct S2.Order_No, S1.PONo, S1.Bal_Qty PO_Qty, S1.Order_ID, S1.PO_Detail_ID, (S2.Order_No + '-' + S1.PONo) Description From Socks_Yarn_GRN_Pending_OCN () S1 left join Socks_Order_Master S2 on S1.Order_ID = S2.RowID Where Item_ID = " + Grid["Item_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Color_ID = " + Grid["Color_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Size_ID = " + Grid["Size_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Rate = " + Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString(), String.Empty, 120, 120, 100);
                        if (Dr != null)
                        {
                            Grid_OCN["Description", Grid_OCN.CurrentCell.RowIndex].Value = Dr["Description"].ToString();
                            Grid_OCN["Order_id", Grid_OCN.CurrentCell.RowIndex].Value = Dr["order_id"].ToString();
                            Txt_OCN.Text = Dr["order_NO"].ToString();
                            Grid_OCN["Order_NO", Grid_OCN.CurrentCell.RowIndex].Value = Dr["order_NO"].ToString();
                            Grid_OCN["PONO", Grid_OCN.CurrentCell.RowIndex].Value = Dr["PONO"].ToString();
                            Grid_OCN["PO_Detail_ID", Grid_OCN.CurrentCell.RowIndex].Value = Dr["PO_Detail_ID"].ToString();
                            Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex].Value = Bal_Qty_OCN ();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_OCN_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref Grid_OCN, ref Dt_OCN[Grid.CurrentCell.RowIndex], Grid.CurrentCell.RowIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_OCN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid_OCN.CurrentCell.ColumnIndex == Grid_OCN.Columns["GRN_Qty"].Index)
                    {
                        if (Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value == null || Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value == DBNull.Value || Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value = "0.000";
                        }


                        if (Convert.ToDouble(Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Qty ...!", "Gainup");
                            Grid_OCN.CurrentCell = Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex];
                            Grid_OCN.Focus();
                            Grid_OCN.BeginEdit(true);
                            return;
                        }

                        if (Convert.ToDouble(Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Qty greater than PO ...!", "Gainup");
                            Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex].Value = Grid_OCN["PO_Qty", Grid_OCN.CurrentCell.RowIndex].Value;
                            Grid_OCN.CurrentCell = Grid_OCN["GRN_Qty", Grid_OCN.CurrentCell.RowIndex];
                            Grid_OCN.Focus();
                            Grid_OCN.BeginEdit(true);
                            return;
                        }

                        e.Handled = true;
                        Load_Lot(Grid_OCN.CurrentCell.RowIndex);
                        Grid_LotNo.CurrentCell = Grid_LotNo["LOt_No", 0];
                        Grid_LotNo.Focus();
                        Grid_LotNo.BeginEdit(true);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_OCN_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Grid.CurrentCell = Grid["Item", Grid.CurrentCell.RowIndex + 1];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_OCN_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid_OCN.CurrentCell != null)
                {
                    Load_Lot(Grid_OCN.CurrentCell.RowIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}