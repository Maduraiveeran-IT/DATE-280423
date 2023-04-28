/*
 * DEVELOPER : RAMKUMAR B
 * TABLES USED : Machine_Slnowise_Breakup_Master, Details, Item_Details
 * PROC USED : Main_MAchine_Link_Save_Proc
 * FUNCTIONS USED : Project_Grn_DEtails_Fn()
 * CREATED DATE : N/A
 * CREATION TICKET : N/A
 * CORRECTION DATE : 13-JAN-2023
 * CORRECTION TICKET : T-12919
 * VERIFIED BY : LIVINGSTONE K / 13-JAN-2023
 * 
 * TICKET NO : T-13691
 * CORRECTION DATE : 13-FEB-2023
 * CORRECTION : CHANGING THE COMPANY CODE TO ADAPT VMAIN COMPANY CODE
 * BY : RAMKUMAR B
 * VERIFIED BY : LIVINGSTOE K
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.IO;

namespace Accounts
{
    public partial class FrmMachSlno : Form,Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();        
        DataRow Dr;
        Int64 Code;        
        TextBox Txt = null;
        String[] Queries;        
        DataTable[] DtSlno;
        TextBox Txt_Sl = null;
        String str = null;
        int temp_row, cmp = 0;
        Int32 proj_div_Code;

        public FrmMachSlno()
        {
            InitializeComponent();
        }

        private void FrmMachSlno_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
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
                DtpDate.Enabled = false;
                TxtGrnNo.Enabled = true;
                TxtSupplier.Enabled = true;
                txtCmp.Enabled = true;
                txtDiv.Enabled = true;
                DtSlno = new DataTable[500];
                btnCancel.Enabled = true;
                GBSlno.Visible = false;
                TxtGrnNo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Save()
        {
            try
            {
                Int64 Array_Index = 0;
                Grid.CurrentCell = Grid["Desc_1", 0];
                TxtGrnNo.Focus();

                if (TxtGrnNo.Text.Trim().ToString() == string.Empty)
                {
                    MessageBox.Show("Invalid GRN Number...!", "Gainup");
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtSupplier.Text.Trim().ToString() == string.Empty)
                {
                    MessageBox.Show("Invalid Supplier...!", "Gainup");
                    MyParent.Save_Error = true;
                    return;
                }
                if (txtCmp.Text.Trim().ToString() == string.Empty)
                {
                    MessageBox.Show("Invalid Company...!", "Gainup");
                    MyParent.Save_Error = true;
                    return;
                }
                if (txtDiv.Text.Trim().ToString() == string.Empty)
                {
                    MessageBox.Show("Invalid Division...!", "Gainup");
                    MyParent.Save_Error = true;
                    return;
                }

                for (int i = 0; i < Grid.Rows.Count; i++)
                {
                    for (int j = 0; j < Grid.Columns.Count; j++)
                    {
                        if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == null || Grid[j, i].Value.ToString() == string.Empty)
                        {
                            MessageBox.Show("Invalid " + Grid.Columns[j].Name + " Details...!", "Gainup");
                            Grid.Focus();
                            Grid.CurrentCell = Grid[j, i];
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                        else if (Grid.Columns[j].Name == "Alt_Qty")
                        {
                            if (Convert.ToDouble(Grid[j, i].Value) > 0)
                            {
                                if (DtSlno[i] == null)
                                {
                                    MessageBox.Show("Invalid Breakup Details for Row " + (i + 1));
                                    Grid_Slno_Load(i);
                                    GBSlno.Visible = true;
                                    Grid.Enabled = false;
                                    GridSlno.Focus();
                                    GridSlno.CurrentCell = GridSlno[1, 0];
                                    GridSlno.BeginEdit(true);
                                    MyParent.Save_Error = true;
                                    return;
                                }
                                else
                                {
                                    for (int k = 0; k < DtSlno[i].Rows.Count; k++)
                                    {
                                        for (int l = 0; l < DtSlno[i].Columns.Count - 1; l++)
                                        {
                                            if (DtSlno[i].Rows[k][l].ToString() == string.Empty || DtSlno[i].Rows[k][l].ToString() == null)
                                            {
                                                MessageBox.Show("Invalid Breakup Details for Row " + (i + 1));
                                                Grid_Slno_Load(i);
                                                GBSlno.Visible = true;
                                                Grid.Enabled = false;
                                                GridSlno.Focus();
                                                GridSlno.CurrentCell = GridSlno[l, k];
                                                GridSlno.BeginEdit(true);
                                                MyParent.Save_Error = true;
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (txtCmp.Tag.ToString() == 1.ToString())
                {
                    txtCmp.Tag = 4;
                }
                else if (txtCmp.Tag.ToString() == 3.ToString())
                {
                    txtCmp.Tag = 5;
                }
                else if (txtCmp.Tag.ToString() == 9.ToString())
                {
                    txtCmp.Tag = 6;
                }
                
                Queries = new String[Grid.Rows.Count * 100 + 250];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert Into Machine_Slnowise_Breakup_Master (Grn_Rowid, Comp_Code, Division_Code,entry_cmp_code) Values (" + TxtGrnNo.Tag.ToString() + "," + cmp.ToString() + "," + txtDiv.Tag.ToString() + "," + txtCmp.Tag.ToString() + "); Select Scope_Identity()";
                    Queries[Array_Index++] = MyParent.EntryLog("Machine_Slnowise_Breakup_Master", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Machine_Slnowise_Breakup_Master Set Grn_Rowid = " + TxtGrnNo.Tag.ToString() + ", Comp_Code = " + cmp.ToString() + ", Division_Code = " + txtDiv.Tag.ToString() + ", entry_cmp_code = " + txtCmp.Tag.ToString() + " Where Rowid = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Machine_Slnowise_Breakup_Master", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete From Machine_Slnowise_Breakup_Detail Where Master_ID = " + Code;
                    Queries[Array_Index++] = "Delete from Machine_Slnowise_Breakup_Item_Details Where Master_ID = " + Code;
                }

                for (int i = 0; i < Grid.Rows.Count; i++)
                {
                    if (Convert.ToDouble(Grid["Alt_Qty", i].Value) > 0)
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert Into Machine_Slnowise_Breakup_Item_Details (Master_ID, Slno, Item_ID, Color_Id, Size_Ide, Qty) Values (@@IDENTITY," + (i + 1) + "," + Grid["Item_ID", i].Value.ToString() + "," + Grid["Color_ID", i].Value.ToString() + "," + Grid["Size_ID", i].Value.ToString() + "," + Grid["Alt_qty", i].Value.ToString() + ")";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert Into Machine_Slnowise_Breakup_Item_Details (Master_ID, Slno, Item_ID, Color_Id, Size_Ide, Qty) Values (" + Code + "," + (i + 1) + "," + Grid["Item_ID", i].Value.ToString() + "," + Grid["Color_ID", i].Value.ToString() + "," + Grid["Size_ID", i].Value.ToString() + "," + Grid["Alt_qty", i].Value.ToString() + ")";
                        }

                        for (int j = 0; j < DtSlno[i].Rows.Count; j++)
                        {
                            if (MyParent._New)
                            {
                                Queries[Array_Index++] = "Insert Into Machine_Slnowise_Breakup_Detail (Master_ID, Mach_Type_ID, Make, Model, Machine_No, Serial_No, Item_Details_Slno) Values (@@IDENTITY,'" + DtSlno[i].Rows[j]["Mach_Type_ID"].ToString() + "','" + DtSlno[i].Rows[j]["Make"].ToString() + "','" + DtSlno[i].Rows[j]["Model"].ToString() + "','" + DtSlno[i].Rows[j]["Machine_No"].ToString() + "','" + DtSlno[i].Rows[j]["Serial_no"].ToString() + "'," + (i + 1) + ")";
                            }
                            else
                            {
                                Queries[Array_Index++] = "Insert Into Machine_Slnowise_Breakup_Detail (Master_ID, Mach_Type_ID, Make, Model, Machine_No, Serial_No, Item_Details_Slno) Values (" + Code + ",'" + DtSlno[i].Rows[j]["Mach_Type_ID"].ToString() + "','" + DtSlno[i].Rows[j]["Make"].ToString() + "','" + DtSlno[i].Rows[j]["Model"].ToString() + "','" + DtSlno[i].Rows[j]["Machine_No"].ToString() + "','" + DtSlno[i].Rows[j]["Serial_no"].ToString() + "'," + (i + 1) + ")";
                            }
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

                MyBase.Run("Exec Main_MAchine_Link_Save_Proc " + txtCmp.Tag.ToString() + ",'" + MyParent.YearCode + "'");

                MyParent.Save_Error = false;
                MessageBox.Show("Saved ..!", "Gainup");
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                DtpDate.Enabled = false;
                TxtGrnNo.Enabled = true;
                TxtSupplier.Enabled = true;
                txtCmp.Enabled = true;
                txtDiv.Enabled = true;
                GBSlno.Visible = false;

                str = " Select distinct C.GRNNo, C.GRNDate, C.Supplier, C.Item DESC_1, C.Color DESC_2, C.Size DESC_3, B.Qty, E.Division_Name, D.CompName, A.Rowid, A.Grn_Rowid, ";
                str += " C.Supplier_Code, B.Item_ID, B.Color_Id, B.Size_Ide Size_ID, A.Division_Code, A.Comp_Code, A.entry_cmp_code ";
                str += " from Machine_Slnowise_Breakup_Master A ";
                str += " Inner Join Machine_Slnowise_Breakup_Item_Details B On A.Rowid = B.Master_ID ";
                str += " Inner Join Project_Grn_DEtails_Fn() C on A.Grn_Rowid = C.RowID and B.Item_ID = C.Item_ID and B.Color_Id = C.Color_ID and B.Size_Ide = C.Size_ID ";
                str += " Inner Join Projects_Companymas D On A.entry_cmp_code = D.CompCode ";
                str += " Inner Join ACCOUNTS.dbo.Division_master E on A.entry_cmp_code = E.COMPANY_CODE and A.Division_Code = E.Division_Code and E.Year_Code = '" + MyParent.YearCode + "' ";

                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "EDIT", str, string.Empty, 100, 100, 100, 100, 100, 100, 50, 80, 80);

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

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                DtpDate.Enabled = false;
                TxtGrnNo.Enabled = true;
                TxtSupplier.Enabled = true;
                txtCmp.Enabled = true;
                txtDiv.Enabled = true;
                GBSlno.Visible = false;

                str = " Select distinct C.GRNNo, C.GRNDate, C.Supplier, C.Item DESC_1, C.Color DESC_2, C.Size DESC_3, B.Qty, E.Division_Name, D.CompName, A.Rowid, A.Grn_Rowid, ";
                str += " C.Supplier_Code, B.Item_ID, B.Color_Id, B.Size_Ide Size_ID, A.Division_Code, A.Comp_Code, A.entry_cmp_code ";
                str += " from Machine_Slnowise_Breakup_Master A ";
                str += " Inner Join Machine_Slnowise_Breakup_Item_Details B On A.Rowid = B.Master_ID ";
                str += " Inner Join Project_Grn_DEtails_Fn() C on A.Grn_Rowid = C.RowID and B.Item_ID = C.Item_ID and B.Color_Id = C.Color_ID and B.Size_Ide = C.Size_ID ";
                str += " Inner Join Projects_Companymas D On A.entry_cmp_code = D.CompCode ";
                str += " Inner Join ACCOUNTS.dbo.Division_master E on A.entry_cmp_code = E.COMPANY_CODE and A.Division_Code = E.Division_Code and E.Year_Code = '" + MyParent.YearCode + "' ";

                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "DELETE", str, string.Empty, 100, 100, 100, 100, 100, 100, 50, 80, 80);

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
                MyBase.Run("Delete From Machine_Slnowise_Breakup_Detail Where Master_ID = " + Code + "; Delete from Machine_Slnowise_Breakup_Item_Details Where Master_ID = " + Code);
                MyBase.Run("Delete From Machine_Slnowise_Breakup_Master Where Rowid = " + Code);
                MessageBox.Show("Deleted", "Gainup");
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
                DtpDate.Enabled = false;
                TxtGrnNo.Enabled = true;
                TxtSupplier.Enabled = true;
                txtCmp.Enabled = true;
                txtDiv.Enabled = true;
                GBSlno.Visible = false;

                str = " Select distinct C.GRNNo, C.GRNDate, C.Supplier, C.Item DESC_1, C.Color DESC_2, C.Size DESC_3, B.Qty, E.Division_Name, D.CompName, A.Rowid, A.Grn_Rowid, ";
                str += " C.Supplier_Code, B.Item_ID, B.Color_Id, B.Size_Ide Size_ID, A.Division_Code, A.Comp_Code, A.entry_cmp_code ";
                str += " from Machine_Slnowise_Breakup_Master A ";
                str += " Inner Join Machine_Slnowise_Breakup_Item_Details B On A.Rowid = B.Master_ID ";
                str += " Inner Join Project_Grn_DEtails_Fn() C on A.Grn_Rowid = C.RowID and B.Item_ID = C.Item_ID and B.Color_Id = C.Color_ID and B.Size_Ide = C.Size_ID ";
                str += " Inner Join Projects_Companymas D On A.entry_cmp_code = D.CompCode ";
                str += " Inner Join ACCOUNTS.dbo.Division_master E on A.entry_cmp_code = E.COMPANY_CODE and A.Division_Code = E.Division_Code and E.Year_Code = '" + MyParent.YearCode + "' ";

                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "VIEW", str, string.Empty, 100, 100, 100, 100, 100, 100, 50, 80, 80);

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
                
        private void FrmMachSlno_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == TxtGrnNo.Name)
                    {
                        txtCmp.Focus();
                        return;
                    }
                    else if (this.ActiveControl.Name == txtCmp.Name)
                    {
                        txtDiv.Focus();
                        return;
                    }
                    else if (this.ActiveControl.Name == txtDiv.Name)
                    {
                        Grid.Focus();
                        Grid.CurrentCell = Grid["Alt_Qty", 0];
                        Grid.BeginEdit(true);
                        return;
                    }
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == TxtGrnNo.Name)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select GRN", "select distinct GRNNo, GRNDate, Supplier, Item, Color, Size, Rowid, Supplier_Code,Company_Code from Project_Grn_DEtails_Fn() where Proj_ACtivity_Name like '%machin%' ", String.Empty, 150, 100, 150, 250, 150, 150);
                        if (Dr != null)
                        {
                            TxtGrnNo.Text = Dr["Grnno"].ToString();
                            TxtGrnNo.Tag = Dr["Rowid"].ToString();
                            DtpDate.Value = Convert.ToDateTime(Dr["Grndate"].ToString());
                            TxtSupplier.Tag = Dr["Supplier_Code"].ToString();
                            TxtSupplier.Text = Dr["Supplier"].ToString();
                            cmp = Convert.ToInt16(Dr["Company_Code"].ToString());
                            Grid_Data();
                        }
                    }
                    
                    if (this.ActiveControl.Name == txtCmp.Name)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Company", "select CompName, CompCode from Projects_Companymas order by CompCode", String.Empty, 250);
                        if (Dr != null)
                        {
                            txtCmp.Text = Dr["CompName"].ToString();
                            txtCmp.Tag = Dr["Compcode"].ToString();
                        }
                    }

                    if (this.ActiveControl.Name == txtDiv.Name)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Division", "select Division_Name, Division_Code, Div_Code from Acc_Division_Master(" + txtCmp.Tag + ",'" + MyParent.YearCode + "')", String.Empty, 250);
                        if (Dr != null)
                        {
                            txtDiv.Text = Dr["Division_Name"].ToString();
                            txtDiv.Tag = Dr["Division_Code"].ToString();
                            proj_div_Code = Convert.ToInt32(Dr["Div_Code"].ToString());
                            TxtGrnNo.Enabled = false;
                            TxtSupplier.Enabled = false;
                            txtCmp.Enabled = false;
                            txtDiv.Enabled = false;
                            Grid.Focus();
                            Grid.CurrentCell = Grid["Alt_Qty", 0];
                            Grid.BeginEdit(true);
                            return;
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

        private void FrmMachSlno_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name != String.Empty)
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

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                if (MyParent.View)
                {
                    btnCancel.Enabled = false;
                }
                else
                {
                    btnCancel.Enabled = true;
                }
                Code = Convert.ToInt64(Dr["Rowid"].ToString());
                TxtGrnNo.Text = Dr["Grnno"].ToString();
                TxtGrnNo.Tag = Dr["Grn_Rowid"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["GrnDate"].ToString());
                txtCmp.Text = Dr["CompName"].ToString();
                txtCmp.Tag = Dr["entry_cmp_code"].ToString();
                txtDiv.Text = Dr["Division_Name"].ToString();
                txtDiv.Tag = Dr["Division_Code"].ToString();
                TxtSupplier.Text = Dr["Supplier"].ToString();
                TxtSupplier.Tag = Dr["Supplier_Code"].ToString();
                cmp = Convert.ToInt16(Dr["COmp_Code"].ToString());
                Grid_Data();
                Grid_Slno_Edit_View();
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
                    str = "select Distinct 0 Slno, Item Desc_1, Color Desc_2, Size Desc_3, (Grn_Qty - Isnull(sum(C.Qty),0)) Qty, 0 Alt_Qty, A.Item_ID, A.Color_ID, Size_ID from Project_Grn_DEtails_Fn() A Left JOin Machine_Slnowise_Breakup_Master B on A.RowID = B.Grn_Rowid Left Join Machine_Slnowise_Breakup_Item_Details C on B.Rowid = C.Master_ID And A.Item_ID = C.Item_ID And A.Color_ID = C.Color_Id And A.Size_ID = C.Size_Ide Where A.RowID = " + TxtGrnNo.Tag + " group by Item , Color , Size , A.Item_ID, A.Color_ID, Size_ID, Grn_Qty having (Grn_Qty - Isnull(sum(C.Qty),0)) > 0";
                }
                else
                {
                    str = " Select distinct 0 Slno, C.Item DESC_1, C.Color DESC_2, C.Size DESC_3, C.Grn_Qty Qty, B.Qty Alt_Qty, ";
                    str += " B.Item_ID, B.Color_Id, B.Size_Ide Size_ID ";
                    str += " from Machine_Slnowise_Breakup_Master A ";
                    str += " Inner Join Machine_Slnowise_Breakup_Item_Details B On A.Rowid = B.Master_ID ";
                    str += " Inner Join Project_Grn_DEtails_Fn() C on A.Grn_Rowid = C.RowID and B.Item_ID = C.Item_ID and B.Color_Id = C.Color_ID and B.Size_Ide = C.Size_ID ";
                    str += " where A.Rowid = " + Code + " and C.Rowid = " + TxtGrnNo.Tag + " ";
                }

                Grid.DataSource = MyBase.Load_Data(str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid, "Alt_QTY");
                Grid.Columns["Alt_QTY"].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                MyBase.Grid_Designing(ref Grid, ref Dt, "ITEM_ID", "COLOR_ID", "SIZE_ID");
                MyBase.Grid_Width(ref Grid, 40, 300, 300, 300, 90, 80);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Grid_Slno_Load(int row)
        {
            try
            {
                temp_row = row;

                if (DtSlno[row] == null)
                {
                    DtSlno[row] = new DataTable();
                    if (MyParent._New)
                    {
                        str = "select no Slno, '' Mach_Type, '' Make, '" + Grid["Desc_3", row].Value.ToString() + "' Model, '' Machine_No, '' Serial_No, 0 Mach_Type_ID, '' T from Number_Series(1," + Grid["Alt_Qty",row].Value + ")";
                        MyBase.Load_Data(str, ref DtSlno[row]);
                    }
                    else if (MyParent.Edit)
                    {
                        str = "select no Slno, '' Mach_Type, '' Make, '' Model, '' Machine_No, '' Serial_No, 0 Mach_Type_ID, '' T from Number_Series(1," + Grid["Alt_Qty", row].Value + ")";
                        MyBase.Load_Data(str, ref DtSlno[row]);
                    }
                }

                GridSlno.DataSource = DtSlno[row];
                MyBase.Grid_Colouring(ref GridSlno, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref GridSlno, "Mach_Type", "Make", "Model", "Machine_No", "Serial_No");
                MyBase.Grid_Designing(ref GridSlno, ref DtSlno[row], "T", "Mach_Type_ID");
                MyBase.Grid_Width(ref GridSlno, 50, 200, 200, 200, 200, 200);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Grid_Slno_Edit_View()
        {
            try
            {
                DataTable dt_tmp = new DataTable();
                String st_tmp = null;

                st_tmp = "Select A.Rowid, B.Slno from Machine_Slnowise_Breakup_Master A Inner Join Machine_Slnowise_Breakup_Item_Details B on A.Rowid = B.Master_ID Where A.rowid = " + Code;
                MyBase.Load_Data(st_tmp, ref dt_tmp);

                for (int i = 0; i < dt_tmp.Rows.Count; i++)
                {
                    DtSlno[i] = new DataTable();
                    str = "select 0 Slno, B.Name Mach_Type, A.Make, A.Model, A.Machine_No, A.Serial_No, A.Mach_Type_ID, '' T From Machine_Slnowise_Breakup_Detail A left join VAAHINI_ERP_GAINUP.dbo.main_object_master B ON A.Mach_Type_Id = B.RowID Where A.Master_ID = " + Code + " and A.Item_Details_Slno = " + dt_tmp.Rows[i]["Slno"].ToString() + " ";
                    MyBase.Load_Data(str, ref DtSlno[i]);
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
                if (e.KeyCode == Keys.Enter)
                {
                    if (Convert.ToDouble(Grid["Qty", Grid.CurrentCell.RowIndex].Value) < Convert.ToDouble(Grid["Alt_Qty", Grid.CurrentCell.RowIndex].Value))
                    {
                        MessageBox.Show("Allocated Quantity Can't be Greater Then GRN Quantity...!", "Gainup");
                        Grid.Focus();
                        Grid.CurrentCell = Grid["Alt_Qty", Grid.CurrentCell.RowIndex];
                        Grid.BeginEdit(true);
                        return;
                    }
                    else
                    {
                        if (Grid.CurrentCell == Grid["Alt_Qty", Grid.CurrentCell.RowIndex])
                        {
                            if (Grid.CurrentCell.Value == DBNull.Value || Grid.CurrentCell.Value.ToString() == string.Empty)
                            {
                            }
                            else
                            {
                                if (Convert.ToDouble(Grid.CurrentCell.Value) > 0)
                                {
                                    GBSlno.Visible = true;
                                    Grid.Enabled = false;
                                    Grid_Slno_Load(Grid.CurrentCell.RowIndex);
                                    GridSlno.Focus();
                                    GridSlno.CurrentCell = GridSlno["Mach_Type", 0];
                                    GridSlno.BeginEdit(true);
                                }
                            }
                        }
                    }
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
                if (Grid.Rows.Count >= 1)
                {
                    MyBase.Row_Number(ref Grid);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < GridSlno.Rows.Count; i++)
                {
                    for (int j = 0; j < GridSlno.Columns.Count - 1; j++)
                    {
                        if (GridSlno[j, i].Value == DBNull.Value || GridSlno[j, i].Value.ToString() == null || GridSlno[j, i].Value.ToString() == string.Empty)
                        {
                            MessageBox.Show("Details Can't be Empty...!", "Gainup");
                            GridSlno.Focus();
                            GridSlno.CurrentCell = GridSlno[j, i];
                            GridSlno.BeginEdit(true);
                            return;
                        }
                    }
                }
                GBSlno.Visible = false;
                Grid.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DtSlno[temp_row] = null;
                GBSlno.Visible = false;
                Grid.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridSlno_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Sl == null)
                {
                    Txt_Sl = (TextBox)e.Control;
                    Txt_Sl.KeyDown += new KeyEventHandler(Txt_Sl_KeyDown);
                    Txt_Sl.KeyPress += new KeyPressEventHandler(Txt_Sl_KeyPress);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Sl_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GridSlno.CurrentCell.ColumnIndex == GridSlno.Columns["Mach_Type"].Index)
                {
                    MyBase.Valid_Null(Txt_Sl, e);
                }
                else
                {
                    MyBase.Return_Ucase(e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Sl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (GridSlno.CurrentCell.ColumnIndex == GridSlno.Columns["Mach_Type"].Index)
                    {
                        Dr = Tool.Selection_Tool(this, 70, 30, SelectionTool_Class.ViewType.NormalView, "Machine Type", "select Name,Rowid from VAAHINI_ERP_GAINUP.dbo.main_object_master where Dept_ID = " + proj_div_Code + " and Type = 'M' and Name not like 'ZZZ%'", string.Empty, 250);
                        if (Dr != null)
                        {
                            GridSlno["Mach_Type", GridSlno.CurrentCell.RowIndex].Value = Dr["Name"].ToString();
                            GridSlno["Mach_Type_ID", GridSlno.CurrentCell.RowIndex].Value = Dr["Rowid"].ToString();
                            Txt_Sl.Text = Dr["Name"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridSlno_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try 
            {
                if (!MyParent._New)
                {
                    if (GridSlno.Rows.Count >= 1)
                    {
                        MyBase.Row_Number(ref GridSlno);
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
