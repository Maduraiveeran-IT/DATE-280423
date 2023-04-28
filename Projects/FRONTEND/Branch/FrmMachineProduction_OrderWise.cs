using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using SelectionTool_NmSp;
using Accounts_ControlModules;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmMachineProduction_OrderWise : Form
    {
        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();

        Int32 Grid_Row = 0;
        Int32 Grid_Col = 0;
        public double Utilization = 0;
        public int Assign_Qty = 0;
        DataTable Dt = new DataTable();
        TextBox Txt = null;
        DataRow Dr;
        Int16 LblUnit = 0;

        public FrmMachineProduction_OrderWise(DateTime Date, Int16 Shift, Int32 Year, Int32 Week, String Needle, String Machine, String Order_No, Int32 Plan_Qty, Int32 Row, Int32 Col)
        {
            InitializeComponent();
            MyBase.Clear(this);
            LblDate.Text = String.Format("{0:dd/MM/yy}", Date);
            LblDate.Tag = String.Format("{0:dd-MMM-yyyy}", Date);
            LblShift.Text = Shift.ToString();
            LblYear.Text = Year.ToString();
            LblWeek.Text = Week.ToString();
            LblOrderNo.Text = Order_No;
            LblWeek.Tag = Order_No;
            LblPlan.Text = Plan_Qty.ToString();
            LblMachine.Text = Machine;
            LblNeedle.Text = Needle;
        }

        void Calc_Utilization()
        {
            try
            {
                Update_Seconds();
                Update_AssignQty();
                if (Convert.ToInt32(LBlPlannedSeconds.Text) == 0)
                {
                    Utilization = 0;
                }
                else
                {
                    Utilization = (Convert.ToDouble(LBlPlannedSeconds.Text) / Convert.ToDouble(28800)) * 100;
                }

                Assign_Qty = Convert.ToInt16(LblAssign.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        Int32 Planned_Seconds()
        {
            Int32 Planned_Seconds = 0;
            try
            {
                for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                {
                    if (Grid["Qty", i].Value != null && Grid["Qty", i].Value != DBNull.Value && Grid["Qty", i].Value.ToString() != String.Empty)
                    {
                        Planned_Seconds += Convert.ToInt32(Grid["Qty", i].Value) * Convert.ToInt32(Grid["CySecd", i].Value);
                    }
                }

                return Planned_Seconds;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        Int32 Free_Seconds()
        {
            Int32 Total_Seconds = 28800;
            try
            {
                return Total_Seconds - Planned_Seconds();
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
                String Str;

                MyBase.Run("Exec Knit_Prod_Tab_Insert");
                MyBase.Run("Exec Knit_Order_Tab_Insert");
                MyBase.Run(" Exec Knit_Order_Sample_Tab_Insert");

                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select Machine_ID From Knitting_Mc_No() Where Machine = '" + LblMachine.Text + "'", ref Tdt);
                LblMachine.Tag = Tdt.Rows[0][0].ToString();

                Tdt = new DataTable();
                MyBase.Load_Data("Select RowID From VFit_Sample_Needle_Master Where Name = '" + LblNeedle.Text + "'", ref Tdt);
                LblNeedle.Tag = Tdt.Rows[0][0].ToString();

                DataTable Tdt1 = new DataTable();
                MyBase.Load_Data("Select Unit_Code From Knitting_Mc_NO_UnitWiseMachine() Where Machine = '" + LblMachine.Text + "'", ref Tdt1);
                LblUnit = Convert.ToInt16(Tdt1.Rows[0][0].ToString());

                //Commented By Sakthi On 0/Dec/2016 For SpeedUp
                //Str = " Select 0 as Slno, N1.Name Needle, S2.Needle_ID, S1.Order_No, S1.OrderColorID, S2.Color Sample, S2.size Size, Convert(Varchar, Cast('00:' + S2.Cycle_Pair as DateTime), 108) CyTime, Datediff(SECOND, 0, cast('00:' + S2.Cycle_Pair as time)) CySecd, S2.Bom_Qty BOM, Isnull(G1.Production, 0) Production, (S2.Bom_Qty - Isnull(G1.Production, 0)) Balance, S1.Qty, '' T, S3.Emplno, E1.Name Operator, (Case When S1.Order_No Is Not Null And S1.OrderColorID Is Not Null Then 'O' Else ' 'End) Record_Type, S1.Rowid DetailID, S1.Master_ID From Socks_Machine_Production_Details S1 ";
                //Str = Str + " Left join Socks_Machine_Production_Master S3 On S1.Master_ID = S3.RowId Left join Socks_Bom() S2 on S1.OrderColorID = S2.OrderColorId and S1.Order_No = S2.Order_No Left join Get_Knit_Prod () G1 on S3.Order_No = G1.Order_No And S2.color = G1.Sample Left Join VFit_Sample_Needle_Master N1 On S3.Needle_ID = N1.RowID Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on S3.Emplno = E1.Emplno ";
                //Str = Str + " Where S3.year = " + LblYear.Text + " and S3.week = " + LblWeek.Text + " and S3.Needle_ID = " + LblNeedle.Tag.ToString() + " and S3.Machine_ID = " + LblMachine.Tag.ToString() + " and S3.Entry_Date = '" + LblDate.Tag.ToString() + "' and S3.Shift = " + LblShift.Text + " Order By S1.RowID";
                //Grid.DataSource = MyBase.Load_Data(Str, ref Dt);

                Str = " Select 0 as Slno, N1.Name Needle, S3.Needle_ID, S1.Order_No, S1.OrderColorID, S2.Color Sample, S2.size Size, Convert(Varchar, Cast('00:' + S2.Cycle_Pair as DateTime), 108) CyTime, Datediff(SECOND, 0, cast('00:' + S2.Cycle_Pair as time)) CySecd, S2.Bom_Qty BOM, Isnull(G1.Production, 0) Production, (S2.Bom_Qty - Isnull(G1.Production, 0)) Balance, S1.Qty, '' T, S3.Emplno, E1.Name Operator, (Case When S1.Order_No Is Not Null And S1.OrderColorID Is Not Null Then 'O' Else ' 'End) Record_Type, S1.Rowid DetailID, S1.Master_ID From Socks_Machine_Production_Details S1 ";
                Str = Str + " Left join Socks_Machine_Production_Master S3 On S1.Master_ID = S3.RowId Left join Socks_Bom_Sample_Selection() S2 on S1.OrderColorID = S2.OrderColorId and S1.Order_No = S2.Order_No Left join Knit_Prod_Tab G1 on S3.Order_No = G1.Order_No And S1.OrderColorID = G1.OrderColorId And S2.ItemID = G1.ItemID And S2.SizeID = G1.SizeID Left Join VFit_Sample_Needle_Master N1 On S3.Needle_ID = N1.RowID Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 on S3.Emplno = E1.Emplno ";
                Str = Str + " Where S3.year = " + LblYear.Text + " and S3.week = " + LblWeek.Text + " and S3.Needle_ID = " + LblNeedle.Tag.ToString() + " and S3.Machine_ID = " + LblMachine.Tag.ToString() + " and S3.Entry_Date = '" + LblDate.Tag.ToString() + "' and S3.Shift = " + LblShift.Text + " Order By S1.RowID";
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);

                MyBase.Grid_Designing(ref Grid, ref Dt, "Needle_ID", "OrderColorID", "T", "CySecd", "Emplno", "Record_Type", "DetailID", "Master_ID");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Needle", "Order_No", "Sample", "Qty", "Operator");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 40, 60, 130, 80, 70, 70, 70, 80, 80, 80, 100);

                Grid.RowHeadersWidth = 20;

                Grid.Columns["Production"].HeaderText = "Produced";

                Grid.Columns["BOM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Production"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                MyBase.Row_Number(ref Grid);

                Update_AssignQty();
                Update_Seconds();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmMachineProduction_OrderWise_Load(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                MyBase.Clear(this);
                MyBase.Disable_Cut_Copy(GBMain);
                LblOrderNo.Text = LblWeek.Tag.ToString();
                Grid_Data();
                LblOrderNo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 Array_Index = 0;
                Int64 Master_ID = 0;
                Int64 Master_ID_Plan = 0;

                Int64 Count = 0;
                Int32 D = 1;
                Int32 D1 = 1;
                Int32 D2 = 0;

                if (MessageBox.Show("Sure to Save ...!", "Gainup", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    BtnCancel.Focus();
                    return;
                }

                Update_Seconds();
                Update_AssignQty();

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid details ...!", "Gainup");
                    LblOrderNo.Focus();
                    return;
                }

                if (Convert.ToInt32(LblFreeSeconds.Text) < 0)
                {
                    MessageBox.Show("Invalid Mins allocation ...!", "Gainup");
                    LblOrderNo.Focus();
                    return;
                }

                String[] Queries = new String[Dt.Rows.Count * 10];
               

                DataTable TDt1 = new DataTable();
                MyBase.Load_Data("SELECT IDENT_CURRENT ('Socks_Machine_production_Master') ", ref TDt1);

                DataTable TDt2 = new DataTable();
                MyBase.Load_Data("SELECT IDENT_CURRENT ('Socks_Needle_Change_Master') ", ref TDt2);

                DataTable TDt3 = new DataTable();
                MyBase.Load_Data("SELECT IDENT_CURRENT ('Socks_Machine_Planning_Master') ", ref TDt3);


                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (LblNeedle.Text != Dt.Rows[i]["Needle"].ToString() && LblNeedle.Tag != Dt.Rows[i]["Needle_ID"])
                    {
                        DataTable Dt1 = new DataTable();
                        MyBase.Load_Data("Select K1.Machine, N1.Name Needle, S1.Effect_From from Socks_Needle_Change_Master S1 Left Join Socks_Needle_Change_Details S2 on S1.RowID = S2.Master_ID Left Join Knitting_Mc_NO() K1 On S2.Machine_ID =  K1.Machine_ID Left Join VFit_Sample_Needle_Master N1 On S2.Needle_ID = N1.RowID Where S2.Machine_ID = " + LblMachine.Tag + " And Year = " + LblYear.Text + " and Week = " + LblWeek.Text + " And Effect_From = '" + LblDate.Tag.ToString() + "' ", ref Dt1);
                        if (Dt1.Rows.Count > 0)
                        {
                            MessageBox.Show("Already '" + Dt1.Rows[0]["Machine"].ToString() + "' Needle '" + Dt1.Rows[0]["Needle"].ToString() + "' Changed On '" + String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[0]["Effect_From"]) + "'");
                            Grid.CurrentCell = Grid["Needle", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            if (Count == 0)
                            {
                                DataTable Dt2 = new DataTable();
                                MyBase.Load_Data("Select Rowid, Year, Week, Effect_From from Socks_Needle_Change_Master Where Year = " + LblYear.Text + " and Week = " + LblWeek.Text + " And Effect_From = '" + LblDate.Tag.ToString() + "' ", ref Dt2);
                                if (Dt2.Rows.Count > 0)
                                {
                                    Int64 Code = Convert.ToInt16(Dt2.Rows[0]["RowID"].ToString());
                                    
                                    Queries[Array_Index++] = "Update Socks_Needle_Change_Master Set Year =  " + LblYear.Text + " , Week =  " + LblWeek.Text + " Where Rowid = " + Code;
                                    Queries[Array_Index++] = "Delete From Socks_Needle_Change_Details Where Master_id = " + Code + " And Machine_ID = " + LblMachine.Tag;
                                    Queries[Array_Index++] = "Insert into Socks_Needle_Change_Details (Master_ID, Machine_ID, Needle_ID) Values (" + Code + ", " + LblMachine.Tag + ", " + Grid["Needle_Id", i].Value + ")";
                                }
                                else
                                {                                    
                                    Queries[Array_Index++] = "Insert Into Socks_Needle_Change_Master (Year, Week, Effect_From) Values (" + LblYear.Text + ", " + LblWeek.Text + ", '" + LblDate.Tag.ToString() + "');";
                                    Queries[Array_Index++] = "Insert into Socks_Needle_Change_Details (Master_ID, Machine_ID, Needle_ID)  Values (" + TDt2.Rows[0][0] + " + " + D1 + ", " + LblMachine.Tag + ", " + Grid["Needle_Id", i].Value + ")";                                    
                                    Queries[Array_Index++] = "Insert into Socks_Machine_Planning_Master (Year, Week, Needle_Id) Values (" + LblYear.Text + ", " + LblWeek.Text + ", " + Dt.Rows[i]["Needle_ID"].ToString() + ")";
                                    
                                    D1 = D1 + 1;
                                }
                            }
                            else
                            {
                                Dt1 = new DataTable();
                                MyBase.Load_Data("Select Rowid, Year, Week, Effect_From from Socks_Needle_Change_Master Where Year = " + LblYear.Text + " and Week = " + LblWeek.Text + " And Effect_From = '" + LblDate.Tag.ToString() + "' ", ref Dt1);
                                if (Dt1.Rows.Count > 0)
                                {
                                    Int64 Code = Convert.ToInt16(Dt1.Rows[0]["RowID"].ToString());
                                    Queries[Array_Index++] = "Update Socks_Needle_Change_Master Set Year =  " + LblYear.Text + " , Week =  " + LblWeek.Text + " Where Rowid = " + Code;
                                    Queries[Array_Index++] = "Delete From Socks_Needle_Change_Details Where Master_id = " + Code + " And Machine_ID = " + LblMachine.Tag;
                                    Queries[Array_Index++] = "Insert into Socks_Needle_Change_Details (Master_ID, Machine_ID, Needle_ID) Values (" + Code + ", " + LblMachine.Tag + ", " + Grid["Needle_Id", i].Value + ")";
                                }
                            }
                            
                        }
                    }

                    DataTable Tdt = new DataTable();
                    if (Dt.Rows[i]["DetailID"].ToString() != "")
                    {
                        MyBase.Load_Data("Select Master_ID RowID From Socks_Machine_Production_Details Where Rowid = " + Dt.Rows[i]["DetailID"].ToString(), ref Tdt);

                        if (Tdt.Rows.Count > 0)
                        {
                            Master_ID = Convert.ToInt64(Tdt.Rows[0]["RowID"]);
                        }
                    }
                    else
                    {
                        Master_ID = 0;
                    }

                    if (Master_ID > 0)
                    {
                        //Queries[Array_Index++] = "Update Socks_Machine_Production_Master Set Plan_Qty = " + LblPlan.Text + ", Assign_Qty = " + LblAssign.Text + ", Emplno = " + TxtEmployee.Tag.ToString() + ", Planned_Seconds = " + LBlPlannedSeconds.Text + " Where RowiD = " + Master_ID;
                        if (D2 == 0)
                        {
                            Queries[Array_Index++] = "Update Socks_Machine_Production_Master Set Plan_Qty = " + LblPlan.Text + ", Assign_Qty = " + LblAssign.Text + ", Emplno = " + Dt.Rows[i]["Emplno"].ToString() + ", Planned_Seconds = " + LBlPlannedSeconds.Text + " Where RowiD = " + Master_ID;
                            //Queries[Array_Index++] = "Delete From Socks_Machine_Production_Details Where Master_ID = " + Master_ID;
                            D2++;
                        }
                        Queries[Array_Index++] = "Delete From Socks_Machine_Production_Details Where Master_ID = " + Master_ID;
                        Queries[Array_Index++] = "Insert Into Socks_Machine_Production_Details (Master_ID, Order_No, OrderColorID, Qty) Values (" + Master_ID + ", '" + Dt.Rows[i]["Order_No"].ToString() + "', " + Dt.Rows[i]["OrderColorID"].ToString() + ", " + Dt.Rows[i]["Qty"].ToString() + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Socks_Machine_production_Master (Entry_Date, Year, Week, Shift, Needle_ID, Machine_ID, Order_No, Plan_Qty, Prod_Qty, Assign_Qty, Emplno, Planned_Seconds) Values ('" + LblDate.Tag.ToString() + "', " + LblYear.Text + ", " + LblWeek.Text + ", " + LblShift.Text + ", " + Dt.Rows[i]["Needle_ID"].ToString() + ", " + LblMachine.Tag.ToString() + ", '" + Dt.Rows[i]["Order_No"].ToString() + "', " + LblPlan.Text + ", 0, " + LblAssign.Text + ", " + Dt.Rows[i]["Emplno"].ToString() + ", " + LBlPlannedSeconds.Text + "); ";
                        Queries[Array_Index++] = "Insert Into Socks_Machine_Production_Details (Master_ID, Order_No, OrderColorID, Qty) Values (" + TDt1.Rows[0][0] + " + " + D + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value + ", " + Grid["Qty", i].Value + ")";
                        D = D + 1;
                    }
                }

                if (listBox1.Items.Count > 0)
                {
                    for (int l = 0; l < listBox1.Items.Count; l++)
                    {
                        Queries[Array_Index++] = " Delete From Socks_Machine_Production_Details Where Rowid = " + listBox1.Items[l] + "";
                    }
                }

                if (listBox2.Items.Count > 0)
                {
                    for (int l = 0; l < listBox2.Items.Count; l++)
                    {
                        Queries[Array_Index++] = " Delete From Socks_Machine_Production_Master Where Rowid = " + listBox2.Items[l] + "";
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (LblNeedle.Text != Dt.Rows[i]["Needle"].ToString() && LblNeedle.Tag != Dt.Rows[i]["Needle_ID"])
                    {
                        DataTable Tdt10 = new DataTable();
                        MyBase.Load_Data("Select RowId From Socks_Machine_Planning_Master where Year = " + LblYear.Text + " and Week = " + LblWeek.Text + " and Needle_Id = " + Dt.Rows[i]["Needle_ID"].ToString() + "", ref Tdt10);
                        if (Tdt10.Rows.Count > 0)
                        {
                            Master_ID_Plan = Convert.ToInt64(Tdt10.Rows[0]["RowID"]);
                        }
                       
                        //if (Master_ID_Plan  > 0)
                        //{
                        //    Queries[Array_Index++] = "Update Socks_Machine_Planning_Master Set EDate = GetDate(), EntrySystem = Host_Name() where RowID = " + Master_ID;
                        //    Queries[Array_Index++] = "Delete From Socks_Machine_Planning_Details Where Master_ID = " + Master_ID_Plan;
                        //}
                        //else
                        //{
                        //    Queries[Array_Index++] = "Insert into Socks_Machine_Planning_Master (Year, Week, Needle_Id) Values (" + LblYear.Text + ", " + LblWeek.Text + ", " + Dt.Rows[i]["Needle"].ToString() + ")";
                        //}              
                        if (Master_ID_Plan == 0)
                        {
                             Queries[Array_Index++] = "Insert into Socks_Machine_Planning_Master (Year, Week, Needle_Id) Values (" + LblYear.Text + ", " + LblWeek.Text + ", " + Dt.Rows[i]["Needle_ID"].ToString() + ")";
                        }
                        for (i = 0; i <= Dt.Rows.Count - 1; i++)
                        {
                            if (Master_ID_Plan > 0)
                            {
                                Queries[Array_Index++] = "Insert Into Socks_Machine_Planning_Details (Master_ID, Machine_ID, Order_No, Target_Qty, Plan_Qty, Actual_Qty, Cycle_Time, Plan_Mins, Cycle_Seconds) Values (" + Master_ID_Plan + ", " + LblMachine.Tag + ", '" + Dt.Rows[i]["Order_No"].ToString() + "', " + Dt.Rows[i]["Qty"].ToString() + ", " + Dt.Rows[i]["Needle_ID"].ToString() + ", 0, '" + Dt.Rows[i]["CyTime"].ToString() + "', " + LBlPlannedSeconds.Text + ", " + LBlPlannedSeconds.Text + ")";
                            }
                            else
                            {
                                Queries[Array_Index++] = "Insert Into Socks_Machine_Planning_Details (Master_ID, Machine_ID, Order_No, Target_Qty, Plan_Qty, Actual_Qty, Cycle_Time, Plan_Mins, Cycle_Seconds) Values (" + TDt3.Rows[0][0] + ", " + LblMachine.Tag + ", '" + Dt.Rows[i]["Order_No"].ToString() + "', " + Dt.Rows[i]["Qty"].ToString() + ", " + Dt.Rows[i]["Needle_ID"].ToString() + ", 0, '" + Dt.Rows[i]["CyTime"].ToString() + "', " + LBlPlannedSeconds.Text + ", " + LBlPlannedSeconds.Text + ")";
                            }
                        }


                    }
                }
             
                if (Master_ID > 0)
                {
                    MyBase.Run_Identity(false, Queries);
                }
                else
                {
                    MyBase.Run_Identity(false, Queries);
                }
                Calc_Utilization();
                MessageBox.Show("Saved ...!", "Gainup");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                Grid_Data();
                LblOrderNo.Text = LblWeek.Tag.ToString();
                LblOrderNo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Sure to Close ...!", "Gainup", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Calc_Utilization();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmMachineProduction_OrderWise_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "LblOrderNo")
                    {
                        Grid.CurrentCell = Grid["Needle", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtEmployee")
                    {
                        
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

        private void FrmMachineProduction_OrderWise_KeyPress(object sender, KeyPressEventArgs e)
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

        private void myDataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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
            Int32 Qty = 0;
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Qty"].Index)
                {
                    if (Grid["Qty", Grid.CurrentCell.RowIndex].Value.ToString() == string.Empty || Grid["Qty", Grid.CurrentCell.RowIndex].Value.ToString() == "0")
                    {
                        Update_Seconds();
                        Update_AssignQty();

                        Qty = Convert.ToInt32(LblFreeSeconds.Text) / Convert.ToInt32(Grid["CySecd", Grid.CurrentCell.RowIndex].Value);

                        if ((Convert.ToDouble(LblPlan.Text) - Convert.ToDouble(LblAssign.Text)) > Convert.ToDouble(Grid["Balance", Grid.CurrentCell.RowIndex].Value))
                        {
                            if (Dr != null )
                            {
                                if (Qty > Convert.ToInt32(Dr["Qty"]))
                                {
                                    Grid["Qty", Grid.CurrentCell.RowIndex].Value = Dr["Qty"].ToString();
                                    Txt.Text = Dr["Qty"].ToString();
                                }
                                else
                                {
                                    Grid["Qty", Grid.CurrentCell.RowIndex].Value = Qty.ToString();
                                    Txt.Text = Qty.ToString();
                                }
                            }
                            else
                            {
                                Grid["Qty", Grid.CurrentCell.RowIndex].Value = Qty.ToString();
                                Txt.Text = Qty.ToString();
                            }
                        }
                        else
                        {
                            if (Qty > (Convert.ToDouble(LblPlan.Text) - Convert.ToDouble(LblAssign.Text)))
                            {
                                Grid["Qty", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(LblPlan.Text) - Convert.ToDouble(LblAssign.Text));
                                Txt.Text = Convert.ToString(Convert.ToDouble(LblPlan.Text) - Convert.ToDouble(LblAssign.Text));
                            }
                            else
                            {
                                Grid["Qty", Grid.CurrentCell.RowIndex].Value = Qty.ToString();
                                Txt.Text = Qty.ToString();
                            }
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Needle"].Index)
                {

                    if (Grid["Needle", Grid.CurrentCell.RowIndex].Value == null || Grid["Needle", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Needle", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        if (Grid.CurrentCell.RowIndex == 0)
                        {
                            Txt.Text = LblNeedle.Text;
                            Grid["Needle", Grid.CurrentCell.RowIndex].Value = LblNeedle.Text;
                            Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value = LblNeedle.Tag;
                        }
                        else
                        {
                            Txt.Text = Grid["Needle", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Grid["Needle", Grid.CurrentCell.RowIndex].Value = Grid["Needle", Grid.CurrentCell.RowIndex - 1].Value;
                            Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value = Grid["Needle_ID", Grid.CurrentCell.RowIndex - 1].Value;
                        }
                    }
                }

                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index)
                {

                    if (Grid["Operator", Grid.CurrentCell.RowIndex].Value == null || Grid["Operator", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Operator", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        if (Grid.CurrentCell.RowIndex > 0)
                        {
                            Txt.Text = Grid["Operator", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Grid["Operator", Grid.CurrentCell.RowIndex].Value = Grid["Operator", Grid.CurrentCell.RowIndex - 1].Value;
                            Grid["Emplno", Grid.CurrentCell.RowIndex].Value = Grid["Emplno", Grid.CurrentCell.RowIndex - 1].Value;
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                {

                    if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value == null || Grid["Order_No", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        if (Grid.CurrentCell.RowIndex == 0 )
                        {
                            if (Grid["Needle", 0].Value.ToString() == LblNeedle.Text)
                            {
                                Txt.Text = LblOrderNo.Text;
                                Grid["Order_No", Grid.CurrentCell.RowIndex].Value = LblOrderNo.Text;
                            }
                        }
                        else
                        {
                            if (Grid["Needle", Grid.CurrentCell.RowIndex].Value == Grid["Needle", Grid.CurrentCell.RowIndex - 1].Value && Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value == Grid["Needle_ID", Grid.CurrentCell.RowIndex - 1].Value)
                            {
                                Txt.Text = Grid["Order_No", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                                Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Grid["Order_No", Grid.CurrentCell.RowIndex - 1].Value;
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

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Qty"].Index)
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

        void Update_AssignQty()
        {
            try
            {
                LblAssign.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "Qty", "OrderColorID", "Sample")));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Update_Seconds()
        {
            try
            {
                LBlPlannedSeconds.Text = Planned_Seconds().ToString();
                LblFreeSeconds.Text = Free_Seconds().ToString();
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
                if (e.KeyCode == Keys.Down && Grid.CurrentCell.ColumnIndex == Grid.Columns["Sample"].Index)
                {

                    if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value == null || Grid["Order_No", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        MessageBox.Show("Invalid Order No ...!", "Gainup");
                        Grid.CurrentCell = Grid["Order_No", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                    if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value == "O")
                    {
                        //Commented On 04/Jun/2016 As per MD Instruction Only balance Sample Available For Selection
                        //Dr = Tool.Selection_Tool_Except_New_WOMDI("Sample", this, 500, 180, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select S1.Color Sample, S1.Size, S1.Bom_Qty BOM, Isnull(G1.Production, 0) Production, (S1.Bom_Qty - Isnull(G1.Production, 0)) Qty, S1.OrderColorID, Datediff(SECOND, 0, cast('00:' + S1.Cycle_Pair as time)) Cycle_Seconds, Convert(Varchar, Cast('00:' + S1.Cycle_Pair as DateTime), 108) Cycle_Time From Socks_Bom () S1 Left Join Get_Knit_Prod () G1 on S1.Order_No = G1.Order_No And S1.color = G1.Sample Where S1.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And S1.Needle_ID = " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and (S1.Bom_Qty - Isnull(G1.Production, 0)) > 0", String.Empty, 100, 80, 80, 80);

                        if (LblUnit == 1)
                        {
                            Dr = Tool.Selection_Tool_Except_New_WOMDI("Sample", this, 500, 180, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select A.Sample, A.Size, A.BOM, A.Production, A.Qty, A.OrderColorID, A.Cycle_Seconds, A.Cycle_Time From Get_Order_Sample_For_Produciton_planning_OLD( '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")A Left Join JOb_Order_Unit_Details_Samplewise()B On A.Sample = B.Sample_No Left Join VFit_Sample_Master C On B.sample_No = C.Sample_No Left Join Item D On C.SampleItemID = D.ItemID Where B.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.SupplierID = 71 And D.Item != 'GUSSET'", String.Empty, 100, 80, 80, 80);
                        }
                        else if (LblUnit == 2)
                        {
                            Dr = Tool.Selection_Tool_Except_New_WOMDI("Sample", this, 500, 180, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select A.Sample, A.Size, A.BOM, A.Production, A.Qty, A.OrderColorID, A.Cycle_Seconds, A.Cycle_Time From Get_Order_Sample_For_Produciton_planning_OLD( '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")A Left Join JOb_Order_Unit_Details_Samplewise()B On A.Sample = B.Sample_No Left Join VFit_Sample_Master C On B.sample_No = C.Sample_No Left Join Item D On C.SampleItemID = D.ItemID Where B.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.SupplierID = 72 And D.Item != 'GUSSET'", String.Empty, 100, 80, 80, 80);
                        }
                        else if (LblUnit == 3)
                        {
                            Dr = Tool.Selection_Tool_Except_New_WOMDI("Sample", this, 500, 180, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select A.Sample, A.Size, A.BOM, A.Production, A.Qty, A.OrderColorID, A.Cycle_Seconds, A.Cycle_Time From Get_Order_Sample_For_Produciton_planning_OLD( '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")A Left Join JOb_Order_Unit_Details_Samplewise()B On A.Sample = B.Sample_No Left Join VFit_Sample_Master C On B.sample_No = C.Sample_No Left Join Item D On C.SampleItemID = D.ItemID Where B.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.SupplierID = 74 And D.Item != 'GUSSET'", String.Empty, 100, 80, 80, 80);
                        }
                        else if (LblUnit == 4)
                        {
                            Dr = Tool.Selection_Tool_Except_New_WOMDI("Sample", this, 500, 180, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select A.Sample, A.Size, A.BOM, A.Production, A.Qty, A.OrderColorID, A.Cycle_Seconds, A.Cycle_Time From Get_Order_Sample_For_Produciton_planning_OLD( '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")A Left Join JOb_Order_Unit_Details_Samplewise()B On A.Sample = B.Sample_No Left Join VFit_Sample_Master C On B.sample_No = C.Sample_No Left Join Item D On C.SampleItemID = D.ItemID Where B.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.SupplierID = 75 And D.Item != 'GUSSET'", String.Empty, 100, 80, 80, 80);
                        }
                        else
                        {
                            Dr = Tool.Selection_Tool_Except_New_WOMDI("Sample", this, 500, 180, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select Sample, Size, BOM, Production, Qty, OrderColorID, Cycle_Seconds, Cycle_Time From Get_Order_Sample_For_Produciton_planning_OLD( '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")A Left Join JOb_Order_Unit_Details_Samplewise()B On A.Sample = B.Sample_No Left Join VFit_Sample_Master C On B.sample_No = C.Sample_No Left Join Item D On C.SampleItemID = D.ItemID Where B.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.SupplierID = 0 And D.Item != 'GUSSET'", String.Empty, 100, 80, 80, 80);
                        }
                    }
                    else
                    {
                        //Dr = Tool.Selection_Tool_Except_New_WOMDI("Sample", this, 500, 180, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select S1.Color Sample, S1.Size, S1.Bom_Qty BOM, Isnull(G1.Production, 0) Production, (S1.Bom_Qty - Isnull(G1.Production, 0)) Qty, S1.OrderColorID, Datediff(SECOND, 0, cast('00:' + (Case When Cast(Getdate() As Date) >= G2.EffectFrom Then Isnull(G2.New_Cycle_Pair, S1.Cycle_Pair) Else S1.Cycle_Pair End) as time)) Cycle_Seconds, Convert(Varchar, Cast('00:' + (Case When Cast(Getdate() As Date) >= G2.EffectFrom Then Isnull(G2.New_Cycle_Pair, S1.Cycle_Pair) Else S1.Cycle_Pair End) as DateTime), 108) Cycle_Time From Socks_Bom () S1 Left Join Get_Knit_Prod () G1 on S1.Order_No = G1.Order_No And S1.color = G1.Sample Left Join Get_Max_Cycle_Time() G2 On S1.OrderColorId = G2.OrderColorID Where S1.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And S1.Needle_ID = " + LblNeedle.Tag.ToString() + " and (S1.Bom_Qty - Isnull(G1.Production, 0)) > 0", String.Empty, 100, 80, 80, 80);
                        //Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select S1.Color Sample, S1.Size, S1.Bom_Qty BOM, Isnull(G1.Production, 0) Production, (S1.Bom_Qty - Isnull(G1.Production, 0)) Qty, S1.OrderColorID, Datediff(SECOND, 0, cast('00:' + (Case When Cast(Getdate() As Date) >= G2.EffectFrom Then Isnull(G2.New_Cycle_Pair, S1.Cycle_Pair) Else S1.Cycle_Pair End) as time)) Cycle_Seconds, Convert(Varchar, Cast('00:' + (Case When Cast(Getdate() As Date) >= G2.EffectFrom Then Isnull(G2.New_Cycle_Pair, S1.Cycle_Pair) Else S1.Cycle_Pair End) as DateTime), 108) Cycle_Time From Socks_Bom () S1 Left Join Get_Knit_Prod () G1 on S1.Order_No = G1.Order_No And S1.color = G1.Sample Left Join Get_Max_Cycle_Time() G2 On S1.OrderColorId = G2.OrderColorID Where S1.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And S1.Needle_ID = " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " and (S1.Bom_Qty - Isnull(G1.Production, 0)) > 0", String.Empty, 100, 80, 80, 80);
                        
                        //Commented On 04/Jun/2016 As per MD Instruction Only balance Sample Available For Selection
                        //Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select S1.Color Sample, S1.Size, S1.Bom_Qty BOM, Isnull(G1.Production, 0) Production, (S1.Bom_Qty - Isnull(G1.Production, 0)) Qty, S1.OrderColorID, Datediff(SECOND, 0, cast('00:' + (Case When Cast(Getdate() As Date) >= G2.EffectFrom Then Isnull(G2.New_Cycle_Pair, S1.Cycle_Pair) Else S1.Cycle_Pair End) as Datetime)) Cycle_Seconds, Convert(Varchar, Cast('00:' + (Case When Cast(Getdate() As Date) >= G2.EffectFrom Then Isnull(G2.New_Cycle_Pair, S1.Cycle_Pair) Else S1.Cycle_Pair End) as DateTime), 108) Cycle_Time From Socks_Bom () S1 Left Join Get_Knit_Prod () G1 on S1.Order_No = G1.Order_No And S1.color = G1.Sample Left Join Get_Max_Cycle_Time() G2 On S1.OrderColorId = G2.OrderColorID Where S1.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And S1.Needle_ID = " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " ", String.Empty, 100, 80, 80, 80);

                        //Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select Sample, Size, BOM, Production, Qty, OrderColorID, Cycle_Seconds, Cycle_Time From Get_Order_Sample_For_Produciton_planning('" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")", String.Empty, 100, 80, 80, 80);

                        MyBase.Run(" Exec Knit_Order_Sample_Tab_Insert");
                        MyBase.Run("Exec Knit_Prod_Tab_Insert");

                        if (LblUnit == 1)
                        {
                            //Commented On 30/Dec/2016 By Sakthi For Fast
                            //Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select A.Sample, A.Size, A.BOM, A.Production, A.Balance, A.Allocated_Machines, A.Allocated_Qty, A.Qty, A.OrderColorID, A.Cycle_Seconds, A.Cycle_Time From Get_Order_Sample_For_Produciton_planning_Allocation_Based( " + LblYear.Text + ", " + LblWeek.Text + ", '" + LblDate.Text + "', " + LblShift.Text + ", '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")A Left Join JOb_Order_Unit_Details_Samplewise()B On A.Sample = B.Sample_No Where B.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.SupplierID = 71 ", String.Empty, 100, 60, 60, 60, 60, 60, 60, 60);

                            Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select A.Sample, A.Size, A.BOM, A.Production, A.Balance, A.Allocated_Machines, A.Allocated_Qty, A.Qty, A.OrderColorID, A.Cycle_Seconds, A.Cycle_Time From Get_Order_Sample_For_Produciton_planning_Allocation_Based_New( " + LblYear.Text + ", " + LblWeek.Text + ", '" + LblDate.Text + "', " + LblShift.Text + ", '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")A Left Join JOb_Order_Unit_Details_Samplewise()B On A.Sample = B.Sample_No Left Join VFit_Sample_Master C On B.sample_No = C.Sample_No Left Join Item D On C.SampleItemID = D.ItemID Where B.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.SupplierID = 71 And D.Item != 'GUSSET'", String.Empty, 100, 60, 60, 60, 60, 60, 60, 60);
                        }
                        else if (LblUnit == 2)
                        {
                            //Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select A.Sample, A.Size, A.BOM, A.Production, A.Balance, A.Allocated_Machines, A.Allocated_Qty, A.Qty, A.OrderColorID, A.Cycle_Seconds, A.Cycle_Time From Get_Order_Sample_For_Produciton_planning_Allocation_Based( " + LblYear.Text + ", " + LblWeek.Text + ", '" + LblDate.Text + "', " + LblShift.Text + ", '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")A Left Join JOb_Order_Unit_Details_Samplewise()B On A.Sample = B.Sample_No Where B.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.SupplierID = 72 ", String.Empty, 100, 60, 60, 60, 60, 60, 60, 60);

                            Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select A.Sample, A.Size, A.BOM, A.Production, A.Balance, A.Allocated_Machines, A.Allocated_Qty, A.Qty, A.OrderColorID, A.Cycle_Seconds, A.Cycle_Time From Get_Order_Sample_For_Produciton_planning_Allocation_Based_New( " + LblYear.Text + ", " + LblWeek.Text + ", '" + LblDate.Text + "', " + LblShift.Text + ", '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")A Left Join JOb_Order_Unit_Details_Samplewise()B On A.Sample = B.Sample_No Left Join VFit_Sample_Master C On B.sample_No = C.Sample_No Left Join Item D On C.SampleItemID = D.ItemID Where B.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.SupplierID = 72 And D.Item != 'GUSSET'", String.Empty, 100, 60, 60, 60, 60, 60, 60, 60);
                        }
                        else if (LblUnit == 3)
                        {
                            //Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select A.Sample, A.Size, A.BOM, A.Production, A.Balance, A.Allocated_Machines, A.Allocated_Qty, A.Qty, A.OrderColorID, A.Cycle_Seconds, A.Cycle_Time From Get_Order_Sample_For_Produciton_planning_Allocation_Based( " + LblYear.Text + ", " + LblWeek.Text + ", '" + LblDate.Text + "', " + LblShift.Text + ", '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")A Left Join JOb_Order_Unit_Details_Samplewise()B On A.Sample = B.Sample_No Where B.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.SupplierID = 72 ", String.Empty, 100, 60, 60, 60, 60, 60, 60, 60);

                            Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select A.Sample, A.Size, A.BOM, A.Production, A.Balance, A.Allocated_Machines, A.Allocated_Qty, A.Qty, A.OrderColorID, A.Cycle_Seconds, A.Cycle_Time From Get_Order_Sample_For_Produciton_planning_Allocation_Based_New( " + LblYear.Text + ", " + LblWeek.Text + ", '" + LblDate.Text + "', " + LblShift.Text + ", '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")A Left Join JOb_Order_Unit_Details_Samplewise()B On A.Sample = B.Sample_No Left Join VFit_Sample_Master C On B.sample_No = C.Sample_No Left Join Item D On C.SampleItemID = D.ItemID Where B.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.SupplierID = 74 And D.Item != 'GUSSET'", String.Empty, 100, 60, 60, 60, 60, 60, 60, 60);
                        }
                        else if (LblUnit == 4)
                        {
                            //Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select A.Sample, A.Size, A.BOM, A.Production, A.Balance, A.Allocated_Machines, A.Allocated_Qty, A.Qty, A.OrderColorID, A.Cycle_Seconds, A.Cycle_Time From Get_Order_Sample_For_Produciton_planning_Allocation_Based( " + LblYear.Text + ", " + LblWeek.Text + ", '" + LblDate.Text + "', " + LblShift.Text + ", '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")A Left Join JOb_Order_Unit_Details_Samplewise()B On A.Sample = B.Sample_No Where B.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.SupplierID = 72 ", String.Empty, 100, 60, 60, 60, 60, 60, 60, 60);

                            Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select A.Sample, A.Size, A.BOM, A.Production, A.Balance, A.Allocated_Machines, A.Allocated_Qty, A.Qty, A.OrderColorID, A.Cycle_Seconds, A.Cycle_Time From Get_Order_Sample_For_Produciton_planning_Allocation_Based_New( " + LblYear.Text + ", " + LblWeek.Text + ", '" + LblDate.Text + "', " + LblShift.Text + ", '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")A Left Join JOb_Order_Unit_Details_Samplewise()B On A.Sample = B.Sample_No Left Join VFit_Sample_Master C On B.sample_No = C.Sample_No Left Join Item D On C.SampleItemID = D.ItemID Where B.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.SupplierID = 75 And D.Item != 'GUSSET'", String.Empty, 100, 60, 60, 60, 60, 60, 60, 60);
                        }
                        else 
                        {
                            Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", "Select A.Sample, A.Size, A.BOM, A.Production, A.Balance, A.Allocated_Machines, A.Allocated_Qty, A.Qty, A.OrderColorID, A.Cycle_Seconds, A.Cycle_Time From Get_Order_Sample_For_Produciton_planning_Allocation_Based( " + LblYear.Text + ", " + LblWeek.Text + ", '" + LblDate.Text + "', " + LblShift.Text + ", '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "', " + Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value.ToString() + ")A Left Join JOb_Order_Unit_Details_Samplewise()B On A.Sample = B.Sample_No Left Join VFit_Sample_Master C On B.sample_No = C.Sample_No Left Join Item D On C.SampleItemID = D.ItemID Where B.Order_No = '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.SupplierID = 0 And D.Item != 'GUSSET'", String.Empty, 100, 60, 60, 60, 60, 60, 60, 60);
                        }
                    }
                    if (Dr != null)
                    {
                        MyBase.Row_Number(ref Grid);
                        Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Dr["OrderColorID"].ToString();
                        Grid["Sample", Grid.CurrentCell.RowIndex].Value = Dr["Sample"].ToString();
                        Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                        Grid["BOM", Grid.CurrentCell.RowIndex].Value = Dr["BOM"].ToString();
                        Grid["CyTime", Grid.CurrentCell.RowIndex].Value = Dr["Cycle_Time"].ToString();
                        Grid["CySecd", Grid.CurrentCell.RowIndex].Value = Dr["Cycle_Seconds"].ToString();
                        LblCycleTime.Text = Dr["Cycle_Seconds"].ToString();

                        Grid["Production", Grid.CurrentCell.RowIndex].Value = Dr["Production"].ToString();
                        Grid["Balance", Grid.CurrentCell.RowIndex].Value = Dr["Qty"].ToString();

                        Update_Seconds();

                        Txt.Text = Dr["Sample"].ToString();
                    }
                }
                else if (e.KeyCode == Keys.Down && Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                {
                    // Commnet On 04/Jun/2016 As per MD Sir Instruction Only Balance Order For Selection
                    //Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select Distinct S1.Order_No, S1.Descr Descrpition From Socks_Bom () S1 Where S1.Needle = '" + Grid["Needle", Grid.CurrentCell.RowIndex].Value + "'", String.Empty, 150, 200);

                    MyBase.Run("Exec Knit_Prod_Tab_Insert");
                    MyBase.Run("Exec Knit_Order_Tab_Insert");
                    
                    if (LblUnit == 1)
                    {
                        //Commented On 30/Dec/2016 By Sakthi For Fast
                        //Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select Distinct A.Order_No, A.Descr Descrpition From Get_Sample_Production_Planning('" + Grid["Needle", Grid.CurrentCell.RowIndex].Value + "')A Left Join JOb_Order_Unit_Details() B On A.Order_No = B.Order_No Where B.SupplierID = 71 ", String.Empty, 150, 200);

                        Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select A.Order_No, Max(A.Descr) Descrpition From Socks_Order_Needle A Left Join JOb_Order_Unit_Details() B On A.Order_No = B.Order_No Where A.Needle = '" + Grid["Needle", Grid.CurrentCell.RowIndex].Value + "' And B.SupplierID = 71 Group BY A.Order_No Order BY A.Order_No ", String.Empty, 150, 200);
                    }
                    else if (LblUnit == 2)
                    {
                        //Commented On 30/Dec/2016 By Sakthi For Fast
                        //Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select Distinct A.Order_No, A.Descr Descrpition From Get_Sample_Production_Planning('" + Grid["Needle", Grid.CurrentCell.RowIndex].Value + "')A Left Join JOb_Order_Unit_Details() B On A.Order_No = B.Order_No Where B.SupplierID = 72 ", String.Empty, 150, 200);

                        Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select A.Order_No, Max(A.Descr) Descrpition From Socks_Order_Needle A Left Join JOb_Order_Unit_Details() B On A.Order_No = B.Order_No Where A.Needle = '" + Grid["Needle", Grid.CurrentCell.RowIndex].Value + "' And B.SupplierID = 72 Group BY A.Order_No Order BY A.Order_No ", String.Empty, 150, 200);
                    }
                    else if (LblUnit == 3)
                    {
                        //Commented On 30/Dec/2016 By Sakthi For Fast
                        //Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select Distinct A.Order_No, A.Descr Descrpition From Get_Sample_Production_Planning('" + Grid["Needle", Grid.CurrentCell.RowIndex].Value + "')A Left Join JOb_Order_Unit_Details() B On A.Order_No = B.Order_No Where B.SupplierID = 72 ", String.Empty, 150, 200);

                        Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select A.Order_No, Max(A.Descr) Descrpition From Socks_Order_Needle A Left Join JOb_Order_Unit_Details() B On A.Order_No = B.Order_No Where A.Needle = '" + Grid["Needle", Grid.CurrentCell.RowIndex].Value + "' And B.SupplierID = 74 Group BY A.Order_No Order BY A.Order_No ", String.Empty, 150, 200);
                    }
                    else if (LblUnit == 4)
                    {
                        //Commented On 30/Dec/2016 By Sakthi For Fast
                        //Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select Distinct A.Order_No, A.Descr Descrpition From Get_Sample_Production_Planning('" + Grid["Needle", Grid.CurrentCell.RowIndex].Value + "')A Left Join JOb_Order_Unit_Details() B On A.Order_No = B.Order_No Where B.SupplierID = 72 ", String.Empty, 150, 200);

                        Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select A.Order_No, Max(A.Descr) Descrpition From Socks_Order_Needle A Left Join JOb_Order_Unit_Details() B On A.Order_No = B.Order_No Where A.Needle = '" + Grid["Needle", Grid.CurrentCell.RowIndex].Value + "' And B.SupplierID = 75 Group BY A.Order_No Order BY A.Order_No ", String.Empty, 150, 200);
                    }
                    else
                    {
                        //Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select Distinct A.Order_No, A.Descr Descrpition From Get_Sample_Production_Planning('" + Grid["Needle", Grid.CurrentCell.RowIndex].Value + "')A Left Join JOb_Order_Unit_Details() B On A.Order_No = B.Order_No", String.Empty, 150, 200);
                    }
                    if (Dr != null)
                    {
                        Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                        Txt.Text = Dr["Order_No"].ToString();

                        Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = 0;
                        Grid["Sample", Grid.CurrentCell.RowIndex].Value = "";
                        Grid["Size", Grid.CurrentCell.RowIndex].Value = "";
                        Grid["BOM", Grid.CurrentCell.RowIndex].Value = 0;
                        Grid["CyTime", Grid.CurrentCell.RowIndex].Value = 0;
                        Grid["CySecd", Grid.CurrentCell.RowIndex].Value = 0;
                        LblCycleTime.Text = "";

                        Grid["Production", Grid.CurrentCell.RowIndex].Value = 0;
                        Grid["Balance", Grid.CurrentCell.RowIndex].Value = 0;
                    }
                }
                else if (e.KeyCode == Keys.Down && Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index)
                {
                    //Dr = Tool.Selection_Tool_WOMDI(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Employee", "Select Name, Tno, Emplno From Vaahini_ERP_Gainup.Dbo.Employeemas E1 Inner Join Vaahini_ERP_Gainup.Dbo.Depttype D1 on E1.Deptcode = D1.DeptCode and E1.COMPCODE = D1.compcode Where E1.compcode =2 and D1.deptCode = 82 and E1.tno not like '%Z' Union Select Name, Tno, Emplno from vaahini_erp_gainup.dbo.EMPLOYEEMAS Where Emplno In (10651)", String.Empty, 300, 80);

                    String Str1 = " Select Name, Tno, Emplno From Vaahini_ERP_Gainup.Dbo.Employeemas E1 Inner Join Vaahini_ERP_Gainup.Dbo.Depttype D1 on E1.Deptcode = D1.DeptCode and E1.COMPCODE = D1.compcode Where E1.tno not like '%Z' ";
                    
                    if (LblUnit == 1)
                    {
                        Str1 = Str1 + " And E1.compcode =2 and D1.deptCode = 82 And E1.Unit_Code = 1 ";
                    }
                    else if (LblUnit == 2)
                    {
                        Str1 = Str1 + " And E1.compcode =2 and D1.deptCode = 82 And E1.Unit_Code = 2 ";
                    }
                    else if (LblUnit == 3)
                    {
                        Str1 = Str1 + " And E1.compcode = 8 and D1.deptCode = 209 And E1.Unit_Code = 3 ";
                    }
                    else if (LblUnit == 4)
                    {
                        Str1 = Str1 + " And E1.compcode = 8 and D1.deptCode = 209 And E1.Unit_Code = 4 ";
                    }

                    Dr = Tool.Selection_Tool_WOMDI(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Employee", Str1, String.Empty, 300, 80);

                    if (Dr != null)
                    {
                        Grid["Operator", Grid.CurrentCell.RowIndex].Value = Dr["Name"].ToString();
                        Txt.Text = Dr["Name"].ToString();
                        Grid["Emplno", Grid.CurrentCell.RowIndex].Value = Dr["Emplno"].ToString();
                    }
                }
                else if (e.KeyCode == Keys.Down && Grid.CurrentCell.ColumnIndex == Grid.Columns["Needle"].Index)
                {
                    Dr = Tool.Selection_Tool_WOMDI(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Needle", "select Name Needle, RowID Needle_ID from VFit_Sample_Needle_Master where Active = 'Y' Order By Name ", String.Empty, 300, 80);
                    if (Dr != null)
                    {
                        Grid["Needle", Grid.CurrentCell.RowIndex].Value = Dr["Needle"].ToString();
                        Txt.Text = Dr["Needle"].ToString();
                        Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value = Dr["Needle_ID"].ToString();
                        if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value != null && Grid["Order_No", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Grid["Order_No", Grid.CurrentCell.RowIndex].Value = "";
                        }
                        if (Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value != null && Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = 0;
                        }
                        if (Grid["Sample", Grid.CurrentCell.RowIndex].Value != null && Grid["Sample", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Grid["Sample", Grid.CurrentCell.RowIndex].Value = "";
                        }
                        if (Grid["Size", Grid.CurrentCell.RowIndex].Value != null && Grid["Size", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Grid["Size", Grid.CurrentCell.RowIndex].Value = "";
                        }
                        if (Grid["BOM", Grid.CurrentCell.RowIndex].Value != null && Grid["BOM", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["BOM", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Grid["BOM", Grid.CurrentCell.RowIndex].Value = 0;
                        }
                        if (Grid["CyTime", Grid.CurrentCell.RowIndex].Value != null && Grid["CyTime", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["CyTime", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Grid["CyTime", Grid.CurrentCell.RowIndex].Value = 0;
                        }
                        if (Grid["CySecd", Grid.CurrentCell.RowIndex].Value != null && Grid["CySecd", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["CySecd", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Grid["CySecd", Grid.CurrentCell.RowIndex].Value = 0;
                        }
                        if (LblCycleTime.Text != null && LblCycleTime.Text != String.Empty)
                        {
                            LblCycleTime.Text = "";
                        }
                        if (Grid["Production", Grid.CurrentCell.RowIndex].Value != null && Grid["Production", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["Production", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Grid["Production", Grid.CurrentCell.RowIndex].Value = 0;
                        }
                        if (Grid["Balance", Grid.CurrentCell.RowIndex].Value != null && Grid["Balance", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["Balance", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Grid["Balance", Grid.CurrentCell.RowIndex].Value = 0;
                        }
                        if (Grid["Operator", Grid.CurrentCell.RowIndex].Value != null && Grid["Operator", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["Operator", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Grid["Operator", Grid.CurrentCell.RowIndex].Value = "";
                        }
                        if (Grid["Emplno", Grid.CurrentCell.RowIndex].Value != null && Grid["Emplno", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["Emplno", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Grid["Emplno", Grid.CurrentCell.RowIndex].Value = 0;
                        }
                        if (Grid["Qty", Grid.CurrentCell.RowIndex].Value != null && Grid["Qty", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["Qty", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Grid["Qty", Grid.CurrentCell.RowIndex].Value = 0;
                        }
                    }
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

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Qty"].Index)
                    {
                        Update_AssignQty();
                        MyBase.Row_Number(ref Grid);
                        //if (Convert.ToInt32(Grid["Balance", Grid.CurrentCell.RowIndex].Value) < Convert.ToInt32(Grid["Qty", Grid.CurrentCell.RowIndex].Value))
                        //{
                        //    e.Handled = true;
                        //    MessageBox.Show("Invalid Assign Qty ...!", "Gainup");
                        //    Grid.CurrentCell = Grid["Qty", Grid.CurrentCell.RowIndex];
                        //    Grid.Focus();
                        //    Grid.BeginEdit(true);
                        //    return;
                        //}
                        Update_Seconds();
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
                //MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentCell.RowIndex);
                Update_AssignQty();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GBMain_Enter(object sender, EventArgs e)
        {

        }

        private void Grid_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (Dt != null && Dt.Rows.Count > 0 && Grid.CurrentCell != null)
                {
                    LblCycleTime.Text = Grid["CySecd", Grid.CurrentCell.RowIndex].Value.ToString();
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
                    BtnOk.Focus(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                //MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentCell.RowIndex);
                if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if ((Grid["DetailID", Grid.CurrentCell.RowIndex].Value != null && Grid["DetailID", Grid.CurrentCell.RowIndex].Value != DBNull.Value) && Grid["Master_ID", Grid.CurrentCell.RowIndex].Value != null && Grid["Master_ID", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                        {
                            listBox1.Items.Add(Grid["DetailID", Grid.CurrentCell.RowIndex].Value.ToString());
                            listBox2.Items.Add(Grid["Master_ID", Grid.CurrentCell.RowIndex].Value.ToString());
                            Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                        }
                        else
                        {
                            Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }    
        }
    }
}