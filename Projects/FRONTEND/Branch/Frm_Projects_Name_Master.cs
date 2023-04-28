using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using Accounts;
using System.Windows.Forms;

namespace Accounts
{
    public partial class Frm_Projects_Name_Master : Form, Entry
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
        
        public Frm_Projects_Name_Master()
        {
            InitializeComponent();
        }

        private void Frm_Projects_Name_Master_Load(object sender, EventArgs e)
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
                label2.Focus();
                Grid_Data();

                Grid.CurrentCell = Grid["Name", 0];
                Grid.Focus();
                Grid.BeginEdit(true);
                return;
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
                    for (int j = 1; j < Dt.Columns.Count - 1; j++)
                    {
                        if ((Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty) &&
                            (Grid.Columns[j].Name != "T" && Grid.Columns[j].Name != "RowID"))
                        {
                            flag = true;
                            MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                            Grid.CurrentCell = Grid[j, i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            Myparent.Save_Error = true;
                            return;
                        }

                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Name", i].Value != null && Grid["Name", i].Value != DBNull.Value && Grid["Name", i].Value.ToString() != String.Empty
                        && (Grid["Short_Name", i].Value == null || Grid["Short_Name", i].Value == DBNull.Value || Grid["Short_Name", i].Value.ToString() == String.Empty))
                    {
                        MessageBox.Show("Short Name should not be empty...!", "Gainup");
                        Grid.CurrentCell = Grid["Short_Name", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        Myparent.Save_Error = true;
                        flag = true;
                    }
                    else if (Grid["Short_Name", i].Value != null && Grid["Short_Name", i].Value != DBNull.Value && Grid["Short_Name", i].Value.ToString() != String.Empty
                        & Grid["Short_Name", i].Value.ToString().Length != 3)
                    {
                        MessageBox.Show("Short Name Must be 3 Characters...!", "Gainup");
                        Grid.CurrentCell = Grid["Short_Name", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        Myparent.Save_Error = true;
                        flag = true;
                    }
                }

                if (Myparent._New)
                {
                    for (int j = 0; j <= Dt.Rows.Count - 1; j++)
                    {
                        check = "Select Name from Project_Name_Master Where Name = '" + Grid["Name", j].Value.ToString() + "' ";

                        MyBase.Load_Data(check, ref chkdata);

                        if (chkdata.Rows.Count > 0)
                        {
                            MessageBox.Show("Name already exit");
                            Grid.CurrentCell = Grid["Name", j];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            Myparent.Save_Error = true;
                            return;

                        }

                    }


                    for (int k = 0; k <= Dt.Rows.Count - 1; k++)
                    {
                        if (Grid["RowID", k].Value != null && Grid["RowID", k].Value != DBNull.Value && Grid["RowID", k].Value.ToString() != String.Empty)
                        {
                            Queries[Array_Index++] = "Update Project_Name_Master set Name ='" + Grid["Name", k].Value.ToString() + "',Short_Name='" + Grid["Short_Name", k].Value.ToString() + "',Entry_Time=getdate(), Entry_System =Host_Name(), User_Code = " + Myparent.UserCode + " Where RowID = " + Grid["RowID", k].Value.ToString() + " ";
                        }
                        else
                        {
                            Queries[Array_Index++] = "insert into Project_Name_Master (Name, Short_Name, Entry_Time, Entry_System, User_Code) Values ('" + Grid["Name", k].Value.ToString() + "','" + Grid["Short_Name", k].Value.ToString() + "',Getdate(), Host_Name(), " + Myparent.UserCode + ")";
                        }
                    }
                }
                else
                {

                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Grid["RowID", i].Value != null && Grid["RowID", i].Value != DBNull.Value && Grid["RowID", i].Value.ToString() != String.Empty)
                        {
                            Queries[Array_Index++] = "Update Project_Name_Master set Name ='" + Grid["Name", i].Value.ToString() + "',Short_Name='" + Grid["Short_Name", i].Value.ToString() + "',Entry_Time=getdate(), Entry_System =Host_Name(), User_Code = " + Myparent.UserCode + " Where RowID = " + Grid["RowID", i].Value.ToString() + " ";
                        }
                        else
                        {
                            Queries[Array_Index++] = "insert into Project_Name_Master (Name, Short_Name, Entry_Time, Entry_System, User_Code) Values ('" + Grid["Name", i].Value.ToString() + "','" + Grid["Short_Name", i].Value.ToString() + "',Getdate(), Host_Name(), " + Myparent.UserCode + ")";
                        }
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
                Str = "Select Name, Short_Name, RowID From Project_Name_Master Order By Name";
                Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "Select Project - Edit...!", Str, String.Empty, 200, 100);
                if (Dr != null)
                {
                    Fill_Datas();
                    label2.Focus();
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
                    Str = "Select Name, Short_Name, RowID From Project_Name_Master Order By Name";
                    Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "Select Project - Delete...!", Str, String.Empty, 200, 100);
                    if (Dr != null)
                    {
                        Fill_Datas();
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
                    Str = "Select 0 Sno, Name, Short_Name, RowID, '' T From Project_Name_Master Where 1 = 2 ";
                }
                else
                {
                    Str = "Select 0 Sno, Name, Short_Name, RowID, '' T From Project_Name_Master Where RowID = " + Code;
                }
                Dt = new DataTable();
                Grid.DataSource = null;
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "RowID", "T");
                MyBase.ReadOnly_Grid(ref Grid, "SNO");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 50, 350, 100);
                Grid.RowHeadersWidth = 40;
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
                Code = Convert.ToInt64(Dr["RowID"]);
                rowid = Convert.ToInt64(Dr["RowID"].ToString());
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
                    Str = "Delete from Project_Name_Master Where RowID = " + Code + "";
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
                Str = " Select Name, Short_Name, RowID From Project_Name_Master Order By RowID ";
                
                Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "Select Project Name - View...!", Str, String.Empty, 250, 100);
                if (Dr != null)
                {
                    Fill_Datas();
                    Grid.CurrentCell = Grid["Name", 0];
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

        private void Frm_Projects_Name_Master_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == label2.Name || this.ActiveControl.Name == DtpDate1.Name)
                    {
                        Grid.CurrentCell = Grid["Name", 0];
                        Grid.BeginEdit(true);
                    }
                    else if (this.ActiveControl.Name == TxtTotOrder.Name)
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Name"].Index)
                {
                    MyBase.Return_Ucase(e);
                    if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && e.KeyChar != '-')
                    {
                        e.Handled = true;
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Short_Name"].Index)
                {
                    MyBase.Return_Ucase(e);
                    MyBase.Valid_Alpha_Numeric(Txt, e);
                }
                else
                {
                    
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Short_Name"].Index)
                {
                    if (Grid["Short_Name", Grid.CurrentCell.RowIndex].Value != null &&
                        Grid["Short_Name", Grid.CurrentCell.RowIndex].Value != DBNull.Value &&
                        Grid["Short_Name", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                    {
                        if (Grid["Short_Name", Grid.CurrentCell.RowIndex].Value.ToString().Length != 3)
                        {
                            MessageBox.Show("Short Name Must Be Three Characters...!", "Gainup");
                            Grid.CurrentCell = Grid["Short_Name", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                        Total_Count();
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
                TxtTotOrder.Text = MyBase.Count(ref Grid, "Name");
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
                    TxtTotOrder.Focus();
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Short_Name"].Index)
                    {
                        if (Grid["Short_Name", Grid.CurrentCell.RowIndex].Value != null &&
                            Grid["Short_Name", Grid.CurrentCell.RowIndex].Value != DBNull.Value &&
                            Grid["Short_Name", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            if (Grid["Short_Name", Grid.CurrentCell.RowIndex].Value.ToString().Length != 3)
                            {
                                MessageBox.Show("Short Name Must Be Three Characters...!", "Gainup");
                                Grid.CurrentCell = Grid["Short_Name", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                            Total_Count();
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
                    }
                    Total_Count();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Projects_Name_Master_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == TxtTotOrder.Name)
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
