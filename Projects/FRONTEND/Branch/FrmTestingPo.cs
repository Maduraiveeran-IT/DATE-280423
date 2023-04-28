using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmTestingPo : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataGrid dgv = new DataGrid();
        DataRow Dr;
        Int64 Code;
        Int16 C = 0;
        TextBox Txt = null;
        TextBox Txt_Qty = null;
        String[] Queries;
        String Str;
        Int32 B = 0;
        DataTable[] DtQty;
        Int16 PCompCode;
        Int16 Vis = 0;
        int Pos = 0;
        public FrmTestingPo()
        {
            InitializeComponent();
        }
        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);                
                Eno_Generate();
                Grid_Data();
                TxtSupplier.Enabled = true;
                DtpDate.Enabled = false;
                TxtRemarks.Enabled = true;
                TxtTotAmnt.Enabled = true;
                DtQty = new DataTable[300];
                TxtSupplier.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        Boolean Check_Qty_Breakup()
        {
            Double BRQty = 0;
            try
            {
                for (int i = 0; i < Grid.Rows.Count - 1; i++)
                {
                    BRQty = 0;
                    if (Convert.ToDouble(Grid["Net_Amnt", i].Value.ToString()) > 0)
                    {
                        if (DtQty[i] == null)
                        {
                            MessageBox.Show("Invalid Amount Breakup Details ...!", "Gainup");
                            MyParent.Save_Error = true;
                            Grid.CurrentCell = Grid["Amount", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return false;
                        }
                        else
                        {
                            if (DtQty[i].Rows.Count == 0)
                            {
                                MessageBox.Show("Invalid Amount Breakup Details in Process...!", "Gainup");
                                Grid.Focus();
                                Grid.CurrentCell = Grid["Amount", i];
                                Grid.BeginEdit(true);
                                return false;
                            }
                            for (int j = 0; j <= (DtQty[i].Rows.Count - 1); j++)
                            {
                                BRQty = Math.Round(Convert.ToDouble(BRQty) + Convert.ToDouble(DtQty[i].Rows[j]["TVal"]), 2);
                            }
                            if (Convert.ToDouble(Grid["Net_Amnt", i].Value.ToString()) != Convert.ToDouble(BRQty))
                            {
                                Grid.Focus();
                                Grid.CurrentCell = Grid["Tax_Per", i];
                                Grid.BeginEdit(true);
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }


        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                DtpDate.Enabled = false;
                //MyBase.Enable_Controls(this, false);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select ManDays Entry - Edit", "Select S1.Eno, S1.Edate, Order_No, S3.Name, S4.Item, S5.Ledger_Name Party, Sum(Amount)Amount, S1.Rowid, S1.Supplier_Id, S1.Remarks from ManDays_Entry_Master S1 Left Join Mandays_Entry_Details S2 on S1.Rowid = S2.Master_Id Left Join Project_Name_Master S3 on S2.Project_Id = S3.Rowid left Join Item S4 on S2.Itemid = S4.ItemID Left Join Supplier_all_Fn_Co1()S5 on   S1.Company_Code  = S5.Company_Code and S1.Supplier_ID = S5.LedgeR_Code Where Isnull(S1.Approved,'N')='N' Group By S1.Eno, S1.Edate, Order_No, S3.Name, S4.Item, S5.Ledger_Name, S1.Rowid, S1.Remarks, Supplier_ID ", String.Empty, 100, 100, 110, 175, 120, 200, 150);
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


        Int16 Max_Slno_Grid()
        {
            Int16 No = 0;
            try
            {
                if (Dt.Rows.Count == 0)
                {
                    No = 1;
                    return No;
                }
                else
                {
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (No < Convert.ToInt16(Dt.Rows[i]["SNo"]))
                        {
                            No = Convert.ToInt16(Dt.Rows[i]["SNo"]);
                        }
                    }
                }
                No += 1;
                return No;
            }
            catch (Exception ex)
            {
                return No;
            }
        }

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                DtQty = new DataTable[300];
                Code = Convert.ToInt64(Dr["RowID"]);
                TxtENo.Text = Dr["ENo"].ToString();                                
                DtpDate.Value = Convert.ToDateTime(Dr["EDate"].ToString());
                TxtSupplier.Text = Dr["Party"].ToString();
                TxtSupplier.Tag = Dr["Supplier_Id"].ToString();                
                TxtRemarks.Text = Dr["Remarks"].ToString();                
                TxtSupplier.Focus();
                Grid_Data();
                Total_Count();
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
                Eno_Generate();
                Int32 Array_Index = 0;
                Total_Count();
                if (TxtSupplier.Text.Trim() == string.Empty)
                {
                    if (TxtSupplier.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Invalid Supplier Details", "Gainup");
                        TxtSupplier.Focus();
                    }                    
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtTotAmnt.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotAmnt.Text) == 0)
                {
                    MessageBox.Show("Invalid Amount Details ", "Gainup");
                    Grid.CurrentCell = Grid["Value", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 2; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 1; j++)
                    {
                        if (Grid.Columns[j].Name.ToString() != "Tax_Per")
                        {
                            if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty || Grid[j, i].Value.ToString() == "0")
                            {
                                MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                Grid.CurrentCell = Grid[j, i];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }
                    }
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 5; j++)
                    {
                        if (Grid["Value", i].Value == DBNull.Value || Grid["Value", i].Value.ToString() == String.Empty || Grid["Value", i].Value.ToString() == "0")
                        {
                            MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                            Grid.CurrentCell = Grid[j, i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                        if(MyParent._New)
                        {
                            if (Convert.ToDouble(Grid["Value", i].Value) > Convert.ToDouble(Grid["Plan_Value", i].Value))
                            {
                                MessageBox.Show("Invalid Amount..!", "Gainup");
                                Grid.CurrentCell = Grid["Value", i];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = "Select Order_No, Project_Id, Itemid, Sum(App_Amnt)App_Amnt, Sum(Paid_Amount)Paid_Amount, Sum(App_Amnt)-Sum(Paid_Amount) Bal_Amnt From(Select S2.Order_No, S2.Project_Id, S2.Itemid, Sum(S2.Amount)Paid_Amount, 0 App_Amnt From Mandays_Entry_Master S1 left Join Mandays_Entry_Details S2 on S1.Rowid = S2.Master_Id ";
                    Str = Str + " Where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' And Project_Id = " + Grid["Project_ID", i].Value.ToString() + " And Itemid = " + Grid["ManDays_ID", i].Value.ToString() + "";
                    if (MyParent.Edit)
                    {
                        Str = Str + " S1.RowID != " + Code + "";
                    }
                    Str = Str + " Group By S2.Order_No, S2.Project_Id, S2.Itemid ";
                    Str = Str + " Union All select Order_No, Proj_Type_ID, Item_ID, 0, Sum(Pur_Amount)Pur_Amount From Project_Planning_Material_Fn() where item like '%man%Days%' And Flag='T' ";
                    Str = Str + " Where Order_No = '" + Grid["Order_No", i].Value.ToString() + "' And Proj_Type_ID = " + Grid["Project_ID", i].Value.ToString() + " And Item_id = " + Grid["ManDays_ID", i].Value.ToString() + "";
                    Str = Str + " Group By Order_No, Proj_Type_ID, Item_ID)T1 Group By Order_No, Project_Id, Itemid ";
                }
                    Queries = new String[Dt.Rows.Count * 700];
                Total_Count();

                if (MyParent._New)
                {
                    //TxtENo.Text = MyBase.MaxOnlyComp("Floor_Testing_Po_Master", "Entry_No", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();
                    Queries[Array_Index++] = "Insert into ManDays_Entry_Master (Eno, EDate, Supplier_ID, Etime, Entry_System, Approved, Approved_System, Approved_Time, Remarks, Entry_Value, Company_Code) Values ('" + TxtENo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtSupplier.Tag.ToString() + ", getdate(), Host_Name(), 'N',  NULL, NULL,  '" + TxtRemarks.Text.ToString() + "', " + TxtTotAmnt.Text + ", " + MyParent.CompCode + "); Select Scope_Identity ()";
                    
                }
                else
                {
                    Queries[Array_Index++] = "Update ManDays_Entry_Master Set Supplier_id = " + TxtSupplier.Tag.ToString() + ", Remarks = '" + TxtRemarks.Text + "',  Entry_Value =  " + TxtTotAmnt.Text + " Where RowID = " + Code;

                    Queries[Array_Index++] = "Delete From Mandays_Entry_Details Where Master_ID = " + Code;
                    //Queries[Array_Index++] = "Delete From Floor_Testing_Po_Detail_Order Where Master_ID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert Into Mandays_Entry_Details (Master_Id, Project_Id, Itemid, Amount, Order_No) values (@@IDENTITY, " + Dt.Rows[i]["Project_Id"].ToString() + ", " + Dt.Rows[i]["ManDays_Id"].ToString() + ", " + Dt.Rows[i]["Value"].ToString() + ", '" + Dt.Rows[i]["Order_no"].ToString() + "')";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert Into Mandays_Entry_Details (Master_Id, Project_Id, Itemid, Amount, Order_No) values (" + Code + ", " + Dt.Rows[i]["Project_Id"].ToString() + ", " + Dt.Rows[i]["ManDays_Id"].ToString() + ", " + Dt.Rows[i]["Value"].ToString() + ", '" + Dt.Rows[i]["Order_no"].ToString() + "')";
                    }
                }                

                if (MyParent._New)
                {
                    MyBase.Run_Identity(false, Queries);
                }
                else
                {
                    MyBase.Run_Identity(true, Queries);
                }

                MyParent.Save_Error = false;
                MessageBox.Show("Saved ..!", "Gainup");
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }


        //public Double SumofGridTotal()
        //{
        //    double Sum = 0;
        //    try
        //    {

        //        foreach (DataGridView row in GridQty.Rows)
        //        {
        //            Sum += Convert.ToDouble(row.["TVal"].Value);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    return Sum;
        //}

        public void Entry_Print()
        {
            try
            {
                //String Str, Str1, Str2, Str3, Str4;
                //String Order = "";
                //DataTable Dt1 = new DataTable();
                //DataTable Dt2 = new DataTable();
                //DataTable Dt3 = new DataTable();
                //DataTable Dt4 = new DataTable();

                //Str = " Select S1.Entry_No PONo, L1.Supplier, '' Bill_No, Cast(S1.EDate As date)PoDate, Cast(S1.EDate As date) Required_Date, '' Yarn_Supplier, '' Ref_No, Cast(Getdate() as Datetime)Ref_Date, 'Testing_Po' PO_Method, Address1 + ' , ' + City Supplier_Address, L1.Phone Supplier_Phone, L1.e_mail Supplier_Email From Floor_Testing_Po_Master S1 Left Join FITERP1314.Dbo.Supplier L1 on S1.Supplierid = L1.Supplierid Left Join FITERP1314.Dbo.City L2 on L1.Cityid = L2.Cityid  Where S1.RowID = " + Code;
                //MyBase.Load_Data(Str, ref Dt1);

                //Str1 = " Select Top 100000 S2.SlNo, Upper(Testing  +' '+ Item +' '+ Color +' '+ Size) Item_Color_Size, S2.Qty Order_Qty, 0 Rate, 0 Amount, 0 Plus_Or_minus, 0 Net_Amnt, S2.Remarks  Item_Remarks, S1.Remarks, L.Abbreviation Uom  From Floor_Testing_Po_Master S1 Inner join Floor_Testing_Po_Detail S2 ON S1.RowID = S2.Master_ID Left Join Fiterp1314.Dbo.Item I on S2.Itemid = I.Itemid Left Join Fiterp1314.Dbo.Color J on S2.Colorid = J.Colorid Left Join Fiterp1314.Dbo.Size K on S2.Sizeid = K.Sizeid Left Join Fiterp1314.Dbo.Unit_of_measurement L on S2.UomId = L.uomid Where S1.RowID = " + Code + " Order By S2.SlNo ";
                //MyBase.Execute_Qry(Str1, "Garments_Testing_Po");


                //Str3 = " Select Distinct Order_No From Floor_Testing_Po_Master S1 Inner join Floor_Testing_Po_Detail S2 ON S1.RowID = S2.Master_ID Where S1.RowID = " + Code;
                //MyBase.Load_Data(Str3, ref Dt3);

                //Str4 = " Select Getdate()PrintOutDate";
                //MyBase.Load_Data(Str4, ref Dt4);

                //if (Dt3.Rows.Count > 0)
                //{
                //    for (int i = 0; i <= Dt3.Rows.Count - 1; i++)
                //    {
                //        if (Order.ToString() == String.Empty)
                //        {
                //            Order = Dt3.Rows[i]["Order_No"].ToString();
                //        }
                //        else
                //        {
                //            Order = Order + ", " + Dt3.Rows[i]["Order_No"].ToString();
                //        }
                //    }
                //}

                //CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RptTestingPo.rpt");
                //MyParent.FormulaFill(ref ObjRpt, "Heading", "TESTING PO");

                //MyParent.FormulaFill(ref ObjRpt, "Supplier", Dt1.Rows[0]["Supplier"].ToString());
                //MyParent.FormulaFill(ref ObjRpt, "Yarn_Supplier", Dt1.Rows[0]["Yarn_Supplier"].ToString());
                //MyParent.FormulaFill(ref ObjRpt, "Supplier_Address", Dt1.Rows[0]["Supplier_Address"].ToString().Replace("\r\n", "__"));
                //MyParent.FormulaFill(ref ObjRpt, "Supplier_Phone", Dt1.Rows[0]["Supplier_Phone"].ToString());
                //MyParent.FormulaFill(ref ObjRpt, "Supplier_Email", Dt1.Rows[0]["Supplier_Email"].ToString());
                //MyParent.FormulaFill(ref ObjRpt, "PONo", Dt1.Rows[0]["PONo"].ToString());
                //MyParent.FormulaFill(ref ObjRpt, "BillNo", Dt1.Rows[0]["Bill_No"].ToString());
                //MyParent.FormulaFill(ref ObjRpt, "Ref_No", Dt1.Rows[0]["Ref_No"].ToString());
                //MyParent.FormulaFill(ref ObjRpt, "PoDate", String.Format("{0:dd-MM-yyyy}", Dt1.Rows[0]["PoDate"]));
                //MyParent.FormulaFill(ref ObjRpt, "Ref_Date", String.Format("{0:dd-MMM-yyyy}", Dt1.Rows[0]["Ref_Date"].ToString()));
                //MyParent.FormulaFill(ref ObjRpt, "ReqDate", String.Format("{0:dd-MM-yyyy}", Dt1.Rows[0]["Required_Date"]));
                //MyParent.FormulaFill(ref ObjRpt, "PO_Method", Dt1.Rows[0]["PO_Method"].ToString());
                //MyParent.FormulaFill(ref ObjRpt, "PrintOutDate", Dt4.Rows[0]["PrintOutDate"].ToString());
                ////MyParent.FormulaFill(ref ObjRpt, "Net_Amount", TxtTotAmnt.Text.ToString());
                ////MyParent.FormulaFill(ref ObjRpt, "Net_Amount_Word", MyBase.Rupee(Convert.ToDouble(TxtTotAmnt.Text.ToString())));
                //MyParent.FormulaFill(ref ObjRpt, "Order", Order.ToString());
                //MyParent.CReport(ref ObjRpt, "TESTING PO..!");
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
                // MyBase.Enable_Controls(this, false);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Mandays Entry - Delete", "Select S1.Eno, S1.Edate, Order_No, S3.Name, S4.Item, S5.Ledger_Name Party, Sum(Amount)Amount, S1.Rowid, S1.Supplier_Id, S1.Remarks from ManDays_Entry_Master S1 Left Join Mandays_Entry_Details S2 on S1.Rowid = S2.Master_Id Left Join Project_Name_Master S3 on S2.Project_Id = S3.Rowid left Join Item S4 on S2.Itemid = S4.ItemID Left Join Supplier_all_Fn_Co1()S5 on   S1.Company_Code  = S5.Company_Code and S1.Supplier_ID = S5.LedgeR_Code Where Isnull(S1.Approved,'N')='N' Group By S1.Eno, S1.Edate, Order_No, S3.Name, S4.Item, S5.Ledger_Name, S1.Rowid, S1.Remarks, Supplier_ID ", String.Empty, 100, 100, 110, 175, 120, 200, 150);
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
                if (Code > 0)
                {
                    MyBase.Run("Delete From ManDays_Entry_Details Where Master_ID = " + Code + "", "Delete From ManDays_Entry_Master Where RowID = " + Code + " ");
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
                try
                {
                    MyBase.Clear(this);
                    //MyBase.Enable_Controls(this, false);
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Mandays Entry - View", "Select S1.Eno, S1.Edate, Order_No, S3.Name, S4.Item, S5.Ledger_Name Party, Sum(Amount)Amount, S1.Rowid, S1.Supplier_Id, S1.Remarks from ManDays_Entry_Master S1 Left Join Mandays_Entry_Details S2 on S1.Rowid = S2.Master_Id Left Join Project_Name_Master S3 on S2.Project_Id = S3.Rowid left Join Item S4 on S2.Itemid = S4.ItemID Left Join Supplier_all_Fn_Co1()S5 on   S1.Company_Code  = S5.Company_Code and S1.Supplier_ID = S5.LedgeR_Code Group By S1.Eno, S1.Edate, Order_No, S3.Name, S4.Item, S5.Ledger_Name, S1.Rowid, S1.Remarks, Supplier_ID ", String.Empty, 100, 100, 110, 175, 120, 200, 150);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Grid_Data()
        {
            String Str = String.Empty;
            try
            {
                if (MyParent._New == true)
                {
                    Str = "Select Distinct 0 SlNo, '' Project, '' Order_No,  '' Description, 0.00 Value, 0.00 Plan_Value, 0 Project_Id, 0 ManDays_Id From Project_Name_Master Where 1 = 2 ";
                }
                else
                {
                    Str = "Select Distinct ROW_NUMBER() Over(Order By S2.Rowid) SlNo, S3.Name Project, S2.Order_No,  S4.Item Description, S2.Amount Value, S2.Amount Plan_Value, S2.Project_Id, S2.Itemid ManDays_Id From Mandays_Entry_Master S1 left Join Mandays_Entry_Details S2 on S1.Rowid = S2.Master_Id left Join Project_Name_Master S3 on S2.Project_Id = S3.Rowid left Join Item S4 on S2.Itemid = S4.Itemid Where S1.Rowid  = " + Code + " Order By 1";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid, "Project", "Value");
                MyBase.Grid_Designing(ref Grid, ref Dt, "Project_Id", "ManDays_Id", "Plan_Value");
                MyBase.Grid_Width(ref Grid, 80, 200, 120, 170, 135);
                Grid.Columns["SlNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Project"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Order_No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Description"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                                
                Grid.Columns["Value"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["Plan_Value"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                             

            }
            catch (Exception ex)
            {
                throw ex;
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
                    Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);
                    Txt.Leave += new EventHandler(Txt_Leave);
                    Txt.TextChanged += new EventHandler(Txt_TextChanged);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index)
                //{
                //    if (Txt.Text == String.Empty)
                //    {
                //        Txt.Text = "0";
                //    }
                //    if (Grid["Qty", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Txt.Text != String.Empty)
                //    {
                //        Grid["Amount", Grid.CurrentCell.RowIndex].Value = Convert.ToString(Convert.ToDouble(Grid["Qty", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Txt.Text.ToString()));
                //    }

                //    if (Grid["Tax_Per", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Tax_Per", Grid.CurrentCell.RowIndex].Value.ToString()) <= 0)
                //    {
                //        Grid["Net_Amnt", Grid.CurrentCell.RowIndex].Value = Convert.ToString(Math.Round(Convert.ToDouble(Grid["Amount", Grid.CurrentCell.RowIndex].Value), 2));
                //    }
                //    else
                //    {
                //        Grid["Net_Amnt", Grid.CurrentCell.RowIndex].Value = Convert.ToString(Math.Round((Convert.ToDouble(Grid["Amount", Grid.CurrentCell.RowIndex].Value) + ((Convert.ToDouble(Grid["Amount", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["Tax_Per", Grid.CurrentCell.RowIndex].Value.ToString())) / 100)), 2));
                //    }
                //}
                //else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Qty"].Index)
                //{
                //    if (Txt.Text == String.Empty)
                //    {
                //        Txt.Text = "0";
                //    }
                //    if (Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Txt.Text != String.Empty)
                //    {
                //        Grid["Amount", Grid.CurrentCell.RowIndex].Value = Convert.ToString(Convert.ToDouble(Grid["Rate", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Txt.Text.ToString()));
                //    }
                //    if (Grid["Tax_Per", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Tax_Per", Grid.CurrentCell.RowIndex].Value.ToString()) <= 0)
                //    {
                //        if (Grid["Amount", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                //        {
                //            Grid["Net_Amnt", Grid.CurrentCell.RowIndex].Value = Convert.ToString(Math.Round(Convert.ToDouble(Grid["Amount", Grid.CurrentCell.RowIndex].Value), 2));
                //        }
                //    }
                //    else
                //    {
                //        Grid["Net_Amnt", Grid.CurrentCell.RowIndex].Value = Convert.ToString(Math.Round((Convert.ToDouble(Grid["Amount", Grid.CurrentCell.RowIndex].Value) + ((Convert.ToDouble(Grid["Amount", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["Tax_Per", Grid.CurrentCell.RowIndex].Value.ToString())) / 100)), 2));
                //    }
                //}
                //else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Tax_Per"].Index)
                //{
                //    if (Txt.Text == String.Empty)
                //    {
                //        Txt.Text = "0";
                //    }
                //    if (Grid["Qty", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Grid["Rate", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty && Txt.Text != String.Empty)
                //    {
                //        if (Txt.Text == "0")
                //        {
                //            Grid["Net_Amnt", Grid.CurrentCell.RowIndex].Value = Convert.ToString(Math.Round(Convert.ToDouble(Grid["Amount", Grid.CurrentCell.RowIndex].Value), 2));
                //        }
                //        else
                //        {
                //            Grid["Net_Amnt", Grid.CurrentCell.RowIndex].Value = Convert.ToString(Math.Round((Convert.ToDouble(Grid["Amount", Grid.CurrentCell.RowIndex].Value) + ((Convert.ToDouble(Grid["Amount", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Txt.Text)) / 100)), 2));
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Value"].Index)
                {
                    if (Txt.Text == String.Empty)
                    {
                        Txt.Text = "0";
                    }
                    else
                    {
                        if (MyParent._New)
                        {
                            if (Convert.ToDouble(Grid["Value", Grid.CurrentCell.RowIndex].Value.ToString()) > Convert.ToDouble(Grid["Plan_Value", Grid.CurrentCell.RowIndex].Value.ToString()))
                            {
                                MessageBox.Show("Invalid Amount", "Gainup");
                                Grid["Value", Grid.CurrentCell.RowIndex].Value = Convert.ToString(Math.Round((Convert.ToDouble(Grid["Plan_Value", Grid.CurrentCell.RowIndex].Value)), 2));
                                return;
                            }
                        }
                    }
                    Total_Count();
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Project"].Index)
                    {
                        //if (Grid["Project", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        //{
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select ManDays", "Select Project, T1.Order_No, T1.Description, T1.Value-Isnull(T2.Amount,0)Pend_Value, T1.Value, T1.Project_Id, ManDays_Id From Mandays_Cost_Plan_Fn()T1 Left Join (Select Order_No, Project_Id, Itemid, Sum(Amount)Amount from ManDays_Entry_Master S1 Left Join Mandays_Entry_Details S2 on S1.Rowid = S2.Master_Id Group By Order_No, Project_Id, Itemid)T2 on T1.Order_No = T2.Order_No And T1.Project_Id = T2.Project_Id And T1.ManDays_Id = T2.Itemid ", String.Empty, 165, 110, 120, 120);
                            
                            if (Dr != null)
                            {
                                Txt.Text = Dr["Project"].ToString();
                                Grid["Project", Grid.CurrentCell.RowIndex].Value = Dr["Project"].ToString();
                                Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                                Grid["Description", Grid.CurrentCell.RowIndex].Value = Dr["Description"].ToString();                                                                                                
                                Grid["Value", Grid.CurrentCell.RowIndex].Value = Dr["Pend_Value"].ToString();
                                Grid["Plan_Value", Grid.CurrentCell.RowIndex].Value = Dr["Pend_Value"].ToString();
                                Grid["Project_Id", Grid.CurrentCell.RowIndex].Value = Dr["Project_Id"].ToString();
                                Grid["ManDays_Id", Grid.CurrentCell.RowIndex].Value = Dr["ManDays_Id"].ToString();
                            }
                        //}
                    }                   
                    Total_Count();
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
        void Eno_Generate()
        {
            try
            {
                if (MyParent._New)
                {
                    DataTable Tdt = new DataTable();
                    MyBase.Load_Data("Select DBo.Get_Max_Garments_Testing_Po ('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "')", ref Tdt);
                    TxtENo.Text = Tdt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                TxtENo.Text = String.Empty;
                throw ex;
            }
        }
        void Total_Count()
        {
            try
            {
                string A;
                A = Convert.ToString(MyBase.Sum(ref Grid, "Value"));                
                TxtTotAmnt.Text = Convert.ToString(Convert.ToDouble(A.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Value"].Index)
                {
                    MyBase.Valid_Decimal((TextBox)Txt, e);
                }                
                else
                {
                    MyBase.Valid_Null(Txt, e);
                }
                Total_Count();
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
                    Total_Count();
                    TxtTotAmnt.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyCode == Keys.Enter)
                //{
                //    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Rate"].Index)
                //    {
                //        Grid["Amount", Grid.CurrentCell.RowIndex].Value = Convert.ToString(Convert.ToDouble(Grid["Qty", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["Rate", Grid.CurrentCell.RowIndex].Value));
                //    }                    
                //}
                //Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmTestingPo_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                DtpDate.Enabled = false;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmTestingPo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl.Name == "TxtRemarks")
            {
                e.Handled = false;
            }
            //else if (this.ActiveControl.Name == "TxtPlusOrMinus")
            //{
            //    MyBase.Valid_DecimalPlusMinus((TextBox)this.ActiveControl, e);
            //}
            else if (this.ActiveControl is TextBox)
            {
                if (this.ActiveControl.Name != String.Empty)
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
            }
        }

        private void FrmTestingPo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtTotAmnt")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }                    
                    else if (this.ActiveControl.Name == "TxtSupplier")
                    {

                        Grid.CurrentCell = Grid["Project", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                        if (this.ActiveControl.Name != "TxtRemarks")
                        {
                            SendKeys.Send("{Tab}");
                        }
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (MyParent._New == true || MyParent.Edit == true)
                    {

                        if (this.ActiveControl.Name == "TxtSupplier")
                        {

                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier", " select Ledger_Name Supplier, Ledger_Code Supplierid from Supplier_all_Fn_Co1() Where Company_Code = " + MyParent.CompCode + "  Order By 1 ", String.Empty, 400);

                            if (Dr != null)
                            {
                                TxtSupplier.Text = Dr["Supplier"].ToString();
                                TxtSupplier.Tag = Dr["Supplierid"].ToString();                                
                                Grid_Data();
                            }

                        }
                        
                    }
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
                //else if (e.KeyCode == Keys.Escape)
                //{
                //    if (GBProc.Visible == true)
                //    {
                //        //GBMain.Enabled = false;
                //    }
                //    else
                //    {
                //        MyBase.ActiveForm_Close(this, MyParent);
                //    }
                //}
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
                if (Grid.Rows.Count > 1)
                {
                    MyBase.Row_Number(ref Grid);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }
        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        DtQty[Grid.CurrentCell.RowIndex] = null;
                        ReArrange_Datatable_Array();
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        Boolean ReArrange_Datatable_Array()
        {
            Boolean IsAllNull = true;
            try
            {
                if (IsAllNullInDatatableArray())
                {
                    return true;
                }
                else
                {
                    for (int i = Grid.CurrentCell.RowIndex; i <= 300 - 2; i++)
                    {
                        if (DtQty[i] == null && DtQty[i + 1] != null)
                        {
                            DtQty[i] = DtQty[i + 1].Copy();
                            DtQty[i + 1] = null;
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        Boolean IsAllNullInDatatableArray()
        {
            try
            {
                for (int i = Grid.CurrentCell.RowIndex + 1; i <= 300 - 1; i++)
                {
                    if (DtQty[Grid.CurrentCell.RowIndex + 1] != null)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        private void DtpEDate_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(DtpDate.Value) > MyBase.GetServerDateTime())
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DtpDate.Value = MyBase.GetServerDateTime();
                    DtpDate.Focus();
                    return;
                }
                if (MyParent.UserCode != 1 && MyParent.UserCode != 81)
                {
                    if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(DtpDate.Value), MyBase.GetServerDateTime()) > 3)
                    {
                        MessageBox.Show("Invalid Date, No Rights", "Gainup");
                        DtpDate.Value = MyBase.GetServerDateTime();
                        DtpDate.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DtpEDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (MyParent._New)
                {
                    Grid.DataSource = null;
                    TxtSupplier.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
