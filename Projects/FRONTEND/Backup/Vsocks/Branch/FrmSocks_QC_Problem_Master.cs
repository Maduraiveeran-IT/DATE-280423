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
    public partial class FrmSocks_QC_Problem_Master : Form, Entry 
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        Int32 Code;

        public FrmSocks_QC_Problem_Master()
        {
            InitializeComponent();
        }

        private void FrmSocks_QC_Problem_Master_Load(object sender, EventArgs e)
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
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                TxtCustomerName.Focus();
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
                if (TxtCustomerName.Text.Trim() == String.Empty || TxtType.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Name ...!", "Gainup");
                    TxtCustomerName.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (MyParent._New)
                {
                    MyBase.Run("Insert into Socks_Qc_Problem_Master(Name, Type) values ('" + TxtCustomerName.Text.Trim() + "', '" + TxtType.Text.Trim() + "')");
                }
                else
                {
                    MyBase.Run("update Socks_Qc_Problem_Master Set Name = '" + TxtCustomerName.Text.Trim() + "', Type = '" + TxtType.Text.Trim() + "' where RowID = " + Code);
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
                    TxtCustomerName.SelectAll();
                    TxtCustomerName.Focus();
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
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "QC Problem Master - Edit", "Select Name, Type, RowID from Socks_Qc_Problem_Master order by Name", "", 250, 250);
                Fill_Datas(Dr);
                TxtCustomerName.Focus();
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
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "QC Problem Master - Delete ", "Select Name, Type, RowID from Socks_Qc_Problem_Master order by Name", "", 250, 250);
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
                    Sql = "Delete from Socks_Qc_Problem_Master where RowID = " + Code;
                    MyBase.Run(Sql);
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
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, " QC Problem Master - View", "Select Name, Type, RowID from Socks_Qc_Problem_Master order by Name", "", 250, 250);
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
                    TxtCustomerName.Text = Dr["Name"].ToString();
                    TxtType.Text = Dr["Type"].ToString();   
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

        private void FrmSocks_QC_Problem_Master_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void TxtBankName_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        void Type_Selection()
        {
            try
            {
                Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Type", "Select 'KNITTING' Type Union Select 'LINKING' Union Select 'BOARDING' Union Select 'STITCHING' Union Select 'PAIRING' Union Select 'PACKING' Union Select 'YARN' ", String.Empty, 150);
                if (Dr != null)
                {
                    TxtType.Text = Dr["Type"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmSocks_QC_Problem_Master_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (ActiveControl.Name == "TxtCustomerName")
                    {
                        TxtType.Focus(); 
                    }
                    else if (ActiveControl.Name == "TxtType")
                    {
                        MyParent.Load_SaveEntry();
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtType")
                    {
                        Type_Selection();
                    }
                }
                else if (e.KeyCode == Keys.Return)
                {
                    if (this.ActiveControl.Name == "TxtType")
                    {
                        MyParent.Load_SaveEntry();
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
    }
}