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
    public partial class FrmMasterSegColor : Form, Entry
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
        public FrmMasterSegColor()
        {
            InitializeComponent();
        }

        private void FrmMasterSegColor_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                TxtColor.Focus();
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
                TxtColor.Focus();
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
                if (TxtColor.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotOrder.Text) == 0)
                {
                    MessageBox.Show("Invalid Color Details", "Gainup");
                    Grid.CurrentCell = Grid["Color", 0];
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
                Queries = new String[(Dt.Rows.Count * 250)+4];

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New == true)
                    {
                        Queries[Array_Index++] = "Exec Update_Master_Seggregation 'Color', 'Colorid', " + TxtColor.Tag + ", " + Grid["Colorid", i].Value + " ";
                        Queries[Array_Index++] = "Exec Update_Master_Seggregation_Yarn_Plan_Det 'Color', 'Colorid', " + TxtColor.Tag + ", " + Grid["Colorid", i].Value + " ";
                        Queries[Array_Index++] = "Exec Update_Master_Seggregation 'Color', 'Color_id', " + TxtColor.Tag + ", " + Grid["Colorid", i].Value + " ";
                        Queries[Array_Index++] = "Exec Update_Master_Seggregation 'Color', 'Dyeing_Item_ID', " + TxtColor.Tag + ", " + Grid["Colorid", i].Value + " ";
                        Queries[Array_Index++] = "Exec Update_Master_Seggregation 'Color', 'Req_ColorID', " + TxtColor.Tag + ", " + Grid["Colorid", i].Value + " ";
                        Queries[Array_Index++] = "Exec Update_Master_Seggregation 'Color', 'Dye_ItemID', " + TxtColor.Tag + ", " + Grid["Colorid", i].Value + " ";
                        Queries[Array_Index++] = "Exec Update_Master_Seggregation_Name 'Color', 'Dye_Color', '" + TxtColor.Text + "', '" + Grid["Color", i].Value + "' ";
                        Queries[Array_Index++] = "Exec Update_Master_Seggregation_Name 'Color', 'Color', '" + TxtColor.Text + "', '" + Grid["Color", i].Value + "' ";
                        Queries[Array_Index++] = "update Color Set Color = (Case When Color Like 'ZZZ%' Then Color Else 'ZZZ'+Color End) where ColorID = " + Grid["Colorid", i].Value + " ";
                    }
                }

                
                MyBase.Run(Queries);
                MyParent.Save_Error = false;
                MessageBox.Show("Saved ..!", "Gainup");

                MyBase.Clear(this);
                Grid_Data();
                TxtColor.Focus();
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
                Str = "Select 0 as SNO, Color, Colorid From Fitsocks.Dbo.Color Where  1=2 ";
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid(ref Grid, "SNO", "Colorid");
                MyBase.Grid_Designing(ref Grid, ref Dt);
                MyBase.Grid_Width(ref Grid, 50, 300);
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["COLOR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;                
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["COLOR"].Index)
                    {
                        Dr = Tool.Selection_Tool_Except_New("COLOR", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Color", "Select Color, Colorid From Fitsocks.Dbo.Color Where Colorid <> " + TxtColor.Tag + " And Color Not like 'ZZZ%' Order By Color", String.Empty, 200, 120);
                        
                        if (Dr != null)
                        {
                            Grid["COLOR", Grid.CurrentCell.RowIndex].Value = Dr["COLOR"].ToString();
                            Txt.Text = Dr["COLOR"].ToString();
                            Grid["COLORID", Grid.CurrentCell.RowIndex].Value = Dr["COLORID"].ToString();
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
                TxtTotOrder.Text = MyBase.Count(ref Grid, "COLOR");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["COLOR"].Index)
                {
                    MyBase.Valid_Null(Txt, e);
                }
                else
                {
                }
                
                Total_Count();
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

        private void FrmMasterSegColor_KeyDown(object sender, KeyEventArgs e)
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
                    else if (this.ActiveControl.Name == "TxtColor")
                    {
                        Grid.CurrentCell = Grid["Color", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }

                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtColor")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color", "Select Color, Colorid From Fitsocks.Dbo.Color Where Color Not like 'ZZZ%' Order By Color", string.Empty, 200, 90);
                        if (Dr != null)
                        {
                            TxtColor.Text = Dr["Color"].ToString();
                            TxtColor.Tag = Dr["Colorid"].ToString();
                            return;
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

        private void FrmMasterSegColor_KeyPress(object sender, KeyPressEventArgs e)
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
