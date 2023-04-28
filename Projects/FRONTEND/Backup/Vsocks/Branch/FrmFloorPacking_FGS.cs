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
    public partial class FrmFloorPacking_FGS : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        Int64 Code = 0;
        DataRow Dr;
        MDIMain MyParent;
        TextBox Txt = null;
        Boolean Delete_Flag = false;
        String Buffer_Table = String.Empty;
        Boolean Buffer_Update = false;
        Int16 delcount = 0;

        public FrmFloorPacking_FGS()
        {
            InitializeComponent();
        }

        private void FrmFloorPacking_FGS_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                Buffer_Table = "Pack_FGS_" + Environment.MachineName.Replace("-", "") + "_" + MyParent.UserCode.ToString();
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
                TxtUnit.Text = Dr["Unit"].ToString();
                TxtUnit.Tag = Dr["UnitCode"].ToString();
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Packing - Edit", "Select F1.EntryNo, F1.ENtryDate, S1.Shiftcode2 Shift, U1.Unit_Name Unit, F1.Timing, F1.ShiftCode, F1.Remarks, F1.RowID, F1.UnitCode From Floor_Packing_FGS_Master F1 Left Join Socks_Shift () S1 on F1.ShiftCode = S1.Shiftcode Left Join VAAHINI_ERP_GAINUP.Dbo.Unit_Master U1 On F1.UnitCode = U1.RowId Where F1.ENtryDate >='14-nov-2015' and F1.ENtryDate >= Dateadd (D, " + MyParent.User_Datelock + ", Cast(GetDate() as Date))", String.Empty, 80, 90, 70, 70);
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

        decimal Fill_BOM_Check(String OrderNo, String Sample, String Size)
        {
            try
            {
                decimal Prod = 0;
                decimal Bal = 0;
                decimal Packed = 0;
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select S1.Bom_Qty Bom, Isnull(K1.Packed,0)Packed, Cast(((S1.Bom_Qty / S1.GUOM_Conv) - Isnull(K1.Packed,0))As Numeric) Balance_Packing From Socks_Bom() S1 Left Join Packing_FGS_Production_All() k1 on S1.Order_No = K1.OrderNo and S1.OrderColorId = K1.OrderColorID and S1.sizeid = K1.SizeID Where S1.Order_No = '" + OrderNo + "' And S1.color = '" + Sample + "' and S1.Size = '" + Size + "'", ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    Packed = Convert.ToDecimal(Tdt.Rows[0]["Packed"].ToString());
                    Prod = Convert.ToDecimal(Tdt.Rows[0]["Packed"].ToString());
                    Bal = Convert.ToDecimal(Tdt.Rows[0]["Bom"].ToString());
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

                if (TxtShift.Text == "" || TxtShift.Tag == "" || TxtShift.Text == null || TxtShift.Tag == null || TxtUnit.Tag == "" || TxtUnit.Text == null)
                {
                    MessageBox.Show("Invalid Shift ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtShift.Focus();
                    return;
                }

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
                    if (Grid["Production", i].Value == DBNull.Value || Grid["Production", i].Value.ToString() == String.Empty || Convert.ToDecimal (Grid["Production", i].Value) == 0)
                    {
                        MessageBox.Show(" ZERO is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Production", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }

                    if (Fill_BOM_Check(Grid["Order_No", i].Value.ToString(), Grid["Sample", i].Value.ToString(), Grid["Size", i].Value.ToString()) < 0)
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
                Queries = new String[(Dt.Rows.Count * 1) + 5];

                TxtNo.Text = MyBase.MaxOnlyComp("Floor_Packing_FGS_Master", "EntryNo", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Floor_Packing_FGS_Master (EntryNo, EntryDate, ShiftCode, Timing, Company_Code, EntryTime, EntrySystem, Remarks, UnitCode) Values (" + TxtNo.Text + ",'" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "','" + TxtShift.Tag.ToString() + "','" + TxtTiming.Text.ToString() + "'," + MyParent.CompCode + ",getdate(),Host_name(), '" + TxtRemarks.Text + "', " + TxtUnit.Tag + ") ; Select Scope_Identity()";
                }
                else
                {
                    Queries[Array_Index++] = "Update Floor_Packing_FGS_Master Set ShiftCode = " + TxtShift.Tag.ToString() + ", Remarks = '" + TxtRemarks.Text + "', UnitCode = " + TxtUnit.Tag + " where RowID = " + Code;
                    Queries[Array_Index++] = "Delete From Floor_Packing_FGS_Details where MAsterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New == true)
                    {
                        Queries[Array_Index++] = "Insert Into Floor_Packing_FGS_Details (MasterID, MachineID, Order_No, OrderColorID, ItemID, SizeID, BOMQty, production, Rework, Unit_Code) Values (@@IDENTITY, '" + Grid["MachineID", i].Value.ToString() + "', '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", " + Grid["ItemID", i].Value + ", '" + Grid["SizeID", i].Value.ToString() + "', " + Grid["Bom", i].Value + ", " + Grid["Production", i].Value + ", " + Grid["Rework", i].Value + ", " + Grid["Unit_Code", i].Value + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert Into Floor_Packing_FGS_Details (MasterID, MachineID, Order_No, OrderColorID, ItemID, SizeID, BOMQty, production, Rework, Unit_Code) Values (" + Code + ", '" + Grid["MachineID", i].Value.ToString() + "', '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", " + Grid["ItemID", i].Value + ", '" + Grid["SizeID", i].Value.ToString() + "', " + Grid["Bom", i].Value + ", " + Grid["Production", i].Value + "," + Grid["Rework", i].Value + ", " + Grid["Unit_Code", i].Value + ")";
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Packing - Delete", "Select F1.EntryNo, F1.ENtryDate, S1.Shiftcode2 Shift, U1.Unit_Name Unit, F1.Timing, F1.ShiftCode, F1.Remarks, F1.RowID, F1.UnitCode From Floor_Packing_FGS_Master F1 Left Join Socks_Shift () S1 on F1.ShiftCode = S1.Shiftcode Left Join VAAHINI_ERP_GAINUP.Dbo.Unit_Master U1 On F1.UnitCode = U1.RowId where F1.ENtryDate >= '14-nov-2015' ", String.Empty, 80, 90, 70, 70);
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
                    MyBase.Run("Delete From Floor_Packing_FGS_Details where MasterID = " + Code, "Delete From Floor_Packing_FGS_Master where RowID = " + Code);
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Packing - View", "Select F1.EntryNo, F1.ENtryDate, S1.Shiftcode2 Shift, U1.Unit_Name Unit, F1.Timing, F1.ShiftCode, F1.Remarks, F1.RowID, F1.UnitCode From Floor_Packing_FGS_Master F1 Left Join Socks_Shift () S1 on F1.ShiftCode = S1.Shiftcode Left Join VAAHINI_ERP_GAINUP.Dbo.Unit_Master U1 On F1.UnitCode = U1.RowId", String.Empty, 80, 90, 70, 70);
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
                    Str = "Select 0 as Slno, F1.MachineID, F1.Machine, F1.Order_No, F1.OrderColorID, Sample, F1.ItemID, Item, F1.SizeID, S1.Size, F1.BOMQty Bom, GUOM_Lookup, GUOM_Conv, Paired_Qty, Max_Pack, F1.Production, F1.Rework, Unit, Unit_Code, '-' T From " + Buffer_Table + " F1 Left Join size S1 on F1.SizeID = S1.sizeid Order By F1.Slno";
                }
                else
                {
                    if (MyParent._New)
                    {
                        Str = "Select 0 as Slno, F1.MachineID, M1.Machine_Name Machine, F1.Order_No, F1.OrderColorID, Cast('' As Varchar (15)) Sample, F1.ItemID, Cast('' As Varchar (15)) Item, F1.SizeID, Cast('' As Varchar (15)) Size, F1.BOMQty Bom, Cast('' as Varchar(15)) GUOM_Lookup, Cast(0 as Bigint) GUom_Conv, Cast(0 as numeric(25,2))Max_Pack, cast(F1.Production as numeric(25,2)) Production ,cast(F1.Rework as numeric(25,2)) Rework, U1.Unit_Name Unit, F1.Unit_Code, '-' T From Floor_Packing_FGS_Details F1 Left Join size S1 on F1.SizeID = S1.Sizeid Left Join Vaahini_ERP_Gainup.Dbo.Unit_Master U1 On F1.Unit_Code = U1.RowID Left Join Socks_Packing_Machine_List M1 On F1.MachineID = M1.Rowid Where 1 = 2";
                    }
                    else
                    {
                        Str = "Select 0 as Slno, F1.MachineID, M1.Machine_Name Machine, F1.Order_No, F1.OrderColorID, C1.color Sample, F1.ItemID, C1.item Item, F1.SizeID, S1.Size, F1.BOMQty Bom, C1.GUOM_Lookup, C1.GUom_Conv, cast(F1.BOMQty/C1.GUom_Conv as numeric(25,2)) Max_Pack, F1.Production, F1.Rework, U1.Unit_Name Unit, F1.Unit_Code, '-' T From Floor_Packing_FGS_Details F1 Left Join Socks_Bom() C1 On F1.OrderColorID = C1.OrderColorId And F1.Order_No = C1.Order_No And F1.ItemID = C1.Itemid And F1.SizeID = C1.sizeid Left Join Floor_Stock F2 On F1.Order_No = F2.Order_No and F1.OrderColorID = F2.OrderColorID and F1.SizeID = F2.SizeID Left Join size S1 on F1.SizeID = S1.sizeid Left Join Vaahini_ERP_Gainup.Dbo.Unit_Master U1 On F1.Unit_Code = U1.RowID  Left Join Socks_Packing_Machine_List M1 On F1.MachineID = M1.Rowid Where F1.MasterID = " + Code + " Order By F1.RowID ";
                    }
                }

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "OrderColorID", "SizeID", "ItemID", "GUOM_Conv", "Unit_Code", "MachineID", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Machine", "Order_No", "Production", "Rework", "Unit");
                MyBase.Grid_Width(ref Grid, 45, 100, 125, 90, 90, 125, 90, 90, 90, 90, 90, 145, 95);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                Grid.RowHeadersWidth = 10;
                Grid.Columns["Max_Pack"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                
                Grid.Columns["Production"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //Grid.Columns["Production"].DefaultCellStyle.Format = "0";

                Grid.Columns["Rework"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //Grid.Columns["Rework"].DefaultCellStyle.Format = "0";

                Grid.Columns["BOM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["BOM"].DefaultCellStyle.Format = "0";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Total_Prod_Qty()
        {
            try
            {
                TxtTotal.Text = String.Format("{0:0.0}", Convert.ToDouble(MyBase.Sum(ref Grid, "Production", "Order_No", "Sample", "Unit_Code")));
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

        private void FrmFloorPacking_FGS_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtUnit")
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
                    else if (this.ActiveControl.Name == "TxtUnit")
                    {
                        Unit_Selection1();
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

        private void FrmFloorPacking_FGS_KeyPress(object sender, KeyPressEventArgs e)
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
            try
            {
                //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index)
                //{
                //    if (Grid["Operator", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                //    {
                //        MessageBox.Show("Please select the operator..!", "Gainup");
                //        Grid.CurrentCell = Grid["Operator", Grid.CurrentCell.RowIndex];
                //        Grid.Focus();
                //        Grid.BeginEdit(true);
                //        return;
                //    }
                //}
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

        void Txt_GotFocus(object sender, EventArgs e)
        {
            try
            {
             
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        Total_Prod_Qty();

                        LblPaired.Text = "0";
                        LblPre_Prod.Text = "0";
                        LblProduction.Text = "0";
                        LblBal.Text = "0";
                        LblDesc.Text = "-";

                        //Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Grid["Order_NO", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //Txt.Text = Grid["Order_NO", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //Grid["Sample", Grid.CurrentCell.RowIndex].Value = Grid["Sample", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Grid["SizeID", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //Grid["Size", Grid.CurrentCell.RowIndex].Value = Grid["Size", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //Grid["Bom", Grid.CurrentCell.RowIndex].Value = Grid["BOM", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //Grid["Item", Grid.CurrentCell.RowIndex].Value = Grid["Item", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Grid["ItemID", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Grid["OrderColorID", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //Grid["SettedQty", Grid.CurrentCell.RowIndex].Value = Grid["SettedQty", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //Fill_BOM(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString());
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Unit"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        Grid["Unit_Code", Grid.CurrentCell.RowIndex].Value = Grid["Unit_Code", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Unit", Grid.CurrentCell.RowIndex].Value = Grid["Unit", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Txt.Text = Grid["Unit", Grid.CurrentCell.RowIndex - 1].Value.ToString();
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Production"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Rework"].Index)
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

        void OrderNo_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select Order_No, Item, Sample, Size, BOM_Qty BOM, GUOM_Lookup, Max_Pack, ItemID, OrderColorID, SizeID, GUOM_Conv From Floor_Packing_FGS_Input_New() ", String.Empty, 100, 120, 100, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                    Txt.Text = Dr["Order_No"].ToString();
                    Grid["Sample", Grid.CurrentCell.RowIndex].Value = Dr["Sample"].ToString();
                    Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                    Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                    Grid["Bom", Grid.CurrentCell.RowIndex].Value = Dr["Bom"].ToString();
                    Grid["GUOM_Lookup", Grid.CurrentCell.RowIndex].Value = Dr["GUOM_Lookup"].ToString();
                    Grid["GUOM_Conv", Grid.CurrentCell.RowIndex].Value = Dr["GUOM_Conv"].ToString();
                    Grid["Max_Pack", Grid.CurrentCell.RowIndex].Value = Dr["Max_Pack"].ToString();
                    Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                    Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                    Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Dr["OrderColorID"].ToString();
                    Fill_BOM(Dr["Order_No"].ToString(), Dr["Sample"].ToString(), Dr["Size"].ToString());
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
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Unit", "Select Unit_Name Unit, Rowid Unit_Code From Vaahini_Erp_Gainup.Dbo.Unit_Master ", String.Empty, 250, 80);
                if (Dr != null)
                {
                    Grid["Unit", Grid.CurrentCell.RowIndex].Value = Dr["Unit"].ToString();
                    Txt.Text = Dr["Unit"].ToString();
                    Grid["Unit_Code", Grid.CurrentCell.RowIndex].Value = Dr["Unit_Code"].ToString();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        void Unit_Selection1()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Unit", "Select Unit_Name Unit, Rowid Unit_Code From Vaahini_Erp_Gainup.Dbo.Unit_Master ", String.Empty, 250, 80);
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

        void Machine_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine_Name Machine, Machine_ID MachineID, '' Remarks From Get_Machine_List_Packing(" + TxtUnit.Tag + ")", String.Empty, 200, 250);
                if (Dr != null)
                {
                    Grid["Machine", Grid.CurrentCell.RowIndex].Value = Dr["Machine"].ToString();
                    Txt.Text = Dr["Machine"].ToString();
                    Grid["MachineID", Grid.CurrentCell.RowIndex].Value = Dr["MachineID"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //void Operator_Selection()
        //{
        //    try
        //    {
        //        Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Operator", "Select Name, Tno, Emplno From Socks_Employee_Present_Detail ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "') Where Tno Not Like '%Z'", String.Empty, 250, 80);
        //        if (Dr != null)
        //        {
        //            Grid["Operator", Grid.CurrentCell.RowIndex].Value = Dr["Name"].ToString();
        //            Txt.Text = Dr["Name"].ToString();
        //            Grid["EmplNo_Operator", Grid.CurrentCell.RowIndex].Value = Dr["Emplno"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Unit"].Index)
                    {
                        Unit_Selection();
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

                        if (Convert.ToDouble(Grid["Production", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(LblBal.Text.Replace("BAL:", "")))
                        {
                            Fill_BOM(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString());
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
                            Fill_BOM(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString());
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rework"].Index)
                    {
                        if (Grid["Rework", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Rework", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Fill_BOM(String OrderNo, String Sample, String Size)
        {
            try
            {
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select S1.Bom_Qty Bom, Isnull(K1.Packed,0) Packed, CAst(((S1.Bom_Qty / S1.GUOM_Conv) - Isnull(K1.Packed,0))As Numeric) Balance_Packing, S1.Bom_Qty / S1.GUOM_Conv Max_Pack From Socks_Bom () S1 Left Join Packing_FGS_Production_All () k1 on S1.Order_No = K1.OrderNo and S1.OrderColorId = K1.OrderColorID and S1.sizeid = K1.SizeID Where S1.Order_No = '" + OrderNo + "' And S1.color = '" + Sample + "' and S1.Size = '" + Size + "'", ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    //LblPaired.Text = "PAIRED: " + Tdt.Rows[0]["Paired"].ToString();
                    LblPre_Prod.Text = "PROD: " + Tdt.Rows[0]["Packed"].ToString();
                    LblBal.Text = "BAL: " + Tdt.Rows[0]["Balance_Packing"].ToString();

                    if (Grid["Production", Grid.CurrentCell.RowIndex].Value == null || Grid["Production", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Production", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["Production", Grid.CurrentCell.RowIndex].Value = "0";
                    }

                    LblProduction.Text = "0";

                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
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
                    MyBase.Load_Data("Select Isnull(Sum(Production), 0) Production From Floor_Packing_FGS_Details Where Order_No = '" + OrderNo + "' And OrderColorID = .Dbo.Get_OrdercolorID ('" + OrderNo + "', '" + Sample + "') and SizeID = Dbo.Get_OrderSizeID ('" + OrderNo + "', '" + Size + "') and MasterID = " + Code, ref Tdt);
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
                Delete_Flag = true;
                MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentCell.RowIndex);
                Delete_Flag = false;
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
                if (!Delete_Flag)
                {
                    if (Grid.CurrentCell != null && Grid.CurrentCell.RowIndex > 0)
                    {
                        if (Grid["Production", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            Fill_BOM(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString());
                        }
                        else
                        {
                            LblBal.Text = "0";
                            LblPre_Prod.Text = "0";
                            LblProduction.Text = "0";
                            LblPaired.Text = "0";
                            LblDesc.Text = "-";
                        }
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value == null || Grid["Order_No", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            OrderNo_Selection();
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        if (Grid["Machine", Grid.CurrentCell.RowIndex].Value == null || Grid["Machine", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Machine_Selection();
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Unit"].Index)
                {
                    if (Grid.CurrentCell.RowIndex == 0)
                    {
                        if (Grid["Unit", Grid.CurrentCell.RowIndex].Value == null || Grid["Unit", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Unit", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Unit_Selection();
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
                        MyBase.Execute("Select Cast(0 as int) Slno, MachineID, Machine, Order_No, OrderColorID, cast('' as varchar(20)) Sample, ItemID, cast('' as varchar(20)) Item, SizeID, cast('' as varchar(20)) Size, BomQty, Cast('' as Varchar(15)) GUOM_Lookup, Cast(0 as Bigint)GUOM_Conv, Cast(0 as Numeric(25,2)) Max_Pack, Production, Rework, Cast('' as Varchar(15)) Unit, Unit_Code into " + Buffer_Table + " From Floor_Packing_FGS_Details Where 1 = 2");
                    }

                    MyBase.Execute("Delete From " + Buffer_Table);

                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        Queries[Array_Index++] = "Insert Into " + Buffer_Table + " (Slno, MachineID, Machine, Order_No, OrderColorID, Sample, ItemID, Item, SizeID, Size, BOMQty, GUOM_Lookup, GUOM_Conv, Max_Pack, production, Rework, Unit, Unit_Code) Values (" + Grid["MachineID", i].Value.ToString() + ", '" + Grid["Machine", i].Value.ToString() + "' " + Grid["Slno", i].Value.ToString() + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", '" + Grid["Sample", i].Value.ToString() + "', " + Grid["ItemID", i].Value.ToString() + ", '" + Grid["Item", i].Value.ToString() + "', " + Grid["SizeID", i].Value + ", '" + Grid["Size", i].Value.ToString() + "', " + Grid["Bom", i].Value + ", '" + Grid["GUOM_Lookup", i].Value + "', " + Grid["GUOM_Conv", i].Value + ", " + Grid["Max_Pack", i].Value + ", " + Grid["Production", i].Value + ", " + Grid["Rework", i].Value + ", '" + Grid["Unit", i].Value.ToString() + "', " + Grid["Unit_Code", i].Value + ")";
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

    }
}