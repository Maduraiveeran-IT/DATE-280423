using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmOrderEnquiry : Form, Entry
    {
        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        Int64 Code = 0;

        public FrmOrderEnquiry()
        {
            InitializeComponent();
        }

        void TableCreation()
        {
            try
            {
                //MyBase.Execute("Create Table Order_Enquiry (RowID Bigint Identity, Enquiry_No Bigint Not Null, Enquiry_Date datetime Not Null, Merchandiser_Code Varchar (10) Not Null, Buyer_Code Varchar (10) Not Null, Order_No Varchar (100) Not Null, Order_Date datetime Not null, Qty Numeric (20) Not Null, EntryAt Datetime Not Null, EntrySystem Varchar (100) Not Null,Constraint Uk_Unique_OrdEnq Unique (Buyer_Code, Order_No, Order_Date))");
                //MyBase.Execute("CREATE VIEW ST_LEDGER_MASTER AS SELECT ADD_CODE, ADD_NAME, ADD_TYPE FROM ADDRESS");
                //MyBase.Execute("Create VIEW ST_USER_LIST AS Select EMP_CODE, EMP_NAME NAME, P1.DESIGNATION_NAME DESIGNATION, d1.dept_name Department From eMPL E1 LEFT JOIN PR_DESIGNATION P1 ON E1.emp_desgn_code = P1.DESIGNATION_CODE left join dept d1 on e1.dept_code = d1.dept_code WHERE E1.ADD_CODE = 'HO/U/2'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmOrderEnquiry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                //TableCreation();
                MyBase.Clear(this);
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
                Code = 0;
                //ChkStatus.Enabled = false;
                DtpRecStatus.Enabled = false;
                TxtBuyer.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Save()
        {
            try
            {

                if (TxtBuyer.Text.Trim() == String.Empty || TxtMerch.Text.Trim() == String.Empty || TxtOrderNo.Text.Trim() == String.Empty || TxtQty.Text.Trim() == String.Empty || TxtIOSDays.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Details to Save ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtBuyer.Focus();
                    return;
                }

                if (TxtIOSDays.Text.ToString() != String.Empty)
                {
                    DtpRecStatus.Value = DtpOrderDate.Value.AddDays(Convert.ToInt32(TxtIOSDays.Text));
                }

                if (TxtQty.Text.Trim() == String.Empty)
                {
                    TxtQty.Text = "0";
                }

                if (TxtNeedle.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Needle ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtNeedle.Focus();
                    return;
                }

                if (DtpOrderDate.Value > DtpDelDate.Value)
                {
                    MessageBox.Show("Invalid Delivery Date ...!", "Gainup");
                    MyParent.Save_Error = true;
                    DtpDelDate.Focus();
                    return;
                }

                if (Convert.ToDouble(TxtQty.Text) == 0)
                {
                    MessageBox.Show("Invalid Qty ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtQty.Focus();
                    return;
                }

                String Status = "T";

                //if (ChkStatus.Checked == false)
                //{
                //    Status = "F";
                //    DtpRecStatus.Value = Convert.ToDateTime("01-Jan-1999");
                //}
                //else
                //{
                //    Status = "T";
                //}

                if (MyParent._New)
                {
                    TxtEntryNo.Text = MyBase.MaxWOCC("Order_Enquiry", "Enquiry_No", String.Empty).ToString();
                    MyBase.Run("Insert into Order_Enquiry (Enquiry_No, Enquiry_Date, Merchandiser_Code, Buyer_Code, Order_No, Order_Date, Qty, EntryAt, EntrySystem, Del_Date, Status, Rec_Date, IOSDays, OCNNo, Needle ) values (" + TxtEntryNo.Text.Trim() + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + TxtMerch.Tag.ToString() + "', '" + TxtBuyer.Tag.ToString() + "', '" + TxtOrderNo.Text.Trim() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpOrderDate.Value) + "', " + Convert.ToDouble(TxtQty.Text.Trim()) + ", '" + String.Format("{0:dd-MMM-yyyy}", MyBase.GetServerDate()) + "', '" + Environment.MachineName.ToUpper() + "', '" + String.Format ("{0:dd-MMM-yyyy}", DtpDelDate.Value) + "', '" + Status + "', '" + String.Format ("{0:dd-MMM-yyyy}", DtpRecStatus.Value) + "', " + TxtIOSDays.Text + ", '" + TxtOCNNo.Text + "', '" + TxtNeedle.Text + "')");
                }
                else
                {
                    MyBase.Run("Update Order_Enquiry Set Del_Date = '" + String.Format ("{0:dd-MMM-yyyy}", DtpDelDate.Value) + "',  Status = '" + Status + "', Rec_Date = '" + String.Format ("{0:dd-MMM-yyyy}", DtpRecStatus.Value) + "', Enquiry_Date = '" + String.Format ("{0:dd-MMM-yyyy}", DtpDate.Value) + "', Merchandiser_Code = '" + TxtMerch.Tag.ToString() + "', Buyer_Code = '" + TxtBuyer.Tag.ToString() + "', Order_No = '" + TxtOrderNo.Text.Trim() + "', Order_Date = '" + String.Format ("{0:dd-MMM-yyyy}", DtpOrderDate.Value) + "', Qty = " + Convert.ToDouble(TxtQty.Text) + ", OCNNo = '" + TxtOCNNo.Text + "', IOSDays = " + TxtIOSDays.Text + ", Needle = '" + TxtNeedle.Text + "' Where RowID = " + Code);
                }
                MessageBox.Show ("Saved ...!", "Gainup");
                MyBase.Clear (this);
                MyParent.Save_Error = false;
                MyBase.Clear (this);
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                if (ex.Message.ToUpper().Contains("UK_UNIQUE"))
                {
                    MessageBox.Show("Order No Already Alloted ...!", "Gainup");
                    TxtOrderNo.Focus();
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
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

        public void Entry_View()
        {
            try
            {
                Code = 0;
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Order - View", "Select Distinct Enquiry_No, O1.Order_no,  O1.Enquiry_Date, S2.Buyer, S1.Name Merchandiser, O1.Order_Date, O1.IOSDays, O1.OCNNo, O1.Qty, O1.Needle, O1.Merchandiser_Code, O1.buyer_Code, O1.RowID, O1.Del_Date, O1.Status, O1.Rec_Date  From Order_Enquiry O1 Left join Vaahini_Erp_Gainup.Dbo.EmployeeMas S1 On O1.Merchandiser_Code = S1.EmplNo  Left join Buyer S2 On O1.Buyer_Code = S2.BuyerID Left Join  Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master B1 On O1.OcnNo = B1.Order_No and B1.Division_ID = 3   Order by Enquiry_No Desc", String.Empty, 90, 150, 90, 250, 200, 90, 100, 100, 100, 100);
                if (Dr != null)
                {                    
                    DtpRecStatus.Enabled = false;
                    Fill_Datas(Dr);
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
                    MyBase.Run("Delete From Order_Enquiry Where RowID = " + Code);
                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyBase.Clear(this);
                    MyParent.Load_DeleteEntry();
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
                Code = 0;
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Order - Delete", "Select Distinct Enquiry_No, O1.Order_no,  O1.Enquiry_Date, S2.Buyer, S1.Name Merchandiser, O1.Order_Date, O1.IOSDays, O1.OCNNo, O1.Qty, O1.Needle, O1.Merchandiser_Code, O1.buyer_Code, O1.RowID, O1.Del_Date, O1.Status, O1.Rec_Date  From Order_Enquiry O1 Left join Vaahini_Erp_Gainup.Dbo.EmployeeMas S1 On O1.Merchandiser_Code = S1.EmplNo  Left join Buyer S2 On O1.Buyer_Code = S2.BuyerID Left Join  Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master B1 On O1.OcnNo = B1.Order_No and B1.Division_ID = 3 Where B1.Order_No Is Null  Order by Enquiry_No Desc", String.Empty, 90, 150, 90, 250, 200, 90, 100, 100, 100, 100);
                if (Dr != null)
                {                  
                    DtpRecStatus.Enabled = false;
                    Fill_Datas(Dr);
                    MyParent.Load_DeleteConfirmEntry();
                }
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
                Code = Convert.ToInt64 (Dr["RowID"]);
                TxtEntryNo.Text = Dr["Enquiry_No"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["Enquiry_Date"]);
                DtpDelDate.Value = Convert.ToDateTime(Dr["Del_Date"]);
                TxtBuyer.Text = Dr["Buyer"].ToString();
                TxtBuyer.Tag = Dr["Buyer_Code"].ToString();
                TxtMerch.Text = Dr["Merchandiser"].ToString();
                TxtMerch.Tag = Dr["Merchandiser_Code"].ToString();
                TxtOrderNo.Text = Dr["Order_No"].ToString();
                DtpOrderDate.Value = Convert.ToDateTime(Dr["Order_Date"]);
                TxtQty.Text = String.Format ("{0:0}", Convert.ToDouble(Dr["Qty"]));
                TxtIOSDays.Text = Dr["IOSDays"].ToString();
                TxtOCNNo.Text = Dr["OCNNo"].ToString();
                DtpRecStatus.Value = Convert.ToDateTime(Dr["Rec_Date"]);
                TxtNeedle.Text = Dr["Needle"].ToString();
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
                Code = 0;
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Order - Edit", "Select Distinct Enquiry_No, O1.Order_no,  O1.Enquiry_Date, S2.Buyer, S1.Name Merchandiser, O1.Order_Date, O1.IOSDays, O1.OCNNo, O1.Qty, O1.Needle, O1.Merchandiser_Code, O1.buyer_Code, O1.RowID, O1.Del_Date, O1.Status, O1.Rec_Date  From Order_Enquiry O1 Left join Vaahini_Erp_Gainup.Dbo.EmployeeMas S1 On O1.Merchandiser_Code = S1.EmplNo  Left join Buyer S2 On O1.Buyer_Code = S2.BuyerID Left Join  Vaahini_Erp_Gainup.Dbo.Time_Action_Plan_Master B1 On O1.OcnNo = B1.Order_No and B1.Division_ID = 3 Where B1.Order_No Is Null  Order by Enquiry_No Desc", String.Empty, 90, 150, 90, 250, 200, 90, 100, 100, 100, 100);
                if (Dr != null)
                {
                    DtpRecStatus.Enabled = false;
                    Fill_Datas(Dr);
                    TxtBuyer.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmOrderEnquiry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtIOSDays")
                    {
                        if (MyParent._New || MyParent.Edit)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    //else if (this.ActiveControl.Name == "ChkStatus")
                    //{
                    //    if (MyParent.Edit)
                    //    {
                    //        MyParent.Load_SaveEntry();
                    //        return;
                    //    }
                    //}
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
                {
                    if (this.ActiveControl.Name == "TxtEntryNo" || this.ActiveControl.Name == "TxtBuyer" || this.ActiveControl.Name == "TxtMerch")
                    {
                        e.Handled = true;
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtBuyer")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer", "Select Buyer, BuyerID From buyer ", String.Empty, 250);
                        if (Dr != null)
                        {
                            TxtBuyer.Tag = Dr["BuyerID"].ToString();
                            TxtBuyer.Text = Dr["Buyer"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtMerch")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Merchandiser", "Select Name,  Tno, EmplNo Code From Vaahini_Erp_Gainup.Dbo.Employeemas where Tno Not like '%Z' and DeptCode in (109,115) and Tno Like '%GKA%' and CompCode = 2", String.Empty, 250, 100);
                        if (Dr != null)
                        {
                            TxtMerch.Tag = Dr["Code"].ToString();
                            TxtMerch.Text = Dr["Name"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtOCNNo")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select OCNNo", "Select A.Order_No, A.Order_Date, A.Ref_No From buy_ord_mas A Left Join Order_Enquiry B On A.Order_No = B.OcnNo Where B.OcnNo Is Null and A.BuyerID = " + TxtBuyer.Tag + "  and A.Order_Date >= '01-Jul-2015'", String.Empty, 250, 100, 160);
                        if (Dr != null)
                        {
                            TxtOCNNo.Text = Dr["Order_No"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtNeedle")
                    {
                        if (TxtBuyer.Text != String.Empty)
                        {
                            if (TxtNeedle.Text.ToString() == String.Empty)
                            {
                                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Needle", "Select  Distinct Needle  from Knitting_Mc_NO() Order by Needle  ", String.Empty, 140);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Needle", "Select  Distinct Needle  from Knitting_Mc_NO() Where  Needle Not In(" + TxtNeedle.Tag.ToString() + ") Order By Needle ", String.Empty, 140);
                            }
                            if (Dr != null)
                            {
                                if (TxtNeedle.Text.Trim().ToString() == String.Empty)
                                {
                                    TxtNeedle.Text = Dr["Needle"].ToString();
                                    TxtNeedle.Tag  = "'" + Dr["Needle"].ToString() + "'";
                                }
                                else
                                {
                                    TxtNeedle.Text = TxtNeedle.Text.ToString() + " , " + Dr["Needle"].ToString();
                                    TxtNeedle.Tag = TxtNeedle.Tag.ToString() + " , '" + Dr["Needle"].ToString() + "'";
                                }                                
                                TxtNeedle.Focus();
                                TxtNeedle.DeselectAll();
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Division", "Gainup");
                            return;
                        }

                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    MyBase.ActiveForm_Close(this, MyParent);
                }
                else if (this.ActiveControl.Name == "TxtNeedle" && (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete))
                {
                    TxtNeedle.Text = "";
                    TxtNeedle.Tag  = "";
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmOrderEnquiry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtEntryNo" || this.ActiveControl.Name == "TxtNeedle" || this.ActiveControl.Name == "TxtBuyer" || this.ActiveControl.Name == "TxtMerch" || this.ActiveControl.Name == "TxtOCNNo")
                    {
                        e.Handled = true;
                    }
                    else if (this.ActiveControl.Name == "TxtQty" || this.ActiveControl.Name == "TxtIOSDays")
                    {
                        MyBase.Valid_Number((TextBox)this.ActiveControl, e);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChkStatus_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (ChkStatus.Checked)
                //{
                //    DtpRecStatus.Enabled = true;
                //    ChkStatus.Text = "YES";
                //    DtpRecStatus.Value = MyBase.GetServerDate();
                //    DtpRecStatus.Focus();
                //}
                //else
                //{
                //    DtpRecStatus.Enabled = false;
                //    ChkStatus.Text = "NO";
                //    DtpRecStatus.Value = Convert.ToDateTime("01-Jan-1999");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtIOSDays_Leave(object sender, EventArgs e)
        {
            try
            {
                if (TxtIOSDays.Text.ToString() != String.Empty)
                {
                    DtpRecStatus.Value = DtpOrderDate.Value.AddDays(Convert.ToInt32(TxtIOSDays.Text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtMerch_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void TxtIOSDays_TextChanged(object sender, EventArgs e)
        {
            if (TxtIOSDays.Text.ToString() != String.Empty)
            {
                DtpRecStatus.Value = DtpOrderDate.Value.AddDays(Convert.ToInt32(TxtIOSDays.Text));
            }
        }

        private void DtpRecStatus_ValueChanged(object sender, EventArgs e)
        {

        }

        private void DtpOrderDate_ValueChanged(object sender, EventArgs e)
        {

        }



    }
}