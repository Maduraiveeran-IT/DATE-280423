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
    public partial class FrmSocks_LotEntry : Form, Entry
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
        public FrmSocks_LotEntry()
        {
            InitializeComponent();
        }

        private void FrmSocks_LotEntry_Load(object sender, EventArgs e)
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
                MyBase.Load_Data("select supplier, color, LotNo, bagno, (case when Item ='Cotton' then 'Combed Cotton' else Item end) Item, size Count, weight Quantity, isnull(buyer,'-') buyer, Qc_Status, Order_No,'-' PO_No, Grn_No, Grn_Date,(case when Order_No='GENERAL' then 'OGENERAL' else Substring(Order_No,8,5) end) OCN, Substring(Grn_No,8,5) GRN, RowID from Barcode_Details() where RowID = " + Code + " ", ref Tdt);
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
                    if (Tdt.Rows[i]["OCN"].ToString() == "OGENERAL")
                    {
                        Str = String.Format("{0:00000000}", Tdt.Rows[i]["OCN"]) + String.Format("{0:00000}", Convert.ToDouble(Tdt.Rows[i]["GRN"])) + String.Format("{0:00000}", Tdt.Rows[i]["LotNo"]) + String.Format("{0:000}", Convert.ToDouble(Tdt.Rows[i]["bagno"]));
                    }
                    else
                    {
                        Str = String.Format("{0:OCN00000}", Convert.ToDouble(Tdt.Rows[i]["OCN"])) + String.Format("{0:00000}", Convert.ToDouble(Tdt.Rows[i]["GRN"])) + String.Format("{0:00000}", Tdt.Rows[i]["LotNo"]) + String.Format("{0:000}", Convert.ToDouble(Tdt.Rows[i]["bagno"]));
                    }
                    Sr.WriteLine("B200,270,0,1,2,4,61,B," + Convert.ToChar(34) + Str + Convert.ToChar(34));
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Lot Entry - Edit", "select A.ENo, A.EDate, A.Grn_No, B.supplier, D.item, E.color, F.size, C.Grn_Qty, C.No_Of_Bags, A.Remarks, A.Supplierid, A.Rowid from Socks_Lot_Master A Left Join FITSOCKS.dbo.Supplier B on A.Supplierid = B.Supplierid Left Join Socks_Lot_Details C on A.RowID = C.Master_ID Left Join Item D on C.ItemID = D.itemid Left Join Color E on C.ColorID = E.Colorid Left Join Size F on C.SizeID = F.sizeid ", String.Empty, 100, 100, 150, 350, 200, 150, 100, 100, 100, 350);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["No_Of_Bags", 0];
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
                        Str = "select 0 as Slno,A.Order_No,B.Item,C.Color,D.Size,0 No_Of_bags,A.Inw_Qty Grn_Qty,A.Itemid,A.Colorid,A.Sizeid,0 Slno1, 0 Slno_Temp from fitsocks.dbo.Grn_Details_New()A Left Join fitsocks.dbo.Item B on A.itemid = B.itemid Left Join fitsocks.dbo.Color C on A.Colorid = C.Colorid Left Join fitsocks.dbo.Size D on A.Sizeid = D.Sizeid Where B.Item_Type='YARN' and 1 = 2 ";
                    }
                    else
                    {
                        Str = "select 0 as Slno,A.Order_No,A.Item,A.Color,A.Size,0 No_Of_bags,A.Inw_Qty Grn_Qty,A.Itemid,A.Colorid,A.Sizeid,0 Slno1, 0 Slno_Temp from fitsocks.dbo.Grn_Details_New()A where A.Grn_No='" + TxtGrnNO.Text + "'";
                    }
                }
                else
                {
                    Str = "select A.Slno,A.Order_No,B1.Item,C.Color,D.Size,A.No_Of_Bags,A.Grn_Qty,A.Itemid,A.Colorid,A.Sizeid,A.Slno1, A.Slno1 Slno_Temp from Socks_Lot_Details A Left Join Socks_Lot_Master B on B.RowID = A.Master_ID  Left Join Item B1 on A.itemid = B1.itemid Left Join Color C on A.Colorid = C.Colorid Left Join Size D on A.Sizeid = D.Sizeid where B.Grn_No='" + TxtGrnNO.Text + "'";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid, "No_Of_Bags");
                MyBase.Grid_Designing(ref Grid, ref Dt, "ItemID", "SizeID", "ColorID", "Slno1","Slno_Temp");
                MyBase.Grid_Width(ref Grid, 50,150,300, 200, 100,100, 100);
                Grid.Columns["Grn_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["No_Of_bags"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
                    for (int j = 1; j < Dt.Columns.Count - 5; j++)
                    {
                        if (Grid["No_Of_Bags", i].Value == DBNull.Value || Grid["No_Of_Bags", i].Value.ToString() == String.Empty || Grid["No_Of_Bags", i].Value.ToString() == "0")
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
                        MessageBox.Show("Invalid Bagwise Breakup Details ...!", "Gainup");
                        MyParent.Save_Error = true;
                        Grid.CurrentCell = Grid["No_Of_Bags", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                }

                TxtEntryNo.Text = MyBase.MaxOnlyComp("Socks_Lot_Master", "ENo", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();
                Queries = new string[Dt.Rows.Count + 100000];                
                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_Lot_Master (ENo, EDate, Grn_No, Remarks,  Company_Code, Year_Code,User_Code,Supplierid) values (" + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + TxtGrnNO.Text + "','" + TxtRemarks.Text + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "'," + MyParent.UserCode + "," + TxtSupplier.Tag.ToString() + "); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("Lot Entry", "ADD", "@@IDENTITY");

                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_Lot_Master Set EDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', Remarks = '" + TxtRemarks.Text + "',Company_Code=" + MyParent.CompCode + " , Year_Code='" + MyParent.YearCode + "',User_Code=" + MyParent.UserCode + " Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Lot Entry", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from Socks_Lot_Details where Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete from Socks_Lot_Bag_Details where Master_ID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Socks_Lot_Details (Master_ID, Slno, Order_No, ItemID, SizeID, ColorID, Grn_Qty,Slno1,No_Of_Bags) Values (@@IDENTITY, " + Grid["Slno", i].Value + ",'" + Grid["Order_No", i].Value + "',  " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ", " + Grid["Grn_Qty", i].Value + "," + Grid["Slno", i].Value + "," + Grid["No_Of_Bags", i].Value + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Socks_Lot_Details (Master_ID, Slno, Order_No, ItemID, SizeID, ColorID, Grn_Qty,Slno1,No_Of_Bags) Values (" + Code + ", " + Grid["Slno", i].Value + ",'" + Grid["Order_No", i].Value + "', " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ", " + Grid["Grn_Qty", i].Value + ", " + Grid["Slno", i].Value + "," + Grid["No_Of_Bags", i].Value + ")";
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
                                Queries[Array_Index++] = "Insert Into Socks_Lot_Bag_Details (slno,Master_ID, LotNo,BagNo, Weight, SlNo1) Values ( " + DtQty[i].Rows[j]["Sno"].ToString() + ",@@IDENTITY, '" + DtQty[i].Rows[j]["LotNo"].ToString() + "'," + DtQty[i].Rows[j]["BagNo"].ToString() + "," + DtQty[i].Rows[j]["Weight"].ToString() + "," + DtQty[i].Rows[j]["Slno1"].ToString() + ")";
                            }
                            else
                            {
                                Queries[Array_Index++] = "Insert Into Socks_Lot_Bag_Details (slno,Master_ID, LotNo,BagNo,  Weight, SlNo1) Values ( " + DtQty[i].Rows[j]["Sno"].ToString() + "," + Code + ", '" + DtQty[i].Rows[j]["LotNo"].ToString() + "'," + DtQty[i].Rows[j]["BagNo"].ToString() + ", " + DtQty[i].Rows[j]["Weight"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + ")";
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Lot Entry - Delete", "select A.ENo, A.EDate, A.Grn_No, B.supplier, D.item, E.color, F.size, C.Grn_Qty, C.No_Of_Bags, A.Remarks, A.Supplierid, A.Rowid from Socks_Lot_Master A Left Join FITSOCKS.dbo.Supplier B on A.Supplierid = B.Supplierid Left Join Socks_Lot_Details C on A.RowID = C.Master_ID Left Join Item D on C.ItemID = D.itemid Left Join Color E on C.ColorID = E.Colorid Left Join Size F on C.SizeID = F.sizeid ", String.Empty, 100, 100, 150, 350, 200, 150, 100, 100, 100, 350);
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
                    MyBase.Run("Delete from Socks_Lot_Bag_Details where Master_ID = " + Code, "Delete from Socks_Lot_Details where Master_ID = " + Code, "Delete From Socks_Lot_Master Where RowID = " + Code, MyParent.EntryLog("Lot Entry", "DELETE", Code.ToString()));
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Lot Entry - View", "select A.ENo, A.EDate, A.Grn_No, B.supplier, D.item, E.color, F.size, C.Grn_Qty, C.No_Of_Bags, A.Remarks, A.Supplierid, A.Rowid from Socks_Lot_Master A Left Join FITSOCKS.dbo.Supplier B on A.Supplierid = B.Supplierid Left Join Socks_Lot_Details C on A.RowID = C.Master_ID Left Join Item D on C.ItemID = D.itemid Left Join Color E on C.ColorID = E.Colorid Left Join Size F on C.SizeID = F.sizeid ", String.Empty, 100, 100, 150, 350, 200, 150, 100, 100, 100, 350);
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

        private void FrmSocks_LotEntry_KeyDown(object sender, KeyEventArgs e)
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
                            Grid.CurrentCell = Grid["No_Of_Bags", 0];
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

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Grn No..!", "Select Distinct Grn_No,Supplier,Supplierid From fitsocks.dbo.Grn_Details_New() where Grn_No not in(select Grn_No from Socks_Lot_Master)", String.Empty, 200, 350);
                        
                        if (Dr != null)
                        {
                            TxtGrnNO.Text = Dr["Grn_No"].ToString();
                            TxtSupplier.Text = Dr["Supplier"].ToString();
                            TxtSupplier.Tag = Dr["Supplierid"].ToString();
                            
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

        private void FrmSocks_LotEntry_KeyPress(object sender, KeyPressEventArgs e)
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["No_of_Bags"].Index)
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
                       
                        
                        GridDetail_Data(Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value), Convert.ToInt32(Grid["No_of_Bags", Grid.CurrentCell.RowIndex].Value));
                        GridDetail.CurrentCell = GridDetail["LotNo", 0];
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["No_Of_Bags"].Index)
                {
                    MyBase.Valid_Number(Txt, e);
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
                //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["No_Of_Bags"].Index)
                //{
                //    if ((Grid["No_Of_Bags", Grid.CurrentCell.RowIndex].Value.ToString()) != String.Empty)
                //    {
                //        if (Convert.ToDouble(Grid["No_Of_Bags", Grid.CurrentCell.RowIndex].Value) >0)
                //        {
                //            GridDetail.CurrentCell = GridDetail["LotNo",0];
                //            GridDetail.Focus();
                //            GridDetail.BeginEdit(true);
                //            MyParent.Save_Error = true;
                //            return;
                //        }
                //        else
                //        {
                //            MessageBox.Show("Invalid No Of Bags..!", "Gainup");
                //            Grid.CurrentCell = Grid["No_Of_Bags", Grid.CurrentCell.RowIndex];
                //            Grid.Focus();
                //            Grid.BeginEdit(true);
                //            MyParent.Save_Error = true;
                //            return;
                //        }
                //    }
                //}
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

                TxtEnteredWeight.Text = String.Format("{0:0.000}", Convert.ToDouble(MyBase.Sum_With_Three_Digits(ref GridDetail, "Weight", "BagNo")));

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
        void GridDetail_Data(Int32 Row, Int32 No_Of_bags)
        {

            try
            {
                if (DtQty[Row] == null)
                {
                    DtQty[Row] = new DataTable();
                    if (MyParent._New)
                    {
                        MyBase.Load_Data("Select No SNo, '0' LotNo, 0 BagNo, CAST(Null as Numeric (25, 3)) Weight," + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() + " SlNo1 From Number_Series (1, " + No_Of_bags + ") ", ref DtQty[Row]);
                    }
                    else
                    {
                        MyBase.Load_Data("select A.slno Sno,A.LotNo,A.BagNo,A.Weight,B.Slno1 from Socks_Lot_Bag_Details A Left Join Socks_Lot_Details B on A.Master_ID = B.Master_ID and A.SlNo1 = B.Slno1 Left Join Socks_Lot_Master C on A.Master_ID = C.RowID and B.Master_ID = C.RowID  Where  A.Master_ID =  " + Code + " and  B.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString(), ref DtQty[Row]);
                    }                    
                }
                GridDetail.DataSource = DtQty[Row];
                MyBase.Grid_Designing(ref GridDetail, ref DtQty[Row], "SlNo1");
                MyBase.ReadOnly_Grid_Without(ref GridDetail,  "LotNo", "BagNo", "Weight");
                MyBase.Grid_Colouring(ref GridDetail, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridDetail, 50, 120, 120, 100);
                GridDetail.Columns["Weight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                GridDetail.RowHeadersWidth = 10;

                if (!MyParent._New)
                {
                    //Balance_Pieces();
                }

                GBQty.Visible = true;

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
                TxtEnteredWeight.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref GridDetail, "Weight", "LotNo", "BagNo")));                

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
                TxtTotal.Text = MyBase.Sum(ref Grid, "Grn_Qty", "No_Of_Bags");
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
                else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["BagNo"].Index)
                {
                    MyBase.Valid_Number(Txt1, e);
                }
                else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Lotno"].Index)
                {
                    if (Convert.ToInt16(Txt1.Text.Length.ToString()) < 5)
                    {
                        e.Handled = false;
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
                    GBQty.Visible = false;
                    Grid.CurrentCell = Grid["No_Of_bags", (Grid.CurrentCell.RowIndex+1)];
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
                for (int i = 0; i <= GridDetail.Rows.Count - 1; i++)
                {
                    if (GridDetail["Weight", i].Value == DBNull.Value || Convert.ToDouble(GridDetail["Weight", i].Value) <= 0)
                    {
                        MessageBox.Show("Invalid KGS ..!", "Gainup");                        
                        //GridDetail.CurrentCell = GridDetail["LotNo", 0];
                        //GridDetail.Focus();
                        //GridDetail.BeginEdit(true);
                        Grid.CurrentCell = Grid["No_Of_bags", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        GBQty.Visible = false;                        
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                DtQty = new DataTable[300];
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Grn_Qty", (Grid.CurrentCell.RowIndex)];
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
                MyBase.Row_Number(ref Grid);
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



    }
}
