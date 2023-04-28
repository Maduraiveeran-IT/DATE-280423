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
    public partial class Frm_Socks_Machine_Stoppage : Form, Entry 
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
        Int16 delcount = 0;

        public Frm_Socks_Machine_Stoppage()
        {
            InitializeComponent();
        }

        private void Frm_Socks_Machine_Stoppage_Load(object sender, EventArgs e)
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
                Grid_Data(true); 
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
                DtpDate1.Value = Convert.ToDateTime(Dr["Date"]);
                TxtShift.Text = Dr["Shift"].ToString();
                //TxtShift.Tag = Dr["ShiftCode"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();
                Grid_Data(false);
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
                Buffer_Update = false;
                DtpDate1.Enabled = false;
                TxtShift.Enabled = false;
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Stoppage - Edit", "select S1.EntryNo, S1.ST_Date Date, S1.ST_Shift Shift, S1.Remarks, S1.RowId from Socks_Machine_Stoppage_Master S1 Where S1.ST_Date >= Dateadd (D, " + MyParent.User_Datelock + ", Cast(GetDate() as Date)) Order by S1.EntryNo", String.Empty, 80, 80, 90, 80);
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
            Grid.Refresh();
            Total_Prod_Qty();
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


                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Reason", i].Value == DBNull.Value || Grid["Reason", i].Value.ToString() == String.Empty )
                    {
                        MessageBox.Show(" Invalid Reason in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Production", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }

                }
                Grid.CurrentCell = Grid[0, 0];

                if (MyParent._New)
                {
                    Dt.AcceptChanges();
                }
                Array_Index = 0;
                Queries = new String[(Dt.Rows.Count * 2) + 5];

                TxtNo.Text = MyBase.MaxOnlyComp("Socks_Machine_Stoppage_Master", "EntryNo", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_Machine_Stoppage_Master (EntryNo, ST_Date, ST_Shift, Remarks, Company_Code) Values (" + TxtNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', '" + TxtShift.Text.ToString() + "', '" + TxtRemarks.Text.ToString() +"', " + MyParent.CompCode + ") ; Select Scope_Identity()";
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_Machine_Stoppage_Master Set Remarks = '" + TxtRemarks.Text + "' where RowID = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_Machine_Stoppage_Details where MasterId = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    
                        if (MyParent._New == true)
                        {
                            Queries[Array_Index++] = "Insert Into Socks_Machine_Stoppage_Details (MasterID, ST_MCode, Sample, Stop_Time, Start_Time, ST_Duration, Reason, Remarks) Values (@@IDENTITY, '" + Grid["Machine", i].Value.ToString() + "', '" + Grid["Sample", i].Value.ToString() + "', '" + String.Format("{0:dd-MMM-yyyy hh:mm:ss tt}", Grid["Stop_Time", i].Value) + "', '" + String.Format("{0:dd-MMM-yyyy hh:mm:ss tt}", Grid["Start_Time", i].Value) + "', '" + Grid["Duration", i].Value.ToString() + "', " + Grid["Reason_No", i].Value + ", '" + Grid["Remarks", i].Value + "')";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert Into Socks_Machine_Stoppage_Details (MasterID, ST_MCode, Sample, Stop_Time, Start_Time, ST_Duration, Reason, Remarks) Values (" + Code + ",'" + Grid["Machine", i].Value.ToString() + "', '" + Grid["Sample", i].Value.ToString() + "', '" + String.Format("{0:dd-MMM-yyyy hh:mm:ss tt}", Grid["Stop_Time", i].Value) + "', '" + String.Format("{0:dd-MMM-yyyy hh:mm:ss tt}", Grid["Start_Time", i].Value) + "', '" + Grid["Duration", i].Value.ToString() + "', " + Grid["Reason_No", i].Value + ", '" + Grid["Remarks", i].Value + "')";
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

        private void TxtTiming_TextChanged()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                Set_Min_Max_Date(true);
                Buffer_Update = false;
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Stoppage - Delete", "select S1.EntryNo, S1.ST_Date Date, S1.ST_Shift Shift, S1.Remarks, S1.RowId from Socks_Machine_Stoppage_Master S1 Order by S1.EntryNo", String.Empty, 80, 80, 90, 80);
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
                    MyBase.Run("Delete From Socks_Machine_Stoppage_Details where MasterID = " + Code, "Delete From Socks_Machine_Stoppage_Master where RowID = " + Code);
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
                Buffer_Update = false;
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Stoppage - View", "select S1.EntryNo, S1.ST_Date Date, S1.ST_Shift Shift, S1.Remarks, S1.RowId from Socks_Machine_Stoppage_Master S1 Order by S1.EntryNo", String.Empty, 80, 80, 90, 80);
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

        void Grid_Data(Boolean Buffer)
        {
            String Str = String.Empty;
            try
            {
                if (Buffer)
                {
                    Str = "Select 0 as Slno, S2.ST_MCode Machine, S2.Sample Sample, S2.Stop_Time Stop_Time, S2.Stop_Time Stop_Time1, S2.Start_Time Start_Time, S2.Start_Time Start_Time1, S2.ST_Duration Duration,  Cast('' as varchar) Duration_Min, cast ('' as varchar) Reason, 0 as Reason_No, cast ('' as Varchar) Remarks, '-' T From Socks_Machine_Stoppage_Details S2 Where 1= 2 Order by S2.ST_MCode";
                }
                else
                {
                    if (MyParent._New)
                    {
                        //Str = "Select 0 as Slno, S2.ST_MCode Machine, S2.Sample Sample, S2.Stop_Time, S2.Start_Time, S2.ST_Duration Duration, cast ('' as varchar) Reason, 0 as Reason_No, cast ('' as Varchar) Remarks, '-' T From ACCOUNTS.DBO.MIS_Socks_Stoppage() S2 Where S2.ST_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' and S2.St_Shift = " + TxtShift.Text.ToString() + " ";
                        Str = "Select 0 as Slno, S2.ST_MCode Machine, S2.Sample Sample, S2.Stop_Time Stop_Time, S2.Stop_Time Stop_Time1, S2.Start_Time Start_Time, S2.Start_Time Start_Time1, S2.ST_Duration Duration, S2.ST_Duration_Mini Duration_Min, cast ('' as varchar) Reason, 0 as Reason_No, cast ('' as Varchar) Remarks, '-' T From ACCOUNTS.DBO.MIS_Socks_Stoppage() S2 Where S2.ST_Reason like ('%Unknown%') and S2.ST_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' and S2.St_Shift = " + TxtShift.Text.ToString() + " Order by S2.ST_MCode";
                    }
                    else
                    {
                        Str = "select 0 as Slno, S2.ST_MCode Machine, S2.Sample Sample, S2.Stop_Time Stop_Time, S2.Stop_Time Stop_Time1, S2.Start_Time Start_Time, S2.Start_Time Start_Time1, S2.ST_Duration Duration, Cast(isnull(S2.St_Duration,0) / 60 as varchar) + ':' + Cast(isnull(S2.St_Duration,0) % 60 as varchar) Duration_Min, S3.Reason_Name Reason, S2.Reason Reason_No, S2.Remarks, '-' T From Socks_Machine_Stoppage_Master S1 Left Join Socks_Machine_Stoppage_Details S2 On S1.RowId = S2.MasterID Left Join Socks_Machine_Stoppage_Reason S3 On S2.Reason = S3.RowId Where S2.MasterID = " + Code + " Order by S2.MasterID, S2.RowID  ";
                    }
                }

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Reason_No", "Stop_Time", "Start_Time", "Duration", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Reason", "Remarks");
                MyBase.Grid_Width(ref Grid, 50, 100, 130, 120, 120, 100, 120, 160);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                Grid.RowHeadersWidth = 10;

                Grid.Columns["Stop_Time1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Stop_Time1"].DefaultCellStyle.Format = "hh:mm tt";

                Grid.Columns["Start_Time1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Start_Time1"].DefaultCellStyle.Format = "hh:mm tt"; 

                //Grid.Columns["Rejected"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //Grid.Columns["Rejected"].DefaultCellStyle.Format = "0";

                //Grid.Columns["BOM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //Grid.Columns["BOM"].DefaultCellStyle.Format = "0";
    
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

        void Data_Selection()
        {
            try
            {
                Grid_Data(false); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Reason_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Reason", "Select Reason_Name Reason, RowId Reason_No From Socks_Machine_Stoppage_Reason", String.Empty, 150, 80);
                if (Dr != null)
                {
                    Grid["Reason", Grid.CurrentCell.RowIndex].Value = Dr["Reason"].ToString();
                    Txt.Text = Dr["Reason"].ToString();
                    Grid["Reason_No", Grid.CurrentCell.RowIndex].Value = Dr["Reason_No"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void Frm_Socks_Machine_Stoppage_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtShift")
                    {
                        DataTable Dt = new DataTable();
                        String Str = "Select * from Socks_Machine_Stoppage_Master Where St_date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' and St_shift= " + TxtShift.Text.ToString() + "";
                        MyBase.Load_Data(Str, ref Dt);   
                        if (Dt.Rows.Count > 0)
                        {
                            MessageBox.Show("Already Saved For This Date and Shift..!", "Gainup");
                            MyBase.Clear(this);
                            MyParent.Load_NewEntry();
                        }
                        else
                        {
                            DataTable Dt1 = new DataTable();
                            String Str1 = "select * from Accounts.dbo.MIS_Socks_Stoppage() where ST_Date ='" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' and St_Shift = " + TxtShift.Text.ToString() + "  and ST_Reason like 'unknown'";
                            MyBase.Load_Data(Str1, ref Dt1);
                            if (Dt1.Rows.Count == 0)
                            {
                                MessageBox.Show("No Details For This Date and Shift...!", "Gainup");
                                MyBase.Clear(this);
                                MyParent.Load_NewEntry();
                            }
                            else
                            {
                                Grid_Data(false);
                                Grid.CurrentCell = Grid["Reason", 0];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                        }
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

        private void Frm_Socks_Machine_Stoppage_KeyPress(object sender, KeyPressEventArgs e)
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
                /* if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                {
                    Machine_Selection();
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Needle"].Index)
                {
                    Needle_Selection();
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                {
                    OrderNo_Selection();
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index)
                {
                    Operator_Selection();
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Technician"].Index)
                {
                    Tech_Selection();
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Supervisor"].Index)
                {
                    Supervisor_Selection();
                } */
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Txt_LostFocus(object sender, EventArgs e)
        {
            //try
            //{
            //    if (MyParent._New != true)
            //    {
            //        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Production"].Index)
            //        {
            //            Grid.CurrentCell = Grid["Rejected", Grid.CurrentCell.RowIndex];
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        void Txt_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Reason"].Index)
                {
                    Total_Prod_Qty();
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Reason"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        Grid["Reason_No", Grid.CurrentCell.RowIndex].Value = Grid["Reason_No", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Reason", Grid.CurrentCell.RowIndex].Value = Grid["Reason", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Txt.Text = Grid["Reason", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                    }
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Reason"].Index)
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
                Grid.Refresh();
                TxtTotal.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "slno")));
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
                        Reason_Selection();
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
                    {
                        if (Grid["Remarks", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Grid["Remarks", Grid.CurrentCell.RowIndex].Value.ToString() == null)
                        {
                            Grid["Remarks", Grid.CurrentCell.RowIndex].Value = "-";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GBMain_Enter(object sender, EventArgs e)
        {

        }

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                delcount = 1;
                MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentCell.RowIndex);
                Total_Prod_Qty();
                MyBase.Row_Number(ref Grid);
                delcount = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_CurrentCellChanged(object sender, EventArgs e)
        {
            //try
            //{
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void Grid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Reason"].Index)
                {
                    //if (Grid["Reason", Grid.CurrentCell.RowIndex].Value == null || Grid["Reason", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Reason", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    if ((Grid["Reason", Grid.CurrentCell.RowIndex].Value == null || Grid["Reason", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Reason", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty) && Grid["Duration", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        if (Grid.CurrentCell.RowIndex != 0 )
                        //if (Grid["Reason", Grid.CurrentCell.RowIndex] != 0)
                        {
                            Reason_Selection();
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

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DtpDate1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void TxtTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtShift_TextChanged(object sender, EventArgs e)
        {

        }
    }
}