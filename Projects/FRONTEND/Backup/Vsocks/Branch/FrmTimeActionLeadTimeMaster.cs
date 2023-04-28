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
    public partial class FrmTimeActionLeadTimeMaster : Form,Entry 
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataRow Dr;
        Int64 code;
        String[] Queries;
        public FrmTimeActionLeadTimeMaster()
        {
            InitializeComponent();
        }

        private void FrmTimeActionLeadTimeMaster_Load(object sender, EventArgs e)
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
                TxtLeadTime.Focus();
                
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
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Lead Time - Edit", "Select Lead_Time,  Rowid From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master  ", string.Empty, 150);
                if (Dr != null)
                {
                    code = Convert.ToInt64(Dr["Rowid"]);
                    TxtLeadTime.Text = Dr["Lead_Time"].ToString();                    
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
                if (TxtLeadTime.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invali LeadDays ");
                    TxtLeadTime.Focus();
                    MyParent.Save_Error = true;
                    return;
                }               
               
                if (MyParent._New == true)
                {
                    Queries = new String[3];
                    Queries[Array_Index++] = "Insert Into Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master (LEad_Time) Values('" + TxtLeadTime.Text + "'); Select Scope_Identity()";
                    Queries[Array_Index++] = MyParent.EntryLog("TIME & ACTION LEADTIME MASTER", "ADD", "@@IDENTITY");
                    MyBase.Run_Identity(false, Queries);                    
                }
                else
                {
                    Queries = new String[3];
                    Queries[Array_Index++] = "Update Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master Set LEad_Time = '" + TxtLeadTime.Text + "' where Rowid = " + code;
                    Queries[Array_Index++] = MyParent.EntryLog("TIME & ACTION LEADTIME MASTER", "EDIT", code.ToString());
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
                    MessageBox.Show("Already LeadDays Available ...!", "Gainup");
                    TxtLeadTime.SelectAll();
                    TxtLeadTime.Focus();
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
               Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Lead Time - Delete", "Select Lead_Time,  Rowid From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master  ", string.Empty, 150);
                if (Dr != null)
                {
                    code = Convert.ToInt64(Dr["Rowid"]);
                    TxtLeadTime.Text = Dr["Lead_Time"].ToString();   
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
                    MyBase.Run("Delete From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master Where Rowid = " + code, MyParent.EntryLog("TIME & ACTION LEADTIME MASTER", "DELETE", code.ToString()));                     
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
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Lead Time - View", "Select Lead_Time,  Rowid From Vaahini_Erp_Gainup.Dbo.Time_Action_LeadTimeDays_Master  ", string.Empty, 150);
                if (Dr != null)
                {
                    code = Convert.ToInt64(Dr["Rowid"]);
                    TxtLeadTime.Text = Dr["Lead_Time"].ToString();                    
                }     
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmTimeActionLeadTimeMaster_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {               
                if (e.KeyCode == Keys.Escape)
                {
                    MyBase.ActiveForm_Close(this, MyParent);
                }
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtLeadTime")
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
        private void FrmTimeActionLeadTimeMaster_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {
                MyBase.Valid_Number((TextBox)this.ActiveControl, e);                                 
            }
        }

           

    }
}