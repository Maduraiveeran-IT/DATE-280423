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
    public partial class FrmNRgpCancel : Form
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        DataTable Dt = new DataTable();
        DataRow Dr;
        Int16 PCompCode;
        String Str;
        public FrmNRgpCancel()
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

        private void ButClear_Click(object sender, EventArgs e)
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

        private void ButCancel_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            try
            {
                if (TxtRgpNo.Text.ToString() != String.Empty)
                {
                    MyBase.Execute(" Update VAAHINI_ERP_GAINUP.dbo.NRGP_DCMASTER Set Entry_Cancel = 'T' , Cancel_Remarks = '" + TxtRemarks.Text.ToString() + "', Cancel_system = HOST_NAME(), Cancel_Date = GETDATE()  Where Division = 1 and Approval_Status = 'F' and Approval_Status1 = 'F' and Entry_Cancel = 'F' and RgpNO = '" + TxtRgpNo.Text + "' and RgpDate =  '" + String.Format("{0:dd-MMM-yyyy}", DtpRDate.Value) + "' ");
                    MessageBox.Show("Cancel", "Gainup");
                    MyBase.Clear(this);
                    TxtRgpNo.Focus();
                }
                else
                {
                    MessageBox.Show("Invalid NRGPNo", "Gainup");
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
                    ButCancel.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmNRgpCancel_Load(object sender, EventArgs e)
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
                Str = " SELECT  NRGP_DCDETAIL.SLNO SNO, NRGP_DCDETAIL.itemdesc DESCRIPTION, NRGP_DCDETAIL.uom UOM, NRGP_DCDETAIL.rgpQTY QTY FROM (VAAHINI_ERP_GAINUP.dbo.NRGP_DCMASTER LEFT JOIN VAAHINI_ERP_GAINUP.dbo.NRGP_DCDETAIL ON (NRGP_DCMASTER.rgpNO = NRGP_DCDETAIL.rgpno) AND (NRGP_DCMASTER.rgpDATE = NRGP_DCDETAIL.RGPDATE)) LEFT JOIN VAAHINI_ERP_GAINUP.dbo.Ledger_Master (" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Ledger_Master ON NRGP_DCMASTER.LEDGERCODE = Ledger_Master.LedgerCode   where NRGP_DCMASTER.Division = 1 and NRGP_DCMASTER.Approval_Status = 'F' and NRGP_DCMASTER.Approval_Status = 'F' and NRGP_DCMASTER.Entry_Cancel = 'F' and NRGP_DCMASTER.RgpNo = '" + TxtRgpNo.Text + "'  ORDER BY  Nrgp_dcmaster.rgpdate desc,Nrgp_dcmaster.rgpno  DESC , NRGP_DCDETAIL.SLNO";                
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

        private void FrmNRgpCancel_KeyDown(object sender, KeyEventArgs e)
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
                    if (this.ActiveControl.Name == "TxtRgpNo")
                    {

                        Str = "SELECT NRGP_DCMASTER.rgpNO RGPNO, NRGP_DCMASTER.rgpDATE RGPDATE,case when NRGP_DCMASTER.Courier_Mode='Y' Then 'YES' when NRGP_DCMASTER.Courier_Mode='N' Then 'NO' end COURIER, (Case When  NRGP_DCMASTER.LedgerCode = 0 Then NRGP_DCMASTER.PartyName Else Ledger_Name End) PARTY, NRGP_DCDETAIL.SLNO, NRGP_DCDETAIL.itemdesc DESCRIPTION, NRGP_DCDETAIL.uom UOM, NRGP_DCDETAIL.rgpQTY QTY,isnull(EmployeeMas.NAME,'-')NAME ,  NRGP_DCMASTER.Splinst REMARKS,  NRGP_DCMASTER.Desp DESP  FROM (VAAHINI_ERP_GAINUP.dbo.NRGP_DCMASTER LEFT JOIN VAAHINI_ERP_GAINUP.dbo.NRGP_DCDETAIL ON (NRGP_DCMASTER.rgpNO = NRGP_DCDETAIL.rgpno) AND (NRGP_DCMASTER.rgpDATE = NRGP_DCDETAIL.RGPDATE))  LEFT JOIN Accounts.dbo.Ledger_Master ON NRGP_DCMASTER.LEDGERCODE = Ledger_Master.Ledger_Code  and Ledger_Master.Year_Code = dbo.Get_Accounts_YearCode(getdate()) and Ledger_Master.Company_Code = CAse When NRGP_DCMASTER.CompCode in (1,2,10) Then  1 When NRGP_DCMASTER.CompCode in (3,4) Then 2 Else 3 End left join VAAHINI_ERP_GAINUP.dbo.EmployeeMas on EmployeeMas.Emplno=NRGP_DCMASTER.Emplno  where NRGP_DCMASTER.Division in (1,3) and NRGP_DCMASTER.Approval_Status = 'F' and NRGP_DCMASTER.Approval_Status1 = 'F' and NRGP_DCMASTER.Entry_Cancel = 'F' ORDER BY  Nrgp_dcmaster.rgpdate desc,Nrgp_dcmaster.rgpno  DESC , NRGP_DCDETAIL.SLNO";
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select NRGPNo", Str, string.Empty, 120, 120, 100, 300, 80, 200, 120, 150);
                        if (Dr != null)
                        {
                            TxtRgpNo.Text = Dr["RGPNO"].ToString();
                            DtpRDate.Value = Convert.ToDateTime(Dr["RGPDATE"].ToString());
                            TxtParty.Text = Dr["PARTY"].ToString();
                            TxtRgpRemarks.Text = Dr["REMARKS"].ToString();
                            TxtDesp.Text = Dr["Desp"].ToString();
                            Txt_Employee.Text = Dr["NAME"].ToString();
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

        private void FrmNRgpCancel_KeyPress(object sender, KeyPressEventArgs e)
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