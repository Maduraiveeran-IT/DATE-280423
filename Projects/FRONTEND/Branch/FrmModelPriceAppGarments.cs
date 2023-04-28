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
    public partial class FrmModelPriceAppGarments : Form
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        DataTable Dt = new DataTable();
        DataTable TempDt = new DataTable();
        DataRow Dr;
        Int64 Code;
        TextBox Txt = null;
        String S = "1";
        Int16 PCompCode;
        Boolean Status_Flag = false;
        public FrmModelPriceAppGarments()
        {
            InitializeComponent();
        }

        private void FrmModelPriceAppGarments_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                //Grid_Data();
                SendKeys.Send("{F5}");
                Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
            }
        }
        void Grid_Data()
        {
            String Str = String.Empty;
            try
            {
                //Str = "Select Row_Number() Over (Order by Model_Name) SNO, *, 1 T From (Select Distinct max(CONVERT(VARCHAR(10),A.Effect_From,103)) Effect_From, C.Model_Name, Cast(Max(B.Rate) as Numeric(20,2)) FC_Price, Cast(Max(B.Rate_Inr) as Numeric(20,2)) INR_Price, max(B.Master_ID)Master_ID, max(B.RowID) RowID From  FITSOCKS.Dbo.Socks_ModelWise_Rate_Master A Inner Join FITSOCKS.Dbo.Socks_ModelWise_Rate_Detail B On A.RowID = B.Master_ID Inner Join FITSOCKS.Dbo.Socks_Model C On B.Model_ID = C.Rowid 	Where B.Approved_Flag = 'f' Group By C.Model_Name) A Order by Row_Number() Over (Order by Effect_From, Model_Name) ";

                Str = "Select Row_Number() Over (Order by Model_Name) SNO, ";
                Str = Str + " O1.Effect_From, Model_Name, FC_Price, INR_Price, Master_ID, RowID, 1 T From ";
                Str = Str + " (Select Distinct CONVERT(VARCHAR(10),A.Effect_From,103) Effect_From, C.Model_Name, ";
                Str = Str + " Cast(B.Rate as Numeric(20,2)) FC_Price, Cast(B.Rate_Inr as Numeric(20,2)) INR_Price, ";
                Str = Str + " B.Master_ID Master_ID, B.RowID RowID, B.Model_ID, Effect_From Effect_From1 ";
                Str = Str + " From FITERP1314.Dbo.GArments_ModelWise_Rate_Master A ";
                Str = Str + " Inner Join FITERP1314.Dbo.GArments_ModelWise_Rate_Detail B On A.RowID = B.Master_ID ";
                Str = Str + " Inner Join FITERP1314.Dbo.GArments_Model C On B.Model_ID = C.Rowid and C.Model_Name Not like '%ZZZ%')O1 ";
                Str = Str + " Inner Join (Select B1.Model_ID, Max(A3.Effect_From)Effect_From From FITERP1314.Dbo.GArments_ModelWise_Rate_Master A3 ";
                Str = Str + " Inner Join FITERP1314.Dbo.GArments_ModelWise_Rate_Detail B1 On A3.RowID = B1.Master_ID ";
                Str = Str + " Where B1.Approved_Flag = 'f' ";
                Str = Str + " Group By B1.Model_ID)O2 On O1.MOdel_ID = O2.Model_ID And O1.Effect_From1 = O2.Effect_From ";
                Str = Str + " Order by Row_Number() Over (Order by Model_Name)";

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                
                MyBase.ReadOnly_Grid_Without(ref Grid);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Master_ID", "RowID", "T");
                //MyBase.ReadOnly_Grid_Without(ref Grid, "C_FROM", "C_TO", "DESCRIPTION", "SIZE", "ORIGIN", "PAIRS", "UOM");

                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["MODEL_NAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;                
                Grid.Columns["FC_PRICE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                Grid.Columns["INR_PRICE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (S != "1")
                {
                    Grid.Columns.Remove("Status");
                }
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                DataGridViewCheckBoxColumn Check = new DataGridViewCheckBoxColumn();
                Check.HeaderText = "Status";
                Check.Name = "Status";
                Check.ValueType = typeof(String);
                Check.Visible = true;
                Check.ReadOnly = false;
                Grid.Columns.Insert(1, Check);
                Status_Flag = true;
                Grid.Columns["Status"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                S = "0";
                MyBase.Grid_Width(ref Grid, 50, 100, 100, 300, 100, 100);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Grid["Status", i].Value = checkBox1.Checked;
                }
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
            }
        }

        private void FrmModelPriceAppGarments_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F5)
                {
                    MyBase.Clear(this);
                    Grid_Data();
                    checkBox1.Checked = true;
                    Grid.Focus();
                    Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name != "TxtRemarks")
                    {
                        e.Handled = true;
                        SendKeys.Send("{Tab}");
                    }

                }
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
            }
        }

        private void FrmModelPriceAppGarments_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name != "TxtRemarks")
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
            }
        }
        void Total_Amount()
        {
            Double Amount = 0;
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (Grid.Rows.Count > 1)
                {
                    // MyBase.Row_Number(ref Grid);
                }
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
            }
        }


        private void ButClear_Click(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                Grid_Data();
                checkBox1.Checked = false;
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
            }
        }

        private void ButExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
            }
        }
        private void ButApprove_Click(object sender, EventArgs e)
        {
            String[] Queries;
            Int32 Array_Index = 0;
            try
            {

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("No Records...!", "Gainup");
                    MyParent.Save_Error = true;
                    return;
                }

                Double C = 0;
                if (Dt.Rows.Count == Convert.ToDouble(MyBase.SumWithCondtion(ref Grid, "T", "Status", "false")) || Dt.Rows.Count == Convert.ToDouble(MyBase.SumWithCondtion(ref Grid, "T", "Status", String.Empty)))
                {
                    MessageBox.Show("No Records are Selected", "Gainup");
                    MyParent.Save_Error = true;
                    Grid.Focus();
                    return;
                }
                if (TxtRemarks.Text.ToString().Trim() == String.Empty)
                {
                    TxtRemarks.Text = " ";
                }                

                Queries = new string[Dt.Rows.Count * 2 + 2];

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["Status", i].Value != null && Grid["Status", i].Value.ToString().ToUpper() == "TRUE")
                    {
                        Queries[Array_Index++] = "Update FITERP1314.Dbo.GArments_ModelWise_Rate_Detail Set Approved_Flag='T', Approved_System = Host_Name(), Approved_Time = Getdate() Where Master_ID = " + Dt.Rows[i]["Master_ID"].ToString() + " and RowID = " + Dt.Rows[i]["RowID"].ToString() + "";
                        
                    }
                }

                if (MessageBox.Show("Are you sure to Approve...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    MyBase.Run_Identity(true, Queries);
                    MyParent.Save_Error = false;
                    MessageBox.Show("Approved..!", "Gainup");
                    MyBase.Clear(this);
                    Grid_Data();
                    if (Grid.Rows.Count > 1)
                    {
                        Grid.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MyBase.Show(ex.Message, this);
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Tab)
                {
                    TxtRemarks.Focus();
                }
            }
            catch (Exception ex)
            {
                MyBase.Show(ex.Message, this);
            }
        }     
    }
}
