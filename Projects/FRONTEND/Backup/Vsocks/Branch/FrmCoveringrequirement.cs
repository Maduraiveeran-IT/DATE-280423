using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using Accounts;
using System.IO;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmCoveringrequirement : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable[] DtQty;
        DataRow Dr;
        Int64 Code;
        Int64 i;
        TextBox Txt = null;
        TextBox Txt1 = null;
        String[] Queries;
        String Str;
        String Buffer_Table = String.Empty;
        Boolean Buffer_Update = false;
        Int64 Mode = 0;
        Int64 ItemID = 0;
        Int64 ColorID = 0;
        Int64 SizeID = 0;
        public FrmCoveringrequirement()
        {
            InitializeComponent();
        }

        private void FrmCoveringrequirement_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                TxtOrder.Focus();
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
                TxtOrder.Focus();
                Grid_Data();
                DtQty = new DataTable[30];
                return;
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
                MyBase.Clear(this);
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Covering Requirement Entry - Edit", "Select B.Eno, Cast(B.Edate as Date)Date, B.Order_No, C.Item Yarn, D.Color, E.Size Count, A.Qty, A.Itemid, A.Colorid, A.Sizeid, B.Rowid, B.Remarks  from fitsocks.dbo.Covering_Req_Details A  Left Join fitsocks.dbo.Covering_Req_Mas B on A.Master_ID = B.Rowid  Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid  Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid  Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid  Left Join Fitsocks.Dbo.Pur_Ind_Mas F on B.ENo = F.Covering_Entry_No Where F.Approved='N' ", String.Empty, 80, 100, 100, 150, 150, 150, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Yarn", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Cancel()
        {
            MyBase.Clear(this);
        }
        public void Entry_Print()
        {
            try
            {
                //MyParent.View_Browser("MIS_SOCKS_YARNDYEING_DC", Code);

                //Str = "Select C1.companyid, C1.company, C1.address1 Comp_Address1, C1.Address2 Comp_Address2, C1.City Comp_City, C1.TinNo Comp_Tin, C1.cst_no Comp_Cst_No, C1.Cst_Date Comp_Cst_Date,";
                //Str = Str + " S1.Type, S1.RowID Supplier_ROdid, S1.ENo, S1.Date, S1.Supplierid, S1.Supplier, S1.Dc_No, S1.Dc_Date, S1.address1 Supplier_Address1, S1.Address2 Supplier_Address2, S1.address3 Supplier_Address3, S1.City Supplier_City,";
                //Str = Str + " D1.Rowid, D1.itemid, D1.item, D1.Colorid, D1.Color, D1.Sizeid, D1.SIze, D1.Ord_Qty, D1.Iss_Qty, D1.remarks";
                //Str = Str + " from [FITSOCKS].dbo.Supplier_Details_Yarn_Dyeing() S1 Left Join [FITSOCKS].Dbo.Dyeing_Issued_For_Dc() D1 On S1.Rowid = D1.Rowid ";
                //Str = Str + " Left Join [FITSOCKS].dbo.Company_Details() C1 On 1 =1 Where S1.Rowid = " + Code + " And S1.Type = 'Delivery' ";

                //MyBase.Execute_Qry(Str, "Yarn_Dyeing_DC");
                //CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Yarn_Dyeing_Delivery.rpt");
                //MyParent.CReport(ref ObjRpt, "Covering Requirement Delivery..!");

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
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Covering Requirement Entry - View", "Select B.Eno, Cast(B.Edate as Date)Date, B.Order_No, C.Item Yarn, D.Color, E.Size Count, A.Qty, A.Itemid, A.Colorid, A.Sizeid, B.Rowid, B.Remarks  from fitsocks.dbo.Covering_Req_Details A  Left Join fitsocks.dbo.Covering_Req_Mas B on A.Master_ID = B.Rowid  Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid  Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid  Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid  ", String.Empty, 80, 100, 100, 150, 150, 150, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    Grid.CurrentCell = Grid["Yarn", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
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
                Int32 Array_Index = 0;
                Int64 Code1;
                String From_Store = String.Empty;
                Total_Count();

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Details..!", "Gainup");
                    Grid.CurrentCell = Grid["Yarn", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtTotal.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotal.Text) == 0)
                {
                    MessageBox.Show("Invalid Details", "Gainup");
                    Grid.CurrentCell = Grid["Yarn", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (DtQty[Convert.ToInt32(Dt.Rows[i]["SlNo1"])] == null)
                    {
                        MessageBox.Show("Invalid Raw Material Details ...!", "Gainup");
                        MyParent.Save_Error = true;
                        Grid.CurrentCell = Grid["Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                }  

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {

                    if (Grid["Qty", i].Value == DBNull.Value || Grid["Qty", i].Value.ToString() == String.Empty || Convert.ToInt64(Grid["Qty", i].Value) == 0)
                    {
                        MessageBox.Show(" ZERO is Invalid in Row " + (i + 1) + "  ", "Gainup");
                        Grid.CurrentCell = Grid["Qty", i];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }
                    

                }
                if (MyParent._New)
                {
                    TxtEntryNo.Text = MyBase.MaxOnlyComp("Covering_Req_Mas", "ENo", String.Empty, MyParent.YearCode, MyParent.CompCode).ToString();
                }
                else
                {
                    TxtEntryNo.Text = " Select Eno from Covering_Req_Mas Where Rowid = " + Code;
                }
                Queries = new string[Dt.Rows.Count * 100];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Covering_Req_Mas (ENo, EDate, Remarks, Company_Code, Year_Code,User_Code,Order_No) values (" + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + TxtRemarks.Text + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "'," + MyParent.UserCode + ", '" + TxtOrder.Text.ToString() + "'); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("Covering Requirement Entry", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Covering_Req_Mas Set EDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',  Remarks = '" + TxtRemarks.Text + "',Company_Code=" + MyParent.CompCode + " , Year_Code='" + MyParent.YearCode + "',User_Code=" + MyParent.UserCode + " Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Covering Requirement Entry", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete from Covering_Rawmaterial_Details where Master_ID = " + Code;   
                    Queries[Array_Index++] = "Delete from Covering_Req_Details where Master_ID = " + Code;                    
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Covering_Req_Details (Master_ID, Slno, ItemID, SizeID, ColorID, Qty, Rate, Value, Slno1) Values (@@IDENTITY, " + Grid["Slno", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ", " + Grid["qty", i].Value + ", " + Grid["Rate", i].Value + ", " + Grid["Value", i].Value + ", " + Grid["Slno", i].Value + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Covering_Req_Details (Master_ID, Slno, ItemID, SizeID, ColorID, Qty, Rate, Value, Slno1) Values (" + Code + ", " + Grid["Slno", i].Value + ", " + Grid["ItemID", i].Value + ", " + Grid["SizeID", i].Value + " , " + Grid["ColorID", i].Value + ", " + Grid["qty", i].Value + ", " + Grid["Rate", i].Value + ", " + Grid["Value", i].Value + ", " + Grid["Slno", i].Value + ")";
                    }
                }
                for (int i = 0; i <= Dt.Rows.Count-1; i++)
                {
                    for (i = 0; i <= DtQty.Length - 1; i++)
                    {
                        if (DtQty[i] != null)
                        {
                            for (int j = 0; j <= DtQty[i].Rows.Count - 1; j++)
                            {
                                if (MyParent._New)
                                {
                                    Queries[Array_Index++] = "Insert Into Covering_Rawmaterial_Details (slno, Master_ID, ItemID, SizeID, ColorID, Qty, Rate, Value, SlNo1, Perc) Values ( " + DtQty[i].Rows[j]["Sno"].ToString() + ",@@IDENTITY,   " + DtQty[i].Rows[j]["Itemid"].ToString() + "," + DtQty[i].Rows[j]["Sizeid"].ToString() + "," + DtQty[i].Rows[j]["Colorid"].ToString() + "," + DtQty[i].Rows[j]["Qty"].ToString() + "," + DtQty[i].Rows[j]["Rate"].ToString() + "," + DtQty[i].Rows[j]["Value"].ToString() + ", " + Dt.Rows[i - 1]["Slno1"].ToString() + "," + DtQty[i].Rows[j]["Perc"].ToString() + ")";
                                }
                                else
                                {
                                    Queries[Array_Index++] = "Insert Into Covering_Rawmaterial_Details (slno, Master_ID, ItemID, SizeID, ColorID, Qty, Rate, Value, SlNo1, Perc) Values ( " + DtQty[i].Rows[j]["Sno"].ToString() + "," + Code + ", " + DtQty[i].Rows[j]["Itemid"].ToString() + ", " + DtQty[i].Rows[j]["Sizeid"].ToString() + "," + DtQty[i].Rows[j]["Colorid"].ToString() + "," + DtQty[i].Rows[j]["Qty"].ToString() + "," + DtQty[i].Rows[j]["Rate"].ToString() + "," + DtQty[i].Rows[j]["Value"].ToString() + ", " + DtQty[i].Rows[j]["Slno1"].ToString() + "," + DtQty[i].Rows[j]["Perc"].ToString() + ")";
                                }
                            }
                        }
                    }
                } 

                if (MyParent._New)
                {                   
                    Str = "Insert into Fitsocks.Dbo.Pur_Ind_Mas (IndentNo, Indent_Date, Request_No, Request_Date, CompanyId, CompanyUnitId, Remarks, Req_Close, IndentBy, Approved, ApprovedBY, Covering_Entry_No) ";
                    Str = Str + " Select 'GUP-CIN' + REPLICATE('0',5-LEN(RTRIM(Eno))) + RTRIM(Eno), Cast(Edate as Date), Eno, Cast(Edate as Date), 93, 73, Remarks, 'N', 97, 'N', 0, Eno from Fitsocks.Dbo.Covering_Req_Mas Where ENo = " + TxtEntryNo.Text + "; Select Scope_Identity() ";
                    Queries[Array_Index++] = Str;
                
                    DataTable TDt = new DataTable();
                    Str = "Select IDENT_CURRENT('Pur_Ind_Mas')+1";
                    MyBase.Load_Data(Str, ref TDt);

                    Str = "Insert Into Fitsocks.Dbo.Pur_Ind_Det(IndentID, ItemId, ColorId, SizeId, UOMId, Request_Date, Quantity, Approved_Qty, Ordered_Qty, Received_Qty, ItemRemarks, Rate, Value, Cov_Raw_MasID, Cov_Raw_RowID)";
                    Str = Str + "Select " + TDt.Rows[0][0].ToString() + " , A.Itemid, A.Colorid, A.Sizeid, 55, B.EDate, Isnull(A.Qty,0), NULL, NULL, NULL, '', A.Rate, A.Value, A.Master_Id, A.Rowid from Covering_Rawmaterial_Details A ";                    
                    Str = Str + "Left Join Covering_Req_Mas B on A.Master_ID = B.RowID  Where B.Eno= " + TxtEntryNo.Text + " ";                    
                    Queries[Array_Index++] = Str;
                    
                }
                else
                {
                    if (MyParent.Edit == true)
                    {
                        Str = "Delete from Fitsocks.Dbo.Pur_Ind_Det Where Indentid in(Select Indentid from Fitsocks.Dbo.Pur_Ind_Mas Where Covering_Entry_No in( " + TxtEntryNo.Text + "))";
                        Queries[Array_Index++] = Str;

                        Str = "Select Indentid From fitsocks.dbo.Pur_Ind_Mas Where Covering_Entry_No in( " + TxtEntryNo.Text + ")";
                        DataTable TDt1 = new DataTable();
                        MyBase.Load_Data(Str, ref TDt1);
                        Code1 = Convert.ToInt64(TDt1.Rows[0][0].ToString());

                        Str = "Insert Into Fitsocks.Dbo.Pur_Ind_Det(IndentID, ItemId, ColorId, SizeId, UOMId, Request_Date, Quantity, Approved_Qty, Ordered_Qty, Received_Qty, ItemRemarks, Rate, Value, Cov_Raw_MasID, Cov_Raw_RowID)";
                        Str = Str + "Select " + Code1 + " , A.Itemid, A.Colorid, A.Sizeid, 55, B.EDate, Isnull(A.Qty,0), NULL, NULL, NULL, '', A.Rate, A.Value, A.Master_Id, A.Rowid from Covering_Rawmaterial_Details A ";                                            
                        Str = Str + "Left Join Fitsocks.Dbo.Covering_Req_Mas B on A.Master_ID = B.RowID  Where B.Eno in( " + TxtEntryNo.Text + ") ";                        
                        Queries[Array_Index++] = Str;

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

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                DtQty = new DataTable[30];
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Covering Requirement Entry - Delete", "Select B.Eno, Cast(B.Edate as Date)Date, B.Order_No, C.Item Yarn, D.Color, E.Size Count, A.Qty, A.Itemid, A.Colorid, A.Sizeid, B.Rowid, B.Remarks  from fitsocks.dbo.Covering_Req_Details A  Left Join fitsocks.dbo.Covering_Req_Mas B on A.Master_ID = B.Rowid  Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid  Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid  Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid  Left Join Fitsocks.Dbo.Pur_Ind_Mas F on B.ENo = F.Covering_Entry_No Where F.Approved='N' ", String.Empty, 80, 100, 100, 150, 150, 150, 100);
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
                    MyBase.Run("Delete from Fitsocks.Dbo.Pur_Ind_Det Where IndentID in(Select IndentID from Fitsocks.Dbo.Pur_Ind_Mas Where Covering_Entry_No = " + TxtEntryNo.Text + ")", "Delete from Fitsocks.Dbo.Pur_Ind_Mas Where Covering_Entry_No = " + TxtEntryNo.Text + "", "Delete from Covering_Rawmaterial_Details where Master_ID = " + Code, "Delete from Covering_Req_Details where Master_ID = " + Code, "Delete From Covering_Req_Mas Where RowID = " + Code, MyParent.EntryLog("Covering Requirement Entry", "DELETE", Code.ToString()));
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

        void Fill_Datas(DataRow Dr)
        {
            try
            {

                Code = Convert.ToInt64(Dr["RowID"]);
                TxtEntryNo.Text = Dr["ENo"].ToString();
                DtpDate.Value = Convert.ToDateTime(Dr["Date"]);
                TxtOrder.Text = Dr["Order_No"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();
                Grid_Data();
                Total_Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Grid_Data()
        {
            String Str = String.Empty;
            try
            {
                if (MyParent._New == true)
                {
                    Str = "select 0 as Slno, Item Yarn,  Color, Size Count, 0.0000 Qty, 0.000 Rate, 0.000 Value, Itemid, Colorid, Sizeid, 0 Slno1, 0 RNo,'-' T  from FITSOCKS.dbo.Yarn_Dyeing_Requirement_Details() where 1=2 Group By Itemid, Item, Colorid, Color, Sizeid, Size";
                }
                else
                {
                    Str = "Select A.Slno, C.Item Yarn, D.Color, E.Size Count, A.Qty, A.Rate, A.Value, A.Itemid, A.Colorid, A.Sizeid, A.Slno1, ROW_NUMBER() Over (Order by A.Itemid, A.Colorid, A.Sizeid) RNo,'-' T  from fitsocks.dbo.Covering_Req_Details A  Left Join fitsocks.dbo.Covering_Req_Mas B on A.Master_ID = B.Rowid Left Join fitsocks.dbo.Item C on A.Itemid = C.Itemid Left Join fitsocks.dbo.Color D on A.COlorid = D.Colorid Left Join fitsocks.dbo.Size E on A.Sizeid = E.Sizeid Where B.Eno = '" + TxtEntryNo.Text + "' Order By A.Slno ";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Designing(ref Grid, ref Dt, "ItemID", "SizeID", "ColorID", "RNo", "Slno1", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Yarn", "Qty");
                MyBase.Grid_Width(ref Grid, 50, 130, 150, 110, 100, 100, 120);
                Grid.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                
            }
            catch (Exception ex)
            {
                throw ex;
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

        private void Grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid);
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                    }
                }
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
                    Txt.Enter += new EventHandler(Txt_Enter);
                    Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                    Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void Txt_Enter(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Yarn"].Index)
                {
                    MyBase.Row_Number(ref Grid);
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Yarn"].Index)
                    {
                        if (TxtOrder.Text != String.Empty)
                        {
                            Dr = Tool.Selection_Tool_Except_New("RNo", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Item", "Select B.Item, C.Color, D.Size, A.quantity, Cast(A.Rate as Numeric(25,3))Rate, Cast((A.quantity*A.Rate)as Numeric(25,3))Value, A.Itemid, A.Colorid, A.Sizeid, ROW_NUMBER() Over (Order by A.Itemid, A.Colorid, A.Sizeid) RNo  from Pur_Ord_Det A Left Join Pur_Ord_mas A1 on A.Pur_ord_id = A1.pur_ord_id Left Join Item B on A.Itemid = B.Itemid Left Join Color C on A.Colorid = C.Colorid Left Join Size D on A.Sizeid = D.Sizeid Where A1.Approved='Y' And A1.Pur_Ord_No='" + TxtOrder.Text + "' Order By B.Item, C.Color, D.Size", String.Empty, 100, 150, 75, 100, 100, 110);                            

                            if (Dr != null)
                            {
                                Txt.Text = Dr["ITEM"].ToString();
                                Grid["Yarn", Grid.CurrentCell.RowIndex].Value = Dr["ITEM"].ToString();
                                Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                                Grid["Count", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                                Grid["Sizeid", Grid.CurrentCell.RowIndex].Value = Dr["Sizeid"].ToString();
                                Grid["Color", Grid.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                                Grid["Colorid", Grid.CurrentCell.RowIndex].Value = Dr["Colorid"].ToString();
                                Grid["Qty", Grid.CurrentCell.RowIndex].Value = Dr["Quantity"].ToString();
                                Grid["Rate", Grid.CurrentCell.RowIndex].Value = Dr["Rate"].ToString();
                                Grid["Value", Grid.CurrentCell.RowIndex].Value = Dr["Value"].ToString();
                                Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                                Grid["RNo", Grid.CurrentCell.RowIndex].Value = Dr["RNo"].ToString();
                            }
                        }
                        else
                        {
                            MessageBox.Show("First Select Po No", "Gainup");
                            TxtOrder.Focus();
                        }

                    }                    
                }
                Total_Count();
                
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
                        if (No < Convert.ToInt16(Dt.Rows[i]["Slno1"]))
                        {
                            No = Convert.ToInt16(Dt.Rows[i]["Slno1"]);
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

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Null(Txt, e);                
                Total_Count();
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
                //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Req_Qty"].Index)
                //{
                //    if ((Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) != String.Empty)
                //    {
                //        if (Convert.ToDouble(Grid["Req_Qty", Grid.CurrentCell.RowIndex].Value)==0.000)
                //        {
                //            MessageBox.Show("Invalid Req_Qty..!", "Gainup");
                //            Grid.CurrentCell = Grid["Req_Qty", Grid.CurrentCell.RowIndex];
                //            Grid.Focus();
                //            Grid.BeginEdit(true);
                //            MyParent.Save_Error = true;
                //            return;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Total_Count()
        {
            try
            {
                TxtTotal.Text = MyBase.Sum_With_Four_Digits(ref Grid, "Qty");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmCoveringrequirement_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;

                    if (this.ActiveControl.Name == "TxtOrder")
                    {
                        if (TxtOrder.Text.ToString() == String.Empty)
                        {
                            MessageBox.Show("Please Select Po No..!", "Gainup");
                            return;
                        }
                        else
                        {
                            Grid.CurrentCell = Grid["Yarn", 0];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtTotal")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
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
                    if (this.ActiveControl.Name == "TxtOrder")
                    {

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Po No..!", "Select Pur_Ord_No Order_No From fitsocks.dbo.Pur_Ord_Mas where supplierid=685 And Pur_Ord_No not in(Select Distinct Order_No From Covering_Req_Mas) Order By Pur_Ord_No Desc", String.Empty, 150);

                        if (Dr != null)
                        {
                            TxtOrder.Text = Dr["Order_No"].ToString();                            
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
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {

                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Qty"].Index)
                    {

                        TxtQty1.Text = String.Format("{0:0.0000}", Convert.ToString(Convert.ToDouble(Grid["Qty", Grid.CurrentCell.RowIndex].Value) + (Convert.ToDouble(Grid["Qty", Grid.CurrentCell.RowIndex].Value)*0.05)));

                        ItemID = Convert.ToInt64(Grid["ItemId", Grid.CurrentCell.RowIndex].Value);
                        ColorID = Convert.ToInt64(Grid["ColorId", Grid.CurrentCell.RowIndex].Value);
                        SizeID = Convert.ToInt64(Grid["SizeId", Grid.CurrentCell.RowIndex].Value);

                        GridDetail_Data(Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value), Convert.ToInt32(Grid["Qty", Grid.CurrentCell.RowIndex].Value), ItemID, ColorID, SizeID);
                        GridDetail.CurrentCell = GridDetail["Item", 0];
                        GridDetail.Focus();
                        GridDetail.BeginEdit(true);
                        e.Handled = true;
                        return;

                    }
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
                //if (e.KeyChar == Convert.ToChar(Keys.Escape))
                //{
                //    Total_Count();
                //    TxtRemarks.Focus();
                //    return;
                //}
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    TxtRemarks.Focus();
                    TxtRemarks.SelectAll();
                    SendKeys.Send("{End}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GridDetail_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {                
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    ButOk.Focus();
                    SendKeys.Send("{End}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void GridDetail_Data(Int32 Row, Int32 Qty, Int64 Item, Int64 Color, Int64 Size)
        {

            try
            {
                if (DtQty[Row] == null)
                {
                    DtQty[Row] = new DataTable();
                    if (MyParent._New)
                    {
                        MyBase.Load_Data("select 0 SNo, '' Item, '' Color, '' Size, 0 Perc, 0 All_Perc,  0.0000 Qty, 0.000 Rate, 0.000 Value,0 Itemid, 0 Sizeid, 0 Colorid, " + Row + " SlNo1, '' T from Yarn_Dyeing_Requirement_Details() where 1=2 ", ref DtQty[Row]);
                    }
                    else
                    {
                        MyBase.Load_Data("select A.slno Sno, D.Item, E.Color, F.Size, A.Perc, 5 All_Perc, Cast(A.Qty as Numeric(20,4)) Qty, A.Rate, A.Value, A.Itemid, A.Colorid, A.Sizeid, B.Slno1,'' T from Covering_Rawmaterial_Details A Left Join Covering_Req_Details B on A.Master_ID = B.Master_ID and A.SlNo1 = B.Slno1 Left Join Covering_Req_Mas C on A.Master_ID = C.RowID and B.Master_ID = C.RowID Left Join Item D on A.Itemid = D.itemid Left Join Color E on A.Colorid = E.Colorid Left Join Size F on A.Sizeid = F.Sizeid   Where  A.Master_ID =  " + Code + " and  B.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString(), ref DtQty[Row]);
                    }
                }
                GridDetail.DataSource = DtQty[Row];
                MyBase.Grid_Designing(ref GridDetail, ref DtQty[Row], "Itemid","Colorid","Sizeid", "All_Perc", "SlNo1", "T");
                MyBase.ReadOnly_Grid_Without(ref GridDetail, "Item", "Color", "Size", "Perc", "Rate");
                MyBase.Grid_Colouring(ref GridDetail, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridDetail, 50, 130, 150, 110, 65, 100, 100, 120);
                GridDetail.Columns["Perc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridDetail.Columns["All_Perc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridDetail.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridDetail.Columns["Rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                GridDetail.Columns["Value"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                
                GridDetail.RowHeadersWidth = 10;

                if (!MyParent._New)
                {
                    //Balance_Pieces();
                }

                GBQty.Visible = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void GridDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.KeyCode == Keys.Enter)
                {
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Perc"].Index)
                    {

                        GridDetail["Qty", GridDetail.CurrentCell.RowIndex].Value = String.Format("{0:0.0000}",(((Convert.ToDouble(Grid["Qty", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridDetail["Perc", GridDetail.CurrentCell.RowIndex].Value)) / 100) * 0.05) + ((Convert.ToDouble(Grid["Qty", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridDetail["Perc", GridDetail.CurrentCell.RowIndex].Value)) / 100));
                    }
                    else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Qty"].Index)
                    {
                        if (GridDetail["Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Qty", GridDetail.CurrentCell.RowIndex].Value) == 0)
                        {
                            e.Handled = true;
                            MessageBox.Show("Invalid Qty...!", "Gainup");
                            GridDetail.CurrentCell = GridDetail["Qty", Grid.CurrentCell.RowIndex];
                            GridDetail.Focus();
                            GridDetail.BeginEdit(true);
                            return;
                        }
                    }
                    else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Rate"].Index)
                    {
                        GridDetail["Value", GridDetail.CurrentCell.RowIndex].Value = Convert.ToDouble(GridDetail["Qty", GridDetail.CurrentCell.RowIndex].Value) * Convert.ToDouble(GridDetail["Rate", GridDetail.CurrentCell.RowIndex].Value); 
                    }
                }
                Iss_Balance();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void GridDetail_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (GridDetail.Rows.Count > 1)
                {
                    MyBase.Row_Number(ref GridDetail);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void GridDetail_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid);
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void GridDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref GridDetail, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], GridDetail.CurrentCell.RowIndex);
                DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].AcceptChanges();
                MyBase.Row_Number(ref GridDetail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }   
        private void GridDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt1 == null)
                {
                    Txt1 = (TextBox)e.Control;
                    Txt1.KeyPress += new KeyPressEventHandler(TxtIss_KeyPress);
                    Txt1.GotFocus += new EventHandler(TxtIss_GotFocus);
                    Txt1.KeyDown += new KeyEventHandler(TxtIss_KeyDown);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void TxtIss_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Item"].Index)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item ", "Select Item, Itemid From Fitsocks.Dbo.Item Order By Item", String.Empty, 150);
                        
                        if (Dr != null)
                        {
                            Txt1.Text = Dr["Item"].ToString();
                            GridDetail["Item", GridDetail.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                            GridDetail["Itemid", GridDetail.CurrentCell.RowIndex].Value = Dr["Itemid"].ToString();
                        }
                    }
                    else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Color"].Index)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color ", "Select Color, Colorid From Fitsocks.Dbo.Color Order By Color", String.Empty, 150);

                        if (Dr != null)
                        {
                            Txt1.Text = Dr["Color"].ToString();
                            GridDetail["Color", GridDetail.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                            GridDetail["Colorid", GridDetail.CurrentCell.RowIndex].Value = Dr["Colorid"].ToString();
                        }
                    }
                    if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Size"].Index)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size ", "Select Size, Sizeid From Fitsocks.Dbo.Size Order By Size", String.Empty, 150);

                        if (Dr != null)
                        {
                            Txt1.Text = Dr["Size"].ToString();
                            GridDetail["Size", GridDetail.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                            GridDetail["Sizeid", GridDetail.CurrentCell.RowIndex].Value = Dr["Sizeid"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void TxtIss_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Rate"].Index)
                {
                    MyBase.Valid_Decimal(Txt1, e);
                }
                else if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Perc"].Index)
                {
                    MyBase.Valid_Number(Txt1, e);

                }
                else
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void TxtIss_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (GridDetail.CurrentCell.ColumnIndex == GridDetail.Columns["Qty"].Index)
                {
                    if (GridDetail["Qty", GridDetail.CurrentCell.RowIndex].Value == null || GridDetail["Qty", GridDetail.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(GridDetail["Qty", GridDetail.CurrentCell.RowIndex].Value) == 0)
                    {

                        //GridDetail["Iss_Qty", GridDetail.CurrentCell.RowIndex].Value = Convert.ToDouble(TxtBalance.Text);
                    }
                }
                Iss_Balance();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Iss_Balance()
        {
            try
            {

                if (TxtQty1.Text.Trim() == String.Empty)
                {
                    TxtQty1.Text = "0.0000";
                }

                TxtEnteredWeight.Text = String.Format("{0:0.0000}", Convert.ToDouble(MyBase.Sum_With_Four_Digits(ref GridDetail, "Qty")));                 
                if (TxtEnteredWeight.Text.Trim() == String.Empty)
                {
                    TxtEnteredWeight.Text = "0.0000";
                }

                TxtBalance.Text = String.Format("{0:0.0000}", Convert.ToDouble(TxtQty1.Text) - Convert.ToDouble(TxtEnteredWeight.Text));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ButOk_Click(object sender, EventArgs e)
        {
            try
            {
                TxtBalance.Text = String.Format("{0:0.00}", Convert.ToDouble(TxtBalance.Text));
                if (TxtBalance.Text.Trim() == String.Empty ||  TxtBalance.Text.ToString() != "0.00")
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    GridDetail.CurrentCell = GridDetail["Qty", 0];
                    GridDetail.Focus();
                    GridDetail.BeginEdit(true);
                    return;
                }
                //Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Grid["Slno1", Grid.CurrentCell.RowIndex].Value;
                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Yarn", (Grid.CurrentCell.RowIndex + 1)];
                Grid.Focus();
                Grid.BeginEdit(true);
                return;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ButExit_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtBalance.Text.Trim() == String.Empty || Convert.ToDouble(TxtBalance.Text) != 0)
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    GridDetail.CurrentCell = GridDetail["Qty", 0];
                    GridDetail.BeginEdit(true);
                    GridDetail.Focus();
                    return;
                }

                GBQty.Visible = false;
                Grid.CurrentCell = Grid["Yarn", (Grid.CurrentCell.RowIndex + 1)];
                Grid.Focus();
                Grid.BeginEdit(true);
                return;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FrmCoveringrequirement_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtOrder" || this.ActiveControl.Name == "TxtTotal")
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = false;
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
