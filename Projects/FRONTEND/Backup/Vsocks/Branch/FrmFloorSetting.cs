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
    public partial class FrmFloorSetting : Form, Entry 
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

        public FrmFloorSetting()
        {
            InitializeComponent();
        }

        private void FrmFloorSetting_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                Buffer_Table = "Set_" + Environment.MachineName.Replace("-", "") + "_" + MyParent.UserCode.ToString();
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

        private void FrmFloorSetting_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtShift")
                    {
                        Grid.CurrentCell = Grid["Machine", 0];
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

        private void FrmFloorSetting_KeyPress(object sender, KeyPressEventArgs e)
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

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                Set_Min_Max_Date(true);
                Buffer_Update = false;
                Grid_Data(false);

                if (MyBase.Check_Table(Buffer_Table) && MyBase.Get_RecordCount(Buffer_Table, String.Empty) > 0)
                {
                    if (MessageBox.Show("Buffer Details Available. Do you Want to Import ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        Grid_Data(true);
                    }
                }

                Buffer_Update = true;
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
                DtpDate1.Value = Convert.ToDateTime(Dr["EntryDate"]);
                TxtShift.Text = Dr["Shift"].ToString();
                TxtTiming.Text = Dr["Timing"].ToString();
                TxtShift.Tag = Dr["ShiftCode"].ToString();
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Setting - Edit", "Select F1.EntryNo, F1.ENtryDate, S1.Shiftcode2 Shift, F1.Timing, F1.ShiftCode, F1.Remarks, F1.RowID From Floor_Setting_Master F1 Left Join Socks_Shift () S1 on F1.ShiftCode = S1.Shiftcode Where F1.ENtryDate >='14-nov-2015' and F1.ENtryDate >= Dateadd (D, " + MyParent.User_Datelock + ", Cast(GetDate() as Date))", String.Empty, 80, 90, 70);
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

                if (TxtShift.Text == "" || TxtShift.Tag == "" || TxtShift.Text == null || TxtShift.Tag == null)
                {
                    MessageBox.Show("Invalid Shift ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtShift.Focus();
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

                    if (Grid["Production", i].Value == DBNull.Value || Grid["Production", i].Value.ToString() == String.Empty || Convert.ToDecimal(Grid["Production", i].Value) == 0)
                    {
                        MessageBox.Show(" ZERO is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Production", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }

                    if (Fill_BOM_Check(Grid["Order_No", i].Value.ToString(), Grid["Sample", i].Value.ToString(), Grid["Size", i].Value.ToString(), Convert.ToDecimal(Grid["Input_Qty", i].Value.ToString()), Grid["Input_From", i].Value.ToString()) < 0)
                    {
                        MessageBox.Show("Production Value Invalid  in Row " + (i + 1) + " For  '" + Grid["Order_No", i].Value.ToString() + "' ", "Gainup");
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
                Queries = new String[(Dt.Rows.Count * 3) + 5];

                TxtNo.Text = MyBase.MaxOnlyComp("Floor_Setting_Master", "EntryNo", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Floor_Setting_master (EntryNo, EntryDate, ShiftCode, Timing, Company_Code, EntryTime, EntrySystem, Remarks) Values (" + TxtNo.Text + ",'" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "','" + TxtShift.Tag.ToString() + "','" + TxtTiming.Text.ToString() + "'," + MyParent.CompCode + ",getdate(),Host_name(), '" + TxtRemarks.Text + "') ; Select Scope_Identity()";
                }
                else
                {
                    Queries[Array_Index++] = "Update Floor_Setting_Master Set ShiftCode = " + TxtShift.Tag.ToString() + ", Remarks = '" + TxtRemarks.Text + "' where RowID = " + Code;
                    //Queries[Array_Index++] = "Update F1 Set F1.KnitQty = F1.KnitQty - Isnull(F2.Production, 0) From Floor_Stock F1 Left join Floor_Knitting_DEtails F2 on F1.Order_No = F2.Order_No and F1.OrderColorID = F2.OrderColorID and F1.SizeID = F2.SizeID Where F2.MasterID = " + Code;
                    Queries[Array_Index++] = "Update F1 Set F1.SetQty = F1.SetQty - Isnull(F2.Production, 0) From Floor_Stock F1 Inner join (Select Order_No, OrderColorID, SizeID, Sum(Production) Production From Floor_Setting_Details Where MasterID = " + Code + " Group By Order_No, OrderColorID, SizeID) F2 on F1.Order_No = F2.Order_No and F1.OrderColorID = F2.OrderColorID and F1.SizeID = F2.SizeID ";
                    Queries[Array_Index++] = "Delete From Floor_Setting_Details where MAsterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New == true)
                    {
                        Queries[Array_Index++] = "Insert Into Floor_Setting_Details (MasterID, MachineID, Tag_No, Order_No, OrderColorID, ItemID, SizeID, BOMQty, production, Emplno_Operator, Input_From) Values (@@IDENTITY, '" + Grid["Machine", i].Value.ToString() + "','" + Grid["Tag_No", i].Value.ToString() + "', '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value.ToString() + ", " + Grid["Bom", i].Value + ", " + Grid["Production", i].Value + ", " + Grid["Emplno_Operator", i].Value + ", '" + Grid["Input_From", i].Value.ToString() + "')";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert Into Floor_Setting_Details (MasterID, MachineID, Tag_No, Order_No, OrderColorID, ItemID, SizeID, BOMQty, production, Emplno_Operator, Input_From) Values (" + Code + ",'" + Grid["Machine", i].Value.ToString() + "','" + Grid["Tag_No", i].Value.ToString() + "', '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value.ToString() + ", " + Grid["Bom", i].Value + ", " + Grid["Production", i].Value + ", " + Grid["Emplno_Operator", i].Value + ", '" + Grid["Input_From", i].Value.ToString() + "')";
                    }

                    Queries[Array_Index++] = " update Floor_Stock set Act_SetQty = Act_SetQty + " + Grid["Production", i].Value + " where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' and OrderColorID = " + Grid["OrderColorID", i].Value + " and SizeID = '" + Grid["SizeID", i].Value.ToString() + "'";
                    Queries[Array_Index++] = " update Floor_Stock set SetQty = SetQty + " + Grid["Production", i].Value + " where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' and OrderColorID = " + Grid["OrderColorID", i].Value + " and SizeID = '" + Grid["SizeID", i].Value.ToString() + "'";
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
                MyBase.Execute("Delete From " + Buffer_Table);
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Setting - Delete", "Select F1.EntryNo, F1.ENtryDate, S1.Shiftcode2 Shift, F1.Timing, F1.ShiftCode, F1.Remarks, F1.RowID From Floor_Setting_Master F1 Left Join Socks_Shift () S1 on F1.ShiftCode = S1.Shiftcode where F1.ENtryDate >='14-nov-2015' ", String.Empty, 80, 90, 70);
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
                    MyBase.Run("Update F1 Set F1.SetQty = F1.SetQty - Isnull(F2.Production, 0), F1.Act_SetQty = F1.Act_SetQty - Isnull(F2.Production, 0) From Floor_Stock F1 Inner join (Select Order_No, OrderColorID, SizeID, Sum(Production) Production From Floor_Setting_Details Where MasterID = " + Code + " Group By Order_No, OrderColorID, SizeID) F2 on F1.Order_No = F2.Order_No and F1.OrderColorID = F2.OrderColorID and F1.SizeID = F2.SizeID", "Delete From Floor_Setting_Details where MasterID = " + Code, "Delete From Floor_Setting_Master where RowID = " + Code);
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Setting - View", "Select F1.EntryNo, F1.EntryDate, S1.Shiftcode2 Shift, F1.Timing, F1.ShiftCode, F1.Remarks, F1.RowID From Floor_Setting_Master F1 Left Join Socks_Shift () S1 on F1.ShiftCode = S1.Shiftcode", String.Empty, 80, 90, 70);
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
                    Str = "Select 0 as Slno, F1.MachineID Machine,F1.Tag_No, F1.Order_No, F1.OrderColorID, Sample, F1.ItemID, Item, F1.SizeID, S1.Size, F1.BOMQty Bom, F1.Input_From, F1.Input_Qty, F1.Production, F1.Emplno_OPerator, E1.Name OPerator, '-' T From " + Buffer_Table + " F1 Left Join size S1 on F1.SizeID = S1.sizeid Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Order By F1.Slno";
                } 
                else
                {
                    if (MyParent._New)
                    {
                        Str = "Select 0 as Slno, F1.MachineID Machine,F1.Tag_No, F1.Order_No, F1.OrderColorID, Cast('' As Varchar (15)) Sample, F1.ItemID, Cast('' As Varchar (15)) Item, F1.SizeID, Cast('' As Varchar (15)) Size, F1.BOMQty Bom,Cast('' As Varchar (15)) Input_From, Cast(0 as numeric(25,2)) Input_Qty, F1.Production, F1.Emplno_OPerator, E1.Name OPerator, '-' T From Floor_Setting_Details F1 Left Join size S1 on F1.SizeID = S1.sizeid Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Where 1 = 2";
                    }
                    else
                    {
                        Str = "Select 0 as Slno, F1.MachineID Machine,F1.Tag_No, F1.Order_No, F1.OrderColorID, C1.color Sample, F1.ItemID, C1.item Item, F1.SizeID, S1.Size, F1.BOMQty Bom, F1.Input_From,(Case When W1.Order_No Is Null Then F2.LinkQty Else F2.WashQty End)Input_Qty,F1.Production, F1.Emplno_OPerator, E1.Name OPerator, '-' T From Floor_Setting_Details F1 Left Join Socks_Bom() C1 On F1.OrderColorID = C1.OrderColorId And F1.Order_No = C1.Order_No Left Join Floor_Stock F2 On F1.Order_No = F2.Order_No and F1.OrderColorID = F2.OrderColorID and F1.SizeID = F2.SizeID Left Join size S1 on F1.SizeID = S1.sizeid Left Join Washing_Order() W1 On F2.Order_No = W1.Order_No and F2.OrderColorID = W1.ColorID and F2.SizeID = W1.SizeID Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Where F1.MasterID = " + Code + " Order By F1.RowID";
                        //Str = "Select 0 as Slno, F1.MachineID Machine, F1.Order_No, F1.OrderColorID, C1.color Sample, F1.ItemID, C1.item Item, F1.SizeID, S1.Size, F1.BOMQty Bom, F1.Input_From, (Case When F1.Input_From = 'Link' Then F2.LinkQty When F1.Input_From = 'Wash' Then F2.WashQty Else 0 End)Input_Qty, F1.Production, F1.Emplno_OPerator, E1.Name OPerator, '-' T From Floor_Setting_Details F1 Left Join Socks_Bom() C1 On F1.OrderColorID = C1.OrderColorId And F1.Order_No = C1.Order_No Left Join Floor_Stock F2 On F1.Order_No = F2.Order_No and F1.OrderColorID = F2.OrderColorID and F1.SizeID = F2.SizeID Left Join size S1 on F1.SizeID = S1.sizeid Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Where F1.MasterID = " + Code + " Order By F1.RowID";
                    }
                }

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "OrderColorID", "SizeID", "ItemID", "Emplno_operator", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Machine", "Tag_No", "Production", "Operator");
                MyBase.Grid_Width(ref Grid, 40, 180, 80, 120, 100, 100, 100, 80, 80, 80, 80, 150);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                Grid.RowHeadersWidth = 10;

                Grid.Columns["Production"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //Grid.Columns["Production"].DefaultCellStyle.Format = "0";

                Grid.Columns["BOM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["BOM"].DefaultCellStyle.Format = "0";

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
                    TxtTiming.Text = Dr["StartTime"].ToString() + " - " + Dr["EndTime"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    Txt.Leave += new EventHandler(Txt_Leave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Leave(object sender, EventArgs e)
        {
            if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index)
            {
                if (Grid["Operator", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                {
                    MessageBox.Show("Please select the operator..!", "Gainup");
                    Grid.CurrentCell = Grid["Operator", Grid.CurrentCell.RowIndex];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    return;
                }
            }
        }

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Production"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
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
            try
            {
                if (MyParent._New != true)
                {
                    //if (e.KeyCode != Keys.Down)
                    //{
                    //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Production"].Index)
                    //{

                    //Grid.CurrentCell = Grid["OPerator", Grid.CurrentCell.RowIndex];
                    //SendKeys.Send("{Tab}");
                    //}
                    //}
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                {
                    Total_Prod_Qty();
                }

                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        Grid["Emplno_Operator", Grid.CurrentCell.RowIndex].Value = Grid["Emplno_Operator", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Operator", Grid.CurrentCell.RowIndex].Value = Grid["Operator", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Txt.Text = Grid["Operator", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                    }
                }

                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Tag_No"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        Grid["Tag_No", Grid.CurrentCell.RowIndex].Value = Grid["Tag_No", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Txt.Text = Grid["Tag_No", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Grid["Order_NO", Grid.CurrentCell.RowIndex - 1].Value.ToString();                        
                        Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Grid["OrderColorID", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Sample", Grid.CurrentCell.RowIndex].Value = Grid["Sample", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Grid["ItemID", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Item", Grid.CurrentCell.RowIndex].Value = Grid["Item", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Grid["SizeID", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Size", Grid.CurrentCell.RowIndex].Value = Grid["Size", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Bom", Grid.CurrentCell.RowIndex].Value = Grid["BOM", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Input_From", Grid.CurrentCell.RowIndex].Value = Grid["Input_From", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Input_Qty", Grid.CurrentCell.RowIndex].Value = Grid["Input_Qty", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Fill_BOM(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Input_From", Grid.CurrentCell.RowIndex].Value.ToString());
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                {
                    LblLinked.Text = "0";
                    LblPre_Prod.Text = "0";
                    LblProduction.Text = "0";
                    LblBal.Text = "0";
                    LblDesc.Text = "-";
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
                TxtTotal.Text = String.Format("{0:0.0}", Convert.ToDouble(MyBase.Sum(ref Grid, "Production", "Order_No", "Sample", "Operator")));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Machine_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine From Setting_Mc_NO ()", String.Empty, 200);
                if (Dr != null)
                {
                    Grid["Machine", Grid.CurrentCell.RowIndex].Value = Dr["Machine"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void OrderNo_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Tag No", "select Tag_No, Order_No, Item, Sample, Size, BOMQty BOM, Input_From, Input_Qty, ItemID, OrderColorID, SizeID from Floor_Setting_Input_New() Where Input_Qty > 0", String.Empty, 100, 120, 100, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Grid["Tag_No", Grid.CurrentCell.RowIndex].Value = Dr["Tag_No"].ToString();
                    Txt.Text = Dr["Tag_No"].ToString();
                    Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                    Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Dr["OrderColorID"].ToString();
                    Grid["Sample", Grid.CurrentCell.RowIndex].Value = Dr["Sample"].ToString();
                    Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                    Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                    Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                    Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                    Grid["Bom", Grid.CurrentCell.RowIndex].Value = Dr["Bom"].ToString();
                    Grid["Input_From", Grid.CurrentCell.RowIndex].Value = Dr["Input_From"].ToString();
                    Grid["Input_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Input_Qty"].ToString();
                    Fill_BOM(Dr["Order_No"].ToString(), Dr["Sample"].ToString(), Dr["Size"].ToString(), Dr["Input_From"].ToString());
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
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Operator", "Select Name, Tno, Emplno From Socks_Employee_Present_Detail ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "') Where DeptName = 'BOARDING' and Tno Not Like '%Z'", String.Empty, 250, 80);
                if (Dr != null)
                {
                    Grid["Operator", Grid.CurrentCell.RowIndex].Value = Dr["Name"].ToString();
                    Txt.Text = Dr["Name"].ToString();
                    Grid["EmplNo_Operator", Grid.CurrentCell.RowIndex].Value = Dr["Emplno"].ToString();
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                    {
                        Machine_Selection();
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Tag_No"].Index)
                    {
                        OrderNo_Selection();
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

        private void Grid_KeyDown(object sender, KeyEventArgs e)
    {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Production"].Index)
                    {
                        if (Grid["Production", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Production", Grid.CurrentCell.RowIndex].Value = "0";
                        }

                        /* if (Grid["Production", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Production", Grid.CurrentCell.RowIndex].Value) <= 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Production Qty ", "Gainup");
                            Grid["Production", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(LblBal.Text.Replace("BAL:", ""));
                            Grid.CurrentCell = Grid["Production", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }*/

                        if (Convert.ToDouble(Grid["Production", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(LblBal.Text.Replace("BAL:", "")))
                        {
                            e.Handled = true;
                            MessageBox.Show("Production is greater than BOM ", "Gainup");
                            Grid["Production", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(LblBal.Text.Replace("BAL:", ""));
                            Grid.CurrentCell = Grid["Production", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            Fill_BOM(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Input_From", Grid.CurrentCell.RowIndex].Value.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Fill_BOM(String OrderNo, String Sample, String Size, String Input_From)
        {
            try
            {
                DataTable Tdt = new DataTable();
                if (Input_From == "Wash")
                {
                    MyBase.Load_Data("Select F1.BomQty Bom, cast(isnull(F1.WashQty,0) as numeric(25,2)) Input, cast(Isnull(L1.Setted, 0) as numeric(25,2)) Setted, (isnull(F1.WashQty,0) - Isnull(L1.Setted, 0)) Balance_Setting From Floor_Stock F1 left join Socks_Bom () S1 on F1.Order_No = S1.Order_No and F1.OrderColorID = S1.OrderColorId and F1.SizeID = S1.sizeid Left Join Setting_Production_All () L1 on F1.Order_No = L1.OrderNo and F1.OrderColorId = L1.OrderColorID and F1.sizeid = L1.SizeID Where F1.Order_No = '" + OrderNo + "' And S1.color = '" + Sample + "' and S1.Size = '" + Size + "'", ref Tdt);
                }
                else
                {
                    MyBase.Load_Data("Select F1.BomQty Bom, cast(isnull(F1.LinkQty,0) as numeric(25,2)) Input, cast(Isnull(L1.Setted, 0) as numeric(25,2)) Setted, (isnull(F1.LinkQty,0) - Isnull(L1.Setted, 0)) Balance_Setting From Floor_Stock F1 left join Socks_Bom () S1 on F1.Order_No = S1.Order_No and F1.OrderColorID = S1.OrderColorId and F1.SizeID = S1.sizeid Left Join Setting_Production_All () L1 on F1.Order_No = L1.OrderNo and F1.OrderColorId = L1.OrderColorID and F1.sizeid = L1.SizeID Where F1.Order_No = '" + OrderNo + "' And S1.color = '" + Sample + "' and S1.Size = '" + Size + "'", ref Tdt);
                }
                if (Tdt.Rows.Count > 0)
                {
                    LblLinked.Text = "Input: " + Tdt.Rows[0]["Input"].ToString();
                    LblPre_Prod.Text = "PROD: " + Tdt.Rows[0]["Setted"].ToString();
                    LblBal.Text = "BAL: " + Tdt.Rows[0]["Balance_Setting"].ToString();

                    if (Grid["Production", Grid.CurrentCell.RowIndex].Value == null || Grid["Production", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Production", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["Production", Grid.CurrentCell.RowIndex].Value = "0";
                    }

                    LblProduction.Text = "0";

                    for (int i = 0; i <= Dt.Rows.Count - 1 - delcount; i++)
                    {
                        if (Grid.CurrentCell.RowIndex != i)
                        {
                            if (Dt.Rows[i]["Order_No"].ToString() == OrderNo && Dt.Rows[i]["Sample"].ToString() == Sample && Dt.Rows[i]["Size"].ToString() == Size)
                            {
                                LblProduction.Text = String.Format("{0:0.0}", Convert.ToDecimal(LblProduction.Text) + Convert.ToDecimal(Dt.Rows[i]["Production"]));
                            }
                        }
                    }

                    LblBal.Text = "BAL: " + String.Format("{0:0.0}", Convert.ToDecimal(LblBal.Text.Replace("BAL: ", "")) - Convert.ToDecimal(LblProduction.Text));
                }

                if (!MyParent._New)
                {
                    Tdt = new DataTable();
                    MyBase.Load_Data("Select Isnull(Sum(Production), 0) Production From Floor_Setting_Details Where Order_No = '" + OrderNo + "' And OrderColorID = .Dbo.Get_OrdercolorID ('" + OrderNo + "', '" + Sample + "') and SizeID = Dbo.Get_OrderSizeID ('" + OrderNo + "', '" + Size + "') and MasterID = " + Code, ref Tdt);
                    LblBal.Text = String.Format("{0:0.0}", Convert.ToDecimal(LblBal.Text.Replace("BAL: ", "")) + Convert.ToDecimal(Tdt.Rows[0][0]));
                }

                Tdt = new DataTable();
                MyBase.Load_Data("Select * From Stage_Item_Desc () Where Im_Item_Code = '" + Sample + "'", ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    LblDesc.Text = Tdt.Rows[0]["Item_Desc"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            try
            {
                if (Grid.CurrentCell != null)
                {
                    if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        Fill_BOM(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Input_From", Grid.CurrentCell.RowIndex].Value.ToString());
                    }
                    else
                    {
                        LblBal.Text = "0";
                        LblPre_Prod.Text = "0";
                        LblProduction.Text = "0";
                        LblLinked.Text = "0";
                        LblDesc.Text = "-";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                {
                    if (Grid["Machine", Grid.CurrentCell.RowIndex].Value == null || Grid["Machine", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Machine_Selection();
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Tag_No"].Index)
                {
                    if (Grid.CurrentCell.RowIndex == 0)
                    {
                        if (Grid["Tag_No", Grid.CurrentCell.RowIndex].Value == null || Grid["Tag_No", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Tag_No", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            OrderNo_Selection();
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index)
                {
                    if (Grid.CurrentCell.RowIndex == 0)
                    {
                        if (Grid["Operator", Grid.CurrentCell.RowIndex].Value == null || Grid["Operator", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Operator", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Operator_Selection();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            String[] Queries = new String[Dt.Rows.Count];
            Int32 Array_Index = 0;
            try
            {
                if (Buffer_Update)
                {
                    if (!MyBase.Check_Table(Buffer_Table))
                    {
                        MyBase.Execute("Select Cast(0 as int) Slno, cast(MachineID as varchar(20)) MachineID,Tag_No, Order_No, OrderColorID,cast('' as varchar(20)) Sample, ItemID,cast('' as varchar(20)) Item, SizeID,cast('' as varchar(20)) Size, BomQty, cast('' as varchar(20)) Input_From, Cast(0 as numeric(25,2)) Input_Qty, Production, Emplno_Operator,cast('' as varchar(20)) Operator into " + Buffer_Table + " From Floor_Washing_Details Where 1 = 2");
                    }

                    MyBase.Execute("Delete From " + Buffer_Table);

                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        Queries[Array_Index++] = "Insert Into " + Buffer_Table + " (Slno, MachineID,Tag_No, Order_No, OrderColorID, Sample, ItemID, Item, SizeID, Size, BOMQty, Input_From, Input_Qty, production, Emplno_Operator, Operator) Values (" + Grid["Slno", i].Value.ToString() + ", '" + Grid["Machine", i].Value.ToString() + "','" + Grid["Tag_No", i].Value.ToString() + "', '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", '" + Grid["Sample", i].Value.ToString() + "', " + Grid["ItemID", i].Value.ToString() + ", '" + Grid["Item", i].Value.ToString() + "', " + Grid["SizeID", i].Value + ", '" + Grid["Size", i].Value.ToString() + "', " + Grid["Bom", i].Value + ", '" + Grid["Input_From", i].Value.ToString() + "'," + Grid["Input_Qty", i].Value + ", " + Grid["Production", i].Value + ", " + Grid["Emplno_Operator", i].Value + ", '" + Grid["Operator", i].Value.ToString() + "')";
                    }

                    if (Dt.Rows.Count > 1)
                    {
                        MyBase.Run_Without_Error_Message(Queries);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().Contains("INCORRECT SYNTAX"))
                {
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
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
        decimal Fill_BOM_Check(String OrderNo, String Sample, String Size, decimal Input, String Input_From)
        {
            try
            {
                decimal Prod = 0;
                decimal Bal = 0;
                decimal Input1 = 0;
                DataTable Tdt = new DataTable();
                if (Input_From == "Wash")
                {
                    MyBase.Load_Data("Select S1.Bom_Qty Bom, cast(Isnull(F1.WashQty, 0) as numeric(25,2)) Inp, cast((isnull(F1.WashQty,0) - Isnull(K1.Setted, 0)) as numeric(25,2)) Balance_Setting From Socks_Bom() S1 Left Join Floor_Stock F1 on S1.Order_No = F1.Order_No and S1.OrderColorId = F1.OrderColorID and S1.sizeid = F1.SizeID and S1.Bom_Qty = F1.BOMQty Left Join Setting_Production_All () k1 on S1.Order_No = K1.OrderNo and S1.OrderColorId = K1.OrderColorID and S1.sizeid = K1.SizeID Where S1.Order_No = '" + OrderNo + "' And S1.color = '" + Sample + "' and S1.Size = '" + Size + "'", ref Tdt);
                }
                else
                {
                    MyBase.Load_Data("Select S1.Bom_Qty Bom, cast(Isnull(F1.LinkQty, 0) as numeric(25,2)) Inp, cast((isnull(F1.LinkQty,0) - Isnull(K1.Setted, 0)) as numeric(25,2)) Balance_Setting From Socks_Bom() S1 Left Join Floor_Stock F1 on S1.Order_No = F1.Order_No and S1.OrderColorId = F1.OrderColorID and S1.sizeid = F1.SizeID and S1.Bom_Qty = F1.BOMQty Left Join Setting_Production_All () k1 on S1.Order_No = K1.OrderNo and S1.OrderColorId = K1.OrderColorID and S1.sizeid = K1.SizeID Where S1.Order_No = '" + OrderNo + "' And S1.color = '" + Sample + "' and S1.Size = '" + Size + "'", ref Tdt);
                }
                if (Tdt.Rows.Count > 0)
                {
                    Input1 = Convert.ToDecimal(Tdt.Rows[0]["Inp"].ToString());
                    Bal = Input1;
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Dt.Rows[i]["Order_No"].ToString() == OrderNo && Dt.Rows[i]["Sample"].ToString() == Sample && Dt.Rows[i]["Size"].ToString() == Size)
                        {
                            Prod = Convert.ToDecimal(Prod) + Convert.ToDecimal(Dt.Rows[i]["Production"]);
                        }
                    }
                    Bal = Convert.ToDecimal(Bal) - Convert.ToDecimal(Prod);
                }
                return Bal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {

        }

        private void LblSpecial_Click(object sender, EventArgs e)
        {

        }

     }
}