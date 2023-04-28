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
    public partial class FrmMachinePLanningWeek : Form
    {
        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        Int32 Grid_Row = 0;
        Int32 Grid_Col = 0;
        DataTable Dt = new DataTable();
        public Double Utilization = 0;
        TextBox Txt = null;
        Int32 Total_Machines = 0;

        public FrmMachinePLanningWeek(Int32 Year, Int32 Week, String Needle, String Efficiency, Int32 Row, Int32 Col)
        {
            InitializeComponent();
            MyBase.Clear(this);
            TxtYear.Text = Year.ToString();
            TxtWeek.Text = Week.ToString();
            TxtNeedle.Text = Needle;
            TxtNeedle.Tag = Needle_ID (Needle);
            LblEfficiency.Text = "0";
            Total_Machines = Total_Machines_For_Needle();
            Grid_Row = Row;
            Grid_Col = Col;
        }

        Double Total_Planned_Mins()
        {
            try
            {
                return Convert.ToDouble(MyBase.Sum(ref Grid, "Plan_Mins", "Machine_ID", "Order_No"));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        Int32 Needle_ID(String NeedleName)
        {
            try
            {
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select RowID from VFit_Sample_Needle_Master Where Name = '" + NeedleName + "'", ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    return Convert.ToInt32 (Tdt.Rows[0]["RowID"]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        void Grid_Data()
        {
            try
            {
                //Grid.DataSource = MyBase.Load_Data("Select 0 as Sl, S1.RowID, S1.Machine_ID, K1.Machine, S1.Order_No, S3.Ship_Date, S3.Bom_Qty Bom, S3.Bal_Qty Req_Production, Convert(Varchar, cast(S1.Cycle_Time as Datetime), 108) CyTime, S1.Cycle_Seconds CySecd, S1.Target_Qty Target, S1.Plan_Qty [Plan], S1.Plan_Mins From Socks_Machine_Planning_Details S1 Inner Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID Left Join (Select A.Order_No, A.Needle_ID, A.Ship_Date, Sum(A.Bom_Qty) Bom_Qty, isnull(B.Production,0) Production, (Sum(A.Bom_Qty) - isnull(B.Production,0)) Bal_Qty From Socks_Bom () A Left Join Get_Knit_Prod_Needle() B on A.Order_No=B.Order_No and A.Needle=B.Needle Group By A.Order_No, A.Needle_ID, A.Ship_Date, B.Production) S3 on S1.Order_No = S3.Order_No  and S3.Needle_ID = S2.Needle_Id Left join Knitting_Mc_NO () K1 On S1.Machine_ID = K1.Machine_ID Where S2.Year = " + TxtYear.Text + " and S2.Week = " + TxtWeek.Text + " and S2.Needle_ID = " + TxtNeedle.Tag.ToString() + " Group By S1.Machine_ID, S1.Order_No, S1.Cycle_Time, K1.Machine, S1.Target_Qty, S1.Plan_Qty, S1.Cycle_Seconds, S3.Bom_Qty, S3.Bal_Qty, S3.Ship_Date, S1.RowID, S1.Plan_Mins Order By S1.RowID ", ref Dt);

                this.Cursor = Cursors.WaitCursor;

                DataTable Tmpdt = new DataTable();
                //String Str = "Select 0 as Sl, S1.RowID, S1.Machine_ID, K1.Machine, S1.Order_No, S3.Ship_Date, S3.Bom_Qty Bom, S3.Bal_Qty Req_Production, Convert(Varchar, cast(S1.Cycle_Time as Datetime), 108) CyTime, S1.Cycle_Seconds CySecd, S1.Target_Qty Target, S1.Plan_Qty [Plan], S1.Plan_Mins From Socks_Machine_Planning_Details S1 Inner Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID Left Join (Select A.Order_No, A.Needle_ID, A.Ship_Date, Sum(A.Bom_Qty) Bom_Qty, Sum(isnull(B.Production,0)) Production, (Sum(A.Bom_Qty) - Sum(isnull(B.Production,0))) Bal_Qty From Socks_Bom () A 	Left Join (Select Order_No, NeedleID Needle, Sum(Production) Production from Floor_Knitting_Details Group by Order_No, NeedleID) B on A.Order_No=B.Order_No and A.Needle=B.Needle	Group By A.Order_No, A.Needle_ID, A.Ship_Date ) S3 on S1.Order_No = S3.Order_No  and S3.Needle_ID = S2.Needle_Id Left join Knitting_Mc_NO () K1 On S1.Machine_ID = K1.Machine_ID Where S2.Year = " + TxtYear.Text + " and S2.Week = " + TxtWeek.Text + " and S2.Needle_ID = " + TxtNeedle.Tag.ToString() + " Group By S1.Machine_ID, S1.Order_No, S1.Cycle_Time, K1.Machine, S1.Target_Qty, S1.Plan_Qty, S1.Cycle_Seconds, S3.Bom_Qty, S3.Bal_Qty, S3.Ship_Date, S1.RowID, S1.Plan_Mins Order By S1.RowID ";

                String Str = "Select 0 as Sl, S1.RowID, S1.Machine_ID, K1.Machine, S1.Order_No, S3.Ship_Date, S3.Bom_Qty Bom, S3.Bal_Qty Req_Production, Convert(Varchar, cast(S1.Cycle_Time as Datetime), 108) CyTime, S1.Cycle_Seconds CySecd, S1.Target_Qty Target, S1.Plan_Qty [Plan], S1.Plan_Mins From Socks_Machine_Planning_Details S1 Inner Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID Left Join (Select A.Order_No, A.Needle_ID, A.Ship_Date, Sum(A.Bom_Qty) Bom_Qty, Sum(isnull(B.Production,0)) Production, (Sum(A.Bom_Qty) - Sum(isnull(B.Production,0))) Bal_Qty From Socks_Bom_Planner () A Left Join (Select Order_No, OrderColorID, Sum(Production) Production from Floor_Knitting_Details Group by Order_No, OrderColorID) B on A.Order_No = B.Order_No And A.OrderColorID = B.OrderColorID Where A.Needle_ID = " + TxtNeedle.Tag.ToString() + " Group By A.Order_No, A.Needle_ID, A.Ship_Date) S3 on S1.Order_No = S3.Order_No  and S3.Needle_ID = S2.Needle_Id Left join Knitting_Mc_NO () K1 On S1.Machine_ID = K1.Machine_ID Where S2.Year = " + TxtYear.Text + " and S2.Week = " + TxtWeek.Text + " and S2.Needle_ID = " + TxtNeedle.Tag.ToString() + " Group By S1.Machine_ID, S1.Order_No, S1.Cycle_Time, K1.Machine, S1.Target_Qty, S1.Plan_Qty, S1.Cycle_Seconds, S3.Bom_Qty, S3.Bal_Qty, S3.Ship_Date, S1.RowID, S1.Plan_Mins Order By S1.RowID ";
                MyBase.Load_Data(Str, ref Tmpdt);

                if (Tmpdt.Rows.Count > 0)
                {
                    //Comment By Sakthi On 24-May-2016 Due to Cycle Time Not change By Planner
 
                    //DataTable Tmpdt1 = new DataTable();
                    //Str = "select A.*, B.Order_No, B.Needle, B.Needle_ID from Get_Max_Cycle_Time() A Left Join Socks_Bom() B on A.OrderColorID=B.OrderColorId Left join (Select Distinct Order_No,Cycle_Time,Cycle_Seconds,Needle_Id From Socks_Machine_Planning_Details S1 Inner Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID Where S2.Year = " + TxtYear.Text + " and S2.Week = " + TxtWeek.Text + " and S2.Needle_ID = " + TxtNeedle.Tag.ToString() + " ) C on B.Order_No=C.Order_No and B.Needle_ID=C.Needle_ID where " + TxtWeek.Text + " >= datepart(WEEK,EffectFrom) and B.Needle_ID=" + TxtNeedle.Tag.ToString() + " and B.Order_No in(Select Distinct Order_No From Socks_Machine_Planning_Details S1 Inner Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID Where S2.Year = " + TxtYear.Text + " and S2.Week = " + TxtWeek.Text + " and S2.Needle_ID = " + TxtNeedle.Tag.ToString() + ") and A.Cycle_Seconds<>C.Cycle_Seconds";
                    //MyBase.Load_Data(Str,ref Tmpdt1);
                    //if (Tmpdt1.Rows.Count > 0)
                    //{
                    //    if (MessageBox.Show("Cycle Time Changed,Do you want to Update ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //    {
                    //        Grid.DataSource = MyBase.Load_Data("Select 0 as Sl, S1.RowID, S1.Machine_ID, K1.Machine, S1.Order_No, S3.Ship_Date, S3.Bom_Qty Bom, S3.Bal_Qty Req_Production, isnull(S4.Cycle_Time,Convert(Varchar, cast(S1.Cycle_Time as Datetime), 108)) CyTime, isnull(S4.Cycle_Seconds,S1.Cycle_Seconds) CySecd, S1.Target_Qty Target, S1.Plan_Qty [Plan], (isnull(S4.Cycle_Seconds,S1.Cycle_Seconds) * S1.Plan_Qty) Plan_Mins From Socks_Machine_Planning_Details S1 Inner Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID Left Join (Select A.Order_No, A.Needle_ID, A.Ship_Date, Sum(A.Bom_Qty) Bom_Qty, Sum(isnull(B.Production,0)) Production, (Sum(A.Bom_Qty) - Sum(isnull(B.Production,0))) Bal_Qty From Socks_Bom () A Left Join (Select Order_No, OrderColorID, NeedleID Needle, Sum(Production) Production from Floor_Knitting_Details Group by Order_No, OrderColorID, NeedleID) B on A.Order_No = B.Order_No And A.OrderColorId = B.OrderColorID and A.Needle = B.Needle Group By A.Order_No, A.Needle_ID, A.Ship_Date ) S3 on S1.Order_No = S3.Order_No and S3.Needle_ID = S2.Needle_Id Left join Knitting_Mc_NO () K1 On S1.Machine_ID = K1.Machine_ID Left join (select A.*,B.Order_No,B.Needle,B.Needle_ID from Get_Max_Cycle_Time() A Left Join Socks_Bom() B on A.OrderColorID=B.OrderColorId where " + TxtWeek.Text + " >= datepart(WEEK,EffectFrom) and Needle_ID=" + TxtNeedle.Tag.ToString() + " ) S4 on S1.Order_No=S4.Order_No Where S2.Year = " + TxtYear.Text + " and S2.Week = " + TxtWeek.Text + " and S2.Needle_ID = " + TxtNeedle.Tag.ToString() + "  Group By S1.Machine_ID, S1.Order_No,S1.Cycle_Time, K1.Machine, S1.Target_Qty, S1.Plan_Qty, S1.Cycle_Seconds, S3.Bom_Qty, S3.Bal_Qty, S3.Ship_Date, S1.RowID, S1.Plan_Mins , S4.Cycle_Time, S4.Cycle_Seconds Order By S1.RowID ", ref Dt);
                    //    }
                    //    else
                    //    {
                    //        Grid.DataSource = MyBase.Load_Data("Select 0 as Sl, S1.RowID, S1.Machine_ID, K1.Machine, S1.Order_No, S3.Ship_Date, S3.Bom_Qty Bom, S3.Bal_Qty Req_Production, Convert(Varchar, cast(S1.Cycle_Time as Datetime), 108) CyTime, S1.Cycle_Seconds CySecd, S1.Target_Qty Target, S1.Plan_Qty [Plan], S1.Plan_Mins From Socks_Machine_Planning_Details S1 Inner Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID Left Join (Select A.Order_No, A.Needle_ID, A.Ship_Date, Sum(A.Bom_Qty) Bom_Qty, Sum(isnull(B.Production,0)) Production, (Sum(A.Bom_Qty) - Sum(isnull(B.Production,0))) Bal_Qty From Socks_Bom () A 	Left Join (Select Order_No, OrderColorID, NeedleID Needle, Sum(Production) Production from Floor_Knitting_Details Group by Order_No, OrderColorID, NeedleID) B on A.Order_No = B.Order_No And A.OrderColorId = B.OrderColorID and A.Needle = B.Needle Group By A.Order_No, A.Needle_ID, A.Ship_Date ) S3 on S1.Order_No = S3.Order_No  and S3.Needle_ID = S2.Needle_Id Left join Knitting_Mc_NO () K1 On S1.Machine_ID = K1.Machine_ID Where S2.Year = " + TxtYear.Text + " and S2.Week = " + TxtWeek.Text + " and S2.Needle_ID = " + TxtNeedle.Tag.ToString() + " Group By S1.Machine_ID, S1.Order_No, S1.Cycle_Time, K1.Machine, S1.Target_Qty, S1.Plan_Qty, S1.Cycle_Seconds, S3.Bom_Qty, S3.Bal_Qty, S3.Ship_Date, S1.RowID, S1.Plan_Mins Order By S1.RowID ", ref Dt);
                    //    }
                    //}
                    //else
                    //{
                        //Grid.DataSource = MyBase.Load_Data("Select 0 as Sl, S1.RowID, S1.Machine_ID, K1.Machine, S1.Order_No, S3.Ship_Date, S3.Bom_Qty Bom, S3.Bal_Qty Req_Production, Convert(Varchar, cast(S1.Cycle_Time as Datetime), 108) CyTime, S1.Cycle_Seconds CySecd, S1.Target_Qty Target, S1.Plan_Qty [Plan], S1.Plan_Mins From Socks_Machine_Planning_Details S1 Inner Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID Left Join (Select A.Order_No, A.Needle_ID, A.Ship_Date, Sum(A.Bom_Qty) Bom_Qty, Sum(isnull(B.Production,0)) Production, (Sum(A.Bom_Qty) - Sum(isnull(B.Production,0))) Bal_Qty From Socks_Bom () A 	Left Join (Select Order_No, OrderColorID, NeedleID Needle, Sum(Production) Production from Floor_Knitting_Details Group by Order_No, OrderColorID, NeedleID) B on A.Order_No = B.Order_No And A.OrderColorId = B.OrderColorID and A.Needle = B.Needle Group By A.Order_No, A.Needle_ID, A.Ship_Date ) S3 on S1.Order_No = S3.Order_No and S3.Needle_ID = S2.Needle_Id Left join Knitting_Mc_NO () K1 On S1.Machine_ID = K1.Machine_ID Where S2.Year = " + TxtYear.Text + " and S2.Week = " + TxtWeek.Text + " and S2.Needle_ID = " + TxtNeedle.Tag.ToString() + " Group By S1.Machine_ID, S1.Order_No, S1.Cycle_Time, K1.Machine, S1.Target_Qty, S1.Plan_Qty, S1.Cycle_Seconds, S3.Bom_Qty, S3.Bal_Qty, S3.Ship_Date, S1.RowID, S1.Plan_Mins Order By S1.RowID ", ref Dt);
                    //}
                    Str = "Select 0 as Sl, S1.RowID, S1.Machine_ID, K1.Machine, S1.Order_No, S3.Ship_Date, S3.Bom_Qty Bom, S3.Bal_Qty Req_Production, ";
                    Str = Str + " Convert(Varchar, cast(S1.Cycle_Time as Datetime), 108) CyTime, S1.Cycle_Seconds CySecd, S1.Target_Qty Target, S1.Plan_Qty [Plan], ";
                    Str = Str + " S1.Plan_Mins From Socks_Machine_Planning_Details S1 Inner Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID ";
                    Str = Str + " Left Join (Select A.Order_No, A.Needle_ID, A.Ship_Date, Sum(A.Bom_Qty) Bom_Qty, Sum(isnull(B.Production,0)) Production, (Sum(A.Bom_Qty) - Sum(isnull(B.Production,0))) Bal_Qty From Socks_Bom_Planner () A ";
                    Str = Str + " Left Join (Select Order_No, OrderColorID, NeedleID Needle, Sum(Production) Production from Floor_Knitting_Details Group by Order_No, OrderColorID, NeedleID) B on A.Order_No = B.Order_No And A.OrderColorId = B.OrderColorID and A.Needle = B.Needle ";
                    Str = Str + " Group By A.Order_No, A.Needle_ID, A.Ship_Date ) S3 on S1.Order_No = S3.Order_No and S3.Needle_ID = S2.Needle_Id  ";
                    Str = Str + " Left join Knitting_Mc_NO () K1 On S1.Machine_ID = K1.Machine_ID Where S2.Year = " + TxtYear.Text + " and S2.Week = " + TxtWeek.Text + " and S2.Needle_ID = " + TxtNeedle.Tag.ToString() + " Group By S1.Machine_ID, S1.Order_No, S1.Cycle_Time, K1.Machine, S1.Target_Qty, S1.Plan_Qty, S1.Cycle_Seconds, S3.Bom_Qty, S3.Bal_Qty, S3.Ship_Date, S1.RowID, S1.Plan_Mins ORDER BY LEFT(K1.Machine, PATINDEX('%[0-9]%', K1.Machine)-1), CONVERT(INT, SUBSTRING(K1.Machine, PATINDEX('%[0-9]%', K1.Machine), LEN(K1.Machine)))"; 
                    Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                }
                else
                {
                    Grid.DataSource = MyBase.Load_Data("Select 0 as Sl, S1.RowID, S1.Machine_ID, K1.Machine, S1.Order_No, S3.Ship_Date, S3.Bom_Qty Bom, S3.Bal_Qty Req_Production, Convert(Varchar, cast(S1.Cycle_Time as Datetime), 108) CyTime, S1.Cycle_Seconds CySecd, S1.Target_Qty Target, S1.Plan_Qty [Plan], S1.Plan_Mins From Socks_Machine_Planning_Details S1 Inner Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID Left Join (Select A.Order_No, A.Needle_ID, A.Ship_Date, Sum(A.Bom_Qty) Bom_Qty, Sum(isnull(B.Production,0)) Production, (Sum(A.Bom_Qty) - Sum(isnull(B.Production,0))) Bal_Qty From Socks_Bom_Planner () A 	Left Join (Select Order_No, OrderColorID, NeedleID Needle, Sum(Production) Production from Floor_Knitting_Details Group by Order_No, OrderColorID, NeedleID) B on A.Order_No = B.Order_No And A.OrderColorId = B.OrderColorID and A.Needle = B.Needle Group By A.Order_No, A.Needle_ID, A.Ship_Date ) S3 on S1.Order_No = S3.Order_No  and S3.Needle_ID = S2.Needle_Id Left join Knitting_Mc_NO () K1 On S1.Machine_ID = K1.Machine_ID Where S2.Year = " + TxtYear.Text + " and S2.Week = " + TxtWeek.Text + " and S2.Needle_ID = " + TxtNeedle.Tag.ToString() + " Group By S1.Machine_ID, S1.Order_No, S1.Cycle_Time, K1.Machine, S1.Target_Qty, S1.Plan_Qty, S1.Cycle_Seconds, S3.Bom_Qty, S3.Bal_Qty, S3.Ship_Date, S1.RowID, S1.Plan_Mins Order By S1.RowID ", ref Dt);
                }

                MyBase.Grid_Designing(ref Grid, ref Dt, "Machine_ID", "RowID", "CySecd", "Plan_Mins");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Machine", "Order_No", "Plan");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 40, 80, 120, 100, 80, 120, 100, 100);


                Grid.Columns["Bom"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Req_Production"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["CySecd"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["CyTime"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Target"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Plan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Plan_Mins"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;



                MyBase.Row_Number(ref Grid);

                Calculate_Utilization();

                Grid.RowHeadersWidth = 20;

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        Int32 Total_Machines_For_Needle()
        {
            try
            {
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select Count(*) From Get_Needle_List (" + TxtYear.Text + ", " + TxtWeek.Text + ") where Needle = '" + TxtNeedle.Text + "'", ref Tdt);
                return Convert.ToInt32(Tdt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        private void FrmMachinePLanningWeek_Load(object sender, EventArgs e)
        {
            DataTable Tdt = new DataTable();
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                MyBase.Load_Data("Select Week_SDate, Week_EDate From Get_Week_Details () Where Week = " + TxtWeek.Text + " and Year = " + TxtYear.Text, ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    LblWeekDays.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Tdt.Rows[0]["Week_SDate"])) + " - " + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Tdt.Rows[0]["Week_EDate"]));
                    DtpEDate.Value = Convert.ToDateTime(Tdt.Rows[0]["Week_EDate"]);
                }

                this.Text = this.Text + " NO OF MACHINES :" + Total_Machines.ToString();

                Dt = new DataTable();
                Grid.DataSource = null;



                Grid_Data();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Calculate_Utilization()
        {
            try
            {
                LblEfficiency.Text = Total_Planned_Mins().ToString();
                if (LblEfficiency.Text == String.Empty || Convert.ToDouble(LblEfficiency.Text) == 0)
                {
                    Utilization = 0;
                }
                else
                {
                    Utilization = (Convert.ToDouble(LblEfficiency.Text) / (Convert.ToDouble(LblTotalMins.Text) * Total_Machines)) * 100;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Sure to Close ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Calculate_Utilization();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmMachinePLanningWeek_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtNeedle")
                    {
                        Grid.CurrentCell = Grid["Machine", 0];
                        Grid.Focus();
                        Grid.BeginEdit (true);
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmMachinePLanningWeek_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == String.Empty)
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

        private void LblEfficiency_Click(object sender, EventArgs e)
        {

        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            String[] Queries;
            Int32 Array_Index = 0;
            Int64 Master_ID = 0;
            try
            {
                DataTable TRes = new DataTable();
                MyBase.Load_Data("Select (Case When Datepart(Week,Getdate()) != 53 And " + TxtWeek.Text + " < Datepart(Week,Getdate()) Then 1 Else 2 End)Res", ref TRes);
                if (Convert.ToInt16(TRes.Rows[0]["Res"].ToString()) != 1)
                {
                    if (MessageBox.Show("Sure to Save ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    if (Dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Invalid Details ..!", "Gainup");
                        return;
                    }


                    DataTable Tdt = new DataTable();
                    MyBase.Load_Data("Select RowId From Socks_Machine_Planning_Master where Year = " + TxtYear.Text + " and Week = " + TxtWeek.Text + " and Needle_Id = " + TxtNeedle.Tag.ToString(), ref Tdt);
                    if (Tdt.Rows.Count > 0)
                    {
                        Master_ID = Convert.ToInt64(Tdt.Rows[0]["RowID"]);
                    }


                    Queries = new String[Dt.Rows.Count + 5];
                    if (Master_ID > 0)
                    {
                        Queries[Array_Index++] = "Update Socks_Machine_Planning_Master Set EDate = GetDate(), EntrySystem = Host_Name() where RowID = " + Master_ID;
                        Queries[Array_Index++] = "Delete From Socks_Machine_Planning_Details Where Master_ID = " + Master_ID;
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Socks_Machine_Planning_Master (Year, Week, Needle_Id) Values (" + TxtYear.Text + ", " + TxtWeek.Text + ", " + TxtNeedle.Tag.ToString() + "); Select Scope_Identity()";
                    }
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Master_ID > 0)
                        {
                            Queries[Array_Index++] = "Insert Into Socks_Machine_Planning_Details (Master_ID, Machine_ID, Order_No, Target_Qty, Plan_Qty, Actual_Qty, Cycle_Time, Plan_Mins, Cycle_Seconds) Values (" + Master_ID + ", " + Grid["Machine_ID", i].Value + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["Target", i].Value + ", " + Grid["Plan", i].Value + ", 0, '" + Grid["CyTime", i].Value.ToString() + "', " + Grid["Plan_Mins", i].Value + ", " + Grid["CySecd", i].Value + ")";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert Into Socks_Machine_Planning_Details (Master_ID, Machine_ID, Order_No, Target_Qty, Plan_Qty, Actual_Qty, Cycle_Time, Plan_Mins, Cycle_Seconds) Values (@@IDENTITY, " + Grid["Machine_ID", i].Value + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["Target", i].Value + ", " + Grid["Plan", i].Value + ", 0, '" + Grid["CyTime", i].Value.ToString() + "', " + Grid["Plan_Mins", i].Value + ", " + Grid["CySecd", i].Value + ")";
                        }
                    }

                    if (Master_ID > 0)
                    {
                        MyBase.Run_Identity(true, Queries);
                    }
                    else
                    {
                        MyBase.Run_Identity(false, Queries);
                    }

                    MessageBox.Show("Saved ...!", "Gainup");
                    Calculate_Utilization();
                    this.Close();
                }
                else
                {
 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Grid_Data();
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
                for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                {
                    if (Grid.Columns.Count > 1)
                    {
                        Grid.Rows[i].Height = 26;
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index && Grid.CurrentCell.RowIndex > 0)
                {
                    if (Grid["Machine_ID", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        if (Get_Free_Seconds(Convert.ToInt32(Grid["Machine_ID", Grid.CurrentCell.RowIndex - 1].Value), Grid.CurrentCell.RowIndex) > 0)
                        {
                            Grid["Machine_ID", Grid.CurrentCell.RowIndex].Value = Grid["Machine_ID", Grid.CurrentCell.RowIndex - 1].Value;
                            Grid["Machine", Grid.CurrentCell.RowIndex].Value = Grid["Machine", Grid.CurrentCell.RowIndex - 1].Value;
                            Txt.Text = Grid["Machine", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        }
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Plan"].Index)
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

        Int32 Get_Planned_Seconds(Int32 MachineID, Int32 Row)
        {
            Int32 Assigned_Mins = 0;
            try
            {
                for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                {
                    if (i != Row)
                    {
                        if (Grid["Plan_Mins", i].Value != null && Grid["Plan_Mins", i].Value.ToString() != String.Empty)
                        {
                            if (Convert.ToInt32(Grid["Machine_ID", i].Value) == MachineID)
                            {
                                Assigned_Mins += Convert.ToInt32(Grid["Plan_Mins", i].Value);
                            }
                        }
                    }
                }

                return Assigned_Mins;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        Int32 Get_Free_Seconds(Int32 MachineID, Int32 Row)
        {
            // 100 % but we need 95 %
            //Int32 Total_Mins = 604800; 
            Int32 Total_Mins = 574560;
            Int32 Assigned_Mins = 0;
            Int32 Result = 0;
            try
            {
                return Total_Mins - Get_Planned_Seconds(MachineID, Grid.CurrentCell.RowIndex);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        Boolean Is_Machine_Order_Repeat(String Order_No, String Machine)
        {
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Machine", i].Value.ToString() == Machine && Grid["Order_No", i].Value.ToString() == Order_No)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                    {
                        Dr = Tool.Selection_Tool_WOMDI(this, 550, 180, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine, Machine_ID From Knitting_Mc_NO() Where .Dbo.Get_Needle_For_Week_Date (Machine_ID, " + TxtYear.Text + ", " + TxtWeek.Text + ", Getdate()) = '" + TxtNeedle.Text + "'", String.Empty, 100);
                        //Dr = Tool.Selection_Tool_WOMDI(this, 550, 180, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine, Machine_ID From Knitting_Mc_NO() Where .Dbo.Get_Needle_For_Week (Machine_ID, " + TxtYear.Text + ", " + TxtWeek.Text + ") = '" + TxtNeedle.Text + "'", String.Empty, 100);
                        if (Dr != null)
                        {
                            Grid["Machine_ID", Grid.CurrentCell.RowIndex].Value = Dr["Machine_ID"].ToString();
                            Grid["Machine", Grid.CurrentCell.RowIndex].Value = Dr["Machine"].ToString();
                            Txt.Text = Dr["Machine"].ToString();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                    {
                        //Dr = Tool.Selection_Tool_WOMDI(this, 550, 180, SelectionTool_Class.ViewType.NormalView, "Select Order", "Select S1.Order_No, S1.Ship_Date, Sum(S1.BOM_Qty) BOM_Qty, S1.Bal_Qty Req_Production,Isnull(Sum(S2.plan_Qty), 0) Planned, cycle, Seconds From (Select A.Order_No, A.Ship_Date, A.Needle, A.Needle_ID, convert(varchar, Dateadd(Second, Avg(Datediff(SECOND, 0, cast('00:' + Cycle_Pair as time))), 0), 108) Cycle, Avg(Datediff(SECOND, 0, cast('00:' + Cycle_Pair as time))) Seconds, Sum(Bom_Qty) Bom_Qty, Sum(isnull(B.Production,0)) Production, (Sum(Bom_Qty) - Sum(isnull(B.Production,0))) Bal_Qty From Socks_Bom() A Left Join Get_Knit_Prod_Needle_Order_ColorWise () B on A.Order_No = B.Order_No And A.OrderColorId = B.OrderColorID and A.Needle = B.Needle Where A.Despatch_Closed = 'N' Group By A.Order_No, A.Ship_Date, A.Needle, A.Needle_ID) S1 Left Join (Select R1.Order_No, R2.Needle_Id, Sum(R1.Plan_Qty) Plan_Qty from Socks_Machine_Planning_Details R1 Inner join Socks_Machine_Planning_Master R2 on R1.Master_ID = R2.RowID Group by R1.Order_No, R2.Needle_Id) S2 on S1.Order_No = S2.Order_No and S1.Needle_ID = S2.Needle_Id Where Needle = '" + TxtNeedle.Text.Trim() + "' Group By S1.Order_No, S1.Ship_Date, cycle, Seconds, S1.Bal_Qty  ", String.Empty, 120, 100, 100, 100, 100, 100);
                        String Str1 = " Select S1.Order_No, S1.Ship_Date, Sum(S1.BOM_Qty) BOM_Qty, S1.Bal_Qty Req_Production,Isnull(Sum(S2.plan_Qty), 0) Planned, cycle, Seconds From ";
                        Str1 = Str1 + " (Select A.Order_No, A.Ship_Date, A.Needle, A.Needle_ID, convert(varchar, Dateadd(Second, Avg(Datediff(SECOND, 0, cast('00:' + Cycle_Pair as time))), 0), 108) Cycle, AVG(A.Cycle_Pair_Seconds)Seconds, ";
                        Str1 = Str1 + " Sum(Bom_Qty) Bom_Qty, Sum(isnull(B.Production,0)) Production, (Sum(Bom_Qty) - Sum(isnull(B.Production,0))) Bal_Qty From Socks_Bom_Planner() A ";
                        Str1 = Str1 + " Left Join (Select Order_No, OrderColorID, SUM(Production)Production From Floor_Knitting_Details Group By Order_No, OrderColorID) B on A.Order_No = B.Order_No And A.OrderColorId = B.OrderColorID ";
                        Str1 = Str1 + " Where A.Despatch_Closed = 'N' Group By A.Order_No, A.Ship_Date, A.Needle, A.Needle_ID Having (Sum(Bom_Qty) - Sum(isnull(B.Production,0))) > 0) S1 ";
                        Str1 = Str1 + " Left Join (Select R1.Order_No, R2.Needle_Id, Sum(R1.Plan_Qty) Plan_Qty from Socks_Machine_Planning_Details R1 Inner join Socks_Machine_Planning_Master R2 on R1.Master_ID = R2.RowID ";
                        Str1 = Str1 + " Group by R1.Order_No, R2.Needle_Id) S2 on S1.Order_No = S2.Order_No and S1.Needle_ID = S2.Needle_Id Where Needle = '" + TxtNeedle.Text.Trim() + "' Group By S1.Order_No, S1.Ship_Date, cycle, Seconds, S1.Bal_Qty, Seconds";
                        Dr = Tool.Selection_Tool_WOMDI(this, 550, 180, SelectionTool_Class.ViewType.NormalView, "Select Order", Str1, String.Empty, 120, 100, 100, 100, 100, 100);

                        if (Dr != null)
                        {

                            if (Is_Machine_Order_Repeat(Dr["Order_No"].ToString(), Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString()))
                            {
                                MessageBox.Show("Already this Order Selected for this Machine ...!", "Gainup");
                                return;
                            }

                            MyBase.Row_Number(ref Grid);
                            Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                            Txt.Text = Dr["Order_No"].ToString();
                            Grid["Ship_Date", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Dr["Ship_Date"]);
                            Grid["Bom", Grid.CurrentCell.RowIndex].Value = Dr["Bom_Qty"].ToString();
                            Grid["Req_Production", Grid.CurrentCell.RowIndex].Value = Dr["Req_Production"].ToString();

                            DataTable Tdt = new DataTable();
                            //String Str = "select A.*,B.Order_No,B.Needle from Get_Max_Cycle_Time() A Left Join Socks_Bom() B on A.OrderColorID=B.OrderColorId where " + TxtWeek.Text + " >= datepart(WEEK,EffectFrom) and Needle='" + TxtNeedle.Text + "' and Order_No= '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value + "' ";
                            String Str = "Select A.*, B.Order_No, B.Needle from Get_Max_Cycle_Time() A Left Join Socks_Bom_Planner() B on A.OrderColorID = B.OrderColorId where " + TxtWeek.Text + " >= datepart(WEEK, EffectFrom) and Needle = '" + TxtNeedle.Text + "' and Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value + "' ";
                            MyBase.Load_Data(Str, ref Tdt);

                            if (Tdt.Rows.Count > 0)
                            {
                                Grid["CyTime", Grid.CurrentCell.RowIndex].Value = Tdt.Rows[0]["cycle_time"].ToString();
                                Grid["CySecd", Grid.CurrentCell.RowIndex].Value = Tdt.Rows[0]["cycle_Seconds"].ToString();
                            }
                            else
                            {
                                Grid["CyTime", Grid.CurrentCell.RowIndex].Value = Dr["cycle"].ToString();
                                Grid["CySecd", Grid.CurrentCell.RowIndex].Value = Dr["Seconds"].ToString();
                            }

                            Update_TobePlanned_Target(Grid.CurrentCell.RowIndex);
                            
                            //Grid["ToBePlanned", Grid.CurrentCell.RowIndex].Value = Convert.ToInt32(Dr["ToBePlanned"]) - Convert.ToInt32(Get_Already_Planned_OcnWise(Dr["Order_No"].ToString()));
                            //Set_Target();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Update_TobePlanned_Target(Int32 Row)
        {
            Int32 Free_Mins = 0;
            try
            {
                if (Grid["Order_No", Row].Value == DBNull.Value || Grid["Machine", Row].Value == DBNull.Value)
                {
                    ToBePlanned.Text = "0";
                    return;
                }

                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select Isnull(Sum(Plan_Qty), 0) From Socks_Machine_Planning_Details where Order_no = '" + Grid["Order_No", Row].Value.ToString() + "' and Master_ID <> " + Get_Master_ID(), ref Tdt);
                ToBePlanned.Text = Convert.ToString(Convert.ToInt32(Grid["Req_Production", Row].Value) - (Get_Already_Planned_OcnWise (Grid["Order_No", Row].Value.ToString()) - Convert.ToInt32(Tdt.Rows[0][0])));

                if (Grid["Plan", Row].Value != DBNull.Value)
                {
                    ToBePlanned.Text = Convert.ToString(Convert.ToInt32 (ToBePlanned.Text) + Convert.ToDouble(Grid["Plan", Row].Value));
                }


                Free_Mins = Get_Free_Seconds(Convert.ToInt32(Grid["Machine_ID", Row].Value), Row);
                if (Convert.ToDouble(ToBePlanned.Text) * Convert.ToDouble(Grid["CySecd", Row].Value) <= Free_Mins)
                {
                    Grid["Target", Row].Value = Convert.ToInt32(ToBePlanned.Text);
                }
                else
                {
                    Grid["Target", Row].Value = (Free_Mins / Convert.ToDouble(Grid["CySecd", Row].Value));
                }

                Fill_Lables(Convert.ToInt32(Grid["Machine_ID", Row].Value), Row);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            Int32 Free_Mins = 0;
            Int32 Row = 0; Int32 Col = 0;
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Plan"].Index)
                    {
                        

                        Row = Grid.CurrentCell.RowIndex;
                        Col = Grid.CurrentCell.ColumnIndex;


                        if (Grid["Plan", Grid.CurrentCell.RowIndex].Value == null || Grid["Plan", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToInt32(Grid["Plan", Grid.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Plan Qty ...!", "Gainup");
                            Grid.CurrentCell = Grid[Col, Row];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                        else if (Convert.ToInt32(Grid["Plan", Grid.CurrentCell.RowIndex].Value) > Convert.ToInt32(Grid["Target", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Greater than Target Qty ...!", "Gainup");
                            Grid.CurrentCell = Grid[Col, Row];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        Free_Mins = Get_Free_Seconds(Convert.ToInt32(Grid["Machine_ID", Grid.CurrentCell.RowIndex].Value), Grid.CurrentCell.RowIndex);
                        Grid["Plan_Mins", Grid.CurrentCell.RowIndex].Value = Convert.ToInt32(Grid["CySecd", Grid.CurrentCell.RowIndex].Value) * Convert.ToInt32(Grid["Plan", Grid.CurrentCell.RowIndex].Value);
                        Update_TobePlanned_Target(Grid.CurrentCell.RowIndex);
                        Fill_Lables (Convert.ToInt32(Grid["Machine_ID", Grid.CurrentCell.RowIndex].Value), Grid.CurrentCell.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        Int32 Get_Already_Planned_OcnWise(String OcnNo)
        {
            Int32 Bom = 0;
            Int32 Qty = 0;
            try
            {
                for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                {
                    if (Grid["Order_No", i].Value != DBNull.Value && Grid["Order_No", i].Value != null && Grid["Plan", i].Value != DBNull.Value && Grid["Plan", i].Value != null)
                    {
                        if (Grid["Order_No", i].Value.ToString() == OcnNo)
                        {
                            Qty += Convert.ToInt32(Grid["Plan", i].Value);
                        }
                    }
                }

                return Qty;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        void Fill_Lables(Int32 MachineID, Int32 RowIndex)
        {
            Int32 Free_Mins = 0;
            try
            {
                // Fill Lables
                LblTotalMins.Text = "574560";
                LblPlanMins.Text = Get_Planned_Seconds (MachineID, Grid.CurrentCell.RowIndex).ToString();
                LblFreeMins.Text = Get_Free_Seconds(MachineID, Grid.CurrentCell.RowIndex).ToString();
                LblCySecds.Text = Grid["CySecd", RowIndex].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Int64 Get_Master_ID()
        {
            DataTable Tdt = new DataTable();
            try
            {
                MyBase.Load_Data("Select RowId From Socks_Machine_Planning_Master where Year = " + TxtYear.Text + " and Week = " + TxtWeek.Text + " and Needle_Id = " + TxtNeedle.Tag.ToString(), ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    return Convert.ToInt64(Tdt.Rows[0]["RowID"]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private void Grid_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (Dt != null && Dt.Rows.Count > 0 && Grid.CurrentCell != null)
                {
                    Update_TobePlanned_Target(Grid.CurrentCell.RowIndex);
                    Calculate_Utilization();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Grid_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell != null && Grid["Machine_ID", Grid.CurrentCell.RowIndex].Value != null && Grid["Machine_ID", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    Fill_Lables(Convert.ToInt32(Grid["Machine_ID", Grid.CurrentCell.RowIndex].Value), Grid.CurrentCell.RowIndex);
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
                MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentCell.RowIndex);
                MyBase.Row_Number(ref Grid);
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message);
            }
        }

    }
}