using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Text;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmProduct_Master : Form, Entry
    {

        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        DataRow Dr;

        public FrmProduct_Master()
        {
            InitializeComponent();
        }

        void Get_Max_Product_NO()
        {
            try
            {
                DataTable Dt1 = new DataTable();
                MyBase.Load_Data("Select (Isnull(Max(Cast(Replace(Product_No, 'HO/', '') as Bigint)), 0) + 1) From VFit_Sample_Product_Master ", ref Dt1);
                TxtProductNo.Text = "HO/" + Dt1.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmProduct_Master_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                TxtItem.Focus();
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
                //Get_Max_Product_NO();
                TxtItem.Focus();
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

                if (TxtItem.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Item ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtItem.Focus();
                    return;
                }

                if (TxtColor.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Color ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtColor.Focus();
                    return;
                }

                if (TxtSize.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Size ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtSize.Focus();
                    return;
                }

                if (MyParent._New)
                {
                    Get_Max_Product_NO();
                    MyBase.Run("Insert into VFit_Sample_Product_Master (Product_No, ItemID, ColorID, SizeID, Usercode, Syscode, EntryAt) Values ('" + TxtProductNo.Text + "', " + TxtItem.Tag.ToString() + ", " + TxtColor.Tag.ToString() + ", " + TxtSize.Tag.ToString() + ", " + MyParent.UserCode + ", " + MyParent.SysCode + ", GetDate())");
                }
                else
                {
                    MyBase.Run("Update VFit_Sample_Product_Master Set ItemID = " + TxtItem.Tag.ToString() + ", ColorID = " + TxtColor.Tag.ToString() + ", SizeID = " + TxtSize.Tag.ToString() + ", UserCode = " + MyParent.UserCode + ", Syscode = " + MyParent.SysCode + ", EntryAt = GetDate() Where RowID = " + TxtProductNo.Tag.ToString());
                }

                MessageBox.Show("Saved ...!", "Gainup");
                MyParent.Save_Error = false;
                MyBase.Clear(this);

            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Product - Edit", "Select V1.Product_No, I1.Item, C1.color, S1.size, V1.ItemID, V1.ColorID, V1.SizeID, V1.RowID From VFit_Sample_Product_Master V1 Left Join Item I1 On V1.ItemID = I1.itemid Left join Color C1 On V1.ColorID = C1.colorid Left join Size S1 On V1.SizeID = S1.sizeid", String.Empty, 120, 150, 150, 150);
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

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Product - Delete", "Select V1.Product_No, I1.Item, C1.color, S1.size, V1.ItemID, V1.ColorID, V1.SizeID, V1.RowID From VFit_Sample_Product_Master V1 Left Join Item I1 On V1.ItemID = I1.itemid Left join Color C1 On V1.ColorID = C1.colorid Left join Size S1 On V1.SizeID = S1.sizeid", String.Empty, 120, 150, 150, 150);
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
                if (TxtItem.Text.Trim() == String.Empty || TxtColor.Text.Trim() == String.Empty || TxtSize.Text.Trim() == String.Empty || TxtProductNo.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                }
                else
                {
                    MyBase.Run("Delete From VFit_Sample_Product_Master Where RowID = " + TxtProductNo.Tag.ToString());
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Product - View", "Select V1.Product_No, I1.Item, C1.color, S1.size, V1.ItemID, V1.ColorID, V1.SizeID, V1.RowID From VFit_Sample_Product_Master V1 Left Join Item I1 On V1.ItemID = I1.itemid Left join Color C1 On V1.ColorID = C1.colorid Left join Size S1 On V1.SizeID = S1.sizeid", String.Empty, 120, 150, 150, 150);
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
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Fill_Datas(DataRow Dr)
        {
            try
            {
                TxtProductNo.Tag = Dr["RowID"].ToString();
                TxtProductNo.Text = Dr["Product_No"].ToString();
                TxtItem.Text = Dr["Item"].ToString();
                TxtItem.Tag = Dr["ItemID"].ToString();
                TxtColor.Text = Dr["Color"].ToString();
                TxtColor.Tag = Dr["ColorID"].ToString();
                TxtSize.Text = Dr["Size"].ToString();
                TxtSize.Tag = Dr["SizeID"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmProduct_Master_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtSize")
                    {
                        if (MyParent._New || MyParent.Edit)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtItem")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item", "Select Item, ItemID from Item Where Item_Type = 'Yarn' And Item Not Like 'ZZZ%' And LEN(LTRIM(RTRIM(Item))) > 1 Order By Item", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            TxtItem.Text = Dr["Item"].ToString();
                            TxtItem.Tag = Dr["ItemID"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtColor")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color", "Select Color, ColorID from Color Where Color Not Like '%ZZZ' And LEN(LTRIM(RTRIM(Color))) > 1 Order By Color", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            TxtColor.Text = Dr["Color"].ToString();
                            TxtColor.Tag = Dr["ColorID"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtSize")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size", "Select Size, SizeID from Size Where Item_Type = 'Yarn' And LEN(LTRIM(RTRIM(Size))) > 1 And Size Not Like 'ZZZ%' Order By Size", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            TxtSize.Text = Dr["Size"].ToString();
                            TxtSize.Tag = Dr["SizeID"].ToString();
                        }                        
                    }
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
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

        private void FrmProduct_Master_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}