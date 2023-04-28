using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using System.Windows.Forms;

namespace Accounts
{

    public partial class Frm_Ledger_Selection : Form
    {

        public String TblName = String.Empty, FldName = String.Empty, CodeName = String.Empty, Condition = String.Empty, CompName = String.Empty;
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        DataTable Dt = new DataTable();
        public DataRow Dr;

        public Frm_Ledger_Selection()
        {
            InitializeComponent();
        }

        private void Frm_Ledger_Selection_Load(object sender, EventArgs e)
        {
            try
            {
                if (CompName.ToUpper().Contains("RAJARAM"))
                {
                    OptP.Checked = true;
                }
                else
                {
                    OptS.Checked = true;
                }
                Load_Data();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Color()
        {
            try
            {
                for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                {
                    if (Convert.ToInt32(Grid["SlnoQ", i].Value) == 1)
                    {
                        Grid.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Load_Data()
        {
            String Condition_New = String.Empty;
            try
            {
                String Str = String.Empty;
                if (Condition == String.Empty)
                {
                    Condition_New = "";
                }
                else
                {
                    Condition_New = Condition + " And ";
                }
                if (OptS.Checked)
                {
                    Str = "Select top 100000000000000000 " + FldName + ", " + CodeName + ", 1 as SlnoQ  from " + TblName + " where " + Condition_New + " Dbo.ledger_String(" + FldName + ") like dbo.ledger_String('" + textBox1.Text + "%') order by " + FldName;
                    MyBase.Execute_Qry(Str, "Sel1");
                    Str = "Select top 100000000000000000 " + FldName + ", " + CodeName + ", 2 as SlnoQ from " + TblName + " where " + Condition_New + " Dbo.ledger_String(" + FldName + ") Not like dbo.ledger_String('" + textBox1.Text + "%') order by " + FldName;
                    MyBase.Execute_Qry(Str, "Sel2");
                    Str = "Select * from sel1 union select * from sel2";
                }
                else if (OptP.Checked)
                {
                    Str = "Select top 100000000000000000 " + FldName + ", " + CodeName + ", 1 as SlnoQ  from " + TblName + " where " + Condition_New + " Dbo.ledger_String(" + FldName + ") like dbo.ledger_String('%" + textBox1.Text + "%') order by " + FldName;
                    MyBase.Execute_Qry(Str, "Sel1");
                    //Str = "Select top 100000000000000000 " + FldName + ", " + CodeName + ", 2 as SlnoQ from " + TblName + " where " + Condition_New + " Dbo.ledger_String(" + FldName + ") Not like dbo.ledger_String('" + textBox1.Text + "%') order by " + FldName;
                    //MyBase.Execute_Qry(Str, "Sel2");
                    Str = "Select * from sel1 ";
                }
                else
                {
                    Str = "Select top 100000000000000000 " + FldName + ", " + CodeName + ", 1 as SlnoQ  from " + TblName + " where " + Condition_New + " Dbo.ledger_String(" + FldName + ") like dbo.ledger_String('%" + textBox1.Text + "') order by " + FldName;
                    MyBase.Execute_Qry(Str, "Sel1");
                    //Str = "Select top 100000000000000000 " + FldName + ", " + CodeName + ", 2 as SlnoQ from " + TblName + " where " + Condition_New + " Dbo.ledger_String(" + FldName + ") Not like dbo.ledger_String('" + textBox1.Text + "%') order by " + FldName;
                    //MyBase.Execute_Qry(Str, "Sel2");
                    Str = "Select * from sel1 ";
                }
                MyBase.Execute_Qry(Str, "Sel3");
                Str = "Select Distinct * from sel3 order by SlnoQ, " + FldName;
                Grid.DataSource = MyBase.Load_Data (Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, CodeName, "SlnoQ");
                Grid.ColumnHeadersVisible = false;
                MyBase.ReadOnly_Grid_Without (ref Grid);
                MyBase.Grid_Width (ref Grid, 450);
                Grid.RowHeadersWidth = 10;
                Color();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Load_Data();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    Grid.Focus();
                }
                else if (e.KeyCode == Keys.Enter)
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Ledger_Selection_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (this.ActiveControl.Name == "Grid")
                    {
                        e.Handled = true;
                        textBox1.Focus();
                        return;
                    }
                    else
                    {
                        Dr = null;
                    }
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell == null)
                    {
                        if (Dt.Rows.Count > 0)
                        {
                            Grid.CurrentCell = Grid[MyBase.Get_First_Column(ref Grid), 0];
                            Dr = Dt.Rows[Grid.CurrentCell.RowIndex];
                        }
                        else
                        {
                            Dr = null;
                        }
                    }
                    else
                    {
                        if (Dt.Rows.Count - 1 >= Grid.CurrentCell.RowIndex)
                        {
                            Dr = Dt.Rows[Grid.CurrentCell.RowIndex];
                        }
                        else
                        {
                            Dr = null;
                        }
                    }
                }

                if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OptS_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                textBox1.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OptP_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                textBox1.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OptE_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                textBox1.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}