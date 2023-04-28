using System;
using System.Collections.Generic;
using System.ComponentModel;
using Accounts_ControlModules;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmCriteria : Form
    {
        DataTable Org_DT;
        public DataTable Criteria_DT;
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        Boolean Number;
        int Txt_No;

        public FrmCriteria()
        {
            InitializeComponent();
        }


        private void FrmCriteria_Load(object sender, EventArgs e)
        {
            try
            {
                //MyParent = (MDIMain)this.MdiParent;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Initial_Data(ref DataTable Dt, String Caption)
        {
            try
            {
                MyBase.Clear(this);
                this.Text = "Criteria For " + Caption;
                Org_DT = Dt;
                Fill_Columns();
                Condition(0);
                if (CmbField.Items.Count > 1)
                {
                    CmbField.SelectedIndex = 1;
                }
                if (CmbField.Items.Count > 0)
                {
                    CmbField.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Fill_Columns()
        {
            try
            {
                CmbField.Items.Clear();
                foreach (DataColumn dc in Org_DT.Columns)
                {
                    CmbField.Items.Add(dc.ColumnName);
                    CmbOrder.Items.Add(dc.ColumnName);
                }
                if (Org_DT.Columns.Count == 1)
                {
                    CmbField.Enabled = false;
                }
                else
                {
                    CmbField.Enabled = true;
                }
                CmbField.SelectedIndex = 0;
                CmbOrder.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Condition(int i)
        {
            CmbText.Items.Clear();
            PLText.Visible = false;
            PLDate.Visible = false;
            PLFromTo.Visible = false;
            if (i == 0)
            {
                CmbText.Items.Add("Equal To");
                CmbText.Items.Add("Greater Than");
                CmbText.Items.Add("Less Than");
                CmbText.Items.Add("Between (1-10)");
                Number = true;
                CmbText.SelectedIndex = 0;
                CmbText.Enabled = true;
                PLFromTo.Visible = true;
                PLText.Visible = true;
                TxtText.Focus();
            }
            else if (i == 1)
            {
                CmbText.Items.Add("Between");
                CmbText.SelectedIndex = 0;
                CmbText.Enabled = false;
                PLDate.Visible = true;
                DtpFrom.Focus();
            }
            else
            {
                CmbText.Items.Add("Starts With");
                CmbText.Items.Add("Not Like");
                Number = false;
                CmbText.Items.Add("Anywhere");
                CmbText.SelectedIndex = 0;
                CmbText.Enabled = true;
                PLText.Visible = true;
                TxtText.Focus();
            }
        }

        void Order_Method()
        {
            try
            {
                if (OptDesc.Checked == true)
                {
                    //Dv.Sort = "" + CmbField.Text + " DESC";
                }
                else
                {
                    //Dv.Sort = "" + CmbField.Text + " Asc";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally
            //{
            //    if (Dv.Table.Columns[0].Caption == "Id")
            //    {
            //        dataGridView1.Columns[0].Visible = false;
            //    }
            //}
        }

        void Text_Clear()
        {
            TxtText.Text = String.Empty;
            TxtFrom.Text = String.Empty;
            TxtTo.Text = String.Empty;
        }

        private void CmbField_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Text_Clear();
                if (Org_DT.Columns[CmbField.SelectedIndex].DataType == System.Type.GetType("System.String"))   
                {
                    Txt_No = 2;
                    Condition(2);
                }
                else if (Org_DT.Columns[CmbField.SelectedIndex].DataType == System.Type.GetType("System.DateTime"))   
                {
                    Txt_No = 1;
                    Condition(1);
                }
                else
                {
                    Txt_No = 0;
                    Condition(0); 
                }
                Order_Method(); 
                TxtText.Focus();  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String Order = string.Empty;
            DataRow[] Rows;
            try
            {
                if (TxtConditions.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Select Conditions ...!");
                    CmbField.Focus();
                    return;
                }
                else
                {
                    if (CmbOrder.Text.Trim() == String.Empty)
                    {
                        Rows = Org_DT.Select(TxtConditions.Text);
                    }
                    else
                    {
                        if (OptAsc.Checked == true)
                        {
                            Rows = Org_DT.Select(TxtConditions.Text, CmbOrder.Text + " asc");
                        }
                        else
                        {
                            Rows = Org_DT.Select(TxtConditions.Text, CmbOrder.Text + " DESC");
                        }
                    }
                }
                Criteria_DT = MyBase.Fill_With_Datarows(ref Org_DT, ref Rows, out Criteria_DT);
                this.Close();
                //MyParent.Return_Datasource(ref Temp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Criteria_DT = null;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbText_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Txt_No == 0)
                {
                    if (CmbText.Text.ToUpper().Contains("BETWEEN"))
                    {
                        PLText.Visible = false;
                        PLFromTo.Visible = true;
                        TxtFrom.Focus();
                    }
                    else
                    {
                        PLText.Visible = true;
                        PLFromTo.Visible = false;
                        TxtText.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Decimal(TxtFrom, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Decimal(TxtFrom, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtText_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Number)
                {
                    MyBase.Valid_Decimal(TxtText, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                Fill_Columns();
                Condition(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String Str = string.Empty;
            try
            {
                Str = CmbField.Text + " ";
                if (Txt_No == 0)
                {
                    if (CmbText.Text.Contains("Between (1-10)"))
                    {
                        if (TxtFrom.Text.Trim() == String.Empty || TxtTo.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("Please Enter From/TO ...!");
                            TxtFrom.Focus();
                            return;
                        }
                    }
                    else
                    {
                        if (TxtText.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("Please Enter Value ...!");
                            TxtText.Focus();
                            return;
                        }
                    }
                    if (CmbText.Text.Contains("Equal To"))
                    {
                        Str += " = " + TxtText.Text;
                    }
                    else if (CmbText.Text.Contains("Greater Than"))
                    {
                        Str += " > " + TxtText.Text;
                    }
                    else if (CmbText.Text.Contains("Less Than"))
                    {
                        Str += " < " + TxtText.Text;
                    }
                    else if (CmbText.Text.Contains("Between (1-10)"))
                    {
                        Str += " >= " + TxtFrom.Text + " and " + CmbField.Text + " <= " + TxtTo.Text;
                    }
                }
                else if (Txt_No == 1)
                {
                    Str += " >= #" + String.Format("{0:MM/dd/yyyy}", DtpFrom.Value) + "# and " + CmbField.Text + "  <= #" + String.Format("{0:MM/dd/yyyy}", DtpTo.Value) + "#";
                }
                else
                {
                    if (TxtText.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Please Enter Value ..!");
                        TxtText.Focus();
                        return;
                    }
                    if (CmbText.Text.Contains("Starts With"))
                    {
                        Str += " like '" + TxtText.Text + "%'";
                    }
                    else if (CmbText.Text.Contains("Not Like"))
                    {
                        Str += " not like '" + TxtText.Text + "%'";
                    }
                    else if (CmbText.Text.Contains("Anywhere"))
                    {
                        Str += " like '%" + TxtText.Text + "%'";
                    }
                }
                CmbField.Items.RemoveAt(CmbField.SelectedIndex);
                if (TxtConditions.Text.Trim() == string.Empty)
                {
                    TxtConditions.Text = Str;
                }
                else
                {
                    TxtConditions.Text += "  And " + Str;
                }
                TxtText.Text = String.Empty;
                TxtFrom.Text = string.Empty;
                TxtTo.Text = string.Empty;
                CmbField.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "TxtText" || this.ActiveControl.Name == "TxtTo" || this.ActiveControl.Name == "DtpTo")
                    {
                        button1.Focus();
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message);
            }
        }

        private void GBCriteria_Enter(object sender, EventArgs e)
        {

        }
    }
}