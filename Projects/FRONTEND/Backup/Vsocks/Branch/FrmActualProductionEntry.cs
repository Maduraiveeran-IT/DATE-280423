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
    public partial class FrmActualProductionEntry : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        Int64 Code = 0;
        DataRow Dr;
        MDIMain MyParent;
        TextBox Txt = null;
        String Buffer_Table = String.Empty;
        Boolean Buffer_Update = false;

        public FrmActualProductionEntry()
        {
            InitializeComponent();
        }

        private void FrmActualProductionEntry_Load(object sender, EventArgs e)
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

        void Set_Min_Max_Date(Boolean Condition)
        {
            try
            {
                DataTable Tdt = new DataTable();
                if (Condition)
                {
                    MyBase.Load_Data("Select DateAdd (d, " + MyParent.User_Datelock + ", Cast(GetDate() as Date)) MinDate, Cast(GetDate() as Date) MaxDate ", ref Tdt);
                    DtpDate1.MinDate = Convert.ToDateTime(Tdt.Rows[0][0]);
                    DtpDate1.MaxDate = Convert.ToDateTime(Tdt.Rows[0][1]);
                }
                else
                {
                    DtpDate1.MinDate = Convert.ToDateTime("01-Apr-2014");
                    DtpDate1.MaxDate = Convert.ToDateTime("31-Mar-2030");
                }
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
                Set_Min_Max_Date(true);
                Grid_Data();
                DtpDate1.Focus();
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
                TxtShift.Text = Dr["Shiftcode2"].ToString();
                TxtShift.Tag = Dr["ShiftCode"].ToString();
                TxtUnit.Text = Dr["Unit"].ToString();
                TxtUnit.Tag = Dr["UnitCode"].ToString();
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
                Set_Min_Max_Date(true);
                DtpDate1.Enabled = false;
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Production - Edit", "Select Distinct S4.Unit_Name Unit, S1.EntryDate, S3.shiftcode2, S1.EntryNo, S1.Rowid, S1.ShiftCode, S1.UnitCode, S1.Remarks from Socks_Floor_Actual_Production_Master S1 Left Join Socks_Floor_Actual_Production_Details S2 On S1.RowId = S2.MasterId Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.Shiftcode = S3.shiftcode And S3.compcode = 2 And S3.Mode = 1 And S3.shiftcode in (15, 16, 17) Left Join Socks_Unit_Master S4 On S1.UnitCode = S4.RowID Where EntryDate >= Dateadd (D, " + MyParent.User_Datelock + ", Cast(GetDate() as Date)) Order By S1.EntryDate Desc", String.Empty, 80, 90, 70);
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
                    for (int j = 0; j < Grid.Columns.Count - 1; j++)
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


                Dt.AcceptChanges();
                Array_Index = 0;
                Queries = new String[(Dt.Rows.Count * 1) + 3];

                TxtNo.Text = MyBase.MaxOnlyWithoutComp("Socks_Floor_Actual_Production_Master", "EntryNo", String.Empty, String.Empty, 0).ToString();

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_Floor_Actual_Production_Master (EntryNo, EntryDate, ShiftCode, UnitCode, EntryAt, EntrySystem, Remarks) Values (" + TxtNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', " + TxtShift.Tag.ToString() + ", " + TxtUnit.Tag + ", getdate(), Host_name(), '" + TxtRemarks.Text + "') ; Select Scope_Identity()";
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_Floor_Actual_Production_Master Set ShiftCode = " + TxtShift.Tag.ToString() + ", Remarks = '" + TxtRemarks.Text + "' where RowID = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_Floor_Actual_Production_Details Where MasterId = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New == true)
                    {
                        Queries[Array_Index++] = "Insert Into Socks_Floor_Actual_Production_Details (MasterID, Knitting, Linking, Washing, Boarding, Pairing, Packing) Values (@@IDENTITY, " + Grid["Knitting", i].Value + ", " + Grid["Linking", i].Value + ", " + Grid["Washing", i].Value.ToString() + ", " + Grid["Boarding", i].Value + ", " + Grid["Pairing", i].Value + ", " + Grid["Packing", i].Value + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert Into Socks_Floor_Actual_Production_Details (MasterID, Knitting, Linking, Washing, Boarding, Pairing, Packing) Values (" + Code + ", " + Grid["Knitting", i].Value + ", " + Grid["Linking", i].Value + ", " + Grid["Washing", i].Value.ToString() + ", " + Grid["Boarding", i].Value + ", " + Grid["Pairing", i].Value + ", " + Grid["Packing", i].Value + ")";
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
                Set_Min_Max_Date(true);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Production - Delete", "Select Distinct S4.Unit_Name Unit, S1.EntryDate, S3.shiftcode2, S1.EntryNo, S1.Rowid, S1.ShiftCode, S1.UnitCode, S1.Remarks from Socks_Floor_Actual_Production_Master S1 Left Join Socks_Floor_Actual_Production_Details S2 On S1.RowId = S2.MasterId Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.Shiftcode = S3.shiftcode And S3.compcode = 2 And S3.Mode = 1 And S3.shiftcode in (15, 16, 17) Left Join Socks_Unit_Master S4 On S1.UnitCode = S4.RowID Order By S1.EntryDate Desc", String.Empty, 80, 90, 70);
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
                    MyBase.Run("Delete From Socks_Floor_Actual_Production_Details where MAsterID = " + Code, "Delete From Socks_Floor_Actual_Production_Master where RowID = " + Code);
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
                Set_Min_Max_Date(false);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Knitting - View", "Select Distinct S4.Unit_Name Unit, S1.EntryDate, S3.shiftcode2, S1.EntryNo, S1.Rowid, S1.ShiftCode, S1.UnitCode, S1.Remarks from Socks_Floor_Actual_Production_Master S1 Left Join Socks_Floor_Actual_Production_Details S2 On S1.RowId = S2.MasterId Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.Shiftcode = S3.shiftcode And S3.compcode = 2 And S3.Mode = 1 And S3.shiftcode in (15, 16, 17) Left Join Socks_Unit_Master S4 On S1.UnitCode = S4.RowID Order By S1.EntryDate Desc", String.Empty, 80, 90, 70);
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
                    Str = "Select 0 As Slno, Knitting, Linking, Washing, Boarding, Pairing, Packing, ''T From Socks_Floor_Actual_Production_Master S1 Left Join Socks_Floor_Actual_Production_Details S2 on S1.RowId = S2.MasterId Where 1 = 2 ";
                }
                else
                {
                    Str = "Select 0 As Slno, Knitting, Linking, Washing, Boarding, Pairing, Packing, ''T From Socks_Floor_Actual_Production_Master S1 Left Join Socks_Floor_Actual_Production_Details S2 on S1.RowId = S2.MasterId Where S1.Rowid = " + Code + " ";
                    
                }
                
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);

                MyBase.Grid_Designing(ref Grid, ref Dt, "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Knitting", "Linking", "Washing", "Boarding", "Pairing", "Packing");
                MyBase.Grid_Width(ref Grid, 50, 100, 100, 100, 100);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                Grid.RowHeadersWidth = 10;

                Grid.Columns["Knitting"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Knitting"].DefaultCellStyle.Format = "0";

                Grid.Columns["Linking"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Linking"].DefaultCellStyle.Format = "0";

                Grid.Columns["Washing"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Washing"].DefaultCellStyle.Format = "0";

                Grid.Columns["Boarding"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Boarding"].DefaultCellStyle.Format = "0";

                Grid.Columns["Pairing"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Pairing"].DefaultCellStyle.Format = "0";

                Grid.Columns["Packing"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Packing"].DefaultCellStyle.Format = "0";

            }
            catch (Exception ex)
            {
                throw ex;
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

        void Unit_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Unit", "Select Unit_Name Unit, RowId UnitCode From Socks_Unit_Master", String.Empty, 80, 80);
                if (Dr != null)
                {
                    TxtUnit.Text = Dr["Unit"].ToString();
                    TxtUnit.Tag = Dr["UnitCode"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmActualProductionEntry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtUnit")
                    {
                        Grid.CurrentCell = Grid["Knitting", 0];
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
                    else if (this.ActiveControl.Name == "TxtUnit")
                    {
                        Unit_Selection();
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

        private void FrmActualProductionEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtRemarks")
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Boarding"].Index)
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Knitting"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Linking"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Washing"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Boarding"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Pairing"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Packing"].Index)
                {
                    MyBase.Valid_Number(Txt, e);
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

        void Total_Prod_Qty()
        {
            try
            {
                TxtTotal.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "Knitting", "Linking", "Washing", "Boarding")));
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Knitting"].Index)
                    {
                        if (Grid["Knitting", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Knitting", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Linking"].Index)
                    {
                        if (Grid["Linking", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Linking", Grid.CurrentCell.RowIndex].Value) <= 0)
                        {
                            Grid["Linking", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Washing"].Index)
                    {
                        if (Grid["Washing", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Washing", Grid.CurrentCell.RowIndex].Value) <= 0)
                        {
                            Grid["Washing", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Boarding"].Index)
                    {
                        if (Grid["Boarding", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Boarding", Grid.CurrentCell.RowIndex].Value) <= 0)
                        {
                            Grid["Boarding", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Pairing"].Index)
                    {
                        if (Grid["Pairing", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Pairing", Grid.CurrentCell.RowIndex].Value) <= 0)
                        {
                            Grid["Pairing", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Packing"].Index)
                    {
                        if (Grid["Packing", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Packing", Grid.CurrentCell.RowIndex].Value) <= 0)
                        {
                            Grid["Packing", Grid.CurrentCell.RowIndex].Value = "0";
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
                MessageBox.Show(ex.Message);
            }
        }
    }
}
