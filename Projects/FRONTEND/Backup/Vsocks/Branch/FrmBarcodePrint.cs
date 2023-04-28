using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.IO; 
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmBarcodePrint : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        Int64 Code = 0;
        DataRow Dr;
        MDIMain MyParent;
        TextBox Txt = null;
        String Buffer_Table = String.Empty;
        String Str;

        Int16 M = 0; 
        DataTable Tdt1 = new DataTable();

        public FrmBarcodePrint()
        {
            InitializeComponent();
        }

        private void FrmBarcodePrint_Load(object sender, EventArgs e)
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
                TxtNo.Text = Dr["EntryNo"].ToString();
                DtpDate1.Value = Convert.ToDateTime(Dr["EntryDate"]);
                TxtShift.Text = Dr["Shift"].ToString();
                TxtShift.Tag = Dr["ShiftCode"].ToString();
                TxtUnit.Text = Dr["Unit"].ToString();
                TxtUnit.Tag = Dr["Unit_Code"].ToString();

                if(Tdt1.Rows.Count > 0)
                {
                    if (Convert.ToInt16(Tdt1.Rows[0][0].ToString())  == 0 )
                    {
                        TxtOperator.Text = Dr["Name"].ToString();
                        TxtOperator.Tag = Dr["Emplno_Operator"].ToString();
                    }
                }
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

                Dt.AcceptChanges();
                Array_Index = 0;
                Queries = new String[(Dt.Rows.Count) + 5];

                TxtNo.Text = MyBase.MaxOnlyWithoutComp("Socks_Bundle_Details", "EntryNo", String.Empty, String.Empty, 0).ToString();

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New == true)
                    {
                        Queries[Array_Index++] = "Insert Into Socks_Bundle_Details (EntryNo, Floor_Knitting_Details_RowID, BundleNO, Slno) Values (" + TxtNo.Text + ", " + Grid["RowID", i].Value + ", '" + Grid["BundleNO", i].Value.ToString() + "', " + Grid["Slno", i].Value + ")";
                    }
                    else
                    {
                        //Queries[Array_Index++] = "Insert Into Socks_Bundle_Details (EntryNo, Floor_Knitting_Details_RowID, BundleNO, Slno) Values (" + TxtNo.Text + ", " + Grid["RowID", i].Value + ", '" + Grid["BundleNO", i].Value.ToString() + "', " + Grid["Slno", i].Value + ")";
                    }
                }

                MyBase.Run(Queries);  

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
                String Str;
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                //Str = " Select Distinct S1.EntryNO, F2.EntryDate, S2.Shiftcode2 Shift, (Case When F1.MachineID Not Like 'I%' Then 'Floor I' Else 'Floor II' End)Unit, F2.Shiftcode, (Case When F1.MachineID Not Like 'I%' Then 1 Else 2 End)Unit_Code  from Socks_Bundle_Details S1 ";
                //Str = Str + " Left Join Floor_Knitting_Details F1 on S1.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 on F1.MasterID = F2.RowID Left Join Socks_Shift() S2 on F2.ShiftCode = S2.Shiftcode";

                //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Barcode - Delete", Str, String.Empty, 80, 90, 70);

                Tdt1 = new DataTable();
                MyBase.Load_Data("Select (Case When Cast(GETDATE() As Date) <= Cast('15-feb-2016' As Date) Then 1 Else 0 End)Con", ref Tdt1);
                if (Tdt1.Rows.Count > 0)
                {
                    if (Convert.ToInt16(Tdt1.Rows[0][0].ToString()) == 1)
                    {
                        Str = " Select Distinct S1.EntryNO, F2.EntryDate, S2.Shiftcode2 Shift, (Case When F1.MachineID Not Like 'I%' Then 'Floor I' Else 'Floor II' End)Unit, F2.Shiftcode, (Case When F1.MachineID Not Like 'I%' Then 1 Else 2 End)Unit_Code  from Socks_Bundle_Details S1 ";
                        Str = Str + " Left Join Floor_Knitting_Details F1 on S1.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 on F1.MasterID = F2.RowID Left Join Socks_Shift() S2 on F2.ShiftCode = S2.Shiftcode";

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Barcode - View", Str, String.Empty, 80, 90, 70);
                    }
                    else
                    {
                        Str = " Select Distinct S1.EntryNO, F2.EntryDate, S2.Shiftcode2 Shift, E1.Name, K1.Unit Unit, K1.Unit_Code Unit_Code, F2.Shiftcode  ,F1.EMplno_Operator from Socks_Bundle_Details S1 ";
                        Str = Str + " Left Join Floor_Knitting_Details F1 on S1.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 on F1.MasterID = F2.RowID ";
                        Str = Str + " Left Join Socks_Shift() S2 on F2.ShiftCode = S2.Shiftcode Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno ";
                        if (MyParent.UserCode == 7)
                        {
                            Str = Str + " Right Join Knitting_Mc_NO_UnitWise(1) K1 On F1.MachineID = K1.Machine ";
                        }
                        else if (MyParent.UserCode == 40)
                        {
                            Str = Str + " Right Join Knitting_Mc_NO_UnitWise(2) K1 On F1.MachineID = K1.Machine ";
                        }
                        else
                        {
                            Str = Str + " Left Join Knitting_MC_NO_Unit() K1 On F1.MachineID = K1.Machine ";
                        }

                        Str = Str + " Where DATEDIFF(dd, F2.entrydate, Cast(Getdate() as date))<= 1 ";

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Barcode - Delete", Str, String.Empty, 80, 90, 70, 150, 70);
                    }


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
                if (Convert.ToInt64(TxtNo.Text.ToString()) > 0)
                {
                    MyBase.Run("Delete From Socks_Barcode_Details Where RowID in(Select S1.RowID from Socks_Barcode_Details S1 Left Join Socks_Bundle_Details S2 on S1.Socks_Bundle_Details_RowID = S2.RowID Left Join Floor_Knitting_Details F1 On S2.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 On F1.MasterID = F2.RowID Where S2.EntryNO = " + TxtNo.Text + " And F1.Emplno_Operator = " + TxtOperator.Tag + ")", "Delete from Socks_Bundle_Details Where EntryNo = " + TxtNo.Text + " And RowID in (Select S2.RowID From Socks_Bundle_Details S2 Left Join Floor_Knitting_Details F1 On S2.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 On F1.MasterID = F2.RowID Where S2.EntryNO = " + TxtNo.Text + " And F1.Emplno_Operator = " + TxtOperator.Tag + " )" );
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

                Tdt1 = new DataTable();
                MyBase.Load_Data("Select (Case When Cast(GETDATE() As Date) <= Cast('15-feb-2016' As Date) Then 1 Else 0 End)Con", ref Tdt1);
                if (Tdt1.Rows.Count > 0)
                {
                    if (Convert.ToInt16(Tdt1.Rows[0][0].ToString()) == 1)
                    {
                        Str = " Select Distinct S1.EntryNO, F2.EntryDate, S2.Shiftcode2 Shift, (Case When F1.MachineID Not Like 'I%' Then 'Floor I' Else 'Floor II' End)Unit, F2.Shiftcode, (Case When F1.MachineID Not Like 'I%' Then 1 Else 2 End)Unit_Code  from Socks_Bundle_Details S1 ";
                        Str = Str + " Left Join Floor_Knitting_Details F1 on S1.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 on F1.MasterID = F2.RowID Left Join Socks_Shift() S2 on F2.ShiftCode = S2.Shiftcode";

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Barcode - View", Str, String.Empty, 80, 90, 70);
                    }
                    else
                    {
                        //Str = " Select Distinct S1.EntryNO, F2.EntryDate, S2.Shiftcode2 Shift, E1.Name, Isnull(K1.Unit, K2.Unit) Unit, Isnull(K1.Unit_Code, K2.Unit_Code)Unit_Code, F2.Shiftcode ";
                        //Str = Str + " ,F1.EMplno_Operator from Socks_Bundle_Details S1 Left Join Floor_Knitting_Details F1 on S1.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 on F1.MasterID = F2.RowID ";
                        //Str = Str + " Left Join Socks_Shift() S2 on F2.ShiftCode = S2.Shiftcode Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno ";
                        //Str = Str + " Left Join Knitting_Mc_NO_UnitWise(1) K1 On F1.MachineID = K1.Machine Left Join Knitting_Mc_NO_UnitWise(2) K2 On F1.MachineID = K2.Machine ";

                        Str = " Select Distinct S1.EntryNO, F2.EntryDate, S2.Shiftcode2 Shift, E1.Name, K1.Unit Unit, K1.Unit_Code Unit_Code, F2.Shiftcode  ,F1.EMplno_Operator from Socks_Bundle_Details S1 ";
                        Str = Str + " Left Join Floor_Knitting_Details F1 on S1.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 on F1.MasterID = F2.RowID ";
                        Str = Str + " Left Join Socks_Shift() S2 on F2.ShiftCode = S2.Shiftcode Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno ";
                        if (MyParent.UserCode == 7)
                        {
                            Str = Str + " Right Join Knitting_Mc_NO_UnitWise(1) K1 On F1.MachineID = K1.Machine ";
                        }
                        else if (MyParent.UserCode == 40)
                        {
                            Str = Str + " Right Join Knitting_Mc_NO_UnitWise(2) K1 On F1.MachineID = K1.Machine ";
                        }
                        else
                        {
                            Str = Str + " Left Join Knitting_MC_NO_Unit() K1 On F1.MachineID = K1.Machine "; 
                        }


                        if (MyParent.UserCode == 1)
                        {
                            Str = Str + " Where DATEDIFF(dd, F2.entrydate, Cast(Getdate() as date))<= 100 ";
                        }
                        else
                        {
                            Str = Str + " Where DATEDIFF(dd, F2.entrydate, Cast(Getdate() as date))<= 1 ";
                        }

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Barcode - View", Str, String.Empty, 80, 90, 70, 150, 70);
                    }

                    //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Barcode - View", Str, String.Empty, 80, 90, 70);
                    if (Dr != null)
                    {
                        Fill_Datas(Dr);
                    }
                    else
                    {
                        Code = 0;
                    }
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
                DataTable Tdt = new DataTable();  
                groupBox1.Visible = true;
                radioButton2.Visible = false;
                radioButton2.Checked = true;


                Str = " Select Isnull(MIN(S2.Slno),0)From_Num, Isnull(MAX(S2.slno),0)To_Num from Socks_Barcode_Details S1 Left Join Socks_Bundle_Details S2 On S1.Socks_Bundle_Details_RowID = S2.RowID Where S2.EntryNO = " + TxtNo.Text +" And S1.Print_Status = 'N'";
                MyBase.Load_Data(Str, ref  Tdt);

                if (Convert.ToInt64(Tdt.Rows[0][0].ToString()) == 0 && Convert.ToInt64(Tdt.Rows[0][1].ToString()) == 0)
                {
                    Str = " Select Isnull(MIN(S1.Slno),0)From_Num, Isnull(MAX(S1.Slno),0)To_Num from Socks_Bundle_Details S1 Where EntryNO = 1";
                    MyBase.Load_Data(Str, ref  Tdt);

                    if (Tdt.Rows.Count > 0)
                    {
                        TxtFrmSlno.Text = Tdt.Rows[0]["From_Num"].ToString();
                        TxtToSlno.Text = Tdt.Rows[0]["To_Num"].ToString();
                    }
                }
                else
                {
                    TxtFrmSlno.Text = "";
                    TxtToSlno.Text = "";
                }

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
                Dt = new DataTable();
                if (MyParent._New)
                {
                    Str = "Select 0 as Slno, F2.RowID, E1.tno, E1.Name, F2.MachineID Machine,F2.Order_No, F2.OrderColorID, S1.color Sample, S1.size, F2.Production, ";
                    Str = Str + " (Case When " + TxtUnit.Tag + " = 1 Then 'G' Else 'S' End)+ '' + RIGHT('0000000000'+ISNULL(Cast(F2.RowID As Varchar),''),10)BundleNO, '-' T from Floor_Knitting_Master F1 ";
                    Str = Str + " Left Join Floor_Knitting_Details F2 On F1.RowID = F2.MasterID Right Join Knitting_Mc_NO_UnitWise(" + TxtUnit.Tag + ") M1 On F2.MachineID = M1.Machine";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F2.Emplno_Operator = E1.Emplno Left Join Socks_Bom() S1 on F2.Order_No = S1.Order_No And F2.OrderColorID = S1.OrderColorId ";
                    Str = Str + " Where F1.EntryDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' and F1.ShiftCode = " + TxtShift.Tag + " ";
                    if (TxtOperator.Tag.ToString() != "0" && TxtOperator.Text.ToString() != "")
                    {
                        Str = Str + " And F2.Emplno_Operator = " + TxtOperator.Tag + "";
                    }
                    Str = Str + " Order By E1.Name, F2.MachineID ";
                }
                else
                {
                    Str = "Select S1.Slno, S1.Floor_Knitting_Details_RowID RowID, E1.tno, E1.Name, F1.MachineID Machine, F1.Order_No, F1.OrderColorID, S2.color Sample, S2.size, ";
                    Str = Str + " F1.Production, S1.BundleNO, '-' T from Socks_Bundle_Details S1 Left Join Floor_Knitting_Details F1 on S1.Floor_Knitting_Details_RowID = F1.RowID ";
                    Str = Str + " Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Right Join Knitting_Mc_NO_UnitWise(" + TxtUnit.Tag + ") M1 On F1.MachineID = M1.Machine  ";
                    Str = Str + " Left Join Socks_Bom() S2 on F1.Order_No = S2.Order_No And F1.OrderColorID = S2.OrderColorId  Where S1.EntryNO = " + TxtNo.Text + " ";
                    if (TxtOperator.Tag.ToString() != "0" && TxtOperator.Text.ToString() != "")
                    {
                        Str = Str + " And F1.Emplno_Operator = " + TxtOperator.Tag + "";
                    }
                    Str = Str + " Order By S1.Slno ";
                }
            
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);

                MyBase.Grid_Designing(ref Grid, ref Dt, "RowID", "OrderColorID", "T");
                //MyBase.ReadOnly_Grid_Without(ref Grid, "Machine", "Order_No", "Production");
                MyBase.Grid_Width(ref Grid, 50, 100, 160, 90, 130, 120, 90, 80, 120);
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
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Unit", "Select Unit_Name, RowID Unit_Code from Socks_Unit_Master", String.Empty, 100, 80);
                if (Dr != null)
                {
                    TxtUnit.Text = Dr["Unit_Name"].ToString();
                    TxtUnit.Tag = Dr["Unit_Code"].ToString();
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
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Operator", "Select Name, Tno, Status, Emplno from Get_Operator_Barcode('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', " + TxtShift.Tag + ", " + TxtUnit.Tag + ")", String.Empty, 200, 80, 100);
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

        private void FrmBarcodePrint_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "BtnLoad")
                    {
                        Grid_Data();
                        TxtTotal.Focus(); 
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
                    else if (this.ActiveControl.Name == "TxtOperator")
                    {
                        Operator_Selection();
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

        private void FrmBarcodePrint_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == String.Empty)
                    {
                    }
                    else if (this.ActiveControl.Name == "TxtFrmSlno" || this.ActiveControl.Name == "TxtToSlno")
                    {
                        MyBase.Valid_Number(Txt, e);
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
                e.Handled = true;
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
                TxtTotal.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "Production", "Order_No", "Sample", "Name")));
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
                MessageBox.Show("Cann't Delete Old Records....Gainup");
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

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {

                if (Dt.Rows.Count <= 0)
                {
                    MessageBox.Show("Must View Data Before Print...!Gainup");
                    groupBox1.Visible = false;
                    return;
                }
                else
                {
                    DataTable Tmpdt = new DataTable();
                    String Str;

                    Str = " Select S1.RowID, S4.ProcessID, S1.Barcode, Print_Status from Socks_Barcode_Details S1 Left Join Socks_Bundle_Details S2 On S1.Socks_Bundle_Details_RowID = S2.RowID ";
                    Str = Str + " Left Join Floor_Knitting_Details F1 On S2.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 on F1.MasterID = F2.RowID ";
                    Str = Str + " Left Join Knitting_Mc_NO_UnitWise(" + TxtUnit.Tag + ") M1 On F1.MachineID = M1.Machine Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno ";
                    Str = Str + " Left Join Socks_Bom() S3 on F1.Order_No = S3.Order_No And F1.OrderColorID = S3.OrderColorId Left Join Barcode_Process_New() S4 On S3.Order_No = S4.Order_No And F1.OrderColorID = S4.ColorId ";
                    Str = Str + " Where S2.EntryNO = " + TxtNo.Text + " ";
                    Str = Str + " And S1.Print_Status = 'Y' ";

                    if (TxtOperator.Tag.ToString() != "0" && TxtOperator.Text.ToString() != "")
                    {
                        Str = Str + " And F1.Emplno_Operator = " + TxtOperator.Tag + "";
                    }

                    if (TxtFrmSlno.Text.ToString() != String.Empty && TxtToSlno.Text.ToString() != String.Empty)
                    {
                        Str = Str + " And S1.Socks_Bundle_Details_Slno Between " + TxtFrmSlno.Text + " And " + TxtToSlno.Text + " Order By S1.Socks_Bundle_Details_Slno ";
                    }
                    else
                    {
                        Str = Str + " Order By E1.Name, F1.MachineID ";
                    }

                    MyBase.Load_Data(Str, ref Tmpdt);
                    if (Tmpdt.Rows.Count > 0)
                    {
                        MessageBox.Show("Print Already Taken...!Gainup");
                        return;
                    }
                    else
                    {
                        Str = " Select S1.RowID, S4.ProcessID, S1.Barcode, Print_Status from Socks_Barcode_Details S1 Left Join Socks_Bundle_Details S2 On S1.Socks_Bundle_Details_RowID = S2.RowID ";
                        Str = Str + " Left Join Floor_Knitting_Details F1 On S2.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 on F1.MasterID = F2.RowID ";
                        Str = Str + " Left Join Knitting_Mc_NO_UnitWise(" + TxtUnit.Tag + ") M1 On F1.MachineID = M1.Machine Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno ";
                        Str = Str + " Left Join Socks_Bom() S3 on F1.Order_No = S3.Order_No And F1.OrderColorID = S3.OrderColorId Left Join Barcode_Process_New() S4 On S3.Order_No = S4.Order_No And F1.OrderColorID = S4.ColorId ";
                        Str = Str + " Where S2.EntryNO = " + TxtNo.Text + " ";
                        if (TxtOperator.Tag.ToString() != "0" && TxtOperator.Text.ToString() != "")
                        {
                            Str = Str + " And F1.Emplno_Operator = " + TxtOperator.Tag + "";
                        }
                        if (TxtFrmSlno.Text.ToString() != String.Empty && TxtToSlno.Text.ToString() != String.Empty)
                        {
                            Str = Str + " And S1.Socks_Bundle_Details_Slno Between " + TxtFrmSlno.Text + " And " + TxtToSlno.Text + " Order By S1.Socks_Bundle_Details_Slno ";
                        }
                        else
                        {
                            Str = Str + " Order By E1.Name, F1.MachineID ";
                        }
                        MyBase.Load_Data(Str, ref Tmpdt);

                        if (Tmpdt.Rows.Count > 0)
                        {

                        }
                        else
                        {
                            Str = "Insert Into Socks_Barcode_Details(Socks_Bundle_Details_RowID, ProcessID, Barcode, Print_Status, Socks_Bundle_Details_Slno) ";
                            Str = Str + " Select S1.RowID, S3.ProcessID, S1.BundleNO+''+ RIGHT('000'+ISNULL(Cast(S3.ProcessID As Varchar),''),3)Barcode, 'N' Print_Status, S1.Slno from Socks_Bundle_Details S1 ";
                            Str = Str + " Left Join Floor_Knitting_Details F1 On S1.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 on F1.MasterID = F2.RowID Left Join Knitting_Mc_NO_UnitWise(" + TxtUnit.Tag +") M1 On F1.MachineID = M1.Machine ";
                            Str = Str + " Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Left Join Socks_Bom() S2 on F1.Order_No = S2.Order_No And F1.OrderColorID = S2.OrderColorId Left Join Barcode_Process_New() S3 On S3.Order_No = F1.Order_No And F1.OrderColorID = S3.ColorId ";
                            Str = Str + " Where S1.EntryNO = " + TxtNo.Text + " ";
                            if (TxtOperator.Tag.ToString() != "0" && TxtOperator.Text.ToString() != "")
                            {
                                Str = Str + " And F1.Emplno_Operator = " + TxtOperator.Tag + "";
                            }
                            if (TxtFrmSlno.Text.ToString() != String.Empty && TxtToSlno.Text.ToString() != String.Empty)
                            {
                                Str = Str + " And S1.Slno Between " + TxtFrmSlno.Text + " And " + TxtToSlno.Text + " Order By S1.Slno ";
                            }
                            else
                            {
                                Str = Str + " Order By E1.Name, F1.MachineID ";
                            }
                            
                            MyBase.Load_Data(Str, ref Tmpdt);
                        }
                        Str = " Select Distinct Top 100000000 S1.RowID, Substring(S4.Process,1,3)Process, S1.Barcode, Print_Status, SubString(F1.Order_No,9,4)Order_No, ";
                        Str = Str + " Substring(S3.color,4,Len(S3.color)-3) Sample, S3.size, S2.BundleNO, F1.MachineID Machine, S1.Socks_Bundle_Details_Slno from Socks_Barcode_Details S1 Left Join Socks_Bundle_Details S2 On S1.Socks_Bundle_Details_RowID = S2.RowID ";
                        Str = Str + " Left Join Floor_Knitting_Details F1 On S2.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 on F1.MasterID = F2.RowID ";
                        Str = Str + " Left Join Knitting_Mc_NO_UnitWise(1) M1 On F1.MachineID = M1.Machine Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno ";
                        Str = Str + " Left Join Socks_Bom() S3 on F1.Order_No = S3.Order_No And F1.OrderColorID = S3.OrderColorId Left Join Barcode_Process_New() S4 On S4.Order_No = S3.Order_No And F1.OrderColorID = S4.ColorId And SUBSTRING(S1.Barcode,12,3) =  S4.processid ";
                        Str = Str + " Where S2.EntryNO = " + TxtNo.Text + " And S1.Print_Status = 'N' ";

                        if (TxtOperator.Tag.ToString() != "0" && TxtOperator.Text.ToString() != "")
                        {
                            Str = Str + " And F1.Emplno_Operator = " + TxtOperator.Tag + "";
                        }

                        if (TxtFrmSlno.Text.ToString() != String.Empty && TxtToSlno.Text.ToString() != String.Empty)
                        {
                            Str = Str + " And S1.Socks_Bundle_Details_Slno Between " + TxtFrmSlno.Text + " And " + TxtToSlno.Text + " Order By S1.Socks_Bundle_Details_Slno ";
                        }
                        else
                        {
                            Str = Str + " Order By E1.Name, F1.MachineID ";
                        }

                        MyBase.Load_Data(Str, ref Tmpdt);

                        if (Tmpdt.Rows.Count > 0)
                        {
                            //Print_BarCode3();
                            if (RBtBar2TVS.Checked == true)
                            {
                                Print_BarCode2();
                            }
                            else if (RBtBar2Honey.Checked == true)
                            {
                                Print_BarCode2_Honey();
                            }
                            MessageBox.Show("Ok ...!", "Gainup");
                            groupBox1.Visible = false;
                            Entry_View();
                        }
                        else
                        {
                            MessageBox.Show("Operation & Operator Details not Entered for this Order No/Style...!", "Gainup");
                            //return;
                            groupBox1.Visible = false;
                            Entry_View();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Print_DotMatrix()
        {
            
        }

        void Print_BarCode()
        {
            StreamWriter Sr = null;
            DataTable Tdt = new DataTable();
            Int32 i = 0;
            String Str = String.Empty;
            try
            {
                MyBase.Load_Data("select substring(order_no,10,13) OCN, Bundle_No,Line_No,Process_ID, Process,Master_ID from Floor_Line_Issue_Barcode() where master_id= " + Code + " ", ref Tdt);
                Sr = new StreamWriter("C:\\vaahrep\\Gar_Bar.txt");
                while (i <= Tdt.Rows.Count - 1)
                {
                    Sr.WriteLine("N");
                    Sr.WriteLine("ZT");
                    Sr.WriteLine("q814");
                    Sr.WriteLine("Q196, 24");
                    Sr.WriteLine("JF");
                    Sr.WriteLine("D9");
                    Sr.WriteLine("S4");
                    Sr.WriteLine("O");
                    Sr.WriteLine("A75,2,0,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Bundle_No"].ToString() + Convert.ToChar(34));
                    Str = String.Format("{0:00000000}", Convert.ToDouble(Tdt.Rows[i]["Bundle_No"])) + String.Format("{0:000}", Convert.ToDouble(Tdt.Rows[i]["Line_No"])) + String.Format("{0:000}", Convert.ToDouble(Tdt.Rows[i]["Process_ID"]));
                    Sr.WriteLine("A330,2,0,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["OCN"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("B75,35,0,1,2,4,61,N," + Convert.ToChar(34) + Str + Convert.ToChar(34));
                    Sr.WriteLine("A75,100,0,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("");

                    i += 1;
                    if (i <= Tdt.Rows.Count - 1)
                    {
                        Sr.WriteLine("A450,2,0,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Bundle_No"].ToString() + Convert.ToChar(34));
                        Str = String.Format("{0:00000000}", Convert.ToDouble(Tdt.Rows[i]["Bundle_No"])) + String.Format("{0:000}", Convert.ToDouble(Tdt.Rows[i]["Line_No"])) + String.Format("{0:000}", Convert.ToDouble(Tdt.Rows[i]["Process_ID"]));
                        Sr.WriteLine("A728,2,0,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["OCN"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("B450,35,0,1,2,4,61,N," + Convert.ToChar(34) + Str + Convert.ToChar(34));
                        Sr.WriteLine("A450,100,0,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("P1");
                        Sr.WriteLine("FE");
                        Sr.WriteLine("");
                        Sr.WriteLine("");
                        Sr.WriteLine("");
                        i += 1;
                    }
                    else
                    {
                        Sr.WriteLine("P1");
                        Sr.WriteLine("FE");
                        Sr.WriteLine("");
                        Sr.WriteLine("");
                        Sr.WriteLine("");
                    }
                }
                Sr.Close();
                MyBase.DosPrint("C:\\vaahrep\\Gar_Bar.txt");
                Sr = null;
                groupBox1.Visible = false;
                Entry_View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Sr != null)
                {
                    Sr.Close();
                }
            }
        }

        void Print_BarCode2()
        {
            StreamWriter Sr = null;
            DataTable Tdt = new DataTable();
            Int32 i = 0;
            String Str = String.Empty;
            try
            {
                if (radioButton2.Checked == true)
                {
                    Str = " Select Distinct Top 100000000 S1.RowID, Substring(S4.Process,1,3)Process, S1.Barcode, Print_Status, SubString(F1.Order_No,9,4)Order_No, ";
                    Str = Str + " Substring(S3.color,4,Len(S3.color)-3) Sample, S3.size, S2.BundleNO, F1.MachineID + ' - '+ Substring(S4.Process,1,1) Machine, S1.Socks_Bundle_Details_Slno, E1.Name, F1.Production, S3.Im_Color Color from Socks_Barcode_Details S1 Left Join Socks_Bundle_Details S2 On S1.Socks_Bundle_Details_RowID = S2.RowID ";
                    Str = Str + " Left Join Floor_Knitting_Details F1 On S2.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 on F1.MasterID = F2.RowID ";
                    Str = Str + " Left Join Knitting_Mc_NO_UnitWise(" + TxtUnit.Tag + ") M1 On F1.MachineID = M1.Machine Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno ";
                    Str = Str + " Left Join Socks_Bom() S3 on F1.Order_No = S3.Order_No And F1.OrderColorID = S3.OrderColorId Left Join Barcode_Process_New() S4 On S4.Order_No = F1.Order_No And F1.OrderColorID = S4.ColorId And SUBSTRING(S1.Barcode,12,3) =  S4.processid ";
                    Str = Str + " Where S2.EntryNO = " + TxtNo.Text + " And S1.Print_Status = 'N' ";

                    if (TxtOperator.Tag.ToString() != "0" && TxtOperator.Text.ToString() != "")
                    {
                        Str = Str + " And F1.Emplno_Operator = " + TxtOperator.Tag + "";
                    }

                    if (TxtFrmSlno.Text.ToString() != String.Empty && TxtToSlno.Text.ToString() != String.Empty)
                    {
                        Str = Str + " And S1.Socks_Bundle_Details_Slno Between " + TxtFrmSlno.Text + " And " + TxtToSlno.Text + " Order By S1.Socks_Bundle_Details_Slno ";
                    }
                    else
                    {

                        Str = Str + " Order By E1.Name, F1.MachineID ";
                    }

                    MyBase.Load_Data(Str, ref Tdt);
                }
                Sr = new StreamWriter("C:\\vaahrep\\Socks_Bar2.txt");
                while (i <= Tdt.Rows.Count - 1)
                {
                    Sr.WriteLine("N");
                    Sr.WriteLine("ZT");
                    Sr.WriteLine("q814");
                    Sr.WriteLine("Q196, 24");
                    Sr.WriteLine("JF");
                    Sr.WriteLine("D9");
                    Sr.WriteLine("S4");
                    Sr.WriteLine("O");
                    Sr.WriteLine("A370,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["BundleNO"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A370,280,1,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Machine"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("B340,50,1,1,2,4,61,B," + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A250,320,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Socks_Bundle_Details_Slno"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A220,50,1,3,1,1,N," + Convert.ToChar(34) + "OCN" + Convert.ToChar(34));
                    Sr.WriteLine("A220,120,1,3,1,1,N," + Convert.ToChar(34) + "SAM" + Convert.ToChar(34));
                    Sr.WriteLine("A220,220,1,3,1,1,N," + Convert.ToChar(34) + "PRO" + Convert.ToChar(34));
                    Sr.WriteLine("A220,280,1,3,1,1,N," + Convert.ToChar(34) + "SI" + Convert.ToChar(34));
                    Sr.WriteLine("A190,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Order_No"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A190,120,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Sample"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A190,220,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A190,280,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["size"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A160,50,1,3,1,1,N," + Convert.ToChar(34) + "QTY" + Convert.ToChar(34));
                    Sr.WriteLine("A160,120,1,3,1,1,N," + Convert.ToChar(34) + "CLR" + Convert.ToChar(34));
                    Sr.WriteLine("A130,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Production"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A130,120,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Color"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A100,50,1,3,1,1,N," + Convert.ToChar(34) + "OPR" + Convert.ToChar(34));
                    Sr.WriteLine("A100,120,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Name"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("");

                    i += 1;
                    if (i <= Tdt.Rows.Count - 1)
                    {
                        Sr.WriteLine("A750,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["BundleNO"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A750,280,1,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Machine"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("B720,50,1,1,2,4,61,B," + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A620,320,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Socks_Bundle_Details_Slno"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A590,50,1,3,1,1,N," + Convert.ToChar(34) + "OCN" + Convert.ToChar(34));
                        Sr.WriteLine("A590,120,1,3,1,1,N," + Convert.ToChar(34) + "SAM" + Convert.ToChar(34));
                        Sr.WriteLine("A590,220,1,3,1,1,N," + Convert.ToChar(34) + "PRO" + Convert.ToChar(34));
                        Sr.WriteLine("A590,280,1,3,1,1,N," + Convert.ToChar(34) + "SI" + Convert.ToChar(34));
                        Sr.WriteLine("A560,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Order_No"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A560,120,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Sample"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A560,220,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A560,280,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["size"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A530,50,1,3,1,1,N," + Convert.ToChar(34) + "QTY" + Convert.ToChar(34));
                        Sr.WriteLine("A530,120,1,3,1,1,N," + Convert.ToChar(34) + "CLR" + Convert.ToChar(34));
                        Sr.WriteLine("A500,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Production"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A500,120,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Color"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A480,50,1,3,1,1,N," + Convert.ToChar(34) + "OPR" + Convert.ToChar(34));
                        Sr.WriteLine("A480,120,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Name"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("");
                        Sr.WriteLine("P1");
                        Sr.WriteLine("FE");
                        Sr.WriteLine("");
                        Sr.WriteLine("");

                        i += 1;
                    }
                    else
                    {
                        Sr.WriteLine("P1");
                        Sr.WriteLine("FE");
                        Sr.WriteLine("");
                        Sr.WriteLine("");
                        Sr.WriteLine("");
                    }
                }
                //MyBase.Run("update Socks_Barcode_Details set Print_Status = 'Y' Where Socks_Bundle_Details_Slno Between " + TxtFrmSlno.Text + " And " + TxtToSlno.Text + " "); 
                Sr.Close();
                MyBase.DosPrint("C:\\vaahrep\\Socks_Bar2.txt");
                Sr = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Sr != null)
                {
                    Sr.Close();
                }
            }
        }

        void Print_BarCode2_Honey()
        {
            StreamWriter Sr = null;
            DataTable Tdt = new DataTable();
            Int32 i = 0;
            String Str = String.Empty;
            try
            {
                if (radioButton2.Checked == true)
                {
                    Str = " Select Distinct Top 100000000 S1.RowID, Substring(S4.Process,1,3)Process, S1.Barcode, Print_Status, SubString(F1.Order_No,9,4)Order_No, ";
                    Str = Str + " Substring(S3.color,4,Len(S3.color)-3) Sample, S3.size, S2.BundleNO, F1.MachineID + ' - '+ Substring(S4.Process,1,1) Machine, S1.Socks_Bundle_Details_Slno, E1.Name, F1.Production, S3.Im_Color Color, Substring(CONVERT(VARCHAR(10),F2.EntryDate,103),1,5)Date1, Cast('S-' +  Rtrim(Ltrim(S5.shiftcode2)) As Varchar(3))Shift from Socks_Barcode_Details S1 Left Join Socks_Bundle_Details S2 On S1.Socks_Bundle_Details_RowID = S2.RowID ";
                    Str = Str + " Left Join Floor_Knitting_Details F1 On S2.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 on F1.MasterID = F2.RowID ";
                    Str = Str + " Left Join Knitting_Mc_NO_UnitWise(" + TxtUnit.Tag + ") M1 On F1.MachineID = M1.Machine Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Left Join VAAHINI_ERP_GAINUP.dbo.Shiftmst S5 on F2.ShiftCode = S5.shiftcode And S5.compcode = 2 And S5.Mode = 1 ";
                    Str = Str + " Left Join Socks_Bom() S3 on F1.Order_No = S3.Order_No And F1.OrderColorID = S3.OrderColorId Left Join Barcode_Process_New() S4 On S4.Order_No = F1.Order_No And F1.OrderColorID = S4.ColorId And SUBSTRING(S1.Barcode,12,3) =  S4.processid ";
                    Str = Str + " Where S2.EntryNO = " + TxtNo.Text + " And S1.Print_Status = 'N' ";

                    if (TxtOperator.Tag.ToString() != "0" && TxtOperator.Text.ToString() != "")
                    {
                        Str = Str + " And F1.Emplno_Operator = " + TxtOperator.Tag + "";
                    }

                    if (TxtFrmSlno.Text.ToString() != String.Empty && TxtToSlno.Text.ToString() != String.Empty)
                    {
                        Str = Str + " And S1.Socks_Bundle_Details_Slno Between " + TxtFrmSlno.Text + " And " + TxtToSlno.Text + " Order By S1.Socks_Bundle_Details_Slno ";
                    }
                    else
                    {

                        Str = Str + " Order By E1.Name, F1.MachineID ";
                    }

                    MyBase.Load_Data(Str, ref Tdt);
                }
                Sr = new StreamWriter("C:\\vaahrep\\Socks_Bar2_Honey.txt");
                while (i <= Tdt.Rows.Count - 1)
                {
                    Sr.WriteLine("INPUT ON");
                    Sr.WriteLine("SYSVAR(48) = 0");
                    Sr.WriteLine("SYSVAR(35)=0");
                    Sr.WriteLine("D20");
                    Sr.WriteLine("OPTIMIZE #BATCH# + ON".Replace('#', '"'));
                    Sr.WriteLine("PP12,52:AN7");
                    Sr.WriteLine("DIR4");
                    Sr.WriteLine("NASC 8");
                    Sr.WriteLine("PP25,54:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Date1"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("PP25,150:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Shift"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("PP25,280:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Machine"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("PP60,54:BARSET #CODE128#,2,2,2,61".Replace('#', '"'));
                    Sr.WriteLine("PB " + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("PP130,54:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("PP130,310:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Socks_Bundle_Details_Slno"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("PP165,54:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + "OCN" + Convert.ToChar(34));
                    Sr.WriteLine("PP165,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + "SAM" + Convert.ToChar(34));
                    Sr.WriteLine("PP165,190:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + "PRO" + Convert.ToChar(34));
                    Sr.WriteLine("PP165,260:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + "SI" + Convert.ToChar(34));
                    Sr.WriteLine("PP200,54:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Order_No"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("PP200,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Sample"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("PP200,190:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("PP200,260:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["size"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("PP245,54:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + "QTY" + Convert.ToChar(34));
                    Sr.WriteLine("PP245,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + "CLR" + Convert.ToChar(34));
                    Sr.WriteLine("PP280,54:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Production"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("PP280,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Color"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("PP325,54:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + "OPR" + Convert.ToChar(34));
                    Sr.WriteLine("PP325,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                    Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Name"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("");

                    i += 1;
                    if (i <= Tdt.Rows.Count - 1)
                    {
                        Sr.WriteLine("PP430,54:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Date1"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("PP430,150:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Shift"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("PP430,280:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Machine"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("PP465,54:BARSET #CODE128#,2,2,2,61".Replace('#', '"'));
                        Sr.WriteLine("PB " + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("PP535,54:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("PP535,310:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Socks_Bundle_Details_Slno"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("PP570,54:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + "OCN" + Convert.ToChar(34));
                        Sr.WriteLine("PP570,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + "SAM" + Convert.ToChar(34));
                        Sr.WriteLine("PP570,190:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + "PRO" + Convert.ToChar(34));
                        Sr.WriteLine("PP570,260:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + "SI" + Convert.ToChar(34));
                        Sr.WriteLine("PP600,54:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Order_No"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("PP600,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Sample"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("PP600,190:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString());
                        Sr.WriteLine("PP600,260:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["size"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("PP645,54:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + "QTY" + Convert.ToChar(34));
                        Sr.WriteLine("PP645,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + "CLR" + Convert.ToChar(34));
                        Sr.WriteLine("PP690,54:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Production"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("PP690,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Color"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("PP725,54:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + "OPR" + Convert.ToChar(34));
                        Sr.WriteLine("PP725,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
                        Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Name"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("");
                        Sr.WriteLine("PF");
                        Sr.WriteLine("");
                        Sr.WriteLine("");

                        i += 1;
                    }
                    else
                    {
                        Sr.WriteLine("PF");
                        Sr.WriteLine("");
                        Sr.WriteLine("");
                        Sr.WriteLine("");
                    }
                }
                //MyBase.Run("update Socks_Barcode_Details set Print_Status = 'Y' Where Socks_Bundle_Details_Slno Between " + TxtFrmSlno.Text + " And " + TxtToSlno.Text + " "); 
                Sr.Close();
                MyBase.DosPrint("C:\\vaahrep\\Socks_Bar2_Honey.txt");
                Sr = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Sr != null)
                {
                    Sr.Close();
                }
            }
        }

        //void Print_BarCode2_Honey()
        //{
        //    StreamWriter Sr = null;
        //    DataTable Tdt = new DataTable();
        //    Int32 i = 0;
        //    String Str = String.Empty;
        //    try
        //    {
        //        if (radioButton2.Checked == true)
        //        {
        //            Str = " Select Distinct Top 100000000 S1.RowID, Substring(S4.Process,1,3)Process, S1.Barcode, Print_Status, SubString(F1.Order_No,9,4)Order_No, ";
        //            Str = Str + " Substring(S3.color,4,Len(S3.color)-3) Sample, S3.size, S2.BundleNO, F1.MachineID + ' - '+ Substring(S4.Process,1,1) Machine, S1.Socks_Bundle_Details_Slno, E1.Name, F1.Production, S3.Im_Color Color from Socks_Barcode_Details S1 Left Join Socks_Bundle_Details S2 On S1.Socks_Bundle_Details_RowID = S2.RowID ";
        //            Str = Str + " Left Join Floor_Knitting_Details F1 On S2.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 on F1.MasterID = F2.RowID ";
        //            Str = Str + " Left Join Knitting_Mc_NO_UnitWise(" + TxtUnit.Tag + ") M1 On F1.MachineID = M1.Machine Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno ";
        //            Str = Str + " Left Join Socks_Bom() S3 on F1.Order_No = S3.Order_No And F1.OrderColorID = S3.OrderColorId Left Join Barcode_Process_New() S4 On S4.Order_No = F1.Order_No And F1.OrderColorID = S4.ColorId And SUBSTRING(S1.Barcode,12,3) =  S4.processid ";
        //            Str = Str + " Where S2.EntryNO = " + TxtNo.Text + " And S1.Print_Status = 'N' ";

        //            if (TxtOperator.Tag.ToString() != "0" && TxtOperator.Text.ToString() != "")
        //            {
        //                Str = Str + " And F1.Emplno_Operator = " + TxtOperator.Tag + "";
        //            }

        //            if (TxtFrmSlno.Text.ToString() != String.Empty && TxtToSlno.Text.ToString() != String.Empty)
        //            {
        //                Str = Str + " And S1.Socks_Bundle_Details_Slno Between " + TxtFrmSlno.Text + " And " + TxtToSlno.Text + " Order By S1.Socks_Bundle_Details_Slno ";
        //            }
        //            else
        //            {

        //                Str = Str + " Order By E1.Name, F1.MachineID ";
        //            }

        //            MyBase.Load_Data(Str, ref Tdt);
        //        }
        //        Sr = new StreamWriter("C:\\vaahrep\\Socks_Bar2_Honey.txt");
        //        while (i <= Tdt.Rows.Count - 1)
        //        {
        //            Sr.WriteLine("INPUT ON");
        //            Sr.WriteLine("SYSVAR(48) = 0");
        //            Sr.WriteLine("SYSVAR(35)=0");
        //            Sr.WriteLine("OPTIMIZE #BATCH# + ON".Replace('#', '"'));
        //            Sr.WriteLine("PP12,52:AN7");
        //            Sr.WriteLine("DIR4");
        //            Sr.WriteLine("NASC 8");
        //            Sr.WriteLine("PP25,34:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["BundleNO"].ToString() + Convert.ToChar(34));
        //            Sr.WriteLine("PP25,290:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Machine"].ToString() + Convert.ToChar(34));
        //            Sr.WriteLine("PP60,34:BARSET #CODE128#,2,2,2,61".Replace('#', '"'));
        //            Sr.WriteLine("PB " + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
        //            Sr.WriteLine("PP130,34:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
        //            Sr.WriteLine("PP130,310:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Socks_Bundle_Details_Slno"].ToString() + Convert.ToChar(34));
        //            Sr.WriteLine("PP165,34:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + "OCN" + Convert.ToChar(34));
        //            Sr.WriteLine("PP165,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + "SAM" + Convert.ToChar(34));
        //            Sr.WriteLine("PP165,190:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + "PRO" + Convert.ToChar(34));
        //            Sr.WriteLine("PP165,260:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + "SI" + Convert.ToChar(34));
        //            Sr.WriteLine("PP200,34:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Order_No"].ToString() + Convert.ToChar(34));
        //            Sr.WriteLine("PP200,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Sample"].ToString() + Convert.ToChar(34));
        //            Sr.WriteLine("PP200,190:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString() + Convert.ToChar(34));
        //            Sr.WriteLine("PP200,260:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["size"].ToString() + Convert.ToChar(34));
        //            Sr.WriteLine("PP245,34:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + "QTY" + Convert.ToChar(34));
        //            Sr.WriteLine("PP245,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + "CLR" + Convert.ToChar(34));
        //            Sr.WriteLine("PP280,34:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Production"].ToString() + Convert.ToChar(34));
        //            Sr.WriteLine("PP280,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Color"].ToString() + Convert.ToChar(34));
        //            Sr.WriteLine("PP325,34:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + "OPR" + Convert.ToChar(34));
        //            Sr.WriteLine("PP325,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //            Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Name"].ToString() + Convert.ToChar(34));
        //            Sr.WriteLine("");

        //            i += 1;
        //            if (i <= Tdt.Rows.Count - 1)
        //            {
        //                Sr.WriteLine("PP430,34:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["BundleNO"].ToString() + Convert.ToChar(34));
        //                Sr.WriteLine("PP430,290:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Machine"].ToString() + Convert.ToChar(34));
        //                Sr.WriteLine("PP465,34:BARSET #CODE128#,2,2,2,61".Replace('#', '"'));
        //                Sr.WriteLine("PB " + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
        //                Sr.WriteLine("PP535,34:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
        //                Sr.WriteLine("PP535,310:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Socks_Bundle_Details_Slno"].ToString() + Convert.ToChar(34));
        //                Sr.WriteLine("PP570,34:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + "OCN" + Convert.ToChar(34));
        //                Sr.WriteLine("PP570,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + "SAM" + Convert.ToChar(34));
        //                Sr.WriteLine("PP570,190:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + "PRO" + Convert.ToChar(34));
        //                Sr.WriteLine("PP570,260:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + "SI" + Convert.ToChar(34));
        //                Sr.WriteLine("PP600,34:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Order_No"].ToString() + Convert.ToChar(34));
        //                Sr.WriteLine("PP600,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Sample"].ToString() + Convert.ToChar(34));
        //                Sr.WriteLine("PP600,190:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString());
        //                Sr.WriteLine("PP600,260:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["size"].ToString() + Convert.ToChar(34));
        //                Sr.WriteLine("PP645,34:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + "QTY" + Convert.ToChar(34));
        //                Sr.WriteLine("PP645,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + "CLR" + Convert.ToChar(34));
        //                Sr.WriteLine("PP690,34:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Production"].ToString() + Convert.ToChar(34));
        //                Sr.WriteLine("PP690,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Color"].ToString() + Convert.ToChar(34));
        //                Sr.WriteLine("PP725,34:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + "OPR" + Convert.ToChar(34));
        //                Sr.WriteLine("PP725,120:FT #CG Triumvirate Condensed Bold#,10,0,99".Replace('#', '"'));
        //                Sr.WriteLine("PT " + Convert.ToChar(34) + Tdt.Rows[i]["Name"].ToString() + Convert.ToChar(34));
        //                Sr.WriteLine("");
        //                Sr.WriteLine("PF");
        //                Sr.WriteLine("");
        //                Sr.WriteLine("");

        //                i += 1;
        //            }
        //            else
        //            {
        //                Sr.WriteLine("PF");
        //                Sr.WriteLine("");
        //                Sr.WriteLine("");
        //                Sr.WriteLine("");
        //            }
        //        }
        //        //MyBase.Run("update Socks_Barcode_Details set Print_Status = 'Y' Where Socks_Bundle_Details_Slno Between " + TxtFrmSlno.Text + " And " + TxtToSlno.Text + " "); 
        //        Sr.Close();
        //        MyBase.DosPrint("C:\\vaahrep\\Socks_Bar2_Honey.txt");
        //        Sr = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (Sr != null)
        //        {
        //            Sr.Close();
        //        }
        //    }
        //}


        void Print_BarCode3()
        {
            StreamWriter Sr = null;
            DataTable Tdt = new DataTable();
            Int32 i = 0;
            String Str = String.Empty;
            try
            {
                if (radioButton2.Checked == true)
                {
                    Str = " Select Distinct Top 100000000 S1.RowID, Substring(S4.Process,1,3)Process, S1.Barcode, Print_Status, SubString(F1.Order_No,9,4)Order_No, ";
                    Str = Str + " Substring(S3.color,4,Len(S3.color)-3) Sample, S3.size, S2.BundleNO, F1.MachineID + ' - '+ Substring(S4.Process,1,1) Machine, S1.Socks_Bundle_Details_Slno from Socks_Barcode_Details S1 Left Join Socks_Bundle_Details S2 On S1.Socks_Bundle_Details_RowID = S2.RowID ";
                    Str = Str + " Left Join Floor_Knitting_Details F1 On S2.Floor_Knitting_Details_RowID = F1.RowID Left Join Floor_Knitting_Master F2 on F1.MasterID = F2.RowID ";
                    Str = Str + " Left Join Knitting_Mc_NO_UnitWise(" + TxtUnit.Tag + ") M1 On F1.MachineID = M1.Machine Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno ";
                    Str = Str + " Left Join Socks_Bom() S3 on F1.Order_No = S3.Order_No And F1.OrderColorID = S3.OrderColorId Left Join Barcode_Process_New() S4 On S4.Order_No = F1.Order_No And F1.OrderColorID = S4.ColorId And SUBSTRING(S1.Barcode,12,3) =  S4.processid ";
                    Str = Str + " Where S2.EntryNO = " + TxtNo.Text + " And S1.Print_Status = 'N' ";

                    if (TxtOperator.Tag.ToString() != "0" && TxtOperator.Text.ToString() != "")
                    {
                        Str = Str + " And F1.Emplno_Operator = " + TxtOperator.Tag + "";
                    }

                    if (TxtFrmSlno.Text.ToString() != String.Empty && TxtToSlno.Text.ToString() != String.Empty)
                    {
                        Str = Str + " And S1.Socks_Bundle_Details_Slno Between " + TxtFrmSlno.Text + " And " + TxtToSlno.Text + " Order By S1.Socks_Bundle_Details_Slno ";
                    }
                    else
                    {

                        Str = Str + " Order By E1.Name, F1.MachineID ";
                    }

                    MyBase.Load_Data(Str, ref Tdt);
                }
                Sr = new StreamWriter("C:\\vaahrep\\Socks_Bar3.txt");
                while (i <= Tdt.Rows.Count - 1)
                {
                    Sr.WriteLine("N");
                    Sr.WriteLine("ZT");
                    Sr.WriteLine("q814");
                    Sr.WriteLine("Q196, 24");
                    Sr.WriteLine("JF");
                    Sr.WriteLine("D9");
                    Sr.WriteLine("S4");
                    Sr.WriteLine("O");
                    Sr.WriteLine("A260,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["BundleNO"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A270,280,1,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Machine"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("B240,50,1,1,2,4,61,B," + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A170,320,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Socks_Bundle_Details_Slno"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A140,50,1,3,1,1,N," + Convert.ToChar(34) + "OCN" + Convert.ToChar(34));
                    Sr.WriteLine("A140,120,1,3,1,1,N," + Convert.ToChar(34) + "SAM" + Convert.ToChar(34));
                    Sr.WriteLine("A140,220,1,3,1,1,N," + Convert.ToChar(34) + "PRO" + Convert.ToChar(34));
                    Sr.WriteLine("A140,280,1,3,1,1,N," + Convert.ToChar(34) + "SI" + Convert.ToChar(34));
                    Sr.WriteLine("A110,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Order_No"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A110,120,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Sample"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A110,220,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A110,280,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["size"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("");

                    i += 1;
                    if (i <= Tdt.Rows.Count - 1)
                    {
                        Sr.WriteLine("A500,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["BundleNO"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("510,280,1,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Machine"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("B480,50,1,1,2,4,61,B," + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A410,320,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Socks_Bundle_Details_Slno"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A380,50,1,3,1,1,N," + Convert.ToChar(34) + "OCN" + Convert.ToChar(34));
                        Sr.WriteLine("A380,120,1,3,1,1,N," + Convert.ToChar(34) + "SAM" + Convert.ToChar(34));
                        Sr.WriteLine("A380,220,1,3,1,1,N," + Convert.ToChar(34) + "PRO" + Convert.ToChar(34));
                        Sr.WriteLine("A380,280,1,3,1,1,N," + Convert.ToChar(34) + "SI" + Convert.ToChar(34));
                        Sr.WriteLine("A350,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Order_No"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A350,120,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Sample"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A350,220,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A350,280,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["size"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("");
                        i += 1;
                    }

                    if (i <= Tdt.Rows.Count - 1)
                    {
                        Sr.WriteLine("A740,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["BundleNO"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A750,280,1,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Machine"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("B720,50,1,1,2,4,61,B," + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A650,320,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Socks_Bundle_Details_Slno"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A620,50,1,3,1,1,N," + Convert.ToChar(34) + "OCN" + Convert.ToChar(34));
                        Sr.WriteLine("A620,120,1,3,1,1,N," + Convert.ToChar(34) + "SAM" + Convert.ToChar(34));
                        Sr.WriteLine("A620,220,1,3,1,1,N," + Convert.ToChar(34) + "PRO" + Convert.ToChar(34));
                        Sr.WriteLine("A620,280,1,3,1,1,N," + Convert.ToChar(34) + "SI" + Convert.ToChar(34));
                        Sr.WriteLine("A590,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Order_No"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A590,120,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Sample"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A590,220,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A590,280,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["size"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("");
                        Sr.WriteLine("P1");
                        Sr.WriteLine("FE");
                        Sr.WriteLine("");
                        Sr.WriteLine("");

                        i += 1;
                    }
                    else
                    {
                        Sr.WriteLine("P1");
                        Sr.WriteLine("FE");
                        Sr.WriteLine("");
                        Sr.WriteLine("");
                        Sr.WriteLine("");
                    }
                }
                //MyBase.Run("update Socks_Barcode_Details set Print_Status = 'Y' Where Socks_Bundle_Details_Slno Between " + TxtFrmSlno.Text + " And " + TxtToSlno.Text + " "); 
                Sr.Close();
                MyBase.DosPrint("C:\\vaahrep\\Socks_Bar3.txt");
                Sr = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Sr != null)
                {
                    Sr.Close();
                }
            }
        }

        private void BtnCancel_Click_1(object sender, EventArgs e)
        {
            radioButton2.Checked = false;
            groupBox1.Visible = false;
        }

        private void myTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            Grid_Data();
            TxtTotal.Focus();
            return;
        }

    }
}