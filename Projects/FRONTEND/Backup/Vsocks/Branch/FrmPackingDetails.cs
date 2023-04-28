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
    public partial class FrmPackingDetails : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        DataTable Dt = new DataTable();
        DataRow Dr;
        Int16 PCompCode;
        Int64 Code;
        String[] Queries;
        String Str;
        TextBox Txt = null;
        public FrmPackingDetails()
        {
            InitializeComponent();
        }


        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                DtpFDate.Focus();
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
                Total();
                if (TxtProcess.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Process No", "Gainup");
                    TxtProcess.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtRemarks.Text.ToString() == String.Empty)
                {
                    TxtRemarks.Text = ".";
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 9; j++)
                    {
                        if (TxtProcess.Tag.ToString() != "3" && TxtProcess.Tag.ToString() != "4")
                        {
                            if (Convert.ToDouble(Grid["PACKED", i].Value) > Convert.ToDouble(Grid["PENDING", i].Value))
                            {
                                MessageBox.Show("Invalid Packed..!", "Gainup");
                                Grid["PACKED", i].Value = 0.00;
                                Grid["KGS", i].Value = 0.00;
                                Grid.CurrentCell = Grid["PACKED", i];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                        }
                        if (Grid[j, i].Value == DBNull.Value)
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

                if (TxtTotal.Text.Trim() == string.Empty || Convert.ToDouble(TxtTotal.Text) == 0 || TxtKgs.Text.Trim() == string.Empty || Convert.ToDouble(TxtKgs.Text) == 0)
                {
                    MessageBox.Show("Invalid Packed", "Gainup");
                    TxtTotal.Focus();
                    MyParent.Save_Error = true;
                    return;
                }


                if (MyParent._New)
                {
                    Queries = new String[Dt.Rows.Count + 3];
                    DataTable TDt = new DataTable();
                    MyBase.Load_Data("Select (Isnull(Max(ENo), 0) + 1) No From Vaahini_Erp_Gainup.Dbo.Packing_Master Where Company_Code = " + MyParent.CompCode + " ", ref TDt);
                    TxtENo.Text = TDt.Rows[0][0].ToString();
                    Queries[Array_Index++] = "Insert Into Vaahini_Erp_Gainup.Dbo.Packing_Master (ENo, Effect_Date, Proc_Type, FromDate, ToDate, Tot_Qty, Tot_Kgs, Remarks, Company_Code, Year_Code) Values (" + TxtENo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtProcess.Tag + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', " + Convert.ToDouble(TxtTotal.Text) + ", " + Convert.ToDouble(TxtKgs.Text) + ", '" + TxtRemarks.Text + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "'); Select Scope_Identity()";
                }
                else
                {
                    Queries = new String[Dt.Rows.Count + 3];
                    Queries[Array_Index++] = "Update Vaahini_Erp_Gainup.Dbo.Packing_Master Set  Tot_Qty= " + Convert.ToDouble(TxtTotal.Text) + ", Tot_Kgs = " + Convert.ToDouble(TxtKgs.Text) + ", Remarks = '" + TxtRemarks.Text + "'  Where Rowid = " + Code;
                    Queries[Array_Index++] = "Delete From Vaahini_Erp_Gainup.Dbo.Packing_Details Where Master_id = " + Code;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Grid["PACKED", i].Value) > 0)
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Packing_Details (Master_ID, ProdDate, WeightCode, Packed, SNo, CountCode, TypeCode)  Values (@@IDENTITY, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', " + Grid["WeightCode", i].Value + ", " + Grid["PACKED", i].Value + ", " + Grid["SNO", i].Value + ", " + Grid["COUNTCODE", i].Value + ", " + Grid["TYPECODE", i].Value + ")";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Packing_Details (Master_ID, ProdDate, WeightCode, Packed, SNo, CountCode, TypeCode)  Values (" + Code + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', " + Grid["WeightCode", i].Value + ", " + Grid["PACKED", i].Value + ", " + Grid["SNO", i].Value + ", " + Grid["COUNTCODE", i].Value + ", " + Grid["TYPECODE", i].Value + ")";
                        }
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

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Packing Details - Edit", " Select Distinct C.Name Process_Name, A.FromDate, A.Tot_Qty, A.Tot_Kgs, A.ENo, A.Effect_Date, A.ToDate, A.Remarks, A.Proc_Type, A.RowID  From Vaahini_Erp_Gainup.Dbo.Packing_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Packing_Details  B On A.RowID = B.Master_ID Inner join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_Type = C.RowID Where A.Approval_Status = 'N' and Company_Code = " + MyParent.CompCode + "  and FromDate >= DATEAdd(DD, Case When " + MyParent.UserCode + " = 1 Then -300 Else -2 End,GetDate()) and A.Proc_Type in (12) ORder by ENo DEsc", string.Empty, 160, 100, 100, 120, 80, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    TxtTotal.Text = Dr["Tot_Qty"].ToString();
                    TxtKgs.Text = Dr["Tot_Kgs"].ToString();
                    TxtTotal.Focus();
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
                Code = Convert.ToInt64(Dr["RowId"]);
                DtpDate.Value = Convert.ToDateTime(Dr["Effect_Date"]);
                DtpFDate.Value = Convert.ToDateTime(Dr["FromDate"]);
                DtpTDate.Value = Convert.ToDateTime(Dr["ToDate"]);
                TxtProcess.Text = Dr["Process_Name"].ToString();
                TxtProcess.Tag = Dr["Proc_Type"].ToString();
                TxtENo.Text = Dr["ENo"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();
                TxtTotal.Text = Dr["Tot_Qty"].ToString();
                TxtKgs.Text = Dr["Tot_Kgs"].ToString();
                Grid_Data();
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
                    Txt.TextChanged += new EventHandler(Txt_TextChanged);
                    Txt.Leave += new EventHandler(Txt_Leave);
                }
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PACKED"].Index)
                {
                    if (Grid["PACKED", Grid.CurrentCell.RowIndex].Value == null || Grid["PACKED", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["PACKED", Grid.CurrentCell.RowIndex].Value) == 0)
                    {
                        Grid["PACKED", Grid.CurrentCell.RowIndex].Value = "0";
                    }
                    else
                    {
                        Grid["KGS", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["PACKED", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["WEIGHT", Grid.CurrentCell.RowIndex].Value));
                    }
                }
                Total();
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["COUNTNAME"].Index)
                    {
                        if (TxtProcess.Tag.ToString() == "1")
                        {
//                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select C.ActualCount + '`s ' + D.ShortName + '(' + B.BagWeight + ')' COUNTNAME,  Sum(acthank) - Dbo.[Get_Packed_Contract]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', A.CountCode, EmplNo, EmplNo2) PENDING, 0 PACKED , 0.000 KGS, Sum(acthank) PACKED_PROD, Emplno2 WEIGHTCODE, C.ActualCount, B.BagWeight, D.CountType, A.countcode, A.EMPLNO TypeCode, Cast(B.BagWeight as Numeric(20,3)) WEIGHT, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' PRODDATE  From TrnPrdSum A Inner Join mas_actualcount C On A.countcode = C.actualcode   Inner Join mas_bagweight B On A.emplno2 = B.weightcode  Inner Join mas_counttype D On A.emplno = D.typecode  LEft Join Packing_Details E On A.proddate = E.ProdDate and A.countcode = E.CountCode and A.EMPLNO = E.TypeCode and A.emplno2 = E.WeightCode LEft Join Packing_MAster F On E.MAster_ID = F.RowID and F.Proc_Type =1  Where A.Compcode = (Case When " + MyParent.CompCode + " = 3 Then 5 Else 1 End)  and A.Proddate Between '01-jan-2013' and '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "'  and DeptCode = 90 and F.Rowid Is Null  Group By  Emplno2, bagweight, C.ActualCount, D.CountType, A.countcode, A.EMPLNO, D.ShortName Having  Sum(acthank) - Dbo.[Get_Packed_Contract]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', A.CountCode, EmplNo, EmplNo2)  > 0 Order by  C.ActualCount + '`s ' + D.ShortName + '(' + B.BagWeight + ')' ", string.Empty, 200, 100);
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select C.ActualCount + '`s ' + D.ShortName + '(' + B.BagWeight + ')' COUNTNAME,  Cast(Sum(acthank) - Dbo.[Get_Packed_Contract]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', A.CountCode, EmplNo, EmplNo2) as Numeric(20,2)) PENDING, 0 PACKED , 0.000 KGS, Sum(acthank) PACKED_PROD, Emplno2 WEIGHTCODE, C.ActualCount, B.BagWeight, D.CountType, A.countcode, A.EMPLNO TypeCode, Cast(B.BagWeight as Numeric(20,3)) WEIGHT, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' PRODDATE  From TrnPrdSum A Inner Join mas_actualcount C On A.countcode = C.actualcode   Inner Join mas_bagweight B On A.emplno2 = B.weightcode  Inner Join mas_counttype D On A.emplno = D.typecode  Where A.Compcode = (Case When " + MyParent.CompCode + " = 3 Then 5 Else 1 End)  and A.Proddate Between '01-jan-2013' and '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "'  and DeptCode = 90  Group By  Emplno2, bagweight, C.ActualCount, D.CountType, A.countcode, A.EMPLNO, D.ShortName Having  Cast(Sum(acthank) - Dbo.[Get_Packed_Contract]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', A.CountCode, EmplNo, EmplNo2) as Numeric(20,2))  > 0 Order by  C.ActualCount , D.ShortName , B.BagWeight ", string.Empty, 200, 100);
                        }
                        //else if (TxtProcess.Tag.ToString() == "2")
                        //{
                        //    Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select C.ActualCount + '`s ' + E.ShortName + '(' + D.BagWeight + ')' COUNTNAME,  Sum(Packed) - Dbo.[Get_Shifted_Contract]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', B.CountCode, B.TypeCode , B.WeightCode) PENDING, 0 PACKED , 0.000 KGS, Sum(Packed) PACKED_PROD, B.WEIGHTCODE, C.ActualCount, D.BagWeight, E.CountType, B.countcode, B.TypeCode, Cast(D.BagWeight as Numeric(20,3)) WEIGHT, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' PRODDATE  From Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID and A.Proc_Type = 1  Inner Join mas_actualcount C On B.countcode = C.actualcode   Inner Join mas_bagweight D On B.WeightCode  = D.weightcode  Inner Join mas_counttype E On B.TypeCode  = E.typecode Group By C.actualcount, E.shortname, D.bagweight,  B.CountCode, B.TypeCode, B.WeightCode, E.counttype Having Sum(Packed) - Dbo.[Get_Shifted_Contract]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', B.CountCode, B.TypeCode , B.WeightCode) > 0  Order by C.ActualCount + '`s ' + E.ShortName + '(' + D.BagWeight + ')' ", string.Empty, 200, 100);
                        //}
                        else if (TxtProcess.Tag.ToString() == "3")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select Distinct A.Name COUNTNAME, A1.Kgs PENDING, A1.Kgs PACKED, 0.000 KGS, 0.000 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight,  '' CountType, A.RowID  countcode, 0 TypeCode, 1.00 WEIGHT, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' PRODDATE  From Waste_Item_Master A Inner Join Waste_Receipt_FN(" + MyParent.CompCode + ") A1 On A.RowID = A1.WasCode  Inner Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 Left join Packing_Details B On A.RowID = B.CountCode and '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' = B.ProdDate LEft join Packing_Master C On B.Master_ID = C.RowID and C.Proc_Type in (3) and C.Company_Code = " + MyParent.CompCode + " Where C.RowID is Null and A1.WDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "'", string.Empty, 250, 100);
                        }
                        else if (TxtProcess.Tag.ToString() == "4")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select Distinct A.Name COUNTNAME, A1.Kgs PENDING, A1.Kgs PACKED, 0.000 KGS, 0.000 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight,  '' CountType, A.RowID  countcode, 0 TypeCode, 1.00 WEIGHT, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' PRODDATE  From Waste_Item_Master A Inner Join Waste_Receipt_FN(" + MyParent.CompCode + ") A1 On A.RowID = A1.WasCode  Inner Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 Left join Packing_Details B On A.RowID = B.CountCode and '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' = B.ProdDate LEft join Packing_Master C On B.Master_ID = C.RowID and C.Proc_Type in (4) and C.Company_Code = " + MyParent.CompCode + " Where C.RowID is Null and A1.WDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and A.RowID in (5,30)", string.Empty, 250, 100);
                        }
                        else if (TxtProcess.Tag.ToString() == "5")
                        {
                            if (MyParent.CompCode == 3)
                            {
                                Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "SELECT MIXMASTER.MIXNO COUNTNAME, SUM(MIXDETAIL.KGS) PENDING, SUM(MIXDETAIL.KGS) PACKED, SUM(MIXDETAIL.KGS) KGS, 0 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight, '' CountType, Cast(MIXMASTER.MIXNO as Numeric(20)) CountCode, 0 TypeCode, D.Weight, MIXMASTER.MIXDATE PRODDATE FROM MIXMASTER Left Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 LEFT JOIN (MIXDETAIL LEFT JOIN (SELECT  DISTINCT LOTNO, SUM(BALES) BALES, COMPCODE, VARCODE  FROM LOTMASTER GROUP BY LOTNO, VARCODE, COMPCODE) LOTMASTER ON MIXDETAIL.LOTNO = LOTMASTER.LOTNO) ON MIXMASTER.MIXNO = MIXDETAIL.MIXNO WHERE MIXDETAIL.COMPCODE = (Case When " + MyParent.CompCode + " = 3 Then 6 Else 1 End) AND  LOTMASTER.COMPCODE = (Case When " + MyParent.CompCode + " = 3 Then 6 Else 1 End) AND  MIXMASTER.COMPCODE = (Case When " + MyParent.CompCode + " = 3 Then 6 Else 1 End) AND  MIXMASTER.MIXDATE ='" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "'  GROUP BY MIXMASTER.MIXDATE, MIXMASTER.MIXNO, D.Code, D.Weight  ORDER BY MIXMASTER.MIXDATE, MIXMASTER.MIXNO", string.Empty, 80, 100, 100, 100);
                            }
                            else
                            {
                                Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "SELECT MIXMASTER.MIXNO COUNTNAME, COUNT(MIXMASTER.IBALES) PENDING, COUNT(MIXMASTER.IBALES) PACKED, SUM(MIXDETAIL.KGS) KGS, 0 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight, '' CountType, Cast(MIXMASTER.MIXNO as Numeric(20)) CountCode, 0 TypeCode, D.Weight, MIXMASTER.MIXDATE PRODDATE FROM MIXMASTER Left Join CountWeight(Case When " + MyParent.CompCode + " = 3 Then 6 Else 1 End) D On CAst(D.Weight as Numeric(3)) = 1 LEFT JOIN (MIXDETAIL LEFT JOIN (SELECT  DISTINCT LOTNO, SUM(BALES) BALES, COMPCODE, VARCODE  FROM LOTMASTER GROUP BY LOTNO, VARCODE, COMPCODE) LOTMASTER ON MIXDETAIL.LOTNO = LOTMASTER.LOTNO) ON MIXMASTER.MIXNO = MIXDETAIL.MIXNO WHERE MIXDETAIL.COMPCODE = (Case When " + MyParent.CompCode + " = 3 Then 6 Else 1 End) AND  LOTMASTER.COMPCODE = (Case When " + MyParent.CompCode + " = 3 Then 6 Else 1 End) AND  MIXMASTER.COMPCODE = (Case When " + MyParent.CompCode + " = 3 Then 6 Else 1 End) AND  MIXMASTER.MIXDATE ='" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "'  GROUP BY MIXMASTER.MIXDATE, MIXMASTER.MIXNO, D.Code, D.Weight  ORDER BY MIXMASTER.MIXDATE, MIXMASTER.MIXNO", string.Empty, 80, 100, 100, 100);
                            }
                        }
                        else if (TxtProcess.Tag.ToString() == "6")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select A.StoppageName COUNTNAME, Count(MAcCode) PENDING ,   SUM(StopMins) TotMins, Count(MAcCode) PACKED, Count(MAcCode) KGS, 0 PACKED_PROD,  D.Code WeightCode, 0 ActualCount, D.Weight BagWeight, '' CountType, B.StoppageCode CountCode, 0 TypeCode, D.Weight, B.ProdDate PRODDATE From MasStoppage A Inner Join TRNSTOPPAGE B On A.StoppageCode = B.STOPPAGECODE and A.compcode = B.compcode Left Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 Where A.Bill_Mode = 'Y' and B.ProdDate =  '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and B.CompCode = Case When " + MyParent.CompCode + " = 3 Then 5 Else " + MyParent.CompCode + " End Group By A.StoppageName, B.StoppageCode, D.Weight, D.Code, B.ProdDate Union All Select StoppageName COUNTNAME, 10000 PENDING, 0 TOTMINS, 0 PACKED, 0 KGS, 0 PACKED_PROD, Case When " + MyParent.CompCode + " = 3 Then 25 Else 21 End  WeightCode, 0 ActualCount, 1 BagWeight, '' CountType, StoppageCode CountCode, 0 TypeCode, D.Weight Weight, GETDATE() PRODDATE from MasStoppage Left Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 Where compcode = Case When " + MyParent.CompCode + " = 3 Then 5 Else " + MyParent.CompCode + " End and Bill_Mode = 'Y'  and Bill_Mode_Stop = 'Y'", string.Empty, 200, 100, 100);
                        }                                               
                        else if (TxtProcess.Tag.ToString() == "8")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "SELECT LOTNO COUNTNAME, SUM(BALES) PENDING, SUM(BALES) PACKED, SUM(BALES) KGS, 0 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight, '' CountType, Cast(LOTNO1 as Numeric(20)) CountCode, 0 TypeCode, D.Weight, LOTDATE PRODDATE FROM LOTMASTER Left Join CountWeight(Case When " + MyParent.CompCode + " = 3 Then 6 Else 1 End) D On CAst(D.Weight as Numeric(3)) = 1 WHERE COMPCODE = (Case When " + MyParent.CompCode + " = 3 Then 6 Else 1 End) AND  LOTDATE ='" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "'  GROUP BY LOTNO, LOTNO1, D.Code, D.Weight, LOTDATE  ORDER BY LOTDATE, LOTNO", string.Empty, 80, 100, 100, 100);
                        }
                        else if (TxtProcess.Tag.ToString() == "9")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "SELECT COUNT COUNTNAME, QTY PENDING, QTY PACKED, KGS, 0 PACKED_PROD, CountWeightCode WeightCode, Actual_Count ActualCount, Weight BagWeight, CountType CountType, CountCode, COuntTypeCode TypeCode, Weight, Invoice_Date PRODDATE From Packed_Contract_Sales(" + MyParent.CompCode + ") Where Invoice_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' Order by Count ", string.Empty, 120, 100, 100, 100);
                        }
                        else if (TxtProcess.Tag.ToString() == "10")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select Distinct A.Name COUNTNAME, A1.Bales PENDING, A1.Bales PACKED, 0.000 KGS, 0.000 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight,  '' CountType, A.RowID  countcode, 0 TypeCode, 1.00 WEIGHT, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' PRODDATE  From Waste_Item_Master A Inner Join Waste_Receipt_FN(" + MyParent.CompCode + ") A1 On A.RowID = A1.WasCode  Inner Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 Left join Packing_Details B On A.RowID = B.CountCode and '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' = B.ProdDate LEft join Packing_Master C On B.Master_ID = C.RowID and C.Proc_Type in (" + TxtProcess.Tag + ") and C.Company_Code = " + MyParent.CompCode + " Where C.RowID is Null and A1.WDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "'", string.Empty, 250, 100);
                        }                        
                        else if (TxtProcess.Tag.ToString() == "12")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select Name COUNTNAME, 10000 PENDING,  0 PACKED, 0 KGS, 0 PACKED_PROD, Case When " + MyParent.CompCode + " = 3 Then 25 Else 21 End  WeightCode, 0 ActualCount, 1 BagWeight, '' CountType, RowID CountCode, 0 TypeCode, D.Weight Weight, GETDATE() PRODDATE from Vaahini_Erp_Gainup.Dbo.Loading_Item_Name_Master Left Join Vaahini_Erp_Gainup.Dbo.CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 ", string.Empty, 250, 100);
                        }
                        else if (TxtProcess.Tag.ToString() == "13")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select Distinct A.Name COUNTNAME, A1.Qty PENDING, A1.Qty PACKED, 0.000 KGS, 0.000 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight,  '' CountType, A.RowID  countcode, 0 TypeCode, 1.00 WEIGHT, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' PRODDATE  From Waste_Item_Master A Inner Join Waste_Sales_To_Irulappa_Contract(" + MyParent.CompCode + ") A1 On A.RowID = A1.WasCode  Inner Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 Left join Packing_Details B On A.RowID = B.CountCode and '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' = B.ProdDate LEft join Packing_Master C On B.Master_ID = C.RowID and C.Proc_Type in (" + TxtProcess.Tag + ") and C.Company_Code = " + MyParent.CompCode + " Where C.RowID is Null and A1.Invoice_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' ", string.Empty, 250, 100);
                        }                        
                        else
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select A.Name COUNTNAME, 0.000 PENDING, 0.000 PACKED, 0.000 KGS, 0.000 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight,  '' CountType, A.RowID  countcode, 0 TypeCode, 1.00 WEIGHT, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' PRODDATE  From Vaahini_Erp_Gainup.Dbo.Waste_Item_Master A Inner Join Vaahini_Erp_Gainup.Dbo.CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 Left join Vaahini_Erp_Gainup.Dbo.Packing_Details B On A.RowID = B.CountCode and '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' = B.ProdDate LEft join Vaahini_Erp_Gainup.Dbo.Packing_Master C On B.Master_ID = C.RowID and C.Proc_Type in (3,4) Where C.RowID is Null", string.Empty, 200, 100);
                        }                        
                        if (Dr != null)
                        {
                            Grid["COUNTNAME", Grid.CurrentCell.RowIndex].Value = Dr["COUNTNAME"].ToString();
                            Grid["PENDING", Grid.CurrentCell.RowIndex].Value = Dr["PENDING"].ToString();
                            Grid["PACKED", Grid.CurrentCell.RowIndex].Value = Dr["PACKED"].ToString();
                            Grid["KGS", Grid.CurrentCell.RowIndex].Value = Dr["KGS"].ToString();
                            Grid["PACKED_PROD", Grid.CurrentCell.RowIndex].Value = Dr["PACKED_PROD"].ToString();
                            Grid["WEIGHTCODE", Grid.CurrentCell.RowIndex].Value = Dr["WEIGHTCODE"].ToString();
                            Grid["ActualCount", Grid.CurrentCell.RowIndex].Value = Dr["ActualCount"].ToString();
                            Grid["BagWeight", Grid.CurrentCell.RowIndex].Value = Dr["BagWeight"].ToString();
                            Grid["CountType", Grid.CurrentCell.RowIndex].Value = Dr["CountType"].ToString();
                            Grid["countcode", Grid.CurrentCell.RowIndex].Value = Dr["countcode"].ToString();
                            Grid["TypeCode", Grid.CurrentCell.RowIndex].Value = Dr["TypeCode"].ToString();
                            Grid["PRODDATE", Grid.CurrentCell.RowIndex].Value = Dr["PRODDATE"].ToString();
                            Grid["WEIGHT", Grid.CurrentCell.RowIndex].Value = Dr["WEIGHT"].ToString();
                            Txt.Text = Dr["COUNTNAME"].ToString();
                        }
                    }
                    //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["WEIGHT"].Index)
                    //{
                    //    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "WEIGHT", "Select Weight, Sum(ClQty) Qty, Cast(Weight * Sum(ClQty) as Numeric(20,3)) Kgs, Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "',B.Process_ID,B.Weight_Code) Rate, Cast(Cast(Weight * Sum(ClQty) as Numeric(20,3)) * Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "',B.Process_ID,B.Weight_Code) as Numeric(22,2)) Amount From RG1_Yarn(" + MyParent.CompCode + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "') A Inner Join Contract_Process_Rate B On A.CountWeightCode = B.Weight_Code and B.Process_ID = " + TxtProcess.Tag + " Inner Join Contract_Process_Name C On B.Process_ID = C.RowID and C.RowID = " + TxtProcess.Tag + " Group By Weight,  C.Name , B.Process_ID, B.Weight_Code ", string.Empty, 120, 100, 100, 100, 120);
                    //    if (Dr != null)
                    //    {
                    //        Grid["WEIGHT", Grid.CurrentCell.RowIndex].Value = Dr["Weight"].ToString();
                    //        Grid["PENDING", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["Qty"].ToString());
                    //        Grid["KGS", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["Kgs"].ToString());
                    //        Grid["RATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["Rate"].ToString());
                    //        Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["Amount"].ToString());
                    //        Grid["PACKED1", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["Qty"].ToString());
                    //        Txt.Text = Dr["Weight"].ToString();
                    //    }
                    //}
                }
                Total();
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
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
                //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PACKED"].Index)
                //{
                //    if (Grid["PACKED", Grid.CurrentCell.RowIndex].Value == null || Grid["PACKED", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["PACKED", Grid.CurrentCell.RowIndex].Value) == 0)
                //    {
                //        Grid["PACKED", Grid.CurrentCell.RowIndex].Value = "0";
                //    }
                //    else
                //    {
                //        if (Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0)
                //        {
                //            Grid["PACKED", Grid.CurrentCell.RowIndex].Value = "0.00";
                //        }
                //        else
                //        {
                //            if (Grid["PACKED", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["PENDING", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                //            {
                //                Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["KGS", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value));
                //            }
                //        }
                //    }
                //}
                Total();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Total()
        {
            Double Amt = 0.0;
            Double TaxAmt = 0.0;
            try
            {
                TxtTotal.Text = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "PACKED", "WEIGHT")));
                TxtKgs.Text = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "KGS", "WEIGHT")));
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PACKED"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
                }
                else
                {
                    MyBase.Valid_Null(Txt, e);
                }
                Total();
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

        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentRow.Index);
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
                Total();
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
                    //if (TxtProcess.Tag.ToString() != "3" && TxtProcess.Tag.ToString() != "4")
                    //{
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PACKED"].Index)
                    {
                        if (Grid["PENDING", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                        {
                            if (Convert.ToDouble(Grid["PACKED", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["PENDING", Grid.CurrentCell.RowIndex].Value))
                            {
                                MessageBox.Show("Invalid Packed..!", "Gainup");
                                Grid["PACKED", Grid.CurrentCell.RowIndex].Value = 0.00;
                                Grid["KGS", Grid.CurrentCell.RowIndex].Value = 0.00;
                                Grid.CurrentCell = Grid["PACKED", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                e.Handled = true;
                                return;
                            }
                        }
                    }
                    //}
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
                    Total();
                    TxtTotal.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void DtpFDate_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(DtpFDate.Value) > MyBase.GetServerDateTime())
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DtpFDate.Value = MyBase.GetServerDateTime();
                    TxtProcess.Text = "";
                    DtpFDate.Focus();
                    return;
                }
                if (MyBase.Date_Difference_In_Days(Convert.ToDateTime(DtpFDate.Value), MyBase.GetServerDateTime()) > 2 && MyParent.UserCode != 1)
                {
                    MessageBox.Show("Date Locked, Only 2 Days Allowed From Current Date", "Gainup");
                    DtpFDate.Value = MyBase.GetServerDateTime();
                    TxtProcess.Text = "";
                    DtpFDate.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmPackingDetails_Load(object sender, EventArgs e)
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

        public void Entry_Print()
        {
            try
            {

                CrystalDecisions.CrystalReports.Engine.ReportDocument ORpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                if (TxtProcess.Tag.ToString() == "1")
                {
                    Str = "Select Top 10000000000 A.ENO, A.FROMDATE, C.Name PROCESS,  B.SNO, F.ActualCount + '`s ' + G.ShortName + '(' + D.BagWeight + ')' COUNTNAME,  E.PACKED_PROD - Dbo.[Get_Packed_Contract]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', B.CountCode, B.TypeCode, B.WeightCode)  PENDING , B.PACKED, Cast(B.PACKED * Cast(D.BagWeight as Numeric(20,3)) as Numeric(25,3)) KGS,   E.PACKED_PROD, B.WEIGHTCODE, F.ActualCount , D.BagWeight, G.CountType,   B.CountCode, B.TypeCode, Cast(D.BagWeight as Numeric(20,3)) WEIGHT, B.PRODDATE From Packing_Master A  Inner Join Packing_Details  B On A.RowID = B.Master_ID Inner join Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join Mas_BagWeight D On B.WeightCode = D.WeightCode Inner Join Pack_Prod_UpToDate('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', " + MyParent.CompCode + ") E On B.ProdDate = E.ProdDate and B.WeightCode = E.WeightCode and B.TypeCode = E.TypeCode and E.CountCode = B.CountCode Inner Join mas_actualcount F On B.countcode = F.actualcode   Inner Join mas_counttype G On B.TypeCode = G.typecode Where  Master_ID = " + Code + " Order By B.SNo  ";
                }
                else if (TxtProcess.Tag.ToString() == "2")
                {
                    Str = "Select Top 10000000000 A.ENO, A.FROMDATE, C.Name PROCESS,  B.SNO, F.ActualCount + '`s ' + G.ShortName + '(' + D.BagWeight + ')' COUNTNAME,  E.PACKED_PROD - Dbo.[Get_Shifted_Contract]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', B.CountCode, B.TypeCode, B.WeightCode)  PENDING , B.PACKED, Cast(B.PACKED * Cast(D.BagWeight as Numeric(20,3)) as Numeric(25,3)) KGS,   E.PACKED_PROD, B.WEIGHTCODE, F.ActualCount , D.BagWeight, G.CountType,   B.CountCode, B.TypeCode, Cast(D.BagWeight as Numeric(20,3)) WEIGHT, B.PRODDATE From Packing_Master A  Inner Join Packing_Details  B On A.RowID = B.Master_ID Inner join Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join Mas_BagWeight D On B.WeightCode = D.WeightCode Inner Join Packed_UpToDate_Fro_Shift('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', " + MyParent.CompCode + ") E On B.ProdDate = E.ProdDate and B.WeightCode = E.WeightCode and B.TypeCode = E.TypeCode and E.CountCode = B.CountCode Inner Join mas_actualcount F On B.countcode = F.actualcode   Inner Join mas_counttype G On B.TypeCode = G.typecode Where  Master_ID = " + Code + " Order By B.SNo  ";
                }
                else if (TxtProcess.Tag.ToString() == "3" || TxtProcess.Tag.ToString() == "4")
                {
                    Str = "Select Top 10000000000  A.ENO, A.FROMDATE, C.Name PROCESS,  B.SNO, E.Name COUNTNAME, B.PACKED PENDING , B.PACKED, Cast(B.PACKED * Cast(D.Weight as Numeric(20,3)) as Numeric(25,3)) KGS,   0.000 PACKED_PROD, B.WEIGHTCODE, B.CountCode ActualCount , D.Weight BagWeight, '' CountType,   B.CountCode, B.TypeCode, Cast(D.Weight as Numeric(20,3)) WEIGHT, B.PRODDATE From Packing_Master A  Inner Join Packing_Details  B On A.RowID = B.Master_ID Inner join Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join CountWeight(" + MyParent.CompCode + ") D On B.WeightCode = D.Code Inner Join Waste_Item_Master  E On B.CountCode = E.RowID  Where  Master_ID = " + Code + " Order By B.SNo  ";
                }
                else if (TxtProcess.Tag.ToString() == "5")
                {
                    Str = "SELECT Top 10000000000 A.ENO, A.FROMDATE, C.Name PROCESS,  B.SNO, MIXMASTER.MIXNO COUNTNAME, MIXMASTER.IBALES  PENDING, B.PACKED, B.PACKED  KGS, 0 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight, '' CountType, Cast(MIXMASTER.MIXNO as Numeric(20)) CountCode, 0 TypeCode, D.Weight, MIXMASTER.MIXDATE PRODDATE  FROM Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID and A.Proc_Type = 5 and B.Master_ID = " + Code + " Left Join  MIXMASTER On B.ProdDate = MIXMASTER.MIXDATE and B.CountCode = MIXMASTER.MIXNO Left Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1  WHERE MIXMASTER.COMPCODE = " + MyParent.CompCode + " AND  MIXMASTER.MIXDATE ='" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and B.Master_ID = " + Code + " ORDER BY MIXMASTER.MIXDATE, MIXMASTER.MIXNO";
                }
                else if (TxtProcess.Tag.ToString() == "6")
                {
                    Str = "SELECT Top 10000000000 A.ENO, A.FROMDATE, C.Name PROCESS,  B.SNO, E.StoppageName  COUNTNAME, B.Packed  PENDING, B.PACKED, B.PACKED  KGS, 0 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight, '' CountType, Cast(B.CountCode as Numeric(20)) CountCode, 0 TypeCode, D.Weight, B.PRODDATE  PRODDATE  FROM Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID and A.Proc_Type = 6 and B.Master_ID = " + Code + " Left Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 Left join MasStoppage E On B.CountCode  = E.StoppageCode and A.Company_Code  = E.compcode  WHERE  B.Master_ID = " + Code + "";
                }
                else
                {
                    Str = "Select Top 10000000000  A.ENO, A.FROMDATE, C.Name PROCESS,  B.SNO, E.Name COUNTNAME, B.PACKED PENDING , B.PACKED, Cast(B.PACKED * Cast(D.Weight as Numeric(20,3)) as Numeric(25,3)) KGS,   0 PACKED_PROD, B.WEIGHTCODE, B.CountCode ActualCount , D.Weight BagWeight, '' CountType,   B.CountCode, B.TypeCode, Cast(D.Weight as Numeric(20,3)) WEIGHT, B.PRODDATE From Vaahini_Erp_Gainup.Dbo.Packing_Master A  Inner Join Vaahini_Erp_Gainup.Dbo.Packing_Details  B On A.RowID = B.Master_ID Inner join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join Vaahini_Erp_Gainup.Dbo.CountWeight(" + MyParent.CompCode + ") D On B.WeightCode = D.Code Inner Join Vaahini_Erp_Gainup.Dbo.Waste_Item_Master  E On B.CountCode = E.RowID  Where  Master_ID = " + Code + " and 1 = 2 Order By B.SNo  ";
                }


                MyBase.Execute_Qry(Str, "RptPackContract");
                ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\RptPackContract.rpt");
                MyParent.FormulaFill(ref ORpt, "CompName", MyParent.CompName);
                MyParent.FormulaFill(ref ORpt, "PDate", MyBase.GetServerDateTime().ToString());
                MyParent.CReport(ref ORpt, "PACKING DETAILS..!");
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
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Packing Details - Delete", " Select Distinct C.Name Process_Name, A.FromDate, A.Tot_Qty, A.Tot_Kgs, A.ENo, A.Effect_Date, A.ToDate, A.Remarks, A.Proc_Type, A.RowID  From Vaahini_Erp_Gainup.Dbo.Packing_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Packing_Details  B On A.RowID = B.Master_ID Inner join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_Type = C.RowID Where A.Approval_Status = 'N' and Company_Code = " + MyParent.CompCode + "  and A.Proc_Type in (12) ORder by ENo DEsc", string.Empty, 120, 100, 100, 120, 80, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    TxtTotal.Text = Dr["Tot_Qty"].ToString();
                    TxtKgs.Text = Dr["Tot_Kgs"].ToString();
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
                    MyBase.Run("Delete From Vaahini_Erp_Gainup.Dbo.Packing_Details Where Master_ID = " + Code, " Delete From Vaahini_Erp_Gainup.Dbo.Packing_Master Where RowID = " + Code);
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
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Packing Details - View", " Select Distinct C.Name Process_Name, A.FromDate, A.Tot_Qty, A.Tot_Kgs, A.ENo, A.Effect_Date, A.ToDate, A.Remarks, A.Proc_Type, A.RowID  From Vaahini_Erp_Gainup.Dbo.Packing_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Packing_Details  B On A.RowID = B.Master_ID Inner join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_Type = C.RowID Where Company_Code = " + MyParent.CompCode + "  and A.Proc_Type in (12) ORder by ENo DEsc", string.Empty, 160, 100, 100, 120, 80, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    TxtTotal.Text = Dr["Tot_Qty"].ToString();
                    TxtKgs.Text = Dr["Tot_Kgs"].ToString();
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
                    if (TxtProcess.Tag.ToString() == "1")
                    {
                        Str = "Select 0 SNO, C.ActualCount + '`s ' + D.ShortName + '(' + B.BagWeight + ')' COUNTNAME,  Sum(acthank) - Dbo.[Get_Packed_Contract]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', A.CountCode, EmplNo, EmplNo2) PENDING, 0.00 PACKED , 0.000 KGS, Sum(acthank) PACKED_PROD, Emplno2 WEIGHTCODE, C.ActualCount, B.BagWeight, D.CountType, A.countcode, A.EMPLNO TypeCode, Cast(B.BagWeight as Numeric(20,3)) WEIGHT, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' PRODDATE  From TrnPrdSum A Inner Join mas_actualcount C On A.countcode = C.actualcode   Inner Join mas_bagweight B On A.emplno2 = B.weightcode  Inner Join mas_counttype D On A.emplno = D.typecode  LEft Join Packing_Details E On A.proddate = E.ProdDate and A.countcode = E.CountCode and A.EMPLNO = E.TypeCode and A.emplno2 = E.WeightCode  Where A.Compcode = " + MyParent.CompCode + "  and A.Proddate Between '01-jan-2013' and '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "'  and DeptCode = 90 and E.Rowid Is Null  and 1 = 2  Group By  Emplno2, bagweight, C.ActualCount, D.CountType, A.countcode, A.EMPLNO, D.ShortName Order by  C.ActualCount + '`s ' + D.ShortName + '(' + B.BagWeight + ')' ";
                    }
                    else if (TxtProcess.Tag.ToString() == "2")
                    {
                        Str = "Select 0 SNO, C.ActualCount + '`s ' + E.ShortName + '(' + D.BagWeight + ')' COUNTNAME,  Sum(Packed) - Dbo.[Get_Shifted_Contract]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', B.CountCode, B.TypeCode , B.WeightCode) PENDING, 0.00 PACKED , 0.000 KGS, Sum(Packed) PACKED_PROD, B.WEIGHTCODE, C.ActualCount, D.BagWeight, E.CountType, B.countcode, B.TypeCode, 1 WEIGHT, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' PRODDATE  From Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID and A.Proc_Type = 1  Inner Join mas_actualcount C On B.countcode = C.actualcode   Inner Join mas_bagweight D On B.WeightCode  = D.weightcode  Inner Join mas_counttype E On B.TypeCode  = E.typecode Where 1= 2 Group By C.actualcount, E.shortname, D.bagweight, B.CountCode, B.TypeCode, B.WeightCode, E.counttype Having Sum(Packed) - Dbo.[Get_Shifted_Contract]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', B.CountCode, B.TypeCode , B.WeightCode) > 0  Order by C.ActualCount + '`s ' + E.ShortName + '(' + D.BagWeight + ')'";
                    }
                    else if (TxtProcess.Tag.ToString() == "3" || TxtProcess.Tag.ToString() == "4" || TxtProcess.Tag.ToString() == "10" )
                    {
                        Str = "Select 0 SNO, A.Name COUNTNAME, 0.000 PENDING, 0.000 PACKED, 0.000 KGS, 0.000 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight,  '' CountType, A.RowID  countcode, 0 TypeCode, 1.00 WEIGHT, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' PRODDATE  From Waste_Item_Master A Inner Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 Left join Packing_Details B On A.RowID = B.CountCode and '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' = B.ProdDate LEft join Packing_Master C On B.Master_ID = C.RowID and C.Proc_Type in (3,4) Where C.RowID is Null and 1 = 2";
                    }
                    else if (TxtProcess.Tag.ToString() == "5")
                    {
                        Str = "SELECT 0 SNO, MIXMASTER.MIXNO COUNTNAME, 0.00 PENDING, 0.00 PACKED, 0.00 KGS, 0 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight, '' CountType, Cast(MIXMASTER.MIXNO as Numeric(20)) CountCode, 0 TypeCode, D.Weight, MIXMASTER.MIXDATE PRODDATE  FROM MIXMASTER Left Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 LEFT JOIN (MIXDETAIL LEFT JOIN (SELECT  DISTINCT LOTNO, SUM(BALES) BALES, COMPCODE, VARCODE  FROM LOTMASTER GROUP BY LOTNO, VARCODE, COMPCODE) LOTMASTER ON MIXDETAIL.LOTNO = LOTMASTER.LOTNO) ON MIXMASTER.MIXNO = MIXDETAIL.MIXNO WHERE MIXDETAIL.COMPCODE = " + MyParent.CompCode + " AND  LOTMASTER.COMPCODE = " + MyParent.CompCode + " AND  MIXMASTER.COMPCODE = " + MyParent.CompCode + " AND  MIXMASTER.MIXDATE ='" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and 1 = 2 GROUP BY MIXMASTER.MIXDATE, MIXMASTER.MIXNO, D.Code, D.Weight  ORDER BY MIXMASTER.MIXDATE, MIXMASTER.MIXNO";
                    }
                    else if (TxtProcess.Tag.ToString() == "6")
                    {
                        Str = "Select 0 SNO, C.ActualCount + '`s ' + D.ShortName + '(' + B.BagWeight + ')' COUNTNAME,  Sum(acthank) - Dbo.[Get_Packed_Contract]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', A.CountCode, EmplNo, EmplNo2) PENDING, 0 PACKED , 0.000 KGS, Sum(acthank) PACKED_PROD, Emplno2 WEIGHTCODE, C.ActualCount, B.BagWeight, D.CountType, A.countcode, A.EMPLNO TypeCode, Cast(B.BagWeight as Numeric(20,3)) WEIGHT, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' PRODDATE  From TrnPrdSum A Inner Join mas_actualcount C On A.countcode = C.actualcode   Inner Join mas_bagweight B On A.emplno2 = B.weightcode  Inner Join mas_counttype D On A.emplno = D.typecode  LEft Join Packing_Details E On A.proddate = E.ProdDate and A.countcode = E.CountCode and A.EMPLNO = E.TypeCode and A.emplno2 = E.WeightCode  Where A.Compcode = " + MyParent.CompCode + "  and A.Proddate Between '01-jan-2013' and '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "'  and DeptCode = 90 and E.Rowid Is Null  and 1 = 2  Group By  Emplno2, bagweight, C.ActualCount, D.CountType, A.countcode, A.EMPLNO, D.ShortName Order by  C.ActualCount + '`s ' + D.ShortName + '(' + B.BagWeight + ')' ";
                    }
                    else if (TxtProcess.Tag.ToString() == "8")
                    {
                        Str = "SELECT 0 SNO, LOTNO COUNTNAME, 0.00 PENDING, 0.00 PACKED, 0.00 KGS, 0 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight, '' CountType, Cast(LOTNO1 as Numeric(20)) CountCode, 0 TypeCode, D.Weight, LOTDATE PRODDATE FROM LOTMASTER Left Join CountWeight(Case When " + MyParent.CompCode + " = 3 Then 6 Else 1 End) D On CAst(D.Weight as Numeric(3)) = 1 WHERE COMPCODE = (Case When " + MyParent.CompCode + " = 3 Then 6 Else 1 End) AND  LOTDATE ='" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and 1 = 2  GROUP BY LOTNO, LOTNO1, D.Code, D.Weight, LOTDATE  ORDER BY LOTDATE, LOTNO";
                    }
                    else if (TxtProcess.Tag.ToString() == "9")
                    {
                        Str = "Select 0 SNO, COUNT COUNTNAME,  0.00 PENDING, 0.00 PACKED, 0.00 KGS, 0 PACKED_PROD, COuntWeightCode WeightCode, 0 ActualCount, Weight Bagweight, '' CountType, CountCode, CountTypeCode TypeCode, Weight, Invoice_Date PRODDATE From Packed_Contract_Sales(" + MyParent.CompCode + ") Where 1 = 2";
                    }
                    else
                    {
                        Str = "Select 0 SNO, COUNT COUNTNAME,  0.00 PENDING, 0.00 PACKED, 0.000 KGS, 0.000 PACKED_PROD, COuntWeightCode WeightCode, 0 ActualCount, Weight Bagweight, '' CountType, CountCode, CountTypeCode TypeCode, Weight, Invoice_Date PRODDATE From Vaahini_Erp_Gainup.Dbo.Packed_Contract_Sales(" + MyParent.CompCode + ") Where 1 = 2";
                    }
                }
                else
                {
                    if (TxtProcess.Tag.ToString() == "1")
                    {
//                        Str = "Select B.SNO, F.ActualCount + '`s ' + G.ShortName + '(' + D.BagWeight + ')' COUNTNAME,  Dbo.[Pack_Prod_Update_CountWise]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', B.CountCode, B.TypeCode, B.WeightCode, Case When " + MyParent.CompCode + " = 3 Then 5 Else " + MyParent.CompCode + " End)  PENDING , CAst (B.PACKED as Numeric(20,2)) PACKED , Cast(B.PACKED * Cast(D.BagWeight as Numeric(20,3)) as Numeric(25,3)) KGS,    Dbo.[Pack_Prod_Update_CountWise]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', B.CountCode, B.TypeCode, B.WeightCode, Case When " + MyParent.CompCode + " = 3 Then 5 Else " + MyParent.CompCode + " End) PACKED_PROD, B.WEIGHTCODE, F.ActualCount , D.BagWeight, G.CountType,   B.CountCode, B.TypeCode, Cast(D.BagWeight as Numeric(20,3)) WEIGHT, B.PRODDATE From Packing_Master A  Inner Join Packing_Details  B On A.RowID = B.Master_ID Inner join Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join Mas_BagWeight D On B.WeightCode = D.WeightCode  Inner Join mas_actualcount F On B.countcode = F.actualcode   Inner Join mas_counttype G On B.TypeCode = G.typecode Where  Master_ID = " + Code + " Order By F.ActualCount,B.SNo  ";
                        Str = "Select A2.SNO, C.ActualCount + '`s ' + D.ShortName + '(' + B.BagWeight + ')' COUNTNAME,  Cast(Sum(acthank) - Dbo.[Get_Packed_Contract]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', A2.CountCode, A2.TypeCode , A2.WeightCode) as Numeric(20,2)) + CAst (A2.PACKED as Numeric(20,2)) PENDING, CAst (A2.PACKED as Numeric(20,2)) PACKED , Cast(A2.PACKED * Cast(B.BagWeight as Numeric(20,3)) as Numeric(25,3)) KGS, Sum(acthank) PACKED_PROD, A2.WeightCode , C.ActualCount, B.BagWeight, D.CountType, A2.countcode, A2.TypeCode, Cast(B.BagWeight as Numeric(20,3)) WEIGHT, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' PRODDATE  From Packing_Master A1 Inner Join Packing_Details A2 On A1.RowID = A2.Master_ID LEft Join TrnPrdSum A On A2.CountCode = A.countcode and A.EMPLNO = A2.TypeCode and A.emplno2 = A2.WeightCode  LEft Join mas_actualcount C On A2.countcode = C.actualcode Left Join mas_bagweight B On A2.WeightCode = B.weightcode  Left Join mas_counttype D On A2.TypeCode = D.typecode  Where A.Compcode = (Case When " + MyParent.CompCode + " = 3 Then 5 Else " + MyParent.CompCode + " End)  and A.Proddate Between '01-jan-2013' and '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and DeptCode = 90 and A1.RowID = " + Code + " Group By A2.Packed , A2.SNo, A2.CountCode , A2.TypeCode , A2.WeightCode , C.ActualCount, D.CountType, D.ShortName , B.bagweight Order by   C.ActualCount + '`s ' + D.ShortName + '(' + B.BagWeight + ')' ";
                    }
                    else if (TxtProcess.Tag.ToString() == "2")
                    {
                        Str = "Select B.SNO, F.ActualCount + '`s ' + G.ShortName + '(' + D.BagWeight + ')' COUNTNAME,  E.Packed_Prod - Dbo.[Get_Shifted_Contract]('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', B.CountCode, B.TypeCode, B.WeightCode) + B.PACKED PENDING , B.PACKED, Cast(B.PACKED * Cast(D.BagWeight as Numeric(20,3)) as Numeric(25,3)) KGS,    E.PACKED_PROD, B.WEIGHTCODE, F.ActualCount , D.BagWeight, G.CountType,   B.CountCode, B.TypeCode, Cast(D.BagWeight as Numeric(20,3)) WEIGHT, B.PRODDATE From Packing_Master A  Inner Join Packing_Details  B On A.RowID = B.Master_ID Inner join Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join Mas_BagWeight D On B.WeightCode = D.WeightCode Inner Join Packed_UpToDate_Fro_Shift('" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', Case When " + MyParent.CompCode + " = 3 Then 5 Else 1 End) E On B.ProdDate = E.ProdDate and B.WeightCode = E.WeightCode and B.TypeCode = E.TypeCode and E.CountCode = B.CountCode Inner Join mas_actualcount F On B.countcode = F.actualcode   Inner Join mas_counttype G On B.TypeCode = G.typecode Where  Master_ID = " + Code + " Order By B.SNo  ";
                    }
                    else if (TxtProcess.Tag.ToString() == "3" || TxtProcess.Tag.ToString() == "4" || TxtProcess.Tag.ToString() == "10")
                    {
                        Str = "Select B.SNO, E.Name COUNTNAME, B.PACKED PENDING , B.PACKED, Cast(B.PACKED * Cast(D.Weight as Numeric(20,3)) as Numeric(25,3)) KGS,   0.000 PACKED_PROD, B.WEIGHTCODE, B.CountCode ActualCount , D.Weight BagWeight, '' CountType,   B.CountCode, B.TypeCode, Cast(D.Weight as Numeric(20,3)) WEIGHT, B.PRODDATE From Packing_Master A  Inner Join Packing_Details  B On A.RowID = B.Master_ID Inner join Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join CountWeight(" + MyParent.CompCode + ") D On B.WeightCode = D.Code Inner Join Waste_Item_Master  E On B.CountCode = E.RowID  Where  Master_ID = " + Code + " Order By B.SNo  ";
                    }
                    else if (TxtProcess.Tag.ToString() == "5")
                    {
                        Str = "SELECT 0 SNO, MIXMASTER.MIXNO COUNTNAME, Case When " + MyParent.CompCode + " = 3 Then B.PAcked  Else MIXMASTER.IBALES End PENDING, B.PACKED, B.PACKED  KGS, 0 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight, '' CountType, Cast(MIXMASTER.MIXNO as Numeric(20)) CountCode, 0 TypeCode, D.Weight, MIXMASTER.MIXDATE PRODDATE  FROM Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID and A.Proc_Type = 5 and B.Master_ID = " + Code + " Left Join  MIXMASTER On B.ProdDate = MIXMASTER.MIXDATE and B.CountCode = MIXMASTER.MIXNO Left Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1  WHERE MIXMASTER.COMPCODE = Case When " + MyParent.CompCode + " = 3 Then 6 Else 1 End AND  MIXMASTER.MIXDATE ='" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and B.Master_ID = " + Code + " ORDER BY MIXMASTER.MIXDATE, MIXMASTER.MIXNO";
                    }
                    else if (TxtProcess.Tag.ToString() == "6")
                    {
                        Str = "SELECT 0 SNO, E.StoppageName  COUNTNAME, B.Packed  PENDING, B.PACKED, B.PACKED  KGS, 0 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight, '' CountType, Cast(B.CountCode as Numeric(20)) CountCode, 0 TypeCode, D.Weight, B.PRODDATE  PRODDATE  FROM Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID and A.Proc_Type = 6 and B.Master_ID = " + Code + " Left Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 Left join MasStoppage E On B.CountCode  = E.StoppageCode and A.Company_Code  = (Case When  E.compcode = 3 Then 2 When E.Compcode = 5 Then 3 Else 1 End)  WHERE  B.Master_ID = " + Code + "";
                    }
                    else if (TxtProcess.Tag.ToString() == "8")
                    {
                        Str = "SELECT 0 SNO, A1.LOTNO COUNTNAME, A1.BALES PENDING, B.PACKED, B.PACKED  KGS, 0 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight, '' CountType, Cast(A1.LOTNO1 as Numeric(20)) CountCode, 0 TypeCode, D.Weight, A1.LOTDATE PRODDATE  FROM Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID and A.Proc_Type = 8 and B.Master_ID = " + Code + " Left Join  LOTMASTER A1 On B.ProdDate = A1.LOTDATE and B.CountCode = A1.LOTNO1 Left Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1  WHERE A1.COMPCODE = Case When " + MyParent.CompCode + " = 3 Then 6 Else 1 End AND  A1.LOTDATE ='" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and B.Master_ID = " + Code + " ORDER BY A1.LOTDATE, A1.LOTNO";
                    }
                    else if (TxtProcess.Tag.ToString() == "9")
                    {
                        Str = "SELECT 0 SNO, E.Count COUNTNAME, E.QTY PENDING, B.PACKED, B.PACKED  KGS, 0 PACKED_PROD, B.WeightCode, E.Actual_Count ActualCount, E.Weight BagWeight, E.CountType, Cast(B.CountCode as Numeric(20)) CountCode, B.TypeCode, E.Weight, B.PRODDATE  PRODDATE  FROM Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID and A.Proc_Type = " + TxtProcess.Tag + " and B.Master_ID = " + Code + "   Left join Packed_Contract_Sales(" + MyParent.CompCode + ") E On B.CountCode  = E.CountCode  and B.TypeCode = E.CountTypeCode and B.WeightCode = E.CountWeightCode and B.ProdDate = E.Invoice_Date  WHERE  B.Master_ID = " + Code + " Order by B.SNo";
                    }
                    else if (TxtProcess.Tag.ToString() == "12")
                    {
                        Str = "SELECT 0 SNO, E.Name  COUNTNAME, B.Packed  PENDING, B.PACKED, B.PACKED  KGS, 0 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight, '' CountType, Cast(B.CountCode as Numeric(20)) CountCode, 0 TypeCode, D.Weight, B.PRODDATE  PRODDATE  FROM Vaahini_Erp_Gainup.Dbo.Packing_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Packing_Details B On A.RowID = B.Master_ID and A.Proc_Type = " + TxtProcess.Tag + " and B.Master_ID = " + Code + " Left Join Vaahini_Erp_Gainup.Dbo.CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 Left join Vaahini_Erp_Gainup.Dbo.Loading_Item_Name_Master E On B.CountCode  = E.RowID  WHERE  B.Master_ID = " + Code + "";
                    }
                    else if (TxtProcess.Tag.ToString() == "13")
                    {
                        Str = "SELECT 0 SNO, E.Name  COUNTNAME, B.Packed  PENDING, B.PACKED, B.PACKED  KGS, 0 PACKED_PROD, D.Code WeightCode, 0 ActualCount, D.Weight BagWeight, '' CountType, Cast(B.CountCode as Numeric(20)) CountCode, 0 TypeCode, D.Weight, B.PRODDATE  PRODDATE  FROM Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID and A.Proc_Type = " + TxtProcess.Tag + " and B.Master_ID = " + Code + " Left Join CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 Left join Waste_Sales_To_Irulappa_Contract(" + MyParent.CompCode + ") E On B.CountCode  = E.WasCode and A.FromDate = E.Invoice_Date  WHERE  B.Master_ID = " + Code + "";
                    }                    
                    else
                    {
                        Str = "Select B.SNO, E.Name COUNTNAME, B.PACKED PENDING , B.PACKED, Cast(B.PACKED * Cast(D.Weight as Numeric(20,3)) as Numeric(25,3)) KGS,   0.000 PACKED_PROD, B.WEIGHTCODE, B.CountCode ActualCount , D.Weight BagWeight, '' CountType,   B.CountCode, B.TypeCode, Cast(D.Weight as Numeric(20,3)) WEIGHT, B.PRODDATE From Vaahini_Erp_Gainup.Dbo.Packing_Master A  Inner Join Vaahini_Erp_Gainup.Dbo.Packing_Details  B On A.RowID = B.Master_ID Inner join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join Vaahini_Erp_Gainup.Dbo.CountWeight(" + MyParent.CompCode + ") D On B.WeightCode = D.Code Inner Join Vaahini_Erp_Gainup.Dbo.Waste_Item_Master  E On B.CountCode = E.RowID  Where  Master_ID = " + Code + " and 1 = 2 Order By B.SNo  ";
                    }
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid, "PACKED", "COUNTNAME");
                MyBase.Grid_Designing(ref Grid, ref Dt, "PRODDATE", "PACKED_PROD", "WEIGHTCODE", "ActualCount", "CountCode", "TypeCode", "CountType", "BagWeight", "Weight");
                MyBase.Grid_Width(ref Grid, 50, 180, 100, 100, 120);
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["PENDING"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["COUNTNAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["PACKED"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["KGS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmPackingDetails_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "TxtProcess")
                    {
                        Grid.CurrentCell = Grid["COUNTNAME", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                    else if (this.ActiveControl.Name == "TxtTotal")
                    {
                        Total();
                        SendKeys.Send("{Tab}");
                        return;
                    }
                    else if (this.ActiveControl.Name == "TxtRemarks")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtProcess")
                    {
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Process ", " Select Name , A.RowID  From Vaahini_Erp_Gainup.Dbo.Contract_Process_Name A Left join Vaahini_Erp_Gainup.Dbo.Packing_Master B On A.RowID = B.Proc_Type and B.Company_Code = " + MyParent.CompCode + " and B.FromDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' Where B.RowID is null and A.RowID Not In (7) and A.RowID in (12)", string.Empty, 250);
                        if (Dr != null)
                        {
                            TxtProcess.Text = Dr["Name"].ToString();
                            TxtProcess.Tag = Dr["RowID"].ToString();
                            Grid_Data();
                            TxtProcess.Focus();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtParty")
                    {
                        Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Party", "Select Party, Code LEDGERCODE  From  Accounts.dbo.Debtors_Creditors(" + MyParent.CompCode + ",'" + MyParent.YearCode + "') Where Party Not Like '%ZZZ%'", string.Empty, 400);
                        if (Dr != null)
                        {
                            //TxtParty.Text = Dr["Party"].ToString();
                            //TxtParty.Tag = Dr["LEDGERCODE"].ToString();
                        }
                    }
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

        private void FrmPackingDetails_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtRemarks" || this.ActiveControl.Name == String.Empty)
                    {
                        MyBase.Return_Ucase(e);
                    }
                    else if (this.ActiveControl.Name == "TxtDeduct")
                    {
                        MyBase.Valid_Decimal((TextBox)this.ActiveControl, e);
                    }
                    else
                    {
                        MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void Grid_Detail()
        {
            
            try
            {
                DataTable DtDet = new DataTable();   
                if (Grid.CurrentCell != null && Grid.CurrentCell.Value != DBNull.Value && Grid.CurrentCell.Value.ToString() != String.Empty && Grid.CurrentCell.RowIndex < Grid.Rows.Count -1)
                {
                    Str = "Select  B.ProdDate PRODDATE, B.ShiftCode SHIFT, E.MACNAME MACHINE ,   StopMins TotMins From Vaahini_Erp_Gainup.Dbo.MasStoppage A Inner Join Vaahini_Erp_Gainup.Dbo.TRNSTOPPAGE B On A.StoppageCode = B.STOPPAGECODE and A.compcode = B.compcode Left Join Vaahini_Erp_Gainup.Dbo.CountWeight(" + MyParent.CompCode + ") D On CAst(D.Weight as Numeric(3)) = 1 left join Vaahini_Erp_Gainup.Dbo.MASMAC E On B.MACCODE = E.MACCODE and B.compcode = E.compcode   Where " + MyParent.CompCode + " = (Case When  E.compcode = 3 Then 2 When E.Compcode = 5 Then 3 Else 1 End) and Bill_Mode = 'Y'  and A.StoppageName = '" + Grid["COUNTNAME", Grid.CurrentCell.RowIndex].Value.ToString() + "' and B.ProdDate =  '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' ";                    
                        Grid1.DataSource = MyBase.Load_Data(Str, ref DtDet);
                        MyBase.Grid_Designing(ref Grid1, ref DtDet);
                        MyBase.ReadOnly_Grid_Without(ref Grid1);
                        MyBase.Grid_Colouring(ref Grid1, Control_Modules.Grid_Design_Mode.Column_Wise);
                        MyBase.Grid_Width(ref Grid1, 100, 80, 100, 100);
                        Grid1.RowHeadersWidth = 10;
                        MyBase.V_DataGridView(ref Grid1);                    
                }
                else
                {
                    Grid1.DataSource = null;
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Grid_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {               
               // Grid_Detail();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                Grid_Detail();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}