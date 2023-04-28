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
    public partial class FrmFloorKnitting : Form, Entry
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

        public FrmFloorKnitting()
        {
            InitializeComponent();
        }

        private void FrmFloorKnitting_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                Buffer_Table = "Knit_" + Environment.MachineName.Replace("-", "") + "_" + MyParent.UserCode.ToString();
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Knitting - Edit", "Select F1.EntryNo, F1.ENtryDate, S1.Shiftcode2 Shift, F1.Timing, F1.ShiftCode, F1.Remarks, F1.RowID From Floor_Knitting_Master F1 Left Join Socks_Shift () S1 on F1.ShiftCode = S1.Shiftcode Where F1.ENtryDate >= Dateadd (D, " + MyParent.User_Datelock + ", Cast(GetDate() as Date))", String.Empty, 80, 90, 70);
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

        Int64 Fill_BOM_Check_On_Edit(String OrderNo, String Sample, String Size)
        {
            try
            {
                Int64 Prod = 0;
                Int64 Bal = 0;
                Int64 Bom = 0;
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select BomQty Bom, LinkQty From Floor_Stock F1 Left Join Socks_Bom() S1 On F1.Order_No = S1.Order_No And F1.OrderColorID = S1.OrderColorId And F1.SizeID = S1.sizeid Where F1.Order_No = '" + OrderNo + "'  And S1.color = '" + Sample + "' And S1.Size = '" + Size + "'", ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    Bom = Convert.ToInt32(Tdt.Rows[0]["Bom"].ToString());
                    Bal = Convert.ToInt32(Tdt.Rows[0]["LinkQty"].ToString());
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Dt.Rows[i]["Order_No"].ToString() == OrderNo && Dt.Rows[i]["Sample"].ToString() == Sample && Dt.Rows[i]["Size"].ToString() == Size)
                        {
                            Prod = Convert.ToInt64(Prod) + Convert.ToInt64(Dt.Rows[i]["Production"]);
                        }
                    }

                    if (Bal != 0)
                    {
                        if (Bal > Prod)
                        {
                            MessageBox.Show("Invalid Production, Linking Entry Against.." + OrderNo + "LinkQty: " + Bal + "KnitQty: " + Prod);
                            return -1;
                        }
                    }
                    //Bal = Convert.ToInt64(Bal) - Convert.ToInt64(Prod);
                }
                return Bal;
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
                MyBase.Load_Data("Select S1.Bom_Qty Bom, Isnull(K1.Knitted, 0) Knitted, (S1.Bom_Qty - Isnull(K1.Knitted, 0)) Balance_knitting From Socks_Bom() S1 Left Join Knitting_Production_All () k1 on S1.Order_No = K1.OrderNo and S1.OrderColorId = K1.OrderColorID and S1.sizeid = K1.SizeID Where S1.Order_No = '" + OrderNo + "' And S1.color = '" + Sample + "' and S1.Size = '" + Size + "'", ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    Bom = Convert.ToInt32(Tdt.Rows[0]["Bom"].ToString());
                    Bal = Bom;
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Dt.Rows[i]["Order_No"].ToString() == OrderNo && Dt.Rows[i]["Sample"].ToString() == Sample && Dt.Rows[i]["Size"].ToString() == Size)
                        {
                            Prod = Convert.ToInt64(Prod) + Convert.ToInt64(Dt.Rows[i]["Production"]);
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

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    MyParent.Save_Error = true;
                    DtpDate1.Focus();
                    return;
                }

                for (int i = 0; i <= Grid.Rows.Count - 2; i++)
                {
                    if (MyParent.Edit == true && Grid["Record", i].Value.ToString() != "O")
                    {
                        for (int j = 0; j < Grid.Columns.Count - 11; j++)
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
                    else
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
                }


                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Production", i].Value == DBNull.Value || Grid["Production", i].Value.ToString() == String.Empty || Convert.ToInt64(Grid["Production", i].Value) == 0)
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
                        MessageBox.Show("Production Value Invalid  in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Production", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }

                    if (MyParent.Edit == true)
                    {
                        if (Grid["Changed", i].Value.ToString() == "Y")
                        {
                            if (Fill_BOM_Check_On_Edit(Grid["Order_No", i].Value.ToString(), Grid["Sample", i].Value.ToString(), Grid["Size", i].Value.ToString()) < 0)
                            {
                                MessageBox.Show("Production Value Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                Grid.CurrentCell = Grid["Production", i];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }
                    }

                }

                Dt.AcceptChanges();
                Array_Index = 0;
                Queries = new String[(Dt.Rows.Count * 2) + 5];

                TxtNo.Text = MyBase.MaxOnlyComp("Floor_Knitting_Master", "EntryNo", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Floor_Knitting_master (EntryNo, EntryDate, ShiftCode, Timing, Company_Code, EntryTime, EntrySystem, Remarks) Values (" + TxtNo.Text + ",'" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "','" + TxtShift.Tag.ToString() + "','" + TxtTiming.Text.ToString() + "'," + MyParent.CompCode + ",getdate(),Host_name(), '" + TxtRemarks.Text + "') ; Select Scope_Identity()";
                }
                else
                {
                    Queries[Array_Index++] = "Update Floor_Knitting_Master Set ShiftCode = " + TxtShift.Tag.ToString() + ", Remarks = '" + TxtRemarks.Text + "' where RowID = " + Code;
                    //Queries[Array_Index++] = "Update F1 Set F1.KnitQty = F1.KnitQty - Isnull(F2.Production, 0) From Floor_Stock F1 Left join Floor_Knitting_DEtails F2 on F1.Order_No = F2.Order_No and F1.OrderColorID = F2.OrderColorID and F1.SizeID = F2.SizeID Where F2.MasterID = " + Code;
                    //Queries[Array_Index++] = "Update F1 Set F1.KnitQty = F1.KnitQty - Isnull(F2.Production, 0) From Floor_Stock F1 Inner join (Select Order_No, OrderColorID, SizeID, Sum(Production) Production From Floor_Knitting_DEtails Where MasterID = " + Code + " Group By Order_No, OrderColorID, SizeID) F2 on F1.Order_No = F2.Order_No and F1.OrderColorID = F2.OrderColorID and F1.SizeID = F2.SizeID ";
                    //Queries[Array_Index++] = "Delete From Floor_Knitting_Details where MAsterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New == true)
                    {
                        Queries[Array_Index++] = "Insert Into Floor_Knitting_Details (MasterID, MachineID, Order_No, OrderColorID, NeedleID, SizeID, OrderQty, ItemID, BOMQty, production, seconds, Waste_Weight, Emplno_Operator, Emplno_Technician, Emplno_Supervisor) Values (@@IDENTITY, '" + Grid["Machine", i].Value.ToString() + "', '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", '" + Grid["NeedleID", i].Value.ToString() + "', '" + Grid["SizeID", i].Value.ToString() + "', " + Grid["OrderQty", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["Bom", i].Value + ", " + Grid["Production", i].Value + "," + Grid["Seconds", i].Value + "," + Grid["Waste", i].Value + "," + Grid["Emplno_Operator", i].Value + "," + Grid["Emplno_Technician", i].Value + "," + Grid["Emplno_Supervisor", i].Value + ")";
                    }
                    else
                    {
                        //Queries[Array_Index++] = "Insert Into Floor_Knitting_Details (MasterID, MachineID, Order_No, OrderColorID, NeedleID, SizeID, OrderQty, ItemID, BOMQty, production, seconds, Waste_Weight, Emplno_Operator, Emplno_Technician, Emplno_Supervisor) Values (" + Code + ", '" + Grid["Machine", i].Value.ToString() + "', '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", '" + Grid["NeedleID", i].Value.ToString() + "', '" + Grid["SizeID", i].Value.ToString() + "', " + Grid["OrderQty", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["Bom", i].Value + ", " + Grid["Production", i].Value + "," + Grid["Seconds", i].Value + "," + Grid["Waste", i].Value + "," + Grid["Emplno_Operator", i].Value + "," + Grid["Emplno_Technician", i].Value + "," + Grid["Emplno_Supervisor", i].Value + ")";
                        if (Grid["Changed", i].Value.ToString() == "Y" && Grid["Record", i].Value.ToString() == "O")
                        {
                            Queries[Array_Index++] = " Update F1 Set F1.KnitQty = F1.KnitQty + (" + Grid["Diff_Prod", i].Value + ") From Floor_Stock F1 Inner join (Select Order_No, OrderColorID, SizeID, Production From Floor_Knitting_DEtails Where RowID = " + Grid["RowNumber", i].Value + " ) F2 on F1.Order_No = F2.Order_No and F1.OrderColorID = F2.OrderColorID and F1.SizeID = F2.SizeID ";
                            Queries[Array_Index++] = " Update Floor_Knitting_Details Set Production = " + Grid["Production", i].Value + ", Seconds = " + Grid["Seconds", i].Value + ", Waste_Weight = " + Grid["Waste", i].Value + " Where RowID = " + Grid["RowNumber", i].Value + "";
                        }
                        else if ((Grid["Changed", i].Value.ToString() == "Y" && Grid["Record", i].Value.ToString() == "N") || (Grid["Changed", i].Value.ToString() == "N" && Grid["Record", i].Value.ToString() == "N"))
                        {
                            Queries[Array_Index++] = "Insert Into Floor_Knitting_Details (MasterID, MachineID, Order_No, OrderColorID, NeedleID, SizeID, OrderQty, ItemID, BOMQty, production, seconds, Waste_Weight, Emplno_Operator, Emplno_Technician, Emplno_Supervisor) Values (" + Code + ", '" + Grid["Machine", i].Value.ToString() + "', '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", '" + Grid["NeedleID", i].Value.ToString() + "', '" + Grid["SizeID", i].Value.ToString() + "', " + Grid["OrderQty", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["Bom", i].Value + ", " + Grid["Production", i].Value + "," + Grid["Seconds", i].Value + "," + Grid["Waste", i].Value + "," + Grid["Emplno_Operator", i].Value + "," + Grid["Emplno_Technician", i].Value + "," + Grid["Emplno_Supervisor", i].Value + ")";
                            Queries[Array_Index++] = "Exec Floor_Stock_Knitting_Fill '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", '" + Grid["SizeID", i].Value.ToString() + "', '" + Grid["Production", i].Value.ToString() + "', " + Grid["ItemID", i].Value.ToString() + ", " + Grid["Bom", i].Value.ToString() + ", " + Grid["OrderQty", i].Value.ToString();
                        }

                    }
                    if (MyParent._New == true)
                    {
                        Queries[Array_Index++] = "Exec Floor_Stock_Knitting_Fill '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", '" + Grid["SizeID", i].Value.ToString() + "', '" + Grid["Production", i].Value.ToString() + "', " + Grid["ItemID", i].Value.ToString() + ", " + Grid["Bom", i].Value.ToString() + ", " + Grid["OrderQty", i].Value.ToString();
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Knitting - Delete", "Select F1.EntryNo, F1.ENtryDate, S1.Shiftcode2 Shift, F1.Timing, F1.ShiftCode, F1.Remarks, F1.RowID From Floor_Knitting_Master F1 Left Join Socks_Shift () S1 on F1.ShiftCode = S1.Shiftcode", String.Empty, 80, 90, 70);
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
                    MyBase.Run("Update F1 Set F1.KnitQty = F1.KnitQty - Isnull(F2.Production, 0) From Floor_Stock F1 Inner join (Select Order_No, OrderColorID, SizeID, Sum(Production) Production From Floor_Knitting_DEtails Where MasterID = " + Code + " Group By Order_No, OrderColorID, SizeID) F2 on F1.Order_No = F2.Order_No and F1.OrderColorID = F2.OrderColorID and F1.SizeID = F2.SizeID", "Delete From Floor_Knitting_Details where MAsterID = " + Code, "Delete From Floor_Knitting_Master where RowID = " + Code);
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Knitting - View", "Select F1.EntryNo, F1.ENtryDate, S1.Shiftcode2 Shift, F1.Timing, F1.ShiftCode, F1.Remarks, F1.RowID From Floor_Knitting_Master F1 Left Join Socks_Shift () S1 on F1.ShiftCode = S1.Shiftcode", String.Empty, 80, 90, 70);
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
            DataTable Tdt = new DataTable();
            int month = DtpDate1.Value.Month;
            int day = DtpDate1.Value.Day;
            int year = DtpDate1.Value.Year;
            try
            {
                if (Buffer)
                {
                    Str = "Select 0 as Slno, F1.MachineID Machine, F1.NeedleID, F1.NeedleID Needle, F1.Order_No, F1.OrderQty, F1.ItemID, C1.color Sample, F1.OrderColorID, F1.SizeID, S1.Size, F1.BOMQty Bom, Cast(0 as Bigint) Bal_Knit, F1.Production, F1.Seconds, F1.Waste_Weight Waste, F1.Emplno_OPerator, E1.Name OPerator, F1.Emplno_Technician, E2.Name Technician, F1.Emplno_Supervisor, E3.Name Supervisor, '-' T From " + Buffer_Table + " F1 Left Join VFit_Sample_Needle_Master V2 On F1.NeedleID = V2.RowID Left Join size S1 on F1.SizeID = S1.sizeid Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E2 on F1.Emplno_Technician = E2.Emplno Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E3 on F1.Emplno_Supervisor = E3.Emplno Left Join Socks_Bom() C1 On F1.OrderColorID = C1.OrderColorId And F1.Order_No = C1.Order_No Order By F1.Slno";
                }
                else
                {
                    if (MyParent._New)
                    {
                        Str = "Select 0 as Slno, F1.MachineID Machine, F1.NeedleID, F1.NeedleID Needle, F1.Order_No, F1.OrderQty, F1.ItemID, Cast('' As Varchar (15)) Sample, F1.OrderColorID, F1.SizeID, S1.Size, F1.BOMQty Bom, Cast(0 as Bigint) Bal_Knit, F1.Production, F1.Seconds, F1.Waste_Weight Waste, F1.Emplno_OPerator, E1.Name OPerator, F1.Emplno_Technician, E2.Name Technician, F1.Emplno_Supervisor, E3.Name Supervisor, '-' T From Floor_Knitting_Details F1 Left Join VFit_Sample_Needle_Master V2 On F1.NeedleID = V2.RowID Left Join size S1 on F1.SizeID = S1.sizeid Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E2 on F1.Emplno_Technician = E2.Emplno Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E3 on F1.Emplno_Supervisor = E3.Emplno Where 1 = 2 ";
                    }
                    else if (MyParent.Edit)
                    {
                        Str = "Select 0 as Slno, F1.MachineID Machine, F1.NeedleID, F1.NeedleID Needle, F1.Order_No, F1.OrderQty, F1.ItemID, C1.color Sample, F1.OrderColorID, F1.SizeID, S1.Size, F1.BOMQty Bom, Cast(0 as Bigint) Bal_Knit, F1.Production, Isnull(F1.Seconds, 0)Seconds, Isnull(F1.Waste_Weight, 0) Waste, F1.Emplno_OPerator, E1.Name OPerator, ISnull(F1.Emplno_Technician,0)Emplno_Technician, Isnull(E2.Name,'-') Technician, Isnull(F1.Emplno_Supervisor,0) Emplno_Supervisor, ISnull(E3.Name,'-') Supervisor, ";
                        Str = Str + " F2.LinkQty Linked, F1.Production Production_Old, Isnull(F1.Seconds,0) Seconds_Old, Isnull(F1.Waste_Weight,0) Waste_Old, Cast(0 As Numeric(10, 2)) Diff_Prod, Cast(0 As Numeric(10, 2)) Diff_Sec, Cast(0 As Numeric(10, 2)) Diff_Was, Cast('N' As Varchar) Changed, Cast('O' As Varchar) Record, F1.RowID RowNumber, '-' T From Floor_Knitting_Details F1 Left Join VFit_Sample_Needle_Master V2 On F1.NeedleID = V2.RowID Left Join Socks_Bom() C1 On F1.OrderColorID = C1.OrderColorId And F1.Order_No = C1.Order_No ";
                        Str = Str + " Left Join size S1 on F1.SizeID = S1.sizeid Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E2 on F1.Emplno_Technician = E2.Emplno Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E3 on F1.Emplno_Supervisor = E3.Emplno ";
                        Str = Str + " Left Join Floor_Stock F2 On F1.Order_No = F2.Order_No And F1.OrderColorID = F2.OrderColorID And F1.SizeID = F2.SizeID  Where F1.MasterID = " + Code + " Order By F1.RowID";
                    }
                    else
                    {
                        Str = "Select 0 as Slno, F1.MachineID Machine, F1.NeedleID, F1.NeedleID Needle, F1.Order_No, F1.OrderQty, F1.ItemID, C1.color Sample, F1.OrderColorID, F1.SizeID, S1.Size, F1.BOMQty Bom, Cast(0 as Bigint) Bal_Knit, F1.Production, F1.Seconds, F1.Waste_Weight Waste, F1.Emplno_OPerator, E1.Name OPerator, F1.Emplno_Technician, E2.Name Technician, F1.Emplno_Supervisor, E3.Name Supervisor, '-' T From Floor_Knitting_Details F1 Left Join VFit_Sample_Needle_Master V2 On F1.NeedleID = V2.RowID Left Join Socks_Bom() C1 On F1.OrderColorID = C1.OrderColorId And F1.Order_No = C1.Order_No Left Join size S1 on F1.SizeID = S1.sizeid Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E1 on F1.Emplno_Operator = E1.Emplno Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E2 on F1.Emplno_Technician = E2.Emplno Left Join Vaahini_ERP_Gainup.Dbo.Employeemas E3 on F1.Emplno_Supervisor = E3.Emplno Where F1.MasterID = " + Code + " Order By F1.RowID "; 
                    }
                }

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);

                if (MyParent.Edit)
                {
                    MyBase.Grid_Designing(ref Grid, ref Dt, "OrderColorID", "NeedleID", "SizeID", "OrderQty", "ItemID", "Bal_Knit", "Emplno_operator", "Emplno_Technician", "Emplno_Supervisor", "Linked", "Production_Old", "Seconds_Old", "Waste_Old", "Diff_Prod", "Diff_Sec", "Diff_Was", "Changed", "Record", "RowNumber", "T");
                }
                else
                {
                    MyBase.Grid_Designing(ref Grid, ref Dt, "OrderColorID", "NeedleID", "SizeID", "OrderQty", "ItemID", "Bal_Knit", "Emplno_operator", "Emplno_Technician", "Emplno_Supervisor", "T");
                }
                
                if (DtpDate1.Value  <= Convert.ToDateTime("01/JUL/2015".ToString()))
                {
                    MyBase.ReadOnly_Grid_Without(ref Grid, "Machine", "Order_No", "Production", "Seconds", "Waste", "Operator", "Technician", "Supervisor");
                }
                else 
                {
                    MyBase.ReadOnly_Grid_Without(ref Grid, "Machine", "Order_No", "Production", "Seconds", "Waste" );
                }
                MyBase.Grid_Width(ref Grid, 50, 70, 70, 120, 90, 70, 100, 100, 100, 80, 150, 150, 150);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                Grid.RowHeadersWidth = 10;

                Grid.Columns["Production"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Production"].DefaultCellStyle.Format = "0";

                Grid.Columns["Seconds"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Seconds"].DefaultCellStyle.Format = "0";

                Grid.Columns["Waste"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Waste"].DefaultCellStyle.Format = "0.000";

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

        private void FrmFloorKnitting_KeyDown(object sender, KeyEventArgs e)
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

        private void FrmFloorKnitting_KeyPress(object sender, KeyPressEventArgs e)
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
                    Txt.LostFocus += new EventHandler(Txt_LostFocus);
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
                if (MyParent.Edit)
                {
                    if (Grid["Record", Grid.CurrentCell.RowIndex].Value.ToString()  == "O")
                    {
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Production"].Index)
                        {
                            if (Convert.ToDouble(Grid["Production_Old", Grid.CurrentCell.RowIndex].Value.ToString()) != Convert.ToDouble(Grid["Production", Grid.CurrentCell.RowIndex].Value.ToString()))
                            {
                                Grid["Diff_Prod", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["Production", Grid.CurrentCell.RowIndex].Value.ToString()) - Convert.ToDouble(Grid["Production_Old", Grid.CurrentCell.RowIndex].Value.ToString());
                                Grid["Changed", Grid.CurrentCell.RowIndex].Value = "Y";

                                
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Seconds"].Index)
                        {
                            if (Convert.ToDouble(Grid["Seconds", Grid.CurrentCell.RowIndex].Value.ToString()) != Convert.ToDouble(Grid["Seconds_Old", Grid.CurrentCell.RowIndex].Value.ToString()))
                            {
                                Grid["Diff_Sec", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["Seconds", Grid.CurrentCell.RowIndex].Value.ToString()) - Convert.ToDouble(Grid["Seconds_Old", Grid.CurrentCell.RowIndex].Value.ToString());
                                Grid["Changed", Grid.CurrentCell.RowIndex].Value = "Y";
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Waste"].Index)
                        {
                            if (Convert.ToDouble(Grid["Waste", Grid.CurrentCell.RowIndex].Value.ToString()) != Convert.ToDouble(Grid["Waste_Old", Grid.CurrentCell.RowIndex].Value.ToString()))
                            {
                                Grid["Diff_Was", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["Waste", Grid.CurrentCell.RowIndex].Value.ToString()) - Convert.ToDouble(Grid["Waste_Old", Grid.CurrentCell.RowIndex].Value.ToString());
                                Grid["Changed", Grid.CurrentCell.RowIndex].Value = "Y";
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

        void Txt_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                {
                    Total_Prod_Qty();
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Technician"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        Grid["Emplno_Technician", Grid.CurrentCell.RowIndex].Value = Grid["Emplno_Technician", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Technician", Grid.CurrentCell.RowIndex].Value = Grid["Technician", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Txt.Text = Grid["Technician", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                    }
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
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Supervisor"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        Grid["Emplno_Supervisor", Grid.CurrentCell.RowIndex].Value = Grid["Emplno_Supervisor", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["Supervisor", Grid.CurrentCell.RowIndex].Value = Grid["Supervisor", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Txt.Text = Grid["Supervisor", Grid.CurrentCell.RowIndex - 1].Value.ToString();
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
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
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
                        Grid["Bal_Knit", Grid.CurrentCell.RowIndex].Value = Grid["Bal_Knit", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Grid["OrderColorID", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        Fill_BOM(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString());
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


        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Production"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Seconds"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Waste"].Index)
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

        void Total_Prod_Qty()
        {
            try
            {
                TxtTotal.Text = String.Format ("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "Production", "Order_No", "Sample", "Operator")));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Machine_Selection()
        {
            DataTable Tdt = new DataTable();
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Machine", "Select Machine, Needle From Knitting_Mc_NO_New(Year('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "'),Datepart(Week,'" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "') )", String.Empty, 150, 150);
                if (Dr != null)
                {
                    Grid["Machine", Grid.CurrentCell.RowIndex].Value = Dr["Machine"].ToString();
                    Grid["NeedleID", Grid.CurrentCell.RowIndex].Value = Dr["Needle"].ToString();
                    Grid["Needle", Grid.CurrentCell.RowIndex].Value = Dr["Needle"].ToString();

                    if (DtpDate1.Value >= Convert.ToDateTime("01-JUL-2015".ToString()))
                    {
                        MyBase.Load_Data("select P2.EmplNo, E1.Name Operator, P2.Technician_Emplno Emplno_Technician, E2.Name Technician, P2.Supervisor_Emplno Emplno_Supervisor, E3.Name Supervisor From Employee_Production_Master_Socks P1 Left Join Employee_Production_Details_Socks P2 On P1.RowID = P2.Master_ID Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E1 On P2.EmplNo = E1.Emplno Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E2 On P2.Technician_Emplno = E2.Emplno Left Join VAAHINI_ERP_GAINUP.dbo.Employeemas E3 On P2.Supervisor_Emplno = E3.Emplno where P2.Machine_Name = '" + Dr["Machine"].ToString() + "' And P1.Shift_Code = " + TxtShift.Tag.ToString() + " and Entry_Date = (select MAX(Entry_Date) from Employee_Production_Master_Socks P1 Left join Employee_Production_Details_Socks P2 On P1.RowID = P2.Master_ID where P2.Machine_Name = '" + Dr["Machine"].ToString() + "' And P1.Shift_Code = " + TxtShift.Tag.ToString() + ")", ref Tdt);
                        if (Tdt.Rows.Count > 0)
                        {
                            Grid["Operator", Grid.CurrentCell.RowIndex].Value = Tdt.Rows[0]["Operator"].ToString();
                            Grid["Emplno_Operator", Grid.CurrentCell.RowIndex].Value = Tdt.Rows[0]["Emplno"].ToString();
                            Grid["Technician", Grid.CurrentCell.RowIndex].Value = Tdt.Rows[0]["Technician"].ToString();
                            Grid["Emplno_Technician", Grid.CurrentCell.RowIndex].Value = Tdt.Rows[0]["Emplno_Technician"].ToString();
                            Grid["Supervisor", Grid.CurrentCell.RowIndex].Value = Tdt.Rows[0]["Supervisor"].ToString();
                            Grid["Emplno_Supervisor", Grid.CurrentCell.RowIndex].Value = Tdt.Rows[0]["Emplno_Supervisor"].ToString();
                        }
                    }
                    if (MyParent.Edit)
                    {
                        Grid["Record", Grid.CurrentCell.RowIndex].Value = "N";
                        Grid["Changed", Grid.CurrentCell.RowIndex].Value = "N"; 
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
                    if (MyParent.Edit)
                    {
                        Grid["Record", Grid.CurrentCell.RowIndex].Value = "N";
                        Grid["Changed", Grid.CurrentCell.RowIndex].Value = "N";
                    }
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
                String Str;
                Str = "Select S1.Order_No, S1.Color Sample, S1.SizeID, S1.Size, S1.Bom_Qty Bom, ISNULL(CAST(S1.AllowancePer as Varchar),'NOT AVAILABLE') Allowance, (S1.Bom_Qty - Isnull(K1.Knitted, 0)) Balance_knitting, S1.Order_Qty, S1.ItemID, S1.OrderColorID From Socks_Bom () S1 Left Join Knitting_Production_All () k1 on S1.Order_No = K1.OrderNo and S1.OrderColorId = K1.OrderColorID and S1.sizeid = K1.SizeID Left Join Fit_Order_Status F1 On S1.Order_No = F1.Order_No Where F1.Order_No is null Order By S1.Order_No";
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Order", Str, String.Empty, 120, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    if (Dr["Bom"].ToString() == String.Empty || Dr["Bom"].ToString() == null)
                    {
                        MessageBox.Show("Allowance Percentage Not Available For "+ Dr["Order_No"].ToString() +" .....! SOCKS");
                        Grid.CurrentCell = Grid["Order_No", Grid.CurrentCell.RowIndex];
                        Grid["Order_No", Grid.CurrentCell.RowIndex].Value = "";
                        Txt.Text = "";
                        Grid["Sample", Grid.CurrentCell.RowIndex].Value = "";
                        Grid["SizeID", Grid.CurrentCell.RowIndex].Value = 0;
                        Grid["Size", Grid.CurrentCell.RowIndex].Value = "";
                        Grid["Bom", Grid.CurrentCell.RowIndex].Value = 0;
                        Grid["OrderQty", Grid.CurrentCell.RowIndex].Value = 0;
                        Grid["ItemID", Grid.CurrentCell.RowIndex].Value = 0;
                        Grid["Bal_Knit", Grid.CurrentCell.RowIndex].Value = 0;
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
                        Grid["Bal_Knit", Grid.CurrentCell.RowIndex].Value = Dr["Balance_Knitting"].ToString();
                        Grid["OrderColorID", Grid.CurrentCell.RowIndex].Value = Dr["OrderColorID"].ToString();
                        Fill_BOM(Dr["Order_No"].ToString(), Dr["Sample"].ToString(), Dr["Size"].ToString());
                    }
                    if (MyParent.Edit)
                    {
                        Grid["Record", Grid.CurrentCell.RowIndex].Value = "N";
                        Grid["Changed", Grid.CurrentCell.RowIndex].Value = "N";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Tech_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Technician", "Select Name, Tno, Emplno From Socks_Technician_Present_Detail ('" + String.Format ("{0:dd-MMM-yyyy}", DtpDate1.Value) + "') Where Tno Not Like '%Z'", String.Empty, 250, 80);
                if (Dr != null)
                {
                    Grid["Technician", Grid.CurrentCell.RowIndex].Value = Dr["Name"].ToString();
                    Txt.Text = Dr["Name"].ToString();
                    Grid["EmplNo_Technician", Grid.CurrentCell.RowIndex].Value = Dr["Emplno"].ToString();
                    if (MyParent.Edit)
                    {
                        Grid["Record", Grid.CurrentCell.RowIndex].Value = "N";
                        Grid["Changed", Grid.CurrentCell.RowIndex].Value = "N";
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
                    if (MyParent.Edit)
                    {
                        Grid["Record", Grid.CurrentCell.RowIndex].Value = "N";
                        Grid["Changed", Grid.CurrentCell.RowIndex].Value = "N";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Supervisor_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Supervisor", "Select Name, Tno, Emplno From Socks_Supervisor_Present_Detail ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "') Where DeptName like 'Knitting%' and Tno Not Like '%Z'", String.Empty, 250, 80);
                if (Dr != null)
                {
                    Grid["Supervisor", Grid.CurrentCell.RowIndex].Value = Dr["Name"].ToString();
                    Txt.Text = Dr["Name"].ToString();
                    Grid["EmplNo_Supervisor", Grid.CurrentCell.RowIndex].Value = Dr["Emplno"].ToString();
                    if (MyParent.Edit)
                    {
                        Grid["Record", Grid.CurrentCell.RowIndex].Value = "N";
                        Grid["Changed", Grid.CurrentCell.RowIndex].Value = "N";
                    }
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
                        if (MyParent._New == true)
                        {
                            Machine_Selection();
                        }
                        else
                        {
                            if (Convert.ToString(Grid["Record", Grid.CurrentCell.RowIndex].Value.ToString()) != "O" || Grid["Record", Grid.CurrentCell.RowIndex].Value == String.Empty)
                            {
                                Machine_Selection();
                            }
                            else
                            {
                                MessageBox.Show("Cann't Change Machine For Old Record.....Gainup");
                            }
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Needle"].Index)
                    {
                        if (MyParent._New == true)
                        {
                            Needle_Selection();
                        }
                        else
                        {
                            if (Convert.ToString(Grid["Record", Grid.CurrentCell.RowIndex].Value.ToString()) != "O" || Grid["Record", Grid.CurrentCell.RowIndex].Value == String.Empty)
                            {
                                Needle_Selection();
                            }
                            else
                            {
                                MessageBox.Show("Cann't Change Needle For Old Record.....Gainup");
                            }
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                    {
                        if (MyParent._New == true)
                        {
                            OrderNo_Selection();
                        }
                        else
                        {
                            if (Convert.ToString(Grid["Record", Grid.CurrentCell.RowIndex].Value.ToString()) != "O" || Grid["Record", Grid.CurrentCell.RowIndex].Value == String.Empty)
                            {
                                OrderNo_Selection();
                            }
                            else
                            {
                                MessageBox.Show("Cann't Change Needle For Old Record.....Gainup");
                            }
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Operator"].Index)
                    {
                        if (MyParent._New == true)
                        {
                            Operator_Selection();
                        }
                        else
                        {
                            if (Convert.ToString(Grid["Record", Grid.CurrentCell.RowIndex].Value.ToString()) != "O" || Grid["Record", Grid.CurrentCell.RowIndex].Value == String.Empty)
                            {
                                Operator_Selection();
                            }
                            else
                            {
                                MessageBox.Show("Cann't Change Operator For Old Record.....Gainup");
                            }
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Technician"].Index)
                    {
                        if (MyParent._New == true)
                        {
                            Tech_Selection();
                        }
                        else
                        {
                            if (Convert.ToString(Grid["Record", Grid.CurrentCell.RowIndex].Value.ToString()) != "O" || Grid["Record", Grid.CurrentCell.RowIndex].Value == String.Empty)
                            {
                                Tech_Selection();
                            }
                            else
                            {
                                MessageBox.Show("Cann't Change Technician For Old Record.....Gainup");
                            }
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Supervisor"].Index)
                    {
                        if (MyParent._New == true)
                        {
                            Supervisor_Selection();
                        }
                        else
                        {
                            if (Convert.ToString(Grid["Record", Grid.CurrentCell.RowIndex].Value.ToString()) != "O" || Grid["Record", Grid.CurrentCell.RowIndex].Value == String.Empty)
                            {
                                Supervisor_Selection();
                            }
                            else
                            {
                                MessageBox.Show("Cann't Change Supervisor For Old Record.....Gainup");
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

                        if (MyParent.Edit)
                        {
                            if (Grid["Record", Grid.CurrentCell.RowIndex].Value.ToString() == "O")
                            {
                                if (Convert.ToDouble(Grid["Production_Old", Grid.CurrentCell.RowIndex].Value.ToString()) != Convert.ToDouble(Grid["Production", Grid.CurrentCell.RowIndex].Value.ToString()))
                                {
                                    if (Fill_BOM_Check_On_Edit(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString()) < 0)
                                    {
                                        //MessageBox.Show("Production Value Invalid  in Row " + (Grid.CurrentCell.RowIndex + 1) + "  ", "Gainup");
                                        Grid["Production", Grid.CurrentCell.RowIndex].Value = Grid["Production_Old", Grid.CurrentCell.RowIndex].Value;
                                        Grid["Changed", Grid.CurrentCell.RowIndex].Value = 0;
                                        Grid.CurrentCell = Grid["Production", Grid.CurrentCell.RowIndex];
                                        Grid.Focus();
                                        Grid.BeginEdit(true);
                                        return;
                                    }
                                }
                            }
 
                        }

                        if (Convert.ToDouble(Grid["Production", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(LblBal.Text.Replace ("BAL:", "")))
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
                            Fill_BOM(Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Sample", Grid.CurrentCell.RowIndex].Value.ToString(), Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString());
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Seconds"].Index)
                    {
                        if (Grid["Seconds", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Seconds", Grid.CurrentCell.RowIndex].Value) <= 0)
                        {
                            Grid["Seconds", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Waste"].Index)
                    {
                        if (Grid["Waste", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Waste", Grid.CurrentCell.RowIndex].Value) <= 0)
                        {
                            Grid["Waste", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                    }
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

        void Fill_BOM(String OrderNo, String Sample, String Size)
        {
            try
            {
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select S1.Bom_Qty Bom, Isnull(K1.Knitted, 0) Knitted, (S1.Bom_Qty - Isnull(K1.Knitted, 0)) Balance_knitting From Socks_Bom () S1 Left Join Knitting_Production_All () k1 on S1.Order_No = K1.OrderNo and S1.OrderColorId = K1.OrderColorID and S1.sizeid = K1.SizeID Where S1.Order_No = '" + OrderNo + "' And S1.color = '" + Sample + "' and S1.Size = '" + Size + "'", ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    LblBOM.Text = "BOM: " + Tdt.Rows[0]["Bom"].ToString();
                    LblPre_Prod.Text = "PROD: " + Tdt.Rows[0]["Knitted"].ToString();
                    LblBal.Text = "BAL: " + Tdt.Rows[0]["Balance_Knitting"].ToString();

                    if (Grid["Production", Grid.CurrentCell.RowIndex].Value == null || Grid["Production", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Production", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["Production", Grid.CurrentCell.RowIndex].Value = "0";
                    }
                    
                    LblProduction.Text = "0";

                    for (int i = 0; i <= Dt.Rows.Count -1; i++)
                    {
                        if (Grid.CurrentCell.RowIndex != i)
                        {
                            if (Dt.Rows[i]["Order_No"].ToString() == OrderNo && Dt.Rows[i]["Sample"].ToString() == Sample && Dt.Rows[i]["Size"].ToString() == Size)
                            {
                                LblProduction.Text = String.Format("{0:0}", Convert.ToDouble(LblProduction.Text) + Convert.ToDouble(Dt.Rows[i]["Production"]));
                            }
                        }
                    }

                    LblBal.Text = "BAL: " + String.Format("{0:0}", Convert.ToDouble(LblBal.Text.Replace("BAL: ", "")) - Convert.ToDouble(LblProduction.Text));
                }

                if (!MyParent._New)
                {
                    Tdt = new DataTable();
                    MyBase.Load_Data("Select Isnull(Sum(Production), 0) Production From Floor_Knitting_Details Where Order_No = '" + OrderNo + "' And OrderColorID = .Dbo.Get_OrdercolorID ('" + OrderNo + "', '" + Sample + "') and SizeID = Dbo.Get_OrderSizeID ('" + OrderNo + "', '" + Size + "') and MasterID = " + Code, ref Tdt);
                    LblBal.Text = String.Format ("{0:0}", Convert.ToDouble(LblBal.Text.Replace("BAL: ", "")) + Convert.ToDouble(Tdt.Rows[0][0]));
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
                if (Grid["Record", Grid.CurrentCell.RowIndex].Value.ToString() == "O")
                {
                    MessageBox.Show("Cann't Delete Old Records....Gainup"); 
                }
                else
                {
                    MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentCell.RowIndex);
                    Total_Prod_Qty();
                    MyBase.Row_Number(ref Grid);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("does not have a value"))
                {

                }
                else if (ex.Message.Contains("There is no row"))
                {

                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
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
                if (ex.Message.Contains("This row has been removed"))
                {

                }
                else 
                {
                    MessageBox.Show(ex.Message);
                }
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
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Technician"].Index)
                {
                    if (Grid.CurrentCell.RowIndex == 0)
                    {
                        if (Grid["Technician", Grid.CurrentCell.RowIndex].Value == null || Grid["Technician", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Technician", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Tech_Selection();
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Supervisor"].Index)
                {
                    if (Grid.CurrentCell.RowIndex == 0)
                    {
                        if (Grid["Supervisor", Grid.CurrentCell.RowIndex].Value == null || Grid["Supervisor", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Supervisor", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Supervisor_Selection();
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
                        MyBase.Execute("Select Cast(0 as int) Slno, MachineID, Order_No, OrderColorID, NeedleID, SizeID, BomQty, Production, Seconds, Waste_Weight, Emplno_Operator, Emplno_Technician, Emplno_Supervisor, OrderQty, ItemID into " + Buffer_Table + " From Floor_Knitting_Details Where 1 = 2");
                    }

                    MyBase.Execute("Delete From " + Buffer_Table);

                    if (Dt.Rows.Count > 2)
                    {
                        for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                        {
                            Queries[Array_Index++] = " Insert Into " + Buffer_Table + " (Slno, MachineID, Order_No, OrderColorID, NeedleID, SizeID, OrderQty, ItemID, BOMQty, production, seconds, Waste_Weight, Emplno_Operator, Emplno_Technician, Emplno_Supervisor) Values (" + Grid["Slno", i].Value.ToString() + ", '" + Grid["Machine", i].Value.ToString() + "', '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["OrderColorID", i].Value.ToString() + ", '" + Grid["NeedleID", i].Value.ToString() + "', '" + Grid["SizeID", i].Value.ToString() + "', " + Grid["OrderQty", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["Bom", i].Value + ", " + Grid["Production", i].Value + "," + Grid["Seconds", i].Value + "," + Grid["Waste", i].Value + "," + Grid["Emplno_Operator", i].Value + "," + Grid["Emplno_Technician", i].Value + "," + Grid["Emplno_Supervisor", i].Value + ")";
                        }
                    }
                    if (Dt.Rows.Count > 2)
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