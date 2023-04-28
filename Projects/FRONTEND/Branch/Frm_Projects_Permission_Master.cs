using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using SelectionTool_NmSp;
using System.Text;
using Accounts_ControlModules;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Accounts
{
    public partial class Frm_Projects_Permission_Master : Form,Entry 
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        DataRow Dr;
        TextBox Txt;
        DataTable Grid_Dt = new DataTable();
        String Str = String.Empty;
        Int32 Current_ID = 0;
        String StrDelete = String.Empty;
        DataTable Permission_Dt = new DataTable();

        public Frm_Projects_Permission_Master()
        {
            InitializeComponent();
        }


         private void buildtree()        
         {       
             try
             {
             treeView1.Nodes.Clear();    // Clear any existing items
             treeView1.BeginUpdate();    // prevent overhead and flicker
             treeView1.EndUpdate();      // re-enable the tree
             treeView1.Refresh();        // refresh the treeview display 
         
//             LoadBaseNodes();            // load all the lowest tree nodes
             }
             catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            }

        void LoadBaseNodes()
        {            
            //int baseParent = 0;                 // Find the lowest root category parent value
            //TreeNode node;
            String Menu_Head_ID = String.Empty;
            String strMenuHead = String.Empty;
            String strMenuType = String.Empty;
            DataTable Temp_Dt = new DataTable();
            int intRowID = 0;
            try
            {

                Str = "Select Menu_Head_ID, Menu_Head, Menu_Type from Menu_Head";

                Str = "Select Menu_CName Menu_Head_ID, Menu_name Menu_Head, Menu_CName Menu_Head_ID from Projects.dbo.Projects_Menu_Master_New where under_Menu_CName = 'Main'";

                MyBase.Load_Data(Str, ref Temp_Dt);
                treeView1.CheckBoxes = true;
                treeView1.CollapseAll(); 


                while (intRowID < Temp_Dt.Rows.Count)
                {

                    Menu_Head_ID = Temp_Dt.Rows[intRowID]["Menu_Head_ID"].ToString();
                    strMenuHead = Temp_Dt.Rows[intRowID]["Menu_Head"].ToString().Replace("&","");
                    Menu_Head_ID = Temp_Dt.Rows[intRowID]["Menu_Head_ID"].ToString();

                    treeView1.Nodes.Add(Menu_Head_ID, strMenuHead, Menu_Head_ID);
                    intRowID++;
                }

                LoadChildNodes();
                LoadChildNodes1();
                LoadChildNodes2();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void LoadChildNodes()
        {
            //int baseParent = 0;                 // Find the lowest root category parent value
            //TreeNode node;

            String strOld_Head = String.Empty;

            String strOld_Menu_Head_ID = String.Empty;
            String strNew_Menu_Head_ID = String.Empty;
            String strMenu_ID = String.Empty;
            String strMenuName = String.Empty;
            String strMenuText = String.Empty;
            int intNodeId = 0;

            try
            {
                while (intNodeId < treeView1.Nodes.Count)
                {
                    //treeView1.Nodes.Add(Menu_Head_ID, strMenuName);
                    //strOld_Head = treeView1.Node
                    strOld_Menu_Head_ID = treeView1.Nodes[intNodeId].ImageKey.ToString();
                    strOld_Head = treeView1.Nodes[intNodeId].Text.ToString();
                    int intRowID = 0;

                    DataTable Temp_Dt = new DataTable();
                   // Str = "Select Menu_ID, Menu_Head_ID, Menu_Name , Menu_Text from Menu_Master Where Menu_Head_ID = " + strOld_Menu_Head_ID;

                    Str = "Select Menu_CName Menu_ID, under_Menu_CName Menu_Head_ID, Menu_CName Menu_Name, Menu_Name Menu_Text from Projects.dbo.Projects_Menu_Master_New Where under_Menu_CName = '" + strOld_Menu_Head_ID + "'";

                    MyBase.Load_Data(Str, ref Temp_Dt);
                       
                    while (intRowID < Temp_Dt.Rows.Count)
                    {
                        //TreeNode subNode = new TreeNode;
                        strMenu_ID = Temp_Dt.Rows[intRowID]["Menu_ID"].ToString();
                        strNew_Menu_Head_ID = Temp_Dt.Rows[intRowID]["Menu_Head_ID"].ToString();
                        strMenuName = Temp_Dt.Rows[intRowID]["Menu_Name"].ToString();
                        strMenuText = Temp_Dt.Rows[intRowID]["Menu_Text"].ToString().Replace("&","") ;
                        treeView1.Nodes[intNodeId].Nodes.Add(strMenu_ID, strMenuText, strMenu_ID);

                        intRowID++;
                    }

                   intNodeId++;
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message) ;
            }


        }

        void LoadChildNodes1()
        {
            //int baseParent = 0;                 // Find the lowest root category parent value
            //TreeNode node;

            String strOld_Head = String.Empty;

            String strOld_Menu_Head_ID = String.Empty;
            String strNew_Menu_Head_ID = String.Empty;
            String strMenu_ID = String.Empty;
            String strMenuName = String.Empty;
            String strMenuText = String.Empty;
            
            int intParentId = 0;

            try
            {
                while (intParentId < treeView1.Nodes.Count)
                {
                    int intNodeId = 0;

                    while (intNodeId < treeView1.Nodes[intParentId].Nodes.Count)
                    {
                        //treeView1.Nodes.Add(Menu_Head_ID, strMenuName);
                        //strOld_Head = treeView1.Node
                        strOld_Menu_Head_ID = treeView1.Nodes[intParentId].Nodes[intNodeId].ImageKey.ToString();
                        strOld_Head = treeView1.Nodes[intParentId].Nodes[intNodeId].Text.ToString();
                        int intRowID = 0;

                        DataTable Temp_Dt = new DataTable();
                        // Str = "Select Menu_ID, Menu_Head_ID, Menu_Name , Menu_Text from Menu_Master Where Menu_Head_ID = " + strOld_Menu_Head_ID;

                        Str = "Select Menu_CName Menu_ID, under_Menu_CName Menu_Head_ID, Menu_CName Menu_Name, Menu_Name Menu_Text from Projects.dbo.Projects_Menu_Master_New Where under_Menu_CName = '" + strOld_Menu_Head_ID + "'";

                        MyBase.Load_Data(Str, ref Temp_Dt);

                        intRowID = 0;

                        while (intRowID < Temp_Dt.Rows.Count)
                        {
                            int NestedRowID = 0;

                            //TreeNode subNode = new TreeNode;
                            strMenu_ID = Temp_Dt.Rows[intRowID]["Menu_ID"].ToString();
                            strNew_Menu_Head_ID = Temp_Dt.Rows[intRowID]["Menu_Head_ID"].ToString();
                            strMenuName = Temp_Dt.Rows[intRowID]["Menu_Name"].ToString();
                            strMenuText = Temp_Dt.Rows[intRowID]["Menu_Text"].ToString().Replace("&", "");
                            treeView1.Nodes[intParentId].Nodes[intNodeId].Nodes.Add(strMenu_ID, strMenuText, strMenu_ID);

                            DataTable Temp_Dt1 = new DataTable();
                            // Str = "Select Menu_ID, Menu_Head_ID, Menu_Name , Menu_Text from Menu_Master Where Menu_Head_ID = " + strOld_Menu_Head_ID;

                            Str = "Select Menu_CName Menu_ID, under_Menu_CName Menu_Head_ID, Menu_CName Menu_Name, Menu_Name Menu_Text from Projects.dbo.Projects_Menu_Master_New Where under_Menu_CName = '" + strMenu_ID + "'";

                            MyBase.Load_Data(Str, ref Temp_Dt1);

                            while (NestedRowID < Temp_Dt1.Rows.Count)
                            {
                                strMenu_ID = Temp_Dt1.Rows[NestedRowID]["Menu_ID"].ToString();
                                strNew_Menu_Head_ID = Temp_Dt1.Rows[NestedRowID]["Menu_Head_ID"].ToString();
                                strMenuName = Temp_Dt1.Rows[NestedRowID]["Menu_Name"].ToString();
                                strMenuText = Temp_Dt1.Rows[NestedRowID]["Menu_Text"].ToString().Replace("&", "");
                                treeView1.Nodes[intParentId].Nodes[intNodeId].Nodes[intRowID].Nodes.Add(strMenu_ID, strMenuText, strMenu_ID);

                                NestedRowID++;
                            }

                            intRowID++;
                        }
                        intNodeId++;
                    }
                    intParentId++;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void LoadChildNodes2()
        {
            //int baseParent = 0;                 // Find the lowest root category parent value
            //TreeNode node;

            String strOld_Head = String.Empty;

            String strOld_Menu_Head_ID = String.Empty;
            String strNew_Menu_Head_ID = String.Empty;
            String strMenu_ID = String.Empty;
            String strMenuName = String.Empty;
            String strMenuText = String.Empty;

            int intParentId = 0;

            try
            {
                while (intParentId < treeView1.Nodes.Count)
                {
                    int intNodeId = 0;

                    while (intNodeId < treeView1.Nodes[intParentId].Nodes.Count)
                    {
                        //treeView1.Nodes.Add(Menu_Head_ID, strMenuName);
                        //strOld_Head = treeView1.Node
                        strOld_Menu_Head_ID = treeView1.Nodes[intParentId].Nodes[intNodeId].ImageKey.ToString();
                        strOld_Head = treeView1.Nodes[intParentId].Nodes[intNodeId].Text.ToString();
                        int intRowID = 0;

                        DataTable Temp_Dt = new DataTable();
                        // Str = "Select Menu_ID, Menu_Head_ID, Menu_Name , Menu_Text from Menu_Master Where Menu_Head_ID = " + strOld_Menu_Head_ID;

                        Str = "Select Menu_CName Menu_ID, under_Menu_CName Menu_Head_ID, Menu_CName Menu_Name, Menu_Name Menu_Text from Projects.dbo.Projects_Menu_Master_New Where under_Menu_CName = '" + strOld_Menu_Head_ID + "'";

                        MyBase.Load_Data(Str, ref Temp_Dt);

                        intRowID = 0;

                        while (intRowID < Temp_Dt.Rows.Count)
                        {
                            int NestedRowID = 0;

                            //TreeNode subNode = new TreeNode;
                           
                            DataTable Temp_Dt1 = new DataTable();
                            // Str = "Select Menu_ID, Menu_Head_ID, Menu_Name , Menu_Text from Menu_Master Where Menu_Head_ID = " + strOld_Menu_Head_ID;

                            Str = "Select Menu_CName Menu_ID, under_Menu_CName Menu_Head_ID, Menu_CName Menu_Name, Menu_Name Menu_Text from Projects.dbo.Projects_Menu_Master_New Where under_Menu_CName = '" + strMenu_ID + "'";

                            MyBase.Load_Data(Str, ref Temp_Dt1);

                            while (NestedRowID < Temp_Dt1.Rows.Count)
                            {
                                strMenu_ID = Temp_Dt1.Rows[NestedRowID]["Menu_ID"].ToString();
                                strNew_Menu_Head_ID = Temp_Dt1.Rows[NestedRowID]["Menu_Head_ID"].ToString();
                                strMenuName = Temp_Dt1.Rows[NestedRowID]["Menu_Name"].ToString();
                                strMenuText = Temp_Dt1.Rows[NestedRowID]["Menu_Text"].ToString().Replace("&", "");
                                treeView1.Nodes[intParentId].Nodes[intNodeId].Nodes[intRowID].Nodes.Add(strMenu_ID, strMenuText, strMenu_ID);

                                NestedRowID++;
                            }

                            intRowID++;
                        }
                        intNodeId++;
                    }
                    intParentId++;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Projects_Permission_Master_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Projects_Permission_Master_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "Chk_Report")
                    {
                        Txt_Description.Focus();
                        SendKeys.Send("{End}");
                    }
                    else if (this.ActiveControl.Name == "Txt_Description")
                    {
                        MyParent.Load_SaveEntry();
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    //Treeview1_Clear(); 
                    //CheckBox_Clear();
                    MyBase.ActiveForm_Close(this, MyParent);
                }
                else if (e.KeyCode == Keys.Down)
                {

                    if (this.ActiveControl.Name == "Txt_UserName")
                    {
                        Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select PRojects_User_Master", "Select User_Name, User_Code from Projects.dbo.Projects_User_Master", String.Empty, 150);
                        if (Dr != null)
                        {
                            Txt_UserName.Text = Dr["User_Name"].ToString();
                            Txt_UserName.Tag = Dr["User_Code"].ToString();
                            Fill_Datas(Convert.ToInt32(Txt_UserName.Tag.ToString()));
                        }
                    }
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
                MyBase.Enable_Controls(this, true);
                MyBase.Clear(this);
                Current_ID = 0;
                LoadBaseNodes();
                Txt_UserName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        Boolean chkValidation()
        {

            try
            {
                if (Txt_UserName.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid User ...!");
                    Txt_UserName.Focus();
                    return false;
                }

                //if (Txt_UserName.Text.Trim() == String.Empty)
                //{
                //    MessageBox.Show("UserName Is Must ...!");
                //    Txt_UserName.Focus();
                //    return false;
                //}


                //if (Txt_Password.Text.Trim() == String.Empty)
                //{
                //    MessageBox.Show("Password Is Must...!");
                //    Txt_Password.Focus();
                //    return false;
                //}

                //if (Txt_Confirm_Password.Text.Trim() == String.Empty)
                //{
                //    MessageBox.Show("Confirm Password Is Must ...!");
                //    Txt_Confirm_Password.Focus();
                //    return false;
                //}

                //if (Txt_Password.Text.Trim() != Txt_Confirm_Password.Text.Trim())
                //{
                //    MessageBox.Show("Password , Confirm Password Not Same...!");
                //    Txt_Password.Focus();
                //    return false;
                //}


                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public void Entry_Save()
        {
            int intRowID = 0;

            String strMenu_Head_Id = String.Empty;
            String strMenu_Name = String.Empty;  
            String strRights = String.Empty;
            String Rights = String.Empty;

            String strNodeName = String.Empty; 
            int intPermission_ID = 0;
            int intUser_ID = 0;
            String[] Queries = new String[500];


            try
            {

                UpdateRights();

                if (chkValidation() == false)
                {
                    MyParent.Save_Error = true;
                    return;
                }

                /// Connection and Command Details;

                //MyBase.Cn_Open();
                //MyBase.SQLTrans = MyBase.Cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                //MyBase.SQLCmd = MyBase.Cn.CreateCommand();
                //MyBase.SQLCmd.Connection = MyBase.Cn;
                //MyBase.SQLCmd.Transaction = MyBase.SQLTrans;
                //MyBase.SQLCmd.CommandType = System.Data.CommandType.StoredProcedure;
                //MyBase.SQLCmd.CommandText = "SP_Insert_Floor_Permission_Master";


                if (Permission_Dt.Rows.Count > 0)
                {
                    intRowID = 0;

                    while (intRowID < Permission_Dt.Rows.Count)
                    {
                        intPermission_ID = 0;

                        if (Permission_Dt.Rows[intRowID]["Permission_ID"].ToString() != "")
                        {

                            intPermission_ID = Convert.ToInt32(Permission_Dt.Rows[intRowID]["Permission_ID"].ToString());
                        }


                        // strMenu_Head_Id = Permission_Dt.Rows[intRowID]["Menu_Head_ID"].ToString();
                        strMenu_Name = Permission_Dt.Rows[intRowID]["Menu_Name"].ToString();
                        intUser_ID = Convert.ToInt32(Permission_Dt.Rows[intRowID]["User_ID"].ToString());
                        strRights = Permission_Dt.Rows[intRowID]["Rights"].ToString();

                        if (strRights != "")
                        {
                            Queries[intRowID] = "insert into Projects.dbo.Projects_Permission_Master values (" + intUser_ID + ", '" + strMenu_Name + "', '" + strRights + "', " + MyParent.UserCode + ", " + MyParent.SysCode + ", " + MyParent.Today + ", " + MyParent.UserCode + ", " + MyParent.SysCode + ", " + MyParent.Today + ", " + MyParent.CompCode + ", '" + MyParent.YearCode + "')";

                            //MyBase.SQLCmd.Parameters.Clear();
                            //MyBase.SQLCmd.Parameters.Add("@Permission_ID", System.Data.SqlDbType.Int).Value = intPermission_ID;
                            //MyBase.SQLCmd.Parameters.Add("@User_ID", System.Data.SqlDbType.Int).Value = intUser_ID;
                            //MyBase.SQLCmd.Parameters.Add("@Menu_Name", System.Data.SqlDbType.VarChar).Value = strMenu_Name;
                            //MyBase.SQLCmd.Parameters.Add("@Rights", System.Data.SqlDbType.VarChar).Value = strRights;
                            //MyBase.SQLCmd.Parameters.Add("@Description", System.Data.SqlDbType.VarChar).Value = Txt_Description.Text.Trim();
                            //MyBase.SQLCmd.Parameters.Add("@Blocked", System.Data.SqlDbType.VarChar).Value = Convert.ToInt32(Convert.ToBoolean(Chk_Cancel.Checked));
                            //MyBase.SQLCmd.Parameters.Add("@Company_Code", System.Data.SqlDbType.VarChar).Value = MyParent.CompCode;
                            //MyBase.SQLCmd.Parameters.Add("@Year_Code", System.Data.SqlDbType.VarChar).Value = MyParent.YearCode;
                            //Current_ID = Convert.ToInt32(MyBase.SQLCmd.ExecuteScalar());
                        }

                        intRowID++;
                    }

               }

                MyBase.Run(Queries, "Delete from Projects.dbo.Projects_Permission_Master where User_ID = " + intUser_ID);
                ///// -------------------------

                // User Entry Log
                //MyBase.SQLCmd.CommandType = System.Data.CommandType.Text;
                //MyBase.SQLCmd.CommandText = " Insert into User_Entry_Log Values (" + MyParent.UserCode + ", '" + Environment.MachineName + "', '" + MyParent.Active_Child_Form_Name() + "', " + Current_ID + ", '" + String.Format("{0:dd-MMM-yyyy} {0:T}", DateTime.Now) + "', '" + MyParent.Get_Entry_Mode() + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "')";
                //MyBase.SQLCmd.ExecuteNonQuery();
                ///// -------------------------

                MessageBox.Show("Saved ...!");
                Treeview1_Clear();
                CheckBox_Clear(); 

                MyParent.Save_Error = false;
            }
            catch (Exception ex)
            {
                if (MyBase.SQLTrans != null)
                {
                    MyBase.SQLTrans.Rollback();
                }
                if (ex.Message.ToUpper().Contains("CONSTRAINT"))
                {
                    MessageBox.Show("User Name already Exists ...!");
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
                MyParent.Save_Error = true;
            }
            finally
            {
                MyBase.Cn_Close();
            }
        }

        //void Fill_Datas(Int32 Identity)
        //{
        //    DataTable TempDt = new DataTable();
        //    string sQuery = String.Empty;
        //    String strMenu_Head_ID = String.Empty;
        //    String strMenu_ID =  String.Empty;
        //    String strExParentID = String.Empty;
        //    String strExNodeID = String.Empty;
        //    String strBreak = String.Empty;
        //    String strTrParentID = String.Empty;
        //    String strTrNodeID = String.Empty;

        //    Current_ID = Identity;
        //   // Permission_Dt = null;

           

        //    Permission_Dt.Rows.Clear();
        //    Permission_Dt.Columns.Clear();  
        //    Permission_Dt.Clear();
            
           
        //    try
        //    {
        //        Str = " Select Pm.Permission_ID,Pm.User_ID,";
        //        Str += " Pm.Menu_Name,Pm.Rights";
        //        //Str += " Pm.[Description],Pm.Blocked, Pm.Company_Code";    
        //        Str += " From Floor_Permission_Master  AS Pm";
        //        Str += " Left Join Floor_Menu_Master_New As Mn On Pm.Menu_Name = Mn.Menu_CName ";
        //        Str += " Left Join Floor_User_Master As Um On Pm.[User_ID]  = Um.[user_Code]";
        //        Str += " Where Pm.[User_ID] = '" + Current_ID + "'";
        //        Str += " And Pm.Company_Code = '" + MyParent.CompCode + "'";
        //       // Str += " Order By Menu_Name";

        //        MyBase.Load_Data(Str, ref Permission_Dt);
                
              
        //        int intRowID = 0;

        //        while (intRowID < Permission_Dt.Rows.Count)
        //        {
        //            //strMenu_Head_ID = Permission_Dt.Rows[intRowID]["Menu_Head_ID"].ToString();
        //            strMenu_ID = Permission_Dt.Rows[intRowID]["Menu_Name"].ToString();

        //            int intParentID = 0;

        //            while(intParentID < treeView1.Nodes.Count)
        //            {
        //                int intNodeID = 0;

        //                while (intNodeID < treeView1.Nodes[intParentID].Nodes.Count)
        //                {
        //                    strBreak = String.Empty;  

        //                    strTrNodeID = treeView1.Nodes[intParentID].Nodes[intNodeID].Name.ToString();
                           

        //                    if (strMenu_ID == strTrNodeID)
        //                    {
        //                        treeView1.Nodes[intParentID].Nodes[intNodeID].Checked = true;
        //                        strBreak = "1";

        //                        break; 
        //                        //treeView1.Nodes[intNodeID].Checked = true;
        //                    }

                           
        //                    intNodeID++;
        //                }

        //                if (strBreak == "1")
        //                {
        //                    break;
        //                }
        //                intParentID++;

        //            }


        //            intRowID++;
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}


        void Fill_Datas(Int32 Identity)
        {
            DataTable TempDt = new DataTable();
            string sQuery = String.Empty;
            String strMenu_Head_ID = String.Empty;
            String strMenu_ID = String.Empty;
            String strExParentID = String.Empty;
            String strExNodeID = String.Empty;
            String strBreak = String.Empty;
            String strBreak1 = String.Empty;
            String strTrParentID = String.Empty;
            String strTrNodeID = String.Empty;

            Current_ID = Identity;
            // Permission_Dt = null;



            Permission_Dt.Rows.Clear();
            Permission_Dt.Columns.Clear();
            Permission_Dt.Clear();

            try
            {
                Str = " Select Pm.Permission_ID,Pm.User_ID,";
                Str += " Pm.Menu_Name,Pm.Rights";
                //Str += " Pm.[Description],Pm.Blocked, Pm.Company_Code";    
                Str += " From Projects.dbo.Projects_Permission_Master  AS Pm";
                Str += " Left Join Projects.dbo.PRojects_Menu_Master_New As Mn On Pm.Menu_Name = Mn.Menu_CName ";
                Str += " Left Join Projects.dbo.PRojects_User_Master As Um On Pm.[User_ID]  = Um.[user_Code]";
                Str += " Where Pm.[User_ID] = '" + Current_ID + "'";
                Str += " And Pm.Company_Code = '" + MyParent.CompCode + "'";
                // Str += " Order By Menu_Name";

                MyBase.Load_Data(Str, ref Permission_Dt);


                int intRowID = 0;

                while (intRowID < Permission_Dt.Rows.Count)
                {
                    //strMenu_Head_ID = Permission_Dt.Rows[intRowID]["Menu_Head_ID"].ToString();
                    strMenu_ID = Permission_Dt.Rows[intRowID]["Menu_Name"].ToString();

                    int intParentID = 0;

                    while (intParentID < treeView1.Nodes.Count)
                    {
                        int intNodeID = 0;

                        while (intNodeID < treeView1.Nodes[intParentID].Nodes.Count)
                        {
                            strBreak = String.Empty;

                            strTrNodeID = treeView1.Nodes[intParentID].Nodes[intNodeID].Name.ToString();


                            if (strMenu_ID == strTrNodeID)
                            {
                                treeView1.Nodes[intParentID].Nodes[intNodeID].Checked = true;
                                strBreak = "1";
                                break;
                            }
                            else
                            {

                                if (treeView1.Nodes[intParentID].Nodes[intNodeID].Nodes.Count > 0)
                                {
                                    int intNodeID1 = 0;

                                    String strTrNodeID1 = String.Empty;

                                    while (intNodeID1 < treeView1.Nodes[intParentID].Nodes[intNodeID].Nodes.Count)
                                    {
                                        strBreak = String.Empty;

                                        strTrNodeID1 = treeView1.Nodes[intParentID].Nodes[intNodeID].Nodes[intNodeID1].Name.ToString();


                                        if (strMenu_ID == strTrNodeID1)
                                        {
                                            treeView1.Nodes[intParentID].Nodes[intNodeID].Nodes[intNodeID1].Checked = true;
                                            strBreak = "1";
                                            break;


                                            //treeView1.Nodes[intNodeID].Checked = true;
                                        }
                                        else
                                        {
                                            if (treeView1.Nodes[intParentID].Nodes[intNodeID].Nodes[intNodeID1].Nodes.Count > 0)
                                            {
                                                int intNodeID2 = 0;

                                                String strTrNodeID2 = String.Empty;

                                                while (intNodeID2 < treeView1.Nodes[intParentID].Nodes[intNodeID].Nodes[intNodeID1].Nodes.Count)
                                                {
                                                    strBreak1 = String.Empty;

                                                    strTrNodeID2 = treeView1.Nodes[intParentID].Nodes[intNodeID].Nodes[intNodeID1].Nodes[intNodeID2].Name.ToString();


                                                    if (strMenu_ID == strTrNodeID2)
                                                    {
                                                        treeView1.Nodes[intParentID].Nodes[intNodeID].Nodes[intNodeID1].Nodes[intNodeID2].Checked = true;
                                                        strBreak = "1";
                                                        break;
                                                    }
                                                    intNodeID2++;
                                                }
                                            }
                                        }
                                 

                                        intNodeID1++;
                                    }
                                }
                            }
                            //treeView1.Nodes[intNodeID].Checked = true;



                            intNodeID++;
                        }

                        if (strBreak == "1")
                        {
                            break;
                        }
                        intParentID++;

                    }


                    intRowID++;
                }



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
                MessageBox.Show("This Entry doesn't Have Edit Option ...!", "Vaahini");
                MyParent.Load_NewEntry();
                return;
                //MyBase.Enable_Controls(this, true);
                //MyBase.Clear(this);
                //Dr = Tool.Selection_Tool(this, 200, 200, SelectionTool_Class.ViewType.NormalView, "User - Edit", "Select UserName, User_ID, Description, Blocked from Floor_User_Master", String.Empty, 150);
                //if (Dr != null)
                //{
                //    Fill_Datas(Convert.ToInt32(Dr["User_ID"]));
                //    Txt_UserName.Focus();
                //}
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
                MessageBox.Show("This Entry doesn't Have Delete Option ...!", "Vaahini");
                MyParent.Load_NewEntry();
                return;
                //MyBase.Enable_Controls(this, false);
                //MyBase.Clear(this);
                //Dr = Tool.Selection_Tool(this, 200, 200, SelectionTool_Class.ViewType.NormalView, "User - Delete", "Select UserName, User_ID, Description, Blocked from Floor_User_Master", String.Empty, 150);
                //if (Dr != null)
                //{
                //    Txt_UserName.Text = Dr["UserName"].ToString(); 
                //    Txt_UserName.Tag = Dr["User_ID"].ToString() ; 

                //    Fill_Datas(Convert.ToInt32(Dr["User_ID"]));

                //    MyParent.Load_DeleteConfirmEntry();
                //}
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
                if (Current_ID > 0)
                {
                    //MyBase.Cn_Open();
                    //MyBase.SQLTrans = MyBase.Cn.BeginTransaction(IsolationLevel.ReadCommitted);

                    //MyBase.SQLCmd = MyBase.Cn.CreateCommand();
                    //MyBase.SQLCmd.Connection = MyBase.Cn;
                    //MyBase.SQLCmd.Transaction = MyBase.SQLTrans;
                    //MyBase.SQLCmd.CommandType = CommandType.StoredProcedure;
                    //MyBase.SQLCmd.CommandText = "SP_Delete_Floor_Permission_Master";

                    //MyBase.SQLCmd.Parameters.Clear();
                    //MyBase.SQLCmd.Parameters.Add("@User_ID", SqlDbType.Int).Value = Current_ID;
                    //MyBase.SQLCmd.ExecuteNonQuery();

                    //// User Entry Log
                    //MyBase.SQLCmd.CommandType = System.Data.CommandType.Text;
                    //MyBase.SQLCmd.CommandText = " Insert into User_Entry_Log Values (" + MyParent.UserCode + ", '" + Environment.MachineName + "', '" + MyParent.Active_Child_Form_Name() + "', " + Current_ID + ", '" + String.Format("{0:dd-MMM-yyyy} {0:T}", DateTime.Now) + "', '" + MyParent.Get_Entry_Mode() + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "')";
                    //MyBase.SQLCmd.ExecuteNonQuery();
                    /////// -------------------------

                    //MyBase.SQLTrans.Commit();
                    MyBase.Execute("Delete from Projects.dbo.PRojects_Permission_Master Where [User_ID] = " + Current_ID);
                    MessageBox.Show("Deleted ...!");
                    MyParent.Load_DeleteEntry();
                }
                else
                {
                    MessageBox.Show("Invalid User ..!");
                    return;
                }
            }
            catch (Exception ex)
            {
                if (MyBase.SQLTrans != null)
                {
                    MyBase.SQLTrans.Rollback();
                }
                MessageBox.Show(ex.Message);
            }
            finally
            {
                MyBase.Cn_Close();
            }
        }


        public void Entry_View()
        {
            try
            {
                MyBase.Enable_Controls(this, false);
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 200, 200, SelectionTool_Class.ViewType.NormalView, "User - View", "Select UserName, User_ID, Description, Blocked from Projects.dbo.PRojects_User_Master", String.Empty, 150);
                if (Dr != null)
                {
                    Fill_Datas(Convert.ToInt32(Dr["User_ID"]));
                }
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
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Projects_Permission_Master_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "Txt_UserName")
                {
                    MyBase.Valid_Null(Txt_UserName, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Txt_UserName_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Down)
            //{


            //    Dr = Tool.Selection_Tool(this, 150, 150, SelectionTool_Class.ViewType.NormalView, "Select Floor_User_Master", "Select UserName, [User_ID] from Socks_User_Master", String.Empty, 150);
            //    if (Dr != null)
            //    {
            //        Txt_UserName.Text = Dr["UserName"].ToString();
            //        Txt_UserName.Tag = Dr["User_ID"].ToString();
            //    }

            //}
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int intRowID = 0;

           // String strMenu_Head_Id = String.Empty;
            String strMenu_Name = String.Empty;
            String StrUser_ID = String.Empty;
            String strRights = String.Empty;
            String Rights = String.Empty;
            String StrNodeName = String.Empty;  
            try
            {

                CheckBox_Clear();

                StrNodeName = treeView1.SelectedNode.Name.ToString();

         

                if (Permission_Dt.Rows.Count > 0)
                {
                    intRowID = 0;

                    while (intRowID < Permission_Dt.Rows.Count)
                    {
                       
                       // strMenu_Head_Id = Permission_Dt.Rows[intRowID]["Menu_Head_ID"].ToString();
                        strMenu_Name = Permission_Dt.Rows[intRowID]["Menu_Name"].ToString();
                        StrUser_ID = Permission_Dt.Rows[intRowID]["User_ID"].ToString();
                        strRights = Permission_Dt.Rows[intRowID]["Rights"].ToString();

                        if (StrNodeName == strMenu_Name)
                        {
                            break;
                        }
                        else
                        {
                            strRights = "";
                        }

                        intRowID++;

                    }

                    if (strRights != "")
                    {

                        string[] words = strRights.Split(',');

                        foreach (string word in words)
                        {
                            Rights = (word);

                            if (Rights == "A")
                            {
                                Chk_Add.Checked = true;
                            }

                            if (Rights == "E")
                            {
                                Chk_Edit.Checked = true;
                            }

                            if (Rights == "D")
                            {
                                Chk_Delete.Checked = true;
                            }

                            if (Rights == "V")
                            {
                                Chk_View.Checked = true;
                            }

                            if (Rights == "P")
                            {
                                Chk_Print.Checked = true;
                            }

                            if (Rights == "R")
                            {
                                Chk_Report.Checked = true;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }


        }

        public String CheckSelRights()
        {
            String SelRights = String.Empty;
            try
            {
                SelRights = "";
                if (Chk_Add.Checked == true)
                {
                    if (SelRights != "")
                    {
                        SelRights = SelRights + "," + "A";
                    }
                    else
                    {
                        SelRights = "A";
                    }
                }
                if (Chk_Edit.Checked == true)
                {
                    if (SelRights != "")
                    {
                        SelRights = SelRights + "," + "E";
                    }
                    else
                    {
                        SelRights = "E";
                    }
                }
                if (Chk_Delete.Checked == true)
                {
                    if (SelRights != "")
                    {
                        SelRights = SelRights + "," + "D";
                    }
                    else
                    {
                        SelRights = "D";
                    }
                  
                }
               if (Chk_View.Checked == true)
                {
                    if (SelRights != "")
                    {
                        SelRights = SelRights + "," + "V";
                    }
                    else
                    {
                        SelRights = "V";
                    }
                }
                if (Chk_Print.Checked == true)
                {
                    if (SelRights != "")
                    {
                        SelRights = SelRights + "," + "P";
                    }
                    else
                    {
                        SelRights = "P";
                    }
                }
                if (Chk_Report.Checked == true)
                {
                    if (SelRights != "")
                    {
                        SelRights = SelRights + "," + "R";
                    }
                    else
                    {
                        SelRights = "R";
                    }

                }
               
                return SelRights;
            }
            catch (Exception ex)
            {
                return SelRights;
            }
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            String StrMenuType = String.Empty;
            try
            {
                if (treeView1.SelectedNode != null)
                {
                    //if (Txt_UserName.Tag.ToString() == "")
                    //{
                    //    MessageBox.Show("Select UserName", this.Name);
                    //    return;
                    //}
                    UpdateRights();
                }
                CheckBox_Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void treeView1_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
          

            try
            {
                if (Txt_UserName.Tag.ToString() =="")
                {
                    MessageBox.Show("Select UserName", this.Name);
                    return; 
                }


                if (treeView1.SelectedNode != null)
                {
                    UpdateRights();
                }
            }
          
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Chk_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            String StrParentText = String.Empty;

             try
            {
            CheckBox_Clear();

            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Parent != null)
                {
                    StrParentText = treeView1.SelectedNode.Parent.Text.ToString();
                }
            }


            if (Chk_SelectAll.Checked == true)
            {
                Chk_Add.Enabled = true;
                Chk_Edit.Enabled = true;
                Chk_Delete.Enabled = true;
                Chk_Print.Enabled = true;
                Chk_View.Enabled = true;

                Chk_Add.Checked = true;
                Chk_Edit.Checked = true;
                Chk_Delete.Checked = true;
                Chk_Print.Checked = true;
                Chk_View.Checked = true;

                if (StrParentText.Contains("REPORTS"))
                {
                    Chk_Report.Checked = true;
                    Chk_Report.Enabled = true;

                    Chk_Add.Enabled = false;
                    Chk_Edit.Enabled = false;
                    Chk_Delete.Enabled = false;
                    Chk_Print.Enabled = false;
                    Chk_View.Enabled = false;

                    Chk_Add.Checked = false;
                    Chk_Edit.Checked = false;
                    Chk_Delete.Checked = false;
                    Chk_Print.Checked = false;
                    Chk_View.Checked = false;


                }
                else
                {
                    Chk_Report.Checked = false;
                    Chk_Report.Enabled = false;
                }
            }
            else
            {
                Chk_Add.Checked = false;
                Chk_Edit.Checked = false;
                Chk_Delete.Checked = false;
                Chk_Print.Checked = false;
                Chk_View.Checked = false;

                if (StrParentText.Contains("Reports"))
                {
                    Chk_Report.Checked = false;
                    Chk_Report.Enabled = false;
                }
         
            }
        }

        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        }
        void UpdateRights()
        {
            String strParentName = String.Empty;
            String strParentText = String.Empty;
            String strParentTag = String.Empty;

            String strNodeName = String.Empty;
            String strNodeText = String.Empty;
            String strNodeTag = String.Empty;

            String strRights = String.Empty;
            String Rights = String.Empty;
            String strMenu_Name = String.Empty;
            String strUser_Id = String.Empty;
            String strUpdate = String.Empty;
            //String strMenu_Head_Id = String.Empty;
            String strUser_ID = String.Empty;
 


            try
            {
                if (treeView1.SelectedNode == null)
                {
                    return;
                }
                else
                {
                    strNodeName = treeView1.SelectedNode.Name.ToString();

                    strNodeText = treeView1.SelectedNode.Text.ToString();

                    strNodeTag = treeView1.SelectedNode.ImageKey.ToString();


                }

               


                
                if (treeView1.SelectedNode.Parent != null)
                {
                    //treeView1.S .Checked = true;     

                    strParentName = treeView1.SelectedNode.Parent.Name.ToString();

                    strParentText = treeView1.SelectedNode.Parent.Text.ToString();

                    //StrParentTag = treeView1.SelectedNode.Parent.Tag.ToString(); ;
                }



                if (strParentName != "" && strNodeName != "")
                {
                    strRights = CheckSelRights();

                        int intRowID = 0;
                        strUpdate = String.Empty;

                        while (intRowID < Permission_Dt.Rows.Count)
                        {
                            strMenu_Name = String.Empty;

                            String  strPermission_ID = String.Empty;

                            strPermission_ID = Permission_Dt.Rows[intRowID]["Permission_ID"].ToString();

                            strMenu_Name = Permission_Dt.Rows[intRowID]["Menu_Name"].ToString();
                            strUser_ID = Permission_Dt.Rows[intRowID]["User_ID"].ToString();
                           
                            DataRow Row = Permission_Dt.Rows[intRowID];


                            if (strPermission_ID != "0" && strPermission_ID != "")
                            {
                                if (strNodeName == strMenu_Name)
                                {



                                    Row["User_ID"] = Txt_UserName.Tag.ToString();
                                    //   Row["Menu_Head_ID"] = strParentName;
                                    Row["Menu_Name"] = strNodeTag;
                                    Row["Rights"] = strRights;

                                    Permission_Dt.Rows[intRowID].AcceptChanges();



                                    strUpdate = "1";

                                    return;
                                }
                            }

                            intRowID++;

                        }


                        if (strUpdate == "")
                        {
                            if (strRights != "")
                            {
                                DataRow newRow = Permission_Dt.NewRow();
                                newRow["User_ID"] = Txt_UserName.Tag.ToString();
                                //newRow["Menu_Head_ID"] = strParentName;
                                newRow["Menu_Name"] = strNodeTag;
                                newRow["Rights"] = strRights;
                                Permission_Dt.Rows.Add(newRow);
                                Permission_Dt.AcceptChanges();
                            }
                        }

                        CheckBox_Clear();

                        Chk_SelectAll.Checked = false;

                    



                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       

        void CheckBox_Clear()
        {
            String StrParentText = String.Empty;
            try
            {
                if (treeView1.SelectedNode != null)
                {
                    if (treeView1.SelectedNode.Parent != null)
                    {
                        StrParentText = treeView1.SelectedNode.Parent.Text.ToString();
                    }
                }

              
            
                Chk_Add.Checked = false;
                Chk_Edit.Checked = false;
                Chk_Delete.Checked = false;
                Chk_Print.Checked = false;
                Chk_View.Checked = false;
                Chk_Report.Checked = false;

                if (StrParentText.Contains("REPORTS"))
                {

                    Chk_Add.Enabled = false;
                    Chk_Edit.Enabled = false;
                    Chk_Delete.Enabled = false;
                    Chk_Print.Enabled = false;
                    Chk_View.Enabled = false;
                    Chk_Report.Enabled = false;
                    Chk_Report.Enabled = true;
                }
                else
                {
                    Chk_Add.Enabled = true;
                    Chk_Edit.Enabled = true;
                    Chk_Delete.Enabled = true;
                    Chk_Print.Enabled = true;
                    Chk_View.Enabled = true;
                    Chk_Report.Enabled = true;
                    Chk_Report.Enabled = false;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

     

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {

            String StrParentName = String.Empty;
            String StrParentText = String.Empty;
            String StrParentTag = String.Empty;
            String StrNodeName = String.Empty;
            String StrNodeText = String.Empty;
            String StrNodeTag = String.Empty;

            try
            {

                if (e.Action != TreeViewAction.Unknown)
                {
                    if (e.Node.Nodes.Count > 0)
                    {
                        /* Calls the CheckAllChildNodes method, passing in the current 
                        Checked value of the TreeNode whose checked state changed. */
                        this.CheckAllChildNodes(e.Node, e.Node.Checked);
                    }
                }

                //if (e.Node.Checked  == true)
                //{
                //  //  this.treeView1.Select();
                //    treeView1.SelectedNode = e.Node;
                //}

                if (e.Node.Parent != null)
                {
                    e.Node.Parent.Checked = true;
                }
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            try
            {
                foreach (TreeNode node in treeNode.Nodes)
                {
                    node.Checked = nodeChecked;
                    if (node.Nodes.Count > 0)
                    {
                        // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                        this.CheckAllChildNodes(node, nodeChecked);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        void CheckSelectMsg()
        {
            try
            {

                if (treeView1.SelectedNode != null)
                {

                    if (treeView1.SelectedNode.Parent == null)
                    {
                        MessageBox.Show("Check and Select Menu", this.Name);
                        return;
                    }
                }
                if (treeView1.SelectedNode == null)
                {
                    MessageBox.Show("Check and Select Menu", this.Name);
                    return;
                }



                if (treeView1.SelectedNode.Checked == false)
                {
                    CheckBox_Clear();
                    MessageBox.Show("Check Selected Menu", this.Name);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        private void butCollapseAll_Click(object sender, EventArgs e)
        {
            try
            {
                treeView1.CollapseAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butExpandAll_Click(object sender, EventArgs e)
        {
            try
            {
                treeView1.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void treeView1_Leave(object sender, EventArgs e)
        {
           
        }

        private void Txt_UserName_TextChanged(object sender, EventArgs e)
        {

        }
        void Treeview1_Clear()
        {
            try
            {




                buildtree(); 
             

                //LoadBaseNodes();
                //int intParentID = 0;
                //while (intParentID < treeView1.Nodes.Count)
                //{
                //    int intNodeID = 0;

                //    while (intNodeID < treeView1.Nodes[intParentID].Nodes.Count)
                //    {
                //        treeView1.Nodes[intParentID].Nodes[intNodeID].Checked = false;
                //        intNodeID++;
                //    }

                //    treeView1.Nodes[intParentID].Checked = false;
                //    intParentID++;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
      
        }
        private void Chk_Add_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                CheckSelectMsg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Chk_Add_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                CheckSelectMsg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Chk_Edit_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                CheckSelectMsg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Chk_Edit_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                CheckSelectMsg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Chk_Delete_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                CheckSelectMsg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Chk_Delete_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                CheckSelectMsg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Chk_View_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                CheckSelectMsg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Chk_View_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                CheckSelectMsg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Chk_Print_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                CheckSelectMsg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Chk_Print_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                CheckSelectMsg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Chk_Report_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                CheckSelectMsg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Chk_Report_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                CheckSelectMsg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Chk_Add_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void treeView1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Txt_UserName.Tag.ToString() == "")
                {
                    MessageBox.Show("Select UserName", this.Name);
                    Txt_UserName.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
