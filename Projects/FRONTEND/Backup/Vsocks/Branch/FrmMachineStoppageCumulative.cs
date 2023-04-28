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
    public partial class FrmMachineStoppageCumulative : Form, Entry
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

        String Str = String.Empty;

        public FrmMachineStoppageCumulative()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void FrmMachineStoppageCumilative_Load(object sender, EventArgs e)
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
                TxtUnit.Focus();
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
                TxtUnit.Text = Dr["Unit"].ToString();
                TxtUnit.Tag = Dr["Unit_Code"].ToString();
                DtpDate1.Value = Convert.ToDateTime(Dr["ST_Date"]);
                TxtShift.Text = Dr["ST_Shift"].ToString();
                DtpStopTime1.Value = Convert.ToDateTime(Dr["ST_StopTime"]);
                DtpStartTIme1.Value = Convert.ToDateTime(Dr["ST_StartTime"]);
                TxtDuration.Text = Dr["ST_Duration"].ToString();
                TxtReason.Text = Dr["Reason"].ToString();
                TxtReason.Tag = Dr["Reason_No"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();
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
                Str = " Select S1.EntryNo, U1.Floor Unit, S2.Reason_Name Reason, S1.ST_Date, S1.ST_Shift, S1.ST_StopTime, S1.ST_StartTime, S1.ST_Duration, ";
                Str = Str + " S1.ST_Reason Reason_No, S1.Remarks, S1.RowID, S1.Unit_Code From Socks_Machine_Stoppage_Cumulative S1 Left Join Socks_Machine_Stoppage_Reason S2 On S1.ST_Reason = S2.RowId ";
                Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ST_Shift = S3.shiftcode2 And S3.Compcode = 2 And S3.Mode = 1 And S3.Shiftcode in (15, 16, 17) Left Join Unit U1 On S1.Unit_Code = U1.Floor_Id Where S1.ST_Date >= Dateadd (D, " + MyParent.User_Datelock + ", Cast(GetDate() as Date)) Order by S1.EntryNo Desc ";
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Stoppage - Edit", Str, String.Empty, 80, 80, 90, 80, 80);
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

                if (TxtUnit.Text.ToString() == String.Empty || TxtShift.Text.ToString() == String.Empty || TxtReason.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    MyParent.Save_Error = true;
                    DtpDate1.Focus();
                    return;
                }

                TxtNo.Text = MyBase.MaxOnlyComp("Socks_Machine_Stoppage_Cumulative", "EntryNo", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();

                if (MyParent._New)
                {
                    Str = "Insert into Socks_Machine_Stoppage_Cumulative (EntryNo, ST_Date, ST_Shift, ST_StopTime, ST_StartTime, ST_Duration, ST_Reason, Unit_Code, Company_Code, Entry_Time, Entry_System, Remarks) Values (" + TxtNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', '" + TxtShift.Text.ToString() + "', Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime) + Cast('" + DtpStopTime1.Text + "' As Time), (Case When " + TxtShift.Text + " = 3 And Cast('" + DtpStartTIme1.Text + "' As Time) < Cast('" + DtpStopTime1.Text + "' As Time) Then DateAdd(DD,1,Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime)) Else Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime) End) + Cast('" + DtpStartTIme1.Text + "' As Time), '" + TxtDuration.Text + "', " + TxtReason.Tag + ", " + TxtUnit.Tag + ", " + MyParent.CompCode + ", GetDate(), Host_Name(), '" + TxtRemarks.Text.ToString() + "') ";
                }
                else
                {
                    Str = "Update Socks_Machine_Stoppage_Cumulative Set ST_Shift = '" + TxtShift.Text.ToString() + "', ST_StopTime = Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime) + Cast('" + DtpStopTime1.Text + "' As Time), ST_StartTime = (Case When " + TxtShift.Text + " = 3 And Cast('" + DtpStartTIme1.Text + "' As Time) < Cast('" + DtpStopTime1.Text + "' As Time) Then DateAdd(DD,1,Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime)) Else Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime) End) + Cast('" + DtpStartTIme1.Text + "' As Time), ST_Duration = '" + TxtDuration.Text + "', ST_Reason = " + TxtReason.Tag + ", Unit_Code = " + TxtUnit.Tag + ", Remarks = '" + TxtRemarks.Text.ToString() + "' Where RowID = " + Code + "";
                }

                MyBase.Execute(Str); 
                
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
                Buffer_Update = false;
                Str = " Select S1.EntryNo, U1.Floor Unit, S2.Reason_Name Reason, S1.ST_Date, S1.ST_Shift, S1.ST_StopTime, S1.ST_StartTime, S1.ST_Duration, ";
                Str = Str + " S1.ST_Reason Reason_No, S1.Remarks, S1.RowID, S1.Unit_Code From Socks_Machine_Stoppage_Cumulative S1 Left Join Socks_Machine_Stoppage_Reason S2 On S1.ST_Reason = S2.RowId ";
                Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ST_Shift = S3.shiftcode2 And S3.Compcode = 2 And S3.Mode = 1 And S3.Shiftcode in (15, 16, 17) Left Join Unit U1 On S1.Unit_Code = U1.Floor_Id Order by S1.EntryNo Desc ";
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Stoppage - Delete", Str, String.Empty, 80, 80, 90, 80, 80);
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
                    MyBase.Run("Delete From Socks_Machine_Stoppage_Cumulative where RowID = " + Code);
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
                Str = " Select S1.EntryNo, U1.Floor Unit, S2.Reason_Name Reason, S1.ST_Date, S1.ST_Shift, S1.ST_StopTime, S1.ST_StartTime, S1.ST_Duration, ";
                Str = Str + " S1.ST_Reason Reason_No, S1.Remarks, S1.RowID, S1.Unit_Code From Socks_Machine_Stoppage_Cumulative S1 Left Join Socks_Machine_Stoppage_Reason S2 On S1.ST_Reason = S2.RowId ";
                Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S3 On S1.ST_Shift = S3.shiftcode2 And S3.Compcode = 2 And S3.Mode = 1 And S3.Shiftcode in (15, 16, 17) Left Join Unit U1 On S1.Unit_Code = U1.Floor_Id Order by S1.EntryNo Desc ";
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Stoppage - View", Str, String.Empty, 80, 80, 90, 80, 80);
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

        void Shift_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Shift", "Select ShiftCode2 Shift, StartTime, EndTime, ShiftCode From Socks_Shift() Where Shiftcode in (15, 16, 17)", String.Empty, 80, 80, 80);
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

        void Reason_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Reason", "Select Reason_Name Reason, RowId Reason_No From Socks_Machine_Stoppage_Reason Where Mode = 'C' ", String.Empty, 150, 80);
                if (Dr != null)
                {
                    TxtReason.Text = Dr["Reason"].ToString();
                    TxtReason.Tag = Dr["Reason_No"].ToString();
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
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Unit", "Select Floor Unit, Floor_Id Unit_Code From Unit", String.Empty, 150, 80);
                if (Dr != null)
                {
                    TxtUnit.Text = Dr["Unit"].ToString();
                    TxtUnit.Tag = Dr["Unit_Code"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmMachineStoppageCumulative_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    //if (this.ActiveControl.Name == "TxtShift")
                    //{
                    //    DataTable Dt = new DataTable();
                    //    String Str = "Select * from Socks_Machine_Stoppage_Cumulative Where St_date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' and St_shift = " + TxtShift.Text.ToString() + " and Cast(Unit_Code As Varchar(2)) = " + TxtUnit.Tag + "";
                    //    MyBase.Load_Data(Str, ref Dt);
                    //    if (Dt.Rows.Count > 0)
                    //    {
                    //        MessageBox.Show("Already Saved For This Date and Shift..!", "Gainup");
                    //        MyBase.Clear(this);
                    //        MyParent.Load_NewEntry();
                    //    }
                    //    else
                    //    {
                    //        DtpStopTime1.Focus(); 
                    //        return;
                    //    }
                    //}
                     if (this.ActiveControl.Name == "TxtRemarks")
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
                    else if (this.ActiveControl.Name == "TxtReason")
                    {
                        Reason_Selection();
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

        private void FrmMachineStoppageCumulative_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtRemarks")
                    {
                    }
                    else if (this.ActiveControl.Name == "TxtDuration")
                    {
                        MyBase.Valid_Number(Txt, e); 
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

        private void DtpStartTIme1_Leave(object sender, EventArgs e)
        {
            Str = " Select DateDiff(MI, Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime) + Cast('" + DtpStopTime1.Text + "' As Time),  (Case When " + TxtShift.Text + " = 3 And Cast('" + DtpStartTIme1.Text + "' As Time) < Cast('" + DtpStopTime1.Text + "' As Time) Then DateAdd(DD, 1, Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime)) Else Cast('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' As DateTime) End) + Cast('" + DtpStartTIme1.Text + "' As Time))Diff";
            MyBase.Load_Data(Str, ref Dt);
            if (Dt.Rows.Count > 0)
            {
                TxtDuration.Text = Dt.Rows[0]["Diff"].ToString();
            }
        }

        private void TxtShift_Leave(object sender, EventArgs e)
        {
            if (Convert.ToInt16(TxtShift.Text.ToString()) == 1)
            {
                //DtpStopTime1.MinimumDateTime = Convert.ToDateTime("01-Jan-2001 06:00:00 Am");
                //DtpStartTIme1.MaxDate = Convert.ToDateTime("01-Jan-2001 02:00:00 Pm");
            }
        }
    }
}
