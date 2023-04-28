using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounts_ControlModules;
using SelectionTool_NmSp;

namespace Accounts
{
    public partial class FrmFit_Bill_Entry : Form
    {
        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        DataTable Dt = new DataTable();
        String Str;
        TextBox Txt = null;
        String[] Queries;
        String BILL_TYPE = String.Empty;

        public FrmFit_Bill_Entry()
        {
            InitializeComponent();
        }

        private void FrmFit_Bill_Entry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                MyBase.Clear(this);

                //if (MyParent.UserCode != 1 && MyParent.UserCode != 4 )
                //{
                //    if (MyParent.Accounts_Input == false)
                //    {
                //        DataTable Tdt = new DataTable();
                //        Str = "select * from VAAHINI_ERP_GAINUP.dbo.Fit_Bill_Company_Rights where Module='Floor' and user_ID= " + MyParent.UserCode + "";
                //        MyBase.Load_Data(Str, ref Tdt);

                //        if (Tdt.Rows.Count == 0)
                //        {
                //            MessageBox.Show("Please Contact IT Department..!", "Gainup");
                //            TxtCompany.Enabled = false;
                //            return;
                //        }
                //        else
                //        {
                //            DataTable Tdt1 = new DataTable();
                //            Str = "select Company_Name,Company_Code from ACCOUNTS.dbo.Fit_Company_Master where company_code=" + Tdt.Rows[0]["Company_Code"] + "";
                //            MyBase.Load_Data(Str, ref Tdt1);

                //            TxtCompany.Text = Tdt1.Rows[0]["Company_Name"].ToString();
                //            TxtCompany.Tag = Tdt1.Rows[0]["Company_Code"].ToString();

