using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using SelectionTool_NmSp;
using System.Text;
using Accounts_ControlModules;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Accounts
{
    public partial class Frm_Staff_KPI_Entry : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataTable Dt2 = new DataTable();
        DataTable Dt3 = new DataTable();
        DataRow Dr;
        TextBox Txt = null;
        String Str;
        DataTable TmpDt = new DataTable();
        Int64 Code = 0;
        String[] Queries_New, Queries;
        Int32 Array_Index = 0;

        public Frm_Staff_KPI_Entry()
        {
            InitializeComponent();
        }
        void Total_Count()
        {
            try
            {
                try
                {
                    TxtTotCount.Text = String.Format("{0}", Convert.ToInt16(MyBase.Count(ref Grid, "KPI_POINT")) - 1).ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
        public void Entry_Delete()
        {
            try
            {
                DtpFDate.Enabled = false;
                TxtMon.Enabled = false;
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "EFFECT_FROM", "Select Distinct cast(datename(Month,Effect_From)as varchar(3))+'-'+ cast(datepart(year,Effect_From)as varchar(25)) Month_Year, Case when Approval = 'T' then 'APPROVED' When Approval = 'R' Then 'REJECTED' Else 'NOT APPROVED' end STATUS , EFFECT_FROM  from Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY where Emplno = " + MyParent.Emplno + " and Isnull(Approval,'A') != 'T' ", String.Empty, 80,150);

                if (Dr != null)
                {

                    DtpFDate.Value = Convert.ToDateTime(Dr["EFFECT_FROM"]);
                    TxtMon.Text = Dr["Month_Year"].ToString();

                    if (MyParent.Emplno > 0)
                    {

                        Str = "Select A.Tno, A.Name, A.Emplno, B.DeptName, C.DesignationName, A.Emplno, A.Deptcode, A.Designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.tno not like '%Z' and A.EmplNO = " + MyParent.Emplno + " ";
                        MyBase.Load_Data(Str, ref Dt2);
                        if (Dt2.Rows.Count > 0)
                        {
                            Txt_Tno.Text = Dt2.Rows[0]["Tno"].ToString();
                            Txt_Tno.Tag = MyParent.Emplno.ToString();
                            Txt_Name.Text = Dt2.Rows[0]["Name"].ToString();
                            Txt_Name.BackColor = System.Drawing.Color.Yellow;
                            Txt_Name.Tag = Dt2.Rows[0]["Emplno"].ToString();
                            Txt_Dept.Text = Dt2.Rows[0]["DeptName"].ToString();
                            Txt_Dept.Tag = Dt2.Rows[0]["Deptcode"].ToString();
                            Txt_Designation.Text = Dt2.Rows[0]["DesignationName"].ToString();
                            Txt_Designation.Tag = Dt2.Rows[0]["designationcode"].ToString();
                        }

                        Str = "Select emplno , name, deptname, designationname from VAAHINI_ERP_GAINUP.dbo.incharge_master_select(" + Txt_Name.Tag + ")";
                        MyBase.Load_Data(Str, ref Dt3);
                        if (Dt3.Rows.Count > 0)
                        {
                            Txt_AgName.Text = Dt3.Rows[0]["Name"].ToString();
                            Txt_AgName.BackColor = System.Drawing.Color.Aqua;

                            Txt_AgDesignation.Text = Dt3.Rows[0]["DesignationName"].ToString();
                            Txt_AgDesignation.BackColor = System.Drawing.Color.Aqua;
                            Txt_AgName.Tag = Dt3.Rows[0]["EMPLNO"].ToString();
                            Grid_Data();
                            Total_Count();
                            label9.Visible = true;
                            TxtRemarks.Visible = true;
                            label7.Text = "KPI APPROVAl STATUS ";
                        }
                    }
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
                Str = "Delete from Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY where Emplno = " + MyParent.Emplno + " and Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "'";
                MyBase.Run_Identity(false, Str);
                MessageBox.Show("Deleted ...!", "Gainup");
                MyBase.Clear(this);
                MyParent.Load_DeleteEntry();
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
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);

                if (MyParent.Emplno > 0)
                {

                    Str = "Select A.Tno, A.Name, A.Emplno, B.DeptName, C.DesignationName, A.Emplno, A.Deptcode, A.Designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.tno not like '%Z' and A.EmplNO = " + MyParent.Emplno + " ";
                    MyBase.Load_Data(Str, ref Dt2);
                    if (Dt2.Rows.Count > 0)
                    {
                        Txt_Tno.Text = Dt2.Rows[0]["Tno"].ToString();
                        Txt_Tno.Tag = MyParent.Emplno.ToString();
                        Txt_Name.Text = Dt2.Rows[0]["Name"].ToString();
                        Txt_Name.BackColor = System.Drawing.Color.Yellow;
                        Txt_Name.Tag = Dt2.Rows[0]["Emplno"].ToString();
                        Txt_Dept.Text = Dt2.Rows[0]["DeptName"].ToString();
                        Txt_Dept.Tag = Dt2.Rows[0]["Deptcode"].ToString();
                        Txt_Designation.Text = Dt2.Rows[0]["DesignationName"].ToString();
                        Txt_Designation.Tag = Dt2.Rows[0]["designationcode"].ToString();
                    }

                    Str = "Select emplno , name, deptname, designationname from VAAHINI_ERP_GAINUP.dbo.incharge_master_select(" + Txt_Tno.Tag + ")";
                    MyBase.Load_Data(Str, ref Dt3);
                    if (Dt3.Rows.Count > 0)
                    {
                        Txt_AgName.Text = Dt3.Rows[0]["Name"].ToString();
                        Txt_AgName.BackColor = System.Drawing.Color.Aqua;

                        Txt_AgDesignation.Text = Dt3.Rows[0]["DesignationName"].ToString();
                        Txt_AgDesignation.BackColor = System.Drawing.Color.Aqua;
                        Txt_AgName.Tag = Dt3.Rows[0]["EMPLNO"].ToString();
                        Grid_Data();
                        label9.Visible = false;
                        TxtRemarks.Visible = false;
                        label7.Text = "PREVIOUS KPI APPROVAl STATUS ";
                        TxtMon.Focus();
                    }
                }
                DtpFDate.Enabled = true;
                TxtMon.Enabled = true;
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
                DtpFDate.Enabled = false;
                TxtMon.Enabled = false;

                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "EFFECT_FROM", "Select Distinct cast(datename(Month,Effect_From)as varchar(3))+'-'+ cast(datepart(year,Effect_From)as varchar(25)) Month_Year, Case when Approval = 'T' then 'APPROVED' When Approval = 'R' Then 'REJECTED' Else 'NOT APPROVED' end STATUS, EFFECT_FROM  from Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY where Emplno = " + MyParent.Emplno + " ", String.Empty, 80 , 150);

                if (Dr != null)
                {

                    DtpFDate.Value = Convert.ToDateTime(Dr["EFFECT_FROM"]);
                    TxtMon.Text = Dr["Month_Year"].ToString();

                    if (MyParent.Emplno > 0)
                    {
                        Str = "Select A.Tno, A.Name, A.Emplno, B.DeptName, C.DesignationName, A.Emplno, A.Deptcode, A.Designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.tno not like '%Z' and A.EmplNO = " + MyParent.Emplno + " ";
                        MyBase.Load_Data(Str, ref Dt2);
                        if (Dt2.Rows.Count > 0)
                        {
                            Txt_Tno.Text = Dt2.Rows[0]["Tno"].ToString();
                            Txt_Tno.Tag = MyParent.Emplno.ToString();
                            Txt_Name.Text = Dt2.Rows[0]["Name"].ToString();
                            Txt_Name.BackColor = System.Drawing.Color.Yellow;
                            Txt_Name.Tag = Dt2.Rows[0]["Emplno"].ToString();
                            Txt_Dept.Text = Dt2.Rows[0]["DeptName"].ToString();
                            Txt_Dept.Tag = Dt2.Rows[0]["Deptcode"].ToString();
                            Txt_Designation.Text = Dt2.Rows[0]["DesignationName"].ToString();
                            Txt_Designation.Tag = Dt2.Rows[0]["designationcode"].ToString();
                        }

                        Str = "Select emplno , name, deptname, designationname from VAAHINI_ERP_GAINUP.dbo.incharge_master_select(" + Txt_Name.Tag + ")";
                        MyBase.Load_Data(Str, ref Dt3);
                        if (Dt3.Rows.Count > 0)
                        {
                            Txt_AgName.Text = Dt3.Rows[0]["Name"].ToString();
                            Txt_AgName.BackColor = System.Drawing.Color.Aqua;

                            Txt_AgDesignation.Text = Dt3.Rows[0]["DesignationName"].ToString();
                            Txt_AgDesignation.BackColor = System.Drawing.Color.Aqua;
                            Txt_AgName.Tag = Dt3.Rows[0]["EMPLNO"].ToString();
                            Grid_Data();
                            Total_Count();
                            label9.Visible = true;
                            TxtRemarks.Visible = true;
                            label7.Text = "KPI APPROVAl STATUS ";
                        }
                    }
                    
                }
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
                String[] Queries;
                Int64 Array_Index = 0;
                Queries = new String[(Dt.Rows.Count * 3) + 5];
                Total_Count();
                Grid.Refresh();

                if (Txt_Tno.Text.ToString().Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Name...!", "GainUp.....!");
                    MyParent.Save_Error = true;
                    Txt_Tno.Focus();
                    return;
                }
                else if (TxtMon.Text.ToString().Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Month Year...!", "GainUp.....!");
                    MyParent.Save_Error = true;
                    TxtMon.Focus();
                    return;
                }
                else if (Txt_AgName.Text.ToString().Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Incharge...!", "GainUp.....!");
                    MyParent.Save_Error = true;
                    Txt_AgName.Focus();
                    return;
                }
                else if (Dt.Rows.Count < 3)
                {
                    MessageBox.Show("Please Fill KPI At Least - 3 ", "GainUp.....!");
                    Grid.CurrentCell = Grid["KPI_POINT", Grid.CurrentCell.RowIndex];
                    MyParent.Save_Error = true;
                    Grid.BeginEdit(true);
                    Grid.Focus();
                    return;
                }
                Grid.Refresh();

                DataTable Dt_Previous = new DataTable();
                Str = "Select * from VAAHINI_ERP_GAINUP.dbo.Month_Year(" + MyParent.Emplno + ") where Edate < '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' order by edate";
                MyBase.Load_Data(Str, ref Dt_Previous);
                if (Dt_Previous.Rows.Count > 0)
                {
                    MessageBox.Show("Previous Month KPI ENTRY Not Entered - Month OF " + Dt_Previous.Rows[0]["Month_Year"] + "", "GainUp.....!");
                    MyParent.Save_Error = true;
                    TxtMon.Focus();
                    return;
                }

                DataTable Check = new DataTable();
                Str = "Select  * from Vaahini_Erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY where Datename(MOnth,Effect_From)+'-'+Cast(Datepart(Year,Effect_From) as varchar(5)) = Datename(MOnth,'" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "')+'-'+Cast(Datepart(Year,'" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "') as varchar(5)) and Emplno = " + MyParent.Emplno + "";
                MyBase.Load_Data(Str, ref Check);
               
                if (MyParent._New == true && Check.Rows.Count > 0)
                {
                    MessageBox.Show("Already Entered OR Approved this Month...!", "GainUp.....!");
                    MyParent.Save_Error = true;
                    Txt_AgName.Focus();
                    return;
                }

                if (MyParent._New != true)
                {
                    Queries[Array_Index++] = " Delete from Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY where Emplno = " + MyParent.Emplno + " and Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "'";
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToString(Grid["KPI_POINT", i].Value) == String.Empty)
                    {
                        MessageBox.Show("Invalid KPI.......!", "GainUp.....!");

                        Grid.CurrentCell = Grid["KPI_POINT", Grid.CurrentCell.RowIndex];
                        MyParent.Save_Error = true;
                        Grid.BeginEdit(true);
                        Grid.Focus();
                        return;
                    }
                    else if (Convert.ToString(Grid["UOM", i].Value) == String.Empty)
                    {
                        MessageBox.Show("Invalid UOM.......!", "GainUp.....!");

                        Grid.CurrentCell = Grid["UOM", Grid.CurrentCell.RowIndex];
                        MyParent.Save_Error = true;
                        Grid.BeginEdit(true);
                        Grid.Focus();
                        return;
                    }
                    if (Convert.ToString(Grid["Target", i].Value) == String.Empty)
                    {
                        Grid["Target", i].Value = "-";
                    }
                    if (Convert.ToString(Grid["ACTUAL", i].Value) == String.Empty)
                    {
                        Grid["ACTUAL", i].Value = "-";
                    }
                    if (MyParent._New == true)
                    {
                        Queries[Array_Index++] = "Insert into Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY(Effect_From, Emplno, Incharge_Emplno, KPI_POINT, Target, ACTUAL,UOM_ID) Values('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "'," + Txt_Name.Tag.ToString() + "," + Txt_AgName.Tag.ToString() + ",'" + Grid["KPI_POINT", i].Value.ToString() + "','" + Grid["Target", i].Value.ToString() + "','" + Grid["ACTUAL", i].Value.ToString() + "','" + Grid["UOM_ID", i].Value.ToString() + "')";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY(Effect_From, Emplno, Incharge_Emplno, KPI_POINT, Target, ACTUAL,UOM_ID) Values('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "'," + Txt_Name.Tag.ToString() + "," + Txt_AgName.Tag.ToString() + ",'" + Grid["KPI_POINT", i].Value.ToString() + "','" + Grid["Target", i].Value.ToString() + "','" + Grid["ACTUAL", i].Value.ToString() + "','" + Grid["UOM_ID", i].Value.ToString() + "')";
                        //Queries[Array_Index++] = " Update Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY  Set  Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' ,Emplno = " + Txt_Name.Tag.ToString() + " , Incharge_Emplno = " + Txt_AgName.Tag.ToString() + " , KPI_POINT = '" + Grid["KPI_POINT", i].Value.ToString() + "' , Target = " + Grid["Target", i].Value.ToString() + ", ACTUAL = " + Grid["ACTUAL", i].Value.ToString() + " where Rowid = " + Grid["Rowid", i].Value.ToString() + " ";
                    }
                }
                MyBase.Run_Identity(false, Queries);
                MessageBox.Show("Saved ....!", "Gainup");
                Dt = new DataTable();
                Grid.DataSource = null;
                MyParent.Load_NewEntry();
                DtpFDate.Enabled = true;
                DtpFDate.Focus();
                return;

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
                    Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);
                    Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Target"].Index)
                {
                   // MyBase.Valid_Decimal(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ACTUAL"].Index)
                {
                    //MyBase.Valid_Decimal(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["KPI_POINT"].Index)
                {
                    e.Handled = false;
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["UOM"].Index)
                {
                    e.Handled = true;
                }
                Total_Count();
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
        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Dt.Rows.Count != 0)
                {
                    MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentRow.Index);
                }
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
                DtpFDate.Enabled = false;
                TxtMon.Enabled = false;
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "EFFECT_FROM", "Select Distinct cast(datename(Month,Effect_From)as varchar(3))+'-'+ cast(datepart(year,Effect_From)as varchar(25)) Month_Year , Case when Approval = 'T' then 'APPROVED' When Approval = 'R' Then 'REJECTED' Else 'NOT APPROVED' end STATUS,  EFFECT_FROM  from Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY where Emplno = " + MyParent.Emplno + " and Isnull(Approval,'A') != 'T' ", String.Empty, 80,150);

                if (Dr != null)
                {

                    DtpFDate.Value = Convert.ToDateTime(Dr["EFFECT_FROM"]);
                    TxtMon.Text = Dr["Month_Year"].ToString();

                    if (MyParent.Emplno > 0)
                    {

                        Str = "Select A.Tno, A.Name, A.Emplno, B.DeptName, C.DesignationName, A.Emplno, A.Deptcode, A.Designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.tno not like '%Z' and A.EmplNO = " + MyParent.Emplno + " ";
                        MyBase.Load_Data(Str, ref Dt2);
                        if (Dt2.Rows.Count > 0)
                        {
                            Txt_Tno.Text = Dt2.Rows[0]["Tno"].ToString();
                            Txt_Tno.Tag = MyParent.Emplno.ToString();
                            Txt_Name.Text = Dt2.Rows[0]["Name"].ToString();
                            Txt_Name.BackColor = System.Drawing.Color.Yellow;
                            Txt_Name.Tag = Dt2.Rows[0]["Emplno"].ToString();
                            Txt_Dept.Text = Dt2.Rows[0]["DeptName"].ToString();
                            Txt_Dept.Tag = Dt2.Rows[0]["Deptcode"].ToString();
                            Txt_Designation.Text = Dt2.Rows[0]["DesignationName"].ToString();
                            Txt_Designation.Tag = Dt2.Rows[0]["designationcode"].ToString();
                        }

                        Str = "Select emplno , name, deptname, designationname from VAAHINI_ERP_GAINUP.dbo.incharge_master_select(" + Txt_Name.Tag + ")";
                        MyBase.Load_Data(Str, ref Dt3);
                        if (Dt3.Rows.Count > 0)
                        {
                            Txt_AgName.Text = Dt3.Rows[0]["Name"].ToString();
                            Txt_AgName.BackColor = System.Drawing.Color.Aqua;

                            Txt_AgDesignation.Text = Dt3.Rows[0]["DesignationName"].ToString();
                            Txt_AgDesignation.BackColor = System.Drawing.Color.Aqua;
                            Txt_AgName.Tag = Dt3.Rows[0]["EMPLNO"].ToString();
                            Grid_Data();
                            Total_Count();
                            label9.Visible = true;
                            TxtRemarks.Visible = true;
                            label7.Text = "KPI APPROVAl STATUS ";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Frm_Staff_KPI_Entry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);

                if (MyParent.Emplno > 0)
                {

                    Str = "Select A.Tno, A.Name, A.Emplno, B.DeptName, C.DesignationName, A.Emplno, A.Deptcode, A.Designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.tno not like '%Z' and A.EmplNO = " + MyParent.Emplno + " ";
                    MyBase.Load_Data(Str, ref Dt2);
                    if (Dt2.Rows.Count > 0)
                    {
                        Txt_Tno.Text = Dt2.Rows[0]["Tno"].ToString();
                        Txt_Tno.Tag = MyParent.Emplno.ToString();
                        Txt_Name.Text = Dt2.Rows[0]["Name"].ToString();
                        Txt_Name.BackColor = System.Drawing.Color.Yellow;
                        Txt_Name.Tag = Dt2.Rows[0]["Emplno"].ToString();
                        Txt_Dept.Text = Dt2.Rows[0]["DeptName"].ToString();
                        Txt_Dept.Tag = Dt2.Rows[0]["Deptcode"].ToString();
                        Txt_Designation.Text = Dt2.Rows[0]["DesignationName"].ToString();
                        Txt_Designation.Tag = Dt2.Rows[0]["designationcode"].ToString();
                    }

                    Str = "Select emplno , name, deptname, designationname from VAAHINI_ERP_GAINUP.dbo.incharge_master_select(" + Txt_Tno.Tag + ")";
                    MyBase.Load_Data(Str, ref Dt3);
                    if (Dt3.Rows.Count > 0)
                    {
                        Txt_AgName.Text = Dt3.Rows[0]["Name"].ToString();
                        Txt_AgName.BackColor = System.Drawing.Color.Aqua;

                        Txt_AgDesignation.Text = Dt3.Rows[0]["DesignationName"].ToString();
                        Txt_AgDesignation.BackColor = System.Drawing.Color.Aqua;
                        Txt_AgName.Tag = Dt3.Rows[0]["EMPLNO"].ToString();
                        Grid_Data();
                        Total_Count();
                    }
                }
                DtpFDate.Enabled = true;
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
                MyBase.Row_Number(ref Grid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Grid_Data()
        {
            DataTable DtCh = new DataTable();
           // DataTable Dt = new DataTable();
            String Str2 = String.Empty;
            try
            {
                if (MyParent._New == true)
                {
                    MyBase.Load_Data("Select * from Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY where Emplno = " + MyParent.Emplno + "", ref DtCh);
                  
                    if (DtCh.Rows.Count > 0)
                    {

                        Str = "Select ROW_NUMBER() over(order by A.Rowid)  SLNO, KPI_POINT , [Target] ,'' ACTUAL , B.Name UOM, A.Rowid , A.UOM_ID , Case when Approval = 'T' then 'APPROVED' When Approval = 'R' Then 'REJECTED' Else 'NOT APPROVED' end STATUS1 , Remarks ,cast(datename(Month,Effect_From)as varchar(3))+'-'+ cast(datepart(year,Effect_From)as varchar(25)) Month_Year   from Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY A  Left Join Vaahini_erp_Gainup.Dbo.KPI_UOM_Master B On A.UOM_ID = B.Rowid where A.Emplno = " + MyParent.Emplno + " and A.Effect_From =(Select Max(Effect_From) Effect_From from Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY where Emplno = " + MyParent.Emplno + ")   ";

                    }
                    else
                    {
                        Str = "Select ROW_NUMBER() over(order by Rowid)  SLNO, KPI_POINT , [Target] ,'' ACTUAL ,'' UOM, Rowid, A.UOM_ID,'' STATUS1 , '' Remarks ,'' Month_Year from Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY A where Emplno = " + MyParent.Emplno + " and 1 = 2";
                    }
                }
                else
                {
                    Str = "Select ROW_NUMBER() over(order by A.Rowid)  SLNO, A.KPI_POINT , A.[Target] , A.ACTUAL ,B.Name  UOM, A.Rowid , A.UOM_ID, Case when A.Approval = 'T' then 'APPROVED' When A.Approval = 'R' Then 'REJECTED' Else 'NOT APPROVED' end STATUS1 , A.Remarks ,cast(datename(Month,Effect_From)as varchar(3))+'-'+ cast(datepart(year,Effect_From)as varchar(25)) Month_Year from Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY A  Left Join Vaahini_erp_Gainup.Dbo.KPI_UOM_Master B On A.UOM_ID = B.Rowid where A.Emplno = " + MyParent.Emplno + " and A.Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' ";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    TxtStatus.Text = Dt.Rows[0]["STATUS1"].ToString() + " For the Month OF - " + Dt.Rows[0]["Month_Year"].ToString();
                    TxtRemarks.Text = Dt.Rows[0]["Remarks"].ToString();
                }

                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Rowid","UOM_ID", "STATUS1", "Month_Year", "Remarks");
                MyBase.ReadOnly_Grid_Without(ref Grid, "KPI_POINT", "Target", "ACTUAL", "UOM");
                MyBase.Grid_Width(ref Grid,70 , 450, 120, 120 );
                Grid.RowHeadersWidth = 10;
                Grid.CurrentCell = Grid["KPI_POINT", 0];
                Grid.Focus();
                Grid.BeginEdit(true);
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["UOM"].Index)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "UOM", "Select Name UOM, Rowid From Vaahini_erp_Gainup.Dbo.KPI_UOM_Master", String.Empty, 100);
                        if (Dr != null)
                        {
                            Grid["UOM", Grid.CurrentCell.RowIndex].Value = Dr["UOM"].ToString();
                            Grid["UOM_ID", Grid.CurrentCell.RowIndex].Value = Dr["Rowid"].ToString();
                            Txt.Text = Dr["UOM"].ToString();
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                        else
                        {
                                    
                        }
                    }
                }
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Frm_Staff_KPI_Entry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "TxtMon")
                    {
                        Grid.CurrentCell = Grid["KPI_POINT", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                    }
                    //else if (this.ActiveControl.Name == "DtpEDate")
                    //{
                    //    Grid_Data();
                    //}
                    else if (this.ActiveControl.Name == "TxtTotCount")
                    {
                        MyParent.Load_SaveEntry();
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtMon")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Employee", "Select * from VAAHINI_ERP_GAINUP.dbo.Month_Year(" + MyParent.Emplno + ")", string.Empty, 300);
                        if (Dr != null)
                        {
                            TxtMon.Text = Dr["Month_Year"].ToString();
                            DtpFDate.Value = Convert.ToDateTime(Dr["Edate"].ToString());
                        }
                    }
                }              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Frm_Staff_KPI_Entry_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    MyParent.Load_SaveEntry();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}