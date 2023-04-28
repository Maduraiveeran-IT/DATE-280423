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
using DotnetVFGrid;

namespace Accounts
{
    public partial class FrmVehicleBookingEntry : Form, Entry
    {

        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataRow Dr = null;
        TextBox Txt = null;
        TextBox Txt2 = null;
        String Str,Str1;
        DataTable TmpDt = new DataTable();
        DataTable TmpDt1 = new DataTable();
        String Strt;
        DataTable Dt7 = new DataTable();
        Int64 Code = 0;
        String[] Queries, Queries_New;
        Int32 Array_Index = 0;
        DataTable Dtn = new DataTable();
        DataTable Dtn1 = new DataTable();
        String Strs, strs1, Str2, Strs2, Str6;
        DataTable Dtn2 = new DataTable();
        DataTable Dtl = new DataTable();
        DataTable Dt8 = new DataTable();
        DataTable Dttime = new DataTable();
        Int64 Bal = 0;

        public FrmVehicleBookingEntry()
        {
            InitializeComponent();
        }


        
        public void Entry_New()
        {
            try
            {
              
                Btn_Cancel1.Visible = false;
                Grid_Data();
                Grid_Data2();
                Grid2.Visible = false;
                Grid.Visible = true;
                DtpDate1.Focus();
                Load_User();
                Min_MaxDate();
                TxtTripTime.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Load_Grid2()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Load_User_Level()
        {
            try
            {
                Strs2 = "Select * From VAAHINI_ERP_GAINUP.dbo.Merchandiser_Emplno_Mapping Where Type='E' And Emplno=" + MyParent.Emplno + "";
                MyBase.Load_Data(Strs2, ref Dtl);
                if (Dtl.Rows.Count > 0)
                {
                    TxtCompany.Enabled = true;
                    Str = "Select Order_No Orderno , Ship_Date ShipDate , Name , Buyer , OrderQty  , Bom , BalanceQty , Item ,  UOM ,  PortOfLoading , Destination , BookType , CompCode From VAAHINI_ERP_GAINUP.dbo.Merchandiser_OrderNo_Details() Where CompCode=" + TxtCompany.Tag + "";
                    if (TxtCompany.Tag.ToString().Trim()!=String.Empty && Convert.ToInt64(TxtCompany.Tag) == 1)
                    {
                        Str = Str + " And Emplno=12379";
                    }
                    if (TxtType.Text.ToString().Trim() != String.Empty)
                    {
                        Str = Str + " And BookType='"+TxtType.Text.ToString()+"'";
                    }
                }
                else
                {
                    TxtCompany.Enabled = false;
                    Str = "Select Order_No Orderno , Ship_Date ShipDate ,  Name , Buyer , OrderQty  , Bom , BalanceQty , Item , UOM ,  PortOfLoading , Destination , BookType , CompCode From VAAHINI_ERP_GAINUP.dbo.Merchandiser_OrderNo_Details() Where Emplno=" + MyParent.Emplno + "";
                    if (TxtType.Text.ToString().Trim() != String.Empty)
                    {
                        Str = Str + " And BookType='" + TxtType.Text.ToString() + "'";
                    }
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        Int64 Fill_BOM_Check(String OrderNo, Int64 DespatchQty, String item, String Uom, String type, Int64 CompCode)
        {
            try
            {
                
                Int64 Act_Bal = 0;
                Int64 Act_Entered = 0;
                DataTable Tdt = new DataTable();
                String StrChk = "Select Order_No Orderno ,  BalanceQty , Item ,  UOM  From VAAHINI_ERP_GAINUP.dbo.Merchandiser_OrderNo_Details() Where Order_No='" + OrderNo + "' And Item='" + item + "' And UOM='" + Uom + "' And BookType='" + type + "' And CompCode=" + CompCode + "";
                MyBase.Load_Data(StrChk, ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    //DataTable Dt1 = new DataTable();
                    
                    //Str = "Select Order_No Orderno ,  BalanceQty , Item ,  UOM , CompCode  From VAAHINI_ERP_GAINUP.dbo.Merchandiser_OrderNo_Details() Where Order_No='" + OrderNo + "' And Item='" + item + "' And UOM='" + Uom + "' And BookType='" + type + "' And CompCode=" + CompCode + "";
                    //MyBase.Load_Data(Str, ref Dt1);
                   
                    Act_Bal = Convert.ToInt32(Tdt.Rows[0]["BalanceQty"].ToString());
                    Act_Entered = 0;
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Dt.Rows[i]["Orderno"].ToString() == OrderNo && Dt.Rows[i]["item"].ToString() == item && Dt.Rows[i]["UOM"].ToString() == Uom && Convert.ToInt64(TxtCompany.Tag) == CompCode)
                        {
                            if (Convert.ToInt64(DespatchQty) > 0)
                            {
                                
                                //Act_Entered = Convert.ToInt64(Act_Entered) + Convert.ToInt64(DespatchQty);
                                Act_Entered = Convert.ToInt64(Act_Entered) + Convert.ToInt64(Grid["DESPATCHQTY", i].Value);
                                
                            }
                        }
                    }
                    Bal = Convert.ToInt64(Act_Bal) - Convert.ToInt64(Act_Entered);
                }
                return Bal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Load_User_Rights()
        {
            try
            {
                Str2 = "Select Mode,Emplno,Min_Rights1 From VAAHINI_ERP_GAINUP.dbo.Allow_Date_For_Booking Where Mode='C' And Emplno=" + MyParent.Emplno + "";
                MyBase.Load_Data(Str2,ref Dtn2);
                if (Dtn2.Rows.Count > 0)
                {
                    tabControl1.TabPages.Remove(GOODS);
                    tabControl1.TabPages.Insert(0, GOODS);
                    TxtCompany.Enabled = true;
                }
                else
                {
                    tabControl1.TabPages.Remove(GOODS);
                    tabControl1.TabPages.Insert(0, GOODS);
                    TxtCompany.Enabled = false;
                }

                Load_User_Level();
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
                TxtTotCount.Focus();
                Array_Index = 0;
                Queries_New = new String[150 + 10 * 5];

                if (!TxtType.Text.ToString().Trim().Contains("GENERAL"))
                {
                    if (Dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Invalid Details ...!", "Gainup");
                        Grid.CurrentCell = Grid["ORDERNO", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);

                        MyParent.Save_Error = true;
                        return;
                    }

                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        for (int j = 1; j < Dt.Columns.Count - 1; j++)
                        {
                            if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == String.Empty)
                            {

                                MessageBox.Show("'" + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                Grid.CurrentCell = Grid[j, i];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;

                            }

                        }
                    }
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Grid["DESPATCHQTY", i].Value != DBNull.Value && Convert.ToDouble(Grid["DESPATCHQTY", i].Value) > Convert.ToDouble(Grid["PACKQTY", i].Value))
                        {
                            MessageBox.Show("Despatch Qty is Greater Then Order Qty", "Gainup..!");
                            Grid.CurrentCell = Grid["DESPATCHQTY", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }

                        if (Convert.ToDouble(Grid["DESPATCHQTY", i].Value) == 0)
                        {
                            MessageBox.Show("Invalid Despatch Qty", "Gainup..!");
                            Grid.CurrentCell = Grid["DESPATCHQTY", i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                        else
                        {
                           
                            if (Fill_BOM_Check(Grid["ORDERNO", i].Value.ToString(), Convert.ToInt64(Grid["DESPATCHQTY", i].Value), Grid["ITEM", i].Value.ToString(),  Grid["UOM", i].Value.ToString(),TxtType.Text.ToString(), Convert.ToInt64(TxtCompany.Tag)) < 0)
                            {
                                MessageBox.Show("Despatch Qty Is Excess For OrderQty ( " + Grid["ORDERNO", i].Value.ToString() + ": ExcessQty =>" + Math.Abs(Bal) + " On SNO " + (i + 1) + ")", "Gainup..!");
                                Grid.CurrentCell = Grid["DESPATCHQTY", i];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;

                            }

                        }
                    }

                
                }
                else
                {
                    TxtTotCount.Focus();
                    if (Dt8.Rows.Count == 0)
                    {
                        MessageBox.Show("Invalid Details ...!", "Gainup");
                        Grid2.CurrentCell = Grid2["ITEMNAME", 0];
                        Grid2.Focus();
                        Grid2.BeginEdit(true);

                        MyParent.Save_Error = true;
                        return;
                    }

                    for (int i = 0; i <= Dt8.Rows.Count - 1; i++)
                    {
                        for (int j = 1; j < Dt8.Columns.Count - 1; j++)
                        {
                            if (Grid2[j, i].Value == DBNull.Value || Grid2[j, i].Value.ToString() == String.Empty)
                            {

                                MessageBox.Show("'" + Grid2.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                Grid2.CurrentCell = Grid2[j, i];
                                Grid2.Focus();
                                Grid2.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;

                            }

                        }
                    }
                }
                Total_Count();
                
                if (TxtRemarks.Text.Trim().ToString() == String.Empty)
                {
                    TxtRemarks.Text = "-";
                }
                if (TxtTotCount.Text.Trim().ToString() == String.Empty)
                {
                    TxtTotCount.Text = "0";
                }
                if (TxtTotQty.Text.Trim().ToString() == String.Empty)
                {
                    TxtTotQty.Text = "0";
                }
                if (txtTotweight.Text.Trim().ToString() == String.Empty)
                {
                    txtTotweight.Text = "0";
                }
                if (TxtTotBox.Text.Trim().ToString() == String.Empty)
                {
                    TxtTotBox.Text = "0";
                }
                if (TxtTotCbm.Text.Trim().ToString() == String.Empty)
                {
                    TxtTotCbm.Text = "0";
                }
                
                if(TxtFrom.Text.Trim().ToString()==String.Empty)
                {
                    MessageBox.Show("Select From Address");
                    TxtFrom.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtTo.Text.Trim().ToString() == String.Empty)
                {
                    MessageBox.Show("Select To Address");
                    TxtTo.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (MyParent.Edit == true)
                {

                    if (DtpDate1.Value < DtpDate.Value)
                    {
                        MessageBox.Show("Invalid BookDate To Save");
                        DtpDate1.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }
                }
                
                    if (MyParent._New == true)
                    {
                        DataTable Temp = new DataTable();
                        String TempStr = "Select IsNull(Max(EntryNo),0)+1 EntryNo From VAAHINI_ERP_GAINUP.dbo.VehicleBookingMas Where Emplno=" + MyParent.Emplno + "";
                        MyBase.Load_Data(TempStr, ref Temp);
                        if (Temp.Rows[0][0].ToString() != String.Empty)
                        {
                            TxtEno.Text = Temp.Rows[0][0].ToString();
                        }
                        else
                        {
                            TxtEno.Text = "1";
                        }

                        DataTable Temp1 = new DataTable();
                        String StrTemp = "Select IsNull(Max(BookNo),0)+1 BookNo From VAAHINI_ERP_GAINUP.dbo.VehicleBookingMas";
                        MyBase.Load_Data(StrTemp, ref Temp1);
                        if (Temp1.Rows[0][0].ToString() != String.Empty)
                        {
                            TxtBno.Text = Temp1.Rows[0][0].ToString();
                        }
                        else
                        {
                            TxtBno.Text = "1";
                        }

                        Queries_New[Array_Index++] = "Insert Into VAAHINI_ERP_GAINUP.dbo.VehicleBookingMas(EntryNo , Emplno , BookDate , BookTime , From_Add , To_Add , Total_Order_Count , Total_Qty , Total_Weight , Total_Box , Total_Cbm , Remarks , Type_Code , Comp_Code , BookNo )values(" + TxtEno.Text + " , " + TxtName.Tag + " , '" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "' , " + TxtTripTime.Tag + " , " + TxtFrom.Tag + " , " + TxtTo.Tag + " , " + TxtTotCount.Text + " , " + TxtTotQty.Text + " , " + txtTotweight.Text + " , " + TxtTotBox.Text + " , " + TxtTotCbm.Text + " , '" + TxtRemarks.Text.ToString() + "' , " + TxtType.Tag + " , " + TxtCompany.Tag + " , " + TxtBno.Text + ");Select Scope_Identity()";  
                    }
                    else
                    {
                        Queries_New[Array_Index++] = "Update VAAHINI_ERP_GAINUP.dbo.VehicleBookingMas Set Emplno=" + TxtName.Tag + ",BookDate='" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', BookTime=" + TxtTripTime.Tag + ", From_Add=" + TxtFrom.Tag + ", To_Add=" + TxtTo.Tag + ", Total_Order_Count=" + TxtTotCount.Text + ", Total_Qty=" + TxtTotQty.Text + ", Total_Weight=" + txtTotweight.Text + ", Total_Box=" + TxtTotBox.Text + ", Total_Cbm=" + TxtTotCbm.Text + ", Remarks='" + TxtRemarks.Text.ToString() + "', Type_Code=" + TxtType.Tag + " , Comp_Code=" + TxtCompany.Tag + " Where Rowid=" + Code + "";
                        Queries_New[Array_Index++] = "Delete From VAAHINI_ERP_GAINUP.dbo.VehicleBookingMasDetails Where Masterid=" + Code + "";
                        Queries_New[Array_Index++] = "Delete From VAAHINI_ERP_GAINUP.dbo.Vehicle_Booking_General_Details Where BookMasid=" + Code + ""; 
                    }

                    if (!TxtType.Text.ToString().Trim().Contains("GENERAL"))
                    {
                        for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                        {
                            if (MyParent._New == true)
                            {

                                Queries_New[Array_Index++] = "Insert into VAAHINI_ERP_GAINUP.dbo.VehicleBookingMasDetails(Masterid , OrderNo , OrderQty , ProdQty , Weight , Item , Lenght , Breadth , Height , UOM ,  No_Of_Box , CBM) values(@@IDENTITY,'" + Grid["ORDERNO", i].Value + "'," + Grid["PACKQTY", i].Value + "," + Grid["DESPATCHQTY", i].Value + "," + Grid["WEIGHT", i].Value + ", '" + Grid["ITEM", i].Value + "', " + Grid["LENGTH", i].Value + " , " + Grid["BREADTH", i].Value + " ,  " + Grid["HEIGHT", i].Value + "  , '" + Grid["UOM", i].Value + "' ," + Grid["NO_OF_BOX", i].Value + "," + Grid["CBM", i].Value + ")";
                            }
                            else
                            {
                                Queries_New[Array_Index++] = "Insert into VAAHINI_ERP_GAINUP.dbo.VehicleBookingMasDetails(Masterid , OrderNo , OrderQty , ProdQty , Weight ,  Item , Lenght , Breadth , Height , UOM , No_Of_Box , CBM) values(" + Code + ",'" + Grid["ORDERNO", i].Value + "'," + Grid["PACKQTY", i].Value + "," + Grid["DESPATCHQTY", i].Value + "," + Grid["WEIGHT", i].Value + ", '" + Grid["ITEM", i].Value + "', " + Grid["LENGTH", i].Value + " , " + Grid["BREADTH", i].Value + " ,  " + Grid["HEIGHT", i].Value + "  , '" + Grid["UOM", i].Value + "' ," + Grid["NO_OF_BOX", i].Value + "," + Grid["CBM", i].Value + ")";
                            }
                        }
                    }
                    else
                    {

                        for (int i = 0; i <= Dt8.Rows.Count - 1; i++)
                        {
                            if (MyParent._New == true)
                            {
                                Queries_New[Array_Index++] = "Insert into VAAHINI_ERP_GAINUP.dbo.Vehicle_Booking_General_Details(BookMasid,Name) values(@@IDENTITY,'" + Grid2["ITEMNAME", i].Value + "')";
                            }
                            else
                            {
                                Queries_New[Array_Index++] = "Insert into VAAHINI_ERP_GAINUP.dbo.Vehicle_Booking_General_Details(BookMasid,Name) values(" + Code + ",'" + Grid2["ITEMNAME", i].Value + "')";
                            }
                        }
                    }

                    if (MyParent._New == true)
                    {
                        MyBase.Run_Identity(false, Queries_New);
                    }
                    else
                    {
                        MyBase.Run_Identity(true, Queries_New);
                    }

                    MessageBox.Show("Saved ..!", "Gainup");
                    MyParent.Save_Error = false;
                    MyBase.Clear(this);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MyParent.Save_Error = true;
            }
        }

        public void Entry_Edit()
        {
            try
            {
                Min_MaxDate();

                Str = "Select A.EntryNo , A.BookNo , A.BookDate , CONVERT(varchar(15),CAST(T2.Time+' '+ T2.Mode AS TIME),100) BookTime , Case When D.Name='-' Then 'MILL' Else D.Name End FROMADD , Case When D1.Name='-' Then 'MILL' Else D1.Name End TOADD , A.Total_Order_Count , A.Total_Qty , A.Total_Weight , A.Total_Box , A.Total_Cbm , B.NAME , B.tno TNO , CompName , A.Remarks , ISNull(T1.Name,'-') BType , IsNull(A.Type_Code,0) Type_Code , A.From_Add , A.To_Add , A.Emplno,A.RoWid , A.BookTime TimeId , C.CompCode , Cast(A.EntryDate As Date) EntryDate From VAAHINI_ERP_GAINUP.dbo.VehicleBookingMas A Left join VAAHINI_ERP_GAINUP.dbo.EmployeeMAs B On A.Emplno=B.Emplno Left join VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1 C On B.COMPCODE=C.CompCode Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D on A.From_Add=D.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D1 on A.To_Add=D1.Rowid Left join VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs T1 On A.Type_Code=T1.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteTimeMaster T2 On A.BookTime=T2.Rowid Left join ACCOUNTS.dbo.Export_Invoice_Master I1 on A.RoWid=I1.Vehicle_Book_Id Where A.Cancel_Booking='F' And A.Approve='F' And MD_Approve='F' And I1.Vehicle_Book_Id Is Null";
                Str = Str + " And A.Emplno=" + MyParent.Emplno + "";
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Type", Str, String.Empty, 90, 100, 100, 100,150,150);
                 if (Dr != null)
                 {
                     Fill_Datas();
                     Btn_Cancel1.Visible = true;
                      
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
                Btn_Cancel1.Visible = false;
                Min_MaxDate();

                Str = "Select A.EntryNo , A.BookNo , A.BookDate , CONVERT(varchar(15),CAST(T2.Time+' '+ T2.Mode AS TIME),100) BookTime , Case When D.Name='-' Then 'MILL' Else D.Name End FROMADD , Case When D1.Name='-' Then 'MILL' Else D1.Name End TOADD , A.Total_Order_Count , A.Total_Qty , A.Total_Weight , A.Total_Box , A.Total_Cbm , B.NAME , B.tno TNO , CompName , A.Remarks , ISNull(T1.Name,'-') BType , IsNull(A.Type_Code,0) Type_Code , A.From_Add , A.To_Add , A.Emplno,A.RoWid , A.BookTime TimeId , C.CompCode , Cast(A.EntryDate As Date) EntryDate From VAAHINI_ERP_GAINUP.dbo.VehicleBookingMas A Left join VAAHINI_ERP_GAINUP.dbo.EmployeeMAs B On A.Emplno=B.Emplno Left join VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1 C On B.COMPCODE=C.CompCode Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D on A.From_Add=D.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D1 on A.To_Add=D1.Rowid Left join VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs T1 On A.Type_Code=T1.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteTimeMaster T2 On A.BookTime=T2.Rowid Left join ACCOUNTS.dbo.Export_Invoice_Master I1 on A.RoWid=I1.Vehicle_Book_Id Where A.Cancel_Booking='F' And A.Approve='F' And MD_Approve='F' And I1.Vehicle_Book_Id Is Null";
                Str = Str + " And A.Emplno=" + MyParent.Emplno + "";

               // Str = "Select A.EntryNo , A.BookNo , A.BookDate , CONVERT(varchar(15),CAST(T2.Time+' '+ T2.Mode AS TIME),100) BookTime , Case When D.Name='-' Then 'MILL' Else D.Name End FROMADD , Case When D1.Name='-' Then 'MILL' Else D1.Name End TOADD , A.Total_Order_Count , A.Total_Qty , A.Total_Weight , A.Total_Box , A.Total_Cbm , B.NAME , B.tno TNO , CompName , A.Remarks , ISNull(T1.Name,'-') BType , IsNull(A.Type_Code,0) Type_Code , A.From_Add , A.To_Add , A.Emplno,A.RoWid , A.BookTime TimeId , C.CompCode From VAAHINI_ERP_GAINUP.dbo.VehicleBookingMas A Left join VAAHINI_ERP_GAINUP.dbo.EmployeeMAs B On A.Emplno=B.Emplno Left join VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1 C On B.COMPCODE=C.CompCode Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D on A.From_Add=D.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D1 on A.To_Add=D1.Rowid Left join VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs T1 On A.Type_Code=T1.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteTimeMaster T2 On A.BookTime=T2.Rowid Left join ACCOUNTS.dbo.Export_Invoice_Master I1 on A.RoWid=I1.Vehicle_Book_Id Where A.Cancel_Booking='F' And A.Approve='F' And MD_Approve='F' And I1.Vehicle_Book_Id Is Null And A.Emplno=" + MyParent.Emplno + "";
              
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Type", Str, String.Empty, 90, 100, 100, 130, 130); 
                if (Dr != null)
                {
                    Fill_Datas();
                    Entry_Delete_Confirm();
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
                Queries = new String[50 + 5 * 5];

                DialogResult m = MessageBox.Show("Sure to Delete...!", "Vehicle Booking", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (m == DialogResult.Yes)
                {
                    Queries[Array_Index++] = "Delete from VAAHINI_ERP_GAINUP.dbo.VehicleBookingMasDetails where Masterid=" + Code + "";
                    Queries[Array_Index++] = "Delete from VAAHINI_ERP_GAINUP.dbo.VehicleBookingMas where Rowid=" + Code + "";
                    MyBase.Run(Queries);
                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyBase.Clear(this);
                    MyParent.Load_DeleteEntry();

                }
                if (m == DialogResult.No)
                {
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
                Btn_Cancel1.Visible = false;

                Str = "Select A.EntryNo , A.BookNo ,  A.BookDate , CONVERT(varchar(15) , CAST(T2.Time+' '+ T2.Mode AS TIME),100) BookTime , Case When D.Name='-' Then 'MILL' Else D.Name End FROMADD , Case When D1.Name='-' Then 'MILL' Else D1.Name End TOADD , A.Total_Order_Count , A.Total_Qty , A.Total_Weight , A.Total_Box , A.Total_Cbm , B.NAME , B.tno TNO , CompName,A.Remarks , ISNull(T1.Name,'-') BType , IsNull(A.Type_Code,0) Type_Code , A.From_Add , A.To_Add , A.Emplno , A.RoWid , A.BookTime TimeId , C.CompCode , Cast(A.EntryDate As Date) EntryDate From VAAHINI_ERP_GAINUP.dbo.VehicleBookingMas A Left join VAAHINI_ERP_GAINUP.dbo.EmployeeMAs B On A.Emplno=B.Emplno Left join VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1 C On B.COMPCODE=C.CompCode Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D on A.From_Add=D.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D1 on A.To_Add=D1.Rowid Left join VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs T1 On A.Type_Code=T1.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteTimeMaster T2 On A.BookTime=T2.Rowid Where A.Cancel_Booking='F' And A.Emplno=" + MyParent.Emplno + "";
                
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Type", Str, String.Empty, 90, 100, 100, 130, 130);
                if (Dr != null)
                {
                    Fill_Datas();
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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Data()
        {
            try
            {
                if (MyParent._New == true)
                {
                    Str = "Select 0 SNO ,'' ORDERNO , '' ITEM , 0.00 PACKQTY , ' ' UOM , 0.00 DESPATCHQTY , 0.000 WEIGHT , 0.00 LENGTH , 0.00 BREADTH , 0.00 HEIGHT , 0.00 NO_OF_BOX , 0.00 CBM ,  '-' T   From VAAHINI_ERP_GAINUP.dbo.VehicleBookingMas Where 1=2";
                }
                else
                {
                    Str = "Select Isnull(B.OrderNo,'-') OrderNo , Isnull(B.ITEM,'-') ITEM , IsNull(B.OrderQty,0)  PACKQTY , IsNull(B.UOM,'-') UOM , B.ProdQty DESPATCHQTY , Isnull(B.Weight,0) WEIGHT , IsNull(B.Lenght,0) LENGTH , IsNull(B.BREADTH,0) BREADTH , IsNull(B.HEIGHT,0) HEIGHT , B.No_Of_Box NO_OF_BOX , B.CBM ,  '-' T  From VAAHINI_ERP_GAINUP.dbo.VehicleBookingMas A Left join VAAHINI_ERP_GAINUP.dbo.VehicleBookingMasDetails B on A.RoWid=B.MasterId Where A.RoWid=" + Code + " And B.OrderNo is Not Null";
                }

                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "ITEM", "T");
                MyBase.ReadOnly_Grid(ref Grid, "SNO", "PACKQTY", "UOM");
                Grid.Columns["NO_OF_BOX"].HeaderText = "NO_OF_BOX/BAG";
                Grid.Columns["PACKQTY"].HeaderText = "ORDERQTY";
                //MyBase.ReadOnly_Grid_Without(ref Grid, "CUG_NO", "AMOUNT");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 60, 100, 90, 70, 90, 70,70,70,70,100,100);
                Grid.Columns["NO_OF_BOX"].DefaultCellStyle.Format = "0";
                Grid.Columns["HEIGHT"].DefaultCellStyle.Format = "0.00";
                Grid.Columns["BREADTH"].DefaultCellStyle.Format = "0.00";
                Grid.Columns["LENGTH"].DefaultCellStyle.Format = "0.00";
                Grid.Columns["DESPATCHQTY"].DefaultCellStyle.Format = "0";
                Grid.Columns["PACKQTY"].DefaultCellStyle.Format = "0";
                Grid.Columns["CBM"].DefaultCellStyle.Format = "0";
                Grid.RowHeadersWidth = 10;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Grid_Data2()
        {
            try
            {
                if (MyParent._New == true)
                {
                    Str6 = "Select 0 SNO , '' ITEMNAME From VAAHINI_ERP_GAINUP.dbo.Vehicle_Booking_General_Details Where 1=2";
                }
                else
                {
                    Str6 = "Select B.NAME ITEMNAME From VAAHINI_ERP_GAINUP.dbo.VehicleBookingMas A Inner join VAAHINI_ERP_GAINUP.dbo.Vehicle_Booking_General_Details B On A.Rowid=B.BookMasid Where A.Rowid=" + Code + "";
                }
                
                Grid2.DataSource = MyBase.Load_Data(Str6, ref Dt8);
                //MyBase.Grid_Designing(ref Grid2, ref Dt8,"BookMasid");
                MyBase.ReadOnly_Grid(ref Grid2, "SNO");
                //MyBase.ReadOnly_Grid_Without(ref Grid, "CUG_NO", "AMOUNT");
                MyBase.Grid_Colouring(ref Grid2, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid2, 90, 600);
                //Grid.Columns["AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid2.RowHeadersWidth = 10;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void Fill_Datas()
        {
            try
            {

                Load_User_Rights();
                Code = Convert.ToInt32(Dr["Rowid"].ToString());
                TxtEno.Text = Dr["EntryNo"].ToString();
                TxtTno.Text = Dr["TNo"].ToString();
                TxtName.Text = Dr["NAME"].ToString();
                TxtName.Tag=Dr["Emplno"].ToString();
                TxtCompany.Text = Dr["CompName"].ToString();
                TxtCompany.Tag = Dr["CompCode"].ToString();
                DtpDate1.Value = Convert.ToDateTime(Dr["BookDate"].ToString());
                DtpDate.Value = Convert.ToDateTime(Dr["EntryDate"]);
                TxtTripTime.Text =  Dr["BookTime"].ToString();
                TxtTripTime.Tag = Dr["TimeId"].ToString();
                TxtFrom.Text = Dr["FROMADD"].ToString();
                TxtFrom.Tag = Dr["From_Add"].ToString();
                TxtTo.Text = Dr["TOADD"].ToString();
                TxtTo.Tag = Dr["To_Add"].ToString();
                TxtTotCount.Text = Dr["Total_Order_Count"].ToString();
                TxtTotQty.Text = Dr["Total_Qty"].ToString();
                txtTotweight.Text = Dr["Total_Weight"].ToString();
                TxtTotBox.Text=Dr["Total_Box"].ToString();
                TxtTotCbm.Text = Dr["Total_Cbm"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();
                TxtType.Text = Dr["BType"].ToString();
                TxtType.Tag = Dr["Type_Code"].ToString();
                TxtBno.Text = Dr["BookNo"].ToString();
                if (!TxtType.Text.ToString().Trim().Contains("GENERAL"))
                {
                    Grid2.Visible = false;
                    Grid.Visible = true;
                    Grid_Data();
                }
                else
                {
                    Grid.Visible = false;
                    Grid2.Visible = true;
                    Grid_Data2();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmVehicleBookingEntry_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
                Min_MaxDate();
                Grid.Visible = true;
                Grid2.Visible = false;
                Lbldimension.Text= "BOX DIMENSION SIZE IN CENTIMETER FORMAT";
                Lbldimension.Visible = false;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Min_MaxDate()
        {

            try
            {
                
                Load_User_Rights();
                if (Dtn2.Rows.Count > 0)
                {

                    strs1 = "Select " + Convert.ToInt64(Dtn2.Rows[0]["Min_Rights1"].ToString()) + " Min_Rights,Max_Rights From VAAHINI_ERP_GAINUP.dbo.Allow_Date_For_Booking Where Emplno=0 And Mode='Y'";
                }
                else
                {
                    strs1 = "Select Min_Rights Min_Rights,Max_Rights From VAAHINI_ERP_GAINUP.dbo.Allow_Date_For_Booking Where Emplno=0 And Mode='Y'";

                }
                MyBase.Load_Data(strs1, ref Dtn1);
                if (MyParent._New)
                {
                    if (Dtn1.Rows.Count > 0)
                    {
                        DtpDate1.MinDate = DtpDate1.Value.AddDays(Convert.ToInt16(Dtn1.Rows[0]["Min_Rights"].ToString()));
                        DtpDate1.MaxDate = DtpDate1.Value.AddDays(Convert.ToInt16(Dtn1.Rows[0]["Max_Rights"].ToString()));
                    }

                }
                else
                {
                    DtpDate1.MaxDate = DtpDate1.Value.AddDays(Convert.ToInt16(Dtn1.Rows[0]["Max_Rights"].ToString()));
                }
                    Dttime = new DataTable();
                    Strt = "Select Cast(Getdate() As Date) GDate";
                    MyBase.Load_Data(Strt, ref Dttime);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmVehicleBookingEntry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (  this.ActiveControl.Name == "TxtCRemarks" || this.ActiveControl.Name == "TxtRemarks")
                    {

                    }
                    if (this.ActiveControl.Name == "TxtTo")
                    {
                        TxtType.Focus();
                        return;
                    }
                    if (this.ActiveControl.Name == "TxtFrom")
                    {
                           TxtTo.Focus();
                            return;
                    }//
                    if (this.ActiveControl.Name == "TxtTripTime")
                    {
                            TxtFrom.Focus();
                            return;
                    }
                    if (this.ActiveControl.Name == "TxtType")
                    {
                            if (TxtType.Text.ToString().Trim() != "GENERAL")
                            {
                                Grid.CurrentCell = Grid["ORDERNO", 0];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }
                            else
                            {
                                Grid2.CurrentCell = Grid2["ITEMNAME", 0];
                                Grid2.Focus();
                                Grid2.BeginEdit(true);
                                return;
                            }
                      }
                      else
                      {
                            SendKeys.Send("{TAB}");
                      }

                    
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    if ( this.ActiveControl.Name == "TxtCRemarks" || this.ActiveControl.Name == "TxtRemarks")
                    {
                        MyParent.Load_SaveEntry();
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtFrom")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            Str = "Select Case When Name='-' Then 'MILL' Else Name End Name,Rowid From VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster Where Rowid Not in(46,75,76)";
                            if (TxtTo.Text.ToString().Trim() != String.Empty)
                            {

                                Str = Str + " And Rowid not in(" + TxtTo.Tag + ")";
                            }
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Route", Str, String.Empty, 200);
                            if (Dr != null)
                            {
                                TxtFrom.Text = Dr["Name"].ToString();
                                TxtFrom.Tag = Dr["Rowid"].ToString();
                            }
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtTo")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            Str = "Select Case When Name='-' Then 'MILL' Else Name End Name,Rowid From VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster Where Rowid Not in(46,75,76)";
                            if (TxtFrom.Text.ToString().Trim() != String.Empty)
                            {

                                Str = Str + " And Rowid not in(" + TxtFrom.Tag + ")";
                            }
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Route", Str, String.Empty, 200);
                            if (Dr != null)
                            {
                                TxtTo.Text = Dr["Name"].ToString();
                                TxtTo.Tag = Dr["Rowid"].ToString();
                            }
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtType")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            Str = "Select Name Type,Rowid From VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs Where Type='G'";
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Type", Str, String.Empty, 200);
                            if (Dr != null)
                            {
                                TxtType.Text = Dr["Type"].ToString();
                                TxtType.Tag = Dr["Rowid"].ToString();

                                if (TxtType.Text.ToString().Trim() == "GENERAL")
                                {
                                    Grid.Visible = false;
                                    Grid2.Visible = true;
                                    MyParent._New = true;
                                    Grid_Data2();
                                }
                                else
                                {
                                    Grid2.Visible = false;
                                    MyParent._New = true;
                                    Grid_Data();
                                    Grid.Visible = true;
                                }
                            }
                        }

                    }
                    else if (this.ActiveControl.Name == "TxtCompany")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            Str = "Select CompName,CompCode From VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1";
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Division", Str, String.Empty, 200);
                            if (Dr != null)
                            {
                                TxtCompany.Text = Dr["CompName"].ToString();
                                TxtCompany.Tag = Dr["CompCode"].ToString();

                                MyParent._New = true;
                               
                                Grid_Data();
                                Grid_Data2();
                            }
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtTripTime")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            Min_MaxDate();
                            Str = "Select Time,Rowid,Flag From VAAHINI_ERP_GAINUP.dbo.TripTime_Fn() Where 1=1";
                            if (Dttime.Rows.Count > 0)
                            {
                                if (DtpDate1.Value == Convert.ToDateTime(Dttime.Rows[0]["GDate"]))
                                {
                                    Str = Str + " And Cast(Time As Time) > Cast(Getdate() As Time)";
                                }
                                else
                                {

                                }
                            }
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Time", Str, String.Empty, 200);
                            if (Dr != null)
                            {
                                TxtTripTime.Text = Dr["Time"].ToString();
                                TxtTripTime.Tag = Dr["Rowid"].ToString();
                                TxtTripTime.Focus();
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

        public void Load_User()
        {
            try
            {
                DataTable Dt2 = new DataTable();
                Str = "Select A.Tno, A.Name, B.CompName,A.COMPCODE, A.Emplno from VAAHINI_ERP_GAINUP.dbo.EmployeeMas A left join VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1 B on A.COMPCODE=B.CompCode where A.tno not like '%Z' and A.EmplNO = " + MyParent.Emplno + " ";
                MyBase.Load_Data(Str, ref Dt2);

                if (Dt2.Rows.Count > 0)
                {
                    TxtTno.Text = Dt2.Rows[0]["Tno"].ToString();
                    TxtName.Text = Dt2.Rows[0]["Name"].ToString();
                    TxtName.Tag = Dt2.Rows[0]["Emplno"].ToString();
                    TxtCompany.Text = Dt2.Rows[0]["CompName"].ToString();
                    TxtCompany.Tag = Dt2.Rows[0]["COMPCODE"].ToString();
                    Load_User_Level();
                }
                else
                {
                    TxtTno.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FrmVehicleBookingEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtTno" || this.ActiveControl.Name == "TxtName" || this.ActiveControl.Name == "TxtCompany" || this.ActiveControl.Name == "TxtTotCount" || this.ActiveControl.Name == "TxtTotQty" || this.ActiveControl.Name == "txtTotweight" || this.ActiveControl.Name == "TxtTotBox" || this.ActiveControl.Name == "TxtTotCbm" || this.ActiveControl.Name == "TxtEno" || this.ActiveControl.Name == "TxtVehno" || this.ActiveControl.Name == "TxtType" || this.ActiveControl.Name == "TxtTripTime" || this.ActiveControl.Name == "TxtFrom" || this.ActiveControl.Name == "TxtTo" || this.ActiveControl.Name == "TxtBno")
                {
                    e.Handled = true;
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
                    Txt.KeyUp += new KeyEventHandler(Txt_KeyUp);
                    Txt.TextChanged += new EventHandler(Txt_TextChanged);
                    Txt.GotFocus += new EventHandler(Txt_GotFocus);
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ORDERNO"].Index)
                {
                    if (Grid.CurrentCell.RowIndex > 0)
                    {
                        //if (Grid["ORDERNO", Grid.CurrentCell.RowIndex].Value == DBNull.Value)
                        //{
                        //    Txt.Text = Grid["ORDERNO", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //    Grid["PACKQTY", Grid.CurrentCell.RowIndex].Value = Grid["PACKQTY", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //    Grid["ITEM", Grid.CurrentCell.RowIndex].Value = Grid["ITEM", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //    Grid["UOM", Grid.CurrentCell.RowIndex].Value = Grid["UOM", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //    Grid["WEIGHT", Grid.CurrentCell.RowIndex].Value = Grid["WEIGHT", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //    Grid["LENGTH", Grid.CurrentCell.RowIndex].Value = Grid["LENGTH", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //    Grid["BREADTH", Grid.CurrentCell.RowIndex].Value = Grid["BREADTH", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //    Grid["HEIGHT", Grid.CurrentCell.RowIndex].Value = Grid["HEIGHT", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //    Grid["CBM", Grid.CurrentCell.RowIndex].Value = Grid["CBM", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                        //}
                    }
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

        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {

                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ORDERNO"].Index)
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            Grid["T", Grid.CurrentCell.RowIndex].Value = String.Empty;
                            Load_User_Level();
                            //Dr = Tool.Selection_Tool_Except_New("ORDERNO", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select OrderNo", Str, String.Empty, 100, 80, 100, 100,75, 60, 80, 80, 70, 100, 100, 100);
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select OrderNo", Str, String.Empty, 100, 80, 100, 100, 75, 60, 80, 80, 70, 100, 100, 100);
                           
                            if (Dr != null)
                            {
                                Txt.Text = Dr["ORDERNO"].ToString();
                                Grid["ORDERNO", Grid.CurrentCell.RowIndex].Value = Dr["ORDERNO"].ToString();
                                Grid["PACKQTY", Grid.CurrentCell.RowIndex].Value = Dr["BalanceQty"].ToString();
                                Grid["ITEM", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                                Grid["UOM", Grid.CurrentCell.RowIndex].Value = Dr["UOM"].ToString();
                                
                                if (Grid.CurrentCell.RowIndex > 0)
                                {
                                    if (Grid["ORDERNO", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["ORDERNO", Grid.CurrentCell.RowIndex].Value.ToString() != Grid["ORDERNO", Grid.CurrentCell.RowIndex - 1].Value.ToString())
                                    {
                                        // Txt.Text = Grid["ORDERNO", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                                        // Grid["PACKQTY", Grid.CurrentCell.RowIndex].Value = Grid["PACKQTY", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                                        // Grid["ITEM", Grid.CurrentCell.RowIndex].Value = Grid["ITEM", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                                        // Grid["UOM", Grid.CurrentCell.RowIndex].Value = Grid["UOM", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                                        Grid["WEIGHT", Grid.CurrentCell.RowIndex].Value = Grid["WEIGHT", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                                        Grid["LENGTH", Grid.CurrentCell.RowIndex].Value = Grid["LENGTH", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                                        Grid["BREADTH", Grid.CurrentCell.RowIndex].Value = Grid["BREADTH", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                                        Grid["HEIGHT", Grid.CurrentCell.RowIndex].Value = Grid["HEIGHT", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                                        Grid["T", Grid.CurrentCell.RowIndex].Value = "D";
                                        //Grid["CBM", Grid.CurrentCell.RowIndex].Value = Grid["CBM", Grid.CurrentCell.RowIndex - 1].Value.ToString();
                                    }
                                    //else
                                    //{
                                    //    Grid["PACKQTY", Grid.CurrentCell.RowIndex].Value = Convert.ToInt64(Grid["PACKQTY", Grid.CurrentCell.RowIndex].Value) - Convert.ToInt64(Grid["DESPATCHQTY", Grid.CurrentCell.RowIndex-1].Value);
                                    //}
                                }

                            }
                        }
                    }

                }

                //if (e.KeyCode == Keys.Enter)
                //{
                //    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DESPATCHQTY"].Index)
                //    {
                //        if (TxtType.Text.ToString().Trim().ToUpper() == "EXPORT" || TxtType.Text.ToString().Trim().ToUpper() == "DOMESTIC")
                //        {
                //            if (Grid["DESPATCHQTY", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Convert.ToDouble(Grid["DESPATCHQTY", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["PACKQTY", Grid.CurrentCell.RowIndex].Value))
                //            {
                //                Grid["DESPATCHQTY", Grid.CurrentCell.RowIndex].Value = Grid["PACKQTY", Grid.CurrentCell.RowIndex].Value;
                //                MessageBox.Show("Despatch Qty is Greater Then Order Qty", "Gainup..!");
                //                Grid.CurrentCell = Grid["DESPATCHQTY", Grid.CurrentCell.RowIndex];
                //                Grid.Focus();
                //                Grid.BeginEdit(true);
                //                return;
                //            }
                //        }
                //    }
                //}
                
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ORDERNO"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["PACKQTY"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["CBM"].Index)
                {
                    e.Handled = true;
                }
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["PACKQTY"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["WEIGHT"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["LENGTH"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["BREADTH"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["HEIGHT"].Index )
                {
                    MyBase.Valid_Decimal(Txt,e);
                }
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DESPATCHQTY"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["NO_OF_BOX"].Index)
                {
                    MyBase.Valid_Number(Txt,e);
                }
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["LENGTH"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["BREADTH"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["HEIGHT"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["HEIGHT"].Index)
                {
                    Grid["T", Grid.CurrentCell.RowIndex].Value = String.Empty;
                    MyBase.Valid_Number(Txt, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void Txt_KeyUp(object sender, KeyEventArgs e)
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
                if (TxtType.Text.ToString().Trim().ToUpper() != "EXPORT")
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["LENGTH"].Index)
                    {
                        if (Grid["LENGTH", Grid.CurrentCell.RowIndex].Value.ToString().Trim() == String.Empty)
                        {
                            Grid["LENGTH", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                    }
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BREADTH"].Index)
                    {
                        if (Grid["BREADTH", Grid.CurrentCell.RowIndex].Value.ToString().Trim() == String.Empty)
                        {
                            Grid["BREADTH", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                    }
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["HEIGHT"].Index)
                    {
                        if (Grid["HEIGHT", Grid.CurrentCell.RowIndex].Value.ToString().Trim() == String.Empty)
                        {
                            Grid["HEIGHT", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                    }
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["CBM"].Index)
                    {
                        if (Grid["CBM", Grid.CurrentCell.RowIndex].Value.ToString().Trim() == String.Empty)
                        {
                            Grid["CBM", Grid.CurrentCell.RowIndex].Value = "0";
                        }
                    }
                }
                if (TxtType.Text.ToString().Trim().ToUpper() == "GENERAL")
                {

                }
                //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DESPATCHQTY"].Index)
                //{
                //    if (TxtType.Text.ToString().Trim().ToUpper() == "EXPORT" || TxtType.Text.ToString().Trim().ToUpper() == "DOMESTIC")
                //    {
                //        if (Grid["DESPATCHQTY", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Convert.ToDouble(Grid["DESPATCHQTY", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["PACKQTY", Grid.CurrentCell.RowIndex].Value))
                //        {
                //            Grid["DESPATCHQTY", Grid.CurrentCell.RowIndex].Value ="0";
                //            MessageBox.Show("Despatch Qty is Greater Then Order Qty", "Gainup..!");
                //            Grid.CurrentCell = Grid["DESPATCHQTY", Grid.CurrentCell.RowIndex];
                //            Grid.Focus();
                //            Grid.BeginEdit(true);
                //            return;
                //        }
                //    }
                //}

                //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["NO_OF_BOX"].Index)
                //{
                //    if (Grid["NO_OF_BOX", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                //    {

                //        Grid["CBM", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["LENGTH", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["BREADTH", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["HEIGHT", Grid.CurrentCell.RowIndex].Value) * Convert.ToDouble(Grid["NO_OF_BOX", Grid.CurrentCell.RowIndex].Value);
                //        Total_Count();
                //    }                   
                //}
                if (Grid["T", Grid.CurrentCell.RowIndex].Value.ToString() != "D" && Grid["T", Grid.CurrentCell.RowIndex].Value.ToString().Trim() != "-" && Grid["T", Grid.CurrentCell.RowIndex].Value.ToString().Trim() == String.Empty)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["HEIGHT"].Index && Grid["HEIGHT", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["HEIGHT", Grid.CurrentCell.RowIndex].Value.ToString().Trim() != String.Empty)
                    {
                        Grid["HEIGHT", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["HEIGHT", Grid.CurrentCell.RowIndex].Value.ToString()) / 100;
                    }
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["LENGTH"].Index && Grid["LENGTH", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["LENGTH", Grid.CurrentCell.RowIndex].Value.ToString().Trim() != String.Empty)
                    {
                        Grid["LENGTH", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["LENGTH", Grid.CurrentCell.RowIndex].Value.ToString()) / 100;
                    }
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["BREADTH"].Index && Grid["BREADTH", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["BREADTH", Grid.CurrentCell.RowIndex].Value.ToString().Trim() != String.Empty)
                    {
                        Grid["BREADTH", Grid.CurrentCell.RowIndex].Value = Convert.ToDouble(Grid["BREADTH", Grid.CurrentCell.RowIndex].Value.ToString()) / 100;
                    }
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["NO_OF_BOX"].Index && Grid["NO_OF_BOX", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["NO_OF_BOX", Grid.CurrentCell.RowIndex].Value.ToString().Trim() != String.Empty)
                    {
                        Grid["CBM", Grid.CurrentCell.RowIndex].Value = Convert.ToInt64((Convert.ToDouble(Grid["BREADTH", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(Grid["LENGTH", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(Grid["HEIGHT", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(Grid["NO_OF_BOX", Grid.CurrentCell.RowIndex].Value.ToString())) * 100);
                        Total_Count();
                    }
                }
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["NO_OF_BOX"].Index && Grid["NO_OF_BOX", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["NO_OF_BOX", Grid.CurrentCell.RowIndex].Value.ToString().Trim() != String.Empty)
                {
                    Grid["CBM", Grid.CurrentCell.RowIndex].Value = Convert.ToInt64((Convert.ToDouble(Grid["BREADTH", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(Grid["LENGTH", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(Grid["HEIGHT", Grid.CurrentCell.RowIndex].Value.ToString()) * Convert.ToDouble(Grid["NO_OF_BOX", Grid.CurrentCell.RowIndex].Value.ToString())) * 100);
                    Total_Count();
                }
                
               // Total_Count();
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
                //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["NO_OF_BOX"].Index)
                //{

                //    Total_Count();

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
                 
                TxtTotCount.Text = Dt.Rows.Count.ToString();
                TxtTotQty.Text = Sum(ref Grid, "DESPATCHQTY");
                txtTotweight.Text = MyBase.Sum(ref Grid, "WEIGHT");
                TxtTotBox.Text = Sum(ref Grid, "NO_OF_BOX");
                //TxtTotCbm.Text = Convert.ToInt64(MyBase.Sum(ref Grid, "CBM")).ToString();
                TxtTotCbm.Text = Sum(ref Grid, "CBM", "NO_OF_BOX", "HEIGHT", "LENGTH", "BREADTH");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Sum(ref MyDataGridView DGV, String ColumnName, params String[] Condition_NotNullColumns)
        {
            Int64 SumValue = 0;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (Check_EmptyinDataGridView(ref DGV, i, Condition_NotNullColumns) != true)
                    {
                        if (Convert.ToString(DGV[ColumnName, i].Value).Trim() != String.Empty)
                        {
                            SumValue = SumValue + Convert.ToInt64(DGV[ColumnName, i].Value);
                        }
                    }
                    
                }
                return String.Format("{0:0}", SumValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Check_EmptyinDataGridView(ref MyDataGridView DGV, int RowIndex, params String[] ColumnNames)
        {
            Boolean Flag = false;
            try
            {
                foreach (String Sql in ColumnNames)
                {
                    if (Convert.ToString(DGV[Sql, RowIndex].Value).Trim() == String.Empty)
                    {
                        Flag = true;
                        break;
                    }
                }
                return Flag;
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

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                //if (MyParent.UserCode == 1)
                //{
                //    if (e.TabPage == tabPage1)
                //        e.Cancel = true;
                //}
                //else
                //{
                //    if (e.TabPage == tabPage1)
                //        e.Cancel = true;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Cancel1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult m = MessageBox.Show("Sure to Cancel Booking...!", "Vehicle Booking", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (m == DialogResult.Yes)
                {
                    DataTable Dtc = new DataTable();
                    String Str5 = "Update VAAHINI_ERP_GAINUP.dbo.VehicleBookingMas Set Cancel_Booking='T' Where Emplno=" + MyParent.Emplno + " And Rowid=" + Code + "";
                    MyBase.Load_Data(Str5, ref Dtc);
                    MessageBox.Show("Canceled..!", "Gainup");
                    MyParent.Save_Error = false;
                    MyBase.Clear(this);

                }
                if (m == DialogResult.No)
                {
                   
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      

        private void Btn_Cancel2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult m = MessageBox.Show("Sure to Cancel Booking...!", "Vehicle Booking", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (m == DialogResult.Yes)
                {
                    DataTable Dtc = new DataTable();
                    String Str5 = "Update VAAHINI_ERP_GAINUP.dbo.VehicleBookingMas Set Cancel_Booking='T' Where Emplno=" + MyParent.Emplno + " And Rowid=" + Code + "";
                    MyBase.Load_Data(Str5, ref Dtc);
                    MessageBox.Show("Canceled..!", "Gainup");
                    MyParent.Save_Error = false;
                    MyBase.Clear(this);

                }
                if (m == DialogResult.No)
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (Grid2.Rows.Count > 1)
                {
                    MyBase.Row_Number(ref Grid2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt2 == null)
                {
                    Txt2 = (TextBox)e.Control;
                    //Txt2.KeyDown += new KeyEventHandler(Txt2_KeyDown);
                    Txt2.KeyPress += new KeyPressEventHandler(Txt2_KeyPress);
                    Txt2.Leave += new EventHandler(Txt2_Leave);
                    // Txt2.TextChanged += new EventHandler(Txt2_TextChanged);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt2_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid2.CurrentCell.ColumnIndex == Grid2.Columns["ITEMNAME"].Index)
                {
                    MyBase.Return_Ucase(e);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void Txt2_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Grid2.CurrentCell.ColumnIndex == Grid2.Columns["ITEMNAME"].Index)
                {
                    Total_Count();
                }

                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Grid2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Grid2.CurrentCell.RowIndex <= Dt8.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Dt8.Rows.RemoveAt(Grid2.CurrentCell.RowIndex);
                    }
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
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DESPATCHQTY"].Index)
                {
                    if (TxtType.Text.ToString().Trim().ToUpper() == "EXPORT" || TxtType.Text.ToString().Trim().ToUpper() == "DOMESTIC")
                    {
                        if (Grid["DESPATCHQTY", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Convert.ToDouble(Grid["DESPATCHQTY", Grid.CurrentCell.RowIndex].Value) > Convert.ToDouble(Grid["PACKQTY", Grid.CurrentCell.RowIndex].Value))
                        {

                            MessageBox.Show("Despatch Qty is Greater Then Order Qty", "Gainup..!");
                            e.Handled = true;
                            Grid.CurrentCell = Grid["DESPATCHQTY", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void Grid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        
    }
}
