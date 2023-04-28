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
    public partial class FrmPairingProductionWithRejection : Form, Entry
    {
        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataRow Dr;
        TextBox Txt = null;
        TextBox Txt_Qty = null;
        TextBox Txt_Cont = null;
        Int64 Code = 0;
        DataTable[] DtQty;
        DataTable[] DtCont;
        String Str;
        Int16 Vis = 0;
        int Pos = 0;

        public FrmPairingProductionWithRejection()
        {
            InitializeComponent();
        }

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                DataTable Dth = new DataTable();
                Grid_Data();
                DtQty = new DataTable[30];
                DtpDate1.Focus(); 
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
                MyBase.Row_Number(ref GridQty);
                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Entry ...!", "Gainup");
                    MyParent.Save_Error = true;
                    DtpDate1.Focus();
                    return;
                }

                if (TxtTotal.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Total ...!", "Gainup");
                    MyParent.Save_Error = true;
                    DtpDate1.Focus();
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 1; j++)
                    {
                        if (Grid["Production", i].Value == DBNull.Value || Grid["Rejection", i].Value.ToString() == String.Empty)
                        {
                            MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                            Grid.CurrentCell = Grid[j, i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                }

                if (!Check_Qty_Breakup())
                {
                    MessageBox.Show("Check Qty Breakup in Reject Details...!", "Gainup");
                    MyParent.Save_Error = true;
                    return;
                }

                TxtEntryNo.Text = MyBase.MaxOnlyWithoutComp("Floor_Socks_Pairing_Production_Master", "EntryNo", String.Empty, String.Empty, 0).ToString();
                Queries = new string[Dt.Rows.Count * 100];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Floor_Socks_Pairing_Production_Master (EntryNo, EntryDate, ShiftCode, Timing, Unit_Code, EntryAt, EntrySystem, Remarks) values (" + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', " + TxtShift.Tag + ", '" + TxtTiming.Text.ToString() + "', " + TxtUnit.Tag + ", Getdate(), Host_Name(), '" + TxtRemarks.Text + "'); Select Scope_Identity() ";
                    //Queries[Array_Index++] = MyParent.EntryLog("AUDIT ISSUE", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Floor_Socks_Pairing_Production_Master Set EntryDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', ShiftCode = " + TxtShift.Tag + ", Timing = '" + TxtTiming.Text.ToString() + "', Unit_Code = " + TxtUnit.Tag + ", Remarks = '" + TxtRemarks.Text + "' Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("AUDIT ISSUE", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete From Floor_Socks_Pairing_Production_Details where MasterID = " + Code;
                    Queries[Array_Index++] = "Delete From Floor_Socks_Pairing_Production_Rejection_Details Where MasterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Floor_Socks_Pairing_Production_Details (MasterID, MachineID, Slno, Order_No, OrderColorID, ItemID, SizeID, BomQty, Production, Rejection, Production_Details_Slno1, Remarks) Values (@@IDENTITY, " + Dt.Rows[i]["MachineID"].ToString() + ", " + Dt.Rows[i]["Slno"].ToString() + ", '" + Dt.Rows[i]["Order_No"].ToString() + "', " + Dt.Rows[i]["OrderColorID"].ToString() + ", " + Dt.Rows[i]["ItemID"].ToString() + ", " + Dt.Rows[i]["SizeID"].ToString() + ", " + Dt.Rows[i]["BomQty"].ToString() + ", " + Dt.Rows[i]["Production"].ToString() + ", " + Dt.Rows[i]["Rejection"].ToString() + ", " + Grid["Slno1", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "')";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Floor_Socks_Pairing_Production_Details (MasterID, MachineID, Slno, Order_No, OrderColorID, ItemID, SizeID, BomQty, Production, Rejection, Production_Details_Slno1, Remarks) Values (" + Code + ", " + Dt.Rows[i]["MachineID"].ToString() + ", " + Dt.Rows[i]["Slno"].ToString() + ", '" + Dt.Rows[i]["Order_No"].ToString() + "', " + Dt.Rows[i]["OrderColorID"].ToString() + ", " + Dt.Rows[i]["ItemID"].ToString() + ", " + Dt.Rows[i]["SizeID"].ToString() + ", " + Dt.Rows[i]["BomQty"].ToString() + ", " + Dt.Rows[i]["Production"].ToString() + ", " + Dt.Rows[i]["Rejection"].ToString() + ", " + Grid["Slno1", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "')";
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToInt16(Dt.Rows[i]["Rejection"].ToString()) > 0)
                    {
                        for (i = 0; i <= DtQty.Length - 1; i++)
                        {
                            if (DtQty[i] != null)
                            {
                                for (int j = 0; j <= DtQty[i].Rows.Count - 1; j++)
                                {
                                    if (MyParent._New)
                                    {
                                        Queries[Array_Index++] = "Insert Into Floor_Socks_Pairing_Production_Rejection_Details (MasterId, Production_Details_Slno1, Slno, RejectionID, RejectionQty) values (@@IDENTITY, " + DtQty[i].Rows[j]["SLNO1"].ToString() + ", " + DtQty[i].Rows[j]["SLNO"] + ", " + DtQty[i].Rows[j]["RejectionID"] + ", " + DtQty[i].Rows[j]["RejectionQty"] + ")";
                                    }
                                    else
                                    {
                                        Queries[Array_Index++] = "Insert Into Floor_Socks_Pairing_Production_Rejection_Details (MasterId, Production_Details_Slno1, Slno, RejectionID, RejectionQty) values (" + Code + ", " + DtQty[i].Rows[j]["SLNO1"].ToString() + ", " + DtQty[i].Rows[j]["SLNO"] + ", " + DtQty[i].Rows[j]["RejectionID"] + ", " + DtQty[i].Rows[j]["RejectionQty"] + ")";
                                    }
                                }
                            }
                        }
                    }
                }

                MyBase.Run_Identity(MyParent.Edit, Queries);
                MessageBox.Show("Saved ...!", "Gainup");
                MyParent.Save_Error = false;
                MyBase.Clear(this);

            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        Boolean Check_Qty_Breakup()
        {
            Double BRQty = 0;
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    BRQty = 0;

                    if (Convert.ToInt16(Dt.Rows[i]["Rejection"].ToString()) > 0)
                    {
                        if (DtQty[Convert.ToInt32(Dt.Rows[i]["SlNo1"])] == null)
                        {
                            MessageBox.Show("Invalid Qty Breakup Details in Rejection ...!", "Gainup");
                            MyParent.Save_Error = true;
                            Grid.CurrentCell = Grid["Rejection", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return false;
                        }
                        else
                        {
                            for (int j = 0; j <= DtQty[Convert.ToInt32(Dt.Rows[i]["Slno1"])].Rows.Count - 1; j++)
                            {
                                BRQty += Convert.ToDouble(DtQty[Convert.ToInt32(Dt.Rows[i]["Slno1"])].Rows[j]["RejectionQty"]);
                            }

                            if (Math.Round(Convert.ToDouble(BRQty), 3) != Math.Round(Convert.ToDouble(Grid["Rejection", i].Value), 3))
                            {
                                MessageBox.Show("Invalid Qty Breakup Details in Rejection...!", "Gainup");
                                Grid.Focus();
                                Grid.CurrentCell = Grid["Rejection", i];
                                Grid.BeginEdit(true);
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Pairing Production Entry - Delete", "Select EntryNo, EntryDate, S1.shiftcode2 Shift, S2.Unit_Name Unit, F1.RowID, F1.ShiftCode, F1.Unit_Code, F1.Timing, F1.Remarks From Floor_Socks_Pairing_Production_Master F1 Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S1 On F1.ShiftCode = S1.shiftcode And S1.compcode = 2 And S1.Mode = 1 And S1.shiftcode in (15, 16, 17) Left Join Socks_Unit_Master S2 On F1.Unit_Code = S2.RowID Order by F1.EntryNo Desc ", String.Empty, 90, 100, 100, 90, 90);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    MyParent.Load_DeleteConfirmEntry();
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
                if (Code > 0 && Dt.Rows.Count > 0)
                {
                    MyBase.Run("Delete from Floor_Socks_Pairing_Production_Rejection_Details where MasterID = " + Code, "Delete From Floor_Socks_Pairing_Production_Details Where MasterID = " + Code, "Delete From Floor_Socks_Pairing_Production_Master Where RowID = " + Code);
                    MessageBox.Show("Deleted ...!", "Gainup");
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

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                DtQty = new DataTable[30];
                DtCont = new DataTable[100];
                Code = Convert.ToInt64(Dr["RowID"]);
                TxtEntryNo.Text = Dr["EntryNo"].ToString();
                DtpDate1.Value = Convert.ToDateTime(Dr["EntryDate"]);
                TxtRemarks.Text = Dr["Remarks"].ToString();
                TxtShift.Text = Dr["Shift"].ToString();
                TxtShift.Tag = Dr["ShiftCode"].ToString();
                TxtTiming.Text = Dr["Timing"].ToString(); 
                TxtUnit.Text = Dr["Unit"].ToString();
                TxtUnit.Tag = Dr["Unit_Code"].ToString();
                Grid_Data();
                Total();
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Pairing Production Entry - Edit", "Select EntryNo, EntryDate, S1.shiftcode2 Shift, S2.Unit_Name Unit, F1.RowID, F1.ShiftCode, F1.Unit_Code, F1.Timing, F1.Remarks From Floor_Socks_Pairing_Production_Master F1 Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S1 On F1.ShiftCode = S1.shiftcode And S1.compcode = 2 And S1.Mode = 1 And S1.shiftcode in (15, 16, 17) Left Join Socks_Unit_Master S2 On F1.Unit_Code = S2.RowID Order by F1.EntryNo Desc ", String.Empty, 90, 100, 100, 90, 90);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    TxtEntryNo.Focus();
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Pairing Production Entry - View", "Select EntryNo, EntryDate, S1.shiftcode2 Shift, S2.Unit_Name Unit, F1.RowID, F1.ShiftCode, F1.Unit_Code, F1.Timing, F1.Remarks From Floor_Socks_Pairing_Production_Master F1 Left Join VAAHINI_ERP_GAINUP.Dbo.Shiftmst S1 On F1.ShiftCode = S1.shiftcode And S1.compcode = 2 And S1.Mode = 1 And S1.shiftcode in (15, 16, 17) Left Join Socks_Unit_Master S2 On F1.Unit_Code = S2.RowID Order by F1.EntryNo Desc ", String.Empty, 90, 100, 100, 90, 90);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    TxtEntryNo.Focus();
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

        private void FrmPairingProductionWithRejection_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                DtpDate1.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Grid_Data()
        {
            String Str = String.Empty;
            try
            {
                if (MyParent._New)
                {
                    Str = " Select F2.Slno Slno, F2.Production_Details_Slno1 Slno1, F2.MachineID, Machine_Name Machine, F2.Order_No, S1.color Sample, F2.OrderColorID, S1.Item, F2.ItemID, S1.Size, F2.SizeID, S1.Bom_Qty BomQty, F2.Production, F2.Rejection, F2.Remarks, '' T From Floor_Socks_Pairing_Production_Master F1 ";
                    Str = Str + " Left Join Floor_Socks_Pairing_Production_Details F2 on F1.RowID = F2.MasterID Left Join Socks_Bom() S1 On F2.Order_No = S1.Order_No And F2.OrderColorID = S1.OrderColorId And F2.ItemID = S1.Itemid And F2.SizeID = S1.sizeid Left Join Socks_Packing_Machine_List S2 On F2.MachineID = S2.RowID Where 1 = 2";
                }
                else
                {
                    Str = " Select F2.Slno Slno, F2.Production_Details_Slno1 Slno1, F2.MachineID, Machine_Name Machine, F2.Order_No, S1.color Sample, F2.OrderColorID, S1.Item, F2.ItemID, S1.Size, F2.SizeID, S1.Bom_Qty BomQty, F2.Production, F2.Rejection, F2.Remarks, '' T From Floor_Socks_Pairing_Production_Master F1 ";
                    Str = Str + " Left Join Floor_Socks_Pairing_Production_Details F2 on F1.RowID = F2.MasterID Left Join Socks_Bom() S1 On F2.Order_No = S1.Order_No And F2.OrderColorID = S1.OrderColorId And F2.ItemID = S1.Itemid And F2.SizeID = S1.sizeid Left Join Socks_Packing_Machine_List S2 On F2.MachineID = S2.RowID Where F1.RowID = " + Code + " Order By F2.Slno";
                }

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "SLNO1", "MachineID", "OrderColorID", "ItemID", "SizeID", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Machine", "Order_No", "Production", "Rejection", "Remarks");
                MyBase.Grid_Width(ref Grid, 50, 140, 120, 90, 90, 90, 90, 90, 90, 90);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);

                Grid.Columns["Production"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Production"].DefaultCellStyle.Format = "0";
                Grid.Columns["Rejection"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Rejection"].DefaultCellStyle.Format = "0";

                Grid.RowHeadersWidth = 10;

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (!MyParent._New)
                    {
                        if (Convert.ToInt16(Grid["Rejection", i].Value) > 0)
                        {
                            TxtQty.Text = Grid["Rejection", i].Value.ToString();
                            Vis = 1;
                            Pos = i;
                            Grid_Data_Qty(Convert.ToInt16(Grid["Slno1", i].Value));
                            Vis = 0;
                            Pos = 0;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Grid_Data_Qty(Int32 Row)
        {
            try
            {
                if (DtQty[Row] == null)
                {
                    DtQty[Row] = new DataTable();
                    if (MyParent._New)
                    {
                        Str = "Select 0 SLNO, '' PROBLEM, RejectionQty, RejectionID, Production_Details_Slno1 SLNO1 From Floor_Socks_Pairing_Production_Rejection_Details WHERE 1 = 2";
                        MyBase.Load_Data(Str, ref DtQty[Row]);
                    }
                    else
                    {
                        if (MyParent.Edit)
                        {
                            if (Vis == 1)
                            {
                                Str = " Select F2.Slno, F3.Problem, F2.RejectionQty, F2.RejectionID, F1.Production_Details_Slno1 SLNO1 From Floor_Socks_Pairing_Production_Details F1 Left Join Floor_Socks_Pairing_Production_Rejection_Details F2 On F1.MasterID = F2.MasterID And F1.Production_Details_Slno1 = F2.Production_Details_Slno1 ";
                                Str = Str + " Left Join Floos_Socks_Pairing_Rejection_Reason_Master F3 on F2.RejectionID = F3.RowID Where F1.MasterID = " + Code + " and F1.Production_Details_Slno1 = " + Grid["Slno1", Pos].Value.ToString();
                                MyBase.Load_Data(Str, ref DtQty[Row]);
                            }
                            else
                            {
                                Str = " Select F2.Slno, F3.Problem, F2.RejectionQty, F2.RejectionID, F1.Production_Details_Slno1 SLNO1 From Floor_Socks_Pairing_Production_Details F1 Left Join Floor_Socks_Pairing_Production_Rejection_Details F2 On F1.MasterID = F2.MasterID And F1.Production_Details_Slno1 = F2.Production_Details_Slno1 ";
                                Str = Str + " Left Join Floos_Socks_Pairing_Rejection_Reason_Master F3 on F2.RejectionID = F3.RowID Where F1.MasterID = " + Code + " and F1.Production_Details_Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();
                                MyBase.Load_Data(Str, ref DtQty[Row]);
                            }

                        }
                        else
                        {
                            Str = " Select F2.Slno, F3.Problem, F2.RejectionQty, F2.RejectionID, F1.Production_Details_Slno1 SLNO1 From Floor_Socks_Pairing_Production_Details F1 Left Join Floor_Socks_Pairing_Production_Rejection_Details F2 On F1.MasterID = F2.MasterID And F1.Production_Details_Slno1 = F2.Production_Details_Slno1 ";
                            Str = Str + " Left Join Floos_Socks_Pairing_Rejection_Reason_Master F3 on F2.RejectionID = F3.RowID Where F1.MasterID = " + Code + " and F1.Production_Details_Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();
                            MyBase.Load_Data(Str, ref DtQty[Row]);
                        }
                    }
                }

                GridQty.DataSource = DtQty[Row];
                MyBase.Grid_Designing(ref GridQty, ref DtQty[Row], "RejectionID", "SLNO1");
                MyBase.ReadOnly_Grid_Without(ref GridQty, "PROBLEM", "RejectionQty");
                MyBase.Grid_Colouring(ref GridQty, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridQty, 50, 200, 100);

                GridQty.RowHeadersWidth = 30;
                GridQty.Columns["RejectionQty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridQty.Columns["RejectionQty"].DefaultCellStyle.Format = "0";
                Balance_Pieces();

                if (MyParent.Edit && Vis == 1)
                {
                    GBQty.Visible = false;
                }
                else
                {
                    GBQty.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Balance_Pieces()
        {
            try
            {
                TxtEnteredPieces.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref GridQty, "RejectionQty", "RejectionID")));
                if (TxtEnteredPieces.Text.Trim() == String.Empty)
                {
                    TxtBalance.Text = String.Format("{0:0}", Convert.ToDouble(TxtQty.Text));
                }
                else
                {
                    if (TxtQty.Text.Trim() != String.Empty)
                    {
                        TxtBalance.Text = String.Format("{0:0}", (Convert.ToDouble(TxtQty.Text) - Convert.ToDouble(TxtEnteredPieces.Text)));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmPairingProductionWithRejection_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtShift")
                    {
                        Shift_Selection();
                    }
                    else if (this.ActiveControl.Name == "TxtUnit")
                    {
                        Unit_Selection();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtUnit")
                    {
                        Grid.Focus();
                        Grid.CurrentCell = Grid["Machine", 0];
                        Grid.BeginEdit(true);
                        return;
                    }
                    else if (this.ActiveControl.Name == "TxtEntryNo")
                    {
                        Grid.Focus();
                        Grid.CurrentCell = Grid["Machine", 0];
                        Grid.BeginEdit(true);
                        return;
                    }
                    else if (this.ActiveControl.Name == "TxtTotal")
                    {
                        if (MyParent._New || MyParent.Edit)
                        {
                            MyParent.Load_SaveEntry();
                        }
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
                    if (GBQty.Visible == true)
                    {

                    }
                    else
                    {
                        MyBase.ActiveForm_Close(this, MyParent);
                    }
                }
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

        void Unit_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Unit", "Select Unit_Name Unit, RowId UnitCode From Socks_Unit_Master", String.Empty, 80, 80);
                if (Dr != null)
                {
                    TxtUnit.Text = Dr["Unit"].ToString();
                    TxtUnit.Tag = Dr["UnitCode"].ToString();
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine_Name Machine, Machine_ID Machine_Code, '' Remarks From Get_Machine_List_Packing(" + TxtUnit.Tag + ") Where Machine_Name Like '%Pair%' Or Machine_Name Like '%Machine%'", String.Empty, 200, 250);
                if (Dr != null)
                {
                    Grid["Machine", Grid.CurrentCell.RowIndex].Value = Dr["Machine"].ToString();
                    Txt.Text = Dr["Machine"].ToString();
                    Grid["MachineID", Grid.CurrentCell.RowIndex].Value = Dr["Machine_Code"].ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmPairingProductionWithRejection_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtEntryNo" || this.ActiveControl.Name == "TxtTotal")
                {
                    MyBase.Valid_Null((TextBox)Txt, e);
                }
                if (this.ActiveControl is TextBox && this.ActiveControl.Name != "TxtRemarks")
                {
                    if (this.ActiveControl.Name != String.Empty)
                    {
                        MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                    }
                }
                if (this.ActiveControl.Name == "DtpDate1")
                {
                    if (Dt.Rows.Count > 0)
                    {
                        e.Handled = true;
                        MessageBox.Show("Already Details Entered..!", "Gainup");
                        return;
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
                    Txt.Enter += new EventHandler(Txt_Enter);
                    Txt.Leave += new EventHandler(Txt_Leave);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void Total()
        {
            try
            {
                TxtTotal.Text = MyBase.Sum(ref Grid, "Production").ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Txt_Enter(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                {
                    MyBase.Row_Number(ref Grid);
                    //if (Grid.CurrentCell.RowIndex > 0)
                    //{
                    //    if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value == null || Grid["Order_No", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    //    {
                    //        Txt.Text = Grid["Order_No", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                    //        Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Grid["Order_No", Grid.CurrentCell.RowIndex - 1].Value;
                    //    }
                    //}
                    Total();
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
                //Grid.Refresh();
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Production"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Rejection"].Index)
                {
                    MyBase.Valid_Number(Txt, e);
                }
                else
                {
                    MyBase.Valid_Null(Txt, e);
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
                //Str = "Select S1.Order_No, S1.Color Sample, S1.Item, S1.SizeID, S1.Size, S1.Bom_Qty BomQty, ISNULL(CAST(S1.AllowancePer as Varchar),'NOT AVAILABLE') Allowance, (S1.Bom_Qty - Isnull(K1.Knitted, 0)) Balance_knitting, S1.Order_Qty, S1.ItemID, S1.OrderColorID From Socks_Bom () S1 Left Join Knitting_Production_All () k1 on S1.Order_No = K1.OrderNo and S1.OrderColorId = K1.OrderColorID and S1.sizeid = K1.SizeID Left Join Fit_Order_Status F1 On S1.Order_No = F1.Order_No Where F1.Order_No is null Order By S1.Order_No";
                Str = " Select Order_No, Color Sample, Item, Size, Bom_Qty BomQty, ISNULL(CAST(AllowancePer as Varchar),'NOT AVAILABLE') Allowance, Order_Qty, ItemID, OrderColorID, SizeID from Socks_Bom() Where Isnull(Despatch_Closed,'N')= 'N' Order By Order_No";
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Order", Str, String.Empty, 120, 150, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                    Txt.Text = Dr["Order_No"].ToString();
                    Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Dr["OrderColorID"].ToString();
                    Grid["Sample", Grid.CurrentCell.RowIndex].Value = Dr["Sample"].ToString();
                    Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                    Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                    Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                    Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                    Grid["BomQty", Grid.CurrentCell.RowIndex].Value = Dr["BomQty"].ToString();
                    Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                    {
                        OrderNo_Selection();
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

        Int16 Max_Slno_Grid()
        {
            Int16 No = 0;
            try
            {
                if (Dt.Rows.Count == 0)
                {
                    No = 1;
                    return No;
                }
                else
                {
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (No < Convert.ToInt16(Dt.Rows[i]["Slno1"]))
                        {
                            No = Convert.ToInt16(Dt.Rows[i]["Slno1"]);
                        }
                    }
                }
                No += 1;
                return No;
            }
            catch (Exception ex)
            {
                return No;
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                    {
                        if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            MessageBox.Show("Order No ...!", "Gainup");
                            Grid.CurrentCell = Grid["Order_No", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rejection"].Index)
                    {
                        if (Grid["Production", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            if (Txt.Text.Trim() != String.Empty)
                            {
                                if (Grid["Production", Grid.CurrentCell.RowIndex].Value == null || Grid["Production", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Production", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                                {
                                    Grid["Production", Grid.CurrentCell.RowIndex].Value = "0";
                                }

                                if ((Convert.ToDouble(Grid["Rejection", Grid.CurrentCell.RowIndex].Value) + Convert.ToDouble(Grid["Production", Grid.CurrentCell.RowIndex].Value)) > Convert.ToDouble(Grid["BomQty", Grid.CurrentCell.RowIndex].Value))
                                {
                                    e.Handled = true;
                                    MessageBox.Show("Invalid Qty ...!", "Gainup");
                                    Grid["Rejection", Grid.CurrentCell.RowIndex].Value = String.Format("{0:0}", (Convert.ToDouble(Grid["BomQty", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["Production", Grid.CurrentCell.RowIndex].Value)));
                                    Grid.CurrentCell = Grid["Rejection", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    return;
                                }

                                if (Convert.ToInt16(Grid["Rejection", Grid.CurrentCell.RowIndex].Value) > 0)
                                {
                                    GBQty.Visible = true;
                                    e.Handled = true;
                                    TxtQty.Text = Grid["Rejection", Grid.CurrentCell.RowIndex].Value.ToString();
                                    Grid_Data_Qty(Convert.ToInt16(Grid["Slno1", Grid.CurrentCell.RowIndex].Value));
                                    GridQty.Focus();
                                    GridQty.CurrentCell = GridQty["PROBLEM", 0];
                                    GridQty.BeginEdit(true);
                                    return;
                                }
                            }
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
                    {
                        if (Grid["Remarks", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Remarks", Grid.CurrentCell.RowIndex].Value = "-";
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
                    //Grid.Refresh();
                    Balance_Pieces();
                    Total();
                    TxtRemarks.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Leave(object sender, EventArgs e)
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
                if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            if (DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)] != null)
                            {
                                DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)] = null;
                            }
                        }
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                        MyBase.Row_Number(ref Grid);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridQty_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref GridQty, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], GridQty.CurrentCell.RowIndex);
                DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].AcceptChanges();
                MyBase.Row_Number(ref GridQty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridQty_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Qty == null)
                {
                    Txt_Qty = (TextBox)e.Control;
                    Txt_Qty.KeyDown += new KeyEventHandler(Tx_Qty_KeyDown);
                    Txt_Qty.KeyPress += new KeyPressEventHandler(Tx_Qty_KeyPress);
                    Txt_Qty.TextChanged += new EventHandler(Tx_Qty_TextChanged);
                    Txt_Qty.GotFocus += new EventHandler(Tx_Qty_GotFocus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Tx_Qty_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["RejectionQty"].Index)
                {
                    if (Txt_Qty.Text.Trim() == String.Empty)
                    {
                        Txt_Qty.Text = TxtBalance.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Tx_Qty_TextChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Tx_Qty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["Problem"].Index)
                {
                    e.Handled = true;
                }
                else if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["RejectionQty"].Index)
                {
                    MyBase.Valid_Number(Txt, e);
                    Balance_Pieces();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Tx_Qty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["Problem"].Index)
                    {
                        Str = "Select Problem, RowID From Floos_Socks_Pairing_Rejection_Reason_Master";
                        Dr = Tool.Selection_Tool_Except_New("Problem", this, 100, 100, ref DtQty[Convert.ToInt16(Grid["SLNO1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Problem...!", Str, String.Empty, 150);
                        if (Dr != null)
                        {
                            Txt_Qty.Text = Dr["Problem"].ToString();
                            GridQty["Problem", GridQty.CurrentCell.RowIndex].Value = Dr["Problem"].ToString();
                            GridQty["RejectionID", GridQty.CurrentCell.RowIndex].Value = Dr["RowID"].ToString();
                            GridQty["Slno1", GridQty.CurrentCell.RowIndex].Value = Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButOk_Click(object sender, EventArgs e)
        {
            try
            {
                Balance_Pieces();
                if (TxtBalance.Text.Trim() == String.Empty)
                {
                    GBQty.Visible = false;
                    return;
                }
                else
                {
                    if (Convert.ToDouble(TxtBalance.Text) == 0)
                    {
                        GBQty.Visible = false;
                        Grid.Focus();
                        Grid.CurrentCell = Grid["Remarks", Grid.CurrentCell.RowIndex];
                        Grid.BeginEdit(true);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Invalid Problem Details ...!", "Gainup");
                        GridQty.CurrentCell = GridQty["Problem", 0];
                        GridQty.Focus();
                        GridQty.BeginEdit(true);
                        return;
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            try
            {
                GBQty.Visible = false;
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
                        if ((Grid["Order_No", Grid.CurrentCell.RowIndex].Value == null || Grid["Order_No", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty) && Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            OrderNo_Selection();
                            if (MyParent.Edit)
                            {
                                //Grid.Focus();
                                //Grid.CurrentCell = Grid["ORDER_NO", Grid.CurrentCell.RowIndex];
                                //Grid.BeginEdit(true);
                                //return;
                                SendKeys.Send("{F2}");
                                SendKeys.Send("{End}");
                            }
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
                            if (MyParent.Edit)
                            {
                                //Grid.Focus();
                                //Grid.CurrentCell = Grid["ORDER_NO", Grid.CurrentCell.RowIndex];
                                //Grid.BeginEdit(true);
                                //return;
                                SendKeys.Send("{F2}");
                                SendKeys.Send("{End}");
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

        private void GridQty_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (GridQty.Rows.Count > 1)
                {
                    MyBase.Row_Number(ref GridQty);
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


    }
}
