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

namespace Accounts
{
    public partial class FrmVehicleCarBooking : Form,Entry
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        MDIMain MyParent;
        DataTable Dt = new DataTable();

        DataTable Dt1 = new DataTable();
        DataRow Dr = null;
       
        TextBox Txt1 = null;
        TextBox Txt2 = null;
        String Str, Str1,Str6;
        String Strt;
        DataTable TmpDt = new DataTable();
        DataTable TmpDt1 = new DataTable();
        DataTable Dt7 = new DataTable();
        DataTable Dt8 = new DataTable();
        Int64 Code = 0;
        String[] Queries, Queries_New;
         
        Int32 Array_Index = 0;
        
        DataTable Dtn = new DataTable();
        DataTable Dtn1 = new DataTable();
        String Strs, strs1, Str2,Strdt;
        DataTable Dtn2 = new DataTable();
        DataTable Dttime = new DataTable();

        public FrmVehicleCarBooking()
        {
            InitializeComponent();
        }

        public void Entry_New()
        {
            try
            {
                
                Btn_Cancel2.Visible = false;
                TxtNoofPersons.Enabled = false;
                
                Grid_Data1();
                Grid_Data2();
                Grid2.Visible = false;
                Grid1.Visible = true;
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

        private void Grid_Data1()
        {
            try
            {
                if (MyParent._New == true)
                {
                    Str1 = "Select 0 SNO,'' TNO,'' NAME ,'' DESIGNATION,0 ENO  From VAAHINI_ERP_GAINUP.dbo.BookingMas Where 1=2";
                }
                else
                {
                    Str1 = "Select IsNull(C.TNO,'-')TNO,IsNull(C.NAME,'-')NAME, IsNull(D.DesignationName,'-') DESIGNATION, Isnull(B.Emplno,0) ENO From VAAHINI_ERP_GAINUP.dbo.BookingMas A Left join VAAHINI_ERP_GAINUP.dbo.BookingMasDetails B Left join VAAHINI_ERP_GAINUP.dbo.EmployeeMAs C On B.Emplno=C.Emplno Left join VAAHINI_ERP_GAINUP.dbo.Designationtype D On C.designationcode=D.DesignationCode on A.RoWid=B.MasterId Where B.Emplno > 0 And A.Rowid=" + Code + "";
                }
                Grid1.DataSource = MyBase.Load_Data(Str1, ref Dt1);
                MyBase.Grid_Designing(ref Grid1, ref Dt1,"ENO");
                MyBase.ReadOnly_Grid(ref Grid1, "SNO", "NAME", "DESIGNATION");
                //MyBase.ReadOnly_Grid_Without(ref Grid, "CUG_NO", "AMOUNT");
                MyBase.Grid_Colouring(ref Grid1, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid1, 90, 100, 300, 200);
                //Grid.Columns["AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid1.RowHeadersWidth = 10;
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
                    Str6 = "Select 0 SNO,'' NAME  From VAAHINI_ERP_GAINUP.dbo.BuyerVisitorsName Where 1=2";
                }
                else
                {
                    Str6 = "Select B.BuyerName NAME From VAAHINI_ERP_GAINUP.dbo.BookingMas A Inner join VAAHINI_ERP_GAINUP.dbo.BuyerVisitorsName B On A.Rowid=B.BookMasid Where A.Rowid=" + Code + "";
                }
                Grid2.DataSource = MyBase.Load_Data(Str6, ref Dt8);
               // MyBase.Grid_Designing(ref Grid1, ref Dt1, "ENO");
                MyBase.ReadOnly_Grid(ref Grid2, "SNO");
                //MyBase.ReadOnly_Grid_Without(ref Grid, "CUG_NO", "AMOUNT");
                MyBase.Grid_Colouring(ref Grid2, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid2, 90,   500 );
                //Grid.Columns["AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid2.RowHeadersWidth = 10;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void Load_User_Rights()
        {
            try
            {
                Str2 = "Select Mode,Emplno,Min_Rights1 From VAAHINI_ERP_GAINUP.dbo.Allow_Date_For_Booking Where Mode='C' And Emplno=" + MyParent.Emplno + "";
                MyBase.Load_Data(Str2, ref Dtn2);
                if (Dtn2.Rows.Count > 0)
                {
                    tabControl1.TabPages.Remove(CAR);
                    tabControl1.TabPages.Insert(0, CAR);
                    TxtDivision.Enabled = true;
                }
                else
                {
                    TxtDivision.Enabled = false;
                }

                //if (MyParent.Emplno == 315 || MyParent.Emplno == 8736 || MyParent.Emplno)
               // {
                   // TxtDivision.Enabled = true;
                //}
                //else
                //{
                  //  TxtDivision.Enabled = false;
                //}
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
                    TxtDivision.Text = Dt2.Rows[0]["CompName"].ToString();
                    TxtDivision.Tag = Dt2.Rows[0]["COMPCODE"].ToString();
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
        
        public void Entry_Save()
        {
            try
            {
                TxtCRemarks.Focus();
                Array_Index = 0;
                Queries_New = new String[10 + 10 * 5];

                if (TxtType.Text.ToString().Trim().Contains("BUYER"))
                {
                    if (TxtBuyerName.Text.Trim().ToString() == String.Empty)
                    {
                        MessageBox.Show("Select BuyerName","Gainup..!");
                        TxtBuyerName.Focus();
                        MyParent.Save_Error = true;
                        return;
                    }

                    if (Dt8.Rows.Count == 0)
                    {
                        MessageBox.Show("Invalid Details ...!", "Gainup");
                        Grid2.CurrentCell = Grid2["NAME", 0];
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
                else
                {
                    if (Dt1.Rows.Count == 0)
                    {
                        MessageBox.Show("Invalid Details ...!", "Gainup");
                        Grid1.CurrentCell = Grid1["TNO", 0];
                        Grid1.Focus();
                        Grid1.BeginEdit(true);
                        MyParent.Save_Error = true;
                        return;
                    }

                    for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                    {
                        for (int j = 1; j < Dt1.Columns.Count - 1; j++)
                        {
                            if (Grid1[j, i].Value == DBNull.Value || Grid1[j, i].Value.ToString() == String.Empty)
                            {

                                MessageBox.Show("'" + Grid1.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                Grid1.CurrentCell = Grid1[j, i];
                                Grid1.Focus();
                                Grid1.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;

                            }

                        }
                    }
                }
                Total_Count();


                if (TxtNoofPersons.Text.Trim().ToString() == String.Empty)
                {
                    TxtNoofPersons.Text = "0";
                }
                if (TxtCRemarks.Text.Trim().ToString() == String.Empty)
                {
                    TxtCRemarks.Text = "-";
                }
                if (TxtBuyerName.Text.Trim().ToString() == String.Empty)
                {
                    TxtBuyerName.Tag ="0";
                }
                if (TxtDivision.Text.Trim().ToString() == String.Empty || TxtDivision.Tag.ToString() == String.Empty)
                {
                    MessageBox.Show("Invalid Division");
                    TxtDivision.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtFrom.Text.Trim().ToString() == String.Empty)
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
                if (TxtTripTime.Text.Trim().ToString() == String.Empty)
                {
                    MessageBox.Show("Select Time");
                    TxtTripTime.Focus();
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
                    String TempStr = "Select IsNull(Max(EntryNo),0)+1 EntryNo From VAAHINI_ERP_GAINUP.dbo.BookingMas Where Emplno=" + MyParent.Emplno + "";
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
                    String StrTemp1 = "Select IsNull(Max(BookNo),0)+1 BookNo From VAAHINI_ERP_GAINUP.dbo.BookingMas";
                    MyBase.Load_Data(StrTemp1, ref Temp1);
                    if (Temp1.Rows[0][0].ToString() != String.Empty)
                    {
                        TxtBNo.Text = Temp1.Rows[0][0].ToString();
                    }
                    else
                    {
                        TxtBNo.Text = "1";
                    }

                    Queries_New[Array_Index++] = "Insert Into VAAHINI_ERP_GAINUP.dbo.BookingMas(EntryNo,Emplno,BookDate,BookTime,From_Add,To_Add,No_Of_Persons,Remarks1,Type_Code,Vehtype,Buyerid,Division,BookNo)values(" + TxtEno.Text + "," + TxtName.Tag + ",'" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "'," + TxtTripTime.Tag + "," + TxtFrom.Tag + "," + TxtTo.Tag + "," + TxtNoofPersons.Text + ",'" + TxtCRemarks.Text.ToString() + "'," + TxtType.Tag + ",'CAR'," + TxtBuyerName.Tag + "," + TxtDivision.Tag + "," + TxtBNo.Text + ");Select Scope_Identity()";
                   
                }
                else
                {

                    Queries_New[Array_Index++] = "Update VAAHINI_ERP_GAINUP.dbo.BookingMas Set Emplno=" + TxtName.Tag + ", BookDate='" + String.Format("{0:dd-MMM-yyyy}", DtpDate1.Value) + "', BookTime=" + TxtTripTime.Tag + ", From_Add=" + TxtFrom.Tag + ", To_Add=" + TxtTo.Tag + ", No_Of_Persons=" + TxtNoofPersons.Text + ", Remarks1='" + TxtCRemarks.Text.ToString() + "', Type_Code=" + TxtType.Tag + ", Vehtype='CAR', Buyerid=" + TxtBuyerName.Tag + ", Division="+TxtDivision.Tag+" Where Rowid=" + Code + "";
                    Queries_New[Array_Index++] = "Delete From VAAHINI_ERP_GAINUP.dbo.BookingMasDetails Where Masterid=" + Code + "";
                    Queries_New[Array_Index++] = "Delete From VAAHINI_ERP_GAINUP.dbo.BuyerVisitorsName Where BookMasid=" + Code + "";  
                }

                if (!TxtType.Text.ToString().Trim().Contains("BUYER"))
                {
                    for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                    {
                        if (MyParent._New == true)
                        {
                            Queries_New[Array_Index++] = "Insert into VAAHINI_ERP_GAINUP.dbo.BookingMasDetails(Masterid,Emplno) values(@@IDENTITY," + Grid1["ENO", i].Value + ")";
                        }
                        else
                        {
                            Queries_New[Array_Index++] = "Insert into VAAHINI_ERP_GAINUP.dbo.BookingMasDetails(Masterid,Emplno) values(" + Code + "," + Grid1["ENO", i].Value + ")";
                        }
                    }

                }
                else
                {

                    for (int i = 0; i <= Dt8.Rows.Count - 1; i++)
                    {
                        if (MyParent._New == true)
                        {
                            Queries_New[Array_Index++] = "Insert into VAAHINI_ERP_GAINUP.dbo.BuyerVisitorsName(BookMasid,BuyerName) values(@@IDENTITY,'" + Grid2["NAME", i].Value + "')";
                        }
                        else
                        {
                            Queries_New[Array_Index++] = "Insert into VAAHINI_ERP_GAINUP.dbo.BuyerVisitorsName(BookMasid,BuyerName) values(" + Code + ",'" + Grid2["NAME", i].Value + "')";
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

        public void GetEntryDate()
        {
            try
            {
                DataTable Dtp = new DataTable();
                Strdt = "";
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

                Str = " Select A.EntryNo , A.BookNo , A.BookDate , CONVERT(varchar(15),CAST(T2.Time+' '+ T2.Mode AS TIME),100) BookTime , Case When D.Name='-' Then 'MILL' Else D.Name End FROMADD , Case When D1.Name='-' Then 'MILL' Else D1.Name End TOADD , B.NAME , B.tno TNO , C.CompName , A.No_Of_Persons , A.Remarks1 , ISNull(T1.Name,'-') BType , ISNull(T3.Name,'-') Buyer , C1.CompName Division , IsNull(A.Type_Code,0) Type_Code , A.From_Add , A.To_Add , A.Emplno , Isnull(A.Buyerid,0)Buyerid , A.BookTime TimeID , A.RoWid , Isnull(A.Division,0) DivisionCode , Cast(A.EntryDate As Date) EntryDate  From VAAHINI_ERP_GAINUP.dbo.BookingMas A Left join VAAHINI_ERP_GAINUP.dbo.EmployeeMAs B On A.Emplno=B.Emplno Left join VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1 C On B.COMPCODE=C.CompCode Left Join VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1 C1 On A.Division=C1.CompCode Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D on A.From_Add=D.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D1 on A.To_Add=D1.Rowid Left join VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs T1 On A.Type_Code=T1.Rowid Left Join VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs T3 On A.Buyerid=T3.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteTimeMaster T2 On A.BookTime=T2.Rowid Left join VAAHINI_ERP_GAINUP.dbo.Vehicle_Travell_Expense_Master T6 On A.RoWid=T6.BookMasid Where T6.BookMasid Is Null And A.Cancel_Booking='F' And A.Approve='F' And MD_Approve='F'";
                Str = Str + " And A.Emplno=" + MyParent.Emplno + "";
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Type", Str, String.Empty, 90, 90, 100, 100, 100, 150, 150 , 100);
                if (Dr != null)
                {
                    Fill_Datas();
                    
                    Btn_Cancel2.Visible = true;
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
                 
                Btn_Cancel2.Visible = false;
                Min_MaxDate();

                Str = " Select A.EntryNo , A.BookNo , A.BookDate , CONVERT(varchar(15),CAST(T2.Time+' '+ T2.Mode AS TIME),100) BookTime , Case When D.Name='-' Then 'MILL' Else D.Name End FROMADD , Case When D1.Name='-' Then 'MILL' Else D1.Name End TOADD ,  B.NAME , B.tno TNO , C.CompName , A.No_Of_Persons , A.Remarks1 , ISNull(T1.Name,'-') BType , ISNull(T3.Name,'-') Buyer , C1.CompName Division , IsNull(A.Type_Code,0) Type_Code , A.From_Add , A.To_Add , A.Emplno , Isnull(A.Buyerid,0)Buyerid , A.BookTime TimeID , A.RoWid , Isnull(A.Division,0) DivisionCode , Cast(A.EntryDate As Date) EntryDate From VAAHINI_ERP_GAINUP.dbo.BookingMas A Left join VAAHINI_ERP_GAINUP.dbo.EmployeeMAs B On A.Emplno=B.Emplno Left join VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1 C On B.COMPCODE=C.CompCode Left Join VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1 C1 On A.Division=C1.CompCode Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D on A.From_Add=D.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D1 on A.To_Add=D1.Rowid Left join VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs T1 On A.Type_Code=T1.Rowid Left Join VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs T3 On A.Buyerid=T3.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteTimeMaster T2 On A.BookTime=T2.Rowid Left join VAAHINI_ERP_GAINUP.dbo.Vehicle_Travell_Expense_Master T6 On A.RoWid=T6.BookMasid Where T6.BookMasid Is Null And A.Cancel_Booking='F' And A.Approve='F' And MD_Approve='F'";
                Str = Str + " And A.Emplno=" + MyParent.Emplno + "";
                //Str = "  Select A.EntryNo , A.BookNo , A.BookDate , CONVERT(varchar(15),CAST(T2.Time+' '+ T2.Mode AS TIME),100) BookTime , Case When D.Name='-' Then 'MILL' Else D.Name End FROMADD , Case When D1.Name='-' Then 'MILL' Else D1.Name End TOADD , B.NAME , B.tno TNO , C.CompName , A.No_Of_Persons , A.Remarks1 , ISNull(T1.Name,'-') BType ,ISNull(T3.Name,'-') Buyer , C1.CompName Division , IsNull(A.Type_Code,0) Type_Code, A.From_Add , A.To_Add , A.Emplno , Isnull(A.Buyerid,0)Buyerid , A.BookTime TimeID , A.RoWid , Isnull(A.Division,0) DivisionCode From VAAHINI_ERP_GAINUP.dbo.BookingMas A Left join VAAHINI_ERP_GAINUP.dbo.EmployeeMAs B On A.Emplno=B.Emplno Left join VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1 C On B.COMPCODE=C.CompCode Left Join VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1 C1 On A.Division=C1.CompCode Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D on A.From_Add=D.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D1 on A.To_Add=D1.Rowid Left join VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs T1 On A.Type_Code=T1.Rowid Left Join VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs T3 On A.Buyerid=T3.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteTimeMaster T2 On A.BookTime=T2.Rowid Where A.Cancel_Booking='F' And A.Approve='F' And MD_Approve='F' And A.Emplno=" + MyParent.Emplno + "";
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Type", Str, String.Empty, 90, 90,100, 100, 130, 130 , 100);
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
                    if (!TxtType.Text.ToString().Trim().ToUpper().Contains("BUYER"))
                    {
                        Queries[Array_Index++] = "Delete from VAAHINI_ERP_GAINUP.dbo.BookingMasDetails where Masterid=" + Code + "";
                    }
                    else 
                    {
                        Queries[Array_Index++] = "Delete from VAAHINI_ERP_GAINUP.dbo.BuyerVisitorsName where BookMasid=" + Code + "";
                    }
                    Queries[Array_Index++] = "Delete from VAAHINI_ERP_GAINUP.dbo.BookingMas where Rowid=" + Code + "";
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
                Btn_Cancel2.Visible = false;
                Str = " Select A.EntryNo , A.BookNo , A.BookDate , CONVERT(varchar(15),CAST(T2.Time+' '+ T2.Mode AS TIME),100) BookTime , Case When D.Name='-' Then 'MILL' Else D.Name End FROMADD , Case When D1.Name='-' Then 'MILL' Else D1.Name End TOADD , B.NAME , B.tno TNO , C.CompName , A.No_Of_Persons , A.Remarks1,ISNull(T1.Name,'-') BType , ISNull(T3.Name,'-') Buyer , C1.CompName Division , IsNull(A.Type_Code,0) Type_Code , A.From_Add , A.To_Add , A.Emplno , Isnull(A.Buyerid,0)Buyerid , A.BookTime TimeID , A.RoWid , Isnull(A.Division,0) DivisionCode , Cast(A.EntryDate As Date) EntryDate  From VAAHINI_ERP_GAINUP.dbo.BookingMas A Left join VAAHINI_ERP_GAINUP.dbo.EmployeeMAs B On A.Emplno=B.Emplno Left join VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1 C On B.COMPCODE=C.CompCode Left Join VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1 C1 On A.Division=C1.CompCode Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D on A.From_Add=D.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteMaster D1 on A.To_Add=D1.Rowid Left join VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs T1 On A.Type_Code=T1.Rowid Left Join VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs T3 On A.Buyerid=T3.Rowid Left join VAAHINI_ERP_GAINUP.dbo.VehicleRouteTimeMaster T2 On A.BookTime=T2.Rowid Where A.Cancel_Booking='F' And A.Emplno=" + MyParent.Emplno + "";
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Type", Str, String.Empty, 90, 90, 100, 100, 130, 130 , 100);
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
            throw new NotImplementedException();
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
                TxtName.Tag = Dr["Emplno"].ToString();
                TxtCompany.Text = Dr["CompName"].ToString();
                DtpDate1.Value = Convert.ToDateTime(Dr["BookDate"]);
                DtpDate.Value = Convert.ToDateTime(Dr["EntryDate"]);
                TxtTripTime.Text = Dr["BookTime"].ToString();
                TxtTripTime.Tag = Dr["TimeID"].ToString();
                TxtFrom.Text = Dr["FROMADD"].ToString();
                TxtFrom.Tag = Dr["From_Add"].ToString();
                TxtTo.Text = Dr["TOADD"].ToString();
                TxtTo.Tag = Dr["To_Add"].ToString();
                TxtNoofPersons.Text = Dr["No_Of_Persons"].ToString();
                TxtCRemarks.Text = Dr["Remarks1"].ToString();
                TxtType.Text = Dr["BType"].ToString();
                TxtType.Tag = Dr["Type_Code"].ToString();
                TxtDivision.Text = Dr["Division"].ToString();
                TxtDivision.Tag = Dr["DivisionCode"].ToString();
                TxtBNo.Text = Dr["BookNo"].ToString();
                Total_Count();
                if (TxtType.Text.ToString().Trim().Contains("BUYER"))
                {
                    TxtNoofPersons.Enabled = false;
                    Grid_Data2();
                    Grid2.Visible = true;
                    Grid1.Visible = false;
                    TxtBuyerName.Enabled = true;
                    TxtBuyerName.Text = Dr["Buyer"].ToString();
                    TxtBuyerName.Tag = Dr["Buyerid"].ToString();
                }
                else
                {
                    TxtNoofPersons.Enabled = false;
                    Grid_Data1();
                    Grid1.Visible = true;
                    Grid2.Visible = false;
                    TxtBuyerName.Enabled = false;
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
                    String Str5 = "Update VAAHINI_ERP_GAINUP.dbo.BookingMas Set Cancel_Booking='T' Where Emplno=" + MyParent.Emplno + " And Rowid=" + Code + "";
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

        private void FrmVehicleCarBooking_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtTno" || this.ActiveControl.Name == "TxtName" || this.ActiveControl.Name == "TxtCompany" || this.ActiveControl.Name == "TxtTotCount" || this.ActiveControl.Name == "TxtTotQty" || this.ActiveControl.Name == "txtTotweight" || this.ActiveControl.Name == "TxtTotBox" || this.ActiveControl.Name == "TxtTotCbm" || this.ActiveControl.Name == "TxtEno" || this.ActiveControl.Name == "TxtVehno" || this.ActiveControl.Name == "TxtType" || this.ActiveControl.Name == "TxtTripTime" || this.ActiveControl.Name == "TxtFrom" || this.ActiveControl.Name == "TxtTo" || this.ActiveControl.Name == "TxtVehtype" || this.ActiveControl.Name == "TxtTotalCount" || this.ActiveControl.Name == "TxtBuyerName" || this.ActiveControl.Name == "TxtDivision" || this.ActiveControl.Name == "TxtBNo")
                {
                    e.Handled = true;
                }
                else if (this.ActiveControl.Name == "TxtNoofPersons")
                {
                    MyBase.Valid_Number(TxtNoofPersons, e);
                }
                if (this.ActiveControl.Name == "TxtVehnoC")
                {
                    e.Handled = !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && Convert.ToInt32(e.KeyChar) != 8;
                    MyBase.Return_Ucase(e);
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
                if (Dt1.Rows.Count > 0)
                {
                    TxtNoofPersons.Text = Dt1.Rows.Count.ToString();
                }
                if (Dt8.Rows.Count > 0)
                {
                    TxtNoofPersons.Text = Dt8.Rows.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Grid1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt1 == null)
                {
                    Txt1 = (TextBox)e.Control;
                    Txt1.KeyDown += new KeyEventHandler(Txt1_KeyDown);
                    Txt1.KeyPress += new KeyPressEventHandler(Txt1_KeyPress);
                    Txt1.Leave += new EventHandler(Txt1_Leave);
                    Txt1.TextChanged += new EventHandler(Txt1_TextChanged);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid1.CurrentCell.ColumnIndex == Grid1.Columns["TNO"].Index)
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            Str = "Select TNO,NAME,DesignationName,Emplno,DesignationCode From VAAHINI_ERP_GAINUP.dbo.Empls_For_VehBooking()";
                            Dr = Tool.Selection_Tool_Except_New("TNO", this, 30, 70, ref Dt1, SelectionTool_Class.ViewType.NormalView, "Select Name", Str, String.Empty, 100, 200, 150);
                            if (Dr != null)
                            {
                                Txt1.Text = Dr["TNO"].ToString();
                                Grid1["TNO", Grid1.CurrentCell.RowIndex].Value = Dr["TNO"].ToString();
                                Grid1["NAME", Grid1.CurrentCell.RowIndex].Value = Dr["NAME"].ToString();
                                Grid1["ENO", Grid1.CurrentCell.RowIndex].Value = Dr["Emplno"].ToString();
                                Grid1["Designation", Grid1.CurrentCell.RowIndex].Value = Dr["DesignationName"].ToString();

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

        void Txt1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid1.CurrentCell.ColumnIndex == Grid1.Columns["NAME"].Index || Grid1.CurrentCell.ColumnIndex == Grid1.Columns["TNO"].Index || Grid1.CurrentCell.ColumnIndex == Grid1.Columns["DESIGNATION"].Index)
                {
                    e.Handled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void Txt1_TextChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void Txt1_Leave(object sender, EventArgs e)
        {
            try
            {
                Total_Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Grid1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Grid1.CurrentCell.RowIndex <= Dt1.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Dt1.Rows.RemoveAt(Grid1.CurrentCell.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmVehicleCarBooking_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {


                    if (this.ActiveControl.Name == "TxtCRemarks" || this.ActiveControl.Name == "TxtRemarks")
                    {

                    }
                    if (this.ActiveControl.Name == "TxtTo")
                    {
                        TxtType.Focus();
                        return;

                    }
                    if (this.ActiveControl.Name == "TxtType")
                    {
                        if (TxtType.Text.ToString().Trim().Contains("BUYER"))
                        {
                            if (TxtDivision.Enabled == true)
                            {
                                TxtDivision.Focus();
                                return;
                            }

                            if (this.ActiveControl.Name == "TxtDivision")
                            {
                                if (TxtBuyerName.Enabled == true)
                                {
                                    TxtBuyerName.Focus();
                                    return;
                                }
                                
                            }

                        }
                        else
                        {
                            Grid1.CurrentCell = Grid1["TNO", 0];
                            Grid1.Focus();
                            Grid1.BeginEdit(true);
                            return;
                        }
                    }
                    
                    if (this.ActiveControl.Name == "TxtBuyerName")
                    {
                        Grid2.CurrentCell = Grid2["NAME", 0];
                        Grid2.Focus();
                        Grid2.BeginEdit(true);
                        return;
                    }
                    else
                    {
                        if (this.ActiveControl.Name == "TxtCRemarks")
                        {

                        }
                        else
                        {
                            SendKeys.Send("{TAB}");
                        }
                    }


                }
                else if (e.KeyCode == Keys.Escape)
                {
                    if (this.ActiveControl.Name == "TxtCRemarks" || this.ActiveControl.Name == "TxtRemarks")
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
                            TxtNoofPersons.Text = String.Empty;
                            TxtBuyerName.Text = String.Empty;
                            Str = "Select Name Type,Rowid From VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs Where Type='C'";
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Type", Str, String.Empty, 200);
                            if (Dr != null)
                            {
                                TxtType.Text = Dr["Type"].ToString();
                                TxtType.Tag = Dr["Rowid"].ToString();

                                if (TxtType.Text.ToString().Trim().Contains("BUYER"))
                                {
                                    TxtNoofPersons.Enabled = false;
                                    Grid1.DataSource = null;
                                    Grid1.Columns.Clear();
                                    Dt1.Rows.Clear();
                                    Grid2.Visible = true;
                                    Grid1.Visible = false;
                                    TxtBuyerName.Enabled = true;
                                    // TxtDivision.Text = String.Empty;
                                    // TxtDivision.Tag = String.Empty;
                                    MyParent._New = true;
                                    Grid_Data2();

                                }
                                else
                                {
                                    if (TxtType.Text.ToString().Trim() != String.Empty)
                                    {
                                        TxtNoofPersons.Enabled = false;
                                        Grid1.Visible = true;
                                        Grid2.Visible = false;
                                        Grid2.DataSource = null;
                                        Grid2.Columns.Clear();
                                        Dt8.Rows.Clear();
                                        TxtBuyerName.Enabled = false;

                                        TxtDivision.Text = TxtCompany.Text;
                                        TxtDivision.Tag = TxtCompany.Tag;
                                        MyParent._New = true;
                                        Grid_Data1();
                                    }
                                }
                            }
                        }
                        
                        

                    }
                    else if (this.ActiveControl.Name == "TxtBuyerName")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            TxtNoofPersons.Text = String.Empty;
                            Str = "Select Name Type,Rowid From VAAHINI_ERP_GAINUP.dbo.Book_Type_MAs Where Type='B'";
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Type", Str, String.Empty, 200);
                            if (Dr != null)
                            {
                                TxtBuyerName.Text = Dr["Type"].ToString();
                                TxtBuyerName.Tag = Dr["Rowid"].ToString();
                                Grid_Data2();

                            }
                        }

                    }
                    //else if (this.ActiveControl.Name == "TxtCompany")
                    //{
                    //    Str = "Select CompName,CompCode From VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1";
                    //    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Division", Str, String.Empty, 200);
                    //    if (Dr != null)
                    //    {
                    //        TxtCompany.Text = Dr["CompName"].ToString();
                    //        TxtCompany.Tag = Dr["CompCode"].ToString();
                    //    }
                    //}
                    else if (this.ActiveControl.Name == "TxtDivision")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            Str = "Select CompName,CompCode From VAAHINI_ERP_GAINUP.dbo.CompanyMas_Pay1";
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Division", Str, String.Empty, 200);
                            if (Dr != null)
                            {
                                TxtDivision.Text = Dr["CompName"].ToString();
                                TxtDivision.Tag = Dr["CompCode"].ToString();
                            }
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtTripTime")
                    {
                        Min_MaxDate();
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
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

        private void FrmVehicleCarBooking_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                
                MyBase.Clear(this);
                Min_MaxDate();
                Grid1.Visible = true;
                Grid2.Visible = false;

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
  
        private void Grid1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (Grid1.Rows.Count > 1)
                {
                    MyBase.Row_Number(ref Grid1);
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
                    Txt2.Leave +=new EventHandler(Txt2_Leave);
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
                if (Grid2.CurrentCell.ColumnIndex == Grid2.Columns["NAME"].Index)
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
                if (Grid2.CurrentCell.ColumnIndex == Grid2.Columns["NAME"].Index)
                {
                    Total_Count();
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
    }
}
