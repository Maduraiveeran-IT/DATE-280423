//FileName    : FrmSupplierMaster
//Module      : Projects

//Developer details
//-----------------------
//Name of the Author : Jemini S
//Date Created                 : 07-mar-2023
//Tables Used            :  FITSOCKS.dbo.supplier,ACCOUNTS.dbo.ledger_master
//Functions Used         :   No 
//                                 
//View Used                   : 
//Crystal report File Name  :  
//Based On Ticket No     :  
//Reviewed By                :
//Based On Ticket No          :
//Reviewed By                 : Livingstone K

//Review Date                : 07-mar-2023




//Modification details
//-----------------------
//Done By                                  : 
//Modified On                            : 
//Event/Procedure/Sub/Function Name      : 
//Based On Ticket No                  : 
//Reviewed By                             : 






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
    public partial class FrmSupplierMaster : Form,Entry
        {


        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        Int64 Code;
        String Str;
       
        public FrmSupplierMaster()
        {
            InitializeComponent();
        }

        private void FrmSupplierMaster_Load(object sender, EventArgs e)
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

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                checkboxActive.Checked = true;
                txtSupplier.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Save()
        {
            String Active = String.Empty;

            try
            {
                   

                

                if (checkboxActive.Checked == true)
                {
                    Active = "Y";
                }
                else
                {
                    Active = "N";
                }
                if (txtSupplier.Text.Trim().ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Supplier !");
                    MyParent.Save_Error = true;
                    txtSupplier.Focus();
                    return;
                }
                if (txtLookUp.Text.Trim().ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Look Up !");
                    MyParent.Save_Error = true;
                    txtLookUp.Focus();
                    return;
                }
               
                if (txtAccoundsLedName.Text.Trim().ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Item Accounds Ledger Name !");
                    MyParent.Save_Error = true;
                    txtAccoundsLedName.Focus();
                    return;
                }
                if (MyParent._New)
                {
                    MyBase.Run("insert into FITSOCKS.dbo.supplier (supplier,supplier_lookup,Active,Acc_Ledger_Code) values ('" + txtSupplier.Text.ToString() + "','" + txtLookUp.Text.ToString() + "','" + Active + "'," + txtAccoundsLedName.Tag.ToString() + ")");
                }
                else {
                    MyBase.Run("UPDATE  FITSOCKS.dbo.supplier set supplier ='" + txtSupplier.Text.ToString() + "',supplier_lookup = '" + txtLookUp.Text.ToString() + "',Active = '" + Active + "',Acc_Ledger_Code = " + txtAccoundsLedName.Tag.ToString()+ " where supplierid = " + Code + "");
                
                
                }

                
                MessageBox.Show("Saved Successfully...!");
                MyParent.Save_Error = false;
                

                   
               
                MyBase.Clear(this); 


            }
            catch (Exception ex) {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "Supplier Master - Edit", " select distinct A.supplier,A.supplier_lookup , B.Ledger_Name,A.Active,A.Acc_Ledger_Code,B.Ledger_Code,A.supplierid from FITSOCKS.dbo.supplier A    left join ACCOUNTS.dbo.ledger_master B on Acc_ledger_code = ledger_code where B.COMPANY_CODE = " + MyParent.CompCode, string.Empty, 200, 150, 200, 60);



                if (Dr != null)
                {

                    Fill_Datas(Dr);

                }
            }
            
            catch (Exception ex) { 
            MessageBox.Show(ex.Message);

            }
        }

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "Supplier Master - Delete", " select distinct A.supplier,A.supplier_lookup , B.Ledger_Name,A.Active,A.Acc_Ledger_Code,B.Ledger_Code,A.supplierid from FITSOCKS.dbo.supplier A    left join ACCOUNTS.dbo.ledger_master B on Acc_ledger_code = ledger_code where B.COMPANY_CODE = " + MyParent.CompCode, string.Empty, 200, 150, 200,60);



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
                MyBase.Run("delete from fitsocks.dbo.supplier where supplierid = " + Code + "");
                MessageBox.Show("Deleted", "Gainup");
                MyBase.Clear(this);

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "Supplier Master - View", " select distinct A.supplier,A.supplier_lookup , B.Ledger_Name,A.Active,A.Acc_Ledger_Code,B.Ledger_Code,A.supplierid from FITSOCKS.dbo.supplier A    left join ACCOUNTS.dbo.ledger_master B on Acc_ledger_code = ledger_code where B.COMPANY_CODE = " + MyParent.CompCode+"", string.Empty, 200, 150, 200,60);



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
            throw new NotImplementedException();
        }

        private void FrmSupplierMaster_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                

                    if (e.KeyCode == Keys.Enter)
                    {
                        SendKeys.Send("{Tab}");
                        
                    }
                    if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
                    {
                        if (txtAccoundsLedName.Text != String.Empty)
                        {
                            MyParent.Load_SaveEntry();
                          

                        }

                    }


                       

                    else if (e.KeyCode == Keys.Down)
                    {
                        if (txtSupplier.Text.Trim().ToString() != string.Empty && txtLookUp.Text.Trim().ToString() != string.Empty)
                        {

                            if (this.ActiveControl.Name == txtSupplierType.Name)
                            {

                                Str = " select SupplierType from FITSOCKS.dbo.suppliertype where prefix = 's'";
                                Dr = Tool.Selection_Tool(this, 200, 200, SelectionTool_Class.ViewType.NormalView, "Select Supplier Type ", Str, String.Empty, 150);
                                if (Dr != null)
                                {
                                    txtSupplierType.Text = Dr["SupplierType"].ToString();

                                }
                            }
                            else if (this.ActiveControl.Name == txtAccoundsLedName.Name)
                            {

                                Str = "select distinct Ledger_Name,Ledger_Code from ACCOUNTS.dbo.ledger_MASter where Ledger_name not like  'zz%' and COMPANY_CODE = " + MyParent.CompCode + "";
                                Dr = Tool.Selection_Tool(this, 200, 200, SelectionTool_Class.ViewType.NormalView, "Select Accounds_ledger", Str, String.Empty, 450);
                                if (Dr != null)
                                {
                                    txtAccoundsLedName.Text = Dr["Ledger_Name"].ToString();
                                    txtAccoundsLedName.Tag = Dr["Ledger_Code"].ToString();

                                }
                            }
                        }
                        else
                        {
                            if (txtSupplier.Text.Trim().ToString() == string.Empty)
                            {
                                MessageBox.Show("Enter Supplier Name");
                                txtSupplier.Focus();
                            }
                            else if (txtLookUp.Text.Trim().ToString() == string.Empty)
                            {
                                MessageBox.Show("Enter look up");
                                txtLookUp.Focus();

                            }
                        }
                        
                    }
                

            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmSupplierMaster_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == txtAccoundsLedName.Name)
                {
                    e.Handled = true;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Fill_Datas(DataRow Dr) {
            try
            {
                Code = Convert.ToInt64(Dr["Supplierid"].ToString());
                txtSupplier.Text = Dr["Supplier"].ToString();


                txtLookUp.Text = Dr["supplier_lookup"].ToString();

                txtAccoundsLedName.Text = Dr["Ledger_Name"].ToString();
                txtAccoundsLedName.Tag = Dr["Acc_Ledger_Code"].ToString();
                if (Dr["Active"].ToString() == "Y")
                {

                    checkboxActive.Checked = true;

                }
                else
                {
                    checkboxActive.Checked = false;
                }


            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            }
        
        
        
        
        }
        }
}
        