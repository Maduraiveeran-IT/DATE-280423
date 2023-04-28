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
    public partial class FrmStoppageEntryALL : Form, Entry
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

        public FrmStoppageEntryALL()
        {
            InitializeComponent();
        }

        private void FrmStoppageEntryALL_Load(object sender, EventArgs e)
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Stoppage - Edit", "select S1.EntryNo, S1.ST_Date Date, S1.ST_Shift Shift, S1.Remarks, S1.Unit_Code Unit, S1.RowId, S2.shiftcode From Socks_Machine_Stoppage_Master_BreakDown S1 Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S2 On S1.ST_Shift = S2.shiftcode2 And S2.compcode = 2 And S2.shiftcode in (15, 16, 17) Where S1.ST_Date >= Dateadd (D, " + MyParent.User_Datelock + ", Cast(GetDate() as Date)) Order by S1.EntryNo", String.Empty, 80, 80, 90, 80, 80);
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
                    if (Grid["Reason", i].Value == DBNull.Value || Grid["Reason", i].Value.ToString() == String.Empty)
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
                Queries = new String[(Dt.Rows.Count) + 5];

                TxtNo.Text = MyBase.MaxOnlyComp("Socks_Machine_Stoppage_Master_BreakDown", "EntryNo", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_Machine_Stoppage_Master_BreakDown (EntryNo, ST_Date, ST_Shift, Remarks, Company_Code, Unit_Code) Values (" + TxtNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', '" + TxtShift.Text.ToString() + "', '" + TxtRemarks.Text.ToString() + "', " + MyParent.CompCode + ", (Case When " + MyParent.UserCode + " in (7, 8) Then 1 When " + MyParent.UserCode + " In (40, 23) Then 2 End)) ; Select Scope_Identity()";
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_Machine_Stoppage_Master_BreakDown Set Remarks = '" + TxtRemarks.Text + "' where RowID = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_Machine_Stoppage_Details_BreakDown where MasterId = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {

                    if (MyParent._New == true)
                    {
                        //Queries[Array_Index++] = "Insert Into Socks_Machine_Stoppage_Details_BreakDown (MasterID, ST_MCode, Stop_Time, Start_Time, ST_Duration, Reason, Atten_By_Emplno, Remarks) Values (@@IDENTITY, '" + Grid["Machine", i].Value.ToString() + "', '" + String.Format("{0:dd-MMM-yyyy hh:mm:ss tt}", Grid["Stop_Time", i].Value) + "', '" + String.Format("{0:dd-MMM-yyyy hh:mm:ss tt}", Grid["Start_Time", i].Value) + "', '" + Grid["Duration", i].Value.ToString() + "', " + Grid["Reason_No", i].Value + ", " + Grid["Atten_By_Emplno", i].Value + ", '" + Grid["Remarks", i].Value + "')";
                        //Queries[Array_Index++] = "Insert Into Socks_Machine_Stoppage_Details_BreakDown (MasterID, ST_MCode, Stop_Time, Start_Time, ST_Duration, Reason, Atten_By_Emplno, Remarks) Values (@@IDENTITY, '" + Grid["Machine", i].Value.ToString() + "', '" + String.Format("{0:dd-MMM-yyyy hh:mm:ss tt}", Grid["Stop_Time", i].Value) + "', '" + String.Format("{0:dd-MMM-yyyy hh:mm:ss tt}",Grid["Start_Time", i].Value) + "', '" + Grid["Duration", i].Value.ToString() + "', " + Grid["Reason_No", i].Value + ", " + Grid["Atten_By_Emplno", i].Value + ", '" + Grid["Remarks", i].Value + "')";
                        Queries[Array_Index++] = "Insert Into Socks_Machine_Stoppage_Details_BreakDown (MasterID, ST_MCode, Stop_Time, Start_Time, ST_Duration, Reason, Atten_By_Emplno, Remarks) Values (@@IDENTITY, '" + Grid["Machine", i].Value.ToString() + "', Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime) + Cast('" + Grid["Stop_Time", i].Value.ToString() + "' As Time) , (Case when Cast('" + Grid["Start_Time", i].Value.ToString() + "' as time) < Cast('" + Grid["Stop_Time", i].Value.ToString() + "' as time) And " + TxtShift.Text + " = 3 then DateAdd(DD, 1, Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime)) Else Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime)End) + Cast('" + Grid["Start_Time", i].Value.ToString() + "' As Time) , '" + Grid["Duration", i].Value.ToString() + "', " + Grid["Reason_No", i].Value + ", " + Grid["Atten_By_Emplno", i].Value + ", '" + Grid["Remarks", i].Value + "')";
                    }
                    else
                    {
                        //Queries[Array_Index++] = "Insert Into Socks_Machine_Stoppage_Details_BreakDown (MasterID, ST_MCode, Stop_Time, Start_Time, ST_Duration, Reason, Atten_By_Emplno, Remarks) Values (" + Code + ",'" + Grid["Machine", i].Value.ToString() + "', '" + String.Format("{0:dd-MMM-yyyy hh:mm:ss tt}", Grid["Stop_Time", i].Value) + "', '" + String.Format("{0:dd-MMM-yyyy hh:mm:ss tt}",Grid["Start_Time", i].Value )+ "', '" + Grid["Duration", i].Value.ToString() + "', " + Grid["Reason_No", i].Value + ",  " + Grid["Atten_By_Emplno", i].Value + ", '" + Grid["Remarks", i].Value + "')";
                        Queries[Array_Index++] = "Insert Into Socks_Machine_Stoppage_Details_BreakDown (MasterID, ST_MCode, Stop_Time, Start_Time, ST_Duration, Reason, Atten_By_Emplno, Remarks) Values (" + Code + ",'" + Grid["Machine", i].Value.ToString() + "', Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime) + Cast('" + Grid["Stop_Time", i].Value.ToString() + "' As Time) , (Case when Cast('" + Grid["Start_Time", i].Value.ToString() + "' as time) < Cast('" + Grid["Stop_Time", i].Value.ToString() + "' as time) And " + TxtShift.Text + " = 3 then DateAdd(DD, 1, Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime)) Else Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime)End) + Cast('" + Grid["Start_Time", i].Value.ToString() + "' As Time) , '" + Grid["Duration", i].Value.ToString() + "', " + Grid["Reason_No", i].Value + ",  " + Grid["Atten_By_Emplno", i].Value + ", '" + Grid["Remarks", i].Value + "')";
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Stoppage - Delete", "select S1.EntryNo, S1.ST_Date Date, S1.ST_Shift Shift, S1.Unit_Code Unit, S1.Remarks, S1.RowId from Socks_Machine_Stoppage_Master_BreakDown S1 Order by S1.EntryNo", String.Empty, 80, 80, 90, 80, 80);
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
                    MyBase.Run("Delete From Socks_Machine_Stoppage_Details_BreakDown where MasterID = " + Code, "Delete From Socks_Machine_Stoppage_Master_BreakDown where RowID = " + Code);
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Stoppage - View", "select S1.EntryNo, S1.ST_Date Date, S1.ST_Shift Shift, S1.Remarks, S1.Unit_Code Unit, S1.RowId from Socks_Machine_Stoppage_Master_BreakDown S1 Order by S1.EntryNo", String.Empty, 80, 80, 90, 80, 80);
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
                    Str = "Select 0 as Slno, S1.ST_MCode Machine, Cast(S1.Stop_Time As Varchar(20)) Stop_Time, Cast(S1.Start_Time  As Varchar(20))Start_Time, S1.ST_Duration Duration, ";
                    Str = Str + " S2.Reason_Name Reason, S1.Reason Reason_No, E1.Name Atten_By, S1.Atten_By_Emplno, S1.Remarks, '-' T From Socks_Machine_Stoppage_Details_BreakDown S1";
                    Str = Str + " Left Join Socks_Machine_Stoppage_Reason S2 On S1.Reason = S2.RowId Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on S1.Atten_By_Emplno = E1.Emplno ";
                    Str = Str + " Where 1 = 2 Order by S1.RowID";
                }
                else
                {
                    if (MyParent._New)
                    {
                        Str = "Select 0 as Slno, S1.ST_MCode Machine, S1.Stop_Time Stop_Time, S1.Start_Time Start_Time, S1.ST_Duration Duration, ";
                        Str = Str + " S2.Reason_Name Reason, S1.Reason Reason_No, E1.Name Atten_By, S1.Atten_By_Emplno, S1.Remarks, '-' T From Socks_Machine_Stoppage_Details_BreakDown S1";
                        Str = Str + " Left Join Socks_Machine_Stoppage_Reason S2 On S1.Reason = S2.RowId Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on S1.Atten_By_Emplno = E1.Emplno ";
                        Str = Str + " Where 1 = 2 Order by S1.RowID";
                    }
                    else
                    {
                        Str = "Select 0 as Slno, S1.ST_MCode Machine, Substring(Cast(Cast(S1.Stop_Time As Time)As Varchar(20)),1,5)Stop_Time, Substring(Cast(Cast(S1.Start_Time As Time)As Varchar(20)),1,5) Start_Time, S1.ST_Duration Duration, ";
                        Str = Str + " S2.Reason_Name Reason, S1.Reason Reason_No, E1.Name Atten_By, S1.Atten_By_Emplno, S1.Remarks, '-' T From Socks_Machine_Stoppage_Details_BreakDown S1";
                        Str = Str + " Left Join Socks_Machine_Stoppage_Reason S2 On S1.Reason = S2.RowId Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on S1.Atten_By_Emplno = E1.Emplno ";
                        Str = Str + " Where S1.MasterID = " + Code + " Order by S1.RowID ";
                    }
                }

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Reason_No", "Atten_By_Emplno", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Machine", "Stop_Time", "Start_Time", "Duration", "Reason", "Atten_By", "Remarks");
                MyBase.Grid_Width(ref Grid, 50, 100, 100, 100, 100, 150, 150, 160);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                Grid.RowHeadersWidth = 10;

                //Grid.Columns["Stop_Time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //Grid.Columns["Stop_Time"].DefaultCellStyle.Format = "hh:mm";

               //Grid.Columns["Start_Time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
               //Grid.Columns["Start_Time"].DefaultCellStyle.Format = "hh:mm";

                Grid.Columns["Stop_Time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Stop_Time"].DefaultCellStyle.Format = "00:00";

                Grid.Columns["Start_Time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Start_Time"].DefaultCellStyle.Format = "00:00";


                //Grid.Columns["Duration"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //Grid.Columns["Duration"].DefaultCellStyle.Format = "00.00";

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

        void Employee_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Employee", "Select Name, Tno, Emplno from VAAHINI_ERP_GAINUP.Dbo.Employeemas Where COMPCODE = 2 And Catcode Not in (1, 3) And Tno Not Like '%Z' Order By Name ", String.Empty, 150, 100);
                if (Dr != null)
                {
                    Grid["Atten_By", Grid.CurrentCell.RowIndex].Value = Dr["Name"].ToString();
                    Txt.Text = Dr["Name"].ToString();
                    Grid["Atten_By_Emplno", Grid.CurrentCell.RowIndex].Value = Dr["Emplno"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Machine_Selection()
        {
            try
            {
                if (MyParent.UserCode == 7 || MyParent.UserCode == 8)
                {
                    Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine, Machine_ID from Knitting_Mc_NO_UnitWise(1) ", String.Empty, 150, 100);
                }
                else if (MyParent.UserCode == 40 || MyParent.UserCode == 23)
                {
                    Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine, Machine_ID from Knitting_Mc_NO_UnitWise(2) ", String.Empty, 150, 100);
                }
                else
                {
                    Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine, Machine_ID from Knitting_Mc_NO() ", String.Empty, 150, 100);
                }
                if (Dr != null)
                {
                    Grid["Machine", Grid.CurrentCell.RowIndex].Value = Dr["Machine"].ToString();
                    Txt.Text = Dr["Machine"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmStoppageEntryALL_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
               if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtShift")
                    {
                        DataTable Dt = new DataTable();
                        String Str = "Select * from Socks_Machine_Stoppage_Master_BreakDown Where St_date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' and St_shift= " + TxtShift.Text.ToString() + " and Cast(Unit_Code As Varchar(2)) in (Case When " + MyParent.UserCode + " = 7 Then '1' When " + MyParent.UserCode + " = 40 Then '2' Else '1, 2' End)";
                        MyBase.Load_Data(Str, ref Dt);
                        if (Dt.Rows.Count > 0)
                        {
                            MessageBox.Show("Already Saved For This Date and Shift..!", "Gainup");
                            MyBase.Clear(this);
                            MyParent.Load_NewEntry();
                        }
                        else
                        {
                                Grid_Data(false);
                                Grid.CurrentCell = Grid["Machine", 0];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
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

        private void FrmStoppageEntryALL_KeyPress(object sender, KeyPressEventArgs e)
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Reason"].Index)
                {
                    if (Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() != null && Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() != null && Convert.ToDouble(Grid["Duration", Grid.CurrentCell.RowIndex].Value.ToString()) == 0)
                    {
                        MessageBox.Show("Invalid Duration...!Gainup");
                        Grid.CurrentCell = Grid["Stop_Time", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
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
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Atten_By"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        Grid["Atten_By", Grid.CurrentCell.RowIndex].Value = Grid["Atten_By", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Atten_By_Emplno", Grid.CurrentCell.RowIndex].Value = Grid["Atten_By_Emplno", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Txt.Text = Grid["Atten_By", Grid.CurrentCell.RowIndex - 1].Value.ToString();
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Reason"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Atten_By_Emplno"].Index)
                {
                    e.Handled = true;
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Stop_Time"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Start_Time"].Index)
                {
                    MyBase.Valid_Semicolon_Decimal(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Duration"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
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
                TxtTotal.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Count(ref Grid, "slno")));
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
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Atten_By"].Index)
                    {
                        Employee_Selection();
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                    {
                        Machine_Selection();
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
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Stop_Time"].Index)
                    {
                        if (Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() != null)
                        {
                            // Replace If Time Contain : Then .
                            Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value = Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString().Replace(":", ".");
                            
                            //Check Reasonable Time limit For Corresponding Shift Choose 
                            //if (Convert.ToInt16(TxtShift.Tag.ToString()) == 15)
                            if(Convert.ToInt16(TxtShift.Text.ToString()) == 1)
                            {
                                 if ((Convert.ToDouble(Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString()) >= 06.00 && Convert.ToDouble(Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString()) <= 12.59) || (Convert.ToDouble(Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString()) >= 00.00 && Convert.ToDouble(Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString()) <= 02.00))
                                 {

                                 }
                                 else 
                                 {
                                    MessageBox.Show("Invalid '" + Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "' Stop Time For Shift " + TxtShift.Text.ToString() + " ....!Gainup");
                                    Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value = "";
                                    Grid["Duration", Grid.CurrentCell.RowIndex].Value = 0;
                                    Grid.CurrentCell = Grid["Stop_Time", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    return;
                                }
                            }
                            else if (Convert.ToInt16(TxtShift.Text.ToString()) == 2)
                            {
                                if (Convert.ToDouble(Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString()) >= 02.00 && Convert.ToDouble(Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString()) <= 10.00)
                                {

                                }
                                else
                                {
                                    MessageBox.Show("Invalid '" + Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "' Stop Time For Shift " + TxtShift.Text.ToString() + " ....!Gainup");
                                    Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value = "";
                                    Grid["Duration", Grid.CurrentCell.RowIndex].Value = 0;
                                    Grid.CurrentCell = Grid["Stop_Time", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    return;
                                }
                            }
                            else if (Convert.ToInt16(TxtShift.Text.ToString()) == 3)
                            {
                                if ((Convert.ToDouble(Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString()) >= 10.00 && Convert.ToDouble(Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString()) <= 12.59) || (Convert.ToDouble(Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString()) >= 00.00 && Convert.ToDouble(Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString()) <= 06.00))
                                {

                                }
                                else
                                {
                                    MessageBox.Show("Invalid '" + Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "' Stop Time For Shift " + TxtShift.Text.ToString() + " ....!Gainup");
                                    Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value = "";
                                    Grid["Duration", Grid.CurrentCell.RowIndex].Value = 0;
                                    Grid.CurrentCell = Grid["Stop_Time", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    return;
                                }
                            }
                            Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value = Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString().Replace(".",":") ;
                            DataTable D1 = new DataTable();
                            String Str1 = "Select RIGHT(REPLICATE('0', 2) + LEFT(SUBSTRING('" + Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "', 1, CharIndex(':','" + Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "')-1), 2), 2) + ':' + LEFT(SUBSTRING('" + Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "',CharIndex(':','" + Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "')+1,LEN('" + Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "')) + replicate('0',2),2)FromTime";
                            MyBase.Load_Data(Str1, ref D1);
                            if (D1.Rows.Count > 0)
                            {
                                Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value = D1.Rows[0][0].ToString();
                            }
                            else
                            {
                                Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value = "";
                                Grid["Duration", Grid.CurrentCell.RowIndex].Value = 0;
                            }
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Start_Time"].Index)
                    {
                        if (Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() != null)
                        {
                            Grid["Start_Time", Grid.CurrentCell.RowIndex].Value = Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString().Replace(":", ".");
                            if (Convert.ToInt16(TxtShift.Text.ToString()) == 1)
                            {
                                if ((Convert.ToDouble(Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString()) >= 06.00 && Convert.ToDouble(Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString()) <= 12.59) || (Convert.ToDouble(Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString()) >= 00.00 && Convert.ToDouble(Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString()) <= 02.00))
                                {

                                }
                                else
                                {
                                    MessageBox.Show("Invalid '" + Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "' Start Time For Shift " + TxtShift.Text.ToString() + " ....!Gainup");
                                    Grid["Start_Time", Grid.CurrentCell.RowIndex].Value = "";
                                    Grid["Duration", Grid.CurrentCell.RowIndex].Value = 0;
                                    Grid.CurrentCell = Grid["Start_Time", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    return;
                                }
                            }
                            else if (Convert.ToInt16(TxtShift.Text.ToString()) == 2)
                            {
                                if (Convert.ToDouble(Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString()) >= 02.00 && Convert.ToDouble(Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString()) <= 10.00)
                                {

                                }
                                else
                                {
                                    MessageBox.Show("Invalid '" + Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "' Start Time For Shift " + TxtShift.Text.ToString() + " ....!Gainup");
                                    Grid["Start_Time", Grid.CurrentCell.RowIndex].Value = "";
                                    Grid["Duration", Grid.CurrentCell.RowIndex].Value = 0;
                                    Grid.CurrentCell = Grid["Start_Time", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    return;
                                }
                            }
                            else if (Convert.ToInt16(TxtShift.Text.ToString()) == 3)
                            {
                                if ((Convert.ToDouble(Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString()) >= 10.00 && Convert.ToDouble(Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString()) <= 12.59) || (Convert.ToDouble(Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString()) >= 00.00 && Convert.ToDouble(Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString()) <= 06.00))
                                {

                                }
                                else
                                {
                                    MessageBox.Show("Invalid '" + Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "' Start Time For Shift " + TxtShift.Text.ToString() + " ....!Gainup");
                                    Grid["Start_Time", Grid.CurrentCell.RowIndex].Value = "";
                                    Grid["Duration", Grid.CurrentCell.RowIndex].Value = 0;
                                    Grid.CurrentCell = Grid["Start_Time", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    return;
                                }
                            }
                            
                            Grid["Start_Time", Grid.CurrentCell.RowIndex].Value = Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString().Replace(".", ":");
                            DataTable D1 = new DataTable();
                            String Str1 = "Select RIGHT(REPLICATE('0', 2) + LEFT(SUBSTRING('" + Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "', 1, CharIndex(':','" + Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "')-1), 2), 2) + ':' + LEFT(SUBSTRING('" + Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "',CharIndex(':','" + Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "')+1,LEN('" + Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "')) + replicate('0',2),2)ToTime";
                            MyBase.Load_Data(Str1, ref D1);
                            if (D1.Rows.Count > 0)
                            {
                                Grid["Start_Time", Grid.CurrentCell.RowIndex].Value = D1.Rows[0][0].ToString();
                            }
                            else
                            {
                                Grid["Start_Time", Grid.CurrentCell.RowIndex].Value = "";
                                Grid["Duration", Grid.CurrentCell.RowIndex].Value = 0;
                            }
                            
                            //Mins Duration Calculated Between Stop_Time And Start_Time
                            if (Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() != null && Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() != null)
                            {
                                D1 = new DataTable();
                                //Str1 = "Select Dbo.Get_Min_Diff_Btwn_Two_Time(" + TxtShift.Tag + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', '" + Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "', '" + Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "')Mins_Diff";
                                Str1 = "Select Isnull(Diff_Mins,0) Mins_Diff From Get_Min_Diff_Btwn_Two_Time1((Case When " + TxtShift.Text + " = 1 Then 15 When " + TxtShift.Text + " = 2 Then 16 When " + TxtShift.Text + " = 3 Then 17 End)  , '" + Grid["Stop_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "', '" + Grid["Start_Time", Grid.CurrentCell.RowIndex].Value.ToString() + "')";
                                MyBase.Load_Data(Str1, ref D1);
                                if (D1.Rows.Count > 0)
                                {
                                    Grid["Duration", Grid.CurrentCell.RowIndex].Value =  Convert.ToInt64(D1.Rows[0][0].ToString());
                                }
                                else
                                {
                                    Grid["Duration", Grid.CurrentCell.RowIndex].Value = 0;
                                }
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

        private void Grid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Reason"].Index)
                {
                    if ((Grid["Reason", Grid.CurrentCell.RowIndex].Value == null || Grid["Reason", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Reason", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty) && Grid["Duration", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        if (Grid.CurrentCell.RowIndex != 0)
                        {
                            Reason_Selection();
                        }
                    }
                 }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                {
                    if ((Grid["Machine", Grid.CurrentCell.RowIndex].Value == null || Grid["Machine", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty))
                    {
                        if (Grid.CurrentCell.RowIndex != 0)
                        {
                            Machine_Selection();
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Atten_By"].Index)
                {
                    if ((Grid["Atten_By", Grid.CurrentCell.RowIndex].Value == null || Grid["Atten_By", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Atten_By", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty))
                    {
                        if (Grid.CurrentCell.RowIndex != 0)
                        {
                            Employee_Selection();
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



    }
}
