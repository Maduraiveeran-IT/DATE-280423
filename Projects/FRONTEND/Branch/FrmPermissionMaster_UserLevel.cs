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
    public partial class FrmPermissionMaster_User_Level_Fixed : Form, Entry 
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        int New, Edit, Delete, Preview, Print, View;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        String Code;

        public FrmPermissionMaster_User_Level_Fixed()
        {
            InitializeComponent();
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

        void Clear()
        {
            try
            {
                TxtBankName.Text = String.Empty;
                TxtBankName.Tag = string.Empty;
                treeView1.Nodes.Clear();
                Load_tree();
                ChkCopy.Checked = false;
                TxtCopyFrom.Text = String.Empty;
                TxtCopyFrom.Tag = string.Empty;
                TxtCopyFrom.Enabled = false;
                TxtBankName.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void FrmBankMaster_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                TableCreation();
                Clear();
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
                    SendKeys.Send("{Tab}"); 
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



        void TableCreation()
        {
            try
            {
                if (MyBase.Check_Table("Socks_Permission_Master_User_Level_Fixed") == false)
                {
                    MyBase.Execute("Create table Socks_Permission_Master_User_Level_Fixed (Menu_Code int, User_Level_Code int)");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        void Load_tree()
        {
            DataTable Dt = new DataTable();
            DataTable Dt1 = new DataTable();
            DataTable Dt2 = new DataTable();
            try
            {
                
                MyBase.Load_Data("Select Menu_Code Code, Menu_Name Menu From Menu_Master Where Under = 'Main' order by Menu_Code", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    treeView1.Nodes.Add(Dt.Rows[i]["Code"].ToString(), Dt.Rows[i]["Menu"].ToString().Replace("&",""));
                    MyBase.Load_Data("Select Menu_Code Code, Menu_Name Menu, Under From Menu_Master Where under = '" + Dt.Rows[i]["Code"].ToString() + "' order by Menu_Code", ref Dt1);
                    for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                    {
                        treeView1.Nodes[Dt1.Rows[j]["Under"].ToString()].Nodes.Add(Dt1.Rows[j]["Code"].ToString(), Dt1.Rows[j]["Menu"].ToString().Replace("&", ""));
                        MyBase.Load_Data("Select Menu_Code Code, Menu_Name Menu, Under From Menu_Master Where under = '" + Dt1.Rows[j]["Code"].ToString() + "' order by Menu_Code", ref Dt2);
                        for (int k = 0; k <= Dt2.Rows.Count - 1; k++)
                        {
                            treeView1.Nodes[Dt1.Rows[j]["Under"].ToString()].Nodes[Dt2.Rows[k]["Under"].ToString()].Nodes.Add(Dt2.Rows[k]["Code"].ToString(), Dt2.Rows[k]["Menu"].ToString().Replace("&", ""));
                        }
                    }
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
                
                TxtBankName.Focus();  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Save()
        {
            String Sql;
            try
            {
                if (Code.Trim() != String.Empty && TxtBankName.Text.Trim() != String.Empty)
                {
                    if (MyParent._New == true)
                    {
                        Code = Convert.ToString(MyBase.Max("Area_Master", "Area_Code", "", MyParent.YearCode, MyParent.CompCode));
                    }
                    if (MyBase.Get_RecordCount("Area_Master", "Area_Name = '" + TxtBankName.Text + "' and Area_Code <> " + Code) > 0)
                    {
                        MessageBox.Show("Area Already Exists ...!");
                        TxtBankName.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                    Sql = "Delete From Area_master where Area_Code = " + Code;
                    MyBase.Execute(Sql);
                    Sql = "Insert into Area_Master values (" + Code + "," + Code + ",'" + TxtBankName.Text + "'," + MyParent.UserCode + "," + MyParent.SysCode + ","+ MyParent.Today +","+ MyParent.UserCode +","+ MyParent.SysCode +","+ MyParent.Today +","+ MyParent.CompCode +",'"+ MyParent.YearCode +"')";  
                    MyBase.Save(Sql);
                    MessageBox.Show("Saved Successfully...!");
                    MyBase.Clear(this); 
                }
                else
                {
                    MessageBox.Show("Please Give Area Details ...!"); 
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
                Dr = Tool.Selection_Tool(this,100,300, SelectionTool_Class.ViewType.NormalView, "Area Master - Edit", "Select Area_Code Code, Area_name Name from Area_Master order by Area_Code","", 100, 200);
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
                MyBase.Enable_Controls(this, false);
                Dr = Tool.Selection_Tool(this, 100, 300, SelectionTool_Class.ViewType.NormalView, "Area Master - Delete", "Select Area_Code Code, Area_name Name from Area_Master order by Area_Code","", 100, 200);
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
                if (Code.Trim() != String.Empty)
                {
                    Sql = "Delete from Area_Master where Area_Code = " + Code;
                    MyBase.Execute(Sql);
                    MessageBox.Show("Deleted Successfully ...!");
                    MyBase.Clear(this);
                    MyParent.Load_DeleteEntry(); 
                }
                else
                {
                    MessageBox.Show("Please Select any Area details ...!");
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
                Dr = Tool.Selection_Tool(this, 100, 300, SelectionTool_Class.ViewType.NormalView, "Area Master - View", "Select Area_Code Code, Area_name Name from Area_Master order by Area_Code","", 100, 200);
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
                    Code = Convert.ToString(Dr["Code"]);
                    TxtBankName.Text = Convert.ToString(Dr["Name"]);
                    //MyBase.EnableContainer(this);
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

        private void TxtBankName_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select User Level ", "Select User_Level_Fixed, User_Level_Code Code from User_Level_Fixed order by User_Level_Fixed", "", 250);
                    if (Dr != null)
                    {
                        TxtBankName.Text = Dr["User_Level_Fixed"].ToString();
                        TxtBankName.Tag = Dr["Code"].ToString();
                    }
                }
                else
                {
                    MyBase.Handle_Delete(TxtBankName, e);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TxtBankName_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Null(TxtBankName, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Rights()
        {
            try
            {
                if (ChkDelete.Checked == true)
                {
                    Delete = 1;
                }
                else
                {
                    Delete = 0;
                }
                if (ChkEdit.Checked == true)
                {
                    Edit = 1;
                }
                else
                {
                    Edit = 0;
                }
                if (ChkNew.Checked == true)
                {
                    New = 1;
                }
                else
                {
                    New = 0;
                }
                if (ChkPreview.Checked == true)
                {
                    Preview = 1;
                }
                else
                {
                    Preview = 0;
                }
                if (ChkPrint.Checked == true)
                {
                    Print = 1;
                }
                else
                {
                    Print = 0;
                }
                if (ChkView.Checked == true)
                {
                    View = 1;
                }
                else
                {
                    View = 0;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Enable_Menu(String Menu_Code)
        {
            try
            {
                MyBase.Execute("Delete from Socks_Permission_Master_User_Level_Fixed where Menu_Code = " + Menu_Code + " and User_Level_Code = " + TxtBankName.Tag.ToString());
                MyBase.Execute("insert into Socks_Permission_Master_User_Level_Fixed values (" + Menu_Code + "," + TxtBankName.Tag.ToString() + ")");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ButSave_Click(object sender, EventArgs e)
        {
            TreeNode Tn;
            try
            {
                if (TxtBankName.Tag.ToString().Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid User ...");
                    TxtBankName.Focus();
                    return;
                }
                Reset();
                //Rights();
                if (ChkCopy.Checked == true)
                {
                    if (TxtCopyFrom.Text.Trim() != String.Empty)
                    {
                        if (MessageBox.Show("Sure to Copy Settings from " + TxtCopyFrom.Text.ToUpper(), "Copy ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            if (MyBase.Check_Table("tempUser"))
                            {
                                MyBase.Execute("Drop table TempUser");
                            }
                            MyBase.Execute("select * into TempUser from Socks_Permission_Master where user_Code = " + TxtCopyFrom.Tag);
                            MyBase.Execute("Update tempuser set user_Code = " + TxtBankName.Tag.ToString());
                            MyBase.Execute("insert into Socks_Permission_Master select * from tempUSer");
                            MessageBox.Show("Saved Successfully ...!");
                            Clear();
                            return;
                        }
                     }
                }
                for (int i = 0; i <= treeView1.Nodes.Count - 1; i++)
                {
                    for (int j = 0; j <= treeView1.Nodes[i].Nodes.Count - 1; j++)
                    {
                        for (int k = 0; k <= treeView1.Nodes[i].Nodes[j].Nodes.Count - 1; k++)
                        {
                            Tn = treeView1.Nodes[i].Nodes[j].Nodes[k];
                            if (Tn.Checked)
                            {
                                Enable_Menu(treeView1.Nodes[i].Name.ToString());
                                Enable_Menu(treeView1.Nodes[i].Nodes[j].Name.ToString());
                                Enable_Menu(treeView1.Nodes[i].Nodes[j].Nodes[k].Name.ToString());
                            }
                        }
                        Tn = treeView1.Nodes[i].Nodes[j];
                        if (Tn.Checked == true)
                        {
                            Enable_Menu(treeView1.Nodes[i].Name.ToString());
                            Enable_Menu(treeView1.Nodes[i].Nodes[j].Name.ToString());
                        }
                    }
                    Tn = treeView1.Nodes[i];
                    if (Tn.Checked == true)
                    {
                        Enable_Menu(treeView1.Nodes[i].Name.ToString());
                    }
                }
                MessageBox.Show("Saved Successfully ...!");
                checkBox1.Checked = false;
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButClear_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ButExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void Reset()
        {
            try
            {
                MyBase.Execute("Delete from Socks_Permission_Master_User_Level_Fixed where User_Level_Code = " + TxtBankName.Tag.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ButReset_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChkCopy_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                TxtCopyFrom.Enabled = true;
                TxtCopyFrom.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TxtCopyFrom_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Copy From", "Select User_Name, User_Code from Socks_User_Master where user_Code in (Select USer_Code from Socks_Permission_Master) order by user_Name", "", 250);
                    if (Dr != null)
                    {
                        TxtCopyFrom.Tag = Dr["User_Code"].ToString();
                        TxtCopyFrom.Text = Dr["User_Name"].ToString();
                    }
                    else
                    {
                        TxtCopyFrom.Tag = String.Empty;
                        TxtCopyFrom.Text = String.Empty;
                    }
                }
                else
                {
                    MyBase.Handle_Delete(TxtCopyFrom, e);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TxtBankName_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= treeView1.Nodes.Count - 1; i++)
                {
                    treeView1.Nodes[i].Checked = checkBox1.Checked;
                    for (int j = 0; j <= treeView1.Nodes[i].Nodes.Count - 1; j++)
                    {
                        treeView1.Nodes[i].Nodes[j].Checked = checkBox1.Checked;
                        for (int k = 0; k <= treeView1.Nodes[i].Nodes[j].Nodes.Count - 1; k++)
                        {
                            treeView1.Nodes[i].Nodes[j].Nodes[k].Checked = checkBox1.Checked;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}