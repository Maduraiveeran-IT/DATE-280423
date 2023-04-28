using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using SelectionTool_NmSp;
using Accounts_ControlModules;
using System.Windows.Forms;
using Accounts;

namespace Accounts
{
    public partial class FrmMachineProduction : Form
    {

        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        DataTable Dt = new DataTable();
     
        public FrmMachineProduction()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        void Fill_Combo()
        {
            DataTable Tdt = new DataTable();
            try
            {

                // SHIFT
                CmbShift.Items.Clear();
                CmbShift.Items.Add("1");
                CmbShift.Items.Add("2");
                CmbShift.Items.Add("3");

                // Needle
                CmbNeedle.Items.Clear();
                MyBase.Load_Data("Select Name From VFit_Sample_Needle_Master where Active='Y' order by Name", ref Tdt);
                for (int i = 0; i <= Tdt.Rows.Count - 1; i++)
                {
                    CmbNeedle.Items.Add(Tdt.Rows[i]["Name"].ToString());
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        void FillFShift_Combo()
        {
            DataTable Tdt = new DataTable();
            try
            {

                // SHIFT
                CmbFShift.Items.Clear();
                CmbFShift.Items.Add("1");
                CmbFShift.Items.Add("2");
                CmbFShift.Items.Add("3");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void FillTShift_Combo()
        {
            DataTable Tdt = new DataTable();
            try
            {

                // SHIFT
                CmbTShift.Items.Clear();
                CmbTShift.Items.Add("1");
                CmbTShift.Items.Add("2");
                CmbTShift.Items.Add("3");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        private void FrmMachineProduction_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                MyBase.Disable_Cut_Copy(groupBox1);
                MyBase.Disable_Cut_Copy(groupBox2);
                groupBox2.Visible = false; 
                Fill_Combo();
                FillFShift_Combo();
                FillTShift_Combo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                Fill_Combo();
                Dt = new DataTable();
                Grid.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmMachineProduction_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    DtpEDate.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmMachineProduction_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Fill_Year()
        {
            DataTable Tdt = new DataTable();
            try
            {
                TxtYear.Text = DtpEDate.Value.Year.ToString();
                MyBase.Load_Data("Select Datepart(Week, '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "')", ref Tdt);
                TxtWeek.Text = Tdt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DtpEDate_Leave(object sender, EventArgs e)
        {
            
            try
            {
                Fill_Year();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Fill_Year();

                if (CmbShift.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Shift ...!", "Gainup");
                    CmbShift.Focus();
                    return;
                }

                if (CmbNeedle.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Needle ...!", "Gainup");
                    CmbNeedle.Focus();
                    return;
                }

                //Grid.DataSource = MyBase.Load_Data("Select 0 as Slno, S1.Machine_ID, K1.Machine, S1.Order_No, S1.Plan_Qty, Isnull(P1.Prod_Qty, 0) Prod_Qty, Isnull(S5.Assign_Qty, 0) Assign_Qty, (Case When S5.Planned_Seconds is null then 0 else Cast(((S5.Planned_Seconds / Cast(28800 as Numeric (10, 2))) * 100) as Numeric (5, 2)) end) Utilization From Socks_Machine_Planning_Details S1 Left Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID Left Join VFit_Sample_Needle_Master V1 on S2.Needle_Id = V1.RowID Left Join Production_Qty_WeekWise () P1 on S1.Order_No = P1.Order_No And P1.Needle_ID = S2.Needle_Id and P1.Year = S2.Year and P1.Week = S2.Week Left Join Knitting_Mc_NO () K1 on S1.Machine_ID = K1.Machine_ID left join Socks_Machine_Production_Master S5 on S5.Machine_ID = S1.Machine_ID and S5.Order_No = S1.Order_No and S5.year = S2.Year and S5.Week = S2.Week and S5.Needle_ID = S2.Needle_Id and S5.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' and S5.Shift = " + CmbShift.Text + " Where S2.Year = " + TxtYear.Text + " and S2.Week = " + TxtWeek.Text + " and V1.Name = '" + CmbNeedle.Text.Trim() + "' Order by K1.Machine", ref Dt);
                if (MyParent.UserCode == 7)
                {
                    Grid.DataSource = MyBase.Load_Data("Select Distinct 0 as Slno, S1.Machine_ID, K1.Machine, S1.Order_No, S1.Plan_Qty, Isnull(P1.Prod_Qty, 0) Prod_Qty, Isnull(S5.Assign_Qty, 0) Assign_Qty, (Case When S5.Planned_Seconds is null then 0 else Cast(((S5.Planned_Seconds / Cast(28800 as Numeric (10, 2))) * 100) as Numeric (10, 2)) end) Utilization, '-' T, Left(K1.Machine,PATINDEX('%[0-9]%',K1.Machine)-1)Machine_Alpha, CONVERT (Int,SUBSTRING(K1.Machine,PATINDEX('%[0-9]%',K1.Machine),LEN(K1.Machine)))Machine_Num From Socks_Machine_Planning_Details S1 Left Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID Left Join VFit_Sample_Needle_Master V1 on S2.Needle_Id = V1.RowID Left Join Production_Qty_WeekWise () P1 on S1.Order_No = P1.Order_No And P1.Needle_ID = S2.Needle_Id and P1.Year = S2.Year and P1.Week = S2.Week Right Join Knitting_Mc_NO_UnitWise(1) K1 on S1.Machine_ID = K1.Machine_ID left join Socks_Machine_Production_Master S5 on S5.Machine_ID = S1.Machine_ID and S5.Order_No = S1.Order_No and S5.year = S2.Year and S5.Week = S2.Week and S5.Needle_ID = S2.Needle_Id and S5.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' and S5.Shift = " + CmbShift.Text + " Where S2.Year = " + TxtYear.Text + " and S2.Week = " + TxtWeek.Text + " and V1.Name = '" + CmbNeedle.Text.Trim() + "' And S1.RowID in (Select MAX(P2.RowID) from Socks_Machine_Planning_Master P1 Left Join Socks_Machine_Planning_Details P2 On P1.RowID = P2.Master_ID Left Join VFit_Sample_Needle_Master V1 On P1.Needle_Id = V1.RowID Where P1.Year = " + TxtYear.Text + " and P1.Week = " + TxtWeek.Text + " and V1.Name = '" + CmbNeedle.Text.Trim() + "' Group by P2.Machine_ID) Order by Left(K1.Machine,PATINDEX('%[0-9]%',K1.Machine)-1),CONVERT (Int,SUBSTRING(K1.Machine,PATINDEX('%[0-9]%',K1.Machine),LEN(K1.Machine)))", ref Dt);
                }
                else if (MyParent.UserCode == 40)
                {
                    Grid.DataSource = MyBase.Load_Data("Select Distinct 0 as Slno, S1.Machine_ID, K1.Machine, S1.Order_No, S1.Plan_Qty, Isnull(P1.Prod_Qty, 0) Prod_Qty, Isnull(S5.Assign_Qty, 0) Assign_Qty, (Case When S5.Planned_Seconds is null then 0 else Cast(((S5.Planned_Seconds / Cast(28800 as Numeric (10, 2))) * 100) as Numeric (10, 2)) end) Utilization, '-' T, Left(K1.Machine,PATINDEX('%[0-9]%',K1.Machine)-1)Machine_Alpha, CONVERT (Int,SUBSTRING(K1.Machine,PATINDEX('%[0-9]%',K1.Machine),LEN(K1.Machine)))Machine_Num From Socks_Machine_Planning_Details S1 Left Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID Left Join VFit_Sample_Needle_Master V1 on S2.Needle_Id = V1.RowID Left Join Production_Qty_WeekWise () P1 on S1.Order_No = P1.Order_No And P1.Needle_ID = S2.Needle_Id and P1.Year = S2.Year and P1.Week = S2.Week Right Join Knitting_Mc_NO_UnitWise(2) K1 on S1.Machine_ID = K1.Machine_ID left join Socks_Machine_Production_Master S5 on S5.Machine_ID = S1.Machine_ID and S5.Order_No = S1.Order_No and S5.year = S2.Year and S5.Week = S2.Week and S5.Needle_ID = S2.Needle_Id and S5.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' and S5.Shift = " + CmbShift.Text + " Where S2.Year = " + TxtYear.Text + " and S2.Week = " + TxtWeek.Text + " and V1.Name = '" + CmbNeedle.Text.Trim() + "' And S1.RowID in (Select MAX(P2.RowID) from Socks_Machine_Planning_Master P1 Left Join Socks_Machine_Planning_Details P2 On P1.RowID = P2.Master_ID Left Join VFit_Sample_Needle_Master V1 On P1.Needle_Id = V1.RowID Where P1.Year = " + TxtYear.Text + " and P1.Week = " + TxtWeek.Text + " and V1.Name = '" + CmbNeedle.Text.Trim() + "' Group by P2.Machine_ID) Order by Left(K1.Machine,PATINDEX('%[0-9]%',K1.Machine)-1),CONVERT (Int,SUBSTRING(K1.Machine,PATINDEX('%[0-9]%',K1.Machine),LEN(K1.Machine)))", ref Dt);
                }
                else
                {
                    Grid.DataSource = MyBase.Load_Data("Select Distinct 0 as Slno, S1.Machine_ID, K1.Machine, S1.Order_No, S1.Plan_Qty, Isnull(P1.Prod_Qty, 0) Prod_Qty, Isnull(S5.Assign_Qty, 0) Assign_Qty, (Case When S5.Planned_Seconds is null then 0 else Cast(((S5.Planned_Seconds / Cast(28800 as Numeric (10, 2))) * 100) as Numeric (10, 2)) end) Utilization, '-' T, Left(K1.Machine,PATINDEX('%[0-9]%',K1.Machine)-1)Machine_Alpha, CONVERT (Int,SUBSTRING(K1.Machine,PATINDEX('%[0-9]%',K1.Machine),LEN(K1.Machine)))Machine_Num From Socks_Machine_Planning_Details S1 Left Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID Left Join VFit_Sample_Needle_Master V1 on S2.Needle_Id = V1.RowID Left Join Production_Qty_WeekWise () P1 on S1.Order_No = P1.Order_No And P1.Needle_ID = S2.Needle_Id and P1.Year = S2.Year and P1.Week = S2.Week Left Join Knitting_Mc_NO () K1 on S1.Machine_ID = K1.Machine_ID left join Socks_Machine_Production_Master S5 on S5.Machine_ID = S1.Machine_ID and S5.Order_No = S1.Order_No and S5.year = S2.Year and S5.Week = S2.Week and S5.Needle_ID = S2.Needle_Id and S5.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "' and S5.Shift = " + CmbShift.Text + " Where S2.Year = " + TxtYear.Text + " and S2.Week = " + TxtWeek.Text + " and V1.Name = '" + CmbNeedle.Text.Trim() + "' And S1.RowID in (Select MAX(P2.RowID) from Socks_Machine_Planning_Master P1 Left Join Socks_Machine_Planning_Details P2 On P1.RowID = P2.Master_ID Left Join VFit_Sample_Needle_Master V1 On P1.Needle_Id = V1.RowID Where P1.Year = " + TxtYear.Text + " and P1.Week = " + TxtWeek.Text + " and V1.Name = '" + CmbNeedle.Text.Trim() + "' Group by P2.Machine_ID) Order by Left(K1.Machine,PATINDEX('%[0-9]%',K1.Machine)-1),CONVERT (Int,SUBSTRING(K1.Machine,PATINDEX('%[0-9]%',K1.Machine),LEN(K1.Machine)))", ref Dt);
                }
                MyBase.Grid_Designing(ref Grid, ref Dt, "Machine_ID", "Machine_Alpha", "Machine_Num");
                MyBase.ReadOnly_Grid_Without(ref Grid, "T");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 60, 100, 150, 100, 100, 100, 100);

                Grid.Columns["Plan_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Prod_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Assign_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Utilization"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid.Columns["Assign_Qty"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                Grid.Columns["Utilization"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;


                Grid.RowHeadersWidth = 30;
                MyBase.Row_Number(ref Grid);                

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
                if (Grid.CurrentCell.ColumnIndex >= 2)
                {
                    FrmMachineProduction_OrderWise Frm = new FrmMachineProduction_OrderWise(DtpEDate.Value, Convert.ToInt16(CmbShift.Text), Convert.ToInt32(TxtYear.Text), Convert.ToInt32(TxtWeek.Text), CmbNeedle.Text, Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Convert.ToInt32(Grid["Plan_Qty", Grid.CurrentCell.RowIndex].Value), Grid.CurrentCell.RowIndex, Grid.CurrentCell.ColumnIndex);
                    Frm.StartPosition = FormStartPosition.Manual;
                    Frm.Left = 150;
                    Frm.Top = 150;
                    Frm.ShowDialog();
                    Grid["Utilization", Grid.CurrentCell.RowIndex].Value = String.Format("{0:0.00}", Frm.Utilization);
                    Grid["Assign_Qty", Grid.CurrentCell.RowIndex].Value = Frm.Assign_Qty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChkImport_CheckedChanged(object sender, EventArgs e)
        {
            if(ChkImport.Checked == true)
            {
                groupBox2.Visible = true;
            }
            else if (ChkImport.Checked == false)
            {
                groupBox2.Visible = false;
            }
        }

        private void BtnImportCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DtpFDate.Value = MyBase.GetServerDateTime();
                DtpTDate.Value = MyBase.GetServerDateTime();
                FillFShift_Combo();
                FillTShift_Combo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnImportExit_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox2.Visible = false;
                ChkImport.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnImportOk_Click(object sender, EventArgs e)
        {
            try 
            {
                if(CmbFShift.Text.ToString() != String.Empty && CmbFNeedle.Text.ToString() != String.Empty && CmbTShift.Text.ToString() != String.Empty && CmbTNeedle.Text.ToString() != String.Empty)
                {
                    if (CmbFNeedle.Text.ToString() == CmbTNeedle.Text.ToString())
                    {

                        DataTable Tdt = new DataTable();
                        String Str2;

                        Str2 = "Select * from Socks_Machine_Production_Master S1 Left Join Socks_Machine_Production_Details S2 On S1.RowId = S2.Master_ID ";
                        Str2 = Str2 + " Left Join Get_Week_Details() W1 On S1.Entry_Date Between W1.Week_SDate And W1.Week_EDate ";
                        Str2 = Str2 + " Left Join VFit_Sample_Needle_Master N1 On S1.Needle_ID = N1.RowID ";
                        if (MyParent.UserCode == 7)
                        {
                            Str2 = Str2 + " Right Join Knitting_Mc_NO_UnitWise(1) K1 On S1.Machine_ID = K1.Machine_ID ";
                        }
                        else if (MyParent.UserCode == 40)
                        {
                            Str2 = Str2 + " Right Join Knitting_Mc_NO_UnitWise(2) K1 On S1.Machine_ID = K1.Machine_ID ";
                        }
                        else
                        {
 
                        }
                        Str2 = Str2 + " Where S1.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and S1.Shift = " + CmbFShift.Text + " And N1.Name = '" + CmbFNeedle.Text + "' And S1.Year = Year('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "') And S1.Week = DATEPART(WEEK,'" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "') ";
                        MyBase.Load_Data(Str2, ref Tdt);

                        if (Tdt.Rows.Count > 0)
                        {
                            Tdt = new DataTable();
                            Str2 = "Select * from Socks_Machine_Production_Master S1 Left Join Socks_Machine_Production_Details S2 On S1.RowId = S2.Master_ID ";
                            Str2 = Str2 + " Left Join Get_Week_Details() W1 On S1.Entry_Date Between W1.Week_SDate And W1.Week_EDate ";
                            Str2 = Str2 + " Left Join VFit_Sample_Needle_Master N1 On S1.Needle_ID = N1.RowID ";
                            if (MyParent.UserCode == 7)
                            {
                                Str2 = Str2 + " Right Join Knitting_Mc_NO_UnitWise(1) K1 On S1.Machine_ID = K1.Machine_ID ";
                            }
                            else if (MyParent.UserCode == 40)
                            {
                                Str2 = Str2 + " Right Join Knitting_Mc_NO_UnitWise(2) K1 On S1.Machine_ID = K1.Machine_ID ";
                            }
                            else
                            {

                            }
                            Str2 = Str2 + " Where S1.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' and S1.Shift = " + CmbTShift.Text + " And N1.Name = '" + CmbTNeedle.Text + "' And S1.Year = Year('" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "') And S1.Week = DATEPART(WEEK,'" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "') ";
                            MyBase.Load_Data(Str2, ref Tdt);

                            DataTable Tdt3 = new DataTable();
                            MyBase.Load_Data(" Select Year('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "')FYear ", ref Tdt3);

                            DataTable Tdt4 = new DataTable();
                            MyBase.Load_Data(" Select DatePart(Week,'" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "')FWeek ", ref Tdt4);

                            DataTable Tdt5 = new DataTable();
                            MyBase.Load_Data(" Select Year('" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "')TYear ", ref Tdt5);

                            DataTable Tdt6 = new DataTable();
                            MyBase.Load_Data(" Select DatePart(Week,'" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "')TWeek ", ref Tdt6);

                            if (Tdt.Rows.Count <= 0)
                            {
                                //FrmMachineProduction_Import Frm = new FrmMachineProduction_Import(DtpFDate.Value, DtpTDate.Value, Convert.ToInt16(CmbFShift.Text), Convert.ToInt16(CmbTShift.Text), CmbFNeedle.Text, CmbTNeedle.Text, Convert.ToInt32(TxtYear.Text), Convert.ToInt32(TxtWeek.Text));

                                Form Frm;

                                Int32 Unit = 0;
                                if (MyParent.UserCode == 7)
                                {
                                    Unit = 1;
                                }
                                else if (MyParent.UserCode == 40)
                                {
                                    Unit = 2;
                                }
                                if (Unit > 0)
                                {
                                    Frm = new FrmMachineProduction_Import_Unitwise(DtpFDate.Value, DtpTDate.Value, Convert.ToInt16(CmbFShift.Text), Convert.ToInt16(CmbTShift.Text), CmbFNeedle.Text, CmbTNeedle.Text, Convert.ToInt32(Tdt3.Rows[0][0].ToString()), Convert.ToInt32(Tdt5.Rows[0][0].ToString()), Convert.ToInt32(Tdt4.Rows[0][0].ToString()), Convert.ToInt32(Tdt6.Rows[0][0].ToString()),Convert.ToInt32(Unit.ToString()));
                                }
                                else
                                {
                                    Frm = new FrmMachineProduction_Import(DtpFDate.Value, DtpTDate.Value, Convert.ToInt16(CmbFShift.Text), Convert.ToInt16(CmbTShift.Text), CmbFNeedle.Text, CmbTNeedle.Text, Convert.ToInt32(Tdt3.Rows[0][0].ToString()), Convert.ToInt32(Tdt5.Rows[0][0].ToString()), Convert.ToInt32(Tdt4.Rows[0][0].ToString()), Convert.ToInt32(Tdt6.Rows[0][0].ToString()));
                                }
                                
                                Frm.StartPosition = FormStartPosition.Manual;
                                Frm.Left = 150;
                                Frm.Top = 150;
                                Frm.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("Data Alreaty Stored For The To Date:'" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' And Shift: " + CmbTShift.Text + " ");
                                DtpTDate.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Data Not Available For The From Date:'" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' And Shift: " + CmbFShift.Text + " ");
                            DtpFDate.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Pls Select Same Needle....!Gainup");
                        CmbTNeedle.Focus();
                    }
                }
                else if(CmbFShift.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Select From Shift...!Gainup");
                    CmbFShift.Focus();  
                }
                else if(CmbFNeedle.Text.ToString() == String.Empty )
                {
                    MessageBox.Show("Select From Needle...!Gainup");
                    CmbFNeedle.Focus();  
                }
                else if(CmbTShift.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Select TO Shift...!Gainup");
                    CmbTShift.Focus();  
                }
                else if(CmbFNeedle.Text.ToString() == String.Empty )
                {
                    MessageBox.Show("Select TO Needle...!Gainup");
                    CmbTNeedle.Focus();  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbFShift_Leave(object sender, EventArgs e)
        {
            try
            {
                DataTable Tdt = new DataTable();
                if (CmbFShift.Text.ToString() != String.Empty)
                {
                    CmbFNeedle.Items.Clear();
                    String Str;

                    if (MyParent.UserCode == 7)
                    {
                        Str = " Select Distinct Needle_ID, Name from Socks_Machine_Production_Master S1 Left JOIn Socks_Machine_Production_Details S2 On S1.RowId = S2.Master_ID Left Join VFit_Sample_Needle_Master N1 On S1.Needle_ID = N1.RowID Right Join Knitting_Mc_NO_UnitWise(1) K1 On S1.Machine_ID = K1.Machine_ID Where S1.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' And S1.Shift = " + CmbFShift.Text + " Order By Name";
                    }
                    else if (MyParent.UserCode == 40)
                    {
                        Str = " Select Distinct Needle_ID, Name from Socks_Machine_Production_Master S1 Left JOIn Socks_Machine_Production_Details S2 On S1.RowId = S2.Master_ID Left Join VFit_Sample_Needle_Master N1 On S1.Needle_ID = N1.RowID Right Join Knitting_Mc_NO_UnitWise(2) K1 On S1.Machine_ID = K1.Machine_ID Where S1.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' And S1.Shift = " + CmbFShift.Text + " Order By Name";
                    }
                    else
                    {
                        Str = " Select Distinct Needle_ID, Name from Socks_Machine_Production_Master S1 Left JOIn Socks_Machine_Production_Details S2 On S1.RowId = S2.Master_ID Left Join VFit_Sample_Needle_Master N1 On S1.Needle_ID = N1.RowID Where S1.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' And S1.Shift = " + CmbFShift.Text + " Order By Name";
                    }
                    
                    MyBase.Load_Data(Str, ref Tdt);
                    for (int i = 0; i <= Tdt.Rows.Count - 1; i++)
                    {
                        CmbFNeedle.Items.Add(Tdt.Rows[i]["Name"].ToString());
                    }
                }
                //else
                //{
                //    //MessageBox.Show("Select From Date Shift...!Gainup");
                //    CmbFShift.Focus();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DtpFDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DataTable Tdt = new DataTable();
                if (CmbFShift.Text.ToString() != String.Empty)
                {
                    CmbFNeedle.Items.Clear();
                    String Str;
                    if (MyParent.UserCode == 7)
                    {
                        Str = "Select Distinct Needle_ID, Name from Socks_Machine_Production_Master S1 Left JOIn Socks_Machine_Production_Details S2 On S1.RowId = S2.Master_ID Left Join VFit_Sample_Needle_Master N1 On S1.Needle_ID = N1.RowID Right Join Knitting_Mc_NO_UnitWise(1) K1 On S1.Machine_ID = K1.Machine_ID Where S1.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' And S1.Shift = " + CmbFShift.Text + " Order By Name";
                    }
                    else if (MyParent.UserCode == 40)
                    {
                        Str = "Select Distinct Needle_ID, Name from Socks_Machine_Production_Master S1 Left JOIn Socks_Machine_Production_Details S2 On S1.RowId = S2.Master_ID Left Join VFit_Sample_Needle_Master N1 On S1.Needle_ID = N1.RowID Right Join Knitting_Mc_NO_UnitWise(2) K1 On S1.Machine_ID = K1.Machine_ID Where S1.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' And S1.Shift = " + CmbFShift.Text + " Order By Name";
                    }
                    else
                    {
                        Str = "Select Distinct Needle_ID, Name from Socks_Machine_Production_Master S1 Left JOIn Socks_Machine_Production_Details S2 On S1.RowId = S2.Master_ID Left Join VFit_Sample_Needle_Master N1 On S1.Needle_ID = N1.RowID Where S1.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' And S1.Shift = " + CmbFShift.Text + " Order By Name";
                    }
                    
                    MyBase.Load_Data(Str, ref Tdt);
                    for (int i = 0; i <= Tdt.Rows.Count - 1; i++)
                    {
                        CmbFNeedle.Items.Add(Tdt.Rows[i]["Name"].ToString());
                    }
                }
                //else
                //{
                //    CmbFShift.Focus();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbTShift_Leave(object sender, EventArgs e)
        {
            try
            {
                String Str1;
                DataTable Tdt = new DataTable();
                if (CmbTShift.Text.ToString() != String.Empty)
                {
                    CmbTNeedle.Items.Clear();
                    Str1 = " Select Distinct S1.Needle_ID, N1.Name From Get_Week_Details() G1 LEft Join Socks_Machine_Planning_Master S1 On G1.Week = S1.Week And G1.Year = S1.Year Left Join VFit_Sample_Needle_Master N1 On S1.Needle_Id = N1.RowID Where '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' Between Week_SDate And Week_EDate  And Needle_ID In ";
                    Str1 = Str1 + " (Select Distinct Needle_ID from Socks_Machine_Production_Master S1 Left JOIn Socks_Machine_Production_Details S2 On S1.RowId = S2.Master_ID Left Join VFit_Sample_Needle_Master N1 On S1.Needle_ID = N1.RowID ";
                    if (MyParent.UserCode == 7)
                    {
                        Str1 = Str1 + " Right Join Knitting_Mc_NO_UnitWise(1) K1 On S1.Machine_ID = K1.Machine_ID";
                    }
                    else if (MyParent.UserCode == 40)
                    {
                        Str1 = Str1 + " Right Join Knitting_Mc_NO_UnitWise(2) K1 On S1.Machine_ID = K1.Machine_ID";
                    }
                    Str1 = Str1 + " Where S1.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' And S1.Shift = " + CmbFShift.Text + " )Order By N1.Name";
                    MyBase.Load_Data(Str1, ref Tdt);
                    for (int i = 0; i <= Tdt.Rows.Count - 1; i++)
                    {
                        CmbTNeedle.Items.Add(Tdt.Rows[i]["Name"].ToString());
                    }
                }
                //else
                //{
                //    //MessageBox.Show("Select To Date Shift...!Gainup");
                //    CmbTShift.Focus();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DtpTDate_Leave(object sender, EventArgs e)
        {
            try
            {
                String Str1;
                DataTable Tdt = new DataTable();
                if (CmbTShift.Text.ToString() != String.Empty)
                {
                    CmbTNeedle.Items.Clear();
                    Str1 = " Select Distinct S1.Needle_ID, N1.Name From Get_Week_Details() G1 LEft Join Socks_Machine_Planning_Master S1 On G1.Week = S1.Week And G1.Year = S1.Year Left Join VFit_Sample_Needle_Master N1 On S1.Needle_Id = N1.RowID Where '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' Between Week_SDate And Week_EDate  And Needle_ID In ";
                    Str1 = Str1 + " (Select Distinct Needle_ID from Socks_Machine_Production_Master S1 Left JOIn Socks_Machine_Production_Details S2 On S1.RowId = S2.Master_ID Left Join VFit_Sample_Needle_Master N1 On S1.Needle_ID = N1.RowID ";
                    if (MyParent.UserCode == 7)
                    {
                        Str1 = Str1 + " Right Join Knitting_Mc_NO_UnitWise(1) K1 On S1.Machine_ID = K1.Machine_ID";
                    }
                    else if (MyParent.UserCode == 40)
                    {
                        Str1 = Str1 + " Right Join Knitting_Mc_NO_UnitWise(2) K1 On S1.Machine_ID = K1.Machine_ID";
                    }
                    Str1 = Str1 + " Where S1.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' And S1.Shift = " + CmbFShift.Text + " )Order By N1.Name";
                    MyBase.Load_Data(Str1, ref Tdt);
                    for (int i = 0; i <= Tdt.Rows.Count - 1; i++)
                    {
                        CmbTNeedle.Items.Add(Tdt.Rows[i]["Name"].ToString());
                    }
                }
                //else
                //{
                //    CmbTShift.Focus();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbFShift_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyValue  == 9)
                {
                    
                        if (Grid.CurrentCell.ColumnIndex >= 2)
                        {
                            FrmMachineProduction_OrderWise Frm = new FrmMachineProduction_OrderWise(DtpEDate.Value, Convert.ToInt16(CmbShift.Text), Convert.ToInt32(TxtYear.Text), Convert.ToInt32(TxtWeek.Text), CmbNeedle.Text, Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Convert.ToInt32(Grid["Plan_Qty", Grid.CurrentCell.RowIndex].Value), Grid.CurrentCell.RowIndex, Grid.CurrentCell.ColumnIndex);
                            Frm.StartPosition = FormStartPosition.Manual;
                            Frm.Left = 150;
                            Frm.Top = 150;
                            Frm.ShowDialog();
                            Grid["Utilization", Grid.CurrentCell.RowIndex].Value = String.Format("{0:0.00}", Frm.Utilization);
                            Grid["Assign_Qty", Grid.CurrentCell.RowIndex].Value = Frm.Assign_Qty;
                        }
                    
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}