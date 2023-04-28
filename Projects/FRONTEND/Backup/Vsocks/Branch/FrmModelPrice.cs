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
    public partial class FrmModelPrice : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        String Str;
        DataRow Dr;
        DataTable Dt = new DataTable();
        DataTable TmpDt = new DataTable();
        TextBox Txt = null;
        Int64 Code = 0;
        public FrmModelPrice()
        {
            InitializeComponent();
        }

        private void FrmModelPrice_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)MdiParent;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                DtpDate.Value = MyBase.GetServerDateTime();                
                Grid_Data();
                Grid.CurrentCell = Grid["Model_Name", 0];
                Grid.Focus();
                Grid.BeginEdit(true);
                return;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Entry_Cancel()
        {
            try
            {
                MyBase.Clear(this);
                Grid_Data();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Save()
        {
            String[] Queries;
            Int32 Array_Index = 0;
            try
            {
                Total_Count();
                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Entry ...!", "Gainup");
                    MyParent.Save_Error = true;
                    DtpDate.Focus();
                    return;
                }

                if (TxtTotal.Text.Trim() == String.Empty || Convert.ToDouble(TxtTotal.Text.ToString()) <= 0)
                {
                    MessageBox.Show("Invalid Total ...!", "Gainup");
                    MyParent.Save_Error = true;
                    DtpDate.Focus();
                    return;
                }                

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Dt.Rows[i]["Exp_Price"]) <= 0 && Convert.ToDouble(Dt.Rows[i]["Loc_Price"]) <= 0)
                    {
                        MessageBox.Show("Invalid Rate", "Gainup");
                        Grid.CurrentCell = Grid["Model_Name", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                }


                TxtEntry_No.Text = MyBase.MaxOnlyComp("Socks_ModelWise_Rate_Master", "Entry_No", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();
                Queries = new string[Dt.Rows.Count + 7];

                if (MyParent._New)
                {
                    if (MyParent.UserCode == 25 || MyParent.UserCode == 64)
                    {
                        Queries[Array_Index++] = "Insert into Socks_ModelWise_Rate_Master (Entry_No, Effect_From,  Remarks, Company_Code, Year_Code) values (" + TxtEntry_No.Text + ", '" + String.Format("{0:dd-MMM-yyyy} {0:T}", DtpDate.Value) + "',  '" + TxtRemarks.Text + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "'); Select Scope_Identity() ";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Socks_ModelWise_Rate_Master (Entry_No, Effect_From,  Remarks, Company_Code, Year_Code) values (" + TxtEntry_No.Text + ", '" + String.Format("{0:dd-MMM-yyyy} {0:T}", DtpDate.Value) + "',  '" + TxtRemarks.Text + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "'); Select Scope_Identity() ";
                    }
                    Queries[Array_Index++] = MyParent.EntryLog("ORDER COST", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_ModelWise_Rate_Master Set  Remarks = '" + TxtRemarks.Text + "' Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("MODEL COST", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from Socks_ModelWise_Rate_Detail where Approval_Flag = 'F' and Master_ID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Socks_ModelWise_Rate_Detail (Master_ID, Slno, Model_ID,  Rate, Rate_Inr) Values (@@IDENTITY, " + Dt.Rows[i]["Slno"].ToString() + ", " + Dt.Rows[i]["Model_ID"].ToString() + ",  " + Dt.Rows[i]["Exp_Price"].ToString() + ", " + Dt.Rows[i]["Loc_Price"].ToString() + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Socks_ModelWise_Rate_Detail (Master_ID, Slno, Model_ID, Rate, Rate_Inr) Values (" + Code + ", " + Dt.Rows[i]["Slno"].ToString() + ", " + Dt.Rows[i]["Model_ID"].ToString() + ",  " + Dt.Rows[i]["Exp_Price"].ToString() + ", " + Dt.Rows[i]["Loc_Price"].ToString() + ")";
                    }
                }

                if (MyParent._New)
                {
                    MyBase.Run_Identity(false, Queries);
                }
                else
                {
                    MyBase.Run_Identity(true, Queries);
                }
                MyParent.Save_Error = false;
                MessageBox.Show("Saved ..!", "Gainup");
                MyBase.Clear(this);
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Order Cost- Delete", "Select A.Entry_No ENo, A.Effect_From, B.Model_Name, C.Rate Exp_Price, C.Rate_Inr Loc_Price,  A.Remarks, A.RowID, C.Model_Id From  Socks_ModelWise_Rate_Master A Left Join Socks_ModelWise_Rate_Detail C on A.Rowid = C.master_Id Inner Join socks_model B on C.Model_Id = B.Rowid Where C.Approved_Flag='F' ", String.Empty, 80, 150, 250, 80, 200);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    MyParent.Load_DeleteConfirmEntry();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void Entry_Delete_Confirm()
        {
            try
            {
                if (Code > 0 && Dt.Rows.Count > 0)
                {
                    MyBase.Run("Delete from Socks_ModelWise_Rate_Detail where  Approval_Flag = 'F' and Master_ID = " + Code, "Delete from Socks_ModelWise_Rate_Master where Rowid = " + Code, MyParent.EntryLog("Model Price", "DELETE", Code.ToString()));
                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyParent.Load_DeleteEntry();
                }
                else
                {
                    MessageBox.Show("Invalid Entry to Delete ...!", "Gainup");
                    MyParent.Load_DeleteEntry();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                Code = Convert.ToInt64(Dr["RowID"]);
                TxtEntry_No.Tag = Convert.ToInt64(Dr["RowID"]);
                TxtEntry_No.Text = Dr["ENo"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["Effect_From"]);
                TxtRemarks.Text = Dr["Remarks"].ToString();                
                //TxtTotal.Text = Dr["Total"].ToString();                
                Grid_Data();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Model Price - Edit", "Select A.Entry_No ENo, A.Effect_From, B.Model_Name, C.Rate Exp_Price, C.Rate_Inr Loc_Price,   A.Remarks, A.RowID, C.Model_Id From  Socks_ModelWise_Rate_Master A Left Join Socks_ModelWise_Rate_Detail C on A.Rowid = C.master_Id Inner Join socks_model B on C.Model_Id = B.Rowid Where A.Effect_From = Cast(getdate() as Date)", String.Empty, 80, 150, 250, 80, 200);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Model Price - View", "Select A.Entry_No ENo, A.Effect_From, B.Model_Name, C.Rate Exp_Price, C.Rate_Inr Loc_Price,  A.Remarks, A.RowID, C.Model_Id From  Socks_ModelWise_Rate_Master A Left Join Socks_ModelWise_Rate_Detail C on A.Rowid = C.master_Id Inner Join socks_model B on C.Model_Id = B.Rowid Where C.Approved_Flag='T'  ", String.Empty, 80, 150, 250, 80, 200);
                if (Dr != null)
                {
                    Fill_Datas(Dr);                 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Print()
        {
            try
            {
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
                if (MyParent._New)
                {
                    Str = "Select '' Slno, '' Model_Name, 0.00 Exp_Price, 0.00 Loc_Price, 0 Model_Id From Socks_ModelWise_Rate_Detail Where 1=2 ";
                }
                else if (MyParent.Edit == true || MyParent.Delete == true)
                {
                    Str = "Select A.Slno, B.Model_Name, A.Rate Exp_Price, A.Rate_Inr Loc_Price, A.Model_Id From  Socks_ModelWise_Rate_Detail A Inner Join Socks_Model B on A.Model_Id = B.Rowid Inner Join Socks_ModelWise_Rate_Master C on A.Master_Id = C.Rowid Where C.RowID = " + Code + "  and A.Approved_Flag = 'F' Order By A.Slno ";
                    
                }
                else
                {
                    Str = "Select A.Slno, B.Model_Name, A.Rate Exp_Price,  A.Rate_Inr Loc_Price, A.Model_Id From  Socks_ModelWise_Rate_Detail A Inner Join Socks_Model B on A.Model_Id = B.Rowid  Inner Join Socks_ModelWise_Rate_Master C on A.Master_Id = C.Rowid Where C.RowID = " + Code + "  and A.Approved_Flag = 'T' Order By A.Slno ";
                }
                Dt = new DataTable();
                Grid.DataSource = null;
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Model_ID");
                MyBase.ReadOnly_Grid(ref Grid, "Slno");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 70, 300, 80, 80);
                Grid.RowHeadersWidth = 40;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmModelPrice_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "DtpDate")
                    {
                        Grid.CurrentCell = Grid["Model_Name", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                    if (this.ActiveControl.Name == "TxtTotal")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name != "TxtRemarks")
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    if (this.ActiveControl.Name == "TxtRemarks")
                    {
                        TxtTotal.Focus();
                    }
                    else
                    {
                        MyBase.ActiveForm_Close(this, MyParent);
                    }

                }
                else if (e.KeyCode == Keys.Down)
                {
                    
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    MyBase.ActiveForm_Close(this, MyParent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmModelPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == String.Empty)
                    {
                        MyBase.Valid_Decimal((TextBox)this.ActiveControl, e);
                    }
                    else if (this.ActiveControl.Name != "TxtRemarks")
                    {
                        MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                    }
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
                    Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                    Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);
                    Txt.GotFocus += new EventHandler(Txt_GotFocus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_GotFocus(object sender, EventArgs e)
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

        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Total_Count();
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {

                    TxtRemarks.Focus();
                    return;
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Exp_Price"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["Loc_Price"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
                }
                else
                {
                    MyBase.Valid_Null(Txt, e);
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
                //if (TxtOrder.Text.ToString().Trim() == String.Empty)
                //{
                //    MessageBox.Show("Please Select Order_No...!", "Gainup");
                //    TxtOrder.Focus();
                //    return;
                //}
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Model_Name"].Index)
                    {
//                        Str = "Select Model_Name, VAAHINI_ERP_GAINUP.Dbo.ModelWise_Rate_Fn_Socks('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', Rowid) Rate, VAAHINI_ERP_GAINUP.Dbo.ModelWise_Rate_Fn_Socks_Local('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', Rowid) Inr_Rate,  Rowid Model_Id From Socks_Model";
                        Str = "Select A.Model_Name, VAAHINI_ERP_GAINUP.Dbo.ModelWise_Rate_Fn_Socks('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', A.Rowid) Rate, VAAHINI_ERP_GAINUP.Dbo.ModelWise_Rate_Fn_Socks_Local('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', A.Rowid) Inr_Rate,  A.Rowid Model_Id From Socks_Model A Left Join Socks_ModelWise_Rate_Master B On '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "' = B.Effect_From left join Socks_ModelWise_Rate_Detail C On B.RowID = C.Master_ID and A.Rowid = C.Model_ID Where C.Model_ID Is Null";
                        Dr = Tool.Selection_Tool_Except_New("Model_Name", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Model...!", Str, String.Empty, 300, 100);                        
                         if (Dr != null)
                            {
                                Txt.Text = Dr["Model_Name"].ToString();
                                Grid["Model_Name", Grid.CurrentCell.RowIndex].Value = Dr["Model_Name"].ToString();
                                Grid["Model_Id", Grid.CurrentCell.RowIndex].Value = Dr["Model_Id"].ToString();
                                Grid["Exp_Price", Grid.CurrentCell.RowIndex].Value = Dr["Rate"].ToString();
                                Grid["Loc_Price", Grid.CurrentCell.RowIndex].Value = Dr["Inr_Rate"].ToString();
                                Total_Count();
                            }
                        
                    }                    
                }

                else
                {
                    Total_Count();
                    e.Handled = true;
                }

                if (e.KeyCode == Keys.Escape)
                {
                    TxtRemarks.Focus();
                }
                Total_Count();
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
                if (Grid.Rows.Count - 1 > Grid.CurrentCell.RowIndex)
                {
                    if (MessageBox.Show("Are you sure to Remove..?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                        //Grid.Rows.RemoveAt(Grid.CurrentCell.RowIndex); 
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToUpper().Contains("THERE IS NO ROW"))
                {
                    Grid.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Total_Count()
        {
            Double Kgs = 0;
            try
            {
                TxtTotal.Text = String.Format("{0:0.00}", Convert.ToDouble(MyBase.Sum(ref Grid, "Exp_Price")));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Grid_Leave(object sender, EventArgs e)
        {
            try
            {
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
