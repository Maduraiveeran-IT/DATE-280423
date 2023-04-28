using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Accounts
{
    public partial class FrmNrgpEntry : Form,Entry
    {

        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataRow Dr;
        TextBox Txt = null;
        String appName; String ModuleName;
        String Str;
        String[] Quries = null;
        Int64 Array_Index = 0;
        Int64 Code = 0;
        DataTable Dts = new DataTable();

        public FrmNrgpEntry()
        {
            InitializeComponent();
        }

        #region Entry Members

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                RbnCurno.Checked = false;
                RbnCuryes.Checked = false;
                BtnControls("Entry_New");
                ClearControls(GBMain, "Entry_New");
               
               
                
                GetServerDate();
                Grid_Data();
                TxtRgpno.Text = GetRgpno();
                LoadEmployee();
                TxtCourier.Enabled = false;
                TxtCompany.Focus();
                GrpPrint.Visible = false;
                RptRgp.Visible = false;
                 
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
                Array_Index = 0;
                Quries = new String[Dt.Rows.Count * 10];


                if (TxtCompany.Text == String.Empty)
                {
                    MessageBox.Show("Select Company...!");
                    MyParent.Save_Error = true;
                    TxtCompany.Focus();
                    return;
                }
                if (TxtRgptype.Text == String.Empty)
                {
                    MessageBox.Show("Select RgpType...!");
                    MyParent.Save_Error = true;
                    TxtRgptype.Focus();
                    return;
                }
                if (TxtRgpno.Text == String.Empty)
                {
                    MessageBox.Show("Invalid Rgpno...!");
                    MyParent.Save_Error = true;
                    TxtRgpno.Focus();
                    return;
                }
                if (TxtDivision.Text == String.Empty)
                {
                    MessageBox.Show("Select Division...!");
                    MyParent.Save_Error = true;
                    TxtDivision.Focus();
                    return;
                }
                if (TxtsupplierName.Text == String.Empty)
                {
                    MessageBox.Show("Select Supplier NAme...!");
                    MyParent.Save_Error = true;
                    TxtsupplierName.Focus();
                    return;
                }


                if (TxtRgptype.Tag.ToString() == "1")
                {
                    if (TxtsupplierName.Tag.ToString() == String.Empty)
                    {
                        TxtsupplierName.Tag = "0";
                         
                    }

                }
               
                if (TxtSampleName.Text == String.Empty && TxtDivision.Tag.ToString() == "2")
                {
                    MessageBox.Show("Select Sample NAme...!");
                    MyParent.Save_Error = true;
                    TxtSampleName.Focus();
                    return;
                }
                else
                {
                    if (TxtSampleName.Text == String.Empty)
                    {
                        TxtSampleName.Tag = "0";
                    }
                }
                
                if (TxtOrderno.Text == String.Empty && TxtDivision.Tag.ToString() == "2")
                {
                    MessageBox.Show("Select Orderno...!");
                    MyParent.Save_Error = true;
                    TxtOrderno.Focus();
                    return;
                }
                else
                {
                    if (TxtColor.Text == String.Empty)
                    {
                        TxtColor.Tag = "0";
                    }
                    if (TxtOrderno.Text == String.Empty)
                    {
                       
                        TxtOrderno.Text = "--------";
                    }
                }
                if (RbnCuryes.Checked == false && RbnCurno.Checked == false)
                {
                    MessageBox.Show("Select Courier YES/NO...!");
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtRefno.Text == String.Empty)
                {
                    TxtRefno.Text = "-";
                }
                if (TxtRefName.Text == String.Empty)
                {
                    MessageBox.Show("Select Referby Name...!");
                    TxtReftno.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtDespatch.Text == String.Empty)
                {
                    TxtDespatch.Text = "-";
                }
                if(TxtRemarks.Text == String.Empty)
                {
                    TxtRemarks.Text = "-";
                }
               
                 
                 

               

                String Courier_Mode = String.Empty;
                if (RbnCurno.Checked == true)
                {
                    Courier_Mode = "N";
                    TxtCourier.Tag = "0";
                }
                if (RbnCuryes.Checked == true)
                {
                    Courier_Mode = "Y";

                    if (TxtCourier.Text == String.Empty)
                    {
                        MessageBox.Show("Select Courier NAme...!");
                        MyParent.Save_Error = true;
                        TxtCourier.Focus();
                        return;
                    }

                }


                if (Dt.Rows.Count <= 0)
                {
                    MessageBox.Show("Invalid Item Details...!");
                    MyParent.Save_Error = true;
                    Grid.CurrentCell = Grid["ITEMDESCRIPTION", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    return;

                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Grid["AMOUNT", i].Value.ToString() == String.Empty)
                    {
                        Grid["AMOUNT", i].Value = "0";
                    }

                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                   
                    for (int j = 1; j < Dt.Columns.Count - 1; j++)
                    {
                        if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty)
                        {
                            if (Grid.Columns[j].Name.ToString().ToUpper() != "DUEDATE")
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








                if (BtnCtl_New.Enabled == true)
                {
                    TxtRgpno.Text = GetRgpno();

                    Quries[Array_Index++] = "Insert into VAAHINI_ERP_GAINUP.dbo.NRGP_DCMASTER(rgpNO,rgpDATE,LEDGERCODE,POTYPE,DESP,SPLINST,REFQUOTNO,RGPNO2,REFQUOTDATE,partyname,Emplno,CompCode,Division,Sample_ID,Order_No,Color_ID,Courier_Mode,Entry_Date,Entry_System,CourierCode,Refbyemplno,EntryBy,RefBy,DivisionName,AccYearCode)values('" + TxtRgpno.Text + "','" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "'," + TxtsupplierName.Tag + "," + TxtRgptype.Tag + ",'" + TxtDespatch.Text + "','" + TxtRemarks.Text + "','" + TxtRefno.Text + "','" + TxtRgpno.Text + "','" + String.Format("{0:dd-MMM-yyyy}", RefDate.Value) + "','" + TxtsupplierName.Text.ToString() + "'," + TxtEmplname.Tag + "," + TxtCompany.Tag + "," + TxtDivision.Tag + "," + TxtSampleName.Tag + ",'" + TxtOrderno.Text + "'," + TxtColor.Tag + ",'" + Courier_Mode + "',Getdate(),Host_Name(),(Case when '" + Courier_Mode + "'='N' Then Null Else " + TxtCourier.Tag + " End)," + TxtRefName.Tag.ToString() + ",'" + TxtEmplname.Text + "','" + TxtRefName.Text + "','" + TxtDivision.Text + "',VAAHINI_ERP_GAINUP.dbo.Get_Accounts_YearCode('" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "'))";
                }
                else
                {
                    Quries[Array_Index++] = "Update VAAHINI_ERP_GAINUP.dbo.NRGP_DCMASTER Set rgpDATE='" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "',LEDGERCODE=" + TxtsupplierName.Tag + ",POTYPE=" + TxtRgptype.Tag + ",DESP='" + TxtDespatch.Text + "',SPLINST='" + TxtRemarks.Text + "',REFQUOTNO='" + TxtRefno.Text + "',RGPNO2='" + TxtRgpno.Text + "',REFQUOTDATE='" + String.Format("{0:dd-MMM-yyyy}", RefDate.Value) + "',partyname='" + TxtsupplierName.Text.ToString() + "',Emplno=" + TxtEmplname.Tag + ",Compcode=" + TxtCompany.Tag + ",Division=" + TxtDivision.Tag + ",Refbyemplno=" + TxtRefName.Tag + ",Sample_ID=" + TxtSampleName.Tag + ",Order_No='" + TxtOrderno.Text + "',Color_ID=" + TxtColor.Tag + ",Courier_Mode='" + Courier_Mode + "',Entry_System=Host_Name(),CourierCode=(Case When '" + Courier_Mode + "'='N' Then Null Else " + TxtCourier.Tag + " End),Entryby='" + TxtEmplname.Text + "',RefBy='" + TxtRefName.Text + "',DivisionName='" + TxtDivision.Text + "',AccYearCode=VAAHINI_ERP_GAINUP.dbo.Get_Accounts_YearCode('" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "') Where Rowid=" + Code + "";
                    Quries[Array_Index++] = "Delete From VAAHINI_ERP_GAINUP.dbo.Nrgp_dcDETAIL Where Rgpno='" + TxtRgpno.Text + "'";
                }

                
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (BtnCtl_New.Enabled == true)
                    {


                        Quries[Array_Index++] = "Insert into VAAHINI_ERP_GAINUP.dbo.Nrgp_dcDETAIL(rgpno,RGPDATE,SLNO,itemdesc,purpose,rgpQTY,RATE,cancelqty,uom,RECQTY,compcode,yearcode) Values('" + TxtRgpno.Text + "','" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "'," + Grid["SNO", i].Value + ",'" + Grid["ITEMDESCRIPTION", i].Value + "','" + Grid["PURPOSE", i].Value + "'," + Grid["RGPQTY", i].Value + "," + Grid["AMOUNT", i].Value + ",0,'" + Grid["UOM", i].Value + "',0," + TxtCompany.Tag + ",Cast(YEAR('" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "') As numeric))";

                    }
                    else
                    {

                        Quries[Array_Index++] = "Insert into VAAHINI_ERP_GAINUP.dbo.Nrgp_dcDETAIL(rgpno,RGPDATE,SLNO,itemdesc,purpose,rgpQTY,RATE,cancelqty,uom,RECQTY,compcode,yearcode) Values('" + TxtRgpno.Text + "','" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "'," + Grid["SNO", i].Value + ",'" + Grid["ITEMDESCRIPTION", i].Value + "','" + Grid["PURPOSE", i].Value + "'," + Grid["RGPQTY", i].Value + "," + Grid["AMOUNT", i].Value + ",0,'" + Grid["UOM", i].Value + "',0," + TxtCompany.Tag + ",Cast(YEAR('" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "') As numeric))";

                    }


                }
                 DialogResult m = MessageBox.Show("Sure to Save...!", "Nrgp Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                 if (m == DialogResult.Yes)
                 {
                     MyBase.Run(Quries);
                     MessageBox.Show("Saved..!");
                     MyParent.Save_Error = false;
                     MyBase.Clear(this);
                     TxtCompany.Focus();
                     ClearControls(GBMain, "Entry_New");
                     BtnControls("Entry_New");
                 }
                 else
                 {

                 }


            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        void GetServerDate()
        {
            try
            {


                RgpDate.MaxDate = Convert.ToDateTime(MyBase.GetServerDate()).Date;
                RefDate.MaxDate = Convert.ToDateTime(MyBase.GetServerDate()).Date;
                 
                RgpDate.Value = Convert.ToDateTime(MyBase.GetServerDate()).Date;
                RefDate.Value = Convert.ToDateTime(MyBase.GetServerDate()).Date;
                

                if (MyParent.UserCode == 1)
                {
                    RgpDate.Enabled = true;

                }
                else
                {
                    RgpDate.Enabled = false;


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
                Str = "SELECT rgpNO, rgpDATE, LedgerName, itemdesc, rgpQTY, uom ,EntryBy,RefBy,DivisionName,DESP ,SPLINST ,Courier_Mode,Emplno,LEDGERCODE,CompCode,Division,POTYPE,Sample_ID,Order_No,Color_ID,RowID,CourierCode,REFQUOTNO,REFQUOTDATE,Refbyemplno   FROM  VAAHINI_ERP_GAINUP.dbo.Vaahini_NRgpEntry_Fn(" + MyParent.UserCode + "," + MyParent.Emplno + ",'EDIT')";
                Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "EDIT..!", Str, String.Empty, 100, 90, 180, 180, 100, 90, 150, 150, 130);
                if (Dr != null)
                {
                    BtnControls("Entry_Edit");
                    ClearControls(GBMain, "Entry_Edit");
                    FillDatas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void FillDatas()
        {
            try
            {
                GetServerDate();
                Code = Convert.ToInt64(Dr["Rowid"]);
                TxtRgpno.Text = Dr["rgpNO"].ToString();
                RgpDate.Value = Convert.ToDateTime(String.Format("{0:dd-MMM-yyyy}", Dr["rgpDATE"].ToString())).Date;
                TxtRefno.Text = Dr["REFQUOTNO"].ToString();
                RefDate.Value = Convert.ToDateTime(String.Format("{0:dd-MMM-yyyy}", Dr["REFQUOTDATE"].ToString())).Date;
                TxtsupplierName.Text = Dr["LedgerName"].ToString();
                TxtsupplierName.Tag = Dr["LEDGERCODE"].ToString();
                TxtDespatch.Text = Dr["DESP"].ToString();
                TxtRemarks.Text = Dr["SPLINST"].ToString();
               
                if (Dr["Courier_Mode"].ToString().ToUpper() == "Y")
                {

                    RbnCurno.Checked = false;
                    RbnCuryes.Checked = true;
                    TxtCourier.Enabled = true;
                }
                else
                {
                    RbnCurno.Checked = true;
                    RbnCuryes.Checked = false;
                    TxtCourier.Enabled = false;
                }

                Grid_Data();
                String SQl = String.Empty; DataTable Dt_Sql = new DataTable();
                SQl = "Select CompNAme,CompCode From VAAHINI_ERP_GAINUP.dbo.Stores_Companymas Where CompCode=" + Dr["CompCode"].ToString() + "";
                MyBase.Load_Data(SQl, ref Dt_Sql);
                if (Dt_Sql.Rows.Count > 0)
                {
                    TxtCompany.Text = Dt_Sql.Rows[0]["CompNAme"].ToString();
                    TxtCompany.Tag = Dt_Sql.Rows[0]["CompCode"].ToString();

                }
                SQl = "Select  Div_Name Division,Div_Code From VAAHINI_ERP_GAINUP.dbo.Rgp_Division() Where Div_Code=" + Dr["Division"].ToString() + "";
                MyBase.Load_Data(SQl, ref Dt_Sql);
                if (Dt_Sql.Rows.Count > 0)
                {
                    TxtDivision.Text = Dt_Sql.Rows[0]["Division"].ToString();
                    TxtDivision.Tag = Dt_Sql.Rows[0]["Div_Code"].ToString();

                    if (TxtDivision.Tag.ToString() == "2")
                    {

                        TxtSampleName.Enabled = true;
                        TxtOrderno.Enabled = true;
                        TxtColor.Enabled = true;
                    }
                    else
                    {
                        TxtSampleName.Enabled = false;
                        TxtOrderno.Enabled = false;
                        TxtColor.Enabled = false;
                    }

                }
                else
                {
                    TxtSampleName.Enabled = false;
                    TxtOrderno.Enabled = false;
                    TxtColor.Enabled = false;
                }

                SQl = "Select Type,typecode From (values(0,'GENERAL'),(1,'SAMPLE')) x(typecode,Type) Where Typecode=" + Dr["POTYPE"].ToString() + "";
                MyBase.Load_Data(SQl, ref Dt_Sql);
                if (Dt_Sql.Rows.Count > 0)
                {
                    TxtRgptype.Text = Dt_Sql.Rows[0]["Type"].ToString();
                    TxtRgptype.Tag = Dt_Sql.Rows[0]["typecode"].ToString();
                }

                if (RbnCuryes.Checked == true)
                {
                    SQl = "Select COURIERNAME,ccode CourierCode FRom VAAHINI_ERP_GAINUP.dbo.Sec_Mas_Courier Where ccode=" + Dr["CourierCode"].ToString() + "";
                    MyBase.Load_Data(SQl, ref Dt_Sql);
                    if (Dt_Sql.Rows.Count > 0)
                    {
                        TxtCourier.Text = Dt_Sql.Rows[0]["COURIERNAME"].ToString();
                        TxtCourier.Tag = Dt_Sql.Rows[0]["CourierCode"].ToString();


                    }
                }
                

                SQl = "Select A.Tno, A.Name,A.Emplno  From VAAHINI_ERP_GAINUP.dbo.EmployeeMas A  Where A.Emplno="+Dr["Emplno"].ToString() +"";
                MyBase.Load_Data(SQl, ref Dt_Sql);
                if (Dt_Sql.Rows.Count > 0)
                {
                    TxtEmplname.Text = Dt_Sql.Rows[0]["Name"].ToString();
                    TxtEmplname.Tag = Dt_Sql.Rows[0]["Emplno"].ToString();
                    TxtTno.Text = Dt_Sql.Rows[0]["Tno"].ToString();
                }


                SQl = "Select A.Tno, A.Name,A.Emplno  From VAAHINI_ERP_GAINUP.dbo.EmployeeMas A  Where A.Emplno=" + Dr["Refbyemplno"].ToString() + "";
                MyBase.Load_Data(SQl, ref Dt_Sql);
                if (Dt_Sql.Rows.Count > 0)
                {
                    TxtRefName.Text = Dt_Sql.Rows[0]["Name"].ToString();
                    TxtRefName.Tag = Dt_Sql.Rows[0]["Emplno"].ToString();
                    TxtReftno.Text = Dt_Sql.Rows[0]["Tno"].ToString();
                }

                if (TxtDivision.Tag.ToString() == "2")
                {
                    SQl = "Select * From (Select 'Others' Order_No, '.' Color, 0 Color_Id Union Select ORder_No, Color, ColorID From VAAHINI_ERP_GAINUP.dbo.mis_order_details_Color()) S Where ORder_No='" + Dr["ORder_No"].ToString() + "'";
                    MyBase.Load_Data(SQl, ref Dt_Sql);
                    if (Dt_Sql.Rows.Count > 0)
                    {
                        TxtOrderno.Text = Dt_Sql.Rows[0]["Order_No"].ToString();
                        TxtColor.Text = Dt_Sql.Rows[0]["Color"].ToString();
                        TxtColor.Tag = Dt_Sql.Rows[0]["Color_Id"].ToString();

                    }

                    SQl = "select Name, Rowid From VAAHINI_ERP_GAINUP.dbo.Sample_Name_MAster Where Rowid=" + Dr["Sample_ID"] + "";
                    MyBase.Load_Data(SQl, ref Dt_Sql);
                    if (Dt_Sql.Rows.Count > 0)
                    {
                        TxtSampleName.Text = Dt_Sql.Rows[0]["Name"].ToString();
                        TxtSampleName.Tag = Dt_Sql.Rows[0]["Rowid"].ToString();
                    }
                }


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
                Str = "SELECT rgpNO, rgpDATE, LedgerName, itemdesc, rgpQTY, uom ,EntryBy,RefBy,DivisionName,DESP ,SPLINST ,Courier_Mode,Emplno,LEDGERCODE,CompCode,Division,POTYPE,Sample_ID,Order_No,Color_ID,RowID,CourierCode,REFQUOTNO,REFQUOTDATE,Refbyemplno   FROM  VAAHINI_ERP_GAINUP.dbo.Vaahini_NRgpEntry_Fn(" + MyParent.UserCode + "," + MyParent.Emplno + ",'DELETE')";
                Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "DELETE..!", Str, String.Empty, 100, 90, 180, 180, 100, 90, 150, 150, 130);
                if (Dr != null)
                {
                    FillDatas();
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
                String[] Queries;
                Array_Index = 0;
                Queries = new String[Dt.Rows.Count + 5 * 5];

                DialogResult m = MessageBox.Show("Sure to Delete...!", "Nrgp Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (m == DialogResult.Yes)
                {

                    Queries[Array_Index++] = "Delete From VAAHINI_ERP_GAINUP.dbo.NRGP_DCDETAIL Where rgpNO='" + TxtRgpno.Text + "' And rgpDATE='" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "'";
                    Queries[Array_Index++] = "Delete From VAAHINI_ERP_GAINUP.dbo.NRGP_DCMaster Where Rgpno='" + TxtRgpno.Text + "' And rgpDATE='" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "'";
                    
                    MyBase.Run(Queries);
                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyBase.Clear(this);
                    BtnControls("ENTRY_CLEAR");

                }
                if (m == DialogResult.No)
                {
                    //MyParent.Load_DeleteEntry();
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
                Str = "SELECT rgpNO, rgpDATE, LedgerName, itemdesc, rgpQTY, uom ,EntryBy,RefBy,DivisionName,DESP ,SPLINST ,Courier_Mode,Emplno,LEDGERCODE,CompCode,Division,POTYPE,Sample_ID,Order_No,Color_ID,RowID,CourierCode,REFQUOTNO,REFQUOTDATE,Refbyemplno   FROM  VAAHINI_ERP_GAINUP.dbo.Vaahini_NRgpEntry_Fn(" + MyParent.UserCode + "," + MyParent.Emplno + ",'VIEW')";
                Dr = Tool.Selection_Tool_Resize(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "VIEW..!", Str, String.Empty, 100, 90, 180, 180, 100, 90,  150, 150, 130);
                if (Dr != null)
                {
                    BtnControls("Entry_View");
                    ClearControls(GBMain, "Entry_View");
                    FillDatas();
                    GrpPrint.Visible = true;
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
                DataTable Dtp = new DataTable();
                String Strpnt = "SELECT Nrgp_dcMASTER.rgpNO,Nrgp_dcMASTER.rgpDATE, Nrgp_dcMASTER.LEDGERCODE, Nrgp_dcMASTER.Approval_Status,Nrgp_dcMASTER.Approval_Status1,Nrgp_dcMASTER.POTYPE, Nrgp_dcMASTER.DESP, Nrgp_dcMASTER.SPLINST, Nrgp_dcMASTER.REFQUOTNO, Nrgp_dcMASTER.REFQUOTDATE, Nrgp_dcDETAIL.SLNO, Nrgp_dcDETAIL.itemdesc, Nrgp_dcDETAIL.purpose, Nrgp_dcDETAIL.rgpQTY , (Select Sum(rgpQTY) From VAAHINI_ERP_GAINUP.dbo.Nrgp_dcDETAIL Where Rgpno='" + TxtRgpno.Text + "') TotalQty ,(Select Sum(RAte) From VAAHINI_ERP_GAINUP.dbo.Nrgp_dcDETAIL Where Rgpno='" + TxtRgpno.Text + "' And Nrgp_dcMASTER.RgpDate='" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "')TotalRate,Nrgp_dcDETAIL.RATE,Isnull((Isnull(Nrgp_dcDETAIL.RATE,0)/Nullif(Nrgp_dcDETAIL.RgpQty,0)),0) NetRAte, Nrgp_dcDETAIL.uom,  Nrgp_DcMaster.partyname FROM VAAHINI_ERP_GAINUP.dbo.Nrgp_dcDETAIL INNER JOIN VAAHINI_ERP_GAINUP.dbo.Nrgp_dcMASTER ON Nrgp_dcDETAIL.rgpno = Nrgp_dcMASTER.rgpNO left join VAAHINI_ERP_GAINUP.dbo.EmployeeMas E1 on Nrgp_dcMASTER.emplno =E1.Emplno Where Nrgp_dcMASTER.rgpNO='" + TxtRgpno.Text + "' And Nrgp_dcMASTER.RgpDate='" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "'";
                MyBase.Load_Data(Strpnt, ref Dtp);


                if (Dtp.Rows[0]["Potype"].ToString() == "0")
                {
                    if (Dtp.Rows[0]["Approval_Status"].ToString().Trim() != "T")
                    {

                        MessageBox.Show("Approval Pending");
                        return;

                    }
                    if (Dtp.Rows[0]["Approval_Status1"].ToString().Trim() != "T")
                    {
                        MessageBox.Show("Second Level Approval Pending");
                        return;
                    }

                }

                if (Dtp.Rows[0]["Potype"].ToString() == "1")
                {


                    if (Dtp.Rows[0]["Approval_Status"].ToString().Trim() != "T")
                    {

                        MessageBox.Show("Approval Pending");
                        return;

                    }
                   

                }
                if (Dtp.Rows.Count > 0)
                {

                    MyBase.Execute_Qry(Strpnt, "tmpNRGPdc");
                    CrystalDecisions.CrystalReports.Engine.ReportDocument ORpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\NRGPDc.rpt");
                    MyParent.FormulaFill(ref ORpt, "head1", TxtCompany.Text);
                    MyParent.FormulaFill(ref ORpt, "head2", "OTTUPATTI,  SINGARAKOTTAI,  DINDIGUL.");
                    MyParent.FormulaFill(ref ORpt, "head3", "Phones : 04543-269000                   GST No   :33445301724         ");
                    MyParent.FormulaFill(ref ORpt, "head4", "Fax    :                                CST No   :133139/21.08.07     ");
                    String Curmode = String.Empty;
                    if (RbnCurno.Checked == true)
                    {
                        Curmode = RbnCurno.Text;
                    }
                    if (RbnCuryes.Checked == true)
                    {
                        Curmode = RbnCuryes.Text;
                    }
                    MyParent.FormulaFill(ref ORpt, "PrintDt", MyBase.GetServerDateTime().ToString());
                    MyParent.FormulaFill(ref ORpt, "Currequest", Curmode);
                    MyParent.FormulaFill(ref ORpt, "typehead",TxtRgptype.Text.ToString().ToUpper() + " GATE PASS(NON RETURNABLE)");
                    MyParent.FormulaFill(ref ORpt, "Refby", TxtReftno.Text.ToString() + " / " + TxtRefName.Text.ToString());
                    MyParent.FormulaFill(ref ORpt, "Entryby", TxtTno.Text.ToString()+" / "+TxtEmplname.Text.ToString());
                    MyParent.FormulaFill(ref ORpt, "Division", TxtDivision.Text.ToString());
                    MyParent.CReport(ref ORpt, "NRGP DETAILS...!");

                }
                else
                {
                    MessageBox.Show("No Record Found..!");
                    return;
                }
                


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        private void FrmNrgpEntry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.KeyCode == Keys.Enter)
                {

                    if (this.ActiveControl.Name == "TxtsupplierName")
                    {
                        Grid.CurrentCell = Grid["ITEMDESCRIPTION", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                    else if (this.ActiveControl.Name == "TxtRemarks")
                    {

                    }
                    else
                    {

                        SendKeys.Send("{TAB}");
                    }
                }
                if (e.KeyCode == Keys.Down && (BtnCtl_New.Enabled == true || BtnCtl_Edit.Enabled == true))
                {


                    if (this.ActiveControl.Name == "TxtCompany")
                    {
                        Str = "Select CompNAme,CompCode From VAAHINI_ERP_GAINUP.dbo.Stores_Companymas";
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Company", Str, String.Empty, 400);
                        if (Dr != null)
                        {
                            TxtCompany.Text = Dr["CompNAme"].ToString();
                            TxtCompany.Tag = Dr["CompCode"].ToString();
                            TxtDivision.Text = String.Empty;
                            TxtDivision.Tag = String.Empty;
                            TxtsupplierName.Text = String.Empty;
                            TxtsupplierName.Tag = String.Empty;
                        }

                    }

                    if (MyParent.UserCode == 1)
                    {
                        if (this.ActiveControl.Name == "TxtTno")
                        {
                            if (TxtDivision.Tag.ToString() == String.Empty)
                            {
                                TxtDivision.Tag = "-10";
                            }
                            if (TxtCompany.Tag.ToString() == String.Empty)
                            {
                                TxtCompany.Tag = "-10";
                            }

                            Str = "Select A.Name,A.Tno,  A.DeptName, A.DesignationName,A.CompName,A.Emplno,A.CompCode From VAAHINI_ERP_GAINUP.dbo.Rgb_NRGP_Division_Employees(" + TxtDivision.Tag + "," + TxtCompany.Tag + "," + MyParent.Emplno + "," + MyParent.UserCode + ",'ALL') A";
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Employee", Str, String.Empty, 100, 200, 150, 150);
                            if (Dr != null)
                            {
                                TxtEmplname.Text = Dr["Name"].ToString();
                                TxtEmplname.Tag = Dr["Emplno"].ToString();
                                TxtTno.Text = Dr["Tno"].ToString();

                            }
                        }
                    
                    }
                    if (this.ActiveControl.Name == "TxtDivision")
                    {
                        if (TxtCompany.Text != String.Empty)
                        {
                            Str = "Select  Division,Div_Code From VAAHINI_ERP_GAINUP.dbo.Rgb_NRGP_Division(" + TxtCompany.Tag.ToString() + "," + MyParent.UserCode + ") Where 1=1";
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Division", Str, String.Empty, 400);
                            if (Dr != null)
                            {
                                TxtDivision.Text = Dr["Division"].ToString();
                                TxtDivision.Tag = Dr["Div_Code"].ToString();
                                TxtRefName.Text = String.Empty;
                                TxtRefName.Tag = String.Empty;
                                TxtReftno.Text = String.Empty;
                              
                                if (TxtDivision.Tag.ToString() == "2")
                                {

                                    TxtSampleName.Enabled = true;
                                    TxtOrderno.Enabled = true;
                                    TxtColor.Enabled = true;
                                }
                                else
                                {
                                    TxtSampleName.Enabled = false;
                                    TxtOrderno.Enabled = false;
                                    TxtColor.Enabled = false;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Select Company..!");
                            TxtCompany.Focus();
                        }

                    }

                    if (this.ActiveControl.Name == "TxtReftno")
                    {
                        if (TxtDivision.Text.ToString() != String.Empty)
                        {
                            Str = "Select A.Name, A.Tno, A.DeptName, A.DesignationName,A.CompName, A.Emplno,A.CompCode From VAAHINI_ERP_GAINUP.dbo.Rgb_NRGP_Division_Employees(" + TxtDivision.Tag + "," + TxtCompany.Tag + "," + MyParent.Emplno + "," + MyParent.UserCode + ",'REFBY') A";
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Employee", Str, String.Empty, 100, 200, 150, 150);
                            if (Dr != null)
                            {
                                TxtReftno.Text = Dr["Tno"].ToString();
                                TxtRefName.Tag = Dr["Emplno"].ToString();
                                TxtRefName.Text = Dr["NAme"].ToString();

                            }
                        }
                        else
                        {
                            MessageBox.Show("Choose Division");
                            TxtDivision.Focus();
                            return;

                        }
                    }

                    if (this.ActiveControl.Name == "TxtCourier")
                    {
                        Str = "Select COURIERNAME,ccode From VAAHINI_ERP_GAINUP.dbo.Sec_Mas_Courier";
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Courier NAme", Str, String.Empty, 400);
                        if (Dr != null)
                        {
                            TxtCourier.Text = Dr["COURIERNAME"].ToString();
                            TxtCourier.Tag = Dr["ccode"].ToString();
                            
                        }

                    }
                    if (this.ActiveControl.Name == "TxtsupplierName")
                    {
                        if (TxtCompany.Text != String.Empty)
                        {
                            Str = "select ledgername,LEDGERCODE From VAAHINI_ERP_GAINUP.dbo.Ledger_Master ((Case When " + TxtCompany.Tag + "=1 Then 1 When " + TxtCompany.Tag + "=3 Then 2 When " + TxtCompany.Tag + "=5 Then 3 End), (Select DateName(year,Dateadd(Month,-3,Getdate()))+'-'+Cast(Datepart(year,Dateadd(Month,-3,Getdate()))+1 As Varchar(10)))) Where ledgername Not like 'ZZZ%' Order by ledgername";
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select LeaderName", Str, String.Empty, 400);
                            if (Dr != null)
                            {
                                TxtsupplierName.Text = Dr["ledgername"].ToString();
                                TxtsupplierName.Tag = Dr["LEDGERCODE"].ToString();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Select Company..!");
                            TxtCompany.Focus();
                        }
                    }
                    if (this.ActiveControl.Name == "TxtSampleName")
                    {
                        if (TxtDivision.Tag.ToString() == "2")
                        {
                            Str = "select Name, Rowid From VAAHINI_ERP_GAINUP.dbo.Sample_Name_MAster";
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select SampleName", Str, String.Empty, 400);
                            if (Dr != null)
                            {
                                TxtSampleName.Text = Dr["Name"].ToString();
                                TxtSampleName.Tag = Dr["Rowid"].ToString();
                            }
                        }
                    }
                    if (this.ActiveControl.Name == "TxtOrderno")
                    {
                        if (TxtDivision.Tag.ToString() == "2")
                        {
                            Str = "Select 'Others' Order_No, '.' Color, 0 Color_Id Union Select ORder_No, Color, ColorID From VAAHINI_ERP_GAINUP.dbo.mis_order_details_Color()";
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select OrderNo", Str, String.Empty, 100,300);
                            if (Dr != null)
                            {
                                TxtOrderno.Text = Dr["Order_No"].ToString();
                                TxtColor.Text = Dr["Color"].ToString();
                                TxtColor.Tag = Dr["Color_Id"].ToString();
                            }
                        }
                    }
                    if (this.ActiveControl.Name == "TxtRgptype")
                    {

                        Str = "Select Type,typecode From (values(0,'GENERAL'),(1,'SAMPLE')) x(typecode,Type)";
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Rgp Type", Str, String.Empty, 100);
                        if (Dr != null)
                        {
                            TxtRgptype.Text = Dr["Type"].ToString();
                            TxtRgptype.Tag = Dr["typecode"].ToString();
                            TxtsupplierName.Text = String.Empty;
                            TxtsupplierName.Tag = String.Empty;
                        }
                    }

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmNrgpEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtDivision" || this.ActiveControl.Name == "TxtCompany" || this.ActiveControl.Name == "TxtRgptype" || this.ActiveControl.Name == "TxtRgpno" || this.ActiveControl.Name == "TxtSampleName" || this.ActiveControl.Name == "TxtCourier" || this.ActiveControl.Name == "TxtOrderno" || this.ActiveControl.Name == "TxtColor" || this.ActiveControl.Name == "TxtEmplname" || this.ActiveControl.Name =="TxtTno")
                {
                    e.Handled = true;
                }
                if (this.ActiveControl.Name == "TxtRemarks"  || this.ActiveControl.Name == "TxtRefno" || this.ActiveControl.Name == "TxtDespatch")
                {
                    MyBase.Return_Ucase(e);
                }
                if (this.ActiveControl.Name == "TxtsupplierName")
                {
                    if (TxtRgptype.Tag.ToString() == "1")
                    {
                        e.Handled = false;
                        MyBase.Return_Ucase(e);
                    }
                    else
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


        void Grid_Data()
        {
            String Str = String.Empty;
            try
            {
                if (BtnCtl_New.Enabled==true)
                {
                    Str = "Select 0 SNO,'' ITEMDESCRIPTION,'' PURPOSE, 0.00 RGPQTY,'' UOM, 0.00 AMOUNT Where 1=2";
                }
                else
                {
                    Str = "Select itemdesc ITEMDESCRIPTION,purpose PURPOSE,rgpQTY RGPQTY,UOM, Rate AMOUNT From VAAHINI_ERP_GAINUP.dbo.NRGP_DCDETAIL Where Rgpno='" + TxtRgpno.Text + "'";
 
                }

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                //MyBase.Grid_Designing(ref Grid, ref Dt, "IND.NO", "UNIT", "ITEMDESCRIPTION", "CENVAT(Y/N)", "DISAMT", "DUTYAMT", "AEDAMT", "STAMT", "SC%", "SCAMT", "CANCELQTY", "PRATE", "PORATE", "DECPL", "IND.PENQTY", "RECQTY", "FRIEGHT", "UFRIEGHT", "DIFFENCE", "UDIFFAMT", "INDDATE", "DUTYAMT2", "UDUTYAMT2", "BILLPDATE", "DUEDATE", "UADDAMT", "ULESSAMT", "ST%", "DUTY%", "T");
                MyBase.ReadOnly_Grid(ref Grid, "SNO");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                Grid.RowHeadersWidth = 10;
                MyBase.Grid_Width(ref Grid, 50, 500, 250, 100, 100, 120);
                for (int i = 0; i <= Dt.Columns.Count - 1; i++)
                {
                    if (Grid.Columns[i].Visible == true)
                    {
                        Grid.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        String GetRgpno()
        {
            DataTable Rgp_Dt = new DataTable();
            String Rgpno = String.Empty;
            Rgpno = "SELECT Rgpno From VAAHINI_ERP_GAINUP.dbo.Max_NrgpNumber()";
            MyBase.Load_Data(Rgpno, ref Rgp_Dt);
            if (Rgp_Dt.Rows.Count > 0)
            {
                Rgpno = 'R'+Rgp_Dt.Rows[0]["Rgpno"].ToString();
            }
            return Rgpno;
        }

        private void FrmNrgpEntry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Disable_Cut_Copy(GBMain);
                MyBase.Clear(this);
                appName = "/" + System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";
                ModuleName = appName.Substring(appName.IndexOf("/") + 1, appName.IndexOf(".") - 1);
                Entry_New();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void LoadEmployee()
        {
            try
            {
                if (TxtDivision.Tag.ToString() == String.Empty)
                {
                    TxtDivision.Tag = "-10";
                }
                if (TxtCompany.Tag.ToString() == String.Empty)
                {
                    TxtCompany.Tag = "-10";
                }
                DataTable Dt2 = new DataTable();
                String Str1 = "Select A.Tno, A.Name,A.CompName, A.DeptName, A.DesignationName,A.Emplno,A.CompCode From VAAHINI_ERP_GAINUP.dbo.Rgb_NRGP_Division_Employees(" + TxtDivision.Tag + "," + TxtCompany.Tag + "," + MyParent.Emplno + "," + MyParent.UserCode + ",'ENTRYBY') A Where Emplno=" + MyParent.Emplno + "";
                MyBase.Load_Data(Str1, ref Dt2);
                if (Dt2.Rows.Count > 0)
                {
                    TxtEmplname.Text = Dt2.Rows[0]["Name"].ToString();
                    TxtEmplname.Tag = Dt2.Rows[0]["Emplno"].ToString();
                    TxtRefName.Text = Dt2.Rows[0]["Name"].ToString();
                    TxtReftno.Text = Dt2.Rows[0]["Tno"].ToString();
                    TxtRefName.Tag = Dt2.Rows[0]["Emplno"].ToString();
                    TxtTno.Text = Dt2.Rows[0]["Tno"].ToString();
                    TxtCompany.Text = Dt2.Rows[0]["CompName"].ToString();
                    TxtCompany.Tag = Dt2.Rows[0]["CompCode"].ToString();
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
                if (Grid.Rows.Count > 0)
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

                Txt = (TextBox)e.Control;
              //  Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);
                //Txt.TextChanged += new EventHandler(Txt1_TextChanged);
                //Txt.GotFocus += new EventHandler(Txt1_GotFocus);
                //Txt.Leave += new EventHandler(Txt1_Leave);
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

                 MyBase.Return_Ucase(e);
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["RGPQTY"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["AMOUNT"].Index)
                 {
                     MyBase.Valid_Decimal((TextBox)Txt, e);
                 } 
                 if (Grid.CurrentCell.ColumnIndex == Grid.Columns["UOM"].Index)
                 {
                     if (!Char.IsLetter(e.KeyChar))
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

        //void Txt_KeyDown(object sender, KeyEventArgs e)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}


        private bool ValidateDate(string date)
        {
            try
            {
                DateTime Test;
                if (DateTime.TryParseExact(date, "dd-MMM-yyyy", null, System.Globalization.DateTimeStyles.None, out Test) == true)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private void RbnCuryes_Click(object sender, EventArgs e)
        {
            try
            {
                if (RbnCuryes.Checked == true)
                {
                    TxtCourier.Enabled = true;
                }
                if (RbnCurno.Checked == true)
                {
                    TxtCourier.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RbnCurno_Click(object sender, EventArgs e)
        {
            try
            {
                if (RbnCurno.Checked == true)
                {
                    TxtCourier.Enabled = false;
                }
                if(RbnCuryes.Checked==true)
                {
                    TxtCourier.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void BtnControls(String ModeName)
        {

            if (MyParent.UserCode == 1)
            {

                BtnCtl_EntryCancel.Visible = true;
                BtnCtl_Delete.Visible = true;
            }
            else
            {
                BtnCtl_EntryCancel.Visible = false;
                BtnCtl_Delete.Visible = false;
            }

            if (ModeName.ToString().ToUpper() == "ENTRY_NEW")
            {

                BtnCtl_New.Enabled = true;
                BtnCtl_Edit.Enabled = false;
                BtnCtl_EntryCancel.Enabled = false;
                BtnCtl_Delete.Enabled = false;
                BtnCtl_Save.Enabled = true;
           
                BtnCtl_View.Enabled = false;
            }

            if (ModeName.ToString().ToUpper() == "ENTRY_EDIT")
            {
                BtnCtl_New.Enabled = false;
                BtnCtl_Edit.Enabled = true;
                BtnCtl_EntryCancel.Enabled = true;
                BtnCtl_Delete.Enabled = true;
                BtnCtl_Save.Enabled = true;
              
                BtnCtl_View.Enabled = false;


            }
            if (ModeName.ToString().ToUpper() == "ENTRY_VIEW")
            {
                BtnCtl_New.Enabled = false;
                BtnCtl_Edit.Enabled = false;
                BtnCtl_EntryCancel.Enabled = false;
                BtnCtl_Delete.Enabled = false;
                BtnCtl_Save.Enabled = false;
               
                BtnCtl_View.Enabled = true;


            }
            if (ModeName.ToString().ToUpper() == "ENTRY_DELETE")
            {
                BtnCtl_New.Enabled = false;
                BtnCtl_Edit.Enabled = false;
                BtnCtl_EntryCancel.Enabled = false;
                BtnCtl_Delete.Enabled = true;
                BtnCtl_Save.Enabled = false;
              
                BtnCtl_View.Enabled = false;


            }
            if (ModeName.ToString().ToUpper() == "ENTRY_CLEAR")
            {
                BtnCtl_New.Enabled = true;
                BtnCtl_Edit.Enabled = true;
                BtnCtl_EntryCancel.Enabled = false;
                BtnCtl_Delete.Enabled = true;
                BtnCtl_Save.Enabled = false;
                 
                BtnCtl_View.Enabled = true;
                BtnCtl_Delete.Enabled = false;


            }

        }
        void ClearControls(GroupBox gbox, String Modename)
        {


            if (Modename.ToString().ToUpper() == "ENTRY_CLEAR")
            {

                foreach (Control ctrl in gbox.Controls)
                {
                    if (ctrl is CheckBox)
                    {

                        CheckBox checkBox = (CheckBox)ctrl;
                        checkBox.Enabled = false;
                        checkBox.Checked = false;
                    }

                }
                foreach (Control ctrl in gbox.Controls)
                {
                    if (ctrl is RadioButton)
                    {

                        RadioButton Rbn = (RadioButton)ctrl;
                        Rbn.Enabled = false;
                        Rbn.Checked = false;
                    }
                    if (ctrl is TextBox)
                    {

                        TextBox Txtbox = (TextBox)ctrl;
                        Txtbox.Enabled = false;
                        Txtbox.Text = String.Empty;
                    }


                }
            }
            if (Modename.ToString().ToUpper() == "ENTRY_NEW" || Modename.ToString().ToUpper() == "ENTRY_EDIT")
            {
                foreach (Control ctrl in gbox.Controls)
                {
                    if (ctrl is CheckBox)
                    {

                        CheckBox checkBox = (CheckBox)ctrl;
                        checkBox.Enabled = true;
                        checkBox.Checked = false;
                    }

                }
                foreach (Control ctrl in gbox.Controls)
                {
                    if (ctrl is RadioButton)
                    {

                        RadioButton Rbn = (RadioButton)ctrl;
                        Rbn.Enabled = true;
                        Rbn.Checked = false;
                    }
                    if (ctrl is TextBox)
                    {

                        TextBox Txtbox = (TextBox)ctrl;
                        Txtbox.Enabled = true;
                        Txtbox.Text = String.Empty;
                    }


                }

            }


        }

        private void Btn_Exit_Click(object sender, EventArgs e)
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

      
        

        private void BtnCtl_New_Click(object sender, EventArgs e)
        {
            try
            {
                Entry_New();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCtl_Save_Click(object sender, EventArgs e)
        {
            try
            {
                Entry_Save();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCtl_Edit_Click(object sender, EventArgs e)
        {
            try
            {

                Entry_Edit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCtl_View_Click(object sender, EventArgs e)
        {
            try
            {
                RbnDosprint.Checked = false;
                Rbnwordprint.Checked = false;
                Entry_View();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      

        private void BtnCtl_Clear_Click(object sender, EventArgs e)
        {
            try
            {

                MyBase.Clear(this);
                ClearControls(GBMain, "Entry_Clear");
                BtnControls("Entry_Clear");
                RptRgp.Visible = false;
                GrpPrint.Visible = false;
                RbnDosprint.Checked = false;
                Rbnwordprint.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCtl_EntryCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Code > 0)
                {
                    DialogResult m = MessageBox.Show("Sure to Cancel...!", "Rgp Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (m == DialogResult.Yes)
                    {
                        MyBase.Run("Update VAAHINI_ERP_GAINUP.dbo.NRGP_DCMASTER Set Entry_Cancel='T',Cancel_Date=Getdate(),Cancel_System=Host_NAme() Where Rowid=" + Code + " And Rgpno='" + TxtRgpno.Text + "' And RgpDate='" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "'");
                        MessageBox.Show("Canceled..!");
                        MyBase.Clear(this);
                        TxtCompany.Focus();
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCtl_Delete_Click(object sender, EventArgs e)
        {
            try
            {

                //Entry_Delete();
                Entry_Delete_Confirm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnPrintDos_Click(object sender, EventArgs e)
        {
            try
            {

               // Str = "SELECT rgpNO, rgpDATE, LedgerName, itemdesc, rgpQTY, uom ,DESP ,SPLINST ,Courier_Mode,Emplno,LEDGERCODE,CompCode,Division,POTYPE,Sample_ID,Order_No,Color_ID,RowID,CourierCode,REFQUOTNO,REFQUOTDATE   FROM  VAAHINI_ERP_GAINUP.dbo.Vaahini_NRgp_Fn()  Where PoType <> 2  and Entry_Cancel = 'F'  And Approval_status = 'F'";

              

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        DataTable GetGridVisibleColToDatatable(DataGridView Grid, params String[] ToHideColumnsName)
        {

            DataTable Exl_Dt = new DataTable();


            foreach (String Sql in ToHideColumnsName)
            {
                Grid.Columns[Sql].Visible = false;
            }

            for (int iC = 0; iC <= Grid.Columns.Count - 1; iC++)
            {
                if (Grid.Columns[iC].Visible)
                {
                    Exl_Dt.Columns.Add(Grid.Columns[iC].Name);
                }

            }


            for (int i = 0; i < Grid.Rows.Count-1; i++)
            {



                if (Grid.Rows[i].Visible)
                {
                    DataRow dtRow = Exl_Dt.NewRow();
                    for (int j = 0; j < Grid.Columns.Count - 1; j++)
                    {

                        if (Grid.Columns[j].Visible == true)
                        {


                            for (int k = 0; k <= Exl_Dt.Columns.Count - 1; k++)
                            {
                                if (Exl_Dt.Columns[k].ColumnName.ToString().ToUpper() == Grid.Columns[j].Name.ToString().ToUpper())
                                {
                                    dtRow[k] = Grid.Rows[i].Cells[j].Value.ToString();
                                }
                            }
                        }


                    }

                    Exl_Dt.Rows.Add(dtRow);
                }
            }

            return Exl_Dt;
        }

        void DosPrint()
        {
            try
            {
                Int16 LineNo = 0;
                StreamWriter Edit = null;
                Edit = new StreamWriter("C:\\Vaahrep\\NRGPDC.txt");
                LineNo = 1;
                 
                Edit.WriteLine(MyBase.Space(1) + "" + MyBase.PadM("" + TxtRgptype.Text.ToString().ToUpper() + " GATE PASS(RETURNABLE)", 81) + "" + MyBase.Space(1));
                Edit.WriteLine(MyBase.Fill_Char(84, '-'));
              
                Edit.WriteLine(MyBase.Space(1) + MyBase.PadM(TxtCompany.Text.ToString(), 82) + "" + MyBase.Space(1));
                Edit.WriteLine(MyBase.Space(2) + MyBase.PadM("OTTUPATTI,  SINGARAKOTTAI,  DINDIGUL.", 81) + "" + MyBase.Space(1));
                 
                Edit.WriteLine(MyBase.Space(84));
                Edit.WriteLine(MyBase.Space(1) + MyBase.PadR("RGPNO   : " + TxtRgpno.Text.ToString() + "", 18) + MyBase.Space(7) + MyBase.PadR("PARTY    : " + TxtsupplierName.Text.ToString() + "", 58));
                Edit.WriteLine(MyBase.Space(1) + MyBase.PadR("RGPDATE : " + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "", 22) + MyBase.Space(3) + MyBase.PadR("DIVISION : " + TxtDivision.Text.ToString() + "", 30) + MyBase.Space(28));
                Edit.WriteLine(MyBase.Space(1) + MyBase.PadR("REFNO   : " + TxtRefno.Text.ToString() + "", 21) + MyBase.PadL("PRINT DT : " + String.Format("{0:dd-MMM-yyyy hh:mm tt}", MyBase.GetServerDateTime()) + "", 35) + MyBase.PadL("", 13) + MyBase.Space(8));
                Edit.WriteLine(MyBase.Space(1) + MyBase.PadR("REFDATE : " + String.Format("{0:dd-MMM-yyyy}", RefDate.Value) + "", 22) + "" + MyBase.Space(61));
                Edit.WriteLine(MyBase.Fill_Char(84, '-'));
                Edit.WriteLine(MyBase.Space(1) + MyBase.PadR("SNO", 5) + '|' + MyBase.PadR("Item Description", 32) + '|' + MyBase.PadR("Purpose", 27) + '|' + MyBase.PadR("UOM", 4) + '|' + MyBase.PadR("QTY", 12));
                Edit.WriteLine(MyBase.Fill_Char(84, '-'));
                LineNo = 13;
                for (int i = 0; i <= Dts.Rows.Count - 1; i++)
                {
                    Edit.WriteLine(MyBase.Space(1) + MyBase.PadR(Dts.Rows[i]["SNO"].ToString(), 5) + '|' + MyBase.PadR(Dts.Rows[i]["ITEMDESCRIPTION"].ToString(), 31) + '|' + MyBase.PadR(Dts.Rows[i]["PURPOSE"].ToString(), 27) + '|' + MyBase.PadR(Dts.Rows[i]["UOM"].ToString(), 4) + '|' + MyBase.PadR(Dts.Rows[i]["RGPQTY"].ToString(), 12));
                    LineNo++;
                }
                if (Dts.Rows.Count > 2)
                {
                    Edit.WriteLine(MyBase.Space(84));
                    LineNo++;
                }
                else
                {
                    Edit.WriteLine(MyBase.Space(84));
                    LineNo++;
                    Edit.WriteLine(MyBase.Space(84));
                    LineNo++;
                }
                Edit.WriteLine(MyBase.Fill_Char(84, '-'));
                LineNo++;
                Edit.WriteLine(MyBase.Space(1) + MyBase.PadR("DESP THRO       : " + TxtDespatch.Text.ToString() + "", 39) + MyBase.PadR("REF BY :" + TxtReftno.Text.ToString() + "/" + TxtRefName.Text.ToString(), 44));
                LineNo++;
                String Cour_Req = String.Empty;
                if (RbnCurno.Checked == true)
                {
                    Cour_Req = RbnCurno.Text;

                }
                if (RbnCuryes.Checked == true)
                {
                    Cour_Req = RbnCuryes.Text;
                }
                Edit.WriteLine(MyBase.Space(1) + MyBase.PadR("COURIER REQUEST : " + Cour_Req + "", 39) + "" + MyBase.PadR("ENTRY BY :" + TxtTno.Text.ToString() + "/" + TxtEmplname.Text.ToString(), 44));
                LineNo++;
                Edit.WriteLine(MyBase.Space(84));
                LineNo++;
                Edit.WriteLine(MyBase.Space(1) + MyBase.PadR("REMARKS   : " + TxtRemarks.Text.ToString(), 84));
                Edit.WriteLine(MyBase.Space(84));
                LineNo++;
                Edit.WriteLine(MyBase.Space(84));
                LineNo++;
                Edit.WriteLine(MyBase.Space(1) + MyBase.PadR("HEAD OF THE DEPT", 25) + MyBase.PadR("IO", 19) + MyBase.PadR("FM", 20) + MyBase.PadR("GM", 20) + MyBase.Space(5));
                while (LineNo < 37)
                {
                    Edit.WriteLine(MyBase.Space(84));
                    LineNo++;
                }
                Edit.WriteLine((Char)12);
                Edit.Close();
                RptRgp.LoadFile("C:\\Vaahrep\\NRGPDC.txt", RichTextBoxStreamType.PlainText);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void BtnPrnExit_Click(object sender, EventArgs e)
        {
            try
            {
                GrpPrint.Visible = false;
                RptRgp.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnPrintClear_Click(object sender, EventArgs e)
        {
            try
            {
                Rbnwordprint.Checked = false;
                RbnDosprint.Checked = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnPrintOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (Rbnwordprint.Checked == true)
                {
                     
                        Entry_Print();
                        //GrpPrint.Visible = false;
                    

                }
                if (RbnDosprint.Checked == true)
                {

                    


                        DataTable Dtp = new DataTable();
                        String Strpnt = "SELECT   Nrgp_dcMASTER.Approval_Status,Nrgp_dcMASTER.Potype,Nrgp_dcMASTER.Approval_Status1     FROM VAAHINI_ERP_GAINUP.dbo.Nrgp_dcMASTER  Where Nrgp_dcMASTER.rgpNO='" + TxtRgpno.Text + "' And Nrgp_dcMASTER.RgpDate='" + String.Format("{0:dd-MMM-yyyy}", RgpDate.Value) + "'";
                        MyBase.Load_Data(Strpnt, ref Dtp);


                        if (Dtp.Rows[0]["Potype"].ToString() == "0")
                        {
                            if (Dtp.Rows[0]["Approval_Status"].ToString().Trim() != "T")
                            {

                                MessageBox.Show("Approval Pending");
                                GrpPrint.Visible = false;
                                return;

                            }
                            else if (Dtp.Rows[0]["Approval_Status1"].ToString().Trim() != "T")
                            {
                                MessageBox.Show("Second Level Approval Pending");
                                GrpPrint.Visible = false;
                                return;
                            }
                            else
                            {

                                Print("C:\\Vaahrep\\NRGPDC.txt");
                            }

                        }
                        else if (Dtp.Rows[0]["Potype"].ToString() == "1")
                        {


                            if (Dtp.Rows[0]["Approval_Status"].ToString().Trim() != "T")
                            {

                                MessageBox.Show("Approval Pending");
                                GrpPrint.Visible = false;
                                return;

                            }
                            else
                            {

                                Print("C:\\Vaahrep\\NRGPDC.txt");
                            }

                        }
                        
 
                       
                        
                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Print(String FilePath)
        {
            String Command;
            try
            {

                Command = "START /MIN NOTEPAD /P " + FilePath + "";
                ProcessStartInfo proc1 = new ProcessStartInfo();
                proc1.UseShellExecute = true;
                proc1.WorkingDirectory = @"C:\Windows\System32";
                proc1.FileName = @"C:\Windows\System32\cmd.exe";
                //proc1.Verb = "runas";
                proc1.Arguments = "/c " + Command;
                proc1.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(proc1);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void RbnDosprint_Click(object sender, EventArgs e)
        {
            if (RbnDosprint.Checked == true)
            {

                    Dts = new DataTable();
                    Dts = GetGridVisibleColToDatatable(Grid);
                    DosPrint();
                    RptRgp.Visible = true;

                   
            }
            else
            {
                RptRgp.Visible = false;
            }
        }

        private void Rbnwordprint_Click(object sender, EventArgs e)
        {
            if (Rbnwordprint.Checked == true)
            {

                RptRgp.Visible = false;
            }
            else
            {
                RptRgp.Visible = true;
            }
        }


    }


}
