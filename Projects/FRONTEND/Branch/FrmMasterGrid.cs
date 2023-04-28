using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.IO;

namespace Accounts
{
    public partial class FrmMasterGrid : Form
    {


        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain Myparent;
        DataRow Dr;
        TextBox Txt = null;
        DataTable Dt = new DataTable();
        DataTable TmpDt = new DataTable();
        String Str;
        Int64 Code = 0;
        TextBox txt_box = null;
        String Jemini = "";
        String [] Queries;



        String str = null;

        //String Proj;



        public FrmMasterGrid()
        {
            InitializeComponent();
        }

        private void FrmMasterGrid_Load(object sender, EventArgs e)
        {
            try
            {

                MyBase.Clear(this);
                Myparent = (MDIMain)this.MdiParent;
                textGridList.Text = "";
                Grid_Data();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void FrmMasterGrid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                
                

                if (e.KeyCode == Keys.Down)
                {

                    if (this.ActiveControl.Name == txtusername.Name)
                    {


                        Str = "select distinct USER_NAME, User_CODE  from Projects_User_Master";
                        Dr = Tool.Selection_Tool(this, 200, 200, SelectionTool_Class.ViewType.NormalView, "Select Projects_User_Master", Str, String.Empty, 200);
                        if (Dr != null)
                        {
                            txtusername.Text = Dr["USER_NAME"].ToString();
                            txtusername.Tag = Dr["User_CODE"].ToString();

                            textGridList.Text = "";
                            Code = 1;
                            Grid_Data();
                            dataGrid.CurrentCell = dataGrid["Name", 0];
                            dataGrid.Focus();
                            dataGrid.BeginEdit(true);
                           

                            if(textGridList.Text == String.Empty){
                            Grid_List();
                            }
           
                        }
                    }


                }
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == txtusername.Name)
                    {
                        MessageBox.Show("Invalid User Name");
                        //Grid_Data();

                        //dataGrid.CurrentCell = dataGrid["Name", 0];
                        //dataGrid.Focus();
                        //dataGrid.BeginEdit(true);
                        //return;

                    }

                }


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void dataGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (txt_box == null)
                {
                    txt_box = (TextBox)e.Control;
                    txt_box.KeyDown += new KeyEventHandler(txt_box_KeyDown);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void txt_box_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    
                    if (dataGrid.CurrentCell.ColumnIndex == dataGrid.Columns["Name"].Index)
                    {
                        Dr = Tool.Selection_Tool_Except_New("Name", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Name", "select Name, Rowid from Project_Login_Name", string.Empty, 250);
                        //Dr = Tool.Selection_Tool(this, 70, 30, SelectionTool_Class.ViewType.NormalView, "Project Name List", "select Name, Rowid from Project_Login_Name", string.Empty, 400);
                        if (Dr != null)
                        {
                            dataGrid["Name", dataGrid.CurrentCell.RowIndex].Value = Dr["Name"].ToString();
                            dataGrid["Rowid", dataGrid.CurrentCell.RowIndex].Value = Dr["Rowid"].ToString();
                            txt_box.Text = Dr["Name"].ToString();

                            Grid_List();
                            //if (textGridList.Text != string.Empty)
                            //{
                            //     //if (dataGrid["Name", dataGrid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            //
                            //    textGridList.Text = textGridList.Text + ',' + dataGrid["Rowid", dataGrid.CurrentCell.RowIndex].Value.ToString();
                            //
                            //}
                            //
                            //
                        }

                    }




                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }




        }
        void Grid_Data()
        {
            try
            {

                //if (Code == 0)
                //{
                //    str = "select Name, Rowid from  form.dbo.User_Project_Map A  Left Join PROJECTS.dbo.Project_Login_Name B on a.project_id = b.Rowid where A.user_id = " + txtusername.Tag + "";
                //}

                //else
                //{
                DataTable dt_chk = new DataTable();
                MyBase.Load_Data("select User_id from projects.dbo.User_Project_Map where user_id = '" + txtusername.Tag.ToString() + "'", ref dt_chk);

                //str = "select Name, Rowid from Project_Login_Name where Rowid in (" + Proj + ")  ";
                if (dt_chk.Rows.Count > 0)
                {
                    str = "select Name, Rowid from  projects.dbo.User_Project_Map A  Left Join PROJECTS.dbo.Project_Login_Name B on a.project_id = b.Rowid where A.user_id = " + txtusername.Tag + "";

                }

                else
                {
                    str = " select Name, Rowid from  projects.dbo.User_Project_Map A  Left Join PROJECTS.dbo.Project_Login_Name B on a.project_id = b.Rowid where 1=2";
                }

                //}

                dataGrid.DataSource = MyBase.Load_Data(str, ref Dt);
                
                

                
                //dataGrid.DataSource = Dt;
                MyBase.Grid_Colouring(ref dataGrid, Control_Modules.Grid_Design_Mode.Column_Wise);

                MyBase.Grid_Designing(ref dataGrid, ref Dt, "Rowid");
                dataGrid.Columns["Name"].HeaderText = "Project List";
                MyBase.Grid_Width(ref dataGrid, 400);
                textGridList.Text = "";





            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void dataGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {

                if (dataGrid.CurrentCell.RowIndex <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        Dt.Rows.RemoveAt(dataGrid.CurrentCell.RowIndex);
                    Grid_List();

                }
            }



            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void FrmMasterGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                
                    e.Handled = true;
                


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 Array_Index = 0;

               
                   
                        if (txtusername.Text.Trim().ToString() == String.Empty)
                        {
                            MessageBox.Show("Invalid User Name !", "GainUp");
                            Myparent.Save_Error = true;
                            txtusername.Focus();
                            return;
                        }
                        if (txtusername.Text != String.Empty)
                        {

                        if (dataGrid.Rows.Count > 1)
                        {
                            for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
                            {
                                for (int j = 0; j < dataGrid.Columns.Count - 1; j++)
                                {
                                    if (dataGrid[j, i].Value.ToString() == String.Empty || dataGrid[j, i].Value == DBNull.Value)
                                    {
                                        MessageBox.Show("Select Details in Grid", "Gainup");
                                        dataGrid.CurrentCell = dataGrid[j, i];
                                        dataGrid.Focus();
                                        dataGrid.BeginEdit(true);
                                        Myparent.Save_Error = true;
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Select Details in Grid", "Gainup");
                            dataGrid.CurrentCell = dataGrid[0, 0];
                            dataGrid.Focus();
                            dataGrid.BeginEdit(true);
                            Myparent.Save_Error = true;
                            return;
                        }

                        DataTable dt_chk = new DataTable();
                        
                        MyBase.Load_Data("select User_id from projects.dbo.User_Project_Map where user_id = " + txtusername.Tag + "", ref dt_chk);

                        if (dt_chk.Rows.Count > 0)
                        {
                            MyBase.Run("delete from  projects.dbo.User_Project_Map where user_id = " + txtusername.Tag + "");
                        }

                        //else if (txt_box.Text == String.Empty)
                        //{
                        //    MessageBox.Show("Invalid Project List !");
                        //    Myparent.Save_Error = true;
                        //    txt_box.Focus();
                        //    return;
                        //}


                        if (MessageBox.Show("Sure To Save ...!", "Save ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {


                         Queries = new String[dataGrid.Rows.Count + 3];



                     


                        for (int i = 0; i <= dataGrid.Rows.Count - 2; i++)
                        {
                            Queries[Array_Index++] = ("insert into projects.dbo.User_Project_Map (USER_ID, PROJECT_ID) values (" + txtusername.Tag + ", " + (dataGrid["Rowid", i].Value) + ")");
                           
                            
                        }
                        Queries[Array_Index++] = ("update Projects_User_Master set Project_List = '" + textGridList.Text.ToString() + "' where USER_CODE = " + txtusername.Tag + "");



                        MyBase.Run_Identity(true, Queries);
                        

                        

                            //MyBase.Run("update Projects_User_Master set Project_List = '" + textGridList.Text.ToString() + "' where USER_CODE in ("+txtusername.Tag+")");
                        

;

                        //MyBase.Run("insert into  form.dbo.User_Project_Map (USER_ID) values (" + txtusername.Tag + ")"); 
                        //MyBase.Run("insert into PROJECTS.dbo.Project_Login_Name (Name) values ('"+txt_box.Text.ToString()+"')");
                        
                        Grid_List();

                        MessageBox.Show("Saved Successfully...!");
                        Myparent.Save_Error = false;
                       
                        MyBase.Clear(this);
                        
                        Grid_Data();
                        txtusername.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                Myparent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGrid != null)
                {

                    MyBase.Clear(this);
                }
                else if (txtusername.Text != String.Empty)
                {

                    MyBase.Clear(this);

                    txtusername.Focus();

                }


                Grid_Data();
                textGridList.Text = "";

                txtusername.Focus();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void Arrow1_Click(object sender, EventArgs e)
        {
            try
            {



                    txtusername.Focus();
                    SendKeys.Send("{Down}");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGrid_KeyDown(object sender, KeyEventArgs e)
        
        {
            try
            {

                
                 if( e.KeyCode == Keys.Escape)
                {

                    if (dataGrid["Name", dataGrid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                    {                        
                        BtnSave_Click(sender, e);
                       
                        
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void Grid_List()
        {

            textGridList.Text = "";
            Jemini = "";
                for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        Jemini = dataGrid["Rowid", i].Value.ToString();
                    }
                    else
                    {
                        Jemini = Jemini + ',' + dataGrid["Rowid", i].Value.ToString();

                    }
                    


                }
                textGridList.Text = Jemini.ToString();



                textRowsCount.Text = (dataGrid.Rows.Count - 1).ToString();
                
                

                



            }
        
        }
    }








                   






               
               

                    
                
               

           

               
                    
                    

           



            






        

        




        
    










                   

                    
               
                




               