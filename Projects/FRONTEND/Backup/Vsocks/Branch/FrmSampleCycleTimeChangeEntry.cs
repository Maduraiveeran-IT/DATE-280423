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
    public partial class FrmSampleCycleTimeChangeEntry : Form, Entry 
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        Int64 Code = 0;
        DataRow Dr;
        MDIMain MyParent;
        TextBox Txt = null;
        
        public FrmSampleCycleTimeChangeEntry()
        {
            InitializeComponent();
        }

        private void FrmSampleCycleTimeChangeEntry_Load(object sender, EventArgs e)
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
                    //MyBase.Load_Data("Select DateAdd (d, " + MyParent.User_Datelock + ", Cast(GetDate() as Date)) MinDate, Cast(GetDate() as Date) MaxDate ", ref Tdt);
                    MyBase.Load_Data("Select Cast(GetDate() as Date) MinDate, DateAdd (d, 28, Cast(GetDate() as Date)) MaxDate ", ref Tdt);
                    DtpDate1.MinDate = Convert.ToDateTime(Tdt.Rows[0][0]);
                    DtpDate1.MaxDate = Convert.ToDateTime(Tdt.Rows[0][1]);
                }
                else
                {
                    DtpDate1.MinDate = Convert.ToDateTime("01-Apr-2014");
                    DtpDate1.MaxDate = Convert.ToDateTime("31-Mar-2050");
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
                DtpDate1.Value = Convert.ToDateTime(Dr["EffectFrom"]);
                TxtShift.Text = Dr["EffectFromShift"].ToString();
                TxtWeek.Text = Dr["Week"].ToString();
                TxtYear.Text = Dr["Year"].ToString();
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Cycle Time Entry - Edit", "select EntryNo, EffectFrom, EffectFromShift, Week, Year, Remarks, Rowid from Sample_Cycle_Time_Entry_Master Where EffectFrom >= Dateadd (D, " + MyParent.User_Datelock + ", Cast(GetDate() as Date))", String.Empty, 80, 90, 70, 60, 60);
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


                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["New_Cycle_Pair", i].Value == DBNull.Value || Grid["New_Cycle_Pair", i].Value.ToString() == String.Empty || Grid["New_Cycle_Pair", i].Value.ToString() == "00:00")
                    {
                        MessageBox.Show(" ZERO is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["New_Cycle_Pair", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }

                //Dt.AcceptChanges();
                Array_Index = 0;
                Queries = new String[(Dt.Rows.Count * 1)+2 ];

                TxtNo.Text = MyBase.MaxOnlyWithoutComp("Sample_Cycle_Time_Entry_Master", "EntryNo", String.Empty, String.Empty, 0).ToString();

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Sample_Cycle_Time_Entry_Master (EntryNo, EffectFrom, EffectFromShift, EntryDate, EntrySystem, Remarks) Values (" + TxtNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', '" + TxtShift.Text + "', getdate(), Host_name(), '" + TxtRemarks.Text + "') ; Select Scope_Identity()";
                }
                else
                {
                    Queries[Array_Index++] = "Update Sample_Cycle_Time_Entry_Master Set EffectFromShift = " + TxtShift.Text.ToString() + ", Remarks = '" + TxtRemarks.Text + "' where RowID = " + Code;
                    Queries[Array_Index++] = "Delete From Sample_Cycle_Time_Entry_Details where MAsterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New == true)
                    {
                        Queries[Array_Index++] = "Insert Into Sample_Cycle_Time_Entry_Details (MasterID, OrderColorID, Actual_Cycle_Pair, New_Cycle_Pair) Values (@@IDENTITY, '" + Grid["OrderColorID", i].Value.ToString() + "', '" + Grid["Actual_Cycle_Pair", i].Value.ToString() + "', '" + Grid["New_Cycle_Pair", i].Value.ToString() + "')";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert Into Sample_Cycle_Time_Entry_Details (MasterID, OrderColorID, Actual_Cycle_Pair, New_Cycle_Pair) Values (" + Code + ", '" + Grid["OrderColorID", i].Value.ToString() + "', '" + Grid["Actual_Cycle_Pair", i].Value.ToString() + "', '" + Grid["New_Cycle_Pair", i].Value.ToString() + "')";
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Cycle Time Entry - Delete", "Select EntryNo, EffectFrom, EffectFromShift, Week, Year, Remarks, Rowid from Sample_Cycle_Time_Entry_Master Order By EntryNo Desc", String.Empty, 80, 90, 70);
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
                    MyBase.Run("Delete From Sample_Cycle_Time_Entry_Details where MAsterID = " + Code, "Delete From Sample_Cycle_Time_Entry_Master where RowID = " + Code);
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Cycle Time Entry - View", "Select EntryNo, EffectFrom, EffectFromShift, Week, Year, Remarks, Rowid from Sample_Cycle_Time_Entry_Master Order By EntryNo Desc", String.Empty, 80, 90, 70);
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
                    //Str = "Select 0 as Slno, S2.Order_No, S3.color Sample, S2.OrderColorID, S3.size, S3.Bom_Qty Bom, ISnull(P1.Produced,0)Produced, S2.Actula_Cycle_Pair, S2.New_Cycle_Pair, '-' T from Sample_Cycle_Time_Entry_Master S1 Left Join Sample_Cycle_Time_Entry_Details S2 On S1.Rowid = S2.MasterID Left Join Socks_Bom() S3 On S2.Order_No = S3.Order_No And S2.OrderColorID = S3.OrderColorId Left Join (Select Order_No, OrderColorID, SUM(Production)Produced from Floor_Knitting_Details Group by Order_No, OrderColorID)P1 On S2.Order_No = P1.Order_No And S2.OrderColorID = P1.OrderColorID Where 1 = 2";
                    Str = "Select 0 as Slno, S3.color Sample, S2.OrderColorID, S2.Actual_Cycle_Pair, S2.New_Cycle_Pair, '-' T from Sample_Cycle_Time_Entry_Master S1 Left Join Sample_Cycle_Time_Entry_Details S2 On S1.Rowid = S2.MasterID Left Join Socks_Bom() S3 On S2.OrderColorID = S3.OrderColorId Where 1 = 2  ";
                }
                else 
                {
                    Str = "Select Distinct 0 as Slno, S3.color Sample, S2.OrderColorID, S2.Actual_Cycle_Pair, S2.New_Cycle_Pair, '-' T from Sample_Cycle_Time_Entry_Master S1 Left Join Sample_Cycle_Time_Entry_Details S2 On S1.Rowid = S2.MasterID Left Join Socks_Bom() S3 On S2.OrderColorID = S3.OrderColorId Where MasterID = " + Code + "";
                }

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "OrderColorID", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Sample", "New_Cycle_Pair");
                MyBase.Grid_Width(ref Grid, 50, 150, 150, 150);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                Grid.RowHeadersWidth = 10;

                Grid.Columns["Actual_Cycle_Pair"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["New_Cycle_Pair"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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

        private void FrmSampleCycleTimeChangeEntry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtShift")
                    {
                        Grid.CurrentCell = Grid["Sample", 0];
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

        private void FrmSampleCycleTimeChangeEntry_KeyPress(object sender, KeyPressEventArgs e)
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Sample"].Index)
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["New_Cycle_Pair"].Index )
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

        void Total_Prod_Qty()
        {
            try
            {
                TxtTotal.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Count(ref Grid,"Sample", "Actual_Cycle_Pair")));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Sample_Selection()
        {
            try
            {
                String Str;
                Str = "Select Distinct Color Sample, OrderColorID, Cycle_Pair Actual_Cycle_Pair from Socks_Bom() Order By Sample";
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Sample", Str, String.Empty, 150, 100);
                if (Dr != null)
                {
                    Grid["Sample", Grid.CurrentCell.RowIndex].Value = Dr["Sample"].ToString();
                    Txt.Text = Dr["Sample"].ToString();
                    Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Dr["OrderColorID"].ToString();
                    Grid["Actual_Cycle_Pair", Grid.CurrentCell.RowIndex].Value = Dr["Actual_Cycle_Pair"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Sample"].Index)
                    {
                        Sample_Selection();
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["New_Cycle_Pair"].Index)
                    {
                        if (Grid["New_Cycle_Pair", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["New_Cycle_Pair", Grid.CurrentCell.RowIndex].Value = "00:00";
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

        private void Grid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Sample"].Index)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        if (Grid["Sample", Grid.CurrentCell.RowIndex].Value == null || Grid["Sample", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Sample_Selection();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtShift_Enter(object sender, EventArgs e)
        {
            try
            {
                Shift_Selection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DtpDate1_ValueChanged(object sender, EventArgs e)
        {
            DataTable Tdt = new DataTable();

            String Str;
            Str = "Select year('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "')YEar, DATEPART(WEEK,'" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "') Week";

            MyBase.Load_Data(Str, ref Tdt);
            TxtYear.Text = Convert.ToString(Tdt.Rows[0][0]);
            TxtWeek.Text = Convert.ToString(Tdt.Rows[0][1]);
        }
    }
}