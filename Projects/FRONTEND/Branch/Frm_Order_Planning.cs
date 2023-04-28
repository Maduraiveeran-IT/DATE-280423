using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Windows.Forms;
using Accounts;

namespace Accounts
{
    public partial class Frm_Order_Planning : Form
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        DataTable Dt = new DataTable();
        public Int32 OCN_No = 0;
        DataRow Dr;
        Int64 Code = 0;
        public Boolean Result = false;
        public DateTime FromDate;
        public DateTime ToDate;
        public String[] Line_Array;
        public String Output;
        public DataTable Main_Order_Details;

        public Int64 Plan_Secs;

        public DateTime EDate ;
        public Int16 Shift;
        public Int32 Year;
        public Int32 Week;
        public String Needle;
        public Int32 Unit;

        Int32 Unit_New=0;

        public Frm_Order_Planning()
        {
            InitializeComponent();
        }

        void Clear()
        {
            try
            {
                TxtBuyer.Text = String.Empty;
                TxtPlanNo.Text = String.Empty;
                CmbProd.Text = String.Empty;
                CmbColor.Text = String.Empty;
                CmbOCN.Text = String.Empty;
                CmbStyle.Text = String.Empty;
                TxtLinesCount.Text = String.Empty;
                ChkLineList.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        int Get_Planned(String Order_No, String Style, String Color)
        {
            Int32 Result = 0;
            try
            {
                for (int i = 0; i <= Main_Order_Details.Rows.Count - 1; i++)
                {
                    if (Main_Order_Details.Rows[i]["Order_No"].ToString() == Order_No && Main_Order_Details.Rows[i]["Style"].ToString() == Style && Main_Order_Details.Rows[i]["Color"].ToString() == Color)
                    {
                        Result += Convert.ToInt32(Main_Order_Details.Rows[i]["Qty"]);
                    }
                }

                return Result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        void Load_Buyer()
        {
            try
            {
                CmbBuyer.Items.Clear();
                DataTable Tdt = new DataTable();
                String Str = " Select Distinct B.Ledger_Name Buyer From Socks_Joborder_Master A Left Join Buyer_All_Fn()B On A.Buyer_ID = B.Ledger_Code Left Join Socks_Joborder_Details C On A.RowID = C.Master_ID ";
                Str = Str + " Left Join Socks_Order_Master D On C.Order_ID = D.RowID Where A.Unit_Code = 71 And D.Despatch_Closed = 'N' And D.Cancel_Order = 'N' And D.Order_no Like '%OCN%' And D.Order_no not Like '%00000%' Order By Buyer ";
                MyBase.Load_Data(Str, ref Tdt);
                for (int i = 0; i <= Tdt.Rows.Count - 1; i++)
                {
                    CmbBuyer.Items.Add(Tdt.Rows[i]["Buyer"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Load_Operator()
        {
            try
            {
                CmbOperator.Items.Clear();
                DataTable Tdt = new DataTable();
                String Str = " Select Tno + ' - ' + Name Operator From Vaahini_ERP_Gainup.Dbo.Employeemas E1 Inner Join Vaahini_ERP_Gainup.Dbo.Depttype D1 on E1.Deptcode = D1.DeptCode and E1.COMPCODE = D1.compcode Where E1.compcode In (2, 8) and D1.deptCode In (82, 209) and E1.tno not like '%Z' And Unit_Code = " + Unit;
                MyBase.Load_Data(Str, ref Tdt);
                for (int i = 0; i <= Tdt.Rows.Count - 1; i++)
                {
                    CmbOperator.Items.Add(Tdt.Rows[i]["Operator"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Load_All()
        {
            try
            {
                CmbStyle.Items.Clear();
                CmbColor.Items.Clear();
                CmbBom.Items.Clear();
                CmbProd.Items.Clear();
                CmbBal.Items.Clear();
                
                TxtActPlanQty.Text = String.Empty;

                Load_Line_Nos();

                Load_Buyer();

                for (int i = 0; i <= ChkLineList.Items.Count - 1; i++)
                {
                    for (int j = 0; j <= Line_Array.Length - 1; j++)
                    {
                        if (Line_Array[j] != null && ChkLineList.Items[i].ToString() == Line_Array[j].ToString())
                        {
                            ChkLineList.SetItemChecked(i, true);
                        }
                    }
                }
                //ChkLineList.Enabled = false;
                Load_Operator();
                Line_Count_No();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Order_Planning_Load(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                Load_All();
                if (Unit == 1)
                {
                    Unit_New = 71;
                }
                else if (Unit == 2)
                {
                    Unit_New = 72;
                }
                else if (Unit == 3)
                {
                    Unit_New = 74;
                }
                else if (Unit == 4)
                {
                    Unit_New = 75;
                }
                DtpDate.Value = Convert.ToDateTime(EDate);
                TxtShift.Text = Shift.ToString();
                TxtUnit.Text = Unit.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Load_Line_Nos()
        {
            try
            {
                ChkLineList.Items.Clear();

                String Str = " Select Distinct K1.Machine, Left(K1.Machine,PATINDEX('%[0-9]%',K1.Machine)-1) Alpha, CONVERT (Int,SUBSTRING(K1.Machine,PATINDEX('%[0-9]%',K1.Machine),LEN(K1.Machine))) Number From Socks_Machine_Planning_Details S1 ";
                Str = Str + " Left Join Socks_Machine_Planning_Master S2 On S1.Master_ID = S2.RowID Left Join VFit_Sample_Needle_Master V1 on S2.Needle_Id = V1.RowID ";
                Str = Str + " Left Join Production_Qty_WeekWise() P1 on S1.Order_No = P1.Order_No And S2.Needle_Id = P1.Needle_ID and S2.Year = P1.Year and S2.Week = P1.Week ";
                Str = Str + " Right Join Knitting_Mc_NO_UnitWise(" + Unit + ") K1 on S1.Machine_ID = K1.Machine_ID Where S2.Year = " + Year + " and S2.Week = " + Week + " and V1.Name = '" + Needle.ToString().Trim() + "' And ";
                Str = Str + " S1.RowID in (Select MAX(P2.RowID) from Socks_Machine_Planning_Master P1 Left Join Socks_Machine_Planning_Details P2 On P1.RowID = P2.Master_ID ";
                Str = Str + " Left Join VFit_Sample_Needle_Master V1 On P1.Needle_Id = V1.RowID Where P1.Year = " + Year + " and P1.Week = " + Week + " and V1.Name = '" + Needle.ToString().Trim() + "' Group by P2.Machine_ID) ";
                Str = Str + " Order By Alpha, Number";

                MyBase.Load_Data(Str, ref Dt);

                //MyBase.Load_Data("Select No From floor_Lines Where Running = 'Y' order By No", ref Dt);

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    ChkLineList.Items.Add(Dt.Rows[i][0].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Load_Entry_No()
        {
            try
            {
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select Isnull(Max(Plan_No), 0) + 1 From Line_Planning Where Company_Code = " + MyParent.CompCode, ref Tdt);
                TxtPlanNo.Text = Tdt.Rows[0][0].ToString();
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Planning Entry - Edit", "Select Distinct L1.Plan_NO, L1.Plan_Date, L1.Order_No, L2.Buyer, L2.Item, L1.SDate, L1.EDate, L1.TobePlanned,  L1.DelDate, L1.ItemID, L1.RowID, L2.Quantity, L2.Planned, L2.Color From Line_Planning L1 left join Orders_to_be_planned() L2 on L1.Order_No = L2.Order_No and L1.ItemID = L2.ItemID Where L1.Company_Code = " + MyParent.CompCode, String.Empty, 80, 90, 120, 200, 150, 80, 90, 90);
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

        void Fill_Datas(DataRow Dr)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Save()
        {
            try
            {

                if (CmbOCN.Text.Trim() == String.Empty || ChkLineList.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Invalid Data's ...!", "Gainup");
                    CmbOCN.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                Load_Entry_No();

                MyBase.Cn_Open();
                MyBase.ODBCTrans = MyBase.Cn.BeginTransaction();
                MyBase.ODBCCmd = MyBase.Cn.CreateCommand();
                MyBase.ODBCCmd.Connection = MyBase.Cn;
                MyBase.ODBCCmd.Transaction = MyBase.ODBCTrans;
                MyBase.ODBCCmd.CommandType = CommandType.Text;

                if (MyParent._New)
                {
                    MyBase.ODBCCmd.CommandText = "Insert into Line_Planning (Plan_No, Plan_date, Order_No, ItemID, SDate, EDate, DelDate, ToBePlanned, Company_Code, Entry_User) Values ('" + TxtPlanNo.Text.Trim() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + CmbOCN.Text.Trim() + "', " + CmbStyle.Tag.ToString() + ", " + Convert.ToDouble(CmbBal.Text.Trim()) + ", " + MyParent.CompCode + ", " + MyParent.UserCode + "); Select Scope_Identity()";
                    Code = Convert.ToInt64(MyBase.ODBCCmd.ExecuteScalar());
                }
                else
                {

                    MyBase.ODBCCmd.CommandText = "Update Line_Planning Set Order_No = '" + CmbOCN.Text + "', ItemID = " + CmbStyle.Tag.ToString() + ", ToBePlanned = " + Convert.ToDouble(CmbBal.Text) + " where RowID = " + Code;
                    MyBase.ODBCCmd.ExecuteNonQuery();

                    MyBase.ODBCCmd.CommandText = "Delete from Line_Planning_Date_Details where Master_ID = " + Code;
                    MyBase.ODBCCmd.ExecuteNonQuery();

                }

                for (int i = 0; i <= ChkLineList.CheckedItems.Count - 1; i++)
                {

                    //for (DateTime Dat = DtpSDate.Value; Dat <= DtpEDate.Value; Dat = Dat.AddDays(1))
                    //{
                        MyBase.ODBCCmd.CommandText = "Insert into Line_Planning_Date_Details (Master_ID, Plan_Date, Line_No, Status) Values (" + Code + ", " + ChkLineList.CheckedItems[i].ToString() + ", 'O')";
                        MyBase.ODBCCmd.ExecuteScalar();
                    //}

                }

                MyBase.ODBCTrans.Commit();
                MessageBox.Show("Saved ...!", "Gainup");
                MyParent.Save_Error = false;
                MyBase.Clear(this);

            }
            catch (Exception ex)
            {

                if (MyBase.ODBCTrans != null)
                {
                    MyBase.ODBCTrans.Rollback();
                }

                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        void Load_OCN()
        {
            try
            {
                if (CmbBuyer.Text.Trim() != String.Empty)
                {
                    CmbOCN.Items.Clear();
                    DataTable Tdt = new DataTable();

                    String Str = "Exec Ord_Selection " + Unit_New + ", '" + CmbBuyer.Text.Trim() + "', '" + Needle + "'";

                    MyBase.Load_Data(Str, ref Tdt);
                    for (int i = 0; i <= Tdt.Rows.Count - 1; i++)
                    {
                        CmbOCN.Items.Add(Tdt.Rows[i]["Order_No"].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Buyer...!", "Gainup");
                    CmbBuyer.Focus();
                    return;
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Planning Entry - Delete ", "Select Distinct L1.Plan_NO, L1.Plan_Date, L1.Order_No, L2.Buyer, L2.Item, L1.SDate, L1.EDate, L1.TobePlanned,  L1.DelDate, L1.ItemID, L1.RowID, L2.Quantity, L2.Planned, L2.Color From Line_Planning L1 left join Orders_to_be_planned() L2 on L1.Order_No = L2.Order_No and L1.ItemID = L2.ItemID Where L1.Company_Code = " + MyParent.CompCode, String.Empty, 80, 90, 120, 200, 150, 80, 90, 90);
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
                    MyBase.Cn_Open();
                    MyBase.ODBCTrans = MyBase.Cn.BeginTransaction();
                    MyBase.ODBCCmd = MyBase.Cn.CreateCommand();
                    MyBase.ODBCCmd.Connection = MyBase.Cn;
                    MyBase.ODBCCmd.Transaction = MyBase.ODBCTrans;
                    MyBase.ODBCCmd.CommandType = CommandType.Text;

                    MyBase.ODBCCmd.CommandText = "Delete from Line_Planning_Date_Details where Master_ID = " + Code;
                    MyBase.ODBCCmd.ExecuteNonQuery();

                    MyBase.ODBCCmd.CommandText = "Delete from Line_Planning Where RowiD = " + Code;
                    MyBase.ODBCCmd.ExecuteNonQuery();

                    MyBase.ODBCTrans.Commit();

                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyBase.Clear(this);

                }
                MyParent.Load_DeleteEntry();
            }
            catch (Exception ex)
            {
                if (MyBase.ODBCTrans != null)
                {
                    MyBase.ODBCTrans.Rollback();
                }
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Planning Entry - View ", "Select Distinct L1.Plan_NO, L1.Plan_Date, L1.Order_No, L2.Buyer, L2.Item, L1.SDate, L1.EDate, L1.TobePlanned,  L1.DelDate, L1.ItemID, L1.RowID, L2.Quantity, L2.Planned, L2.Color From Line_Planning L1 left join Orders_to_be_planned() L2 on L1.Order_No = L2.Order_No and L1.ItemID = L2.ItemID Where L1.Company_Code = " + MyParent.CompCode, String.Empty, 80, 90, 120, 200, 150, 80, 90, 90);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Order_Planning_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtActPlanQty")
                    {
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtOCN")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Order", "Select Order_No, Buyer, Item, Color, Quantity, Planned, ToBePlanned, ItemID From Orders_to_be_planned  ()", String.Empty, 120, 250, 120, 120, 90, 90, 90);
                        if (Dr != null)
                        {

                        }
                    }
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    Result = false;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Order_Planning_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtActPlanQty")
                    {
                        MyBase.Valid_Number(TxtActPlanQty, e);
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
        void Select_Items()
        {
            try
            {
                for (int i = 0; i <= ChkLineList.SelectedItems.Count - 1; i++)
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChkLineList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Line_Count_No();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChkLineList_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                Line_Count_No();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Line_Count_No()
        {
            try
            {
                TxtLinesCount.Text = "MC's : " + ChkLineList.CheckedItems.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChkLineList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                Line_Count_No();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbOCN_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CmbStyle.Items.Clear();
                CmbItem.Items.Clear();
                CmbColor.Items.Clear();
                CmbBom.Items.Clear();
                CmbProd.Items.Clear();
                CmbBal.Items.Clear();
                TxtBufferPlanned.Text = "";
                TxtToBePlanning.Text = "";
                TxtTarget.Text = "";
                TxtActPlanQty.Text = "";

                DataTable Tdt = new DataTable();

                String Str = "Exec Ord_Sample_Selection " + Unit_New + ", '" + CmbBuyer.Text.ToString().Trim() + "', '" + Needle.ToString().Trim() + "', '" + CmbOCN.Text.ToString().Trim() + "'";

                MyBase.Load_Data(Str, ref Tdt);

                CmbStyle.Items.Clear();
                
                for (int i = 0; i <= Tdt.Rows.Count - 1; i++)
                {
                    TxtBuyer.Text = CmbBuyer.Text.ToString().Trim();
                    CmbStyle.Items.Add(Tdt.Rows[i]["Sample_No"].ToString());
                }

                Grid_Sample.DataSource = MyBase.V_DataTable(ref Tdt);
                MyBase.Row_Number(ref Grid_Sample);
                MyBase.Grid_Designing(ref Grid_Sample, ref Tdt, "OrderColorID", "Needle");
                MyBase.ReadOnly_Grid_Without(ref Grid_Sample);
                MyBase.Grid_Colouring(ref Grid_Sample, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid_Sample, 40, 100, 80, 80, 100, 60, 70, 70, 70, 70, 70, 70, 70, 150);
                
                MyBase.V_DataGridView(ref Grid_Sample);
                Grid_Sample.RowHeadersWidth = 10;
                Grid_Sample.Rows[Grid_Sample.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void CmbOCN_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataTable Tdt = new DataTable();

        //        String Str = "Exec Ord_Sample_Selection " + Unit_New + ", '" + CmbBuyer.Text.ToString().Trim() + "', '" + Needle.ToString().Trim() + "', '" + CmbOCN.Text.ToString().Trim() + "'";
                
        //        MyBase.Load_Data(Str, ref Tdt);

        //        CmbStyle.Items.Clear();
        //        CmbColor.Items.Clear();
        //        CmbQuantity.Items.Clear();
        //        CmbPlanned.Items.Clear();
        //        CmbToBePlanned.Items.Clear();
        //        CmbDelDate.Items.Clear();


        //        for (int i = 0; i <= Tdt.Rows.Count - 1; i++)
        //        {
        //            TxtBuyer.Text = Tdt.Rows[i]["Buyer"].ToString();
        //            CmbStyle.Items.Add(Tdt.Rows[i]["Sample_No"].ToString());
        //            CmbColor.Items.Add(Tdt.Rows[i]["Color"].ToString());
        //            CmbQuantity.Items.Add(Tdt.Rows[i]["Quantity"].ToString());
        //            CmbPlanned.Items.Add(Tdt.Rows[i]["Planned"].ToString());
        //            CmbToBePlanned.Items.Add(Tdt.Rows[i]["ToBePlanned"].ToString());
        //            CmbDelDate.Items.Add(String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Tdt.Rows[i]["Del_Date"])));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        void Make_All_Select(int RowIndex)
        {
            try
            {
                CmbColor.SelectedIndex = RowIndex;
                CmbStyle.SelectedIndex = RowIndex;
                CmbBom.SelectedIndex = RowIndex;
                CmbProd.SelectedIndex = RowIndex;
                CmbBal.SelectedIndex = RowIndex;

                TxtBufferPlanned.Text = Convert.ToString(Get_Planned(CmbOCN.Text.Trim(), CmbStyle.Text.Trim(), CmbColor.Text.Trim()));

                Double Perday_Qty = 0;
                Perday_Qty = Convert.ToDouble(CmbBal.Text) - Convert.ToDouble(TxtBufferPlanned.Text);
               // Perday_Qty = Math.Round((Perday_Qty / Convert.ToDouble((DtpEDate.Value.AddDays(1) - DtpSDate.Value).Days * ChkLineList.CheckedItems.Count)), 1);
                Perday_Qty = Math.Truncate(Perday_Qty);
                if (Perday_Qty > 0)
                {
                    TxtActPlanQty.Text = String.Format("{0:0}", Perday_Qty);
                }
                else
                {
                    TxtActPlanQty.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CmbStyle.Text.ToString().Trim() != String.Empty)
                {
                    CmbItem.Items.Clear();
                    CmbColor.Items.Clear();
                    CmbBom.Items.Clear();
                    CmbProd.Items.Clear();
                    CmbBal.Items.Clear();

                    if (Grid_Sample.Rows.Count > 0)
                    {
                        for (int i = 0; i <= Grid_Sample.Rows.Count - 2; i++)
                        {
                            if (Grid_Sample["Sample_No", i].Value.ToString() == CmbStyle.Text.ToString().Trim())
                            {
                                CmbItem.Items.Add(Grid_Sample["Item", i].Value.ToString() + "$" + Grid_Sample["Size", i].Value.ToString());
                                CmbColor.Items.Add(Grid_Sample["Color", i].Value.ToString());
                                CmbBom.Items.Add(Grid_Sample["Bom_Qty", i].Value.ToString());
                                CmbProd.Items.Add(Grid_Sample["Production", i].Value.ToString());
                                CmbBal.Items.Add(Grid_Sample["Bal", i].Value.ToString());

                                CmbItem.SelectedIndex = 0;
                                CmbColor.SelectedIndex = 0;
                                CmbBom.SelectedIndex = 0;
                                CmbProd.SelectedIndex = 0;
                                CmbBal.SelectedIndex = 0;

                                TxtTarget.Text = Grid_Sample["Target", i].Value.ToString();

                                DataTable DtP = new DataTable();
                                
                                String Str = " Select Isnull(Sum(B.Qty), 0)Plan_Qty From Socks_Machine_Production_Master A ";
                                Str = Str + " Left Join Socks_Machine_Production_Details B On A.RowID = B.Master_ID ";
                                Str = Str + " Left Join VFit_Sample_Needle_Master C On A.Needle_ID = C.RowID ";
                                Str = Str + " Left Join Knitting_MC_No_Unit_Report_new()D On A.Machine_ID = D.Machine_ID ";
                                Str = Str + " Where A.Entry_Date = '" + String.Format("{0:dd-MMM-yyyy}", EDate) + "' And A.Shift = " + Shift + " And D.Unit_Code = " + Unit_New + " And C.Name = '" + Needle + "' And B.Order_No = '" + CmbOCN.Text.ToString() + "' ";
                                Str = Str + " And B.OrderColorID = " + Grid_Sample["OrderColorID", i].Value.ToString() + " ";

                                MyBase.Load_Data(Str, ref DtP);
                                if (DtP.Rows.Count > 0)
                                {
                                    TxtBufferPlanned.Text = DtP.Rows[0][0].ToString();
                                }
                                else
                                {
                                    TxtBufferPlanned.Text = "0";
                                }
                                TxtToBePlanning.Text = Convert.ToString(Convert.ToDouble(CmbBal.Text) - Convert.ToDouble(TxtBufferPlanned.Text));
                                if (Convert.ToDouble(TxtTarget.Text.ToString()) < (Convert.ToDouble(CmbBal.Text) - Convert.ToDouble(TxtBufferPlanned.Text)))
                                {
                                    TxtActPlanQty.Text = TxtTarget.Text.ToString();
                                }
                                else
                                {
                                    TxtActPlanQty.Text = Convert.ToString(Convert.ToDouble(CmbBal.Text) - Convert.ToDouble(TxtBufferPlanned.Text));
                                }
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

        private void CmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Make_All_Select(CmbColor.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Make_All_Select(CmbBom.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbPlanned_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Make_All_Select(CmbProd.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbToBePlanned_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Make_All_Select(CmbBal.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbDelDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Make_All_Select(CmbDelDate.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtActPlanQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtActPlanQty_Leave(object sender, EventArgs e)
        {
            try
            {
                if (TxtActPlanQty.Text.Trim() == String.Empty)
                {
                    TxtActPlanQty.Text = "0";
                }

                if (Convert.ToDouble(TxtActPlanQty.Text) > Convert.ToDouble(CmbBal.Text))
                {
                    MessageBox.Show("Invalid Plan Qty ...!", "Gainup");
                    TxtActPlanQty.Text = CmbBal.Text;
                    TxtActPlanQty.SelectAll();
                    TxtActPlanQty.Focus();
                }
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
                Load_All();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Double FillQty = 0;
            try
            {
                if (TxtActPlanQty.Text.Trim() == String.Empty || Convert.ToInt32(TxtActPlanQty.Text.Trim()) == 0)
                {
                    MessageBox.Show("Invalid Plan Qty ...!", "Gainup");
                    TxtActPlanQty.Focus();
                    return;
                }

                if (Convert.ToInt32(TxtActPlanQty.Text.Trim()) > Convert.ToInt32(CmbBal.Text))
                {
                    MessageBox.Show("Invalid Plan Qty ...!", "Gainup");
                    TxtActPlanQty.Focus();
                    return;
                }

                //FillQty = (DtpEDate.Value.AddDays(1) - DtpSDate.Value).Days * ChkLineList.CheckedItems.Count * Convert.ToDouble(TxtActPlanQty.Text);

                if (Convert.ToDouble(CmbBom.Text) < FillQty)
                {
                    MessageBox.Show("You Can't Fill higher Quantity " + FillQty.ToString() + "/" + CmbBom.Text + " ...!", "Gainup");
                    return;
                }

                OCN_No = Convert.ToInt32(CmbOCN.Text.Substring(8, 4));

                /// Result

                String[] Item = CmbItem.Text.ToString().Split('$');

                Output = "BUY : " + CmbBuyer.Text.ToString().Trim(); Output += Environment.NewLine;
                Output += "OCN : " + CmbOCN.Text.ToString().Trim(); Output += Environment.NewLine;
                Output += "SAM : " + CmbStyle.Text.ToString().Trim(); Output += Environment.NewLine;
                Output += "ITM : " + Item[0].ToString().Trim(); Output += Environment.NewLine;
                Output += "COL : " + CmbColor.Text.ToString().Trim(); Output += Environment.NewLine;
                Output += "SIZ : " + Item[1].ToString().Trim(); Output += Environment.NewLine;
                Output += "OPR : " + CmbOperator.Text.ToString().Trim(); Output += Environment.NewLine;
                Output += "QTY : " + CmbBom.Text.ToString().Trim(); Output += Environment.NewLine;
               // Output += "MCN : " + CmbBuyer.Text.ToString().Trim(); Output += Environment.NewLine;
                Output += "BAL : " + CmbBal.Text.ToString().Trim(); Output += Environment.NewLine;
                Output += "PLD : " + TxtActPlanQty.Text.ToString().Trim(); Output += Environment.NewLine;
                Output += "TPL : " + TxtToBePlanning.Text.ToString().Trim(); Output += Environment.NewLine;
                Output += "DAT : " + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value); Output += Environment.NewLine;
                Output += "SHT : " + TxtShift.Text.ToString().Trim(); Output += Environment.NewLine;

                Plan_Secs =0;
                for (int i = 0; i <= Grid_Sample.Rows.Count - 2;i++)
                {
                    if(CmbStyle.Text.ToString().Trim() == Grid_Sample["Sample_no", i].Value.ToString().Trim())
                    {
                        Plan_Secs = Convert.ToInt64(Grid_Sample["Cycle_Pair_Seconds", i].Value.ToString()) * Convert.ToInt64(TxtActPlanQty.Text);
                    }
                }
                Output += "PLS : " + Plan_Secs.ToString(); Output += Environment.NewLine;

                Result = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbBuyer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CmbOCN.Items.Clear();
                CmbStyle.Items.Clear();
                CmbItem.Items.Clear();
                CmbColor.Items.Clear();
                CmbBom.Items.Clear();
                CmbProd.Items.Clear();
                CmbBal.Items.Clear();
                TxtBufferPlanned.Text = "";
                TxtToBePlanning.Text = "";
                TxtTarget.Text = "";
                TxtActPlanQty.Text = "";
                Grid_Sample.DataSource = null;
                
                Load_OCN();
                CmbOCN.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
