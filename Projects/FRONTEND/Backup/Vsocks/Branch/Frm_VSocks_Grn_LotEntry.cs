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
    public partial class Frm_VSocks_Grn_LotEntry : Form, Entry
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
        String Buffer_Table = String.Empty;
        Boolean Buffer_Update = false;
        Int64 Mode = 0;
        Int16 R = 0;

        Double Bal = 0.000;
        public Frm_VSocks_Grn_LotEntry()
        {
            InitializeComponent();
        }

        private void Frm_VSocks_Grn_LotEntry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                int i = 0;
                TxtGrnNO.Focus();
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
                TxtGrnNO.Focus();
                Grid_Data();                
                DtQty = new DataTable[300];                
                return;
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
                Print_BarCode();
                MessageBox.Show("Ok ...!", "Gainup");
                Entry_View();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Print_BarCode()
        {
            StreamWriter Sr = null;
            DataTable Tdt = new DataTable();
            Int32 i = 0;
            String Str = String.Empty;
            try
            {
                MyBase.Load_Data("Select supplier, color, LotNo, bagno, (case when Item ='Cotton' then 'Combed Cotton' else Item end) Item, size Count, Sum(weight) Quantity, isnull(buyer,'-') buyer, Qc_Status, Order_No, '-' PO_No, Grn_No, Grn_Date, (case when Order_No='GENERAL' then 'OGENERAL' else Substring(Order_No,8,5) end) OCN, Substring(Grn_No,8,5) GRN, Location from Barcode_Details_Lot() where RowID = " + Code + " Group By supplier, color, LotNo, bagno, Item, Size, isnull(buyer,'-'), Qc_Status, Order_No, Grn_No, Grn_Date, Location ", ref Tdt);
                //MyBase.Load_Data("Select * From Barcode_Details_For_Print(" + Code + ")", ref Tdt);
                Sr = new StreamWriter("C:\\vaahrep\\Socks_Bar.txt");

                for (i = 0; i <= Tdt.Rows.Count - 1; i++)
                {
                    Sr.WriteLine("N");
                    Sr.WriteLine("ZT");
                    Sr.WriteLine("q814");
                    Sr.WriteLine("Q196, 24");
                    Sr.WriteLine("JF");
                    Sr.WriteLine("D9");
                    Sr.WriteLine("S4");
                    Sr.WriteLine("O");
                    Sr.WriteLine("A110,14,0,4,1,1,N," + Convert.ToChar(34) + "Gainup Industries India Pvt Ltd - Socks" + Convert.ToChar(34));
                    Sr.WriteLine("A60,60,0,4,1,1,N," + Convert.ToChar(34) + "Supplier :" + Tdt.Rows[i]["supplier"].ToString() + Convert.ToChar(34));
                    //Sr.WriteLine("A450,60,0,4,1,1,N," + Convert.ToChar(34) + "Location :" + Tdt.Rows[i]["Location"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A450,90,0,4,1,1,N," + Convert.ToChar(34) + "Lot No   :" + Tdt.Rows[i]["LotNo"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A60,90,0,4,1,1,N," + Convert.ToChar(34) + "Color    :" + Tdt.Rows[i]["color"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A60,120,0,4,1,1,N," + Convert.ToChar(34) + "Count    :" + Tdt.Rows[i]["Count"].ToString().Replace("C"," ") + Convert.ToChar(34));
                    Sr.WriteLine("A450,120,0,4,1,1,N," + Convert.ToChar(34) + "Quantity :" + Tdt.Rows[i]["Quantity"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A450,150,0,4,1,1,N," + Convert.ToChar(34) + "Qc Status:" + Tdt.Rows[i]["Qc_Status"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A60,150,0,4,1,1,N," + Convert.ToChar(34) + "Buyer    :" + Tdt.Rows[i]["buyer"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A60,180,0,4,1,1,N," + Convert.ToChar(34) + "Order No :" + Tdt.Rows[i]["Order_No"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A450,180,0,4,1,1,N," + Convert.ToChar(34) + "PO No    :" + Tdt.Rows[i]["PO_No"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A60,210,0,4,1,1,N," + Convert.ToChar(34) + "GRN No   :" + Tdt.Rows[i]["Grn_No"].ToString() + Convert.ToChar(34));
                    Str = String.Format("{0:dd-MMM-yyyy}", Tdt.Rows[i]["Grn_Date"]);
                    Sr.WriteLine("A450,210,0,4,1,1,N," + Convert.ToChar(34) + "GRN Date :" + Str + Convert.ToChar(34));
                    Sr.WriteLine("A60,240,0,4,1,1,N," + Convert.ToChar(34) + "Material :" + Tdt.Rows[i]["Item"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A500,270,0,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Location"].ToString() + Convert.ToChar(34));
                    if (Tdt.Rows[i]["OCN"].ToString() == "OGENERAL")
                    {
                        Str = String.Format("{0:00000000}", Tdt.Rows[i]["OCN"]) + String.Format("{0:00000}", Convert.ToDouble(Tdt.Rows[i]["GRN"])) + String.Format("{0:00000}", Tdt.Rows[i]["LotNo"]) + String.Format("{0:000}", Convert.ToDouble(Tdt.Rows[i]["bagno"]));
                    }
                    else
                    {
                        //Str = String.Format("{0:OCN00000}", Convert.ToDouble(Tdt.Rows[i]["OCN"])) + String.Format("{0:00000}", Convert.ToDouble(Tdt.Rows[i]["GRN"])) + String.Format("{0:00000}", Tdt.Rows[i]["LotNo"]) + String.Format("{0:000}", Convert.ToDouble(Tdt.Rows[i]["bagno"]));
                        Str = String.Format("{0:00000}", Convert.ToDouble(Tdt.Rows[i]["GRN"])) + String.Format("{0:00000}", Tdt.Rows[i]["LotNo"]) + String.Format("{0:000}", Convert.ToDouble(Tdt.Rows[i]["bagno"]));
                    }
                    //Sr.WriteLine("B200,270,0,1,2,4,61,B," + Convert.ToChar(34) + Str + Convert.ToChar(34));
                    Sr.WriteLine("B60,270,0,1,2,4,61,B," + Convert.ToChar(34) + Str + Convert.ToChar(34));
                    Sr.WriteLine("");
                    Sr.WriteLine("P1");
                    Sr.WriteLine("FE");
                    Sr.WriteLine("");
                    Sr.WriteLine("");
                    Sr.WriteLine("");
                }

                Sr.Close();
                MyBase.DosPrint("C:\\vaahrep\\Socks_Bar.txt");
                Sr = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Sr != null)
                {
                    Sr.Close();
                }
            }
        }

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                Code = Convert.ToInt64(Dr["RowID"]);
                TxtEntryNo.Text = Dr["ENo"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["EDate"]);
                TxtGrnNO.Text = Dr["Grn_No"].ToString();
                TxtSupplier.Text = Dr["Supplier"].ToString();
                TxtSupplier.Tag = Dr["Supplierid"].ToString();
                DtpDate1.Value = Convert.ToDateTime(Dr["Grn_Date"]);
                TxtRemarks.Text = Dr["Remarks"].ToString();                
                Grid_Data();
                Total_Count();
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
                DtQty = new DataTable[300];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Lot Entry - Edit", "select A.ENo, A.EDate, A.Grn_No, B.supplier, D.item, E.color, F.size, C.Grn_Qty, A.Remarks, A.Supplierid, A.Grn_Date, A.Rowid from VSocks_Lot_Master A Left Join FITSOCKS.dbo.Supplier B on A.Supplierid = B.Supplierid Left Join VSocks_Lot_Details C on A.RowID = C.Master_ID Left Join Item D on C.ItemID = D.itemid Left Join Color E on C.ColorID = E.Colorid Left Join Size F on C.SizeID = F.sizeid Where Isnull(A.Auto_Grn,'N')='N'", String.Empty, 100, 100, 150, 350, 200, 150, 100, 100, 100, 350);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Grn_Qty", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
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
                if (MyParent._New == true)
                {
                    if (i == 0)
                    {
                        //Str = "select 0 as Slno, A.Order_No, B.Item, C.Color, D.Size, 0 No_Of_bags, A.Inw_Qty Grn_Qty, A.Itemid, A.Colorid, A.Sizeid, 0 Slno1, 0 Slno_Temp From Fitsocks.dbo.Grn_Details_For_LOT()A Left Join fitsocks.dbo.Item B on A.itemid = B.itemid Left Join fitsocks.dbo.Color C on A.Colorid = C.Colorid Left Join fitsocks.dbo.Size D on A.Sizeid = D.Sizeid Where B.Item_Type='YARN' and 1 = 2 ";
                        Str = "Select 0 as Slno, B.Item, C.Color, D.Size, A.Inw_Qty Grn_Qty, A.Itemid, A.Colorid, A.Sizeid, 0 Slno1, 0 Slno_Temp, '' Remarks, '-' T From Fitsocks.dbo.Grn_Details_For_LOT()A Left Join fitsocks.dbo.Item B on A.itemid = B.itemid Left Join fitsocks.dbo.Color C on A.Colorid = C.Colorid Left Join fitsocks.dbo.Size D on A.Sizeid = D.Sizeid Where B.Item_Type='YARN' and 1 = 2 ";
                    }
                    else
                    {
                        //Str = "Select 0 as Slno, A.Order_No,A.Item,A.Color,A.Size,0 No_Of_bags,A.Inw_Qty Grn_Qty,A.Itemid,A.Colorid,A.Sizeid,0 Slno1, 0 Slno_Temp from fitsocks.dbo.Grn_Details_For_LOT()A where A.Grn_No='" + TxtGrnNO.Text + "'";
                        Str = "Select 0 as Slno, A.Item, A.Color, A.Size, Sum(A.Inw_Qty) Grn_Qty, A.Itemid, A.Colorid, A.Sizeid, 0 Slno1, 0 Slno_Temp, '' Remarks, '-' T from fitsocks.dbo.Grn_Details_For_LOT()A where A.Grn_No = '" + TxtGrnNO.Text + "' Group By A.Item, A.Color, A.Size, A.Itemid, A.Colorid, A.Sizeid";
                    }
                }
                else
                {
                    Str = "Select A.Slno, B1.Item, C.Color, D.Size, A.Grn_Qty, A.Itemid, A.Colorid, A.Sizeid, A.Slno1, A.Slno1 Slno_Temp from VSocks_Lot_Details A Left Join VSocks_Lot_Master B on B.RowID = A.Master_ID Left Join Item B1 on A.itemid = B1.itemid Left Join Color C on A.Colorid = C.Colorid Left Join Size D on A.Sizeid = D.Sizeid where B.Grn_No = '" + TxtGrnNO.Text + "'";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid, "Grn_Qty", "Remarks");
                MyBase.Grid_Designing(ref Grid, ref Dt, "ItemID", "SizeID", "ColorID", "Slno1", "Slno_Temp", "T");
                MyBase.Grid_Width(ref Grid, 50,150,300, 200, 100,100, 100);
                Grid.Columns["Grn_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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

        public void Entry_Save()
        {
            try
            {
                Int32 Array_Index = 0;
                String Order_Type = String.Empty;
                Total_Count();

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Select Grn_No Or Supplier..!", "Gainup");
                    TxtGrnNO.Focus();
                    
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtTotal.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotal.Text) == 0)
                {
                    MessageBox.Show("Invalid Details", "Gainup");
                    TxtGrnNO.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Grid["T", i].Value) > 0.000)
                    {
                        MessageBox.Show(" Invalid in BreakUp's in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Grn_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 5; j++)
                    {
                        if (Grid["Grn_Qty", i].Value == DBNull.Value || Grid["Grn_Qty", i].Value.ToString() == String.Empty || Grid["Grn_Qty", i].Value.ToString() == "0")
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
             
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (DtQty[Convert.ToInt32(Dt.Rows[i]["SlNo1"])] == null)
                    {
                        MessageBox.Show("Invalid LOt No Breakup Details ...!", "Gainup");
                        MyParent.Save_Error = true;
                        Grid.CurrentCell = Grid["Grn_Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                }

                TxtEntryNo.Text = MyBase.MaxOnlyComp("VSocks_Lot_Master", "ENo", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();
                Queries = new string[Dt.Rows.Count + 100000];                
                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into VSocks_Lot_Master (ENo, EDate, Grn_No, Remarks, Company_Code, Year_Code, User_Code, Supplierid, Grn_Date) Values (" + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + TxtGrnNO.Text + "', '" + TxtRemarks.Text + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "', " + MyParent.UserCode + ", " + TxtSupplier.Tag.ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "'); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("VSocks Lot Entry", "ADD", "@@IDENTITY");

                }
                else
                {
                    Queries[Array_Index++] = "Update VSocks_Lot_Master Set EDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', Grn_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', Remarks = '" + TxtRemarks.Text + "', Company_Code = " + MyParent.CompCode + " , Year_Code = '" + MyParent.YearCode + "', User_Code = " + MyParent.UserCode + " Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("VSocks Lot Entry", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from VSocks_Lot_Details where Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete from VSocks_Lot_Bag_Details where Master_ID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into VSocks_Lot_Details (Master_ID, Slno, ItemID, SizeID, ColorID, Grn_Qty, Slno1) Values (@@IDENTITY, " + Grid["Slno", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ", " + Grid["Grn_Qty", i].Value + ", " + Grid["Slno", i].Value + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into VSocks_Lot_Details (Master_ID, Slno, ItemID, SizeID, ColorID, Grn_Qty, Slno1) Values (" + Code + ", " + Grid["Slno", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ", " + Grid["Grn_Qty", i].Value + ", " + Grid["Slno", i].Value + ")";
                    }
                }
                for (int i = 0; i <= DtQty.Length - 1; i++)
                {
                    if (DtQty[i] != null)
                    {
                        for (int j = 0; j <= DtQty[i].Rows.Count - 1; j++)
                        {
                            if (MyParent._New)
                            {
                                Queries[Array_Index++] = "Insert Into VSocks_Lot_Bag_Details (slno, Master_ID, Order_NO, LotNo, Weight, BagNo, LocationID, SlNo1) Values ( " + DtQty[i].Rows[j]["Sno"].ToString() + ", @@IDENTITY,  '" + DtQty[i].Rows[j]["Order_No"].ToString() + "', '" + DtQty[i].Rows[j]["LotNo"].ToString() + "', " + DtQty[i].Rows[j]["Weight"].ToString() + ", " + DtQty[i].Rows[j]["BagNo"].ToString() + ", " + DtQty[i].Rows[j]["LocationID"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ")";
                            }
                            else
                            {
                                Queries[Array_Index++] = "Insert Into VSocks_Lot_Bag_Details (slno, Master_ID,  Order_No, LotNo, Weight, BagNo, LocationID, SlNo1) Values ( " + DtQty[i].Rows[j]["Sno"].ToString() + ", " + Code + ", '" + DtQty[i].Rows[j]["Order_No"].ToString() + "', '" + DtQty[i].Rows[j]["LotNo"].ToString() + "', " + DtQty[i].Rows[j]["Weight"].ToString() + ", " + DtQty[i].Rows[j]["BagNo"].ToString() + ", " + DtQty[i].Rows[j]["LocationID"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ")";
                            }
                        }
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Lot Entry - Delete", "select A.ENo, A.EDate, A.Grn_No, B.supplier, D.item, E.color, F.size, C.Grn_Qty, A.Remarks, A.Supplierid, A.Grn_Date, A.Rowid from VSocks_Lot_Master A Left Join FITSOCKS.dbo.Supplier B on A.Supplierid = B.Supplierid Left Join VSocks_Lot_Details C on A.RowID = C.Master_ID Left Join Item D on C.ItemID = D.itemid Left Join Color E on C.ColorID = E.Colorid Left Join Size F on C.SizeID = F.sizeid Where Isnull(A.Auto_Grn,'N')='N'", String.Empty, 100, 100, 150, 350, 200, 150, 100, 100, 100, 350);
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
                    MyBase.Run("Delete from VSocks_Lot_Bag_Details where Master_ID = " + Code, "Delete from VSocks_Lot_Details where Master_ID = " + Code, "Delete From VSocks_Lot_Master Where RowID = " + Code, MyParent.EntryLog("VSocks Lot Entry", "DELETE", Code.ToString()));
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


        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                DtQty = new DataTable[300];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Lot Entry - View", "select A.ENo, A.EDate, A.Grn_No, B.supplier, D.item, E.color, F.size, C.Grn_Qty, A.Remarks, A.Supplierid, A.Grn_Date, A.Rowid from VSocks_Lot_Master A Left Join FITSOCKS.dbo.Supplier B on A.Supplierid = B.Supplierid Left Join VSocks_Lot_Details C on A.RowID = C.Master_ID Left Join Item D on C.ItemID = D.itemid Left Join Color E on C.ColorID = E.Colorid Left Join Size F on C.SizeID = F.sizeid ", String.Empty, 100, 100, 150, 350, 200, 150, 100, 100, 100, 350);
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
        public void Entry_Cancel()
        {
            try
            {
                MyBase.Clear(this);
                GridDetail_Data(0,0); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_VSocks_Grn_LotEntry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;

                    if (this.ActiveControl.Name == "TxtGrnNO")
                    {
                        if (TxtGrnNO.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Select Grn No..!", "Gainup");
                            return;
                        }
                        else
                        {
                            TxtGrnNO.Text = Dr["Grn_No"].ToString();
                            TxtSupplier.Text = Dr["Supplier"].ToString();
                            TxtSupplier.Tag = Dr["Supplierid"].ToString();
                            i = 1;
                            Grid_Data();
                            Grid.CurrentCell = Grid["Grn_Qty", 0];
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
                if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtGrnNO")
                    {

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Grn No..!", "Select Distinct Grn_No, Isnull(Supplier,'GAINUP')Supplier, Item, Color, Size, Sum(Inw_Qty)Inw_Qty, Supplierid, Edate Grn_Date From fitsocks.dbo.Grn_Details_For_LOT() where Grn_No not in(select Grn_No from VSocks_Lot_Master) Group by Grn_No, Supplier, Item, Color, Size, Supplierid, Edate", String.Empty, 120, 200, 100, 100, 100, 100);
                        
                        if (Dr != null)
                        {
                            TxtGrnNO.Text = Dr["Grn_No"].ToString();
                            TxtSupplier.Text = Dr["Supplier"].ToString();
                            TxtSupplier.Tag = Dr["Supplierid"].ToString();
                            DtpDate1.Value = Convert.ToDateTime(Dr["Grn_Date"]);
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

        private void Frm_VSocks_Grn_LotEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtGrnNO")
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
                //if (this.ActiveControl.Name == "TxtSupplier")
                //{
                //    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                //}
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Grn_Qty"].Index)
                    {
                        //e.Handled = true;
                        TxtQty1.Text = Grid["Grn_Qty", Grid.CurrentCell.RowIndex].Value.ToString();

                            if (Convert.ToInt16(Grid["Slno_Temp", Grid.CurrentCell.RowIndex].Value) != 0)
                            {
                                Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();                                                                
                            }
                            else
                            {
                                Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                            }
                       
                        if (listBox1.Items.Count > 0)
                        {
                            for (int a = listBox1.Items.Count - 1; a >= 0; a--)
                            {
                                listBox1.Items.RemoveAt(a);
                            }
                            listBox1.Refresh();
                        }
                        GridDetail_Data(Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value), Convert.ToInt32(Grid["Grn_Qty", Grid.CurrentCell.RowIndex].Value));
                        GridDetail.CurrentCell = GridDetail["Order_No", 0];
                        GridDetail.Focus();
                        GridDetail.BeginEdit(true);
                        e.Handled = true;
                        return;

                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
                    {
                        if (Grid["Remarks", Grid.CurrentCell.RowIndex].Value == null || Grid["Remarks", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Remarks", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
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
                    Total_Count();
                    TxtRemarks.Focus();
                    return;
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
                {
                    e.Handled = false;
                }
                else
                {
                    MyBase.Valid_Null(Txt, e);
                }
                //Total_Count();
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
        
        void Roll_Balance()
        {
            try
            {

                if (TxtQty1.Text.Trim() == String.Empty)
                {
                    TxtQty1.Text = "0.000";
                }

                TxtEnteredWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum_With_Three_Digits(ref GridDetail, "Weight")));

                if (TxtEnteredWeight.Text.Trim() == String.Empty)
                {
                    TxtEnteredWeight.Text = "0.000";
                }

                TxtBalance.Text = String.Format("{0:0.000}", Convert.ToDouble(TxtQty1.Text) - Convert.ToDouble(TxtEnteredWeight.Text));

                if (TxtOrderWeight.Text.Trim() == String.Empty)
                {
                    TxtOrderWeight.Text = "0.000";
                }

                //if (GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value != null && GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                //{
                //    TxtOrderEnterWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString())));


                if (TxtOrderEnterWeight.Text.Trim() == String.Empty)
                {
                    TxtOrderEnterWeight.Text = "0.000";
                }

                //    TxtOrderBalance.Text = String.Format("{0:0.000}", Convert.ToDouble(TxtOrderWeight.Text) - Convert.ToDouble(TxtOrderEnterWeight.Text));
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Roll_Balance_New()
        {
            try
            {

                if (TxtQty1.Text.Trim() == String.Empty)
                {
                    TxtQty1.Text = "0.000";
                }

                TxtEnteredWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum_With_Three_Digits(ref GridDetail, "Weight")));

                if (TxtEnteredWeight.Text.Trim() == String.Empty)
                {
                    TxtEnteredWeight.Text = "0.000";
                }

                TxtBalance.Text = String.Format("{0:0.000}", Convert.ToDouble(TxtQty1.Text) - Convert.ToDouble(TxtEnteredWeight.Text));

                if (TxtOrderWeight.Text.Trim() == String.Empty)
                {
                    TxtOrderWeight.Text = "0.000";
                }

                DataTable Tot1 = new DataTable();
                Str = "Select Sum(Inw_Qty) Grn_Qty From fitsocks.dbo.Grn_Details_For_LOT() Where Grn_No = '" + TxtGrnNO.Text.ToString() + "' And ItemID = " + Grid["ItemID", Grid.CurrentCell.RowIndex].Value + " And ColorID = " + Grid["ColorID", Grid.CurrentCell.RowIndex].Value + " And SizeID = " + Grid["SizeID", Grid.CurrentCell.RowIndex].Value + " And Order_No = '" + GridDetail["Order_No", GridDetail.CurrentCell.RowIndex - 1].Value.ToString() + "' Group By Order_No, Item, Color, Size Having Sum(Inw_Qty) > 0.000";
                MyBase.Load_Data(Str, ref Tot1);

                if (Tot1.Rows.Count > 0)
                {
                    TxtOrderWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(Tot1.Rows[0][0]));
                }

                TxtOrderEnterWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex - 1].Value.ToString())));

                if (TxtOrderEnterWeight.Text.Trim() == String.Empty)
                {
                    TxtOrderEnterWeight.Text = "0.000";
                }

                TxtOrderBalance.Text = String.Format("{0:0.000}", Convert.ToDouble(TxtOrderWeight.Text) - Convert.ToDouble(TxtOrderEnterWeight.Text));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        
        void GridDetail_Data(Int32 Row, Double Grn_Qty)
        {
            try
            {
                if (DtQty[Row] == null)
                {
                    DtQty[Row] = new DataTable();
                    if (MyParent._New)
                    {
                        MyBase.Load_Data("Select 0 SNo, '' Order_No, '' LotNo, CAST(Null as Numeric (25, 3)) Weight, '' BagNo, '' Location, 0 LocationID, " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() + " SlNo1, '-' T From VSocks_Lot_Bag_Details A Left Join VSocks_Lot_Details B on A.Master_ID = B.Master_ID and A.SlNo1 = B.Slno1 Left Join VSocks_Lot_Master C on A.Master_ID = C.RowID and B.Master_ID = C.RowID Where A.Master_ID = " + Code + " and B.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString(), ref DtQty[Row]);
                    }
                    else
                    {
                        R = 1;
                        MyBase.Load_Data("select A.slno Sno, A.Order_No, A.LotNo, A.Weight, A.BagNo, D.Location, A.LocationID, B.Slno1,'-' T from VSocks_Lot_Bag_Details A Left Join VSocks_Lot_Details B on A.Master_ID = B.Master_ID and A.SlNo1 = B.Slno1 Left Join VSocks_Lot_Master C on A.Master_ID = C.RowID and B.Master_ID = C.RowID Left Join Socks_Yarn_Stores_Location_Master D On A.LocationID = D.RowID Where A.Master_ID = " + Code + " and B.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString(), ref DtQty[Row]);
                    }                    
                }
                GridDetail.DataSource = DtQty[Row];
                
                MyBase.Grid_Designing(ref GridDetail, ref DtQty[Row], "SlNo1", "LocationID", "T");
                MyBase.ReadOnly_Grid_Without(ref GridDetail,  "Order_No", "LotNo", "Weight", "BagNO", "Location");
                MyBase.Grid_Colouring(ref GridDetail, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridDetail, 50, 120, 120, 100, 50, 120);
                GridDetail.Columns["Weight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                GridDetail.RowHeadersWidth = 10;
                if (!MyParent._New)
                {
                    //Balance_Pieces();
                }
                GBQty.Visible = true;
                R = 0;

                if (DtQty[Row].Rows.Count == 0)
                {
                    TxtOrderWeight.Text = "";
                    TxtOrderEnterWeight.Text = "";
                    TxtEnteredWeight.Text = "";
                    TxtBalance.Text = "";
                    TxtOrderBalance.Text = "";
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
                TxtEnteredWeight.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref GridDetail, "Weight", "Order_No", "LotNo")));                

                if (TxtEnteredWeight.Text.Trim() == String.Empty)
                {
                    //TxtBalance.Text = String.Format("{0:0}", Convert.ToDouble(TxtQty.Text));
                    TxtBalance.Text = String.Format("{0:0}");
                }
                else
                {
                    Roll_Balance();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Total_Count()
        {
            try
            {
                TxtTotal.Text = MyBase.Sum(ref Grid, "Grn_Qty");
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
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Weight"].Index)
                    {
                        if (GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Weight...!", "Gainup");
                            GridDetail.CurrentCell = GridDetail["Weight", Grid.CurrentCell.RowIndex];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            return;
                        }
                        else if (Convert.ToDouble(GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value) > Math.Round(Convert.ToDouble(Fill_BOM_Check(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString())), 3))
                        {
                            e.Handled = true;
                            if (Convert.ToDouble(Fill_BOM_Check(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString())) > 0)
                            {
                                GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value = Fill_BOM_Check(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                            }
                            else
                            {
                                GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value = "0";
                            }
                            DataTable D1 = new DataTable();
                            Str = "Select Order_No, Sum(Inw_Qty) Grn_Qty, Item, Color, Size From fitsocks.dbo.Grn_Details_For_LOT() Where Grn_No = '" + TxtGrnNO.Text + "' And ItemID = " + Grid["ItemID", Grid.CurrentCell.RowIndex].Value + " And ColorID = " + Grid["ColorID", Grid.CurrentCell.RowIndex].Value + " And SizeID = " + Grid["SizeID", Grid.CurrentCell.RowIndex].Value + " And Order_No = '" + GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString() + "' Group By Order_No, Item, Color, Size Having Sum(Inw_Qty) > 0.000";
                            MyBase.Load_Data(Str, ref D1);
                            MessageBox.Show(" '" + GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString() + "' Grn_Qty = " + D1.Rows[0][1].ToString() + " Invalid Weight...!", "Gainup");
                            GridDetail.CurrentCell = GridDetail["Weight", GridDetail.CurrentCell.RowIndex];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            Roll_Balance();
                            Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                            if (Convert.ToDouble(TxtOrderBalance.Text.ToString()) == 0.000)
                            {
                                Int16 Count = 0;
                                if (listBox1.Items.Count > 0)
                                {
                                    for (int x = 0; x <= listBox1.Items.Count - 1; x++)
                                    {
                                        if (listBox1.Items[x].ToString() == GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString())
                                        {
                                            Count++;
                                        }
                                    }
                                    if (Convert.ToInt16(Count.ToString()) == 0)
                                    {
                                        listBox1.Items.Add(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                                    }
                                }
                                else
                                {
                                    listBox1.Items.Add(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                                }
                            }
                            else
                            {
                                if (listBox1.Items.Count > 0)
                                {
                                    for (int x = 0; x <= listBox1.Items.Count - 1; x++)
                                    {
                                        if (listBox1.Items[x].ToString() == GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString())
                                        {
                                            listBox1.Items.RemoveAt(x);
                                        }
                                    }
                                }
                            }
                            return;
                        }
                        else
                        {
                            Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                            if (Convert.ToDouble(TxtOrderBalance.Text.ToString()) == 0.000)
                            {
                                Int16 Count = 0;
                                if (listBox1.Items.Count > 0)
                                {
                                    for (int x = 0; x <= listBox1.Items.Count-1; x++)
                                    {
                                        if (listBox1.Items[x].ToString() == GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString())
                                        {
                                            Count++;
                                        }
                                    }
                                    if (Convert.ToInt16(Count.ToString()) == 0)
                                    {
                                        listBox1.Items.Add(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                                    }
                                }
                                else
                                {
                                    listBox1.Items.Add(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                                }
                            }
                            else
                            {
                                if (listBox1.Items.Count > 0)
                                {
                                    for (int x = 0; x <= listBox1.Items.Count - 1; x++)
                                    {
                                        if (listBox1.Items[x].ToString() == GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString())
                                        {
                                            listBox1.Items.RemoveAt(x);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Order_No"].Index)
                    {
                        if (GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Order_NO", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            if (GridDetail.CurrentCell.RowIndex > 0)
                            {
                                Roll_Balance_New();
                                Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                            }
                        }
                    }
                }
                
                Roll_Balance(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void TxtRoll_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Weight"].Index)
                {
                    MyBase.Valid_Decimal(Txt1, e);
                }
                else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["LotNo"].Index)
                {
                    if (Convert.ToInt16(Txt1.Text.Length.ToString()) < 12)
                    {
                        MyBase.Return_Ucase(e);
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
                else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["BagNo"].Index)
                {
                    MyBase.Valid_Number(Txt1, e);
                }
                else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Location"].Index)
                {
                    e.Handled = true;
                }
                else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Order_No"].Index)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void TxtRoll_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Weight"].Index)
                {
                    Roll_Balance();
                    if (GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value) == 0)
                    {
                        GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value = Convert.ToDouble(TxtBalance.Text);
                    }
                }
                else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Order_No"].Index)
                {
                    if (GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        if (Convert.ToDouble(Bal) > 0.000)
                        {
                            if (GridDetail.CurrentCell.RowIndex > 0 && (GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value != null || GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString() != String.Empty))
                            {
                                Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                            }
                        }
                        Roll_Balance();
                    }
                    else
                    {
                        TxtOrderWeight.Text = "0.000";
                        TxtOrderEnterWeight.Text = "0.000";
                        TxtOrderBalance.Text = "0.000";
                    }
                }
                else if (GridDetail.CurrentCell.RowIndex > 0 && GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["LotNo"].Index && GridDetail["LotNo", GridDetail.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                {
                    GridDetail["LotNo", GridDetail.CurrentCell.RowIndex].Value = GridDetail["LotNo", GridDetail.CurrentCell.RowIndex - 1].Value.ToString();
                    Txt1.Text = GridDetail["LotNo", GridDetail.CurrentCell.RowIndex - 1].Value.ToString();
                }
                else if (GridDetail.CurrentCell.RowIndex > 0 && GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["BagNo"].Index && GridDetail["BagNo", GridDetail.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                {
                    if (GridDetail["LotNo", GridDetail.CurrentCell.RowIndex].Value.ToString() == GridDetail["LotNo", GridDetail.CurrentCell.RowIndex - 1].Value.ToString())
                    {
                        GridDetail["BagNo", GridDetail.CurrentCell.RowIndex].Value = Convert.ToInt64(GridDetail["BagNo", GridDetail.CurrentCell.RowIndex - 1].Value.ToString()) + 1;
                        Txt1.Text = Convert.ToString(Convert.ToInt64(GridDetail["BagNo", GridDetail.CurrentCell.RowIndex - 1].Value.ToString()) + 1);
                    }
                    else
                    {
                        GridDetail["BagNo", GridDetail.CurrentCell.RowIndex].Value = "1";
                        Txt1.Text = "1";
                    }
                }
                else if (GridDetail.CurrentCell.RowIndex > 0 && GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Location"].Index && GridDetail["Location", GridDetail.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                {
                    GridDetail["Location", GridDetail.CurrentCell.RowIndex].Value = GridDetail["Location", GridDetail.CurrentCell.RowIndex - 1].Value.ToString();
                    Txt1.Text = GridDetail["Location", GridDetail.CurrentCell.RowIndex - 1].Value.ToString();
                    GridDetail["LocationID", GridDetail.CurrentCell.RowIndex].Value = GridDetail["LocationID", GridDetail.CurrentCell.RowIndex - 1].Value.ToString();
                }
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
                if (Txt1 == null)
                {
                    Txt1 = (TextBox)e.Control;
                    Txt1.KeyPress += new KeyPressEventHandler(TxtRoll_KeyPress);
                    Txt1.GotFocus += new EventHandler(TxtRoll_GotFocus);
                    Txt1.KeyDown += new KeyEventHandler(TxtRoll_KeyDown);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        Double Fill_BOM_Check(String OrderNo)
        {
            try
            {
                Double Lot_Weight = 0.000;
                Double Bal_Qty = 0.000;
                Double Grn_Qty = 0.000;
                DataTable Tdt = new DataTable();
                Str = "Select Order_No, Sum(Inw_Qty) Grn_Qty, Item, Color, Size From fitsocks.dbo.Grn_Details_For_LOT() Where Grn_No = '" + TxtGrnNO.Text + "' And ItemID = " + Grid["ItemID", Grid.CurrentCell.RowIndex].Value + " And ColorID = " + Grid["ColorID", Grid.CurrentCell.RowIndex].Value + " And SizeID = " + Grid["SizeID", Grid.CurrentCell.RowIndex].Value + " And Order_No = '" + GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString() + "' Group By Order_No, Item, Color, Size Having Sum(Inw_Qty) > 0.000";
                MyBase.Load_Data(Str, ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    Grn_Qty = Convert.ToDouble(Tdt.Rows[0]["Grn_Qty"].ToString());
                    Bal_Qty = Grn_Qty;
                    for (int i = 0; i <= DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].Rows.Count - 1; i++)
                    {
                        if (GridDetail.CurrentCell.RowIndex != i)
                        {
                            if (DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].Rows[i]["Order_No"].ToString() == OrderNo)
                            {
                                Lot_Weight = Convert.ToDouble(Lot_Weight) + Convert.ToDouble(DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].Rows[i]["Weight"].ToString());
                            }
                        }
                    }
                    Bal_Qty = Convert.ToDouble(Bal_Qty) - Convert.ToDouble(Lot_Weight);
                    Bal = Bal_Qty;
                }
                return Convert.ToDouble(Bal_Qty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Double Fill_BOM(String OrderNo)
        {
            try
            {
                Double Lot_Weight = 0.000;
                Double Bal_Qty = 0.000;
                Double Grn_Qty = 0.000;
                DataTable Tdt = new DataTable();

                DataTable D1 = new DataTable();
                Str = "Select Sum(Inw_Qty) Grn_Qty From fitsocks.dbo.Grn_Details_For_LOT() Where Grn_No = '" + TxtGrnNO.Text + "' And ItemID = " + Grid["ItemID", Grid.CurrentCell.RowIndex].Value + " And ColorID = " + Grid["ColorID", Grid.CurrentCell.RowIndex].Value + " And SizeID = " + Grid["SizeID", Grid.CurrentCell.RowIndex].Value + " And Order_No = '" + OrderNo + "' Group By Order_No, Item, Color, Size Having Sum(Inw_Qty) > 0.000";
                MyBase.Load_Data(Str, ref D1);
                if (D1.Rows.Count > 0)
                {
                    Grn_Qty = Convert.ToDouble(D1.Rows[0][0]);
                    Bal_Qty = Grn_Qty;
                    TxtOrderWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(D1.Rows[0][0]));
                }

                //for (int i = 0; i <= DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].Rows.Count - 1; i++)
                for (int i = 0; i <= GridDetail.Rows.Count - 2; i++)
                {
                    if(GridDetail["Order_No", i].Value.ToString() == OrderNo) 
                    {
                        if (GridDetail["Weight", i].Value.ToString() != String.Empty && GridDetail["Weight", i].Value.ToString() != null && Convert.ToDouble(GridDetail["Weight", i].Value.ToString()) > 0.000)
                        {
                            Lot_Weight = Convert.ToDouble(Lot_Weight) + Convert.ToDouble(GridDetail["Weight", i].Value.ToString());
                        }
                    }
                }

                TxtOrderEnterWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(Lot_Weight.ToString()));

                TxtOrderBalance.Text = String.Format("{0:0.000}", Convert.ToDouble(TxtOrderWeight.Text) - Convert.ToDouble(TxtOrderEnterWeight.Text));

                return Convert.ToDouble(Lot_Weight);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void TxtRoll_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Order_No"].Index)
                    {
                        String Down_Order = "";
                        if (listBox1.Items.Count > 0)
                        {
                            for (int x = 0; x <= listBox1.Items.Count - 1; x++)
                            {
                                if (Down_Order.ToString() == String.Empty)
                                {
                                    Down_Order = "$" + listBox1.Items[x].ToString() + "$";
                                }
                                else
                                {
                                    Down_Order = Down_Order + ",$" + listBox1.Items[x].ToString() + "$";
                                }
                            }
                        }

                        Str = "Select Order_No, Sum(Inw_Qty) Grn_Qty, Item, Color, Size From fitsocks.dbo.Grn_Details_For_LOT() Where Grn_No = '" + TxtGrnNO.Text + "' And ItemID = " + Grid["ItemID", Grid.CurrentCell.RowIndex].Value + " And ColorID = " + Grid["ColorID", Grid.CurrentCell.RowIndex].Value + " And SizeID = " + Grid["SizeID", Grid.CurrentCell.RowIndex].Value + "";

                        if (Down_Order.ToString() != String.Empty)
                        {
                            Str = Str + " And Order_No Not in (" + Down_Order.ToString().Replace("$", "'") + ")";
                        }
                        Str = Str + " Group By Order_No, Item, Color, Size Having Sum(Inw_Qty) > 0.000";
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Order..!", Str, String.Empty, 150, 100);

                        if (Dr != null)
                        {
                            if (GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            {
                                GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                                Txt1.Text = Dr["Order_No"].ToString();
                                //GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value = Dr["Grn_Qty"].ToString();
                            }
                            else
                            {
                                if (listBox1.Items.Count > 0)
                                {
                                    for (int x = 0; x <= listBox1.Items.Count - 1; x++)
                                    {
                                        if (listBox1.Items[x].ToString() == GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString())
                                        {
                                            listBox1.Items.RemoveAt(x);
                                        }
                                    }
                                }
                                GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                                Txt1.Text = Dr["Order_No"].ToString();
                                GridDetail["LotNo", GridDetail.CurrentCell.RowIndex].Value = "";
                                GridDetail["Weight", GridDetail.CurrentCell.RowIndex].Value = "0";
                                GridDetail["BagNo", GridDetail.CurrentCell.RowIndex].Value = "";
                                GridDetail["Location", GridDetail.CurrentCell.RowIndex].Value = "";
                            }
                            TxtOrderWeight.Text = Dr["Grn_Qty"].ToString();
                            TxtOrderEnterWeight.Text = "";
                            TxtOrderBalance.Text = "";
                            GridDetail["Slno1", GridDetail.CurrentCell.RowIndex].Value = Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();
                            if (GridDetail.Rows.Count > 0)
                            {
                                Fill_BOM(GridDetail["Order_No", GridDetail.CurrentCell.RowIndex].Value.ToString());
                            }
                        }
                    }
                    else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Location"].Index)
                    {
                        Str = "Select Location, Remarks, RowID LocationID from Socks_Yarn_Stores_Location_Master Order By Location";
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Order..!", Str, String.Empty, 150, 100);

                        if (Dr != null)
                        {
                            GridDetail["Location", GridDetail.CurrentCell.RowIndex].Value = Dr["Location"].ToString();
                            Txt1.Text = Dr["Location"].ToString();
                            GridDetail["LocationID", GridDetail.CurrentCell.RowIndex].Value = Dr["LocationID"].ToString();
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
                //if (listBox1.Items.Count > 0)
                //{
                //    for (int x = 0; x <= listBox1.Items.Count - 1; x++)
                //    {
                //        listBox1.Items.RemoveAt(x);
                //    }
                //}

                if (listBox1.Items.Count > 0)
                {
                    for (int a = listBox1.Items.Count - 1; a >= 0; a--)
                    {
                        listBox1.Items.RemoveAt(a);
                    }
                    listBox1.Refresh();
                }

                //if (TxtBalance.Text.Trim() == String.Empty || TxtBalance.Text != "0.00")
                if (TxtBalance.Text != "0.000")
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    GridDetail.CurrentCell = GridDetail["LotNo", 0];
                    GridDetail.Focus();                    
                    GridDetail.BeginEdit(true);
                    return;
                }
                Grid["Slno_Temp", Grid.CurrentCell.RowIndex].Value = Grid["Slno1", Grid.CurrentCell.RowIndex].Value;
                Grid["T", Grid.CurrentCell.RowIndex].Value = TxtBalance.Text;
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Remarks", (Grid.CurrentCell.RowIndex)];
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
                if (listBox1.Items.Count > 0)
                {
                    for (int a = listBox1.Items.Count - 1; a >= 0; a--)
                    {
                        listBox1.Items.RemoveAt(a);
                    }
                    listBox1.Refresh();
                }
                Grid["T", (Grid.CurrentCell.RowIndex)].Value = TxtBalance.Text;
                for (int i = 0; i <= GridDetail.Rows.Count - 2; i++)
                {
                    if (GridDetail["Weight", i].Value == DBNull.Value || Convert.ToDouble(GridDetail["Weight", i].Value) <= 0)
                    {
                        MessageBox.Show("Invalid KGS ..!", "Gainup");                        
                        //GridDetail.CurrentCell = GridDetail["LotNo", 0];
                        //GridDetail.Focus();
                        //GridDetail.BeginEdit(true);
                        Grid.CurrentCell = Grid["Grn_Qty", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        GBQty.Visible = false;                        
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)] = new DataTable();
                //DtQty = new DataTable[300];
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Remarks", (Grid.CurrentCell.RowIndex)];
                Grid.Focus();
                Grid.BeginEdit(true);
                return;
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

        private void GridDetail_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                if (GridDetail.Rows.Count > 1)
                {
                    MyBase.Row_Number(ref GridDetail);
                }
                Total_Count();
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
                    Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);
                    Txt.Leave += new EventHandler(Txt_Leave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void GridDetail_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
               
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
                //Dt.AcceptChanges();
                //GridDetail.RefreshEdit();
                //MyBase.Row_Number(ref GridDetail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
