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
    public partial class FrmSocksYarnMoqPoOrder : Form,Entry
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain Myparent;
        DataRow Dr;
        TextBox Txt = null;
        DataTable Dt = new DataTable();
        DataTable TmpDt = new DataTable();
        String Str;
        Int64 Code = 0;
        String[] Queries;
        Int32 Array_Index = 0;

        public FrmSocksYarnMoqPoOrder()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void FrmSocksYarnMoqPoOrder_Load(object sender, EventArgs e)
        {
            try
            {
                Myparent = (MDIMain)MdiParent;
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
                TxtBuyer.Focus();
                Grid_Data();
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
                //if (Myparent.UserCode == 1)
                //{
                    MyBase.Clear(this);

                    Str = "Select distinct C.LEdgeR_NAme Buyer,A.ITem ITEM,A.Color COLOR,A.Size SIZE,B.Min_Qty MIN_QTY,B.Max_Limit_Qty MAX_LIMIT_QTY,B.Re_ORd_LEvel RE_ORDER_LEVEL,B.Item_ID ITEM_ID,B.Color_ID COLOR_ID,B.SizE_ID SIZE_ID,A.PArty_code Party_COde From Socks_Yarn_MOq_PO_Qty_Limit B Left Join Socks_Yarn_planning_Fn() A On A.Party_Code=B.Party_COde And A.ItemID=B.Item_ID And A.COLORID=B.Color_ID And A.SIZEID = B.SizE_ID left join  Buyer_All_Fn() C on C.LEdgeR_code=B.Party_COde Where B.Party_COde Is not null And B.Item_ID Is not null And B.Color_ID Is not null And B.SizE_ID Is not null";
                    Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "Select Color - Edit...!", Str, String.Empty, 200,100,100,100,100,100,100,100,100,100);
                    
                    if (Dr != null)
                    {
                        Fill_Datas();
                        //TxtTotOrder.Text = Dt.Rows.Count.ToString();
                    }
                //}
                //else
                //{
                //    MessageBox.Show("You are not allowed to EDIT...", "Gainup");
                //}
                
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
                if (Myparent.UserCode == 1)
                {
                    MyBase.Clear(this);
                    //Str = "select distinct * From Socks_Yarn_MOq_PO_Qty_Limit";
                    Str = "Select distinct C.LEdgeR_NAme Buyer,A.ITem ITEM,A.Color COLOR,A.Size SIZE,B.Min_Qty MIN_QTY,B.Max_Limit_Qty MAX_LIMIT_QTY,B.Re_ORd_LEvel RE_ORDER_LEVEL,B.Item_ID ITEM_ID,B.Color_ID COLOR_ID,B.SizE_ID SIZE_ID,A.PArty_code Party_COde From Socks_Yarn_MOq_PO_Qty_Limit B Left Join Socks_Yarn_planning_Fn() A On A.Party_Code=B.Party_COde And A.ItemID=B.Item_ID And A.COLORID=B.Color_ID And A.SIZEID = B.SizE_ID left join  Buyer_All_Fn() C on C.LEdgeR_code=B.Party_COde Where B.Party_COde Is not null And B.Item_ID Is not null And B.Color_ID Is not null And B.SizE_ID Is not null";
                    Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "Select Color - Delete...!", Str, String.Empty, 200, 100, 100, 100, 100, 150, 150);
                    if (Dr != null)
                    {
                        Fill_Datas();
                        TxtBuyer.Focus();
                        Myparent.Load_DeleteConfirmEntry();
                    }
                }
                else
                {
                    MessageBox.Show("You are not allowed to DELETE...", "Gainup");
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Grid_Data()
        {
            try
            {
                if (Myparent._New==true)
                {
                    

                    Str = "Select 0 SNO,Party_COde,' ' Buyer,Item_ID ITEM_ID,Color_ID COLOR_ID,SizE_ID SIZE_ID,''ITEM,''COLOR,''SIZE, 0.00 MIN_QTY, 0.00 MAX_LIMIT_QTY, 0.00 RE_ORDER_LEVEL,convert(varchar,Item_ID)+'_'+convert(varchar,Color_ID)+'_'+convert(varchar,SizE_ID) uniqcols  From Socks_Yarn_MOq_PO_Qty_Limit where 1=2";
                
                }
                else
                {


                    Str = "Select distinct 0 SNO,A.PArty_code Party_COde,C.LEdgeR_NAme Buyer,A.ITem ITEM,A.Color COLOR,A.Size SIZE,B.Item_ID ITEM_ID,B.Color_ID COLOR_ID,B.SizE_ID SIZE_ID,B.Min_Qty MIN_QTY,B.Max_Limit_Qty MAX_LIMIT_QTY,B.Re_ORd_LEvel RE_ORDER_LEVEL,convert(varchar,Item_ID)+'_'+convert(varchar,Color_ID)+'_'+convert(varchar,SizE_ID) uniqcols From Socks_Yarn_MOq_PO_Qty_Limit B Left Join Socks_Yarn_planning_Fn() A On A.Party_Code=B.Party_COde And A.ItemID=B.Item_ID And A.COLORID=B.Color_ID And A.SIZEID = B.SizE_ID left join  Buyer_All_Fn() C on C.LEdgeR_code=B.Party_COde Where B.Party_COde Is not null And B.Item_ID Is not null And B.Color_ID Is not null And B.SizE_ID Is not null and B.Party_COde =  " + TxtBuyer.Tag + "";

                }
                Dt = new DataTable();
                Grid.DataSource = null;
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Buyer", "Party_COde", "ITEM_ID", "COLOR_ID", "SIZE_ID", "uniqcols");
                MyBase.ReadOnly_Grid(ref Grid, "SNO","COLOR","SIZE");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 50, 250, 80,80,80,120,140);
                Grid.RowHeadersWidth = 40;
                Grid.Columns["MIN_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["MAX_LIMIT_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["RE_ORDER_LEVEL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Total_Count();
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
                TxtTotOrder.Text = MyBase.Count(ref Grid, "ITEM");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void Fill_Datas()
        {
            try
            {
                TxtBuyer.Text = Dr["Buyer"].ToString();
                TxtBuyer.Tag = Convert.ToInt64(Dr["Party_COde"]).ToString();
                Grid_Data();
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                Str = "select B.LEdgeR_NAme Buyer, A.Item_ID,A.Color_ID,A.SizE_ID,A.Party_COde,A.Min_Qty MIN_QTY,A.Max_Limit_Qty MAX_LIMIT_QTY,A.Re_ORd_LEvel RE_ORDER_LEVEL from Socks_Yarn_MOq_PO_Qty_Limit A left join Buyer_All_Fn() B on A.Party_COde=B.LEdgeR_code where A.Min_Qty is not null and A.Max_Limit_Qty is not null and A.Re_ORd_LEvel is not null";
                Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "Select Quantity - View...!", Str, String.Empty, 200,100,100);
                if (Dr != null)
                {
                    Fill_Datas();
                    TxtBuyer.Text = Dr["Buyer"].ToString();
                    TxtBuyer.Focus();
                  //  TxtTotOrder.Text = Dt.Rows.Count.ToString();
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
                String[] Queries;
                Int32 Array_Index = 0;
                DataTable chkdata = new DataTable();
                string check;

                Queries = new String[(Dt.Rows.Count + 7)];

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("invalid details...", "Gainup");
                    Myparent.Save_Error = true;
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 1; j++)
                    {
                        if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty || Grid[j,i].Value.ToString()=="0")
                        {
                            if (Grid.Columns[j].Name != "RE_ORDER_LEVEL")
                            {
                                MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                Grid.CurrentCell = Grid[j, i];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                Myparent.Save_Error = true;
                                return;
                            }
                        }

                    }
                }


                if (Myparent.Edit == true)
                {
                    Queries[Array_Index++] = "delete from Socks_Yarn_MOq_PO_Qty_Limit where Party_COde=" + TxtBuyer.Tag + "";
                    Queries[Array_Index++] = Myparent.EntryLog("MOQ PO SETTINGS", "EDIT", TxtBuyer.Tag.ToString());
                }

                else
                {

                    for (int k = 0; k <= Dt.Rows.Count - 1; k++)
                    {
                        
                        Queries[Array_Index++] = "insert into Socks_Yarn_MOq_PO_Qty_Limit (Party_COde,Item_ID,Color_ID,SizE_ID,Min_Qty,Max_Limit_Qty,Re_ORd_LEvel) values(" + TxtBuyer.Tag+ "," + Grid["Item_ID", k].Value.ToString() + "," + Grid["Color_ID", k].Value.ToString() + "," + Grid["SizE_ID", k].Value.ToString() + "," + Grid["Min_Qty", k].Value.ToString() + "," + Grid["Max_Limit_Qty", k].Value.ToString() + "," + Grid["RE_ORDER_LEVEL", k].Value.ToString() + ")";
                        
                    }                        
                }
                if (Myparent._New == true)
                {
                    Queries[Array_Index++] = Myparent.EntryLog("MOQ PO SETTINGS", "NEW", TxtBuyer.Tag.ToString());
                }
                
             
                    MyBase.Run(Queries);
                    MessageBox.Show("Saved..!", "Gainup");
                    MyBase.Clear(this);
                    TxtBuyer.Focus();
                    Myparent.Save_Error = false;
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Myparent.Save_Error = true;
            }
        }
        public void Entry_Delete_Confirm()
        {
            try
            {
                if (Convert.ToInt64(TxtBuyer.Tag) > 0 && Dt.Rows.Count > 0)
                {
                    MyBase.Run("delete from Socks_Yarn_MOq_PO_Qty_Limit where Party_COde=" + TxtBuyer.Tag);
                    MessageBox.Show("Deleted ...!", "Gainup");
                    Myparent.Load_DeleteEntry();
                }
                else
                {
                    MessageBox.Show("Invalid Entry to Delete ...!", "Gainup");
                    Myparent.Load_DeleteEntry();
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

        private void FrmSocksYarnMoqPoOrder_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "TxtBuyer")
                    {

                        Grid.CurrentCell = Grid["ITem", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        
                    }
                    else if (this.ActiveControl.Name == "TxtTotOrder")
                    {
                        if (Myparent._New == true || Myparent.Edit == true)
                        {
                            Myparent.Load_SaveEntry();
                            return;
                        }
                    }
                    else
                    {
                        SendKeys.Send("{TAB}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtBuyer")
                    {

                        Str = "Select LEdgeR_NAme Buyer, LEdger_Code  From Buyer_All_Fn() ORder by 1 ";
                        Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "Select BUYER...!", Str, String.Empty, 250);
                        if (Dr != null)
                        {
                            TxtBuyer.Text = Dr["Buyer"].ToString();
                            TxtBuyer.Tag = Convert.ToInt64(Dr["LEdger_Code"]).ToString();
                            //while (Dt.Rows.Count > 0)
                            //{
                            //    Dt.Rows.RemoveAt(0);
                            //}
                            //TxtBuyer.Focus();
                            //TxtTotOrder.Text = "0";
                            //TxtTotOrder.Focus();
                        }

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
                    Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);
                    Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                    //Txt.GotFocus += new EventHandler(Txt_GotFocus);
                    Txt.Leave += new EventHandler(Txt_Leave);
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
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ITem"].Index)
                        {
                            if (Myparent._New == true)
                            {
                                Str = "Select A.ITem,A.Color,A.Size,A.ItemID,A.ColorID,A.SizeID,A.Party_Code,B.Min_Qty,B.Max_Limit_Qty,B.Re_ORd_LEvel,convert(varchar,A.ItemID)+'_'+convert(varchar,A.ColorID)+'_'+convert(varchar,A.SizeID) uniqcols From Socks_Yarn_planning_Fn() A Left Join Socks_Yarn_MOq_PO_Qty_Limit B On A.Party_Code=B.Party_COde And A.ItemID=B.Item_ID And A.COLORID=B.Color_ID And A.SIZEID = B.SizE_ID Where B.Party_COde Is null And B.Item_ID Is null And B.Color_ID Is null And B.SizE_ID Is null and B.Min_Qty is null and B.Max_Limit_Qty is null and B.Re_ORd_LEvel is null and A.Party_Code=" + TxtBuyer.Tag + "";
                                Dr = Tool.Selection_Tool_Except_New("uniqcols", this, 100, 100, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select BUYER...!", Str, String.Empty, 150, 75, 75);
                                if (Dr != null)
                                {
                                    Txt.Text = Dr["ITem"].ToString();
                                    Grid["ITEM_ID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                                    Grid["COLOR_ID", Grid.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();
                                    Grid["SIZE_ID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                                    Grid["Party_COde", Grid.CurrentCell.RowIndex].Value = Dr["Party_code"].ToString();
                                    Grid["ITEM", Grid.CurrentCell.RowIndex].Value = Dr["ITem"].ToString();
                                    Grid["COLOR", Grid.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                                    Grid["SIZE", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                                    Grid["Buyer", Grid.CurrentCell.RowIndex].Value = "_";
                                    Grid["uniqcols", Grid.CurrentCell.RowIndex].Value = Dr["uniqcols"].ToString();
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
        
        void Txt_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["RE_ORDER_LEVEL"].Index)
                {
                    if (Grid["RE_ORDER_LEVEL", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                    {
                        //TxtTotOrder.Text = (Dt.Rows.Count+1).ToString();
                        Total_Count();
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
                if ((Grid.CurrentCell.ColumnIndex == Grid.Columns["MIN_QTY"].Index)||(Grid.CurrentCell.ColumnIndex == Grid.Columns["MAX_LIMIT_QTY"].Index)||(Grid.CurrentCell.ColumnIndex == Grid.Columns["RE_ORDER_LEVEL"].Index))
                {
                    //MyBase.Valid_Decimal(Txt, e);
                    MyBase.Valid_Number(Txt, e);
                    
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ITEM"].Index)
                {
                    e.Handled = true; 
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
                if (e.KeyCode == Keys.Escape)
                {
                    if (this.ActiveControl.Name == "Grid")
                    {
                        if (Myparent._New == true || Myparent.Edit == true)
                        {
                            //Myparent.Load_SaveEntry();
                            //while (Dt.Rows.Count > 0)
                            //{
                            //    Dt.Rows.RemoveAt(0);
                            //}
                            //TxtBuyer.Focus();
                            //TxtTotOrder.Text = "0";
                            TxtTotOrder.Focus();
                        }
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if(e.KeyCode==Keys.Enter)
                {
                     if (Grid.CurrentCell.ColumnIndex == Grid.Columns["MIN_QTY"].Index)
                    {
                        if (Grid["MIN_QTY", Grid.CurrentCell.RowIndex].Value == null || Grid["MIN_QTY", Grid.CurrentCell.RowIndex].Value.ToString() == string.Empty || Convert.ToInt64(Grid["MIN_QTY", Grid.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("MIN_QTY should not be EMPTY and {0.00}...!", "Gainup");
                            //Grid["MIN_QTY", Grid.CurrentCell.RowIndex].Value = "0.00";
                            Grid.CurrentCell = Grid["MIN_QTY", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;   
                        }
                        //MyBase.Valid_Number(Txt,e);
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["MAX_LIMIT_QTY"].Index)
                    {
                        if (Grid["MAX_LIMIT_QTY", Grid.CurrentCell.RowIndex].Value == null || Grid["MAX_LIMIT_QTY", Grid.CurrentCell.RowIndex].Value.ToString() == string.Empty || Convert.ToInt64(Grid["MAX_LIMIT_QTY", Grid.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("MAX_LIMIT_QTY should not be EMPTY and {0.00}...!", "Gainup");
                            //Grid["MAX_LIMIT_QTY", Grid.CurrentCell.RowIndex].Value = "0.00";
                            Grid.CurrentCell = Grid["MAX_LIMIT_QTY", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }

                    }
                     else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["RE_ORDER_LEVEL"].Index)
                     {
                         if (Grid["RE_ORDER_LEVEL", Grid.CurrentCell.RowIndex].Value == null || Grid["RE_ORDER_LEVEL", Grid.CurrentCell.RowIndex].Value.ToString() == string.Empty || Convert.ToInt64(Grid["RE_ORDER_LEVEL", Grid.CurrentCell.RowIndex].Value) == 0)
                         {
                             //e.Handled = true;
                             //MessageBox.Show("RE_ORDER_LEVEL should not be EMPTY and {0.00}...!", "Gainup");
                             Grid["RE_ORDER_LEVEL", Grid.CurrentCell.RowIndex].Value = "0";
                             //Grid.CurrentCell = Grid["RE_ORDER_LEVEL", Grid.CurrentCell.RowIndex];
                             //Grid.Focus();
                             //Grid.BeginEdit(true);
                             //return;
                         }

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
                if (Grid.Rows.Count > 1)
                {
                    MyBase.Row_Number(ref Grid);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //int rowcount = 0;
            try
            {
                if (Dt.Rows.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Sure to delete..?", "Confirm_Delete_Gainup", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                    }
                    Total_Count();
                    
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmSocksYarnMoqPoOrder_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtBuyer")
                {
                    e.Handled = true;
                }
                else if (this.ActiveControl.Name=="TxtTotOrder")
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



    }
}
