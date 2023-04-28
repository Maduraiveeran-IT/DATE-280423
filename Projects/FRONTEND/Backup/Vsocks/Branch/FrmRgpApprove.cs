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
    public partial class FrmRgpApprove : Form
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        DataTable Dt = new DataTable();
        DataRow Dr;
        Int16 PCompCode;
        public FrmRgpApprove()
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
                TxtRgpNo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButApprove_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            try
            {
                if (TxtRgpNo.Text.ToString() != String.Empty)
                {
                    MyBase.Execute("Update VAAHINI_ERP_GAINUP.dbo.RGP_DCMASTER  Set Approval_Status = 'T', First_Remarks = '" + TxtRemarks.Text.ToString() + "', first_approval_sys = HOST_NAME(), first_Approval_Time = GETDATE()  Where  Entry_Cancel = 'F' and RgpNO = '" + TxtRgpNo.Text + "' and RgpDate =  '" + String.Format("{0:dd-MMM-yyyy}", DtpRDate.Value) + "' ");
                    MessageBox.Show("Approved", "Gainup");
                    MyBase.Clear(this);
                    TxtRgpNo.Focus();
                }
                else
                {
                    MessageBox.Show("Invalid RGPNo", "Gainup");
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
                    ButApprove.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmRgpApprove_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);               
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
                Str = " SELECT  B.SLNO SNO, B.itemdesc DESCRIPTION, B.uom UOM, B.rgpQTY QTY  FROM (VAAHINI_ERP_GAINUP.dbo.RGP_DCMASTER A LEFT JOIN VAAHINI_ERP_GAINUP.dbo.RGP_DCDETAIL B ON (A.rgpNO = B.rgpno) AND (A.rgpDATE = B.RGPDATE))  LEFT JOIN VAAHINI_ERP_GAINUP.dbo.Ledger_Master (" + MyParent.CompCode + ", '" + MyParent.YearCode + "') C ON A.LEDGERCODE = C.LedgerCode    where  A.Entry_Cancel = 'F' and A.Approval_status = 'F' and A.RgpNo = '" + TxtRgpNo.Text + "'  ORDER BY  A.rgpdate desc,A.rgpno  DESC , B.SLNO ";
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);                            
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);                
                MyBase.ReadOnly_Grid(ref Grid, "SNO", "DESCRIPTION", "UOM", "QTY");                
                MyBase.Grid_Width(ref Grid, 50, 200, 120 , 150);
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["DESCRIPTION"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["UOM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;              
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmRgpApprove_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
                {
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtRgpNo")
                    {
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select RGPNo", " SELECT RGP_DCMASTER.rgpNO RGPNO, RGP_DCMASTER.rgpDATE RGPDATE, case when RGP_DCMASTER.Courier_Mode='y' Then 'YES' when RGP_DCMASTER.Courier_Mode='N' Then 'NO' end COURIER, Ledger_Master.Ledger_Name PARTY, RGP_DCDETAIL.SLNO, RGP_DCDETAIL.itemdesc DESCRIPTION, RGP_DCDETAIL.uom UOM, RGP_DCDETAIL.rgpQTY QTY,isnull(EmployeeMas.Name,'-')NAME, RGP_DCMASTER.Splinst REMARKS,  RGP_DCMASTER.Desp DESP  FROM (VAAHINI_ERP_GAINUP.dbo.RGP_DCMASTER RGP_DCMASTER LEFT JOIN VAAHINI_ERP_GAINUP.dbo.RGP_DCDETAIL RGP_DCDETAIL ON (RGP_DCMASTER.rgpNO = RGP_DCDETAIL.rgpno) AND (RGP_DCMASTER.rgpDATE = RGP_DCDETAIL.RGPDATE)) LEFT JOIN Accounts.dbo.Ledger_Master Ledger_Master ON RGP_DCMASTER.LEDGERCODE = Ledger_Master.Ledger_Code  and Ledger_Master.Year_Code = dbo.Get_Accounts_YearCode(getdate()) and Ledger_Master.Company_Code = CAse When RGP_DCMASTER.CompCode in (1,2,10) Then  1 When RGP_DCMASTER.CompCode in (3,4) Then 2 Else 3 End left join VAAHINI_ERP_GAINUP.dbo.EmployeeMas EmployeeMas on EmployeeMas.Emplno=RGP_DCMASTER.Emplno  Where  RGP_DCMASTER.Entry_Cancel = 'F' and RGP_DCMASTER.Approval_status = 'F' and RGP_DCMASTER.Division in  (1,3) ORDER BY  rgp_dcmaster.rgpdate desc,rgp_dcmaster.rgpno  DESC , RGP_DCDETAIL.SLNO ", string.Empty, 120, 120, 100, 300, 80, 200, 120, 150);
                        if (Dr != null)
                        {
                            TxtRgpNo.Text = Dr["RGPNO"].ToString();
                            DtpRDate.Value = Convert.ToDateTime(Dr["RGPDATE"].ToString());
                            TxtParty.Text = Dr["PARTY"].ToString();
                            TxtRgpRemarks.Text = Dr["REMARKS"].ToString();
                            TxtDesp.Text = Dr["Desp"].ToString();
                            Txt_Employee.Text=Dr["NAME"].ToString();
                            Grid_Data();
                            TxtRemarks.Focus();
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

        private void FrmRgpApprove_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtRemarks")
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
      
    }
}