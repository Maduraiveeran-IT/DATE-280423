using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using Accounts;
using System.IO;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmYarnBarcodeDetailsNew : Form
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataRow Dr;
        MDIMain MyParent;
        TextBox Txt = null;
        String Str;
        Int16 count = 0;

        public FrmYarnBarcodeDetailsNew()
        {
            InitializeComponent();
        }

        private void Grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt == null)
                {
                    Txt = (TextBox)e.Control;
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

        private void FrmYarnBarcodeDetailsNew_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "Txt_Barcode")
                    {
                        count = 1;

                        Fill_Datas();
                        Txt_Barcode.Clear();
                        count = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmYarnBarcodeDetailsNew_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmYarnBarcodeDetailsNew_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                count = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Data()
        {
            try
            {
                if (count > 0)
                {
                    DataTable Dt1 = new DataTable();

                    MyBase.Load_Data("Select 0 Slno, supplier, color, LotNo, bagno, (case when Item ='Cotton' then 'Combed Cotton' else Item end) Item, size Count, Sum(weight) Quantity, isnull(buyer,'-') buyer, Qc_Status, Order_No, '-' PO_No, Grn_No, Grn_Date, (case when Order_No = 'GENERAL' then 'OGENERAL' else Substring(Order_No,8,5) end) OCN, Substring(Grn_No,8,5) GRN, Location From Barcode_Details_Lot() where Cast(Substring(Grn_No,8,5)as Varchar(5))+LotNo+Cast(RIGHT('000'+CAST(BagNo AS VARCHAR(3)),3)AS VARCHAR(3)) = '" + Txt_Barcode.Text + "' Group By supplier, color, LotNo, bagno, Item, Size, isnull(buyer,'-'), Qc_Status, Order_No, Grn_No, Grn_Date, Location ", ref Dt1);
                    if (Dt1.Rows.Count > 0)
                    {
                        Str = "Select 0 Slno, supplier, color, LotNo, bagno, (case when Item ='Cotton' then 'Combed Cotton' else Item end) Item, size Count, Sum(weight) Quantity, isnull(buyer,'-') buyer, Qc_Status, Order_No, '-' PO_No, Grn_No, Grn_Date, Location From Barcode_Details_Lot() where Cast(Substring(Grn_No,8,5)as Varchar(5))+LotNo+Cast(RIGHT('000'+CAST(BagNo AS VARCHAR(3)),3)AS VARCHAR(3)) = '" + Txt_Barcode.Text + "' Group By supplier, color, LotNo, bagno, Item, Size, isnull(buyer,'-'), Qc_Status, Order_No, Grn_No, Grn_Date, Location ";
                    }
                    else 
                    {
                        Str = "Select 0 Slno, supplier, color, LotNo, bagno, (case when Item ='Cotton' then 'Combed Cotton' else Item end) Item, size Count, Sum(weight) Quantity, isnull(buyer,'-') buyer, Qc_Status, Order_No, '-' PO_No, Grn_No, Grn_Date, Location From Barcode_Details_Lot() where (case when Order_No = 'GENERAL' then 'OGENERAL' else Substring(Order_No,8,5) end)+Cast(Substring(Grn_No,8,5)as Varchar(5))+LotNo+Cast(RIGHT('000'+CAST(BagNo AS VARCHAR(3)),3)AS VARCHAR(3)) = '" + Txt_Barcode.Text + "' Group By supplier, color, LotNo, bagno, Item, Size, isnull(buyer,'-'), Qc_Status, Order_No, Grn_No, Grn_Date, Location ";
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Barcode...! Gainup");
                    Txt_Barcode.Text = "";
                    Txt_Barcode.Focus();
                    return;
                }

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                if (Dt.Rows.Count > 0)
                {
                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid, 40, 200, 80, 150, 120, 120, 120, 120, 120, 80);
                    Grid.RowHeadersWidth = 10;
                }
                else
                {
                    if (count < 0)
                    {
                        MessageBox.Show("No Details Found...!", "Barcode Details");
                        Txt_Barcode.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void Fill_Datas()
        {
            try
            {

                Grid_Data();

                return;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}
