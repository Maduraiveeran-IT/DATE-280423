using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp; 
using Accounts;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmCompanyMaster : Form, Entry 
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        String Code;

        public FrmCompanyMaster()
        {
            InitializeComponent();
        }

        void TxtBankShortName_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                MyBase.Return_Ucase(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        void TxtBankName_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                MyBase.Return_Ucase(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        void TxtBankName_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Create_Table()
        {
            try
            {
                if (MyBase.Check_Table("Area_Master") == false)
                {
                    MyBase.Execute("Create table Area_Master (Area_Code Int, Area_Name varchar(100), Area_STD varchar(25))");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void NewField()
        {
            try
            {
                MyBase.Add_NewField("Socks_Companymas", "Millions", "Varchar(1)");
                MyBase.Add_NewField("Socks_Companymas", "CurName", "Varchar(50)");
                MyBase.Add_NewField("Socks_Companymas", "CurSymbol", "Varchar(10)");
                MyBase.Add_NewField("Socks_Companymas", "CurDecName", "Varchar(50)");
                MyBase.UpdateSpecialFields("Socks_Companymas");
                if (MyBase.Check_TableField ("Socks_Companymas", "CompTariffNo") == true)
                {
                    MyBase.Execute("sp_rename 'Socks_Companymas.comptariffno', 'compTANNo', 'COLUMN'");
                }
                MyBase.Add_NewField("Socks_Companymas", "SealName", "Varchar(50)");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmBankMaster_Load(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                NewField();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }


        void FrmBankMaster_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "TxtName")
                    {
                        if (TxtName.Text.Trim() == String.Empty)
                        {
                            MessageBox.Show("Invalid Company ...!");
                            TxtName.Focus();
                        }
                        else
                        {
                            TxtInPrinting.Text = TxtName.Text;
                            TxtSeal.Text = TxtName.Text;
                            //Load_Basic();
                            TxtInPrinting.Focus();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtMillions")
                    {
                        TxtMillions.Text = "N";
                        TxtCurDecimal.Focus();
                    }
                    else if (this.ActiveControl.Name == "TxtInPrinting")
                    {
                        TxtAddress.Focus();
                        if (TxtAddress.Text.Trim() != String.Empty)
                        {
                            SendKeys.Send("{End}");
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtAddress")
                    {
                        e.Handled = true;
                    }
                    else if (this.ActiveControl.Name == "DtpFrom")
                    {
                        if (DtpFrom.Value.Month == 4 && DtpFrom.Value.Day == 1)
                        {
                            DtpTo.Value = Convert.ToDateTime("31/03/" + (DtpFrom.Value.Year + 1));
                            TxtCurName.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Date ...!");
                            DtpFrom.Value = Convert.ToDateTime("01/04/" + DateTime.Now.Year);
                            DtpFrom.Focus();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtLicense")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            e.Handled = true;
                            MyParent.Load_SaveEntry();
                        }
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
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

        void Load_Basic()
        {
            try
            {
                if (TxtCurName.Text.Trim() == String.Empty)
                {
                    TxtCurName.Text = "Rupees";
                    TxtCurSym.Text = "Rs.";
                    TxtCurDecimal.Text = "Paise";
                    TxtMillions.Text = "N";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                Code = Convert.ToString(MyBase.MaxWOCC("Socks_Companymas", "CompCode", ""));
                Load_Basic();
                TxtName.Focus();  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Save()
        {
            String Sql, Sql1, GRPRes = string.Empty;
            try
            {
                if (TxtName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Company ...!");
                    TxtInPrinting.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtInPrinting.Text.Trim() == String.Empty)
                {
                    TxtInPrinting.Text = TxtName.Text;
                }
                if (DtpFrom.Value.Day == 1 && DtpFrom.Value.Month == 4 && Convert.ToInt32(DtpFrom.Value.Year + 1) == Convert.ToInt32(DtpTo.Value.Year) && Convert.ToInt32(DtpTo.Value.Day) == 31 && Convert.ToInt32(DtpTo.Value.Month) == 3)
                {
                }
                else
                {
                    MessageBox.Show("Invalid Acc Period ...!");
                    DtpFrom.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (Code.Trim() != String.Empty && TxtName.Text.Trim() != String.Empty)
                {
                    if (MyParent._New == true)
                    {
                        Code = Convert.ToString(MyBase.MaxWOCC("Socks_Companymas", "CompCode", ""));
                    }
                    else
                    {
                        MyParent.New_UserCode = MyBase.GetEntryDetails_User("Socks_Companymas", "CompCode = " + Code);
                        MyParent.New_Today = MyBase.GetEntryDetails_Date("Socks_Companymas", "CompCode = " + Code);
                        MyParent.New_SysCode = MyBase.GetEntryDetails_Sys("Socks_Companymas", "CompCode = " + Code);
                    }
                    if (MyBase.Get_RecordCount("Socks_Companymas", "CompName = '" + TxtName.Text + "' and Sdt = '" + String.Format("{0:dd-MMM-yyyy}", DtpFrom.Value) + "' and Edt = '" + String.Format("{0:dd-MMM-yyyy}", DtpTo.Value) + "' and CompCode <> " + Code) > 0)
                    {
                        MessageBox.Show("Company Already Exists ...!");
                        TxtName.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    Sql = "Delete From Socks_Companymas where CompCode = " + Code;
                    if (MyParent._New == true)
                    {
                        Sql1 = "Insert into Socks_Companymas values (" + Code + ", '" + TxtName.Text.Trim() + "', '" + TxtInPrinting.Text.ToString().Trim() + "', '" + TxtAddress.Text.Trim() + "', '" + TxtPhone.Text.Trim() + "', '" + TxtFax.Text.Trim() + "', '" + TxtEmail.Text.Trim() + "', '" + TxtTin.Text.Trim() + "', '" + TxtCST.Text.Trim() + "', '" + TxtEcc.Text.Trim() + "', '" + TxtTariff.Text.Trim() + "', '" + TxtBED.Text.Trim() + "', '" + TxtAED.Text.Trim() + "', '" + TxtTax.Text.Trim() + "', '" + TxtPan.Text.Trim() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpPanDate.Value) + "', ' ', '" + String.Format("{0:dd-MMM-yyyy}", DtpFrom.Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpTo.Value) + "', '" + TxtPF.Text.Trim() + "', '" + TxtESI.Text.Trim() + "', '" + TxtLicense.Text.Trim() + "', ' ', ' ', '" + TxtMillions.Text.Trim() + "', '" + TxtCurName.Text.Trim() + "', '" + TxtCurSym.Text.Trim() + "', '" + TxtCurDecimal.Text.Trim() + "', " + MyParent.UserCode + ", " + MyParent.SysCode + ", " + MyParent.Today + ", " + MyParent.UserCode + ", " + MyParent.SysCode + ", " + MyParent.Today + ", " + MyParent.CompCode + ", '" + MyParent.YearCode + "', '" + TxtSeal.Text.Trim() + "')";
                    }
                    else
                    {
                        Sql1 = "Insert into Socks_Companymas values (" + Code + ", '" + TxtName.Text.Trim() + "', '" + TxtInPrinting.Text.ToString().Trim() + "', '" + TxtAddress.Text.Trim() + "', '" + TxtPhone.Text.Trim() + "', '" + TxtFax.Text.Trim() + "', '" + TxtEmail.Text.Trim() + "', '" + TxtTin.Text.Trim() + "', '" + TxtCST.Text.Trim() + "', '" + TxtEcc.Text.Trim() + "', '" + TxtTariff.Text.Trim() + "', '" + TxtBED.Text.Trim() + "', '" + TxtAED.Text.Trim() + "', '" + TxtTax.Text.Trim() + "', '" + TxtPan.Text.Trim() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpPanDate.Value) + "', ' ', '" + String.Format("{0:dd-MMM-yyyy}", DtpFrom.Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpTo.Value) + "', '" + TxtPF.Text.Trim() + "', '" + TxtESI.Text.Trim() + "', '" + TxtLicense.Text.Trim() + "', ' ', ' ', '" + TxtMillions.Text.Trim() + "', '" + TxtCurName.Text.Trim() + "', '" + TxtCurSym.Text.Trim() + "', '" + TxtCurDecimal.Text.Trim() + "', " + MyParent.New_UserCode + ", " + MyParent.New_SysCode + ", " + MyParent.New_Today + ", " + MyParent.UserCode + ", " + MyParent.SysCode + ", " + MyParent.Today + ", " + MyParent.CompCode + ", '" + MyParent.YearCode + "', '" + TxtSeal.Text.Trim() + "')";
                    }
                    MyBase.Run(Sql, Sql1);
                    MessageBox.Show("Saved Successfully...!");
                    MyParent.Save_Error = false;
                    MyBase.Clear(this); 
                }
                else
                {
                    MessageBox.Show("Please Give Area Details ...!");
                    MyParent.Save_Error = true;
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
                MyBase.Enable_Controls(this, true);
                Dr = Tool.Selection_Tool(this, 100, 300, SelectionTool_Class.ViewType.NormalView, "Company Master - Edit", "Select CompCode, CompName, SDt, Edt, * from Socks_Companymas order by CompCode", "", 60, 300, 90, 90);
                Fill_Datas(Dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Delete()
        {
            try
            {
                if (MyParent.CompName.ToUpper().Contains("GAINUP") || MyParent.CompName.ToUpper().Contains("ALAMELU"))
                {
                    MessageBox.Show("You Does'nt Have Rights to Delete...!");
                    MyParent.Load_ViewEntry();
                    return;
                }

                MyBase.Enable_Controls(this, false);
                Dr = Tool.Selection_Tool(this, 100, 300, SelectionTool_Class.ViewType.NormalView, "Company Master - Delete", "Select CompCode, CompName, SDt, Edt, * from Socks_Companymas order by CompCode", "", 60, 300, 90, 90);
                Fill_Datas(Dr);
                if (Dr != null)
                {
                    MyParent.Load_DeleteConfirmEntry(); 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Delete_Confirm()
        {
            try
            {
                String Sql, Sql1, Sql2, Sql3, sql4, sql5, sql6, sql7, sql8, Sql9, Sql10;
                if (Code.Trim() != String.Empty)
                {
                    if (MyBase.Get_RecordCount("Voucher_Master", "Company_Code = " + Code) > 0)
                    {
                        MessageBox.Show("Can't Delete...!");
                        MyParent.Load_DeleteEntry();
                        return;
                    }
                    Sql = "Delete from Socks_Companymas where Compcode = " + Code;
                    Sql1 = "delete from ledger_master where company_Code =" + Code;
                    Sql2 = "delete from ledger_Contact where company_Code=" + Code;
                    Sql3 = "delete from ledger_Breakup where company_Code=" + Code;
                    sql4 = "delete from groupMas where company_Code=" + Code;
                    sql5 = "delete from Area_Master where company_Code=" + Code;
                    sql6 = "delete from AreaGroup_Master where company_Code =" + Code;
                    sql7 = "delete from voucher_master where company_code =" + Code;
                    sql8 = "delete from voucher_details where company_code =" + Code;
                    Sql9 = "delete from voucher_Breakup_Bills where company_code =" + Code;
                    Sql10 = "delete from voucher_group where company_Code = " + Code;
                    MyBase.Run(Sql, Sql1, Sql2, Sql3, sql4, sql5, sql6, sql7, sql8, Sql9, Sql10);
                    MessageBox.Show("Deleted Successfully ...!");
                    MyBase.Clear(this);
                    MyParent.Load_DeleteEntry(); 
                }
                else
                {
                    MessageBox.Show("Please Select any Company details ...!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_View()
        {
            try
            {
                MyBase.Enable_Controls(this, false);
                Dr = Tool.Selection_Tool(this, 100, 300, SelectionTool_Class.ViewType.NormalView, "Company Master - View", "Select CompCode, CompName, SDt, Edt, * from Socks_Companymas order by CompCode", "", 60, 300, 90, 90);
                Fill_Datas(Dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Print()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                if (Dr != null)
                {
                    Code = Convert.ToString(Dr["CompCode"]);
                    TxtName.Text = Convert.ToString(Dr["CompName"]);
                    TxtInPrinting.Text = Dr["InPrinting"].ToString();
                    TxtAddress.Text = Dr["CompAddress"].ToString();
                    TxtPhone.Text = Dr["CompPhone"].ToString();
                    TxtFax.Text = Dr["CompFax"].ToString();
                    TxtEmail.Text = Dr["CompEmail"].ToString();
                    TxtTin.Text = Dr["CompTNGSTNo"].ToString();
                    TxtCST.Text = Dr["CompCstno"].ToString();
                    TxtEcc.Text = Dr["CompEccno"].ToString();
                    TxtTariff.Text = Dr["CompTANNo"].ToString();
                    TxtBED.Text = Dr["CompBedno"].ToString();
                    TxtAED.Text = Dr["CompAedNO"].ToString();
                    TxtSeal.Text = Dr["Sealname"].ToString();
                    TxtTax.Text = Dr["CompTaxDeduction"].ToString();
                    TxtPan.Text = Dr["CompPanno"].ToString();
                    DtpPanDate.Value = Convert.ToDateTime(String.Format("{0:dd-MMM-yyyy}", Dr["CompPanDt"]));
                    DtpFrom.Value = Convert.ToDateTime(String.Format("{0:dd-MMM-yyyy}", Dr["SDt"]));
                    DtpTo.Value = Convert.ToDateTime(String.Format("{0:dd-MMM-yyyy}", Dr["EDt"]));
                    TxtPF.Text = Dr["CompPFno"].ToString();
                    TxtESI.Text = Dr["CompEsiNo"].ToString();
                    TxtLicense.Text = Dr["LicenseNo"].ToString();
                    TxtMillions.Text = Dr["Millions"].ToString();
                    TxtCurName.Text = Dr["CurName"].ToString();
                    TxtCurSym.Text = Dr["CurSymbol"].ToString();
                    TxtCurDecimal.Text = Dr["CurDecName"].ToString();
                    MyParent.Vew_Help("Socks_Companymas", "CompCode = " + Code);
                }
                else
                {
                    Code = String.Empty; 
                    MyBase.Clear(this);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TxtStateNAme_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (MyParent._New == true || MyParent.Edit == true)
                    {
                        MyParent.Load_SaveEntry();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message);
            }
        }

        private void TxtStateNAme_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Phone(TxtCST, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtBreakup_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(121) || e.KeyChar == Convert.ToChar(89))
                {
                    e.Handled = true;
                    TxtCST.Text = "Y";
                }
                else if (e.KeyChar == Convert.ToChar(110) || e.KeyChar == Convert.ToChar(78))
                {
                    e.Handled = true;
                    TxtCST.Text = "N";
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void TxtBreakup_TextChanged(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Phone(TxtPhone, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtMillions_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(121) || e.KeyChar == Convert.ToChar(89))
                {
                    e.Handled = true;
                    TxtMillions.Text = "Y";
                }
                else if (e.KeyChar == Convert.ToChar(110) || e.KeyChar == Convert.ToChar(78))
                {
                    e.Handled = true;
                    TxtMillions.Text = "N";
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

        private void TxtTin_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Number(TxtTin, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GBMain_Enter(object sender, EventArgs e)
        {

        }
    }
}