using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Windows.Forms;
using System.IO;
using System.Data.Odbc;
using Accounts;

namespace Floor
{
    public partial class FrmPoAdvanceEntry : Form, Entry
    {
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        Control_Modules MyBase = new Control_Modules();
        DataTable Dt = new DataTable();
        DataTable Dt_Tax = new DataTable();
        DataTable[] DtQty;
        DataTable[, ,] Dt_OCN_New;
        Int64 Code = 0;
        DataRow Dr;
        DataRow Dr1;
        TextBox Txt_Tax = null;
        TextBox Txt_DefectRowid = null;
        TextBox TxtRoll = null;
        Int32 Max_Val = 80;
        Int32 Excess_Limit = 15;
        TextBox Txt = null;
        TextBox Txt_Lot = null;        
        //TextBox Txt_Output = null;
        //TextBox Txt_Input = null;

        String Str = "";
        String Queries = "";

        Int16 Vis = 0;
        int Pos = 0;
        
        public FrmPoAdvanceEntry()
        {
            InitializeComponent();
        }

        private void FrmPoAdvanceEntry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                //Grid_Defect_Data();
                MyBase.Disable_Cut_Copy(GBMain);
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
                Grid_PO();                                
                DtQty = new DataTable[30];
                MyBase.Date_Control(ref DtpDate, 1);
                TxtCompany.Focus();
                checkBox1.Checked = false;
                checkBox1.Enabled = true;
                //GridPoItemDetails();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Cancel()
        {
            try
            {   
                MyBase.Clear(this);
                //GridQty.DataSource = null;
                //GBQty.Visible = false;
                DtpDate.Focus();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Show_Image1()
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Pictures(*.Jpg,*.Gif,*.Bmp)|*.Jpg;,*.Gif;,*.Bmp;";
                openFileDialog1.FileName = String.Empty;
                openFileDialog1.ShowDialog();

                if (openFileDialog1.FileName.Trim() != String.Empty)
                {
                    Update_Image1(openFileDialog1.FileName);
                }
                else
                {
                    //Image1.Image = Image1.InitialImage;                  
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_Image1(String Term)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<String>(Update_Image1), new Object[] { Term });
                    return;
                }
                PhImage.Image = Image.FromFile(Term);
                PhImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtSupplier.Text.ToString() == String.Empty || TxtPI.Text.ToString() == String.Empty)
                {
                    MessageBox.Show("Select Supplier And PI Number");
                    GBImage.Visible = false;
                    TxtSupplier.Focus();
                    return;
                }
                OpenFileDialog op1 = new OpenFileDialog();
                op1.Multiselect = true;
                op1.ShowDialog();
                op1.Filter = "allfiles|*.xls";
                textBox1.Text = op1.FileName;
                int count = 0;
                string[] FName;
 


                
                //foreach (string s in op1.FileNames)
                //{
                //    FName = s.Split('\\');
                //    File.Copy(s, "D:\\File\\" + FName[FName.Length - 1]);
                //    count++;
                //}


                foreach (string s in op1.FileNames)
                {
                    FName = s.Split('\\');
                    if (System.IO.File.Exists("\\\\172.16.10.169\\f\\PO ADVANCE\\" + TxtPI.Text.ToString().Replace("/", "$") + "_" + DtpPIDate.Value.ToShortDateString().Replace("/", "$") + "_" + TxtSupplier.Tag.ToString() + "_" + FName[FName.Length - 1] + "") == true)
                    {
                        System.IO.File.Delete("\\\\172.16.10.169\\f\\PO ADVANCE\\" + TxtPI.Text.ToString().Replace("/", "$") + "_" + DtpPIDate.Value.ToShortDateString().Replace("/", "$") + "_" + TxtSupplier.Tag.ToString() + "_" + FName[FName.Length - 1] + "");
                    }



                    File.Copy(s, "\\\\172.16.10.169\\f\\PO ADVANCE\\" + TxtPI.Text.ToString().Replace("/", "$") + "_" + DtpPIDate.Value.ToShortDateString().Replace("/", "$") + "_" + TxtSupplier.Tag.ToString() + "_" + FName[FName.Length - 1]);
                   // File.Copy(s, "\\\\172.16.10.169\\f\\PO ADVANCE\\" + FName[FName.Length - 1]);

                    count++;
                }


                MessageBox.Show(Convert.ToString(count) + " File(s) copied");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Thread Th = new Thread(Show_Image1);
        //        Th.SetApartmentState(ApartmentState.STA);
        //        Th.Start();                
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (GBImage.Visible)
                {
                    GBImage.Visible = false;
                }
                else
                {
                    GBImage.Visible = true;
                }
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
                PhImage.Image = null;
                string sourceFile = "";   string destinationFile ="";
                string Directory_Det = "";
                int count = 0;
                String FPath = TxtPI.Text.ToString().Replace("/", "$") + "_" + DtpPIDate.Value.ToShortDateString().Replace("/", "$") + "_" + TxtSupplier.Tag.ToString() + "_*.*";

                DirectoryInfo d = new DirectoryInfo("\\\\172.16.10.169\\f\\PO ADVANCE\\"); //Assuming Test is your Folder
               // DirectoryInfo d = new DirectoryInfo("\\\\172.16.10.169\\f\\PO ADVANCE\\" + TxtPI.Text.ToString() + "_" + DtpPIDate.Value.ToShortDateString().Replace("/", "$") + "_" + TxtSupplier.Tag.ToString() + "_*.*"); //Assuming Test is your Folder

                FileInfo[] Files = d.GetFiles(FPath); //Getting Text files

                //if (!System.IO.Directory.Exists(MapPath(MyTree.SelectedValue + "\\" + TextBox1.Text)))
                
                Directory_Det = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                Directory_Det += @"\" + "PO ADVANCE".ToString();
                if (System.IO.Directory.Exists(Directory_Det) == false)
                {
                    System.IO.Directory.CreateDirectory(Directory_Det);
                    label13.Text = "Directory Created Successfully..........";
                    textBox1.Text = "";
                }
                //else   //To Delete Folder//
                //{

                //    Directory.Delete(Directory_Det, true);
                //    return;
                //}
                else // To Delete Files From Folder
                {
                    string[] files = Directory.GetFiles(Directory_Det);
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                }
              
