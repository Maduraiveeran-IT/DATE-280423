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
    public partial class FrmOrderCloseEntry : Form, Entry 
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
        public FrmOrderCloseEntry()
        {
            InitializeComponent();
        }
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                Grid_Data();
                DtpEDate.Focus();
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
                if (TxtTotOrder.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotOrder.Text) == 0)
                {
                    MessageBox.Show("Invalid Order Details", "Gainup");
                    Grid.CurrentCell = Grid["ORDER_NO", 0];
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
                Queries = new String[Dt.Rows.Count * 2];
                
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New == true)
                    {
                        if (Grid["CLOSED", i].Value.ToString() == "Y")
                        {
                            Queries[Array_Index++] = "Insert into Fit_Order_Status (EDate, Order_No, Status) Values ('" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "', '" + Grid["ORDER_NO", i].Value + "', '" + Grid["CLOSED", i].Value + "')";
                            Queries[Array_Index++] = "Update buy_ord_style set Despatch_Closed = '" + Grid["CLOSED", i].Value + "' Where Order_No = '" + Grid["ORDER_NO", i].Value + "'";
                        }
                    }
                    else
                    {
                        if (Grid["CLOSED", i].Value.ToString() == "Y")
                        {
                            Queries[Array_Index++] = "Delete From Fit_Order_Status where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' ";
                            Queries[Array_Index++] = "Insert into Fit_Order_Status (EDate, Order_No, Status) Values ('" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "', '" + Grid["ORDER_NO", i].Value + "', '" + Grid["CLOSED", i].Value + "')";
                            Queries[Array_Index++] = "Update buy_ord_style set Despatch_Closed = '" + Grid["CLOSED", i].Value + "' Where Order_No = '" + Grid["ORDER_NO", i].Value + "'";
                        }
                        else if (Grid["CLOSED", i].Value.ToString() == "N")
                        {
                            Queries[Array_Index++] = "Delete From Fit_Order_Status where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' ";
                            Queries[Array_Index++] = "Update buy_ord_style set Despatch_Closed = '" + Grid["CLOSED", i].Value + "' Where Order_No = '" + Grid["ORDER_NO", i].Value + "'";
                        }
                    }
                }

                MyBase.Run(Queries);
                MyParent.Save_Error = false;
                MessageBox.Show("Saved ..!", "Gainup");

                MyBase.Clear(this);
                Grid_Data();
                DtpEDate.Focus();
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
                DtpEDate.Focus();  
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
                Str = "Select 0 as SNO, F1.Order_No ORDER_NO, Isnull(F2.Status, 'N') CLOSED, '' T From Buy_Ord_Mas F1 Left join Fit_Order_Status F2 On F1.Order_No = F2.Order_No Where 1 = 2 ";
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);                
                MyBase.ReadOnly_Grid(ref Grid, "SNO");
                MyBase.Grid_Designing(ref Grid, ref Dt, "T");
                MyBase.Grid_Width(ref Grid, 50, 200, 120);
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["ORDER_NO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["CLOSED"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch (Exception ex)
            {
                throw ex;
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
        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ORDER_NO"].Index)
                    {
                        if (MyParent._New == true)
                        {
                            Dr = Tool.Selection_Tool_Except_New("ORDER_NO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "ORDER_NO", "Select F1.Order_No ORDER_NO, Isnull(F2.Status, 'N') CLOSED From Buy_Ord_Mas F1 Left join Fit_Order_Status  F2 On F1.Order_No = F2.Order_No Where F2.Order_No is Null ORDER BY  F1.Order_No  Asc", String.Empty, 200, 120);
                        }
                        else
                        {
                            Dr = Tool.Selection_Tool_Except_New("ORDER_NO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "ORDER_NO", "Select Order_No ORDER_NO, Status CLOSED From Fit_Order_Status  Where Status = 'Y' ORDER BY  Order_No  Asc", String.Empty, 200, 120);
                        }
                        if (Dr != null)
                        {
                            Grid["ORDER_NO", Grid.CurrentCell.RowIndex].Value = Dr["ORDER_NO"].ToString();
                            Grid["CLOSED", Grid.CurrentCell.RowIndex].Value = Dr["CLOSED"].ToString();
                            Txt.Text = Dr["ORDER_NO"].ToString();
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
                TxtTotOrder.Text = MyBase.Count(ref Grid, "ORDER_NO", "CLOSED");
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ORDER_NO"].Index)
                {
                    MyBase.Valid_Null(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["CLOSED"].Index)
                {
                    MyBase.Valid_Yes_OR_No(Txt, e);                   
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

        private void FrmOrderCloseEntry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                MyBase.Clear(this);
                DtpEDate.Focus();
                Grid_Data();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmOrderCloseEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {
                if (this.ActiveControl.Name != String.Empty)
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
            }
        }

        private void FrmOrderCloseEntry_KeyDown(object sender, KeyEventArgs e)
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
                    else if (this.ActiveControl.Name == "DtpEDate")
                    {
                        Grid.CurrentCell = Grid["ORDER_NO", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }

                    SendKeys.Send("{Tab}");
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
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        MyBase.Clear(this);
        //        Grid_Data();
        //        DtpEDate.Focus();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Entry_Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
       

    }
}
