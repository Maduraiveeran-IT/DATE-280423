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
    public partial class Frm_Machine_Production_Import_New : Form
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
        Boolean Status_Flag = false;

        Int32 UnitCode;

        String Str;

        public Int64 UnitCode_New;

        public Frm_Machine_Production_Import_New(DateTime FDate, DateTime TDate, Int16 FShift, Int16 TShift, String FNeedle, String TNeedle, Int32 FYear, Int32 TYear, Int32 FWeek, Int32 TWeek, Int32 Unit)
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
            UnitCode = Unit;

            if(UnitCode == 1)
            {
                UnitCode_New = 71;
            }
            else if (UnitCode == 2)
            {
                UnitCode_New = 72;
            }
            else if (UnitCode == 3)
            {
                UnitCode_New = 74;
            }
            else if (UnitCode == 4)
            {
                UnitCode_New = 75;
            }
        }

        private void Frm_Machine_Production_Import_New_Load(object sender, EventArgs e)
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

        private void Frm_Machine_Production_Import_New_KeyDown(object sender, KeyEventArgs e)
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
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtOrder")
                    {
                        OrderNo_Selection1();
                    }
                    else if (this.ActiveControl.Name == "TxtSample")
                    {
                        Sample_Selection1();
                    }
                    else if (this.ActiveControl.Name == "TxtOperator")
                    {
                        Operator_Selection1();
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
                //MyBase.Run("Exec Knit_Prod_Tab_Insert");
                //MyBase.Run("Exec Knit_Order_Tab_Insert");
                //MyBase.Run("Exec Knit_Order_Sample_Tab_Insert");

                Tdt = new DataTable();
                MyBase.Load_Data("Select RowID From VFit_Sample_Needle_Master Where Name = '" + TxtFNeedle.Text + "'", ref Tdt);
                TxtFNeedle.Tag = Tdt.Rows[0][0].ToString();

                Tdt = new DataTable();
                MyBase.Load_Data("Select RowID From VFit_Sample_Needle_Master Where Name = '" + TxtTNeedle.Text + "'", ref Tdt);
                TxtTNeedle.Tag = Tdt.Rows[0][0].ToString();

                //Str = " Select SNo, Needle_ID, Needle, Machine_ID, Machine, Order_No, OrderColorID, Sample, Size, Bom, Prod, Bal_Qty, Plan_Qty, Prod_Qty, Assign_Qty, Emplno, Operator, Planned_Seconds, Qty, Order_No2, 'N' T, Mode From Prod_Plan_Import_New('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', " + TxtFShift.Text + ", " + TxtFNeedle.Tag + ", " + TxtFYear.Text + ", " + TxtFWeek.Text + ", " + UnitCode + ")";

                Str = " Exec Import_Grid_Select_Proc " + UnitCode_New + ", " + TxtFYear.Text + ", " + TxtFWeek.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', " + TxtFShift.Text + ", " + TxtFNeedle.Tag + "";

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Needle_ID", "Needle", "Machine_ID", "OrderColorID", "Plan_Qty", "Emplno", "Planned_Seconds", "Qty", "Order_No2", "Prod_Qty", "T", "Mode");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Order_No", "Sample", "Assign_Qty", "Operator");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);

                DataGridViewCheckBoxColumn Check = new DataGridViewCheckBoxColumn();
                Check.HeaderText = "Status";
                Check.Name = "Status";
                Check.ValueType = typeof(String);
                Check.Visible = true;
                Check.ReadOnly = false;
                Grid.Columns.Insert(1, Check);
                Status_Flag = true;
                Grid.Columns["Status"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;

                MyBase.Grid_Width(ref Grid, 40, 60, 70, 105, 80, 80, 70, 70, 70, 70, 110);

                Grid.RowHeadersWidth = 20;

                Grid.Columns["Prod"].HeaderText = "Produced";

                Grid.Columns["BOM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Prod"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Bal_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Assign_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                MyBase.Row_Number(ref Grid);

                Fill_Bom_Check();
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
                    Txt.GotFocus += new EventHandler(Txt_GotFocus);
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
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
                //MyBase.Run("Exec Knit_Prod_Tab_Insert");
                //MyBase.Run("Exec Knit_Order_Tab_Insert");

                String Str3;
                Str3 = " Exec Import_Order_Select_Proc " + UnitCode_New + ", " + TxtFYear.Text + ", " + TxtFWeek.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', " + TxtFShift.Text + ", " + TxtFNeedle.Tag + "";
                Dr = Tool.Selection_Tool_WOMDI(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Order_No", Str3, String.Empty, 150, 200);
                if (Dr != null)
                {
                    Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                    Txt.Text = Dr["Order_No"].ToString();
                    Grid["Order_No2", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
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

        void OrderNo_Selection1()
        {
            try
            {
                //MyBase.Run("Exec Knit_Prod_Tab_Insert");
                //MyBase.Run("Exec Knit_Order_Tab_Insert");

                String Str3;
                Str3 = " Exec Import_Order_Select_Proc " + UnitCode_New + ", " + TxtFYear.Text + ", " + TxtFWeek.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', " + TxtFShift.Text + ", " + TxtFNeedle.Tag + "";
                Dr = Tool.Selection_Tool_WOMDI(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Order_No", Str3, String.Empty, 150, 200);
                if (Dr != null)
                {
                    TxtOrder.Text = Dr["Order_No"].ToString();
                    //Txt.Text = Dr["Order_No"].ToString();
                    TxtSample.Text = "";
                    TxtSize.Text = "";
                    TxtBom.Text = "";
                    TxtProd.Text = "";
                    TxtBal.Text = "";
                    TxtAssign.Text = "";
                    TxtAssign.Tag = 0;
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
                if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    String Str3;

                    Str3 = " Exec Import_Order_Sample_Select_Proc " + UnitCode_New + ", " + TxtFYear.Text + ", " + TxtFWeek.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', " + TxtFShift.Text + ", " + TxtFNeedle.Tag + ", '" + Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() + "' ";
                    Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", Str3, String.Empty, 100, 80, 80, 80);
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
                        for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                        {
                            if (Dt.Rows[i]["Sample"].ToString() != String.Empty)
                            {
                                if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value == Dt.Rows[i]["Order_No"].ToString() && Grid["Sample", Grid.CurrentCell.RowIndex].Value == Dt.Rows[i]["Sample"].ToString() && Grid["Size", Grid.CurrentCell.RowIndex].Value == Dt.Rows[i]["Size"].ToString())
                                {
                                    Grid["T", i].Value = "N";
                                }
                            }
                        }
                        Fill_Bom_Check();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Sample_Selection1()
        {
            try
            {

                if (TxtOrder.Text.ToString() != String.Empty)
                {
                    String Str3;

                    Str3 = " Exec Import_Order_Sample_Select_Proc " + UnitCode_New + ", " + TxtFYear.Text + ", " + TxtFWeek.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', " + TxtFShift.Text + ", " + TxtFNeedle.Tag + ", '" + TxtOrder.Text.ToString() + "' ";
                    
                    Dr = Tool.Selection_Tool_WOMDI(this, 500, 180, SelectionTool_Class.ViewType.NormalView, "Select Sample", Str3, String.Empty, 100, 80, 80, 80);
                    if (Dr != null)
                    {
                        TxtSample.Text = Dr["Sample"].ToString();
                        //Txt.Text = Dr["Sample"].ToString();
                        TxtSize.Text = Dr["Size"].ToString();
                        TxtBom.Text = Dr["Bom"].ToString();
                        TxtProd.Text = Dr["Prod"].ToString();
                        TxtBal.Text = Dr["Bal_Qty"].ToString();
                        TxtAssign.Text = Dr["Assign_Qty"].ToString();
                        TxtAssign.Tag = Dr["Planned_Seconds"].ToString();
                        TxtSample.Tag = Dr["OrderColorID"].ToString();
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
                    Str3 = " Select Name, Tno, Emplno From Vaahini_ERP_Gainup.Dbo.Employeemas E1 Inner Join Vaahini_ERP_Gainup.Dbo.Depttype D1 on E1.Deptcode = D1.DeptCode and E1.COMPCODE = D1.compcode Where E1.compcode In (2, 8) and D1.deptCode In (82, 209) and E1.tno not like '%Z' Union Select Name, Tno, Emplno from vaahini_erp_gainup.dbo.EMPLOYEEMAS Where Emplno In(10651)";
                    Dr = Tool.Selection_Tool_WOMDI(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Employee", Str3, String.Empty, 300, 80);
                    if (Dr != null)
                    {
                        Grid["Operator", Grid.CurrentCell.RowIndex].Value = Dr["Name"].ToString() + " - " + Dr["Tno"].ToString();
                        Txt.Text = Dr["Name"].ToString() + " - " + Dr["Tno"].ToString();
                        Grid["Emplno", Grid.CurrentCell.RowIndex].Value = Dr["Emplno"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Operator_Selection1()
        {
            try
            {
                String Str3;
                Str3 = " Select Name, Tno, Emplno From Vaahini_ERP_Gainup.Dbo.Employeemas E1 Inner Join Vaahini_ERP_Gainup.Dbo.Depttype D1 on E1.Deptcode = D1.DeptCode and E1.COMPCODE = D1.compcode Where E1.compcode In (2, 8) and D1.deptCode In (82, 209) and E1.tno not like '%Z' Union Select Name, Tno, Emplno from vaahini_erp_gainup.dbo.EMPLOYEEMAS Where Emplno In(10651)";
                Dr = Tool.Selection_Tool_WOMDI(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Employee", Str3, String.Empty, 300, 80);
                if (Dr != null)
                {
                    TxtOperator.Text = Dr["Name"].ToString() + " - " + Dr["Tno"].ToString();
                    TxtOperator.Tag = Dr["Emplno"].ToString();
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
                        if (j != 1)
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
                }

                String[] Queries = new String[(Dt.Rows.Count * 2) + 5];
                Int32 Array_Index = 0;
                Int64 Master_ID = 0;

                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select Max(RowID)+1 RowID From Socks_Machine_Production_Master", ref Tdt);

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
                                Queries[Array_Index++] = "Insert Into Socks_Machine_Production_Details (Master_ID, Order_No, OrderColorID, Qty) Values (@@IDENTITY + " + E + ", '" + Dt.Rows[j]["Order_No"].ToString() + "', " + Dt.Rows[j]["OrderColorID"].ToString() + ", " + Dt.Rows[j]["Assign_Qty"].ToString() + ")";
                                Dt.Rows[j]["Mode"] = "Y";
                            }
                        }
                        E = E + 1;
                    }
                }

                MyBase.Run_Identity(false, Queries);

                MessageBox.Show("Saved ...!", "Gainup");
                this.Close();
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

        private void BtnReplace_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtOrder.Text.ToString() != "" && TxtSample.Text.ToString() != "" && TxtSize.Text.ToString() != "" && TxtBom.Text.ToString() != "" && TxtProd.Text.ToString() != "" && TxtBal.Text.ToString() != "" && TxtAssign.Text.ToString() != "")
                {
                    if (Dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                        {
                            if (Grid["Status", i].Value != null && Grid["Status", i].Value != DBNull.Value && Grid["Status", i].Value.ToString().ToUpper() == "true".ToUpper())
                            {

                                Grid["Order_No", i].Value = TxtOrder.Text.ToString();
                                //Txt.Text = TxtOrder.Text.ToString();
                                Grid["Sample", i].Value = TxtSample.Text.ToString();
                                Grid["Size", i].Value = TxtSize.Text.ToString();
                                Grid["Bom", i].Value = TxtBom.Text.ToString();
                                Grid["Prod", i].Value = TxtProd.Text.ToString();
                                Grid["Bal_Qty", i].Value = TxtBal.Text.ToString();
                                Grid["Assign_Qty", i].Value = TxtAssign.Text.ToString();
                                Grid["Planned_Seconds", i].Value = TxtAssign.Tag.ToString();
                                Grid["OrderColorID", i].Value = TxtSample.Tag.ToString();
                                Grid["Status", i].Value = "FALSE";
                                Grid["T", i].Value = "N";
                                Grid["Order_No2", i].Value = TxtOrder.Text.ToString();
                            }
                        }
                        Fill_Bom_Check();
                        TxtOrder.Text = "";
                        TxtSample.Text = "";
                        TxtSize.Text = "";
                        TxtBom.Text = "";
                        TxtProd.Text = "";
                        TxtBal.Text = "";
                        TxtAssign.Text = "";
                        TxtAssign.Tag = "";
                        TxtSample.Tag = "";
                    }
                    else
                    {
                        MessageBox.Show("Machine Details Not Available ...!", "Gainup");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Pls Select necessary Details To Replace ...!", "Gainup");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Fill_Bom_Check()
        {
            try
            {
                Int64 Bal_Qty = 0;
                Int64 Assign_Qty = 0;
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Bal_Qty = 0;
                    Assign_Qty = 0;
                    if (Dt.Rows[i]["Order_No"].ToString() != String.Empty && Dt.Rows[i]["T"].ToString() != "Y")
                    {
                        Bal_Qty = Convert.ToInt64(Dt.Rows[i]["Bal_Qty"].ToString());
                        for (int j = i; j <= Dt.Rows.Count - 1; j++)
                        {
                            if (Dt.Rows[j]["Order_No"].ToString() != String.Empty)
                            {
                                if (Dt.Rows[i]["Order_No"].ToString() == Dt.Rows[j]["Order_No"].ToString() && Dt.Rows[i]["Sample"].ToString() == Dt.Rows[j]["Sample"].ToString() && Dt.Rows[i]["OrderColorID"].ToString() == Dt.Rows[j]["OrderColorID"].ToString())
                                {
                                    Assign_Qty = Assign_Qty + Convert.ToInt64(Dt.Rows[j]["Assign_Qty"].ToString());
                                    if (Bal_Qty < Assign_Qty)
                                    {
                                        Grid["Order_No", j].Value = "";
                                        Grid["Sample", j].Value = "";
                                        Grid["Size", j].Value = "";
                                        Grid["Bom", j].Value = 0;
                                        Grid["Prod", j].Value = 0;
                                        Grid["Bal_Qty", j].Value = 0;
                                        Grid["Assign_Qty", j].Value = 0;
                                        Grid["Planned_Seconds", j].Value = 0;
                                    }
                                    // Y For Already Checked
                                    Grid["T", j].Value = "Y";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtOperator.Text.ToString() != "")
                {
                    if (Dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                        {
                            if (Grid["Status", i].Value != null && Grid["Status", i].Value != DBNull.Value && Grid["Status", i].Value.ToString().ToUpper() == "true".ToUpper())
                            {
                                Grid["Operator", i].Value = TxtOperator.Text.ToString();
                                //Txt.Text = TxtOperator.Text.ToString();
                                Grid["Emplno", i].Value = TxtOperator.Tag.ToString();
                                Grid["Status", i].Value = "FALSE";
                            }
                        }
                        TxtOperator.Text = "";
                        TxtOperator.Tag = "";
                    }
                    else
                    {
                        MessageBox.Show("Operator Details Not Available ...!", "Gainup");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Pls Select necessary Details To Replace Operator...!", "Gainup");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
