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
    public partial class FrmContractWorkerWages : Form, Entry 
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
        public FrmContractWorkerWages()
        {
            InitializeComponent();
        }


        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                TxtParty.Focus();              
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
                if (TxtGross.Text.Trim() == string.Empty || Convert.ToDouble(TxtGross.Text) == 0)
                {
                    MessageBox.Show("Invalid Gross", "Gainup");
                    TxtGross.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtNet.Text.Trim() == string.Empty || Convert.ToDouble(TxtNet.Text) == 0)
                {
                    MessageBox.Show("Invalid Net", "Gainup");
                    TxtNet.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 9; j++)
                    {
                        if (Convert.ToDouble(Grid["PACKED", i].Value) > Convert.ToDouble(Grid["PACKED1", i].Value))
                        {
                            MessageBox.Show("Invalid Packed..!", "Gainup");
                            Grid["PACKED", i].Value = 0.00;                            
                            Grid["AMOUNT", i].Value = 0.00;
                            Grid.CurrentCell = Grid["PACKED", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);                            
                            return;
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
               

                if (MyParent._New)
                {
                    Queries = new String[Dt.Rows.Count + 4];
                    DataTable TDt = new DataTable();
                    MyBase.Load_Data("Select (Isnull(Max(Entry_No), 0) + 1) No From Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Master Where Company_Code = " + MyParent.CompCode + " ", ref TDt);
                    TxtENo.Text = TDt.Rows[0][0].ToString();
                    Queries[Array_Index++] = "Insert Into Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Master (Entry_No, Effect_Date, Party_Code, Proc_ID, Gross_Amount, Deduction_Amount, RO_Amount, Net_Amount, Remarks,  Company_Code, Year_Code, Tot_Kgs, BillPass_Status, FromDate, ToDate) Values (" + TxtENo.Text + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtParty.Tag + ", " + TxtProcess.Tag + ",   " + Convert.ToDouble(TxtGross.Text) + ", " + Convert.ToDouble(TxtDeduct.Text) + ",   " + Convert.ToDouble(TxtRo.Text) + ", " + Convert.ToDouble(TxtNet.Text) + ", '" + TxtRemarks.Text + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "', '" + Convert.ToDouble(TxtKgs.Text) + "', 0, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "'); Select Scope_Identity()";
                    Queries[Array_Index++] = "Update Vaahini_Erp_Gainup.Dbo.Packing_Master Set Approval_Status = 'Y' , Approval_System = Host_Name() ,Approval_Date = Getdate() Where FromDate Between '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' and Proc_Type = " + TxtProcess.Tag + " and  Company_Code = " + MyParent.CompCode + "";
                }
                else
                {
                    Queries = new String[Dt.Rows.Count + 3];
                    Queries[Array_Index++] = "Update Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Master Set  Party_Code = " + TxtParty.Tag + ",  Deduction_Amount = " + Convert.ToDouble(TxtDeduct.Text) + ",  Gross_Amount = " + Convert.ToDouble(TxtGross.Text) + ", RO_Amount = " + Convert.ToDouble(TxtRo.Text) + ", Net_Amount = " + Convert.ToDouble(TxtNet.Text) + ", Remarks = '" + TxtRemarks.Text + "', Tot_Kgs = " + Convert.ToDouble(TxtKgs.Text) + ", BillPass_Status =  0 Where Rowid = " + Code;
                    Queries[Array_Index++] = "Delete From Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Details Where Master_id = " + Code;                    
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Details  (Master_ID, SNo,  WeightCode, FromDate, ToDate, Packed, Kgs, Rate, Amount, CountCode, TypeCode) Values (@@IDENTITY, " + Grid["SNO", i].Value + ",  " + Grid["WEIGHTCODE", i].Value + " ,  '" + MyBase.Get_Date_Format(Grid["FROMDATE", i].Value.ToString()) + "', '" + MyBase.Get_Date_Format(Grid["TODATE", i].Value.ToString()) + "', " + Grid["PACKED", i].Value + ", " + Grid["KGS", i].Value + ", " + Grid["RATE", i].Value + ", " + Grid["AMOUNT", i].Value + ", " + Grid["ACTUALCODE", i].Value + ", " + Grid["TYPECODE", i].Value + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Details  (Master_ID, SNo,   WeightCode, FromDate, ToDate, Packed, Kgs, Rate, Amount, CountCode, TypeCode) Values (" + Code + ", " + Grid["SNO", i].Value + ", " + Grid["WEIGHTCODE", i].Value + " ,  '" + String.Format("{0:dd-MMM-yyyy}", Grid["FROMDATE", i].Value) + "', '" + String.Format("{0:dd-MMM-yyyy}", Grid["TODATE", i].Value) + "', " + Grid["PACKED", i].Value + ", " + Grid["KGS", i].Value + ", " + Grid["RATE", i].Value + ", " + Grid["AMOUNT", i].Value + ", " + Grid["ACTUALCODE", i].Value + ", " + Grid["TYPECODE", i].Value + ")";
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
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Contract Wages- Edit", "Select A.Entry_No, A.FromDate, A.ToDate, D.NAME Party, C.Name Process_Type, A.Tot_Kgs,  A.Gross_Amount, A.Deduction_Amount, A.Ro_Amount, A.Net_Amount, A.Remarks, A.RowID, A.Proc_ID, A.Party_Code, A.Effect_Date From  Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_ID = C.RowID Left Join Vaahini_Erp_Gainup.Dbo.Spinning_Contract_Name D On D.RowID = A.Party_Code Where BillPass_Status = 0 and Approve_Status = 'F' and A.Company_Code= " + MyParent.CompCode + " and A.Proc_ID = 12", string.Empty, 80, 80, 80, 200, 100, 120, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    TxtProcess.Focus();
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
                TxtProcess.Text = Dr["Process_Type"].ToString();
                TxtProcess.Tag = Dr["Proc_Id"].ToString();
                TxtENo.Text = Dr["Entry_No"].ToString();
                TxtParty.Text = Dr["Party"].ToString();
                TxtParty.Tag = Dr["Party_Code"].ToString();                
                TxtRemarks.Text = Dr["Remarks"].ToString();
                Grid_Data();
                TxtGross.Text = Dr["Gross_Amount"].ToString();                
                TxtDeduct.Text = Dr["Deduction_Amount"].ToString();                
                TxtRo.Text = Dr["Ro_Amount"].ToString();
                TxtNet.Text = Dr["Net_Amount"].ToString();
                TxtKgs.Text = Dr["Tot_Kgs"].ToString();
                //Total();
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PACKED"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["RATE"].Index)
                    {
                        if (Grid["RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) == 0)
                        {
                            Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = "0.00";
                        }
                        else
                        {
                            if (Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0)
                            {
                                Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = "0.00";
                            }
                            else
                            {
                                if (Grid["KGS", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["RATE", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                                {
                                    //if (TxtProcess.Tag.ToString() == "1")
                                    //{
                                        Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["PACKED", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value));
                                    //}
                                    //else
                                    //{
                                    //    Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["KGS", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value));
                                    //}
                                }
                            }
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
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select D.Name + '`s ' + E.ShortName + '(' + F.Weight + ')' COUNTNAME, C.Name PACK_NAME, Sum(PAcked) Packed, Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,WeightCode) RATE, Cast(Cast(F.Weight as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) KGS, Cast(Cast(Cast(1 as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) * Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,WeightCode) as Numeric(20,3)) AMOUNT, SUM(Packed) PACKED1, F.Weight, B.WeightCode, B.CountCode, B.TypeCode, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' FROMDATE, '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' TODATE  From Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID Inner Join Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join  CountName(" + MyParent.CompCode + ") D On B.CountCode = D.Code Inner Join CountType(" + MyParent.CompCode + ") E On B.TypeCode = E.Code Inner Join CountWeight(" + MyParent.CompCode + ") F On F.Code = B.WeightCode Where A.FromDate BetWEen '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' and A.Proc_Type = " + TxtProcess.Tag + " and A.Company_Code = " + MyParent.CompCode + " Group By D.Name, E.ShortName, F.Weight, C.Name, Proc_Type, WeightCode, B.CountCode, B.TypeCode Order by D.Name + '`s ' + E.ShortName + '(' + F.Weight + ')' ", string.Empty, 180, 100, 100, 100, 100, 120);
                        }
                        else if (TxtProcess.Tag.ToString() == "2")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select D.Name + '`s ' + E.ShortName + '(' + F.Weight + ')' COUNTNAME, C.Name PACK_NAME, Sum(PAcked) Packed, Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,WeightCode) RATE, Cast(1 * Sum(PACKED) as Numeric(20,3)) KGS, Cast(Cast(Cast(1 as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) * Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,WeightCode) as Numeric(20,3)) AMOUNT, SUM(Packed) PACKED1, F.Weight, B.WeightCode, B.CountCode, B.TypeCode, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' FROMDATE, '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' TODATE  From Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID Inner Join Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join  CountName(" + MyParent.CompCode + ") D On B.CountCode = D.Code Inner Join CountType(" + MyParent.CompCode + ") E On B.TypeCode = E.Code Inner Join CountWeight(" + MyParent.CompCode + ") F On F.Code = B.WeightCode Where A.FromDate BetWEen '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' and A.Proc_Type = " + TxtProcess.Tag + " and A.Company_Code = " + MyParent.CompCode + " Group By D.Name, E.ShortName, F.Weight, C.Name, Proc_Type, WeightCode, B.CountCode, B.TypeCode Order by D.Name + '`s ' + E.ShortName + '(' + F.Weight + ')' ", string.Empty, 180, 100, 100, 100, 100, 120);
                        }
                        else if (TxtProcess.Tag.ToString() == "5")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select C.Name COUNTNAME, Sum(PAcked) Packed, Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,B.WeightCode) RATE, Cast(Cast(F.bagWeight as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) KGS, Cast(Cast(Cast(F.bagWeight as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) * Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,B.WeightCode) as Numeric(20,3)) AMOUNT, C.Name PACK_NAME, SUM(Packed) PACKED1, F.BagWeight Weight, B.WeightCode, 0 CountCode, B.TypeCode, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' FROMDATE, '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' TODATE  From Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID Inner Join Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join  MixMaster D On B.CountCode = D.MixNo and A.FromDAte = D.MixDate  Inner Join Mas_Bagweight F On F.WeightCode = B.WeightCode Where A.FromDate BetWEen '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' and A.Proc_Type = " + TxtProcess.Tag + " and A.Company_Code = " + MyParent.CompCode + " Group By  F.bagWeight, C.Name, Proc_Type, B.WeightCode,  B.TypeCode ", string.Empty, 150, 100, 100, 100, 120);
                        }
                        else if (TxtProcess.Tag.ToString() == "3" || TxtProcess.Tag.ToString() == "4" || TxtProcess.Tag.ToString() == "7" || TxtProcess.Tag.ToString() == "10")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select D.Name  COUNTNAME,  Sum(PAcked) Packed, Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "'," + TxtProcess.Tag + ",B.WeightCode) RATE, Cast(Cast(F.bagWeight as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) KGS, Cast(Cast(Cast(F.bagWeight as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) * Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "'," + TxtProcess.Tag + ",B.WeightCode) as Numeric(20,3)) AMOUNT, C.Name PACK_NAME, SUM(Packed) PACKED1, F.BagWeight Weight, B.WeightCode, B.CountCode, B.TypeCode, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' FROMDATE, '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' TODATE  From Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID Inner Join Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join  Waste_Item_MAster D On B.CountCode = D.RowID  Inner Join Mas_Bagweight F On F.WeightCode = B.WeightCode Where A.FromDate BetWEen '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' and A.Proc_Type =  Case When " + TxtProcess.Tag + " = 7 Then 3 Else " + TxtProcess.Tag + " End and A.Company_Code = " + MyParent.CompCode + " Group By D.Name, F.bagWeight, C.Name, Proc_Type, B.WeightCode, B.CountCode, B.TypeCode Order by D.NAme ", string.Empty, 180, 100, 100, 100, 100, 120);
                        }
                        else if (TxtProcess.Tag.ToString() == "6")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select D.StoppageName  COUNTNAME,  Sum(PAcked) Packed, Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,B.CountCode) RATE, Cast(Cast(F.bagWeight as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) KGS, Cast(Cast(Cast(F.bagWeight as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) * Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,B.CountCode) as Numeric(20,3)) AMOUNT, C.Name PACK_NAME, SUM(Packed) PACKED1, F.BagWeight Weight, B.WeightCode, B.CountCode, B.TypeCode, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' FROMDATE, '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' TODATE  From Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID Inner Join Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join  MasStoppage D On B.CountCode = D.StoppageCode and D.CompCode = Case When " + MyParent.CompCode + " = 3 Then 5 Else " + MyParent.CompCode + " End Inner Join Mas_Bagweight F On F.WeightCode = B.WeightCode Where A.FromDate BetWEen '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' and A.Proc_Type = " + TxtProcess.Tag + " and A.Company_Code = " + MyParent.CompCode + " Group By D.StoppageName, F.bagWeight, C.Name, Proc_Type, B.WeightCode, B.CountCode, B.TypeCode Order by D.StoppageName ", string.Empty, 180, 100, 100, 100, 100, 120);
                        }
                        else if (TxtProcess.Tag.ToString() == "8")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select D.LotNo COUNTNAME, Sum(PAcked) Packed, Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,B.WeightCode) RATE, Cast(Cast(F.bagWeight as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) KGS, Cast(Cast(Cast(F.bagWeight as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) * Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,B.WeightCode) as Numeric(20,3)) AMOUNT, C.Name PACK_NAME, SUM(Packed) PACKED1, F.BagWeight Weight, B.WeightCode, 0 CountCode, B.TypeCode, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' FROMDATE, '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' TODATE  From Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID Inner Join Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join  LotMaster D On B.CountCode = D.LOTNO1 and D.LotDate Between  '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "'  Inner Join Mas_Bagweight F On F.WeightCode = B.WeightCode Where A.FromDate BetWEen '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' and A.Proc_Type = " + TxtProcess.Tag + " and A.Company_Code = " + MyParent.CompCode + " Group By  F.bagWeight, C.Name, Proc_Type, B.WeightCode,  B.TypeCode, D.LotNo ", string.Empty, 150, 100, 100, 100, 120);
                        }
                        else if (TxtProcess.Tag.ToString() == "9")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select D.Count  COUNTNAME, Sum(PAcked) Packed, Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,B.WeightCode) RATE, Cast(Cast(F.bagWeight as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) KGS, Cast(Cast(Cast(1 as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) * Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,B.WeightCode) as Numeric(20,3)) AMOUNT, C.Name PACK_NAME, SUM(Packed) PACKED1, F.BagWeight Weight, B.WeightCode, B.CountCode, B.TypeCode, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' FROMDATE, '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' TODATE  From Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID Inner Join Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join  Packed_Contract_sales(" + MyParent.CompCode + ") D On B.CountCode = D.CountCode and B.TypeCode = D.CountTypeCode and B.WeightCode = D.CountWeightCode and A.FromDAte = D.Invoice_Date  Inner Join Mas_Bagweight F On F.WeightCode = B.WeightCode Where A.FromDate BetWEen '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' and A.Proc_Type = " + TxtProcess.Tag + " and A.Company_Code = " + MyParent.CompCode + " Group By  F.bagWeight, C.Name, Proc_Type, B.WeightCode,  B.TypeCode, D.Count, B.CountCode ", string.Empty, 150, 100, 100, 100, 120);
                        }
                        else if (TxtProcess.Tag.ToString() == "12")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select D.Name  COUNTNAME,  Sum(B.PAcked) Packed, Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,B.CountCode) RATE, Cast(Cast(F.bagWeight as Numeric(20,3)) * Sum(B.PACKED) as Numeric(20,3)) KGS, Cast(Cast(Cast(F.bagWeight as Numeric(20,3)) * Sum(B.PACKED) as Numeric(20,3)) * Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,B.CountCode) as Numeric(20,3)) AMOUNT, C.Name PACK_NAME, SUM(B.Packed) PACKED1, F.BagWeight Weight, B.WeightCode, B.CountCode, B.TypeCode, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' FROMDATE, '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' TODATE  From Vaahini_Erp_Gainup.Dbo.Packing_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Packing_Details B On A.RowID = B.Master_ID Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join  Vaahini_Erp_Gainup.Dbo.Loading_Item_Name_Master D On B.CountCode = D.RowId  Inner Join Vaahini_Erp_Gainup.Dbo.Mas_Bagweight F On F.WeightCode = B.WeightCode Left join Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Master G On C.RowID = G.Proc_ID and G.Company_Code = " + MyParent.CompCode + " and   (G.FromDate Between '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' or G.ToDate Between '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "')   Left Join Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Details H On G.RowID = H.Master_ID and B.CountCode = H.CountCode and B.TypeCode = H.TypeCode and B.WeightCode = H.WeightCode  Where A.FromDate BetWEen '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' and A.Proc_Type = " + TxtProcess.Tag + "  and A.Company_Code = " + MyParent.CompCode + " and H.Master_ID Is Null Group By  D.Name, F.bagWeight, C.Name, Proc_Type, B.WeightCode, B.CountCode, B.TypeCode Order by D.Name ", string.Empty, 180, 100, 100, 100, 100, 120);
                        }
                        else if (TxtProcess.Tag.ToString() == "13")
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select D.Waste COUNTNAME, Sum(PAcked) Packed, Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,B.WeightCode) RATE, Cast(Cast(F.bagWeight as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) KGS, Cast(Cast(Cast(F.bagWeight as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) * Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,B.WeightCode) as Numeric(20,3)) AMOUNT, C.Name PACK_NAME, SUM(Packed) PACKED1, F.BagWeight Weight, B.WeightCode, B. CountCode, B.TypeCode, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' FROMDATE, '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' TODATE  From Packing_Master A Inner Join Packing_Details B On A.RowID = B.Master_ID Inner Join Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join  Waste_Sales_To_Irulappa_Contract(" + MyParent.CompCode + ") D On B.CountCode = D.WasCode and A.FromDAte = D.Invoice_Date  Inner Join Mas_Bagweight F On F.WeightCode = B.WeightCode Where A.FromDate BetWEen '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' and A.Proc_Type = " + TxtProcess.Tag + " and A.Company_Code = " + MyParent.CompCode + " Group By  F.bagWeight, C.Name, Proc_Type, B.WeightCode,  B.TypeCode, D.Waste , B. CountCode", string.Empty, 150, 100, 100, 100, 120);
                        }
                        else
                        {
                            Dr = Tool.Selection_Tool_Except_New("COUNTNAME", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "COUNTNAME", "Select D.Name + '`s ' + E.ShortName + '(' + F.Weight + ')' COUNTNAME, C.Name PACK_NAME, Sum(PAcked) Packed, Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,WeightCode) RATE, Cast(Cast(F.Weight as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) KGS, Cast(Cast(Cast(F.Weight as Numeric(20,3)) * Sum(PACKED) as Numeric(20,3)) * Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,WeightCode) as Numeric(20,3)) AMOUNT, SUM(Packed) PACKED1, F.Weight, B.WeightCode, B.CountCode, B.TypeCode, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' FROMDATE, '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' TODATE  From Vaahini_Erp_Gainup.Dbo.Packing_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Packing_Details B On A.RowID = B.Master_ID Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_Type = C.RowID  Inner Join  Vaahini_Erp_Gainup.Dbo.CountName(" + MyParent.CompCode + ") D On B.CountCode = D.Code Inner Join Vaahini_Erp_Gainup.Dbo.CountType(" + MyParent.CompCode + ") E On B.TypeCode = E.Code Inner Join Vaahini_Erp_Gainup.Dbo.CountWeight(" + MyParent.CompCode + ") F On F.Code = B.WeightCode Where A.FromDate BetWEen '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' and A.Proc_Type = " + TxtProcess.Tag + " and A.Company_Code = " + MyParent.CompCode + " Group By D.Name, E.ShortName, F.Weight, C.Name, Proc_Type, WeightCode, B.CountCode, B.TypeCode Order by D.Name + '`s ' + E.ShortName + '(' + F.Weight + ')' ", string.Empty, 180, 100, 100, 100, 100, 120);
                        }
                       // Dr = Tool.Selection_Tool_Except_New("SNO",this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "WEIGHT", "Select WEIGHT, NAME PACK_NAME, FROMDATE, TODATE, PACKED, Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,WeightCode)  RATE , Cast(Cast(Weight as Numeric(20,3)) * PACKED as Numeric(20,3)) KGS, Cast(Cast(Weight as Numeric(20,3)) * Dbo.Contract_Rate_Fn_ProcessWise('" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "',Proc_Type,WeightCode) as Numeric(20,3)) AMOUNT, PACKED PACKED1, ROW_NUMBER() Over (Order by WEIGHT,fromdate, Todate) SNO, Proc_Type, WeightCode From Packed_Bags(" + MyParent.CompCode + ") A Inner Join CountWeight(" + MyParent.CompCode + ") C On A.WeightCode = C.Code Inner Join Contract_Process_Name D On A.Proc_Type = D.RowID ", string.Empty, 100, 120, 100, 100, 100, 100, 120);
                        if (Dr != null)
                        {
                            Grid["WEIGHT", Grid.CurrentCell.RowIndex].Value = Dr["WEIGHT"].ToString();
                            Grid["COUNTNAME", Grid.CurrentCell.RowIndex].Value = Dr["COUNTNAME"].ToString();                            
                            Grid["FROMDATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Dr["FROMDATE"].ToString());
                            Grid["TODATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDateTime(Dr["TODATE"].ToString());
                            Grid["PACKED", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["PACKED"].ToString());                            
                            Grid["KGS", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["KGS"].ToString());
                            Grid["RATE", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["RATE"].ToString());
                            Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["AMOUNT"].ToString());
                            Grid["PACKED1", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["PACKED1"].ToString());
                            Grid["WEIGHTCODE", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["WeightCode"].ToString());
                            Grid["ACTUALCODE", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["CountCode"].ToString());
                            Grid["TYPECODE", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Dr["TypeCode"].ToString());  
                            Txt.Text = Dr["COUNTNAME"].ToString();
                        }
                    }
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PACKED"].Index)
                {
                    if (Grid["RATE", Grid.CurrentCell.RowIndex].Value == null || Grid["RATE", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value) == 0)
                    {
                        Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = "0.00";
                    }
                    else
                    {
                        if (Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0)
                        {
                            Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = "0.00";
                        }
                        else
                        {
                            if (Grid["KGS", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["RATE", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                            {
                                //if (TxtProcess.Tag.ToString() == "1")
                                //{
                                    Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["PACKED", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value));
                                //}
                                //else
                                //{
                                //    Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["KGS", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["RATE", Grid.CurrentCell.RowIndex].Value));
                                //}
                            }
                        }
                    }
                }
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
                if (TxtDeduct.Text.ToString() == String.Empty)
                {
                    TxtDeduct.Text = "0.00";
                }
                TxtGross.Text = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "AMOUNT", "PACKED")));
                TxtKgs.Text = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "PACKED", "COUNTNAME")));
                Amt = Convert.ToDouble(TxtGross.Text.ToString()) - Convert.ToDouble(TxtDeduct.Text.ToString());
                TxtNet.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0}", Amt)));
                TxtRo.Text = String.Format("{0:n}", Convert.ToDouble(TxtNet.Text) - (Amt));
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
                    MyBase.Valid_Number(Txt, e);
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
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PACKED"].Index)
                    {
                        if (Grid["PACKED1", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                        {
                            if (Convert.ToDouble(Grid["PACKED", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["PACKED1", Grid.CurrentCell.RowIndex].Value))
                            {
                                MessageBox.Show("Invalid Packed..!", "Gainup");
                                Grid["PACKED", Grid.CurrentCell.RowIndex].Value = 0.00;                                
                                Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = 0.00;
                                Grid.CurrentCell = Grid["PACKED", Grid.CurrentCell.RowIndex];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                e.Handled = true;
                                return;
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


        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Total();
                    TxtDeduct.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmContractWorkerWages_Load(object sender, EventArgs e)
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

                if (TxtProcess.Tag.ToString() == "1" || TxtProcess.Tag.ToString() == "2")
                {
                    Str = "Select A.Entry_No, A.Effect_Date, D.NAME Party, C.Name Process, B.SNO, F.Name + '`s ' + G.ShortName + '(' + E.Weight + ')' COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT, A.Deduction_Amount, A.Gross_Amount, A.RO_Amount, A.Net_Amount , A.RowID  From  Contract_Worker_Wages_Master A Inner Join Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Left Join Spinning_Contract_NAme D On D.RowID = A.Party_Code  Inner Join CountName(" + MyParent.CompCode + ") F On B.CountCode = F.Code Inner Join CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + " ";
                }
                else if (TxtProcess.Tag.ToString() == "5" || TxtProcess.Tag.ToString() == "8")
                {
                    Str = "Select A.Entry_No, A.Effect_Date, D.NAME Party, C.Name Process, B.SNO, '" + TxtProcess.Text.ToString() + "' COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT, A.Deduction_Amount, A.Gross_Amount, A.RO_Amount, A.Net_Amount , A.RowID  From  Contract_Worker_Wages_Master A Inner Join Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Left Join Spinning_Contract_NAme D On D.RowID  = A.Party_Code  left Join CountName(" + MyParent.CompCode + ") F On B.CountCode = F.Code left Join CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + " ";
                }
                else if (TxtProcess.Tag.ToString() == "3" || TxtProcess.Tag.ToString() == "4" ||  TxtProcess.Tag.ToString() == "7")
                {
                    Str = "Select A.Entry_No,  A.Effect_Date, D.NAME Party, C.Name Process, B.SNO, F.Name COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT, A.Deduction_Amount, A.Gross_Amount, A.RO_Amount, A.Net_Amount , A.RowID  From  Contract_Worker_Wages_Master A Inner Join Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Left Join Spinning_Contract_NAme D On D.RowID  = A.Party_Code  Inner Join Waste_Item_Master  F On B.CountCode = F.RowID LEft Join CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + " ";
                }
                else if (TxtProcess.Tag.ToString() == "6")
                {
                    Str = "Select A.Entry_No,  A.Effect_Date, D.NAME Party, C.Name Process, B.SNO, F.StoppageName COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT, A.Deduction_Amount, A.Gross_Amount, A.RO_Amount, A.Net_Amount , A.RowID  From  Contract_Worker_Wages_Master A Inner Join Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Left Join Spinning_Contract_NAme D On D.RowID  = A.Party_Code  Inner Join MasStoppage  F On B.CountCode = F.StoppageCode LEft Join CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + "  ";
                }
                else if (TxtProcess.Tag.ToString() == "12")
                {
                    Str = "Select A.Entry_No,  A.Effect_Date, D.NAME Party, C.Name Process, B.SNO, F.Name COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT, A.Deduction_Amount, A.Gross_Amount, A.RO_Amount, A.Net_Amount , A.RowID  From  Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join Vaahini_Erp_Gainup.Dbo.CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Left Join Vaahini_Erp_Gainup.Dbo.Spinning_Contract_NAme D On D.RowID  = A.Party_Code  Inner Join Vaahini_Erp_Gainup.Dbo.Loading_Item_Name_Master F On B.CountCode = F.RowID   LEft Join Vaahini_Erp_Gainup.Dbo.CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + "  ";
                }
                else if (TxtProcess.Tag.ToString() == "13")
                {
                    Str = "Select A.Entry_No,  A.Effect_Date, D.NAME Party, C.Name Process, B.SNO, F.Name COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT, A.Deduction_Amount, A.Gross_Amount, A.RO_Amount, A.Net_Amount , A.RowID  From  Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join Vaahini_Erp_Gainup.Dbo.CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Left Join Vaahini_Erp_Gainup.Dbo.Spinning_Contract_NAme D On D.RowID  = A.Party_Code  Inner Join Vaahini_Erp_Gainup.Dbo.Waste_Item_Master F On B.CountCode = F.RowID   LEft Join Vaahini_Erp_Gainup.Dbo.CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + "  ";
                }
                else
                {
                    Str = "Select A.Entry_No,  A.Effect_Date, D.NAME Party, C.Name Process, B.SNO, F.Name + '`s ' + G.ShortName + '(' + E.Weight + ')' COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT, A.Deduction_Amount, A.Gross_Amount, A.RO_Amount, A.Net_Amount , A.RowID  From  Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join Vaahini_Erp_Gainup.Dbo.CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Left Join Vaahini_Erp_Gainup.Dbo.Spinning_Contract_NAme D On D.RowID  = A.Party_Code  Inner Join Vaahini_Erp_Gainup.Dbo.CountName(" + MyParent.CompCode + ") F On B.CountCode = F.Code Inner Join Vaahini_Erp_Gainup.Dbo.CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + " ";
                }
              //  Str = Str + " and A.Approve_Status = 'F' "; 
                MyBase.Execute_Qry(Str, "RptContractWagesProcess");
                CrystalDecisions.CrystalReports.Engine.ReportDocument ORpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\RptContractWagesProcess.rpt");
                MyParent.FormulaFill(ref ORpt, "CompName", MyParent.CompName);
                MyParent.FormulaFill(ref ORpt, "Heading", " " + TxtProcess.Text + " From   " + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "   To   " + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + " ");
                MyParent.FormulaFill(ref ORpt, "PDate", string.Format("{0:dd-MMM-yyyy} {0:T}", MyBase.GetServerDateTime()));
                MyParent.CReport(ref ORpt, "CONTRACT WAGES INVOICE..!");

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
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Contract Wages- Edit", "Select A.Entry_No, A.FromDate, A.ToDate, D.NAME Party, C.Name Process_Type, A.Tot_Kgs,  A.Gross_Amount, A.Deduction_Amount, A.Ro_Amount, A.Net_Amount, A.Remarks, A.RowID, A.Proc_ID, A.Party_Code, A.Effect_Date From  Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_ID = C.RowID Left Join Vaahini_Erp_Gainup.Dbo.Spinning_Contract_NAme D On D.RowID  = A.Party_Code Where BillPass_Status = 0 and Approve_Status = 'F' and A.Company_Code = " + MyParent.CompCode + " and A.Proc_Type = 12", string.Empty, 80, 80, 200, 100, 120, 100);
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
                    MyBase.Run("Delete From Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Details Where Master_ID = " + Code, " Delete From Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Master Where RowID = " + Code);
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
                Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Contract Wages- Edit", "Select A.Entry_No, A.FromDate, A.ToDate, D.NAME Party, C.Name Process_Type,  A.Tot_Kgs,  A.Gross_Amount, A.Deduction_Amount, A.Ro_Amount, A.Net_Amount, A.Remarks, A.RowID, A.Proc_ID, A.Party_Code, A.Effect_Date From  Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_ID = C.RowID Left Join Vaahini_Erp_Gainup.Dbo.Spinning_Contract_NAme D On D.RowID  = A.Party_Code Where  A.Company_Code = " + MyParent.CompCode + " and A.Proc_ID = 12", string.Empty, 80, 80, 80, 200, 100, 120, 100);
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

        void Grid_Data()
        {            
            String Str = String.Empty;
            try
            {
                if (MyParent._New == true)
                {
                    Str = "Select  0 SNO, '' COUNTNAME, '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' FROMDATE,  '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' TODATE,  0 PACKED, 0.00 KGS, 0.00 RATE, 0.00 AMOUNT, 0 PACKED1, 0 WEIGHTCODE, 0.00 WEIGHT, 0 ACTUALCODE, 0 TYPECODE   From Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Details Where 1 = 2";                 
                }
                else
                {
                    if (TxtProcess.Tag.ToString() == "1" || TxtProcess.Tag.ToString() == "2" || TxtProcess.Tag.ToString() == "9")
                    {
                        Str = "Select B.SNO, F.Name + '`s ' + G.ShortName + '(' + E.Weight + ')' COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT From  Contract_Worker_Wages_Master A Inner Join Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Left Join Spinning_Contract_NAme D On D.RowID  = A.Party_Code  Inner Join CountName(" + MyParent.CompCode + ") F On B.CountCode = F.Code Inner Join CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + " Order By SNo  ";
                    }
                    else if (TxtProcess.Tag.ToString() == "5" || TxtProcess.Tag.ToString() == "8")
                    {
                        Str = "Select B.SNO, '" + TxtProcess.Text.ToString() + "' COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT From  Contract_Worker_Wages_Master A Inner Join Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Left Join Spinning_Contract_NAme D On D.RowID  = A.Party_Code  left Join CountName(" + MyParent.CompCode + ") F On B.CountCode = F.Code left Join CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + " Order By SNo  ";
                    }
                    else if (TxtProcess.Tag.ToString() == "3" || TxtProcess.Tag.ToString() == "4" || TxtProcess.Tag.ToString() == "7" || TxtProcess.Tag.ToString() == "10")
                    {
                        Str = "Select B.SNO, F.Name COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT From  Contract_Worker_Wages_Master A Inner Join Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Left Join Spinning_Contract_NAme D On D.RowID  = A.Party_Code  Inner Join Waste_Item_Master  F On B.CountCode = F.RowID LEft Join CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + " Order By SNo  ";
                    }
                    else if (TxtProcess.Tag.ToString() == "6")
                    {
                        Str = "Select B.SNO, F.StoppageName COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT From  Contract_Worker_Wages_Master A Inner Join Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Left Join Spinning_Contract_NAme D On D.RowID  = A.Party_Code  Inner Join MasStoppage  F On B.CountCode = F.StoppageCode LEft Join CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + " Order By SNo  ";
                    }
                    else if (TxtProcess.Tag.ToString() == "12")
                    {
                        Str = "Select B.SNO, F.Name COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT From  Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join Vaahini_Erp_Gainup.Dbo.CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Left Join Vaahini_Erp_Gainup.Dbo.Spinning_Contract_NAme D On D.RowID  = A.Party_Code  Inner Join Vaahini_Erp_Gainup.Dbo.Loading_Item_Name_Master F On B.CountCode = F.RowID LEft Join Vaahini_Erp_Gainup.Dbo.CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + " Order By SNo  ";
                    }
                    else if (TxtProcess.Tag.ToString() == "13")
                    {
                        Str = "Select B.SNO, F.Name COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT From  Contract_Worker_Wages_Master A Inner Join Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Left Join Spinning_Contract_NAme D On D.RowID  = A.Party_Code  Inner Join Waste_Item_Master F On B.CountCode = F.RowID LEft Join CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + " Order By SNo  ";
                    }
                    //else if (TxtProcess.Tag.ToString() == "9")
                    //{
                    //    Str = "Select Distinct B.SNO, F.Count COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT From  Contract_Worker_Wages_Master A Inner Join Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Left Join Spinning_Contract_NAme D On D.RowID  = A.Party_Code  Inner Join  Packed_Contract_sales(" + MyParent.CompCode + ") F On B.CountCode = F.CountCode and B.TypeCode = F.CountTypeCode and B.WeightCode = F.CountWeightCode LEft Join CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + " Order By SNo  ";
                    //}
                    else
                    {
                        Str = "Select B.SNO, F.Name + '`s ' + G.ShortName + '(' + E.Weight + ')' COUNTNAME, B.FROMDATE, B.TODATE, B.PACKED, B.KGS, B.RATE, B.AMOUNT, B.Packed PACKED1, B.WeightCode, B.CountCode ACTUALCODE, B.TypeCode, E.WEIGHT From  Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Master A Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Details  B On A.RowID = B.Master_ID    Inner Join Vaahini_Erp_Gainup.Dbo.Contract_Process_Name C On A.Proc_ID = C.RowID  Inner Join Vaahini_Erp_Gainup.Dbo.CountWeight(" + MyParent.CompCode + ") E On B.WeightCode = E.Code  Vaahini_Erp_Gainup.Dbo.Spinning_Contract_NAme D On D.RowID  = A.Party_Code  Inner Join Vaahini_Erp_Gainup.Dbo.CountName(" + MyParent.CompCode + ") F On B.CountCode = F.Code Inner Join Vaahini_Erp_Gainup.Dbo.CountType(" + MyParent.CompCode + ") G On B.TypeCode = G.Code Where  Master_ID = " + Code + " Order By SNo  ";
                    }
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid_Without(ref Grid, "COUNTNAME", "PACKED");
                MyBase.Grid_Designing(ref Grid, ref Dt, "FROMDATE", "TODATE", "WEIGHT", "PACKED1",  "WEIGHTCODE", "ACTUALCODE", "TYPECODE");
                MyBase.Grid_Width(ref Grid, 50, 180, 100, 120, 80, 120);
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                
                //Grid.Columns["WEIGHT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                //Grid.Columns["FROMDATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //Grid.Columns["TODATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["PACKED"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["KGS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["RATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                TxtDeduct.Text = "0.00";
            }
            catch (Exception ex)
            {
                throw ex;
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
               
                TxtProcess.Text= "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DtpTDate_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(DtpTDate.Value) > MyBase.GetServerDateTime())
                {
                    MessageBox.Show("Invalid Date", "Gainup");
                    DtpTDate.Value = MyBase.GetServerDateTime();
                    TxtProcess.Text = "";
                    DtpTDate.Focus();
                    return;
                }
               
                TxtProcess.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmContractWorkerWages_KeyDown(object sender, KeyEventArgs e)
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
                    else if (this.ActiveControl.Name == "TxtDeduct")
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
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Process ", " Select Name , A.RowID  From Vaahini_Erp_Gainup.Dbo.Contract_Process_Name A Left join Vaahini_Erp_Gainup.Dbo.Contract_Worker_Wages_Master B On A.RowID = B.Proc_ID and B.Company_Code = " + MyParent.CompCode + " and (B.FromDate Between '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "' or B.ToDate Between '" + String.Format("{0:dd-MMM-yyyy}", DtpFDate.Value) + "' and '" + String.Format("{0:dd-MMM-yyyy}", DtpTDate.Value) + "')  Where B.FromDate is null and A.RowID = 12  ", string.Empty, 220);    
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
                        //Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Party", "Select Party, Code LEDGERCODE  From  Accounts.dbo.Debtors_Creditors(" + MyParent.CompCode + ",'" + MyParent.YearCode + "') Where Party Not Like '%ZZZ%'", string.Empty, 400);
                        Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Party", "Select Name Party, RowID LEDGERCODE  From  Vaahini_Erp_Gainup.Dbo.Spinning_Contract_NAme ", string.Empty, 400);
                        if (Dr != null)
                        {
                            TxtParty.Text = Dr["Party"].ToString();
                            TxtParty.Tag = Dr["LEDGERCODE"].ToString();
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

        private void FrmContractWorkerWages_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == "TxtRemarks" || this.ActiveControl.Name == String.Empty )
                    {
                        MyBase.Return_Ucase(e);                        
                    }
                    else if (this.ActiveControl.Name == "TxtDeduct" )
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
      
    }
}