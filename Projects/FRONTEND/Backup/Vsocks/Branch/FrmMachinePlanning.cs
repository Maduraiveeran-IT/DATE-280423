using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Accounts_ControlModules;
using System.Text;
using SelectionTool_NmSp;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmMachinePlanning : Form
    {
        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        String Str = String.Empty;
        Int32 Cur_Year = 0; Int32 Cur_Week = 0;

        public FrmMachinePlanning()
        {
            InitializeComponent();
        }

        private void FrmMachinePlanning_Load(object sender, EventArgs e)
        {
            try
            {
                MDIMain MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                Load_From_Year();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Load_From_Year()
        {
            try
            {
                for (int i = 2015; i <= 2020; i++)
                {
                    CmbFromYear.Items.Add(i);
                    CmbToYear.Items.Add(i);
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
                this.Cursor = Cursors.WaitCursor;

                Grid.DataSource = null;
                Dt = new DataTable();


                Str = "Socks_Machine_Planning_Proc " + CmbFromYear.Text + ", " + CmbFromWeek.Text + ",  " + CmbToYear.Text + ", " + CmbToWeek.Text;

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Machine_ID");
                MyBase.ReadOnly_Grid_Without(ref Grid);

                Grid.Columns[1].HeaderText = "NEEDLE";
                Grid.Columns[1].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                Grid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns[1].Width = 100;

                for (int i = 2; i <= Grid.Columns.Count - 1; i++)
                {
                    Grid.Columns[i].Width = 80;
                    Grid.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    #region year
                    if (Grid.Columns[i].HeaderText.Contains("2014"))
                    {
                        Grid.Columns[i].Tag = "2014";
                    }
                    else if (Grid.Columns[i].HeaderText.Contains("2015"))
                    {
                        Grid.Columns[i].Tag = "2015";
                    }
                    else if (Grid.Columns[i].HeaderText.Contains("2016"))
                    {
                        Grid.Columns[i].Tag = "2016";
                    }
                    else if (Grid.Columns[i].HeaderText.Contains("2017"))
                    {
                        Grid.Columns[i].Tag = "2017";
                    }
                    else if (Grid.Columns[i].HeaderText.Contains("2018"))
                    {
                        Grid.Columns[i].Tag = "2018";
                    }
                    else if (Grid.Columns[i].HeaderText.Contains("2019"))
                    {
                        Grid.Columns[i].Tag = "2019";
                    }
                    else if (Grid.Columns[i].HeaderText.Contains("2020"))
                    {
                        Grid.Columns[i].Tag = "2020";
                    }
                    #endregion


                    Str = Grid.Columns[i].HeaderText.Replace("2014_", "").Replace("2015_", "").Replace("2016_", "").Replace("2017_", "").Replace("2018_", "").Replace("2019_", "").Replace("2020_", "");
                    Grid.Columns[i].HeaderText = Str;

                    if (Convert.ToDouble(Grid.Columns[i].Tag) > Cur_Year)
                    {
                        Grid.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    else if (Convert.ToDouble(Grid.Columns[i].Tag) == Cur_Year)
                    {
                        if (Convert.ToDouble(Grid.Columns[i].HeaderText) > Cur_Week)
                        {
                            Grid.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                        }
                        else
                        {
                            Grid.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                        }
                    }
                    else
                    {
                        Grid.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                        Grid.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                    }
                }
                MyBase.Grid_Freeze(ref Grid, Control_Modules.FreezeBY.Column_Wise, 1);
                Grid.RowHeadersWidth = 30;
                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable Tdt = new DataTable();

                MyBase.Load_Data("Select Year(GetDate()) Year, DATEPART(Week, Getdate()) Week", ref Tdt);
                Cur_Year = Convert.ToInt32(Tdt.Rows[0]["year"]);
                Cur_Week = Convert.ToInt32(Tdt.Rows[0]["week"]);

                if (CmbFromYear.Text.Trim() == String.Empty || CmbFromWeek.Text.Trim() == String.Empty || CmbToYear.Text.Trim() == String.Empty || CmbToWeek.Text.Trim() == String.Empty)
                {
                    MessageBox.Show ("Invalid Period ..!", "Gainup");
                    CmbFromWeek.Focus();
                    return;
                }

                Grid_Data();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void CmbFromYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable Tdt = new DataTable();
                if (CmbFromYear.Text != String.Empty)
                {
                    MyBase.Load_Data("Select Week, Week Week1 from Get_Week_Details () Where year = " + CmbFromYear.Text + " order by Week", ref Tdt);
                    CmbFromWeek.DataSource = Tdt;
                    CmbFromWeek.DisplayMember = "Week";
                    CmbFromWeek.ValueMember = "Week1";
                    CmbFromWeek.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbToYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable Tdt1 = new DataTable();
                if (CmbToYear.Text != String.Empty)
                {
                    MyBase.Load_Data("Select Week, Week Week1 from Get_Week_Details () Where year = " + CmbToYear.Text + " order by Week", ref Tdt1);
                    CmbToWeek.DataSource = Tdt1;
                    CmbToWeek.DisplayMember = "Week";
                    CmbToWeek.ValueMember = "Week1";
                    CmbToWeek.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
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

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                CmbFromYear.SelectedIndex = -1;
                CmbToYear.SelectedIndex = -1;
                CmbFromWeek.SelectedIndex = -1;
                CmbToWeek.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex >= 2)
                {
                    FrmMachinePLanningWeek Frm = new FrmMachinePLanningWeek(Convert.ToInt32(Grid.Columns[Grid.CurrentCell.ColumnIndex].Tag), Convert.ToInt32(Grid.Columns[Grid.CurrentCell.ColumnIndex].HeaderText), Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString(), Grid.CurrentCell.Value.ToString(), Grid.CurrentCell.RowIndex, Grid.CurrentCell.ColumnIndex);
                    Frm.StartPosition = FormStartPosition.Manual;
                    Frm.Left = 300;
                    Frm.Top = 150;
                    Frm.ShowDialog();
                    Grid.CurrentCell.Value = String.Format ("{0:0.00}", Frm.Utilization);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}