using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmSocksBarcodeScanner : Form 
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataRow Dr;
        MDIMain MyParent;
        TextBox Txt = null;
        String Str;
        Int16 count = 0;
        public FrmSocksBarcodeScanner()
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

        private void FrmSocksBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyCode == Keys.Down)
                //{
                //    if (this.ActiveControl.Name == "Txt_Barcode")
                //    {
                //        count = 1;
                //        Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Barcode", "select distinct Barcode from Socks_Barcode_Details", String.Empty, 150);
                //        if (Dr != null)
                //        {
                //            Txt_Barcode.Text = Dr["Barcode"].ToString();
                //            Fill_Datas();
                //            Txt_Barcode.Clear();
                //        }
                //    }

                //}

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

        private void FrmSocksBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmSocksBarcode_Load(object sender, EventArgs e)
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

                    MyBase.Load_Data("Select Len('" + Txt_Barcode.Text + "')Length, SUBSTRING('" + Txt_Barcode.Text + "',1,1)First_Char, SUBSTRING('" + Txt_Barcode.Text + "',12,3)Process", ref Dt1);
                    if (Dt1.Rows.Count > 0)
                    {
                        if (Convert.ToInt64(Dt1.Rows[0]["Length"].ToString()) == 14 && (Dt1.Rows[0]["First_Char"].ToString() == "G" || Dt1.Rows[0]["First_Char"].ToString() == "g" || Dt1.Rows[0]["First_Char"].ToString() == "S" || Dt1.Rows[0]["First_Char"].ToString() == "s") && Convert.ToInt64(Dt1.Rows[0]["Process"].ToString()) == 152)
                        {
                            Str = "Select 0 SNO, 'KNITTING' PROCESS, K2.EntryDate DATE, S1.shiftcode2 SHIFT, K1.MachineID MACHINE, E1.Name NAME, K1.Production PRODUCTION, K1.Order_No ORDER_NO, B3.color SAMPLE, S2.size SIZE, IsNull(Im_Color,'-')COLOR, K1.BOMQty BOMQTY, Dbo.Get_Knit_Production(K1.Order_No, K1.OrderColorID, K1.SizeID)Total_Production, K1.BOMQty - Dbo.Get_Knit_Production(K1.Order_No, K1.OrderColorID, K1.SizeID) Balance from  Socks_Barcode_Details B1 left join Socks_Bundle_Details B2 on B1.Socks_Bundle_Details_RowID = B2.RowID left join Floor_Knitting_Details K1 on B2.Floor_Knitting_Details_RowID = K1.RowID left join Floor_Knitting_Master K2 on K1.MasterID = K2.RowID left join Vaahini_ERP_Gainup.Dbo.Shiftmst S1 on S1.Shiftcode = K2.ShiftCode and compcode = 2 and mode = 1 left join VAAHINI_ERP_GAINUP.dbo.EmployeeMas E1 on E1.Emplno = K1.Emplno_Operator left join size S2 on K1.SizeID = S2.sizeid left join Socks_Bom() B3 on K1.Order_No = B3.Order_No and K1.sizeid = B3.SizeID and K1.OrderColorID = B3.OrderColorId where B1.Barcode ='" + Txt_Barcode.Text + "'";
                        }
                        else if (Convert.ToInt64(Dt1.Rows[0]["Length"].ToString()) == 14 && (Dt1.Rows[0]["First_Char"].ToString() == "G" || Dt1.Rows[0]["First_Char"].ToString() == "g" || Dt1.Rows[0]["First_Char"].ToString() == "S" || Dt1.Rows[0]["First_Char"].ToString() == "s") && Convert.ToInt64(Dt1.Rows[0]["Process"].ToString()) != 152)
                        {
                            Str = " Select 0 SNO, PROCESS, DATE, SHIFT, MACHINE, NAME, PRODUCTION, ORDER_NO, SAMPLE, SIZE, COLOR, BOMQTY, Total_Production, Balance from Get_Boarding_Barcode_Details('" + Txt_Barcode.Text + "') Order By ProcessId";
                        }
                        //else if (Convert.ToInt64(Dt1.Rows[0]["Length"].ToString()) == 14 && (Dt1.Rows[0]["First_Char"].ToString() == "G" || Dt1.Rows[0]["First_Char"].ToString() == "g" || Dt1.Rows[0]["First_Char"].ToString() == "S" || Dt1.Rows[0]["First_Char"].ToString() == "s") && Convert.ToInt64(Dt1.Rows[0]["Process"].ToString()) == 0)
                        //{
                        //    Str = " Select 0 SNO, PROCESS, DATE, SHIFT, MACHINE, NAME, PRODUCTION, ORDER_NO, SAMPLE, SIZE, COLOR, BOMQTY, Total_Production, Balance from Get_GreyStore_Barcode_Details('" + Txt_Barcode.Text + "') ";
                        //}
                        //else if (Convert.ToInt64(Dt1.Rows[0]["Length"].ToString()) == 14 && (Dt1.Rows[0]["First_Char"].ToString() == "G" || Dt1.Rows[0]["First_Char"].ToString() == "g" || Dt1.Rows[0]["First_Char"].ToString() == "S" || Dt1.Rows[0]["First_Char"].ToString() == "s") && Convert.ToInt64(Dt1.Rows[0]["Process"].ToString()) == 162)
                        //{
                        //    Str = " Select 0 SNO, PROCESS, DATE, SHIFT, MACHINE, NAME, PRODUCTION, ORDER_NO, SAMPLE, SIZE, COLOR, BOMQTY, Total_Production, Balance from Get_Linking_Barcode_Details('" + Txt_Barcode.Text + "') ";
                        //}
                        //else if (Convert.ToInt64(Dt1.Rows[0]["Length"].ToString()) == 14 && (Dt1.Rows[0]["First_Char"].ToString() == "G" || Dt1.Rows[0]["First_Char"].ToString() == "g" || Dt1.Rows[0]["First_Char"].ToString() == "S" || Dt1.Rows[0]["First_Char"].ToString() == "s") && Convert.ToInt64(Dt1.Rows[0]["Process"].ToString()) == 163)
                        //{
                        //    Str = " Select 0 SNO, PROCESS, DATE, SHIFT, MACHINE, NAME, PRODUCTION, ORDER_NO, SAMPLE, SIZE, COLOR, BOMQTY, Total_Production, Balance from Get_Washing_Barcode_Details('" + Txt_Barcode.Text + "') ";                            
                        //}
                        //else if (Convert.ToInt64(Dt1.Rows[0]["Length"].ToString()) == 14 && (Dt1.Rows[0]["First_Char"].ToString() == "G" || Dt1.Rows[0]["First_Char"].ToString() == "g" || Dt1.Rows[0]["First_Char"].ToString() == "S" || Dt1.Rows[0]["First_Char"].ToString() == "s") && Convert.ToInt64(Dt1.Rows[0]["Process"].ToString()) == 164)
                        //{
                        //    Str = " Select 0 SNO, PROCESS, DATE, SHIFT, MACHINE, NAME, PRODUCTION, ORDER_NO, SAMPLE, SIZE, COLOR, BOMQTY, Total_Production, Balance from Get_Boarding_Barcode_Details('" + Txt_Barcode.Text + "') Order By Processid";                            
                        //}
                        else
                        {
                            MessageBox.Show("Invalid Barcode...! Gainup");
                            Txt_Barcode.Text = "";
                            Txt_Barcode.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Barcode...! Gainup");
                        Txt_Barcode.Text = "";
                        Txt_Barcode.Focus();
                        return;
                    }

                    //if (MyParent._New == true)
                    //{
                    //    Str = "select 0 SNO , 'KNITTING' PROCESS, K2.EntryDate DATE, S1.shiftdesc SHIFT_DESC,  K1.MachineID MACHINE, E1.Name NAME,  K1.Production PRODUCTION ,  K1.Order_No ORDER_NO  , B3.color SAMPLE , S2.size SIZE ,IsNull(Im_Color,'-')COLOR, K1.BOMQty BOMQTY from  Socks_Barcode_Details B1 left join Socks_Bundle_Details B2 on B1.Socks_Bundle_Details_RowID = B2.RowID left join Floor_Knitting_Details K1 on B2.Floor_Knitting_Details_RowID = K1.RowID left join Floor_Knitting_Master K2 on K1.MasterID = K2.RowID left join Vaahini_ERP_Gainup.Dbo.Shiftmst S1 on S1.Shiftcode = K2.ShiftCode and compcode = 2 and mode = 1 left join VAAHINI_ERP_GAINUP.dbo.EmployeeMas E1 on E1.Emplno = K1.Emplno_Operator left join size S2 on K1.SizeID = S2.sizeid left join Socks_Bom() B3 on K1.Order_No = B3.Order_No and K1.sizeid = B3.SizeID and K1.OrderColorID = B3.OrderColorId where B1.Barcode ='" + Txt_Barcode.Text + "'";
                    //}
                    //else
                    //{
                    //    Str = "select 'KNITTING' PROCESS, K2.EntryDate DATE, S1.shiftdesc SHIFT_DESC,  K1.MachineID MACHINE, E1.Name NAME,  K1.Production PRODUCTION ,  K1.Order_No ORDER_NO  , B3.color SAMPLE , S2.size SIZE ,IsNull(Im_Color,'-')COLOR, K1.BOMQty BOMQTY from  Socks_Barcode_Details B1 left join Socks_Bundle_Details B2 on B1.Socks_Bundle_Details_RowID = B2.RowID left join Floor_Knitting_Details K1 on B2.Floor_Knitting_Details_RowID = K1.RowID left join Floor_Knitting_Master K2 on K1.MasterID = K2.RowID left join Vaahini_ERP_Gainup.Dbo.Shiftmst S1 on S1.Shiftcode = K2.ShiftCode and compcode = 2 and mode = 1 left join VAAHINI_ERP_GAINUP.dbo.EmployeeMas E1 on E1.Emplno = K1.Emplno_Operator left join size S2 on K1.SizeID = S2.sizeid left join Socks_Bom() B3 on K1.Order_No = B3.Order_No and K1.sizeid = B3.SizeID and K1.OrderColorID = B3.OrderColorId where B1.Barcode ='" + Txt_Barcode.Text + "'";
                    //}

                    Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                    if (Dt.Rows.Count > 0)
                    {
                        MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                        MyBase.Grid_Width(ref Grid, 40, 80, 100, 50, 120, 150, 80, 120, 90, 80, 120, 90, 100, 100);
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