               for(int i=0;i< Files.Length;i++)
                {
                    sourceFile = @"\\172.16.10.169\f\PO ADVANCE\" + Files[i].ToString() + "";
                    
                   Directory_Det = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                   Directory_Det += "\\" + "PO ADVANCE".ToString();

                 //  destinationFile = System.IO.Path.GetDirectoryName(Directory_Det);
                  destinationFile = "";
                   destinationFile += Directory_Det + "\\" + Files[i].ToString().ToString();
                    File.Copy(sourceFile, destinationFile, true);                 
                    count++;
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
        void Fill_Datas(DataRow Dr)
        {
            try
            {
                Code = Convert.ToInt64(Dr["RowID"]);
                TxtENo.Text = Dr["ENo"].ToString();
                
                TxtCompany.Text = Dr["CompName"].ToString();
                TxtCompany.Tag = Dr["Acc_Company_Code"].ToString();


                TxtDivision.Text = Dr["Division"].ToString();
                TxtDivision.Tag = Dr["Comp_Code"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["EDate"]);
                DtpClsDate.Value = Convert.ToDateTime(Dr["Close_Date"]);
                DtpPIDate.Value = Convert.ToDateTime(Dr["PI_Date"]);

                DataTable Dts = new DataTable();
                String Sts = String.Empty;

                Sts = "Select * From Vaahini_Erp_Gainup.Dbo.Po_Advance_Master Where Rowid = " + Code + " And Without_Po = 'Y' ";

                MyBase.Load_Data(Sts, ref Dts);

                if (Dts.Rows.Count > 0)
                {
                    checkBox1.Checked = true;
                    checkBox1.Enabled = false;
                }
                else
                {
                    checkBox1.Checked = false;
                    checkBox1.Enabled = false;
                }

                TxtReqBy.Text = Dr["Name"].ToString();
                TxtReqBy.Tag = Dr["Req_By"].ToString();

                TxtSupplier.Text = Dr["Supplier"].ToString();
                TxtSupplier.Tag = Dr["Ledger_Code"].ToString();
                TxtFitSupplier.Tag = Dr["Old_Id"].ToString();

                TxtPI.Text = Dr["Pi_No"].ToString();
                TxtPIAmnt.Text = Dr["PI_Amnt"].ToString();
                TxtAdv.Text = Dr["Adv_Per"].ToString();
                TxtTotal.Text = Dr["Tot_Adv"].ToString();
                Load_Image();

                if (GBImage.Visible)
                {
                    GBImage.Visible = false;
                }

                TxtRemarks.Text = Dr["Remarks"].ToString();
                Grid_PO();
                Grid_Defect_Data();
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Load_Image()
        {
            try
            {
                String Str = String.Empty;
                DataTable TmpDt = new DataTable();
                Str = "Select * From VAAHINI_GAINUP_PHOTO.Dbo.Po_Advance_Photo where MAster_ID = " + Code;
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
                        PhImage.SizeMode = PictureBoxSizeMode.StretchImage;
                        PhImage.Image = Ephoto;
                    }
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Po Advance Entry - View", " Select Eno, Edate, Division, Name, Supplier, Tot_Adv, Close_Date, PI_No, PI_Amnt, Adv_Per,  PI_Date, Remarks, Ledger_Code, Rowid, Req_By, Old_Id, Comp_Code, CompName, Acc_Company_Code From Vaahini_erp_Gainup.Dbo.Po_Advance_Entry_Edit_Fn() ", String.Empty, 90, 100, 90, 100, 120, 100, 100, 100, 100, 100, 125);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    GridPo.CurrentCell = GridPo["Po_No", 0];
                    GridPo.Focus();
                    GridPo.BeginEdit(true);
                    //Grid_Defect_Data();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Grid_PO()
        {
            String Str = String.Empty;
            try
            {
                if (MyParent._New == true)
                {
                    Str = "Select 0 Sno, '' [Po_No], '01-Jan-1899' [Po Date], 0.00 [Po Amnt], 0.00 [Prv Adv], 0.00 [Adv Amnt], 0.00 [Bal Amnt], 0 Po_Id From Vaahini_erp_Gainup.Dbo.Fabric_Transfer_Master where 1 = 2 ";
                }
                else
                {
                    if (!checkBox1.Checked)//With PO
                    {
                        Str = "Select S2.Sl_No Sno, S2.Po_No [Po_No], S2.Po_Date [Po Date], S2.Po_Amnt [Po Amnt], Isnull(S3.Prv_Adv_Amnt,0) [Prv Adv], S2.Adv_Amnt [Adv Amnt], (S2.Po_Amnt-Isnull(S3.Prv_Adv_Amnt,0)) [Bal Amnt], S2.Po_Id Po_Id From Vaahini_erp_Gainup.Dbo.Po_Advance_Master S1 Left Join Vaahini_erp_Gainup.Dbo.Po_Advance_Detail S2 on S1.Rowid = S2.Master_Id Left Join(Select Comp_Code, Old_Id, Ledger_Code, Po_No, Po_Id, Sum(Isnull(T2.Adv_Amnt,0))Prv_Adv_Amnt  From Vaahini_erp_Gainup.Dbo.Po_Advance_Master T1 Left Join Vaahini_erp_Gainup.Dbo.Po_Advance_Detail T2 on T1.Rowid = T2.Master_Id Where T1.Rowid ! = " + Code + "  Group By Comp_Code, Old_Id, Ledger_Code, Po_No, Po_Id)S3 on S1.Comp_Code = S3.Comp_Code And S1.Old_Id = S3.Old_Id And S1.Ledger_Code = S3.Ledger_Code And S2.Po_No = S3.Po_No And S2.Po_Id = S3.Po_Id Where S2.Master_ID=" + Code + " Order By S2.Sl_No ";
                    }
                    else
                    {
                        Str = "Select A2.Sl_No Sno, A2.Po_No [Po_No], A2.Po_Date [Po Date], A2.Po_Amnt [Po Amnt], A2.Adv_Amnt [Prv Adv], A2.Adv_Amnt [Adv Amnt], A2.Adv_Amnt [Bal Amnt], A2.Po_Id From Po_Advance_Master A1 Left Join Po_Advance_Detail A2 on A1.Rowid = A2.Master_Id  Where Without_Po='Y'  And A1.Rowid = " + Code + "";
                    }
                }
                GridPo.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref GridPo, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref GridPo, "Po_No", "Adv Amnt");
                MyBase.Grid_Designing(ref GridPo, ref Dt, "Po_Id");
                MyBase.Grid_Width(ref GridPo, 40, 110, 110, 100, 100, 100, 100);

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (!MyParent._New)
                    {
                        GridPoItemDetails();                       
                       
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Save()
        {
            String DH = "";
            String MD = "";
            Boolean PhotoFlag = false;
            try
            {

                String[] Queries;
                Int32 Array_Index = 0;
                String Moved_For = String.Empty;
                TxtRemarks.Focus();
                Total_Count();

                if (GridPo.Rows.Count == 1)
                {
                    MessageBox.Show("Invalid Details..!", "Gainup");
                    GridPo.CurrentCell = GridPo["Po_No", 0];
                    GridPo.Focus();
                    GridPo.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }
                if (Convert.ToDouble(TxtAdv.Text)>100)
                {
                    MessageBox.Show("Advance percentage should be less than or equal to the 100 ");
                    TxtAdv.Focus();
                    return;
                }
                if(Convert.ToDouble(TxtPIAmnt.Text)<Convert.ToDouble(TxtTotal.Text))
                {
                    MessageBox.Show("Advance Amnt should be less than or equal to the PI amount!..", "Gainup");
                    {
                        TxtPIAmnt.Focus();
                        return;
                    }
                }
                if (TxtTotal.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotal.Text) == 0)
                {
                    MessageBox.Show("Invalid Advance Amnt ", "Gainup");
                    GridPo.CurrentCell = GridPo["Po_No", 0];
                    GridPo.Focus();
                    GridPo.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtAdv.Text.ToString() == String.Empty)
                {
                    TxtAdv.Text = "0";
                }
                if (TxtPI.Text.ToString() == String.Empty)
                {
                    TxtPI.Text = "-";
                }
                if (TxtPIAmnt.Text.ToString() == String.Empty)
                {
                    TxtPIAmnt.Text = "0";
                }
                

                //if (PhImage.Image == null)
                //{
                //    if (MessageBox.Show("Invalid PI Copy ..! Sure to Continue ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //    {
                //        PhotoFlag = true;
                //    }
                //    else
                //    {
                //        PhotoFlag = false;
                //        MyParent.Save_Error = true;
                //        return;
                //    }
                //}

                for (int i = 0; i < GridPo.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < GridPo.Columns.Count - 1; j++)
                    {
                        if (GridPo[j, i].Value == DBNull.Value || GridPo[j, i].Value.ToString() == String.Empty || Convert.ToDouble(GridPo["Adv Amnt", i].Value) == 0)
                        {
                            MessageBox.Show("' " + GridPo.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                            GridPo.CurrentCell = GridPo[j, i];
                            GridPo.Focus();
                            GridPo.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                        else
                        {
                            if (!checkBox1.Checked)
                            {
                                if (MyParent.UserCode != 1)
                                {
                                    if (Convert.ToDouble(GridPo["Adv Amnt", i].Value) > Convert.ToDouble(GridPo["Bal Amnt", i].Value))
                                    {
                                        MessageBox.Show("Advance Amount Should Be Less Than or Equal To The Bal Amnt", "Gainup");
                                        GridPo.CurrentCell = GridPo["Adv Amnt", i];
                                        GridPo.Focus();
                                        GridPo.BeginEdit(true);
                                        MyParent.Save_Error = true;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                
                DateTime c = DtpDate.Value;
                DateTime d = DtpClsDate.Value;
                d = d.AddDays(0);
                string cc;
                Console.WriteLine(d);
                Console.WriteLine(c);
                var t = (d - c).Days;
                if (t > 30)
                {
                    MessageBox.Show("Advance close date should be less than or equal to 30 days");
                        DtpClsDate.Focus();
                    return;
                }
                //Console.WriteLine(t);
                //cc = Console.ReadLine();


                if (DtpClsDate.Value <= DtpDate.Value)
                {
                    MessageBox.Show("Advance Close Date should be greater than entry date", "Gainup");
                    DtpClsDate.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (DtpPIDate.Value > DateTime.Now)
                {
                    MessageBox.Show("PI Date should be Less than or equal to the Current date", "Gainup");
                    DtpPIDate.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                DataTable Dtus = new DataTable();
                String Strus = "Select * From Vaahini_Erp_Gainup.Dbo.Spl_User Where Emplno = " + TxtReqBy.Tag + " ";
                MyBase.Load_Data(Strus, ref Dtus);

                DataTable Dtad = new DataTable();
                String StrAd = "Select * From Accounts.Dbo.Po_Advance_Payment_Status(1) Where Division = '" + TxtDivision.Text + "' And Req_By = " + TxtReqBy.Tag + " And Status = 'Over Due' ";
                MyBase.Load_Data(StrAd, ref Dtad);
                if (Dtus.Rows.Count > 0 && Dtad.Rows.Count > 0)//For Andavar, Ramalinkam, Sarvan, Venkatesh, Selvakumaresan
                {
                    DH = "Y";
                    MD = "N";
                    MessageBox.Show("Advance Over Due available for this Supplier. Kindly get approval from MD Sir for this advance entry");
                }
                else if (Dtus.Rows.Count <= 0 && Dtad.Rows.Count > 0)//Over All Due(Except Andavar)
                {

                    DataTable Dtad1 = new DataTable();
                    String StrAd1 = "Select * From Accounts.Dbo.Po_Advance_Payment_Status(1) Where Division = '" + TxtDivision.Text + "' And Req_By = " + TxtReqBy.Tag + " And Status = 'Over Due' And Sup_Id = " + TxtSupplier.Tag + " ";
                    MyBase.Load_Data(StrAd1, ref Dtad1);

                    if (Dtad1.Rows.Count > 0)//Supplierwise Due
                    {
                        DH = "Y";
                        MD = "N";
                        MessageBox.Show("Advance Over Due available for this Supplier. Kindly get approval from MD Sir for this advance entry");
                    }
                    else
                    {
                        DH = "N";
                        MD = "Y";

                        MessageBox.Show("Already Advance Over Due available. Kindly get approval from Division Head for this advance entry");
                    }
                }
                else
                {
                    DH = "Y";
                    MD = "Y";
                }

                                   

                if (MyParent._New)
                {
                    DataTable DE = new DataTable();
                    String StrE = String.Empty;

                    StrE = " Select Isnull(Max(Eno),0)+1 Eno From Vaahini_erp_Gainup.Dbo.Po_Advance_Master  ";
                    MyBase.Load_Data(StrE, ref DE);
                    TxtENo.Text = DE.Rows[0][0].ToString();
                }
                Queries = new string[Dt.Rows.Count + 100000];


                if (MyParent._New)
                {
                    if (!checkBox1.Checked)
                    {
                        Queries[Array_Index++] = "Insert into Vaahini_erp_Gainup.Dbo.Po_Advance_Master(Eno, Edate, User_Code, Comp_Code, Req_By, Ledger_Code, Old_Id, Close_Date, PI_No, Remarks, Tot_Adv, PI_Date, Adv_Per, PI_Amnt, Acc_Company_Code, MD, DH) values (" + TxtENo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + MyParent.UserCode + ", " + TxtDivision.Tag + ", " + TxtReqBy.Tag + ", " + TxtSupplier.Tag + ",  " + TxtFitSupplier.Tag + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpClsDate.Value) + "', '" + TxtPI.Text + "', '" + TxtRemarks.Text + "', " + TxtTotal.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpPIDate.Value) + "', " + TxtAdv.Text + ", " + TxtPIAmnt.Text + ", " + TxtCompany.Tag.ToString() + ", '" + MD + "', '" + DH + "'); Select Scope_Identity() ";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Vaahini_erp_Gainup.Dbo.Po_Advance_Master(Eno, Edate, User_Code, Comp_Code, Req_By, Ledger_Code, Old_Id, Close_Date, PI_No, Remarks, Tot_Adv, PI_Date, Adv_Per, PI_Amnt, Without_Po, Acc_Company_Code, MD, DH) values (" + TxtENo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + MyParent.UserCode + ", " + TxtDivision.Tag + ", " + TxtReqBy.Tag + ", " + TxtSupplier.Tag + ",  " + TxtFitSupplier.Tag + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpClsDate.Value) + "', '" + TxtPI.Text + "', '" + TxtRemarks.Text + "', " + TxtTotal.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpPIDate.Value) + "', " + TxtAdv.Text + ", " + TxtPIAmnt.Text + ", 'Y', " + TxtCompany.Tag.ToString() + ", '" + MD + "', '" + DH + "'); Select Scope_Identity() ";
                    }
                    Queries[Array_Index++] = MyParent.EntryLog("Eno", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Vaahini_erp_Gainup.Dbo.Po_Advance_Master Set PI_No = '" + TxtPI.Text + "',  Adv_Per = " + TxtAdv.Text + ", PI_Amnt = " + TxtPIAmnt.Text + ", Close_Date =  '" + String.Format("{0:dd-MMM-yyyy}", DtpClsDate.Value) + "', PI_Date =  '" + String.Format("{0:dd-MMM-yyyy}", DtpPIDate.Value) + "', Remarks = '" + TxtRemarks.Text + "',  Tot_Adv = " + TxtTotal.Text + " , User_Code=" + MyParent.UserCode + ", MD = '" + MD + "', DH = '" + DH + "' Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Po Advance Entry", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from VAAHINI_GAINUP_PHOTO.Dbo.Po_Advance_Photo Where MAster_ID = " + Code;
                    Queries[Array_Index++] = "Delete from Vaahini_erp_Gainup.Dbo.Po_Advance_Detail where Master_ID = " + Code;
                }

                for (int i = 0; i < GridPo.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Po_Date.Value = Convert.ToDateTime(GridPo["Po Date", i].Value.ToString());
                        Queries[Array_Index++] = "Insert into Vaahini_erp_Gainup.Dbo.Po_Advance_Detail (Master_Id, Sl_No, Po_No, Po_Date, Po_Id, Po_Amnt, Adv_Amnt) Values (@@IDENTITY,  " + GridPo["SNo", i].Value + ", '" + GridPo["Po_No", i].Value + "' , '" + String.Format("{0:dd-MMM-yyyy}", Po_Date.Value) + "', " + GridPo["Po_Id", i].Value + ", " + GridPo["Po Amnt", i].Value + ", " + GridPo["Adv Amnt", i].Value + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Vaahini_erp_Gainup.Dbo.Po_Advance_Detail (Master_Id, Sl_No, Po_No, Po_Date, Po_Id, Po_Amnt, Adv_Amnt) Values (" + Code + ", " + GridPo["SNo", i].Value + ", '" + GridPo["Po_No", i].Value + "' , '" + String.Format("{0:dd-MMM-yyyy}", Po_Date.Value) + "', " + GridPo["Po_Id", i].Value + ", " + GridPo["Po Amnt", i].Value + ", " + GridPo["Adv Amnt", i].Value + ")";
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

                 //Image Old
                String Str = String.Empty;

                if (PhImage.Image != null)
                {
                    MemoryStream DefaultStream = new MemoryStream();
                    Image NewImage = PhImage.Image;
                    Bitmap NewImage1 = new Bitmap(NewImage, new Size(240, 320));
                    Image B = (Image)NewImage1;
                    B.Save(DefaultStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    Byte[] Image1 = DefaultStream.ToArray();

                    DataTable TDt = new DataTable();
                    String Str1 = " Select IDENT_CURRENT('Vaahini_erp_Gainup.Dbo.Po_Advance_Master')  Identity_Mas";
                    MyBase.Load_Data(Str1, ref TDt);
                    if (MyParent._New == true)
                    {
                        Str = "Insert into VAAHINI_GAINUP_PHOTO.Dbo.Po_Advance_Photo (Master_ID, Photo) Values (" + TDt.Rows[0][0].ToString() + ",?) ";
                    }
                    else
                    {
                        Str = "Insert into VAAHINI_GAINUP_PHOTO.Dbo.Po_Advance_Photo (Master_ID, Photo) Values (" + Code + ",?) ";
                    }


                    MyBase.Cn_Open();
                    MyBase.ODBCCmd = new OdbcCommand();
                    MyBase.ODBCCmd.Connection = MyBase.Cn;
                    MyBase.ODBCCmd.Transaction = MyBase.ODBCTrans;
                    MyBase.ODBCCmd.CommandText = Str;
                    MyBase.ODBCCmd.Parameters.Add("@Photo", OdbcType.Image);
                    MyBase.ODBCCmd.Parameters["@Photo"].Value = Image1;
                    int Result = MyBase.ODBCCmd.ExecuteNonQuery();
                }


                MessageBox.Show("Saved ...!", "Gainup");
                MyParent.Save_Error = false;
                MyBase.Clear(this);
            }
            catch (Exception ex)
            {
                if (MyBase.ODBCTrans != null)
                {
                    MyBase.ODBCTrans.Rollback();
                }

                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (MyBase.Cn.State == ConnectionState.Open)
                {
                    MyBase.Cn_Close();
                }
            }
        }

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Po Advance Entry - View", " Select Eno, Edate, Division, Name, Supplier, Tot_Adv, Close_Date, PI_No, PI_Amnt, Adv_Per,  PI_Date, Remarks, Ledger_Code, Rowid, Req_By, Old_Id, Comp_Code, CompName, Acc_Company_Code From Vaahini_erp_Gainup.Dbo.Po_Advance_Entry_Edit_Fn() ", String.Empty, 90, 100, 90, 100, 120, 100, 100, 100, 100, 100, 125);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    MyParent.Load_DeleteConfirmEntry();
                    //Grid_Defect_Data();
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
                    MyBase.Run("Delete from VAAHINI_GAINUP_PHOTO.Dbo.Po_Advance_Photo where Master_ID = " + Code, "Delete from Vaahini_Erp_Gainup.Dbo.Po_Advance_Detail where Master_ID = " + Code, "Delete From Vaahini_Erp_Gainup.Dbo.Po_Advance_Master Where RowID = " + Code, MyParent.EntryLog("Po_Advance_Entry", "DELETE", Code.ToString()));
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Po Advance Entry - View", " Select Eno, Edate, Division, Name, Supplier, Tot_Adv, Close_Date, PI_No, PI_Amnt, Adv_Per,  PI_Date, Remarks, Ledger_Code, Rowid, Req_By, Old_Id, Comp_Code, CompName, Acc_Company_Code From Vaahini_erp_Gainup.Dbo.Po_Advance_Entry_View_Fn() ", String.Empty, 90, 100, 90, 100, 120, 100, 100, 100, 100, 100, 125);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    //Grid_Defect_Data();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmPoAdvanceEntry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == TxtCompany.Name)
                    {
                        if (TxtCompany.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Choose Company..!", "Gainup");
                            return;
                        }
                        else
                        {
                            TxtDivision.Focus();
                            return;
                        }
                    }
                    if (this.ActiveControl.Name == TxtDivision.Name)
                    {
                        if (TxtDivision.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Choose Division..!", "Gainup");
                            return;
                        }
                        else
                        {
                            TxtReqBy.Focus();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == TxtReqBy.Name)
                    {
                        if (TxtReqBy.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Choose Request Person..!", "Gainup");
                            return;
                        }
                        else
                        {
                            TxtSupplier.Focus();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == TxtSupplier.Name)
                    {
                        if (TxtSupplier.Text.ToString() == String.Empty || TxtReqBy.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Choose Supplier/Advance Request User..!", "Gainup");
                            return;
                        }
                        else
                        {
                            TxtPI.Focus();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == TxtPI.Name)
                    {
                        if (TxtPI.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Type PI Number..!", "Gainup");
                            return;
                        }
                        else
                        {
                            TxtPIAmnt.Focus();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == TxtPIAmnt.Name)
                    {
                        DtpPIDate.Focus();
                        return;
                    }
                    else if (this.ActiveControl.Name == DtpPIDate.Name)
                    {
                        DtpClsDate.Focus();
                        return;
                    }
                    else if (this.ActiveControl.Name == DtpClsDate.Name)
                    {
                        TxtAdv.Focus();
                        return;
                    }

                    else if (this.ActiveControl.Name == TxtAdv.Name)
                    {
                        GridPo.CurrentCell = GridPo["Po_No", 0];
                        GridPo.Focus();
                        GridPo.BeginEdit(true);
                        return;

                    }

                    else if (this.ActiveControl.Name == TxtTotal.Name)
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == TxtSupplier.Name)
                    {
                        if (MyParent._New)
                        {
                            if (checkBox1.Checked)//Without PO
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier..!", "Select Ledger_Name Supplier, Ledger_Name Fit_Supplier, Ledger_Code, Ledger_Code Old_Id From Accounts.Dbo.Ledger_Master Where Company_Code = " + TxtCompany.Tag.ToString() + " And YEAR_CODE = Dbo.Get_Accounts_YearCode(getdate()) And Ledger_Name not like '%ZZ%' Order By Ledger_Name  ", String.Empty, 200);
                            }
                            else//with po
                            {
                                if (TxtReqBy.Tag.ToString() != "4458")//Condition enabled for other user except(Andavar)
                                {
                                    if (TxtDivision.Tag.ToString() == "1")//Garments
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier..!", "Select Ledger_Name Supplier, Fit_Supplier, Ledger_Code, Old_Id From Fiterp1314.Dbo.Supplier_All_Fn()  ", String.Empty, 200);
                                    }
                                    else if (TxtDivision.Tag.ToString() == "2")//Socks
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier..!", "Select Ledger_Name Supplier, Fit_Supplier, Ledger_Code, Old_Id From Fitsocks.Dbo.Supplier_All_Fn()  ", String.Empty, 200);
                                    }
                                    else if (TxtDivision.Tag.ToString() == "3")//Gloves
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier..!", "Select Ledger_Name Supplier, Ledger_Name Fit_Supplier, Ledger_Code, Ledger_Code Old_Id From Gloves.Dbo.Supplier_All_Fn_Wc()  Where Company_Code = " + TxtCompany.Tag.ToString() + "   ", String.Empty, 200);
                                    }
                                    else if (TxtDivision.Tag.ToString() == "4")//Spinning
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier..!", "Select Ledger_Name Supplier, Ledger_Name Fit_Supplier, Ledger_Code, Ledger_Code Old_Id From Vaahini_Erp_Gainup.Dbo.All_Supplier_Fn_For_Advance()  ", String.Empty, 200);
                                    }
                                    else if (TxtDivision.Tag.ToString() == "5")//G-Printing
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier..!", "Select Ledger_Name Supplier, Fit_Supplier, Ledger_Code, Old_Id From Gar_Print.Dbo.Supplier_All_Fn()  ", String.Empty, 200);
                                    }
                                    else if (TxtDivision.Tag.ToString() == "6")//O-Printing
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier..!", "Select Ledger_Name Supplier, Fit_Supplier, Ledger_Code, Old_Id From Offset_Printing.Dbo.Supplier_All_Fn()  ", String.Empty, 200);
                                    }
                                    else if (TxtDivision.Tag.ToString() == "7")//Woven
                                    {
                                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier..!", "Select Ledger_Name Supplier, Ledger_Name Fit_Supplier, Ledger_Code,  Ledger_Code Old_Id From Woven.Dbo.Supplier_All_Fn_Wc()  Where Company_Code = " + TxtCompany.Tag.ToString() + "   ", String.Empty, 200);
                                    }
                                }
                                else//For Andavar
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier..!", "Select Ledger_Name Supplier, Ledger_Name Fit_Supplier, Ledger_Code, Ledger_Code Old_Id From Accounts.Dbo.Ledger_Master Where Company_Code = " + TxtCompany.Tag.ToString() + " And YEAR_CODE = Dbo.Get_Accounts_YearCode(getdate()) And Ledger_Name not like '%ZZ%' Order By Ledger_Name  ", String.Empty, 200);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Can't Change Supplier", "Gainup");
                            return;
                        }
                        
                        if (Dr != null)
                        {
                            
                            TxtSupplier.Text = Dr["Supplier"].ToString();
                            TxtSupplier.Tag = Dr["Ledger_Code"].ToString();
                            TxtFitSupplier.Text = Dr["Fit_Supplier"].ToString();
                            TxtFitSupplier.Tag = Dr["Old_Id"].ToString();
                            
                        }
                    }
                    else if (this.ActiveControl.Name == TxtReqBy.Name)
                    {

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Employee..!", "Select Employee, Emplno  From Vaahini_Erp_gainup.Dbo.Employee_Details_For_Adv_Entry() Where DivisionId = " + TxtDivision.Tag + " Order By Employee ", String.Empty, 200);
                        
                        if (Dr != null)
                        {
                            TxtReqBy.Text = Dr["Employee"].ToString();
                            TxtReqBy.Tag = Dr["Emplno"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == TxtCompany.Name)
                    {
                        if (MyParent._New)
                        {
                            if (TxtSupplier.Text == String.Empty)
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Company..!", "select Distinct CompName, Compcode from Accounts.Dbo.Companymas Order By 1 ", String.Empty, 350);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Can't Change Company", "Gainup");
                            return;
                        }
                        if (Dr != null)
                        {
                            TxtCompany.Text = Dr["CompName"].ToString();
                            TxtCompany.Tag = Dr["CompCode"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == TxtDivision.Name)
                    {
                        if(MyParent._New)
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Division..!", "select Division, Rowid from Vaahini_Erp_Gainup.Dbo.Division  Order By Division ", String.Empty, 200);
                        }
                        else
                        {
                            MessageBox.Show("Can't Change Division", "Gainup");
                            return;
                        }
                        if (Dr != null)
                        {
                            TxtDivision.Text = Dr["Division"].ToString();
                            TxtDivision.Tag = Dr["Rowid"].ToString();
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

        private void FrmPoAdvanceEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == String.Empty)
                    {
                    }
                    else if (this.ActiveControl.Name == TxtPI.Name || this.ActiveControl.Name == TxtRemarks.Name)
                    {

                    }
                    else if (this.ActiveControl.Name == TxtPIAmnt.Name || this.ActiveControl.Name == TxtAdv.Name)
                    {
                        MyBase.Valid_Decimal((TextBox)this.ActiveControl, e);
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Grid_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                GridPoItemDetails();
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
                    
                    if (GridPo.CurrentCell.ColumnIndex == GridPo.Columns["Adv Amnt"].Index)
                    {
                        if (GridPo["Adv Amnt", GridPo.CurrentCell.RowIndex].Value == null
                            || GridPo["Adv Amnt", GridPo.CurrentCell.RowIndex].Value == DBNull.Value
                            || GridPo["Adv Amnt", GridPo.CurrentCell.RowIndex].Value.ToString() == String.Empty
                            || Convert.ToDouble(GridPo["Adv Amnt", GridPo.CurrentCell.RowIndex].Value.ToString()) <= 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Adv Amnt...!", "Gainup");
                            GridPo.CurrentCell = GridPo["Adv Amnt", GridPo.CurrentCell.RowIndex];
                            GridPo.Focus();
                            GridPo.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            
                            //GridPoItemDetails();
                            //GridQty.CurrentCell = GridQty["Defect_Rowid", 0];
                            //GridQty.Focus();
                            //GridQty.BeginEdit(true);
                            //e.Handled = true;
                            //return;
                        }
                    }                    
                    Total_Count();
                }
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

        private void Grid_Leave(object sender, EventArgs e)
        {
            try
            {
                //for (int i = 0; i < Dt.Rows.Count; i++)
                //{
                //    for (int j = 0; j < i; j++)
                //    {
                //        if ((Grid["ITEM", i].Value.ToString()) == (Grid["ITEM", j].Value.ToString()) && (Grid["SIZE", i].Value.ToString()) == (Grid["SIZE", j].Value).ToString() && (Grid["COLOR", i].Value.ToString()) == (Grid["COLOR", j].Value).ToString() && (Grid["LOTNO", i].Value.ToString()) == (Grid["LOTNO", j].Value).ToString())
                //        {
                //            MessageBox.Show("Already Item, Size & Color Available", "Gainup");
                //            Grid["Stock_Qty", i].Value = "0";
                //            Grid["Moved_qty", i].Value = "0";
                //            i = Grid.Rows.Count;
                //            j = Grid.Rows.Count;
                //            Total_Count();
                //            Grid.CurrentCell = Grid["ITEM", j - 2];
                //            Grid.Focus();
                //            Grid.BeginEdit(true);
                //            return;
                //        }
                //    }
                //}
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
                MyBase.Row_Number(ref GridPo);
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
                //MyBase.Row_Number(ref Grid);
                //Total_Count();
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
                if (GridPo.CurrentCell.RowIndex <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Dt.Rows.RemoveAt(GridPo.CurrentCell.RowIndex);
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
                    Txt.Leave += new EventHandler(Txt_Leave);                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Txt_Leave(object sender, EventArgs e)
        {
            try
            {
                if (GridPo.CurrentCell.ColumnIndex == GridPo.Columns["Adv Amnt"].Index)
                {
                    if (GridPo["Adv Amnt", GridPo.CurrentCell.RowIndex].Value != null || GridPo["Adv Amnt", GridPo.CurrentCell.RowIndex].Value != DBNull.Value || GridPo["Adv Amnt", GridPo.CurrentCell.RowIndex].Value.ToString() != String.Empty || GridPo["Adv Amnt", GridPo.CurrentCell.RowIndex].Value != "0")
                    {
                        if (!checkBox1.Checked)
                        {
                            //if (MyParent.UserCode != 1)
                            //{
                                if (Convert.ToDouble(Txt.Text) > Convert.ToDouble(GridPo["Bal Amnt", GridPo.CurrentCell.RowIndex].Value))
                                {
                                    if (MyParent.UserCode != 1)
                                    {
                                        MessageBox.Show("Advance should be less than or equal to the Bal Amnt", "Gainup");
                                        Txt.Text = Convert.ToString(GridPo["Bal Amnt", GridPo.CurrentCell.RowIndex].Value);
                                        GridPo["Adv Amnt", GridPo.CurrentCell.RowIndex].Value = Convert.ToString(Txt.Text);
                                        GridPo.CurrentCell = GridPo["Adv Amnt", GridPo.CurrentCell.RowIndex];
                                        GridPo.Focus();
                                        GridPo.BeginEdit(true);
                                        return;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Advance amount greater than balance amount");
                                    }
                                }
                            //}
                            //else
                            //{
                            //    As per md sir instruction advance amount can be enter greater than balance amnt)
                            //    MessageBox.Show("Advance amount greater than balance amount");
                            //}
                        }
                        Total_Count();                     
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
                    if (GridPo.CurrentCell.ColumnIndex == GridPo.Columns["Po_No"].Index)
                    {
                        if (TxtSupplier.Text != String.Empty)
                        {
                            if (!checkBox1.Checked)//With PO
                            {
                                if (TxtReqBy.Tag.ToString() == "4458")//For Andavar
                                {
                                    Dr = Tool.Selection_Tool_Except_New("Po_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Po_No", " Select A1.Pur_Ord_No [Po_No], A1.orddate [Po Date],  A1.Amount [Po Amnt], Isnull(A2.Prv_Adv_Amnt,0) [Prv Adv], 0.00 [Adv Amnt], A1.Amount-Isnull(A2.Prv_Adv_Amnt,0) Bal_Amnt, A1.Pur_Ord_Id Po_Id From Vaahini_Erp_Gainup.Dbo.Po_Val_Fn() A1 Left Join Vaahini_Erp_Gainup.Dbo.Po_Adv_Fn()A2 on A1.Pur_Ord_No = A2.Po_No And A1.Pur_Ord_Id = A2.Po_Id Where (A1.Amount-Isnull(A2.Prv_Adv_Amnt,0))>0 And A1.Ledger_Code = " + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100, 100, 100, 100);
                                }
                                else if (TxtReqBy.Tag.ToString() == "21777" || TxtReqBy.Tag.ToString() == "31583" || TxtReqBy.Tag.ToString() == "29310")
                                {
                                    Dr = Tool.Selection_Tool_Except_New("Po_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Po_No", " Select A1.Pur_Ord_No [Po_No], A1.orddate [Po Date],  A1.Amount [Po Amnt], Isnull(A2.Prv_Adv_Amnt,0) [Prv Adv], 0.00 [Adv Amnt], A1.Amount-Isnull(A2.Prv_Adv_Amnt,0) Bal_Amnt, A1.Pur_Ord_Id Po_Id From Vaahini_Erp_Gainup.Dbo.Po_Val_Fn() A1 Left Join Vaahini_Erp_Gainup.Dbo.Po_Adv_Fn()A2 on A1.Pur_Ord_No = A2.Po_No And A1.Pur_Ord_Id = A2.Po_Id Where (A1.Amount-Isnull(A2.Prv_Adv_Amnt,0))>0 And A1.Ledger_Code = " + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100, 100, 100, 100);
                                }
                                else
                                {
                                    if (TxtDivision.Tag.ToString() == "1")//Garments
                                    {
                                        Dr = Tool.Selection_Tool_Except_New("Po_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Po_No", " Select A1.Pur_Ord_No [Po_No], A1.orddate [Po Date],  A1.Amount [Po Amnt], Isnull(A2.Prv_Adv_Amnt,0) [Prv Adv], 0.00 [Adv Amnt], A1.Amount-Isnull(A2.Prv_Adv_Amnt,0) Bal_Amnt, A1.Pur_Ord_Id Po_Id From Fiterp1314.Dbo.Po_Val_Fn() A1 Left Join Vaahini_Erp_Gainup.Dbo.Po_Adv_Fn()A2 on A1.Pur_Ord_No = A2.Po_No And A1.Pur_Ord_Id = A2.Po_Id Where (A1.Amount-Isnull(A2.Prv_Adv_Amnt,0))>0 And (Supplierid=" + TxtFitSupplier.Tag + " Or Supplierid = " + TxtSupplier.Tag + ") ", String.Empty, 100, 100, 100, 100, 100, 100);
                                    }
                                    else if (TxtDivision.Tag.ToString() == "2")//Socks
                                    {
                                        Dr = Tool.Selection_Tool_Except_New("Po_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Po_No", " Select A1.Pur_Ord_No [Po_No], A1.orddate [Po Date],  A1.Amount [Po Amnt], Isnull(A2.Prv_Adv_Amnt,0) [Prv Adv], 0.00 [Adv Amnt], A1.Amount-Isnull(A2.Prv_Adv_Amnt,0) Bal_Amnt, A1.Pur_Ord_Id Po_Id From Fitsocks.Dbo.Po_Val_Fn() A1 Left Join Vaahini_Erp_Gainup.Dbo.Po_Adv_Fn()A2 on A1.Pur_Ord_No = A2.Po_No And A1.Pur_Ord_Id = A2.Po_Id Where (A1.Amount-Isnull(A2.Prv_Adv_Amnt,0))>0 And A1.Ledger_Code = " + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100, 100, 100, 100);
                                    }
                                    else if (TxtDivision.Tag.ToString() == "3")//Gloves
                                    {
                                        Dr = Tool.Selection_Tool_Except_New("Po_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Po_No", " Select A1.Pur_Ord_No [Po_No], A1.orddate [Po Date],  A1.Amount [Po Amnt], Isnull(A2.Prv_Adv_Amnt,0) [Prv Adv], 0.00 [Adv Amnt], A1.Amount-Isnull(A2.Prv_Adv_Amnt,0) Bal_Amnt, A1.Pur_Ord_Id Po_Id From Gloves.Dbo.Po_Val_Fn() A1 Left Join Vaahini_Erp_Gainup.Dbo.Po_Adv_Fn()A2 on A1.Pur_Ord_No = A2.Po_No And A1.Pur_Ord_Id = A2.Po_Id Where (A1.Amount-Isnull(A2.Prv_Adv_Amnt,0))>0 And A1.Ledger_Code = " + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100, 100, 100, 100);
                                    }
                                    else if (TxtDivision.Tag.ToString() == "4")//Spinning
                                    {
                                        Dr = Tool.Selection_Tool_Except_New("Po_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Po_No", " Select A1.Pur_Ord_No [Po_No], A1.orddate [Po Date],  A1.Amount [Po Amnt], Isnull(A2.Prv_Adv_Amnt,0) [Prv Adv], 0.00 [Adv Amnt], A1.Amount-Isnull(A2.Prv_Adv_Amnt,0) Bal_Amnt, A1.Pur_Ord_Id Po_Id From Vaahini_Erp_Gainup.Dbo.Po_Val_Fn() A1 Left Join Vaahini_Erp_Gainup.Dbo.Po_Adv_Fn()A2 on A1.Pur_Ord_No = A2.Po_No And A1.Pur_Ord_Id = A2.Po_Id Where (A1.Amount-Isnull(A2.Prv_Adv_Amnt,0))>0 And A1.Ledger_Code = " + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100, 100, 100, 100);
                                    }
                                    else if (TxtDivision.Tag.ToString() == "5")//Garment Printing
                                    {
                                        Dr = Tool.Selection_Tool_Except_New("Po_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Po_No", " Select A1.Pur_Ord_No [Po_No], A1.orddate [Po Date],  A1.Amount [Po Amnt], Isnull(A2.Prv_Adv_Amnt,0) [Prv Adv], 0.00 [Adv Amnt], A1.Amount-Isnull(A2.Prv_Adv_Amnt,0) Bal_Amnt, A1.Pur_Ord_Id Po_Id From Gar_Print.Dbo.Po_Val_Fn() A1 Left Join Vaahini_Erp_Gainup.Dbo.Po_Adv_Fn()A2 on A1.Pur_Ord_No = A2.Po_No And A1.Pur_Ord_Id = A2.Po_Id Where (A1.Amount-Isnull(A2.Prv_Adv_Amnt,0))>0 And Supplierid=" + TxtFitSupplier.Tag + " ", String.Empty, 100, 100, 100, 100, 100, 100);
                                    }
                                    else if (TxtDivision.Tag.ToString() == "6")//Offset Printing
                                    {
                                        Dr = Tool.Selection_Tool_Except_New("Po_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Po_No", " Select A1.Pur_Ord_No [Po_No], A1.orddate [Po Date],  A1.Amount [Po Amnt], Isnull(A2.Prv_Adv_Amnt,0) [Prv Adv], 0.00 [Adv Amnt], A1.Amount-Isnull(A2.Prv_Adv_Amnt,0) Bal_Amnt, A1.Pur_Ord_Id Po_Id From OFFSET_PRINTING.Dbo.Po_Val_Fn() A1 Left Join Vaahini_Erp_Gainup.Dbo.Po_Adv_Fn()A2 on A1.Pur_Ord_No = A2.Po_No And A1.Pur_Ord_Id = A2.Po_Id Where (A1.Amount-Isnull(A2.Prv_Adv_Amnt,0))>0 And Supplierid=" + TxtFitSupplier.Tag + " ", String.Empty, 100, 100, 100, 100, 100, 100);
                                    }
                                    else if (TxtDivision.Tag.ToString() == "7")//Woven
                                    {
                                        Dr = Tool.Selection_Tool_Except_New("Po_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Po_No", " Select A1.Pur_Ord_No [Po_No], A1.orddate [Po Date],  A1.Amount [Po Amnt], Isnull(A2.Prv_Adv_Amnt,0) [Prv Adv], 0.00 [Adv Amnt], A1.Amount-Isnull(A2.Prv_Adv_Amnt,0) Bal_Amnt, A1.Pur_Ord_Id Po_Id From Woven.Dbo.Po_Val_Fn() A1 Left Join Vaahini_Erp_Gainup.Dbo.Po_Adv_Fn()A2 on A1.Pur_Ord_No = A2.Po_No And A1.Pur_Ord_Id = A2.Po_Id Where (A1.Amount-Isnull(A2.Prv_Adv_Amnt,0))>0 And Supplierid=" + TxtFitSupplier.Tag + " ", String.Empty, 100, 100, 100, 100, 100, 100);
                                    }
                                }
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool_Except_New("Po_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Po_No", " Select A1.Name [Po_No], Cast(getdate()as Date) [Po Date],  0.00 [Po Amnt], Sum(Isnull(A2.Prv_Adv_Amnt,0)) [Prv Adv], 0.00 [Adv Amnt], 0 Bal_Amnt, A1.Rowid Po_Id From Vaahini_Erp_Gainup.Dbo.Adv_Item_WithoutPO A1 Left Join Vaahini_Erp_Gainup.Dbo.WithoutPO_ADv()A2 on A1.Rowid = A2.Po_Id Where 1=1 Group By A1.Name, A1.Rowid ", String.Empty, 100, 100, 100, 100, 100, 100);
                            }
                            
                            if (Dr != null)
                            {
                                GridPo["Po_No", GridPo.CurrentCell.RowIndex].Value = Dr["Po_No"].ToString();
                                GridPo["Po Date", GridPo.CurrentCell.RowIndex].Value = Dr["Po Date"].ToString();
                                GridPo["Po Amnt", GridPo.CurrentCell.RowIndex].Value = Dr["Po Amnt"].ToString();
                                GridPo["Prv Adv", GridPo.CurrentCell.RowIndex].Value = Dr["Prv Adv"].ToString();
                                GridPo["Adv Amnt", GridPo.CurrentCell.RowIndex].Value = "0.00";
                                GridPo["Bal Amnt", GridPo.CurrentCell.RowIndex].Value = Dr["Bal_Amnt"].ToString();
                                GridPo["Po_Id", GridPo.CurrentCell.RowIndex].Value = Dr["Po_Id"].ToString();
                                //GridPo["Slno1", GridPo.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                                Txt.Text = Dr["Po_No"].ToString();
                                GridPoItemDetails();
                                Grid_Defect_Data();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Select Supplier", "Gainup");
                            TxtSupplier.Focus();
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
        Int16 Max_Slno_Grid()
        {
            Int16 No = 0;
            try
            {
                if (Dt.Rows.Count == 0)
                {
                    No = 1;
                    return No;
                }
                else
                {
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (No < Convert.ToInt16(Dt.Rows[i]["Slno1"]))
                        {
                            No = Convert.ToInt16(Dt.Rows[i]["Slno1"]);
                        }
                    }
                }
                No += 1;
                return No;
            }
            catch (Exception ex)
            {
                return No;
            }
        }

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GridPo.CurrentCell.ColumnIndex == GridPo.Columns["Adv Amnt"].Index)
                {                   
                    MyBase.Valid_Decimal(Txt, e);
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

        
        void Roll_Balance()
        {
            try
            {

                //if (TxtQty1.Text.Trim() == String.Empty)
                //{
                //    TxtQty1.Text = "0.00";
                //}

                //TxtEnteredPieces.Text = String.Format("{0:0.00}", Convert.ToDouble(MyBase.Sum(ref GridQty, "Trans_Qty", "RollNo")));

                //if (TxtEnteredPieces.Text.Trim() == String.Empty)
                //{
                //    TxtEnteredPieces.Text = "0.00";
                //}

                //TxtBalance.Text = String.Format("{0:0.00}", Convert.ToDouble(TxtQty1.Text) - Convert.ToDouble(TxtEnteredPieces.Text));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void GridPoItemDetails()
        {

            DataTable DtDet = new DataTable();
            try
            {
                if (TxtSupplier.Text.Trim() != String.Empty)
                {
                    if (GridPo.CurrentCell != null && GridPo.CurrentCell.Value != DBNull.Value && GridPo.CurrentCell.Value.ToString() != String.Empty && GridPo.CurrentCell.RowIndex < GridPo.Rows.Count)
                    {
                        if (!checkBox1.Checked)//With PO
                        {
                            if (TxtReqBy.Tag.ToString() == "4458")
                            {
                                MyBase.Load_Data("Select Item, Color, Size, Po_Qty, Rate, Po_Amnt  From Vaahini_Erp_Gainup.Dbo.Advance_Po_Details_Fn() Where Supplier_Code = " + TxtFitSupplier.Tag + " And Po_No = '" + GridPo["Po_No", GridPo.CurrentCell.RowIndex].Value + "' And Po_Id = " + GridPo["PO_Id", GridPo.CurrentCell.RowIndex].Value + "  Order By Item", ref DtDet);
                            }
                            else if (TxtReqBy.Tag.ToString() == "21777" || TxtReqBy.Tag.ToString() == "31583")
                            {
                                MyBase.Load_Data("Select Item, Color, Size, Po_Qty, Rate, Po_Amnt  From Vaahini_Erp_Gainup.Dbo.Advance_Po_Details_Fn() Where Supplier_Code = " + TxtFitSupplier.Tag + " And Po_No = '" + GridPo["Po_No", GridPo.CurrentCell.RowIndex].Value + "' And Po_Id = " + GridPo["PO_Id", GridPo.CurrentCell.RowIndex].Value + "  Order By Item", ref DtDet);
                            }
                            else
                            {
                                if (TxtDivision.Tag.ToString() == "1")//Garments
                                {
                                    MyBase.Load_Data("Select Item, Color, Size, Po_Qty, Rate, Po_Amnt  From Fiterp1314.Dbo.PO_Fn_For_Adv() Where (Supplierid = " + TxtFitSupplier.Tag + " Or Supplierid = " + TxtSupplier.Tag + ") And Po_No = '" + GridPo["Po_No", GridPo.CurrentCell.RowIndex].Value + "' And Po_Id = " + GridPo["PO_Id", GridPo.CurrentCell.RowIndex].Value + "  Order By Item", ref DtDet);
                                }
                                else if (TxtDivision.Tag.ToString() == "2")//Socks
                                {
                                    MyBase.Load_Data("Select Item, Color, Size, Po_Qty, Rate, Sum(Po_Val) Po_Amnt From Fitsocks.Dbo.Advance_Po_Details_Fn() Where Supplier_Code = " + TxtSupplier.Tag + " And PoNo = '" + GridPo["Po_No", GridPo.CurrentCell.RowIndex].Value + "' And Po_Id = " + GridPo["PO_Id", GridPo.CurrentCell.RowIndex].Value + " Group By Item, Color, Size, Po_Qty, Rate Order By Item", ref DtDet);
                                }
                                else if (TxtDivision.Tag.ToString() == "3")//Gloves
                                {
                                    MyBase.Load_Data("Select Item, Color, Size, Po_Qty, Rate, Sum(Po_Val) Po_Amnt From Gloves.Dbo.Advance_Po_Details_Fn() Where Supplier_Code = " + TxtSupplier.Tag + " And PoNo = '" + GridPo["Po_No", GridPo.CurrentCell.RowIndex].Value + "' And Po_Id = " + GridPo["PO_Id", GridPo.CurrentCell.RowIndex].Value + " Group By Item, Color, Size, Po_Qty, Rate Order By Item", ref DtDet);
                                }
                                else if (TxtDivision.Tag.ToString() == "4")//Spinning
                                {
                                    MyBase.Load_Data("Select Item, Color, Size, Po_Qty, Rate, Po_Amnt  From Vaahini_Erp_Gainup.Dbo.Advance_Po_Details_Fn() Where Supplier_Code = " + TxtFitSupplier.Tag + " And Po_No = '" + GridPo["Po_No", GridPo.CurrentCell.RowIndex].Value + "' And Po_Id = " + GridPo["PO_Id", GridPo.CurrentCell.RowIndex].Value + "  Order By Item", ref DtDet);
                                }
                                else if (TxtDivision.Tag.ToString() == "5")//G-Printing
                                {
                                    MyBase.Load_Data("Select Item, Color, Size, Po_Qty, Rate, Po_Amnt  From Gar_Print.Dbo.PO_Fn_For_Adv() Where Supplierid = " + TxtFitSupplier.Tag + " And Po_No = '" + GridPo["Po_No", GridPo.CurrentCell.RowIndex].Value + "' And Po_Id = " + GridPo["PO_Id", GridPo.CurrentCell.RowIndex].Value + "  Order By Item", ref DtDet);
                                }
                                else if (TxtDivision.Tag.ToString() == "6")//O-Printing
                                {
                                    MyBase.Load_Data("Select Item, Color, Size, Po_Qty, Rate, Po_Amnt  From Offset_Printing.Dbo.PO_Fn_For_Adv() Where Supplierid = " + TxtFitSupplier.Tag + " And Po_No = '" + GridPo["Po_No", GridPo.CurrentCell.RowIndex].Value + "' And Po_Id = " + GridPo["PO_Id", GridPo.CurrentCell.RowIndex].Value + "  Order By Item", ref DtDet);
                                }
                                else if (TxtDivision.Tag.ToString() == "7")//Woven
                                {
                                    MyBase.Load_Data("Select Item, Color, Size, Po_Qty, Rate, Sum(Po_Val) Po_Amnt From Woven.Dbo.Advance_Po_Details_Fn() Where Supplier_Code = " + TxtSupplier.Tag + " And PoNo = '" + GridPo["Po_No", GridPo.CurrentCell.RowIndex].Value + "' And Po_Id = " + GridPo["PO_Id", GridPo.CurrentCell.RowIndex].Value + " Group By Item, Color, Size, Po_Qty, Rate Order By Item", ref DtDet);
                                }
                            }
                        }
                        else
                        {
                            MyBase.Load_Data("Select '-' Item, '-' Color, '-' Size, 0 Po_Qty, 0 Rate, 0 Po_Amnt  ", ref DtDet);
                        }
                        Grid1.DataSource = MyBase.V_DataTable(ref DtDet);
                        MyBase.Grid_Designing(ref Grid1, ref DtDet);
                        MyBase.ReadOnly_Grid_Without(ref Grid1);
                        MyBase.Grid_Colouring(ref Grid1, Control_Modules.Grid_Design_Mode.Column_Wise);
                        MyBase.Grid_Width(ref Grid1, 140, 140, 120, 100, 100, 100);
                        Grid1.Columns["Po_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        Grid1.Columns["Rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        Grid1.Columns["Po_Amnt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                        
                        Grid1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        Grid1.RowHeadersWidth = 10;
                        MyBase.V_DataGridView(ref Grid1);
                    }
                    else
                    {
                        Grid1.DataSource = null;
                        DtDet = new DataTable();
                    }
                }                                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Grid_Defect_Data()
        {
            String Str = String.Empty;
            DataTable DtD = new DataTable();
            try
            {
                if (TxtDivision.Text == String.Empty || TxtReqBy.Text == String.Empty)
                {
                    Str = "Select Supplier, Pi_No, Close_Date, Amount_Issued-Closed Pending From Accounts.Dbo.Po_Advance_Payment_Status(1) Where 1= 2  ";
                }
                else
                {
                    Str = "Select Supplier, Pi_No, Close_Date, Amount_Issued-Closed Pending From Accounts.Dbo.Po_Advance_Payment_Status(1) Where Status = 'Over Due' And Division = '" + TxtDivision.Text + "'  And Name = '" + TxtReqBy.Text + "'  Order By Supplier, Close_Date  ";
                }

                Grid_Defect.DataSource = MyBase.Load_Data(Str, ref DtD);
                MyBase.Grid_Colouring(ref Grid_Defect, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid_Defect);
                MyBase.Grid_Designing(ref Grid_Defect, ref DtD);
                MyBase.Grid_Width(ref Grid_Defect, 175, 120, 100, 100);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                //{
                //    if (MessageBox.Show("Sure to Delete this ?", "Vaahini", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                //    {
                //        if (DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)] != null)
                //        {
                //            DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)] = null;
                //        }
                //        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                //        MyBase.Row_Number(ref Grid);
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void TxtRoll_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void GBMain_Enter(object sender, EventArgs e)
        {

        }
        

        void TxtRoll_GotFocus(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void TxtRoll_Leave(object sender, EventArgs e)
        {
            try
            {
                
                Total_Roll_Points();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

        void TxtRoll_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["Defect_Rowid"].Index)
                //{
                //    MyBase.Valid_Number(TxtRoll, e);
                //}
                //else
                //{
                //    e.Handled = true;
                //}


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
                TxtTotal.Text = MyBase.Sum(ref GridPo, "Adv Amnt", "Po_No");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Total_Roll_Points()
        {
            try
            {
                //TxtTotRollPoints.Text = MyBase.Sum(ref GridQty, "Points", "Defect_Rowid");                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GridQty_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (TxtRoll == null)
                {
                    TxtRoll = (TextBox)e.Control;
                    TxtRoll.KeyDown += new KeyEventHandler(TxtRoll_KeyDown);
                    TxtRoll.KeyPress += new KeyPressEventHandler(TxtRoll_KeyPress);
                    TxtRoll.Leave += new EventHandler(TxtRoll_Leave);
                    TxtRoll.GotFocus += new EventHandler(TxtRoll_GotFocus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridQty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyCode == Keys.Enter)
                //{
                    
                //    if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["Range"].Index)
                //    {
                //        if (GridQty["Range", GridQty.CurrentCell.RowIndex].Value == null || GridQty["Range", GridQty.CurrentCell.RowIndex].Value == DBNull.Value
                //            || GridQty["Range", GridQty.CurrentCell.RowIndex].Value.ToString() == "")
                            
                //        {
                //            e.Handled = true;
                //            MessageBox.Show("Invalid Range ...!", "Gainup");
                //            GridQty.CurrentCell = GridQty["Range", GridQty.CurrentCell.RowIndex];
                //            GridQty.Focus();
                //            GridQty.BeginEdit(true);
                //            return;
                //        }
                //        else
                //        {
                //            DataTable Dt1 = new DataTable();
                //            String Str1 = "Select Points From Fabric_Defect_Point_Slabs_Master Where Slaps = '" + GridQty["Range", GridQty.CurrentCell.RowIndex].Value + "' And Effect_From = (Select Max(Effect_From) From Fabric_Defect_Point_Slabs_Master)";
                //            MyBase.Load_Data(Str1, ref Dt1);
                //            if (Dt1.Rows.Count > 0)
                //            {
                //                TxtRoll.Text = Dt1.Rows[0]["Points"].ToString();
                //                GridQty["Points", GridQty.CurrentCell.RowIndex].Value = Dt1.Rows[0]["Points"].ToString();
                //            }
                //            else
                //            {
                //                e.Handled = true;
                //                MessageBox.Show("Invalid Points ...!", "Gainup");
                //                GridQty.CurrentCell = GridQty["Points", GridQty.CurrentCell.RowIndex];
                //                GridQty["Points", GridQty.CurrentCell.RowIndex].Value = 0;
                //                GridQty.Focus();
                //                GridQty.BeginEdit(true);
                //                return;
                //            }
                //        }
                //    }
                //    else if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["Defect_Rowid"].Index)
                //    {
                //        if (GridQty["Defect_Rowid", GridQty.CurrentCell.RowIndex].Value != null && GridQty["Defect_Rowid", GridQty.CurrentCell.RowIndex].Value != DBNull.Value
                //            && GridQty["Defect_Rowid", GridQty.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                //        {
                //            //for (int i = 0; i < GridQty.Rows.Count - 1; i++)
                //            //{
                //            //    if (i != GridQty.CurrentCell.RowIndex
                //            //        && GridQty["Defect_Rowid", i].Value.ToString() == GridQty["Defect_Rowid", GridQty.CurrentCell.RowIndex].Value.ToString())
                //            //    {
                //            //        MessageBox.Show("Defect RowID Already Avail...", "Gainup");
                //            //        GridQty.CurrentCell = GridQty["Defect_Rowid", GridQty.CurrentCell.RowIndex];
                //            //        GridQty["Defect_Rowid", GridQty.CurrentCell.RowIndex].Value = "";
                //            //        GridQty.Focus();
                //            //        GridQty.BeginEdit(true);
                //            //        return;   
                //            //    }
                //            //}
                //            DataTable Dt1 = new DataTable();
                //            Str = "Select Rowid Defect_Rowid, Name Defect From Fabric_Defect_Master Where Rowid = " + GridQty["Defect_Rowid", GridQty.CurrentCell.RowIndex].Value + " Order By Name ";
                //            MyBase.Load_Data(Str, ref Dt1);
                //            if (Dt1.Rows.Count > 0)
                //            {
                //                TxtRoll.Text = Dt1.Rows[0]["Defect_Rowid"].ToString();
                //                GridQty["Defect_Rowid", GridQty.CurrentCell.RowIndex].Value = Dt1.Rows[0]["Defect_Rowid"].ToString();
                //                GridQty["Defect", GridQty.CurrentCell.RowIndex].Value = Dt1.Rows[0]["Defect"].ToString();
                //                GridQty["Slno1", GridQty.CurrentCell.RowIndex].Value = GridPo["Slno1", GridPo.CurrentCell.RowIndex].Value.ToString();
                //            }
                //            else
                //            {
                //                MessageBox.Show("Invalid Defect RowID...!", "Gainup");
                //                TxtRoll.Text = "";
                //                GridQty["Defect_Rowid", GridQty.CurrentCell.RowIndex].Value = "";
                //                GridQty.CurrentCell = GridQty["Defect_Rowid", GridQty.CurrentCell.RowIndex];
                //                GridQty.Focus();
                //                GridQty.BeginEdit(true);
                //                return;   
                //            }
                //        }
                //    }
                //}
                //Roll_Balance();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        

        private void GridQty_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //MyBase.Grid_Delete(ref GridQty, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], GridQty.CurrentCell.RowIndex);
                //DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].AcceptChanges();
                //MyBase.Row_Number(ref GridCont);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //private void GridQty_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        //{
        //    try
        //    {
                
        //        if (GridQty.Rows.Count > 2)
        //        {
        //            MyBase.Row_Number(ref GridQty);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void GridQty_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        //{
            
        //    try
        //    {
        //        if (GridQty.Rows.Count > 2)
        //        {
        //            MyBase.Row_Number(ref GridQty);
        //        }
        //        Total_Roll_Points();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //}

        private void GridQty_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Total_Roll_Points();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if (GridPo.CurrentCell.ColumnIndex == GridPo.Columns["Roll_No"].Index)
                //{
                //    if (GridPo["Roll_No", GridPo.CurrentCell.RowIndex].Value == null || GridPo["Roll_No", GridPo.CurrentCell.RowIndex].Value == DBNull.Value || GridPo["Roll_No", GridPo.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                //    {
                //        //if (Grid.CurrentCell.RowIndex > 0)
                //        //{
                //        //    GridQty.DataSource = null;
                //        //    Dr = Tool.Selection_Tool_Except_New("Roll_No", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select RollNo", "Select RollNo Roll_No, Weight From Rollwise_Inward_Details_Fn_Qc() Where Order_No = '" + TxtOcn.Text + "' And LotNo='" + TxtLot.Text + "' And Itemid = " + TxtFabric.Tag + " And Colorid = " + TxtColor.Tag + "", String.Empty, 100, 100);
                //        //    if (Dr != null)
                //        //    {
                //        //        Grid["Roll_No", Grid.CurrentCell.RowIndex].Value = Dr["Roll_No"].ToString();
                //        //        Grid["Weight", Grid.CurrentCell.RowIndex].Value = Dr["WEight"].ToString();
                //        //        Grid["Shade", Grid.CurrentCell.RowIndex].Value = "A";
                //        //        Grid["Gsm", Grid.CurrentCell.RowIndex].Value = "150";
                //        //        Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                //        //        Txt.Text = Dr["Roll_No"].ToString();
                //        //    }
                //        //}
                //    }
                //    else
                //    {
                //        //if (Grid["Slno1", Grid.CurrentCell.RowIndex].Value != null && Grid["Slno1", Grid.CurrentCell.RowIndex].Value != DBNull.Value
                //        //&& Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                //        //{
                //        //    if (DtQty[Convert.ToInt64(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)] != null)
                //        //    {
                //        //        GridQty.DataSource = null;
                //        //        GridQty.DataSource = DtQty[Convert.ToInt64(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)];
                //        //        MyBase.Grid_Designing(ref GridQty, ref DtQty[Convert.ToInt64(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], "Master_Id", "Rowid", "SlNo1");
                //        //        MyBase.ReadOnly_Grid_Without(ref GridQty, "Defect_Rowid", "Points");
                //        //        MyBase.Grid_Colouring(ref GridQty, Control_Modules.Grid_Design_Mode.Column_Wise);
                //        //        MyBase.Grid_Width(ref GridQty, 40, 110, 225, 100, 150);
                //        //        GridQty.Columns["Points"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                //        //        GridQty.Columns["Defect"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                //        //        GridQty.Columns["Defect_Rowid"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //        //        GBQty.Visible = true;
                //        //        Total_Roll_Points();
                //        //    }
                //        //}
                //        //else
                //        //{
                //        //    GridQty.DataSource = null;
                //        //}
                //    }
                //}
                //if (Grid["Slno1", Grid.CurrentCell.RowIndex].Value != null && Grid["Slno1", Grid.CurrentCell.RowIndex].Value != DBNull.Value
                //    && Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                //{
                //    Grid_Data_Qty(Convert.ToInt64(Grid["Slno1", Grid.CurrentCell.RowIndex].Value));
                //    GridQty.CurrentCell = GridQty["Defect_Rowid", 1];
                //    GridQty.Focus();
                //    GridQty.BeginEdit(true);
                //    return;
                //}
                //Total_Count();

                //if (Grid["Slno1", Grid.CurrentCell.RowIndex].Value != null && Grid["Slno1", Grid.CurrentCell.RowIndex].Value != DBNull.Value
                //    && Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                //{
                //    if (DtQty[Convert.ToInt64(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)] != null)
                //    {
                //        GridQty.DataSource = null;
                //        GridQty.DataSource = DtQty[Convert.ToInt64(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)];
                //        MyBase.Grid_Designing(ref GridQty, ref DtQty[Convert.ToInt64(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], "Master_Id", "Rowid", "SlNo1");
                //        MyBase.ReadOnly_Grid_Without(ref GridQty, "Defect_Rowid", "Points");
                //        MyBase.Grid_Colouring(ref GridQty, Control_Modules.Grid_Design_Mode.Column_Wise);
                //        MyBase.Grid_Width(ref GridQty, 40, 110, 225, 100);
                //        GridQty.Columns["Points"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                //        GridQty.Columns["Defect"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                //        GridQty.Columns["Defect_Rowid"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //        GBQty.Visible = true;
                //    }
                //}
                //else
                //{
                //    GridQty.DataSource = null;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridQty_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                
                //if (GridQty.CurrentCell.RowIndex <= DtQty[Convert.ToInt64(GridPo["Slno1", GridPo.CurrentCell.RowIndex].Value.ToString())].Rows.Count)
                //{
                //    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                //    {
                //        DtQty[Convert.ToInt64(GridPo["Slno1", GridPo.CurrentCell.RowIndex].Value.ToString())].Rows.RemoveAt(GridQty.CurrentCell.RowIndex);
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtLot_Enter(object sender, EventArgs e)
        {
            try
            {
                //if (MyParent._New == true && this.ActiveControl.Name == TxtLot.Name && TxtLot.Text == String.Empty)
                //{
                //    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Lot No..!", "Select LotNo, Item, Color, Buyer, Order_No, Count(RollNo)No_Of_Rolls, Sum(Weight)Weight, Buyerid, Itemid, Colorid From Rollwise_Inward_Details_Fn_Qc() Group By LotNo, Item, Color, Buyer, Order_No, Buyerid, Itemid, Colorid ", String.Empty, 100, 200, 200, 200, 110, 100, 100);
                //    if (Dr != null)
                //    {
                //        TxtLot.Text = Dr["LotNo"].ToString();
                //        TxtFabric.Text = Dr["Item"].ToString();
                //        TxtColor.Text = Dr["Color"].ToString();
                //        TxtBuyer.Text = Dr["Buyer"].ToString();
                //        TxtOcn.Text = Dr["Order_No"].ToString();
                //        TxtRolls.Text = Dr["No_Of_Rolls"].ToString();
                //        TxtWeight.Text = Dr["Weight"].ToString();
                //        TxtBuyer.Tag = Dr["Buyerid"].ToString();
                //        TxtFabric.Tag = Dr["Itemid"].ToString();
                //        TxtColor.Tag = Dr["Colorid"].ToString();
                //    }
                //}
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
                if (GBImage.Visible)
                {
                    GBImage.Visible = false;
                }
                else
                {
                    GBImage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
