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
    public partial class FrmDepartmentGroupEntry : Form,Entry  
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
        Int16 PCompCode;
        public FrmDepartmentGroupEntry()
        {
            InitializeComponent();
        }
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                Set_Min_Max_Date(true);
                //DtpEDate.Value = MyBase.GetServerDateTime();
                TxtDivision.Focus();
                TxtDivision.Text = "GAINUP - SOCKS";
                TxtDivision.Tag = 2;
                DtpEDate.Focus();  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Set_Min_Max_Date(Boolean Condition)
        {
            try
            {
                DataTable Tdt = new DataTable();
                if (Condition)
                {
                    MyBase.Load_Data("Select DateAdd (d, " + MyParent.User_Datelock + ", Cast(GetDate() as Date)) MinDate, Cast(GetDate() as Date) MaxDate ", ref Tdt);
                    DtpEDate.MinDate = Convert.ToDateTime(Tdt.Rows[0][0]);
                    DtpEDate.MaxDate = Convert.ToDateTime(Tdt.Rows[0][1]);
                }
                else
                {
                    DtpEDate.MinDate = Convert.ToDateTime("01-Apr-2014");
                    DtpEDate.MaxDate = Convert.ToDateTime("31-Mar-2030");
                }
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
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Department Group-Edit", "Select A.ENo, B.DEPTNAME, A.Effect_From, D.Shiftcode2 Shift, A.Group_Name, C.CompName Division, A.Total_Groups, A.Division_Code, A.RowID, A.Department_Code, A.Remarks, A.Shift_Code From Department_Group_MAster  A Left Join VAAHINI_ERP_GAINUP.dbo.DepTtype B On A.Department_Code = B.DEPTCODE and A.Division_Code = B.compcode LEft Join VAAHINI_ERP_GAINUP.dbo.Companymas_Pay C On C.CompCode = A.Division_Code LEft Join VAAHINI_ERP_GAINUP.dbo.shiftmst D On A.Division_Code = D.CompCode and D.Shiftcode2 Not Like '%Z' And Mode = 1 and A.Shift_Code = D.shiftcode  Where  A.Division_Code  = (Case When " + MyParent.CompCode + " = 2 Then 3 Else 2 End) Order By A.Effect_From Desc ", string.Empty, 80, 150, 100, 80, 120);
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

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                Code = Convert.ToInt64(Dr["RowId"]);
                DtpEDate.Value = Convert.ToDateTime(Dr["Effect_From"]);
                TxtEno.Text = Dr["ENo"].ToString();
                TxtDivision.Text = Dr["Division"].ToString();
                TxtDivision.Tag  = Dr["Division_Code"].ToString();   
                TxtDepartment.Text = Dr["DEPTNAME"].ToString();
                TxtDepartment.Tag = Convert.ToInt64(Dr["Department_Code"]);
                TxtGroupName.Text = Dr["Group_Name"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();
                TxtShift.Text = Dr["Shift"].ToString();
                TxtShift.Tag = Dr["Shift_Code"].ToString();
                Grid_Data();
                Total_Count();
                Grid.CurrentCell = Grid["MACHINE", 0];
                Grid.Focus();
                Grid.BeginEdit(true);
                return;
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
                if (TxtDepartment.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Department", "Gainup");
                    TxtDepartment.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtGroupName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid GroupName", "Gainup");
                    TxtGroupName.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtDivision.Text.Trim() == string.Empty )
                {
                    MessageBox.Show("Invalid Division", "Gainup");
                    TxtDivision.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtShift.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Shift", "Gainup");
                    TxtShift.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtTotal.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotal.Text) == 0)
                {
                    MessageBox.Show("Invalid Group Details", "Gainup");
                    TxtTotal.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 1; j++)
                    {
                        if (Grid["MACHINE", i].Value == DBNull.Value || Grid["MACHINE", i].Value.ToString() == String.Empty)
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
                    DataTable TDt1 = new DataTable();
                    MyBase.Load_Data("Select IsNull(Max(Eno + 1), 1) From Department_Group_MAster Where Division_Code = " + TxtDivision.Tag  + "", ref TDt1);
                    TxtEno.Text = TDt1.Rows[0][0].ToString();
                    if (TxtEno.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Invalid Entry No", "Gainup");
                        TxtEno.Focus();
                    }
                    Queries = new String[Dt.Rows.Count + 3];                    
                    DataTable TDt = new DataTable();
                    Queries[Array_Index++] = "Insert into Department_Group_Master(ENo, Effect_From, Division_Code, Department_Code, Group_Name, Total_Groups, Remarks, Shift_Code ) Values (" + TxtEno.Text + ", '" + String.Format("{0:dd-MMM-yyyy} {0:T}", DtpEDate.Value) + "', " + TxtDivision.Tag + ",  " + TxtDepartment.Tag + ",  '" + TxtGroupName.Text + "' , " + TxtTotal.Text + ", '" + TxtRemarks.Text + "', " + TxtShift.Tag + ") ; Select Scope_Identity()";
                    Queries[Array_Index++] = MyParent.EntryLog("DEPARTMENT GROUP", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries = new String[Dt.Rows.Count + 3];
                    Queries[Array_Index++] = "Update Department_Group_Master Set Group_Name  = '" + TxtGroupName.Text + "', Total_Groups = " + TxtTotal.Text + ", Remarks = '" + TxtRemarks.Text + "' Where RowID = " + Code + "";
                    Queries[Array_Index++] = "Delete From Department_Group_Details Where Master_ID= " + Code;                   
                    Queries[Array_Index++] = MyParent.EntryLog("DEPARTMENT GROUP", "EDIT", Code.ToString());
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {                    
                        if (MyParent._New)
                        {
                            //Queries[Array_Index++] = "Insert into Department_Group_Details (Master_ID, SlNo, Machine_Code, From_Type , To_Type, Machine_Name) Values (@@IDENTITY, " + (i + 1) + ", " + Grid["MACHINE_CODE", i].Value + ", " + Grid["FROM_TYPE", i].Value + ", " + Grid["TO_TYPE", i].Value + ", '" + Grid["MACHINE", i].Value.ToString() + "')";
                            Queries[Array_Index++] = "Insert into Department_Group_Details (Master_ID, SlNo, Machine_Code, Machine_Name) Values (@@IDENTITY, " + (i + 1) + ", " + Grid["MACHINE_CODE", i].Value + ", '" + Grid["MACHINE", i].Value.ToString() + "')";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert into Department_Group_Details (Master_ID, SlNo, Machine_Code, From_Type , To_Type, Machine_Name) Values (" + Code + ", " + (i + 1) + ", " + Grid["MACHINE_CODE", i].Value + ", NULL, NULL, '" + Grid["MACHINE", i].Value.ToString() + "')";
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
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Department Group-Delete", "Select A.ENo, B.DEPTNAME, A.Effect_From, D.Shiftcode2 Shift, A.Group_Name, C.CompName Division, A.Total_Groups, A.Division_Code, A.RowID, A.Department_Code, A.Remarks, A.Shift_Code From Department_Group_MAster  A Left Join VAAHINI_ERP_GAINUP.dbo.DepTtype B On A.Department_Code = B.DEPTCODE and A.Division_Code = B.compcode LEft Join VAAHINI_ERP_GAINUP.dbo.Companymas_Pay C On C.CompCode = A.Division_Code LEft Join VAAHINI_ERP_GAINUP.dbo.shiftmst D On A.Division_Code = D.CompCode and D.Shiftcode2 Not Like '%Z' and Mode = 1 and A.Shift_Code = D.shiftcode  Where  A.Division_Code  = (Case When " + MyParent.CompCode + " = 2 Then 3 Else 2 End) Order By A.Effect_From Desc ", string.Empty, 80, 150, 100, 80, 120);
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

        private void DtpEDate_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(DtpEDate.Value) > MyBase.GetServerDateTime())
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DtpEDate.Value = MyBase.GetServerDateTime();
                    DtpEDate.Focus();
                    return;
                }
                else if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(DtpEDate.Value), MyBase.GetServerDateTime()) > 1)
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DtpEDate.Value = MyBase.GetServerDateTime();
                    DtpEDate.Focus();
                    return;
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
                    MyBase.Run("Delete From Department_Group_Details Where Master_ID = " + Code, "Delete From Department_Group_Master  Where RowID = " + Code, MyParent.EntryLog("DEPARTMENT GROUP", "DELETE", Code.ToString())); 
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
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Department Group-View", "Select A.ENo, B.DEPTNAME, A.Effect_From, D.Shiftcode2 Shift, A.Group_Name, C.CompName Division, A.Total_Groups, A.Division_Code, A.RowID, A.Department_Code, A.Remarks, A.Shift_Code From Department_Group_MAster  A Left Join VAAHINI_ERP_GAINUP.dbo.DepTtype B On A.Department_Code = B.DEPTCODE and A.Division_Code = B.compcode LEft Join VAAHINI_ERP_GAINUP.dbo.Companymas_Pay C On C.CompCode = A.Division_Code LEft Join VAAHINI_ERP_GAINUP.dbo.shiftmst D On A.Division_Code = D.CompCode and D.Shiftcode2 Not Like '%Z' And Mode = 1 and A.Shift_Code = D.shiftcode  Where  A.Division_Code  = (Case When " + MyParent.CompCode + " = 2 Then 3 Else 2 End)  Order By A.Effect_From Desc ", string.Empty, 80, 150, 100, 80, 120);
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
                    Str = "Select 0 as SNO, '' MACHINE , 1.0 FROM_TYPE, 1.0 TO_TYPE, MACHINE_CODE From Department_Group_MAster A Left Join Department_Group_Details B on A.RowID = B.Master_ID  Where 1=2";
                }
                else
                {
                    Str = "Select C.SlNo SNO, F.Value MACHINE, C.FROM_TYPE FROM_TYPE, C.TO_TYPE TO_TYPE, C.MACHINE_CODE From Department_Group_MAster  A Inner Join Department_Group_Details C On A.RowID = C.Master_ID Inner Join VAAHINI_ERP_GAINUP.dbo.DepTtype B On A.Department_Code = B.DEPTCODE and A.Division_Code = B.compcode Inner Join VAAHINI_ERP_GAINUP.dbo.Main_Object_Master D On D.Name = B.DeptName and D.Type = 'M' Inner Join VAAHINI_ERP_GAINUP.dbo.Main_Machine_Master E On D.RowID = E.Object_ID and C.Machine_Code = E.RowID Inner Join VAAHINI_ERP_GAINUP.dbo.Main_Machine_Details F On F.Param_ID = 1 and F.Master_ID = E.RowID  Where A.RowID = " + Code + " Order By C.SlNo ";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);                               
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);               
                MyBase.ReadOnly_Grid(ref Grid, "SNO");
                MyBase.Grid_Designing(ref Grid, ref Dt, "MACHINE_CODE", "FROM_TYPE", "TO_TYPE");
                MyBase.Grid_Width(ref Grid, 50, 150, 100, 100);               
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["MACHINE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["FROM_TYPE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["TO_TYPE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
                    Txt.TextChanged += new EventHandler(Txt_TextChanged);
                    Txt.Leave += new EventHandler(Txt_Leave);
                    //Txt.GotFocus += new EventHandler(Txt_GotFocus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //void Txt_GotFocus(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index && Grid.CurrentCell.Value.ToString() == String.Empty)
        //        //if (Grid["Machine", Grid.CurrentCell.RowIndex].Value == null || Grid["Machine", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
        //        {
        //            Dr = Tool.Selection_Tool_Except_New("MACHINE", this, 50, 50, ref Dt, SelectionTool_Class.ViewType.NormalView, "MACHINE", "Select B.Value MACHINE, C.Name Object, A.RowID  MACHINE_CODE From VAAHINI_ERP_GAINUP.DBO.Main_Machine_Master A Inner join VAAHINI_ERP_GAINUP.DBO.Main_Machine_Details B On A.RowID = B.Master_ID and B.Param_ID = 1 Inner Join VAAHINI_ERP_GAINUP.DBO.Main_Object_Master C On A.Object_ID = C.RowID and Type = 'M'  Where A.Company_Code = (Case When " + TxtDivision.Tag + " = 1 Then 1 When " + TxtDivision.Tag + " = 2 Then 3 When " + TxtDivision.Tag + " = 3 Then 2 When " + TxtDivision.Tag + " = 4 Then 5 Else " + TxtDivision.Tag + " End) and (C.Name = '" + TxtDepartment.Text + "' or C.Name = 'GENERAL') Order by A.RowID ", string.Empty, 150, 100);
        //            if (Dr != null)
        //            {
        //                Txt.Text = Dr["MACHINE"].ToString();
        //                Grid["MACHINE", Grid.CurrentCell.RowIndex].Value = Dr["MACHINE"].ToString();
        //                Grid["MACHINE_CODE", Grid.CurrentCell.RowIndex].Value = Dr["MACHINE_CODE"].ToString();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}


        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (TxtDivision.Text.Trim() == String.Empty || TxtDepartment.Text.Trim() == String.Empty || TxtShift.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Division, Shift, Department & Group Name..!", "Gainup");
                    TxtDepartment.Focus();
                    return;
                }
                else
                {
                    if (e.KeyCode == Keys.Down)
                    {
                        if (Grid.CurrentCell.ColumnIndex == Grid.Columns["MACHINE"].Index)
                        {
                            Dr = Tool.Selection_Tool_Except_New("MACHINE", this, 50, 50, ref Dt, SelectionTool_Class.ViewType.NormalView, "MACHINE", "Select B.Value MACHINE, C.Name Object, A.RowID  MACHINE_CODE From VAAHINI_ERP_GAINUP.DBO.Main_Machine_Master A Inner join VAAHINI_ERP_GAINUP.DBO.Main_Machine_Details B On A.RowID = B.Master_ID and B.Param_ID = 1 Inner Join VAAHINI_ERP_GAINUP.DBO.Main_Object_Master C On A.Object_ID = C.RowID and Type = 'M'  Where A.Company_Code = (Case When " + TxtDivision.Tag + " = 1 Then 1 When " + TxtDivision.Tag + " = 2 Then 3 When " + TxtDivision.Tag + " = 3 Then 2 When " + TxtDivision.Tag + " = 4 Then 5 Else " + TxtDivision.Tag + " End) and (C.Name = '" + TxtDepartment.Text + "' or C.Name = 'GENERAL') Order by A.RowID ", string.Empty, 150, 100);                                                       
                            if (Dr != null)
                            {                               
                                Grid["MACHINE", Grid.CurrentCell.RowIndex].Value = Dr["MACHINE"].ToString();
                                Grid["MACHINE_CODE", Grid.CurrentCell.RowIndex].Value = Dr["MACHINE_CODE"].ToString();                                
                                Txt.Text = Dr["MACHINE"].ToString();
                            }
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

        void Txt_TextChanged(object sender, EventArgs e)
        {
            
        }

        void Txt_Leave(object sender, EventArgs e)
        {
            try
            {

                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["FROM_TYPE"].Index)
                {
                    if (Grid["FROM_TYPE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["FROM_TYPE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["FROM_TYPE", Grid.CurrentCell.RowIndex].Value = 1.0;
                    }
                }

                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["TO_TYPE"].Index)
                {
                    if (Grid["TO_TYPE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["TO_TYPE", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {
                        Grid["TO_TYPE", Grid.CurrentCell.RowIndex].Value = 1.0;
                    }
                    if ((Convert.ToDouble(Grid["TO_TYPE", Grid.CurrentCell.RowIndex].Value)) < (Convert.ToDouble(Grid["FROM_TYPE", Grid.CurrentCell.RowIndex].Value)))
                    {
                        Grid["TO_TYPE", Grid.CurrentCell.RowIndex].Value = 0.0;                       
                        MessageBox.Show ("Invalid No ..! FROM TYPE IS NOT GREATER THAN TO TYPE","Gainup");
                        Grid.CurrentCell = Grid["TO_TYPE", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;                        
                    }
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
                TxtTotal.Text = MyBase.Count(ref Grid, "MACHINE","MACHINE");                
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["FROM_TYPE"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["TO_TYPE"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
                }
                else
                {
                    MyBase.Valid_Null(Txt, e);
                }
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
                    TxtRemarks.Focus();
                    return;
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
                    Total_Count();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmDepartmentGroupEntry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                //if (MyParent.CompCode == 1)
                //{
                //    PCompCode = 1;
                //}
                //else if (MyParent.CompCode == 2)
                //{
                //    PCompCode = 3;
                //}
                TxtDivision.Text = "GAINUP - SOCKS";
                TxtDivision.Tag = 2;
                DtpEDate.Focus();  
                TxtShift.Text = "";
                TxtGroupName.Text = "";
                TxtDepartment.Text = "";                 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmDepartmentGroupEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {
                if (this.ActiveControl.Name == "TxtGroupName")
                {
                    if ((Convert.ToInt16(e.KeyChar) >= 32 && Convert.ToInt16(e.KeyChar) <= 47) || (Convert.ToInt16(e.KeyChar) >= 58 && Convert.ToInt16(e.KeyChar) <= 64) || (Convert.ToInt16(e.KeyChar) >= 91 && Convert.ToInt16(e.KeyChar) <= 96) || (Convert.ToInt16(e.KeyChar) >= 123 && Convert.ToInt16(e.KeyChar) <= 127))
                    {
                        e.Handled = true;
                    }
                }
                if (this.ActiveControl.Name != String.Empty && this.ActiveControl.Name != "TxtGroupName" && this.ActiveControl.Name != "TxtRemarks")
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
                else
                {                    
                    MyBase.Return_Ucase(e);
                }
            }
        }

        private void FrmDepartmentGroupEntry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtDepartment")                    
                    {
                        Grid_Data();
                        Grid.CurrentCell = Grid["MACHINE", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                    else if (this.ActiveControl.Name == "TxtTotal")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (MyParent._New == true)
                    {
                        if (this.ActiveControl.Name == "TxtDivision")
                        {
                            Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Division", "Select CompName Division, COmpCode Code From VAAHINI_ERP_GAINUP.DBO.Companymas_Pay Where Compcode = 2 ", String.Empty, 250);
                            if (Dr != null)
                            {
                                TxtDivision.Text = Dr["Division"].ToString();
                                TxtDivision.Tag = Dr["Code"].ToString();
                                TxtShift.Text = "";
                                TxtGroupName.Text = "";
                                TxtDepartment.Text = "";
                                Grid_Data();
                            }
                        }
                        if (this.ActiveControl.Name == "TxtShift")
                        {
                            if (TxtDivision.Text.Trim() == String.Empty)
                            {
                                MessageBox.Show("Invalid Division...!", "Gainup");
                                TxtDivision.Focus();
                                return;
                            }
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Shift", "Select Shiftcode2 Shift, ShiftDesc, ShiftCode Code From VAAHINI_ERP_GAINUP.DBO.shiftmst where Compcode = " + TxtDivision.Tag.ToString() + " and Shiftcode2 Not Like '%Z' And Mode = 1", String.Empty, 50, 300);
                            if (Dr != null)
                            {
                                TxtShift.Tag = Dr["Code"].ToString();
                                TxtShift.Text = Dr["Shift"].ToString();                             
                                TxtGroupName.Text = "";
                                TxtDepartment.Text = "";
                                Grid_Data();
                            }

                        }
                        if (this.ActiveControl.Name == "TxtDepartment")
                        {
                            if (TxtDivision.Text.Trim() == String.Empty)
                            {
                                MessageBox.Show("Invalid Division..!", "Gainup");
                                return;
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Department", "Select DeptName Department, Deptcode Code From VAAHINI_ERP_GAINUP.DBO.DepTtype where Compcode = " + TxtDivision.Tag.ToString() + " ", String.Empty, 200);
                                if (Dr != null)
                                {
                                    TxtDepartment.Text = Dr["DEPARTMENT"].ToString();
                                    TxtDepartment.Tag = Dr["Code"].ToString();                                        
                                    Grid_Data();
                                }
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
                if (Grid.Rows.Count > 1 ) 
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

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Grid_Leave(object sender, EventArgs e)
        {
            //int i = Grid.Rows.Count;
            if (Dt.Rows.Count > 1)
            {
                int i = Dt.Rows.Count;
                TxtGroupName.Text = Grid["Machine", 0].Value.ToString() + "-" + Grid["Machine", i - 1].Value.ToString();
            }
        }

        private void Grid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Machine"].Index)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        if (Grid["Machine", Grid.CurrentCell.RowIndex].Value == null || Grid["Machine", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Machine", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Dr = Tool.Selection_Tool_Except_New("MACHINE", this, 50, 50, ref Dt, SelectionTool_Class.ViewType.NormalView, "MACHINE", "Select B.Value MACHINE, C.Name Object, A.RowID  MACHINE_CODE From VAAHINI_ERP_GAINUP.DBO.Main_Machine_Master A Inner join VAAHINI_ERP_GAINUP.DBO.Main_Machine_Details B On A.RowID = B.Master_ID and B.Param_ID = 1 Inner Join VAAHINI_ERP_GAINUP.DBO.Main_Object_Master C On A.Object_ID = C.RowID and Type = 'M'  Where A.Company_Code = (Case When " + TxtDivision.Tag + " = 1 Then 1 When " + TxtDivision.Tag + " = 2 Then 3 When " + TxtDivision.Tag + " = 3 Then 2 When " + TxtDivision.Tag + " = 4 Then 5 Else " + TxtDivision.Tag + " End) and (C.Name = '" + TxtDepartment.Text + "' or C.Name = 'GENERAL') Order by A.RowID ", string.Empty, 150, 100);
                            if (Dr != null)
                            {
                                Grid["MACHINE", Grid.CurrentCell.RowIndex].Value = Dr["MACHINE"].ToString();
                                Grid["MACHINE_CODE", Grid.CurrentCell.RowIndex].Value = Dr["MACHINE_CODE"].ToString();
                                Txt.Text = Dr["MACHINE"].ToString();
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

        private void TxtShift_Enter(object sender, EventArgs e)
        {
            try
            {
                if (TxtDivision.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Division...!", "Gainup");
                    TxtDivision.Focus();
                    return;
                }
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Shift", "Select Shiftcode2 Shift, ShiftDesc, ShiftCode Code From VAAHINI_ERP_GAINUP.DBO.shiftmst where Compcode = " + TxtDivision.Tag.ToString() + " and Shiftcode2 Not Like '%Z' And Mode = 1", String.Empty, 50, 300);
                if (Dr != null)
                {
                    TxtShift.Tag = Dr["Code"].ToString();
                    TxtShift.Text = Dr["Shift"].ToString();
                    TxtGroupName.Text = "";
                    TxtDepartment.Text = "";
                    Grid_Data();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtDepartment_Enter(object sender, EventArgs e)
        {
            if (TxtDivision.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Invalid Division..!", "Gainup");
                return;
            }
            else
            {
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Department", "Select DeptName Department, Deptcode Code From VAAHINI_ERP_GAINUP.DBO.DepTtype where Compcode = " + TxtDivision.Tag.ToString() + " ", String.Empty, 200);
                if (Dr != null)
                {
                    TxtDepartment.Text = Dr["DEPARTMENT"].ToString();
                    TxtDepartment.Tag = Dr["Code"].ToString();
                    Grid_Data();
                }
            }
        }

      
       
    }
}
