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
    public partial class FrmNRgpApprove : Form
    {
        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataRow Dr;
        TextBox Txt = null;
        String appName; String ModuleName;
        String Str;
        String[] Quries = null;
        
        Int64 Code = 0;
        DataTable Dts = new DataTable();
        String Division_Code=String.Empty;
        public FrmNRgpApprove()
        {
            InitializeComponent();
        }         

        private void ButExit_Click(object sender, EventArgs e)
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

        private void ButCancel_Click(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                 
                TxtRgpno.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButApprove_Click(object sender, EventArgs e)
        {
             
            try
            {
                
                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Details");
                    TxtRgpno.Focus();
                    return;
                }
                if (TxtRgpno.Text == String.Empty)
                {
                    MessageBox.Show("Invalid Rgpno...!");
                    TxtRgpno.Focus();
                    return;

                }

                Quries = new String[10];
                DialogResult m = MessageBox.Show("Referred By "+TxtRefName.Text.ToString().ToUpper()+"\n Sure to Approve...!", "NRgp Approve", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (m == DialogResult.Yes)
                {

                    MyBase.Execute("Update VAAHINI_ERP_GAINUP.dbo.NRGP_DCMASTER Set Approval_Status = 'T', First_Remarks = '" + Convert.ToString(TxtRemarks.Text) + "', first_approval_sys = HOST_NAME(), first_Approval_Time = GETDATE() Where RgpNO = '" + TxtRgpno.Text + "' and RgpDate =  '" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "'");
                    MessageBox.Show("Approved", "Gainup");
                    MyBase.Clear(this);
                    TxtRgpno.Focus();
                }
                else
                {
                    MyBase.Clear(this);
                    TxtRgpno.Focus();
                }
                 
                
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
                RbnCurno.Checked = false;
                RbnCuryes.Checked = false;
               
                ClearControls(GBMain, "Entry_New");



                GetServerDate();
                Grid_Data();
                
                LoadEmployee();
                TxtCourier.Enabled = false;
                TxtRgpno.Focus();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void GetServerDate()
        {
            try
            {


                RgpDate.MaxDate = Convert.ToDateTime(MyBase.GetServerDate()).Date;
                RefDate.MaxDate = Convert.ToDateTime(MyBase.GetServerDate()).Date;

                RgpDate.Value = Convert.ToDateTime(MyBase.GetServerDate()).Date;
                RefDate.Value = Convert.ToDateTime(MyBase.GetServerDate()).Date;


                 
                    RgpDate.Enabled = false;


                 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void ClearControls(GroupBox gbox, String Modename)
        {


            if (Modename.ToString().ToUpper() == "ENTRY_CLEAR")
            {

                foreach (Control ctrl in gbox.Controls)
                {
                    if (ctrl is CheckBox)
                    {

                        CheckBox checkBox = (CheckBox)ctrl;
                        checkBox.Enabled = false;
                        checkBox.Checked = false;
                    }

                }
                foreach (Control ctrl in gbox.Controls)
                {
                    if (ctrl is RadioButton)
                    {

                        RadioButton Rbn = (RadioButton)ctrl;
                        Rbn.Enabled = false;
                        Rbn.Checked = false;
                    }
                    if (ctrl is TextBox)
                    {

                        TextBox Txtbox = (TextBox)ctrl;
                        Txtbox.Enabled = false;
                        Txtbox.Text = String.Empty;
                    }


                }
            }
            if (Modename.ToString().ToUpper() == "ENTRY_NEW" || Modename.ToString().ToUpper() == "ENTRY_EDIT")
            {
                foreach (Control ctrl in gbox.Controls)
                {
                    if (ctrl is CheckBox)
                    {

                        CheckBox checkBox = (CheckBox)ctrl;
                        checkBox.Enabled = true;
                        checkBox.Checked = false;
                    }

                }
                foreach (Control ctrl in gbox.Controls)
                {
                    if (ctrl is RadioButton)
                    {

                        RadioButton Rbn = (RadioButton)ctrl;
                        Rbn.Enabled = true;
                        Rbn.Checked = false;
                    }
                    if (ctrl is TextBox)
                    {

                        TextBox Txtbox = (TextBox)ctrl;
                        Txtbox.Enabled = true;
                        Txtbox.Text = String.Empty;
                    }


                }

            }


        }

        public void Entry_Edit()
        {
            try
            {
                LoadEmployee();


                if (!String.IsNullOrEmpty(Division_Code.ToString().Trim()))
                {
                    if (MyParent.CompCode == 5 || MyParent.CompanyName.Contains("IRUL"))
                    {
                        Str = "Select rgpNO, rgpDATE, LedgerName, itemdesc, rgpQTY, uom ,EntryBy,RefBy,DivisionName,CompName,DESP ,SPLINST ,Courier_Mode,Emplno,LEDGERCODE,CompCode,Division,POTYPE,Sample_ID,Order_No,Color_ID,RowID,CourierCode,REFQUOTNO,REFQUOTDATE,Refbyemplno  From VAAHINI_ERP_GAINUP.dbo.Vaahini_NRgp_Fn() Where Entry_Cancel = 'F'  And Approval_status = 'F' And Approval_status1 = 'F' And  Division in(5)";
                    }
                    else
                    {
                        Str = "Select rgpNO, rgpDATE, LedgerName, itemdesc, rgpQTY, uom ,EntryBy,RefBy,DivisionName,CompName,DESP ,SPLINST ,Courier_Mode,Emplno,LEDGERCODE,CompCode,Division,POTYPE,Sample_ID,Order_No,Color_ID,RowID,CourierCode,REFQUOTNO,REFQUOTDATE,Refbyemplno  From VAAHINI_ERP_GAINUP.dbo.Vaahini_NRgp_Fn() Where Entry_Cancel = 'F'  And Approval_status = 'F' And Approval_status1 = 'F' And  Division in(" + Division_Code + ")";
                    }
                    Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "NRGP..!", Str, String.Empty, 100, 90, 180, 180, 100, 90, 150, 150, 130, 130);
                    if (Dr != null)
                    {

                        ClearControls(GBMain, "Entry_Edit");
                        FillDatas();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid UserSettings For This User..!");
                    return;
                }


                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void FillDatas()
        {
            try
            {
                GetServerDate();
                Code = Convert.ToInt64(Dr["Rowid"]);
                TxtRgpno.Text = Dr["rgpNO"].ToString();
                RgpDate.Value = Convert.ToDateTime(String.Format("{0:dd-MMM-yyyy}", Dr["rgpDATE"].ToString())).Date;
                TxtRefno.Text = Dr["REFQUOTNO"].ToString();
                RefDate.Value = Convert.ToDateTime(String.Format("{0:dd-MMM-yyyy}", Dr["REFQUOTDATE"].ToString())).Date;
                TxtsupplierName.Text = Dr["LedgerName"].ToString();
                TxtsupplierName.Tag = Dr["LEDGERCODE"].ToString();
                TxtDespatch.Text = Dr["DESP"].ToString();
                TxtNrgpRemarks.Text = Dr["SPLINST"].ToString();

                if (Dr["Courier_Mode"].ToString().ToUpper() == "Y")
                {

                    RbnCurno.Checked = false;
                    RbnCuryes.Checked = true;
                    TxtCourier.Enabled = true;
                }
                else
                {
                    RbnCurno.Checked = true;
                    RbnCuryes.Checked = false;
                    TxtCourier.Enabled = false;
                }

                Grid_Data();
                String SQl = String.Empty; DataTable Dt_Sql = new DataTable();
                SQl = "Select CompNAme,CompCode From VAAHINI_ERP_GAINUP.dbo.Stores_Companymas Where CompCode=" + Dr["CompCode"].ToString() + "";
                MyBase.Load_Data(SQl, ref Dt_Sql);
                if (Dt_Sql.Rows.Count > 0)
                {
                    TxtCompany.Text = Dt_Sql.Rows[0]["CompNAme"].ToString();
                    TxtCompany.Tag = Dt_Sql.Rows[0]["CompCode"].ToString();

                }
                SQl = "Select  Div_Name Division,Div_Code From VAAHINI_ERP_GAINUP.dbo.Rgp_Division() Where Div_Code=" + Dr["Division"].ToString() + "";
                MyBase.Load_Data(SQl, ref Dt_Sql);
                if (Dt_Sql.Rows.Count > 0)
                {
                    TxtDivision.Text = Dt_Sql.Rows[0]["Division"].ToString();
                    TxtDivision.Tag = Dt_Sql.Rows[0]["Div_Code"].ToString();

                    if (TxtDivision.Tag.ToString() == "2")
                    {

                        TxtSampleName.Enabled = true;
                        TxtOrderno.Enabled = true;
                        TxtColor.Enabled = true;
                    }
                    else
                    {
                        TxtSampleName.Enabled = false;
                        TxtOrderno.Enabled = false;
                        TxtColor.Enabled = false;
                    }

                }
                else
                {
                    TxtSampleName.Enabled = false;
                    TxtOrderno.Enabled = false;
                    TxtColor.Enabled = false;
                }

                SQl = "Select Type,typecode From (values(0,'GENERAL'),(1,'SAMPLE')) x(typecode,Type) Where Typecode=" + Dr["POTYPE"].ToString() + "";
                MyBase.Load_Data(SQl, ref Dt_Sql);
                if (Dt_Sql.Rows.Count > 0)
                {
                    TxtRgptype.Text = Dt_Sql.Rows[0]["Type"].ToString();
                    TxtRgptype.Tag = Dt_Sql.Rows[0]["typecode"].ToString();
                }

                if (RbnCuryes.Checked == true)
                {
                    SQl = "Select COURIERNAME,ccode CourierCode FRom VAAHINI_ERP_GAINUP.dbo.Sec_Mas_Courier Where ccode=" + Dr["CourierCode"].ToString() + "";
                    MyBase.Load_Data(SQl, ref Dt_Sql);
                    if (Dt_Sql.Rows.Count > 0)
                    {
                        TxtCourier.Text = Dt_Sql.Rows[0]["COURIERNAME"].ToString();
                        TxtCourier.Tag = Dt_Sql.Rows[0]["CourierCode"].ToString();


                    }
                }


                SQl = "Select A.Tno, A.Name,A.Emplno  From VAAHINI_ERP_GAINUP.dbo.EmployeeMas A  Where A.Emplno=" + Dr["Emplno"].ToString() + "";
                MyBase.Load_Data(SQl, ref Dt_Sql);
                if (Dt_Sql.Rows.Count > 0)
                {
                    TxtEmplname.Text = Dt_Sql.Rows[0]["Name"].ToString();
                    TxtEmplname.Tag = Dt_Sql.Rows[0]["Emplno"].ToString();
                    TxtTno.Text = Dt_Sql.Rows[0]["Tno"].ToString();
                }


                SQl = "Select A.Tno, A.Name,A.Emplno  From VAAHINI_ERP_GAINUP.dbo.EmployeeMas A  Where A.Emplno=" + Dr["Refbyemplno"].ToString() + "";
                MyBase.Load_Data(SQl, ref Dt_Sql);
                if (Dt_Sql.Rows.Count > 0)
                {
                    TxtRefName.Text = Dt_Sql.Rows[0]["Name"].ToString();
                    TxtRefName.Tag = Dt_Sql.Rows[0]["Emplno"].ToString();
                    TxtReftno.Text = Dt_Sql.Rows[0]["Tno"].ToString();
                }

                if (TxtDivision.Tag.ToString() == "2")
                {
                    SQl = "Select * From (Select 'Others' Order_No, '.' Color, 0 Color_Id Union Select ORder_No, Color, ColorID From VAAHINI_ERP_GAINUP.dbo.mis_order_details_Color()) S Where ORder_No='" + Dr["ORder_No"].ToString() + "'";
                    MyBase.Load_Data(SQl, ref Dt_Sql);
                    if (Dt_Sql.Rows.Count > 0)
                    {
                        TxtOrderno.Text = Dt_Sql.Rows[0]["Order_No"].ToString();
                        TxtColor.Text = Dt_Sql.Rows[0]["Color"].ToString();
                        TxtColor.Tag = Dt_Sql.Rows[0]["Color_Id"].ToString();

                    }

                    SQl = "select Name, Rowid From VAAHINI_ERP_GAINUP.dbo.Sample_Name_MAster Where Rowid=" + Dr["Sample_ID"] + "";
                    MyBase.Load_Data(SQl, ref Dt_Sql);
                    if (Dt_Sql.Rows.Count > 0)
                    {
                        TxtSampleName.Text = Dt_Sql.Rows[0]["Name"].ToString();
                        TxtSampleName.Tag = Dt_Sql.Rows[0]["Rowid"].ToString();
                    }
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
                Grid.AllowUserToAddRows = false;
                Str = "Select 0 SNO,itemdesc ITEMDESCRIPTION,purpose PURPOSE,rgpQTY RGPQTY,UOM, Rate AMOUNT From VAAHINI_ERP_GAINUP.dbo.NRGP_DCDETAIL Where Rgpno='" + TxtRgpno.Text + "'";
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                //MyBase.Grid_Designing(ref Grid, ref Dt, "IND.NO", "UNIT", "ITEMDESCRIPTION", "CENVAT(Y/N)", "DISAMT", "DUTYAMT", "AEDAMT", "STAMT", "SC%", "SCAMT", "CANCELQTY", "PRATE", "PORATE", "DECPL", "IND.PENQTY", "RECQTY", "FRIEGHT", "UFRIEGHT", "DIFFENCE", "UDIFFAMT", "INDDATE", "DUTYAMT2", "UDUTYAMT2", "BILLPDATE", "DUEDATE", "UADDAMT", "ULESSAMT", "ST%", "DUTY%", "T");
                MyBase.ReadOnly_Grid(ref Grid, "SNO");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                Grid.RowHeadersWidth = 10;
                MyBase.Grid_Width(ref Grid, 50, 500, 250, 100, 100, 120);
                for (int i = 0; i <= Dt.Columns.Count - 1; i++)
                {
                    if (Grid.Columns[i].Visible == true)
                    {
                        Grid.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

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
                if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    BtnApprove.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmNRgpApprove_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                MyBase.Clear(this);
                appName = "/" + System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";
                ModuleName = appName.Substring(appName.IndexOf("/") + 1, appName.IndexOf(".") - 1);
                Entry_New();             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void LoadEmployee()
        {
            try
            {

                if (MyParent.UserCode == 1)
                {
                    Division_Code = "0,3,1,4,5,2";
                }
                else
                {
                    DataTable Dt2 = new DataTable();
                    String Str1 = "Select A.Rowid,A.Emplno,A.Usercode,A.AppUserCode,A.Division,A.CompCode,A.Type,A.Module,A.Enable_Mode From VAAHINI_ERP_GAINUP.dbo.NrgpRgp_Users_Login A Where Emplno=" + MyParent.Emplno + "";
                    MyBase.Load_Data(Str1, ref Dt2);
                    if (Dt2.Rows.Count > 0)
                    {
                        Division_Code = Dt2.Rows[0]["Division"].ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

         

        private void FrmNRgpApprove_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name != "TxtRemarks")
                    {
                        e.Handled = true;
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
                {
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtRgpno")
                    {

                        Entry_Edit();

                        
                        
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

        private void FrmNRgpApprove_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name =="TxtRemarks")
                    {
                        MyBase.Return_Ucase(e);
                    }
                    else
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

        private void BtnCtl_EntryCancel_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Details");
                    TxtRgpno.Focus();
                    return;
                }
                if (TxtRgpno.Text == String.Empty)
                {
                    MessageBox.Show("Invalid NRgpno...!");
                    TxtRgpno.Focus();
                    return;

                }

                Quries = new String[10];
                DialogResult m = MessageBox.Show("Sure to Cancel...!", "NRgp Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (m == DialogResult.Yes)
                {
                    MyBase.Execute("Update NRGP_DCMASTER Set Entry_Cancel = 'T' , Cancel_Remarks = '" + Convert.ToString(TxtRemarks.Text) + "', Cancel_system = HOST_NAME(), Cancel_Date = GETDATE()  Where Approval_Status = 'F' and Approval_Status1 = 'F' and Entry_Cancel = 'F' and RgpNO = '" + TxtRgpno.Text + "' and RgpDate =  '" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "'");
                    MessageBox.Show("Canceled..!", "Gainup");
                    MyParent.Save_Error = false;
                    MyBase.Clear(this);
                    TxtRgpno.Focus();
                    ClearControls(GBMain, "Entry_New");

                }
                else
                {
                    MyBase.Clear(this);
                    TxtRgpno.Focus();
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

       
      
    }
}