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
    public partial class Frm_Size_Master : Form, Entry
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

        public Frm_Size_Master()
        {
            InitializeComponent();
        }

        private void Frm_Size_Master_Load(object sender, EventArgs e)
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
                Grid_Data();
                Grid.Focus();
                Grid.CurrentCell = Grid["SIZE", 0];
                Grid.BeginEdit(true);
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
                if (Myparent.UserCode == 1)
                {
                    MyBase.Clear(this);
                    Str = "select Size,Sizeid from Size";
                    Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "Select Size - Edit...!", Str, String.Empty, 300);
                    if (Dr != null)
                    {
                        Fill_Datas();
                        //DtpDate1.Focus();

                    }
                }
                else
                {
                    MessageBox.Show("You are not allowed to EDIT...", "Gainup");
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
                    Str = "select Size,Sizeid from Size" ;
                    Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "Select Size - Delete...!", Str, String.Empty, 300);
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
                    Str = "Select 0 SNO,Sizeid,Size,'' T from Size WHERE 1=2";
                }
                else
                {
                    Str = "select 0 SNO,Sizeid,Size,'' T from Size Where Sizeid=" + Code + " ";
                }
                Dt = new DataTable();
                Grid.DataSource = null;
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Sizeid","T");
                MyBase.ReadOnly_Grid(ref Grid, "SNO");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid,50,290,80);
                Grid.RowHeadersWidth = 40;
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
                Code = Convert.ToInt64(Dr["sizeid"]);
                Grid_Data();
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
                Str = "select Size,Sizeid from Size";
                Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "Select Size - View...!", Str, String.Empty, 300);
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


                //for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                //{
                //    for (int j = 1; j < Dt.Columns.Count - 1; j++)
                //    {
                //        if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty)
                //        {


                //            flag = true;
                //            MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                //            Grid.CurrentCell = Grid[j, i];
                //            Grid.Focus();
                //            Grid.BeginEdit(true);
                //            Myparent.Save_Error = true;
                //            return;



                //        }

                //    }
                //}

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    
                        if (Grid["Size", i].Value.ToString() == string.Empty)
                        {
                            MessageBox.Show("Size should not be empty...!", "Gainup");
                            //Grid.CurrentCell = Grid["Size", Grid.CurrentCell.RowIndex];
                            Grid.CurrentCell = Grid["Size", i];
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
                        check = "select Size from Size where Size= '" + Grid["Size", j].Value.ToString() + "' ";
                        
                        MyBase.Load_Data(check, ref chkdata);
                        
                        if (chkdata.Rows.Count > 0)
                        {
                            MessageBox.Show("Size already exit");
                            Myparent.Save_Error = true;
                            return;

                        }
                        
                    }

                    
                    for (int k = 0; k <= Dt.Rows.Count-1; k++)
                    {

                        Queries[Array_Index++] = " insert into Size (Size) values('" + Grid["Size", k].Value.ToString() + "') ";
                    }

                }
                else
                {
                     
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                
                        Queries[Array_Index++] = "Update dbo.Size set Size ='" + Grid["Size", i].Value.ToString() + "' where Sizeid = " + Code;
                    }
                }
                if (flag ==false)
                {
                    MyBase.Run(Queries);
                    MessageBox.Show("Saved..!", "Gainup");
                    MyBase.Clear(this);
                    Myparent.Save_Error = false;
                }
            }
            catch(Exception ex)
            {
               MessageBox.Show(ex.Message);
               Myparent.Save_Error = true;
            }
        }

        public void Entry_Delete_Confirm()
        {
            try
            {
                if (Code > 0 && Dt.Rows.Count > 0)
                {
                    MyBase.Run("Delete From Size Where Sizeid = " + Code, Myparent.EntryLog("SIZE MASTER", "DELETE", Code.ToString()));
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

        private void Frm_Size_Master_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "DtpDate1")
                    {
                        Grid.CurrentCell = Grid["Size", 0];
                        Grid.BeginEdit(true);
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
                    Txt.Leave+=new EventHandler(Txt_Leave);
                    //Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                    //Txt.GotFocus += new EventHandler(Txt_GotFocus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Txt_DoubleClick(object sender,EventArgs e)
        {
            
        }

        void Txt_Leave(object sender, EventArgs e)
        {
            
        }
        void Txt_GotFocus(object sender, EventArgs e)
        {
            //try
            //{
            //    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Size"].Index)
            //    {
            //        MyBase.Row_Number(ref Grid);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        
        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Size"].Index)
                {
                    MyBase.Return_Ucase(e);
                    //MyBase.Valid_Alpha_Numeric(Txt,e);
                    
                    //Txt.CharacterCasing = CharacterCasing.Upper;
                    //if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)&&!char.IsWhiteSpace(e.KeyChar)&& e.KeyChar!='-')
                    //{
                    //    e.Handled = true;
                    //}
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Loss_Perc"].Index)
                {
                    MyBase.Valid_Decimal(Txt,e);

                    //if (!char.IsControl(e.KeyChar)&&!char.IsNumber(e.KeyChar)&& e.KeyChar!='.'&& e.KeyChar!='-')
                    //{
                    //    e.Handled = true;
                    //}
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
            
        }

        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
           
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
                            Myparent.Load_SaveEntry();
                        }
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
              
              else if (e.KeyCode == Keys.Enter)
              {
                  if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Size"].Index)
                  {
                      //if (Grid["Size", Grid.CurrentCell.RowIndex].Value.ToString() == string.Empty)
                      //{
                      //    e.Handled = true;
                      //    MessageBox.Show("Size should not be empty...!", "Gainup");
                      //    Grid.CurrentCell = Grid["Size", Grid.CurrentCell.RowIndex];
                      //    Grid.Focus();
                      //    Grid.BeginEdit(true);
                      //    return;
                      //}
                  }
                  else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Loss_Perc"].Index)
                  {
                      if (Grid["Loss_Perc", Grid.CurrentCell.RowIndex].Value != null && Grid["Loss_Perc", Grid.CurrentCell.RowIndex].Value.ToString() != string.Empty)
                      {
                          if (Convert.ToDouble(Grid["Loss_Perc", Grid.CurrentCell.RowIndex].Value) < 0.0 || Convert.ToDouble(Grid["Loss_Perc", Grid.CurrentCell.RowIndex].Value) > 7.0)
                          {
                              e.Handled = true;
                              MessageBox.Show("Loss Per Only Between 0.0 And 7.0...!", "Gainup");
                              Grid["Loss_Perc", Grid.CurrentCell.RowIndex].Value = "0.00";
                              Grid.CurrentCell = Grid["Loss_Perc", Grid.CurrentCell.RowIndex];
                              Grid.Focus();
                              Grid.BeginEdit(true);
                              return;
                          }
                      }
                      else
                      {
                          e.Handled = true;
                          MessageBox.Show("Loss_Perc should not be empty...!", "Gainup");
                          Grid["Loss_Perc", Grid.CurrentCell.RowIndex].Value = "0.00";
                          Grid.CurrentCell = Grid["Loss_Perc", Grid.CurrentCell.RowIndex];
                          Grid.Focus();
                          Grid.BeginEdit(true);
                          return;
                      }
                  }
              }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

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
                    DialogResult result = MessageBox.Show("Sure to delete..?","Confirm_Delete_Gainup",MessageBoxButtons.YesNo);
                    if (result==DialogResult.Yes)
                    {
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                        MessageBox.Show("Selected row has been deleted");
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
