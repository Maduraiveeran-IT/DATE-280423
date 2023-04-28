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
    public partial class Frm_Size_Master : Form, Entry 
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        Int32 Code;

        public Frm_Size_Master()
        {
            InitializeComponent();
        }

        private void TxtName_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Frm_Size_Master_Load(object sender, EventArgs e)
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

        private void Frm_Size_Master_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "TxtName")
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
                    MyBase.Execute("Insert into Size (size,Item_Type,SizeCount,YarnLength,BundleWeight) values ('" + TxtName.Text.Trim() + "','Garment',0,0,0)");
                }
                else
                {
                    MyBase.Run("update Size Set Size = '" + TxtName.Text.Trim() + "' Where sIZEID = " + Code);
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
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "Size Master - Edit", "Select Size,Item_Type,sizeID,lookup,Seq,SizeSeqid,Sourceid,SizeCount,YarnLength,BundleWeight from Size where Item_Type='Garment' order by Size", "", 250, 200);
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
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "Size Master - Delete", "Select Size,Item_Type,sizeID,lookup,Seq,SizeSeqid,Sourceid,SizeCount,YarnLength,BundleWeight from Size where Item_Type='Garment' order by Size", "", 250, 200);
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
                    Sql = "Delete from Size Where SizeID = " + Code;
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
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "Size Master - View", "Select Size,Item_Type,sizeID,lookup,Seq,SizeSeqid,Sourceid,SizeCount,YarnLength,BundleWeight from Size where Item_Type='Garment' order by Size", "", 250, 200);
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
                    Code = Convert.ToInt32(Dr["SizeID"]);
                    TxtName.Text = Dr["Size"].ToString();
                    //TxtRemarks.Text = Dr["Remarks"].ToString();
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
    }
}