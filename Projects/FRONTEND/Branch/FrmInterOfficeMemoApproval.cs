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

namespace Accounts
{
    public partial class FrmInterOfficeMemoApproval : Form
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();        
        DataGrid dgv = new DataGrid();
        DataRow Dr;       
        TextBox Txt = null;
        String[] Queries;
        Int64 Code;
        Double TaxAmt;
        Double NetAmt;
        Double c;
        public FrmInterOfficeMemoApproval()
        {
            InitializeComponent();
        }
       
       
        void Fill_Datas(DataRow Dr)
        {
            try
            {
                Code  = Convert.ToInt64(Dr["RowID"]);
                TxtENo.Tag = Convert.ToInt64(Dr["RowID"]);
                TxtENo.Text = Dr["Entry_No"].ToString();                
                DtpDate.Value = Convert.ToDateTime(Dr["EDate"]);
                TxtDepartment.Text = Dr["Department"].ToString();
                TxtName.Text = Dr["Name"].ToString();
                TxtName.Tag = Dr["EmplNo"].ToString();                                
                TxtTNo.Text = Dr["TNo"].ToString();
                TxtDesignation.Text = Dr["Designation"].ToString();
                TxtBuyer.Text = Dr["Buyer"].ToString();                
                TxtOrderNo.Text = Dr["Order_No"].ToString();
                TxtSubject.Text = Dr["Subject"].ToString();
                TxtActualLoss.Text = Dr["Loss_Amount"].ToString();
                TxtDebitAmount.Text = Dr["Debit_Amount"].ToString();
                TxtDebitAmount.Tag = Dr["Debit_Mode"].ToString();
                TxtCAP.Text = Dr["CAP"].ToString();
                TxtDescription.Text = Dr["Description"].ToString();
                TxtMemoType.Text = Dr["Memo_Type"].ToString();
                TxtMemoType.Tag = Dr["Action_ID"].ToString();
                TxtAction.Text = Dr["Decision"].ToString();
                TxtAction.Tag = Dr["Decision_ID"].ToString();
                //if (Dr["Debit_Mode"].ToString() == "Y")
                //{
                //    ChkDebit.Checked = true;
                //    TxtDebitAmount.Enabled = true;
                //}
                //else
                //{
                //    ChkDebit.Checked = false;
                //    TxtDebitAmount.Enabled = false;
                //}            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        private void FrmInterOfficeMemoApproval_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                MyBase.Clear(this);
                TxtENo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmInterOfficeMemoApproval_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtActualLoss" || this.ActiveControl.Name == "TxtDebitAmount")
                    {
                        MyBase.Valid_Decimal((TextBox)this.ActiveControl, e);
                    }
                    else if (this.ActiveControl.Name != "TxtCAP" )
                    {
                        MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                    }                    
                    else
                    {
                        e.Handled = false;
                    }
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmInterOfficeMemoApproval_KeyDown(object sender, KeyEventArgs e)
        {          
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtDebitAmount")
                    {
                        ButOK.Focus();
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }                 
                else if  (e.KeyCode == Keys.Down)
                {                    
                        if (this.ActiveControl.Name == "TxtENo")
                        {
                            Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Memo", " Select A.Entry_No , A.EDate, B.Name, B.TNo, E.Order_No, E.Buyer, A.Subject, A.Description, A.CAP, A.Loss_Amount, A.Debit_Mode, A.Debit_Amount, C.DeptName Department, D.DesignationName Designation, IsNull(F.Name, '') Memo_Type, IsNull(G.Name, '') Decision, A.EmplNo, A.RowID, A.ACtion_ID , A.Decision_ID  From Vaahini_Erp_Gainup.Dbo.Inter_Office_Memo_Details A LEft Join Vaahini_Erp_Gainup.Dbo.EmployeeMas B On A.EmplNo = B.EmplNo LEft Join Vaahini_Erp_Gainup.Dbo.DeptType C On B.Deptcode = C.DeptCode and C.CompCode = B.CompCode Left Join Vaahini_Erp_Gainup.Dbo.DesignationType D On B.DesignationCode = D.DesignationCode and B.Compcode = D.CompCode Left Join Vaahini_Erp_Gainup.Dbo.Fit_OrderNos() E On A.Order_No = E.Order_NO  Left Join Vaahini_Erp_Gainup.Dbo.Memo_Action_Name F On A.Action_ID = F.RowID LEft Join Vaahini_Erp_Gainup.Dbo.Memo_Action_Decision_Name G On A.Decision_ID = G.RowID Where A.Approval_Flag = 'N' and Division_Id = 2 and Memo_Type = 'SOCKS'", String.Empty, 80, 100, 130, 100, 120, 150);
                            if (Dr != null)
                            {
                                Fill_Datas(Dr);
                                TxtENo.Focus();
                            }
                        }
                        else if (this.ActiveControl.Name == "TxtMemoType")
                        {
                            Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select MemoType", " Select Name, RowID  from Vaahini_Erp_Gainup.Dbo.Memo_Action_Name  Order by RowID ", String.Empty, 210);
                            if (Dr != null)
                            {
                                TxtMemoType.Text = Dr["Name"].ToString();
                                TxtMemoType.Tag = Dr["RowID"].ToString();
                            }
                        }
                        else if (this.ActiveControl.Name == "TxtAction")
                        {
                            if (TxtMemoType.Text.ToString() != String.Empty)
                            {
                                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select MemoType", " Select Name, RowID  from Vaahini_Erp_Gainup.Dbo.Memo_Action_Decision_Name Where Action_Id = " + TxtMemoType.Tag + "  Order by RowID ", String.Empty, 400);
                                if (Dr != null)
                                {
                                    TxtAction.Text = Dr["Name"].ToString();
                                    TxtAction.Tag = Dr["RowID"].ToString();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid MemoType", "Gainup");
                                TxtMemoType.Focus();
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

          private void ButCancel_Click(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);                               
                TxtENo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

       private void ButOk_Click(object sender, EventArgs e)
        {
            try
            {
               
                    if (TxtENo.Text.ToString() != String.Empty)
                    {
                        if (TxtActualLoss.Text.ToString() == String.Empty)
                        {
                            TxtActualLoss.Text = "0";
                        }
                        if (TxtAction.Text.ToString() == String.Empty)
                        {
                            TxtAction.Tag = 0;
                        }
                        if (TxtMemoType.Text.ToString() == String.Empty)
                        {
                            TxtMemoType.Tag = 0;
                        }
                        if (TxtDebitAmount.Text.ToString() == String.Empty)
                        {
                            TxtDebitAmount.Text = "0";
                            TxtDebitAmount.Tag = "N";
                        }
                        else
                        {
                            TxtDebitAmount.Tag = "Y";
                        }
                        if (MessageBox.Show("Sure To Approve ...!", " Approve ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            MyBase.Run("Update Vaahini_Erp_Gainup.Dbo.Inter_Office_Memo_Details Set  Approval_Flag = 'Y', Approval_Time = Getdate(), Action_ID = " + TxtMemoType.Tag + ", Decision_ID = " + TxtAction.Tag + ", Loss_Amount = " + TxtActualLoss.Text + " , Debit_Mode = '" + TxtDebitAmount.Tag + "', Debit_Amount = " + TxtDebitAmount.Text + ", CAP = '" + TxtCAP.Text.ToString() + "' Where Rowid = " + Code);
                            MessageBox.Show("Approved", "Gainup");
                            MyBase.Clear(this);
                            TxtENo.Focus();
                            return;
                        }
                        else
                        {
                            return;
                        }                                               
                    }      
                    else
                    {
                        MessageBox.Show("Invalid Memo Details", "Gainup");
                        TxtENo.Focus();
                        return;
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      

        private void CmbMemoBase_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (CmbMemoBase.SelectedIndex == 0)
                {
                    TxtBuyer.Enabled = true;
                    TxtOrderNo.Enabled = true;
                }
                else
                {
                    TxtBuyer.Enabled = false;
                    TxtOrderNo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }       
                     
    }              
}