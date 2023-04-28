using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using Accounts;
using System.IO;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmEmbroidery : Form, Entry
    {

        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable[] DtQty;
        DataRow Dr;
        Int64 Code;
        Int64 i;
        TextBox Txt = null;
        TextBox TxtIss = null;
        String[] Queries;
        String Str;
        String Buffer_Table = String.Empty;
        Boolean Buffer_Update = false;
        Int64 Mode = 0;

        Int64 ItemID = 0;
        Int64 ColorID = 0;
        Int64 SizeID = 0;
        Int32 Delivery_No = 0;

        Int32 Row1; 

        public FrmEmbroidery()
        {
            InitializeComponent();
        }

        private void FrmEmbroidery_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                DtpDate1.Focus();
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
                DtpDate1.Focus();
                Set_Min_Max_Date(true);
                Grid_Data();
                DtQty = new DataTable[30];
                return;
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
                DtQty = new DataTable[30];
                Set_Min_Max_Date(true);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Embroidery Entry - Edit", "Select EntryNo, EntryDate, ShiftCode, Timing, RowID, Remarks from Floor_Embroidery_Master Where EntryDate >= Dateadd (D, " + MyParent.User_Datelock + ", Cast(GetDate() as Date)) ", String.Empty, 80, 100, 150, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Cancel()
        {
            MyBase.Clear(this);
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

        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                Set_Min_Max_Date(false);
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Embroidery Entry - View", "Select EntryNo, EntryDate, ShiftCode, Timing, RowID, Remarks from Floor_Embroidery_Master", String.Empty, 80, 100, 150, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Save()
        {
            try
            {
                Int32 Array_Index = 0;
                String From_Store = String.Empty;
                Total_Count();

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Details..!", "Gainup");
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtTotal.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotal.Text) == 0)
                {
                    MessageBox.Show("Invalid Details", "Gainup");
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {

                    if (Grid["Total_Set", i].Value == DBNull.Value || Grid["Total_Set", i].Value.ToString() == String.Empty || Convert.ToInt64(Grid["Total_Set", i].Value) == 0 || Grid["Total_Piecs", i].Value == DBNull.Value || Grid["Total_Piecs", i].Value.ToString() == String.Empty || Convert.ToInt64(Grid["Total_Piecs", i].Value) == 0)
                    {
                        MessageBox.Show(" ZERO is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Total_Set", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (DtQty[Convert.ToInt32(Dt.Rows[i]["SlNo1"])] == null)
                    {
                        MessageBox.Show("Invalid Stich Details ...!", "Gainup");
                        MyParent.Save_Error = true;
                        Grid.CurrentCell = Grid["Remarks", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                }


                TxtEntryNo.Text = MyBase.MaxOnlyComp("Floor_Embroidery_Master ", "EntryNo", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();
                Queries = new string[Dt.Rows.Count * 100];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Floor_Embroidery_Master (EntryNo, EntryDate, ShiftCode, Timing, Company_Code, EntryTime, EntrySystem, Remarks) values (" + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', " + TxtShift.Text + ", '" + TxtTiming.Text.ToString() + "', " + MyParent.CompCode + ", GETDATE(), HOST_NAME(), '" + TxtRemarks.Text + "'); Select Scope_Identity() ";
                }
                else
                {
                    Queries[Array_Index++] = "Update Floor_Embroidery_Master Set EntryDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', Remarks = '" + TxtRemarks.Text + "' Where RowID = " + Code;
                    Queries[Array_Index++] = "Delete from Floor_Embroidery_Details where MasterID = " + Code;
                    Queries[Array_Index++] = "Delete from Floor_Embroidery_Stich_Details  where MasterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Floor_Embroidery_Details (MasterID, Order_No, OrderColorID, SizeID, BomQty, SlNo, SlNo1, Emplno_Operator, Emplno_Framer, Total_Set, Total_Piecs, ItemID) Values (@@IDENTITY, '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value + ", " + Grid["SizeID", i].Value + ", " + Grid["Bom", i].Value + ",  " + Grid["SlNo", i].Value + ",  " + Grid["SlNo1", i].Value + ", " + Grid["Emplno_Operator", i].Value + ", " + Grid["Emplno_Framer", i].Value + ", " + Grid["Total_Set", i].Value + ", " + Grid["Total_Piecs", i].Value + ", " + Grid["ItemID", i].Value + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Floor_Embroidery_Details (MasterID, Order_No, OrderColorID, SizeID, BomQty, SlNo, SlNo1, Emplno_Operator, Emplno_Framer, Total_Set, Total_Piecs, ItemID) Values (" + Code + ", '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value + ", " + Grid["SizeID", i].Value + ", " + Grid["Bom", i].Value + ",  " + Grid["SlNo", i].Value + ",  '" + Grid["SlNo1", i].Value + "', " + Grid["Emplno_Operator", i].Value + ", " + Grid["Emplno_Framer", i].Value + ", " + Grid["Total_Set", i].Value + ", " + Grid["Total_Piecs", i].Value + ", " + Grid["ItemID", i].Value + ")";
                    }
                }

                for (int i = 1; i <= Dt.Rows.Count; i++)
                {
                    if (DtQty[i] != null)
                    {
                        for (int j = 0; j <= DtQty[i].Rows.Count - 1; j++)
                        {
                            if (MyParent._New)
                            {
                                Queries[Array_Index++] = "Insert Into Floor_Embroidery_Stich_Details (MasterID, SlNo1, SlNo, StartTime, EndTime,  Pes, Stich) Values (@@IDENTITY, " + Dt.Rows[i - 1]["SlNo1"].ToString() + ", " + DtQty[i].Rows[j]["SlNo"].ToString() + ", '" + DtQty[i].Rows[j]["StartTime"].ToString() + "', '" + DtQty[i].Rows[j]["EndTime"].ToString() + "', " + DtQty[i].Rows[j]["PES"] + ", '" + DtQty[i].Rows[j]["STICH"].ToString() + "')";
                            }
                            else
                            {
                                Queries[Array_Index++] = "Insert Into Floor_Embroidery_Stich_Details (MasterID, SlNo1, SlNo, StartTime, EndTime,  Pes, Stich) Values (" + Code + ", " + Dt.Rows[i - 1]["SlNo1"].ToString() + ", " + DtQty[i].Rows[j]["SlNo"].ToString() + ", '" + DtQty[i].Rows[j]["StartTime"].ToString() + "', '" + DtQty[i].Rows[j]["EndTime"].ToString() + "', " + DtQty[i].Rows[j]["PES"] + ", '" + DtQty[i].Rows[j]["STICH"].ToString() + "')";
                            }
                        }
                    }
                }

                if (MyParent._New)
                {
                    MyBase.Run_Identity(false, Queries);
                    //MyBase.Run(Str);
                }
                else
                {
                    MyBase.Run_Identity(true, Queries);
                }
                MyParent.Save_Error = false;
                MessageBox.Show("Saved ..!", "Gainup");
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
                Set_Min_Max_Date(true);
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Embroidery Entry - Delete", "Select EntryNo, EntryDate, ShiftCode, Timing, RowID, Remarks from Floor_Embroidery_Master", String.Empty, 80, 100, 150);
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
                    MyBase.Run("Delete from Floor_Embroidery_Stich_Details where MasterID = " + Code, "Delete from Floor_Embroidery_Details where MasterID = " + Code, "Delete from Floor_Embroidery_Master where RowID = " + Code);
                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyBase.Clear(this);
                }
                MyParent.Load_DeleteEntry();
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
                TxtEntryNo.Text = Dr["EntryNo"].ToString();
                DtpDate1.Value = Convert.ToDateTime(Dr["EntryDate"]);
                TxtShift.Text = Dr["ShiftCode"].ToString();
                TxtTiming.Text = Dr["Timing"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();
                Grid_Data();
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Grid_Data()
        {
            String Str = String.Empty;
            try
            {
                if (MyParent._New == true)
                {
                    Str = "Select 0 as SlNo, F1.Order_No, S1.Buyer, S1.color Sample, S1.Size, S1.Item, Cast(''As Varchar) Operator, Cast(''As Varchar) Framer, F1.Emplno_Operator, F1.Emplno_Framer, S1.Bom_Qty Bom, F1.OrderColorID, F1.SizeID, F1.ItemID, 0 as SlNo1, 0 as SlNo_Temp, 0 As Total_Set, 0 As Total_Piecs, '-' T From Floor_embroidery_details F1 Left Join Socks_Bom() S1 On F1.Order_No = S1.Order_No And F1.OrderColorID = S1.OrderColorId And F1.SizeID = S1.sizeid And F1.ItemID = S1.Itemid Where 1 = 2";
                }
                else
                {
                    Str = "Select F2.SlNo SlNo, F2.Order_No, S1.Buyer, S1.color Sample, S1.size, S1.item, E1.Name Operator, E2.Name Framer, F2.Emplno_Operator, F2.Emplno_Framer, S1.Bom_Qty Bom, F2.OrderColorID, F2.SizeID, F2.ItemID, F2.SlNo1 SlNo1, F2.SlNo1 SlNo_Temp, F2.Total_Set, F2.Total_Piecs, '-' T From FITSOCKS.dbo.Floor_Embroidery_Master F1 Left Join FITSOCKS.dbo.Floor_Embroidery_Details F2 On F1.RowID = F2.MasterID Left Join Socks_Bom()  S1 On F2.Order_No = S1.Order_No And F2.OrderColorID = S1.OrderColorId And F2.SizeID = S1.sizeid And F2.ItemID = S1.Itemid Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 On F2.Emplno_Operator = E1.Emplno Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E2 On F2.Emplno_Framer = E2.Emplno Where F1.RowID = " + Code;
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Emplno_Operator", "Emplno_Framer", "Bom", "OrderColorID", "ItemID", "SizeID", "SlNo1", "SlNo_Temp", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Order_No", "Operator", "Framer", "Total_Set", "Total_Piecs");
                MyBase.Grid_Width(ref Grid, 40, 110, 100, 90, 90, 90, 150, 150, 90, 90);
                Grid.Columns["Total_Set"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Total_Piecs"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch (Exception ex)
            {
                throw ex;
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

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
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

        private void Grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt == null)
                {
                    Txt = (TextBox)e.Control;
                    Txt.Enter += new EventHandler(Txt_Enter);
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

        void Txt_Enter(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index)
                {
                    MyBase.Row_Number(ref Grid);
                    Total_Count();
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
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index)
                    {
                        Operator_Selection();
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Framer"].Index)
                    {
                        Framer_Selection();
                    }
                }
                Total_Count();

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
                String Str;
                Str = "Select S1.Order_No, S1.Buyer, S1.Color Sample, S1.Size, S1.Item, S1.Bom_Qty Bom, S1.ItemID, S1.OrderColorID, S1.SizeID From Socks_Bom () S1 Order by S1.Order_No";
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Order", Str, String.Empty, 120, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                    Txt.Text = Dr["Order_No"].ToString();
                    Grid["Buyer", Grid.CurrentCell.RowIndex].Value = Dr["Buyer"].ToString();
                    Grid["Sample", Grid.CurrentCell.RowIndex].Value = Dr["Sample"].ToString();
                    Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                    Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                    Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                    Grid["Bom", Grid.CurrentCell.RowIndex].Value = Dr["Bom"].ToString();
                    Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                    Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Dr["OrderColorID"].ToString();
                    Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
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
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Operator", "Select Name, Tno, Emplno From Operator_Selection_Embroidery()", String.Empty, 250, 80);
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

        void Framer_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Operator", "Select Name, Tno, Emplno From Operator_Selection_Embroidery()", String.Empty, 250, 80);
                if (Dr != null)
                {
                    Grid["Framer", Grid.CurrentCell.RowIndex].Value = Dr["Name"].ToString();
                    Txt.Text = Dr["Name"].ToString();
                    Grid["EmplNo_Framer", Grid.CurrentCell.RowIndex].Value = Dr["Emplno"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
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

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Null(Txt, e);
                
                Total_Count();
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index)
                {
                    if ((Grid["Operator", Grid.CurrentCell.RowIndex].Value.ToString()) == String.Empty)
                    {
                        MessageBox.Show("Invalid Operator..!", "Gainup");
                        Grid.CurrentCell = Grid["Operator", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Framer"].Index)
                {
                    if ((Grid["Framer", Grid.CurrentCell.RowIndex].Value.ToString()) == String.Empty)
                    {
                        MessageBox.Show("Invalid Framer..!", "Gainup");
                        Grid.CurrentCell = Grid["Framer", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;                      
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Total_Set"].Index)
                {
                    Total_Count();
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
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Framer"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        Grid["Emplno_Framer", Grid.CurrentCell.RowIndex].Value = Grid["Emplno_Framer", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Framer", Grid.CurrentCell.RowIndex].Value = Grid["Framer", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Txt.Text = Grid["Framer", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void TxtIss_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["STICH"].Index)
                {
                    Total_Count1();
                }
                else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["EndTime"].Index && GridDetail.CurrentCell.Value.ToString() == String.Empty)
                {
                    if (GridDetail.CurrentCell.RowIndex > 0)
                    {
                        GridDetail["PES", GridDetail.CurrentCell.RowIndex].Value = GridDetail["PES", GridDetail.CurrentCell.RowIndex - 1].Value.ToString();
                        GridDetail["STICH", GridDetail.CurrentCell.RowIndex].Value = GridDetail["STICH", GridDetail.CurrentCell.RowIndex - 1].Value.ToString();
                        Txt.Text = GridDetail["PES", GridDetail.CurrentCell.RowIndex - 1].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        void TxtIss_LostFocus(object sender, EventArgs e)
        {
            try
            {
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["PES"].Index)
                {
                    Total_Count1();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void Total_Count()
        {
            try
            {
                TxtTotal.Text = MyBase.Sum(ref Grid, "Total_Set");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Total_Count1()
        {
            try
            {
                GridDetail.Refresh();  
                TxtTotalSet.Text = MyBase.Count(ref GridDetail, "Total_Set");
                GridDetail.Refresh();  
                TxtTotalPEC.Text = MyBase.Sum(ref GridDetail, "PES"); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void GridDetail_Data(Int32 Row)
        {

            try
            {
                Row1 = Row;
                if (DtQty[Row] == null)
                {
                    DtQty[Row] = new DataTable();
                    if (MyParent._New)
                    {
                        MyBase.Load_Data("Select 0 SlNo, Cast('' As Varchar) StartTime, Cast('' As Varchar) EndTime, Cast('' As Int) PES, Cast('' As Int) STICH, " + Row + " SlNo1, '' T from Yarn_Dyeing_Requirement_Details() where 1 = 2 ", ref DtQty[Row]);
                    }
                    else
                    {
                        MyBase.Load_Data("Select F3.SlNo, F3.StartTime, F3.EndTime, F3.PES, F3.Stich, F2.SlNo1, '-' T from Floor_Embroidery_Master F1 Left Join Floor_Embroidery_Details F2 On F1.RowID = F2.MasterID Left Join Floor_Embroidery_Stich_Details F3 On F1.RowID = F3.MasterID And F2.SlNo1 = F3.SlNo1 Where F1.RowID = " + Code + " And F2.SlNo1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString(), ref DtQty[Row]);
                    }
                }
                GridDetail.DataSource = DtQty[Row];
                MyBase.Grid_Designing(ref GridDetail, ref DtQty[Row], "SlNo1", "T");
                MyBase.ReadOnly_Grid_Without(ref GridDetail, "StartTime", "EndTime", "PES", "STICH");
                MyBase.Grid_Colouring(ref GridDetail, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridDetail, 50, 100, 100, 100, 100);
                GridDetail.Columns["PES"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridDetail.Columns["STICH"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                GridDetail.RowHeadersWidth = 10;

                if (!MyParent._New)
                {

                }

                GBQty.Visible = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GridDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.KeyCode == Keys.Enter)
                {
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["PES"].Index)
                    {
                        if (GridDetail["PES", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["PES", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["PES", GridDetail.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid PES...!", "Gainup");
                            GridDetail.CurrentCell = GridDetail["PES", Grid.CurrentCell.RowIndex];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void TxtIss_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    //if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Order_No"].Index)
                    //{
                    //    Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select A.Order_No, (Isnull(A.Iss_Qty,0)-Isnull(B.Rec_Qty,0))Iss_Qty, (Isnull(A.Iss_Qty,0)-Isnull(B.Rec_Qty,0))Rec_Qty ,A.Itemid, A.Colorid, A.Sizeid  from Orderwise_Dyeing_Issued()A Left Join Orderwise_Dyeing_Received()B on A.Delivery_No = B.Delivery_No and A.Order_no = B.Order_no and A.Itemid = B.Itemid and A.Colorid = B.Colorid and A.Sizeid = B.Sizeid Where A.Delivery_No = " + Delivery_No + " and A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " and (Isnull(A.Iss_Qty,0)-Isnull(B.Rec_Qty,0))>0 Order By A.Order_No", String.Empty, 150, 100, 100);

                    //    if (Dr != null)
                    //    {
                    //        Txt1.Text = Dr["Order_No"].ToString();
                    //        GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                    //        GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value = Dr["Iss_Qty"].ToString();
                    //        GridDetail["Rec_Qty", GridDetail.CurrentCell.RowIndex].Value = Dr["Rec_Qty"].ToString();
                    //    }
                    //}
                }
                Total_Count1();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void TxtIss_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
               if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["StartTime"].Index || GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["EndTime"].Index || GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["PES"].Index)
                {
                    MyBase.Valid_Decimal(TxtIss, e);
                }
                else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["STICH"].Index)
                {
                     
                }
                else
                {
                    e.Handled = true;
                }
                Total_Count1(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void GridDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (TxtIss == null)
                {
                    TxtIss = (TextBox)e.Control;
                    TxtIss.KeyPress += new KeyPressEventHandler(TxtIss_KeyPress);
                    TxtIss.GotFocus += new EventHandler(TxtIss_GotFocus);
                    TxtIss.KeyDown += new KeyEventHandler(TxtIss_KeyDown);
                    TxtIss.GotFocus += new EventHandler(TxtIss_GotFocus);
                    TxtIss.LostFocus += new EventHandler(TxtIss_LostFocus);
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
                int Cnt = DtQty[Row1].Rows.Count;  
                Double Cnt1 = Convert.ToDouble(TxtTotalPEC.Text.ToString());  
                if (TxtTotalSet.Text.Trim() == String.Empty || TxtTotalSet.Text == "0.000")
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    GridDetail.CurrentCell = GridDetail["StartTime", 0];
                    GridDetail.Focus();
                    GridDetail.BeginEdit(true);
                    return;
                }
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Total_SET", (Grid.CurrentCell.RowIndex)];
                Grid["Total_SET", (Grid.CurrentCell.RowIndex)].Value = Cnt; 
                Grid.CurrentCell = Grid["Total_PIECS", (Grid.CurrentCell.RowIndex)];
                Grid["Total_PIECS", (Grid.CurrentCell.RowIndex)].Value = Cnt1;
                Grid.CurrentCell = Grid["Total_PIECS", (Grid.CurrentCell.RowIndex)];
                Grid.Focus();
                Grid.BeginEdit(true);
                return;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButExit_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= DtQty[Row1].Rows.Count - 1; i++)
                {
                    if (GridDetail["PES", i].Value == DBNull.Value || Convert.ToDouble(GridDetail["PES", i].Value) == 0)
                    {
                        MessageBox.Show("Invalid PES ..!", "Gainup");
                        Grid.CurrentCell = Grid["PES", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        GBQty.Visible = false;
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                DtQty = new DataTable[30];
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Total_PIECS", (Grid.CurrentCell.RowIndex)];
                Grid.Focus();
                Grid.BeginEdit(true);
                return;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmEmbroidery_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;

                    if (this.ActiveControl.Name == "TxtShift")
                    {
                        if (TxtShift.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Select Shift..!", "Gainup");
                            return;
                        }
                        else
                        {
                            Grid.CurrentCell = Grid["Order_No", 0];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtTotal")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
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
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
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

        void Shift_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Shift", "Select Shift, StartTime, EndTime, ShiftCode From Socks_Shift_12Hrs()", String.Empty, 80, 80, 80);
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

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {

                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Total_Set"].Index)
                    {

                        //TxtQty1.Text = Grid["Rec_Qty", Grid.CurrentCell.RowIndex].Value.ToString();

                        //ItemID = Convert.ToInt16(Grid["ItemId", Grid.CurrentCell.RowIndex].Value);
                        //ColorID = Convert.ToInt16(Grid["ColorId", Grid.CurrentCell.RowIndex].Value);
                        //SizeID = Convert.ToInt16(Grid["SizeId", Grid.CurrentCell.RowIndex].Value);
                        //Delivery_No = Convert.ToInt16(Grid["Delivery_No", Grid.CurrentCell.RowIndex].Value);

                        GridDetail_Data(Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value));
                        GridDetail.CurrentCell = GridDetail["StartTime", 0];
                        GridDetail.Focus();
                        GridDetail.BeginEdit(true);
                        e.Handled = true;
                        return;

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

        private void GridDetail_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid);
                Total_Count1();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref GridDetail, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], GridDetail.CurrentCell.RowIndex);
                DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].AcceptChanges();
                MyBase.Row_Number(ref GridDetail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmEmbroidery_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtShift" || this.ActiveControl.Name == "TxtTotal")
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridDetail_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (GridDetail.Rows.Count > 1)
                {
                    MyBase.Row_Number(ref GridDetail);
                    Total_Count1();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }


    }
}