using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using Accounts;

namespace Accounts
{
    public partial class Frm_Embroidery_Budget_Breakup_Approval : Form
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataTable Dt2 = new DataTable();
        DataGrid dgv = new DataGrid();
        DataRow Dr;
        TextBox Txt = null;
        String[] Queries;

        Boolean Status_Flag = false;
        Boolean Status_Flag_To = false;

        public Frm_Embroidery_Budget_Breakup_Approval()
        {
            InitializeComponent();
        }

        private void Frm_Embroidery_Budget_Breakup_Approval_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                Grid_Data();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Embroidery_Budget_Breakup_Approval_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtTotOrder" || this.ActiveControl.Name == "TxtTotalApproved" || this.ActiveControl.Name == "TxtTotalEntered"
                        || this.ActiveControl.Name == "TxtSelectedApproved" || this.ActiveControl.Name == "TxtSelectedEntered")
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
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
                Str = " Exec Fitsocks.Dbo.Embroidery_Budget_Breakup_Approval_Pending ";
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);

                //MyBase.ReadOnly_Grid_Without(ref Grid, "Remarks");

                if (Status_Flag)
                {
                    Grid.Columns.Remove("Status");
                    DataGridViewCheckBoxColumn Check = new DataGridViewCheckBoxColumn();
                    Check.HeaderText = "STATUS";
                    Check.Name = "STATUS";
                    Check.ValueType = typeof(String);
                    Check.Visible = true;
                    Check.ReadOnly = false;
                    Grid.Columns.Insert(0, Check);
                    Status_Flag = true;
                    Check.TrueValue = 1;
                }
                else
                {
                    DataGridViewCheckBoxColumn Check = new DataGridViewCheckBoxColumn();
                    Check.HeaderText = "STATUS";
                    Check.Name = "STATUS";
                    Check.ValueType = typeof(String);
                    Check.Visible = true;
                    Check.ReadOnly = false;
                    Grid.Columns.Insert(0, Check);
                    Status_Flag = true;
                }
                if (Dt.Rows.Count > 0)
                {
                    MyBase.Grid_Designing(ref Grid, ref Dt, "ItemID", "OrderColorID", "SizeID", "MasterID", "Slno1", "T");
                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);

                    Grid.Columns["Order_NO"].HeaderText = "OCN";
                    MyBase.Grid_Width(ref Grid, 50, 130, 120, 150, 150, 80, 100);

                    Grid.Columns["Order_NO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    Grid.Columns["Status"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;

                    Grid.RowHeadersWidth = 10;

                    Total_Count_1();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Total_Count_1()
        {
            try
            {
                TxtTotalApproved.Text = MyBase.Sum_With_Three_Digits(ref Grid, "AppRate");
                Double Entered = 0;
                for (int i = 0; i <= Dt.Rows.Count-1; i++)
                {
                    for (int j = 0; j <= Dt.Columns.Count-2; j++)
                    {
                        if (Grid[j, i].ColumnIndex == Grid.Columns["Total"].Index)
                        {
                            Entered += Convert.ToDouble(Grid["Total", i].Value.ToString());
                        }
                    }
                }
                //TxtTotalEntered.Text = MyBase.Sum_With_Three_Digits(ref Grid, "Stock");
                TxtTotalEntered.Text = Entered.ToString();
                TxtSelectedApproved.Text = MyBase.SumWithCondtion(ref Grid, "AppRate", "Status", "True", "AppRate");
                Entered = 0;
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Dt.Columns.Count - 2; j++)
                    {
                        if (Grid[j, i].ColumnIndex == Grid.Columns["Total"].Index)
                        {
                            if(Grid["Status", i].Value != null && Grid["Status", i].Value != DBNull.Value && Grid["Status", i].Value.ToString().ToUpper() == "TRUE")
                            {
                                Entered += Convert.ToDouble(Grid["Total", i].Value.ToString());
                            }
                        }
                    }
                }
                TxtSelectedEntered.Text = Entered.ToString();
                //TxtSelectedEntered.Text = MyBase.SumWithCondtion(ref Grid, "Stock", "Status", "True", "Stock");
                int o = 0;
                if (Dt.Rows.Count > 0)
                {
                    o = 1;
                }
                for (int i = 1; i <= Grid.Rows.Count - 1; i++)
                {
                    if (Grid["Order_No", i].Value.ToString() != Grid["Order_No", i - 1].Value.ToString())
                    {
                        o++;
                    }
                }

                TxtTotOrder.Text = o.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Frm_Embroidery_Budget_Breakup_Approval_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.F5)
                {
                    if (Grid.Rows.Count > 0)
                    {
                        for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                        {
                            //Grid["Status", i].Value = true;
                            //Grid["Priority", i].Value = (i + 1);
                        }
                        Grid.CurrentCell = Grid["Status", 0];
                        Grid.Focus();
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {

                }
                else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Grid["STATUS", i].Value = checkBox1.Checked;
                    if (Grid["STATUS", i].Value.ToString() == "True")
                    {
                        Grid["STATUS", i].Value = "True";
                    }
                }
                Total_Count_1();
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
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Int32 Array_Index = 0;
            try
            {
                
                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Approve...!", "Gainup");
                    checkBox1.Focus();
                    return;
                }

                Total_Count_1();

                if (MessageBox.Show("Sure to Approve...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }

                int Ch = 0;
                for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                {
                    if (Grid["Status", i].Value != null)
                    {
                        if (Grid["Status", i].Value.ToString().ToUpper() == "TRUE")
                        {
                            Ch++;
                        }
                    }
                }

                if (Ch == 0)
                {
                    MessageBox.Show("Kindly Select Any One From List...!", "Gainup");
                    Grid.CurrentCell = Grid["Status", 0];
                    Grid.Focus();
                    return;
                }

                Queries = new string[Dt.Rows.Count * 10];

                for (int i = 0; i <= Grid.Rows.Count - 1; i++)
                {
                    if (Grid["Status", i].Value != null)
                    {
                        if (Grid["Status", i].Value.ToString().ToUpper() == "TRUE")
                        {
                            Queries[Array_Index++] = "Update Embroidery_Budget_Rate_Breakup_Process_Details Set Approval = 'Y', Approval_Time = Getdate(), Approval_System = Host_Name() Where MasterID = " + Grid["MasterID", i].Value + "  And Slno1 = " + Grid["Slno1", i].Value + " ";
                        }
                    }
                }

                MyBase.Run_Identity(false, Queries);
                MyParent.Save_Error = false;
       
                MessageBox.Show("Saved ..!", "Gainup");
                MyBase.Clear(this);
                Grid_Data();
                if (checkBox1.Checked == true)
                {
                    checkBox1.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                if (ex.ToString().Contains("Chk_Yarn_Status_Transin"))
                {
                    MessageBox.Show("Kindly Wrise PO, " + ex.Message);
                }
                else if (ex.ToString().Contains("Chk_Bom_PO_pending"))
                {
                    MessageBox.Show("Kindly Check Stock, " + ex.Message);
                }
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.GetType() == typeof(DataGridViewCheckBoxCell))
                {
                    Grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    Total_Count_1();
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
                    //Txt.GotFocus += new EventHandler(Txt_GotFocus);
                    //Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                    //Txt.Leave += new EventHandler(Txt_Leave);
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
                MyBase.Valid_Null(Txt, e);
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
                    button9.Focus();
                    SendKeys.Send("{End}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //Grid.Refresh();
                //if (Dt.Rows.Count > 0)
                //{
                //    int Pos = Grid.CurrentCell.RowIndex;
                //    Grid.CurrentCell = Grid["Buyer", Pos];
                //    Grid.Focus();
                //    Grid.BeginEdit(true);
                //    Grid.CurrentCell = Grid["Status", Pos];
                //    Grid.Focus();
                //    Grid.BeginEdit(true);
                //    Total_Count_1();
                //    return;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
