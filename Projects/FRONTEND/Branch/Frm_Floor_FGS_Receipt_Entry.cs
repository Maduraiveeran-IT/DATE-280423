using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Windows.Forms;

namespace Accounts
{
    public partial class Frm_Floor_FGS_Receipt_Entry : Form, Entry
    {
        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataRow Dr;
        TextBox Txt = null;
        TextBox Txt_Qty = null;
        TextBox Txt_Cont = null;
        Int64 Code = 0;
        DataTable[] DtQty;
        DataTable[] DtCont;
        String Str;
        Int16 Vis = 0;
        int Pos = 0;

        public Frm_Floor_FGS_Receipt_Entry()
        {
            InitializeComponent();
        }

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                DataTable Dth = new DataTable();
                RbtSingle.Checked = true;
                Grid_Data();
                DtQty = new DataTable[30];
                DtpDate1.Focus();
                RbtMultiple.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Save()
        {
            String[] Queries;
            Int32 Array_Index = 0;
            double Line_Flag = 0;
            try
            {
                MyBase.Row_Number(ref GridQty);
                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Entry ...!", "Gainup");
                    MyParent.Save_Error = true;
                    DtpDate1.Focus();
                    return;
                }

                if (TxtTotal.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Total ...!", "Gainup");
                    MyParent.Save_Error = true;
                    DtpDate1.Focus();
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 1; j++)
                    {
                        if (Grid["Boxes", i].Value == DBNull.Value || Grid["PCB", i].Value.ToString() == String.Empty || Grid["Qty", i].Value.ToString() == String.Empty)
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

                if (!Check_Qty_Breakup())
                {
                    MessageBox.Show("Check Qty Breakup in Reject Details...!", "Gainup");
                    MyParent.Save_Error = true;
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToInt16(Dt.Rows[i]["Qty"].ToString()) > 0)
                    {
                        for(int j = 0; j <= DtQty[Convert.ToInt16(Dt.Rows[i]["Slno1"].ToString())].Rows.Count - 1; j++)
                        {
                            if(Fill_Bom_Check(Convert.ToInt64(DtQty[Convert.ToInt16(Dt.Rows[i]["Slno1"].ToString())].Rows[j]["JoNO_Master_ID"].ToString()), Convert.ToInt64(DtQty[Convert.ToInt16(Dt.Rows[i]["Slno1"].ToString())].Rows[j]["JoNO_Details_ID"].ToString())) < 0)
                            {
                                MessageBox.Show("Invalid Qty For JO No : '" + DtQty[Convert.ToInt16(Dt.Rows[i]["Slno1"].ToString())].Rows[j]["JoNO"].ToString() + "'", "Gainup...!");
                                Grid.CurrentCell = Grid["Qty", i];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }
                    }
                }

                TxtEntryNo.Text = MyBase.MaxOnlyWithoutComp("Floor_FGS_Receipt_Master", "EntryNo", String.Empty, String.Empty, 0).ToString();
                Queries = new string[Dt.Rows.Count * 300];

                if (MyParent._New)
                {
                    if (RbtSingle.Checked == true)
                    {
                        Queries[Array_Index++] = "Insert into Floor_FGS_Receipt_Master (EntryNo, EntryDate, EntryTime, EntrySystem, Remarks, UserCode, Pack_Mode) values (" + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', Getdate(), Host_Name(), '" + TxtRemarks.Text + "', " + MyParent.UserCode + ", 'Single'); Select Scope_Identity() ";
                    }
                    else if (RbtMultiple.Checked == true)
                    {
                        Queries[Array_Index++] = "Insert into Floor_FGS_Receipt_Master (EntryNo, EntryDate, EntryTime, EntrySystem, Remarks, UserCode, Pack_Mode) values (" + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', Getdate(), Host_Name(), '" + TxtRemarks.Text + "', " + MyParent.UserCode + ", 'Multiple'); Select Scope_Identity() ";
                    }
                    Queries[Array_Index++] = MyParent.EntryLog("Floor_FGS_Receipt", "ADD", "@@IDENTITY");
                }
                else
                {
                    if (RbtSingle.Checked == true)
                    {
                        Queries[Array_Index++] = "Update Floor_FGS_Receipt_Master Set EntryDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', Remarks = '" + TxtRemarks.Text + "', UserCode = " + MyParent.UserCode + ", EntryTime = Getdate(), EntrySystem = Host_Name(), Pack_Mode = 'Single' Where RowID = " + Code;
                    }
                    else if (RbtMultiple.Checked == true)
                    {
                        Queries[Array_Index++] = "Update Floor_FGS_Receipt_Master Set EntryDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', Remarks = '" + TxtRemarks.Text + "', UserCode = " + MyParent.UserCode + ", EntryTime = Getdate(), EntrySystem = Host_Name(), Pack_Mode = 'Multiple' Where RowID = " + Code;
                    }
                    Queries[Array_Index++] = MyParent.EntryLog("Floor_FGS_Receipt", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete From Floor_FGS_Receipt_JONo_Details where MasterID = " + Code;
                    Queries[Array_Index++] = "Delete From Floor_FGS_Receipt_Details Where MasterID = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        if (RbtSingle.Checked == true)
                        {
                            Queries[Array_Index++] = "Insert into Floor_FGS_Receipt_Details (MasterID, Slno, Order_No, Sample_ID, ItemID, Pack_Type, PCB, Boxes, Qty, Slno1, Remarks, Pack_Qty) Values (@@IDENTITY, " + Dt.Rows[i]["Slno"].ToString() + ", '" + Dt.Rows[i]["Order_No"].ToString() + "', " + Dt.Rows[i]["Sample_ID"].ToString() + ", " + Dt.Rows[i]["ItemID"].ToString() + ", '" + Dt.Rows[i]["Pack_Type"].ToString() + "', " + Dt.Rows[i]["PCB"].ToString() + ", " + Dt.Rows[i]["Boxes"].ToString() + ", " + Dt.Rows[i]["Qty"].ToString() + ", " + Grid["Slno1", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "', " + Grid["Pack_Qty", i].Value.ToString() + ")";
                        }
                        else if (RbtMultiple.Checked == true)
                        {
                            Queries[Array_Index++] = "Insert into Floor_FGS_Receipt_Details (MasterID, Slno, Order_No, Pack_Type, PCB, Boxes, Qty, Slno1, Remarks, Pack_Qty) Values (@@IDENTITY, " + Dt.Rows[i]["Slno"].ToString() + ", '" + Dt.Rows[i]["Order_No"].ToString() + "', '" + Dt.Rows[i]["Pack_Type"].ToString() + "', " + Dt.Rows[i]["PCB"].ToString() + ", " + Dt.Rows[i]["Boxes"].ToString() + ", " + Dt.Rows[i]["Qty"].ToString() + ", " + Grid["Slno1", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "', " + Grid["Pack_Qty", i].Value.ToString() + ")";
                        }
                    }
                    else
                    {
                        if (RbtSingle.Checked == true)
                        {
                            Queries[Array_Index++] = "Insert into Floor_FGS_Receipt_Details (MasterID, Slno, Order_No, Sample_ID, ItemID, Pack_Type, PCB, Boxes, Qty, Slno1, Remarks) Values (" + Code + ", " + Dt.Rows[i]["Slno"].ToString() + ", '" + Dt.Rows[i]["Order_No"].ToString() + "', " + Dt.Rows[i]["Sample_ID"].ToString() + ", " + Dt.Rows[i]["ItemID"].ToString() + ", '" + Dt.Rows[i]["Pack_Type"].ToString() + "', " + Dt.Rows[i]["PCB"].ToString() + ", " + Dt.Rows[i]["Boxes"].ToString() + ", " + Dt.Rows[i]["Qty"].ToString() + ", " + Grid["Slno1", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "', " + Grid["Pack_Qty", i].Value.ToString() + ")";
                        }
                        else if (RbtMultiple.Checked == true)
                        {
                            Queries[Array_Index++] = "Insert into Floor_FGS_Receipt_Details (MasterID, Slno, Order_No, Pack_Type, PCB, Boxes, Qty, Slno1, Remarks) Values (" + Code + ", " + Dt.Rows[i]["Slno"].ToString() + ", '" + Dt.Rows[i]["Order_No"].ToString() + "', '" + Dt.Rows[i]["Pack_Type"].ToString() + "', " + Dt.Rows[i]["PCB"].ToString() + ", " + Dt.Rows[i]["Boxes"].ToString() + ", " + Dt.Rows[i]["Qty"].ToString() + ", " + Grid["Slno1", i].Value.ToString() + ", '" + Grid["Remarks", i].Value.ToString() + "', " + Grid["Pack_Qty", i].Value.ToString() + ")";
                        }
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToInt16(Dt.Rows[i]["Qty"].ToString()) > 0)
                    {
                        for (i = 0; i <= DtQty.Length - 1; i++)
                        {
                            if (DtQty[i] != null)
                            {
                                for (int j = 0; j <= DtQty[i].Rows.Count - 1; j++)
                                {
                                    if (MyParent._New)
                                    {
                                        Queries[Array_Index++] = "Insert Into Floor_FGS_Receipt_JONo_Details (MasterId, Slno1, Slno, JoNO_Master_ID, JoNO_Details_ID, JoQty, Prod_Qty) values (@@IDENTITY, " + DtQty[i].Rows[j]["SLNO1"].ToString() + ", " + DtQty[i].Rows[j]["SLNO"] + ", " + DtQty[i].Rows[j]["JoNO_Master_ID"] + ", " + DtQty[i].Rows[j]["JoNO_Details_ID"] + ", " + DtQty[i].Rows[j]["JoQty"] + ", " + DtQty[i].Rows[j]["Prod_Qty"] + ")";
                                    }
                                    else
                                    {
                                        Queries[Array_Index++] = "Insert Into Floor_FGS_Receipt_JONo_Details (MasterId, Slno1, Slno, JoNO_Master_ID, JoNO_Details_ID, JoQty, Prod_Qty) values (" + Code + ", " + DtQty[i].Rows[j]["SLNO1"].ToString() + ", " + DtQty[i].Rows[j]["SLNO"] + ", " + DtQty[i].Rows[j]["JoNO_Master_ID"] + ", " + DtQty[i].Rows[j]["JoNO_Details_ID"] + ", " + DtQty[i].Rows[j]["JoQty"] + ", " + DtQty[i].Rows[j]["Prod_Qty"] + ")";
                                    }
                                }
                            }
                        }
                    }
                }

                MyBase.Run_Identity(MyParent.Edit, Queries);
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

        Int64 Fill_Bom_Check(Int64 JoNO_Master_ID, Int64 JoNO_Details_ID)
        {
            Int64 Bal_Qty = 0;
            Int64 Prod_Qty = 0;
            try
            {
                DataTable Dt1 = new DataTable();

                Str = " Select A.JONo, B.Order_ID, B.Sample_ID, B.Po_No, B.JO_Qty, Isnull(C.Prod_Qty,0)Prod_Qty, (B.JO_Qty - Isnull(C.Prod_Qty,0))Bal_Qty, ";
                Str = Str + " A.RowID JoNO_Master_ID, B.RowID JoNO_Details_ID From Socks_JobOrder_Master A Left Join Socks_JobOrder_Details B On A.RowID = B.Master_ID ";
                Str = Str + " Left Join (Select JoNO_Master_ID, JoNO_Details_ID, Sum(Prod_Qty)Prod_Qty From Floor_FGS_Receipt_JONo_Details ";
                Str = Str + " Group By JoNO_Master_ID, JoNO_Details_ID)C On A.RowID = C.JoNO_Master_ID And B.RowID = C.JoNO_Details_ID ";
                Str = Str + " Where A.Print_Out_Taken = 'Y' And A.RowID = " + JoNO_Master_ID + " And B.RowID = " + JoNO_Details_ID + " ";

                MyBase.Load_Data(Str, ref Dt1);

                if (Dt1.Rows.Count > 0)
                {
                    Bal_Qty = Convert.ToInt64(Dt1.Rows[0]["Bal_Qty"].ToString());

                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Convert.ToInt16(Dt.Rows[i]["Qty"].ToString()) > 0)
                        {
                            for (int j = 0; j <= DtQty[Convert.ToInt16(Dt.Rows[i]["Slno1"].ToString())].Rows.Count - 1; j++)
                            {
                                if (Convert.ToInt64(DtQty[Convert.ToInt16(Dt.Rows[i]["Slno1"].ToString())].Rows[j]["JoNO_Master_ID"].ToString()) == JoNO_Master_ID && Convert.ToInt64(DtQty[Convert.ToInt16(Dt.Rows[i]["Slno1"].ToString())].Rows[j]["JoNO_Details_ID"].ToString()) == JoNO_Details_ID)  
                                {
                                    Prod_Qty = Prod_Qty + Convert.ToInt64(DtQty[Convert.ToInt16(Dt.Rows[i]["Slno1"].ToString())].Rows[j]["Prod_Qty"].ToString());
                                }
                            }
                        }
                    }
                }
                return Bal_Qty - Prod_Qty;
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                return 0;
            }
        }

        Boolean Check_Qty_Breakup()
        {
            Double BRQty = 0;
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    BRQty = 0;

                    if (Convert.ToInt16(Dt.Rows[i]["Qty"].ToString()) > 0)
                    {
                        if (DtQty[Convert.ToInt32(Dt.Rows[i]["SlNo1"])] == null)
                        {
                            MessageBox.Show("Invalid Qty Breakup Details ...!", "Gainup");
                            MyParent.Save_Error = true;
                            Grid.CurrentCell = Grid["Prod_Qty", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return false;
                        }
                        else
                        {
                            for (int j = 0; j <= DtQty[Convert.ToInt32(Dt.Rows[i]["Slno1"])].Rows.Count - 1; j++)
                            {
                                BRQty += Convert.ToDouble(DtQty[Convert.ToInt32(Dt.Rows[i]["Slno1"])].Rows[j]["Prod_Qty"]);
                            }

                            if (Math.Round(Convert.ToDouble(BRQty), 3) != Math.Round(Convert.ToDouble(Grid["Qty", i].Value), 3))
                            {
                                MessageBox.Show("Invalid Qty Breakup Details...!", "Gainup");
                                Grid.Focus();
                                Grid.CurrentCell = Grid["Qty", i];
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

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select FGS Receipt Entry - Delete", "Select F1.EntryNo, F1.EntryDate, F1.Remarks, F1.RowID, F1.Pack_Mode From Floor_FGS_Receipt_Master F1 Order by F1.EntryNo Desc ", String.Empty, 90, 100, 150);
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
                if (Code > 0 && Dt.Rows.Count > 0)
                {
                    MyBase.Run("Delete from Floor_FGS_Receipt_JONo_Details where MasterID = " + Code, "Delete From Floor_FGS_Receipt_Details Where MasterID = " + Code, "Delete From Floor_FGS_Receipt_Master Where RowID = " + Code);
                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyParent.Load_DeleteEntry();
                }
                else
                {
                    MessageBox.Show("Invalid Entry to Delete ...!", "Gainup");
                    MyParent.Load_DeleteEntry();
                }
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
                DtQty = new DataTable[30];
                DtCont = new DataTable[100];
                Code = Convert.ToInt64(Dr["RowID"]);
                TxtEntryNo.Text = Dr["EntryNo"].ToString();
                DtpDate1.Value = Convert.ToDateTime(Dr["EntryDate"]);
                TxtRemarks.Text = Dr["Remarks"].ToString();
                if (Dr["Pack_Mode"].ToString() == "Single")
                {
                    RbtSingle.Checked = true;
                }
                else if(Dr["Pack_Mode"].ToString() == "Multiple")
                {
                    RbtMultiple.Checked = true;
                }

                Grid_Data();
                Total();
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
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select FGS Receipt Entry - Edit", "Select F1.EntryNo, F1.EntryDate, F1.Remarks, F1.RowID, F1.Pack_Mode From Floor_FGS_Receipt_Master F1 Order by F1.EntryNo Desc ", String.Empty, 90, 100, 150);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    TxtEntryNo.Focus();
                }
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
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Pairing Production Entry - View", "Select F1.EntryNo, F1.EntryDate, F1.Remarks, F1.RowID, F1.Pack_Mode From Floor_FGS_Receipt_Master F1 Order by F1.EntryNo Desc ", String.Empty, 90, 100, 150);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    TxtEntryNo.Focus();
                }
                RbtSingle.Enabled = false;
                RbtMultiple.Enabled = false;
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

        private void Frm_Floor_FGS_Receipt_Entry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                DtpDate1.Focus();
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
                if (MyParent._New)
                {
                    if(RbtSingle.Checked == true)
                    {
                        Str = " Select B.Slno Slno, B.Order_No, C.Color Sample_No, B.Sample_ID, C.Size, C.Item, B.ItemID, C.GUOM_Lookup Pack_Type, C.GUom_Conv Pack_Qty, 0 Bom_Qty, 0 Openning, 0 Received_Qty, 0 Bal_Qty, 0 Bal_Qty_New, B.Boxes, B.PCB, B.Qty, B.Slno1, B.Remarks, '' Details, ''T ";
                        Str = Str + " From Floor_FGS_Receipt_Master A Left Join Floor_FGS_Receipt_Details B On A.RowID = B.MasterID Left Join Socks_Bom()C On B.Order_No = C.Order_No And B.Sample_ID = C.OrderColorId And B.ItemID = C.ItemID Where 1 = 2 ";
                    }
                    else if(RbtMultiple.Checked == true)
                    {
                        Str = " Select B.Slno Slno, B.Order_No, C.GUOM_Lookup Pack_Type, C.GUom_Conv Pack_Qty, 0 Bom_Qty, 0 Openning, 0 Received_Qty, 0 Bal_Qty, 0 Bal_Qty_New, B.Boxes, B.PCB, B.Qty, B.Slno1, B.Remarks, '' Details, ''T ";
                        Str = Str + " From Floor_FGS_Receipt_Master A Left Join Floor_FGS_Receipt_Details B On A.RowID = B.MasterID Left Join Socks_Bom()C On B.Order_No = C.Order_No And B.Sample_ID = C.OrderColorId And B.ItemID = C.ItemID Where 1 = 2 ";
                    }
                    Dt = new DataTable();
                }
                else
                {
                    //Str = Str + " Select B.Slno Slno, B.Order_No, C.Color Sample_No, B.Sample_ID, C.Size, C.Item, B.ItemID, C.GUOM_Lookup Pack_Type, C.GUom_Conv Pack_Qty, Isnull(C.Bom_Qty, 0) Bom_Qty, Isnull(D.Received_Qty, 0) Received_Qty, (Isnull(C.Bom_Qty, 0) - IsNull(D.Received_Qty, 0)) Bal_Qty, ((Isnull(C.Bom_Qty, 0) - IsNull(D.Received_Qty, 0)) + SUM(B1.Prod_Qty)) Bal_Qty_New, B.Boxes, B.PCB, B.Qty, B.Slno1, B.Remarks, '' Details, ''T ";
                    //Str = Str + " From Floor_FGS_Receipt_Master A Left Join Floor_FGS_Receipt_Details B On A.RowID = B.MasterID Left Join Floor_FGS_Receipt_JONo_Details B1 On A.RowID = B1.MasterID And B.MasterID = B1.MasterID And B.Slno1 = B1.Slno1 ";
                    //Str = Str + " Left Join Socks_Bom()C On B.Order_No = C.Order_No And B.Sample_ID = C.OrderColorId And B.ItemID = C.ItemID Left Join FGS_Receipt_Received_Details() D On B.Order_No = D.Order_No And B.Sample_ID = D.Sample_ID Where A.RowID = " + Code;
                    //Str = Str + " Group BY B.Slno, B.Order_No, C.Color, B.Sample_ID, C.Size, C.Item, B.ItemID, C.GUOM_Lookup, C.GUom_Conv, Isnull(C.Bom_Qty, 0), Isnull(D.Received_Qty, 0), B.Boxes, B.PCB, B.Qty, B.Slno1, B.Remarks Order By B.Slno ";
                    if(RbtSingle.Checked == true)
                    {
                        Str = " Select B.Slno Slno, B.Order_No, C.Color Sample_No, B.Sample_ID, C.Size, C.Item, B.ItemID, C.GUOM_Lookup Pack_Type, C.GUom_Conv Pack_Qty, Isnull(C.Bom_Qty, 0) Bom_Qty, ISNULL(E.Production, 0)Openning, ";
                        Str = Str + " (Isnull(D.Received_Qty, 0) - SUM(B1.Prod_Qty)) Received_Qty, (ISNULL(E.Production, 0) - IsNull(D.Received_Qty, 0)) Bal_Qty, "; 
                        Str = Str + " ((ISNULL(E.Production, 0) - IsNull(D.Received_Qty, 0)) + SUM(B1.Prod_Qty)) Bal_Qty_New, B.Boxes, B.PCB, B.Qty, B.Slno1, B.Remarks, '' Details, ''T ";
                        Str = Str + " From Floor_FGS_Receipt_Master A Left Join Floor_FGS_Receipt_Details B On A.RowID = B.MasterID "; 
                        Str = Str + " Left Join Floor_FGS_Receipt_JONo_Details B1 On A.RowID = B1.MasterID And B.MasterID = B1.MasterID And B.Slno1 = B1.Slno1 ";
                        Str = Str + " Left Join Socks_Bom()C On B.Order_No = C.Order_No And B.Sample_ID = C.OrderColorId And B.ItemID = C.ItemID "; 
                        Str = Str + " Left Join FGS_Receipt_Received_Details() D On B.Order_No = D.Order_No And B.Sample_ID = D.Sample_ID "; 
                        Str = Str + " Left Join Socks_Pairing_Prod_Details()E On B.Order_No = E.Order_No And B.Sample_ID = E.OrderColorID Where A.RowID = " + Code;
                        Str = Str + " Group BY B.Slno, B.Order_No, C.Color, B.Sample_ID, C.Size, C.Item, B.ItemID, C.GUOM_Lookup, C.GUom_Conv, Isnull(C.Bom_Qty, 0), ISNULL(E.Production, 0), Isnull(D.Received_Qty, 0), B.Boxes, B.PCB, B.Qty, B.Slno1, B.Remarks Order By B.Slno ";
                    }
                    else if(RbtMultiple.Checked = true)
                    {
                        Str = " Select B.Slno Slno, B.Order_No, B.Pack_Type, B.Pack_Qty, Isnull(C.Bom_Qty, 0) Bom_Qty, ISNULL(E.Production, 0)Openning, ";
                        Str = Str + " (Isnull(D.Prod_Qty, 0) - SUM(B1.Prod_Qty)) Received_Qty, ((ISNULL(E.Production, 0) - IsNull(D.Prod_Qty, 0)) + SUM(B1.Prod_Qty)) Bal_Qty, ";
                        Str = Str + " ((ISNULL(E.Production, 0) - IsNull(D.Prod_Qty, 0)) + SUM(B1.Prod_Qty)) Bal_Qty_New, B.Boxes, B.PCB, B.Qty, B.Slno1, B.Remarks, '' Details, ''T ";
                        Str = Str + " From Floor_FGS_Receipt_Master A Left Join Floor_FGS_Receipt_Details B On A.RowID = B.MasterID ";
                        Str = Str + " Left Join Floor_FGS_Receipt_JONo_Details B1 On A.RowID = B1.MasterID And B.MasterID = B1.MasterID And B.Slno1 = B1.Slno1 ";
                        Str = Str + " Left Join Socks_Bom_Details()C On B.Order_No = C.Order_No ";
                        Str = Str + " Left Join Orderwise_FGS_Received_Qty() D On B.Order_No = D.Order_No ";
                        Str = Str + " Left Join Socks_Pairing_Prod_Details_Orderwise()E On B.Order_No = E.Order_No Where A.RowID = " + Code;
                        Str = Str + " Group BY B.Slno, B.Order_No, B.Pack_Type, B.Pack_Qty, Isnull(C.Bom_Qty, 0), ISNULL(E.Production, 0), Isnull(D.Prod_Qty, 0), B.Boxes, B.PCB, B.Qty, B.Slno1, B.Remarks Order By B.Slno ";
                    }
                    Dt = new DataTable();
                }

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                if(RbtSingle.Checked == true)
                {
                    MyBase.Grid_Designing(ref Grid, ref Dt, "SLNO1", "Sample_ID", "ItemID", "Bal_Qty_New", "Pack_Qty", "Slno1", "Details", "T");
                    MyBase.ReadOnly_Grid_Without(ref Grid, "Order_No", "Boxes", "PCB", "Qty", "Remarks");
                    MyBase.Grid_Width(ref Grid, 50, 120, 100, 90, 90, 100, 90, 90, 90, 100);
                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                }
                else if(RbtMultiple.Checked == true)
                {
                    MyBase.Grid_Designing(ref Grid, ref Dt, "SLNO1", "Bal_Qty_New", "Pack_Qty", "Slno1", "Details", "T");
                    MyBase.ReadOnly_Grid_Without(ref Grid, "Order_No", "Pack_Type", "Boxes", "PCB", "Qty", "Remarks");
                    MyBase.Grid_Width(ref Grid, 50, 120, 100, 90, 90, 100, 90, 90, 90, 100);
                    MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                }

                Grid.Columns["Boxes"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Boxes"].DefaultCellStyle.Format = "0";
                Grid.Columns["PCB"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["PCB"].DefaultCellStyle.Format = "0";

                Grid.RowHeadersWidth = 10;

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (!MyParent._New)
                    {
                        if (Convert.ToInt16(Grid["Qty", i].Value) > 0)
                        {
                            TxtQty.Text = Grid["Qty", i].Value.ToString();
                            Vis = 1;
                            Pos = i;
                            Grid_Data_Qty(Convert.ToInt16(Grid["Slno1", i].Value));
                            Vis = 0;
                            Pos = 0;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Grid_Data_Qty(Int32 Row)
        {
            try
            {
                if (DtQty[Row] == null)
                {
                    
                    if (MyParent._New)
                    {
                        if (RbtSingle.Checked == true)
                        {
                            Str = "Select 0 SLNO, '' JONO, '' PoNO, JOQty, Prod_Qty, 0 Bal_Qty, 0 JoNO_Master_ID, 0 JoNO_Details_ID, SLNO1, '' Details, 0 Bal_Qty_New, ''T From Floor_FGS_Receipt_JONo_Details WHERE 1 = 2";
                        }
                        else if (RbtMultiple.Checked == true)
                        {
                            Str = "Select 0 SLNO, '' JONO, '' Sample_No, '' Item, '' Size, '' PoNO, JOQty, Prod_Qty, 0 Bal_Qty, 0 JoNO_Master_ID, 0 JoNO_Details_ID, SLNO1, '' Details, 0 Bal_Qty_New, ''T From Floor_FGS_Receipt_JONo_Details WHERE 1 = 2";
                        }
                        DtQty[Row] = new DataTable();
                        MyBase.Load_Data(Str, ref DtQty[Row]);
                    }
                    else
                    {
                        if (MyParent.Edit)
                        {
                            if (Vis == 1)
                            {
                                if (RbtSingle.Checked == true)
                                {
                                    Str = " Select B.Slno, C.JONo, C.Po_No PoNO, B.JoQty, B.Prod_Qty, 0 Bal_Qty, B.JoNO_Master_ID, B.JoNO_Details_ID, B.Slno1, (C.JONo + '' + C.Po_No)Details, 0 Bal_Qty_New, ''T From Floor_FGS_Receipt_Details A ";
                                    Str = Str + " Left Join Floor_FGS_Receipt_JONo_Details B On A.MasterID = B.MasterID And A.Slno1 = B.SLno1 ";
                                    Str = Str + " Left Join (Select A.RowID JoNO_Master_ID, B.RowID JoNO_Details_ID, A.JoNo, B.Po_No, B.Jo_Qty, A.Unit_Code From Socks_JobOrder_Master A ";
                                    Str = Str + " Left Join Socks_JobOrder_Details B On A.RowID = B.Master_ID)C On B.JoNO_Master_ID = C.JoNO_Master_ID And B.JoNO_Details_ID = C.JoNO_Details_ID ";
                                    Str = Str + " Where B.MasterID = " + Code + " And B.Slno1 = " + Grid["Slno1", Pos].Value.ToString();
                                }
                                else if (RbtMultiple.Checked == true)
                                {
                                    Str = " Select B.Slno, C.JONo, F.Sample_No, G.Item, H.Size, C.Po_No PoNO, B.JoQty, B.Prod_Qty, 0 Bal_Qty, B.JoNO_Master_ID, B.JoNO_Details_ID, ";
                                    Str = Str + " B.Slno1, (C.JONo + '' + F.Sample_No + '' + C.Po_No)Details, 0 Bal_Qty_New, ''T From Floor_FGS_Receipt_Details A ";
                                    Str = Str + " Left Join Floor_FGS_Receipt_JONo_Details B On A.MasterID = B.MasterID And A.Slno1 = B.SLno1 ";
                                    Str = Str + " Left Join (Select A.RowID JoNO_Master_ID, B.RowID JoNO_Details_ID, A.JoNo, B.Order_ID, B.Sample_ID, B.Po_No, B.Jo_Qty, A.Unit_Code ";
                                    Str = Str + " From Socks_JobOrder_Master A Left Join Socks_JobOrder_Details B On A.RowID = B.Master_ID)C On B.JoNO_Master_ID = C.JoNO_Master_ID ";
                                    Str = Str + " And B.JoNO_Details_ID = C.JoNO_Details_ID ";
                                    Str = Str + " Left Join Socks_Order_Master D On C.Order_ID = D.RowID ";
                                    Str = Str + " Left Join Socks_Order_Details E On D.RowID = E.Master_ID And C.Order_ID = E.Master_ID And C.Sample_ID = E.Sample_ID And C.Po_No = E.PO_No ";
                                    Str = Str + " Left Join VFit_Sample_Master F On E.Sample_ID = F.RowID And C.Sample_ID = F.RowID ";
                                    Str = Str + " Left Join Item G On F.SampleItemID = G.ItemID ";
                                    Str = Str + " Left Join Size H On F.Sizeid = H.SizeID ";
                                    Str = Str + " Where B.MasterID = " + Code + " And B.Slno1 = " + Grid["Slno1", Pos].Value.ToString();  
                                }
                                DtQty[Row] = new DataTable();
                                MyBase.Load_Data(Str, ref DtQty[Row]);
                            }
                            else
                            {
                                if (RbtSingle.Checked == true)
                                {
                                    Str = " Select B.Slno, C.JONo, C.Po_No PoNO, B.JoQty, B.Prod_Qty, 0 Bal_Qty, B.JoNO_Master_ID, B.JoNO_Details_ID, B.Slno1, (C.JONo + '' + C.Po_No)Details, 0 Bal_Qty_New, ''T From Floor_FGS_Receipt_Details A ";
                                    Str = Str + " Left Join Floor_FGS_Receipt_JONo_Details B On A.MasterID = B.MasterID And A.Slno1 = B.SLno1 ";
                                    Str = Str + " Left Join (Select A.RowID JoNO_Master_ID, B.RowID JoNO_Details_ID, A.JoNo, B.Po_No, B.Jo_Qty, A.Unit_Code From Socks_JobOrder_Master A ";
                                    Str = Str + " Left Join Socks_JobOrder_Details B On A.RowID = B.Master_ID)C On B.JoNO_Master_ID = C.JoNO_Master_ID And B.JoNO_Details_ID = C.JoNO_Details_ID ";
                                    Str = Str + " Where B.MasterID = " + Code + " And B.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();
                                }
                                else if (RbtMultiple.Checked == true)
                                {
                                    Str = " Select B.Slno, C.JONo, F.Sample_No, G.Item, H.Size, C.Po_No PoNO, B.JoQty, B.Prod_Qty, 0 Bal_Qty, B.JoNO_Master_ID, B.JoNO_Details_ID, ";
                                    Str = Str + " B.Slno1, (C.JONo + '' + F.Sample_No + '' + C.Po_No)Details, 0 Bal_Qty_New, ''T From Floor_FGS_Receipt_Details A ";
                                    Str = Str + " Left Join Floor_FGS_Receipt_JONo_Details B On A.MasterID = B.MasterID And A.Slno1 = B.SLno1 ";
                                    Str = Str + " Left Join (Select A.RowID JoNO_Master_ID, B.RowID JoNO_Details_ID, A.JoNo, B.Order_ID, B.Sample_ID, B.Po_No, B.Jo_Qty, A.Unit_Code ";
                                    Str = Str + " From Socks_JobOrder_Master A Left Join Socks_JobOrder_Details B On A.RowID = B.Master_ID)C On B.JoNO_Master_ID = C.JoNO_Master_ID ";
                                    Str = Str + " And B.JoNO_Details_ID = C.JoNO_Details_ID ";
                                    Str = Str + " Left Join Socks_Order_Master D On C.Order_ID = D.RowID ";
                                    Str = Str + " Left Join Socks_Order_Details E On D.RowID = E.Master_ID And C.Order_ID = E.Master_ID And C.Sample_ID = E.Sample_ID And C.Po_No = E.PO_No ";
                                    Str = Str + " Left Join VFit_Sample_Master F On E.Sample_ID = F.RowID And C.Sample_ID = F.RowID ";
                                    Str = Str + " Left Join Item G On F.SampleItemID = G.ItemID ";
                                    Str = Str + " Left Join Size H On F.Sizeid = H.SizeID ";
                                    Str = Str + " Where B.MasterID = " + Code + " And B.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();
                                }
                                DtQty[Row] = new DataTable();
                                MyBase.Load_Data(Str, ref DtQty[Row]);
                            }
                        }
                        else
                        {
                            if (RbtSingle.Checked == true)
                            {
                                Str = " Select B.Slno, C.JONo, C.Po_No PoNO, B.JoQty, B.Prod_Qty, 0 Bal_Qty, B.JoNO_Master_ID, B.JoNO_Details_ID, B.Slno1, (C.JONo + '' + C.Po_No)Details, 0 Bal_Qty_New, ''T From Floor_FGS_Receipt_Details A ";
                                Str = Str + " Left Join Floor_FGS_Receipt_JONo_Details B On A.MasterID = B.MasterID And A.Slno1 = B.SLno1 ";
                                Str = Str + " Left Join (Select A.RowID JoNO_Master_ID, B.RowID JoNO_Details_ID, A.JoNo, B.Po_No, B.Jo_Qty, A.Unit_Code From Socks_JobOrder_Master A ";
                                Str = Str + " Left Join Socks_JobOrder_Details B On A.RowID = B.Master_ID)C On B.JoNO_Master_ID = C.JoNO_Master_ID And B.JoNO_Details_ID = C.JoNO_Details_ID ";
                                Str = Str + " Where B.MasterID = " + Code + " And B.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();
                            }
                            else if (RbtMultiple.Checked == true)
                            {
                                Str = " Select B.Slno, C.JONo, F.Sample_No, G.Item, H.Size, C.Po_No PoNO, B.JoQty, B.Prod_Qty, 0 Bal_Qty, B.JoNO_Master_ID, B.JoNO_Details_ID, ";
                                Str = Str + " B.Slno1, (C.JONo + '' + F.Sample_No + '' + C.Po_No)Details, 0 Bal_Qty_New, ''T From Floor_FGS_Receipt_Details A ";
                                Str = Str + " Left Join Floor_FGS_Receipt_JONo_Details B On A.MasterID = B.MasterID And A.Slno1 = B.SLno1 ";
                                Str = Str + " Left Join (Select A.RowID JoNO_Master_ID, B.RowID JoNO_Details_ID, A.JoNo, B.Order_ID, B.Sample_ID, B.Po_No, B.Jo_Qty, A.Unit_Code ";
                                Str = Str + " From Socks_JobOrder_Master A Left Join Socks_JobOrder_Details B On A.RowID = B.Master_ID)C On B.JoNO_Master_ID = C.JoNO_Master_ID ";
                                Str = Str + " And B.JoNO_Details_ID = C.JoNO_Details_ID ";
                                Str = Str + " Left Join Socks_Order_Master D On C.Order_ID = D.RowID ";
                                Str = Str + " Left Join Socks_Order_Details E On D.RowID = E.Master_ID And C.Order_ID = E.Master_ID And C.Sample_ID = E.Sample_ID And C.Po_No = E.PO_No ";
                                Str = Str + " Left Join VFit_Sample_Master F On E.Sample_ID = F.RowID And C.Sample_ID = F.RowID ";
                                Str = Str + " Left Join Item G On F.SampleItemID = G.ItemID ";
                                Str = Str + " Left Join Size H On F.Sizeid = H.SizeID ";
                                Str = Str + " Where B.MasterID = " + Code + " And B.Slno1 = " + Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();
                            }
                            DtQty[Row] = new DataTable();
                            MyBase.Load_Data(Str, ref DtQty[Row]);
                        }
                    }
                }

                GridQty.DataSource = DtQty[Row];
                MyBase.Grid_Designing(ref GridQty, ref DtQty[Row], "JoNO_Master_ID", "JoNO_Details_ID", "SLNO1", "Details", "Bal_Qty_New", "T");
                MyBase.ReadOnly_Grid_Without(ref GridQty, "JoNO", "Prod_Qty");
                MyBase.Grid_Colouring(ref GridQty, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref GridQty, 50, 100, 100, 90, 90);

                GridQty.RowHeadersWidth = 30;
                GridQty.Columns["Prod_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridQty.Columns["Prod_Qty"].DefaultCellStyle.Format = "0";
                Balance_Pieces();

                //if (MyParent.Edit && Vis == 1)
                if (Vis == 1)
                {
                    GBQty.Visible = false;
                }
                else
                {
                    GBQty.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Balance_Pieces()
        {
            try
            {
                TxtEnteredPieces.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref GridQty, "Prod_Qty")));
                if (TxtEnteredPieces.Text.Trim() == String.Empty)
                {
                    TxtBalance.Text = String.Format("{0:0}", Convert.ToDouble(TxtQty.Text));
                }
                else
                {
                    if (TxtQty.Text.Trim() != String.Empty)
                    {
                        TxtBalance.Text = String.Format("{0:0}", (Convert.ToDouble(TxtQty.Text) - Convert.ToDouble(TxtEnteredPieces.Text)));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Frm_Floor_FGS_Receipt_Entry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    
                }
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "DtpDate1")
                    {
                        Grid.Focus();
                        Grid.CurrentCell = Grid["Order_No", 0];
                        Grid.BeginEdit(true);
                        return;
                    }
                    else if (this.ActiveControl.Name == "TxtEntryNo" || this.ActiveControl.Name == "RbtSingle" || this.ActiveControl.Name == "RbtMultiple")
                    {
                        Grid.Focus();
                        Grid.CurrentCell = Grid["Order_No", 0];
                        Grid.BeginEdit(true);
                        return;
                    }
                    else if (this.ActiveControl.Name == "TxtTotal")
                    {
                        if (MyParent._New || MyParent.Edit)
                        {
                            MyParent.Load_SaveEntry();
                        }
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    if (GBQty.Visible == true)
                    {

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

        private void Frm_Floor_FGS_Receipt_Entry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtEntryNo" || this.ActiveControl.Name == "TxtTotal")
                {
                    MyBase.Valid_Null((TextBox)Txt, e);
                }
                if (this.ActiveControl is TextBox && this.ActiveControl.Name != "TxtRemarks")
                {
                    if (this.ActiveControl.Name != String.Empty)
                    {
                        MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                    }
                }
                if (this.ActiveControl.Name == "DtpDate1")
                {
                    if (Dt.Rows.Count > 0)
                    {
                        e.Handled = true;
                        MessageBox.Show("Already Details Entered..!", "Gainup");
                        return;
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
                    Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                    Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);
                    Txt.Enter += new EventHandler(Txt_Enter);
                    Txt.Leave += new EventHandler(Txt_Leave);
                    Txt.GotFocus += new EventHandler(Txt_GotFocus);
                    Txt.TextChanged += new EventHandler(Txt_TextChanged);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_GotFocus(object sender, EventArgs e)
        {
            try
            {

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void Total()
        {
            try
            {
                TxtTotal.Text = MyBase.Sum(ref Grid, "Qty").ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Txt_Enter(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                {
                    MyBase.Row_Number(ref Grid);
                    Total();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //Grid.Refresh();
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Qty"].Index)
                {
                    e.Handled = true;
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Boxes"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["PCB"].Index)
                {
                    MyBase.Valid_Number(Txt, e);
                }
                else
                {
                    MyBase.Valid_Null(Txt, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void OrderNo_Selection()
        {
            try
            {
                //Str = " Select B.Order_No, B.Color Sample_No, B.Size, B.Item, B.GUOM_Lookup Pack_Type, B.Bom_Qty, Isnull(C.Prod_Qty, 0)Received_Qty, (B.Bom_Qty - Isnull(C.Prod_Qty, 0)) Bal_Qty, (B.Bom_Qty - Isnull(C.Prod_Qty, 0)) Bal_Qty_New, B.OrderColorId Sample_ID, B.ItemID, ";
                //Str = Str + " B.GUom_Conv Pack_Qty, (B.Order_No + '' + B.Color) Details From Socks_Bom()B Left Join Order_FGS_Received_Qty()C On B.Order_No = C.Order_No And B.OrderColorId = C.Sample_ID Where Isnull(B.Despatch_Closed, 'N') = 'N' ";
                
                Str = " Select B.Order_No, B.Color Sample_No, B.Size, B.Item, B.GUOM_Lookup Pack_Type, B.Bom_Qty, Isnull(D.Production, 0)Openning, ";
                Str = Str + " Isnull(C.Prod_Qty, 0)Received_Qty, (Isnull(D.Production, 0) - Isnull(C.Prod_Qty, 0)) Bal_Qty, (Isnull(D.Production, 0) - Isnull(C.Prod_Qty, 0)) Bal_Qty_New, ";
                Str = Str + " B.OrderColorId Sample_ID, B.ItemID, B.GUom_Conv Pack_Qty, (B.Order_No + '' + B.Color) Details From Socks_Bom()B "; 
                Str = Str + " Left Join Order_FGS_Received_Qty()C On B.Order_No = C.Order_No And B.OrderColorId = C.Sample_ID ";
                Str = Str + " Left Join Socks_Pairing_Prod_Details() D On B.Order_No = D.Order_No And B.OrderColorID = D.OrderColorID Where Isnull(B.Despatch_Closed, 'N') = 'N' ";
                
                Dr = Tool.Selection_Tool_Except_New("Details", this, 150, 150, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Order", Str, String.Empty, 120, 120, 100, 100, 100);
                if (Dr != null)
                {
                    Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                    Txt.Text = Dr["Order_No"].ToString();
                    Grid["Sample_ID", Grid.CurrentCell.RowIndex].Value = Dr["Sample_ID"].ToString();
                    Grid["Sample_No", Grid.CurrentCell.RowIndex].Value = Dr["Sample_No"].ToString();
                    Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                    Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                    Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                    Grid["Pack_Type", Grid.CurrentCell.RowIndex].Value = Dr["Pack_Type"].ToString();
                    Grid["Pack_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Pack_Qty"].ToString();
                    Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bom_Qty"].ToString();
                    Grid["Openning", Grid.CurrentCell.RowIndex].Value = Dr["Openning"].ToString();
                    Grid["Received_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Received_Qty"].ToString();
                    Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bal_Qty"].ToString();
                    Grid["Bal_Qty_New", Grid.CurrentCell.RowIndex].Value = Dr["Bal_Qty_New"].ToString();
                    Grid["Details", Grid.CurrentCell.RowIndex].Value = Dr["Details"].ToString();
                    Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void OrderNo_Selection1()
        {
            try
            {
                Str = " Select B.Order_No, B.GUOM_Lookup Pack_Type, B.Bom_Qty, (Case When B.Bom_Qty < Isnull(D.Production, 0) Then B.Bom_Qty Else Isnull(D.Production, 0) End) Openning, ";
                Str = Str + " Isnull(C.Prod_Qty, 0)Received_Qty, ((Case When B.Bom_Qty < Isnull(D.Production, 0) Then B.Bom_Qty Else Isnull(D.Production, 0) End) - Isnull(C.Prod_Qty, 0)) Bal_Qty, ";
                Str = Str + " ((Case When B.Bom_Qty < Isnull(D.Production, 0) Then B.Bom_Qty Else Isnull(D.Production, 0) End) - Isnull(C.Prod_Qty, 0)) Bal_Qty_New, B.GUom_Conv Pack_Qty, (B.Order_No + '' + B.GUOM_Lookup) Details From Socks_Bom_Order_Details()B ";
                Str = Str + " Left Join Orderwise_FGS_Received_Qty()C On B.Order_No = C.Order_No Left Join Socks_Pairing_Prod_Details_Orderwise() D On B.Order_No = D.Order_No Where Isnull(B.Despatch_Closed, 'N') = 'N' ";

                Dr = Tool.Selection_Tool_Except_New("Details", this, 150, 150, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Order", Str, String.Empty, 120, 120, 100, 100, 100);
                if (Dr != null)
                {
                    Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                    Txt.Text = Dr["Order_No"].ToString();
                    Grid["Pack_Type", Grid.CurrentCell.RowIndex].Value = Dr["Pack_Type"].ToString();
                    Grid["Pack_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Pack_Qty"].ToString();
                    Grid["Bom_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bom_Qty"].ToString();
                    Grid["Openning", Grid.CurrentCell.RowIndex].Value = Dr["Openning"].ToString();
                    Grid["Received_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Received_Qty"].ToString();
                    Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bal_Qty"].ToString();
                    Grid["Bal_Qty_New", Grid.CurrentCell.RowIndex].Value = Dr["Bal_Qty_New"].ToString();
                    Grid["Details", Grid.CurrentCell.RowIndex].Value = Dr["Details"].ToString();
                    Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                    {
                        if (RbtSingle.Checked == true)
                        {
                            OrderNo_Selection();
                        }
                        else if (RbtMultiple.Checked == true)
                        {
                            OrderNo_Selection1();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["pack_Type"].Index)
                    {
                        if (RbtMultiple.Checked == true)
                        {
                            Pack_Selection();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Pack_Selection()
        {
            try
            {
                Str = " Select Distinct GUOM_Lookup Pack_Type, Cast(Isnull(To_BUOM, 0) As Numeric(10))Pack_Qty From Garment_UOM Where Cast(Isnull(To_BUOM, 0) As Numeric(10)) > 0 ";
                
                Dr = Tool.Selection_Tool_Except_New("Details", this, 150, 150, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Order", Str, String.Empty, 120, 120);
                if (Dr != null)
                {
                    
                    Grid["Pack_Type", Grid.CurrentCell.RowIndex].Value = Dr["Pack_Type"].ToString();
                    Txt.Text = Dr["Pack_Type"].ToString();
                    Grid["Pack_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Pack_Qty"].ToString();
                    if (Grid["Pack_Qty", Grid.CurrentCell.RowIndex].Value != String.Empty)
                    {
                        Grid["Qty", Grid.CurrentCell.RowIndex].Value = (Convert.ToInt64(Grid["Pack_Qty", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToInt64(Grid["Boxes", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToInt64(Grid["PCB", Grid.CurrentCell.RowIndex].Value.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Boxes"].Index)
                    {
                        if (Grid["Boxes", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            MessageBox.Show("Invalid Boxes ...!", "Gainup");
                            Grid.CurrentCell = Grid["Boxes", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PCB"].Index)
                    {
                        if (Grid["PCB", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            MessageBox.Show("Invalid PCB ...!", "Gainup");
                            Grid.CurrentCell = Grid["PCB", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Qty"].Index)
                    {
                        if (Grid["Qty", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            if (Txt.Text.Trim() != String.Empty)
                            {
                                if (Grid["Qty", Grid.CurrentCell.RowIndex].Value == null || Grid["Qty", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Qty", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                                {
                                    Grid["Qty", Grid.CurrentCell.RowIndex].Value = "0";
                                }

                                if (Convert.ToDouble(Grid["Qty", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["Bal_Qty_New", Grid.CurrentCell.RowIndex].Value))
                                {
                                    e.Handled = true;
                                    MessageBox.Show("Invalid Qty ...!", "Gainup");
                                    //Grid["Qty", Grid.CurrentCell.RowIndex].Value = String.Format("{0:0}", Convert.ToDouble(Grid["Bal_Qty_New", Grid.CurrentCell.RowIndex].Value));
                                    Grid["Qty", Grid.CurrentCell.RowIndex].Value = "0";
                                    Grid["Boxes", Grid.CurrentCell.RowIndex].Value = "0";
                                    Grid["PCb", Grid.CurrentCell.RowIndex].Value = "0";
                                    Grid.CurrentCell = Grid["Qty", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    return;
                                }

                                if (Convert.ToInt16(Grid["Qty", Grid.CurrentCell.RowIndex].Value) > 0)
                                {
                                    GBQty.Visible = true;
                                    e.Handled = true;
                                    TxtQty.Text = Grid["Qty", Grid.CurrentCell.RowIndex].Value.ToString();
                                    Grid_Data_Qty(Convert.ToInt16(Grid["Slno1", Grid.CurrentCell.RowIndex].Value));
                                    GridQty.Focus();
                                    GridQty.CurrentCell = GridQty["JoNO", 0];
                                    GridQty.BeginEdit(true);
                                    return;
                                }
                            }
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
                    {
                        if (Grid["Remarks", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            Grid["Remarks", Grid.CurrentCell.RowIndex].Value = "-";
                        }
                    }
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Boxes"].Index)
                {
                    if (Grid["Qty", Grid.CurrentCell.RowIndex].Value != null && Grid["Qty", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["Qty", Grid.CurrentCell.RowIndex].Value != String.Empty)
                    {
                        if (Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0)
                        {
                            Grid["Qty", Grid.CurrentCell.RowIndex].Value = "0.00";
                        }
                        else
                        {
                            Grid["Qty", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Txt.Text) * Convert.ToDouble(Grid["PCB", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["Pack_Qty", Grid.CurrentCell.RowIndex].Value));
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PCB"].Index)
                {
                    if (Grid["Boxes", Grid.CurrentCell.RowIndex].Value == null || Grid["Boxes", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["Boxes", Grid.CurrentCell.RowIndex].Value) == 0)
                    {
                        Grid["Qty", Grid.CurrentCell.RowIndex].Value = "0.00";
                    }
                    else
                    {
                        if (Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0)
                        {
                            Grid["Qty", Grid.CurrentCell.RowIndex].Value = "0.00";
                        }
                        else
                        {
                            Grid["Qty", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Txt.Text) * Convert.ToDouble(Grid["Boxes", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["Pack_Qty", Grid.CurrentCell.RowIndex].Value));
                        }
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
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    //Grid.Refresh();
                    Balance_Pieces();
                    Total();
                    TxtRemarks.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Leave(object sender, EventArgs e)
        {
            try
            {

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
                if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            if (DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)] != null)
                            {
                                DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)] = null;
                            }
                        }
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                        MyBase.Row_Number(ref Grid);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridQty_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref GridQty, ref DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)], GridQty.CurrentCell.RowIndex);
                DtQty[Convert.ToInt32(Grid["Slno1", Grid.CurrentCell.RowIndex].Value)].AcceptChanges();
                MyBase.Row_Number(ref GridQty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridQty_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Qty == null)
                {
                    Txt_Qty = (TextBox)e.Control;
                    Txt_Qty.KeyDown += new KeyEventHandler(Tx_Qty_KeyDown);
                    Txt_Qty.KeyPress += new KeyPressEventHandler(Tx_Qty_KeyPress);
                    Txt_Qty.TextChanged += new EventHandler(Tx_Qty_TextChanged);
                    Txt_Qty.GotFocus += new EventHandler(Tx_Qty_GotFocus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Tx_Qty_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["Prod_Qty"].Index)
                {
                    if (Txt_Qty.Text.Trim() == String.Empty)
                    {
                        Txt_Qty.Text = TxtBalance.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Tx_Qty_TextChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Tx_Qty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["JoNO"].Index)
                {
                    e.Handled = true;
                }
                else if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["Prod_Qty"].Index)
                {
                    MyBase.Valid_Number(Txt, e);
                    Balance_Pieces();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Tx_Qty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["JoNO"].Index)
                    {
                        if (RbtSingle.Checked == true)
                        {
                            Str = " Select A.JoNo, B.Po_No, B.Jo_Qty, Isnull(C.Prod_Qty, 0)Prod_Qty, (B.Jo_Qty - Isnull(C.Prod_Qty, 0))Bal_Qty, A.RowID JoNO_Master_ID, B.RowID JoNO_Details_ID, (A.JoNo + '' + B.Po_No)Details, (B.Jo_Qty - Isnull(C.Prod_Qty, 0))Bal_Qty_New From Socks_JobOrder_Master A ";
                            Str = Str + " Left Join Socks_JobOrder_Details B On A.RowID = B.Master_ID ";
                            Str = Str + " Left Join Job_Order_FGS_Received_Qty()C On A.RowID = C.JoNO_Master_ID And B.RowID = C.JoNO_Details_ID Left Join Socks_Order_Master D On B.Order_ID = D.RowID Where D.Order_No = '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' And B.Sample_ID = " + Grid["Sample_ID", Grid.CurrentCell.RowIndex].Value.ToString() + " And (B.Jo_Qty - Isnull(C.Prod_Qty, 0)) > 0 ";

                            Dr = Tool.Selection_Tool_Except_New("Details", this, 100, 100, ref DtQty[Convert.ToInt16(Grid["SLNO1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Job Order...!", Str, String.Empty, 110, 110, 100, 100, 100);
                            if (Dr != null)
                            {
                                Txt_Qty.Text = Dr["JoNO"].ToString();
                                GridQty["JoNO", GridQty.CurrentCell.RowIndex].Value = Dr["JoNO"].ToString();
                                GridQty["PoNO", GridQty.CurrentCell.RowIndex].Value = Dr["Po_NO"].ToString();
                                GridQty["JoQty", GridQty.CurrentCell.RowIndex].Value = Dr["Jo_Qty"].ToString();
                                GridQty["Prod_Qty", GridQty.CurrentCell.RowIndex].Value = Dr["Prod_Qty"].ToString();
                                GridQty["Bal_Qty", GridQty.CurrentCell.RowIndex].Value = Dr["Bal_Qty"].ToString();
                                GridQty["Bal_Qty_New", GridQty.CurrentCell.RowIndex].Value = Dr["Bal_Qty_New"].ToString();
                                GridQty["JoNO_Master_ID", GridQty.CurrentCell.RowIndex].Value = Dr["JoNO_Master_ID"].ToString();
                                GridQty["JoNO_Details_ID", GridQty.CurrentCell.RowIndex].Value = Dr["JoNO_Details_ID"].ToString();
                                GridQty["Details", GridQty.CurrentCell.RowIndex].Value = Dr["Details"].ToString();
                                GridQty["Slno1", GridQty.CurrentCell.RowIndex].Value = Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();
                            }
                        }
                        else if (RbtMultiple.Checked == true)
                        {
                            Str = " Select A.JoNo, F.Sample_No, H.Size, B.Po_No, B.Jo_Qty, Isnull(C.Prod_Qty, 0)Prod_Qty, (B.Jo_Qty - Isnull(C.Prod_Qty, 0))Bal_Qty, G.Item, A.RowID JoNO_Master_ID, B.RowID JoNO_Details_ID, (A.JoNo + '' + F.Sample_No + '' + B.Po_No)Details From Socks_JobOrder_Master A ";
                            Str = Str + " Left Join Socks_JobOrder_Details B On A.RowID = B.Master_ID ";
                            Str = Str + " Left Join Job_Order_FGS_Received_Qty()C On A.RowID = C.JoNO_Master_ID And B.RowID = C.JoNO_Details_ID ";
                            Str = Str + " Left Join Socks_Order_Master D On B.Order_ID = D.RowID ";
                            Str = Str + " Left Join Socks_Order_Details E On D.RowID = E.Master_ID And B.Order_ID = E.Master_ID And B.Sample_ID = E.Sample_ID And B.Po_No = E.PO_No ";
                            Str = Str + " Left Join VFit_Sample_Master F On E.Sample_ID = F.RowID And B.Sample_ID = F.RowID ";
                            Str = Str + " Left Join Item G On F.SampleItemID = G.ItemID ";
                            Str = Str + " Left Join Size H On F.Sizeid = H.SizeID ";
                            Str = Str + " Where D.Order_No = '" + Grid["Order_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' And (B.Jo_Qty - Isnull(C.Prod_Qty, 0)) > 0 ";

                            Dr = Tool.Selection_Tool_Except_New("Details", this, 100, 100, ref DtQty[Convert.ToInt16(Grid["SLNO1", Grid.CurrentCell.RowIndex].Value)], SelectionTool_Class.ViewType.NormalView, "Select Job Order...!", Str, String.Empty, 110, 110, 100, 100, 100, 100, 100);
                            if (Dr != null)
                            {
                                Txt_Qty.Text = Dr["JoNO"].ToString();
                                GridQty["JoNO", GridQty.CurrentCell.RowIndex].Value = Dr["JoNO"].ToString();
                                GridQty["Sample_No", GridQty.CurrentCell.RowIndex].Value = Dr["Sample_No"].ToString();
                                GridQty["Item", GridQty.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                                GridQty["Size", GridQty.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                                GridQty["PoNO", GridQty.CurrentCell.RowIndex].Value = Dr["Po_NO"].ToString();
                                GridQty["JoQty", GridQty.CurrentCell.RowIndex].Value = Dr["Jo_Qty"].ToString();
                                GridQty["Prod_Qty", GridQty.CurrentCell.RowIndex].Value = Dr["Prod_Qty"].ToString();
                                GridQty["Bal_Qty", GridQty.CurrentCell.RowIndex].Value = Dr["Bal_Qty"].ToString();
                                GridQty["JoNO_Master_ID", GridQty.CurrentCell.RowIndex].Value = Dr["JoNO_Master_ID"].ToString();
                                GridQty["JoNO_Details_ID", GridQty.CurrentCell.RowIndex].Value = Dr["JoNO_Details_ID"].ToString();
                                GridQty["Details", GridQty.CurrentCell.RowIndex].Value = Dr["Details"].ToString();
                                GridQty["Slno1", GridQty.CurrentCell.RowIndex].Value = Grid["Slno1", Grid.CurrentCell.RowIndex].Value.ToString();
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

        private void ButOk_Click(object sender, EventArgs e)
        {
            try
            {
                Balance_Pieces();
                if (TxtBalance.Text.Trim() == String.Empty)
                {
                    GBQty.Visible = false;
                    return;
                }
                else
                {
                    if (Convert.ToDouble(TxtBalance.Text) == 0)
                    {
                        GBQty.Visible = false;
                        Grid.Focus();
                        Grid.CurrentCell = Grid["Remarks", Grid.CurrentCell.RowIndex];
                        Grid.BeginEdit(true);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Invalid Problem Details ...!", "Gainup");
                        GridQty.CurrentCell = GridQty["JoNO", 0];
                        GridQty.Focus();
                        GridQty.BeginEdit(true);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            try
            {
                GBQty.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_No"].Index)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        if(RbtSingle.Checked == true)
                        {
                            if ((Grid["Order_No", Grid.CurrentCell.RowIndex].Value == null || Grid["Order_No", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty) && Grid["Sample_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                OrderNo_Selection();
                            }
                        }
                        else if (RbtMultiple.Checked == true)
                        {
                            if ((Grid["Order_No", Grid.CurrentCell.RowIndex].Value == null || Grid["Order_No", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty))
                            {
                                OrderNo_Selection1();
                            }
                        }
                        if (MyParent.Edit)
                        {
                            SendKeys.Send("{F2}");
                            SendKeys.Send("{End}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridQty_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (GridQty.Rows.Count > 1)
                {
                    MyBase.Row_Number(ref GridQty);
                }
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

        private void GridQty_KeyDown(object sender, KeyEventArgs e)
        {
            try 
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (GridQty.CurrentCell.ColumnIndex == GridQty.Columns["Prod_Qty"].Index)
                    {
                        if (GridQty["Prod_Qty", GridQty.CurrentCell.RowIndex].Value == null || GridQty["Prod_Qty", GridQty.CurrentCell.RowIndex].Value == DBNull.Value || GridQty["Prod_Qty", GridQty.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                        {
                            GridQty["Prod_Qty", GridQty.CurrentCell.RowIndex].Value = "0";
                        }
                        else if (Convert.ToInt64(GridQty["Prod_Qty", GridQty.CurrentCell.RowIndex].Value.ToString()) > Convert.ToInt64(GridQty["Bal_Qty", GridQty.CurrentCell.RowIndex].Value.ToString()))
                        {
                            MessageBox.Show("Invalid Qty ...!", "Gainup");
                            GridQty["Prod_Qty", GridQty.CurrentCell.RowIndex].Value = "0";
                            GridQty.CurrentCell = GridQty["Prod_Qty", GridQty.CurrentCell.RowIndex];
                            GridQty.Focus();
                            GridQty.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            GridQty["Bal_Qty", GridQty.CurrentCell.RowIndex].Value = Convert.ToInt64(GridQty["Bal_Qty_New", GridQty.CurrentCell.RowIndex].Value) - Convert.ToInt64(GridQty["Prod_Qty", GridQty.CurrentCell.RowIndex].Value);
                        }
                        Balance_Pieces();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RbtSingle_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (Dt.Rows.Count > 0 && !MyParent.View)
                {
                    if (MessageBox.Show("Sure to Clear Grid Detail ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                        {
                            if (Convert.ToInt64(Dt.Rows[i]["Qty"].ToString()) > 0)
                            {
                                DtQty[Convert.ToInt64(Dt.Rows[i]["Slno1"].ToString())] = new DataTable();
                            }
                        }
                        MyBase.Clear(this);
                        Grid_Data();
                    }
                    else
                    {
                        if (RbtSingle.Checked == true)
                        {
                            RbtMultiple.Checked = true;
                        }
                        return;
                    }
                }
                else if (Dt.Rows.Count == 0)
                {
                    MyBase.Clear(this);
                    Grid_Data();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RbtMultiple_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (Dt.Rows.Count > 0 && !MyParent.View)
                {
                    if (MessageBox.Show("Sure to Clear Grid Detail ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                        {
                            if (Convert.ToInt64(Dt.Rows[i]["Qty"].ToString()) > 0)
                            {
                                DtQty[Convert.ToInt64(Dt.Rows[i]["Slno1"].ToString())] = new DataTable();
                            }
                        }
                        MyBase.Clear(this);
                        Grid_Data();
                    }
                    else
                    {
                        if (RbtMultiple.Checked == true)
                        {
                            RbtSingle.Checked = true;
                            return;
                        }
                        
                    }
                }
                else if (Dt.Rows.Count == 0)
                {
                    MyBase.Clear(this);
                    Grid_Data();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
