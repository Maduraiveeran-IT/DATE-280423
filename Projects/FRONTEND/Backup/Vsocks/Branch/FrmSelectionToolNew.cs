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
    public partial class FrmSelectionToolItem : Form
    {
        Control_Modules MyBase = new Control_Modules();
        DataView Dv = new DataView();
        public DataRow Selected_Row;
        DataColumn Dc;
        public String Related_Word = String.Empty;
        public bool Approval;
        Boolean Err_Change = false;
        String Org_Sql = String.Empty;
        String C_Sql = String.Empty;
        String Org_Sql1 = String.Empty;
        int Txt_No;
        private bool ViewColumn;
   
        public FrmSelectionToolItem()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.RowHeadersWidth = 4;
                Dc = new DataColumn("Id", Type.GetType("System.Int64"));
                Dc.AutoIncrement = true; 
                Dc.AutoIncrementSeed = 0;
                Dc.AutoIncrementStep = 1;
                Dc.Unique = true;
                Tool_Status();
                if (ViewColumn == true)
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        SendKeys.Send("{Down}");
                        TxtCriteria.Focus();
                    }
                }
                else
                {
                    TxtCriteria.Focus();
                }
                if (Dv.Table.Columns[1].ColumnName.Contains("NO") == true || Dv.Table.Columns[1].ColumnName.Contains("CODE") == true)
                {
                    Dv.Sort = Dv.Table.Columns[1].ColumnName + " DESC";
                }
                else
                {
                    Dv.Sort = Dv.Table.Columns[1].ColumnName + " ASC";
                }
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
                for (int i = 0; i <= Dt.Columns.Count - 1; i++)
                {
                    if (Dt.Columns[i].ColumnName.ToUpper().Contains("AMOUNT") || Dt.Columns[i].ColumnName.ToUpper().Contains("PRICE") || Dt.Columns[i].ColumnName.ToUpper().Contains("RATE") || Dt.Columns[i].ColumnName.ToUpper().Contains("_PER"))
                    {
                        if (DGV.Columns[i].Visible == true)
                        {
                            DGV.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            DGV.Columns[i].DefaultCellStyle.Format = "0.00";
                            DGV.Columns[i].Width = 100;
                        }
                    }
                    else if (Dt.Columns[i].ColumnName.ToUpper().Contains("SLNO") || Dt.Columns[i].ColumnName.ToUpper().Contains("QMT") || Dt.Columns[i].ColumnName.ToUpper().Contains("PCI") || Dt.Columns[i].ColumnName.ToUpper().Contains("ITEMS") || Dt.Columns[i].ColumnName.ToUpper().Contains("BILLS"))
                    {
                        if (DGV.Columns[i].Visible == true)
                        {
                            if (Dt.Columns[i].ColumnName.ToUpper().Contains("QMT") || Dt.Columns[i].ColumnName.ToUpper().Contains("PCI") || Dt.Columns[i].ColumnName.ToUpper().Contains("ITEMS") || Dt.Columns[i].ColumnName.ToUpper().Contains("BILLS"))
                            {
                                DGV.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            }
                            DGV.Columns[i].Width = 60;
                        }
                    }
                }
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
                Org_Sql = Sql.ToUpper();
                Grid_Data(Sql, false);
                Column_Width(dataGridView1.Columns.Count, Col);
                Fill_Columns();
                ItemCount();
                Grid_Design(ref dataGridView1, Dv.Table);
                if (ViewMode == false)
                {
                    NormalView();
                    ViewColumn = false; 
                }
                else
                {
                    AddressView();
                    ViewColumn = true;
                }
                if (CmbFilter.Items.Count <= 1)
                {
                    SingleColumn(true);
                }
                if (Related_Word.Trim() != String.Empty)
                {
                    TxtCriteria.Text = Related_Word;
                }
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
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "TxtCriteria")
                    {
                        e.Handled = true;
                        if (dataGridView1.Rows.Count > 0)
                        {
                            dataGridView1_KeyDown(dataGridView1, e);
                        }
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    if (this.ActiveControl.Name == "TxtCriteria")
                    {
                        this.Close();
                    }
                    else
                    {
                        TxtCriteria.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        public void Grid_Data(String Sql, Boolean FromCondition)
        {
            try
            {
                //===============================================================
                // Selection Tool Connection String Configuration
                //===============================================================
                // Using ODBCConnection
                //dataGridView1.DataSource = MyBase.Load_DataWithAuto(Sql, out Dv);
                // Using SqlConnection
                if (Sql.Contains(" TOP ") == false)
                {
                    Sql = Sql.ToUpper().Replace("SELECT ", "SELECT TOP 100 ");
                }
                C_Sql = Sql;
                dataGridView1.DataSource = MyBase.Load_DataWithAuto_SqlCn(Sql, out Dv); 
                //===============================================================
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.Columns[0].Visible = false;
                for (int i = 0; i <= Dv.Table.Columns.Count - 1; i++)
                {
                    if (Dv.Table.Columns[i].ColumnName.ToUpper().Contains("AMOUNT") || Dv.Table.Columns[i].ColumnName.ToUpper().Contains("RATE"))
                    {
                        dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    else
                    {
                        dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }
                }
                for (int i=0;i<=dataGridView1.Columns.Count - 1;i++)
                {
                    dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
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
                CmbFilter.Items.Clear(); 
                foreach (DataColumn dc in Dv.Table.Columns)
                {
                    if (dataGridView1.Columns[dc.ColumnName].Visible == true)
                    {
                        CmbFilter.Items.Add(dc.ColumnName);
                    }
                }
                if (Dv.Table.Columns.Count == 1)
                {
                    CmbFilter.Enabled = false;
                }
                else
                {
                    CmbFilter.Enabled = true;
                }
                CmbFilter.SelectedIndex = 0; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void TxtCriteria_TextChanged(object sender, System.EventArgs e)
        {
            String Str = String.Empty;
            String Condition = String.Empty;
            String Filter = String.Empty;
            String DCol = String.Empty;
            String Text = String.Empty;
            try
            {
                if (Err_Change == false)
                {
                    Filter = MyBase.Org_Column_Name(Org_Sql, CmbFilter.Text);
                }
                else
                {
                    Filter = MyBase.Table_Alias(Org_Sql, CmbFilter.Text);
                }
                Text = TxtCriteria.Text;
                if (Txt_No == 0)
                {
                    if (TxtCriteria.Text.Trim() == string.Empty)
                    {
                        Condition = string.Empty;
                    }
                    else
                    {
                        if (CmbCondition.Text.Contains("Equal") == true)
                        {
                            Condition = "" + Filter + " = " + Text;
                        }
                        else if (CmbCondition.Text.Contains("Greater") == true)
                        {
                            Condition = "" + Filter + " > " + Text;
                        }
                        else if (CmbCondition.Text.Contains("Less") == true)
                        {
                            Condition = "" + Filter + " < " + Text;
                        }
                    }
                }
                else if (Txt_No == 2)
                {
                    if (CmbCondition.Text.Contains("Not Like") == true)
                    {
                        Condition = "" + Filter + " Not like '" + Text + "%'";
                    }
                    else if (CmbCondition.Text.Contains("Part") == true)
                    {
                        Condition = "" + Filter + " like '%" + Text + "%'";
                    }
                    else
                    {
                        Condition = "" + Filter + " like '" + Text + "%'";
                    }
                }
                if (Condition.Trim() != String.Empty)
                {
                    if (Condition.Contains("SUM(") || Condition.Contains("COUNT("))
                    {
                        if (Org_Sql.Contains("ORDER BY ") == false)
                        {
                            Str = " HAVING " + Condition;
                            Org_Sql1 = Org_Sql + Str;
                        }
                        else
                        {
                            Org_Sql1 = Org_Sql.Replace("ORDER BY ", "HAVING " + Condition + " ORDER BY ");
                        }
                    }
                    else
                    {
                        if (Org_Sql.Contains("WHERE ") == false)
                        {
                            if (Org_Sql.Contains("GROUP BY ") == false)
                            {
                                if (Org_Sql.Contains("ORDER BY ") == false)
                                {
                                    Str = " WHERE " + Condition;
                                    Org_Sql1 = Org_Sql + Str;
                                }
                                else
                                {
                                    Org_Sql1 = Org_Sql.Replace("ORDER BY ", " WHERE " + Condition + " ORDER BY ");
                                }
                            }
                            else
                            {
                                Org_Sql1 = Org_Sql.Replace("GROUP BY ", " WHERE " + Condition + " GROUP BY ");
                            }
                        }
                        else
                        {
                            Org_Sql1 = Org_Sql.Replace("WHERE ", " WHERE " + Condition + " AND ");
                        }
                    }
                    Grid_Data(Org_Sql1, true);
                }
                else
                {
                    Grid_Data(Org_Sql, false);
                }
                if (Err_Change)
                {
                    Err_Change = false;
                }
                //if (TxtCriteria.Text.Trim() != String.Empty)
                //{
                //    if (Txt_No == 0)
                //    {
                //        if (CmbCondition.Text.Contains("Equal") == true)
                //        {
                //            Dv.RowFilter = "" + CmbFilter.Text + " = " + TxtCriteria.Text;
                //        }
                //        else if (CmbCondition.Text.Contains("Greater") == true)
                //        {
                //            Dv.RowFilter = "" + CmbFilter.Text + " > " + TxtCriteria.Text;
                //        }
                //        else if (CmbCondition.Text.Contains("Less") == true)
                //        {
                //            Dv.RowFilter = "" + CmbFilter.Text + " < " + TxtCriteria.Text;
                //        }
                //        Dv.Sort = "" + CmbFilter.Text + " DESC";
                //    }
                //    else if (Txt_No == 2)
                //    {
                //        if (CmbCondition.Text.Contains("Not Like") == true)
                //        {
                //            Dv.RowFilter = "" + CmbFilter.Text + " Not like '" + TxtCriteria.Text + "%'";
                //        }
                //        else if (CmbCondition.Text.Contains("Part") == true)
                //        {
                            
                //            Dv.RowFilter = "" + CmbFilter.Text + " like '%" + TxtCriteria.Text + "%'";
                //        }
                //        else
                //        {
                //            Dv.RowFilter = "" + CmbFilter.Text + " like '" + TxtCriteria.Text + "%'";
                //        }
                //        if (CmbFilter.Text.Contains("NO") == true || CmbFilter.Text.Contains("CODE") == true)
                //        {
                //            Dv.Sort = "" + CmbFilter.Text + " DESC";
                //        }
                //        else
                //        {
                //            Dv.Sort = "" + CmbFilter.Text + " ASC";
                //        }
                //    }
                //}
                //else
                //{
                //    Dv.RowFilter = null;
                //}
                //if (dataGridView1.Columns["ID"].Visible == true)
                //{
                //    dataGridView1.Columns["ID"].Visible = false;
                //}
                //ItemCount();
                //if (ViewColumn == true)
                //{
                //    GridClick();
                //}
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Ambiguous column"))
                {
                    Err_Change = true;
                    TxtCriteria_TextChanged(sender, e);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void TxtTo_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (Txt_No == 0)
                {
                    if (CmbCondition.Text.Contains("Between") == true)
                    {
                        if (TxtFrom.Text.Trim() != String.Empty && TxtTo.Text.Trim() != String.Empty)
                        {
                            Dv.RowFilter = "" + CmbFilter.Text + " >= " + Convert.ToInt64(TxtFrom.Text) + " and " + CmbFilter.Text + " <=" + Convert.ToInt64(TxtTo.Text);
                            Dv.Sort = "" + CmbFilter.Text + " DESC";
                        }
                    }
                }
                ItemCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        void TxtFrom_TextChanged(object sender, System.EventArgs e)
        {
            TxtTo_TextChanged(TxtTo, e);
        }

        void TxtCriteria_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (CmbFilter.Text.Trim() != String.Empty)
                {
                    if (Txt_No == 0)
                    {
                        Valid_Decimal(TxtCriteria, e);
                    }
                    else if (Txt_No == 2)
                    {
                        Return_Ucase(e);
                    }
                }
                else
                {
                    Valid_Null(TxtCriteria, e);
                }
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
                if (e.KeyCode == Keys.Down)
                {
                    dataGridView1.Focus();
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1_KeyDown(TxtCriteria, e);
                    }
                    e.Handled = true;
                }
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
                Text_Clear();
                if (Dv.Table.Columns[CmbFilter.SelectedIndex + 1].DataType == System.Type.GetType("System.Decimal") || Dv.Table.Columns[CmbFilter.SelectedIndex + 1].DataType == System.Type.GetType("System.Double") || Dv.Table.Columns[CmbFilter.SelectedIndex + 1].DataType == System.Type.GetType("System.Int32") || Dv.Table.Columns[CmbFilter.SelectedIndex + 1].DataType == System.Type.GetType("System.Int64"))   
                {
                    Txt_No = 0;
                    Condition(0); 
                }
                else if (Dv.Table.Columns[CmbFilter.SelectedIndex + 1].DataType == System.Type.GetType("System.DateTime"))   
                {
                    Txt_No = 1;
                    Condition(1);
                }
                else
                {
                    Txt_No = 2;
                    Condition(2);
                }
                Order_Method(); 
                TxtCriteria.Focus();  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        void Condition(int i)
        {
            CmbCondition.Items.Clear();
            if (i == 0)
            {
                CmbCondition.Items.Add("Equal To");
                CmbCondition.Items.Add("Greater Than");
                CmbCondition.Items.Add("Less Than");
                CmbCondition.Items.Add("Between (1-10)");
                CmbCondition.SelectedIndex = 0;
                CmbCondition.Enabled = true;
                Dv.Sort = CmbFilter.Text + " DESC";
            }
            else if (i == 1)
            {
                CmbCondition.Items.Add("Between");
                CmbCondition.SelectedIndex = 0;
                CmbCondition.Enabled = false;
                Dv.Sort = CmbFilter.Text + " DESC";
            }
            else
            {
                CmbCondition.Items.Add("Starts With");
                CmbCondition.Items.Add("Part");
                CmbCondition.Items.Add("Not Like");
                CmbCondition.SelectedIndex = 0;
                CmbCondition.Enabled = true;
                if (CmbFilter.Text.Contains("NO") == true || CmbFilter.Text.Contains("CODE") == true)
                {
                    Dv.Sort = CmbFilter.Text + " DESC";
                }
                else
                {
                    Dv.Sort = CmbFilter.Text + " ASC";
                }
            }
        }

        void Text_Clear()
        {
            TxtCriteria.Text = String.Empty;
            TxtFrom.Text = String.Empty;
            TxtTo.Text = String.Empty;  
        }

        void Valid_Number(TextBox txt,System.Windows.Forms.KeyPressEventArgs e)   
        {
            try
            {
                if (Convert.ToInt16(e.KeyChar) > 47 && Convert.ToInt16(e.KeyChar) < 58 || Convert.ToInt16(e.KeyChar) == 8)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
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
                if (Convert.ToInt16(e.KeyChar) > 47 && Convert.ToInt16(e.KeyChar) < 58 || Convert.ToInt16(e.KeyChar) == 46 || Convert.ToInt16(e.KeyChar) == 8 )
                {
                    if (Convert.ToInt16(e.KeyChar) == 46)
                    {
                        if (txt.Text.Contains(".") == true)
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            if (txt.Text.Trim() != String.Empty)
                            {
                                e.Handled = false;
                            }
                            else
                            {
                                e.Handled = true;
                            }
                        }
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
                else
                {
                    e.Handled = true;
                }
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
                Text_Clear();
                DateTime Date = DateTime.Today; 
                panel1.Visible = true;
                panel2.Visible = false; 
                if (Txt_No == 0 || Txt_No == 1)
                {
                    if (CmbCondition.Text.Contains("Between") == true)
                    {
                        panel1.Visible = false;
                        panel2.Visible = true;
                        TxtFrom.Top = TxtCriteria.Top;
                        TxtTo.Top = TxtCriteria.Top;
                        DtFrom.Top = TxtCriteria.Top;
                        DtTo.Top = TxtCriteria.Top;
                        if (Txt_No == 0)
                        {
                            TxtFrom.Visible = true;
                            TxtTo.Visible = true;
                            DtFrom.Visible = false;
                            DtTo.Visible = false;
                            TxtFrom.Text = String.Empty;
                            TxtTo.Text = String.Empty;
                            TxtFrom.Focus(); 
                        }
                        else
                        {
                            TxtFrom.Visible = false;
                            TxtTo.Visible = false;
                            DtFrom.Text = String.Format("{0:dd/MM/yyyy}", Date);
                            DtTo.Text = String.Format("{0:dd/MM/yyyy}", Date);
                            DtFrom.Visible = true;
                            DtTo.Visible = true;
                            DtFrom.Focus(); 
                        }
                    }
                    else
                    {
                        panel1.Visible = true;
                        panel2.Visible = false;
                        TxtCriteria.Focus();  
                    }
                }
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
                if (e.KeyCode == Keys.Enter)
                {
                    if (Txt_No == 1)
                    {
                        if (CmbCondition.Text.Contains("Between") == true)
                        {
                            Dv.RowFilter = "" + CmbFilter.Text + " >= #" + String.Format("{0:MM/dd/yyyy}",DtFrom.Value) + "# and " + CmbFilter.Text + " <= #" + String.Format("{0:MM/dd/yyyy}",DtTo.Value) + "#";
                            Dv.Sort = "" + CmbFilter.Text + " DESC";
                        }
                    }
                }
                ItemCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        void DtTo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            DtFrom_KeyDown(DtFrom, e); 
        }


        void TxtFrom_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (Txt_No == 0)
                {
                    Valid_Decimal(TxtFrom, e);  
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

        void TxtTo_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (Txt_No == 0)
                {
                    Valid_Decimal(TxtTo, e);
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

        void Column_Width(int Column, params int[] nos)
        {
            try
            {
                int j = 1;
                foreach (int i in nos)
                {
                    dataGridView1.Columns[j].Width = i;
                    j++;
                }
                if (j < Column)
                {
                    for (int i = j; i <= Column-1; i++)
                    {
                        dataGridView1.Columns[i].Visible = false;  
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void dataGridView1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (ViewColumn == true)
            {
                dataGridView1_Click(dataGridView1, e);
            }
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

        void Down()
        {
            Int32 Row = 0;
            try
            {
                Row = dataGridView1.CurrentCell.RowIndex;
                if (Row >= dataGridView1.Rows.Count - 10)
                {
                    if (dataGridView1.Rows.Count % 50 == 0)
                    {
                        C_Sql = C_Sql.Replace(" TOP " + Convert.ToInt64(dataGridView1.Rows.Count) + " ", " TOP " + Convert.ToInt64(dataGridView1.Rows.Count + 150) + " ");
                        Grid_Data(C_Sql, false);
                        dataGridView1.CurrentCell = dataGridView1[1, Convert.ToInt32(Row)];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void dataGridView1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Int32 Row = 0;
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                dataGridView1_DoubleClick(dataGridView1, e);
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                Row = dataGridView1.CurrentCell.RowIndex;
                if (Row >= dataGridView1.Rows.Count - 20)
                {
                    if (dataGridView1.Rows.Count % 50 == 0)
                    {
                        C_Sql = C_Sql.Replace(" TOP " + Convert.ToInt64(dataGridView1.Rows.Count) + " ", " TOP " + Convert.ToInt64(dataGridView1.Rows.Count + 550) + " ");
                        Grid_Data(C_Sql, false);
                        dataGridView1.CurrentCell = dataGridView1[1, Convert.ToInt32(Row)];
                    }
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                Down();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Form1_KeyDown(dataGridView1, e);
            }
        }

        void GridClick()
        {
            Int32 id;
            try
            {
                if (ViewColumn == true)
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        if (dataGridView1.CurrentCell != null)
                        {
                            id = Convert.ToInt32(dataGridView1["Id", dataGridView1.CurrentCell.RowIndex].Value);
                            Selected_Row = Dv.Table.Rows[id];
                            AddressView();
                        }
                    }
                }
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
                GridClick();
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
                if (ViewColumn == true)
                {
                    Approval = true;
                }
                else
                {
                    id = Convert.ToInt32(dataGridView1["Id", dataGridView1.CurrentCell.RowIndex].Value);
                    Selected_Row = Dv.Table.Rows[id];
                    Approval = true;
                }
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

        public String LPad(String Sql, int length)
        {
            String StrL;
            try
            {
                if (Sql.Length <= length)
                {
                    StrL = Sql + Spaces(length - Sql.Length); 
                }
                else if (Sql.Length >= length)
                {
                    StrL = Sql.Substring(1, length); 
                }
                else
                {
                    StrL = Sql;
                }
                return StrL;
            }
            catch (Exception ex)
            {
                throw ex;
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

        void ItemCount()
        {

            try
            {
                //toolStripLabel2.Text = "ItemCount - " + dataGridView1.Rows.Count;
                toolStripLabel2.Text = String.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void NormalView()
        {
            try
            {
                GBox1.Top = GBox2.Top;
                GBox2.Visible = false;
                this.Height = GBox1.Height + 55;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void TxtAddress_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                Valid_Null(TxtAddress, e); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }
        
        void TxtTin_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                Valid_Null(TxtTin, e); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        void TxtCST_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                Valid_Null(TxtCST, e); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }


        void AddressView()
        {
            try
            {
                if (Selected_Row != null)
                {
                    if (Selected_Row.Table.Columns.Count > 3)
                    {
                        if (Selected_Row[dataGridView1.Columns.Count - 1] != null)
                        {
                            TxtCST.Text = Convert.ToString(Selected_Row[dataGridView1.Columns.Count - 1]);
                        }
                        else
                        {
                            TxtCST.Text = String.Empty; 
                        }
                        label4.Text = dataGridView1.Columns[dataGridView1.Columns.Count - 1].Name;
                        if (Selected_Row[dataGridView1.Columns.Count - 2] != null)
                        {
                            TxtTin.Text = Convert.ToString(Selected_Row[dataGridView1.Columns.Count - 2]);
                        }
                        else
                        {
                            TxtTin.Text = String.Empty; 
                        }
                        label3.Text = dataGridView1.Columns[dataGridView1.Columns.Count - 2].Name;
                        if (Selected_Row[dataGridView1.Columns.Count - 3] != null)
                        {
                            TxtAddress.Text = Convert.ToString(Selected_Row[dataGridView1.Columns.Count - 3]);
                        }
                        else
                        {
                            TxtAddress.Text = String.Empty;  
                        }
                        label2.Text = dataGridView1.Columns[dataGridView1.Columns.Count - 3].Name;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void SingleColumn(Boolean Value)
        {
            try
            {
                CmbFilter.Visible = false;
                CmbCondition.Left = CmbFilter.Left;
                panel1.Left = CmbCondition.Left + CmbCondition.Width + 10;
                panel2.Left = CmbCondition.Left + CmbCondition.Width + 10;
                dataGridView1.Width = CmbCondition.Width + panel1.Width + 5;
                GBox1.Width = dataGridView1.Width + 13;
                this.Width = GBox1.Width + 15; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
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

        void Tool_Status()
        {
            try
            {
                ToolStripLabel1.Width = (statusStrip1.Width - 20);
                //ToolStripLabel3.Width = (statusStrip1.Width / 3);
                //toolStripLabel2.Width = Convert.ToInt16((statusStrip1.Width / 3) * .75);
                ToolStripLabel1.Text = "Enter To Select / Esc To Quit";
                //ToolStripLabel3.Text = "List Order";
                ToolStripLabel1.TextAlign = ContentAlignment.MiddleCenter;
                //toolStripLabel2.TextAlign = ContentAlignment.MiddleRight;
                //ToolStripLabel3.TextAlign = ContentAlignment.MiddleCenter; 
                ItemCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ToolStripLabel3_DropDownItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            try
            {
                //ToolStripLabel3.Text = e.ClickedItem.Text;
                Order_Method(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        void Order_Method()
        {
            try
            {
                //if (ToolStripLabel3.Text == "Descending")
                //{
                //    Dv.Sort = "" + CmbFilter.Text + " DESC";
                //}
                //else if (ToolStripLabel3.Text == "List Order")
                //{
                //    Dv.Sort = "" + CmbFilter.Text + " DESC";
                //}
                //else 
                //{
                //    Dv.Sort = "" + CmbFilter.Text + " Asc";
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Dv.Table.Columns[0].Caption == "Id")
                {
                    dataGridView1.Columns[0].Visible = false;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FrmSelectionToolItem_KeyPress(object sender, KeyPressEventArgs e)
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