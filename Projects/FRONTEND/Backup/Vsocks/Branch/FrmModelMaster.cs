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
    public partial class FrmModelMaster : Form, Entry 
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        Int32 Code;
        public FrmModelMaster()
        {
            InitializeComponent();
        }

        private void TxtModel_KeyPress(object sender, KeyPressEventArgs e)
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


        private void FrmModelMaster_Load(object sender, EventArgs e)
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
                TxtModel.Focus();
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
                if (TxtModel.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Model Name ...!", "Gainup");
                    TxtModel.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtPair.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid No_Of_Pairs ...!", "Gainup");
                    TxtPair.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (MyParent._New)
                {
                    MyBase.Execute("Insert into Socks_Model (Model_Name, Uomid, UserCode, Syscode, EntryAt) values ('" + TxtModel.Text.Trim() + "', " + TxtPair.Tag + ", " + MyParent.UserCode + ", " + MyParent.SysCode + ", getdate())");
                }
                else
                {
                    MyBase.Run("update Socks_Model Set Model_Name = '" + TxtModel.Text.Trim() + "', Uomid = " + TxtPair.Tag + ", Entryat = GetDate() Where RowID = " + Code);
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
                    TxtModel.SelectAll();
                    TxtModel.Focus();
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
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "Socks_Model - Edit", "Select Model_Name, Upper(B.Name) No_Of_Pair, A.RowID, B.Rowid Uomid  from Socks_Model A Left Join Accounts.Dbo.Export_UOM_Master B on A.Uomid = B.Rowid  order by Model_Name", "", 250, 200);
                Fill_Datas(Dr);
                TxtModel.Focus();
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
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "Socks_Model - Delete", "Select Model_Name, Upper(B.Name) No_Of_Pair, A.RowID, B.Rowid Uomid  from Socks_Model A Left Join Accounts.Dbo.Export_UOM_Master B on A.Uomid = B.Rowid  order by Model_Name", "", 250, 200);
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
                    Sql = "Delete from Socks_Model Where RowID = " + Code;
                    MyBase.Execute(Sql);
                    MessageBox.Show("Deleted Successfully ...!");
                    MyBase.Clear(this);
                    MyParent.Load_DeleteEntry();
                }
                else
                {
                    MessageBox.Show("Please Select Model ...!");
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
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "Socks_Mode - View", "Select Model_Name, Upper(B.Name) No_Of_Pair, A.RowID, B.Rowid Uomid from Socks_Model A Left Join Accounts.Dbo.Export_UOM_Master B on A.Uomid = B.Rowid  order by Model_Name", "", 250, 200);
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
                    TxtModel.Text = Dr["Model_Name"].ToString();
                    TxtPair.Text = Dr["No_Of_Pair"].ToString();
                    TxtPair.Tag = Dr["Uomid"].ToString();                    
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

        private void FrmModelMaster_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "TxtPair")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtModel")
                    {
                        TxtPair.Focus();
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtPair")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Pair", "Select Name, Rowid from accounts.dbo.Export_UOM_Master ", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            TxtPair.Text = Dr["Name"].ToString();
                            TxtPair.Tag = Dr["Rowid"].ToString();
                        }
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

        private void TxtPair_KeyPress(object sender, KeyPressEventArgs e)
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

        private void FrmModelMaster_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtPair")
                {
                    //MyBase.Valid_Number((TextBox)this.ActiveControl, e);
                }
                else if (this.ActiveControl.Name == "TxtModel")
                {
                    //e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
                }                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
