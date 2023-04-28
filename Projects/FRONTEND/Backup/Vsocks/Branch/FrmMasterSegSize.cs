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
    public partial class FrmMasterSegSize : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataGrid dgv = new DataGrid();
        DataRow Dr;
        TextBox Txt = null;
        String[] Queries;
        public FrmMasterSegSize()
        {
            InitializeComponent();
        }

        private void FrmMasterSegSize_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                TxtSize.Focus();
                Grid_Data();

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
                //DtpEDate.Focus();
                TxtSize.Focus();
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
                Total_Count();
                if (TxtSize.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotOrder.Text) == 0)
                {
                    MessageBox.Show("Invalid Size Details", "Gainup");
                    Grid.CurrentCell = Grid["Size", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 1; j++)
                    {
                        if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty)
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
                Queries = new String[Dt.Rows.Count * 250];

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New == true)
                    {
                        Queries[Array_Index++] = "Exec Update_Master_Seggregation 'Size', 'Sizeid', " + TxtSize.Tag + ", " + Grid["Sizeid", i].Value + " ";
                        Queries[Array_Index++] = "Exec Update_Master_Seggregation_Yarn_Plan_Det 'Size', 'Sizeid', " + TxtSize.Tag + ", " + Grid["Sizeid", i].Value + " ";
                        Queries[Array_Index++] = "update Size Set Size = 'ZZZ' + Size where SizeID = " + Grid["Sizeid", i].Value + " ";
                    }
                }

                MyBase.Run(Queries);
                MyParent.Save_Error = false;
                MessageBox.Show("Saved ..!", "Gainup");

                MyBase.Clear(this);
                Grid_Data();
                TxtSize.Focus();
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_View()
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

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                Grid_Data();
                //DtpEDate.Focus();
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
                MyBase.Clear(this);
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
                Str = "Select 0 as SNO, Size, Sizeid From Fitsocks.Dbo.Size Where  1=2 ";
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid(ref Grid, "SNO", "Sizeid");
                MyBase.Grid_Designing(ref Grid, ref Dt);
                MyBase.Grid_Width(ref Grid, 50, 300);
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Size"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Size"].Index)
                    {
                        Dr = Tool.Selection_Tool_Except_New("Size", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Size", "Select Size, Sizeid From Fitsocks.Dbo.Size Where Item_Type='Yarn' And Sizeid <> " + TxtSize.Tag + " And Size not like 'zzz%' Order By Size", String.Empty, 200, 120);

                        if (Dr != null)
                        {
                            Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                            Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                            Txt.Text = Dr["Size"].ToString();
                            
                        }
                    }
                }
                Total_Count();
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
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
                TxtTotOrder.Text = MyBase.Count(ref Grid, "Size");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {

        }        

        
        
        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Total_Count();
                    TxtTotOrder.Focus();
                    return;
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

        private void FrmMasterSegSize_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtTotOrder")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtSize")
                    {
                        Grid.CurrentCell = Grid["Size", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }

                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtSize")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size", "Select Size, Sizeid From Fitsocks.Dbo.Size Where Item_Type='Yarn' And Size not like 'ZZZ%' Order By Size", string.Empty, 200, 90);
                        if (Dr != null)
                        {
                            TxtSize.Text = Dr["Size"].ToString();
                            TxtSize.Tag = Dr["Sizeid"].ToString();
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

        private void FrmMasterSegSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {
                if (this.ActiveControl.Name != String.Empty)
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
            }
        }
        
        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
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


    }
}
