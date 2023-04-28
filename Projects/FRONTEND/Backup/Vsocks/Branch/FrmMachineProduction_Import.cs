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
    public partial class FrmMachineProduction_Import : Form
    {
        
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Tdt = new DataTable();
        Int64 Code = 0;
        DataRow Dr;
        MDIMain MyParent;
        TextBox Txt = null;
        String Buffer_Table = String.Empty;
        Boolean Buffer_Update = false;

        String Str;

        public FrmMachineProduction_Import(DateTime FDate, DateTime TDate, Int16 FShift, Int16 TShift, String FNeedle, String TNeedle, Int32 FYear, Int32 TYear, Int32 FWeek, Int32 TWeek)
        {
            InitializeComponent();
            MyBase.Clear(this);
            DtpTDate.Text = String.Format("{0:dd/MM/yy}", TDate);
            TxtTShift.Text = TShift.ToString();
            TxtTYear.Text = TYear.ToString();
            TxtTWeek.Text = TWeek.ToString();
            TxtTNeedle.Text = TNeedle;
            DtpFDate.Text = String.Format("{0:dd/MM/yy}", FDate);
            TxtFShift.Text = FShift.ToString();
            TxtFYear.Text = FYear.ToString();
            TxtFWeek.Text = FWeek.ToString();
            TxtFNeedle.Text = FNeedle;
            
        }

        private void FrmMachineProduction_Import_Load(object sender, EventArgs e)
        {
            try
            {
                MyBase.Disable_Cut_Copy(GBMain);
                Grid_Data(); 
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

        private void FrmMachineProduction_Import_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "DtpTDate")
                    {
                        Grid.CurrentCell = Grid["Order_No", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
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
                else if (e.KeyCode == Keys.Escape)
                {
                    DtpTDate.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Grid_Data()
        {
            try
            {
                //DataTable Tdt = new DataTable();
                //MyBase.Load_Data("Select Machine_ID From Knitting_Mc_No() Where Machine = '" + LblMachine.Text + "'", ref Tdt);
                //LblMachine.Tag = Tdt.Rows[0][0].ToString();

                Tdt = new DataTable();
                MyBase.Load_Data("Select RowID From VFit_Sample_Needle_Master Where Name = '" + TxtFNeedle.Text + "'", ref Tdt);
                TxtFNeedle.Tag = Tdt.Rows[0][0].ToString();

                Tdt = new DataTable();
                MyBase.Load_Data("Select RowID From VFit_Sample_Needle_Master Where Name = '" + TxtTNeedle.Text + "'", ref Tdt);
                TxtTNeedle.Tag = Tdt.Rows[0][0].ToString();

                //Grid.DataSource = MyBase.Load_Data("Select 0 as Slno, S1.Order_No, S1.OrderColorID, S2.Color Sample, S2.size Size, Convert(Varchar, Cast('00:' + S2.Cycle_Pair as DateTime), 108) CyTime, Datediff(SECOND, 0, cast('00:' + S2.Cycle_Pair as time)) CySecd, S2.Bom_Qty BOM, Isnull(G1.Production, 0) Production, (S2.Bom_Qty - Isnull(G1.Production, 0)) Balance, S1.Qty, '' T, S3.Emplno, (Case When S1.Order_No Is Not Null And S1.OrderColorID Is Not Null Then 'O' Else ' 'End) Record_Type From Socks_Machine_Production_Details  S1 Left join Socks_Machine_Production_Master S3 On S1.Master_ID = S3.RowId Left join Socks_Bom() S2 on S1.OrderColorID = S2.OrderColorId and S1.Order_No = S2.Order_No Left join Get_Knit_Prod () G1 on S3.Order_No = G1.Order_No And S2.color = G1.Sample Where S3.year = " + LblYear.Text + " and S3.week = " + LblWeek.Text + " and S3.Needle_ID = " + LblNeedle.Tag.ToString() + " and S3.Machine_ID = " + LblMachine.Tag.ToString() + " and S3.Order_No = '" + LblOrderNo.Text + "' and S3.Entry_Date = '" + LblDate.Tag.ToString() + "' and S3.Shift = " + LblShift.Text + " Order By S1.RowID", ref Dt);
                Str = " Select 0 as SNo, S1.Needle_ID, N1.Name Needle, S1.Machine_ID, M1.Machine, S2.Order_No Order_No, S2.OrderColorID, S3.color Sample, S3.size, S3.Bom_Qty Bom, Isnull(P1.Production,0)Prod, (Isnull(S3.Bom_Qty,0) - Isnull(P1.Production,0))Bal_Qty, S1.Plan_Qty, 0 Prod_Qty, S1.Assign_Qty, '' Emplno, '' Operator, S1.Planned_Seconds, S2.Qty, S2.Order_No Order_No2, '-'T, 'N' Mode from Socks_Machine_Production_Master S1 ";
                Str = Str + " Left JOIn Socks_Machine_Production_Details S2 On S1.RowId = S2.Master_ID Left Join VFit_Sample_Needle_Master N1 On S1.Needle_ID = N1.RowID Left Join Knitting_Mc_No() M1 On S1.Machine_ID = M1.Machine_ID ";
                Str = Str + " Left Join Socks_Bom() S3 On S2.Order_No = S3.Order_No And S2.OrderColorID = S3.OrderColorId Left Join Get_Knit_Production_OrderWise() P1 On S2.Order_No = P1.Order_No And S2.OrderColorID = P1.OrderColorID Where S1.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' And S1.Shift = " + TxtFShift.Text + " And S1.Needle_ID = " + TxtFNeedle.Tag + " And S1.Year = " + TxtFYear.Text + " And S1.Week = " + TxtFWeek.Text + " Order By Machine ";
                //Str = Str + " Left Join Socks_Bom() S3 On S2.Order_No = S3.Order_No And S2.OrderColorID = S3.OrderColorId Left Join Get_Knit_Production_OrderWise() P1 On S2.Order_No = P1.Order_No And S2.OrderColorID = P1.OrderColorID Where S1.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' And S1.Shift = " + TxtFShift.Text + " And S1.Needle_ID = " + TxtFNeedle.Tag + " And S1.Year = " + TxtFYear.Text + " And S1.Week = " + TxtFWeek.Text + " And (Isnull(S3.Bom_Qty,0) - Isnull(P1.Production,0)) > 0 Order By Machine ";
                //Str = Str + " Left Join Socks_Bom() S3 On S2.Order_No = S3.Order_No And S2.OrderColorID = S3.OrderColorId Left Join Get_Knit_Production_OrderWise() P1 On S2.Order_No = P1.Order_No And S2.OrderColorID = P1.OrderColorID Where S1.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' And S1.Shift = " + TxtFShift.Text + " And S1.Needle_ID = " + TxtFNeedle.Tag + " And S1.Year = " + TxtFYear.Text + " And S1.Week = " + TxtFWeek.Text + " Order By Machine ";
                
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Needle_ID", "Needle", "Machine_ID", "OrderColorID", "Plan_Qty", "Emplno", "Planned_Seconds", "Qty", "Order_No2", "Prod_Qty", "T", "Mode");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Order_No", "Sample", "Assign_Qty", "Operator");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 40, 80, 120, 100, 100, 100, 80, 80, 80, 110);

                Grid.RowHeadersWidth = 20;

                Grid.Columns["Prod"].HeaderText = "Produced";

                Grid.Columns["BOM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Prod"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Bal_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Assign_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                MyBase.Row_Number(ref Grid);

                //if (Dt.Rows.Count > 0)
                //{
                //    Tdt = new DataTable();
                //    MyBase.Load_Data("Select Name, Tno, Emplno from Vaahini_ERP_Gainup.Dbo.Employeemas Where Emplno = " + Dt.Rows[0]["Emplno"].ToString(), ref Tdt);
                //    if (Tdt.Rows.Count > 0)
                //    {
                //        TxtEmployee.Text = Tdt.Rows[0]["Name"].ToString() + " - " + Tdt.Rows[0]["Tno"].ToString();
                //        TxtEmployee.Tag = Tdt.Rows[0]["Emplno"].ToString();
                //    }
                //    else
                //    {
                //        TxtEmployee.Text = String.Empty;
                //    }

                //}

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
                    //Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);
                    Txt.GotFocus += new EventHandler(Txt_GotFocus);
                    //Txt.TextChanged += new EventHandler(Txt_TextChanged);
                    //Txt.LostFocus += new EventHandler(Txt_LostFocus);
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                    {
                        OrderNo_Selection();
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Sample"].Index)
                    {
                        Sample_Selection();
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index)
                    {
                        Operator_Selection();
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index && Grid.CurrentCell.Value.ToString() == String.Empty )
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        Grid["Operator", Grid.CurrentCell.RowIndex].Value = Grid["Operator", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Emplno", Grid.CurrentCell.RowIndex].Value = Grid["Emplno", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Txt.Text = Grid["Operator", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void OrderNo_Selection()
        {
            try
            {
                String Str3;
                //Str3 = " Select S1.Order_No Order_No, S1.Descr Description from Socks_Bom() S1 Left Join Floor_Knitting_Details F1 On S1.Order_No = F1.Order_No Where S1.Needle = '" + TxtTNeedle.Text.ToString() + "' Group By S1.Order_No, S1.Descr Having (Sum(Isnull(S1.Bom_Qty,0)) - SUM(Isnull(Production,0))) > 0 Order By S1.Order_No";
                //Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Order_No", " Select S1.Order_No Order_No from Socks_Bom() S1 Left Join Floor_Knitting_Details F1 On S1.Order_No = F1.Order_No Where S1.Needle = '" + TxtTNeedle.Text.ToString() + "' Group By S1.Order_No Having (Sum(Isnull(S1.Bom_Qty,0)) - SUM(Isnull(Production,0))) > 0 Order By S1.Order_No", String.Empty, 150);

                Str3 = " Select Order_No, Descr Description from Get_Sample_Production_Planning('" + TxtTNeedle.Text.ToString() + "') ";
                Dr = Tool.Selection_Tool_WOMDI(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Order_No", Str3, String.Empty, 150, 200);
                if (Dr != null)
                {
                    Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                    Txt.Text = Dr["Order_No"].ToString();
                    Grid["Sample", Grid.CurrentCell.RowIndex].Value = "";
                    Grid["Size", Grid.CurrentCell.RowIndex].Value = "";
                    Grid["Bom", Grid.CurrentCell.RowIndex].Value = 0;
                    Grid["Prod", Grid.CurrentCell.RowIndex].Value = 0;
                    Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value = 0;
                    Grid["Assign_Qty", Grid.CurrentCell.RowIndex].Value = 0;
                    Grid["Planned_Seconds", Grid.CurrentCell.RowIndex].Value = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Sample_Selection()
        {
            try
            {
                if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString()  != String.Empty)
                {
                    String Str3;
                    //Str3 = " Select S1.Color Sample, S1.Size, S1.Bom_Qty BOM, Isnull(G1.Production, 0) Production, (S1.Bom_Qty - Isnull(G1.Production, 0)) Qty, S1.OrderColorID, Datediff(SECOND, 0, cast('00:' + (Case When Cast(Getdate() As Date) >= G2.EffectFrom Then Isnull(G2.New_Cycle_Pair, S1.Cycle_Pair) Else S1.Cycle_Pair End) as time)) Cycle_Seconds, ";
                    //Str3 = Str3 + " Convert(Varchar, Cast('00:' + (Case When Cast(Getdate() As Date) >= G2.EffectFrom Then Isnull(G2.New_Cycle_Pair, S1.Cycle_Pair) Else S1.Cycle_Pair End) as DateTime), 108) Cycle_Time, (Case When (28800 / Datediff(SECOND, 0, cast('00:' + (Case When Cast(Getdate() As Date) >= G2.EffectFrom Then Isnull(G2.New_Cycle_Pair, S1.Cycle_Pair) Else S1.Cycle_Pair End) as time)) <= (S1.Bom_Qty - Isnull(G1.Production, 0))) Then (28800 / Datediff(SECOND, 0, cast('00:' + (Case When Cast(Getdate() As Date) >= G2.EffectFrom Then Isnull(G2.New_Cycle_Pair, S1.Cycle_Pair) Else S1.Cycle_Pair End) as time))) Else (S1.Bom_Qty - Isnull(G1.Production, 0)) End)Assign_Qty, ";
                    //Str3 = Str3 + " (S1.Bom_Qty - Isnull(G1.Production, 0))Bal_Qty, Isnull(G1.Production, 0) Prod ";
                    //Str3 = Str3 + " ,S1.OrderColorID, Datediff(SECOND, 0, cast('00:' + (Case When Cast(Getdate() As Date) >= G2.EffectFrom Then Isnull(G2.New_Cycle_Pair, S1.Cycle_Pair) Else S1.Cycle_Pair End) as time)) * (Case When (28800 / Datediff(SECOND, 0, cast('00:' + (Case When Cast(Getdate() As Date) >= G2.EffectFrom Then Isnull(G2.New_Cycle_Pair, S1.Cycle_Pair) Else S1.Cycle_Pair End) as time)) <= (S1.Bom_Qty - Isnull(G1.Production, 0))) Then (28800 / Datediff(SECOND, 0, cast('00:' + (Case When Cast(Getdate() As Date) >= G2.EffectFrom Then Isnull(G2.New_Cycle_Pair, S1.Cycle_Pair) Else S1.Cycle_Pair End) as time))) Else (S1.Bom_Qty - Isnull(G1.Production, 0)) End)Planned_Seconds ";
                    //Str3 = Str3 + " From Socks_Bom () S1 Left Join Get_Knit_Prod () G1 on S1.Order_No = G1.Order_No And S1.color = G1.Sample Left Join Get_Max_Cycle_Time() G2 On S1.OrderColorId = G2.OrderColorID Where S1.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And S1.Needle_ID = " + TxtTNeedle.Tag.ToString() + " ";
                    
                    //Str3 = Str3 + " From Socks_Bom () S1 Left Join Get_Knit_Prod () G1 on S1.Order_No = G1.Order_No And S1.color = G1.Sample Left Join Get_Max_Cycle_Time() G2 On S1.OrderColorId = G2.OrderColorID Where S1.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And S1.Needle_ID = " + TxtTNeedle.Tag.ToString() + " and (S1.Bom_Qty - Isnull(G1.Production, 0)) > 0";

                    Str3 = " Select Sample, Size, BOM, Production, Qty, OrderColorID, Cycle_Seconds, Cycle_Time, Assign_Qty, Bal_Qty, Prod, Planned_Seconds From Get_Order_Sample_For_IMport_Produciton_planning('" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + TxtTNeedle.Tag.ToString() + ")";
                    Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", Str3, String.Empty, 100, 80, 80, 80);
                    
                    //Dr = Tool.Selection_Tool_Except_New_WOMDI("Sample", this, 500, 180, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select S1.Color Sample, S1.Size, S1.Bom_Qty BOM, Isnull(G1.Production, 0) Production, (S1.Bom_Qty - Isnull(G1.Production, 0)) Qty, S1.OrderColorID, Datediff(SECOND, 0, cast('00:' + S1.Cycle_Pair as time)) Cycle_Seconds, Convert(Varchar, Cast('00:' + S1.Cycle_Pair as DateTime), 108) Cycle_Time From Socks_Bom () S1 Left Join Get_Knit_Prod () G1 on S1.Order_No = G1.Order_No And S1.color = G1.Sample Where S1.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And S1.Needle_ID = " + LblNeedle.Tag.ToString() + " and (S1.Bom_Qty - Isnull(G1.Production, 0)) > 0", String.Empty, 100, 80, 80, 80);
                    if (Dr != null)
                    {
                        Grid["Sample", Grid.CurrentCell.RowIndex].Value = Dr["Sample"].ToString();
                        Txt.Text = Dr["Sample"].ToString();
                        Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                        Grid["Bom", Grid.CurrentCell.RowIndex].Value = Dr["Bom"].ToString();
                        Grid["Prod", Grid.CurrentCell.RowIndex].Value = Dr["Prod"].ToString();
                        Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bal_Qty"].ToString();
                        Grid["Assign_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Assign_Qty"].ToString();
                        Grid["Planned_Seconds", Grid.CurrentCell.RowIndex].Value = Dr["Planned_Seconds"].ToString();
                        Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Dr["OrderColorID"].ToString();
                    }
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
                if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    String Str3;
                    Str3 = " Select Name, Tno, Emplno From Vaahini_ERP_Gainup.Dbo.Employeemas E1 Inner Join Vaahini_ERP_Gainup.Dbo.Depttype D1 on E1.Deptcode = D1.DeptCode and E1.COMPCODE = D1.compcode Where E1.compcode =2 and D1.deptCode = 82 and E1.tno not like '%Z'";
                    Dr = Tool.Selection_Tool_WOMDI(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Employee", Str3, String.Empty, 300, 80);
                    if (Dr != null)
                    {
                        Grid["Operator", Grid.CurrentCell.RowIndex].Value = Dr["Name"].ToString();
                        Txt.Text = Dr["Name"].ToString();
                        Grid["Emplno", Grid.CurrentCell.RowIndex].Value = Dr["Emplno"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Sample"].Index)
                    {
                        if (Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            MessageBox.Show("Pls Select Sample.....!Gainup");
                            Grid.CurrentCell = Grid["Sample", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index)
                    {
                        if (Grid["Operator", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            MessageBox.Show("Pls Select Sample.....!Gainup");
                            Grid.CurrentCell = Grid["Operator", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Assign_Qty"].Index)
                    {
                        if (Grid["Assign_Qty", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Assign_Qty", Grid.CurrentCell.RowIndex].Value) <= 0)
                        {
                            Grid["Assign_Qty", Grid.CurrentCell.RowIndex].Value = "0";
                        }
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
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    button1.Focus(); 
                    SendKeys.Send("{End}");
                }
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
                Int32 E = 0;
                if (MessageBox.Show("Sure to Save ...!", "Gainup", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    DtpTDate.Focus(); 
                    return;
                }
                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid details ...!", "Gainup");
                    DtpTDate.Focus();
                    return;
                }
                for (int i = 0; i <= Grid.Rows.Count - 1; i++)
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

                String[] Queries = new String[(Dt.Rows.Count * 2) + 5];
                Int32 Array_Index = 0;
                Int64 Master_ID = 0;

                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select Max(RowID)+1 RowID From Socks_Machine_Production_Master",ref Tdt);
                
                if (Tdt.Rows.Count > 0)
                {
                    Master_ID = Convert.ToInt64(Tdt.Rows[0]["RowID"]);
                }
                E = 0;
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["Mode"].ToString() == "N")
                    {
                        
                        Queries[Array_Index++] = "Insert into Socks_Machine_production_Master (Entry_Date, Year, Week, Shift, Needle_ID, Machine_ID, Order_No, Plan_Qty, Prod_Qty, Assign_Qty, Emplno, Planned_Seconds) Values ('" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "', " + TxtTYear.Text + ", " + TxtTWeek.Text + ", " + TxtTShift.Text + ", " + TxtTNeedle.Tag.ToString() + ", " + Dt.Rows[i]["Machine_ID"].ToString() + ", '" + Dt.Rows[i]["Order_No"].ToString() + "', " + Dt.Rows[i]["Plan_Qty"].ToString() + ", 0, " + Dt.Rows[i]["Assign_Qty"].ToString() + ", " + Dt.Rows[i]["Emplno"].ToString() + ", " + Dt.Rows[i]["Planned_Seconds"].ToString() + ") ; Select Scope_Identity ()";

                        for (int j = i; j <= Dt.Rows.Count - 1; j++)
                        {
                            if (Dt.Rows[i]["Machine_ID"].ToString() == Dt.Rows[j]["Machine_ID"].ToString())
                            {
                                Queries[Array_Index++] = "Insert Into Socks_Machine_Production_Details (Master_ID, Order_No, OrderColorID, Qty) Values (@@IDENTITY + " + E + ", '" + Dt.Rows[j]["Order_No"].ToString() + "', " + Dt.Rows[j]["OrderColorID"].ToString() + ", " + Dt.Rows[j]["Qty"].ToString() + ")";
                                Dt.Rows[j]["Mode"] = "Y";
                            }
                        }
                        E = E + 1;
                    }
                }

                
                MyBase.Run_Identity(false, Queries);

                //MyParent.Save_Error = false;
                MessageBox.Show("Saved ...!", "Gainup");
                this.Close();

                //if (Master_ID > 0)
                //{
                //    Queries[Array_Index++] = "Update Socks_Machine_Production_Master Set Plan_Qty = " + LblPlan.Text + ", Assign_Qty = " + LblAssign.Text + ", Emplno = " + TxtEmployee.Tag.ToString() + ", Planned_Seconds = " + LBlPlannedSeconds.Text + " Where RowiD = " + Master_ID;
                //    Queries[Array_Index++] = "Delete From Socks_Machine_Production_Details Where Master_ID = " + Master_ID;
                //}
                //else
                //{
                //    Queries[Array_Index++] = "Insert into Socks_Machine_production_Master (Entry_Date, Year, Week, Shift, Needle_ID, Machine_ID, Order_No, Plan_Qty, Prod_Qty, Assign_Qty, Emplno, Planned_Seconds) Values ('" + LblDate.Tag.ToString() + "', " + LblYear.Text + ", " + LblWeek.Text + ", " + LblShift.Text + ", " + LblNeedle.Tag.ToString() + ", " + LblMachine.Tag.ToString() + ", '" + LblOrderNo.Text + "', " + LblPlan.Text + ", 0, " + LblAssign.Text + ", " + TxtEmployee.Tag.ToString() + ", " + LBlPlannedSeconds.Text + "); Select Scope_Identity ()";
                //}

                //for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                //{
                //    if (Master_ID > 0)
                //    {
                //        Queries[Array_Index++] = "Insert Into Socks_Machine_Production_Details (Master_ID, Order_No, OrderColorID, Qty) Values (" + Master_ID + ", '" + Dt.Rows[i]["Order_No"].ToString() + "', " + Dt.Rows[i]["OrderColorID"].ToString() + ", " + Dt.Rows[i]["Qty"].ToString() + ")";
                //    }
                //    else
                //    {
                //        Queries[Array_Index++] = "Insert Into Socks_Machine_Production_Details (Master_ID, Order_No, OrderColorID, Qty) Values (@@IDENTITY, '" + Dt.Rows[i]["Order_No"].ToString() + "', " + Dt.Rows[i]["OrderColorID"].ToString() + ", " + Dt.Rows[i]["Qty"].ToString() + ")";
                //    }
                //}

                //if (Master_ID > 0)
                //{
                //    MyBase.Run_Identity(true, Queries);
                //}
                //else
                //{
                //    MyBase.Run_Identity(false, Queries);
                //}
                //MessageBox.Show("Saved ...!", "Gainup");
                //this.Close();
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
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
                MessageBox.Show(ex.Message);
            }
        }
    }
}