using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using Accounts;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmSocks_QC_Entry : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataRow Dr;
        Int64 Code;
        TextBox Txt = null;
        String[] Queries;
        String Str;
        String Buffer_Table = String.Empty;
        Boolean Buffer_Update = false;


        public FrmSocks_QC_Entry()
        {
            this.InitializeComponent();
        }

        private void FrmFGSTransfer_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)MdiParent;
                Buffer_Table = "QC_Knit_" + Environment.MachineName.Replace("-", "") + "_" + MyParent.UserCode.ToString();
                MyBase.Disable_Cut_Copy(GBMain);
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

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                Set_Min_Max_Date(true);
                Buffer_Update = false;
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select QC Entry - Edit", "Select F1.EntryNo, F1.EntryDate, S1.Shiftcode2 Shift, F1.Timing, F1.ShiftCode, F1.Remarks, F1.RowID From Socks_QC_Master F1 Left Join Socks_Shift () S1 on F1.ShiftCode = S1.Shiftcode Where F1.ENtryDate >= Dateadd (D, " + MyParent.User_Datelock + ", Cast(GetDate() as Date))", String.Empty, 80, 90, 70);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Machine", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
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

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Socks QC Entry - Delete", "Select F1.EntryNo, F1.EntryDate, S1.Shiftcode2 Shift, F1.Timing, F1.ShiftCode, F1.Remarks, F1.RowID From Socks_QC_Master F1 Left Join Socks_Shift () S1 on F1.ShiftCode = S1.Shiftcode ", String.Empty, 60, 100, 80);
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
                if (Code > 0)
                {
                    MyBase.Run("Delete from Socks_QC_Details where MasterID = " + Code, "Delete From Socks_QC_Master Where RowID = " + Code, MyParent.EntryLog("SOCKS QC ENTRY", "DELETE", Code.ToString()));
                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyBase.Clear(this);
                }
                this.MyParent.Load_DeleteEntry();
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Socks QC Entry - View", "Select F1.EntryNo, F1.EntryDate, S1.Shiftcode2 Shift, F1.Timing, F1.ShiftCode, F1.Remarks, F1.RowID From Socks_QC_Master F1 Left Join Socks_Shift () S1 on F1.ShiftCode = S1.Shiftcode ", String.Empty, 60, 100, 80);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
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
                MyBase.Clear(this);
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

        Int64 Fill_BOM_Check(String OrderNo, String Sample, String Size)
        {
            try
            {
                Int64 Prod = 0;
                Int64 Bal = 0;
                Int64 Bom = 0;
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select S1.Bom_Qty Bom, Isnull(K1.Tested, 0) Tested, (S1.Bom_Qty - Isnull(K1.Tested, 0)) Balance_QC From Socks_Bom() S1 Left Join Socks_QC_Knitting_All () k1 on S1.Order_No = K1.OrderNo and S1.OrderColorId = K1.OrderColorID and S1.sizeid = K1.SizeID Where S1.Order_No = '" + OrderNo + "' And S1.color = '" + Sample + "' and S1.Size = '" + Size + "'", ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    Bom = Convert.ToInt32(Tdt.Rows[0]["Bom"].ToString());
                    Bal = Bom;
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Dt.Rows[i]["Order_No"].ToString() == OrderNo && Dt.Rows[i]["Sample"].ToString() == Sample && Dt.Rows[i]["Size"].ToString() == Size)
                        {
                            Prod = Convert.ToInt64(Prod) + Convert.ToInt64(Dt.Rows[i]["QCQty"]);
                        }
                    }
                    Bal = Convert.ToInt64(Bal) - Convert.ToInt64(Prod);
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
            String[] Queries;
            Int32 Array_Index = 0;
            double Line_Flag = 0;
            try
            {
                MyBase.Row_Number(ref Grid);
                Total_Prod_Qty();

                if (TxtShift.Text.ToString() == String.Empty || TxtShift.Tag.ToString() == String.Empty || TxtTiming.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Enter Shift...!", "Gainup");
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

                    if (Grid["QCQty", i].Value == DBNull.Value || Grid["QCQty", i].Value.ToString() == String.Empty || Convert.ToInt64(Grid["QCQty", i].Value) == 0)
                    {
                        MessageBox.Show(" ZERO is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["QCQty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }

                    if (Fill_BOM_Check(Grid["Order_No", i].Value.ToString(), Grid["Sample", i].Value.ToString(), Grid["Size", i].Value.ToString()) < 0)
                    {
                        MessageBox.Show("QCQty Value Invalid  in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["QCQty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }

                }

                Dt.AcceptChanges();
                Array_Index = 0;
                Queries = new String[(Dt.Rows.Count * 2) + 5];

                Grid.CurrentCell = Grid[0, 0];

                TxtNo.Text = MyBase.MaxOnlyComp("Socks_QC_Master", "EntryNo", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_QC_Master (EntryNo, EntryDate, ShiftCode, Timing, Company_Code, EntryTime, EntrySystem, Remarks) Values (" + TxtNo.Text + ",'" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "','" + TxtShift.Tag.ToString() + "','" + TxtTiming.Text.ToString() + "'," + MyParent.CompCode + ",getdate(),Host_name(), '" + TxtRemarks.Text + "') ; Select Scope_Identity()";
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_QC_Master Set ShiftCode = " + TxtShift.Tag.ToString() + ", Remarks = '" + TxtRemarks.Text + "' where RowID = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_QC_Details where MAsterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New == true)
                    {
                        Queries[Array_Index++] = "Insert Into Socks_QC_Details (MasterID, MachineID, Order_No, OrderColorID, NeedleID, SizeID, OrderQty, ItemID, BOMQty, QCQty, Problem_ID, Emplno_Operator ) Values (@@IDENTITY, '" + Grid["Machine", i].Value.ToString() + "', '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", '" + Grid["NeedleID", i].Value.ToString() + "', '" + Grid["SizeID", i].Value.ToString() + "', " + Grid["OrderQty", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["Bom", i].Value + ", " + Grid["QCQty", i].Value + ", " + Grid["Problem_ID", i].Value + ", " + Grid["Emplno_Operator", i].Value + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert Into Socks_QC_Details (MasterID, MachineID, Order_No, OrderColorID, NeedleID, SizeID, OrderQty, ItemID, BOMQty, QCQty, Problem_ID, Emplno_Operator) Values (" + Code + ", '" + Grid["Machine", i].Value.ToString() + "', '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", '" + Grid["NeedleID", i].Value.ToString() + "', '" + Grid["SizeID", i].Value.ToString() + "', " + Grid["OrderQty", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["Bom", i].Value + ", " + Grid["QCQty", i].Value + "," + Grid["Problem_ID", i].Value + ", " + Grid["Emplno_Operator", i].Value + ")";
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

        private void FrmFGSTransfer_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (ActiveControl.Name == "TxtShift")
                    {
                        Grid.CurrentCell = Grid["Machine", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                    }
                    else if (ActiveControl.Name == "TxtTotal")
                    {
                        if (MyParent._New || MyParent.Edit)
                        {
                            MyParent.Load_SaveEntry();
                        }
                    }
                    else
                    {
                        SendKeys.Send("{TAB}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtShift")
                    {
                        Shift_Selection();
                    }
                }
                else if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
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

        private void FrmFGSTransfer_KeyPress(object sender, KeyPressEventArgs e)
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

        void Grid_Data(Boolean Buffer)
        {
            String Str = String.Empty;
            DataTable Tdt = new DataTable();
            int month = DtpDate1.Value.Month;
            int day = DtpDate1.Value.Day;
            int year = DtpDate1.Value.Year;
            try
            {
                if (Buffer)
                {
                    Str = "Select 0 as Slno, F1.MachineID Machine, F1.NeedleID, F1.NeedleID Needle, F1.Order_No, F1.OrderQty, F1.ItemID, C1.color Sample, F1.OrderColorID, F1.SizeID, S1.Size, F1.BOMQty Bom, Cast (0 as int)Bal_QC, F1.QCQty, F1.Problem_ID, S2.Name Problem, F1.Emplno_OPerator, E1.Name OPerator, '-' T From " + Buffer_Table + " F1 Left Join VFit_Sample_Needle_Master V2 On F1.NeedleID = V2.RowID Left Join size S1 on F1.SizeID = S1.sizeid Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Left Join Socks_Bom() C1 On F1.OrderColorID = C1.OrderColorId And F1.Order_No = C1.Order_No Left Join Socks_Qc_Problem_Master S2 On F1.problem_ID = S2.RowID Order By F1.Slno";
                }
                else
                {

                    if (MyParent._New)
                    {
                        Str = "Select 0 as Slno, Q1.MachineID Machine, Q1.NeedleID, Q1.NeedleID Needle, Q1.Order_No, Q1.OrderQty, Q1.ItemID, Cast('' As Varchar (15)) Sample, Q1.OrderColorID, Q1.SizeID, S1.Size, Q1.BOMQty Bom, Cast(0 as Bigint) Bal_QC, Q1.QCQty, Q1.Problem_ID, Cast('' as varchar)Problem, Q1.Emplno_OPerator, E1.Name OPerator, '-' T From Socks_QC_Master Q2 Left Join Socks_QC_Details Q1 On Q2.RowID = Q1.MasterID Left Join VFit_Sample_Needle_Master V2 On Q1.NeedleID = V2.RowID Left Join size S1 on Q1.SizeID = S1.sizeid Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E1 on Q1.Emplno_Operator = E1.Emplno Where 1 = 2";
                    }
                    else
                    {
                        Str = "Select 0 as Slno, F1.MachineID Machine, F1.NeedleID, F1.NeedleID Needle, F1.Order_No, F1.OrderQty, F1.ItemID, C1.color Sample, F1.OrderColorID, F1.SizeID, S1.Size, F1.BOMQty Bom, Cast(0 as Bigint) Bal_QC, F1.QCQty, F1.Problem_ID, S2.Name Problem, F1.Emplno_OPerator, E1.Name OPerator, '-' T From Socks_QC_Details F1 Left Join Socks_QC_Master F3 On F1.MasterID = F3.RowID Left Join VFit_Sample_Needle_Master V2 On F1.NeedleID = V2.RowID Left Join Socks_Bom() C1 On F1.OrderColorID = C1.OrderColorId And F1.Order_No = C1.Order_No Left Join size S1 on F1.SizeID = S1.sizeid Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Left Join Socks_Qc_Problem_Master S2 On F1.Problem_ID = S2.RowID Where F1.MasterID = " + Code + " Order By F1.RowID";
                    }
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid, "Machine", "Order_No", "QCQty", "Problem", "Operator");
                MyBase.Grid_Designing(ref Grid, ref Dt, "OrderColorID", "NeedleID", "SizeID", "OrderQty", "ItemID", "Bal_QC", "Emplno_operator", "Problem_ID", "T");
                MyBase.Grid_Width(ref Grid, 50, 100, 100, 150, 110, 110, 110, 110, 160, 200);
                Grid.Columns["Needle"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                Grid.Columns["BOM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["BOM"].DefaultCellStyle.Format = "0";
                
                Grid.Columns["QCQty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["QCQty"].DefaultCellStyle.Format = "0";
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
                    if (Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Needle", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Problem", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Problem_ID", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        if (Grid.CurrentCell.RowIndex > 0)
                        {
                            Grid["Emplno_Operator", Grid.CurrentCell.RowIndex].Value = Grid["Emplno_Operator", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Grid["Operator", Grid.CurrentCell.RowIndex].Value = Grid["Operator", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Txt.Text = Grid["Operator", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Needle"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        Grid["NeedleID", Grid.CurrentCell.RowIndex].Value = Grid["NeedleID", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Needle", Grid.CurrentCell.RowIndex].Value = Grid["Needle", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Txt.Text = Grid["Needle", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                    }
                }

                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Problem"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
                {
                    if (Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Needle", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        if (Grid.CurrentCell.RowIndex > 0)
                        {
                            Grid["Problem_ID", Grid.CurrentCell.RowIndex].Value = Grid["Problem_ID", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Grid["Problem", Grid.CurrentCell.RowIndex].Value = Grid["Problem", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Txt.Text = Grid["Problem", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        }
                    }
                }

                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
                {
                    if (Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Needle", Grid.CurrentCell.RowIndex].Value.ToString()!= String.Empty)
                    {
                        if (Grid.CurrentCell.RowIndex > 0)
                        {
                            Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Grid["Order_NO", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Txt.Text = Grid["Order_NO", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Grid["Sample", Grid.CurrentCell.RowIndex].Value = Grid["Sample", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Grid["SizeID", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Grid["Size", Grid.CurrentCell.RowIndex].Value = Grid["Size", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Grid["Bom", Grid.CurrentCell.RowIndex].Value = Grid["BOM", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Grid["OrderQty", Grid.CurrentCell.RowIndex].Value = Grid["OrderQty", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Grid["ItemID", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Grid["Bal_QC", Grid.CurrentCell.RowIndex].Value = Grid["Bal_QC", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Grid["OrderColorID", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                            Fill_BOM(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString());
                        }
                    }
                }

                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                {
                    LblBOM.Text = "0";
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


        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["QCQty"].Index)
                    {
                        if (Grid["QCQty", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["QCQty", Grid.CurrentCell.RowIndex].Value = "0";
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

                        if (Convert.ToDouble(Grid["QCQty", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(LblBal.Text.Replace("BAL:", "")))
                        {
                            e.Handled = true;
                            MessageBox.Show("Production is greater than BOM ", "Gainup");
                            Grid["QCQty", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(LblBal.Text.Replace("BAL:", ""));
                            Grid.CurrentCell = Grid["QCQty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            Fill_BOM(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString());
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

        void Fill_BOM(String OrderNo, String Sample, String Size)
        {
            try
            {
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select S1.Bom_Qty Bom, Isnull(K1.Tested, 0) Tested, (S1.Bom_Qty - Isnull(K1.Tested, 0)) Balance_Testing From Socks_Bom () S1 Left Join Socks_QC_Knitting_All () k1 on S1.Order_No = K1.OrderNo and S1.OrderColorId = K1.OrderColorID and S1.sizeid = K1.SizeID Where S1.Order_No = '" + OrderNo + "' And S1.color = '" + Sample + "' and S1.Size = '" + Size + "'", ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    LblBOM.Text = "BOM: " + Tdt.Rows[0]["Bom"].ToString();
                    LblPre_Prod.Text = "PROD: " + Tdt.Rows[0]["Tested"].ToString();
                    LblBal.Text = "BAL: " + Tdt.Rows[0]["Balance_Testing"].ToString();

                    if (Grid["QCQty", Grid.CurrentCell.RowIndex].Value == null || Grid["QCQty", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["QCQty", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["QCQty", Grid.CurrentCell.RowIndex].Value = "0";
                    }

                    LblProduction.Text = "0";

                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Grid.CurrentCell.RowIndex != i)
                        {
                            //if (Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Needle", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Convert.ToInt32(Grid["QCQty", Grid.CurrentCell.RowIndex].Value.ToString()) != 0 && Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Problem", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            //if (Dt.Rows[i]["Order_No"].ToString() != String.Empty && Dt.Rows[i]["Sample"].ToString() != String.Empty && Dt.Rows[i]["Size"].ToString() != String.Empty && Dt.Rows[i]["Order_No"] != DBNull.Value && Dt.Rows[i]["Sample"] != DBNull.Value && Dt.Rows[i]["Size"] != DBNull.Value)
                            //{
                                if (Dt.Rows[i]["Order_No"].ToString() == OrderNo && Dt.Rows[i]["Sample"].ToString() == Sample && Dt.Rows[i]["Size"].ToString() == Size)
                                {
                                    LblProduction.Text = String.Format("{0:0}", Convert.ToDouble(LblProduction.Text) + Convert.ToDouble(Dt.Rows[i]["QCQty"]));
                                }
                            //}
                        }                        
                    }

                    LblBal.Text = "BAL: " + String.Format("{0:0}", Convert.ToDouble(LblBal.Text.Replace("BAL: ", "")) - Convert.ToDouble(LblProduction.Text));
                }

                if (!MyParent._New)
                {
                    Tdt = new DataTable();
                    MyBase.Load_Data("Select Isnull(Sum(QCQty), 0) QCQty From Socks_QC_Details Where Order_No = '" + OrderNo + "' And OrderColorID = .Dbo.Get_OrdercolorID ('" + OrderNo + "', '" + Sample + "') and SizeID = Dbo.Get_OrderSizeID ('" + OrderNo + "', '" + Size + "') and MasterID = " + Code, ref Tdt);
                    LblBal.Text = String.Format("{0:0}", Convert.ToDouble(LblBal.Text.Replace("BAL: ", "")) + Convert.ToDouble(Tdt.Rows[0][0]));
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
                if (ex.Message.ToUpper().Contains("System.IndexOutOfRangeException:"))
                {
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
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

        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count) 
                {
                    if(MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);

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
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid);
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Total_Count()
        {
            try
            {
                TxtTotal.Text = MyBase.Sum(ref Grid, "QCQty", "MACHINE");
            }
            catch (Exception ex)
            {
                throw ex;
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

        private void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (e.KeyCode == Keys.Down)
                    {
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                        {
                            Machine_Selection();
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Needle"].Index)
                        {
                            if (Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                Needle_Selection();
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                        {
                            if (Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Needle", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty )
                            {
                                OrderNo_Selection();
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index)
                        {
                            if (Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Needle", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Problem", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Problem_ID", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                Operator_Selection();
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Problem"].Index)
                        {
                            if (Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Needle", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                Problem_Selection();
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        void Total_Prod_Qty()
        {
            try
            {
                TxtTotal.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "QCQty", "Order_No", "Sample", "Operator")));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Problem_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Problem", "Select Name Problem, RowID From Socks_QC_Problem_Master", String.Empty, 150);
                if (Dr != null)
                {
                    Grid["Problem_ID", Grid.CurrentCell.RowIndex].Value = Dr["RowID"].ToString();
                    Grid["Problem", Grid.CurrentCell.RowIndex].Value = Dr["Problem"].ToString();
                    Txt.Text = Dr["Problem"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Machine_Selection()
        {
            DataTable Tdt = new DataTable();
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine, Needle From Knitting_Mc_NO ()", String.Empty, 150, 150);
                if (Dr != null)
                {
                    Grid["Machine", Grid.CurrentCell.RowIndex].Value = Dr["Machine"].ToString();
                    Grid["NeedleID", Grid.CurrentCell.RowIndex].Value = Dr["Needle"].ToString();
                    Grid["Needle", Grid.CurrentCell.RowIndex].Value = Dr["Needle"].ToString();

                    if (DtpDate1.Value >= Convert.ToDateTime("01-JUL-2015".ToString()))
                    {
                        MyBase.Load_Data("select P2.EmplNo, E1.Name Operator from Employee_Production_Master_Socks P1 Left join Employee_Production_Details_Socks P2 On P1.RowID = P2.Master_ID Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 On P2.EmplNo = E1.Emplno where P2.Machine_Name = '" + Dr["Machine"].ToString() + "' and Entry_Date = (select MAX(Entry_Date) from Employee_Production_Master_Socks P1 Left join Employee_Production_Details_Socks P2 On P1.RowID = P2.Master_ID where P2.Machine_Name = '" + Dr["Machine"].ToString() + "')", ref Tdt);
                        if (Tdt.Rows.Count > 0)
                        {
                            Grid["Operator", Grid.CurrentCell.RowIndex].Value = Tdt.Rows[0]["Operator"].ToString();
                            Grid["Emplno_Operator", Grid.CurrentCell.RowIndex].Value = Tdt.Rows[0]["Emplno"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Needle_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Needle", "Select Name Needle, RowID From VFit_Sample_Needle_Master ", String.Empty, 150);
                if (Dr != null)
                {
                    Grid["NeedleID", Grid.CurrentCell.RowIndex].Value = Dr["RowID"].ToString();
                    Grid["Needle", Grid.CurrentCell.RowIndex].Value = Dr["Needle"].ToString();
                    Txt.Text = Dr["Needle"].ToString();
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
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Order", "Select S1.Order_No, S1.Color Sample, S1.SizeID, S1.Size, S1.Bom_Qty Bom, ISNULL(CAST(S1.AllowancePer as Varchar),'NOT AVAILABLE') Allowance, (S1.Bom_Qty - Isnull(K1.Tested, 0)) Balance_Testing, S1.Order_Qty, S1.ItemID, S1.OrderColorID From Socks_Bom () S1 Left Join Socks_QC_Knitting_All() k1 on S1.Order_No = K1.OrderNo and S1.OrderColorId = K1.OrderColorID and S1.sizeid = K1.SizeID ", String.Empty, 120, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    if (Dr["Bom"].ToString() == String.Empty || Dr["Bom"].ToString() == null)
                    {
                        MessageBox.Show("Allowance Percentage Not Available For " + Dr["Order_No"].ToString() + " .....! SOCKS");
                        Grid.CurrentCell = Grid["Order_No", Grid.CurrentCell.RowIndex];
                        Grid["Order_No", Grid.CurrentCell.RowIndex].Value = "";
                        Txt.Text = "";
                        Grid["Sample", Grid.CurrentCell.RowIndex].Value = "";
                        Grid["SizeID", Grid.CurrentCell.RowIndex].Value = 0;
                        Grid["Size", Grid.CurrentCell.RowIndex].Value = "";
                        Grid["Bom", Grid.CurrentCell.RowIndex].Value = 0;
                        Grid["OrderQty", Grid.CurrentCell.RowIndex].Value = 0;
                        Grid["ItemID", Grid.CurrentCell.RowIndex].Value = 0;
                        Grid["Bal_QC", Grid.CurrentCell.RowIndex].Value = 0;
                        Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = 0;
                    }
                    else
                    {
                        Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                        Txt.Text = Dr["Order_No"].ToString();
                        Grid["Sample", Grid.CurrentCell.RowIndex].Value = Dr["Sample"].ToString();
                        Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                        Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                        Grid["Bom", Grid.CurrentCell.RowIndex].Value = Dr["Bom"].ToString();
                        Grid["OrderQty", Grid.CurrentCell.RowIndex].Value = Dr["Order_Qty"].ToString();
                        Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                        Grid["Bal_QC", Grid.CurrentCell.RowIndex].Value = Dr["Balance_Testing"].ToString();
                        Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Dr["OrderColorID"].ToString();
                        Fill_BOM(Dr["Order_No"].ToString(), Dr["Sample"].ToString(), Dr["Size"].ToString());
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
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Operator", "Select Name, Tno, Emplno From Socks_Employee_Present_Detail ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "') Where DeptName = 'Knitting' and Tno Not Like '%Z'", String.Empty, 250, 80);
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

        private void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["QCQty"].Index && Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Needle", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    MyBase.Valid_Number(Txt, e);
                }
                else
                {
                    MyBase.Valid_Null(Txt, e);
                }
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Txt_Leave(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LblSpecial_Click(object sender, EventArgs e)
        {

        }

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentCell.RowIndex);
                Total_Prod_Qty();
                MyBase.Row_Number(ref Grid);
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
                    //if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Convert.ToInt32(Grid["QCQty", Grid.CurrentCell.RowIndex].Value.ToString()) != 0 && Grid["Problem", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty )
                    {
                        Fill_BOM(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString());
                    }
                    else
                    {
                        LblBal.Text = "0";
                        LblPre_Prod.Text = "0";
                        LblProduction.Text = "0";
                        LblBOM.Text = "0";
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
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                {
                    if (Grid.CurrentCell.RowIndex == 0)
                    {
                        if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value == null || Grid["Order_No", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
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
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Problem"].Index)
                {
                    if (Grid.CurrentCell.RowIndex == 0)
                    {
                        if (Grid["Problem", Grid.CurrentCell.RowIndex].Value == null || Grid["Problem", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Problem", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Problem_Selection();
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
                        MyBase.Execute("Select Cast(0 as int) Slno, MachineID, Order_No, OrderColorID, NeedleID, SizeID, BOMQty, QCQty, Problem_ID, Emplno_Operator, OrderQty, ItemID into " + Buffer_Table + " From Socks_QC_Details Where 1 = 2");
                    }

                    MyBase.Execute("Delete From " + Buffer_Table);

                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Grid["Machine", i].Value != null )
                        {
                            //&& Grid["Machine", i].Value.ToString() != null && Grid["Needle", i].Value.ToString() != null && Grid["Order_No", i].Value.ToString() != null && Grid["sample", i].Value.ToString() != null && Grid["Size", i].Value.ToString() != null && Grid["Bom", i].Value.ToString() != null && Grid["QCQty", i].Value.ToString() != null && Grid["Problem", i].Value.ToString() != null && Grid["Operator", i].Value.ToString() != null
                            if (Grid["Machine", i].Value.ToString().Trim() != String.Empty && Grid["Needle", i].Value.ToString() != String.Empty && Grid["Order_No", i].Value.ToString() != String.Empty && Grid["sample", i].Value.ToString() != String.Empty && Grid["Size", i].Value.ToString() != String.Empty && Grid["Bom", i].Value.ToString() != String.Empty && Grid["QCQty", i].Value.ToString() != String.Empty && Grid["Problem", i].Value.ToString() != String.Empty && Grid["Operator", i].Value.ToString() != String.Empty )
                            {
                                Queries[Array_Index++] = " Insert Into " + Buffer_Table + " (Slno, MachineID, Order_No, OrderColorID, NeedleID, SizeID, OrderQty, ItemID, BOMQty, QCQty, Problem_ID, Emplno_Operator) Values (" + Grid["Slno", i].Value.ToString() + ", '" + Grid["Machine", i].Value.ToString() + "', '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", '" + Grid["NeedleID", i].Value.ToString() + "', '" + Grid["SizeID", i].Value.ToString() + "', " + Grid["OrderQty", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["Bom", i].Value + ", " + Grid["QCQty", i].Value + ", " + Grid["Problem_ID", i].Value + ", " + Grid["Emplno_Operator", i].Value + ")";
                            }
                        }
                    }

                    if (Dt.Rows.Count > 0)
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
    }
}
