using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounts_ControlModules;
using SelectionTool_NmSp;

namespace Accounts
{
    public partial class Fabric_DC_Outward : Form, Entry
    {
        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        DataTable Dt = new DataTable();
        DataRow Dr;
        SelectionTool_Class Tool = new SelectionTool_Class();
        TextBox Txt = null;
        Double Code = 0;
        Int16 Max_ProcessCode = 0;

        public Fabric_DC_Outward()
        {
            InitializeComponent();
        }

        private void Fabric_DC_Outward_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                MyBase.Clear(this);
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
                Grid_Data();
                Max_ProcessCode = 0;
                Load_Process();
                MyBase.CmbSelection(ref ChkProcessBox, false);
                ChkProcessBox.Focus();
                ChkProcessBox.SetSelected(0, true);
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
                if (Dr != null)
                {

                    Load_Process();
                    MyBase.CmbSelection(ref ChkProcessBox, false);

                    Code = 0;
                    Code = Convert.ToInt64(Dr["RowID"]);
                    TxtENo.Text = Dr["Entry_No"].ToString();
                    TxtBuyer.Text = Dr["Buyer"].ToString();
                    TxtBuyer.Tag = Dr["Buyer_Code"].ToString();
                    TxtParty.Text = Dr["Party"].ToString();
                    TxtParty.Tag = Dr["Party_Code"].ToString();
                    DtpDate.Value = Convert.ToDateTime(Dr["USer_Date"]);
                    TxtRemarks.Text = Dr["Remarks"].ToString();
                    Max_ProcessCode = Convert.ToInt16(Dr["Process_Code"]);
                    TxtColor.Text = Dr["Color"].ToString();
                    TxtColor.Tag = Dr["ColorID"].ToString();
                    TxtOrder_No.Text = Dr["Order_No"].ToString();
                    TxtLotNo.Text = Dr["LotNo"].ToString();
                    Fill_Process();
                    Grid_Data();
                    Total_Qty();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Load_Process()
        {
            DataTable TempDt = new DataTable();
            try
            {
                ChkProcessBox.Items.Clear();
                MyBase.Load_Data("Select RowID, Name From Process_Code_Master order by RowID", ref TempDt);
                for (int i = 0; i <= TempDt.Rows.Count - 1; i++)
                {
                    ChkProcessBox.Items.Add(MyBase.PadR(TempDt.Rows[i]["Name"].ToString(), 40) + " - " + MyBase.PadL(TempDt.Rows[i]["RowID"].ToString(), 10));
                }
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Fabric DC Outward - Edit", "Select distinct F1.Entry_No, F1.User_Date, F1.Order_NO, F3.BuyerName Buyer, L1.Ledger_Name Party,F1.Remarks, F1.RowID, F1.Buyer_Code, F1.Party_Code, F1.Process_Code, P1.Name Process, F1.LotNo, F1.ColorID, F8.Color From Fabric_DC_Outward_Master F1 Left Join Fabric_DC_Outward_Details F2 On F1.RowID = F2.Master_ID Left Join Process_Code_Master P1 On F1.Process_Code = P1.RowId  Left Join Fit_Buyer_Details () F3 On F1.Buyer_Code = F3.buyerid Left Join ACCOUNTS.dbo.Ledger_Master L1 On F1.Party_Code = L1.Ledger_Code and F1.Company_Code = L1.COMPANY_CODE and F1.Year_Code = L1.YEAR_CODE Left Join Fit_Color_Details () F8 On F1.ColorID = F8.ColorID Where F1.Company_Code = " + MyParent.CompCode + " and F1.Year_Code = '" + MyParent.YearCode + "'", String.Empty, 80, 90, 120, 250, 250,250);
                Fill_Datas(Dr);

                if (Dr != null)
                {
                    ChkProcessBox.SetSelected(0, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Entry_No()
        {
            String Letter = String.Empty;
            try
            {
                if (MyParent._New)
                {
                    DataTable Tdt1 = new DataTable();
                    MyBase.Load_Data("Select Isnull(Max(Cast(Substring(Entry_No, 4, 5) as Bigint)), 0) + 1 Entry_NO From Fabric_DC_Outward_Master Where Company_Code = " + MyParent.CompCode + " and Year_Code = '" + MyParent.YearCode + "'", ref Tdt1);
                    TxtENo.Text = "DC/" + String.Format("{0:00000}", Convert.ToDouble(Tdt1.Rows[0][0]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Fill_Process()
        {
            try
            {
                DataTable Tempdt = new DataTable();
                MyBase.Load_Data("Select Process_Code From Fabric_DC_Process_Details Where Master_ID = " + Code, ref Tempdt);

                for (int i = 0; i <= Tempdt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= ChkProcessBox.Items.Count - 1; j++)
                    {
                        if (Convert.ToInt16(Tempdt.Rows[i]["Process_Code"]) == Convert.ToInt16(ChkProcessBox.Items[j].ToString().Replace(ChkProcessBox.Items[j].ToString().Substring(0, 43), "")))
                        {
                            ChkProcessBox.SetItemChecked(j, true);
                        }
                    }
                }

                if (Tempdt.Rows.Count == 0)
                {
                    for (int j = 0; j <= ChkProcessBox.Items.Count - 1; j++)
                    {
                        if (Max_ProcessCode == Convert.ToInt16(ChkProcessBox.Items[j].ToString().Replace(ChkProcessBox.Items[j].ToString().Substring(0, 43), "")))
                        {
                            ChkProcessBox.SetItemChecked(j, true);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Entry_Save()
        {
            Int32 Array_Index = 0;
            String[] Queries;
            try
            {
                //if (TxtProcess.Text.Trim() == String.Empty)
                //{
                //    MessageBox.Show("Invalid Process ...!", "Gainup");
                //    MyParent.Save_Error = true;
                //    TxtProcess.Focus();
                //    return;
                //}

                Entry_No();

                if (ChkProcessBox.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Invalid Process ...!", "Gainup");
                    MyParent.Save_Error = true;
                    ChkProcessBox.Focus();
                    return;
                }

                if (TxtBuyer.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Buyer ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtBuyer.Focus();
                    return;
                }

                if (TxtParty.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Party ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtParty.Focus();
                    return;
                }

                if (TxtENo.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Entry No ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtBuyer.Focus();
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Entries to Save ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtParty.Focus();
                    return;
                }

                if (TxtOrder_No.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Order No ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtOrder_No.Focus();
                    return;
                }

                for (int i = 0; i <= ChkProcessBox.CheckedItems.Count - 1; i++)
                {
                    Max_ProcessCode = Convert.ToInt16(ChkProcessBox.CheckedItems[i].ToString().Replace(ChkProcessBox.CheckedItems[i].ToString().Substring (0, 43), ""));
                }

                if (!Check_Duplicate())
                {
                    return;
                }

                if (!Check_Grid())
                {
                    return;
                }

                Total_Qty();
                Get_Max_LotNo ();

                Queries = new String[Dt.Rows.Count + 10];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert Into Fabric_DC_Outward_Master (Entry_No, User_Date, Process_Code, Buyer_Code, Party_Code, Remarks, Company_Code, Year_Code, Order_No, LotNo, LotNo_Refer, ColorID) Values ('" + TxtENo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + Max_ProcessCode + ", " + TxtBuyer.Tag.ToString() + ", " + TxtParty.Tag.ToString() + ", '" + TxtRemarks.Text + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "', '" + TxtOrder_No.Text + "', " + TxtLotNo.Text.Trim() + ", 1, " + TxtColor.Tag.ToString() + "); Select Scope_Identity()";
                }
                else
                {
                    Queries[Array_Index++] = "Update Fabric_DC_Outward_Master Set ColorID = " + TxtColor.Tag.ToString() + ", Process_Code = " + Max_ProcessCode + ", Buyer_Code = " + TxtBuyer.Tag.ToString() + ", Party_Code = " + TxtParty.Tag.ToString() + ", Remarks = '" + TxtRemarks.Text + "' Where RowID = " + Code;
                    Queries[Array_Index++] = "Delete From Fabric_DC_Process_Details where Master_Id = " + Code;
                    Queries[Array_Index++] = "Delete From Fabric_DC_Outward_Details where Master_Id = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert Into Fabric_Dc_Outward_Details (Master_ID, SlNo, ItemID, SizeID, Body_Weight, Folding, NoRolls, LL) values (@@IDENTITY, " + Dt.Rows[i]["Slno"].ToString() + ", " + Dt.Rows[i]["ItemID"].ToString() + ", " + Dt.Rows[i]["SizeID"].ToString() + ", " + Dt.Rows[i]["Body_Weight"].ToString() + ", " + Dt.Rows[i]["Folding"].ToString() + ", " + Dt.Rows[i]["NoRolls"].ToString() + ", " + Dt.Rows[i]["LL"].ToString() + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert Into Fabric_Dc_Outward_Details (Master_ID, SlNo, ItemID, SizeID, Body_Weight, Folding, NoRolls, LL) values (" + Code + ", " + Dt.Rows[i]["Slno"].ToString() + ", " + Dt.Rows[i]["ItemID"].ToString() + ", " + Dt.Rows[i]["SizeID"].ToString() + ", " + Dt.Rows[i]["Body_Weight"].ToString() + ", " + Dt.Rows[i]["Folding"].ToString() + ", " + Dt.Rows[i]["NoRolls"].ToString() + ", " + Dt.Rows[i]["LL"].ToString() + ")";
                    }
                }

                for (int i = 0; i <= ChkProcessBox.CheckedItems.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert Into Fabric_DC_Process_Details (Master_ID, SlNo, Process_Code) Values (@@IDENTITY, " + Convert.ToInt32(i + 1) + ", " + Convert.ToInt16(ChkProcessBox.CheckedItems[i].ToString().Replace(ChkProcessBox.CheckedItems[i].ToString().Substring(0, 43), "")) + " )";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert Into Fabric_DC_Process_Details (Master_ID, SlNo, Process_Code) Values (" + Code + ", " + Convert.ToInt32(i + 1) + ", " + Convert.ToInt16(ChkProcessBox.CheckedItems[i].ToString().Replace(ChkProcessBox.CheckedItems[i].ToString().Substring(0, 43), "")) + " )";
                    }
                }

                MyBase.Run_Identity (MyParent.Edit, Queries);
                MessageBox.Show("Saved ...!", "Gainup");
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Fabric DC Outward - Delete", "Select distinct F1.Entry_No, F1.User_Date, F1.Order_NO, F3.BuyerName Buyer, L1.Ledger_Name Party,F1.Remarks, F1.RowID, F1.Buyer_Code, F1.Party_Code, F1.Process_Code, P1.Name Process, F1.LotNo, F1.ColorID, F8.Color From Fabric_DC_Outward_Master F1 Left Join Fabric_DC_Outward_Details F2 On F1.RowID = F2.Master_ID Left Join Process_Code_Master P1 On F1.Process_Code = P1.RowId  Left Join Fit_Buyer_Details () F3 On F1.Buyer_Code = F3.buyerid Left Join ACCOUNTS.dbo.Ledger_Master L1 On F1.Party_Code = L1.Ledger_Code and F1.Company_Code = L1.COMPANY_CODE and F1.Year_Code = L1.YEAR_CODE Left Join Fit_Color_Details () F8 On F1.ColorID = F8.ColorID Where F1.Company_Code = " + MyParent.CompCode + " and F1.Year_Code = '" + MyParent.YearCode + "'", String.Empty, 80, 90, 120, 250, 250,250);
                Fill_Datas(Dr);

                if (Dr != null)
                {
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
                    MyBase.Run("Delete From Fabric_DC_Process_Details Where Master_Id = " + Code, "Delete From Fabric_Dc_Outward_Details Where Master_ID = " + Code, "Delete From Fabric_DC_Outward_Master Where RowID = " + Code);
                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyBase.Clear(this);
                }
                else
                {
                    MessageBox.Show("Invalid Entry to Delete ...!", "Gainup");
                }
                MyParent.Load_DeleteEntry();
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Fabric DC Outward - View", "Select distinct F1.Entry_No, F1.User_Date, F1.Order_NO, F3.BuyerName Buyer, L1.Ledger_Name Party,F1.Remarks, F1.RowID, F1.Buyer_Code, F1.Party_Code, F1.Process_Code, P1.Name Process, F1.LotNo, F1.ColorID, F8.Color From Fabric_DC_Outward_Master F1 Left Join Fabric_DC_Outward_Details F2 On F1.RowID = F2.Master_ID Left Join Process_Code_Master P1 On F1.Process_Code = P1.RowId  Left Join Fit_Buyer_Details () F3 On F1.Buyer_Code = F3.buyerid Left Join ACCOUNTS.dbo.Ledger_Master L1 On F1.Party_Code = L1.Ledger_Code and F1.Company_Code = L1.COMPANY_CODE and F1.Year_Code = L1.YEAR_CODE Left Join Fit_Color_Details () F8 On F1.ColorID = F8.ColorID Where F1.Company_Code = " + MyParent.CompCode + " and F1.Year_Code = '" + MyParent.YearCode + "'", String.Empty, 80, 90, 120, 250, 250,250);
                Fill_Datas(Dr);
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


        void Get_Max_LotNo()
        {
            Int64 LotNo = 0;
            try
            {
                if (MyParent._New)
                {
                    DataTable Tempdt = new DataTable();
                    MyBase.Load_Data("Select (Isnull(Max(LotNo), 0) + 1) LotNo From Fabric_DC_Outward_Master Where LotNo_Refer = '1'", ref Tempdt);
                    if (Tempdt.Rows.Count > 0)
                    {
                        LotNo = Convert.ToInt64(Tempdt.Rows[0]["LotNo"]);
                    }
                    TxtLotNo.Text = LotNo.ToString();
                }
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
                    Str = "Select L1.Slno, L1.ItemID, F1.Item, cast(0 as Numeric (25, 3)) as Bom_Qty, cast(0 as Numeric (25, 3)) as Issued_Qty, L1.SizeID, F2.Size, LL, cast(0 as Numeric (25, 3)) as St_BW, cast(0 as Numeric (25, 3)) as St_FD, cast(0 as Numeric (25, 3)) as St_Rolls, L1.Body_Weight, L1.Folding, NoRolls, '' T From Fabric_DC_Outward_Details L1 Left join FIT_Item_Details () F1 on F1.ItemID = L1.itemid Left Join FIT_Size_Details () F2 On F2.sizeid = L1.SizeID Where 1= 2";
                }
                else
                {
                    Str = "Select F1.Slno, F1.ItemID, F3.Item, sum(F4.BOM_qty) BOM_qty,F7.Body_Weight + F7.Folding Issued_Qty, F1.SizeID, F5.size, F1.LL, sum(Body_Weight_Bal + F1.Body_Weight) as St_BW, sum(Folding_Bal + F1.Folding) as St_FD, sum(NoRolls_Bal + F1.NoRolls) as St_Rolls, F1.Body_Weight, F1.Folding, F1.NoRolls, '' T From Fabric_DC_Outward_Details F1 Left join Fabric_DC_Outward_Master F2 On F1.Master_ID = F2.RowID Left join FIT_Item_Details () F3 On F1.ItemID = F3.itemid Left join Fit_BOM_Details_Cutting () F4 On F2.Order_No = F4.Order_No and  F1.ItemID = F4.itemid and F2.ColorID=F4.ColorID Left join FIT_Size_Details () F5 On F1.SizeID = F5.sizeid Left join Fabric_Pending_For_Process () F6 On F2.Buyer_Code = F6.BuyerID and F1.ItemID = F6.ItemID and  F1.SizeID = F6.Sizeid and F1.LL = F6.LL Left join (Select BuyerID,Order_No,ColorID, ItemID, LL, SUM(Body_Weight) Body_Weight, Sum(Folding) Folding, Sum(NoRolls) NoRolls From Process_Sent() Group By BuyerID,Order_No,ColorID, ItemID, LL) F7 on F2.Buyer_Code = F7.BuyerID and F2.Order_No=F7.Order_No and F1.ItemID = F7.ItemID and F1.LL = F7.LL and F2.ColorID=F7.ColorID Where F1.Master_ID = " + Code + " group by F1.Slno, F1.ItemID, F3.Item,F7.Body_Weight + F7.Folding, F1.SizeID, F5.size, F1.LL, F1.Body_Weight, F1.Folding, F1.NoRolls order by F1.Slno";
                }

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "ItemID", "SizeID", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Item", "Size", "Body_Weight", "Folding", "NoRolls", "LL");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 50, 250,100, 100, 100, 60, 100, 100);

                Grid.RowHeadersWidth = 10;

                Grid.Columns["St_BW"].DefaultCellStyle.Format = "0.000";
                Grid.Columns["St_FD"].DefaultCellStyle.Format = "0.000";
                Grid.Columns["St_Rolls"].DefaultCellStyle.Format = "0";
                Grid.Columns["LL"].DefaultCellStyle.Format = "0";
                Grid.Columns["Bom_Qty"].DefaultCellStyle.Format = "0.000";

                Grid.Columns["Body_Weight"].DefaultCellStyle.Format = "0.000";
                Grid.Columns["Folding"].DefaultCellStyle.Format = "0.000";
                Grid.Columns["NoRolls"].DefaultCellStyle.Format = "0";


                Grid.Columns["St_BW"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["St_FD"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["St_Rolls"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["LL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Bom_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                Grid.Columns["Body_Weight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Folding"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["NoRolls"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                Grid.Columns["St_BW"].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                Grid.Columns["St_BW"].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                Grid.Columns["St_Fd"].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                Grid.Columns["St_Fd"].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                Grid.Columns["St_Rolls"].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                Grid.Columns["St_Rolls"].DefaultCellStyle.ForeColor = System.Drawing.Color.White;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Fabric_DC_Outward_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtColor")
                    {
                        Grid.CurrentCell = Grid["Item", 0];
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
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "Txt" || this.ActiveControl.Name == String.Empty)
                    {
                        return;
                    }

                    //if (Dt.Rows.Count > 0)
                    //{
                    //    MessageBox.Show("Please Clear All Details ...!", "Gainup");
                    //    TxtProcess.Focus();
                    //    return;
                    //}

                    if (this.ActiveControl.Name == "TxtBuyer")
                    {

                        if (TxtOrder_No.Text.Trim() != String.Empty)
                        {
                            MessageBox.Show("Already Order No Selected ...!", "Gainup");
                            TxtBuyer.Focus();
                            return;
                        }

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer", "Select BuyerName Buyer, Buyerid From Fit_Buyer_Details ()", String.Empty, 250, 80);
                        if (Dr != null)
                        {
                            TxtBuyer.Text = Dr["Buyer"].ToString();
                            TxtBuyer.Tag = Dr["BuyerID"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtColor")
                    {
                        if (Dt.Rows.Count > 0)
                        {
                            MessageBox.Show("Already Details Entered ...!", "Gainup");
                            TxtColor.Focus();
                            return;
                        }
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color", "Select distinct Color, ColorID From Fit_BOM_Details_Cutting () where Order_No='" + TxtOrder_No.Text.ToString() + "' ", String.Empty, 180, 120);
                        if (Dr != null)
                        {
                            TxtColor.Text = Dr["Color"].ToString();
                            TxtColor.Tag = Dr["ColorID"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtParty")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Party", "Select * From ACCOUNTS.dbo.Creditors (" + MyParent.CompCode + ", '" + MyParent.YearCode + "')", String.Empty, 250, 80);
                        if (Dr != null)
                        {
                            TxtParty.Text = Dr["Party"].ToString();
                            TxtParty.Tag = Dr["Code"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtOrder_No")
                    {
                        if (TxtBuyer.Text.Trim() == String.Empty)
                        {
                            MessageBox.Show("Invliad Buyer ...!", "Gainup");
                            TxtBuyer.Focus();
                            return;
                        }

                        if (Dt.Rows.Count > 0)
                        {
                            MessageBox.Show("Already Details Selected ...!", "Gainup");
                            TxtOrder_No.Focus();
                            return;
                        }

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Order No", "Select Distinct Order_No Order_No From Fit_BOM_Details_Cutting () Where BuyerID = " + TxtBuyer.Tag.ToString(), String.Empty, 150);
                        if (Dr != null)
                        {
                            TxtOrder_No.Text = Dr["Order_No"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtProcess")
                    {
                        if (Dt.Rows.Count > 0)
                        {
                            MessageBox.Show("Please Clear All Details ...!", "Gainup");
                            //TxtProcess.Focus();
                            return;
                        }

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Process", "Select Name, RowID, Short_Name From Process_Code_Master ", String.Empty, 180, 60);
                        if (Dr != null)
                        {
                            //TxtProcess.Text = Dr["Name"].ToString();
                            //TxtProcess.Tag = Dr["RowID"].ToString();
                            TxtENo.Text = Dr["Short_Name"].ToString();
                            Entry_No();
                        }
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

        private void Fabric_DC_Outward_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == String.Empty)
                    {
                    }
                    else if (this.ActiveControl.Name == "TxtRemarks")
                    {
                    }
                    else
                    {
                        MyBase.Valid_Null((TextBox)this.ActiveControl, e);
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
                Get_Max_LotNo();
                Total_Qty();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        Boolean Check_Duplicate()
        {
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Dt.Rows.Count - 1; j++)
                    {
                        if (i != j)
                        {
                            if (Grid["ItemID", i].Value.ToString() == Grid["ItemID", j].Value.ToString() && Grid["SizEID", i].Value.ToString() == Grid["SizEID", j].Value.ToString() && Grid["LL", i].Value.ToString() == Grid["LL", j].Value.ToString())
                            {
                                MessageBox.Show("Item Duplication ...!", "Gainup");
                                Grid.CurrentCell = Grid["Item", i];
                                Grid.Focus();
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
                throw ex;
            }
        }

        Boolean Check_Grid()
        {
            Double TQty = 0;
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    // General
                    if (Grid["Body_Weight", i].Value == null || Grid["Body_Weight", i].Value == DBNull.Value || Grid["Body_Weight", i].Value.ToString() == String.Empty)
                    {
                        Grid["Body_Weight", i].Value = "0.000";
                    }
                    if (Grid["Folding", i].Value == null || Grid["Folding", i].Value == DBNull.Value || Grid["Folding", i].Value.ToString() == String.Empty)
                    {
                        Grid["Folding", i].Value = "0.000";
                    }
                    if (Grid["NoRolls", i].Value == null || Grid["NoRolls", i].Value == DBNull.Value || Grid["NoRolls", i].Value.ToString() == String.Empty)
                    {
                        MessageBox.Show("Invalid No Of Rolls ...!", "Gainup");
                        Grid["NoRolls", i].Value = Grid["St_Rolls", i].Value;
                        Grid.CurrentCell = Grid["NoRolls", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return false;
                    }

                    if (Convert.ToDouble(Grid["Body_Weight", i].Value) > Convert.ToDouble(Grid["St_BW", i].Value))
                    {
                        MessageBox.Show("Invalid Body Weight greater than Stock ...!", "Gainup");
                        Grid["Body_Weight", i].Value = Grid["St_BW", i].Value;
                        Grid.CurrentCell = Grid["Body_Weight", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return false;
                    }

                    if (Convert.ToDouble(Grid["Folding", i].Value) > Convert.ToDouble(Grid["St_FD", i].Value))
                    {
                        MessageBox.Show("Invalid Folding greater than Stock ...!", "Gainup");
                        Grid["Folding", i].Value = Grid["St_FD", i].Value;
                        Grid.CurrentCell = Grid["Folding", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return false;
                    }

                    if (Convert.ToDouble(Grid["NoRolls", i].Value) > Convert.ToDouble(Grid["St_Rolls", i].Value))
                    {
                        MessageBox.Show("Invalid Rolls greater than Stock ...!", "Gainup");
                        Grid["NoRolls", i].Value = Grid["NoRolls", i].Value;
                        Grid.CurrentCell = Grid["NoRolls", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return false;
                    }

                    TQty = Convert.ToDouble(Grid["Body_Weight", i].Value) + Convert.ToDouble(Grid["Folding", i].Value);
                    if (TQty == 0)
                    {
                        MessageBox.Show("Invalid Details ...!", "Gainup");
                        Grid.CurrentCell = Grid["Body_Weight", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return false;
                    }

                    if (TQty > Convert.ToDouble(Grid["Bom_Qty", i].Value))
                    {
                        MessageBox.Show("Invalid Kgs Greater than BOM ...!", "Gainup");
                        Grid.CurrentCell = Grid["Body_Weight", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Body_Weight"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Folding"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["NoRolls"].Index)
                {
                    MyBase.Valid_Number(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["LL"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["NoRolls"].Index)
                {
                    e.Handled = true;
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

        void Total_Qty()
        {
            try
            {
                TxtTotal.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum(ref Grid, "Body_Weight", "SizeID", "ItemID", "Item")) + Convert.ToDouble(MyBase.Sum(ref Grid, "Folding", "SizeID", "ItemID", "Item")));
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Item"].Index && e.KeyCode == Keys.Down)
                {
                    MyBase.Row_Number(ref Grid);
                    if (TxtOrder_No.Text.Trim() == String.Empty)
                    {
                        e.Handled = true;

                        MessageBox.Show("Invalid Order No ..!", "Gainup");
                        TxtOrder_No.Focus();
                        return;
                    }
                    Dr = Tool.Selection_Tool_Except_New("Item",this, 30, 70,ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Item", "Select A.item, sum(A.BOM_qty) Bom_Qty, ISNULL(B.Issued_Qty,0) Issued_Qty, A.Itemid From Fit_BOM_Details_Cutting () A Left Join(select buyerID,Order_No,ItemID,ColorID,sum(Body_Weight + Folding) Issued_Qty from Process_Sent() group by buyerID,Order_No,ItemID,ColorID) B on A.BuyerID=B.BuyerID and A.Order_No=B.Order_No And A.Itemid=B.ItemID and A.Colorid=B.ColorID where A.Order_No = '" + TxtOrder_No.Text.Trim() + "' and A.ColorID= " + TxtColor.Tag.ToString() + " Group by A.item, A.Itemid,A.Colorid,B.Issued_Qty", String.Empty, 350, 120, 120);
                    if (Dr != null)
                    {
                        Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                        Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bom_Qty"].ToString();
                        Grid["Issued_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Issued_Qty"].ToString();
                        Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                        Txt.Text = Dr["Item"].ToString();
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Size"].Index && e.KeyCode == Keys.Down)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size", "Select Distinct Size, Sizeid From Fit_Size_Details() ", String.Empty, 120, 80);
                    if (Dr != null)
                    {
                        Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                        Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                        Txt.Text = Dr["Size"].ToString();
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["LL"].Index && e.KeyCode == Keys.Down)
                {
                    if (Grid["SizeID", Grid.CurrentCell.RowIndex].Value == null || Grid["SizeID", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["SizeID", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        e.Handled = true;
                        MessageBox.Show("Invalid Size ...!", "Gainup");
                        Grid.CurrentCell = Grid["Size", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }

                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Available LL", "Select Distinct LL From Fabric_Pending_For_Process () where BuyerID = " + TxtBuyer.Tag.ToString() + " and ItemID = " + Grid["ItemID", Grid.CurrentCell.RowIndex].Value.ToString() + " and Sizeid = " + Grid["SizeID", Grid.CurrentCell.RowIndex].Value.ToString(), String.Empty, 50);
                    if (Dr != null)
                    {
                        Grid["LL", Grid.CurrentCell.RowIndex].Value = Dr["LL"].ToString();
                        Txt.Text = Dr["LL"].ToString();

                        DataTable tempDt = new DataTable();
                        MyBase.Load_Data("Select Body_Weight_Bal, Folding_Bal, NoRolls_Bal From Fabric_Pending_For_Process () Where BuyerID = " + TxtBuyer.Tag.ToString() + " And LL = " + Dr["LL"].ToString() + " And ItemID = " + Grid["ItemID", Grid.CurrentCell.RowIndex].Value.ToString() + " And SizeID = " + Grid["SizeID", Grid.CurrentCell.RowIndex].Value.ToString(), ref tempDt);
                        if (tempDt.Rows.Count > 0)
                        {
                            Grid["St_BW", Grid.CurrentCell.RowIndex].Value = String.Format("{0:0.000}", Convert.ToDouble(tempDt.Rows[0]["Body_Weight_Bal"]));
                            Grid["St_FD", Grid.CurrentCell.RowIndex].Value = String.Format("{0:0.000}", Convert.ToDouble(tempDt.Rows[0]["Folding_Bal"]));
                            Grid["St_Rolls", Grid.CurrentCell.RowIndex].Value = String.Format("{0:0.000}", Convert.ToDouble(tempDt.Rows[0]["NoRolls_Bal"]));
                        }
                        else
                        {
                            Grid["St_BW", Grid.CurrentCell.RowIndex].Value = "0.000";
                            Grid["St_FD", Grid.CurrentCell.RowIndex].Value = "0.000";
                            Grid["St_Rolls", Grid.CurrentCell.RowIndex].Value = "0.000";
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
                    Total_Qty();
                    TxtRemarks.Focus();
                    if (TxtRemarks.Text.Trim() != String.Empty)
                    {
                        SendKeys.Send("{END}");
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
            Double TQty = 0;
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Body_Weight"].Index)
                    {
                        // General
                        if (Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value == null || Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value = "0.000";
                        }
                        if (Grid["Folding", Grid.CurrentCell.RowIndex].Value == null || Grid["Folding", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Folding", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Folding", Grid.CurrentCell.RowIndex].Value = "0.000";
                        }
                        if (Grid["NoRolls", Grid.CurrentCell.RowIndex].Value == null || Grid["NoRolls", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["NoRolls", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["NoRolls", Grid.CurrentCell.RowIndex].Value = "0";
                        }

                        if (Convert.ToDouble(Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["St_BW", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Body Weight greater than Stock ...!", "Gainup");
                            Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value = Grid["St_BW", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["Body_Weight", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (Convert.ToDouble(Grid["Folding", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["St_FD", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Folding greater than Stock ...!", "Gainup");
                            Grid["Folding", Grid.CurrentCell.RowIndex].Value = Grid["St_FD", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["Folding", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (Convert.ToDouble(Grid["NoRolls", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["St_Rolls", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Rolls greater than Stock ...!", "Gainup");
                            Grid["NoRolls", Grid.CurrentCell.RowIndex].Value = Grid["NoRolls", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["NoRolls", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        TQty = Convert.ToDouble(Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value) + Convert.ToDouble(Grid["Folding", Grid.CurrentCell.RowIndex].Value);
                        if (TQty == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show ("Invalid Details ...!", "Gainup");
                            Grid.CurrentCell = Grid["Body_Weight", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (TQty > Convert.ToDouble(Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Kgs Greater than BOM ...!", "Gainup");
                            Grid.CurrentCell = Grid["Body_Weight", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (((Convert.ToDouble(Grid["Issued_Qty", Grid.CurrentCell.RowIndex].Value)) + TQty) > Convert.ToDouble(Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Kgs Greater than BOM ...!", "Gainup");
                            Grid.CurrentCell = Grid["Body_Weight", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Folding"].Index)
                    {

                        //if (Grid["Folding", Grid.CurrentCell.RowIndex].Value == null || Grid["Folding", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Folding", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Folding", Grid.CurrentCell.RowIndex].Value) == 0)
                        //{
                        //    e.Handled = true;
                        //    MessageBox.Show("Invalid Folding ...!", "Gainup");
                        //    Grid.CurrentCell = Grid["Folding", Grid.CurrentCell.RowIndex];
                        //    Grid.Focus();
                        //    Grid.BeginEdit(true);
                        //    return;
                        //}

                        // General
                        if (Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value == null || Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value = "0.000";
                        }
                        if (Grid["Folding", Grid.CurrentCell.RowIndex].Value == null || Grid["Folding", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Folding", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Folding", Grid.CurrentCell.RowIndex].Value = "0.000";
                        }
                        if (Grid["NoRolls", Grid.CurrentCell.RowIndex].Value == null || Grid["NoRolls", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["NoRolls", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["NoRolls", Grid.CurrentCell.RowIndex].Value = "0";
                        }

                        if (Convert.ToDouble(Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["St_BW", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Body Weight greater than Stock ...!", "Gainup");
                            Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value = Grid["St_BW", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["Body_Weight", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (Convert.ToDouble(Grid["Folding", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["St_FD", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Folding greater than Stock ...!", "Gainup");
                            Grid["Folding", Grid.CurrentCell.RowIndex].Value = Grid["St_FD", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["Folding", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (Convert.ToDouble(Grid["NoRolls", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["St_Rolls", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Rolls greater than Stock ...!", "Gainup");
                            Grid["NoRolls", Grid.CurrentCell.RowIndex].Value = Grid["NoRolls", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["NoRolls", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        TQty = Convert.ToDouble(Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value) + Convert.ToDouble(Grid["Folding", Grid.CurrentCell.RowIndex].Value);
                        if (TQty == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Details ...!", "Gainup");
                            Grid.CurrentCell = Grid["Body_Weight", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (TQty > Convert.ToDouble(Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Kgs Greater than BOM ...!", "Gainup");
                            Grid.CurrentCell = Grid["Body_Weight", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (((Convert.ToDouble(Grid["Issued_Qty", Grid.CurrentCell.RowIndex].Value)) + TQty) > Convert.ToDouble(Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Kgs Greater than BOM ...!", "Gainup");
                            Grid.CurrentCell = Grid["Body_Weight", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["NoRolls"].Index)
                    {
                        if (Grid["NoRolls", Grid.CurrentCell.RowIndex].Value == null || Grid["NoRolls", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["NoRolls", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid["NoRolls", Grid.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid No of Rolls ...!", "Gainup");
                            Grid.CurrentCell = Grid["NoRolls", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        // General
                        if (Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value == null || Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value = "0.000";
                        }
                        if (Grid["Folding", Grid.CurrentCell.RowIndex].Value == null || Grid["Folding", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Folding", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Folding", Grid.CurrentCell.RowIndex].Value = "0.000";
                        }
                        if (Grid["NoRolls", Grid.CurrentCell.RowIndex].Value == null || Grid["NoRolls", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["NoRolls", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["NoRolls", Grid.CurrentCell.RowIndex].Value = "0";
                        }

                        if (Convert.ToDouble(Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["St_BW", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Body Weight greater than Stock ...!", "Gainup");
                            Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value = Grid["St_BW", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["Body_Weight", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (Convert.ToDouble(Grid["Folding", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["St_FD", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Folding greater than Stock ...!", "Gainup");
                            Grid["Folding", Grid.CurrentCell.RowIndex].Value = Grid["St_FD", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["Folding", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (Convert.ToDouble(Grid["NoRolls", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["St_Rolls", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Rolls greater than Stock ...!", "Gainup");
                            Grid["NoRolls", Grid.CurrentCell.RowIndex].Value = Grid["NoRolls", Grid.CurrentCell.RowIndex].Value;
                            Grid.CurrentCell = Grid["NoRolls", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        TQty = Convert.ToDouble(Grid["Body_Weight", Grid.CurrentCell.RowIndex].Value) + Convert.ToDouble(Grid["Folding", Grid.CurrentCell.RowIndex].Value);
                        if (TQty == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Details ...!", "Gainup");
                            Grid.CurrentCell = Grid["Body_Weight", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                        if (TQty > Convert.ToDouble(Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Kgs Greater than BOM ...!", "Gainup");
                            Grid.CurrentCell = Grid["Body_Weight", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                        if (((Convert.ToDouble(Grid["Issued_Qty", Grid.CurrentCell.RowIndex].Value)) + TQty) > Convert.ToDouble(Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Kgs Greater than BOM ...!", "Gainup");
                            Grid.CurrentCell = Grid["Body_Weight", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
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

        private void CmbProcess_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                Entry_No();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentCell.RowIndex);
                Total_Qty();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}