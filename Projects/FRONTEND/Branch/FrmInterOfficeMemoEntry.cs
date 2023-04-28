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
    public partial class FrmInterOfficeMemoEntry : Form,Entry 
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
        public FrmInterOfficeMemoEntry()
        {
            InitializeComponent();
        }
       
        public void Entry_Save()
        {
            try
            {
                Int32 Array_Index = 0;
                if (TxtName.Text.Trim() == String.Empty || TxtTNo.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Employee / No..!", "Gainup");
                    TxtName.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if ((TxtBuyer.Text.Trim() == String.Empty || TxtOrderNo.Text.Trim() == String.Empty) && CmbMemoBase.SelectedIndex == 0)
                {
                    MessageBox.Show("Invalid Party / Order No..!", "Gainup");
                    TxtBuyer.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (CmbMemoBase.SelectedIndex == 1)
                {
                    TxtOrderNo.Text = "";
                    TxtBuyer.Text = "";
                }

                if (TxtSubject.Text.Trim() == String.Empty || TxtDescription.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Subject / Description ..!", "Gainup");
                    TxtSubject.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtCAP.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Corrective Action Plan ..!", "Gainup");
                    TxtCAP.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtActualLoss.Text.Trim() == String.Empty)
                {
                    TxtActualLoss.Text = "0.00";
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
                //if (ChkDebit.Checked == false)
                //{
                //    TxtDebitAmount.Text = "0.00";
                //    TxtDebitAmount.Tag = "N";
                //}                
                //else
                //{
                //    if (TxtDebitAmount.Text.ToString() == String.Empty)
                //    {
                //        MessageBox.Show("Check Debit Amount / OtherWise Remove Tick In CheckBox ..!", "Gainup");
                //        TxtDebitAmount.Focus();
                //        MyParent.Save_Error = true;
                //        return;
                //    }
                //    else
                //    {
                //        TxtDebitAmount.Tag = "Y";
                //    }
                //}

                if (TxtAction.Text.ToString() == String.Empty)
                {
                    TxtAction.Tag = 0;
                }
                if (TxtMemoType.Text.ToString() == String.Empty)
                {
                    TxtMemoType.Tag = 0;
                }


                Queries = new String[5];
                if (MyParent._New)
                {
                    DataTable TDt1 = new DataTable();
                    MyBase.Load_Data("Select IsNull(Max(Entry_No),0) + 1 EntryNo From Vaahini_Erp_Gainup.Dbo.Inter_Office_Memo_Details ", ref TDt1);
                    if (TDt1.Rows[0][0].ToString() != String.Empty)
                    {
                        TxtENo.Text = TDt1.Rows[0][0].ToString();
                    }
                    else
                    {
                        TxtENo.Text = "1";
                    }
                    Queries[Array_Index++] = "Insert Into Vaahini_Erp_Gainup.Dbo.Inter_Office_Memo_Details(Entry_No, EmplNo, Order_No, Subject, Description, CAP, Debit_Mode, Debit_Amount, Division_ID, Approval_Flag, EDate, Loss_Amount, Action_ID, Decision_ID, Approval_Flag_Authorized, Memo_Type) Values ('" + TxtENo.Text.ToString() + "', " + TxtName.Tag.ToString() + ", '" + TxtOrderNo.Text.ToString() + "', '" + TxtSubject.Text.ToString() + "', '" + TxtDescription.Text.ToString() + "', '" + TxtCAP.Text.ToString() + "', '" + TxtDebitAmount.Tag.ToString() + "', '" + TxtDebitAmount.Text.ToString() + "', 2, 'N', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtActualLoss.Text.ToString() + ", " + TxtMemoType.Tag + ", " + TxtAction.Tag + ", 'N', 'SOCKS')";
                    
                }
                else
                {
                    Queries[Array_Index++] = "Update  Vaahini_Erp_Gainup.Dbo.Inter_Office_Memo_Details Set  EmplNo = " + TxtName.Tag.ToString() + " , Order_No = '" + TxtOrderNo.Text.ToString() + "' , Subject = '" + TxtSubject.Text.ToString() + "' , Description = '" + TxtDescription.Text.ToString() + "' , CAP = '" + TxtCAP.Text.ToString() + "' , Debit_Mode = '" + TxtDebitAmount.Tag.ToString() + "' , Debit_Amount = '" + TxtDebitAmount.Text.ToString() + "' , Division_ID = 2 ,  EDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "' , Loss_Amount = " + TxtActualLoss.Text.ToString() + ", Action_ID = " + TxtMemoType.Tag + ", Decision_ID = " + TxtAction.Tag + ", Approval_Flag = 'N', Approval_Flag_Authorized = 'N' Where RowID = " + Code + " ";
                }

                if (MyParent._New)
                {
                    Queries[Array_Index++] = MyParent.EntryLog("INTER MEMO", "ADD", TxtENo.Text.ToString());
                }
                else
                {
                    Queries[Array_Index++] = MyParent.EntryLog("INTER MEMO", "EDIT", TxtENo.Text.ToString());                    
                }
                MyBase.Run(Queries);
                MyParent.Save_Error = false;
                MessageBox.Show("Saved ..!", "Gainup");
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
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
        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Inter Office - Edit", "Select A.Entry_No , A.EDate, B.Name, B.TNo, E.Order_No, E.Buyer, A.Subject, A.Description, A.CAP, A.Loss_Amount, A.Debit_Mode, A.Debit_Amount, C.DeptName Department, D.DesignationName Designation, IsNull(F.Name, '') Memo_Type, IsNull(G.Name, '') Decision,  A.EmplNo, A.RowID, A.ACtion_ID , A.Decision_ID  From Vaahini_Erp_Gainup.Dbo.Inter_Office_Memo_Details A LEft Join Vaahini_Erp_Gainup.Dbo.EmployeeMas B On A.EmplNo = B.EmplNo LEft Join Vaahini_Erp_Gainup.Dbo.DeptType C On B.Deptcode = C.DeptCode and C.CompCode = B.CompCode Left Join Vaahini_Erp_Gainup.Dbo.DesignationType D On B.DesignationCode = D.DesignationCode and B.Compcode = D.CompCode Left Join Vaahini_Erp_Gainup.Dbo.Fit_OrderNos() E On A.Order_No = E.Order_NO Left Join Vaahini_Erp_Gainup.Dbo.Memo_Action_Name F On A.Action_ID = F.RowID LEft Join Vaahini_Erp_Gainup.Dbo.Memo_Action_Decision_Name G On A.Decision_ID = G.RowID Where Approval_Flag = 'N' and Division_Id = 2 and Memo_Type = 'SOCKS'", String.Empty, 80, 80, 120, 100, 100, 140, 160); 
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    TxtName.Focus();
                }
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
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Inter Office - View", "Select A.Entry_No , A.EDate, B.Name, B.TNo, E.Order_No, E.Buyer, A.Subject, A.Description, A.CAP, A.Loss_Amount, A.Debit_Mode, A.Debit_Amount, C.DeptName Department, D.DesignationName Designation, IsNull(F.Name, '') Memo_Type, IsNull(G.Name, '') Decision,  A.EmplNo, A.RowID, A.ACtion_ID , A.Decision_ID  From Vaahini_Erp_Gainup.Dbo.Inter_Office_Memo_Details A LEft Join Vaahini_Erp_Gainup.Dbo.EmployeeMas B On A.EmplNo = B.EmplNo LEft Join Vaahini_Erp_Gainup.Dbo.DeptType C On B.Deptcode = C.DeptCode and C.CompCode = B.CompCode Left Join Vaahini_Erp_Gainup.Dbo.DesignationType D On B.DesignationCode = D.DesignationCode and B.Compcode = D.CompCode Left Join Vaahini_Erp_Gainup.Dbo.Fit_OrderNos() E On A.Order_No = E.Order_NO Left Join Vaahini_Erp_Gainup.Dbo.Memo_Action_Name F On A.Action_ID = F.RowID LEft Join Vaahini_Erp_Gainup.Dbo.Memo_Action_Decision_Name G On A.Decision_ID = G.RowID Where Division_Id = 2 and Memo_Type = 'SOCKS'", String.Empty, 80, 80, 120, 100, 100, 140, 160); 
                if (Dr != null)
                {
                    Fill_Datas(Dr);                   
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
                String Str = "";
                CrystalDecisions.CrystalReports.Engine.ReportDocument ORpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                Str = "Select A.Entry_No , A.EDate, B.Name, B.TNo, E.Order_No, E.Buyer, A.Subject, A.Description, A.CAP, A.Loss_Amount, A.Debit_Mode, A.Debit_Amount, C.DeptName Department, D.DesignationName Designation,  A.EmplNo, A.RowID  From Vaahini_Erp_Gainup.Dbo.Inter_Office_Memo_Details A LEft Join Vaahini_Erp_Gainup.Dbo.EmployeeMas B On A.EmplNo = B.EmplNo LEft Join Vaahini_Erp_Gainup.Dbo.DeptType C On B.Deptcode = C.DeptCode and C.CompCode = B.CompCode Left Join Vaahini_Erp_Gainup.Dbo.DesignationType D On B.DesignationCode = D.DesignationCode and B.Compcode = D.CompCode Left Join Vaahini_Erp_Gainup.Dbo.Fit_OrderNos() E On A.Order_No = E.Order_NO Where A.RowID = " + Code + " and A.Approval_Flag  = 'Y' ";
                MyBase.Execute_Qry(Str, "Rpt_Inter_Office_Memo_Details");
                DataTable TmpDt1 = new DataTable();
                MyBase.Load_Data("Select * From Rpt_Inter_Office_Memo_Details ", ref TmpDt1);
                if (TmpDt1.Rows.Count > 0)
                {
                    ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Inter_Office_Memo_Details.rpt");
                    MyParent.FormulaFill(ref ORpt, "PDate", MyBase.GetServerDateTime().ToString());
                    MyParent.CReport(ref ORpt, "MEMO DETAILS..!");
                }
                else
                {
                    MessageBox.Show("NOT YET APPROVED", "GAINUP");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Delete()
        {
            try
            {                
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Inter Office - Delete", "Select A.Entry_No , A.EDate, B.Name, B.TNo, E.Order_No, E.Buyer, A.Subject, A.Description, A.CAP, A.Loss_Amount, A.Debit_Mode, A.Debit_Amount, C.DeptName Department, D.DesignationName Designation, IsNull(F.Name, '') Memo_Type, IsNull(G.Name, '') Decision,  A.EmplNo, A.RowID, A.ACtion_ID , A.Decision_ID  From Vaahini_Erp_Gainup.Dbo.Inter_Office_Memo_Details A LEft Join Vaahini_Erp_Gainup.Dbo.EmployeeMas B On A.EmplNo = B.EmplNo LEft Join Vaahini_Erp_Gainup.Dbo.DeptType C On B.Deptcode = C.DeptCode and C.CompCode = B.CompCode Left Join Vaahini_Erp_Gainup.Dbo.DesignationType D On B.DesignationCode = D.DesignationCode and B.Compcode = D.CompCode Left Join Vaahini_Erp_Gainup.Dbo.Fit_OrderNos() E On A.Order_No = E.Order_NO Left Join Vaahini_Erp_Gainup.Dbo.Memo_Action_Name F On A.Action_ID = F.RowID LEft Join Vaahini_Erp_Gainup.Dbo.Memo_Action_Decision_Name G On A.Decision_ID = G.RowID Where Approval_Flag = 'N'  and Division_Id = 2 and Memo_Type = 'SOCKS'", String.Empty, 80, 80, 120, 100, 100, 140, 160); 
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    MyParent.Load_DeleteConfirmEntry();
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
                    MyBase.Run("Delete From Vaahini_Erp_Gainup.Dbo.Inter_Office_Memo_Details  Where RowID = " + Code, MyParent.EntryLog("INTER MEMO", "DELETE", Code.ToString()));                    
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

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                CmbMemoBase.SelectedIndex = 0;
                TxtName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmInterOfficeMemoEntry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                MyBase.Clear(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmInterOfficeMemoEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {                  
                    if (this.ActiveControl.Name == "TxtActualLoss" || this.ActiveControl.Name == "TxtDebitAmount")
                    {
                        MyBase.Valid_Decimal((TextBox)this.ActiveControl, e);
                    }
                    else if (this.ActiveControl.Name != "TxtSubject" && this.ActiveControl.Name != "TxtDescription" && this.ActiveControl.Name != "TxtCAP" && this.ActiveControl.Name != "TxtActualloss" && this.ActiveControl.Name != "TxtDebitAmount")
                    {
                        MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                    }
                    else
                    {
                        e.Handled = false ;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmInterOfficeMemoEntry_KeyDown(object sender, KeyEventArgs e)
        {          
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;                    
                    if (this.ActiveControl.Name == "TxtDebitAmount")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name != "TxtDescription" && this.ActiveControl.Name != "TxtCAP")
                    {
                        SendKeys.Send("{Tab}");
                    }                   
                }                 
                else if  (e.KeyCode == Keys.Down)
                {
                    //if (MyParent._New == true)
                    //{
                        if (this.ActiveControl.Name == "TxtName")
                        {
                            Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Name", " Select A.Name, A.TNo, B.DeptName Department, C.DesignationName Designation, A.EmplNo From Vaahini_Erp_Gainup.Dbo.EmployeeMas A LEft Join Vaahini_Erp_Gainup.Dbo.DeptType B On A.Deptcode = B.DeptCode and A.CompCode = B.CompCode Left Join Vaahini_Erp_Gainup.Dbo.DesignationType C On A.DesignationCode = C.DesignationCode and A.CompCode = C.CompCode Where A.CompCode in (2,3,8) and A.CatCode in (5,6) and A.EmplNo Not In (9460,7802) and A.TNo Not Like '%Z' Order By A.TNo ", String.Empty, 160, 100, 160, 180);
                            if (Dr != null)
                            {                                
                                TxtName.Text  = Dr["Name"].ToString();
                                TxtName.Tag = Dr["EmplNo"].ToString();
                                TxtTNo.Text = Dr["TNo"].ToString();
                                TxtDepartment.Text = Dr["Department"].ToString();
                                TxtDesignation.Text = Dr["Designation"].ToString();                                
                            }
                        }
                        else if (this.ActiveControl.Name == "TxtBuyer")
                        {
                            Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Buyer", " Select Order_No, Buyer From Vaahini_Erp_Gainup.Dbo.Fit_OrderNos() Where Module= 'SOCKS' Order by Order_No ", String.Empty, 120, 180);
                            if (Dr != null)
                            {
                                TxtOrderNo.Text = Dr["Order_No"].ToString();                                
                                TxtBuyer.Text = Dr["Buyer"].ToString();                                
                            }
                        }
                        else if (this.ActiveControl.Name == "TxtMemoType")
                        {
                            Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Type", " Select Name, RowID From Vaahini_Erp_Gainup.Dbo.Memo_Action_Name Order by RowID ", String.Empty, 180);
                            if (Dr != null)
                            {
                                TxtMemoType.Text = Dr["Name"].ToString();
                                TxtMemoType.Tag  = Dr["RowID"].ToString();
                            }
                        }
                        else if (this.ActiveControl.Name == "TxtAction")
                        {
                            if (TxtMemoType.Text.ToString() != String.Empty)
                            {
                                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Action", " select Name , RowID  FRom Vaahini_Erp_Gainup.Dbo.Memo_Action_Decision_Name  Where Action_ID = " + TxtMemoType.Tag + " Order by RowID ", String.Empty, 250);
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
                       
                    //}                    

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

        private void ChkDebit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChkDebit.Checked == true)
                {
                    TxtDebitAmount.Enabled = true;
                    TxtDebitAmount.Focus();
                }
                else
                {
                    TxtDebitAmount.Text = "";
                    TxtDebitAmount.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbMemoBase_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void CmbMemoBase_SelectedValueChanged(object sender, EventArgs e)
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