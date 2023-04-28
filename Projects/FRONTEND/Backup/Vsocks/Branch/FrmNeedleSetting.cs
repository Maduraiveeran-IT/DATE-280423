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
    public partial class FrmNeedleSetting : Form,Entry  
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataGrid dgv = new DataGrid();
        DataRow Dr;        
        Int64 Code;           
        TextBox Txt = null;        
        String[] Queries;
        String Str;
        Int32 B =0;        
        public FrmNeedleSetting()
        {
            InitializeComponent();
        }
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                Grid_Data();
                TxtYear.Focus();
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
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Needle - Edit", " Select Year, Week, Effect_From, C.Machine, D.Name Needle, A.RowID, B.Master_ID, B.Machine_ID, B.Needle_ID  From  Socks_Needle_Change_Master A Inner Join Socks_Needle_Change_Details B On A.RowID = B.Master_ID Inner Join Knitting_Mc_NO() C On B.Machine_ID = C.Machine_ID Inner Join VFit_Sample_Needle_Master D On B.Needle_ID = D.RowID Order by A.RowID Desc, B.RowID  ", String.Empty, 80, 80, 80, 100, 120);
                if (Dr != null)
                {
                    Fill_Datas(Dr);                                        
                    Grid.CurrentCell = Grid["Needle", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
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
                TxtYear.Text = Dr["Year"].ToString();
                TxtWeek.Text = Dr["Week"].ToString();
                Grid_Data();
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Entry_Save()
        {
            try
            {
                Int32 Array_Index = 0;
                Total_Count();
                if (TxtYear.Text.Trim() == string.Empty || TxtWeek.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Year & Week", "Gainup");
                    TxtYear.Focus();
                }
                if (TxtTot.Text.Trim() == string.Empty || Convert.ToDouble(TxtTot.Text) == 0)
                {
                    MessageBox.Show("Invalid Needle Selection", "Gainup");
                    Grid.CurrentCell = Grid["MACHINE", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 2; j++)
                    {
                        if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty)
                        {
                            MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                            Grid.CurrentCell = Grid[j, i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                }
                if (MyParent._New)
                {
                    Queries = new String[Dt.Rows.Count + 3];
                    DataTable TDt = new DataTable();
                    Queries[Array_Index++] = "Insert Into Socks_Needle_Change_Master (Year, Week, Effect_From) Values (" + TxtYear.Text + ", " + TxtWeek.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "'); Select Scope_Identity()";
                }
                else
                {
                    Queries = new String[Dt.Rows.Count + 3];
                    Queries[Array_Index++] = "Update Socks_Needle_Change_Master Set  Year =  " + TxtYear.Text + " , Week =  " + TxtWeek.Text + " Where Rowid = " + Code;
                    Queries[Array_Index++] = "Delete From Socks_Needle_Change_Details Where Master_id = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Grid["NEEDLE_ID", i].Value) > 0)
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into Socks_Needle_Change_Details (Master_ID, Machine_ID, Needle_ID)  Values (@@IDENTITY, " + Grid["Machine_ID", i].Value + ", " + Grid["Needle_Id", i].Value + ")";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert into Socks_Needle_Change_Details (Master_ID, Machine_ID, Needle_ID)  Values (" + Code + ", " + Grid["Machine_ID", i].Value + ", " + Grid["Needle_Id", i].Value + ")";
                        }
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
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Print()
        {
            try
            {
                MyBase.Clear(this);      
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Needle - Delete", " Select Year, Week, Effect_From, C.Machine, D.Name Needle, A.RowID, B.Master_ID, B.Machine_ID, B.Needle_ID  From  Socks_Needle_Change_Master A Inner Join Socks_Needle_Change_Details B On A.RowID = B.Master_ID Inner Join Knitting_Mc_NO() C On B.Machine_ID = C.Machine_ID Inner Join VFit_Sample_Needle_Master D On B.Needle_ID = D.RowID Order by A.RowID Desc, B.RowID  ", String.Empty, 80, 80, 80, 100, 120);
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
                if (Code > 0)
                {
                    MyBase.Run("Delete From Socks_Needle_Change_Details Where Master_ID = " + Code, " Delete From Socks_Needle_Change_Master Where RowID = " + Code);
                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyBase.Clear(this);
                }
                MyParent.Load_DeleteEntry();
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
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Needle - View", " Select Year, Week, Effect_From, C.Machine, D.Name Needle, A.RowID, B.Master_ID, B.Machine_ID, B.Needle_ID  From  Socks_Needle_Change_Master A Inner Join Socks_Needle_Change_Details B On A.RowID = B.Master_ID Inner Join Knitting_Mc_NO() C On B.Machine_ID = C.Machine_ID Inner Join VFit_Sample_Needle_Master D On B.Needle_ID = D.RowID Order by A.RowID Desc, B.RowID  ", String.Empty, 80, 80, 80, 100, 120);
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

        void Grid_Data()
        {
            String Str = String.Empty;          
            try            
            {
                if (MyParent._New == true)
                {
                        Str = "Select 0 SNO, '' MACHINE, '' NEEDLE, 0 Machine_ID, 0 Needle_ID, '' Needle_ID1, '' T ";
                }
                else
                {
                    Str = "Select 0 SNO, C.MACHINE, D.Name NEEDLE, B.Machine_ID, B.Needle_ID, D.Name Needle_ID1, '' T From  Socks_Needle_Change_Master A Inner Join Socks_Needle_Change_Details B On A.RowID = B.Master_ID Inner Join Knitting_Mc_NO() C On B.Machine_ID = C.Machine_ID Inner Join VFit_Sample_Needle_Master D On B.Needle_ID = D.RowID Where A.RowID = " + Code  + "  Order by A.RowID Desc, B.RowID  ";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);                               
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid, "MACHINE", "NEEDLE");  
                MyBase.Grid_Designing(ref Grid, ref Dt, "Machine_ID", "Needle_ID", "T", "Needle_ID1");
                MyBase.Grid_Width(ref Grid, 50, 150, 150);
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["MACHINE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["NEEDLE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;                
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;               
            }
            catch (Exception ex)
            {
                throw ex;
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Needle"].Index)
                {
                    if (Grid.CurrentCell.RowIndex > 0 && Grid["Needle", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["Needle", Grid.CurrentCell.RowIndex].Value = Grid["Needle", Grid.CurrentCell.RowIndex - 1].Value;
                        Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value = Grid["Needle_ID", Grid.CurrentCell.RowIndex - 1].Value;
                        Txt.Text = Grid["Needle", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                    }
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
                   if (e.KeyCode == Keys.Down)
                    {
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["MACHINE"].Index)
                        {
                            Dr = Tool.Selection_Tool_Except_New("Machine", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "MACHINE", "Select Machine, Dbo.Get_Needle_For_Week_Date(Machine_ID, " + TxtYear.Text + " , " + TxtWeek.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "') Current_Needle, Machine_ID  From Knitting_Mc_NO () Order by Machine_ID", String.Empty, 200, 150);
                            if (Dr != null)
                            {
                                Grid["MACHINE", Grid.CurrentCell.RowIndex].Value = Dr["Machine"].ToString();
                                Grid["Machine_ID", Grid.CurrentCell.RowIndex].Value = Dr["Machine_ID"].ToString();
                                Grid["Needle_ID1", Grid.CurrentCell.RowIndex].Value = Dr["Current_Needle"].ToString();
                                Txt.Text = Dr["Machine"].ToString();
                            }
                        }
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["NEEDLE"].Index)
                        {
                            if (Grid["MACHINE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            {
                                MessageBox.Show("Invalid Machine", "Gainup");
                                Grid.CurrentCell = Grid["MACHINE", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "NEEDLE", "Select Distinct A.Name Needle, A.RowID Needle_ID From  VFit_Sample_Needle_Master A  Left Join Socks_Needle_Change_Master B On B.Year = " + TxtYear.Text + " and B.Week = " + TxtWeek.Text + " Left Join Socks_Needle_Change_Details  C On C.Master_ID = B.RowID and C.Needle_ID =A.RowID and C.Machine_ID = " + Grid["Machine_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " Where C.Machine_ID Is Null And A.Active = 'Y' and A.Name != '" + Grid["Needle_ID1", Grid.CurrentCell.RowIndex].Value.ToString() + "' Order by A.Name, A.RowID", String.Empty, 200);
                            if (Dr != null)
                            {
                                Grid["NEEDLE", Grid.CurrentCell.RowIndex].Value = Dr["Needle"].ToString();
                                Grid["Needle_ID", Grid.CurrentCell.RowIndex].Value = Dr["Needle_ID"].ToString();
                                Txt.Text = Dr["Needle"].ToString();
                            }
                        }
                    }                
               Total_Count();               
               if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
        void Total_Count()
        {               
            try
            {                               
                TxtTot.Text = MyBase.Count(ref Grid, "MACHINE", "Needle_ID");                                    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {                
                MyBase.Valid_Null(Txt, e);                
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
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Total_Count();
                    TxtTot.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void FrmNeedleSetting_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmNeedleSetting_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {
                if (this.ActiveControl.Name != String.Empty)
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
            }
        }

        private void FrmNeedleSetting_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtTot")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    else if ((this.ActiveControl.Name == "TxtWeek") || (this.ActiveControl.Name == "TxtYear"))
                    {
                        Grid.CurrentCell = Grid["MACHINE", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        e.Handled = true;
                        return;         
                    }
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (MyParent._New == true)
                    {
                        if ((this.ActiveControl.Name == "TxtWeek") || (this.ActiveControl.Name == "TxtYear"))
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Year/Week", "Select Distinct DATENAME(YY,Edate) Year, CAst(DATENAME(WW,Edate)as Int) Week   from Date_series (GETDATE(), GETDATE() + 360) A Left Join Socks_Needle_Change_Master B On DATENAME(YY,Edate)  = DATENAME(YY,B.Effect_From) and  CAst(DATENAME(WW,Edate)as Int) = CAst(DATENAME(WW,B.Effect_From)as Int) Where B.Effect_From IS Null Order by DATENAME(YY,Edate), CAst(DATENAME(WW,Edate)as Int)", String.Empty, 200, 200);
                            if (Dr != null)
                            {
                                TxtWeek.Text = Dr["Week"].ToString();
                                TxtYear.Text = Dr["Year"].ToString();

                                DataTable Tdt = new DataTable();
                                MyBase.Load_Data("Select Week_SDate From Get_Week_Details() where Year = " + TxtYear.Text + " and week = " + TxtWeek.Text, ref Tdt);
                                DtpDate.Value = Convert.ToDateTime(Tdt.Rows[0][0]);

                                Grid_Data();
                            }
                        }

                    }
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

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
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

        private void Grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid);
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {            
            try
            {
                if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {                        
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
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
