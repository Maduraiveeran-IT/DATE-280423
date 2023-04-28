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
using System.IO;

namespace Accounts
{
    public partial class FrmOrganogram : Form
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        String Code;
        Int64 Emplno1;
        Int64 Emplno2 = 0 ;


        public FrmOrganogram(Int64 Emplno)
        {
            InitializeComponent();
            Emplno1 = Emplno;
        }

        void TxtBankShortName_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
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

        void Load_Combo()
        {
            try
            {
                CmbType.Items.Clear();
                CmbType.Items.Add("Profit & Loss");
                CmbType.Items.Add("Balance Sheet");
                CmbType.SelectedIndex = 0;
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
                //Load_Combo();
                OptPandLV.Checked = true;
                GB2.Visible = false;
                //Load_tree_View();
                //tableCreation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Update_Order_Slno_PV()
        {
            DataTable Dt = new DataTable();
            DataTable Dt1 = new DataTable();
            try
            {
                MyBase.Load_Data("Select distinct SubHead_Code from VAAHINI_ERP_GAINUP.dbo.groupmas_Setting where type = 'P' and vorh = 'V' and company_Code = " + MyParent.CompCode + " and year_Code = '" + MyParent.YearCode + "' order by SubHead_Code ", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.Load_Data("Select groupCode, order_slno from VAAHINI_ERP_GAINUP.dbo.groupmas_Setting where type = 'P' and vorh = 'V' and company_Code = " + MyParent.CompCode + " and year_Code = '" + MyParent.YearCode + "' and subhead_Code = " + Dt.Rows[i]["Subhead_Code"].ToString() + " order by order_Slno ", ref Dt1);
                    for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                    {
                        MyBase.Execute("Update VAAHINI_ERP_GAINUP.dbo.groupmas_Setting Set Order_Slno = " + Convert.ToString(j + 1) + " where type = 'P' and vorh = 'V' and company_Code = " + MyParent.CompCode + " and year_Code = '" + MyParent.YearCode + "' and subhead_Code = " + Dt.Rows[i]["Subhead_Code"].ToString() + " and groupcode = " + Dt1.Rows[j]["GroupCode"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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

        void Load_tree()
        {
            DataTable Dt = new DataTable();
            String Str = String.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Grid_Data()
        {
            DataTable Dt = new DataTable();
            String Str = String.Empty;
            try
            {
                Load_tree_View();
                return;
                Str = "select groupcode, groupname, Null as subgroup_Code, null as subgroup_Name from VAAHINI_ERP_GAINUP.dbo.groupmas where groupcode = groupunder union ";
                Str += " select groupunder groupcode, Null groupname, groupcode as subgroup_Code, groupname subgroup_Name from VAAHINI_ERP_GAINUP.dbo.groupmas where groupcode <> groupunder";
                MyBase.Execute_Qry(Str, "GrpList");
                Str = "select GroupCode Code, GroupName Group_, Subgroup_Code Sub_Code, subgroup_Name Sub_Group from VAAHINI_ERP_GAINUP.dbo.GrpList order by groupcode, subgroup_code";
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt); 
                MyBase.Grid_Designing(ref Grid, ref Dt);
                MyBase.Grid_Width(ref Grid, 80, 350, 80, 350);
                Grid.RowHeadersWidth = 10;
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid);
                Grid_Alter();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Grid2_Data()
        {
            String Str2 = String.Empty;
            try
            {
                String Str;
                DataTable Dt1 = new DataTable();
                Str = "Select * , '' T from VAAHINI_ERP_GAINUP.dbo.Staff_KPI_Report_New_One(" + Emplno2 + ")Order by Month_Year";
                Grid1.DataSource = MyBase.Load_Data(Str, ref Dt1);
                if (Dt1.Rows.Count > 0)
                {
                    MyBase.Grid_Colouring(ref Grid1, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Designing(ref Grid1, ref Dt1, "T");
                    MyBase.ReadOnly_Grid_Without(ref Grid1, "T");
                    MyBase.Grid_Width(ref Grid1, 100, 250, 100, 150, 150, 150, 250, 100, 100);
                    Grid1.RowHeadersWidth = 15;
                    Grid1.Focus();
                    Grid1.BeginEdit(false);
                    GB2.Visible = true;
                }
                else
                {

                    MessageBox.Show("No Details Found...!", "Gainup");
                    GB2.Visible = false;
                    return;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Load_tree_View()
        {
            DataTable Dt = new DataTable();
            DataTable Dt1 = new DataTable();
            DataTable Dt2 = new DataTable();
            DataTable Dt3 = new DataTable();
            DataTable Dt4 = new DataTable();
            DataTable Dt5 = new DataTable();
            DataTable Dt6 = new DataTable();
            try
            {
                treeView1.Nodes.Clear();
                
                //MyBase.Load_Data("Select GroupName, groupcode from groupmas where groupreserved = groupcode and company_Code = " + MyParent.CompCode + " and year_Code = '" + MyParent.YearCode + "' order by groupname", ref Dt);
                if (Emplno1 == 0)
                {
                    MyBase.Load_Data("Select Emplno Groupcode, Employee GroupName From Vaahini_ERP_Gainup.Dbo.Gainup_Organogram () Where Emplno = 5545 Order By TNo", ref Dt);
                    Emplno2 = 5545;
                }
                else
                {
                    MyBase.Load_Data("Select Emplno Groupcode, Employee GroupName From Vaahini_ERP_Gainup.Dbo.Gainup_Organogram () Where Emplno = " + Emplno1 + " Order By TNo", ref Dt);
                    Emplno2 = Emplno1;
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    TreeNode group, group1, group2, group3, group4, group5;
                    group = new TreeNode();
                    group.Text = Dt.Rows[i]["GroupName"].ToString();
                    group.Tag = Dt.Rows[i]["GroupCode"].ToString();
                    MyBase.Load_Data("Select Emplno GroupCode, Employee GroupName From Vaahini_ERP_Gainup.Dbo.Gainup_Organogram () Where Eva_Person = " + Dt.Rows[i]["Groupcode"].ToString() + " order by Tno ", ref Dt1);
                    for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                    {
                        group1 = new TreeNode();
                        group1.Text = Dt1.Rows[j]["GroupName"].ToString();
                        group1.Tag = Dt1.Rows[j]["groupcode"].ToString();
                        MyBase.Load_Data("Select Emplno GroupCode, Employee GroupName From Vaahini_ERP_Gainup.Dbo.Gainup_Organogram () Where Eva_Person = " + Dt1.Rows[j]["Groupcode"].ToString() + " order by TNo", ref Dt2);
                        for (int k = 0; k <= Dt2.Rows.Count - 1; k++)
                        {
                            group2 = new TreeNode();
                            group2.Text = Dt2.Rows[k]["GroupName"].ToString();
                            group2.Tag = Dt2.Rows[k]["groupcode"].ToString();
                            MyBase.Load_Data("Select Emplno GroupCode, Employee GroupName From Vaahini_ERP_Gainup.Dbo.Gainup_Organogram () Where Eva_Person = " + Dt2.Rows[k]["Groupcode"].ToString() + " order by TNo", ref Dt4);
                            for (int l = 0; l <= Dt4.Rows.Count - 1; l++)
                            {
                                group3 = new TreeNode();
                                group3.Text = Dt4.Rows[l]["GroupName"].ToString();
                                group3.Tag = Dt4.Rows[l]["groupcode"].ToString();
                                MyBase.Load_Data("Select Emplno GroupCode, Employee GroupName From Vaahini_ERP_Gainup.Dbo.Gainup_Organogram () Where Eva_Person = " + Dt4.Rows[l]["Groupcode"].ToString() + " order by TNo", ref Dt5);
                                for (int m = 0; m <= Dt5.Rows.Count - 1; m++)
                                {
                                    group4 = new TreeNode();
                                    group4.Text = Dt5.Rows[m]["GroupName"].ToString();
                                    group4.Tag = Dt5.Rows[m]["groupcode"].ToString();
                                    MyBase.Load_Data("Select Emplno GroupCode, Employee GroupName From Vaahini_ERP_Gainup.Dbo.Gainup_Organogram () Where Eva_Person = " + Dt5.Rows[m]["Groupcode"].ToString() + " order by TNo", ref Dt6);
                                    for (int n = 0; n <= Dt6.Rows.Count - 1; n++)
                                    {
                                        group5 = new TreeNode();
                                        group5.Text = Dt6.Rows[n]["GroupName"].ToString();
                                        group5.Tag = Dt6.Rows[n]["groupcode"].ToString();
                                        group4.Nodes.Add(group5);
                                    }
                                    group3.Nodes.Add(group4);
                                }
                                group2.Nodes.Add(group3);
                            }
                            group1.Nodes.Add(group2);
                        }
                        group.Nodes.Add(group1);
                    }
                    treeView1.Nodes.Add(group);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TxtOrderSlno_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //Grid_Data();
                this.Cursor = Cursors.WaitCursor;
                Load_tree_View();
                //listBox1.DataSource = null;
                label2.Text = "Ledgers : 0";
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        void Grid_Alter()
        {
            try
            {
                for (int i = Grid.Rows.Count - 1; i > 0; i--)
                {
                    if (Grid["Code", i].Value != null && Grid["Code", i].Value != DBNull.Value)
                    {
                        if (Convert.ToDouble(Grid["Code", i].Value) == Convert.ToDouble(Grid["Code", i - 1].Value))
                        {
                            Grid["Code", i].Value = DBNull.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DataTable Dt = new DataTable();
            try
            {
                label2.Text = treeView1.SelectedNode.Text.ToString();
                MyBase.Load_Data("Select * From Vaahini_ERP_Gainup.dbo.Gainup_Organogram_Details (" + treeView1.SelectedNode.Tag.ToString() + ")", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    LblName.Text = Dt.Rows[0]["Name"].ToString();
                    Emplno2 = Convert.ToInt64(Dt.Rows[0]["Emplno"].ToString());
                    LblTno.Text = Dt.Rows[0]["tno"].ToString();
                    LblComp.Text = Dt.Rows[0]["CompName"].ToString();
                    Lbldegn.Text = Dt.Rows[0]["DesignationName"].ToString();
                    LblDoj.Text = String.Format("{0:dd-MMM-yyyy}",Convert.ToDateTime(Dt.Rows[0]["dateofjoin"].ToString()));
                    LblDpt.Text = Dt.Rows[0]["Department"].ToString();
                    DataTable Count = new DataTable();
                    MyBase.Load_Data("Select Count(Tno) Cnt From Vaahini_ERP_Gainup.dbo.Gainup_Organogram () Where EVa_Person = " + treeView1.SelectedNode.Tag.ToString() + "", ref Count);
                    if (Count.Rows.Count > 0)
                    {
                        if (Convert.ToInt64(Count.Rows[0]["Cnt"].ToString()) == 0)
                        {
                            LblCount.Text = "-";
                        }
                        else
                        {
                           LblCount.Text = Count.Rows[0]["Cnt"].ToString();
                        }
                    }
                    Load_EMpl_Photo(Convert.ToInt64(treeView1.SelectedNode.Tag.ToString()));

                }

                //Count
                //Select * From Gainup_Organogram () Where EVa_Person = 5545

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Load_EMpl_Photo(Int64 Emplno)
        {
            DataTable TmpDt;
            String Str = String.Empty;
            try
            {
                TmpDt = new DataTable();
                Str = "Select * From VAAHINI_GAINUP_PHOTO.dbo.EMPLPHOTO Where Emplno = " + Emplno + " and type=1 ";
                MyBase.Load_Data(Str, ref TmpDt);
                if (TmpDt.Rows.Count > 0)
                {
                    if (TmpDt.Rows[0]["Photo"] != DBNull.Value)
                    {
                        Byte[] Data = (Byte[])TmpDt.Rows[0]["Photo"];
                        Image Ephoto1;
                        using (MemoryStream MS = new MemoryStream(Data, 0, Data.Length))
                        {
                            MS.Write(Data, 0, Data.Length);
                            Ephoto1 = Image.FromStream(MS, true);

                        }
                        EmplPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
                        EmplPhoto.Image = Ephoto1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Emplno2 > 0)
                {
                    Grid2_Data();
                }
                else
                {
                    MessageBox.Show("Invalid Employee...!", "Gainup");
                    button3.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                GB2.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}