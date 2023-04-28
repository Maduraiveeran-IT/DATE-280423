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
    public partial class FrmSettingBarcode : Form,Entry
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        Int64 Code = 0;
        DataRow Dr;
        MDIMain MyParent;
        TextBox Txt = null;
        String Buffer_Table = String.Empty;
        

        public FrmSettingBarcode()
        {
            InitializeComponent();
        }

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                DtpDate1.Enabled = true;
                Grid_Data();
                DtpDate1.Focus();
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


                //for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                //{
                //    if (Grid["Production", i].Value == DBNull.Value || Grid["Production", i].Value.ToString() == String.Empty || Convert.ToInt64(Grid["Production", i].Value) == 0)
                //    {
                //        MessageBox.Show(" ZERO is Invalid in Row " + (i + 1) + "  ", "Gainup");
                //        Grid.CurrentCell = Grid["Production", i];
                //        Grid.Focus();
                //        Grid.BeginEdit(true);
                //        MyParent.Save_Error = true;
                //        return;
                //    }
                //}

                //Dt.AcceptChanges();
                Array_Index = 0;
                Queries = new String[(Dt.Rows.Count) + 2];

                TxtNo.Text = MyBase.MaxOnlyWithoutComp("Socks_Boarding_Barcode_Master", "EntryNo", String.Empty, "", 0).ToString();

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_Boarding_Barcode_Master (EntryNo, EntryDate, Shiftcode, Emplno_Operator, Machine, EntrySystem, EntryTime, Remarks) Values (" + TxtNo.Text + ",'" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', " + TxtShift.Tag + ", " + TxtOperator.Tag + ", '" + TxtMachine.Text + "', Host_Name(), Getdate(), '" + TxtRemarks.Text + "') ; Select Scope_Identity()";
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_Boarding_Barcode_Master Set Emplno_Operator = " + TxtOperator.Tag + ", ShiftCode = " + TxtShift.Tag + ", Machine = '" + TxtMachine.Text + "', Remarks = '" + TxtRemarks.Text + "' where RowID = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_Boarding_Barcode_Details where MasterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 2; i++)
                {
                    if (MyParent._New == true)
                    {
                        Queries[Array_Index++] = "Insert Into Socks_Boarding_Barcode_Details (MasterID, Board_Barcode, PRoduction) Values (@@IDENTITY, '" + Grid["Board_Barcode", i].Value.ToString() + "', " + Grid["Production", i].Value.ToString() + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert Into Socks_Boarding_Barcode_Details (MasterID, Board_Barcode, PRoduction) Values (" + Code + ", '" + Grid["Board_Barcode", i].Value.ToString() + "', " + Grid["Production", i].Value.ToString() + ")";
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

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                DtpDate1.Enabled = false;
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Boarding - Edit", " Select EntryNo, EntryDate, S2.Shiftcode2, E1.Name, S1.Machine, S1.Remarks, RowID, S1.shiftcode, S1.Emplno_Operator from Socks_Boarding_Barcode_Master S1 Left Join VAAHINI_ERP_GAINUP.dbo.shiftmst S2 on S1.Shiftcode = S2.shiftcode And S2.compcode = 2 And S2.Mode = 1 Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on S1.Emplno_Operator = E1.Emplno Order By EntryNo Desc", String.Empty, 70, 90, 70, 100, 100);
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
                throw ex;
            }
        }

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Boarding Barcode - Delete", " Select EntryNo, EntryDate, S2.Shiftcode2, E1.Name, S1.Machine, S1.Remarks, RowID, S1.shiftcode, S1.Emplno_Operator from Socks_Boarding_Barcode_Master S1 Left Join VAAHINI_ERP_GAINUP.dbo.shiftmst S2 on S1.Shiftcode = S2.shiftcode And S2.compcode = 2 And S2.Mode = 1 Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on S1.Emplno_Operator = E1.Emplno Order By EntryNo Desc", String.Empty, 70, 90, 70, 100, 100);
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
                    MyBase.Run("Delete From Socks_Boarding_Barcode_Details where MAsterID = " + Code, "Delete From Socks_Boarding_Barcode_Master where RowID = " + Code);
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Boarding Barcode - View", " Select EntryNo, EntryDate, S2.Shiftcode2, E1.Name, S1.Machine, S1.Remarks, RowID, S1.shiftcode, S1.Emplno_Operator from Socks_Boarding_Barcode_Master S1 Left Join VAAHINI_ERP_GAINUP.dbo.shiftmst S2 on S1.Shiftcode = S2.shiftcode And S2.compcode = 2 And S2.Mode = 1 Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on S1.Emplno_Operator = E1.Emplno Order By EntryNo Desc", String.Empty, 70, 90, 70, 100, 100);
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
            throw new NotImplementedException();
        }

        private void FrmSettingBarcode_Load(object sender, EventArgs e)
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

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                Code = Convert.ToInt64(Dr["RowID"]);
                TxtNo.Text = Dr["EntryNo"].ToString();
                DtpDate1.Value = Convert.ToDateTime(Dr["EntryDate"]);
                TxtShift.Text = Dr["ShiftCode2"].ToString();
                TxtShift.Tag = Dr["ShiftCode"].ToString();
                TxtOperator.Text = Dr["Name"].ToString();
                TxtOperator.Tag = Dr["Emplno_Operator"].ToString();
                TxtMachine.Text = Dr["Machine"].ToString();
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
                    Str = "Select 0 as SNO, S2.Board_Barcode, Production, '-' T from Socks_Boarding_Barcode_Master S1 Left Join Socks_Boarding_Barcode_Details S2 on S1.RowID = S2.MasterID Where 1 = 2";

                    //Str = " Select 0 as SNO, S2.Board_Barcode, Sum(F1.Production)Production, '-' T from Socks_Boarding_Barcode_Master S1 Left Join Socks_Boarding_Barcode_Details S2 on S1.RowID = S2.MasterID";
                    //Str = Str + " Left Join Socks_GreyStore_Barcode_mapping_Master S3 on S2.Board_Barcode = S3.Board_BarcodeNo Left Join Socks_GreyStore_Barcode_mapping_Details S4 On S3.RowID = S4.MasterID";
                    //Str = Str + " Left Join Socks_Barcode_Details S5 on S4.Knit_Barcode = S5.Barcode Left Join Socks_Bundle_Details S6 on S5.Socks_Bundle_Details_RowID = S6.RowID";
                    //Str = Str + " Left Join Floor_Knitting_Details F1 On S6.Floor_Knitting_Details_RowID = F1.RowID Where 1 = 2 Group By (S2.Board_Barcode)";
                }
                else
                {
                    Str = "Select 0 as SNO, S2.Board_Barcode, Production, '-' T from Socks_Boarding_Barcode_Master S1 Left Join Socks_Boarding_Barcode_Details S2 on S1.RowID = S2.MasterID Where EntryNO = " + TxtNo.Text + "";

                    //Str = " Select 0 as SNO, S2.Board_Barcode, Sum(F1.Production)Production, '-' T from Socks_Boarding_Barcode_Master S1 Left Join Socks_Boarding_Barcode_Details S2 on S1.RowID = S2.MasterID";
                    //Str = Str + " Left Join Socks_GreyStore_Barcode_mapping_Master S3 on S2.Board_Barcode = S3.Board_BarcodeNo Left Join Socks_GreyStore_Barcode_mapping_Details S4 On S3.RowID = S4.MasterID";
                    //Str = Str + " Left Join Socks_Barcode_Details S5 on S4.Knit_Barcode = S5.Barcode Left Join Socks_Bundle_Details S6 on S5.Socks_Bundle_Details_RowID = S6.RowID";
                    //Str = Str + " Left Join Floor_Knitting_Details F1 On S6.Floor_Knitting_Details_RowID = F1.RowID Where S1.EntryNo = '" + TxtNo.Text + "' Group By (S2.Board_Barcode)";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);

                MyBase.Grid_Designing(ref Grid, ref Dt, "T");

                MyBase.ReadOnly_Grid_Without(ref Grid, "Board_Barcode");
                MyBase.Grid_Width(ref Grid, 40, 140, 100);
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
                TxtTotal.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "Production", "Board_Barcode")));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmSettingBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtMachine")
                    {
                        Grid.CurrentCell = Grid["Board_Barcode", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
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
                    if (this.ActiveControl.Name == "TxtShift")
                    {
                        Shift_Selection();
                    }
                    else if (this.ActiveControl.Name == "TxtOperator")
                    {
                        Operator_Selection();
                    }
                    else if (this.ActiveControl.Name == "TxtMachine")
                    {
                        Machine_Selection();
                    }
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

        void Shift_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Shift", "Select ShiftCode2 Shift, StartTime, EndTime, ShiftCode From Socks_Shift ()", String.Empty, 80, 80, 80);
                if (Dr != null)
                {
                    TxtShift.Text = Dr["Shift"].ToString();
                    TxtShift.Tag = Dr["ShiftCode"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Operator_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Operator", "Select Name, Tno, Emplno From Socks_Employee_Present_Detail ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "') Where DeptName = 'BOARDING' and Tno Not Like '%Z'", String.Empty, 250, 80);
                if (Dr != null)
                {
                    TxtOperator.Text = Dr["Name"].ToString();
                    TxtOperator.Tag = Dr["Emplno"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmSettingBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtRemarks" || this.ActiveControl.Name == "TxtShift" || this.ActiveControl.Name == "TxtOperator" || this.ActiveControl.Name == "TxtMachine")
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Board_Barcode"].Index)
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Board_Barcode"].Index)
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

      

        void Machine_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine From Setting_Mc_NO ()", String.Empty, 200);
                if (Dr != null)
                {
                    TxtMachine.Text = Dr["Machine"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Board_Barcode"].Index)
                    {
                        DataTable Dt1 = new DataTable();
                        DataTable Dt2 = new DataTable();
                        DataTable Dt3 = new DataTable();
                        DataTable Dt4 = new DataTable();

                        if (Grid["Board_Barcode", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            MyBase.Load_Data("Select Len('" + Grid["Board_Barcode", Grid.CurrentCell.RowIndex].Value.ToString() + "')Len", ref Dt1);
                            if (Dt1.Rows.Count > 0)
                            {
                                if (Convert.ToInt64(Dt1.Rows[0]["Len"].ToString()) == 14)
                                {
                                    MyBase.Load_Data("Select SUBSTRING('" + Grid["Board_Barcode", Grid.CurrentCell.RowIndex].Value.ToString() + "',1,1)Dept", ref Dt2);
                                    {
                                        if (Dt2.Rows.Count > 0)
                                        {
                                            if (Dt2.Rows[0]["Dept"].ToString() == "S" || Dt2.Rows[0]["Dept"].ToString() == "s")
                                            {
                                                MyBase.Load_Data("Select SUBSTRING('" + Grid["Board_Barcode", Grid.CurrentCell.RowIndex].Value.ToString() + "',12,3)Processid", ref Dt3);
                                                {
                                                    if (Dt3.Rows.Count > 0)
                                                    {
                                                        if (Convert.ToInt64(Dt3.Rows[0]["Processid"].ToString()) == 164)
                                                        {
                                                            MyBase.Load_Data(" select Grey_BarcodeNo, Production from Get_Board_Barcode ('" + Grid["Board_Barcode", Grid.CurrentCell.RowIndex].Value.ToString() + "') ", ref Dt4);
                                                            if (Dt4.Rows.Count > 0)
                                                            {
                                                                //Grid["Process", Grid.CurrentCell.RowIndex].Value = Dt4.Rows[0]["Process"];
                                                                Grid["Production", Grid.CurrentCell.RowIndex].Value = Dt4.Rows[0]["Production"];
                                                                Txt.Text = Dt4.Rows[0]["Production"].ToString();
                                                            }
                                                            else
                                                            {
                                                                //MessageBox.Show("Data Not Available For This Boarding barcode !..Gainup");
                                                                //Grid["Board_Barcode", Grid.CurrentCell.RowIndex].Value = "";
                                                                //Grid.CurrentCell = Grid["Board_Barcode", Grid.CurrentCell.RowIndex];
                                                                //Grid.Focus();
                                                                //Grid.BeginEdit(true);
                                                                //return;
                                                                Grid["Production", Grid.CurrentCell.RowIndex].Value = "0";
                                                                Txt.Text = "0";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("Invalid Boarding barcode !..Gainup");
                                                            Grid["Board_Barcode", Grid.CurrentCell.RowIndex].Value = "";
                                                            Grid.CurrentCell = Grid["Board_Barcode", Grid.CurrentCell.RowIndex];
                                                            Grid.Focus();
                                                            Grid.BeginEdit(true);
                                                            return;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Invalid Boarding barcode !..Gainup");
                                                        Grid["Board_Barcode", Grid.CurrentCell.RowIndex].Value = "";
                                                        Grid.CurrentCell = Grid["Board_Barcode", Grid.CurrentCell.RowIndex];
                                                        Grid.Focus();
                                                        Grid.BeginEdit(true);
                                                        return;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Invalid Boarding barcode !..Gainup");
                                                Grid["Board_Barcode", Grid.CurrentCell.RowIndex].Value = "";
                                                Grid.CurrentCell = Grid["Board_Barcode", Grid.CurrentCell.RowIndex];
                                                Grid.Focus();
                                                Grid.BeginEdit(true);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Invalid Boarding barcode !..Gainup");
                                            Grid["Board_Barcode", Grid.CurrentCell.RowIndex].Value = "";
                                            Grid.CurrentCell = Grid["Board_Barcode", Grid.CurrentCell.RowIndex];
                                            Grid.Focus();
                                            Grid.BeginEdit(true);
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Invalid Boarding barcode !..Gainup");
                                    Grid["Board_Barcode", Grid.CurrentCell.RowIndex].Value = "";
                                    Grid.CurrentCell = Grid["Board_Barcode", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid Boarding barcode !..Gainup");
                                Grid["Board_Barcode", Grid.CurrentCell.RowIndex].Value = "";
                                Grid.CurrentCell = Grid["Board_Barcode", Grid.CurrentCell.RowIndex];
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

        private void Grid_KeyPress_1(object sender, KeyPressEventArgs e)
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
    }
}