                //            TxtCompany.Enabled = false;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmFit_Bill_Entry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (MyParent.UserCode != 1 && MyParent.UserCode != 4)
                {
                    if (MyParent.Accounts_Input == false)
                    {
                        DataTable Tdt = new DataTable();
                        Str = "select * from VAAHINI_ERP_GAINUP.dbo.Fit_Bill_Company_Rights where Module='Projects' and user_ID= " + MyParent.UserCode + "";
                        MyBase.Load_Data(Str, ref Tdt);

                        if (Tdt.Rows.Count == 0)
                        {
                            MessageBox.Show("Please Contact IT Department..!", "Gainup");
                            TxtCompany.Enabled = false;
                            return;
                        }
                        else
                        {
                            DataTable Tdt1 = new DataTable();
                            Str = "select Company_Name,Company_Code from ACCOUNTS.dbo.Fit_Company_Master where company_code=" + Tdt.Rows[0]["Company_Code"] + "";
                            MyBase.Load_Data(Str, ref Tdt1);

                            TxtCompany.Text = Tdt1.Rows[0]["Company_Name"].ToString();
                            TxtCompany.Tag = Tdt1.Rows[0]["Company_Code"].ToString();

                            TxtCompany.Enabled = false;
                        }
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtSupplier")
                    {
                        if (TxtSupplier.Text == String.Empty)
                        {
                            MessageBox.Show("Please Choose Supplier..!", "Gainup");
                            TxtSupplier.Focus();
                            return;
                        }

                        Grid.CurrentCell = Grid["INVNO", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtBillType")
                    {
                        //if (TxtCompany.Text != String.Empty)
                        //{
                        //    MessageBox.Show("Already Details Entered ...!", "Gainup");
                        //    return;
                        //}

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Bill Type", "select 'GRN' Bill_Type Union select 'TESTING/TRANSPORT' Bill_Type", String.Empty, 250);
                        if (Dr != null)
                        {
                            TxtBillType.Text = Dr["Bill_Type"].ToString();
                            TxtCompany.Focus();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtCompany")
                    {
                        if (TxtBillType.Text ==String.Empty)
                        {
                            MessageBox.Show("Please Choose Bill Type ...!", "Gainup");
                            TxtBillType.Focus();
                            return;
                        }

                        //if (TxtSupplier.Text != String.Empty)
                        //{
                        //    MessageBox.Show("Already Details Entered ...!", "Gainup");
                        //    return;
                        //}

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Company", "select Company_Name,Company_Code from ACCOUNTS.dbo.Fit_Company_Master order by Company_Code", String.Empty, 350);

                        if (Dr != null)
                        {
                            TxtCompany.Text = Dr["Company_Name"].ToString();
                            TxtCompany.Tag = Dr["Company_Code"].ToString();
                            TxtSupplier.Focus();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtSupplier")
                    {
                        if (TxtCompany.Text==String.Empty)
                        {
                            MessageBox.Show("Please Choose Company ...!", "Gainup");
                            TxtCompany.Focus();
                            return;
                        }

                        if (Dt.Rows.Count>0)
                        {
                            MessageBox.Show("Already Details Entered ...!", "Gainup");
                            return;
                        }

                        if (TxtCompany.Text == "GARMENTS")
                        {
                            if (MyParent.Accounts_Input == true)
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Garments_Fit_Bill_INV_Acc_Details() ", String.Empty, 350);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Garments_Fit_Bill_INV_Details() ", String.Empty, 350);
                            }
                        }
                        else if (TxtCompany.Text == "SOCKS")
                        {
                            if (MyParent.Accounts_Input == true)
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Socks_Fit_Bill_INV_Acc_Details() ", String.Empty, 350);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Socks_Fit_Bill_INV_Details() ", String.Empty, 350);
                            }
                        }
                        else if (TxtCompany.Text == "GLOVES")
                        {
                            if (TxtBillType.Text == "GRN")
                            {
                                if (MyParent.Accounts_Input == true)
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Gloves_Fit_Bill_INV_Acc_Details() ", String.Empty, 350);
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Gloves_Fit_Bill_INV_Details() ", String.Empty, 350);
                                }
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Ledger_Name Supplier,Ledger_Code SUpplierID from Gloves.dbo.Supplier_All_Fn_Wc() Where Compcode = " + MyParent.CompCode + " ", String.Empty, 350);
                            }
                        }
                        else if (TxtCompany.Text == "WOVEN")
                        {
                            if (MyParent.Accounts_Input == true)
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Woven_Fit_Bill_INV_Acc_Details() ", String.Empty, 350);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Woven_Fit_Bill_INV_Details() ", String.Empty, 350);
                            }
                        }
                        else if (TxtCompany.Text == "PROJECTS")
                        {
                            if (MyParent.Accounts_Input == true)
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Project_Fit_Bill_INV_Acc_Details() ", String.Empty, 350);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Project_Fit_Bill_INV_Details() ", String.Empty, 350);
                            }
                        }
                        else if (TxtCompany.Text == "WOVEN")
                        {
                            if (MyParent.Accounts_Input == true)
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Socks_Fit_Bill_INV_Acc_Details() ", String.Empty, 350);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Socks_Fit_Bill_INV_Details() ", String.Empty, 350);
                            }
                        }
                        else if (TxtCompany.Text == "OFFSET PRINTING")
                        {
                            if (MyParent.Accounts_Input == true)
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Socks_Printing_Fit_Bill_INV_Acc_Details() ", String.Empty, 350);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Socks_Printing_Fit_Bill_INV_Details() ", String.Empty, 350);
                            }
                        }
                        else if (TxtCompany.Text == "GARMENT PRINTING")
                        {
                            if (MyParent.Accounts_Input == true)
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Garments_Printing_Fit_Bill_INV_Acc_Details() ", String.Empty, 350);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", "select Distinct Supplier,SUpplierID from Accounts.dbo.Garments_Printing_Fit_Bill_INV_Details() ", String.Empty, 350);
                            }
                        }

                        if (Dr != null)
                        {
                            TxtSupplier.Text = Dr["SUPPLIER"].ToString();
                            TxtSupplier.Tag = Dr["SUPPLIERID"].ToString();
                            Grid_Data();
                        }
                    }
                //    else if (this.ActiveControl.Name == "TxtInvNo")
                //    {
                //        if (TxtSupplier.Text == String.Empty)
                //        {
                //            MessageBox.Show("Please Choose Supplier ...!", "Gainup");
                //            TxtSupplier.Focus();
                //            return;
                //        }

                //        if (TxtCompany.Text == "GARMENTS")
                //        {
                //            if (MyParent.Accounts_Input == true)
                //            {
                //                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,ENTRY_NO,ENTRY_DATE,ISSUE_DATE,COMPANY_CODE from Accounts.dbo.Garments_Fit_Bill_INV_Acc_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                //            }
                //            else
                //            {
                //                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type from Accounts.dbo.Garments_Fit_Bill_INV_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                //            }
                //        }
                //        else if (TxtCompany.Text == "SOCKS")
                //        {
                //            if (MyParent.Accounts_Input == true)
                //            {
                //                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,ENTRY_NO,ENTRY_DATE,ISSUE_DATE,COMPANY_CODE from Accounts.dbo.Socks_Fit_Bill_INV_Acc_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                //            }
                //            else
                //            {
                //                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type from Accounts.dbo.Socks_Fit_Bill_INV_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                //            }
                //        }
                //        else if (TxtCompany.Text == "OFFSET PRINTING")
                //        {
                //            if (MyParent.Accounts_Input == true)
                //            {
                //                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,ENTRY_NO,ENTRY_DATE,ISSUE_DATE,COMPANY_CODE from Accounts.dbo.Socks_Printing_Fit_Bill_INV_Acc_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                //            }
                //            else
                //            {
                //                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type from Accounts.dbo.Socks_Printing_Fit_Bill_INV_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                //            }
                //        }
                //        else if (TxtCompany.Text == "GARMENT PRINTING")
                //        {
                //            if (MyParent.Accounts_Input == true)
                //            {
                //                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,ENTRY_NO,ENTRY_DATE,ISSUE_DATE,COMPANY_CODE from Accounts.dbo.Garments_Printing_Fit_Bill_INV_Acc_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                //            }
                //            else
                //            {
                //                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type from Accounts.dbo.Garments_Printing_Fit_Bill_INV_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                //            }
                //        }

                //        if (Dr != null)
                //        {
                //            if (MyParent.Accounts_Input == true)
                //            {
                //                TxtEntryNo.Text = Dr["ENTRY_NO"].ToString();
                //                DtpDate.Value = Convert.ToDateTime(Dr["ENTRY_DATE"]);
                //                DtpIssue.Value = Convert.ToDateTime(Dr["ISSUE_DATE"]);
                //                TxtCompany.Tag = Dr["Company_Code"].ToString();
                //            }

                //            TxtInvNo.Text = Dr["InvNo"].ToString();
                //            DtpInvDate.Value = Convert.ToDateTime(Dr["INVDATE"]);
                //            TxtNetAmount.Text = String.Format("{0:n}",  Convert.ToDouble(Dr["AMOUNT"]));
                //            BILL_TYPE = Dr["BILL_TYPE"].ToString();
                //            Grid_Data();
                //        }
                //    }
                }
                else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
                {
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    if (this.ActiveControl.Name == String.Empty)
                    {

                    }
                    else if (this.ActiveControl.Name == "Grid")
                    {
                        BtnOK.Focus();
                        return;
                    }
                    else
                    {
                        MyBase.ActiveForm_Close(this, MyParent);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmFit_Bill_Entry_KeyPress(object sender, KeyPressEventArgs e)
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

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Dt = new DataTable();
            MyBase.Clear(this);
            TxtBillType.Focus();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void Grid_Data()
        {
            try
            {
                Str = "select 0 SNO, INVNO, INVDATE, NETAMOUNT NETAMT,'' [DATE LOCK], ROWID from accounts.dbo.Fit_Bill_Master WHERE 1=2 ";
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                if (MyParent.Accounts_Input == true)
                {
                    MyBase.Grid_Designing(ref Grid, ref Dt, "ROWID");
                }
                else
                {
                    MyBase.Grid_Designing(ref Grid, ref Dt, "ROWID", "DATE LOCK");
                }
                MyBase.ReadOnly_Grid_Without(ref Grid, "INVNO");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 40, 130, 130, 150);
                Grid.RowHeadersWidth = 40;
                Grid.Columns["NETAMT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["DATE LOCK"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Grid_Detail()
        {
            DataTable Dt1 = new DataTable();
            try
            {
                if (Grid.CurrentCell != null && Grid.CurrentCell.Value != DBNull.Value && Grid.CurrentCell.Value.ToString() != String.Empty && Grid.CurrentCell.RowIndex < Grid.Rows.Count - 1)
                {
                    if (TxtCompany.Text == "GARMENTS")
                    {
                        Str = "select 0 SNO, Receipt_No GRNNO,Receipt_Date GRNDATE from Accounts.dbo.Garments_Fit_Bill_GRN_Details() where supplierid=" + TxtSupplier.Tag + " and InvNo='" + Grid["INVNO", Grid.CurrentCell.RowIndex].Value + "'";
                    }
                    else if (TxtCompany.Text == "GLOVES")
                    {
                        Str = "select 0 SNO, Receipt_No GRNNO,Receipt_Date GRNDATE from Accounts.dbo.Gloves_Fit_Bill_GRN_Details() where supplierid=" + TxtSupplier.Tag + " and InvNo='" + Grid["INVNO", Grid.CurrentCell.RowIndex].Value + "'";
                    }
                    else if (TxtCompany.Text == "WOVEN")
                    {
                        Str = "select 0 SNO, Receipt_No GRNNO,Receipt_Date GRNDATE from Accounts.dbo.Woven_Fit_Bill_GRN_Details() where supplierid=" + TxtSupplier.Tag + " and InvNo='" + Grid["INVNO", Grid.CurrentCell.RowIndex].Value + "'";
                    }
                    else if (TxtCompany.Text == "PROJECTS")
                    {
                        Str = "select 0 SNO, Receipt_No GRNNO,Receipt_Date GRNDATE from Accounts.dbo.Project_Fit_Bill_GRN_Details() where supplierid=" + TxtSupplier.Tag + " and InvNo='" + Grid["INVNO", Grid.CurrentCell.RowIndex].Value + "'";
                    }
                    else if (TxtCompany.Text == "SOCKS")
                    {
                        Str = "select 0 SNO, Receipt_No GRNNO,Receipt_Date GRNDATE from Accounts.dbo.Socks_Fit_Bill_GRN_Details() where supplierid=" + TxtSupplier.Tag + " and InvNo='" + Grid["INVNO", Grid.CurrentCell.RowIndex].Value + "'";
                    }
                    else if (TxtCompany.Text == "OFFSET PRINTING")
                    {
                        Str = "select 0 SNO, Receipt_No GRNNO,Receipt_Date GRNDATE from Accounts.dbo.Socks_Printing_Fit_Bill_GRN_Details() where supplierid=" + TxtSupplier.Tag + " and InvNo='" + Grid["INVNO", Grid.CurrentCell.RowIndex].Value + "'";
                    }
                    else if (TxtCompany.Text == "GARMENT PRINTING")
                    {
                        Str = "select 0 SNO, Receipt_No GRNNO,Receipt_Date GRNDATE from Accounts.dbo.Garments_Printing_Fit_Bill_GRN_Details() where supplierid=" + TxtSupplier.Tag + " and InvNo='" + Grid["INVNO", Grid.CurrentCell.RowIndex].Value + "'";
                    }
                    Grid1.DataSource = MyBase.Load_Data(Str, ref Dt1);
                    MyBase.ReadOnly_Grid(ref Grid1, "SNO", "GRNNO", "GRNDATE");
                    MyBase.Grid_Colouring(ref Grid1, Control_Modules.Grid_Design_Mode.Column_Wise);
                    MyBase.Grid_Width(ref Grid1, 40, 150, 130);
                    Grid1.RowHeadersWidth = 40;
                }
                else
                {
                    Grid1.DataSource = null;
                    Dt1 = new DataTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            String[] Queries;
            Int32 Array_Index = 0;

            try
            {
                if (TxtBillType.Text == String.Empty || TxtCompany.Text == String.Empty || TxtSupplier.Text == String.Empty)
                {
                    MessageBox.Show("Invalid Details", "Gainup");
                    TxtBillType.Focus();
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtBillType.Focus();
                    return;
                }

                Queries = new string[Dt.Rows.Count * 100];

                if (MyParent.Accounts_Input == true)
                {
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        Queries[Array_Index++] = "update Accounts.dbo.Fit_Bill_Master set Acc_date=getdate(),Acc_System=Host_Name() Where ROWID=" + Dt.Rows[i]["ROWID"].ToString() + " ";
                    }
                }
                else
                {
                    TxtEntryNo.Text = MyBase.MaxOnlyComp("Accounts.dbo.Fit_Bill_Master", "Entry_No", String.Empty, MyParent.YearCode, Convert.ToInt16(TxtCompany.Tag)).ToString();
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        Queries[Array_Index++] = "Insert into Accounts.dbo.Fit_Bill_Master(ENTRY_NO,ENTRY_DATE,INVNO,INVDATE, SUPPLIER_CODE,NETAMOUNT,ISSUE_DATE, COMPANY_CODE,BILL_TYPE) values(" + TxtEntryNo.Text.ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "','" + Dt.Rows[i]["INVNO"].ToString() + "','" + String.Format("{0:dd-MMM-yyyy}", Dt.Rows[i]["INVDATE"]) + "', " + TxtSupplier.Tag.ToString() + ", " + Dt.Rows[i]["NETAMT"].ToString() + ",'" + String.Format("{0:dd-MMM-yyyy}", DtpIssue.Value) + "'," + TxtCompany.Tag.ToString() + ",'" + BILL_TYPE + "' ); Select Scope_Identity()";
                    }
                    //for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    //{
                    //    //Queries[Array_Index++] = "Insert into Accounts.dbo.Fit_Bill_Details (Master_ID, Slno,GRNNO,GRNDATE) Values (@@IDENTITY, " + Dt.Rows[i]["Sno"].ToString() + ", '" + Dt.Rows[i]["GRNNO"].ToString() + "', '" + String.Format("{0:dd-MMM-yyyy}", Dt.Rows[i]["GRNDATE"]) + "')";
                    //}
                }

                MyBase.Run_Identity(MyParent.Edit, Queries);
                MessageBox.Show("Saved ...!", "Gainup");
                MyParent.Save_Error = false;
                MyBase.Clear(this);
                Dt = new DataTable();
                TxtBillType.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt == null)
                {
                    Txt = (TextBox)e.Control;
                    Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["INVNO"].Index)
                    {
                        if (TxtSupplier.Text != String.Empty)
                        {
                            if (TxtCompany.Text == "GARMENTS")
                            {
                                if (MyParent.Accounts_Input == true)
                                {
                                    Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,ENTRY_NO,ENTRY_DATE,ISSUE_DATE,COMPANY_CODE,ROWID from Accounts.dbo.Garments_Fit_Bill_INV_Acc_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,0 ROWID from Accounts.dbo.Garments_Fit_Bill_INV_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                                }
                            }
                            else if (TxtCompany.Text == "GLOVES")
                            {
                                if (TxtBillType.Text == "GRN")
                                {
                                    if (MyParent.Accounts_Input == true)
                                    {
                                        Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,ENTRY_NO,ENTRY_DATE,ISSUE_DATE,COMPANY_CODE,ROWID from Accounts.dbo.Gloves_Fit_Bill_INV_Acc_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                                    }
                                    else
                                    {
                                        Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,0 ROWID from Accounts.dbo.Gloves_Fit_Bill_INV_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                                    }
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,0 ROWID from Accounts.dbo.Gloves_Testing_Transport() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);                                        
                                }
                            }
                            else if (TxtCompany.Text == "WOVEN")
                            {
                                if (MyParent.Accounts_Input == true)
                                {
                                    Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,ENTRY_NO,ENTRY_DATE,ISSUE_DATE,COMPANY_CODE,ROWID from Accounts.dbo.Woven_Fit_Bill_INV_Acc_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,0 ROWID from Accounts.dbo.Woven_Fit_Bill_INV_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                                }
                            }
                            else if (TxtCompany.Text == "PROJECTS")
                            {
                                if (MyParent.Accounts_Input == true)
                                {
                                    Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,ENTRY_NO,ENTRY_DATE,ISSUE_DATE,COMPANY_CODE,ROWID from Accounts.dbo.Project_Fit_Bill_INV_Acc_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,0 ROWID from Accounts.dbo.Project_Fit_Bill_INV_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                                }
                            }
                            else if (TxtCompany.Text == "SOCKS")
                            {
                                if (MyParent.Accounts_Input == true)
                                {
                                    Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,ENTRY_NO,ENTRY_DATE,ISSUE_DATE,COMPANY_CODE,ROWID from Accounts.dbo.Socks_Fit_Bill_INV_Acc_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,0 ROWID from Accounts.dbo.Socks_Fit_Bill_INV_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                                }
                            }
                            else if (TxtCompany.Text == "OFFSET PRINTING")
                            {
                                if (MyParent.Accounts_Input == true)
                                {
                                    Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,ENTRY_NO,ENTRY_DATE,ISSUE_DATE,COMPANY_CODE,ROWID from Accounts.dbo.Socks_Printing_Fit_Bill_INV_Acc_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,0 ROWID from Accounts.dbo.Socks_Printing_Fit_Bill_INV_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                                }
                            }
                            else if (TxtCompany.Text == "GARMENT PRINTING")
                            {
                                if (MyParent.Accounts_Input == true)
                                {
                                    Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,ENTRY_NO,ENTRY_DATE,ISSUE_DATE,COMPANY_CODE,ROWID from Accounts.dbo.Garments_Printing_Fit_Bill_INV_Acc_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                                }
                                else
                                {
                                    Dr = Tool.Selection_Tool_Except_New("INVNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Invoice No", "select InvNo,InvDate,Amount,'G' Bill_Type,0 ROWID from Accounts.dbo.Garments_Printing_Fit_Bill_INV_Details() where supplierid=" + TxtSupplier.Tag + " ", String.Empty, 100, 100, 100);
                                }
                            }

                            if (Dr != null)
                            {
                                if (MyParent.Accounts_Input == true)
                                {
                                    TxtEntryNo.Text = Dr["ENTRY_NO"].ToString();
                                    DtpDate.Value = Convert.ToDateTime(Dr["ENTRY_DATE"]);
                                    DtpIssue.Value = Convert.ToDateTime(Dr["ISSUE_DATE"]);
                                    TxtCompany.Tag = Dr["Company_Code"].ToString();
                                }

                                Txt.Text = Dr["InvNo"].ToString();
                                Grid["INVNO", Grid.CurrentCell.RowIndex].Value = Dr["InvNo"].ToString();
                                Grid["INVDATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Dr["INVDATE"]);
                                Grid["NETAMT", Grid.CurrentCell.RowIndex].Value = Dr["AMOUNT"].ToString();
                                Grid["ROWID", Grid.CurrentCell.RowIndex].Value = Dr["ROWID"].ToString();
                                BILL_TYPE = Dr["BILL_TYPE"].ToString();

                                if (MyParent.Accounts_Input == true)
                                {
                                    DataTable Tmpdt = new DataTable();
                                    if (TxtCompany.Text == "GARMENTS")
                                    {
                                        Tmpdt = new DataTable();
                                        Str = "select max(Receipt_date) Receipt_date,(case when DATEDIFF(D,max(Receipt_date),GETDATE())>15 then 'Y' else 'N' end) Date_Lock from Accounts.dbo.Garments_Fit_Bill_GRN_Details() where SUpplierID=" + TxtSupplier.Tag + " and InvNo='" + Grid["INVNO", Grid.CurrentCell.RowIndex].Value + "' and InvDate='" + String.Format("{0:dd-MMM-yyyy}", Grid["INVDATE", Grid.CurrentCell.RowIndex].Value) + "'";
                                        MyBase.Load_Data(Str,ref Tmpdt);
                                    }
                                    if (TxtCompany.Text == "PROJECTS")
                                    {
                                        Tmpdt = new DataTable();
                                        Str = "select max(Receipt_date) Receipt_date,(case when DATEDIFF(D,max(Receipt_date),GETDATE())>15 then 'Y' else 'N' end) Date_Lock from Accounts.dbo.PRoject_Fit_Bill_GRN_Details() where SUpplierID=" + TxtSupplier.Tag + " and InvNo='" + Grid["INVNO", Grid.CurrentCell.RowIndex].Value + "' and InvDate='" + String.Format("{0:dd-MMM-yyyy}", Grid["INVDATE", Grid.CurrentCell.RowIndex].Value) + "'";
                                        MyBase.Load_Data(Str, ref Tmpdt);
                                    }
                                    else if (TxtCompany.Text == "SOCKS")
                                    {
                                        Tmpdt = new DataTable();
                                        Str = "select max(Receipt_date) Receipt_date,(case when DATEDIFF(D,max(Receipt_date),GETDATE())>15 then 'Y' else 'N' end) Date_Lock from Accounts.dbo.Socks_Fit_Bill_GRN_Details() where SUpplierID=" + TxtSupplier.Tag + " and InvNo='" + Grid["INVNO", Grid.CurrentCell.RowIndex].Value + "' and InvDate='" + String.Format("{0:dd-MMM-yyyy}", Grid["INVDATE", Grid.CurrentCell.RowIndex].Value) + "'";
                                        MyBase.Load_Data(Str, ref Tmpdt);
                                    }
                                    else if (TxtCompany.Text == "GLOVES")
                                    {
                                        Tmpdt = new DataTable();
                                        Str = "select max(Receipt_date) Receipt_date,(case when DATEDIFF(D,max(Receipt_date),GETDATE())>15 then 'Y' else 'N' end) Date_Lock from Accounts.dbo.Gloves_Fit_Bill_GRN_Details() where SUpplierID=" + TxtSupplier.Tag + " and InvNo='" + Grid["INVNO", Grid.CurrentCell.RowIndex].Value + "' and InvDate='" + String.Format("{0:dd-MMM-yyyy}", Grid["INVDATE", Grid.CurrentCell.RowIndex].Value) + "'";
                                        MyBase.Load_Data(Str, ref Tmpdt);
                                    }
                                    else if (TxtCompany.Text == "WOVEN")
                                    {
                                        Tmpdt = new DataTable();
                                        Str = "select max(Receipt_date) Receipt_date,(case when DATEDIFF(D,max(Receipt_date),GETDATE())>15 then 'Y' else 'N' end) Date_Lock from Accounts.dbo.Woven_Fit_Bill_GRN_Details() where SUpplierID=" + TxtSupplier.Tag + " and InvNo='" + Grid["INVNO", Grid.CurrentCell.RowIndex].Value + "' and InvDate='" + String.Format("{0:dd-MMM-yyyy}", Grid["INVDATE", Grid.CurrentCell.RowIndex].Value) + "'";
                                        MyBase.Load_Data(Str, ref Tmpdt);
                                    }
                                    else if (TxtCompany.Text == "OFFSET PRINTING")
                                    {
                                        Tmpdt = new DataTable();
                                        Str = "select max(Receipt_date) Receipt_date,(case when DATEDIFF(D,max(Receipt_date),GETDATE())>15 then 'Y' else 'N' end) Date_Lock from Accounts.dbo.Socks_Printing_Fit_Bill_GRN_Details() where SUpplierID=" + TxtSupplier.Tag + " and InvNo='" + Grid["INVNO", Grid.CurrentCell.RowIndex].Value + "' and InvDate='" + String.Format("{0:dd-MMM-yyyy}", Grid["INVDATE", Grid.CurrentCell.RowIndex].Value) + "'";
                                        MyBase.Load_Data(Str, ref Tmpdt);
                                    }
                                    else if (TxtCompany.Text == "GARMENT PRINTING")
                                    {
                                        Tmpdt = new DataTable();
                                        Str = "select max(Receipt_date) Receipt_date,(case when DATEDIFF(D,max(Receipt_date),GETDATE())>15 then 'Y' else 'N' end) Date_Lock from Accounts.dbo.Garments_Printing_Fit_Bill_GRN_Details() where SUpplierID=" + TxtSupplier.Tag + " and InvNo='" + Grid["INVNO", Grid.CurrentCell.RowIndex].Value + "' and InvDate='" + String.Format("{0:dd-MMM-yyyy}", Grid["INVDATE", Grid.CurrentCell.RowIndex].Value) + "'";
                                        MyBase.Load_Data(Str, ref Tmpdt);
                                    }


                                    Grid["DATE LOCK", Grid.CurrentCell.RowIndex].Value = Tmpdt.Rows[0]["Date_Lock"].ToString();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Supplier..!", "Gainup");
                            TxtSupplier.Focus();
                        }
                    }
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    BtnOK.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_CurrentCellChanged(object sender, EventArgs e)
        {
            Grid_Detail();
        }

        private void Grid1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure to Delete..?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButView_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtCompany.Text == String.Empty)
                {
                    MessageBox.Show("Invalid Company", "Gainup");
                    TxtCompany.Focus();
                    return;
                }

                if (TxtCompany.Text == "GLOVES")
                {
                    Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Bill - View", " SElect * From Gloves.Dbo.Gloves_Bill_Entry_View_FN()  ORder by 1 desc", String.Empty, 80, 80, 150, 120, 100, 100, 100, 100, 100);
                }
                else if (TxtCompany.Text == "PROJECTS")
                {
                    Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Bill - View", " SElect * From PRojects.Dbo.Project_Bill_Entry_View_FN()  ORder by 1 desc", String.Empty, 80, 80, 150, 120, 100, 100, 100, 100, 100);

                }

                    if (Dr != null)
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