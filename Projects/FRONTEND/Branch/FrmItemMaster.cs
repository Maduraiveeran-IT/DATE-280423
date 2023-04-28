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
    public partial class FrmItemMaster : Form, Entry
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
        Int64 rowid = 0;
        

        public FrmItemMaster()
        {
            InitializeComponent();
        }

        private void FrmItemMaster_Load(object sender, EventArgs e)
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
                //DtpDate1.Focus();
                //label2.Focus();
                Grid_Data();
                Grid.CurrentCell = Grid["ITEM", 0];
                Grid.Focus();
                Grid.BeginEdit(true);
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
                Boolean flag = false;
                String[] Queries;
                Int32 Array_Index = 0;
                DataTable chkdata = new DataTable();
                string check;

                Queries = new String[(Dt.Rows.Count + 7)];
 

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {

                    if (Grid["ITEM", i].Value.ToString() == string.Empty)
                    {
                        MessageBox.Show("Item should not be empty...!", "Gainup");
                        Grid.CurrentCell = Grid["ITEM", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        Myparent.Save_Error = true;
                        //return;
                        flag = true;
                    }
                }


                if (Myparent._New)
                {
                    for (int j = 0; j <= Dt.Rows.Count - 1; j++)
                    {
                        check = "select item from PROJECTS.dbo.Item where item= '" + Grid["ITEM", j].Value.ToString() + "' ";

                        MyBase.Load_Data(check, ref chkdata);

                        if (chkdata.Rows.Count > 0)
                        {
                            MessageBox.Show("Item already exit");
                            Grid.CurrentCell = Grid["ITEM", j];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            Myparent.Save_Error = true;
                            return;

                        }

                    }


                    for (int k = 0; k <= Dt.Rows.Count - 1; k++)
                    {
                        Queries[Array_Index++] = "insert into PROJECTS.dbo.item(item,Calc,Testing)values('" + Grid["ITEM", k].Value.ToString() + "','" + Grid["Calc", k].Value.ToString() + "','" + Grid["Testing", k].Value.ToString() + "')";
                    }

                }
                else
                {

                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        Queries[Array_Index++] = "update PROJECTS.dbo.item set item='" + Grid["ITEM", i].Value.ToString() + "',Calc='" + Grid["Calc", i].Value.ToString() + "',Testing='" + Grid["Testing", i].Value.ToString() + "' where item_id='" + Code + "' and itemid='" + rowid + "'";
                    }
                }
                if (flag == false)
                {
                    MyBase.Run(Queries);
                    MessageBox.Show("Saved..!", "Gainup");
                    MyBase.Clear(this);
                    Myparent.Save_Error = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Myparent.Save_Error = true;
            }
        }

        public void Entry_Edit()
        {
            try
            {
                    MyBase.Clear(this);
                    Str = "select  ITEM, CALC, TESTING, ITEMID, '' T from PROJECTS.dbo.item ";
                    Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "Select Item - Edit...!", Str, String.Empty,150,100,100);
                    if (Dr != null)
                    {
                        Fill_Datas();
                        //label2.Focus();
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
                if (Myparent.UserCode == 1)
                {
                    MyBase.Clear(this);
                    Str = "select  ITEM, CALC, TESTING, ITEMID, '' T from PROJECTS.dbo.item ";
                    Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "Select Item - Delete...!", Str, String.Empty, 150,100,100);
                    if (Dr != null)
                    {
                        Fill_Datas();
                        //DtpDate1.Focus();
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
                if (Myparent._New)
                {
                    Str = "select 0 SNO, ITEM,'' ITEMID, CALC, TESTING, '' T from PROJECTS.dbo.item where 1=2";
                }
                else
                {
                    Str = "select 0 SNO, ITEM, ITEMID, CALC, TESTING, '' T from PROJECTS.dbo.item where Itemid = '" + Code + "'";
                }
                Dt = new DataTable();
                Grid.DataSource = null;
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "ITEMID", "T");
                MyBase.ReadOnly_Grid(ref Grid, "SNO");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 50, 280, 150,150);
                Grid.RowHeadersWidth = 40;
                Grid.Columns["CALC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["TESTING"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Total_Count();
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
                Code = Convert.ToInt64(Dr["Itemid"]);
                //rowid = Convert.ToInt64(Dr["itemid"].ToString());
                Grid_Data();
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Delete_Confirm()
        {
            try
            {
                if (Code > 0 && Dt.Rows.Count > 0)
                {
                    Str = "delete from PROJECTS.dbo.item where itemID='" + Code + "' ";
                    MyBase.Run(Str);
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

        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                Str = "select  ITEM, CALC, TESTING, ITEMID, '' T from PROJECTS.dbo.item ";
                Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "Select Item - View...!", Str, String.Empty, 150,100,100);
                if (Dr != null)
                {
                    Fill_Datas();
                    //DtpDate1.Focus();
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

        private void FrmItemMaster_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "label2")
                    {
                        Grid.CurrentCell = Grid["ITEM", 0];
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
                    Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                   // Txt.GotFocus += new EventHandler(Txt_GotFocus);
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ITEM"].Index)
                {
                    MyBase.Return_Ucase(e);
                    if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && e.KeyChar != '-')
                    {
                        e.Handled = true;
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["TESTING"].Index)
                {
                    e.Handled = false;
                    MyBase.Valid_T_OR_F(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["CALC"].Index)
                {
                    e.Handled = false;
                    MyBase.Valid_M_OR_A(Txt, e);
                }
                else
                {
                    //
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
                //SendKeys.Send("{TAB}");       
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
                double c1 = Convert.ToDouble(TxtTotOrder.Text);
                c1 = c1 - 1;
                TxtTotOrder.Text = c1.ToString();
                
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
                    //
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "Grid")
                    {
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["CALC"].Index)
                        {
                            if (Grid["item", Grid.CurrentCell.RowIndex].Value == null || Grid["item", Grid.CurrentCell.RowIndex].Value.ToString() == string.Empty)
                            {
                                e.Handled = true;
                                MessageBox.Show("Item should not be empty...!", "Gainup");
                                Grid.CurrentCell = Grid["itemtype", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["TESTING"].Index)
                        {
                            if (Grid["CALC", Grid.CurrentCell.RowIndex].Value == null || Grid["CALC", Grid.CurrentCell.RowIndex].Value.ToString() == string.Empty)
                            {
                                e.Handled = true;
                                MessageBox.Show("CALC should not be empty...!", "Gainup");
                                Grid.CurrentCell = Grid["itemgroup", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                        }
                        else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["item"].Index)
                        {
                            if(e.KeyCode==Keys.Return)
                            {
                                if (Grid.CurrentCell.RowIndex == 0)
                                {
                                    if (Grid["item", Grid.CurrentCell.RowIndex].Value == null || Grid["item", Grid.CurrentCell.RowIndex].Value.ToString() == string.Empty)
                                    {
                                        e.Handled = true;
                                        MessageBox.Show("Item should not be empty...!", "Gainup");
                                        Grid.CurrentCell = Grid["item", Grid.CurrentCell.RowIndex];
                                        Grid.Focus();
                                        Grid.BeginEdit(true);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (Grid.CurrentCell.Value == DBNull.Value)
                                    {
                                        TxtTotOrder.Focus();
                                        e.Handled = true;
                                        return;
                                    }
                                }
                                
                            }

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
            try
            {
                if (Dt.Rows.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Sure to delete..?", "Confirm_Delete_Gainup", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                        MessageBox.Show("Selected row has been deleted");
                    }
                    Total_Count();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Arrow2_Click(object sender, EventArgs e)
        {

        }

        private void FrmItemMaster_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtTotOrder")
                {
                    e.Handled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
