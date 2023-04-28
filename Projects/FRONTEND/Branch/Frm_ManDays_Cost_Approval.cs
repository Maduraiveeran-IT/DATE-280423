using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Data.Odbc;
using System.IO;

namespace Accounts
{
    public partial class Frm_ManDays_Cost_Approval : Form
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataTable Dt2 = new DataTable();
        DataGrid dgv = new DataGrid();
        DataRow Dr;
        Int64 Code;
        Int32 C = 0;
        TextBox Txt = null;
        TextBox Txt_Qty = null;
        TextBox Txt_Img = null;
        DataTable[] DtImg;
        String[] Queries;
        String Str, SName = "";      

        public Frm_ManDays_Cost_Approval()
        {
            InitializeComponent();
        }

        private void Frm_ManDays_Cost_Approval_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)MdiParent;
                MyBase.Clear(this);
                TxtEno.Focus();
                DtpDate1.Enabled = false;
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
                Str = " Select M1.Eno, M1.EDate, M5.LedgeR_NAme Supplier, M3.Name Project, M2.Order_No, M4.Item Description, M2.Amount, M1.Rowid, M1.Remarks From PROJECTS.Dbo.Mandays_Entry_Master M1 ";
                Str = Str + " Left Join PROJECTS.Dbo.Mandays_Entry_Details M2 On M1.Rowid = M2.Master_Id Left Join PROJECTS.Dbo.Project_Name_Master M3 On M2.Project_Id = M3.Rowid ";
                Str = Str + " Left Join PROJECTS.Dbo.Item M4 On M2.Itemid = M4.ItemID Left Join PROJECTS.Dbo.Supplier_all_Fn_Co1()M5 On M1.Supplier_ID = M5.LedgeR_Code and M1.Company_Code = M5.Company_Code ";
                Str = Str + " Where Isnull(M1.Approved, 'N') = 'N' and M1.Company_Code = " + MyParent.CompCode + " Order By Eno ";

                Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "Select Entry Number - Approval...!", Str, String.Empty, 100, 100, 150, 150, 110, 110, 110);
                if (Dr != null)
                {
                    Fill_Datas();
                    TxtEno.Focus();
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
                Str = " Select ROW_NUMBER()Over(Order By M2.RowID)Slno, M3.Name Project, M2.Order_No, M4.Item Description, M2.Amount From PROJECTS.Dbo.Mandays_Entry_Master M1 ";
                Str = Str + " Left Join PROJECTS.Dbo.Mandays_Entry_Details M2 On M1.Rowid = M2.Master_Id Left Join PROJECTS.Dbo.Project_Name_Master M3 On M2.Project_Id = M3.Rowid ";
                Str = Str + " Left Join PROJECTS.Dbo.Item M4 On M2.Itemid = M4.ItemID Left Join PROJECTS.Dbo.Supplier_all_Fn_Co1()M5 On M1.Supplier_ID = M5.LedgeR_Code and M1.Company_Code = M5.Company_Code";
                Str = Str + " Where M1.RowID = " + TxtEno.Tag.ToString();
                Dt = new DataTable();
                Grid.DataSource = null;
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt);
                MyBase.ReadOnly_Grid(ref Grid);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 40, 200, 150, 150, 120);
                Grid.RowHeadersWidth = 40;
                Total_Count();
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
                TxtEno.Text = Dr["Eno"].ToString();
                TxtEno.Tag = Dr["RowID"].ToString();
                TxtSupplier.Text = Dr["Supplier"].ToString();
                DtpDate1.Value = Convert.ToDateTime(Dr["EDate"].ToString());
                TxtRemarks.Text = Dr["Remarks"].ToString();
                Grid_Data();
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Total_Count()
        {
            try
            {
                TxtTotOrder.Text = MyBase.Sum(ref Grid, "Amount");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Approve_Click(object sender, EventArgs e)
        {
            Int32 Array_Index = 0;
            DataTable chkdata = new DataTable();
            string check;
            try
            {
                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Approve...!", "Gainup");
                    TxtEno.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (MessageBox.Show("Sure to Approve...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }

                Queries = new string[Dt.Rows.Count];

                for (int k = 0; k <= Dt.Rows.Count - 1; k++)
                {
                    Queries[Array_Index++] = " Update PROJECTS.Dbo.Mandays_Entry_Master Set Approved = 'Y', Approved_System = Host_Name(), Approved_Time = Getdate() Where RowID = " + TxtEno.Tag.ToString();
                }
                MyBase.Run(Queries);
                MessageBox.Show("Approved..!", "Gainup");
                MyBase.Clear(this);
                TxtEno.Focus();
                MyParent.Save_Error = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MyParent.Save_Error = true;
            }
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                Entry_Edit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Exit_Click(object sender, EventArgs e)
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

        private void Frm_ManDays_Cost_Approval_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.ActiveControl.Name == TxtEno.Name)
                {
                    Btn_Approve.Focus();
                }
            }
            else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                if ((this.ActiveControl.Name == TxtEno.Name) || (this.ActiveControl.Name == TxtTotOrder.Name) || (this.ActiveControl.Name == TxtRemarks.Name))
                {
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (this.ActiveControl.Name == TxtEno.Name)
                {
                    Entry_Edit();
                }
            }
            else
            {
                //
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
                    Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                    Txt.Leave += new EventHandler(Txt_Leave);
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
                Total_Count();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void Txt_Leave(object sender, EventArgs e)
        {
            try
            {
                Total_Count();
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
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_ManDays_Cost_Approval_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == TxtEno.Name || this.ActiveControl.Name == TxtSupplier.Name || this.ActiveControl.Name == TxtRemarks.Name)
                {
                    e.Handled = true;
                }
                else if (this.ActiveControl.Name == TxtTotOrder.Name)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (this.ActiveControl.Name == "Grid")
                    {
                        Btn_Approve.Focus();
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
