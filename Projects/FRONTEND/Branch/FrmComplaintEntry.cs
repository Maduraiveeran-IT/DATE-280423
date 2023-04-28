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
using System.Net;

namespace Accounts
{
    public partial class FrmComplaintEntry : Form
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataTable Dt2 = new DataTable();
        DataTable Dts = new DataTable();
        DataTable Dtn = new DataTable();
        DataRow Dr;
        TextBox Txt = null;
        String Str;
        String Str1;
        DataTable TmpDt = new DataTable();
        Int64 Code = 0;
        String[] Queries_New, Queries;
        Int32 Array_Index = 0;
        Font Tamil = new Font("Baamini", 9, FontStyle.Bold);
        Font English = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
        String PhoneNumber;
        String SmsStatus = "NS";
        String Message;
        String Name;
        String TName;

        public FrmComplaintEntry()
        {
            InitializeComponent();
        }

        public Boolean SendSMS(String Mobile, String SMS_Text)
        {
            try
            {
                DataTable Sms_Dt = new DataTable();
                String Stn = String.Empty;
                Stn = "Select Top 1 * From VAAHINI_ERP_GAINUP.dbo.SmsUrl_Link Where Active = 'Y' Order BY EntryDate Desc";
                MyBase.Load_Data(Stn, ref Sms_Dt);

                if (Sms_Dt.Rows.Count > 0)
                {
                    String text = String.Empty;
                    String Url_Link = Sms_Dt.Rows[0]["Url"].ToString() + "&" + Sms_Dt.Rows[0]["Argument1"].ToString() + "=" + Mobile + "&" + Sms_Dt.Rows[0]["Argument2"].ToString() + "=" + SMS_Text.Replace(" ", "%20");
                    StreamWriter myWriter = null;
                    HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(Url_Link);
                    objRequest.Method = "POST";
                    objRequest.ContentLength = Encoding.UTF8.GetByteCount(Url_Link);
                    objRequest.ContentType = "application/x-www-form-urlencoded";
                    try
                    {
                        myWriter = new StreamWriter(objRequest.GetRequestStream());
                        myWriter.Write(Url_Link);
                    }
                    catch (Exception e)
                    {

                        return false;
                    }
                    finally
                    {
                        myWriter.Close();
                    }

                    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                    {
                        text = sr.ReadToEnd();
                        // Close and clean up the StreamReader
                        sr.Close();
                    }

                    //Number of valid mobile numbers are : 1
                    //Numberofmessages of messages are : 1
                    //The messages has been sent

                    if (text.ToUpper().Contains(Sms_Dt.Rows[0]["Success_MSg"].ToString().ToUpper()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Boolean Check_Net_Conneciton()
        {
            try
            {
                bool Connection = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                if (Connection == true)
                {
                    DataTable DtChk = new DataTable();
                    MyBase.Load_Data("Select Top 1 * From VAAHINI_ERP_GAINUP.dbo.Enable_Sms_Send Where Flag='Y'", ref DtChk);
                    if (DtChk.Rows.Count > 0)
                    {
                        String Link = "http://www.google.com";

                        StreamWriter myWriter = null;
                        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(Link);
                        objRequest.Method = "POST";
                        objRequest.ContentLength = Encoding.UTF8.GetByteCount(Link);
                        objRequest.ContentType = "application/x-www-form-urlencoded";
                        try
                        {
                            myWriter = new StreamWriter(objRequest.GetRequestStream());
                            myWriter.Write(Link);
                            return true;
                        }
                        catch (Exception e)
                        {


                            return false;
                        }
                        finally
                        {
                            myWriter.Close();
                        }
                    }
                    else
                    {

                        return false;

                    }

                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {

                return false;

            }
        }
        public void Entry_Save()
        {
            try
            {
                Queries_New = new String[Dt.Rows.Count + 5 * 5];
                Array_Index = 0;
                String F="E";

                if (RBTamil.Checked == true)
                {
                    F = "T";
                }
                
                DataTable TDt1 = new DataTable();
                MyBase.Load_Data("Select Max(IsNull(EntryNo,0))+1 Entry_No From VAAHINI_ERP_GAINUP.dbo.Complaint_Master", ref TDt1);
                if (TDt1.Rows[0][0].ToString() != String.Empty)
                {
                    Txt_Entryno.Text = TDt1.Rows[0][0].ToString();
                }
                else
                {
                    Txt_Entryno.Text = "1";
                }

                Queries_New[Array_Index++] = "insert into VAAHINI_ERP_GAINUP.dbo.Complaint_Master(EntryNo,Emplno,Remarks, User_Code, FontMode, To_Emplno, Complete_Status, Module) values (" + Txt_Entryno.Text.ToString() + ", " + Txt_Name.Tag + ", '" + Txt_Description.Text.ToString() + "', " + MyParent.UserCode + ", '" + F + "', " + Txt_AgName.Tag + ", '" + TxtStatus.Text + "', '" + "Payroll" + "');Select Scope_Identity()";
                Queries_New[Array_Index++] = "insert into VAAHINI_ERP_GAINUP.dbo.Complaint_Details(Master_id,CmpRsn_Master_Id,Description)values(@@IDENTITY," + Txt_Reason.Tag + ",'-')";                                        
                MyBase.Run_Identity(false, Queries_New);

                if (Txt_AgName.Tag.ToString().Trim() != String.Empty)
                {
                    Str1 = "Select Len(Case When cell like '%,%' Then Vaahini_erp_gainup.dbo.STRING_SPLIT_Fn(cell,',') Else RTRIM(LTRIM(cell)) end) Length,(Case When cell like '%,%' Then Vaahini_erp_gainup.dbo.STRING_SPLIT_Fn(cell,',') Else RTRIM(LTRIM(cell)) end) Phone,Name from Vaahini_erp_Gainup.dbo.employeemas where emplno = " + Txt_AgName.Tag + "";
                   
                    MyBase.Load_Data(Str1, ref Dts);
                    
                    

                    if (Convert.ToInt64(Dts.Rows[0]["Length"].ToString()) > 0)
                    {
                        PhoneNumber = Dts.Rows[0]["Phone"].ToString();
                        Name = Dts.Rows[0]["Name"].ToString();

                        Message = "Dear ,\n" + Name + ",\nYou Have New Complaint Request From " + Txt_Name.Text.ToString() + "\nThank You....".Replace("\n", Environment.NewLine);


                        if (Check_Net_Conneciton())
                        {

                            if (SendSMS(PhoneNumber, Message))
                            {
                                SmsStatus = "Y";
                            }

                            MyBase.Run("Insert into Complaint_SMS(FEmplno,TEmplno,Mobile,Sms_Status,Massages,Smsdate,System)values(" + Txt_Name.Tag + "," + Txt_AgName.Tag + ",'" + PhoneNumber + "','" + SmsStatus + "','" + Message + "',Getdate(),host_Name())");
                        }
                        else
                        {
                            MyBase.Run("Insert into Complaint_SMS(FEmplno,TEmplno,Mobile,Sms_Status,Massages)values(" + Txt_Name.Tag + "," + Txt_AgName.Tag + ",'" + PhoneNumber + "','" + SmsStatus + "','" + Message + "')");
                        }

                    }
                }


                MessageBox.Show("Successfully Saved ..!", "Gainup");
                MyParent.Save_Error = false;
                MyBase.Clear(this);

                DataTable Dt2 = new DataTable();
                Str = "Select A.Tno, A.Name, B.DeptName, C.DesignationName, A.Emplno, A.Deptcode, A.designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.tno not like '%Z' and A.EmplNO = " + MyParent.Emplno + " ";
                MyBase.Load_Data(Str, ref Dt2);

                if (Dt2.Rows.Count > 0)
                {
                    Txt_Tno.Text = Dt2.Rows[0]["Tno"].ToString();
                    Txt_Name.Text = Dt2.Rows[0]["Name"].ToString();
                    Txt_Name.Tag = Dt2.Rows[0]["Emplno"].ToString();
                    Txt_Dept.Text = Dt2.Rows[0]["DeptName"].ToString();
                    Txt_Dept.Tag = Dt2.Rows[0]["Deptcode"].ToString();
                    Txt_Designation.Text = Dt2.Rows[0]["DesignationName"].ToString();
                    Txt_Designation.Tag = Dt2.Rows[0]["designationcode"].ToString();
                    button1.Enabled = true;
                    TxtStatus.Enabled = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton2.Checked = true;
                    Txt_Reason.Focus();
                }
                else
                {
                    Txt_Tno.Focus();
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
   

   
        public void Fill_Datas()
        {
            try
            {
                Code = Convert.ToInt64(Dr["Rowid"].ToString());
                DtpDate.Value = Convert.ToDateTime(Dr["EnrtyDate"]);
                Txt_Entryno.Text = Dr["EntryNo"].ToString();
                Txt_Dept.Text = Dr["DeptName"].ToString();
                Txt_Description.Text = Dr["Remarks"].ToString();
                Txt_Designation.Text = Dr["DesignationName"].ToString();
                Txt_Tno.Text = Dr["tno"].ToString();
                Txt_Name.Text = Dr["Name"].ToString();
                Txt_Name.Tag = Dr["Emplno"].ToString();
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmComplaintEntry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                DataTable Dt2 = new DataTable();
                Str = "Select A.Tno, A.Name, B.DeptName, C.DesignationName, A.Emplno, A.Deptcode, A.designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.tno not like '%Z' and A.EmplNO = " + MyParent.Emplno + " ";
                MyBase.Load_Data(Str, ref Dt2);
                
                if (Dt2.Rows.Count > 0)
                {
                    Txt_Tno.Text = Dt2.Rows[0]["Tno"].ToString();
                    Txt_Name.Text = Dt2.Rows[0]["Name"].ToString();
                    Txt_Name.Tag = Dt2.Rows[0]["Emplno"].ToString();
                    Txt_Dept.Text = Dt2.Rows[0]["DeptName"].ToString();
                    Txt_Dept.Tag = Dt2.Rows[0]["Deptcode"].ToString();
                    Txt_Designation.Text = Dt2.Rows[0]["DesignationName"].ToString();
                    Txt_Designation.Tag = Dt2.Rows[0]["designationcode"].ToString();
                    button1.Enabled = true;
                    TxtStatus.Enabled = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton2.Checked = true;
                    Txt_Reason.Focus();
                }
                else
                {
                    Txt_Tno.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void FrmComplaintEntry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "Txt_Tno")
                    {
                        Txt_Reason.Focus();
                    }
                    else if (this.ActiveControl.Name != "Txt_Description")
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    //if (this.ActiveControl.Name == "Txt_Tno")
                    //{
                    //    DataTable Dt2 = new DataTable();
                    //    //if (Convert.ToInt64(MyParent.UserCode.ToString()) <170)
                    //    //{
                    //    //    Str = "select A.Tno,A.Name,B.DeptName,C.DesignationName,A.Emplno,A.Deptcode,A.designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.tno not like '%Z'";
                    //    //}
                    //    //else
                    //    //{
                    //    Str = "select A.Name,A.Tno,B.DeptName,C.DesignationName,A.Emplno,A.Deptcode,A.designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.catcode=6 and A.tno not like '%Z' ";
                    //    //}
                    //    Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Employee", Str, String.Empty, 150, 90, 120, 150);

                    //    if (Dr != null)
                    //    {
                    //        Txt_Tno.Text = Dr["Tno"].ToString();
                    //        Txt_Name.Text = Dr["Name"].ToString();
                    //        Txt_Name.Tag = Dr["Emplno"].ToString();
                    //        Txt_Dept.Text = Dr["DeptName"].ToString();
                    //        Txt_Dept.Tag = Dr["Deptcode"].ToString();
                    //        Txt_Designation.Text = Dr["DesignationName"].ToString();
                    //        Txt_Designation.Tag = Dr["designationcode"].ToString();
                    //        TxtStatus.Enabled = false;
                    //        radioButton1.Enabled = false;
                    //        radioButton2.Enabled = false;
                    //        radioButton2.Checked = true;
                    //    }
                    //}
                    //else 
                    if (this.ActiveControl.Name == "Txt_AgTno")
                    {
                        if (Txt_Reason.Text.ToString() != String.Empty)
                        {
                            Str = "select A.Name,A.Tno,B.DeptName,C.DesignationName,A.Emplno,A.Deptcode,A.designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.catcode = 6 and A.tno not like '%Z' and A.Emplno<> " + Txt_Name.Tag + " and A.Emplno in(" + TxtResEmplno.Text + ") ";
                            
                            Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Responsible Employee", Str, String.Empty, 150, 90, 120, 200);

                            if (Dr != null)
                            {
                                Txt_AgTno.Text = Dr["Tno"].ToString();
                                Txt_AgName.Text = Dr["Name"].ToString();
                                Txt_AgName.Tag = Dr["Emplno"].ToString();
                                Txt_AgDept.Text = Dr["DeptName"].ToString();
                                Txt_AgDept.Tag = Dr["Deptcode"].ToString();
                                Txt_AgDesignation.Text = Dr["DesignationName"].ToString();
                                Txt_AgDesignation.Tag = Dr["designationcode"].ToString();
                                //Txt_Reason.Enabled = false;
                                Txt_Description.Focus();
                            }
                        }
                        else
                        {
                            Txt_Reason.Focus();
                        }
                    }
                    else if (this.ActiveControl.Name == "Txt_Reason")
                    {
                        if (Txt_Tno.Text.ToString() != String.Empty)
                        {
                            Str = "select Reason,Rowid,Responsible_Emplno from VAAHINI_ERP_GAINUP.dbo.Complaint_Reason_Master where Responsible_Emplno <> '" + Txt_Name.Tag + "'";
                            Dr = Tool.Selection_Tool_Except_New("REASON", this, 150, 150, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Reason", Str, String.Empty, 150);
                            if (Dr != null)
                            {
                                Txt_Reason.Text = Dr["Reason"].ToString();
                                Txt_Reason.Tag = Dr["Rowid"].ToString();
                                TxtResEmplno.Text = Dr["Responsible_Emplno"].ToString();

                                DataTable Tdt = new DataTable();
                                //Str = "select A.Name,A.Tno,B.DeptName,C.DesignationName,A.Emplno,A.Deptcode,A.designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.catcode=6 and A.tno not like '%Z' and A.Emplno<> " + Txt_Name.Tag + " and A.Emplno in(" + TxtResEmplno.Text + ") and A.Emplno not in (412,1627)";
                                Str = "select A.Name,A.Tno,B.DeptName,C.DesignationName,A.Emplno,A.Deptcode,A.designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.catcode in(5,6) and A.tno not like '%Z' and A.Emplno<> " + Txt_Name.Tag + " and A.Emplno in(" + TxtResEmplno.Text + ")";
                                MyBase.Load_Data(Str, ref Tdt);

                                if (Tdt.Rows.Count > 0)
                                {
                                    if (Convert.ToInt16(Tdt.Rows[0]["Emplno"]) == 412 || Convert.ToInt16(Tdt.Rows[0]["Emplno"]) == 1627)
                                    {
                                        Txt_AgTno.Enabled = true;
                                        Txt_AgTno.Text = "";
                                        Txt_AgName.Text = "";
                                        Txt_AgName.Tag = "";
                                        Txt_AgDept.Text = "";
                                        Txt_AgDept.Tag = "";
                                        Txt_AgDesignation.Text = "";
                                        Txt_AgDesignation.Tag = "";
                                        Txt_Description.Text = "";
                                        Txt_AgTno.Focus();
                                    }
                                    else
                                    {
                                        Txt_AgTno.Text = Tdt.Rows[0]["Tno"].ToString();
                                        Txt_AgName.Text = Tdt.Rows[0]["Name"].ToString();
                                        Txt_AgName.Tag = Tdt.Rows[0]["Emplno"].ToString();
                                        Txt_AgDept.Text = Tdt.Rows[0]["DeptName"].ToString();
                                        Txt_AgDept.Tag = Tdt.Rows[0]["Deptcode"].ToString();
                                        Txt_AgDesignation.Text = Tdt.Rows[0]["DesignationName"].ToString();
                                        Txt_AgDesignation.Tag = Tdt.Rows[0]["designationcode"].ToString();
                                        //Txt_Reason.Enabled = false;
                                        Txt_AgTno.Enabled = false;
                                        Txt_Description.Text = "";
                                        Txt_Description.Focus();
                                    }
                                }
                            }
                        }
                        else
                        {
                            Txt_Tno.Focus();
                        }
                    }
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

        private void FrmComplaintEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "Txt_Description" || this.ActiveControl.Name == "TxtStatus")
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
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
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

                if (Txt_AgName.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Responsible Name", "Gainup");
                    Txt_AgTno.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
               
                if (Txt_Description.Text.ToString().Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Remarks", "Gainup");
                    Txt_Description.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                DialogResult m = MessageBox.Show("Sure to Save...!", "Complaint Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (m == DialogResult.Yes)
                {
                    Entry_Save();                                       
                }
                if (m == DialogResult.No)
                {
                    Txt_Tno.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                button1.Enabled = true;
                Txt_Tno.Enabled = true;
                Txt_Reason.Enabled = true;
                Txt_AgTno.Enabled = true;
                Txt_Description.Enabled = true;
                TxtStatus.Enabled = false;
                button4.Text = "VIEW";
                DataTable Dt2 = new DataTable();
                Str = "Select A.Tno, A.Name, B.DeptName, C.DesignationName, A.Emplno, A.Deptcode, A.designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.tno not like '%Z' and A.EmplNO = " + MyParent.Emplno + " ";
                MyBase.Load_Data(Str, ref Dt2);

                if (Dt2.Rows.Count > 0)
                {
                    Txt_Tno.Text = Dt2.Rows[0]["Tno"].ToString();
                    Txt_Name.Text = Dt2.Rows[0]["Name"].ToString();
                    Txt_Name.Tag = Dt2.Rows[0]["Emplno"].ToString();
                    Txt_Dept.Text = Dt2.Rows[0]["DeptName"].ToString();
                    Txt_Dept.Tag = Dt2.Rows[0]["Deptcode"].ToString();
                    Txt_Designation.Text = Dt2.Rows[0]["DesignationName"].ToString();
                    Txt_Designation.Tag = Dt2.Rows[0]["designationcode"].ToString();
                    button1.Enabled = true;
                    TxtStatus.Enabled = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton2.Checked = true;
                    Txt_Reason.Focus();
                }
                else
                {
                    Txt_Tno.Focus();
                }
                Txt_Tno.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                String Comp_Flag=String.Empty;

                if (button4.Text == "UPDATE")
                {
                    if (TxtStatus.Text.ToString().Trim() == String.Empty)
                    {
                        MessageBox.Show("Invalid Complaint Status", "Gainup");
                        TxtStatus.Focus();
                        return;
                    }

                    if(radioButton1.Checked==true)
                    {
                        Comp_Flag = "Y";
                    }
                    else
                    {
                        Comp_Flag = "N";
                    }

                    DialogResult m = MessageBox.Show("Sure to Update...!", "Complaint Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (m == DialogResult.Yes)
                    {
                        MyBase.Run("Update VAAHINI_ERP_GAINUP.dbo.Complaint_Master set Complete_Status = '" + TxtStatus.Text + "', Complete_Flag= '" + Comp_Flag + "' ,Completed_DateTime=getdate() where EntryNo = " + Txt_Entryno.Text + " and To_Emplno= " + MyParent.Emplno + " ");
                        MessageBox.Show("Successfully Updated ..!", "Gainup");
                        MyParent.Save_Error = false;
                        MyBase.Clear(this);
                        button1.Enabled = true;
                        Txt_Tno.Enabled = true;
                        Txt_Reason.Enabled = true;
                        Txt_AgTno.Enabled = true;
                        Txt_Description.Enabled = true;
                        button4.Text = "VIEW";
                        Txt_Tno.Focus();
                        MyBase.Enable_Controls(this, true);
                    }
                    if (m == DialogResult.No)
                    {
                        Txt_Tno.Focus();
                    }
                }
                else
                {
                    MyBase.Clear(this);
                    if (MyParent.UserName.ToString() == "MD")
                    {
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Complaint ", "Select A.EntryNo, A.EntryDate, D.Name, D.TNo, F.DeptName, C.Reason Complaint, (case when A.Complete_Flag= 'Y' then 'YES' Else 'PENDING' End) Closed, B.Description, A.Remarks, H.Name Ag_Name, H.TNo Ag_TNo, J.DeptName Ag_DeptName, K.DesignationName Ag_DesignationName, A.Complete_Status, A.Emplno, A.To_Emplno, B.Master_Id, B.CmpRsn_Master_Id, A.Emplno, D.Deptcode, D.designationcode, D.catcode, G.DesignationName, A.FontMode, A.Complete_Flag, IsNull(A.Completed_DateTime,getdate()) Completed_DateTime  FRom VAAHINI_ERP_GAINUP.dbo.Complaint_Master A left join VAAHINI_ERP_GAINUP.dbo.Complaint_Details B On A.Rowid = B.Master_Id left Join VAAHINI_ERP_GAINUP.dbo.Complaint_Reason_Master C On B.CmpRsn_Master_Id = C.Rowid left Join VAAHINI_ERP_GAINUP.dbo.Employeemas D On A.Emplno = D.Emplno left Join VAAHINI_ERP_GAINUP.dbo.Category E On D.CatCode = E.CatCode left Join VAAHINI_ERP_GAINUP.dbo.DeptType F On D.DeptCode = F.DeptCode and D.CompCode = F.CompCode left Join VAAHINI_ERP_GAINUP.dbo.DesignationType G On D.DesignationCode = G.DesignationCode and D.CompCode = G.CompCode left Join VAAHINI_ERP_GAINUP.dbo.Employeemas H On A.To_Emplno = H.Emplno left Join VAAHINI_ERP_GAINUP.dbo.Category I On H.CatCode = I.CatCode left Join VAAHINI_ERP_GAINUP.dbo.DeptType J On H.DeptCode = J.DeptCode and H.CompCode = J.CompCode left Join VAAHINI_ERP_GAINUP.dbo.DesignationType K On H.DesignationCode = K.DesignationCode and H.CompCode = K.CompCode Order by A.EntryDate  Desc", String.Empty, 80, 100, 120, 120, 120, 120, 100);
                    }
                    else
                    {
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Complaint ", "Select A.EntryNo, A.EntryDate, D.Name, D.TNo, F.DeptName, C.Reason Complaint, (case when A.Complete_Flag= 'Y' then 'YES' Else 'PENDING' End) Closed, B.Description, A.Remarks, H.Name Ag_Name, H.TNo Ag_TNo, J.DeptName Ag_DeptName, K.DesignationName Ag_DesignationName, A.Complete_Status, A.Emplno, A.To_Emplno, B.Master_Id, B.CmpRsn_Master_Id, A.Emplno, D.Deptcode, D.designationcode, D.catcode, G.DesignationName, A.FontMode, A.Complete_Flag,  IsNull(A.Completed_DateTime,getdate()) Completed_DateTime FRom VAAHINI_ERP_GAINUP.dbo.Complaint_Master A left join VAAHINI_ERP_GAINUP.dbo.Complaint_Details B On A.Rowid = B.Master_Id left Join VAAHINI_ERP_GAINUP.dbo.Complaint_Reason_Master C On B.CmpRsn_Master_Id = C.Rowid left Join VAAHINI_ERP_GAINUP.dbo.Employeemas D On A.Emplno = D.Emplno left Join VAAHINI_ERP_GAINUP.dbo.Category E On D.CatCode = E.CatCode left Join VAAHINI_ERP_GAINUP.dbo.DeptType F On D.DeptCode = F.DeptCode and D.CompCode = F.CompCode left Join VAAHINI_ERP_GAINUP.dbo.DesignationType G On D.DesignationCode = G.DesignationCode and D.CompCode = G.CompCode left Join VAAHINI_ERP_GAINUP.dbo.Employeemas H On A.To_Emplno = H.Emplno left Join VAAHINI_ERP_GAINUP.dbo.Category I On H.CatCode = I.CatCode left Join VAAHINI_ERP_GAINUP.dbo.DeptType J On H.DeptCode = J.DeptCode and H.CompCode = J.CompCode left Join VAAHINI_ERP_GAINUP.dbo.DesignationType K On H.DesignationCode = K.DesignationCode and H.CompCode = K.CompCode Where (A.EmplNo = " + MyParent.Emplno + ") or (A.To_Emplno= " + MyParent.Emplno + ") Order by A.EntryDate  Desc", String.Empty, 80, 100, 120, 120, 120, 120, 100);
                    }
                    if (Dr != null)
                    {
                        MyBase.Enable_Controls(this, false);
                        Txt_Entryno.Text = Dr["EntryNo"].ToString();
                        DtpDate.Value = Convert.ToDateTime(Dr["EntryDate"].ToString());
                        Txt_Name.Text = Dr["Name"].ToString();
                        Txt_Name.Tag = Dr["Emplno"].ToString();
                        Txt_Tno.Text = Dr["TNo"].ToString();
                        Txt_Dept.Text = Dr["DeptName"].ToString();
                        Txt_Designation.Text = Dr["DesignationName"].ToString();
                        Txt_Description.Text = Dr["Remarks"].ToString();
                        Txt_Reason.Text = Dr["Complaint"].ToString();
                        Txt_AgName.Text = Dr["Ag_Name"].ToString();
                        Txt_AgName.Tag = Dr["To_Emplno"].ToString();
                        Txt_AgTno.Text = Dr["Ag_TNo"].ToString();
                        Txt_AgDept.Text = Dr["Ag_DeptName"].ToString();
                        Txt_AgDesignation.Text = Dr["Ag_DesignationName"].ToString();
                        TxtStatus.Text = Dr["Complete_Status"].ToString();
                        if (Dr["FontMode"].ToString() == "T")
                        {
                            RBTamil.Checked = true;
                        }
                        else
                        {
                            RBEnglish.Checked = true;
                        }
                        button1.Enabled = false;
                        if (Dr["Complete_Flag"].ToString() == "Y")
                        {
                            radioButton1.Checked = true;
                        }
                        else
                        {
                            radioButton2.Checked = true;
                        }
                        DtpCDate.Value = Convert.ToDateTime(Dr["Completed_DateTime"].ToString());
                        DataTable Tdt = new DataTable();
                        Str = "Select * from VAAHINI_ERP_GAINUP.dbo.Complaint_Master where Complete_Flag='N' and EntryNo = " + Txt_Entryno.Text + " and To_Emplno= " + MyParent.Emplno + "";
                        MyBase.Load_Data(Str, ref Tdt);

                        if (Tdt.Rows.Count > 0)
                        {
                            Txt_Tno.Enabled = false;
                            Txt_Reason.Enabled = false;
                            Txt_AgTno.Enabled = false;
                            Txt_Description.Enabled = false;
                            TxtStatus.Enabled = true;
                            radioButton1.Enabled = true;
                            radioButton2.Enabled = true;
                            radioButton2.Checked = true;
                            button4.Text = "UPDATE";
                            TxtStatus.Focus();
                        }
                        else
                        {
                            radioButton1.Enabled = false;
                            radioButton2.Enabled = false;
                        }
                    }
                    else
                    {
                        DataTable Dt2 = new DataTable();
                        Str = "Select A.Tno, A.Name, B.DeptName, C.DesignationName, A.Emplno, A.Deptcode, A.designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.tno not like '%Z' and A.EmplNO = " + MyParent.Emplno + " ";
                        MyBase.Load_Data(Str, ref Dt2);

                        if (Dt2.Rows.Count > 0)
                        {
                            Txt_Tno.Text = Dt2.Rows[0]["Tno"].ToString();
                            Txt_Name.Text = Dt2.Rows[0]["Name"].ToString();
                            Txt_Name.Tag = Dt2.Rows[0]["Emplno"].ToString();
                            Txt_Dept.Text = Dt2.Rows[0]["DeptName"].ToString();
                            Txt_Dept.Tag = Dt2.Rows[0]["Deptcode"].ToString();
                            Txt_Designation.Text = Dt2.Rows[0]["DesignationName"].ToString();
                            Txt_Designation.Tag = Dt2.Rows[0]["designationcode"].ToString();
                            button1.Enabled = true;
                            TxtStatus.Enabled = false;
                            radioButton1.Enabled = false;
                            radioButton2.Enabled = false;
                            radioButton2.Checked = true;
                            Txt_Reason.Focus();
                        }
                        else
                        {
                            Txt_Tno.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RBTamil_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (RBTamil.Checked == true)
                {
                    Txt_Description.Font = Tamil;
                }
                else
                {
                    Txt_Description.Font = English;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void RBEnglish_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (RBTamil.Checked == true)
                {
                    Txt_Description.Font = Tamil;
                }
                else
                {
                    Txt_Description.Font = English;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                MyParent.View_Browser("MIS_Complaint_Employee",MyParent.Emplno);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
