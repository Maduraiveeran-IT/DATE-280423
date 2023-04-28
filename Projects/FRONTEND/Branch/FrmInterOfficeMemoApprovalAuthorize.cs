using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounts;
using Accounts_ControlModules;
using System.IO;
using SelectionTool_NmSp;

namespace Accounts
{
    public partial class FrmInterOfficeMemoApprovalAuthorize : Form
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

        Font Tamil = new Font("Baamini", 9, FontStyle.Bold);
        Font English = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);

        public FrmInterOfficeMemoApprovalAuthorize()
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
                TxtDepartment.Tag = Dr["Catcode"].ToString();
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
                DtpDedDate.Value = Convert.ToDateTime(Dr["EDate"]);
                CmbMemoBase.Text = Dr["Type"].ToString();

                if (Dr["FontMode"].ToString() == "T")
                {
                    TxtSubject.Font = Tamil;
                    TxtDescription.Font = Tamil;
                    TxtCAP.Font = Tamil;
                }
                else
                {
                    TxtSubject.Font = English;
                    TxtDescription.Font = English;
                    TxtCAP.Font = English;
                }

                DataTable Dt1 = new DataTable();
                MyBase.Load_Data("Select Name, Tno From VAAHINI_ERP_GAINUP.DBO.Employeemas Where Emplno = " + TxtName.Tag + "", ref Dt1);
                if (Dt1.Rows.Count > 0)
                {
                    DataTable Tempdt = new DataTable();
                    MyBase.Load_Data("Select Photo From Vaahini_Gainup_Photo.Dbo.EmplPhoto Where Emplno = " + TxtName.Tag, ref Tempdt);
                    if (Tempdt.Rows.Count > 0)
                    {
                        Load_Picture(Tempdt.Rows[0], "Photo");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Employee ...!", "Gainup");
                    pictureBox2.Image = null;
                    return;
                }

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

        void Load_Picture(DataRow Dr, String PhotoCol)
        {
            try
            {
                Byte[] B;
                FileStream Fs = new FileStream(Application.StartupPath + "\\Im.JPG", FileMode.Create, FileAccess.Write);
                B = (byte[])Dr[PhotoCol];
                Fs.Write(B, 0, B.Length);
                Fs.Close();
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox2.ImageLocation = Application.StartupPath + "\\Im.JPG";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmInterOfficeMemoApprovalAuthorize_Load(object sender, EventArgs e)
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

        private void FrmInterOfficeMemoApprovalAuthorize_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtDebitAmount")
                    {
                        if (TxtAction.Text.ToString() != String.Empty)
                        {
                            if (TxtAction.Tag.ToString() == "1" || TxtAction.Tag.ToString() == "3" || TxtAction.Tag.ToString() == "6" || TxtAction.Tag.ToString() == "9") 
                            {
                                if (TxtDebitAmount.Text.ToString().Length <= 1)
                                {
                                    MyBase.Valid_Decimal((TextBox)this.ActiveControl, e);
                                }
                                else
                                {
                                    e.Handled = true;
                                    MessageBox.Show("Invalid Percentage", "Gainup");
                                    TxtDebitAmount.Text = "0";
                                    TxtDebitAmount.Focus();
                                }
                            }
                            else if (TxtAction.Tag.ToString() == "2" || TxtAction.Tag.ToString() == "4" || TxtAction.Tag.ToString() == "7" || TxtAction.Tag.ToString() == "8" || TxtAction.Tag.ToString() == "10" || TxtAction.Tag.ToString() == "11")
                            {
                                MyBase.Valid_Decimal((TextBox)this.ActiveControl, e);
                            }
                            else if (TxtAction.Tag.ToString() == "15" || TxtAction.Tag.ToString() == "16")
                            {
                                MyBase.Valid_Number((TextBox)this.ActiveControl, e);
                            }
                            else
                            {
                                e.Handled = true;
                                TxtDebitAmount.Text = "0";
                            }
                            //2,4,7,8,10,11

                        }
                        else
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Action Name", "Gainup");
                            TxtAction.Focus();
                            TxtDebitAmount.Text = "0";
                            
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtActualLoss")
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

        private void FrmInterOfficeMemoApprovalAuthorize_KeyDown(object sender, KeyEventArgs e)
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
                    else if (this.ActiveControl.Name == "TxtENo")
                    {
                        TxtMemoType.Focus();
                    }
                    else
                    {
                        if (this.ActiveControl.Name != "TxtCAP")
                        {
                            SendKeys.Send("{Tab}");
                        }
                    }
                }               
                else if  (e.KeyCode == Keys.Down)
                {                    
                        if (this.ActiveControl.Name == "TxtENo")
                        {
                            Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Memo", " Select A.Entry_No , A.EDate, B.Name, B.TNo, E.Order_No, E.Buyer, A.Subject, A.Description, A.CAP, A.Loss_Amount, A.Debit_Mode, A.Debit_Amount, C.DeptName Department, D.DesignationName Designation, IsNull(F.Name, '') Memo_Type, IsNull(G.Name, '') Decision, A.EmplNo, A.RowID, A.ACtion_ID , A.Decision_ID, A.FontMode, A.Memo_Type Type, B.Catcode  From Vaahini_Erp_Gainup.Dbo.Inter_Office_Memo_Details A Inner Join Vaahini_Erp_Gainup.Dbo.EmployeeMas B On A.EmplNo = B.EmplNo LEft Join Vaahini_Erp_Gainup.Dbo.DeptType C On B.Deptcode = C.DeptCode and C.CompCode = B.CompCode Left Join Vaahini_Erp_Gainup.Dbo.DesignationType D On B.DesignationCode = D.DesignationCode and B.Compcode = D.CompCode Left Join Vaahini_Erp_Gainup.Dbo.Fit_OrderNos() E On A.Order_No = E.Order_NO  Left Join Vaahini_Erp_Gainup.Dbo.Memo_Action_Name F On A.Action_ID = F.RowID LEft Join Vaahini_Erp_Gainup.Dbo.Memo_Action_Decision_Name G On A.Decision_ID = G.RowID Where A.Approval_Flag = 'N' and A.Approval_Flag_Authorized = 'N' and A.Memo_Type='GENERAL WORKER' and A.Division_ID=2 ", String.Empty, 80, 100, 130, 100, 120, 150);
                            if (Dr != null)
                            {
                                Fill_Datas(Dr);
                                TxtENo.Focus();
                            }
                        }
                        else if (this.ActiveControl.Name == "TxtMemoType")
                        {
                            Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select MemoType", " Select Name, RowID  from Vaahini_Erp_Gainup.Dbo.Memo_Action_Name  Order by RowID ", String.Empty, 210);
                            if (Dr != null)
                            {
                                TxtMemoType.Text = Dr["Name"].ToString();
                                TxtMemoType.Tag = Dr["RowID"].ToString();
                                TxtAction.Text = "";
                                TxtDebitAmount.Text = "0";
                            }
                        }
                        else if (this.ActiveControl.Name == "TxtAction")
                        {
                            if (TxtMemoType.Text.ToString() != String.Empty)
                            {
                                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select MemoType", " Select Name, RowID  from Vaahini_Erp_Gainup.Dbo.Memo_Action_Decision_Name Where RowID not in(2,4,3,1,10,7,9,6) and Action_Id = " + TxtMemoType.Tag + "  Order by RowID ", String.Empty, 400);
                                if (Dr != null)
                                {
                                    TxtAction.Text = Dr["Name"].ToString();
                                    TxtAction.Tag = Dr["RowID"].ToString();
                                    TxtDebitAmount.Text = "0";
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
                CmbMemoBase.Text = "";
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
                        MessageBox.Show("Invalid Action Name", "Gainup");
                        MyParent.Save_Error = true;
                        TxtAction.Focus();
                        return;
                    }
                    if (TxtMemoType.Text.ToString() == String.Empty)
                    {
                        MessageBox.Show("Invalid Memo Type", "Gainup");
                        MyParent.Save_Error = true;
                        TxtMemoType.Focus();
                        return;
                    }
                    if ((TxtDebitAmount.Text.ToString() == String.Empty || Convert.ToDouble(TxtDebitAmount.Text.ToString()) == 0))
                    {
                        if (TxtAction.Tag.ToString() != "12" && TxtAction.Tag.ToString() != "14" && TxtAction.Tag.ToString() != "17")
                        {
                            MessageBox.Show("Invalid Value", "Gainup");
                            TxtDebitAmount.Focus();
                            return;
                        }
                        else
                        {
                            TxtDebitAmount.Text = "0";
                            TxtDebitAmount.Tag = "N";
                        }
                    }

                    if (Convert.ToInt16(TxtAction.Tag.ToString()) == 11)
                    {
                        if (Convert.ToInt16(TxtDepartment.Tag.ToString()) == 5 || Convert.ToInt16(TxtDepartment.Tag.ToString()) == 6)
                        {
                            if (Convert.ToDouble(TxtDebitAmount.Text.ToString()) > 1000)
                            {
                                MessageBox.Show("Maximum 1000 Rs Cash Deduction only Allowed..!", "Gainup");
                                TxtDebitAmount.Focus();
                                MyParent.Save_Error = true;
                                return;
                            }
                        }
                        else
                        {
                            if (Convert.ToDouble(TxtDebitAmount.Text.ToString()) > 500)
                            {
                                MessageBox.Show("Maximum 500 Rs Cash Deduction only Allowed..!", "Gainup");
                                TxtDebitAmount.Focus();
                                MyParent.Save_Error = true;
                                return;
                            }
                        }
                    }

                    if (Convert.ToInt16(TxtAction.Tag.ToString()) == 16)
                    {
                        if (Convert.ToDouble(TxtDebitAmount.Text.ToString()) > 3)
                        {
                            MessageBox.Show("Maximum 3 Days Salary Deduction only Allowed..!", "Gainup");
                            TxtDebitAmount.Focus();
                            MyParent.Save_Error = true;
                            return;
                        }
                    }

                    if ((TxtDebitAmount.Text.ToString() != String.Empty || Convert.ToDouble(TxtDebitAmount.Text.ToString()) > 0))
                    {
                        //12,14,17
                        if (TxtAction.Tag.ToString() != "12" && TxtAction.Tag.ToString() != "14" && TxtAction.Tag.ToString() != "17")
                        {
                            DataTable DtD = new DataTable();
                            MyBase.Load_Data(" Select DATENAME(MM, FromDate) From  Vaahini_Erp_Gainup.Dbo.Paycalculation Where Month(Fromdate) = " + DtpDedDate.Value.Month + " and Year(FromDate) = " + DtpDedDate.Value.Year + " and CompCode In (2,5) ", ref DtD);
                            if (DtD.Rows.Count > 0)
                            {
                                MessageBox.Show("Salary Calculations Already Generated On This Month ('" + DtD.Rows[0][0].ToString() + "') ", "Gainup");
                                DtpDedDate.Focus();
                                return;
                            }
                        }

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
                        MyBase.Run("Update Vaahini_Erp_Gainup.Dbo.Inter_Office_Memo_Details Set CAP = '" + TxtCAP.Text.ToString() + "', Approval_Flag = 'Y', Approval_Time = Getdate(), Approval_Flag_Authorized = 'Y', Approval_Time_Authorize = Getdate(), Action_ID = " + TxtMemoType.Tag + ", Decision_ID = " + TxtAction.Tag + ", Loss_Amount = " + TxtActualLoss.Text + " , Debit_Mode = '" + TxtDebitAmount.Tag + "', Debit_Amount = " + TxtDebitAmount.Text + " , Ded_Effect_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpDedDate.Value) + "' Where Rowid = " + Code);
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