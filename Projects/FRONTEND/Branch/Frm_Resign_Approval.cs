using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounts;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Threading;
using System.Diagnostics;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;

namespace Accounts
{
    public partial class Frm_Resign_Approval : Form
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        String Str;
        DataRow Dr;
        DateTime Fdate;
        DateTime TDate;
        Int32 Stat;
        DataTable Dt = new DataTable();
        DataTable Dt2 = new DataTable();
        DataTable TmpDt = new DataTable();
        Int32 Month;
        int Mode1;
        Int32 Year;
        Font Tamil = new Font("Baamini", 9, FontStyle.Bold);
        Font English = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);

        public Frm_Resign_Approval()
        {
            InitializeComponent();
            
        }

        private void Frm_Resign_Approval_Load(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                MyParent = (MDIMain)MdiParent;
                Txt_Name.Focus();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnApp_Click(object sender, EventArgs e)
        {
           try
            {

                if (MessageBox.Show("Sure to Approve the Records ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
                else
                {
                    String[] Queries;
                    Int32 AI = 0;

                    if (Txt_Tno.Text.ToString().Trim() == String.Empty)
                    {
                        MessageBox.Show("Invalid Employee...!", "Gainup");
                        Txt_Tno.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }

                    if (Txt_Reason.Text.ToString().Trim() == String.Empty)
                    {
                        MessageBox.Show("Reason Mandatory...!", "Gainup");
                        Txt_Reason.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    if (Txt_Floor_Hr_Rmk.Text.ToString().Trim() == String.Empty)
                    {
                        MessageBox.Show("Remarks Mandatory...!", "Gainup");
                        Txt_Reason.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    if (Txt_Floor_Hr_Rmk.Text.ToString().Trim() == String.Empty)
                    {
                        Txt_Floor_Hr_Rmk.Text = "-";
                    }
                    if (TxtHR_Remarks.Text.ToString().Trim() == String.Empty)
                    {
                        MessageBox.Show("Invalid Remarks...!", "Gainup");
                        TxtHR_Remarks.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    else
                    {

                        MyBase.Run("Update Vaahini_Erp_gainup.Dbo.Employee_Resign_Entry set Incharge_Remarks =   '" + TxtHR_Remarks.Text.ToString() + "' , Incharge_App = 1 , Incharge_Appdate = Getdate() where rowid = " + Txt_Name.Tag + "");
                       
                    }

                    MessageBox.Show("Approved...!", "Gainup");
                    MyBase.Clear(this);
                    Txt_Name.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnRej_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Sure to Reject the Records ...!", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
                else
                {

                    if (Txt_Tno.Text.ToString().Trim() == String.Empty)
                    {
                        MessageBox.Show("Invalid Employee...!", "Gainup");
                        Txt_Tno.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }

                    if (Txt_Reason.Text.ToString().Trim() == String.Empty)
                    {
                        MessageBox.Show("Reason Mandatory...!", "Gainup");
                        Txt_Reason.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    if (Txt_Floor_Hr_Rmk.Text.ToString().Trim() == String.Empty)
                    {
                        MessageBox.Show("Remarks Mandatory...!", "Gainup");
                        Txt_Reason.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    if (Txt_Floor_Hr_Rmk.Text.ToString().Trim() == String.Empty)
                    {
                        Txt_Floor_Hr_Rmk.Text = "-";
                    }
                    
                    if (TxtHR_Remarks.Text.ToString().Trim() == String.Empty)
                    {
                        MessageBox.Show("Invalid Remarks...!", "Gainup");
                        TxtHR_Remarks.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    else
                    {
                        if (Stat == 1)
                        {
                            MyBase.Run("Update Vaahini_Erp_gainup.Dbo.Employee_Resign_Entry set Incharge_Remarks =   '" + TxtHR_Remarks.Text.ToString() + "' , Incharge_App = 2 , Incharge_Appdate = Getdate() where rowid = " + Txt_Name.Tag + "");
                        }
                        else if (Stat == 2)
                        {
                            MyBase.Run("Update Vaahini_Erp_gainup.Dbo.Employee_Resign_Entry set HOD_Remarks =   '" + TxtHR_Remarks.Text.ToString() + "' , HOD_App = 2 , HOD_Appdate = Getdate() where rowid = " + Txt_Name.Tag + "");
                        }
                        else if (Stat == 3)
                        {
                            MyBase.Run("Update Vaahini_Erp_gainup.Dbo.Employee_Resign_Entry set Division_HOD_Remarks =   '" + TxtHR_Remarks.Text.ToString() + "' , Division_HOD_App = 2 , Division_HOD_Appdate = Getdate() where rowid = " + Txt_Name.Tag + "");
                        }
                    }

                    MessageBox.Show("Rejected...!", "Gainup");
                    MyBase.Clear(this);
                    Txt_Name.Focus();
                    return;
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
                try
                {
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Resign_Approval_KeyDown(object sender, KeyEventArgs e)
        {
            try
            
            {
                if (e.KeyCode == Keys.Down)
                {

                    if ((this.ActiveControl.Name == "Txt_Tno" || this.ActiveControl.Name == "Txt_Name"))
                    {
                        Str = "Select * from Vaahini_Erp_Gainup.Dbo.Job_Resign_View_Approval() where 1 = 1 ";
                        Str = Str + " And Incharge_no = " + MyParent.Emplno + " and isnull(Incharge_App,0) = 0  ";
                        
                    }
                    Dr = Tool.Selection_Tool(this, 300, 300, SelectionTool_Class.ViewType.NormalView, "Select Employee...!", Str, String.Empty, 90, 180, 100, 100, 100, 100, 120, 100, 100);
                    if (Dr != null)
                    {

                        DtpEDate.Value = Convert.ToDateTime(Dr["RequestDate"].ToString());
                        
                        Txt_Tno.Text = Dr["tno"].ToString();
                        Txt_Tno.Tag = Dr["Emplno"].ToString();
                        Txt_Name.Text = Dr["name"].ToString();
                        Txt_Dept.Text = Dr["Deptname"].ToString();
                        Txt_Designation.Text = Dr["Designationname"].ToString();
                        Txt_AgName.Text = Dr["Enter_by"].ToString();
                        Txt_Reason.Text = Dr["Reason"].ToString();
                        Txt_Reason.Tag = Dr["Reason_ID"].ToString();
                        Txt_Floor_Hr_Rmk.Text = Dr["Remarks"].ToString();
                        Txt_Name.Tag = Dr["Rowid"].ToString();
                        Retrieve_Image();
                        TxtHR_Remarks.Focus();
                        return;
                    }
                   
                }
                else
                {
                    SendKeys.Send("{TAB}");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Retrieve_Image()
        {
            try
            {

                TmpDt = new DataTable();
                Str = "Select * from VAAHINI_GAINUP_PHOTO.dbo.Resign_Letterphoto Where R_Master_Id = " + Txt_Name.Tag + " and type1 = 1 ";
                MyBase.Load_Data(Str, ref TmpDt);
                if (TmpDt.Rows.Count > 0)
                {
                    if (TmpDt.Rows[0]["Photo"] != DBNull.Value)
                    {
                        Byte[] Data = (Byte[])TmpDt.Rows[0]["Photo"];
                        Image Ephoto;
                        using (MemoryStream MS = new MemoryStream(Data, 0, Data.Length))
                        {
                            MS.Write(Data, 0, Data.Length);
                            Ephoto = Image.FromStream(MS, true);
                        }
                        Letterphoto.SizeMode = PictureBoxSizeMode.StretchImage;
                        Letterphoto.Image = Ephoto;
                    }
                }
                TmpDt = new DataTable();
                Str = "Select * from VAAHINI_GAINUP_PHOTO.dbo.EMPLPHOTO Where Emplno = " + Txt_Tno.Tag + " and type=1 ";
                MyBase.Load_Data(Str, ref TmpDt);
                if (TmpDt.Rows.Count > 0)
                {
                    if (TmpDt.Rows[0]["Photo"] != DBNull.Value)
                    {
                        Byte[] Data = (Byte[])TmpDt.Rows[0]["Photo"];
                        Image Ephoto;
                        using (MemoryStream MS = new MemoryStream(Data, 0, Data.Length))
                        {
                            MS.Write(Data, 0, Data.Length);
                            Ephoto = Image.FromStream(MS, true);
                        }
                        EmplPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
                        EmplPhoto.Image = Ephoto;
                    }
                }
                else
                {
                    TmpDt = new DataTable();
                    Str = "Select * from VAAHINI_GAINUP_PHOTO.dbo.EmplPhoto_Deleted where Emplno =" + Txt_Tno.Tag + " and type=1 ";
                    MyBase.Load_Data(Str, ref TmpDt);
                    if (TmpDt.Rows.Count > 0)
                    {
                        if (TmpDt.Rows[0]["Photo"] != DBNull.Value)
                        {
                            Byte[] Data = (Byte[])TmpDt.Rows[0]["Photo"];
                            Image Ephoto;
                            using (MemoryStream MS = new MemoryStream(Data, 0, Data.Length))
                            {
                                MS.Write(Data, 0, Data.Length);
                                Ephoto = Image.FromStream(MS, true);
                            }
                            EmplPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
                            EmplPhoto.Image = Ephoto;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Image_viewer(PictureBox Boxname)
        {
            try
            {
                Image NewImage = Boxname.Image;
                Bitmap NewImage1 = new Bitmap(NewImage, new Size(850, 1040));
                Image B = (Image)NewImage1;
                FileInfo Fi = new FileInfo(Application.StartupPath + "\\Test.bat");
                FileInfo F1 = new FileInfo(Application.StartupPath + "\\V.Bmp");
                if (Fi.Exists == true)
                {
                    Fi.Delete();
                    F1.Delete();
                    StreamWriter Wr = new StreamWriter(Application.StartupPath + "\\Test.Bat");
                    B.Save(Application.StartupPath.ToString() + "\\V.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                    Wr.WriteLine("Start " + Application.StartupPath + "\\V.Bmp");
                    Wr.Close();
                    System.Diagnostics.Process.Start(Application.StartupPath + "\\Test.bat");
                }
                else
                {
                    StreamWriter Wr = new StreamWriter(Application.StartupPath + "\\Test.Bat");
                    B.Save(Application.StartupPath.ToString() + "\\V.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                    Wr.WriteLine("Start " + Application.StartupPath + "\\V.bmp");
                    Wr.Close();
                    System.Diagnostics.Process.Start(Application.StartupPath + "\\Test.bat");
                }
                //F1.Delete();
                //Fi.Delete();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Letterphoto_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Letterphoto.Image != Letterphoto.InitialImage)
                {
                    Image_viewer(Letterphoto);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Resign_Approval_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtHR_Remarks")
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

       
    }
}