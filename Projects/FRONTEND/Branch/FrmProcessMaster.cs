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
    public partial class FrmProcessMaster : Form, Entry 
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        Int32 Code;

        public FrmProcessMaster()
        {
            InitializeComponent();
        }

        void TxtBankShortName_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (Convert.ToInt16(e.KeyChar) == Convert.ToInt16(Keys.Enter))
                {
                    e.Handled = true;
                }
                else
                {
                    MyBase.Return_Ucase(e);
                }
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
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        private void FrmBankMaster_Load(object sender, EventArgs e)
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


        void TxtCustomerAddress_GotFocus(object sender, System.EventArgs e)
        {
            try
            {
                this.KeyPreview = false; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        void TxtCustomerAddress_LostFocus(object sender, System.EventArgs e)
        {
            try
            {
                this.KeyPreview = true;
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
                    if (this.ActiveControl.Name == "TxtRemarks")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
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


        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                TxtName.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Save()
        {
            try
            {
                if (TxtName.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Name ...!", "Gainup");
                    TxtName.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (MyParent._New)
                {
                    MyBase.Execute("Insert into VFit_Sample_Process_Master (Name, Remarks, UserCode, Syscode, EntryAt) values ('" + TxtName.Text.Trim() + "', '" + TxtRemarks.Text.Trim() + "', " + MyParent.UserCode + ", " + MyParent.SysCode + ", getdate())");
                }
                else
                {
                    MyBase.Run("update VFit_Sample_Process_Master Set Name = '" + TxtName.Text.Trim() + "', Entryat = GetDate(), Remarks = '" + TxtRemarks.Text.Trim() + "' Where RowID = " + Code);
                }
                MessageBox.Show("Saved ...!", "Gainup");
                MyBase.Clear(this);
                MyParent.Save_Error = false;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().Contains("CONSTRAINT"))
                {
                    MessageBox.Show("Already Name available ...!", "Gainup");
                    TxtName.SelectAll();
                    TxtName.Focus();
                }
                else
                {
                    throw ex;
                }
                MyParent.Save_Error = true;
            }
        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Enable_Controls(this, true);
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "Instruction Master - Edit", "Select Name, Remarks, RowID from VFit_Sample_Process_Master order by Name", "", 250, 200);
                Fill_Datas(Dr);
                TxtName.Focus();
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
                MyBase.Enable_Controls(this, false);
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "Instruction Master - Delete", "Select Name, Remarks, RowID from VFit_Sample_Process_Master order by Name", "", 250, 200);
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
                String Sql;
                if (Code > 0)
                {
                    Sql = "Delete from VFit_Sample_Process_Master Where RowID = " + Code;
                    MyBase.Execute(Sql);
                    MessageBox.Show("Deleted Successfully ...!");
                    MyBase.Clear(this);
                    MyParent.Load_DeleteEntry(); 
                }
                else
                {
                    MessageBox.Show("Please Select any Description details ...!");
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
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "Instruction Master - View", "Select Name, Remarks, RowID From VFit_Sample_Process_Master order by Name", "", 250, 200);
                Fill_Datas(Dr);
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
                    Code = Convert.ToInt32(Dr["RowID"]);
                    TxtName.Text = Dr["Name"].ToString();
                    TxtRemarks.Text = Dr["Remarks"].ToString();
                }
                else
                {
                    Code = 0; 
                    MyBase.Clear(this);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TxtUserPass_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void TxtUserPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmUserMaster_Deactivate(object sender, EventArgs e)
        {
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}