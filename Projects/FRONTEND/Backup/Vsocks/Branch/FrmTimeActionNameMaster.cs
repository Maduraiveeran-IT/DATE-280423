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
    public partial class FrmTimeActionNameMaster : Form,Entry 
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataRow Dr;
        Int64 code;
        String[] Queries;
        public FrmTimeActionNameMaster()
        {
            InitializeComponent();
        }

        private void FrmTimeActionNameMaster_Load(object sender, EventArgs e)
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
                Option1.Checked = true;
                Option3.Checked = true;
                TxtDivision.Focus();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time&Action Name", "Select A.Name, B.Company Division, A.Order_SlNo, A.Short_Name, A.Edit_Flag, A.Follow_By, A.Division_ID, A.Rowid From Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() B On A.Division_ID = B.COMPCODE ORder By A.Order_SlNo  ", string.Empty, 300, 150, 80);
                if (Dr != null)
                {
                    code = Convert.ToInt64(Dr["Rowid"]);
                    TxtTimeActionName.Text = Dr["Name"].ToString();
                    TxtTimeActionName.Tag  = Dr["Rowid"].ToString();
                    TxtShortName.Text = Dr["Short_Name"].ToString();
                    TxtDivision.Text = Dr["Division"].ToString();
                    TxtDivision.Tag  = Dr["Division_ID"].ToString();
                    TxtOrderSNo.Text = Dr["Order_SlNo"].ToString();
                    if (Dr["Edit_Flag"].ToString() == "Y")
                    {
                        Option1.Checked = true;
                    }
                    else
                    {
                        Option2.Checked = true;
                    }
                    if (Dr["Follow_By"].ToString() == "S")
                    {
                        Option3.Checked = true;
                    }
                    else
                    {
                        Option4.Checked = true;
                    }
                    TxtTimeActionName.Focus();
                }               
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
                Int32 Array_Index = 0;
                if (TxtTimeActionName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Empty Name ");
                    TxtTimeActionName.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtDivision.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Empty Division");
                    TxtDivision.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtOrderSNo.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Empty OrderSlNo ");
                    TxtOrderSNo.Focus();
                    MyParent.Save_Error = true;
                    return;
                } 
                if (TxtShortName.Text.Trim() == string.Empty)
                {
                    TxtShortName.Text = "";
                }
                if (MyParent._New == true)
                {
                    Queries = new String[3];
                    Queries[Array_Index++] = "Insert Into Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master (Name, Short_Name, Division_ID, Order_SlNo, Edit_Flag, Follow_By) Values('" + TxtTimeActionName.Text + "', '" + TxtShortName.Text + "', " + TxtDivision.Tag + ", " + TxtOrderSNo.Text + ", (Case When 'True' = '" + Option1.Checked + "'  Then '" + Option1.Text + "' Else '" + Option2.Text + "' End), (Case When 'True' = '" + Option3.Checked + "'  Then '" + Option3.Text + "' Else '" + Option4.Text + "' End)); Select Scope_Identity()";
                    Queries[Array_Index++] = MyParent.EntryLog("TIME & ACTION NAME MASTER", "ADD", "@@IDENTITY");
                    MyBase.Run_Identity(false, Queries);                    
                }
                else
                {
                    Queries = new String[3];
                    Queries[Array_Index++] = "Update Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master Set Name = '" + TxtTimeActionName.Text + "', Short_Name = '" + TxtShortName.Text + "' , Division_ID = " + TxtDivision.Tag + ", ORder_SlNo = " + TxtOrderSNo.Text + ", Edit_Flag = (Case When 'True' =  '" + Option1.Checked + "'  Then '" + Option1.Text + "' Else '" + Option2.Text + "' End), Follow_By = (Case When 'True' = '" + Option3.Checked + "'  Then '" + Option3.Text + "' Else '" + Option4.Text + "' End) where Rowid = " + code;
                    Queries[Array_Index++] = MyParent.EntryLog("TIME & ACTION NAME MASTER", "EDIT", code.ToString());
                    MyBase.Run_Identity(true, Queries);                    
                }
                MessageBox.Show("Saved ...!", "Gainup");
                MyBase.Clear(this);
                MyParent.Save_Error = false;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().Contains("UNIQUE"))
                {
                    MessageBox.Show("Already Name Available For This Division ...!", "Gainup");
                    TxtTimeActionName.SelectAll();
                    TxtTimeActionName.Focus();
                }
                else
                {
                    throw ex;
                }
                MyParent.Save_Error = true;
            }
        }

        public void Entry_Print()
        {
            try
            {
                MyBase.Clear(this);
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
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time&Action Name", "Select A.Name, B.Company Division, A.Order_SlNo, A.Short_Name, A.Edit_Flag, A.Follow_By, A.Division_ID, A.Rowid From Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() B On A.Division_ID = B.COMPCODE ORder By A.Order_SlNo  ", string.Empty, 300, 150, 80);
                if (Dr != null)
                {
                    code = Convert.ToInt64(Dr["Rowid"]);
                    TxtTimeActionName.Text = Dr["Name"].ToString();
                    TxtTimeActionName.Tag = Dr["Rowid"].ToString();
                    TxtShortName.Text = Dr["Short_Name"].ToString();
                    TxtDivision.Text = Dr["Division"].ToString();
                    TxtDivision.Tag = Dr["Division_ID"].ToString();
                    TxtOrderSNo.Text = Dr["Order_SlNo"].ToString();
                    if (Dr["Edit_Flag"].ToString() == "Y")
                    {
                        Option1.Checked = true;
                    }
                    else
                    {
                        Option2.Checked = true;
                    }
                    if (Dr["Follow_By"].ToString() == "S")
                    {
                        Option3.Checked = true;
                    }
                    else
                    {
                        Option4.Checked = true;
                    }
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

                if (code > 0)
                {
                    MyBase.Run("Delete from Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master where RowID = " + code, MyParent.EntryLog("TIME & ACTION NAME MASTER", "DELETE", code.ToString()));                     
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
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Time&Action Name", "Select A.Name, B.Company Division, A.Order_SlNo, A.Short_Name, A.Edit_Flag, A.Follow_By, A.Division_ID, A.Rowid From Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Division_Mas() B On A.Division_ID = B.COMPCODE ORder By A.Order_SlNo  ", string.Empty, 300, 150, 80);
                if (Dr != null)
                {
                    code = Convert.ToInt64(Dr["Rowid"]);
                    TxtTimeActionName.Text = Dr["Name"].ToString();
                    TxtTimeActionName.Tag = Dr["Rowid"].ToString();
                    TxtShortName.Text = Dr["Short_Name"].ToString();
                    TxtDivision.Text = Dr["Division"].ToString();
                    TxtDivision.Tag = Dr["Division_ID"].ToString();
                    TxtOrderSNo.Text = Dr["Order_SlNo"].ToString();
                    if (Dr["Edit_Flag"].ToString() == "Y")
                    {
                        Option1.Checked = true;
                    }
                    else
                    {
                        Option2.Checked = true;
                    }
                    if (Dr["Follow_By"].ToString() == "S")
                    {
                        Option3.Checked = true;
                    }
                    else
                    {
                        Option4.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmTimeActionNameMaster_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {               
                if (e.KeyCode == Keys.Escape)
                {
                    MyBase.ActiveForm_Close(this, MyParent);
                }

                if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtDivision")
                    {
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Division", "Select COMPANY, COMPCODE  From Vaahini_Erp_Gainup.Dbo.Division_Mas () Where CompCode in (3,4) ORder by COMPCODE ", String.Empty, 400);
                        if (Dr != null)                        
                        {
                            TxtDivision.Text = Dr["COMPANY"].ToString();
                            TxtDivision.Tag = Dr["COMPCODE"].ToString();
                            TxtOrderSNo.Text = MyBase.MaxWOCC("Vaahini_Erp_Gainup.Dbo.Time_Action_Name_Master", "Order_SlNo", " Division_ID = " + TxtDivision.Tag + "").ToString();
                            TxtDivision.Focus();
                            return;
                        }
                    }
                }
    
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtOrderSNo")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    SendKeys.Send("{tab}");
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FrmTimeActionNameMaster_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {
                if (this.ActiveControl.Name == "TxtOrderSNo")
                {
                    MyBase.Valid_Number((TextBox)this.ActiveControl, e);
                }
                else if (this.ActiveControl.Name != "TxtDivision")
                {
                    MyBase.Return_Ucase(e);
                }
                else
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
            }
        }
           

    }
}