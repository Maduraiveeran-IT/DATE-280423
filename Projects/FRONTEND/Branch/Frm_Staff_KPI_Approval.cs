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
using System.Net.Mail;

namespace Accounts
{
    public partial class Frm_Staff_KPI_Approval : Form
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

        public Frm_Staff_KPI_Approval()
        {
            InitializeComponent();
        }
        void Total_Count()
        {
            try
            {
               TxtTotCount.Text = String.Format("{0}", Convert.ToInt16(MyBase.Count(ref Grid, "KPI_POINT")) ).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BtnExit_Click(object sender, EventArgs e)
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

        private void BtnApprove_Click(object sender, EventArgs e)
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

                if (MessageBox.Show("Sure to Approve ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
                else
                {
                    Grid.Refresh();
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

                        Queries[Array_Index++] = "Update Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY Set UOM_ID = " + Grid["UOM_ID", i].Value.ToString() + " , KPI_POINT = '" + Grid["KPI_POINT", i].Value.ToString() + "' , Target = '" + Grid["Target", i].Value.ToString() + "', Incharge_Point = '" + Grid["ACTUAL", i].Value.ToString() + "' , Remarks = '" + TxtRemarks.Text.ToString() + "'  , Approval = 'T' , Approval_At = Getdate()  where Rowid = " + Grid["Rowid", i].Value.ToString() + " ";

                    }
                    MyBase.Run_Identity(false, Queries);

                    DataTable MailId = new DataTable();
                    Str = "Select * from vaahini_erp_gainup.dbo.Employee_Mail_Id where Emplno = " + Txt_Tno.Tag + "";
                    MyBase.Load_Data(Str, ref MailId);

                    if (MailId.Rows.Count > 0)
                    {
                        SendFromGmail_MultiId("gainup.erp@gmail.com", "" + MailId.Rows[0]["Mail_Id"].ToString() + "", " ", " KPI APPROVAL FOR THE MONTH OF " + String.Format("{0:MMM-yyyy}", DtpFDate.Value) + "", " Dear Sir/Madam,  Your KPI Entry Was Approved By - " + Txt_AgName.Text.ToString() + " / " + Txt_AgDesignation.Text.ToString() + "  For the Month Of " + String.Format("{0:MMM-yyyy}", DtpFDate.Value) + ":  Remarks: " + TxtRemarks.Text.ToString() + "", false, "jqvayvskwgylhjgh", "");
                    }

                    MessageBox.Show("Approved ....!", "Gainup");
                    Dt = new DataTable();
                    Grid.DataSource = null;
                    DtpFDate.Enabled = true;
                    MyBase.Clear(this);
                    this.KeyPreview = true;
                    Txt_Tno.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                this.KeyPreview = true;
                Txt_Tno.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Frm_Staff_KPI_Approval_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "Txt_Tno" || this.ActiveControl.Name == "Txt_Name")
                    {
                        DtpFDate.Enabled = false;
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "EFFECT_FROM", "Select Distinct cast(datename(Month,A.Effect_From)as varchar(3))+'-'+ cast(datepart(year,A.Effect_From)as varchar(25)) EFFECTFROM , B.Name , A.Emplno , A.EFFECT_FROM , A.Incharge_Emplno from Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY A Left join Vaahini_erp_Gainup.Dbo.Employeemas B On A.Emplno = B.Emplno where Incharge_Emplno = " + MyParent.Emplno + " and Approval is null ", String.Empty, 100, 250);

                        if (Dr != null)
                        {

                            DtpFDate.Value = Convert.ToDateTime(Dr["EFFECT_FROM"]);

                            if (MyParent.Emplno > 0)
                            {

                                Str = "Select A.Tno, A.Name, A.Emplno, B.DeptName, C.DesignationName, A.Emplno, A.Deptcode, A.Designationcode from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.DeptType B on A.Deptcode=B.DeptCode left join VAAHINI_ERP_GAINUP.dbo.Designationtype C on A.designationcode = C.DesignationCode where A.tno not like '%Z' and A.EmplNO = " + Dr["Emplno"].ToString() + " ";
                                MyBase.Load_Data(Str, ref Dt2);
                                if (Dt2.Rows.Count > 0)
                                {
                                    Txt_Tno.Text = Dt2.Rows[0]["Tno"].ToString();
                                    Txt_Tno.Tag = Dt2.Rows[0]["Emplno"].ToString();
                                    Load_EMpl_Photo(Convert.ToInt64(Dt2.Rows[0]["Emplno"].ToString()));
                                    Txt_Name.Text = Dt2.Rows[0]["Name"].ToString();
                                    Txt_Name.BackColor = System.Drawing.Color.Yellow;
                                    Txt_Name.Tag = Dt2.Rows[0]["Emplno"].ToString();
                                    Txt_Dept.Text = Dt2.Rows[0]["DeptName"].ToString();
                                    Txt_Dept.Tag = Dt2.Rows[0]["Deptcode"].ToString();
                                    Txt_Designation.Text = Dt2.Rows[0]["DesignationName"].ToString();
                                    Txt_Designation.Tag = Dt2.Rows[0]["designationcode"].ToString();
                                }

                                Str = "Select emplno , name, deptname, designationname from VAAHINI_ERP_GAINUP.dbo.Employee_Details_A() where Emplno= " + Dr["Incharge_Emplno"].ToString() + "";
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

                        }
                        else
                        {
                            
                        }
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    BtnApprove.Focus();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Load_EMpl_Photo(Int64 Emplno)
        {
            DataTable TmpDt;
            String Str = String.Empty;
            try
            {
                TmpDt = new DataTable();
                Str = "Select * From VAAHINI_GAINUP_PHOTO.dbo.EMPLPHOTO Where Emplno = " + Emplno + " and type=1 ";
                MyBase.Load_Data(Str, ref TmpDt);
                if (TmpDt.Rows.Count > 0)
                {
                    if (TmpDt.Rows[0]["Photo"] != DBNull.Value)
                    {
                        Byte[] Data = (Byte[])TmpDt.Rows[0]["Photo"];
                        Image Ephoto1;
                        using (MemoryStream MS = new MemoryStream(Data, 0, Data.Length))
                        {
                            MS.Write(Data, 0, Data.Length);
                            Ephoto1 = Image.FromStream(MS, true);

                        }
                        EmplPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
                        EmplPhoto.Image = Ephoto1;
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
                MyBase.Row_Number(ref Grid);
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

        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["UOM"].Index)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "UOM", "Select Name UOM, Rowid From VAAHINI_ERP_GAINUP.dbo.KPI_UOM_Master", String.Empty, 100);
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
            //try
            //{
            //    MyBase.Row_Number(ref Grid);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                //if (Dt.Rows.Count != 0)
                //{
                //    MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentRow.Index);
                //}
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        void Grid_Data()
        {
            String Str2 = String.Empty;
            try
            {

                Str = "Select ROW_NUMBER() over(order by A.Rowid)  SLNO, A.KPI_POINT , A.[Target] , A.ACTUAL ,B.Name  UOM, A.Rowid , A.UOM_ID from Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY A  Left Join VAAHINI_ERP_GAINUP.dbo.KPI_UOM_Master B On A.UOM_ID = B.Rowid where A.Emplno = " + Txt_Tno.Tag + " and A.Incharge_Emplno = " + Txt_AgName.Tag + " and A.Effect_From = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' order by A.Rowid ";
               
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Rowid", "UOM_ID");
                MyBase.ReadOnly_Grid_Without(ref Grid, "KPI_POINT", "Target", "ACTUAL", "UOM");
                MyBase.Grid_Width(ref Grid, 50, 530, 120, 120);
                Grid.RowHeadersWidth = 10;
                Grid.CurrentCell = Grid["KPI_POINT", 0];
                Grid.Focus();
                Grid.BeginEdit(true);
                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Frm_Staff_KPI_Approval_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtRemarks")
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

        private void Frm_Staff_KPI_Approval_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                return;
                Txt_Tno.Focus();
                
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
                TxtRemarks.Focus();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Boolean SendFromGmail_MultiId(String FromID, String ToId, String CCID, String Subject, String Body, Boolean IsHtmlBody, String Password, params String[] AttachmentFilePath)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(FromID);
                                //foreach (var address in ToId.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                //{
                mail.To.Add(ToId);
                //}
                //mail.To.Add(ToId);

                if (CCID.Trim() != String.Empty)
                {
                    //foreach (var address in CCID.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    //{
                    mail.CC.Add(CCID);
                    //}
                    //mail.CC.Add(CCID);
                }
                mail.Subject = Subject;
                mail.IsBodyHtml = IsHtmlBody;
                mail.Body = Body;

                System.Net.Mail.Attachment attachment;
                foreach (String Str in AttachmentFilePath)
                {
                    if (File.Exists(Str))
                    {
                        attachment = new System.Net.Mail.Attachment(Str);
                        mail.Attachments.Add(attachment);
                    }
                }

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(FromID, Password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
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
                else if (TxtRemarks.Text.ToString().Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Remarks...!", "GainUp.....!");
                    MyParent.Save_Error = true;
                    TxtRemarks.Focus();
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

                if (MessageBox.Show("Sure to Reject...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
                else
                {
                    Grid.Refresh();
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
                        if (Convert.ToString(Grid["Target", i].Value) == String.Empty)
                        {
                            Grid["Target", i].Value = "-";
                        }
                        if (Convert.ToString(Grid["ACTUAL", i].Value) == String.Empty)
                        {
                            Grid["ACTUAL", i].Value = "-";
                        }

                        Queries[Array_Index++] = "Update Vaahini_erp_Gainup.Dbo.STAFF_KPI_POINT_ENTRY  Set  KPI_POINT = '" + Grid["KPI_POINT", i].Value.ToString() + "' , Target = '" + Grid["Target", i].Value.ToString() + "', Incharge_Point = '" + Grid["ACTUAL", i].Value.ToString() + "' , Remarks = '" + TxtRemarks.Text.ToString() + "'  , Approval = 'R' , Approval_At = Getdate()  where Rowid = " + Grid["Rowid", i].Value.ToString() + " ";

                    }
                    MyBase.Run_Identity(false, Queries);

                    DataTable MailId = new DataTable();
                    Str = "Select * from vaahini_erp_gainup.dbo.Employee_Mail_Id where Emplno = " + Txt_Tno.Tag + "";
                    MyBase.Load_Data(Str, ref MailId);

                    if (MailId.Rows.Count > 0)
                    {
                        SendFromGmail_MultiId("gainup.erp@gmail.com", "" + MailId.Rows[0]["Mail_Id"].ToString() + "", " ", " KPI REJECT FOR THE MONTH OF " + String.Format("{0:MMM-yyyy}", DtpFDate.Value) + "", " Dear Sir/Madam,  Your KPI Entry Was Rejected By - " + Txt_AgName.Text.ToString() + " / " + Txt_AgDesignation.Text.ToString() + "  For the Month Of " + String.Format("{0:MMM-yyyy}", DtpFDate.Value) + ":  Remarks: " + TxtRemarks.Text.ToString() + "", false, "jqvayvskwgylhjgh", "");
                    }
                    

                    
                    MessageBox.Show("Rejected ....!", "Gainup");
                    Dt = new DataTable();
                    Grid.DataSource = null;
                    DtpFDate.Enabled = true;
                    MyBase.Clear(this);
                    this.KeyPreview = true;
                    Txt_Tno.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}