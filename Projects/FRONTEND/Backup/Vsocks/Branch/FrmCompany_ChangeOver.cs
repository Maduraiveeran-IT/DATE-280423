using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Accounts_ControlModules;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmCompany_ChangeOver : Form
    {
        Control_Modules MyBase = new Control_Modules();
        public String CompName = String.Empty;
        public Int16 User_Code = 0;
        public Int32 Company_Code = 0;
        public String Year_Code = String.Empty;

        public FrmCompany_ChangeOver()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void FrmCompany_ChangeOver_Load(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();
            try
            {
                this.StartPosition = FormStartPosition.CenterScreen;
                if (CompName.ToUpper().Contains("AEGAN"))
                {
                    if (User_Code == 1)
                    {
                        MyBase.Load_Data("Select CompCode, CompName Company, Cast(Year(Sdt) as varchar(4)) + '-' + Cast(Year(Edt) as varchar(4)) YearCode from Socks_Companymas order by CompName, Cast(Year(Sdt) as varchar(4)) + ' - ' + Cast(Year(Edt) as varchar(4)) Desc", ref Dt);
                    }
                    else
                    {
                        MyBase.Load_Data("Select CompCode, CompName Company, Cast(Year(Sdt) as varchar(4)) + '-' + Cast(Year(Edt) as varchar(4)) YearCode from Socks_Companymas where compcode = 2 order by CompName, Cast(Year(Sdt) as varchar(4)) + ' - ' + Cast(Year(Edt) as varchar(4)) Desc", ref Dt);
                    }
                }
                else if (CompName.ToUpper().Contains("AVANEETHA") || CompName.ToUpper().Contains("GAINUP") || CompName.ToUpper().Contains("ALAMELU"))
                {
                    MyBase.Load_Data("Select CompCode, CompName Company, Cast(Year(Sdt) as varchar(4)) + '-' + Cast(Year(Edt) as varchar(4)) YearCode from Socks_Companymas order by CompCode, Cast(Year(Sdt) as varchar(4)) + '-' + Cast(Year(Edt) as varchar(4)) DESC ", ref Dt);
                }
                else
                {
                    MyBase.Load_Data("Select CompCode, CompName Company, Cast(Year(Sdt) as varchar(4)) + '-' + Cast(Year(Edt) as varchar(4)) YearCode from Socks_Companymas order by CompCode, Cast(Year(Sdt) as varchar(4)) + '-' + Cast(Year(Edt) as varchar(4))", ref Dt);
                }
                Grid.DataSource = Dt;
                MyBase.Grid_Designing(ref Grid, ref Dt);
                Grid.Columns["CompCode"].HeaderText = "Code";
                MyBase.Grid_Width(ref Grid, 80, 320, 100);
                Grid.RowHeadersWidth = 10;
                MyBase.ReadOnly_Grid_Without(ref Grid);
                Grid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Blue;
                Grid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                Grid.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmCompany_ChangeOver_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell != null)
                    {
                        Company_Code = Convert.ToInt32(Grid["CompCode", Grid.CurrentCell.RowIndex].Value);
                        Year_Code = Grid["YearCode", Grid.CurrentCell.RowIndex].Value.ToString();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell != null)
                {
                    Company_Code = Convert.ToInt32(Grid["CompCode", Grid.CurrentCell.RowIndex].Value);
                    Year_Code = Grid["YearCode", Grid.CurrentCell.RowIndex].Value.ToString();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}