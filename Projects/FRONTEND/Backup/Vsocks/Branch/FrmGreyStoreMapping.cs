using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using SelectionTool_NmSp;
using Accounts_ControlModules;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmGreyStoreMapping : Form, Entry
    {
        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        Int64 Code = 0;
        Int32 Grid_Row = 0;
        Int32 Grid_Col = 0;
        public double Utilization = 0;
        public int Assign_Qty = 0;
        DataTable Dt = new DataTable();
        TextBox Txt = null;
        DataRow Dr;

        public FrmGreyStoreMapping()
        {
            InitializeComponent();
        }

        private void FrmGreyStoreMapping_Load(object sender, EventArgs e)
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
                DtpDate1.Enabled = false;
                Grid_Data();
                TxtBarcode.Focus(); 
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
                TxtNo.Text = Dr["EntryNo"].ToString();
                DtpDate1.Value = Convert.ToDateTime(Dr["EntryDate"]);
                TxtBarcode.Text = Dr["Grey_BarcodeNo"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();
                Grid_Data();
                MyBase.Row_Number(ref Grid);
                Total_Prod_Qty();
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
                DtpDate1.Enabled = false;
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Barcode - Edit", " Select EntryNo, EntryDate, Grey_BarcodeNo, RowID, Remarks from Socks_GreyStore_Barcode_mapping_Master Order By EntryNO desc", String.Empty, 80, 90, 70);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                }
                else
                {
                    Code = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Save()
        {
            String[] Queries;
            Int32 Array_Index = 0;
            double Line_Flag = 0;
            try
            {
                MyBase.Row_Number(ref Grid);
                Total_Prod_Qty();

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    MyParent.Save_Error = true;
                    DtpDate1.Focus();
                    return;
                }

                for (int i = 0; i <= Grid.Rows.Count - 2; i++)
                {
                    for (int j = 0; j < Grid.Columns.Count - 11; j++)
                    {
                        if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty)
                        {
                            MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid Column  in Row " + (i + 1) + "  ", "Gainup");
                            Grid.CurrentCell = Grid[j, i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                }


                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Production", i].Value == DBNull.Value || Grid["Production", i].Value.ToString() == String.Empty || Convert.ToInt64(Grid["Production", i].Value) == 0)
                    {
                        MessageBox.Show(" ZERO is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Production", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }

                //Dt.AcceptChanges();
                Array_Index = 0;
                Queries = new String[(Dt.Rows.Count) + 5];

                TxtNo.Text = MyBase.MaxOnlyWithoutComp("Socks_GreyStore_Barcode_mapping_Master", "EntryNo", String.Empty, "", 0).ToString();

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_GreyStore_Barcode_mapping_Master (EntryNo, EntryDate, Grey_BarcodeNo, Remarks, EntrySystem, EntryAt) Values (" + TxtNo.Text + ",'" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "','" + TxtBarcode.Text.ToString() + "',  '" + TxtRemarks.Text + "', Host_name(), getdate()) ; Select Scope_Identity()";
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_GreyStore_Barcode_mapping_Master Set Grey_BarcodeNo = '" + TxtBarcode.Text.ToString() + "', Remarks = '" + TxtRemarks.Text + "' where RowID = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_GreyStore_Barcode_mapping_Details where MasterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New == true)
                    {
                        Queries[Array_Index++] = "Insert Into Socks_GreyStore_Barcode_mapping_Details (MasterID, Knit_Barcode) Values (@@IDENTITY, '" + Grid["Knit_Barcode", i].Value.ToString() + "')";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert Into Socks_GreyStore_Barcode_mapping_Details (MasterID, Knit_Barcode) Values (" + Code + ", '" + Grid["Knit_Barcode", i].Value.ToString() + "')";
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
                MyBase.Clear(this);

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
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Barcode - Delete", "  Select EntryNo, EntryDate, Grey_BarcodeNo, RowID, Remarks from Socks_GreyStore_Barcode_mapping_Master Order By EntryNO desc", String.Empty, 80, 90, 70);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    MyParent.Load_DeleteConfirmEntry();
                }
                else
                {
                    Code = 0;
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
                    MyBase.Run("Delete From Socks_GreyStore_Barcode_mapping_Detials where MAsterID = " + Code, "Delete From Socks_GreyStore_Barcode_mapping_Master where RowID = " + Code);
                    MessageBox.Show("Deleted ...!", "Vaahini");
                    MyBase.Clear(this);
                    MyParent.Load_DeleteEntry();
                }
                else
                {
                    MessageBox.Show("Invalid Entry to Delete ...!", "Gainup");
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
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, false);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Barcode - View", " Select EntryNo, EntryDate, Grey_BarcodeNo, RowID, Remarks from Socks_GreyStore_Barcode_mapping_Master Order By EntryNO desc", String.Empty, 80, 90, 70);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                }
                else
                {
                    Code = 0;
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

        void Grid_Data()
        {
            String Str = String.Empty;
            DataTable Tdt = new DataTable();
            int month = DtpDate1.Value.Month;
            int day = DtpDate1.Value.Day;
            int year = DtpDate1.Value.Year;
            try
            {
                if (MyParent._New)
                {
                    Str = "Select 0 As Slno, S2.Knit_Barcode, 'Knitting' process, F1.Production, F1.MachineID Machine, E1.Name Operator, '_' T  from Socks_GreyStore_Barcode_mapping_Master S1 Left Join Socks_GreyStore_Barcode_mapping_Details S2 On S1.Rowid = S2.MasterID Left Join Socks_Barcode_Details S3 on S2.Knit_Barcode = S3.Barcode Left Join Socks_Bundle_Details S4 On S3.Socks_Bundle_Details_RowID = S4.RowID  Left Join Floor_Knitting_Details F1 On S4.Floor_Knitting_Details_RowID = F1.RowID Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Where 1 = 2 ";
                }
                else
                {
                    Str = "Select 0 As Slno, S2.Knit_Barcode, 'Knitting' process, F1.Production, F1.MachineID Machine, E1.Name Operator, '_' T  from Socks_GreyStore_Barcode_mapping_Master S1 Left Join Socks_GreyStore_Barcode_mapping_Details S2 On S1.Rowid = S2.MasterID Left Join Socks_Barcode_Details S3 on S2.Knit_Barcode = S3.Barcode Left Join Socks_Bundle_Details S4 On S3.Socks_Bundle_Details_RowID = S4.RowID  Left Join Floor_Knitting_Details F1 On S4.Floor_Knitting_Details_RowID = F1.RowID Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Where S1.EntryNO = " + TxtNo.Text + "";
                }                    
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                
                MyBase.Grid_Designing(ref Grid, ref Dt, "T");
                
                MyBase.ReadOnly_Grid_Without(ref Grid, "Knit_Barcode");
                MyBase.Grid_Width(ref Grid, 40, 140, 80, 70, 140, 80);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                Grid.RowHeadersWidth = 10;

                Grid.Columns["Production"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Production"].DefaultCellStyle.Format = "0";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Total_Prod_Qty()
        {
            try
            {
                TxtTotal.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "Production", "Process", "Knit_Barcode")));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmGreyStoreMapping_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtBarcode")
                    {
                        DataTable Dt1 = new DataTable();
                        DataTable Dt2 = new DataTable();
                        DataTable Dt3 = new DataTable();

                        MyBase.Load_Data("Select Len('" + TxtBarcode.Text.ToString() + "')Len", ref Dt1);
                        if (Dt1.Rows.Count > 0)
                        {
                            if (Convert.ToInt64(Dt1.Rows[0]["Len"].ToString()) == 14)
                            {
                                MyBase.Load_Data("Select SUBSTRING('" + TxtBarcode.Text.ToString() + "',1,1)Dept", ref Dt2);
                                {
                                    if (Dt2.Rows.Count > 0)
                                    {
                                        if (Dt2.Rows[0]["Dept"].ToString() == "S" || Dt2.Rows[0]["Dept"].ToString() == "s")
                                        {
                                            MyBase.Load_Data("Select SUBSTRING('" + TxtBarcode.Text.ToString() + "',12,3)Processid", ref Dt3);
                                            {
                                                if (Dt3.Rows.Count > 0)
                                                {
                                                    if (Convert.ToInt64(Dt3.Rows[0]["Processid"].ToString()) == 0)
                                                    {
                                                        Grid.CurrentCell = Grid["Knit_Barcode", 0];
                                                        Grid.Focus();
                                                        Grid.BeginEdit(true);
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Invalid barcode !..Gainup");
                                                        TxtBarcode.Text = "";
                                                        TxtBarcode.Focus();
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Invalid barcode !..Gainup");
                                                    TxtBarcode.Text = "";
                                                    TxtBarcode.Focus();
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Invalid barcode !..Gainup");
                                            TxtBarcode.Text = "";
                                            TxtBarcode.Focus();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid barcode !..Gainup");
                                        TxtBarcode.Text = "";
                                        TxtBarcode.Focus();
                                        return;
                                    }
                                }

                            }
                            else
                            {
                                MessageBox.Show("Invalid barcode !..Gainup");
                                TxtBarcode.Text = "";
                                TxtBarcode.Focus();
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid barcode !..Gainup");
                            TxtBarcode.Text = "";
                            TxtBarcode.Focus(); 
                            return; 
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtRemarks")
                    {
                        TxtTotal.Focus();
                    }
                    else if (this.ActiveControl.Name == "TxtTotal")
                    {
                        if (MyParent._New || MyParent.Edit)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    //if (this.ActiveControl.Name == "TxtShift")
                    //{
                    //    Shift_Selection();
                    //}
                }
                else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Delete)
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

        private void FrmGreyStoreMapping_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtRemarks" || this.ActiveControl.Name == "TxtBarcode")
                    {
                    }
                    else if (this.ActiveControl.Name == String.Empty)
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
                    Txt.GotFocus += new EventHandler(Txt_GotFocus);
                    Txt.TextChanged += new EventHandler(Txt_TextChanged);
                    Txt.LostFocus += new EventHandler(Txt_LostFocus);
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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_LostFocus(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Knit_Barcode"].Index)
                {
                    if (Grid["Knit_Barcode", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        
                    }
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Knit_Barcode"].Index)
                {
                    Total_Prod_Qty();
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Knit_Barcode"].Index)
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

        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {

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

        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    TxtRemarks.Focus();
                    TxtRemarks.SelectAll();
                    SendKeys.Send("{End}");
                }
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Knit_Barcode"].Index)
                    {
                        DataTable Dt1 = new DataTable();
                        DataTable Dt2 = new DataTable();
                        DataTable Dt3 = new DataTable();
                        DataTable Dt4 = new DataTable();

                        if (Grid["Knit_Barcode", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            MyBase.Load_Data("Select Len('" + Grid["Knit_Barcode", Grid.CurrentCell.RowIndex].Value.ToString() + "')Len", ref Dt1);
                            if (Dt1.Rows.Count > 0)
                            {
                                if (Convert.ToInt64(Dt1.Rows[0]["Len"].ToString()) == 14)
                                {
                                    MyBase.Load_Data("Select SUBSTRING('" + Grid["Knit_Barcode", Grid.CurrentCell.RowIndex].Value.ToString() + "',1,1)Dept", ref Dt2);
                                    {
                                        if (Dt2.Rows.Count > 0)
                                        {
                                            if (Dt2.Rows[0]["Dept"].ToString() == "G" || Dt2.Rows[0]["Dept"].ToString() == "g" || Dt2.Rows[0]["Dept"].ToString() == "S" || Dt2.Rows[0]["Dept"].ToString() == "s")
                                            {
                                                MyBase.Load_Data("Select SUBSTRING('" + Grid["Knit_Barcode", Grid.CurrentCell.RowIndex].Value.ToString() + "',12,3)Processid", ref Dt3);
                                                {
                                                    if (Dt3.Rows.Count > 0)
                                                    {
                                                        if (Convert.ToInt64(Dt3.Rows[0]["Processid"].ToString()) == 152)
                                                        {
                                                            MyBase.Load_Data(" select 'Knitting' Process, F1.MachineID Machine, E1.Name Operator, F1.Production from Socks_Barcode_Details S1 Left Join Socks_Bundle_Details S2 On S1.Socks_Bundle_Details_RowID = S2.RowID Left Join Floor_Knitting_Details F1 On S2.Floor_Knitting_Details_RowID = F1.RowID Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 On F1.Emplno_Operator = E1.Emplno Where S1.Barcode = '" + Grid["Knit_Barcode", Grid.CurrentCell.RowIndex].Value.ToString() + "' ", ref Dt4);
                                                            if (Dt4.Rows.Count > 0)
                                                            {
                                                                Grid["Process", Grid.CurrentCell.RowIndex].Value = Dt4.Rows[0]["Process"];
                                                                Grid["Machine", Grid.CurrentCell.RowIndex].Value = Dt4.Rows[0]["Machine"];
                                                                Grid["Operator", Grid.CurrentCell.RowIndex].Value = Dt4.Rows[0]["Operator"];
                                                                Grid["Production", Grid.CurrentCell.RowIndex].Value = Dt4.Rows[0]["Production"];
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("Data Not Available For This Knitting barcode !..Gainup");
                                                                Grid["Knit_Barcode", Grid.CurrentCell.RowIndex].Value = "";
                                                                Grid.CurrentCell = Grid["Knit_Barcode", Grid.CurrentCell.RowIndex];
                                                                Grid.Focus();
                                                                Grid.BeginEdit(true);
                                                                return;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("Invalid Knitting barcode !..Gainup");
                                                            Grid["Knit_Barcode", Grid.CurrentCell.RowIndex].Value = "";
                                                            Grid.CurrentCell = Grid["Knit_Barcode", Grid.CurrentCell.RowIndex];
                                                            Grid.Focus();
                                                            Grid.BeginEdit(true);
                                                            return;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Invalid Knitting barcode !..Gainup");
                                                        Grid["Knit_Barcode", Grid.CurrentCell.RowIndex].Value = "";
                                                        Grid.CurrentCell = Grid["Knit_Barcode", Grid.CurrentCell.RowIndex];
                                                        Grid.Focus();
                                                        Grid.BeginEdit(true);
                                                        return;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Invalid Knitting barcode !..Gainup");
                                                Grid["Knit_Barcode", Grid.CurrentCell.RowIndex].Value = "";
                                                Grid.CurrentCell = Grid["Knit_Barcode", Grid.CurrentCell.RowIndex];
                                                Grid.Focus();
                                                Grid.BeginEdit(true);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Invalid Knitting barcode !..Gainup");
                                            Grid["Knit_Barcode", Grid.CurrentCell.RowIndex].Value = "";
                                            Grid.CurrentCell = Grid["Knit_Barcode", Grid.CurrentCell.RowIndex];
                                            Grid.Focus();
                                            Grid.BeginEdit(true);
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Invalid Knitting barcode !..Gainup");
                                    Grid["Knit_Barcode", Grid.CurrentCell.RowIndex].Value = "";
                                    Grid.CurrentCell = Grid["Knit_Barcode", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid Knitting barcode !..Gainup");
                                Grid["Knit_Barcode", Grid.CurrentCell.RowIndex].Value = "";
                                Grid.CurrentCell = Grid["Knit_Barcode", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
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

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentCell.RowIndex);
                Total_Prod_Qty();
                MyBase.Row_Number(ref Grid);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("does not have a value"))
                {

                }
                else if (ex.Message.Contains("There is no row"))
                {

                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}