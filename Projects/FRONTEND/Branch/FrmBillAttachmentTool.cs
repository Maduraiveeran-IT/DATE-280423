using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Accounts_ControlModules;
using System.Security.Principal;
using System.IO;
using System.Collections;

namespace Accounts
{
    public partial class FrmBillAttachmentTool : Form
    {
        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        Image image1,image2,image3,image4,image5;
        String Refno = String.Empty;
        DateTime RefDate;
        String CompCode = String.Empty;
        String Division = String.Empty;
        String LedgerCode = String.Empty;
        String FolderName = String.Empty;
        Form FormName = null;Boolean UpdateMode;
        String appName, ModuleName;
        
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        
        
        String destinationPath = String.Empty;
        String ExeName = String.Empty;
        String Module = String.Empty;
        String RootDirectory = String.Empty;
        String RootName = String.Empty;
        
        public FrmBillAttachmentTool()
        {
            InitializeComponent();
        }

        

        public void AttachImage(Form GetFormName, String GetFolderName, String GetRefno, DateTime GetRefDate, String GetDivision, String GetCompCode, String GetLedgerCode, Boolean GetUpdateMode)
        {
            try
            {
                FormName = GetFormName;
                FolderName = GetFolderName;
                Refno = GetRefno.ToString();
                RefDate = GetRefDate;
                Division = GetDivision.ToString();
                CompCode = GetCompCode;
                LedgerCode = GetLedgerCode;
                UpdateMode=GetUpdateMode;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmBillAttachmentTool_Load(object sender, EventArgs e)
        {
            try
            {

                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                this.KeyPreview = true;
                this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                appName = "/" + System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";
                ModuleName = appName.Substring(appName.IndexOf("/") + 1, appName.IndexOf(".") - 1);
                RootDirectory = @"\\172.16.10.169\F$\ScanBillDoc";




                ClearTextbox(GpImage);
                Bill_Attachment_Controls();
                TxtImage1.Text = ShowImage(FormName, FolderName, Refno.ToString(),  RefDate.Date, Division, CompCode, LedgerCode, Picture1);
                TxtImage2.Text = ShowImage(FormName, FolderName, Refno.ToString(), RefDate.Date, Division, CompCode, LedgerCode, Picture2);
                TxtImage3.Text = ShowImage(FormName, FolderName, Refno.ToString(), RefDate.Date, Division, CompCode, LedgerCode, Picture3);
                TxtImage4.Text = ShowImage(FormName, FolderName, Refno.ToString(),  RefDate.Date, Division, CompCode, LedgerCode, Picture4);
                TxtImage5.Text = ShowImage(FormName, FolderName, Refno.ToString(),  RefDate.Date, Division, CompCode, LedgerCode, Picture5);
                Bill_Attachment_Controls();

            }
            catch (Exception Ex)
            {
                if (Ex.Message.Contains("\\"))
                {
                    MessageBox.Show("File Not Found...!");
                }
                else
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        void ClearTextbox(GroupBox gbox)
        {
           

            try
            {

                if (image1 != null)
                {
                    image1.Dispose();
                }
                if (image2 != null)
                {
                    image2.Dispose();
                }
                if (image3 != null)
                {
                    image3.Dispose();
                }
                if (image4 != null)
                {
                    image4.Dispose();
                }
                if (image5 != null)
                {
                    image5.Dispose();
                }



                foreach (Control ctrl in gbox.Controls)
                {



                    if (ctrl is CheckBox)
                    {

                        CheckBox checkBox = (CheckBox)ctrl;

                        checkBox.Checked = false;
                        checkBox.Tag = String.Empty;

                    }
                    if (ctrl is TextBox)
                    {

                        TextBox TxtBox = (TextBox)ctrl;
                        TxtBox.Text = String.Empty;
                        TxtBox.Tag = String.Empty;
                       
                        TxtBox.Font = new Font("Microsoft Sans Serif", 8f,FontStyle.Bold);
                    }
                    if (ctrl is PictureBox)
                    {

                        PictureBox Pic = (PictureBox)ctrl;
                        Pic.Image = null;


                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CreateDirectoty(String RootDirectory, String FolderName)
        {
            try
            {
                

                IntPtr admin_token = default(IntPtr);
                WindowsIdentity wid_current = WindowsIdentity.GetCurrent();
                WindowsIdentity wid_admin = null;
                WindowsImpersonationContext wic = null;


                //bool bImpersonated = LogonUser("fileserver", "GAINUPIPL.COM", "File@123", 9, 0, ref admin_token);
                //wid_admin = new WindowsIdentity(admin_token);
                //wic = wid_admin.Impersonate();


                if (!Directory.Exists(RootDirectory))
                {

                    Directory.CreateDirectory(RootDirectory);
                    Directory.CreateDirectory(Path.Combine(RootDirectory, FolderName));


                }
                if (Directory.Exists(RootDirectory))
                {
                    if (!Directory.Exists(Path.Combine(RootDirectory, FolderName + "\\DELETED")))
                    {
                        Directory.CreateDirectory(Path.Combine(RootDirectory, FolderName + "\\DELETED"));

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        void Bill_Attachment_Controls()
        {
            try
            {
                attachAll();
                if (UpdateMode)
                {
                    BtnAdd.Enabled = true;
                    BtnAdd2.Enabled = true;
                    BtnAdd3.Enabled = true;
                    BtnAdd4.Enabled = true;
                    BtnAdd5.Enabled = true;
                    BtnRemove.Enabled = true;
                    BtnRemove2.Enabled = true;
                    BtnRemove3.Enabled = true;
                    BtnRemove4.Enabled = true;
                    BtnRemove5.Enabled = true;
                    BtnAddAll.Enabled = true;
                    BtnRemoveAll.Enabled = true;
                    if (Picture1.Image == null)
                    {
                        BtnAdd.Enabled = true;
                        BtnRemove.Enabled = false;
                    }
                    else
                    {
                        BtnAdd.Enabled = false;
                        BtnRemove.Enabled = true;
                    }
                    if (Picture2.Image == null)
                    {
                        BtnAdd2.Enabled = true;
                        BtnRemove2.Enabled = false;
                    }
                    else
                    {
                        BtnAdd2.Enabled = false;
                        BtnRemove2.Enabled = true;
                    }
                    if (Picture3.Image == null)
                    {
                        BtnAdd3.Enabled = true;
                        BtnRemove3.Enabled = false;
                    }
                    else
                    {
                        BtnAdd3.Enabled = false;
                        BtnRemove3.Enabled = true;
                    }
                    if (Picture4.Image == null)
                    {
                        BtnAdd4.Enabled = true;
                        BtnRemove4.Enabled = false;
                    }
                    else
                    {
                        BtnAdd4.Enabled = false;
                        BtnRemove4.Enabled = true;
                    }
                    if (Picture5.Image == null)
                    {
                        BtnAdd5.Enabled = true;
                        BtnRemove5.Enabled = false;
                    }
                    else
                    {
                        BtnAdd5.Enabled = false;
                        BtnRemove5.Enabled = true;
                    }
                }
                else
                {
                    BtnAdd.Enabled = false;
                    BtnAdd2.Enabled = false;
                    BtnAdd3.Enabled = false;
                    BtnAdd4.Enabled = false;
                    BtnAdd5.Enabled = false;
                    BtnRemove.Enabled = false;
                    BtnRemove2.Enabled = false;
                    BtnRemove3.Enabled = false;
                    BtnRemove4.Enabled = false;
                    BtnRemove5.Enabled = false;
                    BtnAddAll.Enabled = false;
                    BtnRemoveAll.Enabled = false;


                }


                if (Refno == String.Empty || CompCode==String.Empty)
                {
                    BtnAdd.Enabled = false;
                    BtnAdd2.Enabled = false;
                    BtnAdd3.Enabled = false;
                    BtnAdd4.Enabled = false;
                    BtnAdd5.Enabled = false;
                    BtnRemove.Enabled = false;
                    BtnRemove2.Enabled = false;
                    BtnRemove3.Enabled = false;
                    BtnRemove4.Enabled = false;
                    BtnRemove5.Enabled = false;
                    BtnAddAll.Enabled = false;
                }

                attachAll();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        String ShowImage(Form FormName, String FolderName, String Refno, DateTime RefDate, String Division, String CompCode, String LedgerCode, PictureBox PictureBoxName)
        {
            try
            {

                IntPtr admin_token = default(IntPtr);
                WindowsIdentity wid_current = WindowsIdentity.GetCurrent();
                WindowsIdentity wid_admin = null;
                WindowsImpersonationContext wic = null;
                bool bImpersonated = LogonUser("fileserver", "GAINUPIPL.COM", "File@123", 9, 0, ref admin_token);
                wid_admin = new WindowsIdentity(admin_token);
                wic = wid_admin.Impersonate();


                String ImageName = String.Empty,ImagePAth=String.Empty;
                if (Refno == String.Empty)
                {

                }
                else if (CompCode==String.Empty)
                {

                }
               
                else
                {

                    if (LedgerCode == String.Empty)
                    {
                        LedgerCode = "-";
                    }
                    if (Division == String.Empty)
                    {
                        Division = "-";
                    }


                    PictureBoxName.Image = null;
                    
                    DataTable Dt1 = new DataTable();
                    String Sts1 = "Select  Top 1 * From VAAHINI_ERP_GAINUP.dbo.GrnBillScanMaster A Left join VAAHINI_ERP_GAINUP.dbo.GrnBillScanDetail B On A.Rowid = B.Masterid Where A.Refno='" + Refno + "' And A.RefDate='" + String.Format("{0:dd-MMM-yyyy}", RefDate.Date) + "' And A.Mode='ADD' And A.LedgerCode=" + LedgerCode + " And A.Type='" + FolderName + "' And PictureBoxName='" + PictureBoxName.Name.ToString() + "' And A.CompCode=" + CompCode + " And FormName='" + FormName.Name.ToString() + "' Order by  A.Rowid desc,B.Rowid  desc";
                    MyBase.Load_Data(Sts1, ref Dt1);

                    if (Dt1.Rows.Count > 0)
                    {
                        FileInfo fi = new FileInfo(Dt1.Rows[0]["Imagepath"].ToString());
                        if (fi.Exists)
                        {
                            if (PictureBoxName.Name.ToString() == "Picture1")
                            {
                                image1 = null;
                                image1 = Image.FromFile(Dt1.Rows[0]["Imagepath"].ToString());
                                PictureBoxName.Image = image1;
                                ImageName = Dt1.Rows[0]["ImageName"].ToString();
                                ImagePAth = Dt1.Rows[0]["Imagepath"].ToString();

                            }
                            if (PictureBoxName.Name.ToString() == "Picture2")
                            {
                                image2 = null;
                                image2 = Image.FromFile(Dt1.Rows[0]["Imagepath"].ToString());
                                PictureBoxName.Image = image2;
                                ImageName = Dt1.Rows[0]["ImageName"].ToString();
                                ImagePAth = Dt1.Rows[0]["Imagepath"].ToString();

                            }
                            if (PictureBoxName.Name.ToString() == "Picture3")
                            {
                                image3 = null;
                                image3 = Image.FromFile(Dt1.Rows[0]["Imagepath"].ToString());
                                PictureBoxName.Image = image3;
                                ImageName = Dt1.Rows[0]["ImageName"].ToString();
                                ImagePAth = Dt1.Rows[0]["Imagepath"].ToString();

                            }
                            if (PictureBoxName.Name.ToString() == "Picture4")
                            {
                                image4 = null;
                                image4 = Image.FromFile(Dt1.Rows[0]["Imagepath"].ToString());
                                PictureBoxName.Image = image4;
                                ImageName = Dt1.Rows[0]["ImageName"].ToString();
                                ImagePAth = Dt1.Rows[0]["Imagepath"].ToString();

                            }
                            if (PictureBoxName.Name.ToString() == "Picture5")
                            {
                                image5 = null;
                                image5 = Image.FromFile(Dt1.Rows[0]["Imagepath"].ToString());
                                PictureBoxName.Image = image5;
                                ImageName = Dt1.Rows[0]["ImageName"].ToString();
                                ImagePAth = Dt1.Rows[0]["Imagepath"].ToString();
                            }
                        }

                    }
                }
                attachAll();
                FileIsLocked(ImagePAth);
                return ImageName;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }



        }

        void attachAll()
        {
            try
            {
                if (UpdateMode)
                {
                    if (Picture1.Image == null && Picture2.Image == null && Picture3.Image == null && Picture4.Image == null && Picture5.Image == null)
                    {
                        BtnAddAll.Enabled = true;
                    }
                    else
                    {
                        BtnAddAll.Enabled = false;
                    }
                }
                else
                {
                    BtnAddAll.Enabled = false;
                    BtnRemoveAll.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                 
                throw ex;
            }

        }

        String UploadImage(Form FormName, String FolderName, String Refno, DateTime RefDate, String Division, String CompCode, String LedgerCode, PictureBox PictureBoxName,Boolean UploadEach)
        {
            try
            {
                ArrayList a1 = new ArrayList();
                if (Division.ToString() == String.Empty)
                {
                    Division = "-";
                }
               
                if (LedgerCode.ToString() == String.Empty)
                {
                    LedgerCode = "-";
                }

               
                destinationPath = String.Empty;
                ExeName = "/" + System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";
                Module = appName.Substring(appName.IndexOf("/") + 1, appName.IndexOf(".") - 1);
                
                RootName = Module + "_" + FolderName + "_" + FormName.Name.ToString().ToUpper().Replace("FRM", "");
                CreateDirectoty(RootDirectory, RootName);
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                 
                openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
                if (UploadEach == true)
                {
                    openFileDialog1.Multiselect = false;
                    openFileDialog1.FilterIndex = 1;
                }
                else
                {
                    openFileDialog1.Multiselect = true;
                    openFileDialog1.FilterIndex = 5;
                }
               
               

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    

                        foreach (String file in openFileDialog1.FileNames)
                        {

                            a1.Add(file);


                        }
                    
                 

                    openFileDialog1.Dispose();
                }
                else
                {

                }
                    if (UploadEach == false)
                    {
                        for (int i = 0; i <= a1.Count - 1; i++)
                        {
                            if (i == 0)
                            {
                                TxtImage1.Text = InsertImage(a1[i].ToString(), Picture1);

                            }
                            if (i == 1)
                            {
                                TxtImage2.Text = InsertImage(a1[i].ToString(), Picture2);
                            }
                            if (i == 2)
                            {
                                TxtImage3.Text = InsertImage(a1[i].ToString(), Picture3);
                            }
                            if (i == 3)
                            {
                                TxtImage4.Text = InsertImage(a1[i].ToString(), Picture4);
                            }
                            if (i == 4)
                            {
                                TxtImage5.Text = InsertImage(a1[i].ToString(), Picture5);
                            }
                        }
                    }
                    
                    String ImgName = String.Empty;
                    if (a1.Count > 0)
                    {
                        ImgName = InsertImage(a1[0].ToString(), PictureBoxName);
                    }
                    attachAll();
                    FileIsLocked(destinationPath);
                    return ImgName;



            }

            catch (Exception ex)
            {
               
                    throw ex;
                 
            }
        }

        String InsertImage(String file, PictureBox PictureBoxName)
        {
            try
            {
                String sourcePath = Path.GetFullPath(file);
                int Count = 1;

                String Newname = Refno + "_" + String.Format("{0:dd-MMM-yyyy}", RefDate.Date) + "(" + Count + ")" + Path.GetExtension(sourcePath);
                String directory = Path.GetDirectoryName(sourcePath);
                destinationPath = String.Concat(Path.Combine(RootDirectory, RootName) + "\\" + System.IO.Path.GetFileName(Newname));

                 

                while (FileExistsRecursive(Path.Combine(RootDirectory, RootName), System.IO.Path.GetFileName(Newname)))
                {
                    Count++;
                    Newname = Refno + "_" + String.Format("{0:dd-MMM-yyyy}", RefDate.Date) + "(" + Count + ")" + Path.GetExtension(sourcePath);
                    directory = Path.GetDirectoryName(sourcePath);
                    destinationPath = String.Concat(Path.Combine(RootDirectory, RootName) + "\\" + System.IO.Path.GetFileName(Newname));


                }



                FileInfo fi = new FileInfo(sourcePath);
               
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

                if (fi.Exists)
                {
                            fi.IsReadOnly = false;
                            fi.MoveTo(destinationPath);

                            System.GC.Collect();
                            System.GC.WaitForPendingFinalizers();
                            
                        String[] Qur = new String[100];
                        Int32 index_ary = 0;

                        DataTable Dt1 = new DataTable();
                        String Sts1 = "Select * FRom VAAHINI_ERP_GAINUP.dbo.GrnBillScanMaster Where Refno='" + Refno + "' And RefDate='" + String.Format("{0:dd-MMM-yyyy}", RefDate) + "' And Mode='ADD' And LedgerCode=" + LedgerCode + " And Type='" + FolderName + "' And FormName='" + FormName.Name.ToString() + "' And CompCode=" + CompCode + "";
                        MyBase.Load_Data(Sts1, ref Dt1);

                        if (Dt1.Rows.Count > 0)
                        {
                            Qur[index_ary++] = "Insert into VAAHINI_ERP_GAINUP.dbo.GrnBillScanDetail(Masterid,ImageName,ImagePath,PictureBoxName)values(" + Dt1.Rows[0]["Rowid"].ToString() + ",'" + System.IO.Path.GetFileName(destinationPath) + "','" + destinationPath + "','" + PictureBoxName.Name + "')";

                        }
                        else
                        {
                            Qur[index_ary++] = "Insert into VAAHINI_ERP_GAINUP.dbo.GrnBillScanMaster(Type,Mode,RefNo,RefDate,Division,CompCode,LedgerCode,Module,FormName)values('" + FolderName + "','ADD','" + Refno + "','" + String.Format("{0:dd-MMM-yyyy}", RefDate.Date) + "',(Case When " + Division + "='-' Then Null Else " + Division + " End)," + CompCode + ",(Case When " + LedgerCode + "='-' Then Null Else " + LedgerCode + " End),'" + Module + "','" + FormName.Name.ToString() + "');Select Scope_Identity();";
                            Qur[index_ary++] = "Insert into VAAHINI_ERP_GAINUP.dbo.GrnBillScanDetail(Masterid,ImageName,ImagePath,PictureBoxName)values(@@identity,'" + System.IO.Path.GetFileName(destinationPath) + "','" + destinationPath + "','" + PictureBoxName.Name + "')";
                        }
                        MyBase.Run_Identity(false, Qur);
                        PictureBoxName.Image =Image.FromFile(destinationPath);
                        PictureBoxName.SizeMode = PictureBoxSizeMode.StretchImage;
                        FileIsLocked(destinationPath);
                        return Path.GetFileName(destinationPath);


                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {

                throw ex;

            }

        }

        public bool FileIsLocked(string strFullFileName)
        {
            bool blnReturn = false;
            System.IO.FileStream fs;
            try
            {
                if (strFullFileName.ToString().Trim() != String.Empty)
                {
                    fs = System.IO.File.Open(strFullFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read, System.IO.FileShare.None);
                    fs.Close();
                }
            }
            catch (System.IO.IOException ex)
            {
                blnReturn = true;
            }
            return blnReturn;
        }

        private bool FileExistsRecursive(string rootPath, string filename)
        {
            if (File.Exists(Path.Combine(rootPath, filename)))
                return true;

            foreach (string subDir in Directory.GetDirectories(rootPath))
            {
                return FileExistsRecursive(subDir, filename);
            }

            return false;
        }

    


        void RemoveImage(Form FormName, String FolderName, String Refno, DateTime RefDate, String ImageName, String Division, String CompCode, String LedgerCode, PictureBox PictureBoxName)
        {

            try
            {
                if (ImageName != String.Empty && PictureBoxName.Image != null)
                {

                    if (PictureBoxName.Name.ToString() == "Picture1")
                    {
                        if (image1 != null)
                        {
                            image1.Dispose();

                        }
                    }
                    if (PictureBoxName.Name.ToString() == "Picture2")
                    {
                        if (image2 != null)
                        {
                            image2.Dispose();

                        }
                    }
                    if (PictureBoxName.Name.ToString() == "Picture3")
                    {
                        if (image3 != null)
                        {
                            image3.Dispose();

                        }
                    }
                    if (PictureBoxName.Name.ToString() == "Picture4")
                    {
                        if (image4 != null)
                        {
                            image4.Dispose();

                        }
                    }
                    if (PictureBoxName.Name.ToString() == "Picture5")
                    {
                        if (image5 != null)
                        {
                            image5.Dispose();

                        }
                    }

                    if (Division.ToString() == String.Empty)
                    {
                        Division = "-";
                    }

                    if (LedgerCode.ToString() == String.Empty)
                    {
                        LedgerCode = "-";
                    }

                    ExeName = "/" + System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";
                    Module = appName.Substring(appName.IndexOf("/") + 1, appName.IndexOf(".") - 1);
                    
                    RootName = Module + "_" + FolderName + "_" + FormName.Name.ToString().ToUpper().Replace("FRM", "");
                    CreateDirectoty(RootDirectory, RootName);
                    String SourceFile = Path.Combine(RootDirectory, Path.Combine(RootName, ImageName));
                    String Destination = Path.Combine(Path.Combine(RootDirectory, RootName), Path.Combine("DELETED", ImageName));
                    int Count = 1;
                    String directory = String.Empty, Newname = String.Empty;

                    Newname = ImageName.Replace(Path.GetExtension(Destination), "") + "_" + Count + "_" + Path.GetExtension(Destination);
                    directory = Path.GetDirectoryName(Destination);
                    destinationPath = String.Concat(Path.Combine(RootDirectory, RootName) + "\\DELETED\\" + System.IO.Path.GetFileName(Newname));

                    //destinationPath = Destination;

                    while (File.Exists(destinationPath))
                    {
                        String FileNAme = Path.GetFileName(destinationPath).ToString().Replace(Path.GetExtension(destinationPath), "");
                        String s1 = FileNAme;
                        String ws = String.Empty;
                        ws = s1.Substring(s1.Length - 3, 3);
                        int st = ws.ToString().IndexOf("_") + 1;
                        int ed = ws.ToString().LastIndexOf("_") - 1;



                        String val = String.Empty;

                        if (st > 0 && ed > 0)
                        {
                            val = ws.ToString().Substring(st, ed);
                        }
                        if (val != String.Empty && isNumeric(val, System.Globalization.NumberStyles.Integer))
                        {
                            Count = Convert.ToInt32(val) + 1;

                        }
                        else
                        {
                            Count++;
                        }

                        Newname = ImageName.Replace(Path.GetExtension(Destination), "") + "_" + Count + "_" + Path.GetExtension(Destination);
                        directory = Path.GetDirectoryName(Destination);
                        destinationPath = String.Concat(Path.Combine(RootDirectory, RootName) + "\\DELETED\\" + System.IO.Path.GetFileName(Newname));

                    }

                    PictureBoxName.Image = null;
                    //DirectoryInfo di = new DirectoryInfo(SourceFile);
                    FileInfo fi = new FileInfo(SourceFile);
                    //foreach (FileInfo fi in di.GetFiles())
                    //{
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    fi.IsReadOnly = false;
                    if (fi.Exists)
                    {
                        fi.MoveTo(destinationPath);
                    }
                    //}
                    //di.Delete();


                    //File.Move(SourceFile, destinationPath);
                    String[] Qur = new String[100];
                    Int32 index_ary = 0;

                    DataTable Dt2 = new DataTable();
                    String Sts2;

                    DataTable Dt1 = new DataTable();
                    String Sts1 = "Select Top 1 A.Rowid,B.Rowid Dt_Rowid,B.MasterId FRom GrnBillScanMaster A Left join GrnBillScanDetail B On A.Rowid=B.Masterid Where A.Refno='" + Refno + "' And A.RefDate='" + String.Format("{0:dd-MMM-yyyy}", RefDate) + "' And Mode='ADD' And A.LedgerCode=" + LedgerCode + " And A.Type='" + FolderName + "' And A.FormName='" + FormName.Name.ToString() + "' And A.CompCode=" + CompCode + " And PictureBoxName='" + PictureBoxName.Name.ToString() + "' Order By A.Rowid desc";
                    MyBase.Load_Data(Sts1, ref Dt1);

                    if (Dt1.Rows.Count > 0)
                    {


                        Dt2 = new DataTable();
                        Sts2 = "Select * FRom GrnBillScanMaster_DeleteLog Where Refno='" + Refno + "' And RefDate='" + String.Format("{0:dd-MMM-yyyy}", RefDate) + "' And Mode='DELETE' And LedgerCode=" + LedgerCode + " And Type='" + FolderName + "' And FormName='" + FormName.Name.ToString() + "' And CompCode=" + CompCode + "";
                        MyBase.Load_Data(Sts2, ref Dt2);

                        if (Dt2.Rows.Count > 0)
                        {
                            Qur[index_ary++] = "Insert into GrnBillScanDetail_DeleteLog(Masterid,ImageName,ImagePath,PictureBoxName)values(" + Dt2.Rows[0]["Rowid"].ToString() + ",'" + System.IO.Path.GetFileName(destinationPath) + "','" + destinationPath + "','" + PictureBoxName.Name + "')";

                        }
                        else
                        {
                            Qur[index_ary++] = "Insert into GrnBillScanMaster_DeleteLog(Type,Mode,RefNo,RefDate,Division,CompCode,LedgerCode,Module,FormName)values('" + FolderName + "','DELETE','" + Refno + "','" + String.Format("{0:dd-MMM-yyyy}", RefDate.Date) + "',(Case When " + Division + "='-' Then Null Else " + Division + " End)," + CompCode + ",(Case When " + LedgerCode + "='-' Then Null Else " + LedgerCode + " End),'" + Module + "','" + FormName.Name.ToString() + "');Select Scope_Identity();";
                            Qur[index_ary++] = "Insert into GrnBillScanDetail_DeleteLog(Masterid,ImageName,ImagePath,PictureBoxName,UploadFlag_Id)values(@@identity,'" + System.IO.Path.GetFileName(destinationPath) + "','" + destinationPath + "','" + PictureBoxName.Name.ToString() + "'," + Dt1.Rows[0]["Rowid"].ToString() + ")";
                        }
                        MyBase.Run_Identity(false, Qur);

                        index_ary = 0;

                        Qur[index_ary++] = "Delete from GrnBillScanDetail Where  Rowid=" + Dt1.Rows[0]["Dt_Rowid"].ToString() + "";
                        MyBase.Run_Identity(false, Qur);

                        index_ary = 0;

                        Dt2 = new DataTable();
                        Sts2 = "Select * From GrnBillScanDetail Where Masterid=" + Dt1.Rows[0]["Rowid"].ToString() + "";
                        MyBase.Load_Data(Sts2, ref Dt2);
                        if (Dt2.Rows.Count == 0)
                        {
                            Qur[index_ary++] = "Delete from GrnBillScanMaster Where Refno='" + Refno + "' And RefDate='" + String.Format("{0:dd-MMM-yyyy}", RefDate) + "' And Mode='ADD' And LedgerCode=" + LedgerCode + " And Type='" + FolderName + "' And FormName='" + FormName.Name.ToString() + "' And CompCode=" + CompCode + " And Rowid=" + Dt1.Rows[0]["Rowid"].ToString() + "";
                        }

                    }
                    MyBase.Run_Identity(false, Qur);

                    ImageName = String.Empty;
                    PictureBoxName.Image = null;
                    attachAll();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void FrmBillAttachmentTool_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                this.Dispose();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }






        public string[] getFiles(string SourceFolder, string Filter,
         System.IO.SearchOption searchOption)
        {
            // ArrayList will hold all file names
            ArrayList alFiles = new ArrayList();

            // Create an array of filter string
            string[] MultipleFilters = Filter.Split('|');

            // for each filter find mathing file names
            foreach (string FileFilter in MultipleFilters)
            {
                // add found file names to array list
                alFiles.AddRange(Directory.GetFiles(SourceFolder, FileFilter, searchOption));
            }

            // returns string array of relevant file names
            return (string[])alFiles.ToArray(typeof(string));
        }

        public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
        {
            try
            {
                Double result;
                return Double.TryParse(val, NumberStyle,
                System.Globalization.CultureInfo.CurrentCulture, out result);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        Boolean CompareTwoImages(String SourcePath, PictureBox DestinationPath)
        {

            string img1_ref, img2_ref;
            Stream bmpStream;

            bmpStream = System.IO.File.Open(SourcePath, System.IO.FileMode.Open, FileAccess.Read);

            Image image1 = Image.FromStream(bmpStream);


            Bitmap img1 = new Bitmap(image1);
            Bitmap img2 = new Bitmap(DestinationPath.Image);
            Boolean flag = false; Int64 count1 = 0, count2 = 0;
            if (img1.Width == img2.Width && img1.Height == img2.Height)
            {
                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        img1_ref = img1.GetPixel(i, j).ToString();
                        img2_ref = img2.GetPixel(i, j).ToString();
                        if (img1_ref != img2_ref)
                        {
                            count2++;
                            flag = true;
                            break;
                        }
                        count1++;
                    }

                }

            }
            bmpStream.Close();
            bmpStream.Dispose();
            return flag;

        }


        public bool CheckIfFileIsBeingUsed(string fileName)
        {

            try
            {
                FileStream Fm;
                //Fm = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                Fm = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                //Fm.Flush();
                Fm.Close();
                Fm.Dispose();
            }

            catch (Exception exp)
            {

                return true;

            }

            return false;

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                    Bill_Attachment_Controls();
                    TxtImage1.Text = UploadImage(FormName, FolderName, Refno, RefDate.Date, Division,CompCode, LedgerCode, Picture1,true);
                    Bill_Attachment_Controls();
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToUpper().Contains("THE PATH IS NOT OF A LEGAL FORM"))
                {

                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                    if (MessageBox.Show("Are you sure to Remove Grn Bill Image..?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Bill_Attachment_Controls();
                        RemoveImage(FormName, FolderName, Refno,  RefDate.Date, TxtImage1.Text, Division, CompCode, LedgerCode, Picture1);
                        TxtImage1.Text = String.Empty;
                        Bill_Attachment_Controls();
                    }
                 

            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToUpper().Contains("THE PATH IS NOT OF A LEGAL FORM"))
                {

                }
                else
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnAdd2_Click(object sender, EventArgs e)
        {
            try
            {

                Bill_Attachment_Controls();
                TxtImage2.Text = UploadImage(FormName, FolderName, Refno, RefDate.Date, Division, CompCode, LedgerCode, Picture2, true);
                
                Bill_Attachment_Controls();
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToUpper().Contains("THE PATH IS NOT OF A LEGAL FORM"))
                {

                }
                else
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnRemove2_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show("Are you sure to Remove Grn Bill Image..?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Bill_Attachment_Controls();
                    RemoveImage(FormName, FolderName, Refno, RefDate.Date, TxtImage2.Text, Division, CompCode, LedgerCode, Picture2);
                    TxtImage2.Text = String.Empty;
                    Bill_Attachment_Controls();
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToUpper().Contains("THE PATH IS NOT OF A LEGAL FORM"))
                {

                }
                else
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnAdd3_Click(object sender, EventArgs e)
        {
            try
            {

                Bill_Attachment_Controls();
                TxtImage3.Text = UploadImage(FormName, FolderName, Refno, RefDate.Date, Division, CompCode, LedgerCode, Picture3, true);
                Bill_Attachment_Controls();
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToUpper().Contains("THE PATH IS NOT OF A LEGAL FORM"))
                {

                }
                else
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnRemove3_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show("Are you sure to Remove Grn Bill Image..?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Bill_Attachment_Controls();
                    RemoveImage(FormName, FolderName, Refno, RefDate.Date,TxtImage3.Text, Division, CompCode, LedgerCode, Picture3);
                    TxtImage3.Text = String.Empty;
                    Bill_Attachment_Controls();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAdd4_Click(object sender, EventArgs e)
        {
            try
            {

                Bill_Attachment_Controls();
                TxtImage4.Text = UploadImage(FormName, FolderName, Refno, RefDate.Date, Division, CompCode, LedgerCode, Picture4, true);
                Bill_Attachment_Controls();
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToUpper().Contains("THE PATH IS NOT OF A LEGAL FORM"))
                {

                }
                else
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnRemove4_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show("Are you sure to Remove Grn Bill Image..?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Bill_Attachment_Controls();
                    RemoveImage(FormName, FolderName, Refno, RefDate.Date,TxtImage4.Text, Division, CompCode, LedgerCode, Picture4);
                    TxtImage4.Text = String.Empty;
                    Bill_Attachment_Controls();

                }

            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToUpper().Contains("THE PATH IS NOT OF A LEGAL FORM"))
                {

                }
                else
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnAdd5_Click(object sender, EventArgs e)
        {

            try
            {

                Bill_Attachment_Controls();
                TxtImage5.Text = UploadImage(FormName, FolderName, Refno, RefDate.Date, Division, CompCode, LedgerCode, Picture5, true);
                Bill_Attachment_Controls();
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToUpper().Contains("THE PATH IS NOT OF A LEGAL FORM"))
                {

                }
                else
                {

                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void BtnRemove5_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show("Are you sure to Remove Grn Bill Image..?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Bill_Attachment_Controls();
                    RemoveImage(FormName, FolderName, Refno,RefDate.Date, TxtImage5.Text, Division, CompCode, LedgerCode, Picture5);
                    TxtImage5.Text = String.Empty;
                    Bill_Attachment_Controls();

                }

            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToUpper().Contains("THE PATH IS NOT OF A LEGAL FORM"))
                {

                }
                else
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void FrmBillAttachmentTool_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmBillAttachmentTool_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAddAll_Click(object sender, EventArgs e)
        {
            try
            {
                Bill_Attachment_Controls();
                UploadImage(FormName, FolderName, Refno, RefDate.Date, Division, CompCode, LedgerCode, Picture5,false);
                Bill_Attachment_Controls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnRemoveAll_Click(object sender, EventArgs e)
        {
            try
            {
               if (MessageBox.Show("Are you sure to Remove Grn Bill Image..?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {

                    Bill_Attachment_Controls();
                    RemoveImage(FormName, FolderName, Refno, RefDate.Date, TxtImage1.Text, Division, CompCode, LedgerCode, Picture1);
                    RemoveImage(FormName, FolderName, Refno, RefDate.Date, TxtImage2.Text, Division, CompCode, LedgerCode, Picture2);
                    RemoveImage(FormName, FolderName, Refno, RefDate.Date, TxtImage3.Text, Division, CompCode, LedgerCode, Picture3);
                    RemoveImage(FormName, FolderName, Refno, RefDate.Date, TxtImage4.Text, Division, CompCode, LedgerCode, Picture4);
                    RemoveImage(FormName, FolderName, Refno, RefDate.Date, TxtImage5.Text, Division, CompCode, LedgerCode, Picture5);
                    Bill_Attachment_Controls();
                    ClearTextbox(GpImage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



    }
}
