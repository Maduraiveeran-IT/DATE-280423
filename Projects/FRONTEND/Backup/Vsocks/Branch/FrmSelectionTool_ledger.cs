using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules; 
using System.Windows.Forms;

namespace SelectionTool
{
    public partial class FrmSelectionTool_ledger : Form
    {
        Control_Modules MyBase = new Control_Modules();
        public DataView Dv = new DataView();
        public DataRow Selected_Row;
        DataColumn Dc;
        public String Related_Word = String.Empty;
        public bool Approval;
        int Txt_No;
        private bool ViewColumn;
   
        public FrmSelectionTool_ledger()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        private void Grid_Design(ref DataGridView DGV, DataTable Dt)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_Detail(bool ViewMode, String Sql, params int[] Col)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         public void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        public void Grid_Refresh()
        {
            try
            {
                dataGridView1.Columns["Id"].Visible = false;
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_Data(String Sql)
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void TxtCriteria_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void TxtTo_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        void TxtFrom_TextChanged(object sender, System.EventArgs e)
        {
        }

        void TxtCriteria_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void TxtCriteria_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        void Condition(int i)
        {
        }

        void Text_Clear()
        {
        }

        void Valid_Number(TextBox txt,System.Windows.Forms.KeyPressEventArgs e)   
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Valid_Decimal(TextBox txt, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Valid_Null(TextBox txt, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                e.Handled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CmbCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        void DtFrom_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        void DtTo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }


        void TxtFrom_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        void TxtTo_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Column_Width(int Column, params int[] nos)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void dataGridView1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }

        void dataGridView1_LostFocus(object sender, System.EventArgs e)
        {
            try
            {
                this.KeyPreview = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void dataGridView1_GotFocus(object sender, System.EventArgs e)
        {
            try
            {
                this.KeyPreview = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        void dataGridView1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }

        void GridClick()
        {
            Int32 id;
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void dataGridView1_Click(object sender, System.EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        void dataGridView1_DoubleClick(object sender, System.EventArgs e)
        {
            Int32 id;
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Return_Row();
                this.Close();
            }
        }

        public DataRow Return_Row()
        {
            try
            {
                return Selected_Row;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Caption(String Tit)
        {
            try
            {
                this.Text = "   " + Tit;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        public String Spaces(int Num)
        {
            try
            {
                String Spc=String.Empty;
                for (int i = 1; i <= Num; i++)
                {
                    Spc = Spc + " ";
                }
                return Spc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Return_Ucase(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (char.IsLower(e.KeyChar))
            {
                e.Handled = true;
                SendKeys.Send (Convert.ToString(char.ToUpper(e.KeyChar))); 
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FrmSelectionTool_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(39))
                {
                    e.Handled = true;
                    SendKeys.Send(Convert.ToChar(96).ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
   }
}