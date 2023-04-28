using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Accounts_ControlModules;
using Accounts;
using System.Text;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmCalculator : Form
    {
        Control_Modules MyBase = new Control_Modules();
        public String Calculator_Table = "Calc" + Environment.MachineName.Replace("-", "");
        Int32 Calc_Code = 0;

        public FrmCalculator()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmCalculator_Load(object sender, EventArgs e)
        {
            try
            {
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmCalculator_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable Dt = new DataTable();
            try
            {
                if (this.ActiveControl.Name == "TxtOutput")
                {
                    MyBase.Valid_Null(TxtOutput, e);
                }
                else if (this.ActiveControl.Name == "TxtFormula")
                {
                    if (e.KeyChar == '@')
                    {
                        e.Handled = true;
                        MyBase.Load_Data("Select output from " + Calculator_Table + " where slno = " + Calc_Code, ref Dt);
                        if (Dt.Rows.Count > 0)
                        {
                            if (Dt.Rows[0][0] != null && Dt.Rows[0][0] != DBNull.Value)
                            {
                                TxtFormula.Text = TxtOutput.Text;
                                TxtOutput.Text = String.Empty;
                                SendKeys.Send("{END}");
                            }
                        }
                    }
                    else
                    {
                        if (e.KeyChar == ')' || e.KeyChar == '(' || e.KeyChar == '+' || e.KeyChar == '.' || e.KeyChar == '-' || e.KeyChar == '*' || e.KeyChar == '/' || e.KeyChar == '%')
                        {

                        }
                        else
                        {
                            MyBase.Valid_Number(TxtOutput, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message);
            }
        }


        bool Get_OutPut()
        {
            DataTable Dt = new DataTable();
            String Formula = String.Empty;
            try
            {
                if (TxtFormula.Text.Trim() == String.Empty)
                {
                    return false;
                }
                else
                {
                    Formula = Validate_Formula(TxtFormula.Text.Trim());
                    MyBase.Load_Data("Select " + Formula, ref Dt);
                    if (Dt.Rows.Count > 0)
                    {
                        if (Dt.Rows[0][0] != null && Dt.Rows[0][0] != DBNull.Value)
                        {
                            if (Dt.Columns[0].DataType == typeof(int) || Dt.Columns[0].DataType == typeof(Int16) || Dt.Columns[0].DataType == typeof(Int32) || Dt.Columns[0].DataType == typeof(Int64))
                            {
                                TxtOutput.Text = Convert.ToDouble(Dt.Rows[0][0]).ToString();
                            }
                            else
                            {
                                TxtOutput.Text = Convert.ToDouble(Dt.Rows[0][0]).ToString();
                            }
                            if (TxtOutput.Text.Contains("."))
                            {
                                TxtOutput.Text = String.Format("{0:0.00}", Convert.ToDouble(TxtOutput.Text));
                            }
                            return true;
                        }
                        else
                        {
                            TxtOutput.Text = String.Empty;
                            return false;
                        }
                    }
                    else
                    {
                        TxtOutput.Text = "Error: 000028";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        String Validate_Formula(String Formula)
        {
            String Str = String.Empty;
            try
            {
                if (Formula.Contains(".0"))
                {
                    return Formula;
                }
                else
                {
                    if (Formula.Contains("/"))
                    {
                        Str = Formula.Replace("/", ".00/");
                    }
                    else if (Formula.Contains("%"))
                    {
                        Str = Formula.Replace("%", ".00%");
                    }
                    else
                    {
                        Str = Formula;
                    }
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmCalculator_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable Dt = new DataTable();
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "TxtFormula")
                    {
                        if (Get_OutPut())
                        {
                            Calc_Code = Convert.ToInt32(MyBase.MaxWOCC(Calculator_Table, "Slno", String.Empty));
                            if (MyBase.Get_RecordCount(Calculator_Table, "formula = '" + TxtFormula.Text.Trim() + "' and output = " + TxtOutput.Text) == 0)
                            {
                                MyBase.Execute("Insert into " + Calculator_Table + " values (" + Calc_Code + ", '" + TxtFormula.Text.Trim() + "', " + TxtOutput.Text + ")");
                            }
                            else
                            {
                                Calc_Code -= 1;
                            }
                        }
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtFormula")
                    {
                        Calc_Code += 1;
                        if (MyBase.Get_RecordCount(Calculator_Table, "slno = " + Calc_Code) > 0)
                        {
                            MyBase.Load_Data("Select * from " + Calculator_Table + " where slno = " + Calc_Code, ref Dt);
                            if (Dt.Rows.Count > 0)
                            {
                                TxtFormula.Text = Dt.Rows[0]["Formula"].ToString();
                                TxtOutput.Text = Dt.Rows[0]["output"].ToString();
                                TxtOutput.Text = TxtOutput.Text.Replace(".00", "");
                            }
                            else
                            {
                                Calc_Code = Convert.ToInt32(MyBase.MaxWOCC(Calculator_Table, "Slno", String.Empty));
                                TxtFormula.Text = string.Empty;
                                TxtOutput.Text = string.Empty;
                            }
                        }
                        else
                        {
                            Calc_Code = Convert.ToInt32(MyBase.MaxWOCC(Calculator_Table, "Slno", String.Empty));
                            TxtFormula.Text = String.Empty;
                            TxtOutput.Text = String.Empty;
                        }
                        TxtFormula.Focus();
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (this.ActiveControl.Name == "TxtFormula")
                    {
                        if (Calc_Code > 0)
                        {
                            if (Calc_Code == 1)
                            {
                                Calc_Code = 1;
                            }
                            else
                            {
                                Calc_Code -= 1;
                            }
                            MyBase.Load_Data("Select * from " + Calculator_Table + " where slno = " + Calc_Code, ref Dt);
                            if (Dt.Rows.Count > 0)
                            {
                                TxtFormula.Text = Dt.Rows[0]["Formula"].ToString();
                                TxtOutput.Text = Dt.Rows[0]["output"].ToString();
                                TxtOutput.Text = TxtOutput.Text.Replace(".00", "");
                            }
                            else
                            {
                                TxtFormula.Text = string.Empty;
                                TxtOutput.Text = string.Empty;
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
    }
}