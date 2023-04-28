using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Windows.Forms;
using Accounts;

namespace Accounts
{
    public partial class Frm_Floor_FGS_Receipt_Delivery_Entry : Form, Entry
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

        public Frm_Floor_FGS_Receipt_Delivery_Entry()
        {
            InitializeComponent();
        }

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                MyBase.Enable_Controls(this, true);
                DataTable Dth = new DataTable();
                Load_Combo();
                Code = 0;
                Grid_Data();
                DtpDate1.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LblSpecial_Click(object sender, EventArgs e)
        {

        }

        private void Frm_Floor_FGS_Receipt_Delivery_Entry_Load(object sender, EventArgs e)
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

        void Total_Qty()
        {
            try
            {
                TxtTotalQty.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "JO_QTY", "Order_No", "PO_No", "SAMPLE_NO")));
                TxtTotal.Text = String.Format("{0:0}", Convert.ToDouble(MyBase.Sum(ref Grid, "Bal", "Order_No", "PO_No", "SAMPLE_NO")));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Grid_Data()
        {
            try
            {
                if (MyParent._New)
                {
                    if (TxtJONo.Tag.ToString() == String.Empty)
                    {
                        Str = " Select Slno, Order_NO, Po_no, Item, Sample_NO, Size, Jo_Qty, Knit_Prod, Ord_Rec, Received, (Case When Bal < 0 Then 0 Else Bal End)Bal, ";
                        Str = Str + " (Case When Bal_New < 0 Then 0 Else Bal_New End)Bal_New, Order_ID, Sample_ID, JoNO_Master_ID, JoNO_Details_ID, Slno1, Remarks, T ";
                        Str = Str + " From (Select ROW_NUMBER() Over(Order By B.Order_No, A.Po_No, F.Item, D.Sample_No) Slno, B.Order_No, A.Po_No, F.Item, D.Sample_No, ";
                        Str = Str + " E.Size,  A.JO_Qty, Isnull(I.Knit_Prod, 0)Knit_Prod, Isnull(J.Rec_Qty, 0)Ord_Rec, ISNULL(G.Received, 0)Received, ";
                        Str = Str + " (Case When (Isnull(I.Knit_Prod, 0) - ISnull(J.Rec_Qty, 0)) < (A.JO_Qty - ISNULL(G.Received, 0)) Then ";
                        Str = Str + " (Isnull(I.Knit_Prod, 0) - ISnull(J.Rec_Qty, 0)) Else (A.JO_Qty - ISNULL(G.Received, 0)) End)Bal, ";
                        Str = Str + " (Case When (Isnull(I.Knit_Prod, 0) - ISnull(J.Rec_Qty, 0)) < (A.JO_Qty - ISNULL(G.Received, 0)) Then ";
                        Str = Str + " (Isnull(I.Knit_Prod, 0) - ISnull(J.Rec_Qty, 0)) Else (A.JO_Qty - ISNULL(G.Received, 0)) End) Bal_New, ";
                        Str = Str + " A.Order_ID,  A.Sample_ID, A.Master_ID JoNO_Master_ID, A.RowID JoNO_Details_ID, ";
                        Str = Str + " ROW_NUMBER() Over(Order By B.Order_No, A.Po_No, F.Item, D.Sample_No) Slno1, '' Remarks, '' T  From Socks_JobOrder_Details A ";
                        Str = Str + " Left Join Socks_Order_Master B On A.Order_ID = B.RowID ";
                        Str = Str + " Left Join Socks_Order_Details C On B.RowID = C.Master_ID And A.Order_ID = C.Master_ID And A.Sample_ID = C.Sample_ID ";
                        Str = Str + " And A.Po_No = C.PO_No ";
                        Str = Str + " Left Join VFit_Sample_Master D On A.Sample_ID = D.RowID and C.Sample_ID = D.RowID ";
                        Str = Str + " Left Join Size E On D.SizeID = E.SizeID Left Join Item F On D.SampleItemID = F.ItemID ";
                        Str = Str + " Left Join JobOrder_Against_Delivered_FGS(0)G On A.Master_ID = G.JoNO_Master_ID And A.RowID = JoNO_Details_ID ";
                        Str = Str + " Left Join Socks_Joborder_Master H On A.Master_ID = H.RowID ";
                        Str = Str + " Left Join Orderwise_Knit_Prod_Qty()I On H.Unit_Code = I.Unit_Code And B.Order_No = I.Order_No And A.Sample_ID = I.OrderColorID ";
                        Str = Str + " Left Join OrderWise_Fgs_Delivered_Details()J On H.Unit_Code = J.Unit_Code And B.Order_No = J.Order_No And A.Sample_ID = J.OrderColorID ";
                        Str = Str + " Where 1 = 2 ) A1 ";
                    }
                    else
                    {
                        if (!TxtBuyer.Text.ToString().ToUpper().Contains("DECATH") && MyParent.UserName.ToString().ToUpper() == "ADMIN")
                        {
                            Str = " Select Slno, Order_NO, Po_no, Item, Sample_NO, Size, Jo_Qty, Knit_Prod, Ord_Rec, Received, (Case When Bal < 0 Then 0 Else Bal End)Bal, ";
                            Str = Str + " (Case When Bal_New < 0 Then 0 Else Bal_New End)Bal_New, Order_ID, Sample_ID, JoNO_Master_ID, JoNO_Details_ID, Slno1, Remarks, T ";
                            Str = Str + " From ( ";
                            Str = Str + " Select ROW_NUMBER() Over(Order By B.Order_No, A.Po_No, F.Item, D.Sample_No) Slno, B.Order_No, A.Po_No, F.Item, D.Sample_No, ";
                            Str = Str + " E.Size,  A.JO_Qty, Isnull(I.Knit_Prod, 0)Knit_Prod, Isnull(J.Rec_Qty, 0)Ord_Rec, ISNULL(G.Received, 0)Received, ";
                            Str = Str + " (Case When (Isnull(I.Knit_Prod, 0) - ISnull(J.Rec_Qty, 0)) < (A.JO_Qty - ISNULL(G.Received, 0)) Then ";
                            Str = Str + " (Isnull(I.Knit_Prod, 0) - ISnull(J.Rec_Qty, 0)) Else (A.JO_Qty - ISNULL(G.Received, 0)) End)Bal, ";
                            Str = Str + " (Case When (Isnull(I.Knit_Prod, 0) - ISnull(J.Rec_Qty, 0)) < (A.JO_Qty - ISNULL(G.Received, 0)) Then ";
                            Str = Str + " (Isnull(I.Knit_Prod, 0) - ISnull(J.Rec_Qty, 0)) Else (A.JO_Qty - ISNULL(G.Received, 0)) End) Bal_New, ";
                            Str = Str + " A.Order_ID,  A.Sample_ID, A.Master_ID JoNO_Master_ID, A.RowID JoNO_Details_ID, ";
                            Str = Str + " ROW_NUMBER() Over(Order By B.Order_No, A.Po_No, F.Item, D.Sample_No) Slno1, '-' Remarks, '' T From Socks_JobOrder_Details A ";
                            Str = Str + " Left Join Socks_Order_Master B On A.Order_ID = B.RowID ";
                            Str = Str + " Left Join Socks_Order_Details C On B.RowID = C.Master_ID And A.Order_ID = C.Master_ID And A.Sample_ID = C.Sample_ID ";
                            Str = Str + " And A.Po_No = C.PO_No ";
                            Str = Str + " Left Join VFit_Sample_Master D On A.Sample_ID = D.RowID and C.Sample_ID = D.RowID ";
                            Str = Str + " Left Join Size E On D.SizeID = E.SizeID Left Join Item F On D.SampleItemID = F.ItemID ";
                            Str = Str + " Left Join JobOrder_Against_Delivered_FGS(" + TxtJONo.Tag.ToString() + ")G On A.Master_ID = G.JoNO_Master_ID And A.RowID = JoNO_Details_ID ";
                            Str = Str + " Left Join Socks_Joborder_Master H On A.Master_ID = H.RowID ";
                            Str = Str + " Left Join Orderwise_Knit_Prod_Qty_New()I On H.Unit_Code = I.Unit_Code And B.Order_No = I.Order_No And A.Sample_ID = I.OrderColorID ";
                            Str = Str + " Left Join OrderWise_Fgs_Delivered_Details()J On H.Unit_Code = J.Unit_Code And B.Order_No = J.Order_No And A.Sample_ID = J.OrderColorID ";
                            Str = Str + " Where A.Master_ID = " + TxtJONo.Tag.ToString() + ")A1 ";
                        }
                        else
                        {
                            Str = " Select Slno, Order_NO, Po_no, Item, Sample_NO, Size, Jo_Buyer_Qty Jo_Qty, Knit_Prod, Ord_Rec, Received, ";
                            Str = Str + " (Case When Bal < 0 Then 0 Else Bal End)Bal, (Case When Bal_New < 0 Then 0 Else Bal_New End)Bal_New, Order_ID, Sample_ID, ";
                            Str = Str + " JoNO_Master_ID, JoNO_Details_ID, Slno1, Remarks, T From ( ";
                            Str = Str + " Select ROW_NUMBER() Over(Order By A.Order_No, A.Po_No, A.Item, A.Sample_No) Slno, A.Order_No, A.Po_No, A.Item, A.Sample_No, ";
                            Str = Str + " A.Size, A.Conv_Buyer_Qty, A.Conv_Bom_Qty, A.Jo_Qty, A.Jo_Buyer_Qty, Isnull(C.Knit_QTy, 0)Knit_Prod, Isnull(D.Rec_Qty, 0)Ord_Rec, ";
                            Str = Str + " Isnull(B.Received, 0)Received, (Case When (Isnull(C.Knit_QTy, 0) - ISnull(D.Rec_Qty, 0)) < (A.Jo_Buyer_Qty - Isnull(B.Received, 0)) ";
                            Str = Str + " Then (Isnull(C.Knit_QTy, 0) - ISnull(D.Rec_Qty, 0)) Else (A.Jo_Buyer_Qty - Isnull(B.Received, 0)) End) Bal, ";
                            Str = Str + " (Case When (Isnull(C.Knit_QTy, 0) - ISnull(D.Rec_Qty, 0)) < (A.Jo_Buyer_Qty - Isnull(B.Received, 0)) ";
                            Str = Str + " Then (Isnull(C.Knit_QTy, 0) - ISnull(D.Rec_Qty, 0)) Else (A.Jo_Buyer_Qty - Isnull(B.Received, 0)) End) Bal_New, ";
                            Str = Str + " A.Allow_Per, A.Order_ID, A.Sample_ID, A.JoNo_Master_ID, A.JoNo_Details_ID, ROW_NUMBER() Over(Order By A.Order_No, A.Po_No, A.Item, A.Sample_No) Slno1, ";
                            Str = Str + " '-' Remarks, '' T From Jo_Details(" + TxtJONo.Tag.ToString() + ") A Left Join JobOrder_Against_Delivered_FGS(" + TxtJONo.Tag.ToString() + ")B On A.JoNo_Master_ID = B.JoNo_Master_ID And A.JoNo_Details_ID = B.JoNo_Details_ID ";
                            Str = Str + " Left Join JoWise_Knit_Qty(" + TxtJONo.Tag.ToString() + ")C On A.Unit_Code = C.Unit_Code And A.Order_No = C.Order_No And A.Sample_ID = C.Sample_ID ";
                            Str = Str + " Left Join OrderWise_Fgs_Delivered_Details() D On A.Unit_Code = D.Unit_Code And A.Order_No = D.Order_No And A.Sample_ID = D.OrderColorID)A10 ";
                        }
                    }
                }
                else
                {
                    Str = " Select A.Slno, D.Order_No, C.Po_No, H.Item, F.Sample_No, G.Size, C.JO_Qty, Isnull(J.Knit_Prod, 0)Knit_Prod, ";
                    Str = Str + " (Isnull(K.Rec_Qty, 0) - ISnull(L.Rec_Qty_Edit, 0))Ord_Rec, (ISNULL(I.Received, 0) - Isnull(A.Prod_Qty, 0))Received, ";
                    Str = Str + " A.Prod_Qty Bal, ((Case When (Isnull(J.Knit_Prod, 0) - (Isnull(K.Rec_Qty, 0) - ISnull(L.Rec_Qty_Edit, 0))) < (C.JO_Qty - ISNULL(I.Received, 0)) Then ";
                    Str = Str + " (Isnull(J.Knit_Prod, 0) - (Isnull(K.Rec_Qty, 0) - ISnull(L.Rec_Qty_Edit, 0))) Else (C.JO_Qty - ISNULL(I.Received, 0)) End) + A.Prod_Qty)Bal_New, ";
                    Str = Str + " C.Order_ID, C.Sample_ID, A.JoNO_Master_ID, ";
                    Str = Str + " A.JoNO_Details_ID, A.Slno1, Isnull(A.Remarks, '-')Remarks, '' T From Socks_FGS_Delivery_Details A ";
                    Str = Str + " Left Join Socks_JobOrder_Master B On A.JoNO_Master_ID = B.RowID ";
                    Str = Str + " Left Join Socks_JobOrder_Details C On A.JoNO_Details_ID = C.RowID And B.RowID = C.Master_ID And A.JoNO_Master_ID = C.Master_ID ";
                    Str = Str + " Left Join Socks_Order_Master D On C.Order_ID = D.RowID  Left Join Socks_Order_Details E On D.RowID = E.Master_ID ";
                    Str = Str + " And C.Order_ID = E.Master_ID And C.Sample_ID = E.Sample_ID And C.Po_No = E.PO_No ";
                    Str = Str + " Left Join VFit_Sample_Master F On C.Sample_ID = F.RowID and E.Sample_ID = F.RowID Left Join Size G On F.SizeID = G.SizeID ";
                    Str = Str + " Left Join Item H On F.SampleItemID = H.ItemID ";
                    Str = Str + " Left Join JobOrder_Against_Delivered_FGS_All()I On A.JoNO_Master_ID = I.JoNO_Master_ID And A.JoNO_Details_ID = I.JoNO_Details_ID ";
                    Str = Str + " And B.RowID = I.JoNO_Master_ID And C.RowID = I.JoNO_Details_ID ";
                    Str = Str + " Left Join Orderwise_Knit_Prod_Qty_New()J On B.Unit_Code = J.Unit_Code And D.Order_No = J.Order_No And C.Sample_ID = J.OrderColorID ";
                    Str = Str + " Left Join OrderWise_Fgs_Delivered_Details()K On B.Unit_Code = K.Unit_Code And D.Order_No = K.Order_No And C.Sample_ID = K.OrderColorID ";
                    Str = Str + " Left Join (Select B.Order_ID, B.Sample_ID, SUm(Prod_Qty)Rec_Qty_Edit From Socks_FGS_Delivery_Details A ";
                    Str = Str + " Left Join Socks_JobOrder_Details B On A.JoNo_Master_ID = B.Master_ID And A.JoNo_Details_ID = B.RowID ";
                    Str = Str + " Where MasterID = " + Code + " Group By B.Order_ID, B.Sample_ID)L On C.Order_ID = L.Order_ID And C.Sample_ID = L.Sample_ID ";
                    Str = Str + " Where Isnull(A.Acknowledged, 'N') = 'N' And A.MasterID = " + Code;

                }
                MyBase.Load_Data(Str, ref Dt);
                Grid.DataSource = Dt;
                MyBase.Grid_Designing(ref Grid, ref Dt, "Bal_New", "Order_ID", "Sample_ID", "JoNO_Master_ID", "JoNO_Details_ID", "Slno1", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid, "Bal", "Remarks");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 40, 120, 110, 110, 100, 90, 90, 110, 100, 100);

                Grid.Columns["JO_QTY"].HeaderText = "JOQTY";
                Grid.Columns["Ord_Rec"].HeaderText = "Ord_Del";
                Grid.Columns["Received"].HeaderText = "DELIVERED UPTO";
                Grid.Columns["Bal"].HeaderText = "DELIVERED";
                Grid.Columns["JO_QTY"].DefaultCellStyle.Format = "0";
                Grid.Columns["JO_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                Grid.Columns["Received"].DefaultCellStyle.Format = "0";
                Grid.Columns["Bal"].DefaultCellStyle.Format = "0";
                Grid.Columns["Bal_New"].DefaultCellStyle.Format = "0";
                Grid.RowHeadersWidth = 10;

                MyBase.Row_Number(ref Grid);
                Total_Qty();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Load_Combo()
        {
            try
            {
                CmbIssueType.Items.Clear();
                CmbIssueType.Items.Add("Internal");
                CmbIssueType.Items.Add("Supplier");
                CmbIssueType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Save()
        {
            String[] Queries;
            Int32 Array_Index = 0;
            double Line_Flag = 0;
            try
            {
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
                    if (Grid["Bal", i].Value != null)
                    {
                        if (Convert.ToInt64(Grid["Bal", i].Value.ToString()) > 0)
                        {
                            if (Convert.ToInt64(Grid["Bal", i].Value.ToString()) > Convert.ToInt64(Grid["Bal_New", i].Value.ToString()))
                            {
                                MessageBox.Show(" Balance is Invalid in Row " + (i + 1) + "  ", "Gainup");
                                Grid["Bal", i].Value = Grid["Bal_New", i].Value;
                                Grid.CurrentCell = Grid["Bal", i];
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
                    for (int j = 1; j < Dt.Columns.Count - 1; j++)
                    {
                        if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty)
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

                if (MyParent._New)
                {
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Convert.ToInt64(Dt.Rows[i]["Bal"].ToString()) > 0)
                        {
                            if (Fill_Bom_Check(Convert.ToInt64(Dt.Rows[i]["JoNO_Master_ID"].ToString()), Convert.ToInt64(Dt.Rows[i]["JoNO_Details_ID"].ToString())) < 0)
                            {
                                MessageBox.Show("Invalid Qty For Sample No : '" + Dt.Rows[i]["Sample_No"].ToString() + "' ", "Gainup...!");
                                Grid.CurrentCell = Grid["Bal", i];
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
                    if (Convert.ToInt64(Dt.Rows[i]["Bal"].ToString()) > 0)
                    {
                        if (Fill_Bom_Check_Knit_Prod(Convert.ToInt64(TxtUnit.Tag), Dt.Rows[i]["Order_No"].ToString(), Convert.ToInt64(Dt.Rows[i]["Sample_ID"].ToString())) < 0)
                        {
                            MessageBox.Show("Invalid Qty For Order_NO : '" + Dt.Rows[i]["Order_No"].ToString() + "' Sample No : '" + Dt.Rows[i]["Sample_No"].ToString() + "' ", "Gainup...!");
                            for (int j = 0; j <= Dt.Rows.Count - 1; j++)
                            {
                                if (Dt.Rows[i]["Order_No"].ToString() == Dt.Rows[j]["Order_No"].ToString() && Dt.Rows[i]["Sample_ID"].ToString() == Dt.Rows[j]["Sample_ID"].ToString())
                                {
                                    Grid["Bal", j].Value = "0";
                                }
                            }
                            Grid.CurrentCell = Grid["Bal", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                }

                TxtEntryNo.Text = MyBase.MaxOnlyWithoutComp("Socks_FGS_Delivery_Master", "DeliveryNo", String.Empty, String.Empty, 0).ToString();
                Queries = new string[Dt.Rows.Count * 300];

                if (MyParent._New)
                {
                    Queries[Array_Index++] = "Insert into Socks_FGS_Delivery_Master (DeliveryNo, DeliveryDate, JoNO, JoNo_Master_ID, EntryTime, EntrySystem, Remarks, UserCode) values (" + TxtEntryNo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', '" + TxtJONo.Text.ToString() + "', " + TxtJONo.Tag.ToString() + ", Getdate(), Host_Name(), '" + TxtRemarks.Text + "', " + MyParent.UserCode + "); Select Scope_Identity() ";
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_FGS_Delivery_Master", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries[Array_Index++] = "Update Socks_FGS_Delivery_Master Set JoNo = '" + TxtJONo.Text.ToString() + "', JoNO_Master_ID = " + TxtJONo.Tag.ToString() + ", DeliveryDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', Remarks = '" + TxtRemarks.Text + "', UserCode = " + MyParent.UserCode + ", EntryTime = Getdate(), EntrySystem = Host_Name() Where RowID = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("Socks_FGS_Delivery_Master", "EDIT", Code.ToString());
                    Queries[Array_Index++] = "Delete From Socks_FGS_Delivery_Details Where MasterID = " + Code + " And Acknowledged = 'N'";
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToInt64(Dt.Rows[i]["Bal"].ToString()) > 0)
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert Into Socks_FGS_Delivery_Details (MasterId, Slno1, Slno, JoNO_Master_ID, JoNO_Details_ID, JoQty, Prod_Qty, Remarks) values (@@IDENTITY, " + Dt.Rows[i]["SLNO1"].ToString() + ", " + Dt.Rows[i]["SLNO"] + ", " + Dt.Rows[i]["JoNO_Master_ID"] + ", " + Dt.Rows[i]["JoNO_Details_ID"] + ", " + Dt.Rows[i]["Jo_Qty"] + ", " + Dt.Rows[i]["Bal"] + ", '" + Dt.Rows[i]["Remarks"].ToString() + "')";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert Into Socks_FGS_Delivery_Details (MasterId, Slno1, Slno, JoNO_Master_ID, JoNO_Details_ID, JoQty, Prod_Qty, Remarks) values (" + Code + ", " + Dt.Rows[i]["SLNO1"].ToString() + ", " + Dt.Rows[i]["SLNO"] + ", " + Dt.Rows[i]["JoNO_Master_ID"] + ", " + Dt.Rows[i]["JoNO_Details_ID"] + ", " + Dt.Rows[i]["Jo_Qty"] + ", " + Dt.Rows[i]["Bal"] + ", '" + Dt.Rows[i]["Remarks"].ToString() + "')";
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

        Int64 Fill_Bom_Check_Knit_Prod(Int64 Unit_Code, String Order_No, Int64 Sample_ID)
        {
            Int64 Knit_Qty = 0;
            Int64 Ord_Rec = 0;
            Int64 Bal_Qty = 0;
            Int64 Prod_Qty = 0;
            try
            {
                DataTable Dt1 = new DataTable();
                DataTable Dt2 = new DataTable();

                Str = " Select * From Orderwise_Knit_Prod_Qty() Where Unit_Code = " + Unit_Code + " And Order_No = '" + Order_No + "' And OrderColorID = " + Sample_ID + " ";

                MyBase.Load_Data(Str, ref Dt1);

                if (Dt1.Rows.Count > 0)
                {
                    Knit_Qty = Convert.ToInt64(Dt1.Rows[0]["Knit_Prod"].ToString());
                }
                if (Knit_Qty == 0)
                {
                    TxtKnit.Text = "0";
                }
                else
                {
                    TxtKnit.Text = Knit_Qty.ToString();
                }
                if (MyParent._New)
                {
                    Str = " Select * From OrderWise_Fgs_Delivered_Details() Where Unit_Code = " + Unit_Code + " And Order_No = '" + Order_No + "' And OrderColorID = " + Sample_ID + " ";
                }
                else
                {
                    Str = " Select * From OrderWise_Fgs_Delivered_Details_Edit(" + Code + ") Where Unit_Code = " + Unit_Code + " And Order_No = '" + Order_No + "' And OrderColorID = " + Sample_ID + " ";
                }

                MyBase.Load_Data(Str, ref Dt2);

                if (Dt2.Rows.Count > 0)
                {
                    Ord_Rec = Convert.ToInt64(Dt2.Rows[0]["Rec_Qty"].ToString());
                }

                if (Ord_Rec == 0)
                {
                    TxtRec.Text = "0";
                }
                else
                {
                    TxtRec.Text = Ord_Rec.ToString();
                }

                TxtBal.Text = (Knit_Qty - Ord_Rec).ToString();

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToInt64(Dt.Rows[i]["Bal"].ToString()) > 0)
                    {
                        if (Convert.ToInt64(TxtUnit.Tag.ToString()) == Unit_Code && Dt.Rows[i]["Order_No"].ToString() == Order_No && Convert.ToInt64(Dt.Rows[i]["Sample_ID"].ToString()) == Sample_ID)
                        {
                            Prod_Qty = Prod_Qty + Convert.ToInt64(Dt.Rows[i]["Bal"].ToString());
                        }
                    }
                }
                TxtEntered.Text = Prod_Qty.ToString();
                TxtExcess.Text = ((Knit_Qty - Ord_Rec) - Prod_Qty).ToString();
                return ((Knit_Qty - Ord_Rec) - Prod_Qty);
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                return 0;
            }
        }

        Int64 Fill_Bom_Check(Int64 JoNO_Master_ID, Int64 JoNO_Details_ID)
        {
            Int64 Bal_Qty = 0;
            Int64 Prod_Qty = 0;
            try
            {
                DataTable Dt1 = new DataTable();

                if (!TxtBuyer.Text.ToString().ToUpper().Contains("DECATH") && MyParent.UserName.ToString().ToUpper() != "ADMIN")
                {
                    Str = " Select A.JONo, B.Order_ID, B.Sample_ID, B.Po_No, B.JO_Qty, Isnull(C.Prod_Qty,0)Prod_Qty, (B.JO_Qty - Isnull(C.Prod_Qty,0))Bal_Qty, ";
                    Str = Str + " A.RowID JoNO_Master_ID, B.RowID JoNO_Details_ID From Socks_JobOrder_Master A Left Join Socks_JobOrder_Details B On A.RowID = B.Master_ID ";
                    Str = Str + " Left Join (Select JoNO_Master_ID, JoNO_Details_ID, Sum(Prod_Qty)Prod_Qty From Socks_FGS_Delivery_Details ";
                    Str = Str + " Group By JoNO_Master_ID, JoNO_Details_ID)C On A.RowID = C.JoNO_Master_ID And B.RowID = C.JoNO_Details_ID ";
                    Str = Str + " Where A.Print_Out_Taken = 'Y' And A.RowID = " + JoNO_Master_ID + " And B.RowID = " + JoNO_Details_ID + " ";
                }
                else
                {
                    Str = " Select Order_NO, Po_no, Item, Sample_NO, Size, Jo_Buyer_Qty Jo_Qty, Knit_Prod, Ord_Rec, Received, ";
                    Str = Str + " (Case When Bal < 0 Then 0 Else Bal End)Bal_Qty, (Case When Bal_New < 0 Then 0 Else Bal_New End)Bal_New, Order_ID, Sample_ID, ";
                    Str = Str + " JoNO_Master_ID, JoNO_Details_ID From ( ";
                    Str = Str + " Select A.Order_No, A.Po_No, A.Item, A.Sample_No, A.Size, A.Conv_Buyer_Qty, A.Conv_Bom_Qty, A.Jo_Qty, A.Jo_Buyer_Qty, ";
                    Str = Str + " Isnull(C.Knit_QTy, 0)Knit_Prod, Isnull(D.Rec_Qty, 0)Ord_Rec, Isnull(B.Received, 0)Received, ";
                    Str = Str + " (Case When (Isnull(C.Knit_QTy, 0) - ISnull(D.Rec_Qty, 0)) < (A.Jo_Buyer_Qty - Isnull(B.Received, 0))  Then ";
                    Str = Str + " (Isnull(C.Knit_QTy, 0) - ISnull(D.Rec_Qty, 0)) Else (A.Jo_Buyer_Qty - Isnull(B.Received, 0)) End) Bal, ";
                    Str = Str + " (Case When (Isnull(C.Knit_QTy, 0) - ISnull(D.Rec_Qty, 0)) < (A.Jo_Buyer_Qty - Isnull(B.Received, 0))  Then ";
                    Str = Str + " (Isnull(C.Knit_QTy, 0) - ISnull(D.Rec_Qty, 0)) Else (A.Jo_Buyer_Qty - Isnull(B.Received, 0)) End) Bal_New,  A.Allow_Per, ";
                    Str = Str + " A.Order_ID, A.Sample_ID, A.JoNo_Master_ID, A.JoNo_Details_ID ";
                    Str = Str + " From Jo_Details(" + JoNO_Master_ID + ") A Left Join JobOrder_Against_Delivered_FGS(" + JoNO_Master_ID + ")B On A.JoNo_Master_ID = B.JoNo_Master_ID And A.JoNo_Details_ID = B.JoNo_Details_ID ";
                    Str = Str + " Left Join JoWise_Knit_Qty(" + JoNO_Master_ID + ")C On A.Unit_Code = C.Unit_Code And A.Order_No = C.Order_No And A.Sample_ID = C.Sample_ID ";
                    Str = Str + " Left Join OrderWise_Fgs_Delivered_Details() D On A.Unit_Code = D.Unit_Code And A.Order_No = D.Order_No And A.Sample_ID = D.OrderColorID)A10 ";
                    Str = Str + " Where A10.JoNo_Details_ID = " + JoNO_Details_ID + " ";

                }

                MyBase.Load_Data(Str, ref Dt1);

                if (Dt1.Rows.Count > 0)
                {
                    Bal_Qty = Convert.ToInt64(Dt1.Rows[0]["Bal_Qty"].ToString());

                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Convert.ToInt64(Dt.Rows[i]["Bal"].ToString()) > 0)
                        {
                            if (Convert.ToInt64(Dt.Rows[i]["JoNO_Master_ID"].ToString()) == JoNO_Master_ID && Convert.ToInt64(Dt.Rows[i]["JoNO_Details_ID"].ToString()) == JoNO_Details_ID)
                            {
                                Prod_Qty = Prod_Qty + Convert.ToInt64(Dt.Rows[i]["Bal"].ToString());
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

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);

                Str = " Select A.DeliveryNo, A.DeliveryDate, A.Jono, E.Order_No, G.Sample_No, H.Size, D.JO_Qty, B.Prod_Qty, I.LEdgeR_NAme Buyer, ";
                Str = Str + " ISNULL(K.LEdgeR_NAme, J.company_unit) Unit, C.Issue_Type, E.Party_Code Buyer_ID, C.Unit_Code, A.JoNo_Master_ID, A.RowID From Socks_FGS_Delivery_Master A ";
                Str = Str + " Left Join Socks_FGS_Delivery_Details B On A.RowID = B.MasterID Left Join Socks_JobOrder_Master C On A.Jono = C.JONo And B.JoNo_Master_ID = C.RowID ";
                Str = Str + " Left Join Socks_JobOrder_Details D On C.RowID = D.Master_ID And A.JoNo_Master_ID = D.Master_ID And B.JoNo_Master_ID = D.Master_ID And B.JoNo_Details_ID = D.RowID ";
                Str = Str + " Left Join Socks_Order_Master E On D.Order_ID = E.RowID Left Join Socks_Order_Details F On E.RowID = F.Master_ID And D.Sample_ID = F.Sample_ID And D.Po_No = F.Po_No ";
                Str = Str + " Left Join VFit_Sample_Master G On D.Sample_ID = G.RowID And F.Sample_ID = G.RowID Left Join Size H On G.SizeID = H.SizeID ";
                Str = Str + " Left Join Buyer_All_Fn()I On C.Buyer_ID = I.LEdgeR_code Left Join company_unit J On C.Unit_Code = J.company_unitid Left Join Supplier_All_Fn()K On C.Unit_Code = K.LEdgeR_code Where B.Acknowledged = 'N' ";

                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select FGS Receipt Entry - Edit", Str, String.Empty, 90, 100, 120, 120, 90, 90, 90);
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

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                TxtBuyer.Enabled = false;
                Code = Convert.ToInt64(Dr["RowID"]);
                TxtEntryNo.Text = Dr["DeliveryNo"].ToString();
                DtpDate1.Value = Convert.ToDateTime(Dr["DeliveryDate"]);
                TxtJONo.Text = Dr["JONo"].ToString();
                TxtJONo.Tag = Dr["JoNo_Master_ID"].ToString();
                TxtBuyer.Tag = Dr["Buyer_ID"].ToString();
                TxtBuyer.Text = Dr["Buyer"].ToString();
                //Fir = 0;
                //SI = Convert.ToInt32(Dr["Issue_Type"]);
                CmbIssueType.SelectedIndex = Convert.ToInt32(Dr["Issue_Type"]);
                TxtUnit.Text = Dr["Unit"].ToString();
                TxtUnit.Tag = Dr["Unit_Code"].ToString();
                Grid_Data();
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
                MyBase.Clear(this);

                Str = " Select A.DeliveryNo, A.DeliveryDate, A.Jono, E.Order_No, G.Sample_No, H.Size, D.JO_Qty, B.Prod_Qty, I.LEdgeR_NAme Buyer, ";
                Str = Str + " ISNULL(K.LEdgeR_NAme, J.company_unit) Unit, C.Issue_Type, E.Party_Code Buyer_ID, C.Unit_Code, A.JoNo_Master_ID, A.RowID From Socks_FGS_Delivery_Master A ";
                Str = Str + " Left Join Socks_FGS_Delivery_Details B On A.RowID = B.MasterID Left Join Socks_JobOrder_Master C On A.Jono = C.JONo And B.JoNo_Master_ID = C.RowID ";
                Str = Str + " Left Join Socks_JobOrder_Details D On C.RowID = D.Master_ID And A.JoNo_Master_ID = D.Master_ID And B.JoNo_Master_ID = D.Master_ID And B.JoNo_Details_ID = D.RowID ";
                Str = Str + " Left Join Socks_Order_Master E On D.Order_ID = E.RowID Left Join Socks_Order_Details F On E.RowID = F.Master_ID And D.Sample_ID = F.Sample_ID And D.Po_No = F.Po_No ";
                Str = Str + " Left Join VFit_Sample_Master G On D.Sample_ID = G.RowID And F.Sample_ID = G.RowID Left Join Size H On G.SizeID = H.SizeID ";
                Str = Str + " Left Join Buyer_All_Fn()I On C.Buyer_ID = I.LEdgeR_code Left Join company_unit J On C.Unit_Code = J.company_unitid Left Join Supplier_All_Fn()K On C.Unit_Code = K.LEdgeR_code Where B.Acknowledged = 'N' ";

                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select FGS Receipt Entry - Delete", Str, String.Empty, 90, 100, 120, 120, 90, 90, 90);

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
                    //MyBase.Run("Delete from Socks_FGS_Delivery_Details where MasterID = " + Code + " And Acknowledged = 'N' ", " Delete From Socks_FGS_Delivery_Master Where RowID = " + Code);

                    MyBase.Run("Delete from Socks_FGS_Delivery_Details where MasterID = " + Code + " And Acknowledged = 'N' ");
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

        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);

                Str = " Select A.DeliveryNo, A.DeliveryDate, A.Jono, E.Order_No, G.Sample_No, H.Size, D.JO_Qty, B.Prod_Qty, I.LEdgeR_NAme Buyer, ";
                Str = Str + " ISNULL(K.LEdgeR_NAme, J.company_unit) Unit, C.Issue_Type, E.Party_Code Buyer_ID, C.Unit_Code, A.JoNo_Master_ID, A.RowID From Socks_FGS_Delivery_Master A ";
                Str = Str + " Left Join Socks_FGS_Delivery_Details B On A.RowID = B.MasterID Left Join Socks_JobOrder_Master C On A.Jono = C.JONo And B.JoNo_Master_ID = C.RowID ";
                Str = Str + " Left Join Socks_JobOrder_Details D On C.RowID = D.Master_ID And A.JoNo_Master_ID = D.Master_ID And B.JoNo_Master_ID = D.Master_ID And B.JoNo_Details_ID = D.RowID ";
                Str = Str + " Left Join Socks_Order_Master E On D.Order_ID = E.RowID Left Join Socks_Order_Details F On E.RowID = F.Master_ID And D.Sample_ID = F.Sample_ID And D.Po_No = F.Po_No ";
                Str = Str + " Left Join VFit_Sample_Master G On D.Sample_ID = G.RowID And F.Sample_ID = G.RowID Left Join Size H On G.SizeID = H.SizeID ";
                Str = Str + " Left Join Buyer_All_Fn()I On C.Buyer_ID = I.LEdgeR_code Left Join company_unit J On C.Unit_Code = J.company_unitid Left Join Supplier_All_Fn()K On C.Unit_Code = K.LEdgeR_code ";

                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select FGS Receipt Entry - View", Str, String.Empty, 90, 100, 120, 120, 90, 90, 90);
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

        public void Entry_Print()
        {
            try
            {
                //MyParent.View_Browser("MIS_SOCKS_YARNDYEING_GRN", Code);

                //GBLOT.Visible = false;

                //Str = " Select Top 10000000000 U1.Unit_Name, E1.Name Merchandiser, I.Item, C.COlor, S.Size, Sum(ISnull(S3.Indent_Qty,0))Indent_Qty, Sum(ISnull(S5.Issued,0))Issued_Qty, 0 Given From Socks_Sample_Yarn_Indent_Request_Master S1 ";
                //Str = Str + " Left Join Socks_Sample_Yarn_Indent_Request_Details S2 On S1.RowID = S2.MasterID Left Join Socks_Sample_Yarn_Indent_SampleWise_Request_Details S3 On S1.RowId = S3.MasterID And S2.Slno1 = S3.Slno1 ";
                //Str = Str + " Left Join Item I On S3.ItemID = I.ItemID Left Join Color C On S3.ColorID = C.ColorID Left Join Size S On S3.SizeID = S.SizeID Left Join VAAHINI_ERP_GAINUP.Dbo.Unit_Master U1 On S1.UnitCode = U1.RowId ";
                //Str = Str + " Left Join Socks_Store_Sample_Indent_Issued()S5 On S1.RowID = S5.Indent_Master_MasterID And S3.RowID = S5.Indent_Request_Details_RowID And S3.RowID = S5.Indent_Request_Details_RowID And S3.ItemID = S5.ItemID And S3.ColorID = S5.ColorID And S3.SizeID = S5.SizeID ";
                //Str = Str + " Left Join VAAHINI_ERP_GAINUP.Dbo.EmployeeMas E1 On S2.Merchandiser_ID = E1.Emplno ";
                //Str = Str + " Where S1.DeptCode in (104) And S1.RowID =  " + Code + " Group By U1.Unit_Name, E1.Name , I.Item, C.COlor, S.Size  Order By I.Item, C.COlor, S.Size ";
                Str = " Select A.Slno, D.Order_No, C.Po_No, H.Item, F.Sample_No, G.Size, C.JO_Qty, Isnull(J.Knit_Prod, 0)Knit_Prod, ";
                Str = Str + " (Isnull(K.Rec_Qty, 0) - ISnull(L.Rec_Qty_Edit, 0))Ord_Rec, (ISNULL(I.Received, 0) - Isnull(A.Prod_Qty, 0))Received, ";
                Str = Str + " A.Prod_Qty Bal, ((Case When (Isnull(J.Knit_Prod, 0) - (Isnull(K.Rec_Qty, 0) - ISnull(L.Rec_Qty_Edit, 0))) < (C.JO_Qty - ISNULL(I.Received, 0)) Then ";
                Str = Str + " (Isnull(J.Knit_Prod, 0) - (Isnull(K.Rec_Qty, 0) - ISnull(L.Rec_Qty_Edit, 0))) Else (C.JO_Qty - ISNULL(I.Received, 0)) End) + A.Prod_Qty)Bal_New, ";
                Str = Str + " C.Order_ID, C.Sample_ID, A.JoNO_Master_ID, ";
                Str = Str + " A.JoNO_Details_ID, A.Slno1, Isnull(A.Remarks, '-')Remarks, '' T From Socks_FGS_Delivery_Details A ";
                Str = Str + " Left Join Socks_JobOrder_Master B On A.JoNO_Master_ID = B.RowID ";
                Str = Str + " Left Join Socks_JobOrder_Details C On A.JoNO_Details_ID = C.RowID And B.RowID = C.Master_ID And A.JoNO_Master_ID = C.Master_ID ";
                Str = Str + " Left Join Socks_Order_Master D On C.Order_ID = D.RowID  Left Join Socks_Order_Details E On D.RowID = E.Master_ID ";
                Str = Str + " And C.Order_ID = E.Master_ID And C.Sample_ID = E.Sample_ID And C.Po_No = E.PO_No ";
                Str = Str + " Left Join VFit_Sample_Master F On C.Sample_ID = F.RowID and E.Sample_ID = F.RowID Left Join Size G On F.SizeID = G.SizeID ";
                Str = Str + " Left Join Item H On F.SampleItemID = H.ItemID ";
                Str = Str + " Left Join JobOrder_Against_Delivered_FGS_All()I On A.JoNO_Master_ID = I.JoNO_Master_ID And A.JoNO_Details_ID = I.JoNO_Details_ID ";
                Str = Str + " And B.RowID = I.JoNO_Master_ID And C.RowID = I.JoNO_Details_ID ";
                Str = Str + " Left Join Orderwise_Knit_Prod_Qty_New()J On B.Unit_Code = J.Unit_Code And D.Order_No = J.Order_No And C.Sample_ID = J.OrderColorID ";
                Str = Str + " Left Join OrderWise_Fgs_Delivered_Details()K On B.Unit_Code = K.Unit_Code And D.Order_No = K.Order_No And C.Sample_ID = K.OrderColorID ";
                Str = Str + " Left Join (Select B.Order_ID, B.Sample_ID, SUm(Prod_Qty)Rec_Qty_Edit From Socks_FGS_Delivery_Details A ";
                Str = Str + " Left Join Socks_JobOrder_Details B On A.JoNo_Master_ID = B.Master_ID And A.JoNo_Details_ID = B.RowID ";
                Str = Str + " Where MasterID = " + Code + " Group By B.Order_ID, B.Sample_ID)L On C.Order_ID = L.Order_ID And C.Sample_ID = L.Sample_ID ";
                Str = Str + " Where Isnull(A.Acknowledged, 'N') = 'N' And A.MasterID = " + Code;

                //MyBase.Execute_Qry(Str, "Sample_Yarn_Indent_Request");
                MyBase.Execute_Qry(Str, "FGS_DELIVERY_REQUEST");

                //Str = "Select Sno, Grn_No, Item, Color, Size, LotNo, Stock From Sample_Indent_Request_Lot_Details(" + Code + ")";
                //MyBase.Execute_Qry(Str, "Sample_Yarn_Order_Lot_Details");

                //DataTable Dt1 = new DataTable();
                //String Str1 = "Select Getdate()Date1";
                //MyBase.Load_Data(Str1, ref Dt1);

                //DataTable Dt2 = new DataTable();
                //String Str2 = "Select IndentNo, ProductionDate from Socks_Sample_Yarn_Indent_Request_Master Where RowID = " + Code + "";
                //MyBase.Load_Data(Str2, ref Dt2);

                CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Yarn_Indent_Request_Sample.rpt");
                ObjRpt.Load(System.Windows.Forms.Application.StartupPath + "\\RptFGS_Delivery_Request.rpt");
                MyParent.FormulaFill(ref ObjRpt, "Date", String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value).ToString());
                MyParent.FormulaFill(ref ObjRpt, "Unit", TxtUnit.Text.ToString());
                MyParent.FormulaFill(ref ObjRpt, "#No", TxtEntryNo.Text.ToString());
                MyParent.CReport(ref ObjRpt, "FGS Delivery Request..!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Floor_FGS_Receipt_Delivery_Entry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtJONo")
                    {
                        if (TxtJONo.Text.ToString() != String.Empty)
                        {
                            Grid.CurrentCell = Grid["Bal", 0];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                        }
                        else
                        {
                            MessageBox.Show("Select Job Order...!", "Gainup..!");
                            return;
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtTotalQty")
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
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtJONo")
                    {
                        Str = " Select S1.JONo, S3.Order_NO, S3.Sample_No, T.Size, S2.JO_Qty, Isnull (C1.LEdgeR_NAme, C2.Company_Unit) Unit, T.Model_Name, ";
                        Str = Str + " L1.Ledger_Name Buyer, S2.Po_No, S1.JoDate, S1.Buyer_ID, S1.Issue_Type, S1.Unit_Code, Isnull(S1.KnitDate, S1.JoDate)KnitDate, ";
                        Str = Str + " Isnull(S1.PackDate, S1.JoDate)PackDate, S1.RowID Code From Socks_JobOrder_Master S1 ";
                        Str = Str + " Inner Join Socks_JobOrder_Details S2 on S1.RowID = S2.Master_ID Left Join Buyer_All_Fn() L1 on S1.Buyer_ID = L1.Ledger_Code ";
                        Str = Str + " Left Join Supplier_All_Fn() C1 on S1.Unit_Code = C1.LEdgeR_code Left join company_unit C2 On S1.Unit_Code = C2.company_unitid ";
                        Str = Str + " Left join Socks_Order_BOM () S3 on S2.Order_ID = S3.Order_ID and S2.Sample_ID = S3.Sample_ID and S2.Po_No = S3.Po_No ";
                        Str = Str + " Left Join (Select Distinct C.Sample_No, C.RowID, D.Model_Name, E.Size From VFit_Sample_Master C ";
                        Str = Str + " Left Join Socks_Model D on C.ModelID = D.Rowid Left Join Size E On C.Sizeid = E.SizeID)T on S2.Sample_ID = T.RowID Where S1.Print_Out_Taken = 'Y' ";

                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select JO No", Str, String.Empty, 110, 110, 90, 80, 80, 100, 120, 120, 120, 90);
                        if (Dr != null)
                        {
                            TxtBuyer.Enabled = false;
                            TxtUnit.Enabled = false;
                            CmbIssueType.Enabled = false;
                            TxtJONo.Text = Dr["JONo"].ToString();
                            TxtJONo.Tag = Convert.ToInt64(Dr["Code"]);
                            //DtpDate1.Value = Convert.ToDateTime(Dr["JoDate"]);
                            TxtBuyer.Tag = Dr["Buyer_ID"].ToString();
                            TxtBuyer.Text = Dr["Buyer"].ToString();
                            CmbIssueType.SelectedIndex = Convert.ToInt32(Dr["Issue_Type"]);
                            TxtUnit.Text = Dr["Unit"].ToString();
                            TxtUnit.Tag = Dr["Unit_Code"].ToString();
                            Grid_Data();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Floor_FGS_Receipt_Delivery_Entry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == String.Empty)
                {
                }
                else
                {
                    if (this.ActiveControl is TextBox)
                    {
                        e.Handled = true;
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Bal"].Index)
                {
                    MyBase.Valid_Number(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
                {
                    MyBase.Valid_Alpha_Numeric(Txt, e);
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

        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Order_NO"].Index)
                    {
                        //SI = CmbIssueType.SelectedIndex;
                        Dr = Tool.Selection_Tool_Except_New("Description", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Order", "Select Order_No, PO_No, Sample_NO, BOM_Qty, JO_Issued_Qty Issued, Bal_Qty, Order_ID, Sample_ID, (Order_No + '-' + PO_No + '-' + Sample_NO) Description From Socks_JobOrder_Pending () Where Buyer_ID = " + TxtBuyer.Tag.ToString() + " and Bal_Qty > 0", String.Empty, 100, 100, 80, 80, 80, 80);
                        if (Dr != null)
                        {

                            MyBase.Row_Number(ref Grid);
                            Txt.Text = Dr["Order_No"].ToString();
                            Grid["Order_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                            Grid["Order_ID", Grid.CurrentCell.RowIndex].Value = Dr["Order_ID"].ToString();
                            Grid["Sample_No", Grid.CurrentCell.RowIndex].Value = Dr["Sample_No"].ToString();
                            Grid["Sample_ID", Grid.CurrentCell.RowIndex].Value = Dr["Sample_ID"].ToString();
                            Grid["PO_NO", Grid.CurrentCell.RowIndex].Value = Dr["PO_NO"].ToString();
                            Grid["Description", Grid.CurrentCell.RowIndex].Value = Dr["Description"].ToString();
                            Grid["BOM_Qty", Grid.CurrentCell.RowIndex].Value = Dr["BOM_Qty"].ToString();
                            Grid["Issued_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Issued"].ToString();
                            Grid["Bal_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bal_Qty"].ToString();
                            Grid["JO_Qty", Grid.CurrentCell.RowIndex].Value = Dr["Bal_Qty"].ToString();
                            Grid["Remarks", Grid.CurrentCell.RowIndex].Value = "-";
                            Grid["Slno1", Grid.CurrentCell.RowIndex].Value = Max_Slno_Grid().ToString();
                        }
                    }
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Bal"].Index)
                    {
                        if (Grid["Bal", Grid.CurrentCell.RowIndex].Value == null || Grid["Bal", Grid.CurrentCell.RowIndex].Value == DBNull.Value)
                        {
                            if (Grid["JO_Qty", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                            {
                                Grid["Bal", Grid.CurrentCell.RowIndex].Value = "0";
                                return;
                            }
                        }

                        if (Grid["JO_Qty", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                        {
                            if (Convert.ToDouble(Grid["Bal", Grid.CurrentCell.RowIndex].Value) < 0)
                            {
                                e.Handled = true;
                                MessageBox.Show("Invalid Bal Qty ...!", "Gainup");
                                Grid["Bal", Grid.CurrentCell.RowIndex].Value = Grid["Bal_New", Grid.CurrentCell.RowIndex].Value;
                                Grid.CurrentCell = Grid["Bal", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }

                            if (Convert.ToDouble(Grid["Bal", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["Bal_New", Grid.CurrentCell.RowIndex].Value))
                            {
                                e.Handled = true;
                                MessageBox.Show("Bal Qty is greater than Available Qty ...!", "Gainup");
                                Grid["Bal", Grid.CurrentCell.RowIndex].Value = Grid["Bal_New", Grid.CurrentCell.RowIndex].Value;
                                Grid.CurrentCell = Grid["Bal", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }

                            if (Fill_Bom_Check_Knit_Prod(Convert.ToInt64(TxtUnit.Tag), Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString(), Convert.ToInt64(Grid["Sample_ID", Grid.CurrentCell.RowIndex].Value.ToString())) < 0)
                            {
                                e.Handled = true;
                                MessageBox.Show("Bal Qty is greater than Knitting Qty ...!", "Gainup");
                                Grid["Bal", Grid.CurrentCell.RowIndex].Value = 0;
                                Grid.CurrentCell = Grid["Bal", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                            Total_Qty();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Remarks"].Index)
                    {
                        if (Grid["Remarks", Grid.CurrentCell.RowIndex].Value == null || Grid["Remarks", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Grid["Remarks", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
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

        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    e.Handled = true;
                    Total_Qty();
                    TxtTotalQty.Focus();
                }
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
                MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentCell.RowIndex);
                Total_Qty();
                MyBase.Row_Number(ref Grid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void RowsCount()
        {
            try
            {
                if (Grid["Order_No", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    Grid.CurrentCell = Grid["Order_No", Grid.CurrentCell.RowIndex + 1];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
                if (Grid["Jo_Qty", Grid.CurrentCell.RowIndex].Value.ToString() != String.Empty)
                {
                    Grid.CurrentCell = Grid["Order_No", Grid.CurrentCell.RowIndex + 1];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                }
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
                //TxtTotal.Text = MyBase.Sum_With_Three_Digits(ref Grid, "Iss_Qty");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
