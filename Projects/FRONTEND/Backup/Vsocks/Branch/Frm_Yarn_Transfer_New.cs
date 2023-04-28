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
    public partial class Frm_Yarn_Transfer_New : Form, Entry
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
        TextBox Txt1 = null;
        String[] Queries;
        String Str;
        TextBox TxtOrder = null;
        String Eno;
        String A;
        String Buffer_Table = String.Empty;
        Boolean Buffer_Update = false;
        Int64 Mode = 0;
        Double Tfr = 0;

        Int16 Vis = 0;
        int Pos = 0;

        Int64 ItemID = 0;
        Int64 ColorID = 0;
        Int64 SizeID = 0;
        public Frm_Yarn_Transfer_New()
        {
            InitializeComponent();
        }

        private void Frm_Yarn_Transfer_New_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                ChkClosed.Visible = false;
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
                //11- Hari Ram
                //39- Ram Kumar
                //12- Mani Kumar
                //13- Naresh Kumar
                //15- Alagu Pandi
                if (MyParent.UserCode == 11 || MyParent.UserCode == 39 || MyParent.UserCode == 12 || MyParent.UserCode == 13 || MyParent.UserCode == 15)
                {
                    TxtItem.Enabled = false;
                    TxtColor.Enabled = false;
                    TxtSize.Enabled = false;
                }

                
                //if (MyParent.UserCode == 1 || MyParent.UserCode == 19 || MyParent.UserCode == 11 || MyParent.UserCode == 39)
                //{
                //    ChkClosed.Enabled = true;
                //    ChkClosed.Visible = true;
                //    if (ChkClosed.Checked == true)
                //    {
                //        ChkClosed.Checked = false;
                //    }
                //}
                
                //19 - Gopal
                //11 - Hari Ram
                if (MyParent.UserCode == 1 || MyParent.UserCode == 19)
                {
                    ChkClosed.Enabled = true;
                    ChkClosed.Visible = true;
                    if (ChkClosed.Checked == true)
                    {
                        ChkClosed.Checked = false;
                    }
                }
                else
                {
                    if (ChkClosed.Checked == true)
                    {
                        ChkClosed.Checked = false;
                    }
                    ChkClosed.Enabled = false;
                    ChkClosed.Visible = false;
                }

                if (ChkOtherMerchandiser.Checked == true)
                {
                    ChkOtherMerchandiser.Checked = false;
                }
                
                if (MyParent.UserCode == 19)
                {
                    ChkOtherMerchandiser.Enabled = false;
                    ChkOtherMerchandiser.Visible = false;
                }
                
                if (MyParent.UserCode != 11 && MyParent.UserCode != 39 && MyParent.UserCode != 12 && MyParent.UserCode != 13 && MyParent.UserCode != 15)
                {
                    TxtItem.Focus();
                    Grid_Data();
                    DtQty = new DataTable[30];
                    return;
                }
                else
                {
                    Grid_Data();
                    DtQty = new DataTable[30];
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    return;
                }
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
                
                Str = " Select A.TransferNo, A.Entryno, A.TransferDate, D.Item, E.Color, F.Size, B.From_Order_no, C.To_Order_No, SUM(Isnull(C.Weight,0)) Transfer_Qty, A.RowID, B.ItemID, B.ColorID, B.SizeID, B.Grn_No From Socks_Store_Yarn_Transfer_Master A ";
                Str = Str + " Left Join Socks_Store_Yarn_Transfer_Details B On A.RowID = B.MasterID Left Join Socks_Store_Yarn_SampleWise_Transfer_Details C On A.RowID = C.MasterID And B.SlNO1 = C.Slno1 ";
                Str = Str + " Left Join Item D On B.ItemID = D.ItemID Left Join Color E On B.ColorID = E.ColorID Left Join Size F On B.SizeID = F.SizeID ";
                Str = Str + " Group BY A.TransferNo, A.Entryno, A.TransferDate, D.Item, E.Color, F.Size, B.From_Order_no, C.To_Order_No, A.RowID, B.ItemID, B.ColorID, B.SizeID, B.Grn_No Order By A.TransferNo Desc, A.Entryno Desc ";
                
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Store Yarn Stock Transfer Entry - Edit", Str, String.Empty, 120, 80, 100, 150, 140, 120, 120, 120, 100);
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
                DtQty = new DataTable[30];

                Str = " Select A.TransferNo, A.Entryno, A.TransferDate, D.Item, E.Color, F.Size, B.From_Order_no, C.To_Order_No, SUM(Isnull(C.Weight,0)) Transfer_Qty, A.RowID, B.ItemID, B.ColorID, B.SizeID, B.Grn_No From Socks_Store_Yarn_Transfer_Master A ";
                Str = Str + " Left Join Socks_Store_Yarn_Transfer_Details B On A.RowID = B.MasterID Left Join Socks_Store_Yarn_SampleWise_Transfer_Details C On A.RowID = C.MasterID And B.SlNO1 = C.Slno1 ";
                Str = Str + " Left Join Item D On B.ItemID = D.ItemID Left Join Color E On B.ColorID = E.ColorID Left Join Size F On B.SizeID = F.SizeID ";
                Str = Str + " Group BY A.TransferNo, A.Entryno, A.TransferDate, D.Item, E.Color, F.Size, B.From_Order_no, C.To_Order_No, A.RowID, B.ItemID, B.ColorID, B.SizeID, B.Grn_No Order By A.TransferNo Desc, A.Entryno Desc ";
                
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Store Yarn Transfer Entry - View", Str, String.Empty, 120, 80, 100, 150, 140, 120, 120, 120, 100);
                
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

        Boolean Check_Qty_Breakup()
        {
            Double BRQty = 0;
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    BRQty = 0;
                    if (Convert.ToDouble(Dt.Rows[i]["Iss_Qty"].ToString()) > 0)
                    {
                        if (DtQty[Convert.ToInt32(Dt.Rows[i]["SlNo1"])] == null)
                        {
                            MessageBox.Show("Invalid Orderwise Breakup Details  ...!", "Gainup");
                            MyParent.Save_Error = true;
                            Grid.CurrentCell = Grid["Iss_Qty", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return false;
                        }
                        else
                        {
                            for (int j = 0; j <= DtQty[Convert.ToInt32(Dt.Rows[i]["Slno1"])].Rows.Count - 1; j++)
                            {
                                BRQty += Convert.ToDouble(DtQty[Convert.ToInt32(Dt.Rows[i]["Slno1"])].Rows[j]["Iss_Qty"]);
                            }

                            if (Math.Round(Convert.ToDouble(BRQty),3) != Convert.ToDouble(Grid["Iss_Qty", i].Value))
                            {
                                MessageBox.Show("Check Orderwise Iss Qty...!", "Gainup");                                
                                Grid.CurrentCell = Grid["Iss_Qty", i];
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
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void Entry_Save()
        {
            try
            {
                Int32 Array_Index = 0;
                
                String From_Store = String.Empty;
                Total_Count();
                DataTable Is1 = new DataTable();
                DataTable Is2 = new DataTable();

                if (GBQty.Visible)
                {
                    Total_Count();
                    ButOk.PerformClick();
                    //GBQty.Visible = false;
                }

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
                    if (Grid["Iss_Qty", i].Value == DBNull.Value || Grid["Iss_Qty", i].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Iss_Qty", i].Value) == 0)
                    {
                        MessageBox.Show(" ZERO is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Iss_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }

                    for (i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (DtQty[Convert.ToInt32(Dt.Rows[i]["SlNo1"])] == null)
                        {
                            MessageBox.Show("Invalid Orderwise Breakup Details ...!", "Gainup");
                            MyParent.Save_Error = true;
                            Grid.CurrentCell = Grid["Iss_Qty", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                    }

                }

                for (i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (DtQty[i + 1] != null)
                    {
                        for(int j = 0; j <= DtQty[i + 1].Rows.Count - 1; j++)
                        {
                            ItemID = Convert.ToInt64(DtQty[i + 1].Rows[j]["ItemID"].ToString());
                            ColorID = Convert.ToInt64(DtQty[i + 1].Rows[j]["ColorID"].ToString());
                            SizeID = Convert.ToInt64(DtQty[i + 1].Rows[j]["SizeID"].ToString());
                            if (Convert.ToDouble(Fill_BOM_Check(DtQty[i + 1].Rows[j]["Order_No"].ToString())) < 0)
                            {
                                MessageBox.Show("Invalid Orderwise Breakup Details ...!", "Gainup");
                                MyParent.Save_Error = true;
                                Grid.CurrentCell = Grid["Iss_Qty", 0];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                        }
                    }
                }

                if (!Check_Qty_Breakup())
                {
                   // MessageBox.Show("Invalid Orderwise Breakup Details...!", "Gainup");
                    MyParent.Save_Error = true;
                    return;
                }

                if (MyParent._New == true)
                {
                    TxtEntryNo.Text = MyBase.MaxOnlyWithoutComp("Socks_Store_Yarn_Transfer_Master", "EntryNO", String.Empty, String.Empty, 0).ToString();
                }

                if (MyParent._New)
                {
                    Str = " Select Cast('GUP-SST' As Varchar(20)) + RIGHT('00000' + Cast(Isnull(Max(Cast(Replace(TransferNo,'GUP-SST','') As Numeric(20))),0) + 1 As Varchar(20)), 5)TransferNO from Socks_Store_Yarn_Transfer_Master ";
                    MyBase.Load_Data(Str, ref Is1);
                }

                Queries = new string[Dt.Rows.Count * 150];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_Store_Yarn_Transfer_Master (EntryNo, TransferNo, TransferDate, UserCode, Compcode, EntrySystem, EntryTime, Remarks) values (" + TxtEntryNo.Text + ", '" + Is1.Rows[0]["TransferNo"] + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + MyParent.UserCode + ", " + MyParent.CompCode + ", Host_Name(), GetDate(), '" + TxtRemarks.Text + "' ); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_Store_Yarn_Transfer_Master", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Exec Update_Socks_Yarn_BOM_Status_Trasfer_In " + Code;
                    Queries[Array_Index++] = "Exec Update_Socks_Yarn_BOM_Status_Trasfer_Out " + Code;
                    Queries[Array_Index++] = "Update Socks_Store_Yarn_Transfer_Master Set TransferDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',  Remarks = '" + TxtRemarks.Text + "', CompCode = " + MyParent.CompCode + ", UserCode=" + MyParent.UserCode + " Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_Store_Yarn_Transfer_Master", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from Socks_Store_Yarn_Transfer_Details where MasterID = " + Code;
                    Queries[Array_Index++] = "Delete from Socks_Store_Yarn_SampleWise_Transfer_Details where MasterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Socks_Store_Yarn_Transfer_Details (MasterID, Slno, Grn_No, From_Order_NO, ItemID, ColorID, SizeID, LotNo, TransferQty, Slno1) Values (@@IDENTITY, " + Grid["Slno", i].Value + ", '" + Grid["Grn_No", i].Value.ToString() + "', '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + ", " + Grid["SizeID", i].Value + ", '" + Grid["LotNo", i].Value + "', '" + Grid["Iss_Qty", i].Value + "', " + Grid["Slno1", i].Value + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Socks_Store_Yarn_Transfer_Details (MasterID, Slno, Grn_No, From_Order_NO, ItemID, ColorID, SizeID, LotNo, TransferQty, Slno1) Values (" + Code + ", " + Grid["Slno", i].Value + ", '" + Grid["Grn_No", i].Value.ToString() + "', '" + Grid["Order_No", i].Value.ToString() + "', " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + ", " + Grid["SizeID", i].Value + ", '" + Grid["LotNo", i].Value + "', '" + Grid["Iss_Qty", i].Value + "', " + Grid["Slno1", i].Value + ")";
                    }
                }

                if (MyParent._New)
                {
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Convert.ToDouble(Grid["Iss_Qty", i].Value.ToString()) > 0 && DtQty[i + 1] != null)
                        {
                            DataTable Dt1 = new DataTable();
                            Str = " Select Grn_Date, Grn_No, LotNo, BagNo, Cur_Stock Stock, VSocks_Lot_Bag_Details_RowID From Socks_Store_Current_Stock() ";
                            Str = Str + " Where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' And ItemID = " + Grid["ItemID", i].Value + " And ColorID = " + Grid["ColorID", i].Value + " And SizeID = " + Grid["SizeID", i].Value + " And LotNo = '" + Grid["LotNo", i].Value + "' And Grn_No = '" + Grid["Grn_No", i].Value + "' and Cur_Stock > 0";
                            Str = Str + " Order By Grn_Date, Grn_No, LotNo, BagNo ";

                            MyBase.Load_Data(Str, ref Dt1);

                            int l = 0;
                            Double Bal_Qty = 0.000;
                            Double Tot_Qty = 0.000;
                            Double Stock = 0.000;
                            Double Tot_Iss_Qty = 0.000;
                            Double Iss_Qty = 0.000;

                            if (Dt1.Rows.Count > 0)
                            {
                                for (int j = 0; j <= DtQty[i + 1].Rows.Count - 1; j++)
                                {
                                    if (Math.Round(Bal_Qty, 3) == 0)
                                    {
                                        Tot_Iss_Qty = Math.Round(Tot_Iss_Qty, 3) + Math.Round(Convert.ToDouble(DtQty[i + 1].Rows[j]["Iss_Qty"].ToString()), 3);
                                        Bal_Qty = Math.Round(Convert.ToDouble(DtQty[i + 1].Rows[j]["Iss_Qty"].ToString()), 3);
                                    }

                                    if (Math.Round(Stock, 3) == 0)
                                    {
                                        Stock = Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                        Iss_Qty = Math.Round(Iss_Qty, 3) + Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                    }

                                    //while (Convert.ToDouble(Tot_Iss_Qty) > Convert.ToDouble(Iss_Qty) && Math.Round(Bal_Qty, 3) > 0)
                                    while (Math.Round(Bal_Qty, 3) > 0)
                                    {
                                        if (Math.Round(Stock, 3) == 0)
                                        {
                                            Stock = Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                            Iss_Qty = Math.Round(Iss_Qty, 3) + Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                        }

                                        if (Math.Round(Iss_Qty, 3) < Math.Round(Tot_Iss_Qty, 3) && Math.Round(Stock, 3) > Math.Round(Bal_Qty, 3))
                                        {
                                            Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_SampleWise_Transfer_Details (MasterID, SlNo, SlNo1, To_Order_No, Weight, BagNo, VSocks_Lot_Bag_Details_RowID) Values (@@IDENTITY, " + DtQty[i + 1].Rows[j]["Slno"].ToString() + ", " + DtQty[i + 1].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "', " + Dt1.Rows[l]["Stock"].ToString() + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ")";
                                            Queries[Array_Index++] = "Update A Set Transfer_In = Transfer_In + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                            Queries[Array_Index++] = "Update A Set Transfer_Out = Transfer_Out + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                            Stock = Math.Round(Stock, 3) - Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                            Bal_Qty = Math.Round(Bal_Qty, 3) - Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                        }
                                        else if ((Math.Round(Iss_Qty, 3) >= Math.Round(Tot_Iss_Qty, 3)) && (Math.Round(Bal_Qty, 3) > 0))
                                        {
                                            //if (Math.Round(Bal_Qty, 3) <= Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()),3))
                                            if (Math.Round(Bal_Qty, 3) <= Math.Round(Stock, 3))
                                            {
                                                Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_SampleWise_Transfer_Details (MasterID, SlNo, SlNo1, To_Order_No, Weight, BagNo, VSocks_Lot_Bag_Details_RowID) Values (@@IDENTITY, " + DtQty[i + 1].Rows[j]["Slno"].ToString() + ", " + DtQty[i + 1].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "', " + Math.Round(Bal_Qty, 3) + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ")";
                                                Queries[Array_Index++] = "Update A Set Transfer_In = Transfer_In + " + Math.Round(Bal_Qty, 3) + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                                Queries[Array_Index++] = "Update A Set Transfer_Out = Transfer_Out + " + Math.Round(Bal_Qty, 3) + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                                Stock = Math.Round(Stock, 3) - Math.Round(Convert.ToDouble(Bal_Qty.ToString()), 3);
                                                Bal_Qty = 0;

                                            }
                                            else if (Math.Round(Bal_Qty, 3) > Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3))
                                            {
                                                Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_SampleWise_Transfer_Details (MasterID, SlNo, SlNo1, To_Order_No, Weight, BagNo, VSocks_Lot_Bag_Details_RowID) Values (@@IDENTITY, " + DtQty[i + 1].Rows[j]["Slno"].ToString() + ", " + DtQty[i + 1].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "', " + Dt1.Rows[l]["Stock"].ToString() + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ")";
                                                Queries[Array_Index++] = "Update A Set Transfer_In = Transfer_In + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                                Queries[Array_Index++] = "Update A Set Transfer_Out = Transfer_Out + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                                Stock = Math.Round(Stock, 3) - Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                                Bal_Qty = Math.Round(Bal_Qty, 3) - Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                            }
                                        }
                                        else if (Math.Round(Bal_Qty, 3) > Math.Round(Stock, 3))
                                        {
                                            Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_SampleWise_Transfer_Details (MasterID, SlNo, SlNo1, To_Order_No, Weight, BagNo, VSocks_Lot_Bag_Details_RowID) Values (@@IDENTITY, " + DtQty[i + 1].Rows[j]["Slno"].ToString() + ", " + DtQty[i + 1].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "', " + Math.Round(Stock, 3) + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ")";
                                            Queries[Array_Index++] = "Update A Set Transfer_In = Transfer_In + " + Math.Round(Stock, 3) + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                            Queries[Array_Index++] = "Update A Set Transfer_Out = Transfer_Out + " + Math.Round(Stock, 3) + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                            Bal_Qty = Math.Round(Bal_Qty, 3) - Math.Round(Stock, 3);
                                            Stock = Math.Round(Stock, 3) - Math.Round(Stock, 3);
                                        }
                                        if (Math.Round(Stock, 3) == 0 && Math.Round(Bal_Qty, 3) > 0)
                                        {
                                            l = l + 1;
                                        }
                                    }
                                    if (Math.Round(Stock, 3) == 0 && Math.Round(Bal_Qty, 3) > 0)
                                    {
                                        l = l + 1;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Convert.ToDouble(Grid["Iss_Qty", i].Value.ToString()) > 0 && DtQty[i + 1] != null)
                        {
                            DataTable Dt1 = new DataTable();

                            Str = " Select Grn_Date, Grn_No, LotNo, BagNo, Stock, VSocks_Lot_Bag_Details_RowID From Socks_Store_Current_Stock_For_Transfer_Edit_New('" + Grid["Order_No", i].Value.ToString() + "' , '" + Code + "', " + Grid["ItemID", i].Value + ", " + Grid["ColorID", i].Value + ", " + Grid["SizeID", i].Value + ", '" + Grid["Grn_No", i].Value.ToString() + "', '" + Grid["LotNO", i].Value.ToString() + "')";

                            MyBase.Load_Data(Str, ref Dt1);
                            int l = 0;
                            Double Bal_Qty = 0.000;
                            Double Tot_Qty = 0.000;
                            Double Stock = 0.000;
                            Double Tot_Iss_Qty = 0.000;
                            Double Iss_Qty = 0.000;

                            if (Dt1.Rows.Count > 0)
                            {
                                for (int j = 0; j <= DtQty[i + 1].Rows.Count - 1; j++)
                                {
                                    if (Math.Round(Bal_Qty, 3) == 0)
                                    {
                                        Tot_Iss_Qty = Math.Round(Tot_Iss_Qty, 3) + Math.Round(Convert.ToDouble(DtQty[i + 1].Rows[j]["Iss_Qty"].ToString()), 3);
                                        Bal_Qty = Math.Round(Convert.ToDouble(DtQty[i + 1].Rows[j]["Iss_Qty"].ToString()), 3);
                                    }

                                    if (Math.Round(Stock, 3) == 0)
                                    {
                                        Stock = Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                        Iss_Qty = Math.Round(Iss_Qty, 3) + Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                    }

                                    //while (Convert.ToDouble(Tot_Iss_Qty) > Convert.ToDouble(Iss_Qty) && Math.Round(Bal_Qty, 3) > 0)
                                    while (Math.Round(Bal_Qty, 3) > 0)
                                    {
                                        if (Math.Round(Stock, 3) == 0)
                                        {
                                            Stock = Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                            Iss_Qty = Math.Round(Iss_Qty, 3) + Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                        }

                                        if (Math.Round(Iss_Qty, 3) < Math.Round(Tot_Iss_Qty, 3) && Math.Round(Stock, 3) > Math.Round(Bal_Qty, 3))
                                        {
                                            Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_SampleWise_Transfer_Details (MasterID, SlNo, SlNo1, To_Order_No, Weight, BagNo, VSocks_Lot_Bag_Details_RowID) Values (" + Code + ", " + DtQty[i + 1].Rows[j]["Slno"].ToString() + ", " + DtQty[i + 1].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "', " + Dt1.Rows[l]["Stock"].ToString() + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ")";
                                            Queries[Array_Index++] = "Update A Set Transfer_In = Transfer_In + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                            Queries[Array_Index++] = "Update A Set Transfer_Out = Transfer_Out + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                            Stock = Math.Round(Stock, 3) - Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                            Bal_Qty = Math.Round(Bal_Qty, 3) - Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                        }
                                        else if ((Math.Round(Iss_Qty, 3) >= Math.Round(Tot_Iss_Qty, 3)) && (Math.Round(Bal_Qty, 3) > 0))
                                        {
                                            //if (Math.Round(Bal_Qty, 3) <= Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()),3))
                                            if (Math.Round(Bal_Qty, 3) <= Math.Round(Stock, 3))
                                            {
                                                Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_SampleWise_Transfer_Details (MasterID, SlNo, SlNo1, To_Order_No, Weight, BagNo, VSocks_Lot_Bag_Details_RowID) Values (" + Code + ", " + DtQty[i + 1].Rows[j]["Slno"].ToString() + ", " + DtQty[i + 1].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "', " + Math.Round(Bal_Qty, 3) + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ")";
                                                Queries[Array_Index++] = "Update A Set Transfer_In = Transfer_In + " + Math.Round(Bal_Qty, 3) + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                                Queries[Array_Index++] = "Update A Set Transfer_Out = Transfer_Out + " + Math.Round(Bal_Qty, 3) + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                                Stock = Math.Round(Stock, 3) - Math.Round(Convert.ToDouble(Bal_Qty.ToString()), 3);
                                                Bal_Qty = 0;
                                            }
                                            else if (Math.Round(Bal_Qty, 3) > Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3))
                                            {
                                                Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_SampleWise_Transfer_Details (MasterID, SlNo, SlNo1, To_Order_No, Weight, BagNo, VSocks_Lot_Bag_Details_RowID) Values (" + Code + ", " + DtQty[i + 1].Rows[j]["Slno"].ToString() + ", " + DtQty[i + 1].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "', " + Dt1.Rows[l]["Stock"].ToString() + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ")";
                                                Queries[Array_Index++] = "Update A Set Transfer_In = Transfer_In + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                                Queries[Array_Index++] = "Update A Set Transfer_Out = Transfer_Out + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                                Stock = Math.Round(Stock, 3) - Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                                Bal_Qty = Math.Round(Bal_Qty, 3) - Math.Round(Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()), 3);
                                            }
                                        }
                                        else if (Math.Round(Bal_Qty, 3) > Math.Round(Stock, 3))
                                        {
                                            Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_SampleWise_Transfer_Details (MasterID, SlNo, SlNo1, To_Order_No, Weight, BagNo, VSocks_Lot_Bag_Details_RowID) Values (" + Code + ", " + DtQty[i + 1].Rows[j]["Slno"].ToString() + ", " + DtQty[i + 1].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "', " + Math.Round(Stock, 3) + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ")";
                                            Queries[Array_Index++] = "Update A Set Transfer_In = Transfer_In + " + Math.Round(Stock, 3) + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i + 1].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                            Queries[Array_Index++] = "Update A Set Transfer_Out = Transfer_Out + " + Math.Round(Stock, 3) + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i].Value + " and A.Color_ID = " + Grid["ColorID", i].Value + " and A.Size_ID = " + Grid["SizeID", i].Value + "";
                                            Bal_Qty = Math.Round(Bal_Qty, 3) - Math.Round(Stock, 3);
                                            Stock = Math.Round(Stock, 3) - Math.Round(Stock, 3);
                                        }
                                        if (Math.Round(Stock, 3) == 0 && Math.Round(Bal_Qty, 3) > 0)
                                        {
                                            l = l + 1;
                                        }
                                    }
                                    if (Math.Round(Stock, 3) == 0 && Math.Round(Bal_Qty, 3) > 0)
                                    {
                                        l = l + 1;
                                    }
                                }
                            }
                        }
                    }
                }

                              
                //for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                //{
                //    for (i = 0; i <= DtQty.Length - 1; i++)
                //    {
                //        if (DtQty[i] != null)
                //        {
                //            for (int j = 0; j <= DtQty[i].Rows.Count - 1; j++)
                //            {
                //                if (MyParent._New)
                //                {
                //                    if (Convert.ToDouble(DtQty[i].Rows[j]["Iss_Qty"].ToString()) > 0)
                //                    {
                //                        DataTable Dt1 = new DataTable();

                //                        Str = " Select Grn_Date, Grn_No, LotNo, BagNo, Cur_Stock Stock, VSocks_Lot_Bag_Details_RowID From Socks_Store_Current_Stock() ";
                //                        Str = Str + " Where Order_No = '" + Grid["Order_No", i - 1].Value.ToString() + "' And ItemID = " + Grid["ItemID", i - 1].Value + " And ColorID = " + Grid["ColorID", i - 1].Value + " And SizeID = " + Grid["SizeID", i - 1].Value + " And LotNo = '" + Grid["LotNo", i - 1].Value + "' And Grn_No = '" + Grid["Grn_No", i - 1].Value + "' ";
                //                        Str = Str + " Order By Grn_Date, Grn_No, LotNo, BagNo ";

                //                        MyBase.Load_Data(Str, ref Dt1);
                //                        if (Dt1.Rows.Count > 0)
                //                        {
                //                            Double Tot_Iss_Qty = 0.000;
                //                            Double Iss_Qty = 0.000;
                //                            Double Bal_Qty = 0.000;
                //                            int l = 0;

                //                            Tot_Iss_Qty = Convert.ToDouble(DtQty[i].Rows[j]["Iss_Qty"].ToString());
                //                            Bal_Qty = Convert.ToDouble(DtQty[i].Rows[j]["Iss_Qty"].ToString());

                //                            while (Convert.ToDouble(Tot_Iss_Qty) > Convert.ToDouble(Iss_Qty) && Math.Round(Bal_Qty,3) > 0)
                //                            {
                //                                Iss_Qty = Iss_Qty + Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString());

                //                                if (Iss_Qty <= Tot_Iss_Qty)
                //                                {
                //                                    Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_SampleWise_Transfer_Details (MasterID, SlNo, SlNo1, To_Order_No, Weight, BagNo, VSocks_Lot_Bag_Details_RowID) Values (@@IDENTITY, " + DtQty[i].Rows[j]["Slno"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i].Rows[j]["Order_No"].ToString() + "', " + Dt1.Rows[l]["Stock"].ToString() + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ")";
                //                                    Queries[Array_Index++] = "Update A Set Transfer_In = Transfer_In + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and A.Size_ID = " + Grid["SizeID", i - 1].Value + "";
                //                                    Queries[Array_Index++] = "Update A Set Transfer_Out = Transfer_Out + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i - 1].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and A.Size_ID = " + Grid["SizeID", i - 1].Value + "";
                //                                    Bal_Qty = Bal_Qty - Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString());
                //                                }
                //                                else if ((Iss_Qty > Tot_Iss_Qty) && (Math.Round(Bal_Qty, 3) > 0))
                //                                {
                //                                    if (Bal_Qty <= Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()))
                //                                    {
                //                                        Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_SampleWise_Transfer_Details (MasterID, SlNo, SlNo1, To_Order_No, Weight, BagNo, VSocks_Lot_Bag_Details_RowID) Values (@@IDENTITY, " + DtQty[i].Rows[j]["Slno"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i].Rows[j]["Order_No"].ToString() + "', " + Bal_Qty + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ")";
                //                                        Queries[Array_Index++] = "Update A Set Transfer_In = Transfer_In + " + Bal_Qty + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and A.Size_ID = " + Grid["SizeID", i - 1].Value + "";
                //                                        Queries[Array_Index++] = "Update A Set Transfer_Out = Transfer_Out + " + Bal_Qty + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i - 1].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and A.Size_ID = " + Grid["SizeID", i - 1].Value + "";
                //                                        Bal_Qty = 0;

                //                                    }
                //                                    else if (Bal_Qty > Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()))
                //                                    {
                //                                        Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_SampleWise_Transfer_Details (MasterID, SlNo, SlNo1, To_Order_No, Weight, BagNo, VSocks_Lot_Bag_Details_RowID) Values (@@IDENTITY, " + DtQty[i].Rows[j]["Slno"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i].Rows[j]["Order_No"].ToString() + "', " + Dt1.Rows[l]["Stock"].ToString() + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ")";
                //                                        Queries[Array_Index++] = "Update A Set Transfer_In = Transfer_In + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and A.Size_ID = " + Grid["SizeID", i - 1].Value + "";
                //                                        Queries[Array_Index++] = "Update A Set Transfer_Out = Transfer_Out + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i - 1].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and A.Size_ID = " + Grid["SizeID", i - 1].Value + "";
                //                                        Bal_Qty = Bal_Qty - Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString());
                //                                    }
                //                                }

                //                                l = l + 1;
                //                            }
                //                        }
                //                    }
                //                }
                //                else
                //                {
                //                    if (Convert.ToDouble(DtQty[i].Rows[j]["Iss_Qty"].ToString()) > 0)
                //                    {
                //                        DataTable Dt1 = new DataTable();

                //                        Str = " Select Grn_Date, Grn_No, LotNo, BagNo, Stock, VSocks_Lot_Bag_Details_RowID From Socks_Store_Current_Stock_For_Transfer_Edit('" + Grid["Order_No", i - 1].Value.ToString() + "' , '" + DtQty[i].Rows[j]["Order_No"].ToString() + "', " + Grid["ItemID", i - 1].Value + ", " + Grid["ColorID", i - 1].Value + ", " + Grid["SizeID", i - 1].Value + ", '" + Grid["Grn_No", i - 1].Value.ToString() + "', '" + Grid["LotNO", i - 1].Value.ToString() + "')";
                                        
                //                        MyBase.Load_Data(Str, ref Dt1);
                //                        if (Dt1.Rows.Count > 0)
                //                        {
                //                            Double Tot_Iss_Qty = 0.000;
                //                            Double Iss_Qty = 0.000;
                //                            Double Bal_Qty = 0.000;
                //                            int l = 0;

                //                            Tot_Iss_Qty = Convert.ToDouble(DtQty[i].Rows[j]["Iss_Qty"].ToString());
                //                            Bal_Qty = Convert.ToDouble(DtQty[i].Rows[j]["Iss_Qty"].ToString());

                //                            while (Convert.ToDouble(Tot_Iss_Qty) >= Convert.ToDouble(Iss_Qty) && Math.Round(Bal_Qty,3) > 0)
                //                            {
                //                                Iss_Qty = Iss_Qty + Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString());

                //                                if (Iss_Qty <= Tot_Iss_Qty)
                //                                {
                //                                    Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_SampleWise_Transfer_Details (MasterID, SlNo, SlNo1, To_Order_No, Weight, BagNo, VSocks_Lot_Bag_Details_RowID) Values (" + Code + ", " + DtQty[i].Rows[j]["Slno"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i].Rows[j]["Order_No"].ToString() + "', " + Dt1.Rows[l]["Stock"].ToString() + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ")";
                //                                    Queries[Array_Index++] = "Update A Set Transfer_In = Transfer_In + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and A.Size_ID = " + Grid["SizeID", i - 1].Value + "";
                //                                    Queries[Array_Index++] = "Update A Set Transfer_Out = Transfer_Out + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i - 1].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and A.Size_ID = " + Grid["SizeID", i - 1].Value + "";
                //                                    Bal_Qty = Bal_Qty - Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString());
                //                                }
                //                                else if ((Iss_Qty > Tot_Iss_Qty) && (Math.Round(Bal_Qty, 3) > 0))
                //                                {
                //                                    if (Bal_Qty <= Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()))
                //                                    {
                //                                        Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_SampleWise_Transfer_Details (MasterID, SlNo, SlNo1, To_Order_No, Weight, BagNo, VSocks_Lot_Bag_Details_RowID) Values (" + Code + ", " + DtQty[i].Rows[j]["Slno"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i].Rows[j]["Order_No"].ToString() + "', " + Bal_Qty + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ")";
                //                                        Queries[Array_Index++] = "Update A Set Transfer_In = Transfer_In + " + Bal_Qty + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and A.Size_ID = " + Grid["SizeID", i - 1].Value + "";
                //                                        Queries[Array_Index++] = "Update A Set Transfer_Out = Transfer_Out + " + Bal_Qty + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i - 1].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and A.Size_ID = " + Grid["SizeID", i - 1].Value + "";
                //                                        Bal_Qty = 0;
                //                                    }
                //                                    else if (Bal_Qty > Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString()))
                //                                    {
                //                                        Queries[Array_Index++] = "Insert Into Socks_Store_Yarn_SampleWise_Transfer_Details (MasterID, SlNo, SlNo1, To_Order_No, Weight, BagNo, VSocks_Lot_Bag_Details_RowID) Values (" + Code + ", " + DtQty[i].Rows[j]["Slno"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ", '" + DtQty[i].Rows[j]["Order_No"].ToString() + "', " + Dt1.Rows[l]["Stock"].ToString() + ", " + Dt1.Rows[l]["BagNo"].ToString() + ", " + Dt1.Rows[l]["VSocks_Lot_Bag_Details_RowID"].ToString() + ")";
                //                                        Queries[Array_Index++] = "Update A Set Transfer_In = Transfer_In + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + DtQty[i].Rows[j]["Order_No"].ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and A.Size_ID = " + Grid["SizeID", i - 1].Value + "";
                //                                        Queries[Array_Index++] = "Update A Set Transfer_Out = Transfer_Out + " + Dt1.Rows[l]["Stock"].ToString() + " From Socks_Yarn_BOM_Status A Inner Join Socks_Order_Master B On A.Order_ID = B.RowID Where Order_No = '" + Grid["Order_No", i - 1].Value.ToString() + "' and A.Item_ID = " + Grid["ItemID", i - 1].Value + " and A.Color_ID = " + Grid["ColorID", i - 1].Value + " and A.Size_ID = " + Grid["SizeID", i - 1].Value + "";
                //                                        Bal_Qty = Bal_Qty - Convert.ToDouble(Dt1.Rows[l]["Stock"].ToString());
                //                                    }
                //                                }
                                                
                //                                l = l + 1;
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                
                if (MyParent._New)
                {
                    MyBase.Run_Identity(false, Queries);
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
                DtQty = new DataTable[30];

                Str = " Select A.TransferNo, A.Entryno, A.TransferDate, D.Item, E.Color, F.Size, B.From_Order_no, C.To_Order_No, SUM(Isnull(C.Weight,0)) Transfer_Qty, A.RowID, B.ItemID, B.ColorID, B.SizeID, B.Grn_No From Socks_Store_Yarn_Transfer_Master A ";
                Str = Str + " Left Join Socks_Store_Yarn_Transfer_Details B On A.RowID = B.MasterID Left Join Socks_Store_Yarn_SampleWise_Transfer_Details C On A.RowID = C.MasterID And B.SlNO1 = C.Slno1 ";
                Str = Str + " Left Join Item D On B.ItemID = D.ItemID Left Join Color E On B.ColorID = E.ColorID Left Join Size F On B.SizeID = F.SizeID ";
                Str = Str + " Group BY A.TransferNo, A.Entryno, A.TransferDate, D.Item, E.Color, F.Size, B.From_Order_no, C.To_Order_No, A.RowID, B.ItemID, B.ColorID, B.SizeID, B.Grn_No Order By A.TransferNo Desc, A.Entryno Desc ";

                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Store Yarn Stock Transfer Entry - Delete", Str, String.Empty, 120, 80, 100, 150, 140, 120, 120, 120, 100);
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
                    MyBase.Run("Exec Update_Socks_Yarn_BOM_Status_Trasfer_In " + Code, "Exec Update_Socks_Yarn_BOM_Status_Trasfer_Out " + Code, "Delete from Socks_Store_Yarn_SampleWise_Transfer_Details where MasterID = " + Code, "Delete from Socks_Store_Yarn_Transfer_Details where MasterID = " + Code, "Delete From Socks_Store_Yarn_Transfer_Master Where RowID = " + Code, MyParent.EntryLog("Socks_Store_Yarn_Transfer_Master", "DELETE", Code.ToString()));
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
                //String A;
                Code = Convert.ToInt64(Dr["RowID"]);
                TxtEntryNo.Text = Dr["EntryNo"].ToString();
                A = Dr["TransferNo"].ToString();
                TxtItem.Text = Dr["Item"].ToString();
                TxtItem.Tag = Dr["ItemID"].ToString();
                TxtColor.Text = Dr["Color"].ToString();
                TxtColor.Tag = Dr["ColorID"].ToString();
                TxtSize.Text = Dr["Size"].ToString();
                TxtSize.Tag = Dr["SizeID"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["TransferDate"]);                                
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
                    Str = "Select 0 as Slno, ''Order_No, '' LotNo, '' Grn_No,  Item,  Color, Size, 0.000 Stock_Qty, 0.000 Iss_Qty, Itemid, Colorid, Sizeid,0 Slno1, '' Detail, '-' T  from FITSOCKS.dbo.Yarn_Dyeing_Requirement_Details() where 1=2";
                }
                else
                {
                    Str = " Select B.SlNo, B.From_Order_no Order_No, B.LotNo, B.Grn_No, D.Item, E.Color, F.Size, (Isnull(G.Cur_Stock_LotWise,0) + Isnull(B.TransferQty,0))Stock_Qty, ";
                    Str = Str + " B.TransferQty Iss_Qty, B.ItemID, B.ColorID, B.SizeID, B.SlNO1, Cast(B.From_Order_No As Varchar(20))+Cast(B.Grn_No As Varchar(50))+Cast(B.ItemID As Varchar(20))+Cast(B.ColorID As Varchar(20))+Cast(B.SizeID As Varchar(20)) Detail, ''T ";
                    Str = Str + " From Socks_Store_Yarn_Transfer_Master A Left Join Socks_Store_Yarn_Transfer_Details B On A.RowID = B.MasterID ";
                    Str = Str + " Left Join Item D On B.ItemID = D.ItemID Left Join Color E On B.ColorID = E.ColorID Left Join Size F On B.SizeID = F.SizeID ";
                    Str = Str + " Left Join Socks_Store_Current_Stock_LotWise() G On B.Grn_No = G.Grn_No And B.From_Order_no = G.Order_No And B.ItemID = G.ItemID And B.ColorID = G.ColorID And B.SizeID = G.SizeID ";
                    Str = Str + " Where A.Rowid = " + Code + " Order By B.Slno ";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Detail", "ItemID", "SizeID", "ColorID", "Slno1", "T");
                
                MyBase.ReadOnly_Grid_Without(ref Grid, "Order_No", "Iss_Qty");
                MyBase.Grid_Width(ref Grid, 50, 120, 120, 120, 150, 150, 90, 100, 100);
                Grid.Columns["Stock_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Iss_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (!MyParent._New)
                    {
                        Vis = 1;
                        Pos = i;
                        GridDetail_Data(Convert.ToInt16(Grid["Slno1", i].Value), Convert.ToInt64(Grid["ItemID", i].Value), Convert.ToInt64(Grid["ColorID", i].Value), Convert.ToInt64(Grid["SizeID", i].Value));
                        Vis = 0;
                        Pos = 0;
                    }
                }
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

        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                // Due To Fill Bom Function For Transfer, Bal Qty Updation in Text box's
                //if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                //{
                //    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                //    {
                //        listBox1.Items.Add(Grid["StockID", Grid.CurrentCell.RowIndex].Value.ToString());
                //        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                //        //Dt.AcceptChanges(); 
                //    }
                //}
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
                    Txt.Leave += new EventHandler(Txt_Leave);
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Item"].Index)
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
                        if (ChkClosed.Checked == true)
                        {
                            if (MyParent.UserCode != 11 && MyParent.UserCode != 39)
                            {
                                if (TxtItem.Text.ToString().Trim() == String.Empty || TxtColor.Text.ToString().Trim() == String.Empty || TxtSize.Text.ToString().Trim() == String.Empty)
                                {
                                    MessageBox.Show("Pls Select Item, Color, Size..!Gainup");
                                    TxtItem.Focus();
                                    return;
                                }
                            }
                            if (MyParent.UserCode == 1 || MyParent.UserCode == 19)
                            {
                                Str = " Select A.Order_No, A.Lotno, A.Grn_No, C.Item, D.Color, E.Size, Sum(Cur_Stock)Stock_Qty, 0 Iss_Qty, A.ItemID, A.ColorID, A.SizeID, ";
                                Str = Str + " Cast(A.Order_No As Varchar(20))+Cast(A.Lotno As Varchar(20))+CAst(A.Grn_No As Varchar(50))+Cast(A.ItemID As Varchar(20))+Cast(A.ColorID As Varchar(20))+Cast(A.SizeID As Varchar(20)) Detail, ";
                                Str = Str + " 0 MerchandiserID From Socks_Store_Current_Stock() A Left Join (Select Distinct Order_No, Buyer From Socks_Bom()Where Isnull(Despatch_Closed, 'N') = 'Y' Union Select Distinct Order_No, 'DECATHLON - FRANCE' Buyer From Buy_Ord_Style Where Isnull(Despatch_Closed, 'N') = 'Y' And Order_No Like '%MOQ%' Union Select A.Order_No, C.Buyer From Buy_ord_Style A Left Join Buy_Ord_mas B On A.order_no = B.Order_No Left Join Buyer C On B.Buyerid = C.Buyerid Where A.Order_No Not Like '%MOQ%' And A.Despatch_Closed = 'Y')B On A.Order_No = B.Order_No ";
                                Str = Str + " Left Join Item C On A.ItemID = C.ItemID Left Join Color D On A.ColorID = D.ColorID Left Join Size E on A.SizeID = E.SizeID Where B.Order_No Is Not Null ";
                                //if (MyParent.UserCode == 19)
                                //{
                                //    Str = Str + " And A.Order_No Like '%OCN%' And B.Buyer Not Like '%DECATHLON%'";
                                //}
                                //else if (MyParent.UserCode == 11 || MyParent.UserCode == 39)
                                //{
                                //    Str = Str + " And B.Buyer Like '%DECATHLON%'";
                                //}
                                //if (MyParent.UserCode != 11 && MyParent.UserCode != 39)
                                //{
                                    Str = Str + " And A.ItemID = " + TxtItem.Tag + " And A.ColorID = " + TxtColor.Tag + " And A.SizeID = " + TxtSize.Tag + "";
                                //}
                                Str = Str + " Group BY A.Order_No, A.Lotno, A.Grn_No, C.Item, D.Color, E.Size, A.ItemID, A.ColorID, A.SizeID, ";
                                Str = Str + " Cast(A.Order_No As Varchar(20))+Cast(A.Lotno As Varchar(20))+CAst(A.Grn_No As Varchar(50))+Cast(A.ItemID As Varchar(20))+Cast(A.ColorID As Varchar(20))+Cast(A.SizeID As Varchar(20)) ";
                                Str = Str + " Having Sum(Cur_Stock) > 0 Order By C.Item, D.Color, E.Size, A.Order_No ";

                                Dr = Tool.Selection_Tool_Except_New("Detail", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Order_No", Str, String.Empty, 102, 102, 128, 128, 75, 75, 50);
                            }
                        }
                        else
                        {
                            Str = " Select Distinct A.Order_No, A.LotNo, A.Grn_No, A.Item, A.Color, A.Size, ISNULL(A.Stock,0)Stock_Qty, 0 Iss_Qty, A.ItemID, A.ColorID, A.SizeID, ";
                            Str = Str + " Cast(A.Order_No As Varchar(20))+Cast(A.LotNO As Varchar(20))+CAst(A.Grn_No As Varchar(50))+Cast(A.ItemID As Varchar(20))+Cast(A.ColorID As Varchar(20))+Cast(A.SizeID As Varchar(20)) Detail";
                            if (MyParent.UserCode == 1 || MyParent.UserCode == 19)
                            {
                                Str = Str + " ,0 MerchandiserID From Socks_Store_Lot_For_Transfer_Admin() A";
                            }
                            else
                            {
                                Str = Str + ", A.MerchandiserID From Socks_Store_Lot_For_Transfer_Merchandiser() A Left Join (Select Order_No, ItemID, ColorID, SizeID, SUM(Isnull(Knit_Req_Qty,0))Knit_Req_Qty, Print_out_taken From VSocks_Samplewise_All_Transfer() Group By Order_No, ItemID, ColorID, SizeID, Print_out_taken)B On A.Order_No = B.Order_No And A.ItemID = B.ItemID And A.ColorID = B.ColorID And A.SizeID = B.SizeID Where (Isnull(B.Print_Out_Taken,'N')= 'N' Or Isnull(B.Knit_Req_Qty,0) < ISNULL(A.Stock,0))";
                            }
                            if (MyParent.UserCode == 19) //19 - Gopal
                            {
                                if (TxtItem.Text.ToString() != String.Empty && TxtColor.Text.ToString() != String.Empty && TxtSize.Text.ToString() != String.Empty)
                                {
                                    Str = Str + " Where A.ItemID = " + TxtItem.Tag.ToString() + " And A.ColorID = " + TxtColor.Tag.ToString() + " And A.SizeID = " + TxtSize.Tag.ToString() + " And A.Order_No Like '%GENERAL%'";
                                }
                                else
                                {
                                    MessageBox.Show("Select Item , Color And Size ", "Gainup");
                                    TxtItem.Focus();
                                }
                            }
                            else if (MyParent.UserCode == 11 || MyParent.UserCode == 39) //11 - HariRam //39 - Ramkumar
                            {
                                Str = Str + " And A.MerchandiserID in (90, 104)";
                            }
                            else if (MyParent.UserCode == 15) //15 - Alagupandi
                            {
                                Str = Str + " And A.MerchandiserID in (89)";
                            }
                            else if (MyParent.UserCode == 12) //12 - Manikumar
                            {
                                Str = Str + " And A.MerchandiserID in (94)";
                            }
                            else if (MyParent.UserCode == 13) //13 - Naresh
                            {
                                Str = Str + " And A.MerchandiserID in (95)";
                            }
                            else
                            {
                                if (TxtItem.Text.ToString() != String.Empty && TxtColor.Text.ToString() != String.Empty && TxtSize.Text.ToString() != String.Empty)
                                {
                                    Str = Str + " Where A.ItemID = " + TxtItem.Tag.ToString() + " And A.ColorID = " + TxtColor.Tag.ToString() + " And A.SizeID = " + TxtSize.Tag.ToString() + " ";
                                }
                                else
                                {
                                    MessageBox.Show("Select Item , Color And Size ", "Gainup");
                                    TxtItem.Focus();
                                }
                            }

                            Dr = Tool.Selection_Tool_Except_New("Detail", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Order_No", Str, String.Empty, 102, 102, 128, 128, 75, 75, 50);
                        }
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Order_No"].ToString();
                            Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["ITEM"].ToString();
                            Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                            Grid["LotNo", Grid.CurrentCell.RowIndex].Value = Dr["LotNo"].ToString();
                            Grid["Grn_No", Grid.CurrentCell.RowIndex].Value = Dr["Grn_No"].ToString();
                            Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["SIZE"].ToString();
                            Grid["Color", Grid.CurrentCell.RowIndex].Value = Dr["COLOR"].ToString();
                            Grid["Stock_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Stock_Qty"].ToString();
                            Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Iss_Qty"].ToString();
                            Grid["Detail", Grid.CurrentCell.RowIndex].Value = Dr["Detail"].ToString();
                            Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                            Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                            Grid["ColorID", Grid.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();
                            Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                            
                            if (MyParent.UserCode == 11 || MyParent.UserCode == 39)
                            {
                                TxtItem.Text = Dr["Item"].ToString();
                                TxtItem.Tag = Dr["Itemid"].ToString();
                                TxtColor.Text = Dr["Color"].ToString();
                                TxtColor.Tag = Dr["Colorid"].ToString();
                                TxtSize.Text = Dr["Size"].ToString();
                                TxtSize.Tag = Dr["Sizeid"].ToString();
                            }
                        }
                        else
                        {
                            if (MyParent.UserCode == 11 || MyParent.UserCode == 39)
                            {
                                MessageBox.Show("Stock Not Availabe", "Gainup");
                                TxtItem.Focus();
                            }
                            else
                            {
                                if (TxtItem.ToString() == String.Empty || TxtColor.ToString() == String.Empty || TxtSize.ToString() == String.Empty)
                                {
                                    MessageBox.Show("Select Item , Color And Size ", "Gainup");
                                    TxtItem.Focus();
                                }
                            }
                        }
                    }

                    Total_Count();    
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

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Iss_Qty"].Index)
                {
                    MyBase.Valid_Null(Txt, e);
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

        void Txt_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Iss_Qty"].Index)
                {
                    if ((Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) != String.Empty)
                    {
                        if (Convert.ToDouble(Grid["Stock_Qty", Grid.CurrentCell.RowIndex].Value) < Convert.ToDouble(Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value))
                        {
                            MessageBox.Show("Invalid Iss_Qty..!", "Gainup");
                            Grid.CurrentCell = Grid["Iss_Qty", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
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

        void TxtIss_Leave(object sender, EventArgs e)
        {
            try
            {   
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Order_No"].Index)
                {
                    LblBal.Text = "0";
                    LblReq.Text = "0";
                    LblTfr.Text = "0";
                    GridDetail.Refresh();
                    Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString()); 
                    if ((GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString()) != String.Empty)
                    {   
                        if(Convert.ToDouble(LblBal.Text.ToString()) < 0)
                        {
                            MessageBox.Show("Invalid Iss_Qty..!", "Gainup");
                            GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value = "0.000";
                            GridDetail.CurrentCell = GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                }
                else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Iss_Qty"].Index)
                {
                    if (Convert.ToDouble(LblBal.Text.ToString()) < 0)
                    {
                        MessageBox.Show("Invalid Iss_Qty..!", "Gainup");
                        GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value = "0.000";
                        GridDetail.CurrentCell = GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex];
                        GridDetail.Focus();
                        GridDetail.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                    LblBal.Text = "0";
                    LblReq.Text = "0";
                    LblTfr.Text = "0";
                    if (GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                    }
                    if ((GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString()) != String.Empty)
                    {   
                        if(Convert.ToDouble(LblBal.Text.ToString()) < 0)
                        {
                            MessageBox.Show("Invalid Iss_Qty..!", "Gainup");
                            GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value = "0.000";
                            GridDetail.CurrentCell = GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            MyParent.Save_Error = true;
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

        void Total_Count()
        {
            try
            {
                TxtTotal.Text = MyBase.Sum_With_Three_Digits(ref Grid, "Iss_Qty");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        void GridDetail_Data(Int32 Row, Int64 ItemID_Detail, Int64 ColorID_Detail, Int64 SizeID_Detail)
        {
            try
            {
                ItemID = ItemID_Detail;
                ColorID = ColorID_Detail;
                SizeID = SizeID_Detail;

                LblReq.Text = "0";
                LblTfr.Text = "0";
                LblBal.Text = "0";
                if (DtQty[Row] == null)
                {
                    DtQty[Row] = new DataTable();
                    if (MyParent._New)
                    {
                        MyBase.Load_Data("select 0 SlNo, '0' Order_No, 0.000 Iss_Qty," + Row + " SlNo1, 0.000 Iss_Qty_old, 0 ItemID, 0 ColorID, 0 SizeID, '' T from Yarn_Dyeing_Requirement_Details() where 1=2 ", ref DtQty[Row]);
                    }
                    else
                    {
                        if (MyParent.Edit && Vis == 1)
                        {
                            MyBase.Load_Data("Select A.Slno, A.To_Order_No Order_no, Sum(A.Weight) Iss_Qty, A.Slno1, Sum(A.Weight) Iss_Qty_Old, B.ItemID, B.ColorID, B.SizeID, '' T From Socks_Store_Yarn_SampleWise_Transfer_Details A Inner Join Socks_Store_Yarn_Transfer_Details B On A.MasterID = B.MasterID And A.Slno1 = B.SlNO1 Where A.MasterID = " + Code + " And A.Slno1 = " + Grid["Slno1", Pos].Value.ToString() + " And B.MasterID = " + Code + " And B.Slno1 = " + Grid["Slno1", Pos].Value.ToString() + " Group By A.Slno, A.To_Order_No, A.Slno1, B.ItemID, B.ColorID, B.SizeID ", ref DtQty[Row]);
                        }
                        else
                        {
                            MyBase.Load_Data("Select A.Slno, A.To_Order_No Order_no, Sum(A.Weight) Iss_Qty, A.Slno1, Sum(A.Weight) Iss_Qty_Old, B.ItemID, B.ColorID, B.SizeID, '' T From Socks_Store_Yarn_SampleWise_Transfer_Details A Left Join Socks_Store_Yarn_Transfer_Details B On A.MasterID = B.MasterID And A.Slno1 = B.SlNO1 Where A.MasterID = " + Code + " And A.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() + " And B.MasterID = " + Code + " And B.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() + " Group By A.Slno, A.To_Order_No, A.Slno1, B.ItemID, B.ColorID, B.SizeID ", ref DtQty[Row]);
                        }
                    }
                }
                GridDetail.DataSource = DtQty[Row];
                MyBase.Grid_Designing(ref GridDetail, ref DtQty[Row],"Iss_Qty_Old", "SlNo1", "ItemID", "ColorID", "SizeID", "T");
                MyBase.ReadOnly_Grid_Without(ref GridDetail, "Iss_Qty", "Order_No");
                MyBase.Grid_Colouring(ref GridDetail, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridDetail, 50, 150, 100);
                GridDetail.Columns["Iss_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                GridDetail.RowHeadersWidth = 10;

                if (!MyParent._New)
                {
                    //Balance_Pieces();
                }

                if (!MyParent._New && Vis == 1)
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
        private void GridDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {   
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Iss_Qty"].Index)
                    {
                        if (GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString() != String.Empty && (GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value) == 0))
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Iss_Qty...!", "Gainup");
                            if (GridDetail.Rows.Count <= 2)
                            {
                                GridDetail.CurrentCell = GridDetail["Iss_Qty", 0];
                            }
                            else
                            {
                                GridDetail.CurrentCell = GridDetail["Iss_Qty", Grid.CurrentCell.RowIndex];
                            }
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                            if (Convert.ToDouble(LblBal.Text.ToString()) < 0)
                            {
                                e.Handled = true;
                                MessageBox.Show("Invalid Iss_Qty...!", "Gainup");
                                GridDetail.CurrentCell = GridDetail["Iss_Qty", Grid.CurrentCell.RowIndex];
                                GridDetail["Iss_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                                GridDetail.Focus();
                                GridDetail.BeginEdit(true);
                                return;
                            }
                        }
                    }
                   
                }
                Iss_Balance();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Double Fill_BOM_Check(String Order_No1)
        {
            try
            {
                Double Tfr = 0;
                Double Req = 0;
                Double Bal = 0;

                String Order_No = Order_No1;

                DataTable Tdt1 = new DataTable();
                //MyBase.Load_Data("Select A.Order_No, A.Bal_Qty Req_Qty from fitsocks.dbo.Socks_Trans_Knit_Req_Orderwise()A Left Join Buy_Ord_Mas B on A.Order_No = B.Order_No where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + "  And A.Order_No = '" + OrderNo + "' ", ref Tdt);

                if (ChkClosed.Checked == true)
                {
                    MyBase.Load_Data("Select 'GENERAL' Order_No, 10000 Req_Qty ", ref Tdt1);

                    //DataTable Dt5 = new DataTable();
                    //if(Order_No.Contains("MOQ"))
                    //{
                    //    MyBase.Load_Data("Select A.Order_No, A.Bal_Qty Req_Qty from fitsocks.dbo.Socks_Store_Transfer_knit_Req_Orderwise()A Where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No = '" + Order_No + "'", ref Tdt1);
                    //    if (Tdt1.Rows.Count == 0)
                    //    {
                    //        MyBase.Load_Data("Select 'GENERAL' Order_No, 10000 Req_Qty ", ref Tdt1);
                    //    }
                    //}
                    //else
                    //{
                    //    MyBase.Load_Data(" Select Distinct Buyer From Socks_Bom() Where Order_No = '" + Order_No + "'", ref Dt5);
                    //    if (Dt5.Rows.Count > 0)
                    //    {
                    //        if (Dt5.Rows[0][0].ToString().Trim() == "DECATHLON - FRANCE" || Dt5.Rows[0][0].ToString().Trim() == "Decathlon Sa, France")
                    //        {
                    //            MyBase.Load_Data("Select A.Order_No, A.Bal_Qty Req_Qty from fitsocks.dbo.Socks_Store_Transfer_knit_Req_Orderwise()A Where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No = '" + Order_No + "'", ref Tdt1);
                    //            if (Tdt1.Rows.Count == 0)
                    //            {
                    //                MyBase.Load_Data("Select 'GENERAL' Order_No, 10000 Req_Qty ", ref Tdt1);
                    //            }
                    //        }
                    //        else if (Dt5.Rows[0][0].ToString().Trim() != "DECATHLON - FRANCE" && Dt5.Rows[0][0].ToString().Trim() != "Decathlon Sa, France")
                    //        {
                    //            MyBase.Load_Data("Select 'GENERAL' Order_No, 10000 Req_Qty ", ref Tdt1);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        MyBase.Load_Data("Select 'GENERAL' Order_No, 10000 Req_Qty ", ref Tdt1);
                    //    }
                    //}
                }
                else
                {
                    if (MyParent._New)
                    {
                        MyBase.Load_Data("Select A.Order_No, A.Bal_Qty Req_Qty from fitsocks.dbo.Socks_Store_Transfer_knit_Req_Orderwise()A Where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No = '" + Order_No + "'", ref Tdt1);
                    }
                    else
                    {
                        MyBase.Load_Data("Select A.Order_No, A.Bal_Qty Req_Qty from fitsocks.dbo.Socks_Store_Transfer_knit_Req_Orderwise_Edit(" + Code + ")A Where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No = '" + Order_No + "'", ref Tdt1);
                    }
                }
                if (Tdt1.Rows.Count > 0)
                {
                    Req = Math.Round(Convert.ToDouble(Tdt1.Rows[0]["Req_Qty"].ToString()),3);
                }

                for (int i = 1; i <= Dt.Rows.Count; i++)
                {
                    if (DtQty[i] != null)
                    {
                        for (int j = 0; j <= DtQty[i].Rows.Count - 1; j++)
                        {
                            if (DtQty[i].Rows[j]["Order_No"].ToString() == Order_No && Convert.ToInt64(DtQty[i].Rows[j]["ItemID"].ToString()) == Convert.ToInt64(ItemID.ToString()) && Convert.ToInt64(DtQty[i].Rows[j]["ColorID"].ToString()) == Convert.ToInt64(ColorID.ToString()) && Convert.ToInt64(DtQty[i].Rows[j]["SizeID"].ToString()) == Convert.ToInt64(SizeID.ToString()))
                            {
                                {
                                    Tfr = Math.Round(Convert.ToDouble(Tfr),3) + Math.Round(Convert.ToDouble(DtQty[i].Rows[j]["Iss_Qty"]),3);
                                }
                            }
                        }
                    }
                }

                Bal = Math.Round(Convert.ToDouble(Req),3) - Math.Round(Convert.ToDouble(Tfr),3);

                return Math.Round(Bal,3);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Fill_BOM( String OrderNo)
        {
            try
            {
                LblTfr.Text = "0";
                LblReq.Text = "0";
                LblBal.Text="0";
                DataTable Tdt = new DataTable();
                //MyBase.Load_Data("Select A.Order_No, A.Bal_Qty Req_Qty from fitsocks.dbo.Socks_Trans_Knit_Req_Orderwise()A Left Join Buy_Ord_Mas B on A.Order_No = B.Order_No where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + "  And A.Order_No = '" + OrderNo + "' ", ref Tdt);

                if (ChkClosed.Checked == true)
                {
                    MyBase.Load_Data("Select 'GENERAL' Order_No, 10000 Req_Qty ", ref Tdt);
                    //DataTable Dt5 = new DataTable();
                    //if (Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString().Contains("MOQ"))
                    //{
                    //    MyBase.Load_Data("Select A.Order_No, A.Bal_Qty Req_Qty from fitsocks.dbo.Socks_Store_Transfer_knit_Req_Orderwise()A Where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No = '" + OrderNo + "'", ref Tdt);
                    //    if (Tdt.Rows.Count == 0)
                    //    {
                    //        MyBase.Load_Data("Select 'GENERAL' Order_No, 10000 Req_Qty ", ref Tdt);
                    //    }
                    //}
                    //else
                    //{
                    //    MyBase.Load_Data(" Select Distinct Buyer From Socks_Bom() Where Order_No like '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "'", ref Dt5);
                    //    if (Dt5.Rows.Count > 0)
                    //    {
                    //        if (Dt5.Rows[0][0].ToString().Trim() == "DECATHLON - FRANCE" || Dt5.Rows[0][0].ToString().Trim() == "Decathlon Sa, France")
                    //        {
                    //            MyBase.Load_Data("Select A.Order_No, A.Bal_Qty Req_Qty from fitsocks.dbo.Socks_Store_Transfer_knit_Req_Orderwise()A Where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No = '" + OrderNo + "'", ref Tdt);
                    //            if (Tdt.Rows.Count == 0)
                    //            {
                    //                MyBase.Load_Data("Select 'GENERAL' Order_No, 10000 Req_Qty ", ref Tdt);
                    //            }
                    //        }
                    //        else if (Dt5.Rows[0][0].ToString().Trim() != "DECATHLON - FRANCE" && Dt5.Rows[0][0].ToString().Trim() != "Decathlon Sa, France")
                    //        {
                    //            MyBase.Load_Data("Select 'GENERAL' Order_No, 10000 Req_Qty ", ref Tdt);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        MyBase.Load_Data("Select A.Order_No, A.Bal_Qty Req_Qty from fitsocks.dbo.Socks_Store_Transfer_knit_Req_Orderwise()A Where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No = '" + OrderNo + "'", ref Tdt);
                    //        if (Tdt.Rows.Count == 0)
                    //        {
                    //            MyBase.Load_Data("Select 'GENERAL' Order_No, 10000 Req_Qty ", ref Tdt);
                    //        }
                    //    }
                    //}
                }
                else
                {
                    if (MyParent._New)
                    {
                        MyBase.Load_Data("Select A.Order_No, A.Bal_Qty Req_Qty from fitsocks.dbo.Socks_Store_Transfer_knit_Req_Orderwise()A Where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No = '" + OrderNo + "'", ref Tdt);
                    }
                    else
                    {
                        MyBase.Load_Data("Select A.Order_No, A.Bal_Qty Req_Qty from fitsocks.dbo.Socks_Store_Transfer_knit_Req_Orderwise_Edit(" + Code + ")A Where A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No = '" + OrderNo + "'", ref Tdt);
                    }
                }
                if (Tdt.Rows.Count > 0)
                {
                    LblReq.Text = Tdt.Rows[0]["Req_Qty"].ToString();
                }

                for (int i = 1; i <= Dt.Rows.Count; i++)
                {
                    if (DtQty[i] != null)
                    {
                        for (int j = 0; j <= DtQty[i].Rows.Count - 1; j++)
                        {
                            if (DtQty[i].Rows[j]["Order_No"].ToString() == OrderNo && Convert.ToInt64(DtQty[i].Rows[j]["ItemID"].ToString()) == Convert.ToInt64(ItemID.ToString()) && Convert.ToInt64(DtQty[i].Rows[j]["ColorID"].ToString()) == Convert.ToInt64(ColorID.ToString()) && Convert.ToInt64(DtQty[i].Rows[j]["SizeID"].ToString()) == Convert.ToInt64(SizeID.ToString()))
                            {
                                //if (Grid.CurrentCell.RowIndex != i - 1)
                                {
                                    LblTfr.Text = String.Format("{0:0.000}", Convert.ToDouble(LblTfr.Text) + Convert.ToDouble(DtQty[i].Rows[j]["Iss_Qty"]));
                                }
                            }
                        }
                    }
                }
                
                LblBal.Text = String.Format("{0:0.000}", Convert.ToDouble(LblReq.Text) - Convert.ToDouble(LblTfr.Text));
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void TxtIss_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Order_No"].Index)
                    {
                        if (ChkClosed.Checked == true)
                        {
                            //if (MyParent.UserCode == 1 || MyParent.UserCode == 19 || MyParent.UserCode == 11 || MyParent.UserCode == 39)
                            //{
                            //    DataTable Dt5 = new DataTable();
                            //    if (Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString().Contains("MOQ"))
                            //    {
                            //        Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Order_No, Bal_Qty, Req_Qty, Rec_Qty, Iss_Qty, ItemID, ColorID, SizeID From Closed_Order_Stock_To_Moved(" + ItemID + ", " + ColorID + ", " + SizeID + ") Where Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "'", String.Empty, 150, 100);
                            //    }
                            //    else
                            //    {
                            //        MyBase.Load_Data(" Select Distinct Buyer From Socks_Bom() Where Order_No like '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "'", ref Dt5);
                            //        if (Dt5.Rows.Count > 0)
                            //        {
                            //            if (Dt5.Rows[0][0].ToString().Trim() == "DECATHLON - FRANCE" || Dt5.Rows[0][0].ToString().Trim() == "Decathlon Sa, France")
                            //            {
                            //                DataTable St1 = new DataTable();
                            //                MyBase.Load_Data("Select Order_No, Bal_Qty, Req_Qty, Rec_Qty, Iss_Qty, ItemID, ColorID, SizeID From Closed_Order_Stock_To_Moved(" + ItemID + ", " + ColorID + ", " + SizeID + ") Where Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' And Order_No Like '%MOQ%'", ref St1);
                            //                if (St1.Rows.Count != 0)
                            //                {
                            //                    Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Order_No, Bal_Qty, Req_Qty, Rec_Qty, Iss_Qty, ItemID, ColorID, SizeID From Closed_Order_Stock_To_Moved(" + ItemID + ", " + ColorID + ", " + SizeID + ") Where Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' And Order_No Like '%MOQ%'", String.Empty, 150, 100);
                            //                }
                            //                else
                            //                {
                            //                    Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Order_No, Bal_Qty, Req_Qty, Rec_Qty, Iss_Qty, ItemID, ColorID, SizeID From Closed_Order_Stock_To_Moved(" + ItemID + ", " + ColorID + ", " + SizeID + ") Where Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "'", String.Empty, 150, 100);
                            //                }

                            //                //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Distinct A.Order_No, A.Bal_Qty, A.Req_Qty, A.Rec_Qty, 0.000 Iss_Qty, A.ItemID, A.ColorID, A.SizeID From Socks_Store_Transfer_knit_Req_Orderwise()A Left Join buy_ord_Style B On A.Order_No = B.Order_No Where Isnull(B.Despatch_Closed, 'N') = 'N' And A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No Like '%MOQ%' And A.Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' ", String.Empty, 150, 100);
                            //                //if (Dr == null)
                            //                //{
                            //                //    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select 'GENERAL' Order_No, 10000 Bal_Qty, 10000 Req_Qty, 0 Rec_Qty, 0.000 Iss_Qty, " + ItemID + " ItemID,  " + ColorID + " ColorID, " + SizeID + " SizeID ", String.Empty, 150, 100);
                            //                //}
                            //            }
                            //            else if (Dt5.Rows[0][0].ToString().Trim() != "DECATHLON - FRANCE" && Dt5.Rows[0][0].ToString().Trim() != "Decathlon Sa, France")
                            //            {
                            //                //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select 'GENERAL' Order_No, 10000 Bal_Qty, 10000 Req_Qty, 0 Rec_Qty, 0.000 Iss_Qty, " + ItemID + " ItemID,  " + ColorID + " ColorID, " + SizeID + " SizeID ", String.Empty, 150, 100);
                            //                Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Order_No, Bal_Qty, Req_Qty, Rec_Qty, Iss_Qty, ItemID, ColorID, SizeID From Closed_Order_Stock_To_Moved(" + ItemID + ", " + ColorID + ", " + SizeID + ") Where Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "'", String.Empty, 150, 100);
                            //            }
                            //        }
                            //        else
                            //        {
                            //            Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Order_No, Bal_Qty, Req_Qty, Rec_Qty, Iss_Qty, ItemID, ColorID, SizeID From Closed_Order_Stock_To_Moved(" + ItemID + ", " + ColorID + ", " + SizeID + ") Where Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "'", String.Empty, 150, 100);
                            //        }
                            //    }
                            //}

                            if (MyParent.UserCode == 1 || MyParent.UserCode == 19)
                            {
                                DataTable Dt5 = new DataTable();
                                
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select 'GENERAL' Order_No, 10000 Bal_Qty, 10000 Req_Qty, 0 Rec_Qty, 0.000 Iss_Qty, " + ItemID + " ItemID,  " + ColorID + " ColorID, " + SizeID + " SizeID ", String.Empty, 150, 100);                                
                            }
                        }
                        else
                        {
                            if (MyParent.UserCode == 19)
                            {
                                Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Distinct A.Order_No, A.Bal_Qty, A.Req_Qty, A.Rec_Qty, 0.000 Iss_Qty, A.ItemID, A.ColorID, A.SizeID From Socks_Store_Transfer_knit_Req_Orderwise()A Left Join (Select Order_No, Despatch_Closed From buy_ord_Style Union All Select Order_No, Despatch_Closed From Socks_Order_Master) B On A.Order_No = B.Order_No Where A.Order_no Like '%OCN%' And Isnull(B.Despatch_Closed, 'N') = 'N' And A.Itemid = " + ItemID + "  and A.Colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No! = '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' ", String.Empty, 150, 100);
                            }
                            else if (MyParent.UserCode == 11 || MyParent.UserCode == 39)
                            {
                                if (ChkOtherMerchandiser.Checked == true)
                                {
                                    Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Distinct A.Order_No, A.Bal_Qty, A.Req_Qty, A.Rec_Qty, 0.000 Iss_Qty, A.ItemID, A.ColorID, A.SizeID From Socks_Store_Transfer_knit_Req_Orderwise()A Left Join (Select Order_No, Despatch_Closed From buy_ord_Style Union All Select Order_No, Despatch_Closed From Socks_Order_Master) B On A.Order_No = B.Order_No Where Isnull(B.Despatch_Closed, 'N') = 'N' And A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' And A.Buyerid Not In(100, 5275, 5465)", String.Empty, 150, 100);
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Distinct Order_No, Bal_Qty, Req_Qty, Rec_Qty, Iss_Qty, ItemID, ColorID, SizeID From Socks_Store_Transfer_knit_Req_Orderwise_Dec()Where itemid = " + ItemID + "  and colorid = " + ColorID + " and sizeid = " + SizeID + " And Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' And Order_No Like '%OCN%'", String.Empty, 150, 100, 100, 100);
                                }
                            }
                            else if (MyParent.UserCode == 1)
                            {
                                Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Distinct A.Order_No, A.Bal_Qty, A.Req_Qty, A.Rec_Qty, 0.000 Iss_Qty, A.ItemID, A.ColorID, A.SizeID From Socks_Store_Transfer_knit_Req_Orderwise()A Left Join (Select Order_No, Despatch_Closed From buy_ord_Style Union All Select Order_No, Despatch_Closed From Socks_Order_Master) B On A.Order_No = B.Order_No Where Isnull(B.Despatch_Closed, 'N') = 'N' And A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' ", String.Empty, 150, 100);
                            }
                            else if (MyParent.UserCode == 15)
                            {
                                if (ChkOtherMerchandiser.Checked == true)
                                {
                                    Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Distinct A.Order_No, A.Bal_Qty, A.Req_Qty, A.Rec_Qty, A.Bal_Qty, 0.000 Iss_Qty, A.ItemID, A.ColorID, A.SizeID From Socks_Store_Transfer_knit_Req_Orderwise()A Left Join (Select Order_No, Despatch_Closed From buy_ord_Style Union All Select Order_No, Despatch_Closed From Socks_Order_Master) B On A.Order_No = B.Order_No Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_orders()C On A.Order_No = C.Order_No Where A.Order_no Like '%OCN%' And Isnull(B.Despatch_Closed, 'N') = 'N' And A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' And C.Empl_ID <> 89", String.Empty, 150, 100);
                                }
                                else
                                {
                                    //Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Distinct A.Order_No, A.Bal_Qty, A.Req_Qty, A.Rec_Qty, A.Bal_Qty, 0.000 Iss_Qty, A.ItemID, A.ColorID, A.SizeID From Socks_Store_Transfer_knit_Req_Orderwise()A Left Join buy_ord_Style B On A.Order_No = B.Order_No Where A.Order_no Like '%OCN%' And Isnull(B.Despatch_Closed, 'N') = 'N' And A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' ", String.Empty, 150, 100);
                                    Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Distinct A.Order_No, A.Bal_Qty, A.Req_Qty, A.Rec_Qty, A.Bal_Qty, 0.000 Iss_Qty, A.ItemID, A.ColorID, A.SizeID From Socks_Store_Transfer_knit_Req_Orderwise()A Left Join (Select Order_No, Despatch_Closed From buy_ord_Style Union All Select Order_No, Despatch_Closed From Socks_Order_Master) B On A.Order_No = B.Order_No Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_orders()C On A.Order_No = C.Order_No Where A.Order_no Like '%OCN%' And Isnull(B.Despatch_Closed, 'N') = 'N' And A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' And C.Empl_ID = 89", String.Empty, 150, 100);
                                }
                            }
                            else if (MyParent.UserCode == 12)
                            {
                                if (ChkOtherMerchandiser.Checked == true)
                                {
                                    Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Distinct A.Order_No, A.Bal_Qty, A.Req_Qty, A.Rec_Qty, A.Bal_Qty, 0.000 Iss_Qty, A.ItemID, A.ColorID, A.SizeID From Socks_Store_Transfer_knit_Req_Orderwise()A Left Join (Select Order_No, Despatch_Closed From buy_ord_Style Union All Select Order_No, Despatch_Closed From Socks_Order_Master) B On A.Order_No = B.Order_No Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_orders()C On A.Order_No = C.Order_No Where A.Order_no Like '%OCN%' And Isnull(B.Despatch_Closed, 'N') = 'N' And A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' And C.Empl_ID <> 94", String.Empty, 150, 100);
                                }
                                else
                                {
                                    //Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Distinct A.Order_No, A.Bal_Qty, A.Req_Qty, A.Rec_Qty, A.Bal_Qty, 0.000 Iss_Qty, A.ItemID, A.ColorID, A.SizeID From Socks_Store_Transfer_knit_Req_Orderwise()A Left Join buy_ord_Style B On A.Order_No = B.Order_No Where A.Order_no Like '%OCN%' And Isnull(B.Despatch_Closed, 'N') = 'N' And A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' ", String.Empty, 150, 100);
                                    Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Distinct A.Order_No, A.Bal_Qty, A.Req_Qty, A.Rec_Qty, A.Bal_Qty, 0.000 Iss_Qty, A.ItemID, A.ColorID, A.SizeID From Socks_Store_Transfer_knit_Req_Orderwise()A Left Join (Select Order_No, Despatch_Closed From buy_ord_Style Union All Select Order_No, Despatch_Closed From Socks_Order_Master) B On A.Order_No = B.Order_No Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_orders()C On A.Order_No = C.Order_No Where A.Order_no Like '%OCN%' And Isnull(B.Despatch_Closed, 'N') = 'N' And A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' And C.Empl_ID = 94", String.Empty, 150, 100);
                                }
                            }
                            else if (MyParent.UserCode == 13)
                            {
                                if (ChkOtherMerchandiser.Checked == true)
                                {
                                    Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Distinct A.Order_No, A.Bal_Qty, A.Req_Qty, A.Rec_Qty, A.Bal_Qty, 0.000 Iss_Qty, A.ItemID, A.ColorID, A.SizeID From Socks_Store_Transfer_knit_Req_Orderwise()A Left Join (Select Order_No, Despatch_Closed From buy_ord_Style Union All Select Order_No, Despatch_Closed From Socks_Order_Master) B On A.Order_No = B.Order_No Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_orders()C On A.Order_No = C.Order_No Where A.Order_no Like '%OCN%' And Isnull(B.Despatch_Closed, 'N') = 'N' And A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' And C.Empl_ID <> 95", String.Empty, 150, 100);
                                }
                                else
                                {
                                    //Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Distinct A.Order_No, A.Bal_Qty, A.Req_Qty, A.Rec_Qty, A.Bal_Qty, 0.000 Iss_Qty, A.ItemID, A.ColorID, A.SizeID From Socks_Store_Transfer_knit_Req_Orderwise()A Left Join buy_ord_Style B On A.Order_No = B.Order_No Where A.Order_no Like '%OCN%' And Isnull(B.Despatch_Closed, 'N') = 'N' And A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' ", String.Empty, 150, 100);
                                    Dr = Tool.Selection_Tool_Except_New("Order_No", this, 30, 70, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Orderwise Req Details ", "Select Distinct A.Order_No, A.Bal_Qty, A.Req_Qty, A.Rec_Qty, A.Bal_Qty, 0.000 Iss_Qty, A.ItemID, A.ColorID, A.SizeID From Socks_Store_Transfer_knit_Req_Orderwise()A Left Join (Select Order_No, Despatch_Closed From buy_ord_Style Union All Select Order_No, Despatch_Closed From Socks_Order_Master) B On A.Order_No = B.Order_No Left Join Vaahini_Erp_Gainup.Dbo.Time_Action_orders()C On A.Order_No = C.Order_No Where A.Order_no Like '%OCN%' And Isnull(B.Despatch_Closed, 'N') = 'N' And A.itemid = " + ItemID + "  and A.colorid = " + ColorID + " and A.sizeid = " + SizeID + " And A.Order_No != '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' And C.Empl_ID = 95", String.Empty, 150, 100);
                                }
                            }
                        }

                        if (Dr != null)
                        {
                            Txt1.Text = Dr["Order_No"].ToString();
                            
                            GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                            GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value = Dr["Iss_Qty"].ToString();
                            GridDetail["ItemID", GridDetail.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                            GridDetail["ColorID", GridDetail.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();
                            GridDetail["SizeID", GridDetail.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                            GridDetail["SlNo1", GridDetail.CurrentCell.RowIndex].Value = Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();
                            ItemID = Convert.ToInt64(Grid["ItemID", Grid.CurrentCell.RowIndex].Value);
                            ColorID = Convert.ToInt64(Grid["ColorID", Grid.CurrentCell.RowIndex].Value);
                            SizeID = Convert.ToInt64(Grid["SizeID", Grid.CurrentCell.RowIndex].Value);
                            Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString()); 
                        }
                    }
                }
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
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Iss_Qty"].Index)
                {
                    MyBase.Valid_Decimal(Txt1, e);
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
        void TxtIss_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Order_No"].Index)
                {
                    if (GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString() != String.Empty && GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                    }
                    if (GridDetail.CurrentCell.RowIndex > 0)
                    {
                        if (GridDetail["Order_No", GridDetail.CurrentCell.RowIndex - 1].Value.ToString() != String.Empty && GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex - 1].Value.ToString() != String.Empty)
                        {
                            Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex - 1].Value.ToString());
                        }
                    }
                }
                else if(GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Iss_Qty"].Index)
                {
                    if (GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                    }
                }
                Iss_Balance();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Iss_Balance()
        {
            try
            {
                if (TxtQty1.Text.Trim() == String.Empty)
                {
                    TxtQty1.Text = "0.000";
                }

                TxtEnteredWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum_With_Three_Digits(ref GridDetail, "Iss_Qty")));

                if (TxtEnteredWeight.Text.Trim() == String.Empty)
                {
                    TxtEnteredWeight.Text = "0.000";
                }

                TxtBalance.Text = String.Format("{0:0.000}", Convert.ToDouble(TxtQty1.Text) - Convert.ToDouble(TxtEnteredWeight.Text));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GridDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt1 == null)
                {
                    Txt1 = (TextBox)e.Control;
                    Txt1.KeyPress += new KeyPressEventHandler(TxtIss_KeyPress);
                    Txt1.GotFocus += new EventHandler(TxtIss_GotFocus);
                    Txt1.KeyDown += new KeyEventHandler(TxtIss_KeyDown);
                    Txt1.Leave += new EventHandler(TxtIss_Leave);
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
                if (TxtBalance.Text.Trim() == String.Empty || Convert.ToDouble(TxtBalance.Text.ToString()) < 0 || Convert.ToDouble(LblBal.Text.ToString()) < 0)
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    GridDetail.CurrentCell = GridDetail["Iss_Qty", 0];
                    GridDetail.Focus();
                    GridDetail.BeginEdit(true);
                    return;
                }
                else
                {
                    GBQty.Visible = false;
                    Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value = TxtEnteredWeight.Text;
                    Grid.CurrentCell = Grid["Order_No", (Grid.CurrentCell.RowIndex)+1];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    Total_Count();
                    return;
                }
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
                for (int i = 0; i <= GridDetail.Rows.Count - 1; i++)
                {
                    if (GridDetail["Iss_Qty", i].Value == DBNull.Value || Convert.ToDouble(GridDetail["Iss_Qty", i].Value) == 0.000)
                    {
                        MessageBox.Show("Invalid KGS ..!", "Gainup");
                        Grid.CurrentCell = Grid["Iss_Qty", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        GBQty.Visible = false;
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                DtQty = new DataTable[30];
                GBQty.Visible = false;
                Grid["Iss_Qty", Grid.CurrentCell.RowIndex].Value = TxtEnteredWeight.Text;
                Grid.CurrentCell = Grid["Iss_Qty", (Grid.CurrentCell.RowIndex)];
                Grid.Focus();
                Total_Count();
                Grid.BeginEdit(true);
                return;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Yarn_Transfer_New_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                    if (e.KeyCode == Keys.Enter)
                    {
                        e.Handled = true;
                        if (MyParent.UserCode != 11 && MyParent.UserCode != 39)
                        {
                            if (this.ActiveControl.Name == "TxtItem")
                            {
                                TxtColor.Focus();
                                return;
                            }
                            else if (this.ActiveControl.Name == "TxtColor")
                            {
                                TxtSize.Focus();
                                return;
                            }
                            else if (this.ActiveControl.Name == "TxtSize")
                            {
                                Grid.CurrentCell = Grid["Order_No", 0];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                        }
                        if (this.ActiveControl.Name == "TxtTotal")
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
                        if (MyParent.UserCode != 11 && MyParent.UserCode != 39)
                        {
                            if (this.ActiveControl.Name == "TxtItem")
                            {
                                if (Dt.Rows.Count > 0)
                                {
                                    if (MessageBox.Show("Sure to Clear Grid Detail ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                    {
                                        MyBase.Clear(this);
                                        Grid_Data();
                                        TxtItem.Focus();
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                //Admin User
                                if (MyParent.UserCode == 1 || MyParent.UserCode == 19)
                                {
                                    if (TxtSize.Text == String.Empty && TxtColor.Text == String.Empty)
                                    {
                                        if (ChkClosed.Checked == true)
                                        {
                                            Str = " Select C.Item, A.ItemID, Sum(Cur_Stock)Cur_Stock From Socks_Store_Current_Stock() A Left Join (Select Distinct Order_No From Socks_Bom()Where Isnull(Despatch_Closed, 'N') = 'Y')B On A.Order_No = B.Order_No ";
                                            Str = Str + " Left Join Item C On A.ItemID = C.ItemID Left Join Color D On A.ColorID = D.ColorID Left Join Size E on A.SizeID = E.SizeID Where B.Order_No Is Not Null Group By C.Item, A.ItemID Having Sum(Cur_Stock) > 0 Order By C.Item, A.ItemID ";
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", Str, String.Empty, 150);
                                        }
                                        else
                                        {
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", " Select Distinct Item, ItemID From Socks_Store_Lot_For_Transfer_Admin() Order By Item ", String.Empty, 150);
                                        }
                                    }
                                    else if (TxtSize.Text != String.Empty && TxtColor.Text == String.Empty)
                                    {
                                        if (ChkClosed.Checked == true)
                                        {
                                            Str = " Select C.Item, A.ItemID, Sum(Cur_Stock)Cur_Stock From Socks_Store_Current_Stock() A Left Join (Select Distinct Order_No From Socks_Bom()Where Isnull(Despatch_Closed, 'N') = 'Y')B On A.Order_No = B.Order_No ";
                                            Str = Str + " Left Join Item C On A.ItemID = C.ItemID Left Join Color D On A.ColorID = D.ColorID Left Join Size E on A.SizeID = E.SizeID ";
                                            Str = Str + " Where B.Order_No Is Not Null And E.Size = '" + TxtSize.Text + "' Group By C.Item, A.ItemID Having Sum(Cur_Stock) > 0 Order By C.Item, A.ItemID ";
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", Str, String.Empty, 150);
                                        }
                                        else
                                        {
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", " Select Distinct Item, ItemID From Socks_Store_Lot_For_Transfer_Admin() Where Size = '" + TxtSize.Text + "'  Order By Item ", String.Empty, 150);
                                        }
                                    }
                                    else if (TxtSize.Text == String.Empty && TxtColor.Text != String.Empty)
                                    {
                                        if (ChkClosed.Checked == true)
                                        {
                                            Str = " Select C.Item, A.ItemID, Sum(Cur_Stock)Cur_Stock From Socks_Store_Current_Stock() A Left Join (Select Distinct Order_No From Socks_Bom()Where Isnull(Despatch_Closed, 'N') = 'Y')B On A.Order_No = B.Order_No ";
                                            Str = Str + " Left Join Item C On A.ItemID = C.ItemID Left Join Color D On A.ColorID = D.ColorID Left Join Size E on A.SizeID = E.SizeID ";
                                            Str = Str + " Where B.Order_No Is Not Null And D.Color = '" + TxtColor.Text + "' Group By C.Item, A.ItemID Having Sum(Cur_Stock) > 0 Order By C.Item, A.ItemID ";
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", Str, String.Empty, 150);
                                        }
                                        else
                                        {
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", " Select Distinct Item, ItemID From Socks_Store_Lot_For_Transfer_Admin() Where Color = '" + TxtColor.Text + "' Order By Item ", String.Empty, 150);
                                        }
                                    }
                                    else
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", " Select Distinct Item, ItemID From Socks_Store_Lot_For_Transfer_Admin() Where Size = '" + TxtSize.Text + "' And Color = '" + TxtColor.Text + "' Order By Item ", String.Empty, 150);
                                    }
                                }
                                //Other User
                                else
                                {
                                    if (TxtSize.Text == String.Empty && TxtColor.Text == String.Empty)
                                    {
                                        
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", " Select Distinct Item, ItemID From Socks_Store_Lot_For_Transfer() Order By Item ", String.Empty, 150);
                                    }
                                    else if (TxtSize.Text != String.Empty && TxtColor.Text == String.Empty)
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", " Select Distinct Item, ItemID From Socks_Store_Lot_For_Transfer() Where Size = '" + TxtSize.Text + "'  Order By Item ", String.Empty, 150);
                                    }
                                    else if (TxtSize.Text == String.Empty && TxtColor.Text != String.Empty)
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", " Select Distinct Item, ItemID From Socks_Store_Lot_For_Transfer() Where Color = '" + TxtColor.Text + "' Order By Item ", String.Empty, 150);
                                    }
                                    else
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item..!", " Select Distinct Item, ItemID From Socks_Store_Lot_For_Transfer() Where Size = '" + TxtSize.Text + "' And Color = '" + TxtColor.Text + "' Order By Item ", String.Empty, 150);
                                    }
                                }
                                if (Dr != null)
                                {
                                    TxtItem.Text = Dr["Item"].ToString();
                                    TxtItem.Tag = Dr["Itemid"].ToString();
                                }
                            }
                            if (this.ActiveControl.Name == "TxtColor")
                            {
                                if (Dt.Rows.Count > 0)
                                {
                                    if (MessageBox.Show("Sure to Clear Grid Detail ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                    {
                                        MyBase.Clear(this);
                                        Grid_Data();
                                        TxtColor.Focus();
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                //Admin User
                                if (MyParent.UserCode == 1 || MyParent.UserCode == 19)
                                {
                                    if (TxtItem.Text == String.Empty && TxtSize.Text == String.Empty)
                                    {
                                        if (ChkClosed.Checked == true)
                                        {
                                            Str = " Select D.Color, A.ColorID, Sum(Cur_Stock)Cur_Stock From Socks_Store_Current_Stock() A Left Join (Select Distinct Order_No From Socks_Bom()Where Isnull(Despatch_Closed, 'N') = 'Y')B On A.Order_No = B.Order_No ";
                                            Str = Str + " Left Join Item C On A.ItemID = C.ItemID Left Join Color D On A.ColorID = D.ColorID Left Join Size E on A.SizeID = E.SizeID ";
                                            Str = Str + " Where B.Order_No Is Not Null Group By D.Color, A.ColorID Having Sum(Cur_Stock) > 0 Order By D.Color, A.ColorID ";
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", Str, String.Empty, 150);
                                        }
                                        else
                                        {
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", " Select Distinct Color, ColorID From Socks_Store_Lot_For_Transfer_Admin() Order By Color ", String.Empty, 150);
                                        }
                                    }
                                    else if (TxtItem.Text != String.Empty && TxtSize.Text == String.Empty)
                                    {
                                        if (ChkClosed.Checked == true)
                                        {
                                            Str = " Select D.Color, A.ColorID, Sum(Cur_Stock)Cur_Stock From Socks_Store_Current_Stock() A Left Join (Select Distinct Order_No From Socks_Bom()Where Isnull(Despatch_Closed, 'N') = 'Y')B On A.Order_No = B.Order_No ";
                                            Str = Str + " Left Join Item C On A.ItemID = C.ItemID Left Join Color D On A.ColorID = D.ColorID Left Join Size E on A.SizeID = E.SizeID ";
                                            Str = Str + " Where B.Order_No Is Not Null And C.Item = '" + TxtItem.Text + "' Group By D.Color, A.ColorID Having Sum(Cur_Stock) > 0 Order By D.Color, A.ColorID ";
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", Str, String.Empty, 150);
                                        }
                                        else
                                        {
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", " Select Distinct Color, ColorID From Socks_Store_Lot_For_Transfer_Admin() Where Item = '" + TxtItem.Text + "' Order By Color", String.Empty, 150);
                                        }
                                    }
                                    else if (TxtItem.Text == String.Empty && TxtSize.Text != String.Empty)
                                    {
                                        if (ChkClosed.Checked == true)
                                        {
                                            Str = " Select D.Color, A.ColorID, Sum(Cur_Stock)Cur_Stock From Socks_Store_Current_Stock() A Left Join (Select Distinct Order_No From Socks_Bom()Where Isnull(Despatch_Closed, 'N') = 'Y')B On A.Order_No = B.Order_No ";
                                            Str = Str + " Left Join Item C On A.ItemID = C.ItemID Left Join Color D On A.ColorID = D.ColorID Left Join Size E on A.SizeID = E.SizeID ";
                                            Str = Str + " Where B.Order_No Is Not Null And E.Size = '" + TxtSize.Text + "' Group By D.Color, A.ColorID Having Sum(Cur_Stock) > 0 Order By D.Color, A.ColorID ";
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", Str, String.Empty, 150);
                                        }
                                        else
                                        {
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", " Select Distinct Color, ColorID From Socks_Store_Lot_For_Transfer_Admin() Where Size = '" + TxtSize.Text + "' Order By Color ", String.Empty, 150);
                                        }
                                    }
                                    else
                                    {
                                        if (ChkClosed.Checked == true)
                                        {
                                            Str = " Select D.Color, A.ColorID, Sum(Cur_Stock)Cur_Stock From Socks_Store_Current_Stock() A Left Join (Select Distinct Order_No From Socks_Bom()Where Isnull(Despatch_Closed, 'N') = 'Y')B On A.Order_No = B.Order_No ";
                                            Str = Str + " Left Join Item C On A.ItemID = C.ItemID Left Join Color D On A.ColorID = D.ColorID Left Join Size E on A.SizeID = E.SizeID ";
                                            Str = Str + " Where B.Order_No Is Not Null And C.Item = '" + TxtItem.Text + "' And E.Size = '" + TxtSize.Text + "' Group By D.Color, A.ColorID Having Sum(Cur_Stock) > 0 Order By D.Color, A.ColorID ";
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", Str, String.Empty, 150);
                                        }
                                        else
                                        {
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", " Select Distinct Color, ColorID From Socks_Store_Lot_For_Transfer_Admin() Where Item = '" + TxtItem.Text + "' And Size = '" + TxtSize.Text + "' Order By Color ", String.Empty, 150);
                                        }
                                    }
                                }
                                // Other User
                                else
                                {
                                    if (TxtItem.Text == String.Empty && TxtSize.Text == String.Empty)
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", " Select Distinct Color, ColorID From Socks_Store_Lot_For_Transfer() Order By Color ", String.Empty, 150);
                                    }
                                    else if (TxtItem.Text != String.Empty && TxtSize.Text == String.Empty)
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", " Select Distinct Color, ColorID From Socks_Store_Lot_For_Transfer() Where Item = '" + TxtItem.Text + "' Order By Color", String.Empty, 150);
                                    }
                                    else if (TxtItem.Text == String.Empty && TxtSize.Text != String.Empty)
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", " Select Distinct Color, ColorID From Socks_Store_Lot_For_Transfer() Where Size = '" + TxtSize.Text + "' Order By Color ", String.Empty, 150);
                                    }
                                    else
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", " Select Distinct Color, ColorID From Socks_Store_Lot_For_Transfer() Where Item = '" + TxtItem.Text + "' And Size = '" + TxtSize.Text + "' Order By Color ", String.Empty, 150);
                                    }
                                }
                                if (Dr != null)
                                {
                                    TxtColor.Text = Dr["Color"].ToString();
                                    TxtColor.Tag = Dr["Colorid"].ToString();
                                }
                            }
                            if (this.ActiveControl.Name == "TxtSize")
                            {
                                if (Dt.Rows.Count > 0)
                                {
                                    if (MessageBox.Show("Sure to Clear Grid Detail ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                    {
                                        MyBase.Clear(this);
                                        Grid_Data();
                                        TxtSize.Focus();
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                //Admin User
                                if (MyParent.UserCode == 1 || MyParent.UserCode == 19)
                                {
                                    if (TxtItem.Text == String.Empty && TxtColor.Text == String.Empty)
                                    {
                                        if (ChkClosed.Checked == true)
                                        {
                                            Str = " Select E.Size, A.SizeID, Sum(Cur_Stock)Cur_Stock From Socks_Store_Current_Stock() A Left Join (Select Distinct Order_No From Socks_Bom()Where Isnull(Despatch_Closed, 'N') = 'Y')B On A.Order_No = B.Order_No ";
                                            Str = Str + " Left Join Item C On A.ItemID = C.ItemID Left Join Color D On A.ColorID = D.ColorID Left Join Size E on A.SizeID = E.SizeID ";
                                            Str = Str + " Where B.Order_No Is Not Null Group By E.Size, A.SizeID Having Sum(Cur_Stock) > 0 Order By E.Size, A.SizeID ";
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", Str, String.Empty, 150);
                                        }
                                        else
                                        {
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", " Select Distinct Size, SizeID From Socks_Store_Lot_For_Transfer_Admin() Order By Size ", String.Empty, 150);
                                        }
                                    }
                                    else if (TxtItem.Text != String.Empty && TxtColor.Text == String.Empty)
                                    {
                                        if (ChkClosed.Checked == true)
                                        {
                                            Str = " Select E.Size, A.SizeID, Sum(Cur_Stock)Cur_Stock From Socks_Store_Current_Stock() A Left Join (Select Distinct Order_No From Socks_Bom()Where Isnull(Despatch_Closed, 'N') = 'Y')B On A.Order_No = B.Order_No ";
                                            Str = Str + " Left Join Item C On A.ItemID = C.ItemID Left Join Color D On A.ColorID = D.ColorID Left Join Size E on A.SizeID = E.SizeID ";
                                            Str = Str + " Where B.Order_No Is Not Null And C.Item = '" + TxtItem.Text + "' Group By E.Size, A.SizeID Having Sum(Cur_Stock) > 0 Order By E.Size, A.SizeID ";
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", Str, String.Empty, 150);
                                        }
                                        else
                                        {
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", " Select Distinct Size, SizeID From Socks_Store_Lot_For_Transfer_Admin() Where Item = '" + TxtItem.Text + "' Order By Size", String.Empty, 150);
                                        }
                                    }
                                    else if (TxtItem.Text == String.Empty && TxtColor.Text != String.Empty)
                                    {
                                        if (ChkClosed.Checked == true)
                                        {
                                            Str = " Select E.Size, A.SizeID, Sum(Cur_Stock)Cur_Stock From Socks_Store_Current_Stock() A Left Join (Select Distinct Order_No From Socks_Bom()Where Isnull(Despatch_Closed, 'N') = 'Y')B On A.Order_No = B.Order_No ";
                                            Str = Str + " Left Join Item C On A.ItemID = C.ItemID Left Join Color D On A.ColorID = D.ColorID Left Join Size E on A.SizeID = E.SizeID ";
                                            Str = Str + " Where B.Order_No Is Not Null And D.Color = '" + TxtColor.Text + "' Group By E.Size, A.SizeID Having Sum(Cur_Stock) > 0 Order By E.Size, A.SizeID ";
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", Str, String.Empty, 150);
                                        }
                                        else
                                        {
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", " Select Distinct Size, SizeID From Socks_Store_Lot_For_Transfer_Admin() Where Color = '" + TxtColor.Text + "' Order By Size ", String.Empty, 150);
                                        }
                                    }
                                    else
                                    {
                                        if (ChkClosed.Checked == true)
                                        {
                                            Str = " Select E.Size, A.SizeID, Sum(Cur_Stock)Cur_Stock From Socks_Store_Current_Stock() A Left Join (Select Distinct Order_No From Socks_Bom()Where Isnull(Despatch_Closed, 'N') = 'Y')B On A.Order_No = B.Order_No ";
                                            Str = Str + " Left Join Item C On A.ItemID = C.ItemID Left Join Color D On A.ColorID = D.ColorID Left Join Size E on A.SizeID = E.SizeID ";
                                            Str = Str + " Where B.Order_No Is Not Null And C.Item = '" + TxtItem.Text + "' And D.Color = '" + TxtColor.Text + "' Group By E.Size, A.SizeID Having Sum(Cur_Stock) > 0 Order By E.Size, A.SizeID ";
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", Str, String.Empty, 150);
                                        }
                                        else
                                        {
                                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", " Select Distinct Size, SizeID From Socks_Store_Lot_For_Transfer_Admin() Where Item = '" + TxtItem.Text + "' And Color = '" + TxtColor.Text + "' Order By Size ", String.Empty, 150);
                                        }
                                    }
                                }
                                //Other User's
                                else
                                {
                                    if (TxtItem.Text == String.Empty && TxtColor.Text == String.Empty)
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", " Select Distinct Size, SizeID From Socks_Store_Lot_For_Transfer() Order By Size ", String.Empty, 150);
                                    }
                                    else if (TxtItem.Text != String.Empty && TxtColor.Text == String.Empty)
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", " Select Distinct Size, SizeID From Socks_Store_Lot_For_Transfer() Where Item = '" + TxtItem.Text + "' Order By Size", String.Empty, 150);
                                    }
                                    else if (TxtItem.Text == String.Empty && TxtColor.Text != String.Empty)
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", " Select Distinct Size, SizeID From Socks_Store_Lot_For_Transfer() Where Color = '" + TxtColor.Text + "'Order By Size ", String.Empty, 150);
                                    }
                                    else
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size..!", " Select Distinct Size, SizeID From Socks_Store_Lot_For_Transfer() Where Item = '" + TxtItem.Text + "' And Color = '" + TxtColor.Text + "'Order By Size ", String.Empty, 150);
                                    }
                                }
                                if (Dr != null)
                                {
                                    TxtSize.Text = Dr["Size"].ToString();
                                    TxtSize.Tag = Dr["Sizeid"].ToString();
                                }
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
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Iss_Qty"].Index)
                    {
                        if (Convert.ToDouble(Grid["Stock_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) > 0)
                        {
                            TxtQty1.Text = Grid["Stock_Qty", Grid.CurrentCell.RowIndex].Value.ToString();

                            GridDetail_Data(Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value), Convert.ToInt64(Grid["ItemID", Grid.CurrentCell.RowIndex].Value), Convert.ToInt64(Grid["ColorID", Grid.CurrentCell.RowIndex].Value), Convert.ToInt64(Grid["SizeID", Grid.CurrentCell.RowIndex].Value));
                            GridDetail.CurrentCell = GridDetail["Order_No", 0];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            e.Handled = true;
                            return;
                        }
                        else
                        {
 
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
                //if (e.KeyChar == Convert.ToChar(Keys.Escape))
                //{
                //    Total_Count();
                //    TxtRemarks.Focus();
                //    return;
                //}
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
                Total_Count();
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
                //MyBase.Grid_Delete(ref GridDetail, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], GridDetail.CurrentCell.RowIndex);
                //DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].AcceptChanges();
                //MyBase.Row_Number(ref GridDetail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Frm_Yarn_Transfer_New_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtItem" || this.ActiveControl.Name == "TxtColor" || this.ActiveControl.Name == "TxtSize" || this.ActiveControl.Name == "TxtTotal")
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ChkClosed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChkClosed.Checked == false)
                {
                    MyBase.Clear(this);
                    Grid_Data();
                }
                else if (ChkClosed.Checked == true)
                {
                    MyBase.Clear(this);
                    Grid_Data();
                    TxtItem.Focus();
                }
                if (TxtItem.Enabled == false)
                {
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChkOtherMerchandiser_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChkOtherMerchandiser.Checked == false)
                {
                    MyBase.Clear(this);
                    Grid_Data();
                }
                else if (ChkOtherMerchandiser.Checked == true)
                {
                    MyBase.Clear(this);
                    Grid_Data();
                    TxtItem.Focus();
                }
                if (TxtItem.Enabled == false)
                {
                    Grid.CurrentCell = Grid["Order_No", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridDetail_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {   
                if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Iss_Qty"].Index)
                    {
                        if (Txt1.Text.ToString() != String.Empty && GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value = Txt1.Text;
                        }
                        if (GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString() != String.Empty && (GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value) == 0))
                        {
                            //Txt.Text
                            MessageBox.Show("Invalid Iss_Qty...!", "Gainup");
                            GridDetail.CurrentCell = GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                            if (Convert.ToDouble(LblBal.Text.ToString()) < 0)
                            {
                                MessageBox.Show("Invalid Iss_Qty...!", "Gainup");
                                GridDetail.CurrentCell = GridDetail["Iss_Qty", Grid.CurrentCell.RowIndex];
                                GridDetail["Iss_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
                                GridDetail.Focus();
                                GridDetail.BeginEdit(true);
                                return;
                            }
                        }
                    }
                    Iss_Balance();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void GridDetail_Leave(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Iss_Qty"].Index)
        //        {
        //            if (GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString() != String.Empty && (GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value) == 0))
        //            {
        //                //Txt.Text
        //                MessageBox.Show("Invalid Iss_Qty...!", "Gainup");
        //                if (GridDetail.Rows.Count <= 2)
        //                {
        //                    GridDetail.CurrentCell = GridDetail["Iss_Qty", 0];
        //                }
        //                else
        //                {
        //                    GridDetail.CurrentCell = GridDetail["Iss_Qty", Grid.CurrentCell.RowIndex];
        //                }
        //                GridDetail.Focus();
        //                GridDetail.BeginEdit(true);
        //                return;
        //            }
        //            else
        //            {
        //                Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
        //                if (Convert.ToDouble(LblBal.Text.ToString()) < 0)
        //                {
        //                    MessageBox.Show("Invalid Iss_Qty...!", "Gainup");
        //                    GridDetail.CurrentCell = GridDetail["Iss_Qty", Grid.CurrentCell.RowIndex];
        //                    GridDetail["Iss_Qty", Grid.CurrentCell.RowIndex].Value = "0.000";
        //                    GridDetail.Focus();
        //                    GridDetail.BeginEdit(true);
        //                    return;
        //                }
        //            }
        //        }
        //        Iss_Balance();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
    }
}
