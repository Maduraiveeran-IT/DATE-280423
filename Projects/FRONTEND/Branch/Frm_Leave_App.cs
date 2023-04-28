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
    public partial class Frm_Leave_App : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataRow Dr;
        TextBox Txt = null;
        String Str;
        DataTable TmpDt = new DataTable();
        Int64 Code = 0;
        String[] Queries_New, Queries;
        Int32 Array_Index = 0;
        
        public Frm_Leave_App()
        {
            InitializeComponent();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        void Total_Count()
        {
            try
            {
                if (radioButton2.Checked == false)
                {
                    if (radioButton2.Checked == true)
                    {
                        DataTable Dt40 = new DataTable();

                        Str = "SELECT Cast(CAST(DATEDIFF(mi, cast('" + String.Format("{0:dd-MMM-yyyy HH:mm}", DtpShiftFrom.Value) + "' as datetime), cast('" + String.Format("{0:dd-MMM-yyyy HH:mm}", Dtp3.Value) + "' as datetime)) AS FLOAT)/60 as DECIMAL(18,2)) AS Time";

                        MyBase.Load_Data(Str, ref Dt40);
                        Txtdays.Text = Dt40.Rows[0]["Time"].ToString();

                    }
                    else if (Co_OFF_BTN.Checked == false)
                    {
                        DateTime start = DtpEDate.Value.Date;
                        DateTime end = DptTDate.Value.Date;
                        Txtdays.Text = Convert.ToInt32(end.Subtract(start).Days + 1).ToString();
                    }
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

         public void Entry_Delete_Confirm()
        {
            try
            {
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
                label9.Visible = false;
                DptTDate.Visible = true;
                DtpEDate.Visible = true;
                DtpShiftFrom.Visible = false;
                Dtp3.Visible = false;
                label9.Visible = false;
                label2.Visible = true;
                DptTDate.Visible = true;
                DtpEDate.Visible = true;
                DtpShiftFrom.Visible = false;
                Dtp3.Visible = false;
                label4.Visible = true;
                label1.Visible = true;
                
                Dpt1.Visible = false;
                label14.Visible = false;
                label12.Visible = false;
                label13.Visible = false;
                checkBox2.Visible = false;
                label7.Visible = false;
                radioButton1.Checked = true;
                Dtp3.CustomFormat = "hh:mm:ss tt";
                DtpShiftFrom.CustomFormat = "hh:mm:ss tt";
                DtpShiftFrom.Value = MyBase.GetServerDateTime();
                Dtp3.Value = MyBase.GetServerDateTime();

                DataTable Dt2 = new DataTable();
                DataTable Dt3 = new DataTable();
                DataTable Dt4 = new DataTable();
                if (MyParent.Emplno > 0)
                {

                    Str = "Select A.Tno, A.Name, A.Emplno, B.DeptName, C.DesignationName, A.Emplno, A.Deptcode, A.designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.tno not like '%Z' and A.EmplNO = " + MyParent.Emplno + " ";
                    MyBase.Load_Data(Str, ref Dt2);

                    Str = "Select Top 1 flag from VAAHINI_ERP_GAINUP.dbo.Staff_leave_Apply where emplno = " + MyParent.Emplno + " order by edate desc ";
                    MyBase.Load_Data(Str, ref Dt4);


                    if (Dt2.Rows.Count > 0)
                    {
                        Txt_Tno.Text = Dt2.Rows[0]["Tno"].ToString();
                        Txt_EMPLNo.Text = Dt2.Rows[0]["Emplno"].ToString();
                        Txt_Name.Text = Dt2.Rows[0]["Name"].ToString();
                        Txt_Name.BackColor = System.Drawing.Color.Yellow;
                        Txt_Name.Tag = Dt2.Rows[0]["Emplno"].ToString();
                        Txt_Dept.Text = Dt2.Rows[0]["DeptName"].ToString();
                        Txt_Dept.Tag = Dt2.Rows[0]["Deptcode"].ToString();
                        Txt_Designation.Text = Dt2.Rows[0]["DesignationName"].ToString();
                        Txt_Designation.Tag = Dt2.Rows[0]["designationcode"].ToString();
                        button1.Enabled = true;
                        DptTDate.MinDate = DateTime.Today.AddDays(0);
                        DtpEDate.MinDate = DateTime.Today.AddDays(0);

                        if (Dt4.Rows.Count > 0)
                        {
                            if (Dt4.Rows[0]["Flag"].ToString() == "R")
                            {
                                Txt_Status.Text = "Requested";
                                Txt_Status.ForeColor = Color.Pink;
                            }
                            else if (Dt4.Rows[0]["Flag"].ToString() == "F")
                            {
                                Txt_Status.Text = "Rejected";
                                Txt_Status.ForeColor = Color.Red;
                            }
                            else if (Dt4.Rows[0]["Flag"].ToString() == "A")
                            {
                                Txt_Status.Text = "Approved";

                                Txt_Status.ForeColor = Color.Green;
                            }
                        }
                        else
                        {
                            Txt_Status.Text = "NO ReCord";
                            Txt_Status.ForeColor = Color.Gray;
                        }
                    }

                    else
                    {
                        Txt_Tno.Focus();
                    }
                    Str = "Select emplno , name,deptname,designationname from VAAHINI_ERP_GAINUP.dbo.incharge_master_select(" + Txt_Name.Tag + ")";
                    MyBase.Load_Data(Str, ref Dt3);
                    if (Dt3.Rows.Count > 0)
                    {
                        Txt_AgName.Text = Dt3.Rows[0]["Name"].ToString();
                        Txt_AgName.BackColor = System.Drawing.Color.Aqua;

                        Txt_AgDesignation.Text = Dt3.Rows[0]["DesignationName"].ToString();
                        Txt_AgDesignation.BackColor = System.Drawing.Color.Aqua;
                        Remplno.Text = Dt3.Rows[0]["EMPLNO"].ToString();

                    }
                    DtpEDate.Focus();
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
                if (radioButton2.Checked == true)
                {
                    Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "VIEW..!", "SELECT Tno, Name, Department, Permission_date,cast(from_time as datetime) from_time,cast(to_time as datetime) to_time,total, REASON, HOD, Designation, APPROVAL_STATUS,REMARK, Entry_Date , ROWID   FROM VAAHINI_ERP_GAINUP.dbo.Permission_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + " ", String.Empty, 80, 150, 100, 100, 50, 50, 50, 180, 100, 200, 100, 100);

                }
                else if (radioButton1.Checked == true)
                {
                    Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "VIEW.!", " SELECT Tno, Name, Department, FROMDATE, TODATE, leave_count, REASON, HOD, Designation, APPROVAL_STATUS,Remark,ROWID,Entry_Date FROM VAAHINI_ERP_GAINUP.dbo.leave_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + "", String.Empty, 80, 200, 150, 100, 100, 50, 100, 200, 150, 100, 100);

                }
                else if (radioButton3.Checked == true)
                {
                    Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "VIEW..!", " Select Tno, Name, Department, OD_Date, Reason, HOD, Designation, Approval_Status,Remark,rowid,Entry_Date from VAAHINI_ERP_GAINUP.dbo.OD_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + " ", String.Empty, 80, 200, 150, 100, 200, 200, 150, 100, 100);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Already Entered ....!", "Gainup");
            }
        }
         public void Entry_Save()
        {
            try
            {
                if (radioButton1.Checked == true)
                {
                    if (this.DtpEDate.Value.Date < DateTime.Today.Date && this.DtpEDate.Value.Date != DateTime.Today.Date)
                    {
                        MessageBox.Show("Invalid Date", "Gainup");
                        DtpEDate.Value = DateTime.Now;
                        DtpEDate.Focus();
                        DtpEDate.Enabled = true;
                        return;
                    }
                }
                if (radioButton3.Checked == true )
                {
                }
                else if (radioButton2.Checked == true)
                {
                }
                else if (this.DptTDate.Value.Date < this.DtpEDate.Value.Date && Co_OFF_BTN.Checked == false)
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DptTDate.Value = DateTime.Today.AddDays(+1);
                    DptTDate.Focus();
                    DptTDate.Enabled = true;
                    MyParent.Save_Error = true;
                    return;
                }
                if (Txt_Name.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Name", "Gainup");
                    Txt_Tno.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (Txt_Reason.Text.ToString() == String.Empty && Co_OFF_BTN.Checked == false)
                {
                    MessageBox.Show("Invalid Reason", "Gainup");
                    Txt_Reason.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (Txt_Remark.Text.ToString() == String.Empty && Co_OFF_BTN.Checked == false)
                {
                    MessageBox.Show("Invalid remark", "Gainup");
                    Txt_Remark.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (Txt_AgName.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Responsible Name", "Gainup");
                    Txt_AgName.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                DialogResult m = MessageBox.Show("Sure to Save...!", "Leave Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (m == DialogResult.Yes)
                {
                    Total_Count();
                    Entrysave();

                    //radioButton3.Checked == true;
                    DtpEDate.Value = DateTime.Today.AddDays(0);
                    DptTDate.Value = DateTime.Today.AddDays(0);
                    DptTDate.Enabled = true;
                    DtpEDate.Enabled = true;
                    Txt_Reason.Text = "";
                    Txt_Remark.Text = "";
                    return;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Already Entered ....!", "Gainup");
                MyBase.Clear(this);
            }

        }
         public void Entry_Edit()
        {
            try
            {
                if (radioButton1.Checked == true)
                {
                    if (this.DtpEDate.Value.Date < DateTime.Today.Date && this.DtpEDate.Value.Date != DateTime.Today.Date)
                    {
                        MessageBox.Show("Invalid Date", "Gainup");
                        DtpEDate.Value = DateTime.Now;
                        DtpEDate.Focus();
                        DtpEDate.Enabled = true;
                        return;
                    }
                }
                if (radioButton2.Checked == true)
                {
                    if (Convert.ToDouble(Txtdays.Text) <= 0)
                    {
                        MessageBox.Show("Invalid Permission hours..!", "Gainup");
                        Dtp3.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                else if (radioButton3.Checked == true)
                {
                }
                else if (this.DptTDate.Value.Date < this.DtpEDate.Value.Date)
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DptTDate.Value = DateTime.Today.AddDays(0);
                    DptTDate.Focus();
                    DptTDate.Enabled = true;
                    return;
                }
                if (Txt_Name.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Name", "Gainup");
                    Txt_Tno.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (Txt_Reason.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Reason", "Gainup");
                    Txt_Reason.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (Txt_Remark.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid remark", "Gainup");
                    Txt_Remark.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (Txt_AgName.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Responsible Name", "Gainup");
                    Txt_AgName.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                DialogResult m = MessageBox.Show("Sure to Save...!", "Leave Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (m == DialogResult.Yes)
                {
                    Total_Count();
                    Entry_Update();
                    DptTDate.Enabled = true;
                    DtpEDate.Enabled = true;
                    DtpEDate.Value = DateTime.Today.AddDays(0);
                    DptTDate.Value = DateTime.Today.AddDays(0);
                    DptTDate.Enabled = true;
                    DtpEDate.Enabled = true;
                    Txt_Reason.Text = "";
                    Txt_Remark.Text = "";
                    return;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Already Entered ....!", "Gainup");
                MyBase.Clear(this);
            }

        }

        private void Frm_Leave_App_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                label9.Visible = false;
                DptTDate.Visible = true;
                DtpEDate.Visible = true;
                DtpShiftFrom.Visible = false;
                Dtp3.Visible = false;
                label9.Visible = false;
                label2.Visible = true;
                DptTDate.Visible = true;
                DtpEDate.Visible = true;
                DtpShiftFrom.Visible = false;
                Dtp3.Visible = false;
                label4.Visible = true;
                label1.Visible = true;
                DtpEDate.Value = MyBase.GetServerDateTime();
                DptTDate.Value = MyBase.GetServerDateTime();
                Dpt1.Visible = false;
                label14.Visible = false;
                label12.Visible = false;
                label13.Visible = false;
                checkBox2.Visible = false;
                label7.Visible = false;
                radioButton1.Checked = true;
                Dtp3.CustomFormat = "hh:mm:ss tt";
                DtpShiftFrom.CustomFormat = "hh:mm:ss tt";
                DtpShiftFrom.Value = MyBase.GetServerDateTime();
                Dtp3.Value = MyBase.GetServerDateTime();
                DtpEDate.MaxDate = DateTime.Today.AddDays(+15);
                DptTDate.MaxDate = DateTime.Today.AddDays(+15);

                DataTable Dt2 = new DataTable();
                DataTable Dt3 = new DataTable();
                DataTable Dt4 = new DataTable();
                if (MyParent.Emplno > 0)
                {

                    Str = "Select A.Tno, A.Name, A.Emplno, B.DeptName, C.DesignationName, A.Emplno, A.Deptcode, A.designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.tno not like '%Z' and A.EmplNO = " + MyParent.Emplno + " ";
                    MyBase.Load_Data(Str, ref Dt2);

                    Str = "Select Top 1 flag from VAAHINI_ERP_GAINUP.dbo.Staff_leave_Apply where emplno = " + MyParent.Emplno + " order by edate desc ";
                    MyBase.Load_Data(Str, ref Dt4);


                    if (Dt2.Rows.Count > 0)
                    {
                        Txt_Tno.Text = Dt2.Rows[0]["Tno"].ToString();
                        Txt_EMPLNo.Text = Dt2.Rows[0]["Emplno"].ToString();
                        Txt_Name.Text = Dt2.Rows[0]["Name"].ToString();
                        Txt_Name.BackColor = System.Drawing.Color.Yellow;
                        Txt_Name.Tag = Dt2.Rows[0]["Emplno"].ToString();
                        Txt_Dept.Text = Dt2.Rows[0]["DeptName"].ToString();
                        Txt_Dept.Tag = Dt2.Rows[0]["Deptcode"].ToString();
                        Txt_Designation.Text = Dt2.Rows[0]["DesignationName"].ToString();
                        Txt_Designation.Tag = Dt2.Rows[0]["designationcode"].ToString();
                        button1.Enabled = true;

                        if (Dt4.Rows.Count > 0)
                        {
                            if (Dt4.Rows[0]["Flag"].ToString() == "R")
                            {
                                Txt_Status.Text = "Requested";
                                Txt_Status.ForeColor = Color.Pink;
                            }
                            else if (Dt4.Rows[0]["Flag"].ToString() == "F")
                            {
                                Txt_Status.Text = "Rejected";
                                Txt_Status.ForeColor = Color.Red;
                            }
                            else if (Dt4.Rows[0]["Flag"].ToString() == "A")
                            {
                                Txt_Status.Text = "Approved";

                                Txt_Status.ForeColor = Color.Green;
                            }
                        }
                        else
                        {
                            Txt_Status.Text = "NO ReCord";
                            Txt_Status.ForeColor = Color.Gray;
                        }
                    }

                    else
                    {
                        Txt_Tno.Focus();
                    }
                    Str = "Select emplno , name, deptname, designationname from VAAHINI_ERP_GAINUP.dbo.incharge_master_select(" + Txt_Name.Tag + ")";
                    MyBase.Load_Data(Str, ref Dt3);
                    if (Dt3.Rows.Count > 0)
                    {
                        Txt_AgName.Text = Dt3.Rows[0]["Name"].ToString();
                        Txt_AgName.BackColor = System.Drawing.Color.Aqua;

                        Txt_AgDesignation.Text = Dt3.Rows[0]["DesignationName"].ToString();
                        Txt_AgDesignation.BackColor = System.Drawing.Color.Aqua;
                        Remplno.Text = Dt3.Rows[0]["EMPLNO"].ToString();

                    }
                    DtpEDate.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Frm_Leave_App_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                if (this.ActiveControl.Name == "Txt_Reason")
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
                if (this.ActiveControl.Name == "Txtdays")
                {
                   
                   e.Handled = false;
                    
                }
              }
            else
            {
                e.Handled = true;
            }
        }
             
        private void  Frm_Leave_App_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
               String Str = String.Empty;

                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "DtpEDate")
                    {
                        if (radioButton1.Checked == true)
                        {
                            if (this.DtpEDate.Value.Date < DateTime.Today.Date && this.DtpEDate.Value.Date != DateTime.Today.Date)
                            {
                                MessageBox.Show("Invalid Date", "Gainup");
                                if (radioButton1.Checked == true)
                                {
                                    DptTDate.MinDate = DateTime.Today.AddDays(0);
                                }
                                else
                                {
                                    DptTDate.MinDate = DateTime.Today.AddDays(-10);
                                }
                                DtpEDate.Focus();
                                DtpEDate.Enabled = true;
                                return;
                            }
                        }
                        else if (this.DptTDate.Value.Date < this.DtpEDate.Value.Date)
                        {
                            
                            //if (radioButton1.Checked == true)
                            //{
                            //    DptTDate.MinDate = DateTime.Today.AddDays(+1);
                            //}
                            //else
                            //{
                            //    DptTDate.MinDate = DateTime.Today.AddDays(-10);
                            //}
                            //DptTDate.Focus();
                            //DptTDate.Enabled = true;
                            //return;
                        }
                        if (radioButton3.Checked == true)
                        {
                            Txt_Reason.Focus();
                        }
                        else
                        {
                            DptTDate.Focus();
                        }
                    }
                    
                    else if (this.ActiveControl.Name == "DptTDate")
                    {
                        if (radioButton1.Checked == true)
                        {
                            if (this.DtpEDate.Value.Date < DateTime.Today.Date && this.DtpEDate.Value.Date != DateTime.Today.Date)
                            {
                                MessageBox.Show("Invalid Date", "Gainup");
                                if (radioButton1.Checked == true)
                                {
                                    DtpEDate.MinDate = DateTime.Today.AddDays(0);
                                }
                                else
                                {
                                    DtpEDate.MinDate = DateTime.Today.AddDays(-10);
                                }
                                DtpEDate.Focus();
                                DtpEDate.Enabled = true;
                                return;
                            }
                        }
                        else if (this.DptTDate.Value.Date < this.DtpEDate.Value.Date)
                        {
                            MessageBox.Show("Invalid Date", "Gainup");
                            if (radioButton1.Checked == true)
                            {
                                DptTDate.MinDate = DateTime.Today.AddDays(0);
                            }
                            else
                            {
                                DptTDate.MinDate = DateTime.Today.AddDays(-10);
                            }
                            DptTDate.Focus();
                            DptTDate.Enabled = true;
                            return;
                        }
                        Txt_Reason.Focus();
                    }
                    else if (this.ActiveControl.Name == "Txt_Reason")
                    {
                        Txt_Remark.Focus();
                    }
                    else if (this.ActiveControl.Name == "Txt_Remark")
                    {
                        button1.Focus();
                    }
                    else if (this.ActiveControl.Name == "Dpt1")
                    {
                        DtpShiftFrom.Focus();
                    }
                    else if (this.ActiveControl.Name == "DtpShiftFrom")
                    {
                        Dtp3.Focus();
                    }
                    else if (this.ActiveControl.Name == "Dtp3")
                    {
                        Txt_Reason.Focus();
                    }
                      
                Total_Count();
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (radioButton1.Checked == true)
                    {
                        if (this.DtpEDate.Value.Date < DateTime.Today.Date && this.DtpEDate.Value.Date != DateTime.Today.Date)
                        {
                            MessageBox.Show("Invalid Date", "Gainup");
                            DptTDate.MinDate = DateTime.Today.AddDays(0);
                            DtpEDate.Focus();
                            DtpEDate.Enabled = true;
                            return;
                        }
                        else if (this.DptTDate.Value.Date < this.DtpEDate.Value.Date)
                        {
                            MessageBox.Show("Invalid Date", "Gainup");
                            DptTDate.MinDate = DateTime.Today.AddDays(0);
                            DptTDate.Focus();
                            DptTDate.Enabled = true;
                            return;
                        }
                    }
                    if (radioButton1.Checked == true)
                    {
                        if (this.ActiveControl.Name == "Txt_Reason")
                        {
                            Str = "SELECT STATUS_DETAIL FROM Vaahini_Erp_Gainup.Dbo.Leave_Status_Master";
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "STATUS_DETAIL", Str, String.Empty, 150);
                            if (Dr != null)
                            {
                                Txt_Reason.Text = Dr["STATUS_DETAIL"].ToString();
                                Txt_Remark.Focus();
                            }
                        }
                    }
                    if (radioButton3.Checked == false && radioButton1.Checked == false)
                    {
                        
                        if ((this.ActiveControl.Name == "Txt_Reason" && radioButton3.Checked == false) || (this.ActiveControl.Name == "Txt_Reason" && radioButton1.Checked == false))
                        {
                            Str = "Select case when cast(getdate() as time) < cast('" + String.Format("{0:dd-MMM-yyyy HH:mm}", DtpShiftFrom.Value) + "' as time) then 1 else 0 end time";
                            DataTable Dt4 = new DataTable();

                            MyBase.Load_Data(Str, ref Dt4);


                            if (this.Dpt1.Value.Date == DateTime.Today.Date && Dt4.Rows[0]["Time"].ToString() == "0")
                            {
                                MessageBox.Show("Invalid Permission Hours..!", "Gainup");
                                Dtp3.Focus();
                                MyParent.Save_Error = true;
                                return;
                            }
                            //else if (this.Dpt1.Value.Date == DateTime.Today.Date  && this.DtpShiftFrom.Value.TimeOfDay < DateTime.Today.TimeOfDay)
                            //{
                            //    MessageBox.Show("Invalid Permission hours..!", "Gainup");
                            //    Dtp3.Focus();
                            //    MyParent.Save_Error = true;
                            //    return;
                            //}
                            else if (Convert.ToDouble(Txtdays.Text) <= 0)
                            {
                                MessageBox.Show("Invalid Permission hours..!", "Gainup");
                                Dtp3.Focus();
                                MyParent.Save_Error = true;
                                return;
                            }
                            else if (Convert.ToDouble(Txtdays.Text) >= 2.1 && checkBox2.Checked == true)
                            {
                                MessageBox.Show("If Official Permission Hours Exist 2:00 Hours Kindly Enter OD ...!", "Gainup");
                                Dtp3.Focus();
                                MyParent.Save_Error = true;
                                return;
                            }
                            else if (Convert.ToDouble(Txtdays.Text) >= 2.1 && Convert.ToDouble(Txtdays.Text) <= 4.1 && checkBox1.Checked == true)
                            {
                                MessageBox.Show("Permission Hours Exist 2:00 Hours ...! It Consider As HalfDay Leave", "Gainup");

                                Dtp3.Enabled = false;
                                DtpShiftFrom.Enabled = false;



                                Str = "SELECT STATUS_DETAIL FROM Vaahini_Erp_Gainup.Dbo.Leave_Status_Master";
                                if (radioButton2.Checked == true)
                                {
                                    if (checkBox2.Checked == true)
                                    {
                                        Str = Str + " Where remark_Id in (5,6,7)";
                                    }
                                    else
                                    {
                                        Str = Str + " Where remark_Id not in (5,6,7)";
                                    }
                                }
                                else
                                {
                                    Str = Str + " Where remark_Id not in (5,6,7)";
                                }
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "STATUS_DETAIL", Str, String.Empty, 150);
                                if (Dr != null)
                                {
                                    Txt_Reason.Text = Dr["STATUS_DETAIL"].ToString();
                                    Txt_Remark.Focus();
                                }
                                
                                //Dtp3.Focus();
                                //MyParent.Save_Error = true;
                                //return;
                            }
                            else if (Convert.ToDouble(Txtdays.Text) >= 4 && checkBox1.Checked == true)
                            {
                                MessageBox.Show("Permission Hours Exist 4:00 Hours ...! It Consider As One Day Leave......! Choose Leave Entry", "Gainup");
                                radioButton1.Checked = true;
                                return;
                            }
                            else
                            {
                                if (radioButton1.Checked == true)
                                {
                                    DptTDate.Enabled = false;
                                    DtpEDate.Enabled = false;
                                }
                                else if (radioButton2.Checked == true)
                                {
                                    Dtp3.Enabled = false;
                                    DtpShiftFrom.Enabled = false;
                                }
                                Str = "SELECT STATUS_DETAIL FROM Vaahini_Erp_Gainup.Dbo.Leave_Status_Master";
                                if (radioButton2.Checked == true)
                                {
                                    if (checkBox2.Checked == true)
                                    {
                                        Str = Str + " Where remark_Id in (5,6,7)";
                                    }
                                    else
                                    {
                                        Str = Str + " Where remark_Id not in (5,6,7)";
                                    }
                                }
                                else
                                {
                                    Str = Str + " Where remark_Id not in (5,6,7)";
                                }
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "STATUS_DETAIL", Str, String.Empty, 150);
                                if (Dr != null)
                                {
                                    Txt_Reason.Text = Dr["STATUS_DETAIL"].ToString();
                                    Txt_Remark.Focus();
                                }

                            }
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
        private void DtpEDate_ValueChanged_1(object sender, EventArgs e)
        {
            if (radioButton3.Checked == false)
            {
                DtpEDate.MinDate = DateTime.Today.Date;
            }
        }

        private void DptTDate_ValueChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == false)
            {
                DptTDate.MinDate = DateTime.Today.Date;
            }
        }

        private void button3_Click(object sender, EventArgs e)
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
                if (radioButton1.Checked == true)
                {
                    if (this.DtpEDate.Value.Date < DateTime.Today.Date && this.DtpEDate.Value.Date != DateTime.Today.Date)
                    {
                        MessageBox.Show("Invalid Date", "Gainup");
                        DtpEDate.Value = DateTime.Now;
                        DtpEDate.Focus();
                        DtpEDate.Enabled = true;
                        return;
                    }
                }
                if (radioButton3.Checked == true || radioButton2.Checked == true||Co_OFF_BTN.Checked == true)
                {
                }
                else if (this.DptTDate.Value.Date < this.DtpEDate.Value.Date)
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DptTDate.Value = DateTime.Today.AddDays(+1);
                    DptTDate.Focus();
                    DptTDate.Enabled = true;
                    return;
                }
                if (Txt_Name.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Name", "Gainup");
                    Txt_Tno.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (Txt_Reason.Text.ToString() == String.Empty && Co_OFF_BTN.Checked == false)
                {
                    MessageBox.Show("Invalid Reason", "Gainup");
                    Txt_Reason.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (Txt_Remark.Text.ToString() == String.Empty && Co_OFF_BTN.Checked == false)
                {
                    MessageBox.Show("Invalid remark", "Gainup");
                    Txt_Remark.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (Txt_AgName.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Responsible Name", "Gainup");
                    Txt_AgName.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                else if (Txtdays.Text.ToString() == String.Empty && radioButton3.Checked == false)
                {
                    DataTable Dt10 = new DataTable();
                    Str = "Select Edate, Intime, OUTTIME, SHIFT, Total_Hrs, Bal_Hrs, Att_Type from Vaahini_Erp_Gainup.dbo.CO_Date(getdate()," + MyParent.Emplno + ") Where edate not in (select CoffDate from VAAHINI_ERP_GAINUP.dbo.COff_Request_Entry where emplno = " + MyParent.Emplno + ") and edate = '" + String.Format("{0:dd-MMM-yyyy}", DtpCO.Value) + "'";
                    MyBase.Load_Data(Str, ref Dt10);
                    if (Dt10.Rows.Count > 0)
                    {
                        DtpCO.Value = Convert.ToDateTime(Dt10.Rows[0]["Edate"]);
                        DtpCO.Enabled = false;
                        Txtdays.Text = Convert.ToString(Dt10.Rows[0]["Bal_Hrs"]);
                        button6.Focus();
                        button1.Focus();
                    }
                    else if (Dt10.Rows.Count == 0)
                    {
                        MessageBox.Show("C-Off Date Not Available....!", "Gainup");
                        MyParent.Save_Error = true;
                        return;
                    }

                    //MessageBox.Show("C-Off Date Not Available", "Gainup");
                    //DtpCO.Focus();
                    //MyParent.Save_Error = true;
                    //return;
                }
                else
                {
                    Entry_Save();

                    DtpEDate.Value = DateTime.Today.AddDays(0);
                    DptTDate.Value = DateTime.Today.AddDays(0);
                    DptTDate.Enabled = true;
                    DtpEDate.Enabled = true;
                    Txt_Reason.Text = "";
                    Txt_Remark.Text = "";
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Already Entered ....!", "Gainup");
                MyBase.Clear(this);
            }

        }
        public void Entrysave()
        {
            try
            {
                if (radioButton2.Checked == true)
                {
                    if (checkBox2.Checked == true)
                    {
                        Str = "Insert Into VAAHINI_ERP_GAINUP.dbo.Staff_Permission_Rqs( Emplno, REmplno, Edate ,Fromtime , Totime, Type ,Total, Reason, Flag,Remarks) Values(" + Txt_EMPLNo.Text + "," + Remplno.Text + ",'" + String.Format("{0:dd-MMM-yyyy}", Dpt1.Value) + "', Cast('" + String.Format("{0:dd-MMM-yyyy HH:mm}", DtpShiftFrom.Value) + "' as Time), Cast('" + String.Format("{0:dd-MMM-yyyy HH:mm}", Dtp3.Value) + "' as Time),1, " + Txtdays.Text + ",'" + Txt_Reason.Text + "','R','" + Txt_Remark.Text + "')";
                    }
                    else
                    {
                        if (Convert.ToDouble(Txtdays.Text) >= 2.1 && checkBox1.Checked == true)
                        {
                            Str = "insert into VAAHINI_ERP_GAINUP.dbo.Staff_leave_Apply(emplno,fromdate,todate,reason,Permission_leave,Remplno,Flag,Remark)values(" + Txt_EMPLNo.Text + ",'" + String.Format("{0:dd-MMM-yyyy}", Dpt1.Value) + "','" + String.Format("{0:dd-MMM-yyyy}", Dpt1.Value) + "','" + Txt_Reason.Text + "',0.5," + Remplno.Text + ",'R','" + Txt_Remark.Text + "')";
                        }
                        else
                        {
                            Str = "Insert Into VAAHINI_ERP_GAINUP.dbo.Staff_Permission_Rqs( Emplno, REmplno, Edate ,Fromtime , Totime, Type ,Total, Reason, Flag,Remarks) Values(" + Txt_EMPLNo.Text + "," + Remplno.Text + ",'" + String.Format("{0:dd-MMM-yyyy}", Dpt1.Value) + "', Cast('" + String.Format("{0:dd-MMM-yyyy HH:mm}", DtpShiftFrom.Value) + "' as Time), Cast('" + String.Format("{0:dd-MMM-yyyy HH:mm}", Dtp3.Value) + "' as Time),0, " + Txtdays.Text + ",'" + Txt_Reason.Text + "','R','" + Txt_Remark.Text + "')";
                        }
                    }
                    MyBase.Run(Str);

                    MessageBox.Show("SAVED ....!", "Gainup");

                    Dt = new DataTable();
                    DtpEDate.Enabled = true;
                    Txt_Reason.Text = "";
                    Txt_Remark.Text = "";
                    DtpEDate.Focus();

                    radioButton1.Checked = true;
                    return; 
                }
                else if (Co_OFF_BTN.Checked == true)
                {
                    {
                       
                            Str = "Insert Into Vaahini_ERP_Gainup.Dbo.COff_Request_Entry(Emplno,CoffDate,INTIME,OUTTIME,Shift,Total_Hrs,REmplno,Approval_Flag) Select Emplno,Edate CoffDate, Intime, OUTTIME, SHIFT ,Bal_Hrs Total_Hrs, " + Remplno.Text + " REmplno,'R' Approval_Flag from Vaahini_Erp_Gainup.dbo.CO_Date_1(getdate()," + Txt_EMPLNo.Text + ") where edate = '" + String.Format("{0:dd-MMM-yyyy}", DtpCO.Value) + "'";
                        
                    }
                    MyBase.Run(Str);

                    MessageBox.Show("SAVED ....!", "Gainup");

                    Dt = new DataTable();
                    DtpEDate.Enabled = true;
                    Txt_Reason.Text = "";
                    Txt_Remark.Text = "";
                    DtpEDate.Focus();

                    radioButton1.Checked = true;
                    return;
                }
                else if (radioButton3.Checked == true)
                {

                    if (checkBox3.Checked == true)
                    {
                        Str = "Insert Into VAAHINI_ERP_GAINUP.dbo.OD_Req_Entry(Emplno,Fromdate,Att_Type_Res,Att_days,REmplno,Approval,Remark)Values(" + Txt_EMPLNo.Text + ",'" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "','" + Txt_Reason.Text + "',1," + Remplno.Text + ",'R','" + Txt_Remark.Text + "')";
                    }
                    else
                    {
                        Str = "Insert Into VAAHINI_ERP_GAINUP.dbo.OD_Req_Entry(Emplno,Fromdate,Att_Type_Res,Att_days,REmplno,Approval,Remark)Values(" + Txt_EMPLNo.Text + ",'" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "','" + Txt_Reason.Text + "',0.5," + Remplno.Text + ",'R','" + Txt_Remark.Text + "')";
                    }
                    MyBase.Run(Str);

                    MessageBox.Show("SAVED ....!", "Gainup");

                    Dt = new DataTable();
                    DtpEDate.Enabled = true;
                    Txt_Reason.Text = "";
                    Txt_Remark.Text = "";
                    DtpEDate.MaxDate = DateTime.Today.AddDays(+1000000);
                    DtpEDate.MinDate = DateTime.Today.AddDays(0);
                    radioButton3.Checked = true;
                    DtpEDate.Focus();
                    return; 
                }
                else
                {
                    MyBase.Run("insert into VAAHINI_ERP_GAINUP.dbo.Staff_leave_Apply(emplno,fromdate,todate,reason,leave_count,Remplno,Flag,Remark)values(" + Txt_EMPLNo.Text + ",'" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "','" + String.Format("{0:dd-MMM-yyyy}", DptTDate.Value) + "','" + Txt_Reason.Text + "'," + Txtdays.Text + "," + Remplno.Text + ",'R','" + Txt_Remark.Text + "')");

                    MessageBox.Show("SAVED ....!", "Gainup");

                    Dt = new DataTable();
                    DtpEDate.Enabled = true;
                    Txt_Reason.Text = "";
                    Txt_Remark.Text = "";
                    DptTDate.MinDate = DateTime.Today.AddDays(0);
                    DtpEDate.MaxDate = DateTime.Today.AddDays(+1000000);
                    DtpEDate.MinDate = DateTime.Today.AddDays(0);
                    DtpEDate.MaxDate = DateTime.Today.AddDays(+1000000);
                    radioButton1.Checked = true;
                    DtpEDate.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Already Entered ....!", "Gainup");
            }
        }
        public void Entry_Update()
        {
            try
            {
                if (radioButton2.Checked == true)
                {
                    if (checkBox2.Checked == true)
                    {
                        Str = "Update VAAHINI_ERP_GAINUP.dbo.Staff_Permission_Rqs set edate = '" + String.Format("{0:dd-MMM-yyyy}", Dpt1.Value) + "', fromtime = Cast('" + String.Format("{0:dd-MMM-yyyy HH:mm}", DtpShiftFrom.Value) + "' as Time),totime = Cast('" + String.Format("{0:dd-MMM-yyyy HH:mm}", Dtp3.Value) + "' as Time), Reason = '" + Txt_Reason.Text + "', total = " + Txtdays.Text + ", Type = 1 ,Remarks = '" + Txt_Remark.Text + "',entry_date = getdate() where rowid = "+Txt_Reason.Tag+"";
                    }
                    else
                    {
                        Str = "Update VAAHINI_ERP_GAINUP.dbo.Staff_Permission_Rqs set edate = '" + String.Format("{0:dd-MMM-yyyy}", Dpt1.Value) + "', fromtime = Cast('" + String.Format("{0:dd-MMM-yyyy HH:mm}", DtpShiftFrom.Value) + "' as Time),totime = Cast('" + String.Format("{0:dd-MMM-yyyy HH:mm}", Dtp3.Value) + "' as Time), Reason = '" + Txt_Reason.Text + "', total = " + Txtdays.Text + ", Type = 0 ,Remarks = '" + Txt_Remark.Text + "',entry_date = getdate() where rowid = "+Txt_Reason.Tag+"";
                    }
                    MyBase.Run(Str);

                    MessageBox.Show("Updated ....!", "Gainup");

                    Dt = new DataTable();
                    Txt_Reason.Text = "";
                    Txt_Remark.Text = "";
                    DtpEDate.Enabled = true;
                    button1.Visible = true;
                    button5.Visible = false;
                    DtpEDate.Focus();
                    radioButton1.Checked = true;
                    return;
                }
                else if (Co_OFF_BTN.Checked == true)
                {
                    Str = "Update VAAHINI_ERP_GAINUP.dbo.COff_Request_Entry set CoffDate = '" + String.Format("{0:dd-MMM-yyyy HH:mm}", DtpCO.Value) + "' , Total_Hrs = " + Txtdays.Text + ",Entry_date = getdate() where rowid = " + Txt_Reason.Tag + "";
                   
                    MyBase.Run(Str);

                    MessageBox.Show("Updated ....!", "Gainup");

                    Dt = new DataTable();
                    Txt_Reason.Text = "";
                    Txt_Remark.Text = "";
                    DtpEDate.Enabled = true;
                    button1.Visible = true;
                    button5.Visible = false;
                    DtpEDate.Focus();
                    radioButton1.Checked = true;
                    return;
                }

                else if (radioButton3.Checked == true)
                {
                    if (checkBox3.Checked == true)
                    {
                        MyBase.Run("Update VAAHINI_ERP_GAINUP.dbo.OD_Req_Entry set Att_days = 1 ,FromDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "', att_type_res = '" + Txt_Reason.Text + "',remark = '" + Txt_Remark.Text + "', entry_date = getdate() where rowid = " + Txt_Reason.Tag + "");
                    }
                    else
                    {
                        MyBase.Run("Update VAAHINI_ERP_GAINUP.dbo.OD_Req_Entry set Att_days = 0.5 ,FromDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "', att_type_res = '" + Txt_Reason.Text + "',remark = '" + Txt_Remark.Text + "', entry_date = getdate() where rowid = " + Txt_Reason.Tag + "");
                    }

                    MessageBox.Show("Updated ....!", "Gainup");

                    Dt = new DataTable();
                    DtpEDate.Enabled = true;
                    Txt_Reason.Text = "";
                    Txt_Remark.Text = "";
                    radioButton1.Checked = true;
                    DtpEDate.Focus();
                    return;
                }
                else
                {
                    Str = "Update VAAHINI_ERP_GAINUP.dbo.Staff_leave_Apply set fromdate = '" + String.Format("{0:dd-MMM-yyyy}", DtpEDate.Value) + "',todate = '" + String.Format("{0:dd-MMM-yyyy}", DptTDate.Value) + "', reason = '" + Txt_Reason.Text + "', leave_count = " + Txtdays.Text + ", edate = getdate() , remark ='" + Txt_Remark.Text + "' where rowid = " + Txt_Reason.Tag + "";
                    MyBase.Run(Str);

                    MessageBox.Show("Updated ....!", "Gainup");
                    Txt_Reason.Text = "";
                    Txt_Remark.Text = "";
                    Dt = new DataTable();
                    DtpEDate.Enabled = true;
                    button1.Visible = true;
                    button5.Visible = false;
                    radioButton1.Checked = true;
                    DtpEDate.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Already Entered ....!", "Gainup");
            }
        }
        private void GBMain_Enter(object sender, EventArgs e)
        {
            DtpEDate.Focus();
        }
        private void label14_Click(object sender, EventArgs e)
        {

        }
        private void Txt_Reason_TextChanged(object sender, EventArgs e)
        {

        }
        private void DtpShiftFrom_ValueChanged(object sender, EventArgs e)
        {

        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox2.Checked == false)
            {
                checkBox1.Checked = true;
                Txt_Reason.Text = "";
            }
            else
            {
                checkBox1.Checked = false;
                Txt_Reason.Text = "";
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                checkBox2.Checked = true;
            }
            else
            {
                checkBox2.Checked = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            DtpEDate.Enabled = true;
            DtpEDate.Focus();
            DptTDate.MinDate = DateTime.Today.AddDays(0);
            DptTDate.MaxDate = DateTime.Today.AddDays(+30);
            DptTDate.Value = DateTime.Today.AddDays(0);

            DtpEDate.MinDate = DateTime.Today.AddDays(0);
            DtpEDate.MaxDate = DateTime.Today.AddDays(+30);
            DtpEDate.Value = DateTime.Today.AddDays(0);

                DataTable Dt5 = new DataTable();
                label9.Visible = false;
                label2.Visible = true;
                DtpCO.Visible = false;
                label3.Visible = true;
                Txt_Reason.Visible = true;
                label15.Visible = true;
                Txt_Remark.Visible = true;
                //Arrow2.Visible = false;
                DptTDate.Enabled = true;
                DtpEDate.Enabled = true;
                label17.Visible = false;
                TxtIn.Visible = false;
                label18.Visible = false;
                TxtOut.Visible = false;
                DptTDate.Visible = true;
                DtpEDate.Visible = true;
                DtpShiftFrom.Visible = false;
                label16.Visible = false;
                Dtp3.Visible = false;
                DtpCO.Visible = false;
                label4.Visible = true;
                Arrow1.Visible = true;
                label1.Visible = true;
                Dpt1.Visible = false;
                label11.Visible = false;
                checkBox3.Visible = false;
                checkBox4.Visible = false;
                button1.Visible = true;
                button5.Visible = false;
                Txtdays.Visible = true;
                label14.Visible = false;
                label12.Visible = false;
                checkBox2.Visible = false;
                label7.Visible = false;
                Txt_Reason.Text = "";
                checkBox1.Visible = false;
                Txt_Remark.Text = "";
                label13.Visible = false;
                Total_Count();
                Str = "Select top 1 flag from VAAHINI_ERP_GAINUP.dbo.Staff_leave_Apply where emplno = " + MyParent.Emplno + " order by edate desc ";
                MyBase.Load_Data(Str, ref Dt5);
                DtpEDate.Focus();
                if (Dt5.Rows.Count > 0)
                {
                    if (Dt5.Rows[0]["Flag"].ToString() == "R")
                    {
                        Txt_Status.Text = "Request";
                        Txt_Status.ForeColor = Color.Pink;
                    }
                    else if (Dt5.Rows[0]["Flag"].ToString() == "F")
                    {
                        Txt_Status.Text = "Rejected";
                        Txt_Status.ForeColor = Color.Red;

                    }
                    else if (Dt5.Rows[0]["Flag"].ToString() == "A")
                    {
                        Txt_Status.Text = "Approved";
                        Txt_Status.ForeColor = Color.Green;

                    }
                }
                else
                {
                    Txt_Status.Text = "No Record";
                    Txt_Status.ForeColor = Color.Gray;
                }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                DataTable Dt5 = new DataTable();
                label9.Visible = true;
                label2.Visible = false;
                label1.Visible = true;
                Dpt1.Enabled = true;
                DptTDate.Visible = false;
                DtpEDate.Visible = false;
                DtpShiftFrom.Visible = true;
                DtpCO.Visible = false;
                Dtp3.Visible = true;
                label3.Visible = true;
                label11.Visible = false;
                checkBox3.Visible = false;
                label17.Visible = false;
                TxtIn.Visible = false;
                label18.Visible = false;
                TxtOut.Visible = false;
                checkBox4.Visible = false;
                Txt_Reason.Visible = true;
                label15.Visible = true;
                Txt_Remark.Visible = true;
                DtpCO.Visible = false;
                label16.Visible = false;
                label4.Visible = false;
                //Arrow2.Visible = false;
                Dpt1.Visible = true;
                checkBox2.Visible = true;
                label7.Visible = true;
                Arrow1.Visible = true;
                label12.Visible = true;
                Txtdays.Visible = true;
                label14.Visible = false;
                label13.Visible = true;
                checkBox1.Checked = true;
                checkBox1.Visible = true;
                button1.Visible = true;
                button5.Visible = false;
                Dtp3.Enabled = true;
                DtpShiftFrom.Enabled = true;
                Txt_Remark.Text = "";
                //Dpt1.MinDate = DateTime.Today.AddDays(-10);
                Dpt1.MinDate = DateTime.Today.AddDays(0);
                DtpShiftFrom.Value = DateTime.Now;
                Dtp3.Value = DateTime.Now;
                Total_Count();
                Str = "Select top 1 flag from VAAHINI_ERP_GAINUP.dbo.Staff_Permission_Rqs where emplno = " + MyParent.Emplno + " order by edate desc ";
                MyBase.Load_Data(Str, ref Dt5);

                if (Dt5.Rows.Count > 0)
                {
                    if (Dt5.Rows[0]["Flag"].ToString() == "R")
                    {
                        Txt_Status.Text = "Request";
                        Txt_Status.ForeColor = Color.Pink;
                    }
                    else if (Dt5.Rows[0]["Flag"].ToString() == "F")
                    {
                        Txt_Status.Text = "Rejected";
                        Txt_Status.ForeColor = Color.Red;
                    }
                    else if (Dt5.Rows[0]["Flag"].ToString() == "A")
                    {
                        Txt_Status.Text = "Approved";
                        Txt_Status.ForeColor = Color.Green;
                    }
                }
                else
                {
                    Txt_Status.Text = "No Record";
                    Txt_Status.ForeColor = Color.Gray;
                }
                Dpt1.Focus();
            }
            else
            {
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            DataTable Dt5 = new DataTable();
            DataTable Dt6 = new DataTable();
            label9.Visible = false;
            label2.Visible = false;
            label14.Visible = true;
            DptTDate.Visible = false;
            DtpEDate.Visible = true;
            DtpCO.Visible = false;
            DtpCO.Visible = false;
            checkBox3.Checked = true;
            label17.Visible = false;
            TxtIn.Visible = false;
            label18.Visible = false;
            TxtOut.Visible = false;


            //.Visible = false;
            label3.Visible = true;
            label11.Visible = true;
            checkBox3.Visible = true;
            checkBox4.Visible = true;
            Txtdays.Visible = false;
            Txt_Reason.Visible = true;
            label15.Visible = true;
            Txt_Remark.Visible = true;
            DtpShiftFrom.Visible = false;
            Dtp3.Visible = false;
            label16.Visible = false;
            label4.Visible = false;
            label1.Visible = true;
            Dpt1.Visible = false;
            DtpEDate.Enabled = true;
            label14.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            checkBox2.Visible = false;
            label7.Visible = false;
            button1.Visible = true;
            button5.Visible = false;
            Arrow1.Visible = false;
            checkBox1.Visible = false;
            Txt_Reason.Text = "";
            Total_Count();
            Str = "Select top 1 Approval from VAAHINI_ERP_GAINUP.dbo.OD_Req_Entry where emplno = " + MyParent.Emplno + " order by Entry_date desc ";
            MyBase.Load_Data(Str, ref Dt5);

              
            DtpEDate.MaxDate = DateTime.Today.AddDays(+1000000);
            DtpEDate.MinDate = DateTime.Today.AddDays(0);
            DtpEDate.Value = DateTime.Now;

            if (Dt5.Rows.Count > 0)
            {
                if (Dt5.Rows[0]["Approval"].ToString() == "R")
                {
                    Txt_Status.Text = "Request";
                    Txt_Status.ForeColor = Color.Pink;
                }
                else if (Dt5.Rows[0]["Approval"].ToString() == "F")
                {
                    Txt_Status.Text = "Rejected";
                    Txt_Status.ForeColor = Color.Red;

                }
                else if (Dt5.Rows[0]["Approval"].ToString() == "A")
                {
                    Txt_Status.Text = "Approved";
                    Txt_Status.ForeColor = Color.Green;
                }
            }
            else
            {
                Txt_Status.Text = "No Record";
                Txt_Status.ForeColor = Color.Gray;
            }
            DtpEDate.Focus();
        }

        private void Dtp3_ValueChanged(object sender, EventArgs e)
        {

        }
        private void Dpt1_ValueChanged(object sender, EventArgs e)
        {

        }
        private void myTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Arrow1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                if (Co_OFF_BTN.Checked == true)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select EDIT..!", " Select * from Vaahini_Erp_Gainup.dbo.Coff_Request_View(" + MyParent.Emplno + ")", String.Empty, 100, 80, 250, 200, 200, 200);
                    if (Dr != null)
                    {
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        DtpCO.Text = Convert.ToString(Dr["CO_Date"]);
                        TxtIn.Text = Convert.ToString(Dr["Intime"]);
                        TxtOut.Text = Convert.ToString(Dr["OUTTIME"]);
                        Txtdays.Text = Dr["Total_Hrs"].ToString();
                        button1.Visible = false;
                        button5.Visible = false;
                    }
                       
                }
                else if (radioButton1.Checked == true)
                {
                    Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "VIEW.!", " SELECT Tno, Name, Department, FROMDATE, TODATE, leave_count, REASON, HOD, Designation, APPROVAL_STATUS,Remark,ROWID,Entry_Date FROM VAAHINI_ERP_GAINUP.dbo.leave_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + "", String.Empty, 80, 200, 150, 100, 100, 50, 100, 200, 150, 100, 100);
                    if (Dr != null)
                    {
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        DtpEDate.MinDate = DateTime.Today.AddDays(-100);
                        DtpEDate.Value = Convert.ToDateTime(Dr["FROMDATE"]);
                        DptTDate.MinDate = DateTime.Today.AddDays(-100);
                        DptTDate.Value = Convert.ToDateTime(Dr["TODATE"]);
                        Txt_Reason.Text = Dr["REASON"].ToString();
                        Txtdays.Text = Dr["Leave_count"].ToString();
                        Txt_Remark.Text = Dr["REMARK"].ToString();
                        button1.Visible = false;
                        button5.Visible = false;
                    }
                    
                }
                else if (radioButton2.Checked == true)
                {
                    Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "VIEW..!", "SELECT Tno, Name, Department, Permission_date, from_time, to_time,total, REASON, HOD, Designation, APPROVAL_STATUS,REMARK, Entry_Date , ROWID   FROM VAAHINI_ERP_GAINUP.dbo.Permission_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + " ", String.Empty, 80, 150, 100, 100, 80, 80, 80, 180, 100, 200, 100, 100);
                    if (Dr != null)
                    {
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        Dpt1.MinDate = DateTime.Today.AddDays(-1000);
                        Dpt1.Value = Convert.ToDateTime(Dr["Permission_date"]);
                        DtpShiftFrom.Value = Convert.ToDateTime(Dr["from_time"]);
                        Dtp3.Value = Convert.ToDateTime(Dr["to_time"]);


                        Txt_Reason.Text = Dr["REASON"].ToString();
                        Txtdays.Text = Dr["total"].ToString();
                        Txt_Remark.Text = Dr["REMARK"].ToString();
                        button1.Visible = false;
                        button5.Visible = false;

                    }

                }
    
                else if (radioButton3.Checked == true)
                {
                    Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "VIEW..!", " Select Tno, Name, Department, OD_Date, Att_days, Reason, HOD, Designation, Approval_Status,Remark,rowid,Entry_Date from VAAHINI_ERP_GAINUP.dbo.OD_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + " ", String.Empty, 80, 200, 150, 100, 200, 200, 150, 100, 100,100);
                    if (Dr != null)
                    {
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        DtpEDate.MinDate = DateTime.Today.AddDays(-1000);
                        DtpEDate.Value = Convert.ToDateTime(Dr["OD_Date"]);
                        Txt_Reason.Text = Dr["REASON"].ToString();

                        Txt_Remark.Text = Dr["REMARK"].ToString();
                        button1.Visible = false;
                        button5.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Already Entered ....!", "Gainup");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                if (radioButton2.Checked == true)
                {
                    //Dr = Tool.Selection_Tool(this, 100, 100, SelectionTool_Class.ViewType.NormalView, "VIEW..!", "SELECT Tno, Name, Department, Permission_date,cast(from_time as datetime) from_time,cast(to_time as datetime) to_time,total, REASON, HOD, Designation, APPROVAL_STATUS,REMARK, Entry_Date , ROWID   FROM VAAHINI_ERP_GAINUP.dbo.Permission_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + " ", String.Empty, 80, 150, 100, 100, 50, 50, 50, 180, 100, 200, 100, 100);
                    //if (Dr != null)
                    //{
                    //    Txt_Reason.Tag = Dr["ROWID"].ToString();
                    //    Dpt1.MinDate = DateTime.Today.AddDays(-1000);
                    //    Dpt1.Value = Convert.ToDateTime(Dr["Permission_date"]);
                    //    DtpShiftFrom.Value = Convert.ToDateTime(Dr["from_time"]);
                    //    Dtp3.Value = Convert.ToDateTime(Dr["to_time"]);
                    //    Txt_Reason.Text = Dr["REASON"].ToString();
                    //    Txtdays.Text = Dr["total"].ToString();
                    //    Txt_Remark.Text = Dr["REMARK"].ToString();
                    //    button1.Visible = false;
                    //    button5.Visible = false;

                    //}
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select EDIT..!", "SELECT Tno, Name, Department, Permission_date,cast(from_time as datetime) from_time,cast(to_time as datetime) to_time,total, REASON, Entry_Date, HOD, Designation, APPROVAL_STATUS, ROWID , REMARK FROM VAAHINI_ERP_GAINUP.dbo.Permission_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + " AND APPROVAL_STATUS = 'Requested'", String.Empty, 100, 80, 250, 200, 200, 200, 180, 180, 50, 200, 100);
                    if (Dr != null)
                    {
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        Dpt1.MinDate = DateTime.Today.AddDays(-1000);
                        Dpt1.Value = Convert.ToDateTime(Dr["Permission_date"]);
                        DtpShiftFrom.Value = Convert.ToDateTime(Dr["from_time"]);
                        //Dtp3.Value = Convert.ToDateTime(Dr["to_time"]);
                        Txt_Reason.Text = Dr["REASON"].ToString();
                        Txtdays.Text = Dr["total"].ToString();
                        Txt_Remark.Text = Dr["REMARK"].ToString();
                        button1.Visible = false;
                        button5.Visible = true;
                    }
                    else
                    {
                        Txtdays.Text = "";
                        return;
                    }
                }
                else if (radioButton1.Checked == true)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select EDIT..!", " SELECT Tno, Name, Department, FROMDATE, TODATE, leave_count, REASON, Entry_Date, HOD, Designation, APPROVAL_STATUS,ROWID,Remark FROM VAAHINI_ERP_GAINUP.dbo.leave_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + " AND APPROVAL_STATUS = 'Requested'", String.Empty, 100, 80, 250, 200, 200, 200, 180, 180, 50, 200);
                    if (Dr != null)
                    {
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        DtpEDate.MinDate = DateTime.Today.AddDays(0);
                        DtpEDate.Value = Convert.ToDateTime(Dr["FROMDATE"]);
                        DptTDate.MinDate = DateTime.Today.AddDays(0);
                        DptTDate.Value = Convert.ToDateTime(Dr["TODATE"]);
                        Txt_Reason.Text = Dr["REASON"].ToString();
                        Txtdays.Text = Dr["leave_count"].ToString();
                        Txt_Remark.Text = Dr["REMARK"].ToString();
                        button1.Visible = false;
                        button5.Visible = true;
                    }
                    else
                    {
                        Txtdays.Text = "";
                        return;
                    }
                }
                else if (radioButton3.Checked == true)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select EDIT..!", " Select Tno, Name, Department, OD_Date, Att_days, Reason, Entry_Date, HOD, Designation, Approval_Status,rowid,Remark from VAAHINI_ERP_GAINUP.dbo.OD_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + " AND APPROVAL_STATUS = 'Requested'", String.Empty, 100, 80, 250, 200, 200, 200, 180, 180, 50, 200);
                    if (Dr != null)
                    {
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        DtpEDate.Value = Convert.ToDateTime(Dr["OD_Date"]);
                        Txt_Reason.Text = Dr["REASON"].ToString();
                        
                        Txt_Remark.Text = Dr["REMARK"].ToString();
                        button1.Visible = false;
                        button5.Visible = true;
                    }
                    else
                    {
                        Txtdays.Text = "";
                        return;
                    }
                }
                else if (Co_OFF_BTN.Checked == true)
                {
                    Txtdays.Text = "";
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select EDIT..!", " Select * from Vaahini_Erp_Gainup.dbo.Coff_Request_Edit("+MyParent.Emplno+")", String.Empty, 100, 100, 200, 150, 150, 150);
                    if (Dr != null)
                    { 
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        DtpCO.Value = Convert.ToDateTime(Dr["CO_Date"]);
                        //Txtdays.Text = Dr["Total_Hrs"].ToString();
                        button1.Visible = false;
                        button5.Visible = true;
                        DtpCO.Focus();
                    }
                    else
                    {
                        Txtdays.Text = "";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Entered Wrongly....!", "Gainup");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked == true)
                {
                    if (this.DtpEDate.Value.Date < DateTime.Today.Date && this.DtpEDate.Value.Date != DateTime.Today.Date)
                    {
                        MessageBox.Show("Invalid Date", "Gainup");
                        DtpEDate.Value = DateTime.Now;
                        DtpEDate.Focus();
                        DtpEDate.Enabled = true;
                        return;
                    }
                }
                
                    if ((Txtdays.Text.ToString() == String.Empty || Convert.ToDouble(Txtdays.Text) <= 0) && radioButton2.Checked == true)
                    {
                        MessageBox.Show("Invalid Permission hours..!", "Gainup");
                        Dtp3.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
               
                else if (radioButton3.Checked == true)
                {
                }
                else if (this.DptTDate.Value.Date < this.DtpEDate.Value.Date && Co_OFF_BTN.Checked == false)
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DptTDate.Value = DateTime.Today.AddDays(0); 
                    DptTDate.Focus();
                    DptTDate.Enabled = true;
                    return;
                }
                if (Txt_Name.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Name", "Gainup");
                    Txt_Tno.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (Txt_Reason.Text.ToString() == String.Empty && Co_OFF_BTN.Checked == false)
                {
                    MessageBox.Show("Invalid Reason", "Gainup");
                    Txt_Reason.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (Txt_Remark.Text.ToString() == String.Empty && Co_OFF_BTN.Checked == false)
                {
                    MessageBox.Show("Invalid remark", "Gainup");
                    Txt_Remark.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (Txt_AgName.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Responsible Name", "Gainup");
                    Txt_AgName.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (Co_OFF_BTN.Checked == true)
                {
                    if (Txtdays.Text.ToString() == String.Empty)
                    {
                        MessageBox.Show("Invalid Coff Date", "Gainup");
                        DtpCO.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    else
                    {
                        Entry_Update();
                        DptTDate.Enabled = true;
                        DtpEDate.Enabled = true;
                        DtpEDate.MinDate = DateTime.Today.AddDays(0);
                        DptTDate.MinDate = DateTime.Today.AddDays(0);
                        DtpEDate.Value = DateTime.Today.AddDays(0);
                        DptTDate.Value = DateTime.Today.AddDays(0);
                        DptTDate.Enabled = true;
                        DtpEDate.Enabled = true;
                        Txt_Reason.Text = "";
                        Txt_Remark.Text = "";
                        return;
                    }
                }
                else
                {
                    Total_Count();
                    Entry_Update();
                    DptTDate.Enabled = true;
                    DtpEDate.Enabled = true;
                    DtpEDate.MinDate = DateTime.Today.AddDays(0);
                    DptTDate.MinDate = DateTime.Today.AddDays(0);
                    DtpEDate.Value = DateTime.Today.AddDays(0);
                    DptTDate.Value = DateTime.Today.AddDays(0);
                    DptTDate.Enabled = true;
                    DtpEDate.Enabled = true;
                    Txt_Reason.Text = "";
                    Txt_Remark.Text = "";
                    return;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Already Entered ....!", "Gainup");
                MyBase.Clear(this);
            }

        }
        public void Entry_Delete()
        {
          try
            {
                if (radioButton2.Checked == true)
                {

                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select EDIT..!", "SELECT Tno, Name, Department, Permission_date,cast(from_time as datetime) from_time,cast(to_time as datetime) to_time,total, REASON, Entry_Date, HOD, Designation, APPROVAL_STATUS, ROWID , REMARK FROM VAAHINI_ERP_GAINUP.dbo.Permission_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + " AND APPROVAL_STATUS = 'Requested' AND CAST(PERMISSION_DATE AS DATE) >= CAST(GETDATE() AS DATE) ", String.Empty, 100, 80, 250, 200, 200, 200, 180, 180, 50, 200, 100);
                    if (Dr != null)
                    {
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        Dpt1.Value = Convert.ToDateTime(Dr["Permission_date"]);
                        DtpShiftFrom.Value = Convert.ToDateTime(Dr["from_time"]);
                        Dtp3.Value = Convert.ToDateTime(Dr["To_time"]);
                        Txt_Reason.Text = Dr["REASON"].ToString();
                        Txtdays.Text = Dr["total"].ToString();
                        Txt_Remark.Text = Dr["REMARK"].ToString();
                        
                        DialogResult m = MessageBox.Show("Sure to Delete...!", "Permission Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (m == DialogResult.Yes)
                        {
                            Total_Count();
                            MyBase.Run("Delete from VAAHINI_ERP_GAINUP.dbo.Staff_Permission_Rqs where ROWID = " + Txt_Reason.Tag + "");

                            MessageBox.Show("Deleted ....!", "Gainup");
                            Dt = new DataTable();
                            DtpEDate.Enabled = true;
                            button1.Visible = true;
                            button5.Visible = false;
                            DtpEDate.Focus();
                            Txt_Reason.Text = "";
                            Txt_Remark.Text = "";
                            radioButton1.Checked = true;
                            return;

                        }
                        else
                        {
                            Txtdays.Text = "";
                            return;
                        }
                    }
                    else
                    {
                        Txtdays.Text = "";
                        return;
                    }
                }
                else if (radioButton1.Checked == true)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Delete..!", " SELECT Tno, Name, Department, FROMDATE, TODATE, leave_count, REASON, Entry_Date, HOD, Designation, APPROVAL_STATUS,ROWID,Remark FROM VAAHINI_ERP_GAINUP.dbo.leave_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + " AND APPROVAL_STATUS = 'Requested'", String.Empty, 100, 80, 250, 200, 200, 200, 180, 180, 50, 200);
                    if (Dr != null)
                    {
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        DtpEDate.Value = Convert.ToDateTime(Dr["FROMDATE"]);
                        DptTDate.Value = Convert.ToDateTime(Dr["TODATE"]);
                        Txt_Reason.Text = Dr["REASON"].ToString();
                        Txtdays.Text = Dr["leave_count"].ToString();
                        Txt_Remark.Text = Dr["REMARK"].ToString();
                        DialogResult m = MessageBox.Show("Sure to Delete...!", "Permission Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        
                        if (m == DialogResult.Yes)
                        {
                            Total_Count();
                            MyBase.Run("Delete from VAAHINI_ERP_GAINUP.dbo.Staff_Leave_Apply where ROWID = " + Txt_Reason.Tag + "");

                            MessageBox.Show("Deleted ....!", "Gainup");
                            Dt = new DataTable();
                            DtpEDate.Value = DateTime.Today.AddDays(0);
                            DptTDate.Value = DateTime.Today.AddDays(0);
                            DptTDate.Enabled = true;
                            DtpEDate.Enabled = true;
                            Txt_Reason.Text = "";
                            Txt_Remark.Text = "";
                            button1.Visible = true;
                            button5.Visible = false;
                            DtpEDate.Focus();
                            radioButton1.Checked = true;
                            return;

                        }
                        else
                        {
                            Txtdays.Text = "";
                            return;
                        }
                    }
                    else
                    {
                        Txtdays.Text = "";
                        return;
                    }
                }
                else if (radioButton3.Checked == true)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select EDIT..!", " Select Tno, Name, Department, OD_Date, Reason, Entry_Date, HOD, Designation, Approval_Status,rowid,Remark from VAAHINI_ERP_GAINUP.dbo.OD_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + " AND APPROVAL_STATUS = 'Requested'", String.Empty, 100, 80, 250, 200, 200, 200, 180, 180, 50, 200);
                    if (Dr != null)
                    {
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        DtpEDate.Value = Convert.ToDateTime(Dr["OD_Date"]);
                        Txt_Reason.Text = Dr["REASON"].ToString();
                        
                        Txt_Remark.Text = Dr["REMARK"].ToString();
                        
                        DialogResult m = MessageBox.Show("Sure to Delete...!", "Permission Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (m == DialogResult.Yes)
                        {
                            Total_Count();
                            MyBase.Run("Delete from VAAHINI_ERP_GAINUP.dbo.OD_Req_Entry where ROWID = " + Txt_Reason.Tag + "");

                            MessageBox.Show("Deleted ....!", "Gainup");
                            Dt = new DataTable();
                            DtpEDate.Enabled = true;
                            button1.Visible = true;
                            button5.Visible = false;
                            DtpEDate.Focus();
                            Txt_Reason.Text = "";
                            Txt_Remark.Text = "";
                            radioButton1.Checked = true;
                            return;

                        }
                        else
                        {
                            Txtdays.Text = "";
                            return;
                        }
                    }
                    else
                    {
                        Txtdays.Text = "";
                        return;
                    }
                }
             }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong Entry ....!", "Gainup");
            }
        }
        private void button6_Click(object sender, EventArgs e)
     
        {
             try
            {
                if (radioButton2.Checked == true)
                {

                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select EDIT..!", "SELECT Tno, Name, Department, Permission_date,cast(from_time as datetime) from_time,cast(to_time as datetime) to_time,total, REASON, Entry_Date, HOD, Designation, APPROVAL_STATUS, ROWID , REMARK FROM VAAHINI_ERP_GAINUP.dbo.Permission_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + " AND APPROVAL_STATUS = 'Requested' AND CAST(PERMISSION_DATE AS DATE) >= CAST(GETDATE() AS DATE) ", String.Empty, 100, 80, 250, 200, 200, 200, 180, 180, 50, 200, 100);
                    if (Dr != null)
                    {
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        Dpt1.Value = Convert.ToDateTime(Dr["Permission_date"]);
                        DtpShiftFrom.Value = Convert.ToDateTime(Dr["from_time"]);
                        Dtp3.Value = Convert.ToDateTime(Dr["To_time"]);
                        Txt_Reason.Text = Dr["REASON"].ToString();
                        Txtdays.Text = Dr["total"].ToString();
                        Txt_Remark.Text = Dr["REMARK"].ToString();
                        
                        DialogResult m = MessageBox.Show("Sure to Delete...!", "Permission Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (m == DialogResult.Yes)
                        {
                            Total_Count();
                            MyBase.Run("Delete from VAAHINI_ERP_GAINUP.dbo.Staff_Permission_Rqs where ROWID = " + Txt_Reason.Tag + "");

                            MessageBox.Show("Deleted ....!", "Gainup");
                            Dt = new DataTable();
                            DtpEDate.Enabled = true;
                            button1.Visible = true;
                            button5.Visible = false;
                            DtpEDate.Focus();
                            Txt_Reason.Text = "";
                            Txt_Remark.Text = "";
                            radioButton1.Checked = true;
                            return;

                        }
                        else
                        {
                            Txtdays.Text = "";
                            return;
                        }
                    }
                }
              else if (Co_OFF_BTN.Checked == true)
                {

                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select EDIT..!", " Select * from Vaahini_Erp_Gainup.dbo.Coff_Request_Edit(" + MyParent.Emplno + ")", String.Empty, 100, 100, 200, 150, 150, 150);
                    if (Dr != null)
                    {
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        DtpCO.Value = Convert.ToDateTime(Dr["CO_Date"]);

                        Txtdays.Text = Dr["Total_Hrs"].ToString();
    
                        DialogResult m = MessageBox.Show("Sure to Delete...!", "Permission Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (m == DialogResult.Yes)
                        {
                            Total_Count();
                            MyBase.Run("Delete from VAAHINI_ERP_GAINUP.dbo.COff_Request_Entry where ROWID = " + Txt_Reason.Tag + "");

                            MessageBox.Show("Deleted ....!", "Gainup");
                            Dt = new DataTable();
                            DtpEDate.Enabled = true;
                            button1.Visible = true;
                            button5.Visible = false;
                            DtpEDate.Focus();
                            Txt_Reason.Text = "";
                            Txt_Remark.Text = "";
                            radioButton1.Checked = true;
                            return;

                        }
                        else
                        {
                            Txtdays.Text = "";
                            return;
                        }
                    }
                }
                else if (radioButton1.Checked == true)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Delete..!", " SELECT Tno, Name, Department, FROMDATE, TODATE, leave_count, REASON, Entry_Date, HOD, Designation, APPROVAL_STATUS,ROWID,Remark FROM VAAHINI_ERP_GAINUP.dbo.leave_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + " AND APPROVAL_STATUS = 'Requested'", String.Empty, 100, 80, 250, 200, 200, 200, 180, 180, 50, 200);
                    if (Dr != null)
                    {
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        DtpEDate.Value = Convert.ToDateTime(Dr["FROMDATE"]);
                        DptTDate.Value = Convert.ToDateTime(Dr["TODATE"]);
                        Txt_Reason.Text = Dr["REASON"].ToString();
                        Txtdays.Text = Dr["leave_count"].ToString();
                        Txt_Remark.Text = Dr["REMARK"].ToString();
                        DialogResult m = MessageBox.Show("Sure to Delete...!", "Permission Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        
                        if (m == DialogResult.Yes)
                        {
                            Total_Count();
                            MyBase.Run("Delete from VAAHINI_ERP_GAINUP.dbo.Staff_Leave_Apply where ROWID = " + Txt_Reason.Tag + "");

                            MessageBox.Show("Deleted ....!", "Gainup");
                            Dt = new DataTable();
                            DtpEDate.Value = DateTime.Today.AddDays(0);
                            DptTDate.Value = DateTime.Today.AddDays(0);
                            DptTDate.Enabled = true;
                            DtpEDate.Enabled = true;
                            Txt_Reason.Text = "";
                            Txt_Remark.Text = "";
                            button1.Visible = true;
                            button5.Visible = false;
                            DtpEDate.Focus();
                            radioButton1.Checked = true;
                            return;

                        }
                        else
                        {
                            Txtdays.Text = "";
                            return;
                        }
                    }
                }
                else if (radioButton3.Checked == true)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select EDIT..!", " Select Tno, Name, Department, OD_Date, Reason, Entry_Date, HOD, Designation, Approval_Status,rowid,Remark from VAAHINI_ERP_GAINUP.dbo.OD_Approval_Status_Deatail() WHERE EMPLNO = " + MyParent.Emplno + " AND APPROVAL_STATUS = 'Requested'", String.Empty, 100, 80, 250, 200, 200, 200, 180, 180, 50, 200);
                    if (Dr != null)
                    {
                        Txt_Reason.Tag = Dr["ROWID"].ToString();
                        DtpEDate.Value = Convert.ToDateTime(Dr["OD_Date"]);
                        Txt_Reason.Text = Dr["REASON"].ToString();
                        
                        Txt_Remark.Text = Dr["REMARK"].ToString();
                        
                        DialogResult m = MessageBox.Show("Sure to Delete...!", "Permission Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (m == DialogResult.Yes)
                        {
                            Total_Count();
                            MyBase.Run("Delete from VAAHINI_ERP_GAINUP.dbo.OD_Req_Entry where ROWID = " + Txt_Reason.Tag + "");

                            MessageBox.Show("Deleted ....!", "Gainup");
                            Dt = new DataTable();
                            DtpEDate.Enabled = true;
                            button1.Visible = true;
                            button5.Visible = false;
                            DtpEDate.Focus();
                            Txt_Reason.Text = "";
                            Txt_Remark.Text = "";
                            radioButton1.Checked = true;
                            return;

                        }
                        else
                        {
                            Txtdays.Text = "";
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong Entry ....!", "Gainup");
            }
        
        }

        private void Dtp3_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void Dtp3_Leave(object sender, EventArgs e)
        {
            try
            {
                long Temp = 0;
                long Temp1 = 0;
                Temp1 = MyBase.DateDiff(Control_Modules.DateInterval.Minute, DtpShiftFrom.Value, Dtp3.Value);
                Temp = Temp1 / 60;
                Temp1 = Temp1 - (Temp * 60);
                String Tt1 = Convert.ToString(Temp1);
                if (Tt1.Length == 1)
                {
                    Tt1 = "0" + Tt1;
                }
                Txtdays.Text = Convert.ToString(Temp) + "." + Tt1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DtpShiftFrom_Leave(object sender, EventArgs e)
        {
            try
            {
                Dtp3_Leave(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Co_OFF_BTN_CheckedChanged(object sender, EventArgs e)
        {
            if (Co_OFF_BTN.Checked == true)
            {
                DataTable Dt5 = new DataTable();
                DataTable Dt6 = new DataTable();
                label9.Visible = true;
                label2.Visible = false;
                label16.Visible = true;
                label14.Visible = true;

                label17.Visible = true;
                TxtIn.Visible = true;
                label18.Visible = true;
                TxtOut.Visible = true;
                TxtIn.Text = "";
                TxtOut.Text = "";
                //.Visible = true;
                DtpEDate.Enabled = false;
                DtpCO.Visible = true;
                DtpCO.Enabled = true;
                DptTDate.Visible = false;
                DtpEDate.Visible = true;
                Dpt1.Enabled = false;
                DtpShiftFrom.Visible = false;
                Dtp3.Visible = true;
                label4.Visible = false;
                label1.Visible = true;
                Dpt1.Visible = false;
                label14.Visible = false;
                Txtdays.Visible = true;
                label12.Visible = false;
                label13.Visible = false;
                label11.Visible = false;
                checkBox3.Visible = false;
                checkBox4.Visible = false;
                checkBox2.Visible = false;
                label7.Visible = false;
                button1.Visible = true;
                button5.Visible = false;
                label15.Visible = false;
                label3.Visible = false;
                Txt_Reason.Visible = false;
                Txt_Remark.Visible = false;
                Arrow1.Visible = false;
                Arrow1.Visible = false;
                checkBox1.Visible = false;
                Txt_Reason.Text = "";
                Txtdays.Text = "";
                Dpt1.Visible = false;
                Dtp3.Visible = false;
                DtpCO.Enabled = true;
                DtpCO.Value = MyBase.GetServerDateTime();
                Str = "Select top 1 Approval_Flag from Vaahini_Erp_Gainup.dbo.COff_Request_Entry where Emplno = " + MyParent.Emplno + " order by Entry_date desc";
                MyBase.Load_Data(Str, ref Dt5);
                if (Dt5.Rows.Count > 0)
                {
                    if (Dt5.Rows[0]["Approval_Flag"].ToString() == "R")
                    {
                        Txt_Status.Text = "Request";
                        Txt_Status.ForeColor = Color.Pink;
                    }
                    else if (Dt5.Rows[0]["Approval_Flag"].ToString() == "F")
                    {
                        Txt_Status.Text = "Rejected";
                        Txt_Status.ForeColor = Color.Red;

                    }
                    else if (Dt5.Rows[0]["Approval_Flag"].ToString() == "A")
                    {
                        Txt_Status.Text = "Approved";
                        Txt_Status.ForeColor = Color.Green;

                    }
                }
                else
                {
                    Txt_Status.Text = "No Record";
                    Txt_Status.ForeColor = Color.Gray;
                }
                DtpCO.Focus();
            }
        }

        private void DtpCo2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.ActiveControl.Name == "DtpCO")
                {
                    DataTable Dt11 = new DataTable();
                    String Str2 = String.Empty;

                    Str2 = "Select * from Vaahini_Erp_Gainup.dbo.Empl_Att Where Emplno = " + MyParent.Emplno + " and edate = '" + String.Format("{0:dd-MMM-yyyy}", DtpCO.Value) + "' ";
                    MyBase.Load_Data(Str2, ref Dt11);

                    DataTable Dt12 = new DataTable();
                    String Str3 = String.Empty;

                    Str3 = "Select case when cast((Case When Datename(Dw,getdate())='Monday' Then DateAdd(day,-4,Getdate()) Else DateAdd(day,-3,getdate()) End) as date) > '" + String.Format("{0:dd-MMM-yyyy}", DtpCO.Value) + "' then 0 else 1 end ";
                    MyBase.Load_Data(Str3, ref Dt12);

                    DataTable Dt10 = new DataTable();
                    Str = "Select Edate,ltrim(right(convert(varchar(25), Intime, 100), 7)) Intime, ltrim(right(convert(varchar(25), Outtime, 100), 7)) Outtime, SHIFT, Total_Hrs, Bal_Hrs, Att_Type from Vaahini_Erp_Gainup.dbo.CO_Date(getdate()," + MyParent.Emplno + ") Where edate not in (select CoffDate from VAAHINI_ERP_GAINUP.dbo.COff_Request_Entry where emplno = " + MyParent.Emplno + ") and edate = '" + String.Format("{0:dd-MMM-yyyy}", DtpCO.Value) + "'";
                    MyBase.Load_Data(Str, ref Dt10);
                    
                    if (Dt11.Rows.Count == 0)
                    {
                        MessageBox.Show("Attendance Not Stored Try Again Later....!", "Gainup");
                        MyParent.Save_Error = true;
                        return;
                    }
                    else if (Dt12.Rows[0][0].ToString() == "0")
                    {
                        MessageBox.Show("Three Days Coff Date Locked....!", "Gainup");
                        MyParent.Save_Error = true;
                        return;
                    }
                    else if (Dt10.Rows.Count > 0)
                    {
                        DtpCO.Value = Convert.ToDateTime(Dt10.Rows[0]["Edate"]);
                        DtpCO.Enabled = false;
                        Txtdays.Text = Convert.ToString(Dt10.Rows[0]["Bal_Hrs"]);
                        TxtIn.Text = Convert.ToString(Dt10.Rows[0]["Intime"]);
                        TxtOut.Text = Convert.ToString(Dt10.Rows[0]["OUTTIME"]);
                        button6.Focus();
                        button1.Focus();
                    }
                    else if (Dt10.Rows.Count == 0)
                    {
                        MessageBox.Show("C-Off Date Not Available....!", "Gainup");
                        MyParent.Save_Error = true;
                        return;
                    }

                }
            }
            else if (e.KeyCode == Keys.Down ||e.KeyCode == Keys.Up)
            {
                if (this.ActiveControl.Name == "TxtCodate")
                {
                    DataTable Dt10 = new DataTable();
                    Str = "Select Edate, Intime, OUTTIME, SHIFT, Total_Hrs, Bal_Hrs, Att_Type from Vaahini_Erp_Gainup.dbo.CO_Date(getdate()," + MyParent.Emplno + ") Where edate not in (select CoffDate from VAAHINI_ERP_GAINUP.dbo.COff_Request_Entry where emplno = " + MyParent.Emplno + ")";
                    MyBase.Load_Data(Str, ref Dt10);
                    if (Dt10.Rows.Count > 0)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "COff Date", Str, String.Empty, 80, 150, 150, 80, 80, 80, 80);
                        if (Dr != null)
                        {
                            DtpCO.Text = Convert.ToString(Dr["Edate"]);
                            DtpCO.Enabled = false;
                            Txtdays.Text = Convert.ToString(Dr["Bal_Hrs"]);
                            button1.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Date....!", "Gainup");
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                    else if (Dt10.Rows.Count == 0)
                    {
                        MessageBox.Show("C-Off Date Not Available....!", "Gainup");
                        MyParent.Save_Error = true;
                        return;
                    }

                }
            }
        }

        private void DtpCo2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void DtpCo2_MouseEnter(object sender, EventArgs e)
        {

        }

        private void myTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.ActiveControl.Name == "myTextBox1")
                {
                    DataTable Dt10 = new DataTable();
                    Str = "Select Edate, Intime, OUTTIME, SHIFT, TOTAL_HRS,Bal_Hrs from Vaahini_Erp_Gainup.dbo.CO_Date(getdate()," + MyParent.Emplno + ") Where edate not in (select CoffDate from VAAHINI_ERP_GAINUP.dbo.COff_Request_Entry where emplno = " + MyParent.Emplno + ")";
                    MyBase.Load_Data(Str, ref Dt10);
                    if (Dt10.Rows.Count > 0)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "COff Date", Str, String.Empty, 150, 150);
                        if (Dr != null)
                        {
                            DtpCO.Text = Convert.ToString(Dr["Edate"]);
                            DtpCO.Enabled = false;
                            Txtdays.Text = Convert.ToString(Dr["Bal_Hrs"]);
                            button1.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Date....!", "Gainup");
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                    else if (Dt10.Rows.Count == 0)
                    {
                        MessageBox.Show("C-Off Date Not Available....!", "Gainup");
                        MyParent.Save_Error = true;
                        return;
                    }

                }
            }
        }

        private void myTextBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.ActiveControl.Name == "TxtCodate")
                {
                    DataTable Dt10 = new DataTable();
                    Str = "Select Edate, Intime, OUTTIME, SHIFT, TOTAL_HRS,Bal_Hrs from Vaahini_Erp_Gainup.dbo.CO_Date(getdate()," + MyParent.Emplno + ") Where edate not in (select CoffDate from VAAHINI_ERP_GAINUP.dbo.COff_Request_Entry where emplno = " + MyParent.Emplno + ")";
                    MyBase.Load_Data(Str, ref Dt10);
                    if (Dt10.Rows.Count > 0)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "COff Date", Str, String.Empty, 150, 150);
                        if (Dr != null)
                        {
                            DtpCO.Text = Convert.ToString(Dr["Edate"]);
                            DtpCO.Enabled = false;
                            Txtdays.Text = Convert.ToString(Dr["Bal_Hrs"]);
                            button1.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Date....!", "Gainup");
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                    else if (Dt10.Rows.Count == 0)
                    {
                        MessageBox.Show("C-Off Date Not Available....!", "Gainup");
                        MyParent.Save_Error = true;
                        return;
                    }

                }
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                if (this.ActiveControl.Name == "TxtCodate")
                {
                    DataTable Dt10 = new DataTable();
                    Str = "Select Edate, Intime, OUTTIME, SHIFT, TOTAL_HRS,Bal_Hrs from Vaahini_Erp_Gainup.dbo.CO_Date(Getdate()," + MyParent.Emplno + ") Where edate not in (select CoffDate from VAAHINI_ERP_GAINUP.dbo.COff_Request_Entry where emplno = " + MyParent.Emplno + ")";
                    MyBase.Load_Data(Str, ref Dt10);
                    if (Dt10.Rows.Count > 0)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "C-Off Date", Str, String.Empty, 150, 150);
                        if (Dr != null)
                        {
                            DtpCO.Text = Convert.ToString(Dr["Edate"]);
                            DtpCO.Enabled = false;
                            Txtdays.Text = Convert.ToString(Dr["Bal_Hrs"]);
                            button1.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Date....!", "Gainup");
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                    else if (Dt10.Rows.Count == 0)
                    {
                        MessageBox.Show("C-Off Date Not Available....!", "Gainup");
                        MyParent.Save_Error = true;
                        return;
                    }

                }
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == false)
            {
                checkBox4.Checked = true;
            }
            else
            {
                checkBox4.Checked = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                checkBox3.Checked = false;
            }
            else
            {
                checkBox3.Checked = true;
            }
        }

        private void myTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
             
    }
}